<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmSediment
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents SedAlphaTc As System.Windows.Forms.TextBox
    Public WithEvents InitialSedimentImperviousLandConstant As System.Windows.Forms.TextBox
	Public WithEvents InitialSedimentPerviousLandConstant As System.Windows.Forms.TextBox
	Public WithEvents SedAccumulationRate As System.Windows.Forms.TextBox
	Public WithEvents SedRoutingCoeffBeta As System.Windows.Forms.TextBox
	Public WithEvents SedCalibCoeffAlpha As System.Windows.Forms.TextBox
    Public WithEvents SedDepletionRate As System.Windows.Forms.TextBox
	Public WithEvents SedYieldCapacity As System.Windows.Forms.TextBox
	Public WithEvents Label77 As System.Windows.Forms.Label
	Public WithEvents Label73 As System.Windows.Forms.Label
	Public WithEvents Label35 As System.Windows.Forms.Label
	Public WithEvents Label20 As System.Windows.Forms.Label
	Public WithEvents Label36 As System.Windows.Forms.Label
	Public WithEvents frameOverland As System.Windows.Forms.GroupBox
	Public WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
	Public WithEvents LakeInitialSediment As System.Windows.Forms.TextBox
	Public WithEvents SedEquilibriumConc As System.Windows.Forms.TextBox
	Public WithEvents SedDecayConstant As System.Windows.Forms.TextBox
	Public WithEvents SedPercentClay As System.Windows.Forms.TextBox
	Public WithEvents SedPercentSilt As System.Windows.Forms.TextBox
	Public WithEvents SedPercentSand As System.Windows.Forms.TextBox
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents Label40 As System.Windows.Forms.Label
	Public WithEvents Label41 As System.Windows.Forms.Label
	Public WithEvents Label37 As System.Windows.Forms.Label
	Public WithEvents Label38 As System.Windows.Forms.Label
	Public WithEvents Label39 As System.Windows.Forms.Label
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
	Public WithEvents SSTab1 As System.Windows.Forms.TabControl
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSediment))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.frameOverland = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.SedAlphaTc = New System.Windows.Forms.TextBox
        Me.InitialSedimentImperviousLandConstant = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.InitialSedimentPerviousLandConstant = New System.Windows.Forms.TextBox
        Me.SedAccumulationRate = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SedRoutingCoeffBeta = New System.Windows.Forms.TextBox
        Me.SedCalibCoeffAlpha = New System.Windows.Forms.TextBox
        Me.SedDepletionRate = New System.Windows.Forms.TextBox
        Me.SedYieldCapacity = New System.Windows.Forms.TextBox
        Me.Label77 = New System.Windows.Forms.Label
        Me.Label73 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.LakeInitialSediment = New System.Windows.Forms.TextBox
        Me.SedEquilibriumConc = New System.Windows.Forms.TextBox
        Me.SedDecayConstant = New System.Windows.Forms.TextBox
        Me.SedPercentClay = New System.Windows.Forms.TextBox
        Me.SedPercentSilt = New System.Windows.Forms.TextBox
        Me.SedPercentSand = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.frameOverland.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(550, 346)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(67, 24)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(477, 346)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(67, 24)
        Me.cmdSave.TabIndex = 1
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'SSTab1
        '
        Me.SSTab1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(42, 19)
        Me.SSTab1.Location = New System.Drawing.Point(12, 12)
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(605, 328)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.frameOverland)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(597, 301)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Overland"
        '
        'frameOverland
        '
        Me.frameOverland.BackColor = System.Drawing.SystemColors.Control
        Me.frameOverland.Controls.Add(Me.Label4)
        Me.frameOverland.Controls.Add(Me.SedAlphaTc)
        Me.frameOverland.Controls.Add(Me.InitialSedimentImperviousLandConstant)
        Me.frameOverland.Controls.Add(Me.Label3)
        Me.frameOverland.Controls.Add(Me.Label2)
        Me.frameOverland.Controls.Add(Me.InitialSedimentPerviousLandConstant)
        Me.frameOverland.Controls.Add(Me.SedAccumulationRate)
        Me.frameOverland.Controls.Add(Me.Label1)
        Me.frameOverland.Controls.Add(Me.SedRoutingCoeffBeta)
        Me.frameOverland.Controls.Add(Me.SedCalibCoeffAlpha)
        Me.frameOverland.Controls.Add(Me.SedDepletionRate)
        Me.frameOverland.Controls.Add(Me.SedYieldCapacity)
        Me.frameOverland.Controls.Add(Me.Label77)
        Me.frameOverland.Controls.Add(Me.Label73)
        Me.frameOverland.Controls.Add(Me.Label35)
        Me.frameOverland.Controls.Add(Me.Label20)
        Me.frameOverland.Controls.Add(Me.Label36)
        Me.frameOverland.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frameOverland.ForeColor = System.Drawing.SystemColors.ControlText
        Me.frameOverland.Location = New System.Drawing.Point(18, 13)
        Me.frameOverland.Name = "frameOverland"
        Me.frameOverland.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.frameOverland.Size = New System.Drawing.Size(559, 264)
        Me.frameOverland.TabIndex = 0
        Me.frameOverland.TabStop = False
        Me.frameOverland.Text = "Define Constants for Sediment"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(47, 227)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(154, 14)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Routing Coefficient for SDR, ?:"
        '
        'SedAlphaTc
        '
        Me.SedAlphaTc.AcceptsReturn = True
        Me.SedAlphaTc.BackColor = System.Drawing.SystemColors.Window
        Me.SedAlphaTc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedAlphaTc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedAlphaTc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedAlphaTc.Location = New System.Drawing.Point(476, 149)
        Me.SedAlphaTc.MaxLength = 0
        Me.SedAlphaTc.Name = "SedAlphaTc"
        Me.SedAlphaTc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedAlphaTc.Size = New System.Drawing.Size(40, 20)
        Me.SedAlphaTc.TabIndex = 10
        Me.SedAlphaTc.Text = "0.5"
        '
        'InitialSedimentImperviousLandConstant
        '
        Me.InitialSedimentImperviousLandConstant.AcceptsReturn = True
        Me.InitialSedimentImperviousLandConstant.BackColor = System.Drawing.SystemColors.Window
        Me.InitialSedimentImperviousLandConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialSedimentImperviousLandConstant.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialSedimentImperviousLandConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialSedimentImperviousLandConstant.Location = New System.Drawing.Point(476, 49)
        Me.InitialSedimentImperviousLandConstant.MaxLength = 0
        Me.InitialSedimentImperviousLandConstant.Name = "InitialSedimentImperviousLandConstant"
        Me.InitialSedimentImperviousLandConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialSedimentImperviousLandConstant.Size = New System.Drawing.Size(40, 20)
        Me.InitialSedimentImperviousLandConstant.TabIndex = 3
        Me.InitialSedimentImperviousLandConstant.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(47, 203)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 14)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Calibration Coefficient for SDR, ?:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 177)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(396, 14)
        Me.Label2.TabIndex = 34
        Me.Label2.Text = "Sediment Delivery Ratio, SDR = ? * exp(-? * Tc), where Tc = Travel Time to Outlet" & _
            ""
        '
        'InitialSedimentPerviousLandConstant
        '
        Me.InitialSedimentPerviousLandConstant.AcceptsReturn = True
        Me.InitialSedimentPerviousLandConstant.BackColor = System.Drawing.SystemColors.Window
        Me.InitialSedimentPerviousLandConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialSedimentPerviousLandConstant.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialSedimentPerviousLandConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialSedimentPerviousLandConstant.Location = New System.Drawing.Point(476, 24)
        Me.InitialSedimentPerviousLandConstant.MaxLength = 0
        Me.InitialSedimentPerviousLandConstant.Name = "InitialSedimentPerviousLandConstant"
        Me.InitialSedimentPerviousLandConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialSedimentPerviousLandConstant.Size = New System.Drawing.Size(40, 20)
        Me.InitialSedimentPerviousLandConstant.TabIndex = 1
        Me.InitialSedimentPerviousLandConstant.Text = "0"
        '
        'SedAccumulationRate
        '
        Me.SedAccumulationRate.AcceptsReturn = True
        Me.SedAccumulationRate.BackColor = System.Drawing.SystemColors.Window
        Me.SedAccumulationRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedAccumulationRate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedAccumulationRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedAccumulationRate.Location = New System.Drawing.Point(476, 74)
        Me.SedAccumulationRate.MaxLength = 0
        Me.SedAccumulationRate.Name = "SedAccumulationRate"
        Me.SedAccumulationRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedAccumulationRate.Size = New System.Drawing.Size(40, 20)
        Me.SedAccumulationRate.TabIndex = 5
        Me.SedAccumulationRate.Text = "0.05"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 152)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(235, 14)
        Me.Label1.TabIndex = 33
        Me.Label1.Text = "Fraction of Daily Rainfall Occurs During Tc, ? tc"
        '
        'SedRoutingCoeffBeta
        '
        Me.SedRoutingCoeffBeta.AcceptsReturn = True
        Me.SedRoutingCoeffBeta.BackColor = System.Drawing.SystemColors.Window
        Me.SedRoutingCoeffBeta.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedRoutingCoeffBeta.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedRoutingCoeffBeta.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedRoutingCoeffBeta.Location = New System.Drawing.Point(476, 224)
        Me.SedRoutingCoeffBeta.MaxLength = 0
        Me.SedRoutingCoeffBeta.Name = "SedRoutingCoeffBeta"
        Me.SedRoutingCoeffBeta.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedRoutingCoeffBeta.Size = New System.Drawing.Size(40, 20)
        Me.SedRoutingCoeffBeta.TabIndex = 12
        Me.SedRoutingCoeffBeta.Text = "0.01"
        '
        'SedCalibCoeffAlpha
        '
        Me.SedCalibCoeffAlpha.AcceptsReturn = True
        Me.SedCalibCoeffAlpha.BackColor = System.Drawing.SystemColors.Window
        Me.SedCalibCoeffAlpha.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedCalibCoeffAlpha.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedCalibCoeffAlpha.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedCalibCoeffAlpha.Location = New System.Drawing.Point(476, 200)
        Me.SedCalibCoeffAlpha.MaxLength = 0
        Me.SedCalibCoeffAlpha.Name = "SedCalibCoeffAlpha"
        Me.SedCalibCoeffAlpha.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedCalibCoeffAlpha.Size = New System.Drawing.Size(40, 20)
        Me.SedCalibCoeffAlpha.TabIndex = 11
        Me.SedCalibCoeffAlpha.Text = "1.0"
        '
        'SedDepletionRate
        '
        Me.SedDepletionRate.AcceptsReturn = True
        Me.SedDepletionRate.BackColor = System.Drawing.SystemColors.Window
        Me.SedDepletionRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedDepletionRate.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedDepletionRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedDepletionRate.Location = New System.Drawing.Point(476, 99)
        Me.SedDepletionRate.MaxLength = 0
        Me.SedDepletionRate.Name = "SedDepletionRate"
        Me.SedDepletionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedDepletionRate.Size = New System.Drawing.Size(40, 20)
        Me.SedDepletionRate.TabIndex = 7
        Me.SedDepletionRate.Text = "0.12"
        '
        'SedYieldCapacity
        '
        Me.SedYieldCapacity.AcceptsReturn = True
        Me.SedYieldCapacity.BackColor = System.Drawing.SystemColors.Window
        Me.SedYieldCapacity.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedYieldCapacity.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedYieldCapacity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedYieldCapacity.Location = New System.Drawing.Point(476, 124)
        Me.SedYieldCapacity.MaxLength = 0
        Me.SedYieldCapacity.Name = "SedYieldCapacity"
        Me.SedYieldCapacity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedYieldCapacity.Size = New System.Drawing.Size(40, 20)
        Me.SedYieldCapacity.TabIndex = 9
        Me.SedYieldCapacity.Text = "30"
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.BackColor = System.Drawing.SystemColors.Control
        Me.Label77.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label77.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label77.Location = New System.Drawing.Point(16, 52)
        Me.Label77.Name = "Label77"
        Me.Label77.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label77.Size = New System.Drawing.Size(220, 14)
        Me.Label77.TabIndex = 2
        Me.Label77.Text = "Initial Sediments on Impervious Land (kg/ha):"
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.BackColor = System.Drawing.SystemColors.Control
        Me.Label73.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label73.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label73.Location = New System.Drawing.Point(16, 27)
        Me.Label73.Name = "Label73"
        Me.Label73.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label73.Size = New System.Drawing.Size(213, 14)
        Me.Label73.TabIndex = 0
        Me.Label73.Text = "Initial Sediments on Pervious Land (kg/ha) :"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.BackColor = System.Drawing.SystemColors.Control
        Me.Label35.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label35.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label35.Location = New System.Drawing.Point(16, 77)
        Me.Label35.Name = "Label35"
        Me.Label35.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label35.Size = New System.Drawing.Size(311, 14)
        Me.Label35.TabIndex = 4
        Me.Label35.Text = "Sediments Accumulation Rate on Impervious Land (kg/ha/day) :"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.SystemColors.Control
        Me.Label20.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label20.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label20.Location = New System.Drawing.Point(16, 127)
        Me.Label20.Name = "Label20"
        Me.Label20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label20.Size = New System.Drawing.Size(187, 14)
        Me.Label20.TabIndex = 8
        Me.Label20.Text = "Sediment Yield Capacity (kg/ha/day) :"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.BackColor = System.Drawing.SystemColors.Control
        Me.Label36.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label36.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label36.Location = New System.Drawing.Point(16, 102)
        Me.Label36.Name = "Label36"
        Me.Label36.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label36.Size = New System.Drawing.Size(280, 14)
        Me.Label36.TabIndex = 6
        Me.Label36.Text = "Sediments Depletion Rate on Impervious Land (per day) :"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(597, 301)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Lake/Reservoir Routing"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.LakeInitialSediment)
        Me.Frame1.Controls.Add(Me.SedEquilibriumConc)
        Me.Frame1.Controls.Add(Me.SedDecayConstant)
        Me.Frame1.Controls.Add(Me.SedPercentClay)
        Me.Frame1.Controls.Add(Me.SedPercentSilt)
        Me.Frame1.Controls.Add(Me.SedPercentSand)
        Me.Frame1.Controls.Add(Me.Label7)
        Me.Frame1.Controls.Add(Me.Label40)
        Me.Frame1.Controls.Add(Me.Label41)
        Me.Frame1.Controls.Add(Me.Label37)
        Me.Frame1.Controls.Add(Me.Label38)
        Me.Frame1.Controls.Add(Me.Label39)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(44, 18)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(505, 265)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Define Constants for Sediment"
        '
        'LakeInitialSediment
        '
        Me.LakeInitialSediment.AcceptsReturn = True
        Me.LakeInitialSediment.BackColor = System.Drawing.SystemColors.Window
        Me.LakeInitialSediment.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.LakeInitialSediment.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LakeInitialSediment.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LakeInitialSediment.Location = New System.Drawing.Point(392, 49)
        Me.LakeInitialSediment.MaxLength = 0
        Me.LakeInitialSediment.Name = "LakeInitialSediment"
        Me.LakeInitialSediment.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LakeInitialSediment.Size = New System.Drawing.Size(40, 20)
        Me.LakeInitialSediment.TabIndex = 1
        Me.LakeInitialSediment.Text = "0.5"
        '
        'SedEquilibriumConc
        '
        Me.SedEquilibriumConc.AcceptsReturn = True
        Me.SedEquilibriumConc.BackColor = System.Drawing.SystemColors.Window
        Me.SedEquilibriumConc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedEquilibriumConc.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedEquilibriumConc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedEquilibriumConc.Location = New System.Drawing.Point(392, 81)
        Me.SedEquilibriumConc.MaxLength = 0
        Me.SedEquilibriumConc.Name = "SedEquilibriumConc"
        Me.SedEquilibriumConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedEquilibriumConc.Size = New System.Drawing.Size(40, 20)
        Me.SedEquilibriumConc.TabIndex = 3
        Me.SedEquilibriumConc.Text = "0.4"
        '
        'SedDecayConstant
        '
        Me.SedDecayConstant.AcceptsReturn = True
        Me.SedDecayConstant.BackColor = System.Drawing.SystemColors.Window
        Me.SedDecayConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedDecayConstant.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedDecayConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedDecayConstant.Location = New System.Drawing.Point(392, 113)
        Me.SedDecayConstant.MaxLength = 0
        Me.SedDecayConstant.Name = "SedDecayConstant"
        Me.SedDecayConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedDecayConstant.Size = New System.Drawing.Size(40, 20)
        Me.SedDecayConstant.TabIndex = 5
        Me.SedDecayConstant.Text = "0.184"
        '
        'SedPercentClay
        '
        Me.SedPercentClay.AcceptsReturn = True
        Me.SedPercentClay.BackColor = System.Drawing.SystemColors.Window
        Me.SedPercentClay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedPercentClay.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedPercentClay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedPercentClay.Location = New System.Drawing.Point(392, 145)
        Me.SedPercentClay.MaxLength = 0
        Me.SedPercentClay.Name = "SedPercentClay"
        Me.SedPercentClay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedPercentClay.Size = New System.Drawing.Size(40, 20)
        Me.SedPercentClay.TabIndex = 7
        Me.SedPercentClay.Text = "0.5"
        '
        'SedPercentSilt
        '
        Me.SedPercentSilt.AcceptsReturn = True
        Me.SedPercentSilt.BackColor = System.Drawing.SystemColors.Window
        Me.SedPercentSilt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedPercentSilt.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedPercentSilt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedPercentSilt.Location = New System.Drawing.Point(392, 177)
        Me.SedPercentSilt.MaxLength = 0
        Me.SedPercentSilt.Name = "SedPercentSilt"
        Me.SedPercentSilt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedPercentSilt.Size = New System.Drawing.Size(40, 20)
        Me.SedPercentSilt.TabIndex = 9
        Me.SedPercentSilt.Text = "0.4"
        '
        'SedPercentSand
        '
        Me.SedPercentSand.AcceptsReturn = True
        Me.SedPercentSand.BackColor = System.Drawing.SystemColors.Window
        Me.SedPercentSand.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SedPercentSand.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SedPercentSand.ForeColor = System.Drawing.SystemColors.WindowText
        Me.SedPercentSand.Location = New System.Drawing.Point(392, 209)
        Me.SedPercentSand.MaxLength = 0
        Me.SedPercentSand.Name = "SedPercentSand"
        Me.SedPercentSand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SedPercentSand.Size = New System.Drawing.Size(40, 20)
        Me.SedPercentSand.TabIndex = 11
        Me.SedPercentSand.Text = "0.1"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(48, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(283, 14)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Initial Total Suspended Solids (TSS) Concentration (mg/l) :"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.BackColor = System.Drawing.SystemColors.Control
        Me.Label40.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label40.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label40.Location = New System.Drawing.Point(48, 84)
        Me.Label40.Name = "Label40"
        Me.Label40.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label40.Size = New System.Drawing.Size(306, 14)
        Me.Label40.TabIndex = 2
        Me.Label40.Text = "Equilibrium Concentration of Suspended Solids in Water (mg/l):"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.BackColor = System.Drawing.SystemColors.Control
        Me.Label41.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label41.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label41.Location = New System.Drawing.Point(48, 116)
        Me.Label41.Name = "Label41"
        Me.Label41.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label41.Size = New System.Drawing.Size(213, 14)
        Me.Label41.TabIndex = 4
        Me.Label41.Text = "Solids Decay Constant in Water (per day) :"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.BackColor = System.Drawing.SystemColors.Control
        Me.Label37.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label37.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label37.Location = New System.Drawing.Point(48, 148)
        Me.Label37.Name = "Label37"
        Me.Label37.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label37.Size = New System.Drawing.Size(186, 14)
        Me.Label37.TabIndex = 6
        Me.Label37.Text = "Fraction of Clay in Inflow Sediments :"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.SystemColors.Control
        Me.Label38.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label38.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label38.Location = New System.Drawing.Point(48, 180)
        Me.Label38.Name = "Label38"
        Me.Label38.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label38.Size = New System.Drawing.Size(179, 14)
        Me.Label38.TabIndex = 8
        Me.Label38.Text = "Fraction of Silt in Inflow Sediments :"
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.BackColor = System.Drawing.SystemColors.Control
        Me.Label39.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label39.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label39.Location = New System.Drawing.Point(48, 212)
        Me.Label39.Name = "Label39"
        Me.Label39.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label39.Size = New System.Drawing.Size(190, 14)
        Me.Label39.TabIndex = 10
        Me.Label39.Text = "Fraction of Sand in Inflow Sediments :"
        '
        'frmSediment
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(629, 382)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.SSTab1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSediment"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sediment Input"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me.frameOverland.ResumeLayout(False)
        Me.frameOverland.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
#End Region 
End Class