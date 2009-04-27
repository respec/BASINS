Imports atcMwGisUtility

Friend Class clsFolders
    Friend AppFolder As String
    Friend ProjectFolder As String 'GBMM parent for all folder below
    Friend InputFolder As String 'GBMM C++ input files
    Friend OutputFolder As String 'GBMM C++ output files
    Friend DataFolder As String 'data tables
    Friend TempFolder As String 'temporary GIS files
    Public Sub New()
        AppFolder = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location) & "\Plugins\GBMM"
        ProjectFolder = IO.Path.GetDirectoryName(GisUtil.ProjectFileName) & "\GBMM"
        InputFolder = ProjectFolder & "\Input" : Create(InputFolder)
        OutputFolder = ProjectFolder & "\Output" : Create(OutputFolder)
        DataFolder = ProjectFolder & "\Data" : Create(DataFolder)
        TempFolder = ProjectFolder & "\Temp" : Create(TempFolder)
    End Sub
    Private Sub Create(ByVal Folder As String)
        If Not My.Computer.FileSystem.DirectoryExists(Folder) Then My.Computer.FileSystem.CreateDirectory(Folder)
    End Sub
End Class

Friend Enum enumDEMUnits
    Meters
    Centimeters
    Feet
End Enum

Friend Enum enumLandUseType
    GIRAS
    NLCD_1992
    NLCD_2001
    UserShapefile
    UserGrid
End Enum

Friend Enum enumLayerType
    Polyline
    Polygon
    Grid
    Point
End Enum

