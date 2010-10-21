Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmAnalysis

    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private pResultForm As System.Windows.Forms.Form

    Private pDefaultNumClasses As Integer = 35
    Private pDefaultClasses As Double() = {0}
    Private pAnalysis As String = String.Empty

    Public Property DataGroup() As atcTimeseriesGroup
        Get
            Return pDataGroup
        End Get
        Set(ByVal value As atcTimeseriesGroup)
            pDataGroup = value
        End Set
    End Property

    Public Property ResultForm() As frmResult
        Get
            Return pResultForm
        End Get
        Set(ByVal value As frmResult)
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

    Private Sub DataChanged() Handles pDataGroup.Added, pDataGroup.Removed
        DefaultClasses = GenerateClasses(pDefaultNumClasses, DataGroup)
    End Sub
    Private Sub frmAnalysis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblAnalysisInfo.Text = ""
        'lblAnalysisInfo.Text = "Step1. Select timeseries data" & vbNewLine & _
        '                       "Step2. Examine the class limits (use default or create anew)" & vbNewLine & _
        '                       "Step3. Choose an analysis" & vbNewLine & _
        '                       "Step4. Click either Graph or Report to carry out analysis and see results"
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

        'ResultForm = CreateForm()
        pAnalysis = "Compare"
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

        'ResultForm = CreateForm()
        pAnalysis = "Duration"
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If ResultForm Is Nothing Then
            ResultForm = CreateForm()
        ElseIf ResultForm.IsDisposed Then
            ResultForm = Nothing
            ResultForm = CreateForm()
        ElseIf pAnalysis = String.Empty Then
            Exit Sub
        End If

        If pAnalysis = "Compare" Then
            If DataGroup.Count < 2 Then
                Logger.Msg("Need to select two timeseries to conduct compare analysis.")
                'If txtDS1.Text = "" Then
                '    txtDS1.Focus()
                'ElseIf txtDS2.Text = "" Then
                '    txtDS2.Focus()
                'End If
                Exit Sub
            End If
        ElseIf pAnalysis = "" Then
            Exit Sub
        End If
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
        ResultForm.Initialize(pAnalysis, DataGroup, lstClassLimits.CurrentValues, "Report")
        ResultForm.Show()
        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.Default
        ResultForm.txtReport.SelectionLength = 0
    End Sub

    Private Sub btnGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraph.Click
        If ResultForm Is Nothing Then
            ResultForm = CreateForm()
        ElseIf ResultForm.IsDisposed Then
            ResultForm = Nothing
            ResultForm = CreateForm()
        ElseIf pAnalysis = String.Empty Then
            Exit Sub
        End If

        If pAnalysis = "Compare" Then
            If DataGroup.Count < 2 Then
                Logger.Msg("Need to select two timeseries to conduct compare analysis.")
                'If txtDS1.Text = "" Then
                '    txtDS1.Focus()
                'ElseIf txtDS2.Text = "" Then
                '    txtDS2.Focus()
                'End If
                Exit Sub
            End If
        End If

        ResultForm.Initialize(pAnalysis, DataGroup, lstClassLimits.CurrentValues, "Graph")
    End Sub

    Private Sub SettingChanged() Handles CtlClassLimits1.SettingChanged
        If CtlClassLimits1.UsePresetClasses Then
            lstClassLimits.CurrentValues = GenerateClasses()
        ElseIf CtlClassLimits1.UpperBound < 0 Or CtlClassLimits1.LowerBound < 0 Then
            lstClassLimits.CurrentValues = GenerateClasses(CtlClassLimits1.NumClasses, DataGroup)
        ElseIf CtlClassLimits1.LowerBound < CtlClassLimits1.UpperBound Then
            lstClassLimits.CurrentValues = GenerateClasses(CtlClassLimits1.NumClasses, DataGroup, CtlClassLimits1.LowerBound, CtlClassLimits1.UpperBound)
        End If
    End Sub

    Private Sub DisplayDefault() Handles lstClassLimits.DefaultRequested
        lstClassLimits.CurrentValues = DefaultClasses
    End Sub

    Private Function CreateForm() As frmResult
        Dim lDisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        Dim lFrmResult As New frmResult
        lFrmResult.Icon = Me.Icon
        For Each lDataDisplay As atcDataDisplay In lDisplayPlugins
            Dim lMenuText As String = lDataDisplay.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            lFrmResult.mnuAnalysis.DropDownItems().Add(lMenuText, Nothing, New EventHandler(AddressOf lFrmResult.mnuAnalysis_Click))
        Next
        Return lFrmResult
    End Function

    Private Sub SelectDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectDataToolStripMenuItem.Click
        DataGroup = atcDataManager.UserSelectData("Select Data For Analysis", _
                                                  DataGroup, Nothing, True, True, Me.Icon)
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
        Dim lDesc As String = aTS.Attributes.GetValue("Constituent") & " at"
        Dim lStaId As String = aTS.Attributes.GetValue("STAID")
        If lStaId.Length = 0 OrElse lStaId = "0" Then
            Dim lLocation As String = aTS.Attributes.GetValue("Location")
            If IsNumeric(lLocation) Then
                lStaId = " " & lLocation
            Else
                lStaId = ""
            End If
        Else
            lStaId = " " & lStaId
        End If
        lDesc &= lStaId & " " & aTS.Attributes.GetValue("STANAM") & vbCrLf & Space(2)
        Return lDesc
    End Function
End Class
