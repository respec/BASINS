﻿Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMEvaporation
    Implements IBlock

    Private pName As String = "[EVAPORATION]"
    Private pSWMMProject As atcSWMMProject
    Public Timeseries As atcData.atcTimeseries

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub New(ByVal aSWMMProject As atcSWMMProject)
        pSWMMProject = aSWMMProject
    End Sub

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject, ByVal aContents As String)
        pSWMMProject = aSWMMPRoject
        FromString(aContents)
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        Timeseries = New atcData.atcTimeseries(pSWMMProject)
        Timeseries.Attributes.SetValue("Location", "")
        Timeseries.Attributes.SetValue("Constituent", "PEVT")
        Timeseries.ValuesNeedToBeRead = True

        'TODO: populate Timeseries
        'Need to do a delayed action here

        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lWord As String = "TIMESERIES"
        Dim laTSFile As String = String.Empty
        For I As Integer = 0 To lLines.Length - 1
            If Not lLines(I).Trim().StartsWith(";") Then
                If lLines(I).Trim().StartsWith("CONSTANT") Then
                    Timeseries = Nothing
                    Exit Sub
                End If
                laTSFile = lLines(I).Trim().Substring(lWord.Length).Trim()
                'Assuming there is only one TS for Evap
                If laTSFile.Length > 0 Then
                    Timeseries.Attributes.SetValue("Scenario", laTSFile)
                    Timeseries.Attributes.SetValue("Source", "")
                    Timeseries.Attributes.AddHistory("Read from " & pSWMMProject.Specification)
                    Timeseries.Attributes.SetValue("ID", Me.pSWMMProject.DatasetId(True))
                    If pSWMMProject.RainGages.Count > 0 Then
                        Dim lInterval As Double = pSWMMProject.RainGages(0).TimeSeries.Attributes.GetValue("interval")
                        Timeseries.Attributes.SetValue("interval", lInterval) ' this is to go with the raingages' interval
                    End If
                    Timeseries.Attributes.SetValue("Location", pSWMMProject.FilterFileName(laTSFile.TrimEnd("E")))
                    Exit For
                End If
            ElseIf lLines(I).Contains("LatDeg") Then
                'TODO: this info should be in the SWMM climate file for temperature, PET, and wind
                Dim lTemps() As String = lLines(I).Split(" ")
                If IsNumeric(lTemps(1)) Then
                    Timeseries.Attributes.SetValue("LatDeg", Double.Parse(lTemps(1)))
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        If Timeseries IsNot Nothing Then
            lSB.Append(pName & vbCrLf & _
                       ";;Type       Parameters" & vbCrLf & _
                       ";;---------- ----------" & vbCrLf)

            lSB.Append(StrPad("TIMESERIES", 12, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":E", 10, " ", False))
            lSB.Append(" ")
            lSB.Append(vbCrLf)
        End If
        Return lSB.ToString
    End Function

    Public Shared Function TimeSeriesHeaderToString() As String
        Return "[TIMESERIES]" & vbCrLf & _
               ";;Name           Date       Time       Value     " & vbCrLf & _
               ";;-------------- ---------- ---------- ----------"
    End Function

    Public Sub TimeSeriesToStream(ByVal aSW As IO.StreamWriter)
        If Timeseries IsNot Nothing Then
            Dim lTS As atcData.atcTimeseries = Me.pSWMMProject.DataSets.ItemByKey(Timeseries.Attributes.GetValue("ID"))
            If lTS IsNot Nothing Then
                Timeseries = lTS
            End If

            aSW.Write(";EVAPORATION" & vbCrLf)
            pSWMMProject.TimeSeriesToStream(Timeseries, Timeseries.Attributes.GetValue("Location") & ":E", aSW)
        End If
    End Sub

    Public Function TimeSeriesFileNamesToString() As String
        Dim lSB As New StringBuilder

        Dim lFileName As String = ""
        If Timeseries IsNot Nothing Then
            lFileName = """" & PathNameOnly(Me.pSWMMProject.Specification) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "E.DAT" & """"
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":E", 16, " ", False))
            lSB.Append(" FILE " & lFileName)
        End If
        lSB.AppendLine()
        Return lSB.ToString
    End Function

    Public Function TimeSeriesToFile() As Boolean
        If Timeseries IsNot Nothing Then
            Dim lTS As atcData.atcTimeseries = Me.pSWMMProject.DataSets.ItemByKey(Timeseries.Attributes.GetValue("ID"))
            If lTS IsNot Nothing Then
                Timeseries = lTS
            End If
            Timeseries = atcData.modTimeseriesMath.Aggregate(Timeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            Dim lFileName As String = PathNameOnly(Me.pSWMMProject.Specification) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "E.DAT"
            Dim lSB As New StringBuilder
            lSB.Append(Me.pSWMMProject.TimeSeriesToString(Timeseries, Timeseries.Attributes.GetValue("Location") & ":E", "PEVT"))
            SaveFileString(lFileName, lSB.ToString)
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub TimeSeriesFromFile(ByVal aFilename As String, ByVal aTS As atcData.atcTimeseries)
        If Not aTS.ValuesNeedToBeRead Then
            Exit Sub
        End If
        Dim lStn As String = aTS.Attributes.GetValue("Location")
        Dim lDates As New List(Of Double)
        Dim lValues As New List(Of Double)

        'Set up common date array

        Dim lSR As System.IO.StreamReader = New System.IO.StreamReader(aFilename)
        Dim lLineCtr As Integer = 0
        Dim lTimeStep As Double = aTS.Attributes.GetValue("interval")
        While Not lSR.EndOfStream
            Dim line As String = lSR.ReadLine()
            Dim lItems As Generic.List(Of String) = atcSWMMProject.SplitSpaceDelimitedWithQuotes(line.Trim())
            Dim lDateParts() As String = lItems(0).Split("/")
            Dim lTimeParts() As String = lItems(1).Split(":")

            Dim ldate As Double = Jday(Integer.Parse(lDateParts(2)), Integer.Parse(lDateParts(0)), Integer.Parse(lDateParts(1)), Integer.Parse(lTimeParts(0)), Integer.Parse(lTimeParts(1)), 0)
            If lLineCtr = 0 Then
                lDates.Add(ldate)
                lValues.Add(Double.NaN)
            End If
            'SWMM5 denote a day has 0 hour to 23 hour, but atcTimeseries denote a day as 1 ~ 24 hour
            ldate += lTimeStep
            lDates.Add(ldate)
            lValues.Add(Double.Parse(lItems(lItems.Count - 1)))

            lLineCtr += 1
        End While

        aTS.ValuesNeedToBeRead = False

        Dim lDates1 As New atcData.atcTimeseries(Nothing)
        lDates1.numValues = lDates.Count
        lDates1.Values = lDates.ToArray()

        aTS.numValues = lDates.Count
        aTS.Dates = lDates1
        aTS.Values = lValues.ToArray
        lSR.Close()
        aTS.Attributes.SetValue("Source", aFilename)
    End Sub

End Class

