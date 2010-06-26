Imports atcData
Imports atcUtility
Imports atcGraph
Imports ZedGraph

Public Class frmResult
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pClassLimits As Double()
    Private pAnalysis As String = String.Empty
    Private pListPEGroups As New List(Of atcTimeseriesGroup)

    Public Sub Initialize(ByVal aAnalysis As String, ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, ByVal aClassLimits As Double(), ByVal aOperation As String)
        pAnalysis = aAnalysis
        pDataGroup = aTimeseriesGroup
        pClassLimits = aClassLimits
        pListPEGroups.Clear()

        Select Case pAnalysis.ToLower
            Case "durationhydrograph"
                Me.Text = "Duration Hydrograph Analysis Result"
                If aOperation.ToLower() = "report" Then
                    doReportDH(pDataGroup, pClassLimits)
                ElseIf aOperation.ToLower() = "graph" Then
                    doPlot(pDataGroup)
                End If
            Case "duration"
                Me.Text = "Duration Analysis Result"
            Case "compare"
                Me.Text = "Compare Analysis Result"
            Case Else
        End Select
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lSaveFileDialog As New Windows.Forms.SaveFileDialog
        Dim lFileDialogTitle As String
        Select Case pAnalysis.ToLower
            Case "duration"
                lFileDialogTitle = "Save Duration Report As..."
            Case "compare"
                lFileDialogTitle = "Save Compare Report As..."
            Case "durationhydrograph"
                lFileDialogTitle = "Save Duration Hydrograph Report As..."
            Case Else
                lFileDialogTitle = "Save Report As..."
        End Select
        With lSaveFileDialog
            .Title = lFileDialogTitle
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                IO.File.WriteAllText(.FileName, txtReport.Text)
                OpenFile(.FileName)
            End If
        End With
    End Sub

    Public Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If sender.text = "Graph" Then
            doPlot(pDataGroup)
        Else
            atcDataManager.ShowDisplay(sender.Text, pDataGroup)
        End If
    End Sub

    Private pHelpLocationCompare As String = "BASINS Details\Analysis\Time Series Functions\Compare.html"
    Private pHelpLocationDuration As String = "BASINS Details\Analysis\Time Series Functions\Duration.html"
    Private pHelpLocationDurationHydrograph As String = "BASINS Details\Analysis\Time Series Functions\DurationHydrograph.html"

    Public ReadOnly Property HelpLocation() As String
        Get
            Select Case pAnalysis.ToLower
                Case "duration"
                    Return pHelpLocationDuration
                Case "compare"
                    Return pHelpLocationCompare
                Case "durationhydrograph"
                    Return pHelpLocationDurationHydrograph
                Case Else
                    Return pHelpLocationDuration
            End Select
        End Get
    End Property

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(HelpLocation)
    End Sub

    Public Overridable Function doPlot(ByRef aDatagroup As atcDataGroup) As Boolean
        Select Case pAnalysis.ToLower
            Case "duration"
            Case "compare"
            Case "durationhydrograph"
                doPlotDH(aDatagroup, pClassLimits)
        End Select
        Return True
    End Function

    Public Overridable Sub doReportDH(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double())
        Dim lStrbuilder As New Text.StringBuilder
        For Each lTS In aTimeseriesGroup
            lStrbuilder.AppendLine(DurationHydrograph(lTS, aClassLimits))
        Next
        txtReport.Text = lStrbuilder.ToString
    End Sub

    Public Sub doPlotDH(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double())
        If pListPEGroups.Count = 0 Then
            For Each lTS As atcTimeseries In aTimeseriesGroup
                pListPEGroups.Add(DurationHydrographSeasons(lTS, aClassLimits))
            Next
        End If
        For Each lTSgroup As atcTimeseriesGroup In pListPEGroups
            DurationHydrographPlot(lTSgroup)
        Next
    End Sub

    Private Sub DataGroupChanged() Handles pDataGroup.Added, pDataGroup.Removed
        If pDataGroup IsNot Nothing And pClassLimits IsNot Nothing Then
            If pDataGroup.Count > 0 Then
                doReportDH(pDataGroup, pClassLimits)
            End If
        End If
    End Sub

    Private Sub mnuSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Data For Analysis", pDataGroup)
    End Sub

    Private Sub frmResult_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        pDataGroup = Nothing
        pClassLimits = Nothing
    End Sub

    Private Sub frmResult_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public Function DurationHydrographPlot(ByVal aDataGroup As atcTimeseriesGroup) As Boolean
        Dim doneIt As Boolean = True
        Dim lp As String = ""
        Dim lgraphForm As New atcGraph.atcGraphForm()

        Dim lZgc As ZedGraphControl = lgraphForm.ZedGraphCtrl
        Dim lGraphDurHyd As New clsGraphTime(aDataGroup, lZgc)
        lgraphForm.Grapher = lGraphDurHyd

        Dim lXTitle As String = "Duration hydrograph for " & aDataGroup(0).Attributes.GetValue("stanam") & vbCrLf
        lXTitle &= "For period " & aDataGroup(0).Attributes.GetValue("StartYMD") & " to " & aDataGroup(0).Attributes.GetValue("EndYMD")
        Dim lYTitle As String = "STREAMFLOW IN CUBIC FEET PER SECOND"
        With lGraphDurHyd.ZedGraphCtrl.GraphPane

            With .XAxis
                .MinorGrid.IsVisible = False
                .MajorGrid.IsVisible = False
                .Title.Text = lXTitle
            End With
            With .YAxis
                .Type = AxisType.Log
                .Scale.IsUseTenPower = True
                .MinorGrid.IsVisible = False
                .MajorGrid.IsVisible = False
                .Title.Text = lYTitle
            End With
            .AxisChange()

            '.Legend.Position = LegendPos.TopFlushLeft
            '.IsPenWidthScaled = True
            '.LineType = LineType.Stack
            '.ScaledPenWidth(50, 2)
            '.CurveList.Item(0).Color = Drawing.ColorTranslator.FromHtml("#FF0000") 'Base condition: red
            '.CurveList.Item(1).Color = Drawing.ColorTranslator.FromHtml("#00FF00") 'Natural condition: green

            'For Each li As LineItem In .CurveList
            '    li.Line.Width = 2
            '    Dim lFS As New FontSpec
            '    lFS.FontColor = li.Line.Color
            '    li.Label.FontSpec = lFS
            '    li.Label.FontSpec.Border.IsVisible = False
            'Next

            .Legend.IsVisible = False
        End With
        Dim lPEDecimals As String = "Percentiles" & vbCrLf
        For I As Integer = 0 To aDataGroup.Count - 1
            lPEDecimals &= aDataGroup(I).Attributes.GetValue("PEDecimal")
            If I + 1 Mod 5 = 0 Then
                lPEDecimals &= Environment.NewLine
            End If
        Next
        Dim lText As New TextObj(lPEDecimals, 0.45F, 0.05F)
        With lText
            .Location.CoordinateFrame = CoordType.ChartFraction
            .Location.AlignH = AlignH.Left
            .FontSpec.Family = "Courier"
            .FontSpec.Border.IsVisible = False
            .FontSpec.StringAlignment = Drawing.StringAlignment.Near
        End With

        lGraphDurHyd.ZedGraphCtrl.GraphPane.GraphObjList.Add(lText)
        lgraphForm.Width = 720
        lgraphForm.Height = 560
        lgraphForm.Show()

        Return doneIt
    End Function
End Class