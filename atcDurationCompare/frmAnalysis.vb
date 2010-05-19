Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmAnalysis

    Private pDataGroup As atcTimeseriesGroup
    Private pResultForm As System.Windows.Forms.Form

    Private pDefaultNumClasses As Integer = 35
    Private pDefaultClasses As Double() = {0}

    Public Property DataGroup() As atcTimeseriesGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal value As atcTimeseriesGroup)
            pDataGroup = value
        End Set
    End Property

    Public Property ResultForm() As System.Windows.Forms.Form
        Get
            Return pResultForm
        End Get
        Set(ByVal value As System.Windows.Forms.Form)
            pResultForm = value
        End Set
    End Property

    Public Property DefaultClasses() As Double()
        Get
            Return pDefaultClasses
        End Get
        Set(ByVal value As Double())
            pDefaultClasses = value
        End Set
    End Property

    Public Sub New(ByVal aDataGroup As atcTimeseriesGroup)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '
        DataGroup = aDataGroup
        DefaultClasses = GenerateClasses(pDefaultNumClasses, DataGroup)
        'lstClassLimits.CurrentValues = DefaultClasses
    End Sub

    Private Sub frmAnalysis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblAnalysisInfo.Text = ""
        lblAnalysisInfo.Text = "Step1. Select timeseries data" & vbNewLine & _
                               "Step2. Examine the class limits (use default or create anew)" & vbNewLine & _
                               "Step3. Choose an anlysis" & vbNewLine & _
                               "Step4. Click either Graph or Report to carry out analysis and see results"
        lstClassLimits.CurrentValues = DefaultClasses
    End Sub

    Private Sub rdoAnalysisCompare_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoAnalysisCompare.CheckedChanged
        If Not sender.checked Then
            Exit Sub
        End If
        Dim lAnalysisInfo As String = String.Empty

        'lblDS1.Visible = True
        'lblDS2.Visible = True
        'txtDS1.Visible = True
        'txtDS2.Visible = True

        With lblAnalysisInfo
            .Text = ""
            .Text = "Compare analysis:" & vbCrLf
            If DataGroup.Count > 1 Then
                .Text &= Space(2) & "TS1: " & TSDescription(DataGroup(0)).TrimEnd(" ")
                .Text &= Space(2) & "TS2: " & TSDescription(DataGroup(1)).TrimEnd(vbTab).TrimEnd(vbCrLf)
            ElseIf DataGroup.Count = 1 Then
                .Text &= Space(2) & "TS1: " & TSDescription(DataGroup(0)).TrimEnd(" ")
                .Text &= Space(2) & "Need to select an additional timeseries."
            End If
        End With


        ResultForm = CreateForm("compare")
    End Sub

    Private Sub rdoAnalysisDuration_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoAnalysisDuration.CheckedChanged
        If Not sender.checked Then
            Exit Sub
        End If

        Dim lAnalysisInfo As String = String.Empty
        For Each lTS As atcTimeseries In DataGroup
            lAnalysisInfo &= TSDescription(lTS)
        Next

        With lblAnalysisInfo
            .Text = ""
            .Text = "Duration analysis:" & vbCrLf
            .Text &= Space(2) & DataGroup.Count & " timeseries are included."
        End With

        ResultForm = CreateForm("duration")
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If ResultForm Is Nothing Then
            Logger.Dbg("Duration/Compare analysis failed to initialize proper form")
            Exit Sub
        End If
        Select Case ResultForm.Name
            Case "frmDuration"
                If ResultForm.IsDisposed Then
                    ResultForm = Nothing
                    ResultForm = CreateForm("duration")
                End If
                CType(ResultForm, frmDuration).Initialize(DataGroup, lstClassLimits.CurrentValues)
                'CType(ResultForm, frmDuration).Initialize(DataGroup, Nothing)
            Case "frmCompare"
                If DataGroup.Count < 2 Then
                    Logger.Msg("Need to select two timeseries to conduct compare analysis.")
                    'If txtDS1.Text = "" Then
                    '    txtDS1.Focus()
                    'ElseIf txtDS2.Text = "" Then
                    '    txtDS2.Focus()
                    'End If
                    Exit Sub
                End If
                If ResultForm.IsDisposed Then
                    ResultForm = Nothing
                    ResultForm = CreateForm("compare")
                End If
                CType(ResultForm, frmCompare).Initialize(DataGroup, lstClassLimits.CurrentValues)
                'CType(ResultForm, frmCompare).Initialize(DataGroup, Nothing)
        End Select
        ResultForm.Show()
    End Sub

    Private Sub btnGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraph.Click
        If ResultForm Is Nothing Then
            Logger.Dbg("Duration/Compare analysis failed to initialize proper form")
            Exit Sub
        End If

        Select Case ResultForm.Name
            Case "frmDuration"
                CType(ResultForm, frmDuration).doDurPlot(DataGroup)
            Case "frmCompare"
                If DataGroup.Count < 2 Then
                    Logger.Msg("Need to select two timeseries to conduct compare analysis.")
                    'If txtDS1.Text = "" Then
                    '    txtDS1.Focus()
                    'ElseIf txtDS2.Text = "" Then
                    '    txtDS2.Focus()
                    'End If
                    Exit Sub
                End If
                CType(ResultForm, frmCompare).doComparePlot(DataGroup)
        End Select
    End Sub

    'Private Sub txtDS1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDS1.Click
    '    Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For The Observed Timeseries in a Compare Analysis", DataGroup)
    '    If lTimeseriesGroup.Count > 0 Then
    '        txtDS1.Text = TSDescription(lTimeseriesGroup(0)).TrimEnd(vbTab).TrimEnd(vbCrLf)
    '        DataGroup(0) = lTimeseriesGroup(0)
    '    End If
    'End Sub

    'Private Sub txtDS2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDS2.Click
    '    Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For The Simulated Timeseries in a Compare Analysis", DataGroup)
    '    If lTimeseriesGroup.Count > 1 Then
    '        txtDS2.Text = TSDescription(lTimeseriesGroup(1)).TrimEnd(vbTab).TrimEnd(vbCrLf)
    '        DataGroup(1) = lTimeseriesGroup(1)
    '    End If
    'End Sub

    Private Sub ChangedNumClasses() Handles CtlClassLimits1.ChangedNumClasses
        lstClassLimits.CurrentValues = GenerateClasses(CtlClassLimits1.NumClasses, DataGroup)
    End Sub

    Private Sub DisplayDefault() Handles lstClassLimits.DefaultRequested
        lstClassLimits.CurrentValues = DefaultClasses
    End Sub

    Private Function CreateForm(ByVal Analysis As String) As Object
        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        Dim lFrm As Object = Nothing
        If Analysis = "duration" Then
            lFrm = New frmDuration
        ElseIf Analysis = "compare" Then
            lFrm = New frmCompare
        End If
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            lFrm.mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf lFrm.mnuAnalysis_Click))
        Next
        Return lFrm
    End Function

    Private Sub SelectDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectDataToolStripMenuItem.Click
        DataGroup = atcDataManager.UserSelectData("Select Data For Analysis", DataGroup)
        If rdoAnalysisDuration.Checked Then
            rdoAnalysisDuration_CheckedChanged(rdoAnalysisDuration, Nothing)
        ElseIf rdoAnalysisCompare.Checked Then
            rdoAnalysisCompare_CheckedChanged(rdoAnalysisCompare, Nothing)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Function TSDescription(ByVal aTS As atcTimeseries) As String
        Dim lDesc As String = String.Empty
        lDesc &= aTS.Attributes.GetValue("Constituent") & " at "
        lDesc &= aTS.Attributes.GetValue("STAID") & " " & aTS.Attributes.GetValue("STANAM") & vbCrLf & Space(2)
        Return lDesc
    End Function
End Class
