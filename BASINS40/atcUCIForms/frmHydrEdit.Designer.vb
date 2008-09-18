<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHydrEdit
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
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.radio1n2 = New System.Windows.Forms.RadioButton
        Me.radio1n1 = New System.Windows.Forms.RadioButton
        Me.chkAssign = New System.Windows.Forms.CheckBox
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkAux3 = New System.Windows.Forms.CheckBox
        Me.chkAux2 = New System.Windows.Forms.CheckBox
        Me.chkAux1 = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.radio2n3 = New System.Windows.Forms.RadioButton
        Me.radio2n2 = New System.Windows.Forms.RadioButton
        Me.radio2n1 = New System.Windows.Forms.RadioButton
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.cboExit = New System.Windows.Forms.ComboBox
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton3 = New System.Windows.Forms.RadioButton
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.atxGt = New atcControls.atcText
        Me.atxFvol = New atcControls.atcText
        Me.AtcText2 = New atcControls.atcText
        Me.AtcText1 = New atcControls.atcText
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.radio1n2)
        Me.GroupBox7.Controls.Add(Me.radio1n1)
        Me.GroupBox7.Location = New System.Drawing.Point(14, 48)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(462, 57)
        Me.GroupBox7.TabIndex = 13
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "F(vol) outflow demand components (VCONFG)"
        '
        'radio1n2
        '
        Me.radio1n2.AutoSize = True
        Me.radio1n2.Location = New System.Drawing.Point(12, 34)
        Me.radio1n2.Name = "radio1n2"
        Me.radio1n2.Size = New System.Drawing.Size(85, 17)
        Me.radio1n2.TabIndex = 1
        Me.radio1n2.TabStop = True
        Me.radio1n2.Text = "Vary monthly"
        Me.radio1n2.UseVisualStyleBackColor = True
        '
        'radio1n1
        '
        Me.radio1n1.AutoSize = True
        Me.radio1n1.Location = New System.Drawing.Point(12, 17)
        Me.radio1n1.Name = "radio1n1"
        Me.radio1n1.Size = New System.Drawing.Size(67, 17)
        Me.radio1n1.TabIndex = 0
        Me.radio1n1.TabStop = True
        Me.radio1n1.Text = "Constant"
        Me.radio1n1.UseVisualStyleBackColor = True
        '
        'chkAssign
        '
        Me.chkAssign.AutoSize = True
        Me.chkAssign.Location = New System.Drawing.Point(301, 9)
        Me.chkAssign.Name = "chkAssign"
        Me.chkAssign.Size = New System.Drawing.Size(135, 17)
        Me.chkAssign.TabIndex = 12
        Me.chkAssign.Text = "Assign To All RCHRES"
        Me.chkAssign.UseVisualStyleBackColor = True
        '
        'cboReach
        '
        Me.cboReach.FormattingEnabled = True
        Me.cboReach.Location = New System.Drawing.Point(74, 7)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(212, 21)
        Me.cboReach.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "RCHRES:"
        '
        'chkAux3
        '
        Me.chkAux3.AutoSize = True
        Me.chkAux3.Location = New System.Drawing.Point(24, 169)
        Me.chkAux3.Name = "chkAux3"
        Me.chkAux3.Size = New System.Drawing.Size(290, 17)
        Me.chkAux3.TabIndex = 14
        Me.chkAux3.Text = "Calculate shear velocity and bed shear stress (AUX3FG)"
        Me.chkAux3.UseVisualStyleBackColor = True
        '
        'chkAux2
        '
        Me.chkAux2.AutoSize = True
        Me.chkAux2.Location = New System.Drawing.Point(24, 142)
        Me.chkAux2.Name = "chkAux2"
        Me.chkAux2.Size = New System.Drawing.Size(362, 17)
        Me.chkAux2.TabIndex = 15
        Me.chkAux2.Text = "Calculate average velocity and average cross-sectional area (AUX2FG)"
        Me.chkAux2.UseVisualStyleBackColor = True
        '
        'chkAux1
        '
        Me.chkAux1.AutoSize = True
        Me.chkAux1.Location = New System.Drawing.Point(24, 115)
        Me.chkAux1.Name = "chkAux1"
        Me.chkAux1.Size = New System.Drawing.Size(393, 17)
        Me.chkAux1.TabIndex = 16
        Me.chkAux1.Text = "Calculate depth, stage, surface area, average depth, and top width (AUX1FG)"
        Me.chkAux1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.atxGt)
        Me.GroupBox1.Controls.Add(Me.atxFvol)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Controls.Add(Me.cboExit)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 197)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(460, 254)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Exit"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.radio2n3)
        Me.GroupBox2.Controls.Add(Me.radio2n2)
        Me.GroupBox2.Controls.Add(Me.radio2n1)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 151)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(429, 80)
        Me.GroupBox2.TabIndex = 14
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Function used to combine outflow demand components (FUNCT)"
        '
        'radio2n3
        '
        Me.radio2n3.AutoSize = True
        Me.radio2n3.Location = New System.Drawing.Point(12, 51)
        Me.radio2n3.Name = "radio2n3"
        Me.radio2n3.Size = New System.Drawing.Size(128, 17)
        Me.radio2n3.TabIndex = 2
        Me.radio2n3.TabStop = True
        Me.radio2n3.Text = "Sum of F(vol) and G(t)"
        Me.radio2n3.UseVisualStyleBackColor = True
        '
        'radio2n2
        '
        Me.radio2n2.AutoSize = True
        Me.radio2n2.Location = New System.Drawing.Point(12, 34)
        Me.radio2n2.Name = "radio2n2"
        Me.radio2n2.Size = New System.Drawing.Size(137, 17)
        Me.radio2n2.TabIndex = 1
        Me.radio2n2.TabStop = True
        Me.radio2n2.Text = "Larger of F(vol) and G(t)"
        Me.radio2n2.UseVisualStyleBackColor = True
        '
        'radio2n1
        '
        Me.radio2n1.AutoSize = True
        Me.radio2n1.Location = New System.Drawing.Point(12, 17)
        Me.radio2n1.Name = "radio2n1"
        Me.radio2n1.Size = New System.Drawing.Size(141, 17)
        Me.radio2n1.TabIndex = 0
        Me.radio2n1.TabStop = True
        Me.radio2n1.Text = "Smaller of F(vol) and G(t)"
        Me.radio2n1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(242, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "G(t) component of the outflow demand (ODGTFG)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(249, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "F(vol) component of the outflow demand (ODFVFG)"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(238, 31)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(112, 17)
        Me.CheckBox1.TabIndex = 18
        Me.CheckBox1.Text = "Assign To All Exits"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'cboExit
        '
        Me.cboExit.FormattingEnabled = True
        Me.cboExit.Location = New System.Drawing.Point(11, 29)
        Me.cboExit.Name = "cboExit"
        Me.cboExit.Size = New System.Drawing.Size(212, 21)
        Me.cboExit.TabIndex = 18
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(234, 461)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(81, 26)
        Me.cmdCancel.TabIndex = 19
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(129, 461)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(81, 26)
        Me.cmdOK.TabIndex = 18
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(12, 17)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(141, 17)
        Me.RadioButton2.TabIndex = 0
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Smaller of F(vol) and G(t)"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(12, 51)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(128, 17)
        Me.RadioButton3.TabIndex = 2
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Sum of F(vol) and G(t)"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(11, 29)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(212, 21)
        Me.ComboBox1.TabIndex = 18
        '
        'atxGt
        '
        Me.atxGt.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxGt.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxGt.DefaultValue = ""
        Me.atxGt.HardMax = -999
        Me.atxGt.HardMin = -999
        Me.atxGt.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxGt.Location = New System.Drawing.Point(268, 96)
        Me.atxGt.MaxDecimal = -999
        Me.atxGt.maxWidth = -999
        Me.atxGt.Name = "atxGt"
        Me.atxGt.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxGt.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxGt.SelLength = 0
        Me.atxGt.SelStart = 0
        Me.atxGt.Size = New System.Drawing.Size(91, 25)
        Me.atxGt.SoftMax = -999
        Me.atxGt.SoftMin = -999
        Me.atxGt.TabIndex = 22
        '
        'atxFvol
        '
        Me.atxFvol.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxFvol.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxFvol.DefaultValue = ""
        Me.atxFvol.HardMax = -999
        Me.atxFvol.HardMin = -999
        Me.atxFvol.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxFvol.Location = New System.Drawing.Point(268, 65)
        Me.atxFvol.MaxDecimal = -999
        Me.atxFvol.maxWidth = -999
        Me.atxFvol.Name = "atxFvol"
        Me.atxFvol.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxFvol.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxFvol.SelLength = 0
        Me.atxFvol.SelStart = 0
        Me.atxFvol.Size = New System.Drawing.Size(91, 25)
        Me.atxFvol.SoftMax = -999
        Me.atxFvol.SoftMin = -999
        Me.atxFvol.TabIndex = 21
        '
        'AtcText2
        '
        Me.AtcText2.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText2.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText2.DefaultValue = ""
        Me.AtcText2.HardMax = -999
        Me.AtcText2.HardMin = -999
        Me.AtcText2.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText2.Location = New System.Drawing.Point(268, 96)
        Me.AtcText2.MaxDecimal = -999
        Me.AtcText2.maxWidth = -999
        Me.AtcText2.Name = "AtcText2"
        Me.AtcText2.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText2.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText2.SelLength = 0
        Me.AtcText2.SelStart = 0
        Me.AtcText2.Size = New System.Drawing.Size(91, 25)
        Me.AtcText2.SoftMax = -999
        Me.AtcText2.SoftMin = -999
        Me.AtcText2.TabIndex = 22
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.AtcText1.DefaultValue = ""
        Me.AtcText1.HardMax = -999
        Me.AtcText1.HardMin = -999
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText1.Location = New System.Drawing.Point(268, 65)
        Me.AtcText1.MaxDecimal = -999
        Me.AtcText1.maxWidth = -999
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText1.SelLength = 0
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(91, 25)
        Me.AtcText1.SoftMax = -999
        Me.AtcText1.SoftMin = -999
        Me.AtcText1.TabIndex = 21
        '
        'frmHydrEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(490, 500)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkAux1)
        Me.Controls.Add(Me.chkAux2)
        Me.Controls.Add(Me.chkAux3)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.chkAssign)
        Me.Controls.Add(Me.cboReach)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmHydrEdit"
        Me.Text = "Form1"
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents radio1n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1n1 As System.Windows.Forms.RadioButton
    Friend WithEvents chkAssign As System.Windows.Forms.CheckBox
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAux3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkAux2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkAux1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents cboExit As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents radio2n3 As System.Windows.Forms.RadioButton
    Friend WithEvents radio2n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio2n1 As System.Windows.Forms.RadioButton
    Friend WithEvents atxGt As atcControls.atcText
    Friend WithEvents atxFvol As atcControls.atcText
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents AtcText2 As atcControls.atcText
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
End Class
