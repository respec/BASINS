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
            GridFromShapefile(lSubbasinLayerIndex, lPitFillDEMFileName, lSubbasinGridFileName)
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
            GridFromShapefile(lStreamLayerIndex, lPitFillDEMFileName, lStreamGridFileName)
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
            lStreamGridFileName = FilenameNoExt(lDEMFileName) & "Stream.bgd"
            lSubbasinGridFileName = FilenameNoExt(lDEMFileName) & "Watershed.bgd"

            MapWinGeoProc.Hydrology.DelinStreamGrids(lDEMFileName, lPitFillDEMFileName, lFlowDirGridFileName, lSlopeGridFileName, lFlowAccGridFileName, "", lStrahlOrdResultGridFileName, lLongUpslopeResultGridFileName, lTotalUpslopeResultGridFileName, lStreamGridFileName, lStreamOrderResultGridFileName, lTreeDatResultFileName, lCoordDatResultFileName, aThresh, False, False, Nothing)
            MapWinGeoProc.Hydrology.DelinStreamsAndSubBasins(lFlowDirGridFileName, lTreeDatResultFileName, lCoordDatResultFileName, lStreamShapeResultFileName, lSubbasinGridFileName, Nothing)

            If Not GisUtil.IsLayer(lSubbasinGridLayerName) Then
                GisUtil.AddLayer(lSubbasinGridFileName, "Subbasin Grid")
            End If
            If Not GisUtil.IsLayer(lStreamGridLayerName) Then
                GisUtil.AddLayer(lStreamGridFileName, "Stream Grid")
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

        'how if this river grid different from the stream link grid?
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
                        ByVal zonegname As String, ByVal fdirgname As String, ByVal flowlengname As String, _
                        ByVal outletgname As String, ByVal demgname As String, ByVal facgname As String, _
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

        Dim mycellsize As Integer = GisUtil.GridGetCellSizeX(basingthm)
        Dim lZoneLayerIndex As Integer = GisUtil.LayerIndex(zonegname)
        Dim lZoneFileName As String = GisUtil.LayerFileName(lZoneLayerIndex)
        Dim lVelocityGridLayerName As String = "Velocity Grid"
        Dim lVelocityGridLayerIndex As Integer = 0
        Dim lVelocityGridFileName As String = ""

        Dim facgthm As Integer = -1
        If facgname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + facgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            facgthm = GisUtil.LayerIndex(facgname)
        End If

        If (veltype = 1) Then

            Dim demgthm As Integer = -1
            If demgname = "<none>" Then
                Logger.Msg("DEM Grid, " + demgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
                Exit Sub
            Else
                demgthm = GisUtil.LayerIndex(demgname)
            End If

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
        Else
            'set velocity grid to a constant
            Dim lValue As Double = OverlandFlowVelocity
            If lValue < 0.01 Then lValue = 0.01
            If GisUtil.IsLayer(lVelocityGridLayerName) Then
                lVelocityGridLayerIndex = GisUtil.LayerIndex(lVelocityGridLayerName)
                GisUtil.RemoveLayer(lVelocityGridLayerIndex)
            End If
            lVelocityGridFileName = FilenameNoExt(lZoneFileName) & "Velocity.bgd"
            GisUtil.GridAssignConstant(lVelocityGridFileName, lZoneFileName, lValue)
            GisUtil.AddLayer(lVelocityGridFileName, lVelocityGridLayerName)
        End If

        'given the velocity grid, compute travel times

        '        maskgrid = ((outgrid.isnull) / (outgrid.isnull))
        '        newfdrgrid = flowdirgrid * maskgrid
        Dim lFlowDirGridFileName As String = GisUtil.LayerFileName(flowdirgthm)
        Dim lFlowAccGridFileName As String = GisUtil.LayerFileName(facgthm)
        Dim lOutletGridFileName As String = GisUtil.LayerFileName(outgthm)

        '        invelgrid = (1.AsGrid / velgrid)
        Dim lInverseVelocityGridFileName As String = FilenameNoExt(lZoneFileName) & "InverseVelocity.bgd"
        GisUtil.GridInverse(lVelocityGridFileName, lInverseVelocityGridFileName)

        '        flowtimegrid = newfdrgrid.flowlength(invelgrid, False)
        '        inverse velocity grid is used as a weighting factor 
        Dim lTravelTimeGridFileName As String = FilenameNoExt(lZoneFileName) & "TravelTime.bgd"
        GisUtil.DownstreamFlowLength(lFlowDirGridFileName, lFlowAccGridFileName, lTravelTimeGridFileName, lOutletGridFileName)

        '        daysgrid = (flowtimegrid / 86400).floor
        Dim lDaysGridLayerName As String = "Travel Time Grid (Days)"
        Dim lDaysGridLayerIndex As Integer = 0
        If GisUtil.IsLayer(lDaysGridLayerName) Then
            lDaysGridLayerIndex = GisUtil.LayerIndex(lDaysGridLayerName)
            GisUtil.RemoveLayer(lDaysGridLayerIndex)
        End If
        Dim lDaysGridFileName As String = FilenameNoExt(lZoneFileName) & "DaysGrid.bgd"
        GisUtil.GridMultiplyConstant(lTravelTimeGridFileName, 1 / 86400, lDaysGridFileName)
        'need to make sure days grid is an integer 
        GisUtil.AddLayer(lDaysGridFileName, lDaysGridLayerName)
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
        Dim lDaysArray(zonelist.Count, numdays) As Integer
        For nday As Integer = 0 To numdays - 1
            Dim lDaysZonalStats As atcCollection = GisUtil.GridZoneCountValue(basingthm, lDaysGridLayerIndex, nday)
            Dim lCnt As Integer = 0
            For Each lzone As Integer In zonelist
                lCnt += 1
                If Not lDaysZonalStats.ItemByKey(lzone) Is Nothing Then
                    lDaysArray(lCnt, nday) = lDaysZonalStats.ItemByKey(lzone)
                End If
            Next
        Next

        For Each bid As Integer In zonelist
            lStr = CStr(bid)
            Dim lSum As Integer = 0
            Dim lCnt As Integer = 0
            For nday As Integer = 0 To numdays - 1
                lSum += lDaysArray(lCnt, nday)
            Next
            For nday As Integer = 0 To numdays - 1
                If lSum > 0 Then
                    lStr = lStr & ", " & (lDaysArray(lCnt, nday) / lSum)
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

    Friend Sub RainEvap()
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
    End Sub

    Friend Sub Balance()
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
    End Sub

    Friend Sub Route()
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
    End Sub

    Friend Sub Sensitivity()
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
    End Sub

    'subs below are new GIS routines needed for GeoSFM

    Friend Sub GridFromShapefile(ByVal aShapefileLayerIndex As Integer, ByVal aBaseGridFileName As String, ByVal aNewGridFileName As String)
        'given a shapefile (subbasins or streams) and a base grid, return a grid version of those shapes

        'only used if streams and subbasins are entered as shapefiles
    End Sub

End Module
