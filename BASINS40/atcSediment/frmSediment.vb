Imports System.Drawing
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcControls
Imports System.Windows.Forms
Imports HTMLBuilder

Friend Class frmSediment
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
    Friend WithEvents BMPDescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BMPType As System.Windows.Forms.DataGridViewTextBoxColumn

    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnFillTables As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents btnMethod As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnPreview As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnSaveAs As System.Windows.Forms.Button
    Friend WithEvents btnSelectR As System.Windows.Forms.Button
    Friend WithEvents C As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cboBMPIDField As System.Windows.Forms.ComboBox
    Friend WithEvents cboBMPLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboDEMLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboDEMUnits As System.Windows.Forms.ComboBox
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents cboLandUseIDField As System.Windows.Forms.ComboBox
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboRoadLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboSedimentDelivery As System.Windows.Forms.ComboBox
    Friend WithEvents cboSoilIDField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSoilLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents chkBMPs As System.Windows.Forms.CheckBox

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents dgBMP As System.Windows.Forms.DataGridView
    Friend WithEvents dgLandUse As System.Windows.Forms.DataGridView
    Friend WithEvents dgSoilType As System.Windows.Forms.DataGridView
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents K As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LandUseDescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LandUseID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblBMP As System.Windows.Forms.Label
    Friend WithEvents lblBMPLayer As System.Windows.Forms.Label
    Friend WithEvents lblBMPType As System.Windows.Forms.Label
    Friend WithEvents lblDEMLayer As System.Windows.Forms.Label
    Friend WithEvents lblGridSize As System.Windows.Forms.Label
    Friend WithEvents lblLandUse As System.Windows.Forms.Label
    Friend WithEvents lblLandUseIDField As System.Windows.Forms.Label
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents lblLanduseType As System.Windows.Forms.Label
    Friend WithEvents lblOutput As System.Windows.Forms.Label
    Friend WithEvents lblRoadsLayer As System.Windows.Forms.Label
    Friend WithEvents lblSoilIDField As System.Windows.Forms.Label
    Friend WithEvents lblSoilsLayer As System.Windows.Forms.Label
    Friend WithEvents lblSoilType As System.Windows.Forms.Label
    Friend WithEvents lblSubbasinsLayer As System.Windows.Forms.Label
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents P As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SoilDescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SoilID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tabBMPs As System.Windows.Forms.TabPage
    Friend WithEvents tabCoef As System.Windows.Forms.TabPage
    Friend WithEvents tabDEM As System.Windows.Forms.TabPage
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabLanduse As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tabResults As System.Windows.Forms.TabPage
    Friend WithEvents tabSEDIMENT As System.Windows.Forms.TabControl
    Friend WithEvents tabSoils As System.Windows.Forms.TabPage
    Friend WithEvents tabYield As System.Windows.Forms.TabPage
    Friend WithEvents txtGridSize As atcControls.atcText
    Friend WithEvents txtOutputName As System.Windows.Forms.TextBox
    Friend WithEvents txtRainfall As System.Windows.Forms.TextBox
    Friend WithEvents txtThreshold As System.Windows.Forms.TextBox
    Friend WithEvents lblGenerate As System.Windows.Forms.Label
    Friend WithEvents wbResults As System.Windows.Forms.WebBrowser

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSediment))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.tabSEDIMENT = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.txtOutputName = New System.Windows.Forms.TextBox
        Me.txtGridSize = New atcControls.atcText
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblGridSize = New System.Windows.Forms.Label
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.lblOutput = New System.Windows.Forms.Label
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.tabSoils = New System.Windows.Forms.TabPage
        Me.lblSoilIDField = New System.Windows.Forms.Label
        Me.cboSoilIDField = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboSoilLayer = New System.Windows.Forms.ComboBox
        Me.lblSoilsLayer = New System.Windows.Forms.Label
        Me.tabLanduse = New System.Windows.Forms.TabPage
        Me.Label4 = New System.Windows.Forms.Label
        Me.cboRoadLayer = New System.Windows.Forms.ComboBox
        Me.lblRoadsLayer = New System.Windows.Forms.Label
        Me.cboLandUseIDField = New System.Windows.Forms.ComboBox
        Me.lblLandUseIDField = New System.Windows.Forms.Label
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.lblLanduseType = New System.Windows.Forms.Label
        Me.tabDEM = New System.Windows.Forms.TabPage
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cboDEMUnits = New System.Windows.Forms.ComboBox
        Me.cboDEMLayer = New System.Windows.Forms.ComboBox
        Me.lblDEMLayer = New System.Windows.Forms.Label
        Me.tabCoef = New System.Windows.Forms.TabPage
        Me.btnSelectR = New System.Windows.Forms.Button
        Me.btnFillTables = New System.Windows.Forms.Button
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.dgSoilType = New System.Windows.Forms.DataGridView
        Me.SoilID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SoilDescription = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.K = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgLandUse = New System.Windows.Forms.DataGridView
        Me.LandUseID = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LandUseDescription = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.C = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.lblSoilType = New System.Windows.Forms.Label
        Me.lblLandUse = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtRainfall = New System.Windows.Forms.TextBox
        Me.tabBMPs = New System.Windows.Forms.TabPage
        Me.dgBMP = New System.Windows.Forms.DataGridView
        Me.BMPType = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BMPDescription = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.P = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblBMP = New System.Windows.Forms.Label
        Me.lblBMPType = New System.Windows.Forms.Label
        Me.lblBMPLayer = New System.Windows.Forms.Label
        Me.cboBMPIDField = New System.Windows.Forms.ComboBox
        Me.cboBMPLayer = New System.Windows.Forms.ComboBox
        Me.chkBMPs = New System.Windows.Forms.CheckBox
        Me.tabYield = New System.Windows.Forms.TabPage
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.txtThreshold = New System.Windows.Forms.TextBox
        Me.btnMethod = New System.Windows.Forms.Button
        Me.cboSedimentDelivery = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.tabResults = New System.Windows.Forms.TabPage
        Me.lblGenerate = New System.Windows.Forms.Label
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnPreview = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.wbResults = New System.Windows.Forms.WebBrowser
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnHelp = New System.Windows.Forms.Button
        Me.btnAbout = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.btnSaveAs = New System.Windows.Forms.Button
        Me.tabSEDIMENT.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabSoils.SuspendLayout()
        Me.tabLanduse.SuspendLayout()
        Me.tabDEM.SuspendLayout()
        Me.tabCoef.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgSoilType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBMPs.SuspendLayout()
        CType(Me.dgBMP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabYield.SuspendLayout()
        Me.tabResults.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabSEDIMENT
        '
        Me.tabSEDIMENT.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabSEDIMENT.Controls.Add(Me.tabGeneral)
        Me.tabSEDIMENT.Controls.Add(Me.tabSoils)
        Me.tabSEDIMENT.Controls.Add(Me.tabLanduse)
        Me.tabSEDIMENT.Controls.Add(Me.tabDEM)
        Me.tabSEDIMENT.Controls.Add(Me.tabCoef)
        Me.tabSEDIMENT.Controls.Add(Me.tabBMPs)
        Me.tabSEDIMENT.Controls.Add(Me.tabYield)
        Me.tabSEDIMENT.Controls.Add(Me.tabResults)
        Me.tabSEDIMENT.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabSEDIMENT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabSEDIMENT.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabSEDIMENT.Location = New System.Drawing.Point(14, 14)
        Me.tabSEDIMENT.Name = "tabSEDIMENT"
        Me.tabSEDIMENT.SelectedIndex = 0
        Me.tabSEDIMENT.Size = New System.Drawing.Size(739, 312)
        Me.tabSEDIMENT.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.Color.Transparent
        Me.tabGeneral.Controls.Add(Me.txtOutputName)
        Me.tabGeneral.Controls.Add(Me.txtGridSize)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.lblGridSize)
        Me.tabGeneral.Controls.Add(Me.cboSubbasins)
        Me.tabGeneral.Controls.Add(Me.lblOutput)
        Me.tabGeneral.Controls.Add(Me.lblSubbasinsLayer)
        Me.tabGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(731, 283)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'txtOutputName
        '
        Me.txtOutputName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.txtOutputName, "Enter name of folder where output reports will be stored. This folder will be pla" & _
                "ced under the BASINS data directory for this project (e.g., C:\Basins\Data\03060" & _
                "201\Sediment\Report 1)")
        Me.txtOutputName.Location = New System.Drawing.Point(167, 142)
        Me.txtOutputName.Name = "txtOutputName"
        Me.HelpProvider1.SetShowHelp(Me.txtOutputName, True)
        Me.txtOutputName.Size = New System.Drawing.Size(543, 20)
        Me.txtOutputName.TabIndex = 2
        '
        'txtGridSize
        '
        Me.txtGridSize.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtGridSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGridSize.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtGridSize.DefaultValue = "30"
        Me.txtGridSize.HardMax = 1000
        Me.txtGridSize.HardMin = 0
        Me.HelpProvider1.SetHelpString(Me.txtGridSize, "Enter the desired grid side for all grid layers that will be created.")
        Me.txtGridSize.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtGridSize.Location = New System.Drawing.Point(167, 168)
        Me.txtGridSize.MaxWidth = 20
        Me.txtGridSize.Name = "txtGridSize"
        Me.txtGridSize.NumericFormat = "0.#####"
        Me.txtGridSize.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtGridSize.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtGridSize.SelLength = 0
        Me.txtGridSize.SelStart = 2
        Me.HelpProvider1.SetShowHelp(Me.txtGridSize, True)
        Me.txtGridSize.Size = New System.Drawing.Size(543, 21)
        Me.txtGridSize.SoftMax = 300
        Me.txtGridSize.SoftMin = 5
        Me.txtGridSize.TabIndex = 4
        Me.txtGridSize.ValueDouble = 30
        Me.txtGridSize.ValueInteger = 30
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.Info
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(725, 133)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblGridSize
        '
        Me.lblGridSize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblGridSize.AutoSize = True
        Me.lblGridSize.Location = New System.Drawing.Point(21, 173)
        Me.lblGridSize.Name = "lblGridSize"
        Me.lblGridSize.Size = New System.Drawing.Size(89, 13)
        Me.lblGridSize.TabIndex = 3
        Me.lblGridSize.Text = "Grid &Cell Size (m):"
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSubbasins, "Select the polygon shapefile layer containing one or more subbasins for which you" & _
                " want erosion and sediment computed.")
        Me.cboSubbasins.Location = New System.Drawing.Point(167, 240)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.HelpProvider1.SetShowHelp(Me.cboSubbasins, True)
        Me.cboSubbasins.Size = New System.Drawing.Size(543, 21)
        Me.cboSubbasins.Sorted = True
        Me.cboSubbasins.TabIndex = 8
        '
        'lblOutput
        '
        Me.lblOutput.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblOutput.AutoSize = True
        Me.lblOutput.Location = New System.Drawing.Point(21, 146)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(105, 13)
        Me.lblOutput.TabIndex = 1
        Me.lblOutput.Text = "Output Folder &Name:"
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSubbasinsLayer.AutoSize = True
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(21, 243)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(88, 13)
        Me.lblSubbasinsLayer.TabIndex = 7
        Me.lblSubbasinsLayer.Text = "Subbasins &Layer:"
        '
        'tabSoils
        '
        Me.tabSoils.Controls.Add(Me.lblSoilIDField)
        Me.tabSoils.Controls.Add(Me.cboSoilIDField)
        Me.tabSoils.Controls.Add(Me.Label3)
        Me.tabSoils.Controls.Add(Me.cboSoilLayer)
        Me.tabSoils.Controls.Add(Me.lblSoilsLayer)
        Me.tabSoils.Location = New System.Drawing.Point(4, 25)
        Me.tabSoils.Name = "tabSoils"
        Me.tabSoils.Size = New System.Drawing.Size(731, 283)
        Me.tabSoils.TabIndex = 1
        Me.tabSoils.Text = "Soil Type"
        Me.tabSoils.UseVisualStyleBackColor = True
        '
        'lblSoilIDField
        '
        Me.lblSoilIDField.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSoilIDField.AutoSize = True
        Me.lblSoilIDField.Location = New System.Drawing.Point(22, 198)
        Me.lblSoilIDField.Name = "lblSoilIDField"
        Me.lblSoilIDField.Size = New System.Drawing.Size(93, 13)
        Me.lblSoilIDField.TabIndex = 3
        Me.lblSoilIDField.Text = "Soil Type ID &Field:"
        '
        'cboSoilIDField
        '
        Me.cboSoilIDField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSoilIDField.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboSoilIDField, "Select the field containing the soil type identifier (this is used in the Erosion" & _
                " Coefficients lookup to determine the K factor).")
        Me.cboSoilIDField.Location = New System.Drawing.Point(168, 194)
        Me.cboSoilIDField.Name = "cboSoilIDField"
        Me.HelpProvider1.SetShowHelp(Me.cboSoilIDField, True)
        Me.cboSoilIDField.Size = New System.Drawing.Size(544, 21)
        Me.cboSoilIDField.Sorted = True
        Me.cboSoilIDField.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.BackColor = System.Drawing.SystemColors.Info
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(725, 115)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = resources.GetString("Label3.Text")
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboSoilLayer
        '
        Me.cboSoilLayer.AllowDrop = True
        Me.cboSoilLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSoilLayer, "Select the polygon shapefile layer defining the soil type distribution.")
        Me.cboSoilLayer.Location = New System.Drawing.Point(168, 168)
        Me.cboSoilLayer.Name = "cboSoilLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboSoilLayer, True)
        Me.cboSoilLayer.Size = New System.Drawing.Size(544, 21)
        Me.cboSoilLayer.Sorted = True
        Me.cboSoilLayer.TabIndex = 2
        '
        'lblSoilsLayer
        '
        Me.lblSoilsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSoilsLayer.AutoSize = True
        Me.lblSoilsLayer.Location = New System.Drawing.Point(22, 171)
        Me.lblSoilsLayer.Name = "lblSoilsLayer"
        Me.lblSoilsLayer.Size = New System.Drawing.Size(83, 13)
        Me.lblSoilsLayer.TabIndex = 1
        Me.lblSoilsLayer.Text = "Soil Type &Layer:"
        '
        'tabLanduse
        '
        Me.tabLanduse.Controls.Add(Me.Label4)
        Me.tabLanduse.Controls.Add(Me.cboRoadLayer)
        Me.tabLanduse.Controls.Add(Me.lblRoadsLayer)
        Me.tabLanduse.Controls.Add(Me.cboLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.lblLandUseIDField)
        Me.tabLanduse.Controls.Add(Me.cboLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.cboLanduse)
        Me.tabLanduse.Controls.Add(Me.lblLanduseType)
        Me.tabLanduse.Location = New System.Drawing.Point(4, 25)
        Me.tabLanduse.Name = "tabLanduse"
        Me.tabLanduse.Size = New System.Drawing.Size(731, 283)
        Me.tabLanduse.TabIndex = 2
        Me.tabLanduse.Text = "Land Use"
        Me.tabLanduse.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.SystemColors.Info
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(725, 115)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = resources.GetString("Label4.Text")
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboRoadLayer
        '
        Me.cboRoadLayer.AllowDrop = True
        Me.cboRoadLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRoadLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRoadLayer.Location = New System.Drawing.Point(165, 233)
        Me.cboRoadLayer.Name = "cboRoadLayer"
        Me.cboRoadLayer.Size = New System.Drawing.Size(543, 21)
        Me.cboRoadLayer.Sorted = True
        Me.cboRoadLayer.TabIndex = 8
        Me.cboRoadLayer.Visible = False
        '
        'lblRoadsLayer
        '
        Me.lblRoadsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblRoadsLayer.AutoSize = True
        Me.lblRoadsLayer.Location = New System.Drawing.Point(19, 236)
        Me.lblRoadsLayer.Name = "lblRoadsLayer"
        Me.lblRoadsLayer.Size = New System.Drawing.Size(65, 13)
        Me.lblRoadsLayer.TabIndex = 7
        Me.lblRoadsLayer.Text = "&Road Layer:"
        Me.lblRoadsLayer.Visible = False
        '
        'cboLandUseIDField
        '
        Me.cboLandUseIDField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseIDField.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboLandUseIDField, "Select the field containing the land use identifier (this is used in the Erosion " & _
                "Coefficients lookup to determine the C factor).")
        Me.cboLandUseIDField.Location = New System.Drawing.Point(186, 188)
        Me.cboLandUseIDField.Name = "cboLandUseIDField"
        Me.HelpProvider1.SetShowHelp(Me.cboLandUseIDField, True)
        Me.cboLandUseIDField.Size = New System.Drawing.Size(499, 21)
        Me.cboLandUseIDField.Sorted = True
        Me.cboLandUseIDField.TabIndex = 6
        '
        'lblLandUseIDField
        '
        Me.lblLandUseIDField.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLandUseIDField.AutoSize = True
        Me.lblLandUseIDField.Location = New System.Drawing.Point(71, 191)
        Me.lblLandUseIDField.Name = "lblLandUseIDField"
        Me.lblLandUseIDField.Size = New System.Drawing.Size(95, 13)
        Me.lblLandUseIDField.TabIndex = 5
        Me.lblLandUseIDField.Text = "Land Use ID &Field:"
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboLandUseLayer, "Select the shapefile or grid layer containing the land use distribution.")
        Me.cboLandUseLayer.Location = New System.Drawing.Point(186, 161)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboLandUseLayer, True)
        Me.cboLandUseLayer.Size = New System.Drawing.Size(499, 21)
        Me.cboLandUseLayer.Sorted = True
        Me.cboLandUseLayer.TabIndex = 4
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLandUseLayer.AutoSize = True
        Me.lblLandUseLayer.Location = New System.Drawing.Point(71, 164)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(85, 13)
        Me.lblLandUseLayer.TabIndex = 3
        Me.lblLandUseLayer.Text = "Land Use &Layer:"
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboLanduse, "Select the type of land use layers you are using for this project.")
        Me.cboLanduse.Location = New System.Drawing.Point(165, 133)
        Me.cboLanduse.Name = "cboLanduse"
        Me.HelpProvider1.SetShowHelp(Me.cboLanduse, True)
        Me.cboLanduse.Size = New System.Drawing.Size(543, 21)
        Me.cboLanduse.TabIndex = 2
        '
        'lblLanduseType
        '
        Me.lblLanduseType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLanduseType.AutoSize = True
        Me.lblLanduseType.Location = New System.Drawing.Point(19, 137)
        Me.lblLanduseType.Name = "lblLanduseType"
        Me.lblLanduseType.Size = New System.Drawing.Size(112, 13)
        Me.lblLanduseType.TabIndex = 1
        Me.lblLanduseType.Text = "Land Use Layer &Type:"
        '
        'tabDEM
        '
        Me.tabDEM.Controls.Add(Me.Label9)
        Me.tabDEM.Controls.Add(Me.Label2)
        Me.tabDEM.Controls.Add(Me.cboDEMUnits)
        Me.tabDEM.Controls.Add(Me.cboDEMLayer)
        Me.tabDEM.Controls.Add(Me.lblDEMLayer)
        Me.tabDEM.Location = New System.Drawing.Point(4, 25)
        Me.tabDEM.Name = "tabDEM"
        Me.tabDEM.Size = New System.Drawing.Size(731, 283)
        Me.tabDEM.TabIndex = 3
        Me.tabDEM.Text = "DEM"
        Me.tabDEM.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(19, 198)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(130, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "DEM Grid Elevation Units:"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.SystemColors.Info
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(725, 115)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = resources.GetString("Label2.Text")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboDEMUnits
        '
        Me.cboDEMUnits.AllowDrop = True
        Me.cboDEMUnits.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboDEMUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboDEMUnits, "Select the units associated with the values stored in the DEM grid.")
        Me.cboDEMUnits.Items.AddRange(New Object() {"Meters", "Centimeters", "Feet"})
        Me.cboDEMUnits.Location = New System.Drawing.Point(165, 195)
        Me.cboDEMUnits.Name = "cboDEMUnits"
        Me.HelpProvider1.SetShowHelp(Me.cboDEMUnits, True)
        Me.cboDEMUnits.Size = New System.Drawing.Size(120, 21)
        Me.cboDEMUnits.TabIndex = 4
        '
        'cboDEMLayer
        '
        Me.cboDEMLayer.AllowDrop = True
        Me.cboDEMLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDEMLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboDEMLayer, "Select the digital elevation model (DEM) grid layer containing the elevations in " & _
                "your area of interest.")
        Me.cboDEMLayer.Location = New System.Drawing.Point(165, 168)
        Me.cboDEMLayer.Name = "cboDEMLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboDEMLayer, True)
        Me.cboDEMLayer.Size = New System.Drawing.Size(543, 21)
        Me.cboDEMLayer.Sorted = True
        Me.cboDEMLayer.TabIndex = 2
        '
        'lblDEMLayer
        '
        Me.lblDEMLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblDEMLayer.AutoSize = True
        Me.lblDEMLayer.Location = New System.Drawing.Point(19, 171)
        Me.lblDEMLayer.Name = "lblDEMLayer"
        Me.lblDEMLayer.Size = New System.Drawing.Size(63, 13)
        Me.lblDEMLayer.TabIndex = 1
        Me.lblDEMLayer.Text = "DEM &Layer:"
        '
        'tabCoef
        '
        Me.tabCoef.Controls.Add(Me.btnSelectR)
        Me.tabCoef.Controls.Add(Me.btnFillTables)
        Me.tabCoef.Controls.Add(Me.TableLayoutPanel1)
        Me.tabCoef.Controls.Add(Me.Label8)
        Me.tabCoef.Controls.Add(Me.Label7)
        Me.tabCoef.Controls.Add(Me.txtRainfall)
        Me.tabCoef.Location = New System.Drawing.Point(4, 25)
        Me.tabCoef.Name = "tabCoef"
        Me.tabCoef.Size = New System.Drawing.Size(731, 283)
        Me.tabCoef.TabIndex = 4
        Me.tabCoef.Text = "Erosion Coefficients"
        Me.tabCoef.UseVisualStyleBackColor = True
        '
        'btnSelectR
        '
        Me.HelpProvider1.SetHelpString(Me.btnSelectR, resources.GetString("btnSelectR.HelpString"))
        Me.btnSelectR.Location = New System.Drawing.Point(239, 125)
        Me.btnSelectR.Name = "btnSelectR"
        Me.HelpProvider1.SetShowHelp(Me.btnSelectR, True)
        Me.btnSelectR.Size = New System.Drawing.Size(75, 23)
        Me.btnSelectR.TabIndex = 3
        Me.btnSelectR.Text = "Select..."
        Me.btnSelectR.UseVisualStyleBackColor = True
        '
        'btnFillTables
        '
        Me.btnFillTables.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnFillTables, "This will add any additional soil type and land use IDs found in your study area " & _
                "to the tables below; you will need to manually type the description and K/C fact" & _
                "ors for these items.")
        Me.btnFillTables.Location = New System.Drawing.Point(570, 125)
        Me.btnFillTables.Name = "btnFillTables"
        Me.HelpProvider1.SetShowHelp(Me.btnFillTables, True)
        Me.btnFillTables.Size = New System.Drawing.Size(155, 23)
        Me.btnFillTables.TabIndex = 4
        Me.btnFillTables.Text = "&Fill Tables with Unique IDs"
        Me.btnFillTables.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.dgSoilType, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.dgLandUse, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblSoilType, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblLandUse, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 159)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(725, 121)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'dgSoilType
        '
        Me.dgSoilType.AllowUserToResizeColumns = False
        Me.dgSoilType.AllowUserToResizeRows = False
        Me.dgSoilType.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSoilType.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgSoilType.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dgSoilType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgSoilType.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SoilID, Me.SoilDescription, Me.K})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgSoilType.DefaultCellStyle = DataGridViewCellStyle8
        Me.HelpProvider1.SetHelpString(Me.dgSoilType, "This contains the standard soil IDs and K factors.")
        Me.dgSoilType.Location = New System.Drawing.Point(3, 16)
        Me.dgSoilType.Name = "dgSoilType"
        Me.dgSoilType.RowTemplate.Height = 24
        Me.dgSoilType.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.HelpProvider1.SetShowHelp(Me.dgSoilType, True)
        Me.dgSoilType.Size = New System.Drawing.Size(356, 102)
        Me.dgSoilType.TabIndex = 2
        '
        'SoilID
        '
        Me.SoilID.HeaderText = "Soil ID"
        Me.SoilID.Name = "SoilID"
        Me.SoilID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'SoilDescription
        '
        Me.SoilDescription.HeaderText = "Description"
        Me.SoilDescription.Name = "SoilDescription"
        Me.SoilDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'K
        '
        Me.K.HeaderText = "Erodibility (K)"
        Me.K.Name = "K"
        Me.K.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'dgLandUse
        '
        Me.dgLandUse.AllowUserToResizeColumns = False
        Me.dgLandUse.AllowUserToResizeRows = False
        Me.dgLandUse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgLandUse.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgLandUse.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.dgLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgLandUse.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LandUseID, Me.LandUseDescription, Me.C})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgLandUse.DefaultCellStyle = DataGridViewCellStyle10
        Me.HelpProvider1.SetHelpString(Me.dgLandUse, "This contains the land use IDs and C factors for the selected land use type.")
        Me.dgLandUse.Location = New System.Drawing.Point(365, 16)
        Me.dgLandUse.Name = "dgLandUse"
        Me.dgLandUse.RowTemplate.Height = 24
        Me.dgLandUse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.HelpProvider1.SetShowHelp(Me.dgLandUse, True)
        Me.dgLandUse.Size = New System.Drawing.Size(357, 102)
        Me.dgLandUse.TabIndex = 3
        '
        'LandUseID
        '
        Me.LandUseID.HeaderText = "Land Use ID"
        Me.LandUseID.Name = "LandUseID"
        Me.LandUseID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'LandUseDescription
        '
        Me.LandUseDescription.HeaderText = "Description"
        Me.LandUseDescription.Name = "LandUseDescription"
        Me.LandUseDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'C
        '
        Me.C.HeaderText = "Cropping (C)"
        Me.C.Name = "C"
        Me.C.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'lblSoilType
        '
        Me.lblSoilType.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblSoilType.AutoSize = True
        Me.lblSoilType.Location = New System.Drawing.Point(110, 0)
        Me.lblSoilType.Name = "lblSoilType"
        Me.lblSoilType.Size = New System.Drawing.Size(141, 13)
        Me.lblSoilType.TabIndex = 0
        Me.lblSoilType.Text = "Soil Type/&Erodibility Factors:"
        '
        'lblLandUse
        '
        Me.lblLandUse.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblLandUse.AutoSize = True
        Me.lblLandUse.Location = New System.Drawing.Point(473, 0)
        Me.lblLandUse.Name = "lblLandUse"
        Me.lblLandUse.Size = New System.Drawing.Size(141, 13)
        Me.lblLandUse.TabIndex = 1
        Me.lblLandUse.Text = "&Land Use/Cropping Factors:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 130)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "&Rainfall Erosivity Factor:"
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.BackColor = System.Drawing.SystemColors.Info
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Location = New System.Drawing.Point(3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(725, 115)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = resources.GetString("Label7.Text")
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRainfall
        '
        Me.HelpProvider1.SetHelpString(Me.txtRainfall, "Enter the rainfall erosivity factor for your study area.")
        Me.txtRainfall.Location = New System.Drawing.Point(133, 127)
        Me.txtRainfall.Name = "txtRainfall"
        Me.HelpProvider1.SetShowHelp(Me.txtRainfall, True)
        Me.txtRainfall.Size = New System.Drawing.Size(100, 20)
        Me.txtRainfall.TabIndex = 2
        '
        'tabBMPs
        '
        Me.tabBMPs.Controls.Add(Me.dgBMP)
        Me.tabBMPs.Controls.Add(Me.Label11)
        Me.tabBMPs.Controls.Add(Me.lblBMP)
        Me.tabBMPs.Controls.Add(Me.lblBMPType)
        Me.tabBMPs.Controls.Add(Me.lblBMPLayer)
        Me.tabBMPs.Controls.Add(Me.cboBMPIDField)
        Me.tabBMPs.Controls.Add(Me.cboBMPLayer)
        Me.tabBMPs.Controls.Add(Me.chkBMPs)
        Me.tabBMPs.Location = New System.Drawing.Point(4, 25)
        Me.tabBMPs.Name = "tabBMPs"
        Me.tabBMPs.Size = New System.Drawing.Size(731, 283)
        Me.tabBMPs.TabIndex = 5
        Me.tabBMPs.Text = "BMPs"
        Me.tabBMPs.UseVisualStyleBackColor = True
        '
        'dgBMP
        '
        Me.dgBMP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgBMP.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgBMP.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle11
        Me.dgBMP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgBMP.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BMPType, Me.BMPDescription, Me.P})
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgBMP.DefaultCellStyle = DataGridViewCellStyle12
        Me.HelpProvider1.SetHelpString(Me.dgBMP, "This is the BMP type to P factor lookup table.")
        Me.dgBMP.Location = New System.Drawing.Point(225, 182)
        Me.dgBMP.Name = "dgBMP"
        Me.dgBMP.RowTemplate.Height = 24
        Me.HelpProvider1.SetShowHelp(Me.dgBMP, True)
        Me.dgBMP.Size = New System.Drawing.Size(492, 98)
        Me.dgBMP.TabIndex = 7
        '
        'BMPType
        '
        Me.BMPType.HeaderText = "BMP Type"
        Me.BMPType.Name = "BMPType"
        Me.BMPType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'BMPDescription
        '
        Me.BMPDescription.HeaderText = "Description"
        Me.BMPDescription.Name = "BMPDescription"
        Me.BMPDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'P
        '
        Me.P.HeaderText = "Practice (P)"
        Me.P.Name = "P"
        Me.P.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Label11
        '
        Me.Label11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.BackColor = System.Drawing.SystemColors.Info
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Location = New System.Drawing.Point(3, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(725, 115)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = resources.GetString("Label11.Text")
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBMP
        '
        Me.lblBMP.AutoSize = True
        Me.lblBMP.Location = New System.Drawing.Point(95, 182)
        Me.lblBMP.Name = "lblBMP"
        Me.lblBMP.Size = New System.Drawing.Size(124, 26)
        Me.lblBMP.TabIndex = 6
        Me.lblBMP.Text = "BMP Type/Conservation" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "&Practice Factors:"
        '
        'lblBMPType
        '
        Me.lblBMPType.AutoSize = True
        Me.lblBMPType.Location = New System.Drawing.Point(95, 158)
        Me.lblBMPType.Name = "lblBMPType"
        Me.lblBMPType.Size = New System.Drawing.Size(85, 13)
        Me.lblBMPType.TabIndex = 4
        Me.lblBMPType.Text = "BMP Type &Field:"
        '
        'lblBMPLayer
        '
        Me.lblBMPLayer.AutoSize = True
        Me.lblBMPLayer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblBMPLayer.Location = New System.Drawing.Point(95, 130)
        Me.lblBMPLayer.Name = "lblBMPLayer"
        Me.lblBMPLayer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBMPLayer.Size = New System.Drawing.Size(62, 13)
        Me.lblBMPLayer.TabIndex = 2
        Me.lblBMPLayer.Text = "BMP &Layer:"
        '
        'cboBMPIDField
        '
        Me.cboBMPIDField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPIDField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPIDField.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboBMPIDField, "Select the field describing the BMP type that will be used to look up the P facto" & _
                "rs.")
        Me.cboBMPIDField.Location = New System.Drawing.Point(191, 155)
        Me.cboBMPIDField.Name = "cboBMPIDField"
        Me.HelpProvider1.SetShowHelp(Me.cboBMPIDField, True)
        Me.cboBMPIDField.Size = New System.Drawing.Size(526, 21)
        Me.cboBMPIDField.Sorted = True
        Me.cboBMPIDField.TabIndex = 5
        '
        'cboBMPLayer
        '
        Me.cboBMPLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBMPLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBMPLayer.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboBMPLayer, "Select the polygon shapefile containing the BMPs being used in the study area.")
        Me.cboBMPLayer.Location = New System.Drawing.Point(191, 127)
        Me.cboBMPLayer.Name = "cboBMPLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboBMPLayer, True)
        Me.cboBMPLayer.Size = New System.Drawing.Size(526, 21)
        Me.cboBMPLayer.Sorted = True
        Me.cboBMPLayer.TabIndex = 3
        '
        'chkBMPs
        '
        Me.chkBMPs.AutoSize = True
        Me.chkBMPs.Checked = True
        Me.chkBMPs.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.HelpProvider1.SetHelpString(Me.chkBMPs, "If selected, P factors are determined based on a BMP layer and lookup table; othe" & _
                "rwise P factors will all be set to 1.0.")
        Me.chkBMPs.Location = New System.Drawing.Point(8, 129)
        Me.chkBMPs.Name = "chkBMPs"
        Me.HelpProvider1.SetShowHelp(Me.chkBMPs, True)
        Me.chkBMPs.Size = New System.Drawing.Size(76, 17)
        Me.chkBMPs.TabIndex = 1
        Me.chkBMPs.Text = "&Use BMPs"
        Me.chkBMPs.UseVisualStyleBackColor = True
        '
        'tabYield
        '
        Me.tabYield.Controls.Add(Me.lblThreshold)
        Me.tabYield.Controls.Add(Me.txtThreshold)
        Me.tabYield.Controls.Add(Me.btnMethod)
        Me.tabYield.Controls.Add(Me.cboSedimentDelivery)
        Me.tabYield.Controls.Add(Me.Label6)
        Me.tabYield.Controls.Add(Me.Label5)
        Me.tabYield.Location = New System.Drawing.Point(4, 25)
        Me.tabYield.Name = "tabYield"
        Me.tabYield.Padding = New System.Windows.Forms.Padding(3)
        Me.tabYield.Size = New System.Drawing.Size(731, 283)
        Me.tabYield.TabIndex = 6
        Me.tabYield.Text = "Sediment Yield"
        Me.tabYield.UseVisualStyleBackColor = True
        '
        'lblThreshold
        '
        Me.lblThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.Location = New System.Drawing.Point(23, 159)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(119, 13)
        Me.lblThreshold.TabIndex = 4
        Me.lblThreshold.Text = "Area &Threshold (sq km):"
        '
        'txtThreshold
        '
        Me.txtThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.HelpProvider1.SetHelpString(Me.txtThreshold, resources.GetString("txtThreshold.HelpString"))
        Me.txtThreshold.Location = New System.Drawing.Point(169, 157)
        Me.txtThreshold.Name = "txtThreshold"
        Me.HelpProvider1.SetShowHelp(Me.txtThreshold, True)
        Me.txtThreshold.Size = New System.Drawing.Size(100, 20)
        Me.txtThreshold.TabIndex = 5
        '
        'btnMethod
        '
        Me.btnMethod.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.HelpProvider1.SetHelpString(Me.btnMethod, "Click this to see a explanatory dialog box.")
        Me.btnMethod.Location = New System.Drawing.Point(580, 128)
        Me.btnMethod.Name = "btnMethod"
        Me.HelpProvider1.SetShowHelp(Me.btnMethod, True)
        Me.btnMethod.Size = New System.Drawing.Size(133, 23)
        Me.btnMethod.TabIndex = 3
        Me.btnMethod.Text = "Method &Descriptions..."
        Me.btnMethod.UseVisualStyleBackColor = True
        '
        'cboSedimentDelivery
        '
        Me.cboSedimentDelivery.AllowDrop = True
        Me.cboSedimentDelivery.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSedimentDelivery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSedimentDelivery, "Select the method to be used to compute the sediment delivery ratio.")
        Me.cboSedimentDelivery.Location = New System.Drawing.Point(169, 129)
        Me.cboSedimentDelivery.Name = "cboSedimentDelivery"
        Me.HelpProvider1.SetShowHelp(Me.cboSedimentDelivery, True)
        Me.cboSedimentDelivery.Size = New System.Drawing.Size(401, 21)
        Me.cboSedimentDelivery.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 133)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(134, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Sediment Delivery &Method:"
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.BackColor = System.Drawing.SystemColors.Info
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Location = New System.Drawing.Point(3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(725, 115)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = resources.GetString("Label5.Text")
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabResults
        '
        Me.tabResults.Controls.Add(Me.lblGenerate)
        Me.tabResults.Controls.Add(Me.btnCopy)
        Me.tabResults.Controls.Add(Me.btnPreview)
        Me.tabResults.Controls.Add(Me.btnPrint)
        Me.tabResults.Controls.Add(Me.wbResults)
        Me.tabResults.Location = New System.Drawing.Point(4, 25)
        Me.tabResults.Name = "tabResults"
        Me.tabResults.Size = New System.Drawing.Size(731, 283)
        Me.tabResults.TabIndex = 7
        Me.tabResults.Text = "Results"
        Me.tabResults.UseVisualStyleBackColor = True
        '
        'lblGenerate
        '
        Me.lblGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblGenerate.AutoSize = True
        Me.lblGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGenerate.Location = New System.Drawing.Point(85, 261)
        Me.lblGenerate.Name = "lblGenerate"
        Me.lblGenerate.Size = New System.Drawing.Size(190, 13)
        Me.lblGenerate.TabIndex = 4
        Me.lblGenerate.Text = "Click Generate button to refresh results"
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnCopy, "Copy the entire HTML report to the clipboard.")
        Me.btnCopy.Location = New System.Drawing.Point(4, 256)
        Me.btnCopy.Name = "btnCopy"
        Me.HelpProvider1.SetShowHelp(Me.btnCopy, True)
        Me.btnCopy.Size = New System.Drawing.Size(75, 24)
        Me.btnCopy.TabIndex = 1
        Me.btnCopy.Text = "&Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnPreview, "Display the print preview form.")
        Me.btnPreview.Location = New System.Drawing.Point(572, 256)
        Me.btnPreview.Name = "btnPreview"
        Me.HelpProvider1.SetShowHelp(Me.btnPreview, True)
        Me.btnPreview.Size = New System.Drawing.Size(75, 24)
        Me.btnPreview.TabIndex = 2
        Me.btnPreview.Text = "Pre&view..."
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnPrint, "Send the HTML report to the printer.")
        Me.btnPrint.Location = New System.Drawing.Point(653, 256)
        Me.btnPrint.Name = "btnPrint"
        Me.HelpProvider1.SetShowHelp(Me.btnPrint, True)
        Me.btnPrint.Size = New System.Drawing.Size(75, 24)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "&Print..."
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'wbResults
        '
        Me.wbResults.AllowWebBrowserDrop = False
        Me.wbResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.wbResults, "When the sediment computations are done, this browser will contain the summary re" & _
                "port.")
        Me.wbResults.IsWebBrowserContextMenuEnabled = False
        Me.wbResults.Location = New System.Drawing.Point(4, 3)
        Me.wbResults.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbResults.Name = "wbResults"
        Me.HelpProvider1.SetShowHelp(Me.wbResults, True)
        Me.wbResults.Size = New System.Drawing.Size(724, 248)
        Me.wbResults.TabIndex = 0
        Me.wbResults.WebBrowserShortcutsEnabled = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnOK, "Compute the sediment generated and delivered to the subbasin outlets, and generat" & _
                "e a summary report.")
        Me.btnOK.Location = New System.Drawing.Point(588, 343)
        Me.btnOK.Name = "btnOK"
        Me.HelpProvider1.SetShowHelp(Me.btnOK, True)
        Me.btnOK.Size = New System.Drawing.Size(85, 26)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "&Generate"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(678, 343)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 26)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Close"
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnHelp, "Display the help manual.")
        Me.btnHelp.Location = New System.Drawing.Point(305, 343)
        Me.btnHelp.Name = "btnHelp"
        Me.HelpProvider1.SetShowHelp(Me.btnHelp, True)
        Me.btnHelp.Size = New System.Drawing.Size(75, 26)
        Me.btnHelp.TabIndex = 3
        Me.btnHelp.Text = "&Help"
        '
        'btnAbout
        '
        Me.btnAbout.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnAbout, "Describe information about this application.")
        Me.btnAbout.Location = New System.Drawing.Point(385, 343)
        Me.btnAbout.Name = "btnAbout"
        Me.HelpProvider1.SetShowHelp(Me.btnAbout, True)
        Me.btnAbout.Size = New System.Drawing.Size(75, 26)
        Me.btnAbout.TabIndex = 4
        Me.btnAbout.Text = "&About"
        '
        'btnOpen
        '
        Me.btnOpen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnOpen, "Open an existing sediment scenario file which contains all settings specified on " & _
                "these tabs. If you do not explicitly open a data file, the default file will aut" & _
                "omatically be opened and used.")
        Me.btnOpen.Location = New System.Drawing.Point(12, 343)
        Me.btnOpen.Name = "btnOpen"
        Me.HelpProvider1.SetShowHelp(Me.btnOpen, True)
        Me.btnOpen.Size = New System.Drawing.Size(75, 26)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "&Open..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSaveAs
        '
        Me.btnSaveAs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnSaveAs, "Save all input settings to a named scenario file.")
        Me.btnSaveAs.Location = New System.Drawing.Point(93, 343)
        Me.btnSaveAs.Name = "btnSaveAs"
        Me.HelpProvider1.SetShowHelp(Me.btnSaveAs, True)
        Me.btnSaveAs.Size = New System.Drawing.Size(75, 26)
        Me.btnSaveAs.TabIndex = 2
        Me.btnSaveAs.Text = "&Save As..."
        Me.btnSaveAs.UseVisualStyleBackColor = True
        '
        'frmSediment
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(765, 386)
        Me.Controls.Add(Me.btnSaveAs)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tabSEDIMENT)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(552, 420)
        Me.Name = "frmSediment"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "BASINS Sediment Estimator"
        Me.tabSEDIMENT.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabSoils.ResumeLayout(False)
        Me.tabSoils.PerformLayout()
        Me.tabLanduse.ResumeLayout(False)
        Me.tabLanduse.PerformLayout()
        Me.tabDEM.ResumeLayout(False)
        Me.tabDEM.PerformLayout()
        Me.tabCoef.ResumeLayout(False)
        Me.tabCoef.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.dgSoilType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBMPs.ResumeLayout(False)
        Me.tabBMPs.PerformLayout()
        CType(Me.dgBMP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabYield.ResumeLayout(False)
        Me.tabYield.PerformLayout()
        Me.tabResults.ResumeLayout(False)
        Me.tabResults.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Dim gInitializing As Boolean
    Dim gLanduseType As Integer
    Dim gMethod As Integer

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        Logger.Msg("BASINS USLE Clean Sediment Estimator" & vbCrLf & vbCrLf & _
                   "Version " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, _
                   "BASINS - SEDIMENT")
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Logger.Dbg("UserCanceled")
        Me.Close()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim Header As String = String.Format("Version:1.0{0}StartHTML:0000000105{0}EndHTML:{1,10:0}{0}StartFragment:0000000105{0}EndFragment:{1,10:0}{0}", vbCrLf, wbResults.DocumentText.Length + 105)
        Clipboard.SetDataObject(New DataObject(DataFormats.Html, Header & wbResults.DocumentText))
    End Sub

    Private Sub btnFillTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFillTables.Click
        If Logger.Message("This will update both the Soil Type and Land Use tables below and fill the first column with all unique ID found in the layers previously identified; new values found will be assigned a factor of 1.0. Are you sure you want to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, Windows.Forms.DialogResult.Cancel) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        SaveForm()
        ProgressForm = New frmProgress()

        Try
            ProgressForm.Show(Me)

            With Project
                For i As Integer = 1 To 2
                    Dim LayerName As String = Choose(i, .SoilLayer, .LandUseLayer)
                    Dim FieldName As String = Choose(i, .SoilField, .LandUseField)
                    Dim IsShapeFile As Boolean = .LanduseType = enumLandUseType.GIRAS Or .LanduseType = enumLandUseType.UserShapefile
                    If .LanduseType = enumLandUseType.UserShapefile And (LayerName = "" Or FieldName = "") Then
                        Logger.Message("Both the layer and field names must be specified for both the soil and landuse themes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                        Exit Sub
                    End If
                    Dim dict As Generic.Dictionary(Of String, clsLookup) = Choose(i, .dictSoil, .dictLandUse(cboLanduse.SelectedIndex))
                    If i = 1 Or IsShapeFile Then
                        Dim lyrList As Generic.List(Of Integer) = Nothing
                        Dim lyr, fld As Integer
                        If i = 2 And .LanduseType = enumLandUseType.GIRAS Then
                            lyrList = GIRASLayers(GisUtil.LayerIndex(.SubbasinLayer))
                            If lyrList Is Nothing Then
                                Logger.Message("The GIRAS land use index layer was not found, or is in the wrong format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                                Exit Sub
                            End If
                            If lyrList.Count > 0 Then
                                fld = GisUtil.FieldIndex(lyrList(0), "LUCODE")
                            Else
                                Exit Sub
                            End If
                        Else
                            lyr = GisUtil.LayerIndex(LayerName)
                            fld = GisUtil.FieldIndex(lyr, FieldName)
                            lyrList = New Generic.List(Of Integer)
                            lyrList.Add(lyr)
                        End If
                        For Each lyridx As Integer In lyrList
                            For ctr As Integer = 0 To GisUtil.NumFeatures(lyridx) - 1
                                Dim ID As String = GisUtil.FieldValue(lyridx, ctr, fld)
                                If Not dict.ContainsKey(ID) Then dict.Add(ID, New clsLookup("", 1.0))
                                ProgressForm.SetProgress("Examining " & GisUtil.LayerName(lyridx) & "...", ctr, GisUtil.NumFeatures(lyridx) - 1)
                            Next
                        Next
                    Else
                        Dim grid As New MapWinGIS.Grid
                        grid.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(cboLandUseLayer.Text)))
                        For c As Integer = 0 To grid.Header.NumberCols - 1
                            For r As Integer = 0 To grid.Header.NumberRows - 1
                                If Not dict.ContainsKey(grid.Value(c, r)) Then dict.Add(grid.Value(c, r), New clsLookup("", 1.0))
                            Next
                            ProgressForm.SetProgress("Examining " & LayerName & "...", c, grid.Header.NumberCols - 1)
                        Next
                        grid.Close()
                    End If
                Next i
            End With
        Catch ex As Exception
            Logger.Message("Error occurred:" & vbCr & vbCr & ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        Finally
            ProgressForm.Close()
            ProgressForm.Dispose()
            ProgressForm = Nothing
            LoadForm()
        End Try
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\SEDIMENT.html")
    End Sub

    Private Sub btnMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMethod.Click
        MessageBox.Show("(1) Distance-based method (Sun and McNulty 1988):\n\n\tMd = M * (1 - 0.97 * D / L)\n\tL = 5.1 + 1.79 * M\n\n\twhere:\tMd is the mass moved from each cell to the closest stream network (US tons/acre/yr);\n\t\tD is the least cost distance (feet) from a cell to the nearest stream network; and\n\t\tL is the maximum distance (feet) that sediment with mass M (US ton) may travel.\n\n(2) Distance- and relief-based method (Yagow et al. 1998):\n\n\tDR = exp (-0.4233 * L * Sf)\n\tSf = exp (-16.1 * (r / L + 0.057)) - 0.6\n\n\twhere:\tDR is the sediment delivery ratio;\n\t\tL is the distance to stream in meters and\n\t\tr is the relief to stream in meters.\n\n(3) Area-based method (converted from a curve from National Engineering Handbook by Soil Conservation Service 1983):\n\n\tDR = 0.417762 * A ^ (-0.134958) - 0.127097\n\tDR <=1.0\n\n\twhere:\tDR is the sediment delivery ratio and\n\t\tA is area in square miles.\n\n(4) WEPP regression equation (L.W. Swift, Jr., 2000):\n\n\tZ = 0.9004 - 0.1341 * X - 0.0465 * X^2 + 0.00749 * X^3 - 0.0399 * Y + 0.0144 * Y^2 + 0.00308 * y^3\n\n\twhere:\tZ is the percent of source sediment passing to next cell grid;\n\t\tX is cumulative distance downslope;\n\t\tY is percent slope in grid cell.\n\nNOTE: at this time, only the Area-based Method is implemented.".Replace("\n", vbCr).Replace("\t", vbTab), "Method Description", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'note: check equations with original papers; 2nd method may be wrong: in one TMDL writeup indicated -0.4233 s/b -1.033, also -0.6 was +0.6 in WCS script
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        tabSEDIMENT.SelectedIndex = tabSEDIMENT.TabCount - 1
        wbResults.DocumentText = ""

        EnableControls(False)

        'save current settings as defaults
        SaveForm()

        Dim dictSoil As New Generic.Dictionary(Of String, Double)
        ProgressForm = New frmProgress

        With ProgressForm
            .Show(Me)

            If GenerateLoads() Then
                .Status = "Refreshing Results report..."
                wbResults.Navigate(SummaryReport)
            End If

            .Close()
            .Dispose()
            ProgressForm = Nothing
        End With

        EnableControls(True)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        With New OpenFileDialog
            .AddExtension = True
            .CheckFileExists = True
            .CheckPathExists = True
            .DefaultExt = ".Sediment"
            .Filter = "Sediment files (*.sediment)|*.sediment"
            .FilterIndex = 0
            .InitialDirectory = Project.SedimentFolder
            If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Open Sediment File"
            Dim sedfiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Project.SedimentFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.sediment")
            If sedfiles.Count > 0 Then .FileName = sedfiles(0)
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                LoadData(.FileName)
                LoadForm()
                wbResults.DocumentText = ""
            End If
            .Dispose()
        End With
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        wbResults.ShowPrintPreviewDialog()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        wbResults.ShowPrintDialog()
    End Sub

    Private Sub btnSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAs.Click
        SaveForm()
        With New SaveFileDialog
            .AddExtension = True
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = ".Sediment"
            .Filter = "Sediment files (*.sediment)|*.sediment"
            .FilterIndex = 0
            .InitialDirectory = Project.SedimentFolder
            .FileName = Project.FileName
            If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Save Sediment File"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then Project.FileName = .FileName : SaveData()
            Text = "BASINS Sediment Estimator"
            If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
            .Dispose()
        End With
    End Sub

    Private Sub btnSelectR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectR.Click
        With New frmSelectR
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtRainfall.Text = .txtAvg.Text
            End If
            .Dispose()
        End With
    End Sub

    Private Sub cboBMPIDField_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBMPIDField.SelectionChangeCommitted, cboBMPLayer.SelectionChangeCommitted, cboDEMLayer.SelectionChangeCommitted, cboDEMUnits.SelectionChangeCommitted, cboLanduse.SelectionChangeCommitted, cboLandUseIDField.SelectionChangeCommitted, cboLandUseLayer.SelectionChangeCommitted, cboRoadLayer.SelectionChangeCommitted, cboSedimentDelivery.SelectionChangeCommitted, cboSoilIDField.SelectionChangeCommitted, cboSoilLayer.SelectionChangeCommitted, cboSubbasins.SelectionChangeCommitted, chkBMPs.Click
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub

    Private Sub cboBMPLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBMPLayer.SelectedIndexChanged
        Dim lyr As Integer = GisUtil.LayerIndex(cboBMPLayer.Text)
        With cboBMPIDField.Items
            .Clear()
            For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                .Add(GisUtil.FieldName(i, lyr))
            Next
        End With
    End Sub

    Private Sub cboDEMLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDEMLayer.SelectedIndexChanged
        If cboDEMLayer.Text.ToUpper.Contains("NHD") Then
            cboDEMUnits.SelectedIndex = enumDEMUnits.Centimeters
        Else
            cboDEMUnits.SelectedIndex = enumDEMUnits.Meters
        End If
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        cboLandUseLayer.Enabled = True
        lblLandUseLayer.Enabled = True
        cboLandUseIDField.Enabled = False
        lblLandUseIDField.Enabled = False
        cboLandUseLayer.Items.Clear()
        For lLyr As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(lLyr)
            If cboLanduse.Text.StartsWith("USGS GIRAS") Then
                If GisUtil.LayerFileName(lLyr).ToUpper.Contains("GIRAS") And lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                cboLandUseLayer.Enabled = False
                lblLandUseLayer.Enabled = False
            ElseIf cboLanduse.Text = "User Shapefile" Then
                If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
                cboLandUseIDField.Enabled = True
                lblLandUseIDField.Enabled = True
            ElseIf cboLanduse.Text.StartsWith("NLCD") Then
                If GisUtil.LayerFileName(lLyr).ToUpper.Contains("NLCD") And lt = MapWindow.Interfaces.eLayerType.Grid Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
            Else
                If lt = MapWindow.Interfaces.eLayerType.Grid Then cboLandUseLayer.Items.Add(GisUtil.LayerName(lLyr))
            End If
        Next
        If cboLandUseLayer.Items.Count > 0 Then cboLandUseLayer.SelectedIndex = 0
        Project.LanduseType = cboLanduse.SelectedIndex
        lblLandUse.Text = String.Format("&Land Use/Cropping Factors ({0}):", cboLanduse.Text)
        LoadForm()
    End Sub

    Private Sub cboLandUseLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLandUseLayer.SelectedIndexChanged
        If Not GisUtil.IsLayer(cboLandUseLayer.Text) Then Exit Sub
        Dim lyr As Integer = GisUtil.LayerIndex(cboLandUseLayer.Text)
        Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(lyr)
        With cboLandUseIDField.Items
            .Clear()
            If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                    .Add(GisUtil.FieldName(i, lyr))
                Next
            End If
        End With
    End Sub

    Private Sub cboSedimentDelivery_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSedimentDelivery.SelectedIndexChanged
        Dim tf As Boolean = cboSedimentDelivery.SelectedIndex <> 2
        lblThreshold.Visible = tf
        txtThreshold.Visible = tf
    End Sub

    Private Sub cboSedimentDelivery_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSedimentDelivery.SelectionChangeCommitted
        If cboSedimentDelivery.SelectedIndex <> 2 Then
            Logger.Msg("Only the area-based sediment delivery method is currently implemented.")
            cboSedimentDelivery.SelectedIndex = 2
        End If
    End Sub

    Private Sub cboSoilLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSoilLayer.SelectedIndexChanged
        If cboSoilLayer.SelectedIndex = -1 Then Exit Sub
        Dim lyr As Integer = GisUtil.LayerIndex(cboSoilLayer.Text)
        With cboSoilIDField.Items
            .Clear()
            For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                .Add(GisUtil.FieldName(i, lyr))
            Next
        End With
        lblSoilType.Text = String.Format("Soil Type/&Erodibility Factors ({0}):", cboSoilLayer.Text)
    End Sub

    Private Sub chkBMPs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBMPs.CheckedChanged
        lblBMPLayer.Enabled = chkBMPs.Checked
        cboBMPLayer.Enabled = chkBMPs.Checked
        lblBMPType.Enabled = chkBMPs.Checked
        cboBMPIDField.Enabled = chkBMPs.Checked
        lblBMP.Enabled = chkBMPs.Checked
        dgBMP.Enabled = chkBMPs.Checked
    End Sub

    Private Sub dgBMP_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBMP.CellEndEdit, dgLandUse.CellEndEdit, dgSoilType.CellEndEdit
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub

    Private Sub dgLandUse_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgLandUse.Validated
        'may change landuse type--need to save current values
        SaveForm()
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        btnOK.Enabled = b
        btnHelp.Enabled = b
        btnCancel.Enabled = b
        btnAbout.Enabled = b
        tabSEDIMENT.Enabled = b
        btnOpen.Enabled = b
        btnSaveAs.Enabled = b
    End Sub

    Private Sub frmSediment_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SedimentForm = Nothing
        gMapWin.StatusBar(2).Text = ""
    End Sub

    Private Sub frmSediment_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        With Project
            If .Modified Then
                If .FileName = "" Then
                    btnSaveAs.PerformClick()
                    'e.Cancel = .FileName = ""
                Else
                    Select Case Logger.Message(String.Format("Save changes to {0}?", .FileName), "File Has Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)
                        Case Windows.Forms.DialogResult.Yes : SaveForm() : e.Cancel = Not SaveData()
                        Case Windows.Forms.DialogResult.No : e.Cancel = False
                        Case Windows.Forms.DialogResult.Cancel : e.Cancel = True
                    End Select
                End If
            End If
        End With
    End Sub

    Private Sub frmSediment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Project.Initialize()

        With cboSedimentDelivery
            .Items.Clear()
            .Items.AddRange(New String() {"Distance-based method", "Distance- and relief-based method", "Area-based method", "WEPP regression equation"})
            .SelectedIndex = 0
        End With

        With cboLanduse
            .Items.Clear()
            .Items.AddRange(New String() {"USGS GIRAS Shapefiles", "NLCD 1992 Grid", "NLCD 2001 Grid", "User Shapefile", "User Grid"})
            .SelectedIndex = 0
        End With

        cboSubbasins.Items.Clear()
        cboSoilLayer.Items.Clear()
        cboLandUseLayer.Items.Clear()
        cboRoadLayer.Items.Clear()
        cboDEMLayer.Items.Clear()

        Try
            For i As Integer = 0 To GisUtil.NumLayers - 1
                Dim ln As String = GisUtil.LayerName(i)
                Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(i)

                If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cboSubbasins.Items.Add(ln)
                    cboSoilLayer.Items.Add(ln)
                    cboBMPLayer.Items.Add(ln)
                ElseIf lt = MapWindow.Interfaces.eLayerType.LineShapefile Then
                    cboRoadLayer.Items.Add(ln)
                ElseIf lt = MapWindow.Interfaces.eLayerType.Grid Then
                    cboDEMLayer.Items.Add(ln)
                    'cboStreamLayer.Items.Add(ln)
                End If
            Next

            'set reasonable defaults, in case Default.sediment not found

            With cboSubbasins
                For Each item As String In .Items
                    If item.ToUpper.Contains("SUBBASIN") Then Project.SubbasinLayer = item : Exit For
                Next
            End With
            With cboSoilLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("SOIL") Then Project.SoilLayer = item : Exit For
                Next
            End With
            With cboBMPLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("BMP") Then Project.BMPLayer = item : Exit For
                Next
            End With
            With cboRoadLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("ROADS") Then Project.RoadLayer = item : Exit For
                Next
            End With
            With cboLandUseLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("LANDUSE") Then Project.LandUseLayer = item : Exit For
                Next
            End With
            With cboDEMLayer
                For Each item As String In .Items
                    If item.ToUpper.Contains("DIGITAL ELEVATION") Then Project.DEMLayer = item : Exit For
                Next
            End With

        Catch ex As Exception
            Logger.Message("Unable to connect to MapWindow", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try

        LoadData()
        LoadForm()

        Dim sedfiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Project.SedimentFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.sediment")
        Select Case sedfiles.Count
            Case 0
            Case 1
                LoadData(sedfiles(0))
                LoadForm()
                wbResults.DocumentText = ""
            Case Else
                btnOpen.PerformClick()
        End Select

        SedimentForm = Me
    End Sub

    ''' <summary>
    ''' Move data from Project structure into form fields
    ''' </summary>
    Private Sub LoadForm()
        Try
            With Project
                Text = "BASINS Sediment Estimator"
                If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
                txtOutputName.Text = .OutputFolder
                txtGridSize.ValueInteger = .GridSize
                cboSubbasins.Text = .SubbasinLayer

                cboSoilLayer.Text = .SoilLayer
                cboSoilIDField.Text = .SoilField

                cboLanduse.SelectedIndex = .LanduseType
                cboLandUseLayer.Text = .LandUseLayer
                cboLandUseIDField.Text = .LandUseField
                cboRoadLayer.Text = .RoadLayer

                cboDEMLayer.Text = .DEMLayer
                cboDEMUnits.SelectedIndex = .DEMUnits

                txtRainfall.Text = .R_Factor

                For i As Integer = 1 To 3
                    Dim dict As Generic.Dictionary(Of String, clsLookup) = Choose(i, .dictSoil, .dictLandUse(cboLanduse.SelectedIndex), .dictBMP)
                    With CType(Choose(i, dgSoilType, dgLandUse, dgBMP), DataGridView)
                        .AllowUserToAddRows = False
                        .Rows.Clear()
                        For Each key As String In dict.Keys
                            .Rows.Add(key, dict.Item(key).Description, dict.Item(key).Factor)
                        Next
                        .Sort(.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                        For c As Integer = 0 To .ColumnCount - 1
                            .Columns(c).SortMode = DataGridViewColumnSortMode.NotSortable
                            If c = 2 Then
                                .Columns(c).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                            Else
                                .Columns(c).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft
                            End If
                        Next
                        .Sort(.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
                        .AllowUserToAddRows = True
                        For c As Integer = 0 To .ColumnCount - 1
                            .Columns(c).SortMode = DataGridViewColumnSortMode.NotSortable
                        Next
                    End With
                Next i

                chkBMPs.Checked = .UseBMP
                cboBMPLayer.Text = .BMPLayer
                cboBMPIDField.Text = .BMPField

                cboSedimentDelivery.SelectedIndex = .DeliveryMethod
                'cboStreamLayer.Text = .StreamLayer
                txtThreshold.Text = .StreamThreshold
            End With
        Catch ex As Exception
            Logger.Message("Error while trying to load form:" & vbCr & vbCr & ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub

    ''' <summary>
    ''' Move data from form fields into Project structure
    ''' </summary>
    Friend Sub SaveForm()
        Try
            With Project
                Text = "BASINS Sediment Estimator"
                If .FileName <> "" Then Text &= " - " & IO.Path.GetFileName(.FileName)
                .OutputFolder = txtOutputName.Text
                .GridSize = txtGridSize.ValueInteger
                .SubbasinLayer = cboSubbasins.Text

                .SoilLayer = cboSoilLayer.Text
                .SoilField = cboSoilIDField.Text

                .LanduseType = cboLanduse.SelectedIndex
                .LandUseLayer = cboLandUseLayer.Text
                .LandUseField = cboLandUseIDField.Text
                .RoadLayer = cboRoadLayer.Text

                .DEMLayer = cboDEMLayer.Text
                .DEMUnits = cboDEMUnits.SelectedIndex

                .R_Factor = txtRainfall.Text

                For i As Integer = 1 To 3
                    Dim dict As Generic.Dictionary(Of String, clsLookup) = Choose(i, .dictSoil, .dictLandUse(cboLanduse.SelectedIndex), .dictBMP)
                    dict.Clear()
                    With CType(Choose(i, dgSoilType, dgLandUse, dgBMP), DataGridView)
                        .EndEdit()
                        For r As Integer = 0 To .RowCount - 2
                            With .Rows(r)
                                If Not (IsDBNull(.Cells(0).Value) Or IsDBNull(.Cells(2).Value)) And Not dict.ContainsKey(.Cells(0).Value) Then
                                    dict.Add(.Cells(0).Value, New clsLookup(.Cells(1).Value, .Cells(2).Value))
                                End If
                            End With
                        Next
                    End With
                Next
                .UseBMP = chkBMPs.Checked
                .BMPLayer = cboBMPLayer.Text
                .BMPField = cboBMPIDField.Text

                .DeliveryMethod = cboSedimentDelivery.SelectedIndex
                '.StreamLayer = cboStreamLayer.Text
                .StreamThreshold = Val(txtThreshold.Text)
            End With
        Catch ex As Exception
            Logger.Message("Error while trying to save form:" & vbCr & vbCr & ex.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub

    Private Sub txtGridSize_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGridSize.KeyPress, txtOutputName.KeyPress, txtRainfall.KeyPress, txtThreshold.KeyPress
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub

    Private Sub txtGridSize_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGridSize.Validated
        Project.Modified = True
        wbResults.DocumentText = ""
    End Sub
End Class

