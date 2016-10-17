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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartUp))
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.chkAdditionalgraphs = New System.Windows.Forms.CheckBox()
        Me.btn_help = New System.Windows.Forms.Button()
        Me.chkHydrologySensitivity = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cmbUCIPath = New System.Windows.Forms.ComboBox()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Plotting = New System.Windows.Forms.GroupBox()
        Me.chkReganGraphs = New System.Windows.Forms.CheckBox()
        Me.ttHSPEXP = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Plotting.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdStart.Enabled = False
        Me.cmdStart.Location = New System.Drawing.Point(326, 596)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(75, 23)
        Me.cmdStart.TabIndex = 19
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.AllowDrop = True
        Me.cmdBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowse.Location = New System.Drawing.Point(401, 52)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(75, 23)
        Me.cmdBrowse.TabIndex = 2
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
        'lblRCH
        '
        Me.lblRCH.AutoSize = True
        Me.lblRCH.Enabled = False
        Me.lblRCH.Location = New System.Drawing.Point(19, 538)
        Me.lblRCH.MaximumSize = New System.Drawing.Size(500, 0)
        Me.lblRCH.Name = "lblRCH"
        Me.lblRCH.Size = New System.Drawing.Size(441, 13)
        Me.lblRCH.TabIndex = 7
        Me.lblRCH.Text = "Number(s) of the outlet reach at which you would like water and/or nutrient balan" &
    "ce reports?"
        '
        'txtRCH
        '
        Me.txtRCH.BackColor = System.Drawing.SystemColors.Window
        Me.txtRCH.Enabled = False
        Me.txtRCH.Location = New System.Drawing.Point(24, 562)
        Me.txtRCH.Name = "txtRCH"
        Me.txtRCH.Size = New System.Drawing.Size(47, 20)
        Me.txtRCH.TabIndex = 18
        Me.ttHSPEXP.SetToolTip(Me.txtRCH, "Enter the location at which you want to generate constituent balance and load all" &
        "ocation reports.")
        '
        'lblOutReach2
        '
        Me.lblOutReach2.AutoSize = True
        Me.lblOutReach2.Enabled = False
        Me.lblOutReach2.Location = New System.Drawing.Point(82, 565)
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
        Me.cmdEnd.Location = New System.Drawing.Point(407, 596)
        Me.cmdEnd.Name = "cmdEnd"
        Me.cmdEnd.Size = New System.Drawing.Size(75, 23)
        Me.cmdEnd.TabIndex = 20
        Me.cmdEnd.Text = "End"
        Me.cmdEnd.UseVisualStyleBackColor = True
        '
        'chkAreaReports
        '
        Me.chkAreaReports.AutoSize = True
        Me.chkAreaReports.Enabled = False
        Me.chkAreaReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAreaReports.Location = New System.Drawing.Point(22, 104)
        Me.chkAreaReports.Name = "chkAreaReports"
        Me.chkAreaReports.Size = New System.Drawing.Size(143, 17)
        Me.chkAreaReports.TabIndex = 4
        Me.chkAreaReports.Text = "Watershed Area Reports"
        Me.ttHSPEXP.SetToolTip(Me.chkAreaReports, "Check to provide watershed area reports based on the schematic block.")
        Me.chkAreaReports.UseVisualStyleBackColor = True
        '
        'chkGraphStandard
        '
        Me.chkGraphStandard.AutoSize = True
        Me.chkGraphStandard.Enabled = False
        Me.chkGraphStandard.Location = New System.Drawing.Point(3, 15)
        Me.chkGraphStandard.Name = "chkGraphStandard"
        Me.chkGraphStandard.Size = New System.Drawing.Size(162, 17)
        Me.chkGraphStandard.TabIndex = 9
        Me.chkGraphStandard.Text = "Hydrology Calibration Graphs"
        Me.ttHSPEXP.SetToolTip(Me.chkGraphStandard, "Hydrology calibration graphs can be prepared only if Expert Statistics are being " &
        "generated.")
        Me.chkGraphStandard.UseVisualStyleBackColor = True
        '
        'chkRunHSPF
        '
        Me.chkRunHSPF.AutoSize = True
        Me.chkRunHSPF.Enabled = False
        Me.chkRunHSPF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRunHSPF.Location = New System.Drawing.Point(22, 81)
        Me.chkRunHSPF.Name = "chkRunHSPF"
        Me.chkRunHSPF.Size = New System.Drawing.Size(77, 17)
        Me.chkRunHSPF.TabIndex = 3
        Me.chkRunHSPF.Text = "Run HSPF"
        Me.ttHSPEXP.SetToolTip(Me.chkRunHSPF, "Check to run HSPF before HSPEXP+ analysis.")
        Me.chkRunHSPF.UseVisualStyleBackColor = True
        '
        'pnlHighlight
        '
        Me.pnlHighlight.BackColor = System.Drawing.Color.Red
        Me.pnlHighlight.Enabled = False
        Me.pnlHighlight.Location = New System.Drawing.Point(18, 558)
        Me.pnlHighlight.Name = "pnlHighlight"
        Me.pnlHighlight.Size = New System.Drawing.Size(58, 28)
        Me.pnlHighlight.TabIndex = 17
        '
        'chkWaterBalance
        '
        Me.chkWaterBalance.AutoSize = True
        Me.chkWaterBalance.Enabled = False
        Me.chkWaterBalance.Location = New System.Drawing.Point(6, 19)
        Me.chkWaterBalance.Name = "chkWaterBalance"
        Me.chkWaterBalance.Size = New System.Drawing.Size(137, 17)
        Me.chkWaterBalance.TabIndex = 12
        Me.chkWaterBalance.Text = "Water Balance Reports"
        Me.chkWaterBalance.UseVisualStyleBackColor = True
        '
        'chkSedimentBalance
        '
        Me.chkSedimentBalance.AutoSize = True
        Me.chkSedimentBalance.Enabled = False
        Me.chkSedimentBalance.Location = New System.Drawing.Point(6, 42)
        Me.chkSedimentBalance.Name = "chkSedimentBalance"
        Me.chkSedimentBalance.Size = New System.Drawing.Size(152, 17)
        Me.chkSedimentBalance.TabIndex = 13
        Me.chkSedimentBalance.Text = "Sediment Balance Reports"
        Me.chkSedimentBalance.UseVisualStyleBackColor = True
        '
        'chkFecalColiform
        '
        Me.chkFecalColiform.AutoSize = True
        Me.chkFecalColiform.Enabled = False
        Me.chkFecalColiform.Location = New System.Drawing.Point(6, 134)
        Me.chkFecalColiform.Name = "chkFecalColiform"
        Me.chkFecalColiform.Size = New System.Drawing.Size(132, 17)
        Me.chkFecalColiform.TabIndex = 17
        Me.chkFecalColiform.Text = "Fecal Coliform Reports"
        Me.chkFecalColiform.UseVisualStyleBackColor = True
        '
        'chkBODBalance
        '
        Me.chkBODBalance.AutoSize = True
        Me.chkBODBalance.Enabled = False
        Me.chkBODBalance.Location = New System.Drawing.Point(6, 111)
        Me.chkBODBalance.Name = "chkBODBalance"
        Me.chkBODBalance.Size = New System.Drawing.Size(120, 17)
        Me.chkBODBalance.TabIndex = 16
        Me.chkBODBalance.Text = "BOD-Labile Reports"
        Me.chkBODBalance.UseVisualStyleBackColor = True
        '
        'chkTotalPhosphorus
        '
        Me.chkTotalPhosphorus.AutoSize = True
        Me.chkTotalPhosphorus.Enabled = False
        Me.chkTotalPhosphorus.Location = New System.Drawing.Point(6, 88)
        Me.chkTotalPhosphorus.Name = "chkTotalPhosphorus"
        Me.chkTotalPhosphorus.Size = New System.Drawing.Size(191, 17)
        Me.chkTotalPhosphorus.TabIndex = 15
        Me.chkTotalPhosphorus.Text = "Total Phosphorus Balance Reports"
        Me.chkTotalPhosphorus.UseVisualStyleBackColor = True
        '
        'chkTotalNitrogen
        '
        Me.chkTotalNitrogen.AutoSize = True
        Me.chkTotalNitrogen.Enabled = False
        Me.chkTotalNitrogen.Location = New System.Drawing.Point(6, 65)
        Me.chkTotalNitrogen.Name = "chkTotalNitrogen"
        Me.chkTotalNitrogen.Size = New System.Drawing.Size(175, 17)
        Me.chkTotalNitrogen.TabIndex = 14
        Me.chkTotalNitrogen.Text = "Total Nitrogen Balance Reports"
        Me.chkTotalNitrogen.UseVisualStyleBackColor = True
        '
        'chkExpertStats
        '
        Me.chkExpertStats.AutoSize = True
        Me.chkExpertStats.Enabled = False
        Me.chkExpertStats.Location = New System.Drawing.Point(6, 42)
        Me.chkExpertStats.Name = "chkExpertStats"
        Me.chkExpertStats.Size = New System.Drawing.Size(148, 17)
        Me.chkExpertStats.TabIndex = 8
        Me.chkExpertStats.Text = "Calculate Expert Statistics"
        Me.ttHSPEXP.SetToolTip(Me.chkExpertStats, "Make sure that the basins specification file (EXS) file is available for the cali" &
        "bration location in the project folder.")
        Me.chkExpertStats.UseVisualStyleBackColor = True
        '
        'chkAdditionalgraphs
        '
        Me.chkAdditionalgraphs.AutoSize = True
        Me.chkAdditionalgraphs.Enabled = False
        Me.chkAdditionalgraphs.Location = New System.Drawing.Point(3, 39)
        Me.chkAdditionalgraphs.Name = "chkAdditionalgraphs"
        Me.chkAdditionalgraphs.Size = New System.Drawing.Size(283, 17)
        Me.chkAdditionalgraphs.TabIndex = 10
        Me.chkAdditionalgraphs.Text = "Generate Graphs from Graph Specification Files (*.csv)"
        Me.ttHSPEXP.SetToolTip(Me.chkAdditionalgraphs, "Make sure that the graph specification file(s) (*.csv) is/are available in the pr" &
        "oject folder.")
        Me.chkAdditionalgraphs.UseVisualStyleBackColor = True
        '
        'btn_help
        '
        Me.btn_help.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_help.Location = New System.Drawing.Point(401, 19)
        Me.btn_help.Name = "btn_help"
        Me.btn_help.Size = New System.Drawing.Size(75, 23)
        Me.btn_help.TabIndex = 0
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
        Me.chkHydrologySensitivity.TabIndex = 7
        Me.chkHydrologySensitivity.Text = "Hydrology Sensitivity Analysis"
        Me.chkHydrologySensitivity.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkHydrologySensitivity)
        Me.GroupBox1.Controls.Add(Me.chkExpertStats)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(18, 189)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(442, 68)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Hydrology Calibration"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkFecalColiform)
        Me.GroupBox2.Controls.Add(Me.chkBODBalance)
        Me.GroupBox2.Controls.Add(Me.chkTotalPhosphorus)
        Me.GroupBox2.Controls.Add(Me.chkWaterBalance)
        Me.GroupBox2.Controls.Add(Me.chkSedimentBalance)
        Me.GroupBox2.Controls.Add(Me.chkTotalNitrogen)
        Me.GroupBox2.Location = New System.Drawing.Point(18, 366)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(442, 160)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Constituent Balance Reports"
        Me.ttHSPEXP.SetToolTip(Me.GroupBox2, "All the following reports requre the user to output these constituents in binaru " &
        "file(s) at monthly or shorter intervals. ")
        '
        'cmbUCIPath
        '
        Me.cmbUCIPath.AllowDrop = True
        Me.cmbUCIPath.DisplayMember = """STYLE"""
        Me.cmbUCIPath.DropDownHeight = 150
        Me.cmbUCIPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbUCIPath.FormattingEnabled = True
        Me.cmbUCIPath.IntegralHeight = False
        Me.cmbUCIPath.Location = New System.Drawing.Point(22, 54)
        Me.cmbUCIPath.Name = "cmbUCIPath"
        Me.cmbUCIPath.Size = New System.Drawing.Size(311, 21)
        Me.cmbUCIPath.TabIndex = 1
        Me.cmbUCIPath.ValueMember = """STYLE"""
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Enabled = False
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(51, 19)
        Me.DateTimePicker1.MaxDate = New Date(2200, 12, 31, 0, 0, 0, 0)
        Me.DateTimePicker1.MinDate = New Date(1800, 1, 1, 0, 0, 0, 0)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(96, 20)
        Me.DateTimePicker1.TabIndex = 5
        Me.DateTimePicker1.Value = New Date(1996, 1, 1, 0, 0, 0, 0)
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Enabled = False
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker2.Location = New System.Drawing.Point(204, 19)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(96, 20)
        Me.DateTimePicker2.TabIndex = 6
        Me.DateTimePicker2.Value = New Date(2009, 12, 31, 0, 0, 0, 0)
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.DateTimePicker1)
        Me.GroupBox3.Controls.Add(Me.DateTimePicker2)
        Me.GroupBox3.Location = New System.Drawing.Point(18, 132)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(442, 51)
        Me.GroupBox3.TabIndex = 47
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Analysis Period"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(161, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 13)
        Me.Label3.TabIndex = 48
        Me.Label3.Text = "End"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Start"
        '
        'Plotting
        '
        Me.Plotting.Controls.Add(Me.chkReganGraphs)
        Me.Plotting.Controls.Add(Me.chkGraphStandard)
        Me.Plotting.Controls.Add(Me.chkAdditionalgraphs)
        Me.Plotting.Location = New System.Drawing.Point(21, 265)
        Me.Plotting.Name = "Plotting"
        Me.Plotting.Size = New System.Drawing.Size(438, 86)
        Me.Plotting.TabIndex = 48
        Me.Plotting.TabStop = False
        Me.Plotting.Text = "Graph"
        '
        'chkReganGraphs
        '
        Me.chkReganGraphs.AutoSize = True
        Me.chkReganGraphs.Location = New System.Drawing.Point(3, 62)
        Me.chkReganGraphs.Name = "chkReganGraphs"
        Me.chkReganGraphs.Size = New System.Drawing.Size(84, 17)
        Me.chkReganGraphs.TabIndex = 11
        Me.chkReganGraphs.Text = "Regan Plots"
        Me.ttHSPEXP.SetToolTip(Me.chkReganGraphs, "Make sure that PLANK section is active and models sre output in binary files at m" &
        "onthly, or shorter intervals.")
        Me.chkReganGraphs.UseVisualStyleBackColor = True
        '
        'StartUp
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdEnd
        Me.ClientSize = New System.Drawing.Size(493, 636)
        Me.Controls.Add(Me.Plotting)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.cmbUCIPath)
        Me.Controls.Add(Me.lblRCH)
        Me.Controls.Add(Me.txtRCH)
        Me.Controls.Add(Me.lblOutReach2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.pnlHighlight)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btn_help)
        Me.Controls.Add(Me.chkRunHSPF)
        Me.Controls.Add(Me.chkAreaReports)
        Me.Controls.Add(Me.cmdEnd)
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
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.Plotting.ResumeLayout(False)
        Me.Plotting.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
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
    Friend WithEvents chkAdditionalgraphs As System.Windows.Forms.CheckBox
    Friend WithEvents btn_help As System.Windows.Forms.Button
    Friend WithEvents chkHydrologySensitivity As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbUCIPath As System.Windows.Forms.ComboBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Plotting As GroupBox
    Friend WithEvents chkReganGraphs As CheckBox
    Friend WithEvents ttHSPEXP As ToolTip
End Class
