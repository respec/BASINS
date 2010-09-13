Imports atcData
Imports atcUtility
Imports atcGraph
Imports atcDurationCompare
Imports zedgraph

Public Class frmDuration
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pClassLimits As Double()

    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, ByVal aClassLimits As Double())
        pDataGroup = aTimeseriesGroup
        pClassLimits = aClassLimits

        doDurReport(aTimeseriesGroup, aClassLimits)
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        Dim lSaveFileDialog As New Windows.Forms.SaveFileDialog
        With lSaveFileDialog
            .Title = "Save Duration Report As..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                IO.File.WriteAllText(.FileName, txtReport.Text)
                OpenFile(.FileName)
            End If
        End With
    End Sub

    Public Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        If sender.text = "Graph" Then
            doDurPlot(pDataGroup)
        Else
            atcDataManager.ShowDisplay(sender.Text, pDataGroup)
        End If
    End Sub

    Private pHelpLocation As String = "BASINS Details\Analysis\Time Series Functions\Duration.html"
    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(pHelpLocation)
    End Sub

    Public Function doDurPlot(ByRef aDatagroup As atcDataGroup) As Boolean
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

    Private Sub doDurReport(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double())
        Dim lReport As New DurationReport(aClassLimits)
        With txtReport
            .Text = ""
            For Each lTs As atcTimeseries In aTimeseriesGroup
                .Text &= "Duration Report for " & lTs.ToString & vbCrLf & vbCrLf & DurationStats(lTs, lReport)
            Next
            .SelectionStart = 0
            .SelectionLength = 0
        End With
    End Sub
    Private Sub DataGroupChanged() Handles pDataGroup.Added, pDataGroup.Removed
        txtReport.Text = ""
        doDurReport(pDataGroup, pClassLimits)
    End Sub

    Private Sub mnuSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Data For Analysis", pDataGroup)
    End Sub

    Private Sub frmDuration_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        pDataGroup = Nothing
        pClassLimits = Nothing
    End Sub
End Class