<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWCS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWCS))
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.cboSubbasinField = New System.Windows.Forms.ComboBox
        Me.txtOutputName = New System.Windows.Forms.TextBox
        Me.btnOutputName = New System.Windows.Forms.Button
        Me.cboSubbasinLayer = New System.Windows.Forms.ComboBox
        Me.lstReports = New System.Windows.Forms.CheckedListBox
        Me.btnAll = New System.Windows.Forms.Button
        Me.btnNone = New System.Windows.Forms.Button
        Me.cboReachLayer = New System.Windows.Forms.ComboBox
        Me.cbo303dLayer = New System.Windows.Forms.ComboBox
        Me.cboReachField = New System.Windows.Forms.ComboBox
        Me.cboWaterBodyField = New System.Windows.Forms.ComboBox
        Me.cboImpairmentField = New System.Windows.Forms.ComboBox
        Me.cboPop1Layer = New System.Windows.Forms.ComboBox
        Me.cboPop2Layer = New System.Windows.Forms.ComboBox
        Me.cboPopNameField = New System.Windows.Forms.ComboBox
        Me.cboPopPopField = New System.Windows.Forms.ComboBox
        Me.cboSewerLayer = New System.Windows.Forms.ComboBox
        Me.cboSewerNameField = New System.Windows.Forms.ComboBox
        Me.cboSewerPopField = New System.Windows.Forms.ComboBox
        Me.cboSewerHouseField = New System.Windows.Forms.ComboBox
        Me.cboSewerPublicField = New System.Windows.Forms.ComboBox
        Me.cboSewerSepticField = New System.Windows.Forms.ComboBox
        Me.cboSewerOtherField = New System.Windows.Forms.ComboBox
        Me.cboSoilLayer = New System.Windows.Forms.ComboBox
        Me.cboSoilField = New System.Windows.Forms.ComboBox
        Me.cboLandUseType = New System.Windows.Forms.ComboBox
        Me.cboLanduseField = New System.Windows.Forms.ComboBox
        Me.cboLanduseLayer = New System.Windows.Forms.ComboBox
        Me.cboPCSLayer = New System.Windows.Forms.ComboBox
        Me.cboNPDESField = New System.Windows.Forms.ComboBox
        Me.cboFacNameField = New System.Windows.Forms.ComboBox
        Me.cboSICField = New System.Windows.Forms.ComboBox
        Me.cboSICNameField = New System.Windows.Forms.ComboBox
        Me.cboCityField = New System.Windows.Forms.ComboBox
        Me.cboRecWaterField = New System.Windows.Forms.ComboBox
        Me.cboMajorField = New System.Windows.Forms.ComboBox
        Me.cboActiveField = New System.Windows.Forms.ComboBox
        Me.lstDataSources = New System.Windows.Forms.CheckedListBox
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnPreview = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.wbResults = New System.Windows.Forms.WebBrowser
        Me.btnAbout = New System.Windows.Forms.Button
        Me.btnAllDS = New System.Windows.Forms.Button
        Me.btnNoneDS = New System.Windows.Forms.Button
        Me.lnkDeleteReports = New System.Windows.Forms.LinkLabel
        Me.dgSoil = New System.Windows.Forms.DataGridView
        Me.dgLandUse = New System.Windows.Forms.DataGridView
        Me.btnHelp = New System.Windows.Forms.Button
        Me.lnkLandUseClear = New System.Windows.Forms.LinkLabel
        Me.lnkLandUseRefresh = New System.Windows.Forms.LinkLabel
        Me.lnkLandUseSave = New System.Windows.Forms.LinkLabel
        Me.lnkLandUseReset = New System.Windows.Forms.LinkLabel
        Me.tabWCS = New System.Windows.Forms.TabControl
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblOutput = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel11 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel
        Me.Label38 = New System.Windows.Forms.Label
        Me.tabReports = New System.Windows.Forms.TabControl
        Me.TabPage9 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.TabPage8 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.TabPage7 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel
        Me.lblLanduseField = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.lblLanduseLayer = New System.Windows.Forms.Label
        Me.Label40 = New System.Windows.Forms.Label
        Me.tblLanduse = New System.Windows.Forms.TableLayoutPanel
        Me.chkLanduseIDShown = New System.Windows.Forms.CheckBox
        Me.TabPage10 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.TabPage11 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel10 = New System.Windows.Forms.TableLayoutPanel
        Me.Label37 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lblGenerate = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.tblProgress = New System.Windows.Forms.TableLayoutPanel
        Me.ProgressBar = New System.Windows.Forms.ProgressBar
        Me.lnkCancel = New System.Windows.Forms.LinkLabel
        Me.lblProgress = New System.Windows.Forms.Label
        Me.chkActiveOnly = New System.Windows.Forms.CheckBox
        CType(Me.dgSoil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabWCS.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel11.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        Me.tabReports.SuspendLayout()
        Me.TabPage9.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.tblLanduse.SuspendLayout()
        Me.TabPage10.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TabPage11.SuspendLayout()
        Me.TableLayoutPanel10.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.tblProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnGenerate, "Generated the selected WCS reports.")
        Me.btnGenerate.Location = New System.Drawing.Point(414, 453)
        Me.btnGenerate.Name = "btnGenerate"
        Me.HelpProvider1.SetShowHelp(Me.btnGenerate, True)
        Me.btnGenerate.Size = New System.Drawing.Size(85, 26)
        Me.btnGenerate.TabIndex = 4
        Me.btnGenerate.Text = "&Generate"
        '
        'cboSubbasinField
        '
        Me.cboSubbasinField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasinField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasinField.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.cboSubbasinField, "You must select the field in the subbasin layer which contains the subbasin names" & _
                " or IDs. Note that these MUST be unique for each subbasin.")
        Me.cboSubbasinField.Location = New System.Drawing.Point(119, 211)
        Me.cboSubbasinField.Name = "cboSubbasinField"
        Me.HelpProvider1.SetShowHelp(Me.cboSubbasinField, True)
        Me.cboSubbasinField.Size = New System.Drawing.Size(367, 21)
        Me.cboSubbasinField.TabIndex = 8
        '
        'txtOutputName
        '
        Me.txtOutputName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.txtOutputName, resources.GetString("txtOutputName.HelpString"))
        Me.txtOutputName.Location = New System.Drawing.Point(119, 136)
        Me.txtOutputName.Name = "txtOutputName"
        Me.HelpProvider1.SetShowHelp(Me.txtOutputName, True)
        Me.txtOutputName.Size = New System.Drawing.Size(367, 20)
        Me.txtOutputName.TabIndex = 2
        '
        'btnOutputName
        '
        Me.btnOutputName.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.HelpProvider1.SetHelpString(Me.btnOutputName, "Browse to select the folder where report files will be stored.")
        Me.btnOutputName.Location = New System.Drawing.Point(492, 135)
        Me.btnOutputName.Name = "btnOutputName"
        Me.HelpProvider1.SetShowHelp(Me.btnOutputName, True)
        Me.btnOutputName.Size = New System.Drawing.Size(65, 23)
        Me.btnOutputName.TabIndex = 3
        Me.btnOutputName.Text = "Browse..."
        Me.btnOutputName.UseVisualStyleBackColor = True
        '
        'cboSubbasinLayer
        '
        Me.cboSubbasinLayer.AllowDrop = True
        Me.cboSubbasinLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasinLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSubbasinLayer, "Select the layer which contains closed polygons describing the extents of each su" & _
                "bbasin in your project.")
        Me.cboSubbasinLayer.Location = New System.Drawing.Point(119, 184)
        Me.cboSubbasinLayer.Name = "cboSubbasinLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboSubbasinLayer, True)
        Me.cboSubbasinLayer.Size = New System.Drawing.Size(367, 21)
        Me.cboSubbasinLayer.Sorted = True
        Me.cboSubbasinLayer.TabIndex = 6
        '
        'lstReports
        '
        Me.lstReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel9.SetColumnSpan(Me.lstReports, 2)
        Me.lstReports.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstReports, "This is a list of available WCS reports. Check the ones you are interested in; th" & _
                "ey will be generated sequentially in a single HTML report file and displayed on " & _
                "the Results tab.")
        Me.lstReports.IntegralHeight = False
        Me.lstReports.Items.AddRange(New Object() {"Water Bodies & 303d", "Population Estimates", "Housing and Sewage", "Soil Characteristics", "Landuse Classification", "Permitted Point Sources", "Data Summary"})
        Me.lstReports.Location = New System.Drawing.Point(3, 23)
        Me.lstReports.Name = "lstReports"
        Me.HelpProvider1.SetShowHelp(Me.lstReports, True)
        Me.lstReports.Size = New System.Drawing.Size(144, 342)
        Me.lstReports.TabIndex = 1
        '
        'btnAll
        '
        Me.HelpProvider1.SetHelpString(Me.btnAll, "Check all reports above")
        Me.btnAll.Location = New System.Drawing.Point(3, 371)
        Me.btnAll.Name = "btnAll"
        Me.HelpProvider1.SetShowHelp(Me.btnAll, True)
        Me.btnAll.Size = New System.Drawing.Size(69, 23)
        Me.btnAll.TabIndex = 2
        Me.btnAll.Text = "All"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnNone
        '
        Me.HelpProvider1.SetHelpString(Me.btnNone, "Uncheck all reports above")
        Me.btnNone.Location = New System.Drawing.Point(78, 371)
        Me.btnNone.Name = "btnNone"
        Me.HelpProvider1.SetShowHelp(Me.btnNone, True)
        Me.btnNone.Size = New System.Drawing.Size(69, 23)
        Me.btnNone.TabIndex = 3
        Me.btnNone.Text = "None"
        Me.btnNone.UseVisualStyleBackColor = True
        '
        'cboReachLayer
        '
        Me.cboReachLayer.AllowDrop = True
        Me.cboReachLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReachLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboReachLayer, "Select the line layer containing the streams (e.g., RF1 or RF3 layer)")
        Me.cboReachLayer.Location = New System.Drawing.Point(137, 3)
        Me.cboReachLayer.Name = "cboReachLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboReachLayer, True)
        Me.cboReachLayer.Size = New System.Drawing.Size(238, 21)
        Me.cboReachLayer.Sorted = True
        Me.cboReachLayer.TabIndex = 1
        '
        'cbo303dLayer
        '
        Me.cbo303dLayer.AllowDrop = True
        Me.cbo303dLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbo303dLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cbo303dLayer, "Select the line layer containing the 303d listed waterbodies")
        Me.cbo303dLayer.Location = New System.Drawing.Point(137, 30)
        Me.cbo303dLayer.Name = "cbo303dLayer"
        Me.HelpProvider1.SetShowHelp(Me.cbo303dLayer, True)
        Me.cbo303dLayer.Size = New System.Drawing.Size(238, 21)
        Me.cbo303dLayer.Sorted = True
        Me.cbo303dLayer.TabIndex = 3
        '
        'cboReachField
        '
        Me.cboReachField.AllowDrop = True
        Me.cboReachField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReachField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboReachField, "Select the name of the field in the reach file that contains the stream name")
        Me.cboReachField.Location = New System.Drawing.Point(137, 81)
        Me.cboReachField.Name = "cboReachField"
        Me.HelpProvider1.SetShowHelp(Me.cboReachField, True)
        Me.cboReachField.Size = New System.Drawing.Size(238, 21)
        Me.cboReachField.Sorted = True
        Me.cboReachField.TabIndex = 6
        '
        'cboWaterBodyField
        '
        Me.cboWaterBodyField.AllowDrop = True
        Me.cboWaterBodyField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboWaterBodyField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboWaterBodyField, "Select the name of the field in the 303d file that contains the water body name")
        Me.cboWaterBodyField.Location = New System.Drawing.Point(137, 108)
        Me.cboWaterBodyField.Name = "cboWaterBodyField"
        Me.HelpProvider1.SetShowHelp(Me.cboWaterBodyField, True)
        Me.cboWaterBodyField.Size = New System.Drawing.Size(238, 21)
        Me.cboWaterBodyField.Sorted = True
        Me.cboWaterBodyField.TabIndex = 8
        '
        'cboImpairmentField
        '
        Me.cboImpairmentField.AllowDrop = True
        Me.cboImpairmentField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboImpairmentField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboImpairmentField, "Select the name of the field in the 303d file that contains the description of th" & _
                "e impairment (either state or EPA definition)")
        Me.cboImpairmentField.Location = New System.Drawing.Point(137, 135)
        Me.cboImpairmentField.Name = "cboImpairmentField"
        Me.HelpProvider1.SetShowHelp(Me.cboImpairmentField, True)
        Me.cboImpairmentField.Size = New System.Drawing.Size(238, 21)
        Me.cboImpairmentField.Sorted = True
        Me.cboImpairmentField.TabIndex = 10
        '
        'cboPop1Layer
        '
        Me.cboPop1Layer.AllowDrop = True
        Me.cboPop1Layer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPop1Layer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboPop1Layer, "Select the polygon layer associated with the first census period (e.g., 1990)")
        Me.cboPop1Layer.Location = New System.Drawing.Point(92, 3)
        Me.cboPop1Layer.Name = "cboPop1Layer"
        Me.HelpProvider1.SetShowHelp(Me.cboPop1Layer, True)
        Me.cboPop1Layer.Size = New System.Drawing.Size(283, 21)
        Me.cboPop1Layer.Sorted = True
        Me.cboPop1Layer.TabIndex = 1
        '
        'cboPop2Layer
        '
        Me.cboPop2Layer.AllowDrop = True
        Me.cboPop2Layer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPop2Layer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboPop2Layer, "Select the polygon layer associated with the second census period (e.g., 2000)")
        Me.cboPop2Layer.Location = New System.Drawing.Point(92, 30)
        Me.cboPop2Layer.Name = "cboPop2Layer"
        Me.HelpProvider1.SetShowHelp(Me.cboPop2Layer, True)
        Me.cboPop2Layer.Size = New System.Drawing.Size(283, 21)
        Me.cboPop2Layer.Sorted = True
        Me.cboPop2Layer.TabIndex = 3
        '
        'cboPopNameField
        '
        Me.cboPopNameField.AllowDrop = True
        Me.cboPopNameField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPopNameField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboPopNameField, "Select the field in the census files that contains the name of each census area. " & _
                "Note that census results are tabulated by county, zip code, community, etc.")
        Me.cboPopNameField.Location = New System.Drawing.Point(92, 81)
        Me.cboPopNameField.Name = "cboPopNameField"
        Me.HelpProvider1.SetShowHelp(Me.cboPopNameField, True)
        Me.cboPopNameField.Size = New System.Drawing.Size(283, 21)
        Me.cboPopNameField.Sorted = True
        Me.cboPopNameField.TabIndex = 6
        '
        'cboPopPopField
        '
        Me.cboPopPopField.AllowDrop = True
        Me.cboPopPopField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPopPopField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboPopPopField, "Select the field that contains the population of each census area")
        Me.cboPopPopField.Location = New System.Drawing.Point(92, 108)
        Me.cboPopPopField.Name = "cboPopPopField"
        Me.HelpProvider1.SetShowHelp(Me.cboPopPopField, True)
        Me.cboPopPopField.Size = New System.Drawing.Size(283, 21)
        Me.cboPopPopField.Sorted = True
        Me.cboPopPopField.TabIndex = 8
        '
        'cboSewerLayer
        '
        Me.cboSewerLayer.AllowDrop = True
        Me.cboSewerLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerLayer, "Select the polygon layer that contains the census data including sewer coverage")
        Me.cboSewerLayer.Location = New System.Drawing.Point(86, 3)
        Me.cboSewerLayer.Name = "cboSewerLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerLayer, True)
        Me.cboSewerLayer.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerLayer.Sorted = True
        Me.cboSewerLayer.TabIndex = 1
        '
        'cboSewerNameField
        '
        Me.cboSewerNameField.AllowDrop = True
        Me.cboSewerNameField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerNameField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerNameField, "Select the field in the census files that contains the name of each census area. " & _
                "Note that census results are tabulated by county, zip code, community, etc.")
        Me.cboSewerNameField.Location = New System.Drawing.Point(86, 54)
        Me.cboSewerNameField.Name = "cboSewerNameField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerNameField, True)
        Me.cboSewerNameField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerNameField.Sorted = True
        Me.cboSewerNameField.TabIndex = 4
        '
        'cboSewerPopField
        '
        Me.cboSewerPopField.AllowDrop = True
        Me.cboSewerPopField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerPopField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerPopField, "Select the field that contains the population of each census area")
        Me.cboSewerPopField.Location = New System.Drawing.Point(86, 81)
        Me.cboSewerPopField.Name = "cboSewerPopField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerPopField, True)
        Me.cboSewerPopField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerPopField.Sorted = True
        Me.cboSewerPopField.TabIndex = 6
        '
        'cboSewerHouseField
        '
        Me.cboSewerHouseField.AllowDrop = True
        Me.cboSewerHouseField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerHouseField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerHouseField, "Select the field that contains the number of housing units in each census area")
        Me.cboSewerHouseField.Location = New System.Drawing.Point(86, 108)
        Me.cboSewerHouseField.Name = "cboSewerHouseField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerHouseField, True)
        Me.cboSewerHouseField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerHouseField.Sorted = True
        Me.cboSewerHouseField.TabIndex = 8
        '
        'cboSewerPublicField
        '
        Me.cboSewerPublicField.AllowDrop = True
        Me.cboSewerPublicField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerPublicField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerPublicField, "Select the field that contains the housing units served by public sewers in each " & _
                "census area")
        Me.cboSewerPublicField.Location = New System.Drawing.Point(86, 135)
        Me.cboSewerPublicField.Name = "cboSewerPublicField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerPublicField, True)
        Me.cboSewerPublicField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerPublicField.Sorted = True
        Me.cboSewerPublicField.TabIndex = 10
        '
        'cboSewerSepticField
        '
        Me.cboSewerSepticField.AllowDrop = True
        Me.cboSewerSepticField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerSepticField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerSepticField, "Select the field that contains the housing units served by septic systems in each" & _
                " census area")
        Me.cboSewerSepticField.Location = New System.Drawing.Point(86, 162)
        Me.cboSewerSepticField.Name = "cboSewerSepticField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerSepticField, True)
        Me.cboSewerSepticField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerSepticField.Sorted = True
        Me.cboSewerSepticField.TabIndex = 12
        '
        'cboSewerOtherField
        '
        Me.cboSewerOtherField.AllowDrop = True
        Me.cboSewerOtherField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSewerOtherField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSewerOtherField, "Select the field that contains the housing units served by other sewer system in " & _
                "each census area")
        Me.cboSewerOtherField.Location = New System.Drawing.Point(86, 189)
        Me.cboSewerOtherField.Name = "cboSewerOtherField"
        Me.HelpProvider1.SetShowHelp(Me.cboSewerOtherField, True)
        Me.cboSewerOtherField.Size = New System.Drawing.Size(289, 21)
        Me.cboSewerOtherField.Sorted = True
        Me.cboSewerOtherField.TabIndex = 14
        '
        'cboSoilLayer
        '
        Me.cboSoilLayer.AllowDrop = True
        Me.cboSoilLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSoilLayer, "Select the polygon layer describing the soil type distribution")
        Me.cboSoilLayer.Location = New System.Drawing.Point(65, 3)
        Me.cboSoilLayer.Name = "cboSoilLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboSoilLayer, True)
        Me.cboSoilLayer.Size = New System.Drawing.Size(310, 21)
        Me.cboSoilLayer.Sorted = True
        Me.cboSoilLayer.TabIndex = 1
        '
        'cboSoilField
        '
        Me.cboSoilField.AllowDrop = True
        Me.cboSoilField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSoilField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSoilField, "Select the field which contains the soil ID for each polygon (e.g., MUID); this i" & _
                "s used to link to the soil data table displayed below which contains more detail" & _
                "ed information on the soils")
        Me.cboSoilField.Location = New System.Drawing.Point(65, 54)
        Me.cboSoilField.Name = "cboSoilField"
        Me.HelpProvider1.SetShowHelp(Me.cboSoilField, True)
        Me.cboSoilField.Size = New System.Drawing.Size(310, 21)
        Me.cboSoilField.Sorted = True
        Me.cboSoilField.TabIndex = 4
        '
        'cboLandUseType
        '
        Me.cboLandUseType.AllowDrop = True
        Me.cboLandUseType.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboLandUseType, "Select the type of landuse layer to use")
        Me.cboLandUseType.Location = New System.Drawing.Point(89, 3)
        Me.cboLandUseType.Name = "cboLandUseType"
        Me.HelpProvider1.SetShowHelp(Me.cboLandUseType, True)
        Me.cboLandUseType.Size = New System.Drawing.Size(286, 21)
        Me.cboLandUseType.TabIndex = 1
        '
        'cboLanduseField
        '
        Me.cboLanduseField.AllowDrop = True
        Me.cboLanduseField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduseField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboLanduseField, "If the landuse type is not GIRAS or User Grid, select the field name correspondin" & _
                "g to the landuse ID. This is linked to the table below to return the landuse nam" & _
                "e for the report.")
        Me.cboLanduseField.Location = New System.Drawing.Point(89, 81)
        Me.cboLanduseField.Name = "cboLanduseField"
        Me.HelpProvider1.SetShowHelp(Me.cboLanduseField, True)
        Me.cboLanduseField.Size = New System.Drawing.Size(286, 21)
        Me.cboLanduseField.Sorted = True
        Me.cboLanduseField.TabIndex = 6
        '
        'cboLanduseLayer
        '
        Me.cboLanduseLayer.AllowDrop = True
        Me.cboLanduseLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboLanduseLayer, "If not using the GIRAS landuse type, select the polygon or grid layer containing " & _
                "the land use information")
        Me.cboLanduseLayer.Location = New System.Drawing.Point(89, 30)
        Me.cboLanduseLayer.Name = "cboLanduseLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboLanduseLayer, True)
        Me.cboLanduseLayer.Size = New System.Drawing.Size(286, 21)
        Me.cboLanduseLayer.Sorted = True
        Me.cboLanduseLayer.TabIndex = 3
        '
        'cboPCSLayer
        '
        Me.cboPCSLayer.AllowDrop = True
        Me.cboPCSLayer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboPCSLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboPCSLayer, "Select the point layer associated with the EPA Permit Compliance System (PCS) NPD" & _
                "ES permit locations. Note that only active permits are listed.")
        Me.cboPCSLayer.Location = New System.Drawing.Point(108, 3)
        Me.cboPCSLayer.Name = "cboPCSLayer"
        Me.HelpProvider1.SetShowHelp(Me.cboPCSLayer, True)
        Me.cboPCSLayer.Size = New System.Drawing.Size(267, 21)
        Me.cboPCSLayer.Sorted = True
        Me.cboPCSLayer.TabIndex = 1
        '
        'cboNPDESField
        '
        Me.cboNPDESField.AllowDrop = True
        Me.cboNPDESField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboNPDESField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboNPDESField, "Select the field containing the NPDES permit number.")
        Me.cboNPDESField.Location = New System.Drawing.Point(108, 54)
        Me.cboNPDESField.Name = "cboNPDESField"
        Me.HelpProvider1.SetShowHelp(Me.cboNPDESField, True)
        Me.cboNPDESField.Size = New System.Drawing.Size(267, 21)
        Me.cboNPDESField.Sorted = True
        Me.cboNPDESField.TabIndex = 4
        '
        'cboFacNameField
        '
        Me.cboFacNameField.AllowDrop = True
        Me.cboFacNameField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFacNameField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboFacNameField, "Select the field containing the NPDES facility name.")
        Me.cboFacNameField.Location = New System.Drawing.Point(108, 81)
        Me.cboFacNameField.Name = "cboFacNameField"
        Me.HelpProvider1.SetShowHelp(Me.cboFacNameField, True)
        Me.cboFacNameField.Size = New System.Drawing.Size(267, 21)
        Me.cboFacNameField.Sorted = True
        Me.cboFacNameField.TabIndex = 6
        '
        'cboSICField
        '
        Me.cboSICField.AllowDrop = True
        Me.cboSICField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSICField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSICField, "Select the field containing the Standard Industrial Classification (SIC) number.")
        Me.cboSICField.Location = New System.Drawing.Point(108, 108)
        Me.cboSICField.Name = "cboSICField"
        Me.HelpProvider1.SetShowHelp(Me.cboSICField, True)
        Me.cboSICField.Size = New System.Drawing.Size(267, 21)
        Me.cboSICField.Sorted = True
        Me.cboSICField.TabIndex = 8
        '
        'cboSICNameField
        '
        Me.cboSICNameField.AllowDrop = True
        Me.cboSICNameField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSICNameField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboSICNameField, "Select the field containing the Standard Industrial Classification (SIC) descript" & _
                "ion.")
        Me.cboSICNameField.Location = New System.Drawing.Point(108, 135)
        Me.cboSICNameField.Name = "cboSICNameField"
        Me.HelpProvider1.SetShowHelp(Me.cboSICNameField, True)
        Me.cboSICNameField.Size = New System.Drawing.Size(267, 21)
        Me.cboSICNameField.Sorted = True
        Me.cboSICNameField.TabIndex = 10
        '
        'cboCityField
        '
        Me.cboCityField.AllowDrop = True
        Me.cboCityField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCityField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboCityField, "Select the field containing the name of the nearest city to the facility.")
        Me.cboCityField.Location = New System.Drawing.Point(108, 162)
        Me.cboCityField.Name = "cboCityField"
        Me.HelpProvider1.SetShowHelp(Me.cboCityField, True)
        Me.cboCityField.Size = New System.Drawing.Size(267, 21)
        Me.cboCityField.Sorted = True
        Me.cboCityField.TabIndex = 12
        '
        'cboRecWaterField
        '
        Me.cboRecWaterField.AllowDrop = True
        Me.cboRecWaterField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRecWaterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboRecWaterField, "Select the field containing the name of the receiving water body.")
        Me.cboRecWaterField.Location = New System.Drawing.Point(108, 216)
        Me.cboRecWaterField.Name = "cboRecWaterField"
        Me.HelpProvider1.SetShowHelp(Me.cboRecWaterField, True)
        Me.cboRecWaterField.Size = New System.Drawing.Size(267, 21)
        Me.cboRecWaterField.Sorted = True
        Me.cboRecWaterField.TabIndex = 16
        '
        'cboMajorField
        '
        Me.cboMajorField.AllowDrop = True
        Me.cboMajorField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMajorField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboMajorField, "Select the field containing the flag which describes the discharger as either maj" & _
                "or or minor.")
        Me.cboMajorField.Location = New System.Drawing.Point(108, 189)
        Me.cboMajorField.Name = "cboMajorField"
        Me.HelpProvider1.SetShowHelp(Me.cboMajorField, True)
        Me.cboMajorField.Size = New System.Drawing.Size(267, 21)
        Me.cboMajorField.Sorted = True
        Me.cboMajorField.TabIndex = 14
        '
        'cboActiveField
        '
        Me.cboActiveField.AllowDrop = True
        Me.cboActiveField.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboActiveField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HelpProvider1.SetHelpString(Me.cboActiveField, "Select the field containing the flag which describes whether the permit is active" & _
                " or not.")
        Me.cboActiveField.Location = New System.Drawing.Point(108, 243)
        Me.cboActiveField.Name = "cboActiveField"
        Me.HelpProvider1.SetShowHelp(Me.cboActiveField, True)
        Me.cboActiveField.Size = New System.Drawing.Size(267, 21)
        Me.cboActiveField.Sorted = True
        Me.cboActiveField.TabIndex = 18
        '
        'lstDataSources
        '
        Me.lstDataSources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstDataSources.CheckOnClick = True
        Me.TableLayoutPanel10.SetColumnSpan(Me.lstDataSources, 3)
        Me.lstDataSources.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lstDataSources, "This is a list of all previously downloaded BASINS datasets (e.g., STORET, NWIS, " & _
                "Meteorology, etc.)")
        Me.lstDataSources.IntegralHeight = False
        Me.lstDataSources.Location = New System.Drawing.Point(3, 23)
        Me.lstDataSources.Name = "lstDataSources"
        Me.HelpProvider1.SetShowHelp(Me.lstDataSources, True)
        Me.lstDataSources.Size = New System.Drawing.Size(372, 243)
        Me.lstDataSources.TabIndex = 1
        '
        'btnCopy
        '
        Me.HelpProvider1.SetHelpString(Me.btnCopy, "Copy the entire HTML report to the clipboard.")
        Me.btnCopy.Location = New System.Drawing.Point(3, 310)
        Me.btnCopy.Name = "btnCopy"
        Me.HelpProvider1.SetShowHelp(Me.btnCopy, True)
        Me.btnCopy.Size = New System.Drawing.Size(75, 23)
        Me.btnCopy.TabIndex = 1
        Me.btnCopy.Text = "&Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HelpProvider1.SetHelpString(Me.btnPreview, "Display the print preview form.")
        Me.btnPreview.Location = New System.Drawing.Point(395, 310)
        Me.btnPreview.Name = "btnPreview"
        Me.HelpProvider1.SetShowHelp(Me.btnPreview, True)
        Me.btnPreview.Size = New System.Drawing.Size(75, 23)
        Me.btnPreview.TabIndex = 3
        Me.btnPreview.Text = "Pre&view..."
        Me.btnPreview.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.HelpProvider1.SetHelpString(Me.btnPrint, "Send the HTML report to the printer.")
        Me.btnPrint.Location = New System.Drawing.Point(476, 310)
        Me.btnPrint.Name = "btnPrint"
        Me.HelpProvider1.SetShowHelp(Me.btnPrint, True)
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "&Print..."
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'wbResults
        '
        Me.wbResults.AllowWebBrowserDrop = False
        Me.wbResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.SetColumnSpan(Me.wbResults, 4)
        Me.HelpProvider1.SetHelpString(Me.wbResults, "When the reports are ready, this browser will contain all selected reports.")
        Me.wbResults.IsWebBrowserContextMenuEnabled = False
        Me.wbResults.Location = New System.Drawing.Point(3, 3)
        Me.wbResults.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbResults.Name = "wbResults"
        Me.HelpProvider1.SetShowHelp(Me.wbResults, True)
        Me.wbResults.Size = New System.Drawing.Size(548, 301)
        Me.wbResults.TabIndex = 0
        Me.wbResults.WebBrowserShortcutsEnabled = False
        '
        'btnAbout
        '
        Me.btnAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnAbout, "Displays the About dialog box.")
        Me.btnAbout.Location = New System.Drawing.Point(12, 453)
        Me.btnAbout.Name = "btnAbout"
        Me.HelpProvider1.SetShowHelp(Me.btnAbout, True)
        Me.btnAbout.Size = New System.Drawing.Size(75, 26)
        Me.btnAbout.TabIndex = 1
        Me.btnAbout.Text = "&About"
        '
        'btnAllDS
        '
        Me.HelpProvider1.SetHelpString(Me.btnAllDS, "Check all datasets above")
        Me.btnAllDS.Location = New System.Drawing.Point(231, 272)
        Me.btnAllDS.Name = "btnAllDS"
        Me.HelpProvider1.SetShowHelp(Me.btnAllDS, True)
        Me.btnAllDS.Size = New System.Drawing.Size(69, 23)
        Me.btnAllDS.TabIndex = 2
        Me.btnAllDS.Text = "All"
        Me.btnAllDS.UseVisualStyleBackColor = True
        '
        'btnNoneDS
        '
        Me.HelpProvider1.SetHelpString(Me.btnNoneDS, "Uncheck all datasets above")
        Me.btnNoneDS.Location = New System.Drawing.Point(306, 272)
        Me.btnNoneDS.Name = "btnNoneDS"
        Me.HelpProvider1.SetShowHelp(Me.btnNoneDS, True)
        Me.btnNoneDS.Size = New System.Drawing.Size(69, 23)
        Me.btnNoneDS.TabIndex = 3
        Me.btnNoneDS.Text = "None"
        Me.btnNoneDS.UseVisualStyleBackColor = True
        '
        'lnkDeleteReports
        '
        Me.lnkDeleteReports.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkDeleteReports.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkDeleteReports, "Report files are automatically created and sequentially numbered each time you cl" & _
                "ick the Generate button. Click this link to delete all old report files.")
        Me.lnkDeleteReports.Location = New System.Drawing.Point(209, 164)
        Me.lnkDeleteReports.Name = "lnkDeleteReports"
        Me.HelpProvider1.SetShowHelp(Me.lnkDeleteReports, True)
        Me.lnkDeleteReports.Size = New System.Drawing.Size(187, 13)
        Me.lnkDeleteReports.TabIndex = 4
        Me.lnkDeleteReports.TabStop = True
        Me.lnkDeleteReports.Text = "Delete all previously generated reports"
        '
        'dgSoil
        '
        Me.dgSoil.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgSoil.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel5.SetColumnSpan(Me.dgSoil, 2)
        Me.HelpProvider1.SetHelpString(Me.dgSoil, resources.GetString("dgSoil.HelpString"))
        Me.dgSoil.Location = New System.Drawing.Point(3, 105)
        Me.dgSoil.Name = "dgSoil"
        Me.dgSoil.ReadOnly = True
        Me.HelpProvider1.SetShowHelp(Me.dgSoil, True)
        Me.dgSoil.Size = New System.Drawing.Size(372, 190)
        Me.dgSoil.TabIndex = 6
        '
        'dgLandUse
        '
        Me.dgLandUse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel6.SetColumnSpan(Me.dgLandUse, 2)
        Me.HelpProvider1.SetHelpString(Me.dgLandUse, resources.GetString("dgLandUse.HelpString"))
        Me.dgLandUse.Location = New System.Drawing.Point(3, 176)
        Me.dgLandUse.Name = "dgLandUse"
        Me.HelpProvider1.SetShowHelp(Me.dgLandUse, True)
        Me.dgLandUse.Size = New System.Drawing.Size(372, 186)
        Me.dgLandUse.TabIndex = 10
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HelpProvider1.SetHelpString(Me.btnHelp, "Display the help file for WCS")
        Me.btnHelp.Location = New System.Drawing.Point(93, 453)
        Me.btnHelp.Name = "btnHelp"
        Me.HelpProvider1.SetShowHelp(Me.btnHelp, True)
        Me.btnHelp.Size = New System.Drawing.Size(75, 26)
        Me.btnHelp.TabIndex = 2
        Me.btnHelp.Text = "&Help"
        '
        'lnkLandUseClear
        '
        Me.lnkLandUseClear.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkLandUseClear.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkLandUseClear, "Clear the grid below")
        Me.lnkLandUseClear.Location = New System.Drawing.Point(31, 1)
        Me.lnkLandUseClear.Name = "lnkLandUseClear"
        Me.HelpProvider1.SetShowHelp(Me.lnkLandUseClear, True)
        Me.lnkLandUseClear.Size = New System.Drawing.Size(31, 13)
        Me.lnkLandUseClear.TabIndex = 0
        Me.lnkLandUseClear.TabStop = True
        Me.lnkLandUseClear.Text = "Clear"
        '
        'lnkLandUseRefresh
        '
        Me.lnkLandUseRefresh.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkLandUseRefresh.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkLandUseRefresh, "Determine all distinct landuses within all subbasins and add them to the custom g" & _
                "rid below")
        Me.lnkLandUseRefresh.Location = New System.Drawing.Point(116, 1)
        Me.lnkLandUseRefresh.Name = "lnkLandUseRefresh"
        Me.HelpProvider1.SetShowHelp(Me.lnkLandUseRefresh, True)
        Me.lnkLandUseRefresh.Size = New System.Drawing.Size(44, 13)
        Me.lnkLandUseRefresh.TabIndex = 1
        Me.lnkLandUseRefresh.TabStop = True
        Me.lnkLandUseRefresh.Text = "Refresh"
        '
        'lnkLandUseSave
        '
        Me.lnkLandUseSave.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkLandUseSave.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkLandUseSave, "All edits to the grid below will be saved")
        Me.lnkLandUseSave.Location = New System.Drawing.Point(214, 1)
        Me.lnkLandUseSave.Name = "lnkLandUseSave"
        Me.HelpProvider1.SetShowHelp(Me.lnkLandUseSave, True)
        Me.lnkLandUseSave.Size = New System.Drawing.Size(32, 13)
        Me.lnkLandUseSave.TabIndex = 2
        Me.lnkLandUseSave.TabStop = True
        Me.lnkLandUseSave.Text = "Save"
        '
        'lnkLandUseReset
        '
        Me.lnkLandUseReset.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkLandUseReset.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.lnkLandUseReset, "Reset all landuse categories and names to default values")
        Me.lnkLandUseReset.Location = New System.Drawing.Point(306, 1)
        Me.lnkLandUseReset.Name = "lnkLandUseReset"
        Me.HelpProvider1.SetShowHelp(Me.lnkLandUseReset, True)
        Me.lnkLandUseReset.Size = New System.Drawing.Size(35, 13)
        Me.lnkLandUseReset.TabIndex = 3
        Me.lnkLandUseReset.TabStop = True
        Me.lnkLandUseReset.Text = "Reset"
        '
        'tabWCS
        '
        Me.tabWCS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabWCS.Controls.Add(Me.TabPage4)
        Me.tabWCS.Controls.Add(Me.TabPage1)
        Me.tabWCS.Controls.Add(Me.TabPage2)
        Me.tabWCS.Location = New System.Drawing.Point(12, 12)
        Me.tabWCS.Name = "tabWCS"
        Me.tabWCS.SelectedIndex = 0
        Me.tabWCS.Size = New System.Drawing.Size(568, 435)
        Me.tabWCS.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(560, 342)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "General"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblOutput, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtOutputName, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnOutputName, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.cboSubbasinField, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.cboSubbasinLayer, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lnkDeleteReports, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblSubbasinsLayer, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(560, 342)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.Info
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label1, 3)
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(554, 132)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOutput
        '
        Me.lblOutput.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblOutput.AutoSize = True
        Me.lblOutput.Location = New System.Drawing.Point(3, 140)
        Me.lblOutput.Name = "lblOutput"
        Me.lblOutput.Size = New System.Drawing.Size(105, 13)
        Me.lblOutput.TabIndex = 1
        Me.lblOutput.Text = "&Output Folder Name:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 215)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Subbasin &Name Field:"
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSubbasinsLayer.AutoSize = True
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(3, 188)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(83, 13)
        Me.lblSubbasinsLayer.TabIndex = 5
        Me.lblSubbasinsLayer.Text = "Subbasin &Layer:"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel11)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(560, 409)
        Me.TabPage1.TabIndex = 1
        Me.TabPage1.Text = "Available Reports"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel11
        '
        Me.TableLayoutPanel11.ColumnCount = 2
        Me.TableLayoutPanel11.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel11.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel11.Controls.Add(Me.TableLayoutPanel9, 0, 0)
        Me.TableLayoutPanel11.Controls.Add(Me.tabReports, 1, 0)
        Me.TableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel11.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel11.Name = "TableLayoutPanel11"
        Me.TableLayoutPanel11.RowCount = 1
        Me.TableLayoutPanel11.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel11.Size = New System.Drawing.Size(554, 403)
        Me.TableLayoutPanel11.TabIndex = 0
        '
        'TableLayoutPanel9
        '
        Me.TableLayoutPanel9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel9.ColumnCount = 2
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel9.Controls.Add(Me.lstReports, 0, 1)
        Me.TableLayoutPanel9.Controls.Add(Me.btnAll, 0, 2)
        Me.TableLayoutPanel9.Controls.Add(Me.btnNone, 1, 2)
        Me.TableLayoutPanel9.Controls.Add(Me.Label38, 0, 0)
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 3
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(150, 397)
        Me.TableLayoutPanel9.TabIndex = 0
        '
        'Label38
        '
        Me.Label38.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label38.AutoSize = True
        Me.TableLayoutPanel9.SetColumnSpan(Me.Label38, 2)
        Me.Label38.Location = New System.Drawing.Point(3, 3)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(93, 13)
        Me.Label38.TabIndex = 0
        Me.Label38.Text = "A&vailable Reports:"
        '
        'tabReports
        '
        Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabReports.Controls.Add(Me.TabPage9)
        Me.tabReports.Controls.Add(Me.TabPage5)
        Me.tabReports.Controls.Add(Me.TabPage6)
        Me.tabReports.Controls.Add(Me.TabPage8)
        Me.tabReports.Controls.Add(Me.TabPage7)
        Me.tabReports.Controls.Add(Me.TabPage10)
        Me.tabReports.Controls.Add(Me.TabPage11)
        Me.tabReports.Location = New System.Drawing.Point(159, 3)
        Me.tabReports.Name = "tabReports"
        Me.tabReports.SelectedIndex = 0
        Me.tabReports.Size = New System.Drawing.Size(392, 397)
        Me.tabReports.TabIndex = 1
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.TableLayoutPanel7)
        Me.TabPage9.Location = New System.Drawing.Point(4, 22)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9.Size = New System.Drawing.Size(384, 304)
        Me.TabPage9.TabIndex = 0
        Me.TabPage9.Text = "Water Bodies"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 2
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.cboReachLayer, 1, 0)
        Me.TableLayoutPanel7.Controls.Add(Me.Label20, 0, 0)
        Me.TableLayoutPanel7.Controls.Add(Me.Label22, 0, 1)
        Me.TableLayoutPanel7.Controls.Add(Me.cbo303dLayer, 1, 1)
        Me.TableLayoutPanel7.Controls.Add(Me.Label23, 0, 3)
        Me.TableLayoutPanel7.Controls.Add(Me.Label24, 0, 2)
        Me.TableLayoutPanel7.Controls.Add(Me.cboReachField, 1, 3)
        Me.TableLayoutPanel7.Controls.Add(Me.cboWaterBodyField, 1, 4)
        Me.TableLayoutPanel7.Controls.Add(Me.Label25, 0, 4)
        Me.TableLayoutPanel7.Controls.Add(Me.Label26, 0, 5)
        Me.TableLayoutPanel7.Controls.Add(Me.cboImpairmentField, 1, 5)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 7
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(378, 298)
        Me.TableLayoutPanel7.TabIndex = 0
        '
        'Label20
        '
        Me.Label20.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(3, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(90, 13)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "&Reach File Layer:"
        '
        'Label22
        '
        Me.Label22.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(3, 34)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(63, 13)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "&303d Layer:"
        '
        'Label23
        '
        Me.Label23.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(3, 85)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(128, 13)
        Me.Label23.TabIndex = 5
        Me.Label23.Text = "Reach File Stream &Name:"
        '
        'Label24
        '
        Me.Label24.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label24.AutoSize = True
        Me.TableLayoutPanel7.SetColumnSpan(Me.Label24, 2)
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(156, 59)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(65, 13)
        Me.Label24.TabIndex = 4
        Me.Label24.Text = "Field Names"
        '
        'Label25
        '
        Me.Label25.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(3, 112)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(124, 13)
        Me.Label25.TabIndex = 7
        Me.Label25.Text = "303d &Water Body Name:"
        '
        'Label26
        '
        Me.Label26.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(3, 139)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(88, 13)
        Me.Label26.TabIndex = 9
        Me.Label26.Text = "303d &Impairment:"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.TableLayoutPanel3)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(384, 304)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Population"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.cboPop1Layer, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label4, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label5, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.cboPop2Layer, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Label6, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.cboPopNameField, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.cboPopPopField, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.Label15, 0, 4)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 6
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(378, 298)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Census Layer &1:"
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Census Layer &2:"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 85)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "&Area Name:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.TableLayoutPanel3.SetColumnSpan(Me.Label2, 2)
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(156, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Field Names"
        '
        'Label15
        '
        Me.Label15.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(3, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 13)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "&Population:"
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.TableLayoutPanel4)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(384, 304)
        Me.TabPage6.TabIndex = 2
        Me.TabPage6.Text = "Sewage"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerLayer, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.Label7, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerNameField, 1, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label9, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label8, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.Label10, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.Label11, 0, 4)
        Me.TableLayoutPanel4.Controls.Add(Me.Label12, 0, 5)
        Me.TableLayoutPanel4.Controls.Add(Me.Label13, 0, 6)
        Me.TableLayoutPanel4.Controls.Add(Me.Label14, 0, 7)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerPopField, 1, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerHouseField, 1, 4)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerPublicField, 1, 5)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerSepticField, 1, 6)
        Me.TableLayoutPanel4.Controls.Add(Me.cboSewerOtherField, 1, 7)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 9
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(378, 298)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Census &Layer:"
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 58)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Area &Name:"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoSize = True
        Me.TableLayoutPanel4.SetColumnSpan(Me.Label8, 2)
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(156, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Field Names"
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 85)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "&Population:"
        '
        'Label11
        '
        Me.Label11.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 112)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(68, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "&House Units:"
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 139)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(72, 13)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "P&ublic Sewer:"
        '
        'Label13
        '
        Me.Label13.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(3, 166)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(77, 13)
        Me.Label13.TabIndex = 11
        Me.Label13.Text = "&Septic System:"
        '
        'Label14
        '
        Me.Label14.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(3, 193)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(69, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "&Other Sewer:"
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.TableLayoutPanel5)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage8.Size = New System.Drawing.Size(384, 304)
        Me.TabPage8.TabIndex = 3
        Me.TabPage8.Text = "Soils"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.cboSoilLayer, 1, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.Label16, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.Label18, 0, 3)
        Me.TableLayoutPanel5.Controls.Add(Me.Label19, 0, 2)
        Me.TableLayoutPanel5.Controls.Add(Me.cboSoilField, 1, 3)
        Me.TableLayoutPanel5.Controls.Add(Me.Label39, 0, 5)
        Me.TableLayoutPanel5.Controls.Add(Me.dgSoil, 0, 6)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 7
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(378, 298)
        Me.TableLayoutPanel5.TabIndex = 0
        '
        'Label16
        '
        Me.Label16.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(3, 7)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(56, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Soil &Layer:"
        '
        'Label18
        '
        Me.Label18.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(3, 58)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(41, 13)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Soil &ID:"
        '
        'Label19
        '
        Me.Label19.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label19.AutoSize = True
        Me.TableLayoutPanel5.SetColumnSpan(Me.Label19, 2)
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(156, 32)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(65, 13)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "Field Names"
        '
        'Label39
        '
        Me.Label39.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label39.AutoSize = True
        Me.TableLayoutPanel5.SetColumnSpan(Me.Label39, 2)
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(159, 83)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(60, 13)
        Me.Label39.TabIndex = 5
        Me.Label39.Text = "Data Table"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.TableLayoutPanel6)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(384, 371)
        Me.TabPage7.TabIndex = 4
        Me.TabPage7.Text = "Landuse"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 2
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.cboLandUseType, 1, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.lblLanduseField, 0, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.Label21, 0, 2)
        Me.TableLayoutPanel6.Controls.Add(Me.cboLanduseField, 1, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.Label17, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.lblLanduseLayer, 0, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.cboLanduseLayer, 1, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.Label40, 0, 5)
        Me.TableLayoutPanel6.Controls.Add(Me.dgLandUse, 0, 7)
        Me.TableLayoutPanel6.Controls.Add(Me.tblLanduse, 0, 6)
        Me.TableLayoutPanel6.Controls.Add(Me.chkLanduseIDShown, 0, 4)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 8
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(378, 365)
        Me.TableLayoutPanel6.TabIndex = 0
        '
        'lblLanduseField
        '
        Me.lblLanduseField.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLanduseField.AutoSize = True
        Me.lblLanduseField.Location = New System.Drawing.Point(3, 85)
        Me.lblLanduseField.Name = "lblLanduseField"
        Me.lblLanduseField.Size = New System.Drawing.Size(65, 13)
        Me.lblLanduseField.TabIndex = 5
        Me.lblLanduseField.Text = "Landuse &ID:"
        '
        'Label21
        '
        Me.Label21.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label21.AutoSize = True
        Me.TableLayoutPanel6.SetColumnSpan(Me.Label21, 2)
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(156, 59)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(65, 13)
        Me.Label21.TabIndex = 4
        Me.Label21.Text = "Field Names"
        '
        'Label17
        '
        Me.Label17.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(3, 7)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(63, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Layer &Type:"
        '
        'lblLanduseLayer
        '
        Me.lblLanduseLayer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblLanduseLayer.AutoSize = True
        Me.lblLanduseLayer.Location = New System.Drawing.Point(3, 34)
        Me.lblLanduseLayer.Name = "lblLanduseLayer"
        Me.lblLanduseLayer.Size = New System.Drawing.Size(80, 13)
        Me.lblLanduseLayer.TabIndex = 2
        Me.lblLanduseLayer.Text = "Landuse &Layer:"
        '
        'Label40
        '
        Me.Label40.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label40.AutoSize = True
        Me.TableLayoutPanel6.SetColumnSpan(Me.Label40, 2)
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(159, 133)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(60, 13)
        Me.Label40.TabIndex = 8
        Me.Label40.Text = "Data Table"
        '
        'tblLanduse
        '
        Me.tblLanduse.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tblLanduse.AutoSize = True
        Me.tblLanduse.BackColor = System.Drawing.Color.Transparent
        Me.tblLanduse.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.tblLanduse.ColumnCount = 4
        Me.TableLayoutPanel6.SetColumnSpan(Me.tblLanduse, 2)
        Me.tblLanduse.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
        Me.tblLanduse.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00063!))
        Me.tblLanduse.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00063!))
        Me.tblLanduse.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99812!))
        Me.tblLanduse.Controls.Add(Me.lnkLandUseClear, 0, 0)
        Me.tblLanduse.Controls.Add(Me.lnkLandUseRefresh, 1, 0)
        Me.tblLanduse.Controls.Add(Me.lnkLandUseSave, 2, 0)
        Me.tblLanduse.Controls.Add(Me.lnkLandUseReset, 3, 0)
        Me.tblLanduse.Location = New System.Drawing.Point(3, 155)
        Me.tblLanduse.Name = "tblLanduse"
        Me.tblLanduse.RowCount = 1
        Me.tblLanduse.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tblLanduse.Size = New System.Drawing.Size(372, 15)
        Me.tblLanduse.TabIndex = 9
        '
        'chkLanduseIDShown
        '
        Me.chkLanduseIDShown.AutoSize = True
        Me.TableLayoutPanel6.SetColumnSpan(Me.chkLanduseIDShown, 2)
        Me.chkLanduseIDShown.Location = New System.Drawing.Point(3, 108)
        Me.chkLanduseIDShown.Name = "chkLanduseIDShown"
        Me.chkLanduseIDShown.Size = New System.Drawing.Size(203, 17)
        Me.chkLanduseIDShown.TabIndex = 7
        Me.chkLanduseIDShown.Text = "Include Landuse ID Column in &Report"
        Me.chkLanduseIDShown.UseVisualStyleBackColor = True
        '
        'TabPage10
        '
        Me.TabPage10.Controls.Add(Me.TableLayoutPanel8)
        Me.TabPage10.Location = New System.Drawing.Point(4, 22)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage10.Size = New System.Drawing.Size(384, 371)
        Me.TabPage10.TabIndex = 5
        Me.TabPage10.Text = "PCS"
        Me.TabPage10.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 2
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.cboPCSLayer, 1, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.Label27, 0, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.cboNPDESField, 1, 2)
        Me.TableLayoutPanel8.Controls.Add(Me.Label28, 0, 2)
        Me.TableLayoutPanel8.Controls.Add(Me.Label29, 0, 1)
        Me.TableLayoutPanel8.Controls.Add(Me.Label30, 0, 3)
        Me.TableLayoutPanel8.Controls.Add(Me.Label31, 0, 4)
        Me.TableLayoutPanel8.Controls.Add(Me.Label32, 0, 5)
        Me.TableLayoutPanel8.Controls.Add(Me.Label33, 0, 6)
        Me.TableLayoutPanel8.Controls.Add(Me.Label34, 0, 7)
        Me.TableLayoutPanel8.Controls.Add(Me.cboFacNameField, 1, 3)
        Me.TableLayoutPanel8.Controls.Add(Me.cboSICField, 1, 4)
        Me.TableLayoutPanel8.Controls.Add(Me.cboSICNameField, 1, 5)
        Me.TableLayoutPanel8.Controls.Add(Me.cboCityField, 1, 6)
        Me.TableLayoutPanel8.Controls.Add(Me.Label35, 0, 8)
        Me.TableLayoutPanel8.Controls.Add(Me.cboRecWaterField, 1, 8)
        Me.TableLayoutPanel8.Controls.Add(Me.cboMajorField, 1, 7)
        Me.TableLayoutPanel8.Controls.Add(Me.Label36, 0, 9)
        Me.TableLayoutPanel8.Controls.Add(Me.cboActiveField, 1, 9)
        Me.TableLayoutPanel8.Controls.Add(Me.chkActiveOnly, 0, 10)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 12
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31.0!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(378, 365)
        Me.TableLayoutPanel8.TabIndex = 0
        '
        'Label27
        '
        Me.Label27.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(3, 7)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(60, 13)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = "PCS &Layer:"
        '
        'Label28
        '
        Me.Label28.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(3, 58)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(99, 13)
        Me.Label28.TabIndex = 3
        Me.Label28.Text = "NPDES &Permit No.:"
        '
        'Label29
        '
        Me.Label29.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label29.AutoSize = True
        Me.TableLayoutPanel8.SetColumnSpan(Me.Label29, 2)
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(156, 32)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(65, 13)
        Me.Label29.TabIndex = 2
        Me.Label29.Text = "Field Names"
        '
        'Label30
        '
        Me.Label30.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(3, 85)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(73, 13)
        Me.Label30.TabIndex = 5
        Me.Label30.Text = "&Facility Name:"
        '
        'Label31
        '
        Me.Label31.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(3, 112)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(47, 13)
        Me.Label31.TabIndex = 7
        Me.Label31.Text = "&SIC No.:"
        '
        'Label32
        '
        Me.Label32.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(3, 139)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(58, 13)
        Me.Label32.TabIndex = 9
        Me.Label32.Text = "SIC &Name:"
        '
        'Label33
        '
        Me.Label33.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(3, 166)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(27, 13)
        Me.Label33.TabIndex = 11
        Me.Label33.Text = "&City:"
        '
        'Label34
        '
        Me.Label34.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(3, 193)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(67, 13)
        Me.Label34.TabIndex = 13
        Me.Label34.Text = "&Major/Minor:"
        '
        'Label35
        '
        Me.Label35.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(3, 220)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(62, 13)
        Me.Label35.TabIndex = 15
        Me.Label35.Text = "&Waterbody:"
        '
        'Label36
        '
        Me.Label36.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(3, 247)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(83, 13)
        Me.Label36.TabIndex = 17
        Me.Label36.Text = "&Active/Inactive:"
        '
        'TabPage11
        '
        Me.TabPage11.Controls.Add(Me.TableLayoutPanel10)
        Me.TabPage11.Location = New System.Drawing.Point(4, 22)
        Me.TabPage11.Name = "TabPage11"
        Me.TabPage11.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage11.Size = New System.Drawing.Size(384, 304)
        Me.TabPage11.TabIndex = 6
        Me.TabPage11.Text = "Data"
        Me.TabPage11.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel10
        '
        Me.TableLayoutPanel10.ColumnCount = 3
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel10.Controls.Add(Me.Label37, 0, 0)
        Me.TableLayoutPanel10.Controls.Add(Me.btnAllDS, 1, 2)
        Me.TableLayoutPanel10.Controls.Add(Me.btnNoneDS, 2, 2)
        Me.TableLayoutPanel10.Controls.Add(Me.lstDataSources, 0, 1)
        Me.TableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel10.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel10.Name = "TableLayoutPanel10"
        Me.TableLayoutPanel10.RowCount = 3
        Me.TableLayoutPanel10.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel10.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel10.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel10.Size = New System.Drawing.Size(378, 298)
        Me.TableLayoutPanel10.TabIndex = 0
        '
        'Label37
        '
        Me.Label37.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label37.AutoSize = True
        Me.TableLayoutPanel10.SetColumnSpan(Me.Label37, 3)
        Me.Label37.Location = New System.Drawing.Point(3, 3)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(168, 13)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = "Select data sources to summarize:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(560, 342)
        Me.TabPage2.TabIndex = 2
        Me.TabPage2.Text = "Results"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 4
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.Controls.Add(Me.lblGenerate, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.btnCopy, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.btnPreview, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.btnPrint, 3, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.wbResults, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(554, 336)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'lblGenerate
        '
        Me.lblGenerate.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblGenerate.AutoSize = True
        Me.lblGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGenerate.Location = New System.Drawing.Point(84, 315)
        Me.lblGenerate.Name = "lblGenerate"
        Me.lblGenerate.Size = New System.Drawing.Size(190, 13)
        Me.lblGenerate.TabIndex = 2
        Me.lblGenerate.Text = "Click Generate button to refresh results"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(505, 453)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 26)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Close"
        '
        'tblProgress
        '
        Me.tblProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tblProgress.ColumnCount = 3
        Me.tblProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118.0!))
        Me.tblProgress.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.tblProgress.Controls.Add(Me.ProgressBar, 1, 0)
        Me.tblProgress.Controls.Add(Me.lnkCancel, 2, 0)
        Me.tblProgress.Controls.Add(Me.lblProgress, 0, 0)
        Me.tblProgress.Location = New System.Drawing.Point(174, 453)
        Me.tblProgress.Name = "tblProgress"
        Me.tblProgress.RowCount = 1
        Me.tblProgress.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tblProgress.Size = New System.Drawing.Size(234, 26)
        Me.tblProgress.TabIndex = 3
        Me.tblProgress.Visible = False
        '
        'ProgressBar
        '
        Me.ProgressBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar.Location = New System.Drawing.Point(73, 4)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(112, 18)
        Me.ProgressBar.TabIndex = 1
        '
        'lnkCancel
        '
        Me.lnkCancel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lnkCancel.AutoSize = True
        Me.lnkCancel.Location = New System.Drawing.Point(191, 6)
        Me.lnkCancel.Name = "lnkCancel"
        Me.lnkCancel.Size = New System.Drawing.Size(40, 13)
        Me.lnkCancel.TabIndex = 2
        Me.lnkCancel.TabStop = True
        Me.lnkCancel.Text = "Cancel"
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(6, 6)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(61, 13)
        Me.lblProgress.TabIndex = 0
        Me.lblProgress.Text = "Initializing..."
        '
        'chkActiveOnly
        '
        Me.chkActiveOnly.AutoSize = True
        Me.TableLayoutPanel8.SetColumnSpan(Me.chkActiveOnly, 2)
        Me.HelpProvider1.SetHelpString(Me.chkActiveOnly, "If checked, only records for which the contents of Active/Inactive field start wi" & _
                "th A, a, Y, or y will be included in the report.")
        Me.chkActiveOnly.Location = New System.Drawing.Point(3, 270)
        Me.chkActiveOnly.Name = "chkActiveOnly"
        Me.HelpProvider1.SetShowHelp(Me.chkActiveOnly, True)
        Me.chkActiveOnly.Size = New System.Drawing.Size(342, 17)
        Me.chkActiveOnly.TabIndex = 20
        Me.chkActiveOnly.Text = "&Display only active permits (Active/Inactive field starts with A or Y)?"
        Me.chkActiveOnly.UseVisualStyleBackColor = True
        '
        'frmWCS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 491)
        Me.Controls.Add(Me.tblProgress)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnAbout)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.tabWCS)
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 458)
        Me.Name = "frmWCS"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Watershed Characterization System (WCS)"
        CType(Me.dgSoil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgLandUse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabWCS.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel11.ResumeLayout(False)
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
        Me.tabReports.ResumeLayout(False)
        Me.TabPage9.ResumeLayout(False)
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel6.PerformLayout()
        Me.tblLanduse.ResumeLayout(False)
        Me.tblLanduse.PerformLayout()
        Me.TabPage10.ResumeLayout(False)
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel8.PerformLayout()
        Me.TabPage11.ResumeLayout(False)
        Me.TableLayoutPanel10.ResumeLayout(False)
        Me.TableLayoutPanel10.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.tblProgress.ResumeLayout(False)
        Me.tblProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents tabWCS As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblOutput As System.Windows.Forms.Label
    Friend WithEvents txtOutputName As System.Windows.Forms.TextBox
    Friend WithEvents btnOutputName As System.Windows.Forms.Button
    Friend WithEvents lblSubbasinsLayer As System.Windows.Forms.Label
    Friend WithEvents cboSubbasinLayer As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasinField As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnPreview As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAbout As System.Windows.Forms.Button
    Friend WithEvents wbResults As System.Windows.Forms.WebBrowser
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents lstReports As System.Windows.Forms.CheckedListBox
    Friend WithEvents tabReports As System.Windows.Forms.TabControl
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboPop1Layer As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cboPop2Layer As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboPopNameField As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboSewerLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cboSewerNameField As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cboSewerPopField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSewerHouseField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSewerPublicField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSewerSepticField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSewerOtherField As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboPopPopField As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboSoilLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cboSoilField As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboLandUseType As System.Windows.Forms.ComboBox
    Friend WithEvents lblLanduseField As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cboLanduseField As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblLanduseLayer As System.Windows.Forms.Label
    Friend WithEvents cboLanduseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents tblProgress As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents lnkCancel As System.Windows.Forms.LinkLabel
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboReachLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents cbo303dLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents cboReachField As System.Windows.Forms.ComboBox
    Friend WithEvents cboWaterBodyField As System.Windows.Forms.ComboBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents cboImpairmentField As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cboPCSLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents cboNPDESField As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents cboFacNameField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSICField As System.Windows.Forms.ComboBox
    Friend WithEvents cboSICNameField As System.Windows.Forms.ComboBox
    Friend WithEvents cboCityField As System.Windows.Forms.ComboBox
    Friend WithEvents cboRecWaterField As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage11 As System.Windows.Forms.TabPage
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents cboMajorField As System.Windows.Forms.ComboBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents cboActiveField As System.Windows.Forms.ComboBox
    Friend WithEvents lstDataSources As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel9 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents btnNone As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel10 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnAllDS As System.Windows.Forms.Button
    Friend WithEvents btnNoneDS As System.Windows.Forms.Button
    Friend WithEvents lnkDeleteReports As System.Windows.Forms.LinkLabel
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents dgSoil As System.Windows.Forms.DataGridView
    Friend WithEvents dgLandUse As System.Windows.Forms.DataGridView
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents lblGenerate As System.Windows.Forms.Label
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel11 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tblLanduse As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lnkLandUseClear As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkLandUseRefresh As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkLandUseSave As System.Windows.Forms.LinkLabel
    Friend WithEvents chkLanduseIDShown As System.Windows.Forms.CheckBox
    Friend WithEvents lnkLandUseReset As System.Windows.Forms.LinkLabel
    Friend WithEvents chkActiveOnly As System.Windows.Forms.CheckBox
End Class
