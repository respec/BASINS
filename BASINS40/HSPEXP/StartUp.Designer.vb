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
        Me.lblPrefixWarning = New System.Windows.Forms.Label
        Me.lblRCH = New System.Windows.Forms.Label
        Me.txtRCH = New System.Windows.Forms.TextBox
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
        Me.chkFecalColiform = New System.Windows.Forms.CheckBox
        Me.chkBODBalance = New System.Windows.Forms.CheckBox
        Me.chkTotalPhosphorus = New System.Windows.Forms.CheckBox
        Me.chkTotalNitrogen = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkExpertStats = New System.Windows.Forms.CheckBox
        Me.btnMakeEXSFile = New System.Windows.Forms.Button
        Me.grpGraphs.SuspendLayout()
        Me.grpConstituentBalance.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdStart.Enabled = False
        Me.cmdStart.Location = New System.Drawing.Point(370, 614)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(75, 23)
        Me.cmdStart.TabIndex = 15
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowse.Location = New System.Drawing.Point(445, 31)
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
        Me.Label1.Size = New System.Drawing.Size(164, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "UCI file for current calibration run:"
        '
        'txtUCIPath
        '
        Me.txtUCIPath.AllowDrop = True
        Me.txtUCIPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUCIPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtUCIPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList
        Me.txtUCIPath.Location = New System.Drawing.Point(15, 33)
        Me.txtUCIPath.Name = "txtUCIPath"
        Me.txtUCIPath.Size = New System.Drawing.Size(424, 20)
        Me.txtUCIPath.TabIndex = 2
        '
        'lblPrefixWarning
        '
        Me.lblPrefixWarning.AutoSize = True
        Me.lblPrefixWarning.Location = New System.Drawing.Point(15, 57)
        Me.lblPrefixWarning.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblPrefixWarning.Name = "lblPrefixWarning"
        Me.lblPrefixWarning.Size = New System.Drawing.Size(393, 13)
        Me.lblPrefixWarning.TabIndex = 5
        Me.lblPrefixWarning.Text = "Note that the output WDM file, HBN file and UCI file name must have same name."
        '
        'lblRCH
        '
        Me.lblRCH.AutoSize = True
        Me.lblRCH.Enabled = False
        Me.lblRCH.Location = New System.Drawing.Point(3, 221)
        Me.lblRCH.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblRCH.Name = "lblRCH"
        Me.lblRCH.Size = New System.Drawing.Size(457, 13)
        Me.lblRCH.TabIndex = 7
        Me.lblRCH.Text = "What is the number of the outlet reach at which you would like area and water bal" & _
            "ance reports?"
        '
        'txtRCH
        '
        Me.txtRCH.BackColor = System.Drawing.SystemColors.Window
        Me.txtRCH.Enabled = False
        Me.txtRCH.Location = New System.Drawing.Point(13, 253)
        Me.txtRCH.Name = "txtRCH"
        Me.txtRCH.Size = New System.Drawing.Size(47, 20)
        Me.txtRCH.TabIndex = 14
        '
        'lblOutReach2
        '
        Me.lblOutReach2.AutoSize = True
        Me.lblOutReach2.Enabled = False
        Me.lblOutReach2.Location = New System.Drawing.Point(71, 251)
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
        Me.cmdEnd.Location = New System.Drawing.Point(451, 614)
        Me.cmdEnd.Name = "cmdEnd"
        Me.cmdEnd.Size = New System.Drawing.Size(75, 23)
        Me.cmdEnd.TabIndex = 19
        Me.cmdEnd.Text = "End"
        Me.cmdEnd.UseVisualStyleBackColor = True
        '
        'chkAreaReports
        '
        Me.chkAreaReports.AutoSize = True
        Me.chkAreaReports.Location = New System.Drawing.Point(15, 177)
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
        Me.chkRunHSPF.Location = New System.Drawing.Point(15, 131)
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
        Me.pnlHighlight.Location = New System.Drawing.Point(7, 249)
        Me.pnlHighlight.Name = "pnlHighlight"
        Me.pnlHighlight.Size = New System.Drawing.Size(58, 28)
        Me.pnlHighlight.TabIndex = 17
        '
        'chkWaterBalance
        '
        Me.chkWaterBalance.AutoSize = True
        Me.chkWaterBalance.Location = New System.Drawing.Point(6, 72)
        Me.chkWaterBalance.Name = "chkWaterBalance"
        Me.chkWaterBalance.Size = New System.Drawing.Size(180, 17)
        Me.chkWaterBalance.TabIndex = 11
        Me.chkWaterBalance.Text = "Produce Water Balance Reports"
        Me.chkWaterBalance.UseVisualStyleBackColor = True
        '
        'chkSedimentBalance
        '
        Me.chkSedimentBalance.AutoSize = True
        Me.chkSedimentBalance.Location = New System.Drawing.Point(6, 95)
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
        Me.grpGraphs.Location = New System.Drawing.Point(15, 200)
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
        Me.grpConstituentBalance.Controls.Add(Me.chkFecalColiform)
        Me.grpConstituentBalance.Controls.Add(Me.chkBODBalance)
        Me.grpConstituentBalance.Controls.Add(Me.chkTotalPhosphorus)
        Me.grpConstituentBalance.Controls.Add(Me.chkTotalNitrogen)
        Me.grpConstituentBalance.Controls.Add(Me.Label3)
        Me.grpConstituentBalance.Controls.Add(Me.chkWaterBalance)
        Me.grpConstituentBalance.Controls.Add(Me.chkSedimentBalance)
        Me.grpConstituentBalance.Controls.Add(Me.lblRCH)
        Me.grpConstituentBalance.Controls.Add(Me.txtRCH)
        Me.grpConstituentBalance.Controls.Add(Me.lblOutReach2)
        Me.grpConstituentBalance.Controls.Add(Me.pnlHighlight)
        Me.grpConstituentBalance.Location = New System.Drawing.Point(18, 292)
        Me.grpConstituentBalance.Name = "grpConstituentBalance"
        Me.grpConstituentBalance.Size = New System.Drawing.Size(508, 293)
        Me.grpConstituentBalance.TabIndex = 29
        Me.grpConstituentBalance.TabStop = False
        Me.grpConstituentBalance.Text = "Constituent Balance Reports"
        '
        'chkFecalColiform
        '
        Me.chkFecalColiform.AutoSize = True
        Me.chkFecalColiform.Location = New System.Drawing.Point(7, 187)
        Me.chkFecalColiform.Name = "chkFecalColiform"
        Me.chkFecalColiform.Size = New System.Drawing.Size(175, 17)
        Me.chkFecalColiform.TabIndex = 34
        Me.chkFecalColiform.Text = "Produce Fecal Coliform Reports"
        Me.chkFecalColiform.UseVisualStyleBackColor = True
        '
        'chkBODBalance
        '
        Me.chkBODBalance.AutoSize = True
        Me.chkBODBalance.Location = New System.Drawing.Point(6, 164)
        Me.chkBODBalance.Name = "chkBODBalance"
        Me.chkBODBalance.Size = New System.Drawing.Size(246, 17)
        Me.chkBODBalance.TabIndex = 33
        Me.chkBODBalance.Text = "Produce BOD Reports (Includes BOD-PQUAL)"
        Me.chkBODBalance.UseVisualStyleBackColor = True
        '
        'chkTotalPhosphorus
        '
        Me.chkTotalPhosphorus.AutoSize = True
        Me.chkTotalPhosphorus.Location = New System.Drawing.Point(6, 141)
        Me.chkTotalPhosphorus.Name = "chkTotalPhosphorus"
        Me.chkTotalPhosphorus.Size = New System.Drawing.Size(333, 17)
        Me.chkTotalPhosphorus.TabIndex = 32
        Me.chkTotalPhosphorus.Text = "Produce Total Phoshporus Balance Reports (Includes Organic P)"
        Me.chkTotalPhosphorus.UseVisualStyleBackColor = True
        '
        'chkTotalNitrogen
        '
        Me.chkTotalNitrogen.AutoSize = True
        Me.chkTotalNitrogen.Location = New System.Drawing.Point(6, 118)
        Me.chkTotalNitrogen.Name = "chkTotalNitrogen"
        Me.chkTotalNitrogen.Size = New System.Drawing.Size(276, 17)
        Me.chkTotalNitrogen.TabIndex = 30
        Me.chkTotalNitrogen.Text = "Produce Total Nitrogen Reports (Includes Organic N)"
        Me.chkTotalNitrogen.UseVisualStyleBackColor = True
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
        'chkExpertStats
        '
        Me.chkExpertStats.AutoSize = True
        Me.chkExpertStats.Location = New System.Drawing.Point(15, 154)
        Me.chkExpertStats.Name = "chkExpertStats"
        Me.chkExpertStats.Size = New System.Drawing.Size(148, 17)
        Me.chkExpertStats.TabIndex = 30
        Me.chkExpertStats.Text = "Calculate Expert Statistics"
        Me.chkExpertStats.UseVisualStyleBackColor = True
        '
        'btnMakeEXSFile
        '
        Me.btnMakeEXSFile.AutoSize = True
        Me.btnMakeEXSFile.Location = New System.Drawing.Point(15, 86)
        Me.btnMakeEXSFile.Name = "btnMakeEXSFile"
        Me.btnMakeEXSFile.Size = New System.Drawing.Size(195, 23)
        Me.btnMakeEXSFile.TabIndex = 31
        Me.btnMakeEXSFile.Text = "Create Basins Specification (EXS) File"
        Me.btnMakeEXSFile.UseVisualStyleBackColor = True
        '
        'StartUp
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdEnd
        Me.ClientSize = New System.Drawing.Size(532, 649)
        Me.Controls.Add(Me.btnMakeEXSFile)
        Me.Controls.Add(Me.chkExpertStats)
        Me.Controls.Add(Me.grpConstituentBalance)
        Me.Controls.Add(Me.grpGraphs)
        Me.Controls.Add(Me.chkRunHSPF)
        Me.Controls.Add(Me.chkAreaReports)
        Me.Controls.Add(Me.cmdEnd)
        Me.Controls.Add(Me.lblPrefixWarning)
        Me.Controls.Add(Me.txtUCIPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.cmdStart)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "StartUp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
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
    Friend WithEvents lblPrefixWarning As System.Windows.Forms.Label
    Friend WithEvents lblRCH As System.Windows.Forms.Label
    Friend WithEvents txtRCH As System.Windows.Forms.TextBox
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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkExpertStats As System.Windows.Forms.CheckBox
    Friend WithEvents chkTotalNitrogen As System.Windows.Forms.CheckBox
    Friend WithEvents chkTotalPhosphorus As System.Windows.Forms.CheckBox
    Friend WithEvents chkBODBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkFecalColiform As System.Windows.Forms.CheckBox
    Friend WithEvents btnMakeEXSFile As System.Windows.Forms.Button

End Class
