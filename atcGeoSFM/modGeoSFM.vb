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
        'Dim lFlowDirGridFileName As String = GisUtil.LayerFileName(flowdirgthm)
        Dim lOutletGridFileName As String = GisUtil.LayerFileName(outgthm)
        'Dim lFlowLengthGridFileName As String = GisUtil.LayerFileName(flowlengthm)

        '        invelgrid = (1.AsGrid / velgrid)
        Dim lInverseVelocityGridFileName As String = FilenameNoExt(lVelocityGridFileName) & "InverseVelocity.bgd"
        GisUtil.GridInverse(lVelocityGridFileName, lInverseVelocityGridFileName)

        '        flowtimegrid = newfdrgrid.flowlength(invelgrid, False)
        Dim lTravelTimeGridFileName As String = FilenameNoExt(lFlowAccGridFileName) & "TravelTime." & FileExt(lFlowAccGridFileName)

        'GisUtil.FlowLengthToOutlet(lFlowLengthGridFileName, lOutletGridFileName, lZoneFileName, lTravelTimeGridFileName)
        '        inverse velocity grid is used as a weighting factor 
        GisUtil.DownstreamFlowLength(lFlowDirGridFileName, lFlowAccGridFileName, lTravelTimeGridFileName, lOutletGridFileName, lInverseVelocityGridFileName)
        'Dim lWeightedGridFileName As String = FilenameNoExt(lTravelTimeGridFileName) & "Weighted." & FileExt(lTravelTimeGridFileName)
        'GisUtil.GridMultiply(lTravelTimeGridFileName, lInverseVelocityGridFileName, lWeightedGridFileName)

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

        '  fldlist = {"Rainfall and Evaporation", "Rainfall Only", "Evaporation Only"}
        '  fldchoice = MsgBox.ChoiceAsString(fldlist, "Select Parameter to Extract", "Extraction Parameter")
        Dim typenum As Integer = 0
        'If (fldchoice = "") Then
        '    Exit Sub
        'ElseIf (fldchoice = "Rainfall Only") Then
        '    typenum = 1
        'ElseIf (fldchoice = "Evaporation Only") Then
        '    typenum = 2
        'ElseIf (fldchoice = "Rainfall and Evaporation") Then
        '    typenum = 3
        'End If

        'TheWkDir = TheProject.GetFileName.ReturnDir
        'orderFNini = FileName.Merge(TheWkDir.AsString, "order.txt")
        'If (TheWkDir.AsString.contains("\")) Then
        '    myraindir = ((thewkdir.asstring.astokens("\").get(0)) + "\raindata\rain_year\")
        '    myevapdir = ((thewkdir.asstring.astokens("\").get(0)) + "\evapdata\evap_year\")
        'Else
        '    myraindir = ((thewkdir.asstring.astokens("/").get(0)) + "/raindata/rain_year/")
        '    myevapdir = ((thewkdir.asstring.astokens("/").get(0)) + "/evapdata/evap_year/")
        'End If

        If (typenum = 3) Then
            '  defaultlist = { "Basins" , TheWkDir.AsString , myraindir.AsString , myevapdir.AsString , "1999" , "1999" , "1" , "240", orderFNini.AsString }
            '  labellist = { "Basin Grid" , "Output Directory", "Rain Data Directory" , "Evap Data Directory" , "Start Year" , "End Year" , "Start Day Number" , "End Day Number", "Computational Order File" }

            '    inputlist = MsgBox.MultiInput("Enter Model Parameters", "Geospatial Stream Flow Model", labellist, defaultlist)

            '    If (inputlist.isempty) Then
            '        Exit Sub
            '    End If

            '    basingname = inputlist.get(0)
            '    workdirname = inputlist.get(1)
            '    raindirname = inputlist.get(2)
            '    evapdirname = inputlist.get(3)
            '    startyear = inputlist.get(4)
            '    endyear = inputlist.get(5)
            '    startday = inputlist.get(6)
            '    endday = inputlist.get(7)
            '    orderFileN = inputlist.get(8)
        ElseIf (typenum = 2) Then
            '  defaultlist = { "Basins" , TheWkDir.AsString , myevapdir.AsString , "1999" , "1999" , "1" , "240", orderFNini.AsString }
            '  labellist = { "Basin Grid" , "Output Directory", "Evap Data Directory" , "Start Year" , "End Year" , "Start Day Number" , "End Day Number", "Computational Order File" }

            '    inputlist = MsgBox.MultiInput("Enter Model Parameters", "Geospatial Stream Flow Model", labellist, defaultlist)

            '    If (inputlist.isempty) Then
            '        Exit Sub
            '    End If

            '    basingname = inputlist.get(0)
            '    workdirname = inputlist.get(1)
            '    raindirname = inputlist.get(2)
            '    evapdirname = inputlist.get(2)
            '    startyear = inputlist.get(3)
            '    endyear = inputlist.get(4)
            '    startday = inputlist.get(5)
            '    endday = inputlist.get(6)
            '    orderFileN = inputlist.get(7)
        ElseIf (typenum = 1) Then
            '  defaultlist = { "Basins" , TheWkDir.AsString , myraindir.AsString , "1999" , "1999" , "1" , "240", orderFNini.AsString }
            '  labellist = { "Basin Grid" , "Output Directory", "Rain Data Directory" , "Start Year" , "End Year" , "Start Day Number" , "End Day Number", "Computational Order File" }

            '    inputlist = MsgBox.MultiInput("Enter Model Parameters", "Geospatial Stream Flow Model", labellist, defaultlist)

            '    If (inputlist.isempty) Then
            '        Exit Sub
            '    End If

            '    basingname = inputlist.get(0)
            '    workdirname = inputlist.get(1)
            '    raindirname = inputlist.get(2)
            '    evapdirname = inputlist.get(2)
            '    startyear = inputlist.get(3)
            '    endyear = inputlist.get(4)
            '    startday = inputlist.get(5)
            '    endday = inputlist.get(6)
            '    orderFileN = inputlist.get(7)
        End If

        'If (startyear.IsNumber.Not) Then
        '    MsgBox.error("Start year must be a number     eg 1999", "Geospatial Stream Flow Model")
        '    Exit Sub
        'ElseIf (endyear.IsNumber.Not) Then
        '    MsgBox.error("End year must be a number     eg 1999", "Geospatial Stream Flow Model")
        '    Exit Sub
        'ElseIf (startday.IsNumber.Not) Then
        '    MsgBox.error("Start day must be a number from 1 to 366", "Geospatial Stream Flow Model")
        '    Exit Sub
        'ElseIf (endday.IsNumber.Not) Then
        '    MsgBox.error("End day must be a number from 1 to 366", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'basingrdthm = TheView.FindTheme(basingname)
        'If (basingrdthm = nil) Then
        '    MsgBox.error("Basin grid theme not found in the active View", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'If (basingrdthm.Is(GTHEME).Not) Then
        '    MsgBox.error(Basingname + " is not a grid theme. Specify a grid theme as the basin grid.", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'zoneObj = basingrdthm.GetGrid

        'zonetable = basingrdthm.GetVtab
        'If (zonetable = nil) Then
        '    MsgBox.error("Cannot open basin grid Value Attribute Table", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'zoneField = zoneTable.FindField("Value")
        'If (zonefield = nil) Then
        '    MsgBox.error("Field VALUE not found in basin grid Value Attribute Table", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'thebitmap = zonetable.GetSelection
        'thebitmap.ClearAll()
        'zonetable.UpdateSelection()

        'orderfile = LineFile.Make((orderFileN).AsFileName, #FILE_PERM_READ)
        'If (orderfile = nil) Then
        '    MsgBox.error("Cannot find computational order file, " + orderFileN + nl + "Run 'Generate Basin Characteristics File' menu to create order.txt", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If
        'ordlist = List.make
        'ordsize = orderfile.getsize
        'If (ordsize = 1) Then
        '    MsgBox.error(orderFileN + " is empty or has data in a single row." + nl + "Run 'Generate Basin Characteristics File' menu to create a new order.txt file", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If
        'orderfile.read(ordlist, ordsize)

        'headerstr = "Time"
        'zonelist = List.make
        'If (ordlist.get(0).IsNumber) Then
        '    ostart = 0
        'Else
        '    ostart = 1
        'End If

        'av.ShowMsg("Setting computational order.....")
        'av.ShowStopButton()
        'for each orec in ostart..(ordsize - 1)
        '    If (ordlist.get(orec).asstring.isnumber.not) Then
        '        MsgBox.error("Unrecognized basin-id format: " + nl + " ' " + ordlist.get(orec).asstring + " ' " + nl + " found in computation order file." + nl + "Run 'Generate Basin Characteristics File' menu to create a new order.txt file", "Geospatial Stream Flow Model")
        '        Exit Sub
        '    End If
        '    ordnum = ordlist.get(orec).asstring.asnumber

        '    headerstr = headerstr + ", " + ordnum.asstring
        '    For Each zrec In zonetable
        '        zonenum = zonetable.returnvalue(zonefield, zrec)
        '        If (zonenum = ordnum) Then
        '            zonelist.add(zrec.asstring.AsNumber)
        '            break()
        '        End If
        '    Next
        '    doMore = av.SetWorkingStatus
        '    If (doMore = False) Then
        '        Exit Sub
        '    End If
        'Next


        '--set the extent before extracting
        'ae = theView.GetExtension(AnalysisEnvironment)
        'ae.SetExtent(#ANALYSISENV_VALUE,basingrdthm.ReturnExtent)
        'ae.SetCellSize(#ANALYSISENV_VALUE,zoneobj.GetCellSize)
        'ae.Activate()

        'currentyear = startyear.AsNumber
        'tempday = startday.AsNumber
        'Day = 0
        'lastday = 0

        'If ((typenum = 1) Or (typenum = 3)) Then

        '    rfilename = FileName.Merge(workdirname.AsString, "rain.txt")

        '                  rainfile = TextFile.Make(rfilename, #FILE_PERM_WRITE)
        '    If (rainfile = nil) Then
        '        MsgBox.error("Cannot create output file: " + rfileName.AsString, "Geospatial Stream Flow Model")
        '        Exit Sub
        '    End If

        '    av.ShowMsg("Creating output file, rain.txt........")

        '    rainfile.Write(headerstr, headerstr.count)
        '    rainfile.Write(nl, 1)
        '    rainfile.flush()
        '    rainfile.close()
        'End If

        'If ((typenum = 2) Or (typenum = 3)) Then

        '    efilename = FileName.Merge(workdirname.AsString, "evap.txt")

        '      evapfile = TextFile.Make(efilename, #FILE_PERM_WRITE)
        '    If (evapfile = nil) Then
        '        MsgBox.error("Cannot create output file: " + efilename.AsString, "Geospatial Stream Flow Model")
        '        Exit Sub
        '    End If

        '    av.ShowMsg("Creating output file, evap.txt........")

        '    evapfile.Write(headerstr, headerstr.count)
        '    evapfile.Write(nl, 1)
        '    evapfile.flush()
        '    evapfile.close()
        'End If

        'Do While (currentyear <= (endyear.AsNumber))

        '    Day = tempday

        '    If (currentyear = (endyear.AsNumber)) Then
        '        lastday = endday.AsNumber
        '    ElseIf (currentyear.Mod(4) = 0) Then
        '        lastday = 366
        '    Else
        '        lastday = 365
        '    End If

        '        raingrdlst = {}
        '        evapgrdlst = {}

        '    Do While (Day <= lastday)

        '        Day.setformat("dddd")

        '        av.ShowMsg("Computing Zonal Mean for Day  " + +Day.asString + +" of Year  " + +currentyear.AsString)

        '        If ((typenum = 1) Or (typenum = 3)) Then
        '            Day.setformat("dddd")

        '            zRainFN = FileName.Merge(workdirname, ("zr" + currentyear.asstring + Day.asstring + ".dbf"))
        '            currentraindir = raindirname.substitute("year", currentyear.asstring)

        '            If (File.Exists(currentraindir.AsFileName).Not) Then
        '                MsgBox.Error("Cannot read directory " + currentraindir, "Directory Specification Error")
        '                Exit Sub
        '            End If

        '            theRainName = Grid.MakeSrcName(currentraindir + "rain_" + currentyear.asstring + Day.asstring)
        '            If (theRainName = nil) Then
        '                theRainName = Grid.MakeSrcName(currentraindir + "rain_" + currentyear.asstring + Day.setformat("").asstring)
        '                If (theRainName = nil) Then
        '                    MsgBox.Error("Rainfall Grid " + currentraindir + "rain_" + currentyear.asstring + Day.asstring + " Not Found", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '            End If
        '            theRainThm = Theme.Make(theRainName)
        '            TheRainGrid = Grid.Make(theRainName)
        '            If (TheRainGrid.isinteger.not) Then
        '                tmpgrid = TheRainGrid.int
        '                TheRainGrid = tmpgrid
        '            End If

        '            raingrdlst.Add(theRainGrid)

        '            RainVTab = theRainGrid.ZonalStatsTable(zoneObj, ThePrj, zoneField, False, zRainFN)
        '            If (RainVTab.HasError) Then
        '                Return NIL
        '                MsgBox.error("Cannot create rainfall zonal statistics table, " + zRainFN, "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            rainfield = RainVtab.FindField("median")
        '            If (rainfield = nil) Then
        '                MsgBox.error("Cannot find 'median' field in " + zRainFN + " table.", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            outputday = Day.setformat("dddd")
        '                        av.run("FEWS.rainwrite.ave", {rfilename, zRainFN, outputday, currentyear, zonelist })
        '            theproject.save()

        '        End If

        '        If ((typenum = 2) Or (typenum = 3)) Then
        '            Day.setformat("dddd")

        '            zEvapFN = FileName.Merge(workdirname, ("ze" + currentyear.asstring + Day.asstring + ".dbf"))
        '            currentevapdir = evapdirname.substitute("year", currentyear.asstring)

        '            If (File.Exists(currentevapdir.AsFileName).Not) Then
        '                MsgBox.Error("Cannot read directory " + currentevapdir, "Directory Specification Error")
        '                Exit Sub
        '            End If

        '            theEvapName = Grid.MakeSrcName(currentevapdir + "evap_" + currentyear.asstring + Day.asstring)
        '            If (theEvapName = nil) Then
        '                theEvapName = Grid.MakeSrcName(currentevapdir + "evap_" + currentyear.asstring + Day.setformat("").asstring)
        '                If (theEvapName = nil) Then
        '                    MsgBox.Error("Evaporation Grid " + currentevapdir + "evap_" + currentyear.asstring + Day.asstring + " Not Found", "Geospatial Stream Flow Model")
        '                    Exit Sub
        '                End If
        '            End If
        '            theEvapThm = Theme.Make(theEvapName)
        '            TheEvapGrid = Grid.Make(theEvapName)
        '            If (TheEvapGrid.isinteger.not) Then
        '                tmpgrid = TheEvapGrid.int
        '                TheEvapGrid = tmpgrid
        '            End If

        '            evapgrdlst.Add(theEvapGrid)

        '            EvapVTab = theEvapGrid.ZonalStatsTable(zoneObj, ThePrj, zoneField, False, zEvapFN)
        '            If (EvapVTab.HasError) Then
        '                Return NIL
        '                MsgBox.error("Cannot create evaporation zonal statistics table, " + zEvapFN, "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            evapfield = EvapVtab.FindField("median")
        '            If (evapfield = nil) Then
        '                MsgBox.error("Cannot find 'median' field in " + zEvapFN + " table.", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            outputday = Day.setformat("dddd")
        '                av.run("FEWS.rainwrite.ave", {efilename, zEvapFN, outputday, currentyear, zonelist})
        '            theproject.save()

        '        End If

        '        Day = Day + 1

        '    Loop


        '    tempday = 1
        '    currentyear = currentyear + 1

        'Loop

        'orderfile.close()

        'If (typenum = 1) Then
        '    MsgBox.Info("Processing Complete. Output files: " + nl + rfilename.AsString, "Geospatial Stream Flow Model")
        'ElseIf (typenum = 2) Then
        '    MsgBox.Info("Processing Complete. Output files: " + nl + efilename.AsString, "Geospatial Stream Flow Model")
        'Else
        '    MsgBox.Info("Processing Complete. Output files: " + nl + rfilename.AsString + nl + efilename.AsString, "Geospatial Stream Flow Model")
        'End If

        'sub rainwrite
        'rfilename = SELF.Get(0)
        'zRainFN = SELF.Get(1)
        'outputday = SELF.Get(2)
        'currentyear = SELF.Get(3)
        'zonelist = SELF.Get(4)
        'rainfile = TextFile.Make(rfilename, #FILE_PERM_APPEND)

        'RainVtab = Vtab.Make(zRainFN, False, False)
        'rainfield = RainVtab.FindField("median")
        'If (rainfield = nil) Then
        '    MsgBox.error("Cannot find 'median' field in " + zRainFN + " table.", "Geospatial Stream Flow Model")
        '    Exit Sub
        'End If

        'rainfile = TextFile.Make(rfilename, #FILE_PERM_APPEND)

        'rainfile.Write(currentyear.asstring + outputday.asstring, 7)

        'rreccount = 0

        'For Each rrecord In RainVtab
        '    rrecposition = zonelist.get(rreccount)
        '    'rainwnum = (RainVtab.ReturnValue(Rainfield, rrecord))
        '    rainwnum = (RainVtab.ReturnValue(Rainfield, rrecposition))
        '    rainwrt = (rainwnum.setformat("d.d")).AsString
        '    rainfile.Write(", " + rainwrt, (rainwrt.count + 2))
        '    rreccount = rreccount + 1
        'Next

        ''msgbox.info("finishwriting","")

        'rainfile.Write(nl, 1)
        'rainfile.flush()
        'rainfile.close()
        'rainVtab.Deactivate()
        'rainVtab = nil
        'av.PurgeObjects()

        'File.Delete(zRainFN)

        'The rain.txt file created from this process contains an average rainfall value in millimeters for each subbasin per day. 
        'The evap.txt file contains a potential evapotranspiration (PET) value in tenths of millimeters for each subbasin per day.
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

        '        TheProject = av.GetProject
        '        theFileName = theProject.GetFileName
        '        If (theFileName = nil) Then
        '            av.Run("Project.SaveAs", nil)
        '            theFileName = theProject.GetFileName
        '            If (theFileName = nil) Then
        '    exit
        '                End
        '            Else
        '                If (av.Run("Project.CheckForEdits", nil).Not) Then
        '                    Return nil
        '                    End
        '                    If (theProject.Save) Then
        '                        av.ShowMsg("Project saved to '" + theFileName.GetBaseName + "'")
        '    if (System.GetOS = #SYSTEM_OS_MAC) then
        '                            Script.Make("MacClass.SetDocInfo(SELF, Project)").DoIt(theFileName)
        '                            End
        '                            End
        '                            End

        '                            TheView = av.GetActiveDoc
        '                            ViewChk = TheView.GetGUI
        '                            If (ViewChk <> "View") Then
        '                                MsgBox.error("Click on the View to make it 'active' before running this program", "")
        ' exit
        '                                End

        '                                ' Check the working directory
        '                                TheWkDir = TheProject.GetWorkDir.asstring
        '                                myWkDirname = MsgBox.Input("Specify your working directory", "Working Directory", TheWkDir)
        '                                If (mywkDirName = nil) Then
        ' exit
        '                                    End
        '                                    If (File.Exists(myWkDirname.AsFileName).not) Then
        '                                        MsgBox.Error("Cannot read directory " + myWkDirname, "Directory Specification Error")
        ' exit
        '                                        End
        '                                        If (File.IsWritable(myWkDirname.AsFileName).not) Then
        '                                            MsgBox.Error(myWkDirname + +"is not writable.", "Directory Specification Error")
        ' exit
        '                                            End
        '                                            TheProject.SetWorkDir(myWkDirname.AsFileName)

        '                                            If ((myWkDirname.contains("\").Not) And (myWkDirname.contains("/")) And (myWkDirname.right(1) <> "/")) Then
        '                                                myWkDirname = myWkDirname + "/"
        '                                            ElseIf ((myWkDirname.contains("\")) And (myWkDirname.right(1) <> "\")) Then
        '                                                myWkDirname = myWkDirname + "\"
        '                                            ElseIf ((myWkDirname.contains("\").Not) And (myWkDirname.contains("\").Not) And (myWkDirname.right(1) <> "/")) Then
        '                                                myWkDirname = myWkDirname + "/"
        '                                                End

        '                                                'CREATE A LIST OF INPUT LABELS
        'labels = { "Input Rainfall File", "Input Evap File", "Input Basin File", 
        '          "Input Response File", "Input Parameter File", "Basin Runoff Yield File",
        '          "Output Soil Moisture File", "Output Actual Evap.File", "Ground Water Loss File", 
        '          "Current Soil Water File","Excess Runoff File", "Interflow File", "Baseflow File", 
        '          "Mass Balance File", "Process/Error Log File",  "Initial Soil Moisture File"}

        'defaults = { "rain.txt", "evap.txt","basin.txt","response.txt", "balparam.txt", "basinrunoffyield.txt", "soilwater.txt", "actualevap.txt", "gwloss.txt", "cswater.txt", "excessflow.txt", "interflow.txt", "baseflow.txt", "massbalance.txt", "logfilesoil.txt", "initial.txt"}

        '                                                inpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", labels, defaults)

        '                                                If (inpList.IsEmpty) Then
        '  exit
        '                                                    End

        '                                                    rainfilename = myWkDirname + inplist.get(0)
        '                                                    evapfilename = myWkDirname + inplist.get(1)
        '                                                    basinfilename = myWkDirname + inplist.get(2)
        '                                                    respfilename = myWkDirname + inplist.get(3)
        '                                                    paramfilename = myWkDirname + inplist.get(4)
        '                                                    surpfilename = myWkDirname + inplist.get(5)
        '                                                    storefilename = myWkDirname + inplist.get(6)
        '                                                    aevapfilename = myWkDirname + inplist.get(7)
        '                                                    gwlossfilename = myWkDirname + inplist.get(8)
        '                                                    outswfilename = myWkDirname + inplist.get(9)
        '                                                    excessfilename = myWkDirname + inplist.get(10)
        '                                                    interfilename = myWkDirname + inplist.get(11)
        '                                                    basefilename = myWkDirname + inplist.get(12)
        '                                                    massfilename = myWkDirname + inplist.get(13)
        '                                                    logfilename = myWkDirname + inplist.get(14)
        '                                                    initialfilename = myWkDirname + inplist.get(15)
        '                                                    balfilename = myWkDirname + "balfiles.txt"
        '                                                    'maxtimefilename = myWkDirname + "maxtime.txt"
        '                                                    testfilename = myWkDirname + "testfile.txt"
        '                                                    whichmodelFN = myWkDirname + "whichmodel.txt"



        '                                                    'rainfilename = inplist.get(0)
        '                                                    'evapfilename = inplist.get(1)
        '                                                    'basinfilename = inplist.get(2)
        '                                                    'respfilename = inplist.get(3)
        '                                                    'paramfilename = inplist.get(4)
        '                                                    'surpfilename = inplist.get(5)
        '                                                    'storefilename = inplist.get(6)
        '                                                    'aevapfilename = inplist.get(7)
        '                                                    'gwlossfilename = inplist.get(8)
        '                                                    'outswfilename = inplist.get(9)
        '                                                    'excessfilename = inplist.get(10)
        '                                                    'interfilename = inplist.get(11)
        '                                                    'basefilename = inplist.get(12)
        '                                                    'massfilename = inplist.get(13)
        '                                                    'logfilename = inplist.get(14)
        '                                                    'initialfilename = inplist.get(15)
        '                                                    '
        '                                                    oldout = theProject.finddoc("Final Soil Moisture")
        '                                                    If (oldout <> nil) Then
        '                                                        theproject.removedoc(oldout)
        '                                                        End
        '                                                        oldout = theProject.finddoc("cswater.txt")
        '                                                        If (oldout <> nil) Then
        '                                                            theproject.removedoc(oldout)
        '                                                            End

        '                                                            TableGUI = av.FindGUI("Table")
        '                                                            theDocs = theProject.GetDocsWithGroupGUI(TableGUI)
        '                                                            If (theDocs.IsEmpty.Not) Then
        '                                                                For Each ddoc In thedocs
        '                                                                    ddoc.getvtab.UnJoinall()
        '                                                                    ddoc.getvtab.UnLinkall()
        '                                                                    theProject.RemoveDoc(ddoc)
        '                                                                    End
        '                                                                    End

        '                                                                    ChartGUI = av.FindGUI("Chart")
        '                                                                    theDocs = theProject.GetDocsWithGroupGUI(ChartGUI)
        '                                                                    If (theDocs.IsEmpty.Not) Then
        '                                                                        For Each ddoc In thedocs
        '                                                                            ddoc.getvtab.UnJoinall()
        '                                                                            ddoc.getvtab.UnLinkall()
        '                                                                            theProject.RemoveDoc(ddoc)
        '                                                                            End
        '                                                                            End

        '                                                                            theFileName = theProject.GetFileName
        '                                                                            If (theFileName = nil) Then
        '                                                                                av.Run("Project.SaveAs", nil)
        '                                                                                theFileName = theProject.GetFileName
        '                                                                                If (theFileName = nil) Then
        '    exit
        '                                                                                    End
        '                                                                                Else
        '                                                                                    If (av.Run("Project.CheckForEdits", nil).Not) Then
        '                                                                                        Return nil
        '                                                                                        End
        '                                                                                        If (theProject.Save) Then
        '                                                                                            av.ShowMsg("Project saved to '" + theFileName.GetBaseName + "'")
        '    if (System.GetOS = #SYSTEM_OS_MAC) then
        '                                                                                                Script.Make("MacClass.SetDocInfo(SELF, Project)").DoIt(theFileName)
        '                                                                                                End
        '                                                                                                End
        '                                                                                                End

        'chkfile=TextFile.Make((balfilename).asfilename,#file_perm_write)
        '                                                                                                If (chkfile = nil) Then
        '                                                                                                    MsgBox.info("Could not create the input/output file: " + nl + "balfiles.txt", "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                    End

        'rainfile = LineFile.Make((rainfilename).asfilename,#file_perm_read)
        '                                                                                                    If (rainfile = nil) Then
        '                                                                                                        MsgBox.info("Could not open rain file," + nl + rainfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                        End

        'evapfile = LineFile.Make((evapfilename).asfilename,#file_perm_read)
        '                                                                                                        If (evapfile = nil) Then
        '                                                                                                            MsgBox.info("Could not open evap file," + nl + evapfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                            End

        'basinfile = LineFile.Make((basinfilename).asfilename,#file_perm_read)
        '                                                                                                            If (basinfile = nil) Then
        '                                                                                                                MsgBox.info("Could not open basin file," + nl + basinfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                End

        'respfile=LineFile.Make(respfilename.asfilename,#file_perm_read)
        '                                                                                                                If (respfile = nil) Then
        '                                                                                                                    MsgBox.info("Could not create the input/output file: " + nl + respfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                    End

        'paramfile=TextFile.Make((paramfilename).asfilename,#file_perm_write)
        '                                                                                                                    If (paramfile = nil) Then
        '                                                                                                                        MsgBox.info("Could not create the balance parameter file: " + nl + paramfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                        End

        'surpfile=LineFile.Make(surpfilename.asfilename,#file_perm_write)
        '                                                                                                                        If (surpfile = nil) Then
        '                                                                                                                            MsgBox.info("Could not create the basin runoff yield file: " + nl + surpfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                            End

        'storefile=LineFile.Make(storefilename.asfilename,#file_perm_write)
        '                                                                                                                            If (storefile = nil) Then
        '                                                                                                                                MsgBox.info("Could not create the soil water file: " + nl + storefilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                End

        'aevapfile=LineFile.Make(aevapfilename.asfilename,#file_perm_write)
        '                                                                                                                                If (aevapfile = nil) Then
        '                                                                                                                                    MsgBox.info("Could not create the actual evapotranspiration file: " + nl + aevapfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                    End

        'gwlossfile=LineFile.Make(gwlossfilename.asfilename,#file_perm_write)
        '                                                                                                                                    If (gwlossfile = nil) Then
        '                                                                                                                                        MsgBox.info("Could not create the ground water loss file: " + nl + gwlossfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                        End

        'outswfile=LineFile.Make(outswfilename.asfilename,#file_perm_write)
        '                                                                                                                                        If (outswfile = nil) Then
        '                                                                                                                                            MsgBox.info("Could not create the current soil water file: " + nl + outswfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                            End

        'excessfile=LineFile.Make(excessfilename.asfilename,#file_perm_write)
        '                                                                                                                                            If (excessfile = nil) Then
        '                                                                                                                                                MsgBox.info("Could not create the input/output file: " + nl + excessfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                End

        'interfile=LineFile.Make(interfilename.asfilename,#file_perm_write)
        '                                                                                                                                                If (interfile = nil) Then
        '                                                                                                                                                    MsgBox.info("Could not create the input/output file: " + nl + interfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                    End

        'basefile=LineFile.Make(basefilename.asfilename,#file_perm_write)
        '                                                                                                                                                    If (basefile = nil) Then
        '                                                                                                                                                        MsgBox.info("Could not create the input/output file: " + nl + basefilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                        End

        'massfile=LineFile.Make(massfilename.asfilename,#file_perm_write)
        '                                                                                                                                                        If (massfile = nil) Then
        '                                                                                                                                                            MsgBox.info("Could not create the input/output file: " + nl + massfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                            End

        'logfile=LineFile.Make(logfilename.asfilename,#file_perm_write)
        '                                                                                                                                                            If (logfile = nil) Then
        '                                                                                                                                                                MsgBox.info("Could not create the input/output file: " + nl + logfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                End

        'initialfile=LineFile.Make(initialfilename.asfilename,#file_perm_write)
        '                                                                                                                                                                If (initialfile = nil) Then
        '                                                                                                                                                                    MsgBox.info("Could not create the input/output file: " + nl + initialfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                    End

        '                                                                                                                                                                    'timefile=LineFile.Make((maxtimefilename).asfilename,#file_perm_write)
        '                                                                                                                                                                    'if (timefile=nil) then
        '                                                                                                                                                                    '  msgbox.info("Could not create the input/output file: " +nl+ maxtimefilename,"Geospatial Stream Flow Model")
        '                                                                                                                                                                    '  exit
        '                                                                                                                                                                    'end

        '                                                                                                                                                                    surpfile.SetScratch(True)
        '                                                                                                                                                                    storefile.SetScratch(True)
        '                                                                                                                                                                    aevapfile.SetScratch(True)
        '                                                                                                                                                                    gwlossfile.SetScratch(True)
        '                                                                                                                                                                    outswfile.SetScratch(True)
        '                                                                                                                                                                    excessfile.SetScratch(True)
        '                                                                                                                                                                    interfile.SetScratch(True)
        '                                                                                                                                                                    basefile.SetScratch(True)
        '                                                                                                                                                                    massfile.SetScratch(True)
        '                                                                                                                                                                    logfile.SetScratch(True)
        '                                                                                                                                                                    initialfile.SetScratch(True)

        '                                                                                                                                                                    rainsize = rainfile.getsize
        '                                                                                                                                                                    evapsize = evapfile.getsize
        '                                                                                                                                                                    If (evapsize > rainsize) Then
        '                                                                                                                                                                        runsize = rainsize - 1
        '                                                                                                                                                                    ElseIf (rainsize > evapsize) Then
        '                                                                                                                                                                        runsize = evapsize - 1
        '                                                                                                                                                                    Else
        '                                                                                                                                                                        runsize = rainsize - 1
        '                                                                                                                                                                        End
        '                                                                                                                                                                        bassize = (basinfile.getsize - 1)

        '                                                                                                                                                                        rainlist = List.make
        '                                                                                                                                                                        rainfile.setpos(0)
        '                                                                                                                                                                        rainfile.read(rainlist, rainsize)
        '                                                                                                                                                                        timestr = ((((rainlist.get(1)).substitute(" ", ",")).astokens(",")).get(0)).asstring
        '                                                                                                                                                                        If ((timestr.count >= 7) And (timestr.isnumber)) Then
        '                                                                                                                                                                            startyr = (timestr.left(4)).asstring
        '                                                                                                                                                                            startdy = (timestr.middle(4, 3)).asstring
        '                                                                                                                                                                        Else
        '                                                                                                                                                                            startyr = "2000"
        '                                                                                                                                                                            startdy = "001"
        '                                                                                                                                                                            End

        '                                                                                                                                                                            resplist = List.make
        '                                                                                                                                                                            respsize = respfile.getsize
        '                                                                                                                                                                            respfile.read(resplist, (respsize))
        '                                                                                                                                                                            If (respsize = 0) Then
        '                                                                                                                                                                                MsgBox.info("The response file " + respfilename + " is empty", "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                                End
        '                                                                                                                                                                                respfile.setpos(0)
        '                                                                                                                                                                                If (respsize < bassize) Then
        '                                                                                                                                                                                    MsgBox.info("The response file " + respfilename + " does not contain enough subbasins.", "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                                    End
        '                                                                                                                                                                                    resdays = (((((resplist.get(0)).substitute(" ", ",")).astokens(",")).count) - 1)

        '                                                                                                                                                                                    'INITIALIZE GRAPHICS VARIABLES
        '                                                                                                                                                                                    Gcnt = theView.GetGraphics.count
        '                                                                                                                                                                                    Tsym = RasterFill.Make
        '                                                                                                                                                                                    Tsym.Setcolor(Color.getRed)

        '                                                                                                                                                                                    chkfile.write(rainfilename, rainfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(evapfilename, evapfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(basinfilename, basinfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(respfilename, respfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(paramfilename, paramfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(surpfilename, surpfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(storefilename, storefilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(aevapfilename, aevapfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(gwlossfilename, gwlossfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(outswfilename, outswfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(excessfilename, excessfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(interfilename, interfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(basefilename, basefilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(massfilename, massfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(logfilename, logfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(initialfilename, initialfilename.count)
        '                                                                                                                                                                                    chkfile.writeelt(nl)
        '                                                                                                                                                                                    chkfile.write(myWkDirname, myWkDirname.count)

        '                                                                                                                                                                                    chkfile.close()
        '                                                                                                                                                                                    rainfile.close()
        '                                                                                                                                                                                    evapfile.close()
        '                                                                                                                                                                                    basinfile.close()
        '                                                                                                                                                                                    respfile.close()
        '                                                                                                                                                                                    surpfile.close()
        '                                                                                                                                                                                    storefile.close()
        '                                                                                                                                                                                    aevapfile.close()
        '                                                                                                                                                                                    gwlossfile.close()
        '                                                                                                                                                                                    outswfile.close()
        '                                                                                                                                                                                    excessfile.close()
        '                                                                                                                                                                                    interfile.close()
        '                                                                                                                                                                                    basefile.close()
        '                                                                                                                                                                                    massfile.close()
        '                                                                                                                                                                                    logfile.close()
        '                                                                                                                                                                                    initialfile.close()

        'plabels = { "Computation Start Year", "Computation Start Day", "Number of Rain/Evap Days", 
        '          "Number of Response Days", "Number of Subbasins", "Initial Soil Moisture", "Data Format (0=Hourly/1=Daily)", 
        '          "New Run (0) or Continue Previous Run(1)", "Basin Polygon Theme","Key Field eg Grid Code","Simulation(0) or Calibration(1) Mode"}

        'pdefaults = { startyr, startdy, runsize.asstring, resdays.asstring, bassize.asstring, "0.1", "1", "0", "basply.shp", "gridcode", "0"}

        '                                                                                                                                                                                    runpList = MsgBox.MultiInput("Enter Model Parameters.", "Geospatial Stream Flow Model", plabels, pdefaults)

        '                                                                                                                                                                                    If (runpList.IsEmpty) Then
        '  exit
        '                                                                                                                                                                                        End

        '                                                                                                                                                                                        startyr = runpList.get(0)
        '                                                                                                                                                                                        startdy = runpList.get(1)
        '                                                                                                                                                                                        runsize = runpList.get(2)
        '                                                                                                                                                                                        resdays = runpList.get(3)
        '                                                                                                                                                                                        bassize = runpList.get(4)
        '                                                                                                                                                                                        inifract = runpList.get(5)
        '                                                                                                                                                                                        dformat = runpList.get(6)
        '                                                                                                                                                                                        inimode = runpList.get(7)
        '                                                                                                                                                                                        basinthmname = runplist.get(8)
        '                                                                                                                                                                                        idfieldname = runplist.get(9)
        '                                                                                                                                                                                        runmode = runpList.get(10)

        '                                                                                                                                                                                        If (runmode.isnumber.not) Then
        '                                                                                                                                                                                            runmode = "0"
        '                                                                                                                                                                                        ElseIf ((runmode.asnumber < 0) Or (runmode.asnumber > 1)) Then
        '                                                                                                                                                                                            runmode = "0"
        '                                                                                                                                                                                            End

        '                                                                                                                                                                                            If (dformat.asstring.asnumber = 1) Then
        '                                                                                                                                                                                                tinterval = "24"
        '                                                                                                                                                                                            ElseIf (dformat.asstring.asnumber = 0) Then
        '                                                                                                                                                                                                tinterval = "1"
        '                                                                                                                                                                                            Else
        '                                                                                                                                                                                                tinterval = "24"
        '                                                                                                                                                                                                End

        '                                                                                                                                                                                                basinthm = theView.FindTheme(basinthmname)
        '                                                                                                                                                                                                If (basinthm = nil) Then
        '                                                                                                                                                                                                    basinthm = theView.FindTheme("basply.shp")
        '                                                                                                                                                                                                    basinthmname = "basply.shp"
        '                                                                                                                                                                                                    If (basinthm = nil) Then
        '                                                                                                                                                                                                        theViewthmlist = theView.Getthemes
        '                                                                                                                                                                                                        For Each vthm In theViewthmlist
        '                                                                                                                                                                                                            thethmnm = vthm.getname
        '                                                                                                                                                                                                            If ((thethmnm.contains("basply")) And (vthm.CanSelect)) Then
        '                                                                                                                                                                                                                basinthm = theView.FindTheme(thethmnm)
        '                                                                                                                                                                                                                basinthmname = thethmnm
        '                                                                                                                                                                                                                End
        '                                                                                                                                                                                                                End
        '                                                                                                                                                                                                                If (basinthm = nil) Then
        '                                                                                                                                                                                                                    basinthmname = "basply.shp"
        '                                                                                                                                                                                                                    MsgBox.info("Basin theme, " + basinthmname + " not found in the View." + nl + "Add the Basin Shapefile to the View before performing routing.", "Geospatial Stream Flow Model")
        '      exit
        '                                                                                                                                                                                                                    End
        '                                                                                                                                                                                                                    End
        '                                                                                                                                                                                                                    End

        '                                                                                                                                                                                                                    basinthm.clearselection()

        '                                                                                                                                                                                                                    basinVtab = basinthm.GetFtab
        '                                                                                                                                                                                                                    basinVtab.UnJoinall()
        '                                                                                                                                                                                                                    basinVtab.UnLinkall()
        '                                                                                                                                                                                                                    idfield = basinVtab.FindField(idfieldname)
        '                                                                                                                                                                                                                    nowfield = basinVtab.FindField("soilwater")
        '                                                                                                                                                                                                                    If (nowfield = nil) Then
        '                                                                                                                                                                                                                        basinVtab.SetEditable(True)
        '                                                                                                                                                                                                                        addlist = List.make
        '  fldadd = Field.Make("soilwater", #field_decimal, 10 , 1)
        '                                                                                                                                                                                                                        addlist.Add(fldadd)
        '                                                                                                                                                                                                                        basinVtab.Addfields(addlist)
        '                                                                                                                                                                                                                        basinVtab.SetEditable(False)
        '                                                                                                                                                                                                                        nowfield = basinVtab.FindField("soilwater")
        '                                                                                                                                                                                                                        End
        '                                                                                                                                                                                                                        basinVtab.flush()
        '                                                                                                                                                                                                                        basinVtab.refresh()

        '                                                                                                                                                                                                                        paramfile.write(resdays.asstring, resdays.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(runsize.asstring, runsize.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(startyr.asstring, startyr.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(startdy.asstring, startdy.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(bassize.asstring, bassize.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(tinterval.asstring, tinterval.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(dformat.asstring, dformat.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(inimode.asstring, inimode.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(inifract.asstring, inifract.asstring.count)
        '                                                                                                                                                                                                                        paramfile.writeelt(nl)
        '                                                                                                                                                                                                                        paramfile.write(runmode.asstring, runmode.asstring.count)
        '                                                                                                                                                                                                                        paramfile.close()

        '                                                                                                                                                                                                                        'timefile.writeelt(runsize.asstring)
        '                                                                                                                                                                                                                        'timefile.close

        '                                                                                                                                                                                                                        balancetype = "1"
        '                                                                                                                                                                                                                        routetype = "2"
        '                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                        routstr = "Muskingum-Cunge Routing"

        '                                                                                                                                                                                                                        If (File.Exists(whichmodelFN.AsFileName)) Then
        '  whichmodelfile = LineFile.Make((myWkDirname + "whichModel.txt").asfilename,#file_perm_read)
        '                                                                                                                                                                                                                            If (whichmodelfile = nil) Then
        '                                                                                                                                                                                                                                MsgBox.info("Could not create whichModel.txt file" + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '    exit
        '                                                                                                                                                                                                                                End
        '                                                                                                                                                                                                                                whichsize = (whichmodelfile.getsize)

        '                                                                                                                                                                                                                                biglist = List.make
        '                                                                                                                                                                                                                                If (whichsize > 2) Then
        '                                                                                                                                                                                                                                    whichmodelfile.Read(biglist, whichsize)
        '                                                                                                                                                                                                                                    balancetype = biglist.get(1).asstring.astokens(" ,/").get(0).asstring
        '                                                                                                                                                                                                                                    routetype = biglist.get(2).asstring.astokens(" ,/").get(0).asstring
        '                                                                                                                                                                                                                                Else
        '                                                                                                                                                                                                                                    balancetype = "1"
        '                                                                                                                                                                                                                                    routetype = "2"
        '                                                                                                                                                                                                                                    End

        '                                                                                                                                                                                                                                    whichmodelfile.close()

        '                                                                                                                                                                                                                                    If ((balancetype = "1") And (routetype = "3")) Then
        '                                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Simple Lag Routing Method"
        '                                                                                                                                                                                                                                    ElseIf ((balancetype = "1") And (routetype = "1")) Then
        '                                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Diffusion Analog Routing Method"
        '                                                                                                                                                                                                                                    ElseIf ((balancetype = "1") And (routetype = "2")) Then
        '                                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Muskingum-Cunge Routing"
        '                                                                                                                                                                                                                                    ElseIf ((balancetype = "2") And (routetype = "3")) Then
        '                                                                                                                                                                                                                                        balstr = "Non-Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Simple Lag Routing Method"
        '                                                                                                                                                                                                                                    ElseIf ((balancetype = "2") And (routetype = "1")) Then
        '                                                                                                                                                                                                                                        balstr = "Non-Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Diffusion Analog Routing Method"
        '                                                                                                                                                                                                                                    ElseIf ((balancetype = "2") And (routetype = "2")) Then
        '                                                                                                                                                                                                                                        balstr = "Non-Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Muskingum Cunge Routing Method"
        '                                                                                                                                                                                                                                    Else
        '                                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Muskingum-Cunge Routing"
        '                                                                                                                                                                                                                                        End
        ' ballst = {balstr, "Linear Soil Model", "Non-Linear Soil Model" }
        'else
        '  ballst = {"Linear Soil Model", "Non-Linear Soil Model" }
        '                                                                                                                                                                                                                                        balstr = "Linear Soil Model"
        '                                                                                                                                                                                                                                        routstr = "Muskingum-Cunge Routing"
        '                                                                                                                                                                                                                                        End

        'whichmodelfile = LineFile.Make(whichmodelFN.asfilename,#file_perm_write)
        '                                                                                                                                                                                                                                        If (whichmodelfile = nil) Then
        '                                                                                                                                                                                                                                            MsgBox.info("Could not create file, " + whichmodelFN + nl + "File may be open or tied up by another program", "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                                                                                            End

        '                                                                                                                                                                                                                                            balancestr = MsgBox.choiceasstring(ballst, "Select Flow Routing Method for Computation", "Geospatial Stream Flow Model")
        '                                                                                                                                                                                                                                            If (balancestr = NIL) Then
        '  exit
        '                                                                                                                                                                                                                                            Else
        '                                                                                                                                                                                                                                                If (balancestr = "Linear Soil Model") Then
        '                                                                                                                                                                                                                                                    balancetype = "1"
        '                                                                                                                                                                                                                                                ElseIf (balancestr = "Non-Linear Soil Model") Then
        '                                                                                                                                                                                                                                                    balancetype = "2"
        '                                                                                                                                                                                                                                                Else
        '                                                                                                                                                                                                                                                    balancetype = "1"
        '                                                                                                                                                                                                                                                    End

        '                                                                                                                                                                                                                                                    If (routstr = "Muskingum Cunge Routing Method") Then
        '                                                                                                                                                                                                                                                        routetype = "2"
        '                                                                                                                                                                                                                                                    ElseIf (routstr = "Simple Lag Routing Method") Then
        '                                                                                                                                                                                                                                                        routetype = "3"
        '                                                                                                                                                                                                                                                    ElseIf (routstr = "Diffusion Analog Routing Method") Then
        '                                                                                                                                                                                                                                                        routetype = "1"
        '                                                                                                                                                                                                                                                    Else
        '                                                                                                                                                                                                                                                        routetype = "2"
        '                                                                                                                                                                                                                                                        End

        '                                                                                                                                                                                                                                                        whichmodelfile.WriteElt("Model Index   Index description")
        '                                                                                                                                                                                                                                                        whichmodelfile.WriteElt(balancetype + " //water balance model:  1=1D balance, 2=2D balance")
        '                                                                                                                                                                                                                                                        whichmodelfile.WriteElt(routetype + " //routing model:  1=diffusion 2=Muskingum-Cunge 3=lag")
        '                                                                                                                                                                                                                                                        whichmodelfile.close()
        '                                                                                                                                                                                                                                                        End

        '                                                                                                                                                                                                                                                        myfilename = ("$AVEXT\geosfm.dll").AsFileName
        '                                                                                                                                                                                                                                                        If (myfilename = Nil) Then
        '                                                                                                                                                                                                                                                            MsgBox.info("Unable to locate the program file: geosfm.dll." + nl + nl + "Please install the program, balance.dll, before you continue.", "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                                                                                                            End
        '                                                                                                                                                                                                                                                            mydll = DLL.Make(myfilename)

        '                                                                                                                                                                                                                                                            If (balancestr = "Linear Soil Model") Then
        '  myproc=DLLProc.Make(myDLL, "ONELAYERBALANCE", #DLLPROC_TYPE_INT32,{#DLLPROC_TYPE_STR})
        '                                                                                                                                                                                                                                                                If (myproc = Nil) Then
        '                                                                                                                                                                                                                                                                    MsgBox.info("Unable to make the procedure, ONELAYERBALANCE" + nl + nl + "Please install the program, geosfm.dll, before you continue.", "FEWS SFM Model")
        '    exit
        '                                                                                                                                                                                                                                                                    End
        '                                                                                                                                                                                                                                                                Else
        '  myproc=DLLProc.Make(myDLL, "twolayerbalance", #DLLPROC_TYPE_INT32,{#DLLPROC_TYPE_STR})  
        '                                                                                                                                                                                                                                                                    If (myproc = Nil) Then
        '                                                                                                                                                                                                                                                                        MsgBox.info("Unable to make the procedure, twolayerbalance" + nl + nl + "Please install the program, geosfm.dll, before you continue.", "FEWS SFM Model")
        '    exit
        '                                                                                                                                                                                                                                                                        End
        '                                                                                                                                                                                                                                                                        End

        '                                                                                                                                                                                                                                                                        av.ShowStopButton()
        '                                                                                                                                                                                                                                                                        av.SetStatus(0)
        '                                                                                                                                                                                                                                                                        av.showmsg("Performing Soil Water Balance.......")

        '                                                                                                                                                                                                                                                                        'procGetLastError = 1
        'procGetLastError = myproc.call({balfilename})

        '                                                                                                                                                                                                                                                                        If (procGetLastError > 0) Then
        '                                                                                                                                                                                                                                                                            MsgBox.info("An error occurred during the balance computation." + nl + "Check for possible causes in  " + logfilename, "Geospatial Stream Flow Model")
        '  exit
        '                                                                                                                                                                                                                                                                            End

        '                                                                                                                                                                                                                                                                            If (runmode.asstring = "0") Then
        '  outswfile = LineFile.Make((outswfilename).asfilename,#file_perm_read)
        '                                                                                                                                                                                                                                                                                If (outswfile = nil) Then
        '                                                                                                                                                                                                                                                                                    MsgBox.info("Could not open current soil water file," + nl + outswfilename, "Geospatial Stream Flow Model")
        '    exit
        '                                                                                                                                                                                                                                                                                    End

        '                                                                                                                                                                                                                                                                                    GUIName = "Table"
        '                                                                                                                                                                                                                                                                                    outswfile.close()
        '                                                                                                                                                                                                                                                                                    oldout = theProject.finddoc("Final Soil Moisture")
        '                                                                                                                                                                                                                                                                                    If (oldout <> nil) Then
        '                                                                                                                                                                                                                                                                                        theproject.removedoc(oldout)
        '                                                                                                                                                                                                                                                                                        End

        '                                                                                                                                                                                                                                                                                        outswVtab = Vtab.make((outswfilename).asfilename, False, False)
        '                                                                                                                                                                                                                                                                                        t = Table.MakeWithGUI(outswVtab, GUIName)
        '                                                                                                                                                                                                                                                                                        t.SetName("Final Soil Moisture")
        '                                                                                                                                                                                                                                                                                        t.GetWin.Open()

        '                                                                                                                                                                                                                                                                                        swfldlst = outswVtab.getfields
        '                                                                                                                                                                                                                                                                                        swcount = swfldlst.count
        '                                                                                                                                                                                                                                                                                        outswfld = swfldlst.get(swcount - 1)
        '                                                                                                                                                                                                                                                                                        outidfld = outswVtab.findfield("basinid")
        '                                                                                                                                                                                                                                                                                        basinVtab.join(idfield, outswVtab, outidfld)
        '                                                                                                                                                                                                                                                                                        basinVtab.SetEditable(True)
        '                                                                                                                                                                                                                                                                                        basinVtab.Calculate("[" + outswfld.getname.asstring + "]", nowfield)
        '                                                                                                                                                                                                                                                                                        basinVtab.SetEditable(False)
        '                                                                                                                                                                                                                                                                                        basinVtab.flush()
        '                                                                                                                                                                                                                                                                                        basinVtab.refresh()

        '  mylegend = legend.make(#SYMBOL_FILL)
        '  mylegend.setlegendtype(#LEGEND_TYPE_COLOR)
        '  mysymbol1 = symbol.make(#SYMBOL_FILL)
        '  mysymbol2 = symbol.make(#SYMBOL_FILL)
        '  mysymbol3 = symbol.make(#SYMBOL_FILL)
        '                                                                                                                                                                                                                                                                                        myyellow = Color.make
        '                                                                                                                                                                                                                                                                                        mygreen = Color.make
        '                                                                                                                                                                                                                                                                                        myblue = Color.make
        '  myyellow.setrgblist({250, 250, 0}) 
        '  mygreen.setrgblist({0, 240, 0})
        '  myblue.setrgblist({0, 0, 250})
        '                                                                                                                                                                                                                                                                                        mysymbol1.SetColor(myyellow)
        '                                                                                                                                                                                                                                                                                        mysymbol2.SetColor(mygreen)
        '                                                                                                                                                                                                                                                                                        mysymbol3.SetColor(myblue)
        '                                                                                                                                                                                                                                                                                        mylegend.Interval(Basinthm, nowfield.getname.asstring, 3)
        '  mylegend.SetClassInfo(0, {"dry (0 - 50)", "1", mySymbol1, 0.0, 50.0})
        '  mylegend.SetClassInfo(1, {"moist (50 - 100)", "2", mySymbol2, 50.0, 100.0})
        '  mylegend.SetClassInfo(2, {"wet (> 100mm)", "3", mySymbol3, 100.0, 1000.0})
        '                                                                                                                                                                                                                                                                                        mylegendFN = filename.Merge(myWkDirname, "soilwater.avl")
        '                                                                                                                                                                                                                                                                                        mylegend.Save(mylegendFN)
        '                                                                                                                                                                                                                                                                                        Basinthm.SetLegend(mylegend)
        '                                                                                                                                                                                                                                                                                        Basinthm.UpdateLegend()
        '                                                                                                                                                                                                                                                                                        Basinthm.SetVisible(True)
        '                                                                                                                                                                                                                                                                                        theView.invalidate()
        '                                                                                                                                                                                                                                                                                        End

        '                                                                                                                                                                                                                                                                                        av.ClearStatus()
        '                                                                                                                                                                                                                                                                                        av.clearmsg()

        '                                                                                                                                                                                                                                                                                        myfilename = nil
        '                                                                                                                                                                                                                                                                                        mydll = nil
        '                                                                                                                                                                                                                                                                                        myproc = Nil
        '                                                                                                                                                                                                                                                                                        av.PurgeObjects()
        '                                                                                                                                                                                                                                                                                        theFileName = theProject.GetFileName
        '                                                                                                                                                                                                                                                                                        If (theFileName = nil) Then
        '                                                                                                                                                                                                                                                                                            av.Run("Project.SaveAs", nil)
        '                                                                                                                                                                                                                                                                                            theFileName = theProject.GetFileName
        '                                                                                                                                                                                                                                                                                            If (theFileName = nil) Then
        '    exit
        '                                                                                                                                                                                                                                                                                                End
        '                                                                                                                                                                                                                                                                                            Else
        '                                                                                                                                                                                                                                                                                                If (av.Run("Project.CheckForEdits", nil).Not) Then
        '                                                                                                                                                                                                                                                                                                    Return nil
        '                                                                                                                                                                                                                                                                                                    End
        '                                                                                                                                                                                                                                                                                                    If (theProject.Save) Then
        '                                                                                                                                                                                                                                                                                                        av.ShowMsg("Project saved to '" + theFileName.GetBaseName + "'")
        '    if (System.GetOS = #SYSTEM_OS_MAC) then
        '                                                                                                                                                                                                                                                                                                            Script.Make("MacClass.SetDocInfo(SELF, Project)").DoIt(theFileName)
        '                                                                                                                                                                                                                                                                                                            End
        '                                                                                                                                                                                                                                                                                                            End
        '                                                                                                                                                                                                                                                                                                            End

        '                                                                                                                                                                                                                                                                                                            MsgBox.info("Soil Water Balance Complete. Results written to:" + nl + surpfilename, "Geospatial Stream Flow Model")

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

    'subs below are new GIS routines needed for GeoSFM

    Friend Sub GridFromShapefile(ByVal aShapefileLayerIndex As Integer, ByVal aBaseGridFileName As String, ByVal aNewGridFileName As String)
        'given a shapefile (subbasins or streams) and a base grid, return a grid version of those shapes

        'only used if streams and subbasins are entered as shapefiles
    End Sub

    Friend Class StationDetails
        Public Name As String
        Public StartJDate As Double
        Public EndJDate As Double
        Public Description As String
    End Class

End Module
