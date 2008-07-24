<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReach
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
        Me.grdReach = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'grdReach
        '
        Me.grdReach.AllowHorizontalScrolling = True
        Me.grdReach.AllowNewValidValues = False
        Me.grdReach.CellBackColor = System.Drawing.Color.Empty
        Me.grdReach.LineColor = System.Drawing.Color.Empty
        Me.grdReach.LineWidth = 0.0!
        Me.grdReach.Location = New System.Drawing.Point(13, 16)
        Me.grdReach.Name = "grdReach"
        Me.grdReach.Size = New System.Drawing.Size(448, 206)
        Me.grdReach.Source = Nothing
        Me.grdReach.TabIndex = 0
        '
        'frmReach
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 272)
        Me.Controls.Add(Me.grdReach)
        Me.Name = "frmReach"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdReach As atcControls.atcGrid
End Class
