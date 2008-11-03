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
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDefine = New System.Windows.Forms.TextBox
        Me.optH = New System.Windows.Forms.RadioButton
        Me.optD = New System.Windows.Forms.RadioButton
        Me.cmdClose = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'lblOperation
        '
        Me.lblOperation.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblOperation.AutoSize = True
        Me.lblOperation.Location = New System.Drawing.Point(12, 7)
        Me.lblOperation.Name = "lblOperation"
        Me.lblOperation.Size = New System.Drawing.Size(56, 13)
        Me.lblOperation.TabIndex = 0
        Me.lblOperation.Text = "Operation:"
        '
        'lblGroup
        '
        Me.lblGroup.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Location = New System.Drawing.Point(195, 7)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(82, 13)
        Me.lblGroup.TabIndex = 1
        Me.lblGroup.Text = "Group/Member:"
        '
        'TextBox4
        '
        Me.TextBox4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox4.Location = New System.Drawing.Point(168, 216)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(80, 20)
        Me.TextBox4.TabIndex = 6
        '
        'TextBox5
        '
        Me.TextBox5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox5.Location = New System.Drawing.Point(168, 242)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(80, 20)
        Me.TextBox5.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(68, 220)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "WDM Location ID:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(73, 246)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Base WDM DSN:"
        '
        'txtDefine
        '
        Me.txtDefine.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtDefine.BackColor = System.Drawing.SystemColors.Control
        Me.txtDefine.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefine.Location = New System.Drawing.Point(15, 150)
        Me.txtDefine.Multiline = True
        Me.txtDefine.Name = "txtDefine"
        Me.txtDefine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDefine.Size = New System.Drawing.Size(350, 49)
        Me.txtDefine.TabIndex = 10
        '
        'optH
        '
        Me.optH.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.optH.AutoSize = True
        Me.optH.Location = New System.Drawing.Point(254, 221)
        Me.optH.Name = "optH"
        Me.optH.Size = New System.Drawing.Size(55, 17)
        Me.optH.TabIndex = 11
        Me.optH.TabStop = True
        Me.optH.Text = "Hourly"
        Me.optH.UseVisualStyleBackColor = True
        '
        'optD
        '
        Me.optD.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.optD.AutoSize = True
        Me.optD.Location = New System.Drawing.Point(254, 242)
        Me.optD.Name = "optD"
        Me.optD.Size = New System.Drawing.Size(48, 17)
        Me.optD.TabIndex = 12
        Me.optD.TabStop = True
        Me.optD.Text = "Daily"
        Me.optD.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cmdClose.Location = New System.Drawing.Point(102, 287)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 24)
        Me.cmdClose.TabIndex = 13
        Me.cmdClose.Text = "&OK"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Button1.Location = New System.Drawing.Point(200, 287)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 24)
        Me.Button1.TabIndex = 14
        Me.Button1.Text = "&Close"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(15, 27)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(167, 108)
        Me.ListBox1.TabIndex = 15
        '
        'ListBox2
        '
        Me.ListBox2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(198, 27)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(167, 108)
        Me.ListBox2.TabIndex = 16
        '
        'frmAddExpert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(377, 322)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.optD)
        Me.Controls.Add(Me.optH)
        Me.Controls.Add(Me.txtDefine)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.lblGroup)
        Me.Controls.Add(Me.lblOperation)
        Me.Name = "frmAddExpert"
        Me.Text = "WinHSPF - Add Output"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblOperation As System.Windows.Forms.Label
    Friend WithEvents lblGroup As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDefine As System.Windows.Forms.TextBox
    Friend WithEvents optH As System.Windows.Forms.RadioButton
    Friend WithEvents optD As System.Windows.Forms.RadioButton
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
End Class
