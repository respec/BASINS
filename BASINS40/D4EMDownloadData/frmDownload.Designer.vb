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
        Me.chkBASINS_Met = New System.Windows.Forms.CheckBox
        Me.chkBASINS_PCS3 = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NHD = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NED = New System.Windows.Forms.CheckBox
        Me.chkBASINS_LSTORET = New System.Windows.Forms.CheckBox
        Me.chkBASINS_GIRAS = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEMG = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEM = New System.Windows.Forms.CheckBox
        Me.chkBASINS_Census = New System.Windows.Forms.CheckBox
        Me.grpNWIS = New System.Windows.Forms.GroupBox
        Me.chkNWIS_Measurements = New System.Windows.Forms.CheckBox
        Me.chkNWIS_WQ = New System.Windows.Forms.CheckBox
        Me.chkNWIS_DailyFlow = New System.Windows.Forms.CheckBox
        Me.chkTerraServer_Urban = New System.Windows.Forms.CheckBox
        Me.grpTerraServer = New System.Windows.Forms.GroupBox
        Me.chkTerraServer_DRG = New System.Windows.Forms.CheckBox
        Me.chkTerraServer_DOQ = New System.Windows.Forms.CheckBox
        Me.chkNLCD_1992 = New System.Windows.Forms.CheckBox
        Me.grpNLCD = New System.Windows.Forms.GroupBox
        Me.chkNLCD_2001 = New System.Windows.Forms.CheckBox
        Me.grpNHDplus = New System.Windows.Forms.GroupBox
        Me.chkNHDplus_Subbasin = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Area = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Line = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Point = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_GageEvents = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Waterbody = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Flowline = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_CatchmentGrid = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_FDR = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_Elevation = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_CatchmentShape = New System.Windows.Forms.CheckBox
        Me.chkNHDplus_FAC = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDownload = New System.Windows.Forms.Button
        Me.cboAOI = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpBASINS.SuspendLayout()
        Me.grpNWIS.SuspendLayout()
        Me.grpTerraServer.SuspendLayout()
        Me.grpNLCD.SuspendLayout()
        Me.grpNHDplus.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpBASINS
        '
        Me.grpBASINS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Met)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_PCS3)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NHD)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NED)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_LSTORET)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_GIRAS)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEMG)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEM)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Census)
        Me.grpBASINS.Location = New System.Drawing.Point(12, 39)
        Me.grpBASINS.Name = "grpBASINS"
        Me.grpBASINS.Size = New System.Drawing.Size(351, 88)
        Me.grpBASINS.TabIndex = 0
        Me.grpBASINS.TabStop = False
        Me.grpBASINS.Text = "Basins"
        '
        'chkBASINS_Met
        '
        Me.chkBASINS_Met.AutoSize = True
        Me.chkBASINS_Met.Location = New System.Drawing.Point(112, 65)
        Me.chkBASINS_Met.Name = "chkBASINS_Met"
        Me.chkBASINS_Met.Size = New System.Drawing.Size(89, 17)
        Me.chkBASINS_Met.TabIndex = 5
        Me.chkBASINS_Met.Text = "Meterological"
        Me.chkBASINS_Met.UseVisualStyleBackColor = True
        '
        'chkBASINS_PCS3
        '
        Me.chkBASINS_PCS3.AutoSize = True
        Me.chkBASINS_PCS3.Location = New System.Drawing.Point(229, 65)
        Me.chkBASINS_PCS3.Name = "chkBASINS_PCS3"
        Me.chkBASINS_PCS3.Size = New System.Drawing.Size(53, 17)
        Me.chkBASINS_PCS3.TabIndex = 8
        Me.chkBASINS_PCS3.Text = "PCS3"
        Me.chkBASINS_PCS3.UseVisualStyleBackColor = True
        '
        'chkBASINS_NHD
        '
        Me.chkBASINS_NHD.AutoSize = True
        Me.chkBASINS_NHD.Location = New System.Drawing.Point(229, 42)
        Me.chkBASINS_NHD.Name = "chkBASINS_NHD"
        Me.chkBASINS_NHD.Size = New System.Drawing.Size(50, 17)
        Me.chkBASINS_NHD.TabIndex = 7
        Me.chkBASINS_NHD.Text = "NHD"
        Me.chkBASINS_NHD.UseVisualStyleBackColor = True
        '
        'chkBASINS_NED
        '
        Me.chkBASINS_NED.AutoSize = True
        Me.chkBASINS_NED.Location = New System.Drawing.Point(229, 19)
        Me.chkBASINS_NED.Name = "chkBASINS_NED"
        Me.chkBASINS_NED.Size = New System.Drawing.Size(49, 17)
        Me.chkBASINS_NED.TabIndex = 6
        Me.chkBASINS_NED.Text = "NED"
        Me.chkBASINS_NED.UseVisualStyleBackColor = True
        '
        'chkBASINS_LSTORET
        '
        Me.chkBASINS_LSTORET.AutoSize = True
        Me.chkBASINS_LSTORET.Location = New System.Drawing.Point(112, 42)
        Me.chkBASINS_LSTORET.Name = "chkBASINS_LSTORET"
        Me.chkBASINS_LSTORET.Size = New System.Drawing.Size(108, 17)
        Me.chkBASINS_LSTORET.TabIndex = 4
        Me.chkBASINS_LSTORET.Text = "Legacy STORET"
        Me.chkBASINS_LSTORET.UseVisualStyleBackColor = True
        '
        'chkBASINS_GIRAS
        '
        Me.chkBASINS_GIRAS.AutoSize = True
        Me.chkBASINS_GIRAS.Location = New System.Drawing.Point(112, 19)
        Me.chkBASINS_GIRAS.Name = "chkBASINS_GIRAS"
        Me.chkBASINS_GIRAS.Size = New System.Drawing.Size(59, 17)
        Me.chkBASINS_GIRAS.TabIndex = 3
        Me.chkBASINS_GIRAS.Text = "GIRAS"
        Me.chkBASINS_GIRAS.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEMG
        '
        Me.chkBASINS_DEMG.AutoSize = True
        Me.chkBASINS_DEMG.Location = New System.Drawing.Point(6, 65)
        Me.chkBASINS_DEMG.Name = "chkBASINS_DEMG"
        Me.chkBASINS_DEMG.Size = New System.Drawing.Size(72, 17)
        Me.chkBASINS_DEMG.TabIndex = 2
        Me.chkBASINS_DEMG.Text = "DEM Grid"
        Me.chkBASINS_DEMG.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEM
        '
        Me.chkBASINS_DEM.AutoSize = True
        Me.chkBASINS_DEM.Location = New System.Drawing.Point(6, 42)
        Me.chkBASINS_DEM.Name = "chkBASINS_DEM"
        Me.chkBASINS_DEM.Size = New System.Drawing.Size(84, 17)
        Me.chkBASINS_DEM.TabIndex = 1
        Me.chkBASINS_DEM.Text = "DEM Shape"
        Me.chkBASINS_DEM.UseVisualStyleBackColor = True
        '
        'chkBASINS_Census
        '
        Me.chkBASINS_Census.AutoSize = True
        Me.chkBASINS_Census.Location = New System.Drawing.Point(6, 19)
        Me.chkBASINS_Census.Name = "chkBASINS_Census"
        Me.chkBASINS_Census.Size = New System.Drawing.Size(61, 17)
        Me.chkBASINS_Census.TabIndex = 0
        Me.chkBASINS_Census.Text = "Census"
        Me.chkBASINS_Census.UseVisualStyleBackColor = True
        '
        'grpNWIS
        '
        Me.grpNWIS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_Measurements)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_WQ)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_DailyFlow)
        Me.grpNWIS.Location = New System.Drawing.Point(12, 133)
        Me.grpNWIS.Name = "grpNWIS"
        Me.grpNWIS.Size = New System.Drawing.Size(351, 42)
        Me.grpNWIS.TabIndex = 1
        Me.grpNWIS.TabStop = False
        Me.grpNWIS.Text = "NWIS"
        '
        'chkNWIS_Measurements
        '
        Me.chkNWIS_Measurements.AutoSize = True
        Me.chkNWIS_Measurements.Location = New System.Drawing.Point(229, 19)
        Me.chkNWIS_Measurements.Name = "chkNWIS_Measurements"
        Me.chkNWIS_Measurements.Size = New System.Drawing.Size(120, 17)
        Me.chkNWIS_Measurements.TabIndex = 11
        Me.chkNWIS_Measurements.Text = "Flow Measurements"
        Me.chkNWIS_Measurements.UseVisualStyleBackColor = True
        '
        'chkNWIS_WQ
        '
        Me.chkNWIS_WQ.AutoSize = True
        Me.chkNWIS_WQ.Location = New System.Drawing.Point(112, 19)
        Me.chkNWIS_WQ.Name = "chkNWIS_WQ"
        Me.chkNWIS_WQ.Size = New System.Drawing.Size(90, 17)
        Me.chkNWIS_WQ.TabIndex = 10
        Me.chkNWIS_WQ.Text = "Water Quality"
        Me.chkNWIS_WQ.UseVisualStyleBackColor = True
        '
        'chkNWIS_DailyFlow
        '
        Me.chkNWIS_DailyFlow.AutoSize = True
        Me.chkNWIS_DailyFlow.Location = New System.Drawing.Point(6, 19)
        Me.chkNWIS_DailyFlow.Name = "chkNWIS_DailyFlow"
        Me.chkNWIS_DailyFlow.Size = New System.Drawing.Size(100, 17)
        Me.chkNWIS_DailyFlow.TabIndex = 9
        Me.chkNWIS_DailyFlow.Text = "Daily Discharge"
        Me.chkNWIS_DailyFlow.UseVisualStyleBackColor = True
        '
        'chkTerraServer_Urban
        '
        Me.chkTerraServer_Urban.AutoSize = True
        Me.chkTerraServer_Urban.Location = New System.Drawing.Point(6, 19)
        Me.chkTerraServer_Urban.Name = "chkTerraServer_Urban"
        Me.chkTerraServer_Urban.Size = New System.Drawing.Size(80, 17)
        Me.chkTerraServer_Urban.TabIndex = 12
        Me.chkTerraServer_Urban.Text = "Urban Area"
        Me.chkTerraServer_Urban.UseVisualStyleBackColor = True
        '
        'grpTerraServer
        '
        Me.grpTerraServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_DRG)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_DOQ)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_Urban)
        Me.grpTerraServer.Location = New System.Drawing.Point(12, 181)
        Me.grpTerraServer.Name = "grpTerraServer"
        Me.grpTerraServer.Size = New System.Drawing.Size(351, 42)
        Me.grpTerraServer.TabIndex = 9
        Me.grpTerraServer.TabStop = False
        Me.grpTerraServer.Text = "TerraServer Images"
        '
        'chkTerraServer_DRG
        '
        Me.chkTerraServer_DRG.AutoSize = True
        Me.chkTerraServer_DRG.Location = New System.Drawing.Point(229, 19)
        Me.chkTerraServer_DRG.Name = "chkTerraServer_DRG"
        Me.chkTerraServer_DRG.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServer_DRG.TabIndex = 14
        Me.chkTerraServer_DRG.Text = "DRG"
        Me.chkTerraServer_DRG.UseVisualStyleBackColor = True
        '
        'chkTerraServer_DOQ
        '
        Me.chkTerraServer_DOQ.AutoSize = True
        Me.chkTerraServer_DOQ.Location = New System.Drawing.Point(112, 19)
        Me.chkTerraServer_DOQ.Name = "chkTerraServer_DOQ"
        Me.chkTerraServer_DOQ.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServer_DOQ.TabIndex = 13
        Me.chkTerraServer_DOQ.Text = "DOQ"
        Me.chkTerraServer_DOQ.UseVisualStyleBackColor = True
        '
        'chkNLCD_1992
        '
        Me.chkNLCD_1992.AutoSize = True
        Me.chkNLCD_1992.Location = New System.Drawing.Point(6, 19)
        Me.chkNLCD_1992.Name = "chkNLCD_1992"
        Me.chkNLCD_1992.Size = New System.Drawing.Size(50, 17)
        Me.chkNLCD_1992.TabIndex = 15
        Me.chkNLCD_1992.Text = "1992"
        Me.chkNLCD_1992.UseVisualStyleBackColor = True
        '
        'grpNLCD
        '
        Me.grpNLCD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLCD.Controls.Add(Me.chkNLCD_2001)
        Me.grpNLCD.Controls.Add(Me.chkNLCD_1992)
        Me.grpNLCD.Location = New System.Drawing.Point(12, 229)
        Me.grpNLCD.Name = "grpNLCD"
        Me.grpNLCD.Size = New System.Drawing.Size(351, 42)
        Me.grpNLCD.TabIndex = 11
        Me.grpNLCD.TabStop = False
        Me.grpNLCD.Text = "NLCD"
        '
        'chkNLCD_2001
        '
        Me.chkNLCD_2001.AutoSize = True
        Me.chkNLCD_2001.Location = New System.Drawing.Point(112, 19)
        Me.chkNLCD_2001.Name = "chkNLCD_2001"
        Me.chkNLCD_2001.Size = New System.Drawing.Size(50, 17)
        Me.chkNLCD_2001.TabIndex = 16
        Me.chkNLCD_2001.Text = "2001"
        Me.chkNLCD_2001.UseVisualStyleBackColor = True
        '
        'grpNHDplus
        '
        Me.grpNHDplus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Subbasin)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Area)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Line)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Point)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_GageEvents)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Waterbody)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Flowline)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_CatchmentGrid)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_FDR)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_Elevation)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_CatchmentShape)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_FAC)
        Me.grpNHDplus.Location = New System.Drawing.Point(12, 277)
        Me.grpNHDplus.Name = "grpNHDplus"
        Me.grpNHDplus.Size = New System.Drawing.Size(351, 111)
        Me.grpNHDplus.TabIndex = 12
        Me.grpNHDplus.TabStop = False
        Me.grpNHDplus.Text = "NHD Plus"
        '
        'chkNHDplus_Subbasin
        '
        Me.chkNHDplus_Subbasin.AutoSize = True
        Me.chkNHDplus_Subbasin.Location = New System.Drawing.Point(229, 19)
        Me.chkNHDplus_Subbasin.Name = "chkNHDplus_Subbasin"
        Me.chkNHDplus_Subbasin.Size = New System.Drawing.Size(70, 17)
        Me.chkNHDplus_Subbasin.TabIndex = 25
        Me.chkNHDplus_Subbasin.Text = "Subbasin"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Subbasin, "Boundaries of 8-digit hydrologic units")
        Me.chkNHDplus_Subbasin.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Area
        '
        Me.chkNHDplus_Area.AutoSize = True
        Me.chkNHDplus_Area.Location = New System.Drawing.Point(229, 88)
        Me.chkNHDplus_Area.Name = "chkNHDplus_Area"
        Me.chkNHDplus_Area.Size = New System.Drawing.Size(48, 17)
        Me.chkNHDplus_Area.TabIndex = 28
        Me.chkNHDplus_Area.Text = "Area"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Area, resources.GetString("chkNHDplus_Area.ToolTip"))
        Me.chkNHDplus_Area.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Line
        '
        Me.chkNHDplus_Line.AutoSize = True
        Me.chkNHDplus_Line.Location = New System.Drawing.Point(229, 65)
        Me.chkNHDplus_Line.Name = "chkNHDplus_Line"
        Me.chkNHDplus_Line.Size = New System.Drawing.Size(46, 17)
        Me.chkNHDplus_Line.TabIndex = 27
        Me.chkNHDplus_Line.Text = "Line"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Line, "Bridge, DamWeir, Flume, Gate, Lock Chamber, Nonearthen Shore, Rapids, Reef, SinkR" & _
                "ise, Tunnel, Wall, Waterfall, Sounding Datum Line, and Special Use Zone Limit")
        Me.chkNHDplus_Line.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Point
        '
        Me.chkNHDplus_Point.AutoSize = True
        Me.chkNHDplus_Point.Location = New System.Drawing.Point(229, 42)
        Me.chkNHDplus_Point.Name = "chkNHDplus_Point"
        Me.chkNHDplus_Point.Size = New System.Drawing.Size(50, 17)
        Me.chkNHDplus_Point.TabIndex = 26
        Me.chkNHDplus_Point.Text = "Point"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Point, "Gate, Lock Chamber, Rapids, Reservoir, Rock, SinkRise, SpringSeep, Water IntakeOu" & _
                "tflow, Waterfall, and Well")
        Me.chkNHDplus_Point.UseVisualStyleBackColor = True
        '
        'chkNHDplus_GageEvents
        '
        Me.chkNHDplus_GageEvents.AutoSize = True
        Me.chkNHDplus_GageEvents.Location = New System.Drawing.Point(6, 88)
        Me.chkNHDplus_GageEvents.Name = "chkNHDplus_GageEvents"
        Me.chkNHDplus_GageEvents.Size = New System.Drawing.Size(88, 17)
        Me.chkNHDplus_GageEvents.TabIndex = 20
        Me.chkNHDplus_GageEvents.Text = "Gage Events"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_GageEvents, "StreamGageEvent shape file contains the physical locations of the USGS stream gag" & _
                "es as well as their location on NHDFlowline features through events linked to re" & _
                "ach codes and measures")
        Me.chkNHDplus_GageEvents.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Waterbody
        '
        Me.chkNHDplus_Waterbody.AutoSize = True
        Me.chkNHDplus_Waterbody.Location = New System.Drawing.Point(6, 42)
        Me.chkNHDplus_Waterbody.Name = "chkNHDplus_Waterbody"
        Me.chkNHDplus_Waterbody.Size = New System.Drawing.Size(78, 17)
        Me.chkNHDplus_Waterbody.TabIndex = 18
        Me.chkNHDplus_Waterbody.Text = "Waterbody"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Waterbody, "Playa, Ice Mass, LakePond, Reservoir, SwampMarsh, and Estuary")
        Me.chkNHDplus_Waterbody.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Flowline
        '
        Me.chkNHDplus_Flowline.AutoSize = True
        Me.chkNHDplus_Flowline.Location = New System.Drawing.Point(6, 19)
        Me.chkNHDplus_Flowline.Name = "chkNHDplus_Flowline"
        Me.chkNHDplus_Flowline.Size = New System.Drawing.Size(64, 17)
        Me.chkNHDplus_Flowline.TabIndex = 17
        Me.chkNHDplus_Flowline.Text = "Flowline"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Flowline, "stream/river, canal/ditch, pipeline, artificial path, coastline, and connector")
        Me.chkNHDplus_Flowline.UseVisualStyleBackColor = True
        '
        'chkNHDplus_CatchmentGrid
        '
        Me.chkNHDplus_CatchmentGrid.AutoSize = True
        Me.chkNHDplus_CatchmentGrid.Location = New System.Drawing.Point(112, 42)
        Me.chkNHDplus_CatchmentGrid.Name = "chkNHDplus_CatchmentGrid"
        Me.chkNHDplus_CatchmentGrid.Size = New System.Drawing.Size(99, 17)
        Me.chkNHDplus_CatchmentGrid.TabIndex = 22
        Me.chkNHDplus_CatchmentGrid.Text = "Catchment Grid"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_CatchmentGrid, "integer grid dataset that associates each cell with a catchment")
        Me.chkNHDplus_CatchmentGrid.UseVisualStyleBackColor = True
        '
        'chkNHDplus_FDR
        '
        Me.chkNHDplus_FDR.AutoSize = True
        Me.chkNHDplus_FDR.Location = New System.Drawing.Point(112, 88)
        Me.chkNHDplus_FDR.Name = "chkNHDplus_FDR"
        Me.chkNHDplus_FDR.Size = New System.Drawing.Size(93, 17)
        Me.chkNHDplus_FDR.TabIndex = 24
        Me.chkNHDplus_FDR.Text = "Flow Direction"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_FDR, "codes that indicate the direction water would flow from each grid cell")
        Me.chkNHDplus_FDR.UseVisualStyleBackColor = True
        '
        'chkNHDplus_Elevation
        '
        Me.chkNHDplus_Elevation.AutoSize = True
        Me.chkNHDplus_Elevation.Location = New System.Drawing.Point(6, 65)
        Me.chkNHDplus_Elevation.Name = "chkNHDplus_Elevation"
        Me.chkNHDplus_Elevation.Size = New System.Drawing.Size(70, 17)
        Me.chkNHDplus_Elevation.TabIndex = 19
        Me.chkNHDplus_Elevation.Text = "Elevation"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_Elevation, "integer grid that gives the elevation in centimeters (from the North American Ver" & _
                "tical Datum of 1988). Derived from the 30-meter NED, projected into the national" & _
                " Albers projection")
        Me.chkNHDplus_Elevation.UseVisualStyleBackColor = True
        '
        'chkNHDplus_CatchmentShape
        '
        Me.chkNHDplus_CatchmentShape.AutoSize = True
        Me.chkNHDplus_CatchmentShape.Location = New System.Drawing.Point(112, 19)
        Me.chkNHDplus_CatchmentShape.Name = "chkNHDplus_CatchmentShape"
        Me.chkNHDplus_CatchmentShape.Size = New System.Drawing.Size(111, 17)
        Me.chkNHDplus_CatchmentShape.TabIndex = 21
        Me.chkNHDplus_CatchmentShape.Text = "Catchment Shape"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_CatchmentShape, "a catchment polygon for each NHD Flowline that received a catchment")
        Me.chkNHDplus_CatchmentShape.UseVisualStyleBackColor = True
        '
        'chkNHDplus_FAC
        '
        Me.chkNHDplus_FAC.AutoSize = True
        Me.chkNHDplus_FAC.Location = New System.Drawing.Point(112, 65)
        Me.chkNHDplus_FAC.Name = "chkNHDplus_FAC"
        Me.chkNHDplus_FAC.Size = New System.Drawing.Size(115, 17)
        Me.chkNHDplus_FAC.TabIndex = 23
        Me.chkNHDplus_FAC.Text = "Flow Accumulation"
        Me.ToolTip1.SetToolTip(Me.chkNHDplus_FAC, "integer grid dataset that counts the number of cells that drain to each cell in t" & _
                "he grid")
        Me.chkNHDplus_FAC.UseVisualStyleBackColor = True
        '
        'btnDownload
        '
        Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDownload.Location = New System.Drawing.Point(288, 397)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 29
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'cboAOI
        '
        Me.cboAOI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAOI.FormattingEnabled = True
        Me.cboAOI.Items.AddRange(New Object() {"View Rectangle", "Extent of Selected Layer"})
        Me.cboAOI.Location = New System.Drawing.Point(97, 12)
        Me.cboAOI.Name = "cboAOI"
        Me.cboAOI.Size = New System.Drawing.Size(266, 21)
        Me.cboAOI.TabIndex = 30
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Area of Interest"
        '
        'frmDownload
        '
        Me.AcceptButton = Me.btnDownload
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 432)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboAOI)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.grpNHDplus)
        Me.Controls.Add(Me.grpNLCD)
        Me.Controls.Add(Me.grpTerraServer)
        Me.Controls.Add(Me.grpNWIS)
        Me.Controls.Add(Me.grpBASINS)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDownload"
        Me.Text = "Download Data"
        Me.grpBASINS.ResumeLayout(False)
        Me.grpBASINS.PerformLayout()
        Me.grpNWIS.ResumeLayout(False)
        Me.grpNWIS.PerformLayout()
        Me.grpTerraServer.ResumeLayout(False)
        Me.grpTerraServer.PerformLayout()
        Me.grpNLCD.ResumeLayout(False)
        Me.grpNLCD.PerformLayout()
        Me.grpNHDplus.ResumeLayout(False)
        Me.grpNHDplus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpBASINS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_Census As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWIS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_PCS3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_NHD As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_NED As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_LSTORET As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_GIRAS As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEMG As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEM As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_Met As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_Measurements As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_WQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_DailyFlow As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServer_Urban As System.Windows.Forms.CheckBox
    Friend WithEvents grpTerraServer As System.Windows.Forms.GroupBox
    Friend WithEvents chkTerraServer_DRG As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServer_DOQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD_1992 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNLCD As System.Windows.Forms.GroupBox
    Friend WithEvents chkNLCD_2001 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNHDplus As System.Windows.Forms.GroupBox
    Friend WithEvents chkNHDplus_FDR As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Elevation As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_CatchmentShape As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_FAC As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_CatchmentGrid As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Area As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Line As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Point As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Waterbody As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Flowline As System.Windows.Forms.CheckBox
    Friend WithEvents chkNHDplus_Subbasin As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkNHDplus_GageEvents As System.Windows.Forms.CheckBox
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents cboAOI As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
