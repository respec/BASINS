<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLand
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
        Me.ListSources = New System.Windows.Forms.CheckedListBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.grpSources = New System.Windows.Forms.GroupBox
        Me.chkAllSources = New System.Windows.Forms.CheckBox
        Me.txtLabelTotal = New System.Windows.Forms.Label
        Me.txtLabelOrigTotal = New System.Windows.Forms.Label
        Me.txtLabelDifference = New System.Windows.Forms.Label
        Me.txtDifference = New System.Windows.Forms.Label
        Me.txtOrigTotal = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.Label
        Me.grdLand = New atcControls.atcGrid
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkAllTargets = New System.Windows.Forms.CheckBox
        Me.ListTargets = New System.Windows.Forms.CheckedListBox
        Me.grpSources.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListSources
        '
        Me.ListSources.FormattingEnabled = True
        Me.ListSources.Location = New System.Drawing.Point(9, 42)
        Me.ListSources.Name = "ListSources"
        Me.ListSources.Size = New System.Drawing.Size(190, 184)
        Me.ListSources.TabIndex = 6
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(344, 608)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(125, 30)
        Me.cmdOK.TabIndex = 9
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(494, 608)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(125, 30)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'grpSources
        '
        Me.grpSources.Controls.Add(Me.chkAllSources)
        Me.grpSources.Controls.Add(Me.ListSources)
        Me.grpSources.Location = New System.Drawing.Point(12, 12)
        Me.grpSources.Name = "grpSources"
        Me.grpSources.Size = New System.Drawing.Size(209, 237)
        Me.grpSources.TabIndex = 11
        Me.grpSources.TabStop = False
        Me.grpSources.Text = "Sources"
        '
        'chkAllSources
        '
        Me.chkAllSources.AutoSize = True
        Me.chkAllSources.Location = New System.Drawing.Point(9, 19)
        Me.chkAllSources.Name = "chkAllSources"
        Me.chkAllSources.Size = New System.Drawing.Size(70, 17)
        Me.chkAllSources.TabIndex = 7
        Me.chkAllSources.Text = "Select All"
        Me.chkAllSources.UseVisualStyleBackColor = True
        '
        'txtLabelTotal
        '
        Me.txtLabelTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLabelTotal.AutoSize = True
        Me.txtLabelTotal.Location = New System.Drawing.Point(737, 516)
        Me.txtLabelTotal.Name = "txtLabelTotal"
        Me.txtLabelTotal.Size = New System.Drawing.Size(71, 13)
        Me.txtLabelTotal.TabIndex = 13
        Me.txtLabelTotal.Text = "Current Total:"
        Me.txtLabelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLabelOrigTotal
        '
        Me.txtLabelOrigTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLabelOrigTotal.AutoSize = True
        Me.txtLabelOrigTotal.Location = New System.Drawing.Point(736, 541)
        Me.txtLabelOrigTotal.Name = "txtLabelOrigTotal"
        Me.txtLabelOrigTotal.Size = New System.Drawing.Size(72, 13)
        Me.txtLabelOrigTotal.TabIndex = 14
        Me.txtLabelOrigTotal.Text = "Original Total:"
        Me.txtLabelOrigTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLabelDifference
        '
        Me.txtLabelDifference.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLabelDifference.AutoSize = True
        Me.txtLabelDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtLabelDifference.Location = New System.Drawing.Point(749, 566)
        Me.txtLabelDifference.Name = "txtLabelDifference"
        Me.txtLabelDifference.Size = New System.Drawing.Size(59, 13)
        Me.txtLabelDifference.TabIndex = 15
        Me.txtLabelDifference.Text = "Difference:"
        Me.txtLabelDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDifference
        '
        Me.txtDifference.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDifference.AutoSize = True
        Me.txtDifference.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtDifference.Location = New System.Drawing.Point(816, 565)
        Me.txtDifference.Name = "txtDifference"
        Me.txtDifference.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDifference.Size = New System.Drawing.Size(133, 14)
        Me.txtDifference.TabIndex = 18
        Me.txtDifference.Text = "                 0"
        Me.txtDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtOrigTotal
        '
        Me.txtOrigTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOrigTotal.AutoSize = True
        Me.txtOrigTotal.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrigTotal.Location = New System.Drawing.Point(816, 540)
        Me.txtOrigTotal.Name = "txtOrigTotal"
        Me.txtOrigTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOrigTotal.Size = New System.Drawing.Size(133, 14)
        Me.txtOrigTotal.TabIndex = 17
        Me.txtOrigTotal.Text = "                 0"
        Me.txtOrigTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtTotal.AutoSize = True
        Me.txtTotal.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotal.Location = New System.Drawing.Point(816, 515)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTotal.Size = New System.Drawing.Size(133, 14)
        Me.txtTotal.TabIndex = 16
        Me.txtTotal.Text = "                 0"
        Me.txtTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grdLand
        '
        Me.grdLand.AllowHorizontalScrolling = True
        Me.grdLand.AllowNewValidValues = False
        Me.grdLand.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdLand.CellBackColor = System.Drawing.Color.Empty
        Me.grdLand.Fixed3D = False
        Me.grdLand.LineColor = System.Drawing.Color.Empty
        Me.grdLand.LineWidth = 0.0!
        Me.grdLand.Location = New System.Drawing.Point(240, 12)
        Me.grdLand.Name = "grdLand"
        Me.grdLand.Size = New System.Drawing.Size(701, 493)
        Me.grdLand.Source = Nothing
        Me.grdLand.TabIndex = 8
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkAllTargets)
        Me.GroupBox1.Controls.Add(Me.ListTargets)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 268)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(209, 237)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Targets"
        '
        'chkAllTargets
        '
        Me.chkAllTargets.AutoSize = True
        Me.chkAllTargets.Location = New System.Drawing.Point(9, 19)
        Me.chkAllTargets.Name = "chkAllTargets"
        Me.chkAllTargets.Size = New System.Drawing.Size(70, 17)
        Me.chkAllTargets.TabIndex = 7
        Me.chkAllTargets.Text = "Select All"
        Me.chkAllTargets.UseVisualStyleBackColor = True
        '
        'ListTargets
        '
        Me.ListTargets.FormattingEnabled = True
        Me.ListTargets.Location = New System.Drawing.Point(9, 42)
        Me.ListTargets.Name = "ListTargets"
        Me.ListTargets.Size = New System.Drawing.Size(190, 184)
        Me.ListTargets.TabIndex = 6
        '
        'frmLand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 643)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtDifference)
        Me.Controls.Add(Me.txtOrigTotal)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.txtLabelDifference)
        Me.Controls.Add(Me.txtLabelOrigTotal)
        Me.Controls.Add(Me.txtLabelTotal)
        Me.Controls.Add(Me.grpSources)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.grdLand)
        Me.Name = "frmLand"
        Me.Text = "Form1"
        Me.grpSources.ResumeLayout(False)
        Me.grpSources.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListSources As System.Windows.Forms.CheckedListBox
    Friend WithEvents grdLand As atcControls.atcGrid
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents grpSources As System.Windows.Forms.GroupBox
    Friend WithEvents txtLabelTotal As System.Windows.Forms.Label
    Friend WithEvents txtLabelOrigTotal As System.Windows.Forms.Label
    Friend WithEvents txtLabelDifference As System.Windows.Forms.Label
    Friend WithEvents txtDifference As System.Windows.Forms.Label
    Friend WithEvents txtOrigTotal As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.Label
    Friend WithEvents chkAllSources As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkAllTargets As System.Windows.Forms.CheckBox
    Friend WithEvents ListTargets As System.Windows.Forms.CheckedListBox
End Class
