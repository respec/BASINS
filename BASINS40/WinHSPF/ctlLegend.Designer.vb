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
        Me.btnScrollLegendUp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScrollLegendUp.Location = New System.Drawing.Point(0, 0)
        Me.btnScrollLegendUp.Name = "btnScrollLegendUp"
        Me.btnScrollLegendUp.Size = New System.Drawing.Size(300, 21)
        Me.btnScrollLegendUp.TabIndex = 7
        Me.btnScrollLegendUp.Text = "^"
        Me.btnScrollLegendUp.UseVisualStyleBackColor = True
        '
        'btnScrollLegendDown
        '
        Me.btnScrollLegendDown.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnScrollLegendDown.Location = New System.Drawing.Point(0, 289)
        Me.btnScrollLegendDown.Name = "btnScrollLegendDown"
        Me.btnScrollLegendDown.Size = New System.Drawing.Size(300, 21)
        Me.btnScrollLegendDown.TabIndex = 6
        Me.btnScrollLegendDown.Text = "v"
        Me.btnScrollLegendDown.UseVisualStyleBackColor = True
        '
        'pnlLegend
        '
        Me.pnlLegend.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLegend.Location = New System.Drawing.Point(0, 22)
        Me.pnlLegend.Name = "pnlLegend"
        Me.pnlLegend.Size = New System.Drawing.Size(300, 268)
        Me.pnlLegend.TabIndex = 8
        '
        'ctlLegend
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnScrollLegendDown)
        Me.Controls.Add(Me.btnScrollLegendUp)
        Me.Controls.Add(Me.pnlLegend)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ctlLegend"
        Me.Size = New System.Drawing.Size(300, 310)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnScrollLegendUp As System.Windows.Forms.Button
    Friend WithEvents btnScrollLegendDown As System.Windows.Forms.Button
    Friend WithEvents pnlLegend As PanelDoubleBuffer

End Class
