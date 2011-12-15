<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIndex
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIndex))
        Me.lstIndices = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'lstIndices
        '
        Me.lstIndices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstIndices.FormattingEnabled = True
        Me.lstIndices.Location = New System.Drawing.Point(0, 0)
        Me.lstIndices.Name = "lstIndices"
        Me.lstIndices.ScrollAlwaysVisible = True
        Me.lstIndices.Size = New System.Drawing.Size(280, 329)
        Me.lstIndices.TabIndex = 0
        '
        'frmIndex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 337)
        Me.Controls.Add(Me.lstIndices)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmIndex"
        Me.Text = "Click on a recession index"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstIndices As System.Windows.Forms.ListBox
End Class
