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
        Me.pnlHighlight = New System.Windows.Forms.Panel
        Me.lblOutReach2 = New System.Windows.Forms.Label
        Me.cmdEnd = New System.Windows.Forms.Button
        Me.chkAreaReports = New System.Windows.Forms.CheckBox
        Me.chkGraphs = New System.Windows.Forms.CheckBox
        Me.chkGraphStandard = New System.Windows.Forms.CheckBox
        Me.chkLogGraphs = New System.Windows.Forms.CheckBox
        Me.chkSupportingGraphs = New System.Windows.Forms.CheckBox
        Me.chkRunHSPF = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdStart.Enabled = False
        Me.cmdStart.Location = New System.Drawing.Point(327, 300)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(75, 23)
        Me.cmdStart.TabIndex = 0
        Me.cmdStart.Text = "Start"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(410, 4)
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
        Me.txtUCIPath.Enabled = False
        Me.txtUCIPath.Location = New System.Drawing.Point(15, 33)
        Me.txtUCIPath.Name = "txtUCIPath"
        Me.txtUCIPath.Size = New System.Drawing.Size(469, 20)
        Me.txtUCIPath.TabIndex = 3
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
        Me.txtPrefix.Size = New System.Drawing.Size(100, 20)
        Me.txtPrefix.TabIndex = 6
        '
        'lblRCH
        '
        Me.lblRCH.AutoSize = True
        Me.lblRCH.Location = New System.Drawing.Point(11, 249)
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
        Me.txtRCH.Location = New System.Drawing.Point(211, 246)
        Me.txtRCH.Name = "txtRCH"
        Me.txtRCH.Size = New System.Drawing.Size(47, 20)
        Me.txtRCH.TabIndex = 8
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
        Me.txtRunNo.TabIndex = 13
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
        Me.txtRootPath.Location = New System.Drawing.Point(293, 151)
        Me.txtRootPath.Name = "txtRootPath"
        Me.txtRootPath.Size = New System.Drawing.Size(192, 20)
        Me.txtRootPath.TabIndex = 16
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
        'pnlHighlight
        '
        Me.pnlHighlight.BackColor = System.Drawing.Color.Red
        Me.pnlHighlight.Location = New System.Drawing.Point(205, 242)
        Me.pnlHighlight.Name = "pnlHighlight"
        Me.pnlHighlight.Size = New System.Drawing.Size(58, 28)
        Me.pnlHighlight.TabIndex = 17
        Me.pnlHighlight.Visible = False
        '
        'lblOutReach2
        '
        Me.lblOutReach2.AutoSize = True
        Me.lblOutReach2.Location = New System.Drawing.Point(269, 249)
        Me.lblOutReach2.MaximumSize = New System.Drawing.Size(250, 0)
        Me.lblOutReach2.Name = "lblOutReach2"
        Me.lblOutReach2.Size = New System.Drawing.Size(248, 39)
        Me.lblOutReach2.TabIndex = 18
        Me.lblOutReach2.Text = "NOTE: If your observation station is at the outlet of multiple reaches, enter all" & _
            " reaches separated by a comma - e.g.: 5, 10"
        '
        'cmdEnd
        '
        Me.cmdEnd.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdEnd.Location = New System.Drawing.Point(408, 300)
        Me.cmdEnd.Name = "cmdEnd"
        Me.cmdEnd.Size = New System.Drawing.Size(75, 23)
        Me.cmdEnd.TabIndex = 19
        Me.cmdEnd.Text = "End"
        Me.cmdEnd.UseVisualStyleBackColor = True
        '
        'chkAreaReports
        '
        Me.chkAreaReports.AutoSize = True
        Me.chkAreaReports.Location = New System.Drawing.Point(15, 300)
        Me.chkAreaReports.Name = "chkAreaReports"
        Me.chkAreaReports.Size = New System.Drawing.Size(241, 17)
        Me.chkAreaReports.TabIndex = 20
        Me.chkAreaReports.Text = "Produce land use and watershed area reports"
        Me.chkAreaReports.UseVisualStyleBackColor = True
        '
        'chkGraphs
        '
        Me.chkGraphs.AutoSize = True
        Me.chkGraphs.Location = New System.Drawing.Point(15, 323)
        Me.chkGraphs.Name = "chkGraphs"
        Me.chkGraphs.Size = New System.Drawing.Size(101, 17)
        Me.chkGraphs.TabIndex = 21
        Me.chkGraphs.Text = "Produce graphs"
        Me.chkGraphs.UseVisualStyleBackColor = True
        '
        'chkGraphStandard
        '
        Me.chkGraphStandard.AutoSize = True
        Me.chkGraphStandard.Location = New System.Drawing.Point(28, 339)
        Me.chkGraphStandard.Name = "chkGraphStandard"
        Me.chkGraphStandard.Size = New System.Drawing.Size(373, 17)
        Me.chkGraphStandard.TabIndex = 22
        Me.chkGraphStandard.Text = "Produce standard monthly flow, daily flow, storm, and flow duration graphs"
        Me.chkGraphStandard.UseVisualStyleBackColor = True
        Me.chkGraphStandard.Visible = False
        '
        'chkLogGraphs
        '
        Me.chkLogGraphs.AutoSize = True
        Me.chkLogGraphs.Location = New System.Drawing.Point(28, 375)
        Me.chkLogGraphs.Name = "chkLogGraphs"
        Me.chkLogGraphs.Size = New System.Drawing.Size(470, 17)
        Me.chkLogGraphs.TabIndex = 23
        Me.chkLogGraphs.Text = "Produce logarithmic graphs (requires standard and/or supporting graph box(es) to " & _
            "be checked)"
        Me.chkLogGraphs.UseVisualStyleBackColor = True
        Me.chkLogGraphs.Visible = False
        '
        'chkSupportingGraphs
        '
        Me.chkSupportingGraphs.AutoSize = True
        Me.chkSupportingGraphs.Location = New System.Drawing.Point(28, 357)
        Me.chkSupportingGraphs.Name = "chkSupportingGraphs"
        Me.chkSupportingGraphs.Size = New System.Drawing.Size(180, 17)
        Me.chkSupportingGraphs.TabIndex = 24
        Me.chkSupportingGraphs.Text = "Produce other supporting graphs"
        Me.chkSupportingGraphs.UseVisualStyleBackColor = True
        Me.chkSupportingGraphs.Visible = False
        '
        'chkRunHSPF
        '
        Me.chkRunHSPF.AutoSize = True
        Me.chkRunHSPF.Location = New System.Drawing.Point(15, 184)
        Me.chkRunHSPF.Name = "chkRunHSPF"
        Me.chkRunHSPF.Size = New System.Drawing.Size(253, 17)
        Me.chkRunHSPF.TabIndex = 25
        Me.chkRunHSPF.Text = "Run WinHSPFLt before calculating the statistics"
        Me.chkRunHSPF.UseVisualStyleBackColor = True
        '
        'StartUp
        '
        Me.AcceptButton = Me.cmdStart
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdEnd
        Me.ClientSize = New System.Drawing.Size(530, 439)
        Me.Controls.Add(Me.chkRunHSPF)
        Me.Controls.Add(Me.chkSupportingGraphs)
        Me.Controls.Add(Me.chkLogGraphs)
        Me.Controls.Add(Me.chkGraphStandard)
        Me.Controls.Add(Me.chkGraphs)
        Me.Controls.Add(Me.chkAreaReports)
        Me.Controls.Add(Me.cmdEnd)
        Me.Controls.Add(Me.lblOutReach2)
        Me.Controls.Add(Me.txtRootPath)
        Me.Controls.Add(Me.lblRootDirectory)
        Me.Controls.Add(Me.lblRunNoInfo)
        Me.Controls.Add(Me.txtRunNo)
        Me.Controls.Add(Me.lblRunNo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtRCH)
        Me.Controls.Add(Me.lblRCH)
        Me.Controls.Add(Me.txtPrefix)
        Me.Controls.Add(Me.lblPrefixWarning)
        Me.Controls.Add(Me.lblPrefix)
        Me.Controls.Add(Me.txtUCIPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.cmdStart)
        Me.Controls.Add(Me.pnlHighlight)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "StartUp"
        Me.Text = "HSPEXP+ (Developed by AQUA TERRA Consultants and BSE, VT)"
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
    Friend WithEvents pnlHighlight As System.Windows.Forms.Panel
    Friend WithEvents lblOutReach2 As System.Windows.Forms.Label
    Friend WithEvents cmdEnd As System.Windows.Forms.Button
    Friend WithEvents chkAreaReports As System.Windows.Forms.CheckBox
    Friend WithEvents chkGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkGraphStandard As System.Windows.Forms.CheckBox
    Friend WithEvents chkLogGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkSupportingGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkRunHSPF As System.Windows.Forms.CheckBox

End Class
