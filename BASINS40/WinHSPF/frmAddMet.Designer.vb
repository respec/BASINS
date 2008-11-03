<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddMet
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
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.Cancel = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(53, 10)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(308, 21)
        Me.ComboBox1.TabIndex = 1
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
        Me.AtcGrid1.Location = New System.Drawing.Point(11, 43)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(488, 249)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 2
        '
        'Cancel
        '
        Me.Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Cancel.Location = New System.Drawing.Point(263, 313)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(76, 25)
        Me.Cancel.TabIndex = 8
        Me.Cancel.Text = "&Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button2.Location = New System.Drawing.Point(175, 313)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(76, 25)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "&OK"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmAddMet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 350)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.AtcGrid1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmAddMet"
        Me.Text = "WinHSPF - Add Met Segment"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
