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
        Me.lblOperation = New System.Windows.Forms.Label
        Me.lblGroup = New System.Windows.Forms.Label
        Me.lblLoc = New System.Windows.Forms.Label
        Me.lblBase = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.optH = New System.Windows.Forms.RadioButton
        Me.optD = New System.Windows.Forms.RadioButton
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.lstOperation = New System.Windows.Forms.ListBox
        Me.lstGroup = New System.Windows.Forms.ListBox
        Me.atxBase = New atcControls.atcText
        Me.txtLoc = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblOperation
        '
        Me.lblOperation.AutoSize = True
        Me.lblOperation.Location = New System.Drawing.Point(16, 9)
        Me.lblOperation.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOperation.Name = "lblOperation"
        Me.lblOperation.Size = New System.Drawing.Size(75, 17)
        Me.lblOperation.TabIndex = 0
        Me.lblOperation.Text = "Operation:"
        '
        'lblGroup
        '
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Location = New System.Drawing.Point(260, 9)
        Me.lblGroup.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(107, 17)
        Me.lblGroup.TabIndex = 1
        Me.lblGroup.Text = "Group/Member:"
        '
        'lblLoc
        '
        Me.lblLoc.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblLoc.AutoSize = True
        Me.lblLoc.Location = New System.Drawing.Point(91, 271)
        Me.lblLoc.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLoc.Name = "lblLoc"
        Me.lblLoc.Size = New System.Drawing.Size(121, 17)
        Me.lblLoc.TabIndex = 8
        Me.lblLoc.Text = "WDM Location ID:"
        '
        'lblBase
        '
        Me.lblBase.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblBase.AutoSize = True
        Me.lblBase.Location = New System.Drawing.Point(97, 303)
        Me.lblBase.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBase.Name = "lblBase"
        Me.lblBase.Size = New System.Drawing.Size(115, 17)
        Me.lblBase.TabIndex = 9
        Me.lblBase.Text = "Base WDM DSN:"
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Control
        Me.txtDescription.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(20, 185)
        Me.txtDescription.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(465, 59)
        Me.txtDescription.TabIndex = 10
        '
        'optH
        '
        Me.optH.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.optH.AutoSize = True
        Me.optH.Location = New System.Drawing.Point(339, 268)
        Me.optH.Margin = New System.Windows.Forms.Padding(4)
        Me.optH.Name = "optH"
        Me.optH.Size = New System.Drawing.Size(67, 21)
        Me.optH.TabIndex = 11
        Me.optH.Text = "Hourly"
        Me.optH.UseVisualStyleBackColor = True
        '
        'optD
        '
        Me.optD.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.optD.AutoSize = True
        Me.optD.Checked = True
        Me.optD.Location = New System.Drawing.Point(339, 300)
        Me.optD.Margin = New System.Windows.Forms.Padding(4)
        Me.optD.Name = "optD"
        Me.optD.Size = New System.Drawing.Size(57, 21)
        Me.optD.TabIndex = 12
        Me.optD.TabStop = True
        Me.optD.Text = "Daily"
        Me.optD.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(136, 353)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(100, 30)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(267, 353)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(100, 30)
        Me.cmdClose.TabIndex = 14
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'lstOperation
        '
        Me.lstOperation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstOperation.FormattingEnabled = True
        Me.lstOperation.ItemHeight = 16
        Me.lstOperation.Location = New System.Drawing.Point(20, 33)
        Me.lstOperation.Margin = New System.Windows.Forms.Padding(4)
        Me.lstOperation.Name = "lstOperation"
        Me.lstOperation.Size = New System.Drawing.Size(221, 132)
        Me.lstOperation.TabIndex = 15
        '
        'lstGroup
        '
        Me.lstGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstGroup.FormattingEnabled = True
        Me.lstGroup.ItemHeight = 16
        Me.lstGroup.Location = New System.Drawing.Point(264, 33)
        Me.lstGroup.Margin = New System.Windows.Forms.Padding(4)
        Me.lstGroup.Name = "lstGroup"
        Me.lstGroup.Size = New System.Drawing.Size(221, 132)
        Me.lstGroup.TabIndex = 16
        '
        'atxBase
        '
        Me.atxBase.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxBase.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxBase.DefaultValue = ""
        Me.atxBase.HardMax = -999
        Me.atxBase.HardMin = 1
        Me.atxBase.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxBase.Location = New System.Drawing.Point(224, 298)
        Me.atxBase.Margin = New System.Windows.Forms.Padding(4)
        Me.atxBase.MaxWidth = 20
        Me.atxBase.Name = "atxBase"
        Me.atxBase.NumericFormat = "0.#####"
        Me.atxBase.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxBase.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxBase.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.atxBase.SelLength = 0
        Me.atxBase.SelStart = 0
        Me.atxBase.Size = New System.Drawing.Size(107, 25)
        Me.atxBase.SoftMax = -999
        Me.atxBase.SoftMin = -999
        Me.atxBase.TabIndex = 17
        Me.atxBase.ValueDouble = 1000
        Me.atxBase.ValueInteger = 1000
        '
        'txtLoc
        '
        Me.txtLoc.Location = New System.Drawing.Point(224, 266)
        Me.txtLoc.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLoc.Name = "txtLoc"
        Me.txtLoc.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.txtLoc.Size = New System.Drawing.Size(105, 22)
        Me.txtLoc.TabIndex = 18
        '
        'frmAddExpert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(503, 396)
        Me.Controls.Add(Me.txtLoc)
        Me.Controls.Add(Me.atxBase)
        Me.Controls.Add(Me.lstGroup)
        Me.Controls.Add(Me.lstOperation)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.optD)
        Me.Controls.Add(Me.optH)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblBase)
        Me.Controls.Add(Me.lblLoc)
        Me.Controls.Add(Me.lblGroup)
        Me.Controls.Add(Me.lblOperation)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAddExpert"
        Me.Text = "WinHSPF - Add Output"
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
End Class
