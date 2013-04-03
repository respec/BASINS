<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWASPInitialize
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPInitialize))
        Me.btnContinue = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSelect = New System.Windows.Forms.Button
        Me.cboLowest = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.lblInfo = New System.Windows.Forms.Label
        Me.lblWarning = New System.Windows.Forms.Label
        Me.grpSelect = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.grpSelect.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnContinue
        '
        Me.btnContinue.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.HelpProvider1.SetHelpString(Me.btnContinue, "When all desired stream segments are selected, click this to display the WASP Bui" & _
                "lder form.")
        Me.btnContinue.Location = New System.Drawing.Point(99, 174)
        Me.btnContinue.Margin = New System.Windows.Forms.Padding(2)
        Me.btnContinue.Name = "btnContinue"
        Me.HelpProvider1.SetShowHelp(Me.btnContinue, True)
        Me.btnContinue.Size = New System.Drawing.Size(77, 26)
        Me.btnContinue.TabIndex = 3
        Me.btnContinue.Text = "&Continue"
        Me.btnContinue.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdCancel.Location = New System.Drawing.Point(188, 174)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(77, 26)
        Me.cmdCancel.TabIndex = 4
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdSelect
        '
        Me.HelpProvider1.SetHelpString(Me.cmdSelect, "After selecting one or more stream segments on the map and setting the lowest des" & _
                "ired stream order, click this button to scan the layer and select additional ups" & _
                "tream segments.")
        Me.cmdSelect.Location = New System.Drawing.Point(253, 18)
        Me.cmdSelect.Margin = New System.Windows.Forms.Padding(2)
        Me.cmdSelect.Name = "cmdSelect"
        Me.HelpProvider1.SetShowHelp(Me.cmdSelect, True)
        Me.cmdSelect.Size = New System.Drawing.Size(80, 27)
        Me.cmdSelect.TabIndex = 2
        Me.cmdSelect.Text = "&Select"
        Me.cmdSelect.UseVisualStyleBackColor = True
        '
        'cboLowest
        '
        Me.cboLowest.AllowDrop = True
        Me.cboLowest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLowest.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboLowest, "Select the lowest order segment you want to include in the model. Order 1 refers " & _
                "to a headwater segment; where two headwater segments join, an order 2 segment is" & _
                " formed, etc.")
        Me.cboLowest.Location = New System.Drawing.Point(176, 22)
        Me.cboLowest.Margin = New System.Windows.Forms.Padding(2)
        Me.cboLowest.Name = "cboLowest"
        Me.HelpProvider1.SetShowHelp(Me.cboLowest, True)
        Me.cboLowest.Size = New System.Drawing.Size(50, 21)
        Me.cboLowest.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(154, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "&Lowest Stream Order to Select:"
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.BackColor = System.Drawing.SystemColors.Info
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.HelpProvider1.SetHelpString(Me.lblInfo, "This box displays the current selection layer and number of selected shapes. It i" & _
                "s automatically updated as you work with the map.")
        Me.lblInfo.Location = New System.Drawing.Point(6, 16)
        Me.lblInfo.Name = "lblInfo"
        Me.HelpProvider1.SetShowHelp(Me.lblInfo, True)
        Me.lblInfo.Size = New System.Drawing.Size(328, 54)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWarning
        '
        Me.lblWarning.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWarning.BackColor = System.Drawing.Color.Transparent
        Me.lblWarning.ForeColor = System.Drawing.Color.Red
        Me.lblWarning.Location = New System.Drawing.Point(12, 144)
        Me.lblWarning.Name = "lblWarning"
        Me.lblWarning.Size = New System.Drawing.Size(341, 26)
        Me.lblWarning.TabIndex = 2
        Me.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpSelect
        '
        Me.grpSelect.Controls.Add(Me.cmdSelect)
        Me.grpSelect.Controls.Add(Me.cboLowest)
        Me.grpSelect.Controls.Add(Me.Label1)
        Me.grpSelect.Location = New System.Drawing.Point(12, 88)
        Me.grpSelect.Name = "grpSelect"
        Me.grpSelect.Size = New System.Drawing.Size(341, 53)
        Me.grpSelect.TabIndex = 1
        Me.grpSelect.TabStop = False
        Me.grpSelect.Text = "Automatically Select Additional Upstream Segments:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblInfo)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(341, 80)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Current Selection:"
        '
        'frmWASPInitialize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(364, 211)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpSelect)
        Me.Controls.Add(Me.lblWarning)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.btnContinue)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWASPInitialize"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BASINS WASP Initialization"
        Me.TopMost = True
        Me.grpSelect.ResumeLayout(False)
        Me.grpSelect.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnContinue As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdSelect As System.Windows.Forms.Button
    Friend WithEvents cboLowest As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lblWarning As System.Windows.Forms.Label
    Friend WithEvents grpSelect As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
End Class
