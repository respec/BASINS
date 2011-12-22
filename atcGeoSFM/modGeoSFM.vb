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

            Dim lIDField As Integer = 1
            If GisUtil.IsField(lSubbasinLayerIndex, "Gridcode") Then
                lIDField = GisUtil.FieldIndex(lSubbasinLayerIndex, "Gridcode")
            Else
                If GisUtil.IsField(lSubbasinLayerIndex, "PolygonID") Then
                    lIDField = GisUtil.FieldIndex(lSubbasinLayerIndex, "PolygonID")
                End If
            End If

            GisUtil.GridFromShapefile(lSubbasinLayerIndex, lIDField, lPitFillDEMFileName, lSubbasinGridFileName)
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

            Dim lIDField As Integer = 1
            If GisUtil.IsField(lStreamLayerIndex, "Gridcode") Then
                lIDField = GisUtil.FieldIndex(lStreamLayerIndex, "Gridcode")
            Else
                If GisUtil.IsField(lStreamLayerIndex, "PolygonID") Then
                    lIDField = GisUtil.FieldIndex(lStreamLayerIndex, "PolygonID")
                End If
            End If

            GisUtil.GridFromShapefile(lStreamLayerIndex, lIDField, lPitFillDEMFileName, lStreamGridFileName)
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

        'dam locations?
        Dim lHasDamField As Integer = -1
        Dim lSubbasinLayerIndex As Integer = -1
        Dim lIDField As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            lSubbasinLayerIndex = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(lSubbasinLayerIndex, "HasDam") Then
                lHasDamField = GisUtil.FieldIndex(lSubbasinLayerIndex, "HasDam")
            End If
            If GisUtil.IsField(lSubbasinLayerIndex, "Gridcode") Then
                lIDField = GisUtil.FieldIndex(lSubbasinLayerIndex, "Gridcode")
            Else
                If GisUtil.IsField(lSubbasinLayerIndex, "PolygonID") Then
                    lIDField = GisUtil.FieldIndex(lSubbasinLayerIndex, "PolygonID")
                End If
            End If
        End If

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
            Dim lId As Integer = -1
            If lSubbasinLayerIndex > -1 Then
                If lHasDamField > -1 Then
                    For lIndex As Integer = 0 To GisUtil.NumFeatures(lSubbasinLayerIndex) - 1
                        lId = GisUtil.FieldValue(lSubbasinLayerIndex, lIndex, lIDField)
                        If lId = lBasinValue Then
                            If GisUtil.FieldValue(lSubbasinLayerIndex, lIndex, lHasDamField) = 1 Then
                                lHasDamValue = "1"
                            End If
                            Exit For
                        End If
                    Next
                End If
            End If

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
        System.GC.Collect()
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

    Friend Function Balance(ByVal aIniFract As Single, ByVal aDformat As Integer, ByVal aIniMode As Integer, ByVal aRunMode As Integer, _
                            ByVal aBalanceType As Integer, ByVal aSjDate As Double) As Boolean
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

        Dim lRainFileName As String = pOutputPath & "rain.txt"
        Dim lEvapFileName As String = pOutputPath & "evap.txt"
        Dim lBasinFileName As String = pOutputPath & "basin.txt"
        Dim lRespFileName As String = pOutputPath & "response.txt"
        Dim lParamFileName As String = pOutputPath & "balparam.txt"
        Dim lSurpFileName As String = pOutputPath & "basinrunoffyield.txt"
        Dim lStoreFileName As String = pOutputPath & "soilwater.txt"
        Dim lAEvapFileName As String = pOutputPath & "actualevap.txt"
        Dim lGWLossFileName As String = pOutputPath & "gwloss.txt"
        Dim lOutswFileName As String = pOutputPath & "cswater.txt"
        Dim lExcessFileName As String = pOutputPath & "excessflow.txt"
        Dim lInterFileName As String = pOutputPath & "interflow.txt"
        Dim lBaseFileName As String = pOutputPath & "baseflow.txt"
        Dim lMassFileName As String = pOutputPath & "massbalance.txt"
        Dim lLogFileName As String = pOutputPath & "logfilesoil.txt"
        Dim lInitialFileName As String = pOutputPath & "initial.txt"
        Dim lBalFileName As String = pOutputPath & "balfiles.txt"
        Dim lMaxTimeFileName As String = pOutputPath & "maxtime.txt"
        Dim lTestFileName As String = pOutputPath & "testfile.txt"
        Dim lWhichModelFN As String = pOutputPath & "whichmodel.txt"

        If Not FileExists(lRainFileName) Then
            Logger.Msg("Could not open rain file," & vbCrLf & lRainFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(lEvapFileName) Then
            Logger.Msg("Could not open evap file," & vbCrLf & lEvapFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(lBasinFileName) Then
            Logger.Msg("Could not open basin file," & vbCrLf & lBasinFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(lRespFileName) Then
            Logger.Msg("Could not open response file," & vbCrLf & lRespFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lRStreamReader As New StreamReader(lRainFileName)
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

        Dim lEStreamReader As New StreamReader(lEvapFileName)
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

        Dim lBStreamReader As New StreamReader(lBasinFileName)
        Dim lBasSize As Integer = 0
        Do
            lCurrentRecord = lBStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            Else
                lBasSize += 1
            End If
        Loop
        lBasSize = lBasSize - 1

        Dim lDate(6) As Integer
        J2Date(aSjDate, lDate)
        Dim lStartYr As Integer = lDate(0)
        Dim lJYr As Double = Date2J(lStartYr, 1, 1)
        Dim lStartDy As Integer = aSjDate - lJYr + 1

        Dim lRespStreamReader As New StreamReader(lRespFileName)
        lCurrentRecord = lRespStreamReader.ReadLine
        Dim lResDays As Integer = 0
        Dim lstr As String = ""
        Do Until lCurrentRecord.Length = 0
            lResDays += 1
            lstr = StrRetRem(lCurrentRecord)
        Loop
        lResDays = lResDays - 1

        Dim lChkFile As New StringBuilder
        lChkFile.AppendLine(lRainFileName)
        lChkFile.AppendLine(lEvapFileName)
        lChkFile.AppendLine(lBasinFileName)
        lChkFile.AppendLine(lRespFileName)
        lChkFile.AppendLine(lParamFileName)
        lChkFile.AppendLine(lSurpFileName)
        lChkFile.AppendLine(lStoreFileName)
        lChkFile.AppendLine(lAEvapFileName)
        lChkFile.AppendLine(lGWLossFileName)
        lChkFile.AppendLine(lOutswFileName)
        lChkFile.AppendLine(lExcessFileName)
        lChkFile.AppendLine(lInterFileName)
        lChkFile.AppendLine(lBaseFileName)
        lChkFile.AppendLine(lMassFileName)
        lChkFile.AppendLine(lLogFileName)
        lChkFile.AppendLine(lInitialFileName)
        lChkFile.AppendLine(pOutputPath)
        SaveFileString(lBalFileName, lChkFile.ToString)

        'plabels = { "Computation Start Year", "Computation Start Day", "Number of Rain/Evap Days", 
        '          "Number of Response Days", "Number of Subbasins", "Initial Soil Moisture", "Data Format (0=Hourly/1=Daily)", 
        '          "New Run (0) or Continue Previous Run(1)", "Basin Polygon Theme","Key Field eg Grid Code","Simulation(0) or Calibration(1) Mode"}

        'pdefaults = { startyr, startdy, runsize.asstring, resdays.asstring, bassize.asstring, "0.1", "1", "0", "basply.shp", "gridcode", "0"}
        'runpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", plabels, pdefaults)

        Dim lTinterval As String = ""
        If (aDformat = 1) Then
            lTinterval = "24"
        ElseIf (aDformat = 0) Then
            lTinterval = "1"
        Else
            lTinterval = "24"
        End If

        Dim lBasinLayerIndex As Integer = -1
        Dim lIDFieldIndex As Integer = -1
        Dim lNowFieldIndex As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            lBasinLayerIndex = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(lBasinLayerIndex, "gridcode") Then
                lIDFieldIndex = GisUtil.FieldIndex(lBasinLayerIndex, "gridcode")
            Else
                lIDFieldIndex = 0
            End If
            If GisUtil.IsField(lBasinLayerIndex, "soilwater") Then
                lNowFieldIndex = GisUtil.FieldIndex(lBasinLayerIndex, "soilwater")
            Else
                GisUtil.AddField(lBasinLayerIndex, "soilwater", 2, 10)
                lNowFieldIndex = GisUtil.FieldIndex(lBasinLayerIndex, "soilwater")
            End If
        End If

        Dim lParamFile As New StringBuilder
        lParamFile.AppendLine(lResDays.ToString)
        lParamFile.AppendLine(lRunSize.ToString)
        lParamFile.AppendLine(lStartYr.ToString)
        lParamFile.AppendLine(Format(lStartDy, "000"))
        lParamFile.AppendLine(lBasSize.ToString)
        lParamFile.AppendLine(lTinterval.ToString)
        lParamFile.AppendLine(aDformat.ToString)
        lParamFile.AppendLine(aIniMode.ToString)
        lParamFile.AppendLine(aIniFract.ToString)
        lParamFile.AppendLine(aRunMode.ToString)
        SaveFileString(lParamFileName, lParamFile.ToString)

        ''timefile.writeelt(runsize.asstring)  'commented out in the avenue 
        ''timefile.close

        Dim lRouteType As Integer = 2

        Dim lWhichFile As New StringBuilder
        lWhichFile.AppendLine("Model Index   Index description")
        lWhichFile.AppendLine(aBalanceType & " //water balance model:  1=1D balance, 2=2D balance")
        lWhichFile.AppendLine(lRouteType & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        SaveFileString(lWhichModelFN, lWhichFile.ToString)

        'If FileExists(surpfilename) Then
        '    IO.File.Delete(surpfilename)
        'End If

        If aBalanceType = 1 Then    '"Linear Soil Model"
            ONELAYERBALANCE(lBalFileName)
        Else                       '"Non-Linear Soil Model"
            TWOLAYERBALANCE(lBalFileName)
        End If

        If Not FileExists(lSurpFileName) Then
            Logger.Msg("An error occurred during the balance computation." & vbCrLf & "Check for possible causes in  " + lLogFileName, "Geospatial Stream Flow Model")
            Return False
        End If

        If (aRunMode = 0) Then
            If Not FileExists(lOutswFileName) Then
                Logger.Msg("Could not open current soil water file," & vbCrLf & lOutswFileName, "Geospatial Stream Flow Model")
                Return False
            End If

            Dim lCurrent As New atcCollection
            Dim lOutStreamReader As New StreamReader(lOutswFileName)
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
            GisUtil.StartSetFeatureValue(lBasinLayerIndex)
            Dim lId As String = ""
            For lIndex As Integer = 0 To GisUtil.NumFeatures(lBasinLayerIndex) - 1
                lId = GisUtil.FieldValue(lBasinLayerIndex, lIndex, lIDFieldIndex)
                If Not lCurrent.ItemByKey(lId) Is Nothing Then
                    GisUtil.SetFeatureValue(lBasinLayerIndex, lNowFieldIndex, lIndex, lCurrent.ItemByKey(lId))
                End If
            Next
            GisUtil.StopSetFeatureValue(lBasinLayerIndex)

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
            GisUtil.SetLayerRendererWithRanges(lBasinLayerIndex, lNowFieldIndex, lColors, lCaptions, lLowRange, lHighRange)
        End If

        Logger.Msg("Soil Water Balance Complete. Results written to:" & vbCrLf & lSurpFileName, "Geospatial Stream Flow Model")
        Return True
    End Function

    Friend Function Route(ByVal aForecastDays As Integer, ByVal aRunMode As Integer, ByVal aDFormat As Integer, _
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

        Dim lRunoffFN As String = pOutputPath & "basinrunoffyield.txt"
        Dim lResponseFN As String = pOutputPath & "response.txt"
        Dim lRiverFN As String = pOutputPath & "river.txt"
        Dim lReservoirFN As String = pOutputPath & "reservoir.txt"
        Dim lRiverdepthFN As String = pOutputPath & "riverdepth.txt"
        Dim lInflowFN As String = pOutputPath & "inflow.txt"
        Dim lFlowFN As String = pOutputPath & "streamflow.txt"
        Dim lDamsFN As String = pOutputPath & "damstatus.txt"
        Dim lLocalflowFN As String = pOutputPath & "localflow.txt"
        Dim lRoutparamFN As String = pOutputPath & "routparam.txt"
        Dim lRoutfilesFN As String = pOutputPath & "routfiles.txt"
        Dim lForecastFN As String = pOutputPath & "forecast.txt"
        Dim lLogFN As String = pOutputPath & "logfileflow.txt"

        If (aForecastDays > 99) Then
            aForecastDays = 99
        ElseIf (aForecastDays < 0) Then
            aForecastDays = 0
        End If

        Dim lInitialFN As String = pOutputPath & "initial.txt"
        Dim lTimesFN As String = pOutputPath & "times.txt"
        'Dim maxtimeFN As String = pOutputPath & "maxtime.txt"
        Dim lDamlinkFN As String = pOutputPath & "damlink.txt"
        Dim lRatingFN As String = pOutputPath & "rating.txt"
        Dim lWhichModelFN As String = pOutputPath & "whichmodel.txt"

        If Not FileExists(lRunoffFN) Then
            Logger.Msg("Could not open runoff file." & vbCrLf & lRunoffFN, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(lResponseFN) Then
            Logger.Msg("Could not open response file," & vbCrLf & lResponseFN, "Geospatial Stream Flow Model")
            Return False
        End If

        If Not FileExists(lRiverFN) Then
            Logger.Msg("Could not open river file," & vbCrLf & lRiverFN, "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lDay As String = System.DateTime.Now.Date.ToString
        Dim lYear As String = System.DateTime.Now.Year.ToString

        Dim lChkFile As New StringBuilder
        lChkFile.AppendLine("Starting Time:" & " " & System.DateTime.Now.ToString)

        'IDENTIFY THE CALCULATION THEME, THE CELL FIELD AND THE WATER HOLDING CAPACITY FIELD

        Dim lBasinLayerIndex As Integer = -1
        Dim lIDFieldIndex As Integer = -1
        If GisUtil.IsLayer("Subbasins") Then
            lBasinLayerIndex = GisUtil.LayerIndex("Subbasins")
            If GisUtil.IsField(lBasinLayerIndex, "gridcode") Then
                lIDFieldIndex = GisUtil.FieldIndex(lBasinLayerIndex, "gridcode")
            Else
                lIDFieldIndex = 0
            End If
        End If

        Logger.Dbg("Reading Input Files.......")

        Dim lRivList As New atcCollection
        Dim lRivStart As Integer = 1
        Dim lRiv As String = ""
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lRiverFN)
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
            Logger.Msg("Problem reading the river file " & lRiverFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lResponseList As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lResponseFN)
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
            Logger.Msg("Problem reading the response file " & lResponseFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        If (lResponseList.Count < lRivList.Count) Then
            Logger.Msg("The response file " + lResponseFN + " does not contain enough subbasins.", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lResDays As Integer = lResponseList.Count

        ' READ IN THE INPUT ARRAYS FOR PRECIPITATION AND POTENTIAL EVAPORATION
        Dim lRunoffList As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lRunoffFN)
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
            Logger.Msg("Problem reading the runoff file " & lRunoffFN & ".", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End Try

        Dim lStr As String = lRunoffList(0)
        Dim lDayOneStr As String = StrRetRem(lStr)
        Dim lTimeType As String = "Daily"
        Dim lTimeinHrs As String = "24"

        If aDFormat = 0 Then
            lTimeType = "Hourly"
            lTimeinHrs = "1"
        End If

        Dim lDate(6) As Integer
        J2Date(aSjDate, lDate)
        Dim lStartYr As Integer = lDate(0)
        Dim lJYr As Double = Date2J(lStartYr, 1, 1)
        Dim lStartDy As Integer = aSjDate - lJYr + 1

        Dim lRunoffDays As Integer = lRunoffList.Count

        Dim lOutSteps As Integer = lRunoffDays + lResDays + aForecastDays

        Dim lRunDays As Integer = (lRunoffDays + lResDays + aForecastDays + lResDays + 1)

        Dim lForeFile As New StringBuilder
        lForeFile.AppendLine(lRunoffList.Item(0))

        Logger.Dbg("Updating Reservoir Discharges.......")

        ' Performing a convolution to update PFLOW
        ' ie the local contribution to streamflow

        Dim lTCount As Integer = 0
        Dim lIsFirst As Integer = 0
        Dim lDamCount As Integer = 0
        Dim lReservoirList As New atcCollection
        For lBRecIndex As Integer = (lRivList.Count - 1) To lRivStart Step -1

            Dim lCharList As String = lRivList.ItemByIndex(lBRecIndex)
            Dim lRespList As String = lResponseList.ItemByIndex(lBRecIndex)
            Dim lRiverId As String = StrRetRem(lCharList)
            Dim lTmpStr As String = ""
            For lIndex As Integer = 1 To 8
                lTmpStr = StrRetRem(lCharList)
            Next
            Dim lHasDam As Integer = StrRetRem(lCharList)

            If (lHasDam <> 0) Then
                lDamCount = lDamCount + 1
                If (lIsFirst = 0) Then

                    Dim lResvid As String
                    Try
                        Dim lCurrentRecord As String
                        Dim lStreamReader As New StreamReader(lReservoirFN)
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
                        Logger.Msg("Could not open reservoir characteristics file," & vbCrLf & lReservoirFN, MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                        Exit Function
                    End Try

                    lIsFirst = lIsFirst + 1
                End If

                Dim lIsOperated As Integer = 0
                Dim lResDTime As Single = 0.0
                Dim lDamoutFile As New StringBuilder
                Dim lDamoutFileName As String = pOutputPath & "dam" & lRiverId.ToString & ".txt"

                Dim lResvList As String = ""
                If lReservoirList.ItemByKey(lRiverId) Is Nothing Then
                    Logger.Msg("No reservoir characteristic found for basin " & lRiverId & vbCrLf & "Update " & lReservoirFN & " before computing streamflow.", "Geospatial Stream Flow Model")
                    Return False
                Else
                    lResvList = lReservoirList.Item(lRiverId)

                    If (lResvList.Length < 4) Then
                        Logger.Msg("The reservoir characteristic file must contain 3 or more fields:" & vbCrLf & "riverid, storage, residencetime, isoperated", "Geospatial Stream Flow Model")
                    End If
                End If
                Dim lDamStore As Single = StrRetRem(lResvList)
                lResDTime = StrRetRem(lResvList)
                lIsOperated = StrRetRem(lResvList)
                If (lIsOperated <> 0) Then
                    Dim lOperateFN As String = StrRetRem(lResvList)
                    Dim lWithD As Integer = 0
                    If Not FileExists(lOperateFN) Then
                        If Not FileExists(pOutputPath & lOperateFN) Then
                            Logger.Msg("Operations file " & lOperateFN & " for reservoir in basin " & lRiverId & "Not found." + vbCrLf + "Create the file before computing streamflow", "Geospatial Stream Flow Model")
                            Return False
                        End If
                        lWithD = 1
                    End If

                    Dim lOpList As New atcCollection
                    Dim lOpSize As Integer = 0
                    Try
                        Dim lCurrentRecord As String
                        Dim lStreamReader As New StreamReader(lOperateFN)
                        lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                        Do
                            lCurrentRecord = lStreamReader.ReadLine
                            If lCurrentRecord Is Nothing Then
                                Exit Do
                            Else
                                lOpSize = lOpSize + 1
                                lOpList.Add(lCurrentRecord)
                            End If
                        Loop
                    Catch e As ApplicationException
                        Logger.Msg("Problem reading reservoir operations file," & vbCrLf & lOperateFN, MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                        Return False
                    End Try

                    Dim lTestfLst As String = ""
                    Dim lLastFlow As String = ""
                    For lOflow As Integer = lOpSize - 1 To 0 Step -1
                        lTestfLst = lOpList(lOflow)
                        If (lTestfLst.Length >= 2) Then
                            lLastFlow = StrRetRem(lTestfLst)
                            lLastFlow = StrRetRem(lTestfLst)
                            lOpSize = lOflow + 1
                            Exit For
                        End If
                    Next

                    If (lOpSize < lOutSteps) Then
                        For lOrec As Integer = 1 To (lOutSteps - lOpSize)
                            Dim lOpday As String = ""
                            If (lOrec <= lRunoffDays) Then
                                lOpday = lRunoffList(lOrec)
                            Else
                                lOpday = (lOrec - lRunoffDays).ToString
                            End If
                            lOpList.Add(lOpday & ", " & "1" & ", " & lLastFlow)
                        Next
                    End If

                    lDamoutFile.AppendLine("Time,Stage,Discharge")
                    For trec As Integer = 0 To (lOutSteps - 1)
                        lDamoutFile.AppendLine(lOpList(trec).ToString)
                    Next
                    SaveFileString(lDamoutFileName, lDamoutFile.ToString)

                End If

                Dim lDamLinkFile As New StringBuilder
                lDamLinkFile.AppendLine(lRiverId.ToString)
                lDamLinkFile.AppendLine(pOutputPath & "dam" & lRiverId.ToString & ".txt")
                lDamLinkFile.AppendLine(lIsOperated.ToString)
                lDamLinkFile.AppendLine(lResDTime.ToString)
                SaveFileString(lDamlinkFN, lDamLinkFile.ToString)
            End If

        Next

        Dim lBalstr As String = ""
        Dim lRoutstr As String = ""
        Dim lBalanceType As String = ""
        Dim lRouteType As String = ""
        If (FileExists(lWhichModelFN)) Then
            Dim lWhichSize As Integer = 0
            Dim lBigList As New atcCollection
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lWhichModelFN)
                lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lWhichSize = lWhichSize + 1
                        lBigList.Add(lCurrentRecord)
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Could not open whichModel.txt file" & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try

            If (lWhichSize > 2) Then
                lBalanceType = lBigList(1)
                lRouteType = lBigList(2)
            Else
                lBalanceType = "1"
                lRouteType = "2"
            End If

            'Model Index   Index description
            '1 //water balance model:  1=1D balance, 2=2D balance
            '2 //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag
            If ((lBalanceType = "1") And (lRouteType = "3")) Then
                lBalstr = "Linear Soil Model"
                lRoutstr = "Simple Lag Routing Method"
            ElseIf ((lBalanceType = "1") And (lRouteType = "1")) Then
                lBalstr = "Linear Soil Model"
                lRoutstr = "Diffusion Analog Routing Method"
            ElseIf ((lBalanceType = "1") And (lRouteType = "2")) Then
                lBalstr = "Linear Soil Model"
                lRoutstr = "Muskingum-Cunge Routing"
            ElseIf ((lBalanceType = "2") And (lRouteType = "3")) Then
                lBalstr = "Non-Linear Soil Model"
                lRoutstr = "Simple Lag Routing Method"
            ElseIf ((lBalanceType = "2") And (lRouteType = "1")) Then
                lBalstr = "Non-Linear Soil Model"
                lRoutstr = "Diffusion Analog Routing Method"
            ElseIf ((lBalanceType = "2") And (lRouteType = "2")) Then
                lBalstr = "Non-Linear Soil Model"
                lRoutstr = "Muskingum Cunge Routing Method"
            Else
                lBalstr = "Linear Soil Model"
                lRoutstr = "Muskingum-Cunge Routing"
            End If
            ' routelst = {routstr, "Simple Lag Routing Method", "Diffusion Analog Routing Method", "Muskingum Cunge Routing Method" }
        Else
            '  routelst = {"Muskingum Cunge Routing Method", "Diffusion Analog Routing Method","Simple Lag Routing Method" }
            lBalstr = "Linear Soil Model"
        End If

        '        routestr = MsgBox.choiceasstring(routelst, "Select Flow Routing Method for Computation", "Geospatial Stream Flow Model")

        If (lBalstr = "Linear Soil Model") Then
            lBalanceType = "1"
        ElseIf (lBalstr = "Non-Linear Soil Model") Then
            lBalanceType = "2"
        Else
            lBalanceType = "1"
        End If

        lRouteType = aRouteMethod.ToString

        Dim lWhichFile As New StringBuilder
        lWhichFile.AppendLine("Model Index   Index description")
        lWhichFile.AppendLine(lBalanceType & " //water balance model:  1=1D balance, 2=2D balance")
        lWhichFile.AppendLine(lRouteType & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        SaveFileString(lWhichModelFN, lWhichFile.ToString)

        Dim lRunoff As Integer = 0
        If (lRunoffDays > 100) Then
            lRunoff = (lRunoffDays - 100)
        Else
            lRunoff = lRunoffDays
        End If

        Dim lOutFormat As Integer = 0
        If (lTimeType = "Hourly") Then
            lOutFormat = 1
        Else
            lOutFormat = 0
        End If

        Dim lParFile As New StringBuilder
        lParFile.AppendLine(lRunoff.ToString)
        lParFile.AppendLine(lStartYr.ToString)
        lParFile.AppendLine(Format(lStartDy, "000"))
        lParFile.AppendLine(lRivList.Count.ToString)
        lParFile.AppendLine(lTimeinHrs.ToString)
        lParFile.AppendLine("0")
        lParFile.AppendLine(aForecastDays.ToString)
        lParFile.AppendLine(lOutFormat.ToString)
        lParFile.AppendLine(lDamCount.ToString)
        lParFile.AppendLine(aRunMode.ToString)
        SaveFileString(lRoutparamFN, lParFile.ToString)

        Dim lRoutFile As New StringBuilder
        lRoutFile.AppendLine(lRoutparamFN)
        lRoutFile.AppendLine(lRiverFN)
        lRoutFile.AppendLine(lInitialFN)
        lRoutFile.AppendLine(lRunoffFN)
        lRoutFile.AppendLine(lDamlinkFN)
        lRoutFile.AppendLine(lForecastFN)
        lRoutFile.AppendLine(lRatingFN)
        lRoutFile.AppendLine(lFlowFN)
        lRoutFile.AppendLine(lLocalflowFN)
        lRoutFile.AppendLine(lRiverdepthFN)
        lRoutFile.AppendLine(lInflowFN)
        lRoutFile.AppendLine(lLogFN)
        lRoutFile.AppendLine(pOutputPath)
        SaveFileString(lRoutfilesFN, lRoutFile.ToString)

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)

        Dim lGeosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        If (aRouteMethod = 3) Then
            LAGROUTE(lRoutfilesFN)
        ElseIf (aRouteMethod = 1) Then
            DIFFROUTE(lRoutfilesFN)
        Else
            cungeroute(lRoutfilesFN)
        End If

        'timefile.writeelt(myrunoff.asstring)  'commented out in the avenue 
        'timefile.close

        If Not FileExists(lFlowFN) Then
            Logger.Msg("An error occurred during the routing computations." & vbCrLf & "Check for possible causes in logfileflow.txt", "Geospatial Stream Flow Model")
            Return False
        End If

        lChkFile.AppendLine("Ending Time: " & System.DateTime.Now.ToString)
        SaveFileString(lTimesFN, lChkFile.ToString)

        SetFlowTimeseries(lFlowFN)

        Logger.Msg("Stream Flow Routing Complete." & vbCrLf & "Results written as BASINS internal timeseries and to file: " & vbCrLf & lFlowFN, "Geospatial Stream Flow Model")
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

        Dim lNtsteps As String = lBalList(2)
        If Not IsNumeric(lNtsteps) Then
            lNtsteps = lRoutList(1)
        End If

        Dim lLogfileFN As String = pOutputPath & "logfilesarun.txt"
        Dim lLogfile As New StringBuilder

        Dim lBalanceType As Integer = 0
        Dim lRouteType As Integer = 0
        Dim lWhichModelFN As String = pOutputPath & "whichModel.txt"
        If Not FileExists(lWhichModelFN) Then
            Dim lWhichFile As New StringBuilder
            lWhichFile.AppendLine("Model Index   Index description")
            lWhichFile.AppendLine(aBalanceType & " //water balance model:  1=1D balance, 2=2D balance")
            lWhichFile.AppendLine(aRouteType & " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
            SaveFileString(lWhichModelFN, lWhichFile.ToString)
        Else
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lWhichModelFN)
                lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lBalanceType = StrRetRem(lCurrentRecord)
                        lCurrentRecord = lStreamReader.ReadLine
                        lRouteType = StrRetRem(lCurrentRecord)
                    End If
                Loop
                lStreamReader.Close()
            Catch e As ApplicationException
                Logger.Msg("Could not open whichModel.txt file" & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        End If

        Dim lMethodTypeStr As String = ""
        If ((lBalanceType = 1) And (lRouteType = 2)) Then
            lMethodTypeStr = "One Soil Layer, Muskingum-Cunge Routing"
        ElseIf ((lBalanceType = 1) And (lRouteType = 1)) Then
            lMethodTypeStr = "One Soil Layer, Diffusion Routing"
        ElseIf ((lBalanceType = 1) And (lRouteType = 3)) Then
            lMethodTypeStr = "One Soil Layer, Lag Routing"
        ElseIf ((lBalanceType = 2) And (lRouteType = 2)) Then
            lMethodTypeStr = "Two Soil Layers, Muskingum-Cunge Routing"
        ElseIf ((lBalanceType = 2) And (lRouteType = 1)) Then
            lMethodTypeStr = "Two Soil Layers, Diffusion Routing"
        ElseIf ((lBalanceType = 2) And (lRouteType = 3)) Then
            lMethodTypeStr = "Two Soil Layers, Lag Routing"
        Else
            lMethodTypeStr = "One Soil Layer, Muskingum-Cunge Routing"
        End If

        lLogfile.AppendLine("Routing Models Selected as " & lMethodTypeStr.ToString)

        Dim lRangeFileFN As String = pOutputPath & "range.txt"
        Dim lRangeFile As New StringBuilder
        lRangeFile.AppendLine("Min, Max, Name, Description")
        For rindex As Integer = 0 To aParameterRanges.Count - 1
            lRangeFile.AppendLine(aParameterRanges.ItemByIndex(rindex) & ", " & aParameterRanges.Keys(rindex))
        Next
        SaveFileString(lRangeFileFN, lRangeFile.ToString)

        lLogfile.AppendLine("River and Basin Files Read")

        Dim lMoscemParamfileFN As String = pOutputPath & "moscem_param.txt"
        Dim lMoscemParamfile As New StringBuilder
        lMoscemParamfile.AppendLine(1.ToString)
        lMoscemParamfile.AppendLine(lNtsteps)
        lMoscemParamfile.AppendLine(lNtsteps)
        lMoscemParamfile.AppendLine(1.ToString)
        lMoscemParamfile.AppendLine(-9999.ToString)
        lMoscemParamfile.AppendLine(1.ToString)
        lMoscemParamfile.AppendLine((aSelectedReachIndex + 1).ToString)
        SaveFileString(lMoscemParamfileFN, lMoscemParamfile.ToString)

        lLogfile.AppendLine("River and Basin Parameter Ranges Confirmed")

        Dim lGeosfmsarunFN As String = ""
        If FileExists(pOutputPath & "geosfmsarun.exe") Then
            lGeosfmsarunFN = pOutputPath & "geosfmsarun.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmsarun.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmsarun.exe", pOutputPath & "geosfmsarun.exe")
            lGeosfmsarunFN = pOutputPath & "geosfmsarun.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmsarun.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("geosfmsarun.exe program copied to working directory")

        Dim lGeosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("geosfm.dll program copied to working directory")

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

        lLogfile.AppendLine("Calibration Program Successfully Executed!")
        SaveFileString(lLogfileFN, lLogfile.ToString)

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

        Dim lBalfilesFN As String = pOutputPath & "balfiles.txt"
        If Not FileExists(lBalfilesFN) Then
            Logger.Msg("Could not open balfiles.txt file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lRoutfilesFN As String = pOutputPath & "routfiles.txt"
        If Not FileExists(lRoutfilesFN) Then
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

        Dim lParameterFN As String = pOutputPath & "parameter.in"
        Dim lObjoptflagFN As String = pOutputPath & "objoptflag.in"
        Dim lObsflowFN As String = pOutputPath & "observed_streamflow.txt"
        Dim lObjectiveFN As String = pOutputPath & "objectives.out"
        Dim lParamvalFN As String = pOutputPath & "parameter_values.out"
        Dim lParamconFN As String = pOutputPath & "par_convergence.out"
        Dim lMosceminFN As String = pOutputPath & "moscem.in"
        Dim lMoscemParamFN As String = pOutputPath & "moscem_param.txt"
        Dim lWhichModelFN As String = pOutputPath & "whichModel.txt"
        Dim lOrigbasFN As String = pOutputPath & "basin_original.txt"
        Dim lOrigrivFN As String = pOutputPath & "river_original.txt"
        Dim lBasinFN As String = pOutputPath & "basin.txt"
        Dim lRiverFN As String = pOutputPath & "river.txt"
        Dim lStreamflowFN As String = pOutputPath & "streamflow.txt"
        'timeseriesFN = myWkDirname + inplist.get(14)     commented out in the avenue
        'objpostprocFN = myWkDirname + inplist.get(15)
        'trdoffboundFN = myWkDirname + inplist.get(16)
        'postprocinFN = myWkDirname + inplist.get(17)

        Dim lLogfilename As String = pOutputPath & "logfilecalib.txt"
        Dim lLogfile As New StringBuilder

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
        Dim lObsflowFile As New StringBuilder
        Dim lString As String = ""
        lString = """" & "Date" & """" & ",1"
        For lGageIndex = 2 To aFlowGageNames.Count
            lString = lString & "," & lGageIndex.ToString
        Next
        lObsflowFile.AppendLine(lString)
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
            lObsflowFile.AppendLine(lString)
        Next
        SaveFileString(lObsflowFN, lObsflowFile.ToString)

        If Not FileExists(lOrigbasFN) Then
            If (File.Exists(lBasinFN)) Then
                FileCopy(lBasinFN, lOrigbasFN)
                'origbasinfile = LineFile.Make(origbasFN.asfilename,#file_perm_read)
            Else
                Logger.Msg("Could not open basin.txt or basin_original.txt" & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End If
        End If

        If Not FileExists(lOrigrivFN) Then
            If FileExists(lRiverFN) Then
                FileCopy(lRiverFN, lOrigrivFN)
                'origriverfile = LineFile.Make(origrivFN.asfilename,#file_perm_read)
            Else
                Logger.Msg("Could not open " & lRiverFN & " or " & lOrigrivFN & vbCrLf & "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
                Return False
            End If
        End If

        'postlabels = { "Output Time Series File",
        '          "Output Objective Convergence File",
        '          "Output Tradeoff Boundary File",
        '          "Processing Parameter File"}

        '        inpostList = MsgBox.MultiInput("Enter Post Processing Parameters.", "Geospatial Stream Flow Model", postlabels, postdefaults)

        Dim lTimeseriesFN As String = pOutputPath & "timeseries.txt"
        Dim lObjpostprocFN As String = pOutputPath & "objectives_postproc.out"
        Dim lTrdoffboundFN As String = pOutputPath & "trdoff_bounds.out"
        Dim lPostprocinFN As String = pOutputPath & "postproc.in"

        'objpostprocFile = LineFile.Make(objpostprocFN.asfilename,#file_perm_write)
        'If Not FileExists(objpostprocFN) Then
        '    Logger.Msg("Could not open " + objpostprocFN + " file," & vbCrLf & "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        Dim lMinparlst(20) As String
        Dim lMaxparlst(20) As String

        Dim lOrigbsize As Integer = 0
        Dim lBigblist As New Collection
        If FileExists(lOrigbasFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lOrigbasFN)
                lCurrentRecord = lStreamReader.ReadLine  'header line
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lOrigbsize = lOrigbsize + 1
                        lBigblist.Add(lCurrentRecord)
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
        Dim lMagbposlst() As Integer = {1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14}
        Dim lMagrposlst() As Integer = {6, 3, 14, 7, 8, 12, 13}

        For lIndex As Integer = 1 To 20
            lMinparlst(lIndex) = 999999
            lMaxparlst(lIndex) = 0
        Next

        Dim lHeaderblst As New Collection
        Dim lVal As Single = 0.0
        For lObrec As Integer = 1 To lOrigbsize
            Dim lTmpStr As String = lBigblist(lObrec)
            lHeaderblst.Add(StrRetRem(lTmpStr))
            For lBbrec As Integer = 1 To UBound(lMagbposlst)
                lVal = StrRetRem(lTmpStr)
                'skip parm 5
                If lBbrec = 5 Then
                    lVal = StrRetRem(lTmpStr)
                End If
                If lVal < lMinparlst(lBbrec) Then
                    lMinparlst(lBbrec) = lVal
                End If
                If lVal > lMaxparlst(lBbrec) Then
                    lMaxparlst(lBbrec) = lVal
                End If
            Next
        Next

        Dim lOrigrsize As Integer = 0
        Dim lBigrlist As New Collection
        If FileExists(lOrigrivFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lOrigrivFN)
                lCurrentRecord = lStreamReader.ReadLine   'header
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lOrigrsize = lOrigrsize + 1
                        lBigrlist.Add(lCurrentRecord)
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

        Dim lHeaderlst As New Collection
        For lOrec As Integer = 1 To lOrigrsize
            Dim lTmpStr As String = lBigrlist(lOrec)
            lHeaderlst.Add(StrRetRem(lTmpStr))
            'Dim magrposlst() As Integer = {6, 3, 14, 7, 8, 12, 13}
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(15) Then
                lMinparlst(15) = lVal
            End If
            If lVal > lMaxparlst(15) Then
                lMaxparlst(15) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(14) Then
                lMinparlst(14) = lVal
            End If
            If lVal > lMaxparlst(14) Then
                lMaxparlst(14) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(17) Then
                lMinparlst(17) = lVal
            End If
            If lVal > lMaxparlst(17) Then
                lMaxparlst(17) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(18) Then
                lMinparlst(18) = lVal
            End If
            If lVal > lMaxparlst(18) Then
                lMaxparlst(18) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(19) Then
                lMinparlst(19) = lVal
            End If
            If lVal > lMaxparlst(19) Then
                lMaxparlst(19) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(20) Then
                lMinparlst(20) = lVal
            End If
            If lVal > lMaxparlst(20) Then
                lMaxparlst(20) = lVal
            End If
            lVal = StrRetRem(lTmpStr)
            If lVal < lMinparlst(16) Then
                lMinparlst(16) = lVal
            End If
            If lVal > lMaxparlst(16) Then
                lMaxparlst(16) = lVal
            End If
        Next

        lLogfile.AppendLine("River and Basin Files Read")

        lLogfile.AppendLine("Observed Streamflow File Read")

        Dim lNcposstr As String = "1"
        Dim lNbasidlst As New Collection
        Dim lObjoptflagFile As New StringBuilder
        Dim lNcid As Integer = 0
        For lFrec As Integer = 0 To aFlowGageNames.Count - 1
            lNcid = aFlowGageNames.Keys(lFrec)
            lNbasidlst.Add(lNcid)
            If (lNcid <> 0) Then
                lObjoptflagFile.AppendLine("runoff_" & lNcid.ToString & "      1")
            End If
        Next
        SaveFileString(lObjoptflagFN, lObjoptflagFile.ToString)

        Dim lParaminlst(20) As String
        lParaminlst(1) = "1,SoilWhc,1,0.1,5,0,Soil water holding capacity (mm)"
        lParaminlst(2) = "2,Depth,1,0.1,5,0,Total soil depth (cm)"
        lParaminlst(3) = "3,Texture,1,0.3,3,0,Soil texture 1=Sand 2=Loam 3=Clay 5=Water"
        lParaminlst(4) = "4,Ks,1,0.1,10,0,Saturated hydraulic conductivity (cm/hr)"
        lParaminlst(5) = "5,Interflow,1,0.1,5,0,Interflow storage residence time (days)"
        lParaminlst(6) = "6,HSlope,1,0.5,1.5,0,Average subbasin slope"
        lParaminlst(7) = "7,Baseflow,1,0.1,5,0,Baseflow reservoir residence time (days)"
        lParaminlst(8) = "8,CurveNum,1,0.1,1.5,0,SCS runoff curve number"
        lParaminlst(9) = "9,MaxCover,1,0.1,5,0,Fraction of the subbasin with permanently impervious cover"
        lParaminlst(10) = "10,BasinLoss,1,0.1,5,0,Fraction of soil water infiltrating to ground water"
        lParaminlst(11) = "11,PanCoeff,1,0.1,5,0,Pan coefficient for correcting PET readings"
        lParaminlst(12) = "12,TopSoil,1,0.1,5,0,Fraction of soil layer that is hydrologically active"
        lParaminlst(13) = "13,RainCalc,1,0.1,5,0,Excess rainfall mode 1=Philip 2=SCS 3=BucketModel"
        lParaminlst(14) = "14,RivRough,1,0.1,5,0,River Channel Roughness Coefficient (Manning n)"
        lParaminlst(15) = "15,RivSlope,1,0.1,5,0,Average slope of the river"
        lParaminlst(16) = "16,RivWidth,1,0.1,5,0,Average channel width (m)"
        lParaminlst(17) = "17,RivLoss,1,0.1,5,0,Fraction of flow lost within the river channel"
        lParaminlst(18) = "18,RivFPLoss,1,0.1,5,0,Fraction of the river flow lost in floodplain"
        lParaminlst(19) = "19,Celerity,1,0.1,5,0,Flood wave celerity (m/s)"
        lParaminlst(20) = "20,Diffusion,1,0.05,10,0,Flow attenuation coefficient (m^2/s)"

        Dim lData2(20) As String
        Dim lData3(20) As String
        Dim lData4(20) As String
        Dim lData5(20) As String
        Dim lData6(20) As String
        Dim lData7(20) As String
        Dim lMinistr As String = ""
        Dim lMinilst As String = ""
        For lMrec As Integer = 1 To 20
            lMinistr = lParaminlst(lMrec)
            lMinilst = StrRetRem(lMinistr)
            lMinilst = StrRetRem(lMinistr)
            lData2(lMrec) = lMinilst
            lMinilst = StrRetRem(lMinistr)
            lData3(lMrec) = lMinilst
            lMinilst = StrRetRem(lMinistr)
            lData4(lMrec) = lMinilst
            lMinilst = StrRetRem(lMinistr)
            lData5(lMrec) = lMinilst
            lMinilst = StrRetRem(lMinistr)
            lData6(lMrec) = lMinilst
            lData7(lMrec) = lMinistr
        Next

        '        choices = MsgBox.MultiListAsString(desclst, "Select Parameters to be Calibrated", "Which Parameters Do You Want to Calibrate?")

        Dim lCcount As Integer = aCalibParms.Count
        For lCrec As Integer = 1 To lCcount
            Dim lCurchoice As String = aCalibParms(lCrec)
            For lDrec As Integer = 1 To 20
                Dim lCurdesc As String = lData2(lDrec)
                If (lCurchoice = lCurdesc) Then
                    Dim lChglst As String = lParaminlst(lDrec)
                    Dim lLowval As Single = lMinparlst(lDrec)
                    Dim lLowfrac As Single = lData4(lDrec) / lLowval
                    Dim lHighval As Single = lMaxparlst(lDrec)
                    Dim lHighfrac As Single = lData5(lDrec) / lHighval
                    lParaminlst(lDrec) = lDrec.ToString & "," & lData2(lDrec) & "," & lData3(lDrec) & "," & lLowfrac & "," & lHighfrac & ",1," & lData7(lDrec)
                End If
            Next
        Next

        Dim lParaminFile As New StringBuilder
        lParaminFile.AppendLine("No,Name,Default,Lower,Upper,OptIdx,Description")
        For lDrec As Integer = 1 To 20
            lParaminFile.AppendLine(lParaminlst(lDrec))
        Next
        SaveFileString(lParameterFN, lParaminFile.ToString)

        lLogfile.AppendLine("River and Basin Calibration Parameters Identified")

        Dim lCount As Integer = aCalibParms.Count
        Dim lNcomplex As Integer = 2
        Dim lNsamples As Integer = 2 * lNcomplex * lCount
        Dim lNummult As Integer = 0
        If (lNsamples < 10) Then
            lNummult = (10 / (lNcomplex * lCount)) + 1
            lNsamples = lNummult * lNcomplex * lCount
        ElseIf (lNsamples > 200) Then
            lNummult = (100 / (lNcomplex * lCount))
            lNsamples = lNummult * lNcomplex * lCount
        End If

        '        runstr = MsgBox.ChoiceAsString(runlist, "Define a Maximum No. of Runs" + nl + "NOTE: Each run could take upto 1 minute!", "Geospatial Stream Flow Model")
        Dim lRunstr As String = aMaxRuns.ToString

        Dim lMosceminfile As New StringBuilder
        lMosceminfile.AppendLine(lCount.ToString & ",  nOptPar")
        lMosceminfile.AppendLine(aFlowGageNames.Count.ToString & ", nOptObj")
        lMosceminfile.AppendLine(lNsamples.ToString & ", nSamples")
        lMosceminfile.AppendLine(lNcomplex.ToString & ", nComplex")
        lMosceminfile.AppendLine(lRunstr + ", nMaxDraw")
        lMosceminfile.AppendLine(lParameterFN)
        lMosceminfile.AppendLine(lObjoptflagFN)
        lMosceminfile.AppendLine(lObsflowFN)
        lMosceminfile.AppendLine(lObjectiveFN)
        lMosceminfile.AppendLine(lParamvalFN)
        lMosceminfile.AppendLine(lParamconFN)
        lMosceminfile.AppendLine(lBalParamFN)
        lMosceminfile.AppendLine(lMoscemParamFN)
        lMosceminfile.AppendLine(lWhichModelFN)
        lMosceminfile.AppendLine(lOrigbasFN)
        lMosceminfile.AppendLine(lOrigrivFN)
        lMosceminfile.AppendLine(lBasinFN)
        lMosceminfile.AppendLine(lRiverFN)
        lMosceminfile.AppendLine(lStreamflowFN)
        lMosceminfile.AppendLine(lBalfilesFN)
        lMosceminfile.AppendLine(lRoutfilesFN)
        SaveFileString(lMosceminFN, lMosceminfile.ToString)

        Dim lCaliblst As New Collection
        lCaliblst.Add("Root Mean Square Error (RMSE)")
        lCaliblst.Add("Standard Deviation (STD)")
        lCaliblst.Add("Maximum Likelihood Error (MLE)")
        lCaliblst.Add("Nash-Sutcliffe Efficiency(NSE)")
        lCaliblst.Add("Number of Sign Changes (NSC)")
        lCaliblst.Add("BIAS")

        '        Calibtypestr = MsgBox.ChoiceAsString(caliblst, "Select Objective Function Type", "How Should Convergence Be Measured?")

        Dim lCalibposition As Integer = aObjFunction

        Dim lMyrstr As String = ""
        For lNrec As Integer = 1 To aFlowGageNames.Count
            Dim lKey As Integer = aFlowGageNames.Keys(lNrec - 1)
            Dim lRstrnum As Integer = 0
            'where is lkey in streamflow 
            For lIndex As Integer = 1 To lHeaderlst.Count
                If lHeaderlst(lIndex) = lKey Then
                    lRstrnum = lIndex
                End If
            Next
            Dim lRstr As String = ""
            Dim lPnum As Integer = 0
            If (lNrec = 1) Then
                lRstr = lRstrnum.ToString
                lPnum = lRstrnum
                lMyrstr = lRstr
            Else
                lRstr = (lRstrnum - lPnum).ToString
                lPnum = lRstrnum
                lMyrstr = lMyrstr + ", " + lRstr
            End If
        Next

        lLogfile.AppendLine("Calibration Method set as " & lCaliblst(lCalibposition))

        Dim lMoscemParamfile As New StringBuilder
        lMoscemParamfile.AppendLine(aFlowGageNames.Count.ToString & "  //nflux")
        lMoscemParamfile.AppendLine(lRoutList(1).ToString & "  //ntstep1")
        lMoscemParamfile.AppendLine(CInt(lRoutList(1)) + CInt(lRoutList(7)) & "  //ntstep2")
        lMoscemParamfile.AppendLine(lCalibposition.ToString & "  //obj_func")
        lMoscemParamfile.AppendLine("-9999  //missing value")
        lMoscemParamfile.AppendLine(lNcposstr & "  //nflux_obs, column in observed_streamflow.txt to test (not including timestep)")
        lMoscemParamfile.AppendLine(lMyrstr & "  //nflux_model, column in streamflow.txt (not including timestep)")
        SaveFileString(lMoscemParamFN, lMoscemParamfile.ToString)

        lLogfile.AppendLine("Starting Time:" & " " & System.DateTime.Now.ToString)

        Dim lGeosfmcalibFN As String = ""
        If FileExists(pOutputPath & "geosfmcalib.exe") Then
            lGeosfmcalibFN = pOutputPath & "geosfmcalib.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmcalib.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmcalib.exe", pOutputPath & "geosfmcalib.exe")
            lGeosfmcalibFN = pOutputPath & "geosfmcalib.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmcalib.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("geosfmcalib.exe program copied to working directory")

        Dim lGeosfmdllFN As String = ""
        If FileExists(pOutputPath & "geosfm.dll") Then
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", pOutputPath & "geosfm.dll")
            lGeosfmdllFN = pOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("geosfm.dll program copied to working directory")

        lLogfile.AppendLine("Performing Model Calibration with " & lRunstr.ToString & " ........")

        Dim lProcess As New Diagnostics.Process
        With lProcess.StartInfo
            .FileName = pOutputPath & "geosfmcalib.exe"
            .WorkingDirectory = pOutputPath
            .Arguments = lMosceminFN
            .CreateNoWindow = False
            .UseShellExecute = True
        End With
        lProcess.Start()
        While Not lProcess.HasExited
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While

        Dim lPcfilesize As Integer = 0
        If FileExists(lParamconFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lParamconFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lPcfilesize = lPcfilesize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lParamconFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lParamconFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        If (lPcfilesize > 1) Then
            lLogfile.AppendLine("Calibration Program Successfully Executed!")
        Else
            lLogfile.AppendLine("Problems encounted during calibration.")
            lLogfile.AppendLine("Output parameter convergence file, " + lParamconFN + ", is empty.")
            SaveFileString(lLogfilename, lLogfile.ToString)
            Logger.Msg("Calibration problems encountered." & vbCrLf & "Output parameter convergence file, " & lParamconFN & ", is empty", "Geospatial Stream Flow Model")
            Return False
        End If

        Dim lPostprocinFile As New StringBuilder
        lPostprocinFile.AppendLine(lNsamples.ToString + ", nSamples")
        lPostprocinFile.AppendLine(lObsflowFN)
        lPostprocinFile.AppendLine(lParamvalFN)
        lPostprocinFile.AppendLine(lObjectiveFN)
        lPostprocinFile.AppendLine(lTimeseriesFN)
        lPostprocinFile.AppendLine(lObjpostprocFN)
        lPostprocinFile.AppendLine(lTrdoffboundFN)
        lPostprocinFile.AppendLine(lBalParamFN)
        lPostprocinFile.AppendLine(lMoscemParamFN)
        lPostprocinFile.AppendLine(lParameterFN)
        lPostprocinFile.AppendLine(lBasinFN)
        lPostprocinFile.AppendLine(lRiverFN)
        lPostprocinFile.AppendLine(lOrigbasFN)
        lPostprocinFile.AppendLine(lOrigrivFN)
        lPostprocinFile.AppendLine(lBalfilesFN)
        lPostprocinFile.AppendLine(lRoutfilesFN)
        lPostprocinFile.AppendLine(lWhichModelFN)
        SaveFileString(lPostprocinFN, lPostprocinFile.ToString)

        lLogfile.AppendLine("Initiating Post Processing Program...")

        Dim lGeosfmpostFN As String = ""
        If FileExists(pOutputPath & "geosfmpost.exe") Then
            lGeosfmpostFN = pOutputPath & "geosfmpost.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmpost.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmpost.exe", pOutputPath & "geosfmpost.exe")
            lGeosfmpostFN = pOutputPath & "geosfmpost.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmpost.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("Performing Model PostProcessing........")

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

        Dim lTsfilesize As Integer = 0
        If FileExists(lTimeseriesFN) Then
            Try
                Dim lCurrentRecord As String
                Dim lStreamReader As New StreamReader(lTimeseriesFN)
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        lTsfilesize = lTsfilesize + 1
                    End If
                Loop
            Catch e As ApplicationException
                Logger.Msg("Cannot read output file, " & lTimeseriesFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
                Return False
            End Try
        Else
            Logger.Msg("Cannot open output file, " & lTimeseriesFN & vbCrLf & "File may be open or tied up by another program", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Return False
        End If

        If (lTsfilesize > 1) Then
            FileCopy(lTimeseriesFN, pOutputPath & "tmpfile.txt")
            lLogfile.AppendLine("Post Processing Program Successfully Executed!")
        Else
            lLogfile.AppendLine("Problems encounted during post processing.")
            lLogfile.AppendLine("Output time series file, " & lTimeseriesFN & ", is empty.")
            SaveFileString(lLogfilename, lLogfile.ToString)
            Logger.Msg("Post processing problems encountered." & vbCrLf & "Output time series file, " & lTimeseriesFN & ", is empty", "Geospatial Stream Flow Model")
            Return False
        End If

        lLogfile.AppendLine("Ending Time:" & " " & DateTime.Now.ToString)
        SaveFileString(lLogfilename, lLogfile.ToString)

        Logger.Msg("Model Calibration Run Complete." & vbCrLf & "Results written to: " & vbCrLf & lTimeseriesFN, "Geospatial Stream Flow Model")
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
