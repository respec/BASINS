Imports atcData
Imports atcUtility
Imports atcDurationCompare
Imports atcGraph
Imports ZedGraph
'Imports System.Windows.Forms.WebBrowser

Public Class frmCompare

    Private pObserved As atcTimeseries
    Private pSimulated As atcTimeseries

    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, ByVal aClassLimits As Double())
        If aTimeseriesGroup.Count > 0 Then
            pObserved = aTimeseriesGroup(0)
            If aTimeseriesGroup.Count > 1 Then
                pSimulated = aTimeseriesGroup(1)
            End If
        End If

        Dim lReport As New DurationReport(aClassLimits)
        With txtReport
            .Text = ""
            .Text &= "Compare Report:" & vbCrLf & vbCrLf & CompareStats(pObserved, pSimulated, lReport.ClassLimitsNeeded(pObserved))
            .SelectionStart = 0
            .SelectionLength = 0
        End With
    End Sub

    Public Sub mnuAnalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, New atcTimeseriesGroup(pObserved, pSimulated))
    End Sub

    Private pHelpLocation As String = "BASINS Details\Analysis\Time Series Functions\Compare.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub

    Public Function doComparePlot(ByRef aDatagroup As atcDataGroup) As Boolean

        Dim doneIt As Boolean = True
        Dim lp As String = ""
        Dim lgraphForm As New atcGraph.atcGraphForm()

        Dim lZgc As ZedGraphControl = lgraphForm.ZedGraphCtrl
        Dim lGraphDur As New clsGraphProbability(aDatagroup, lZgc)
        lgraphForm.Grapher = lGraphDur

        'With lGraphDur.ZedGraphCtrl.GraphPane
        '    With .XAxis
        '        .Scale.MaxAuto = False
        '        .Scale.MinAuto = False
        '        .MinorGrid.IsVisible = False
        '        .MajorGrid.IsVisible = False
        '        .Scale.Min = 0.001
        '    End With
        '    '.YAxis.Type = AxisType.Linear
        '    .YAxis.MinorGrid.IsVisible = False
        '    .YAxis.MajorGrid.IsVisible = False

        '    If .YAxis.Scale.Min < 1 Then
        '        .YAxis.Scale.MinAuto = False
        '        .YAxis.Scale.Min = 1
        '        '.YAxis.Scale.Max = 1000000
        '        .AxisChange()
        '    End If

        '    '.Legend.Position = LegendPos.TopFlushLeft
        '    '.IsPenWidthScaled = True
        '    '.LineType = LineType.Stack
        '    '.ScaledPenWidth(50, 2)
        '    .CurveList.Item(0).Color = Drawing.ColorTranslator.FromHtml("#FF0000") 'Base condition: red
        '    '.CurveList.Item(1).Color = Drawing.ColorTranslator.FromHtml("#00FF00") 'Natural condition: green

        '    For Each li As LineItem In .CurveList
        '        li.Line.Width = 2
        '        Dim lFS As New FontSpec
        '        lFS.FontColor = li.Line.Color
        '        li.Label.FontSpec = lFS
        '        li.Label.FontSpec.Border.IsVisible = False
        '    Next
        'End With

        lgraphForm.Show()

        Return doneIt
    End Function

End Class