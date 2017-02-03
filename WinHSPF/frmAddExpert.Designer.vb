<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddExpert
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblOperation = New System.Windows.Forms.Label()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.lblLoc = New System.Windows.Forms.Label()
        Me.lblBase = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.optH = New System.Windows.Forms.RadioButton()
        Me.optD = New System.Windows.Forms.RadioButton()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.lstOperation = New System.Windows.Forms.ListBox()
        Me.lstGroup = New System.Windows.Forms.ListBox()
        Me.txtLoc = New System.Windows.Forms.TextBox()
        Me.atxBase = New atcControls.atcText()
        Me.optMetric = New System.Windows.Forms.RadioButton()
        Me.optEnglish = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTimeStep = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblUnits = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblOperation
        '
        Me.lblOperation.AutoSize = True
        Me.lblOperation.Location = New System.Drawing.Point(12, 7)
        Me.lblOperation.Name = "lblOperation"
        Me.lblOperation.Size = New System.Drawing.Size(56, 13)
        Me.lblOperation.TabIndex = 0
        Me.lblOperation.Text = "Operation:"
        '
        'lblGroup
        '
        Me.lblGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Location = New System.Drawing.Point(195, 7)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(82, 13)
        Me.lblGroup.TabIndex = 1
        Me.lblGroup.Text = "Group/Member:"
        '
        'lblLoc
        '
        Me.lblLoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLoc.AutoSize = True
        Me.lblLoc.Location = New System.Drawing.Point(86, 255)
        Me.lblLoc.Name = "lblLoc"
        Me.lblLoc.Size = New System.Drawing.Size(96, 13)
        Me.lblLoc.TabIndex = 8
        Me.lblLoc.Text = "WDM Location ID:"
        '
        'lblBase
        '
        Me.lblBase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblBase.AutoSize = True
        Me.lblBase.Location = New System.Drawing.Point(91, 285)
        Me.lblBase.Name = "lblBase"
        Me.lblBase.Size = New System.Drawing.Size(91, 13)
        Me.lblBase.TabIndex = 9
        Me.lblBase.Text = "Base WDM DSN:"
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Control
        Me.txtDescription.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(12, 197)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(350, 49)
        Me.txtDescription.TabIndex = 10
        '
        'optH
        '
        Me.optH.AutoSize = True
        Me.optH.Location = New System.Drawing.Point(222, 3)
        Me.optH.Name = "optH"
        Me.optH.Size = New System.Drawing.Size(55, 17)
        Me.optH.TabIndex = 11
        Me.optH.Text = "Hourly"
        Me.optH.UseVisualStyleBackColor = True
        '
        'optD
        '
        Me.optD.AutoSize = True
        Me.optD.Checked = True
        Me.optD.Location = New System.Drawing.Point(157, 3)
        Me.optD.Name = "optD"
        Me.optD.Size = New System.Drawing.Size(48, 17)
        Me.optD.TabIndex = 12
        Me.optD.TabStop = True
        Me.optD.Text = "Daily"
        Me.optD.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(102, 365)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 24)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(200, 365)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 24)
        Me.cmdClose.TabIndex = 14
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'lstOperation
        '
        Me.lstOperation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstOperation.FormattingEnabled = True
        Me.lstOperation.Location = New System.Drawing.Point(15, 27)
        Me.lstOperation.Name = "lstOperation"
        Me.lstOperation.Size = New System.Drawing.Size(167, 160)
        Me.lstOperation.TabIndex = 15
        '
        'lstGroup
        '
        Me.lstGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstGroup.FormattingEnabled = True
        Me.lstGroup.Location = New System.Drawing.Point(198, 27)
        Me.lstGroup.Name = "lstGroup"
        Me.lstGroup.Size = New System.Drawing.Size(167, 160)
        Me.lstGroup.TabIndex = 16
        '
        'txtLoc
        '
        Me.txtLoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLoc.Location = New System.Drawing.Point(198, 252)
        Me.txtLoc.Name = "txtLoc"
        Me.txtLoc.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtLoc.Size = New System.Drawing.Size(80, 20)
        Me.txtLoc.TabIndex = 18
        '
        'atxBase
        '
        Me.atxBase.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.atxBase.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxBase.DefaultValue = ""
        Me.atxBase.HardMax = -999.0R
        Me.atxBase.HardMin = 1.0R
        Me.atxBase.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxBase.Location = New System.Drawing.Point(198, 278)
        Me.atxBase.MaxWidth = 20
        Me.atxBase.Name = "atxBase"
        Me.atxBase.NumericFormat = "0.#####"
        Me.atxBase.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxBase.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxBase.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.atxBase.SelLength = 0
        Me.atxBase.SelStart = 0
        Me.atxBase.Size = New System.Drawing.Size(80, 20)
        Me.atxBase.SoftMax = -999.0R
        Me.atxBase.SoftMin = -999.0R
        Me.atxBase.TabIndex = 17
        Me.atxBase.ValueDouble = 1000.0R
        Me.atxBase.ValueInteger = 1000
        '
        'optMetric
        '
        Me.optMetric.AutoSize = True
        Me.optMetric.Location = New System.Drawing.Point(222, 0)
        Me.optMetric.Name = "optMetric"
        Me.optMetric.Size = New System.Drawing.Size(54, 17)
        Me.optMetric.TabIndex = 20
        Me.optMetric.Text = "Metric"
        Me.optMetric.UseVisualStyleBackColor = True
        '
        'optEnglish
        '
        Me.optEnglish.AutoSize = True
        Me.optEnglish.Checked = True
        Me.optEnglish.Location = New System.Drawing.Point(157, 0)
        Me.optEnglish.Name = "optEnglish"
        Me.optEnglish.Size = New System.Drawing.Size(59, 17)
        Me.optEnglish.TabIndex = 19
        Me.optEnglish.TabStop = True
        Me.optEnglish.Text = "English"
        Me.optEnglish.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.lblTimeStep)
        Me.Panel1.Controls.Add(Me.optH)
        Me.Panel1.Controls.Add(Me.optD)
        Me.Panel1.Location = New System.Drawing.Point(41, 304)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(290, 24)
        Me.Panel1.TabIndex = 21
        '
        'lblTimeStep
        '
        Me.lblTimeStep.AutoSize = True
        Me.lblTimeStep.Location = New System.Drawing.Point(48, 5)
        Me.lblTimeStep.Name = "lblTimeStep"
        Me.lblTimeStep.Size = New System.Drawing.Size(93, 13)
        Me.lblTimeStep.TabIndex = 13
        Me.lblTimeStep.Text = "Output Time Step:"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.lblUnits)
        Me.Panel2.Controls.Add(Me.optEnglish)
        Me.Panel2.Controls.Add(Me.optMetric)
        Me.Panel2.Location = New System.Drawing.Point(41, 334)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(289, 18)
        Me.Panel2.TabIndex = 22
        '
        'lblUnits
        '
        Me.lblUnits.AutoSize = True
        Me.lblUnits.Location = New System.Drawing.Point(72, 2)
        Me.lblUnits.Name = "lblUnits"
        Me.lblUnits.Size = New System.Drawing.Size(69, 13)
        Me.lblUnits.TabIndex = 21
        Me.lblUnits.Text = "Output Units:"
        '
        'frmAddExpert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(377, 400)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtLoc)
        Me.Controls.Add(Me.atxBase)
        Me.Controls.Add(Me.lstGroup)
        Me.Controls.Add(Me.lstOperation)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblBase)
        Me.Controls.Add(Me.lblLoc)
        Me.Controls.Add(Me.lblGroup)
        Me.Controls.Add(Me.lblOperation)
        Me.Controls.Add(Me.Panel2)
        Me.KeyPreview = True
        Me.Name = "frmAddExpert"
        Me.Text = "WinHSPF - Add Output"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblOperation As System.Windows.Forms.Label
    Friend WithEvents lblGroup As System.Windows.Forms.Label
    Friend WithEvents lblLoc As System.Windows.Forms.Label
    Friend WithEvents lblBase As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents optH As System.Windows.Forms.RadioButton
    Friend WithEvents optD As System.Windows.Forms.RadioButton
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents lstOperation As System.Windows.Forms.ListBox
    Friend WithEvents lstGroup As System.Windows.Forms.ListBox
    Friend WithEvents atxBase As atcControls.atcText
    Friend WithEvents txtLoc As System.Windows.Forms.TextBox
    Friend WithEvents optMetric As System.Windows.Forms.RadioButton
    Friend WithEvents optEnglish As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblTimeStep As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblUnits As System.Windows.Forms.Label
End Class
