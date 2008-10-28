<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInputDataEditor
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
        Me.treUci = New System.Windows.Forms.TreeView
        Me.cmdClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'treUci
        '
        Me.treUci.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treUci.Location = New System.Drawing.Point(4, 9)
        Me.treUci.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.treUci.Name = "treUci"
        Me.treUci.Size = New System.Drawing.Size(233, 244)
        Me.treUci.TabIndex = 0
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(80, 262)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(80, 25)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'frmInputDataEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(242, 298)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.treUci)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmInputDataEditor"
        Me.Text = "WinHSPF - Input Data Editor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents treUci As System.Windows.Forms.TreeView
    Friend WithEvents cmdClose As System.Windows.Forms.Button
End Class
