<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmMercuryInput
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
    Public WithEvents InitialSoilHgMultiplier As System.Windows.Forms.TextBox
    Public WithEvents InitialConstantHg As System.Windows.Forms.TextBox
    Public WithEvents optionInitialHgConstant As System.Windows.Forms.RadioButton
    Public WithEvents optionSoilHgGrid As System.Windows.Forms.RadioButton
    Public WithEvents cboInitialSoilHg As System.Windows.Forms.ComboBox
    Public WithEvents btnInitialSoilHg As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cboWetDepTS As System.Windows.Forms.ComboBox
    Public WithEvents cboDryDepTS As System.Windows.Forms.ComboBox
    Public WithEvents btnHg_Stations As System.Windows.Forms.Button
    Public WithEvents cboHg_Stations As System.Windows.Forms.ComboBox
    Public WithEvents btnHg_Wet_Deposition_Time_Series As System.Windows.Forms.Button
    Public WithEvents cboHg_Wet_Deposition_Time_Series As System.Windows.Forms.ComboBox
    Public WithEvents btnHg_Dry_Deposition_Time_Series As System.Windows.Forms.Button
    Public WithEvents cboHg_Dry_Deposition_Time_Series As System.Windows.Forms.ComboBox
    Public WithEvents chkTime As System.Windows.Forms.CheckBox
    Public WithEvents chkGrid As System.Windows.Forms.CheckBox
    Public WithEvents chkConstant As System.Windows.Forms.CheckBox
    Public WithEvents HgWetPrcpConc As System.Windows.Forms.TextBox
    Public WithEvents optionHgWetPrcpConc As System.Windows.Forms.RadioButton
    Public WithEvents HgWetMultiplier As System.Windows.Forms.TextBox
    Public WithEvents HgWetConstant As System.Windows.Forms.TextBox
    Public WithEvents optionHgWetConst As System.Windows.Forms.RadioButton
    Public WithEvents btnHg_Wet_Deposition_Flux As System.Windows.Forms.Button
    Public WithEvents cboHg_Wet_Deposition_Flux As System.Windows.Forms.ComboBox
    Public WithEvents HgDryMultiplier As System.Windows.Forms.TextBox
    Public WithEvents HgDryConstant As System.Windows.Forms.TextBox
    Public WithEvents cboHg_Dry_Deposition_Flux As System.Windows.Forms.ComboBox
    Public WithEvents btnHg_Dry_Deposition_Flux As System.Windows.Forms.Button
    Public WithEvents lblHgWet As System.Windows.Forms.Label
    Public WithEvents lblHgDry As System.Windows.Forms.Label
    Public WithEvents lblHgWetDepTS As System.Windows.Forms.Label
    Public WithEvents lblDryHgDepTS As System.Windows.Forms.Label
    Public WithEvents lblHgStation As System.Windows.Forms.Label
    Public WithEvents lblDryDeposition As System.Windows.Forms.Label
    Public WithEvents lblHgWetMul As System.Windows.Forms.Label
    Public WithEvents lblHgDryMul As System.Windows.Forms.Label
    Public WithEvents Frame2 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage0 As System.Windows.Forms.TabPage
    Public WithEvents InitialHgSaturatedSoilConstant As System.Windows.Forms.TextBox
    Public WithEvents HgLandSoilParticleDensity As System.Windows.Forms.TextBox
    Public WithEvents HgLandBedrockDensity As System.Windows.Forms.TextBox
    Public WithEvents HgLandBedrockHgConc As System.Windows.Forms.TextBox
    Public WithEvents HgLandPollutantEnrichmentFactor As System.Windows.Forms.TextBox
    Public WithEvents HgLandSoilReductionDepth As System.Windows.Forms.TextBox
    Public WithEvents HgLandSoilBaseReductionRate As System.Windows.Forms.TextBox
    Public WithEvents HgLandChemicalWeatheringRate As System.Windows.Forms.TextBox
    Public WithEvents HgLandSoilWaterPartitionCoeff As System.Windows.Forms.TextBox
    Public WithEvents HgLandSoilMixingDepth As System.Windows.Forms.TextBox
    Public WithEvents HgLandAirPlantBioConc As System.Windows.Forms.TextBox
    Public WithEvents HgLandAirHgConc As System.Windows.Forms.TextBox
    Public WithEvents Label85 As System.Windows.Forms.Label
    Public WithEvents Label109 As System.Windows.Forms.Label
    Public WithEvents Label60 As System.Windows.Forms.Label
    Public WithEvents Label59 As System.Windows.Forms.Label
    Public WithEvents Label61 As System.Windows.Forms.Label
    Public WithEvents Label58 As System.Windows.Forms.Label
    Public WithEvents Label57 As System.Windows.Forms.Label
    Public WithEvents Label56 As System.Windows.Forms.Label
    Public WithEvents Label55 As System.Windows.Forms.Label
    Public WithEvents Label54 As System.Windows.Forms.Label
    Public WithEvents Label53 As System.Windows.Forms.Label
    Public WithEvents Label52 As System.Windows.Forms.Label
    Public WithEvents MercuryLand As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents HgLandAnnualEvapo As System.Windows.Forms.TextBox
    Public WithEvents InitialLeafLitterConstant As System.Windows.Forms.TextBox
    Public WithEvents HgLandLeafAdhereFraction As System.Windows.Forms.TextBox
    Public WithEvents HgLandLeafInterFraction As System.Windows.Forms.TextBox
    Public WithEvents HgLandKDcomp1 As System.Windows.Forms.TextBox
    Public WithEvents HgLandKDcomp2 As System.Windows.Forms.TextBox
    Public WithEvents HgLandKDcomp3 As System.Windows.Forms.TextBox
    Public WithEvents Label51 As System.Windows.Forms.Label
    Public WithEvents Label50 As System.Windows.Forms.Label
    Public WithEvents Label49 As System.Windows.Forms.Label
    Public WithEvents Label113 As System.Windows.Forms.Label
    Public WithEvents Label114 As System.Windows.Forms.Label
    Public WithEvents Label115 As System.Windows.Forms.Label
    Public WithEvents Frame10 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents HgBenthicPorewaterDiffusionCoeff As System.Windows.Forms.TextBox
    Public WithEvents LakeHgWaterColumn As System.Windows.Forms.TextBox
    Public WithEvents HgWaterAlphaW As System.Windows.Forms.TextBox
    Public WithEvents HgWaterKWR As System.Windows.Forms.TextBox
    Public WithEvents HgWaterBetaW As System.Windows.Forms.TextBox
    Public WithEvents HgWaterKWM As System.Windows.Forms.TextBox
    Public WithEvents HgWaterVSB As System.Windows.Forms.TextBox
    Public WithEvents HgWaterVRS As System.Windows.Forms.TextBox
    Public WithEvents HgWaterKDsw As System.Windows.Forms.TextBox
    Public WithEvents HgWaterKbio As System.Windows.Forms.TextBox
    Public WithEvents HgWaterCbio As System.Windows.Forms.TextBox
    Public WithEvents HgWaterHgDecayInChannel As System.Windows.Forms.TextBox
    Public WithEvents HgMethylHgFraction As System.Windows.Forms.TextBox
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label121 As System.Windows.Forms.Label
    Public WithEvents MercuryWater As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents HgWaterTheta_bs As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicCbs As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicKDbs As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicSedimentDepth As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicVbur As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicKBM As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicBetaB As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicKBR As System.Windows.Forms.TextBox
    Public WithEvents HgBenthicAlphaB As System.Windows.Forms.TextBox
    Public WithEvents LakeHgBenthic As System.Windows.Forms.TextBox
    Public WithEvents Label127 As System.Windows.Forms.Label
    Public WithEvents Label126 As System.Windows.Forms.Label
    Public WithEvents Label12 As System.Windows.Forms.Label
    Public WithEvents Frame11 As System.Windows.Forms.GroupBox
    Public WithEvents _SSTab1_TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents SSTab1 As System.Windows.Forms.TabControl
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdSave As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMercuryInput))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SSTab1 = New System.Windows.Forms.TabControl
        Me._SSTab1_TabPage0 = New System.Windows.Forms.TabPage
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.InitialSoilHgMultiplier = New System.Windows.Forms.TextBox
        Me.InitialConstantHg = New System.Windows.Forms.TextBox
        Me.optionInitialHgConstant = New System.Windows.Forms.RadioButton
        Me.optionSoilHgGrid = New System.Windows.Forms.RadioButton
        Me.cboInitialSoilHg = New System.Windows.Forms.ComboBox
        Me.btnInitialSoilHg = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Frame2 = New System.Windows.Forms.GroupBox
        Me.cboWetDepTS = New System.Windows.Forms.ComboBox
        Me.cboDryDepTS = New System.Windows.Forms.ComboBox
        Me.btnHg_Stations = New System.Windows.Forms.Button
        Me.cboHg_Stations = New System.Windows.Forms.ComboBox
        Me.btnHg_Wet_Deposition_Time_Series = New System.Windows.Forms.Button
        Me.cboHg_Wet_Deposition_Time_Series = New System.Windows.Forms.ComboBox
        Me.btnHg_Dry_Deposition_Time_Series = New System.Windows.Forms.Button
        Me.cboHg_Dry_Deposition_Time_Series = New System.Windows.Forms.ComboBox
        Me.chkTime = New System.Windows.Forms.CheckBox
        Me.chkGrid = New System.Windows.Forms.CheckBox
        Me.chkConstant = New System.Windows.Forms.CheckBox
        Me.HgWetPrcpConc = New System.Windows.Forms.TextBox
        Me.optionHgWetPrcpConc = New System.Windows.Forms.RadioButton
        Me.HgWetMultiplier = New System.Windows.Forms.TextBox
        Me.HgWetConstant = New System.Windows.Forms.TextBox
        Me.optionHgWetConst = New System.Windows.Forms.RadioButton
        Me.btnHg_Wet_Deposition_Flux = New System.Windows.Forms.Button
        Me.cboHg_Wet_Deposition_Flux = New System.Windows.Forms.ComboBox
        Me.HgDryMultiplier = New System.Windows.Forms.TextBox
        Me.HgDryConstant = New System.Windows.Forms.TextBox
        Me.cboHg_Dry_Deposition_Flux = New System.Windows.Forms.ComboBox
        Me.btnHg_Dry_Deposition_Flux = New System.Windows.Forms.Button
        Me.lblHgWet = New System.Windows.Forms.Label
        Me.lblHgDry = New System.Windows.Forms.Label
        Me.lblHgWetDepTS = New System.Windows.Forms.Label
        Me.lblDryHgDepTS = New System.Windows.Forms.Label
        Me.lblHgStation = New System.Windows.Forms.Label
        Me.lblDryDeposition = New System.Windows.Forms.Label
        Me.lblHgWetMul = New System.Windows.Forms.Label
        Me.lblHgDryMul = New System.Windows.Forms.Label
        Me._SSTab1_TabPage1 = New System.Windows.Forms.TabPage
        Me.MercuryLand = New System.Windows.Forms.GroupBox
        Me.InitialHgSaturatedSoilConstant = New System.Windows.Forms.TextBox
        Me.HgLandSoilParticleDensity = New System.Windows.Forms.TextBox
        Me.HgLandBedrockDensity = New System.Windows.Forms.TextBox
        Me.HgLandBedrockHgConc = New System.Windows.Forms.TextBox
        Me.HgLandPollutantEnrichmentFactor = New System.Windows.Forms.TextBox
        Me.HgLandSoilReductionDepth = New System.Windows.Forms.TextBox
        Me.HgLandSoilBaseReductionRate = New System.Windows.Forms.TextBox
        Me.HgLandChemicalWeatheringRate = New System.Windows.Forms.TextBox
        Me.HgLandSoilWaterPartitionCoeff = New System.Windows.Forms.TextBox
        Me.HgLandSoilMixingDepth = New System.Windows.Forms.TextBox
        Me.HgLandAirPlantBioConc = New System.Windows.Forms.TextBox
        Me.HgLandAirHgConc = New System.Windows.Forms.TextBox
        Me.Label85 = New System.Windows.Forms.Label
        Me.Label109 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.Label61 = New System.Windows.Forms.Label
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.Label54 = New System.Windows.Forms.Label
        Me.Label53 = New System.Windows.Forms.Label
        Me.Label52 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage2 = New System.Windows.Forms.TabPage
        Me.Frame10 = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.HgLandAnnualEvapo = New System.Windows.Forms.TextBox
        Me.InitialLeafLitterConstant = New System.Windows.Forms.TextBox
        Me.HgLandLeafAdhereFraction = New System.Windows.Forms.TextBox
        Me.HgLandLeafInterFraction = New System.Windows.Forms.TextBox
        Me.HgLandKDcomp1 = New System.Windows.Forms.TextBox
        Me.HgLandKDcomp2 = New System.Windows.Forms.TextBox
        Me.HgLandKDcomp3 = New System.Windows.Forms.TextBox
        Me.Label51 = New System.Windows.Forms.Label
        Me.Label50 = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.Label113 = New System.Windows.Forms.Label
        Me.Label114 = New System.Windows.Forms.Label
        Me.Label115 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage3 = New System.Windows.Forms.TabPage
        Me.MercuryWater = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.HgBenthicPorewaterDiffusionCoeff = New System.Windows.Forms.TextBox
        Me.LakeHgWaterColumn = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.HgWaterAlphaW = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.HgWaterKWR = New System.Windows.Forms.TextBox
        Me.HgWaterBetaW = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.HgWaterKWM = New System.Windows.Forms.TextBox
        Me.HgWaterVSB = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.HgWaterVRS = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.HgWaterKDsw = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.HgWaterKbio = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.HgWaterCbio = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.HgWaterHgDecayInChannel = New System.Windows.Forms.TextBox
        Me.HgMethylHgFraction = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label120 = New System.Windows.Forms.Label
        Me.Label121 = New System.Windows.Forms.Label
        Me._SSTab1_TabPage4 = New System.Windows.Forms.TabPage
        Me.Frame11 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.HgWaterTheta_bs = New System.Windows.Forms.TextBox
        Me.HgBenthicCbs = New System.Windows.Forms.TextBox
        Me.HgBenthicKDbs = New System.Windows.Forms.TextBox
        Me.HgBenthicSedimentDepth = New System.Windows.Forms.TextBox
        Me.HgBenthicVbur = New System.Windows.Forms.TextBox
        Me.HgBenthicKBM = New System.Windows.Forms.TextBox
        Me.HgBenthicBetaB = New System.Windows.Forms.TextBox
        Me.HgBenthicKBR = New System.Windows.Forms.TextBox
        Me.HgBenthicAlphaB = New System.Windows.Forms.TextBox
        Me.LakeHgBenthic = New System.Windows.Forms.TextBox
        Me.Label127 = New System.Windows.Forms.Label
        Me.Label126 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.SSTab1.SuspendLayout()
        Me._SSTab1_TabPage0.SuspendLayout()
        Me.Frame1.SuspendLayout()
        Me.Frame2.SuspendLayout()
        Me._SSTab1_TabPage1.SuspendLayout()
        Me.MercuryLand.SuspendLayout()
        Me._SSTab1_TabPage2.SuspendLayout()
        Me.Frame10.SuspendLayout()
        Me._SSTab1_TabPage3.SuspendLayout()
        Me.MercuryWater.SuspendLayout()
        Me._SSTab1_TabPage4.SuspendLayout()
        Me.Frame11.SuspendLayout()
        Me.SuspendLayout()
        '
        'SSTab1
        '
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage0)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage1)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage2)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage3)
        Me.SSTab1.Controls.Add(Me._SSTab1_TabPage4)
        Me.SSTab1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SSTab1.ItemSize = New System.Drawing.Size(42, 19)
        Me.SSTab1.Location = New System.Drawing.Point(8, 8)
        Me.SSTab1.Name = "SSTab1"
        Me.SSTab1.SelectedIndex = 0
        Me.SSTab1.Size = New System.Drawing.Size(521, 419)
        Me.SSTab1.TabIndex = 0
        '
        '_SSTab1_TabPage0
        '
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame1)
        Me._SSTab1_TabPage0.Controls.Add(Me.Frame2)
        Me._SSTab1_TabPage0.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage0.Name = "_SSTab1_TabPage0"
        Me._SSTab1_TabPage0.Size = New System.Drawing.Size(513, 392)
        Me._SSTab1_TabPage0.TabIndex = 0
        Me._SSTab1_TabPage0.Text = "Mercury Data"
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.InitialSoilHgMultiplier)
        Me.Frame1.Controls.Add(Me.InitialConstantHg)
        Me.Frame1.Controls.Add(Me.optionInitialHgConstant)
        Me.Frame1.Controls.Add(Me.optionSoilHgGrid)
        Me.Frame1.Controls.Add(Me.cboInitialSoilHg)
        Me.Frame1.Controls.Add(Me.btnInitialSoilHg)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(3, 14)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(507, 73)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Initial Hg Concentration in watershed soil  (ng/g)"
        '
        'InitialSoilHgMultiplier
        '
        Me.InitialSoilHgMultiplier.AcceptsReturn = True
        Me.InitialSoilHgMultiplier.BackColor = System.Drawing.SystemColors.Window
        Me.InitialSoilHgMultiplier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialSoilHgMultiplier.Enabled = False
        Me.InitialSoilHgMultiplier.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialSoilHgMultiplier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialSoilHgMultiplier.Location = New System.Drawing.Point(461, 42)
        Me.InitialSoilHgMultiplier.MaxLength = 0
        Me.InitialSoilHgMultiplier.Name = "InitialSoilHgMultiplier"
        Me.InitialSoilHgMultiplier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialSoilHgMultiplier.Size = New System.Drawing.Size(30, 20)
        Me.InitialSoilHgMultiplier.TabIndex = 6
        Me.InitialSoilHgMultiplier.Text = "1"
        '
        'InitialConstantHg
        '
        Me.InitialConstantHg.AcceptsReturn = True
        Me.InitialConstantHg.BackColor = System.Drawing.SystemColors.Window
        Me.InitialConstantHg.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialConstantHg.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialConstantHg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialConstantHg.Location = New System.Drawing.Point(215, 16)
        Me.InitialConstantHg.MaxLength = 0
        Me.InitialConstantHg.Name = "InitialConstantHg"
        Me.InitialConstantHg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialConstantHg.Size = New System.Drawing.Size(50, 20)
        Me.InitialConstantHg.TabIndex = 1
        Me.InitialConstantHg.Text = "50"
        '
        'optionInitialHgConstant
        '
        Me.optionInitialHgConstant.AutoSize = True
        Me.optionInitialHgConstant.BackColor = System.Drawing.SystemColors.Control
        Me.optionInitialHgConstant.Checked = True
        Me.optionInitialHgConstant.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionInitialHgConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionInitialHgConstant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionInitialHgConstant.Location = New System.Drawing.Point(24, 17)
        Me.optionInitialHgConstant.Name = "optionInitialHgConstant"
        Me.optionInitialHgConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionInitialHgConstant.Size = New System.Drawing.Size(102, 18)
        Me.optionInitialHgConstant.TabIndex = 0
        Me.optionInitialHgConstant.TabStop = True
        Me.optionInitialHgConstant.Text = "Constant Value:"
        Me.optionInitialHgConstant.UseVisualStyleBackColor = False
        '
        'optionSoilHgGrid
        '
        Me.optionSoilHgGrid.AutoSize = True
        Me.optionSoilHgGrid.BackColor = System.Drawing.SystemColors.Control
        Me.optionSoilHgGrid.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionSoilHgGrid.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionSoilHgGrid.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionSoilHgGrid.Location = New System.Drawing.Point(24, 43)
        Me.optionSoilHgGrid.Name = "optionSoilHgGrid"
        Me.optionSoilHgGrid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionSoilHgGrid.Size = New System.Drawing.Size(176, 18)
        Me.optionSoilHgGrid.TabIndex = 2
        Me.optionSoilHgGrid.TabStop = True
        Me.optionSoilHgGrid.Text = "Initial soil Hg concentration grid:"
        Me.optionSoilHgGrid.UseVisualStyleBackColor = False
        '
        'cboInitialSoilHg
        '
        Me.cboInitialSoilHg.BackColor = System.Drawing.SystemColors.Window
        Me.cboInitialSoilHg.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboInitialSoilHg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInitialSoilHg.Enabled = False
        Me.cboInitialSoilHg.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInitialSoilHg.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboInitialSoilHg.Location = New System.Drawing.Point(215, 40)
        Me.cboInitialSoilHg.Name = "cboInitialSoilHg"
        Me.cboInitialSoilHg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboInitialSoilHg.Size = New System.Drawing.Size(149, 22)
        Me.cboInitialSoilHg.TabIndex = 3
        Me.cboInitialSoilHg.Tag = "Raster"
        '
        'btnInitialSoilHg
        '
        Me.btnInitialSoilHg.AutoSize = True
        Me.btnInitialSoilHg.BackColor = System.Drawing.SystemColors.Control
        Me.btnInitialSoilHg.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnInitialSoilHg.Enabled = False
        Me.btnInitialSoilHg.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInitialSoilHg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInitialSoilHg.Image = Global.atcGBMM.My.Resources.Resources.openHS
        Me.btnInitialSoilHg.Location = New System.Drawing.Point(370, 40)
        Me.btnInitialSoilHg.Name = "btnInitialSoilHg"
        Me.btnInitialSoilHg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnInitialSoilHg.Size = New System.Drawing.Size(29, 22)
        Me.btnInitialSoilHg.TabIndex = 4
        Me.btnInitialSoilHg.Tag = "Raster"
        Me.btnInitialSoilHg.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnInitialSoilHg.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Enabled = False
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(405, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(51, 14)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Multiplier:"
        '
        'Frame2
        '
        Me.Frame2.AutoSize = True
        Me.Frame2.BackColor = System.Drawing.SystemColors.Control
        Me.Frame2.Controls.Add(Me.cboWetDepTS)
        Me.Frame2.Controls.Add(Me.cboDryDepTS)
        Me.Frame2.Controls.Add(Me.btnHg_Stations)
        Me.Frame2.Controls.Add(Me.cboHg_Stations)
        Me.Frame2.Controls.Add(Me.btnHg_Wet_Deposition_Time_Series)
        Me.Frame2.Controls.Add(Me.cboHg_Wet_Deposition_Time_Series)
        Me.Frame2.Controls.Add(Me.btnHg_Dry_Deposition_Time_Series)
        Me.Frame2.Controls.Add(Me.cboHg_Dry_Deposition_Time_Series)
        Me.Frame2.Controls.Add(Me.chkTime)
        Me.Frame2.Controls.Add(Me.chkGrid)
        Me.Frame2.Controls.Add(Me.chkConstant)
        Me.Frame2.Controls.Add(Me.HgWetPrcpConc)
        Me.Frame2.Controls.Add(Me.optionHgWetPrcpConc)
        Me.Frame2.Controls.Add(Me.HgWetMultiplier)
        Me.Frame2.Controls.Add(Me.HgWetConstant)
        Me.Frame2.Controls.Add(Me.optionHgWetConst)
        Me.Frame2.Controls.Add(Me.btnHg_Wet_Deposition_Flux)
        Me.Frame2.Controls.Add(Me.cboHg_Wet_Deposition_Flux)
        Me.Frame2.Controls.Add(Me.HgDryMultiplier)
        Me.Frame2.Controls.Add(Me.HgDryConstant)
        Me.Frame2.Controls.Add(Me.cboHg_Dry_Deposition_Flux)
        Me.Frame2.Controls.Add(Me.btnHg_Dry_Deposition_Flux)
        Me.Frame2.Controls.Add(Me.lblHgWet)
        Me.Frame2.Controls.Add(Me.lblHgDry)
        Me.Frame2.Controls.Add(Me.lblHgWetDepTS)
        Me.Frame2.Controls.Add(Me.lblDryHgDepTS)
        Me.Frame2.Controls.Add(Me.lblHgStation)
        Me.Frame2.Controls.Add(Me.lblDryDeposition)
        Me.Frame2.Controls.Add(Me.lblHgWetMul)
        Me.Frame2.Controls.Add(Me.lblHgDryMul)
        Me.Frame2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame2.Location = New System.Drawing.Point(3, 94)
        Me.Frame2.Name = "Frame2"
        Me.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame2.Size = New System.Drawing.Size(507, 291)
        Me.Frame2.TabIndex = 1
        Me.Frame2.TabStop = False
        Me.Frame2.Text = "Daily Mercury Deposition Flux (ug/m²)"
        '
        'cboWetDepTS
        '
        Me.cboWetDepTS.BackColor = System.Drawing.SystemColors.Window
        Me.cboWetDepTS.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboWetDepTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWetDepTS.Enabled = False
        Me.cboWetDepTS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboWetDepTS.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboWetDepTS.Location = New System.Drawing.Point(408, 250)
        Me.cboWetDepTS.Name = "cboWetDepTS"
        Me.cboWetDepTS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboWetDepTS.Size = New System.Drawing.Size(93, 22)
        Me.cboWetDepTS.TabIndex = 29
        '
        'cboDryDepTS
        '
        Me.cboDryDepTS.BackColor = System.Drawing.SystemColors.Window
        Me.cboDryDepTS.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDryDepTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDryDepTS.Enabled = False
        Me.cboDryDepTS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDryDepTS.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDryDepTS.Location = New System.Drawing.Point(408, 226)
        Me.cboDryDepTS.Name = "cboDryDepTS"
        Me.cboDryDepTS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDryDepTS.Size = New System.Drawing.Size(93, 22)
        Me.cboDryDepTS.TabIndex = 25
        '
        'btnHg_Stations
        '
        Me.btnHg_Stations.AutoSize = True
        Me.btnHg_Stations.BackColor = System.Drawing.SystemColors.Control
        Me.btnHg_Stations.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHg_Stations.Enabled = False
        Me.btnHg_Stations.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHg_Stations.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHg_Stations.Image = CType(resources.GetObject("btnHg_Stations.Image"), System.Drawing.Image)
        Me.btnHg_Stations.Location = New System.Drawing.Point(370, 200)
        Me.btnHg_Stations.Name = "btnHg_Stations"
        Me.btnHg_Stations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHg_Stations.Size = New System.Drawing.Size(29, 22)
        Me.btnHg_Stations.TabIndex = 20
        Me.btnHg_Stations.Tag = "Feature"
        Me.btnHg_Stations.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnHg_Stations.UseVisualStyleBackColor = False
        '
        'cboHg_Stations
        '
        Me.cboHg_Stations.BackColor = System.Drawing.SystemColors.Window
        Me.cboHg_Stations.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHg_Stations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHg_Stations.Enabled = False
        Me.cboHg_Stations.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHg_Stations.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHg_Stations.Location = New System.Drawing.Point(218, 202)
        Me.cboHg_Stations.Name = "cboHg_Stations"
        Me.cboHg_Stations.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHg_Stations.Size = New System.Drawing.Size(146, 22)
        Me.cboHg_Stations.TabIndex = 21
        Me.cboHg_Stations.Tag = "Feature"
        '
        'btnHg_Wet_Deposition_Time_Series
        '
        Me.btnHg_Wet_Deposition_Time_Series.AutoSize = True
        Me.btnHg_Wet_Deposition_Time_Series.BackColor = System.Drawing.SystemColors.Control
        Me.btnHg_Wet_Deposition_Time_Series.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHg_Wet_Deposition_Time_Series.Enabled = False
        Me.btnHg_Wet_Deposition_Time_Series.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHg_Wet_Deposition_Time_Series.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHg_Wet_Deposition_Time_Series.Image = CType(resources.GetObject("btnHg_Wet_Deposition_Time_Series.Image"), System.Drawing.Image)
        Me.btnHg_Wet_Deposition_Time_Series.Location = New System.Drawing.Point(370, 248)
        Me.btnHg_Wet_Deposition_Time_Series.Name = "btnHg_Wet_Deposition_Time_Series"
        Me.btnHg_Wet_Deposition_Time_Series.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHg_Wet_Deposition_Time_Series.Size = New System.Drawing.Size(29, 22)
        Me.btnHg_Wet_Deposition_Time_Series.TabIndex = 27
        Me.btnHg_Wet_Deposition_Time_Series.Tag = "Table"
        Me.btnHg_Wet_Deposition_Time_Series.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnHg_Wet_Deposition_Time_Series.UseVisualStyleBackColor = False
        '
        'cboHg_Wet_Deposition_Time_Series
        '
        Me.cboHg_Wet_Deposition_Time_Series.BackColor = System.Drawing.SystemColors.Window
        Me.cboHg_Wet_Deposition_Time_Series.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHg_Wet_Deposition_Time_Series.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHg_Wet_Deposition_Time_Series.Enabled = False
        Me.cboHg_Wet_Deposition_Time_Series.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHg_Wet_Deposition_Time_Series.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHg_Wet_Deposition_Time_Series.Location = New System.Drawing.Point(218, 250)
        Me.cboHg_Wet_Deposition_Time_Series.Name = "cboHg_Wet_Deposition_Time_Series"
        Me.cboHg_Wet_Deposition_Time_Series.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHg_Wet_Deposition_Time_Series.Size = New System.Drawing.Size(146, 22)
        Me.cboHg_Wet_Deposition_Time_Series.TabIndex = 28
        Me.cboHg_Wet_Deposition_Time_Series.Tag = "Table"
        '
        'btnHg_Dry_Deposition_Time_Series
        '
        Me.btnHg_Dry_Deposition_Time_Series.AutoSize = True
        Me.btnHg_Dry_Deposition_Time_Series.BackColor = System.Drawing.SystemColors.Control
        Me.btnHg_Dry_Deposition_Time_Series.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHg_Dry_Deposition_Time_Series.Enabled = False
        Me.btnHg_Dry_Deposition_Time_Series.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHg_Dry_Deposition_Time_Series.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHg_Dry_Deposition_Time_Series.Image = CType(resources.GetObject("btnHg_Dry_Deposition_Time_Series.Image"), System.Drawing.Image)
        Me.btnHg_Dry_Deposition_Time_Series.Location = New System.Drawing.Point(370, 224)
        Me.btnHg_Dry_Deposition_Time_Series.Name = "btnHg_Dry_Deposition_Time_Series"
        Me.btnHg_Dry_Deposition_Time_Series.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHg_Dry_Deposition_Time_Series.Size = New System.Drawing.Size(29, 22)
        Me.btnHg_Dry_Deposition_Time_Series.TabIndex = 23
        Me.btnHg_Dry_Deposition_Time_Series.Tag = "Table"
        Me.btnHg_Dry_Deposition_Time_Series.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnHg_Dry_Deposition_Time_Series.UseVisualStyleBackColor = False
        '
        'cboHg_Dry_Deposition_Time_Series
        '
        Me.cboHg_Dry_Deposition_Time_Series.BackColor = System.Drawing.SystemColors.Window
        Me.cboHg_Dry_Deposition_Time_Series.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHg_Dry_Deposition_Time_Series.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHg_Dry_Deposition_Time_Series.Enabled = False
        Me.cboHg_Dry_Deposition_Time_Series.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHg_Dry_Deposition_Time_Series.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHg_Dry_Deposition_Time_Series.Location = New System.Drawing.Point(218, 226)
        Me.cboHg_Dry_Deposition_Time_Series.Name = "cboHg_Dry_Deposition_Time_Series"
        Me.cboHg_Dry_Deposition_Time_Series.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHg_Dry_Deposition_Time_Series.Size = New System.Drawing.Size(146, 22)
        Me.cboHg_Dry_Deposition_Time_Series.TabIndex = 24
        Me.cboHg_Dry_Deposition_Time_Series.Tag = "Table"
        '
        'chkTime
        '
        Me.chkTime.AutoSize = True
        Me.chkTime.BackColor = System.Drawing.SystemColors.Control
        Me.chkTime.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTime.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTime.Location = New System.Drawing.Point(8, 183)
        Me.chkTime.Name = "chkTime"
        Me.chkTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTime.Size = New System.Drawing.Size(186, 18)
        Me.chkTime.TabIndex = 18
        Me.chkTime.Text = "Time-variable Mercury Deposition"
        Me.chkTime.UseVisualStyleBackColor = False
        '
        'chkGrid
        '
        Me.chkGrid.AutoSize = True
        Me.chkGrid.BackColor = System.Drawing.SystemColors.Control
        Me.chkGrid.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkGrid.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkGrid.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkGrid.Location = New System.Drawing.Point(6, 107)
        Me.chkGrid.Name = "chkGrid"
        Me.chkGrid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkGrid.Size = New System.Drawing.Size(142, 18)
        Me.chkGrid.TabIndex = 7
        Me.chkGrid.Text = "Mercury Deposition Grid"
        Me.chkGrid.UseVisualStyleBackColor = False
        '
        'chkConstant
        '
        Me.chkConstant.AutoSize = True
        Me.chkConstant.BackColor = System.Drawing.SystemColors.Control
        Me.chkConstant.Checked = True
        Me.chkConstant.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkConstant.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkConstant.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkConstant.Location = New System.Drawing.Point(8, 16)
        Me.chkConstant.Name = "chkConstant"
        Me.chkConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkConstant.Size = New System.Drawing.Size(165, 18)
        Me.chkConstant.TabIndex = 0
        Me.chkConstant.Text = "Constant Mercury Deposition"
        Me.chkConstant.UseVisualStyleBackColor = False
        '
        'HgWetPrcpConc
        '
        Me.HgWetPrcpConc.AcceptsReturn = True
        Me.HgWetPrcpConc.BackColor = System.Drawing.SystemColors.Window
        Me.HgWetPrcpConc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWetPrcpConc.Enabled = False
        Me.HgWetPrcpConc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWetPrcpConc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWetPrcpConc.Location = New System.Drawing.Point(272, 80)
        Me.HgWetPrcpConc.MaxLength = 0
        Me.HgWetPrcpConc.Name = "HgWetPrcpConc"
        Me.HgWetPrcpConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWetPrcpConc.Size = New System.Drawing.Size(54, 20)
        Me.HgWetPrcpConc.TabIndex = 6
        Me.HgWetPrcpConc.Text = "15.0"
        '
        'optionHgWetPrcpConc
        '
        Me.optionHgWetPrcpConc.AutoSize = True
        Me.optionHgWetPrcpConc.BackColor = System.Drawing.SystemColors.Control
        Me.optionHgWetPrcpConc.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionHgWetPrcpConc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionHgWetPrcpConc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionHgWetPrcpConc.Location = New System.Drawing.Point(40, 80)
        Me.optionHgWetPrcpConc.Name = "optionHgWetPrcpConc"
        Me.optionHgWetPrcpConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionHgWetPrcpConc.Size = New System.Drawing.Size(211, 18)
        Me.optionHgWetPrcpConc.TabIndex = 5
        Me.optionHgWetPrcpConc.TabStop = True
        Me.optionHgWetPrcpConc.Text = "Hg Concentration in Precipitation (ng/l):"
        Me.optionHgWetPrcpConc.UseVisualStyleBackColor = False
        '
        'HgWetMultiplier
        '
        Me.HgWetMultiplier.AcceptsReturn = True
        Me.HgWetMultiplier.BackColor = System.Drawing.SystemColors.Window
        Me.HgWetMultiplier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWetMultiplier.Enabled = False
        Me.HgWetMultiplier.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWetMultiplier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWetMultiplier.Location = New System.Drawing.Point(461, 152)
        Me.HgWetMultiplier.MaxLength = 0
        Me.HgWetMultiplier.Name = "HgWetMultiplier"
        Me.HgWetMultiplier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWetMultiplier.Size = New System.Drawing.Size(30, 20)
        Me.HgWetMultiplier.TabIndex = 17
        Me.HgWetMultiplier.Text = "1"
        '
        'HgWetConstant
        '
        Me.HgWetConstant.AcceptsReturn = True
        Me.HgWetConstant.BackColor = System.Drawing.SystemColors.Window
        Me.HgWetConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWetConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWetConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWetConstant.Location = New System.Drawing.Point(272, 56)
        Me.HgWetConstant.MaxLength = 0
        Me.HgWetConstant.Name = "HgWetConstant"
        Me.HgWetConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWetConstant.Size = New System.Drawing.Size(54, 20)
        Me.HgWetConstant.TabIndex = 4
        Me.HgWetConstant.Text = "0.15"
        '
        'optionHgWetConst
        '
        Me.optionHgWetConst.AutoSize = True
        Me.optionHgWetConst.BackColor = System.Drawing.SystemColors.Control
        Me.optionHgWetConst.Checked = True
        Me.optionHgWetConst.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionHgWetConst.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionHgWetConst.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionHgWetConst.Location = New System.Drawing.Point(40, 56)
        Me.optionHgWetConst.Name = "optionHgWetConst"
        Me.optionHgWetConst.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionHgWetConst.Size = New System.Drawing.Size(185, 18)
        Me.optionHgWetConst.TabIndex = 3
        Me.optionHgWetConst.TabStop = True
        Me.optionHgWetConst.Text = "Hg Wet Deposition Flux Constant:"
        Me.optionHgWetConst.UseVisualStyleBackColor = False
        '
        'btnHg_Wet_Deposition_Flux
        '
        Me.btnHg_Wet_Deposition_Flux.AutoSize = True
        Me.btnHg_Wet_Deposition_Flux.BackColor = System.Drawing.SystemColors.Control
        Me.btnHg_Wet_Deposition_Flux.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHg_Wet_Deposition_Flux.Enabled = False
        Me.btnHg_Wet_Deposition_Flux.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHg_Wet_Deposition_Flux.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHg_Wet_Deposition_Flux.Image = CType(resources.GetObject("btnHg_Wet_Deposition_Flux.Image"), System.Drawing.Image)
        Me.btnHg_Wet_Deposition_Flux.Location = New System.Drawing.Point(370, 150)
        Me.btnHg_Wet_Deposition_Flux.Name = "btnHg_Wet_Deposition_Flux"
        Me.btnHg_Wet_Deposition_Flux.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHg_Wet_Deposition_Flux.Size = New System.Drawing.Size(29, 22)
        Me.btnHg_Wet_Deposition_Flux.TabIndex = 14
        Me.btnHg_Wet_Deposition_Flux.Tag = "Raster"
        Me.btnHg_Wet_Deposition_Flux.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnHg_Wet_Deposition_Flux.UseVisualStyleBackColor = False
        '
        'cboHg_Wet_Deposition_Flux
        '
        Me.cboHg_Wet_Deposition_Flux.BackColor = System.Drawing.SystemColors.Window
        Me.cboHg_Wet_Deposition_Flux.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHg_Wet_Deposition_Flux.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHg_Wet_Deposition_Flux.Enabled = False
        Me.cboHg_Wet_Deposition_Flux.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHg_Wet_Deposition_Flux.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHg_Wet_Deposition_Flux.Location = New System.Drawing.Point(218, 152)
        Me.cboHg_Wet_Deposition_Flux.Name = "cboHg_Wet_Deposition_Flux"
        Me.cboHg_Wet_Deposition_Flux.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHg_Wet_Deposition_Flux.Size = New System.Drawing.Size(146, 22)
        Me.cboHg_Wet_Deposition_Flux.TabIndex = 15
        Me.cboHg_Wet_Deposition_Flux.Tag = "Raster"
        '
        'HgDryMultiplier
        '
        Me.HgDryMultiplier.AcceptsReturn = True
        Me.HgDryMultiplier.BackColor = System.Drawing.SystemColors.Window
        Me.HgDryMultiplier.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgDryMultiplier.Enabled = False
        Me.HgDryMultiplier.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgDryMultiplier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgDryMultiplier.Location = New System.Drawing.Point(461, 128)
        Me.HgDryMultiplier.MaxLength = 0
        Me.HgDryMultiplier.Name = "HgDryMultiplier"
        Me.HgDryMultiplier.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgDryMultiplier.Size = New System.Drawing.Size(30, 20)
        Me.HgDryMultiplier.TabIndex = 12
        Me.HgDryMultiplier.Text = "1"
        '
        'HgDryConstant
        '
        Me.HgDryConstant.AcceptsReturn = True
        Me.HgDryConstant.BackColor = System.Drawing.SystemColors.Window
        Me.HgDryConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgDryConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgDryConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgDryConstant.Location = New System.Drawing.Point(272, 32)
        Me.HgDryConstant.MaxLength = 0
        Me.HgDryConstant.Name = "HgDryConstant"
        Me.HgDryConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgDryConstant.Size = New System.Drawing.Size(54, 20)
        Me.HgDryConstant.TabIndex = 2
        Me.HgDryConstant.Text = "0.03"
        '
        'cboHg_Dry_Deposition_Flux
        '
        Me.cboHg_Dry_Deposition_Flux.BackColor = System.Drawing.SystemColors.Window
        Me.cboHg_Dry_Deposition_Flux.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboHg_Dry_Deposition_Flux.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHg_Dry_Deposition_Flux.Enabled = False
        Me.cboHg_Dry_Deposition_Flux.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboHg_Dry_Deposition_Flux.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboHg_Dry_Deposition_Flux.Location = New System.Drawing.Point(218, 128)
        Me.cboHg_Dry_Deposition_Flux.Name = "cboHg_Dry_Deposition_Flux"
        Me.cboHg_Dry_Deposition_Flux.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboHg_Dry_Deposition_Flux.Size = New System.Drawing.Size(146, 22)
        Me.cboHg_Dry_Deposition_Flux.TabIndex = 10
        Me.cboHg_Dry_Deposition_Flux.Tag = "Raster"
        '
        'btnHg_Dry_Deposition_Flux
        '
        Me.btnHg_Dry_Deposition_Flux.AutoSize = True
        Me.btnHg_Dry_Deposition_Flux.BackColor = System.Drawing.SystemColors.Control
        Me.btnHg_Dry_Deposition_Flux.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnHg_Dry_Deposition_Flux.Enabled = False
        Me.btnHg_Dry_Deposition_Flux.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHg_Dry_Deposition_Flux.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHg_Dry_Deposition_Flux.Image = CType(resources.GetObject("btnHg_Dry_Deposition_Flux.Image"), System.Drawing.Image)
        Me.btnHg_Dry_Deposition_Flux.Location = New System.Drawing.Point(370, 126)
        Me.btnHg_Dry_Deposition_Flux.Name = "btnHg_Dry_Deposition_Flux"
        Me.btnHg_Dry_Deposition_Flux.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnHg_Dry_Deposition_Flux.Size = New System.Drawing.Size(29, 22)
        Me.btnHg_Dry_Deposition_Flux.TabIndex = 9
        Me.btnHg_Dry_Deposition_Flux.Tag = "Raster"
        Me.btnHg_Dry_Deposition_Flux.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnHg_Dry_Deposition_Flux.UseVisualStyleBackColor = False
        '
        'lblHgWet
        '
        Me.lblHgWet.AutoSize = True
        Me.lblHgWet.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgWet.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgWet.Enabled = False
        Me.lblHgWet.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgWet.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgWet.Location = New System.Drawing.Point(32, 155)
        Me.lblHgWet.Name = "lblHgWet"
        Me.lblHgWet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgWet.Size = New System.Drawing.Size(144, 14)
        Me.lblHgWet.TabIndex = 13
        Me.lblHgWet.Text = "Hg Wet Deposition Flux Grid:"
        '
        'lblHgDry
        '
        Me.lblHgDry.AutoSize = True
        Me.lblHgDry.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgDry.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgDry.Enabled = False
        Me.lblHgDry.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgDry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgDry.Location = New System.Drawing.Point(32, 131)
        Me.lblHgDry.Name = "lblHgDry"
        Me.lblHgDry.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgDry.Size = New System.Drawing.Size(142, 14)
        Me.lblHgDry.TabIndex = 8
        Me.lblHgDry.Text = "Hg Dry Deposition Flux Grid:"
        '
        'lblHgWetDepTS
        '
        Me.lblHgWetDepTS.AutoSize = True
        Me.lblHgWetDepTS.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgWetDepTS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgWetDepTS.Enabled = False
        Me.lblHgWetDepTS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgWetDepTS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgWetDepTS.Location = New System.Drawing.Point(32, 253)
        Me.lblHgWetDepTS.Name = "lblHgWetDepTS"
        Me.lblHgWetDepTS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgWetDepTS.Size = New System.Drawing.Size(184, 14)
        Me.lblHgWetDepTS.TabIndex = 26
        Me.lblHgWetDepTS.Text = "Mercury Wet Deposition Time Series:"
        '
        'lblDryHgDepTS
        '
        Me.lblDryHgDepTS.AutoSize = True
        Me.lblDryHgDepTS.BackColor = System.Drawing.SystemColors.Control
        Me.lblDryHgDepTS.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDryHgDepTS.Enabled = False
        Me.lblDryHgDepTS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDryHgDepTS.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDryHgDepTS.Location = New System.Drawing.Point(32, 229)
        Me.lblDryHgDepTS.Name = "lblDryHgDepTS"
        Me.lblDryHgDepTS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDryHgDepTS.Size = New System.Drawing.Size(182, 14)
        Me.lblDryHgDepTS.TabIndex = 22
        Me.lblDryHgDepTS.Text = "Mercury Dry Deposition Time Series:"
        '
        'lblHgStation
        '
        Me.lblHgStation.AutoSize = True
        Me.lblHgStation.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgStation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgStation.Enabled = False
        Me.lblHgStation.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgStation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgStation.Location = New System.Drawing.Point(32, 207)
        Me.lblHgStation.Name = "lblHgStation"
        Me.lblHgStation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgStation.Size = New System.Drawing.Size(148, 14)
        Me.lblHgStation.TabIndex = 19
        Me.lblHgStation.Text = "Mercury Observation Station:"
        '
        'lblDryDeposition
        '
        Me.lblDryDeposition.AutoSize = True
        Me.lblDryDeposition.BackColor = System.Drawing.SystemColors.Control
        Me.lblDryDeposition.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDryDeposition.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDryDeposition.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDryDeposition.Location = New System.Drawing.Point(37, 35)
        Me.lblDryDeposition.Name = "lblDryDeposition"
        Me.lblDryDeposition.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDryDeposition.Size = New System.Drawing.Size(165, 14)
        Me.lblDryDeposition.TabIndex = 1
        Me.lblDryDeposition.Text = "Hg Dry Deposition Flux Constant:"
        '
        'lblHgWetMul
        '
        Me.lblHgWetMul.AutoSize = True
        Me.lblHgWetMul.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgWetMul.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgWetMul.Enabled = False
        Me.lblHgWetMul.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgWetMul.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgWetMul.Location = New System.Drawing.Point(405, 155)
        Me.lblHgWetMul.Name = "lblHgWetMul"
        Me.lblHgWetMul.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgWetMul.Size = New System.Drawing.Size(51, 14)
        Me.lblHgWetMul.TabIndex = 16
        Me.lblHgWetMul.Text = "Multiplier:"
        '
        'lblHgDryMul
        '
        Me.lblHgDryMul.AutoSize = True
        Me.lblHgDryMul.BackColor = System.Drawing.SystemColors.Control
        Me.lblHgDryMul.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblHgDryMul.Enabled = False
        Me.lblHgDryMul.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHgDryMul.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHgDryMul.Location = New System.Drawing.Point(405, 131)
        Me.lblHgDryMul.Name = "lblHgDryMul"
        Me.lblHgDryMul.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHgDryMul.Size = New System.Drawing.Size(51, 14)
        Me.lblHgDryMul.TabIndex = 11
        Me.lblHgDryMul.Text = "Multiplier:"
        '
        '_SSTab1_TabPage1
        '
        Me._SSTab1_TabPage1.Controls.Add(Me.MercuryLand)
        Me._SSTab1_TabPage1.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage1.Name = "_SSTab1_TabPage1"
        Me._SSTab1_TabPage1.Size = New System.Drawing.Size(513, 392)
        Me._SSTab1_TabPage1.TabIndex = 1
        Me._SSTab1_TabPage1.Text = "Land"
        '
        'MercuryLand
        '
        Me.MercuryLand.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.MercuryLand.BackColor = System.Drawing.SystemColors.Control
        Me.MercuryLand.Controls.Add(Me.InitialHgSaturatedSoilConstant)
        Me.MercuryLand.Controls.Add(Me.HgLandSoilParticleDensity)
        Me.MercuryLand.Controls.Add(Me.HgLandBedrockDensity)
        Me.MercuryLand.Controls.Add(Me.HgLandBedrockHgConc)
        Me.MercuryLand.Controls.Add(Me.HgLandPollutantEnrichmentFactor)
        Me.MercuryLand.Controls.Add(Me.HgLandSoilReductionDepth)
        Me.MercuryLand.Controls.Add(Me.HgLandSoilBaseReductionRate)
        Me.MercuryLand.Controls.Add(Me.HgLandChemicalWeatheringRate)
        Me.MercuryLand.Controls.Add(Me.HgLandSoilWaterPartitionCoeff)
        Me.MercuryLand.Controls.Add(Me.HgLandSoilMixingDepth)
        Me.MercuryLand.Controls.Add(Me.HgLandAirPlantBioConc)
        Me.MercuryLand.Controls.Add(Me.HgLandAirHgConc)
        Me.MercuryLand.Controls.Add(Me.Label85)
        Me.MercuryLand.Controls.Add(Me.Label109)
        Me.MercuryLand.Controls.Add(Me.Label60)
        Me.MercuryLand.Controls.Add(Me.Label59)
        Me.MercuryLand.Controls.Add(Me.Label61)
        Me.MercuryLand.Controls.Add(Me.Label58)
        Me.MercuryLand.Controls.Add(Me.Label57)
        Me.MercuryLand.Controls.Add(Me.Label56)
        Me.MercuryLand.Controls.Add(Me.Label55)
        Me.MercuryLand.Controls.Add(Me.Label54)
        Me.MercuryLand.Controls.Add(Me.Label53)
        Me.MercuryLand.Controls.Add(Me.Label52)
        Me.MercuryLand.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MercuryLand.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MercuryLand.Location = New System.Drawing.Point(52, 22)
        Me.MercuryLand.Name = "MercuryLand"
        Me.MercuryLand.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.MercuryLand.Size = New System.Drawing.Size(399, 344)
        Me.MercuryLand.TabIndex = 0
        Me.MercuryLand.TabStop = False
        Me.MercuryLand.Text = "Define Constants for Mercury ( Land )"
        '
        'InitialHgSaturatedSoilConstant
        '
        Me.InitialHgSaturatedSoilConstant.AcceptsReturn = True
        Me.InitialHgSaturatedSoilConstant.BackColor = System.Drawing.SystemColors.Window
        Me.InitialHgSaturatedSoilConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialHgSaturatedSoilConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialHgSaturatedSoilConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialHgSaturatedSoilConstant.Location = New System.Drawing.Point(322, 72)
        Me.InitialHgSaturatedSoilConstant.MaxLength = 0
        Me.InitialHgSaturatedSoilConstant.Name = "InitialHgSaturatedSoilConstant"
        Me.InitialHgSaturatedSoilConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialHgSaturatedSoilConstant.Size = New System.Drawing.Size(67, 20)
        Me.InitialHgSaturatedSoilConstant.TabIndex = 5
        Me.InitialHgSaturatedSoilConstant.Text = "0.00001"
        '
        'HgLandSoilParticleDensity
        '
        Me.HgLandSoilParticleDensity.AcceptsReturn = True
        Me.HgLandSoilParticleDensity.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandSoilParticleDensity.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandSoilParticleDensity.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandSoilParticleDensity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandSoilParticleDensity.Location = New System.Drawing.Point(322, 144)
        Me.HgLandSoilParticleDensity.MaxLength = 0
        Me.HgLandSoilParticleDensity.Name = "HgLandSoilParticleDensity"
        Me.HgLandSoilParticleDensity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandSoilParticleDensity.Size = New System.Drawing.Size(67, 20)
        Me.HgLandSoilParticleDensity.TabIndex = 11
        Me.HgLandSoilParticleDensity.Text = "2.65"
        '
        'HgLandBedrockDensity
        '
        Me.HgLandBedrockDensity.AcceptsReturn = True
        Me.HgLandBedrockDensity.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandBedrockDensity.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandBedrockDensity.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandBedrockDensity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandBedrockDensity.Location = New System.Drawing.Point(322, 240)
        Me.HgLandBedrockDensity.MaxLength = 0
        Me.HgLandBedrockDensity.Name = "HgLandBedrockDensity"
        Me.HgLandBedrockDensity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandBedrockDensity.Size = New System.Drawing.Size(67, 20)
        Me.HgLandBedrockDensity.TabIndex = 19
        Me.HgLandBedrockDensity.Text = "2.6"
        '
        'HgLandBedrockHgConc
        '
        Me.HgLandBedrockHgConc.AcceptsReturn = True
        Me.HgLandBedrockHgConc.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandBedrockHgConc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandBedrockHgConc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandBedrockHgConc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandBedrockHgConc.Location = New System.Drawing.Point(322, 288)
        Me.HgLandBedrockHgConc.MaxLength = 0
        Me.HgLandBedrockHgConc.Name = "HgLandBedrockHgConc"
        Me.HgLandBedrockHgConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandBedrockHgConc.Size = New System.Drawing.Size(67, 20)
        Me.HgLandBedrockHgConc.TabIndex = 23
        Me.HgLandBedrockHgConc.Text = "60"
        '
        'HgLandPollutantEnrichmentFactor
        '
        Me.HgLandPollutantEnrichmentFactor.AcceptsReturn = True
        Me.HgLandPollutantEnrichmentFactor.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandPollutantEnrichmentFactor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandPollutantEnrichmentFactor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandPollutantEnrichmentFactor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandPollutantEnrichmentFactor.Location = New System.Drawing.Point(322, 216)
        Me.HgLandPollutantEnrichmentFactor.MaxLength = 0
        Me.HgLandPollutantEnrichmentFactor.Name = "HgLandPollutantEnrichmentFactor"
        Me.HgLandPollutantEnrichmentFactor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandPollutantEnrichmentFactor.Size = New System.Drawing.Size(67, 20)
        Me.HgLandPollutantEnrichmentFactor.TabIndex = 17
        Me.HgLandPollutantEnrichmentFactor.Text = "2"
        '
        'HgLandSoilReductionDepth
        '
        Me.HgLandSoilReductionDepth.AcceptsReturn = True
        Me.HgLandSoilReductionDepth.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandSoilReductionDepth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandSoilReductionDepth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandSoilReductionDepth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandSoilReductionDepth.Location = New System.Drawing.Point(322, 120)
        Me.HgLandSoilReductionDepth.MaxLength = 0
        Me.HgLandSoilReductionDepth.Name = "HgLandSoilReductionDepth"
        Me.HgLandSoilReductionDepth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandSoilReductionDepth.Size = New System.Drawing.Size(67, 20)
        Me.HgLandSoilReductionDepth.TabIndex = 9
        Me.HgLandSoilReductionDepth.Text = "0.5"
        '
        'HgLandSoilBaseReductionRate
        '
        Me.HgLandSoilBaseReductionRate.AcceptsReturn = True
        Me.HgLandSoilBaseReductionRate.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandSoilBaseReductionRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandSoilBaseReductionRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandSoilBaseReductionRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandSoilBaseReductionRate.Location = New System.Drawing.Point(322, 192)
        Me.HgLandSoilBaseReductionRate.MaxLength = 0
        Me.HgLandSoilBaseReductionRate.Name = "HgLandSoilBaseReductionRate"
        Me.HgLandSoilBaseReductionRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandSoilBaseReductionRate.Size = New System.Drawing.Size(67, 20)
        Me.HgLandSoilBaseReductionRate.TabIndex = 15
        Me.HgLandSoilBaseReductionRate.Text = "0.0001"
        '
        'HgLandChemicalWeatheringRate
        '
        Me.HgLandChemicalWeatheringRate.AcceptsReturn = True
        Me.HgLandChemicalWeatheringRate.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandChemicalWeatheringRate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandChemicalWeatheringRate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandChemicalWeatheringRate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandChemicalWeatheringRate.Location = New System.Drawing.Point(322, 264)
        Me.HgLandChemicalWeatheringRate.MaxLength = 0
        Me.HgLandChemicalWeatheringRate.Name = "HgLandChemicalWeatheringRate"
        Me.HgLandChemicalWeatheringRate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandChemicalWeatheringRate.Size = New System.Drawing.Size(67, 20)
        Me.HgLandChemicalWeatheringRate.TabIndex = 21
        Me.HgLandChemicalWeatheringRate.Text = "0.0004"
        '
        'HgLandSoilWaterPartitionCoeff
        '
        Me.HgLandSoilWaterPartitionCoeff.AcceptsReturn = True
        Me.HgLandSoilWaterPartitionCoeff.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandSoilWaterPartitionCoeff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandSoilWaterPartitionCoeff.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandSoilWaterPartitionCoeff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandSoilWaterPartitionCoeff.Location = New System.Drawing.Point(322, 168)
        Me.HgLandSoilWaterPartitionCoeff.MaxLength = 0
        Me.HgLandSoilWaterPartitionCoeff.Name = "HgLandSoilWaterPartitionCoeff"
        Me.HgLandSoilWaterPartitionCoeff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandSoilWaterPartitionCoeff.Size = New System.Drawing.Size(67, 20)
        Me.HgLandSoilWaterPartitionCoeff.TabIndex = 13
        Me.HgLandSoilWaterPartitionCoeff.Text = "58000"
        '
        'HgLandSoilMixingDepth
        '
        Me.HgLandSoilMixingDepth.AcceptsReturn = True
        Me.HgLandSoilMixingDepth.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandSoilMixingDepth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandSoilMixingDepth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandSoilMixingDepth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandSoilMixingDepth.Location = New System.Drawing.Point(322, 96)
        Me.HgLandSoilMixingDepth.MaxLength = 0
        Me.HgLandSoilMixingDepth.Name = "HgLandSoilMixingDepth"
        Me.HgLandSoilMixingDepth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandSoilMixingDepth.Size = New System.Drawing.Size(67, 20)
        Me.HgLandSoilMixingDepth.TabIndex = 7
        Me.HgLandSoilMixingDepth.Text = "1"
        '
        'HgLandAirPlantBioConc
        '
        Me.HgLandAirPlantBioConc.AcceptsReturn = True
        Me.HgLandAirPlantBioConc.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandAirPlantBioConc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandAirPlantBioConc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandAirPlantBioConc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandAirPlantBioConc.Location = New System.Drawing.Point(322, 48)
        Me.HgLandAirPlantBioConc.MaxLength = 0
        Me.HgLandAirPlantBioConc.Name = "HgLandAirPlantBioConc"
        Me.HgLandAirPlantBioConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandAirPlantBioConc.Size = New System.Drawing.Size(67, 20)
        Me.HgLandAirPlantBioConc.TabIndex = 3
        Me.HgLandAirPlantBioConc.Text = "18000"
        '
        'HgLandAirHgConc
        '
        Me.HgLandAirHgConc.AcceptsReturn = True
        Me.HgLandAirHgConc.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandAirHgConc.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandAirHgConc.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandAirHgConc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandAirHgConc.Location = New System.Drawing.Point(322, 24)
        Me.HgLandAirHgConc.MaxLength = 0
        Me.HgLandAirHgConc.Name = "HgLandAirHgConc"
        Me.HgLandAirHgConc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandAirHgConc.Size = New System.Drawing.Size(67, 20)
        Me.HgLandAirHgConc.TabIndex = 1
        Me.HgLandAirHgConc.Text = "0.00155"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.BackColor = System.Drawing.SystemColors.Control
        Me.Label85.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label85.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label85.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label85.Location = New System.Drawing.Point(40, 72)
        Me.Label85.Name = "Label85"
        Me.Label85.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label85.Size = New System.Drawing.Size(242, 14)
        Me.Label85.TabIndex = 4
        Me.Label85.Text = "Initial Groundwater Mercury Concentration (ng/l):"
        '
        'Label109
        '
        Me.Label109.AutoSize = True
        Me.Label109.BackColor = System.Drawing.SystemColors.Control
        Me.Label109.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label109.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label109.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label109.Location = New System.Drawing.Point(40, 144)
        Me.Label109.Name = "Label109"
        Me.Label109.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label109.Size = New System.Drawing.Size(142, 14)
        Me.Label109.TabIndex = 10
        Me.Label109.Text = "Soil Particle Density (g/cm³):"
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.BackColor = System.Drawing.SystemColors.Control
        Me.Label60.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label60.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label60.Location = New System.Drawing.Point(40, 288)
        Me.Label60.Name = "Label60"
        Me.Label60.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label60.Size = New System.Drawing.Size(195, 14)
        Me.Label60.TabIndex = 22
        Me.Label60.Text = "Bedrock Mercury Concentration (ng/g):"
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.BackColor = System.Drawing.SystemColors.Control
        Me.Label59.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label59.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label59.Location = New System.Drawing.Point(40, 264)
        Me.Label59.Name = "Label59"
        Me.Label59.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label59.Size = New System.Drawing.Size(182, 14)
        Me.Label59.TabIndex = 20
        Me.Label59.Text = "Chemical Weathering Rate (µm/day):"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.BackColor = System.Drawing.SystemColors.Control
        Me.Label61.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label61.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label61.Location = New System.Drawing.Point(40, 240)
        Me.Label61.Name = "Label61"
        Me.Label61.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label61.Size = New System.Drawing.Size(127, 14)
        Me.Label61.TabIndex = 18
        Me.Label61.Text = "Bedrock Density (g/cm³):"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.BackColor = System.Drawing.SystemColors.Control
        Me.Label58.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label58.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label58.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label58.Location = New System.Drawing.Point(40, 216)
        Me.Label58.Name = "Label58"
        Me.Label58.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label58.Size = New System.Drawing.Size(140, 14)
        Me.Label58.TabIndex = 16
        Me.Label58.Text = "Pollutant Enrichment Factor:"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.BackColor = System.Drawing.SystemColors.Control
        Me.Label57.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label57.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label57.Location = New System.Drawing.Point(40, 120)
        Me.Label57.Name = "Label57"
        Me.Label57.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label57.Size = New System.Drawing.Size(134, 14)
        Me.Label57.TabIndex = 8
        Me.Label57.Text = "Soil Reduction Depth (cm):"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.BackColor = System.Drawing.SystemColors.Control
        Me.Label56.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label56.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label56.Location = New System.Drawing.Point(40, 192)
        Me.Label56.Name = "Label56"
        Me.Label56.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label56.Size = New System.Drawing.Size(179, 14)
        Me.Label56.TabIndex = 14
        Me.Label56.Text = "Soil Base Reduction Rate (per day):"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.BackColor = System.Drawing.SystemColors.Control
        Me.Label55.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label55.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label55.Location = New System.Drawing.Point(40, 168)
        Me.Label55.Name = "Label55"
        Me.Label55.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label55.Size = New System.Drawing.Size(185, 14)
        Me.Label55.TabIndex = 12
        Me.Label55.Text = "Soil Water Partition Coefficient (ml/g):"
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.BackColor = System.Drawing.SystemColors.Control
        Me.Label54.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label54.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label54.Location = New System.Drawing.Point(40, 96)
        Me.Label54.Name = "Label54"
        Me.Label54.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label54.Size = New System.Drawing.Size(172, 14)
        Me.Label54.TabIndex = 6
        Me.Label54.Text = "Watershed Soil Mixing Depth (cm):"
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.BackColor = System.Drawing.SystemColors.Control
        Me.Label53.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label53.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label53.Location = New System.Drawing.Point(40, 48)
        Me.Label53.Name = "Label53"
        Me.Label53.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label53.Size = New System.Drawing.Size(205, 14)
        Me.Label53.TabIndex = 2
        Me.Label53.Text = "Air-Plant Bio-Concentration Factor (BCF):"
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.BackColor = System.Drawing.SystemColors.Control
        Me.Label52.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label52.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label52.Location = New System.Drawing.Point(40, 24)
        Me.Label52.Name = "Label52"
        Me.Label52.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label52.Size = New System.Drawing.Size(169, 14)
        Me.Label52.TabIndex = 0
        Me.Label52.Text = "Air Mercury Concentration (ng/g):"
        '
        '_SSTab1_TabPage2
        '
        Me._SSTab1_TabPage2.Controls.Add(Me.Frame10)
        Me._SSTab1_TabPage2.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage2.Name = "_SSTab1_TabPage2"
        Me._SSTab1_TabPage2.Size = New System.Drawing.Size(513, 392)
        Me._SSTab1_TabPage2.TabIndex = 2
        Me._SSTab1_TabPage2.Text = "Forest"
        '
        'Frame10
        '
        Me.Frame10.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Frame10.BackColor = System.Drawing.SystemColors.Control
        Me.Frame10.Controls.Add(Me.Label9)
        Me.Frame10.Controls.Add(Me.HgLandAnnualEvapo)
        Me.Frame10.Controls.Add(Me.InitialLeafLitterConstant)
        Me.Frame10.Controls.Add(Me.HgLandLeafAdhereFraction)
        Me.Frame10.Controls.Add(Me.HgLandLeafInterFraction)
        Me.Frame10.Controls.Add(Me.HgLandKDcomp1)
        Me.Frame10.Controls.Add(Me.HgLandKDcomp2)
        Me.Frame10.Controls.Add(Me.HgLandKDcomp3)
        Me.Frame10.Controls.Add(Me.Label51)
        Me.Frame10.Controls.Add(Me.Label50)
        Me.Frame10.Controls.Add(Me.Label49)
        Me.Frame10.Controls.Add(Me.Label113)
        Me.Frame10.Controls.Add(Me.Label114)
        Me.Frame10.Controls.Add(Me.Label115)
        Me.Frame10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame10.Location = New System.Drawing.Point(31, 52)
        Me.Frame10.Name = "Frame10"
        Me.Frame10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame10.Size = New System.Drawing.Size(447, 271)
        Me.Frame10.TabIndex = 0
        Me.Frame10.TabStop = False
        Me.Frame10.Text = "Define Parameters for Forest Land:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 113)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(195, 14)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Initial Leaf Litter for Forest Land (g/m²):"
        '
        'HgLandAnnualEvapo
        '
        Me.HgLandAnnualEvapo.AcceptsReturn = True
        Me.HgLandAnnualEvapo.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandAnnualEvapo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandAnnualEvapo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandAnnualEvapo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandAnnualEvapo.Location = New System.Drawing.Point(330, 21)
        Me.HgLandAnnualEvapo.MaxLength = 0
        Me.HgLandAnnualEvapo.Name = "HgLandAnnualEvapo"
        Me.HgLandAnnualEvapo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandAnnualEvapo.Size = New System.Drawing.Size(87, 20)
        Me.HgLandAnnualEvapo.TabIndex = 1
        Me.HgLandAnnualEvapo.Text = "30"
        '
        'InitialLeafLitterConstant
        '
        Me.InitialLeafLitterConstant.AcceptsReturn = True
        Me.InitialLeafLitterConstant.BackColor = System.Drawing.SystemColors.Window
        Me.InitialLeafLitterConstant.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InitialLeafLitterConstant.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InitialLeafLitterConstant.ForeColor = System.Drawing.SystemColors.WindowText
        Me.InitialLeafLitterConstant.Location = New System.Drawing.Point(330, 110)
        Me.InitialLeafLitterConstant.MaxLength = 0
        Me.InitialLeafLitterConstant.Name = "InitialLeafLitterConstant"
        Me.InitialLeafLitterConstant.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InitialLeafLitterConstant.Size = New System.Drawing.Size(87, 20)
        Me.InitialLeafLitterConstant.TabIndex = 7
        Me.InitialLeafLitterConstant.Text = "40"
        '
        'HgLandLeafAdhereFraction
        '
        Me.HgLandLeafAdhereFraction.AcceptsReturn = True
        Me.HgLandLeafAdhereFraction.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandLeafAdhereFraction.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandLeafAdhereFraction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandLeafAdhereFraction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandLeafAdhereFraction.Location = New System.Drawing.Point(330, 80)
        Me.HgLandLeafAdhereFraction.MaxLength = 0
        Me.HgLandLeafAdhereFraction.Name = "HgLandLeafAdhereFraction"
        Me.HgLandLeafAdhereFraction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandLeafAdhereFraction.Size = New System.Drawing.Size(87, 20)
        Me.HgLandLeafAdhereFraction.TabIndex = 5
        Me.HgLandLeafAdhereFraction.Text = "0.6"
        '
        'HgLandLeafInterFraction
        '
        Me.HgLandLeafInterFraction.AcceptsReturn = True
        Me.HgLandLeafInterFraction.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandLeafInterFraction.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandLeafInterFraction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandLeafInterFraction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandLeafInterFraction.Location = New System.Drawing.Point(330, 50)
        Me.HgLandLeafInterFraction.MaxLength = 0
        Me.HgLandLeafInterFraction.Name = "HgLandLeafInterFraction"
        Me.HgLandLeafInterFraction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandLeafInterFraction.Size = New System.Drawing.Size(87, 20)
        Me.HgLandLeafInterFraction.TabIndex = 3
        Me.HgLandLeafInterFraction.Text = "0.47"
        '
        'HgLandKDcomp1
        '
        Me.HgLandKDcomp1.AcceptsReturn = True
        Me.HgLandKDcomp1.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandKDcomp1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandKDcomp1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandKDcomp1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandKDcomp1.Location = New System.Drawing.Point(330, 149)
        Me.HgLandKDcomp1.MaxLength = 0
        Me.HgLandKDcomp1.Name = "HgLandKDcomp1"
        Me.HgLandKDcomp1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandKDcomp1.Size = New System.Drawing.Size(87, 20)
        Me.HgLandKDcomp1.TabIndex = 9
        Me.HgLandKDcomp1.Text = "0.0019"
        '
        'HgLandKDcomp2
        '
        Me.HgLandKDcomp2.AcceptsReturn = True
        Me.HgLandKDcomp2.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandKDcomp2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandKDcomp2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandKDcomp2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandKDcomp2.Location = New System.Drawing.Point(330, 186)
        Me.HgLandKDcomp2.MaxLength = 0
        Me.HgLandKDcomp2.Name = "HgLandKDcomp2"
        Me.HgLandKDcomp2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandKDcomp2.Size = New System.Drawing.Size(87, 20)
        Me.HgLandKDcomp2.TabIndex = 11
        Me.HgLandKDcomp2.Text = "0.0005"
        '
        'HgLandKDcomp3
        '
        Me.HgLandKDcomp3.AcceptsReturn = True
        Me.HgLandKDcomp3.BackColor = System.Drawing.SystemColors.Window
        Me.HgLandKDcomp3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgLandKDcomp3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgLandKDcomp3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgLandKDcomp3.Location = New System.Drawing.Point(330, 223)
        Me.HgLandKDcomp3.MaxLength = 0
        Me.HgLandKDcomp3.Name = "HgLandKDcomp3"
        Me.HgLandKDcomp3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgLandKDcomp3.Size = New System.Drawing.Size(87, 20)
        Me.HgLandKDcomp3.TabIndex = 13
        Me.HgLandKDcomp3.Text = "0.0012"
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.BackColor = System.Drawing.SystemColors.Control
        Me.Label51.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label51.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label51.Location = New System.Drawing.Point(32, 24)
        Me.Label51.Name = "Label51"
        Me.Label51.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label51.Size = New System.Drawing.Size(207, 14)
        Me.Label51.TabIndex = 0
        Me.Label51.Text = "Average Annual Evapotranspiration (cm):"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.BackColor = System.Drawing.SystemColors.Control
        Me.Label50.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label50.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label50.Location = New System.Drawing.Point(32, 83)
        Me.Label50.Name = "Label50"
        Me.Label50.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label50.Size = New System.Drawing.Size(199, 14)
        Me.Label50.TabIndex = 4
        Me.Label50.Text = "Leaf Adhering Fraction (Air Deposition):"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.BackColor = System.Drawing.SystemColors.Control
        Me.Label49.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label49.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label49.Location = New System.Drawing.Point(32, 54)
        Me.Label49.Name = "Label49"
        Me.Label49.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label49.Size = New System.Drawing.Size(211, 14)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Leaf Interception Fraction (Air Deposition):"
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.BackColor = System.Drawing.SystemColors.Control
        Me.Label113.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label113.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label113.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label113.Location = New System.Drawing.Point(32, 143)
        Me.Label113.Name = "Label113"
        Me.Label113.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label113.Size = New System.Drawing.Size(237, 28)
        Me.Label113.TabIndex = 8
        Me.Label113.Text = "Litter Decomposition Rate for Deciduous Forest/" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Deciduous Shrubland (per day):"
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.BackColor = System.Drawing.SystemColors.Control
        Me.Label114.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label114.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label114.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label114.Location = New System.Drawing.Point(32, 180)
        Me.Label114.Name = "Label114"
        Me.Label114.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label114.Size = New System.Drawing.Size(236, 28)
        Me.Label114.TabIndex = 10
        Me.Label114.Text = "Litter Decomposition Rate for Evergreen Forest/" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Evergreen Shrubland (per day):"
        '
        'Label115
        '
        Me.Label115.AutoSize = True
        Me.Label115.BackColor = System.Drawing.SystemColors.Control
        Me.Label115.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label115.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label115.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label115.Location = New System.Drawing.Point(32, 217)
        Me.Label115.Name = "Label115"
        Me.Label115.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label115.Size = New System.Drawing.Size(217, 28)
        Me.Label115.TabIndex = 12
        Me.Label115.Text = "Litter Decomposition Rate for Mixed Forest/" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mixed Shrubland/Woody Wetland (per d" & _
            "ay):"
        '
        '_SSTab1_TabPage3
        '
        Me._SSTab1_TabPage3.Controls.Add(Me.MercuryWater)
        Me._SSTab1_TabPage3.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage3.Name = "_SSTab1_TabPage3"
        Me._SSTab1_TabPage3.Size = New System.Drawing.Size(513, 392)
        Me._SSTab1_TabPage3.TabIndex = 3
        Me._SSTab1_TabPage3.Text = "Water"
        '
        'MercuryWater
        '
        Me.MercuryWater.BackColor = System.Drawing.SystemColors.Control
        Me.MercuryWater.Controls.Add(Me.Label11)
        Me.MercuryWater.Controls.Add(Me.HgBenthicPorewaterDiffusionCoeff)
        Me.MercuryWater.Controls.Add(Me.LakeHgWaterColumn)
        Me.MercuryWater.Controls.Add(Me.Label21)
        Me.MercuryWater.Controls.Add(Me.HgWaterAlphaW)
        Me.MercuryWater.Controls.Add(Me.Label13)
        Me.MercuryWater.Controls.Add(Me.HgWaterKWR)
        Me.MercuryWater.Controls.Add(Me.HgWaterBetaW)
        Me.MercuryWater.Controls.Add(Me.Label14)
        Me.MercuryWater.Controls.Add(Me.HgWaterKWM)
        Me.MercuryWater.Controls.Add(Me.HgWaterVSB)
        Me.MercuryWater.Controls.Add(Me.Label15)
        Me.MercuryWater.Controls.Add(Me.HgWaterVRS)
        Me.MercuryWater.Controls.Add(Me.Label18)
        Me.MercuryWater.Controls.Add(Me.Label16)
        Me.MercuryWater.Controls.Add(Me.HgWaterKDsw)
        Me.MercuryWater.Controls.Add(Me.Label20)
        Me.MercuryWater.Controls.Add(Me.HgWaterKbio)
        Me.MercuryWater.Controls.Add(Me.Label17)
        Me.MercuryWater.Controls.Add(Me.HgWaterCbio)
        Me.MercuryWater.Controls.Add(Me.Label19)
        Me.MercuryWater.Controls.Add(Me.HgWaterHgDecayInChannel)
        Me.MercuryWater.Controls.Add(Me.HgMethylHgFraction)
        Me.MercuryWater.Controls.Add(Me.Label10)
        Me.MercuryWater.Controls.Add(Me.Label120)
        Me.MercuryWater.Controls.Add(Me.Label121)
        Me.MercuryWater.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MercuryWater.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MercuryWater.Location = New System.Drawing.Point(24, 32)
        Me.MercuryWater.Name = "MercuryWater"
        Me.MercuryWater.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.MercuryWater.Size = New System.Drawing.Size(479, 336)
        Me.MercuryWater.TabIndex = 0
        Me.MercuryWater.TabStop = False
        Me.MercuryWater.Text = "Define Constants for Mercury ( Water )"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(42, 262)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(259, 14)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Biomass Concentration in Water Column, Cbio (mg/l):"
        '
        'HgBenthicPorewaterDiffusionCoeff
        '
        Me.HgBenthicPorewaterDiffusionCoeff.AcceptsReturn = True
        Me.HgBenthicPorewaterDiffusionCoeff.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicPorewaterDiffusionCoeff.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicPorewaterDiffusionCoeff.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicPorewaterDiffusionCoeff.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicPorewaterDiffusionCoeff.Location = New System.Drawing.Point(338, 187)
        Me.HgBenthicPorewaterDiffusionCoeff.MaxLength = 0
        Me.HgBenthicPorewaterDiffusionCoeff.Name = "HgBenthicPorewaterDiffusionCoeff"
        Me.HgBenthicPorewaterDiffusionCoeff.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicPorewaterDiffusionCoeff.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicPorewaterDiffusionCoeff.TabIndex = 15
        Me.HgBenthicPorewaterDiffusionCoeff.Text = "0.000000005"
        '
        'LakeHgWaterColumn
        '
        Me.LakeHgWaterColumn.AcceptsReturn = True
        Me.LakeHgWaterColumn.BackColor = System.Drawing.SystemColors.Window
        Me.LakeHgWaterColumn.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.LakeHgWaterColumn.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LakeHgWaterColumn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LakeHgWaterColumn.Location = New System.Drawing.Point(338, 19)
        Me.LakeHgWaterColumn.MaxLength = 0
        Me.LakeHgWaterColumn.Name = "LakeHgWaterColumn"
        Me.LakeHgWaterColumn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LakeHgWaterColumn.Size = New System.Drawing.Size(87, 20)
        Me.LakeHgWaterColumn.TabIndex = 1
        Me.LakeHgWaterColumn.Text = "1"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(42, 238)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(207, 14)
        Me.Label21.TabIndex = 18
        Me.Label21.Text = "Biomass-Water Partition Coefficient, Kbio:"
        '
        'HgWaterAlphaW
        '
        Me.HgWaterAlphaW.AcceptsReturn = True
        Me.HgWaterAlphaW.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterAlphaW.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterAlphaW.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterAlphaW.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterAlphaW.Location = New System.Drawing.Point(338, 43)
        Me.HgWaterAlphaW.MaxLength = 0
        Me.HgWaterAlphaW.Name = "HgWaterAlphaW"
        Me.HgWaterAlphaW.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterAlphaW.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterAlphaW.TabIndex = 3
        Me.HgWaterAlphaW.Text = "1"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(42, 214)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(220, 14)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "Sediment Water Partition Coefficient, Kd sw:"
        '
        'HgWaterKWR
        '
        Me.HgWaterKWR.AcceptsReturn = True
        Me.HgWaterKWR.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterKWR.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterKWR.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterKWR.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterKWR.Location = New System.Drawing.Point(338, 67)
        Me.HgWaterKWR.MaxLength = 0
        Me.HgWaterKWR.Name = "HgWaterKWR"
        Me.HgWaterKWR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterKWR.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterKWR.TabIndex = 5
        Me.HgWaterKWR.Text = "0.075"
        '
        'HgWaterBetaW
        '
        Me.HgWaterBetaW.AcceptsReturn = True
        Me.HgWaterBetaW.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterBetaW.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterBetaW.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterBetaW.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterBetaW.Location = New System.Drawing.Point(338, 91)
        Me.HgWaterBetaW.MaxLength = 0
        Me.HgWaterBetaW.Name = "HgWaterBetaW"
        Me.HgWaterBetaW.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterBetaW.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterBetaW.TabIndex = 7
        Me.HgWaterBetaW.Text = "0.4"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(42, 190)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(222, 14)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Porewater Diffusion Coefficient, Esw (m²/s):"
        '
        'HgWaterKWM
        '
        Me.HgWaterKWM.AcceptsReturn = True
        Me.HgWaterKWM.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterKWM.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterKWM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterKWM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterKWM.Location = New System.Drawing.Point(338, 115)
        Me.HgWaterKWM.MaxLength = 0
        Me.HgWaterKWM.Name = "HgWaterKWM"
        Me.HgWaterKWM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterKWM.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterKWM.TabIndex = 9
        Me.HgWaterKWM.Text = "0.001"
        '
        'HgWaterVSB
        '
        Me.HgWaterVSB.AcceptsReturn = True
        Me.HgWaterVSB.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterVSB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterVSB.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterVSB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterVSB.Location = New System.Drawing.Point(338, 139)
        Me.HgWaterVSB.MaxLength = 0
        Me.HgWaterVSB.Name = "HgWaterVSB"
        Me.HgWaterVSB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterVSB.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterVSB.TabIndex = 11
        Me.HgWaterVSB.Text = "0.2"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(42, 118)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(292, 14)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Mercury Methylation Rate in Water Column, Kwm (per day):"
        '
        'HgWaterVRS
        '
        Me.HgWaterVRS.AcceptsReturn = True
        Me.HgWaterVRS.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterVRS.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterVRS.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterVRS.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterVRS.Location = New System.Drawing.Point(338, 163)
        Me.HgWaterVRS.MaxLength = 0
        Me.HgWaterVRS.Name = "HgWaterVRS"
        Me.HgWaterVRS.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterVRS.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterVRS.TabIndex = 13
        Me.HgWaterVRS.Text = "0.00001"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(42, 46)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(250, 14)
        Me.Label18.TabIndex = 2
        Me.Label18.Text = "Net Reduction Loss Factor in Water Column, ? wc:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(42, 94)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(256, 14)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Net Methylation Loss Factor in Water Column, ? wc:"
        '
        'HgWaterKDsw
        '
        Me.HgWaterKDsw.AcceptsReturn = True
        Me.HgWaterKDsw.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterKDsw.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterKDsw.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterKDsw.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterKDsw.Location = New System.Drawing.Point(338, 211)
        Me.HgWaterKDsw.MaxLength = 0
        Me.HgWaterKDsw.Name = "HgWaterKDsw"
        Me.HgWaterKDsw.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterKDsw.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterKDsw.TabIndex = 17
        Me.HgWaterKDsw.Text = "100000"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.SystemColors.Control
        Me.Label20.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label20.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label20.Location = New System.Drawing.Point(42, 142)
        Me.Label20.Name = "Label20"
        Me.Label20.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label20.Size = New System.Drawing.Size(197, 14)
        Me.Label20.TabIndex = 10
        Me.Label20.Text = "Biomass Settling Velocity, Vsb (m/day):"
        '
        'HgWaterKbio
        '
        Me.HgWaterKbio.AcceptsReturn = True
        Me.HgWaterKbio.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterKbio.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterKbio.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterKbio.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterKbio.Location = New System.Drawing.Point(338, 235)
        Me.HgWaterKbio.MaxLength = 0
        Me.HgWaterKbio.Name = "HgWaterKbio"
        Me.HgWaterKbio.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterKbio.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterKbio.TabIndex = 19
        Me.HgWaterKbio.Text = "200000"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(42, 70)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(282, 14)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Mercury Reduction Rate in Water Column, Kwr (per day):"
        '
        'HgWaterCbio
        '
        Me.HgWaterCbio.AcceptsReturn = True
        Me.HgWaterCbio.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterCbio.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterCbio.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterCbio.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterCbio.Location = New System.Drawing.Point(338, 259)
        Me.HgWaterCbio.MaxLength = 0
        Me.HgWaterCbio.Name = "HgWaterCbio"
        Me.HgWaterCbio.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterCbio.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterCbio.TabIndex = 21
        Me.HgWaterCbio.Text = "0.7"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.SystemColors.Control
        Me.Label19.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label19.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label19.Location = New System.Drawing.Point(42, 166)
        Me.Label19.Name = "Label19"
        Me.Label19.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label19.Size = New System.Drawing.Size(236, 14)
        Me.Label19.TabIndex = 12
        Me.Label19.Text = "Sediment Re-suspension Velocity, Vrs (m/day):"
        '
        'HgWaterHgDecayInChannel
        '
        Me.HgWaterHgDecayInChannel.AcceptsReturn = True
        Me.HgWaterHgDecayInChannel.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterHgDecayInChannel.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterHgDecayInChannel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterHgDecayInChannel.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterHgDecayInChannel.Location = New System.Drawing.Point(338, 283)
        Me.HgWaterHgDecayInChannel.MaxLength = 0
        Me.HgWaterHgDecayInChannel.Name = "HgWaterHgDecayInChannel"
        Me.HgWaterHgDecayInChannel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterHgDecayInChannel.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterHgDecayInChannel.TabIndex = 23
        Me.HgWaterHgDecayInChannel.Text = "0.04"
        '
        'HgMethylHgFraction
        '
        Me.HgMethylHgFraction.AcceptsReturn = True
        Me.HgMethylHgFraction.BackColor = System.Drawing.SystemColors.Window
        Me.HgMethylHgFraction.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgMethylHgFraction.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgMethylHgFraction.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgMethylHgFraction.Location = New System.Drawing.Point(338, 307)
        Me.HgMethylHgFraction.MaxLength = 0
        Me.HgMethylHgFraction.Name = "HgMethylHgFraction"
        Me.HgMethylHgFraction.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgMethylHgFraction.Size = New System.Drawing.Size(87, 20)
        Me.HgMethylHgFraction.TabIndex = 25
        Me.HgMethylHgFraction.Text = "0.06"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(42, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(228, 14)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Initial Hg Concentration in Water Column (ng/l):"
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.BackColor = System.Drawing.SystemColors.Control
        Me.Label120.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label120.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label120.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label120.Location = New System.Drawing.Point(42, 286)
        Me.Label120.Name = "Label120"
        Me.Label120.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label120.Size = New System.Drawing.Size(260, 14)
        Me.Label120.TabIndex = 22
        Me.Label120.Text = "Mercury Decay Rate in Channel Transport (per day):"
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.BackColor = System.Drawing.SystemColors.Control
        Me.Label121.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label121.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label121.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label121.Location = New System.Drawing.Point(42, 310)
        Me.Label121.Name = "Label121"
        Me.Label121.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label121.Size = New System.Drawing.Size(219, 14)
        Me.Label121.TabIndex = 24
        Me.Label121.Text = "Fraction of Methyl Mercury in Total Mercury:"
        '
        '_SSTab1_TabPage4
        '
        Me._SSTab1_TabPage4.Controls.Add(Me.Frame11)
        Me._SSTab1_TabPage4.Location = New System.Drawing.Point(4, 23)
        Me._SSTab1_TabPage4.Name = "_SSTab1_TabPage4"
        Me._SSTab1_TabPage4.Size = New System.Drawing.Size(513, 392)
        Me._SSTab1_TabPage4.TabIndex = 4
        Me._SSTab1_TabPage4.Text = "Benthic"
        '
        'Frame11
        '
        Me.Frame11.BackColor = System.Drawing.SystemColors.Control
        Me.Frame11.Controls.Add(Me.Label8)
        Me.Frame11.Controls.Add(Me.Label7)
        Me.Frame11.Controls.Add(Me.Label6)
        Me.Frame11.Controls.Add(Me.Label5)
        Me.Frame11.Controls.Add(Me.Label4)
        Me.Frame11.Controls.Add(Me.Label3)
        Me.Frame11.Controls.Add(Me.Label2)
        Me.Frame11.Controls.Add(Me.HgWaterTheta_bs)
        Me.Frame11.Controls.Add(Me.HgBenthicCbs)
        Me.Frame11.Controls.Add(Me.HgBenthicKDbs)
        Me.Frame11.Controls.Add(Me.HgBenthicSedimentDepth)
        Me.Frame11.Controls.Add(Me.HgBenthicVbur)
        Me.Frame11.Controls.Add(Me.HgBenthicKBM)
        Me.Frame11.Controls.Add(Me.HgBenthicBetaB)
        Me.Frame11.Controls.Add(Me.HgBenthicKBR)
        Me.Frame11.Controls.Add(Me.HgBenthicAlphaB)
        Me.Frame11.Controls.Add(Me.LakeHgBenthic)
        Me.Frame11.Controls.Add(Me.Label127)
        Me.Frame11.Controls.Add(Me.Label126)
        Me.Frame11.Controls.Add(Me.Label12)
        Me.Frame11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame11.Location = New System.Drawing.Point(24, 48)
        Me.Frame11.Name = "Frame11"
        Me.Frame11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame11.Size = New System.Drawing.Size(463, 280)
        Me.Frame11.TabIndex = 0
        Me.Frame11.TabStop = False
        Me.Frame11.Text = "Define Constants for Mercury (Benthic)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(16, 243)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(272, 14)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Solids Concentration in Benthic Sediments, C bs (g/m³):"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 219)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(316, 14)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Bed Sediment / Sediment Pore Water Partition Coefficient, Kd bs:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 195)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(184, 14)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Benthic Sediment Bed Porosity, ? bs:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 124)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(293, 14)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Benthic Sediment Mercury Methylation Rate, Kbm (per day):"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(268, 14)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Net Methylation Loss Factor in Benthic Sediment, ? bs:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(283, 14)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Benthic Sediment Mercury Reduction Rate, Kbr (per day):"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(268, 14)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Net Reduction Loss Factor in Benthic Sediments, ? bs:"
        '
        'HgWaterTheta_bs
        '
        Me.HgWaterTheta_bs.AcceptsReturn = True
        Me.HgWaterTheta_bs.BackColor = System.Drawing.SystemColors.Window
        Me.HgWaterTheta_bs.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgWaterTheta_bs.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgWaterTheta_bs.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgWaterTheta_bs.Location = New System.Drawing.Point(360, 192)
        Me.HgWaterTheta_bs.MaxLength = 0
        Me.HgWaterTheta_bs.Name = "HgWaterTheta_bs"
        Me.HgWaterTheta_bs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgWaterTheta_bs.Size = New System.Drawing.Size(87, 20)
        Me.HgWaterTheta_bs.TabIndex = 15
        Me.HgWaterTheta_bs.Text = "0.9"
        '
        'HgBenthicCbs
        '
        Me.HgBenthicCbs.AcceptsReturn = True
        Me.HgBenthicCbs.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicCbs.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicCbs.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicCbs.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicCbs.Location = New System.Drawing.Point(360, 240)
        Me.HgBenthicCbs.MaxLength = 0
        Me.HgBenthicCbs.Name = "HgBenthicCbs"
        Me.HgBenthicCbs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicCbs.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicCbs.TabIndex = 19
        Me.HgBenthicCbs.Text = "75000"
        '
        'HgBenthicKDbs
        '
        Me.HgBenthicKDbs.AcceptsReturn = True
        Me.HgBenthicKDbs.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicKDbs.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicKDbs.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicKDbs.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicKDbs.Location = New System.Drawing.Point(360, 216)
        Me.HgBenthicKDbs.MaxLength = 0
        Me.HgBenthicKDbs.Name = "HgBenthicKDbs"
        Me.HgBenthicKDbs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicKDbs.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicKDbs.TabIndex = 17
        Me.HgBenthicKDbs.Text = "50000"
        '
        'HgBenthicSedimentDepth
        '
        Me.HgBenthicSedimentDepth.AcceptsReturn = True
        Me.HgBenthicSedimentDepth.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicSedimentDepth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicSedimentDepth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicSedimentDepth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicSedimentDepth.Location = New System.Drawing.Point(360, 168)
        Me.HgBenthicSedimentDepth.MaxLength = 0
        Me.HgBenthicSedimentDepth.Name = "HgBenthicSedimentDepth"
        Me.HgBenthicSedimentDepth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicSedimentDepth.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicSedimentDepth.TabIndex = 13
        Me.HgBenthicSedimentDepth.Text = "0.02"
        '
        'HgBenthicVbur
        '
        Me.HgBenthicVbur.AcceptsReturn = True
        Me.HgBenthicVbur.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicVbur.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicVbur.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicVbur.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicVbur.Location = New System.Drawing.Point(360, 144)
        Me.HgBenthicVbur.MaxLength = 0
        Me.HgBenthicVbur.Name = "HgBenthicVbur"
        Me.HgBenthicVbur.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicVbur.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicVbur.TabIndex = 11
        Me.HgBenthicVbur.Text = "0.00000035"
        '
        'HgBenthicKBM
        '
        Me.HgBenthicKBM.AcceptsReturn = True
        Me.HgBenthicKBM.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicKBM.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicKBM.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicKBM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicKBM.Location = New System.Drawing.Point(360, 121)
        Me.HgBenthicKBM.MaxLength = 0
        Me.HgBenthicKBM.Name = "HgBenthicKBM"
        Me.HgBenthicKBM.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicKBM.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicKBM.TabIndex = 9
        Me.HgBenthicKBM.Text = "0.0001"
        '
        'HgBenthicBetaB
        '
        Me.HgBenthicBetaB.AcceptsReturn = True
        Me.HgBenthicBetaB.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicBetaB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicBetaB.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicBetaB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicBetaB.Location = New System.Drawing.Point(360, 97)
        Me.HgBenthicBetaB.MaxLength = 0
        Me.HgBenthicBetaB.Name = "HgBenthicBetaB"
        Me.HgBenthicBetaB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicBetaB.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicBetaB.TabIndex = 7
        Me.HgBenthicBetaB.Text = "0.4"
        '
        'HgBenthicKBR
        '
        Me.HgBenthicKBR.AcceptsReturn = True
        Me.HgBenthicKBR.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicKBR.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicKBR.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicKBR.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicKBR.Location = New System.Drawing.Point(360, 73)
        Me.HgBenthicKBR.MaxLength = 0
        Me.HgBenthicKBR.Name = "HgBenthicKBR"
        Me.HgBenthicKBR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicKBR.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicKBR.TabIndex = 5
        Me.HgBenthicKBR.Text = "0.000001"
        '
        'HgBenthicAlphaB
        '
        Me.HgBenthicAlphaB.AcceptsReturn = True
        Me.HgBenthicAlphaB.BackColor = System.Drawing.SystemColors.Window
        Me.HgBenthicAlphaB.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.HgBenthicAlphaB.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HgBenthicAlphaB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HgBenthicAlphaB.Location = New System.Drawing.Point(360, 49)
        Me.HgBenthicAlphaB.MaxLength = 0
        Me.HgBenthicAlphaB.Name = "HgBenthicAlphaB"
        Me.HgBenthicAlphaB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.HgBenthicAlphaB.Size = New System.Drawing.Size(87, 20)
        Me.HgBenthicAlphaB.TabIndex = 3
        Me.HgBenthicAlphaB.Text = "1"
        '
        'LakeHgBenthic
        '
        Me.LakeHgBenthic.AcceptsReturn = True
        Me.LakeHgBenthic.BackColor = System.Drawing.SystemColors.Window
        Me.LakeHgBenthic.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.LakeHgBenthic.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LakeHgBenthic.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LakeHgBenthic.Location = New System.Drawing.Point(360, 24)
        Me.LakeHgBenthic.MaxLength = 0
        Me.LakeHgBenthic.Name = "LakeHgBenthic"
        Me.LakeHgBenthic.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LakeHgBenthic.Size = New System.Drawing.Size(87, 20)
        Me.LakeHgBenthic.TabIndex = 0
        Me.LakeHgBenthic.Text = "30"
        '
        'Label127
        '
        Me.Label127.AutoSize = True
        Me.Label127.BackColor = System.Drawing.SystemColors.Control
        Me.Label127.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label127.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label127.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label127.Location = New System.Drawing.Point(16, 171)
        Me.Label127.Name = "Label127"
        Me.Label127.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label127.Size = New System.Drawing.Size(178, 14)
        Me.Label127.TabIndex = 12
        Me.Label127.Text = "Depth of Benthic Sediment Bed (m):"
        '
        'Label126
        '
        Me.Label126.AutoSize = True
        Me.Label126.BackColor = System.Drawing.SystemColors.Control
        Me.Label126.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label126.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label126.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label126.Location = New System.Drawing.Point(16, 147)
        Me.Label126.Name = "Label126"
        Me.Label126.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label126.Size = New System.Drawing.Size(149, 14)
        Me.Label126.TabIndex = 10
        Me.Label126.Text = "Burial Velocity, Vbur (m/day):"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(16, 27)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(248, 14)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Initial Hg Concentration in Benthic Sediment (ng/g):"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(476, 433)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(54, 24)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSave.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSave.Location = New System.Drawing.Point(416, 433)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSave.Size = New System.Drawing.Size(54, 24)
        Me.cmdSave.TabIndex = 1
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'frmMercury
        '
        Me.AcceptButton = Me.cmdSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(542, 469)
        Me.Controls.Add(Me.SSTab1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdSave)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(9, 28)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMercury"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Mercury Input"
        Me.SSTab1.ResumeLayout(False)
        Me._SSTab1_TabPage0.ResumeLayout(False)
        Me._SSTab1_TabPage0.PerformLayout()
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.Frame2.ResumeLayout(False)
        Me.Frame2.PerformLayout()
        Me._SSTab1_TabPage1.ResumeLayout(False)
        Me.MercuryLand.ResumeLayout(False)
        Me.MercuryLand.PerformLayout()
        Me._SSTab1_TabPage2.ResumeLayout(False)
        Me.Frame10.ResumeLayout(False)
        Me.Frame10.PerformLayout()
        Me._SSTab1_TabPage3.ResumeLayout(False)
        Me.MercuryWater.ResumeLayout(False)
        Me.MercuryWater.PerformLayout()
        Me._SSTab1_TabPage4.ResumeLayout(False)
        Me.Frame11.ResumeLayout(False)
        Me.Frame11.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Public WithEvents Label19 As System.Windows.Forms.Label
    Public WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Public WithEvents Label120 As System.Windows.Forms.Label
#End Region
End Class