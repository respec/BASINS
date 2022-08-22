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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDownload))
        Me.grpBASINS = New System.Windows.Forms.GroupBox()
        Me.chkBASINS_MetData = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_303d = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_MetStations = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_NHD = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_NED = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_LSTORET = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_GIRAS = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_DEMG = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_DEM = New System.Windows.Forms.CheckBox()
        Me.chkBASINS_Census = New System.Windows.Forms.CheckBox()
        Me.grpNWIS = New System.Windows.Forms.GroupBox()
        Me.chkNWIS_GetNWISWQ = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISDailyDischarge = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISIdaDischarge = New System.Windows.Forms.CheckBox()
        Me.panelNWISnoStations = New System.Windows.Forms.Panel()
        Me.chkNWIS_GetNWISPeriodicGW = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISMeasurements = New System.Windows.Forms.CheckBox()
        Me.lblNWISnoStations = New System.Windows.Forms.Label()
        Me.chkNWIS_GetNWISDailyGW = New System.Windows.Forms.CheckBox()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.grpUSGS_Seamless = New System.Windows.Forms.GroupBox()
        Me.chkUSGS_Seamless_NLCD2016_Impervious = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2016_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2004_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2008_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2013_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2019_Impervious = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2019_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2011_Impervious = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2011_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2001_LandCover = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2001_Impervious = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2006_Impervious = New System.Windows.Forms.CheckBox()
        Me.chkUSGS_Seamless_NLCD2006_LandCover = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkNWISStations_qw = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_measurement = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_discharge = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_gw_daily = New System.Windows.Forms.CheckBox()
        Me.chkClip = New System.Windows.Forms.CheckBox()
        Me.chkMerge = New System.Windows.Forms.CheckBox()
        Me.chkNLDAS_GetNLDASParameter = New System.Windows.Forms.CheckBox()
        Me.chkNLDAS_GetNLDASGrid = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_gw_daily_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_gw_periodic_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_discharge_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWISStations_gw_periodic = New System.Windows.Forms.CheckBox()
        Me.txtMinCount_GW = New System.Windows.Forms.TextBox()
        Me.chkNWISStations_precipitation_GW = New System.Windows.Forms.CheckBox()
        Me.chkGetNewest = New System.Windows.Forms.CheckBox()
        Me.chkNCDC_MetData = New System.Windows.Forms.CheckBox()
        Me.chkNCDC_MetStations = New System.Windows.Forms.CheckBox()
        Me.chkNHDplus2_hydrography = New System.Windows.Forms.CheckBox()
        Me.chkNHDplus2_elev_cm = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISPeriodicGW_GW = New System.Windows.Forms.CheckBox()
        Me.btnDownload = New System.Windows.Forms.Button()
        Me.cboRegion = New System.Windows.Forms.ComboBox()
        Me.lblRegion = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpSTORET = New System.Windows.Forms.GroupBox()
        Me.chkSTORET_Results = New System.Windows.Forms.CheckBox()
        Me.chkSTORET_Stations = New System.Windows.Forms.CheckBox()
        Me.grpNWISStations = New System.Windows.Forms.GroupBox()
        Me.chkCacheOnly = New System.Windows.Forms.CheckBox()
        Me.grpNLDAS = New System.Windows.Forms.GroupBox()
        Me.lblTimeZone = New System.Windows.Forms.Label()
        Me.txtTimeZone = New System.Windows.Forms.TextBox()
        Me.grpNWISStations_GW = New System.Windows.Forms.GroupBox()
        Me.lblMinCount = New System.Windows.Forms.Label()
        Me.grpNWIS_GW = New System.Windows.Forms.GroupBox()
        Me.chkNWIS_GetNWISPrecipitation_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISDailyDischarge_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISIdaDischarge_GW = New System.Windows.Forms.CheckBox()
        Me.chkNWIS_GetNWISDailyGW_GW = New System.Windows.Forms.CheckBox()
        Me.panelNWISnoStations_GW = New System.Windows.Forms.Panel()
        Me.lblNWISnoStations_GW = New System.Windows.Forms.Label()
        Me.grpNCDC = New System.Windows.Forms.GroupBox()
        Me.grpNHDplus2 = New System.Windows.Forms.GroupBox()
        Me.chkNHDplus2_All = New System.Windows.Forms.CheckBox()
        Me.chkNHDplus2_Catchment = New System.Windows.Forms.CheckBox()
        Me.grpSoils = New System.Windows.Forms.GroupBox()
        Me.chkSSURGO = New System.Windows.Forms.CheckBox()
        Me.grpBASINS.SuspendLayout()
        Me.grpNWIS.SuspendLayout()
        Me.panelNWISnoStations.SuspendLayout()
        Me.grpUSGS_Seamless.SuspendLayout()
        Me.grpSTORET.SuspendLayout()
        Me.grpNWISStations.SuspendLayout()
        Me.grpNLDAS.SuspendLayout()
        Me.grpNWISStations_GW.SuspendLayout()
        Me.grpNWIS_GW.SuspendLayout()
        Me.panelNWISnoStations_GW.SuspendLayout()
        Me.grpNCDC.SuspendLayout()
        Me.grpNHDplus2.SuspendLayout()
        Me.grpSoils.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpBASINS
        '
        Me.grpBASINS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_MetData)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_303d)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_MetStations)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NHD)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NED)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_LSTORET)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_GIRAS)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEMG)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEM)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Census)
        Me.grpBASINS.Location = New System.Drawing.Point(11, 185)
        Me.grpBASINS.Name = "grpBASINS"
        Me.grpBASINS.Size = New System.Drawing.Size(487, 65)
        Me.grpBASINS.TabIndex = 0
        Me.grpBASINS.TabStop = False
        Me.grpBASINS.Text = "BASINS"
        Me.grpBASINS.Visible = False
        '
        'chkBASINS_MetData
        '
        Me.chkBASINS_MetData.AutoSize = True
        Me.chkBASINS_MetData.Location = New System.Drawing.Point(6, 42)
        Me.chkBASINS_MetData.Name = "chkBASINS_MetData"
        Me.chkBASINS_MetData.Size = New System.Drawing.Size(70, 17)
        Me.chkBASINS_MetData.TabIndex = 3
        Me.chkBASINS_MetData.Text = "Met Data"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_MetData, "Weather station data in WDM format")
        Me.chkBASINS_MetData.UseVisualStyleBackColor = True
        '
        'chkBASINS_303d
        '
        Me.chkBASINS_303d.AutoSize = True
        Me.chkBASINS_303d.Location = New System.Drawing.Point(390, 42)
        Me.chkBASINS_303d.Name = "chkBASINS_303d"
        Me.chkBASINS_303d.Size = New System.Drawing.Size(56, 17)
        Me.chkBASINS_303d.TabIndex = 11
        Me.chkBASINS_303d.Text = "303(d)"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_303d, "EPA Listed Impaired Waters")
        Me.chkBASINS_303d.UseVisualStyleBackColor = True
        Me.chkBASINS_303d.Visible = False
        '
        'chkBASINS_MetStations
        '
        Me.chkBASINS_MetStations.AutoSize = True
        Me.chkBASINS_MetStations.Location = New System.Drawing.Point(6, 19)
        Me.chkBASINS_MetStations.Name = "chkBASINS_MetStations"
        Me.chkBASINS_MetStations.Size = New System.Drawing.Size(85, 17)
        Me.chkBASINS_MetStations.TabIndex = 2
        Me.chkBASINS_MetStations.Text = "Met Stations"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_MetStations, "Weather station locations")
        Me.chkBASINS_MetStations.UseVisualStyleBackColor = True
        '
        'chkBASINS_NHD
        '
        Me.chkBASINS_NHD.AutoSize = True
        Me.chkBASINS_NHD.Location = New System.Drawing.Point(235, 19)
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
        Me.chkBASINS_NED.Location = New System.Drawing.Point(390, 19)
        Me.chkBASINS_NED.Name = "chkBASINS_NED"
        Me.chkBASINS_NED.Size = New System.Drawing.Size(49, 17)
        Me.chkBASINS_NED.TabIndex = 6
        Me.chkBASINS_NED.Text = "NED"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_NED, "National Elevation Dataset")
        Me.chkBASINS_NED.UseVisualStyleBackColor = True
        Me.chkBASINS_NED.Visible = False
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
        Me.chkBASINS_DEMG.Location = New System.Drawing.Point(300, 19)
        Me.chkBASINS_DEMG.Name = "chkBASINS_DEMG"
        Me.chkBASINS_DEMG.Size = New System.Drawing.Size(72, 17)
        Me.chkBASINS_DEMG.TabIndex = 8
        Me.chkBASINS_DEMG.Text = "DEM Grid"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_DEMG, "Digital Elevation Model as raster coverage")
        Me.chkBASINS_DEMG.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEM
        '
        Me.chkBASINS_DEM.AutoSize = True
        Me.chkBASINS_DEM.Location = New System.Drawing.Point(300, 42)
        Me.chkBASINS_DEM.Name = "chkBASINS_DEM"
        Me.chkBASINS_DEM.Size = New System.Drawing.Size(84, 17)
        Me.chkBASINS_DEM.TabIndex = 9
        Me.chkBASINS_DEM.Text = "DEM Shape"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_DEM, "Digital Elevation Model as shape file")
        Me.chkBASINS_DEM.UseVisualStyleBackColor = True
        '
        'chkBASINS_Census
        '
        Me.chkBASINS_Census.AutoSize = True
        Me.chkBASINS_Census.Location = New System.Drawing.Point(235, 42)
        Me.chkBASINS_Census.Name = "chkBASINS_Census"
        Me.chkBASINS_Census.Size = New System.Drawing.Size(61, 17)
        Me.chkBASINS_Census.TabIndex = 10
        Me.chkBASINS_Census.Text = "Census"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_Census, "Selected map layers from US Census")
        Me.chkBASINS_Census.UseVisualStyleBackColor = True
        '
        'grpNWIS
        '
        Me.grpNWIS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISWQ)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISDailyDischarge)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISIdaDischarge)
        Me.grpNWIS.Controls.Add(Me.panelNWISnoStations)
        Me.grpNWIS.Location = New System.Drawing.Point(11, 355)
        Me.grpNWIS.Name = "grpNWIS"
        Me.grpNWIS.Size = New System.Drawing.Size(487, 65)
        Me.grpNWIS.TabIndex = 26
        Me.grpNWIS.TabStop = False
        Me.grpNWIS.Text = "Data Values from US Geological Survey National Water Information System"
        Me.grpNWIS.Visible = False
        '
        'chkNWIS_GetNWISWQ
        '
        Me.chkNWIS_GetNWISWQ.AutoSize = True
        Me.chkNWIS_GetNWISWQ.Enabled = False
        Me.chkNWIS_GetNWISWQ.Location = New System.Drawing.Point(112, 19)
        Me.chkNWIS_GetNWISWQ.Name = "chkNWIS_GetNWISWQ"
        Me.chkNWIS_GetNWISWQ.Size = New System.Drawing.Size(90, 17)
        Me.chkNWIS_GetNWISWQ.TabIndex = 28
        Me.chkNWIS_GetNWISWQ.Text = "Water Quality"
        Me.chkNWIS_GetNWISWQ.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISDailyDischarge
        '
        Me.chkNWIS_GetNWISDailyDischarge.AutoSize = True
        Me.chkNWIS_GetNWISDailyDischarge.Enabled = False
        Me.chkNWIS_GetNWISDailyDischarge.Location = New System.Drawing.Point(6, 19)
        Me.chkNWIS_GetNWISDailyDischarge.Name = "chkNWIS_GetNWISDailyDischarge"
        Me.chkNWIS_GetNWISDailyDischarge.Size = New System.Drawing.Size(100, 17)
        Me.chkNWIS_GetNWISDailyDischarge.TabIndex = 27
        Me.chkNWIS_GetNWISDailyDischarge.Text = "Daily Discharge"
        Me.chkNWIS_GetNWISDailyDischarge.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISIdaDischarge
        '
        Me.chkNWIS_GetNWISIdaDischarge.AutoSize = True
        Me.chkNWIS_GetNWISIdaDischarge.Enabled = False
        Me.chkNWIS_GetNWISIdaDischarge.Location = New System.Drawing.Point(6, 42)
        Me.chkNWIS_GetNWISIdaDischarge.Name = "chkNWIS_GetNWISIdaDischarge"
        Me.chkNWIS_GetNWISIdaDischarge.Size = New System.Drawing.Size(144, 17)
        Me.chkNWIS_GetNWISIdaDischarge.TabIndex = 31
        Me.chkNWIS_GetNWISIdaDischarge.Text = "Instantaneous Discharge"
        Me.chkNWIS_GetNWISIdaDischarge.UseVisualStyleBackColor = True
        '
        'panelNWISnoStations
        '
        Me.panelNWISnoStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelNWISnoStations.Controls.Add(Me.chkNWIS_GetNWISPeriodicGW)
        Me.panelNWISnoStations.Controls.Add(Me.chkNWIS_GetNWISMeasurements)
        Me.panelNWISnoStations.Controls.Add(Me.lblNWISnoStations)
        Me.panelNWISnoStations.Controls.Add(Me.chkNWIS_GetNWISDailyGW)
        Me.panelNWISnoStations.Location = New System.Drawing.Point(6, 19)
        Me.panelNWISnoStations.Name = "panelNWISnoStations"
        Me.panelNWISnoStations.Size = New System.Drawing.Size(475, 43)
        Me.panelNWISnoStations.TabIndex = 28
        Me.panelNWISnoStations.Visible = False
        '
        'chkNWIS_GetNWISPeriodicGW
        '
        Me.chkNWIS_GetNWISPeriodicGW.AutoSize = True
        Me.chkNWIS_GetNWISPeriodicGW.Location = New System.Drawing.Point(398, 2)
        Me.chkNWIS_GetNWISPeriodicGW.Name = "chkNWIS_GetNWISPeriodicGW"
        Me.chkNWIS_GetNWISPeriodicGW.Size = New System.Drawing.Size(64, 17)
        Me.chkNWIS_GetNWISPeriodicGW.TabIndex = 32
        Me.chkNWIS_GetNWISPeriodicGW.Text = "Periodic"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISPeriodicGW, "Periodic Groundwater Station Point Layer")
        Me.chkNWIS_GetNWISPeriodicGW.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISMeasurements
        '
        Me.chkNWIS_GetNWISMeasurements.AutoSize = True
        Me.chkNWIS_GetNWISMeasurements.Enabled = False
        Me.chkNWIS_GetNWISMeasurements.Location = New System.Drawing.Point(205, 0)
        Me.chkNWIS_GetNWISMeasurements.Name = "chkNWIS_GetNWISMeasurements"
        Me.chkNWIS_GetNWISMeasurements.Size = New System.Drawing.Size(95, 17)
        Me.chkNWIS_GetNWISMeasurements.TabIndex = 29
        Me.chkNWIS_GetNWISMeasurements.Text = "Measurements"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISMeasurements, "Periodic Manual Streamflow Measurements")
        Me.chkNWIS_GetNWISMeasurements.UseVisualStyleBackColor = True
        '
        'lblNWISnoStations
        '
        Me.lblNWISnoStations.AutoSize = True
        Me.lblNWISnoStations.Location = New System.Drawing.Point(4, 6)
        Me.lblNWISnoStations.Name = "lblNWISnoStations"
        Me.lblNWISnoStations.Size = New System.Drawing.Size(363, 13)
        Me.lblNWISnoStations.TabIndex = 0
        Me.lblNWISnoStations.Text = "Station Locations must be selected on the map before data value download"
        '
        'chkNWIS_GetNWISDailyGW
        '
        Me.chkNWIS_GetNWISDailyGW.AutoSize = True
        Me.chkNWIS_GetNWISDailyGW.Enabled = False
        Me.chkNWIS_GetNWISDailyGW.Location = New System.Drawing.Point(320, 0)
        Me.chkNWIS_GetNWISDailyGW.Name = "chkNWIS_GetNWISDailyGW"
        Me.chkNWIS_GetNWISDailyGW.Size = New System.Drawing.Size(71, 17)
        Me.chkNWIS_GetNWISDailyGW.TabIndex = 30
        Me.chkNWIS_GetNWISDailyGW.Text = "Daily GW"
        Me.chkNWIS_GetNWISDailyGW.UseVisualStyleBackColor = True
        '
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(293, 775)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(59, 23)
        Me.btnHelp.TabIndex = 46
        Me.btnHelp.Text = "Help"
        Me.ToolTip1.SetToolTip(Me.btnHelp, "Launch old data download tool")
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'grpUSGS_Seamless
        '
        Me.grpUSGS_Seamless.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2016_Impervious)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2016_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2004_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2008_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2013_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2019_Impervious)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2019_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2011_Impervious)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2011_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2001_LandCover)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2001_Impervious)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2006_Impervious)
        Me.grpUSGS_Seamless.Controls.Add(Me.chkUSGS_Seamless_NLCD2006_LandCover)
        Me.grpUSGS_Seamless.Location = New System.Drawing.Point(12, 426)
        Me.grpUSGS_Seamless.Name = "grpUSGS_Seamless"
        Me.grpUSGS_Seamless.Size = New System.Drawing.Size(486, 118)
        Me.grpUSGS_Seamless.TabIndex = 30
        Me.grpUSGS_Seamless.TabStop = False
        Me.grpUSGS_Seamless.Text = "National Land Cover Data from National Map"
        Me.grpUSGS_Seamless.Visible = False
        '
        'chkUSGS_Seamless_NLCD2016_Impervious
        '
        Me.chkUSGS_Seamless_NLCD2016_Impervious.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2016_Impervious.Location = New System.Drawing.Point(5, 88)
        Me.chkUSGS_Seamless_NLCD2016_Impervious.Name = "chkUSGS_Seamless_NLCD2016_Impervious"
        Me.chkUSGS_Seamless_NLCD2016_Impervious.Size = New System.Drawing.Size(104, 17)
        Me.chkUSGS_Seamless_NLCD2016_Impervious.TabIndex = 45
        Me.chkUSGS_Seamless_NLCD2016_Impervious.Text = "2016 Impervious"
        Me.chkUSGS_Seamless_NLCD2016_Impervious.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2016_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2016_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2016_LandCover.Location = New System.Drawing.Point(5, 65)
        Me.chkUSGS_Seamless_NLCD2016_LandCover.Name = "chkUSGS_Seamless_NLCD2016_LandCover"
        Me.chkUSGS_Seamless_NLCD2016_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2016_LandCover.TabIndex = 44
        Me.chkUSGS_Seamless_NLCD2016_LandCover.Text = "2016 Land Cover"
        Me.chkUSGS_Seamless_NLCD2016_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2004_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2004_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2004_LandCover.Location = New System.Drawing.Point(365, 19)
        Me.chkUSGS_Seamless_NLCD2004_LandCover.Name = "chkUSGS_Seamless_NLCD2004_LandCover"
        Me.chkUSGS_Seamless_NLCD2004_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2004_LandCover.TabIndex = 43
        Me.chkUSGS_Seamless_NLCD2004_LandCover.Text = "2004 Land Cover"
        Me.chkUSGS_Seamless_NLCD2004_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2008_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2008_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2008_LandCover.Location = New System.Drawing.Point(247, 19)
        Me.chkUSGS_Seamless_NLCD2008_LandCover.Name = "chkUSGS_Seamless_NLCD2008_LandCover"
        Me.chkUSGS_Seamless_NLCD2008_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2008_LandCover.TabIndex = 41
        Me.chkUSGS_Seamless_NLCD2008_LandCover.Text = "2008 Land Cover"
        Me.chkUSGS_Seamless_NLCD2008_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2013_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2013_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2013_LandCover.Location = New System.Drawing.Point(133, 19)
        Me.chkUSGS_Seamless_NLCD2013_LandCover.Name = "chkUSGS_Seamless_NLCD2013_LandCover"
        Me.chkUSGS_Seamless_NLCD2013_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2013_LandCover.TabIndex = 39
        Me.chkUSGS_Seamless_NLCD2013_LandCover.Text = "2013 Land Cover"
        Me.chkUSGS_Seamless_NLCD2013_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2019_Impervious
        '
        Me.chkUSGS_Seamless_NLCD2019_Impervious.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2019_Impervious.Location = New System.Drawing.Point(6, 42)
        Me.chkUSGS_Seamless_NLCD2019_Impervious.Name = "chkUSGS_Seamless_NLCD2019_Impervious"
        Me.chkUSGS_Seamless_NLCD2019_Impervious.Size = New System.Drawing.Size(104, 17)
        Me.chkUSGS_Seamless_NLCD2019_Impervious.TabIndex = 35
        Me.chkUSGS_Seamless_NLCD2019_Impervious.Text = "2019 Impervious"
        Me.chkUSGS_Seamless_NLCD2019_Impervious.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2019_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2019_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2019_LandCover.Location = New System.Drawing.Point(6, 19)
        Me.chkUSGS_Seamless_NLCD2019_LandCover.Name = "chkUSGS_Seamless_NLCD2019_LandCover"
        Me.chkUSGS_Seamless_NLCD2019_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2019_LandCover.TabIndex = 31
        Me.chkUSGS_Seamless_NLCD2019_LandCover.Text = "2019 Land Cover"
        Me.chkUSGS_Seamless_NLCD2019_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2011_Impervious
        '
        Me.chkUSGS_Seamless_NLCD2011_Impervious.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2011_Impervious.Location = New System.Drawing.Point(133, 65)
        Me.chkUSGS_Seamless_NLCD2011_Impervious.Name = "chkUSGS_Seamless_NLCD2011_Impervious"
        Me.chkUSGS_Seamless_NLCD2011_Impervious.Size = New System.Drawing.Size(104, 17)
        Me.chkUSGS_Seamless_NLCD2011_Impervious.TabIndex = 36
        Me.chkUSGS_Seamless_NLCD2011_Impervious.Text = "2011 Impervious"
        Me.chkUSGS_Seamless_NLCD2011_Impervious.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2011_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2011_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2011_LandCover.Location = New System.Drawing.Point(133, 42)
        Me.chkUSGS_Seamless_NLCD2011_LandCover.Name = "chkUSGS_Seamless_NLCD2011_LandCover"
        Me.chkUSGS_Seamless_NLCD2011_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2011_LandCover.TabIndex = 32
        Me.chkUSGS_Seamless_NLCD2011_LandCover.Text = "2011 Land Cover"
        Me.chkUSGS_Seamless_NLCD2011_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2001_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2001_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2001_LandCover.Location = New System.Drawing.Point(365, 42)
        Me.chkUSGS_Seamless_NLCD2001_LandCover.Name = "chkUSGS_Seamless_NLCD2001_LandCover"
        Me.chkUSGS_Seamless_NLCD2001_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2001_LandCover.TabIndex = 34
        Me.chkUSGS_Seamless_NLCD2001_LandCover.Text = "2001 Land Cover"
        Me.chkUSGS_Seamless_NLCD2001_LandCover.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2001_Impervious
        '
        Me.chkUSGS_Seamless_NLCD2001_Impervious.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2001_Impervious.Location = New System.Drawing.Point(365, 65)
        Me.chkUSGS_Seamless_NLCD2001_Impervious.Name = "chkUSGS_Seamless_NLCD2001_Impervious"
        Me.chkUSGS_Seamless_NLCD2001_Impervious.Size = New System.Drawing.Size(104, 17)
        Me.chkUSGS_Seamless_NLCD2001_Impervious.TabIndex = 38
        Me.chkUSGS_Seamless_NLCD2001_Impervious.Text = "2001 Impervious"
        Me.chkUSGS_Seamless_NLCD2001_Impervious.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2006_Impervious
        '
        Me.chkUSGS_Seamless_NLCD2006_Impervious.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2006_Impervious.Location = New System.Drawing.Point(247, 65)
        Me.chkUSGS_Seamless_NLCD2006_Impervious.Name = "chkUSGS_Seamless_NLCD2006_Impervious"
        Me.chkUSGS_Seamless_NLCD2006_Impervious.Size = New System.Drawing.Size(104, 17)
        Me.chkUSGS_Seamless_NLCD2006_Impervious.TabIndex = 37
        Me.chkUSGS_Seamless_NLCD2006_Impervious.Text = "2006 Impervious"
        Me.chkUSGS_Seamless_NLCD2006_Impervious.UseVisualStyleBackColor = True
        '
        'chkUSGS_Seamless_NLCD2006_LandCover
        '
        Me.chkUSGS_Seamless_NLCD2006_LandCover.AutoSize = True
        Me.chkUSGS_Seamless_NLCD2006_LandCover.Location = New System.Drawing.Point(247, 42)
        Me.chkUSGS_Seamless_NLCD2006_LandCover.Name = "chkUSGS_Seamless_NLCD2006_LandCover"
        Me.chkUSGS_Seamless_NLCD2006_LandCover.Size = New System.Drawing.Size(108, 17)
        Me.chkUSGS_Seamless_NLCD2006_LandCover.TabIndex = 33
        Me.chkUSGS_Seamless_NLCD2006_LandCover.Text = "2006 Land Cover"
        Me.chkUSGS_Seamless_NLCD2006_LandCover.UseVisualStyleBackColor = True
        '
        'chkNWISStations_qw
        '
        Me.chkNWISStations_qw.AutoSize = True
        Me.chkNWISStations_qw.Location = New System.Drawing.Point(112, 19)
        Me.chkNWISStations_qw.Name = "chkNWISStations_qw"
        Me.chkNWISStations_qw.Size = New System.Drawing.Size(90, 17)
        Me.chkNWISStations_qw.TabIndex = 23
        Me.chkNWISStations_qw.Text = "Water Quality"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_qw, "Water Quality Station Point Layer")
        Me.chkNWISStations_qw.UseVisualStyleBackColor = True
        '
        'chkNWISStations_measurement
        '
        Me.chkNWISStations_measurement.AutoSize = True
        Me.chkNWISStations_measurement.Location = New System.Drawing.Point(211, 19)
        Me.chkNWISStations_measurement.Name = "chkNWISStations_measurement"
        Me.chkNWISStations_measurement.Size = New System.Drawing.Size(95, 17)
        Me.chkNWISStations_measurement.TabIndex = 24
        Me.chkNWISStations_measurement.Text = "Measurements"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_measurement, "Manual Streamflow Measurement Station Point Layer")
        Me.chkNWISStations_measurement.UseVisualStyleBackColor = True
        '
        'chkNWISStations_discharge
        '
        Me.chkNWISStations_discharge.AutoSize = True
        Me.chkNWISStations_discharge.Location = New System.Drawing.Point(6, 19)
        Me.chkNWISStations_discharge.Name = "chkNWISStations_discharge"
        Me.chkNWISStations_discharge.Size = New System.Drawing.Size(74, 17)
        Me.chkNWISStations_discharge.TabIndex = 22
        Me.chkNWISStations_discharge.Text = "Discharge"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_discharge, "Discharge Station Point Layer")
        Me.chkNWISStations_discharge.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_daily
        '
        Me.chkNWISStations_gw_daily.AutoSize = True
        Me.chkNWISStations_gw_daily.Location = New System.Drawing.Point(326, 19)
        Me.chkNWISStations_gw_daily.Name = "chkNWISStations_gw_daily"
        Me.chkNWISStations_gw_daily.Size = New System.Drawing.Size(71, 17)
        Me.chkNWISStations_gw_daily.TabIndex = 25
        Me.chkNWISStations_gw_daily.Text = "Daily GW"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw_daily, "Daily Groundwater Station Point Layer")
        Me.chkNWISStations_gw_daily.UseVisualStyleBackColor = True
        '
        'chkClip
        '
        Me.chkClip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkClip.AutoSize = True
        Me.chkClip.Location = New System.Drawing.Point(17, 779)
        Me.chkClip.Name = "chkClip"
        Me.chkClip.Size = New System.Drawing.Size(92, 17)
        Me.chkClip.TabIndex = 44
        Me.chkClip.Text = "Clip to Region"
        Me.ToolTip1.SetToolTip(Me.chkClip, "Discard additional data if a larger area was retrieved than was requested")
        Me.chkClip.UseVisualStyleBackColor = True
        '
        'chkMerge
        '
        Me.chkMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Location = New System.Drawing.Point(222, 765)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(56, 17)
        Me.chkMerge.TabIndex = 43
        Me.chkMerge.Text = "Merge"
        Me.ToolTip1.SetToolTip(Me.chkMerge, "Merge parts of the same dataset from different areas to form one layer")
        Me.chkMerge.UseVisualStyleBackColor = True
        Me.chkMerge.Visible = False
        '
        'chkNLDAS_GetNLDASParameter
        '
        Me.chkNLDAS_GetNLDASParameter.AutoSize = True
        Me.chkNLDAS_GetNLDASParameter.Enabled = False
        Me.chkNLDAS_GetNLDASParameter.Location = New System.Drawing.Point(112, 19)
        Me.chkNLDAS_GetNLDASParameter.Name = "chkNLDAS_GetNLDASParameter"
        Me.chkNLDAS_GetNLDASParameter.Size = New System.Drawing.Size(293, 17)
        Me.chkNLDAS_GetNLDASParameter.TabIndex = 42
        Me.chkNLDAS_GetNLDASParameter.Text = "Hourly Data (available when grid cell(s) selected on map)"
        Me.ToolTip1.SetToolTip(Me.chkNLDAS_GetNLDASParameter, "Hourly Data for selected NLDAS grids")
        Me.chkNLDAS_GetNLDASParameter.UseVisualStyleBackColor = True
        '
        'chkNLDAS_GetNLDASGrid
        '
        Me.chkNLDAS_GetNLDASGrid.AutoSize = True
        Me.chkNLDAS_GetNLDASGrid.Location = New System.Drawing.Point(6, 19)
        Me.chkNLDAS_GetNLDASGrid.Name = "chkNLDAS_GetNLDASGrid"
        Me.chkNLDAS_GetNLDASGrid.Size = New System.Drawing.Size(45, 17)
        Me.chkNLDAS_GetNLDASGrid.TabIndex = 41
        Me.chkNLDAS_GetNLDASGrid.Text = "Grid"
        Me.ToolTip1.SetToolTip(Me.chkNLDAS_GetNLDASGrid, "NLDAS Grid Layer")
        Me.chkNLDAS_GetNLDASGrid.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_daily_GW
        '
        Me.chkNWISStations_gw_daily_GW.AutoSize = True
        Me.chkNWISStations_gw_daily_GW.Location = New System.Drawing.Point(6, 19)
        Me.chkNWISStations_gw_daily_GW.Name = "chkNWISStations_gw_daily_GW"
        Me.chkNWISStations_gw_daily_GW.Size = New System.Drawing.Size(113, 17)
        Me.chkNWISStations_gw_daily_GW.TabIndex = 25
        Me.chkNWISStations_gw_daily_GW.Text = "Daily Groundwater"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw_daily_GW, "Daily Groundwater Station Point Layer")
        Me.chkNWISStations_gw_daily_GW.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_periodic_GW
        '
        Me.chkNWISStations_gw_periodic_GW.AutoSize = True
        Me.chkNWISStations_gw_periodic_GW.Location = New System.Drawing.Point(134, 19)
        Me.chkNWISStations_gw_periodic_GW.Name = "chkNWISStations_gw_periodic_GW"
        Me.chkNWISStations_gw_periodic_GW.Size = New System.Drawing.Size(128, 17)
        Me.chkNWISStations_gw_periodic_GW.TabIndex = 24
        Me.chkNWISStations_gw_periodic_GW.Text = "Periodic Groundwater"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw_periodic_GW, "Non-Daily Groundwater Station Point Layer")
        Me.chkNWISStations_gw_periodic_GW.UseVisualStyleBackColor = True
        '
        'chkNWISStations_discharge_GW
        '
        Me.chkNWISStations_discharge_GW.AutoSize = True
        Me.chkNWISStations_discharge_GW.Location = New System.Drawing.Point(289, 19)
        Me.chkNWISStations_discharge_GW.Name = "chkNWISStations_discharge_GW"
        Me.chkNWISStations_discharge_GW.Size = New System.Drawing.Size(74, 17)
        Me.chkNWISStations_discharge_GW.TabIndex = 22
        Me.chkNWISStations_discharge_GW.Text = "Discharge"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_discharge_GW, "Discharge Station Point Layer")
        Me.chkNWISStations_discharge_GW.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_periodic
        '
        Me.chkNWISStations_gw_periodic.AutoSize = True
        Me.chkNWISStations_gw_periodic.Location = New System.Drawing.Point(404, 19)
        Me.chkNWISStations_gw_periodic.Name = "chkNWISStations_gw_periodic"
        Me.chkNWISStations_gw_periodic.Size = New System.Drawing.Size(64, 17)
        Me.chkNWISStations_gw_periodic.TabIndex = 26
        Me.chkNWISStations_gw_periodic.Text = "Periodic"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw_periodic, "Periodic Groundwater Station Point Layer")
        Me.chkNWISStations_gw_periodic.UseVisualStyleBackColor = True
        '
        'txtMinCount_GW
        '
        Me.txtMinCount_GW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMinCount_GW.Location = New System.Drawing.Point(432, 18)
        Me.txtMinCount_GW.Name = "txtMinCount_GW"
        Me.txtMinCount_GW.Size = New System.Drawing.Size(49, 20)
        Me.txtMinCount_GW.TabIndex = 26
        Me.txtMinCount_GW.Text = "1000"
        Me.ToolTip1.SetToolTip(Me.txtMinCount_GW, "Minimum number of values - omit stations with fewer")
        '
        'chkNWISStations_precipitation_GW
        '
        Me.chkNWISStations_precipitation_GW.AutoSize = True
        Me.chkNWISStations_precipitation_GW.Location = New System.Drawing.Point(6, 42)
        Me.chkNWISStations_precipitation_GW.Name = "chkNWISStations_precipitation_GW"
        Me.chkNWISStations_precipitation_GW.Size = New System.Drawing.Size(84, 17)
        Me.chkNWISStations_precipitation_GW.TabIndex = 28
        Me.chkNWISStations_precipitation_GW.Text = "Precipitation"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_precipitation_GW, "Precipitation Station Point Layer")
        Me.chkNWISStations_precipitation_GW.UseVisualStyleBackColor = True
        '
        'chkGetNewest
        '
        Me.chkGetNewest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkGetNewest.AutoSize = True
        Me.chkGetNewest.Location = New System.Drawing.Point(123, 779)
        Me.chkGetNewest.Name = "chkGetNewest"
        Me.chkGetNewest.Size = New System.Drawing.Size(82, 17)
        Me.chkGetNewest.TabIndex = 49
        Me.chkGetNewest.Text = "Get Newest"
        Me.ToolTip1.SetToolTip(Me.chkGetNewest, "Discard previously cached data for this result")
        Me.chkGetNewest.UseVisualStyleBackColor = True
        '
        'chkNCDC_MetData
        '
        Me.chkNCDC_MetData.AutoSize = True
        Me.chkNCDC_MetData.Location = New System.Drawing.Point(111, 19)
        Me.chkNCDC_MetData.Name = "chkNCDC_MetData"
        Me.chkNCDC_MetData.Size = New System.Drawing.Size(298, 17)
        Me.chkNCDC_MetData.TabIndex = 3
        Me.chkNCDC_MetData.Text = "ISH Data (Available when ISH station(s) selected on map)"
        Me.ToolTip1.SetToolTip(Me.chkNCDC_MetData, "Integrated Surface Hourly weather station data")
        Me.chkNCDC_MetData.UseVisualStyleBackColor = True
        '
        'chkNCDC_MetStations
        '
        Me.chkNCDC_MetStations.AutoSize = True
        Me.chkNCDC_MetStations.Location = New System.Drawing.Point(6, 19)
        Me.chkNCDC_MetStations.Name = "chkNCDC_MetStations"
        Me.chkNCDC_MetStations.Size = New System.Drawing.Size(85, 17)
        Me.chkNCDC_MetStations.TabIndex = 4
        Me.chkNCDC_MetStations.Text = "ISH Stations"
        Me.ToolTip1.SetToolTip(Me.chkNCDC_MetStations, "Integrated Surface Hourly weather station locations")
        Me.chkNCDC_MetStations.UseVisualStyleBackColor = True
        '
        'chkNHDplus2_hydrography
        '
        Me.chkNHDplus2_hydrography.AutoSize = True
        Me.chkNHDplus2_hydrography.Location = New System.Drawing.Point(326, 19)
        Me.chkNHDplus2_hydrography.Name = "chkNHDplus2_hydrography"
        Me.chkNHDplus2_hydrography.Size = New System.Drawing.Size(86, 17)
        Me.chkNHDplus2_hydrography.TabIndex = 18
        Me.chkNHDplus2_hydrography.Text = "Hydrography"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus2_hydrography, "NHDArea, NHDFlowline, NHDLine, NHDPoint, NHDWaterbody")
        Me.chkNHDplus2_hydrography.UseVisualStyleBackColor = True
        '
        'chkNHDplus2_elev_cm
        '
        Me.chkNHDplus2_elev_cm.AutoSize = True
        Me.chkNHDplus2_elev_cm.Location = New System.Drawing.Point(73, 19)
        Me.chkNHDplus2_elev_cm.Name = "chkNHDplus2_elev_cm"
        Me.chkNHDplus2_elev_cm.Size = New System.Drawing.Size(92, 17)
        Me.chkNHDplus2_elev_cm.TabIndex = 14
        Me.chkNHDplus2_elev_cm.Text = "Elevation Grid"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus2_elev_cm, "elev_cm grid")
        Me.chkNHDplus2_elev_cm.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISPeriodicGW_GW
        '
        Me.chkNWIS_GetNWISPeriodicGW_GW.AutoSize = True
        Me.chkNWIS_GetNWISPeriodicGW_GW.Enabled = False
        Me.chkNWIS_GetNWISPeriodicGW_GW.Location = New System.Drawing.Point(134, 19)
        Me.chkNWIS_GetNWISPeriodicGW_GW.Name = "chkNWIS_GetNWISPeriodicGW_GW"
        Me.chkNWIS_GetNWISPeriodicGW_GW.Size = New System.Drawing.Size(128, 17)
        Me.chkNWIS_GetNWISPeriodicGW_GW.TabIndex = 29
        Me.chkNWIS_GetNWISPeriodicGW_GW.Text = "Periodic Groundwater"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISPeriodicGW_GW, "Periodic Groundwater")
        Me.chkNWIS_GetNWISPeriodicGW_GW.UseVisualStyleBackColor = True
        '
        'btnDownload
        '
        Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDownload.Location = New System.Drawing.Point(423, 775)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 48
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'cboRegion
        '
        Me.cboRegion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRegion.FormattingEnabled = True
        Me.cboRegion.Location = New System.Drawing.Point(132, 12)
        Me.cboRegion.Name = "cboRegion"
        Me.cboRegion.Size = New System.Drawing.Size(367, 21)
        Me.cboRegion.TabIndex = 0
        '
        'lblRegion
        '
        Me.lblRegion.AutoSize = True
        Me.lblRegion.Location = New System.Drawing.Point(12, 15)
        Me.lblRegion.Name = "lblRegion"
        Me.lblRegion.Size = New System.Drawing.Size(104, 13)
        Me.lblRegion.TabIndex = 0
        Me.lblRegion.Text = "Region to Download"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(358, 775)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(59, 23)
        Me.btnCancel.TabIndex = 47
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpSTORET
        '
        Me.grpSTORET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Results)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Stations)
        Me.grpSTORET.Location = New System.Drawing.Point(11, 550)
        Me.grpSTORET.Name = "grpSTORET"
        Me.grpSTORET.Size = New System.Drawing.Size(487, 42)
        Me.grpSTORET.TabIndex = 36
        Me.grpSTORET.TabStop = False
        Me.grpSTORET.Text = "EPA STORET Water Quality"
        Me.grpSTORET.Visible = False
        '
        'chkSTORET_Results
        '
        Me.chkSTORET_Results.AutoSize = True
        Me.chkSTORET_Results.Enabled = False
        Me.chkSTORET_Results.Location = New System.Drawing.Point(112, 19)
        Me.chkSTORET_Results.Name = "chkSTORET_Results"
        Me.chkSTORET_Results.Size = New System.Drawing.Size(267, 17)
        Me.chkSTORET_Results.TabIndex = 40
        Me.chkSTORET_Results.Text = "Results (available when station(s) selected on map)"
        Me.chkSTORET_Results.UseVisualStyleBackColor = True
        '
        'chkSTORET_Stations
        '
        Me.chkSTORET_Stations.AutoSize = True
        Me.chkSTORET_Stations.Location = New System.Drawing.Point(6, 19)
        Me.chkSTORET_Stations.Name = "chkSTORET_Stations"
        Me.chkSTORET_Stations.Size = New System.Drawing.Size(64, 17)
        Me.chkSTORET_Stations.TabIndex = 39
        Me.chkSTORET_Stations.Text = "Stations"
        Me.chkSTORET_Stations.UseVisualStyleBackColor = True
        '
        'grpNWISStations
        '
        Me.grpNWISStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_gw_periodic)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_gw_daily)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_qw)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_measurement)
        Me.grpNWISStations.Controls.Add(Me.chkNWISStations_discharge)
        Me.grpNWISStations.Location = New System.Drawing.Point(11, 307)
        Me.grpNWISStations.Name = "grpNWISStations"
        Me.grpNWISStations.Size = New System.Drawing.Size(487, 42)
        Me.grpNWISStations.TabIndex = 21
        Me.grpNWISStations.TabStop = False
        Me.grpNWISStations.Text = "Station Locations from US Geological Survey National Water Information System"
        Me.grpNWISStations.Visible = False
        '
        'chkCacheOnly
        '
        Me.chkCacheOnly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkCacheOnly.AutoSize = True
        Me.chkCacheOnly.Location = New System.Drawing.Point(222, 779)
        Me.chkCacheOnly.Name = "chkCacheOnly"
        Me.chkCacheOnly.Size = New System.Drawing.Size(81, 17)
        Me.chkCacheOnly.TabIndex = 45
        Me.chkCacheOnly.Text = "Cache Only"
        Me.chkCacheOnly.UseVisualStyleBackColor = True
        Me.chkCacheOnly.Visible = False
        '
        'grpNLDAS
        '
        Me.grpNLDAS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLDAS.Controls.Add(Me.lblTimeZone)
        Me.grpNLDAS.Controls.Add(Me.txtTimeZone)
        Me.grpNLDAS.Controls.Add(Me.chkNLDAS_GetNLDASParameter)
        Me.grpNLDAS.Controls.Add(Me.chkNLDAS_GetNLDASGrid)
        Me.grpNLDAS.Location = New System.Drawing.Point(11, 598)
        Me.grpNLDAS.Name = "grpNLDAS"
        Me.grpNLDAS.Size = New System.Drawing.Size(487, 60)
        Me.grpNLDAS.TabIndex = 39
        Me.grpNLDAS.TabStop = False
        Me.grpNLDAS.Text = "North American Land Data Assimilation System"
        Me.grpNLDAS.Visible = False
        '
        'lblTimeZone
        '
        Me.lblTimeZone.AutoSize = True
        Me.lblTimeZone.Enabled = False
        Me.lblTimeZone.Location = New System.Drawing.Point(131, 37)
        Me.lblTimeZone.Name = "lblTimeZone"
        Me.lblTimeZone.Size = New System.Drawing.Size(155, 13)
        Me.lblTimeZone.TabIndex = 44
        Me.lblTimeZone.Text = "Project Time Zone - UTC minus"
        '
        'txtTimeZone
        '
        Me.txtTimeZone.Enabled = False
        Me.txtTimeZone.Location = New System.Drawing.Point(315, 37)
        Me.txtTimeZone.Name = "txtTimeZone"
        Me.txtTimeZone.Size = New System.Drawing.Size(39, 20)
        Me.txtTimeZone.TabIndex = 43
        Me.txtTimeZone.Text = "0"
        '
        'grpNWISStations_GW
        '
        Me.grpNWISStations_GW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWISStations_GW.Controls.Add(Me.lblMinCount)
        Me.grpNWISStations_GW.Controls.Add(Me.chkNWISStations_precipitation_GW)
        Me.grpNWISStations_GW.Controls.Add(Me.txtMinCount_GW)
        Me.grpNWISStations_GW.Controls.Add(Me.chkNWISStations_gw_daily_GW)
        Me.grpNWISStations_GW.Controls.Add(Me.chkNWISStations_gw_periodic_GW)
        Me.grpNWISStations_GW.Controls.Add(Me.chkNWISStations_discharge_GW)
        Me.grpNWISStations_GW.Location = New System.Drawing.Point(11, 39)
        Me.grpNWISStations_GW.Name = "grpNWISStations_GW"
        Me.grpNWISStations_GW.Size = New System.Drawing.Size(487, 69)
        Me.grpNWISStations_GW.TabIndex = 48
        Me.grpNWISStations_GW.TabStop = False
        Me.grpNWISStations_GW.Text = "Station Locations from US Geological Survey National Water Information System"
        Me.grpNWISStations_GW.Visible = False
        '
        'lblMinCount
        '
        Me.lblMinCount.AutoSize = True
        Me.lblMinCount.Location = New System.Drawing.Point(363, 21)
        Me.lblMinCount.Name = "lblMinCount"
        Me.lblMinCount.Size = New System.Drawing.Size(55, 13)
        Me.lblMinCount.TabIndex = 27
        Me.lblMinCount.Text = "Min Count"
        '
        'grpNWIS_GW
        '
        Me.grpNWIS_GW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISPrecipitation_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISDailyDischarge_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISPeriodicGW_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISIdaDischarge_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISDailyGW_GW)
        Me.grpNWIS_GW.Controls.Add(Me.panelNWISnoStations_GW)
        Me.grpNWIS_GW.Location = New System.Drawing.Point(11, 114)
        Me.grpNWIS_GW.Name = "grpNWIS_GW"
        Me.grpNWIS_GW.Size = New System.Drawing.Size(487, 65)
        Me.grpNWIS_GW.TabIndex = 32
        Me.grpNWIS_GW.TabStop = False
        Me.grpNWIS_GW.Text = "Data Values from US Geological Survey National Water Information System"
        Me.grpNWIS_GW.Visible = False
        '
        'chkNWIS_GetNWISPrecipitation_GW
        '
        Me.chkNWIS_GetNWISPrecipitation_GW.AutoSize = True
        Me.chkNWIS_GetNWISPrecipitation_GW.Location = New System.Drawing.Point(6, 39)
        Me.chkNWIS_GetNWISPrecipitation_GW.Name = "chkNWIS_GetNWISPrecipitation_GW"
        Me.chkNWIS_GetNWISPrecipitation_GW.Size = New System.Drawing.Size(84, 17)
        Me.chkNWIS_GetNWISPrecipitation_GW.TabIndex = 1
        Me.chkNWIS_GetNWISPrecipitation_GW.Text = "Precipitation"
        Me.chkNWIS_GetNWISPrecipitation_GW.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISDailyDischarge_GW
        '
        Me.chkNWIS_GetNWISDailyDischarge_GW.AutoSize = True
        Me.chkNWIS_GetNWISDailyDischarge_GW.Enabled = False
        Me.chkNWIS_GetNWISDailyDischarge_GW.Location = New System.Drawing.Point(290, 19)
        Me.chkNWIS_GetNWISDailyDischarge_GW.Name = "chkNWIS_GetNWISDailyDischarge_GW"
        Me.chkNWIS_GetNWISDailyDischarge_GW.Size = New System.Drawing.Size(100, 17)
        Me.chkNWIS_GetNWISDailyDischarge_GW.TabIndex = 27
        Me.chkNWIS_GetNWISDailyDischarge_GW.Text = "Daily Discharge"
        Me.chkNWIS_GetNWISDailyDischarge_GW.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISIdaDischarge_GW
        '
        Me.chkNWIS_GetNWISIdaDischarge_GW.AutoSize = True
        Me.chkNWIS_GetNWISIdaDischarge_GW.Enabled = False
        Me.chkNWIS_GetNWISIdaDischarge_GW.Location = New System.Drawing.Point(290, 39)
        Me.chkNWIS_GetNWISIdaDischarge_GW.Name = "chkNWIS_GetNWISIdaDischarge_GW"
        Me.chkNWIS_GetNWISIdaDischarge_GW.Size = New System.Drawing.Size(144, 17)
        Me.chkNWIS_GetNWISIdaDischarge_GW.TabIndex = 31
        Me.chkNWIS_GetNWISIdaDischarge_GW.Text = "Instantaneous Discharge"
        Me.chkNWIS_GetNWISIdaDischarge_GW.UseVisualStyleBackColor = True
        '
        'chkNWIS_GetNWISDailyGW_GW
        '
        Me.chkNWIS_GetNWISDailyGW_GW.AutoSize = True
        Me.chkNWIS_GetNWISDailyGW_GW.Enabled = False
        Me.chkNWIS_GetNWISDailyGW_GW.Location = New System.Drawing.Point(6, 19)
        Me.chkNWIS_GetNWISDailyGW_GW.Name = "chkNWIS_GetNWISDailyGW_GW"
        Me.chkNWIS_GetNWISDailyGW_GW.Size = New System.Drawing.Size(113, 17)
        Me.chkNWIS_GetNWISDailyGW_GW.TabIndex = 30
        Me.chkNWIS_GetNWISDailyGW_GW.Text = "Daily Groundwater"
        Me.chkNWIS_GetNWISDailyGW_GW.UseVisualStyleBackColor = True
        '
        'panelNWISnoStations_GW
        '
        Me.panelNWISnoStations_GW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelNWISnoStations_GW.Controls.Add(Me.lblNWISnoStations_GW)
        Me.panelNWISnoStations_GW.Location = New System.Drawing.Point(6, 16)
        Me.panelNWISnoStations_GW.Name = "panelNWISnoStations_GW"
        Me.panelNWISnoStations_GW.Size = New System.Drawing.Size(475, 43)
        Me.panelNWISnoStations_GW.TabIndex = 28
        Me.panelNWISnoStations_GW.Visible = False
        '
        'lblNWISnoStations_GW
        '
        Me.lblNWISnoStations_GW.AutoSize = True
        Me.lblNWISnoStations_GW.Location = New System.Drawing.Point(3, 4)
        Me.lblNWISnoStations_GW.Name = "lblNWISnoStations_GW"
        Me.lblNWISnoStations_GW.Size = New System.Drawing.Size(363, 13)
        Me.lblNWISnoStations_GW.TabIndex = 0
        Me.lblNWISnoStations_GW.Text = "Station Locations must be selected on the map before data value download"
        '
        'grpNCDC
        '
        Me.grpNCDC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNCDC.Controls.Add(Me.chkNCDC_MetStations)
        Me.grpNCDC.Controls.Add(Me.chkNCDC_MetData)
        Me.grpNCDC.Location = New System.Drawing.Point(12, 664)
        Me.grpNCDC.Name = "grpNCDC"
        Me.grpNCDC.Size = New System.Drawing.Size(487, 44)
        Me.grpNCDC.TabIndex = 50
        Me.grpNCDC.TabStop = False
        Me.grpNCDC.Text = "National Climatic Data Center"
        Me.grpNCDC.Visible = False
        '
        'grpNHDplus2
        '
        Me.grpNHDplus2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNHDplus2.Controls.Add(Me.chkNHDplus2_hydrography)
        Me.grpNHDplus2.Controls.Add(Me.chkNHDplus2_elev_cm)
        Me.grpNHDplus2.Controls.Add(Me.chkNHDplus2_All)
        Me.grpNHDplus2.Controls.Add(Me.chkNHDplus2_Catchment)
        Me.grpNHDplus2.Location = New System.Drawing.Point(11, 256)
        Me.grpNHDplus2.Name = "grpNHDplus2"
        Me.grpNHDplus2.Size = New System.Drawing.Size(487, 45)
        Me.grpNHDplus2.TabIndex = 51
        Me.grpNHDplus2.TabStop = False
        Me.grpNHDplus2.Text = "National Hydrography Dataset Plus v2.1"
        Me.grpNHDplus2.Visible = False
        '
        'chkNHDplus2_All
        '
        Me.chkNHDplus2_All.AutoSize = True
        Me.chkNHDplus2_All.Location = New System.Drawing.Point(6, 19)
        Me.chkNHDplus2_All.Name = "chkNHDplus2_All"
        Me.chkNHDplus2_All.Size = New System.Drawing.Size(37, 17)
        Me.chkNHDplus2_All.TabIndex = 13
        Me.chkNHDplus2_All.Text = "All"
        Me.chkNHDplus2_All.UseVisualStyleBackColor = True
        '
        'chkNHDplus2_Catchment
        '
        Me.chkNHDplus2_Catchment.AutoSize = True
        Me.chkNHDplus2_Catchment.Location = New System.Drawing.Point(205, 19)
        Me.chkNHDplus2_Catchment.Name = "chkNHDplus2_Catchment"
        Me.chkNHDplus2_Catchment.Size = New System.Drawing.Size(82, 17)
        Me.chkNHDplus2_Catchment.TabIndex = 17
        Me.chkNHDplus2_Catchment.Text = "Catchments"
        Me.chkNHDplus2_Catchment.UseVisualStyleBackColor = True
        '
        'grpSoils
        '
        Me.grpSoils.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSoils.Controls.Add(Me.chkSSURGO)
        Me.grpSoils.Location = New System.Drawing.Point(12, 714)
        Me.grpSoils.Name = "grpSoils"
        Me.grpSoils.Size = New System.Drawing.Size(487, 44)
        Me.grpSoils.TabIndex = 52
        Me.grpSoils.TabStop = False
        Me.grpSoils.Text = "USDA NRCS Soils"
        Me.grpSoils.Visible = False
        '
        'chkSSURGO
        '
        Me.chkSSURGO.AutoSize = True
        Me.chkSSURGO.Location = New System.Drawing.Point(6, 19)
        Me.chkSSURGO.Name = "chkSSURGO"
        Me.chkSSURGO.Size = New System.Drawing.Size(72, 17)
        Me.chkSSURGO.TabIndex = 4
        Me.chkSSURGO.Text = "SSURGO"
        Me.ToolTip1.SetToolTip(Me.chkSSURGO, "SSURGO Data from USDA Soil Data Mart")
        Me.chkSSURGO.UseVisualStyleBackColor = True
        '
        'frmDownload
        '
        Me.AcceptButton = Me.btnDownload
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(511, 810)
        Me.Controls.Add(Me.grpSoils)
        Me.Controls.Add(Me.grpNHDplus2)
        Me.Controls.Add(Me.grpNCDC)
        Me.Controls.Add(Me.chkGetNewest)
        Me.Controls.Add(Me.grpNWISStations_GW)
        Me.Controls.Add(Me.grpNWIS_GW)
        Me.Controls.Add(Me.grpNLDAS)
        Me.Controls.Add(Me.grpNWISStations)
        Me.Controls.Add(Me.grpUSGS_Seamless)
        Me.Controls.Add(Me.grpSTORET)
        Me.Controls.Add(Me.chkClip)
        Me.Controls.Add(Me.lblRegion)
        Me.Controls.Add(Me.cboRegion)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.grpNWIS)
        Me.Controls.Add(Me.grpBASINS)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.chkCacheOnly)
        Me.Controls.Add(Me.chkMerge)
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
        Me.grpUSGS_Seamless.ResumeLayout(False)
        Me.grpUSGS_Seamless.PerformLayout()
        Me.grpSTORET.ResumeLayout(False)
        Me.grpSTORET.PerformLayout()
        Me.grpNWISStations.ResumeLayout(False)
        Me.grpNWISStations.PerformLayout()
        Me.grpNLDAS.ResumeLayout(False)
        Me.grpNLDAS.PerformLayout()
        Me.grpNWISStations_GW.ResumeLayout(False)
        Me.grpNWISStations_GW.PerformLayout()
        Me.grpNWIS_GW.ResumeLayout(False)
        Me.grpNWIS_GW.PerformLayout()
        Me.panelNWISnoStations_GW.ResumeLayout(False)
        Me.panelNWISnoStations_GW.PerformLayout()
        Me.grpNCDC.ResumeLayout(False)
        Me.grpNCDC.PerformLayout()
        Me.grpNHDplus2.ResumeLayout(False)
        Me.grpNHDplus2.PerformLayout()
        Me.grpSoils.ResumeLayout(False)
        Me.grpSoils.PerformLayout()
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
    Friend WithEvents chkBASINS_MetStations As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISMeasurements As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDailyDischarge As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISIdaDischarge As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDailyGW As System.Windows.Forms.CheckBox
    Friend WithEvents grpUSGS_Seamless As System.Windows.Forms.GroupBox
    Friend WithEvents chkUSGS_Seamless_NLCD2006_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents cboRegion As System.Windows.Forms.ComboBox
    Friend WithEvents lblRegion As System.Windows.Forms.Label
    Friend WithEvents chkClip As System.Windows.Forms.CheckBox
    Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpSTORET As System.Windows.Forms.GroupBox
    Friend WithEvents chkSTORET_Stations As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2006_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2001_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISWQ As System.Windows.Forms.CheckBox
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents chkUSGS_Seamless_NLCD2001_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWISStations As System.Windows.Forms.GroupBox
    Friend WithEvents chkNWISStations_qw As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_measurement As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_discharge As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw_daily As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_303d As System.Windows.Forms.CheckBox
    Friend WithEvents chkSTORET_Results As System.Windows.Forms.CheckBox
    Friend WithEvents panelNWISnoStations As System.Windows.Forms.Panel
    Friend WithEvents lblNWISnoStations As System.Windows.Forms.Label
    Friend WithEvents chkCacheOnly As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_MetData As System.Windows.Forms.CheckBox
    Friend WithEvents grpNLDAS As System.Windows.Forms.GroupBox
    Friend WithEvents chkNLDAS_GetNLDASParameter As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLDAS_GetNLDASGrid As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWISStations_GW As System.Windows.Forms.GroupBox
    Friend WithEvents chkNWISStations_gw_daily_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw_periodic_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_discharge_GW As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWIS_GW As System.Windows.Forms.GroupBox
    Friend WithEvents chkNWIS_GetNWISDailyDischarge_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDailyGW_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISPeriodicGW_GW As System.Windows.Forms.CheckBox
    Friend WithEvents panelNWISnoStations_GW As System.Windows.Forms.Panel
    Friend WithEvents lblNWISnoStations_GW As System.Windows.Forms.Label
    Friend WithEvents chkNWIS_GetNWISIdaDischarge_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw_periodic As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISPeriodicGW As System.Windows.Forms.CheckBox
    Friend WithEvents lblMinCount As System.Windows.Forms.Label
    Friend WithEvents txtMinCount_GW As System.Windows.Forms.TextBox
    Friend WithEvents chkNWISStations_precipitation_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISPrecipitation_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2019_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2019_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2011_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2011_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents chkGetNewest As System.Windows.Forms.CheckBox
    Friend WithEvents grpNCDC As System.Windows.Forms.GroupBox
    Friend WithEvents chkNCDC_MetData As System.Windows.Forms.CheckBox
    Friend WithEvents chkNCDC_MetStations As System.Windows.Forms.CheckBox
    Friend WithEvents lblTimeZone As System.Windows.Forms.Label
    Friend WithEvents txtTimeZone As System.Windows.Forms.TextBox
    Friend WithEvents grpNHDplus2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkNHDplus2_hydrography As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus2_elev_cm As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus2_All As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus2_Catchment As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2013_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2004_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2008_LandCover As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2016_Impervious As Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2016_LandCover As Windows.Forms.CheckBox
    Friend WithEvents grpSoils As Windows.Forms.GroupBox
    Friend WithEvents chkSSURGO As Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2016_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkUSGS_Seamless_NLCD2016_LandCover As System.Windows.Forms.CheckBox
End Class
