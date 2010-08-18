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
    Public WithEvents agdDataMapping As atcControls.atcGrid
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
    Public WithEvents agdSample As atcControls.atcGrid
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
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtHeaderLines = New System.Windows.Forms.TextBox
        Me.chkSkipHeader = New System.Windows.Forms.CheckBox
        Me.txtHeaderStart = New System.Windows.Forms.TextBox
        Me.txtLineLen = New System.Windows.Forms.TextBox
        Me.txtLineEndChar = New System.Windows.Forms.TextBox
        Me.txtDelimiter = New System.Windows.Forms.TextBox
        Me.txtDataFile = New System.Windows.Forms.TextBox
        Me.cmdSaveDesc = New System.Windows.Forms.Button
        Me._fraTab_2 = New System.Windows.Forms.Panel
        Me.agdDataMapping = New atcControls.atcGrid
        Me._fraTab_1 = New System.Windows.Forms.Panel
        Me.txtScriptDesc = New System.Windows.Forms.TextBox
        Me.fraHeader = New System.Windows.Forms.GroupBox
        Me._optHeader_3 = New System.Windows.Forms.RadioButton
        Me._optHeader_2 = New System.Windows.Forms.RadioButton
        Me._optHeader_1 = New System.Windows.Forms.RadioButton
        Me.fraLineEnd = New System.Windows.Forms.GroupBox
        Me._optLineEnd_0 = New System.Windows.Forms.RadioButton
        Me._optLineEnd_2 = New System.Windows.Forms.RadioButton
        Me._optLineEnd_1 = New System.Windows.Forms.RadioButton
        Me._optLineEnd_3 = New System.Windows.Forms.RadioButton
        Me.fraColumns = New System.Windows.Forms.GroupBox
        Me._optDelimiter_3 = New System.Windows.Forms.RadioButton
        Me._optDelimiter_2 = New System.Windows.Forms.RadioButton
        Me._optDelimiter_1 = New System.Windows.Forms.RadioButton
        Me._optDelimiter_0 = New System.Windows.Forms.RadioButton
        Me.cmdBrowseDesc = New System.Windows.Forms.Button
        Me.txtScriptFile = New System.Windows.Forms.TextBox
        Me.cmdBrowseData = New System.Windows.Forms.Button
        Me.lblScriptDesc = New System.Windows.Forms.Label
        Me.lblDataDescFile = New System.Windows.Forms.Label
        Me.lblDataFile = New System.Windows.Forms.Label
        Me.fraSash = New System.Windows.Forms.Panel
        Me.dlgOpenFileOpen = New System.Windows.Forms.OpenFileDialog
        Me.dlgOpenFileSave = New System.Windows.Forms.SaveFileDialog
        Me.fraButtons = New System.Windows.Forms.Panel
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOk = New System.Windows.Forms.Button
        Me.fraTextSample = New System.Windows.Forms.Panel
        Me.VScrollSample = New System.Windows.Forms.VScrollBar
        Me.HScrollSample = New System.Windows.Forms.HScrollBar
        Me.txtRuler2 = New System.Windows.Forms.TextBox
        Me._txtSample_0 = New System.Windows.Forms.TextBox
        Me.txtRuler1 = New System.Windows.Forms.TextBox
        Me.fraColSample = New System.Windows.Forms.Panel
        Me.agdSample = New atcControls.atcGrid
        Me.lblInputColumns = New System.Windows.Forms.Label
        Me.fraTab = New Microsoft.VisualBasic.Compatibility.VB6.PanelArray(Me.components)
        Me.optDelimiter = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optHeader = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.optLineEnd = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.txtSample = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(Me.components)
        Me._fraTab_2.SuspendLayout()
        Me._fraTab_1.SuspendLayout()
        Me.fraHeader.SuspendLayout()
        Me.fraLineEnd.SuspendLayout()
        Me.fraColumns.SuspendLayout()
        Me.fraButtons.SuspendLayout()
        Me.fraTextSample.SuspendLayout()
        Me.fraColSample.SuspendLayout()
        CType(Me.fraTab, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optDelimiter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.optLineEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSample, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtHeaderLines
        '
        Me.txtHeaderLines.AcceptsReturn = True
        Me.txtHeaderLines.BackColor = System.Drawing.SystemColors.Window
        Me.txtHeaderLines.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHeaderLines.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeaderLines.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHeaderLines.Location = New System.Drawing.Point(96, 64)
        Me.txtHeaderLines.MaxLength = 0
        Me.txtHeaderLines.Name = "txtHeaderLines"
        Me.txtHeaderLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHeaderLines.Size = New System.Drawing.Size(25, 20)
        Me.txtHeaderLines.TabIndex = 11
        Me.txtHeaderLines.Text = "1"
        Me.ToolTip1.SetToolTip(Me.txtHeaderLines, "Single printable character delimiter")
        '
        'chkSkipHeader
        '
        Me.chkSkipHeader.BackColor = System.Drawing.SystemColors.Control
        Me.chkSkipHeader.Checked = True
        Me.chkSkipHeader.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSkipHeader.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSkipHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSkipHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSkipHeader.Location = New System.Drawing.Point(8, 16)
        Me.chkSkipHeader.Name = "chkSkipHeader"
        Me.chkSkipHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSkipHeader.Size = New System.Drawing.Size(113, 17)
        Me.chkSkipHeader.TabIndex = 6
        Me.chkSkipHeader.Text = "Skip"
        Me.ToolTip1.SetToolTip(Me.chkSkipHeader, "Do not search header for any information")
        Me.chkSkipHeader.UseVisualStyleBackColor = False
        '
        'txtHeaderStart
        '
        Me.txtHeaderStart.AcceptsReturn = True
        Me.txtHeaderStart.BackColor = System.Drawing.SystemColors.Window
        Me.txtHeaderStart.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHeaderStart.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeaderStart.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHeaderStart.Location = New System.Drawing.Point(96, 48)
        Me.txtHeaderStart.MaxLength = 0
        Me.txtHeaderStart.Name = "txtHeaderStart"
        Me.txtHeaderStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHeaderStart.Size = New System.Drawing.Size(25, 19)
        Me.txtHeaderStart.TabIndex = 9
        Me.txtHeaderStart.Text = "#"
        Me.ToolTip1.SetToolTip(Me.txtHeaderStart, "Single printable character delimiter")
        '
        'txtLineLen
        '
        Me.txtLineLen.AcceptsReturn = True
        Me.txtLineLen.BackColor = System.Drawing.SystemColors.Window
        Me.txtLineLen.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLineLen.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLineLen.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLineLen.Location = New System.Drawing.Point(96, 64)
        Me.txtLineLen.MaxLength = 0
        Me.txtLineLen.Name = "txtLineLen"
        Me.txtLineLen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLineLen.Size = New System.Drawing.Size(25, 20)
        Me.txtLineLen.TabIndex = 22
        Me.txtLineLen.Text = "80"
        Me.ToolTip1.SetToolTip(Me.txtLineLen, "Single printable character delimiter")
        '
        'txtLineEndChar
        '
        Me.txtLineEndChar.AcceptsReturn = True
        Me.txtLineEndChar.BackColor = System.Drawing.SystemColors.Window
        Me.txtLineEndChar.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLineEndChar.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLineEndChar.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLineEndChar.Location = New System.Drawing.Point(96, 48)
        Me.txtLineEndChar.MaxLength = 0
        Me.txtLineEndChar.Name = "txtLineEndChar"
        Me.txtLineEndChar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLineEndChar.Size = New System.Drawing.Size(25, 19)
        Me.txtLineEndChar.TabIndex = 20
        Me.txtLineEndChar.Text = "13"
        Me.ToolTip1.SetToolTip(Me.txtLineEndChar, "Single printable character delimiter")
        '
        'txtDelimiter
        '
        Me.txtDelimiter.AcceptsReturn = True
        Me.txtDelimiter.BackColor = System.Drawing.SystemColors.Window
        Me.txtDelimiter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDelimiter.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDelimiter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDelimiter.Location = New System.Drawing.Point(96, 64)
        Me.txtDelimiter.MaxLength = 0
        Me.txtDelimiter.Name = "txtDelimiter"
        Me.txtDelimiter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDelimiter.Size = New System.Drawing.Size(17, 19)
        Me.txtDelimiter.TabIndex = 16
        Me.txtDelimiter.Text = ","
        Me.ToolTip1.SetToolTip(Me.txtDelimiter, "Single printable character delimiter")
        '
        'txtDataFile
        '
        Me.txtDataFile.AcceptsReturn = True
        Me.txtDataFile.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataFile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataFile.Location = New System.Drawing.Point(80, 0)
        Me.txtDataFile.MaxLength = 0
        Me.txtDataFile.Name = "txtDataFile"
        Me.txtDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataFile.Size = New System.Drawing.Size(337, 19)
        Me.txtDataFile.TabIndex = 1
        Me.txtDataFile.Text = "txtDataFile"
        Me.ToolTip1.SetToolTip(Me.txtDataFile, "Name of file containing data to import")
        '
        'cmdSaveDesc
        '
        Me.cmdSaveDesc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveDesc.Location = New System.Drawing.Point(104, 0)
        Me.cmdSaveDesc.Name = "cmdSaveDesc"
        Me.cmdSaveDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveDesc.Size = New System.Drawing.Size(89, 25)
        Me.cmdSaveDesc.TabIndex = 25
        Me.cmdSaveDesc.Text = "&Save Script"
        Me.ToolTip1.SetToolTip(Me.cmdSaveDesc, "Save selections and data mapping information to a data descriptor file.")
        Me.cmdSaveDesc.UseVisualStyleBackColor = False
        '
        '_fraTab_2
        '
        Me._fraTab_2.BackColor = System.Drawing.SystemColors.Control
        Me._fraTab_2.Controls.Add(Me.agdDataMapping)
        Me._fraTab_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._fraTab_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraTab_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTab.SetIndex(Me._fraTab_2, CType(2, Short))
        Me._fraTab_2.Location = New System.Drawing.Point(528, 40)
        Me._fraTab_2.Name = "_fraTab_2"
        Me._fraTab_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraTab_2.Size = New System.Drawing.Size(457, 225)
        Me._fraTab_2.TabIndex = 38
        Me._fraTab_2.Text = "Frame1"
        Me._fraTab_2.Visible = False
        '
        'agdDataMapping
        '
        Me.agdDataMapping.AllowHorizontalScrolling = True
        Me.agdDataMapping.AllowNewValidValues = False
        Me.agdDataMapping.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdDataMapping.Fixed3D = False
        Me.agdDataMapping.LineColor = System.Drawing.SystemColors.Control
        Me.agdDataMapping.LineWidth = 1.0!
        Me.agdDataMapping.Location = New System.Drawing.Point(0, 0)
        Me.agdDataMapping.Name = "agdDataMapping"
        Me.agdDataMapping.Size = New System.Drawing.Size(521, 201)
        Me.agdDataMapping.Source = Nothing
        Me.agdDataMapping.TabIndex = 23
        '
        '_fraTab_1
        '
        Me._fraTab_1.BackColor = System.Drawing.SystemColors.Control
        Me._fraTab_1.Controls.Add(Me.txtScriptDesc)
        Me._fraTab_1.Controls.Add(Me.fraHeader)
        Me._fraTab_1.Controls.Add(Me.fraLineEnd)
        Me._fraTab_1.Controls.Add(Me.fraColumns)
        Me._fraTab_1.Controls.Add(Me.cmdBrowseDesc)
        Me._fraTab_1.Controls.Add(Me.txtScriptFile)
        Me._fraTab_1.Controls.Add(Me.cmdBrowseData)
        Me._fraTab_1.Controls.Add(Me.txtDataFile)
        Me._fraTab_1.Controls.Add(Me.lblScriptDesc)
        Me._fraTab_1.Controls.Add(Me.lblDataDescFile)
        Me._fraTab_1.Controls.Add(Me.lblDataFile)
        Me._fraTab_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._fraTab_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._fraTab_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTab.SetIndex(Me._fraTab_1, CType(1, Short))
        Me._fraTab_1.Location = New System.Drawing.Point(19, 40)
        Me._fraTab_1.Name = "_fraTab_1"
        Me._fraTab_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._fraTab_1.Size = New System.Drawing.Size(490, 161)
        Me._fraTab_1.TabIndex = 39
        Me._fraTab_1.Text = "Frame1"
        '
        'txtScriptDesc
        '
        Me.txtScriptDesc.AcceptsReturn = True
        Me.txtScriptDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtScriptDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScriptDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScriptDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScriptDesc.Location = New System.Drawing.Point(80, 40)
        Me.txtScriptDesc.MaxLength = 0
        Me.txtScriptDesc.Name = "txtScriptDesc"
        Me.txtScriptDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScriptDesc.Size = New System.Drawing.Size(337, 19)
        Me.txtScriptDesc.TabIndex = 5
        Me.txtScriptDesc.Text = "txtScriptDesc"
        '
        'fraHeader
        '
        Me.fraHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraHeader.Controls.Add(Me.txtHeaderLines)
        Me.fraHeader.Controls.Add(Me.chkSkipHeader)
        Me.fraHeader.Controls.Add(Me._optHeader_3)
        Me.fraHeader.Controls.Add(Me._optHeader_2)
        Me.fraHeader.Controls.Add(Me._optHeader_1)
        Me.fraHeader.Controls.Add(Me.txtHeaderStart)
        Me.fraHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraHeader.Location = New System.Drawing.Point(0, 64)
        Me.fraHeader.Name = "fraHeader"
        Me.fraHeader.Padding = New System.Windows.Forms.Padding(0)
        Me.fraHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraHeader.Size = New System.Drawing.Size(129, 89)
        Me.fraHeader.TabIndex = 42
        Me.fraHeader.TabStop = False
        Me.fraHeader.Text = "Header"
        '
        '_optHeader_3
        '
        Me._optHeader_3.BackColor = System.Drawing.SystemColors.Control
        Me._optHeader_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optHeader_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optHeader_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeader.SetIndex(Me._optHeader_3, CType(3, Short))
        Me._optHeader_3.Location = New System.Drawing.Point(8, 64)
        Me._optHeader_3.Name = "_optHeader_3"
        Me._optHeader_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optHeader_3.Size = New System.Drawing.Size(89, 17)
        Me._optHeader_3.TabIndex = 10
        Me._optHeader_3.TabStop = True
        Me._optHeader_3.Text = "Lines"
        Me._optHeader_3.UseVisualStyleBackColor = False
        '
        '_optHeader_2
        '
        Me._optHeader_2.BackColor = System.Drawing.SystemColors.Control
        Me._optHeader_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optHeader_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optHeader_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeader.SetIndex(Me._optHeader_2, CType(2, Short))
        Me._optHeader_2.Location = New System.Drawing.Point(8, 48)
        Me._optHeader_2.Name = "_optHeader_2"
        Me._optHeader_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optHeader_2.Size = New System.Drawing.Size(89, 17)
        Me._optHeader_2.TabIndex = 8
        Me._optHeader_2.TabStop = True
        Me._optHeader_2.Text = "Starts With"
        Me._optHeader_2.UseVisualStyleBackColor = False
        '
        '_optHeader_1
        '
        Me._optHeader_1.BackColor = System.Drawing.SystemColors.Control
        Me._optHeader_1.Checked = True
        Me._optHeader_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optHeader_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optHeader_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeader.SetIndex(Me._optHeader_1, CType(1, Short))
        Me._optHeader_1.Location = New System.Drawing.Point(8, 32)
        Me._optHeader_1.Name = "_optHeader_1"
        Me._optHeader_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optHeader_1.Size = New System.Drawing.Size(113, 17)
        Me._optHeader_1.TabIndex = 7
        Me._optHeader_1.TabStop = True
        Me._optHeader_1.Text = "None"
        Me._optHeader_1.UseVisualStyleBackColor = False
        '
        'fraLineEnd
        '
        Me.fraLineEnd.BackColor = System.Drawing.SystemColors.Control
        Me.fraLineEnd.Controls.Add(Me.txtLineLen)
        Me.fraLineEnd.Controls.Add(Me.txtLineEndChar)
        Me.fraLineEnd.Controls.Add(Me._optLineEnd_0)
        Me.fraLineEnd.Controls.Add(Me._optLineEnd_2)
        Me.fraLineEnd.Controls.Add(Me._optLineEnd_1)
        Me.fraLineEnd.Controls.Add(Me._optLineEnd_3)
        Me.fraLineEnd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLineEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLineEnd.Location = New System.Drawing.Point(272, 64)
        Me.fraLineEnd.Name = "fraLineEnd"
        Me.fraLineEnd.Padding = New System.Windows.Forms.Padding(0)
        Me.fraLineEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLineEnd.Size = New System.Drawing.Size(129, 89)
        Me.fraLineEnd.TabIndex = 41
        Me.fraLineEnd.TabStop = False
        Me.fraLineEnd.Text = "Line Ending"
        '
        '_optLineEnd_0
        '
        Me._optLineEnd_0.BackColor = System.Drawing.SystemColors.Control
        Me._optLineEnd_0.Checked = True
        Me._optLineEnd_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLineEnd_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLineEnd_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEnd.SetIndex(Me._optLineEnd_0, CType(0, Short))
        Me._optLineEnd_0.Location = New System.Drawing.Point(8, 16)
        Me._optLineEnd_0.Name = "_optLineEnd_0"
        Me._optLineEnd_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLineEnd_0.Size = New System.Drawing.Size(113, 17)
        Me._optLineEnd_0.TabIndex = 17
        Me._optLineEnd_0.TabStop = True
        Me._optLineEnd_0.Text = "CR/LF or CR"
        Me._optLineEnd_0.UseVisualStyleBackColor = False
        '
        '_optLineEnd_2
        '
        Me._optLineEnd_2.BackColor = System.Drawing.SystemColors.Control
        Me._optLineEnd_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLineEnd_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLineEnd_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEnd.SetIndex(Me._optLineEnd_2, CType(2, Short))
        Me._optLineEnd_2.Location = New System.Drawing.Point(8, 48)
        Me._optLineEnd_2.Name = "_optLineEnd_2"
        Me._optLineEnd_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLineEnd_2.Size = New System.Drawing.Size(97, 17)
        Me._optLineEnd_2.TabIndex = 19
        Me._optLineEnd_2.TabStop = True
        Me._optLineEnd_2.Text = "ASCII Char:"
        Me._optLineEnd_2.UseVisualStyleBackColor = False
        '
        '_optLineEnd_1
        '
        Me._optLineEnd_1.BackColor = System.Drawing.SystemColors.Control
        Me._optLineEnd_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLineEnd_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLineEnd_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEnd.SetIndex(Me._optLineEnd_1, CType(1, Short))
        Me._optLineEnd_1.Location = New System.Drawing.Point(8, 32)
        Me._optLineEnd_1.Name = "_optLineEnd_1"
        Me._optLineEnd_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLineEnd_1.Size = New System.Drawing.Size(97, 17)
        Me._optLineEnd_1.TabIndex = 18
        Me._optLineEnd_1.TabStop = True
        Me._optLineEnd_1.Text = "LF"
        Me._optLineEnd_1.UseVisualStyleBackColor = False
        '
        '_optLineEnd_3
        '
        Me._optLineEnd_3.BackColor = System.Drawing.SystemColors.Control
        Me._optLineEnd_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optLineEnd_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optLineEnd_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEnd.SetIndex(Me._optLineEnd_3, CType(3, Short))
        Me._optLineEnd_3.Location = New System.Drawing.Point(8, 64)
        Me._optLineEnd_3.Name = "_optLineEnd_3"
        Me._optLineEnd_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optLineEnd_3.Size = New System.Drawing.Size(97, 17)
        Me._optLineEnd_3.TabIndex = 21
        Me._optLineEnd_3.TabStop = True
        Me._optLineEnd_3.Text = "Line Length:"
        Me._optLineEnd_3.UseVisualStyleBackColor = False
        '
        'fraColumns
        '
        Me.fraColumns.BackColor = System.Drawing.SystemColors.Control
        Me.fraColumns.Controls.Add(Me._optDelimiter_3)
        Me.fraColumns.Controls.Add(Me._optDelimiter_2)
        Me.fraColumns.Controls.Add(Me._optDelimiter_1)
        Me.fraColumns.Controls.Add(Me.txtDelimiter)
        Me.fraColumns.Controls.Add(Me._optDelimiter_0)
        Me.fraColumns.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraColumns.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraColumns.Location = New System.Drawing.Point(136, 64)
        Me.fraColumns.Name = "fraColumns"
        Me.fraColumns.Padding = New System.Windows.Forms.Padding(0)
        Me.fraColumns.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraColumns.Size = New System.Drawing.Size(129, 89)
        Me.fraColumns.TabIndex = 40
        Me.fraColumns.TabStop = False
        Me.fraColumns.Text = "Column Format"
        '
        '_optDelimiter_3
        '
        Me._optDelimiter_3.BackColor = System.Drawing.SystemColors.Control
        Me._optDelimiter_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDelimiter_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDelimiter_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiter.SetIndex(Me._optDelimiter_3, CType(3, Short))
        Me._optDelimiter_3.Location = New System.Drawing.Point(8, 64)
        Me._optDelimiter_3.Name = "_optDelimiter_3"
        Me._optDelimiter_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDelimiter_3.Size = New System.Drawing.Size(89, 17)
        Me._optDelimiter_3.TabIndex = 15
        Me._optDelimiter_3.TabStop = True
        Me._optDelimiter_3.Text = "Character:"
        Me._optDelimiter_3.UseVisualStyleBackColor = False
        '
        '_optDelimiter_2
        '
        Me._optDelimiter_2.BackColor = System.Drawing.SystemColors.Control
        Me._optDelimiter_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDelimiter_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDelimiter_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiter.SetIndex(Me._optDelimiter_2, CType(2, Short))
        Me._optDelimiter_2.Location = New System.Drawing.Point(8, 32)
        Me._optDelimiter_2.Name = "_optDelimiter_2"
        Me._optDelimiter_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDelimiter_2.Size = New System.Drawing.Size(113, 17)
        Me._optDelimiter_2.TabIndex = 13
        Me._optDelimiter_2.TabStop = True
        Me._optDelimiter_2.Text = "Tab Delimited"
        Me._optDelimiter_2.UseVisualStyleBackColor = False
        '
        '_optDelimiter_1
        '
        Me._optDelimiter_1.BackColor = System.Drawing.SystemColors.Control
        Me._optDelimiter_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDelimiter_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDelimiter_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiter.SetIndex(Me._optDelimiter_1, CType(1, Short))
        Me._optDelimiter_1.Location = New System.Drawing.Point(8, 48)
        Me._optDelimiter_1.Name = "_optDelimiter_1"
        Me._optDelimiter_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDelimiter_1.Size = New System.Drawing.Size(117, 17)
        Me._optDelimiter_1.TabIndex = 14
        Me._optDelimiter_1.TabStop = True
        Me._optDelimiter_1.Text = "Space Delimited"
        Me._optDelimiter_1.UseVisualStyleBackColor = False
        '
        '_optDelimiter_0
        '
        Me._optDelimiter_0.BackColor = System.Drawing.SystemColors.Control
        Me._optDelimiter_0.Checked = True
        Me._optDelimiter_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._optDelimiter_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._optDelimiter_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiter.SetIndex(Me._optDelimiter_0, CType(0, Short))
        Me._optDelimiter_0.Location = New System.Drawing.Point(8, 16)
        Me._optDelimiter_0.Name = "_optDelimiter_0"
        Me._optDelimiter_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._optDelimiter_0.Size = New System.Drawing.Size(113, 17)
        Me._optDelimiter_0.TabIndex = 12
        Me._optDelimiter_0.TabStop = True
        Me._optDelimiter_0.Text = "Fixed Width"
        Me._optDelimiter_0.UseVisualStyleBackColor = False
        '
        'cmdBrowseDesc
        '
        Me.cmdBrowseDesc.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowseDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseDesc.Location = New System.Drawing.Point(424, 24)
        Me.cmdBrowseDesc.Name = "cmdBrowseDesc"
        Me.cmdBrowseDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowseDesc.Size = New System.Drawing.Size(57, 19)
        Me.cmdBrowseDesc.TabIndex = 4
        Me.cmdBrowseDesc.Text = "Browse"
        Me.cmdBrowseDesc.UseVisualStyleBackColor = False
        '
        'txtScriptFile
        '
        Me.txtScriptFile.AcceptsReturn = True
        Me.txtScriptFile.BackColor = System.Drawing.SystemColors.Window
        Me.txtScriptFile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScriptFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScriptFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScriptFile.Location = New System.Drawing.Point(80, 24)
        Me.txtScriptFile.MaxLength = 0
        Me.txtScriptFile.Name = "txtScriptFile"
        Me.txtScriptFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScriptFile.Size = New System.Drawing.Size(337, 19)
        Me.txtScriptFile.TabIndex = 3
        Me.txtScriptFile.Text = "txtScriptFile"
        '
        'cmdBrowseData
        '
        Me.cmdBrowseData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseData.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowseData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseData.Location = New System.Drawing.Point(424, 0)
        Me.cmdBrowseData.Name = "cmdBrowseData"
        Me.cmdBrowseData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowseData.Size = New System.Drawing.Size(57, 19)
        Me.cmdBrowseData.TabIndex = 2
        Me.cmdBrowseData.Text = "Browse"
        Me.cmdBrowseData.UseVisualStyleBackColor = False
        '
        'lblScriptDesc
        '
        Me.lblScriptDesc.BackColor = System.Drawing.SystemColors.Control
        Me.lblScriptDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblScriptDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScriptDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblScriptDesc.Location = New System.Drawing.Point(0, 40)
        Me.lblScriptDesc.Name = "lblScriptDesc"
        Me.lblScriptDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScriptDesc.Size = New System.Drawing.Size(73, 17)
        Me.lblScriptDesc.TabIndex = 45
        Me.lblScriptDesc.Text = "Description:"
        Me.lblScriptDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDataDescFile
        '
        Me.lblDataDescFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblDataDescFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataDescFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataDescFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDataDescFile.Location = New System.Drawing.Point(0, 24)
        Me.lblDataDescFile.Name = "lblDataDescFile"
        Me.lblDataDescFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataDescFile.Size = New System.Drawing.Size(81, 17)
        Me.lblDataDescFile.TabIndex = 44
        Me.lblDataDescFile.Text = "Script File:"
        '
        'lblDataFile
        '
        Me.lblDataFile.BackColor = System.Drawing.SystemColors.Control
        Me.lblDataFile.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDataFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDataFile.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDataFile.Location = New System.Drawing.Point(0, 0)
        Me.lblDataFile.Name = "lblDataFile"
        Me.lblDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataFile.Size = New System.Drawing.Size(81, 20)
        Me.lblDataFile.TabIndex = 43
        Me.lblDataFile.Text = "Data File:"
        '
        'fraSash
        '
        Me.fraSash.BackColor = System.Drawing.SystemColors.Control
        Me.fraSash.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.fraSash.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSash.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSash.Location = New System.Drawing.Point(0, 216)
        Me.fraSash.Name = "fraSash"
        Me.fraSash.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSash.Size = New System.Drawing.Size(537, 9)
        Me.fraSash.TabIndex = 37
        Me.fraSash.Text = "Frame3"
        '
        'fraButtons
        '
        Me.fraButtons.BackColor = System.Drawing.SystemColors.Control
        Me.fraButtons.Controls.Add(Me.cmdSaveDesc)
        Me.fraButtons.Controls.Add(Me.cmdCancel)
        Me.fraButtons.Controls.Add(Me.cmdOk)
        Me.fraButtons.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraButtons.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraButtons.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraButtons.Location = New System.Drawing.Point(96, 440)
        Me.fraButtons.Name = "fraButtons"
        Me.fraButtons.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraButtons.Size = New System.Drawing.Size(297, 25)
        Me.fraButtons.TabIndex = 35
        Me.fraButtons.Text = "Frame3"
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(208, 0)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOk
        '
        Me.cmdOk.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOk.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOk.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOk.Location = New System.Drawing.Point(0, 0)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOk.Size = New System.Drawing.Size(89, 25)
        Me.cmdOk.TabIndex = 24
        Me.cmdOk.Text = "&Read Data"
        Me.cmdOk.UseVisualStyleBackColor = False
        '
        'fraTextSample
        '
        Me.fraTextSample.BackColor = System.Drawing.SystemColors.Control
        Me.fraTextSample.Controls.Add(Me.VScrollSample)
        Me.fraTextSample.Controls.Add(Me.HScrollSample)
        Me.fraTextSample.Controls.Add(Me.txtRuler2)
        Me.fraTextSample.Controls.Add(Me._txtSample_0)
        Me.fraTextSample.Controls.Add(Me.txtRuler1)
        Me.fraTextSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraTextSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTextSample.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTextSample.Location = New System.Drawing.Point(8, 232)
        Me.fraTextSample.Name = "fraTextSample"
        Me.fraTextSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTextSample.Size = New System.Drawing.Size(489, 145)
        Me.fraTextSample.TabIndex = 27
        '
        'VScrollSample
        '
        Me.VScrollSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScrollSample.LargeChange = 5
        Me.VScrollSample.Location = New System.Drawing.Point(472, 32)
        Me.VScrollSample.Maximum = 32771
        Me.VScrollSample.Name = "VScrollSample"
        Me.VScrollSample.Size = New System.Drawing.Size(14, 81)
        Me.VScrollSample.TabIndex = 36
        Me.VScrollSample.TabStop = True
        '
        'HScrollSample
        '
        Me.HScrollSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.HScrollSample.LargeChange = 40
        Me.HScrollSample.Location = New System.Drawing.Point(0, 132)
        Me.HScrollSample.Maximum = 1039
        Me.HScrollSample.Minimum = 1
        Me.HScrollSample.Name = "HScrollSample"
        Me.HScrollSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HScrollSample.Size = New System.Drawing.Size(489, 14)
        Me.HScrollSample.TabIndex = 34
        Me.HScrollSample.TabStop = True
        Me.HScrollSample.Value = 1
        '
        'txtRuler2
        '
        Me.txtRuler2.AcceptsReturn = True
        Me.txtRuler2.BackColor = System.Drawing.SystemColors.Control
        Me.txtRuler2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRuler2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRuler2.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRuler2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRuler2.HideSelection = False
        Me.txtRuler2.Location = New System.Drawing.Point(0, 16)
        Me.txtRuler2.MaxLength = 0
        Me.txtRuler2.Name = "txtRuler2"
        Me.txtRuler2.ReadOnly = True
        Me.txtRuler2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRuler2.Size = New System.Drawing.Size(489, 15)
        Me.txtRuler2.TabIndex = 33
        Me.txtRuler2.Text = "1234567890"
        '
        '_txtSample_0
        '
        Me._txtSample_0.AcceptsReturn = True
        Me._txtSample_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtSample_0.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtSample_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtSample_0.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtSample_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtSample_0.HideSelection = False
        Me.txtSample.SetIndex(Me._txtSample_0, CType(0, Short))
        Me._txtSample_0.Location = New System.Drawing.Point(0, 32)
        Me._txtSample_0.MaxLength = 0
        Me._txtSample_0.Name = "_txtSample_0"
        Me._txtSample_0.ReadOnly = True
        Me._txtSample_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtSample_0.Size = New System.Drawing.Size(473, 17)
        Me._txtSample_0.TabIndex = 29
        Me._txtSample_0.Text = "Sample"
        '
        'txtRuler1
        '
        Me.txtRuler1.AcceptsReturn = True
        Me.txtRuler1.BackColor = System.Drawing.SystemColors.Control
        Me.txtRuler1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRuler1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRuler1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRuler1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtRuler1.HideSelection = False
        Me.txtRuler1.Location = New System.Drawing.Point(0, 0)
        Me.txtRuler1.MaxLength = 0
        Me.txtRuler1.Name = "txtRuler1"
        Me.txtRuler1.ReadOnly = True
        Me.txtRuler1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtRuler1.Size = New System.Drawing.Size(489, 15)
        Me.txtRuler1.TabIndex = 32
        Me.txtRuler1.Text = "         1"
        '
        'fraColSample
        '
        Me.fraColSample.BackColor = System.Drawing.SystemColors.Control
        Me.fraColSample.Controls.Add(Me.agdSample)
        Me.fraColSample.Controls.Add(Me.lblInputColumns)
        Me.fraColSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraColSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraColSample.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraColSample.Location = New System.Drawing.Point(8, 232)
        Me.fraColSample.Name = "fraColSample"
        Me.fraColSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraColSample.Size = New System.Drawing.Size(489, 153)
        Me.fraColSample.TabIndex = 28
        Me.fraColSample.Text = "fraColSample"
        Me.fraColSample.Visible = False
        '
        'agdSample
        '
        Me.agdSample.AllowHorizontalScrolling = True
        Me.agdSample.AllowNewValidValues = False
        Me.agdSample.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdSample.Fixed3D = False
        Me.agdSample.LineColor = System.Drawing.SystemColors.Control
        Me.agdSample.LineWidth = 1.0!
        Me.agdSample.Location = New System.Drawing.Point(0, 16)
        Me.agdSample.Name = "agdSample"
        Me.agdSample.Size = New System.Drawing.Size(489, 129)
        Me.agdSample.Source = Nothing
        Me.agdSample.TabIndex = 30
        '
        'lblInputColumns
        '
        Me.lblInputColumns.BackColor = System.Drawing.SystemColors.Control
        Me.lblInputColumns.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInputColumns.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInputColumns.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblInputColumns.Location = New System.Drawing.Point(8, 0)
        Me.lblInputColumns.Name = "lblInputColumns"
        Me.lblInputColumns.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInputColumns.Size = New System.Drawing.Size(161, 17)
        Me.lblInputColumns.TabIndex = 31
        Me.lblInputColumns.Text = "Column Number:"
        '
        'optDelimiter
        '
        '
        'optHeader
        '
        '
        'optLineEnd
        '
        '
        'txtSample
        '
        '
        'frmInputWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(1126, 473)
        Me.Controls.Add(Me._fraTab_2)
        Me.Controls.Add(Me._fraTab_1)
        Me.Controls.Add(Me.fraSash)
        Me.Controls.Add(Me.fraButtons)
        Me.Controls.Add(Me.fraTextSample)
        Me.Controls.Add(Me.fraColSample)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Courier New", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInputWizard"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Script Creation Wizard"
        Me._fraTab_2.ResumeLayout(False)
        Me._fraTab_1.ResumeLayout(False)
        Me.fraHeader.ResumeLayout(False)
        Me.fraLineEnd.ResumeLayout(False)
        Me.fraColumns.ResumeLayout(False)
        Me.fraButtons.ResumeLayout(False)
        Me.fraTextSample.ResumeLayout(False)
        Me.fraColSample.ResumeLayout(False)
        CType(Me.fraTab, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optDelimiter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.optLineEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSample, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class