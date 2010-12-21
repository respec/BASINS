<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDFLOWExcursions
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
        Me.btnCopy2Clipbrd = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'btnCopy2Clipbrd
        '
        Me.btnCopy2Clipbrd.Location = New System.Drawing.Point(21, 233)
        Me.btnCopy2Clipbrd.Name = "btnCopy2Clipbrd"
        Me.btnCopy2Clipbrd.Size = New System.Drawing.Size(113, 23)
        Me.btnCopy2Clipbrd.TabIndex = 1
        Me.btnCopy2Clipbrd.Text = "Copy to Clipboard"
        Me.btnCopy2Clipbrd.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(154, 233)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(108, 23)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'AtcGrid1
        '
        Me.AtcGrid1.AllowHorizontalScrolling = True
        Me.AtcGrid1.AllowNewValidValues = False
        Me.AtcGrid1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.AtcGrid1.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineWidth = 0.0!
        Me.AtcGrid1.Location = New System.Drawing.Point(21, 22)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(320, 200)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 0
        '
        'frmDFLOWExcursions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(426, 266)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCopy2Clipbrd)
        Me.Controls.Add(Me.AtcGrid1)
        Me.Name = "frmDFLOWExcursions"
        Me.Text = "frmDFLOWExcursions"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents btnCopy2Clipbrd As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
