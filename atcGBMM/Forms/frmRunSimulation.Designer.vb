<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmRunSimulation
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
    Public WithEvents InputFileName As System.Windows.Forms.TextBox
    Public WithEvents btnOutputPath As System.Windows.Forms.Button
    Public WithEvents btnDataPath As System.Windows.Forms.Button
    Public WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Public WithEvents txtDataPath As System.Windows.Forms.TextBox
    Public WithEvents Label13 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents Frame5 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents txtWhAEMDuration As System.Windows.Forms.TextBox
    Public WithEvents whaemLabel As System.Windows.Forms.Label
    Public WithEvents fraWhAEM As System.Windows.Forms.GroupBox
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents fraSimulationTimeFrame As System.Windows.Forms.GroupBox
    Public WithEvents startMONTH As System.Windows.Forms.ComboBox
    Public WithEvents endMONTH As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents LblDateRange As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents lblDateRange2 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents fraDryHg As System.Windows.Forms.GroupBox
    Public WithEvents lblDateRange3 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents fraWetHg As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRunSimulation))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame5 = New System.Windows.Forms.GroupBox
        Me.InputFileName = New System.Windows.Forms.TextBox
        Me.btnOutputPath = New System.Windows.Forms.Button
        Me.btnDataPath = New System.Windows.Forms.Button
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.txtDataPath = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.fraWhAEM = New System.Windows.Forms.GroupBox
        Me.txtWhAEMDuration = New System.Windows.Forms.TextBox
        Me.whaemLabel = New System.Windows.Forms.Label
        Me.fraSimulationTimeFrame = New System.Windows.Forms.GroupBox
        Me.dtEndSim = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.dtStartSim = New System.Windows.Forms.DateTimePicker
        Me.Label5 = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.startMONTH = New System.Windows.Forms.ComboBox
        Me.endMONTH = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.dtEnd = New System.Windows.Forms.DateTimePicker
        Me.dtStart = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.LblDateRange = New System.Windows.Forms.Label
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me.fraDryHg = New System.Windows.Forms.GroupBox
        Me.dtEndDry = New System.Windows.Forms.DateTimePicker
        Me.dtStartDry = New System.Windows.Forms.DateTimePicker
        Me.lblDateRange2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.fraWetHg = New System.Windows.Forms.GroupBox
        Me.dtEndWet = New System.Windows.Forms.DateTimePicker
        Me.dtStartWet = New System.Windows.Forms.DateTimePicker
        Me.lblDateRange3 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.Frame5.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.fraWhAEM.SuspendLayout()
        Me.fraSimulationTimeFrame.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.fraDryHg.SuspendLayout()
        Me.fraWetHg.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(42, 19)
        Me.SSTab1.Location = New System.Drawing.Point(12, 12)
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(467, 317)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame5)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(459, 290)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "File Path"
        '
        'Frame5
        '
        Me.Frame5.BackColor = System.Drawing.SystemColors.Control
        Me.Frame5.Controls.Add(Me.InputFileName)
        Me.Frame5.Controls.Add(Me.btnOutputPath)
        Me.Frame5.Controls.Add(Me.btnDataPath)
        Me.Frame5.Controls.Add(Me.txtOutputPath)
        Me.Frame5.Controls.Add(Me.txtDataPath)
        Me.Frame5.Controls.Add(Me.Label13)
        Me.Frame5.Controls.Add(Me.Label12)
        Me.Frame5.Controls.Add(Me.Label11)
        Me.Frame5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame5.Location = New System.Drawing.Point(39, 65)
        Me.Frame5.Name = "Frame5"
        Me.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame5.Size = New System.Drawing.Size(377, 153)
        Me.Frame5.TabIndex = 0
        Me.Frame5.TabStop = False
        Me.Frame5.Text = "Define File Path for GBMM Simulation Module"
        '
        'InputFileName
        '
        Me.InputFileName.AcceptsReturn = True
        Me.InputFileName.BackColor = System.Drawing.SystemColors.Window
        Me.InputFileName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InputFileName.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InputFileName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InputFileName.Location = New System.Drawing.Point(128, 40)
        Me.InputFileName.MaxLength = 0
        Me.InputFileName.Name = "InputFileName"
        Me.InputFileName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InputFileName.Size = New System.Drawing.Size(185, 20)
        Me.InputFileName.TabIndex = 1
        '
        'btnOutputPath
        '
        Me.btnOutputPath.AutoSize = True
        Me.btnOutputPath.BackColor = System.Drawing.SystemColors.Control
        Me.btnOutputPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnOutputPath.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOutputPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOutputPath.Image = Global.atcGBMM.My.Resources.Resources.openHS
        Me.btnOutputPath.Location = New System.Drawing.Point(320, 104)
        Me.btnOutputPath.Name = "btnOutputPath"
        Me.btnOutputPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnOutputPath.Size = New System.Drawing.Size(24, 22)
        Me.btnOutputPath.TabIndex = 7
        Me.btnOutputPath.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnOutputPath.UseVisualStyleBackColor = False
        '
        'btnDataPath
        '
        Me.btnDataPath.AutoSize = True
        Me.btnDataPath.BackColor = System.Drawing.SystemColors.Control
        Me.btnDataPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDataPath.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDataPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDataPath.Image = Global.atcGBMM.My.Resources.Resources.openHS
        Me.btnDataPath.Location = New System.Drawing.Point(320, 72)
        Me.btnDataPath.Name = "btnDataPath"
        Me.btnDataPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDataPath.Size = New System.Drawing.Size(24, 22)
        Me.btnDataPath.TabIndex = 4
        Me.btnDataPath.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnDataPath.UseVisualStyleBackColor = False
        '
        'txtOutputPath
        '
        Me.txtOutputPath.AcceptsReturn = True
        Me.txtOutputPath.BackColor = System.Drawing.SystemColors.Window
        Me.txtOutputPath.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtOutputPath.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutputPath.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtOutputPath.Location = New System.Drawing.Point(128, 104)
        Me.txtOutputPath.MaxLength = 0
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtOutputPath.Size = New System.Drawing.Size(185, 20)
        Me.txtOutputPath.TabIndex = 6
        '
        'txtDataPath
        '
        Me.txtDataPath.AcceptsReturn = True
        Me.txtDataPath.BackColor = System.Drawing.SystemColors.Window
        Me.txtDataPath.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDataPath.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataPath.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDataPath.Location = New System.Drawing.Point(128, 72)
        Me.txtDataPath.MaxLength = 0
        Me.txtDataPath.Name = "txtDataPath"
        Me.txtDataPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDataPath.Size = New System.Drawing.Size(185, 20)
        Me.txtDataPath.TabIndex = 3
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.SystemColors.Control
        Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label13.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(24, 43)
        Me.Label13.Name = "Label13"
        Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label13.Size = New System.Drawing.Size(82, 14)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Input File Name:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(24, 107)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(99, 14)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "Output Folder Path:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(24, 75)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(89, 14)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Data Folder Path:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.fraWhAEM)
        Me._SSTab1_TabPage1.Controls.Add(Me.fraSimulationTimeFrame)
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame2)
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(459, 290)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Simulation Time"
        '
        'fraWhAEM
        '
        Me.fraWhAEM.BackColor = System.Drawing.SystemColors.Control
        Me.fraWhAEM.Controls.Add(Me.txtWhAEMDuration)
        Me.fraWhAEM.Controls.Add(Me.whaemLabel)
        Me.fraWhAEM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWhAEM.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWhAEM.Location = New System.Drawing.Point(8, 199)
        Me.fraWhAEM.Name = "fraWhAEM"
        Me.fraWhAEM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWhAEM.Size = New System.Drawing.Size(441, 49)
        Me.fraWhAEM.TabIndex = 3
        Me.fraWhAEM.TabStop = False
        Me.fraWhAEM.Text = "WhAEM Link"
        '
        'txtWhAEMDuration
        '
        Me.txtWhAEMDuration.AcceptsReturn = True
        Me.txtWhAEMDuration.BackColor = System.Drawing.SystemColors.Window
        Me.txtWhAEMDuration.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtWhAEMDuration.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWhAEMDuration.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtWhAEMDuration.Location = New System.Drawing.Point(310, 19)
        Me.txtWhAEMDuration.MaxLength = 0
        Me.txtWhAEMDuration.Name = "txtWhAEMDuration"
        Me.txtWhAEMDuration.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtWhAEMDuration.Size = New System.Drawing.Size(49, 20)
        Me.txtWhAEMDuration.TabIndex = 1
        Me.txtWhAEMDuration.Text = "1"
        '
        'whaemLabel
        '
        Me.whaemLabel.AutoSize = True
        Me.whaemLabel.BackColor = System.Drawing.SystemColors.Control
        Me.whaemLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.whaemLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.whaemLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.whaemLabel.Location = New System.Drawing.Point(8, 22)
        Me.whaemLabel.Name = "whaemLabel"
        Me.whaemLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.whaemLabel.Size = New System.Drawing.Size(253, 13)
        Me.whaemLabel.TabIndex = 0
        Me.whaemLabel.Text = "Average Duration for Groundwater Recharge (days):"
        '
        'fraSimulationTimeFrame
        '
        Me.fraSimulationTimeFrame.BackColor = System.Drawing.SystemColors.Control
        Me.fraSimulationTimeFrame.Controls.Add(Me.dtEndSim)
        Me.fraSimulationTimeFrame.Controls.Add(Me.Label7)
        Me.fraSimulationTimeFrame.Controls.Add(Me.dtStartSim)
        Me.fraSimulationTimeFrame.Controls.Add(Me.Label5)
        Me.fraSimulationTimeFrame.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraSimulationTimeFrame.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraSimulationTimeFrame.Location = New System.Drawing.Point(8, 3)
        Me.fraSimulationTimeFrame.Name = "fraSimulationTimeFrame"
        Me.fraSimulationTimeFrame.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraSimulationTimeFrame.Size = New System.Drawing.Size(440, 49)
        Me.fraSimulationTimeFrame.TabIndex = 0
        Me.fraSimulationTimeFrame.TabStop = False
        Me.fraSimulationTimeFrame.Text = "Define Simulation Time Period"
        '
        'dtEndSim
        '
        Me.dtEndSim.CustomFormat = "MM/dd/yyyy"
        Me.dtEndSim.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEndSim.Location = New System.Drawing.Point(286, 20)
        Me.dtEndSim.Name = "dtEndSim"
        Me.dtEndSim.Size = New System.Drawing.Size(117, 20)
        Me.dtEndSim.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(227, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "End Date: "
        '
        'dtStartSim
        '
        Me.dtStartSim.CustomFormat = "MM/dd/yyyy"
        Me.dtStartSim.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtStartSim.Location = New System.Drawing.Point(72, 20)
        Me.dtStartSim.Name = "dtStartSim"
        Me.dtStartSim.Size = New System.Drawing.Size(117, 20)
        Me.dtStartSim.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(8, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Start Date:"
        '
        'Frame2
        '
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.startMONTH)
        Me.Frame2.Controls.Add(Me.endMONTH)
        Me.Frame2.Controls.Add(Me.Label4)
        Me.Frame2.Controls.Add(Me.Label3)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(8, 144)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(440, 49)
        Me.Frame2.TabIndex = 2
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Define Growing Season"
        '
        'startMONTH
        '
        Me.startMONTH.BackColor = System.Drawing.SystemColors.Window
        Me.startMONTH.Cursor = System.Windows.Forms.Cursors.Default
        Me.startMONTH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.startMONTH.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.startMONTH.ForeColor = System.Drawing.SystemColors.WindowText
        Me.startMONTH.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.startMONTH.Location = New System.Drawing.Point(94, 19)
        Me.startMONTH.Name = "startMONTH"
        Me.startMONTH.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.startMONTH.Size = New System.Drawing.Size(47, 22)
        Me.startMONTH.TabIndex = 1
        '
        'endMONTH
        '
        Me.endMONTH.BackColor = System.Drawing.SystemColors.Window
        Me.endMONTH.Cursor = System.Windows.Forms.Cursors.Default
        Me.endMONTH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.endMONTH.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.endMONTH.ForeColor = System.Drawing.SystemColors.WindowText
        Me.endMONTH.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.endMONTH.Location = New System.Drawing.Point(312, 19)
        Me.endMONTH.Name = "endMONTH"
        Me.endMONTH.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.endMONTH.Size = New System.Drawing.Size(47, 22)
        Me.endMONTH.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(227, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "End Month:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(11, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Start Month:"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.dtEnd)
        Me.Frame1.Controls.Add(Me.dtStart)
        Me.Frame1.Controls.Add(Me.Label2)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.LblDateRange)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 58)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(440, 80)
        Me.Frame1.TabIndex = 1
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Select Climate Data Time Period to be used for Simulation"
        '
        'dtEnd
        '
        Me.dtEnd.CustomFormat = "MM/dd/yyyy"
        Me.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEnd.Location = New System.Drawing.Point(286, 48)
        Me.dtEnd.Name = "dtEnd"
        Me.dtEnd.Size = New System.Drawing.Size(117, 20)
        Me.dtEnd.TabIndex = 4
        '
        'dtStart
        '
        Me.dtStart.CustomFormat = "MM/dd/yyyy"
        Me.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtStart.Location = New System.Drawing.Point(72, 48)
        Me.dtStart.Name = "dtStart"
        Me.dtStart.Size = New System.Drawing.Size(117, 20)
        Me.dtStart.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(227, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "End Date:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Start Date:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LblDateRange
        '
        Me.LblDateRange.AutoSize = True
        Me.LblDateRange.BackColor = System.Drawing.SystemColors.Control
        Me.LblDateRange.Cursor = System.Windows.Forms.Cursors.Default
        Me.LblDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDateRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDateRange.Location = New System.Drawing.Point(8, 23)
        Me.LblDateRange.Name = "LblDateRange"
        Me.LblDateRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LblDateRange.Size = New System.Drawing.Size(122, 13)
        Me.LblDateRange.TabIndex = 0
        Me.LblDateRange.Text = "Available Climate Data: ("
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.fraDryHg)
        Me._SSTab1_TabPage2.Controls.Add(Me.fraWetHg)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(459, 290)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Mercury Time Series"
        '
        'fraDryHg
        '
        Me.fraDryHg.BackColor = System.Drawing.SystemColors.Control
        Me.fraDryHg.Controls.Add(Me.dtEndDry)
        Me.fraDryHg.Controls.Add(Me.dtStartDry)
        Me.fraDryHg.Controls.Add(Me.lblDateRange2)
        Me.fraDryHg.Controls.Add(Me.Label6)
        Me.fraDryHg.Controls.Add(Me.Label8)
        Me.fraDryHg.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraDryHg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraDryHg.Location = New System.Drawing.Point(16, 32)
        Me.fraDryHg.Name = "fraDryHg"
        Me.fraDryHg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraDryHg.Size = New System.Drawing.Size(425, 81)
        Me.fraDryHg.TabIndex = 0
        Me.fraDryHg.TabStop = False
        Me.fraDryHg.Text = "Select Dry Hg Deposition Data Time Period to be used for Simulation"
        '
        'dtEndDry
        '
        Me.dtEndDry.CustomFormat = "MM/dd/yyyy"
        Me.dtEndDry.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEndDry.Location = New System.Drawing.Point(294, 52)
        Me.dtEndDry.Name = "dtEndDry"
        Me.dtEndDry.Size = New System.Drawing.Size(117, 20)
        Me.dtEndDry.TabIndex = 4
        '
        'dtStartDry
        '
        Me.dtStartDry.CustomFormat = "MM/dd/yyyy"
        Me.dtStartDry.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtStartDry.Location = New System.Drawing.Point(80, 52)
        Me.dtStartDry.Name = "dtStartDry"
        Me.dtStartDry.Size = New System.Drawing.Size(117, 20)
        Me.dtStartDry.TabIndex = 2
        '
        'lblDateRange2
        '
        Me.lblDateRange2.AutoSize = True
        Me.lblDateRange2.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateRange2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateRange2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateRange2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateRange2.Location = New System.Drawing.Point(16, 24)
        Me.lblDateRange2.Name = "lblDateRange2"
        Me.lblDateRange2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateRange2.Size = New System.Drawing.Size(152, 13)
        Me.lblDateRange2.TabIndex = 0
        Me.lblDateRange2.Text = "Available Hg Deposition Data: "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(232, 56)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(55, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "End Date:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(16, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Start Date:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'fraWetHg
        '
        Me.fraWetHg.BackColor = System.Drawing.SystemColors.Control
        Me.fraWetHg.Controls.Add(Me.dtEndWet)
        Me.fraWetHg.Controls.Add(Me.dtStartWet)
        Me.fraWetHg.Controls.Add(Me.lblDateRange3)
        Me.fraWetHg.Controls.Add(Me.Label9)
        Me.fraWetHg.Controls.Add(Me.Label10)
        Me.fraWetHg.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraWetHg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraWetHg.Location = New System.Drawing.Point(16, 152)
        Me.fraWetHg.Name = "fraWetHg"
        Me.fraWetHg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraWetHg.Size = New System.Drawing.Size(425, 81)
        Me.fraWetHg.TabIndex = 1
        Me.fraWetHg.TabStop = False
        Me.fraWetHg.Text = "Select Wet Hg Deposition Data Time Period to be used for Simulation"
        '
        'dtEndWet
        '
        Me.dtEndWet.CustomFormat = "MM/dd/yyyy"
        Me.dtEndWet.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEndWet.Location = New System.Drawing.Point(294, 52)
        Me.dtEndWet.Name = "dtEndWet"
        Me.dtEndWet.Size = New System.Drawing.Size(117, 20)
        Me.dtEndWet.TabIndex = 4
        '
        'dtStartWet
        '
        Me.dtStartWet.CustomFormat = "MM/dd/yyyy"
        Me.dtStartWet.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtStartWet.Location = New System.Drawing.Point(80, 52)
        Me.dtStartWet.Name = "dtStartWet"
        Me.dtStartWet.Size = New System.Drawing.Size(117, 20)
        Me.dtStartWet.TabIndex = 2
        '
        'lblDateRange3
        '
        Me.lblDateRange3.AutoSize = True
        Me.lblDateRange3.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateRange3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateRange3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateRange3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateRange3.Location = New System.Drawing.Point(16, 24)
        Me.lblDateRange3.Name = "lblDateRange3"
        Me.lblDateRange3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateRange3.Size = New System.Drawing.Size(152, 13)
        Me.lblDateRange3.TabIndex = 0
        Me.lblDateRange3.Text = "Available Hg Deposition Data: "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(232, 55)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(55, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "End Date:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(16, 55)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(58, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Start Date:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(407, 335)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(72, 26)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(330, 335)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(71, 26)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmRunSimulation
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(491, 373)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRunSimulation"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create Model Input File"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.Frame5.ResumeLayout(False)
        Me.Frame5.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.fraWhAEM.ResumeLayout(False)
        Me.fraWhAEM.PerformLayout()
        Me.fraSimulationTimeFrame.ResumeLayout(False)
        Me.fraSimulationTimeFrame.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.fraDryHg.ResumeLayout(False)
        Me.fraDryHg.PerformLayout()
        Me.fraWetHg.ResumeLayout(False)
        Me.fraWetHg.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtEndSim As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtStartSim As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtEndDry As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtStartDry As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtEndWet As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtStartWet As System.Windows.Forms.DateTimePicker
#End Region 
End Class