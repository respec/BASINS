<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecifyRegion
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
        Me.lblBoxProjection = New System.Windows.Forms.Label
        Me.txtRegionProjection = New System.Windows.Forms.TextBox
        Me.lblRight = New System.Windows.Forms.Label
        Me.txtRight = New System.Windows.Forms.TextBox
        Me.lblBottom = New System.Windows.Forms.Label
        Me.txtBottom = New System.Windows.Forms.TextBox
        Me.lblLeft = New System.Windows.Forms.Label
        Me.txtLeft = New System.Windows.Forms.TextBox
        Me.lblTop = New System.Windows.Forms.Label
        Me.txtTop = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblBoxProjection
        '
        Me.lblBoxProjection.AutoSize = True
        Me.lblBoxProjection.Location = New System.Drawing.Point(15, 94)
        Me.lblBoxProjection.Name = "lblBoxProjection"
        Me.lblBoxProjection.Size = New System.Drawing.Size(75, 13)
        Me.lblBoxProjection.TabIndex = 40
        Me.lblBoxProjection.Text = "Box Projection"
        '
        'txtRegionProjection
        '
        Me.txtRegionProjection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRegionProjection.Location = New System.Drawing.Point(109, 91)
        Me.txtRegionProjection.Name = "txtRegionProjection"
        Me.txtRegionProjection.Size = New System.Drawing.Size(347, 20)
        Me.txtRegionProjection.TabIndex = 39
        Me.txtRegionProjection.Text = "+proj=latlong +datum=NAD83"
        '
        'lblRight
        '
        Me.lblRight.AutoSize = True
        Me.lblRight.Location = New System.Drawing.Point(142, 41)
        Me.lblRight.Name = "lblRight"
        Me.lblRight.Size = New System.Drawing.Size(32, 13)
        Me.lblRight.TabIndex = 38
        Me.lblRight.Text = "Right"
        '
        'txtRight
        '
        Me.txtRight.Location = New System.Drawing.Point(180, 38)
        Me.txtRight.Name = "txtRight"
        Me.txtRight.Size = New System.Drawing.Size(67, 20)
        Me.txtRight.TabIndex = 37
        Me.txtRight.Text = "-84.1"
        '
        'lblBottom
        '
        Me.lblBottom.AutoSize = True
        Me.lblBottom.Location = New System.Drawing.Point(64, 68)
        Me.lblBottom.Name = "lblBottom"
        Me.lblBottom.Size = New System.Drawing.Size(40, 13)
        Me.lblBottom.TabIndex = 36
        Me.lblBottom.Text = "Bottom"
        '
        'txtBottom
        '
        Me.txtBottom.Location = New System.Drawing.Point(110, 65)
        Me.txtBottom.Name = "txtBottom"
        Me.txtBottom.Size = New System.Drawing.Size(67, 20)
        Me.txtBottom.TabIndex = 35
        Me.txtBottom.Text = "33.5"
        '
        'lblLeft
        '
        Me.lblLeft.AutoSize = True
        Me.lblLeft.Location = New System.Drawing.Point(15, 41)
        Me.lblLeft.Name = "lblLeft"
        Me.lblLeft.Size = New System.Drawing.Size(25, 13)
        Me.lblLeft.TabIndex = 34
        Me.lblLeft.Text = "Left"
        '
        'txtLeft
        '
        Me.txtLeft.Location = New System.Drawing.Point(46, 38)
        Me.txtLeft.Name = "txtLeft"
        Me.txtLeft.Size = New System.Drawing.Size(67, 20)
        Me.txtLeft.TabIndex = 33
        Me.txtLeft.Text = "-84.2"
        '
        'lblTop
        '
        Me.lblTop.AutoSize = True
        Me.lblTop.Location = New System.Drawing.Point(78, 15)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(26, 13)
        Me.lblTop.TabIndex = 32
        Me.lblTop.Text = "Top"
        '
        'txtTop
        '
        Me.txtTop.Location = New System.Drawing.Point(110, 12)
        Me.txtTop.Name = "txtTop"
        Me.txtTop.Size = New System.Drawing.Size(67, 20)
        Me.txtTop.TabIndex = 31
        Me.txtTop.Text = "33.6"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(354, 128)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(102, 29)
        Me.btnOk.TabIndex = 62
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmSpecifyRegion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(468, 169)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblBoxProjection)
        Me.Controls.Add(Me.txtRegionProjection)
        Me.Controls.Add(Me.lblRight)
        Me.Controls.Add(Me.txtRight)
        Me.Controls.Add(Me.lblBottom)
        Me.Controls.Add(Me.txtBottom)
        Me.Controls.Add(Me.lblLeft)
        Me.Controls.Add(Me.txtLeft)
        Me.Controls.Add(Me.lblTop)
        Me.Controls.Add(Me.txtTop)
        Me.Name = "frmSpecifyRegion"
        Me.Text = "Specify Bounding Box of Region"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblBoxProjection As System.Windows.Forms.Label
    Friend WithEvents txtRegionProjection As System.Windows.Forms.TextBox
    Friend WithEvents lblRight As System.Windows.Forms.Label
    Friend WithEvents txtRight As System.Windows.Forms.TextBox
    Friend WithEvents lblBottom As System.Windows.Forms.Label
    Friend WithEvents txtBottom As System.Windows.Forms.TextBox
    Friend WithEvents lblLeft As System.Windows.Forms.Label
    Friend WithEvents txtLeft As System.Windows.Forms.TextBox
    Friend WithEvents lblTop As System.Windows.Forms.Label
    Friend WithEvents txtTop As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
