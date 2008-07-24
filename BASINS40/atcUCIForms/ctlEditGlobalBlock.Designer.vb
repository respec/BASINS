<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditGlobalBlock
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
        Me.lblRunInfo = New System.Windows.Forms.Label
        Me.txtRunInfo = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblRunInfo
        '
        Me.lblRunInfo.AutoSize = True
        Me.lblRunInfo.Location = New System.Drawing.Point(15, 15)
        Me.lblRunInfo.Name = "lblRunInfo"
        Me.lblRunInfo.Size = New System.Drawing.Size(112, 17)
        Me.lblRunInfo.TabIndex = 0
        Me.lblRunInfo.Text = "Run Information:"
        '
        'txtRunInfo
        '
        Me.txtRunInfo.Location = New System.Drawing.Point(133, 12)
        Me.txtRunInfo.Name = "txtRunInfo"
        Me.txtRunInfo.Size = New System.Drawing.Size(437, 22)
        Me.txtRunInfo.TabIndex = 1
        '
        'ctlEditGlobalBlock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtRunInfo)
        Me.Controls.Add(Me.lblRunInfo)
        Me.Name = "ctlEditGlobalBlock"
        Me.Size = New System.Drawing.Size(589, 89)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblRunInfo As System.Windows.Forms.Label
    Friend WithEvents txtRunInfo As System.Windows.Forms.TextBox

End Class
