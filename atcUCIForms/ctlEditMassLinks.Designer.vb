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
        Me.txtDefine = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cboID = New System.Windows.Forms.ComboBox
        Me.cmdAddNew = New System.Windows.Forms.Button
        Me.grdMassLink = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'txtDefine
        '
        Me.txtDefine.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtDefine.Location = New System.Drawing.Point(0, 300)
        Me.txtDefine.Margin = New System.Windows.Forms.Padding(2)
        Me.txtDefine.Multiline = True
        Me.txtDefine.Name = "txtDefine"
        Me.txtDefine.Size = New System.Drawing.Size(725, 54)
        Me.txtDefine.TabIndex = 1
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
        Me.cmdAddNew.Location = New System.Drawing.Point(600, 12)
        Me.cmdAddNew.Name = "cmdAddNew"
        Me.cmdAddNew.Size = New System.Drawing.Size(111, 30)
        Me.cmdAddNew.TabIndex = 4
        Me.cmdAddNew.Text = "Add New"
        Me.cmdAddNew.UseVisualStyleBackColor = True
        '
        'grdMassLink
        '
        Me.grdMassLink.AllowHorizontalScrolling = True
        Me.grdMassLink.AllowNewValidValues = False
        Me.grdMassLink.CellBackColor = System.Drawing.Color.Empty
        Me.grdMassLink.LineColor = System.Drawing.Color.Empty
        Me.grdMassLink.LineWidth = 0.0!
        Me.grdMassLink.Location = New System.Drawing.Point(15, 56)
        Me.grdMassLink.Name = "grdMassLink"
        Me.grdMassLink.Size = New System.Drawing.Size(695, 239)
        Me.grdMassLink.Source = Nothing
        Me.grdMassLink.TabIndex = 5
        '
        'ctlEditMasslink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grdMassLink)
        Me.Controls.Add(Me.cmdAddNew)
        Me.Controls.Add(Me.cboID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDefine)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ctlEditMasslink"
        Me.Size = New System.Drawing.Size(725, 356)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDefine As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboID As System.Windows.Forms.ComboBox
    Friend WithEvents cmdAddNew As System.Windows.Forms.Button
    Friend WithEvents grdMassLink As atcControls.atcGrid

End Class
