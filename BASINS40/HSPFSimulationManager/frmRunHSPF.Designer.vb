<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRunHSPF
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
        Me.lstModels = New System.Windows.Forms.CheckedListBox()
        Me.lblTop = New System.Windows.Forms.Label()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstModels
        '
        Me.lstModels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstModels.CheckOnClick = True
        Me.lstModels.IntegralHeight = False
        Me.lstModels.Location = New System.Drawing.Point(12, 25)
        Me.lstModels.Name = "lstModels"
        Me.lstModels.Size = New System.Drawing.Size(679, 271)
        Me.lstModels.TabIndex = 0
        '
        'lblTop
        '
        Me.lblTop.AutoSize = True
        Me.lblTop.Location = New System.Drawing.Point(12, 9)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(158, 13)
        Me.lblTop.TabIndex = 1
        Me.lblTop.Text = "Run the selected HSPF models:"
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(616, 302)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 2
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'frmRunHSPF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(703, 337)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.lblTop)
        Me.Controls.Add(Me.lstModels)
        Me.Name = "frmRunHSPF"
        Me.Text = "Run HSPF"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstModels As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblTop As System.Windows.Forms.Label
    Friend WithEvents btnRun As System.Windows.Forms.Button
End Class
