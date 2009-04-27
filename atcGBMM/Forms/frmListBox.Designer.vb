<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class FrmListBox
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents cmdOK As System.Windows.Forms.Button
	Public WithEvents ListBoxErrors As System.Windows.Forms.ListBox
	Public WithEvents lblMissing As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmListBox))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdOK = New System.Windows.Forms.Button
        Me.ListBoxErrors = New System.Windows.Forms.ListBox
        Me.lblMissing = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(259, 170)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(65, 25)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "Close"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'ListBoxErrors
        '
        Me.ListBoxErrors.BackColor = System.Drawing.SystemColors.Window
        Me.ListBoxErrors.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListBoxErrors.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxErrors.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ListBoxErrors.Location = New System.Drawing.Point(12, 28)
        Me.ListBoxErrors.Name = "ListBoxErrors"
        Me.ListBoxErrors.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListBoxErrors.Size = New System.Drawing.Size(312, 134)
        Me.ListBoxErrors.TabIndex = 1
        '
        'lblMissing
        '
        Me.lblMissing.AutoSize = True
        Me.lblMissing.BackColor = System.Drawing.SystemColors.Control
        Me.lblMissing.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMissing.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissing.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMissing.Location = New System.Drawing.Point(12, 9)
        Me.lblMissing.Name = "lblMissing"
        Me.lblMissing.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMissing.Size = New System.Drawing.Size(267, 13)
        Me.lblMissing.TabIndex = 0
        Me.lblMissing.Text = "Following input parameter(s) are required to continue:"
        '
        'FrmListBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(336, 207)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.ListBoxErrors)
        Me.Controls.Add(Me.lblMissing)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "FrmListBox"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Error List"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region 
End Class