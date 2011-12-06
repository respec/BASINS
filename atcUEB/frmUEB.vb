Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
Imports System.Windows.Forms

Public Class frmUEB
    Inherits System.Windows.Forms.Form

    Friend pParmData As clsUEBParameterFile
    Friend pSiteData As clsUEBSiteFile

    Friend pWeatherFileName As String
    Friend pBCParameterFileName As String
    Friend pOutputFileName As String
    Friend pRadOpt As Integer
    Friend pSDate(5) As Integer
    Friend pTStep As Integer
    Friend pInitialEnergy As Double
    Friend pInitialH2OEquiv As Double
    Friend pInitialSnowAge As Double
    Friend WithEvents txtNetRadStation As System.Windows.Forms.TextBox
    Friend WithEvents txtShortRadStation As System.Windows.Forms.TextBox
    Friend WithEvents txtRelHStation As System.Windows.Forms.TextBox
    Friend WithEvents txtWindStation As System.Windows.Forms.TextBox
    Friend WithEvents txtPrecipStation As System.Windows.Forms.TextBox
    Friend WithEvents txtAtempStation As System.Windows.Forms.TextBox
    Friend pBCDataArray(37) As Double

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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
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
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents AtcTextEDay As atcControls.atcText
    Friend WithEvents AtcTextSDay As atcControls.atcText
    Friend WithEvents AtcTextSYear As atcControls.atcText
    Friend WithEvents AtcTextEMon As atcControls.atcText
    Friend WithEvents AtcTextSMonth As atcControls.atcText
    Friend WithEvents AtcTextEYear As atcControls.atcText
    Friend WithEvents lblATempStation As System.Windows.Forms.Label
    Friend WithEvents lblWindStation As System.Windows.Forms.Label
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cmdSimulate As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents rdoMeasuredNet As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRadMeasuredInput As System.Windows.Forms.RadioButton
    Friend WithEvents rdoRadEstimate As System.Windows.Forms.RadioButton
    Friend WithEvents lblTimeStep As System.Windows.Forms.Label
    Friend WithEvents atcTextTimeStep As atcControls.atcText
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AtcTextEHour As atcControls.atcText
    Friend WithEvents AtcTextSHour As atcControls.atcText
    Friend WithEvents lblShortRadStation As System.Windows.Forms.Label
    Friend WithEvents lblRelHStation As System.Windows.Forms.Label
    Friend WithEvents lblPrecipStation As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents AtcTextSnowAge As atcControls.atcText
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents AtcTextIniEnergyContent As atcControls.atcText
    Friend WithEvents AtcTextIniWaterEquiv As atcControls.atcText
    Friend WithEvents lblNetRadStation As System.Windows.Forms.Label
    Friend WithEvents AtcGridModelParms As atcControls.atcGrid
    Friend WithEvents AtcGridSiteVars As atcControls.atcGrid
    Friend WithEvents AtcTextCParm As atcControls.atcText
    Friend WithEvents AtcTextAParm As atcControls.atcText
    Friend WithEvents lblCParm As System.Windows.Forms.Label
    Friend WithEvents lblAParm As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblInstructions As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtWeatherFile As System.Windows.Forms.TextBox
    Friend WithEvents txtParameterFile As System.Windows.Forms.TextBox
    Friend WithEvents txtSiteFile As System.Windows.Forms.TextBox
    Friend WithEvents txtBCParameterFile As System.Windows.Forms.TextBox
    Friend WithEvents txtMasterFile As System.Windows.Forms.TextBox
    Friend WithEvents txtOutputFile As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtProjectName As System.Windows.Forms.TextBox
    Friend WithEvents AtcGridBCMonthly As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUEB))
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdAbout = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtOutputFile = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtProjectName = New System.Windows.Forms.TextBox()
        Me.txtMasterFile = New System.Windows.Forms.TextBox()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.AtcTextSnowAge = New atcControls.atcText()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.AtcTextIniEnergyContent = New atcControls.atcText()
        Me.AtcTextIniWaterEquiv = New atcControls.atcText()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.txtWeatherFile = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblNetRadStation = New System.Windows.Forms.Label()
        Me.lblShortRadStation = New System.Windows.Forms.Label()
        Me.lblRelHStation = New System.Windows.Forms.Label()
        Me.lblPrecipStation = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.rdoMeasuredNet = New System.Windows.Forms.RadioButton()
        Me.rdoRadMeasuredInput = New System.Windows.Forms.RadioButton()
        Me.rdoRadEstimate = New System.Windows.Forms.RadioButton()
        Me.lblATempStation = New System.Windows.Forms.Label()
        Me.lblWindStation = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblTimeStep = New System.Windows.Forms.Label()
        Me.atcTextTimeStep = New atcControls.atcText()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.AtcTextEHour = New atcControls.atcText()
        Me.AtcTextSHour = New atcControls.atcText()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.AtcTextEDay = New atcControls.atcText()
        Me.AtcTextSDay = New atcControls.atcText()
        Me.AtcTextSYear = New atcControls.atcText()
        Me.AtcTextEMon = New atcControls.atcText()
        Me.AtcTextSMonth = New atcControls.atcText()
        Me.AtcTextEYear = New atcControls.atcText()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.txtParameterFile = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.AtcGridModelParms = New atcControls.atcGrid()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.txtSiteFile = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.AtcGridSiteVars = New atcControls.atcGrid()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtBCParameterFile = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.AtcTextCParm = New atcControls.atcText()
        Me.AtcTextAParm = New atcControls.atcText()
        Me.lblCParm = New System.Windows.Forms.Label()
        Me.lblAParm = New System.Windows.Forms.Label()
        Me.AtcGridBCMonthly = New atcControls.atcGrid()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.ComboBox14 = New System.Windows.Forms.ComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.ComboBox15 = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.ComboBox16 = New System.Windows.Forms.ComboBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ComboBox17 = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.ComboBox18 = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.ComboBox19 = New System.Windows.Forms.ComboBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.ComboBox20 = New System.Windows.Forms.ComboBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.ComboBox21 = New System.Windows.Forms.ComboBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.ComboBox22 = New System.Windows.Forms.ComboBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.ComboBox23 = New System.Windows.Forms.ComboBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.ComboBox24 = New System.Windows.Forms.ComboBox()
        Me.ComboBox25 = New System.Windows.Forms.ComboBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.ComboBox26 = New System.Windows.Forms.ComboBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.ComboBox27 = New System.Windows.Forms.ComboBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.ComboBox28 = New System.Windows.Forms.ComboBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.ComboBox29 = New System.Windows.Forms.ComboBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.ComboBox30 = New System.Windows.Forms.ComboBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.ComboBox31 = New System.Windows.Forms.ComboBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.ComboBox32 = New System.Windows.Forms.ComboBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.ComboBox33 = New System.Windows.Forms.ComboBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.ComboBox34 = New System.Windows.Forms.ComboBox()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.ComboBox35 = New System.Windows.Forms.ComboBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.ComboBox36 = New System.Windows.Forms.ComboBox()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.ComboBox37 = New System.Windows.Forms.ComboBox()
        Me.ComboBox38 = New System.Windows.Forms.ComboBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.ComboBox39 = New System.Windows.Forms.ComboBox()
        Me.cmdSimulate = New System.Windows.Forms.Button()
        Me.txtAtempStation = New System.Windows.Forms.TextBox()
        Me.txtPrecipStation = New System.Windows.Forms.TextBox()
        Me.txtWindStation = New System.Windows.Forms.TextBox()
        Me.txtRelHStation = New System.Windows.Forms.TextBox()
        Me.txtShortRadStation = New System.Windows.Forms.TextBox()
        Me.txtNetRadStation = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(323, 542)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(73, 28)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Close"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(402, 542)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(65, 28)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(473, 542)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(72, 28)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "About"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(15, 15)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(529, 459)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.txtOutputFile)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.txtProjectName)
        Me.TabPage2.Controls.Add(Me.txtMasterFile)
        Me.TabPage2.Controls.Add(Me.lblInstructions)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.Label62)
        Me.TabPage2.Controls.Add(Me.GroupBox4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(521, 430)
        Me.TabPage2.TabIndex = 12
        Me.TabPage2.Text = "General"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'txtOutputFile
        '
        Me.txtOutputFile.Location = New System.Drawing.Point(121, 175)
        Me.txtOutputFile.Name = "txtOutputFile"
        Me.txtOutputFile.Size = New System.Drawing.Size(142, 20)
        Me.txtOutputFile.TabIndex = 48
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 178)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(92, 13)
        Me.Label7.TabIndex = 47
        Me.Label7.Text = "Output File Name:"
        '
        'txtProjectName
        '
        Me.txtProjectName.Location = New System.Drawing.Point(121, 136)
        Me.txtProjectName.Name = "txtProjectName"
        Me.txtProjectName.Size = New System.Drawing.Size(142, 20)
        Me.txtProjectName.TabIndex = 46
        '
        'txtMasterFile
        '
        Me.txtMasterFile.Location = New System.Drawing.Point(121, 97)
        Me.txtMasterFile.Name = "txtMasterFile"
        Me.txtMasterFile.Size = New System.Drawing.Size(380, 20)
        Me.txtMasterFile.TabIndex = 45
        '
        'lblInstructions
        '
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.Location = New System.Drawing.Point(18, 19)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(0, 13)
        Me.lblInstructions.TabIndex = 35
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Master Project File:"
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(18, 139)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(74, 13)
        Me.Label62.TabIndex = 31
        Me.Label62.Text = "Project Name:"
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.Label50)
        Me.GroupBox4.Controls.Add(Me.AtcTextSnowAge)
        Me.GroupBox4.Controls.Add(Me.Label52)
        Me.GroupBox4.Controls.Add(Me.Label53)
        Me.GroupBox4.Controls.Add(Me.AtcTextIniEnergyContent)
        Me.GroupBox4.Controls.Add(Me.AtcTextIniWaterEquiv)
        Me.GroupBox4.Location = New System.Drawing.Point(21, 233)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(480, 128)
        Me.GroupBox4.TabIndex = 30
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Initial Conditions"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(20, 88)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(96, 13)
        Me.Label50.TabIndex = 39
        Me.Label50.Text = "Snow Surface Age"
        '
        'AtcTextSnowAge
        '
        Me.AtcTextSnowAge.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextSnowAge.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.AtcTextSnowAge.DefaultValue = ""
        Me.AtcTextSnowAge.HardMax = 9999.0R
        Me.AtcTextSnowAge.HardMin = 0.0R
        Me.AtcTextSnowAge.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextSnowAge.Location = New System.Drawing.Point(156, 91)
        Me.AtcTextSnowAge.MaxWidth = 20
        Me.AtcTextSnowAge.Name = "AtcTextSnowAge"
        Me.AtcTextSnowAge.NumericFormat = "0"
        Me.AtcTextSnowAge.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextSnowAge.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextSnowAge.SelLength = 0
        Me.AtcTextSnowAge.SelStart = 0
        Me.AtcTextSnowAge.Size = New System.Drawing.Size(64, 21)
        Me.AtcTextSnowAge.SoftMax = -999.0R
        Me.AtcTextSnowAge.SoftMin = -999.0R
        Me.AtcTextSnowAge.TabIndex = 38
        Me.AtcTextSnowAge.ValueDouble = 0.0R
        Me.AtcTextSnowAge.ValueInteger = 0
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(20, 64)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(129, 13)
        Me.Label52.TabIndex = 37
        Me.Label52.Text = "Water Equivalence W (m)"
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(20, 37)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(130, 13)
        Me.Label53.TabIndex = 36
        Me.Label53.Text = "Energy Content U (kJ/m2)"
        '
        'AtcTextIniEnergyContent
        '
        Me.AtcTextIniEnergyContent.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextIniEnergyContent.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.AtcTextIniEnergyContent.DefaultValue = ""
        Me.AtcTextIniEnergyContent.HardMax = 9999999.0R
        Me.AtcTextIniEnergyContent.HardMin = 0.0R
        Me.AtcTextIniEnergyContent.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextIniEnergyContent.Location = New System.Drawing.Point(156, 37)
        Me.AtcTextIniEnergyContent.MaxWidth = 20
        Me.AtcTextIniEnergyContent.Name = "AtcTextIniEnergyContent"
        Me.AtcTextIniEnergyContent.NumericFormat = "0"
        Me.AtcTextIniEnergyContent.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextIniEnergyContent.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextIniEnergyContent.SelLength = 0
        Me.AtcTextIniEnergyContent.SelStart = 0
        Me.AtcTextIniEnergyContent.Size = New System.Drawing.Size(64, 21)
        Me.AtcTextIniEnergyContent.SoftMax = -999.0R
        Me.AtcTextIniEnergyContent.SoftMin = -999.0R
        Me.AtcTextIniEnergyContent.TabIndex = 30
        Me.AtcTextIniEnergyContent.ValueDouble = 0.0R
        Me.AtcTextIniEnergyContent.ValueInteger = 0
        '
        'AtcTextIniWaterEquiv
        '
        Me.AtcTextIniWaterEquiv.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextIniWaterEquiv.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.AtcTextIniWaterEquiv.DefaultValue = ""
        Me.AtcTextIniWaterEquiv.HardMax = 9999.0R
        Me.AtcTextIniWaterEquiv.HardMin = 0.0R
        Me.AtcTextIniWaterEquiv.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextIniWaterEquiv.Location = New System.Drawing.Point(156, 64)
        Me.AtcTextIniWaterEquiv.MaxWidth = 20
        Me.AtcTextIniWaterEquiv.Name = "AtcTextIniWaterEquiv"
        Me.AtcTextIniWaterEquiv.NumericFormat = "0"
        Me.AtcTextIniWaterEquiv.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextIniWaterEquiv.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextIniWaterEquiv.SelLength = 0
        Me.AtcTextIniWaterEquiv.SelStart = 0
        Me.AtcTextIniWaterEquiv.Size = New System.Drawing.Size(64, 21)
        Me.AtcTextIniWaterEquiv.SoftMax = -999.0R
        Me.AtcTextIniWaterEquiv.SoftMin = -999.0R
        Me.AtcTextIniWaterEquiv.TabIndex = 27
        Me.AtcTextIniWaterEquiv.ValueDouble = 0.0R
        Me.AtcTextIniWaterEquiv.ValueInteger = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.txtNetRadStation)
        Me.TabPage3.Controls.Add(Me.txtShortRadStation)
        Me.TabPage3.Controls.Add(Me.txtRelHStation)
        Me.TabPage3.Controls.Add(Me.txtWindStation)
        Me.TabPage3.Controls.Add(Me.txtPrecipStation)
        Me.TabPage3.Controls.Add(Me.txtAtempStation)
        Me.TabPage3.Controls.Add(Me.txtWeatherFile)
        Me.TabPage3.Controls.Add(Me.Label3)
        Me.TabPage3.Controls.Add(Me.lblNetRadStation)
        Me.TabPage3.Controls.Add(Me.lblShortRadStation)
        Me.TabPage3.Controls.Add(Me.lblRelHStation)
        Me.TabPage3.Controls.Add(Me.lblPrecipStation)
        Me.TabPage3.Controls.Add(Me.GroupBox3)
        Me.TabPage3.Controls.Add(Me.lblATempStation)
        Me.TabPage3.Controls.Add(Me.lblWindStation)
        Me.TabPage3.Controls.Add(Me.GroupBox2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(521, 430)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Weather"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'txtWeatherFile
        '
        Me.txtWeatherFile.Location = New System.Drawing.Point(92, 12)
        Me.txtWeatherFile.Name = "txtWeatherFile"
        Me.txtWeatherFile.Size = New System.Drawing.Size(410, 20)
        Me.txtWeatherFile.TabIndex = 44
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 43
        Me.Label3.Text = "Weather File:"
        '
        'lblNetRadStation
        '
        Me.lblNetRadStation.AutoSize = True
        Me.lblNetRadStation.Location = New System.Drawing.Point(19, 398)
        Me.lblNetRadStation.Name = "lblNetRadStation"
        Me.lblNetRadStation.Size = New System.Drawing.Size(111, 13)
        Me.lblNetRadStation.TabIndex = 41
        Me.lblNetRadStation.Text = "Net Radiation Station:"
        '
        'lblShortRadStation
        '
        Me.lblShortRadStation.AutoSize = True
        Me.lblShortRadStation.Location = New System.Drawing.Point(19, 370)
        Me.lblShortRadStation.Name = "lblShortRadStation"
        Me.lblShortRadStation.Size = New System.Drawing.Size(145, 13)
        Me.lblShortRadStation.TabIndex = 39
        Me.lblShortRadStation.Text = "Shortwave Radiation Station:"
        '
        'lblRelHStation
        '
        Me.lblRelHStation.AutoSize = True
        Me.lblRelHStation.Location = New System.Drawing.Point(19, 343)
        Me.lblRelHStation.Name = "lblRelHStation"
        Me.lblRelHStation.Size = New System.Drawing.Size(128, 13)
        Me.lblRelHStation.TabIndex = 37
        Me.lblRelHStation.Text = "Relative Humidity Station:"
        '
        'lblPrecipStation
        '
        Me.lblPrecipStation.AutoSize = True
        Me.lblPrecipStation.Location = New System.Drawing.Point(19, 289)
        Me.lblPrecipStation.Name = "lblPrecipStation"
        Me.lblPrecipStation.Size = New System.Drawing.Size(76, 13)
        Me.lblPrecipStation.TabIndex = 35
        Me.lblPrecipStation.Text = "Precip Station:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.rdoMeasuredNet)
        Me.GroupBox3.Controls.Add(Me.rdoRadMeasuredInput)
        Me.GroupBox3.Controls.Add(Me.rdoRadEstimate)
        Me.GroupBox3.Location = New System.Drawing.Point(19, 157)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(483, 96)
        Me.GroupBox3.TabIndex = 34
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Radiation Input Type"
        '
        'rdoMeasuredNet
        '
        Me.rdoMeasuredNet.AutoSize = True
        Me.rdoMeasuredNet.Location = New System.Drawing.Point(18, 65)
        Me.rdoMeasuredNet.Name = "rdoMeasuredNet"
        Me.rdoMeasuredNet.Size = New System.Drawing.Size(140, 17)
        Me.rdoMeasuredNet.TabIndex = 2
        Me.rdoMeasuredNet.Text = "Measured Net Radiation"
        Me.rdoMeasuredNet.UseVisualStyleBackColor = True
        '
        'rdoRadMeasuredInput
        '
        Me.rdoRadMeasuredInput.AutoSize = True
        Me.rdoRadMeasuredInput.Checked = True
        Me.rdoRadMeasuredInput.Location = New System.Drawing.Point(18, 42)
        Me.rdoRadMeasuredInput.Name = "rdoRadMeasuredInput"
        Me.rdoRadMeasuredInput.Size = New System.Drawing.Size(174, 17)
        Me.rdoRadMeasuredInput.TabIndex = 1
        Me.rdoRadMeasuredInput.TabStop = True
        Me.rdoRadMeasuredInput.Text = "Measured Input Solar Radiation"
        Me.rdoRadMeasuredInput.UseVisualStyleBackColor = True
        '
        'rdoRadEstimate
        '
        Me.rdoRadEstimate.AutoSize = True
        Me.rdoRadEstimate.Location = New System.Drawing.Point(18, 19)
        Me.rdoRadEstimate.Name = "rdoRadEstimate"
        Me.rdoRadEstimate.Size = New System.Drawing.Size(212, 17)
        Me.rdoRadEstimate.TabIndex = 0
        Me.rdoRadEstimate.Text = "Estimate from Daily Temperature Range"
        Me.rdoRadEstimate.UseVisualStyleBackColor = True
        '
        'lblATempStation
        '
        Me.lblATempStation.AutoSize = True
        Me.lblATempStation.Location = New System.Drawing.Point(19, 262)
        Me.lblATempStation.Name = "lblATempStation"
        Me.lblATempStation.Size = New System.Drawing.Size(88, 13)
        Me.lblATempStation.TabIndex = 32
        Me.lblATempStation.Text = "Air Temp Station:"
        '
        'lblWindStation
        '
        Me.lblWindStation.AutoSize = True
        Me.lblWindStation.Location = New System.Drawing.Point(19, 316)
        Me.lblWindStation.Name = "lblWindStation"
        Me.lblWindStation.Size = New System.Drawing.Size(105, 13)
        Me.lblWindStation.TabIndex = 30
        Me.lblWindStation.Text = "Wind Speed Station:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lblTimeStep)
        Me.GroupBox2.Controls.Add(Me.atcTextTimeStep)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.AtcTextEHour)
        Me.GroupBox2.Controls.Add(Me.AtcTextSHour)
        Me.GroupBox2.Controls.Add(Me.Label54)
        Me.GroupBox2.Controls.Add(Me.Label55)
        Me.GroupBox2.Controls.Add(Me.Label56)
        Me.GroupBox2.Controls.Add(Me.Label57)
        Me.GroupBox2.Controls.Add(Me.Label58)
        Me.GroupBox2.Controls.Add(Me.AtcTextEDay)
        Me.GroupBox2.Controls.Add(Me.AtcTextSDay)
        Me.GroupBox2.Controls.Add(Me.AtcTextSYear)
        Me.GroupBox2.Controls.Add(Me.AtcTextEMon)
        Me.GroupBox2.Controls.Add(Me.AtcTextSMonth)
        Me.GroupBox2.Controls.Add(Me.AtcTextEYear)
        Me.GroupBox2.Location = New System.Drawing.Point(19, 45)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(483, 96)
        Me.GroupBox2.TabIndex = 29
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Simulation Dates"
        '
        'lblTimeStep
        '
        Me.lblTimeStep.AutoSize = True
        Me.lblTimeStep.Location = New System.Drawing.Point(297, 38)
        Me.lblTimeStep.Name = "lblTimeStep"
        Me.lblTimeStep.Size = New System.Drawing.Size(78, 13)
        Me.lblTimeStep.TabIndex = 42
        Me.lblTimeStep.Text = "Time Step (hrs)"
        '
        'atcTextTimeStep
        '
        Me.atcTextTimeStep.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atcTextTimeStep.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atcTextTimeStep.DefaultValue = ""
        Me.atcTextTimeStep.HardMax = 24.0R
        Me.atcTextTimeStep.HardMin = 1.0R
        Me.atcTextTimeStep.InsideLimitsBackground = System.Drawing.Color.White
        Me.atcTextTimeStep.Location = New System.Drawing.Point(381, 38)
        Me.atcTextTimeStep.MaxWidth = 20
        Me.atcTextTimeStep.Name = "atcTextTimeStep"
        Me.atcTextTimeStep.NumericFormat = "0"
        Me.atcTextTimeStep.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atcTextTimeStep.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atcTextTimeStep.SelLength = 0
        Me.atcTextTimeStep.SelStart = 0
        Me.atcTextTimeStep.Size = New System.Drawing.Size(64, 21)
        Me.atcTextTimeStep.SoftMax = -999.0R
        Me.atcTextTimeStep.SoftMin = -999.0R
        Me.atcTextTimeStep.TabIndex = 41
        Me.atcTextTimeStep.ValueDouble = 1.0R
        Me.atcTextTimeStep.ValueInteger = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(224, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 40
        Me.Label1.Text = "Hour"
        '
        'AtcTextEHour
        '
        Me.AtcTextEHour.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextEHour.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextEHour.DefaultValue = ""
        Me.AtcTextEHour.HardMax = 24.0R
        Me.AtcTextEHour.HardMin = 0.0R
        Me.AtcTextEHour.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextEHour.Location = New System.Drawing.Point(227, 64)
        Me.AtcTextEHour.MaxWidth = 20
        Me.AtcTextEHour.Name = "AtcTextEHour"
        Me.AtcTextEHour.NumericFormat = "0"
        Me.AtcTextEHour.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextEHour.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextEHour.SelLength = 0
        Me.AtcTextEHour.SelStart = 0
        Me.AtcTextEHour.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextEHour.SoftMax = -999.0R
        Me.AtcTextEHour.SoftMin = -999.0R
        Me.AtcTextEHour.TabIndex = 39
        Me.AtcTextEHour.ValueDouble = 0.0R
        Me.AtcTextEHour.ValueInteger = 0
        '
        'AtcTextSHour
        '
        Me.AtcTextSHour.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextSHour.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextSHour.DefaultValue = ""
        Me.AtcTextSHour.HardMax = 24.0R
        Me.AtcTextSHour.HardMin = 0.0R
        Me.AtcTextSHour.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextSHour.Location = New System.Drawing.Point(227, 38)
        Me.AtcTextSHour.MaxWidth = 20
        Me.AtcTextSHour.Name = "AtcTextSHour"
        Me.AtcTextSHour.NumericFormat = "0"
        Me.AtcTextSHour.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextSHour.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextSHour.SelLength = 0
        Me.AtcTextSHour.SelStart = 0
        Me.AtcTextSHour.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextSHour.SoftMax = -999.0R
        Me.AtcTextSHour.SoftMin = -999.0R
        Me.AtcTextSHour.TabIndex = 38
        Me.AtcTextSHour.ValueDouble = 0.0R
        Me.AtcTextSHour.ValueInteger = 0
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(20, 64)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(26, 13)
        Me.Label54.TabIndex = 37
        Me.Label54.Text = "End"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(16, 38)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(29, 13)
        Me.Label55.TabIndex = 36
        Me.Label55.Text = "Start"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(175, 20)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(26, 13)
        Me.Label56.TabIndex = 35
        Me.Label56.Text = "Day"
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(126, 20)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(37, 13)
        Me.Label57.TabIndex = 34
        Me.Label57.Text = "Month"
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(58, 20)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(29, 13)
        Me.Label58.TabIndex = 33
        Me.Label58.Text = "Year"
        '
        'AtcTextEDay
        '
        Me.AtcTextEDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextEDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextEDay.DefaultValue = ""
        Me.AtcTextEDay.HardMax = 31.0R
        Me.AtcTextEDay.HardMin = 1.0R
        Me.AtcTextEDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextEDay.Location = New System.Drawing.Point(178, 64)
        Me.AtcTextEDay.MaxWidth = 20
        Me.AtcTextEDay.Name = "AtcTextEDay"
        Me.AtcTextEDay.NumericFormat = "0"
        Me.AtcTextEDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextEDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextEDay.SelLength = 0
        Me.AtcTextEDay.SelStart = 0
        Me.AtcTextEDay.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextEDay.SoftMax = -999.0R
        Me.AtcTextEDay.SoftMin = -999.0R
        Me.AtcTextEDay.TabIndex = 32
        Me.AtcTextEDay.ValueDouble = 31.0R
        Me.AtcTextEDay.ValueInteger = 31
        '
        'AtcTextSDay
        '
        Me.AtcTextSDay.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextSDay.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextSDay.DefaultValue = ""
        Me.AtcTextSDay.HardMax = 31.0R
        Me.AtcTextSDay.HardMin = 1.0R
        Me.AtcTextSDay.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextSDay.Location = New System.Drawing.Point(178, 38)
        Me.AtcTextSDay.MaxWidth = 20
        Me.AtcTextSDay.Name = "AtcTextSDay"
        Me.AtcTextSDay.NumericFormat = "0"
        Me.AtcTextSDay.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextSDay.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextSDay.SelLength = 0
        Me.AtcTextSDay.SelStart = 0
        Me.AtcTextSDay.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextSDay.SoftMax = -999.0R
        Me.AtcTextSDay.SoftMin = -999.0R
        Me.AtcTextSDay.TabIndex = 31
        Me.AtcTextSDay.ValueDouble = 1.0R
        Me.AtcTextSDay.ValueInteger = 1
        '
        'AtcTextSYear
        '
        Me.AtcTextSYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextSYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextSYear.DefaultValue = ""
        Me.AtcTextSYear.HardMax = 9999.0R
        Me.AtcTextSYear.HardMin = 0.0R
        Me.AtcTextSYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextSYear.Location = New System.Drawing.Point(60, 38)
        Me.AtcTextSYear.MaxWidth = 20
        Me.AtcTextSYear.Name = "AtcTextSYear"
        Me.AtcTextSYear.NumericFormat = "0"
        Me.AtcTextSYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextSYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextSYear.SelLength = 0
        Me.AtcTextSYear.SelStart = 0
        Me.AtcTextSYear.Size = New System.Drawing.Size(64, 21)
        Me.AtcTextSYear.SoftMax = -999.0R
        Me.AtcTextSYear.SoftMin = -999.0R
        Me.AtcTextSYear.TabIndex = 30
        Me.AtcTextSYear.ValueDouble = 2000.0R
        Me.AtcTextSYear.ValueInteger = 2000
        '
        'AtcTextEMon
        '
        Me.AtcTextEMon.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextEMon.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextEMon.DefaultValue = ""
        Me.AtcTextEMon.HardMax = 12.0R
        Me.AtcTextEMon.HardMin = 1.0R
        Me.AtcTextEMon.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextEMon.Location = New System.Drawing.Point(129, 64)
        Me.AtcTextEMon.MaxWidth = 20
        Me.AtcTextEMon.Name = "AtcTextEMon"
        Me.AtcTextEMon.NumericFormat = "0"
        Me.AtcTextEMon.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextEMon.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextEMon.SelLength = 0
        Me.AtcTextEMon.SelStart = 0
        Me.AtcTextEMon.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextEMon.SoftMax = -999.0R
        Me.AtcTextEMon.SoftMin = -999.0R
        Me.AtcTextEMon.TabIndex = 29
        Me.AtcTextEMon.ValueDouble = 12.0R
        Me.AtcTextEMon.ValueInteger = 12
        '
        'AtcTextSMonth
        '
        Me.AtcTextSMonth.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextSMonth.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextSMonth.DefaultValue = ""
        Me.AtcTextSMonth.HardMax = 12.0R
        Me.AtcTextSMonth.HardMin = 1.0R
        Me.AtcTextSMonth.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextSMonth.Location = New System.Drawing.Point(129, 38)
        Me.AtcTextSMonth.MaxWidth = 20
        Me.AtcTextSMonth.Name = "AtcTextSMonth"
        Me.AtcTextSMonth.NumericFormat = "0"
        Me.AtcTextSMonth.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextSMonth.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextSMonth.SelLength = 0
        Me.AtcTextSMonth.SelStart = 0
        Me.AtcTextSMonth.Size = New System.Drawing.Size(44, 21)
        Me.AtcTextSMonth.SoftMax = -999.0R
        Me.AtcTextSMonth.SoftMin = -999.0R
        Me.AtcTextSMonth.TabIndex = 28
        Me.AtcTextSMonth.ValueDouble = 1.0R
        Me.AtcTextSMonth.ValueInteger = 1
        '
        'AtcTextEYear
        '
        Me.AtcTextEYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextEYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.AtcTextEYear.DefaultValue = ""
        Me.AtcTextEYear.HardMax = 9999.0R
        Me.AtcTextEYear.HardMin = 0.0R
        Me.AtcTextEYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextEYear.Location = New System.Drawing.Point(60, 64)
        Me.AtcTextEYear.MaxWidth = 20
        Me.AtcTextEYear.Name = "AtcTextEYear"
        Me.AtcTextEYear.NumericFormat = "0"
        Me.AtcTextEYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextEYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextEYear.SelLength = 0
        Me.AtcTextEYear.SelStart = 0
        Me.AtcTextEYear.Size = New System.Drawing.Size(64, 21)
        Me.AtcTextEYear.SoftMax = -999.0R
        Me.AtcTextEYear.SoftMin = -999.0R
        Me.AtcTextEYear.TabIndex = 27
        Me.AtcTextEYear.ValueDouble = 2000.0R
        Me.AtcTextEYear.ValueInteger = 2000
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.txtParameterFile)
        Me.TabPage7.Controls.Add(Me.Label4)
        Me.TabPage7.Controls.Add(Me.AtcGridModelParms)
        Me.TabPage7.Location = New System.Drawing.Point(4, 25)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(521, 430)
        Me.TabPage7.TabIndex = 8
        Me.TabPage7.Text = "Model Parameters"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'txtParameterFile
        '
        Me.txtParameterFile.Location = New System.Drawing.Point(92, 15)
        Me.txtParameterFile.Name = "txtParameterFile"
        Me.txtParameterFile.Size = New System.Drawing.Size(410, 20)
        Me.txtParameterFile.TabIndex = 46
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 13)
        Me.Label4.TabIndex = 45
        Me.Label4.Text = "Parameter File:"
        '
        'AtcGridModelParms
        '
        Me.AtcGridModelParms.AllowHorizontalScrolling = True
        Me.AtcGridModelParms.AllowNewValidValues = False
        Me.AtcGridModelParms.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridModelParms.CellBackColor = System.Drawing.SystemColors.Window
        Me.AtcGridModelParms.Fixed3D = False
        Me.AtcGridModelParms.LineColor = System.Drawing.SystemColors.Control
        Me.AtcGridModelParms.LineWidth = 1.0!
        Me.AtcGridModelParms.Location = New System.Drawing.Point(3, 57)
        Me.AtcGridModelParms.Name = "AtcGridModelParms"
        Me.AtcGridModelParms.Size = New System.Drawing.Size(511, 360)
        Me.AtcGridModelParms.Source = Nothing
        Me.AtcGridModelParms.TabIndex = 0
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.txtSiteFile)
        Me.TabPage8.Controls.Add(Me.Label5)
        Me.TabPage8.Controls.Add(Me.AtcGridSiteVars)
        Me.TabPage8.Location = New System.Drawing.Point(4, 25)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(521, 430)
        Me.TabPage8.TabIndex = 9
        Me.TabPage8.Text = "Site Variables"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'txtSiteFile
        '
        Me.txtSiteFile.Location = New System.Drawing.Point(68, 17)
        Me.txtSiteFile.Name = "txtSiteFile"
        Me.txtSiteFile.Size = New System.Drawing.Size(410, 20)
        Me.txtSiteFile.TabIndex = 48
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 47
        Me.Label5.Text = "Site File:"
        '
        'AtcGridSiteVars
        '
        Me.AtcGridSiteVars.AllowHorizontalScrolling = True
        Me.AtcGridSiteVars.AllowNewValidValues = False
        Me.AtcGridSiteVars.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridSiteVars.CellBackColor = System.Drawing.SystemColors.Window
        Me.AtcGridSiteVars.Fixed3D = False
        Me.AtcGridSiteVars.LineColor = System.Drawing.SystemColors.Control
        Me.AtcGridSiteVars.LineWidth = 1.0!
        Me.AtcGridSiteVars.Location = New System.Drawing.Point(3, 47)
        Me.AtcGridSiteVars.Name = "AtcGridSiteVars"
        Me.AtcGridSiteVars.Size = New System.Drawing.Size(515, 379)
        Me.AtcGridSiteVars.Source = Nothing
        Me.AtcGridSiteVars.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtBCParameterFile)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.AtcTextCParm)
        Me.TabPage1.Controls.Add(Me.AtcTextAParm)
        Me.TabPage1.Controls.Add(Me.lblCParm)
        Me.TabPage1.Controls.Add(Me.lblAParm)
        Me.TabPage1.Controls.Add(Me.AtcGridBCMonthly)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(521, 430)
        Me.TabPage1.TabIndex = 11
        Me.TabPage1.Text = "Bristow Campbell Parameters"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'txtBCParameterFile
        '
        Me.txtBCParameterFile.Location = New System.Drawing.Point(111, 15)
        Me.txtBCParameterFile.Name = "txtBCParameterFile"
        Me.txtBCParameterFile.Size = New System.Drawing.Size(403, 20)
        Me.txtBCParameterFile.TabIndex = 57
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 56
        Me.Label6.Text = "B-C Parameter File:"
        '
        'AtcTextCParm
        '
        Me.AtcTextCParm.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextCParm.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.AtcTextCParm.DefaultValue = ""
        Me.AtcTextCParm.HardMax = -999.0R
        Me.AtcTextCParm.HardMin = -999.0R
        Me.AtcTextCParm.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextCParm.Location = New System.Drawing.Point(325, 58)
        Me.AtcTextCParm.MaxWidth = 20
        Me.AtcTextCParm.Name = "AtcTextCParm"
        Me.AtcTextCParm.NumericFormat = "0.#####"
        Me.AtcTextCParm.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextCParm.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextCParm.SelLength = 0
        Me.AtcTextCParm.SelStart = 0
        Me.AtcTextCParm.Size = New System.Drawing.Size(97, 20)
        Me.AtcTextCParm.SoftMax = -999.0R
        Me.AtcTextCParm.SoftMin = -999.0R
        Me.AtcTextCParm.TabIndex = 55
        Me.AtcTextCParm.ValueDouble = 0.0R
        Me.AtcTextCParm.ValueInteger = 0
        '
        'AtcTextAParm
        '
        Me.AtcTextAParm.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.AtcTextAParm.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.AtcTextAParm.DefaultValue = ""
        Me.AtcTextAParm.HardMax = -999.0R
        Me.AtcTextAParm.HardMin = -999.0R
        Me.AtcTextAParm.InsideLimitsBackground = System.Drawing.Color.White
        Me.AtcTextAParm.Location = New System.Drawing.Point(112, 58)
        Me.AtcTextAParm.MaxWidth = 20
        Me.AtcTextAParm.Name = "AtcTextAParm"
        Me.AtcTextAParm.NumericFormat = "0.#####"
        Me.AtcTextAParm.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.AtcTextAParm.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.AtcTextAParm.SelLength = 0
        Me.AtcTextAParm.SelStart = 0
        Me.AtcTextAParm.Size = New System.Drawing.Size(97, 20)
        Me.AtcTextAParm.SoftMax = -999.0R
        Me.AtcTextAParm.SoftMin = -999.0R
        Me.AtcTextAParm.TabIndex = 54
        Me.AtcTextAParm.ValueDouble = 0.0R
        Me.AtcTextAParm.ValueInteger = 0
        '
        'lblCParm
        '
        Me.lblCParm.AutoSize = True
        Me.lblCParm.Location = New System.Drawing.Point(251, 58)
        Me.lblCParm.Name = "lblCParm"
        Me.lblCParm.Size = New System.Drawing.Size(68, 13)
        Me.lblCParm.TabIndex = 53
        Me.lblCParm.Text = "Parameter C:"
        '
        'lblAParm
        '
        Me.lblAParm.AutoSize = True
        Me.lblAParm.Location = New System.Drawing.Point(38, 58)
        Me.lblAParm.Name = "lblAParm"
        Me.lblAParm.Size = New System.Drawing.Size(68, 13)
        Me.lblAParm.TabIndex = 52
        Me.lblAParm.Text = "Parameter A:"
        '
        'AtcGridBCMonthly
        '
        Me.AtcGridBCMonthly.AllowHorizontalScrolling = True
        Me.AtcGridBCMonthly.AllowNewValidValues = False
        Me.AtcGridBCMonthly.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridBCMonthly.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridBCMonthly.Fixed3D = False
        Me.AtcGridBCMonthly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AtcGridBCMonthly.LineColor = System.Drawing.Color.Empty
        Me.AtcGridBCMonthly.LineWidth = 0.0!
        Me.AtcGridBCMonthly.Location = New System.Drawing.Point(0, 97)
        Me.AtcGridBCMonthly.Name = "AtcGridBCMonthly"
        Me.AtcGridBCMonthly.Size = New System.Drawing.Size(521, 320)
        Me.AtcGridBCMonthly.Source = Nothing
        Me.AtcGridBCMonthly.TabIndex = 51
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(15, 480)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(530, 48)
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
        Me.lblStatus.Size = New System.Drawing.Size(505, 14)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Update specifications if desired, then click OK to proceed."
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
        'cmdSimulate
        '
        Me.cmdSimulate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSimulate.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdSimulate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSimulate.Location = New System.Drawing.Point(19, 541)
        Me.cmdSimulate.Name = "cmdSimulate"
        Me.cmdSimulate.Size = New System.Drawing.Size(73, 28)
        Me.cmdSimulate.TabIndex = 10
        Me.cmdSimulate.Text = "Simulate"
        '
        'txtAtempStation
        '
        Me.txtAtempStation.Location = New System.Drawing.Point(163, 263)
        Me.txtAtempStation.Name = "txtAtempStation"
        Me.txtAtempStation.Size = New System.Drawing.Size(338, 20)
        Me.txtAtempStation.TabIndex = 45
        '
        'txtPrecipStation
        '
        Me.txtPrecipStation.Location = New System.Drawing.Point(163, 289)
        Me.txtPrecipStation.Name = "txtPrecipStation"
        Me.txtPrecipStation.Size = New System.Drawing.Size(338, 20)
        Me.txtPrecipStation.TabIndex = 46
        '
        'txtWindStation
        '
        Me.txtWindStation.Location = New System.Drawing.Point(163, 313)
        Me.txtWindStation.Name = "txtWindStation"
        Me.txtWindStation.Size = New System.Drawing.Size(338, 20)
        Me.txtWindStation.TabIndex = 47
        '
        'txtRelHStation
        '
        Me.txtRelHStation.Location = New System.Drawing.Point(163, 341)
        Me.txtRelHStation.Name = "txtRelHStation"
        Me.txtRelHStation.Size = New System.Drawing.Size(338, 20)
        Me.txtRelHStation.TabIndex = 48
        '
        'txtShortRadStation
        '
        Me.txtShortRadStation.Location = New System.Drawing.Point(163, 370)
        Me.txtShortRadStation.Name = "txtShortRadStation"
        Me.txtShortRadStation.Size = New System.Drawing.Size(338, 20)
        Me.txtShortRadStation.TabIndex = 49
        '
        'txtNetRadStation
        '
        Me.txtNetRadStation.Location = New System.Drawing.Point(163, 395)
        Me.txtNetRadStation.Name = "txtNetRadStation"
        Me.txtNetRadStation.Size = New System.Drawing.Size(338, 20)
        Me.txtNetRadStation.TabIndex = 50
        '
        'frmUEB
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(560, 581)
        Me.Controls.Add(Me.cmdSimulate)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmUEB"
        Me.Text = "Utah Energy Balance Snow Accumulation and Melt Model (UEB) for BASINS"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage8.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("UEB for BASINS/MapWindow" & vbCrLf & vbCrLf & "Version 1.01", MsgBoxStyle.OkOnly, "BASINS UEB")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\UEB.html")
    End Sub

    Private Sub frmUEB_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\UEB.html")
        End If
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        cmdCancel.Enabled = aEnabled
        cmdHelp.Enabled = aEnabled
        cmdAbout.Enabled = aEnabled
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)

        lblInstructions.Text = "Select existing UEB project by clicking in the Master Project File box OR " & vbCrLf & _
                               "build new UEB project by defining all fields on all tabs." & vbCrLf & _
                               "Existing data files (e.g. Weather, Parameter, ...) may be accessed on other tabs."

        With AtcGridModelParms
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        AtcGridModelParms.Clear()
        With AtcGridModelParms.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .Rows = 22
            .CellValue(0, 0) = "Model Parameter"
            .CellValue(0, 1) = "Value"
            For i As Integer = 1 To .Rows
                .CellColor(i, 0) = SystemColors.ControlDark
                .CellEditable(i, 1) = True
                .CellValue(i, 0) = clsUEBParameterFile.ParameterName(i - 1)
            Next
        End With
        AtcGridModelParms.ColumnWidth(0) = 400

        With AtcGridSiteVars
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        AtcGridSiteVars.Clear()
        With AtcGridSiteVars.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .Rows = 9
            For i As Integer = 0 To .Rows - 1
                .CellColor(i, 0) = SystemColors.ControlDark
                .CellEditable(i, 1) = True
            Next
            .CellValue(0, 0) = "Site Variable"
            .CellValue(0, 1) = "Value"
            .CellValue(1, 0) = "Forest cover fraction"
            .CellValue(2, 0) = "Drift factor"
            .CellValue(3, 0) = "Atmospheric pressure, Pa"
            .CellValue(4, 0) = "Ground heat flux, kJ/m^2/hr"
            .CellValue(5, 0) = "Albedo extinction depth, m"
            .CellValue(6, 0) = "Slope, degrees"
            .CellValue(7, 0) = "Aspect, degrees from North"
            .CellValue(8, 0) = "Latitude, degrees"
        End With
        AtcGridSiteVars.ColumnWidth(0) = 300

        With AtcGridBCMonthly
            .Source = New atcControls.atcGridSource
            .AllowHorizontalScrolling = False
        End With
        AtcGridBCMonthly.Clear()
        With AtcGridBCMonthly.Source
            .Columns = 2
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .Rows = 13
            For i As Integer = 0 To .Rows - 1
                .CellColor(i, 0) = SystemColors.ControlDark
                .CellEditable(i, 1) = True
            Next
            .CellValue(0, 0) = "Month"
            .CellValue(0, 1) = "Avg. diurnal temp range"
            .CellValue(1, 0) = "January"
            .CellValue(2, 0) = "February"
            .CellValue(3, 0) = "March"
            .CellValue(4, 0) = "April"
            .CellValue(5, 0) = "May"
            .CellValue(6, 0) = "June"
            .CellValue(7, 0) = "July"
            .CellValue(8, 0) = "August"
            .CellValue(9, 0) = "September"
            .CellValue(10, 0) = "October"
            .CellValue(11, 0) = "November"
            .CellValue(12, 0) = "December"
        End With
        AtcGridBCMonthly.ColumnWidth(0) = 300

        pParmData = New clsUEBParameterFile
        pSiteData = New clsUEBSiteFile
    End Sub

    Private Sub lblStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStatus.TextChanged
        Logger.Dbg(lblStatus.Text)
    End Sub

    Private Sub rdoMeasuredNet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoMeasuredNet.CheckedChanged
        If rdoMeasuredNet.Checked Then
            txtNetRadStation.Enabled = True
        Else
            txtNetRadStation.Enabled = False
        End If
    End Sub

    Private Sub rdoRadMeasuredInput_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRadMeasuredInput.CheckedChanged
        If rdoRadMeasuredInput.Checked Then
            txtShortRadStation.Enabled = True
        Else
            txtShortRadStation.Enabled = False
        End If
    End Sub

    Private Sub rdoRadEstimate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRadEstimate.CheckedChanged
        If rdoRadEstimate.Checked Then 'no need for Input Shortwave or Net Radition Timeseries
            txtNetRadStation.Enabled = False
            txtShortRadStation.Enabled = False
        End If
    End Sub

    Private Sub txtWeatherFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeatherFile.Click
        Dim cdlg As New Windows.Forms.OpenFileDialog
        cdlg.Title = "Open UCI file containing base model"
        cdlg.Filter = "UCI files|*.uci|All Files|*.*"
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtWeatherFile.Text = cdlg.FileName
        End If

    End Sub

    Private Sub txtMasterFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMasterFile.Click
        Dim cdlg As New Windows.Forms.OpenFileDialog
        cdlg.Title = "Open UEB master file containing all model data files"
        cdlg.Filter = "Master Input files|*.in|All Files|*.*"
        If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim lFilename As String = cdlg.FileName
            txtMasterFile.Text = lFilename
            OpenMasterFile(lFilename, pWeatherFileName, pOutputFileName, pParmData.FileName, pSiteData.FileName, pBCParameterFileName, pRadOpt)
            txtWeatherFile.Text = pWeatherFileName
            txtOutputFile.Text = pOutputFileName
            txtParameterFile.Text = pParmData.FileName
            txtSiteFile.Text = pSiteData.FileName
            txtBCParameterFile.Text = pBCParameterFileName
            Select Case pRadOpt
                Case 0
                    rdoRadEstimate.Checked = True
                Case 1
                    rdoRadMeasuredInput.Checked = True
                Case 2
                    rdoMeasuredNet.Checked = True
            End Select
        End If
    End Sub

    Private Sub txtWeatherFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeatherFile.TextChanged
        If FileExists(txtWeatherFile.Text) Then
            ReadWeatherFile(txtWeatherFile.Text, pSDate, pTStep, pInitialEnergy, pInitialH2OEquiv, pInitialSnowAge)
            AtcTextSYear.ValueInteger = pSDate(0)
            AtcTextSMonth.ValueInteger = pSDate(1)
            AtcTextSDay.ValueInteger = pSDate(2)
            AtcTextSHour.ValueInteger = pSDate(3)
            atcTextTimeStep.ValueInteger = pTStep
            AtcTextIniEnergyContent.Text = pInitialEnergy
            AtcTextIniWaterEquiv.Text = pInitialH2OEquiv.ToString
            AtcTextSnowAge.Text = pInitialSnowAge
        End If
        pWeatherFileName = txtWeatherFile.Text
    End Sub

    Private Sub txtParameterFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtParameterFile.TextChanged
        If FileExists(txtParameterFile.Text) Then
            ReadDataFile(txtParameterFile.Text, pParmData.ParameterValue)
            For i As Integer = 1 To clsUEBParameterFile.NumParameters
                AtcGridModelParms.Source.CellValue(i, 1) = pParmData.ParameterValue(i - 1)
            Next
        End If
        pParmData.FileName = txtParameterFile.Text
    End Sub

    Private Sub txtSiteFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSiteFile.TextChanged
        If FileExists(txtSiteFile.Text) Then
            ReadDataFile(txtSiteFile.Text, pSiteData.VariableValue)
            For i As Integer = 1 To clsUEBSiteFile.NumVariables
                AtcGridSiteVars.Source.CellValue(i, 1) = pSiteData.VariableValue(i - 1)
            Next
        End If
        pSiteData.FileName = txtSiteFile.Text
    End Sub

    Private Sub txtBCParameterFile_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBCParameterFile.TextChanged
        Dim lMonth As Integer
        If FileExists(txtBCParameterFile.Text) Then
            ReadDataFile(txtBCParameterFile.Text, pBCDataArray)
            AtcTextAParm.ValueDouble = pBCDataArray(0)
            AtcTextCParm.ValueDouble = pBCDataArray(1)
            For i As Integer = 1 To 12
                lMonth = pBCDataArray(3 * i - 1)
                AtcGridBCMonthly.Source.CellValue(lMonth, 1) = pBCDataArray(3 * i)
            Next
        End If
        pBCParameterFileName = txtBCParameterFile.Text
    End Sub

    Private Sub cmdSimulate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSimulate.Click

        UpdateInputFiles()

    End Sub

    Private Sub UpdateInputFiles()
        Dim lMsg As String

        lMsg = ""
        If pParmData.FileName.Length > 0 Then 'check 
            For i As Integer = 1 To clsUEBParameterFile.NumParameters
                If IsNumeric(AtcGridModelParms.Source.CellValue(i, 1)) Then
                    pParmData.ParameterValue(i - 1) = AtcGridModelParms.Source.CellValue(i, 1)
                Else 'problem with a parameter value
                    lMsg = "Problem processing value on Model Parameters tab in row " & i
                    Exit For
                End If
            Next
            If lMsg.Length = 0 Then
                pParmData.WriteParameterFile()
            End If
        End If

        lMsg = ""
        If pSiteData.FileName.Length > 0 Then
            For i As Integer = 1 To clsUEBSiteFile.NumVariables
                If IsNumeric(AtcGridSiteVars.Source.CellValue(i, 1)) Then
                    pSiteData.VariableValue(i - 1) = AtcGridSiteVars.Source.CellValue(i, 1)
                Else 'problem with a site value
                    lMsg = "Problem processing value on Site Variables tab in row " & i
                    Exit For
                End If
            Next
            If lMsg.Length = 0 Then
                pSiteData.WriteSiteFile()
            End If
        End If

        lMsg = ""
        If pBCParameterFileName.Length > 0 Then
            pBCDataArray(0) = AtcTextAParm.ValueDouble
            pBCDataArray(1) = AtcTextCParm.ValueDouble
            Dim lMonth As Integer
            For i As Integer = 1 To 12
                If IsNumeric(AtcGridBCMonthly.Source.CellValue(i, 1)) Then
                    lMonth = pBCDataArray(3 * i - 1)
                    pBCDataArray(3 * i) = AtcGridBCMonthly.Source.CellValue(lMonth, 1)
                Else 'problem with a site value
                    lMsg = "Problem processing value on Bristow-Campbell Parameters tab in row " & i
                    Exit For
                End If
            Next
            If lMsg.Length = 0 Then
                WriteBCParmsFile(pBCParameterFileName, pBCDataArray)
            End If
        End If

    End Sub

End Class