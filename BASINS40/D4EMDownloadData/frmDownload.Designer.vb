<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownload
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDownload))
        Me.grpBASINS = New System.Windows.Forms.GroupBox
        Me.chkBASINS_303d = New System.Windows.Forms.CheckBox
        Me.btnBrowseWDMmet = New System.Windows.Forms.Button
        Me.chkBASINS_Met = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NHD = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NED = New System.Windows.Forms.CheckBox
        Me.chkBASINS_LSTORET = New System.Windows.Forms.CheckBox
        Me.chkBASINS_GIRAS = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEMG = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEM = New System.Windows.Forms.CheckBox
        Me.chkBASINS_Census = New System.Windows.Forms.CheckBox
        Me.grpNWIS = New System.Windows.Forms.GroupBox
        Me.chkNWIS_GetNWISWQ = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISMeasurements = New System.Windows.Forms.CheckBox
        Me.btnBrowseWDMdailydischarge = New System.Windows.Forms.Button
        Me.chkNWIS_GetNWISDischarge = New System.Windows.Forms.CheckBox
        Me.panelNWISnoStations = New System.Windows.Forms.Panel
        Me.lblNWISnoStations = New System.Windows.Forms.Label
        Me.chkTerraServerWebService_Urban = New System.Windows.Forms.CheckBox
        Me.grpTerraServerWebService = New System.Windows.Forms.GroupBox
        Me.chkTerraServerWebService_DRG = New System.Windows.Forms.CheckBox
        Me.chkTerraServerWebService_DOQ = New System.Windows.Forms.CheckBox
        Me.btnOther = New System.Windows.Forms.Button
        Me.grpNLCD2001 = New System.Windows.Forms.GroupBox
        Me.chkNLCD2001_1992 = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_Canopy = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_Impervious = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_LandCover = New System.Windows.Forms.CheckBox
        Me.grpNHDplus = New System.Windows.Forms.GroupBox
        Me.chkNHDplus_streamgageevent = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_hydrologicunits = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_hydrography = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_fac = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_fdr = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_elev_cm = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_All = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Catchment = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkNWISStations_qw = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_measurement = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_discharge = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_gw = New System.Windows.Forms.CheckBox
        Me.chkClip = New System.Windows.Forms.CheckBox
        Me.chkMerge = New System.Windows.Forms.CheckBox
        Me.btnDownload = New System.Windows.Forms.Button
        Me.cboRegion = New System.Windows.Forms.ComboBox
        Me.lblRegion = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpSTORET = New System.Windows.Forms.GroupBox
        Me.chkSTORET_Results = New System.Windows.Forms.CheckBox
        Me.chkSTORET_Stations = New System.Windows.Forms.CheckBox
        Me.grpNWISStations = New System.Windows.Forms.GroupBox
        Me.grpBASINS.SuspendLayout()
        Me.grpNWIS.SuspendLayout()
        Me.panelNWISnoStations.SuspendLayout()
        Me.grpTerraServerWebService.SuspendLayout()
        Me.grpNLCD2001.SuspendLayout()
        Me.grpNHDplus.SuspendLayout()
        Me.grpSTORET.SuspendLayout()
        Me.grpNWISStations.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpBASINS
        '
        Me.grpBASINS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_303d)
        Me.grpBASINS.Controls.Add(Me.btnBrowseWDMmet)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Met)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NHD)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NED)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_LSTORET)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_GIRAS)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEMG)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEM)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Census)
        Me.grpBASINS.Location = New System.Drawing.Point(12, 39)
        Me.grpBASINS.Name = "grpBASINS"
        Me.grpBASINS.Size = New System.Drawing.Size(430, 68)
        Me.grpBASINS.TabIndex = 0
        Me.grpBASINS.TabStop = False
        Me.grpBASINS.Text = "BASINS"
        '
        'chkBASINS_303d
        '
        Me.chkBASINS_303d.AutoSize = True
        Me.chkBASINS_303d.Location = New System.Drawing.Point(365, 19)
        Me.chkBASINS_303d.Name = "chkBASINS_303d"
        Me.chkBASINS_303d.Size = New System.Drawing.Size(56, 17)
        Me.chkBASINS_303d.TabIndex = 10
        Me.chkBASINS_303d.Text = "303(d)"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_303d, "EPA Listed Impaired Waters")
        Me.chkBASINS_303d.UseVisualStyleBackColor = True
        '
        'btnBrowseWDMmet
        '
        Me.btnBrowseWDMmet.Image = CType(resources.GetObject("btnBrowseWDMmet.Image"), System.Drawing.Image)
        Me.btnBrowseWDMmet.Location = New System.Drawing.Point(382, 39)
        Me.btnBrowseWDMmet.Name = "btnBrowseWDMmet"
        Me.btnBrowseWDMmet.Size = New System.Drawing.Size(20, 20)
        Me.btnBrowseWDMmet.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.btnBrowseWDMmet, "Browse to choose file name to save Meteorologic data in")
        Me.btnBrowseWDMmet.UseVisualStyleBackColor = True
        '
        'chkBASINS_Met
        '
        Me.chkBASINS_Met.AutoSize = True
        Me.chkBASINS_Met.Location = New System.Drawing.Point(289, 42)
        Me.chkBASINS_Met.Name = "chkBASINS_Met"
        Me.chkBASINS_Met.Size = New System.Drawing.Size(87, 17)
        Me.chkBASINS_Met.TabIndex = 9
        Me.chkBASINS_Met.Text = "Meteorologic"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_Met, "Weather station data in WDM format")
        Me.chkBASINS_Met.UseVisualStyleBackColor = True
        '
        'chkBASINS_NHD
        '
        Me.chkBASINS_NHD.AutoSize = True
        Me.chkBASINS_NHD.Location = New System.Drawing.Point(233, 42)
        Me.chkBASINS_NHD.Name = "chkBASINS_NHD"
        Me.chkBASINS_NHD.Size = New System.Drawing.Size(50, 17)
        Me.chkBASINS_NHD.TabIndex = 7
        Me.chkBASINS_NHD.Text = "NHD"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_NHD, "National Hydrography Dataset")
        Me.chkBASINS_NHD.UseVisualStyleBackColor = True
        '
        'chkBASINS_NED
        '
        Me.chkBASINS_NED.AutoSize = True
        Me.chkBASINS_NED.Location = New System.Drawing.Point(233, 19)
        Me.chkBASINS_NED.Name = "chkBASINS_NED"
        Me.chkBASINS_NED.Size = New System.Drawing.Size(49, 17)
        Me.chkBASINS_NED.TabIndex = 6
        Me.chkBASINS_NED.Text = "NED"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_NED, "National Elevation Dataset")
        Me.chkBASINS_NED.UseVisualStyleBackColor = True
        '
        'chkBASINS_LSTORET
        '
        Me.chkBASINS_LSTORET.AutoSize = True
        Me.chkBASINS_LSTORET.Location = New System.Drawing.Point(112, 42)
        Me.chkBASINS_LSTORET.Name = "chkBASINS_LSTORET"
        Me.chkBASINS_LSTORET.Size = New System.Drawing.Size(108, 17)
        Me.chkBASINS_LSTORET.TabIndex = 5
        Me.chkBASINS_LSTORET.Text = "Legacy STORET"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_LSTORET, "STORET data from before 1999")
        Me.chkBASINS_LSTORET.UseVisualStyleBackColor = True
        '
        'chkBASINS_GIRAS
        '
        Me.chkBASINS_GIRAS.AutoSize = True
        Me.chkBASINS_GIRAS.Location = New System.Drawing.Point(112, 19)
        Me.chkBASINS_GIRAS.Name = "chkBASINS_GIRAS"
        Me.chkBASINS_GIRAS.Size = New System.Drawing.Size(108, 17)
        Me.chkBASINS_GIRAS.TabIndex = 4
        Me.chkBASINS_GIRAS.Text = "GIRAS Land Use"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_GIRAS, "Geographic Information Retrieval and Analysis System Land Use as Shape File")
        Me.chkBASINS_GIRAS.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEMG
        '
        Me.chkBASINS_DEMG.AutoSize = True
        Me.chkBASINS_DEMG.Location = New System.Drawing.Point(6, 42)
        Me.chkBASINS_DEMG.Name = "chkBASINS_DEMG"
        Me.chkBASINS_DEMG.Size = New System.Drawing.Size(72, 17)
        Me.chkBASINS_DEMG.TabIndex = 3
        Me.chkBASINS_DEMG.Text = "DEM Grid"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_DEMG, "Digital Elevation Model as raster coverage")
        Me.chkBASINS_DEMG.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEM
        '
        Me.chkBASINS_DEM.AutoSize = True
        Me.chkBASINS_DEM.Location = New System.Drawing.Point(6, 19)
        Me.chkBASINS_DEM.Name = "chkBASINS_DEM"
        Me.chkBASINS_DEM.Size = New System.Drawing.Size(84, 17)
        Me.chkBASINS_DEM.TabIndex = 2
        Me.chkBASINS_DEM.Text = "DEM Shape"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_DEM, "Digital Elevation Model as shape file")
        Me.chkBASINS_DEM.UseVisualStyleBackColor = True
        '
        'chkBASINS_Census
        '
        Me.chkBASINS_Census.AutoSize = True
        Me.chkBASINS_Census.Location = New System.Drawing.Point(289, 19)
        Me.chkBASINS_Census.Name = "chkBASINS_Census"
        Me.chkBASINS_Census.Size = New System.Drawing.Size(61, 17)
        Me.chkBASINS_Census.TabIndex = 8
        Me.chkBASINS_Census.Text = "Census"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_Census, "Selected map layers from US Census")
        Me.chkBASINS_Census.UseVisualStyleBackColor = True
        '
        'grpNWIS
        '
        Me.grpNWIS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISWQ)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISMeasurements)
        Me.grpNWIS.Controls.Add(Me.btnBrowseWDMdailydischarge)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISDischarge)
        Me.grpNWIS.Controls.Add(Me.panelNWISnoStations)
        Me.grpNWIS.Location = New System.Drawing.Point(12, 280)
        Me.grpNWIS.Name = "grpNWIS"
        Me.grpNWIS.Size = New System.Drawing.Size(430, 42)
        Me.grpNWIS.TabIndex = 1
        Me.grpNWIS.TabStop = False
        Me.grpNWIS.Text = "Data Values from US Geological Survey National Water Information System"
        '
        'chkNWIS_GetNWISWQ
        '
        Me.chkNWIS_GetNWISWQ.AutoSize = True
        Me.chkNWIS_GetNWISWQ.Enabled = False
        Me.chkNWIS_GetNWISWQ.Location = New System.Drawing.Point(112, 19)
        Me.chkNWIS_GetNWISWQ.Name = "chkNWIS_GetNWISWQ"
        Me.chkNWIS_GetNWISWQ.Size = New System.Drawing.Size(90, 17)
        Me.chkNWIS_GetNWISWQ.TabIndex = 26
        Me.chkNWIS_GetNWISWQ.Text = "Water Quality"
        Me.chkNWIS_GetNWISWQ.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISMeasurements
        '
        Me.chkNWIS_GetNWISMeasurements.AutoSize = True
        Me.chkNWIS_GetNWISMeasurements.Enabled = False
        Me.chkNWIS_GetNWISMeasurements.Location = New System.Drawing.Point(205, 19)
        Me.chkNWIS_GetNWISMeasurements.Name = "chkNWIS_GetNWISMeasurements"
        Me.chkNWIS_GetNWISMeasurements.Size = New System.Drawing.Size(95, 17)
        Me.chkNWIS_GetNWISMeasurements.TabIndex = 27
        Me.chkNWIS_GetNWISMeasurements.Text = "Measurements"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISMeasurements, "Periodic Manual Streamflow Measurements")
        Me.chkNWIS_GetNWISMeasurements.UseVisualStyleBackColor = True
        '
        'btnBrowseWDMdailydischarge
        '
        Me.btnBrowseWDMdailydischarge.Image = CType(resources.GetObject("btnBrowseWDMdailydischarge.Image"), System.Drawing.Image)
        Me.btnBrowseWDMdailydischarge.Location = New System.Drawing.Point(112, 16)
        Me.btnBrowseWDMdailydischarge.Name = "btnBrowseWDMdailydischarge"
        Me.btnBrowseWDMdailydischarge.Size = New System.Drawing.Size(20, 20)
        Me.btnBrowseWDMdailydischarge.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.btnBrowseWDMdailydischarge, "Browse to choose WDM file name")
        Me.btnBrowseWDMdailydischarge.UseVisualStyleBackColor = True
        Me.btnBrowseWDMdailydischarge.Visible = False
        '
        'chkNWIS_GetNWISDischarge
        '
        Me.chkNWIS_GetNWISDischarge.AutoSize = True
        Me.chkNWIS_GetNWISDischarge.Enabled = False
        Me.chkNWIS_GetNWISDischarge.Location = New System.Drawing.Point(6, 19)
        Me.chkNWIS_GetNWISDischarge.Name = "chkNWIS_GetNWISDischarge"
        Me.chkNWIS_GetNWISDischarge.Size = New System.Drawing.Size(100, 17)
        Me.chkNWIS_GetNWISDischarge.TabIndex = 24
        Me.chkNWIS_GetNWISDischarge.Text = "Daily Discharge"
        Me.chkNWIS_GetNWISDischarge.UseVisualStyleBackColor = True
        '
        'panelNWISnoStations
        '
        Me.panelNWISnoStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelNWISnoStations.Controls.Add(Me.lblNWISnoStations)
        Me.panelNWISnoStations.Location = New System.Drawing.Point(6, 16)
        Me.panelNWISnoStations.Name = "panelNWISnoStations"
        Me.panelNWISnoStations.Size = New System.Drawing.Size(418, 20)
        Me.panelNWISnoStations.TabIndex = 28
        Me.panelNWISnoStations.Visible = False
        '
        'lblNWISnoStations
        '
        Me.lblNWISnoStations.AutoSize = True
        Me.lblNWISnoStations.Location = New System.Drawing.Point(3, 4)
        Me.lblNWISnoStations.Name = "lblNWISnoStations"
        Me.lblNWISnoStations.Size = New System.Drawing.Size(363, 13)
        Me.lblNWISnoStations.TabIndex = 0
        Me.lblNWISnoStations.Text = "Station Locations must be selected on the map before data value download"
        '
        'chkTerraServerWebService_Urban
        '
        Me.chkTerraServerWebService_Urban.AutoSize = True
        Me.chkTerraServerWebService_Urban.Location = New System.Drawing.Point(6, 19)
        Me.chkTerraServerWebService_Urban.Name = "chkTerraServerWebService_Urban"
        Me.chkTerraServerWebService_Urban.Size = New System.Drawing.Size(80, 17)
        Me.chkTerraServerWebService_Urban.TabIndex = 12
        Me.chkTerraServerWebService_Urban.Text = "Urban Area"
        Me.chkTerraServerWebService_Urban.UseVisualStyleBackColor = True
        '
        'grpTerraServerWebService
        '
        Me.grpTerraServerWebService.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTerraServerWebService.Controls.Add(Me.chkTerraServerWebService_DRG)
        Me.grpTerraServerWebService.Controls.Add(Me.chkTerraServerWebService_DOQ)
        Me.grpTerraServerWebService.Controls.Add(Me.chkTerraServerWebService_Urban)
        Me.grpTerraServerWebService.Location = New System.Drawing.Point(418, 308)
        Me.grpTerraServerWebService.Name = "grpTerraServerWebService"
        Me.grpTerraServerWebService.Size = New System.Drawing.Size(430, 42)
        Me.grpTerraServerWebService.TabIndex = 9
        Me.grpTerraServerWebService.TabStop = False
        Me.grpTerraServerWebService.Text = "TerraServer Images"
        Me.grpTerraServerWebService.Visible = False
        '
        'chkTerraServerWebService_DRG
        '
        Me.chkTerraServerWebService_DRG.AutoSize = True
        Me.chkTerraServerWebService_DRG.Location = New System.Drawing.Point(233, 19)
        Me.chkTerraServerWebService_DRG.Name = "chkTerraServerWebService_DRG"
        Me.chkTerraServerWebService_DRG.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServerWebService_DRG.TabIndex = 14
        Me.chkTerraServerWebService_DRG.Text = "DRG"
        Me.chkTerraServerWebService_DRG.UseVisualStyleBackColor = True
        '
        'chkTerraServerWebService_DOQ
        '
        Me.chkTerraServerWebService_DOQ.AutoSize = True
        Me.chkTerraServerWebService_DOQ.Location = New System.Drawing.Point(112, 19)
        Me.chkTerraServerWebService_DOQ.Name = "chkTerraServerWebService_DOQ"
        Me.chkTerraServerWebService_DOQ.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServerWebService_DOQ.TabIndex = 13
        Me.chkTerraServerWebService_DOQ.Text = "DOQ"
        Me.chkTerraServerWebService_DOQ.UseVisualStyleBackColor = True
        '
        'btnOther
        '
        Me.btnOther.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOther.Location = New System.Drawing.Point(267, 432)
        Me.btnOther.Name = "btnOther"
        Me.btnOther.Size = New System.Drawing.Size(75, 23)
        Me.btnOther.TabIndex = 29
        Me.btnOther.Text = "&Other..."
        Me.ToolTip1.SetToolTip(Me.btnOther, "Launch old data download tool")
        Me.btnOther.UseVisualStyleBackColor = True
        Me.btnOther.Visible = False
        '
        'grpNLCD2001
        '
        Me.grpNLCD2001.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_1992)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_Canopy)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_Impervious)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_LandCover)
        Me.grpNLCD2001.Location = New System.Drawing.Point(12, 328)
        Me.grpNLCD2001.Name = "grpNLCD2001"
        Me.grpNLCD2001.Size = New System.Drawing.Size(430, 42)
        Me.grpNLCD2001.TabIndex = 11
        Me.grpNLCD2001.TabStop = False
        Me.grpNLCD2001.Text = "National Land Cover Data 2001"
        '
        'chkNLCD2001_1992
        '
        Me.chkNLCD2001_1992.AutoSize = True
        Me.chkNLCD2001_1992.Location = New System.Drawing.Point(289, 19)
        Me.chkNLCD2001_1992.Name = "chkNLCD2001_1992"
        Me.chkNLCD2001_1992.Size = New System.Drawing.Size(108, 17)
        Me.chkNLCD2001_1992.TabIndex = 26
        Me.chkNLCD2001_1992.Text = "1992 Land Cover"
        Me.chkNLCD2001_1992.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_Canopy
        '
        Me.chkNLCD2001_Canopy.AutoSize = True
        Me.chkNLCD2001_Canopy.Location = New System.Drawing.Point(205, 19)
        Me.chkNLCD2001_Canopy.Name = "chkNLCD2001_Canopy"
        Me.chkNLCD2001_Canopy.Size = New System.Drawing.Size(62, 17)
        Me.chkNLCD2001_Canopy.TabIndex = 25
        Me.chkNLCD2001_Canopy.Text = "Canopy"
        Me.chkNLCD2001_Canopy.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_Impervious
        '
        Me.chkNLCD2001_Impervious.AutoSize = True
        Me.chkNLCD2001_Impervious.Location = New System.Drawing.Point(112, 19)
        Me.chkNLCD2001_Impervious.Name = "chkNLCD2001_Impervious"
        Me.chkNLCD2001_Impervious.Size = New System.Drawing.Size(77, 17)
        Me.chkNLCD2001_Impervious.TabIndex = 24
        Me.chkNLCD2001_Impervious.Text = "Impervious"
        Me.chkNLCD2001_Impervious.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_LandCover
        '
        Me.chkNLCD2001_LandCover.AutoSize = True
        Me.chkNLCD2001_LandCover.Location = New System.Drawing.Point(6, 19)
        Me.chkNLCD2001_LandCover.Name = "chkNLCD2001_LandCover"
        Me.chkNLCD2001_LandCover.Size = New System.Drawing.Size(81, 17)
        Me.chkNLCD2001_LandCover.TabIndex = 23
        Me.chkNLCD2001_LandCover.Text = "Land Cover"
        Me.chkNLCD2001_LandCover.UseVisualStyleBackColor = True
        '
        'grpNHDplus
        '
        Me.grpNHDplus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_streamgageevent)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_hydrologicunits)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_hydrography)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_fac)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_fdr)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_elev_cm)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_All)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Catchment)
        Me.grpNHDplus.Location = New System.Drawing.Point(12, 113)
        Me.grpNHDplus.Name = "grpNHDplus"
        Me.grpNHDplus.Size = New System.Drawing.Size(430, 113)
        Me.grpNHDplus.TabIndex = 12
        Me.grpNHDplus.TabStop = False
        Me.grpNHDplus.Text = "National Hydrography Dataset Plus"
        '
        'chkNHDplus_streamgageevent
        '
        Me.chkNHDplus_streamgageevent.AutoSize = True
        Me.chkNHDplus_streamgageevent.Location = New System.Drawing.Point(205, 88)
        Me.chkNHDplus_streamgageevent.Name = "chkNHDplus_streamgageevent"
        Me.chkNHDplus_streamgageevent.Size = New System.Drawing.Size(119, 17)
        Me.chkNHDplus_streamgageevent.TabIndex = 18
        Me.chkNHDplus_streamgageevent.Text = "Streamgage Events"
        Me.chkNHDplus_streamgageevent.UseVisualStyleBackColor = True
        '
        'chkNHDplus_hydrologicunits
        '
        Me.chkNHDplus_hydrologicunits.AutoSize = True
        Me.chkNHDplus_hydrologicunits.Location = New System.Drawing.Point(205, 65)
        Me.chkNHDplus_hydrologicunits.Name = "chkNHDplus_hydrologicunits"
        Me.chkNHDplus_hydrologicunits.Size = New System.Drawing.Size(103, 17)
        Me.chkNHDplus_hydrologicunits.TabIndex = 17
        Me.chkNHDplus_hydrologicunits.Text = "Hydrologic Units"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_hydrologicunits, "Basin, Region, SubBasin, Subregion, Subwatershed, Watershed")
        Me.chkNHDplus_hydrologicunits.UseVisualStyleBackColor = True
        '
        'chkNHDplus_hydrography
        '
        Me.chkNHDplus_hydrography.AutoSize = True
        Me.chkNHDplus_hydrography.Location = New System.Drawing.Point(205, 42)
        Me.chkNHDplus_hydrography.Name = "chkNHDplus_hydrography"
        Me.chkNHDplus_hydrography.Size = New System.Drawing.Size(86, 17)
        Me.chkNHDplus_hydrography.TabIndex = 16
        Me.chkNHDplus_hydrography.Text = "Hydrography"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_hydrography, "NHDArea, NHDFlowline, NHDLine, NHDPoint, NHDWaterbody")
        Me.chkNHDplus_hydrography.UseVisualStyleBackColor = True
        '
        'chkNHDplus_fac
        '
        Me.chkNHDplus_fac.AutoSize = True
        Me.chkNHDplus_fac.Location = New System.Drawing.Point(6, 88)
        Me.chkNHDplus_fac.Name = "chkNHDplus_fac"
        Me.chkNHDplus_fac.Size = New System.Drawing.Size(137, 17)
        Me.chkNHDplus_fac.TabIndex = 14
        Me.chkNHDplus_fac.Text = "Flow Accumulation Grid"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_fac, "fac grid")
        Me.chkNHDplus_fac.UseVisualStyleBackColor = True
        '
        'chkNHDplus_fdr
        '
        Me.chkNHDplus_fdr.AutoSize = True
        Me.chkNHDplus_fdr.Location = New System.Drawing.Point(6, 65)
        Me.chkNHDplus_fdr.Name = "chkNHDplus_fdr"
        Me.chkNHDplus_fdr.Size = New System.Drawing.Size(115, 17)
        Me.chkNHDplus_fdr.TabIndex = 13
        Me.chkNHDplus_fdr.Text = "Flow Direction Grid"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_fdr, "fdr grid")
        Me.chkNHDplus_fdr.UseVisualStyleBackColor = True
        '
        'chkNHDplus_elev_cm
        '
        Me.chkNHDplus_elev_cm.AutoSize = True
        Me.chkNHDplus_elev_cm.Location = New System.Drawing.Point(6, 42)
        Me.chkNHDplus_elev_cm.Name = "chkNHDplus_elev_cm"
        Me.chkNHDplus_elev_cm.Size = New System.Drawing.Size(92, 17)
        Me.chkNHDplus_elev_cm.TabIndex = 12
        Me.chkNHDplus_elev_cm.Text = "Elevation Grid"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_elev_cm, "elev_cm grid")
        Me.chkNHDplus_elev_cm.UseVisualStyleBackColor = True
        '
        'chkNHDplus_All
        '
        Me.chkNHDplus_All.AutoSize = True
        Me.chkNHDplus_All.Location = New System.Drawing.Point(6, 19)
        Me.chkNHDplus_All.Name = "chkNHDplus_All"
        Me.chkNHDplus_All.Size = New System.Drawing.Size(37, 17)
        Me.chkNHDplus_All.TabIndex = 11
        Me.chkNHDplus_All.Text = "All"
        Me.chkNHDplus_All.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Catchment
        '
        Me.chkNHDplus_Catchment.AutoSize = True
        Me.chkNHDplus_Catchment.Location = New System.Drawing.Point(205, 19)
        Me.chkNHDplus_Catchment.Name = "chkNHDplus_Catchment"
        Me.chkNHDplus_Catchment.Size = New System.Drawing.Size(82, 17)
        Me.chkNHDplus_Catchment.TabIndex = 15
        Me.chkNHDplus_Catchment.Text = "Catchments"
        Me.chkNHDplus_Catchment.UseVisualStyleBackColor = True
        '
        'chkNWISStations_qw
        '
        Me.chkNWISStations_qw.AutoSize = True
        Me.chkNWISStations_qw.Location = New System.Drawing.Point(112, 19)
        Me.chkNWISStations_qw.Name = "chkNWISStations_qw"
        Me.chkNWISStations_qw.Size = New System.Drawing.Size(90, 17)
        Me.chkNWISStations_qw.TabIndex = 21
        Me.chkNWISStations_qw.Text = "Water Quality"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_qw, "Water Quality Station Point Layer")
        Me.chkNWISStations_qw.UseVisualStyleBackColor = True
        '
        'chkNWISStations_measurement
        '
        Me.chkNWISStations_measurement.AutoSize = True
        Me.chkNWISStations_measurement.Location = New System.Drawing.Point(205, 19)
        Me.chkNWISStations_measurement.Name = "chkNWISStations_measurement"
        Me.chkNWISStations_measurement.Size = New System.Drawing.Size(95, 17)
        Me.chkNWISStations_measurement.TabIndex = 22
        Me.chkNWISStations_measurement.Text = "Measurements"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_measurement, "Manual Streamflow Measurement Station Point Layer")
        Me.chkNWISStations_measurement.UseVisualStyleBackColor = True
        '
        'chkNWISStations_discharge
        '
        Me.chkNWISStations_discharge.AutoSize = True
        Me.chkNWISStations_discharge.Location = New System.Drawing.Point(6, 19)
        Me.chkNWISStations_discharge.Name = "chkNWISStations_discharge"
        Me.chkNWISStations_discharge.Size = New System.Drawing.Size(100, 17)
        Me.chkNWISStations_discharge.TabIndex = 19
        Me.chkNWISStations_discharge.Text = "Daily Discharge"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_discharge, "Daily Discharge Station Point Layer")
        Me.chkNWISStations_discharge.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw
        '
        Me.chkNWISStations_gw.AutoSize = True
        Me.chkNWISStations_gw.Location = New System.Drawing.Point(309, 19)
        Me.chkNWISStations_gw.Name = "chkNWISStations_gw"
        Me.chkNWISStations_gw.Size = New System.Drawing.Size(93, 17)
        Me.chkNWISStations_gw.TabIndex = 23
        Me.chkNWISStations_gw.Text = "Ground Water"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw, "Ground Water Station Point Layer")
        Me.chkNWISStations_gw.UseVisualStyleBackColor = True
        '
        'chkClip
        '
        Me.chkClip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkClip.AutoSize = True
        Me.chkClip.Location = New System.Drawing.Point(124, 436)
        Me.chkClip.Name = "chkClip"
        Me.chkClip.Size = New System.Drawing.Size(92, 17)
        Me.chkClip.TabIndex = 28
        Me.chkClip.Text = "Clip to Region"
        Me.ToolTip1.SetToolTip(Me.chkClip, "Discard additional data if a larger area was retrieved than was requested")
        Me.chkClip.UseVisualStyleBackColor = True
        '
        'chkMerge
        '
        Me.chkMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Location = New System.Drawing.Point(17, 436)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(56, 17)
        Me.chkMerge.TabIndex = 27
        Me.chkMerge.Text = "Merge"
        Me.ToolTip1.SetToolTip(Me.chkMerge, "Merge parts of the same dataset from different areas to form one layer")
        Me.chkMerge.UseVisualStyleBackColor = True
        '
        'btnDownload
        '
        Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDownload.Location = New System.Drawing.Point(366, 432)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 31
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'cboRegion
        '
        Me.cboRegion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegion.FormattingEnabled = True
        Me.cboRegion.Items.AddRange(New Object() {"View Rectangle", "Extent of Selected Layer"})
        Me.cboRegion.Location = New System.Drawing.Point(124, 12)
        Me.cboRegion.Name = "cboRegion"
        Me.cboRegion.Size = New System.Drawing.Size(318, 21)
        Me.cboRegion.TabIndex = 0
        '
        'lblRegion
        '
        Me.lblRegion.AutoSize = True
        Me.lblRegion.Location = New System.Drawing.Point(12, 15)
        Me.lblRegion.Name = "lblRegion"
        Me.lblRegion.Size = New System.Drawing.Size(104, 13)
        Me.lblRegion.TabIndex = 31
        Me.lblRegion.Text = "Region to Download"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(285, 432)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 30
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpSTORET
        '
        Me.grpSTORET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Results)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Stations)
        Me.grpSTORET.Location = New System.Drawing.Point(12, 376)
        Me.grpSTORET.Name = "grpSTORET"
        Me.grpSTORET.Size = New System.Drawing.Size(430, 44)
        Me.grpSTORET.TabIndex = 42
        Me.grpSTORET.TabStop = False
        Me.grpSTORET.Text = "EPA STORET Water Quality"
        '
        'chkSTORET_Results
        '
        Me.chkSTORET_Results.AutoSize = True
        Me.chkSTORET_Results.Location = New System.Drawing.Point(112, 19)
        Me.chkSTORET_Results.Name = "chkSTORET_Results"
        Me.chkSTORET_Results.Size = New System.Drawing.Size(61, 17)
        Me.chkSTORET_Results.TabIndex = 18
        Me.chkSTORET_Results.Text = "Results"
        Me.chkSTORET_Results.UseVisualStyleBackColor = True
        '
        'chkSTORET_Stations
        '
        Me.chkSTORET_Stations.AutoSize = True
        Me.chkSTORET_Stations.Location = New System.Drawing.Point(6, 19)
        Me.chkSTORET_Stations.Name = "chkSTORET_Stations"
        Me.chkSTORET_Stations.Size = New System.Drawing.Size(64, 17)
        Me.chkSTORET_Stations.TabIndex = 17
        Me.chkSTORET_Stations.Text = "Stations"
        Me.chkSTORET_Stations.UseVisualStyleBackColor = True
        '
        'grpNWISStations
        '
        Me.grpNWISStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_gw)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_qw)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_measurement)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_discharge)
        Me.grpNWISStations.Location = New System.Drawing.Point(12, 232)
        Me.grpNWISStations.Name = "grpNWISStations"
        Me.grpNWISStations.Size = New System.Drawing.Size(430, 42)
        Me.grpNWISStations.TabIndex = 23
        Me.grpNWISStations.TabStop = False
        Me.grpNWISStations.Text = "Station Locations from US Geological Survey National Water Information System"
        '
        'frmDownload
        '
        Me.AcceptButton = Me.btnDownload
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(454, 467)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpNWISStations)
        Me.Controls.Add(Me.btnOther)
        Me.Controls.Add(Me.grpNLCD2001)
        Me.Controls.Add(Me.grpSTORET)
        Me.Controls.Add(Me.chkMerge)
        Me.Controls.Add(Me.chkClip)
        Me.Controls.Add(Me.lblRegion)
        Me.Controls.Add(Me.cboRegion)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.grpNHDplus)
        Me.Controls.Add(Me.grpNWIS)
        Me.Controls.Add(Me.grpBASINS)
        Me.Controls.Add(Me.grpTerraServerWebService)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDownload"
        Me.Text = "Download Data"
        Me.grpBASINS.ResumeLayout(False)
        Me.grpBASINS.PerformLayout()
        Me.grpNWIS.ResumeLayout(False)
        Me.grpNWIS.PerformLayout()
        Me.panelNWISnoStations.ResumeLayout(False)
        Me.panelNWISnoStations.PerformLayout()
        Me.grpTerraServerWebService.ResumeLayout(False)
        Me.grpTerraServerWebService.PerformLayout()
        Me.grpNLCD2001.ResumeLayout(False)
        Me.grpNLCD2001.PerformLayout()
        Me.grpNHDplus.ResumeLayout(False)
        Me.grpNHDplus.PerformLayout()
        Me.grpSTORET.ResumeLayout(False)
        Me.grpSTORET.PerformLayout()
        Me.grpNWISStations.ResumeLayout(False)
        Me.grpNWISStations.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpBASINS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_Census As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWIS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_NHD As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_NED As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_LSTORET As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_GIRAS As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEMG As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEM As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_Met As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISMeasurements As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDischarge As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServerWebService_Urban As System.Windows.Forms.CheckBox
    Friend WithEvents grpTerraServerWebService As System.Windows.Forms.GroupBox
    Friend WithEvents chkTerraServerWebService_DRG As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServerWebService_DOQ As System.Windows.Forms.CheckBox
    Friend WithEvents grpNLCD2001 As System.Windows.Forms.GroupBox
    Friend WithEvents chkNLCD2001_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents grpNHDplus As System.Windows.Forms.GroupBox
    Friend WithEvents chkNHDplus_All As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents cboRegion As System.Windows.Forms.ComboBox
    Friend WithEvents lblRegion As System.Windows.Forms.Label
    Friend WithEvents chkClip As System.Windows.Forms.CheckBox
    Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpSTORET As System.Windows.Forms.GroupBox
    Friend WithEvents chkSTORET_Stations As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseWDMmet As System.Windows.Forms.Button
    Friend WithEvents chkNHDplus_elev_cm As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_streamgageevent As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_hydrologicunits As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Catchment As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_hydrography As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_fac As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_fdr As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD2001_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD2001_Canopy As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseWDMdailydischarge As System.Windows.Forms.Button
    Friend WithEvents chkNWIS_GetNWISWQ As System.Windows.Forms.CheckBox
    Friend WithEvents btnOther As System.Windows.Forms.Button
    Friend WithEvents chkNLCD2001_1992 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWISStations As System.Windows.Forms.GroupBox
    Friend WithEvents chkNWISStations_qw As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_measurement As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_discharge As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_303d As System.Windows.Forms.CheckBox
    Friend WithEvents chkSTORET_Results As System.Windows.Forms.CheckBox
    Friend WithEvents panelNWISnoStations As System.Windows.Forms.Panel
    Friend WithEvents lblNWISnoStations As System.Windows.Forms.Label
End Class
