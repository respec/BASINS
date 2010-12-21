Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions


Public Class atcSWMMTemperature
    Implements IBlock

    Private pName As String = "[TEMPERATURE]"
    Private pSWMMProject As atcSWMMProject
    Public Timeseries As atcData.atcTimeseries

    'Public AuxiParms As New atcData.atcDataAttributes
    Public AuxiParms As New Dictionary(Of String, String)

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject, ByVal aContents As String)
        pSWMMProject = aSWMMPRoject
        FromString(aContents)
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        Timeseries = New atcData.atcTimeseries(pSWMMProject)
        Timeseries.Attributes.SetValue("Location", "")
        Timeseries.Attributes.SetValue("Constituent", "ATEM")
        Timeseries.ValuesNeedToBeRead = True

        'TODO: populate Timeseries
        'Need to do a delayed action here

        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lWord As String = "TIMESERIES"
        Dim laTSFile As String = String.Empty
        For I As Integer = 0 To lLines.Length - 1
            If Not lLines(I).Trim().StartsWith(";") Then
                If lLines(I).StartsWith(lWord) Then
                    laTSFile = lLines(I).Trim().Substring(lWord.Length).Trim()
                    If laTSFile.Length > 0 Then
                        Timeseries.Attributes.SetValue("Scenario", laTSFile)
                        Timeseries.Attributes.SetValue("Source", "")
                        Timeseries.Attributes.AddHistory("Read from " & pSWMMProject.Specification)
                        Timeseries.Attributes.SetValue("ID", "T")
                        Timeseries.Attributes.SetValue("Location", pSWMMProject.FilterFileName(laTSFile.TrimEnd("T")))
                    End If
                Else
                    'Assuming there is only one TS for Temp

                    'Dim lDef As New atcData.atcDefinedValue
                    'With lDef
                    '    .Definition = New atcData.atcAttributeDefinition()
                    '    .Definition.Name = lparms(0) 'Parm name such as WINDSPEED
                    '    If lparms.Length > 2 Then
                    '        .Definition.Description = lparms(1) 'specification eg monthly
                    '        .Value = lparms(2) 'Parm values as a string
                    '    Else
                    '        .Value = lparms(1) 'sometimes, there is no description
                    '    End If
                    '    .Definition.TypeString = "String"
                    'End With
                    'AuxiParms.Add(lDef)

                    Dim lLine As String = lLines(I)
                    Dim laKey As String = StrSplit(lLine, " ", "")
                    If AuxiParms.ContainsKey(laKey.Trim()) Then
                        AuxiParms.Item(laKey.Trim()) &= ";" & lLine
                    Else
                        AuxiParms.Add(laKey.Trim(), lLine)
                    End If
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        If Timeseries IsNot Nothing Then
            lSB.AppendLine(pName)
            lSB.Append(StrPad("TIMESERIES", 12, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":T", 10, " ", False))
            lSB.Append(" ")
            lSB.AppendLine()
        End If

        If AuxiParms.Count > 0 Then
            For Each lParmKey As String In AuxiParms.Keys
                For Each line As String In AuxiParms(lParmKey).Split(";")
                    lSB.Append(StrPad(lParmKey, 12, " ", False))
                    lSB.AppendLine(line)
                Next
            Next
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

            aSW.Write(";TEMPERATURE" & vbCrLf)
            Me.pSWMMProject.TimeSeriesToStream(Timeseries, Timeseries.Attributes.GetValue("Location") & ":T", aSW)
        End If
    End Sub

    Public Function TimeSeriesFileNamesToString() As String
        Dim lSB As New StringBuilder

        If Timeseries IsNot Nothing Then
            Dim lFileName As String = """" & PathNameOnly(Me.pSWMMProject.Specification) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "T.DAT" & """"
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":T", 16, " ", False))
            lSB.Append(" FILE " & lFileName)
        End If

        Return lSB.ToString
    End Function

    Public Function TimeSeriesToFile() As Boolean
        If Timeseries IsNot Nothing Then
            Dim lTS As atcData.atcTimeseries = Me.pSWMMProject.DataSets.ItemByKey(Timeseries.Attributes.GetValue("ID"))
            If lTS IsNot Nothing Then
                Timeseries = lTS
            End If
            Dim lFileName As String = PathNameOnly(Me.pSWMMProject.Specification) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "T.DAT"
            Dim lSB As New StringBuilder
            lSB.Append(Me.pSWMMProject.TimeSeriesToString(Timeseries, Timeseries.Attributes.GetValue("Location") & ":T", "ATEM"))
            SaveFileString(lFileName, lSB.ToString)
        End If

    End Function

    Public Sub TimeseriesFromFile(ByVal aFilename As String, ByVal aTS As atcData.atcTimeseries)
        If Not aTS.ValuesNeedToBeRead Then
            Exit Sub
        End If
        Dim lStn As String = aTS.Attributes.GetValue("Location")
        Dim lDates As New List(Of Double)
        Dim lValues As New List(Of Double)

        'Set up common date array

        Dim lSR As System.IO.StreamReader = New System.IO.StreamReader(aFilename)
        Dim lLineCtr As Integer = 0
        While Not lSR.EndOfStream
            Dim line As String = lSR.ReadLine()
            Dim lItems() As String = Regex.Split(line.Trim(), "\s+")
            Dim lDateParts() As String = lItems(0).Split("/")
            Dim lTimeParts() As String = lItems(1).Split(":")

            'If lItems(0) <> lStn Then
            '    Continue While
            'End If

            Dim ldate As Double = -99.0
            If lLineCtr = 0 Then
                ldate = Jday(Integer.Parse(lDateParts(2)), Integer.Parse(lDateParts(0)), Integer.Parse(lDateParts(1)), Integer.Parse(lTimeParts(0)), Integer.Parse(lTimeParts(1)), 0)
                lDates.Add(ldate)
                lValues.Add(Double.NaN)
            End If

            'SWMM5 denote a day has 0 hour to 23 hour, but atcTimeseries denote a day as 1 ~ 24 hour
            ldate = Jday(Integer.Parse(lDateParts(2)), Integer.Parse(lDateParts(0)), Integer.Parse(lDateParts(1)), Integer.Parse(lTimeParts(0)) + 1, Integer.Parse(lTimeParts(1)), 0)
            lDates.Add(ldate)
            lValues.Add(Double.Parse(lItems(lItems.Length - 1)))

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
