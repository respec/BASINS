<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlSchematic
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.picTree = New System.Windows.Forms.PictureBox
        Me.btnScrollLegendUp = New System.Windows.Forms.Button
        Me.btnScrollLegendDown = New System.Windows.Forms.Button
        CType(Me.picTree, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picTree
        '
        Me.picTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picTree.Location = New System.Drawing.Point(0, 0)
        Me.picTree.Name = "picTree"
        Me.picTree.Size = New System.Drawing.Size(150, 150)
        Me.picTree.TabIndex = 0
        Me.picTree.TabStop = False
        '
        'btnScrollLegendUp
        '
        Me.btnScrollLegendUp.Location = New System.Drawing.Point(0, 0)
        Me.btnScrollLegendUp.Name = "btnScrollLegendUp"
        Me.btnScrollLegendUp.Size = New System.Drawing.Size(117, 23)
        Me.btnScrollLegendUp.TabIndex = 1
        Me.btnScrollLegendUp.Text = "ScrollLegendUp"
        Me.btnScrollLegendUp.UseVisualStyleBackColor = True
        '
        'btnScrollLegendDown
        '
        Me.btnScrollLegendDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScrollLegendDown.Location = New System.Drawing.Point(0, 127)
        Me.btnScrollLegendDown.Name = "btnScrollLegendDown"
        Me.btnScrollLegendDown.Size = New System.Drawing.Size(117, 23)
        Me.btnScrollLegendDown.TabIndex = 2
        Me.btnScrollLegendDown.Text = "ScrollLegendDown"
        Me.btnScrollLegendDown.UseVisualStyleBackColor = True
        '
        'ctlSchematic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnScrollLegendDown)
        Me.Controls.Add(Me.btnScrollLegendUp)
        Me.Controls.Add(Me.picTree)
        Me.Name = "ctlSchematic"
        CType(Me.picTree, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picTree As System.Windows.Forms.PictureBox
    Friend WithEvents btnScrollLegendUp As System.Windows.Forms.Button
    Friend WithEvents btnScrollLegendDown As System.Windows.Forms.Button

End Class
