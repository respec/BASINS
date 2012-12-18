Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text
Imports atcTimeseriesNetCDF.NetCDF
Imports atcTimeseriesNetCDF.atcNetCDF

''' <summary>
''' Reads NetCDF binary files containing time-series values
''' </summary>
''' <remarks>
''' </remarks>
Public Class atcTimeseriesNetCDF
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "NetCDF Files (*.nc)|*.nc|All Files (*.*)|*.*"
    Private pErrorDescription As String
    Private pJulianOffset As Double = New Date(1900, 1, 1).Subtract(New Date(1, 1, 1)).TotalDays
    Private pDebug As Boolean

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "NetCDF File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::NetCDF"
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

    Public Property Debug() As Boolean
        Get
            Return pDebug
        End Get
        Set(ByVal value As Boolean)
            pDebug = value
        End Set
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            'aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)

            Dim cdlg As New Windows.Forms.OpenFileDialog
            With cdlg
                .Title = "Select " & Name & " file(s) to open"
                .Filter = pFilter
                .FilterIndex = 1
                .DefaultExt = ".nc"
                .Multiselect = True
                If .ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    ' user clicked Cancel
                    Logger.Dbg("User Cancelled File Selection Dialog")
                    Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
                    Return False
                End If
                aFileName = String.Join(";", .FileNames)
            End With
        End If
        Me.Specification = IO.Path.GetFullPath(aFileName)
        For Each lFileName As String In aFileName.Split(";")
            If Not FileExists(lFileName) Then
                pErrorDescription = "File '" & lFileName & "' not found"
                Me.Specification = ""
            Else
                Dim lLogfileName As String = Logger.FileName
                Try
                    If pDebug Then
                        Logger.StartToFile(IO.Path.ChangeExtension(lFileName, ".log"), , False, True)
                    End If

                    Dim lNetCDFFile As New atcNetCDFFile(lFileName)
                    If lNetCDFFile.TimeDimension Is Nothing Then
                        pErrorDescription = "No timeseries found in " & lFileName
                        Logger.Dbg(pErrorDescription)
                        Return False
                    Else
                        'how many timeseries?
                        Dim lTimeseriesCount As Integer = 1
                        If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                            lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                        ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                            lTimeseriesCount = lNetCDFFile.NorthSouthDimension.Length
                        ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                            lTimeseriesCount = lNetCDFFile.EastWestDimension.Length * lNetCDFFile.NorthSouthDimension.Length
                        End If
                        Logger.Dbg("TimeseriesCount:" & lTimeseriesCount)

                        'get the timeseries
                        Dim lTimeVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.TimeDimension.ID)
                        Dim lTimeUnits As String = lTimeVariable.Attributes.GetValue("Units").ToString.Replace("days since ", "")
                        Dim lDateBase As Date = lTimeUnits.Substring(0, 10)
                        Dim lDates As New atcTimeseries(Me)

                        'TODO: check to see that dates and values are put in the correct spot in the array
                        Dim lNumValues As Integer = lTimeVariable.Dimensions(0).Length
                        ReDim lDates.Values(lNumValues)
                        lDates.Values(0) = lDateBase.ToOADate
                        For lTimeIndex As Integer = 1 To lNumValues - 1
                            lDates.Values(lTimeIndex) = lDates.Values(0) + lTimeVariable.Values(lTimeIndex)
                        Next
                        'kludge in last value
                        lDates.Values(lNumValues) = lDates.Values(lNumValues - 1) + lTimeVariable.Values(1)

                        Dim lDataVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.Variables.Count - 1)
                        Dim lYIndex As Integer = 0
                        Dim lXIndex As Integer = 0
                        For lTimeseriesIndex As Integer = 0 To lTimeseriesCount - 1
                            Dim lTimeseries As New atcTimeseries(Me)
                            lTimeseries.Attributes.SetValue("Constituent", IO.Path.GetFileNameWithoutExtension(lFileName))
                            lTimeseries.Attributes.SetValue("ID", lTimeseriesIndex + 1)
                            Dim lLocation As String = ""
                            If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                Dim lYValue As String = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex)
                                lTimeseries.Attributes.SetValue("Y", lYValue)
                                lTimeseries.Attributes.SetValue("Y index", lYIndex)
                                lLocation &= "Y:" & lYValue
                                lYIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                Dim lXValue As String = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex)
                                lTimeseries.Attributes.SetValue("X", lXValue)
                                lTimeseries.Attributes.SetValue("X index", lXIndex)
                                lLocation &= "X:" & lXValue
                                lXIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                Dim lXValue As String = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex)
                                lTimeseries.Attributes.SetValue("X", lXValue)
                                lLocation &= "X:" & lXValue
                                lTimeseries.Attributes.SetValue("X index", lXIndex)
                                Dim lYValue As String = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex)
                                lTimeseries.Attributes.SetValue("Y", lYValue)
                                lTimeseries.Attributes.SetValue("Y index", lYIndex)
                                lLocation &= " Y:" & lYValue
                                If lNetCDFFile.EastWestDimension.ID = 0 OrElse (lNetCDFFile.TimeDimension.ID = 0 AndAlso lNetCDFFile.EastWestDimension.ID = 1) Then
                                    lXIndex += 1
                                    If lXIndex = lNetCDFFile.EastWestDimension.Length Then
                                        lXIndex = 0
                                        lYIndex += 1
                                    End If
                                Else
                                    lYIndex += 1
                                    If lYIndex = lNetCDFFile.NorthSouthDimension.Length Then
                                        lYIndex = 0
                                        lXIndex += 1
                                    End If
                                End If
                            End If
                            lTimeseries.Attributes.SetValue("Location", lLocation)
                            For Each lDataAttribute As atcDefinedValue In lDataVariable.Attributes
                                lTimeseries.Attributes.SetValue(lDataAttribute.Definition.Name, lDataAttribute.Value)
                            Next

                            lTimeseries.Attributes.SetValue("NetCDFValues", lDataVariable)

                            If lTimeVariable.ID > 0 Then
                                lTimeseries.Attributes.SetValue("Base", lTimeseriesIndex)
                                lTimeseries.Attributes.SetValue("Increment", lTimeseriesCount)
                            Else
                                lTimeseries.Attributes.SetValue("Base", lTimeseriesIndex * lNumValues)
                                lTimeseries.Attributes.SetValue("Increment", 1)
                            End If

                            lTimeseries.ValuesNeedToBeRead = True

                            lTimeseries.Dates = lDates

                            Me.DataSets.Add(lTimeseries)
                        Next
                        Logger.Dbg("TimeseriesBuilt")
                    End If
                Catch e As Exception
                    Logger.Dbg("Exception reading '" & aFileName & "': " & e.Message, e.StackTrace)
                    Return False
                Finally
                    If pDebug Then
                        Logger.StartToFile(lLogfileName, True)
                    End If
                End Try
            End If
        Next
        Return (pData.Count > 0)
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Overrides Sub ReadData(ByVal aData As atcData.atcDataSet)
        Dim lTimeseries As atcTimeseries = aData
        Dim lNumValues As Integer = lTimeseries.Dates.numValues
        Dim lBase As Integer = lTimeseries.Attributes.GetValue("Base")
        Dim lIncrement As Integer = lTimeseries.Attributes.GetValue("Increment")
        Dim lDataVariable As atcNetCDFVariable = lTimeseries.Attributes.GetValue("NetCDFValues")
        ReDim lTimeseries.Values(lNumValues)
        For lTimeIndex As Integer = 0 To lNumValues - 1
            Dim lValueIndex As Integer = (lIncrement * lTimeIndex) + lBase
            lTimeseries.Values(lTimeIndex + 1) = lDataVariable.Values(lValueIndex)
        Next
    End Sub
End Class
