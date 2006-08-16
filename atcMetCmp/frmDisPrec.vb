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
    Friend WithEvents panelBottom As System.Windows.Forms.Panel
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
        Me.panelBottom = New System.Windows.Forms.Panel
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
        Me.panelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDailyPrec
        '
        Me.lblDailyPrec.Location = New System.Drawing.Point(19, 18)
        Me.lblDailyPrec.Name = "lblDailyPrec"
        Me.lblDailyPrec.Size = New System.Drawing.Size(355, 19)
        Me.lblDailyPrec.TabIndex = 2
        Me.lblDailyPrec.Text = "Specify Daily Precipitation Timeseries to Disaggregate"
        '
        'lblObsTime
        '
        Me.lblObsTime.Location = New System.Drawing.Point(10, 222)
        Me.lblObsTime.Name = "lblObsTime"
        Me.lblObsTime.Size = New System.Drawing.Size(134, 18)
        Me.lblObsTime.TabIndex = 3
        Me.lblObsTime.Text = "Observation Hour:"
        '
        'txtObsTime
        '
        Me.txtObsTime.Location = New System.Drawing.Point(125, 222)
        Me.txtObsTime.Name = "txtObsTime"
        Me.txtObsTime.Size = New System.Drawing.Size(57, 22)
        Me.txtObsTime.TabIndex = 4
        '
        'panelBottom
        '
        Me.panelBottom.Controls.Add(Me.btnCancel)
        Me.panelBottom.Controls.Add(Me.btnOk)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 328)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(556, 37)
        Me.panelBottom.TabIndex = 16
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(288, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 28)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(182, 0)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(77, 28)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        '
        'btnDailyPrec
        '
        Me.btnDailyPrec.Location = New System.Drawing.Point(10, 46)
        Me.btnDailyPrec.Name = "btnDailyPrec"
        Me.btnDailyPrec.Size = New System.Drawing.Size(67, 23)
        Me.btnDailyPrec.TabIndex = 18
        Me.btnDailyPrec.Text = "Select"
        '
        'txtDailyPrec
        '
        Me.txtDailyPrec.Location = New System.Drawing.Point(86, 46)
        Me.txtDailyPrec.Name = "txtDailyPrec"
        Me.txtDailyPrec.ReadOnly = True
        Me.txtDailyPrec.Size = New System.Drawing.Size(461, 22)
        Me.txtDailyPrec.TabIndex = 19
        '
        'lblHourlyPrec
        '
        Me.lblHourlyPrec.Location = New System.Drawing.Point(10, 92)
        Me.lblHourlyPrec.Name = "lblHourlyPrec"
        Me.lblHourlyPrec.Size = New System.Drawing.Size(278, 19)
        Me.lblHourlyPrec.TabIndex = 21
        Me.lblHourlyPrec.Text = "Specify Hourly Precipitation Timeseries:"
        '
        'btnAddHourlyPrec
        '
        Me.btnAddHourlyPrec.Location = New System.Drawing.Point(10, 138)
        Me.btnAddHourlyPrec.Name = "btnAddHourlyPrec"
        Me.btnAddHourlyPrec.Size = New System.Drawing.Size(67, 24)
        Me.btnAddHourlyPrec.TabIndex = 22
        Me.btnAddHourlyPrec.Text = "Select"
        '
        'lstHourlyPrec
        '
        Me.lstHourlyPrec.Enabled = False
        Me.lstHourlyPrec.ItemHeight = 16
        Me.lstHourlyPrec.Location = New System.Drawing.Point(86, 120)
        Me.lstHourlyPrec.Name = "lstHourlyPrec"
        Me.lstHourlyPrec.Size = New System.Drawing.Size(461, 68)
        Me.lstHourlyPrec.TabIndex = 23
        '
        'lblDataTol
        '
        Me.lblDataTol.Location = New System.Drawing.Point(278, 222)
        Me.lblDataTol.Name = "lblDataTol"
        Me.lblDataTol.Size = New System.Drawing.Size(135, 18)
        Me.lblDataTol.TabIndex = 25
        Me.lblDataTol.Text = "Data Tolerance (%):"
        '
        'txtDataTol
        '
        Me.txtDataTol.Location = New System.Drawing.Point(403, 222)
        Me.txtDataTol.Name = "txtDataTol"
        Me.txtDataTol.Size = New System.Drawing.Size(58, 22)
        Me.txtDataTol.TabIndex = 26
        '
        'lblSummFile
        '
        Me.lblSummFile.Location = New System.Drawing.Point(10, 258)
        Me.lblSummFile.Name = "lblSummFile"
        Me.lblSummFile.Size = New System.Drawing.Size(153, 19)
        Me.lblSummFile.TabIndex = 27
        Me.lblSummFile.Text = "Summary File (optional):"
        '
        'txtSummFile
        '
        Me.txtSummFile.Location = New System.Drawing.Point(86, 286)
        Me.txtSummFile.Name = "txtSummFile"
        Me.txtSummFile.ReadOnly = True
        Me.txtSummFile.Size = New System.Drawing.Size(461, 22)
        Me.txtSummFile.TabIndex = 28
        '
        'btnSummFile
        '
        Me.btnSummFile.Location = New System.Drawing.Point(10, 286)
        Me.btnSummFile.Name = "btnSummFile"
        Me.btnSummFile.Size = New System.Drawing.Size(67, 23)
        Me.btnSummFile.TabIndex = 29
        Me.btnSummFile.Text = "Select"
        '
        'frmDisPrec
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(556, 365)
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
        Me.Controls.Add(Me.panelBottom)
        Me.Controls.Add(Me.lblObsTime)
        Me.Controls.Add(Me.lblDailyPrec)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDisPrec"
        Me.Text = "Disaggregate Precipitation"
        Me.panelBottom.ResumeLayout(False)
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
