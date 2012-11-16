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
        Me.chkBASINS_MetData = New System.Windows.Forms.CheckBox
        Me.chkBASINS_303d = New System.Windows.Forms.CheckBox
        Me.chkBASINS_MetStations = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NHD = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NED = New System.Windows.Forms.CheckBox
        Me.chkBASINS_LSTORET = New System.Windows.Forms.CheckBox
        Me.chkBASINS_GIRAS = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEMG = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEM = New System.Windows.Forms.CheckBox
        Me.chkBASINS_Census = New System.Windows.Forms.CheckBox
        Me.grpNWIS = New System.Windows.Forms.GroupBox
        Me.chkNWIS_GetNWISPeriodicGW = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISWQ = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISMeasurements = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISDailyDischarge = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISIdaDischarge = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISDailyGW = New System.Windows.Forms.CheckBox
        Me.panelNWISnoStations = New System.Windows.Forms.Panel
        Me.lblNWISnoStations = New System.Windows.Forms.Label
        Me.btnHelp = New System.Windows.Forms.Button
        Me.grpNLCD2001 = New System.Windows.Forms.GroupBox
        Me.chkNLCD2001_NED30 = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_1992 = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_Canopy = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_Impervious = New System.Windows.Forms.CheckBox
        Me.chkNLCD2001_LandCover = New System.Windows.Forms.CheckBox
        Me.grpNHDplus = New System.Windows.Forms.GroupBox
        Me.chkNHDplus_hydrography = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_elev_cm = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_All = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Catchment = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkNWISStations_qw = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_measurement = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_discharge = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_gw_daily = New System.Windows.Forms.CheckBox
        Me.chkClip = New System.Windows.Forms.CheckBox
        Me.chkMerge = New System.Windows.Forms.CheckBox
        Me.chkNLDAS_GetNLDASParameter = New System.Windows.Forms.CheckBox
        Me.chkNLDAS_GetNLDASGrid = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_gw_daily_GW = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_gw_periodic_GW = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_discharge_GW = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISPeriodicGW_GW = New System.Windows.Forms.CheckBox
        Me.chkNWISStations_gw_periodic = New System.Windows.Forms.CheckBox
        Me.txtMinCount_GW = New System.Windows.Forms.TextBox
        Me.chkNWISStations_precipitation_GW = New System.Windows.Forms.CheckBox
        Me.btnDownload = New System.Windows.Forms.Button
        Me.cboRegion = New System.Windows.Forms.ComboBox
        Me.lblRegion = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpSTORET = New System.Windows.Forms.GroupBox
        Me.chkSTORET_Results = New System.Windows.Forms.CheckBox
        Me.chkSTORET_Stations = New System.Windows.Forms.CheckBox
        Me.grpNWISStations = New System.Windows.Forms.GroupBox
        Me.chkCacheOnly = New System.Windows.Forms.CheckBox
        Me.grpNLDAS = New System.Windows.Forms.GroupBox
        Me.grpNWISStations_GW = New System.Windows.Forms.GroupBox
        Me.lblMinCount = New System.Windows.Forms.Label
        Me.grpNWIS_GW = New System.Windows.Forms.GroupBox
        Me.chkNWIS_GetNWISDailyDischarge_GW = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISIdaDischarge_GW = New System.Windows.Forms.CheckBox
        Me.chkNWIS_GetNWISDailyGW_GW = New System.Windows.Forms.CheckBox
        Me.panelNWISnoStations_GW = New System.Windows.Forms.Panel
        Me.chkNWIS_GetNWISPrecipitation_GW = New System.Windows.Forms.CheckBox
        Me.lblNWISnoStations_GW = New System.Windows.Forms.Label
        Me.grpBASINS.SuspendLayout()
        Me.grpNWIS.SuspendLayout()
        Me.panelNWISnoStations.SuspendLayout()
        Me.grpNLCD2001.SuspendLayout()
        Me.grpNHDplus.SuspendLayout()
        Me.grpSTORET.SuspendLayout()
        Me.grpNWISStations.SuspendLayout()
        Me.grpNLDAS.SuspendLayout()
        Me.grpNWISStations_GW.SuspendLayout()
        Me.grpNWIS_GW.SuspendLayout()
        Me.panelNWISnoStations_GW.SuspendLayout()
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
        Me.grpBASINS.Location = New System.Drawing.Point(11, 169)
        Me.grpBASINS.Name = "grpBASINS"
        Me.grpBASINS.Size = New System.Drawing.Size(460, 65)
        Me.grpBASINS.TabIndex = 0
        Me.grpBASINS.TabStop = False
        Me.grpBASINS.Text = "BASINS"
        Me.grpBASINS.Visible = False
        '
        'chkBASINS_MetData
        '
        Me.chkBASINS_MetData.AutoSize = True
        Me.chkBASINS_MetData.Location = New System.Drawing.Point(356, 42)
        Me.chkBASINS_MetData.Name = "chkBASINS_MetData"
        Me.chkBASINS_MetData.Size = New System.Drawing.Size(70, 17)
        Me.chkBASINS_MetData.TabIndex = 11
        Me.chkBASINS_MetData.Text = "Met Data"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_MetData, "Weather station data in WDM format")
        Me.chkBASINS_MetData.UseVisualStyleBackColor = True
        '
        'chkBASINS_303d
        '
        Me.chkBASINS_303d.AutoSize = True
        Me.chkBASINS_303d.Location = New System.Drawing.Point(289, 42)
        Me.chkBASINS_303d.Name = "chkBASINS_303d"
        Me.chkBASINS_303d.Size = New System.Drawing.Size(56, 17)
        Me.chkBASINS_303d.TabIndex = 9
        Me.chkBASINS_303d.Text = "303(d)"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_303d, "EPA Listed Impaired Waters")
        Me.chkBASINS_303d.UseVisualStyleBackColor = True
        '
        'chkBASINS_MetStations
        '
        Me.chkBASINS_MetStations.AutoSize = True
        Me.chkBASINS_MetStations.Location = New System.Drawing.Point(356, 19)
        Me.chkBASINS_MetStations.Name = "chkBASINS_MetStations"
        Me.chkBASINS_MetStations.Size = New System.Drawing.Size(85, 17)
        Me.chkBASINS_MetStations.TabIndex = 10
        Me.chkBASINS_MetStations.Text = "Met Stations"
        Me.ToolTip1.SetToolTip(Me.chkBASINS_MetStations, "Weather station locations")
        Me.chkBASINS_MetStations.UseVisualStyleBackColor = True
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
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISPeriodicGW)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISWQ)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISMeasurements)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISDailyDischarge)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISIdaDischarge)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_GetNWISDailyGW)
        Me.grpNWIS.Controls.Add(Me.panelNWISnoStations)
        Me.grpNWIS.Location = New System.Drawing.Point(11, 361)
        Me.grpNWIS.Name = "grpNWIS"
        Me.grpNWIS.Size = New System.Drawing.Size(460, 65)
        Me.grpNWIS.TabIndex = 26
        Me.grpNWIS.TabStop = False
        Me.grpNWIS.Text = "Data Values from US Geological Survey National Water Information System"
        Me.grpNWIS.Visible = False
        '
        'chkNWIS_GetNWISPeriodicGW
        '
        Me.chkNWIS_GetNWISPeriodicGW.AutoSize = True
        Me.chkNWIS_GetNWISPeriodicGW.Location = New System.Drawing.Point(386, 19)
        Me.chkNWIS_GetNWISPeriodicGW.Name = "chkNWIS_GetNWISPeriodicGW"
        Me.chkNWIS_GetNWISPeriodicGW.Size = New System.Drawing.Size(64, 17)
        Me.chkNWIS_GetNWISPeriodicGW.TabIndex = 32
        Me.chkNWIS_GetNWISPeriodicGW.Text = "Periodic"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISPeriodicGW, "Periodic Groundwater Station Point Layer")
        Me.chkNWIS_GetNWISPeriodicGW.UseVisualStyleBackColor = True
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
        'chkNWIS_GetNWISMeasurements
        '
        Me.chkNWIS_GetNWISMeasurements.AutoSize = True
        Me.chkNWIS_GetNWISMeasurements.Enabled = False
        Me.chkNWIS_GetNWISMeasurements.Location = New System.Drawing.Point(205, 19)
        Me.chkNWIS_GetNWISMeasurements.Name = "chkNWIS_GetNWISMeasurements"
        Me.chkNWIS_GetNWISMeasurements.Size = New System.Drawing.Size(95, 17)
        Me.chkNWIS_GetNWISMeasurements.TabIndex = 29
        Me.chkNWIS_GetNWISMeasurements.Text = "Measurements"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISMeasurements, "Periodic Manual Streamflow Measurements")
        Me.chkNWIS_GetNWISMeasurements.UseVisualStyleBackColor = True
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
        'chkNWIS_GetNWISDailyGW
        '
        Me.chkNWIS_GetNWISDailyGW.AutoSize = True
        Me.chkNWIS_GetNWISDailyGW.Enabled = False
        Me.chkNWIS_GetNWISDailyGW.Location = New System.Drawing.Point(309, 19)
        Me.chkNWIS_GetNWISDailyGW.Name = "chkNWIS_GetNWISDailyGW"
        Me.chkNWIS_GetNWISDailyGW.Size = New System.Drawing.Size(71, 17)
        Me.chkNWIS_GetNWISDailyGW.TabIndex = 30
        Me.chkNWIS_GetNWISDailyGW.Text = "Daily GW"
        Me.chkNWIS_GetNWISDailyGW.UseVisualStyleBackColor = True
        '
        'panelNWISnoStations
        '
        Me.panelNWISnoStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelNWISnoStations.Controls.Add(Me.lblNWISnoStations)
        Me.panelNWISnoStations.Location = New System.Drawing.Point(6, 19)
        Me.panelNWISnoStations.Name = "panelNWISnoStations"
        Me.panelNWISnoStations.Size = New System.Drawing.Size(448, 43)
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
        'btnHelp
        '
        Me.btnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHelp.Location = New System.Drawing.Point(266, 576)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(59, 23)
        Me.btnHelp.TabIndex = 45
        Me.btnHelp.Text = "Help"
        Me.ToolTip1.SetToolTip(Me.btnHelp, "Launch old data download tool")
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'grpNLCD2001
        '
        Me.grpNLCD2001.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_NED30)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_1992)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_Canopy)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_Impervious)
        Me.grpNLCD2001.Controls.Add(Me.chkNLCD2001_LandCover)
        Me.grpNLCD2001.Location = New System.Drawing.Point(11, 432)
        Me.grpNLCD2001.Name = "grpNLCD2001"
        Me.grpNLCD2001.Size = New System.Drawing.Size(460, 42)
        Me.grpNLCD2001.TabIndex = 30
        Me.grpNLCD2001.TabStop = False
        Me.grpNLCD2001.Text = "National Land Cover Data 2001"
        Me.grpNLCD2001.Visible = False
        '
        'chkNLCD2001_NED30
        '
        Me.chkNLCD2001_NED30.AutoSize = True
        Me.chkNLCD2001_NED30.Location = New System.Drawing.Point(6, 42)
        Me.chkNLCD2001_NED30.Name = "chkNLCD2001_NED30"
        Me.chkNLCD2001_NED30.Size = New System.Drawing.Size(72, 17)
        Me.chkNLCD2001_NED30.TabIndex = 35
        Me.chkNLCD2001_NED30.Text = "NED 30m"
        Me.chkNLCD2001_NED30.UseVisualStyleBackColor = True
        Me.chkNLCD2001_NED30.Visible = False
        '
        'chkNLCD2001_1992
        '
        Me.chkNLCD2001_1992.AutoSize = True
        Me.chkNLCD2001_1992.Location = New System.Drawing.Point(289, 19)
        Me.chkNLCD2001_1992.Name = "chkNLCD2001_1992"
        Me.chkNLCD2001_1992.Size = New System.Drawing.Size(108, 17)
        Me.chkNLCD2001_1992.TabIndex = 34
        Me.chkNLCD2001_1992.Text = "1992 Land Cover"
        Me.chkNLCD2001_1992.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_Canopy
        '
        Me.chkNLCD2001_Canopy.AutoSize = True
        Me.chkNLCD2001_Canopy.Location = New System.Drawing.Point(205, 19)
        Me.chkNLCD2001_Canopy.Name = "chkNLCD2001_Canopy"
        Me.chkNLCD2001_Canopy.Size = New System.Drawing.Size(62, 17)
        Me.chkNLCD2001_Canopy.TabIndex = 33
        Me.chkNLCD2001_Canopy.Text = "Canopy"
        Me.chkNLCD2001_Canopy.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_Impervious
        '
        Me.chkNLCD2001_Impervious.AutoSize = True
        Me.chkNLCD2001_Impervious.Location = New System.Drawing.Point(112, 19)
        Me.chkNLCD2001_Impervious.Name = "chkNLCD2001_Impervious"
        Me.chkNLCD2001_Impervious.Size = New System.Drawing.Size(77, 17)
        Me.chkNLCD2001_Impervious.TabIndex = 32
        Me.chkNLCD2001_Impervious.Text = "Impervious"
        Me.chkNLCD2001_Impervious.UseVisualStyleBackColor = True
        '
        'chkNLCD2001_LandCover
        '
        Me.chkNLCD2001_LandCover.AutoSize = True
        Me.chkNLCD2001_LandCover.Location = New System.Drawing.Point(6, 19)
        Me.chkNLCD2001_LandCover.Name = "chkNLCD2001_LandCover"
        Me.chkNLCD2001_LandCover.Size = New System.Drawing.Size(81, 17)
        Me.chkNLCD2001_LandCover.TabIndex = 31
        Me.chkNLCD2001_LandCover.Text = "Land Cover"
        Me.chkNLCD2001_LandCover.UseVisualStyleBackColor = True
        '
        'grpNHDplus
        '
        Me.grpNHDplus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_hydrography)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_elev_cm)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_All)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Catchment)
        Me.grpNHDplus.Location = New System.Drawing.Point(11, 240)
        Me.grpNHDplus.Name = "grpNHDplus"
        Me.grpNHDplus.Size = New System.Drawing.Size(460, 67)
        Me.grpNHDplus.TabIndex = 12
        Me.grpNHDplus.TabStop = False
        Me.grpNHDplus.Text = "National Hydrography Dataset Plus"
        Me.grpNHDplus.Visible = False
        '
        'chkNHDplus_hydrography
        '
        Me.chkNHDplus_hydrography.AutoSize = True
        Me.chkNHDplus_hydrography.Location = New System.Drawing.Point(205, 42)
        Me.chkNHDplus_hydrography.Name = "chkNHDplus_hydrography"
        Me.chkNHDplus_hydrography.Size = New System.Drawing.Size(86, 17)
        Me.chkNHDplus_hydrography.TabIndex = 18
        Me.chkNHDplus_hydrography.Text = "Hydrography"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_hydrography, "NHDArea, NHDFlowline, NHDLine, NHDPoint, NHDWaterbody")
        Me.chkNHDplus_hydrography.UseVisualStyleBackColor = True
        '
        'chkNHDplus_elev_cm
        '
        Me.chkNHDplus_elev_cm.AutoSize = True
        Me.chkNHDplus_elev_cm.Location = New System.Drawing.Point(6, 42)
        Me.chkNHDplus_elev_cm.Name = "chkNHDplus_elev_cm"
        Me.chkNHDplus_elev_cm.Size = New System.Drawing.Size(92, 17)
        Me.chkNHDplus_elev_cm.TabIndex = 14
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
        Me.chkNHDplus_All.TabIndex = 13
        Me.chkNHDplus_All.Text = "All"
        Me.chkNHDplus_All.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Catchment
        '
        Me.chkNHDplus_Catchment.AutoSize = True
        Me.chkNHDplus_Catchment.Location = New System.Drawing.Point(205, 19)
        Me.chkNHDplus_Catchment.Name = "chkNHDplus_Catchment"
        Me.chkNHDplus_Catchment.Size = New System.Drawing.Size(82, 17)
        Me.chkNHDplus_Catchment.TabIndex = 17
        Me.chkNHDplus_Catchment.Text = "Catchments"
        Me.chkNHDplus_Catchment.UseVisualStyleBackColor = True
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
        Me.chkNWISStations_measurement.Location = New System.Drawing.Point(205, 19)
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
        Me.chkNWISStations_gw_daily.Location = New System.Drawing.Point(309, 19)
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
        Me.chkClip.Location = New System.Drawing.Point(124, 580)
        Me.chkClip.Name = "chkClip"
        Me.chkClip.Size = New System.Drawing.Size(92, 17)
        Me.chkClip.TabIndex = 43
        Me.chkClip.Text = "Clip to Region"
        Me.ToolTip1.SetToolTip(Me.chkClip, "Discard additional data if a larger area was retrieved than was requested")
        Me.chkClip.UseVisualStyleBackColor = True
        '
        'chkMerge
        '
        Me.chkMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Location = New System.Drawing.Point(18, 580)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(56, 17)
        Me.chkMerge.TabIndex = 42
        Me.chkMerge.Text = "Merge"
        Me.ToolTip1.SetToolTip(Me.chkMerge, "Merge parts of the same dataset from different areas to form one layer")
        Me.chkMerge.UseVisualStyleBackColor = True
        '
        'chkNLDAS_GetNLDASParameter
        '
        Me.chkNLDAS_GetNLDASParameter.AutoSize = True
        Me.chkNLDAS_GetNLDASParameter.Enabled = False
        Me.chkNLDAS_GetNLDASParameter.Location = New System.Drawing.Point(112, 19)
        Me.chkNLDAS_GetNLDASParameter.Name = "chkNLDAS_GetNLDASParameter"
        Me.chkNLDAS_GetNLDASParameter.Size = New System.Drawing.Size(262, 17)
        Me.chkNLDAS_GetNLDASParameter.TabIndex = 41
        Me.chkNLDAS_GetNLDASParameter.Text = "Precipitation (available after grid selection on map)"
        Me.ToolTip1.SetToolTip(Me.chkNLDAS_GetNLDASParameter, "Hourly Precipitation for selected NLDAS grids")
        Me.chkNLDAS_GetNLDASParameter.UseVisualStyleBackColor = True
        '
        'chkNLDAS_GetNLDASGrid
        '
        Me.chkNLDAS_GetNLDASGrid.AutoSize = True
        Me.chkNLDAS_GetNLDASGrid.Location = New System.Drawing.Point(6, 19)
        Me.chkNLDAS_GetNLDASGrid.Name = "chkNLDAS_GetNLDASGrid"
        Me.chkNLDAS_GetNLDASGrid.Size = New System.Drawing.Size(45, 17)
        Me.chkNLDAS_GetNLDASGrid.TabIndex = 40
        Me.chkNLDAS_GetNLDASGrid.Text = "Grid"
        Me.ToolTip1.SetToolTip(Me.chkNLDAS_GetNLDASGrid, "NLDAS Grid Layer")
        Me.chkNLDAS_GetNLDASGrid.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_daily_GW
        '
        Me.chkNWISStations_gw_daily_GW.AutoSize = True
        Me.chkNWISStations_gw_daily_GW.Location = New System.Drawing.Point(12, 19)
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
        Me.chkNWISStations_gw_periodic_GW.Location = New System.Drawing.Point(131, 19)
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
        'chkNWIS_GetNWISPeriodicGW_GW
        '
        Me.chkNWIS_GetNWISPeriodicGW_GW.AutoSize = True
        Me.chkNWIS_GetNWISPeriodicGW_GW.Enabled = False
        Me.chkNWIS_GetNWISPeriodicGW_GW.Location = New System.Drawing.Point(132, 19)
        Me.chkNWIS_GetNWISPeriodicGW_GW.Name = "chkNWIS_GetNWISPeriodicGW_GW"
        Me.chkNWIS_GetNWISPeriodicGW_GW.Size = New System.Drawing.Size(128, 17)
        Me.chkNWIS_GetNWISPeriodicGW_GW.TabIndex = 29
        Me.chkNWIS_GetNWISPeriodicGW_GW.Text = "Periodic Groundwater"
        Me.ToolTip1.SetToolTip(Me.chkNWIS_GetNWISPeriodicGW_GW, "Periodic Groundwater")
        Me.chkNWIS_GetNWISPeriodicGW_GW.UseVisualStyleBackColor = True
        '
        'chkNWISStations_gw_periodic
        '
        Me.chkNWISStations_gw_periodic.AutoSize = True
        Me.chkNWISStations_gw_periodic.Location = New System.Drawing.Point(386, 19)
        Me.chkNWISStations_gw_periodic.Name = "chkNWISStations_gw_periodic"
        Me.chkNWISStations_gw_periodic.Size = New System.Drawing.Size(64, 17)
        Me.chkNWISStations_gw_periodic.TabIndex = 26
        Me.chkNWISStations_gw_periodic.Text = "Periodic"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_gw_periodic, "Periodic Groundwater Station Point Layer")
        Me.chkNWISStations_gw_periodic.UseVisualStyleBackColor = True
        '
        'txtMinCount_GW
        '
        Me.txtMinCount_GW.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMinCount_GW.Location = New System.Drawing.Point(429, 16)
        Me.txtMinCount_GW.Name = "txtMinCount_GW"
        Me.txtMinCount_GW.Size = New System.Drawing.Size(25, 20)
        Me.txtMinCount_GW.TabIndex = 26
        Me.txtMinCount_GW.Text = "10"
        Me.ToolTip1.SetToolTip(Me.txtMinCount_GW, "Minimum number of values - omit stations with fewer")
        '
        'chkNWISStations_precipitation_GW
        '
        Me.chkNWISStations_precipitation_GW.AutoSize = True
        Me.chkNWISStations_precipitation_GW.Location = New System.Drawing.Point(12, 42)
        Me.chkNWISStations_precipitation_GW.Name = "chkNWISStations_precipitation_GW"
        Me.chkNWISStations_precipitation_GW.Size = New System.Drawing.Size(84, 17)
        Me.chkNWISStations_precipitation_GW.TabIndex = 28
        Me.chkNWISStations_precipitation_GW.Text = "Precipitation"
        Me.ToolTip1.SetToolTip(Me.chkNWISStations_precipitation_GW, "Precipitation Station Point Layer")
        Me.chkNWISStations_precipitation_GW.UseVisualStyleBackColor = True
        '
        'btnDownload
        '
        Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDownload.Location = New System.Drawing.Point(396, 576)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 47
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'cboRegion
        '
        Me.cboRegion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRegion.FormattingEnabled = True
        Me.cboRegion.Location = New System.Drawing.Point(124, 12)
        Me.cboRegion.Name = "cboRegion"
        Me.cboRegion.Size = New System.Drawing.Size(348, 21)
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
        Me.btnCancel.Location = New System.Drawing.Point(331, 576)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(59, 23)
        Me.btnCancel.TabIndex = 46
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpSTORET
        '
        Me.grpSTORET.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Results)
        Me.grpSTORET.Controls.Add(Me.chkSTORET_Stations)
        Me.grpSTORET.Location = New System.Drawing.Point(11, 480)
        Me.grpSTORET.Name = "grpSTORET"
        Me.grpSTORET.Size = New System.Drawing.Size(460, 42)
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
        Me.chkSTORET_Results.Size = New System.Drawing.Size(276, 17)
        Me.chkSTORET_Results.TabIndex = 38
        Me.chkSTORET_Results.Text = "Results (available after Stations are selected on map)"
        Me.chkSTORET_Results.UseVisualStyleBackColor = True
        '
        'chkSTORET_Stations
        '
        Me.chkSTORET_Stations.AutoSize = True
        Me.chkSTORET_Stations.Location = New System.Drawing.Point(6, 19)
        Me.chkSTORET_Stations.Name = "chkSTORET_Stations"
        Me.chkSTORET_Stations.Size = New System.Drawing.Size(64, 17)
        Me.chkSTORET_Stations.TabIndex = 37
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
        Me.grpNWISStations.Location = New System.Drawing.Point(11, 313)
        Me.grpNWISStations.Name = "grpNWISStations"
        Me.grpNWISStations.Size = New System.Drawing.Size(460, 42)
        Me.grpNWISStations.TabIndex = 21
        Me.grpNWISStations.TabStop = False
        Me.grpNWISStations.Text = "Station Locations from US Geological Survey National Water Information System"
        Me.grpNWISStations.Visible = False
        '
        'chkCacheOnly
        '
        Me.chkCacheOnly.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkCacheOnly.AutoSize = True
        Me.chkCacheOnly.Location = New System.Drawing.Point(222, 580)
        Me.chkCacheOnly.Name = "chkCacheOnly"
        Me.chkCacheOnly.Size = New System.Drawing.Size(81, 17)
        Me.chkCacheOnly.TabIndex = 44
        Me.chkCacheOnly.Text = "Cache Only"
        Me.chkCacheOnly.UseVisualStyleBackColor = True
        Me.chkCacheOnly.Visible = False
        '
        'grpNLDAS
        '
        Me.grpNLDAS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLDAS.Controls.Add(Me.chkNLDAS_GetNLDASParameter)
        Me.grpNLDAS.Controls.Add(Me.chkNLDAS_GetNLDASGrid)
        Me.grpNLDAS.Location = New System.Drawing.Point(11, 528)
        Me.grpNLDAS.Name = "grpNLDAS"
        Me.grpNLDAS.Size = New System.Drawing.Size(460, 42)
        Me.grpNLDAS.TabIndex = 39
        Me.grpNLDAS.TabStop = False
        Me.grpNLDAS.Text = "North American Land Data Assimilation System"
        Me.grpNLDAS.Visible = False
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
        Me.grpNWISStations_GW.Location = New System.Drawing.Point(12, 39)
        Me.grpNWISStations_GW.Name = "grpNWISStations_GW"
        Me.grpNWISStations_GW.Size = New System.Drawing.Size(460, 69)
        Me.grpNWISStations_GW.TabIndex = 48
        Me.grpNWISStations_GW.TabStop = False
        Me.grpNWISStations_GW.Text = "Station Locations from US Geological Survey National Water Information System"
        Me.grpNWISStations_GW.Visible = False
        '
        'lblMinCount
        '
        Me.lblMinCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMinCount.AutoSize = True
        Me.lblMinCount.Location = New System.Drawing.Point(369, 20)
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
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISIdaDischarge_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISDailyGW_GW)
        Me.grpNWIS_GW.Controls.Add(Me.chkNWIS_GetNWISPeriodicGW_GW)
        Me.grpNWIS_GW.Controls.Add(Me.panelNWISnoStations_GW)
        Me.grpNWIS_GW.Location = New System.Drawing.Point(11, 98)
        Me.grpNWIS_GW.Name = "grpNWIS_GW"
        Me.grpNWIS_GW.Size = New System.Drawing.Size(460, 65)
        Me.grpNWIS_GW.TabIndex = 32
        Me.grpNWIS_GW.TabStop = False
        Me.grpNWIS_GW.Text = "Data Values from US Geological Survey National Water Information System"
        Me.grpNWIS_GW.Visible = False
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
        Me.chkNWIS_GetNWISIdaDischarge_GW.Location = New System.Drawing.Point(290, 42)
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
        Me.chkNWIS_GetNWISDailyGW_GW.Location = New System.Drawing.Point(13, 19)
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
        Me.panelNWISnoStations_GW.Size = New System.Drawing.Size(448, 43)
        Me.panelNWISnoStations_GW.TabIndex = 28
        Me.panelNWISnoStations_GW.Visible = False
        '
        'chkNWIS_GetNWISPrecipitation_GW
        '
        Me.chkNWIS_GetNWISPrecipitation_GW.AutoSize = True
        Me.chkNWIS_GetNWISPrecipitation_GW.Location = New System.Drawing.Point(13, 39)
        Me.chkNWIS_GetNWISPrecipitation_GW.Name = "chkNWIS_GetNWISPrecipitation_GW"
        Me.chkNWIS_GetNWISPrecipitation_GW.Size = New System.Drawing.Size(84, 17)
        Me.chkNWIS_GetNWISPrecipitation_GW.TabIndex = 1
        Me.chkNWIS_GetNWISPrecipitation_GW.Text = "Precipitation"
        Me.chkNWIS_GetNWISPrecipitation_GW.UseVisualStyleBackColor = True
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
        'frmDownload
        '
        Me.AcceptButton = Me.btnDownload
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(484, 611)
        Me.Controls.Add(Me.grpNWISStations_GW)
        Me.Controls.Add(Me.grpNWIS_GW)
        Me.Controls.Add(Me.grpNLDAS)
        Me.Controls.Add(Me.grpNWISStations)
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
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.chkCacheOnly)
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
        Me.grpNLCD2001.ResumeLayout(False)
        Me.grpNLCD2001.PerformLayout()
        Me.grpNHDplus.ResumeLayout(False)
        Me.grpNHDplus.PerformLayout()
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
    Friend WithEvents chkNHDplus_elev_cm As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Catchment As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_hydrography As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD2001_Impervious As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD2001_Canopy As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISWQ As System.Windows.Forms.CheckBox
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents chkNLCD2001_1992 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWISStations As System.Windows.Forms.GroupBox
    Friend WithEvents chkNWISStations_qw As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_measurement As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_discharge As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw_daily As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_303d As System.Windows.Forms.CheckBox
    Friend WithEvents chkSTORET_Results As System.Windows.Forms.CheckBox
    Friend WithEvents panelNWISnoStations As System.Windows.Forms.Panel
    Friend WithEvents lblNWISnoStations As System.Windows.Forms.Label
    Friend WithEvents chkNLCD2001_NED30 As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkNWIS_GetNWISPeriodicGW_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDailyDischarge_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISDailyGW_GW As System.Windows.Forms.CheckBox
    Friend WithEvents panelNWISnoStations_GW As System.Windows.Forms.Panel
    Friend WithEvents lblNWISnoStations_GW As System.Windows.Forms.Label
    Friend WithEvents chkNWIS_GetNWISIdaDischarge_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWISStations_gw_periodic As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISPeriodicGW As System.Windows.Forms.CheckBox
    Friend WithEvents lblMinCount As System.Windows.Forms.Label
    Friend WithEvents txtMinCount_GW As System.Windows.Forms.TextBox
    Friend WithEvents chkNWISStations_precipitation_GW As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_GetNWISPrecipitation_GW As System.Windows.Forms.CheckBox
End Class
