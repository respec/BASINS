Imports atcUtility
Imports atcData
Imports MapWinUtility

''' <summary>
''' Binary storage format for atcTimeseries
''' </summary>
''' <remarks>Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
Public Class atcDataSourceTimeseriesBinary
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "Binary Files (*.tsbin)|*.tsbin"
    Private Shared pVersion As Integer = &H54534231 'TSB1 

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Timeseries Binary"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Binary"
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
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If Not MyBase.Open(aFileName, aAttributes) Then
            Return False
        Else
            If IO.File.Exists(Specification) Then
                Logger.Status("Reading " & IO.Path.GetFileName(Specification), True)
                Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Open, IO.FileAccess.Read)
                Dim lReader As New IO.BinaryReader(lFileStream)
                Dim lVersion As Integer = lReader.ReadInt32
                If lVersion <> pVersion Then
                    Logger.Dbg("BadMagicNumber for " & Specification & " (" & Hex(lVersion) & "<>" & Hex(pVersion) & ")")
                    lReader.Close()
                    Return False
                Else
                    Try
                        Do
                            Dim lData As New atcTimeseries(Me)
                            Dim lAttributeName As String
                            Dim lAttributeType As Byte
                            Do
                                lAttributeName = lReader.ReadString
                                If lAttributeName = "<done>" Then
                                    Exit Do
                                End If
                                lAttributeType = lReader.ReadByte
                                Select Case lAttributeType
                                    Case 1 : lData.Attributes.SetValue(lAttributeName, lReader.ReadString)
                                    Case 2 : lData.Attributes.SetValue(lAttributeName, lReader.ReadInt32)
                                    Case 3 : lData.Attributes.SetValue(lAttributeName, lReader.ReadDouble)
                                    Case 4 : lData.Attributes.SetValue(lAttributeName, lReader.ReadSingle)
                                    Case Else
                                        Debug.Print(lAttributeType)
                                End Select
                            Loop
                            Dim lNumDates As Integer = lReader.ReadInt64
                            Dim lDates(Math.Abs(lNumDates)) As Double
                            If lNumDates < 0 Then 'compressed dates
                                lNumDates = -lNumDates
                                Dim lTimeUnits As Object = lData.Attributes.GetValue("tu", 4)
                                Dim lTU As atcUtility.atcTimeUnit
                                If lTimeUnits.GetType.IsInstanceOfType("") Then
                                    lTU = System.Enum.Parse(lTU.GetType, lTimeUnits) '6 'TODO: dont hard code this!!!
                                    lData.Attributes.SetValue("tu", lTU)
                                Else
                                    lTU = lTimeUnits
                                End If
                                Dim lTimeStep = lData.Attributes.GetValue("ts", 1)
                                Dim lDateStart As Double = lReader.ReadDouble
                                lDates(0) = lDateStart
                                For lIndex As Integer = 1 To lNumDates
                                    lDates(lIndex) = TimAddJ(lDateStart, lTU, lTimeStep, lIndex)
                                Next
                            Else
                                For lIndex As Integer = 0 To lNumDates
                                    lDates(lIndex) = lReader.ReadDouble
                                Next
                            End If
                            lData.Dates = New atcTimeseries(Me)
                            lData.Dates.Values = lDates

                            Dim lValues(lNumDates) As Double
                            For lIndex As Integer = 0 To lNumDates
                                lValues(lIndex) = lReader.ReadDouble
                            Next
                            lData.Values = lValues
                            DataSets.Add(lData)
                            Logger.Progress(lFileStream.Position, lFileStream.Length)
                        Loop
                    Catch ex As IO.EndOfStreamException
                        Logger.Status("")
                        Logger.Dbg("Read " & DataSets.Count & " from " & Specification)
                    End Try
                    lReader.Close()
                End If
            End If
        End If
        Return True
    End Function

    Public Overrides Function AddDatasets(ByVal aDataGroup As atcTimeseriesGroup) As Boolean
        Logger.Status("Writing " & IO.Path.GetFileName(Specification), True)
        Dim lIndex As Integer = 0
        For Each lDataSet As atcData.atcDataSet In aDataGroup
            AddDataset(lDataSet)
            lIndex += 1
            Logger.Progress(lIndex, aDataGroup.Count)
        Next
        Logger.Status("")
        Logger.Dbg("Wrote " & aDataGroup.Count & " Datasets")
    End Function

    Public Overrides Function AddDataset(ByVal aDataSet As atcData.atcDataSet, _
                                Optional ByVal aExistAction As atcData.atcTimeseriesSource.EnumExistAction = atcData.atcTimeseriesSource.EnumExistAction.ExistReplace) _
                                         As Boolean
        Dim lFileStream As New IO.FileStream(Specification, IO.FileMode.Append)
        Dim lWriter As New IO.BinaryWriter(lFileStream)
        If lFileStream.Position = 0 Then
            lWriter.Write(pVersion)
        End If

        For Each lAttribute As atcDefinedValue In aDataSet.Attributes
            If Not lAttribute.Definition.Calculated Then
                Dim lName As String = lAttribute.Definition.Name
                Select Case lName
                    Case "Key", "Data Source"
                    Case Else
                        Dim lType As Byte = 0
                        Select Case lAttribute.Definition.TypeString
                            Case "String", "atcTimeUnit"
                                lWriter.Write(lName)
                                lWriter.Write(CByte(1))
                                lWriter.Write(lAttribute.Value.ToString.TrimEnd)
                            Case "Integer"
                                lWriter.Write(lName)
                                lWriter.Write(CByte(2))
                                lWriter.Write(CInt(lAttribute.Value))
                            Case "Double"
                                lWriter.Write(lName)
                                lWriter.Write(CByte(3))
                                lWriter.Write(CDbl(lAttribute.Value))
                            Case "Single"
                                lWriter.Write(lName)
                                lWriter.Write(CByte(4))
                                lWriter.Write(CSng(lAttribute.Value))
                            Case Else
                                Debug.Print("AttributeTypeNotDefined:" & lAttribute.Definition.TypeString)
                        End Select
                End Select
            End If
        Next
        lWriter.Write("<done>")

        Dim lTimeseries As atcTimeseries = aDataSet
        Dim lTimeUnits As Integer = lTimeseries.Attributes.GetValue("tu", 4)
        Dim lTimeStep As Integer = lTimeseries.Attributes.GetValue("ts", 1)
        Dim lDateEndComputed As Double = TimAddJ(lTimeseries.Dates.Value(0), lTimeUnits, lTimeStep, lTimeseries.numValues)
        If Math.Abs(lDateEndComputed - lTimeseries.Dates.Value(lTimeseries.numValues)) < 0.00001 Then
            lWriter.Write(-lTimeseries.numValues)
            lWriter.Write(lTimeseries.Dates.Values(0))
        Else
            lWriter.Write(lTimeseries.numValues)
            Dim lDates() As Double = lTimeseries.Dates.Values
            For lIndex As Integer = 0 To lTimeseries.numValues
                lWriter.Write(lDates(lIndex))
            Next
        End If
        Dim lValues() As Double = lTimeseries.Values
        For lIndex As Integer = 0 To lTimeseries.numValues
            lWriter.Write(lValues(lIndex))
        Next
        'todo: write value attributes (if any)

        lWriter.Close()
    End Function

    Public Overrides Function Save(ByVal SaveFileName As String, _
                          Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        If SaveFileName.ToLower.Equals(Me.Specification.ToLower) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class
