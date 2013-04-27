﻿Option Strict Off
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
            With New Windows.Forms.OpenFileDialog
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

        Dim lXIndexToget As Integer = -1
        Dim lYIndexToget As Integer = -1
        Dim lGetOnlyOneTimseries As Boolean = False
        If aAttributes IsNot Nothing Then
            lXIndexToget = aAttributes.GetValue("XIndex", lXIndexToget)
            lYIndexToget = aAttributes.GetValue("YIndex", lYIndexToget)
            If lXIndexToget >= 0 AndAlso lYIndexToget >= 0 Then lGetOnlyOneTimseries = True
        End If

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

                    'TODO: check to see if this file is open for another timeseries group, need the collection of open timeseries files
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

                        If lTimeseriesCount > 1000 AndAlso Not lGetOnlyOneTimseries Then
                            'too many timeseries, ask user which one
                            Dim lFrmIndex As New frmIndex
                            With lFrmIndex
                                .txtXIndex.HardMax = lNetCDFFile.EastWestDimension.Length
                                .txtYIndex.HardMax = lNetCDFFile.NorthSouthDimension.Length
                                If .ShowDialog() <> Windows.Forms.DialogResult.Cancel Then
                                    lXIndexToget = .txtXIndex.Text
                                    lYIndexToget = .txtYIndex.Text
                                    lGetOnlyOneTimseries = True
                                    Logger.Dbg("User Specified X " & lXIndexToget & " Y " & lYIndexToget)
                                Else
                                    Logger.Dbg("Too Many Timeseries, user did not specify")
                                    Return False
                                End If
                            End With
                        End If

                        'get the timeseries
                        Dim lTimeVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.TimeDimension.ID)
                        Dim lUnits As String = lTimeVariable.Attributes.GetValue("Units").ToString
                        Dim lTimeUnit As atcTimeUnit = atcTimeUnit.TUUnknown
                        Dim lTimeStepMultiplier As Double = 1.0
                        If lUnits.StartsWith("hour") Then
                            lTimeUnit = atcTimeUnit.TUHour
                            lTimeStepMultiplier = lTimeStepMultiplier / 24.0
                        ElseIf lUnits.StartsWith("day") Then
                            lTimeUnit = atcTimeUnit.TUDay
                        Else
                            Throw New Exception("unknown time units '" & lUnits & "'")
                        End If
                        Dim lDateBase As Date = lUnits.Remove(0, lUnits.IndexOf("since") + 6).Substring(0, 10)

                        Dim lDates As New atcTimeseries(Me)

                        'TODO: check to see that dates and values are put in the correct spot in the array
                        Dim lNumValues As Integer = lTimeVariable.Dimensions(0).Length
                        ReDim lDates.Values(lNumValues)
                        'lDates.Values(0) = lDateBase.ToOADate
                        For lTimeIndex As Integer = 0 To lNumValues - 1
                            lDates.Values(lTimeIndex) = lDateBase.ToOADate + (lTimeStepMultiplier * lTimeVariable.Values(lTimeIndex))
                            'If lTimeIndex = 1 Then 'deal with base date a long time before first value
                            '    Dim lTimeInterval As Double = lTimeVariable.Values(lTimeIndex) - lTimeVariable.Values(lTimeIndex - 1)
                            '    lDates.Values(0) = lDates.Values(1) - lTimeInterval
                            'End If
                        Next
                        lDates.Values(lNumValues) = lDates.Values(lNumValues - 1) + (lDates.Values(lNumValues - 1) - lDates.Values(lNumValues - 2))

                        Dim lYIndex As Integer = 1
                        Dim lYValue As String = ""
                        Dim lXIndex As Integer = 1
                        Dim lXValue As String = ""
                        For lTimeseriesIndex As Integer = 0 To lTimeseriesCount - 1
                            Dim lLocation As String = ""
                            If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lYValue = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex - 1)
                                lLocation &= "Y:" & lYValue
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                lXValue = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex - 1)
                                lLocation &= "X:" & lXValue
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lXValue = lNetCDFFile.Variables(lNetCDFFile.EastWestDimension.ID).Values(lXIndex - 1)
                                lLocation &= "X:" & lXValue
                                lYValue = lNetCDFFile.Variables(lNetCDFFile.NorthSouthDimension.ID).Values(lYIndex - 1)
                                lLocation &= " Y:" & lYValue
                            End If

                            If Not lGetOnlyOneTimseries OrElse (lYIndex = lYIndexToget AndAlso lXIndex = lXIndexToget) Then
                                Dim lTimeseries As New atcTimeseries(Me)
                                Dim lDataVariable As atcNetCDFVariable = lNetCDFFile.Variables(lNetCDFFile.Variables.Count - 1)

                                For Each lDataAttribute As atcDefinedValue In lDataVariable.Attributes
                                    lTimeseries.Attributes.SetValue(lDataAttribute.Definition.Name, lDataAttribute.Value)
                                Next
                                lTimeseries.Attributes.SetValue("NetCDFValues", lDataVariable)

                                lTimeseries.SetInterval(lTimeUnit, 1)
                                lTimeseries.Attributes.SetValue("Constituent", IO.Path.GetFileNameWithoutExtension(lFileName))
                                lTimeseries.Attributes.SetValue("ID", lTimeseriesIndex + 1)
                                If lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                    lTimeseries.Attributes.SetValue("Y", lYValue)
                                    lTimeseries.Attributes.SetValue("Y index", lYIndex)
                                End If
                                If lNetCDFFile.EastWestDimension IsNot Nothing Then
                                    lTimeseries.Attributes.SetValue("X", lXValue)
                                    lTimeseries.Attributes.SetValue("X index", lXIndex)
                                End If
                                lTimeseries.Attributes.SetValue("Location", lLocation)

                                'If lTimeVariable.ID > 0 Then
                                '    lTimeseries.Attributes.SetValue("Base", lTimeseriesIndex)
                                '    lTimeseries.Attributes.SetValue("Increment", lTimeseriesCount)
                                'Else
                                '    lTimeseries.Attributes.SetValue("Base", lTimeseriesIndex * lNumValues)
                                '    lTimeseries.Attributes.SetValue("Increment", 1)
                                'End If

                                lTimeseries.ValuesNeedToBeRead = True
                                lTimeseries.Dates = lDates

                                Me.DataSets.Add(lTimeseries)
                            End If

                            If lNetCDFFile.EastWestDimension Is Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                lYIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension Is Nothing Then
                                lXIndex += 1
                            ElseIf lNetCDFFile.EastWestDimension IsNot Nothing AndAlso lNetCDFFile.NorthSouthDimension IsNot Nothing Then
                                If lNetCDFFile.EastWestDimension.ID = 0 OrElse (lNetCDFFile.TimeDimension.ID = 0 AndAlso lNetCDFFile.EastWestDimension.ID = 1) Then
                                    lXIndex += 1
                                    If lXIndex > lNetCDFFile.EastWestDimension.Length Then
                                        lXIndex = 1
                                        lYIndex += 1
                                    End If
                                Else
                                    lYIndex += 1
                                    If lYIndex > lNetCDFFile.NorthSouthDimension.Length Then
                                        lYIndex = 1
                                        lXIndex += 1
                                    End If
                                End If
                            End If
                        Next
                        Logger.Dbg("TimeseriesBuilt:Count " & Me.DataSets.Count & " Length " & lNumValues)
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
        Dim lDataVariable As atcNetCDFVariable = lTimeseries.Attributes.GetValue("NetCDFValues")
        Dim lXIndex As Integer = lTimeseries.Attributes.GetValue("X index")
        Dim lYIndex As Integer = lTimeseries.Attributes.GetValue("Y index")
        Dim lValues = lDataVariable.ReadArray(lNumValues, lXIndex - 1, lYIndex - 1)
        ReDim lTimeseries.Values(lNumValues)
        For lTimeIndex As Integer = 0 To lNumValues - 1
            lTimeseries.Values(lTimeIndex + 1) = lValues(lTimeIndex)
        Next
    End Sub
End Class