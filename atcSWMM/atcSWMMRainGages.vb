Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMRainGages
    Inherits KeyedCollection(Of String, atcSWMMRainGage)
    Implements IBlock

    Private pName As String = "[RAINGAGES]"
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aRainGage As atcSWMMRainGage) As String
        Dim lKey As String = aRainGage.Name
        Return lKey
    End Function

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'TODO: populate Timeseries
        'Need to do a delayed action here if the Form is TIMESERIES

        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim laTSFile As String = String.Empty

        Dim lSectionName As String = lLines(0).Trim()
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).Trim().StartsWith(";") And lLines(I).Trim().Length > 0 Then

                Dim lItems() As String = Regex.Split(lLines(I).Trim(), "\s+")
                Dim lRainGage As atcSWMMRainGage = Nothing
                If Me.Contains(lItems(0)) Then
                    lRainGage = Me(lItems(0))
                Else
                    lRainGage = New atcSWMMRainGage
                End If

                With lRainGage
                    For J As Integer = 0 To lItems.Length - 1
                        Select Case J
                            Case 0 : .Name = lItems(J).Trim
                            Case 1
                                If lSectionName = "[RAINGAGES]" Then
                                    .Form = lItems(J).Trim
                                ElseIf lSectionName = "[SYMBOLS]" Then
                                    .XPos = Double.Parse(lItems(J))
                                End If
                            Case 2
                                If lSectionName = "[RAINGAGES]" Then
                                    .Interval = lItems(J).Trim()
                                ElseIf lSectionName = "[SYMBOLS]" Then
                                    .YPos = Double.Parse(lItems(J))
                                End If
                            Case 3 : .SnowCatchFactor = Double.Parse(lItems(J))
                            Case 4 : .Type = lItems(J).Trim
                            Case 5
                                .TimeSeries = New atcData.atcTimeseries(pSWMMProject)
                                .TimeSeries.Attributes.SetValue("Location", .Name.Trim())
                                .TimeSeries.Attributes.SetValue("Constituent", "PREC")
                                Dim lItem As String = lItems(J).Trim()
                                'lItem = lItem.Trim("""")
                                lItem = pSWMMProject.FilterFileName(lItem)
                                .TimeSeries.Attributes.SetValue("Scenario", lItem)

                                If .Type.ToLower() = "file" Then
                                    Dim lpath As String = FindFile("Rain Gage Data File", lItem, lItem.Substring(lItem.LastIndexOf(".")))
                                    .TimeSeries.ValuesNeedToBeRead = True
                                    TimeseriesFromFile(lpath, .TimeSeries)
                                ElseIf .Type.ToLower() = "timeseries" Then
                                    .TimeSeries.ValuesNeedToBeRead = True
                                End If
                            Case Else
                                .AuxiParms &= lItems(J) & "  " 'some leftover text not sure what, but save it here
                        End Select
                    Next
                End With
                If Not Me.Contains(lItems(0).Trim) Then
                    Me.Add(lRainGage)
                    Me.ChangeItemKey(lRainGage, lItems(0).Trim())
                End If
                'Debug.Print(Me(lItems(0).Trim()).TimeSeries.numValues & ":" & Me(lItems(0).Trim()).TimeSeries.Value(5))
            End If
        Next
    End Sub

    Public Sub AddValue(ByVal aLine As String)

        Dim lItems() As String = aLine.Split(" ")
        Dim lLocation As String = lItems(0).Trim()
        Dim lRaingage As atcSWMMRainGage = Nothing
        For Each lRaingage In Me
            If lRaingage.TimeSeries.Attributes.GetValue("Location") = lLocation Then
                Exit For
            End If
        Next
        Dim ldatePart As String = aLine.Substring(17, 10).Trim()
        Dim ltimePart As String = aLine.Substring(28, 10).Trim()

        Dim ltimeMn As Integer = Integer.Parse(ltimePart.Substring(0, ltimePart.IndexOf(":")))
        Dim ltimeSe As Integer = Integer.Parse(ltimePart.Substring(ltimePart.IndexOf(":") + 1))
        Dim lDateElapsed() As Integer = {0, 0, 0, 0, ltimeMn, ltimeSe}
        Dim ltimeElapsed As Double = pSWMMProject.Options.SJDate + Date2J(lDateElapsed)
        'lRaingage.ListDates.Add(ltimeElapsed)
        'lRaingage.ListValues.Add(Double.Parse(aLine.Substring(39, 8)))

        With lRaingage.TimeSeries
            .numValues += 1
            .Value(.numValues) = Double.Parse(aLine.Substring(39, 8))
            .Dates.Value(.numValues) = ltimeElapsed
            .ValuesNeedToBeRead = False
        End With
    End Sub

    Public Sub TimeseriesFromFile(ByVal aFilename As String, ByVal aTS As atcData.atcTimeseries)
        If Not aTS.ValuesNeedToBeRead Then
            Exit Sub
        End If
        Dim lStn As String = aTS.Attributes.GetValue("Location")
        Dim lDates As New List(Of Double)
        Dim lValues As New List(Of Double)

        'Set up common date array

        Dim lSR As System.IO.StreamReader = New System.IO.StreamReader(aFilename)
        While Not lSR.EndOfStream
            Dim line As String = lSR.ReadLine()
            Dim lItems() As String = Regex.Split(line.Trim(), "\s+")
            'There should be 7 columns
            'Stn Y M D H M Value
            If lItems(0) <> lStn Then
                Continue While
            End If

            'SWMM5 denote a day has 0 hour to 23 hour, but atcTimeseries denote a day as 1 ~ 24 hour
            Dim ldate As Double = Jday(Integer.Parse(lItems(1)), Integer.Parse(lItems(2)), Integer.Parse(lItems(3)), Integer.Parse(lItems(4)) + 1, Integer.Parse(lItems(5)), 0)
            lDates.Add(ldate)
            lValues.Add(Double.Parse(lItems(lItems.Length - 1)))
        End While


        aTS.ValuesNeedToBeRead = False

        Dim lDates1 As New atcData.atcTimeseries(Nothing)
        lDates1.numValues = lDates.Count
        lDates1.Values = lDates.ToArray()

        aTS.numValues = lDates.Count
        aTS.Dates = lDates1
        aTS.Values = lValues.ToArray

        lSR.Close()
    End Sub


    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append(Name & vbCrLf & _
                   ";;               Rain      Recd.  Snow   Data      " & vbCrLf & _
                   ";;Name           Type      Freq.  Catch  Source    " & vbCrLf & _
                   ";;-------------- --------- ------ ------ ----------" & vbCrLf)

        For Each lRaingage As atcSWMMRainGage In Me
            With lRaingage
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.Form, 9, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.Interval, 6, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.SnowCatchFactor, "0.0"), 6, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.Type, 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad("""" & PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & .Name & "P.DAT" & """", 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.Name, 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.Units, 5, " ", False))
                lSB.Append(" ")
                lSB.Append(vbCrLf)
            End With
        Next

        Return lSB.ToString
    End Function

    Public Function CoordinatesToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[SYMBOLS]" & vbCrLf & _
                   ";;Gage           X-Coord            Y-Coord           " & vbCrLf & _
                   ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lRaingage As atcSWMMRainGage In Me
            With lRaingage
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.XPos, "0.000"), 18, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.YPos, "0.000"), 18, " ", False))
                lSB.Append(vbCrLf)
            End With
        Next

        Return lSB.ToString
    End Function

    Public Function TimeSeriesToFile() As Boolean
        For Each lRaingage As atcSWMMRainGage In Me
            Dim lFileName As String = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & lRaingage.Name & "P.DAT"
            Dim lSB As New StringBuilder
            lSB.Append(Me.pSWMMProject.TimeSeriesToString(lRaingage.TimeSeries, lRaingage.Name))
            SaveFileString(lFileName, lSB.ToString)
        Next
    End Function
End Class

Public Class atcSWMMRainGage
    Public Name As String
    Public Form As String = "INTENSITY" 'intensity (or volume or cumulative)
    Public Interval As String = "1:00"
    Public SnowCatchFactor As Double = 1.0
    Public Type As String = "FILE" '(timeseries) or file
    Public TimeSeries As atcData.atcTimeseries
    Public Units As String = "IN" 'in (or mm)
    Public YPos As Double = 0.0
    Public XPos As Double = 0.0
    Public ListDates As New List(Of Double)
    Public ListValues As New List(Of Double)
    Public AuxiParms As String = String.Empty
End Class
