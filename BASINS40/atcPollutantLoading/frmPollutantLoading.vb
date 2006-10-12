Imports System.Drawing
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcControls

Public Class frmModelSetup
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents lblRatio As System.Windows.Forms.Label
    Friend WithEvents lblPrecip As System.Windows.Forms.Label
    Friend WithEvents atxRatio As atcControls.atcText
    Friend WithEvents atxPrec As atcControls.atcText
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
    Friend WithEvents ofdValues As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSetup))
        Me.tabPLOAD = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.atxRatio = New atcControls.atcText
        Me.atxPrec = New atcControls.atcText
        Me.lblRatio = New System.Windows.Forms.Label
        Me.lblPrecip = New System.Windows.Forms.Label
        Me.lblMethod = New System.Windows.Forms.Label
        Me.lblPollutants = New System.Windows.Forms.Label
        Me.lstConstituents = New System.Windows.Forms.ListBox
        Me.rbSimpleMethod = New System.Windows.Forms.RadioButton
        Me.rbExportCoefficientMethod = New System.Windows.Forms.RadioButton
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.lblLanduseType = New System.Windows.Forms.Label
        Me.tabLanduse = New System.Windows.Forms.TabPage
        Me.cboLandUseIDField = New System.Windows.Forms.ComboBox
        Me.lblLandUseIDField = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.tabValues = New System.Windows.Forms.TabPage
        Me.lblValueUnits = New System.Windows.Forms.Label
        Me.cmdChangeFile = New System.Windows.Forms.Button
        Me.lblValueFileName = New System.Windows.Forms.Label
        Me.lblValueFile = New System.Windows.Forms.Label
        Me.atcGridValues = New atcControls.atcGrid
        Me.tabPointSources = New System.Windows.Forms.TabPage
        Me.tabBMPs = New System.Windows.Forms.TabPage
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
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdValues = New System.Windows.Forms.OpenFileDialog
        Me.tabPLOAD.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabLanduse.SuspendLayout()
        Me.tabValues.SuspendLayout()
        Me.tabBMPs.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPLOAD
        '
        Me.tabPLOAD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPLOAD.Controls.Add(Me.tabGeneral)
        Me.tabPLOAD.Controls.Add(Me.tabLanduse)
        Me.tabPLOAD.Controls.Add(Me.tabValues)
        Me.tabPLOAD.Controls.Add(Me.tabPointSources)
        Me.tabPLOAD.Controls.Add(Me.tabBMPs)
        Me.tabPLOAD.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabPLOAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPLOAD.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabPLOAD.Location = New System.Drawing.Point(16, 16)
        Me.tabPLOAD.Name = "tabPLOAD"
        Me.tabPLOAD.SelectedIndex = 0
        Me.tabPLOAD.Size = New System.Drawing.Size(631, 276)
        Me.tabPLOAD.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.tabGeneral.Controls.Add(Me.atxRatio)
        Me.tabGeneral.Controls.Add(Me.atxPrec)
        Me.tabGeneral.Controls.Add(Me.lblRatio)
        Me.tabGeneral.Controls.Add(Me.lblPrecip)
        Me.tabGeneral.Controls.Add(Me.lblMethod)
        Me.tabGeneral.Controls.Add(Me.lblPollutants)
        Me.tabGeneral.Controls.Add(Me.lstConstituents)
        Me.tabGeneral.Controls.Add(Me.rbSimpleMethod)
        Me.tabGeneral.Controls.Add(Me.rbExportCoefficientMethod)
        Me.tabGeneral.Controls.Add(Me.cboSubbasins)
        Me.tabGeneral.Controls.Add(Me.cboLanduse)
        Me.tabGeneral.Controls.Add(Me.lblSubbasinsLayer)
        Me.tabGeneral.Controls.Add(Me.lblLanduseType)
        Me.tabGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(623, 247)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'atxRatio
        '
        Me.atxRatio.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxRatio.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxRatio.DefaultValue = 0
        Me.atxRatio.HardMax = 1
        Me.atxRatio.HardMin = 0
        Me.atxRatio.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxRatio.Location = New System.Drawing.Point(265, 120)
        Me.atxRatio.MaxDecimal = 0
        Me.atxRatio.maxWidth = 0
        Me.atxRatio.Name = "atxRatio"
        Me.atxRatio.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxRatio.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxRatio.SelLength = 0
        Me.atxRatio.SelStart = 1
        Me.atxRatio.Size = New System.Drawing.Size(55, 26)
        Me.atxRatio.SoftMax = 0
        Me.atxRatio.SoftMin = 0
        Me.atxRatio.TabIndex = 17
        Me.atxRatio.Value = 0.0!
        '
        'atxPrec
        '
        Me.atxPrec.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxPrec.DataType = atcControls.atcText.ATCoDataType.ATCoSng
        Me.atxPrec.DefaultValue = 0
        Me.atxPrec.HardMax = 999
        Me.atxPrec.HardMin = 0
        Me.atxPrec.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atxPrec.Location = New System.Drawing.Point(265, 93)
        Me.atxPrec.MaxDecimal = 0
        Me.atxPrec.maxWidth = 0
        Me.atxPrec.Name = "atxPrec"
        Me.atxPrec.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atxPrec.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atxPrec.SelLength = 0
        Me.atxPrec.SelStart = 1
        Me.atxPrec.Size = New System.Drawing.Size(55, 24)
        Me.atxPrec.SoftMax = 0
        Me.atxPrec.SoftMin = 0
        Me.atxPrec.TabIndex = 16
        Me.atxPrec.Value = 0.0!
        '
        'lblRatio
        '
        Me.lblRatio.AutoSize = True
        Me.lblRatio.Location = New System.Drawing.Point(40, 120)
        Me.lblRatio.Name = "lblRatio"
        Me.lblRatio.Size = New System.Drawing.Size(219, 17)
        Me.lblRatio.TabIndex = 15
        Me.lblRatio.Text = "Ratio of Storms Producing Runoff"
        '
        'lblPrecip
        '
        Me.lblPrecip.AutoSize = True
        Me.lblPrecip.Location = New System.Drawing.Point(40, 93)
        Me.lblPrecip.Name = "lblPrecip"
        Me.lblPrecip.Size = New System.Drawing.Size(159, 17)
        Me.lblPrecip.TabIndex = 14
        Me.lblPrecip.Text = "Annual Precipitation (in)"
        '
        'lblMethod
        '
        Me.lblMethod.AutoSize = True
        Me.lblMethod.Location = New System.Drawing.Point(40, 16)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.Size = New System.Drawing.Size(59, 17)
        Me.lblMethod.TabIndex = 13
        Me.lblMethod.Text = "Method:"
        '
        'lblPollutants
        '
        Me.lblPollutants.AutoSize = True
        Me.lblPollutants.Location = New System.Drawing.Point(336, 16)
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
        Me.lstConstituents.ItemHeight = 16
        Me.lstConstituents.Location = New System.Drawing.Point(358, 38)
        Me.lstConstituents.Name = "lstConstituents"
        Me.lstConstituents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstConstituents.Size = New System.Drawing.Size(237, 100)
        Me.lstConstituents.TabIndex = 11
        '
        'rbSimpleMethod
        '
        Me.rbSimpleMethod.AutoSize = True
        Me.rbSimpleMethod.Location = New System.Drawing.Point(58, 65)
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
        Me.rbExportCoefficientMethod.Location = New System.Drawing.Point(58, 38)
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
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(201, 164)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(343, 24)
        Me.cboSubbasins.TabIndex = 8
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Location = New System.Drawing.Point(201, 201)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(343, 24)
        Me.cboLanduse.TabIndex = 7
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(25, 164)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(152, 24)
        Me.lblSubbasinsLayer.TabIndex = 2
        Me.lblSubbasinsLayer.Text = "Subbasins Layer:"
        '
        'lblLanduseType
        '
        Me.lblLanduseType.Location = New System.Drawing.Point(25, 201)
        Me.lblLanduseType.Name = "lblLanduseType"
        Me.lblLanduseType.Size = New System.Drawing.Size(152, 24)
        Me.lblLanduseType.TabIndex = 1
        Me.lblLanduseType.Text = "Land Use Type:"
        '
        'tabLanduse
        '
        Me.tabLanduse.Controls.Add(Me.cboLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.lblLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.cboLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblLandUseLayer)
        Me.tabLanduse.Location = New System.Drawing.Point(4, 25)
        Me.tabLanduse.Name = "tabLanduse"
        Me.tabLanduse.Size = New System.Drawing.Size(623, 247)
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
        Me.cboLandUseIDField.Location = New System.Drawing.Point(200, 78)
        Me.cboLandUseIDField.Name = "cboLandUseIDField"
        Me.cboLandUseIDField.Size = New System.Drawing.Size(269, 24)
        Me.cboLandUseIDField.TabIndex = 12
        '
        'lblLandUseIDField
        '
        Me.lblLandUseIDField.AutoSize = True
        Me.lblLandUseIDField.Location = New System.Drawing.Point(40, 81)
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
        Me.cboLandUseLayer.Location = New System.Drawing.Point(200, 32)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(343, 24)
        Me.cboLandUseLayer.TabIndex = 10
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.Location = New System.Drawing.Point(40, 32)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(152, 24)
        Me.lblLandUseLayer.TabIndex = 9
        Me.lblLandUseLayer.Text = "Land Use Layer:"
        '
        'tabValues
        '
        Me.tabValues.Controls.Add(Me.lblValueUnits)
        Me.tabValues.Controls.Add(Me.cmdChangeFile)
        Me.tabValues.Controls.Add(Me.lblValueFileName)
        Me.tabValues.Controls.Add(Me.lblValueFile)
        Me.tabValues.Controls.Add(Me.atcGridValues)
        Me.tabValues.Location = New System.Drawing.Point(4, 25)
        Me.tabValues.Name = "tabValues"
        Me.tabValues.Size = New System.Drawing.Size(623, 247)
        Me.tabValues.TabIndex = 4
        Me.tabValues.Text = "Export Coefficients"
        Me.tabValues.UseVisualStyleBackColor = True
        '
        'lblValueUnits
        '
        Me.lblValueUnits.AutoSize = True
        Me.lblValueUnits.Location = New System.Drawing.Point(17, 32)
        Me.lblValueUnits.Name = "lblValueUnits"
        Me.lblValueUnits.Size = New System.Drawing.Size(71, 17)
        Me.lblValueUnits.TabIndex = 23
        Me.lblValueUnits.Text = "(lbs/ac/yr)"
        '
        'cmdChangeFile
        '
        Me.cmdChangeFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeFile.Location = New System.Drawing.Point(530, 12)
        Me.cmdChangeFile.Name = "cmdChangeFile"
        Me.cmdChangeFile.Size = New System.Drawing.Size(73, 24)
        Me.cmdChangeFile.TabIndex = 22
        Me.cmdChangeFile.Text = "Change"
        '
        'lblValueFileName
        '
        Me.lblValueFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblValueFileName.Location = New System.Drawing.Point(176, 16)
        Me.lblValueFileName.Name = "lblValueFileName"
        Me.lblValueFileName.Size = New System.Drawing.Size(346, 16)
        Me.lblValueFileName.TabIndex = 21
        Me.lblValueFileName.Text = "<none>"
        Me.lblValueFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblValueFile
        '
        Me.lblValueFile.Location = New System.Drawing.Point(17, 16)
        Me.lblValueFile.Name = "lblValueFile"
        Me.lblValueFile.Size = New System.Drawing.Size(153, 16)
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
        Me.atcGridValues.Location = New System.Drawing.Point(20, 54)
        Me.atcGridValues.Name = "atcGridValues"
        Me.atcGridValues.Size = New System.Drawing.Size(583, 177)
        Me.atcGridValues.Source = Nothing
        Me.atcGridValues.TabIndex = 19
        '
        'tabPointSources
        '
        Me.tabPointSources.Location = New System.Drawing.Point(4, 25)
        Me.tabPointSources.Name = "tabPointSources"
        Me.tabPointSources.Size = New System.Drawing.Size(623, 247)
        Me.tabPointSources.TabIndex = 2
        Me.tabPointSources.Text = "Point Sources"
        Me.tabPointSources.UseVisualStyleBackColor = True
        '
        'tabBMPs
        '
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
        Me.tabBMPs.Size = New System.Drawing.Size(623, 247)
        Me.tabBMPs.TabIndex = 3
        Me.tabBMPs.Text = "BMPs"
        Me.tabBMPs.UseVisualStyleBackColor = True
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
        Me.atcGridBMP.Location = New System.Drawing.Point(15, 141)
        Me.atcGridBMP.Name = "atcGridBMP"
        Me.atcGridBMP.Size = New System.Drawing.Size(594, 96)
        Me.atcGridBMP.Source = Nothing
        Me.atcGridBMP.TabIndex = 27
        '
        'cmdChangeBMP
        '
        Me.cmdChangeBMP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeBMP.Location = New System.Drawing.Point(539, 116)
        Me.cmdChangeBMP.Name = "cmdChangeBMP"
        Me.cmdChangeBMP.Size = New System.Drawing.Size(71, 24)
        Me.cmdChangeBMP.TabIndex = 26
        Me.cmdChangeBMP.Text = "Change"
        '
        'lblBMPFile
        '
        Me.lblBMPFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBMPFile.Location = New System.Drawing.Point(213, 120)
        Me.lblBMPFile.Name = "lblBMPFile"
        Me.lblBMPFile.Size = New System.Drawing.Size(318, 16)
        Me.lblBMPFile.TabIndex = 25
        Me.lblBMPFile.Text = "<none>"
        Me.lblBMPFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(114, 49)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(111, 17)
        Me.lblType.TabIndex = 24
        Me.lblType.Text = "BMP Type Field:"
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblArea
        '
        Me.lblArea.AutoSize = True
        Me.lblArea.Location = New System.Drawing.Point(149, 79)
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
        Me.lblLayer.Location = New System.Drawing.Point(147, 17)
        Me.lblLayer.Name = "lblLayer"
        Me.lblLayer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLayer.Size = New System.Drawing.Size(81, 17)
        Me.lblLayer.TabIndex = 22
        Me.lblLayer.Text = "BMP Layer:"
        Me.lblLayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRemoval
        '
        Me.lblRemoval.Location = New System.Drawing.Point(12, 120)
        Me.lblRemoval.Name = "lblRemoval"
        Me.lblRemoval.Size = New System.Drawing.Size(195, 16)
        Me.lblRemoval.TabIndex = 21
        Me.lblRemoval.Text = "BMP Removal Efficiency File:"
        Me.lblRemoval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboBMPType
        '
        Me.cboBMPType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPType.FormattingEnabled = True
        Me.cboBMPType.Location = New System.Drawing.Point(234, 46)
        Me.cboBMPType.Name = "cboBMPType"
        Me.cboBMPType.Size = New System.Drawing.Size(246, 24)
        Me.cboBMPType.TabIndex = 3
        '
        'cboAreaField
        '
        Me.cboAreaField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAreaField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAreaField.FormattingEnabled = True
        Me.cboAreaField.Location = New System.Drawing.Point(234, 76)
        Me.cboAreaField.Name = "cboAreaField"
        Me.cboAreaField.Size = New System.Drawing.Size(246, 24)
        Me.cboAreaField.TabIndex = 2
        '
        'cboBMPLayer
        '
        Me.cboBMPLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPLayer.FormattingEnabled = True
        Me.cboBMPLayer.Location = New System.Drawing.Point(234, 14)
        Me.cboBMPLayer.Name = "cboBMPLayer"
        Me.cboBMPLayer.Size = New System.Drawing.Size(301, 24)
        Me.cboBMPLayer.TabIndex = 1
        '
        'cbxBMPs
        '
        Me.cbxBMPs.AutoSize = True
        Me.cbxBMPs.Location = New System.Drawing.Point(15, 16)
        Me.cbxBMPs.Name = "cbxBMPs"
        Me.cbxBMPs.Size = New System.Drawing.Size(92, 21)
        Me.cbxBMPs.TabIndex = 0
        Me.cbxBMPs.Text = "Use BMPs"
        Me.cbxBMPs.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 310)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(103, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Generate"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(125, 310)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(471, 310)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(80, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(559, 310)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(88, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "&About"
        '
        'ofdValues
        '
        Me.ofdValues.DefaultExt = "dbf"
        Me.ofdValues.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdValues.Title = "Select File"
        '
        'frmModelSetup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(663, 355)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabPLOAD)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmModelSetup"
        Me.Text = "BASINS Pollutant Loading Estimator"
        Me.tabPLOAD.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabLanduse.ResumeLayout(False)
        Me.tabLanduse.PerformLayout()
        Me.tabValues.ResumeLayout(False)
        Me.tabValues.PerformLayout()
        Me.tabBMPs.ResumeLayout(False)
        Me.tabBMPs.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Logger.Dbg("UserCanceled")
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        Dim lLyr As Long
        Dim lDef As Integer

        Logger.Dbg("LandUseChangedTo " & cboLanduse.Items(cboLanduse.SelectedIndex))
        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            cboLandUseLayer.Visible = False
            lblLandUseLayer.Visible = False
            cboLandUseIDField.Visible = False
            lblLandUseIDField.Visible = False
        Else
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
                    If InStr(GisUtil.LayerFileName(lLyr), "\nlcd\") > 0 Then
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
        SetGridValues()
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
        'TODO: update with PLOAD help
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If lstConstituents.SelectedItems.Count = 0 Then
            Logger.Message("At least one pollutant must be selected", "Pollutant Loading Estimator Problem", _
                           Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Stop, Windows.Forms.MessageBoxDefaultButton.Button1)
        Else
            If lblValueFileName.Text <> "<none>" Then
                Logger.Dbg("GenerateLoads")
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim lConstituents As New atcCollection
                For i As Integer = 1 To lstConstituents.SelectedItems.Count
                    lConstituents.Add(lstConstituents.SelectedItems(i - 1))
                Next

                Dim lLandUseLayer As String = ""
                If cboLandUseLayer.SelectedIndex > -1 Then
                    llanduselayer = cboLandUseLayer.Items(cboLandUseLayer.SelectedIndex)
                End If

                Dim lLandUseId As String = ""
                If cboLandUseIDField.SelectedIndex > -1 Then
                    lLandUseId = cboLandUseIDField.Items(cboLandUseIDField.SelectedIndex)
                End If

                GenerateLoads(cboSubbasins.Items(cboSubbasins.SelectedIndex), _
                              atcGridValues.Source, _
                              rbExportCoefficientMethod.Checked, _
                              cboLanduse.Items(cboLanduse.SelectedIndex), _
                              lLandUseLayer, _
                              lLandUseId, _
                              atxPrec.Value, _
                              atxRatio.Value, _
                              lConstituents)

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

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")

        Dim lLyr As Integer
        Dim lSelectedLayer As Integer = -1
        For lLyr = 0 To GisUtil.NumLayers() - 1
            lTemp = GisUtil.LayerName(lLyr)
            If GisUtil.LayerType(lLyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(lTemp)
                If UCase(lTemp) = "SUBBASINS" Or InStr(lTemp, "Watershed Shapefile") > 0 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
                If GisUtil.CurrentLayer = lLyr Then
                    lSelectedLayer = cboSubbasins.Items.Count - 1
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
            End If
        Next
        If lSelectedLayer > -1 Then
            cboSubbasins.SelectedIndex = lSelectedLayer
        End If
        'if all else fails set it to the first one
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            cboSubbasins.SelectedIndex = 0
        End If
        If cboBMPLayer.Items.Count > 0 And cboBMPLayer.SelectedIndex < 0 Then
            cboBMPLayer.SelectedIndex = 0
        End If

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
        SetBMPGridValues()

        cboLanduse.SelectedIndex = 0

        atxPrec.Value = 40.0
        atxRatio.Value = 0.9
    End Sub

    Private Sub frmPollutantLoading_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
        End If
    End Sub

    Private Sub rbExportCoefficientMethod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbExportCoefficientMethod.CheckedChanged
        If rbExportCoefficientMethod.Checked = True Then
            tabValues.Text = "Export Coefficients"
            lblRatio.Visible = False
            lblPrecip.Visible = False
            atxRatio.Visible = False
            atxPrec.Visible = False
            lblValueFile.Text = "Export Coefficient File"
            lblValueUnits.Text = "(lbs/ac/yr)"
        Else
            tabValues.Text = "Event Mean Concentrations"
            lblRatio.Visible = True
            lblPrecip.Visible = True
            atxRatio.Visible = True
            atxPrec.Visible = True
            lblValueFile.Text = "EMC File"
            lblValueUnits.Text = "(mg/L, counts/100mL for bacteria)"
        End If
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

    Private Sub SetGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable
        Dim lSorted As New atcCollection

        If atcGridValues.Source Is Nothing Then Exit Sub

        If rbExportCoefficientMethod.Checked = True Then
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                lblValueFileName.Text = "\BASINS\etc\ecgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                lblValueFileName.Text = "\BASINS\etc\ecgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
                lblValueFileName.Text = "\BASINS\etc\ecnlcd.dbf"
            Else 'grid
                lblValueFileName.Text = "\BASINS\etc\ecnlcd.dbf"
            End If
        Else
            'emc (simple) method
            If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                lblValueFileName.Text = "\BASINS\etc\emcgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
                lblValueFileName.Text = "\BASINS\etc\emcgiras.dbf"
            ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
                lblValueFileName.Text = "\BASINS\etc\emcnlcd.dbf"
            Else 'grid
                lblValueFileName.Text = "\BASINS\etc\emcnlcd.dbf"
            End If
        End If
        atcGridValues.Clear()

        If lblValueFileName.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblValueFileName.Text)

            With atcGridValues.Source
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
        atcGridValues.SizeAllColumnsToContents()
        atcGridValues.Refresh()
        SetPollutantList()
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
        If lstConstituents.Items.Count > 0 Then
            lstConstituents.SelectedItems.Add(lstConstituents.Items(0))
        End If
    End Sub

    Private Sub cboBMPLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBMPLayer.SelectedIndexChanged
        Logger.Dbg("BMPLayerChangedTo " & cboBMPLayer.Items(cboBMPLayer.SelectedIndex))

        If GisUtil.LayerType(GisUtil.LayerIndex(cboBMPLayer.Items(cboBMPLayer.SelectedIndex))) = 3 Then
            cboAreaField.Visible = False
            lblArea.Visible = False
        Else
            'area field just applies to point bmps
            cboAreaField.Visible = True
            lblArea.Visible = True
        End If
        PopulateBMPFields()
    End Sub

    Private Sub SetBMPGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable
        Dim lSorted As New atcCollection

        If atcGridBMP.Source Is Nothing Then Exit Sub

        lblBMPFile.Text = "\BASINS\etc\bmpeffic.dbf"
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

    Private Sub cmdChangeBMP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeBMP.Click
        ofdValues.Title = "Set BMP Removal Efficiency File"
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblBMPFile.Text = ofdValues.FileName
            SetBMPGridValues()
        End If
    End Sub
End Class