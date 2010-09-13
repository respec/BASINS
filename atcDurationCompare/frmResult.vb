Imports atcData
Imports atcUtility
Imports atcGraph
Imports ZedGraph
Imports MapWinUtility

Public Class frmResult
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pClassLimits As Double()
    Private pAnalysis As String = String.Empty
    Private pListPEGroups As New List(Of atcTimeseriesGroup)
    Public pOpened As Boolean = False

    Public Sub Initialize(ByVal aAnalysis As String, ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, ByVal aClassLimits As Double(), ByVal aOperation As String)
        pAnalysis = aAnalysis
        pDataGroup = aTimeseriesGroup
        pClassLimits = aClassLimits
        pListPEGroups.Clear()

        If aOperation.ToLower = "graph" Then
            doPlot(pDataGroup)
        ElseIf aOperation.ToLower = "report" Then
            Select Case pAnalysis.ToLower
                Case "durationhydrograph"
                    Me.Text = "Duration Hydrograph Analysis Result"
                Case "duration"
                    Me.Text = "Duration Analysis Result"
                Case "compare"
                    Me.Text = "Compare Analysis Result"
                Case Else
            End Select
            doReport(pDataGroup)
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

    'This is designed for automated testing of this class in the TestingScript project
    Public ReadOnly Property Results() As String
        Get
            Return txtReport.Text
        End Get
    End Property

    Public Overridable Function doPlot(ByVal aDatagroup As atcDataGroup) As Boolean
        Select Case pAnalysis.ToLower
            Case "duration", "compare"
                doPlotDurCompare(aDatagroup)
            Case "durationhydrograph"
                doPlotDH(aDatagroup, pClassLimits)
        End Select
        Return True
    End Function

    Public Overridable Function doReport(ByVal adatagroup As atcDataGroup) As Boolean
        Select Case pAnalysis.ToLower
            Case "duration"
                doReportDur(adatagroup, pClassLimits)
            Case "compare"
                doReportCompare(adatagroup, pClassLimits)
            Case "durationhydrograph"
                doReportDH(adatagroup, pClassLimits)
        End Select
        Return True

    End Function

    Public Overridable Sub doReportDH(ByVal aTimeseriesGroup As atcDataGroup, ByVal aPCTs As Double())
        Dim lStrbuilder As New Text.StringBuilder
        For Each lTS In aTimeseriesGroup
            lStrbuilder.AppendLine(DurationHydrograph(lTS, aPCTs))
        Next
        txtReport.Text = lStrbuilder.ToString
    End Sub

    Private Sub doReportDur(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double())
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

    Private Sub doReportCompare(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double())
        Dim lObserved As atcTimeseries = Nothing
        Dim lSimulated As atcTimeseries = Nothing
        If aTimeseriesGroup.Count > 0 Then
            lObserved = aTimeseriesGroup(0)
            If aTimeseriesGroup.Count > 1 Then
                lSimulated = aTimeseriesGroup(1)
            End If
        End If

        'Check if the two timeseries has common starting and ending dates
        If aTimeseriesGroup(0).Attributes.GetValue("Start Date") <> aTimeseriesGroup(1).Attributes.GetValue("Start Date") Or _
           aTimeseriesGroup(0).Attributes.GetValue("End Date") <> aTimeseriesGroup(1).Attributes.GetValue("End Date") Then
            txtReport.Text = "The two timeseries needs to have common start and end dates."
            Exit Sub
        End If

        Dim lReport As New DurationReport(aClassLimits)
        With txtReport
            .Text = ""
            .Text &= "Compare Report:" & vbCrLf & vbCrLf & CompareStats(lObserved, lSimulated, lReport.ClassLimitsNeeded(lObserved, lSimulated))
            .SelectionStart = 0
            .SelectionLength = 0
        End With
    End Sub

    Public Sub doPlotDH(ByVal aTimeseriesGroup As atcDataGroup, ByVal aClassLimits As Double(), Optional ByVal aSpecification As String = "")
        If pListPEGroups.Count = 0 Then
            For Each lTS As atcTimeseries In aTimeseriesGroup
                pListPEGroups.Add(DurationHydrographSeasons(lTS, aClassLimits))
            Next
        End If

        Dim lFileCounter As Integer = 0
        Dim lFileName As String = ""
        For Each lTSgroup As atcTimeseriesGroup In pListPEGroups
            If pListPEGroups.Count > 1 And aSpecification IsNot Nothing Then
                'Only do this if more than one graphs are to be plotted and user wants to save into specified files
                If aSpecification.Length > 10 And IO.Directory.Exists(IO.Path.GetDirectoryName(aSpecification)) Then
                    lFileCounter += 1
                    lFileName = IO.Path.Combine(IO.Path.GetFileNameWithoutExtension(aSpecification) & "_" & lFileCounter, IO.Path.GetExtension(aSpecification))
                    DurationHydrographPlot(lTSgroup, lFileName)
                Else
                    Logger.Dbg("atcDurationCompare:doPlotDH: cannot be saved into user specified file: " & aSpecification)
                    DurationHydrographPlot(lTSgroup)
                End If
            Else
                If aSpecification IsNot Nothing AndAlso Not aSpecification = "" Then
                    DurationHydrographPlot(lTSgroup, aSpecification)
                Else
                    DurationHydrographPlot(lTSgroup)
                End If
            End If
        Next
    End Sub

    Public Function doPlotDurCompare(ByRef aDatagroup As atcDataGroup, Optional ByVal aSpecification As String = "") As Boolean
        Dim doneIt As Boolean = True
        Dim lp As String = ""
        Dim lgraphForm As New atcGraph.atcGraphForm()

        Dim lZgc As ZedGraphControl = lgraphForm.ZedGraphCtrl
        Dim lGraphDur As New clsGraphProbability(aDatagroup, lZgc)
        lgraphForm.Grapher = lGraphDur
        If aSpecification = "" Then
            lgraphForm.Show()
        Else
            lZgc.SaveIn(aSpecification)
        End If
        Return doneIt
    End Function

    Public Function DurationHydrographPlot(ByVal aDataGroup As atcTimeseriesGroup, Optional ByVal aSpecification As String = "") As Boolean
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

        If aSpecification = "" Then
            lgraphForm.Show()
        Else
            lZgc.SaveIn(aSpecification)
        End If
        Return doneIt
    End Function

    Private Sub DataGroupChanged() Handles pDataGroup.Added, pDataGroup.Removed
        If Not pOpened Then Exit Sub
        If pDataGroup IsNot Nothing And pClassLimits IsNot Nothing Then
            If pDataGroup.Count > 0 Then
                doReport(pDataGroup)
            End If
        End If
    End Sub

    Private Sub frmResult_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        pDataGroup = Nothing
        pClassLimits = Nothing
        pOpened = False
    End Sub

    Private Sub frmResult_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pOpened = True
    End Sub

    Private Sub mnuSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSave.Click
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

    Public Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        If sender.text = "Graph" Then
            doPlot(pDataGroup)
        Else
            atcDataManager.ShowDisplay(sender.Text, pDataGroup)
        End If
    End Sub

    Private Sub mnuSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Data For Analysis", pDataGroup)
    End Sub

    Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        frmResult_Disposed(sender, e)
        Me.Close()
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp(HelpLocation)
    End Sub

    Private Sub txtReport_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReport.TextChanged
        txtReport.SelectionLength = 0
    End Sub
End Class