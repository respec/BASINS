<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPwatEdit
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboLand = New System.Windows.Forms.ComboBox
        Me.chkAssign = New System.Windows.Forms.CheckBox
        Me.chkSnow = New System.Windows.Forms.CheckBox
        Me.chkHigh = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.radio1n4 = New System.Windows.Forms.RadioButton
        Me.radio1n3 = New System.Windows.Forms.RadioButton
        Me.radio1n2 = New System.Windows.Forms.RadioButton
        Me.radio1n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.radio6n2 = New System.Windows.Forms.RadioButton
        Me.radio6n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.radio7n2 = New System.Windows.Forms.RadioButton
        Me.radio7n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.radio8n2 = New System.Windows.Forms.RadioButton
        Me.radio8n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.radio9n2 = New System.Windows.Forms.RadioButton
        Me.radio9n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.radio10n4 = New System.Windows.Forms.RadioButton
        Me.radio10n3 = New System.Windows.Forms.RadioButton
        Me.radio10n2 = New System.Windows.Forms.RadioButton
        Me.radio10n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.radio2n2 = New System.Windows.Forms.RadioButton
        Me.radio2n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.radio3n2 = New System.Windows.Forms.RadioButton
        Me.radio3n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.radio4n2 = New System.Windows.Forms.RadioButton
        Me.radio4n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.radio5n2 = New System.Windows.Forms.RadioButton
        Me.radio5n1 = New System.Windows.Forms.RadioButton
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Land Use"
        '
        'cboLand
        '
        Me.cboLand.FormattingEnabled = True
        Me.cboLand.Location = New System.Drawing.Point(68, 18)
        Me.cboLand.Name = "cboLand"
        Me.cboLand.Size = New System.Drawing.Size(212, 21)
        Me.cboLand.TabIndex = 1
        '
        'chkAssign
        '
        Me.chkAssign.AutoSize = True
        Me.chkAssign.Location = New System.Drawing.Point(288, 20)
        Me.chkAssign.Name = "chkAssign"
        Me.chkAssign.Size = New System.Drawing.Size(87, 17)
        Me.chkAssign.TabIndex = 2
        Me.chkAssign.Text = "Assign To All"
        Me.chkAssign.UseVisualStyleBackColor = True
        '
        'chkSnow
        '
        Me.chkSnow.AutoSize = True
        Me.chkSnow.Location = New System.Drawing.Point(12, 53)
        Me.chkSnow.Name = "chkSnow"
        Me.chkSnow.Size = New System.Drawing.Size(199, 17)
        Me.chkSnow.TabIndex = 3
        Me.chkSnow.Text = "Use snow simulation data (CSNOFG)"
        Me.chkSnow.UseVisualStyleBackColor = True
        '
        'chkHigh
        '
        Me.chkHigh.AutoSize = True
        Me.chkHigh.Location = New System.Drawing.Point(288, 53)
        Me.chkHigh.Name = "chkHigh"
        Me.chkHigh.Size = New System.Drawing.Size(211, 17)
        Me.chkHigh.TabIndex = 4
        Me.chkHigh.Text = "High Water Table Conditions (HWTFG)"
        Me.chkHigh.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radio1n4)
        Me.GroupBox1.Controls.Add(Me.radio1n3)
        Me.GroupBox1.Controls.Add(Me.radio1n2)
        Me.GroupBox1.Controls.Add(Me.radio1n1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 81)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(270, 87)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Overland flow routing (RTOPFG)"
        '
        'radio1n4
        '
        Me.radio1n4.AutoSize = True
        Me.radio1n4.Location = New System.Drawing.Point(12, 66)
        Me.radio1n4.Name = "radio1n4"
        Me.radio1n4.Size = New System.Drawing.Size(96, 17)
        Me.radio1n4.TabIndex = 3
        Me.radio1n4.TabStop = True
        Me.radio1n4.Text = "FTable method"
        Me.radio1n4.UseVisualStyleBackColor = True
        '
        'radio1n3
        '
        Me.radio1n3.AutoSize = True
        Me.radio1n3.Location = New System.Drawing.Point(12, 49)
        Me.radio1n3.Name = "radio1n3"
        Me.radio1n3.Size = New System.Drawing.Size(134, 17)
        Me.radio1n3.TabIndex = 2
        Me.radio1n3.TabStop = True
        Me.radio1n3.Text = "Power function method"
        Me.radio1n3.UseVisualStyleBackColor = True
        '
        'radio1n2
        '
        Me.radio1n2.AutoSize = True
        Me.radio1n2.Location = New System.Drawing.Point(12, 32)
        Me.radio1n2.Name = "radio1n2"
        Me.radio1n2.Size = New System.Drawing.Size(114, 17)
        Me.radio1n2.TabIndex = 1
        Me.radio1n2.TabStop = True
        Me.radio1n2.Text = "Alternative Method"
        Me.radio1n2.UseVisualStyleBackColor = True
        '
        'radio1n1
        '
        Me.radio1n1.AutoSize = True
        Me.radio1n1.Location = New System.Drawing.Point(12, 15)
        Me.radio1n1.Name = "radio1n1"
        Me.radio1n1.Size = New System.Drawing.Size(180, 17)
        Me.radio1n1.TabIndex = 0
        Me.radio1n1.TabStop = True
        Me.radio1n1.Text = "Method in HSPX, ARM and NPS"
        Me.radio1n1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.radio6n2)
        Me.GroupBox2.Controls.Add(Me.radio6n1)
        Me.GroupBox2.Location = New System.Drawing.Point(288, 81)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Interflow inflow parameter (VIFWFG)"
        '
        'radio6n2
        '
        Me.radio6n2.AutoSize = True
        Me.radio6n2.Location = New System.Drawing.Point(12, 34)
        Me.radio6n2.Name = "radio6n2"
        Me.radio6n2.Size = New System.Drawing.Size(85, 17)
        Me.radio6n2.TabIndex = 1
        Me.radio6n2.TabStop = True
        Me.radio6n2.Text = "Vary monthly"
        Me.radio6n2.UseVisualStyleBackColor = True
        '
        'radio6n1
        '
        Me.radio6n1.AutoSize = True
        Me.radio6n1.Location = New System.Drawing.Point(12, 17)
        Me.radio6n1.Name = "radio6n1"
        Me.radio6n1.Size = New System.Drawing.Size(67, 17)
        Me.radio6n1.TabIndex = 0
        Me.radio6n1.TabStop = True
        Me.radio6n1.Text = "Constant"
        Me.radio6n1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.radio7n2)
        Me.GroupBox3.Controls.Add(Me.radio7n1)
        Me.GroupBox3.Location = New System.Drawing.Point(288, 144)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Interflow recession const (VIRCFG)"
        '
        'radio7n2
        '
        Me.radio7n2.AutoSize = True
        Me.radio7n2.Location = New System.Drawing.Point(12, 34)
        Me.radio7n2.Name = "radio7n2"
        Me.radio7n2.Size = New System.Drawing.Size(85, 17)
        Me.radio7n2.TabIndex = 1
        Me.radio7n2.TabStop = True
        Me.radio7n2.Text = "Vary monthly"
        Me.radio7n2.UseVisualStyleBackColor = True
        '
        'radio7n1
        '
        Me.radio7n1.AutoSize = True
        Me.radio7n1.Location = New System.Drawing.Point(12, 17)
        Me.radio7n1.Name = "radio7n1"
        Me.radio7n1.Size = New System.Drawing.Size(67, 17)
        Me.radio7n1.TabIndex = 0
        Me.radio7n1.TabStop = True
        Me.radio7n1.Text = "Constant"
        Me.radio7n1.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.radio8n2)
        Me.GroupBox4.Controls.Add(Me.radio8n1)
        Me.GroupBox4.Location = New System.Drawing.Point(288, 207)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox4.TabIndex = 8
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Lower Zone ET parameter (VLEFG)"
        '
        'radio8n2
        '
        Me.radio8n2.AutoSize = True
        Me.radio8n2.Location = New System.Drawing.Point(12, 34)
        Me.radio8n2.Name = "radio8n2"
        Me.radio8n2.Size = New System.Drawing.Size(85, 17)
        Me.radio8n2.TabIndex = 1
        Me.radio8n2.TabStop = True
        Me.radio8n2.Text = "Vary monthly"
        Me.radio8n2.UseVisualStyleBackColor = True
        '
        'radio8n1
        '
        Me.radio8n1.AutoSize = True
        Me.radio8n1.Location = New System.Drawing.Point(12, 17)
        Me.radio8n1.Name = "radio8n1"
        Me.radio8n1.Size = New System.Drawing.Size(67, 17)
        Me.radio8n1.TabIndex = 0
        Me.radio8n1.TabStop = True
        Me.radio8n1.Text = "Constant"
        Me.radio8n1.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.radio9n2)
        Me.GroupBox5.Controls.Add(Me.radio9n1)
        Me.GroupBox5.Location = New System.Drawing.Point(288, 270)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Frozen ground infiltration computation (IFFCFG)"
        '
        'radio9n2
        '
        Me.radio9n2.AutoSize = True
        Me.radio9n2.Location = New System.Drawing.Point(12, 34)
        Me.radio9n2.Name = "radio9n2"
        Me.radio9n2.Size = New System.Drawing.Size(128, 17)
        Me.radio9n2.TabIndex = 1
        Me.radio9n2.TabStop = True
        Me.radio9n2.Text = "Soil temperature basis"
        Me.radio9n2.UseVisualStyleBackColor = True
        '
        'radio9n1
        '
        Me.radio9n1.AutoSize = True
        Me.radio9n1.Location = New System.Drawing.Point(12, 17)
        Me.radio9n1.Name = "radio9n1"
        Me.radio9n1.Size = New System.Drawing.Size(151, 17)
        Me.radio9n1.TabIndex = 0
        Me.radio9n1.TabStop = True
        Me.radio9n1.Text = "Ice in the snow pack basis"
        Me.radio9n1.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.radio10n4)
        Me.GroupBox6.Controls.Add(Me.radio10n3)
        Me.GroupBox6.Controls.Add(Me.radio10n2)
        Me.GroupBox6.Controls.Add(Me.radio10n1)
        Me.GroupBox6.Location = New System.Drawing.Point(288, 333)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(270, 87)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Irrigation demand method (IRRGFG)"
        '
        'radio10n4
        '
        Me.radio10n4.AutoSize = True
        Me.radio10n4.Location = New System.Drawing.Point(12, 66)
        Me.radio10n4.Name = "radio10n4"
        Me.radio10n4.Size = New System.Drawing.Size(145, 17)
        Me.radio10n4.TabIndex = 3
        Me.radio10n4.TabStop = True
        Me.radio10n4.Text = "Schedule defined by user"
        Me.radio10n4.UseVisualStyleBackColor = True
        '
        'radio10n3
        '
        Me.radio10n3.AutoSize = True
        Me.radio10n3.Location = New System.Drawing.Point(12, 49)
        Me.radio10n3.Name = "radio10n3"
        Me.radio10n3.Size = New System.Drawing.Size(172, 17)
        Me.radio10n3.TabIndex = 2
        Me.radio10n3.TabStop = True
        Me.radio10n3.Text = "Allowable water depletion basis"
        Me.radio10n3.UseVisualStyleBackColor = True
        '
        'radio10n2
        '
        Me.radio10n2.AutoSize = True
        Me.radio10n2.Location = New System.Drawing.Point(12, 32)
        Me.radio10n2.Name = "radio10n2"
        Me.radio10n2.Size = New System.Drawing.Size(140, 17)
        Me.radio10n2.TabIndex = 1
        Me.radio10n2.TabStop = True
        Me.radio10n2.Text = "From external time series"
        Me.radio10n2.UseVisualStyleBackColor = True
        '
        'radio10n1
        '
        Me.radio10n1.AutoSize = True
        Me.radio10n1.Location = New System.Drawing.Point(12, 15)
        Me.radio10n1.Name = "radio10n1"
        Me.radio10n1.Size = New System.Drawing.Size(85, 17)
        Me.radio10n1.TabIndex = 0
        Me.radio10n1.TabStop = True
        Me.radio10n1.Text = "No demands"
        Me.radio10n1.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.radio2n2)
        Me.GroupBox7.Controls.Add(Me.radio2n1)
        Me.GroupBox7.Location = New System.Drawing.Point(12, 173)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox7.TabIndex = 7
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Upper Zone inflow computation (UZFG)"
        '
        'radio2n2
        '
        Me.radio2n2.AutoSize = True
        Me.radio2n2.Location = New System.Drawing.Point(12, 34)
        Me.radio2n2.Name = "radio2n2"
        Me.radio2n2.Size = New System.Drawing.Size(113, 17)
        Me.radio2n2.TabIndex = 1
        Me.radio2n2.TabStop = True
        Me.radio2n2.Text = "Alternative method"
        Me.radio2n2.UseVisualStyleBackColor = True
        '
        'radio2n1
        '
        Me.radio2n1.AutoSize = True
        Me.radio2n1.Location = New System.Drawing.Point(12, 17)
        Me.radio2n1.Name = "radio2n1"
        Me.radio2n1.Size = New System.Drawing.Size(180, 17)
        Me.radio2n1.TabIndex = 0
        Me.radio2n1.TabStop = True
        Me.radio2n1.Text = "Method in HSPX, ARM and NPS"
        Me.radio2n1.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.radio3n2)
        Me.GroupBox8.Controls.Add(Me.radio3n1)
        Me.GroupBox8.Location = New System.Drawing.Point(12, 235)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox8.TabIndex = 8
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Interception storage capacity (VCSFG)"
        '
        'radio3n2
        '
        Me.radio3n2.AutoSize = True
        Me.radio3n2.Location = New System.Drawing.Point(12, 34)
        Me.radio3n2.Name = "radio3n2"
        Me.radio3n2.Size = New System.Drawing.Size(85, 17)
        Me.radio3n2.TabIndex = 1
        Me.radio3n2.TabStop = True
        Me.radio3n2.Text = "Vary monthly"
        Me.radio3n2.UseVisualStyleBackColor = True
        '
        'radio3n1
        '
        Me.radio3n1.AutoSize = True
        Me.radio3n1.Location = New System.Drawing.Point(12, 17)
        Me.radio3n1.Name = "radio3n1"
        Me.radio3n1.Size = New System.Drawing.Size(67, 17)
        Me.radio3n1.TabIndex = 0
        Me.radio3n1.TabStop = True
        Me.radio3n1.Text = "Constant"
        Me.radio3n1.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.radio4n2)
        Me.GroupBox9.Controls.Add(Me.radio4n1)
        Me.GroupBox9.Location = New System.Drawing.Point(12, 297)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox9.TabIndex = 9
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Upper Zone nominal storage (VUZFG)"
        '
        'radio4n2
        '
        Me.radio4n2.AutoSize = True
        Me.radio4n2.Location = New System.Drawing.Point(12, 34)
        Me.radio4n2.Name = "radio4n2"
        Me.radio4n2.Size = New System.Drawing.Size(85, 17)
        Me.radio4n2.TabIndex = 1
        Me.radio4n2.TabStop = True
        Me.radio4n2.Text = "Vary monthly"
        Me.radio4n2.UseVisualStyleBackColor = True
        '
        'radio4n1
        '
        Me.radio4n1.AutoSize = True
        Me.radio4n1.Location = New System.Drawing.Point(12, 17)
        Me.radio4n1.Name = "radio4n1"
        Me.radio4n1.Size = New System.Drawing.Size(67, 17)
        Me.radio4n1.TabIndex = 0
        Me.radio4n1.TabStop = True
        Me.radio4n1.Text = "Constant"
        Me.radio4n1.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.radio5n2)
        Me.GroupBox10.Controls.Add(Me.radio5n1)
        Me.GroupBox10.Location = New System.Drawing.Point(12, 363)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(270, 57)
        Me.GroupBox10.TabIndex = 10
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Manning's n for overland flow plane (VMNFG)"
        '
        'radio5n2
        '
        Me.radio5n2.AutoSize = True
        Me.radio5n2.Location = New System.Drawing.Point(12, 34)
        Me.radio5n2.Name = "radio5n2"
        Me.radio5n2.Size = New System.Drawing.Size(85, 17)
        Me.radio5n2.TabIndex = 1
        Me.radio5n2.TabStop = True
        Me.radio5n2.Text = "Vary monthly"
        Me.radio5n2.UseVisualStyleBackColor = True
        '
        'radio5n1
        '
        Me.radio5n1.AutoSize = True
        Me.radio5n1.Location = New System.Drawing.Point(12, 17)
        Me.radio5n1.Name = "radio5n1"
        Me.radio5n1.Size = New System.Drawing.Size(67, 17)
        Me.radio5n1.TabIndex = 0
        Me.radio5n1.TabStop = True
        Me.radio5n1.Text = "Constant"
        Me.radio5n1.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(192, 444)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(81, 26)
        Me.cmdOK.TabIndex = 11
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(297, 444)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(81, 26)
        Me.cmdCancel.TabIndex = 12
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'frmPwatEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 480)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkHigh)
        Me.Controls.Add(Me.chkSnow)
        Me.Controls.Add(Me.chkAssign)
        Me.Controls.Add(Me.cboLand)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmPwatEdit"
        Me.Text = "frmPwatEdit"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboLand As System.Windows.Forms.ComboBox
    Friend WithEvents chkAssign As System.Windows.Forms.CheckBox
    Friend WithEvents chkSnow As System.Windows.Forms.CheckBox
    Friend WithEvents chkHigh As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radio1n4 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1n3 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents radio6n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio6n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents radio7n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio7n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents radio8n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio8n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents radio9n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio9n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents radio10n4 As System.Windows.Forms.RadioButton
    Friend WithEvents radio10n3 As System.Windows.Forms.RadioButton
    Friend WithEvents radio10n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio10n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents radio2n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio2n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents radio3n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio3n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents radio4n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio4n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents radio5n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio5n1 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
End Class
