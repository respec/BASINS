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
    Public WithEvents txtScriptDesc As System.Windows.Forms.TextBox
    Public WithEvents txtHeaderLines As System.Windows.Forms.TextBox
    Public WithEvents chkSkipHeader As System.Windows.Forms.CheckBox
    Public WithEvents optHeaderLines As System.Windows.Forms.RadioButton
    Public WithEvents optHeaderStartsWith As System.Windows.Forms.RadioButton
    Public WithEvents optHeaderNone As System.Windows.Forms.RadioButton
    Public WithEvents txtHeaderStart As System.Windows.Forms.TextBox
    Public WithEvents fraHeader As System.Windows.Forms.GroupBox
    Public WithEvents txtLineLen As System.Windows.Forms.TextBox
    Public WithEvents txtLineEndChar As System.Windows.Forms.TextBox
    Public WithEvents optLineEndCRLF As System.Windows.Forms.RadioButton
    Public WithEvents optLineEndASCII As System.Windows.Forms.RadioButton
    Public WithEvents optLineEndLF As System.Windows.Forms.RadioButton
    Public WithEvents optLineEndLength As System.Windows.Forms.RadioButton
    Public WithEvents fraLineEnd As System.Windows.Forms.GroupBox
    Public WithEvents optDelimiterChar As System.Windows.Forms.RadioButton
    Public WithEvents optDelimiterTab As System.Windows.Forms.RadioButton
    Public WithEvents optDelimiterSpace As System.Windows.Forms.RadioButton
    Public WithEvents txtDelimiter As System.Windows.Forms.TextBox
    Public WithEvents optDelimiterNone As System.Windows.Forms.RadioButton
    Public WithEvents fraColumns As System.Windows.Forms.GroupBox
    Public WithEvents cmdBrowseScript As System.Windows.Forms.Button
    Public WithEvents txtScriptFile As System.Windows.Forms.TextBox
    Public WithEvents cmdBrowseData As System.Windows.Forms.Button
    Public WithEvents txtDataFile As System.Windows.Forms.TextBox
    Public WithEvents lblScriptDesc As System.Windows.Forms.Label
    Public WithEvents lblDataDescFile As System.Windows.Forms.Label
    Public WithEvents lblDataFile As System.Windows.Forms.Label
    Public WithEvents cmdSaveScript As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdReadData As System.Windows.Forms.Button
    Public WithEvents VScrollSample As System.Windows.Forms.VScrollBar
    Public WithEvents HScrollSample As System.Windows.Forms.HScrollBar
    Public WithEvents txtRuler2 As System.Windows.Forms.TextBox
    Public WithEvents _txtSample_0 As System.Windows.Forms.TextBox
    Public WithEvents txtRuler1 As System.Windows.Forms.TextBox
    Public WithEvents fraTextSample As System.Windows.Forms.Panel
    Public WithEvents agdSample As atcControls.atcGrid
    Public WithEvents lblInputColumns As System.Windows.Forms.Label
    Public WithEvents fraColSample As System.Windows.Forms.Panel
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInputWizard))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtHeaderLines = New System.Windows.Forms.TextBox
        Me.chkSkipHeader = New System.Windows.Forms.CheckBox
        Me.txtHeaderStart = New System.Windows.Forms.TextBox
        Me.txtLineLen = New System.Windows.Forms.TextBox
        Me.txtLineEndChar = New System.Windows.Forms.TextBox
        Me.txtDelimiter = New System.Windows.Forms.TextBox
        Me.txtDataFile = New System.Windows.Forms.TextBox
        Me.cmdSaveScript = New System.Windows.Forms.Button
        Me.agdDataMapping = New atcControls.atcGrid
        Me.txtScriptDesc = New System.Windows.Forms.TextBox
        Me.fraHeader = New System.Windows.Forms.GroupBox
        Me.optHeaderLines = New System.Windows.Forms.RadioButton
        Me.optHeaderStartsWith = New System.Windows.Forms.RadioButton
        Me.optHeaderNone = New System.Windows.Forms.RadioButton
        Me.fraLineEnd = New System.Windows.Forms.GroupBox
        Me.optLineEndCRLF = New System.Windows.Forms.RadioButton
        Me.optLineEndASCII = New System.Windows.Forms.RadioButton
        Me.optLineEndLF = New System.Windows.Forms.RadioButton
        Me.optLineEndLength = New System.Windows.Forms.RadioButton
        Me.fraColumns = New System.Windows.Forms.GroupBox
        Me.optDelimiterChar = New System.Windows.Forms.RadioButton
        Me.optDelimiterTab = New System.Windows.Forms.RadioButton
        Me.optDelimiterSpace = New System.Windows.Forms.RadioButton
        Me.optDelimiterNone = New System.Windows.Forms.RadioButton
        Me.cmdBrowseScript = New System.Windows.Forms.Button
        Me.txtScriptFile = New System.Windows.Forms.TextBox
        Me.cmdBrowseData = New System.Windows.Forms.Button
        Me.lblScriptDesc = New System.Windows.Forms.Label
        Me.lblDataDescFile = New System.Windows.Forms.Label
        Me.lblDataFile = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdReadData = New System.Windows.Forms.Button
        Me.fraTextSample = New System.Windows.Forms.Panel
        Me.VScrollSample = New System.Windows.Forms.VScrollBar
        Me.HScrollSample = New System.Windows.Forms.HScrollBar
        Me.txtRuler2 = New System.Windows.Forms.TextBox
        Me._txtSample_0 = New System.Windows.Forms.TextBox
        Me.txtRuler1 = New System.Windows.Forms.TextBox
        Me.fraColSample = New System.Windows.Forms.Panel
        Me.agdSample = New atcControls.atcGrid
        Me.lblInputColumns = New System.Windows.Forms.Label
        Me.tabTop = New System.Windows.Forms.TabControl
        Me.tabFileProperty = New System.Windows.Forms.TabPage
        Me.tabDataMapping = New System.Windows.Forms.TabPage
        Me.fraHeader.SuspendLayout()
        Me.fraLineEnd.SuspendLayout()
        Me.fraColumns.SuspendLayout()
        Me.fraTextSample.SuspendLayout()
        Me.fraColSample.SuspendLayout()
        Me.tabTop.SuspendLayout()
        Me.tabFileProperty.SuspendLayout()
        Me.tabDataMapping.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtHeaderLines
        '
        Me.txtHeaderLines.AcceptsReturn = True
        Me.txtHeaderLines.BackColor = System.Drawing.SystemColors.Window
        Me.txtHeaderLines.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtHeaderLines.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeaderLines.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtHeaderLines.Location = New System.Drawing.Point(103, 64)
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
        Me.chkSkipHeader.Size = New System.Drawing.Size(89, 17)
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
        Me.txtHeaderStart.Location = New System.Drawing.Point(103, 47)
        Me.txtHeaderStart.MaxLength = 0
        Me.txtHeaderStart.Name = "txtHeaderStart"
        Me.txtHeaderStart.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtHeaderStart.Size = New System.Drawing.Size(25, 20)
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
        Me.txtLineEndChar.Size = New System.Drawing.Size(25, 20)
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
        Me.txtDelimiter.Location = New System.Drawing.Point(108, 68)
        Me.txtDelimiter.MaxLength = 0
        Me.txtDelimiter.Name = "txtDelimiter"
        Me.txtDelimiter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDelimiter.Size = New System.Drawing.Size(17, 20)
        Me.txtDelimiter.TabIndex = 16
        Me.txtDelimiter.Text = ","
        Me.ToolTip1.SetToolTip(Me.txtDelimiter, "Single printable character delimiter")
        '
        'txtDataFile
        '
        Me.txtDataFile.AcceptsReturn = True
        Me.txtDataFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataFile.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataFile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataFile.Location = New System.Drawing.Point(93, 6)
        Me.txtDataFile.MaxLength = 0
        Me.txtDataFile.Name = "txtDataFile"
        Me.txtDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataFile.Size = New System.Drawing.Size(337, 20)
        Me.txtDataFile.TabIndex = 1
        Me.txtDataFile.Text = "txtDataFile"
        Me.ToolTip1.SetToolTip(Me.txtDataFile, "Name of file containing data to import")
        '
        'cmdSaveScript
        '
        Me.cmdSaveScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSaveScript.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveScript.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveScript.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveScript.Location = New System.Drawing.Point(198, 461)
        Me.cmdSaveScript.Name = "cmdSaveScript"
        Me.cmdSaveScript.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveScript.Size = New System.Drawing.Size(89, 25)
        Me.cmdSaveScript.TabIndex = 25
        Me.cmdSaveScript.Text = "&Save Script"
        Me.ToolTip1.SetToolTip(Me.cmdSaveScript, "Save selections and data mapping information to a data descriptor file.")
        Me.cmdSaveScript.UseVisualStyleBackColor = False
        '
        'agdDataMapping
        '
        Me.agdDataMapping.AllowHorizontalScrolling = True
        Me.agdDataMapping.AllowNewValidValues = False
        Me.agdDataMapping.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdDataMapping.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdDataMapping.Fixed3D = False
        Me.agdDataMapping.LineColor = System.Drawing.SystemColors.Control
        Me.agdDataMapping.LineWidth = 1.0!
        Me.agdDataMapping.Location = New System.Drawing.Point(3, 3)
        Me.agdDataMapping.Name = "agdDataMapping"
        Me.agdDataMapping.Size = New System.Drawing.Size(495, 192)
        Me.agdDataMapping.TabIndex = 23
        '
        'txtScriptDesc
        '
        Me.txtScriptDesc.AcceptsReturn = True
        Me.txtScriptDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScriptDesc.BackColor = System.Drawing.SystemColors.Window
        Me.txtScriptDesc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScriptDesc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScriptDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScriptDesc.Location = New System.Drawing.Point(93, 58)
        Me.txtScriptDesc.MaxLength = 0
        Me.txtScriptDesc.Name = "txtScriptDesc"
        Me.txtScriptDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScriptDesc.Size = New System.Drawing.Size(337, 20)
        Me.txtScriptDesc.TabIndex = 5
        Me.txtScriptDesc.Text = "txtScriptDesc"
        '
        'fraHeader
        '
        Me.fraHeader.BackColor = System.Drawing.SystemColors.Control
        Me.fraHeader.Controls.Add(Me.txtHeaderLines)
        Me.fraHeader.Controls.Add(Me.chkSkipHeader)
        Me.fraHeader.Controls.Add(Me.optHeaderLines)
        Me.fraHeader.Controls.Add(Me.optHeaderStartsWith)
        Me.fraHeader.Controls.Add(Me.optHeaderNone)
        Me.fraHeader.Controls.Add(Me.txtHeaderStart)
        Me.fraHeader.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraHeader.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraHeader.Location = New System.Drawing.Point(6, 84)
        Me.fraHeader.Name = "fraHeader"
        Me.fraHeader.Padding = New System.Windows.Forms.Padding(0)
        Me.fraHeader.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraHeader.Size = New System.Drawing.Size(139, 95)
        Me.fraHeader.TabIndex = 42
        Me.fraHeader.TabStop = False
        Me.fraHeader.Text = "Header"
        '
        'optHeaderLines
        '
        Me.optHeaderLines.BackColor = System.Drawing.SystemColors.Control
        Me.optHeaderLines.Cursor = System.Windows.Forms.Cursors.Default
        Me.optHeaderLines.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optHeaderLines.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeaderLines.Location = New System.Drawing.Point(8, 64)
        Me.optHeaderLines.Name = "optHeaderLines"
        Me.optHeaderLines.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optHeaderLines.Size = New System.Drawing.Size(89, 17)
        Me.optHeaderLines.TabIndex = 10
        Me.optHeaderLines.TabStop = True
        Me.optHeaderLines.Text = "Lines"
        Me.optHeaderLines.UseVisualStyleBackColor = False
        '
        'optHeaderStartsWith
        '
        Me.optHeaderStartsWith.BackColor = System.Drawing.SystemColors.Control
        Me.optHeaderStartsWith.Cursor = System.Windows.Forms.Cursors.Default
        Me.optHeaderStartsWith.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optHeaderStartsWith.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeaderStartsWith.Location = New System.Drawing.Point(8, 48)
        Me.optHeaderStartsWith.Name = "optHeaderStartsWith"
        Me.optHeaderStartsWith.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optHeaderStartsWith.Size = New System.Drawing.Size(89, 17)
        Me.optHeaderStartsWith.TabIndex = 8
        Me.optHeaderStartsWith.TabStop = True
        Me.optHeaderStartsWith.Text = "Starts With"
        Me.optHeaderStartsWith.UseVisualStyleBackColor = False
        '
        'optHeaderNone
        '
        Me.optHeaderNone.BackColor = System.Drawing.SystemColors.Control
        Me.optHeaderNone.Checked = True
        Me.optHeaderNone.Cursor = System.Windows.Forms.Cursors.Default
        Me.optHeaderNone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optHeaderNone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optHeaderNone.Location = New System.Drawing.Point(8, 32)
        Me.optHeaderNone.Name = "optHeaderNone"
        Me.optHeaderNone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optHeaderNone.Size = New System.Drawing.Size(89, 17)
        Me.optHeaderNone.TabIndex = 7
        Me.optHeaderNone.TabStop = True
        Me.optHeaderNone.Text = "None"
        Me.optHeaderNone.UseVisualStyleBackColor = False
        '
        'fraLineEnd
        '
        Me.fraLineEnd.BackColor = System.Drawing.SystemColors.Control
        Me.fraLineEnd.Controls.Add(Me.txtLineLen)
        Me.fraLineEnd.Controls.Add(Me.txtLineEndChar)
        Me.fraLineEnd.Controls.Add(Me.optLineEndCRLF)
        Me.fraLineEnd.Controls.Add(Me.optLineEndASCII)
        Me.fraLineEnd.Controls.Add(Me.optLineEndLF)
        Me.fraLineEnd.Controls.Add(Me.optLineEndLength)
        Me.fraLineEnd.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraLineEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraLineEnd.Location = New System.Drawing.Point(296, 84)
        Me.fraLineEnd.Name = "fraLineEnd"
        Me.fraLineEnd.Padding = New System.Windows.Forms.Padding(0)
        Me.fraLineEnd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraLineEnd.Size = New System.Drawing.Size(134, 95)
        Me.fraLineEnd.TabIndex = 41
        Me.fraLineEnd.TabStop = False
        Me.fraLineEnd.Text = "Line Ending"
        '
        'optLineEndCRLF
        '
        Me.optLineEndCRLF.BackColor = System.Drawing.SystemColors.Control
        Me.optLineEndCRLF.Checked = True
        Me.optLineEndCRLF.Cursor = System.Windows.Forms.Cursors.Default
        Me.optLineEndCRLF.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optLineEndCRLF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEndCRLF.Location = New System.Drawing.Point(8, 16)
        Me.optLineEndCRLF.Name = "optLineEndCRLF"
        Me.optLineEndCRLF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optLineEndCRLF.Size = New System.Drawing.Size(113, 17)
        Me.optLineEndCRLF.TabIndex = 17
        Me.optLineEndCRLF.TabStop = True
        Me.optLineEndCRLF.Text = "CR/LF or CR"
        Me.optLineEndCRLF.UseVisualStyleBackColor = False
        '
        'optLineEndASCII
        '
        Me.optLineEndASCII.BackColor = System.Drawing.SystemColors.Control
        Me.optLineEndASCII.Cursor = System.Windows.Forms.Cursors.Default
        Me.optLineEndASCII.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optLineEndASCII.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEndASCII.Location = New System.Drawing.Point(8, 48)
        Me.optLineEndASCII.Name = "optLineEndASCII"
        Me.optLineEndASCII.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optLineEndASCII.Size = New System.Drawing.Size(97, 17)
        Me.optLineEndASCII.TabIndex = 19
        Me.optLineEndASCII.TabStop = True
        Me.optLineEndASCII.Text = "ASCII Char:"
        Me.optLineEndASCII.UseVisualStyleBackColor = False
        '
        'optLineEndLF
        '
        Me.optLineEndLF.BackColor = System.Drawing.SystemColors.Control
        Me.optLineEndLF.Cursor = System.Windows.Forms.Cursors.Default
        Me.optLineEndLF.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optLineEndLF.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEndLF.Location = New System.Drawing.Point(8, 32)
        Me.optLineEndLF.Name = "optLineEndLF"
        Me.optLineEndLF.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optLineEndLF.Size = New System.Drawing.Size(97, 17)
        Me.optLineEndLF.TabIndex = 18
        Me.optLineEndLF.TabStop = True
        Me.optLineEndLF.Text = "LF"
        Me.optLineEndLF.UseVisualStyleBackColor = False
        '
        'optLineEndLength
        '
        Me.optLineEndLength.BackColor = System.Drawing.SystemColors.Control
        Me.optLineEndLength.Cursor = System.Windows.Forms.Cursors.Default
        Me.optLineEndLength.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optLineEndLength.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optLineEndLength.Location = New System.Drawing.Point(8, 64)
        Me.optLineEndLength.Name = "optLineEndLength"
        Me.optLineEndLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optLineEndLength.Size = New System.Drawing.Size(97, 17)
        Me.optLineEndLength.TabIndex = 21
        Me.optLineEndLength.TabStop = True
        Me.optLineEndLength.Text = "Line Length:"
        Me.optLineEndLength.UseVisualStyleBackColor = False
        '
        'fraColumns
        '
        Me.fraColumns.BackColor = System.Drawing.SystemColors.Control
        Me.fraColumns.Controls.Add(Me.optDelimiterChar)
        Me.fraColumns.Controls.Add(Me.optDelimiterTab)
        Me.fraColumns.Controls.Add(Me.optDelimiterSpace)
        Me.fraColumns.Controls.Add(Me.txtDelimiter)
        Me.fraColumns.Controls.Add(Me.optDelimiterNone)
        Me.fraColumns.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraColumns.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraColumns.Location = New System.Drawing.Point(151, 84)
        Me.fraColumns.Name = "fraColumns"
        Me.fraColumns.Padding = New System.Windows.Forms.Padding(0)
        Me.fraColumns.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraColumns.Size = New System.Drawing.Size(139, 95)
        Me.fraColumns.TabIndex = 40
        Me.fraColumns.TabStop = False
        Me.fraColumns.Text = "Column Format"
        '
        'optDelimiterChar
        '
        Me.optDelimiterChar.BackColor = System.Drawing.SystemColors.Control
        Me.optDelimiterChar.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDelimiterChar.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDelimiterChar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiterChar.Location = New System.Drawing.Point(8, 71)
        Me.optDelimiterChar.Name = "optDelimiterChar"
        Me.optDelimiterChar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDelimiterChar.Size = New System.Drawing.Size(89, 17)
        Me.optDelimiterChar.TabIndex = 15
        Me.optDelimiterChar.TabStop = True
        Me.optDelimiterChar.Text = "Character:"
        Me.optDelimiterChar.UseVisualStyleBackColor = False
        '
        'optDelimiterTab
        '
        Me.optDelimiterTab.BackColor = System.Drawing.SystemColors.Control
        Me.optDelimiterTab.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDelimiterTab.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDelimiterTab.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiterTab.Location = New System.Drawing.Point(8, 32)
        Me.optDelimiterTab.Name = "optDelimiterTab"
        Me.optDelimiterTab.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDelimiterTab.Size = New System.Drawing.Size(113, 17)
        Me.optDelimiterTab.TabIndex = 13
        Me.optDelimiterTab.TabStop = True
        Me.optDelimiterTab.Text = "Tab Delimited"
        Me.optDelimiterTab.UseVisualStyleBackColor = False
        '
        'optDelimiterSpace
        '
        Me.optDelimiterSpace.BackColor = System.Drawing.SystemColors.Control
        Me.optDelimiterSpace.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDelimiterSpace.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDelimiterSpace.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiterSpace.Location = New System.Drawing.Point(8, 48)
        Me.optDelimiterSpace.Name = "optDelimiterSpace"
        Me.optDelimiterSpace.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDelimiterSpace.Size = New System.Drawing.Size(117, 17)
        Me.optDelimiterSpace.TabIndex = 14
        Me.optDelimiterSpace.TabStop = True
        Me.optDelimiterSpace.Text = "Space Delimited"
        Me.optDelimiterSpace.UseVisualStyleBackColor = False
        '
        'optDelimiterNone
        '
        Me.optDelimiterNone.BackColor = System.Drawing.SystemColors.Control
        Me.optDelimiterNone.Checked = True
        Me.optDelimiterNone.Cursor = System.Windows.Forms.Cursors.Default
        Me.optDelimiterNone.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDelimiterNone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optDelimiterNone.Location = New System.Drawing.Point(8, 16)
        Me.optDelimiterNone.Name = "optDelimiterNone"
        Me.optDelimiterNone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optDelimiterNone.Size = New System.Drawing.Size(113, 17)
        Me.optDelimiterNone.TabIndex = 12
        Me.optDelimiterNone.TabStop = True
        Me.optDelimiterNone.Text = "Fixed Width"
        Me.optDelimiterNone.UseVisualStyleBackColor = False
        '
        'cmdBrowseScript
        '
        Me.cmdBrowseScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowseScript.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseScript.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseScript.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowseScript.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseScript.Location = New System.Drawing.Point(436, 30)
        Me.cmdBrowseScript.Name = "cmdBrowseScript"
        Me.cmdBrowseScript.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBrowseScript.Size = New System.Drawing.Size(57, 19)
        Me.cmdBrowseScript.TabIndex = 4
        Me.cmdBrowseScript.Text = "Browse"
        Me.cmdBrowseScript.UseVisualStyleBackColor = False
        '
        'txtScriptFile
        '
        Me.txtScriptFile.AcceptsReturn = True
        Me.txtScriptFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScriptFile.BackColor = System.Drawing.SystemColors.Window
        Me.txtScriptFile.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtScriptFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScriptFile.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtScriptFile.Location = New System.Drawing.Point(93, 32)
        Me.txtScriptFile.MaxLength = 0
        Me.txtScriptFile.Name = "txtScriptFile"
        Me.txtScriptFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtScriptFile.Size = New System.Drawing.Size(337, 20)
        Me.txtScriptFile.TabIndex = 3
        Me.txtScriptFile.Text = "txtScriptFile"
        '
        'cmdBrowseData
        '
        Me.cmdBrowseData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBrowseData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBrowseData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBrowseData.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBrowseData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBrowseData.Location = New System.Drawing.Point(436, 7)
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
        Me.lblScriptDesc.Location = New System.Drawing.Point(6, 61)
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
        Me.lblDataDescFile.Location = New System.Drawing.Point(6, 35)
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
        Me.lblDataFile.Location = New System.Drawing.Point(6, 9)
        Me.lblDataFile.Name = "lblDataFile"
        Me.lblDataFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDataFile.Size = New System.Drawing.Size(81, 20)
        Me.lblDataFile.TabIndex = 43
        Me.lblDataFile.Text = "Data File:"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(293, 461)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(89, 25)
        Me.cmdCancel.TabIndex = 26
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdReadData
        '
        Me.cmdReadData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdReadData.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReadData.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReadData.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReadData.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReadData.Location = New System.Drawing.Point(103, 461)
        Me.cmdReadData.Name = "cmdReadData"
        Me.cmdReadData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReadData.Size = New System.Drawing.Size(89, 25)
        Me.cmdReadData.TabIndex = 24
        Me.cmdReadData.Text = "&Read Data"
        Me.cmdReadData.UseVisualStyleBackColor = False
        '
        'fraTextSample
        '
        Me.fraTextSample.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraTextSample.BackColor = System.Drawing.SystemColors.Control
        Me.fraTextSample.Controls.Add(Me.VScrollSample)
        Me.fraTextSample.Controls.Add(Me.HScrollSample)
        Me.fraTextSample.Controls.Add(Me.txtRuler2)
        Me.fraTextSample.Controls.Add(Me._txtSample_0)
        Me.fraTextSample.Controls.Add(Me.txtRuler1)
        Me.fraTextSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraTextSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraTextSample.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraTextSample.Location = New System.Drawing.Point(12, 247)
        Me.fraTextSample.Name = "fraTextSample"
        Me.fraTextSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraTextSample.Size = New System.Drawing.Size(509, 208)
        Me.fraTextSample.TabIndex = 27
        '
        'VScrollSample
        '
        Me.VScrollSample.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.VScrollSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.VScrollSample.LargeChange = 5
        Me.VScrollSample.Location = New System.Drawing.Point(492, 32)
        Me.VScrollSample.Maximum = 32771
        Me.VScrollSample.Name = "VScrollSample"
        Me.VScrollSample.Size = New System.Drawing.Size(14, 163)
        Me.VScrollSample.TabIndex = 36
        Me.VScrollSample.TabStop = True
        '
        'HScrollSample
        '
        Me.HScrollSample.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HScrollSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.HScrollSample.LargeChange = 40
        Me.HScrollSample.Location = New System.Drawing.Point(0, 195)
        Me.HScrollSample.Maximum = 1039
        Me.HScrollSample.Minimum = 1
        Me.HScrollSample.Name = "HScrollSample"
        Me.HScrollSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HScrollSample.Size = New System.Drawing.Size(493, 14)
        Me.HScrollSample.TabIndex = 34
        Me.HScrollSample.TabStop = True
        Me.HScrollSample.Value = 1
        '
        'txtRuler2
        '
        Me.txtRuler2.AcceptsReturn = True
        Me.txtRuler2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.txtRuler2.Size = New System.Drawing.Size(509, 15)
        Me.txtRuler2.TabIndex = 33
        Me.txtRuler2.Text = "1234567890"
        '
        '_txtSample_0
        '
        Me._txtSample_0.AcceptsReturn = True
        Me._txtSample_0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._txtSample_0.BackColor = System.Drawing.SystemColors.Window
        Me._txtSample_0.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me._txtSample_0.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtSample_0.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtSample_0.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtSample_0.HideSelection = False
        Me._txtSample_0.Location = New System.Drawing.Point(0, 32)
        Me._txtSample_0.MaxLength = 0
        Me._txtSample_0.Name = "_txtSample_0"
        Me._txtSample_0.ReadOnly = True
        Me._txtSample_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtSample_0.Size = New System.Drawing.Size(493, 15)
        Me._txtSample_0.TabIndex = 29
        Me._txtSample_0.Text = "Sample"
        '
        'txtRuler1
        '
        Me.txtRuler1.AcceptsReturn = True
        Me.txtRuler1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.txtRuler1.Size = New System.Drawing.Size(509, 15)
        Me.txtRuler1.TabIndex = 32
        Me.txtRuler1.Text = "         1"
        '
        'fraColSample
        '
        Me.fraColSample.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraColSample.BackColor = System.Drawing.SystemColors.Control
        Me.fraColSample.Controls.Add(Me.agdSample)
        Me.fraColSample.Controls.Add(Me.lblInputColumns)
        Me.fraColSample.Cursor = System.Windows.Forms.Cursors.Default
        Me.fraColSample.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraColSample.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraColSample.Location = New System.Drawing.Point(12, 247)
        Me.fraColSample.Name = "fraColSample"
        Me.fraColSample.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraColSample.Size = New System.Drawing.Size(509, 208)
        Me.fraColSample.TabIndex = 28
        Me.fraColSample.Text = "fraColSample"
        Me.fraColSample.Visible = False
        '
        'agdSample
        '
        Me.agdSample.AllowHorizontalScrolling = True
        Me.agdSample.AllowNewValidValues = False
        Me.agdSample.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdSample.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdSample.Fixed3D = False
        Me.agdSample.LineColor = System.Drawing.SystemColors.Control
        Me.agdSample.LineWidth = 1.0!
        Me.agdSample.Location = New System.Drawing.Point(0, 16)
        Me.agdSample.Name = "agdSample"
        Me.agdSample.Size = New System.Drawing.Size(509, 192)
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
        'tabTop
        '
        Me.tabTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabTop.Controls.Add(Me.tabFileProperty)
        Me.tabTop.Controls.Add(Me.tabDataMapping)
        Me.tabTop.Location = New System.Drawing.Point(12, 13)
        Me.tabTop.Name = "tabTop"
        Me.tabTop.SelectedIndex = 0
        Me.tabTop.Size = New System.Drawing.Size(509, 228)
        Me.tabTop.TabIndex = 40
        '
        'tabFileProperty
        '
        Me.tabFileProperty.BackColor = System.Drawing.SystemColors.Control
        Me.tabFileProperty.Controls.Add(Me.txtScriptDesc)
        Me.tabFileProperty.Controls.Add(Me.lblDataFile)
        Me.tabFileProperty.Controls.Add(Me.fraHeader)
        Me.tabFileProperty.Controls.Add(Me.lblDataDescFile)
        Me.tabFileProperty.Controls.Add(Me.fraLineEnd)
        Me.tabFileProperty.Controls.Add(Me.lblScriptDesc)
        Me.tabFileProperty.Controls.Add(Me.fraColumns)
        Me.tabFileProperty.Controls.Add(Me.txtDataFile)
        Me.tabFileProperty.Controls.Add(Me.cmdBrowseScript)
        Me.tabFileProperty.Controls.Add(Me.cmdBrowseData)
        Me.tabFileProperty.Controls.Add(Me.txtScriptFile)
        Me.tabFileProperty.ForeColor = System.Drawing.SystemColors.ControlText
        Me.tabFileProperty.Location = New System.Drawing.Point(4, 26)
        Me.tabFileProperty.Name = "tabFileProperty"
        Me.tabFileProperty.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFileProperty.Size = New System.Drawing.Size(501, 198)
        Me.tabFileProperty.TabIndex = 0
        Me.tabFileProperty.Text = "File Properties  "
        '
        'tabDataMapping
        '
        Me.tabDataMapping.BackColor = System.Drawing.SystemColors.Control
        Me.tabDataMapping.Controls.Add(Me.agdDataMapping)
        Me.tabDataMapping.Location = New System.Drawing.Point(4, 26)
        Me.tabDataMapping.Name = "tabDataMapping"
        Me.tabDataMapping.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDataMapping.Size = New System.Drawing.Size(501, 198)
        Me.tabDataMapping.TabIndex = 1
        Me.tabDataMapping.Text = "Data Mapping  "
        '
        'frmInputWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(537, 498)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSaveScript)
        Me.Controls.Add(Me.cmdReadData)
        Me.Controls.Add(Me.tabTop)
        Me.Controls.Add(Me.fraColSample)
        Me.Controls.Add(Me.fraTextSample)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Courier New", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.Name = "frmInputWizard"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Script Creation Wizard"
        Me.fraHeader.ResumeLayout(False)
        Me.fraHeader.PerformLayout()
        Me.fraLineEnd.ResumeLayout(False)
        Me.fraLineEnd.PerformLayout()
        Me.fraColumns.ResumeLayout(False)
        Me.fraColumns.PerformLayout()
        Me.fraTextSample.ResumeLayout(False)
        Me.fraTextSample.PerformLayout()
        Me.fraColSample.ResumeLayout(False)
        Me.tabTop.ResumeLayout(False)
        Me.tabFileProperty.ResumeLayout(False)
        Me.tabFileProperty.PerformLayout()
        Me.tabDataMapping.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabTop As System.Windows.Forms.TabControl
    Friend WithEvents tabDataMapping As System.Windows.Forms.TabPage
    Friend WithEvents tabFileProperty As System.Windows.Forms.TabPage
#End Region
End Class
