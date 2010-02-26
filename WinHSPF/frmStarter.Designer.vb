<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStarter
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
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdStarter = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.agdStarter = New atcControls.atcGrid
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.Location = New System.Drawing.Point(195, 340)
        Me.cmdApply.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(117, 36)
        Me.cmdApply.TabIndex = 1
        Me.cmdApply.Text = "Apply to UCI"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'cmdStarter
        '
        Me.cmdStarter.Location = New System.Drawing.Point(320, 340)
        Me.cmdStarter.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdStarter.Name = "cmdStarter"
        Me.cmdStarter.Size = New System.Drawing.Size(117, 36)
        Me.cmdStarter.TabIndex = 2
        Me.cmdStarter.Text = "Set Starter"
        Me.cmdStarter.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(320, 396)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(135, 32)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(177, 396)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(135, 32)
        Me.cmdOK.TabIndex = 23
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'agdStarter
        '
        Me.agdStarter.AllowHorizontalScrolling = True
        Me.agdStarter.AllowNewValidValues = False
        Me.agdStarter.CellBackColor = System.Drawing.Color.Empty
        Me.agdStarter.Fixed3D = False
        Me.agdStarter.LineColor = System.Drawing.Color.Empty
        Me.agdStarter.LineWidth = 0.0!
        Me.agdStarter.Location = New System.Drawing.Point(17, 15)
        Me.agdStarter.Margin = New System.Windows.Forms.Padding(4)
        Me.agdStarter.Name = "agdStarter"
        Me.agdStarter.Size = New System.Drawing.Size(600, 318)
        Me.agdStarter.Source = Nothing
        Me.agdStarter.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmStarter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(636, 443)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdStarter)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.agdStarter)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmStarter"
        Me.Text = "WinHSPF - Starting Values Manager"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents agdStarter As atcControls.atcGrid
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdStarter As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
