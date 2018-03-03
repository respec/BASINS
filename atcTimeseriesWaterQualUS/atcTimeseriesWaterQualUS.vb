Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.FileIO.TextFieldParser

Public Class atcTimeseriesWaterQualUS
    Inherits atcTimeseriesSource
    Private Shared pFilter As String = "WaterQualityData US Files (*.csv)|*.csv;|All Files (*.*)|*.*"
    Private pJulianInterval As Double = 1 'Add one day for daily values to record date at end of interval
    Public RawDataGroup As New clsWQDUSLocations() 'Dictionary(Of String, clsWQDUSLocation)()
    Public SelectAllLocations As Boolean = False
    Public SelectAllConstituents As Boolean = False
    Public SelectedLocations As New List(Of String)()
    Public SelectedConstituents As New List(Of String)()
    Public AllConstituents As New List(Of String)()

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "WaterQualityData US"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::WaterQualityData US"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String,
                      Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            If Not IO.File.Exists(Specification) Then
                Logger.Dbg("Opening new file " & Specification)
                Return True
            ElseIf IO.Path.GetFileName(Specification).ToLower.StartsWith("stations") Then
                Throw New ApplicationException("Station file does not contain timeseries data: " & IO.Path.GetFileName(Specification))
            Else
                Try
                    Dim lTimeStartOpen As Date = Now
                    Logger.Dbg("OpenStartFor " & Specification)
                    Dim CurrentRecord() As String
                    Dim lDataColumnsValid As Boolean = True
                    Dim lFileInfo As New FileInfo(Specification)
                    Dim lSr As New StreamReader(Specification, True)
                    Dim lEncoding As Text.Encoding = lSr.CurrentEncoding()
                    lSr.Close()
                    lSr = Nothing
                    Dim lFileSize As Long = lFileInfo.Length
                    Dim lRunningSize As Long = 0
                    Using afile As New Microsoft.VisualBasic.FileIO.TextFieldParser(Specification)
                        afile.TextFieldType = FileIO.FieldType.Delimited
                        afile.Delimiters = New String() {","}
                        afile.HasFieldsEnclosedInQuotes = True

                        Dim first As Boolean = True
                        ' parse the actual file
                        Dim loc_id As String
                        Dim loc As clsWQDUSLocation
                        While Not afile.EndOfData
                            Try
                                CurrentRecord = afile.ReadFields
                                lRunningSize += lEncoding.GetByteCount(String.Join(",", CurrentRecord))
                                Logger.Progress(lRunningSize / lFileSize * 100.0, 100)
                                If first Then
                                    first = False
                                    If Not clsWQDUSColumns.SetColumns(CurrentRecord) Then
                                        lDataColumnsValid = False
                                        Exit While
                                    End If
                                    Continue While
                                Else
                                    loc_id = CurrentRecord(clsWQDUSColumns.colMonitoringLocationIdentifier)
                                    If RawDataGroup.ContainsKey(loc_id) Then
                                        loc = RawDataGroup.Item(loc_id)
                                    Else
                                        loc = New clsWQDUSLocation(loc_id)
                                        RawDataGroup.Add(loc_id, loc)
                                    End If
                                    loc.AddData(CurrentRecord)
                                End If
                            Catch ex As FileIO.MalformedLineException
                                MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                            End Try
                        End While
                    End Using
                    Logger.Progress(100, 100)
                    If lDataColumnsValid Then
                        SelectedConstituents.Clear()
                        SelectedLocations.Clear()
                        Dim lfrm As New frmSelect(RawDataGroup, SelectedLocations, SelectedConstituents)
                        If lfrm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            If SelectedLocations.Count > 0 AndAlso SelectedConstituents.Count > 0 Then
                                Return BuildTimeseries()
                            End If
                        End If
                    Else
                        Return False
                    End If
                    Return True
                Catch lException As Exception
                    Logger.Progress(100, 100)
                    Throw New ApplicationException("Exception reading '" & Specification & "': " & lException.Message, lException)
                End Try
            End If
        End If
        Return True
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Function BuildTimeseries() As Boolean
        If RawDataGroup.Count = 0 Then
            Return False
        End If
        Dim lBuildOk As Boolean = True
        Try
            Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Me)
            Dim lTSBuilder As atcTimeseriesBuilder
            For Each loc As clsWQDUSLocation In RawDataGroup.Values
                If Not SelectAllLocations AndAlso Not SelectedLocations.Contains(loc.Location) Then
                    Continue For
                End If
                For Each cons As clsWQDUSConstituent In loc.Constituents.Values
                    For Each lcon_unit As String In cons.RecordGroup.Keys
                        If Not SelectAllConstituents Then
                            If Not SelectedConstituents.Contains(cons.Name & "-" & lcon_unit) Then
                                Continue For
                            End If
                        End If
                        Dim ldata As List(Of clsWQDUSDataPoint) = cons.RecordGroup.Item(lcon_unit)
                        If ldata.Count > 0 Then
                            lTSBuilder = lGroupBuilder.Builder(loc.Location & ":" & cons.Name & ":" & lcon_unit)
                            With lTSBuilder.Attributes
                                'Set attributes of newly created builder
                                If Not .ContainsAttribute("Location") Then
                                    .SetValue("Scenario", "Observed")
                                    .SetValue("Location", loc.Location)
                                    .SetValue("Constituent", cons.Name)
                                    .SetValue("Units", lcon_unit)
                                    '.SetValue("CCode", lCcode)
                                    .SetValue("Point", True)
                                    .AddHistory("Read from " & Specification)
                                End If
                            End With
                            For I As Integer = 0 To ldata.Count - 1
                                'ldt = Date.Parse(lTable.Value(lDateCol)).ToOADate
                                If ldata(I).SetDataTime() Then
                                    lTSBuilder.AddValue(ldata(I).atcDataTime, ldata(I).Value)
                                    If Not String.IsNullOrEmpty(ldata(I).Attrib) Then
                                        lTSBuilder.AddValueAttribute("RCode", ldata(I).Attrib)
                                    End If
                                Else
                                    Continue For
                                End If
                            Next 'data point
                        End If
                    Next 'lcon_unit
                Next 'cons
            Next 'loc
            lGroupBuilder.CreateTimeseriesAddToGroup(DataSets)
            lBuildOk = True
        Catch ex As Exception
            lBuildOk = False
            Logger.Msg("Build datasets failed in " & Specification, "WaterQualityData US Build Dataset")
        End Try
        Return lBuildOk
    End Function
End Class
