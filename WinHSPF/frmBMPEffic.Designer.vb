<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBMPEffic
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
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdUpdateUCI = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "BMP Name:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(77, 10)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(254, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(346, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "BMP Operation <>"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Control
        Me.TextBox1.Location = New System.Drawing.Point(10, 50)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(508, 89)
        Me.TextBox1.TabIndex = 3
        Me.TextBox1.Text = "Reference: <not applicable>"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 157)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Removal Fractions"
        '
        'AtcGrid1
        '
        Me.AtcGrid1.AllowHorizontalScrolling = True
        Me.AtcGrid1.AllowNewValidValues = False
        Me.AtcGrid1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGrid1.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid1.Fixed3D = False
        Me.AtcGrid1.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineWidth = 0.0!
        Me.AtcGrid1.Location = New System.Drawing.Point(10, 176)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(508, 282)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 5
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(119, 471)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(101, 26)
        Me.cmdClose.TabIndex = 22
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdUpdateUCI
        '
        Me.cmdUpdateUCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdUpdateUCI.Location = New System.Drawing.Point(10, 471)
        Me.cmdUpdateUCI.Name = "cmdUpdateUCI"
        Me.cmdUpdateUCI.Size = New System.Drawing.Size(101, 26)
        Me.cmdUpdateUCI.TabIndex = 21
        Me.cmdUpdateUCI.Text = "&Update UCI"
        Me.cmdUpdateUCI.UseVisualStyleBackColor = True
        '
        'frmBMPEffic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(530, 516)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdUpdateUCI)
        Me.Controls.Add(Me.AtcGrid1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmBMPEffic"
        Me.Text = "WinHSPF - Best Management Practices Efficiency Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateUCI As System.Windows.Forms.Button
End Class
