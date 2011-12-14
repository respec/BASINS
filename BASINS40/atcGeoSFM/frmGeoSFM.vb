Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
Imports System.Windows.Forms
Imports System.Text
Imports System.IO

Public Class frmGeoSFM
    Inherits System.Windows.Forms.Form
    Dim pStationsRead As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents tpgRain As System.Windows.Forms.TabPage
    Friend WithEvents tpgBasin As System.Windows.Forms.TabPage
    Friend WithEvents lblProcessed As System.Windows.Forms.Label
    Friend WithEvents cboBC2 As System.Windows.Forms.ComboBox
    Friend WithEvents cboBC1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblGrid As System.Windows.Forms.Label
    Friend WithEvents lblRunoff As System.Windows.Forms.Label
    Friend WithEvents cboBC5 As System.Windows.Forms.ComboBox
    Friend WithEvents lblHillLen As System.Windows.Forms.Label
    Friend WithEvents cboBC4 As System.Windows.Forms.ComboBox
    Friend WithEvents lblFlowAcc As System.Windows.Forms.Label
    Friend WithEvents cboBC3 As System.Windows.Forms.ComboBox
    Friend WithEvents lblMaxImp As System.Windows.Forms.Label
    Friend WithEvents cboBC13 As System.Windows.Forms.ComboBox
    Friend WithEvents lblDownBasin As System.Windows.Forms.Label
    Friend WithEvents cboBC12 As System.Windows.Forms.ComboBox
    Friend WithEvents lblStreamLink As System.Windows.Forms.Label
    Friend WithEvents cboBC11 As System.Windows.Forms.ComboBox
    Friend WithEvents lblDownstream As System.Windows.Forms.Label
    Friend WithEvents cboBC10 As System.Windows.Forms.ComboBox
    Friend WithEvents lblHydraulic As System.Windows.Forms.Label
    Friend WithEvents cboBC9 As System.Windows.Forms.ComboBox
    Friend WithEvents lblTexture As System.Windows.Forms.Label
    Friend WithEvents cboBC8 As System.Windows.Forms.ComboBox
    Friend WithEvents lblSoil As System.Windows.Forms.Label
    Friend WithEvents cboBC7 As System.Windows.Forms.ComboBox
    Friend WithEvents lblWater As System.Windows.Forms.Label
    Friend WithEvents cboBC6 As System.Windows.Forms.ComboBox
    Friend WithEvents tpgResponse As System.Windows.Forms.TabPage
    Friend WithEvents lblMethod As System.Windows.Forms.Label
    Friend WithEvents rbnUniform As System.Windows.Forms.RadioButton
    Friend WithEvents rbnNonUniformUser As System.Windows.Forms.RadioButton
    Friend WithEvents rbnNonUniformUSGS As System.Windows.Forms.RadioButton
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ComboBox14 As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents ComboBox15 As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ComboBox16 As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents ComboBox17 As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents ComboBox18 As System.Windows.Forms.ComboBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents ComboBox19 As System.Windows.Forms.ComboBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents ComboBox20 As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents ComboBox21 As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents ComboBox22 As System.Windows.Forms.ComboBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents ComboBox23 As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents ComboBox24 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox25 As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents ComboBox26 As System.Windows.Forms.ComboBox
    Friend WithEvents cboUSGS As System.Windows.Forms.ComboBox
    Friend WithEvents lblInstream As System.Windows.Forms.Label
    Friend WithEvents lblOverland As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents ComboBox27 As System.Windows.Forms.ComboBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents ComboBox28 As System.Windows.Forms.ComboBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents ComboBox29 As System.Windows.Forms.ComboBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents ComboBox30 As System.Windows.Forms.ComboBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents ComboBox31 As System.Windows.Forms.ComboBox
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents ComboBox32 As System.Windows.Forms.ComboBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents ComboBox33 As System.Windows.Forms.ComboBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents ComboBox34 As System.Windows.Forms.ComboBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents ComboBox35 As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents ComboBox36 As System.Windows.Forms.ComboBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents ComboBox37 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox38 As System.Windows.Forms.ComboBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents ComboBox39 As System.Windows.Forms.ComboBox
    Friend WithEvents AtcGridMannings As atcControls.atcGrid
    Friend WithEvents tpgTerrain As System.Windows.Forms.TabPage
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents AtcText1 As atcControls.atcText
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents cboSubbasin As System.Windows.Forms.ComboBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents cboDEM As System.Windows.Forms.ComboBox
    Friend WithEvents gbxSimulationDates As System.Windows.Forms.GroupBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents atxEDay As atcControls.atcText
    Friend WithEvents atxSDay As atcControls.atcText
    Friend WithEvents atxSYear As atcControls.atcText
    Friend WithEvents atxEMonth As atcControls.atcText
    Friend WithEvents atxSMonth As atcControls.atcText
    Friend WithEvents atxEYear As atcControls.atcText
    Friend WithEvents tpgSoil As System.Windows.Forms.TabPage
    Friend WithEvents tpgFlow As System.Windows.Forms.TabPage
    Friend WithEvents tpgSensitivity As System.Windows.Forms.TabPage
    Friend WithEvents tpgCalibrate As System.Windows.Forms.TabPage
    Friend WithEvents cmdTerrainNext As System.Windows.Forms.Button
    Friend WithEvents cmdBasinNext As System.Windows.Forms.Button
    Friend WithEvents cmdResponseNext As System.Windows.Forms.Button
    Friend WithEvents cmdRainEvapNext As System.Windows.Forms.Button
    Friend WithEvents cmdBalanceNext As System.Windows.Forms.Button
    Friend WithEvents cmdRouteNext As System.Windows.Forms.Button
    Friend WithEvents cmdSensitivityNext As System.Windows.Forms.Button
    Friend WithEvents cmdCalibrationNext As System.Windows.Forms.Button
    Friend WithEvents cmdMapGenerate As System.Windows.Forms.Button
    Friend WithEvents atxInstream As atcControls.atcText
    Friend WithEvents atxOverland As atcControls.atcText
    Friend WithEvents lblOutlets As System.Windows.Forms.Label
    Friend WithEvents cboOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents lblFlow As System.Windows.Forms.Label
    Friend WithEvents cboFlowDir As System.Windows.Forms.ComboBox
    Friend WithEvents AtcGridPrec As atcControls.atcGrid
    Friend WithEvents lblInitial As System.Windows.Forms.Label
    Friend WithEvents gbxData As System.Windows.Forms.GroupBox
    Friend WithEvents rbnDaily As System.Windows.Forms.RadioButton
    Friend WithEvents rbnHourly As System.Windows.Forms.RadioButton
    Friend WithEvents gbxRun As System.Windows.Forms.GroupBox
    Friend WithEvents rbnRun As System.Windows.Forms.RadioButton
    Friend WithEvents rbnContinue As System.Windows.Forms.RadioButton
    Friend WithEvents gbxMode As System.Windows.Forms.GroupBox
    Friend WithEvents rbnCal As System.Windows.Forms.RadioButton
    Friend WithEvents rbnSim As System.Windows.Forms.RadioButton
    Friend WithEvents atxInitial As atcControls.atcText
    Friend WithEvents gbxFlow As System.Windows.Forms.GroupBox
    Friend WithEvents rbnNonlinear As System.Windows.Forms.RadioButton
    Friend WithEvents rbnLinear As System.Windows.Forms.RadioButton
    Friend WithEvents atxForecast As atcControls.atcText
    Friend WithEvents lflForecast As System.Windows.Forms.Label
    Friend WithEvents gbxStreamFlow As System.Windows.Forms.GroupBox
    Friend WithEvents rbnMusk As System.Windows.Forms.RadioButton
    Friend WithEvents rbnDiffusion As System.Windows.Forms.RadioButton
    Friend WithEvents rbnSimple As System.Windows.Forms.RadioButton
    Friend WithEvents lblReach As System.Windows.Forms.Label
    Friend WithEvents cboReachSensitivity As System.Windows.Forms.ComboBox
    Friend WithEvents AtcGridSensitivity As atcControls.atcGrid
    Friend WithEvents gbxConnect As System.Windows.Forms.GroupBox
    Friend WithEvents AtcConnectFlows As atcControls.atcConnectFields
    Friend WithEvents lblMax As System.Windows.Forms.Label
    Friend WithEvents lstMax As System.Windows.Forms.ListBox
    Friend WithEvents gbxObjective As System.Windows.Forms.GroupBox
    Friend WithEvents lblParms As System.Windows.Forms.Label
    Friend WithEvents lstCalib As System.Windows.Forms.ListBox
    Friend WithEvents rbnObj2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbnObj1 As System.Windows.Forms.RadioButton
    Friend WithEvents rbnObj6 As System.Windows.Forms.RadioButton
    Friend WithEvents rbnObj5 As System.Windows.Forms.RadioButton
    Friend WithEvents rbnObj4 As System.Windows.Forms.RadioButton
    Friend WithEvents rbnObj3 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdReadStreamflow As System.Windows.Forms.Button
    Friend WithEvents gbxMapper As System.Windows.Forms.GroupBox
    Friend WithEvents gbxMapDates As System.Windows.Forms.GroupBox
    Friend WithEvents lblMapDay As System.Windows.Forms.Label
    Friend WithEvents lblMapMonth As System.Windows.Forms.Label
    Friend WithEvents lblMapYear As System.Windows.Forms.Label
    Friend WithEvents AtxMapDay As atcControls.atcText
    Friend WithEvents atxMapYear As atcControls.atcText
    Friend WithEvents AtxMapMonth As atcControls.atcText
    Friend WithEvents gbxMap As System.Windows.Forms.GroupBox
    Friend WithEvents rbnSoil As System.Windows.Forms.RadioButton
    Friend WithEvents rbnStreamflow As System.Windows.Forms.RadioButton
    Friend WithEvents cmdBankfullNext As System.Windows.Forms.Button
    Friend WithEvents gbxBankfull As System.Windows.Forms.GroupBox
    Friend WithEvents gbxHydrographs As System.Windows.Forms.GroupBox
    Friend WithEvents lblRchHydro As System.Windows.Forms.Label
    Friend WithEvents cboRchHydro As System.Windows.Forms.ComboBox
    Friend WithEvents LabelModel As System.Windows.Forms.Label
    Friend WithEvents atxModel As atcControls.atcText
    Friend WithEvents tpgMap As System.Windows.Forms.TabPage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGeoSFM))
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tpgTerrain = New System.Windows.Forms.TabPage
        Me.LabelModel = New System.Windows.Forms.Label
        Me.atxModel = New atcControls.atcText
        Me.cmdTerrainNext = New System.Windows.Forms.Button
        Me.Label53 = New System.Windows.Forms.Label
        Me.AtcText1 = New atcControls.atcText
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.Label52 = New System.Windows.Forms.Label
        Me.cboSubbasin = New System.Windows.Forms.ComboBox
        Me.Label50 = New System.Windows.Forms.Label
        Me.Label51 = New System.Windows.Forms.Label
        Me.cboDEM = New System.Windows.Forms.ComboBox
        Me.tpgBasin = New System.Windows.Forms.TabPage
        Me.cmdBasinNext = New System.Windows.Forms.Button
        Me.lblMaxImp = New System.Windows.Forms.Label
        Me.cboBC13 = New System.Windows.Forms.ComboBox
        Me.lblDownBasin = New System.Windows.Forms.Label
        Me.cboBC12 = New System.Windows.Forms.ComboBox
        Me.lblStreamLink = New System.Windows.Forms.Label
        Me.cboBC11 = New System.Windows.Forms.ComboBox
        Me.lblDownstream = New System.Windows.Forms.Label
        Me.cboBC10 = New System.Windows.Forms.ComboBox
        Me.lblHydraulic = New System.Windows.Forms.Label
        Me.cboBC9 = New System.Windows.Forms.ComboBox
        Me.lblTexture = New System.Windows.Forms.Label
        Me.cboBC8 = New System.Windows.Forms.ComboBox
        Me.lblSoil = New System.Windows.Forms.Label
        Me.cboBC7 = New System.Windows.Forms.ComboBox
        Me.lblWater = New System.Windows.Forms.Label
        Me.cboBC6 = New System.Windows.Forms.ComboBox
        Me.lblRunoff = New System.Windows.Forms.Label
        Me.cboBC5 = New System.Windows.Forms.ComboBox
        Me.lblHillLen = New System.Windows.Forms.Label
        Me.cboBC4 = New System.Windows.Forms.ComboBox
        Me.lblFlowAcc = New System.Windows.Forms.Label
        Me.cboBC3 = New System.Windows.Forms.ComboBox
        Me.cboBC1 = New System.Windows.Forms.ComboBox
        Me.lblGrid = New System.Windows.Forms.Label
        Me.lblProcessed = New System.Windows.Forms.Label
        Me.cboBC2 = New System.Windows.Forms.ComboBox
        Me.tpgResponse = New System.Windows.Forms.TabPage
        Me.lblFlow = New System.Windows.Forms.Label
        Me.cboFlowDir = New System.Windows.Forms.ComboBox
        Me.lblOutlets = New System.Windows.Forms.Label
        Me.cboOutlets = New System.Windows.Forms.ComboBox
        Me.atxInstream = New atcControls.atcText
        Me.atxOverland = New atcControls.atcText
        Me.cmdResponseNext = New System.Windows.Forms.Button
        Me.AtcGridMannings = New atcControls.atcGrid
        Me.cboUSGS = New System.Windows.Forms.ComboBox
        Me.lblInstream = New System.Windows.Forms.Label
        Me.lblOverland = New System.Windows.Forms.Label
        Me.rbnUniform = New System.Windows.Forms.RadioButton
        Me.rbnNonUniformUser = New System.Windows.Forms.RadioButton
        Me.rbnNonUniformUSGS = New System.Windows.Forms.RadioButton
        Me.lblMethod = New System.Windows.Forms.Label
        Me.tpgRain = New System.Windows.Forms.TabPage
        Me.AtcGridPrec = New atcControls.atcGrid
        Me.cmdRainEvapNext = New System.Windows.Forms.Button
        Me.gbxSimulationDates = New System.Windows.Forms.GroupBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label58 = New System.Windows.Forms.Label
        Me.atxEDay = New atcControls.atcText
        Me.atxSDay = New atcControls.atcText
        Me.atxSYear = New atcControls.atcText
        Me.atxEMonth = New atcControls.atcText
        Me.atxSMonth = New atcControls.atcText
        Me.atxEYear = New atcControls.atcText
        Me.tpgSoil = New System.Windows.Forms.TabPage
        Me.gbxFlow = New System.Windows.Forms.GroupBox
        Me.rbnNonlinear = New System.Windows.Forms.RadioButton
        Me.rbnLinear = New System.Windows.Forms.RadioButton
        Me.atxInitial = New atcControls.atcText
        Me.gbxMode = New System.Windows.Forms.GroupBox
        Me.rbnCal = New System.Windows.Forms.RadioButton
        Me.rbnSim = New System.Windows.Forms.RadioButton
        Me.gbxRun = New System.Windows.Forms.GroupBox
        Me.rbnContinue = New System.Windows.Forms.RadioButton
        Me.rbnRun = New System.Windows.Forms.RadioButton
        Me.gbxData = New System.Windows.Forms.GroupBox
        Me.rbnDaily = New System.Windows.Forms.RadioButton
        Me.rbnHourly = New System.Windows.Forms.RadioButton
        Me.lblInitial = New System.Windows.Forms.Label
        Me.cmdBalanceNext = New System.Windows.Forms.Button
        Me.tpgFlow = New System.Windows.Forms.TabPage
        Me.gbxStreamFlow = New System.Windows.Forms.GroupBox
        Me.rbnSimple = New System.Windows.Forms.RadioButton
        Me.rbnMusk = New System.Windows.Forms.RadioButton
        Me.rbnDiffusion = New System.Windows.Forms.RadioButton
        Me.atxForecast = New atcControls.atcText
        Me.lflForecast = New System.Windows.Forms.Label
        Me.cmdRouteNext = New System.Windows.Forms.Button
        Me.tpgSensitivity = New System.Windows.Forms.TabPage
        Me.AtcGridSensitivity = New atcControls.atcGrid
        Me.lblReach = New System.Windows.Forms.Label
        Me.cboReachSensitivity = New System.Windows.Forms.ComboBox
        Me.cmdSensitivityNext = New System.Windows.Forms.Button
        Me.tpgCalibrate = New System.Windows.Forms.TabPage
        Me.lblMax = New System.Windows.Forms.Label
        Me.lstMax = New System.Windows.Forms.ListBox
        Me.gbxObjective = New System.Windows.Forms.GroupBox
        Me.rbnObj6 = New System.Windows.Forms.RadioButton
        Me.rbnObj5 = New System.Windows.Forms.RadioButton
        Me.rbnObj4 = New System.Windows.Forms.RadioButton
        Me.rbnObj3 = New System.Windows.Forms.RadioButton
        Me.rbnObj2 = New System.Windows.Forms.RadioButton
        Me.rbnObj1 = New System.Windows.Forms.RadioButton
        Me.lblParms = New System.Windows.Forms.Label
        Me.lstCalib = New System.Windows.Forms.ListBox
        Me.gbxConnect = New System.Windows.Forms.GroupBox
        Me.AtcConnectFlows = New atcControls.atcConnectFields
        Me.cmdCalibrationNext = New System.Windows.Forms.Button
        Me.tpgMap = New System.Windows.Forms.TabPage
        Me.gbxHydrographs = New System.Windows.Forms.GroupBox
        Me.lblRchHydro = New System.Windows.Forms.Label
        Me.cboRchHydro = New System.Windows.Forms.ComboBox
        Me.cmdReadStreamflow = New System.Windows.Forms.Button
        Me.gbxBankfull = New System.Windows.Forms.GroupBox
        Me.cmdBankfullNext = New System.Windows.Forms.Button
        Me.gbxMapper = New System.Windows.Forms.GroupBox
        Me.gbxMapDates = New System.Windows.Forms.GroupBox
        Me.lblMapDay = New System.Windows.Forms.Label
        Me.lblMapMonth = New System.Windows.Forms.Label
        Me.lblMapYear = New System.Windows.Forms.Label
        Me.AtxMapDay = New atcControls.atcText
        Me.atxMapYear = New atcControls.atcText
        Me.AtxMapMonth = New atcControls.atcText
        Me.gbxMap = New System.Windows.Forms.GroupBox
        Me.rbnSoil = New System.Windows.Forms.RadioButton
        Me.rbnStreamflow = New System.Windows.Forms.RadioButton
        Me.cmdMapGenerate = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.ComboBox14 = New System.Windows.Forms.ComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.ComboBox15 = New System.Windows.Forms.ComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.ComboBox16 = New System.Windows.Forms.ComboBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.ComboBox17 = New System.Windows.Forms.ComboBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.ComboBox18 = New System.Windows.Forms.ComboBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.ComboBox19 = New System.Windows.Forms.ComboBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.ComboBox20 = New System.Windows.Forms.ComboBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.ComboBox21 = New System.Windows.Forms.ComboBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.ComboBox22 = New System.Windows.Forms.ComboBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.ComboBox23 = New System.Windows.Forms.ComboBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.ComboBox24 = New System.Windows.Forms.ComboBox
        Me.ComboBox25 = New System.Windows.Forms.ComboBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.ComboBox26 = New System.Windows.Forms.ComboBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.ComboBox27 = New System.Windows.Forms.ComboBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.ComboBox28 = New System.Windows.Forms.ComboBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.ComboBox29 = New System.Windows.Forms.ComboBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.ComboBox30 = New System.Windows.Forms.ComboBox
        Me.Label37 = New System.Windows.Forms.Label
        Me.ComboBox31 = New System.Windows.Forms.ComboBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.ComboBox32 = New System.Windows.Forms.ComboBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.ComboBox33 = New System.Windows.Forms.ComboBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.ComboBox34 = New System.Windows.Forms.ComboBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.ComboBox35 = New System.Windows.Forms.ComboBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.ComboBox36 = New System.Windows.Forms.ComboBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.ComboBox37 = New System.Windows.Forms.ComboBox
        Me.ComboBox38 = New System.Windows.Forms.ComboBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.ComboBox39 = New System.Windows.Forms.ComboBox
        Me.tabMain.SuspendLayout()
        Me.tpgTerrain.SuspendLayout()
        Me.tpgBasin.SuspendLayout()
        Me.tpgResponse.SuspendLayout()
        Me.tpgRain.SuspendLayout()
        Me.gbxSimulationDates.SuspendLayout()
        Me.tpgSoil.SuspendLayout()
        Me.gbxFlow.SuspendLayout()
        Me.gbxMode.SuspendLayout()
        Me.gbxRun.SuspendLayout()
        Me.gbxData.SuspendLayout()
        Me.tpgFlow.SuspendLayout()
        Me.gbxStreamFlow.SuspendLayout()
        Me.tpgSensitivity.SuspendLayout()
        Me.tpgCalibrate.SuspendLayout()
        Me.gbxObjective.SuspendLayout()
        Me.gbxConnect.SuspendLayout()
        Me.tpgMap.SuspendLayout()
        Me.gbxHydrographs.SuspendLayout()
        Me.gbxBankfull.SuspendLayout()
        Me.gbxMapper.SuspendLayout()
        Me.gbxMapDates.SuspendLayout()
        Me.gbxMap.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(449, 520)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(73, 28)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Close"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(528, 520)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(65, 28)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(599, 520)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(72, 28)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tpgTerrain)
        Me.tabMain.Controls.Add(Me.tpgBasin)
        Me.tabMain.Controls.Add(Me.tpgResponse)
        Me.tabMain.Controls.Add(Me.tpgRain)
        Me.tabMain.Controls.Add(Me.tpgSoil)
        Me.tabMain.Controls.Add(Me.tpgFlow)
        Me.tabMain.Controls.Add(Me.tpgSensitivity)
        Me.tabMain.Controls.Add(Me.tpgCalibrate)
        Me.tabMain.Controls.Add(Me.tpgMap)
        Me.tabMain.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabMain.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabMain.Location = New System.Drawing.Point(15, 15)
        Me.tabMain.Multiline = True
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(655, 437)
        Me.tabMain.TabIndex = 8
        '
        'tpgTerrain
        '
        Me.tpgTerrain.Controls.Add(Me.LabelModel)
        Me.tpgTerrain.Controls.Add(Me.atxModel)
        Me.tpgTerrain.Controls.Add(Me.cmdTerrainNext)
        Me.tpgTerrain.Controls.Add(Me.Label53)
        Me.tpgTerrain.Controls.Add(Me.AtcText1)
        Me.tpgTerrain.Controls.Add(Me.cboReach)
        Me.tpgTerrain.Controls.Add(Me.Label52)
        Me.tpgTerrain.Controls.Add(Me.cboSubbasin)
        Me.tpgTerrain.Controls.Add(Me.Label50)
        Me.tpgTerrain.Controls.Add(Me.Label51)
        Me.tpgTerrain.Controls.Add(Me.cboDEM)
        Me.tpgTerrain.Location = New System.Drawing.Point(4, 46)
        Me.tpgTerrain.Name = "tpgTerrain"
        Me.tpgTerrain.Size = New System.Drawing.Size(647, 387)
        Me.tpgTerrain.TabIndex = 10
        Me.tpgTerrain.Text = "Terrain Analysis"
        Me.tpgTerrain.UseVisualStyleBackColor = True
        '
        'LabelModel
        '
        Me.LabelModel.AutoSize = True
        Me.LabelModel.Location = New System.Drawing.Point(36, 30)
        Me.LabelModel.Name = "LabelModel"
        Me.LabelModel.Size = New System.Drawing.Size(119, 13)
        Me.LabelModel.TabIndex = 41
        Me.LabelModel.Text = "GeoSFM Project Name:"
        '
        'atxModel
        '
        Me.atxModel.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxModel.DataType = atcControls.atcText.ATCoDataType.ATCoTxt
        Me.atxModel.DefaultValue = "GeoSFM"
        Me.atxModel.HardMax = -999
        Me.atxModel.HardMin = -999
        Me.atxModel.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxModel.Location = New System.Drawing.Point(161, 27)
        Me.atxModel.MaxWidth = 20
        Me.atxModel.Name = "atxModel"
        Me.atxModel.NumericFormat = "0"
        Me.atxModel.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxModel.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxModel.SelLength = 0
        Me.atxModel.SelStart = 0
        Me.atxModel.Size = New System.Drawing.Size(121, 21)
        Me.atxModel.SoftMax = -999
        Me.atxModel.SoftMin = -999
        Me.atxModel.TabIndex = 40
        Me.atxModel.ValueDouble = 0
        Me.atxModel.ValueInteger = 0
        '
        'cmdTerrainNext
        '
        Me.cmdTerrainNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdTerrainNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdTerrainNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTerrainNext.Location = New System.Drawing.Point(555, 341)
        Me.cmdTerrainNext.Name = "cmdTerrainNext"
        Me.cmdTerrainNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdTerrainNext.TabIndex = 39
        Me.cmdTerrainNext.Text = "Next >"
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(36, 178)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(149, 13)
        Me.Label53.TabIndex = 38
        Me.Label53.Text = "Stream Delineation Threshold:"
        '
        'AtcText1
        '
        Me.AtcText1.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcText1.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcText1.DefaultValue = ""
        Me.AtcText1.HardMax = 9999
        Me.AtcText1.HardMin = 0
        Me.AtcText1.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcText1.Location = New System.Drawing.Point(192, 175)
        Me.AtcText1.MaxWidth = 20
        Me.AtcText1.Name = "AtcText1"
        Me.AtcText1.NumericFormat = "0"
        Me.AtcText1.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcText1.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcText1.SelLength = 0
        Me.AtcText1.SelStart = 0
        Me.AtcText1.Size = New System.Drawing.Size(64, 21)
        Me.AtcText1.SoftMax = -999
        Me.AtcText1.SoftMin = -999
        Me.AtcText1.TabIndex = 37
        Me.AtcText1.ValueDouble = 1000
        Me.AtcText1.ValueInteger = 1000
        '
        'cboReach
        '
        Me.cboReach.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReach.Location = New System.Drawing.Point(124, 97)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(403, 21)
        Me.cboReach.TabIndex = 31
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.Location = New System.Drawing.Point(36, 100)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(82, 13)
        Me.Label52.TabIndex = 30
        Me.Label52.Text = "River Shapefile:"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboSubbasin
        '
        Me.cboSubbasin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSubbasin.Location = New System.Drawing.Point(124, 70)
        Me.cboSubbasin.Name = "cboSubbasin"
        Me.cboSubbasin.Size = New System.Drawing.Size(403, 21)
        Me.cboSubbasin.TabIndex = 29
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(36, 73)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(83, 13)
        Me.Label50.TabIndex = 28
        Me.Label50.Text = "Basin Shapefile:"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(36, 127)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(56, 13)
        Me.Label51.TabIndex = 26
        Me.Label51.Text = "DEM Grid:"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboDEM
        '
        Me.cboDEM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDEM.Location = New System.Drawing.Point(124, 124)
        Me.cboDEM.Name = "cboDEM"
        Me.cboDEM.Size = New System.Drawing.Size(403, 21)
        Me.cboDEM.TabIndex = 27
        '
        'tpgBasin
        '
        Me.tpgBasin.Controls.Add(Me.cmdBasinNext)
        Me.tpgBasin.Controls.Add(Me.lblMaxImp)
        Me.tpgBasin.Controls.Add(Me.cboBC13)
        Me.tpgBasin.Controls.Add(Me.lblDownBasin)
        Me.tpgBasin.Controls.Add(Me.cboBC12)
        Me.tpgBasin.Controls.Add(Me.lblStreamLink)
        Me.tpgBasin.Controls.Add(Me.cboBC11)
        Me.tpgBasin.Controls.Add(Me.lblDownstream)
        Me.tpgBasin.Controls.Add(Me.cboBC10)
        Me.tpgBasin.Controls.Add(Me.lblHydraulic)
        Me.tpgBasin.Controls.Add(Me.cboBC9)
        Me.tpgBasin.Controls.Add(Me.lblTexture)
        Me.tpgBasin.Controls.Add(Me.cboBC8)
        Me.tpgBasin.Controls.Add(Me.lblSoil)
        Me.tpgBasin.Controls.Add(Me.cboBC7)
        Me.tpgBasin.Controls.Add(Me.lblWater)
        Me.tpgBasin.Controls.Add(Me.cboBC6)
        Me.tpgBasin.Controls.Add(Me.lblRunoff)
        Me.tpgBasin.Controls.Add(Me.cboBC5)
        Me.tpgBasin.Controls.Add(Me.lblHillLen)
        Me.tpgBasin.Controls.Add(Me.cboBC4)
        Me.tpgBasin.Controls.Add(Me.lblFlowAcc)
        Me.tpgBasin.Controls.Add(Me.cboBC3)
        Me.tpgBasin.Controls.Add(Me.cboBC1)
        Me.tpgBasin.Controls.Add(Me.lblGrid)
        Me.tpgBasin.Controls.Add(Me.lblProcessed)
        Me.tpgBasin.Controls.Add(Me.cboBC2)
        Me.tpgBasin.Location = New System.Drawing.Point(4, 46)
        Me.tpgBasin.Name = "tpgBasin"
        Me.tpgBasin.Size = New System.Drawing.Size(647, 387)
        Me.tpgBasin.TabIndex = 8
        Me.tpgBasin.Text = "Basin Characteristics"
        Me.tpgBasin.UseVisualStyleBackColor = True
        '
        'cmdBasinNext
        '
        Me.cmdBasinNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBasinNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdBasinNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBasinNext.Location = New System.Drawing.Point(558, 345)
        Me.cmdBasinNext.Name = "cmdBasinNext"
        Me.cmdBasinNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdBasinNext.TabIndex = 48
        Me.cmdBasinNext.Text = "Next >"
        '
        'lblMaxImp
        '
        Me.lblMaxImp.AutoSize = True
        Me.lblMaxImp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxImp.Location = New System.Drawing.Point(27, 362)
        Me.lblMaxImp.Name = "lblMaxImp"
        Me.lblMaxImp.Size = New System.Drawing.Size(137, 13)
        Me.lblMaxImp.TabIndex = 46
        Me.lblMaxImp.Text = "Max Impervious Cover Grid:"
        Me.lblMaxImp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC13
        '
        Me.cboBC13.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC13.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC13.Location = New System.Drawing.Point(183, 359)
        Me.cboBC13.Name = "cboBC13"
        Me.cboBC13.Size = New System.Drawing.Size(351, 21)
        Me.cboBC13.TabIndex = 47
        '
        'lblDownBasin
        '
        Me.lblDownBasin.AutoSize = True
        Me.lblDownBasin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDownBasin.Location = New System.Drawing.Point(27, 335)
        Me.lblDownBasin.Name = "lblDownBasin"
        Me.lblDownBasin.Size = New System.Drawing.Size(134, 13)
        Me.lblDownBasin.TabIndex = 44
        Me.lblDownBasin.Text = "Downstream Basin ID Grid:"
        Me.lblDownBasin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC12
        '
        Me.cboBC12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC12.Location = New System.Drawing.Point(183, 333)
        Me.cboBC12.Name = "cboBC12"
        Me.cboBC12.Size = New System.Drawing.Size(351, 21)
        Me.cboBC12.TabIndex = 45
        '
        'lblStreamLink
        '
        Me.lblStreamLink.AutoSize = True
        Me.lblStreamLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStreamLink.Location = New System.Drawing.Point(27, 309)
        Me.lblStreamLink.Name = "lblStreamLink"
        Me.lblStreamLink.Size = New System.Drawing.Size(88, 13)
        Me.lblStreamLink.TabIndex = 42
        Me.lblStreamLink.Text = "Stream Link Grid:"
        Me.lblStreamLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC11
        '
        Me.cboBC11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC11.Location = New System.Drawing.Point(183, 306)
        Me.cboBC11.Name = "cboBC11"
        Me.cboBC11.Size = New System.Drawing.Size(351, 21)
        Me.cboBC11.TabIndex = 43
        '
        'lblDownstream
        '
        Me.lblDownstream.AutoSize = True
        Me.lblDownstream.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDownstream.Location = New System.Drawing.Point(27, 282)
        Me.lblDownstream.Name = "lblDownstream"
        Me.lblDownstream.Size = New System.Drawing.Size(152, 13)
        Me.lblDownstream.TabIndex = 40
        Me.lblDownstream.Text = "Downstream Flow Length Grid:"
        Me.lblDownstream.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC10
        '
        Me.cboBC10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC10.Location = New System.Drawing.Point(183, 279)
        Me.cboBC10.Name = "cboBC10"
        Me.cboBC10.Size = New System.Drawing.Size(351, 21)
        Me.cboBC10.TabIndex = 41
        '
        'lblHydraulic
        '
        Me.lblHydraulic.AutoSize = True
        Me.lblHydraulic.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHydraulic.Location = New System.Drawing.Point(27, 255)
        Me.lblHydraulic.Name = "lblHydraulic"
        Me.lblHydraulic.Size = New System.Drawing.Size(137, 13)
        Me.lblHydraulic.TabIndex = 38
        Me.lblHydraulic.Text = "Hydraulic Conductivity Grid:"
        Me.lblHydraulic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC9
        '
        Me.cboBC9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC9.Location = New System.Drawing.Point(183, 252)
        Me.cboBC9.Name = "cboBC9"
        Me.cboBC9.Size = New System.Drawing.Size(351, 21)
        Me.cboBC9.TabIndex = 39
        '
        'lblTexture
        '
        Me.lblTexture.AutoSize = True
        Me.lblTexture.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTexture.Location = New System.Drawing.Point(27, 228)
        Me.lblTexture.Name = "lblTexture"
        Me.lblTexture.Size = New System.Drawing.Size(88, 13)
        Me.lblTexture.TabIndex = 36
        Me.lblTexture.Text = "Soil Texture Grid:"
        Me.lblTexture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC8
        '
        Me.cboBC8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC8.Location = New System.Drawing.Point(183, 225)
        Me.cboBC8.Name = "cboBC8"
        Me.cboBC8.Size = New System.Drawing.Size(351, 21)
        Me.cboBC8.TabIndex = 37
        '
        'lblSoil
        '
        Me.lblSoil.AutoSize = True
        Me.lblSoil.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoil.Location = New System.Drawing.Point(27, 201)
        Me.lblSoil.Name = "lblSoil"
        Me.lblSoil.Size = New System.Drawing.Size(81, 13)
        Me.lblSoil.TabIndex = 34
        Me.lblSoil.Text = "Soil Depth Grid:"
        Me.lblSoil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC7
        '
        Me.cboBC7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC7.Location = New System.Drawing.Point(183, 198)
        Me.cboBC7.Name = "cboBC7"
        Me.cboBC7.Size = New System.Drawing.Size(351, 21)
        Me.cboBC7.TabIndex = 35
        '
        'lblWater
        '
        Me.lblWater.AutoSize = True
        Me.lblWater.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWater.Location = New System.Drawing.Point(27, 174)
        Me.lblWater.Name = "lblWater"
        Me.lblWater.Size = New System.Drawing.Size(144, 13)
        Me.lblWater.TabIndex = 32
        Me.lblWater.Text = "Water Holding Capacity Grid:"
        Me.lblWater.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC6
        '
        Me.cboBC6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC6.Location = New System.Drawing.Point(183, 171)
        Me.cboBC6.Name = "cboBC6"
        Me.cboBC6.Size = New System.Drawing.Size(351, 21)
        Me.cboBC6.TabIndex = 33
        '
        'lblRunoff
        '
        Me.lblRunoff.AutoSize = True
        Me.lblRunoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRunoff.Location = New System.Drawing.Point(27, 147)
        Me.lblRunoff.Name = "lblRunoff"
        Me.lblRunoff.Size = New System.Drawing.Size(135, 13)
        Me.lblRunoff.TabIndex = 30
        Me.lblRunoff.Text = "Runoff Curve Number Grid:"
        Me.lblRunoff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC5
        '
        Me.cboBC5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC5.Location = New System.Drawing.Point(183, 144)
        Me.cboBC5.Name = "cboBC5"
        Me.cboBC5.Size = New System.Drawing.Size(351, 21)
        Me.cboBC5.TabIndex = 31
        '
        'lblHillLen
        '
        Me.lblHillLen.AutoSize = True
        Me.lblHillLen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHillLen.Location = New System.Drawing.Point(27, 120)
        Me.lblHillLen.Name = "lblHillLen"
        Me.lblHillLen.Size = New System.Drawing.Size(82, 13)
        Me.lblHillLen.TabIndex = 28
        Me.lblHillLen.Text = "Hill Length Grid:"
        Me.lblHillLen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC4
        '
        Me.cboBC4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC4.Location = New System.Drawing.Point(183, 117)
        Me.cboBC4.Name = "cboBC4"
        Me.cboBC4.Size = New System.Drawing.Size(351, 21)
        Me.cboBC4.TabIndex = 29
        '
        'lblFlowAcc
        '
        Me.lblFlowAcc.AutoSize = True
        Me.lblFlowAcc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlowAcc.Location = New System.Drawing.Point(27, 92)
        Me.lblFlowAcc.Name = "lblFlowAcc"
        Me.lblFlowAcc.Size = New System.Drawing.Size(121, 13)
        Me.lblFlowAcc.TabIndex = 26
        Me.lblFlowAcc.Text = "Flow Accumulation Grid:"
        Me.lblFlowAcc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC3
        '
        Me.cboBC3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC3.Location = New System.Drawing.Point(183, 90)
        Me.cboBC3.Name = "cboBC3"
        Me.cboBC3.Size = New System.Drawing.Size(351, 21)
        Me.cboBC3.TabIndex = 27
        '
        'cboBC1
        '
        Me.cboBC1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC1.Location = New System.Drawing.Point(183, 36)
        Me.cboBC1.Name = "cboBC1"
        Me.cboBC1.Size = New System.Drawing.Size(351, 21)
        Me.cboBC1.TabIndex = 25
        '
        'lblGrid
        '
        Me.lblGrid.AutoSize = True
        Me.lblGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrid.Location = New System.Drawing.Point(27, 38)
        Me.lblGrid.Name = "lblGrid"
        Me.lblGrid.Size = New System.Drawing.Size(58, 13)
        Me.lblGrid.TabIndex = 24
        Me.lblGrid.Text = "Basin Grid:"
        Me.lblGrid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProcessed
        '
        Me.lblProcessed.AutoSize = True
        Me.lblProcessed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProcessed.Location = New System.Drawing.Point(27, 65)
        Me.lblProcessed.Name = "lblProcessed"
        Me.lblProcessed.Size = New System.Drawing.Size(87, 13)
        Me.lblProcessed.TabIndex = 20
        Me.lblProcessed.Text = "Processed DEM:"
        Me.lblProcessed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBC2
        '
        Me.cboBC2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBC2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBC2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBC2.Location = New System.Drawing.Point(183, 63)
        Me.cboBC2.Name = "cboBC2"
        Me.cboBC2.Size = New System.Drawing.Size(351, 21)
        Me.cboBC2.TabIndex = 23
        '
        'tpgResponse
        '
        Me.tpgResponse.Controls.Add(Me.lblFlow)
        Me.tpgResponse.Controls.Add(Me.cboFlowDir)
        Me.tpgResponse.Controls.Add(Me.lblOutlets)
        Me.tpgResponse.Controls.Add(Me.cboOutlets)
        Me.tpgResponse.Controls.Add(Me.atxInstream)
        Me.tpgResponse.Controls.Add(Me.atxOverland)
        Me.tpgResponse.Controls.Add(Me.cmdResponseNext)
        Me.tpgResponse.Controls.Add(Me.AtcGridMannings)
        Me.tpgResponse.Controls.Add(Me.cboUSGS)
        Me.tpgResponse.Controls.Add(Me.lblInstream)
        Me.tpgResponse.Controls.Add(Me.lblOverland)
        Me.tpgResponse.Controls.Add(Me.rbnUniform)
        Me.tpgResponse.Controls.Add(Me.rbnNonUniformUser)
        Me.tpgResponse.Controls.Add(Me.rbnNonUniformUSGS)
        Me.tpgResponse.Controls.Add(Me.lblMethod)
        Me.tpgResponse.Location = New System.Drawing.Point(4, 46)
        Me.tpgResponse.Name = "tpgResponse"
        Me.tpgResponse.Size = New System.Drawing.Size(647, 387)
        Me.tpgResponse.TabIndex = 9
        Me.tpgResponse.Text = "Basin Response"
        Me.tpgResponse.UseVisualStyleBackColor = True
        '
        'lblFlow
        '
        Me.lblFlow.AutoSize = True
        Me.lblFlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFlow.Location = New System.Drawing.Point(27, 135)
        Me.lblFlow.Name = "lblFlow"
        Me.lblFlow.Size = New System.Drawing.Size(99, 13)
        Me.lblFlow.TabIndex = 58
        Me.lblFlow.Text = "Flow Direction Grid:"
        Me.lblFlow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboFlowDir
        '
        Me.cboFlowDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFlowDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFlowDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFlowDir.Location = New System.Drawing.Point(154, 132)
        Me.cboFlowDir.Name = "cboFlowDir"
        Me.cboFlowDir.Size = New System.Drawing.Size(368, 21)
        Me.cboFlowDir.TabIndex = 59
        '
        'lblOutlets
        '
        Me.lblOutlets.AutoSize = True
        Me.lblOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutlets.Location = New System.Drawing.Point(27, 162)
        Me.lblOutlets.Name = "lblOutlets"
        Me.lblOutlets.Size = New System.Drawing.Size(107, 13)
        Me.lblOutlets.TabIndex = 56
        Me.lblOutlets.Text = "Subbasin Outlet Grid:"
        Me.lblOutlets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboOutlets
        '
        Me.cboOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboOutlets.Location = New System.Drawing.Point(154, 159)
        Me.cboOutlets.Name = "cboOutlets"
        Me.cboOutlets.Size = New System.Drawing.Size(368, 21)
        Me.cboOutlets.TabIndex = 57
        '
        'atxInstream
        '
        Me.atxInstream.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxInstream.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxInstream.DefaultValue = ""
        Me.atxInstream.HardMax = -999
        Me.atxInstream.HardMin = -999
        Me.atxInstream.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxInstream.Location = New System.Drawing.Point(192, 222)
        Me.atxInstream.MaxWidth = 20
        Me.atxInstream.Name = "atxInstream"
        Me.atxInstream.NumericFormat = "0.#####"
        Me.atxInstream.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxInstream.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxInstream.SelLength = 0
        Me.atxInstream.SelStart = 0
        Me.atxInstream.Size = New System.Drawing.Size(93, 23)
        Me.atxInstream.SoftMax = -999
        Me.atxInstream.SoftMin = -999
        Me.atxInstream.TabIndex = 53
        Me.atxInstream.ValueDouble = 0
        Me.atxInstream.ValueInteger = 0
        '
        'atxOverland
        '
        Me.atxOverland.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxOverland.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxOverland.DefaultValue = ""
        Me.atxOverland.HardMax = -999
        Me.atxOverland.HardMin = -999
        Me.atxOverland.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxOverland.Location = New System.Drawing.Point(192, 196)
        Me.atxOverland.MaxWidth = 20
        Me.atxOverland.Name = "atxOverland"
        Me.atxOverland.NumericFormat = "0.#####"
        Me.atxOverland.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxOverland.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxOverland.SelLength = 0
        Me.atxOverland.SelStart = 0
        Me.atxOverland.Size = New System.Drawing.Size(94, 22)
        Me.atxOverland.SoftMax = -999
        Me.atxOverland.SoftMin = -999
        Me.atxOverland.TabIndex = 52
        Me.atxOverland.ValueDouble = 0
        Me.atxOverland.ValueInteger = 0
        '
        'cmdResponseNext
        '
        Me.cmdResponseNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdResponseNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdResponseNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdResponseNext.Location = New System.Drawing.Point(558, 346)
        Me.cmdResponseNext.Name = "cmdResponseNext"
        Me.cmdResponseNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdResponseNext.TabIndex = 51
        Me.cmdResponseNext.Text = "Next >"
        '
        'AtcGridMannings
        '
        Me.AtcGridMannings.AllowHorizontalScrolling = True
        Me.AtcGridMannings.AllowNewValidValues = False
        Me.AtcGridMannings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridMannings.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridMannings.Fixed3D = False
        Me.AtcGridMannings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridMannings.LineColor = System.Drawing.Color.Empty
        Me.AtcGridMannings.LineWidth = 0.0!
        Me.AtcGridMannings.Location = New System.Drawing.Point(30, 196)
        Me.AtcGridMannings.Name = "AtcGridMannings"
        Me.AtcGridMannings.Size = New System.Drawing.Size(492, 169)
        Me.AtcGridMannings.Source = Nothing
        Me.AtcGridMannings.TabIndex = 50
        '
        'cboUSGS
        '
        Me.cboUSGS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboUSGS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUSGS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUSGS.Location = New System.Drawing.Point(274, 53)
        Me.cboUSGS.Name = "cboUSGS"
        Me.cboUSGS.Size = New System.Drawing.Size(248, 21)
        Me.cboUSGS.TabIndex = 49
        '
        'lblInstream
        '
        Me.lblInstream.AutoSize = True
        Me.lblInstream.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstream.Location = New System.Drawing.Point(28, 223)
        Me.lblInstream.Name = "lblInstream"
        Me.lblInstream.Size = New System.Drawing.Size(142, 13)
        Me.lblInstream.TabIndex = 44
        Me.lblInstream.Text = "Instream Flow Velocity (m/s):"
        Me.lblInstream.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOverland
        '
        Me.lblOverland.AutoSize = True
        Me.lblOverland.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverland.Location = New System.Drawing.Point(28, 196)
        Me.lblOverland.Name = "lblOverland"
        Me.lblOverland.Size = New System.Drawing.Size(145, 13)
        Me.lblOverland.TabIndex = 42
        Me.lblOverland.Text = "Overland Flow Velocity (m/s):"
        Me.lblOverland.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbnUniform
        '
        Me.rbnUniform.AutoSize = True
        Me.rbnUniform.Location = New System.Drawing.Point(48, 103)
        Me.rbnUniform.Name = "rbnUniform"
        Me.rbnUniform.Size = New System.Drawing.Size(223, 17)
        Me.rbnUniform.TabIndex = 3
        Me.rbnUniform.Text = "Uniform from User Supplied Velocity Value"
        Me.rbnUniform.UseVisualStyleBackColor = True
        '
        'rbnNonUniformUser
        '
        Me.rbnNonUniformUser.AutoSize = True
        Me.rbnNonUniformUser.Location = New System.Drawing.Point(48, 80)
        Me.rbnNonUniformUser.Name = "rbnNonUniformUser"
        Me.rbnNonUniformUser.Size = New System.Drawing.Size(224, 17)
        Me.rbnNonUniformUser.TabIndex = 2
        Me.rbnNonUniformUser.Text = "Non-Uniform from User Supplied Velocities"
        Me.rbnNonUniformUser.UseVisualStyleBackColor = True
        '
        'rbnNonUniformUSGS
        '
        Me.rbnNonUniformUSGS.AutoSize = True
        Me.rbnNonUniformUSGS.Checked = True
        Me.rbnNonUniformUSGS.Location = New System.Drawing.Point(48, 57)
        Me.rbnNonUniformUSGS.Name = "rbnNonUniformUSGS"
        Me.rbnNonUniformUSGS.Size = New System.Drawing.Size(220, 17)
        Me.rbnNonUniformUSGS.TabIndex = 1
        Me.rbnNonUniformUSGS.TabStop = True
        Me.rbnNonUniformUSGS.Text = "Non-Uniform from USGS Land Cover Grid"
        Me.rbnNonUniformUSGS.UseVisualStyleBackColor = True
        '
        'lblMethod
        '
        Me.lblMethod.AutoSize = True
        Me.lblMethod.Location = New System.Drawing.Point(28, 30)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.Size = New System.Drawing.Size(231, 13)
        Me.lblMethod.TabIndex = 0
        Me.lblMethod.Text = "Method of Overland Flow Velocity Computation:"
        '
        'tpgRain
        '
        Me.tpgRain.Controls.Add(Me.AtcGridPrec)
        Me.tpgRain.Controls.Add(Me.cmdRainEvapNext)
        Me.tpgRain.Controls.Add(Me.gbxSimulationDates)
        Me.tpgRain.Location = New System.Drawing.Point(4, 46)
        Me.tpgRain.Name = "tpgRain"
        Me.tpgRain.Size = New System.Drawing.Size(647, 387)
        Me.tpgRain.TabIndex = 2
        Me.tpgRain.Text = "Rain/Evap Data"
        Me.tpgRain.UseVisualStyleBackColor = True
        '
        'AtcGridPrec
        '
        Me.AtcGridPrec.AllowHorizontalScrolling = True
        Me.AtcGridPrec.AllowNewValidValues = False
        Me.AtcGridPrec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridPrec.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridPrec.Fixed3D = False
        Me.AtcGridPrec.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridPrec.LineColor = System.Drawing.Color.Empty
        Me.AtcGridPrec.LineWidth = 0.0!
        Me.AtcGridPrec.Location = New System.Drawing.Point(19, 129)
        Me.AtcGridPrec.Name = "AtcGridPrec"
        Me.AtcGridPrec.Size = New System.Drawing.Size(516, 246)
        Me.AtcGridPrec.Source = Nothing
        Me.AtcGridPrec.TabIndex = 41
        '
        'cmdRainEvapNext
        '
        Me.cmdRainEvapNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRainEvapNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdRainEvapNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRainEvapNext.Location = New System.Drawing.Point(556, 343)
        Me.cmdRainEvapNext.Name = "cmdRainEvapNext"
        Me.cmdRainEvapNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdRainEvapNext.TabIndex = 40
        Me.cmdRainEvapNext.Text = "Next >"
        '
        'gbxSimulationDates
        '
        Me.gbxSimulationDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxSimulationDates.Controls.Add(Me.Label54)
        Me.gbxSimulationDates.Controls.Add(Me.Label55)
        Me.gbxSimulationDates.Controls.Add(Me.Label56)
        Me.gbxSimulationDates.Controls.Add(Me.Label57)
        Me.gbxSimulationDates.Controls.Add(Me.Label58)
        Me.gbxSimulationDates.Controls.Add(Me.atxEDay)
        Me.gbxSimulationDates.Controls.Add(Me.atxSDay)
        Me.gbxSimulationDates.Controls.Add(Me.atxSYear)
        Me.gbxSimulationDates.Controls.Add(Me.atxEMonth)
        Me.gbxSimulationDates.Controls.Add(Me.atxSMonth)
        Me.gbxSimulationDates.Controls.Add(Me.atxEYear)
        Me.gbxSimulationDates.Location = New System.Drawing.Point(19, 18)
        Me.gbxSimulationDates.Name = "gbxSimulationDates"
        Me.gbxSimulationDates.Size = New System.Drawing.Size(516, 96)
        Me.gbxSimulationDates.TabIndex = 29
        Me.gbxSimulationDates.TabStop = False
        Me.gbxSimulationDates.Text = "Simulation Dates"
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(87, 58)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(26, 13)
        Me.Label54.TabIndex = 37
        Me.Label54.Text = "End"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(83, 32)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(29, 13)
        Me.Label55.TabIndex = 36
        Me.Label55.Text = "Start"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(242, 14)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(26, 13)
        Me.Label56.TabIndex = 35
        Me.Label56.Text = "Day"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(193, 14)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(37, 13)
        Me.Label57.TabIndex = 34
        Me.Label57.Text = "Month"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(125, 14)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(29, 13)
        Me.Label58.TabIndex = 33
        Me.Label58.Text = "Year"
        '
        'atxEDay
        '
        Me.atxEDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEDay.DefaultValue = ""
        Me.atxEDay.HardMax = 31
        Me.atxEDay.HardMin = 1
        Me.atxEDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEDay.Location = New System.Drawing.Point(245, 58)
        Me.atxEDay.MaxWidth = 20
        Me.atxEDay.Name = "atxEDay"
        Me.atxEDay.NumericFormat = "0"
        Me.atxEDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEDay.SelLength = 0
        Me.atxEDay.SelStart = 0
        Me.atxEDay.Size = New System.Drawing.Size(44, 21)
        Me.atxEDay.SoftMax = -999
        Me.atxEDay.SoftMin = -999
        Me.atxEDay.TabIndex = 32
        Me.atxEDay.ValueDouble = 31
        Me.atxEDay.ValueInteger = 31
        '
        'atxSDay
        '
        Me.atxSDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSDay.DefaultValue = ""
        Me.atxSDay.HardMax = 31
        Me.atxSDay.HardMin = 1
        Me.atxSDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSDay.Location = New System.Drawing.Point(245, 32)
        Me.atxSDay.MaxWidth = 20
        Me.atxSDay.Name = "atxSDay"
        Me.atxSDay.NumericFormat = "0"
        Me.atxSDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSDay.SelLength = 0
        Me.atxSDay.SelStart = 0
        Me.atxSDay.Size = New System.Drawing.Size(44, 21)
        Me.atxSDay.SoftMax = -999
        Me.atxSDay.SoftMin = -999
        Me.atxSDay.TabIndex = 31
        Me.atxSDay.ValueDouble = 1
        Me.atxSDay.ValueInteger = 1
        '
        'atxSYear
        '
        Me.atxSYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSYear.DefaultValue = ""
        Me.atxSYear.HardMax = 9999
        Me.atxSYear.HardMin = 0
        Me.atxSYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSYear.Location = New System.Drawing.Point(127, 32)
        Me.atxSYear.MaxWidth = 20
        Me.atxSYear.Name = "atxSYear"
        Me.atxSYear.NumericFormat = "0"
        Me.atxSYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSYear.SelLength = 0
        Me.atxSYear.SelStart = 0
        Me.atxSYear.Size = New System.Drawing.Size(64, 21)
        Me.atxSYear.SoftMax = -999
        Me.atxSYear.SoftMin = -999
        Me.atxSYear.TabIndex = 30
        Me.atxSYear.ValueDouble = 2000
        Me.atxSYear.ValueInteger = 2000
        '
        'atxEMonth
        '
        Me.atxEMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEMonth.DefaultValue = ""
        Me.atxEMonth.HardMax = 12
        Me.atxEMonth.HardMin = 1
        Me.atxEMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEMonth.Location = New System.Drawing.Point(196, 58)
        Me.atxEMonth.MaxWidth = 20
        Me.atxEMonth.Name = "atxEMonth"
        Me.atxEMonth.NumericFormat = "0"
        Me.atxEMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEMonth.SelLength = 0
        Me.atxEMonth.SelStart = 0
        Me.atxEMonth.Size = New System.Drawing.Size(44, 21)
        Me.atxEMonth.SoftMax = -999
        Me.atxEMonth.SoftMin = -999
        Me.atxEMonth.TabIndex = 29
        Me.atxEMonth.ValueDouble = 12
        Me.atxEMonth.ValueInteger = 12
        '
        'atxSMonth
        '
        Me.atxSMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxSMonth.DefaultValue = ""
        Me.atxSMonth.HardMax = 12
        Me.atxSMonth.HardMin = 1
        Me.atxSMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSMonth.Location = New System.Drawing.Point(196, 32)
        Me.atxSMonth.MaxWidth = 20
        Me.atxSMonth.Name = "atxSMonth"
        Me.atxSMonth.NumericFormat = "0"
        Me.atxSMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSMonth.SelLength = 0
        Me.atxSMonth.SelStart = 0
        Me.atxSMonth.Size = New System.Drawing.Size(44, 21)
        Me.atxSMonth.SoftMax = -999
        Me.atxSMonth.SoftMin = -999
        Me.atxSMonth.TabIndex = 28
        Me.atxSMonth.ValueDouble = 1
        Me.atxSMonth.ValueInteger = 1
        '
        'atxEYear
        '
        Me.atxEYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxEYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxEYear.DefaultValue = ""
        Me.atxEYear.HardMax = 9999
        Me.atxEYear.HardMin = 0
        Me.atxEYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxEYear.Location = New System.Drawing.Point(127, 58)
        Me.atxEYear.MaxWidth = 20
        Me.atxEYear.Name = "atxEYear"
        Me.atxEYear.NumericFormat = "0"
        Me.atxEYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxEYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxEYear.SelLength = 0
        Me.atxEYear.SelStart = 0
        Me.atxEYear.Size = New System.Drawing.Size(64, 21)
        Me.atxEYear.SoftMax = -999
        Me.atxEYear.SoftMin = -999
        Me.atxEYear.TabIndex = 27
        Me.atxEYear.ValueDouble = 2000
        Me.atxEYear.ValueInteger = 2000
        '
        'tpgSoil
        '
        Me.tpgSoil.Controls.Add(Me.gbxFlow)
        Me.tpgSoil.Controls.Add(Me.atxInitial)
        Me.tpgSoil.Controls.Add(Me.gbxMode)
        Me.tpgSoil.Controls.Add(Me.gbxRun)
        Me.tpgSoil.Controls.Add(Me.gbxData)
        Me.tpgSoil.Controls.Add(Me.lblInitial)
        Me.tpgSoil.Controls.Add(Me.cmdBalanceNext)
        Me.tpgSoil.Location = New System.Drawing.Point(4, 46)
        Me.tpgSoil.Name = "tpgSoil"
        Me.tpgSoil.Size = New System.Drawing.Size(647, 387)
        Me.tpgSoil.TabIndex = 11
        Me.tpgSoil.Text = "Compute Soil Water Balance"
        Me.tpgSoil.UseVisualStyleBackColor = True
        '
        'gbxFlow
        '
        Me.gbxFlow.Controls.Add(Me.rbnNonlinear)
        Me.gbxFlow.Controls.Add(Me.rbnLinear)
        Me.gbxFlow.Location = New System.Drawing.Point(209, 130)
        Me.gbxFlow.Name = "gbxFlow"
        Me.gbxFlow.Size = New System.Drawing.Size(159, 70)
        Me.gbxFlow.TabIndex = 59
        Me.gbxFlow.TabStop = False
        Me.gbxFlow.Text = "Flow Routing Method"
        '
        'rbnNonlinear
        '
        Me.rbnNonlinear.AutoSize = True
        Me.rbnNonlinear.Location = New System.Drawing.Point(7, 42)
        Me.rbnNonlinear.Name = "rbnNonlinear"
        Me.rbnNonlinear.Size = New System.Drawing.Size(129, 17)
        Me.rbnNonlinear.TabIndex = 2
        Me.rbnNonlinear.Text = "Non-Linear Soil Model"
        Me.rbnNonlinear.UseVisualStyleBackColor = True
        '
        'rbnLinear
        '
        Me.rbnLinear.AutoSize = True
        Me.rbnLinear.Checked = True
        Me.rbnLinear.Location = New System.Drawing.Point(7, 19)
        Me.rbnLinear.Name = "rbnLinear"
        Me.rbnLinear.Size = New System.Drawing.Size(106, 17)
        Me.rbnLinear.TabIndex = 1
        Me.rbnLinear.TabStop = True
        Me.rbnLinear.Text = "Linear Soil Model"
        Me.rbnLinear.UseVisualStyleBackColor = True
        '
        'atxInitial
        '
        Me.atxInitial.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxInitial.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxInitial.DefaultValue = "0.1"
        Me.atxInitial.HardMax = 1
        Me.atxInitial.HardMin = 0
        Me.atxInitial.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxInitial.Location = New System.Drawing.Point(183, 30)
        Me.atxInitial.MaxWidth = 20
        Me.atxInitial.Name = "atxInitial"
        Me.atxInitial.NumericFormat = "0.#####"
        Me.atxInitial.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxInitial.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxInitial.SelLength = 0
        Me.atxInitial.SelStart = 0
        Me.atxInitial.Size = New System.Drawing.Size(85, 22)
        Me.atxInitial.SoftMax = -999
        Me.atxInitial.SoftMin = -999
        Me.atxInitial.TabIndex = 58
        Me.atxInitial.ValueDouble = 0.1
        Me.atxInitial.ValueInteger = 0
        '
        'gbxMode
        '
        Me.gbxMode.Controls.Add(Me.rbnCal)
        Me.gbxMode.Controls.Add(Me.rbnSim)
        Me.gbxMode.Location = New System.Drawing.Point(386, 130)
        Me.gbxMode.Name = "gbxMode"
        Me.gbxMode.Size = New System.Drawing.Size(159, 70)
        Me.gbxMode.TabIndex = 57
        Me.gbxMode.TabStop = False
        Me.gbxMode.Text = "Mode"
        '
        'rbnCal
        '
        Me.rbnCal.AutoSize = True
        Me.rbnCal.Location = New System.Drawing.Point(7, 42)
        Me.rbnCal.Name = "rbnCal"
        Me.rbnCal.Size = New System.Drawing.Size(74, 17)
        Me.rbnCal.TabIndex = 2
        Me.rbnCal.Text = "Calibration"
        Me.rbnCal.UseVisualStyleBackColor = True
        '
        'rbnSim
        '
        Me.rbnSim.AutoSize = True
        Me.rbnSim.Checked = True
        Me.rbnSim.Location = New System.Drawing.Point(7, 19)
        Me.rbnSim.Name = "rbnSim"
        Me.rbnSim.Size = New System.Drawing.Size(73, 17)
        Me.rbnSim.TabIndex = 1
        Me.rbnSim.TabStop = True
        Me.rbnSim.Text = "Simulation"
        Me.rbnSim.UseVisualStyleBackColor = True
        '
        'gbxRun
        '
        Me.gbxRun.Controls.Add(Me.rbnContinue)
        Me.gbxRun.Controls.Add(Me.rbnRun)
        Me.gbxRun.Location = New System.Drawing.Point(31, 130)
        Me.gbxRun.Name = "gbxRun"
        Me.gbxRun.Size = New System.Drawing.Size(159, 70)
        Me.gbxRun.TabIndex = 56
        Me.gbxRun.TabStop = False
        Me.gbxRun.Text = "Run Option"
        '
        'rbnContinue
        '
        Me.rbnContinue.AutoSize = True
        Me.rbnContinue.Location = New System.Drawing.Point(7, 42)
        Me.rbnContinue.Name = "rbnContinue"
        Me.rbnContinue.Size = New System.Drawing.Size(134, 17)
        Me.rbnContinue.TabIndex = 2
        Me.rbnContinue.Text = "Continue Previous Run"
        Me.rbnContinue.UseVisualStyleBackColor = True
        '
        'rbnRun
        '
        Me.rbnRun.AutoSize = True
        Me.rbnRun.Checked = True
        Me.rbnRun.Location = New System.Drawing.Point(7, 19)
        Me.rbnRun.Name = "rbnRun"
        Me.rbnRun.Size = New System.Drawing.Size(70, 17)
        Me.rbnRun.TabIndex = 1
        Me.rbnRun.TabStop = True
        Me.rbnRun.Text = "New Run"
        Me.rbnRun.UseVisualStyleBackColor = True
        '
        'gbxData
        '
        Me.gbxData.Controls.Add(Me.rbnDaily)
        Me.gbxData.Controls.Add(Me.rbnHourly)
        Me.gbxData.Location = New System.Drawing.Point(31, 68)
        Me.gbxData.Name = "gbxData"
        Me.gbxData.Size = New System.Drawing.Size(152, 47)
        Me.gbxData.TabIndex = 55
        Me.gbxData.TabStop = False
        Me.gbxData.Text = "Data Format"
        '
        'rbnDaily
        '
        Me.rbnDaily.AutoSize = True
        Me.rbnDaily.Checked = True
        Me.rbnDaily.Location = New System.Drawing.Point(75, 19)
        Me.rbnDaily.Name = "rbnDaily"
        Me.rbnDaily.Size = New System.Drawing.Size(48, 17)
        Me.rbnDaily.TabIndex = 1
        Me.rbnDaily.TabStop = True
        Me.rbnDaily.Text = "Daily"
        Me.rbnDaily.UseVisualStyleBackColor = True
        '
        'rbnHourly
        '
        Me.rbnHourly.AutoSize = True
        Me.rbnHourly.Location = New System.Drawing.Point(14, 19)
        Me.rbnHourly.Name = "rbnHourly"
        Me.rbnHourly.Size = New System.Drawing.Size(55, 17)
        Me.rbnHourly.TabIndex = 0
        Me.rbnHourly.Text = "Hourly"
        Me.rbnHourly.UseVisualStyleBackColor = True
        '
        'lblInitial
        '
        Me.lblInitial.AutoSize = True
        Me.lblInitial.Location = New System.Drawing.Point(28, 31)
        Me.lblInitial.Name = "lblInitial"
        Me.lblInitial.Size = New System.Drawing.Size(97, 13)
        Me.lblInitial.TabIndex = 53
        Me.lblInitial.Text = "Initial Soil Moisture:"
        '
        'cmdBalanceNext
        '
        Me.cmdBalanceNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBalanceNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdBalanceNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBalanceNext.Location = New System.Drawing.Point(554, 344)
        Me.cmdBalanceNext.Name = "cmdBalanceNext"
        Me.cmdBalanceNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdBalanceNext.TabIndex = 52
        Me.cmdBalanceNext.Text = "Next >"
        '
        'tpgFlow
        '
        Me.tpgFlow.Controls.Add(Me.gbxStreamFlow)
        Me.tpgFlow.Controls.Add(Me.atxForecast)
        Me.tpgFlow.Controls.Add(Me.lflForecast)
        Me.tpgFlow.Controls.Add(Me.cmdRouteNext)
        Me.tpgFlow.Location = New System.Drawing.Point(4, 46)
        Me.tpgFlow.Name = "tpgFlow"
        Me.tpgFlow.Size = New System.Drawing.Size(647, 387)
        Me.tpgFlow.TabIndex = 12
        Me.tpgFlow.Text = "Compute Stream Flow"
        Me.tpgFlow.UseVisualStyleBackColor = True
        '
        'gbxStreamFlow
        '
        Me.gbxStreamFlow.Controls.Add(Me.rbnSimple)
        Me.gbxStreamFlow.Controls.Add(Me.rbnMusk)
        Me.gbxStreamFlow.Controls.Add(Me.rbnDiffusion)
        Me.gbxStreamFlow.Location = New System.Drawing.Point(51, 100)
        Me.gbxStreamFlow.Name = "gbxStreamFlow"
        Me.gbxStreamFlow.Size = New System.Drawing.Size(227, 95)
        Me.gbxStreamFlow.TabIndex = 61
        Me.gbxStreamFlow.TabStop = False
        Me.gbxStreamFlow.Text = "Flow Routing Method"
        '
        'rbnSimple
        '
        Me.rbnSimple.AutoSize = True
        Me.rbnSimple.Location = New System.Drawing.Point(18, 65)
        Me.rbnSimple.Name = "rbnSimple"
        Me.rbnSimple.Size = New System.Drawing.Size(156, 17)
        Me.rbnSimple.TabIndex = 3
        Me.rbnSimple.Text = "Simple Lag Routing Method"
        Me.rbnSimple.UseVisualStyleBackColor = True
        '
        'rbnMusk
        '
        Me.rbnMusk.AutoSize = True
        Me.rbnMusk.Checked = True
        Me.rbnMusk.Location = New System.Drawing.Point(18, 42)
        Me.rbnMusk.Name = "rbnMusk"
        Me.rbnMusk.Size = New System.Drawing.Size(192, 17)
        Me.rbnMusk.TabIndex = 2
        Me.rbnMusk.TabStop = True
        Me.rbnMusk.Text = "Muskingum Cunge Routing Method"
        Me.rbnMusk.UseVisualStyleBackColor = True
        '
        'rbnDiffusion
        '
        Me.rbnDiffusion.AutoSize = True
        Me.rbnDiffusion.Location = New System.Drawing.Point(18, 19)
        Me.rbnDiffusion.Name = "rbnDiffusion"
        Me.rbnDiffusion.Size = New System.Drawing.Size(181, 17)
        Me.rbnDiffusion.TabIndex = 1
        Me.rbnDiffusion.Text = "Diffusion Analog Routing Method"
        Me.rbnDiffusion.UseVisualStyleBackColor = True
        '
        'atxForecast
        '
        Me.atxForecast.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxForecast.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxForecast.DefaultValue = "3"
        Me.atxForecast.HardMax = 99
        Me.atxForecast.HardMin = 0
        Me.atxForecast.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxForecast.Location = New System.Drawing.Point(222, 58)
        Me.atxForecast.MaxWidth = 20
        Me.atxForecast.Name = "atxForecast"
        Me.atxForecast.NumericFormat = "0.#####"
        Me.atxForecast.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxForecast.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxForecast.SelLength = 0
        Me.atxForecast.SelStart = 0
        Me.atxForecast.Size = New System.Drawing.Size(85, 22)
        Me.atxForecast.SoftMax = -999
        Me.atxForecast.SoftMin = -999
        Me.atxForecast.TabIndex = 60
        Me.atxForecast.ValueDouble = 3
        Me.atxForecast.ValueInteger = 3
        '
        'lflForecast
        '
        Me.lflForecast.AutoSize = True
        Me.lflForecast.Location = New System.Drawing.Point(48, 58)
        Me.lflForecast.Name = "lflForecast"
        Me.lflForecast.Size = New System.Drawing.Size(168, 13)
        Me.lflForecast.TabIndex = 59
        Me.lflForecast.Text = "No. of Days of Forecast Required:"
        '
        'cmdRouteNext
        '
        Me.cmdRouteNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRouteNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdRouteNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRouteNext.Location = New System.Drawing.Point(555, 340)
        Me.cmdRouteNext.Name = "cmdRouteNext"
        Me.cmdRouteNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdRouteNext.TabIndex = 53
        Me.cmdRouteNext.Text = "Next >"
        '
        'tpgSensitivity
        '
        Me.tpgSensitivity.Controls.Add(Me.AtcGridSensitivity)
        Me.tpgSensitivity.Controls.Add(Me.lblReach)
        Me.tpgSensitivity.Controls.Add(Me.cboReachSensitivity)
        Me.tpgSensitivity.Controls.Add(Me.cmdSensitivityNext)
        Me.tpgSensitivity.Location = New System.Drawing.Point(4, 46)
        Me.tpgSensitivity.Name = "tpgSensitivity"
        Me.tpgSensitivity.Size = New System.Drawing.Size(647, 387)
        Me.tpgSensitivity.TabIndex = 13
        Me.tpgSensitivity.Text = "Sensitivity Analysis"
        Me.tpgSensitivity.UseVisualStyleBackColor = True
        '
        'AtcGridSensitivity
        '
        Me.AtcGridSensitivity.AllowHorizontalScrolling = True
        Me.AtcGridSensitivity.AllowNewValidValues = False
        Me.AtcGridSensitivity.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridSensitivity.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridSensitivity.Fixed3D = False
        Me.AtcGridSensitivity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridSensitivity.LineColor = System.Drawing.Color.Empty
        Me.AtcGridSensitivity.LineWidth = 0.0!
        Me.AtcGridSensitivity.Location = New System.Drawing.Point(23, 77)
        Me.AtcGridSensitivity.Name = "AtcGridSensitivity"
        Me.AtcGridSensitivity.Size = New System.Drawing.Size(516, 255)
        Me.AtcGridSensitivity.Source = Nothing
        Me.AtcGridSensitivity.TabIndex = 44
        '
        'lblReach
        '
        Me.lblReach.AutoSize = True
        Me.lblReach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReach.Location = New System.Drawing.Point(20, 43)
        Me.lblReach.Name = "lblReach"
        Me.lblReach.Size = New System.Drawing.Size(56, 13)
        Me.lblReach.TabIndex = 43
        Me.lblReach.Text = "Reach ID:"
        Me.lblReach.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboReachSensitivity
        '
        Me.cboReachSensitivity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReachSensitivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReachSensitivity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReachSensitivity.Location = New System.Drawing.Point(100, 40)
        Me.cboReachSensitivity.Name = "cboReachSensitivity"
        Me.cboReachSensitivity.Size = New System.Drawing.Size(183, 21)
        Me.cboReachSensitivity.TabIndex = 42
        '
        'cmdSensitivityNext
        '
        Me.cmdSensitivityNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSensitivityNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdSensitivityNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSensitivityNext.Location = New System.Drawing.Point(558, 353)
        Me.cmdSensitivityNext.Name = "cmdSensitivityNext"
        Me.cmdSensitivityNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdSensitivityNext.TabIndex = 40
        Me.cmdSensitivityNext.Text = "Next >"
        '
        'tpgCalibrate
        '
        Me.tpgCalibrate.Controls.Add(Me.lblMax)
        Me.tpgCalibrate.Controls.Add(Me.lstMax)
        Me.tpgCalibrate.Controls.Add(Me.gbxObjective)
        Me.tpgCalibrate.Controls.Add(Me.lblParms)
        Me.tpgCalibrate.Controls.Add(Me.lstCalib)
        Me.tpgCalibrate.Controls.Add(Me.gbxConnect)
        Me.tpgCalibrate.Controls.Add(Me.cmdCalibrationNext)
        Me.tpgCalibrate.Location = New System.Drawing.Point(4, 46)
        Me.tpgCalibrate.Name = "tpgCalibrate"
        Me.tpgCalibrate.Size = New System.Drawing.Size(647, 387)
        Me.tpgCalibrate.TabIndex = 14
        Me.tpgCalibrate.Text = "Model Calibration"
        Me.tpgCalibrate.UseVisualStyleBackColor = True
        '
        'lblMax
        '
        Me.lblMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMax.AutoSize = True
        Me.lblMax.Location = New System.Drawing.Point(506, 24)
        Me.lblMax.Name = "lblMax"
        Me.lblMax.Size = New System.Drawing.Size(131, 13)
        Me.lblMax.TabIndex = 47
        Me.lblMax.Text = "Maximum Number of Runs"
        '
        'lstMax
        '
        Me.lstMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMax.FormattingEnabled = True
        Me.lstMax.Location = New System.Drawing.Point(509, 40)
        Me.lstMax.Name = "lstMax"
        Me.lstMax.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstMax.Size = New System.Drawing.Size(125, 121)
        Me.lstMax.TabIndex = 46
        '
        'gbxObjective
        '
        Me.gbxObjective.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxObjective.Controls.Add(Me.rbnObj6)
        Me.gbxObjective.Controls.Add(Me.rbnObj5)
        Me.gbxObjective.Controls.Add(Me.rbnObj4)
        Me.gbxObjective.Controls.Add(Me.rbnObj3)
        Me.gbxObjective.Controls.Add(Me.rbnObj2)
        Me.gbxObjective.Controls.Add(Me.rbnObj1)
        Me.gbxObjective.Location = New System.Drawing.Point(369, 166)
        Me.gbxObjective.Name = "gbxObjective"
        Me.gbxObjective.Size = New System.Drawing.Size(265, 168)
        Me.gbxObjective.TabIndex = 45
        Me.gbxObjective.TabStop = False
        Me.gbxObjective.Text = "Objective Function Type"
        '
        'rbnObj6
        '
        Me.rbnObj6.AutoSize = True
        Me.rbnObj6.Location = New System.Drawing.Point(10, 138)
        Me.rbnObj6.Name = "rbnObj6"
        Me.rbnObj6.Size = New System.Drawing.Size(49, 17)
        Me.rbnObj6.TabIndex = 5
        Me.rbnObj6.Text = "BIAS"
        Me.rbnObj6.UseVisualStyleBackColor = True
        '
        'rbnObj5
        '
        Me.rbnObj5.AutoSize = True
        Me.rbnObj5.Location = New System.Drawing.Point(10, 113)
        Me.rbnObj5.Name = "rbnObj5"
        Me.rbnObj5.Size = New System.Drawing.Size(174, 17)
        Me.rbnObj5.TabIndex = 4
        Me.rbnObj5.Text = "Number of Sign Changes (NSC)"
        Me.rbnObj5.UseVisualStyleBackColor = True
        '
        'rbnObj4
        '
        Me.rbnObj4.AutoSize = True
        Me.rbnObj4.Location = New System.Drawing.Point(10, 90)
        Me.rbnObj4.Name = "rbnObj4"
        Me.rbnObj4.Size = New System.Drawing.Size(171, 17)
        Me.rbnObj4.TabIndex = 3
        Me.rbnObj4.Text = "Nash-Sutcliffe Efficiency (NSE)"
        Me.rbnObj4.UseVisualStyleBackColor = True
        '
        'rbnObj3
        '
        Me.rbnObj3.AutoSize = True
        Me.rbnObj3.Location = New System.Drawing.Point(10, 67)
        Me.rbnObj3.Name = "rbnObj3"
        Me.rbnObj3.Size = New System.Drawing.Size(176, 17)
        Me.rbnObj3.TabIndex = 2
        Me.rbnObj3.Text = "Maximum Likelihood Error (MLE)"
        Me.rbnObj3.UseVisualStyleBackColor = True
        '
        'rbnObj2
        '
        Me.rbnObj2.AutoSize = True
        Me.rbnObj2.Location = New System.Drawing.Point(10, 44)
        Me.rbnObj2.Name = "rbnObj2"
        Me.rbnObj2.Size = New System.Drawing.Size(147, 17)
        Me.rbnObj2.TabIndex = 1
        Me.rbnObj2.Text = "Standard Deviation (STD)"
        Me.rbnObj2.UseVisualStyleBackColor = True
        '
        'rbnObj1
        '
        Me.rbnObj1.AutoSize = True
        Me.rbnObj1.Checked = True
        Me.rbnObj1.Location = New System.Drawing.Point(10, 21)
        Me.rbnObj1.Name = "rbnObj1"
        Me.rbnObj1.Size = New System.Drawing.Size(180, 17)
        Me.rbnObj1.TabIndex = 0
        Me.rbnObj1.TabStop = True
        Me.rbnObj1.Text = "Root Mean Square Error (RMSE)"
        Me.rbnObj1.UseVisualStyleBackColor = True
        '
        'lblParms
        '
        Me.lblParms.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblParms.AutoSize = True
        Me.lblParms.Location = New System.Drawing.Point(366, 24)
        Me.lblParms.Name = "lblParms"
        Me.lblParms.Size = New System.Drawing.Size(112, 13)
        Me.lblParms.TabIndex = 44
        Me.lblParms.Text = "Calibration Parameters"
        '
        'lstCalib
        '
        Me.lstCalib.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstCalib.FormattingEnabled = True
        Me.lstCalib.Location = New System.Drawing.Point(369, 40)
        Me.lstCalib.Name = "lstCalib"
        Me.lstCalib.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstCalib.Size = New System.Drawing.Size(125, 121)
        Me.lstCalib.TabIndex = 43
        '
        'gbxConnect
        '
        Me.gbxConnect.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxConnect.Controls.Add(Me.AtcConnectFlows)
        Me.gbxConnect.Location = New System.Drawing.Point(22, 22)
        Me.gbxConnect.Name = "gbxConnect"
        Me.gbxConnect.Size = New System.Drawing.Size(329, 313)
        Me.gbxConnect.TabIndex = 42
        Me.gbxConnect.TabStop = False
        Me.gbxConnect.Text = "Observed Flow / Subbasin Connections"
        '
        'AtcConnectFlows
        '
        Me.AtcConnectFlows.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcConnectFlows.Location = New System.Drawing.Point(5, 18)
        Me.AtcConnectFlows.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.AtcConnectFlows.Name = "AtcConnectFlows"
        Me.AtcConnectFlows.Size = New System.Drawing.Size(319, 281)
        Me.AtcConnectFlows.TabIndex = 42
        '
        'cmdCalibrationNext
        '
        Me.cmdCalibrationNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCalibrationNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCalibrationNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCalibrationNext.Location = New System.Drawing.Point(554, 341)
        Me.cmdCalibrationNext.Name = "cmdCalibrationNext"
        Me.cmdCalibrationNext.Size = New System.Drawing.Size(73, 28)
        Me.cmdCalibrationNext.TabIndex = 40
        Me.cmdCalibrationNext.Text = "Next >"
        '
        'tpgMap
        '
        Me.tpgMap.Controls.Add(Me.gbxHydrographs)
        Me.tpgMap.Controls.Add(Me.gbxBankfull)
        Me.tpgMap.Controls.Add(Me.gbxMapper)
        Me.tpgMap.Location = New System.Drawing.Point(4, 46)
        Me.tpgMap.Name = "tpgMap"
        Me.tpgMap.Size = New System.Drawing.Size(647, 387)
        Me.tpgMap.TabIndex = 16
        Me.tpgMap.Text = "Output Results"
        Me.tpgMap.UseVisualStyleBackColor = True
        '
        'gbxHydrographs
        '
        Me.gbxHydrographs.Controls.Add(Me.lblRchHydro)
        Me.gbxHydrographs.Controls.Add(Me.cboRchHydro)
        Me.gbxHydrographs.Controls.Add(Me.cmdReadStreamflow)
        Me.gbxHydrographs.Location = New System.Drawing.Point(460, 32)
        Me.gbxHydrographs.Name = "gbxHydrographs"
        Me.gbxHydrographs.Size = New System.Drawing.Size(169, 154)
        Me.gbxHydrographs.TabIndex = 66
        Me.gbxHydrographs.TabStop = False
        Me.gbxHydrographs.Text = "Flow Hydrographs"
        '
        'lblRchHydro
        '
        Me.lblRchHydro.AutoSize = True
        Me.lblRchHydro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRchHydro.Location = New System.Drawing.Point(6, 30)
        Me.lblRchHydro.Name = "lblRchHydro"
        Me.lblRchHydro.Size = New System.Drawing.Size(56, 13)
        Me.lblRchHydro.TabIndex = 65
        Me.lblRchHydro.Text = "Reach ID:"
        Me.lblRchHydro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboRchHydro
        '
        Me.cboRchHydro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRchHydro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRchHydro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRchHydro.Location = New System.Drawing.Point(68, 27)
        Me.cboRchHydro.Name = "cboRchHydro"
        Me.cboRchHydro.Size = New System.Drawing.Size(85, 21)
        Me.cboRchHydro.TabIndex = 64
        '
        'cmdReadStreamflow
        '
        Me.cmdReadStreamflow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdReadStreamflow.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdReadStreamflow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReadStreamflow.Location = New System.Drawing.Point(18, 98)
        Me.cmdReadStreamflow.Name = "cmdReadStreamflow"
        Me.cmdReadStreamflow.Size = New System.Drawing.Size(135, 28)
        Me.cmdReadStreamflow.TabIndex = 60
        Me.cmdReadStreamflow.Text = "Generate Plot"
        '
        'gbxBankfull
        '
        Me.gbxBankfull.Controls.Add(Me.cmdBankfullNext)
        Me.gbxBankfull.Location = New System.Drawing.Point(15, 32)
        Me.gbxBankfull.Name = "gbxBankfull"
        Me.gbxBankfull.Size = New System.Drawing.Size(163, 100)
        Me.gbxBankfull.TabIndex = 63
        Me.gbxBankfull.TabStop = False
        Me.gbxBankfull.Text = "Bankfull and Flow Statistics"
        '
        'cmdBankfullNext
        '
        Me.cmdBankfullNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBankfullNext.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdBankfullNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBankfullNext.Location = New System.Drawing.Point(20, 49)
        Me.cmdBankfullNext.Name = "cmdBankfullNext"
        Me.cmdBankfullNext.Size = New System.Drawing.Size(124, 28)
        Me.cmdBankfullNext.TabIndex = 62
        Me.cmdBankfullNext.Text = "Generate Statistics"
        '
        'gbxMapper
        '
        Me.gbxMapper.Controls.Add(Me.gbxMapDates)
        Me.gbxMapper.Controls.Add(Me.gbxMap)
        Me.gbxMapper.Controls.Add(Me.cmdMapGenerate)
        Me.gbxMapper.Location = New System.Drawing.Point(193, 32)
        Me.gbxMapper.Name = "gbxMapper"
        Me.gbxMapper.Size = New System.Drawing.Size(252, 254)
        Me.gbxMapper.TabIndex = 61
        Me.gbxMapper.TabStop = False
        Me.gbxMapper.Text = "Flow Percentile Map"
        '
        'gbxMapDates
        '
        Me.gbxMapDates.Controls.Add(Me.lblMapDay)
        Me.gbxMapDates.Controls.Add(Me.lblMapMonth)
        Me.gbxMapDates.Controls.Add(Me.lblMapYear)
        Me.gbxMapDates.Controls.Add(Me.AtxMapDay)
        Me.gbxMapDates.Controls.Add(Me.atxMapYear)
        Me.gbxMapDates.Controls.Add(Me.AtxMapMonth)
        Me.gbxMapDates.Location = New System.Drawing.Point(21, 115)
        Me.gbxMapDates.Name = "gbxMapDates"
        Me.gbxMapDates.Size = New System.Drawing.Size(207, 80)
        Me.gbxMapDates.TabIndex = 60
        Me.gbxMapDates.TabStop = False
        Me.gbxMapDates.Text = "Result Date"
        '
        'lblMapDay
        '
        Me.lblMapDay.AutoSize = True
        Me.lblMapDay.Location = New System.Drawing.Point(136, 27)
        Me.lblMapDay.Name = "lblMapDay"
        Me.lblMapDay.Size = New System.Drawing.Size(26, 13)
        Me.lblMapDay.TabIndex = 35
        Me.lblMapDay.Text = "Day"
        '
        'lblMapMonth
        '
        Me.lblMapMonth.AutoSize = True
        Me.lblMapMonth.Location = New System.Drawing.Point(87, 27)
        Me.lblMapMonth.Name = "lblMapMonth"
        Me.lblMapMonth.Size = New System.Drawing.Size(37, 13)
        Me.lblMapMonth.TabIndex = 34
        Me.lblMapMonth.Text = "Month"
        '
        'lblMapYear
        '
        Me.lblMapYear.AutoSize = True
        Me.lblMapYear.Location = New System.Drawing.Point(19, 27)
        Me.lblMapYear.Name = "lblMapYear"
        Me.lblMapYear.Size = New System.Drawing.Size(29, 13)
        Me.lblMapYear.TabIndex = 33
        Me.lblMapYear.Text = "Year"
        '
        'AtxMapDay
        '
        Me.AtxMapDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtxMapDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtxMapDay.DefaultValue = ""
        Me.AtxMapDay.HardMax = 31
        Me.AtxMapDay.HardMin = 1
        Me.AtxMapDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtxMapDay.Location = New System.Drawing.Point(139, 45)
        Me.AtxMapDay.MaxWidth = 20
        Me.AtxMapDay.Name = "AtxMapDay"
        Me.AtxMapDay.NumericFormat = "0"
        Me.AtxMapDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtxMapDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtxMapDay.SelLength = 0
        Me.AtxMapDay.SelStart = 0
        Me.AtxMapDay.Size = New System.Drawing.Size(44, 21)
        Me.AtxMapDay.SoftMax = -999
        Me.AtxMapDay.SoftMin = -999
        Me.AtxMapDay.TabIndex = 31
        Me.AtxMapDay.ValueDouble = 1
        Me.AtxMapDay.ValueInteger = 1
        '
        'atxMapYear
        '
        Me.atxMapYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxMapYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxMapYear.DefaultValue = ""
        Me.atxMapYear.HardMax = 9999
        Me.atxMapYear.HardMin = 0
        Me.atxMapYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxMapYear.Location = New System.Drawing.Point(21, 45)
        Me.atxMapYear.MaxWidth = 20
        Me.atxMapYear.Name = "atxMapYear"
        Me.atxMapYear.NumericFormat = "0"
        Me.atxMapYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxMapYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxMapYear.SelLength = 0
        Me.atxMapYear.SelStart = 0
        Me.atxMapYear.Size = New System.Drawing.Size(64, 21)
        Me.atxMapYear.SoftMax = -999
        Me.atxMapYear.SoftMin = -999
        Me.atxMapYear.TabIndex = 30
        Me.atxMapYear.ValueDouble = 2000
        Me.atxMapYear.ValueInteger = 2000
        '
        'AtxMapMonth
        '
        Me.AtxMapMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtxMapMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtxMapMonth.DefaultValue = ""
        Me.AtxMapMonth.HardMax = 12
        Me.AtxMapMonth.HardMin = 1
        Me.AtxMapMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtxMapMonth.Location = New System.Drawing.Point(90, 45)
        Me.AtxMapMonth.MaxWidth = 20
        Me.AtxMapMonth.Name = "AtxMapMonth"
        Me.AtxMapMonth.NumericFormat = "0"
        Me.AtxMapMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtxMapMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtxMapMonth.SelLength = 0
        Me.AtxMapMonth.SelStart = 0
        Me.AtxMapMonth.Size = New System.Drawing.Size(44, 21)
        Me.AtxMapMonth.SoftMax = -999
        Me.AtxMapMonth.SoftMin = -999
        Me.AtxMapMonth.TabIndex = 28
        Me.AtxMapMonth.ValueDouble = 1
        Me.AtxMapMonth.ValueInteger = 1
        '
        'gbxMap
        '
        Me.gbxMap.Controls.Add(Me.rbnSoil)
        Me.gbxMap.Controls.Add(Me.rbnStreamflow)
        Me.gbxMap.Location = New System.Drawing.Point(21, 30)
        Me.gbxMap.Name = "gbxMap"
        Me.gbxMap.Size = New System.Drawing.Size(159, 70)
        Me.gbxMap.TabIndex = 59
        Me.gbxMap.TabStop = False
        Me.gbxMap.Text = "Result File"
        '
        'rbnSoil
        '
        Me.rbnSoil.AutoSize = True
        Me.rbnSoil.Location = New System.Drawing.Point(7, 42)
        Me.rbnSoil.Name = "rbnSoil"
        Me.rbnSoil.Size = New System.Drawing.Size(74, 17)
        Me.rbnSoil.TabIndex = 2
        Me.rbnSoil.Text = "Soil Water"
        Me.rbnSoil.UseVisualStyleBackColor = True
        '
        'rbnStreamflow
        '
        Me.rbnStreamflow.AutoSize = True
        Me.rbnStreamflow.Checked = True
        Me.rbnStreamflow.Location = New System.Drawing.Point(7, 19)
        Me.rbnStreamflow.Name = "rbnStreamflow"
        Me.rbnStreamflow.Size = New System.Drawing.Size(83, 17)
        Me.rbnStreamflow.TabIndex = 1
        Me.rbnStreamflow.TabStop = True
        Me.rbnStreamflow.Text = "Stream Flow"
        Me.rbnStreamflow.UseVisualStyleBackColor = True
        '
        'cmdMapGenerate
        '
        Me.cmdMapGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdMapGenerate.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdMapGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMapGenerate.Location = New System.Drawing.Point(68, 210)
        Me.cmdMapGenerate.Name = "cmdMapGenerate"
        Me.cmdMapGenerate.Size = New System.Drawing.Size(103, 28)
        Me.cmdMapGenerate.TabIndex = 41
        Me.cmdMapGenerate.Text = "Generate Map"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 458)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(656, 48)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Status"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(13, 21)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(631, 14)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(27, 362)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(137, 13)
        Me.Label20.TabIndex = 46
        Me.Label20.Text = "Max Impervious Cover Grid:"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox14
        '
        Me.ComboBox14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox14.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox14.Location = New System.Drawing.Point(183, 359)
        Me.ComboBox14.Name = "ComboBox14"
        Me.ComboBox14.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox14.TabIndex = 47
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(27, 335)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(134, 13)
        Me.Label21.TabIndex = 44
        Me.Label21.Text = "Downstream Basin ID Grid:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox15
        '
        Me.ComboBox15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox15.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox15.Location = New System.Drawing.Point(183, 333)
        Me.ComboBox15.Name = "ComboBox15"
        Me.ComboBox15.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox15.TabIndex = 45
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(27, 309)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(88, 13)
        Me.Label22.TabIndex = 42
        Me.Label22.Text = "Stream Link Grid:"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox16
        '
        Me.ComboBox16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox16.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox16.Location = New System.Drawing.Point(183, 306)
        Me.ComboBox16.Name = "ComboBox16"
        Me.ComboBox16.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox16.TabIndex = 43
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(27, 282)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(152, 13)
        Me.Label23.TabIndex = 40
        Me.Label23.Text = "Downstream Flow Length Grid:"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox17
        '
        Me.ComboBox17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox17.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox17.Location = New System.Drawing.Point(183, 279)
        Me.ComboBox17.Name = "ComboBox17"
        Me.ComboBox17.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox17.TabIndex = 41
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(27, 255)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(137, 13)
        Me.Label24.TabIndex = 38
        Me.Label24.Text = "Hydraulic Conductivity Grid:"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox18
        '
        Me.ComboBox18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox18.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox18.Location = New System.Drawing.Point(183, 252)
        Me.ComboBox18.Name = "ComboBox18"
        Me.ComboBox18.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox18.TabIndex = 39
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(27, 228)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(88, 13)
        Me.Label25.TabIndex = 36
        Me.Label25.Text = "Soil Texture Grid:"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox19
        '
        Me.ComboBox19.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox19.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox19.Location = New System.Drawing.Point(183, 225)
        Me.ComboBox19.Name = "ComboBox19"
        Me.ComboBox19.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox19.TabIndex = 37
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(27, 201)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(81, 13)
        Me.Label26.TabIndex = 34
        Me.Label26.Text = "Soil Depth Grid:"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox20
        '
        Me.ComboBox20.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox20.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox20.Location = New System.Drawing.Point(183, 198)
        Me.ComboBox20.Name = "ComboBox20"
        Me.ComboBox20.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox20.TabIndex = 35
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(27, 174)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(144, 13)
        Me.Label27.TabIndex = 32
        Me.Label27.Text = "Water Holding Capacity Grid:"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox21
        '
        Me.ComboBox21.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox21.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox21.Location = New System.Drawing.Point(183, 171)
        Me.ComboBox21.Name = "ComboBox21"
        Me.ComboBox21.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox21.TabIndex = 33
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(27, 147)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(135, 13)
        Me.Label28.TabIndex = 30
        Me.Label28.Text = "Runoff Curve Number Grid:"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox22
        '
        Me.ComboBox22.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox22.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox22.Location = New System.Drawing.Point(183, 144)
        Me.ComboBox22.Name = "ComboBox22"
        Me.ComboBox22.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox22.TabIndex = 31
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(27, 120)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(82, 13)
        Me.Label29.TabIndex = 28
        Me.Label29.Text = "Hill Length Grid:"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox23
        '
        Me.ComboBox23.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox23.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox23.Location = New System.Drawing.Point(183, 117)
        Me.ComboBox23.Name = "ComboBox23"
        Me.ComboBox23.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox23.TabIndex = 29
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(27, 92)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(121, 13)
        Me.Label30.TabIndex = 26
        Me.Label30.Text = "Flow Accumulation Grid:"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox24
        '
        Me.ComboBox24.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox24.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox24.Location = New System.Drawing.Point(183, 90)
        Me.ComboBox24.Name = "ComboBox24"
        Me.ComboBox24.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox24.TabIndex = 27
        '
        'ComboBox25
        '
        Me.ComboBox25.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox25.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox25.Location = New System.Drawing.Point(183, 36)
        Me.ComboBox25.Name = "ComboBox25"
        Me.ComboBox25.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox25.TabIndex = 25
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(27, 38)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(58, 13)
        Me.Label31.TabIndex = 24
        Me.Label31.Text = "Basin Grid:"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(27, 65)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(87, 13)
        Me.Label32.TabIndex = 20
        Me.Label32.Text = "Processed DEM:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox26
        '
        Me.ComboBox26.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox26.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox26.Location = New System.Drawing.Point(183, 63)
        Me.ComboBox26.Name = "ComboBox26"
        Me.ComboBox26.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox26.TabIndex = 23
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(27, 362)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(137, 13)
        Me.Label33.TabIndex = 46
        Me.Label33.Text = "Max Impervious Cover Grid:"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox27
        '
        Me.ComboBox27.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox27.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox27.Location = New System.Drawing.Point(183, 359)
        Me.ComboBox27.Name = "ComboBox27"
        Me.ComboBox27.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox27.TabIndex = 47
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(27, 335)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(134, 13)
        Me.Label34.TabIndex = 44
        Me.Label34.Text = "Downstream Basin ID Grid:"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox28
        '
        Me.ComboBox28.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox28.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox28.Location = New System.Drawing.Point(183, 333)
        Me.ComboBox28.Name = "ComboBox28"
        Me.ComboBox28.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox28.TabIndex = 45
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(27, 309)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(88, 13)
        Me.Label35.TabIndex = 42
        Me.Label35.Text = "Stream Link Grid:"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox29
        '
        Me.ComboBox29.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox29.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox29.Location = New System.Drawing.Point(183, 306)
        Me.ComboBox29.Name = "ComboBox29"
        Me.ComboBox29.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox29.TabIndex = 43
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(27, 282)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(152, 13)
        Me.Label36.TabIndex = 40
        Me.Label36.Text = "Downstream Flow Length Grid:"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox30
        '
        Me.ComboBox30.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox30.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox30.Location = New System.Drawing.Point(183, 279)
        Me.ComboBox30.Name = "ComboBox30"
        Me.ComboBox30.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox30.TabIndex = 41
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(27, 255)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(137, 13)
        Me.Label37.TabIndex = 38
        Me.Label37.Text = "Hydraulic Conductivity Grid:"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox31
        '
        Me.ComboBox31.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox31.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox31.Location = New System.Drawing.Point(183, 252)
        Me.ComboBox31.Name = "ComboBox31"
        Me.ComboBox31.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox31.TabIndex = 39
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(27, 228)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(88, 13)
        Me.Label38.TabIndex = 36
        Me.Label38.Text = "Soil Texture Grid:"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox32
        '
        Me.ComboBox32.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox32.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox32.Location = New System.Drawing.Point(183, 225)
        Me.ComboBox32.Name = "ComboBox32"
        Me.ComboBox32.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox32.TabIndex = 37
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(27, 201)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(81, 13)
        Me.Label39.TabIndex = 34
        Me.Label39.Text = "Soil Depth Grid:"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox33
        '
        Me.ComboBox33.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox33.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox33.Location = New System.Drawing.Point(183, 198)
        Me.ComboBox33.Name = "ComboBox33"
        Me.ComboBox33.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox33.TabIndex = 35
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(27, 174)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(144, 13)
        Me.Label40.TabIndex = 32
        Me.Label40.Text = "Water Holding Capacity Grid:"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox34
        '
        Me.ComboBox34.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox34.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox34.Location = New System.Drawing.Point(183, 171)
        Me.ComboBox34.Name = "ComboBox34"
        Me.ComboBox34.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox34.TabIndex = 33
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(27, 147)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(135, 13)
        Me.Label41.TabIndex = 30
        Me.Label41.Text = "Runoff Curve Number Grid:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox35
        '
        Me.ComboBox35.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox35.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox35.Location = New System.Drawing.Point(183, 144)
        Me.ComboBox35.Name = "ComboBox35"
        Me.ComboBox35.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox35.TabIndex = 31
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(27, 120)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(82, 13)
        Me.Label42.TabIndex = 28
        Me.Label42.Text = "Hill Length Grid:"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox36
        '
        Me.ComboBox36.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox36.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox36.Location = New System.Drawing.Point(183, 117)
        Me.ComboBox36.Name = "ComboBox36"
        Me.ComboBox36.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox36.TabIndex = 29
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(27, 92)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(121, 13)
        Me.Label43.TabIndex = 26
        Me.Label43.Text = "Flow Accumulation Grid:"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox37
        '
        Me.ComboBox37.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox37.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox37.Location = New System.Drawing.Point(183, 90)
        Me.ComboBox37.Name = "ComboBox37"
        Me.ComboBox37.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox37.TabIndex = 27
        '
        'ComboBox38
        '
        Me.ComboBox38.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox38.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox38.Location = New System.Drawing.Point(183, 36)
        Me.ComboBox38.Name = "ComboBox38"
        Me.ComboBox38.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox38.TabIndex = 25
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(27, 38)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(58, 13)
        Me.Label44.TabIndex = 24
        Me.Label44.Text = "Basin Grid:"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(27, 65)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(87, 13)
        Me.Label45.TabIndex = 20
        Me.Label45.Text = "Processed DEM:"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboBox39
        '
        Me.ComboBox39.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox39.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox39.Location = New System.Drawing.Point(183, 63)
        Me.ComboBox39.Name = "ComboBox39"
        Me.ComboBox39.Size = New System.Drawing.Size(312, 21)
        Me.ComboBox39.TabIndex = 23
        '
        'frmGeoSFM
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(686, 559)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmGeoSFM"
        Me.Text = "Geospatial Stream Flow Model (GeoSFM) for BASINS"
        Me.tabMain.ResumeLayout(False)
        Me.tpgTerrain.ResumeLayout(False)
        Me.tpgTerrain.PerformLayout()
        Me.tpgBasin.ResumeLayout(False)
        Me.tpgBasin.PerformLayout()
        Me.tpgResponse.ResumeLayout(False)
        Me.tpgResponse.PerformLayout()
        Me.tpgRain.ResumeLayout(False)
        Me.gbxSimulationDates.ResumeLayout(False)
        Me.gbxSimulationDates.PerformLayout()
        Me.tpgSoil.ResumeLayout(False)
        Me.tpgSoil.PerformLayout()
        Me.gbxFlow.ResumeLayout(False)
        Me.gbxFlow.PerformLayout()
        Me.gbxMode.ResumeLayout(False)
        Me.gbxMode.PerformLayout()
        Me.gbxRun.ResumeLayout(False)
        Me.gbxRun.PerformLayout()
        Me.gbxData.ResumeLayout(False)
        Me.gbxData.PerformLayout()
        Me.tpgFlow.ResumeLayout(False)
        Me.tpgFlow.PerformLayout()
        Me.gbxStreamFlow.ResumeLayout(False)
        Me.gbxStreamFlow.PerformLayout()
        Me.tpgSensitivity.ResumeLayout(False)
        Me.tpgSensitivity.PerformLayout()
        Me.tpgCalibrate.ResumeLayout(False)
        Me.tpgCalibrate.PerformLayout()
        Me.gbxObjective.ResumeLayout(False)
        Me.gbxObjective.PerformLayout()
        Me.gbxConnect.ResumeLayout(False)
        Me.tpgMap.ResumeLayout(False)
        Me.gbxHydrographs.ResumeLayout(False)
        Me.gbxHydrographs.PerformLayout()
        Me.gbxBankfull.ResumeLayout(False)
        Me.gbxMapper.ResumeLayout(False)
        Me.gbxMapDates.ResumeLayout(False)
        Me.gbxMapDates.PerformLayout()
        Me.gbxMap.ResumeLayout(False)
        Me.gbxMap.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend pPrecStations As atcCollection
    Friend pMetStations As atcCollection
    Friend pFlowStations As atcCollection

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("GeoSFM for BASINS/MapWindow" & vbCrLf & vbCrLf & "Version 1.01", MsgBoxStyle.OkOnly, "BASINS GeoSFM")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\GeoSFM.html")
    End Sub

    Private Sub frmGeoSFM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\GeoSFM.html")
        End If
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        cmdCancel.Enabled = aEnabled
        cmdHelp.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        If Not GisUtil.ProjectFileName Is Nothing Then
            atxModel.Text = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)
        End If

        'set choices for dem layer
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.Grid Then
                cboDEM.Items.Add(lLayerName)
                'If GisUtil.LayerFileName(lLayerIndex).IndexOf("\demg\") >= 0 Or GisUtil.LayerFileName(lLayerIndex).IndexOf("\dem\") >= 0 Then
                'cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                If GisUtil.LayerName(lLayerIndex) = "Digital Elevation Model" Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                ElseIf GisUtil.LayerFileName(lLayerIndex).IndexOf("\ned\") >= 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                End If
            End If
        Next lLayerIndex
        If cboDEM.SelectedIndex < 0 And cboDEM.Items.Count > 0 Then
            cboDEM.SelectedIndex = 0
        End If

        'set reach layer
        cboReach.Items.Add("<none>")
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.LineShapefile Then
                cboReach.Items.Add(GisUtil.LayerName(lLayerIndex))
                'If GisUtil.LayerFileName(lLayerIndex).IndexOf("\nhd\") >= 0 Then
                'cboReach.SelectedIndex = cboReach.Items.Count - 1
                'End If
            End If
        Next lLayerIndex
        If cboReach.SelectedIndex = -1 Then
            cboReach.SelectedIndex = 0
        End If

        'set subbasin layer
        cboSubbasin.Items.Add("<none>")
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                cboSubbasin.Items.Add(GisUtil.LayerName(lLayerIndex))
                'If GisUtil.CurrentLayer = lLayerIndex Then 'this is the current layer
                'cboSubbasin.SelectedIndex = cboSubbasin.Items.Count - 1
                'End If
            End If
        Next lLayerIndex
        'If cboSubbasin.SelectedIndex = -1 Then 'make a guess
        'cboSubbasin.SelectedIndex = cboSubbasin.Items.IndexOf("Cataloging Unit Boundaries")
        'End If
        If cboSubbasin.SelectedIndex = -1 Then
            cboSubbasin.SelectedIndex = 0
        End If

        'set prec/evap grid
        With AtcGridPrec
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        pPrecStations = New atcCollection
        pMetStations = New atcCollection
        pFlowStations = New atcCollection

        Logger.Dbg("SetValidValues")
        Dim lValidValues As New atcCollection
        For Each lPrecStation As StationDetails In pPrecStations
            lValidValues.Add(lPrecStation.Description)
        Next
        AtcGridPrec.ValidValues = lValidValues
        AtcGridPrec.SizeAllColumnsToContents()
        AtcGridPrec.Refresh()
        Logger.Dbg("PrecipStationGrid refreshed")

        'set manning's grid
        With AtcGridMannings
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With AtcGridMannings.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .Rows = 25
            .CellValue(0, 0) = "Land Cover"
            .CellValue(0, 1) = "Mannings (Velocity) Coefficient"
            .CellValue(1, 0) = "Urban and Built-Up Land"
            .CellValue(2, 0) = "Dryland Cropland and Pasture"
            .CellValue(3, 0) = "Irrigated Cropland and Pasture"
            .CellValue(4, 0) = "Mixed Dryland/Irrigated Cropland and Pasture"
            .CellValue(5, 0) = "Cropland/Grassland Mosaic"
            .CellValue(6, 0) = "Cropland/Woodland Mosaic"
            .CellValue(7, 0) = "Grassland"
            .CellValue(8, 0) = "Shrubland"
            .CellValue(9, 0) = "Mixed Shrubland/Grassland"
            .CellValue(10, 0) = "Savanna"
            .CellValue(11, 0) = "Deciduous Broadleaf Forest"
            .CellValue(12, 0) = "Deciduous Needleleaf Forest"
            .CellValue(13, 0) = "Evergreen Broadleaf Forest"
            .CellValue(14, 0) = "Evergreen Needleleaf Forest"
            .CellValue(15, 0) = "Mixed Forest"
            .CellValue(16, 0) = "Water Bodies"
            .CellValue(17, 0) = "Herbaceous Wetland"
            .CellValue(18, 0) = "Wooded Wetland"
            .CellValue(19, 0) = "Barren or Sparsely Vegetated"
            .CellValue(20, 0) = "Herbaceous Tundra"
            .CellValue(21, 0) = "Wooded Tundra"
            .CellValue(22, 0) = "Mixed Tundra"
            .CellValue(23, 0) = "Bare Ground Tundra"
            .CellValue(24, 0) = "Snow or Ice"
            .CellValue(1, 1) = "0.03"
            .CellValue(2, 1) = "0.03"
            .CellValue(3, 1) = "0.035"
            .CellValue(4, 1) = "0.033"
            .CellValue(5, 1) = "0.035"
            .CellValue(6, 1) = "0.04"
            .CellValue(7, 1) = "0.05"
            .CellValue(8, 1) = "0.05"
            .CellValue(9, 1) = "0.05"
            .CellValue(10, 1) = "0.06"
            .CellValue(11, 1) = "0.1"
            .CellValue(12, 1) = "0.1"
            .CellValue(13, 1) = "0.12"
            .CellValue(14, 1) = "0.12"
            .CellValue(15, 1) = "0.1"
            .CellValue(16, 1) = "0.035"
            .CellValue(17, 1) = "0.05"
            .CellValue(18, 1) = "0.05"
            .CellValue(19, 1) = "0.03"
            .CellValue(20, 1) = "0.05"
            .CellValue(21, 1) = "0.05"
            .CellValue(22, 1) = "0.05"
            .CellValue(23, 1) = "0.04"
            .CellValue(24, 1) = "0.04"
            For lIndex As Integer = 1 To 24
                .CellColor(lIndex, 0) = SystemColors.ControlDark
                .CellEditable(lIndex, 1) = True
            Next
        End With
        AtcGridMannings.SizeAllColumnsToContents()
        AtcGridMannings.Refresh()

        'set sensitivity grid
        With AtcGridSensitivity
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        With AtcGridSensitivity.Source
            .Columns = 3
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .Rows = 21
            .CellValue(0, 0) = "Name/Description"
            .CellValue(0, 1) = "Min Multiplier"
            .CellValue(0, 2) = "Max Multiplier"
            .CellValue(1, 0) = "SoilWhc, Soil water holding capacity (mm)"
            .CellValue(2, 0) = "Depth, Total soil depth (cm)"
            .CellValue(3, 0) = "Texture, Soil texture: 1=Sand 2=Loam 3=Clay 5=Water"
            .CellValue(4, 0) = "Ks, Saturated hydraulic conductivity (cm/hr)"
            .CellValue(5, 0) = "Interflow, Interflow storage residence time (days)"
            .CellValue(6, 0) = "HSlope, Average subbasin slope"
            .CellValue(7, 0) = "Baseflow, Baseflow reservoir residence time (days)"
            .CellValue(8, 0) = "CurveNum, SCS runoff curve number"
            .CellValue(9, 0) = "MaxCover, Permanently impervious cover fraction"
            .CellValue(10, 0) = "BasinLoss, Fraction of soil water to ground water"
            .CellValue(11, 0) = "PanCoeff, Pan coefficient for correcting PET readings"
            .CellValue(12, 0) = "TopSoil, Fraction of soil layer hydrologically active"
            .CellValue(13, 0) = "RainCalc, Excess mode 1=Philip 2=SCS 3=BucketModel"
            .CellValue(14, 0) = "RivRough, Channel Roughness (Manning n)"
            .CellValue(15, 0) = "RivSlope, Average slope of the river"
            .CellValue(16, 0) = "RivWidth, Average channel width (m)"
            .CellValue(17, 0) = "RivLoss, Fraction of flow lost within river channel"
            .CellValue(18, 0) = "RivFPLoss, Fraction of river flow lost in floodplain"
            .CellValue(19, 0) = "Celerity, Flood wave celerity (m/s)"
            .CellValue(20, 0) = "Diffusion, Flow attenuation coefficient (m^2/s)"
            .CellValue(1, 1) = "1"
            .CellValue(2, 1) = "1"
            .CellValue(3, 1) = "1"
            .CellValue(4, 1) = "0.001"
            .CellValue(5, 1) = "1"
            .CellValue(6, 1) = "0.001"
            .CellValue(7, 1) = "1"
            .CellValue(8, 1) = "25"
            .CellValue(9, 1) = "0.01"
            .CellValue(10, 1) = "0.001"
            .CellValue(11, 1) = "0.6"
            .CellValue(12, 1) = "0.05"
            .CellValue(13, 1) = "1"
            .CellValue(14, 1) = "0.012"
            .CellValue(15, 1) = "0.001"
            .CellValue(16, 1) = "30"
            .CellValue(17, 1) = "0.01"
            .CellValue(18, 1) = "0.01"
            .CellValue(19, 1) = "0.1"
            .CellValue(20, 1) = "100"
            .CellValue(1, 2) = "600"
            .CellValue(2, 2) = "800"
            .CellValue(3, 2) = "5"
            .CellValue(4, 2) = "150"
            .CellValue(5, 2) = "365"
            .CellValue(6, 2) = "10"
            .CellValue(7, 2) = "365"
            .CellValue(8, 2) = "98"
            .CellValue(9, 2) = "1"
            .CellValue(10, 2) = "0.5"
            .CellValue(11, 2) = "0.95"
            .CellValue(12, 2) = "1"
            .CellValue(13, 2) = "3"
            .CellValue(14, 2) = "0.07"
            .CellValue(15, 2) = "10"
            .CellValue(16, 2) = "1000"
            .CellValue(17, 2) = "0.5"
            .CellValue(18, 2) = "0.5"
            .CellValue(19, 2) = "5"
            .CellValue(20, 2) = "10000"
            For lIndex As Integer = 1 To 20
                .CellColor(lIndex, 0) = SystemColors.ControlDark
                .CellEditable(lIndex, 1) = True
                .CellEditable(lIndex, 2) = True
            Next
        End With
        AtcGridSensitivity.SizeAllColumnsToContents()
        AtcGridSensitivity.Refresh()

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        EnableControls(True)

        DefaultBasinCharacteristicsGrids()
        DefaultResponseGrids()
    End Sub

    Private Sub lblStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStatus.TextChanged
        Logger.Dbg(lblStatus.Text)
    End Sub

    Private Sub cmdTerrainNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTerrainNext.Click
        SetOutputPath(atxModel.Text)

        If cboDEM.SelectedIndex = -1 Then
            Logger.Msg("No DEM has been selected." & vbCrLf & "Select a DEM to proceed.", "Geospatial Stream Flow Model")
            Exit Sub
        End If
        Dim lDEMLayerName As String = cboDEM.Items(cboDEM.SelectedIndex)
        Dim lSubbasinLayerName As String = cboSubbasin.Items(cboSubbasin.SelectedIndex)
        Dim lStreamLayerName As String = cboReach.Items(cboReach.SelectedIndex)
        Dim lThresh As Integer = AtcText1.ValueInteger
        EnableControls(False)
        lblStatus.Text = "Performing Terrain Analysis ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        'call what was the avenue script 'terrain'
        If Terrain(lDEMLayerName, lSubbasinLayerName, lStreamLayerName, lThresh) Then
            tabMain.SelectedIndex = 1
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
        DefaultBasinCharacteristicsGrids()
    End Sub

    Private Sub DefaultBasinCharacteristicsGrids()
        'set choices for grid layers
        cboBC1.Items.Clear()
        cboBC2.Items.Clear()
        cboBC3.Items.Clear()
        cboBC4.Items.Clear()
        cboBC5.Items.Clear()
        cboBC6.Items.Clear()
        cboBC7.Items.Clear()
        cboBC8.Items.Clear()
        cboBC9.Items.Clear()
        cboBC10.Items.Clear()
        cboBC11.Items.Clear()
        cboBC12.Items.Clear()
        cboBC13.Items.Clear()

        cboBC1.Items.Add("<none>")
        cboBC2.Items.Add("<none>")
        cboBC3.Items.Add("<none>")
        cboBC4.Items.Add("<none>")
        cboBC5.Items.Add("<none>")
        cboBC6.Items.Add("<none>")
        cboBC7.Items.Add("<none>")
        cboBC8.Items.Add("<none>")
        cboBC9.Items.Add("<none>")
        cboBC10.Items.Add("<none>")
        cboBC11.Items.Add("<none>")
        cboBC12.Items.Add("<none>")
        cboBC13.Items.Add("<none>")

        cboBC1.SelectedIndex = 0
        cboBC2.SelectedIndex = 0
        cboBC3.SelectedIndex = 0
        cboBC4.SelectedIndex = 0
        cboBC5.SelectedIndex = 0
        cboBC6.SelectedIndex = 0
        cboBC7.SelectedIndex = 0
        cboBC8.SelectedIndex = 0
        cboBC9.SelectedIndex = 0
        cboBC10.SelectedIndex = 0
        cboBC11.SelectedIndex = 0
        cboBC12.SelectedIndex = 0
        cboBC13.SelectedIndex = 0

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.Grid Then
                cboBC1.Items.Add(lLayerName)
                cboBC2.Items.Add(lLayerName)
                cboBC3.Items.Add(lLayerName)
                cboBC4.Items.Add(lLayerName)
                cboBC5.Items.Add(lLayerName)
                cboBC6.Items.Add(lLayerName)
                cboBC7.Items.Add(lLayerName)
                cboBC8.Items.Add(lLayerName)
                cboBC9.Items.Add(lLayerName)
                cboBC10.Items.Add(lLayerName)
                cboBC11.Items.Add(lLayerName)
                cboBC12.Items.Add(lLayerName)
                cboBC13.Items.Add(lLayerName)

                If lLayerName = "Basin Grid" Or lLayerName = "Subbasin Grid" Then
                    cboBC1.SelectedIndex = cboBC1.Items.Count - 1
                End If
                If lLayerName = "Processed DEM" Or lLayerName = "Corrected DEM" Then
                    cboBC2.SelectedIndex = cboBC2.Items.Count - 1
                End If
                If lLayerName = "Flow Accumulation Grid" Then
                    cboBC3.SelectedIndex = cboBC3.Items.Count - 1
                End If
                If lLayerName = "Hill Length Grid" Then
                    cboBC4.SelectedIndex = cboBC4.Items.Count - 1
                End If
                If lLayerName = "Runoff Curve Number Grid" Or lLayerName = "rcn" Then
                    cboBC5.SelectedIndex = cboBC5.Items.Count - 1
                End If
                If lLayerName = "Water Holding Capacity" Or lLayerName = "whc" Then
                    cboBC6.SelectedIndex = cboBC6.Items.Count - 1
                End If
                If lLayerName = "Soil Depth Grid" Or lLayerName.Contains("soildepth") Then
                    cboBC7.SelectedIndex = cboBC7.Items.Count - 1
                End If
                If lLayerName = "Soil Texture Grid" Or lLayerName.Contains("texture") Then
                    cboBC8.SelectedIndex = cboBC8.Items.Count - 1
                End If
                If lLayerName = "Hydraulic Conductivity Grid" Or lLayerName = "ks" Then
                    cboBC9.SelectedIndex = cboBC9.Items.Count - 1
                End If
                If lLayerName = "Downstream Flow Length Grid" Or lLayerName = "Downstream Flow Length" Then
                    cboBC10.SelectedIndex = cboBC10.Items.Count - 1
                End If
                If lLayerName = "Stream Link Grid" Then
                    cboBC11.SelectedIndex = cboBC11.Items.Count - 1
                End If
                If lLayerName = "Downstream Basin id Grid" Or lLayerName = "Downstream Subbasin Grid" Then
                    cboBC12.SelectedIndex = cboBC12.Items.Count - 1
                End If
                If lLayerName = "Max Impervious Cover Grid" Or lLayerName = "maxcover" Then
                    cboBC13.SelectedIndex = cboBC13.Items.Count - 1
                End If
            End If
        Next lLayerIndex

        If cboBC1.SelectedIndex = 0 Then
            'cmdBasinNext.Enabled = False
        End If
    End Sub

    Private Sub cmdBasinNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBasinNext.Click
        SetOutputPath(atxModel.Text)

        Dim lZonegname As String = cboBC1.Items(cboBC1.SelectedIndex)
        Dim lDemgname As String = cboBC2.Items(cboBC2.SelectedIndex)
        Dim lFacgname As String = cboBC3.Items(cboBC3.SelectedIndex)
        Dim lHlengname As String = cboBC4.Items(cboBC4.SelectedIndex)
        Dim lRcngname As String = cboBC5.Items(cboBC5.SelectedIndex)
        Dim lWhcgname As String = cboBC6.Items(cboBC6.SelectedIndex)
        Dim lDepthgname As String = cboBC7.Items(cboBC7.SelectedIndex)
        Dim lTexturegname As String = cboBC8.Items(cboBC8.SelectedIndex)
        Dim lDraingname As String = cboBC9.Items(cboBC9.SelectedIndex)
        Dim lFlowlengname As String = cboBC10.Items(cboBC10.SelectedIndex)
        Dim lRivlinkgname As String = cboBC11.Items(cboBC11.SelectedIndex)
        Dim lDowngname As String = cboBC12.Items(cboBC12.SelectedIndex)
        Dim lMaxcovergname As String = cboBC13.Items(cboBC13.SelectedIndex)

        EnableControls(False)
        lblStatus.Text = "Computing Basin Characteristics ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        'call what was the avenue script 'basin'
        If Basin(lZonegname, lDemgname, lFacgname, lHlengname, lRcngname, lWhcgname, _
              lDepthgname, lTexturegname, lDraingname, lFlowlengname, _
              lRivlinkgname, lDowngname, lMaxcovergname) Then
            tabMain.SelectedIndex = 2
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
        DefaultResponseGrids()
    End Sub

    Private Sub DefaultResponseGrids()
        'set choices for grid layers
        cboUSGS.Items.Clear()
        cboOutlets.Items.Clear()
        cboUSGS.Items.Add("<none>")
        cboFlowDir.Items.Add("<none>")
        cboOutlets.Items.Add("<none>")
        cboUSGS.SelectedIndex = 0
        cboFlowDir.SelectedIndex = 0
        cboOutlets.SelectedIndex = 0

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.Grid Then
                cboUSGS.Items.Add(lLayerName)
                cboOutlets.Items.Add(lLayerName)
                cboFlowDir.Items.Add(lLayerName)
                If UCase(lLayerName).Contains("USGS") Then
                    cboUSGS.SelectedIndex = cboUSGS.Items.Count - 1
                End If
                If UCase(lLayerName).Contains("OUTLET") Then
                    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
                End If
                If UCase(lLayerName).Contains("FLOW DIRECTION") Then
                    cboFlowDir.SelectedIndex = cboFlowDir.Items.Count - 1
                End If
            End If
        Next lLayerIndex

        If cboUSGS.SelectedIndex = 0 Or cboOutlets.SelectedIndex = 0 Then
            'cmdResponseNext.Enabled = False
        End If
    End Sub

    Private Sub cmdResponseNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdResponseNext.Click
        SetOutputPath(atxModel.Text)

        Dim lVelMethod As Integer = 0
        If rbnNonUniformUSGS.Checked Then
            lVelMethod = 1
        ElseIf rbnNonUniformUser.Checked Then
            lVelMethod = 2
        ElseIf rbnUniform.Checked Then
            lVelMethod = 3
        End If

        Dim lZonegname As String = cboBC1.Items(cboBC1.SelectedIndex)
        Dim lDemgname As String = cboBC2.Items(cboBC2.SelectedIndex)
        Dim lFacgname As String = cboBC3.Items(cboBC3.SelectedIndex)
        Dim lFlowlengname As String = cboBC10.Items(cboBC10.SelectedIndex)
        Dim lUSGSLandcoverGridName As String = cboUSGS.Items(cboUSGS.SelectedIndex)
        Dim lFlowDirGridName As String = cboFlowDir.Items(cboFlowDir.SelectedIndex)
        Dim lOutletGridName As String = cboOutlets.Items(cboOutlets.SelectedIndex)

        Dim lOverlandFlowVelocity As Double = atxOverland.ValueDouble
        Dim lInstreamFlowVelocity As Double = atxInstream.ValueDouble

        Dim lManningsFactors As New atcCollection
        'Dim lLuCodes() As Integer = {100, 211, 212, 213, 280, 290, 311, 321, 330, 332, 411, 412, 421, 422, 430, 500, 620, 610, 770, 820, 810, 850, 830, 900}
        Dim lLuCodes() As Integer = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
        With AtcGridMannings.Source
            For lIndex As Integer = 1 To 24
                lManningsFactors.Add(lLuCodes(lIndex - 1), .CellValue(lIndex, 1))
            Next
        End With

        EnableControls(False)
        lblStatus.Text = "Computing Basin Response ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If Response(lVelMethod, lZonegname, lFlowlengname, lOutletGridName, lDemgname, lFacgname, lFlowDirGridName, _
                 lUSGSLandcoverGridName, lOverlandFlowVelocity, lInstreamFlowVelocity, lManningsFactors) Then
            tabMain.SelectedIndex = 3
            SetPrecipStationGrid()
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub SetPrecipStationGrid()
        If AtcGridPrec.Source Is Nothing Then
            Logger.Dbg("No atcGridPrec")
        Else

            'read order file to get subbasin id list
            Dim OrderFileName As String = pOutputPath & "order.txt"

            Dim lSubbasins As New atcCollection

            If FileExists(OrderFileName) Then
                Try
                    Dim lCurrentRecord As String
                    Dim lStreamReader As New StreamReader(OrderFileName)
                    lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                    Do
                        lCurrentRecord = lStreamReader.ReadLine
                        If lCurrentRecord Is Nothing Then
                            Exit Do
                        Else
                            lSubbasins.Add(lCurrentRecord)
                        End If
                    Loop
                Catch e As ApplicationException
                    Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Basin Characteristics' to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                    Exit Sub
                End Try
            Else
                'Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Basin Characteristics' to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Exit Sub
            End If

            AtcGridPrec.Clear()
            With AtcGridPrec.Source
                .Columns = 3
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellColor(0, 0) = SystemColors.ControlDark
                .CellColor(0, 1) = SystemColors.ControlDark
                .CellColor(0, 2) = SystemColors.ControlDark
                .Rows = 1 + lSubbasins.Count
                .CellValue(0, 0) = "Subbasin ID"
                .CellValue(0, 1) = "Precip Station"
                .CellValue(0, 2) = "Evap Station"
                For lIndex As Integer = 1 To lSubbasins.Count
                    .CellValue(lIndex, 0) = lSubbasins(lIndex - 1)
                    .CellColor(lIndex, 0) = SystemColors.ControlDark
                    If pPrecStations.Count > 0 Then
                        .CellValue(lIndex, 1) = pPrecStations(0).Description
                        .CellEditable(lIndex, 1) = True
                    End If
                    If pMetStations.Count > 0 Then
                        .CellValue(lIndex, 2) = pMetStations(0).Description
                        .CellEditable(lIndex, 2) = True
                    End If
                Next
            End With

            Logger.Dbg("SetValidValues")
            Dim lValidValues As New atcCollection
            For Each lPrecStation As StationDetails In pPrecStations
                lValidValues.Add(lPrecStation.Description)
            Next
            AtcGridPrec.ValidValues = lValidValues
            AtcGridPrec.SizeAllColumnsToContents()
            AtcGridPrec.Refresh()
            Logger.Dbg("PrecipStationGrid refreshed")
            If pPrecStations.Count > 0 Then
                SetDates()
            End If
        End If
    End Sub

    Private Sub SetMetStationValidValues(ByVal aRow As Integer, ByVal aColumn As Integer)
        Logger.Dbg("SetValidValues")
        Dim lValidValues As New atcCollection
        If aColumn = 1 Then
            For Each lPrecStation As StationDetails In pPrecStations
                lValidValues.Add(lPrecStation.Description)
            Next
        ElseIf aColumn = 2 Then
            For Each lMetStation As StationDetails In pMetStations
                lValidValues.Add(lMetStation.Description)
            Next
        End If
        AtcGridPrec.ValidValues = lValidValues
    End Sub

    Private Sub SetDates()
        ''set dates to the last common year of the selected met data        

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0
        Dim lSelectedStation As StationDetails

        'set dates from prec dsn
        For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
            lSelectedStation = pPrecStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 1))
            'set dates
            If lSelectedStation.StartJDate > lSJDate Then
                lSJDate = lSelectedStation.StartJDate
            End If
            If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                lEJDate = lSelectedStation.EndJDate
            End If
        Next

        'change dates to reflect met dsns if different
        For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
            lSelectedStation = pMetStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 2))
            'set dates
            If lSelectedStation.StartJDate > lSJDate Then
                lSJDate = lSelectedStation.StartJDate
            End If
            If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                lEJDate = lSelectedStation.EndJDate
            End If
        Next

        Dim lEDate(5) As Integer, lSDate(5) As Integer
        J2Date(lEJDate, lEDate)
        J2Date(lSJDate, lSDate)

        'set limits
        atxSYear.HardMax = lEDate(0)
        atxSYear.HardMin = lSDate(0)
        atxEYear.HardMax = lEDate(0)
        atxEYear.HardMin = lSDate(0)

        'default to last calendar year of data
        lSDate(0) = lEDate(0) - 1
        lSDate(1) = 1
        lSDate(2) = 1
        lEDate(0) = lSDate(0)
        lEDate(1) = 12
        lEDate(2) = 31
        atxSYear.Text = lSDate(0)
        atxSMonth.Text = lSDate(1)
        atxSDay.Text = lSDate(2)
        atxEYear.Text = lEDate(0)
        atxEMonth.Text = lEDate(1)
        atxEDay.Text = lEDate(2)
    End Sub

    Private Sub cmdRainEvapNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRainEvapNext.Click
        SetOutputPath(atxModel.Text)

        Dim lPrecGageNamesBySubbasin As New Collection
        Dim lEvapGageNamesBySubbasin As New Collection
        Dim lSelectedStation As StationDetails
        Dim lSJMetDate As Double = 0.0
        Dim lEJMetDate As Double = 0.0
        'set precip stations
        For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
            lSelectedStation = pPrecStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 1))
            'set dates
            If lSelectedStation.StartJDate > lSJMetDate Then
                lSJMetDate = lSelectedStation.StartJDate
            End If
            If lEJMetDate = 0.0 Or lSelectedStation.EndJDate < lEJMetDate Then
                lEJMetDate = lSelectedStation.EndJDate
            End If
            'remember which precip gage goes with each catchment
            lPrecGageNamesBySubbasin.Add(lSelectedStation.Name)
        Next
        'set evap stations
        For lrow As Integer = 1 To AtcGridPrec.Source.Rows - 1
            lSelectedStation = pMetStations.ItemByKey(AtcGridPrec.Source.CellValue(lrow, 2))
            'set dates
            If lSelectedStation.StartJDate > lSJMetDate Then
                lSJMetDate = lSelectedStation.StartJDate
            End If
            If lEJMetDate = 0.0 Or lSelectedStation.EndJDate < lEJMetDate Then
                lEJMetDate = lSelectedStation.EndJDate
            End If
            'remember which precip gage goes with each catchment
            lEvapGageNamesBySubbasin.Add(lSelectedStation.Name)
        Next

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0
        Dim lSDate(5) As Integer
        Dim lEDate(5) As Integer
        lSDate(0) = atxSYear.Text
        lSDate(1) = atxSMonth.Text
        lSDate(2) = atxSDay.Text
        lEDate(0) = atxEYear.Text
        lEDate(1) = atxEMonth.Text
        lEDate(2) = atxEDay.Text
        lSJDate = Date2J(lSDate)
        lEJDate = Date2J(lEDate)

        If lSJDate < 1.0 Or lEJDate < 1 Then 'failed date check
            Logger.Msg("The specified start/end dates are invalid.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        If lSJDate > lEJDate Then 'failed date check
            Logger.Msg("The specified starting date is after the ending date.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        If lSJMetDate > lEJMetDate Then 'failed date check
            Logger.Msg("The specified meteorologic stations do not have a common period of record.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        'compare dates from met data with specified start and end dates, make sure they are valid
        If lSJDate < lSJMetDate Or lEJMetDate < lEJDate Then 'failed date check
            Logger.Msg("The specified start/end dates are not within the dates of the specified meteorologic stations.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If

        EnableControls(False)
        lblStatus.Text = "Writing Prec/Evap Data ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If RainEvap(lPrecGageNamesBySubbasin, lEvapGageNamesBySubbasin, lSJDate, lEJDate) Then
            tabMain.SelectedIndex = 4
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdBalanceNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBalanceNext.Click
        SetOutputPath(atxModel.Text)

        Dim lIniFract As Single = atxInitial.Text
        Dim lDformat As Integer = 0
        If rbnDaily.Checked Then
            lDformat = 1
        End If
        Dim lInimode As Integer = 0
        If rbnContinue.Checked Then
            lInimode = 1
        End If
        Dim lRunmode As Integer = 0
        If rbnCal.Checked Then
            lRunmode = 1
        End If
        Dim lBalType As Integer = 1
        If rbnNonlinear.Checked Then
            lBalType = 2
        End If
        Dim lSJDate As Double = 0.0
        Dim lSDate(5) As Integer
        lSDate(0) = atxSYear.Text
        lSDate(1) = atxSMonth.Text
        lSDate(2) = atxSDay.Text
        lSJDate = Date2J(lSDate)

        EnableControls(False)
        lblStatus.Text = "Performing Soil Water Balance ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If Balance(lIniFract, lDformat, lInimode, lRunmode, lBalType, lSJDate) Then
            tabMain.SelectedIndex = 5
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdRouteNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRouteNext.Click
        SetOutputPath(atxModel.Text)

        Dim lForecast As Integer = atxForecast.Text
        Dim lRunmode As Integer = 0
        If rbnCal.Checked Then
            lRunmode = 1
        End If
        Dim lDformat As Integer = 0
        If rbnDaily.Checked Then
            lDformat = 1
        End If
        Dim lRouteMethod As Integer = 2
        If rbnDiffusion.Checked Then
            lRouteMethod = 1
        End If
        If rbnSimple.Checked Then
            lRouteMethod = 3
        End If
        Dim lSJDate As Double = 0.0
        Dim lSDate(5) As Integer
        lSDate(0) = atxSYear.Text
        lSDate(1) = atxSMonth.Text
        lSDate(2) = atxSDay.Text
        lSJDate = Date2J(lSDate)

        EnableControls(False)
        lblStatus.Text = "Performing Flow Routing ..."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If Route(lForecast, lRunmode, lDformat, lRouteMethod, lSJDate) Then
            tabMain.SelectedIndex = 6
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdSensitivityNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSensitivityNext.Click
        SetOutputPath(atxModel.Text)

        Dim lBalType As Integer = 1
        If rbnNonlinear.Checked Then
            lBalType = 2
        End If
        Dim lRouteMethod As Integer = 2
        If rbnDiffusion.Checked Then
            lRouteMethod = 1
        End If
        If rbnSimple.Checked Then
            lRouteMethod = 3
        End If

        Dim lParameterRanges As New atcCollection
        With AtcGridSensitivity.Source
            For lIndex As Integer = 1 To 20
                lParameterRanges.Add(.CellValue(lIndex, 0), .CellValue(lIndex, 1) & "," & .CellValue(lIndex, 2))
            Next
        End With

        EnableControls(False)
        lblStatus.Text = "Performing Sensitivity Analysis........"
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If Sensitivity(cboReachSensitivity.SelectedIndex, lBalType, lRouteMethod, lParameterRanges) Then
            tabMain.SelectedIndex = 7
        End If

        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdCalibrationNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalibrationNext.Click
        SetOutputPath(atxModel.Text)

        'do a lot of checking here to make sure entries are valid
        If AtcConnectFlows.lstConnections.Items.Count = 0 Then
            Logger.Msg("No calibration timeseries / location connections have been specified.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        If lstCalib.SelectedItems.Count = 0 Then
            Logger.Msg("No calibration parameters have been specified.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        If lstMax.SelectedItems.Count = 0 Then
            Logger.Msg("The maximum number of calibration runs must be set.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If

        Dim lFlowGageNames As New atcCollection
        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0
        Dim lSJFlowDate As Double = 0.0
        Dim lEJFlowDate As Double = 0.0
        Dim lSelectedStation As StationDetails
        Dim lSelectedStationName As String = ""
        Dim lReachId As String = ""
        'set flow stations
        For lIndex As Integer = 0 To AtcConnectFlows.lstConnections.Items.Count - 1
            Dim lString() As String = AtcConnectFlows.lstConnections.Items(lIndex).Split(" ")
            lSelectedStationName = lString(0)
            lReachId = lString(2)
            lSelectedStation = pFlowStations.ItemByKey(lSelectedStationName)
            'set dates
            If lSelectedStation.StartJDate > lSJDate Then
                lSJDate = lSelectedStation.StartJDate
            End If
            If lEJDate = 0.0 Or lSelectedStation.EndJDate < lEJDate Then
                lEJDate = lSelectedStation.EndJDate
            End If

            If lSelectedStation.StartJDate > lSJFlowDate Then
                lSJFlowDate = lSelectedStation.StartJDate
            End If
            If lEJFlowDate = 0.0 Or lSelectedStation.EndJDate < lEJFlowDate Then
                lEJFlowDate = lSelectedStation.EndJDate
            End If
            'remember which flow gage goes with each reach segment
            If Not lFlowGageNames.Contains(lReachId) Then
                lFlowGageNames.Add(lReachId, lSelectedStation.Name)
            Else
                'already have this reach, tell user
                Logger.Msg("One reach may not be associated with multiple streamflow gages.", vbOKOnly, "BASINS GeoSFM Problem")
                EnableControls(True)
                Exit Sub
            End If
        Next

        Dim lSDate(5) As Integer
        Dim lEDate(5) As Integer
        lSDate(0) = atxSYear.Text
        lSDate(1) = atxSMonth.Text
        lSDate(2) = atxSDay.Text
        lEDate(0) = atxEYear.Text
        lEDate(1) = atxEMonth.Text
        lEDate(2) = atxEDay.Text
        lSJDate = Date2J(lSDate)
        lEJDate = Date2J(lEDate)

        If lSJFlowDate > lEJFlowDate Then 'failed date check
            Logger.Msg("The specified flow stations do not have a common period of record.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If
        'compare dates from flow data with specified start and end dates, make sure they are valid
        If lSJDate < lSJFlowDate Or lEJFlowDate < lEJDate Then 'failed date check
            Logger.Msg("The specified start/end dates are not within the dates of the specified flow stations.", vbOKOnly, "BASINS GeoSFM Problem")
            EnableControls(True)
            Exit Sub
        End If

        Dim lCalibParms As New Collection
        For lIndex As Integer = 1 To lstCalib.SelectedItems.Count
            lCalibParms.Add(lstCalib.SelectedItems(lIndex - 1))
        Next

        Dim lObjFunction As Integer = 0
        If rbnObj1.Checked Then
            lObjFunction = 1
        ElseIf rbnObj2.Checked Then
            lObjFunction = 2
        ElseIf rbnObj3.Checked Then
            lObjFunction = 3
        ElseIf rbnObj4.Checked Then
            lObjFunction = 4
        ElseIf rbnObj5.Checked Then
            lObjFunction = 5
        ElseIf rbnObj6.Checked Then
            lObjFunction = 6
        End If

        EnableControls(False)
        lblStatus.Text = "Performing Model Calibration........"
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        If Calibrate(lFlowGageNames, lCalibParms, lstMax.SelectedItems(0), lObjFunction, lSJDate, lEJDate) Then
            tabMain.SelectedIndex = 8
            lblStatus.Text = "Update specifications if desired, then click one of the 'Generate' buttons to view output."
        Else
            lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        End If

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdBankfullNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBankfullNext.Click
        SetOutputPath(atxModel.Text)

        EnableControls(False)
        lblStatus.Text = "Computing Monthly and Annual Fluxes......."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        BankFull()

        lblStatus.Text = "Update specifications if desired, then click one of the 'Generate' buttons to view output."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub cmdMapGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMapGenerate.Click
        SetOutputPath(atxModel.Text)

        Dim lStreamFlow As Boolean = True
        If rbnSoil.Checked Then
            lStreamFlow = False
        End If

        Dim lMapJDate As Double = 0.0
        Dim lMapDate(5) As Integer
        lMapDate(0) = atxMapYear.Text
        lMapDate(1) = AtxMapMonth.Text
        lMapDate(2) = AtxMapDay.Text
        lMapJDate = Date2J(lMapDate)

        EnableControls(False)
        lblStatus.Text = "Generating Map......."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Me.Refresh()

        PlotMap(lStreamFlow, lMapJDate)

        lblStatus.Text = "Update specifications if desired, then click one of the 'Generate' buttons to view output."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        EnableControls(True)
    End Sub

    Private Sub rbnNonUniformUSGS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbnNonUniformUSGS.CheckedChanged
        ResponseRadioButtonsChanged()
    End Sub

    Private Sub rbnNonUniformUser_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbnNonUniformUser.CheckedChanged
        ResponseRadioButtonsChanged()
    End Sub

    Private Sub rbnUniform_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbnUniform.CheckedChanged
        ResponseRadioButtonsChanged()
    End Sub

    Private Sub ResponseRadioButtonsChanged()
        If rbnNonUniformUSGS.Checked Then
            cboUSGS.Visible = True
            AtcGridMannings.Visible = True
            lblOverland.Visible = False
            atxOverland.Visible = False
            lblInstream.Visible = False
            atxInstream.Visible = False
        ElseIf rbnNonUniformUser.Checked Then
            lblOverland.Visible = True
            atxOverland.Visible = True
            lblInstream.Visible = True
            atxInstream.Visible = True
            cboUSGS.Visible = False
            AtcGridMannings.Visible = False
            atxOverland.ValueDouble = 0.05
            atxInstream.ValueDouble = 0.5
        ElseIf rbnUniform.Checked Then
            lblOverland.Visible = True
            atxOverland.Visible = True
            lblInstream.Visible = False
            atxInstream.Visible = False
            cboUSGS.Visible = False
            AtcGridMannings.Visible = False
            atxOverland.ValueDouble = 0.3
        End If
    End Sub

    Private Sub AtcGridPrec_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridPrec.CellEdited
        SetDates()
    End Sub

    Private Sub AtcGridPrec_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridPrec.MouseDownCell
        SetMetStationValidValues(aRow, aColumn)
    End Sub

    Private Sub tabMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMain.SelectedIndexChanged
        SetOutputPath(atxModel.Text)
        lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Refresh()
        Dim lAppEx As ApplicationException
        If tabMain.SelectedIndex = 3 Then
            If pStationsRead = False Then
                lblStatus.Text = "Reading Precipitation Data ..."
                Me.Refresh()
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                EnableControls(False)
                BuildListofValidStationNames("PREC", pPrecStations)
                lblStatus.Text = "Reading Evap Data ..."
                Me.Refresh()
                BuildListofValidStationNames("PEVT", pMetStations)
                SetPrecipStationGrid()
                lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                Me.Refresh()
                EnableControls(True)
                pStationsRead = True
            End If
        End If
        If tabMain.SelectedIndex = 6 Or tabMain.SelectedIndex = 7 Then
            'read reaches for sensitivity analysis or calibration 
            lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            Me.Refresh()
            cboReachSensitivity.Items.Clear()
            AtcConnectFlows.lstTarget.Items.Clear()
            Dim orderfilename As String = pOutputPath & "order.txt"
            If FileExists(orderfilename) Then
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(orderfilename)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        If IsNumeric(lCurrentRecord) Then
                            cboReachSensitivity.Items.Add(lCurrentRecord)
                            AtcConnectFlows.lstTarget.Items.Add(lCurrentRecord)
                        End If
                    End If
                Loop
            Else
                'Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Basin Characteristics' to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Exit Sub
            End If
            cboReachSensitivity.SelectedIndex = 0
        End If
        If tabMain.SelectedIndex = 7 Then
            'set up calibration tab
            AtcConnectFlows.lstSource.Items.Clear()
            pFlowStations.Clear()
            For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
                Dim lTotalCount As Integer = lDataSource.DataSets.Count
                Dim lCounter As Integer = 0
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    lCounter += 1
                    Logger.Progress("Building list of valid calibration station names...", lCounter, lDataSource.DataSets.Count)
                    If lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" And lDataSet.Attributes.GetValue("Constituent") = "FLOW" Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        Dim lSJDay As Double
                        Dim lEJDay As Double
                        lSJDay = lDataSet.Attributes.GetValue("Start Date", 0)
                        lEJDay = lDataSet.Attributes.GetValue("End Date", 0)
                        If lSJDay = 0 Then
                            lSJDay = lDataSet.Dates.Value(0)
                        End If
                        If lEJDay = 0 Then
                            lEJDay = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        End If
                        Dim lSdate(6) As Integer
                        Dim lEdate(6) As Integer
                        J2Date(lSJDay, lSdate)
                        J2Date(lEJDay, lEdate)
                        Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                        AtcConnectFlows.lstSource.Items.Add(lLoc & ":" & lDateString)
                        Dim lStationDetails As New StationDetails
                        lStationDetails.Name = lLoc
                        lStationDetails.StartJDate = lSJDay
                        lStationDetails.EndJDate = lEJDay
                        lStationDetails.Description = lLoc & ":" & lDateString
                        pFlowStations.Add(lStationDetails.Description, lStationDetails)
                    End If
                    'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                    lDataSet.ValuesNeedToBeRead = True
                Next
            Next
            If lstCalib.Items.Count = 0 Then
                lstCalib.Items.Clear()
                lstCalib.Items.Add("SoilWhc")
                lstCalib.Items.Add("Depth")
                lstCalib.Items.Add("Texture")
                lstCalib.Items.Add("Ks")
                lstCalib.Items.Add("Interflow")
                lstCalib.Items.Add("HSlope")
                lstCalib.Items.Add("Baseflow")
                lstCalib.Items.Add("CurveNum")
                lstCalib.Items.Add("MaxCover")
                lstCalib.Items.Add("BasinLoss")
                lstCalib.Items.Add("PanCoeff")
                lstCalib.Items.Add("TopSoil")
                lstCalib.Items.Add("RainCalc")
                lstCalib.Items.Add("RivRough")
                lstCalib.Items.Add("RivSlope")
                lstCalib.Items.Add("RivWidth")
                lstCalib.Items.Add("RivLoss")
                lstCalib.Items.Add("RivFPLoss")
                lstCalib.Items.Add("Celerity")
                lstCalib.Items.Add("Diffusion")
            End If
            lblStatus.Text = "Update specifications if desired, then click 'Next' to proceed."
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            Me.Refresh()
        ElseIf tabMain.SelectedIndex = 8 Then
            'output tab
            atxMapYear.Text = atxSYear.Text
            AtxMapMonth.Text = atxSMonth.Text
            AtxMapDay.Text = atxSDay.Text
            'read reaches for hydrograph plot 
            cboRchHydro.Items.Clear()
            Dim lFlowFileName As String = pOutputPath & "streamflow.txt"
            If FileExists(lFlowFileName) Then
                Try
                    Dim lCurrentRecord As String
                    Dim lStreamReader As New StreamReader(lFlowFileName)
                    Do
                        lCurrentRecord = lStreamReader.ReadLine
                        If lCurrentRecord Is Nothing Then
                            Exit Do
                        Else
                            Dim lstr As String = lCurrentRecord
                            Dim lstr1 As String = ""
                            Do While lstr.Length > 0
                                lstr1 = StrRetRem(lstr)
                                If IsNumeric(lstr1) Then
                                    cboRchHydro.Items.Add(lstr1)
                                End If
                            Loop
                            Exit Do
                        End If
                    Loop
                Catch lAppEx
                    Exit Sub
                End Try
            Else
                Exit Sub
            End If
            cboRchHydro.SelectedIndex = 0
            lblStatus.Text = "Update specifications if desired, then click one of the 'Generate' buttons to view output."
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            Me.Refresh()
        End If
    End Sub

    Private Sub AtcGridSensitivity_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridSensitivity.CellEdited
        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewValueNumeric As Double = GetNaN()
        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)

        If lNewValueNumeric >= 0 Then
            lNewColor = aGrid.CellBackColor
        Else
            lNewColor = Color.Pink
        End If

        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If
    End Sub

    Private Sub lstCalib_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstCalib.SelectedIndexChanged
        Dim lccount As Integer = lstCalib.SelectedItems.Count
        Dim lncomplex As Integer = 2
        Dim lnsamples As Integer = 2 * lncomplex * lccount
        Dim lnummult As Integer = 0
        If (lnsamples < 10) Then
            lnummult = (10 / (lncomplex * lccount)) + 0.5
            lnsamples = lnummult * lncomplex * lccount
        ElseIf (lnsamples > 200) Then
            lnummult = (100 / (lncomplex * lccount))
            lnsamples = lnummult * lncomplex * lccount
        End If
        lstMax.Items.Clear()
        lstMax.Items.Add(lnsamples * lccount * 10)
        lstMax.Items.Add(lnsamples * lccount * 4)
        lstMax.Items.Add(lnsamples * lccount * 8)
        lstMax.Items.Add(lnsamples * lccount * 16)
        lstMax.Items.Add(lnsamples * lccount * 32)
        lstMax.Items.Add(lnsamples * lccount * 64)
        lstMax.Items.Add(lnsamples * lccount * 128)
        lstMax.SelectedItem = 1
    End Sub

    Private Sub cmdReadStreamflow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReadStreamflow.Click
        SetOutputPath(atxModel.Text)

        Dim lFlowFN As String = pOutputPath & "streamflow.txt"
        SetFlowTimeseries(lFlowFN)

        Dim lGroup As New atcTimeseriesGroup
        For lDSIndex As Integer = 0 To atcDataManager.DataSources.Count - 1
            Dim lDS As atcDataSource = atcDataManager.DataSources(lDSIndex)
            If lDS.Name = "" And lDS.Description = "" Then
                'assume this is the right source
                For Each lts As atcDataSet In lDS.DataSets
                    If lts.Attributes.GetDefinedValue("Location").Value = "Reach " & cboRchHydro.SelectedItem.ToString Then
                        lGroup.Add(lts)
                    End If
                Next
            End If
        Next

        Dim lForm As New atcGraph.atcGraphForm()
        Dim lGrapher As atcGraph.clsGraphBase = New atcGraph.clsGraphTime(lGroup, lForm.ZedGraphCtrl)
        If lGrapher Is Nothing Then
            lForm.Dispose()
        Else
            lForm.Grapher = lGrapher
            lForm.Show()
        End If
    End Sub

    Private Sub AtcGridMannings_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles AtcGridMannings.CellEdited
        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewValueNumeric As Double = GetNaN()
        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)

        If lNewValueNumeric >= 0 Then
            lNewColor = aGrid.CellBackColor
        Else
            lNewColor = Color.Pink
        End If

        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If
    End Sub
End Class