<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOutput
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.radio4 = New System.Windows.Forms.RadioButton
        Me.radio3 = New System.Windows.Forms.RadioButton
        Me.radio2 = New System.Windows.Forms.RadioButton
        Me.radio1 = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.agdOutput = New atcControls.atcGrid
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdCopy = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.txtDesc)
        Me.GroupBox1.Controls.Add(Me.radio4)
        Me.GroupBox1.Controls.Add(Me.radio3)
        Me.GroupBox1.Controls.Add(Me.radio2)
        Me.GroupBox1.Controls.Add(Me.radio1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(426, 171)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Output Type"
        '
        'txtDesc
        '
        Me.txtDesc.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDesc.Location = New System.Drawing.Point(181, 24)
        Me.txtDesc.Multiline = True
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(199, 132)
        Me.txtDesc.TabIndex = 4
        '
        'radio4
        '
        Me.radio4.AutoSize = True
        Me.radio4.Location = New System.Drawing.Point(27, 93)
        Me.radio4.Name = "radio4"
        Me.radio4.Size = New System.Drawing.Size(51, 17)
        Me.radio4.TabIndex = 3
        Me.radio4.TabStop = True
        Me.radio4.Text = "Other"
        Me.radio4.UseVisualStyleBackColor = True
        '
        'radio3
        '
        Me.radio3.AutoSize = True
        Me.radio3.Location = New System.Drawing.Point(27, 70)
        Me.radio3.Name = "radio3"
        Me.radio3.Size = New System.Drawing.Size(118, 17)
        Me.radio3.TabIndex = 2
        Me.radio3.TabStop = True
        Me.radio3.Text = "AQUATOX Linkage"
        Me.radio3.UseVisualStyleBackColor = True
        '
        'radio2
        '
        Me.radio2.AutoSize = True
        Me.radio2.Location = New System.Drawing.Point(27, 47)
        Me.radio2.Name = "radio2"
        Me.radio2.Size = New System.Drawing.Size(47, 17)
        Me.radio2.TabIndex = 1
        Me.radio2.TabStop = True
        Me.radio2.Text = "Flow"
        Me.radio2.UseVisualStyleBackColor = True
        '
        'radio1
        '
        Me.radio1.AutoSize = True
        Me.radio1.Location = New System.Drawing.Point(27, 24)
        Me.radio1.Name = "radio1"
        Me.radio1.Size = New System.Drawing.Size(124, 17)
        Me.radio1.TabIndex = 0
        Me.radio1.TabStop = True
        Me.radio1.Text = "Hydrology Calibration"
        Me.radio1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.cmdCopy)
        Me.GroupBox2.Controls.Add(Me.cmdRemove)
        Me.GroupBox2.Controls.Add(Me.cmdAdd)
        Me.GroupBox2.Controls.Add(Me.agdOutput)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 197)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(426, 260)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Output Locations"
        '
        'cmdRemove
        '
        Me.cmdRemove.Location = New System.Drawing.Point(82, 234)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(60, 20)
        Me.cmdRemove.TabIndex = 2
        Me.cmdRemove.Text = "&Remove"
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(15, 234)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(60, 20)
        Me.cmdAdd.TabIndex = 1
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'agdOutput
        '
        Me.agdOutput.AllowHorizontalScrolling = True
        Me.agdOutput.AllowNewValidValues = False
        Me.agdOutput.CellBackColor = System.Drawing.Color.Empty
        Me.agdOutput.Fixed3D = False
        Me.agdOutput.LineColor = System.Drawing.Color.Empty
        Me.agdOutput.LineWidth = 0.0!
        Me.agdOutput.Location = New System.Drawing.Point(15, 21)
        Me.agdOutput.Name = "agdOutput"
        Me.agdOutput.Size = New System.Drawing.Size(396, 205)
        Me.agdOutput.Source = Nothing
        Me.agdOutput.TabIndex = 0
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdClose.Location = New System.Drawing.Point(176, 477)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(102, 28)
        Me.cmdClose.TabIndex = 6
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdCopy
        '
        Me.cmdCopy.Location = New System.Drawing.Point(149, 234)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.Size = New System.Drawing.Size(60, 20)
        Me.cmdCopy.TabIndex = 3
        Me.cmdCopy.Text = "Copy"
        Me.cmdCopy.UseVisualStyleBackColor = True
        '
        'frmOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 512)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmOutput"
        Me.Text = "frmOutput"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents radio4 As System.Windows.Forms.RadioButton
    Friend WithEvents radio3 As System.Windows.Forms.RadioButton
    Friend WithEvents radio2 As System.Windows.Forms.RadioButton
    Friend WithEvents radio1 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents agdOutput As atcControls.atcGrid
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdCopy As System.Windows.Forms.Button
End Class
