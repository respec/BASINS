Imports atcData
Imports atcUtility
Imports atcGraph
Imports ZedGraph

Public Class frmResult
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pClassLimits As Double()

    Private pAnalysis As String = String.Empty

    Public Sub Initialize(ByVal aAnalysis As String, ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, ByVal aClassLimits As Double())
        pAnalysis = aAnalysis
        pDataGroup = aTimeseriesGroup
        pClassLimits = aClassLimits

        Select Case pAnalysis.ToLower
            Case "durationhydrograph"
                doReportDH(pDataGroup, pClassLimits)
                Me.Text = "Duration Hydrograph Analysis Result"
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
                doPlotDH(aDatagroup)
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

    Private Sub DataGroupChanged() Handles pDataGroup.Added, pDataGroup.Removed
        doReportDH(pDataGroup, pClassLimits)
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
End Class