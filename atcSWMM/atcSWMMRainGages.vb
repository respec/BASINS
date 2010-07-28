Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

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
        Dim lLine As String = String.Empty
        For I As Integer = 0 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 0 Then
                lLine = lLines(I)
                Dim lRainGage As New atcSWMMRainGage
                With lRainGage
                    Dim lItem As String = StrSplit(lLine, " ", "")
                    Dim lIndex As Integer = 0
                    While lItem.Length > 0
                        Select Case lIndex
                            Case 0 : .Name = lItem.Trim
                            Case 1 : .Form = lItem.Trim
                            Case 2 : .Interval = lItem.Trim
                            Case 3 : .SnowCatchFactor = lItem.Trim
                            Case 4 : .Type = lItem.Trim
                            Case 5
                                .TimeSeries = New atcData.atcTimeseries(pSWMMProject)
                                .TimeSeries.Attributes.SetValue("Location", .Name)
                                .TimeSeries.Attributes.SetValue("Constituent", "PREC")
                                lItem = lItem.Trim()
                                lItem = lItem.Trim("""")
                                .TimeSeries.Attributes.SetValue("Scenario", lItem)

                                If .Type.ToLower() = "file" Then
                                    Dim lpath As String = FindFile("Rain Gage Data File", lItem, lItem.Substring(lItem.LastIndexOf(".")))
                                    ReadDataExternal(lpath, .TimeSeries)
                                ElseIf .Type.ToLower() = "timeseries" Then
                                    .TimeSeries.ValuesNeedToBeRead = True
                                End If
                        End Select

                        lItem = StrSplit(lLine, " ", "")
                        lIndex += 1
                    End While
                End With
                Me.Add(lRainGage)
            End If
        Next
    End Sub

    Public Sub ReadDataExternal(ByVal aFilename As String, ByVal aTS As atcData.atcTimeseries)
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
            Dim lItems() As String = line.Split(" ")
            'There should be 7 columns
            'Stn Y M D H M Value
            If lItems(0) <> lStn Then
                Continue While
            End If
            Dim ldate As Double = Jday(Integer.Parse(lItems(1)), Integer.Parse(lItems(2)), Integer.Parse(lItems(3)), Integer.Parse(lItems(4)), Integer.Parse(lItems(5)), 0)
            lDates.Add(ldate)
            lValues.Add(Double.Parse(lItems(lItems.Length - 1)))
        End While

        aTS.ValuesNeedToBeRead = False
        aTS.Dates.numValues = lDates.Count
        aTS.numValues = lDates.Count
        aTS.Dates.Values = lDates.ToArray
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
End Class
