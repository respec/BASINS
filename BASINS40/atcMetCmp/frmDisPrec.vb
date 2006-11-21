Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmDisPrec
    Inherits System.Windows.Forms.Form

    Private pOk As Boolean
    Private pDPrecTS As atcTimeseries
    Private pHPrecTS As atcDataGroup
    Private pDataManager As atcDataManager
    Private cObsTime As Integer
    Private cDataTol As Double

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblObsTime As System.Windows.Forms.Label
    Friend WithEvents txtObsTime As System.Windows.Forms.TextBox
    Friend WithEvents lblDailyPrec As System.Windows.Forms.Label
    Friend WithEvents btnDailyPrec As System.Windows.Forms.Button
    Friend WithEvents txtDailyPrec As System.Windows.Forms.TextBox
    Friend WithEvents lblHourlyPrec As System.Windows.Forms.Label
    Friend WithEvents btnAddHourlyPrec As System.Windows.Forms.Button
    Friend WithEvents lstHourlyPrec As System.Windows.Forms.ListBox
    Friend WithEvents lblDataTol As System.Windows.Forms.Label
    Friend WithEvents txtDataTol As System.Windows.Forms.TextBox
    Friend WithEvents lblSummFile As System.Windows.Forms.Label
    Friend WithEvents txtSummFile As System.Windows.Forms.TextBox
    Friend WithEvents btnSummFile As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDisPrec))
        Me.lblDailyPrec = New System.Windows.Forms.Label
        Me.lblObsTime = New System.Windows.Forms.Label
        Me.txtObsTime = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnDailyPrec = New System.Windows.Forms.Button
        Me.txtDailyPrec = New System.Windows.Forms.TextBox
        Me.lblHourlyPrec = New System.Windows.Forms.Label
        Me.btnAddHourlyPrec = New System.Windows.Forms.Button
        Me.lstHourlyPrec = New System.Windows.Forms.ListBox
        Me.lblDataTol = New System.Windows.Forms.Label
        Me.txtDataTol = New System.Windows.Forms.TextBox
        Me.lblSummFile = New System.Windows.Forms.Label
        Me.txtSummFile = New System.Windows.Forms.TextBox
        Me.btnSummFile = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblDailyPrec
        '
        Me.lblDailyPrec.AutoSize = True
        Me.lblDailyPrec.Location = New System.Drawing.Point(12, 9)
        Me.lblDailyPrec.Name = "lblDailyPrec"
        Me.lblDailyPrec.Size = New System.Drawing.Size(260, 13)
        Me.lblDailyPrec.TabIndex = 2
        Me.lblDailyPrec.Text = "Specify Daily Precipitation Timeseries to Disaggregate"
        '
        'lblObsTime
        '
        Me.lblObsTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblObsTime.AutoSize = True
        Me.lblObsTime.Location = New System.Drawing.Point(12, 218)
        Me.lblObsTime.Name = "lblObsTime"
        Me.lblObsTime.Size = New System.Drawing.Size(93, 13)
        Me.lblObsTime.TabIndex = 8
        Me.lblObsTime.Text = "Observation Hour:"
        '
        'txtObsTime
        '
        Me.txtObsTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtObsTime.Location = New System.Drawing.Point(111, 215)
        Me.txtObsTime.Name = "txtObsTime"
        Me.txtObsTime.Size = New System.Drawing.Size(48, 20)
        Me.txtObsTime.TabIndex = 9
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(265, 306)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(12, 306)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(64, 24)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        '
        'btnDailyPrec
        '
        Me.btnDailyPrec.Location = New System.Drawing.Point(12, 40)
        Me.btnDailyPrec.Name = "btnDailyPrec"
        Me.btnDailyPrec.Size = New System.Drawing.Size(56, 20)
        Me.btnDailyPrec.TabIndex = 3
        Me.btnDailyPrec.Text = "Select"
        '
        'txtDailyPrec
        '
        Me.txtDailyPrec.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDailyPrec.Location = New System.Drawing.Point(74, 40)
        Me.txtDailyPrec.Name = "txtDailyPrec"
        Me.txtDailyPrec.ReadOnly = True
        Me.txtDailyPrec.Size = New System.Drawing.Size(255, 20)
        Me.txtDailyPrec.TabIndex = 4
        '
        'lblHourlyPrec
        '
        Me.lblHourlyPrec.AutoSize = True
        Me.lblHourlyPrec.Location = New System.Drawing.Point(12, 88)
        Me.lblHourlyPrec.Name = "lblHourlyPrec"
        Me.lblHourlyPrec.Size = New System.Drawing.Size(192, 13)
        Me.lblHourlyPrec.TabIndex = 5
        Me.lblHourlyPrec.Text = "Specify Hourly Precipitation Timeseries:"
        '
        'btnAddHourlyPrec
        '
        Me.btnAddHourlyPrec.Location = New System.Drawing.Point(12, 104)
        Me.btnAddHourlyPrec.Name = "btnAddHourlyPrec"
        Me.btnAddHourlyPrec.Size = New System.Drawing.Size(56, 20)
        Me.btnAddHourlyPrec.TabIndex = 6
        Me.btnAddHourlyPrec.Text = "Select"
        '
        'lstHourlyPrec
        '
        Me.lstHourlyPrec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstHourlyPrec.Enabled = False
        Me.lstHourlyPrec.IntegralHeight = False
        Me.lstHourlyPrec.Location = New System.Drawing.Point(72, 104)
        Me.lstHourlyPrec.Name = "lstHourlyPrec"
        Me.lstHourlyPrec.Size = New System.Drawing.Size(257, 105)
        Me.lstHourlyPrec.TabIndex = 7
        '
        'lblDataTol
        '
        Me.lblDataTol.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataTol.AutoSize = True
        Me.lblDataTol.Location = New System.Drawing.Point(174, 218)
        Me.lblDataTol.Name = "lblDataTol"
        Me.lblDataTol.Size = New System.Drawing.Size(101, 13)
        Me.lblDataTol.TabIndex = 10
        Me.lblDataTol.Text = "Data Tolerance (%):"
        '
        'txtDataTol
        '
        Me.txtDataTol.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataTol.Location = New System.Drawing.Point(281, 215)
        Me.txtDataTol.Name = "txtDataTol"
        Me.txtDataTol.Size = New System.Drawing.Size(48, 20)
        Me.txtDataTol.TabIndex = 11
        '
        'lblSummFile
        '
        Me.lblSummFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSummFile.AutoSize = True
        Me.lblSummFile.Location = New System.Drawing.Point(12, 255)
        Me.lblSummFile.Name = "lblSummFile"
        Me.lblSummFile.Size = New System.Drawing.Size(118, 13)
        Me.lblSummFile.TabIndex = 12
        Me.lblSummFile.Text = "Summary File (optional):"
        '
        'txtSummFile
        '
        Me.txtSummFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSummFile.Location = New System.Drawing.Point(74, 271)
        Me.txtSummFile.Name = "txtSummFile"
        Me.txtSummFile.ReadOnly = True
        Me.txtSummFile.Size = New System.Drawing.Size(255, 20)
        Me.txtSummFile.TabIndex = 14
        '
        'btnSummFile
        '
        Me.btnSummFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSummFile.Location = New System.Drawing.Point(12, 271)
        Me.btnSummFile.Name = "btnSummFile"
        Me.btnSummFile.Size = New System.Drawing.Size(56, 20)
        Me.btnSummFile.TabIndex = 13
        Me.btnSummFile.Text = "Select"
        '
        'frmDisPrec
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(341, 342)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnSummFile)
        Me.Controls.Add(Me.txtSummFile)
        Me.Controls.Add(Me.txtDataTol)
        Me.Controls.Add(Me.txtDailyPrec)
        Me.Controls.Add(Me.txtObsTime)
        Me.Controls.Add(Me.lblSummFile)
        Me.Controls.Add(Me.lblDataTol)
        Me.Controls.Add(Me.lstHourlyPrec)
        Me.Controls.Add(Me.btnAddHourlyPrec)
        Me.Controls.Add(Me.lblHourlyPrec)
        Me.Controls.Add(Me.btnDailyPrec)
        Me.Controls.Add(Me.lblObsTime)
        Me.Controls.Add(Me.lblDailyPrec)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisPrec"
        Me.Text = "Disaggregate Precipitation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public Function AskUser(ByVal aDataManager As atcDataManager, ByRef aDPrecTS As atcTimeseries, ByRef aHPrecTS As atcDataGroup, ByRef aObsTime As Integer, ByRef aDataTol As Double, ByRef aSummFile As String) As Boolean
        pDataManager = aDataManager
        Me.ShowDialog()
        If pOk Then
            aDPrecTS = pDPrecTS
            aHPrecTS = pHPrecTS
            aObsTime = cObsTime
            aDataTol = cDataTol
            aSummFile = txtSummFile.Text
        End If
        Return pOk
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not pDPrecTS Is Nothing Then
            If Not pHPrecTS Is Nothing AndAlso pHPrecTS.Count > 0 Then
                If IsNumeric(txtObsTime.Text) And IsNumeric(txtDataTol.Text) Then
                    cObsTime = CInt(txtObsTime.Text)
                    cDataTol = CDbl(txtDataTol.Text)
                    If cObsTime >= 1 And cObsTime <= 24 Then
                        If cDataTol >= 0 And cDataTol <= 100 Then
                            pOk = True
                            Close()
                        Else
                            Logger.Msg("Value for 'Data Tolerance' must be between 0 and 100" & vbCrLf & _
                                       "Value specified is '" & cDataTol & "'", "Disaggregate Daily Precipitation Problem")
                        End If
                    Else
                        Logger.Msg("Value for 'Observation Hour' must be between 1 and 24" & vbCrLf & _
                                   "Value specified is '" & cObsTime & "'", Me.Text & " Problem")
                    End If
                Else
                    Logger.Msg("Values must be specified for 'Observation Hour' and 'Data Tolerance'." & vbCrLf & _
                               "At least one of these values is currently not numeric.", Me.Text & " Problem")
                End If
            Else
                Logger.Msg("No 'Hourly Precipitation Timeseries' selected." & vbCrLf & _
                           "Use 'Select' button to specify the timeseries.", Me.Text & " Problem")
            End If
        Else
            Logger.Msg("No 'Daily Precipitation Timeseries to Disaggregate' selected." & vbCrLf & _
                       "Use 'Select' button to specify the timeseries.", Me.Text & " Problem")
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnDailyPrec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDailyPrec.Click
        Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select Daily Precipitation Timeseries to Disaggregate")
        If lTSGroup.Count > 0 Then
            pDPrecTS = lTSGroup(0)
            txtDailyPrec.Text = pDPrecTS.ToString
        End If
    End Sub

    Private Sub btnHourlyPrec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddHourlyPrec.Click
        Dim lTSGroup As atcDataGroup = pDataManager.UserSelectData("Select Hourly Precipitation Timeseries to use in Disaggregation")
        If lTSGroup.Count > 0 Then
            pHPrecTS = lTSGroup
            lstHourlyPrec.Items.Clear()
            For i As Integer = 0 To lTSGroup.Count - 1
                lstHourlyPrec.Items.Add(pHPrecTS(i).ToString)
            Next
        End If
    End Sub

    Private Sub btnSummFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummFile.Click
        Dim cdlg As New Windows.Forms.SaveFileDialog
        With cdlg
            .Title = "Summary Output File"
            .FileName = ""
            .Filter = "Summary Files (*.sum)|*.sum|All Files (*.*)|(*.*)"
            .OverwritePrompt = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtSummFile.Text = .FileName
            End If
        End With

    End Sub

    Private Sub frmDisPrec_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Compute\Disaggregations.html")
        End If
    End Sub
End Class
