<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectScript
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectScript))
        Me.lstScripts = New System.Windows.Forms.ListBox
        Me.cmdRun = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lstScripts
        '
        Me.lstScripts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstScripts.FormattingEnabled = True
        Me.lstScripts.ItemHeight = 16
        Me.lstScripts.Location = New System.Drawing.Point(12, 12)
        Me.lstScripts.Name = "lstScripts"
        Me.lstScripts.Size = New System.Drawing.Size(251, 388)
        Me.lstScripts.TabIndex = 0
        '
        'cmdRun
        '
        Me.cmdRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRun.Location = New System.Drawing.Point(55, 419)
        Me.cmdRun.Name = "cmdRun"
        Me.cmdRun.Size = New System.Drawing.Size(63, 26)
        Me.cmdRun.TabIndex = 1
        Me.cmdRun.Text = "Run"
        Me.cmdRun.UseVisualStyleBackColor = True
        '
        'frmSelectScript
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(275, 457)
        Me.Controls.Add(Me.cmdRun)
        Me.Controls.Add(Me.lstScripts)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSelectScript"
        Me.Text = "Select Script"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstScripts As System.Windows.Forms.ListBox
    Friend WithEvents cmdRun As System.Windows.Forms.Button
End Class
