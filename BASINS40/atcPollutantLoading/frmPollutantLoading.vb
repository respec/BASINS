Imports System.Drawing
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcControls

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Friend Class frmPollutantLoading
    Inherits System.Windows.Forms.Form

    Dim gLanduseType As Integer
    Dim gMethod As Integer
    Dim gInitializing As Boolean

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
    Friend WithEvents tabPLOAD As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabLanduse As System.Windows.Forms.TabPage
    Friend WithEvents lblLanduseType As System.Windows.Forms.Label
    Friend WithEvents lblSubbasinsLayer As System.Windows.Forms.Label
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents tabPointSources As System.Windows.Forms.TabPage
    Friend WithEvents tabBMPs As System.Windows.Forms.TabPage
    Friend WithEvents tabValues As System.Windows.Forms.TabPage
    Friend WithEvents rbSimpleMethod As System.Windows.Forms.RadioButton
    Friend WithEvents rbExportCoefficientMethod As System.Windows.Forms.RadioButton
    Friend WithEvents cmdChangeFile As System.Windows.Forms.Button
    Friend WithEvents lblValueFileName As System.Windows.Forms.Label
    Friend WithEvents lblValueFile As System.Windows.Forms.Label
    Friend WithEvents atcGridValues As atcControls.atcGrid
    Friend WithEvents lblValueUnits As System.Windows.Forms.Label
    Friend WithEvents lstConstituents As System.Windows.Forms.ListBox
    Friend WithEvents lblMethod As System.Windows.Forms.Label
    Friend WithEvents lblPollutants As System.Windows.Forms.Label
    Friend WithEvents cboLandUseIDField As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseIDField As System.Windows.Forms.Label
    Friend WithEvents cboBMPLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cbxBMPs As System.Windows.Forms.CheckBox
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents lblArea As System.Windows.Forms.Label
    Friend WithEvents lblLayer As System.Windows.Forms.Label
    Friend WithEvents lblRemoval As System.Windows.Forms.Label
    Friend WithEvents cboBMPType As System.Windows.Forms.ComboBox
    Friend WithEvents cboAreaField As System.Windows.Forms.ComboBox
    Friend WithEvents atcGridBMP As atcControls.atcGrid
    Friend WithEvents cmdChangeBMP As System.Windows.Forms.Button
    Friend WithEvents lblBMPFile As System.Windows.Forms.Label
    Friend WithEvents lblPointID As System.Windows.Forms.Label
    Friend WithEvents lblPointLayer As System.Windows.Forms.Label
    Friend WithEvents atcGridPoint As atcControls.atcGrid
    Friend WithEvents cmdChangePoint As System.Windows.Forms.Button
    Friend WithEvents lblPointLoadFile As System.Windows.Forms.Label
    Friend WithEvents lblPointSourceFile As System.Windows.Forms.Label
    Friend WithEvents cboPointIDField As System.Windows.Forms.ComboBox
    Friend WithEvents cboPointLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cbxPoint As System.Windows.Forms.CheckBox
    Friend WithEvents lblUnits As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents sfdValues As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cmdSavePoint As System.Windows.Forms.Button
    Friend WithEvents cmdSaveBMPs As System.Windows.Forms.Button
    Friend WithEvents tabBank As System.Windows.Forms.TabPage
    Friend WithEvents cmdSaveBank As System.Windows.Forms.Button
    Friend WithEvents lblUnitsBank As System.Windows.Forms.Label
    Friend WithEvents lblSubbasinId As System.Windows.Forms.Label
    Friend WithEvents atcGridBank As atcControls.atcGrid
    Friend WithEvents cmdChangeBank As System.Windows.Forms.Button
    Friend WithEvents lblBankFile As System.Windows.Forms.Label
    Friend WithEvents lblStreambank As System.Windows.Forms.Label
    Friend WithEvents cboSubbasinIDField As System.Windows.Forms.ComboBox
    Friend WithEvents cbxBank As System.Windows.Forms.CheckBox
    Friend WithEvents lblTSS As System.Windows.Forms.Label
    Friend WithEvents lblNoLandUse As System.Windows.Forms.Label
    Friend WithEvents tabPrecip As System.Windows.Forms.TabPage
    Friend WithEvents lblNoprec As System.Windows.Forms.Label
    Friend WithEvents atxRatio As atcControls.atcText
    Friend WithEvents atxPrec As atcControls.atcText
    Friend WithEvents lblRatio As System.Windows.Forms.Label
    Friend WithEvents lblPrecip As System.Windows.Forms.Label
    Friend WithEvents rbMultiple As System.Windows.Forms.RadioButton
    Friend WithEvents rbSingle As System.Windows.Forms.RadioButton
    Friend WithEvents lblPrecSubs As System.Windows.Forms.Label
    Friend WithEvents cboPrecSubbasinField As System.Windows.Forms.ComboBox
    Friend WithEvents atcGridPrec As atcControls.atcGrid
    Friend WithEvents cmdSavePrec As System.Windows.Forms.Button
    Friend WithEvents cmdChangePrec As System.Windows.Forms.Button
    Friend WithEvents lblPrecFileName As System.Windows.Forms.Label
    Friend WithEvents lblPrecFile As System.Windows.Forms.Label
    Friend WithEvents ofdValues As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPollutantLoading))
        Me.tabPLOAD = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.lblMethod = New System.Windows.Forms.Label
        Me.lblPollutants = New System.Windows.Forms.Label
        Me.lstConstituents = New System.Windows.Forms.ListBox
        Me.rbSimpleMethod = New System.Windows.Forms.RadioButton
        Me.rbExportCoefficientMethod = New System.Windows.Forms.RadioButton
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.lblLanduseType = New System.Windows.Forms.Label
        Me.tabPrecip = New System.Windows.Forms.TabPage
        Me.lblPrecFileName = New System.Windows.Forms.Label
        Me.cmdSavePrec = New System.Windows.Forms.Button
        Me.cmdChangePrec = New System.Windows.Forms.Button
        Me.lblPrecFile = New System.Windows.Forms.Label
        Me.atcGridPrec = New atcControls.atcGrid
        Me.rbMultiple = New System.Windows.Forms.RadioButton
        Me.rbSingle = New System.Windows.Forms.RadioButton
        Me.lblPrecSubs = New System.Windows.Forms.Label
        Me.cboPrecSubbasinField = New System.Windows.Forms.ComboBox
        Me.lblNoprec = New System.Windows.Forms.Label
        Me.atxRatio = New atcControls.atcText
        Me.atxPrec = New atcControls.atcText
        Me.lblRatio = New System.Windows.Forms.Label
        Me.lblPrecip = New System.Windows.Forms.Label
        Me.tabLanduse = New System.Windows.Forms.TabPage
        Me.cboLandUseIDField = New System.Windows.Forms.ComboBox
        Me.lblLandUseIDField = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.lblNoLandUse = New System.Windows.Forms.Label
        Me.tabValues = New System.Windows.Forms.TabPage
        Me.cmdSave = New System.Windows.Forms.Button
        Me.lblValueUnits = New System.Windows.Forms.Label
        Me.cmdChangeFile = New System.Windows.Forms.Button
        Me.lblValueFileName = New System.Windows.Forms.Label
        Me.lblValueFile = New System.Windows.Forms.Label
        Me.atcGridValues = New atcControls.atcGrid
        Me.tabPointSources = New System.Windows.Forms.TabPage
        Me.cmdSavePoint = New System.Windows.Forms.Button
        Me.lblUnits = New System.Windows.Forms.Label
        Me.lblPointID = New System.Windows.Forms.Label
        Me.lblPointLayer = New System.Windows.Forms.Label
        Me.atcGridPoint = New atcControls.atcGrid
        Me.cmdChangePoint = New System.Windows.Forms.Button
        Me.lblPointLoadFile = New System.Windows.Forms.Label
        Me.lblPointSourceFile = New System.Windows.Forms.Label
        Me.cboPointIDField = New System.Windows.Forms.ComboBox
        Me.cboPointLayer = New System.Windows.Forms.ComboBox
        Me.cbxPoint = New System.Windows.Forms.CheckBox
        Me.tabBMPs = New System.Windows.Forms.TabPage
        Me.cmdSaveBMPs = New System.Windows.Forms.Button
        Me.atcGridBMP = New atcControls.atcGrid
        Me.cmdChangeBMP = New System.Windows.Forms.Button
        Me.lblBMPFile = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.lblArea = New System.Windows.Forms.Label
        Me.lblLayer = New System.Windows.Forms.Label
        Me.lblRemoval = New System.Windows.Forms.Label
        Me.cboBMPType = New System.Windows.Forms.ComboBox
        Me.cboAreaField = New System.Windows.Forms.ComboBox
        Me.cboBMPLayer = New System.Windows.Forms.ComboBox
        Me.cbxBMPs = New System.Windows.Forms.CheckBox
        Me.tabBank = New System.Windows.Forms.TabPage
        Me.lblTSS = New System.Windows.Forms.Label
        Me.cmdSaveBank = New System.Windows.Forms.Button
        Me.lblUnitsBank = New System.Windows.Forms.Label
        Me.lblSubbasinId = New System.Windows.Forms.Label
        Me.atcGridBank = New atcControls.atcGrid
        Me.cmdChangeBank = New System.Windows.Forms.Button
        Me.lblBankFile = New System.Windows.Forms.Label
        Me.lblStreambank = New System.Windows.Forms.Label
        Me.cboSubbasinIDField = New System.Windows.Forms.ComboBox
        Me.cbxBank = New System.Windows.Forms.CheckBox
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdValues = New System.Windows.Forms.OpenFileDialog
        Me.sfdValues = New System.Windows.Forms.SaveFileDialog
        Me.tabPLOAD.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabPrecip.SuspendLayout()
        Me.tabLanduse.SuspendLayout()
        Me.tabValues.SuspendLayout()
        Me.tabPointSources.SuspendLayout()
        Me.tabBMPs.SuspendLayout()
        Me.tabBank.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPLOAD
        '
        Me.tabPLOAD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPLOAD.Controls.Add(Me.tabGeneral)
        Me.tabPLOAD.Controls.Add(Me.tabPrecip)
        Me.tabPLOAD.Controls.Add(Me.tabLanduse)
        Me.tabPLOAD.Controls.Add(Me.tabValues)
        Me.tabPLOAD.Controls.Add(Me.tabPointSources)
        Me.tabPLOAD.Controls.Add(Me.tabBMPs)
        Me.tabPLOAD.Controls.Add(Me.tabBank)
        Me.tabPLOAD.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabPLOAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPLOAD.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabPLOAD.Location = New System.Drawing.Point(20, 17)
        Me.tabPLOAD.Name = "tabPLOAD"
        Me.tabPLOAD.SelectedIndex = 0
        Me.tabPLOAD.Size = New System.Drawing.Size(675, 300)
        Me.tabPLOAD.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.Color.Transparent
        Me.tabGeneral.Controls.Add(Me.lblMethod)
        Me.tabGeneral.Controls.Add(Me.lblPollutants)
        Me.tabGeneral.Controls.Add(Me.lstConstituents)
        Me.tabGeneral.Controls.Add(Me.rbSimpleMethod)
        Me.tabGeneral.Controls.Add(Me.rbExportCoefficientMethod)
        Me.tabGeneral.Controls.Add(Me.cboSubbasins)
        Me.tabGeneral.Controls.Add(Me.cboLanduse)
        Me.tabGeneral.Controls.Add(Me.lblSubbasinsLayer)
        Me.tabGeneral.Controls.Add(Me.lblLanduseType)
        Me.tabGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(639, 271)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'lblMethod
        '
        Me.lblMethod.AutoSize = True
        Me.lblMethod.Location = New System.Drawing.Point(29, 17)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.Size = New System.Drawing.Size(59, 17)
        Me.lblMethod.TabIndex = 13
        Me.lblMethod.Text = "Method:"
        '
        'lblPollutants
        '
        Me.lblPollutants.AutoSize = True
        Me.lblPollutants.Location = New System.Drawing.Point(230, 17)
        Me.lblPollutants.Name = "lblPollutants"
        Me.lblPollutants.Size = New System.Drawing.Size(74, 17)
        Me.lblPollutants.TabIndex = 12
        Me.lblPollutants.Text = "Pollutants:"
        '
        'lstConstituents
        '
        Me.lstConstituents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstConstituents.FormattingEnabled = True
        Me.lstConstituents.IntegralHeight = False
        Me.lstConstituents.ItemHeight = 17
        Me.lstConstituents.Location = New System.Drawing.Point(234, 41)
        Me.lstConstituents.Name = "lstConstituents"
        Me.lstConstituents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstConstituents.Size = New System.Drawing.Size(373, 110)
        Me.lstConstituents.TabIndex = 11
        '
        'rbSimpleMethod
        '
        Me.rbSimpleMethod.AutoSize = True
        Me.rbSimpleMethod.Location = New System.Drawing.Point(36, 69)
        Me.rbSimpleMethod.Name = "rbSimpleMethod"
        Me.rbSimpleMethod.Size = New System.Drawing.Size(111, 21)
        Me.rbSimpleMethod.TabIndex = 10
        Me.rbSimpleMethod.Text = "Simple (EMC)"
        Me.rbSimpleMethod.UseVisualStyleBackColor = True
        '
        'rbExportCoefficientMethod
        '
        Me.rbExportCoefficientMethod.AutoSize = True
        Me.rbExportCoefficientMethod.Checked = True
        Me.rbExportCoefficientMethod.Location = New System.Drawing.Point(34, 41)
        Me.rbExportCoefficientMethod.Name = "rbExportCoefficientMethod"
        Me.rbExportCoefficientMethod.Size = New System.Drawing.Size(140, 21)
        Me.rbExportCoefficientMethod.TabIndex = 9
        Me.rbExportCoefficientMethod.TabStop = True
        Me.rbExportCoefficientMethod.Text = "Export Coefficient "
        Me.rbExportCoefficientMethod.UseVisualStyleBackColor = True
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(234, 173)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(373, 25)
        Me.cboSubbasins.TabIndex = 8
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Location = New System.Drawing.Point(234, 214)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(373, 25)
        Me.cboLanduse.TabIndex = 7
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(29, 173)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(177, 26)
        Me.lblSubbasinsLayer.TabIndex = 2
        Me.lblSubbasinsLayer.Text = "Subbasins Layer:"
        '
        'lblLanduseType
        '
        Me.lblLanduseType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLanduseType.Location = New System.Drawing.Point(29, 214)
        Me.lblLanduseType.Name = "lblLanduseType"
        Me.lblLanduseType.Size = New System.Drawing.Size(177, 26)
        Me.lblLanduseType.TabIndex = 1
        Me.lblLanduseType.Text = "Land Use Type:"
        '
        'tabPrecip
        '
        Me.tabPrecip.Controls.Add(Me.lblPrecFileName)
        Me.tabPrecip.Controls.Add(Me.cmdSavePrec)
        Me.tabPrecip.Controls.Add(Me.cmdChangePrec)
        Me.tabPrecip.Controls.Add(Me.lblPrecFile)
        Me.tabPrecip.Controls.Add(Me.atcGridPrec)
        Me.tabPrecip.Controls.Add(Me.rbMultiple)
        Me.tabPrecip.Controls.Add(Me.rbSingle)
        Me.tabPrecip.Controls.Add(Me.lblPrecSubs)
        Me.tabPrecip.Controls.Add(Me.cboPrecSubbasinField)
        Me.tabPrecip.Controls.Add(Me.lblNoprec)
        Me.tabPrecip.Controls.Add(Me.atxRatio)
        Me.tabPrecip.Controls.Add(Me.atxPrec)
        Me.tabPrecip.Controls.Add(Me.lblRatio)
        Me.tabPrecip.Controls.Add(Me.lblPrecip)
        Me.tabPrecip.Location = New System.Drawing.Point(4, 25)
        Me.tabPrecip.Name = "tabPrecip"
        Me.tabPrecip.Size = New System.Drawing.Size(639, 271)
        Me.tabPrecip.TabIndex = 6
        Me.tabPrecip.Text = "Precipitation"
        Me.tabPrecip.UseVisualStyleBackColor = True
        '
        'lblPrecFileName
        '
        Me.lblPrecFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPrecFileName.Location = New System.Drawing.Point(143, 116)
        Me.lblPrecFileName.Name = "lblPrecFileName"
        Me.lblPrecFileName.Size = New System.Drawing.Size(300, 23)
        Me.lblPrecFileName.TabIndex = 52
        Me.lblPrecFileName.Text = "<none>"
        Me.lblPrecFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdSavePrec
        '
        Me.cmdSavePrec.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSavePrec.Location = New System.Drawing.Point(450, 113)
        Me.cmdSavePrec.Name = "cmdSavePrec"
        Me.cmdSavePrec.Size = New System.Drawing.Size(77, 27)
        Me.cmdSavePrec.TabIndex = 54
        Me.cmdSavePrec.Text = "Save"
        Me.cmdSavePrec.UseVisualStyleBackColor = True
        '
        'cmdChangePrec
        '
        Me.cmdChangePrec.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangePrec.Location = New System.Drawing.Point(537, 113)
        Me.cmdChangePrec.Name = "cmdChangePrec"
        Me.cmdChangePrec.Size = New System.Drawing.Size(84, 27)
        Me.cmdChangePrec.TabIndex = 53
        Me.cmdChangePrec.Text = "Change"
        '
        'lblPrecFile
        '
        Me.lblPrecFile.AutoSize = True
        Me.lblPrecFile.Location = New System.Drawing.Point(4, 118)
        Me.lblPrecFile.Name = "lblPrecFile"
        Me.lblPrecFile.Size = New System.Drawing.Size(126, 17)
        Me.lblPrecFile.TabIndex = 51
        Me.lblPrecFile.Text = "Annual Precip File:"
        Me.lblPrecFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'atcGridPrec
        '
        Me.atcGridPrec.AllowHorizontalScrolling = True
        Me.atcGridPrec.AllowNewValidValues = False
        Me.atcGridPrec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridPrec.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridPrec.LineColor = System.Drawing.Color.Empty
        Me.atcGridPrec.LineWidth = 0.0!
        Me.atcGridPrec.Location = New System.Drawing.Point(15, 148)
        Me.atcGridPrec.Name = "atcGridPrec"
        Me.atcGridPrec.Size = New System.Drawing.Size(606, 103)
        Me.atcGridPrec.Source = Nothing
        Me.atcGridPrec.TabIndex = 50
        '
        'rbMultiple
        '
        Me.rbMultiple.AutoSize = True
        Me.rbMultiple.Location = New System.Drawing.Point(27, 39)
        Me.rbMultiple.Name = "rbMultiple"
        Me.rbMultiple.Size = New System.Drawing.Size(244, 21)
        Me.rbMultiple.TabIndex = 49
        Me.rbMultiple.Text = "Specify a Value for Each Subbasin"
        Me.rbMultiple.UseVisualStyleBackColor = True
        '
        'rbSingle
        '
        Me.rbSingle.AutoSize = True
        Me.rbSingle.Checked = True
        Me.rbSingle.Location = New System.Drawing.Point(27, 16)
        Me.rbSingle.Name = "rbSingle"
        Me.rbSingle.Size = New System.Drawing.Size(134, 21)
        Me.rbSingle.TabIndex = 48
        Me.rbSingle.TabStop = True
        Me.rbSingle.Text = "Use Single Value"
        Me.rbSingle.UseVisualStyleBackColor = True
        '
        'lblPrecSubs
        '
        Me.lblPrecSubs.AutoSize = True
        Me.lblPrecSubs.Location = New System.Drawing.Point(283, 85)
        Me.lblPrecSubs.Name = "lblPrecSubs"
        Me.lblPrecSubs.Size = New System.Drawing.Size(122, 17)
        Me.lblPrecSubs.TabIndex = 47
        Me.lblPrecSubs.Text = "Subbasin ID Field:"
        Me.lblPrecSubs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboPrecSubbasinField
        '
        Me.cboPrecSubbasinField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPrecSubbasinField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrecSubbasinField.FormattingEnabled = True
        Me.cboPrecSubbasinField.Location = New System.Drawing.Point(421, 81)
        Me.cboPrecSubbasinField.Name = "cboPrecSubbasinField"
        Me.cboPrecSubbasinField.Size = New System.Drawing.Size(200, 25)
        Me.cboPrecSubbasinField.TabIndex = 46
        '
        'lblNoprec
        '
        Me.lblNoprec.AutoSize = True
        Me.lblNoprec.Location = New System.Drawing.Point(55, 34)
        Me.lblNoprec.Name = "lblNoprec"
        Me.lblNoprec.Size = New System.Drawing.Size(475, 17)
        Me.lblNoprec.TabIndex = 22
        Me.lblNoprec.Text = "No precipitation specifications are required when using export coefficients."
        '
        'atxRatio
        '
        Me.atxRatio.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atxRatio.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxRatio.DefaultValue = 0
        Me.atxRatio.HardMax = 1
        Me.atxRatio.HardMin = 0
        Me.atxRatio.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxRatio.Location = New System.Drawing.Point(558, 16)
        Me.atxRatio.MaxWidth = 0
        Me.atxRatio.Name = "atxRatio"
        Me.atxRatio.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxRatio.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxRatio.SelLength = 0
        Me.atxRatio.SelStart = 1
        Me.atxRatio.Size = New System.Drawing.Size(63, 28)
        Me.atxRatio.SoftMax = 0
        Me.atxRatio.SoftMin = 0
        Me.atxRatio.TabIndex = 21
        Me.atxRatio.ValueDouble = 0.0
        '
        'atxPrec
        '
        Me.atxPrec.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxPrec.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atxPrec.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxPrec.DefaultValue = 0
        Me.atxPrec.HardMax = 999
        Me.atxPrec.HardMin = 0
        Me.atxPrec.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxPrec.Location = New System.Drawing.Point(558, 62)
        Me.atxPrec.MaxWidth = 0
        Me.atxPrec.Name = "atxPrec"
        Me.atxPrec.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxPrec.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxPrec.SelLength = 0
        Me.atxPrec.SelStart = 1
        Me.atxPrec.Size = New System.Drawing.Size(63, 25)
        Me.atxPrec.SoftMax = 0
        Me.atxPrec.SoftMin = 0
        Me.atxPrec.TabIndex = 20
        Me.atxPrec.ValueDouble = 0.0
        '
        'lblRatio
        '
        Me.lblRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRatio.AutoSize = True
        Me.lblRatio.Location = New System.Drawing.Point(317, 21)
        Me.lblRatio.Name = "lblRatio"
        Me.lblRatio.Size = New System.Drawing.Size(219, 17)
        Me.lblRatio.TabIndex = 19
        Me.lblRatio.Text = "Ratio of Storms Producing Runoff"
        '
        'lblPrecip
        '
        Me.lblPrecip.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPrecip.AutoSize = True
        Me.lblPrecip.Location = New System.Drawing.Point(383, 64)
        Me.lblPrecip.Name = "lblPrecip"
        Me.lblPrecip.Size = New System.Drawing.Size(159, 17)
        Me.lblPrecip.TabIndex = 18
        Me.lblPrecip.Text = "Annual Precipitation (in)"
        '
        'tabLanduse
        '
        Me.tabLanduse.Controls.Add(Me.cboLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.lblLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.cboLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblNoLandUse)
        Me.tabLanduse.Location = New System.Drawing.Point(4, 25)
        Me.tabLanduse.Name = "tabLanduse"
        Me.tabLanduse.Size = New System.Drawing.Size(639, 271)
        Me.tabLanduse.TabIndex = 1
        Me.tabLanduse.Text = "Land Use"
        Me.tabLanduse.UseVisualStyleBackColor = True
        '
        'cboLandUseIDField
        '
        Me.cboLandUseIDField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseIDField.FormattingEnabled = True
        Me.cboLandUseIDField.Location = New System.Drawing.Point(232, 82)
        Me.cboLandUseIDField.Name = "cboLandUseIDField"
        Me.cboLandUseIDField.Size = New System.Drawing.Size(313, 25)
        Me.cboLandUseIDField.TabIndex = 12
        '
        'lblLandUseIDField
        '
        Me.lblLandUseIDField.AutoSize = True
        Me.lblLandUseIDField.Location = New System.Drawing.Point(48, 86)
        Me.lblLandUseIDField.Name = "lblLandUseIDField"
        Me.lblLandUseIDField.Size = New System.Drawing.Size(124, 17)
        Me.lblLandUseIDField.TabIndex = 11
        Me.lblLandUseIDField.Text = "Land Use ID Field:"
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(232, 34)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(313, 25)
        Me.cboLandUseLayer.TabIndex = 10
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.AutoSize = True
        Me.lblLandUseLayer.Location = New System.Drawing.Point(48, 38)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(113, 17)
        Me.lblLandUseLayer.TabIndex = 9
        Me.lblLandUseLayer.Text = "Land Use Layer:"
        '
        'lblNoLandUse
        '
        Me.lblNoLandUse.AutoSize = True
        Me.lblNoLandUse.Location = New System.Drawing.Point(48, 37)
        Me.lblNoLandUse.Name = "lblNoLandUse"
        Me.lblNoLandUse.Size = New System.Drawing.Size(412, 17)
        Me.lblNoLandUse.TabIndex = 13
        Me.lblNoLandUse.Text = "No land use specifications are required when using GIRAS data."
        '
        'tabValues
        '
        Me.tabValues.Controls.Add(Me.cmdSave)
        Me.tabValues.Controls.Add(Me.lblValueUnits)
        Me.tabValues.Controls.Add(Me.cmdChangeFile)
        Me.tabValues.Controls.Add(Me.lblValueFileName)
        Me.tabValues.Controls.Add(Me.lblValueFile)
        Me.tabValues.Controls.Add(Me.atcGridValues)
        Me.tabValues.Location = New System.Drawing.Point(4, 25)
        Me.tabValues.Name = "tabValues"
        Me.tabValues.Size = New System.Drawing.Size(667, 271)
        Me.tabValues.TabIndex = 4
        Me.tabValues.Text = "Export Coefficients"
        Me.tabValues.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.Location = New System.Drawing.Point(474, 14)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(77, 27)
        Me.cmdSave.TabIndex = 24
        Me.cmdSave.Text = "Save"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'lblValueUnits
        '
        Me.lblValueUnits.AutoSize = True
        Me.lblValueUnits.Location = New System.Drawing.Point(20, 38)
        Me.lblValueUnits.Name = "lblValueUnits"
        Me.lblValueUnits.Size = New System.Drawing.Size(232, 17)
        Me.lblValueUnits.TabIndex = 23
        Me.lblValueUnits.Text = "(lbs/ac/yr, counts/ac/yr for bacteria)"
        '
        'cmdChangeFile
        '
        Me.cmdChangeFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeFile.Location = New System.Drawing.Point(558, 14)
        Me.cmdChangeFile.Name = "cmdChangeFile"
        Me.cmdChangeFile.Size = New System.Drawing.Size(85, 27)
        Me.cmdChangeFile.TabIndex = 22
        Me.cmdChangeFile.Text = "Change"
        '
        'lblValueFileName
        '
        Me.lblValueFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblValueFileName.Location = New System.Drawing.Point(206, 18)
        Me.lblValueFileName.Name = "lblValueFileName"
        Me.lblValueFileName.Size = New System.Drawing.Size(259, 18)
        Me.lblValueFileName.TabIndex = 21
        Me.lblValueFileName.Text = "<none>"
        Me.lblValueFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblValueFile
        '
        Me.lblValueFile.AutoSize = True
        Me.lblValueFile.Location = New System.Drawing.Point(20, 20)
        Me.lblValueFile.Name = "lblValueFile"
        Me.lblValueFile.Size = New System.Drawing.Size(148, 17)
        Me.lblValueFile.TabIndex = 20
        Me.lblValueFile.Text = "Export Coefficient File:"
        Me.lblValueFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'atcGridValues
        '
        Me.atcGridValues.AllowHorizontalScrolling = True
        Me.atcGridValues.AllowNewValidValues = False
        Me.atcGridValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridValues.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridValues.LineColor = System.Drawing.Color.Empty
        Me.atcGridValues.LineWidth = 0.0!
        Me.atcGridValues.Location = New System.Drawing.Point(22, 58)
        Me.atcGridValues.Name = "atcGridValues"
        Me.atcGridValues.Size = New System.Drawing.Size(621, 195)
        Me.atcGridValues.Source = Nothing
        Me.atcGridValues.TabIndex = 19
        '
        'tabPointSources
        '
        Me.tabPointSources.Controls.Add(Me.cmdSavePoint)
        Me.tabPointSources.Controls.Add(Me.lblUnits)
        Me.tabPointSources.Controls.Add(Me.lblPointID)
        Me.tabPointSources.Controls.Add(Me.lblPointLayer)
        Me.tabPointSources.Controls.Add(Me.atcGridPoint)
        Me.tabPointSources.Controls.Add(Me.cmdChangePoint)
        Me.tabPointSources.Controls.Add(Me.lblPointLoadFile)
        Me.tabPointSources.Controls.Add(Me.lblPointSourceFile)
        Me.tabPointSources.Controls.Add(Me.cboPointIDField)
        Me.tabPointSources.Controls.Add(Me.cboPointLayer)
        Me.tabPointSources.Controls.Add(Me.cbxPoint)
        Me.tabPointSources.Location = New System.Drawing.Point(4, 25)
        Me.tabPointSources.Name = "tabPointSources"
        Me.tabPointSources.Size = New System.Drawing.Size(667, 271)
        Me.tabPointSources.TabIndex = 2
        Me.tabPointSources.Text = "Point Sources"
        Me.tabPointSources.UseVisualStyleBackColor = True
        '
        'cmdSavePoint
        '
        Me.cmdSavePoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSavePoint.Location = New System.Drawing.Point(481, 91)
        Me.cmdSavePoint.Name = "cmdSavePoint"
        Me.cmdSavePoint.Size = New System.Drawing.Size(77, 27)
        Me.cmdSavePoint.TabIndex = 38
        Me.cmdSavePoint.Text = "Save"
        Me.cmdSavePoint.UseVisualStyleBackColor = True
        '
        'lblUnits
        '
        Me.lblUnits.Location = New System.Drawing.Point(41, 114)
        Me.lblUnits.Name = "lblUnits"
        Me.lblUnits.Size = New System.Drawing.Size(226, 16)
        Me.lblUnits.TabIndex = 37
        Me.lblUnits.Text = "(lbs/yr or counts/yr)"
        Me.lblUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPointID
        '
        Me.lblPointID.AutoSize = True
        Me.lblPointID.Location = New System.Drawing.Point(196, 53)
        Me.lblPointID.Name = "lblPointID"
        Me.lblPointID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPointID.Size = New System.Drawing.Size(144, 17)
        Me.lblPointID.TabIndex = 36
        Me.lblPointID.Text = "Point Source ID Field:"
        Me.lblPointID.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPointLayer
        '
        Me.lblPointLayer.AutoSize = True
        Me.lblPointLayer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblPointLayer.Location = New System.Drawing.Point(196, 18)
        Me.lblPointLayer.Name = "lblPointLayer"
        Me.lblPointLayer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPointLayer.Size = New System.Drawing.Size(133, 17)
        Me.lblPointLayer.TabIndex = 35
        Me.lblPointLayer.Text = "Point Source Layer:"
        Me.lblPointLayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'atcGridPoint
        '
        Me.atcGridPoint.AllowHorizontalScrolling = True
        Me.atcGridPoint.AllowNewValidValues = False
        Me.atcGridPoint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridPoint.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridPoint.LineColor = System.Drawing.Color.Empty
        Me.atcGridPoint.LineWidth = 0.0!
        Me.atcGridPoint.Location = New System.Drawing.Point(17, 134)
        Me.atcGridPoint.Name = "atcGridPoint"
        Me.atcGridPoint.Size = New System.Drawing.Size(633, 124)
        Me.atcGridPoint.Source = Nothing
        Me.atcGridPoint.TabIndex = 34
        '
        'cmdChangePoint
        '
        Me.cmdChangePoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangePoint.Location = New System.Drawing.Point(565, 91)
        Me.cmdChangePoint.Name = "cmdChangePoint"
        Me.cmdChangePoint.Size = New System.Drawing.Size(85, 27)
        Me.cmdChangePoint.TabIndex = 33
        Me.cmdChangePoint.Text = "Change"
        '
        'lblPointLoadFile
        '
        Me.lblPointLoadFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPointLoadFile.Location = New System.Drawing.Point(188, 95)
        Me.lblPointLoadFile.Name = "lblPointLoadFile"
        Me.lblPointLoadFile.Size = New System.Drawing.Size(283, 19)
        Me.lblPointLoadFile.TabIndex = 32
        Me.lblPointLoadFile.Text = "<none>"
        Me.lblPointLoadFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPointSourceFile
        '
        Me.lblPointSourceFile.Location = New System.Drawing.Point(14, 95)
        Me.lblPointSourceFile.Name = "lblPointSourceFile"
        Me.lblPointSourceFile.Size = New System.Drawing.Size(227, 17)
        Me.lblPointSourceFile.TabIndex = 31
        Me.lblPointSourceFile.Text = "Point Source Loading File:"
        Me.lblPointSourceFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboPointIDField
        '
        Me.cboPointIDField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPointIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPointIDField.FormattingEnabled = True
        Me.cboPointIDField.Location = New System.Drawing.Point(358, 49)
        Me.cboPointIDField.Name = "cboPointIDField"
        Me.cboPointIDField.Size = New System.Drawing.Size(292, 25)
        Me.cboPointIDField.TabIndex = 30
        '
        'cboPointLayer
        '
        Me.cboPointLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPointLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPointLayer.FormattingEnabled = True
        Me.cboPointLayer.Location = New System.Drawing.Point(358, 15)
        Me.cboPointLayer.Name = "cboPointLayer"
        Me.cboPointLayer.Size = New System.Drawing.Size(292, 25)
        Me.cboPointLayer.TabIndex = 29
        '
        'cbxPoint
        '
        Me.cbxPoint.AutoSize = True
        Me.cbxPoint.Location = New System.Drawing.Point(17, 17)
        Me.cbxPoint.Name = "cbxPoint"
        Me.cbxPoint.Size = New System.Drawing.Size(144, 21)
        Me.cbxPoint.TabIndex = 28
        Me.cbxPoint.Text = "Use Point Sources"
        Me.cbxPoint.UseVisualStyleBackColor = True
        '
        'tabBMPs
        '
        Me.tabBMPs.Controls.Add(Me.cmdSaveBMPs)
        Me.tabBMPs.Controls.Add(Me.atcGridBMP)
        Me.tabBMPs.Controls.Add(Me.cmdChangeBMP)
        Me.tabBMPs.Controls.Add(Me.lblBMPFile)
        Me.tabBMPs.Controls.Add(Me.lblType)
        Me.tabBMPs.Controls.Add(Me.lblArea)
        Me.tabBMPs.Controls.Add(Me.lblLayer)
        Me.tabBMPs.Controls.Add(Me.lblRemoval)
        Me.tabBMPs.Controls.Add(Me.cboBMPType)
        Me.tabBMPs.Controls.Add(Me.cboAreaField)
        Me.tabBMPs.Controls.Add(Me.cboBMPLayer)
        Me.tabBMPs.Controls.Add(Me.cbxBMPs)
        Me.tabBMPs.Location = New System.Drawing.Point(4, 25)
        Me.tabBMPs.Name = "tabBMPs"
        Me.tabBMPs.Size = New System.Drawing.Size(667, 271)
        Me.tabBMPs.TabIndex = 3
        Me.tabBMPs.Text = "BMPs"
        Me.tabBMPs.UseVisualStyleBackColor = True
        '
        'cmdSaveBMPs
        '
        Me.cmdSaveBMPs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSaveBMPs.Location = New System.Drawing.Point(479, 122)
        Me.cmdSaveBMPs.Name = "cmdSaveBMPs"
        Me.cmdSaveBMPs.Size = New System.Drawing.Size(77, 27)
        Me.cmdSaveBMPs.TabIndex = 39
        Me.cmdSaveBMPs.Text = "Save"
        Me.cmdSaveBMPs.UseVisualStyleBackColor = True
        '
        'atcGridBMP
        '
        Me.atcGridBMP.AllowHorizontalScrolling = True
        Me.atcGridBMP.AllowNewValidValues = False
        Me.atcGridBMP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridBMP.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridBMP.LineColor = System.Drawing.Color.Empty
        Me.atcGridBMP.LineWidth = 0.0!
        Me.atcGridBMP.Location = New System.Drawing.Point(17, 159)
        Me.atcGridBMP.Name = "atcGridBMP"
        Me.atcGridBMP.Size = New System.Drawing.Size(633, 99)
        Me.atcGridBMP.Source = Nothing
        Me.atcGridBMP.TabIndex = 27
        '
        'cmdChangeBMP
        '
        Me.cmdChangeBMP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeBMP.Location = New System.Drawing.Point(565, 122)
        Me.cmdChangeBMP.Name = "cmdChangeBMP"
        Me.cmdChangeBMP.Size = New System.Drawing.Size(85, 27)
        Me.cmdChangeBMP.TabIndex = 26
        Me.cmdChangeBMP.Text = "Change"
        '
        'lblBMPFile
        '
        Me.lblBMPFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBMPFile.Location = New System.Drawing.Point(225, 124)
        Me.lblBMPFile.Name = "lblBMPFile"
        Me.lblBMPFile.Size = New System.Drawing.Size(247, 24)
        Me.lblBMPFile.TabIndex = 25
        Me.lblBMPFile.Text = "<none>"
        Me.lblBMPFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(139, 53)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(111, 17)
        Me.lblType.TabIndex = 24
        Me.lblType.Text = "BMP Type Field:"
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblArea
        '
        Me.lblArea.AutoSize = True
        Me.lblArea.Location = New System.Drawing.Point(139, 85)
        Me.lblArea.Name = "lblArea"
        Me.lblArea.Size = New System.Drawing.Size(76, 17)
        Me.lblArea.TabIndex = 23
        Me.lblArea.Text = "Area Field:"
        Me.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLayer
        '
        Me.lblLayer.AutoSize = True
        Me.lblLayer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblLayer.Location = New System.Drawing.Point(139, 18)
        Me.lblLayer.Name = "lblLayer"
        Me.lblLayer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLayer.Size = New System.Drawing.Size(81, 17)
        Me.lblLayer.TabIndex = 22
        Me.lblLayer.Text = "BMP Layer:"
        Me.lblLayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRemoval
        '
        Me.lblRemoval.AutoSize = True
        Me.lblRemoval.Location = New System.Drawing.Point(13, 128)
        Me.lblRemoval.Name = "lblRemoval"
        Me.lblRemoval.Size = New System.Drawing.Size(206, 17)
        Me.lblRemoval.TabIndex = 21
        Me.lblRemoval.Text = "% BMP Removal Efficiency File:"
        Me.lblRemoval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBMPType
        '
        Me.cboBMPType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPType.FormattingEnabled = True
        Me.cboBMPType.Location = New System.Drawing.Point(273, 49)
        Me.cboBMPType.Name = "cboBMPType"
        Me.cboBMPType.Size = New System.Drawing.Size(377, 25)
        Me.cboBMPType.TabIndex = 3
        '
        'cboAreaField
        '
        Me.cboAreaField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAreaField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAreaField.FormattingEnabled = True
        Me.cboAreaField.Location = New System.Drawing.Point(273, 81)
        Me.cboAreaField.Name = "cboAreaField"
        Me.cboAreaField.Size = New System.Drawing.Size(377, 25)
        Me.cboAreaField.TabIndex = 2
        '
        'cboBMPLayer
        '
        Me.cboBMPLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPLayer.FormattingEnabled = True
        Me.cboBMPLayer.Location = New System.Drawing.Point(273, 15)
        Me.cboBMPLayer.Name = "cboBMPLayer"
        Me.cboBMPLayer.Size = New System.Drawing.Size(377, 25)
        Me.cboBMPLayer.TabIndex = 1
        '
        'cbxBMPs
        '
        Me.cbxBMPs.AutoSize = True
        Me.cbxBMPs.Location = New System.Drawing.Point(17, 17)
        Me.cbxBMPs.Name = "cbxBMPs"
        Me.cbxBMPs.Size = New System.Drawing.Size(92, 21)
        Me.cbxBMPs.TabIndex = 0
        Me.cbxBMPs.Text = "Use BMPs"
        Me.cbxBMPs.UseVisualStyleBackColor = True
        '
        'tabBank
        '
        Me.tabBank.Controls.Add(Me.lblTSS)
        Me.tabBank.Controls.Add(Me.cmdSaveBank)
        Me.tabBank.Controls.Add(Me.lblUnitsBank)
        Me.tabBank.Controls.Add(Me.lblSubbasinId)
        Me.tabBank.Controls.Add(Me.atcGridBank)
        Me.tabBank.Controls.Add(Me.cmdChangeBank)
        Me.tabBank.Controls.Add(Me.lblBankFile)
        Me.tabBank.Controls.Add(Me.lblStreambank)
        Me.tabBank.Controls.Add(Me.cboSubbasinIDField)
        Me.tabBank.Controls.Add(Me.cbxBank)
        Me.tabBank.Location = New System.Drawing.Point(4, 25)
        Me.tabBank.Name = "tabBank"
        Me.tabBank.Size = New System.Drawing.Size(667, 271)
        Me.tabBank.TabIndex = 5
        Me.tabBank.Text = "Bank Erosion"
        Me.tabBank.UseVisualStyleBackColor = True
        '
        'lblTSS
        '
        Me.lblTSS.AutoSize = True
        Me.lblTSS.Location = New System.Drawing.Point(41, 42)
        Me.lblTSS.Name = "lblTSS"
        Me.lblTSS.Size = New System.Drawing.Size(144, 17)
        Me.lblTSS.TabIndex = 48
        Me.lblTSS.Text = "(Only Applies to TSS)"
        '
        'cmdSaveBank
        '
        Me.cmdSaveBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSaveBank.Location = New System.Drawing.Point(481, 84)
        Me.cmdSaveBank.Name = "cmdSaveBank"
        Me.cmdSaveBank.Size = New System.Drawing.Size(77, 27)
        Me.cmdSaveBank.TabIndex = 47
        Me.cmdSaveBank.Text = "Save"
        Me.cmdSaveBank.UseVisualStyleBackColor = True
        '
        'lblUnitsBank
        '
        Me.lblUnitsBank.AutoSize = True
        Me.lblUnitsBank.Location = New System.Drawing.Point(41, 113)
        Me.lblUnitsBank.Name = "lblUnitsBank"
        Me.lblUnitsBank.Size = New System.Drawing.Size(52, 17)
        Me.lblUnitsBank.TabIndex = 46
        Me.lblUnitsBank.Text = "(lbs/yr)"
        Me.lblUnitsBank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSubbasinId
        '
        Me.lblSubbasinId.AutoSize = True
        Me.lblSubbasinId.Location = New System.Drawing.Point(284, 18)
        Me.lblSubbasinId.Name = "lblSubbasinId"
        Me.lblSubbasinId.Size = New System.Drawing.Size(122, 17)
        Me.lblSubbasinId.TabIndex = 45
        Me.lblSubbasinId.Text = "Subbasin ID Field:"
        Me.lblSubbasinId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'atcGridBank
        '
        Me.atcGridBank.AllowHorizontalScrolling = True
        Me.atcGridBank.AllowNewValidValues = False
        Me.atcGridBank.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridBank.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridBank.LineColor = System.Drawing.Color.Empty
        Me.atcGridBank.LineWidth = 0.0!
        Me.atcGridBank.Location = New System.Drawing.Point(17, 133)
        Me.atcGridBank.Name = "atcGridBank"
        Me.atcGridBank.Size = New System.Drawing.Size(633, 124)
        Me.atcGridBank.Source = Nothing
        Me.atcGridBank.TabIndex = 44
        '
        'cmdChangeBank
        '
        Me.cmdChangeBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeBank.Location = New System.Drawing.Point(565, 84)
        Me.cmdChangeBank.Name = "cmdChangeBank"
        Me.cmdChangeBank.Size = New System.Drawing.Size(85, 27)
        Me.cmdChangeBank.TabIndex = 43
        Me.cmdChangeBank.Text = "Change"
        '
        'lblBankFile
        '
        Me.lblBankFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBankFile.Location = New System.Drawing.Point(196, 90)
        Me.lblBankFile.Name = "lblBankFile"
        Me.lblBankFile.Size = New System.Drawing.Size(276, 17)
        Me.lblBankFile.TabIndex = 42
        Me.lblBankFile.Text = "<none>"
        Me.lblBankFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblStreambank
        '
        Me.lblStreambank.AutoSize = True
        Me.lblStreambank.Location = New System.Drawing.Point(13, 90)
        Me.lblStreambank.Name = "lblStreambank"
        Me.lblStreambank.Size = New System.Drawing.Size(177, 17)
        Me.lblStreambank.TabIndex = 41
        Me.lblStreambank.Text = "Bank Erosion Loading File:"
        Me.lblStreambank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboSubbasinIDField
        '
        Me.cboSubbasinIDField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasinIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasinIDField.FormattingEnabled = True
        Me.cboSubbasinIDField.Location = New System.Drawing.Point(423, 15)
        Me.cboSubbasinIDField.Name = "cboSubbasinIDField"
        Me.cboSubbasinIDField.Size = New System.Drawing.Size(227, 25)
        Me.cboSubbasinIDField.TabIndex = 40
        '
        'cbxBank
        '
        Me.cbxBank.AutoSize = True
        Me.cbxBank.Location = New System.Drawing.Point(17, 17)
        Me.cbxBank.Name = "cbxBank"
        Me.cbxBank.Size = New System.Drawing.Size(160, 21)
        Me.cbxBank.TabIndex = 39
        Me.cbxBank.Text = "Include Bank Erosion"
        Me.cbxBank.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(20, 338)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(119, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Generate"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(146, 338)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(102, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(489, 338)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(94, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(593, 338)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(102, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "&About"
        '
        'ofdValues
        '
        Me.ofdValues.DefaultExt = "dbf"
        Me.ofdValues.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdValues.Title = "Select File"
        '
        'sfdValues
        '
        Me.sfdValues.DefaultExt = "dbf"
        Me.sfdValues.Filter = "DBF Files (*.dbf)|*.dbf"
        '
        'frmPollutantLoading
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 16)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(713, 386)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabPLOAD)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmPollutantLoading"
        Me.Text = "BASINS Pollutant Loading Estimator"
        Me.tabPLOAD.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabPrecip.ResumeLayout(False)
        Me.tabPrecip.PerformLayout()
        Me.tabLanduse.ResumeLayout(False)
        Me.tabLanduse.PerformLayout()
        Me.tabValues.ResumeLayout(False)
        Me.tabValues.PerformLayout()
        Me.tabPointSources.ResumeLayout(False)
        Me.tabPointSources.PerformLayout()
        Me.tabBMPs.ResumeLayout(False)
        Me.tabBMPs.PerformLayout()
        Me.tabBank.ResumeLayout(False)
        Me.tabBank.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Logger.Dbg("UserCanceled")
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        Dim lLyr As Integer
        Dim lDef As Integer

        Logger.Dbg("LandUseChangedTo " & cboLanduse.Items(cboLanduse.SelectedIndex))
        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboLandUseIDField.Visible = False
            lblLandUseIDField.Visible = False
            lblNoLandUse.Visible = True
        Else
            lblNoLandUse.Visible = False
            cboLandUseLayer.Items.Clear()
            lDef = 0
            For lLyr = 0 To GisUtil.NumLayers() - 1
                If cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                    If GisUtil.LayerType(lLyr) = 3 Then
                        cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                    End If
                    cboLandUseIDField.Visible = True
                    lblLandUseIDField.Visible = True
                ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
                    If GisUtil.LayerFileName(lLyr).ToUpper.Contains("\NLCD\") Then
                        cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                    End If
                    cboLandUseIDField.Visible = False
                    lblLandUseIDField.Visible = False
                ElseIf GisUtil.LayerType(lLyr) = 4 Then
                    cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                    cboLandUseIDField.Visible = False
                    lblLandUseIDField.Visible = False
                End If
            Next
            If cboLandUseLayer.Items.Count > 0 And cboLandUseLayer.SelectedIndex < 0 Then
                cboLandUseLayer.SelectedIndex = 0
            End If
            cboLandUseLayer.Visible = True
            lblLandUseLayer.Visible = True
        End If

        Logger.Dbg("SetGridValues")
        lblValueFileName.Text = "<none>"
        SetGridValues()
        gLanduseType = cboLanduse.SelectedIndex
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        Logger.Dbg("LandUseLayerChangedTo " & cboLanduse.Items(cboLanduse.SelectedIndex))
        If cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            PopulateLandUseFields()
        End If
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        Logger.Msg("BASINS Pollutant Loading Estimator" & vbCrLf & vbCrLf & _
                   "Version " & System.Reflection.Assembly.GetEntryAssembly.GetName.Version.ToString, _
                   "BASINS - PLOAD")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\PLOAD.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim i As Integer

        If lstConstituents.SelectedItems.Count = 0 Then
            Logger.Message("At least one pollutant must be selected", "Pollutant Loading Estimator Problem", _
                           Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Stop, Windows.Forms.MessageBoxDefaultButton.Button1)
        Else
            If lblValueFileName.Text <> "<none>" Then
                Logger.Dbg("GenerateLoads")
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim lConstituents As New atcCollection
                For i = 1 To lstConstituents.SelectedItems.Count
                    lConstituents.Add(lstConstituents.SelectedItems(i - 1))
                Next

                Dim lLandUseLayer As String = ""
                If cboLandUseLayer.SelectedIndex > -1 Then
                    lLandUseLayer = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                End If

                Dim lLandUseId As String = ""
                If cboLandUseIDField.SelectedIndex > -1 Then
                    lLandUseId = cboLandUseIDField.Items(cboLandUseIDField.SelectedIndex)
                End If

                Dim lBMPAreaField As String = ""
                If cboAreaField.SelectedIndex > -1 Then
                    lBMPAreaField = cboAreaField.Items(cboAreaField.SelectedIndex)
                End If

                Dim lBMPTypeField As String = ""
                If cboBMPType.SelectedIndex > -1 Then
                    lBMPTypeField = cboBMPType.Items(cboBMPType.SelectedIndex)
                End If

                Dim lPointIdField As String = ""
                If cboPointIDField.SelectedIndex > -1 Then
                    lPointIdField = cboPointIDField.Items(cboPointIDField.SelectedIndex)
                End If

                Dim lBmps As PollutantLoadingBMPs = Nothing
                If cbxBMPs.Checked Then
                    lBmps = New PollutantLoadingBMPs( _
                                cboBMPLayer.Items(cboBMPLayer.SelectedIndex), _
                                lBMPAreaField, _
                                lBMPTypeField, _
                                atcGridBMP.Source)
                End If

                Dim lPointLoads As PollutantLoadingPointLoads = Nothing
                If cbxPoint.Checked Then
                    lPointLoads = New PollutantLoadingPointLoads( _
                                cboPointLayer.Items(cboPointLayer.SelectedIndex), _
                                lPointIdField, _
                                atcGridPoint.Source)
                End If

                Dim lStreamBankLoads As PollutantLoadingStreamBankLoads = Nothing
                If cbxBank.Checked Then
                    lStreamBankLoads = New PollutantLoadingStreamBankLoads( _
                                     cboSubbasinIDField.Items(cboSubbasinIDField.SelectedIndex), _
                                     atcGridBank.Source)
                End If

                'set precip array
                Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
                Dim lNumSubbasins As Integer = GisUtil.NumFeatures(lSubbasinLayerIndex)
                Dim lPrec(lNumSubbasins) As Double
                If rbSingle.Checked Then
                    For i = 0 To lNumSubbasins - 1
                        lPrec(i) = atxPrec.ValueDouble
                    Next i
                Else
                    For i = 0 To lNumSubbasins - 1
                        lPrec(i) = atcGridPrec.Source.CellValue(i + 1, 2)
                    Next i
                End If

                GenerateLoads(cboSubbasins.Items(cboSubbasins.SelectedIndex), _
                              atcGridValues.Source, _
                              rbExportCoefficientMethod.Checked, _
                              cboLanduse.Items(cboLanduse.SelectedIndex), _
                              lLandUseLayer, _
                              lLandUseId, _
                              lPrec, _
                              atxRatio.ValueDouble, _
                              lConstituents, _
                              lBmps, _
                              lPointLoads, _
                              lStreamBankLoads)

                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                Me.Close()
            Else
                'TODO: add an error message
            End If
        End If
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        cmdOK.Enabled = b
        cmdHelp.Enabled = b
        cmdCancel.Enabled = b
        cmdAbout.Enabled = b
        tabPLOAD.Enabled = b
    End Sub

    Public Sub InitializeUI()
        Dim lTemp As String

        gInitializing = True

        With atcGridValues
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With
        With atcGridBMP
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With
        With atcGridPoint
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With
        With atcGridBank
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With
        With atcGridPrec
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")
        cboPointLayer.Items.Add("<none>")
        cboBMPLayer.Items.Add("<none>")

        gLanduseType = GetSetting("PLOAD", "UserDefault", "LandUse", "0")
        gMethod = GetSetting("PLOAD", "UserDefault", "Method", "0")

        Dim lLyr As Integer
        Dim lSelectedLayer As Integer = -1
        Dim lSelectedLayerLastResort As Integer = -1
        For lLyr = 0 To GisUtil.NumLayers() - 1
            lTemp = GisUtil.LayerName(lLyr)
            If GisUtil.LayerType(lLyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(lTemp)
                If (UCase(lTemp) = "SUBBASINS" Or InStr(lTemp, "Watershed Shapefile") > 0) And lSelectedLayer = -1 Then
                    lSelectedLayer = cboSubbasins.Items.Count - 1
                End If
                If GisUtil.CurrentLayer = lLyr And GisUtil.NumFeatures(lLyr) < 1000 Then
                    lSelectedLayer = cboSubbasins.Items.Count - 1
                ElseIf GisUtil.NumFeatures(lLyr) < 1000 Then
                    'this one ok as last resort 
                    lSelectedLayerLastResort = cboSubbasins.Items.Count - 1
                End If
                'also possible bmp layer
                cboBMPLayer.Items.Add(lTemp)
                If InStr(lTemp, "BMP") > 0 Then
                    cboBMPLayer.SelectedIndex = cboBMPLayer.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLyr) = 1 Then
                'PointShapefile
                'possible bmp layer
                cboBMPLayer.Items.Add(lTemp)
                If InStr(lTemp, "BMP") > 0 Then
                    cboBMPLayer.SelectedIndex = cboBMPLayer.Items.Count - 1
                End If
                'possible point source layer
                cboPointLayer.Items.Add(lTemp)
                If InStr(lTemp, "Permit Compliance") > 0 Then
                    cboPointLayer.SelectedIndex = cboPointLayer.Items.Count - 1
                End If
            End If
        Next

        gInitializing = False
        'if all else fails set it to the first one
        If lSelectedLayer > -1 Then
            cboSubbasins.SelectedIndex = lSelectedLayer
        ElseIf cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            If lSelectedLayerLastResort > -1 Then
                cboSubbasins.SelectedIndex = lSelectedLayerLastResort
            Else
                cboSubbasins.SelectedIndex = 0
            End If
        End If
        If cboBMPLayer.Items.Count > 0 And cboBMPLayer.SelectedIndex < 0 Then
            cboBMPLayer.SelectedIndex = 0
        End If
        If cboPointLayer.Items.Count > 0 And cboPointLayer.SelectedIndex < 0 Then
            cboPointLayer.SelectedIndex = 0
        End If

        'SetBMPGridValues()

        cboLanduse.SelectedIndex = gLanduseType
        If gMethod = 0 Then
            rbExportCoefficientMethod.Checked = True
            rbSimpleMethod.Checked = False
        Else
            rbExportCoefficientMethod.Checked = False
            rbSimpleMethod.Checked = True
        End If

        atxPrec.ValueDouble = 40.0
        atxRatio.ValueDouble = 0.9
    End Sub

    Private Sub frmPollutantLoading_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveSetting("PLOAD", "UserDefault", "LandUse", gLanduseType)
        SaveSetting("PLOAD", "UserDefault", "Method", gMethod)
    End Sub

    Private Sub frmPollutantLoading_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\PLOAD.html")
        End If
    End Sub

    Private Sub rbExportCoefficientMethod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbExportCoefficientMethod.CheckedChanged
        If rbExportCoefficientMethod.Checked = True Then
            tabValues.Text = "Export Coefficients"
            lblRatio.Visible = False
            lblPrecip.Visible = False
            atxRatio.Visible = False
            atxPrec.Visible = False
            rbSingle.Visible = False
            rbMultiple.Visible = False
            lblPrecSubs.Visible = False
            cboPrecSubbasinField.Visible = False
            atcGridPrec.Visible = False
            lblPrecFile.Visible = False
            lblPrecFileName.Visible = False
            cmdSavePrec.Visible = False
            cmdChangePrec.Visible = False
            lblNoprec.Visible = True
            lblValueFile.Text = "Export Coefficient File"
            lblValueUnits.Text = "(lbs/ac/yr, counts/ac/yr for bacteria)"
            rbSingle.Checked = True
            gMethod = 0
        Else
            tabValues.Text = "Event Mean Concentrations"
            lblRatio.Visible = True
            lblPrecip.Visible = True
            atxRatio.Visible = True
            atxPrec.Visible = True
            lblNoprec.Visible = False
            rbSingle.Visible = True
            rbMultiple.Visible = True
            lblValueFile.Text = "EMC File"
            lblValueUnits.Text = "(mg/L, counts/100mL for bacteria)"
            gMethod = 1
        End If
        lblValueFileName.Text = "<none>"
        SetGridValues()
    End Sub

    Private Sub cmdChangeFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeFile.Click
        If rbExportCoefficientMethod.Checked Then
            ofdValues.Title = "Set Export Coefficient File"
        Else
            ofdValues.Title = "Set Event Mean Concentration File"
        End If
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblValueFileName.Text = ofdValues.FileName
            SetGridValues()
        End If
    End Sub

    Private Sub PopulateLandUseFields()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex))
        cboLandUseIDField.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboLandUseIDField.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboLandUseIDField.Items.Count > 0 And cboLandUseIDField.SelectedIndex < 0 Then
            cboLandUseIDField.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateBMPFields()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboBMPLayer.Items(cboBMPLayer.SelectedIndex))
        cboAreaField.Items.Clear()
        cboBMPType.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboAreaField.Items.Add(GisUtil.FieldName(i, lLyr))
            cboBMPType.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboAreaField.Items.Count > 0 And cboAreaField.SelectedIndex < 0 Then
            cboAreaField.SelectedIndex = 0
        End If
        If cboBMPType.Items.Count > 0 And cboBMPType.SelectedIndex < 0 Then
            cboBMPType.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulatePointFields()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboPointLayer.Items(cboPointLayer.SelectedIndex))
        cboPointIDField.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboPointIDField.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboPointIDField.Items.Count > 0 And cboPointIDField.SelectedIndex < 0 Then
            cboPointIDField.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateSubbasinsFields()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        cboSubbasinIDField.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboSubbasinIDField.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboSubbasinIDField.Items.Count > 0 And cboSubbasinIDField.SelectedIndex < 0 Then
            cboSubbasinIDField.SelectedIndex = 0
        End If

        cboPrecSubbasinField.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboPrecSubbasinField.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboPrecSubbasinField.Items.Count > 0 And cboPrecSubbasinField.SelectedIndex < 0 Then
            cboPrecSubbasinField.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateSubbasinsFieldsForPointSources()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        cboPointIDField.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboPointIDField.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboPointIDField.Items.Count > 0 And cboPointIDField.SelectedIndex < 0 Then
            cboPointIDField.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateSubbasinsFieldsForBMPs()
        Dim lLyr As Integer
        Dim i As Integer

        lLyr = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
        cboBMPType.Items.Clear()
        For i = 0 To GisUtil.NumFields(lLyr) - 1
            cboBMPType.Items.Add(GisUtil.FieldName(i, lLyr))
        Next i

        If cboBMPType.Items.Count > 0 And cboBMPType.SelectedIndex < 0 Then
            cboBMPType.SelectedIndex = 0
        End If
    End Sub

    Private Sub SetGridValues()
        Dim lSorted As New atcCollection
        Dim lDefaultFile As String
        Dim i As Integer

        If atcGridValues.Source Is Nothing Then Exit Sub
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        If rbExportCoefficientMethod.Checked = True Then
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\ecgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\ecgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\ecnlcd.dbf"
            Else 'grid
                lDefaultFile = lBasinsFolder & "\etc\pload\ecnlcd.dbf"
            End If
        Else
            'emc (simple) method
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\emcgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\emcgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
                lDefaultFile = lBasinsFolder & "\etc\pload\emcnlcd.dbf"
            Else 'grid
                lDefaultFile = lBasinsFolder & "\etc\pload\emcnlcd.dbf"
            End If
        End If

        If lblValueFileName.Text = "<none>" Then
            lblValueFileName.Text = lDefaultFile
        End If

        'remember which pollutants are selected
        Dim lSelectedPollutantIndices As New Collection
        For i = 1 To lstConstituents.SelectedIndices.Count
            lSelectedPollutantIndices.Add(lstConstituents.SelectedIndices(i - 1))
        Next i
        'refresh file
        atcGridValues.Clear()
        If lblValueFileName.Text <> "<none>" Then
            SetGridValuesSource(lblValueFileName.Text, atcGridValues.Source)
        End If
        atcGridValues.SizeAllColumnsToContents()
        atcGridValues.Refresh()
        SetPollutantList()
        'set selected pollutants back again
        If lstConstituents.Items.Count > 0 Then
            For i = 1 To lSelectedPollutantIndices.Count
                lstConstituents.SelectedIndices.Add(lSelectedPollutantIndices(i))
            Next i
        End If
        If lstConstituents.Items.Count > 0 And lstConstituents.SelectedItems.Count = 0 Then
            lstConstituents.SelectedItems.Add(lstConstituents.Items(0))
        End If

    End Sub

    Private Sub SetPollutantList()
        Dim i As Integer
        Dim lOffset As Integer

        lstConstituents.Items.Clear()
        If rbExportCoefficientMethod.Checked Then
            lOffset = 3
        Else
            'emc method (has extra column for impervious)
            lOffset = 4
        End If
        For i = 0 To atcGridValues.Source.Columns - lOffset - 1
            lstConstituents.Items.Add(atcGridValues.Source.CellValue(0, i + lOffset))
        Next i
        lstConstituents.SelectedItems.Clear()
    End Sub

    Private Sub cboBMPLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBMPLayer.SelectedIndexChanged
        Logger.Dbg("BMPLayerChangedTo " & cboBMPLayer.Items(cboBMPLayer.SelectedIndex))
        If cboBMPLayer.Items(cboBMPLayer.SelectedIndex) <> "<none>" Then
            lblType.Text = "BMP Type Field:"
            If GisUtil.LayerType(GisUtil.LayerIndex(cboBMPLayer.Items(cboBMPLayer.SelectedIndex))) = 3 Then
                cboAreaField.Visible = False
                lblArea.Visible = False
            Else
                'area field just applies to point bmps
                cboAreaField.Visible = True
                lblArea.Visible = True
            End If
            PopulateBMPFields()
            atcGridBMP.Source.Rows = 0
            atcGridBMP.Source.Columns = 0
            atcGridBMP.Clear()
        Else
            cboAreaField.Visible = False
            lblArea.Visible = False
            lblType.Text = "Subbasin ID Field:"
            lblBMPFile.Text = "<none>"
            PopulateSubbasinsFieldsForBMPs()
        End If
    End Sub

    Private Sub cboPointLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPointLayer.SelectedIndexChanged
        Logger.Dbg("PointLoadLayerChangedTo " & cboPointLayer.Items(cboPointLayer.SelectedIndex))
        If cboPointLayer.Items(cboPointLayer.SelectedIndex) <> "<none>" Then
            lblPointID.Text = "Point Source ID Field:"
            PopulatePointFields()
            atcGridPoint.Source.Rows = 0
            atcGridPoint.Source.Columns = 0
            atcGridPoint.Clear()
        Else
            lblPointID.Text = "Subbasin ID Field:"
            PopulateSubbasinsFieldsForPointSources()
        End If
    End Sub

    Private Sub SetBMPGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable
        Dim lSorted As New atcCollection

        If atcGridBMP.Source Is Nothing Then Exit Sub

        If lblBMPFile.Text = "<none>" Then
            Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
            lblBMPFile.Text = lBasinsFolder & "\etc\pload\bmpeffic.dbf"
        End If
        atcGridBMP.Clear()

        If lblBMPFile.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblBMPFile.Text)

            With atcGridBMP.Source
                .Rows = 1
                .Columns = lDbf.NumFields
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                For i = 1 To lDbf.NumFields
                    .CellValue(0, i) = lDbf.FieldName(i)
                    .CellColor(0, i) = SystemColors.ControlDark
                Next i

                For k = 1 To lDbf.NumRecords
                    lDbf.CurrentRecord = k
                    For i = 1 To lDbf.NumFields
                        .CellValue(k, i) = lDbf.Value(i)
                        If i > 2 Then
                            .CellEditable(k, i) = True
                        Else
                            .CellColor(k, i) = SystemColors.ControlDark
                        End If
                    Next i
                Next k
            End With

        End If
        atcGridBMP.SizeAllColumnsToContents()
        atcGridBMP.Refresh()
    End Sub

    Private Sub SetBankGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable

        If atcGridBank.Source Is Nothing Then Exit Sub

        If lblBankFile.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblBankFile.Text)

            With atcGridBank.Source
                For i = 1 To .Rows
                    'loop thru each row of grid
                    For k = 1 To lDbf.NumRecords
                        'loop thru each row of dbf
                        lDbf.CurrentRecord = k
                        If .CellValue(i, 1) = lDbf.Value(1) Then
                            'found a match, insert load value
                            .CellValue(i, 2) = lDbf.Value(2)
                        End If
                    Next k
                Next i
            End With

        End If
        atcGridBank.SizeAllColumnsToContents()
        atcGridBank.Refresh()
    End Sub

    Private Sub SetPrecGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable

        If atcGridPrec.Source Is Nothing Then Exit Sub

        If lblPrecFileName.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblPrecFileName.Text)

            With atcGridPrec.Source
                For i = 1 To .Rows
                    'loop thru each row of grid
                    For k = 1 To lDbf.NumRecords
                        'loop thru each row of dbf
                        lDbf.CurrentRecord = k
                        If .CellValue(i, 1) = lDbf.Value(1) Then
                            'found a match, insert prec value from file
                            .CellValue(i, 2) = lDbf.Value(2)
                        End If
                    Next k
                Next i
            End With

        End If
        atcGridPrec.SizeAllColumnsToContents()
        atcGridPrec.Refresh()
    End Sub

    Private Sub SetPointGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable
        Dim lSorted As New atcCollection

        If atcGridPoint.Source Is Nothing Then Exit Sub

        'lblPointLoadFile.Text = "\BASINS\etc\pload\pointload.dbf"
        atcGridPoint.Clear()

        If lblPointLoadFile.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblPointLoadFile.Text)

            With atcGridPoint.Source
                .Rows = 1
                .Columns = lDbf.NumFields
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                For i = 1 To lDbf.NumFields
                    .CellValue(0, i) = lDbf.FieldName(i)
                    .CellColor(0, i) = SystemColors.ControlDark
                Next i

                For k = 1 To lDbf.NumRecords
                    lDbf.CurrentRecord = k
                    For i = 1 To lDbf.NumFields
                        .CellValue(k, i) = lDbf.Value(i)
                        If i > 1 Then
                            .CellEditable(k, i) = True
                        Else
                            .CellColor(k, i) = SystemColors.ControlDark
                        End If
                    Next i
                Next k
            End With

        End If
        atcGridPoint.SizeAllColumnsToContents()
        atcGridPoint.Refresh()
    End Sub

    Private Sub cmdChangeBMP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeBMP.Click
        ofdValues.Title = "Set BMP Removal Efficiency File"
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblBMPFile.Text = ofdValues.FileName
            If cboBMPLayer.Items(cboBMPLayer.SelectedIndex) <> "<none>" Then
                SetBMPGridValues()
            Else
                SetBMPGridValuesForDefaultTableFromFile()
            End If
        End If
    End Sub

    Private Sub cmdChangePoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangePoint.Click
        ofdValues.Title = "Set Point Source Loading File"
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblPointLoadFile.Text = ofdValues.FileName
            If cboPointLayer.Items(cboPointLayer.SelectedIndex) <> "<none>" Then
                SetPointGridValues()
            Else
                SetPointGridValuesForDefaultTableFromFile()
            End If
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If rbExportCoefficientMethod.Checked Then
            SaveGrid("Save Export Coefficient File", atcGridValues.Source)
        Else
            SaveGrid("Save Event Mean Concentration File", atcGridValues.Source)
        End If
    End Sub

    Private Sub SaveGrid(ByVal aCaption As String, ByVal aSource As atcControls.atcGridSource)
        Dim dbfname As String
        Dim tmpDbf As IatcTable
        Dim i As Integer
        Dim j As Integer

        sfdValues.Title = aCaption
        If sfdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            dbfname = sfdValues.FileName
            'does this dbf already exist?
            If FileExists(dbfname) Then
                'delete this file first
                TryDelete(dbfname)
            End If
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(dbfname)
            tmpDbf.NumFields = aSource.Columns - 1
            For i = 1 To tmpDbf.NumFields
                tmpDbf.FieldName(i) = aSource.CellValue(0, i)
                tmpDbf.FieldType(i) = "C"
                tmpDbf.FieldLength(i) = 10
            Next i

            'now write out main grid
            For i = 1 To aSource.Rows - 1
                tmpDbf.CurrentRecord = i
                For j = 1 To aSource.Columns - 1
                    tmpDbf.Value(j) = aSource.CellValue(i, j)
                Next j
            Next i
            tmpDbf.WriteFile(dbfname)
        End If
    End Sub

    Private Sub cmdSavePoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSavePoint.Click
        SaveGrid("Save Point Source Loading File", atcGridPoint.Source)
    End Sub

    Private Sub cmdSaveBMPs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveBMPs.Click
        SaveGrid("Save BMP Removal Efficiency File", atcGridBMP.Source)
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSubbasins.SelectedIndexChanged
        Logger.Dbg("SubbainsLayerChangedTo " & cboSubbasins.Items(cboSubbasins.SelectedIndex))
        PopulateSubbasinsFields()
    End Sub

    Private Sub cmdSaveBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveBank.Click
        SaveGrid("Save Bank Erosion Loading File", atcGridBank.Source)
    End Sub

    Private Sub cmdSavePrec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSavePrec.Click
        SaveGrid("Save Precipitation File", atcGridPrec.Source)
    End Sub

    Private Sub cmdChangeBank_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeBank.Click
        ofdValues.Title = "Set Bank Erosion Loading File"
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblBankFile.Text = ofdValues.FileName
            SetBankGridValues()
        End If
    End Sub

    Private Sub cmdChangePrec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangePrec.Click
        ofdValues.Title = "Set Precipitation File"
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblPrecFileName.Text = ofdValues.FileName
            SetPrecGridValues()
        End If
    End Sub

    Private Sub cboSubbasinIDField_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubbasinIDField.SelectedIndexChanged
        Dim k As Integer
        Dim lSubbasinLayerIndex As Integer
        Dim lSubbasinFieldIndex As Integer

        If Not gInitializing Then

            If atcGridBank.Source Is Nothing Then Exit Sub

            atcGridBank.Clear()

            With atcGridBank.Source
                .Rows = 1
                .Columns = 2
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellValue(0, 1) = cboSubbasinIDField.Items(cboSubbasinIDField.SelectedIndex)
                .CellColor(0, 1) = SystemColors.ControlDark
                .CellValue(0, 2) = "LOAD"
                .CellColor(0, 2) = SystemColors.ControlDark
                lSubbasinLayerIndex = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
                lSubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, cboSubbasinIDField.Items(cboSubbasinIDField.SelectedIndex))
                For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    .CellValue(k, 1) = GisUtil.FieldValue(lSubbasinLayerIndex, k - 1, lSubbasinFieldIndex) 'subid
                    .CellColor(k, 1) = SystemColors.ControlDark
                    .CellValue(k, 2) = 0
                    .CellEditable(k, 2) = True
                Next
            End With

            atcGridBank.SizeAllColumnsToContents()
            atcGridBank.Refresh()
        End If
    End Sub

    Private Sub rbSingle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSingle.CheckedChanged
        If Not rbExportCoefficientMethod.Checked Then
            If rbSingle.Checked = True Then
                atxRatio.Visible = True
                atxPrec.Visible = True
                lblPrecip.Visible = True
                lblPrecSubs.Visible = False
                cboPrecSubbasinField.Visible = False
                lblPrecFile.Visible = False
                lblPrecFileName.Visible = False
                cmdSavePrec.Visible = False
                cmdChangePrec.Visible = False
                atcGridPrec.Visible = False
            Else
                atxRatio.Visible = True
                atxPrec.Visible = False
                lblPrecSubs.Visible = True
                lblPrecip.Visible = False
                cboPrecSubbasinField.Visible = True
                lblPrecFile.Visible = True
                lblPrecFileName.Visible = True
                cmdSavePrec.Visible = True
                cmdChangePrec.Visible = True
                atcGridPrec.Visible = True
            End If
        End If
    End Sub

    Private Sub cboPrecSubbasinField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPrecSubbasinField.SelectedIndexChanged
        Dim k As Integer
        Dim lSubbasinLayerIndex As Integer
        Dim lSubbasinFieldIndex As Integer

        If Not gInitializing Then
            If atcGridPrec.Source Is Nothing Then Exit Sub

            atcGridPrec.Clear()

            With atcGridPrec.Source
                .Rows = 1
                .Columns = 2
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellValue(0, 1) = cboSubbasinIDField.Items(cboSubbasinIDField.SelectedIndex)
                .CellColor(0, 1) = SystemColors.ControlDark
                .CellValue(0, 2) = "Precip (in/yr)"
                .CellColor(0, 2) = SystemColors.ControlDark
                lSubbasinLayerIndex = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
                lSubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, cboSubbasinIDField.Items(cboSubbasinIDField.SelectedIndex))
                For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    .CellValue(k, 1) = GisUtil.FieldValue(lSubbasinLayerIndex, k - 1, lSubbasinFieldIndex) 'subid
                    .CellColor(k, 1) = SystemColors.ControlDark
                    .CellValue(k, 2) = 40
                    .CellEditable(k, 2) = True
                Next
            End With

            atcGridPrec.SizeAllColumnsToContents()
            atcGridPrec.Refresh()
        End If
    End Sub

    Private Sub cboPointIDField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPointIDField.SelectedIndexChanged
        If Not gInitializing And cboPointLayer.Items(cboPointLayer.SelectedIndex) = "<none>" Then
            SetPointGridValuesForDefaultTable()
        End If
    End Sub

    Private Sub SetPointGridValuesForDefaultTable()
        Dim k As Integer
        Dim i As Integer
        Dim lSubbasinLayerIndex As Integer
        Dim lSubbasinFieldIndex As Integer

        If Not gInitializing And cboPointLayer.Items(cboPointLayer.SelectedIndex) = "<none>" Then
            If atcGridPoint.Source Is Nothing Then Exit Sub

            atcGridPoint.Clear()
            lSubbasinLayerIndex = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
            lSubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, cboPointIDField.Items(cboPointIDField.SelectedIndex))

            With atcGridPoint.Source
                .Rows = 1
                .Columns = 1 + lstConstituents.SelectedItems.Count
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellValue(0, 1) = cboPointIDField.Items(cboPointIDField.SelectedIndex)
                .CellColor(0, 1) = SystemColors.ControlDark
                For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    .CellValue(k, 1) = GisUtil.FieldValue(lSubbasinLayerIndex, k - 1, lSubbasinFieldIndex) 'subid
                    .CellColor(k, 1) = SystemColors.ControlDark
                Next
                For i = 1 To lstConstituents.SelectedItems.Count
                    .CellValue(0, 1 + i) = lstConstituents.SelectedItems(i - 1)
                    .CellColor(0, 1 + i) = SystemColors.ControlDark
                    For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                        .CellValue(k, 1 + i) = 0
                        .CellEditable(k, 1 + i) = True
                    Next
                Next i
            End With

            atcGridPoint.SizeAllColumnsToContents()
            atcGridPoint.Refresh()
        End If
    End Sub

    Private Sub SetPointGridValuesForDefaultTableFromFile()
        Dim i As Integer, k As Integer, j As Integer
        Dim lDbf As IatcTable

        If atcGridPoint.Source Is Nothing Then Exit Sub

        If lblPointLoadFile.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblPointLoadFile.Text)

            With atcGridPoint.Source
                For i = 1 To .Rows
                    'loop thru each row of grid
                    For k = 1 To lDbf.NumRecords
                        'loop thru each row of dbf
                        lDbf.CurrentRecord = k
                        If .CellValue(i, 1) = lDbf.Value(1) Then
                            'found a match, insert point source values from file
                            For j = 2 To .Columns - 1
                                .CellValue(i, j) = lDbf.Value(j)
                            Next j
                        End If
                    Next k
                Next i
            End With

        End If
        atcGridPoint.SizeAllColumnsToContents()
        atcGridPoint.Refresh()
    End Sub

    Private Sub lstConstituents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstConstituents.SelectedIndexChanged
        If Not gInitializing And cboPointLayer.Items(cboPointLayer.SelectedIndex) = "<none>" Then
            SetPointGridValuesForDefaultTable()
        End If
        If Not gInitializing And cboBMPLayer.Items(cboBMPLayer.SelectedIndex) = "<none>" Then
            SetBMPGridValuesForDefaultTable()
        End If
    End Sub

    Private Sub cboBMPType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBMPType.SelectedIndexChanged
        If Not gInitializing And cboBMPLayer.Items(cboBMPLayer.SelectedIndex) = "<none>" Then
            SetBMPGridValuesForDefaultTable()
        End If
    End Sub

    Private Sub SetBMPGridValuesForDefaultTable()
        Dim k As Integer
        Dim i As Integer
        Dim lSubbasinLayerIndex As Integer
        Dim lSubbasinFieldIndex As Integer

        If Not gInitializing And cboBMPLayer.Items(cboBMPLayer.SelectedIndex) = "<none>" Then
            If atcGridBMP.Source Is Nothing Then Exit Sub

            atcGridBMP.Clear()
            lSubbasinLayerIndex = GisUtil.LayerIndex(cboSubbasins.Items(cboSubbasins.SelectedIndex))
            lSubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, cboBMPType.Items(cboBMPType.SelectedIndex))

            With atcGridBMP.Source
                .Rows = 1
                .Columns = 1 + lstConstituents.SelectedItems.Count
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                .CellValue(0, 1) = cboBMPType.Items(cboBMPType.SelectedIndex)
                .CellColor(0, 1) = SystemColors.ControlDark
                For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    .CellValue(k, 1) = GisUtil.FieldValue(lSubbasinLayerIndex, k - 1, lSubbasinFieldIndex) 'subid
                    .CellColor(k, 1) = SystemColors.ControlDark
                Next
                For i = 1 To lstConstituents.SelectedItems.Count
                    .CellValue(0, 1 + i) = lstConstituents.SelectedItems(i - 1)
                    .CellColor(0, 1 + i) = SystemColors.ControlDark
                    For k = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                        .CellValue(k, 1 + i) = 0
                        .CellEditable(k, 1 + i) = True
                    Next
                Next i
            End With

            atcGridBMP.SizeAllColumnsToContents()
            atcGridBMP.Refresh()
        End If
    End Sub

    Private Sub SetBMPGridValuesForDefaultTableFromFile()
        Dim i As Integer, k As Integer, j As Integer
        Dim lDbf As IatcTable

        If atcGridBMP.Source Is Nothing Then Exit Sub

        If lblBMPFile.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblBMPFile.Text)

            With atcGridBMP.Source
                For i = 1 To .Rows
                    'loop thru each row of grid
                    For k = 1 To lDbf.NumRecords
                        'loop thru each row of dbf
                        lDbf.CurrentRecord = k
                        If .CellValue(i, 1) = lDbf.Value(1) Then
                            'found a match, insert bmp removal values from file
                            For j = 2 To .Columns - 1
                                .CellValue(i, j) = lDbf.Value(j)
                            Next j
                        End If
                    Next k
                Next i
            End With

        End If
        atcGridBMP.SizeAllColumnsToContents()
        atcGridBMP.Refresh()
    End Sub

End Class