<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated(), ToolboxBitmap(GetType(ATCoSelectListSortByProp), "ATCoSelectListSortByProp.ToolboxBitmap")> Partial Class ATCoSelectListSortByProp
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
    Friend WithEvents cmdMoveAllLeft As System.Windows.Forms.Button
	Friend WithEvents cmdMoveAllRight As System.Windows.Forms.Button
	Friend WithEvents cmdMoveLeft As System.Windows.Forms.Button
	Friend WithEvents cmdMoveRight As System.Windows.Forms.Button
	Friend WithEvents lstRight As System.Windows.Forms.ListBox
	Friend WithEvents lstLeft As System.Windows.Forms.ListBox
	Friend WithEvents lblRight As System.Windows.Forms.Label
	Friend WithEvents lblLeft As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ATCoSelectListSortByProp))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdMoveDown = New System.Windows.Forms.Button()
        Me.cmdMoveUp = New System.Windows.Forms.Button()
        Me.cmdMoveAllLeft = New System.Windows.Forms.Button()
        Me.cmdMoveAllRight = New System.Windows.Forms.Button()
        Me.cmdMoveLeft = New System.Windows.Forms.Button()
        Me.cmdMoveRight = New System.Windows.Forms.Button()
        Me.lstRight = New System.Windows.Forms.ListBox()
        Me.lstLeft = New System.Windows.Forms.ListBox()
        Me.lblRight = New System.Windows.Forms.Label()
        Me.lblLeft = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdMoveDown
        '
        Me.cmdMoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMoveDown.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveDown.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveDown.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveDown.Image = CType(resources.GetObject("cmdMoveDown.Image"), System.Drawing.Image)
        Me.cmdMoveDown.Location = New System.Drawing.Point(389, 175)
        Me.cmdMoveDown.Name = "cmdMoveDown"
        Me.cmdMoveDown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveDown.Size = New System.Drawing.Size(25, 41)
        Me.cmdMoveDown.TabIndex = 10
        Me.cmdMoveDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdMoveDown, "Move Item Down In List")
        Me.cmdMoveDown.UseVisualStyleBackColor = False
        '
        'cmdMoveUp
        '
        Me.cmdMoveUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMoveUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveUp.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveUp.Image = CType(resources.GetObject("cmdMoveUp.Image"), System.Drawing.Image)
        Me.cmdMoveUp.Location = New System.Drawing.Point(389, 24)
        Me.cmdMoveUp.Name = "cmdMoveUp"
        Me.cmdMoveUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveUp.Size = New System.Drawing.Size(25, 41)
        Me.cmdMoveUp.TabIndex = 11
        Me.cmdMoveUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdMoveUp, "Move Item Up In List")
        Me.cmdMoveUp.UseVisualStyleBackColor = False
        '
        'cmdMoveAllLeft
        '
        Me.cmdMoveAllLeft.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveAllLeft.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveAllLeft.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveAllLeft.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveAllLeft.Location = New System.Drawing.Point(143, 117)
        Me.cmdMoveAllLeft.Name = "cmdMoveAllLeft"
        Me.cmdMoveAllLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveAllLeft.Size = New System.Drawing.Size(97, 25)
        Me.cmdMoveAllLeft.TabIndex = 4
        Me.cmdMoveAllLeft.Text = "<<- Remove All"
        Me.cmdMoveAllLeft.UseVisualStyleBackColor = False
        '
        'cmdMoveAllRight
        '
        Me.cmdMoveAllRight.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveAllRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveAllRight.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveAllRight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveAllRight.Location = New System.Drawing.Point(143, 86)
        Me.cmdMoveAllRight.Name = "cmdMoveAllRight"
        Me.cmdMoveAllRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveAllRight.Size = New System.Drawing.Size(97, 25)
        Me.cmdMoveAllRight.TabIndex = 3
        Me.cmdMoveAllRight.Text = "Add All ->>"
        Me.cmdMoveAllRight.UseVisualStyleBackColor = False
        '
        'cmdMoveLeft
        '
        Me.cmdMoveLeft.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveLeft.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveLeft.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveLeft.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveLeft.Location = New System.Drawing.Point(143, 55)
        Me.cmdMoveLeft.Name = "cmdMoveLeft"
        Me.cmdMoveLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveLeft.Size = New System.Drawing.Size(97, 25)
        Me.cmdMoveLeft.TabIndex = 2
        Me.cmdMoveLeft.Text = "<- Remove"
        Me.cmdMoveLeft.UseVisualStyleBackColor = False
        '
        'cmdMoveRight
        '
        Me.cmdMoveRight.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveRight.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveRight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveRight.Location = New System.Drawing.Point(143, 24)
        Me.cmdMoveRight.Name = "cmdMoveRight"
        Me.cmdMoveRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveRight.Size = New System.Drawing.Size(97, 25)
        Me.cmdMoveRight.TabIndex = 1
        Me.cmdMoveRight.Text = "Add ->"
        Me.cmdMoveRight.UseVisualStyleBackColor = False
        '
        'lstRight
        '
        Me.lstRight.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstRight.BackColor = System.Drawing.SystemColors.Window
        Me.lstRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstRight.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstRight.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstRight.IntegralHeight = False
        Me.lstRight.ItemHeight = 14
        Me.lstRight.Location = New System.Drawing.Point(246, 24)
        Me.lstRight.Name = "lstRight"
        Me.lstRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstRight.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstRight.Size = New System.Drawing.Size(137, 192)
        Me.lstRight.TabIndex = 5
        '
        'lstLeft
        '
        Me.lstLeft.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstLeft.BackColor = System.Drawing.SystemColors.Window
        Me.lstLeft.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstLeft.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstLeft.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstLeft.IntegralHeight = False
        Me.lstLeft.ItemHeight = 14
        Me.lstLeft.Location = New System.Drawing.Point(0, 24)
        Me.lstLeft.Name = "lstLeft"
        Me.lstLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstLeft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstLeft.Size = New System.Drawing.Size(137, 192)
        Me.lstLeft.TabIndex = 0
        '
        'lblRight
        '
        Me.lblRight.BackColor = System.Drawing.SystemColors.Control
        Me.lblRight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRight.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRight.Location = New System.Drawing.Point(246, 4)
        Me.lblRight.Name = "lblRight"
        Me.lblRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRight.Size = New System.Drawing.Size(113, 17)
        Me.lblRight.TabIndex = 9
        Me.lblRight.Text = "Selected:"
        '
        'lblLeft
        '
        Me.lblLeft.BackColor = System.Drawing.SystemColors.Control
        Me.lblLeft.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLeft.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeft.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLeft.Location = New System.Drawing.Point(0, 4)
        Me.lblLeft.Name = "lblLeft"
        Me.lblLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLeft.Size = New System.Drawing.Size(113, 17)
        Me.lblLeft.TabIndex = 8
        Me.lblLeft.Text = "Available:"
        '
        'ATCoSelectListSortByProp
        '
        Me.Controls.Add(Me.cmdMoveUp)
        Me.Controls.Add(Me.cmdMoveDown)
        Me.Controls.Add(Me.cmdMoveAllLeft)
        Me.Controls.Add(Me.cmdMoveAllRight)
        Me.Controls.Add(Me.cmdMoveLeft)
        Me.Controls.Add(Me.cmdMoveRight)
        Me.Controls.Add(Me.lstRight)
        Me.Controls.Add(Me.lstLeft)
        Me.Controls.Add(Me.lblRight)
        Me.Controls.Add(Me.lblLeft)
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "ATCoSelectListSortByProp"
        Me.Size = New System.Drawing.Size(414, 219)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdMoveDown As System.Windows.Forms.Button
    Friend WithEvents cmdMoveUp As System.Windows.Forms.Button
#End Region 
End Class