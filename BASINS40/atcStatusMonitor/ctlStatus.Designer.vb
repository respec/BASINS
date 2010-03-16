<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlStatus
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
        Me.lblRight = New System.Windows.Forms.Label
        Me.lblLeft = New System.Windows.Forms.Label
        Me.lblTop = New System.Windows.Forms.Label
        Me.Progress = New System.Windows.Forms.ProgressBar
        Me.lblMiddle = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblRight
        '
        Me.lblRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRight.AutoSize = True
        Me.lblRight.BackColor = System.Drawing.Color.Transparent
        Me.lblRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRight.Location = New System.Drawing.Point(675, 23)
        Me.lblRight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRight.Name = "lblRight"
        Me.lblRight.Size = New System.Drawing.Size(55, 17)
        Me.lblRight.TabIndex = 11
        Me.lblRight.Text = "lblRight"
        Me.lblRight.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblLeft
        '
        Me.lblLeft.AutoSize = True
        Me.lblLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeft.Location = New System.Drawing.Point(0, 23)
        Me.lblLeft.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLeft.Name = "lblLeft"
        Me.lblLeft.Size = New System.Drawing.Size(46, 17)
        Me.lblLeft.TabIndex = 9
        Me.lblLeft.Text = "lblLeft"
        '
        'lblTop
        '
        Me.lblTop.AutoSize = True
        Me.lblTop.BackColor = System.Drawing.Color.Transparent
        Me.lblTop.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTop.Location = New System.Drawing.Point(0, 0)
        Me.lblTop.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(47, 17)
        Me.lblTop.TabIndex = 8
        Me.lblTop.Text = "lblTop"
        '
        'Progress
        '
        Me.Progress.Location = New System.Drawing.Point(0, 47)
        Me.Progress.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Progress.Name = "Progress"
        Me.Progress.Size = New System.Drawing.Size(745, 30)
        Me.Progress.TabIndex = 7
        '
        'lblMiddle
        '
        Me.lblMiddle.BackColor = System.Drawing.Color.Transparent
        Me.lblMiddle.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMiddle.Location = New System.Drawing.Point(0, 23)
        Me.lblMiddle.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMiddle.Name = "lblMiddle"
        Me.lblMiddle.Size = New System.Drawing.Size(741, 20)
        Me.lblMiddle.TabIndex = 10
        Me.lblMiddle.Text = "lblMiddle"
        Me.lblMiddle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ctlStatus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblRight)
        Me.Controls.Add(Me.lblLeft)
        Me.Controls.Add(Me.lblTop)
        Me.Controls.Add(Me.Progress)
        Me.Controls.Add(Me.lblMiddle)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "ctlStatus"
        Me.Size = New System.Drawing.Size(745, 79)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblRight As System.Windows.Forms.Label
    Friend WithEvents lblLeft As System.Windows.Forms.Label
    Friend WithEvents lblTop As System.Windows.Forms.Label
    Friend WithEvents Progress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblMiddle As System.Windows.Forms.Label

End Class
