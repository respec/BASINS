<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RWZProgress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RWZProgress))
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.lblProgressTitle = New System.Windows.Forms.Label()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.txtProgress = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(22, 42)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(499, 19)
        Me.pbProgress.TabIndex = 0
        Me.pbProgress.UseWaitCursor = True
        '
        'lblProgressTitle
        '
        Me.lblProgressTitle.AutoSize = True
        Me.lblProgressTitle.Location = New System.Drawing.Point(19, 16)
        Me.lblProgressTitle.Name = "lblProgressTitle"
        Me.lblProgressTitle.Size = New System.Drawing.Size(198, 13)
        Me.lblProgressTitle.TabIndex = 1
        Me.lblProgressTitle.Text = "Your run statistics are being calculated..."
        Me.lblProgressTitle.UseWaitCursor = True
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(210, 265)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(126, 24)
        Me.cmdExit.TabIndex = 3
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = True
        Me.cmdExit.UseWaitCursor = True
        Me.cmdExit.Visible = False
        '
        'txtProgress
        '
        Me.txtProgress.AcceptsReturn = True
        Me.txtProgress.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.txtProgress.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProgress.Location = New System.Drawing.Point(24, 73)
        Me.txtProgress.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.txtProgress.Multiline = True
        Me.txtProgress.Name = "txtProgress"
        Me.txtProgress.ReadOnly = True
        Me.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtProgress.Size = New System.Drawing.Size(498, 186)
        Me.txtProgress.TabIndex = 4
        Me.txtProgress.UseWaitCursor = True
        '
        'RWZProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(546, 297)
        Me.Controls.Add(Me.txtProgress)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblProgressTitle)
        Me.Controls.Add(Me.pbProgress)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "RWZProgress"
        Me.Text = "Progress"
        Me.TransparencyKey = System.Drawing.Color.White
        Me.UseWaitCursor = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgressTitle As System.Windows.Forms.Label
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents txtProgress As System.Windows.Forms.TextBox
End Class
