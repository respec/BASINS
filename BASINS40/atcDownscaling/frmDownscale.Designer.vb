<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownscale
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
        Me.lblRexe = New System.Windows.Forms.Label
        Me.txtRexe = New System.Windows.Forms.TextBox
        Me.btnRun = New System.Windows.Forms.Button
        Me.txtRscript = New System.Windows.Forms.TextBox
        Me.lblRscript = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblRexe
        '
        Me.lblRexe.AutoSize = True
        Me.lblRexe.Location = New System.Drawing.Point(12, 15)
        Me.lblRexe.Name = "lblRexe"
        Me.lblRexe.Size = New System.Drawing.Size(35, 13)
        Me.lblRexe.TabIndex = 0
        Me.lblRexe.Text = "R.exe"
        '
        'txtRexe
        '
        Me.txtRexe.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRexe.Location = New System.Drawing.Point(94, 12)
        Me.txtRexe.Name = "txtRexe"
        Me.txtRexe.Size = New System.Drawing.Size(561, 20)
        Me.txtRexe.TabIndex = 1
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(12, 402)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 2
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'txtRscript
        '
        Me.txtRscript.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRscript.Location = New System.Drawing.Point(94, 38)
        Me.txtRscript.Name = "txtRscript"
        Me.txtRscript.Size = New System.Drawing.Size(561, 20)
        Me.txtRscript.TabIndex = 4
        '
        'lblRscript
        '
        Me.lblRscript.AutoSize = True
        Me.lblRscript.Location = New System.Drawing.Point(12, 41)
        Me.lblRscript.Name = "lblRscript"
        Me.lblRscript.Size = New System.Drawing.Size(43, 13)
        Me.lblRscript.TabIndex = 3
        Me.lblRscript.Text = "R script"
        '
        'frmDownscale
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(667, 437)
        Me.Controls.Add(Me.txtRscript)
        Me.Controls.Add(Me.lblRscript)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.txtRexe)
        Me.Controls.Add(Me.lblRexe)
        Me.Name = "frmDownscale"
        Me.Text = "frmDownscale"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblRexe As System.Windows.Forms.Label
    Friend WithEvents txtRexe As System.Windows.Forms.TextBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents txtRscript As System.Windows.Forms.TextBox
    Friend WithEvents lblRscript As System.Windows.Forms.Label
End Class