Friend Class clsLayer
    Friend LayerName As String
    Friend FieldName As String
    Friend LayerType As enumLayerType
    Friend Sub New(ByVal _LayerType As enumLayerType)
        LayerType = _LayerType
        LayerName = ""
        FieldName = ""
    End Sub
    Friend Function Filename() As String
        If GisUtil.IsLayer(LayerName) Then
            Return GisUtil.LayerFileName(LayerName)
        Else
            Return Project.Folders.TempFolder & "\" & LayerName & ".shp"
        End If
    End Function

    Friend Function AddLayer(Optional ByVal GroupNameSuffix As String = "") As Boolean
        GisUtil.AddGroup(GroupName & GroupNameSuffix)
        Return GisUtil.AddLayerToGroup(Filename, LayerName, GroupName & GroupNameSuffix)
    End Function

    ''' <summary>Determine number of distinct key values in layer</summary>
    Friend Function GetRowCount() As Integer
        Try
            If LayerName = "" Or FieldName = "" Or Not GisUtil.IsLayer(LayerName) Then Return 0
            If LayerType = enumLayerType.Grid Then
                Return GetGridValues(Filename).Count
            Else
                Return ExecuteScaler(GisUtil.LayerFileName(LayerName), "SELECT COUNT(*) FROM (SELECT DISTINCT {0} FROM ~)", FieldName)
            End If
        Catch ex As Exception
            Throw New Exception("Unable to determine number of features in " & LayerName, ex)
            Return 0
        End Try
    End Function
End Class

Friend Class clsLayers
    Friend Subbasins As New clsLayer(enumLayerType.Polygon)
    Friend Soils As New clsLayer(enumLayerType.Polygon)
    Friend Landuse As New clsLayer(enumLayerType.Grid)
    Friend Lakes As New clsLayer(enumLayerType.Polygon)
    Friend DEM As New clsLayer(enumLayerType.Grid)
    Friend ClimateSta As New clsLayer(enumLayerType.Point)
    Friend MercurySta As New clsLayer(enumLayerType.Point)
    Friend PointSources As New clsLayer(enumLayerType.Point)
    Friend Streams As New clsLayer(enumLayerType.Polyline)
    Friend FlowDir As New clsLayer(enumLayerType.Grid)
    Friend FlowAcc As New clsLayer(enumLayerType.Grid)
    Friend AssessPt As New clsLayer(enumLayerType.Point)
End Class

Friend Enum enumGridType
    IntegerGrid = 1
    RealGrid = 2
End Enum

Friend Class clsGrid
    Friend GridID As Integer
    Friend LayerName As String
    Friend Gridtype As enumGridType
    Friend Grid As MapWinGIS.Grid

    Friend Sub New(ByVal _GridID As Integer, ByVal _LayerName As String, ByVal _GridType As enumGridType)
        GridID = _GridID
        LayerName = _LayerName
        Gridtype = _GridType
    End Sub

    Friend Function FileName() As String
        With Project.Folders
            Select Case GridID
                Case 0
                    Return String.Format("{0}\{1}.tif", .TempFolder, LayerName)
                Case 109, 303 To 305
                    If GisUtil.IsLayer(LayerName) Then
                        Return GisUtil.LayerFileName(LayerName)
                    Else
                        Return ""
                    End If
                Case Else
                    'Return String.Format("{0}\{1}.asc", .InputFolder, LayerName)
                    Return String.Format("{0}\{1}.tif", .TempFolder, LayerName)
            End Select
        End With
    End Function

    ''' <summary>
    ''' Save grid in ASCII format in Input directory
    ''' </summary>
    ''' <returns>True if successful</returns>
    Friend Function Export() As Boolean
        Dim ExportFile As String = String.Format("{0}\{1}.asc", Project.Folders.InputFolder, LayerName)
        If GridID = 0 Then
            Return False 'should only call for grids with IDs
        ElseIf FileName() = "" Then
            Return True 'not needed
            'ElseIf IsUpToDate(ExportFile) Then
            '    Return True
        Else
            Dim g As New MapWinGIS.Grid
            Logger.Dbg("Starting to export: " & FileName())
            If Not My.Computer.FileSystem.FileExists(FileName) Then Return False
            If Not g.Open(FileName) Then Return False
            If My.Computer.FileSystem.FileExists(ExportFile) Then
                If GisUtil.IsLayerByFileName(ExportFile) Then GisUtil.RemoveLayer(GisUtil.LayerIndex(ExportFile))
                For Each fn As String In My.Computer.FileSystem.GetFiles(IO.Path.GetDirectoryName(ExportFile), FileIO.SearchOption.SearchTopLevelOnly, IO.Path.GetFileNameWithoutExtension(ExportFile) & ".*")
                    My.Computer.FileSystem.DeleteFile(fn)
                Next
            End If
            'note: ascii grid format for MapWindow is different than ArcGIS (and what is expected by GBMM program); do manual export
            Dim sw As New IO.StreamWriter(ExportFile)
            With g.Header
                sw.WriteLine("ncols         {0}", .NumberCols)
                sw.WriteLine("nrows         {0}", .NumberRows)
                sw.WriteLine("xllcorner     {0}", .XllCenter)
                sw.WriteLine("yllcorner     {0}", .YllCenter)
                sw.WriteLine("cellsize      {0}", .dX)
                sw.WriteLine("NODATA_value  {0}", -9999) '.NodataValue)
                Dim ar(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    g.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If ar(c) = .NodataValue Then ar(c) = -9999
                        If Gridtype = enumGridType.IntegerGrid Then
                            sw.Write(CInt(ar(c)) & " ")
                        Else
                            sw.Write(Math.Round(ar(c), 4) & " ")
                        End If
                    Next
                    sw.WriteLine()
                Next
            End With
            sw.Close()
            sw.Dispose()
            'g.Save(ExportFile, MapWinGIS.GridFileType.Ascii)
            'g.Close()
            Return True
        End If
    End Function

    Friend Function AddLayer(Optional ByVal Visible As Boolean = False, Optional ByVal GroupNameSuffix As String = "") As Boolean
        If Not Project.AddLayers Then Return True
        Dim g As New MapWinGIS.Grid
        g.Open(FileName)
        g.AssignNewProjection(GisUtil.ProjectProjection)
        g.Close()
        GisUtil.AddGroup(GroupName & GroupNameSuffix)
        Return GisUtil.AddLayerToGroup(FileName, LayerName, GroupName & GroupNameSuffix)
        GisUtil.LayerVisible = Visible
    End Function

    Friend Sub RemoveLayer()
        While GisUtil.IsLayerByFileName(FileName)
            GisUtil.RemoveLayer(GisUtil.LayerIndex(FileName))
        End While
    End Sub

    ''' <summary>
    ''' Delete grid and all associated files
    ''' </summary>
    ''' <remarks></remarks>
    Friend Function Delete() As Boolean
        Return DeleteGrid(FileName)
    End Function

    ''' <summary>
    ''' Compare date of grid file to other parent files; if parent files are newer than this grid, it needs to be rebuild
    ''' </summary>
    ''' <param name="ParentFiles"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function IsUpToDate(ByVal ParamArray ParentFiles() As String) As Boolean
        If FileName() = "" Then Return False
        If Project.ForceRebuild Then Return False
        Dim dt As Date = IO.File.GetLastWriteTime(FileName)
        If Project.Modified Then Return False
        'If dt < IO.File.GetLastWriteTime(Project.FileName) Then Return False 'all depend on project file
        If dt < IO.File.GetLastWriteTime(Project.Layers.Subbasins.Filename) Then Return False 'all depend on subbasin file
        For Each fn As String In ParentFiles
            Dim ext As String = IO.Path.GetExtension(fn).ToLower
            'If (ext = ".shp" Or ext = ".tif") AndAlso Not GisUtil.IsLayerByFileName(fn) Then WarningMsg("Parent file is not loaded as layer: " & fn) : Return False
            If dt < IO.File.GetLastWriteTime(fn) Then Return False
        Next
        Return True
    End Function
End Class

Friend Class clsGrids
    'the grids having a GridID are passed to the C++ program as ascii grids; others are temporary tiff grids; all are at user-specified resolution
    Friend ClimateThiessan As New clsGrid(101, "Climate Thiessan", enumGridType.IntegerGrid)
    Friend LandUse As New clsGrid(102, "Land Use", enumGridType.IntegerGrid)
    Friend Soils As New clsGrid(103, "Soil Map", enumGridType.IntegerGrid)
    Friend Subbasins As New clsGrid(104, "SubBasin Grid", enumGridType.IntegerGrid)
    Friend CNPerv As New clsGrid(105, "CN AMC2", enumGridType.RealGrid)
    Friend CNImperv As New clsGrid(106, "CN Imperv", enumGridType.RealGrid)
    Friend TravelTotal As New clsGrid(107, "Total Time", enumGridType.RealGrid)
    Friend TravelStream As New clsGrid(108, "Stream Time", enumGridType.RealGrid)
    Friend FlowLength As New clsGrid(110, "Overland Length", enumGridType.RealGrid)
    Friend AvgRoughness As New clsGrid(111, "Avg Roughness", enumGridType.RealGrid)
    Friend AvgSlopeOverland As New clsGrid(112, "Avg Slope Overland", enumGridType.RealGrid)
    Friend LSFactor As New clsGrid(201, "LS Factor", enumGridType.RealGrid)
    Friend MercuryLitter As New clsGrid(301, "Kd Comp", enumGridType.RealGrid)
    Friend MercuryThiessan As New clsGrid(302, "Mercury Thiessan", enumGridType.IntegerGrid)
    'these will be specified by user
    Friend SoilWater As New clsGrid(109, "", enumGridType.RealGrid)
    Friend MercuryDryDepo As New clsGrid(303, "", enumGridType.RealGrid)
    Friend MercuryWetDepo As New clsGrid(304, "", enumGridType.RealGrid)
    Friend MercurySoil As New clsGrid(305, "", enumGridType.RealGrid)
    'these are only temp grids
    Friend DEM As New clsGrid(0, "DEM Filtered", enumGridType.RealGrid) 'resampled grid at desired resolution
    Friend DEMFilled As New clsGrid(0, "DEM Filled", enumGridType.RealGrid)
    Friend DEMBurnin As New clsGrid(0, "DEM Burn In", enumGridType.RealGrid)
    Friend Slope As New clsGrid(0, "Slope", enumGridType.RealGrid)
    Friend FlowAccum As New clsGrid(0, "Upstream Area", enumGridType.RealGrid)
    Friend FlowDir As New clsGrid(0, "Flow Direction", enumGridType.IntegerGrid)
    Friend FlowPath As New clsGrid(0, "Flow Path", enumGridType.IntegerGrid)
    Friend DistToStream As New clsGrid(0, "Distance to Stream", enumGridType.RealGrid)
    Friend DistToOutlet As New clsGrid(0, "Distance to Outlet", enumGridType.RealGrid)
    Friend TravelOverland As New clsGrid(0, "Overland Time", enumGridType.RealGrid)
    Friend Roughness As New clsGrid(0, "Surface Roughness", enumGridType.RealGrid)
    Friend HydRadius As New clsGrid(0, "Hydraulic Radius", enumGridType.RealGrid)
    Friend AvgHydRadius As New clsGrid(0, "Avg Hydraulic Radius", enumGridType.RealGrid)
    Friend AvgSlopeStream As New clsGrid(0, "Avg Slope Stream", enumGridType.RealGrid)
    Friend CSL As New clsGrid(0, "CSL", enumGridType.RealGrid)
End Class

Friend Class clsSimFlags
    Friend Hydro, Sediment, Mercury, Wasp, WHAEM As Boolean
    Friend WHAEMDuration As Double
    Friend Sub New()
        Hydro = True
        Sediment = True
        Mercury = True
        Wasp = True
        WHAEM = False
        WHAEMDuration = 1
    End Sub
End Class

Friend Class clsSimPeriods
    Friend StartDate, EndDate As Date 'model start & end date
    Friend StartMonth, EndMonth As Integer 'starting & ending of growing season
    Friend DeltaT As Integer 'fixed at 1
    Friend Sub New()
        StartDate = "1/1/2000"
        EndDate = "1/1/2001"
        StartMonth = 4
        EndMonth = 10
        DeltaT = 1
    End Sub
End Class

Friend Class clsTable
    Friend TableName As String
    Friend StartDate, EndDate As Date
    Friend Sub New()
        StartDate = "1/1/2000"
        EndDate = "1/1/2001"
        TableName = ""
    End Sub
    Friend Function Filename() As String
        Return Project.Folders.DataFolder & "\" & TableName
    End Function
    Friend Function Export() As Boolean
        Try
            Dim ExportFile As String = Project.Folders.InputFolder & "\" & IO.Path.GetFileName(Filename)
            If My.Computer.FileSystem.FileExists(ExportFile) Then My.Computer.FileSystem.DeleteFile(ExportFile)
            My.Computer.FileSystem.CopyFile(Filename, ExportFile)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class

Friend Class clsTables
    Friend Soils As New clsTable
    Friend LandUse As New clsTable
    Friend LandUseCN As New clsTable
    Friend Climate As New clsTable
    Friend HgDryDep As New clsTable
    Friend HgWetDep As New clsTable
    Friend PointSource As New clsTable
    Friend FlowRelation As New clsTable
End Class

Friend Class clsWatershedCntl
    Friend NumSubbasins
    Friend NumLandUses
    Friend NumSoils
    Friend NumClimateSta
    Friend NumMercurySta
    Friend NumPointSources
    Friend NumLakes
End Class

Friend Class clsWatershed
    Friend WsID As Integer
    Friend Area As Double
    Friend Sub New(ByVal _WsID As Integer, ByVal _Area As Double)
        WsID = _WsID
        Area = _Area
    End Sub
End Class

Friend Class clsAssessPt
    Friend SubID, MinElev, Row, Col As Integer
    Friend Sub New(ByVal _SubID As Integer, ByVal _MinElev As Integer, ByVal _Row As Integer, ByVal _Col As Integer)
        SubID = _SubID
        MinElev = _MinElev
        Row = _Row
        Col = _Col
    End Sub
End Class

Friend Class clsLandUse
    Friend LuID As Integer 'grid value
    Friend LuType As Integer '0=water body; 1=perv. land; 2=imperv. land; 3=forest
    Friend GrowVcf As Double 'growing season vegetation factor
    Friend NGrowVcf As Double 'nongrowing season vegetation factor
    Friend CFact As Double 'crop mgmt (C) factor
    Friend PFact As Double 'practice (P) factor
    Friend Imp_DCon As Double 'disconnected impervious fraction (note: definition appears to be incorrect on page 12 of users guide)
    Friend Imp_Tot As Double 'total impervious fraction
    Friend LuName As String 'name
    Friend Sub New()
    End Sub
    Friend Sub New(ByVal _LuID As Integer, ByVal _LuType As Integer, ByVal _GrowVcf As Double, ByVal _NGrowVcf As Double, ByVal _CFact As Double, ByVal _PFact As Double, ByVal _Imp_DCon As Double, ByVal _Imp_Tot As Double, ByVal _LuName As String)
        LuID = _LuID
        LuType = _LuType
        GrowVcf = _GrowVcf
        NGrowVcf = _NGrowVcf
        CFact = _CFact
        PFact = _PFact
        Imp_DCon = _Imp_DCon
        Imp_Tot = _Imp_Tot
        LuName = _LuName
    End Sub
End Class

Friend Class clsSoil
    Friend SoilID As Integer 'grid value (integer assigned sequentially to MUID in SoilData table)
    Friend MUID As String 'soil ID like GA123
    Friend AWC As Double 'avail water capacity
    Friend BD As Double 'bulk density
    Friend ClayFrac As Double 'clay fraction
    Friend Perm As Double 'permeability
    Friend KFact As Double 'erodibility (K) factor
    Friend GroupValue As Integer 'soil group value (100=A, 200=B, 300=C, 400=D)
    Friend Sub New()
    End Sub
    Friend Sub New(ByVal _SoilID As Integer, ByVal _MUID As String, ByVal _AWC As Double, ByVal _BD As Double, ByVal _ClayFrac As Double, ByVal _Perm As Double, ByVal _KFact As Double, ByVal _GroupValue As Integer)
        SoilID = _SoilID
        MUID = _MUID
        AWC = _AWC
        BD = _BD
        ClayFrac = _ClayFrac
        Perm = _Perm
        KFact = _KFact
        GroupValue = _GroupValue
    End Sub
End Class

Friend Class clsStation
    Friend StaID As Integer
    Friend StaName As String
    Friend Latitude As Double 'only used for climate stations
    Friend TravelTime As Double 'only used for point sources
    Friend Sub New(ByVal _StaID As Integer, ByVal _StaName As String, Optional ByVal _Latitude As Double = 0, Optional ByVal _TravelTime As Double = 0)
        StaID = _StaID
        StaName = _StaName
        Latitude = _Latitude
        TravelTime = _TravelTime
    End Sub
    Friend Sub New()
    End Sub
End Class

Friend Class clsClimStaFrac
    Friend StaID As Integer
    Friend Frac As Double
    Friend Sub New(ByVal _StaID As Integer, ByVal _Frac As Double)
        StaID = _StaID
        Frac = _Frac
    End Sub
End Class

Friend Class clsLake
    Friend LakeID As Integer 'grid value
    Friend SubBasinID As Integer 'grid value
    Friend SurfaceArea As Double
    Friend ClimStaFrac As New Generic.Dictionary(Of Integer, clsClimStaFrac)
    Friend Sub New(ByVal _LakeID As Integer, ByVal _SubBasinID As Integer, ByVal _SurfaceArea As Double)
        LakeID = _LakeID
        SubBasinID = _SubBasinID
        SurfaceArea = _SurfaceArea
    End Sub
End Class

Friend Class clsTravelTime
    Friend FromID, ToID As Integer
    Friend TravelTime As Double
    Friend Sub New(ByVal _FromID As Integer, ByVal _ToID As Integer, ByVal _TravelTime As Double)
        FromID = _FromID
        ToID = _ToID
        TravelTime = _TravelTime
    End Sub
    Friend Sub New()
    End Sub
End Class

Friend Enum enumSoilWater
    Constant = 1
    InputGrid
    FieldCapacity
End Enum

Friend Class clsWatershedParms
    Friend SoilWaterFlag As enumSoilWater
    Friend InitSoilWater As Double
    Friend InitSnow As Double
    Friend GrowA, GrowB, NGrowA, NGrowB As Double
    Friend P2Rainfall As Double
    Friend GWInit, GWRecess, GWSeepage, GWRecharge As Double
    Friend USDepth, BRDepth As Double
    Friend Sub New()
        SoilWaterFlag = enumSoilWater.FieldCapacity
        InitSoilWater = 30
        InitSnow = 0
        GrowA = 3.6
        GrowB = 5.3
        NGrowA = 1.3
        NGrowB = 2.8
        P2Rainfall = 10
        GWInit = 30
        GWRecess = 0.1
        GWSeepage = 0
        USDepth = 1
        BRDepth = 1.5
    End Sub
End Class

Friend Enum enumRegion
    Eastern
    SFBay
    UpperGreen
    UpperSalmon
    UserDefined
End Enum

Friend Class clsStreamParms
    Friend RegionArray() As String = {"Eastern US", "San Francisco Bay Region", "Upper Green River, Wyoming", "Upper Salmon River, Idaho", "User Defined"}
    Friend AlphaDepthArray() As Double = {0.33, 0.378, 0.3094, 0.1593, 0}
    Friend BetaDepthArray() As Double = {0.2964, 0.2582, 0.1923, 0.2625, 0}
    Friend AlphaWidthArray() As Double = {2.933, 3.513, 1.579, 1.4968, 0}
    Friend BetaWidthArray() As Double = {0.3916, 0.3692, 0.3866, 0.4562, 0}
    Friend Manning As Double
    Friend Z1, Z2 As Double
    Friend Region As enumRegion
    Friend AlphaDepth, BetaDepth As Double
    Friend AlphaWidth, BetaWidth As Double
    Friend Sub New()
        Manning = 0.014
        Z1 = 1
        Z2 = 1
    End Sub
End Class

Friend Class clsLakeParms
    Friend AreaThreshold As Double 'sq m
    Friend FullDepth As Double
    Friend Depth, SedimentHg, WaterHg, BenthicHg As Double 'initial values
    Friend LakeCount As Integer
    Friend Infilt, Evap As Double
    Friend OrifH, OrifD, OrifC, WeirL, WeirC As Double
    Friend Sub New()
        AreaThreshold = 1000000
        FullDepth = 0.5
        Depth = 0.5
        SedimentHg = 0.5
        WaterHg = 1
        BenthicHg = 30
        Evap = 0.1
        OrifH = 0
        OrifD = 0
        OrifC = 0.6
        WeirL = 30
        WeirC = 1.84
        Infilt = 0.001
    End Sub
End Class

Friend Enum enumLSFlag
    DEM
    Constant
    Existing
End Enum

Friend Class clsLSFactor
    Friend LSFlag As enumLSFlag
    Friend MaxSlopeLengthFlag As Boolean
    Friend MaxSlopeLength As Double
    Friend ConstantSlopeLength As Double
    Friend Sub New()
        LSFlag = enumLSFlag.Constant
        MaxSlopeLengthFlag = False
        MaxSlopeLength = 100
        ConstantSlopeLength = 30
    End Sub
End Class

Friend Class clsWatershedSedParms
    Friend InitPerv, InitImperv As Double
    Friend Accum, Deplet, Capacity, RainAlpha
    Friend SDRAlpha, SDRBeta As Double
    Friend Sub New()
        InitPerv = 0
        InitImperv = 0
        Accum = 0.05
        Deplet = 0.12
        Capacity = 30
        RainAlpha = 0.5
        SDRAlpha = 1
        SDRBeta = 0.01
    End Sub
End Class

Friend Class clsLakeSedParms
    Friend TssEquil, SettleDecay As Double
    Friend Clay, Silt, Sand As Double
    Friend Sub New()
        TssEquil = 0.4
        SettleDecay = 0.184
        Clay = 0.5
        Silt = 0.4
        Sand = 0.1
    End Sub
End Class

Friend Enum enumDepoFlag
    Constant = 1
    InputGrid
    TimeSeries
End Enum

Friend Enum enumWetDepo
    WetDepo = 1
    Rainfall
End Enum

Friend Enum enumDepoStep
    Daily = 1
    Monthly
End Enum

Friend Class clsMercuryDepo
    Friend AirDepoFlag As enumDepoFlag, WetDepoFlag As enumWetDepo
    Friend DryDepoFlux, WetDepoFlux, WetDepoPrecip As Double 'if AirDepoFlag=1
    Friend DryDepoMult, WetDepoMult As Double 'if AirDepoFlag=2
    Friend DryDepoStep, WetDepoStep As enumDepoStep 'if AirDepoFlag=3
    Friend Sub New()
        AirDepoFlag = enumDepoFlag.Constant
        DryDepoFlux = 0.03
        WetDepoFlag = enumWetDepo.WetDepo
        WetDepoFlux = 0.15
        WetDepoPrecip = 15.0
        DryDepoMult = 1.0
        WetDepoMult = 1.0
    End Sub
End Class

Friend Enum enumMercFlag
    InitConc = 1
    GridMult
End Enum

Friend Class clsWatershedMercuryParms
    Friend SoilFlag As enumMercFlag
    Friend InitSoil, InitGW, GridMult As Double
    Friend SoilMixing, SoilReductionDepth, SoilPartition, SoilBaseReductionRate As Double
    Friend Enrich, BedrockDensity, Weathering, InitRock As Double
    Friend ChannelDecay, Fraction As Double
    Friend Sub New()
        SoilFlag = enumMercFlag.InitConc
        InitSoil = 50
        GridMult = 1
        InitGW = 0.00001
        SoilMixing = 1
        SoilReductionDepth = 0.5
        SoilPartition = 58000
        SoilBaseReductionRate = 0.0001
        Enrich = 2
        BedrockDensity = 2.6
        Weathering = 0.0004
        InitRock = 60
        ChannelDecay = 0.04
        Fraction = 0.06
    End Sub
End Class

Friend Class clsForestMercuryParms
    Friend AET, IntFrac, AdhereFrac As Double
    Friend Litter, BioConc, AirConc As Double
    Friend LitterDecomp1, LitterDecomp2, LitterDecomp3 As Double
    Friend Sub New()
        AET = 30
        IntFrac = 0.47
        AdhereFrac = 0.6
        Litter = 40
        BioConc = 18000
        AirConc = 0.00155
        LitterDecomp1 = 0.0019
        LitterDecomp2 = 0.0005
        LitterDecomp3 = 0.0012
    End Sub
End Class

Friend Class clsLakeMercuryParms
    Friend AlphaW, BetaW, KWR, KWM, VSB, VRS, KDSW, KBio, CBio, PD As Double
    Friend Sub New()
        AlphaW = 1
        BetaW = 0.4
        KWR = 0.075
        KWM = 0.001
        VSB = 0.2
        VRS = 0.00001
        KDSW = 100000
        KBio = 200000
        CBio = 0.7
        PD = 2.65
    End Sub
End Class

Friend Class clsBenthicMercuryParms
    Friend AlphaB, BetaB, KBR, KBM, Por_BS, VBur, ZB, Kdbs, Cbs, Esw As Double
    Friend Sub New()
        AlphaB = 1
        BetaB = 0.4
        KBR = 0.000001
        KBM = 0.0001
        Por_BS = 0.9
        VBur = 0.00000035
        ZB = 0.02
        Kdbs = 50000
        Cbs = 75000
        Esw = 0.000000005
    End Sub
End Class

''' <summary>
''' Structure corresponding to each input item required by GBMM C++ computational program
''' </summary>
Friend Class clsProject

    Friend FileName As String
    Friend Folders As New clsFolders

    Friend Layers As New clsLayers
    Friend Grids As New clsGrids
    Friend Tables As New clsTables

    Friend SimFlags As New clsSimFlags
    Friend SimPeriods As New clsSimPeriods
    Friend WatershedCntl As New clsWatershedCntl
    Friend AssessmentPoints As New Generic.Dictionary(Of Integer, clsAssessPt)
    Friend Watersheds As New Generic.List(Of clsWatershed)
    Friend Landuses As New Generic.Dictionary(Of Integer, clsLandUse)
    Friend Soils As New Generic.Dictionary(Of String, clsSoil)
    Friend ClimateStations As New Generic.Dictionary(Of Integer, clsStation)
    Friend MercuryStations As New Generic.Dictionary(Of Integer, clsStation)
    Friend PointSources As New Generic.Dictionary(Of Integer, clsStation)
    Friend Lakes As New Generic.List(Of clsLake)
    Friend TravelTimes As New Generic.List(Of clsTravelTime)
    Friend WatershedParms As New clsWatershedParms
    Friend StreamParms As New clsStreamParms
    Friend LakeParms As New clsLakeParms
    Friend LSFactor As New clsLSFactor
    Friend WatershedSedParms As New clsWatershedSedParms
    Friend LakeSedParms As New clsLakeSedParms
    Friend MercuryDepo As New clsMercuryDepo
    Friend WatershedMercuryParms As New clsWatershedMercuryParms
    Friend ForestMercuryParms As New clsForestMercuryParms
    Friend LakeMercuryParms As New clsLakeMercuryParms
    Friend BenthicMercuryParms As New clsBenthicMercuryParms

    Friend LanduseType As enumLandUseType
    Friend _DEMUnits As enumDEMUnits

    ''' <summary>Area threshold (sq km) above which grid cells are considered streams</summary>
    Friend StreamThreshold As Double

    ''' <summary>Conversion factor for distance (units/meter)</summary>
    Friend DistFactor As Double

    ''' <summary>Conversion factor for dem grid elevations (units/meter)</summary>
    Friend ElevFactor As Double

    ''' <summary>Desired computational grid size in map units</summary>
    Friend GridSize As Double

    Friend ForceRebuild As Boolean
    Friend AddLayers As Boolean

    Friend Modified As Boolean

    Private sw As IO.StreamWriter
    Private sr As IO.StreamReader

    Public Sub New()
        FileName = ""
        LanduseType = enumLandUseType.GIRAS
        With Layers.AssessPt
            .LayerName = "Assessment Points"
            .FieldName = "SubID"
        End With
        DEMUnits = enumDEMUnits.Centimeters 'default for NHD
        DistFactor = 1.0
        ElevFactor = 1.0
        StreamThreshold = 0.5 'upstream area sq km
        ForceRebuild = False
        AddLayers = True
        Modified = False
    End Sub

    Friend Property DEMUnits() As enumDEMUnits
        Get
            Return _DEMUnits
        End Get
        Set(ByVal value As enumDEMUnits)
            _DEMUnits = value
            Select Case _DEMUnits
                Case enumDEMUnits.Centimeters : ElevFactor = 100
                Case enumDEMUnits.Meters : ElevFactor = 1
                Case enumDEMUnits.Feet : ElevFactor = 3.2808
            End Select
        End Set
    End Property

    ''' <summary>
    ''' Use GBMM C++ input file as data file format for this application; the few input items not required by C++ app will be save as comments
    ''' </summary>
    Friend Function Save() As Boolean
        Try
            sw = New IO.StreamWriter(IO.Path.ChangeExtension(FileName, ".gbmm"), False, Text.Encoding.ASCII)
            WriteCardHeader()
            WriteCard000()
            sw.Close()
            sw.Dispose()

            sw = New IO.StreamWriter(FileName)
            WriteCardHeader()

            WriteCard100()
            WriteCard110()
            WriteCard120()
            WriteCard130()
            WriteCard140()
            WriteCard150()

            If SimFlags.Sediment Then WriteCard160()
            If SimFlags.Mercury Then WriteCard170()

            WriteCard200()
            WriteCard210()
            WriteCard220()
            WriteCard230()
            WriteCard240()

            If MercuryDepo.AirDepoFlag = enumDepoFlag.TimeSeries Then WriteCard250()

            If Layers.PointSources.LayerName <> "" Then WriteCard260()

            If Layers.Lakes.LayerName <> "" And LakeParms.LakeCount > 0 Then
                WriteCard270()
                WriteCard280()
            End If

            WriteCard290()
            WriteCard300()
            If Layers.Lakes.LayerName <> "" And LakeParms.LakeCount > 0 Then WriteCard310()

            If SimFlags.Sediment Then
                WriteCard320()
                If Layers.Lakes.LayerName <> "" And LakeParms.LakeCount > 0 Then WriteCard330()
            End If

            If SimFlags.Mercury Then
                WriteCard340()
                WriteCard350()
                WriteCard360()
                If Layers.Lakes.LayerName <> "" And LakeParms.LakeCount > 0 Then
                    WriteCard370()
                    WriteCard380()
                End If
            End If

            Modified = False
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            'Close the input file
            sw.Close()
            sw.Dispose()
        End Try
    End Function

#Region "Routines to write each card type..."

    Private Const Blank As String = ("c---------------------------------------------------------------------------------------------------------------------")

    Private Sub Write(ByVal ParamArray Args() As Object)
        For Each o As Object In Args
            If TypeOf o Is Boolean Then
                sw.Write(vbTab & IIf(o, 1, 0))
            ElseIf TypeOf o Is Date Then
                sw.Write(vbTab & CDate(o).ToString("MM/dd/yy"))
            Else
                sw.Write(vbTab & o)
            End If
        Next
        sw.WriteLine()
    End Sub

    Private Sub WriteCardHeader()
        sw.WriteLine(Blank)
        sw.WriteLine("c   GBMM  - Grid Based Mercury Model, C++ Version 2.0")
        sw.WriteLine("c")
        sw.WriteLine("c   Designed for:")
        sw.WriteLine("c      U.S. EPA, National Exposure Research Laboratory")
        sw.WriteLine("c      Ecosystems Research Division")
        sw.WriteLine("c      960 College Station Rd.")
        sw.WriteLine("c      Athens, GA 30605")
        sw.WriteLine("c")
        sw.WriteLine("c   Developed and maintained by:")
        sw.WriteLine("c      Tetra Tech, Inc.")
        sw.WriteLine("c      10306 Eaton Place, Suite 340")
        sw.WriteLine("c      Fairfax, VA 22030")
        sw.WriteLine("c      Phone: (703) 385-6000")
        sw.WriteLine("c   Preprocessor redeveloped for BASINS 4.0/MapWindow by:")
        sw.WriteLine("c      Lloyd Chris Wilson, Ph.D., P.E.")
        sw.WriteLine("c      Clayton Engineering")
        sw.WriteLine("c      11920 Westline Industrial Drive")
        sw.WriteLine("c      St. Louis, MO 63146")
        sw.WriteLine("c      Phone: (314) 692-8888")
        sw.WriteLine("c      EMail: cwilson@claytoneng.pro")
        sw.WriteLine(Blank)
        sw.WriteLine("c   GBMM INPUT FILE")
        sw.WriteLine("c   This input file was created at " & Now)
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard000()
        With Layers
            sw.WriteLine("c PREPROCESSOR INFO FOLLOWS ON CARDS 010 THROUGH 090")
            sw.WriteLine("c")
            sw.WriteLine("c 010 SUBBASIN INFO")
            sw.WriteLine("c")
            sw.WriteLine("c SubbasinLayer")
            sw.WriteLine("c")
            Write(.Subbasins.LayerName)
            sw.WriteLine("c")

            sw.WriteLine("c020 DEM INFO")
            sw.WriteLine("c")
            sw.WriteLine("c DEMLayer          DEMUnits         GridSize         FlowAccLayer       FlowDirLayer       StreamThresh")
            Write(.DEM.LayerName, DEMUnits, GridSize, Grids.FlowAccum.LayerName, Grids.FlowDir.LayerName, StreamThreshold)
            sw.WriteLine("c")

            sw.WriteLine("c030 SOILS INFO")
            sw.WriteLine("c")
            sw.WriteLine("c SoilsLayer        SoilsField       SoilsTable")
            sw.WriteLine("c")
            Write(.Soils.LayerName, .Soils.FieldName, Tables.Soils.TableName)
            sw.WriteLine("c")

            sw.WriteLine("c040 LANDUSE INFO")
            sw.WriteLine("c")
            sw.WriteLine("c LandUseType       LandUseLayer     LandUseField        LanduseTable       CurveNumberTable")
            sw.WriteLine("c")
            Write(LanduseType, .Landuse.LayerName, .Landuse.FieldName, Tables.LandUse.TableName, Tables.LandUseCN.TableName)

            sw.WriteLine("c050 LAKES INFO")
            sw.WriteLine("c")
            sw.WriteLine("c LakesLayer        LakesField       AreaThreshold")
            sw.WriteLine("c")
            Write(.Lakes.LayerName, .Lakes.FieldName, LakeParms.AreaThreshold)

            sw.WriteLine("c060 CLIMATE, POINT SOURCE, & MERCURY STA INFO")
            sw.WriteLine("c")
            sw.WriteLine("c ClimateStaLayer   ClimateStaField  ClimateTable     PointSourceLayer   PointSourceField    PointSourceTable    MercuryStaLayer   MercuryField")
            sw.WriteLine("c")
            Write(.ClimateSta.LayerName, .ClimateSta.FieldName, Tables.Climate.TableName, .PointSources.LayerName, .PointSources.FieldName, Tables.PointSource.TableName, .MercurySta.LayerName, .MercurySta.FieldName)

            sw.WriteLine("c070 MISC MERCURY VALUES")
            sw.WriteLine("c")
            sw.WriteLine("c WHAEM Duration    Init. Const.      Grid Mult        Wet Flux         Precip Conc.    Litter Decomp Rates")
            sw.WriteLine("c")
            With ForestMercuryParms
                Write(SimFlags.WHAEMDuration, WatershedMercuryParms.InitSoil, WatershedMercuryParms.GridMult, MercuryDepo.WetDepoFlux, MercuryDepo.WetDepoPrecip, .LitterDecomp1, .LitterDecomp2, .LitterDecomp3)
            End With

            sw.WriteLine("c080 LS FACTOR SETTINGS")
            sw.WriteLine("c")
            sw.WriteLine("c Option   MaxLengthFlag   MaxLength   ConstLength")
            sw.WriteLine("c")
            With LSFactor
                Write(.LSFlag, .MaxSlopeLengthFlag, .MaxSlopeLength, .ConstantSlopeLength)
            End With

            sw.WriteLine("c")
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard100()
        sw.WriteLine("c100 SIMULATION CONTROL")
        sw.WriteLine("c")
        sw.WriteLine("c    hydrofg      if = 1 run hydrology module")
        sw.WriteLine("c    sedfg        if = 1 run sediment module")
        sw.WriteLine("c    mercuryfg    if = 1 run mercury module")
        sw.WriteLine("c    waspfg       if = 1 run model with wasp linkage")
        sw.WriteLine("c    whaemfg      if = 1 run model with whaem linkage")
        sw.WriteLine("c")
        sw.WriteLine("c  hydrofg sedfg   mercuryfg   waspfg  whaemfg")
        With SimFlags
            Write(.Hydro, .Sediment, .Mercury, .Wasp, .WHAEM)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard110()
        sw.WriteLine("c110 MODEL SIMULATION TIME PERIOD")
        sw.WriteLine("c")
        sw.WriteLine("c    simstart     model start date")
        sw.WriteLine("c    simend       model end date")
        sw.WriteLine("c    startmonth   growing season start month")
        sw.WriteLine("c    endmonth     growing season end month")
        sw.WriteLine("c    delt         time step in days (fixed = 1)")
        sw.WriteLine("c")
        sw.WriteLine("c  simstart    simend  startmonth  endmonth    delt")
        With SimPeriods
            Write(.StartDate, .EndDate, .StartMonth, .EndMonth, .DeltaT)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard120()
        sw.WriteLine("c120 DATA TIME PERIOD")
        sw.WriteLine("c")
        sw.WriteLine("c    ******data time period should be within simulation time period in card 110******")
        sw.WriteLine("c    ******data time period should be minimum one year if simulation time period is greater than or equal to one year******")
        sw.WriteLine("c    ******data time period should be equal to simulation time period if simulation time period is less than one year******")
        sw.WriteLine("c")
        sw.WriteLine("c     dataindex   data index to distinguish the data type")
        sw.WriteLine("c                 if = 1 climate data")
        sw.WriteLine("c                 if = 2 mercury dry deposition data")
        sw.WriteLine("c                 if = 3 mercury wet deposition data")
        sw.WriteLine("c     datastart   data start date")
        sw.WriteLine("c     dataend     data end date")
        sw.WriteLine("c")
        sw.WriteLine("c     dataindex   datastart   dataend")
        With Tables.Climate
            Write(1, .StartDate, .EndDate)
        End With
        If SimFlags.Mercury And Layers.MercurySta.LayerName <> "" Then
            With Tables.HgDryDep
                Write(2, .StartDate, .EndDate)
            End With
            With Tables.HgWetDep
                Write(3, .StartDate, .EndDate)
            End With
        End If
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard130()
        sw.WriteLine("c130 TIME SERIES FILE PATH")
        sw.WriteLine("c")
        sw.WriteLine("c     fileindex   file index to distinguish the data type")
        sw.WriteLine("c                 if = 1  climate file (month/day/year, station_ID, precipitation_cm, average temperature_C)")
        sw.WriteLine("c                 if = 2  dry deposition mercury file (month/day/year, station_ID, dry deposition rate_ug/m2)")
        sw.WriteLine("c                 if = 3  wet deposition mercury file (month/day/year, station_ID, wet deposition rate_ug/m2)")
        sw.WriteLine("c                 if = 4  point source file (month/day/year, station_ID, flow rate_m3/s, sediment load_kg, mercury load_ug)")
        sw.WriteLine("c     filepath    time series file path")
        sw.WriteLine("c")
        sw.WriteLine("c     fileindex   filepath")
        With Tables.Climate
            Write(1, .TableName)
        End With
        If SimFlags.Mercury And Layers.MercurySta.LayerName <> "" Then
            With Tables.HgDryDep
                Write(2, .TableName)
            End With
            With Tables.HgWetDep
                Write(3, .TableName)
            End With
        End If
        With Tables.PointSource
            If .TableName <> "" Then Write(4, .TableName)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard140()
        sw.WriteLine("c140 INPUT/OUTPUT FILE PATH")
        sw.WriteLine("c")
        sw.WriteLine("c pathindex   path index")
        sw.WriteLine("c             if = 1 input folder path")
        sw.WriteLine("c             if = 2 output folder path")
        sw.WriteLine("c folderpath")
        sw.WriteLine("c")
        sw.WriteLine("c pathindex   folderpath")
        With Folders
            Write(1, .InputFolder)
            Write(2, .OutputFolder)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard150()
        sw.WriteLine("c150 HYDROLOGY INPUT GRIDS (ASCII FORMAT)")
        sw.WriteLine("c")
        sw.WriteLine("c    gridindex    grid index to distinguish the grid type")
        sw.WriteLine("c                 if 101 thiessen grid for climate stations (INTEGER GRID)")
        sw.WriteLine("c                 if 102 landuse grid i.e. MRLC (INTEGER GRID)")
        sw.WriteLine("c                 if 103 soil properties grid (INTEGER GRID)")
        sw.WriteLine("c                 if 104 subwatershed grid (INTEGER GRID)")
        sw.WriteLine("c                 if 105 curve number pervious land grid2 (REAL GRID)")
        sw.WriteLine("c                 if 106 curve number impervious land grid (REAL GRID)")
        sw.WriteLine("c                 if 107 totaltime (REAL GRID)")
        sw.WriteLine("c                 if 108 streamtime (REAL GRID)")
        sw.WriteLine("c                 if 109 soil water grid, optional (REAL GRID)")
        sw.WriteLine("c                 if 110 overland length (REAL GRID)")
        sw.WriteLine("c                 if 111 average roughness (REAL GRID)")
        sw.WriteLine("c                 if 112 average slope (REAL GRID)")
        sw.WriteLine("c     gridtype    grid type (integer or real)")
        sw.WriteLine("c                     if = 1    integer grid")
        sw.WriteLine("c                     if = 2    real grid")
        sw.WriteLine("c     gridfile    grid file path")
        sw.WriteLine("c")
        sw.WriteLine("c     gridindex   gridtype    gridfile")
        Dim lstGrids As New Generic.List(Of clsGrid)
        With Grids
            lstGrids.Add(.ClimateThiessan)
            lstGrids.Add(.LandUse)
            lstGrids.Add(.Soils)
            lstGrids.Add(.Subbasins)
            lstGrids.Add(.CNPerv)
            lstGrids.Add(.CNImperv)
            lstGrids.Add(.TravelTotal)
            lstGrids.Add(.TravelStream)
            If WatershedParms.SoilWaterFlag = enumSoilWater.Constant Then lstGrids.Add(.SoilWater)
            lstGrids.Add(.FlowLength)
            lstGrids.Add(.AvgRoughness)
            lstGrids.Add(.AvgSlopeOverland)
        End With
        For Each g As clsGrid In lstGrids
            With g
                Write(.GridID, .Gridtype, IO.Path.GetFileNameWithoutExtension(.FileName) & ".asc")
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard160()
        sw.WriteLine("c160 SEDIMENT INPUT GRIDS (ASCII FORMAT)")
        sw.WriteLine("c")
        sw.WriteLine("c gridindex   grid index to distinguish the grid type")
        sw.WriteLine("c             if = 201 LS grid for MUSLE equation(REAL GRID)")
        sw.WriteLine("c    gridtype    grid type (integer or real)")
        sw.WriteLine("c                if = 1    integer grid")
        sw.WriteLine("c                if = 2    real grid")
        sw.WriteLine("c    gridfile    grid file name")
        sw.WriteLine("c")
        sw.WriteLine("c    gridindex   gridtype   gridfile")
        With Grids.LSFactor
            Write(.GridID, .Gridtype, IO.Path.GetFileNameWithoutExtension(.FileName) & ".asc")
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard170()
        sw.WriteLine("c170 MERCURY INPUT GRIDS (ASCII FORMAT)")
        sw.WriteLine("c")
        sw.WriteLine("c gridindex   grid index to distinguish the grid type")
        sw.WriteLine("c                if = 301  forest litter decomposition rate grid (REAL GRID)")
        sw.WriteLine("c                if = 302  thiessen grid for mercury stations, optional (INTEGER GRID)")
        sw.WriteLine("c                if = 303  mercury dry deposition grid, optional (REAL GRID)")
        sw.WriteLine("c                if = 304  mercury wet deposition grid, optional (REAL GRID)")
        sw.WriteLine("c                if = 305  soil mercury conc grid, optional (REAL GRID)")
        sw.WriteLine("c")
        sw.WriteLine("c    gridtype    grid type (integer or real)")
        sw.WriteLine("c                if = 1    integer grid")
        sw.WriteLine("c                if = 2    real grid")
        sw.WriteLine("c")
        sw.WriteLine("c    gridfile    grid file name")
        sw.WriteLine("c")
        sw.WriteLine("c    gridindex   gridtype   gridfile")
        Dim lstGrids As New Generic.List(Of clsGrid)
        With Grids
            lstGrids.Add(.MercuryLitter)
            If MercuryDepo.AirDepoFlag = enumDepoFlag.TimeSeries Then lstGrids.Add(.MercuryThiessan)
            If .MercuryDryDepo.LayerName <> "" Then lstGrids.Add(.MercuryDryDepo)
            If .MercuryWetDepo.LayerName <> "" Then lstGrids.Add(.MercuryWetDepo)
            If .MercurySoil.LayerName <> "" Then lstGrids.Add(.MercurySoil)
        End With
        For Each g As clsGrid In lstGrids
            With g
                Write(.GridID, .Gridtype, IO.Path.GetFileNameWithoutExtension(.FileName) & ".asc")
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard200()
        sw.WriteLine("c200 WATERSHED CONTROLS")
        sw.WriteLine("c")
        sw.WriteLine("c nsws    number of subwatersheds in study area")
        sw.WriteLine("c nlus    number of land uses in study area")
        sw.WriteLine("c nsls    number of soil types in study area")
        sw.WriteLine("c ncls    number of climate data stations in study area")
        sw.WriteLine("c nhgs    number of mercury data stations in study area")
        sw.WriteLine("c npts    number of point sources in study area")
        sw.WriteLine("c nlks    number of lakes in study area")
        sw.WriteLine("c")
        sw.WriteLine("c nsws    nlus    nsls    ncls    nhgs    npts    nlks")
        Write(Watersheds.Count, Landuses.Count, Soils.Count, ClimateStations.Count, MercuryStations.Count, PointSources.Count, Lakes.Count)
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard210()
        sw.WriteLine("c210 SUBWATERSHED INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c swsindex    subwatershed index(Serial number)")
        sw.WriteLine("c swsid       subwatershed id (grid value)")
        sw.WriteLine("c swsarea     subwatershed area (m2)")
        sw.WriteLine("c")
        sw.WriteLine("c swsindex    swsid    swsarea")
        For i As Integer = 0 To Watersheds.Count - 1
            With Watersheds(i)
                Write(i + 1, .WsID, Math.Round(.Area, 2))
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard220()
        sw.WriteLine("c220 LANDUSE INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c luindex     landuse index (serial number)")
        sw.WriteLine("c luid        landuse id (grid value)")
        sw.WriteLine("c lutype      landuse type")
        sw.WriteLine("c             0 for water body")
        sw.WriteLine("c             1 for pervious land")
        sw.WriteLine("c             2 for impervious land")
        sw.WriteLine("c             3 for forest land")
        sw.WriteLine("c")
        sw.WriteLine("c growvcf     growing season vegetation cover factor (0-1)")
        sw.WriteLine("c ngrowvcf    nongrowing season vegetation cover factor (0-1)")
        sw.WriteLine("c cfact       crop factor (0-1)")
        sw.WriteLine("c pfact       practice factor (0-1)")
        sw.WriteLine("c luname      landuse name")
        sw.WriteLine("c")
        sw.WriteLine("c luindex luid    lutype  growvcf ngrowvcf    cfact   pfact   luname")
        Dim i As Integer = 1
        For Each lu As clsLandUse In Landuses.Values
            With lu
                Write(i, .LuID, .LuType, .GrowVcf, .NGrowVcf, .CFact, .PFact, .LuName)
                i += 1
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard230()
        sw.WriteLine("c230 SOIL PROPERTIES INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c slindex     soil properties index (serial number)")
        sw.WriteLine("c slid        soil properties id (grid value)")
        sw.WriteLine("c awc         plant available water content (cm/m)")
        sw.WriteLine("c bd          soil bulk density (g/cm3)")
        sw.WriteLine("c clayfr      fraction of clay content in soil")
        sw.WriteLine("c perm        soil permeability (cm/hr)")
        sw.WriteLine("c kfact       soil erodability factor (0-1)")
        sw.WriteLine("c")
        sw.WriteLine("c slindex slid  awc   bd  clayfr  perm    kfact")
        Dim i As Integer = 1
        For Each s As clsSoil In Soils.Values
            With s
                Write(i, .SoilID, .AWC, .BD, .ClayFrac, .Perm, .KFact)
                i += 1
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard240()
        sw.WriteLine("c240 CLIMATE STATION INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c clsindex    climate station index (serial number)")
        sw.WriteLine("c clsid       climate station id (grid value)")
        sw.WriteLine("c clsname     climate station name (station_id)")
        sw.WriteLine("c clslat      climate station latitude (degrees)")
        sw.WriteLine("c")
        sw.WriteLine("c clsindex clsid  clsname clslat")
        Dim i As Integer = 0
        For Each kv As KeyValuePair(Of Integer, clsStation) In ClimateStations
            With kv.Value
                i += 1
                Write(i, .StaID, .StaName, .Latitude)
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard250()
        sw.WriteLine("c250 MERCURY STATION INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c hgsindex    mercury station index (serial number)")
        sw.WriteLine("c hgsid       mercury station id (grid value)")
        sw.WriteLine("c hgsname     mercury station name (station_id)")
        sw.WriteLine("c")
        sw.WriteLine("c hgsindex hgsid  hgsname")
        Dim i As Integer = 0
        For Each kv As KeyValuePair(Of Integer, clsStation) In MercuryStations
            With kv.Value
                i += 1
                Write(i, .StaID, .StaName)
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard260()
        sw.WriteLine("c260 POINT SOURCE INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c    psindex  point source index (seriel number)")
        sw.WriteLine("c    swsid    subwatershed id (grid value)")
        sw.WriteLine("c    psname   point source name (permit)")
        sw.WriteLine("c    psttime  point source travel time to outlet (hr)")
        sw.WriteLine("c")
        sw.WriteLine("c    psindex  swsid   psname  psttime")
        Dim i As Integer = 0
        For Each kv As KeyValuePair(Of Integer, clsStation) In PointSources
            With kv.Value
                i += 1
                Write(i, .StaID, .StaName, Math.Round(.TravelTime, 3))
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard270()
        sw.WriteLine("c270 LAKE INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c lkindex     lake index (serial number)")
        sw.WriteLine("c lkid        lake id (grid value)")
        sw.WriteLine("c swsid       subwatershed id (grid value)")
        sw.WriteLine("c lkarea      lake surface area (m2)")
        sw.WriteLine("c lkdepth     lake bankfull depth (m)")
        sw.WriteLine("c lkidepth    lake initial water depth (m)")
        sw.WriteLine("c lkised      lake initial sediment concentration (mg/l)")
        sw.WriteLine("c lkihg       lake initial mercury concentration (ng/l)")
        sw.WriteLine("c lkihgb      lake initial benthic mercury concentration (ng/g)")
        sw.WriteLine("c")
        sw.WriteLine("c lkindex lkid    swsid   lkarea lkdepth lkidepth lkised   lkihg  lkihgb")

        'assume that each lake is entirely contained within a subbasin (or not in any subbasins)

        If Layers.Lakes.LayerName = "" Or Layers.Subbasins.LayerName = "" Then Exit Sub
        For i As Integer = 0 To Project.Lakes.Count - 1
            With LakeParms
                Write(i + 1, Lakes(i).LakeID, Lakes(i).SubBasinID, Lakes(i).SurfaceArea, .FullDepth, .Depth, .SedimentHg, .WaterHg, .BenthicHg)
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard280()
        sw.WriteLine("c280 LAKE-CLIMATE STATION INFORMATION")
        sw.WriteLine("c")
        sw.WriteLine("c index   sequence number")
        sw.WriteLine("c lkid    lake id (grid value)")
        sw.WriteLine("c clsid   climate station id (grid value)")
        sw.WriteLine("c frac    fraction contribution of the climate station to the lake, (0-1)")
        sw.WriteLine("c")
        sw.WriteLine("c index   lkid    clsid   frac")

        Dim cnt As Integer = 1
        For i As Integer = 0 To Lakes.Count - 1
            With Lakes(i)
                For j As Integer = 0 To .ClimStaFrac.Count - 1
                    Write(cnt, .LakeID, .ClimStaFrac(j).StaID, Math.Round(.ClimStaFrac(j).Frac, 3))
                    cnt += 1
                Next
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard290()
        sw.WriteLine("c290 ROUTING NETWORK")
        sw.WriteLine("c")
        sw.WriteLine("c from        swsid (outlet)")
        sw.WriteLine("c to          swsid (outlet)")
        sw.WriteLine("c traveltime  travel time from outlet to outlet (hr)")
        sw.WriteLine("c")
        sw.WriteLine("c from    to  traveltime")
        For Each tt As clsTravelTime In TravelTimes
            With tt
                Write(.FromID, .ToID, Math.Round(.TravelTime, 2))
            End With
        Next
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard300()
        sw.WriteLine("c300 WATERSHED HYDROLOGY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c swfg    soil water flag")
        sw.WriteLine("c         if = 1 constant value")
        sw.WriteLine("c         if = 2 input grid")
        sw.WriteLine("c         if = 3 field capacity")
        sw.WriteLine("c swater  initial soil water if swfg=1 otherwise 0 (cm/m)")
        sw.WriteLine("c isnow   initial snow on land (cm on water)")
        sw.WriteLine("c grow_a  5day precipitation parameter a for growing season (cm)")
        sw.WriteLine("c grow_b  5day precipitation parameter b for growing season (cm)")
        sw.WriteLine("c ngrow_a 5day precipitation parameter a for non growing season (cm)")
        sw.WriteLine("c ngrow_b 5day precipitation parameter b for non growing season (cm)")
        sw.WriteLine("c usdepth unsaturated soil depth (m)")
        sw.WriteLine("c brdepth soil depth to bed rock (m)")
        sw.WriteLine("c gwater  initial shallow ground water (cm/m)")
        sw.WriteLine("c gr      groundwater recession coefficient (/day)")
        sw.WriteLine("c sr      groundwater seepage coefficient (/day)")
        sw.WriteLine("c gwrp    average groundwater recharge period (days)")
        sw.WriteLine("c")
        sw.WriteLine("c swfg    swater  isnow   grow_a  grow_b  ngrow_a ngrow_b usdepth brdepth gwater  gr  sr  gwrp")
        With WatershedParms
            Write(.SoilWaterFlag, .InitSoilWater, .InitSnow, .GrowA, .GrowB, .NGrowA, .NGrowB, .USDepth, .BRDepth, .GWInit, .GWRecess, .GWSeepage, .GWRecharge)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard310()
        sw.WriteLine("c310 LAKE HYDROLOGY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c ks      lake infiltration rate (cm/hr)")
        sw.WriteLine("c evap_c  evaporation coefficient used to compute AET from lake (AET = evap_c * PET")
        sw.WriteLine("c orif_h  orifice depth (m)")
        sw.WriteLine("c orif_d  orifice diameter (m)")
        sw.WriteLine("c orif_c  orifice coefficient of discharge")
        sw.WriteLine("c weir_l  length of the weir crest (m)")
        sw.WriteLine("c weir_c  weir coefficient of discharge")
        sw.WriteLine("c")
        sw.WriteLine("c ks  evap_c  orif_h  orif_d  orif_c  weir_l  weir_c")
        With LakeParms
            Write(.Infilt, .Evap, .OrifH, .OrifD, .OrifC, .WeirL, .WeirC)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard320()
        sw.WriteLine("c320 WATERSHED SEDIMENT PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c psed_init   initial sediment on pervious land (kg/ha)")
        sw.WriteLine("c ised_init   initial sediment on impervious land (kg/ha)")
        sw.WriteLine("c ised_acc    sediment accumulation rate on impervious land (kg/ha/day)")
        sw.WriteLine("c ised_depl   sediment depletion rate constant on impervious land (/day)")
        sw.WriteLine("c sed_cap     sediment yield capacity on land (kg/ha)")
        sw.WriteLine("c rain_alph   fraction of daily rainfall that occurs during the time of concentration")
        sw.WriteLine("c sdr_alph    calibration coefficient for computing sediment delivery ratio")
        sw.WriteLine("c sdr_beta    routing coefficient for computing sediment delivery ratio")
        sw.WriteLine("c")
        sw.WriteLine("c psed_init   ised_init   ised_acc    ised_depl   sed_cap rain_alph   sdr_alph    sdr_beta")
        With WatershedSedParms
            Write(.InitPerv, .InitImperv, .Accum, .Deplet, .Capacity, .RainAlpha, .SDRAlpha, .SDRBeta)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard330()
        sw.WriteLine("c330 LAKE SEDIMENT PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c tss_eq      equilibrium concentration of suspended solids in the water (g/m3)")
        sw.WriteLine("c sett_k      settling constant rate (/day)")
        sw.WriteLine("c tss_clay    fraction of clay in the inflow sediment")
        sw.WriteLine("c tss_silt    fraction of silt in the inflow sediment")
        sw.WriteLine("c tss_sand    fraction of sand in the inflow sediment")
        sw.WriteLine("c")
        sw.WriteLine("c tss_eq  sett_k  tss_clay    tss_silt    tss_sand")
        With LakeSedParms
            Write(.TssEquil, .SettleDecay, .Clay, .Silt, .Sand)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard340()
        sw.WriteLine("c340 AIR DEPOSITION MERCURY PARAMETERS")
        sw.WriteLine("c adfg    air deposition mercury flag")
        sw.WriteLine("c         if = 1 constant value")
        sw.WriteLine("c         if = 2 input grid")
        sw.WriteLine("c         if = 3 time series")
        sw.WriteLine("c")
        sw.WriteLine("c 'if adfg = 1'")
        sw.WriteLine("c adfg    air deposition mercury flag")
        sw.WriteLine("c dd_f    daily dry deposition mercury flux (µg/m2/day)")
        sw.WriteLine("c wdfg    wet deposition mercury flag")
        sw.WriteLine("c         if = 1 daily wet deposition mercury flux (µg/m2/day)")
        sw.WriteLine("c         if = 2 daily precipitation mercury concentration (ng/l)")
        sw.WriteLine("c wd_v    daily wet deposition mercury based on wdfg")
        sw.WriteLine("c")
        sw.WriteLine("c adfg    dd_f    wdfg    wd_v")
        sw.WriteLine("c")
        sw.WriteLine("c 'if adfg = 2'")
        sw.WriteLine("c adfg    air deposition mercury flag")
        sw.WriteLine("c dd_m    daily dry deposition mercury grid multiplier")
        sw.WriteLine("c wd_m    daily wet deposition mercury grid multiplier")
        sw.WriteLine("c")
        sw.WriteLine("c adfg    dd_m    wd_m")
        sw.WriteLine("c")
        sw.WriteLine("c 'if adfg = 3'")
        sw.WriteLine("c adfg    air deposition mercury flag")
        sw.WriteLine("c dd_d    dry deposition mercury time series data type")
        sw.WriteLine("c         if = 1 daily data")
        sw.WriteLine("c         if = 2 monthly data")
        sw.WriteLine("c wd_d    wet deposition mercury time series data type")
        sw.WriteLine("c         if = 1 daily data")
        sw.WriteLine("c         if = 2 monthly data")
        sw.WriteLine("c")
        sw.WriteLine("c adfg    dd_d    wd_d")
        With MercuryDepo
            Select Case .AirDepoFlag
                Case enumDepoFlag.Constant
                    Write(.AirDepoFlag, .DryDepoFlux, .WetDepoFlag, IIf(.WetDepoFlag = enumWetDepo.WetDepo, .WetDepoFlux, .WetDepoPrecip))
                Case enumDepoFlag.InputGrid
                    Write(.AirDepoFlag, .DryDepoMult, .WetDepoMult)
                Case enumDepoFlag.TimeSeries
                    Write(.AirDepoFlag, .DryDepoStep, .WetDepoStep)
            End Select
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard350()
        sw.WriteLine("c350 WATERSHED MERCURY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c shgfg       soil mercury flag")
        sw.WriteLine("c             if = 1 initial soil mercury constant concentration(ng/g)")
        sw.WriteLine("c             if = 2 initial soil mercury grid multiplier")
        sw.WriteLine("c smercury    value based on shgfg")
        sw.WriteLine("c gwmercury   initial groundwater mercury concentration(ng/l)")
        sw.WriteLine("c zd          watershed soil mixing depth (cm)")
        sw.WriteLine("c zr          soil reduction depth (cm)")
        sw.WriteLine("c kds         soil water partition coefficient (ml/g)")
        sw.WriteLine("c krs         soil base reduction rate (per day)")
        sw.WriteLine("c ef          pollutant enrichment factor")
        sw.WriteLine("c rd          bedrock density (g/cm3)")
        sw.WriteLine("c kcw         chemical weathering rate constant (µm/day")
        sw.WriteLine("c crock       concentration of mercury in bedrock (ng/g")
        sw.WriteLine("c kd          mercury decay rate in channel (per hour)")
        sw.WriteLine("c fmehg       fraction of methylmercury in total mercury")
        sw.WriteLine("c")
        sw.WriteLine("c shgfg   smercury    gwmercury   zd  zr  kds krs ef  rd  kcw crock   kd  femhg")
        With WatershedMercuryParms
            Write(.SoilFlag, IIf(.SoilFlag = enumMercFlag.InitConc, .InitSoil, .GridMult), String.Format("{0:0.000000}", .InitGW), .SoilMixing, .SoilReductionDepth, .SoilPartition, .SoilBaseReductionRate, .Enrich, .BedrockDensity, .Weathering, .InitRock, .ChannelDecay, .Fraction)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard360()
        sw.WriteLine("c360 FOREST MERCURY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c aaet    actual annual evapotranspiration (cm/year)")
        sw.WriteLine("c fint    interception fraction")
        sw.WriteLine("c fadh    adhering fraction")
        sw.WriteLine("c lit     initial amount of litter (g/m2)")
        sw.WriteLine("c bcf     air-plant bio-concentration factor")
        sw.WriteLine("c ca      air mercury concentration (ng/g)")
        sw.WriteLine("c")
        sw.WriteLine("c aaet    fint    fadh    lit bcf  ca")
        With ForestMercuryParms
            Write(.AET, .IntFrac, .AdhereFrac, .Litter, .BioConc, .AirConc)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard370()
        sw.WriteLine("c370 LAKE MERCURY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c alpha_w net reduction loss factor in the water column")
        sw.WriteLine("c beta_w  net methylation loss factor in the water column")
        sw.WriteLine("c kwr     water body mercury reduction rate constant (per day)")
        sw.WriteLine("c kwm     water body mercury methylation rate constant (per day)")
        sw.WriteLine("c vsb     biomass settling velocity in the water column (m/day)")
        sw.WriteLine("c vrs     sediment resuspension velocity (m/day)")
        sw.WriteLine("c kdsw    sediment/water partition coefficient in the water column")
        sw.WriteLine("c kbio    biomass/water partition coefficient in the water column")
        sw.WriteLine("c cbio    biomass concentration in the water column (m2/sec)")
        sw.WriteLine("c pd      soil particle density (cm)")
        sw.WriteLine("c")
        sw.WriteLine("c alpha_w beta_w  kwr kwm vsb vrs kdsw    kbio    cbio    pd")
        With LakeMercuryParms
            Write(.AlphaW, .BetaW, .KWR, .KWM, .VSB, .VRS, .KDSW, .KBio, .CBio, .PD)
        End With
        sw.WriteLine(Blank)
    End Sub

    Private Sub WriteCard380()
        sw.WriteLine("c380 BENTHIC MERCURY PARAMETERS")
        sw.WriteLine("c")
        sw.WriteLine("c alpha_b net reduction loss factor in the benthic sediments")
        sw.WriteLine("c beta_b  net methylation loss factor in the benthic sediments")
        sw.WriteLine("c kbr     benthic sediment mercury reduction rate constant (per day)")
        sw.WriteLine("c kbm     benthic sediment mercury methylation rate constant (per day)")
        sw.WriteLine("c por_bs  porosity of the benthic sediment bed")
        sw.WriteLine("c vbur    burial velocity (m/day)")
        sw.WriteLine("c zb      depth of the benthic sediment bed (m)")
        sw.WriteLine("c kdbs    bed sediment/sediment pore water partition coefficient")
        sw.WriteLine("c cbs     solids concentration in the benthic sediments (g/m3)")
        sw.WriteLine("c esw     pore water diffusion coefficient (m2/sec)")
        sw.WriteLine("c")
        sw.WriteLine("c alpha_b beta_b  kbr kbm por_bs  vbur    zb  kdbs    cbs esw")
        With BenthicMercuryParms
            Write(.AlphaB, .BetaB, .KBR, .KBM, .Por_BS, .VBur, .ZB, .Kdbs, .Cbs, .Esw)
        End With
        sw.WriteLine(Blank)
    End Sub

#End Region

    ''' <summary>
    ''' Load data from input files
    ''' </summary>
    ''' <returns>True if successful</returns>
    ''' <remarks>Data are stored in .inp file (read by C++ computational program) and .GBMM file (additional info needed by preprocessor)</remarks>
    Friend Function Load() As Boolean
        Dim LastCard As Integer
        Dim Line As String

        Try
            sr = New IO.StreamReader(FileName)

            While Not sr.EndOfStream
                Line = sr.ReadLine
                If Line.Length < 4 Then Continue While
                If Line.Length >= 4 AndAlso Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) AndAlso IsNumeric(Line.Substring(1, 3)) Then
                    LastCard = Val(Line.Substring(1, 3))
                ElseIf Line.Length >= 1 AndAlso Not Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Then
                    Select Case LastCard
                        Case 100
                            With SimFlags
                                .Hydro = Read(Line)
                                .Sediment = Read()
                                .Mercury = Read()
                                .Wasp = Read()
                                .WHAEM = Read()
                            End With
                        Case 110
                            With SimPeriods
                                'odd format for this line in sample file; dates are space-separated and there are two tabs after???
                                If Line.Contains(" ") Then
                                    Dim ar As String() = Read(Line).Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
                                    .StartDate = ar(0)
                                    .EndDate = ar(1)
                                    Read()
                                Else
                                    .StartDate = Read(Line)
                                    .EndDate = Read()
                                End If
                                .StartMonth = Read()
                                .EndMonth = Read()
                                .DeltaT = Read()
                            End With
                        Case 120
                            Do
                                Dim dataindex As Integer = Read(Line)
                                With Tables
                                    With CType(Choose(dataindex, .Climate, .HgDryDep, .HgWetDep, .PointSource), clsTable)
                                        .StartDate = Read()
                                        .EndDate = Read()
                                    End With
                                End With
                                Line = sr.ReadLine
                            Loop Until Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Or sr.EndOfStream
                        Case 130
                            Do
                                Dim dataindex As Integer = Read(Line)
                                With Tables
                                    With CType(Choose(dataindex, .Climate, .HgDryDep, .HgWetDep, .PointSource), clsTable)
                                        .TableName = Read()
                                    End With
                                End With
                                Line = sr.ReadLine
                            Loop Until Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Or sr.EndOfStream
                        Case 140
                            With Folders
                                Dim foldertype As Integer = Read(Line)
                                Dim inp As String = Read()
                                If foldertype = 1 And inp <> "" AndAlso My.Computer.FileSystem.DirectoryExists(inp) Then .InputFolder = inp
                                Read(Line)
                                Dim out As String = Read()
                                If foldertype = 2 And out <> "" AndAlso My.Computer.FileSystem.DirectoryExists(out) Then .OutputFolder = out
                            End With
                        Case 150
                            Do
                                Dim gridindex As Integer = Read(Line)
                                Dim gridtype As enumGridType = Read()
                                With Grids
                                    Select Case gridindex
                                        Case 101
                                            With .ClimateThiessan
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 102
                                            With .LandUse
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 103
                                            With .Soils
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 104
                                            With .Subbasins
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 105
                                            With .CNPerv
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 106
                                            With .CNImperv
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 107
                                            With .TravelTotal
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 108
                                            With .TravelStream
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 109
                                            With .SoilWater
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 110
                                            With .FlowLength
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 111
                                            With .AvgRoughness
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 112
                                            With .AvgSlopeOverland
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                    End Select
                                End With
                                Line = sr.ReadLine
                            Loop Until Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Or sr.EndOfStream
                        Case 160
                            Do
                                Dim gridindex As Integer = Read(Line)
                                Dim gridtype As enumGridType = Read()
                                With Grids
                                    Select Case gridindex
                                        Case 201
                                            With .LSFactor
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                    End Select
                                End With
                                Line = sr.ReadLine
                            Loop Until Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Or sr.EndOfStream
                        Case 170
                            Do
                                Dim gridindex As Integer = Read(Line)
                                Dim gridtype As enumGridType = Read()
                                With Grids
                                    Select Case gridindex
                                        Case 301
                                            With .MercuryLitter
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 302
                                            With .MercuryThiessan
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 303
                                            With .MercuryDryDepo
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 304
                                            With .MercuryWetDepo
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                        Case 305
                                            With .MercurySoil
                                                .Gridtype = gridtype
                                                .LayerName = Read().Replace(".asc", "")
                                            End With
                                    End Select
                                End With
                                Line = sr.ReadLine
                            Loop Until Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Or sr.EndOfStream
                        Case 200
                            'number of basins, soils, etc., are taken from layers, however these counts are helpful when reading rest of data input file
                            With WatershedCntl
                                .NumSubbasins = Read(Line)
                                .NumLandUses = Read()
                                .NumSoils = Read()
                                .NumClimateSta = Read()
                                .NumMercurySta = Read()
                                .NumPointSources = Read()
                                .NumLakes = Read()
                            End With
                        Case 210
                            'watershed IDs and areas taken from layers--do not read this
                            For i As Integer = 1 To WatershedCntl.NumSubbasins - 1
                                sr.ReadLine()
                            Next
                        Case 220
                            Landuses.Clear()
                            For i As Integer = 1 To WatershedCntl.NumLandUses - 1
                                Dim lu As New clsLandUse
                                With lu
                                    Read(Line)
                                    .LuID = Read()
                                    .LuType = Read()
                                    .GrowVcf = Read()
                                    .NGrowVcf = Read()
                                    .CFact = Read()
                                    .PFact = Read()
                                    .LuName = Read()
                                    Landuses.Add(.LuID, lu)
                                End With
                                Line = sr.ReadLine()
                            Next
                        Case 230
                            Soils.Clear()
                            For i As Integer = 1 To WatershedCntl.NumSoils - 1
                                Dim soil As New clsSoil
                                With soil
                                    Read(Line)
                                    .SoilID = Read()
                                    .AWC = Read()
                                    .BD = Read()
                                    .ClayFrac = Read()
                                    .Perm = Read()
                                    .KFact = Read()
                                    Soils.Add(.SoilID, soil)
                                End With
                                Line = sr.ReadLine()
                            Next
                        Case 240
                            ClimateStations.Clear()
                            For i As Integer = 1 To WatershedCntl.NumClimateSta - 1
                                Dim sta As New clsStation
                                With sta
                                    Read(Line)
                                    .StaID = Read()
                                    .StaName = Read()
                                    .Latitude = Read()
                                End With
                                Line = sr.ReadLine()
                            Next
                        Case 250
                            MercuryStations.Clear()
                            For i As Integer = 1 To WatershedCntl.NumMercurySta - 1
                                Dim sta As New clsStation
                                With sta
                                    Read(Line)
                                    .StaID = Read()
                                    .StaName = Read()
                                    MercuryStations.Add(.StaID, sta)
                                End With
                                Line = sr.ReadLine()
                            Next
                        Case 260
                            PointSources.Clear()
                            For i As Integer = 1 To WatershedCntl.NumPointSources - 1
                                Dim sta As New clsStation
                                With sta
                                    Read(Line)
                                    .StaID = Read()
                                    .StaName = Read()
                                    .TravelTime = Read()
                                    PointSources.Add(.StaID, sta)
                                End With
                                Line = sr.ReadLine()
                            Next
                        Case 270
                            'get overall lake setting from first lake in list; individual lake info is computed each time it is saved
                            With LakeParms
                                Read(Line) 'lake index
                                Read() 'lake id
                                Read() 'sub id
                                Read() 'area  
                                'todo: must read area threshold elsewhere!!!
                                .FullDepth = Read()
                                .Depth = Read()
                                .SedimentHg = Read()
                                .WaterHg = Read()
                                .BenthicHg = Read()
                            End With
                            For i As Integer = 1 To WatershedCntl.NumLakes - 1
                                sr.ReadLine()
                            Next
                        Case 280
                            'don't read-is computed each time
                            sr.ReadLine()
                        Case 290
                            TravelTimes.Clear()
                            For i As Integer = 0 To WatershedCntl.NumSubbasins - 1
                                Dim tt As New clsTravelTime
                                With tt
                                    .FromID = Read(Line)
                                    .ToID = Read()
                                    .TravelTime = Read()
                                End With
                                TravelTimes.Add(tt)
                                Line = sr.ReadLine
                            Next
                        Case 300
                            With WatershedParms
                                .SoilWaterFlag = Read(Line)
                                .InitSoilWater = Read()
                                .InitSnow = Read()
                                .GrowA = Read()
                                .GrowB = Read()
                                .NGrowA = Read()
                                .NGrowB = Read()
                                .USDepth = Read()
                                .BRDepth = Read()
                                .GWInit = Read()
                                .GWRecess = Read()
                                .GWSeepage = Read()
                                .GWRecharge = Read()
                            End With
                        Case 310
                            With LakeParms
                                .Infilt = Read(Line)
                                .Evap = Read()
                                .OrifH = Read()
                                .OrifD = Read()
                                .OrifC = Read()
                                .WeirL = Read()
                                .WeirC = Read()
                            End With
                        Case 320
                            With WatershedSedParms
                                .InitPerv = Read(Line)
                                .InitImperv = Read()
                                .Accum = Read()
                                .Deplet = Read()
                                .Capacity = Read()
                                .RainAlpha = Read()
                                .SDRAlpha = Read()
                                .SDRBeta = Read()
                            End With
                        Case 330
                            With LakeSedParms
                                .TssEquil = Read(Line)
                                .SettleDecay = Read()
                                .Clay = Read()
                                .Silt = Read()
                                .Sand = Read()
                            End With
                        Case 340
                            With MercuryDepo
                                'sample file contained non-commented line here???
                                If Line.Contains("adfg") Then Line = sr.ReadLine
                                .AirDepoFlag = Read(Line)
                                Select Case .AirDepoFlag
                                    Case enumDepoFlag.Constant
                                        .DryDepoFlux = Read()
                                        .WetDepoFlag = Read()
                                        .WetDepoFlux = Read()
                                    Case enumDepoFlag.InputGrid
                                        .DryDepoMult = Read()
                                        .WetDepoMult = Read()
                                    Case enumDepoFlag.TimeSeries
                                        .DryDepoStep = Read()
                                        .WetDepoStep = Read()
                                End Select
                            End With
                        Case 350
                            With WatershedMercuryParms
                                .SoilFlag = Read(Line)
                                .InitSoil = Read()
                                .InitGW = Read()
                                .SoilMixing = Read()
                                .SoilReductionDepth = Read()
                                .SoilPartition = Read()
                                .SoilBaseReductionRate = Read()
                                .Enrich = Read()
                                .BedrockDensity = Read()
                                .Weathering = Read()
                                .InitRock = Read()
                                .ChannelDecay = Read()
                                .Fraction = Read()
                            End With
                        Case 360
                            With ForestMercuryParms
                                .AET = Read(Line)
                                .IntFrac = Read()
                                .AdhereFrac = Read()
                                .Litter = Read()
                                .BioConc = Read()
                                .AirConc = Read()
                            End With
                        Case 370
                            With LakeMercuryParms
                                .AlphaW = Read(Line)
                                .BetaW = Read()
                                .KWR = Read()
                                .KWM = Read()
                                .VSB = Read()
                                .VRS = Read()
                                .KDSW = Read()
                                .KBio = Read()
                                .CBio = Read()
                                .PD = Read()
                            End With
                        Case 380
                            With BenthicMercuryParms
                                .AlphaB = Read(Line)
                                .BetaB = Read()
                                .KBR = Read()
                                .KBM = Read()
                                .Por_BS = Read()
                                .VBur = Read()
                                .ZB = Read()
                                .Kdbs = Read()
                                .Cbs = Read()
                                .Esw = Read()
                            End With
                    End Select
                End If
            End While
            sr.Close()

            'additional information for preprocessor is stored in .gbmm file

            Dim PreProcFilename As String = IO.Path.ChangeExtension(FileName, ".gbmm")

            If My.Computer.FileSystem.FileExists(PreProcFilename) Then

                sr = New IO.StreamReader(PreProcFilename)

                While Not sr.EndOfStream
                    Line = sr.ReadLine
                    If Line.Length < 4 Then Continue While
                    If Line.Length >= 4 AndAlso Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) AndAlso IsNumeric(Line.Substring(1, 3)) Then
                        LastCard = Val(Line.Substring(1, 3))
                    ElseIf Line.Length >= 1 AndAlso Not Line.StartsWith("c", StringComparison.OrdinalIgnoreCase) Then
                        Select Case LastCard
                            Case 10 'special card types to store info for preprocessor
                                Layers.Subbasins.LayerName = Read(Line)
                            Case 20
                                Layers.DEM.LayerName = Read(Line)
                                DEMUnits = Read()
                                GridSize = Read()
                                Grids.FlowAccum.LayerName = Read()
                                Grids.FlowDir.LayerName = Read()
                                StreamThreshold = Read()
                            Case 30
                                Layers.Soils.LayerName = Read(Line)
                                Layers.Soils.FieldName = Read()
                                Tables.Soils.TableName = Read()
                            Case 40
                                LanduseType = Read(Line)
                                Layers.Landuse.LayerName = Read()
                                Layers.Landuse.FieldName = Read()
                                Tables.LandUse.TableName = Read()
                                Tables.LandUseCN.TableName = Read()
                            Case 50
                                Layers.Lakes.LayerName = Read(Line)
                                Layers.Lakes.FieldName = Read()
                                LakeParms.AreaThreshold = Read()
                            Case 60
                                Layers.ClimateSta.LayerName = Read(Line)
                                Layers.ClimateSta.FieldName = Read()
                                Tables.Climate.TableName = Read()
                                Layers.PointSources.LayerName = Read()
                                Layers.PointSources.FieldName = Read()
                                Tables.PointSource.TableName = Read()
                                Layers.MercurySta.LayerName = Read()
                            Case 70
                                SimFlags.WHAEMDuration = Read(Line)
                                WatershedMercuryParms.InitSoil = Read()
                                WatershedMercuryParms.GridMult = Read()
                                MercuryDepo.WetDepoFlux = Read()
                                MercuryDepo.WetDepoPrecip = Read()
                                With ForestMercuryParms
                                    .LitterDecomp1 = Read()
                                    .LitterDecomp2 = Read()
                                    .LitterDecomp3 = Read()
                                End With
                            Case 80
                                With LSFactor
                                    .LSFlag = Read(Line)
                                    .MaxSlopeLengthFlag = Read()
                                    .MaxSlopeLength = Read()
                                    .ConstantSlopeLength = Read()
                                End With
                        End Select
                    End If
                End While
            Else
                'see if legacy datafile found; if so, load into dictionary and get some data not found in GBMM input file
                Dim LegacyInput As String = IO.Path.GetDirectoryName(FileName) & "\InputData.txt"
                If My.Computer.FileSystem.FileExists(LegacyInput) Then
                    Dim dictInputData As New Generic.Dictionary(Of String, String)
                    Dim sr2 As New IO.StreamReader(LegacyInput)
                    While Not sr2.EndOfStream
                        Dim ar() As String = sr2.ReadLine.Split(",")
                        If Not dictInputData.ContainsKey(ar(0)) Then dictInputData.Add(ar(0), ar(1))
                    End While
                    sr2.Close()
                    sr2.Dispose()
                    With dictInputData
                        If .TryGetValue("SoilMap", Layers.Soils.LayerName) Then Layers.Soils.FieldName = "MUID"
                        If .TryGetValue("Landuse", Layers.Landuse.LayerName) Then Layers.Landuse.FieldName = "LUCODE"
                        If .TryGetValue("DEM", Layers.DEM.LayerName) Then
                            If Layers.DEM.LayerName.Contains("30") Then GridSize = 30 Else GridSize = 90
                            DEMUnits = enumDEMUnits.Meters
                        End If
                        If .TryGetValue("ClimateStation", Layers.ClimateSta.LayerName) Then Layers.ClimateSta.FieldName = "Sta_ID"
                        If .TryGetValue("PointSources", Layers.PointSources.LayerName) Then Layers.PointSources.FieldName = "Sta_ID"
                        .TryGetValue("NHD", Layers.Streams.LayerName)
                        If .TryGetValue("Lakes", Layers.Lakes.LayerName) Then Layers.Lakes.FieldName = "COMID"

                        If .TryGetValue("LuLookupTable", Tables.LandUse.TableName) Then Tables.LandUse.TableName &= ".dbf"
                        If .TryGetValue("LUcodeCNTable", Tables.LandUseCN.TableName) Then Tables.LandUseCN.TableName &= ".dbf"
                        If .TryGetValue("SoilProperty", Tables.Soils.TableName) Then Tables.Soils.TableName &= ".dbf"
                        .TryGetValue("ClimateDataTextFile", Tables.Climate.TableName)
                        .TryGetValue("PSdataTable", Tables.PointSource.TableName)

                        .TryGetValue("LakesThreshold", LakeParms.AreaThreshold)
                        If .TryGetValue("HgStation", Layers.MercurySta.LayerName) Then Layers.MercurySta.FieldName = "StaID"

                        .TryGetValue("InitialConstantHg", WatershedMercuryParms.InitSoil)
                        .TryGetValue("InitialSoilHgMultiplier", WatershedMercuryParms.GridMult)

                        .TryGetValue("HgWetConstant", MercuryDepo.WetDepoFlux)
                        .TryGetValue("HgWetPrcpConc", MercuryDepo.WetDepoPrecip)

                        .TryGetValue("HgLandKDcomp1", ForestMercuryParms.LitterDecomp1)
                        .TryGetValue("HgLandKDcomp2", ForestMercuryParms.LitterDecomp2)
                        .TryGetValue("HgLandKDcomp3", ForestMercuryParms.LitterDecomp3)
                    End With
                End If
            End If
            sr.Close()

            Return True
        Catch ex As Exception
            ErrorMsg("An error occurred while reading card group " & LastCard & ". The last line read was: " & Line, ex)
            Return False
        Finally
            sr.Close()
            sr.Dispose()
        End Try
    End Function

    Private Function Read(Optional ByVal NewLine As String = "") As String
        Static Index As Integer, Items() As String
        If NewLine <> "" Then
            Index = 0
            Items = NewLine.Split(vbTab)
        End If
        Index += 1
        Return Items(Index)
    End Function

    ''' <summary>
    ''' Return the current gridsize expressed as square kilometers
    ''' </summary>
    Friend Function CellAreaKm() As Double
        Return (GridSize / DistFactor / 1000) ^ 2
    End Function

    Friend Event ShapesSelected(ByVal NumShapes As Integer)

    Friend Sub RaiseShapesSelected(ByVal NumShapes As Integer)
        RaiseEvent ShapesSelected(NumShapes)
    End Sub

    ''' <summary>
    ''' GIRAS land use consists of multiple layers; these shapefiles are contained as field values in the "Land Use Index" layer
    ''' This routine will return the list of shapefiles contained in the GIRAS landuse coverages
    ''' Will return empty list if an error occurs
    ''' </summary>
    ''' <param name="BasinLayerIndex">Layer index associated with subbasins</param>
    Friend Function GIRASLayers(ByVal BasinLayerIndex As Integer) As Generic.List(Of Integer)
        Try
            Const LayerName As String = "Land Use Index", FieldName As String = "COVNAME"
            Dim ShapeFiles As New Generic.List(Of Integer)

            If GisUtil.IsLayer(LayerName) Then
                Dim LayerIndex As Integer = GisUtil.LayerIndex(LayerName)
                If GisUtil.IsField(LayerIndex, FieldName) Then
                    Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, FieldName)
                    For i As Integer = 0 To GisUtil.NumFeatures(LayerIndex) - 1
                        Dim LyrName As String = GisUtil.FieldValue(LayerIndex, i, FieldIndex)
                        Dim LayerFileName As String = IO.Path.GetDirectoryName(GisUtil.LayerFileName(LayerIndex)) & "\landuse\" & LyrName & ".shp"
                        For j As Integer = 0 To GisUtil.NumFeatures(BasinLayerIndex) - 1
                            If GisUtil.OverlappingPolygons(LayerIndex, i, BasinLayerIndex, j) Then
                                If GisUtil.IsLayer(LayerFileName) Then ShapeFiles.Add(GisUtil.LayerIndex(LayerFileName))
                                Exit For
                            End If
                        Next j
                    Next
                End If
            End If
            If ShapeFiles.Count = 0 Then LastErrorMsg = "GIRAS land use layers have not been loaded."
            Return ShapeFiles
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function

End Class

''' <summary>
''' This class is used when I call TauDEM functions which need a callback function
''' </summary>
Public Class clsTkCallback
    Implements TKTAUDEMLib.ItkCallback

    Public Sub [Error](ByVal Key As String, ByVal ErrorCode As Integer, ByVal ErrorMsg As String) Implements TKTAUDEMLib.ItkCallback.Error
        If ProgressForm IsNot Nothing Then
            ProgressForm.Status = ErrorMsg
            ProgressForm.IsCancelled = True
            Logger.Message(ErrorMsg, "TauDEM Error", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End If
    End Sub

    Public Sub Progress(ByVal Key As String, ByVal Percent As Integer, ByVal Message As String) Implements TKTAUDEMLib.ItkCallback.Progress
        ProgressForm.SetProgress(Percent, 100)
    End Sub
End Class

''' <summary>
''' This class is used when I call TauDEM functions which need a callback function
''' </summary>
Public Class clsMWCallback
    Implements MapWinGIS.ICallback

    Public Sub [Error](ByVal Key As String, ByVal ErrorMsg As String) Implements MapWinGIS.ICallback.Error
        If ProgressForm IsNot Nothing Then
            ProgressForm.Status = ErrorMsg
            ProgressForm.IsCancelled = True
        End If
        Logger.Message(ErrorMsg, "TauDEM Error", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
    End Sub

    Public Sub Progress(ByVal Key As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
        ProgressForm.SetProgress(Percent, 100)
    End Sub
End Class

