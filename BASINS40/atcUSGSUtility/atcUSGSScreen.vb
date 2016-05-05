Imports atcData
Imports atcGraph
Imports atcUtility
Imports ZedGraph

Public Class atcUSGSScreen
    Private Shared gRand As Random
    Private Shared gRandSeed As Integer = 1
    Public Shared Function PrintDataSummary(ByVal aTS As atcTimeseries) As String
        'print out data summary, usgs style
        'If pMissingDataMonth Is Nothing Then
        '    pMissingDataMonth = New atcCollection()
        'Else
        '    pMissingDataMonth.Clear()
        'End If
        'Dim lNeedToRecordMissingMonth As Boolean = (pMissingDataMonth.Count = 0)
        'Dim lNeedToRecordMissingMonth As Boolean = True
        Dim lFileName As String = IO.Path.GetFileName(aTS.Attributes.GetValue("History 1"))
        Dim lDate(5) As Integer
        Dim lStrBuilderDataSummary As New System.Text.StringBuilder
        lStrBuilderDataSummary.AppendLine("READING FILE NAMED " & lFileName)
        J2Date(aTS.Dates.Value(0), lDate)
        lStrBuilderDataSummary.AppendLine("FIRST YEAR IN RECORD =  " & lDate(0))
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        lStrBuilderDataSummary.AppendLine(" LAST YEAR IN RECORD =  " & lDate(0))
        lStrBuilderDataSummary.AppendLine( _
            "                 MONTH        " & vbCrLf & _
            " YEAR   J F M A M J J A S O N D")

        For I As Integer = 0 To aTS.numValues - 1
            J2Date(aTS.Dates.Value(I), lDate)
            lStrBuilderDataSummary.Append(lDate(0).ToString.PadLeft(5, " "))
            Dim lDaysInMonth As Integer = DayMon(lDate(0), lDate(1))

            For M As Integer = 1 To 12
                Dim lMonthFlag As String = "."
                If lDate(1) = M Then
                    lDaysInMonth = DayMon(lDate(0), M)
                    Dim lDayInMonthDone As Integer = 0
                    While lDate(2) <= lDaysInMonth And lDate(1) = M
                        lDayInMonthDone += 1
                        If I = aTS.numValues Then Exit While
                        If Double.IsNaN(aTS.Value(I + 1)) OrElse aTS.Value(I + 1) < 0 Then
                            lMonthFlag = "X"
                        End If
                        I += 1
                        J2Date(aTS.Dates.Value(I), lDate)
                    End While
                    If lDayInMonthDone < lDaysInMonth Then
                        If lMonthFlag = "." Then lMonthFlag = "X"
                    End If
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("   " & lMonthFlag)
                    ElseIf M = 12 Then
                        'End of one year
                        lStrBuilderDataSummary.AppendLine(" " & lMonthFlag)
                        I -= 1
                        J2Date(aTS.Dates.Value(I), lDate) 'need to re-read the year that is just being examined of its December
                    Else
                        lStrBuilderDataSummary.Append(" " & lMonthFlag)
                    End If
                Else
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("X".PadLeft(4, " "))
                    ElseIf M = 12 Then
                        lStrBuilderDataSummary.AppendLine(" X")
                    Else
                        lStrBuilderDataSummary.Append(" X")
                    End If
                End If
                'If lNeedToRecordMissingMonth Then
                '    Dim lKeyToBeAdded As String = lDate(0).ToString & "_" & M.ToString.PadLeft(2, "0")
                '    If Not pMissingDataMonth.Keys.Contains(lKeyToBeAdded) Then
                '        If lMonthFlag = "X" Then
                '            pMissingDataMonth.Add(lKeyToBeAdded, lMonthFlag)
                '        End If
                '    End If
                'End If
            Next 'month
        Next 'day
        lStrBuilderDataSummary.AppendLine("")
        lStrBuilderDataSummary.AppendLine(" COMPLETE RECORD = .      INCOMPLETE = X")

        'Dim lDataSummaryFilename As String = IO.Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
        'lDataSummaryFilename = lDataSummaryFilename.Substring("Read from ".Length)
        'lDataSummaryFilename = IO.Path.Combine(lDataSummaryFilename, "DataSummary.txt")
        'Dim lSW As New StreamWriter(lDataSummaryFilename, False)
        'lSW.WriteLine(lStrBuilderDataSummary.ToString)
        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing
        'lStrBuilderDataSummary.Remove(0, lStrBuilderDataSummary.Length)

        Return lStrBuilderDataSummary.ToString()
    End Function

    ''' <summary>
    ''' This routine will plot timeseries duration to show case overlap durations
    ''' This is to facilitate the formation of group of timeseries for batch processing
    ''' </summary>
    ''' <param name="aTSGroup">A group of raw timeseries data</param>
    ''' <returns>% overlap</returns>
    ''' <remarks></remarks>
    Public Shared Function GraphDataDuration(ByVal aTSGroup As atcTimeseriesGroup) As Integer
        'Dim lGraphPlugin As New atcGraph.atcGraphPlugin
        If aTSGroup Is Nothing OrElse aTSGroup.Count = 0 Then
            Return -99
        End If

        Dim lNewTSGroupDuration As New atcTimeseriesGroup()
        Dim lValue As Double
        For I As Integer = 1 To aTSGroup.Count
            Dim lTS As atcTimeseries = aTSGroup(I - 1)
            Dim location As String = lTS.Attributes.GetValue("location")
            Dim lTSDur As atcTimeseries = lTS.Clone()
            For J As Integer = 1 To lTSDur.numValues
                lValue = lTSDur.Value(J)
                If Double.IsNaN(lValue) OrElse lValue < 0 Then
                    lTSDur.Value(J) = atcUtility.GetNaN()
                Else
                    lTSDur.Value(J) = 1 * I
                End If
            Next
            lNewTSGroupDuration.Add(lTSDur)
        Next

        Dim lYAxisTitleText As String = "Timeseries"

        'Make sure graph can't find provisional attribute
        For Each lTs As atcTimeseries In lNewTSGroupDuration
            lTs.Attributes.SetValue("ProvisionalValueAttribute", "X" & lTs.Attributes.GetValue("ProvisionalValueAttribute", ""))
        Next
        Dim lGraphForm As New atcGraph.atcGraphForm()
        'lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphTime(lNewTSGroupDuration, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            .YAxis.Title.Text = "Streamflow Dataset Ordinals"
            .Title.Text = "Streamflow Dataset Timespan"
            'Dim lScaleMin As Double = 1
            '.YAxis.Scale.Min = lScaleMin
            .YAxis.Scale.MaxAuto = False
            .YAxis.Scale.Max = aTSGroup.Count + 1
            '.BarSettings.Base = BarBase.Y
            .Y2Axis.MinSpace = 5
            .YAxis.MinorTic.Color = Drawing.Color.White
            .AxisChange()
            '.CurveList.Item(0).Color = Drawing.Color.Red
            For Each lCuv As CurveItem In .CurveList
                lCuv.Color = RandomColor()
                CType(lCuv, LineItem).Line.Width = 10
            Next
        End With
        'For Each lTS As atcTimeseries In lNewTSGroupDuration
        '    lTS.Clear()
        'Next
        'lNewTSGroupDuration.Clear()
        'lNewTSGroupDuration = Nothing
        lGraphForm.Show()
    End Function

    Public Shared Function RandomColor() As Drawing.Color
        If gRandSeed = 5000 Then gRandSeed = 1
        gRand = New Random(gRandSeed)
        gRandSeed += 1
        Return System.Drawing.Color.FromArgb(gRand.Next(0, 256), gRand.Next(0, 256), gRand.Next(0, 256))
    End Function
End Class
