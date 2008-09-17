<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIwatEdit
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
        Me.chkSnow = New System.Windows.Forms.CheckBox
        Me.chkAssign = New System.Windows.Forms.CheckBox
        Me.cboLand = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkLateral = New System.Windows.Forms.CheckBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.radio1n2 = New System.Windows.Forms.RadioButton
        Me.radio1n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.radio2n2 = New System.Windows.Forms.RadioButton
        Me.radio2n1 = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.radio3n2 = New System.Windows.Forms.RadioButton
        Me.radio3n1 = New System.Windows.Forms.RadioButton
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkSnow
        '
        Me.chkSnow.AutoSize = True
        Me.chkSnow.Location = New System.Drawing.Point(18, 41)
        Me.chkSnow.Name = "chkSnow"
        Me.chkSnow.Size = New System.Drawing.Size(199, 17)
        Me.chkSnow.TabIndex = 7
        Me.chkSnow.Text = "Use snow simulation data (CSNOFG)"
        Me.chkSnow.UseVisualStyleBackColor = True
        '
        'chkAssign
        '
        Me.chkAssign.AutoSize = True
        Me.chkAssign.Location = New System.Drawing.Point(292, 9)
        Me.chkAssign.Name = "chkAssign"
        Me.chkAssign.Size = New System.Drawing.Size(87, 17)
        Me.chkAssign.TabIndex = 6
        Me.chkAssign.Text = "Assign To All"
        Me.chkAssign.UseVisualStyleBackColor = True
        '
        'cboLand
        '
        Me.cboLand.FormattingEnabled = True
        Me.cboLand.Location = New System.Drawing.Point(74, 7)
        Me.cboLand.Name = "cboLand"
        Me.cboLand.Size = New System.Drawing.Size(212, 21)
        Me.cboLand.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Land Use"
        '
        'chkLateral
        '
        Me.chkLateral.AutoSize = True
        Me.chkLateral.Location = New System.Drawing.Point(18, 73)
        Me.chkLateral.Name = "chkLateral"
        Me.chkLateral.Size = New System.Drawing.Size(266, 17)
        Me.chkLateral.TabIndex = 8
        Me.chkLateral.Text = "Lateral surface inflow subject to retention (RTLIFG)"
        Me.chkLateral.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.radio1n2)
        Me.GroupBox7.Controls.Add(Me.radio1n1)
        Me.GroupBox7.Location = New System.Drawing.Point(16, 105)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(361, 57)
        Me.GroupBox7.TabIndex = 9
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Overland flow routing (RTOPFG)"
        '
        'radio1n2
        '
        Me.radio1n2.AutoSize = True
        Me.radio1n2.Location = New System.Drawing.Point(12, 34)
        Me.radio1n2.Name = "radio1n2"
        Me.radio1n2.Size = New System.Drawing.Size(113, 17)
        Me.radio1n2.TabIndex = 1
        Me.radio1n2.TabStop = True
        Me.radio1n2.Text = "Alternative method"
        Me.radio1n2.UseVisualStyleBackColor = True
        '
        'radio1n1
        '
        Me.radio1n1.AutoSize = True
        Me.radio1n1.Location = New System.Drawing.Point(12, 17)
        Me.radio1n1.Name = "radio1n1"
        Me.radio1n1.Size = New System.Drawing.Size(97, 17)
        Me.radio1n1.TabIndex = 0
        Me.radio1n1.TabStop = True
        Me.radio1n1.Text = "Method in NPS"
        Me.radio1n1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.radio2n2)
        Me.GroupBox1.Controls.Add(Me.radio2n1)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 178)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(361, 57)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Retention storage capacity (VRSFG)"
        '
        'radio2n2
        '
        Me.radio2n2.AutoSize = True
        Me.radio2n2.Location = New System.Drawing.Point(12, 34)
        Me.radio2n2.Name = "radio2n2"
        Me.radio2n2.Size = New System.Drawing.Size(85, 17)
        Me.radio2n2.TabIndex = 1
        Me.radio2n2.TabStop = True
        Me.radio2n2.Text = "Vary monthly"
        Me.radio2n2.UseVisualStyleBackColor = True
        '
        'radio2n1
        '
        Me.radio2n1.AutoSize = True
        Me.radio2n1.Location = New System.Drawing.Point(12, 17)
        Me.radio2n1.Name = "radio2n1"
        Me.radio2n1.Size = New System.Drawing.Size(67, 17)
        Me.radio2n1.TabIndex = 0
        Me.radio2n1.TabStop = True
        Me.radio2n1.Text = "Constant"
        Me.radio2n1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.radio3n2)
        Me.GroupBox2.Controls.Add(Me.radio3n1)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 251)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(361, 57)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Manning's n for overland flow plane (VMNFG)"
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
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(208, 322)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(81, 26)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(103, 322)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(81, 26)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'frmIwatEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(393, 360)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.chkLateral)
        Me.Controls.Add(Me.chkSnow)
        Me.Controls.Add(Me.chkAssign)
        Me.Controls.Add(Me.cboLand)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmIwatEdit"
        Me.Text = "frmIwatEdit"
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkSnow As System.Windows.Forms.CheckBox
    Friend WithEvents chkAssign As System.Windows.Forms.CheckBox
    Friend WithEvents cboLand As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkLateral As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents radio1n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents radio2n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio2n1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents radio3n2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio3n1 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
