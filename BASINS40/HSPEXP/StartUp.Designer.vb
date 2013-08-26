<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartUp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartUp))
        Me.cmdStart = New System.Windows.Forms.Button
        Me.cmdBrowse = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtUCIPath = New System.Windows.Forms.TextBox
        Me.lblPrefix = New System.Windows.Forms.Label
        Me.lblPrefixWarning = New System.Windows.Forms.Label
        Me.txtPrefix = New System.Windows.Forms.TextBox
        Me.lblRCH = New System.Windows.Forms.Label
        Me.txtRCH = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtRunNo = New System.Windows.Forms.TextBox
        Me.lblRunNo = New System.Windows.Forms.Label
        Me.lblRunNoInfo = New System.Windows.Forms.Label
        Me.txtRootPath = New System.Windows.Forms.TextBox
        Me.lblRootDirectory = New System.Windows.Forms.Label
        Me.lblOutReach2 = New System.Windows.Forms.Label
        Me.cmdEnd = New System.Windows.Forms.Button
        Me.chkAreaReports = New System.Windows.Forms.CheckBox
        Me.chkGraphStandard = New System.Windows.Forms.CheckBox
        Me.chkLogGraphs = New System.Windows.Forms.CheckBox
        Me.chkSupportingGraphs = New System.Windows.Forms.CheckBox
        Me.chkRunHSPF = New System.Windows.Forms.CheckBox
        Me.pnlHighlight = New System.Windows.Forms.Panel
        Me.chkWaterBalance = New System.Windows.Forms.CheckBox
        Me.chkSedimentBalance = New System.Windows.Forms.CheckBox
        Me.grpGraphs = New System.Windows.Forms.GroupBox
        Me.grpConstituentBalance = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkNitrogenBalance = New System.Windows.Forms.CheckBox
        Me.chkExpertStats = New System.Windows.Forms.CheckBox
        Me.grpGraphs.SuspendLayout()
        Me.grpConstituentBalance.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdStart.Enabled = False
        Me.cmdStart.Location = New System.Drawing.Point(364, 638)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(75, 23)
        Me.cmdStart.TabIndex = 15
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowse.Location = New System.Drawing.Point(445, 4)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(75, 23)
        Me.cmdBrowse.TabIndex = 1
        Me.cmdBrowse.Text = "Browse"
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(392, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Please specify the path to your UCI and WDM files for your current calibration ru" & _
            "n:"
        '
        'txtUCIPath
        '
        Me.txtUCIPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUCIPath.Enabled = False
        Me.txtUCIPath.Location = New System.Drawing.Point(15, 33)
        Me.txtUCIPath.Name = "txtUCIPath"
        Me.txtUCIPath.Size = New System.Drawing.Size(505, 20)
        Me.txtUCIPath.TabIndex = 2
        '
        'lblPrefix
        '
        Me.lblPrefix.AutoSize = True
        Me.lblPrefix.Location = New System.Drawing.Point(12, 94)
        Me.lblPrefix.Name = "lblPrefix"
        Me.lblPrefix.Size = New System.Drawing.Size(58, 13)
        Me.lblPrefix.TabIndex = 4
        Me.lblPrefix.Text = "File Prefix: "
        '
        'lblPrefixWarning
        '
        Me.lblPrefixWarning.AutoSize = True
        Me.lblPrefixWarning.Location = New System.Drawing.Point(12, 116)
        Me.lblPrefixWarning.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblPrefixWarning.Name = "lblPrefixWarning"
        Me.lblPrefixWarning.Size = New System.Drawing.Size(489, 26)
        Me.lblPrefixWarning.TabIndex = 5
        Me.lblPrefixWarning.Text = "Note that this file prefix MUST be used to name your UCI, WDM, and EXS files.  If" & _
            " this is not the case, change your file names NOW before proceeding."
        '
        'txtPrefix
        '
        Me.txtPrefix.Location = New System.Drawing.Point(64, 91)
        Me.txtPrefix.Name = "txtPrefix"
        Me.txtPrefix.Size = New System.Drawing.Size(110, 20)
        Me.txtPrefix.TabIndex = 3
        '
        'lblRCH
        '
        Me.lblRCH.AutoSize = True
        Me.lblRCH.Enabled = False
        Me.lblRCH.Location = New System.Drawing.Point(0, 169)
        Me.lblRCH.MaximumSize = New System.Drawing.Size(190, 0)
        Me.lblRCH.Name = "lblRCH"
        Me.lblRCH.Size = New System.Drawing.Size(188, 39)
        Me.lblRCH.TabIndex = 7
        Me.lblRCH.Text = "What is the number of the outlet reach at which you would like area and water bal" & _
            "ance reports?"
        '
        'txtRCH
        '
        Me.txtRCH.BackColor = System.Drawing.SystemColors.Window
        Me.txtRCH.Enabled = False
        Me.txtRCH.Location = New System.Drawing.Point(200, 184)
        Me.txtRCH.Name = "txtRCH"
        Me.txtRCH.Size = New System.Drawing.Size(47, 20)
        Me.txtRCH.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 54)
        Me.Label2.MaximumSize = New System.Drawing.Size(500, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(491, 26)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'txtRunNo
        '
        Me.txtRunNo.Location = New System.Drawing.Point(134, 151)
        Me.txtRunNo.Name = "txtRunNo"
        Me.txtRunNo.Size = New System.Drawing.Size(40, 20)
        Me.txtRunNo.TabIndex = 4
        '
        'lblRunNo
        '
        Me.lblRunNo.AutoSize = True
        Me.lblRunNo.Location = New System.Drawing.Point(12, 154)
        Me.lblRunNo.Name = "lblRunNo"
        Me.lblRunNo.Size = New System.Drawing.Size(116, 13)
        Me.lblRunNo.TabIndex = 12
        Me.lblRunNo.Text = "Run Number (optional):"
        '
        'lblRunNoInfo
        '
        Me.lblRunNoInfo.AutoSize = True
        Me.lblRunNoInfo.Location = New System.Drawing.Point(10, 213)
        Me.lblRunNoInfo.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblRunNoInfo.Name = "lblRunNoInfo"
        Me.lblRunNoInfo.Size = New System.Drawing.Size(476, 26)
        Me.lblRunNoInfo.TabIndex = 14
        Me.lblRunNoInfo.Text = "The Run Number and Root Directory are not required, but will facilitate copying o" & _
            "f the files to a new folder for your next run."
        '
        'txtRootPath
        '
        Me.txtRootPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRootPath.Location = New System.Drawing.Point(293, 151)
        Me.txtRootPath.Name = "txtRootPath"
        Me.txtRootPath.Size = New System.Drawing.Size(227, 20)
        Me.txtRootPath.TabIndex = 5
        '
        'lblRootDirectory
        '
        Me.lblRootDirectory.AutoSize = True
        Me.lblRootDirectory.Location = New System.Drawing.Point(209, 154)
        Me.lblRootDirectory.Name = "lblRootDirectory"
        Me.lblRootDirectory.Size = New System.Drawing.Size(78, 13)
        Me.lblRootDirectory.TabIndex = 15
        Me.lblRootDirectory.Text = "Root Directory:"
        '
        'lblOutReach2
        '
        Me.lblOutReach2.AutoSize = True
        Me.lblOutReach2.Enabled = False
        Me.lblOutReach2.Location = New System.Drawing.Point(258, 182)
        Me.lblOutReach2.MaximumSize = New System.Drawing.Size(250, 0)
        Me.lblOutReach2.Name = "lblOutReach2"
        Me.lblOutReach2.Size = New System.Drawing.Size(245, 26)
        Me.lblOutReach2.TabIndex = 18
        Me.lblOutReach2.Text = "NOTE: You can enter multiple reaches, separated by a comma - e.g.: 5, 10"
        '
        'cmdEnd
        '
        Me.cmdEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdEnd.Location = New System.Drawing.Point(445, 638)
        Me.cmdEnd.Name = "cmdEnd"
        Me.cmdEnd.Size = New System.Drawing.Size(75, 23)
        Me.cmdEnd.TabIndex = 19
        Me.cmdEnd.Text = "End"
        Me.cmdEnd.UseVisualStyleBackColor = True
        '
        'chkAreaReports
        '
        Me.chkAreaReports.AutoSize = True
        Me.chkAreaReports.Enabled = False
        Me.chkAreaReports.Location = New System.Drawing.Point(15, 275)
        Me.chkAreaReports.Name = "chkAreaReports"
        Me.chkAreaReports.Size = New System.Drawing.Size(500, 17)
        Me.chkAreaReports.TabIndex = 7
        Me.chkAreaReports.Text = "Produce land use and watershed area reports. You may need these reports to verify" & _
            " the model setup."
        Me.chkAreaReports.UseVisualStyleBackColor = True
        '
        'chkGraphStandard
        '
        Me.chkGraphStandard.AutoSize = True
        Me.chkGraphStandard.Location = New System.Drawing.Point(6, 19)
        Me.chkGraphStandard.Name = "chkGraphStandard"
        Me.chkGraphStandard.Size = New System.Drawing.Size(373, 17)
        Me.chkGraphStandard.TabIndex = 8
        Me.chkGraphStandard.Text = "Produce standard monthly flow, daily flow, storm, and flow duration graphs"
        Me.chkGraphStandard.UseVisualStyleBackColor = True
        '
        'chkLogGraphs
        '
        Me.chkLogGraphs.AutoSize = True
        Me.chkLogGraphs.Enabled = False
        Me.chkLogGraphs.Location = New System.Drawing.Point(6, 65)
        Me.chkLogGraphs.Name = "chkLogGraphs"
        Me.chkLogGraphs.Size = New System.Drawing.Size(470, 17)
        Me.chkLogGraphs.TabIndex = 10
        Me.chkLogGraphs.Text = "Produce logarithmic graphs (requires standard and/or supporting graph box(es) to " & _
            "be checked)"
        Me.chkLogGraphs.UseVisualStyleBackColor = True
        '
        'chkSupportingGraphs
        '
        Me.chkSupportingGraphs.AutoSize = True
        Me.chkSupportingGraphs.Location = New System.Drawing.Point(6, 42)
        Me.chkSupportingGraphs.Name = "chkSupportingGraphs"
        Me.chkSupportingGraphs.Size = New System.Drawing.Size(180, 17)
        Me.chkSupportingGraphs.TabIndex = 9
        Me.chkSupportingGraphs.Text = "Produce other supporting graphs"
        Me.chkSupportingGraphs.UseVisualStyleBackColor = True
        '
        'chkRunHSPF
        '
        Me.chkRunHSPF.AutoSize = True
        Me.chkRunHSPF.Location = New System.Drawing.Point(15, 184)
        Me.chkRunHSPF.Name = "chkRunHSPF"
        Me.chkRunHSPF.Size = New System.Drawing.Size(253, 17)
        Me.chkRunHSPF.TabIndex = 6
        Me.chkRunHSPF.Text = "Run WinHSPFLt before calculating the statistics"
        Me.chkRunHSPF.UseVisualStyleBackColor = True
        '
        'pnlHighlight
        '
        Me.pnlHighlight.BackColor = System.Drawing.Color.Red
        Me.pnlHighlight.Enabled = False
        Me.pnlHighlight.Location = New System.Drawing.Point(194, 180)
        Me.pnlHighlight.Name = "pnlHighlight"
        Me.pnlHighlight.Size = New System.Drawing.Size(58, 28)
        Me.pnlHighlight.TabIndex = 17
        '
        'chkWaterBalance
        '
        Me.chkWaterBalance.AutoSize = True
        Me.chkWaterBalance.Location = New System.Drawing.Point(6, 76)
        Me.chkWaterBalance.Name = "chkWaterBalance"
        Me.chkWaterBalance.Size = New System.Drawing.Size(180, 17)
        Me.chkWaterBalance.TabIndex = 11
        Me.chkWaterBalance.Text = "Produce Water Balance Reports"
        Me.chkWaterBalance.UseVisualStyleBackColor = True
        '
        'chkSedimentBalance
        '
        Me.chkSedimentBalance.AutoSize = True
        Me.chkSedimentBalance.Location = New System.Drawing.Point(6, 99)
        Me.chkSedimentBalance.Name = "chkSedimentBalance"
        Me.chkSedimentBalance.Size = New System.Drawing.Size(195, 17)
        Me.chkSedimentBalance.TabIndex = 12
        Me.chkSedimentBalance.Text = "Produce Sediment Balance Reports"
        Me.chkSedimentBalance.UseVisualStyleBackColor = True
        '
        'grpGraphs
        '
        Me.grpGraphs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpGraphs.Controls.Add(Me.chkGraphStandard)
        Me.grpGraphs.Controls.Add(Me.chkLogGraphs)
        Me.grpGraphs.Controls.Add(Me.chkSupportingGraphs)
        Me.grpGraphs.Enabled = False
        Me.grpGraphs.Location = New System.Drawing.Point(12, 298)
        Me.grpGraphs.Name = "grpGraphs"
        Me.grpGraphs.Size = New System.Drawing.Size(508, 86)
        Me.grpGraphs.TabIndex = 28
        Me.grpGraphs.TabStop = False
        Me.grpGraphs.Text = "Graphs"
        '
        'grpConstituentBalance
        '
        Me.grpConstituentBalance.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpConstituentBalance.Controls.Add(Me.Label3)
        Me.grpConstituentBalance.Controls.Add(Me.chkNitrogenBalance)
        Me.grpConstituentBalance.Controls.Add(Me.chkWaterBalance)
        Me.grpConstituentBalance.Controls.Add(Me.chkSedimentBalance)
        Me.grpConstituentBalance.Controls.Add(Me.lblRCH)
        Me.grpConstituentBalance.Controls.Add(Me.txtRCH)
        Me.grpConstituentBalance.Controls.Add(Me.lblOutReach2)
        Me.grpConstituentBalance.Controls.Add(Me.pnlHighlight)
        Me.grpConstituentBalance.Location = New System.Drawing.Point(12, 399)
        Me.grpConstituentBalance.Name = "grpConstituentBalance"
        Me.grpConstituentBalance.Size = New System.Drawing.Size(508, 225)
        Me.grpConstituentBalance.TabIndex = 29
        Me.grpConstituentBalance.TabStop = False
        Me.grpConstituentBalance.Text = "Constituent Balance Reports"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(6, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(496, 48)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Make sure that the Binary File (HBN) file is produced with the same name as the u" & _
            "ci file. And that the time interval for Binary file has been set up to Monthly."
        '
        'chkNitrogenBalance
        '
        Me.chkNitrogenBalance.AutoSize = True
        Me.chkNitrogenBalance.Enabled = False
        Me.chkNitrogenBalance.Location = New System.Drawing.Point(6, 122)
        Me.chkNitrogenBalance.Name = "chkNitrogenBalance"
        Me.chkNitrogenBalance.Size = New System.Drawing.Size(273, 17)
        Me.chkNitrogenBalance.TabIndex = 13
        Me.chkNitrogenBalance.Text = "Produce Nitrogen Balance Reports (Unimplemented)"
        Me.chkNitrogenBalance.UseVisualStyleBackColor = True
        '
        'chkExpertStats
        '
        Me.chkExpertStats.AutoSize = True
        Me.chkExpertStats.Location = New System.Drawing.Point(15, 252)
        Me.chkExpertStats.Name = "chkExpertStats"
        Me.chkExpertStats.Size = New System.Drawing.Size(148, 17)
        Me.chkExpertStats.TabIndex = 30
        Me.chkExpertStats.Text = "Calculate Expert Statistics"
        Me.chkExpertStats.UseVisualStyleBackColor = True
        '
        'StartUp
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdEnd
        Me.ClientSize = New System.Drawing.Size(532, 673)
        Me.Controls.Add(Me.chkExpertStats)
        Me.Controls.Add(Me.grpConstituentBalance)
        Me.Controls.Add(Me.grpGraphs)
        Me.Controls.Add(Me.chkRunHSPF)
        Me.Controls.Add(Me.chkAreaReports)
        Me.Controls.Add(Me.cmdEnd)
        Me.Controls.Add(Me.txtRootPath)
        Me.Controls.Add(Me.lblRootDirectory)
        Me.Controls.Add(Me.lblRunNoInfo)
        Me.Controls.Add(Me.txtRunNo)
        Me.Controls.Add(Me.lblRunNo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPrefix)
        Me.Controls.Add(Me.lblPrefixWarning)
        Me.Controls.Add(Me.lblPrefix)
        Me.Controls.Add(Me.txtUCIPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.cmdStart)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "StartUp"
        Me.Text = "HSPEXP+ (Developed by AQUA TERRA Consultants and BSE, VT)"
        Me.grpGraphs.ResumeLayout(False)
        Me.grpGraphs.PerformLayout()
        Me.grpConstituentBalance.ResumeLayout(False)
        Me.grpConstituentBalance.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUCIPath As System.Windows.Forms.TextBox
    Friend WithEvents lblPrefix As System.Windows.Forms.Label
    Friend WithEvents lblPrefixWarning As System.Windows.Forms.Label
    Friend WithEvents txtPrefix As System.Windows.Forms.TextBox
    Friend WithEvents lblRCH As System.Windows.Forms.Label
    Friend WithEvents txtRCH As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRunNo As System.Windows.Forms.TextBox
    Friend WithEvents lblRunNo As System.Windows.Forms.Label
    Friend WithEvents lblRunNoInfo As System.Windows.Forms.Label
    Friend WithEvents txtRootPath As System.Windows.Forms.TextBox
    Friend WithEvents lblRootDirectory As System.Windows.Forms.Label
    Friend WithEvents lblOutReach2 As System.Windows.Forms.Label
    Friend WithEvents cmdEnd As System.Windows.Forms.Button
    Friend WithEvents chkAreaReports As System.Windows.Forms.CheckBox
    Friend WithEvents chkGraphStandard As System.Windows.Forms.CheckBox
    Friend WithEvents chkLogGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkSupportingGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkRunHSPF As System.Windows.Forms.CheckBox
    Friend WithEvents pnlHighlight As System.Windows.Forms.Panel
    Friend WithEvents chkWaterBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkSedimentBalance As System.Windows.Forms.CheckBox
    Friend WithEvents grpGraphs As System.Windows.Forms.GroupBox
    Friend WithEvents grpConstituentBalance As System.Windows.Forms.GroupBox
    Friend WithEvents chkNitrogenBalance As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkExpertStats As System.Windows.Forms.CheckBox

End Class
