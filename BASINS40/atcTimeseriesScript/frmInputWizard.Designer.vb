<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmInputWizard
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
	Public WithEvents agdDataMapping As AxATCoCtl.AxATCoGrid
	Public WithEvents _fraTab_2 As System.Windows.Forms.Panel
	Public WithEvents txtScriptDesc As System.Windows.Forms.TextBox
	Public WithEvents txtHeaderLines As System.Windows.Forms.TextBox
	Public WithEvents chkSkipHeader As System.Windows.Forms.CheckBox
	Public WithEvents _optHeader_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optHeader_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optHeader_1 As System.Windows.Forms.RadioButton
	Public WithEvents txtHeaderStart As System.Windows.Forms.TextBox
	Public WithEvents fraHeader As System.Windows.Forms.GroupBox
	Public WithEvents txtLineLen As System.Windows.Forms.TextBox
	Public WithEvents txtLineEndChar As System.Windows.Forms.TextBox
	Public WithEvents _optLineEnd_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optLineEnd_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optLineEnd_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optLineEnd_3 As System.Windows.Forms.RadioButton
	Public WithEvents fraLineEnd As System.Windows.Forms.GroupBox
	Public WithEvents _optDelimiter_3 As System.Windows.Forms.RadioButton
	Public WithEvents _optDelimiter_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optDelimiter_1 As System.Windows.Forms.RadioButton
	Public WithEvents txtDelimiter As System.Windows.Forms.TextBox
	Public WithEvents _optDelimiter_0 As System.Windows.Forms.RadioButton
	Public WithEvents fraColumns As System.Windows.Forms.GroupBox
	Public WithEvents cmdBrowseDesc As System.Windows.Forms.Button
	Public WithEvents txtScriptFile As System.Windows.Forms.TextBox
	Public WithEvents cmdBrowseData As System.Windows.Forms.Button
	Public WithEvents txtDataFile As System.Windows.Forms.TextBox
	Public WithEvents lblScriptDesc As System.Windows.Forms.Label
	Public WithEvents lblDataDescFile As System.Windows.Forms.Label
	Public WithEvents lblDataFile As System.Windows.Forms.Label
	Public WithEvents _fraTab_1 As System.Windows.Forms.Panel
	Public WithEvents tabTop As AxComctlLib.AxTabStrip
	Public WithEvents fraSash As System.Windows.Forms.Panel
	Public dlgOpenFileOpen As System.Windows.Forms.OpenFileDialog
	Public dlgOpenFileSave As System.Windows.Forms.SaveFileDialog
	Public WithEvents cmdSaveDesc As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOk As System.Windows.Forms.Button
	Public WithEvents fraButtons As System.Windows.Forms.Panel
	Public WithEvents VScrollSample As System.Windows.Forms.VScrollBar
	Public WithEvents HScrollSample As System.Windows.Forms.HScrollBar
	Public WithEvents txtRuler2 As System.Windows.Forms.TextBox
	Public WithEvents _txtSample_0 As System.Windows.Forms.TextBox
	Public WithEvents txtRuler1 As System.Windows.Forms.TextBox
	Public WithEvents fraTextSample As System.Windows.Forms.Panel
	Public WithEvents agdSample As AxATCoCtl.AxATCoGrid
	Public WithEvents lblInputColumns As System.Windows.Forms.Label
	Public WithEvents fraColSample As System.Windows.Forms.Panel
	Public WithEvents fraTab As Microsoft.VisualBasic.Compatibility.VB6.PanelArray
	Public WithEvents optDelimiter As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optHeader As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents optLineEnd As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
	Public WithEvents txtSample As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmInputWizard))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me._fraTab_2 = New System.Windows.Forms.Panel
		Me.agdDataMapping = New AxATCoCtl.AxATCoGrid
		Me._fraTab_1 = New System.Windows.Forms.Panel
		Me.txtScriptDesc = New System.Windows.Forms.TextBox
		Me.fraHeader = New System.Windows.Forms.GroupBox
		Me.txtHeaderLines = New System.Windows.Forms.TextBox
		Me.chkSkipHeader = New System.Windows.Forms.CheckBox
		Me._optHeader_3 = New System.Windows.Forms.RadioButton
		Me._optHeader_2 = New System.Windows.Forms.RadioButton
		Me._optHeader_1 = New System.Windows.Forms.RadioButton
		Me.txtHeaderStart = New System.Windows.Forms.TextBox
		Me.fraLineEnd = New System.Windows.Forms.GroupBox
		Me.txtLineLen = New System.Windows.Forms.TextBox
		Me.txtLineEndChar = New System.Windows.Forms.TextBox
		Me._optLineEnd_0 = New System.Windows.Forms.RadioButton
		Me._optLineEnd_2 = New System.Windows.Forms.RadioButton
		Me._optLineEnd_1 = New System.Windows.Forms.RadioButton
		Me._optLineEnd_3 = New System.Windows.Forms.RadioButton
		Me.fraColumns = New System.Windows.Forms.GroupBox
		Me._optDelimiter_3 = New System.Windows.Forms.RadioButton
		Me._optDelimiter_2 = New System.Windows.Forms.RadioButton
		Me._optDelimiter_1 = New System.Windows.Forms.RadioButton
		Me.txtDelimiter = New System.Windows.Forms.TextBox
		Me._optDelimiter_0 = New System.Windows.Forms.RadioButton
		Me.cmdBrowseDesc = New System.Windows.Forms.Button
		Me.txtScriptFile = New System.Windows.Forms.TextBox
		Me.cmdBrowseData = New System.Windows.Forms.Button
		Me.txtDataFile = New System.Windows.Forms.TextBox
		Me.lblScriptDesc = New System.Windows.Forms.Label
		Me.lblDataDescFile = New System.Windows.Forms.Label
		Me.lblDataFile = New System.Windows.Forms.Label
		Me.tabTop = New AxComctlLib.AxTabStrip
		Me.fraSash = New System.Windows.Forms.Panel
		Me.dlgOpenFileOpen = New System.Windows.Forms.OpenFileDialog
		Me.dlgOpenFileSave = New System.Windows.Forms.SaveFileDialog
		Me.fraButtons = New System.Windows.Forms.Panel
		Me.cmdSaveDesc = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOk = New System.Windows.Forms.Button
		Me.fraTextSample = New System.Windows.Forms.Panel
		Me.VScrollSample = New System.Windows.Forms.VScrollBar
		Me.HScrollSample = New System.Windows.Forms.HScrollBar
		Me.txtRuler2 = New System.Windows.Forms.TextBox
		Me._txtSample_0 = New System.Windows.Forms.TextBox
		Me.txtRuler1 = New System.Windows.Forms.TextBox
		Me.fraColSample = New System.Windows.Forms.Panel
		Me.agdSample = New AxATCoCtl.AxATCoGrid
		Me.lblInputColumns = New System.Windows.Forms.Label
		Me.fraTab = New Microsoft.VisualBasic.Compatibility.VB6.PanelArray(components)
		Me.optDelimiter = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(components)
		Me.optHeader = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(components)
		Me.optLineEnd = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(components)
		Me.txtSample = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(components)
		Me._fraTab_2.SuspendLayout()
		Me._fraTab_1.SuspendLayout()
		Me.fraHeader.SuspendLayout()
		Me.fraLineEnd.SuspendLayout()
		Me.fraColumns.SuspendLayout()
		Me.fraButtons.SuspendLayout()
		Me.fraTextSample.SuspendLayout()
		Me.fraColSample.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.agdDataMapping, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tabTop, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.agdSample, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.fraTab, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optDelimiter, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optHeader, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.optLineEnd, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtSample, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Text = "Script Creation Wizard"
		Me.ClientSize = New System.Drawing.Size(947, 473)
		Me.Location = New System.Drawing.Point(4, 23)
		Me.Font = New System.Drawing.Font("Courier New", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MaximizeBox = True
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "frmInputWizard"
		Me._fraTab_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._fraTab_2.Text = "Frame1"
		Me._fraTab_2.Size = New System.Drawing.Size(457, 225)
		Me._fraTab_2.Location = New System.Drawing.Point(528, 40)
		Me._fraTab_2.TabIndex = 38
		Me._fraTab_2.Visible = False
		Me._fraTab_2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._fraTab_2.BackColor = System.Drawing.SystemColors.Control
		Me._fraTab_2.Enabled = True
		Me._fraTab_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._fraTab_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._fraTab_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._fraTab_2.Name = "_fraTab_2"
		agdDataMapping.OcxState = CType(resources.GetObject("agdDataMapping.OcxState"), System.Windows.Forms.AxHost.State)
		Me.agdDataMapping.Size = New System.Drawing.Size(521, 201)
		Me.agdDataMapping.Location = New System.Drawing.Point(0, 0)
		Me.agdDataMapping.TabIndex = 23
		Me.agdDataMapping.Name = "agdDataMapping"
		Me._fraTab_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._fraTab_1.Text = "Frame1"
		Me._fraTab_1.Size = New System.Drawing.Size(471, 161)
		Me._fraTab_1.Location = New System.Drawing.Point(19, 40)
		Me._fraTab_1.TabIndex = 39
		Me._fraTab_1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._fraTab_1.BackColor = System.Drawing.SystemColors.Control
		Me._fraTab_1.Enabled = True
		Me._fraTab_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._fraTab_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._fraTab_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._fraTab_1.Visible = True
		Me._fraTab_1.Name = "_fraTab_1"
		Me.txtScriptDesc.AutoSize = False
		Me.txtScriptDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtScriptDesc.Size = New System.Drawing.Size(337, 19)
		Me.txtScriptDesc.Location = New System.Drawing.Point(80, 40)
		Me.txtScriptDesc.TabIndex = 5
		Me.txtScriptDesc.Text = "txtScriptDesc"
		Me.txtScriptDesc.AcceptsReturn = True
		Me.txtScriptDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtScriptDesc.BackColor = System.Drawing.SystemColors.Window
		Me.txtScriptDesc.CausesValidation = True
		Me.txtScriptDesc.Enabled = True
		Me.txtScriptDesc.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtScriptDesc.HideSelection = True
		Me.txtScriptDesc.ReadOnly = False
		Me.txtScriptDesc.Maxlength = 0
		Me.txtScriptDesc.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtScriptDesc.MultiLine = False
		Me.txtScriptDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtScriptDesc.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtScriptDesc.TabStop = True
		Me.txtScriptDesc.Visible = True
		Me.txtScriptDesc.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtScriptDesc.Name = "txtScriptDesc"
		Me.fraHeader.Text = "Header"
		Me.fraHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraHeader.Size = New System.Drawing.Size(129, 89)
		Me.fraHeader.Location = New System.Drawing.Point(0, 64)
		Me.fraHeader.TabIndex = 42
		Me.fraHeader.BackColor = System.Drawing.SystemColors.Control
		Me.fraHeader.Enabled = True
		Me.fraHeader.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraHeader.Visible = True
		Me.fraHeader.Padding = New System.Windows.Forms.Padding(0)
		Me.fraHeader.Name = "fraHeader"
		Me.txtHeaderLines.AutoSize = False
		Me.txtHeaderLines.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtHeaderLines.Size = New System.Drawing.Size(25, 20)
		Me.txtHeaderLines.Location = New System.Drawing.Point(96, 64)
		Me.txtHeaderLines.TabIndex = 11
		Me.txtHeaderLines.Text = "1"
		Me.ToolTip1.SetToolTip(Me.txtHeaderLines, "Single printable character delimiter")
		Me.txtHeaderLines.AcceptsReturn = True
		Me.txtHeaderLines.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtHeaderLines.BackColor = System.Drawing.SystemColors.Window
		Me.txtHeaderLines.CausesValidation = True
		Me.txtHeaderLines.Enabled = True
		Me.txtHeaderLines.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtHeaderLines.HideSelection = True
		Me.txtHeaderLines.ReadOnly = False
		Me.txtHeaderLines.Maxlength = 0
		Me.txtHeaderLines.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtHeaderLines.MultiLine = False
		Me.txtHeaderLines.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtHeaderLines.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtHeaderLines.TabStop = True
		Me.txtHeaderLines.Visible = True
		Me.txtHeaderLines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtHeaderLines.Name = "txtHeaderLines"
		Me.chkSkipHeader.Text = "Skip"
		Me.chkSkipHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkSkipHeader.Size = New System.Drawing.Size(113, 17)
		Me.chkSkipHeader.Location = New System.Drawing.Point(8, 16)
		Me.chkSkipHeader.TabIndex = 6
		Me.ToolTip1.SetToolTip(Me.chkSkipHeader, "Do not search header for any information")
		Me.chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkSkipHeader.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkSkipHeader.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkSkipHeader.BackColor = System.Drawing.SystemColors.Control
		Me.chkSkipHeader.CausesValidation = True
		Me.chkSkipHeader.Enabled = True
		Me.chkSkipHeader.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkSkipHeader.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkSkipHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkSkipHeader.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkSkipHeader.TabStop = True
		Me.chkSkipHeader.Visible = True
		Me.chkSkipHeader.Name = "chkSkipHeader"
		Me._optHeader_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_3.Text = "Lines"
		Me._optHeader_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optHeader_3.Size = New System.Drawing.Size(89, 17)
		Me._optHeader_3.Location = New System.Drawing.Point(8, 64)
		Me._optHeader_3.TabIndex = 10
		Me._optHeader_3.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_3.BackColor = System.Drawing.SystemColors.Control
		Me._optHeader_3.CausesValidation = True
		Me._optHeader_3.Enabled = True
		Me._optHeader_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optHeader_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._optHeader_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optHeader_3.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optHeader_3.TabStop = True
		Me._optHeader_3.Checked = False
		Me._optHeader_3.Visible = True
		Me._optHeader_3.Name = "_optHeader_3"
		Me._optHeader_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_2.Text = "Starts With"
		Me._optHeader_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optHeader_2.Size = New System.Drawing.Size(89, 17)
		Me._optHeader_2.Location = New System.Drawing.Point(8, 48)
		Me._optHeader_2.TabIndex = 8
		Me._optHeader_2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_2.BackColor = System.Drawing.SystemColors.Control
		Me._optHeader_2.CausesValidation = True
		Me._optHeader_2.Enabled = True
		Me._optHeader_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optHeader_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optHeader_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optHeader_2.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optHeader_2.TabStop = True
		Me._optHeader_2.Checked = False
		Me._optHeader_2.Visible = True
		Me._optHeader_2.Name = "_optHeader_2"
		Me._optHeader_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_1.Text = "None"
		Me._optHeader_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optHeader_1.Size = New System.Drawing.Size(113, 17)
		Me._optHeader_1.Location = New System.Drawing.Point(8, 32)
		Me._optHeader_1.TabIndex = 7
		Me._optHeader_1.Checked = True
		Me._optHeader_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optHeader_1.BackColor = System.Drawing.SystemColors.Control
		Me._optHeader_1.CausesValidation = True
		Me._optHeader_1.Enabled = True
		Me._optHeader_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optHeader_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optHeader_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optHeader_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optHeader_1.TabStop = True
		Me._optHeader_1.Visible = True
		Me._optHeader_1.Name = "_optHeader_1"
		Me.txtHeaderStart.AutoSize = False
		Me.txtHeaderStart.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtHeaderStart.Size = New System.Drawing.Size(25, 19)
		Me.txtHeaderStart.Location = New System.Drawing.Point(96, 48)
		Me.txtHeaderStart.TabIndex = 9
		Me.txtHeaderStart.Text = "#"
		Me.ToolTip1.SetToolTip(Me.txtHeaderStart, "Single printable character delimiter")
		Me.txtHeaderStart.AcceptsReturn = True
		Me.txtHeaderStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtHeaderStart.BackColor = System.Drawing.SystemColors.Window
		Me.txtHeaderStart.CausesValidation = True
		Me.txtHeaderStart.Enabled = True
		Me.txtHeaderStart.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtHeaderStart.HideSelection = True
		Me.txtHeaderStart.ReadOnly = False
		Me.txtHeaderStart.Maxlength = 0
		Me.txtHeaderStart.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtHeaderStart.MultiLine = False
		Me.txtHeaderStart.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtHeaderStart.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtHeaderStart.TabStop = True
		Me.txtHeaderStart.Visible = True
		Me.txtHeaderStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtHeaderStart.Name = "txtHeaderStart"
		Me.fraLineEnd.Text = "Line Ending"
		Me.fraLineEnd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraLineEnd.Size = New System.Drawing.Size(129, 89)
		Me.fraLineEnd.Location = New System.Drawing.Point(272, 64)
		Me.fraLineEnd.TabIndex = 41
		Me.fraLineEnd.BackColor = System.Drawing.SystemColors.Control
		Me.fraLineEnd.Enabled = True
		Me.fraLineEnd.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraLineEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraLineEnd.Visible = True
		Me.fraLineEnd.Padding = New System.Windows.Forms.Padding(0)
		Me.fraLineEnd.Name = "fraLineEnd"
		Me.txtLineLen.AutoSize = False
		Me.txtLineLen.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtLineLen.Size = New System.Drawing.Size(25, 20)
		Me.txtLineLen.Location = New System.Drawing.Point(96, 64)
		Me.txtLineLen.TabIndex = 22
		Me.txtLineLen.Text = "80"
		Me.ToolTip1.SetToolTip(Me.txtLineLen, "Single printable character delimiter")
		Me.txtLineLen.AcceptsReturn = True
		Me.txtLineLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLineLen.BackColor = System.Drawing.SystemColors.Window
		Me.txtLineLen.CausesValidation = True
		Me.txtLineLen.Enabled = True
		Me.txtLineLen.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLineLen.HideSelection = True
		Me.txtLineLen.ReadOnly = False
		Me.txtLineLen.Maxlength = 0
		Me.txtLineLen.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLineLen.MultiLine = False
		Me.txtLineLen.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLineLen.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLineLen.TabStop = True
		Me.txtLineLen.Visible = True
		Me.txtLineLen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLineLen.Name = "txtLineLen"
		Me.txtLineEndChar.AutoSize = False
		Me.txtLineEndChar.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtLineEndChar.Size = New System.Drawing.Size(25, 19)
		Me.txtLineEndChar.Location = New System.Drawing.Point(96, 48)
		Me.txtLineEndChar.TabIndex = 20
		Me.txtLineEndChar.Text = "13"
		Me.ToolTip1.SetToolTip(Me.txtLineEndChar, "Single printable character delimiter")
		Me.txtLineEndChar.AcceptsReturn = True
		Me.txtLineEndChar.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLineEndChar.BackColor = System.Drawing.SystemColors.Window
		Me.txtLineEndChar.CausesValidation = True
		Me.txtLineEndChar.Enabled = True
		Me.txtLineEndChar.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLineEndChar.HideSelection = True
		Me.txtLineEndChar.ReadOnly = False
		Me.txtLineEndChar.Maxlength = 0
		Me.txtLineEndChar.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLineEndChar.MultiLine = False
		Me.txtLineEndChar.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLineEndChar.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLineEndChar.TabStop = True
		Me.txtLineEndChar.Visible = True
		Me.txtLineEndChar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLineEndChar.Name = "txtLineEndChar"
		Me._optLineEnd_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_0.Text = "CR/LF or CR"
		Me._optLineEnd_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optLineEnd_0.Size = New System.Drawing.Size(113, 17)
		Me._optLineEnd_0.Location = New System.Drawing.Point(8, 16)
		Me._optLineEnd_0.TabIndex = 17
		Me._optLineEnd_0.Checked = True
		Me._optLineEnd_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_0.BackColor = System.Drawing.SystemColors.Control
		Me._optLineEnd_0.CausesValidation = True
		Me._optLineEnd_0.Enabled = True
		Me._optLineEnd_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optLineEnd_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optLineEnd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optLineEnd_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optLineEnd_0.TabStop = True
		Me._optLineEnd_0.Visible = True
		Me._optLineEnd_0.Name = "_optLineEnd_0"
		Me._optLineEnd_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_2.Text = "ASCII Char:"
		Me._optLineEnd_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optLineEnd_2.Size = New System.Drawing.Size(97, 17)
		Me._optLineEnd_2.Location = New System.Drawing.Point(8, 48)
		Me._optLineEnd_2.TabIndex = 19
		Me._optLineEnd_2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_2.BackColor = System.Drawing.SystemColors.Control
		Me._optLineEnd_2.CausesValidation = True
		Me._optLineEnd_2.Enabled = True
		Me._optLineEnd_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optLineEnd_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optLineEnd_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optLineEnd_2.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optLineEnd_2.TabStop = True
		Me._optLineEnd_2.Checked = False
		Me._optLineEnd_2.Visible = True
		Me._optLineEnd_2.Name = "_optLineEnd_2"
		Me._optLineEnd_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_1.Text = "LF"
		Me._optLineEnd_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optLineEnd_1.Size = New System.Drawing.Size(97, 17)
		Me._optLineEnd_1.Location = New System.Drawing.Point(8, 32)
		Me._optLineEnd_1.TabIndex = 18
		Me._optLineEnd_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_1.BackColor = System.Drawing.SystemColors.Control
		Me._optLineEnd_1.CausesValidation = True
		Me._optLineEnd_1.Enabled = True
		Me._optLineEnd_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optLineEnd_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optLineEnd_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optLineEnd_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optLineEnd_1.TabStop = True
		Me._optLineEnd_1.Checked = False
		Me._optLineEnd_1.Visible = True
		Me._optLineEnd_1.Name = "_optLineEnd_1"
		Me._optLineEnd_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_3.Text = "Line Length:"
		Me._optLineEnd_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optLineEnd_3.Size = New System.Drawing.Size(97, 17)
		Me._optLineEnd_3.Location = New System.Drawing.Point(8, 64)
		Me._optLineEnd_3.TabIndex = 21
		Me._optLineEnd_3.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optLineEnd_3.BackColor = System.Drawing.SystemColors.Control
		Me._optLineEnd_3.CausesValidation = True
		Me._optLineEnd_3.Enabled = True
		Me._optLineEnd_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optLineEnd_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._optLineEnd_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optLineEnd_3.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optLineEnd_3.TabStop = True
		Me._optLineEnd_3.Checked = False
		Me._optLineEnd_3.Visible = True
		Me._optLineEnd_3.Name = "_optLineEnd_3"
		Me.fraColumns.Text = "Column Format"
		Me.fraColumns.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraColumns.Size = New System.Drawing.Size(129, 89)
		Me.fraColumns.Location = New System.Drawing.Point(136, 64)
		Me.fraColumns.TabIndex = 40
		Me.fraColumns.BackColor = System.Drawing.SystemColors.Control
		Me.fraColumns.Enabled = True
		Me.fraColumns.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraColumns.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraColumns.Visible = True
		Me.fraColumns.Padding = New System.Windows.Forms.Padding(0)
		Me.fraColumns.Name = "fraColumns"
		Me._optDelimiter_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_3.Text = "Character:"
		Me._optDelimiter_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optDelimiter_3.Size = New System.Drawing.Size(89, 17)
		Me._optDelimiter_3.Location = New System.Drawing.Point(8, 64)
		Me._optDelimiter_3.TabIndex = 15
		Me._optDelimiter_3.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_3.BackColor = System.Drawing.SystemColors.Control
		Me._optDelimiter_3.CausesValidation = True
		Me._optDelimiter_3.Enabled = True
		Me._optDelimiter_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optDelimiter_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._optDelimiter_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optDelimiter_3.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optDelimiter_3.TabStop = True
		Me._optDelimiter_3.Checked = False
		Me._optDelimiter_3.Visible = True
		Me._optDelimiter_3.Name = "_optDelimiter_3"
		Me._optDelimiter_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_2.Text = "Tab Delimited"
		Me._optDelimiter_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optDelimiter_2.Size = New System.Drawing.Size(113, 17)
		Me._optDelimiter_2.Location = New System.Drawing.Point(8, 32)
		Me._optDelimiter_2.TabIndex = 13
		Me._optDelimiter_2.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_2.BackColor = System.Drawing.SystemColors.Control
		Me._optDelimiter_2.CausesValidation = True
		Me._optDelimiter_2.Enabled = True
		Me._optDelimiter_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optDelimiter_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._optDelimiter_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optDelimiter_2.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optDelimiter_2.TabStop = True
		Me._optDelimiter_2.Checked = False
		Me._optDelimiter_2.Visible = True
		Me._optDelimiter_2.Name = "_optDelimiter_2"
		Me._optDelimiter_1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_1.Text = "Space Delimited"
		Me._optDelimiter_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optDelimiter_1.Size = New System.Drawing.Size(117, 17)
		Me._optDelimiter_1.Location = New System.Drawing.Point(8, 48)
		Me._optDelimiter_1.TabIndex = 14
		Me._optDelimiter_1.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_1.BackColor = System.Drawing.SystemColors.Control
		Me._optDelimiter_1.CausesValidation = True
		Me._optDelimiter_1.Enabled = True
		Me._optDelimiter_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optDelimiter_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._optDelimiter_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optDelimiter_1.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optDelimiter_1.TabStop = True
		Me._optDelimiter_1.Checked = False
		Me._optDelimiter_1.Visible = True
		Me._optDelimiter_1.Name = "_optDelimiter_1"
		Me.txtDelimiter.AutoSize = False
		Me.txtDelimiter.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtDelimiter.Size = New System.Drawing.Size(17, 19)
		Me.txtDelimiter.Location = New System.Drawing.Point(96, 64)
		Me.txtDelimiter.TabIndex = 16
		Me.txtDelimiter.Text = ","
		Me.ToolTip1.SetToolTip(Me.txtDelimiter, "Single printable character delimiter")
		Me.txtDelimiter.AcceptsReturn = True
		Me.txtDelimiter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDelimiter.BackColor = System.Drawing.SystemColors.Window
		Me.txtDelimiter.CausesValidation = True
		Me.txtDelimiter.Enabled = True
		Me.txtDelimiter.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDelimiter.HideSelection = True
		Me.txtDelimiter.ReadOnly = False
		Me.txtDelimiter.Maxlength = 0
		Me.txtDelimiter.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDelimiter.MultiLine = False
		Me.txtDelimiter.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDelimiter.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDelimiter.TabStop = True
		Me.txtDelimiter.Visible = True
		Me.txtDelimiter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDelimiter.Name = "txtDelimiter"
		Me._optDelimiter_0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_0.Text = "Fixed Width"
		Me._optDelimiter_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._optDelimiter_0.Size = New System.Drawing.Size(113, 17)
		Me._optDelimiter_0.Location = New System.Drawing.Point(8, 16)
		Me._optDelimiter_0.TabIndex = 12
		Me._optDelimiter_0.Checked = True
		Me._optDelimiter_0.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me._optDelimiter_0.BackColor = System.Drawing.SystemColors.Control
		Me._optDelimiter_0.CausesValidation = True
		Me._optDelimiter_0.Enabled = True
		Me._optDelimiter_0.ForeColor = System.Drawing.SystemColors.ControlText
		Me._optDelimiter_0.Cursor = System.Windows.Forms.Cursors.Default
		Me._optDelimiter_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._optDelimiter_0.Appearance = System.Windows.Forms.Appearance.Normal
		Me._optDelimiter_0.TabStop = True
		Me._optDelimiter_0.Visible = True
		Me._optDelimiter_0.Name = "_optDelimiter_0"
		Me.cmdBrowseDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdBrowseDesc.Text = "Browse"
		Me.cmdBrowseDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdBrowseDesc.Size = New System.Drawing.Size(57, 19)
		Me.cmdBrowseDesc.Location = New System.Drawing.Point(424, 24)
		Me.cmdBrowseDesc.TabIndex = 4
		Me.cmdBrowseDesc.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBrowseDesc.CausesValidation = True
		Me.cmdBrowseDesc.Enabled = True
		Me.cmdBrowseDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBrowseDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBrowseDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdBrowseDesc.TabStop = True
		Me.cmdBrowseDesc.Name = "cmdBrowseDesc"
		Me.txtScriptFile.AutoSize = False
		Me.txtScriptFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtScriptFile.Size = New System.Drawing.Size(337, 19)
		Me.txtScriptFile.Location = New System.Drawing.Point(80, 24)
		Me.txtScriptFile.TabIndex = 3
		Me.txtScriptFile.Text = "txtScriptFile"
		Me.txtScriptFile.AcceptsReturn = True
		Me.txtScriptFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtScriptFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtScriptFile.CausesValidation = True
		Me.txtScriptFile.Enabled = True
		Me.txtScriptFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtScriptFile.HideSelection = True
		Me.txtScriptFile.ReadOnly = False
		Me.txtScriptFile.Maxlength = 0
		Me.txtScriptFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtScriptFile.MultiLine = False
		Me.txtScriptFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtScriptFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtScriptFile.TabStop = True
		Me.txtScriptFile.Visible = True
		Me.txtScriptFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtScriptFile.Name = "txtScriptFile"
		Me.cmdBrowseData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdBrowseData.Text = "Browse"
		Me.cmdBrowseData.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdBrowseData.Size = New System.Drawing.Size(57, 19)
		Me.cmdBrowseData.Location = New System.Drawing.Point(424, 0)
		Me.cmdBrowseData.TabIndex = 2
		Me.cmdBrowseData.BackColor = System.Drawing.SystemColors.Control
		Me.cmdBrowseData.CausesValidation = True
		Me.cmdBrowseData.Enabled = True
		Me.cmdBrowseData.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdBrowseData.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdBrowseData.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdBrowseData.TabStop = True
		Me.cmdBrowseData.Name = "cmdBrowseData"
		Me.txtDataFile.AutoSize = False
		Me.txtDataFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtDataFile.Size = New System.Drawing.Size(337, 19)
		Me.txtDataFile.Location = New System.Drawing.Point(80, 0)
		Me.txtDataFile.TabIndex = 1
		Me.txtDataFile.Text = "txtDataFile"
		Me.ToolTip1.SetToolTip(Me.txtDataFile, "Name of file containing data to import")
		Me.txtDataFile.AcceptsReturn = True
		Me.txtDataFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDataFile.BackColor = System.Drawing.SystemColors.Window
		Me.txtDataFile.CausesValidation = True
		Me.txtDataFile.Enabled = True
		Me.txtDataFile.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDataFile.HideSelection = True
		Me.txtDataFile.ReadOnly = False
		Me.txtDataFile.Maxlength = 0
		Me.txtDataFile.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDataFile.MultiLine = False
		Me.txtDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDataFile.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDataFile.TabStop = True
		Me.txtDataFile.Visible = True
		Me.txtDataFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDataFile.Name = "txtDataFile"
		Me.lblScriptDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.lblScriptDesc.Text = "Description:"
		Me.lblScriptDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblScriptDesc.Size = New System.Drawing.Size(73, 17)
		Me.lblScriptDesc.Location = New System.Drawing.Point(0, 40)
		Me.lblScriptDesc.TabIndex = 45
		Me.lblScriptDesc.BackColor = System.Drawing.SystemColors.Control
		Me.lblScriptDesc.Enabled = True
		Me.lblScriptDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblScriptDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblScriptDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblScriptDesc.UseMnemonic = True
		Me.lblScriptDesc.Visible = True
		Me.lblScriptDesc.AutoSize = False
		Me.lblScriptDesc.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblScriptDesc.Name = "lblScriptDesc"
		Me.lblDataDescFile.Text = "Script File:"
		Me.lblDataDescFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDataDescFile.Size = New System.Drawing.Size(81, 17)
		Me.lblDataDescFile.Location = New System.Drawing.Point(0, 24)
		Me.lblDataDescFile.TabIndex = 44
		Me.lblDataDescFile.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDataDescFile.BackColor = System.Drawing.SystemColors.Control
		Me.lblDataDescFile.Enabled = True
		Me.lblDataDescFile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDataDescFile.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDataDescFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDataDescFile.UseMnemonic = True
		Me.lblDataDescFile.Visible = True
		Me.lblDataDescFile.AutoSize = False
		Me.lblDataDescFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDataDescFile.Name = "lblDataDescFile"
		Me.lblDataFile.Text = "Data File:"
		Me.lblDataFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDataFile.Size = New System.Drawing.Size(81, 20)
		Me.lblDataFile.Location = New System.Drawing.Point(0, 0)
		Me.lblDataFile.TabIndex = 43
		Me.lblDataFile.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDataFile.BackColor = System.Drawing.SystemColors.Control
		Me.lblDataFile.Enabled = True
		Me.lblDataFile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDataFile.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDataFile.UseMnemonic = True
		Me.lblDataFile.Visible = True
		Me.lblDataFile.AutoSize = False
		Me.lblDataFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDataFile.Name = "lblDataFile"
		tabTop.OcxState = CType(resources.GetObject("tabTop.OcxState"), System.Windows.Forms.AxHost.State)
		Me.tabTop.CausesValidation = False
		Me.tabTop.Size = New System.Drawing.Size(497, 201)
		Me.tabTop.Location = New System.Drawing.Point(8, 8)
		Me.tabTop.TabIndex = 0
		Me.tabTop.Name = "tabTop"
		Me.fraSash.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraSash.Text = "Frame3"
		Me.fraSash.Size = New System.Drawing.Size(537, 9)
		Me.fraSash.Location = New System.Drawing.Point(0, 216)
		Me.fraSash.Cursor = System.Windows.Forms.Cursors.SizeNS
		Me.fraSash.TabIndex = 37
		Me.fraSash.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraSash.BackColor = System.Drawing.SystemColors.Control
		Me.fraSash.Enabled = True
		Me.fraSash.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraSash.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraSash.Visible = True
		Me.fraSash.Name = "fraSash"
		Me.fraButtons.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraButtons.Text = "Frame3"
		Me.fraButtons.Size = New System.Drawing.Size(297, 25)
		Me.fraButtons.Location = New System.Drawing.Point(96, 440)
		Me.fraButtons.TabIndex = 35
		Me.fraButtons.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
		Me.fraButtons.Enabled = True
		Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraButtons.Visible = True
		Me.fraButtons.Name = "fraButtons"
		Me.cmdSaveDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSaveDesc.Text = "&Save Script"
		Me.cmdSaveDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSaveDesc.Size = New System.Drawing.Size(89, 25)
		Me.cmdSaveDesc.Location = New System.Drawing.Point(104, 0)
		Me.cmdSaveDesc.TabIndex = 25
		Me.ToolTip1.SetToolTip(Me.cmdSaveDesc, "Save selections and data mapping information to a data descriptor file.")
		Me.cmdSaveDesc.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSaveDesc.CausesValidation = True
		Me.cmdSaveDesc.Enabled = True
		Me.cmdSaveDesc.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSaveDesc.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSaveDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSaveDesc.TabStop = True
		Me.cmdSaveDesc.Name = "cmdSaveDesc"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "&Cancel"
		Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
		Me.cmdCancel.Location = New System.Drawing.Point(208, 0)
		Me.cmdCancel.TabIndex = 26
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdOk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOk.Text = "&Read Data"
		Me.cmdOk.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOk.Size = New System.Drawing.Size(89, 25)
		Me.cmdOk.Location = New System.Drawing.Point(0, 0)
		Me.cmdOk.TabIndex = 24
		Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOk.CausesValidation = True
		Me.cmdOk.Enabled = True
		Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOk.TabStop = True
		Me.cmdOk.Name = "cmdOk"
		Me.fraTextSample.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraTextSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraTextSample.Size = New System.Drawing.Size(489, 145)
		Me.fraTextSample.Location = New System.Drawing.Point(8, 232)
		Me.fraTextSample.TabIndex = 27
		Me.fraTextSample.BackColor = System.Drawing.SystemColors.Control
		Me.fraTextSample.Enabled = True
		Me.fraTextSample.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraTextSample.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraTextSample.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraTextSample.Visible = True
		Me.fraTextSample.Name = "fraTextSample"
		Me.VScrollSample.Size = New System.Drawing.Size(14, 81)
		Me.VScrollSample.LargeChange = 5
		Me.VScrollSample.Location = New System.Drawing.Point(472, 32)
		Me.VScrollSample.TabIndex = 36
		Me.VScrollSample.CausesValidation = True
		Me.VScrollSample.Enabled = True
		Me.VScrollSample.Maximum = 32771
		Me.VScrollSample.Minimum = 0
		Me.VScrollSample.Cursor = System.Windows.Forms.Cursors.Default
		Me.VScrollSample.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.VScrollSample.SmallChange = 1
		Me.VScrollSample.TabStop = True
		Me.VScrollSample.Value = 0
		Me.VScrollSample.Visible = True
		Me.VScrollSample.Name = "VScrollSample"
		Me.HScrollSample.Size = New System.Drawing.Size(489, 14)
		Me.HScrollSample.LargeChange = 40
		Me.HScrollSample.Location = New System.Drawing.Point(0, 132)
		Me.HScrollSample.Maximum = 1039
		Me.HScrollSample.Minimum = 1
		Me.HScrollSample.TabIndex = 34
		Me.HScrollSample.Value = 1
		Me.HScrollSample.CausesValidation = True
		Me.HScrollSample.Enabled = True
		Me.HScrollSample.Cursor = System.Windows.Forms.Cursors.Default
		Me.HScrollSample.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HScrollSample.SmallChange = 1
		Me.HScrollSample.TabStop = True
		Me.HScrollSample.Visible = True
		Me.HScrollSample.Name = "HScrollSample"
		Me.txtRuler2.AutoSize = False
		Me.txtRuler2.BackColor = System.Drawing.SystemColors.Control
		Me.txtRuler2.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtRuler2.Size = New System.Drawing.Size(489, 15)
		Me.txtRuler2.HideSelection = False
		Me.txtRuler2.Location = New System.Drawing.Point(0, 16)
		Me.txtRuler2.ReadOnly = True
		Me.txtRuler2.TabIndex = 33
		Me.txtRuler2.Text = "1234567890"
		Me.txtRuler2.AcceptsReturn = True
		Me.txtRuler2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRuler2.CausesValidation = True
		Me.txtRuler2.Enabled = True
		Me.txtRuler2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRuler2.Maxlength = 0
		Me.txtRuler2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRuler2.MultiLine = False
		Me.txtRuler2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRuler2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRuler2.TabStop = True
		Me.txtRuler2.Visible = True
		Me.txtRuler2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtRuler2.Name = "txtRuler2"
		Me._txtSample_0.AutoSize = False
		Me._txtSample_0.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtSample_0.Size = New System.Drawing.Size(473, 17)
		Me._txtSample_0.HideSelection = False
		Me._txtSample_0.Location = New System.Drawing.Point(0, 32)
		Me._txtSample_0.ReadOnly = True
		Me._txtSample_0.TabIndex = 29
		Me._txtSample_0.Text = "Sample"
		Me._txtSample_0.AcceptsReturn = True
		Me._txtSample_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtSample_0.BackColor = System.Drawing.SystemColors.Window
		Me._txtSample_0.CausesValidation = True
		Me._txtSample_0.Enabled = True
		Me._txtSample_0.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtSample_0.Maxlength = 0
		Me._txtSample_0.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtSample_0.MultiLine = False
		Me._txtSample_0.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtSample_0.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtSample_0.TabStop = True
		Me._txtSample_0.Visible = True
		Me._txtSample_0.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._txtSample_0.Name = "_txtSample_0"
		Me.txtRuler1.AutoSize = False
		Me.txtRuler1.BackColor = System.Drawing.SystemColors.Control
		Me.txtRuler1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtRuler1.Size = New System.Drawing.Size(489, 15)
		Me.txtRuler1.HideSelection = False
		Me.txtRuler1.Location = New System.Drawing.Point(0, 0)
		Me.txtRuler1.ReadOnly = True
		Me.txtRuler1.TabIndex = 32
		Me.txtRuler1.Text = "         1"
		Me.txtRuler1.AcceptsReturn = True
		Me.txtRuler1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtRuler1.CausesValidation = True
		Me.txtRuler1.Enabled = True
		Me.txtRuler1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtRuler1.Maxlength = 0
		Me.txtRuler1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtRuler1.MultiLine = False
		Me.txtRuler1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtRuler1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtRuler1.TabStop = True
		Me.txtRuler1.Visible = True
		Me.txtRuler1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.txtRuler1.Name = "txtRuler1"
		Me.fraColSample.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.fraColSample.Text = "fraColSample"
		Me.fraColSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraColSample.Size = New System.Drawing.Size(489, 153)
		Me.fraColSample.Location = New System.Drawing.Point(8, 232)
		Me.fraColSample.TabIndex = 28
		Me.fraColSample.Visible = False
		Me.fraColSample.BackColor = System.Drawing.SystemColors.Control
		Me.fraColSample.Enabled = True
		Me.fraColSample.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraColSample.Cursor = System.Windows.Forms.Cursors.Default
		Me.fraColSample.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraColSample.Name = "fraColSample"
		agdSample.OcxState = CType(resources.GetObject("agdSample.OcxState"), System.Windows.Forms.AxHost.State)
		Me.agdSample.Size = New System.Drawing.Size(489, 129)
		Me.agdSample.Location = New System.Drawing.Point(0, 16)
		Me.agdSample.TabIndex = 30
		Me.agdSample.Name = "agdSample"
		Me.lblInputColumns.Text = "Column Number:"
		Me.lblInputColumns.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblInputColumns.Size = New System.Drawing.Size(161, 17)
		Me.lblInputColumns.Location = New System.Drawing.Point(8, 0)
		Me.lblInputColumns.TabIndex = 31
		Me.lblInputColumns.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInputColumns.BackColor = System.Drawing.SystemColors.Control
		Me.lblInputColumns.Enabled = True
		Me.lblInputColumns.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInputColumns.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInputColumns.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInputColumns.UseMnemonic = True
		Me.lblInputColumns.Visible = True
		Me.lblInputColumns.AutoSize = False
		Me.lblInputColumns.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInputColumns.Name = "lblInputColumns"
		Me.Controls.Add(_fraTab_2)
		Me.Controls.Add(_fraTab_1)
		Me.Controls.Add(tabTop)
		Me.Controls.Add(fraSash)
		Me.Controls.Add(fraButtons)
		Me.Controls.Add(fraTextSample)
		Me.Controls.Add(fraColSample)
		Me._fraTab_2.Controls.Add(agdDataMapping)
		Me._fraTab_1.Controls.Add(txtScriptDesc)
		Me._fraTab_1.Controls.Add(fraHeader)
		Me._fraTab_1.Controls.Add(fraLineEnd)
		Me._fraTab_1.Controls.Add(fraColumns)
		Me._fraTab_1.Controls.Add(cmdBrowseDesc)
		Me._fraTab_1.Controls.Add(txtScriptFile)
		Me._fraTab_1.Controls.Add(cmdBrowseData)
		Me._fraTab_1.Controls.Add(txtDataFile)
		Me._fraTab_1.Controls.Add(lblScriptDesc)
		Me._fraTab_1.Controls.Add(lblDataDescFile)
		Me._fraTab_1.Controls.Add(lblDataFile)
		Me.fraHeader.Controls.Add(txtHeaderLines)
		Me.fraHeader.Controls.Add(chkSkipHeader)
		Me.fraHeader.Controls.Add(_optHeader_3)
		Me.fraHeader.Controls.Add(_optHeader_2)
		Me.fraHeader.Controls.Add(_optHeader_1)
		Me.fraHeader.Controls.Add(txtHeaderStart)
		Me.fraLineEnd.Controls.Add(txtLineLen)
		Me.fraLineEnd.Controls.Add(txtLineEndChar)
		Me.fraLineEnd.Controls.Add(_optLineEnd_0)
		Me.fraLineEnd.Controls.Add(_optLineEnd_2)
		Me.fraLineEnd.Controls.Add(_optLineEnd_1)
		Me.fraLineEnd.Controls.Add(_optLineEnd_3)
		Me.fraColumns.Controls.Add(_optDelimiter_3)
		Me.fraColumns.Controls.Add(_optDelimiter_2)
		Me.fraColumns.Controls.Add(_optDelimiter_1)
		Me.fraColumns.Controls.Add(txtDelimiter)
		Me.fraColumns.Controls.Add(_optDelimiter_0)
		Me.fraButtons.Controls.Add(cmdSaveDesc)
		Me.fraButtons.Controls.Add(cmdCancel)
		Me.fraButtons.Controls.Add(cmdOk)
		Me.fraTextSample.Controls.Add(VScrollSample)
		Me.fraTextSample.Controls.Add(HScrollSample)
		Me.fraTextSample.Controls.Add(txtRuler2)
		Me.fraTextSample.Controls.Add(_txtSample_0)
		Me.fraTextSample.Controls.Add(txtRuler1)
		Me.fraColSample.Controls.Add(agdSample)
		Me.fraColSample.Controls.Add(lblInputColumns)
		Me.fraTab.SetIndex(_fraTab_2, CType(2, Short))
		Me.fraTab.SetIndex(_fraTab_1, CType(1, Short))
		Me.optDelimiter.SetIndex(_optDelimiter_3, CType(3, Short))
		Me.optDelimiter.SetIndex(_optDelimiter_2, CType(2, Short))
		Me.optDelimiter.SetIndex(_optDelimiter_1, CType(1, Short))
		Me.optDelimiter.SetIndex(_optDelimiter_0, CType(0, Short))
		Me.optHeader.SetIndex(_optHeader_3, CType(3, Short))
		Me.optHeader.SetIndex(_optHeader_2, CType(2, Short))
		Me.optHeader.SetIndex(_optHeader_1, CType(1, Short))
		Me.optLineEnd.SetIndex(_optLineEnd_0, CType(0, Short))
		Me.optLineEnd.SetIndex(_optLineEnd_2, CType(2, Short))
		Me.optLineEnd.SetIndex(_optLineEnd_1, CType(1, Short))
		Me.optLineEnd.SetIndex(_optLineEnd_3, CType(3, Short))
		Me.txtSample.SetIndex(_txtSample_0, CType(0, Short))
		CType(Me.txtSample, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optLineEnd, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optHeader, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.optDelimiter, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.fraTab, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.agdSample, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tabTop, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.agdDataMapping, System.ComponentModel.ISupportInitialize).EndInit()
		Me._fraTab_2.ResumeLayout(False)
		Me._fraTab_1.ResumeLayout(False)
		Me.fraHeader.ResumeLayout(False)
		Me.fraLineEnd.ResumeLayout(False)
		Me.fraColumns.ResumeLayout(False)
		Me.fraButtons.ResumeLayout(False)
		Me.fraTextSample.ResumeLayout(False)
		Me.fraColSample.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class