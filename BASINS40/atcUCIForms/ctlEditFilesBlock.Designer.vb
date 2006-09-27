<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditFilesBlock
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.grdEdit = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'grdEdit
        '
        Me.grdEdit.AllowHorizontalScrolling = True
        Me.grdEdit.AllowNewValidValues = False
        Me.grdEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grdEdit.CellBackColor = System.Drawing.Color.Empty
        Me.grdEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grdEdit.LineColor = System.Drawing.Color.Empty
        Me.grdEdit.LineWidth = 0.0!
        Me.grdEdit.Location = New System.Drawing.Point(0, 0)
        Me.grdEdit.Name = "grdEdit"
        Me.grdEdit.Size = New System.Drawing.Size(552, 226)
        Me.grdEdit.Source = Nothing
        Me.grdEdit.TabIndex = 0
        '
        'ctlEditFilesBlock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grdEdit)
        Me.Name = "ctlEditFilesBlock"
        Me.Size = New System.Drawing.Size(552, 226)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdEdit As atcControls.atcGrid

End Class
