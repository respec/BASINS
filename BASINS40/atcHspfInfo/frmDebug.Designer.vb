<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmDebug
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
	Public WithEvents ListType As System.Windows.Forms.CheckedListBox
	Public WithEvents CheckSave As System.Windows.Forms.CheckBox
	Public WithEvents txtLev As System.Windows.Forms.TextBox
	Public WithEvents txtFlsh As System.Windows.Forms.TextBox
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents cmdClear As System.Windows.Forms.Button
	Public WithEvents lblLev As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents fraOptions As System.Windows.Forms.GroupBox
	Public WithEvents cdlg As System.Windows.Forms.PictureBox
	Public WithEvents txtDetails As System.Windows.Forms.TextBox
	Public WithEvents lblDetails As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDebug))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.fraOptions = New System.Windows.Forms.GroupBox
		Me.ListType = New System.Windows.Forms.CheckedListBox
		Me.CheckSave = New System.Windows.Forms.CheckBox
		Me.txtLev = New System.Windows.Forms.TextBox
		Me.txtFlsh = New System.Windows.Forms.TextBox
		Me.cmdSave = New System.Windows.Forms.Button
		Me.cmdClear = New System.Windows.Forms.Button
		Me.lblLev = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.cdlg = New System.Windows.Forms.PictureBox
		Me.txtDetails = New System.Windows.Forms.TextBox
		Me.lblDetails = New System.Windows.Forms.Label
		Me.fraOptions.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.Text = "Debug"
		Me.ClientSize = New System.Drawing.Size(495, 398)
		Me.Location = New System.Drawing.Point(3, 18)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmDebug"
		Me.fraOptions.Text = "Options"
		Me.fraOptions.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraOptions.Size = New System.Drawing.Size(477, 97)
		Me.fraOptions.Location = New System.Drawing.Point(8, 296)
		Me.fraOptions.TabIndex = 8
		Me.fraOptions.BackColor = System.Drawing.SystemColors.Control
		Me.fraOptions.Enabled = True
		Me.fraOptions.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraOptions.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraOptions.Visible = True
		Me.fraOptions.Padding = New System.Windows.Forms.Padding(0)
		Me.fraOptions.Name = "fraOptions"
		Me.ListType.Size = New System.Drawing.Size(121, 49)
		Me.ListType.Location = New System.Drawing.Point(16, 24)
		Me.ListType.TabIndex = 1
		Me.ListType.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ListType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.ListType.BackColor = System.Drawing.SystemColors.Window
		Me.ListType.CausesValidation = True
		Me.ListType.Enabled = True
		Me.ListType.ForeColor = System.Drawing.SystemColors.WindowText
		Me.ListType.IntegralHeight = True
		Me.ListType.Cursor = System.Windows.Forms.Cursors.Default
		Me.ListType.SelectionMode = System.Windows.Forms.SelectionMode.One
		Me.ListType.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ListType.Sorted = False
		Me.ListType.TabStop = True
		Me.ListType.Visible = True
		Me.ListType.MultiColumn = False
		Me.ListType.Name = "ListType"
		Me.CheckSave.Text = "Continuous Save"
		Me.CheckSave.Size = New System.Drawing.Size(105, 17)
		Me.CheckSave.Location = New System.Drawing.Point(344, 56)
		Me.CheckSave.TabIndex = 6
		Me.CheckSave.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CheckSave.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.CheckSave.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.CheckSave.BackColor = System.Drawing.SystemColors.Control
		Me.CheckSave.CausesValidation = True
		Me.CheckSave.Enabled = True
		Me.CheckSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.CheckSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.CheckSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.CheckSave.Appearance = System.Windows.Forms.Appearance.Normal
		Me.CheckSave.TabStop = True
		Me.CheckSave.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.CheckSave.Visible = True
		Me.CheckSave.Name = "CheckSave"
		Me.txtLev.AutoSize = False
		Me.txtLev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtLev.Size = New System.Drawing.Size(21, 17)
		Me.txtLev.Location = New System.Drawing.Point(288, 28)
		Me.txtLev.TabIndex = 2
		Me.txtLev.Text = "3"
		Me.txtLev.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtLev.AcceptsReturn = True
		Me.txtLev.BackColor = System.Drawing.SystemColors.Window
		Me.txtLev.CausesValidation = True
		Me.txtLev.Enabled = True
		Me.txtLev.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLev.HideSelection = True
		Me.txtLev.ReadOnly = False
		Me.txtLev.Maxlength = 0
		Me.txtLev.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLev.MultiLine = False
		Me.txtLev.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLev.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLev.TabStop = True
		Me.txtLev.Visible = True
		Me.txtLev.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLev.Name = "txtLev"
		Me.txtFlsh.AutoSize = False
		Me.txtFlsh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.txtFlsh.Size = New System.Drawing.Size(21, 17)
		Me.txtFlsh.Location = New System.Drawing.Point(288, 52)
		Me.txtFlsh.TabIndex = 3
		Me.txtFlsh.Text = "8"
		Me.txtFlsh.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtFlsh.AcceptsReturn = True
		Me.txtFlsh.BackColor = System.Drawing.SystemColors.Window
		Me.txtFlsh.CausesValidation = True
		Me.txtFlsh.Enabled = True
		Me.txtFlsh.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtFlsh.HideSelection = True
		Me.txtFlsh.ReadOnly = False
		Me.txtFlsh.Maxlength = 0
		Me.txtFlsh.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtFlsh.MultiLine = False
		Me.txtFlsh.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtFlsh.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtFlsh.TabStop = True
		Me.txtFlsh.Visible = True
		Me.txtFlsh.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtFlsh.Name = "txtFlsh"
		Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSave.Text = "&Save"
		Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSave.Size = New System.Drawing.Size(45, 25)
		Me.cmdSave.Location = New System.Drawing.Point(400, 24)
		Me.cmdSave.TabIndex = 5
		Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSave.CausesValidation = True
		Me.cmdSave.Enabled = True
		Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSave.TabStop = True
		Me.cmdSave.Name = "cmdSave"
		Me.cmdClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdClear.Text = "&Clear"
		Me.cmdClear.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdClear.Size = New System.Drawing.Size(45, 25)
		Me.cmdClear.Location = New System.Drawing.Point(344, 24)
		Me.cmdClear.TabIndex = 4
		Me.cmdClear.BackColor = System.Drawing.SystemColors.Control
		Me.cmdClear.CausesValidation = True
		Me.cmdClear.Enabled = True
		Me.cmdClear.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdClear.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdClear.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdClear.TabStop = True
		Me.cmdClear.Name = "cmdClear"
		Me.lblLev.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblLev.Text = "Display Entries <= Level"
		Me.lblLev.Size = New System.Drawing.Size(125, 17)
		Me.lblLev.Location = New System.Drawing.Point(152, 32)
		Me.lblLev.TabIndex = 10
		Me.lblLev.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblLev.BackColor = System.Drawing.SystemColors.Control
		Me.lblLev.Enabled = True
		Me.lblLev.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLev.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLev.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLev.UseMnemonic = True
		Me.lblLev.Visible = True
		Me.lblLev.AutoSize = False
		Me.lblLev.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLev.Name = "lblLev"
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label1.Text = "Flush New Entries > Level"
		Me.Label1.Size = New System.Drawing.Size(133, 17)
		Me.Label1.Location = New System.Drawing.Point(144, 52)
		Me.Label1.TabIndex = 9
		Me.Label1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me.cdlg.Size = New System.Drawing.Size(80, 32)
		Me.cdlg.Location = New System.Drawing.Point(436, 0)
		Me.cdlg.TabIndex = 11
		Me.cdlg.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cdlg.Dock = System.Windows.Forms.DockStyle.None
		Me.cdlg.BackColor = System.Drawing.SystemColors.Control
		Me.cdlg.CausesValidation = True
		Me.cdlg.Enabled = True
		Me.cdlg.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cdlg.Cursor = System.Windows.Forms.Cursors.Default
		Me.cdlg.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cdlg.TabStop = True
		Me.cdlg.Visible = True
		Me.cdlg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal
		Me.cdlg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.cdlg.Name = "cdlg"
		Me.txtDetails.AutoSize = False
		Me.txtDetails.BackColor = System.Drawing.SystemColors.InactiveBorder
		Me.txtDetails.Font = New System.Drawing.Font("Courier New", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtDetails.Size = New System.Drawing.Size(473, 261)
		Me.txtDetails.Location = New System.Drawing.Point(8, 28)
		Me.txtDetails.ReadOnly = True
		Me.txtDetails.MultiLine = True
		Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.txtDetails.TabIndex = 0
		Me.txtDetails.TabStop = False
		Me.txtDetails.AcceptsReturn = True
		Me.txtDetails.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDetails.CausesValidation = True
		Me.txtDetails.Enabled = True
		Me.txtDetails.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDetails.HideSelection = True
		Me.txtDetails.Maxlength = 0
		Me.txtDetails.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDetails.Visible = True
		Me.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDetails.Name = "txtDetails"
		Me.lblDetails.Text = "Time, Module, Type:Level:Msg"
		Me.lblDetails.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDetails.Size = New System.Drawing.Size(309, 17)
		Me.lblDetails.Location = New System.Drawing.Point(8, 8)
		Me.lblDetails.TabIndex = 7
		Me.lblDetails.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDetails.BackColor = System.Drawing.SystemColors.Control
		Me.lblDetails.Enabled = True
		Me.lblDetails.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDetails.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDetails.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDetails.UseMnemonic = True
		Me.lblDetails.Visible = True
		Me.lblDetails.AutoSize = False
		Me.lblDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDetails.Name = "lblDetails"
		Me.Controls.Add(fraOptions)
		Me.Controls.Add(cdlg)
		Me.Controls.Add(txtDetails)
		Me.Controls.Add(lblDetails)
		Me.fraOptions.Controls.Add(ListType)
		Me.fraOptions.Controls.Add(CheckSave)
		Me.fraOptions.Controls.Add(txtLev)
		Me.fraOptions.Controls.Add(txtFlsh)
		Me.fraOptions.Controls.Add(cmdSave)
		Me.fraOptions.Controls.Add(cmdClear)
		Me.fraOptions.Controls.Add(lblLev)
		Me.fraOptions.Controls.Add(Label1)
		Me.fraOptions.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class