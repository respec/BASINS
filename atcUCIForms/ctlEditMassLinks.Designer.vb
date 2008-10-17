<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditMassLinks
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboID = New System.Windows.Forms.ComboBox
        Me.cmdAddNew = New System.Windows.Forms.Button
        Me.grdMassLink = New atcControls.atcGrid
        Me.txtDefine = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Mass-Link Number:"
        '
        'cboID
        '
        Me.cboID.FormattingEnabled = True
        Me.cboID.Location = New System.Drawing.Point(121, 17)
        Me.cboID.Name = "cboID"
        Me.cboID.Size = New System.Drawing.Size(91, 21)
        Me.cboID.TabIndex = 3
        '
        'cmdAddNew
        '
        Me.cmdAddNew.Location = New System.Drawing.Point(234, 16)
        Me.cmdAddNew.Name = "cmdAddNew"
        Me.cmdAddNew.Size = New System.Drawing.Size(128, 23)
        Me.cmdAddNew.TabIndex = 4
        Me.cmdAddNew.Text = "Add New Mass-Link"
        Me.cmdAddNew.UseVisualStyleBackColor = True
        '
        'grdMassLink
        '
        Me.grdMassLink.AllowHorizontalScrolling = True
        Me.grdMassLink.AllowNewValidValues = False
        Me.grdMassLink.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdMassLink.CellBackColor = System.Drawing.Color.Empty
        Me.grdMassLink.Fixed3D = True
        Me.grdMassLink.LineColor = System.Drawing.Color.Empty
        Me.grdMassLink.LineWidth = 0.0!
        Me.grdMassLink.Location = New System.Drawing.Point(14, 51)
        Me.grdMassLink.Name = "grdMassLink"
        Me.grdMassLink.Size = New System.Drawing.Size(568, 196)
        Me.grdMassLink.Source = Nothing
        Me.grdMassLink.TabIndex = 7
        '
        'txtDefine
        '
        Me.txtDefine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefine.BackColor = System.Drawing.SystemColors.Control
        Me.txtDefine.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefine.Location = New System.Drawing.Point(14, 253)
        Me.txtDefine.Multiline = True
        Me.txtDefine.Name = "txtDefine"
        Me.txtDefine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDefine.Size = New System.Drawing.Size(568, 92)
        Me.txtDefine.TabIndex = 6
        '
        'ctlEditMassLinks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grdMassLink)
        Me.Controls.Add(Me.txtDefine)
        Me.Controls.Add(Me.cmdAddNew)
        Me.Controls.Add(Me.cboID)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ctlEditMassLinks"
        Me.Size = New System.Drawing.Size(597, 348)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboID As System.Windows.Forms.ComboBox
    Friend WithEvents cmdAddNew As System.Windows.Forms.Button
    Friend WithEvents grdMassLink As atcControls.atcGrid
    Friend WithEvents txtDefine As System.Windows.Forms.TextBox

End Class
