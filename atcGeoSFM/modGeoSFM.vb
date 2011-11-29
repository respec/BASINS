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

    Friend Sub Terrain(ByVal aDEMLayerName As String, ByVal aSubbasinLayerName As String, ByVal aStreamLayerName As String, ByVal aThresh As Integer)

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
    End Sub

    Friend Sub Basin(ByVal aZoneGname As String, ByVal aDemGname As String, ByVal aFacGname As String, ByVal aHlenGname As String, ByVal aRcnGname As String, _
                     ByVal aWhcGname As String, ByVal aDepthGname As String, ByVal aTextureGname As String, ByVal aDrainGname As String, _
                     ByVal aFlowlenGname As String, ByVal aRivlinkGname As String, ByVal aDownGname As String, ByVal aMaxcoverGname As String)

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

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this

        Dim lDescFile As String = lOutputPath & "describe.txt"
        Dim lOutFile As String = lOutputPath & "basin.txt"
        Dim lRivFile = lOutputPath & "river.txt"
        Dim lOrderFile = lOutputPath & "order.txt"

        ' get input grids
        'defaultlist = { "Basins" , "Elevations" , "FlowAcc" , "Hilllength" , "Rcn" , "Whc" , "Soildepth" , "Texture" , "Ks", "FlowLen", "StrLinks", "Downstream", "Maxcover" }
        'labellist = { "Basin Grid" , "Processed DEM", "Flow Accumulation Grid" , "Hill Length Grid", "Runoff Curve Number Grid", "Water holding Capacity", "Soil Depth Grid" , "Soil Texture Grid" , "Hydraulic Conductivity Grid", "Downstream Flow Length Grid", "Stream Link Grid", "Downstream Basin id Grid", "Max Impervious Cover Grid" }

        Dim lBasinGridIndex As Integer = -1
        If aZoneGname = "<none>" Then
            Logger.Msg("Basin Grid, " + aZoneGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lBasinGridIndex = GisUtil.LayerIndex(aZoneGname)
        End If

        Dim lDemGridIndex As Integer = -1
        If aDemGname = "<none>" Then
            Logger.Msg("DEM Grid, " + aDemGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lDemGridIndex = GisUtil.LayerIndex(aDemGname)
        End If

        Dim lFacGridIndex As Integer = -1
        If aFacGname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + aFacGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lFacGridIndex = GisUtil.LayerIndex(aFacGname)
        End If

        Dim lHlenGridIndex As Integer = -1
        If aHlenGname = "<none>" Then
            Logger.Msg("Hill Length Grid, " + aHlenGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lHlenGridIndex = GisUtil.LayerIndex(aHlenGname)
        End If

        Dim lRcnGridIndex As Integer = -1
        If aRcnGname = "<none>" Then
            Logger.Msg("Runoff Curve Number Grid, " + aRcnGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lRcnGridIndex = GisUtil.LayerIndex(aRcnGname)
        End If

        Dim lWhcGridIndex As Integer = -1
        If aWhcGname = "<none>" Then
            Logger.Msg("Soil Water Holding Capacity Grid, " + aWhcGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lWhcGridIndex = GisUtil.LayerIndex(aWhcGname)
        End If

        Dim lDepthGridIndex As Integer = -1
        If aDepthGname = "<none>" Then
            Logger.Msg("Soil Depth Grid, " + aDepthGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lDepthGridIndex = GisUtil.LayerIndex(aDepthGname)
        End If

        Dim lTextureGridIndex As Integer = -1
        If aTextureGname = "<none>" Then
            Logger.Msg("Soil Texture Grid, " + aTextureGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lTextureGridIndex = GisUtil.LayerIndex(aTextureGname)
        End If

        Dim lDrainGridIndex As Integer = -1
        If aDrainGname = "<none>" Then
            Logger.Msg("Hydraulic Conductivity Grid, " + aDrainGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lDrainGridIndex = GisUtil.LayerIndex(aDrainGname)
        End If

        Dim lFlowlenGridIndex As Integer = -1
        If aFlowlenGname = "<none>" Then
            Logger.Msg("Downstream Flow Length Grid, " + aFlowlenGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lFlowlenGridIndex = GisUtil.LayerIndex(aFlowlenGname)
        End If

        Dim lRivlinkGridIndex As Integer = -1
        If aRivlinkGname = "<none>" Then
            Logger.Msg("Stream Link Grid, " + aRivlinkGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lRivlinkGridIndex = GisUtil.LayerIndex(aRivlinkGname)
        End If

        Dim lDownGridIndex As Integer = -1
        If aDownGname = "<none>" Then
            Logger.Msg("Downstream Basin Id Grid, " + aDownGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            lDownGridIndex = GisUtil.LayerIndex(aDownGname)
        End If

        Dim lMaxcoverGridIndex As Integer = -1
        If aMaxcoverGname = "<none>" Then
            Logger.Msg("Maximum Impervious Cover Grid, " + aMaxcoverGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
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

            Dim baseflowlag As String = Format((CStr(lInterflowlag) * 3), "#.####")

            'RcnZoneVtab
            Dim rcnvalue As String = Format(lRcnZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "#.0") '(RcnZoneVtab.ReturnValue(Rcnfield, rrecord)).SetFormat("d.d").AsString

            'whcZoneVtab
            Dim whcvalue As String = Format(lWhcZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "###.###")  '(whcZoneVtab.ReturnValue(Whcfield, rrecord)).AsString

            'DepthZoneVtab
            Dim depthvalue As String = Format(lDepthZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mean").Value, "###.###") '(DepthZoneVtab.ReturnValue(depthfield, rrecord)).AsString

            'TextureZoneVtab
            Dim texturevalue As String = lTextureZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Mode").Value.ToString '(TextureZoneVtab.ReturnValue(texturefield, rrecord)).AsString

            ' Soil texture (1=Sand,2=Loam,3=Clay,5=Water)
            Dim basinlossvalue As String = ""
            If texturevalue = "1" Then
                basinlossvalue = "0.95"
            ElseIf texturevalue = "2" Then
                basinlossvalue = "0.97"
            ElseIf texturevalue = "3" Then
                basinlossvalue = "0.99"
            ElseIf texturevalue = "5" Then
                basinlossvalue = "0.98"
            Else
                basinlossvalue = "0.99"
            End If

            'rivdemZoneVtab
            Dim rivdropvalue As Single = lRivDemZonalStats.ItemByKey(lBasinValue).GetValue("Max") - lRivDemZonalStats.ItemByKey(lBasinValue).GetDefinedValue("Min").Value '(rivdemZoneVtab.ReturnValue(rivdemfield, rrecord))
            Dim rivslopevalue As String = Format((rivdropvalue * 100) / CStr(lRivlenValue), "0.0000")   '(((rivdropvalue * 100) / CStr(rivlenvalue)).SetFormat("d.dddd")).AsString
            If Not IsNumeric(rivslopevalue) Then
                rivslopevalue = "0.0010"
            End If
            If CStr(rivslopevalue) < 0.001 Then
                rivslopevalue = "0.0010"
            End If

            Dim lCelerity As String = ""
            Dim lDiffusion As String = ""
            If CSng(rivslopevalue) < 0.1 Then

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

            ElseIf CSng(rivslopevalue) < 0.2 Then

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

            ElseIf CSng(rivslopevalue) < 0.3 Then

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

            ElseIf CSng(rivslopevalue) < 0.4 Then

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
            Dim downvalue As String = lDownZonalStats.ItemByKey(lBasinValue).GetValue("Mode", "").ToString '(DownZoneVtab.ReturnValue(downfield, rrecord)).AsString

            'MaxCoverZoneVtab
            Dim maxcovervalue As String = Format((lMaxCoverZonalStats.ItemByKey(lBasinValue).GetValue("Mean", GetNaN) / 100), "0.####") '((MaxCoverZoneVtab.ReturnValue(maxcoverfield, rrecord)) / 100).SetFormat("d.ddddd").AsString
            If maxcovervalue.Length = 0 OrElse CSng(maxcovervalue) <= 0.001 Then
                maxcovervalue = "0.001"
            ElseIf CSng(maxcovervalue) >= 1 Then
                maxcovervalue = "1.0"
            End If

            Dim hasdamvalue As String = "0" '(BasinTable.ReturnValue(hasdamfld, rrecord)).SetFormat("d").AsString
            Dim rivpolyloss As String = "1.0"
            Dim hasrating As String = "0"
            Dim hasflowdata As String = "0"
            Dim rivwidth As String = Format(6.13 * (CSng(lUpareaValue) ^ (0.347)), "#.###")
            Dim flowref As String = Format(36 * 0.02832 * ((CSng(lAreaValue) / 2.59) ^ (0.68)), "#.####")
            Dim runtype As String = "0"
            Dim mannvalue As String = "0.035"
            Dim pancoef As String = "0.85"
            Dim topsoil As String = "0.1"
            Dim aridity As String = "2"
            lSBOut.AppendLine(CStr(lBasinValue) + "," + whcvalue + "," + depthvalue + "," + texturevalue + "," + CStr(lDrainValue) + "," + Format(lAreaValue, "#.0") + "," + lInterflowlag + "," + lSlopeValue + "," + baseflowlag + "," + rcnvalue + "," + maxcovervalue + "," + basinlossvalue + "," + pancoef + "," + topsoil + "," + aridity)
            lSBRiv.AppendLine(CStr(lBasinValue) + "," + Format(lAreaValue, "#.0") + "," + Format(lUpareaValue, "#.0") + "," + rivslopevalue + "," + lRivlenValue + "," + downvalue + "," + mannvalue + "," + lRiverlossValue + "," + rivpolyloss + "," + hasdamvalue + "," + hasrating + "," + hasflowdata + "," + lCelerity + "," + lDiffusion + "," + rivwidth + "," + flowref + "," + runtype)
            lSBOrder.AppendLine(CStr(lBasinValue))
        Next

        SaveFileString(lOutFile, lSBOut.ToString)
        SaveFileString(lRivFile, lSBRiv.ToString)
        SaveFileString(lOrderFile, lSBOrder.ToString)

        If FileExists(lOutputPath & "basin_original.txt") Then
            IO.File.Delete(lOutputPath & "basin_original.txt")
        End If
        IO.File.Copy(lOutFile, lOutputPath & "basin_original.txt")

        If FileExists(lOutputPath & "river_original.txt") Then
            IO.File.Delete(lOutputPath & "river_original.txt")
        End If
        IO.File.Copy(lRivFile, lOutputPath & "river_original.txt")

        Logger.Msg("Basin Characteristics Computed. Outputs written to: " & vbCrLf & vbCrLf & "      " & lOutFile & vbCrLf & "      " & lOrderFile & vbCrLf & "      " & lRivFile, "Geospatial Stream Flow Model")

    End Sub

    Friend Sub Response(ByVal veltype As Integer, _
                        ByVal zonegname As String, ByVal flowlengname As String, _
                        ByVal outletgname As String, ByVal demgname As String, ByVal facgname As String, ByVal fdirgname As String, _
                        ByVal UsgsLandCoverGname As String, ByVal OverlandFlowVelocity As Double, ByVal InstreamFlowVelocity As Double)

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

        'TODO: Implement for USGS Land Cover Grid

        If veltype = 1 Then
            Logger.Msg("'Non-Uniform from USGS Land Cover Grid' option is not yet implemented.", MsgBoxStyle.Critical, "")
            Exit Sub
        End If
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

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this
        Dim OrderFileName As String = lOutputPath & "order.txt"

        Dim basingthm As Integer = -1
        If zonegname = "<none>" Then
            Logger.Msg("Basin Grid, " + zonegname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            basingthm = GisUtil.LayerIndex(zonegname)
        End If

        Dim flowdirgthm As Integer = -1
        If fdirgname = "<none>" Then
            Logger.Msg("Flow Direction Grid, " + fdirgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            flowdirgthm = GisUtil.LayerIndex(fdirgname)
        End If
        Dim lFlowDirGridFileName As String = GisUtil.LayerFileName(flowdirgthm)

        Dim flowlengthm As Integer = -1
        If flowlengname = "<none>" Then
            Logger.Msg("Flow Length Grid, " + flowlengname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            flowlengthm = GisUtil.LayerIndex(flowlengname)
        End If

        Dim outgthm As Integer = -1
        If outletgname = "<none>" Then
            Logger.Msg("Outlet Grid, " + outletgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            outgthm = GisUtil.LayerIndex(outletgname)
        End If

        Dim zonelist As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(OrderFileName)
            lCurrentRecord = lStreamReader.ReadLine  'only if first line is a header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    zonelist.Add(lCurrentRecord)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Generate basin file' menu to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Exit Sub
        End Try

        Dim facgthm As Integer = -1
        If facgname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + facgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            facgthm = GisUtil.LayerIndex(facgname)
        End If
        Dim lFlowAccGridFileName As String = GisUtil.LayerFileName(facgthm)

        Dim demgthm As Integer = -1
        If demgname = "<none>" Then
            Logger.Msg("DEM Grid, " + demgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            demgthm = GisUtil.LayerIndex(demgname)
        End If

        Dim mycellsize As Integer = GisUtil.GridGetCellSizeX(basingthm)
        Dim lZoneLayerIndex As Integer = GisUtil.LayerIndex(zonegname)
        Dim lZoneFileName As String = GisUtil.LayerFileName(lZoneLayerIndex)
        Dim lDemFileName As String = GisUtil.LayerFileName(demgthm)
        Dim lVelocityGridLayerName As String = "Velocity Grid"
        Dim lVelocityGridLayerIndex As Integer = 0
        Dim lVelocityGridFileName As String = ""

        If (veltype = 1) Then

            '  meandrop = demgrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false) 
            '  mindrop = demgrid.zonalStats(#GRID_STATYPE_MIN, basingrid, Prj.MakeNull, basinField, false)
            Logger.Status("Computing Zonal Statistics for " + demgname + "........")
            Dim lDemZonalStats As New atcCollection
            lDemZonalStats = GisUtil.GridZonalStatistics(basingthm, demgthm)
            '  avgdrop = meandrop - mindrop

            '  avgraw = flowlengrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false)
            Logger.Status("Computing Zonal Statistics for " + flowlengname + "........")
            Dim lFlowLenZonalStats As New atcCollection
            lFlowLenZonalStats = GisUtil.GridZonalStatistics(basingthm, flowlengthm)

            'Grid.Con(Yes,No) 
            '  avglength = (avgraw < mycellsize).con(mycellsize.asgrid, avgraw)
            '  sloperaw = ((avgdrop * 100) / avglength)
            '  slopegrid = (sloperaw < 0.001).con(0.001.asgrid, sloperaw)

            Dim lcovgthm As Integer = -1
            If UsgsLandCoverGname = "<none>" Then
                Logger.Msg("USGS Land Cover Grid, " + UsgsLandCoverGname + ", Not Found in the View", MsgBoxStyle.Critical, "")
                Exit Sub
            Else
                lcovgthm = GisUtil.LayerIndex(UsgsLandCoverGname)
            End If

            '  Mannlist = { "0.03","0.03","0.035","0.033","0.035","0.04","0.05","0.05","0.05","0.06","0.1","0.1","0.12","0.12","0.1","0.035","0.05","0.05","0.03","0.05","0.05","0.05","0.04","0.04" }
            '  lcvaluelist = { 100,211,212,213,280,290,311,321,330,332,411,412,421,422,430,500,620,610,770,820,810,850,830,900 }
            '  lcnamelist = {"Urban and Built-Up Land", "Dryland Cropland and Pasture", "Irrigated Cropland and Pasture", "Mixed Dryland/Irrigated Cropland and Pasture", "Cropland/Grassland Mosaic", "Cropland/Woodland Mosaic", "Grassland", "Shrubland", "Mixed Shrubland/Grassland", "Savanna", "Deciduous Broadleaf Forest", "Deciduous Needleleaf Forest", "Evergreen Broadleaf Forest", "Evergreen Needleleaf Forest", "Mixed Forest", "Water Bodies", "Herbaceous Wetland", "Wooded Wetland", "Barren or Sparsely Vegetated", "Herbaceous Tundra", "Wooded Tundra", "Mixed Tundra", "Bare Ground Tundra", "Snow or Ice" }

            '  lccodelist = MsgBox.MultiInput("Specify Mannings (Velocity) Coefficients for each land cover", "Land Cover, Anderson Code, Manning's N", lcnamelist, Mannlist)
            '  If (lccodelist.isempty) Then
            '     Exit Sub
            '  End If
            '  lcfield = lcovVtab.FindField("lc_code".Lcase)
            '  If (lcfield = nil) Then
            '    lcfield = lcovVtab.FindField("lccode".Lcase)
            '    If (lcfield = nil) Then
            '      lcfield = lcovVtab.FindField("lu_code".Lcase)
            '      If (lcfield = nil) Then
            '        lcfield = lcovVtab.FindField("lucode".Lcase)
            '        If (lcfield = nil) Then
            '          lcfldlst = lcovVtab.getfields.deepclone
            '          lcfield = MsgBox.choiceasstring(lcfldlst, "Select the field containing the" + nl + "Anderson Land Cover Classification Code eg lc_code", "Geospatial Stream Flow Model")
            '          If (lcfield = nil) Then
            '            Exit Sub
            '          End If
            '        End If
            '      End If
            '    End If
            '  End If

            '  lcfile = LineFile.Make((myWkdirname + "roughness.txt").AsFileName, #FILE_PERM_WRITE)
            '  if (lcfile = nil) then
            '    msgbox.error("Cannot create roughness.txt file"+nl+"File may be open or held up by another program", "Geospatial Stream Flow Model")
            '    exit
            '  end

            '  If (lcovVtab.CanEdit.Not) Then
            '    MsgBox.info("Cannot Edit USGS Land Cover Grid." + nl + "Copy it to a location where you have write access.", "Geospatial Stream Flow Model")
            '    Exit Sub
            '  Else
            '    lcovVtab.seteditable(True)
            '    mannField = Field.Make("ManningN", #FIELD_DOUBLE, 10, 5)  
            '    lcovVtab.AddFields({mannfield})

            '    testlocate = 0

            '    For Each lrec In lcovVtab
            '      lcvalue = (lcovVtab.ReturnValue(lcfield, lrec))
            '      If (lcvalue = 0) Then
            '        lcovVtab.setvalue(mannfield, lrec, 0.05)
            '      Else
            '        lstlocate = lcvaluelist.findbyvalue(lcvalue)
            '        If ((lstlocate = -1) And (testlocate = 0)) Then
            '          MsgBox.info("Anderson Code of " + lcvalue.asstring + " is not supported in the USGS Land Cover Grid" + nl + "Defaulting to Mannings n of 0.05", "Geospatial Stream Flow Model")
            '          testlocate = 1
            '          mannvalue = 0.05
            '        ElseIf ((lstlocate = -1) And (testlocate = 1)) Then
            '          mannvalue = 0.05
            '        Else
            '          mannvalue = lccodelist.get(lstlocate).asnumber
            '        End If
            '          lcovVtab.setvalue(mannfield, lrec, Mannvalue)
            '      End If
            '    Next

            '    lcovgrid = lcovgthm.getgrid
            '    manngrid = lcovgrid.lookup(mannfield.getname)

            ' from Manning's equation, v = (1/n)(R^(2/3)(S^0.5) = K (S^0.5)
            ' for Flowacc = 0-1000, R = 0.002,             k = (1/n)(0.002^(2/3)      = 0.0158740/n
            ' for Flowacc = 1000-2000, R = 0.005,             k = (1/n)(0.005^(2/3)      = 0.0292402/n
            ' for Flowacc = 2000-3000, R = 0.01,             k = (1/n)(0.01^(2/3)      = 0.0464159/n
            ' for Flowacc = 3000-4000, R = 0.02,             k = (1/n)(0.02^(2/3)      = 0.0736806/n   
            ' for Flowacc = 4000-5000, R = 0.05,             k = (1/n)(0.05^(2/3)      = 0.1357209/n
            ' for Flowacc = 5000-10000,              vel = 0.3
            ' for Flowacc = 10000-50000,             vel = 0.45
            ' for Flowacc = 50000-100000,            vel = 0.6
            ' for Flowacc = 100000-250000,           vel = 0.75
            ' for Flowacc = 250000-500000,           vel = 0.9
            ' for Flowacc = 500000-750000,           vel = 1.2
            ' for Flowacc = > 750000,                vel = 1.5

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

            '    If (file.exists((myWkDirname + "velocity").AsFileName)) Then
            '      grid.DeleteDataset((myWkDirname + "velocity").AsFileName)
            '    End If

            '    velgthm = Gtheme.Make(velgrid)

            '  End If

        ElseIf (veltype = 2) Then
            'set velocity grid based on a threshold value
            ' velgrid = (facgrid < 1000).con((OverlandFlowVelocity).asgrid, (InstreamFlowVelocity).asgrid)
            ' velgthm = Gtheme.Make(velgrid)
            Dim lValue As Double = OverlandFlowVelocity
            If GisUtil.IsLayer(lVelocityGridLayerName) Then
                lVelocityGridLayerIndex = GisUtil.LayerIndex(lVelocityGridLayerName)
                GisUtil.RemoveLayer(lVelocityGridLayerIndex)
            End If
            lVelocityGridFileName = FilenameNoExt(lDemFileName) & "Velocity.bgd"
            GisUtil.GridAssignConstant(lVelocityGridFileName, lDemFileName, lValue)
            'now for cells >= 1000, use instream flow velocity
            lValue = InstreamFlowVelocity
            GisUtil.GridAssignConstantAboveThreshold(lVelocityGridFileName, lFlowAccGridFileName, 999, lValue)
            GisUtil.AddLayer(lVelocityGridFileName, lVelocityGridLayerName)
        Else
            'set velocity grid to a constant
            Dim lValue As Double = OverlandFlowVelocity
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
        Dim lOutletGridFileName As String = GisUtil.LayerFileName(outgthm)

        '        invelgrid = (1.AsGrid / velgrid)
        Dim lInverseVelocityGridFileName As String = FilenameNoExt(lVelocityGridFileName) & "InverseVelocity.bgd"
        GisUtil.GridInverse(lVelocityGridFileName, lInverseVelocityGridFileName)

        '        flowtimegrid = newfdrgrid.flowlength(invelgrid, False)
        Dim lTravelTimeGridFileName As String = FilenameNoExt(lFlowAccGridFileName) & "TravelTime." & FileExt(lFlowAccGridFileName)

        '        inverse velocity grid is used as a weighting factor 
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
        Dim numdays As Integer = GisUtil.GridLayerMaximum(lDaysGridLayerIndex)

        '        this comment is from the avenue script, not sure what to make of it
        '        ' This is a temporary solution that will be removed when
        '        ' the routing program can read a parameter file

        If (numdays > 22) Then
            numdays = 22
            'else 
            '  numdays = 22
        End If

        'prepare response file
        Dim lRespOut As New StringBuilder
        Dim lStr As String = """" & "BasinId" & """"
        For nday As Integer = 0 To numdays - 1
            lStr = lStr & ", " & """" & "Day" & CStr(nday) & """"
        Next
        lRespOut.AppendLine(lStr)

        'figure out how many cells of each 'day' in each zone
        Dim lCnt As Integer = 0
        Dim lDaysArray(zonelist.Count, numdays) As Integer
        For nday As Integer = 0 To numdays - 1
            Dim lDaysZonalStats As atcCollection = GisUtil.GridZoneCountValue(basingthm, lDaysGridLayerIndex, nday)
            lCnt = 0
            For Each lzone As Integer In zonelist
                lCnt += 1
                If Not lDaysZonalStats.ItemByKey(lzone) Is Nothing Then
                    lDaysArray(lCnt, nday) = lDaysZonalStats.ItemByKey(lzone)
                End If
            Next
        Next

        lCnt = 0
        For Each bid As Integer In zonelist
            lCnt += 1
            lStr = CStr(bid)
            Dim lSum As Integer = 0
            For nday As Integer = 0 To numdays - 1
                lSum += lDaysArray(lCnt, nday)
            Next
            For nday As Integer = 0 To numdays - 1
                If lSum > 0 Then
                    lStr = lStr & ", " & Format((lDaysArray(lCnt, nday) / lSum), "0.000000")
                Else
                    lStr = lStr & ", 0.000000"
                End If
            Next
            lRespOut.AppendLine(lStr)
        Next

        Dim lOutFile As String = lOutputPath & "response.txt"
        SaveFileString(lOutFile, lRespOut.ToString)

        Logger.Msg("Basin Response Computed. Output file: " & vbCrLf & lOutputPath + "response.txt", "Geospatial Stream Flow Model")

    End Sub

    Friend Sub RainEvap(ByVal aPrecGageNamesBySubbasin As Collection, ByVal aEvapGageNamesBySubbasin As Collection, ByVal aSJDate As Double, ByVal aEJDate As Double)
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

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this
        Dim OrderFileName As String = lOutputPath & "order.txt"

        Dim lSubbasins As New atcCollection
        Try
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(OrderFileName)
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
            Logger.Msg("Cannot determine computational order." & vbCrLf & "Run 'Generate basin file' menu to create order.txt", MsgBoxStyle.Critical, "Geospatial Stream Flow Model")
            Exit Sub
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

        Dim lOutFile As String = lOutputPath & "rain.txt"
        SaveFileString(lOutFile, lRainOut.ToString)

        lOutFile = lOutputPath & "evap.txt"
        SaveFileString(lOutFile, lEvapOut.ToString)

        Logger.Msg("Processing Complete. Output files: " & vbCrLf & lOutputPath + "rain.txt" & vbCrLf & lOutputPath + "evap.txt", "Geospatial Stream Flow Model")

    End Sub

    Friend Sub Balance(ByVal inifract As Single, ByVal dformat As Integer, ByVal inimode As Integer, ByVal runmode As Integer, _
                       ByVal abalancetype As Integer, ByVal aSjDate As Double)
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

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this

        Dim rainfilename As String = lOutputPath & "rain.txt"
        Dim evapfilename As String = lOutputPath & "evap.txt"
        Dim basinfilename As String = lOutputPath & "basin.txt"
        Dim respfilename As String = lOutputPath & "response.txt"
        Dim paramfilename As String = lOutputPath & "balparam.txt"
        Dim surpfilename As String = lOutputPath & "basinrunoffyield.txt"
        Dim storefilename As String = lOutputPath & "soilwater.txt"
        Dim aevapfilename As String = lOutputPath & "actualevap.txt"
        Dim gwlossfilename As String = lOutputPath & "gwloss.txt"
        Dim outswfilename As String = lOutputPath & "cswater.txt"
        Dim excessfilename As String = lOutputPath & "excessflow.txt"
        Dim interfilename As String = lOutputPath & "interflow.txt"
        Dim basefilename As String = lOutputPath & "baseflow.txt"
        Dim massfilename As String = lOutputPath & "massbalance.txt"
        Dim logfilename As String = lOutputPath & "logfilesoil.txt"
        Dim initialfilename As String = lOutputPath & "initial.txt"
        Dim balfilename As String = lOutputPath & "balfiles.txt"
        Dim maxtimefilename As String = lOutputPath & "maxtime.txt"
        Dim testfilename As String = lOutputPath & "testfile.txt"
        Dim whichmodelFN As String = lOutputPath & "whichmodel.txt"

        If Not FileExists(rainfilename) Then
            Logger.Msg("Could not open rain file," & vbCrLf & rainfilename, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If Not FileExists(evapfilename) Then
            Logger.Msg("Could not open evap file," & vbCrLf & evapfilename, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If Not FileExists(basinfilename) Then
            Logger.Msg("Could not open basin file," & vbCrLf & basinfilename, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If Not FileExists(respfilename) Then
            Logger.Msg("Could not open response file," & vbCrLf & respfilename, "Geospatial Stream Flow Model")
            Exit Sub
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
        lChkFile.AppendLine(lOutputPath)
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
            Exit Sub
        End If

        If (runmode = 0) Then
            If Not FileExists(outswfilename) Then
                Logger.Msg("Could not open current soil water file," & vbCrLf & outswfilename, "Geospatial Stream Flow Model")
                Exit Sub
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
                lId = GisUtil.FieldValue(basinthm, lIndex, 1)
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

    End Sub

    Friend Sub Route(ByVal forecastdays As Integer, ByVal runmode As Integer, ByVal dformat As Integer, _
                     ByVal aRouteMethod As Integer, ByVal aSjDate As Double)
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

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this

        Dim runoffFN As String = lOutputPath & "basinrunoffyield.txt"
        Dim responseFN As String = lOutputPath & "response.txt"
        Dim riverFN As String = lOutputPath & "river.txt"
        Dim reservoirFN As String = lOutputPath & "reservoir.txt"
        Dim riverdepthFN As String = lOutputPath & "riverdepth.txt"
        Dim inflowFN As String = lOutputPath & "inflow.txt"
        Dim flowFN As String = lOutputPath & "streamflow.txt"
        Dim damsFN As String = lOutputPath & "damstatus.txt"
        Dim localflowFN As String = lOutputPath & "localflow.txt"
        Dim routparamFN As String = lOutputPath & "routparam.txt"
        Dim routfilesFN As String = lOutputPath & "routfiles.txt"
        Dim forecastFN As String = lOutputPath & "forecast.txt"
        Dim logFN As String = lOutputPath & "logfileflow.txt"

        If (forecastdays > 99) Then
            forecastdays = 99
        ElseIf (forecastdays < 0) Then
            forecastdays = 0
        End If

        Dim initialFN As String = lOutputPath & "initial.txt"
        Dim timesFN As String = lOutputPath & "times.txt"
        'Dim maxtimeFN As String = lOutputPath & "maxtime.txt"
        Dim damlinkFN As String = lOutputPath & "damlink.txt"
        Dim ratingFN As String = lOutputPath & "rating.txt"
        Dim whichmodelFN As String = lOutputPath & "whichmodel.txt"

        If Not FileExists(runoffFN) Then
            Logger.Msg("Could not open runoff file." & vbCrLf & runoffFN, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If Not FileExists(responseFN) Then
            Logger.Msg("Could not open response file," & vbCrLf & responseFN, "Geospatial Stream Flow Model")
            Exit Sub
        End If

        If Not FileExists(riverFN) Then
            Logger.Msg("Could not open river file," & vbCrLf & riverFN, "Geospatial Stream Flow Model")
            Exit Sub
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
            Exit Sub
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
            Exit Sub
        End Try

        If (lResponseList.Count < lRivList.Count) Then
            Logger.Msg("The response file " + responseFN + " does not contain enough subbasins.", "Geospatial Stream Flow Model")
            Exit Sub
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
            Exit Sub
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
                        Exit Sub
                    End Try

                    isfirst = isfirst + 1
                End If

                Dim isoperated As Integer = 0
                Dim resdtime As Single = 0.0
                Dim lDamoutFile As New StringBuilder
                Dim lDamoutFileName As String = lOutputPath & "dam" & riverid.ToString & ".txt"

                Dim resvlist As String = ""
                If lReservoirList.ItemByKey(riverid) Is Nothing Then
                    Logger.Msg("No reservoir characteristic found for basin " & riverid & vbCrLf & "Update " & reservoirFN & " before computing streamflow.", "Geospatial Stream Flow Model")
                    Exit Sub
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
                        If Not FileExists(lOutputPath & operateFN) Then
                            Logger.Msg("Operations file " & operateFN & " for reservoir in basin " & riverid & "Not found." + vbCrLf + "Create the file before computing streamflow", "Geospatial Stream Flow Model")
                            Exit Sub
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
                        Exit Sub
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
                lDamLinkFile.AppendLine(lOutputPath & "dam" & riverid.ToString & ".txt")
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
                Exit Sub
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
        lRoutFile.AppendLine(lOutputPath)
        SaveFileString(routfilesFN, lRoutFile.ToString)

        If (aRouteMethod = 3) Then
            LAGROUTE(routfilesFN)
        ElseIf (aRouteMethod = 1) Then
            DIFFROUTE(routfilesFN)
        Else
            cungeroute(routfilesFN)
        End If

        'timefile.writeelt(myrunoff.asstring)  'commented out in the avenue 
        'timefile.close

        If Not FileExists(flowFN) Then
            Logger.Msg("An error occurred during the routing computations." & vbCrLf & "Check for possible causes in logfileflow.txt", "Geospatial Stream Flow Model")
            Exit Sub
        End If

        lChkFile.AppendLine("Ending Time: " & System.DateTime.Now.ToString)
        SaveFileString(timesFN, lChkFile.ToString)

        Logger.Msg("Stream Flow Routing Complete. Results written to: " & vbCrLf & flowFN, "Geospatial Stream Flow Model")

    End Sub

    Friend Sub Sensitivity(ByVal aSelectedReachIndex As Integer, ByVal aBalanceType As Integer, ByVal aRouteType As Integer, _
                           ByVal aParameterRanges As atcCollection)
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
        Dim lOutputPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "modelout\GeoSFM\"   'will need to do more with this

        'read/write balance parameters
        Dim lBalParamFN As String = lOutputPath & "balparam.txt"
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
            Exit Sub
        End Try
        'write it out in calibration mode
        Dim lParamFile As New StringBuilder
        For lIndex As Integer = 1 To 9
            lParamFile.AppendLine(lBalList(lIndex))
        Next
        lParamFile.AppendLine(1.ToString)
        SaveFileString(lBalParamFN, lParamFile.ToString)

        'read/write routing parameters
        Dim lRoutParamFN As String = lOutputPath & "routparam.txt"
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
            Exit Sub
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

        Dim logfileFN As String = lOutputPath & "logfilesarun.txt"
        Dim logfile As New StringBuilder

        Dim balancetype As Integer = 0
        Dim routetype As Integer = 0
        Dim whichmodelFN As String = lOutputPath & "whichModel.txt"
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
                Exit Sub
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

        Dim rangeFileFN As String = lOutputPath & "range.txt"
        Dim rangeFile As New StringBuilder
        rangeFile.AppendLine("Min, Max, Name, Description")
        For rindex As Integer = 0 To aParameterRanges.Count - 1
            rangeFile.AppendLine(aParameterRanges.ItemByIndex(rindex) & ", " & aParameterRanges.Keys(rindex))
        Next
        SaveFileString(rangeFileFN, rangeFile.ToString)

        logfile.AppendLine("River and Basin Files Read")

        Dim moscemparamfileFN As String = lOutputPath & "moscem_param.txt"
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
        If FileExists(lOutputPath & "geosfmsarun.exe") Then
            geosfmsarunFN = lOutputPath & "geosfmsarun.exe"
        ElseIf FileExists(lBasinsBinLoc & "\geosfmsarun.exe") Then
            File.Copy(lBasinsBinLoc & "\geosfmsarun.exe", lOutputPath & "geosfmsarun.exe")
            geosfmsarunFN = lOutputPath & "geosfmsarun.exe"
        Else
            Logger.Msg("Unable to locate the program file: geosfmsarun.exe " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Exit Sub
        End If

        logfile.AppendLine("geosfmsarun.exe program copied to working directory")

        Dim geosfmdllFN As String = ""
        If FileExists(lOutputPath & "geosfm.dll") Then
            geosfmdllFN = lOutputPath & "geosfm.dll"
        ElseIf FileExists(lBasinsBinLoc & "\geosfm.dll") Then
            File.Copy(lBasinsBinLoc & "\geosfm.dll", lOutputPath & "geosfm.dll")
            geosfmdllFN = lOutputPath & "geosfm.dll"
        Else
            Logger.Msg("Unable to locate the program file: geosfm.dll " & vbCrLf & vbCrLf & "Install the programs in your BASINS/bin folder.", "Geospatial Stream Flow Model")
            Exit Sub
        End If

        logfile.AppendLine("geosfm.dll program copied to working directory")

        If Not FileExists(lOutputPath & "dforrt.dll") Then
            If FileExists(lBasinsBinLoc & "\dforrt.dll") Then
                File.Copy(lBasinsBinLoc & "\dforrt.dll", lOutputPath & "dforrt.dll")
            End If
        End If

        Dim lProcess As New Diagnostics.Process
        With lProcess.StartInfo
            .FileName = lOutputPath & "geosfmsarun.exe"
            .WorkingDirectory = lOutputPath
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

        Dim SArunOutputFN As String = lOutputPath & "sarunoutput.txt"
        If FileExists(SArunOutputFN) Then
            Logger.Msg("Sensitivity Analysis Complete. Results written to: sarunoutput.txt", "Geospatial Stream Flow Model")
        Else
            Logger.Msg("Output file, sarunoutput.txt, not found." & vbCrLf & "Check log file, logfilesarun.txt, for possible errors", "Geospatial Stream Flow Model")
        End If

    End Sub

    Friend Sub Calibrate()
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

        '        balparamfn = myWkDirname + "balparam.txt"
        '        If (File.Exists(balparamfn.AsFileName).not) Then
        '  av.run("FEWS.balance.ave", {})
        '        End If
        'balparamFile = LineFile.Make(balparamfn.asfilename,#file_perm_modify)
        '        If (balparamFile = nil) Then
        '            MsgBox.info("Could not open balparam.txt file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        If (balparamFile.getsize < 9) Then
        '            MsgBox.info("Not enough parameters in balparam.txt file," + nl + "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        ballist = List.make
        '        balparamFile.read(ballist, balparamFile.getsize)
        '        balparamfile.gotobeg()
        '        balparamfile.setpos(9)
        '        balparamfile.writeelt("1")
        '        balparamfile.flush()
        '        balparamFile.close()

        '        routparamfn = myWkDirname + "routparam.txt"

        '        If (File.Exists(routparamfn.AsFileName).not) Then
        '  av.run("FEWS.route.ave", {})
        '        End If
        'routparamFile = LineFile.Make(routparamfn.asfilename,#file_perm_modify)
        '        If (routparamFile = nil) Then
        '            MsgBox.info("Could not open routparam.txt file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        If (routparamFile.getsize < 9) Then
        '            MsgBox.info("Not enough parameters in routparam.txt file," + nl + "Check the contents of this file before continuing.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        routlist = List.make
        '        routparamFile.read(routlist, routparamFile.getsize)
        '        routparamFile.gotobeg()
        '        routparamFile.setpos(9)
        '        routparamFile.writeelt("1")
        '        routparamFile.flush()
        '        routparamFile.close()

        '        balfilesfn = myWkDirname + "balfiles.txt"
        '        If (File.Exists(balfilesfn.AsFileName).not) Then
        '  av.run("FEWS.balance.ave", {})
        '        End If
        'balfilesFile = LineFile.Make(balfilesfn.asfilename,#file_perm_read)
        '        If (balfilesFile = nil) Then
        '            MsgBox.info("Could not open balfiles.txt file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        balfilesFile.close()

        '        routfilesfn = myWkDirname + "routfiles.txt"
        '        If (File.Exists(routfilesfn.AsFileName).not) Then
        '  av.run("FEWS.route.ave", {})
        '        End If
        'routfilesFile = LineFile.Make(routfilesfn.asfilename,#file_perm_read)
        '        If (routfilesFile = nil) Then
        '            MsgBox.info("Could not open routfiles.txt file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        routfilesFile.close()

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

        'calibdefaults = { "parameter.in", 
        '          "objoptflag.in", 
        '          "observed_streamflow.txt", 
        '          "objectives.out", 
        '          "parameter_values.out", 
        '          "par_convergence.out", 
        '          "moscem.in", 
        '          "moscem_param.txt", 
        '          "whichModel.txt", 
        '          "basin_original.txt", 
        '          "river_original.txt", 
        '          "basin.txt", 
        '          "river.txt", 
        '          "streamflow.txt" }

        '        inpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", caliblabels, calibdefaults)

        '        If (inpList.IsEmpty) Then
        '            Exit Sub
        '        End If

        '        parameterFN = myWkDirname + inplist.get(0)
        '        objoptflagFN = myWkDirname + inplist.get(1)
        '        obsflowFN = myWkDirname + inplist.get(2)
        '        objectiveFN = myWkDirname + inplist.get(3)
        '        paramvalFN = myWkDirname + inplist.get(4)
        '        paramconFN = myWkDirname + inplist.get(5)
        '        mosceminFN = myWkDirname + inplist.get(6)
        '        moscemparamFN = myWkDirname + inplist.get(7)
        '        whichModelFN = myWkDirname + inplist.get(8)
        '        origbasFN = myWkDirname + inplist.get(9)
        '        origrivFN = myWkDirname + inplist.get(10)
        '        basinFN = myWkDirname + inplist.get(11)
        '        riverFN = myWkDirname + inplist.get(12)
        '        streamflowFN = myWkDirname + inplist.get(13)
        '        'timeseriesFN = myWkDirname + inplist.get(14)
        '        'objpostprocFN = myWkDirname + inplist.get(15)
        '        'trdoffboundFN = myWkDirname + inplist.get(16)
        '        'postprocinFN = myWkDirname + inplist.get(17)

        'logfile = LineFile.Make((myWkDirname + "logfilecalib.txt").asfilename,#file_perm_write)
        '        If (logfile = nil) Then
        '            MsgBox.info("Could not create logfilecalib.txt file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'paraminFile = LineFile.Make(parameterFN.asfilename,#file_perm_write)
        '        If (paraminFile = nil) Then
        '            MsgBox.info("Could not create " + parameterFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'objoptflagFile = LineFile.Make(objoptflagFN.asfilename,#file_perm_write)
        '        If (objoptflagFile = nil) Then
        '            MsgBox.info("Could not open " + objoptflagFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        If (File.Exists(obsflowFN.AsFileName).not) Then
        '            MsgBox.info("Could not find observed streamflow file, " + nl + obsflowFN + nl + "Place the file in the work directory before running this program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        Else
        '  observedfile = LineFile.Make(obsflowFN.asfilename,#file_perm_read)
        '            If (observedfile = nil) Then
        '                MsgBox.info("Could not open observed streamflow file, " + nl + obsflowFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '        End If

        'objectivesFile = LineFile.Make(objectiveFN.asfilename,#file_perm_write)
        '        If (objectivesFile = nil) Then
        '            MsgBox.info("Could not create file, " + objectiveFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'paramvaluesFile = LineFile.Make(paramvalFN.asfilename,#file_perm_write)
        '        If (paramvaluesFile = nil) Then
        '            MsgBox.info("Could not create file, " + paramvalFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'parconvergeFile = LineFile.Make(paramconFN.asfilename,#file_perm_write)
        '        If (parconvergeFile = nil) Then
        '            MsgBox.info("Could not create file, " + paramconFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'mosceminfile = LineFile.Make(mosceminFN.asfilename,#file_perm_write)
        '        If (mosceminfile = nil) Then
        '            MsgBox.info("Could not create file, " + mosceminFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'moscemparamfile = LineFile.Make(moscemparamFN.asfilename,#file_perm_write)
        '        If (moscemparamfile = nil) Then
        '            MsgBox.info("Could not create file, " + moscemparamFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'basinfile = LineFile.Make(basinFN.asfilename,#file_perm_read)
        '        If (basinfile = nil) Then
        '            MsgBox.info("Could not open file, " + basinFN + nl + "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        basinfile.close()

        'riverfile = LineFile.Make(riverFN.asfilename,#file_perm_read)
        '        If (riverfile = nil) Then
        '            MsgBox.info("Could not open file, " + riverFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        riverfile.close()

        '        If (File.Exists(origbasFN.AsFileName).not) Then
        '            If (File.Exists(basinFN.AsFileName)) Then
        '                File.Copy((basinFN.AsFileName), (origbasFN.AsFileName))
        '    origbasinfile = LineFile.Make(origbasFN.asfilename,#file_perm_read)
        '                If (origbasinfile = nil) Then
        '                    MsgBox.info("Could not open basin.txt or basin_original.txt" + nl + "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '            Else
        '                MsgBox.info("Could not open basin.txt or basin_original.txt" + nl + "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        'else
        '  origbasinfile = LineFile.Make(origbasFN.asfilename,#file_perm_read)
        '                If (origbasinfile = nil) Then
        '                    MsgBox.info("Could not open file, " + origbasFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '        End If

        '        If (File.Exists(origrivFN.AsFileName).not) Then
        '            If (File.Exists(riverFN.AsFileName)) Then
        '                File.Copy((riverFN.AsFileName), (origrivFN.AsFileName))
        '    origriverfile = LineFile.Make(origrivFN.asfilename,#file_perm_read)
        '                If (origriverfile = nil) Then
        '                    MsgBox.info("Could not open " + riverFN + " or " + origrivFN + nl + "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '            Else
        '                MsgBox.info("Could not open " + riverFN + " or " + origrivFN + nl + "File(s) may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        'else
        '  origriverfile = LineFile.Make(origrivFN.asfilename,#file_perm_read)
        '                If (origriverfile = nil) Then
        '                    MsgBox.info("Could not open file, " + origrivFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '        End If

        'streamflowfile = LineFile.Make(streamflowFN.asfilename,#file_perm_write)
        '        If (streamflowfile = nil) Then
        '            MsgBox.info("Could not create file, " + streamflowFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        streamflowfile.close()


        'postlabels = { "Output Time Series File",
        '          "Output Objective Convergence File",
        '          "Output Tradeoff Boundary File",
        '          "Processing Parameter File"}

        'postdefaults = { "timeseries.txt",
        '          "objectives_postproc.out",
        '          "trdoff_bounds.out",
        '          "postproc.in"}

        '        inpostList = MsgBox.MultiInput("Enter Post Processing Parameters.", "Geospatial Stream Flow Model", postlabels, postdefaults)

        '        If (inpostList.IsEmpty) Then
        '            Exit Sub
        '        End If

        '        timeseriesFN = myWkDirname + inpostList.get(0)
        '        objpostprocFN = myWkDirname + inpostList.get(1)
        '        trdoffboundFN = myWkDirname + inpostList.get(2)
        '        postprocinFN = myWkDirname + inpostList.get(3)


        'timeseriesfile = LineFile.Make((timeseriesFN).asfilename,#file_perm_write)
        '        If (timeseriesfile = nil) Then
        '            MsgBox.info("Could not create file, " + timeseriesFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        timeseriesfile.close()

        'objpostprocFile = LineFile.Make(objpostprocFN.asfilename,#file_perm_write)
        '        If (objpostprocFile = nil) Then
        '            MsgBox.info("Could not open " + objpostprocFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        objpostprocFile.close()

        'trdoffboundFile = LineFile.Make(trdoffboundFN.asfilename,#file_perm_write)
        '        If (trdoffboundFile = nil) Then
        '            MsgBox.info("Could not create " + trdoffboundFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        trdoffboundFile.close()

        'postprocinFile = LineFile.Make(postprocinFN.asfilename,#file_perm_write)
        '        If (postprocinFile = nil) Then
        '            MsgBox.info("Could not create " + postprocinFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        postprocinFile.close()


        '        If (File.Exists(whichmodelFN.AsFileName)) Then
        '  whichmodelfile = LineFile.Make((myWkDirname + "whichModel.txt").asfilename,#file_perm_read)
        '            If (whichmodelfile = nil) Then
        '                MsgBox.info("Could not create whichModel.txt file" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            whichsize = (whichmodelfile.getsize)

        '            biglist = List.make
        '            If (whichsize > 2) Then
        '                whichmodelfile.Read(biglist, whichsize)
        '                balancetype = biglist.get(1).asstring.astokens(" ,/").get(0).asstring
        '                routetype = biglist.get(2).asstring.astokens(" ,/").get(0).asstring
        '            Else
        '                balancetype = "1"
        '                routetype = "2"
        '            End If

        '            whichmodelfile.close()
        '            If ((balancetype = "1") And (routetype = "3")) Then
        '                balstr = "Linear Soil Model, Lag Routing"
        '            ElseIf ((balancetype = "1") And (routetype = "1")) Then
        '                balstr = "Linear Soil Model, Diffusion Routing"
        '            ElseIf ((balancetype = "1") And (routetype = "2")) Then
        '                balstr = "Linear Soil Model, Muskingum-Cunge Routing"
        '            ElseIf ((balancetype = "2") And (routetype = "3")) Then
        '                balstr = "Non-Linear Soil Model, Lag Routing"
        '            ElseIf ((balancetype = "2") And (routetype = "1")) Then
        '                balstr = "Non-Linear Soil Model, Diffusion Routing"
        '            Else
        '                balstr = "Non-Linear Soil Model, Muskingum-Cunge Routing"
        '            End If
        '  methodlst = {balstr, "Linear Soil Model, Muskingum-Cunge Routing", "Linear Soil Model, Diffusion Routing", "Linear Soil Model, Lag Routing", "Non-Linear Soil Model, Muskingum-Cunge Routing", "Non-Linear Soil Model, Diffusion Routing", "Non-Linear Soil Model, Lag Routing" }
        'else
        '  methodlst = {"Linear Soil Model, Muskingum-Cunge Routing", "Linear Soil Model, Diffusion Routing", "Linear Soil Model, Lag Routing", "Non-Linear Soil Model, Muskingum-Cunge Routing", "Non-Linear Soil Model, Diffusion Routing", "Non-Linear Soil Model, Lag Routing" }
        '        End If

        'whichmodelfile = LineFile.Make(whichmodelFN.asfilename,#file_perm_write)
        '        If (whichmodelfile = nil) Then
        '            MsgBox.info("Could not create file, " + whichmodelFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        methodtypestr = MsgBox.ChoiceAsString(methodlst, "Select Model Configuration", "Geospatial Stream Flow Model")
        '        If (methodtypestr = nil) Then
        '            Exit Sub
        '        End If
        'methodlst = {"Linear Soil Model, Muskingum-Cunge Routing", "Linear Soil Model, Diffusion Routing", "Linear Soil Model, Lag Routing", "Non-Linear Soil Model, Muskingum-Cunge Routing", "Non-Linear Soil Model, Diffusion Routing", "Non-Linear Soil Model, Lag Routing" }

        '        If (methodtypestr = "Linear Soil Model, Lag Routing") Then
        '            balancetype = "1"
        '            routetype = "3"
        '        ElseIf (methodtypestr = "Linear Soil Model, Diffusion Routing") Then
        '            balancetype = "1"
        '            routetype = "1"
        '        ElseIf (methodtypestr = "Linear Soil Model, Muskingum-Cunge Routing") Then
        '            balancetype = "1"
        '            routetype = "2"
        '        ElseIf (methodtypestr = "Non-Linear Soil Model, Lag Routing") Then
        '            balancetype = "2"
        '            routetype = "3"
        '        ElseIf (methodtypestr = "Non-Linear Soil Model, Diffusion Routing") Then
        '            balancetype = "2"
        '            routetype = "1"
        '        ElseIf (methodtypestr = "Non-Linear Soil Model, Muskingum-Cunge Routing") Then
        '            balancetype = "2"
        '            routetype = "2"
        '        Else
        '            balancetype = "1"
        '            routetype = "2"
        '        End If

        '        whichmodelfile.WriteElt("Model Index   Index description")
        '        whichmodelfile.WriteElt(balancetype + " //water balance model:  1=1D balance, 2=2D balance")
        '        whichmodelfile.WriteElt(routetype + " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        '        whichmodelfile.flush()

        '        logfile.WriteElt("Routing Models Selected as " + methodtypestr.asstring)


        '        minparlst = List.make
        '        maxparlst = List.make

        '        origbsize = (origbasinfile.getsize)
        '        bigblist = List.make

        '        'store the positions of calibered variables
        'magbposlst = {1,2,3,4,6,7,8,9,10,11,12,13,14}
        'magrposlst = {6,3,14,7,8,12,13}

        '        origbasinfile.Read(bigblist, origbsize)
        '        minparlst = List.make
        '        minparlst = List.make

        'for each beg in 0..19
        '            minparlst.add(0)
        '            maxparlst.add(0)
        '        Next

        '        headerblst = List.make
        'for each obrec in 1..(origbsize - 1)
        '            headerbstr = bigblist.get(obrec).asstring.astokens(",").get(0)
        '            headerblst.add(headerbstr)
        '            chkparblst = bigblist.get(obrec).asstring.astokens(",")
        '            numbrecs = chkparblst.count
        '  For each bbrec in 0..(magbposlst.count - 1)
        '                magbid = magbposlst.get(bbrec)
        '                chkbpar = chkparblst.get(magbid).asstring.asnumber
        '                If (chkbpar < minparlst.get(bbrec)) Then
        '                    minparlst.set(bbrec, chkbpar)
        '                End If
        '                If (chkbpar > maxparlst.get(bbrec)) Then
        '                    maxparlst.set(bbrec, chkbpar)
        '                End If
        '            Next
        '        Next
        '        origbasinfile.close()

        '        'numbrecs = chkparblst.count

        '        origrsize = (origriverfile.getsize)
        '        bigrlist = List.make
        '        origriverfile.Read(bigrlist, origrsize)

        '        headerlst = List.make
        'for each orec in 1..(origrsize - 1)
        '            headerstr = bigrlist.get(orec).asstring.astokens(",").get(0)
        '            headerlst.add(headerstr)
        '            chkparlst = bigrlist.get(orec).asstring.astokens(",")
        '            numrrecs = chkparlst.count
        '            bendpos = ((magbposlst.count) - 1)
        '  For each brrec in 0..(magrposlst.count - 1)
        '                magbid = magbposlst.get(brrec)
        '                chkrpar = chkparlst.get(magbid).asstring.asnumber
        '                If (chkrpar < minparlst.get(brrec + bendpos)) Then
        '                    minparlst.set(brrec + bendpos, chkrpar)
        '                End If
        '                If (chkrpar > maxparlst.get(brrec + bendpos)) Then
        '                    maxparlst.set(brrec + bendpos, chkrpar)
        '                End If
        '            Next
        '        Next
        '        origriverfile.close()

        '        logfile.WriteElt("River and Basin Files Read")

        '        If (observedfile.getsize > 1) Then
        '            'linecount = observedfile.ReadElt.asstring.astokens(",").count
        '            fluxlist = observedfile.ReadElt.asstring.astokens(",")
        '            linecount = fluxlist.count
        '            If (linecount > 1) Then
        '                fluxcount = linecount - 1
        '            Else
        '                MsgBox.info("Observed Streamflow file may be empty or badly formatted." + nl + "Ensure the file is comma delimited with one timestep per line.", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '        Else
        '            MsgBox.info("Observed Streamflow file may be empty or badly formatted." + nl + "Ensure the file is comma delimited with one timestep per line.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        nfluxlst = List.make
        '        nposlst = List.make

        'for each mrec in 1..(fluxlist.count - 1)
        '            nfluxstr = fluxlist.get(mrec).asstring
        '            nfluxlst.Add(nfluxstr)
        '            nposlst.Add("0")
        '        Next

        '        nfluxchoices = MsgBox.MultiListAsString(nfluxlst, "Select Observed Streamflow Stations for Calibration", "Which Stations Do You Want to Calibrate?")
        '        If (nfluxchoices = nil) Then
        '            Exit Sub
        '        End If

        '        nfcount = nfluxlst.count
        '        nccount = nfluxchoices.count
        '        nbasidlst = List.make
        '        nfluxcount = nccount

        'for each crec in 0..(nccount-1)
        '            ncurchoice = (nfluxchoices.get(crec)).asstring
        '  for each drec in 0..(nfcount-1)
        '                ncurdesc = (nfluxlst.get(drec)).asstring
        '                If (ncurchoice = ncurdesc) Then
        '                    nposlst.set(drec, "1")
        '                    'nfluxcount = nfluxcount + 1
        '                    idchoice = MsgBox.ChoiceAsString(headerlst, "Select BasinID for Streamflow Station " + ncurchoice, "Link River Reach to Gauging Station")
        '                    If (idchoice = nil) Then
        '                        Exit Sub
        '                    End If
        '                    nbasposition = (headerlst.FindByValue(idchoice))
        '                    nbasidlst.Add((nbasposition + 1).asstring)
        '                End If
        '            Next
        '        Next

        '        logfile.WriteElt("Observed Streamflow File Read")

        '        ncposstr = ""

        'For each frec in 1..(nccount.asstring.asnumber)
        '            ncid = (nfluxlst.FindByValue(nfluxchoices.get(frec - 1))) + 1
        '            If (ncid <> 0) Then

        '                If (frec = 1) Then
        '                    nstr = ncid.asstring
        '                    pnum = ncid
        '                    ncposstr = nstr
        '                Else
        '                    nstr = (ncid - pnum).asstring
        '                    pnum = ncid
        '                    ncposstr = ncposstr + ", " + nstr
        '                End If

        '                'objoptflagFile.WriteElt("runoff_" + ncid.asstring + "      " + nposlst.get(frec - 1).asstring)
        '                objoptflagFile.WriteElt("runoff_" + ncid.asstring + "      1")
        '            End If
        '        Next
        '        objoptflagFile.close()

        '        'paraminlst = list.make
        '        'paraminlst.Add("No,Name,Default,Lower,Upper,OptIdx,Description")
        '        'paraminlst.Add("1,SoilWhc,1,1,600,0,Soil Water Holding Capacity (mm)")
        '        'paraminlst.Add("2,Depth,1,1,800,0,Total soil depth (cm)")
        '        'paraminlst.Add("3,Texture,1,1,5,0,Soil 1=Sand 2=Loam 3=Clay 5=Water")
        '        'paraminlst.Add("4,Ks,1,0.001,150,0,Saturated hydraulic conductivity (cm/hr)")
        '        'paraminlst.Add("5,Interflow,1,1,365,0,Interflow storage residence time (days)")
        '        'paraminlst.Add("6,HSlope,1,0.001,10,0,Average subbasin slope") 
        '        'paraminlst.Add("7,Baseflow,1,1,365,0,Baseflow residence time (days)") 
        '        'paraminlst.Add("8,CurveNum,1,25,98,0,SCS runoff curve number")
        '        'paraminlst.Add("9,MaxCover,1,0.01,1,0,Fraction Impervious Cover")
        '        'paraminlst.Add("10,BasinLoss,1,0.001,0.5,0,Regional Ground Water Loss Fraction") 
        '        'paraminlst.Add("11,PanCoeff,1,0.6,0.95,0,Evapotranspiration Pan Coefficient")
        '        'paraminlst.Add("12,TopSoil,1,0.05,1,0,Hydrologically Active Top Soil Fraction")
        '        'paraminlst.Add("13,RainCalc,1,1,3,0, Saturation type 1=Philip 2=SCS 3=BucketModel")
        '        'paraminlst.Add("14,RivRough,1,0.012,0.07,0, Manning's n for Channel Roughness") 
        '        'paraminlst.Add("15,RivSlope,1,0.001,10,0,Average slope of the river")    
        '        'paraminlst.Add("16,RivWidth,1,30,1000,0,Average channel width (m)") 
        '        'paraminlst.Add("17,RivLoss,1,0.01,0.5,0,Channel Flow Loss Fraction")
        '        'paraminlst.Add("18,RivFPLoss,1,0.01,0.5,0,Floodplain Flow Loss Fraction")
        '        'paraminlst.Add("19,Celerity,1,0.1,5,0,Flood Wave Celerity (m/s)")
        '        'paraminlst.Add("20,Diffusion,1,100,10000,0,Flow Attenuation Coefficient (m^2/s)")
        '        'paraminlst.clone
        '        '
        '        'paraminlst1 = list.make
        '        ''paraminlst1.Add("No,Name,Default,Lower,Upper,OptIdx,Description")
        '        'paraminlst1.Add("1,SoilWhc,1,1,600,1,Soil Water Holding Capacity (mm)")
        '        'paraminlst1.Add("2,Depth,1,1,800,1,Total soil depth (cm)")
        '        'paraminlst1.Add("3,Texture,1,1,5,1,Soil 1=Sand 2=Loam 3=Clay 5=Water")
        '        'paraminlst1.Add("4,Ks,1,0.001,150,1,Saturated hydraulic conductivity (cm/hr)")
        '        'paraminlst1.Add("5,Interflow,1,1,365,1,Interflow storage residence time (days)")
        '        'paraminlst1.Add("6,HSlope,1,0.001,10,1,Average subbasin slope") 
        '        'paraminlst1.Add("7,Baseflow,1,1,365,1,Baseflow residence time (days)") 
        '        'paraminlst1.Add("8,CurveNum,1,25,98,1,SCS runoff curve number")
        '        'paraminlst1.Add("9,MaxCover,1,0.01,1,1,Fraction Impervious Cover")
        '        'paraminlst1.Add("10,BasinLoss,1,0.001,0.5,1,Regional Ground Water Loss Fraction") 
        '        'paraminlst1.Add("11,PanCoeff,1,0.6,0.95,1,Evapotranspiration Pan Coefficient")
        '        'paraminlst1.Add("12,TopSoil,1,0.05,1,1,Hydrologically Active Top Soil Fraction")
        '        'paraminlst1.Add("13,RainCalc,1,1,3,1, Saturation type 1=Philip 2=SCS 3=BucketModel")
        '        'paraminlst1.Add("14,RivRough,1,0.012,0.07,1, Manning's n for Channel Roughness") 
        '        'paraminlst1.Add("15,RivSlope,1,0.001,10,1,Average slope of the river")    
        '        'paraminlst1.Add("16,RivWidth,1,30,1000,1,Average channel width (m)") 
        '        'paraminlst1.Add("17,RivLoss,1,0.01,0.5,1,Channel Flow Loss Fraction")
        '        'paraminlst1.Add("18,RivFPLoss,1,0.01,0.5,1,Floodplain Flow Loss Fraction")
        '        'paraminlst1.Add("19,Celerity,1,0.1,5,1,Flood Wave Celerity (m/s)")
        '        'paraminlst1.Add("20,Diffusion,1,100,10000,1,Flow Attenuation Coefficient (m^2/s)")
        '        'paraminlst1.clone

        '        paraminlst = List.make
        '        paraminlst.Add("1,SoilWhc,1,0.1,5,0,Soil water holding capacity (mm)")
        '        paraminlst.Add("2,Depth,1,0.1,5,0,Total soil depth (cm)")
        '        paraminlst.Add("3,Texture,1,0.3,3,0,Soil texture 1=Sand 2=Loam 3=Clay 5=Water")
        '        paraminlst.Add("4,Ks,1,0.1,10,0,Saturated hydraulic conductivity (cm/hr)")
        '        paraminlst.Add("5,Interflow,1,0.1,5,0,Interflow storage residence time (days)")
        '        paraminlst.Add("6,HSlope,1,0.5,1.5,0,Average subbasin slope")
        '        paraminlst.Add("7,Baseflow,1,0.1,5,0,Baseflow reservoir residence time (days)")
        '        paraminlst.Add("8,CurveNum,1,0.1,1.5,0,SCS runoff curve number")
        '        paraminlst.Add("9,MaxCover,1,0.1,5,0,Fraction of the subbasin with permanently impervious cover")
        '        paraminlst.Add("10,BasinLoss,1,0.1,5,0,Fraction of soil water infiltrating to ground water")
        '        paraminlst.Add("11,PanCoeff,1,0.1,5,0,Pan coefficient for correcting PET readings")
        '        paraminlst.Add("12,TopSoil,1,0.1,5,0,Fraction of soil layer that is hydrologically active")
        '        paraminlst.Add("13,RainCalc,1,0.1,5,0,Excess rainfall mode 1=Philip 2=SCS 3=BucketModel")
        '        paraminlst.Add("14,RivRough,1,0.1,5,0,River Channel Roughness Coefficient (Manning n)")
        '        paraminlst.Add("15,RivSlope,1,0.1,5,0,Average slope of the river")
        '        paraminlst.Add("16,RivWidth,1,0.1,5,0,Average channel width (m)")
        '        paraminlst.Add("17,RivLoss,1,0.1,5,0,Fraction of flow lost within the river channel")
        '        paraminlst.Add("18,RivFPLoss,1,0.1,5,0,Fraction of the river flow lost in floodplain")
        '        paraminlst.Add("19,Celerity,1,0.1,5,0,Flood wave celerity (m/s)")
        '        paraminlst.Add("20,Diffusion,1,0.05,10,0,Flow attenuation coefficient (m^2/s)")
        '        paraminlst.clone()

        '        paraminlst1 = List.make
        '        paraminlst1.Add("1,SoilWhc,1,0.1,5,1,Soil water holding capacity (mm)")
        '        paraminlst1.Add("2,Depth,1,0.1,5,1,Total soil depth (cm)")
        '        paraminlst1.Add("3,Texture,1,0.3,3,1,Soil texture 1=Sand 2=Loam 3=Clay 5=Water")
        '        paraminlst1.Add("4,Ks,1,0.1,10,1,Saturated hydraulic conductivity (cm/hr)")
        '        paraminlst1.Add("5,Interflow,1,0.1,5,1,Interflow storage residence time (days)")
        '        paraminlst1.Add("6,HSlope,1,0.5,1.5,1,Average subbasin slope")
        '        paraminlst1.Add("7,Baseflow,1,0.1,5,1,Baseflow reservoir residence time (days)")
        '        paraminlst1.Add("8,CurveNum,1,0.1,1.5,1,SCS runoff curve number")
        '        paraminlst1.Add("9,MaxCover,1,0.1,5,1,Fraction of the subbasin with permanently impervious cover")
        '        paraminlst1.Add("10,BasinLoss,1,0.1,5,1,Fraction of soil water infiltrating to ground water")
        '        paraminlst1.Add("11,PanCoeff,1,0.1,5,1,Pan coefficient for correcting PET readings")
        '        paraminlst1.Add("12,TopSoil,1,0.1,5,1,Fraction of soil layer that is hydrologically active")
        '        paraminlst1.Add("13,RainCalc,1,0.1,5,1,Excess rainfall mode 1=Philip 2=SCS 3=BucketModel")
        '        paraminlst1.Add("14,RivRough,1,0.1,5,1,River Channel Roughness Coefficient (Manning n)")
        '        paraminlst1.Add("15,RivSlope,1,0.1,5,1,Average slope of the river")
        '        paraminlst1.Add("16,RivWidth,1,0.1,5,1,Average channel width (m)")
        '        paraminlst1.Add("17,RivLoss,1,0.1,5,1,Fraction of flow lost within the river channel")
        '        paraminlst1.Add("18,RivFPLoss,1,0.1,5,1,Fraction of the river flow lost in floodplain")
        '        paraminlst1.Add("19,Celerity,1,0.1,5,1,Flood wave celerity (m/s)")
        '        paraminlst1.Add("20,Diffusion,1,0.05,10,1,Flow attenuation coefficient (m^2/s)")
        '        paraminlst1.clone()

        '        'paraminlst1 = list.make
        '        'paraminlst1.Add("1,SoilWhc,1,0.01,7.29,1,Soil water holding capacity (mm)")
        '        'paraminlst1.Add("2,Depth,1,0.009,1.83,1,Total soil depth (cm)")
        '        'paraminlst1.Add("3,Texture,1,0.50,1.50,1,Soil texture 1=Sand 2=Loam 3=Clay 5=Water")
        '        'paraminlst1.Add("4,Ks,1,0.0003,37.7,1,Saturated hydraulic conductivity (cm/hr)")
        '        'paraminlst1.Add("5,Interflow,1,0.12,11.02,1,Interflow storage residence time (days)")
        '        'paraminlst1.Add("6,HSlope,1,0.01,1.03,1,Average subbasin slope") 
        '        'paraminlst1.Add("7,Baseflow,1,0.06,7.47,1,Baseflow reservoir residence time (days)") 
        '        'paraminlst1.Add("8,CurveNum,1,0.33,1.29,1,SCS runoff curve number")
        '        'paraminlst1.Add("9,MaxCover,1,5.6,168,1,Fraction of the subbasin with permanently impervious cover")
        '        'paraminlst1.Add("10,BasinLoss,1,0.1,1.01,1,Fraction of soil water infiltrating to ground water") 
        '        'paraminlst1.Add("11,PanCoeff,1,0.71,1.12,1,Pan coefficient for correcting PET readings")
        '        'paraminlst1.Add("12,TopSoil,1,0.25,5,1,Fraction of soil layer that is hydrologically active")
        '        'paraminlst1.Add("13,RainCalc,1,0.5,1.5,1,Excess rainfall mode 1=Philip 2=SCS 3=BucketModel")
        '        'paraminlst1.Add("14,RivRough,1,0.34,2,1,River Channel Roughness Coefficient (Manning n)") 
        '        'paraminlst1.Add("15,RivSlope,1,0.02,4.12,1,Average slope of the river")    
        '        'paraminlst1.Add("16,RivWidth,1,0.005,4.38,1,Average channel width (m)") 
        '        'paraminlst1.Add("17,RivLoss,1,0.1,1.01,1,Fraction of flow lost within the river channel")
        '        'paraminlst1.Add("18,RivFPLoss,1,0.11,1.09,1,Fraction of the river flow lost in floodplain")
        '        'paraminlst1.Add("19,Celerity,1,0.13,6.25,1,Flood wave celerity (m/s)")
        '        'paraminlst1.Add("20,Diffusion,1,0.07,6.67,1,Flow attenuation coefficient (m^2/s)")
        '        'paraminlst1.clone


        '        desclst = List.make
        'for each mrec in 0..(paraminlst.count - 1)
        '            ministr = paraminlst.get(mrec).asstring
        '            minilst = ministr.asstring.astokens(",")
        '            desclst.Add(((minilst.get(1)).asstring))
        '        Next

        '        choices = MsgBox.MultiListAsString(desclst, "Select Parameters to be Calibrated", "Which Parameters Do You Want to Calibrate?")
        '        If (choices = nil) Then
        '            Exit Sub
        '        End If

        '        dcount = desclst.count
        '        Ccount = Choices.count
        '        paramidlst = List.make

        'for each crec in 0..(Ccount-1)
        '            curchoice = (Choices.get(crec)).asstring
        '  for each drec in 0..(dcount-1)
        '                curdesc = (desclst.get(drec)).asstring
        '                If (curchoice = curdesc) Then
        '                    chglst = paraminlst1.get(drec)

        '                    lowval = minparlst.get(drec)
        '                    lowrng = chglst.astokens(",").get(3).asstring.asnumber
        '                    lowfrac = lowrng / lowval

        '                    highval = minparlst.get(drec)
        '                    highrng = chglst.astokens(",").get(4).asstring.asnumber
        '                    highfrac = highrng / highval

        '                    chglst.astokens(",").set(3, lowfrac)
        '                    chglst.astokens(",").set(4, highfrac)

        '                    paraminlst.Set(drec, chglst)
        '                    paramidlst.Add((((chglst.astokens(",").get(0)).asstring).asnumber + 1).asstring)
        '                End If
        '            Next
        '        Next

        '        paraminFile.WriteElt("No,Name,Default,Lower,Upper,OptIdx,Description")

        'for each drec in 0..(dcount-1)
        '            paraminFile.WriteElt(paraminlst.get(drec).asstring)
        '        Next
        '        paraminFile.flush()
        '        paraminFile.close()

        '        logfile.WriteElt("River and Basin Calibration Parameters Identified")
        '        ncomplex = 2
        '        'ncomplex = ( Ccount / 2 ).ceiling
        '        'if(ncomplex.asstring.isnumber.not) then
        '        '  ncomplex = 4
        '        'end

        '        nsamples = 2 * ncomplex * ccount
        '        If (nsamples < 10) Then
        '            nummult = (10 / (ncomplex * ccount)).ceiling
        '            nsamples = nummult * ncomplex * ccount
        '        ElseIf (nsamples > 200) Then
        '            nummult = (100 / (ncomplex * ccount)).floor
        '            nsamples = nummult * ncomplex * ccount
        '        End If

        'runlist = { (nsamples * Ccount * 10).asstring, (nsamples * Ccount * 4).asstring, (nsamples * Ccount * 8).asstring, (nsamples * Ccount * 16).asstring, (nsamples * Ccount * 32).asstring, (nsamples * Ccount * 64).asstring, (nsamples * Ccount * 128).asstring }

        '        runstr = MsgBox.ChoiceAsString(runlist, "Define a Maximum No. of Runs" + nl + "NOTE: Each run could take upto 1 minute!", "Geospatial Stream Flow Model")
        '        If (runstr = nil) Then
        '            Exit Sub
        '        End If

        '        mosceminfile.WriteElt((Ccount.asstring) + ",  nOptPar")
        '        mosceminfile.WriteElt(nfluxcount.asstring + ", nOptObj")
        '        mosceminfile.WriteElt(nsamples.asstring + ", nSamples")
        '        mosceminfile.WriteElt(ncomplex.asstring + ", nComplex")
        '        mosceminfile.WriteElt(runstr.asstring + ", nMaxDraw")
        '        mosceminfile.WriteElt(parameterFN)
        '        mosceminfile.WriteElt(objoptflagFN)
        '        mosceminfile.WriteElt(obsflowFN)
        '        mosceminfile.WriteElt(objectiveFN)
        '        mosceminfile.WriteElt(paramvalFN)
        '        mosceminfile.WriteElt(paramconFN)
        '        mosceminfile.WriteElt(balparamFN)
        '        mosceminfile.WriteElt(moscemparamFN)
        '        mosceminfile.WriteElt(whichModelFN)
        '        mosceminfile.WriteElt(origbasFN)
        '        mosceminfile.WriteElt(origrivFN)
        '        mosceminfile.WriteElt(basinFN)
        '        mosceminfile.WriteElt(riverFN)
        '        mosceminfile.WriteElt(streamflowFN)
        '        mosceminfile.WriteElt(balfilesfn)
        '        mosceminfile.WriteElt(routfilesfn)
        '        mosceminfile.flush()
        '        mosceminfile.close()

        'caliblst = {"Root Mean Square Error (RMSE)", "Standard Deviation (STD)", "Maximum Likelihood Error (MLE)", "Nash-Sutcliffe Efficiency(NSE)", "Number of Sign Changes (NSC)", "BIAS" }

        '        Calibtypestr = MsgBox.ChoiceAsString(caliblst, "Select Objective Function Type", "How Should Convergence Be Measured?")
        '        If (Calibtypestr = nil) Then
        '            Exit Sub
        '        End If

        '        calibposition = ((caliblst.FindByValue(Calibtypestr)) + 1)
        '        myrstr = ""
        'For each nrec in 0..(nfluxcount - 1 )
        '            rstrnum = nbasidlst.get(nrec).asstring.asnumber
        '            If (nrec = 0) Then
        '                rstr = rstrnum.asstring
        '                pnum = rstrnum
        '                myrstr = rstr
        '            Else
        '                rstr = (rstrnum - pnum).asstring
        '                pnum = rstrnum
        '                myrstr = myrstr + ", " + rstr
        '            End If
        '        Next

        '        logfile.WriteElt("Calibration Method set as " + Calibtypestr.asstring)

        '        moscemparamfile.WriteElt(nfluxcount.asstring + "  //nflux")
        '        moscemparamfile.WriteElt(routlist.get(0).asstring + "  //ntstep1")
        '        moscemparamfile.WriteElt((routlist.get(0).asnumber + routlist.get(7).asnumber).asstring + "  //ntstep2")
        '        moscemparamfile.WriteElt(calibposition.asstring + "  //obj_func")
        '        moscemparamfile.WriteElt("-9999  //missing value")
        '        moscemparamfile.WriteElt(ncposstr + "  //nflux_obs, column in observed_streamflow.txt to test (not including timestep)")
        '        moscemparamfile.WriteElt(myrstr + "  //nflux_model, column in streamflow.txt (not including timestep)")
        '        moscemparamfile.flush()

        '        observedfile.close()
        '        objectivesFile.close()
        '        paramvaluesFile.close()
        '        parconvergeFile.close()
        '        moscemparamfile.close()
        '        whichmodelfile.close()

        '        theDate = Date.Now
        '        theday = Date.Now.setformat("jj").asstring
        '        theyear = Date.Now.setformat("yyy").asstring
        '        logfile.writeelt("Starting Time:" + +theDate.Asstring)

        '        If (File.Exists((myWkDirname + "geosfmcalib.exe").AsFileName)) Then
        '            geosfmcalibFN = (myWkDirname + "geosfmcalib.exe").AsFileName
        '        ElseIf (File.Exists(("$AVEXT\geosfmcalib.exe").AsFileName)) Then
        '  ziplistfile = LineFile.Make((myWkDirname + "ziplist.bat").AsFileName, #FILE_PERM_WRITE)
        '            If (ziplistfile = nil) Then
        '                MsgBox.error("Cannot open output file: ziplist.bat" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            ziplistfile.WriteElt("Copy " + (("$AVEXT\geosfmcalib.exe").AsFileName).GetFullName + +myWkDirname + "geosfmcalib.exe")
        '            ziplistfile.flush()
        '            ziplistfile.close()

        '            System.ExecuteSynchronous("ziplist.bat")
        '            geosfmcalibFN = (myWkDirname + "geosfmcalib.exe").AsFileName
        '        Else
        '            MsgBox.info("Unable to locate the program file: geosfmcalib.exe " + nl + nl + "Install the programs in your ArcView/Ext32 or working directory.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        logfile.WriteElt("geosfmcalib.exe program copied to working directory")

        '        If (File.Exists((myWkDirname + "geosfm.dll").AsFileName)) Then
        '            geosfmdllFN = (myWkDirname + "geosfm.dll").AsFileName
        '        ElseIf (File.Exists(("$AVEXT\geosfm.dll").AsFileName)) Then
        '  ziplistfile = LineFile.Make((myWkDirname + "ziplist.bat").AsFileName, #FILE_PERM_WRITE)
        '            If (ziplistfile = nil) Then
        '                MsgBox.error("Cannot open output file: ziplist.bat" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            ziplistfile.WriteElt("Copy " + (("$AVEXT\geosfm.dll").AsFileName).GetFullName + +myWkDirname + "geosfm.dll")
        '            ziplistfile.flush()
        '            ziplistfile.close()

        '            System.ExecuteSynchronous("ziplist.bat")
        '            geosfmdllFN = (myWkDirname + "geosfm.dll").AsFileName
        '        Else
        '            MsgBox.info("Unable to locate the program file: geosfm.dll " + nl + nl + "Install the programs in your ArcView/Ext32 or working directory.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        logfile.WriteElt("geosfm.dll program copied to working directory")

        '        av.ShowMsg("Performing Model Calibration with " + runstr.asstring + " ........")

        'ziplistfile = LineFile.Make((myWkDirname + "ziplist.bat").AsFileName, #FILE_PERM_WRITE)
        '        If (ziplistfile = nil) Then
        '            MsgBox.error("Cannot open output file: ziplist.bat" + nl + "File may be open or tied up by another program", "GeoSFM Utilities")
        '            Exit Sub
        '        End If
        '        ziplistfile.WriteElt("geosfmcalib.exe " + mosceminFN)
        '        ziplistfile.flush()
        '        ziplistfile.close()

        '        logfile.WriteElt("Performing Model Calibration with " + runstr.asstring + " ........")

        '        System.ExecuteSynchronous("ziplist.bat")

        '        If (File.Exists(paramconFN.asfilename)) Then
        '  paramconfile = LineFile.Make(paramconFN.AsFileName, #FILE_PERM_READ)
        '            If (paramconfile = nil) Then
        '                MsgBox.error("Cannot open output file, " + paramconFN + nl + "File may be open or tied up by another program", "GeoSFM Utilities")
        '                Exit Sub
        '            End If
        '            pcfilesize = paramconfile.getsize
        '            paramconfile.close()
        '        End If

        '        If (pcfilesize > 1) Then
        '            logfile.WriteElt("Calibration Program Successfully Executed!")
        '        Else
        '            logfile.WriteElt("Problems encounted during calibration.")
        '            logfile.WriteElt("Output parameter convergence file, " + paramconFN + ", is empty.")
        '            logfile.close()
        '            MsgBox.info("Calibration problems encountered." + nl + "Output parameter convergence file, " + paramconFN + ", is empty", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        av.ClearMsg()

        '        av.ShowMsg("Performing Model PostProcessing........")

        'postprocinFile = LineFile.Make(postprocinFN.asfilename,#file_perm_write)
        '        If (postprocinFile = nil) Then
        '            MsgBox.info("Could not create " + postprocinFN + " file," + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        postprocinFile.WriteElt(nsamples.asstring + ", nSamples")
        '        postprocinFile.WriteElt(obsflowFN)
        '        postprocinFile.WriteElt(paramvalFN)
        '        postprocinFile.WriteElt(objectiveFN)
        '        postprocinFile.WriteElt(timeseriesFN)
        '        postprocinFile.WriteElt(objpostprocFN)
        '        postprocinFile.WriteElt(trdoffboundFN)
        '        postprocinFile.WriteElt(balparamFN)
        '        postprocinFile.WriteElt(moscemparamFN)
        '        postprocinFile.WriteElt(parameterFN)
        '        postprocinFile.WriteElt(basinFN)
        '        postprocinFile.WriteElt(riverFN)
        '        postprocinFile.WriteElt(origbasFN)
        '        postprocinFile.WriteElt(origrivFN)
        '        postprocinFile.WriteElt(balfilesfn)
        '        postprocinFile.WriteElt(routfilesfn)
        '        postprocinFile.WriteElt(whichModelFN)
        '        postprocinFile.flush()
        '        postprocinFile.close()

        '        logfile.WriteElt("Initiating Post Processing Program...")


        '        If (File.Exists((myWkDirname + "geosfmpost.exe").AsFileName)) Then
        '            geosfmpostFN = (myWkDirname + "geosfmpost.exe").AsFileName
        '        ElseIf (File.Exists(("$AVEXT\geosfmpost.exe").AsFileName)) Then
        '  ziplistfile = LineFile.Make((myWkDirname + "ziplist.bat").AsFileName, #FILE_PERM_WRITE)
        '            If (ziplistfile = nil) Then
        '                MsgBox.error("Cannot open output file: ziplist.bat" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            ziplistfile.WriteElt("Copy " + (("$AVEXT\geosfmpost.exe").AsFileName).GetFullName + +myWkDirname + "geosfmpost.exe")
        '            ziplistfile.flush()
        '            ziplistfile.close()

        '            System.ExecuteSynchronous("ziplist.bat")
        '            geosfmpostFN = (myWkDirname + "geosfmpost.exe").AsFileName
        '        Else
        '            MsgBox.info("Unable to locate the program file: geosfmpost.exe " + nl + nl + "Install the programs in your ArcView/Ext32 or working directory.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        'ziplistfile = LineFile.Make((myWkDirname + "ziplist.bat").AsFileName, #FILE_PERM_WRITE)
        '        If (ziplistfile = nil) Then
        '            MsgBox.error("Cannot open output file: ziplist.bat" + nl + "File may be open or tied up by another program", "GeoSFM Utilities")
        '            Exit Sub
        '        End If
        '        ziplistfile.WriteElt("geosfmpost.exe postproc.in")
        '        ziplistfile.flush()
        '        ziplistfile.close()

        '        logfile.WriteElt("Performing Model PostProcessing........")

        '        System.ExecuteSynchronous("ziplist.bat")

        '        If (File.Exists(timeseriesFN.asfilename)) Then
        '  timeseriesfile = LineFile.Make(timeseriesFN.AsFileName, #FILE_PERM_READ)
        '            If (timeseriesfile = nil) Then
        '                MsgBox.error("Cannot open output file, " + timeseriesFN + nl + "File may be open or tied up by another program", "GeoSFM Utilities")
        '                Exit Sub
        '            End If
        '            tsfilesize = timeseriesfile.getsize
        '            timeseriesfile.close()
        '        End If

        '        If (tsfilesize > 1) Then
        '            tempFN = (myWkDirname.asfilename.maketmp("tmpfile", "txt")).asstring
        '            File.Copy(timeseriesFN.asfilename, tempFN.asfilename)
        '            logfile.WriteElt("Post Processing Program Successfully Executed!")
        '        Else
        '            logfile.WriteElt("Problems encounted during post processing.")
        '            logfile.WriteElt("Output time series file, " + timeseriesFN + ", is empty.")
        '            logfile.close()
        '            MsgBox.info("Post processing problems encountered." + nl + "Output time series file, " + timeseriesFN + ", is empty", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        temptable = Vtab.make(tempFN.asfilename, False, False)
        '        If (temptable = nil) Then
        '            MsgBox.info("Could not open output file," + nl + timeseriesFN, "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        flowtable = temptable.export(timeseriesFN.AsFileName, Dtext, False)

        '        GUIName = "Table"
        '        flowtable.unlinkall()
        '        flowtable.unjoinall()
        '        flowtable.deactivate()
        '        flowtable = nil
        '        temptable.unlinkall()
        '        temptable.unjoinall()
        '        temptable.deactivate()
        '        temptable = nil
        '        File.Delete(tempFN.asfilename)

        '        oldout = theProject.finddoc("Timeseries.txt")
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If

        '        flowtable = Vtab.make(timeseriesFN.asfilename, False, False)
        '        t = Table.MakeWithGUI(flowtable, GUIName)
        '        t.SetName("Timeseries.txt")
        '        t.GetWin.Open()

        '        av.ClearStatus()
        '        av.clearmsg()
        '        theDate = Date.Now
        '        logfile.writeelt("Ending Time:" + +theDate.Asstring)
        '        logfile.WriteElt("Output time series file added to ArcView.")
        '        logfile.close()
        '        av.ClearMsg()

        '        MsgBox.info("Model Calibration Run Complete." + nl + "Results written to: " + nl + timeseriesFN, "Geospatial Stream Flow Model")
    End Sub

    Friend Sub BankFull()
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

        '        TheProject = av.GetProject
        '        theFileName = theProject.GetFileName
        '        If (theFileName = nil) Then
        '            av.Run("Project.SaveAs", nil)
        '            theFileName = theProject.GetFileName
        '            If (theFileName = nil) Then
        '                Exit Sub
        '            End If
        '        Else
        '            If (av.Run("Project.CheckForEdits", nil).Not) Then
        '                Return nil
        '            End If
        '            If (theProject.Save) Then
        '                av.ShowMsg("Project saved to '" + theFileName.GetBaseName + "'")
        '    if (System.GetOS = #SYSTEM_OS_MAC) then
        '                    Script.Make("MacClass.SetDocInfo(SELF, Project)").DoIt(theFileName)
        '                End If
        '            End If
        '        End If

        '        TheView = av.GetActiveDoc
        '        ViewChk = TheView.GetGUI
        '        If (ViewChk <> "View") Then
        '            MsgBox.error("Click on the View to make it 'active' before running this program", "")
        '            Exit Sub
        '        End If

        '        ' Check the working directory
        '        TheWkDir = TheProject.GetFileName.ReturnDir
        '        myWkDirname = MsgBox.Input("Specify your working directory", "Working Directory", TheWkDir.GetFullName)
        '        If (mywkDirName = nil) Then
        '            Exit Sub
        '        End If
        '        If (File.Exists(myWkDirname.AsFileName).not) Then
        '            MsgBox.Error("Cannot read directory " + myWkDirname, "Directory Specification Error")
        '            Exit Sub
        '        End If
        '        If (File.IsWritable(myWkDirname.AsFileName).not) Then
        '            MsgBox.Error(myWkDirname + +"is not writable.", "Directory Specification Error")
        '            Exit Sub
        '        End If
        '        TheProject.SetWorkDir(myWkDirname.AsFileName)


        '        If ((myWkDirname.contains("\").Not) And (myWkDirname.contains("/")) And (myWkDirname.right(1) <> "/")) Then
        '            myWkDirname = myWkDirname + "/"
        '        ElseIf ((myWkDirname.contains("\")) And (myWkDirname.right(1) <> "\")) Then
        '            myWkDirname = myWkDirname + "\"
        '        ElseIf ((myWkDirname.contains("\").Not) And (myWkDirname.contains("\").Not) And (myWkDirname.right(1) <> "/")) Then
        '            myWkDirname = myWkDirname + "/"
        '        End If


        '        theThemelist = theView.GetThemes
        '        mypolythmlist = List.make
        '        For Each thm In theThemelist
        '            If (thm.CanSelect) Then
        '                athmname = thm.getname
        '                If ((athmname.contains("basply")) Or (athmname.contains("basin"))) Then
        '                    mypolythmlist.add(thm)
        '                End If
        '            End If
        '        Next

        '        If (mypolythmlist.isempty) Then
        '            theTheme = MsgBox.Choice(theThemelist, "Select basin coverage/grid theme", "Basin Theme")
        '            If (theTheme = nil) Then
        '                Exit Sub
        '            End If
        '        Else
        '            theTheme = MsgBox.Choice(mypolythmlist, "Select basin coverage/grid theme", "Basin Theme")
        '            If (theTheme = nil) Then
        '                Exit Sub
        '            End If
        '        End If


        '        'CREATE A LIST OF INPUT LABELS
        'labels = { "Input Daily Time Series File", "Output Monthly Time Series File", "Output Annual Time Series File" } 

        'defaults = { "streamflow.txt", "monthlyflow.txt", "annualflow.txt" }

        '        inpList = MsgBox.MultiInput("Enter Model Parameters.", "GeoSFM Utilities", labels, defaults)
        '        If (inpList.IsEmpty) Then
        '            Exit Sub
        '        End If

        '        runoffFN = myWkDirname + inplist.get(0)
        '        monthlyFN = myWkDirname + inplist.get(1)
        '        annualFN = myWkDirname + inplist.get(2)

        '        oldout = theProject.finddoc(inplist.get(1).asstring)
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If
        '        oldout = theProject.finddoc(inplist.get(2).asstring)
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If

        'chkfile=LineFile.Make((myWkDirname+"times.txt").asfilename,#file_perm_write)
        '        If (chkfile = nil) Then
        '            MsgBox.info("Could not create time file, times.txt", "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'paramfile=LineFile.Make((myWkDirname+"statsparam.txt").asfilename,#file_perm_write)
        '        If (paramfile = nil) Then
        '            MsgBox.info("Could not create parameter file, statsparam.txt", "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'runofffile = LineFile.Make((runoffFN).asfilename,#file_perm_read)
        '        If (runofffile = nil) Then
        '            MsgBox.info("Could not open time series file," + nl + runoffFN, "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'monthlyfile = LineFile.Make((monthlyFN).asfilename,#file_perm_write)
        '        If (monthlyfile = nil) Then
        '            MsgBox.info("Could not create monthly time series file, " + monthlyFN, "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'annualfile = LineFile.Make((annualFN).asfilename,#file_perm_write)
        '        If (annualfile = nil) Then
        '            MsgBox.info("Could not create annual time series file, " + annualFN, "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'logfile = LineFile.Make(("logfilestats.txt").asfilename,#file_perm_write)
        '        If (logfile = nil) Then
        '            MsgBox.info("Could not create logfilestats.txt file", "GeoSFM Utilities")
        '            Exit Sub
        '        End If


        '        theDate = Date.Now
        '        theday = Date.Now.setformat("jj").asstring
        '        theyear = Date.Now.setformat("yyy").asstring

        '        chkfile.writeelt("Starting Time:" + +theDate.Asstring)

        '        'msgbox.info("The resdays "+resdays.asstring,"")

        '        ' READ IN THE INPUT ARRAYS FOR PRECIPITATION AND POTENTIAL EVAPORATION
        '        runofflist = List.make
        '        runoffsize = runofffile.getsize
        '        useyear = 1
        '        If (runoffsize = 0) Then
        '            MsgBox.info("The runofffile " + runoffFN + " is empty", "")
        '            Exit Sub
        '        ElseIf ((runoffsize < 3285) And (runoffsize > 280)) Then
        '            MsgBox.info("Less than 9 years of data supplied." + nl + NL + "Computing bankfull from monthly data...", "")
        '            useyear = 12
        '        ElseIf (runoffsize < 270) Then
        '            MsgBox.info("Less than 9 month of data supplied." + nl + NL + "Computing bankfull from daily data...", "")
        '            useyear = 365
        '        End If

        '        runofffile.setpos(0)
        '        runofffile.read(runofflist, runoffsize)
        '        dayonestr = (((((runofflist.get(1)).substitute(" ", ",")).astokens(",")).get(0)))
        '        enddaystr = (((((runofflist.get(runoffsize - 1)).substitute(" ", ",")).astokens(",")).get(0)))

        '        If (dayonestr.count = 7) Then
        '            startyear = dayonestr.left(4)
        '            If (startyear.isnumber.not) Then
        '                startyear = MsgBox.input("Enter 4 digit start year eg 1999", "GeoSFM Utilities", theYear)
        '            End If
        '            If ((startyear = nil) Or (startyear.isnumber.not)) Then
        '                MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf ((startyear.count) <> 4) Then
        '                MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '                Exit Sub
        '            End If

        '            startday = dayonestr.right(3)
        '            If (startday.isnumber.not) Then
        '                startday = MsgBox.input("Enter 3 digit start day eg 003", "GeoSFM Utilities", theday)
        '            End If
        '            If ((startday = nil) Or (startday.isnumber.not)) Then
        '                MsgBox.info("Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf (((startday.asnumber) > 366) Or ((startday.asnumber) < 1)) Then
        '                MsgBox.info("Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
        '                Exit Sub
        '            End If
        '        Else
        '            startyear = MsgBox.input("Enter 4 digit start year eg 1999", "GeoSFM Utilities", theYear)
        '            If ((startyear = nil) Or (startyear.isnumber.not)) Then
        '                MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf ((startyear.count) <> 4) Then
        '                MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '                Exit Sub
        '            End If

        '            startday = MsgBox.input("Enter 3 digit start day eg 003", "GeoSFM Utilities", theday)
        '            If ((startday = nil) Or (startday.isnumber.not)) Then
        '                MsgBox.info("Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf (((startday.asnumber) > 366) Or ((startday.asnumber) < 1)) Then
        '                MsgBox.info("Start day must be a 3 digit number from 1 to 366", "GeoSFM Utilities")
        '                Exit Sub
        '            End If
        '        End If

        '        startjdate = Date.Make(dayonestr, "yyyjj")
        '        startmonth = startjdate.GetMonthOfYear.asstring
        '        startdayofmonth = startjdate.GetDayOfMonth.asstring

        '        If (enddaystr.count = 7) Then
        '            endyear = enddaystr.left(4)
        '            If (endyear.isnumber.not) Then
        '                endyear = MsgBox.input("Enter 4 digit end year eg 1999", "GeoSFM Utilities", theYear)
        '            End If
        '        Else
        '            endyear = MsgBox.input("Enter 4 digit end year eg 1999", "GeoSFM Utilities", theYear)
        '        End If
        '        If ((endyear = nil) Or (endyear.isnumber.not)) Then
        '            MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '            Exit Sub
        '        ElseIf ((endyear.count) <> 4) Then
        '            MsgBox.info("Start year must be a 4 digit number", "GeoSFM Utilities")
        '            Exit Sub
        '        ElseIf ((endyear.asnumber) < startyear.asnumber) Then
        '            MsgBox.info("End year must be greater than or equal to start year", "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        '        'statslist = { "Max" , "Mean"}
        '        'statstype = msgbox.choiceasstring(statslist, "Select Statistic to be Computed" , "GeoSFM Utilities" )
        '        'if(statstype = nil ) then
        '        '  exit    
        '        'end
        '        statstype = "Max"

        '        runoffdays = (runoffsize - 1)
        '        basinsize = (((((runofflist.get(1)).substitute(" ", ",")).astokens(",")).count) - 1).asstring
        '        numofyears = ((endyear.asnumber - startyear.asnumber) + 1)

        '        paramfile.writeelt(startyear.asstring)
        '        paramfile.writeelt(startmonth.asstring)
        '        paramfile.writeelt(startdayofmonth.asstring)
        '        paramfile.writeelt(endyear.asstring)
        '        paramfile.writeelt(runoffdays.asstring)
        '        paramfile.writeelt(basinsize.asstring)
        '        paramfile.writeelt(statstype.asstring)
        '        paramfile.writeelt(runoffFN)
        '        paramfile.writeelt(monthlyFN)
        '        paramfile.writeelt(annualFN)

        '        paramfile.close()
        '        runofffile.close()
        '        logfile.close()
        '        monthlyfile.close()
        '        annualfile.close()

        '        av.ShowStopButton()
        '        av.SetStatus(0)
        '        av.showmsg("Computing Monthly and Annual Fluxes.......")
        '        '
        '        'myfilename = ("$AVEXT\geosfmstats.dll").AsFileName 
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

        '        myrun = 0

        '        myfilename = ("$AVEXT\geosfmstats.dll").AsFileName
        '        myfilename2 = ("$AVEXT\geosfmstats.exe").AsFileName

        '        If (File.Exists(("$AVEXT\geosfmstats.dll").AsFileName)) Then
        '            mydll = DLL.Make(myfilename)
        '            If (mydll <> Nil) Then
        '    myproc=DLLProc.Make(myDLL, "aggregateflows", #DLLPROC_TYPE_INT32,{#DLLPROC_TYPE_VOID})
        '                If (myproc <> Nil) Then
        '                    myrun = 1
        '      procGetLastError=myproc.call({})
        '                End If
        '            End If
        '        End If

        '        If (myrun = 0) Then
        '            If (File.Exists(("$AVEXT\geosfmstats.exe").AsFileName)) Then
        '    ziplistfile = LineFile.Make((myWkDirname + "statslist.bat").AsFileName, #FILE_PERM_WRITE)
        '                If (ziplistfile = nil) Then
        '                    MsgBox.error("Cannot open output file: statslist.bat" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '                ziplistfile.WriteElt("Copy " + (("$AVEXT\geosfmstats.exe").AsFileName).GetFullName + +myWkDirname + "geosfmstats.exe")
        '                ziplistfile.flush()
        '                ziplistfile.close()
        '                System.ExecuteSynchronous("statslist.bat")

        '    ziplistfile = LineFile.Make((myWkDirname + "statslist.bat").AsFileName, #FILE_PERM_WRITE)
        '                If (ziplistfile = nil) Then
        '                    MsgBox.error("Cannot open output file: statslist.bat" + nl + "File may be open or tied up by another program", "GeoSFM Utilities")
        '                    Exit Sub
        '                End If
        '                ziplistfile.WriteElt("geosfmstats.exe")
        '                ziplistfile.flush()
        '                ziplistfile.close()
        '                System.ExecuteSynchronous("statslist.bat")
        '            ElseIf (File.Exists(("$AVEXT\geosfmstats.exe").AsFileName).NOT) Then
        '                MsgBox.info("Unable to locate the program file: geosfmstats.exe " + nl + nl + "Install the programs in your ArcView/Ext32 or working directory.", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            ElseIf (myfilename = Nil) Then
        '                MsgBox.info("Unable to locate the program file: geosfmstats.dll." + nl + nl + "Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf (mydll = Nil) Then
        '                MsgBox.info("Unable to access the dll, geosfmstats.dll" + nl + nl + "Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities")
        '                Exit Sub
        '            ElseIf (myproc = Nil) Then
        '                MsgBox.info("Unable to make the procedure, aggregateflows" + nl + nl + "Please install the program, geosfmstats.dll, before you continue.", "GeoSFM Utilities")
        '                Exit Sub
        '            End If
        '        End If


        'monthlyfile = LineFile.Make((monthlyFN).asfilename,#file_perm_read)
        '        If (monthlyfile = nil) Then
        '            MsgBox.info("Could not open output monthly time series file," + nl + monthlyFN, "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        'annualfile = LineFile.Make((annualFN).asfilename,#file_perm_read)
        '        If (annualfile = nil) Then
        '            MsgBox.info("Could not open output annual time series file," + nl + annualFN, "GeoSFM Utilities")
        '            Exit Sub
        '        End If

        '        GUIName = "Table"
        '        monthlyfile.close()
        '        annualfile.close()

        '        doMore = av.SetWorkingStatus
        '        If (doMore = False) Then
        '            Exit Sub
        '        End If

        '        oldout = theProject.finddoc(monthlyFN)
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If

        '        oldout = theProject.finddoc(annualFN)
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If

        '        monthlyfilename = monthlyFN.asfilename
        '        monthlytable = Vtab.make(monthlyFN.asfilename, False, False)
        '        tm = Table.MakeWithGUI(monthlytable, GUIName)
        '        tm.SetName(monthlyFN.asfilename.getbasename.asstring)

        '        annualfilename = annualFN.asfilename
        '        annualfile = Vtab.make(annualfilename, False, False)
        '        annualTbl = Table.MakeWithGUI(annualfile, GUIName)
        '        annualTbl.SetName(annualfilename.getbasename.asstring)

        '        dailyfilename = runoffFN.asfilename

        '        If (useyear = 365) Then
        '            resultfilename = dailyfilename
        '        ElseIf (useyear = 12) Then
        '            resultfilename = monthlyfilename
        '        Else
        '            resultfilename = annualfilename
        '        End If
        '        resultfile = Vtab.make(resultfilename, False, False)

        '        av.ClearStatus()
        '        av.clearmsg()

        '        subname = (resultfilename.AsString.Substitute(".txt", ".dbf")).AsFileName
        '        newVtab = resultfile.export(subname, DBase, False)
        '        newTable = Table.Make(newVtab)

        '        'select parameter to update
        '        chksum = 1
        '        chkcnt = 1
        '        chkavg = 1
        '        chkmax = 1
        '        chkmin = 1
        '        chkrng = 1
        '        chkvar = 1
        '        chkstd = 1
        '        chkbkf = 1
        '        chkmed = 1
        '        chkq25 = 1
        '        chkq33 = 1
        '        chkq66 = 1
        '        chkq75 = 1

        '        updatethr = True

        '        If (TheTheme.CanSelect) Then
        '            theftheme = theTheme
        '            theftab = theTheme.GetFtab
        '            shpfld = theFTab.FindField("Shape")
        '            theFTab.CreateIndex(shpfld)
        '        Else
        '            p = False
        '            def = av.GetProject.MakeFileName("theme", "shp")
        '            def = FileDialog.Put(def, "*.shp", "Convert " + TheTheme.getName)
        '            If (def = NIL) Then
        '                Exit Sub
        '            End If
        '            theFTab = TheTheme.ExportToFtab(def)
        '            ' For Database themes, which can return a nil FTab sometimes 
        '            If (theFTab = nil) Then
        '    MsgBox.Warning("Error occurred while converting to shapefile."+NL+
        '        "Shapefile was not created.", "Convert " + TheTheme.getName)
        '                Exit Sub
        '            End If
        '            shpfld = theFTab.FindField("Shape")
        '            ' build the spatial index
        '            theFTab.CreateIndex(shpfld)
        '            theftheme = FTheme.Make(theFTab)
        '            theView.AddTheme(theftheme)
        '            '  theView.GetWin.Activate
        '            End If

        '            theFtab.SetEditable(True)
        '            addlist = List.Make

        '            sumfld = theFtab.FindField("Sum")
        '            If (sumfld = nil) Then
        '  fldadd = Field.Make("Sum", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            countfld = theFtab.FindField("Count")
        '            If (Countfld = nil) Then
        '  fldadd = Field.Make("Count", #field_decimal, 10 , 1)  
        '                addlist.Add(fldadd)
        '            End If
        '            meanfld = theFtab.FindField("Mean")
        '            If (Meanfld = nil) Then
        '  fldadd = Field.Make("Mean", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            maxfld = theFtab.FindField("Maximum")
        '            If (Maxfld = nil) Then
        '  fldadd = Field.Make("Maximum", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            minfld = theFtab.FindField("Minimum")
        '            If (Minfld = nil) Then
        '  fldadd = Field.Make("Minimum", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            rangefld = theFtab.FindField("Range")
        '            If (Rangefld = nil) Then
        '  fldadd = Field.Make("Range", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            varfld = theFtab.FindField("Variance")
        '            If (Varfld = nil) Then
        '  fldadd = Field.Make("Variance", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            stddevfld = theFtab.FindField("Stddev")
        '            If (Stddevfld = nil) Then
        '  fldadd = Field.Make("Stddev", #field_decimal, 10 , 1)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            medianfld = theFtab.FindField("Median")
        '            If (medianfld = nil) Then
        '  fldadd = Field.Make("Median", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            Quart25fld = theFtab.FindField("Quart25")
        '            If (Quart25fld = nil) Then
        '  fldadd = Field.Make("Quart25", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            Quart75fld = theFtab.FindField("Quart75")
        '            If (Quart75fld = nil) Then
        '  fldadd = Field.Make("Quart75", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            Quart33fld = theFtab.FindField("Quart33")
        '            If (Quart33fld = nil) Then
        '  fldadd = Field.Make("Quart33", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            Quart66fld = theFtab.FindField("Quart66")
        '            If (Quart66fld = nil) Then
        '  fldadd = Field.Make("Quart66", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            Highflowfld = theFtab.FindField("Highflow")
        '            If (Highflowfld = nil) Then
        '  fldadd = Field.Make("Highflow", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            lowflowfld = theFtab.FindField("Lowflow")
        '            If (lowflowfld = nil) Then
        '  fldadd = Field.Make("Lowflow", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()
        '            medflowfld = theFtab.FindField("Medflow")
        '            If (medflowfld = nil) Then
        '  fldadd = Field.Make("Medflow", #field_decimal, 10 , 3)
        '                addlist.Add(fldadd)
        '            End If
        '            addlist.DeepClone()

        '            theFtab.Addfields(addlist)
        '            sumfld = theFtab.FindField("Sum")
        '            meanfld = theFtab.FindField("Mean")
        '            maxfld = theFtab.FindField("Maximum")
        '            minfld = theFtab.FindField("Minimum")
        '            stddevfld = theFtab.FindField("StdDev")
        '            varfld = theFtab.FindField("Variance")
        '            rangefld = theFtab.FindField("Range")
        '            countfld = theFtab.FindField("Count")
        '            medianfld = theFtab.FindField("Median")
        '            Quart25fld = theFtab.FindField("Quart25")
        '            Quart75fld = theFtab.FindField("Quart75")
        '            Quart33fld = theFtab.FindField("Quart33")
        '            Quart66fld = theFtab.FindField("Quart66")
        '            Highflowfld = theFtab.FindField("Highflow")
        '            lowflowfld = theFtab.FindField("Lowflow")
        '            Medflowfld = theFtab.FindField("Medflow")

        '            theFields = resultfile.GetFields
        '            theFields.clone()
        '            pidfield = theFtab.FindField("Gridcode")
        '            polyidfld = theFtab.FindField("Id")
        '            pnumrecs = theFtab.GetNumRecords
        '            pidlist = List.Make
        'For each prec in 0..(pnumrecs-1)
        '                pidval = theFtab.ReturnValue(pidfield, prec)
        '                pidlist.Add(pidval)
        '        Next

        '        numflds = theFields.count
        '        numrecs = resultfile.GetNumRecords
        '        If (numflds < 2) Then
        '            MsgBox.error("Incorrect Textfile Format. Contains less than 2 fields." + nl + "Make sure input textfile is comma delimited.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        theSum = 0
        '        theCount = 0
        '        themean = 0
        '        theMinimum = 0
        '        theMaximum = 0
        '        theRange = 0
        '        theStdDev = 0
        '        theVariance = 0
        '        theMedian = 0
        '        theQuart25 = 0
        '        theQuart75 = 0
        '        theQuart33 = 0
        '        theQuart66 = 0

        '        av.UseWaitCursor()
        '        av.ShowStopButton()
        '        av.ShowMsg("Computing Statistics..........")
        '        tempcount = 0
        'for each mynum in 0..(numflds-1)
        '            theSum = 0
        '            theCount = 0
        '            theMinimum = nil
        '            theMaximum = nil

        '            av.ShowMsg("Processing Basin " + (mynum + 1).asstring + " of " + numflds.asstring + " .........")
        '            progress = (mynum / (numflds - 1)) * 100
        '            doMore = av.SetStatus(progress)
        '            If (Not doMore) Then
        '                av.ClearWorkingStatus()
        '                av.clearstatus()
        '                Exit Sub
        '            End If

        '            fld = theFields.Get(mynum)
        '            If ((fld.IsTypeNumber) And (fld.GetName.IsNumber)) Then
        '    reclist = {} 
        '    For each rec in 0..(numrecs-1)     
        '                    theValue = resultfile.ReturnValueNumber(Fld, rec)
        '                    If (Not (theValue.IsNull)) Then
        '                        reclist.Add(theValue)
        '                        theSum = theValue + theSum
        '                        theCount = theCount + 1
        '                    End If
        '                Next

        '                theMean = theSum / theCount

        '                reclist.clone()
        '                reclist.Sort(True)
        '                recCount = reclist.count

        '                theMinimum = reclist.get(0)
        '                theMaximum = reclist.get(recCount - 1)
        '                theRange = (theMaximum - theMinimum)

        '                If ((chkq25 = 1) Or (chkq33 = 1) Or (chkq66 = 1) Or (chkq75 = 1) Or (chkmed = 1) Or (chkbkf = 1)) Then

        '                    If (chkmed = 1) Then
        '                        If (recCount < 9) Then
        '                            MsgBox.info("Too few records for flow statistics computation", "Less than 9 records")
        '                            Exit Sub
        '                        ElseIf (((recCount) Mod (2)) = 1) Then
        '                            theMedian = reclist.get((recCount - 1) / 2)
        '                        ElseIf (((recCount) Mod (2)) = 0) Then
        '                            theMedian = (((reclist.get((recCount - 2) / 2)) + (reclist.get((recCount) / 2))) / 2)
        '                        End If
        '                    End If

        '                    If ((chkq25 = 1) Or (chkq75 = 1)) Then
        '                        If (recCount < 9) Then
        '                            MsgBox.info("Too few records for flow statistics computation", "Less than 9 records")
        '                            Exit Sub
        '                        ElseIf (((recCount) Mod (2)) = 1) Then
        '                            If (((recCount) Mod (4)) = 1) Then
        '                                theQuart25 = reclist.get((recCount - 1) / 4)
        '                                theQuart75 = reclist.get((recCount - 1) * 3 / 4)
        '                            ElseIf (((recCount) Mod (4)) = 3) Then
        '                                theQuart25 = (((reclist.get((recCount + 1) / 4)) + (reclist.get((recCount - 3) / 4))) / 2)
        '                                theQuart75 = (((reclist.get((recCount + 1) * 3 / 4)) + (reclist.get(((recCount + 1) * 3 / 4) - 1))) / 2)
        '                            End If
        '                        ElseIf (((recCount) Mod (2)) = 0) Then
        '                            If (((recCount) Mod (4)) = 2) Then
        '                                theQuart25 = reclist.get((recCount - 2) / 4)
        '                                theQuart75 = reclist.get((recCount - 2) * 3 / 4)
        '                            ElseIf (((recCount) Mod (4)) = 0) Then
        '                                theQuart25 = (((reclist.get((recCount - 4) / 4)) + (reclist.get((recCount) / 4))) / 2)
        '                                theQuart75 = (((reclist.get((recCount * 3 / 4) - 1)) + (reclist.get((recCount) * 3 / 4))) / 2)
        '                            End If
        '                        End If
        '                    End If

        '                    If ((chkq33 = 1) Or (chkq66 = 1)) Then
        '                        If ((((recCount) Mod (3)) = 0) And (recCount >= 9)) Then
        '                            theQuart33 = (((reclist.get((recCount) / 3)) * 2 / 3) + ((reclist.get((recCount - 3) / 3)) * 1 / 3))
        '                            theQuart66 = (((reclist.get((recCount) * 2 / 3)) * 1 / 3) + ((reclist.get((recCount - 1.5) * 2 / 3)) * 2 / 3))
        '                        ElseIf ((((recCount) Mod (3)) = 1) And (recCount >= 9)) Then
        '                            theQuart33 = (reclist.get((recCount - 1) / 3))
        '                            theQuart66 = (reclist.get((recCount - 4) * 2 / 3))
        '                        ElseIf ((((recCount) Mod (3)) = 2) And (recCount >= 9)) Then
        '                            theQuart33 = (((reclist.get((recCount - 2) / 3)) * 2 / 3) + ((reclist.get((recCount + 1) / 3)) * 1 / 3))
        '                            theQuart66 = (((reclist.get((recCount - 2) * 2 / 3)) * 2 / 3) + ((reclist.get(((recCount - 0.5) * 2 / 3) + 1)) * 1 / 3))
        '                        Else
        '                            theQuart33 = 0
        '                            theQuart66 = 0
        '                        End If
        '                    End If
        '                End If

        '                If ((chkstd = 1) Or (chkvar = 1)) Then
        '                    theSumSqDev = 0
        '      for each rec in 0..(numrecs-1)
        '                        theValue = resultfile.ReturnValueNumber(Fld, rec)
        '                        If (Not (theValue.IsNull)) Then
        '                            theSqDev = (theValue - theMean) * (theValue - theMean)
        '                            theSumSqDev = theSqDev + theSumSqDev
        '                        End If
        '                    Next

        '                    If (theCount > 1) Then
        '                        theVariance = theSumsqdev / (theCount - 1)
        '                        theStdDev = theVariance.Sqrt
        '                    Else
        '                        theVariance = 0
        '                        theStdDev = 0
        '                    End If
        '                End If

        '                rid = fld.AsString.AsNumber

        '                For Each precs In theFtab
        '                    cpid = theFtab.ReturnValue(pidfield, precs)
        '                    If (cpid = rid) Then
        '                        If (chksum = 1) Then
        '                            theFtab.SetValue(sumfld, precs, theSum)
        '                        End If
        '                        If (chkcnt = 1) Then
        '                            theFtab.SetValue(countfld, precs, theCount)
        '                        End If
        '                        If (chkavg = 1) Then
        '                            theFtab.SetValue(meanfld, precs, themean)
        '                        End If
        '                        If (chkmin = 1) Then
        '                            theFtab.SetValue(minfld, precs, theMinimum)
        '                        End If
        '                        If (chkmax = 1) Then
        '                            theFtab.SetValue(maxfld, precs, theMaximum)
        '                        End If
        '                        If (chkrng = 1) Then
        '                            theFtab.SetValue(rangefld, precs, theRange)
        '                        End If
        '                        If (chkstd = 1) Then
        '                            theFtab.SetValue(stddevfld, precs, theStdDev)
        '                        End If
        '                        If (chkvar = 1) Then
        '                            theFtab.SetValue(varfld, precs, theVariance)
        '                        End If
        '                        If (chkmed = 1) Then
        '                            theFtab.SetValue(medianfld, precs, theMedian)
        '                        End If
        '                        If (chkq25 = 1) Then
        '                            theFtab.SetValue(Quart25fld, precs, theQuart25)
        '                        End If
        '                        If (chkq33 = 1) Then
        '                            theFtab.SetValue(Quart33fld, precs, theQuart33)
        '                        End If
        '                        If (chkq66 = 1) Then
        '                            TheFtab.SetValue(Quart66fld, precs, theQuart66)
        '                        End If
        '                        If (chkq75 = 1) Then
        '                            theFtab.SetValue(Quart75fld, precs, theQuart75)
        '                        End If

        '                        If (updatethr = True) Then
        '                            mymean = theFtab.ReturnValue(meanfld, precs)
        '                            mymax = theFtab.ReturnValue(maxfld, precs)
        '                            mymin = theFtab.ReturnValue(minfld, precs)
        '                            myhigh = (mymean * mymax).sqrt
        '                            mylow = mymean.sqrt
        '                            theFtab.SetValue(Highflowfld, precs, myhigh)
        '                            theFtab.SetValue(Medflowfld, precs, mymean)
        '                            theFtab.SetValue(Lowflowfld, precs, mylow)
        '                        End If
        '                    End If
        '                Next

        '            End If
        '        Next

        '        choicestrg = "Median"

        'mylegend = legend.make(#SYMBOL_FILL)
        'mylegend.setlegendtype(#LEGEND_TYPE_COLOR)
        '        mylegend.Interval(theFtheme, choicestrg, 5)

        'myColorRamp = SymbolList.GetPreDefined(#SYMLIST_TYPE_COLORRAMP).Get(25)
        '        mylegend.GetSymbols.RampSavedColors(myColorRamp)

        '        mylegend.Save(filename.Merge(myWkDirname, "flow.avl"))
        '        theFtheme.SetLegend(mylegend)
        '        theFtheme.UpdateLegend()

        '        theFtab.SetEditable(False)
        '        theFtheme.SetVisible(True)
        '        av.ClearWorkingStatus()
        '        av.clearstatus()

        '        oldout = theProject.finddoc("riverstats.txt")
        '        If (oldout <> nil) Then
        '            theproject.removedoc(oldout)
        '        End If

        '        theFtab.Export("riverstats.txt".asFileName, DText, False)

        '        bankfullVtab = Vtab.make("riverstats.txt".asFileName, False, False)
        '        bankfulltable = Table.MakeWithGUI(bankfullVtab, GUIName)
        '        bankfulltable.SetName("riverstats.txt".asFileName.getbasename.asstring)
        '        bankfulltable.GetWin.Open()

        '        MsgBox.Info("Bankfull and Flow Characteristics Computed. Outputs written to: " + nl + nl + "      " + mywkDirname + "riverstats.txt", "Geospatial Stream Flow Model")

    End Sub

    Friend Sub PlotMap()
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

        '        TheProject = av.GetProject
        '        theFileName = theProject.GetFileName
        '        If (theFileName = nil) Then
        '            av.Run("Project.SaveAs", nil)
        '            theFileName = theProject.GetFileName
        '            If (theFileName = nil) Then
        '                Exit Sub
        '            End If
        '        Else
        '            If (av.Run("Project.CheckForEdits", nil).Not) Then
        '                Return nil
        '            End If
        '            If (theProject.Save) Then
        '                av.ShowMsg("Project saved to '" + theFileName.GetBaseName + "'")
        '    if (System.GetOS = #SYSTEM_OS_MAC) then
        '                    Script.Make("MacClass.SetDocInfo(SELF, Project)").DoIt(theFileName)
        '                End If
        '            End If
        '        End If

        '        TheView = av.GetActiveDoc
        '        ViewChk = TheView.GetGUI
        '        If (ViewChk <> "View") Then
        '            MsgBox.error("Click on the View to make it 'active' before running this program", "")
        '            Exit Sub
        '        End If

        '        ' Check the working directory
        '        TheWkDir = TheProject.GetFileName.ReturnDir
        '        myWkDirname = MsgBox.Input("Specify your working directory", "Working Directory", TheWkDir.GetFullName)
        '        If (mywkDirName = nil) Then
        '            Exit Sub
        '        End If
        '        If (File.Exists(myWkDirname.AsFileName).not) Then
        '            MsgBox.Error("Cannot read directory " + myWkDirname, "Directory Specification Error")
        '            Exit Sub
        '        End If
        '        If (File.IsWritable(myWkDirname.AsFileName).not) Then
        '            MsgBox.Error(myWkDirname + +"is not writable.", "Directory Specification Error")
        '            Exit Sub
        '        End If
        '        TheProject.SetWorkDir(myWkDirname.AsFileName)


        '        If ((myWkDirname.contains("\").Not) And (myWkDirname.contains("/")) And (myWkDirname.right(1) <> "/")) Then
        '            myWkDirname = myWkDirname + "/"
        '        ElseIf ((myWkDirname.contains("\")) And (myWkDirname.right(1) <> "\")) Then
        '            myWkDirname = myWkDirname + "\"
        '        ElseIf ((myWkDirname.contains("\").Not) And (myWkDirname.contains("\").Not) And (myWkDirname.right(1) <> "/")) Then
        '            myWkDirname = myWkDirname + "/"
        '        End If

        '        newtablelist = List.make
        '        sflowFN = theProject.Finddoc("streamflow.txt")
        '        If (sflowFN <> nil) Then
        '            newtablelist.add(sflowFN)
        '        End If

        '        soilwFN = theProject.Finddoc("soilwater.txt")
        '        If (soilwFN <> nil) Then
        '            newtablelist.add(soilwFN)
        '        End If

        '        If (newtablelist.isempty) Then
        '            resultfilename = FileDialog.Show("*.txt", "Textfile", "Select Input Data File")
        '            If (resultfilename = nil) Then
        '                Exit Sub
        '            End If
        '        Else
        '            resulttablename = MsgBox.Choice(newtablelist, "Select Input Data File", "Input Flow Time Series")
        '            If (resulttablename = nil) Then
        '                resultfilename = FileDialog.Show("*.txt", "Textfile", "Select Input Data File")
        '                If (resultfilename = nil) Then
        '                    Exit Sub
        '                End If
        '            Else
        '                resultfilename = resulttablename.getVtab.getname.asfilename
        '            End If
        '        End If

        '        resultfile = Vtab.Make(resultfilename, False, False)

        '        theThemelist = theView.GetThemes
        '        mypolythmlist = List.make
        '        For Each thm In theThemelist
        '            If (thm.CanSelect) Then
        '                athmname = thm.getname
        '                If ((athmname.contains("basply")) Or (athmname.contains("basin"))) Then
        '                    mypolythmlist.add(thm)
        '                End If
        '            End If
        '        Next

        '        If (mypolythmlist.isempty) Then
        '            theTheme = MsgBox.Choice(theThemelist, "Select basin coverage/grid theme", "Basin Theme")
        '            If (theTheme = nil) Then
        '                Exit Sub
        '            End If
        '        Else
        '            theTheme = MsgBox.Choice(mypolythmlist, "Select basin coverage/grid theme", "Basin Theme")
        '            If (theTheme = nil) Then
        '                Exit Sub
        '            End If
        '        End If

        '        If (TheTheme.CanSelect) Then
        '            theftheme = theTheme
        '            theftab = theTheme.GetFtab
        '            shpfld = theFTab.FindField("Shape")
        '            theFTab.CreateIndex(shpfld)
        '        Else
        '            p = False
        '            def = av.GetProject.MakeFileName("theme", "shp")
        '            def = FileDialog.Put(def, "*.shp", "Convert " + TheTheme.getName)
        '            If (def = NIL) Then
        '                Return NIL
        '            End If
        '            theFTab = TheTheme.ExportToFtab(def)
        '            ' For Database themes, which can return a nil FTab sometimes 
        '            If (theFTab = nil) Then
        '    MsgBox.Warning("Error occurred while converting to shapefile."+NL+
        '        "Shapefile was not created.", "Convert " + TheTheme.getName)
        '                Exit Sub
        '            End If
        '            shpfld = theFTab.FindField("Shape")
        '            ' build the spatial index
        '            theFTab.CreateIndex(shpfld)
        '            theftheme = FTheme.Make(theFTab)
        '            theView.AddTheme(theftheme)
        '            '  theView.GetWin.Activate
        '            End If

        '            theFtab.SetEditable(True)
        '            addlist = List.Make

        '            Highflowfld = theFtab.FindField("Highflow")
        '            If (Highflowfld = nil) Then
        '                MsgBox.error("High flow field, Highflow, has not been initialized", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            lowflowfld = theFtab.FindField("Lowflow")
        '            If (lowflowfld = nil) Then
        '                MsgBox.error("Low flow field, Lowflow, has not been initialized", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            medflowfld = theFtab.FindField("Medflow")
        '            If (medflowfld = nil) Then
        '                MsgBox.error("Median flow field, Medflow, has not been initialized", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            newflds = List.make
        '            theFields = resultfile.GetFields
        '            theFields.clone()
        '            pidfield = theFtab.FindField("Gridcode")
        '            If (pidfield = nil) Then
        '                pidfield = theFtab.FindField("Grid_code")
        '                If (pidfield = nil) Then
        '                    pidfield = MsgBox.choice(thefields, "Select key field for the basin coverage/grid", "Geospatial Stream Flow Model")
        '                    If (pidfield = nil) Then
        '                        Exit Sub
        '                    End If
        '                End If
        '            End If

        '            cflowfld = theFtab.FindField("FlowNow")
        '            If (cflowfld = nil) Then
        '  fldadd = Field.Make("FlowNow", #field_decimal, 12 , 3)
        '                newflds.Add(fldadd)
        '                theFtab.AddFields(newflds)
        '                cflowfld = theFtab.FindField("FlowNow")
        '  newflds = {}
        '            End If

        '            cfindexfld = theFtab.FindField("IndexNow")
        '            If (cfindexfld = nil) Then
        '  fldadd = Field.Make("IndexNow", #field_byte, 4 , 0)
        '                newflds.Add(fldadd)
        '                theFtab.AddFields(newflds)
        '                cfindexfld = theFtab.FindField("IndexNow")
        '            End If

        '            pnumrecs = theFtab.GetNumRecords
        '            pidlist = List.Make
        'For each prec in 0..(pnumrecs-1)
        '                pidval = (theFtab.ReturnValue(pidfield, prec)).setformat("d")
        '                pidlist.Add(pidval)
        '        Next

        '        numflds = theFields.count
        '        numrecs = resultfile.GetNumRecords
        '        Dayfld = resultfile.FindField("Timestep")
        '        If (Dayfld = nil) Then
        '            Dayfld = resultfile.FindField("Day")
        '            If (Dayfld = nil) Then
        '                Dayfld = resultfile.FindField("BasinID")
        '                If (Dayfld = nil) Then
        '                    Dayfld = resultfile.FindField("Time")
        '                    If (Dayfld = nil) Then
        '                        Dayfld = MsgBox.choice(thefields, "Select key field for the time series file", "Geospatial Stream Flow Model")
        '                        If (Dayfld = nil) Then
        '                            Exit Sub
        '                        End If
        '                    End If
        '                End If
        '            End If
        '        End If

        '        daylist = List.make
        'For each frec in (numrecs-1)..0 by -1
        '            dayval = resultfile.ReturnValue(dayfld, frec)
        '            daylist.Add((dayval.setformat("dd")).asstring)
        '        Next

        '        plotday = MsgBox.ChoiceAsString(daylist, "Select day to plot flow percentile map", "Geospatial Stream Flow Model")
        '        If (plotday = nil) Then
        '            Exit Sub
        '        End If
        '        dayindex = (daylist.count - (daylist.findbyvalue(plotday)) - 1)
        '        If (dayindex = -1) Then
        '            MsgBox.error("Selected day not found in " + resultfilename.AsString, "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        av.UseWaitCursor()

        'for each mynum in 0..(numflds-1)
        '            fld = theFields.Get(mynum)
        '            av.ShowMsg("Checking Status for Basin " + (mynum + 1).asstring + " of " + numflds.asstring)
        '            progress = (mynum / (numflds - 1)) * 100
        '            doMore = av.SetStatus(progress)
        '            If (Not doMore) Then
        '                av.ClearWorkingStatus()
        '                av.clearstatus()
        '                Exit Sub
        '            End If
        '            If ((fld.IsTypeNumber) And (fld.AsString <> "timestep") And (fld.AsString <> "day")) Then
        '    For each pid in 0..(pnumrecs-1)
        '                    If (pidlist.Get(pid).AsString = fld.AsString) Then
        '                        theHighflow = theFtab.ReturnValue(highflowfld, pid)
        '                        theLowflow = theFtab.ReturnValue(Lowflowfld, pid)
        '                        theMedflow = theFtab.ReturnValue(medflowfld, pid)
        '                        theflow = resultfile.ReturnValueNumber(fld, dayindex)
        '                        theFtab.SetValue(cflowfld, pid, theflow)
        '                        If (theflow > theHighflow) Then
        '                            theFtab.SetValue(cfindexfld, pid, 3)
        '                        ElseIf (theflow < thelowflow) Then
        '                            theFtab.SetValue(cfindexfld, pid, 1)
        '                        Else
        '                            theFtab.SetValue(cfindexfld, pid, 2)
        '                        End If
        '                    End If
        '                                end if 
        '        Next
        '    Next

        '        theFtab.Flush()
        '        theFtab.SetEditable(False)
        'mylegend = legend.make(#SYMBOL_FILL)
        'mylegend.setlegendtype(#LEGEND_TYPE_COLOR)
        'mysymbol1 = symbol.make(#SYMBOL_FILL)
        'mysymbol2 = symbol.make(#SYMBOL_FILL)
        'mysymbol3 = symbol.make(#SYMBOL_FILL)

        '        myyellow = Color.make
        '        mygreen = Color.make
        '        myblue = Color.make
        'myyellow.setrgblist({250, 250, 0}) 
        'mygreen.setrgblist({0, 240, 0})
        'myblue.setrgblist({0, 0, 250})
        '        mysymbol1.SetColor(myyellow)
        '        mysymbol2.SetColor(mygreen)
        '        mysymbol3.SetColor(myblue)
        '        mylegend.Interval(theFtheme, "IndexNow", 3)
        'mylegend.SetClassInfo(0, {"low_flow", "1", mySymbol1, 0, 1})
        'mylegend.SetClassInfo(1, {"normal_flow", "2", mySymbol2, 1, 2})
        'mylegend.SetClassInfo(2, {"high_flow", "3", mySymbol3, 2, 3})
        '        mylegendFN = filename.Merge(myWkDirname, "flow.avl")
        '        mylegend.Save(mylegendFN)
        '        'thelegend = legend.make(#SYMBOL_FILL)
        '        'thelegend.Load(mylegendFN, #LEGEND_LOADTYPE_ALL) 
        '        theFtheme.SetLegend(mylegend)
        '        theFtheme.UpdateLegend()
        '        theFtheme.SetVisible(True)
        '        'theFtheme.SetName("Basin.shp")


    End Sub

    Friend Sub SetPlot()
        ' ***********************************************************************************************
        ' ***********************************************************************************************
        '
        '      Program: setplot.ave
        '
        '      Function: 
        '          Activates the plotting tool active. 
        '          Subsequent clicks on the View initiates plotxy.ave
        '
        '      Inputs: 
        '          none
        '
        '      Outputs: 
        '          none. Plotting tool becomes active.
        '
        '      Assumptions: The program assumes that 
        '          the View is active and it contains the coverage/grid of subbasins
        '          the plotting tool is the second from last tool in the View tool bar
        '
        ' ***********************************************************************************************
        ' ***********************************************************************************************

        '        TheProject = av.GetProject
        '        theView = av.getactiveDoc
        '        ViewChk = TheView.GetGUI
        '        If (ViewChk <> "View") Then
        '            MsgBox.error("Click on the View to make it 'active' before running this program", "")
        '            Exit Sub
        '        End If

        '        theThemelist = theView.GetThemes
        '        TheTheme = theView.FindTheme("basply.shp")
        '        If (TheTheme = nil) Then
        '            For Each vthm In theThemelist
        '                thethmnm = vthm.getname
        '                If ((thethmnm.contains("basply")) And (vthm.CanSelect)) Then
        '                    TheTheme = theView.FindTheme(thethmnm)
        '                    break()
        '                End If
        '            Next
        '            If (thetheme = nil) Then
        '                theTheme = MsgBox.Choice(theThemelist, "Select basin grid/polygon theme", "Basin Theme")
        '                If (theTheme = nil) Then
        '                    Exit Sub
        '                End If
        '            End If
        '        End If



        '        theWKdir = theproject.GetFileName.ReturnDir

        '        found = False
        '        p = theView.GetDisplay.ReturnUserPoint

        '        If (TheTheme.Is(Ftheme).Not) Then
        '            theGrid = theTheme.GetGrid
        '            mycellsize = theGrid.GetCellSize
        '            pntx = (p.GetX)
        '            pnty = (p.GetY)
        '            cenp = Point.Make(pntx, pnty)
        '            ElementId = TheGrid.CellValue(cenp, Prj.MakeNull)
        '        Else
        '            TheVtab = TheTheme.GetFtab
        '            keyfield = TheVtab.FindField("Gridcode")
        '            If (keyField = nil) Then
        '                fieldslist = TheVtab.GetFields
        '                fieldName = MsgBox.Choice(fieldslist, "Select a subbasin id field", "Geospatial Stream Flow Model")
        '                If (fieldName = nil) Then
        '                    Exit Sub
        '                End If

        '                keyField = TheVtab.FindField(fieldName.AsString)
        '                If (keyField = nil) Then
        '                    Exit Sub
        '                End If
        '            End If

        '            recs = TheTheme.FindByPoint(p)
        '            If (recs.count = 0) Then
        '                MsgBox.info("The selected point is outside the basin extent", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            rec = recs.get(0)
        '            ElementId = TheVtab.ReturnValue(keyField, rec)
        '        End If

        '        If (ElementId.AsString.IsNumber.not) Then
        '            System.beep()
        '            MsgBox.info("The selected point is outside the basin extent", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If


        '        'MAKE THE SELECTED POLYGON ACTIVE
        '        '(PRIMARILY FOR VISUAL EFFECTS)

        '        If (theTheme.Is(FTheme)) Then
        '            If (System.IsShiftKeyDown) Then
        '    op = #VTAB_SELTYPE_XOR
        '            Else
        '    op = #VTAB_SELTYPE_NEW
        '            End If
        '            If (theTheme.CanSelect) Then
        '                theTheme.SelectByPoint(p, op)
        '            End If
        '            av.GetProject.SetModified(True)
        '        End If

        '        docList = theProject.GetDocs
        '        tabList = List.Make
        '        numdocs = docList.count
        'for each i in 0..(numdocs-1)
        '            dtype = (docList.get(i)).getclass.getclassname
        '            If (dtype = "Table") Then
        '                tabList.Add(docList.Get(i).getname)
        '            End If
        '        Next

        '        'chk=msgbox.choiceAsString(tabList,"Select A Table for Plotting",tabList.Get(0))

        '        yList = List.Make
        '        'yList.Add("Precipitation")
        '        'yList.Add("Evaporation")
        '        'yList.Add("Soil Water")
        '        yList.Add("Streamflow")

        '        nList = List.Make
        '        'nList.Add("Precipitation")
        '        'nList.Add("Evaporation")
        '        'nList.Add("Soil Water")
        '        nList.Add("Streamflow")

        '        tabList = List.Make
        '        'tabList.Add("rain.txt")
        '        'tabList.Add("evap.txt")
        '        'tabList.Add("soilwater.txt")
        '        tabList.Add("Streamflow.txt")

        '        colList = List.Make
        '        colList.Add("green")
        '        colList.Add("blue")
        '        colList.Add("red")
        '        colList.Add("yellow")

        '        numtabs = tabList.Count

        '        '************************
        'for each i in 0..(numtabs-1)

        '            tabname = tabList.Get(i)

        '            FName = ElementId.AsString


        '            plotTable = theProject.FindDoc(tabname)

        '            If (plotTable = nil) Then
        '                MsgBox.info("Could not find the file, " + tabList.get(i) + " in this project", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            plotVtab = plotTable.GetVtab


        '            'SELECT FIELDS TO PLOT
        '            timeField = plotVtab.findfield("day")
        '            If (timeField = nil) Then
        '                timeField = plotVtab.findfield("Time")
        '                If (timeField = nil) Then
        '                    MsgBox.info("Table " + tablist.get(i) + " does not Contain a 'day' or 'Time' field.", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '            End If
        '            plotField = plotVtab.FindField(Fname)

        '            If (plotField = nil) Then
        '                MsgBox.info("Could not find the field named " + fname.asstring + nl + "in the file,  " + tablist.get(i), "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If


        '            xname = "Julian Day"

        '            yname = yList.Get(i)
        '            col = colList.Get(i)
        '            If (yname = "Streamflow") Then
        '                yunits = " (m3/s)"
        '            Else
        '                yunits = " (mm)"
        '            End If

        '            If (col = "blue") Then
        '                precColor = Color.GetBlue
        '            ElseIf (col = "red") Then
        '                precColor = Color.GetRed
        '            ElseIf (col = "green") Then
        '                precColor = Color.GetGreen
        '            ElseIf (col = "yellow") Then
        '                precColor = Color.GetYellow
        '            Else
        '                precColor = Color.GetBlue
        '            End If


        '            plotinfo = "Making plot...."
        '            av.Showmsg(plotinfo)
        '            'fldlist = { timeField, plotField }
        '    fldlist = { plotField }

        '            mychart = chart.make(plotVtab, fldlist)
        '            mychart.SetSeriesFromRecords(False)
        '            mychart.SetRecordLabelField(timeField)
        '            myChartName = MyChart.getname
        '            myChartDisp = mychart.getchartDisplay
        '    myChartDisp.setType(#CHARTDISPLAY_line)
        '            'myChartDisp.setMark(#CHARTDISPLAY_MARK_SQUARE)
        '            myChartDisp.SetSeriesColor(0, precColor)
        '            myChartDisp.SetMaxDataPoints(plotVtab.GetNumRecords)
        '            'myChartDisp.SetSeriesColor(0,newColor)
        '            myLegend = myChart.getchartLegend
        '            myLegend.setvisible(False)
        '            'chname=msgbox.input ( "Enter a chart name.","User Input", plotfield.getname)
        '            chname = plotfield.getname
        '            myChart.GetTitle.SetName("Hydrograph of Subbasin " + FName)
        '            the_x = myChart.GetXaxis
        '            the_y = myChart.GetYaxis
        '            the_x.SetName(xname)
        '            the_y.SetName(yname + yunits)
        '            the_x.SetTicklabelsVisible(True)
        '            the_y.SetTicklabelsVisible(True)
        '            the_x.setMajorGridVisible(False)
        '            the_x.SetMinorGridVisible(False)
        '            the_y.setMajorGridVisible(True)
        '            the_x.SetCrossValue(0)
        '            the_y.SetCrossValue(0)
        '            the_x.SetLabelVisible(True)
        '            the_y.SetLabelVisible(True)
        '            '  av.Showmsg("")
        '            mychart.getwin.open()

        '            mychart.setname("Basin" + +chname + +yname)
        '        Next

        '        'TheBitMap=PlotVtab.GetSelection
        '        '  TheBitMap.ClearAll
        '        '  PlotVtab.UpdateSelection
    End Sub

    Friend Sub BuildListofValidStationNames(ByRef aMetConstituent As String, _
                                            ByVal aStations As atcCollection)
        aStations.Clear()

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            Dim lTotalCount As Integer = lDataSource.DataSets.Count
            Dim lCounter As Integer = 0

            For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                lCounter += 1
                Logger.Progress("Building list of valid station names...", lCounter, lDataSource.DataSets.Count)

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

    Friend Class StationDetails
        Public Name As String
        Public StartJDate As Double
        Public EndJDate As Double
        Public Description As String
    End Class

End Module
