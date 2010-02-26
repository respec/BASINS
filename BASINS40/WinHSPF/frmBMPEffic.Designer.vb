<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBMPEffic
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.lblBmpName = New System.Windows.Forms.Label
        Me.cmbBmpName = New System.Windows.Forms.ComboBox
        Me.lblId = New System.Windows.Forms.Label
        Me.lblReference = New System.Windows.Forms.TextBox
        Me.lblRemoval = New System.Windows.Forms.Label
        Me.agdBmpEfc = New atcControls.atcGrid
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdUpdateUCI = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblBmpName
        '
        Me.lblBmpName.AutoSize = True
        Me.lblBmpName.Location = New System.Drawing.Point(9, 17)
        Me.lblBmpName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBmpName.Name = "lblBmpName"
        Me.lblBmpName.Size = New System.Drawing.Size(82, 17)
        Me.lblBmpName.TabIndex = 0
        Me.lblBmpName.Text = "BMP Name:"
        '
        'cmbBmpName
        '
        Me.cmbBmpName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBmpName.FormattingEnabled = True
        Me.cmbBmpName.Location = New System.Drawing.Point(103, 12)
        Me.cmbBmpName.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbBmpName.Name = "cmbBmpName"
        Me.cmbBmpName.Size = New System.Drawing.Size(337, 24)
        Me.cmbBmpName.TabIndex = 1
        '
        'lblId
        '
        Me.lblId.AutoSize = True
        Me.lblId.Location = New System.Drawing.Point(461, 17)
        Me.lblId.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblId.Name = "lblId"
        Me.lblId.Size = New System.Drawing.Size(124, 17)
        Me.lblId.TabIndex = 2
        Me.lblId.Text = "BMP Operation <>"
        '
        'lblReference
        '
        Me.lblReference.BackColor = System.Drawing.SystemColors.Control
        Me.lblReference.Location = New System.Drawing.Point(13, 62)
        Me.lblReference.Margin = New System.Windows.Forms.Padding(4)
        Me.lblReference.Multiline = True
        Me.lblReference.Name = "lblReference"
        Me.lblReference.Size = New System.Drawing.Size(676, 109)
        Me.lblReference.TabIndex = 3
        Me.lblReference.Text = "Reference: <not applicable>"
        '
        'lblRemoval
        '
        Me.lblRemoval.AutoSize = True
        Me.lblRemoval.Location = New System.Drawing.Point(9, 193)
        Me.lblRemoval.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRemoval.Name = "lblRemoval"
        Me.lblRemoval.Size = New System.Drawing.Size(125, 17)
        Me.lblRemoval.TabIndex = 4
        Me.lblRemoval.Text = "Removal Fractions"
        '
        'agdBmpEfc
        '
        Me.agdBmpEfc.AllowHorizontalScrolling = True
        Me.agdBmpEfc.AllowNewValidValues = False
        Me.agdBmpEfc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdBmpEfc.CellBackColor = System.Drawing.Color.Empty
        Me.agdBmpEfc.Fixed3D = False
        Me.agdBmpEfc.LineColor = System.Drawing.Color.Empty
        Me.agdBmpEfc.LineWidth = 0.0!
        Me.agdBmpEfc.Location = New System.Drawing.Point(13, 217)
        Me.agdBmpEfc.Margin = New System.Windows.Forms.Padding(4)
        Me.agdBmpEfc.Name = "agdBmpEfc"
        Me.agdBmpEfc.Size = New System.Drawing.Size(677, 347)
        Me.agdBmpEfc.Source = Nothing
        Me.agdBmpEfc.TabIndex = 5
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(159, 580)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(135, 32)
        Me.cmdClose.TabIndex = 22
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdUpdateUCI
        '
        Me.cmdUpdateUCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdUpdateUCI.Location = New System.Drawing.Point(13, 580)
        Me.cmdUpdateUCI.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdUpdateUCI.Name = "cmdUpdateUCI"
        Me.cmdUpdateUCI.Size = New System.Drawing.Size(135, 32)
        Me.cmdUpdateUCI.TabIndex = 21
        Me.cmdUpdateUCI.Text = "&Update UCI"
        Me.cmdUpdateUCI.UseVisualStyleBackColor = True
        '
        'frmBMPEffic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(707, 635)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdUpdateUCI)
        Me.Controls.Add(Me.agdBmpEfc)
        Me.Controls.Add(Me.lblRemoval)
        Me.Controls.Add(Me.lblReference)
        Me.Controls.Add(Me.lblId)
        Me.Controls.Add(Me.cmbBmpName)
        Me.Controls.Add(Me.lblBmpName)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmBMPEffic"
        Me.Text = "WinHSPF - Best Management Practices Efficiency Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblBmpName As System.Windows.Forms.Label
    Friend WithEvents cmbBmpName As System.Windows.Forms.ComboBox
    Friend WithEvents lblId As System.Windows.Forms.Label
    Friend WithEvents lblReference As System.Windows.Forms.TextBox
    Friend WithEvents lblRemoval As System.Windows.Forms.Label
    Friend WithEvents agdBmpEfc As atcControls.atcGrid
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateUCI As System.Windows.Forms.Button
End Class
