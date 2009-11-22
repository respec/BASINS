<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RunMultiWeppForm1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkEnableAlso = New System.Windows.Forms.CheckBox()
        Me.lstCopyAlso = New System.Windows.Forms.CheckedListBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkDelAll = New System.Windows.Forms.CheckBox()
        Me.txtRunCount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRunStatus = New System.Windows.Forms.Label()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnPathPlot = New System.Windows.Forms.Button()
        Me.txtPathPlot = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSlopePath = New System.Windows.Forms.Button()
        Me.txtPathSlope = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnOutPath = New System.Windows.Forms.Button()
        Me.txtPathOutput = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnWeppPath = New System.Windows.Forms.Button()
        Me.txtPathWepp = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBasePath = New System.Windows.Forms.Button()
        Me.txtPathBase = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.boxOFE = New System.Windows.Forms.GroupBox()
        Me.cboRoadType = New System.Windows.Forms.ComboBox()
        Me.radioOFEBuffer = New System.Windows.Forms.RadioButton()
        Me.radioOFEFillSlope = New System.Windows.Forms.RadioButton()
        Me.radioOFERoad = New System.Windows.Forms.RadioButton()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtSlopeStart = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtSlopeStop = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSlopeDelta = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtLengthStart = New System.Windows.Forms.TextBox()
        Me.txtLengthStop = New System.Windows.Forms.TextBox()
        Me.txtLengthDelta = New System.Windows.Forms.TextBox()
        Me.tabMode = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.lstRootSubdir = New System.Windows.Forms.CheckedListBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnScenarioPathCopyResults = New System.Windows.Forms.Button()
        Me.txtScenarioCopyResults = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnScenarioPathWeppExe = New System.Windows.Forms.Button()
        Me.txtScenarioWeppExe = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnScenarioPathRoot = New System.Windows.Forms.Button()
        Me.txtScenarioRoot = New System.Windows.Forms.TextBox()
        Me.lstScenarioCopyFiles = New System.Windows.Forms.CheckedListBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.boxOFE.SuspendLayout()
        Me.tabMode.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.chkEnableAlso)
        Me.GroupBox4.Controls.Add(Me.lstCopyAlso)
        Me.GroupBox4.Location = New System.Drawing.Point(485, 211)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(327, 258)
        Me.GroupBox4.TabIndex = 33
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Copy Additional Files from Base Folder to Wepp Exec Folder ?"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(69, 25)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(171, 13)
        Me.Label8.TabIndex = 36
        Me.Label8.Text = "(Caution, Overwrites Existing Files!)"
        '
        'chkEnableAlso
        '
        Me.chkEnableAlso.AutoSize = True
        Me.chkEnableAlso.Location = New System.Drawing.Point(19, 23)
        Me.chkEnableAlso.Name = "chkEnableAlso"
        Me.chkEnableAlso.Size = New System.Drawing.Size(44, 17)
        Me.chkEnableAlso.TabIndex = 29
        Me.chkEnableAlso.Text = "Yes"
        Me.chkEnableAlso.UseVisualStyleBackColor = True
        '
        'lstCopyAlso
        '
        Me.lstCopyAlso.FormattingEnabled = True
        Me.lstCopyAlso.Location = New System.Drawing.Point(52, 54)
        Me.lstCopyAlso.Name = "lstCopyAlso"
        Me.lstCopyAlso.Size = New System.Drawing.Size(222, 169)
        Me.lstCopyAlso.TabIndex = 28
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkDelAll)
        Me.GroupBox3.Controls.Add(Me.txtRunCount)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtRunStatus)
        Me.GroupBox3.Controls.Add(Me.btnExecute)
        Me.GroupBox3.Controls.Add(Me.ProgressBar1)
        Me.GroupBox3.Location = New System.Drawing.Point(21, 530)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(842, 99)
        Me.GroupBox3.TabIndex = 32
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Execute"
        '
        'chkDelAll
        '
        Me.chkDelAll.AutoSize = True
        Me.chkDelAll.Checked = True
        Me.chkDelAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDelAll.Location = New System.Drawing.Point(608, 60)
        Me.chkDelAll.Name = "chkDelAll"
        Me.chkDelAll.Size = New System.Drawing.Size(208, 17)
        Me.chkDelAll.TabIndex = 37
        Me.chkDelAll.Text = "Remove All other files in the Ouput Dir."
        Me.chkDelAll.UseVisualStyleBackColor = True
        '
        'txtRunCount
        '
        Me.txtRunCount.AutoSize = True
        Me.txtRunCount.Location = New System.Drawing.Point(121, 64)
        Me.txtRunCount.Name = "txtRunCount"
        Me.txtRunCount.Size = New System.Drawing.Size(13, 13)
        Me.txtRunCount.TabIndex = 36
        Me.txtRunCount.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(50, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Total Runs: "
        '
        'txtRunStatus
        '
        Me.txtRunStatus.AutoSize = True
        Me.txtRunStatus.Location = New System.Drawing.Point(215, 64)
        Me.txtRunStatus.Name = "txtRunStatus"
        Me.txtRunStatus.Size = New System.Drawing.Size(68, 13)
        Me.txtRunStatus.TabIndex = 34
        Me.txtRunStatus.Text = "<Press Run>"
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(53, 24)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(114, 28)
        Me.btnExecute.TabIndex = 25
        Me.btnExecute.Text = "Run"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(218, 29)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(598, 19)
        Me.ProgressBar1.TabIndex = 26
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.btnPathPlot)
        Me.GroupBox2.Controls.Add(Me.txtPathPlot)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.btnSlopePath)
        Me.GroupBox2.Controls.Add(Me.txtPathSlope)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnOutPath)
        Me.GroupBox2.Controls.Add(Me.txtPathOutput)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnWeppPath)
        Me.GroupBox2.Controls.Add(Me.txtPathWepp)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.btnBasePath)
        Me.GroupBox2.Controls.Add(Me.txtPathBase)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(806, 187)
        Me.GroupBox2.TabIndex = 31
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Paths"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(25, 111)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Plot File Path:"
        '
        'btnPathPlot
        '
        Me.btnPathPlot.Location = New System.Drawing.Point(165, 105)
        Me.btnPathPlot.Name = "btnPathPlot"
        Me.btnPathPlot.Size = New System.Drawing.Size(28, 25)
        Me.btnPathPlot.TabIndex = 23
        Me.btnPathPlot.Text = "..."
        Me.btnPathPlot.UseVisualStyleBackColor = True
        '
        'txtPathPlot
        '
        Me.txtPathPlot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathPlot.Location = New System.Drawing.Point(204, 107)
        Me.txtPathPlot.Name = "txtPathPlot"
        Me.txtPathPlot.Size = New System.Drawing.Size(586, 20)
        Me.txtPathPlot.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(25, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(125, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Modified Slope Out Path:"
        '
        'btnSlopePath
        '
        Me.btnSlopePath.Location = New System.Drawing.Point(165, 46)
        Me.btnSlopePath.Name = "btnSlopePath"
        Me.btnSlopePath.Size = New System.Drawing.Size(28, 25)
        Me.btnSlopePath.TabIndex = 20
        Me.btnSlopePath.Text = "..."
        Me.btnSlopePath.UseVisualStyleBackColor = True
        '
        'txtPathSlope
        '
        Me.txtPathSlope.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathSlope.Location = New System.Drawing.Point(204, 48)
        Me.txtPathSlope.Name = "txtPathSlope"
        Me.txtPathSlope.Size = New System.Drawing.Size(586, 20)
        Me.txtPathSlope.TabIndex = 21
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 142)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Output Folder:"
        '
        'btnOutPath
        '
        Me.btnOutPath.Location = New System.Drawing.Point(165, 136)
        Me.btnOutPath.Name = "btnOutPath"
        Me.btnOutPath.Size = New System.Drawing.Size(28, 25)
        Me.btnOutPath.TabIndex = 17
        Me.btnOutPath.Text = "..."
        Me.btnOutPath.UseVisualStyleBackColor = True
        '
        'txtPathOutput
        '
        Me.txtPathOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathOutput.Location = New System.Drawing.Point(204, 138)
        Me.txtPathOutput.Name = "txtPathOutput"
        Me.txtPathOutput.Size = New System.Drawing.Size(586, 20)
        Me.txtPathOutput.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(124, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Wepp Executable Folder"
        '
        'btnWeppPath
        '
        Me.btnWeppPath.Location = New System.Drawing.Point(165, 75)
        Me.btnWeppPath.Name = "btnWeppPath"
        Me.btnWeppPath.Size = New System.Drawing.Size(28, 25)
        Me.btnWeppPath.TabIndex = 14
        Me.btnWeppPath.Text = "..."
        Me.btnWeppPath.UseVisualStyleBackColor = True
        '
        'txtPathWepp
        '
        Me.txtPathWepp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathWepp.Location = New System.Drawing.Point(204, 77)
        Me.txtPathWepp.Name = "txtPathWepp"
        Me.txtPathWepp.Size = New System.Drawing.Size(586, 20)
        Me.txtPathWepp.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(108, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Base Slope File Path:"
        '
        'btnBasePath
        '
        Me.btnBasePath.Location = New System.Drawing.Point(165, 17)
        Me.btnBasePath.Name = "btnBasePath"
        Me.btnBasePath.Size = New System.Drawing.Size(28, 25)
        Me.btnBasePath.TabIndex = 11
        Me.btnBasePath.Text = "..."
        Me.btnBasePath.UseVisualStyleBackColor = True
        '
        'txtPathBase
        '
        Me.txtPathBase.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPathBase.Location = New System.Drawing.Point(204, 19)
        Me.txtPathBase.Name = "txtPathBase"
        Me.txtPathBase.Size = New System.Drawing.Size(586, 20)
        Me.txtPathBase.TabIndex = 12
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.boxOFE)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtSlopeStart)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.txtSlopeStop)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtSlopeDelta)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.txtLengthStart)
        Me.GroupBox1.Controls.Add(Me.txtLengthStop)
        Me.GroupBox1.Controls.Add(Me.txtLengthDelta)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 211)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(457, 258)
        Me.GroupBox1.TabIndex = 30
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Variables"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(361, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 13)
        Me.Label7.TabIndex = 69
        Me.Label7.Text = "Delta"
        '
        'boxOFE
        '
        Me.boxOFE.Controls.Add(Me.cboRoadType)
        Me.boxOFE.Controls.Add(Me.radioOFEBuffer)
        Me.boxOFE.Controls.Add(Me.radioOFEFillSlope)
        Me.boxOFE.Controls.Add(Me.radioOFERoad)
        Me.boxOFE.Location = New System.Drawing.Point(28, 133)
        Me.boxOFE.Name = "boxOFE"
        Me.boxOFE.Size = New System.Drawing.Size(413, 103)
        Me.boxOFE.TabIndex = 68
        Me.boxOFE.TabStop = False
        Me.boxOFE.Text = "OFE To Model"
        '
        'cboRoadType
        '
        Me.cboRoadType.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.cboRoadType.Location = New System.Drawing.Point(126, 16)
        Me.cboRoadType.Name = "cboRoadType"
        Me.cboRoadType.Size = New System.Drawing.Size(261, 21)
        Me.cboRoadType.TabIndex = 3
        '
        'radioOFEBuffer
        '
        Me.radioOFEBuffer.AutoSize = True
        Me.radioOFEBuffer.Location = New System.Drawing.Point(43, 75)
        Me.radioOFEBuffer.Name = "radioOFEBuffer"
        Me.radioOFEBuffer.Size = New System.Drawing.Size(53, 17)
        Me.radioOFEBuffer.TabIndex = 2
        Me.radioOFEBuffer.TabStop = True
        Me.radioOFEBuffer.Text = "Buffer"
        Me.radioOFEBuffer.UseVisualStyleBackColor = True
        '
        'radioOFEFillSlope
        '
        Me.radioOFEFillSlope.AutoSize = True
        Me.radioOFEFillSlope.Enabled = False
        Me.radioOFEFillSlope.Location = New System.Drawing.Point(43, 46)
        Me.radioOFEFillSlope.Name = "radioOFEFillSlope"
        Me.radioOFEFillSlope.Size = New System.Drawing.Size(64, 17)
        Me.radioOFEFillSlope.TabIndex = 1
        Me.radioOFEFillSlope.TabStop = True
        Me.radioOFEFillSlope.Text = "FillSlope"
        Me.radioOFEFillSlope.UseVisualStyleBackColor = True
        '
        'radioOFERoad
        '
        Me.radioOFERoad.AutoSize = True
        Me.radioOFERoad.Location = New System.Drawing.Point(43, 17)
        Me.radioOFERoad.Name = "radioOFERoad"
        Me.radioOFERoad.Size = New System.Drawing.Size(51, 17)
        Me.radioOFERoad.TabIndex = 0
        Me.radioOFERoad.TabStop = True
        Me.radioOFERoad.Text = "Road"
        Me.radioOFERoad.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(25, 36)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 13)
        Me.Label9.TabIndex = 60
        Me.Label9.Text = "Slope Values (%)"
        '
        'txtSlopeStart
        '
        Me.txtSlopeStart.Location = New System.Drawing.Point(154, 36)
        Me.txtSlopeStart.Name = "txtSlopeStart"
        Me.txtSlopeStart.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeStart.TabIndex = 54
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(270, 15)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 13)
        Me.Label11.TabIndex = 63
        Me.Label11.Text = "Stop"
        '
        'txtSlopeStop
        '
        Me.txtSlopeStop.Location = New System.Drawing.Point(245, 36)
        Me.txtSlopeStop.Name = "txtSlopeStop"
        Me.txtSlopeStop.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeStop.TabIndex = 55
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(179, 15)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(29, 13)
        Me.Label12.TabIndex = 62
        Me.Label12.Text = "Start"
        '
        'txtSlopeDelta
        '
        Me.txtSlopeDelta.Location = New System.Drawing.Point(336, 36)
        Me.txtSlopeDelta.Name = "txtSlopeDelta"
        Me.txtSlopeDelta.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeDelta.TabIndex = 56
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(25, 92)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(83, 13)
        Me.Label13.TabIndex = 61
        Me.Label13.Text = "Road length (ft.)"
        '
        'txtLengthStart
        '
        Me.txtLengthStart.Location = New System.Drawing.Point(154, 89)
        Me.txtLengthStart.Name = "txtLengthStart"
        Me.txtLengthStart.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthStart.TabIndex = 57
        '
        'txtLengthStop
        '
        Me.txtLengthStop.Location = New System.Drawing.Point(245, 89)
        Me.txtLengthStop.Name = "txtLengthStop"
        Me.txtLengthStop.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthStop.TabIndex = 58
        '
        'txtLengthDelta
        '
        Me.txtLengthDelta.Location = New System.Drawing.Point(336, 89)
        Me.txtLengthDelta.Name = "txtLengthDelta"
        Me.txtLengthDelta.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthDelta.TabIndex = 59
        '
        'tabMode
        '
        Me.tabMode.Controls.Add(Me.TabPage1)
        Me.tabMode.Controls.Add(Me.TabPage2)
        Me.tabMode.Location = New System.Drawing.Point(21, 12)
        Me.tabMode.Name = "tabMode"
        Me.tabMode.SelectedIndex = 0
        Me.tabMode.Size = New System.Drawing.Size(842, 512)
        Me.tabMode.TabIndex = 34
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(834, 486)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Slope vs. Length"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox5)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(834, 486)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Scenarios"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.lstScenarioCopyFiles)
        Me.GroupBox5.Controls.Add(Me.lstRootSubdir)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.btnScenarioPathCopyResults)
        Me.GroupBox5.Controls.Add(Me.txtScenarioCopyResults)
        Me.GroupBox5.Controls.Add(Me.Label16)
        Me.GroupBox5.Controls.Add(Me.btnScenarioPathWeppExe)
        Me.GroupBox5.Controls.Add(Me.txtScenarioWeppExe)
        Me.GroupBox5.Controls.Add(Me.Label17)
        Me.GroupBox5.Controls.Add(Me.btnScenarioPathRoot)
        Me.GroupBox5.Controls.Add(Me.txtScenarioRoot)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(806, 421)
        Me.GroupBox5.TabIndex = 32
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Paths"
        '
        'lstRootSubdir
        '
        Me.lstRootSubdir.FormattingEnabled = True
        Me.lstRootSubdir.Location = New System.Drawing.Point(204, 90)
        Me.lstRootSubdir.Name = "lstRootSubdir"
        Me.lstRootSubdir.Size = New System.Drawing.Size(365, 214)
        Me.lstRootSubdir.TabIndex = 26
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(25, 381)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(113, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Copy All Plot File Path:"
        '
        'btnScenarioPathCopyResults
        '
        Me.btnScenarioPathCopyResults.Location = New System.Drawing.Point(165, 375)
        Me.btnScenarioPathCopyResults.Name = "btnScenarioPathCopyResults"
        Me.btnScenarioPathCopyResults.Size = New System.Drawing.Size(28, 25)
        Me.btnScenarioPathCopyResults.TabIndex = 23
        Me.btnScenarioPathCopyResults.Text = "..."
        Me.btnScenarioPathCopyResults.UseVisualStyleBackColor = True
        '
        'txtScenarioCopyResults
        '
        Me.txtScenarioCopyResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScenarioCopyResults.Location = New System.Drawing.Point(204, 377)
        Me.txtScenarioCopyResults.Name = "txtScenarioCopyResults"
        Me.txtScenarioCopyResults.Size = New System.Drawing.Size(586, 20)
        Me.txtScenarioCopyResults.TabIndex = 24
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(25, 351)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(124, 13)
        Me.Label16.TabIndex = 16
        Me.Label16.Text = "Wepp Executable Folder"
        '
        'btnScenarioPathWeppExe
        '
        Me.btnScenarioPathWeppExe.Location = New System.Drawing.Point(165, 345)
        Me.btnScenarioPathWeppExe.Name = "btnScenarioPathWeppExe"
        Me.btnScenarioPathWeppExe.Size = New System.Drawing.Size(28, 25)
        Me.btnScenarioPathWeppExe.TabIndex = 14
        Me.btnScenarioPathWeppExe.Text = "..."
        Me.btnScenarioPathWeppExe.UseVisualStyleBackColor = True
        '
        'txtScenarioWeppExe
        '
        Me.txtScenarioWeppExe.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScenarioWeppExe.Location = New System.Drawing.Point(204, 347)
        Me.txtScenarioWeppExe.Name = "txtScenarioWeppExe"
        Me.txtScenarioWeppExe.Size = New System.Drawing.Size(586, 20)
        Me.txtScenarioWeppExe.TabIndex = 15
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(25, 23)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(75, 13)
        Me.Label17.TabIndex = 13
        Me.Label17.Text = "Root Directory"
        '
        'btnScenarioPathRoot
        '
        Me.btnScenarioPathRoot.Location = New System.Drawing.Point(165, 17)
        Me.btnScenarioPathRoot.Name = "btnScenarioPathRoot"
        Me.btnScenarioPathRoot.Size = New System.Drawing.Size(28, 25)
        Me.btnScenarioPathRoot.TabIndex = 11
        Me.btnScenarioPathRoot.Text = "..."
        Me.btnScenarioPathRoot.UseVisualStyleBackColor = True
        '
        'txtScenarioRoot
        '
        Me.txtScenarioRoot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtScenarioRoot.Location = New System.Drawing.Point(204, 19)
        Me.txtScenarioRoot.Name = "txtScenarioRoot"
        Me.txtScenarioRoot.Size = New System.Drawing.Size(586, 20)
        Me.txtScenarioRoot.TabIndex = 12
        '
        'lstScenarioCopyFiles
        '
        Me.lstScenarioCopyFiles.FormattingEnabled = True
        Me.lstScenarioCopyFiles.Location = New System.Drawing.Point(584, 90)
        Me.lstScenarioCopyFiles.Name = "lstScenarioCopyFiles"
        Me.lstScenarioCopyFiles.Size = New System.Drawing.Size(205, 214)
        Me.lstScenarioCopyFiles.TabIndex = 28
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(201, 63)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(85, 13)
        Me.Label14.TabIndex = 29
        Me.Label14.Text = "SubDirs of Root:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(581, 63)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(145, 13)
        Me.Label15.TabIndex = 30
        Me.Label15.Text = "Things in the SubDir to Copy:"
        '
        'RunMultiWeppForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(889, 654)
        Me.Controls.Add(Me.tabMode)
        Me.Controls.Add(Me.GroupBox3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(905, 690)
        Me.Name = "RunMultiWeppForm1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Run Multi Wepp Tool"
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.boxOFE.ResumeLayout(False)
        Me.boxOFE.PerformLayout()
        Me.tabMode.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkEnableAlso As System.Windows.Forms.CheckBox
    Friend WithEvents lstCopyAlso As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtRunCount As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRunStatus As System.Windows.Forms.Label
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnPathPlot As System.Windows.Forms.Button
    Friend WithEvents txtPathPlot As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSlopePath As System.Windows.Forms.Button
    Friend WithEvents txtPathSlope As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOutPath As System.Windows.Forms.Button
    Friend WithEvents txtPathOutput As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnWeppPath As System.Windows.Forms.Button
    Friend WithEvents txtPathWepp As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBasePath As System.Windows.Forms.Button
    Friend WithEvents txtPathBase As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents boxOFE As System.Windows.Forms.GroupBox
    Friend WithEvents radioOFEBuffer As System.Windows.Forms.RadioButton
    Friend WithEvents radioOFEFillSlope As System.Windows.Forms.RadioButton
    Friend WithEvents radioOFERoad As System.Windows.Forms.RadioButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeStart As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeStop As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeDelta As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtLengthStart As System.Windows.Forms.TextBox
    Friend WithEvents txtLengthStop As System.Windows.Forms.TextBox
    Friend WithEvents txtLengthDelta As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkDelAll As System.Windows.Forms.CheckBox
    Friend WithEvents tabMode As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents cboRoadType As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnScenarioPathCopyResults As System.Windows.Forms.Button
    Friend WithEvents txtScenarioCopyResults As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnScenarioPathWeppExe As System.Windows.Forms.Button
    Friend WithEvents txtScenarioWeppExe As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents btnScenarioPathRoot As System.Windows.Forms.Button
    Friend WithEvents txtScenarioRoot As System.Windows.Forms.TextBox
    Friend WithEvents lstRootSubdir As System.Windows.Forms.CheckedListBox
    Friend WithEvents lstScenarioCopyFiles As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class
