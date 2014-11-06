﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUCIPath = New System.Windows.Forms.TextBox()
        Me.lblRCH = New System.Windows.Forms.Label()
        Me.txtRCH = New System.Windows.Forms.TextBox()
        Me.lblOutReach2 = New System.Windows.Forms.Label()
        Me.cmdEnd = New System.Windows.Forms.Button()
        Me.chkAreaReports = New System.Windows.Forms.CheckBox()
        Me.chkGraphStandard = New System.Windows.Forms.CheckBox()
        Me.chkRunHSPF = New System.Windows.Forms.CheckBox()
        Me.pnlHighlight = New System.Windows.Forms.Panel()
        Me.chkWaterBalance = New System.Windows.Forms.CheckBox()
        Me.chkSedimentBalance = New System.Windows.Forms.CheckBox()
        Me.chkFecalColiform = New System.Windows.Forms.CheckBox()
        Me.chkBODBalance = New System.Windows.Forms.CheckBox()
        Me.chkTotalPhosphorus = New System.Windows.Forms.CheckBox()
        Me.chkTotalNitrogen = New System.Windows.Forms.CheckBox()
        Me.chkExpertStats = New System.Windows.Forms.CheckBox()
        Me.btnMakeEXSFile = New System.Windows.Forms.Button()
        Me.chkAdditionalgraphs = New System.Windows.Forms.CheckBox()
        Me.btn_help = New System.Windows.Forms.Button()
        Me.chkHydrologySensitivity = New System.Windows.Forms.CheckBox()
        Me.chkWaterQualitySensitivity = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdStart.Enabled = False
        Me.cmdStart.Location = New System.Drawing.Point(296, 529)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(75, 23)
        Me.cmdStart.TabIndex = 22
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowse.Location = New System.Drawing.Point(371, 52)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(75, 23)
        Me.cmdBrowse.TabIndex = 1
        Me.cmdBrowse.Text = "Browse"
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Browse to the model UCI file"
        '
        'txtUCIPath
        '
        Me.txtUCIPath.AllowDrop = True
        Me.txtUCIPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUCIPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtUCIPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList
        Me.txtUCIPath.Location = New System.Drawing.Point(20, 54)
        Me.txtUCIPath.Name = "txtUCIPath"
        Me.txtUCIPath.Size = New System.Drawing.Size(345, 20)
        Me.txtUCIPath.TabIndex = 2
        '
        'lblRCH
        '
        Me.lblRCH.AutoSize = True
        Me.lblRCH.Enabled = False
        Me.lblRCH.Location = New System.Drawing.Point(17, 478)
        Me.lblRCH.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblRCH.Name = "lblRCH"
        Me.lblRCH.Size = New System.Drawing.Size(441, 13)
        Me.lblRCH.TabIndex = 7
        Me.lblRCH.Text = "Number(s) of the outlet reach at which you would like water and/or nutrient balan" & _
            "ce reports?"
        '
        'txtRCH
        '
        Me.txtRCH.BackColor = System.Drawing.SystemColors.Window
        Me.txtRCH.Enabled = False
        Me.txtRCH.Location = New System.Drawing.Point(22, 502)
        Me.txtRCH.Name = "txtRCH"
        Me.txtRCH.Size = New System.Drawing.Size(47, 20)
        Me.txtRCH.TabIndex = 21
        '
        'lblOutReach2
        '
        Me.lblOutReach2.AutoSize = True
        Me.lblOutReach2.Enabled = False
        Me.lblOutReach2.Location = New System.Drawing.Point(85, 510)
        Me.lblOutReach2.MaximumSize = New System.Drawing.Size(400, 0)
        Me.lblOutReach2.Name = "lblOutReach2"
        Me.lblOutReach2.Size = New System.Drawing.Size(359, 13)
        Me.lblOutReach2.TabIndex = 18
        Me.lblOutReach2.Text = "NOTE: You can enter multiple reaches, separated by a comma - e.g.: 5, 10"
        '
        'cmdEnd
        '
        Me.cmdEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdEnd.Location = New System.Drawing.Point(377, 529)
        Me.cmdEnd.Name = "cmdEnd"
        Me.cmdEnd.Size = New System.Drawing.Size(75, 23)
        Me.cmdEnd.TabIndex = 23
        Me.cmdEnd.Text = "End"
        Me.cmdEnd.UseVisualStyleBackColor = True
        '
        'chkAreaReports
        '
        Me.chkAreaReports.AutoSize = True
        Me.chkAreaReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAreaReports.Location = New System.Drawing.Point(20, 146)
        Me.chkAreaReports.Name = "chkAreaReports"
        Me.chkAreaReports.Size = New System.Drawing.Size(143, 17)
        Me.chkAreaReports.TabIndex = 7
        Me.chkAreaReports.Text = "Watershed Area Reports"
        Me.chkAreaReports.UseVisualStyleBackColor = True
        '
        'chkGraphStandard
        '
        Me.chkGraphStandard.AutoSize = True
        Me.chkGraphStandard.Location = New System.Drawing.Point(6, 65)
        Me.chkGraphStandard.Name = "chkGraphStandard"
        Me.chkGraphStandard.Size = New System.Drawing.Size(162, 17)
        Me.chkGraphStandard.TabIndex = 11
        Me.chkGraphStandard.Text = "Hydrology Calibration Graphs"
        Me.chkGraphStandard.UseVisualStyleBackColor = True
        '
        'chkRunHSPF
        '
        Me.chkRunHSPF.AutoSize = True
        Me.chkRunHSPF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunHSPF.Location = New System.Drawing.Point(20, 123)
        Me.chkRunHSPF.Name = "chkRunHSPF"
        Me.chkRunHSPF.Size = New System.Drawing.Size(77, 17)
        Me.chkRunHSPF.TabIndex = 6
        Me.chkRunHSPF.Text = "Run HSPF"
        Me.chkRunHSPF.UseVisualStyleBackColor = True
        '
        'pnlHighlight
        '
        Me.pnlHighlight.BackColor = System.Drawing.Color.Red
        Me.pnlHighlight.Enabled = False
        Me.pnlHighlight.Location = New System.Drawing.Point(16, 498)
        Me.pnlHighlight.Name = "pnlHighlight"
        Me.pnlHighlight.Size = New System.Drawing.Size(58, 28)
        Me.pnlHighlight.TabIndex = 17
        '
        'chkWaterBalance
        '
        Me.chkWaterBalance.AutoSize = True
        Me.chkWaterBalance.Location = New System.Drawing.Point(6, 88)
        Me.chkWaterBalance.Name = "chkWaterBalance"
        Me.chkWaterBalance.Size = New System.Drawing.Size(137, 17)
        Me.chkWaterBalance.TabIndex = 10
        Me.chkWaterBalance.Text = "Water Balance Reports"
        Me.chkWaterBalance.UseVisualStyleBackColor = True
        '
        'chkSedimentBalance
        '
        Me.chkSedimentBalance.AutoSize = True
        Me.chkSedimentBalance.Location = New System.Drawing.Point(6, 65)
        Me.chkSedimentBalance.Name = "chkSedimentBalance"
        Me.chkSedimentBalance.Size = New System.Drawing.Size(152, 17)
        Me.chkSedimentBalance.TabIndex = 16
        Me.chkSedimentBalance.Text = "Sediment Balance Reports"
        Me.chkSedimentBalance.UseVisualStyleBackColor = True
        '
        'chkFecalColiform
        '
        Me.chkFecalColiform.AutoSize = True
        Me.chkFecalColiform.Enabled = False
        Me.chkFecalColiform.Location = New System.Drawing.Point(6, 157)
        Me.chkFecalColiform.Name = "chkFecalColiform"
        Me.chkFecalColiform.Size = New System.Drawing.Size(132, 17)
        Me.chkFecalColiform.TabIndex = 20
        Me.chkFecalColiform.Text = "Fecal Coliform Reports"
        Me.chkFecalColiform.UseVisualStyleBackColor = True
        '
        'chkBODBalance
        '
        Me.chkBODBalance.AutoSize = True
        Me.chkBODBalance.Location = New System.Drawing.Point(6, 134)
        Me.chkBODBalance.Name = "chkBODBalance"
        Me.chkBODBalance.Size = New System.Drawing.Size(89, 17)
        Me.chkBODBalance.TabIndex = 19
        Me.chkBODBalance.Text = "BOD Reports"
        Me.chkBODBalance.UseVisualStyleBackColor = True
        '
        'chkTotalPhosphorus
        '
        Me.chkTotalPhosphorus.AutoSize = True
        Me.chkTotalPhosphorus.Location = New System.Drawing.Point(6, 111)
        Me.chkTotalPhosphorus.Name = "chkTotalPhosphorus"
        Me.chkTotalPhosphorus.Size = New System.Drawing.Size(191, 17)
        Me.chkTotalPhosphorus.TabIndex = 18
        Me.chkTotalPhosphorus.Text = "Total Phosphorus Balance Reports"
        Me.chkTotalPhosphorus.UseVisualStyleBackColor = True
        '
        'chkTotalNitrogen
        '
        Me.chkTotalNitrogen.AutoSize = True
        Me.chkTotalNitrogen.Location = New System.Drawing.Point(6, 88)
        Me.chkTotalNitrogen.Name = "chkTotalNitrogen"
        Me.chkTotalNitrogen.Size = New System.Drawing.Size(175, 17)
        Me.chkTotalNitrogen.TabIndex = 17
        Me.chkTotalNitrogen.Text = "Total Nitrogen Balance Reports"
        Me.chkTotalNitrogen.UseVisualStyleBackColor = True
        '
        'chkExpertStats
        '
        Me.chkExpertStats.AutoSize = True
        Me.chkExpertStats.Location = New System.Drawing.Point(6, 42)
        Me.chkExpertStats.Name = "chkExpertStats"
        Me.chkExpertStats.Size = New System.Drawing.Size(148, 17)
        Me.chkExpertStats.TabIndex = 9
        Me.chkExpertStats.Text = "Calculate Expert Statistics"
        Me.chkExpertStats.UseVisualStyleBackColor = True
        '
        'btnMakeEXSFile
        '
        Me.btnMakeEXSFile.AutoSize = True
        Me.btnMakeEXSFile.Enabled = False
        Me.btnMakeEXSFile.Location = New System.Drawing.Point(20, 86)
        Me.btnMakeEXSFile.Name = "btnMakeEXSFile"
        Me.btnMakeEXSFile.Size = New System.Drawing.Size(195, 23)
        Me.btnMakeEXSFile.TabIndex = 31
        Me.btnMakeEXSFile.Text = "Create Basins Specification (EXS) File"
        Me.btnMakeEXSFile.UseVisualStyleBackColor = True
        '
        'chkAdditionalgraphs
        '
        Me.chkAdditionalgraphs.AutoSize = True
        Me.chkAdditionalgraphs.Location = New System.Drawing.Point(6, 42)
        Me.chkAdditionalgraphs.Name = "chkAdditionalgraphs"
        Me.chkAdditionalgraphs.Size = New System.Drawing.Size(408, 17)
        Me.chkAdditionalgraphs.TabIndex = 15
        Me.chkAdditionalgraphs.Text = "Generate Additional graphs (Make sure that AdditionalGraphs.csv file is available" & _
            ")"
        Me.chkAdditionalgraphs.UseVisualStyleBackColor = True
        '
        'btn_help
        '
        Me.btn_help.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_help.Location = New System.Drawing.Point(371, 19)
        Me.btn_help.Name = "btn_help"
        Me.btn_help.Size = New System.Drawing.Size(75, 23)
        Me.btn_help.TabIndex = 43
        Me.btn_help.Text = "Help"
        Me.btn_help.UseVisualStyleBackColor = True
        '
        'chkHydrologySensitivity
        '
        Me.chkHydrologySensitivity.AutoSize = True
        Me.chkHydrologySensitivity.Enabled = False
        Me.chkHydrologySensitivity.Location = New System.Drawing.Point(6, 19)
        Me.chkHydrologySensitivity.Name = "chkHydrologySensitivity"
        Me.chkHydrologySensitivity.Size = New System.Drawing.Size(164, 17)
        Me.chkHydrologySensitivity.TabIndex = 8
        Me.chkHydrologySensitivity.Text = "Hydrology Sensitivity Analysis"
        Me.chkHydrologySensitivity.UseVisualStyleBackColor = True
        '
        'chkWaterQualitySensitivity
        '
        Me.chkWaterQualitySensitivity.AutoSize = True
        Me.chkWaterQualitySensitivity.Enabled = False
        Me.chkWaterQualitySensitivity.Location = New System.Drawing.Point(6, 19)
        Me.chkWaterQualitySensitivity.Name = "chkWaterQualitySensitivity"
        Me.chkWaterQualitySensitivity.Size = New System.Drawing.Size(181, 17)
        Me.chkWaterQualitySensitivity.TabIndex = 14
        Me.chkWaterQualitySensitivity.Text = "Water Quality Sensitivity Analysis"
        Me.chkWaterQualitySensitivity.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkGraphStandard)
        Me.GroupBox1.Controls.Add(Me.chkHydrologySensitivity)
        Me.GroupBox1.Controls.Add(Me.chkExpertStats)
        Me.GroupBox1.Controls.Add(Me.chkWaterBalance)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 164)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(442, 112)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Hydrology"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkFecalColiform)
        Me.GroupBox2.Controls.Add(Me.chkWaterQualitySensitivity)
        Me.GroupBox2.Controls.Add(Me.chkBODBalance)
        Me.GroupBox2.Controls.Add(Me.chkAdditionalgraphs)
        Me.GroupBox2.Controls.Add(Me.chkTotalPhosphorus)
        Me.GroupBox2.Controls.Add(Me.chkSedimentBalance)
        Me.GroupBox2.Controls.Add(Me.chkTotalNitrogen)
        Me.GroupBox2.Location = New System.Drawing.Point(15, 282)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(442, 183)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Water Quality"
        '
        'StartUp
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdEnd
        Me.ClientSize = New System.Drawing.Size(463, 569)
        Me.Controls.Add(Me.lblRCH)
        Me.Controls.Add(Me.txtRCH)
        Me.Controls.Add(Me.lblOutReach2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.pnlHighlight)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btn_help)
        Me.Controls.Add(Me.btnMakeEXSFile)
        Me.Controls.Add(Me.chkRunHSPF)
        Me.Controls.Add(Me.chkAreaReports)
        Me.Controls.Add(Me.cmdEnd)
        Me.Controls.Add(Me.txtUCIPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.cmdStart)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "StartUp"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HSPEXP+ "
        Me.TransparencyKey = System.Drawing.SystemColors.ActiveBorder
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUCIPath As System.Windows.Forms.TextBox
    Friend WithEvents lblRCH As System.Windows.Forms.Label
    Friend WithEvents txtRCH As System.Windows.Forms.TextBox
    Friend WithEvents lblOutReach2 As System.Windows.Forms.Label
    Friend WithEvents cmdEnd As System.Windows.Forms.Button
    Friend WithEvents chkAreaReports As System.Windows.Forms.CheckBox
    Friend WithEvents chkGraphStandard As System.Windows.Forms.CheckBox
    Friend WithEvents chkRunHSPF As System.Windows.Forms.CheckBox
    Friend WithEvents pnlHighlight As System.Windows.Forms.Panel
    Friend WithEvents chkWaterBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkSedimentBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkExpertStats As System.Windows.Forms.CheckBox
    Friend WithEvents chkTotalNitrogen As System.Windows.Forms.CheckBox
    Friend WithEvents chkTotalPhosphorus As System.Windows.Forms.CheckBox
    Friend WithEvents chkBODBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkFecalColiform As System.Windows.Forms.CheckBox
    Friend WithEvents btnMakeEXSFile As System.Windows.Forms.Button
    Friend WithEvents chkAdditionalgraphs As System.Windows.Forms.CheckBox
    Friend WithEvents btn_help As System.Windows.Forms.Button
    Friend WithEvents chkHydrologySensitivity As System.Windows.Forms.CheckBox
    Friend WithEvents chkWaterQualitySensitivity As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox

End Class
