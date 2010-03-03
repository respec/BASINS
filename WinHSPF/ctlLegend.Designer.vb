<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlLegend
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
        Me.btnScrollLegendUp = New System.Windows.Forms.Button
        Me.btnScrollLegendDown = New System.Windows.Forms.Button
        Me.pnlLegend = New PanelDoubleBuffer()
        Me.SuspendLayout()
        '
        'btnScrollLegendUp
        '
        Me.btnScrollLegendUp.Location = New System.Drawing.Point(0, 0)
        Me.btnScrollLegendUp.Name = "btnScrollLegendUp"
        Me.btnScrollLegendUp.Size = New System.Drawing.Size(400, 26)
        Me.btnScrollLegendUp.TabIndex = 7
        Me.btnScrollLegendUp.Text = "^"
        Me.btnScrollLegendUp.UseVisualStyleBackColor = True
        '
        'btnScrollLegendDown
        '
        Me.btnScrollLegendDown.Location = New System.Drawing.Point(0, 356)
        Me.btnScrollLegendDown.Name = "btnScrollLegendDown"
        Me.btnScrollLegendDown.Size = New System.Drawing.Size(400, 26)
        Me.btnScrollLegendDown.TabIndex = 6
        Me.btnScrollLegendDown.Text = "v"
        Me.btnScrollLegendDown.UseVisualStyleBackColor = True
        '
        'pnlLegend
        '
        Me.pnlLegend.Location = New System.Drawing.Point(0, 27)
        Me.pnlLegend.Name = "pnlLegend"
        Me.pnlLegend.Size = New System.Drawing.Size(400, 330)
        Me.pnlLegend.TabIndex = 8
        '
        'ctlLegend
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.btnScrollLegendDown)
        Me.Controls.Add(Me.btnScrollLegendUp)
        Me.Controls.Add(Me.pnlLegend)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ctlLegend"
        Me.Size = New System.Drawing.Size(400, 382)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnScrollLegendUp As System.Windows.Forms.Button
    Friend WithEvents btnScrollLegendDown As System.Windows.Forms.Button
    Friend WithEvents pnlLegend As PanelDoubleBuffer

End Class
