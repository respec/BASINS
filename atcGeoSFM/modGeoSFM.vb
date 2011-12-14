Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcData
Imports System.Drawing
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports System.Text

Public Module modGeoSFM

    Declare Sub ONELAYERBALANCE Lib "geosfm.dll" (ByVal aFileName As String)
    Declare Sub TWOLAYERBALANCE Lib "geosfm.dll" (ByVal aFileName As String)
    Declare Sub LAGROUTE Lib "geosfm.dll" (ByVal aFileName As String)
    Declare Sub DIFFROUTE Lib "geosfm.dll" (ByVal aFileName As String)
    Declare Sub cungeroute Lib "geosfm.dll" (ByVal aFileName As String)
    'Declare Sub aggregateflow Lib "geosfmstats.dll" ()

    Friend pOutputPath As String

    Friend Function Terrain(ByVal aDEMLayerName As String, ByVal aSubbasinLayerName As String, ByVal aStreamLayerName As String, ByVal aThresh As Integer) As Boolean

        ' ***********************************************************************************************
        ' ***********************************************************************************************
        ' 
        '      Program: terrain.ave
        '
        '      Function: The program checks for the presence of required terrain grids 
        '          and calls up menu items to create those that are missing
        '
        '      Inputs include
        '          a grid of surface elevations (corrected DEM) in meters
        '          a grid of flow direction
        '          a grid of flow accumulation
        '          a grid of flow length
        '          a grid of stream cells only
        '          a grid of stream links
        '          a grid of outlet cells
        '          a grid of subbasins in a basin
        '          a grid of hill length (distance to river channel) in meters
        '          a grid of terrain slope expressed as a percentange   
        '          a coverage of previously delineated subbasins <optional>
        '          a coverage of previously delineated streams <optional>       
        '
        '      Outputs:  
        '          Creates missing input grids for subsequently preprocessing steps
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains all existing grids
        '          the corrected DEM is the required minimum input
        '          (optional) the river and subbasin coverages from an existing delineation
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'TODO: implement subbasins and streams from shapefiles

        Dim lDEMLayerIndex As Integer = GisUtil.LayerIndex(aDEMLayerName)
        Dim lDEMFileName As String = GisUtil.LayerFileName(lDEMLayerIndex)

        'innames = {"Corrected DEM", "Flow Direction Grid", "Flow Accumulation Grid", "Downstream Flow Length", "Stream Grid", "Stream Link Grid", "Outlet Grid", "Subbasin Grid", "Hill Length Grid", "Hill Slope Grid", "Downstream Grid"} 
        'defaults = {"Elevations", "FlowDir", "FlowAcc", "FlowLen",  "Streams", "StrLinks", "Outlets", "Basins", "Hilllength", "Slope", "Downstream"}

        'first make a bgd copy of the tif 
        'lDEMFileName = FilenameNoExt(lDEMFileName) & ".bgd"

        'set nodata value properly
        GisUtil.GridSetNoData(lDEMFileName, -100.0)

        'pit fill 
        Dim lPitFillDEMLayerName As String = "Corrected DEM"
        Dim lPitFillDEMLayerIndex As Integer = 0
        Dim lPitFillDEMFileName As String = ""
        If GisUtil.IsLayer(lPitFillDEMLayerName) Then
            lPitFillDEMLayerIndex = GisUtil.LayerIndex(lPitFillDEMLayerName)
            lPitFillDEMFileName = GisUtil.LayerFileName(lPitFillDEMLayerIndex)
        Else
            Logger.Status("Computing Pit Fill")
            lPitFillDEMFileName = FilenameNoExt(lDEMFileName) & "PitFill.bgd"
            MapWinGeoProc.Hydrology.Fill(lDEMFileName, lPitFillDEMFileName, False)
            GisUtil.AddLayer(lPitFillDEMFileName, lPitFillDEMLayerName)
        End If

        '"Flow Direction Grid" and Slope grid
        Dim lFlowDirGridLayerName As String = "Flow Direction Grid"
        Dim lFlowDirGridLayerIndex As Integer = 0
        Dim lFlowDirGridFileName As String = ""
        Dim lSlopeGridLayerName As String = "Hill Slope Grid"
        Dim lSlopeGridLayerIndex As Integer = 0
        Dim lSlopeGridFileName As String = ""
        If GisUtil.IsLayer(lFlowDirGridLayerName) And GisUtil.IsLayer(lSlopeGridLayerName) Then
            lFlowDirGridLayerIndex = GisUtil.LayerIndex(lFlowDirGridLayerName)
            lFlowDirGridFileName = GisUtil.LayerFileName(lFlowDirGridLayerIndex)
            lSlopeGridLayerIndex = GisUtil.LayerIndex(lSlopeGridLayerName)
            lSlopeGridFileName = GisUtil.LayerFileName(lSlopeGridLayerIndex)
        Else
            Logger.Status("Computing Flow Direction")
            lFlowDirGridFileName = FilenameNoExt(lDEMFileName) & "FlowDir.bgd"
            lSlopeGridFileName = FilenameNoExt(lDEMFileName) & "Slope.bgd"
            Dim lRet As Integer = MapWinGeoProc.Hydrology.D8(lPitFillDEMFileName, lFlowDirGridFileName, lSlopeGridFileName, Nothing)
            If Not GisUtil.IsLayer(lFlowDirGridLayerName) Then
                GisUtil.AddLayer(lFlowDirGridFileName, lFlowDirGridLayerName)
            End If
            If Not GisUtil.IsLayer(lSlopeGridLayerName) Then
                GisUtil.AddLayer(lSlopeGridFileName, lSlopeGridLayerName)
            End If
        End If

        '"Flow Accumulation Grid"
        Dim lFlowAccGridLayerName As String = "Flow Accumulation Grid"
        Dim lFlowAccGridLayerIndex As Integer = 0
        Dim lFlowAccGridFileName As String = ""
        If GisUtil.IsLayer(lFlowAccGridLayerName) Then
            lFlowAccGridLayerIndex = GisUtil.LayerIndex(lFlowAccGridLayerName)
            lFlowAccGridFileName = GisUtil.LayerFileName(lFlowAccGridLayerIndex)
        Else
            Logger.Status("Computing Flow Accumulation")
            lFlowAccGridFileName = FilenameNoExt(lDEMFileName) & "FlowAcc.bgd"
            Dim lRet As Integer = MapWinGeoProc.Hydrology.AreaD8(lFlowDirGridFileName, "", lFlowAccGridFileName, False, False, Nothing)
            GisUtil.AddLayer(lFlowAccGridFileName, lFlowAccGridLayerName)
        End If

        '"Subbasin Grid" from shapefile
        Dim lSubbasinGridLayerName As String = "Subbasin Grid"
        Dim lSubbasinGridLayerIndex As Integer = 0
        Dim lSubbasinGridFileName As String = ""
        If Not GisUtil.IsLayer(lSubbasinGridLayerName) And aSubbasinLayerName <> "<none>" Then
            'compute from input shapefile
            Logger.Status("Computing Subbasin Grid")
            Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinLayerName)
            lSubbasinGridFileName = FilenameNoExt(lDEMFileName) & "Watershed.bgd"
            'GridFromShapefile(lSubbasinLayerIndex, lPitFillDEMFileName, lSubbasinGridFileName)
            GisUtil.AddLayer(lSubbasinGridFileName, lSubbasinGridLayerName)
        End If

        '"Stream Grid" from shapefile
        Dim lStreamGridLayerName As String = "Stream Grid"
        Dim lStreamGridLayerIndex As Integer = 0
        Dim lStreamGridFileName As String = ""
        If Not GisUtil.IsLayer(lStreamGridLayerName) And aStreamLayerName <> "<none>" Then
            'compute from input shapefile
            Logger.Status("Computing Stream Grid")
            Dim lStreamLayerIndex As Integer = GisUtil.LayerIndex(aStreamLayerName)
            lStreamGridFileName = FilenameNoExt(lDEMFileName) & "Stream.bgd"
            'GridFromShapefile(lStreamLayerIndex, lPitFillDEMFileName, lStreamGridFileName)
            GisUtil.AddLayer(lStreamGridFileName, lSubbasinGridLayerName)
        End If

        'Stream and Subbasin grids (if no shapefiles provided)
        If GisUtil.IsLayer(lStreamGridLayerName) And GisUtil.IsLayer(lSubbasinGridLayerName) Then
            lStreamGridLayerIndex = GisUtil.LayerIndex(lStreamGridLayerName)
            lStreamGridFileName = GisUtil.LayerFileName(lStreamGridLayerIndex)
            lSubbasinGridLayerIndex = GisUtil.LayerIndex(lSubbasinGridLayerName)
            lSubbasinGridFileName = GisUtil.LayerFileName(lSubbasinGridLayerIndex)
        Else
            Logger.Status("Computing Stream and Subbasin Grids")
            'need to run taudem if we don't already have the stream and subbasin grids
            Dim lStrahlOrdResultGridFileName As String = FilenameNoExt(lDEMFileName) & "Strahl.bgd"
            Dim lLongUpslopeResultGridFileName As String = FilenameNoExt(lDEMFileName) & "LongUp.bgd"
            Dim lTotalUpslopeResultGridFileName As String = FilenameNoExt(lDEMFileName) & "TotUp.bgd"
            Dim lStreamOrderResultGridFileName As String = FilenameNoExt(lDEMFileName) & "StreamOrder.bgd"
            Dim lTreeDatResultFileName As String = FilenameNoExt(lDEMFileName) & "Tree.Dat"
            Dim lCoordDatResultFileName As String = FilenameNoExt(lDEMFileName) & "Coord.Dat"
            Dim lStreamShapeResultFileName As String = FilenameNoExt(lDEMFileName) & "StreamsShape.shp"
            Dim lSubbasinShapeResultFileName As String = FilenameNoExt(lDEMFileName) & "SubbasinsShape.shp"
            lStreamGridFileName = FilenameNoExt(lDEMFileName) & "Stream.bgd"
            lSubbasinGridFileName = FilenameNoExt(lDEMFileName) & "Watershed.bgd"

            MapWinGeoProc.Hydrology.DelinStreamGrids(lDEMFileName, lPitFillDEMFileName, lFlowDirGridFileName, lSlopeGridFileName, lFlowAccGridFileName, "", lStrahlOrdResultGridFileName, lLongUpslopeResultGridFileName, lTotalUpslopeResultGridFileName, lStreamGridFileName, lStreamOrderResultGridFileName, lTreeDatResultFileName, lCoordDatResultFileName, aThresh, False, False, Nothing)
            MapWinGeoProc.Hydrology.DelinStreamsAndSubBasins(lFlowDirGridFileName, lTreeDatResultFileName, lCoordDatResultFileName, lStreamShapeResultFileName, lSubbasinGridFileName, Nothing)
            MapWinGeoProc.Hydrology.SubbasinsToShape(lFlowDirGridFileName, lSubbasinGridFileName, lSubbasinShapeResultFileName, Nothing)

            If Not GisUtil.IsLayer(lSubbasinGridLayerName) Then
                GisUtil.AddLayer(lSubbasinGridFileName, "Subbasin Grid")
            End If
            If Not GisUtil.IsLayer(lStreamGridLayerName) Then
                GisUtil.AddLayer(lStreamGridFileName, "Stream Grid")
            End If
            If Not GisUtil.IsLayer("Streams") Then
                GisUtil.AddLayer(lStreamShapeResultFileName, "Streams")
            End If
            If Not GisUtil.IsLayer("Subbasins") Then
                GisUtil.AddLayer(lSubbasinShapeResultFileName, "Subbasins")
            End If
        End If

        '"Downstream Flow Length"
        Dim lFlowLenGridLayerName As String = "Downstream Flow Length"
        Dim lFlowLenGridLayerIndex As Integer = 0
        Dim lFlowLenGridFileName As String = ""
        If GisUtil.IsLayer(lFlowLenGridLayerName) Then
            lFlowLenGridLayerIndex = GisUtil.LayerIndex(lFlowLenGridLayerName)
            lFlowLenGridFileName = GisUtil.LayerFileName(lFlowLenGridLayerIndex)
        Else
            Logger.Status("Computing Downstream Flow Length")
            lFlowLenGridFileName = FilenameNoExt(lDEMFileName) & "FlowLen.bgd"
            GisUtil.DownstreamFlowLength(lFlowDirGridFileName, lFlowAccGridFileName, lFlowLenGridFileName)
            GisUtil.AddLayer(lFlowLenGridFileName, lFlowLenGridLayerName)
        End If

        '"Stream Link Grid"
        Dim lStreamLinkGridLayerName As String = "Stream Link Grid"
        Dim lStreamLinkGridLayerIndex As Integer = 0
        Dim lStreamLinkGridFileName As String = ""
        If GisUtil.IsLayer(lStreamLinkGridLayerName) Then
            lStreamLinkGridLayerIndex = GisUtil.LayerIndex(lStreamLinkGridLayerName)
            lStreamLinkGridFileName = GisUtil.LayerFileName(lStreamLinkGridLayerIndex)
        Else
            Logger.Status("Computing Stream Link Grid")
            lStreamLinkGridFileName = FilenameNoExt(lDEMFileName) & "StreamLink.bgd"
            'to the first grid, assign the values at the coresponding cells of the second grid, and save the result as the third
            GisUtil.GridAssignValues(lStreamGridFileName, lSubbasinGridFileName, lStreamLinkGridFileName)
            GisUtil.AddLayer(lStreamLinkGridFileName, lStreamLinkGridLayerName)
        End If

        '"Outlet Grid"
        Dim lOutletGridLayerName As String = "Outlet Grid"
        Dim lOutletGridLayerIndex As Integer = 0
        Dim lOutletGridFileName As String = ""
        If GisUtil.IsLayer(lOutletGridLayerName) Then
            lOutletGridLayerIndex = GisUtil.LayerIndex(lOutletGridLayerName)
            lOutletGridFileName = GisUtil.LayerFileName(lOutletGridLayerIndex)
        Else
            Logger.Status("Computing Outlets Grid")
            lOutletGridFileName = FilenameNoExt(lDEMFileName) & "Outlet.bgd"
            'zonefield = stlVtab.Findfield("Value")
            'maxfac = facgrid.zonalstats( #grid_statype_max, stlgrid, prj.makenull, zoneField, false )
            'outgrid = (facgrid <> maxfac).setnull(stlgrid)
            GisUtil.GridBuildOutlets(lStreamLinkGridFileName, lFlowAccGridFileName, lOutletGridFileName)
            GisUtil.AddLayer(lOutletGridFileName, lOutletGridLayerName)
        End If

        '"Hill Length Grid"
        Dim lHillLenGridLayerName As String = "Hill Length Grid"
        Dim lHillLenGridLayerIndex As Integer = 0
        Dim lHillLenGridFileName As String = ""
        If GisUtil.IsLayer(lHillLenGridLayerName) Then
            lHillLenGridLayerIndex = GisUtil.LayerIndex(lHillLenGridLayerName)
            lHillLenGridFileName = GisUtil.LayerFileName(lHillLenGridLayerIndex)
        Else
            Logger.Status("Computing Hill Length Grid")
            lHillLenGridFileName = FilenameNoExt(lDEMFileName) & "HillLen.bgd"
            Dim lFlowDirHillGridFileName As String = FilenameNoExt(lDEMFileName) & "FlowDirHill.bgd"
            'fdrhill = (strgrid.IsNull)*(fdrgrid)
            'to the first grid, assign null at the corresponding cells of the second grid, and save the result as the third
            GisUtil.GridAssignValuesToNull(lFlowDirGridFileName, lStreamGridFileName, lFlowDirHillGridFileName)
            'hlggrid = fdrhill.FlowLength(Nil, False)
            GisUtil.DownstreamFlowLength(lFlowDirHillGridFileName, lFlowAccGridFileName, lHillLenGridFileName, lStreamGridFileName)
            GisUtil.AddLayer(lHillLenGridFileName, lHillLenGridLayerName)
        End If

        '"Downstream Grid"
        Dim lDownstreamGridLayerName As String = "Downstream Subbasin Grid"
        Dim lDownstreamGridLayerIndex As Integer = 0
        Dim lDownstreamGridFileName As String = ""
        If GisUtil.IsLayer(lDownstreamGridLayerName) Then
            lDownstreamGridLayerIndex = GisUtil.LayerIndex(lDownstreamGridLayerName)
            lDownstreamGridFileName = GisUtil.LayerFileName(lDownstreamGridLayerIndex)
        Else
            Logger.Status("Computing Downstream Grid")
            lDownstreamGridFileName = FilenameNoExt(lDEMFileName) & "Downstream.bgd"
            GisUtil.GridDownstreamSubbasins(lOutletGridFileName, lFlowDirGridFileName, lSubbasinGridFileName, lDownstreamGridFileName)
            GisUtil.AddLayer(lDownstreamGridFileName, lDownstreamGridLayerName)
        End If

        Return True
    End Function

    Friend Function Basin(ByVal aZoneGname As String, ByVal aDemGname As String, ByVal aFacGname As String, ByVal aHlenGname As String, ByVal aRcnGname As String, _
                     ByVal aWhcGname As String, ByVal aDepthGname As String, ByVal aTextureGname As String, ByVal aDrainGname As String, _
                     ByVal aFlowlenGname As String, ByVal aRivlinkGname As String, ByVal aDownGname As String, ByVal aMaxcoverGname As String) As Boolean

        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: basin.ave
        '
        '      Function: 
        '          The program determines the mean values of terrain parameters 
        '          for subbasins within each drainage basin
        '
        '      Inputs include
        '          a grid of subbasins in a basin, 'basins'
        '          a grid of soil water holding capacity in cm, 'whc'
        '          a grid of total soil depth in cm, 'depth'
        '          a grid of soil texture, 'texture'
        '          a grid of hydraulic conductivity in cm/hr, 'ks'
        '          a grid of hill length (distance to subbasin outlet) in meters, 'hilllength'
        '          a grid of flow accumulation (dimensionless), 'flowacc'
        '          a grid of surface elevations (corrected DEM) in m, 'elevations'
        '          a grid of SCS runoff curve numbers expressed as a dimensionless number from 30 to 100, 'rcn'
        '          a grid of downstream flow lengths in m, 'flowlen'
        '          a grid of stream links, 'strlinks'
        '          a grid of downstream basin ids, 'downstream'
        '          a grid of maximum impervious cover as a fraction, 'maxcover'
        '
        '      Outputs:  
        '          a basin parameter file entitled 'basin.txt'
        '          a file with basin ids in the correct computational order entitled 'basin.txt'
        '  
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin grid and other products of terrain analysis
        '          all input soil and land cover grids are in a single directory
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'TODO: Need way to include dam locations

        Dim lDescFile As String = pOutputPath & "describe.txt"
        Dim lOutFile As String = pOutputPath & "basin.txt"
        Dim lRivFile = pOutputPath & "river.txt"
        Dim lOrderFile = pOutputPath & "order.txt"

        ' get input grids
        'defaultlist = { "Basins" , "Elevations" , "FlowAcc" , "Hilllength" , "Rcn" , "Whc" , "Soildepth" , "Texture" , "Ks", "FlowLen", "StrLinks", "Downstream", "Maxcover" }
        'labellist = { "Basin Grid" , "Processed DEM", "Flow Accumulation Grid" , "Hill Length Grid", "Runoff Curve Number Grid", "Water holding Capacity", "Soil Depth Grid" , "Soil Texture Grid" , "Hydraulic Conductivity Grid", "Downstream Flow Length Grid", "Stream Link Grid", "Downstream Basin id Grid", "Max Impervious Cover Grid" }

        Dim lBasinGridIndex As Integer = -1
        If aZoneGname = "<none>" Then
            Logger.Msg("Basin Grid, " + aZoneGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lBasinGridIndex = GisUtil.LayerIndex(aZoneGname)
        End If

        Dim lDemGridIndex As Integer = -1
        If aDemGname = "<none>" Then
            Logger.Msg("DEM Grid, " + aDemGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lDemGridIndex = GisUtil.LayerIndex(aDemGname)
        End If

        Dim lFacGridIndex As Integer = -1
        If aFacGname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + aFacGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lFacGridIndex = GisUtil.LayerIndex(aFacGname)
        End If

        Dim lHlenGridIndex As Integer = -1
        If aHlenGname = "<none>" Then
            Logger.Msg("Hill Length Grid, " + aHlenGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lHlenGridIndex = GisUtil.LayerIndex(aHlenGname)
        End If

        Dim lRcnGridIndex As Integer = -1
        If aRcnGname = "<none>" Then
            Logger.Msg("Runoff Curve Number Grid, " + aRcnGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lRcnGridIndex = GisUtil.LayerIndex(aRcnGname)
        End If

        Dim lWhcGridIndex As Integer = -1
        If aWhcGname = "<none>" Then
            Logger.Msg("Soil Water Holding Capacity Grid, " + aWhcGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lWhcGridIndex = GisUtil.LayerIndex(aWhcGname)
        End If

        Dim lDepthGridIndex As Integer = -1
        If aDepthGname = "<none>" Then
            Logger.Msg("Soil Depth Grid, " + aDepthGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lDepthGridIndex = GisUtil.LayerIndex(aDepthGname)
        End If

        Dim lTextureGridIndex As Integer = -1
        If aTextureGname = "<none>" Then
            Logger.Msg("Soil Texture Grid, " + aTextureGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lTextureGridIndex = GisUtil.LayerIndex(aTextureGname)
        End If

        Dim lDrainGridIndex As Integer = -1
        If aDrainGname = "<none>" Then
            Logger.Msg("Hydraulic Conductivity Grid, " + aDrainGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lDrainGridIndex = GisUtil.LayerIndex(aDrainGname)
        End If

        Dim lFlowlenGridIndex As Integer = -1
        If aFlowlenGname = "<none>" Then
            Logger.Msg("Downstream Flow Length Grid, " + aFlowlenGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lFlowlenGridIndex = GisUtil.LayerIndex(aFlowlenGname)
        End If

        Dim lRivlinkGridIndex As Integer = -1
        If aRivlinkGname = "<none>" Then
            Logger.Msg("Stream Link Grid, " + aRivlinkGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lRivlinkGridIndex = GisUtil.LayerIndex(aRivlinkGname)
        End If

        Dim lDownGridIndex As Integer = -1
        If aDownGname = "<none>" Then
            Logger.Msg("Downstream Basin Id Grid, " + aDownGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lDownGridIndex = GisUtil.LayerIndex(aDownGname)
        End If

        Dim lMaxcoverGridIndex As Integer = -1
        If aMaxcoverGname = "<none>" Then
            Logger.Msg("Maximum Impervious Cover Grid, " + aMaxcoverGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lMaxcoverGridIndex = GisUtil.LayerIndex(aMaxcoverGname)
        End If

        'ToDo:Need way to include dam locations
        '        hasdamfld = basinTable.FindField("HasDam")
        '        If (hasdamfld = nil) Then
        '            basinTable.setEditable(True)
        '            dfldlst = List.make
        ' dfldlst.add(Field.Make("HasDam",#FIELD_BYTE,4,0))
        '            dfldlst.deepclone()
        '            basinTable.Addfields(dfldlst)
        '            basinTable.setEditable(False)
        '            hasdamfld = basinTable.FindField("HasDam")
        '            basinTable.Calculate("0", hasdamfld)
        '        End If

        'try to preempt errors that come from having nodata value incorrectly set in input grids
        GisUtil.GridSetNoData(GisUtil.LayerFileName(lBasinGridIndex), 1.0)
        GisUtil.GridSetNoData(GisUtil.LayerFileName(lRivlinkGridIndex), 1.0)

        GisUtil.StatusShow = False

        Logger.Status("Computing Zonal Statistics for " + aDemGname + "........")
        'find average value of demgthm for each unique value of basingthm
        Dim lDemZonalStats As New atcCollection
        lDemZonalStats = GisUtil.GridZonalStatistics(lBasinGridIndex, lDemGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aFacGname + "........")
        'find average value of facgthm for each unique value of basingthm
        Dim lFacZonalStats As New atcCollection
        lFacZonalStats = GisUtil.GridZonalStatistics(lBasinGridIndex, lFacGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aHlenGname + "........")
        'find average value of hlengthm for each unique value of basingthm
        Dim lHlenZonalStats As New atcCollection
        lHlenZonalStats = GisUtil.GridZonalStatistics(lBasinGridIndex, lHlenGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aRcnGname + "........")
        'find average value of rcngthm for each unique value of basingthm
        Dim lRcnZonalStats As New atcCollection
        lRcnZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lRcnGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aWhcGname + "........")
        'find average value of whcgthm for each unique value of basingthm
        Dim lWhcZonalStats As New atcCollection
        lWhcZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lWhcGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aDepthGname + "........")
        'find average value of depthgthm for each unique value of basingthm
        Dim lDepthZonalStats As New atcCollection
        lDepthZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lDepthGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aTextureGname + "........")
        'find average value of texturegthm for each unique value of basingthm
        Dim lTextureZonalStats As New atcCollection
        lTextureZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lTextureGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aDrainGname + "........")
        'find average value of draingthm for each unique value of basingthm
        Dim lDrainZonalStats As New atcCollection
        lDrainZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lDrainGridIndex)

        'how is this river grid different from the stream link grid?
        'Logger.Status("Computing River Grid......")
        '                rivgrid = (basingrid * ((rivlinkgrid + 1) / (rivlinkgrid + 1))).Int
        '                rivVtab = rivgrid.GetVtab

        Logger.Status("Computing Zonal Statistics for " + aFlowlenGname + "........")
        'find average value of flowlengthm for each unique value of rivgrid
        Dim lRivlenZonalStats As New atcCollection
        lRivlenZonalStats = GisUtil.GridZonalStatistics(lRivlinkGridIndex, lFlowlenGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aFlowlenGname + "........")
        'find average value of flowlengthm for each unique value of basingthm
        Dim lLengthZonalStats As New atcCollection
        lLengthZonalStats = GisUtil.GridZonalStatistics(lBasinGridIndex, lFlowlenGridIndex)

        Logger.Status("Computing Zonal Statistics for river cell elevations........")
        'find average value of demgthm for each unique value of rivgrid
        Dim lRivDemZonalStats As New atcCollection
        lRivDemZonalStats = GisUtil.GridZonalStatistics(lRivlinkGridIndex, lDemGridIndex)

        Logger.Status("Computing Zonal Statistics for downstream basin ids........")
        'find average value of downgthm for each unique value of basingthm
        Dim lDownZonalStats As New atcCollection
        lDownZonalStats = GisUtil.GridZonalStatistics(lBasinGridIndex, lDownGridIndex)

        Logger.Status("Computing Zonal Statistics for " + aMaxcoverGname + "........")
        'find average value of maxcovergthm for each unique value of basingthm
        Dim lMaxCoverZonalStats As New atcCollection
        lMaxCoverZonalStats = GisUtil.GridZonalStatisticsMismatched(lBasinGridIndex, lMaxcoverGridIndex)

        GisUtil.StatusShow = True

        'Sort basins in order of ascending drainage area
        'This ensures that upstream basins are listed before downstream ones
        Dim lBasinIDs As New atcCollection
        Dim lZone As Integer = -1
        Dim lMax As Double = 0.0
        For Each lBasin As atcDataAttributes In lFacZonalStats
            lZone = lBasin.GetDefinedValue("Zone").Value
            lMax = lBasin.GetDefinedValue("Max").Value
            If lZone > -1 Then
                lBasinIDs.Add(lZone, lMax)
            End If
        Next
        lBasinIDs.SortByValue()

        ' Begin writing to the output file

        Dim lSBOut As New StringBuilder
        Dim lSBDesc As New StringBuilder
        Dim lSBRiv As New StringBuilder
        Dim lSBOrder As New StringBuilder

        lSBOut.AppendLine("BasinID,SoilWHC,SoilDepth,Texture,Ks,Area,Interflowlag,HSlope,Baseflowlag,RCNumber,MaxCover,BasinLoss,Pancoeff,TopSoil,Aridity")
        lSBRiv.AppendLine("RiverID,Area,UpArea,RivSlope,RivLength,DownID,RivManning,RiverLoss,RiverPloss,HasDam,HasRating,HasFlowData,Celerity,Diffusion,RivWidth,FlowRef,RunType")
        lSBOrder.AppendLine("BasinID")

        lSBDesc.AppendLine("BASIN CHARACTERISTICS, basin.txt")
        lSBDesc.AppendLine("1 =  BasinID is the subbasin identification number ")
        lSBDesc.AppendLine("2 =  SoilWHC is the soil water holding capacity (mm)")
        lSBDesc.AppendLine("3 =  SoilDepth is the total soil depth (cm)")
        lSBDesc.AppendLine("4 =  Texture is the soil texture (1=Sand,2=Loam,3=Clay,5=Water)")
        lSBDesc.AppendLine("5 =  Ks is the saturated hydraulic conductivity (cm/hr)")
        lSBDesc.AppendLine("6 =  Area is the drainage area of the subbasin (km^2)")
        lSBDesc.AppendLine("7 =  Residence time for the interflow reservoir (days)")
        lSBDesc.AppendLine("8 =  HSlope is the average subbasin slope ")
        lSBDesc.AppendLine("9 =  Residence time for the baseflow reservoir (days)")
        lSBDesc.AppendLine("10 = RCNumber is the SCS runoff curve numbers")
        lSBDesc.AppendLine("11 = MaxCover is the fraction of the subbasin with impervious cover")
        lSBDesc.AppendLine("12 = BasinLoss is the fraction of soil water infiltrating to ground water")
        lSBDesc.AppendLine("13 = Pancoeff is the pan coefficient for correcting PET readings")
        lSBDesc.AppendLine("14 = TopSoil is the fraction of soil layer classified as top soil")
        lSBDesc.AppendLine("15 = Aridity is an aridity index ranging from 1 (arid) to 3 (wet)")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("RIVER CHARACTERISTICS, river.txt")
        lSBDesc.AppendLine("1 =  RiverID is the identification number of the river reach")
        lSBDesc.AppendLine("2 =  Area is the local drainage area of the river reach (km^2)")
        lSBDesc.AppendLine("3 =  UpArea is the total area upstream of the river reach(km^2)")
        lSBDesc.AppendLine("4 =  RivSlope is the average slope of the river reach ")
        lSBDesc.AppendLine("5 =  RivLength is the length of the river reach (m)")
        lSBDesc.AppendLine("6 =  DownID is the identification number of the downstream river reach")
        lSBDesc.AppendLine("7 =  RivManning is the value of Mannings n for the river reach")
        lSBDesc.AppendLine("8 =  RiverLoss is the fraction of river water lost to infiltration ")
        lSBDesc.AppendLine("9 =  RiverPloss is the fraction of river water lost to evaporation ")
        lSBDesc.AppendLine("10 = HasDam indicates whether the river reach contain a dam (1) or not (0)")
        lSBDesc.AppendLine("11 = HasRating indicates whether the river reach has a rating curve (1) or not (0)")
        lSBDesc.AppendLine("12 = HasFlowData indicates whether the river reach has observed flow data (1) or not (0)")
        lSBDesc.AppendLine("13 = Celerity is the velocity of the flood wave through the river reach (m/s)")
        lSBDesc.AppendLine("14 = Diffusion is the flow attenuation (or dispersion) coefficient of the reach (m^2/s) ")
        lSBDesc.AppendLine("15 = RivWidth is the average channel width (m)")
        lSBDesc.AppendLine("16 = FlowRef is the reference flow for the section (m^3/s)")
        lSBDesc.AppendLine("17 = RunType indicates if simulation should proceed from an existing (1) or a new (0) run")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("RESPONSE CHARACTERISTICS, response.txt")
        lSBDesc.AppendLine("basinid, fraction of excess runoff arriving at subbasin outlet in each subsequent time step")
        lSBDesc.AppendLine("basinid, fraction of excess runoff arriving ..............")
        lSBDesc.AppendLine("basinid, fraction of excess runoff arriving ..............")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("COMPUTATION ORDER, order.txt")
        lSBDesc.AppendLine("basinid of the most downstream subbasin")
        lSBDesc.AppendLine("basinid of the 2nd downstream subbasin")
        lSBDesc.AppendLine("basinid of the .......")
        lSBDesc.AppendLine("basinid of the most upstream subbasin")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("RAINFALL CHARACTERISTICS, rain.txt")
        lSBDesc.AppendLine("timestep, rainfall (mm) for each subbasin beginning with the most downstream subbasin")
        lSBDesc.AppendLine("timestep, rainfall (mm) for each subbasin beginning.......")
        lSBDesc.AppendLine("timestep, rainfall (mm) for each subbasin beginning.......")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("EVAPOTRANSPIRATION CHARACTERISTICS, evap.txt")
        lSBDesc.AppendLine("timestep, PET (tenths of mm) for each subbasin beginning with the most downstream subbasin")
        lSBDesc.AppendLine("timestep, PET (tenths of mm) for each subbasin beginning......")
        lSBDesc.AppendLine("timestep, PET (tenths of mm) for each subbasin beginning......")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("BALANCE PARAMETERS, balparam.txt")
        lSBDesc.AppendLine("1 = no. of ordinates of unit hydrograph response")
        lSBDesc.AppendLine("2 = no. of simulation time steps")
        lSBDesc.AppendLine("3 = simulation start year")
        lSBDesc.AppendLine("4 = simulation start day")
        lSBDesc.AppendLine("5 = no. of catchments")
        lSBDesc.AppendLine("6 = simulation interval in hours")
        lSBDesc.AppendLine("7 = data format indicator (1/0)")
        lSBDesc.AppendLine("8 = model initialization  mode (1/0)")
        lSBDesc.AppendLine("9 = initial soil fraction")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("BALANCE FILE LISTING, balfiles.txt")
        lSBDesc.AppendLine("1 = input rainfall file (rain.txt)")
        lSBDesc.AppendLine("2 = input potential evapotranspiration file (evap.txt)")
        lSBDesc.AppendLine("3 = input basin characteristics file (basin.txt)")
        lSBDesc.AppendLine("4 = input unit hydrograph response file (response.txt)")
        lSBDesc.AppendLine("5 = output balance parameter file (balparam.txt)")
        lSBDesc.AppendLine("6 = output runoff yield file (basinrunoffyield.txt)")
        lSBDesc.AppendLine("7 = output soil moisture storage file (soilwater.txt)")
        lSBDesc.AppendLine("8 = output actual evapotranspiration file (actualevap.txt)")
        lSBDesc.AppendLine("9 = output ground water loss file (gwloss.txt)")
        lSBDesc.AppendLine("10 = output final soil water storage file (cswater.txt)")
        lSBDesc.AppendLine("11 = output surface precipitation execess file (excessflow.txt)")
        lSBDesc.AppendLine("12 = output interflow file (interflow.txt)")
        lSBDesc.AppendLine("13 = output baseflow file (baseflow.txt)")
        lSBDesc.AppendLine("14 = output mass balance file (massbalance.txt)")
        lSBDesc.AppendLine("15 = output log file (logfilesoil.txt)")
        lSBDesc.AppendLine("16 = output initialization file (initial.txt)")
        lSBDesc.AppendLine("17 = output workdirectory (<full directory pathname>)")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("ROUTE PARAMETERS, routparam.txt")
        lSBDesc.AppendLine("1 = no. of simulation time steps")
        lSBDesc.AppendLine("2 = simulation start year")
        lSBDesc.AppendLine("3 = simulation start day")
        lSBDesc.AppendLine("4 = no. of catchments")
        lSBDesc.AppendLine("5 = simulation interval in hours")
        lSBDesc.AppendLine("6 = model initialization  mode (1/0)")
        lSBDesc.AppendLine("7 = no. of forecast days")
        lSBDesc.AppendLine("8 = output format 0=yyyjjj, 1=yyyymmddhh")
        lSBDesc.AppendLine("9 = no. of reservoirs")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("ROUTE FILE LISTING, routfiles.txt")
        lSBDesc.AppendLine("1 = route parameter file (routparam.txt)")
        lSBDesc.AppendLine("2 = input river characteristics file (river.txt)")
        lSBDesc.AppendLine("3 = input river initialization file (initial.txt)")
        lSBDesc.AppendLine("4 = input runoff yield file (basinrunoffyield.txt)")
        lSBDesc.AppendLine("5 = input reservoir-river link file (damlink.txt)")
        lSBDesc.AppendLine("6 = input observed flow file (obsflow.txt)")
        lSBDesc.AppendLine("7 = input rating curve flow file (rating.txt)")
        lSBDesc.AppendLine("8 = output streamflow file (streamflow.txt)")
        lSBDesc.AppendLine("9 = output subbasin flow contribution file (localflow.txt)")
        lSBDesc.AppendLine("10 = output average river depth file (riverdepth.txt)")
        lSBDesc.AppendLine("11 = output subbasin upstream flow contribution (inflow.txt)")
        lSBDesc.AppendLine("12 = output flow routing log file (logfileflow.txt)")
        lSBDesc.AppendLine("13 = output workdirectory (<full directory pathname>)")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("TOTAL RUNOFF FROM EACH SUBBASIN, basinrunoffyield.txt")
        lSBDesc.AppendLine("timestep, total runoff (mm) for each subbasin beginning with the most downstream subbasin")
        lSBDesc.AppendLine("timestep, total runoff (mm) for each subbasin beginning.......")
        lSBDesc.AppendLine("timestep, total runoff (mm) for each subbasin beginning.......")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("TOTAL STREAMFLOW FROM EACH SUBBASIN, streamflow.txt")
        lSBDesc.AppendLine("timestep, total discharge (m3/s) for each subbasin beginning with the most downstream subbasin")
        lSBDesc.AppendLine("timestep, total discharge (m3/s) for each subbasin beginning.......")
        lSBDesc.AppendLine("timestep, total discharge (m3/s) for each subbasin beginning.......")
        lSBDesc.AppendLine("")
        lSBDesc.AppendLine("")
        SaveFileString(lDescFile, lSBDesc.ToString)

        Dim lBasinValue As Integer
        For lKey As Integer = lBasinIDs.Keys.Count - 1 To 0 Step -1
            lBasinValue = lBasinIDs.Keys(lKey)

            '   areafield = DemZoneVtab.FindField("area")
            '   demfield = DemZoneVtab.FindField("mean")
            '   demminfield = DemZoneVtab.FindField("min")
            '   facfield = FacZoneVtab.FindField("max")
            '   lenfield = LengthZoneVtab.FindField("mean")
            '   lenminfield = LengthZoneVtab.FindField("min")
            '   hlenfield = HlenZoneVtab.FindField("mean")
            '   drainfield = DrainZoneVtab.FindField("mean")
            '   rivlenfield = rivlenZoneVtab.FindField("range")
            '   rcnfield = RcnZoneVtab.FindField("mean")
            '   whcfield = whcZoneVtab.FindField("mean")
            '   depthfield = DepthZoneVtab.FindField("mean")
            '   texturefield = TextureZoneVtab.FindField("majority")
            '   rivdemfield = RivDemZoneVtab.FindField("range")
            '   Downfield = DownZoneVtab.FindField("majority")
            '   maxcoverfield = MaxCoverZoneVtab.FindField("mean")

            'DemZoneVtab
            Dim lAreaValue As Single = lDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Area").Value / 1000000.0 '((DemZoneVtab.ReturnValue(areafield, rrecord)) / (1000000.0)).SetFormat("d.d").AsString
            Dim lDemValue As Single = lDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value '(DemZoneVtab.ReturnValue(demfield, rrecord)).SetFormat("d.d").AsString
            Dim lAvgdrop As Single = lDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value - lDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Min").Value '(DemZoneVtab.ReturnValue(Demfield, rrecord)) - (DemZoneVtab.ReturnValue(Demminfield, rrecord))

            'FacZoneVtab
            Dim lFacValue As Single = lFacZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Max").Value '(FacZoneVtab.ReturnValue(facfield, rrecord)).AsString

            'HlenZoneVtab
            Dim lHlenValue As String = lHlenZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value '(HlenZoneVtab.ReturnValue(hlenfield, rrecord)).SetFormat("d.d").AsString
            If Not IsNumeric(lHlenValue) Then
                lHlenValue = GisUtil.GridGetCellSizeX(lBasinGridIndex)
            End If
            If (lHlenValue < GisUtil.GridGetCellSizeX(lBasinGridIndex)) Then
                lHlenValue = GisUtil.GridGetCellSizeX(lBasinGridIndex)
            End If

            'LengthZoneVtab
            Dim lAvglength As Single = lLengthZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value - lLengthZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Min").Value '(LengthZoneVtab.ReturnValue(Lenfield, rrecord)) - (LengthZoneVtab.ReturnValue(Lenminfield, rrecord))
            If (lAvglength < GisUtil.GridGetCellSizeX(lBasinGridIndex)) Then
                lAvglength = GisUtil.GridGetCellSizeX(lBasinGridIndex)
            End If

            'DrainZoneVtab
            Dim lDrainValue As Single = Format(lDrainZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "#.##0") '(DrainZoneVtab.ReturnValue(drainfield, rrecord)).SetFormat("d.ddd").AsString

            Dim lUpareaValue As Single = (lFacValue * GisUtil.GridGetCellSizeX(lFacGridIndex) * GisUtil.GridGetCellSizeX(lFacGridIndex)) / 1000000.0  '((facvalue.asstring.asnumber * facgrid.GetCellSize * facgrid.GetCellSize) / 1000000.0).SetFormat("d.d").asstring

            Dim lSlopeValue As String = Format(((lAvgdrop * 100) / lAvglength), "0.####")  '(((avgdrop * 100) / avglength).Format("d.dddd")).AsString
            If Not IsNumeric(lSlopeValue) Then
                lSlopeValue = "0.0010"
            End If
            If CSng(lSlopeValue) < 0.001 Then
                lSlopeValue = "0.0010"
            End If

            'rivlenZoneVtab
            Dim lRiverlossValue As String = "1.0"

            Dim lRivlenValue As String = Format((lRivlenZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Max").Value - lRivlenZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Min").Value), "#.0")  ' (rivlenZoneVtab.ReturnValue(rivlenfield, rrecord)).SetFormat("d.d").AsString
            If Not IsNumeric(lRivlenValue) Then
                lRivlenValue = GisUtil.GridGetCellSizeX(lBasinGridIndex).ToString
            End If
            If CSng(lRivlenValue) < GisUtil.GridGetCellSizeX(lBasinGridIndex) Then
                lRivlenValue = GisUtil.GridGetCellSizeX(lBasinGridIndex).ToString
            End If

            ' Assume porosity of 0.439 for medium soil texture in computing max storage for the catchment
            ' Assume both sides of the river (2 * No. of River Cells) are draining at the saturated rate
            '  under the influence of the average head (= avgdrop) in the catchment
            ' Assume baseflow is 3 times as slow as interflow

            Dim lInterflowlag As String = Format((((lAreaValue) * (lAvgdrop) * GisUtil.GridGetCellSizeX(lBasinGridIndex) * 0.439) / (lRivlenValue * lAvgdrop * lDrainValue * 0.24 * 2)), "#.####")
            If Not IsNumeric(lInterflowlag) Then
                lInterflowlag = "10"
            ElseIf CStr(lInterflowlag) < 2 Then
                lInterflowlag = "2"
            ElseIf CStr(lInterflowlag) > 120 Then
                lInterflowlag = "120"
            End If

            Dim lBaseFlowLag As String = Format((CStr(lInterflowlag) * 3), "#.####")

            'RcnZoneVtab
            Dim lRcnValue As String = Format(lRcnZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "#.0") '(RcnZoneVtab.ReturnValue(Rcnfield, rrecord)).SetFormat("d.d").AsString

            'whcZoneVtab
            Dim lWhcValue As String = Format(lWhcZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "###.###")  '(whcZoneVtab.ReturnValue(Whcfield, rrecord)).AsString

            'DepthZoneVtab
            Dim lDepthValue As String = Format(lDepthZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "###.###") '(DepthZoneVtab.ReturnValue(depthfield, rrecord)).AsString

            'TextureZoneVtab
            Dim lTextureValue As String = lTextureZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mode").Value.ToString '(TextureZoneVtab.ReturnValue(texturefield, rrecord)).AsString

            ' Soil texture (1=Sand,2=Loam,3=Clay,5=Water)
            Dim lBasinLossValue As String = ""
            If lTextureValue = "1" Then
                lBasinLossValue = "0.95"
            ElseIf lTextureValue = "2" Then
                lBasinLossValue = "0.97"
            ElseIf lTextureValue = "3" Then
                lBasinLossValue = "0.99"
            ElseIf lTextureValue = "5" Then
                lBasinLossValue = "0.98"
            Else
                lBasinLossValue = "0.99"
            End If

            'rivdemZoneVtab
            Dim lRivdropValue As Single = lRivDemZonalStats.ItemByKey(lBasinValue).GetValue("Max") - lRivDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Min").Value '(rivdemZoneVtab.ReturnValue(rivdemfield, rrecord))
            Dim lRivSlopeValue As String = Format((lRivdropValue * 100) / CStr(lRivlenValue), "0.0000")   '(((rivdropvalue * 100) / CStr(rivlenvalue)).SetFormat("d.dddd")).AsString
            If Not IsNumeric(lRivSlopeValue) Then
                lRivSlopeValue = "0.0010"
            End If
            If CStr(lRivSlopeValue) < 0.001 Then
                lRivSlopeValue = "0.0010"
            End If

            Dim lCelerity As String = ""
            Dim lDiffusion As String = ""
            If CSng(lRivSlopeValue) < 0.1 Then

                If CSng(lUpareaValue) <= 10000 Then
                    lCelerity = "0.3"
                ElseIf CSng(lUpareaValue) <= 50000 Then
                    lCelerity = "0.45"
                ElseIf CSng(lUpareaValue) <= 100000 Then
                    lCelerity = "0.6"
                ElseIf CSng(lUpareaValue) <= 250000 Then
                    lCelerity = "0.75"
                ElseIf CSng(lUpareaValue) <= 500000 Then
                    lCelerity = "0.9"
                ElseIf CSng(lUpareaValue) <= 750000 Then
                    lCelerity = "1.2"
                Else
                    lCelerity = "1.5"
                End If

                lDiffusion = Format(0.15 * CSng(lCelerity) * CSng(lRivlenValue), "#.0")  '(((0.15 * (CSng(celerity)) * (CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(lDiffusion) < 100.0) Then
                    lDiffusion = "100.0"
                ElseIf (CSng(lDiffusion) > 10000.0) Then
                    lDiffusion = "10000.0"
                End If

            ElseIf CSng(lRivSlopeValue) < 0.2 Then

                If (CSng(lUpareaValue) <= 10000) Then
                    lCelerity = "0.4"
                ElseIf (CSng(lUpareaValue) <= 50000) Then
                    lCelerity = "0.6"
                ElseIf (CSng(lUpareaValue) <= 100000) Then
                    lCelerity = "0.8"
                ElseIf (CSng(lUpareaValue) <= 250000) Then
                    lCelerity = "1.0"
                ElseIf (CSng(lUpareaValue) <= 500000) Then
                    lCelerity = "1.2"
                ElseIf (CSng(lUpareaValue) <= 750000) Then
                    lCelerity = "1.6"
                Else
                    lCelerity = "2.0"
                End If

                lDiffusion = Format(0.15 * CSng(lCelerity) * CSng(lRivlenValue), "#.0") '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(lDiffusion) < 100.0) Then
                    lDiffusion = "100.0"
                ElseIf (CSng(lDiffusion) > 10000.0) Then
                    lDiffusion = "10000.0"
                End If

            ElseIf CSng(lRivSlopeValue) < 0.3 Then

                If (CSng(lUpareaValue) <= 10000) Then
                    lCelerity = "0.6"
                ElseIf (CSng(lUpareaValue) <= 50000) Then
                    lCelerity = "0.9"
                ElseIf (CSng(lUpareaValue) <= 100000) Then
                    lCelerity = "1.2"
                ElseIf (CSng(lUpareaValue) <= 250000) Then
                    lCelerity = "1.5"
                ElseIf (CSng(lUpareaValue) <= 500000) Then
                    lCelerity = "1.8"
                ElseIf (CSng(lUpareaValue) <= 750000) Then
                    lCelerity = "2.4"
                Else
                    lCelerity = "3.0"
                End If

                lDiffusion = Format(0.15 * CSng(lCelerity) * CSng(lRivlenValue), "#.0") '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(lDiffusion) < 100.0) Then
                    lDiffusion = "100.0"
                ElseIf (CSng(lDiffusion) > 10000.0) Then
                    lDiffusion = "10000.0"
                End If

            ElseIf CSng(lRivSlopeValue) < 0.4 Then

                If (CSng(lUpareaValue) <= 10000) Then
                    lCelerity = "0.8"
                ElseIf (CSng(lUpareaValue) <= 50000) Then
                    lCelerity = "1.2"
                ElseIf (CSng(lUpareaValue) <= 100000) Then
                    lCelerity = "1.6"
                ElseIf (CSng(lUpareaValue) <= 250000) Then
                    lCelerity = "2.0"
                ElseIf (CSng(lUpareaValue) <= 500000) Then
                    lCelerity = "2.4"
                ElseIf (CSng(lUpareaValue) <= 750000) Then
                    lCelerity = "3.2"
                Else
                    lCelerity = "4.0"
                End If

                lDiffusion = Format(0.15 * CSng(lCelerity) * CSng(lRivlenValue), "#.0")  '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(lDiffusion) < 100.0) Then
                    lDiffusion = "100.0"
                ElseIf (CSng(lDiffusion) > 10000.0) Then
                    lDiffusion = "10000.0"
                End If

            Else

                If (CSng(lUpareaValue) <= 10000) Then
                    lCelerity = "1.0"
                ElseIf (CSng(lUpareaValue) <= 50000) Then
                    lCelerity = "1.5"
                ElseIf (CSng(lUpareaValue) <= 100000) Then
                    lCelerity = "2.0"
                ElseIf (CSng(lUpareaValue) <= 250000) Then
                    lCelerity = "2.5"
                ElseIf (CSng(lUpareaValue) <= 500000) Then
                    lCelerity = "3.0"
                ElseIf (CSng(lUpareaValue) <= 750000) Then
                    lCelerity = "4.0"
                Else
                    lCelerity = "5.0"
                End If

                lDiffusion = Format(0.15 * CSng(lCelerity) * CSng(lRivlenValue), "#.0")  '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(lDiffusion) < 100.0) Then
                    lDiffusion = "100.0"
                ElseIf (CSng(lDiffusion) > 10000.0) Then
                    lDiffusion = "10000.0"
                End If

            End If

            'DownZoneVtab
            Dim lDownValue As String = lDownZonalStats.ItemByKey(lBasinValue).GetValue("Mode", "").ToString '(DownZoneVtab.ReturnValue(downfield, rrecord)).AsString

            'MaxCoverZoneVtab
            Dim lMaxcoverValue As String = Format((lMaxCoverZonalStats.ItemByKey(lBasinValue).GetValue("Mean", GetNaN) / 100), "0.####") '((MaxCoverZoneVtab.ReturnValue(maxcoverfield, rrecord)) / 100).SetFormat("d.ddddd").AsString
            If lMaxcoverValue.Length = 0 OrElse CSng(lMaxcoverValue) <= 0.001 Then
                lMaxcoverValue = "0.001"
            ElseIf CSng(lMaxcoverValue) >= 1 Then
                lMaxcoverValue = "1.0"
            End If

            Dim lHasDamValue As String = "0" '(BasinTable.ReturnValue(hasdamfld, rrecord)).SetFormat("d").AsString
            Dim lRivPolyLoss As String = "1.0"
            Dim lHasRating As String = "0"
            Dim lHasFlowData As String = "0"
            Dim lRivWidth As String = Format(6.13 * (CSng(lUpareaValue) ^ (0.347)), "#.###")
            Dim lFlowRef As String = Format(36 * 0.02832 * ((CSng(lAreaValue) / 2.59) ^ (0.68)), "#.####")
            Dim lRunType As String = "0"
            Dim lMannValue As String = "0.035"
            Dim lPanCoef As String = "0.85"
            Dim lTopSoil As String = "0.1"
            Dim lAridity As String = "2"
            lSBOut.AppendLine(CStr(lBasinValue) + "," + lWhcValue + "," + lDepthValue + "," + lTextureValue + "," + CStr(lDrainValue) + "," + Format(lAreaValue, "#.0") + "," + lInterflowlag + "," + lSlopeValue + "," + lBaseFlowLag + "," + lRcnValue + "," + lMaxcoverValue + "," + lBasinLossValue + "," + lPanCoef + "," + lTopSoil + "," + lAridity)
            lSBRiv.AppendLine(CStr(lBasinValue) + "," + Format(lAreaValue, "#.0") + "," + Format(lUpareaValue, "#.0") + "," + lRivSlopeValue + "," + lRivlenValue + "," + lDownValue + "," + lMannValue + "," + lRiverlossValue + "," + lRivPolyLoss + "," + lHasDamValue + "," + lHasRating + "," + lHasFlowData + "," + lCelerity + "," + lDiffusion + "," + lRivWidth + "," + lFlowRef + "," + lRunType)
            lSBOrder.AppendLine(CStr(lBasinValue))
        Next

        SaveFileString(lOutFile, lSBOut.ToString)
        SaveFileString(lRivFile, lSBRiv.ToString)
        SaveFileString(lOrderFile, lSBOrder.ToString)

        If FileExists(pOutputPath & "basin_original.txt") Then
            IO.File.Delete(pOutputPath & "basin_original.txt")
        End If
        IO.File.Copy(lOutFile, pOutputPath & "basin_original.txt")

        If FileExists(pOutputPath & "river_original.txt") Then
            IO.File.Delete(pOutputPath & "river_original.txt")
        End If
        IO.File.Copy(lRivFile, pOutputPath & "river_original.txt")

        Logger.Msg("Basin Characteristics Computed. Outputs written to: " & vbCrLf & vbCrLf & "      " & lOutFile & vbCrLf & "      " & lOrderFile & vbCrLf & "      " & lRivFile, "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function Response(ByVal aVelType As Integer, _
                        ByVal aZoneGname As String, ByVal aFlowlenGname As String, _
                        ByVal aOutletGname As String, ByVal aDemGname As String, ByVal aFacGname As String, ByVal aFdirGname As String, _
                        ByVal aUsgsLandCoverGname As String, ByVal aOverlandFlowVelocity As Double, ByVal aInstreamFlowVelocity As Double, _
                        ByVal aManningsValues As atcCollection) As Boolean

        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: response.ave
        '
        '      Function: 
        '          The program calculates overland flow velocity based on landcover and 
        '          slope and computes a response function (hydrograph) for each subbasin
        '
        '      Inputs include
        '          a grid of subbasins in a basin
        '          a grid of terrain slope expressed as a decimal
        '          a grid of flow direction (8 pourpoint model)
        '          a grid of surface elevations (corrected DEM)
        '          a USGS Land Cover grid or User Specified Velocity grid or User Specified Velocity
        '          a grid of downstream flow lengths
        '          a grid of stream outlets
        '
        '      Outputs:  
        '          a single output data file entitled 'response.txt'. This file details the 
        '          fraction of input flow that reaches the subbasin outlet during each subsequent day
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains all the input grids
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'velmethodlist = { "Non-Uniform from USGS Land Cover Grid" , "Non-Uniform with User Supplied Velocities", "Uniform from User Supplied Velocity Value" }
        'If veltype = 1 Then
        '    ' "Non-Uniform from USGS Land Cover Grid"
        '    '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Outlets", "Elevations", "Flowacc", "Usgslandcov", myWkdirname + "order.txt" }
        'ElseIf veltype = 2 Then
        '    ' "Non-Uniform with User Supplied Velocities"
        '    '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Flowacc", "Outlets", "0.05", "0.5", myWkdirname + "order.txt" }
        '    '            'labellist = { "Basin Grid" , "Flow Direction Grid", "Flow Length Grid", "Flow Accumulation Grid", "Stream Outlet Grid", "Overland Flow Velocity (m/s)", "Instream Flow Velocity (m/s)", "Computational Order File" }
        'Else
        '    ' Uniform Velocity Value
        '    '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Outlets", "0.3", myWkdirname + "order.txt" }
        '    '            'labellist = { "Basin Grid" , "Flow Direction Grid", "Flow Length Grid", "Stream Outlet Grid", "Overland Flow Velocity (m/s)", "Computational Order File" }
        'End If

        Dim lOrderFileName As String = pOutputPath & "order.txt"

        Dim lBasinGthm As Integer = -1
        If aZoneGname = "<none>" Then
            Logger.Msg("Basin Grid, " + aZoneGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lBasinGthm = GisUtil.LayerIndex(aZoneGname)
        End If

        Dim lFlowdirGthm As Integer = -1
        If aFdirGname = "<none>" Then
            Logger.Msg("Flow Direction Grid, " + aFdirGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lFlowdirGthm = GisUtil.LayerIndex(aFdirGname)
        End If
        Dim lFlowDirGridFileName As String = GisUtil.LayerFileName(lFlowdirGthm)

        Dim lFlowlenGthm As Integer = -1
        If aFlowlenGname = "<none>" Then
            Logger.Msg("Flow Length Grid, " + aFlowlenGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lFlowlenGthm = GisUtil.LayerIndex(aFlowlenGname)
        End If

        Dim lOutGthm As Integer = -1
        If aOutletGname = "<none>" Then
            Logger.Msg("Outlet Grid, " + aOutletGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lOutGthm = GisUtil.LayerIndex(aOutletGname)
        End If

        Dim lZoneList As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lOrderFileName)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lZoneList.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Basin Characteristics' to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lFacGthm As Integer = -1
        If aFacGname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + aFacGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lFacGthm = GisUtil.LayerIndex(aFacGname)
        End If
        Dim lFlowAccGridFileName As String = GisUtil.LayerFileName(lFacGthm)

        Dim lDemGthm As Integer = -1
        If aDemGname = "<none>" Then
            Logger.Msg("DEM Grid, " + aDemGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Return False
        Else
            lDemGthm = GisUtil.LayerIndex(aDemGname)
        End If

        Dim lCellSize As Integer = GisUtil.GridGetCellSizeX(lBasinGthm)
        Dim lZoneLayerIndex As Integer = GisUtil.LayerIndex(aZoneGname)
        Dim lZoneFileName As String = GisUtil.LayerFileName(lZoneLayerIndex)
        Dim lDemFileName As String = GisUtil.LayerFileName(lDemGthm)
        Dim lVelocityGridLayerName As String = "Velocity Grid"
        Dim lVelocityGridLayerIndex As Integer = 0
        Dim lVelocityGridFileName As String = ""

        If (aVelType = 1) Then

            '  meandrop = demgrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false) 
            '  mindrop = demgrid.zonalStats(#GRID_STATYPE_MIN, basingrid, Prj.MakeNull, basinField, false)
            Logger.Status("Computing Zonal Statistics for " + aDemGname + "........")
            Dim lDemZonalStats As New atcCollection
            lDemZonalStats = GisUtil.GridZonalStatistics(lBasinGthm, lDemGthm)

            '  avgraw = flowlengrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false)
            Logger.Status("Computing Zonal Statistics for " + aFlowlenGname + "........")
            Dim lFlowLenZonalStats As New atcCollection
            lFlowLenZonalStats = GisUtil.GridZonalStatistics(lBasinGthm, lFlowlenGthm)

            'build a collection of the subbasin ids 
            Dim lBasinIDs As New atcCollection
            Dim lZone As Integer = -1
            Dim lMax As Double = 0.0
            For Each lBasin As atcDataAttributes In lDemZonalStats
                lZone = lBasin.GetDefinedValue("Zone").Value
                If lZone > -1 Then
                    lBasinIDs.Add(lZone)
                End If
            Next

            Dim lSlopeBySubbasin As New atcCollection
            For Each lBasinID As Integer In lBasinIDs
                Dim lMeanDrop As Single = lDemZonalStats.ItemByKey(lBasinID).GetDefinedValue("Mean").Value
                Dim lMinDrop As Single = lDemZonalStats.ItemByKey(lBasinID).GetDefinedValue("Min").Value
                'avgdrop = meandrop - mindrop
                Dim lAvgDrop As Single = lMeanDrop - lMinDrop
                Dim lAvgRaw As Single = lFlowLenZonalStats.ItemByKey(lBasinID).GetDefinedValue("Mean").Value
                'avglength = (avgraw < mycellsize).con(mycellsize.asgrid, avgraw)
                Dim lAvgLength As Single = 0.0
                'Grid.Con(Yes,No) 
                If lAvgRaw < lCellSize Then
                    lAvgLength = lCellSize
                Else
                    lAvgLength = lAvgRaw
                End If
                Dim lSlopeRaw As Single = ((lAvgDrop * 100) / lAvgLength)
                Dim lSlopeVal As Single
                'slopegrid = (sloperaw < 0.001).con(0.001.asgrid, sloperaw)
                If lSlopeRaw < 0.001 Then
                    lSlopeVal = 0.001
                Else
                    lSlopeVal = lSlopeRaw
                End If
                lSlopeBySubbasin.Add(lBasinID, lSlopeVal)
            Next

            Dim lLcovGthm As Integer = -1
            If aUsgsLandCoverGname = "<none>" Then
                Logger.Msg("USGS Land Cover Grid, " + aUsgsLandCoverGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
                Return False
            Else
                lLcovGthm = GisUtil.LayerIndex(aUsgsLandCoverGname)
            End If

            '  Mannlist = { "0.03","0.03","0.035","0.033","0.035","0.04","0.05","0.05","0.05","0.06","0.1","0.1","0.12","0.12","0.1","0.035","0.05","0.05","0.03","0.05","0.05","0.05","0.04","0.04" }
            '  lcvaluelist = { 100,211,212,213,280,290,311,321,330,332,411,412,421,422,430,500,620,610,770,820,810,850,830,900 }
            '  lcnamelist = {"Urban and Built-Up Land", "Dryland Cropland and Pasture", "Irrigated Cropland and Pasture", "Mixed Dryland/Irrigated Cropland and Pasture", "Cropland/Grassland Mosaic", "Cropland/Woodland Mosaic", "Grassland", "Shrubland", "Mixed Shrubland/Grassland", "Savanna", "Deciduous Broadleaf Forest", "Deciduous Needleleaf Forest", "Evergreen Broadleaf Forest", "Evergreen Needleleaf Forest", "Mixed Forest", "Water Bodies", "Herbaceous Wetland", "Wooded Wetland", "Barren or Sparsely Vegetated", "Herbaceous Tundra", "Wooded Tundra", "Mixed Tundra", "Bare Ground Tundra", "Snow or Ice" }

            '  lccodelist = MsgBox.MultiInput("Specify Mannings (Velocity) Coefficients for each land cover", "Land Cover, Anderson Code, Manning's N", lcnamelist, Mannlist)

            '  lcfile = LineFile.Make((myWkdirname + "roughness.txt").AsFileName, #FILE_PERM_WRITE)
            Dim lLcfile As String = pOutputPath & "roughness.txt"

            'initialize velocity grid 
            If GisUtil.IsLayer(lVelocityGridLayerName) Then
                lVelocityGridLayerIndex = GisUtil.LayerIndex(lVelocityGridLayerName)
                GisUtil.RemoveLayer(lVelocityGridLayerIndex)
            End If
            lVelocityGridFileName = FilenameNoExt(lDemFileName) & "Velocity.bgd"

            If FileExists(lVelocityGridFileName) Then
                IO.File.Delete(lVelocityGridFileName)
            End If
            'IO.File.Copy(lFlowAccGridFileName, lVelocityGridFileName)
            GisUtil.GridAssignConstant(lVelocityGridFileName, lDemFileName, 0.0)

            Dim lLuFileName As String = GisUtil.LayerFileName(aUsgsLandCoverGname)
            Dim lSubbasinFileName As String = GisUtil.LayerFileName(lBasinGthm)
            Dim lFlowAccFileName As String = GisUtil.LayerFileName(lFacGthm)

            'at each grid cell we know the land use code -> mannings value
            '                              subbasin id   -> slope
            '                              flow accumulation 

            'from flow acc, mannings value and slope we can compute velocity
            Logger.Status("Computing Velocity Grid........")
            GisUtil.GridComputeVelocity(lVelocityGridFileName, lLuFileName, lSubbasinFileName, lFlowAccFileName, aManningsValues, lSlopeBySubbasin)

            ' from Manning's equation, v = (1/n)(R^(2/3)(S^0.5) = K (S^0.5)
            '    for Flowacc = 0-1000, R = 0.002,             k = (1/n)(0.002^(2/3)      = 0.0158740/n
            '    for Flowacc = 1000-2000, R = 0.005,             k = (1/n)(0.005^(2/3)      = 0.0292402/n
            '    for Flowacc = 2000-3000, R = 0.01,             k = (1/n)(0.01^(2/3)      = 0.0464159/n
            '    for Flowacc = 3000-4000, R = 0.02,             k = (1/n)(0.02^(2/3)      = 0.0736806/n   
            '    for Flowacc = 4000-5000, R = 0.05,             k = (1/n)(0.05^(2/3)      = 0.1357209/n
            '    for Flowacc = 5000-10000,              vel = 0.3
            '    for Flowacc = 10000-50000,             vel = 0.45
            '    for Flowacc = 50000-100000,            vel = 0.6
            '    for Flowacc = 100000-250000,           vel = 0.75
            '    for Flowacc = 250000-500000,           vel = 0.9
            '    for Flowacc = 500000-750000,           vel = 1.2
            '    for Flowacc = > 750000,                vel = 1.5

            'note:  comments above do not seem consistent with the way this was coded in avenue
            '    velgrid1 = ((0.1357209).AsGrid / manngrid) * (slopegrid ^ 0.5)
            '    velgrid2 = ((0.0736806).AsGrid / manngrid) * (slopegrid ^ 0.5)
            '    velgrid3 = ((0.0464159).AsGrid / manngrid) * (slopegrid ^ 0.5)
            '    velgrid4 = ((0.0292402).AsGrid / manngrid) * (slopegrid ^ 0.5)
            '    velgrid5 = ((0.015874).AsGrid / manngrid) * (slopegrid ^ 0.5)
            '    velgrid6 = (0.3).asgrid
            '    velgrid7 = (0.45).asgrid
            '    velgrid8 = (0.6).asgrid
            '    velgrid9 = (0.75).asgrid
            '    velgrid10 = (0.9).asgrid
            '    velgrid11 = (1.2).asgrid
            '    velgrid12 = (1.5).asgrid

            '    velgridraw = (facgrid <= 1000).con(velgrid1, ((facgrid <= 2000).con(velgrid2, ((facgrid <= 3000).con(velgrid3, ((facgrid <= 4000).con(velgrid4, ((facgrid <= 5000).con(velgrid5, ((facgrid <= 10000).con(velgrid6, ((facgrid <= 50000).con(velgrid7, ((facgrid <= 100000).con(velgrid8, ((facgrid <= 250000).con(velgrid9, ((facgrid <= 500000).con(velgrid10, ((facgrid <= 750000).con(velgrid11, velgrid12)))))))))))))))))))))
            '    velgrid = (velgridraw < 0.01).con((0.01).asgrid, (velgridraw > 1.5).con((1.5).asgrid, velgridraw))

            GisUtil.AddLayer(lVelocityGridFileName, lVelocityGridLayerName)

        ElseIf (aVelType = 2) Then
            'set velocity grid based on a threshold value
            ' velgrid = (facgrid < 1000).con((OverlandFlowVelocity).asgrid, (InstreamFlowVelocity).asgrid)
            ' velgthm = Gtheme.Make(velgrid)
            Dim lValue As Double = aOverlandFlowVelocity
            If GisUtil.IsLayer(lVelocityGridLayerName) Then
                lVelocityGridLayerIndex = GisUtil.LayerIndex(lVelocityGridLayerName)
                GisUtil.RemoveLayer(lVelocityGridLayerIndex)
            End If
            lVelocityGridFileName = FilenameNoExt(lDemFileName) & "Velocity.bgd"
            GisUtil.GridAssignConstant(lVelocityGridFileName, lDemFileName, lValue)
            'now for cells >= 1000, use instream flow velocity
            lValue = aInstreamFlowVelocity
            GisUtil.GridAssignConstantAboveThreshold(lVelocityGridFileName, lFlowAccGridFileName, 999, lValue)
            GisUtil.AddLayer(lVelocityGridFileName, lVelocityGridLayerName)
        Else
            'set velocity grid to a constant
            Dim lValue As Double = aOverlandFlowVelocity
            If lValue < 0.01 Then lValue = 0.01
            If GisUtil.IsLayer(lVelocityGridLayerName) Then
                lVelocityGridLayerIndex = GisUtil.LayerIndex(lVelocityGridLayerName)
                GisUtil.RemoveLayer(lVelocityGridLayerIndex)
            End If
            lVelocityGridFileName = FilenameNoExt(lDemFileName) & "Velocity.bgd"
            GisUtil.GridAssignConstant(lVelocityGridFileName, lDemFileName, lValue)
            GisUtil.AddLayer(lVelocityGridFileName, lVelocityGridLayerName)
        End If

        'given the velocity grid, compute travel times

        '        maskgrid = ((outgrid.isnull) / (outgrid.isnull))
        '        newfdrgrid = flowdirgrid * maskgrid
        Dim lOutletGridFileName As String = GisUtil.LayerFileName(lOutGthm)

        '        invelgrid = (1.AsGrid / velgrid)
        Dim lInverseVelocityGridFileName As String = FilenameNoExt(lVelocityGridFileName) & "InverseVelocity.bgd"
        GisUtil.GridInverse(lVelocityGridFileName, lInverseVelocityGridFileName)

        '        flowtimegrid = newfdrgrid.flowlength(invelgrid, False)
        Dim lTravelTimeGridFileName As String = FilenameNoExt(lFlowAccGridFileName) & "TravelTime." & FileExt(lFlowAccGridFileName)

        '        inverse velocity grid is used as a weighting factor 
        Logger.Status("Computing Downstream Flow Length........")
        GisUtil.DownstreamFlowLength(lFlowDirGridFileName, lFlowAccGridFileName, lTravelTimeGridFileName, lOutletGridFileName, lInverseVelocityGridFileName)

        '        daysgrid = (flowtimegrid / 86400).floor   'floor returns the greatest integer value less than or equal to aGrid.
        Dim lDaysGridLayerName As String = "Travel Time Grid (Days)"
        Dim lDaysGridLayerIndex As Integer = 0
        If GisUtil.IsLayer(lDaysGridLayerName) Then
            lDaysGridLayerIndex = GisUtil.LayerIndex(lDaysGridLayerName)
            GisUtil.RemoveLayer(lDaysGridLayerIndex)
        End If
        Dim lDaysGridFileName As String = FilenameNoExt(lTravelTimeGridFileName) & "DaysGrid." & FileExt(lTravelTimeGridFileName)
        GisUtil.GridMultiplyConstant(lTravelTimeGridFileName, 1 / 86400, lDaysGridFileName)

        'need to make sure days grid is an integer 
        Dim lDaysIntegerGridFileName As String = FilenameNoExt(lDaysGridFileName) & "Integer." & FileExt(lDaysGridFileName)
        GisUtil.GridToInteger(lDaysGridFileName, lDaysIntegerGridFileName)

        GisUtil.AddLayer(lDaysIntegerGridFileName, lDaysGridLayerName)
        lDaysGridLayerIndex = GisUtil.LayerIndex(lDaysGridLayerName)

        '        newdaysgrid = daysgrid * basingrid / basingrid
        '        numdays = newdaysgrid.GetStatistics.Get(1)
        Dim lNumdays As Integer = GisUtil.GridLayerMaximum(lDaysGridLayerIndex)

        '        this comment is from the avenue script, not sure what to make of it
        '        ' This is a temporary solution that will be removed when
        '        ' the routing program can read a parameter file

        If (lNumdays > 22) Then
            lNumdays = 22
            'else 
            '  numdays = 22
        End If

        'prepare response file
        Dim lRespOut As New StringBuilder
        Dim lStr As String = """" & "BasinId" & """"
        For lnday As Integer = 0 To lNumdays - 1
            lStr = lStr & ", " & """" & "Day" & CStr(lnday) & """"
        Next
        lRespOut.AppendLine(lStr)

        'figure out how many cells of each 'day' in each zone
        Dim lCnt As Integer = 0
        Dim lDaysArray(lZoneList.Count, lNumdays) As Integer
        For lnday As Integer = 0 To lNumdays - 1
            Dim lDaysZonalStats As atcCollection = GisUtil.GridZoneCountValue(lBasinGthm, lDaysGridLayerIndex, lnday)
            lCnt = 0
            For Each lzone As Integer In lZoneList
                lCnt += 1
                If Not lDaysZonalStats.ItemByKey(lzone) Is Nothing Then
                    lDaysArray(lCnt, lnday) = lDaysZonalStats.ItemByKey(lzone)
                End If
            Next
        Next

        lCnt = 0
        For Each lBid As Integer In lZoneList
            lCnt += 1
            lStr = CStr(lBid)
            Dim lSum As Integer = 0
            For lnday As Integer = 0 To lNumdays - 1
                lSum += lDaysArray(lCnt, lnday)
            Next
            For lnday As Integer = 0 To lNumdays - 1
                If lSum > 0 Then
                    lStr = lStr & ", " & Format((lDaysArray(lCnt, lnday) / lSum), "0.000000")
                Else
                    lStr = lStr & ", 0.000000"
                End If
            Next
            lRespOut.AppendLine(lStr)
        Next

        Dim lOutFile As String = pOutputPath & "response.txt"
        SaveFileString(lOutFile, lRespOut.ToString)

        Logger.Msg("Basin Response Computed. Output file: " & vbCrLf & pOutputPath + "response.txt", "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function RainEvap(ByVal aPrecGageNamesBySubbasin As Collection, ByVal aEvapGageNamesBySubbasin As Collection, ByVal aSJDate As Double, ByVal aEJDate As Double) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: RainEvap.ave
        '
        '      Function: 
        '          The program determines the mean rainfall and evaporation values
        '          for subbasins within each drainage basin for each time step
        '
        '      Inputs: 
        '          a grid of subbasins in a basin
        '          the full path to a directory where the evaporation and rainfall grids are stored
        '          the number of subbasins in the basin   
        '          the beginning year (4 digit number) for the rainfall and evaporation grids
        '          the ending year (4 digit number) for the rainfall and evaporation grids
        '          the beginning day (1 to 366) for the rainfall and evaporation grids
        '          the ending day (1 to 366) for the rainfall and evaporation grids
        '
        '      Outputs:  
        '          a rainfall file entitled 'rain.txt'
        '          an evaporation file entitled 'evap.txt'
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin grid
        '          all input evaporation and rainfall grids are in a single directory
        '          evaporation grids are named evapyearday eg evap_1999002 for the second day of 1999
        '          rainfall grids are named rainyearday eg rain_1999002 for the second day of 1999
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'TODO: Only works for BASINS timeseries at this point, default to using closest stations

        Dim lOrderFileName As String = pOutputPath & "order.txt"

        Dim lSubbasins As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lOrderFileName)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lSubbasins.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Basin Characteristics' to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lRainOut As New StringBuilder
        Dim lEvapOut As New StringBuilder
        Dim lStr As String = "Time"
        For Each lSubbasin As String In lSubbasins
            lStr = lStr & ", " & lSubbasin
        Next
        lRainOut.AppendLine(lStr)
        lEvapOut.AppendLine(lStr)

        'populate prec and evap arrays
        Dim lNdates As Integer = aEJDate - aSJDate + 1
        Dim lPrecArray(lSubbasins.Count, lNdates) As Single
        Dim lEvapArray(lSubbasins.Count, lNdates) As Single

        Dim lPrecTimeseries As atcTimeseries = Nothing
        Dim lEvapTimeseries As atcTimeseries = Nothing

        Dim lPrecGageNamePrev As String = ""
        Dim lEvapGageNamePrev As String = ""

        For lSubbasin As Integer = 0 To lSubbasins.Count - 1
            Dim lPrecGageName As String = aPrecGageNamesBySubbasin(lSubbasin + 1)
            Dim lEvapGageName As String = aEvapGageNamesBySubbasin(lSubbasin + 1)
            If lPrecGageName <> lPrecGageNamePrev Then
                For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
                    For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                        If (lDataSet.Attributes.GetValue("Constituent") = "PREC" And _
                            lDataSet.Attributes.GetValue("Location") = lPrecGageName) Then
                            lPrecTimeseries = Aggregate(lDataSet, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                        End If
                    Next
                Next
            End If
            If lEvapGageName <> lEvapGageNamePrev Then
                For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
                    For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                        If (lDataSet.Attributes.GetValue("Constituent") = "PEVT" And _
                            lDataSet.Attributes.GetValue("Location") = lEvapGageName) Then
                            lEvapTimeseries = Aggregate(lDataSet, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                        End If
                    Next
                Next
            End If
            If Not lPrecTimeseries Is Nothing Then
                Dim lStartIndex As Integer = lPrecTimeseries.Dates.IndexOfValue(aSJDate, True)
                If aSJDate = lPrecTimeseries.Dates.Values(0) Or lStartIndex < 0 Then
                    lStartIndex = 0
                End If
                Dim lEndIndex As Integer = lPrecTimeseries.Dates.IndexOfValue(aEJDate, True)
                For lIndex As Integer = lStartIndex To lEndIndex - 1
                    'The rain.txt file created from this process contains an average rainfall value in millimeters for each subbasin per day. 
                    lPrecArray(lSubbasin, lIndex - lStartIndex) = lPrecTimeseries.Values(lIndex + 1) * 25.4   'mm/inch 
                Next
            End If
            If Not lEvapTimeseries Is Nothing Then
                Dim lStartIndex As Integer = lEvapTimeseries.Dates.IndexOfValue(aSJDate, True)
                If aSJDate = lEvapTimeseries.Dates.Values(0) Or lStartIndex < 0 Then
                    lStartIndex = 0
                End If
                Dim lEndIndex As Integer = lEvapTimeseries.Dates.IndexOfValue(aEJDate, True)
                For lIndex As Integer = lStartIndex To lEndIndex - 1
                    'The evap.txt file contains a potential evapotranspiration (PET) value in tenths of millimeters for each subbasin per day.
                    lEvapArray(lSubbasin, lIndex - lStartIndex) = lEvapTimeseries.Values(lIndex + 1) * 254.0   'tenths of mm/inch 
                Next
            End If
            lPrecGageNamePrev = lPrecGageName
            lEvapGageNamePrev = lEvapGageName
        Next

        'write record for each date
        For lIndex As Integer = 1 To lNdates
            Dim lDate(6) As Integer
            J2Date(aSJDate + lIndex - 1, lDate)
            Dim lYr As Integer = lDate(0)
            Dim lJYr As Double = Date2J(lYr, 1, 1)
            Dim lDaysAfter As Integer = aSJDate + lIndex - lJYr
            Dim lPrecString As String = lDate(0) & Format(lDaysAfter, "000")
            Dim lEvapString As String = lDate(0) & Format(lDaysAfter, "000")
            For lSubbasin As Integer = 0 To lSubbasins.Count - 1
                lPrecString = lPrecString & ", " & Format(lPrecArray(lSubbasin, lIndex - 1), "0.0")
            Next
            For lSubbasin As Integer = 0 To lSubbasins.Count - 1
                lEvapString = lEvapString & ", " & Format(lEvapArray(lSubbasin, lIndex - 1), "0.0")
            Next
            lRainOut.AppendLine(lPrecString)
            lEvapOut.AppendLine(lEvapString)
        Next

        Dim lOutFile As String = pOutputPath & "rain.txt"
        SaveFileString(lOutFile, lRainOut.ToString)

        lOutFile = pOutputPath & "evap.txt"
        SaveFileString(lOutFile, lEvapOut.ToString)

        Logger.Msg("Processing Complete. Output files: " & vbCrLf & pOutputPath + "rain.txt" & vbCrLf & pOutputPath + "evap.txt", "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function Balance(ByVal inifract As Single, ByVal dformat As Integer, ByVal inimode As Integer, ByVal runmode As Integer, _
                            ByVal abalancetype As Integer, ByVal aSjDate As Double) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: balance.ave
        '
        '      Function: 
        '          The program computes a soil water balance for each subbasin in the model  
        '          called and executed from an ArcView interphase. 
        '
        '      Inputs:
        '          a textfile containing hydrologic parameters for each subbasin, basin.txt
        '          a textfile containing rainfall data for each subbasin, rain.txt
        '          a textfile containing evaporation data for each subbasin, evap.txt
        '          a textfile containing initialization data for each subbasin
        '          a textfile containing parameters specific to the simulation run
        '
        '      Outputs:  
        '          a textfile containing soil water balance results for each subbasin
        '          a textfile containing streamflow data for each subbasin
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin polygon coverage. basin.shp
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'CREATE A LIST OF INPUT LABELS
        'labels = { "Input Rainfall File", "Input Evap File", "Input Basin File", 
        '          "Input Response File", "Input Parameter File", "Basin Runoff Yield File",
        '          "Output Soil Moisture File", "Output Actual Evap.File", "Ground Water Loss File", 
        '          "Current Soil Water File","Excess Runoff File", "Interflow File", "Baseflow File", 
        '          "Mass Balance File", "Process/Error Log File",  "Initial Soil Moisture File"}

        'defaults = { "rain.txt", "evap.txt","basin.txt","response.txt", "balparam.txt", "basinrunoffyield.txt", "soilwater.txt", "actualevap.txt", "gwloss.txt", "cswater.txt", "excessflow.txt", "interflow.txt", "baseflow.txt", "massbalance.txt", "logfilesoil.txt", "initial.txt"}
        'inpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", labels, defaults)

        Dim rainfilename As String = pOutputPath & "rain.txt"
        Dim evapfilename As String = pOutputPath & "evap.txt"
        Dim basinfilename As String = pOutputPath & "basin.txt"
        Dim respfilename As String = pOutputPath & "response.txt"
        Dim paramfilename As String = pOutputPath & "balparam.txt"
        Dim surpfilename As String = pOutputPath & "basinrunoffyield.txt"
        Dim storefilename As String = pOutputPath & "soilwater.txt"
        Dim aevapfilename As String = pOutputPath & "actualevap.txt"
        Dim gwlossfilename As String = pOutputPath & "gwloss.txt"
        Dim outswfilename As String = pOutputPath & "cswater.txt"
        Dim excessfilename As String = pOutputPath & "excessflow.txt"
        Dim interfilename As String = pOutputPath & "interflow.txt"
        Dim basefilename As String = pOutputPath & "baseflow.txt"
        Dim massfilename As String = pOutputPath & "massbalance.txt"
        Dim logfilename As String = pOutputPath & "logfilesoil.txt"
        Dim initialfilename As String = pOutputPath & "initial.txt"
        Dim balfilename As String = pOutputPath & "balfiles.txt"
        Dim maxtimefilename As String = pOutputPath & "maxtime.txt"
        Dim testfilename As String = pOutputPath & "testfile.txt"
        Dim whichmodelFN As String = pOutputPath & "whichmodel.txt"

        If Not FileExists(rainfilename) Then
            Logger.Msg("Could not open rain file," & vbCrLf & rainfilename, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(evapfilename) Then
            Logger.Msg("Could not open evap file," & vbCrLf & evapfilename, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(basinfilename) Then
            Logger.Msg("Could not open basin file," & vbCrLf & basinfilename, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(respfilename) Then
            Logger.Msg("Could not open response file," & vbCrLf & respfilename, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lRStreamReader As New StreamReader(rainfilename)
        Dim lCurrentRecord As String = ""
        Dim lRainSize As Integer = 0
        Do
            lCurrentRecord = lRStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                lRainSize += 1
            End If
        Loop

        Dim lEStreamReader As New StreamReader(evapfilename)
        Dim lEvapSize As Integer = 0
        Do
            lCurrentRecord = lEStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                lEvapSize += 1
            End If
        Loop

        Dim lRunSize As Integer = 0
        If (lEvapSize > lRainSize) Then
            lRunSize = lRainSize - 1
        ElseIf (lRainSize > lEvapSize) Then
            lRunSize = lEvapSize - 1
        Else
            lRunSize = lRainSize - 1
        End If

        Dim lBStreamReader As New StreamReader(basinfilename)
        Dim bassize As Integer = 0
        Do
            lCurrentRecord = lBStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                bassize += 1
            End If
        Loop
        bassize = bassize - 1

        Dim lDate(6) As Integer
        J2Date(aSjDate, lDate)
        Dim lStartYr As Integer = lDate(0)
        Dim lJYr As Double = Date2J(lStartYr, 1, 1)
        Dim lStartDy As Integer = aSjDate - lJYr + 1

        Dim lRespStreamReader As New StreamReader(respfilename)
        lCurrentRecord = lRespStreamReader.ReadLine
        Dim resdays As Integer = 0
        Dim lstr As String = ""
        Do Until lCurrentRecord.Length = 0
            resdays += 1
            lstr = StrRetRem(lCurrentRecord)
        Loop
        resdays = resdays - 1

        Dim lChkFile As New StringBuilder
        lChkFile.AppendLine(rainfilename)
        lChkFile.AppendLine(evapfilename)
        lChkFile.AppendLine(basinfilename)
        lChkFile.AppendLine(respfilename)
        lChkFile.AppendLine(paramfilename)
        lChkFile.AppendLine(surpfilename)
        lChkFile.AppendLine(storefilename)
        lChkFile.AppendLine(aevapfilename)
        lChkFile.AppendLine(gwlossfilename)
        lChkFile.AppendLine(outswfilename)
        lChkFile.AppendLine(excessfilename)
        lChkFile.AppendLine(interfilename)
        lChkFile.AppendLine(basefilename)
        lChkFile.AppendLine(massfilename)
        lChkFile.AppendLine(logfilename)
        lChkFile.AppendLine(initialfilename)
        lChkFile.AppendLine(pOutputPath)
        SaveFileString(balfilename, lChkFile.ToString)

        'plabels = { "Computation Start Year", "Computation Start Day", "Number of Rain/Evap Days", 
        '          "Number of Response Days", "Number of Subbasins", "Initial Soil Moisture", "Data Format (0=Hourly/1=Daily)", 
        '          "New Run (0) or Continue Previous Run(1)", "Basin Polygon Theme","Key Field eg Grid Code","Simulation(0) or Calibration(1) Mode"}

        'pdefaults = { startyr, startdy, runsize.asstring, resdays.asstring, bassize.asstring, "0.1", "1", "0", "basply.shp", "gridcode", "0"}
        'runpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", plabels, pdefaults)

        Dim tinterval As String = ""
        If (dformat = 1) Then
            tinterval = "24"
        ElseIf (dformat = 0) Then
            tinterval = "1"
        Else
            tinterval = "24"
        End If

        Dim basinthm As Integer = -1
        Dim idfield As Integer = -1
        Dim nowfield As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            basinthm = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(basinthm, "gridcode") Then
                idfield = GisUtil.FieldIndex(basinthm, "gridcode")
            Else
                idfield = 0
            End If
            If GisUtil.IsField(basinthm, "soilwater") Then
                nowfield = GisUtil.FieldIndex(basinthm, "soilwater")
            Else
                GisUtil.AddField(basinthm, "soilwater", 2, 10)
                nowfield = GisUtil.FieldIndex(basinthm, "soilwater")
            End If
        End If

        Dim lParamFile As New StringBuilder
        lParamFile.AppendLine(resdays.ToString)
        lParamFile.AppendLine(lRunSize.ToString)
        lParamFile.AppendLine(lStartYr.ToString)
        lParamFile.AppendLine(Format(lStartDy, "000"))
        lParamFile.AppendLine(bassize.ToString)
        lParamFile.AppendLine(tinterval.ToString)
        lParamFile.AppendLine(dformat.ToString)
        lParamFile.AppendLine(inimode.ToString)
        lParamFile.AppendLine(inifract.ToString)
        lParamFile.AppendLine(runmode.ToString)
        SaveFileString(paramfilename, lParamFile.ToString)

        ''timefile.writeelt(runsize.asstring)  'commented out in the avenue 
        ''timefile.close

        Dim routetype As Integer = 2

        Dim lWhichFile As New StringBuilder
        lWhichFile.AppendLine("Model Index   Index description")
        lWhichFile.AppendLine(abalancetype & " //water balance model:  1=1D balance, 2=2D balance")
        lWhichFile.AppendLine(routetype & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        SaveFileString(whichmodelFN, lWhichFile.ToString)

        'If FileExists(surpfilename) Then
        '    IO.File.Delete(surpfilename)
        'End If

        If abalancetype = 1 Then    '"Linear Soil Model"
            ONELAYERBALANCE(balfilename)
        Else                       '"Non-Linear Soil Model"
            TWOLAYERBALANCE(balfilename)
        End If

        If Not FileExists(surpfilename) Then
            Logger.Msg("An error occurred during the balance computation." & vbCrLf & "Check for possible causes in  " + logfilename, "Geospatial Stream Flow Model")
            Return False
        End If

        If (runmode = 0) Then
            If Not FileExists(outswfilename) Then
                Logger.Msg("Could not open current soil water file," & vbCrLf & outswfilename, "Geospatial Stream Flow Model")
                Return False
            End If

            Dim lCurrent As New atcCollection
            Dim lOutStreamReader As New StreamReader(outswfilename)
            lCurrentRecord = lOutStreamReader.ReadLine
            Do
                lCurrentRecord = lOutStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                End If
                lstr = StrRetRem(lCurrentRecord)
                lCurrent.Add(lstr, lCurrentRecord)
            Loop

            'populate soilwater field in subbasins shapefile
            GisUtil.StartSetFeatureValue(basinthm)
            Dim lId As String = ""
            For lIndex As Integer = 0 To GisUtil.NumFeatures(basinthm) - 1
                lId = GisUtil.FieldValue(basinthm, lIndex, idfield)
                If Not lCurrent.ItemByKey(lId) Is Nothing Then
                    GisUtil.SetFeatureValue(basinthm, nowfield, lIndex, lCurrent.ItemByKey(lId))
                End If
            Next
            GisUtil.StopSetFeatureValue(basinthm)

            Dim lColors As New Collection
            lColors.Add(System.Convert.ToUInt32(RGB(250, 250, 0))) 'yellow
            lColors.Add(System.Convert.ToUInt32(RGB(0, 240, 0)))   'green
            lColors.Add(System.Convert.ToUInt32(RGB(0, 0, 250)))   'blue
            Dim lCaptions As New Collection
            lCaptions.Add("dry (0 - 50)")
            lCaptions.Add("moist (50 - 100)")
            lCaptions.Add("wet (> 100mm)")
            Dim lLowRange As New Collection
            lLowRange.Add(0.0)
            lLowRange.Add(50.0)
            lLowRange.Add(100.0)
            Dim lHighRange As New Collection
            lHighRange.Add(50.0)
            lHighRange.Add(100.0)
            lHighRange.Add(1000.0)
            GisUtil.SetLayerRendererWithRanges(basinthm, nowfield, lColors, lCaptions, lLowRange, lHighRange)
        End If

        Logger.Msg("Soil Water Balance Complete. Results written to:" & vbCrLf & surpfilename, "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function Route(ByVal forecastdays As Integer, ByVal runmode As Integer, ByVal dformat As Integer, _
                          ByVal aRouteMethod As Integer, ByVal aSjDate As Double) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: route.ave
        '
        '      Function: 
        '          The program computes a soil water balance for each subbasin in the model  
        '          called and executed from an ArcView interphase. 
        '
        '      Inputs:
        '          a textfile containing hydrologic parameters for each subbasin, river.txt
        '          a textfile containing rainfall data for each subbasin, soilwater.txt
        '          a textfile containing evaporation data for each subbasin, response.txt
        '          a textfile containing computational for each subbasin, order.txt
        '          a textfile containing parameters specific to the simulation run, parameter.txt
        '          a textfile containing discharge data for reservoirs in the basin, reservoir.txt
        '
        '      Outputs:  
        '          a textfile containing daily water balance for each reservoir in the basin, damstatus.txt
        '          a textfile containing streamflow data for each subbasin, streamflow.txt
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin polygon coverage. basin.shp
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        '        'CREATE A LIST OF INPUT LABELS
        'labels = { "Input Runoff File", "Input Response File", "Input River Characteristics File", 
        '          "Input Reservoir Characteristic File", "Output River Depth File", "Output Upstream Flow File", "Output Streamflow File","Output Reservoir Status File",
        '          "Output Local Flow File", "Routing Parameter File", "Listing of Routing Files", "Streamflow Forecast File", "Routing Log File", "Basin Polygon Theme","Key Field eg Grid Code", "No. of Days of Forecast Required", "Simulation(0) or Calibration(1) Mode"}

        'defaults = { "basinrunoffyield.txt","response.txt","river.txt", "reservoir.txt", "riverdepth.txt", "inflow.txt", "streamflow.txt", "damstatus.txt", "localflow.txt", "routparam.txt", "routfiles.txt", "forecast.txt", "logfileflow.txt", "basply.shp","gridcode", "3","0"}

        Dim runoffFN As String = pOutputPath & "basinrunoffyield.txt"
        Dim responseFN As String = pOutputPath & "response.txt"
        Dim riverFN As String = pOutputPath & "river.txt"
        Dim reservoirFN As String = pOutputPath & "reservoir.txt"
        Dim riverdepthFN As String = pOutputPath & "riverdepth.txt"
        Dim inflowFN As String = pOutputPath & "inflow.txt"
        Dim flowFN As String = pOutputPath & "streamflow.txt"
        Dim damsFN As String = pOutputPath & "damstatus.txt"
        Dim localflowFN As String = pOutputPath & "localflow.txt"
        Dim routparamFN As String = pOutputPath & "routparam.txt"
        Dim routfilesFN As String = pOutputPath & "routfiles.txt"
        Dim forecastFN As String = pOutputPath & "forecast.txt"
        Dim logFN As String = pOutputPath & "logfileflow.txt"

        If (forecastdays > 99) Then
            forecastdays = 99
        ElseIf (forecastdays < 0) Then
            forecastdays = 0
        End If

        Dim initialFN As String = pOutputPath & "initial.txt"
        Dim timesFN As String = pOutputPath & "times.txt"
        'Dim maxtimeFN As String = pOutputPath & "maxtime.txt"
        Dim damlinkFN As String = pOutputPath & "damlink.txt"
        Dim ratingFN As String = pOutputPath & "rating.txt"
        Dim whichmodelFN As String = pOutputPath & "whichmodel.txt"

        If Not FileExists(runoffFN) Then
            Logger.Msg("Could not open runoff file." & vbCrLf & runoffFN, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(responseFN) Then
            Logger.Msg("Could not open response file," & vbCrLf & responseFN, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(riverFN) Then
            Logger.Msg("Could not open river file," & vbCrLf & riverFN, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim theday As String = System.DateTime.Now.Date.ToString
        Dim theyear As String = System.DateTime.Now.Year.ToString

        Dim lChkFile As New StringBuilder
        lChkFile.AppendLine("Starting Time:" & " " & System.DateTime.Now.ToString)

        'IDENTIFY THE CALCULATION THEME, THE CELL FIELD AND THE WATER HOLDING CAPACITY FIELD

        Dim basinthm As Integer = -1
        Dim idfield As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            basinthm = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(basinthm, "gridcode") Then
                idfield = GisUtil.FieldIndex(basinthm, "gridcode")
            Else
                idfield = 0
            End If
        End If

        Logger.Dbg("Reading Input Files.......")

        Dim lRivList As New atcCollection
        Dim rivstart As Integer = 1
        Dim lRiv As String = ""
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(riverFN)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lRivList.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading the river file " & riverFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lResponseList As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(responseFN)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lResponseList.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading the response file " & responseFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        If (lResponseList.Count < lRivList.Count) Then
            Logger.Msg("The response file " + responseFN + " does not contain enough subbasins.", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim resdays As Integer = lResponseList.Count

        ' READ IN THE INPUT ARRAYS FOR PRECIPITATION AND POTENTIAL EVAPORATION
        Dim lRunoffList As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(runoffFN)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lRunoffList.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading the runoff file " & runoffFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lStr As String = lRunoffList(0)
        Dim dayonestr As String = StrRetRem(lStr)
        Dim timetype As String = "Daily"
        Dim timeinhrs As String = "24"

        If dformat = 0 Then
            timetype = "Hourly"
            timeinhrs = "1"
        End If

        Dim lDate(6) As Integer
        J2Date(aSjDate, lDate)
        Dim lStartYr As Integer = lDate(0)
        Dim lJYr As Double = Date2J(lStartYr, 1, 1)
        Dim lStartDy As Integer = aSjDate - lJYr + 1

        Dim runoffdays As Integer = lRunoffList.Count

        Dim outsteps As Integer = runoffdays + resdays + forecastdays

        Dim rundays As Integer = (runoffdays + resdays + forecastdays + resdays + 1)

        Dim lForeFile As New StringBuilder
        lForeFile.AppendLine(lRunoffList.Item(0))

        Logger.Dbg("Updating Reservoir Discharges.......")

        ' Performing a convolution to update PFLOW
        ' ie the local contribution to streamflow

        Dim tcount As Integer = 0
        Dim isfirst As Integer = 0
        Dim damcount As Integer = 0
        Dim lReservoirList As New atcCollection
        For brec As Integer = (lRivList.Count - 1) To rivstart Step -1

            Dim charlist As String = lRivList.ItemByIndex(brec)
            Dim resplist As String = lResponseList.ItemByIndex(brec)
            Dim riverid As String = StrRetRem(charlist)
            Dim lTmpStr As String = ""
            For lIndex As Integer = 1 To 8
                lTmpStr = StrRetRem(charlist)
            Next
            Dim hasdam As Integer = StrRetRem(charlist)

            If (hasdam <> 0) Then
                damcount = damcount + 1
                If (isfirst = 0) Then

                    Dim lResvid As String
                    Try
                        Dim lCurrentRecord As String
                        Dim lStreamReader As New StreamReader(reservoirFN)
                        lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                        Do
                            lCurrentRecord = lStreamReader.ReadLine
                            lResvid = StrRetRem(lCurrentRecord)
                            If lCurrentRecord Is Nothing Then
                                Exit Do
                            Else
                                lReservoirList.Add(lResvid, lCurrentRecord)
                            End If
                        Loop
                    Catch e As ApplicationException
                        Logger.Msg("Could not open reservoir characteristics file," & vbCrLf & reservoirFN, MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                        Exit Function
                    End Try

                    isfirst = isfirst + 1
                End If

                Dim isoperated As Integer = 0
                Dim resdtime As Single = 0.0
                Dim lDamoutFile As New StringBuilder
                Dim lDamoutFileName As String = pOutputPath & "dam" & riverid.ToString & ".txt"

                Dim resvlist As String = ""
                If lReservoirList.ItemByKey(riverid) Is Nothing Then
                    Logger.Msg("No reservoir characteristic found for basin " & riverid & vbCrLf & "Update " & reservoirFN & " before computing streamflow.", "Geospatial Stream Flow Model")
                    Return False
                Else
                    resvlist = lReservoirList.Item(riverid)

                    If (resvlist.Length < 4) Then
                        Logger.Msg("The reservoir characteristic file must contain 3 or more fields:" & vbCrLf & "riverid, storage, residencetime, isoperated", "Geospatial Stream Flow Model")
                    End If
                End If
                Dim damstore As Single = StrRetRem(resvlist)
                resdtime = StrRetRem(resvlist)
                isoperated = StrRetRem(resvlist)
                If (isoperated <> 0) Then
                    Dim operateFN As String = StrRetRem(resvlist)
                    Dim withd As Integer = 0
                    If Not FileExists(operateFN) Then
                        If Not FileExists(pOutputPath & operateFN) Then
                            Logger.Msg("Operations file " & operateFN & " for reservoir in basin " & riverid & "Not found." + vbCrLf + "Create the file before computing streamflow", "Geospatial Stream Flow Model")
                            Return False
                        End If
                        withd = 1
                    End If

                    Dim oplist As New atcCollection
                    Dim opsize As Integer = 0
                    Try
                        Dim lCurrentRecord As String
                        Dim lStreamReader As New StreamReader(operateFN)
                        lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                        Do
                            lCurrentRecord = lStreamReader.ReadLine
                            If lCurrentRecord Is Nothing Then
                                Exit Do
                            Else
                                opsize = opsize + 1
                                oplist.Add(lCurrentRecord)
                            End If
                        Loop
                    Catch e As ApplicationException
                        Logger.Msg("Problem reading reservoir operations file," & vbCrLf & operateFN, MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                        Return False
                    End Try

                    Dim testflst As String = ""
                    Dim lastflow As String = ""
                    For oflow As Integer = opsize - 1 To 0 Step -1
                        testflst = oplist(oflow)
                        If (testflst.Length >= 2) Then
                            lastflow = StrRetRem(testflst)
                            lastflow = StrRetRem(testflst)
                            opsize = oflow + 1
                            Exit For
                        End If
                    Next

                    If (opsize < outsteps) Then
                        For orec As Integer = 1 To (outsteps - opsize)
                            Dim opday As String = ""
                            If (orec <= runoffdays) Then
                                opday = lRunoffList(orec)
                            Else
                                opday = (orec - runoffdays).ToString
                            End If
                            oplist.Add(opday & ", " & "1" & ", " & lastflow)
                        Next
                    End If

                    lDamoutFile.AppendLine("Time,Stage,Discharge")
                    For trec As Integer = 0 To (outsteps - 1)
                        lDamoutFile.AppendLine(oplist(trec).ToString)
                    Next
                    SaveFileString(lDamoutFileName, lDamoutFile.ToString)

                End If

                Dim lDamLinkFile As New StringBuilder
                lDamLinkFile.AppendLine(riverid.ToString)
                lDamLinkFile.AppendLine(pOutputPath & "dam" & riverid.ToString & ".txt")
                lDamLinkFile.AppendLine(isoperated.ToString)
                lDamLinkFile.AppendLine(resdtime.ToString)
                SaveFileString(damlinkFN, lDamLinkFile.ToString)
            End If

        Next

        Dim balstr As String = ""
        Dim routstr As String = ""
        Dim balancetype As String = ""
        Dim routetype As String = ""
        If (FileExists(whichmodelFN)) Then
            Dim whichsize As Integer = 0
            Dim biglist As New atcCollection
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(whichmodelFN)
                lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        whichsize = whichsize + 1
                        biglist.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Could not open whichModel.txt file" & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try

            If (whichsize > 2) Then
                balancetype = biglist(1)
                routetype = biglist(2)
            Else
                balancetype = "1"
                routetype = "2"
            End If

            'Model Index   Index description
            '1 //water balance model:  1=1D balance, 2=2D balance
            '2 //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag
            If ((balancetype = "1") And (routetype = "3")) Then
                balstr = "Linear Soil Model"
                routstr = "Simple Lag Routing Method"
            ElseIf ((balancetype = "1") And (routetype = "1")) Then
                balstr = "Linear Soil Model"
                routstr = "Diffusion Analog Routing Method"
            ElseIf ((balancetype = "1") And (routetype = "2")) Then
                balstr = "Linear Soil Model"
                routstr = "Muskingum-Cunge Routing"
            ElseIf ((balancetype = "2") And (routetype = "3")) Then
                balstr = "Non-Linear Soil Model"
                routstr = "Simple Lag Routing Method"
            ElseIf ((balancetype = "2") And (routetype = "1")) Then
                balstr = "Non-Linear Soil Model"
                routstr = "Diffusion Analog Routing Method"
            ElseIf ((balancetype = "2") And (routetype = "2")) Then
                balstr = "Non-Linear Soil Model"
                routstr = "Muskingum Cunge Routing Method"
            Else
                balstr = "Linear Soil Model"
                routstr = "Muskingum-Cunge Routing"
            End If
            ' routelst = {routstr, "Simple Lag Routing Method", "Diffusion Analog Routing Method", "Muskingum Cunge Routing Method" }
        Else
            '  routelst = {"Muskingum Cunge Routing Method", "Diffusion Analog Routing Method","Simple Lag Routing Method" }
            balstr = "Linear Soil Model"
        End If

        '        routestr = MsgBox.choiceasstring(routelst, "Select Flow Routing Method for Computation", "Geospatial Stream Flow Model")

        If (balstr = "Linear Soil Model") Then
            balancetype = "1"
        ElseIf (balstr = "Non-Linear Soil Model") Then
            balancetype = "2"
        Else
            balancetype = "1"
        End If

        routetype = aRouteMethod.ToString

        Dim lWhichFile As New StringBuilder
        lWhichFile.AppendLine("Model Index   Index description")
        lWhichFile.AppendLine(balancetype & " //water balance model:  1=1D balance, 2=2D balance")
        lWhichFile.AppendLine(routetype & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        SaveFileString(whichmodelFN, lWhichFile.ToString)

        Dim myrunoff As Integer = 0
        If (runoffdays > 100) Then
            myrunoff = (runoffdays - 100)
        Else
            myrunoff = runoffdays
        End If

        Dim outformat As Integer = 0
        If (timetype = "Hourly") Then
            outformat = 1
        Else
            outformat = 0
        End If

        Dim lParFile As New StringBuilder
        lParFile.AppendLine(myrunoff.ToString)
        lParFile.AppendLine(lStartYr.ToString)
        lParFile.AppendLine(Format(lStartDy, "000"))
        lParFile.AppendLine(lRivList.Count.ToString)
        lParFile.AppendLine(timeinhrs.ToString)
        lParFile.AppendLine("0")
        lParFile.AppendLine(forecastdays.ToString)
        lParFile.AppendLine(outformat.ToString)
        lParFile.AppendLine(damcount.ToString)
        lParFile.AppendLine(runmode.ToString)
        SaveFileString(routparamFN, lParFile.ToString)

        Dim lRoutFile As New StringBuilder
        lRoutFile.AppendLine(routparamFN)
        lRoutFile.AppendLine(riverFN)
        lRoutFile.AppendLine(initialFN)
        lRoutFile.AppendLine(runoffFN)
        lRoutFile.AppendLine(damlinkFN)
        lRoutFile.AppendLine(forecastFN)
        lRoutFile.AppendLine(ratingFN)
        lRoutFile.AppendLine(flowFN)
        lRoutFile.AppendLine(localflowFN)
        lRoutFile.AppendLine(riverdepthFN)
        lRoutFile.AppendLine(inflowFN)
        lRoutFile.AppendLine(logFN)
        lRoutFile.AppendLine(pOutputPath)
        SaveFileString(routfilesFN, lRoutFile.ToString)

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)

        Dim geosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            geosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            geosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        If (aRouteMethod = 3) Then
            LAGROUTE(routfilesFN)
        ElseIf (aRouteMethod = 1) Then
            DIFFROUTE(routfilesFN)
        Else
            CUNGEROUTE(routfilesFN)
        End If

        'timefile.writeelt(myrunoff.asstring)  'commented out in the avenue 
        'timefile.close

        If Not FileExists(flowFN) Then
            Logger.Msg("An error occurred during the routing computations." & vbCrLf & "Check for possible causes in logfileflow.txt", "Geospatial Stream Flow Model")
            Return False
        End If

        lChkFile.AppendLine("Ending Time: " & System.DateTime.Now.ToString)
        SaveFileString(timesFN, lChkFile.ToString)

        SetFlowTimeseries(flowFN)

        Logger.Msg("Stream Flow Routing Complete." & vbCrLf & "Results written as BASINS internal timeseries and to file: " & vbCrLf & flowFN, "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function Sensitivity(ByVal aSelectedReachIndex As Integer, ByVal aBalanceType As Integer, ByVal aRouteType As Integer, _
                                ByVal aParameterRanges As atcCollection) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: sensititivity.ave
        '
        '      Function: 
        '          The program calibrates basin.txt and river.txt parameters to improve the match  
        '          between simulated and observed models runs. 
        '
        '      Inputs:
        '          a textfile containing hydrologic parameters for each subbasin, basin.txt
        '          a textfile containing hydrologic parameters for each river reach, river.txt
        '          a textfile containing rainfall data for each subbasin, rain.txt
        '          a textfile containing evaporation data for each subbasin, evap.txt
        '          a textfile containing computational for each subbasin, order.txt
        '          a textfile containing parameters for the water balance simulation run, balparam.txt
        '          a textfile containing parameters for the river routing simulation run, routparam.txt
        '          a textfile containing discharge data for reservoirs in the basin, reservoir.txt
        '
        '      Outputs:  
        '          a logfile documenting the pre and post processing of calibration input and output files, logfilesarun.txt
        '          a textfile containing streamflow data for each subbasin, streamflow.txt
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin polygon coverage. basin.shp
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)

        'read/write balance parameters
        Dim lBalParamFN As String = pOutputPath & "balparam.txt"
        Dim lBalList As New Collection
        If FileExists(lBalParamFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lBalParamFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lBalList.Add(lCurrentRecord)
                    End If
                Loop
                lStreamReader.Close()
            Catch e As ApplicationException
                Logger.Msg("Problem reading parameters from balparam.txt file," & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Problem reading parameters from balparam.txt file," & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
            Return False
        End If

        'write it out in calibration mode
        Dim lParamFile As New StringBuilder
        For lIndex As Integer = 1 To 9
            lParamFile.AppendLine(lBalList(lIndex))
        Next
        lParamFile.AppendLine(1.ToString)
        SaveFileString(lBalParamFN, lParamFile.ToString)

        'read/write routing parameters
        Dim lRoutParamFN As String = pOutputPath & "routparam.txt"
        Dim lRoutList As New Collection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lRoutParamFN)
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lRoutList.Add(lCurrentRecord)
                End If
            Loop
            lStreamReader.Close()
        Catch e As ApplicationException
            Logger.Msg("Problem reading parameters from routparam.txt file," & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
            Return False
        End Try
        'write it out in calibration mode
        Dim lRParamFile As New StringBuilder
        For lIndex As Integer = 1 To 9
            lRParamFile.AppendLine(lRoutList(lIndex))
        Next
        lRParamFile.AppendLine(1.ToString)
        SaveFileString(lRoutParamFN, lRParamFile.ToString)

        Dim ntsteps As String = lBalList(2)
        If Not IsNumeric(ntsteps) Then
            ntsteps = lRoutList(1)
        End If

        Dim logfileFN As String = pOutputPath & "logfilesarun.txt"
        Dim logfile As New StringBuilder

        Dim balancetype As Integer = 0
        Dim routetype As Integer = 0
        Dim whichmodelFN As String = pOutputPath & "whichModel.txt"
        If Not FileExists(whichmodelFN) Then
            Dim lWhichFile As New StringBuilder
            lWhichFile.AppendLine("Model Index   Index description")
            lWhichFile.AppendLine(aBalanceType & " //water balance model:  1=1D balance, 2=2D balance")
            lWhichFile.AppendLine(aRouteType & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
            SaveFileString(whichmodelFN, lWhichFile.ToString)
        Else
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(whichmodelFN)
                lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        balancetype = StrRetRem(lCurrentRecord)
                        lCurrentRecord = lStreamReader.ReadLine
                        routetype = StrRetRem(lCurrentRecord)
                    End If
                Loop
                lStreamReader.Close()
            Catch e As ApplicationException
                Logger.Msg("Could not open whichModel.txt file" & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        End If

        Dim methodtypestr As String = ""
        If ((balancetype = 1) And (routetype = 2)) Then
            methodtypestr = "One Soil Layer, Muskingum-Cunge Routing"
        ElseIf ((balancetype = 1) And (routetype = 1)) Then
            methodtypestr = "One Soil Layer, Diffusion Routing"
        ElseIf ((balancetype = 1) And (routetype = 3)) Then
            methodtypestr = "One Soil Layer, Lag Routing"
        ElseIf ((balancetype = 2) And (routetype = 2)) Then
            methodtypestr = "Two Soil Layers, Muskingum-Cunge Routing"
        ElseIf ((balancetype = 2) And (routetype = 1)) Then
            methodtypestr = "Two Soil Layers, Diffusion Routing"
        ElseIf ((balancetype = 2) And (routetype = 3)) Then
            methodtypestr = "Two Soil Layers, Lag Routing"
        Else
            methodtypestr = "One Soil Layer, Muskingum-Cunge Routing"
        End If

        logfile.AppendLine("Routing Models Selected as " & methodtypestr.ToString)

        Dim rangeFileFN As String = pOutputPath & "range.txt"
        Dim rangeFile As New StringBuilder
        rangeFile.AppendLine("Min, Max, Name, Description")
        For rindex As Integer = 0 To aParameterRanges.Count - 1
            rangeFile.AppendLine(aParameterRanges.ItemByIndex(rindex) & ", " & aParameterRanges.Keys(rindex))
        Next
        SaveFileString(rangeFileFN, rangeFile.ToString)

        logfile.AppendLine("River and Basin Files Read")

        Dim moscemparamfileFN As String = pOutputPath & "moscem_param.txt"
        Dim moscemparamfile As New StringBuilder
        moscemparamfile.AppendLine(1.ToString)
        moscemparamfile.AppendLine(ntsteps)
        moscemparamfile.AppendLine(ntsteps)
        moscemparamfile.AppendLine(1.ToString)
        moscemparamfile.AppendLine(-9999.ToString)
        moscemparamfile.AppendLine(1.ToString)
        moscemparamfile.AppendLine((aSelectedReachIndex + 1).ToString)
        SaveFileString(moscemparamfileFN, moscemparamfile.ToString)

        logfile.AppendLine("River and Basin Parameter Ranges Confirmed")

        Dim geosfmsarunFN As String = ""
        If FileExists(pOutputPath & "geosfmsarun.exe") Then
            geosfmsarunFN = pOutputPath & "geosfmsarun.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmsarun.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmsarun.exe", pOutputPath & "geosfmsarun.exe")
            geosfmsarunFN = pOutputPath & "geosfmsarun.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmsarun.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("geosfmsarun.exe program copied to working directory")

        Dim geosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            geosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            geosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("geosfm.dll program copied to working directory")

        If Not FileExists(pOutputPath & "dforrt.dll") Then
            If FileExists(lBasinsBinLoc & "\dforrt.dll") Then
                File.Copy(lBasinsBinLoc & "\dforrt.dll", pOutputPath & "dforrt.dll")
            End If
        End If

        Dim lProcess As New Diagnostics.Process
        With lProcess.StartInfo
            .FileName = pOutputPath & "geosfmsarun.exe"
            .WorkingDirectory = pOutputPath
            .Arguments = ""
            .CreateNoWindow = False
            .UseShellExecute = True
        End With
        lProcess.Start()
        While Not lProcess.HasExited
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While

        logfile.AppendLine("Calibration Program Successfully Executed!")
        SaveFileString(logfileFN, logfile.ToString)

        Dim SArunOutputFN As String = pOutputPath & "sarunoutput.txt"
        If FileExists(SArunOutputFN) Then
            Logger.Msg("Sensitivity Analysis Complete. Results written to: sarunoutput.txt", "Geospatial Stream Flow Model")
        Else
            Logger.Msg("Output file, sarunoutput.txt, not found." & vbCrLf & "Check log file, logfilesarun.txt, for possible errors", "Geospatial Stream Flow Model")
        End If
        Return True
    End Function

    Friend Function Calibrate(ByVal aFlowGageNames As atcCollection, ByVal aCalibParms As Collection, ByVal aMaxRuns As Integer, ByVal aObjFunction As Integer, ByVal aSJDate As Double, ByVal aEJDate As Double) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: calibrate.ave
        '
        '      Function: 
        '          The program calibrates basin.txt and river.txt parameters to improve the match  
        '          between simulated and observed models runs. 
        '
        '      Inputs:
        '          a textfile containing hydrologic parameters for each subbasin, basin.txt
        '          a textfile containing hydrologic parameters for each river reach, river.txt
        '          a textfile containing rainfall data for each subbasin, rain.txt
        '          a textfile containing evaporation data for each subbasin, evap.txt
        '          a textfile containing computational for each subbasin, order.txt
        '          a textfile containing parameters for the water balance simulation run, balparam.txt
        '          a textfile containing parameters for the river routing simulation run, routparam.txt
        '          a textfile containing discharge data for reservoirs in the basin, reservoir.txt
        '
        '      Outputs:  
        '          a logfile documenting the pre and post processing of calibration input and output files, logfilecalib.txt
        '          a textfile containing streamflow data for each subbasin, streamflow.txt
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin polygon coverage. basin.shp
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        'TODO: Need example observed streamflow data to run/debug example

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)

        'read/write balance parameters
        Dim lBalParamFN As String = pOutputPath & "balparam.txt"
        Dim lBalList As New Collection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lBalParamFN)
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lBalList.Add(lCurrentRecord)
                End If
            Loop
            lStreamReader.Close()
        Catch e As ApplicationException
            Logger.Msg("Problem reading parameters from balparam.txt file," & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
            Return False
        End Try
        'write it out in calibration mode
        Dim lParamFile As New StringBuilder
        For lIndex As Integer = 1 To 9
            lParamFile.AppendLine(lBalList(lIndex))
        Next
        lParamFile.AppendLine(1.ToString)
        SaveFileString(lBalParamFN, lParamFile.ToString)

        'read/write routing parameters
        Dim lRoutParamFN As String = pOutputPath & "routparam.txt"
        Dim lRoutList As New Collection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lRoutParamFN)
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lRoutList.Add(lCurrentRecord)
                End If
            Loop
            lStreamReader.Close()
        Catch e As ApplicationException
            Logger.Msg("Problem reading parameters from routparam.txt file," & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
            Return False
        End Try
        'write it out in calibration mode
        Dim lRParamFile As New StringBuilder
        For lIndex As Integer = 1 To 9
            lRParamFile.AppendLine(lRoutList(lIndex))
        Next
        lRParamFile.AppendLine(1.ToString)
        SaveFileString(lRoutParamFN, lRParamFile.ToString)

        Dim balfilesfn As String = pOutputPath & "balfiles.txt"
        If Not FileExists(balfilesfn) Then
            Logger.Msg("Could not open balfiles.txt file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim routfilesfn As String = pOutputPath & "routfiles.txt"
        If Not FileExists(routfilesfn) Then
            Logger.Msg("Could not open routfiles.txt file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
            Return False
        End If

        'caliblabels = { "Streamflow Model Parameters File", 
        '          "Optimization Flag File", 
        '          "Observed Streamflow File", 
        '          "Objective Function File", 
        '          "Parameter Multiplier File", 
        '          "Parameter Convergence File", 
        '          "Calibration Input-Ouput File",
        '          "Calibration Configuration File", 
        '          "Streamflow Model Configuration File", 
        '          "Original Basin Parameter File", 
        '          "Original River Parameter File", 
        '          "Final Basin Parameter File", 
        '          "Final River Parameter File",
        '          "Output Streamflow File"}

        '        inpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", caliblabels, calibdefaults)

        Dim parameterFN As String = pOutputPath & "parameter.in"
        Dim objoptflagFN As String = pOutputPath & "objoptflag.in"
        Dim obsflowFN As String = pOutputPath & "observed_streamflow.txt"
        Dim objectiveFN As String = pOutputPath & "objectives.out"
        Dim paramvalFN As String = pOutputPath & "parameter_values.out"
        Dim paramconFN As String = pOutputPath & "par_convergence.out"
        Dim mosceminFN As String = pOutputPath & "moscem.in"
        Dim moscemparamFN As String = pOutputPath & "moscem_param.txt"
        Dim whichModelFN As String = pOutputPath & "whichModel.txt"
        Dim origbasFN As String = pOutputPath & "basin_original.txt"
        Dim origrivFN As String = pOutputPath & "river_original.txt"
        Dim basinFN As String = pOutputPath & "basin.txt"
        Dim riverFN As String = pOutputPath & "river.txt"
        Dim streamflowFN As String = pOutputPath & "streamflow.txt"
        'timeseriesFN = myWkDirname + inplist.get(14)     commented out in the avenue
        'objpostprocFN = myWkDirname + inplist.get(15)
        'trdoffboundFN = myWkDirname + inplist.get(16)
        'postprocinFN = myWkDirname + inplist.get(17)

        Dim logfilename As String = pOutputPath & "logfilecalib.txt"
        Dim logfile As New StringBuilder

        'paraminFile = LineFile.Make(parameterFN.asfilename,#file_perm_write)
        'objectivesFile = LineFile.Make(objectiveFN.asfilename,#file_perm_write)
        'paramvaluesFile = LineFile.Make(paramvalFN.asfilename,#file_perm_write)
        'parconvergeFile = LineFile.Make(paramconFN.asfilename,#file_perm_write)
        'mosceminfile = LineFile.Make(mosceminFN.asfilename,#file_perm_write)
        'moscemparamfile = LineFile.Make(moscemparamFN.asfilename,#file_perm_write)
        'basinfile = LineFile.Make(basinFN.asfilename,#file_perm_read)
        'riverfile = LineFile.Make(riverFN.asfilename,#file_perm_read)
        'streamflowfile = LineFile.Make(streamflowFN.asfilename,#file_perm_write)

        'objoptflagFile = LineFile.Make(objoptflagFN.asfilename,#file_perm_write)
        'If Not FileExists(objoptflagFN) Then
        '    Logger.Msg("Could not open " + objoptflagFN + " file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'will need to access observed flow timeseries here
        Dim lNdates As Integer = aEJDate - aSJDate + 1
        Dim lFlowArray(aFlowGageNames.Count, lNdates) As Single
        Dim lFlowTimeseries As atcTimeseries = Nothing

        Dim lGageIndex As Integer = 0
        For Each lGage As String In aFlowGageNames
            lGageIndex += 1
            For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    If (lDataSet.Attributes.GetValue("Constituent") = "FLOW" And _
                        lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" And _
                        lDataSet.Attributes.GetValue("Location") = lGage) Then
                        lFlowTimeseries = Aggregate(lDataSet, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                    End If
                Next
            Next

            If Not lFlowTimeseries Is Nothing Then
                Dim lStartIndex As Integer = lFlowTimeseries.Dates.IndexOfValue(aSJDate, True)
                If aSJDate = lFlowTimeseries.Dates.Values(0) Or lStartIndex < 0 Then
                    lStartIndex = 0
                End If
                Dim lEndIndex As Integer = lFlowTimeseries.Dates.IndexOfValue(aEJDate, True)
                For lIndex As Integer = lStartIndex To lEndIndex - 1
                    lFlowArray(lGageIndex, lIndex - lStartIndex) = lFlowTimeseries.Values(lIndex + 1)
                Next
            End If
        Next

        'now write out observed flows in the expected format
        Dim obsflowFile As New StringBuilder
        Dim lString As String = ""
        lString = """" & "Date" & """" & ",1"
        For lGageIndex = 2 To aFlowGageNames.Count
            lString = lString & "," & lGageIndex.ToString
        Next
        obsflowFile.AppendLine(lString)
        For lIndex As Integer = 1 To lNdates
            Dim lDate(6) As Integer
            J2Date(aSJDate + lIndex - 1, lDate)
            Dim lYr As Integer = lDate(0)
            Dim lJYr As Double = Date2J(lYr, 1, 1)
            Dim lDaysAfter As Integer = aSJDate + lIndex - lJYr
            lString = lDate(0) & Format(lDaysAfter, "000") & "," & Format(lFlowArray(1, lIndex - 1), "0.0")
            For lGageIndex = 2 To aFlowGageNames.Count
                lString = lString & ", " & Format(lFlowArray(lGageIndex, lIndex - 1), "0.0")
            Next
            obsflowFile.AppendLine(lString)
        Next
        SaveFileString(obsflowFN, obsflowFile.ToString)

        If Not FileExists(origbasFN) Then
            If (File.Exists(basinFN)) Then
                FileCopy(basinFN, origbasFN)
                'origbasinfile = LineFile.Make(origbasFN.asfilename,#file_perm_read)
            Else
                Logger.Msg("Could not open basin.txt or basin_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End If
        End If

        If Not FileExists(origrivFN) Then
            If FileExists(riverFN) Then
                FileCopy(riverFN, origrivFN)
                'origriverfile = LineFile.Make(origrivFN.asfilename,#file_perm_read)
            Else
                Logger.Msg("Could not open " & riverFN & " or " & origrivFN & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End If
        End If

        'postlabels = { "Output Time Series File",
        '          "Output Objective Convergence File",
        '          "Output Tradeoff Boundary File",
        '          "Processing Parameter File"}

        '        inpostList = MsgBox.MultiInput("Enter Post Processing Parameters.", "Geospatial Stream Flow Model", postlabels, postdefaults)

        Dim timeseriesFN As String = pOutputPath & "timeseries.txt"
        Dim objpostprocFN As String = pOutputPath & "objectives_postproc.out"
        Dim trdoffboundFN As String = pOutputPath & "trdoff_bounds.out"
        Dim postprocinFN As String = pOutputPath & "postproc.in"

        'objpostprocFile = LineFile.Make(objpostprocFN.asfilename,#file_perm_write)
        'If Not FileExists(objpostprocFN) Then
        '    Logger.Msg("Could not open " + objpostprocFN + " file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        Dim minparlst(20) As String
        Dim maxparlst(20) As String

        Dim origbsize As Integer = 0
        Dim bigblist As New Collection
        If FileExists(origbasFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(origbasFN)
                lCurrentRecord = lStreamReader.ReadLine  'header line
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        origbsize = origbsize + 1
                        bigblist.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Problem reading basin_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Could not open basin_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
            Return False
        End If

        'store the positions of calibered variables
        Dim magbposlst() As Integer = {1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14}
        Dim magrposlst() As Integer = {6, 3, 14, 7, 8, 12, 13}

        For lIndex As Integer = 1 To 20
            minparlst(lIndex) = 999999
            maxparlst(lIndex) = 0
        Next

        Dim headerblst As New Collection
        Dim lval As Single = 0.0
        For obrec As Integer = 1 To origbsize
            Dim lTmpStr As String = bigblist(obrec)
            headerblst.Add(StrRetRem(lTmpStr))
            For bbrec As Integer = 1 To UBound(magbposlst)
                lval = StrRetRem(lTmpStr)
                'skip parm 5
                If bbrec = 5 Then
                    lval = StrRetRem(lTmpStr)
                End If
                If lval < minparlst(bbrec) Then
                    minparlst(bbrec) = lval
                End If
                If lval > maxparlst(bbrec) Then
                    maxparlst(bbrec) = lval
                End If
            Next
        Next

        Dim origrsize As Integer = 0
        Dim bigrlist As New Collection
        If FileExists(origrivFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(origrivFN)
                lCurrentRecord = lStreamReader.ReadLine   'header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        origrsize = origrsize + 1
                        bigrlist.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Problem reading river_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Could not open river_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim headerlst As New Collection
        For orec As Integer = 1 To origrsize
            Dim lTmpStr As String = bigrlist(orec)
            headerlst.Add(StrRetRem(lTmpStr))
            'Dim magrposlst() As Integer = {6, 3, 14, 7, 8, 12, 13}
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(15) Then
                minparlst(15) = lval
            End If
            If lval > maxparlst(15) Then
                maxparlst(15) = lval
            End If
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(14) Then
                minparlst(14) = lval
            End If
            If lval > maxparlst(14) Then
                maxparlst(14) = lval
            End If
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(17) Then
                minparlst(17) = lval
            End If
            If lval > maxparlst(17) Then
                maxparlst(17) = lval
            End If
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(18) Then
                minparlst(18) = lval
            End If
            If lval > maxparlst(18) Then
                maxparlst(18) = lval
            End If
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(19) Then
                minparlst(19) = lval
            End If
            If lval > maxparlst(19) Then
                maxparlst(19) = lval
            End If
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(20) Then
                minparlst(20) = lval
            End If
            If lval > maxparlst(20) Then
                maxparlst(20) = lval
            End If
            lval = StrRetRem(lTmpStr)
            If lval < minparlst(16) Then
                minparlst(16) = lval
            End If
            If lval > maxparlst(16) Then
                maxparlst(16) = lval
            End If
        Next

        logfile.AppendLine("River and Basin Files Read")

        logfile.AppendLine("Observed Streamflow File Read")

        Dim ncposstr As String = "1"
        Dim nbasidlst As New Collection
        Dim objoptflagFile As New StringBuilder
        Dim ncid As Integer = 0
        For frec As Integer = 0 To aFlowGageNames.Count - 1
            ncid = aFlowGageNames.Keys(frec)
            nbasidlst.Add(ncid)
            If (ncid <> 0) Then
                objoptflagFile.AppendLine("runoff_" & ncid.ToString & "      1")
            End If
        Next
        SaveFileString(objoptflagFN, objoptflagFile.ToString)

        Dim paraminlst(20) As String
        paraminlst(1) = "1,SoilWhc,1,0.1,5,0,Soil water holding capacity (mm)"
        paraminlst(2) = "2,Depth,1,0.1,5,0,Total soil depth (cm)"
        paraminlst(3) = "3,Texture,1,0.3,3,0,Soil texture 1=Sand 2=Loam 3=Clay 5=Water"
        paraminlst(4) = "4,Ks,1,0.1,10,0,Saturated hydraulic conductivity (cm/hr)"
        paraminlst(5) = "5,Interflow,1,0.1,5,0,Interflow storage residence time (days)"
        paraminlst(6) = "6,HSlope,1,0.5,1.5,0,Average subbasin slope"
        paraminlst(7) = "7,Baseflow,1,0.1,5,0,Baseflow reservoir residence time (days)"
        paraminlst(8) = "8,CurveNum,1,0.1,1.5,0,SCS runoff curve number"
        paraminlst(9) = "9,MaxCover,1,0.1,5,0,Fraction of the subbasin with permanently impervious cover"
        paraminlst(10) = "10,BasinLoss,1,0.1,5,0,Fraction of soil water infiltrating to ground water"
        paraminlst(11) = "11,PanCoeff,1,0.1,5,0,Pan coefficient for correcting PET readings"
        paraminlst(12) = "12,TopSoil,1,0.1,5,0,Fraction of soil layer that is hydrologically active"
        paraminlst(13) = "13,RainCalc,1,0.1,5,0,Excess rainfall mode 1=Philip 2=SCS 3=BucketModel"
        paraminlst(14) = "14,RivRough,1,0.1,5,0,River Channel Roughness Coefficient (Manning n)"
        paraminlst(15) = "15,RivSlope,1,0.1,5,0,Average slope of the river"
        paraminlst(16) = "16,RivWidth,1,0.1,5,0,Average channel width (m)"
        paraminlst(17) = "17,RivLoss,1,0.1,5,0,Fraction of flow lost within the river channel"
        paraminlst(18) = "18,RivFPLoss,1,0.1,5,0,Fraction of the river flow lost in floodplain"
        paraminlst(19) = "19,Celerity,1,0.1,5,0,Flood wave celerity (m/s)"
        paraminlst(20) = "20,Diffusion,1,0.05,10,0,Flow attenuation coefficient (m^2/s)"

        Dim data2(20) As String
        Dim data3(20) As String
        Dim data4(20) As String
        Dim data5(20) As String
        Dim data6(20) As String
        Dim data7(20) As String
        Dim ministr As String = ""
        Dim minilst As String = ""
        For mrec As Integer = 1 To 20
            ministr = paraminlst(mrec)
            minilst = StrRetRem(ministr)
            minilst = StrRetRem(ministr)
            data2(mrec) = minilst
            minilst = StrRetRem(ministr)
            data3(mrec) = minilst
            minilst = StrRetRem(ministr)
            data4(mrec) = minilst
            minilst = StrRetRem(ministr)
            data5(mrec) = minilst
            minilst = StrRetRem(ministr)
            data6(mrec) = minilst
            data7(mrec) = ministr
        Next

        '        choices = MsgBox.MultiListAsString(desclst, "Select Parameters to be Calibrated", "Which Parameters Do You Want to Calibrate?")

        Dim lCcount As Integer = aCalibParms.Count
        For crec As Integer = 1 To lCcount
            Dim curchoice As String = aCalibParms(crec)
            For drec As Integer = 1 To 20
                Dim curdesc As String = data2(drec)
                If (curchoice = curdesc) Then
                    Dim chglst As String = paraminlst(drec)
                    Dim lowval As Single = minparlst(drec)
                    Dim lowfrac As Single = data4(drec) / lowval
                    Dim highval As Single = maxparlst(drec)
                    Dim highfrac As Single = data5(drec) / highval
                    paraminlst(drec) = drec.ToString & "," & data2(drec) & "," & data3(drec) & "," & lowfrac & "," & highfrac & ",1," & data7(drec)
                End If
            Next
        Next

        Dim paraminFile As New StringBuilder
        paraminFile.AppendLine("No,Name,Default,Lower,Upper,OptIdx,Description")
        For drec As Integer = 1 To 20
            paraminFile.AppendLine(paraminlst(drec))
        Next
        SaveFileString(parameterFN, paraminFile.ToString)

        logfile.AppendLine("River and Basin Calibration Parameters Identified")

        Dim ccount As Integer = aCalibParms.Count
        Dim ncomplex As Integer = 2
        Dim nsamples As Integer = 2 * ncomplex * ccount
        Dim nummult As Integer = 0
        If (nsamples < 10) Then
            nummult = (10 / (ncomplex * ccount)) + 1
            nsamples = nummult * ncomplex * ccount
        ElseIf (nsamples > 200) Then
            nummult = (100 / (ncomplex * ccount))
            nsamples = nummult * ncomplex * ccount
        End If

        '        runstr = MsgBox.ChoiceAsString(runlist, "Define a Maximum No. of Runs" + nl + "NOTE: Each run could take upto 1 minute!", "Geospatial Stream Flow Model")
        Dim runstr As String = aMaxRuns.ToString

        Dim mosceminfile As New StringBuilder
        mosceminfile.AppendLine(ccount.ToString & ",  nOptPar")
        mosceminfile.AppendLine(aFlowGageNames.Count.ToString & ", nOptObj")
        mosceminfile.AppendLine(nsamples.ToString & ", nSamples")
        mosceminfile.AppendLine(ncomplex.ToString & ", nComplex")
        mosceminfile.AppendLine(runstr + ", nMaxDraw")
        mosceminfile.AppendLine(parameterFN)
        mosceminfile.AppendLine(objoptflagFN)
        mosceminfile.AppendLine(obsflowFN)
        mosceminfile.AppendLine(objectiveFN)
        mosceminfile.AppendLine(paramvalFN)
        mosceminfile.AppendLine(paramconFN)
        mosceminfile.AppendLine(lBalParamFN)
        mosceminfile.AppendLine(moscemparamFN)
        mosceminfile.AppendLine(whichModelFN)
        mosceminfile.AppendLine(origbasFN)
        mosceminfile.AppendLine(origrivFN)
        mosceminfile.AppendLine(basinFN)
        mosceminfile.AppendLine(riverFN)
        mosceminfile.AppendLine(streamflowFN)
        mosceminfile.AppendLine(balfilesfn)
        mosceminfile.AppendLine(routfilesfn)
        SaveFileString(mosceminFN, mosceminfile.ToString)

        Dim caliblst As New Collection
        caliblst.Add("Root Mean Square Error (RMSE)")
        caliblst.Add("Standard Deviation (STD)")
        caliblst.Add("Maximum Likelihood Error (MLE)")
        caliblst.Add("Nash-Sutcliffe Efficiency(NSE)")
        caliblst.Add("Number of Sign Changes (NSC)")
        caliblst.Add("BIAS")

        '        Calibtypestr = MsgBox.ChoiceAsString(caliblst, "Select Objective Function Type", "How Should Convergence Be Measured?")

        Dim calibposition As Integer = aObjFunction

        Dim myrstr As String = ""
        For nrec As Integer = 1 To aFlowGageNames.Count
            Dim lkey As Integer = aFlowGageNames.Keys(nrec - 1)
            Dim rstrnum As Integer = 0
            'where is lkey in streamflow 
            For lindex As Integer = 1 To headerlst.Count
                If headerlst(lindex) = lkey Then
                    rstrnum = lindex
                End If
            Next
            Dim rstr As String = ""
            Dim pnum As Integer = 0
            If (nrec = 1) Then
                rstr = rstrnum.ToString
                pnum = rstrnum
                myrstr = rstr
            Else
                rstr = (rstrnum - pnum).ToString
                pnum = rstrnum
                myrstr = myrstr + ", " + rstr
            End If
        Next

        logfile.AppendLine("Calibration Method set as " & caliblst(calibposition))

        Dim moscemparamfile As New StringBuilder
        moscemparamfile.AppendLine(aFlowGageNames.Count.ToString & "  //nflux")
        moscemparamfile.AppendLine(lRoutList(1).ToString & "  //ntstep1")
        moscemparamfile.AppendLine(CInt(lRoutList(1)) + CInt(lRoutList(7)) & "  //ntstep2")
        moscemparamfile.AppendLine(calibposition.ToString & "  //obj_func")
        moscemparamfile.AppendLine("-9999  //missing value")
        moscemparamfile.AppendLine(ncposstr & "  //nflux_obs, column in observed_streamflow.txt to test (not including timestep)")
        moscemparamfile.AppendLine(myrstr & "  //nflux_model, column in streamflow.txt (not including timestep)")
        SaveFileString(moscemparamFN, moscemparamfile.ToString)

        logfile.AppendLine("Starting Time:" & " " & System.DateTime.Now.ToString)

        Dim geosfmcalibFN As String = ""
        If FileExists(pOutputPath & "geosfmcalib.exe") Then
            geosfmcalibFN = pOutputPath & "geosfmcalib.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmcalib.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmcalib.exe", pOutputPath & "geosfmcalib.exe")
            geosfmcalibFN = pOutputPath & "geosfmcalib.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmcalib.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("geosfmcalib.exe program copied to working directory")

        Dim geosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            geosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            geosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("geosfm.dll program copied to working directory")

        logfile.AppendLine("Performing Model Calibration with " & runstr.ToString & " ........")

        Dim lProcess As New Diagnostics.Process
        With lProcess.StartInfo
            .FileName = pOutputPath & "geosfmcalib.exe"
            .WorkingDirectory = pOutputPath
            .Arguments = mosceminFN
            .CreateNoWindow = False
            .UseShellExecute = True
        End With
        lProcess.Start()
        While Not lProcess.HasExited
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While

        Dim pcfilesize As Integer = 0
        If FileExists(paramconFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(paramconFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        pcfilesize = pcfilesize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & paramconFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & paramconFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        If (pcfilesize > 1) Then
            logfile.AppendLine("Calibration Program Successfully Executed!")
        Else
            logfile.AppendLine("Problems encounted during calibration.")
            logfile.AppendLine("Output parameter convergence file, " + paramconFN + ", is empty.")
            SaveFileString(logfilename, logfile.ToString)
            Logger.Msg("Calibration problems encountered." & vbCrLf & "Output parameter convergence file, " & paramconFN & ", is empty", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim postprocinFile As New StringBuilder
        postprocinFile.AppendLine(nsamples.ToString + ", nSamples")
        postprocinFile.AppendLine(obsflowFN)
        postprocinFile.AppendLine(paramvalFN)
        postprocinFile.AppendLine(objectiveFN)
        postprocinFile.AppendLine(timeseriesFN)
        postprocinFile.AppendLine(objpostprocFN)
        postprocinFile.AppendLine(trdoffboundFN)
        postprocinFile.AppendLine(lBalParamFN)
        postprocinFile.AppendLine(moscemparamFN)
        postprocinFile.AppendLine(parameterFN)
        postprocinFile.AppendLine(basinFN)
        postprocinFile.AppendLine(riverFN)
        postprocinFile.AppendLine(origbasFN)
        postprocinFile.AppendLine(origrivFN)
        postprocinFile.AppendLine(balfilesfn)
        postprocinFile.AppendLine(routfilesfn)
        postprocinFile.AppendLine(whichModelFN)
        SaveFileString(postprocinFN, postprocinFile.ToString)

        logfile.AppendLine("Initiating Post Processing Program...")

        Dim geosfmpostFN As String = ""
        If FileExists(pOutputPath & "geosfmpost.exe") Then
            geosfmpostFN = pOutputPath & "geosfmpost.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmpost.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmpost.exe", pOutputPath & "geosfmpost.exe")
            geosfmpostFN = pOutputPath & "geosfmpost.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmpost.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("Performing Model PostProcessing........")

        Dim lPProcess As New Diagnostics.Process
        With lPProcess.StartInfo
            .FileName = pOutputPath & "geosfmpost.exe"
            .WorkingDirectory = pOutputPath
            .Arguments = "postproc.in"
            .CreateNoWindow = False
            .UseShellExecute = True
        End With
        lPProcess.Start()
        While Not lPProcess.HasExited
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While

        Dim tsfilesize As Integer = 0
        If FileExists(timeseriesFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(timeseriesFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        tsfilesize = tsfilesize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & timeseriesFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & timeseriesFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        If (tsfilesize > 1) Then
            FileCopy(timeseriesFN, pOutputPath & "tmpfile.txt")
            logfile.AppendLine("Post Processing Program Successfully Executed!")
        Else
            logfile.AppendLine("Problems encounted during post processing.")
            logfile.AppendLine("Output time series file, " & timeseriesFN & ", is empty.")
            SaveFileString(logfilename, logfile.ToString)
            Logger.Msg("Post processing problems encountered." & vbCrLf & "Output time series file, " & timeseriesFN & ", is empty", "Geospatial Stream Flow Model")
            Return False
        End If

        logfile.AppendLine("Ending Time:" & " " & DateTime.Now.ToString)
        SaveFileString(logfilename, logfile.ToString)

        Logger.Msg("Model Calibration Run Complete." & vbCrLf & "Results written to: " & vbCrLf & timeseriesFN, "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function BankFull() As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: bankfull.ave
        '
        '      Function: 
        '          The program computes bankfull (= median annual maxima) for each subbasin.
        '            also computes percentiles, variance and mean maximum flows.  
        '          . 
        '
        '      Inputs:
        '          a textfile containing hydrologic parameters for each subbasin, basin.txt
        '          a textfile containing rainfall data for each subbasin, soilwater.txt
        '          a textfile containing evaporation data for each subbasin, response.txt
        '          a textfile containing computational for each subbasin, order.txt
        '          a textfile containing parameters specific to the simulation run, parameter.txt
        '          a textfile containing discharge data for reservoirs in the basin, reservoir.txt
        '
        '      Outputs:  
        '          a textfile containing daily water balance for each reservoir in the basin, damstatus.txt
        '          a textfile containing streamflow data for each subbasin, streamflow.txt
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the basin polygon coverage. basin.shp
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        
        '        'CREATE A LIST OF INPUT LABELS
        'labels = { "Input Daily Time Series File", "Output Monthly Time Series File", "Output Annual Time Series File" } 

        'defaults = { "streamflow.txt", "monthlyflow.txt", "annualflow.txt" }

        Dim lRunoffFN As String = pOutputPath & "streamflow.txt"
        Dim lMonthlyFN As String = pOutputPath & "monthlyflow.txt"
        Dim lAnnualFN As String = pOutputPath & "annualflow.txt"

        Dim lChkFN As String = pOutputPath & "times.txt"
        Dim lParamFN As String = pOutputPath & "statsparam.txt"

        If Not FileExists(lRunoffFN) Then
            Logger.Msg("Could not open time series file," & vbCrLf & lRunoffFN, "GeoSFM Utilities")
            Return False
        End If

        Dim lLogFN As String = pOutputPath & "logfilestats.txt"

        Dim lChkFile As New StringBuilder
        lChkFile.AppendLine("Starting Time:" & " " & System.DateTime.Now.ToString)

        Dim lRunoffList As New Collection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lRunoffFN)
            lCurrentRecord = lStreamReader.ReadLine
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    lRunoffList.Add(lCurrentRecord)
                End If
            Loop
            lStreamReader.Close()
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lRunoffFN & vbCrLf & "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lRunoffSize As Integer = lRunoffList.Count
        Dim lUseYear As Integer = 1
        If (lRunoffSize = 0) Then
            Logger.Msg("The runofffile " & vbCrLf & lRunoffFN & " is empty", "")
            Return False
        ElseIf ((lRunoffSize < 3285) And (lRunoffSize > 280)) Then
            Logger.Msg("Less than 9 years of data supplied." & vbCrLf & vbCrLf & "Computing bankfull from monthly data...", "")
            lUseYear = 12
        ElseIf (lRunoffSize < 270) Then
            Logger.Msg("Less than 9 month of data supplied." & vbCrLf & vbCrLf & "Computing bankfull from daily data...", "")
            lUseYear = 365
        End If

        Dim lRec As String = lRunoffList(1)
        Dim lDayOneStr As String = StrRetRem(lRec)
        lRec = lRunoffList(lRunoffList.Count)
        Dim lEndDayStr As String = StrRetRem(lRec)

        Dim lStartYear As String = ""
        Dim lStartDay As String = ""
        lStartYear = Left(lDayOneStr, 4)
        If Not IsNumeric(lStartYear) Then
            Logger.Msg("Start year must be a 4 digit number", "GeoSFM Utilities")
            Return False
        End If
        lStartDay = Right(lDayOneStr, 3)
        If Not IsNumeric(lStartDay) Then
            Logger.Msg("Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
            Return False
        End If

        Dim lEndyear As String = ""
        Dim lEndday As String = ""
        lEndyear = Left(lEndDayStr, 4)
        If Not IsNumeric(lEndyear) Then
            Logger.Msg("End year must be a 4 digit number", "GeoSFM Utilities")
            Return False
        End If
        lEndday = Right(lEndDayStr, 3)
        If Not IsNumeric(lEndday) Then
            Logger.Msg("End day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
            Return False
        End If

        Dim lSDate(5) As Integer
        lSDate(0) = CInt(lStartYear)
        lSDate(1) = 1
        lSDate(2) = 1
        Dim lSJDate As Double = Date2J(lSDate) + lStartDay - 1
        J2Date(lSJDate, lSDate)

        Dim lStartMonth As String = lSDate(1).ToString
        Dim lStartDayofMonth As String = lSDate(2).ToString

        If CInt(lEndyear) < CInt(lStartYear) Then
            Logger.Msg("End year must be greater than or equal to start year", "GeoSFM Utilities")
            Return False
        End If

        '        'statslist = { "Max" , "Mean"}            'commented in the avenue
        '        'statstype = msgbox.choiceasstring(statslist, "Select Statistic to be Computed" , "GeoSFM Utilities" )
        '        'if(statstype = nil ) then
        '        '  exit    
        '        'end
        Dim lStatsType As String = "Max"

        Dim lRunoffDays As Integer = lRunoffList.Count
        Dim lBasinSize As Integer = 0
        Dim lStr As String = lRunoffList(1)
        Dim lStr1 As String = ""
        Do While lStr.Length > 0
            lStr1 = StrRetRem(lStr)
            lBasinSize += 1
        Loop

        Dim lNumofYears As Integer = CInt(lEndyear) - CInt(lStartYear) + 1

        Dim lParamFile As New StringBuilder
        lParamFile.AppendLine(lStartYear)
        lParamFile.AppendLine(lStartMonth)
        lParamFile.AppendLine(lStartDayofMonth)
        lParamFile.AppendLine(lEndyear)
        lParamFile.AppendLine(lRunoffDays.ToString)
        lParamFile.AppendLine(lBasinSize.ToString)
        lParamFile.AppendLine(lStatsType.ToString)
        lParamFile.AppendLine(lRunoffFN)
        lParamFile.AppendLine(lMonthlyFN)
        lParamFile.AppendLine(lAnnualFN)
        SaveFileString(lParamFN, lParamFile.ToString)

        '        '
        '        'myfilename = ("$AVEXT\geosfmstats.dll").AsFileName                 'commented in the avenue
        '        'if(myfilename = Nil) then
        '        '  msgbox.info("Unable to locate the program file: geosfmstats.dll."+nl+nl+"Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities") 
        '        '  exit
        '        'end
        '        '
        '        'mydll = DLL.Make(myfilename)
        '        'if(mydll = Nil) then
        '        '  msgbox.info("Unable to access the dll, geosfmstats.dll"+nl+nl+"Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities") 
        '        '  exit
        '        'end 
        '        '
        '        '
        '        'myproc=DLLProc.Make(myDLL, "aggregateflows", #DLLPROC_TYPE_INT32,{#DLLPROC_TYPE_VOID})
        '        'if(myproc = Nil) then
        '        '  msgbox.info("Unable to make the procedure, aggregateflows"+nl+nl+"Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities") 
        '        '  exit
        '        'end 
        '        '  
        '        'doMore = av.SetWorkingStatus
        '        'if (doMore = False) then
        '        '  exit
        '        'end
        '        '
        '        'procGetLastError=myproc.call({})
        '        '

        Dim lGeoSFMstatsDllFN As String = ""
        If FileExists(pOutputPath & "geosfmstats.dll") Then
            lGeoSFMstatsDllFN = pOutputPath & "geosfmstats.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmstats.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfmstats.dll", pOutputPath & "geosfmstats.dll")
            lGeoSFMstatsDllFN = pOutputPath & "geosfmstats.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfmstats.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        'aggregateflows()   now appears to be done using geosfmstats.exe 

        Dim lGeoSFMstatsFN As String = ""
        If FileExists(pOutputPath & "geosfmstats.exe") Then
            lGeoSFMstatsFN = pOutputPath & "geosfmstats.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmstats.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmstats.exe", pOutputPath & "geosfmstats.exe")
            lGeoSFMstatsFN = pOutputPath & "geosfmstats.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmstats.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lPProcess As New Diagnostics.Process
        With lPProcess.StartInfo
            .FileName = pOutputPath & "geosfmstats.exe"
            .WorkingDirectory = pOutputPath
            .Arguments = ""
            .CreateNoWindow = False
            .UseShellExecute = True
        End With
        lPProcess.Start()
        While Not lPProcess.HasExited
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While

        Dim lMonthFileSize As Integer = 0
        If FileExists(lMonthlyFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lMonthlyFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lMonthFileSize = lMonthFileSize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lMonthlyFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lMonthlyFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lAnnualFileSize As Integer = 0
        If FileExists(lAnnualFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lAnnualFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lAnnualFileSize = lAnnualFileSize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lAnnualFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lAnnualFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lDailyFilename As String = lRunoffFN

        Dim lResultFilename As String = ""
        If (lUseYear = 365) Then
            lResultFilename = lDailyFilename
        ElseIf (lUseYear = 12) Then
            lResultFilename = lMonthlyFN
        Else
            lResultFilename = lAnnualFN
        End If

        'select parameter to update
        Dim lChksum As Integer = 1
        Dim lChkcnt As Integer = 1
        Dim lChkavg As Integer = 1
        Dim lChkmax As Integer = 1
        Dim lChkmin As Integer = 1
        Dim lChkrng As Integer = 1
        Dim lChkvar As Integer = 1
        Dim lChkstd As Integer = 1
        Dim lChkbkf As Integer = 1
        Dim lChkmed As Integer = 1
        Dim lChkq25 As Integer = 1
        Dim lChkq33 As Integer = 1
        Dim lChkq66 As Integer = 1
        Dim lChkq75 As Integer = 1

        Dim lUpdatethr As Boolean = True

        Dim lSubTheme As Integer = -1
        Dim lSumfld As Integer = -1
        Dim lMeanfld As Integer = -1
        Dim lMaxfld As Integer = -1
        Dim lMinfld As Integer = -1
        Dim lStddevfld As Integer = -1
        Dim lVarfld As Integer = -1
        Dim lRangefld As Integer = -1
        Dim lCountfld As Integer = -1
        Dim lMedianfld As Integer = -1
        Dim lQuart25fld As Integer = -1
        Dim lQuart75fld As Integer = -1
        Dim lQuart33fld As Integer = -1
        Dim lQuart66fld As Integer = -1
        Dim lHighflowfld As Integer = -1
        Dim lLowflowfld As Integer = -1
        Dim lMedflowfld As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            lSubTheme = GisUtil.LayerIndex("Subbasins")
            lSumfld = AddField(lSubTheme, "Sum")
            lMeanfld = AddField(lSubTheme, "Mean")
            lMaxfld = AddField(lSubTheme, "Maximum")
            lMinfld = AddField(lSubTheme, "Minimum")
            lStddevfld = AddField(lSubTheme, "StdDev")
            lVarfld = AddField(lSubTheme, "Variance")
            lRangefld = AddField(lSubTheme, "Range")
            lCountfld = AddField(lSubTheme, "Count")
            lMedianfld = AddField(lSubTheme, "Median")
            lQuart25fld = AddField(lSubTheme, "Quart25")
            lQuart75fld = AddField(lSubTheme, "Quart75")
            lQuart33fld = AddField(lSubTheme, "Quart33")
            lQuart66fld = AddField(lSubTheme, "Quart66")
            lHighflowfld = AddField(lSubTheme, "Highflow")
            lLowflowfld = AddField(lSubTheme, "Lowflow")
            lMedflowfld = AddField(lSubTheme, "Medflow")
        End If

        Dim lPidfield As Integer = 0
        If GisUtil.IsField(lSubTheme, "Gridcode") Then
            lPidfield = GisUtil.FieldIndex(lSubTheme, "Gridcode")
        Else
            If GisUtil.IsField(lSubTheme, "PolygonID") Then
                lPidfield = GisUtil.FieldIndex(lSubTheme, "PolygonID")
            End If
        End If

        Dim lPolyidfld As Integer = 0
        If GisUtil.IsField(lSubTheme, "Id") Then
            lPolyidfld = GisUtil.FieldIndex(lSubTheme, "Id")
        Else
            If GisUtil.IsField(lSubTheme, "PolygonID") Then
                lPolyidfld = GisUtil.FieldIndex(lSubTheme, "PolygonID")
            End If
        End If

        Dim lPNumRecs As Integer = GisUtil.NumFeatures(lSubTheme)
        Dim lPidList As New Collection
        Dim lPidVal As String = ""
        For lPRec As Integer = 0 To lPNumRecs - 1
            lPidVal = GisUtil.FieldValue(lSubTheme, lPRec, lPidfield)
            lPidList.Add(lPidVal)
        Next

        'read result file into memory
        Dim lNumRecs As Integer = 0
        Dim lResultRecs As New Collection
        If FileExists(lResultFilename) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lResultFilename)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lNumRecs = lNumRecs + 1
                        lResultRecs.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lResultFilename & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lResultFilename & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lNumFlds As Integer = 0
        lStr = lRunoffList(1)
        lStr1 = ""
        Do While lStr.Length > 0
            lStr1 = StrRetRem(lStr)
            lNumFlds += 1
        Loop

        'read results into local array
        Dim lResultVals(lNumRecs, lNumFlds) As Single
        Dim lIrec As Integer = 0
        For Each lRec In lResultRecs
            lIrec += 1
            Dim lIfield As Integer = -1
            Do While lRec.Length > 0
                lStr1 = StrRetRem(lRec)
                lIfield += 1
                If IsNumeric(lStr1) Then
                    lResultVals(lIrec, lIfield) = lStr1
                End If
            Loop
        Next

        Dim lTheSum As Single = 0
        Dim lTheCount As Single = 0
        Dim lTheMean As Single = 0
        Dim lTheMinimum As Single = 0
        Dim lTheMaximum As Single = 0
        Dim lTheRange As Single = 0
        Dim lTheStdDev As Single = 0
        Dim lTheVariance As Single = 0
        Dim lTheMedian As Single = 0
        Dim lTheQuart25 As Single = 0
        Dim lTheQuart75 As Single = 0
        Dim lTheQuart33 As Single = 0
        Dim lTheQuart66 As Single = 0

        Dim lTempCount As Integer = 0
        Dim lMaxMedian As Single = 0
        For lFieldIndex As Integer = 1 To lNumFlds - 1
            lTheSum = 0
            lTheCount = 0
            lTheMinimum = -9999999
            lTheMaximum = 9999999

            Dim lSubbasinId As Integer = lResultVals(1, lFieldIndex)
            Dim lRecList As New atcCollection
            Dim lTheValue As String
            For lRecIndex As Integer = 2 To lNumRecs
                lTheValue = lResultVals(lRecIndex, lFieldIndex)
                If IsNumeric(lTheValue) Then
                    lRecList.Add(lTheValue)
                    lTheSum = lTheValue + lTheSum
                    lTheCount = lTheCount + 1
                End If
            Next

            If lTheCount > 0 Then
                lTheMean = lTheSum / lTheCount
            Else
                lTheMean = -999
            End If

            lRecList.SortByValue()
            Dim lRecCount As Integer = lRecList.Count

            lTheMinimum = lRecList(0)
            lTheMaximum = lRecList(lRecCount - 1)
            lTheRange = (lTheMaximum - lTheMinimum)

            If ((lChkq25 = 1) Or (lChkq33 = 1) Or (lChkq66 = 1) Or (lChkq75 = 1) Or (lChkmed = 1) Or (lChkbkf = 1)) Then

                If (lChkmed = 1) Then
                    If (lRecCount < 9) Then
                        Logger.Msg("Too few records for flow statistics computation", "Less than 9 records")
                        Return False
                    ElseIf (((lRecCount) Mod (2)) = 1) Then
                        lTheMedian = lRecList((lRecCount - 1) / 2)
                    ElseIf (((lRecCount) Mod (2)) = 0) Then
                        lTheMedian = (CSng((lRecList((lRecCount - 2) / 2)) + CSng(lRecList((lRecCount - 1) / 2))) / 2)
                    End If
                End If

                If ((lChkq25 = 1) Or (lChkq75 = 1)) Then
                    If (lRecCount < 9) Then
                        Logger.Msg("Too few records for flow statistics computation", "Less than 9 records")
                        Return False
                    ElseIf (((lRecCount) Mod (2)) = 1) Then
                        If (((lRecCount) Mod (4)) = 1) Then
                            lTheQuart25 = lRecList((lRecCount - 1) / 4)
                            lTheQuart75 = lRecList((lRecCount - 1) * 3 / 4)
                        ElseIf (((lRecCount) Mod (4)) = 3) Then
                            lTheQuart25 = (CSng((lRecList((lRecCount + 1) / 4)) + CSng(lRecList((lRecCount - 3) / 4))) / 2)
                            lTheQuart75 = (CSng((lRecList((lRecCount + 1) * 3 / 4)) + CSng(lRecList(((lRecCount + 1) * 3 / 4) - 1))) / 2)
                        End If
                    ElseIf (((lRecCount) Mod (2)) = 0) Then
                        If (((lRecCount) Mod (4)) = 2) Then
                            lTheQuart25 = lRecList((lRecCount - 2) / 4)
                            lTheQuart75 = lRecList((lRecCount - 2) * 3 / 4)
                        ElseIf (((lRecCount) Mod (4)) = 0) Then
                            lTheQuart25 = (CSng((lRecList((lRecCount - 4) / 4)) + CSng(lRecList((lRecCount) / 4))) / 2)
                            lTheQuart75 = (CSng((lRecList((lRecCount * 3 / 4) - 1)) + CSng(lRecList((lRecCount) * 3 / 4))) / 2)
                        End If
                    End If
                End If

                If ((lChkq33 = 1) Or (lChkq66 = 1)) Then
                    If ((((lRecCount) Mod (3)) = 0) And (lRecCount >= 9)) Then
                        lTheQuart33 = (CSng((lRecList((lRecCount) / 3)) * 2 / 3) + CSng((lRecList((lRecCount - 3) / 3)) * 1 / 3))
                        lTheQuart66 = (CSng((lRecList((lRecCount) * 2 / 3)) * 1 / 3) + CSng((lRecList((lRecCount - 1.5) * 2 / 3)) * 2 / 3))
                    ElseIf ((((lRecCount) Mod (3)) = 1) And (lRecCount >= 9)) Then
                        lTheQuart33 = (lRecList((lRecCount - 1) / 3))
                        lTheQuart66 = (lRecList((lRecCount - 4) * 2 / 3))
                    ElseIf ((((lRecCount) Mod (3)) = 2) And (lRecCount >= 9)) Then
                        lTheQuart33 = (CSng((lRecList((lRecCount - 2) / 3)) * 2 / 3) + CSng((lRecList((lRecCount + 1) / 3)) * 1 / 3))
                        lTheQuart66 = (CSng((lRecList((lRecCount - 2) * 2 / 3)) * 2 / 3) + CSng((lRecList(((lRecCount - 0.5) * 2 / 3) + 1)) * 1 / 3))
                    Else
                        lTheQuart33 = 0
                        lTheQuart66 = 0
                    End If
                End If
            End If

            If ((lChkstd = 1) Or (lChkvar = 1)) Then
                Dim lTheSqDev As Double = 0.0
                Dim lTheSumSqDev As Double = 0.0
                For lRecInx As Integer = 0 To (lNumRecs - 1)
                    lTheValue = lResultVals(lRecInx, lFieldIndex)
                    If (IsNumeric(lTheValue)) Then
                        lTheSqDev = (lTheValue - lTheMean) * (lTheValue - lTheMean)
                        lTheSumSqDev = lTheSqDev + lTheSumSqDev
                    End If
                Next

                If (lTheCount > 1) Then
                    lTheVariance = lTheSumSqDev / (lTheCount - 1)
                    lTheStdDev = Math.Sqrt(lTheVariance)
                Else
                    lTheVariance = 0
                    lTheStdDev = 0
                End If
            End If

            If lTheMedian > lMaxMedian Then
                lMaxMedian = SignificantDigits(lTheMedian, 4)
            End If

            GisUtil.StartSetFeatureValue(lSubTheme)
            For lPRecs As Integer = 0 To GisUtil.NumFeatures(lSubTheme) - 1
                Dim cpid As Integer = GisUtil.FieldValue(lSubTheme, lPRecs, lPidfield)
                If (cpid = lSubbasinId) Then
                    If (lChksum = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lSumfld, lPRecs, SignificantDigits(lTheSum, 4))
                    End If
                    If (lChkcnt = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lCountfld, lPRecs, SignificantDigits(lTheCount, 4))
                    End If
                    If (lChkavg = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lMeanfld, lPRecs, SignificantDigits(lTheMean, 4))
                    End If
                    If (lChkmin = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lMinfld, lPRecs, SignificantDigits(lTheMinimum, 4))
                    End If
                    If (lChkmax = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lMaxfld, lPRecs, SignificantDigits(lTheMaximum, 4))
                    End If
                    If (lChkrng = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lRangefld, lPRecs, SignificantDigits(lTheRange, 4))
                    End If
                    If (lChkstd = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lStddevfld, lPRecs, SignificantDigits(lTheStdDev, 4))
                    End If
                    If (lChkvar = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lVarfld, lPRecs, lTheVariance)
                    End If
                    If (lChkmed = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lMedianfld, lPRecs, SignificantDigits(lTheMedian, 4))
                    End If
                    If (lChkq25 = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lQuart25fld, lPRecs, SignificantDigits(lTheQuart25, 4))
                    End If
                    If (lChkq33 = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lQuart33fld, lPRecs, SignificantDigits(lTheQuart33, 4))
                    End If
                    If (lChkq66 = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lQuart66fld, lPRecs, SignificantDigits(lTheQuart66, 4))
                    End If
                    If (lChkq75 = 1) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lQuart75fld, lPRecs, SignificantDigits(lTheQuart75, 4))
                    End If

                    If (lUpdatethr = True) Then
                        Dim lMyMean As Single = GisUtil.FieldValue(lSubTheme, lPRecs, lMeanfld)
                        Dim lMyMax As Single = GisUtil.FieldValue(lSubTheme, lPRecs, lMaxfld)
                        Dim lMyMin As Single = GisUtil.FieldValue(lSubTheme, lPRecs, lMinfld)
                        Dim lMyHigh As Single = Math.Sqrt(lMyMean * lMyMax)
                        Dim lMyLow As Single = Math.Sqrt(lMyMean)
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lHighflowfld, lPRecs, SignificantDigits(lMyHigh, 4))
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lMedflowfld, lPRecs, SignificantDigits(lMyMean, 4))
                        GisUtil.SetFeatureValueNoStartStop(lSubTheme, lLowflowfld, lPRecs, SignificantDigits(lMyLow, 4))
                    End If
                End If
            Next
            GisUtil.StopSetFeatureValue(lSubTheme)
        Next

        'render the map layer by median flow
        Dim lInc As Single = lMaxMedian / 5
        Dim lColors As New Collection
        lColors.Add(System.Convert.ToUInt32(RGB(250, 250, 0))) 'yellow
        lColors.Add(System.Convert.ToUInt32(RGB(200, 255, 32)))   'lt green
        lColors.Add(System.Convert.ToUInt32(RGB(0, 240, 0)))   'green
        lColors.Add(System.Convert.ToUInt32(RGB(0, 200, 122)))   '
        lColors.Add(System.Convert.ToUInt32(RGB(0, 125, 200)))   'blue
        Dim lLowRange As New Collection
        lLowRange.Add(0.0)
        lLowRange.Add(1 * lInc)
        lLowRange.Add(2 * lInc)
        lLowRange.Add(3 * lInc)
        lLowRange.Add(4 * lInc)
        Dim lHighRange As New Collection
        lHighRange.Add(1 * lInc)
        lHighRange.Add(2 * lInc)
        lHighRange.Add(3 * lInc)
        lHighRange.Add(4 * lInc)
        lHighRange.Add(6 * lInc)
        Dim lCaptions As New Collection
        lCaptions.Add("Median Flow in cms " & lLowRange(1).ToString & " - " & lHighRange(1).ToString)
        lCaptions.Add(lLowRange(2).ToString & " - " & lHighRange(2).ToString)
        lCaptions.Add(lLowRange(3).ToString & " - " & lHighRange(3).ToString)
        lCaptions.Add(lLowRange(4).ToString & " - " & lHighRange(4).ToString)
        lCaptions.Add(lLowRange(5).ToString & " + ")
        GisUtil.SetLayerRendererWithRanges(lSubTheme, lMedianfld, lColors, lCaptions, lLowRange, lHighRange)

        'write basin attribute table to text file
        Dim lRivStatsFile As New StringBuilder
        lStr = GisUtil.FieldName(0, lSubTheme)
        For lCol As Integer = 1 To GisUtil.NumFields(lSubTheme) - 1
            lStr = lStr & ", " & GisUtil.FieldName(lCol, lSubTheme)
        Next
        lRivStatsFile.AppendLine(lStr)
        For lRow As Integer = 0 To GisUtil.NumFeatures(lSubTheme) - 1
            lStr = GisUtil.FieldValue(lSubTheme, lRow, 0)
            For lCol As Integer = 1 To GisUtil.NumFields(lSubTheme) - 1
                lStr = lStr & ", " & GisUtil.FieldValue(lSubTheme, lRow, lCol)
            Next
            lRivStatsFile.AppendLine(lStr)
        Next
        SaveFileString(pOutputPath & "riverstats.txt", lRivStatsFile.ToString)

        Logger.Msg("Bankfull and Flow Characteristics Computed. Outputs written to: " & vbCrLf & vbCrLf & "      " & pOutputPath & "riverstats.txt", "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function PlotMap(ByVal aStreamflow As Boolean, ByVal aMapJDate As Double) As Boolean
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: plotmap.ave
        '
        '      Function: 
        '          Produces a color-coded map based on the magnitude of the selected day's flow
        '          compared to the preassigned or computed high/medium/low flows for each subbasin
        '
        '      Inputs: 
        '          a polygon coverage of subbasins
        '          a table (streamflow.txt) containing streamflow values for each subbasin
        '
        '      Outputs:  
        '          a color-coded map based, ranking flow in each subbasin as high/medium/low
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the coverage/grid of subbasins
        '          the project contains the streamflow table, streamflow.txt
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)

        Dim lResultFileName As String = ""
        If aStreamflow Then
            lResultFileName = pOutputPath & "streamflow.txt"
        Else
            lResultFileName = pOutputPath & "soilwater.txt"
        End If

        If Not FileExists(lResultFileName) Then
            Logger.Msg("Could not open time series file," & vbCrLf & lResultFileName, "GeoSFM Utilities")
            Return False
        End If

        Dim lSubThemeIndex As Integer = -1
        Dim lHighFlowFld As Integer = -1
        Dim lLowFlowFld As Integer = -1
        Dim lMedFlowFld As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            lSubThemeIndex = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(lSubThemeIndex, "Highflow") Then
                lHighFlowFld = GisUtil.FieldIndex(lSubThemeIndex, "Highflow")
            End If
            If GisUtil.IsField(lSubThemeIndex, "Lowflow") Then
                lLowFlowFld = GisUtil.FieldIndex(lSubThemeIndex, "Lowflow")
            End If
            If GisUtil.IsField(lSubThemeIndex, "Medflow") Then
                lMedFlowFld = GisUtil.FieldIndex(lSubThemeIndex, "Medflow")
            End If
        End If

        If (lSubThemeIndex = -1) Then
            Logger.Msg("The map must contain a layer named 'Subbasins' to use this feature.", "Geospatial Stream Flow Model")
            Return False
        End If

        If (lHighFlowFld = -1) Then
            Logger.Msg("High flow field, Highflow, has not been initialized.  Run 'Bankfull and Flow Statistics' to compute this field.", "Geospatial Stream Flow Model")
            Return False
        End If
        If (lLowFlowFld = -1) Then
            Logger.Msg("Low flow field, Lowflow, has not been initialized.  Run 'Bankfull and Flow Statistics' to compute this field.", "Geospatial Stream Flow Model")
            Return False
        End If
        If (lMedFlowFld = -1) Then
            Logger.Msg("Median flow field, Medflow, has not been initialized.  Run 'Bankfull and Flow Statistics' to compute this field.", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lPidfield As Integer = 0
        If GisUtil.IsField(lSubThemeIndex, "Gridcode") Then
            lPidfield = GisUtil.FieldIndex(lSubThemeIndex, "Gridcode")
        Else
            If GisUtil.IsField(lSubThemeIndex, "PolygonID") Then
                lPidfield = GisUtil.FieldIndex(lSubThemeIndex, "PolygonID")
            End If
        End If
        If lPidfield < 0 Then
            Logger.Msg("Subbasins layer must have a field named 'Gridcode'.", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lCurFlowFld As Integer = AddField(lSubThemeIndex, "FlowNow")
        Dim lCurIndexFld As Integer = AddField(lSubThemeIndex, "IndexNow", 1)

        'read result file into memory
        Dim lNumRecs As Integer = 0
        Dim lResultRecs As New Collection
        If FileExists(lResultFileName) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lResultFileName)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lNumRecs = lNumRecs + 1
                        lResultRecs.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lResultFileName & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lResultFileName & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lNumFlds As Integer = 0
        Dim lstr As String = lResultRecs(1)
        Dim lstr1 As String = ""
        Do While lstr.Length > 0
            lstr1 = StrRetRem(lstr)
            lNumFlds += 1
        Loop

        'read results into local array
        Dim lResultVals(lNumRecs, lNumFlds) As Single
        Dim lIrec As Integer = 0
        For Each lRec As String In lResultRecs
            lIrec += 1
            Dim lIfield As Integer = -1
            Do While lRec.Length > 0
                lstr1 = StrRetRem(lRec)
                lIfield += 1
                If IsNumeric(lstr1) Then
                    lResultVals(lIrec, lIfield) = lstr1
                End If
            Loop
        Next

        'figure out date string from julian date
        Dim lDate(6) As Integer
        J2Date(aMapJDate, lDate)
        Dim lYr As Integer = lDate(0)
        Dim lJYr As Double = Date2J(lYr, 1, 1)
        Dim lDaysAfter As Integer = aMapJDate - lJYr + 1
        Dim lDateString As String = lDate(0) & Format(lDaysAfter, "000")

        Dim lPlotDay As Integer = 0
        For lRow As Integer = 1 To lNumRecs
            If lResultVals(lRow, 0) = CSng(lDateString) Then
                'found the date in the output file
                lPlotDay = lRow
            End If
        Next

        If lPlotDay = 0 Then
            Logger.Msg("Selected day not found in " & lResultFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lTheHighflow As Single = 0.0
        Dim lTheLowflow As Single = 0.0
        Dim lTheMedflow As Single = 0.0
        Dim lTheFlow As Single = 0.0
        GisUtil.StartSetFeatureValue(lSubThemeIndex)
        For lFieldNum As Integer = 1 To lNumFlds - 1
            Dim lSubbasinId As Integer = lResultVals(1, lFieldNum)
            For lPolyRecs As Integer = 0 To GisUtil.NumFeatures(lSubThemeIndex) - 1
                Dim lCurPolyid As Integer = GisUtil.FieldValue(lSubThemeIndex, lPolyRecs, lPidfield)
                If (lCurPolyid = lSubbasinId) Then
                    lTheHighflow = GisUtil.FieldValue(lSubThemeIndex, lPolyRecs, lHighFlowFld)
                    lTheLowflow = GisUtil.FieldValue(lSubThemeIndex, lPolyRecs, lLowFlowFld)
                    lTheMedflow = GisUtil.FieldValue(lSubThemeIndex, lPolyRecs, lMedFlowFld)
                    lTheFlow = lResultVals(lPlotDay, lFieldNum)
                    GisUtil.SetFeatureValueNoStartStop(lSubThemeIndex, lCurFlowFld, lPolyRecs, SignificantDigits(lTheFlow, 4))
                    If (lTheFlow > lTheHighflow) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubThemeIndex, lCurIndexFld, lPolyRecs, 3)
                    ElseIf (lTheFlow < lTheLowflow) Then
                        GisUtil.SetFeatureValueNoStartStop(lSubThemeIndex, lCurIndexFld, lPolyRecs, 1)
                    Else
                        GisUtil.SetFeatureValueNoStartStop(lSubThemeIndex, lCurIndexFld, lPolyRecs, 2)
                    End If
                End If
            Next
        Next
        GisUtil.StopSetFeatureValue(lSubThemeIndex)

        'generate thematic map
        Dim lColors As New Collection
        lColors.Add(System.Convert.ToUInt32(RGB(250, 250, 0))) 'yellow
        lColors.Add(System.Convert.ToUInt32(RGB(0, 240, 0)))   'green
        lColors.Add(System.Convert.ToUInt32(RGB(0, 0, 250)))   'blue
        Dim lCaptions As New Collection
        lCaptions.Add("low_flow")
        lCaptions.Add("normal_flow")
        lCaptions.Add("high_flow")
        GisUtil.SetLayerRendererUniqueValues("Subbasins", lCurIndexFld, lColors, lCaptions)

        Return True
    End Function

    Friend Sub SetFlowTimeseries(ByVal aFlowFileName As String)
        'read result file into memory
        Dim lNnumRecs As Integer = 0
        Dim lResultRecs As New Collection
        If FileExists(aFlowFileName) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(aFlowFileName)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lNnumRecs = lNnumRecs + 1
                        lResultRecs.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & aFlowFileName & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Exit Sub
            End Try
        Else
            Logger.Msg("Cannot open output file, " & aFlowFileName & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If lResultRecs.Count = 0 Then
            Exit Sub
        End If

        Dim lNumFlds As Integer = 0
        Dim lstr As String = lResultRecs(1)
        Dim lstr1 As String = ""
        Do While lstr.Length > 0
            lstr1 = StrRetRem(lstr)
            lNumFlds += 1
        Loop

        'read results into local array
        Dim lResultVals(lNnumRecs, lNumFlds) As Single
        Dim lIrec As Integer = 0
        For Each lRec As String In lResultRecs
            lIrec += 1
            Dim lIfield As Integer = -1
            Do While lRec.Length > 0
                lstr1 = StrRetRem(lRec)
                lIfield += 1
                If IsNumeric(lstr1) Then
                    lResultVals(lIrec, lIfield) = lstr1
                End If
            Loop
        Next

        'set the start/end dates
        Dim lStartYear As String = ""
        Dim lStartDay As String = ""
        lStartYear = Left(lResultVals(2, 0), 4)
        If Not IsNumeric(lStartYear) Then
            Logger.Msg("Problem reading streamflow file:  Start year must be a 4 digit number", "GeoSFM Utilities")
            Exit Sub
        End If
        lStartDay = Right(lResultVals(2, 0), 3)
        If Not IsNumeric(lStartDay) Then
            Logger.Msg("Problem reading streamflow file:  Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
            Exit Sub
        End If
        Dim lEndYear As String = ""
        Dim lEndDay As String = ""
        lEndYear = Left(lResultVals(lNnumRecs, 0), 4)
        If Not IsNumeric(lEndYear) Then
            Logger.Msg("End year must be a 4 digit number", "GeoSFM Utilities")
            Exit Sub
        End If
        lEndDay = Right(lResultVals(lNnumRecs, 0), 3)
        If Not IsNumeric(lEndDay) Then
            Logger.Msg("End day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
            Exit Sub
        End If
        Dim lSDate(5) As Integer
        lSDate(0) = CInt(lStartYear)
        lSDate(1) = 1
        lSDate(2) = 1
        Dim lSJDate As Double = Date2J(lSDate) + lStartDay - 1
        Dim lEDate(5) As Integer
        lEDate(0) = CInt(lEndYear)
        lEDate(1) = 1
        lEDate(2) = 1
        Dim lEJDate As Double = Date2J(lEDate) + lEndDay - 1
        Dim lNvals As Double = lEJDate - lSJDate
        Dim lDates(lNvals) As Double
        For lDateIndex As Integer = 0 To lNvals
            lDates(lDateIndex) = lSJDate + lDateIndex
        Next

        'does this type of data source already exist?
        For lDSIndex As Integer = 0 To atcDataManager.DataSources.Count - 1
            Dim lDS As atcDataSource = atcDataManager.DataSources(lDSIndex)
            If lDS.Name = "" And lDS.Description = "" Then
                atcDataManager.RemoveDataSource(lDSIndex)
            End If
        Next

        'create new data source to receive the data
        Dim lDataSource As New atcTimeseriesSource

        For lTsIndex As Integer = 1 To lNumFlds - 1
            'now convert the local array into atcTimeseries
            Dim lDsn As Integer = lTsIndex
            Dim lRchId As String = CInt(lResultVals(1, lTsIndex)).ToString
            Dim lGenericTs As New atcData.atcTimeseries(Nothing)
            With lGenericTs.Attributes
                .SetValue("ID", lDsn)
                .SetValue("Scenario", "Simulated")
                .SetValue("Constituent", "Flow")
                .SetValue("Location", "Reach " & lRchId)
                .SetValue("Description", "Simulated flow from GeoSFM")
                .SetValue("STANAM", "GeoSFM Reach " & lRchId)
                .SetValue("TU", 4)  'assume daily
                .SetValue("TS", 1)
                .SetValue("TSTYPE", "FLOW")
                .SetValue("Data Source", aFlowFileName)
            End With

            Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
            lTsDate.Values = lDates
            lGenericTs.Dates = lTsDate

            'now fill in the values
            Dim lValues(lNvals) As Double
            Dim lCurDate As Double
            lCurDate = lSJDate
            Dim lDayCounter As Integer = 0
            Dim lValueCounter As Integer = 1
            Do While lCurDate <= lEJDate 'loop through each day
                lValues(lDayCounter) = lResultVals(lDayCounter, lTsIndex)
                lDayCounter = lDayCounter + 1
                lCurDate = lCurDate + 1
            Loop

            lGenericTs.Values = lValues
            lDataSource.DataSets.Add(lDsn, lGenericTs)
        Next

        atcDataManager.DataSources.Add(lDataSource)
    End Sub

    Friend Sub BuildListofValidStationNames(ByRef aMetConstituent As String, _
                                            ByVal aStations As atcCollection)
        aStations.Clear()

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            Dim lTotalCount As Integer = lDataSource.DataSets.Count
            Dim lCounter As Integer = 0

            For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                lCounter += 1
                Logger.Progress("Building list of valid " & aMetConstituent & " station names...", lCounter, lDataSource.DataSets.Count)

                If lDataSet.Attributes.GetValue("Constituent") = aMetConstituent Then
                    Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                    Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                    Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                    Dim lSJDay As Double
                    Dim lEJDay As Double
                    lSJDay = lDataSet.Attributes.GetValue("Start Date", 0)
                    lEJDay = lDataSet.Attributes.GetValue("End Date", 0)
                    If lSJDay = 0 Then
                        lSJDay = lDataSet.Dates.Value(0)
                    End If
                    If lEJDay = 0 Then
                        lEJDay = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                    End If
                    Dim lAddIt As Boolean = True

                    'if this one is computed and observed also exists at same location, just use observed
                    If lDataSet.Attributes.GetValue("Scenario") = "COMPUTED" Then
                        For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                            If lDataSet2.Attributes.GetValue("Constituent") = aMetConstituent And _
                               lDataSet2.Attributes.GetValue("Scenario") = "OBSERVED" And _
                               lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                lAddIt = False
                                Exit For
                            End If
                        Next
                    End If

                    If lAddIt Then
                        Dim lSdate(6) As Integer
                        Dim lEdate(6) As Integer
                        J2Date(lSJDay, lSdate)
                        J2Date(lEJDay, lEdate)
                        Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                        Dim lStationDetails As New StationDetails
                        lStationDetails.Name = lLoc
                        lStationDetails.StartJDate = lSJDay
                        lStationDetails.EndJDate = lEJDay
                        lStationDetails.Description = lLoc & ":" & lStanam & " " & lDateString
                        aStations.Add(lStationDetails.Description, lStationDetails)
                        'Logger.Dbg("Added " & lStationDetails.Description)
                    End If
                End If
                'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                lDataSet.ValuesNeedToBeRead = True
            Next
        Next

        Logger.Dbg("Found " & aStations.Count & " Stations")
    End Sub

    Friend Sub SetOutputPath(ByVal aProjectName As String)
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\"
        If FileExists(lOutputPath) Then
            lOutputPath = lOutputPath & aProjectName & "\"
        Else
            Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
            lOutputPath = lBasinsFolder & "\modelout\" & aProjectName & "\"
        End If
        If Not FileExists(lOutputPath, True) Then
            MkDir(lOutputPath)
        End If
        pOutputPath = lOutputPath
    End Sub
    
        
    Private Function AddField(ByVal aTheme As Integer, ByVal aFieldName As String, Optional ByVal aFieldType As Integer = 2) As Integer
        Dim lfld As Integer = -1
        If GisUtil.IsField(aTheme, aFieldName) Then
            lfld = GisUtil.FieldIndex(aTheme, aFieldName)
        Else
            GisUtil.AddField(aTheme, aFieldName, aFieldType, 10)
            lfld = GisUtil.FieldIndex(aTheme, aFieldName)
        End If
        Return lfld
    End Function


    Friend Class StationDetails
        Public Name As String
        Public StartJDate As Double
        Public EndJDate As Double
        Public Description As String
    End Class

End Module
