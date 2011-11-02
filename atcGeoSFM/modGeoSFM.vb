Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
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
        GisUtil.GridSetNoData(lDEMFileName)

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
        'this is more complicated
        'we don't have to do this exactly, but we need to know which subbasin is downstream of each one
    End Sub

    Friend Sub Basin(ByVal zonegname As String, ByVal demgname As String, ByVal facgname As String, ByVal hlengname As String, ByVal rcngname As String, _
                     ByVal whcgname As String, ByVal depthgname As String, ByVal texturegname As String, ByVal draingname As String, _
                     ByVal flowlengname As String, ByVal rivlinkgname As String, ByVal downgname As String, ByVal maxcovergname As String)

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

        Dim basingthm As Integer = -1
        If zonegname = "<none>" Then
            Logger.Msg("Basin Grid, " + zonegname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            basingthm = GisUtil.LayerIndex(zonegname)
        End If

        Dim demgthm As Integer = -1
        If demgname = "<none>" Then
            Logger.Msg("DEM Grid, " + demgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            demgthm = GisUtil.LayerIndex(demgname)
        End If

        Dim facgthm As Integer = -1
        If facgname = "<none>" Then
            Logger.Msg("Flow Accumulation Grid, " + facgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            facgthm = GisUtil.LayerIndex(facgname)
        End If

        Dim hlengthm As Integer = -1
        If hlengname = "<none>" Then
            Logger.Msg("Hill Length Grid, " + hlengname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            hlengthm = GisUtil.LayerIndex(hlengname)
        End If

        Dim rcngthm As Integer = -1
        If rcngname = "<none>" Then
            Logger.Msg("Runoff Curve Number Grid, " + rcngname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            rcngthm = GisUtil.LayerIndex(rcngname)
        End If

        Dim whcgthm As Integer = -1
        If whcgname = "<none>" Then
            Logger.Msg("Soil Water Holding Capacity Grid, " + whcgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            whcgthm = GisUtil.LayerIndex(whcgname)
        End If

        Dim depthgthm As Integer = -1
        If depthgname = "<none>" Then
            Logger.Msg("Soil Depth Grid, " + depthgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            depthgthm = GisUtil.LayerIndex(depthgname)
        End If

        Dim texturegthm As Integer = -1
        If texturegname = "<none>" Then
            Logger.Msg("Soil Texture Grid, " + texturegname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            texturegthm = GisUtil.LayerIndex(texturegname)
        End If

        Dim draingthm As Integer = -1
        If draingname = "<none>" Then
            Logger.Msg("Hydraulic Conductivity Grid, " + draingname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            draingthm = GisUtil.LayerIndex(draingname)
        End If

        Dim flowlengthm As Integer = -1
        If flowlengname = "<none>" Then
            Logger.Msg("Downstream Flow Length Grid, " + flowlengname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            flowlengthm = GisUtil.LayerIndex(flowlengname)
        End If

        Dim rivlinkgthm As Integer = -1
        If rivlinkgname = "<none>" Then
            Logger.Msg("Stream Link Grid, " + rivlinkgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            rivlinkgthm = GisUtil.LayerIndex(rivlinkgname)
        End If

        Dim downgthm As Integer = -1
        If downgname = "<none>" Then
            Logger.Msg("Downstream Basin Id Grid, " + downgname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            downgthm = GisUtil.LayerIndex(downgname)
        End If

        Dim maxcovergthm As Integer = -1
        If maxcovergname = "<none>" Then
            Logger.Msg("Maximum Impervious Cover Grid, " + maxcovergname + ", Not Found in the View", MsgBoxStyle.Critical, "")
            Exit Sub
        Else
            maxcovergthm = GisUtil.LayerIndex(maxcovergname)
        End If

        '        If (basingrid.GetVtab <> nil) Then
        '            basintable = basingrid.GetVtab
        '        ElseIf ((basingrid.GetVtab = nil) And (basingrid.IsInteger)) Then
        '            basinVtabTF = basingrid.buildvat
        '            If (basinVtabTF.IsTrue) Then
        '                basintable = basingrid.GetVtab
        '            Else
        '                MsgBox.error("Cannot open/create basin grid Value Attribute Table", "")
        '                Exit Sub
        '            End If
        'elseif ((basingrid.GetVtab = nil) AND (basingrid.IsInteger.Not)) then
        '                MsgBox.error("Basin grid is not an integer grid. Specify an integer basin grid.", "")
        '                Exit Sub
        '        End If

        '        thebitmap = basintable.GetSelection
        '        thebitmap.ClearAll()
        '        basintable.UpdateSelection()

        '        basinField = basinTable.FindField("Value")
        '        If (basinfield = nil) Then
        '            MsgBox.error("Field VALUE not found in basin grid Value Attribute Table", "")
        '            Exit Sub
        '        End If

        '        basincount = basinTable.FindField("Count")
        '        If (basincount = nil) Then
        '            MsgBox.error("Field COUNT not found in basin grid Value Attribute Table", "")
        '            Exit Sub
        '        End If

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

        '        For Each rec In basinTable
        '            cellcount = basinTable.returnValue(basincount, rec)
        '            If (cellcount = 1) Then
        '                MsgBox.report("Basin Grid contains single-cell subbasins" + nl + "for which zonalstatistics cannot be computed." + nl + nl + "Repeat delineation with higher stream definition" + nl + "threshold to eliminate this problem", "Error in Basin Grid")
        '                Exit Sub
        '            End
        '        End

        Logger.Status("Computing Zonal Statistics for " + demgname + "........")
        'find average value of demgthm for each unique value of basingthm
        Dim lDemZonalStats As New atcCollection
        lDemZonalStats = GisUtil.GridZonalStatistics(basingthm, demgthm)

        Logger.Status("Computing Zonal Statistics for " + facgname + "........")
        'FaczoneVTab = FacGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zFacFN.AsFileName)
        'find average value of facgthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + hlengname + "........")
        'HlenzoneVTab = HlenGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zHlenFN.AsFileName)
        'find average value of hlengthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + rcngname + "........")
        'RcnzoneVTab = RcnGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zRcnFN.AsFileName)
        'find average value of rcngthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + whcgname + "........")
        'WhczoneVTab = WhcGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zwhcFN.AsFileName)
        'find average value of whcgthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + depthgname + "........")
        'DepthzoneVTab = DepthGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zDepthFN.AsFileName)
        'find average value of depthgthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + texturegname + "........")
        'TexturezoneVTab = TextureGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zTextureFN.AsFileName)
        'find average value of texturegthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + draingname + "........")
        'DrainzoneVTab = DrainGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zDrainFN.AsFileName)
        'find average value of draingthm for each unique value of basingthm




        Logger.Status("Computing River Grid......")
        '                rivgrid = (basingrid * ((rivlinkgrid + 1) / (rivlinkgrid + 1))).Int
        '                rivVtab = rivgrid.GetVtab
        '                If (rivVtab = nil) Then
        '                    rivgrid.buildvat()
        '                    rivVtab = rivgrid.GetVtab
        '                    End
        '                    zonefield = rivVtab.Findfield("Value")




        Logger.Status("Computing Zonal Statistics for " + flowlengname + "........")
        'rivlenzoneVTab = flowlengrid.ZonalStatsTable(rivgrid, ThePrj, zoneField, False, zRivlenFN.AsFileName)
        'find average value of flowlengthm for each unique value of rivgrid

        Logger.Status("Computing Zonal Statistics for " + flowlengname + "........")
        'lengthzoneVTab = flowlengrid.ZonalStatsTable(basingrid, ThePrj, zoneField, False, zlengthFN.AsFileName)
        'find average value of flowlengthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for river cell elevations........")
        'rivdemzoneVTab = DemGrid.ZonalStatsTable(rivgrid, ThePrj, zoneField, False, zRivDemFN.AsFileName)
        'find average value of demgthm for each unique value of rivgrid

        Logger.Status("Computing Zonal Statistics for downstream basin ids........")
        'DownzoneVTab = DownGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zDownFN.AsFileName)
        'find average value of downgthm for each unique value of basingthm

        Logger.Status("Computing Zonal Statistics for " + maxcovergname + "........")
        'maxcoverzoneVTab = maxcoverGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, zmaxcoverFN.AsFileName)
        'find average value of maxcovergthm for each unique value of basingthm

        '                    areafield = DemZoneVtab.FindField("area")
        '                    demfield = DemZoneVtab.FindField("mean")
        '                    demminfield = DemZoneVtab.FindField("min")
        '                    lenfield = LengthZoneVtab.FindField("mean")
        '                    lenminfield = LengthZoneVtab.FindField("min")
        '                    facfield = FacZoneVtab.FindField("max")
        '                    hlenfield = HlenZoneVtab.FindField("mean")
        '                    rcnfield = RcnZoneVtab.FindField("mean")
        '                    whcfield = whcZoneVtab.FindField("mean")
        '                    depthfield = DepthZoneVtab.FindField("mean")
        '                    texturefield = TextureZoneVtab.FindField("majority")
        '                    drainfield = DrainZoneVtab.FindField("mean")
        '                    rivlenfield = rivlenZoneVtab.FindField("range")
        '                    rivdemfield = RivDemZoneVtab.FindField("range")
        '                    Downfield = DownZoneVtab.FindField("majority")
        '                    maxcoverfield = MaxCoverZoneVtab.FindField("mean")

        '                    'Sort basins in order of ascending drainage area
        '                    'This ensures that upstream basins are listed before downstream ones

        '                    srtlist = List.make
        '                    For Each brecord In BasinTable
        '                        basinvalue = (BasinTable.ReturnValue(basinfield, brecord)).AsString
        '                        facvalue = (FacZoneVtab.ReturnValue(facfield, brecord)).setformat("d").AsString
        '                        faccount = (facfield.getwidth) - (facvalue.count)
        '                        If (faccount > 0) Then
        '    for each fnum in 0..faccount
        '                                facvalue = "0" + facvalue
        '                                End If 
        '                        End If 
        '                srtstring = facvalue + "xxx" + brecord.asstring
        '                srtlist.Add(srtstring)
        '            End If
        '            srtlist.sort(False)


        'find unique values from the basins grid
        Dim lUniqueBasinValues As New atcCollection
        'GisUtil.GridUniqueValues(basingthm, lUniqueBasinValues)
        'may also need counts and hasdam fields?

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

        For Each basinvalue As Integer In lUniqueBasinValues
            'jstring = srtlist.get(srtnum)
            'rrecord = jstring.astokens("xxx").get(1).asnumber

            'DemZoneVtab
            Dim areavalue As Single = 0 '((DemZoneVtab.ReturnValue(areafield, rrecord)) / (1000000.0)).SetFormat("d.d").AsString
            Dim demvalue As Single = 0 '(DemZoneVtab.ReturnValue(demfield, rrecord)).SetFormat("d.d").AsString
            Dim avgdrop As Single = 0 '(DemZoneVtab.ReturnValue(Demfield, rrecord)) - (DemZoneVtab.ReturnValue(Demminfield, rrecord))

            'FacZoneVtab
            Dim facvalue As Single = 0 '(FacZoneVtab.ReturnValue(facfield, rrecord)).AsString

            'HlenZoneVtab
            Dim hlenvalue As Single = 0 '(HlenZoneVtab.ReturnValue(hlenfield, rrecord)).SetFormat("d.d").AsString
            'If (hlenvalue.isnumber.not) Then
            '    hlenvalue = basingrid.GetCellSize
            'End If
            'If (hlenvalue < basingrid.GetCellSize) Then
            '    hlenvalue = basingrid.GetCellSize
            'End If

            'LengthZoneVtab
            Dim avglength As Single = 0 '(LengthZoneVtab.ReturnValue(Lenfield, rrecord)) - (LengthZoneVtab.ReturnValue(Lenminfield, rrecord))
            'If (avglength < basingrid.GetCellSize) Then
            '    avglength = basingrid.GetCellSize
            'End If

            'DrainZoneVtab
            Dim drainvalue As Single = 0 '(DrainZoneVtab.ReturnValue(drainfield, rrecord)).SetFormat("d.ddd").AsString

            Dim upareavalue As Single = 0 '((facvalue.asstring.asnumber * facgrid.GetCellSize * facgrid.GetCellSize) / 1000000.0).SetFormat("d.d").asstring

            Dim slopevalue As String = ((avgdrop * 100) / avglength).ToString  '(((avgdrop * 100) / avglength).Format("d.dddd")).AsString
            If Not IsNumeric(slopevalue) Then
                slopevalue = "0.0010"
            End If
            If CSng(slopevalue) < 0.001 Then
                slopevalue = "0.0010"
            End If

            'rivlenZoneVtab
            Dim riverlossvalue As String = "1.0"

            Dim rivlenvalue As String = "0.0" ' (rivlenZoneVtab.ReturnValue(rivlenfield, rrecord)).SetFormat("d.d").AsString
            If Not IsNumeric(rivlenvalue) Then
                'rivlenvalue = basingrid.GetCellSize.asstring
            End If
            'If CSng(rivlenvalue) < basingrid.GetCellSize Then
            ' rivlenvalue = basingrid.GetCellSize.asstring
            'End If

            ' Assume porosity of 0.439 for medium soil texture in computing max storage for the catchment
            ' Assume both sides of the river (2 * No. of River Cells) are draining at the saturated rate
            '  under the influence of the average head (= avgdrop) in the catchment
            ' Assume baseflow is 3 times as slow as interflow

            Dim interflowlag As String = "0" '(((areavalue) * (avgdrop) * basingrid.GetCellSize * 0.439) / (rivlenvalue * avgdrop * drainvalue * 0.24 * 2)).tostring
            If Not IsNumeric(interflowlag) Then
                interflowlag = "10"
            ElseIf CStr(interflowlag) < 2 Then
                interflowlag = "2"
            ElseIf CStr(interflowlag) > 120 Then
                interflowlag = "120"
            End If

            Dim baseflowlag As String = (CStr(interflowlag) * 3).ToString

            'RcnZoneVtab
            Dim rcnvalue As String = "0" '(RcnZoneVtab.ReturnValue(Rcnfield, rrecord)).SetFormat("d.d").AsString

            'whcZoneVtab
            Dim whcvalue As String = "0" '(whcZoneVtab.ReturnValue(Whcfield, rrecord)).AsString

            'DepthZoneVtab
            Dim depthvalue As String = "0" '(DepthZoneVtab.ReturnValue(depthfield, rrecord)).AsString

            'TextureZoneVtab
            Dim texturevalue As String = "0" '(TextureZoneVtab.ReturnValue(texturefield, rrecord)).AsString

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
            Dim rivdropvalue As Single = 0.0 '(rivdemZoneVtab.ReturnValue(rivdemfield, rrecord))
            Dim rivslopevalue As String = ((rivdropvalue * 100) / CStr(rivlenvalue)).ToString   '(((rivdropvalue * 100) / CStr(rivlenvalue)).SetFormat("d.dddd")).AsString
            If Not IsNumeric(rivslopevalue) Then
                rivslopevalue = "0.0010"
            End If
            If CStr(rivslopevalue) < 0.001 Then
                rivslopevalue = "0.0010"
            End If

            Dim celerity As String = ""
            Dim diffusion As String = ""
            If CSng(rivslopevalue) < 0.1 Then

                If CSng(upareavalue) <= 10000 Then
                    celerity = "0.3"
                ElseIf CSng(upareavalue) <= 50000 Then
                    celerity = "0.45"
                ElseIf CSng(upareavalue) <= 100000 Then
                    celerity = "0.6"
                ElseIf CSng(upareavalue) <= 250000 Then
                    celerity = "0.75"
                ElseIf CSng(upareavalue) <= 500000 Then
                    celerity = "0.9"
                ElseIf CSng(upareavalue) <= 750000 Then
                    celerity = "1.2"
                Else
                    celerity = "1.5"
                End If

                diffusion = (0.15 * CSng(celerity) * CSng(rivlenvalue)).ToString  '(((0.15 * (CSng(celerity)) * (CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(diffusion) < 100.0) Then
                    diffusion = "100.0"
                ElseIf (CSng(diffusion) > 10000.0) Then
                    diffusion = "10000.0"
                End If

            ElseIf CSng(rivslopevalue) < 0.2 Then

                If (CSng(upareavalue) <= 10000) Then
                    celerity = "0.4"
                ElseIf (CSng(upareavalue) <= 50000) Then
                    celerity = "0.6"
                ElseIf (CSng(upareavalue) <= 100000) Then
                    celerity = "0.8"
                ElseIf (CSng(upareavalue) <= 250000) Then
                    celerity = "1.0"
                ElseIf (CSng(upareavalue) <= 500000) Then
                    celerity = "1.2"
                ElseIf (CSng(upareavalue) <= 750000) Then
                    celerity = "1.6"
                Else
                    celerity = "2.0"
                End If

                diffusion = (0.15 * CSng(celerity) * CSng(rivlenvalue)).ToString '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(diffusion) < 100.0) Then
                    diffusion = "100.0"
                ElseIf (CSng(diffusion) > 10000.0) Then
                    diffusion = "10000.0"
                End If

            ElseIf CSng(rivslopevalue) < 0.3 Then

                If (CSng(upareavalue) <= 10000) Then
                    celerity = "0.6"
                ElseIf (CSng(upareavalue) <= 50000) Then
                    celerity = "0.9"
                ElseIf (CSng(upareavalue) <= 100000) Then
                    celerity = "1.2"
                ElseIf (CSng(upareavalue) <= 250000) Then
                    celerity = "1.5"
                ElseIf (CSng(upareavalue) <= 500000) Then
                    celerity = "1.8"
                ElseIf (CSng(upareavalue) <= 750000) Then
                    celerity = "2.4"
                Else
                    celerity = "3.0"
                End If

                diffusion = (0.15 * CSng(celerity) * CSng(rivlenvalue)).ToString '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(diffusion) < 100.0) Then
                    diffusion = "100.0"
                ElseIf (CSng(diffusion) > 10000.0) Then
                    diffusion = "10000.0"
                End If

            ElseIf CSng(rivslopevalue) < 0.4 Then

                If (CSng(upareavalue) <= 10000) Then
                    celerity = "0.8"
                ElseIf (CSng(upareavalue) <= 50000) Then
                    celerity = "1.2"
                ElseIf (CSng(upareavalue) <= 100000) Then
                    celerity = "1.6"
                ElseIf (CSng(upareavalue) <= 250000) Then
                    celerity = "2.0"
                ElseIf (CSng(upareavalue) <= 500000) Then
                    celerity = "2.4"
                ElseIf (CSng(upareavalue) <= 750000) Then
                    celerity = "3.2"
                Else
                    celerity = "4.0"
                End If

                diffusion = (0.15 * CSng(celerity) * CSng(rivlenvalue)).ToString  '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(diffusion) < 100.0) Then
                    diffusion = "100.0"
                ElseIf (CSng(diffusion) > 10000.0) Then
                    diffusion = "10000.0"
                End If

            Else

                If (CSng(upareavalue) <= 10000) Then
                    celerity = "1.0"
                ElseIf (CSng(upareavalue) <= 50000) Then
                    celerity = "1.5"
                ElseIf (CSng(upareavalue) <= 100000) Then
                    celerity = "2.0"
                ElseIf (CSng(upareavalue) <= 250000) Then
                    celerity = "2.5"
                ElseIf (CSng(upareavalue) <= 500000) Then
                    celerity = "3.0"
                ElseIf (CSng(upareavalue) <= 750000) Then
                    celerity = "4.0"
                Else
                    celerity = "5.0"
                End If

                diffusion = (0.15 * CSng(celerity) * CSng(rivlenvalue)).ToString  '(((0.15 * CSng(celerity) * CSng(rivlenvalue)).SetFormat("d.d")).asstring)
                If (CSng(diffusion) < 100.0) Then
                    diffusion = "100.0"
                ElseIf (CSng(diffusion) > 10000.0) Then
                    diffusion = "10000.0"
                End If

            End If

            'DownZoneVtab
            Dim downvalue As String = "" '(DownZoneVtab.ReturnValue(downfield, rrecord)).AsString

            'MaxCoverZoneVtab
            Dim maxcovervalue As String = "" '((MaxCoverZoneVtab.ReturnValue(maxcoverfield, rrecord)) / 100).SetFormat("d.ddddd").AsString
            If CSng(maxcovervalue) <= 0.001 Then
                maxcovervalue = "0.001"
            ElseIf CSng(maxcovervalue) >= 1 Then
                maxcovervalue = "1.0"
            End If

            Dim hasdamvalue As String = "" '(BasinTable.ReturnValue(hasdamfld, rrecord)).SetFormat("d").AsString
            Dim rivpolyloss As String = "1.0"
            Dim hasrating As String = "0"
            Dim hasflowdata As String = "0"
            Dim rivwidth As String = (6.13 * (CSng(upareavalue) ^ (0.347))).ToString
            Dim flowref As String = (36 * 0.02832 * ((CSng(areavalue) / 2.59) ^ (0.68))).ToString
            Dim runtype As String = "0"
            Dim mannvalue As String = "0.035"
            Dim pancoef As String = "0.85"
            Dim topsoil As String = "0.1"
            Dim aridity As String = "2"
            lSBOut.AppendLine(basinvalue + "," + whcvalue + "," + depthvalue + "," + texturevalue + "," + drainvalue + "," + areavalue + "," + interflowlag + "," + slopevalue + "," + baseflowlag + "," + rcnvalue + "," + maxcovervalue + "," + basinlossvalue + "," + pancoef + "," + topsoil + "," + aridity)
            lSBRiv.AppendLine(basinvalue + "," + areavalue + "," + upareavalue + "," + rivslopevalue + "," + rivlenvalue + "," + downvalue + "," + mannvalue + "," + riverlossvalue + "," + rivpolyloss + "," + hasdamvalue + "," + hasrating + "," + hasflowdata + "," + celerity + "," + diffusion + "," + rivwidth + "," + flowref + "," + runtype)
            lSBOrder.AppendLine(basinvalue)
        Next

        'outVtab = Vtab.Make(outfile.GetFileName, False, False)

        SaveFileString(lOutFile, lSBOut.ToString)
        SaveFileString(lRivFile, lSBRiv.ToString)
        SaveFileString(lOrderFile, lSBOrder.ToString)

        IO.File.Copy(lOutFile, lOutputPath & "basin_original.txt")
        IO.File.Copy(lRivFile, lOutputPath & "river_original.txt")

        Logger.Msg("Basin Characteristics Computed. Outputs written to: " & vbCrLf & vbCrLf & "      " & lOutFile & vbCrLf & "      " & lOrderFile & vbCrLf & "      " & lRivFile, "Geospatial Stream Flow Model")

    End Sub

    Friend Sub Response()

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




        ' choose overland velocity computation method
        'velmethodlist = { "Non-Uniform from USGS Land Cover Grid" , "Non-Uniform with User Supplied Velocities", "Uniform from User Supplied Velocity Value" }

        'velmethod = MsgBox.ChoiceAsString(velmethodlist, "Method of OverLand Flow Velocity Computation: ", "Geospatial Stream Flow Model")

        '        If (velmethod = "Non-Uniform from USGS Land Cover Grid") Then
        '            veltype = 1
        '            MsgBox.report("For Non-Uniform Velocity from USGS Land Cover Grid," + nl + "required inputs include:" + nl + nl + "(1)    Basin Grid" + nl + "(2)    Flow Direction Grid" + nl + "(3)    Flow Length Grid" + nl + "(4)    Stream Outlet Grid" + nl + "(5)    USGS Land Cover Grid" + nl + "(6)    Elevation Grid" + nl + "(7)    Flow Accumulation Grid" + nl + "(8)    Computational Order File (created along with basin file)", "Required Inputs")
        '            addlcgrid = MsgBox.YesNo("Would you like to add the USGS Land Cover grid to the view now?", "Geospatial Stream Flow Model", True)
        '            If (addlcgrid = True) Then
        '                av.Run("View.Add", "USGS Land Cover Grid")
        '            End If
        '            firstthm = theView.getThemes.get(0).getname
        '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Outlets", "Elevations", "Flowacc", "Usgslandcov", myWkdirname + "order.txt" }
        '            'labellist = { "Basin Grid" , "Flow Direction Grid", "Flow Length Grid", "Stream Outlet Grid", "Elevation Grid", "Flow Accumulation Grid", "USGS Land Cover Grid", "Computational Order File" } 
        '        ElseIf (velmethod = "Non-Uniform with User Supplied Velocities") Then
        '            veltype = 2
        '            MsgBox.report("For Non-Uniform Velocity with User Supplied Velocities, " + nl + "required inputs include:" + nl + nl + "(1)    Basin Grid" + nl + "(2)    Flow Direction Grid" + nl + "(3)    Flow Length Grid" + nl + "(4)    Flow Accumulation Grid" + nl + "(5)    Stream Outlet Grid" + nl + "(6)    Overland Flow Velocity Value" + nl + "(7)    Instream Flow Velocity Value" + nl + "(8)    Computational Order File (created along with basin file)", "Required Inputs")
        '            addvgrid = MsgBox.YesNo("Would you like to add any of the input grids to the view now?", "Geospatial Stream Flow Model", True)
        '            If (addvgrid = True) Then
        '                av.Run("View.Add", "Input Grids")
        '            End If
        '            firstthm = theView.getThemes.get(0).getname
        '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Flowacc", "Outlets", "0.05", "0.5", myWkdirname + "order.txt" }
        '            'labellist = { "Basin Grid" , "Flow Direction Grid", "Flow Length Grid", "Flow Accumulation Grid", "Stream Outlet Grid", "Overland Flow Velocity (m/s)", "Instream Flow Velocity (m/s)", "Computational Order File" }
        '            Else
        '                veltype = 3
        '                MsgBox.report("For Uniform Velocity with User Supplied Velocity Value, " + nl + "required inputs include:" + nl + nl + "(1)    Basin Grid" + nl + "(2)    Flow Direction Grid" + nl + "(3)    Flow Length Grid" + nl + "(4)    Stream Outlet Grid" + nl + "(5)    Overland Flow Velocity Value" + nl + "(6)    Computational Order File (created along with basin file)", "Required Inputs")
        '                addvgrid = MsgBox.YesNo("Would you like to add any of the input grids to the view now?", "Geospatial Stream Flow Model", True)
        '                If (addvgrid = True) Then
        '                    av.Run("View.Add", "Input Grids")
        '            End If
        '            'defaultlist = { "Basins" , "Flowdir", "Flowlen", "Outlets", "0.3", myWkdirname + "order.txt" }
        '            'labellist = { "Basin Grid" , "Flow Direction Grid", "Flow Length Grid", "Stream Outlet Grid", "Overland Flow Velocity (m/s)", "Computational Order File" }
        '        End If

        '        If (veltype = 1) Then
        '            zonegname = inputlist.get(0)
        '            flowdirgname = inputlist.get(1)
        '            flowlengname = inputlist.get(2)
        '            outgname = inputlist.get(3)
        '            demgname = inputlist.get(4)
        '            facgname = inputlist.get(5)
        '            lcovgname = inputlist.get(6)
        '            orderfilename = inputlist.get(7)
        '        ElseIf (veltype = 2) Then
        '            zonegname = inputlist.get(0)
        '            flowdirgname = inputlist.get(1)
        '            flowlengname = inputlist.get(2)
        '            facgname = inputlist.get(3)
        '            outgname = inputlist.get(4)
        '            overlandvelstr = inputlist.get(5)
        '            instreamvelstr = inputlist.get(6)
        '            orderfilename = inputlist.get(7)
        '        Else
        '            zonegname = inputlist.get(0)
        '            flowdirgname = inputlist.get(1)
        '            flowlengname = inputlist.get(2)
        '            outgname = inputlist.get(3)
        '            velstrname = inputlist.get(4)
        '            orderfilename = inputlist.get(5)
        '        End If

        '        basingthm = TheView.FindTheme(zonegname)
        '        flowdirgthm = TheView.FindTheme(flowdirgname)
        '        flowlengthm = TheView.FindTheme(flowlengname)
        '        outgthm = TheView.FindTheme(outgname)

        '        basingrid = basingthm.GetGrid
        '        flowdirgrid = flowdirgthm.GetGrid
        '        flowlengrid = flowlengthm.GetGrid
        '        outgrid = outgthm.GetGrid

        '        If (basingrid.GetVtab <> nil) Then
        '            basintable = basingrid.GetVtab
        '        ElseIf ((basingrid.GetVtab = nil) And (basingrid.IsInteger)) Then
        '            basinVtabTF = basingrid.buildvat
        '            If (basinVtabTF.IsTrue) Then
        '                basintable = basingrid.GetVtab
        '            Else
        '                MsgBox.error("Cannot open/create basin grid Value Attribute Table", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '        ElseIf ((basingrid.GetVtab = nil) And (basingrid.IsInteger.Not)) Then
        '            MsgBox.error("Basin grid is not an integer grid. Specify an integer basin grid.", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        thebitmap = basintable.GetSelection
        '        thebitmap.ClearAll()
        '        basintable.UpdateSelection()

        '        basinField = basinTable.FindField("Value")
        '        If (basinfield = nil) Then
        '            MsgBox.error("Field VALUE not found in basin grid Value Attribute Table", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        'orderfile = LineFile.Make((orderfilename).AsFileName, #FILE_PERM_READ)
        '        If (orderfile = nil) Then
        '            MsgBox.error("Cannot determine computational order." + nl + "Run 'Generate basin file' menu to create order.txt", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If
        '        ordlist = List.make
        '        ordsize = orderfile.getsize
        '        If (ordsize < 2) Then
        '            MsgBox.error("Cannot determine computational order." + nl + "Run 'Generate basin file' menu to create order.txt", "Geospatial Stream Flow Model")
        '            Exit Sub
        '        End If

        '        orderfile.read(ordlist, ordsize)
        '        If (ordlist.get(0).IsNumber.Not) Then
        '            ordstart = 1
        '        Else
        '            ordstart = 0
        '        End If

        '        zonelist = List.make
        'for each orec in ordstart..(ordsize - 1)
        '            ordnum = ordlist.get(orec).asstring.asnumber
        '            For Each zrec In basintable
        '                zonenum = basintable.returnvalue(basinfield, zrec)
        '                If (ordnum = zonenum) Then
        '                    zonelist.add(zrec.asstring.AsNumber)
        '                End If
        '            Next
        '        Next

        '        '--set the extent before extracting
        '        ae = theView.GetExtension(AnalysisEnvironment)
        'ae.SetExtent(#ANALYSISENV_VALUE,basingthm.ReturnExtent)
        'ae.SetCellSize(#ANALYSISENV_VALUE,basingrid.GetCellSize)
        '        ae.Activate()

        '        mycellsize = basingrid.GetCellSize

        '        If (veltype = 1) Then
        '            demgthm = TheView.FindTheme(demgname)
        '            If (demgthm = nil) Then
        '                MsgBox.error("Elevation Grid, " + demgname + ", Not Found in the View", "")
        '                Exit Sub
        '            End If

        '            If (demgthm.CanSelect) Then
        '                MsgBox.error(demgname + "is not a grid theme", "")
        '                Exit Sub
        '            End If

        '            facgthm = TheView.FindTheme(facgname)
        '            If (facgthm = nil) Then
        '                MsgBox.error("Flow Accumulation Grid, " + facgname + ", Not Found in the View", "")
        '                Exit Sub
        '            End If

        '            If (facgthm.CanSelect) Then
        '                MsgBox.error(facgname + "is not a grid theme", "")
        '                Exit Sub
        '            End If

        '            facgrid = facgthm.getgrid
        '            demgrid = demgthm.getgrid
        '  meandrop = demgrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false) 
        '  mindrop = demgrid.zonalStats(#GRID_STATYPE_MIN, basingrid, Prj.MakeNull, basinField, false)
        '            avgdrop = meandrop - mindrop
        '  avgraw = flowlengrid.zonalStats(#GRID_STATYPE_MEAN, basingrid, Prj.MakeNull, basinField, false)
        '            avglength = (avgraw < mycellsize).con(mycellsize.asgrid, avgraw)
        '            sloperaw = ((avgdrop * 100) / avglength)
        '            slopegrid = (sloperaw < 0.001).con(0.001.asgrid, sloperaw)

        '            lcovgthm = theView.findtheme(lcovgname)
        '            If (lcovgthm = nil) Then
        '                MsgBox.error(lcovgname + " not found in the active View", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            If (lcovgthm.CanSelect) Then
        '                MsgBox.error(lcovgname + "is not a grid theme", "")
        '                Exit Sub
        '            End If
        '            If (lcovgthm.getgrid.isinteger) Then
        '                lcovVtab = lcovgthm.GetVtab
        '            Else
        '                MsgBox.Error(lcovgthm.GetName + " is not an integer grid" + nl + "USGS Land Cover Grid must be an integer grid", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            If (lcovVtab = nil) Then
        '                MsgBox.Error("Cannot open value attribute table for " + lcovgthm.GetName, "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            lcovtempfld = lcovVtab.findfield("lu_code")
        '            If (lcovtempfld = nil) Then
        '                MsgBox.Error("Cannot find field 'lu_code' in land cover grid, " + lcovgthm.GetName, "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '            lufield = lcovtempfld.clone
        '            If (lcovVtab.CanEdit.Not) Then
        '                lcgridtemp = lcovgthm.getgrid.lookup("lu_code")
        '                lcovVtab = lcgridtemp.getVtab
        '                lcovVtab.seteditable(True)
        '    lcovVtab.addfields({lufield})
        '                lcovgthm = Gtheme.Make(lcgridtemp)
        '            End If

        '            Mannlist = { "0.03","0.03","0.035","0.033","0.035","0.04","0.05","0.05","0.05","0.06","0.1","0.1","0.12","0.12","0.1","0.035","0.05","0.05","0.03","0.05","0.05","0.05","0.04","0.04" }
        '            lcvaluelist = { 100,211,212,213,280,290,311,321,330,332,411,412,421,422,430,500,620,610,770,820,810,850,830,900 }
        '            lcnamelist = {"Urban and Built-Up Land", "Dryland Cropland and Pasture", "Irrigated Cropland and Pasture", "Mixed Dryland/Irrigated Cropland and Pasture", "Cropland/Grassland Mosaic", "Cropland/Woodland Mosaic", "Grassland", "Shrubland", "Mixed Shrubland/Grassland", "Savanna", "Deciduous Broadleaf Forest", "Deciduous Needleleaf Forest", "Evergreen Broadleaf Forest", "Evergreen Needleleaf Forest", "Mixed Forest", "Water Bodies", "Herbaceous Wetland", "Wooded Wetland", "Barren or Sparsely Vegetated", "Herbaceous Tundra", "Wooded Tundra", "Mixed Tundra", "Bare Ground Tundra", "Snow or Ice" }

        '            lccodelist = MsgBox.MultiInput("Specify Mannings (Velocity) Coefficients for each land cover", "Land Cover, Anderson Code, Manning's N", lcnamelist, Mannlist)
        '            If (lccodelist.isempty) Then
        '                Exit Sub
        '            End If
        '            lcfield = lcovVtab.FindField("lc_code".Lcase)
        '            If (lcfield = nil) Then
        '                lcfield = lcovVtab.FindField("lccode".Lcase)
        '                If (lcfield = nil) Then
        '                    lcfield = lcovVtab.FindField("lu_code".Lcase)
        '                    If (lcfield = nil) Then
        '                        lcfield = lcovVtab.FindField("lucode".Lcase)
        '                        If (lcfield = nil) Then
        '                            lcfldlst = lcovVtab.getfields.deepclone
        '                            lcfield = MsgBox.choiceasstring(lcfldlst, "Select the field containing the" + nl + "Anderson Land Cover Classification Code eg lc_code", "Geospatial Stream Flow Model")
        '                            If (lcfield = nil) Then
        '                                Exit Sub
        '                            End If
        '                        End If
        '                    End If
        '                End If
        '            End If

        '            '  lcfile = LineFile.Make((myWkdirname + "roughness.txt").AsFileName, #FILE_PERM_WRITE)
        '            '  if (lcfile = nil) then
        '            '   msgbox.error("Cannot create roughness.txt file"+nl+"File may be open or held up by another program", "Geospatial Stream Flow Model")
        '            '   exit
        '            '  end

        '            If (lcovVtab.CanEdit.Not) Then
        '                MsgBox.info("Cannot Edit USGS Land Cover Grid." + nl + "Copy it to a location where you have write access.", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            Else
        '                lcovVtab.seteditable(True)
        '    mannField = Field.Make("ManningN", #FIELD_DOUBLE, 10, 5)  
        '    lcovVtab.AddFields({mannfield})

        '                testlocate = 0

        '                For Each lrec In lcovVtab
        '                    lcvalue = (lcovVtab.ReturnValue(lcfield, lrec))
        '                    If (lcvalue = 0) Then
        '                        lcovVtab.setvalue(mannfield, lrec, 0.05)
        '                    Else
        '                        lstlocate = lcvaluelist.findbyvalue(lcvalue)
        '                        If ((lstlocate = -1) And (testlocate = 0)) Then
        '                            MsgBox.info("Anderson Code of " + lcvalue.asstring + " is not supported in the USGS Land Cover Grid" + nl + "Defaulting to Mannings n of 0.05", "Geospatial Stream Flow Model")
        '                            testlocate = 1
        '                            mannvalue = 0.05
        '                        ElseIf ((lstlocate = -1) And (testlocate = 1)) Then
        '                            mannvalue = 0.05
        '                        Else
        '                            mannvalue = lccodelist.get(lstlocate).asnumber
        '                        End If
        '                        lcovVtab.setvalue(mannfield, lrec, Mannvalue)
        '                    End If
        '                Next

        '                lcovgrid = lcovgthm.getgrid
        '                manngrid = lcovgrid.lookup(mannfield.getname)
        '                ' from Manning's equation, v = (1/n)(R^(2/3)(S^0.5) = K (S^0.5)
        '                ' for Flowacc = 0-1000, R = 0.002,             k = (1/n)(0.002^(2/3)      = 0.0158740/n
        '                ' for Flowacc = 1000-2000, R = 0.005,             k = (1/n)(0.005^(2/3)      = 0.0292402/n
        '                ' for Flowacc = 2000-3000, R = 0.01,             k = (1/n)(0.01^(2/3)      = 0.0464159/n
        '                ' for Flowacc = 3000-4000, R = 0.02,             k = (1/n)(0.02^(2/3)      = 0.0736806/n   
        '                ' for Flowacc = 4000-5000, R = 0.05,             k = (1/n)(0.05^(2/3)      = 0.1357209/n
        '                ' for Flowacc = 5000-10000,              vel = 0.3
        '                ' for Flowacc = 10000-50000,             vel = 0.45
        '                ' for Flowacc = 50000-100000,            vel = 0.6
        '                ' for Flowacc = 100000-250000,           vel = 0.75
        '                ' for Flowacc = 250000-500000,           vel = 0.9
        '                ' for Flowacc = 500000-750000,           vel = 1.2
        '                ' for Flowacc = > 750000,                vel = 1.5

        '                velgrid1 = ((0.1357209).AsGrid / manngrid) * (slopegrid ^ 0.5)
        '                velgrid2 = ((0.0736806).AsGrid / manngrid) * (slopegrid ^ 0.5)
        '                velgrid3 = ((0.0464159).AsGrid / manngrid) * (slopegrid ^ 0.5)
        '                velgrid4 = ((0.0292402).AsGrid / manngrid) * (slopegrid ^ 0.5)
        '                velgrid5 = ((0.015874).AsGrid / manngrid) * (slopegrid ^ 0.5)
        '                velgrid6 = (0.3).asgrid
        '                velgrid7 = (0.45).asgrid
        '                velgrid8 = (0.6).asgrid
        '                velgrid9 = (0.75).asgrid
        '                velgrid10 = (0.9).asgrid
        '                velgrid11 = (1.2).asgrid
        '                velgrid12 = (1.5).asgrid

        '                velgridraw = (facgrid <= 1000).con(velgrid1, ((facgrid <= 2000).con(velgrid2, ((facgrid <= 3000).con(velgrid3, ((facgrid <= 4000).con(velgrid4, ((facgrid <= 5000).con(velgrid5, ((facgrid <= 10000).con(velgrid6, ((facgrid <= 50000).con(velgrid7, ((facgrid <= 100000).con(velgrid8, ((facgrid <= 250000).con(velgrid9, ((facgrid <= 500000).con(velgrid10, ((facgrid <= 750000).con(velgrid11, velgrid12)))))))))))))))))))))
        '                velgrid = (velgridraw < 0.01).con((0.01).asgrid, (velgridraw > 1.5).con((1.5).asgrid, velgridraw))

        '                If (file.exists((myWkDirname + "velocity").AsFileName)) Then
        '                    grid.DeleteDataset((myWkDirname + "velocity").AsFileName)
        '                End If

        '                velgthm = Gtheme.Make(velgrid)

        '            End If
        '        ElseIf (veltype = 2) Then
        '            facgthm = TheView.FindTheme(facgname)
        '            If (facgthm = nil) Then
        '                MsgBox.error("Flow Accumulation Grid, " + facgname + ", Not Found in the View", "")
        '                Exit Sub
        '            End If

        '            If (facgthm.CanSelect) Then
        '                MsgBox.error(facgname + "is not a grid theme", "")
        '                Exit Sub
        '            End If

        '            facgrid = facgthm.getgrid

        '            If (overlandvelstr.isnumber) Then
        '                overlandvel = overlandvelstr.asnumber
        '            Else
        '                MsgBox.info("Overland velocity, " + overlandvelstr + " must be a number", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            If (instreamvelstr.isnumber) Then
        '                instreamvel = instreamvelstr.asnumber
        '            Else
        '                MsgBox.info("Instream velocity, " + instreamvelstr + " must be a number", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If

        '            velgrid = (facgrid < 1000).con((overlandvel).asgrid, (instreamvel).asgrid)
        '            velgthm = Gtheme.Make(velgrid)
        '        Else
        '            velgrid1 = velstrname.asnumber.asgrid
        '            velgrid = (velgrid1 < 0.01).con((0.01).asgrid, velgrid1)
        '            velgthm = Gtheme.Make(velgrid)
        '        End If

        '        velgthm.SetName("velocity")
        '        If (file.exists((myWkDirname + "velocity").AsFileName).not) Then
        '            velgrid.SaveDataset((myWkDirname + "velocity").AsFileName)
        '        End If

        '        maskgrid = ((outgrid.isnull) / (outgrid.isnull))
        '        newfdrgrid = flowdirgrid * maskgrid

        '        numsubs = basinTable.GetNumRecords
        '        invelgrid = (1.AsGrid / velgrid)
        '        flowtimegrid = newfdrgrid.flowlength(invelgrid, False)
        '        If (file.exists((myWkDirname + "traveltime").AsFileName)) Then
        '            grid.DeleteDataset((myWkDirname + "traveltime").AsFileName)
        '        End If

        '        daysgrid = (flowtimegrid / 86400).floor
        '        daysgthm = Gtheme.Make(daysgrid)
        '        daysgthm.SetName("traveltime")
        '        If (file.exists((myWkDirname + "traveltime").AsFileName).not) Then
        '            daysgrid.SaveDataset((myWkDirname + "traveltime").AsFileName)
        '        End If

        '        newdaysgrid = daysgrid * basingrid / basingrid
        '        numdays = newdaysgrid.GetStatistics.Get(1)

        '        ' This is a temporary solution that will be removed when
        '        ' the routing program can read a parameter file

        '        extralist = List.make
        '        If (numdays > 22) Then
        '            numdays = 22
        '            'else 
        '            '  numdays = 22
        '        End If

        '        basinlist = List.make
        '        newfile = Vtab.Makenew((myWkdirname + "response.dbf").AsFilename, dBase)
        'idField = Field.Make("BasinId", #FIELD_DOUBLE, 10, 0)  
        'newfile.AddFields({idfield})

        '        For Each bid In BasinTable
        '            basinid = BasinTable.returnvalue(basinfield, bid)
        '            basinlist.Add(basinid)
        '            addedrec = newfile.AddRecord
        '            newfile.seteditable(True)
        '            newfile.setvalue(idfield, addedrec, basinid)
        '        Next
        '        basinlist.deepclone()

        ' for each nday in 0..(numdays - 1)
        '            countergrid = (daysgrid = nday.asGrid).con(1.AsGrid, 0.AsGrid)
        '            counterFN = myWkDirname + "count" + nday.asstring + ".dbf"
        '            counterVTab = CounterGrid.ZonalStatsTable(basingrid, ThePrj, basinField, False, CounterFN.AsFileName)
        '            If (CounterVTab.HasError) Then
        '                Return NIL
        '                MsgBox.Error("Unable to write zonal statistics to " + counterFN, "")
        '                Exit Sub
        '            ElseIf (CounterVTab.GetNumRecords <> basintable.GetNumRecords) Then
        '                MsgBox.Error("Spatial Extent of counter grid" + +nday.asstring + +"less than Basin grid", "Geospatial Stream Flow Model")
        '                Exit Sub
        '            End If
        '   cdayfld = Field.Make("Day" + nday.asstring, #FIELD_FLOAT, 12, 6)
        '   newfile.Addfields({cdayfld})
        '            sumfield = CounterVtab.FindField("sum")
        '            countfield = CounterVtab.FindField("count")
        '            countidfld = CounterVtab.FindField("value")
        '            newfile.join(idfield, counterVtab, countidfld)
        '            newfile.calculate("[Sum]/[Count]", cdayfld)
        '            newfile.unjoinall()
        '            newfile.refresh()
        '            deleteFN = counterFN.AsFileName
        '            counterVtab.DeActivate()
        '            counterVtab = nil
        '            av.purgeObjects()
        '            File.delete(deleteFN)
        '        Next
        '        newfile.Flush()

        '        For Each brec In newfile
        '            dsum = 0
        '            for each exday in 0..(numdays - 1)
        '                sfld = newfile.FindField("Day" + exday.asstring)
        '                thenum = newfile.returnValue(sfld, brec)
        '                dsum = dsum + thenum
        '            Next
        '            If (dsum <> 0) Then
        '                dfactor = 1 / dsum
        '            Else
        '                dfactor = 1
        '            End If
        '            If (dfactor <> 1) Then
        '                for each eday in 0..(numdays - 1)
        '                    efld = newfile.FindField("Day" + eday.asstring)
        '                    enum = newfile.returnValue(efld, brec)
        '                    newfile.SetValue(efld, brec, (enum * dfactor))
        '                next 
        '            end if 
        '        next 

        ' newfile.seteditable(false)
        '        tempfile = newfile.export((myWkDirname + "tempfile.txt").AsFileName, Dtext, False)

        '        GUIName = "Table"
        '        t = Table.MakeWithGUI(newfile, GUIName)
        't.SetName(newfile.GetName)
        't.GetWin.Open

        'expfile = LineFile.Make((myWkDirname+"tempfile.txt").AsFileName, #FILE_PERM_MODIFY )
        'expfile.setscratch(true)
        'respfile = LineFile.Make((myWkDirname+"response.txt").AsFileName, #FILE_PERM_WRITE )

        '        expsize = expfile.GetSize
        'expfile.GoToBeg

        'for each lnum in 0..(expsize-1)  
        '  if (lnum = 0) then
        '    expfile.GoToBeg
        '  else
        '    recposition = zonelist.get(lnum-1)
        '    expfile.setpos(recposition+1) 
        '  end if 
        '  readstr = expfile.ReadElt.Asstring
        '  if (readstr = "nil") then
        '    break
        '  end if 
        '  newstr = readstr.Substitute(",", ", ")
        '  respfile.write({newstr}, 1)
        'next 

        'if (theView.FindTheme("velocity") <> nil) then
        '  theView.DeleteTheme(theView.FindTheme("velocity"))
        'endif 
        'if (theView.FindTheme("traveltime") <> nil) then
        '  theView.DeleteTheme(theView.FindTheme("traveltime"))
        'endif 

        'theView.AddTheme(velgthm)
        'theView.AddTheme(daysgthm)


        'GUIName = "Table"
        'oldout = theProject.finddoc("response.txt")
        'if (oldout <> nil) then
        '  theproject.removedoc(oldout)
        'end if 

        'olddbf = theProject.finddoc("response.dbf")
        'if (olddbf <> nil) then
        '  theproject.removedoc(olddbf)
        'end if 

        'resptable = Vtab.make((myWkDirname+"response.dbf").asfilename, false, false)
        't = Table.MakeWithGUI(resptable, GUIName)
        't.SetName("response.dbf")
        't.GetWin.Open

        'av.ClearStatus
        'av.clearmsg

        'orderfile.close
        '    'if (veltype = 1) then
        '    '  lcfile.close
        '    'end
        'respfile.close
        'expfile.close

        'msgbox.Info( "Basin Response Computed. Output file: " +nl+ mywkDirname+"response.txt", "Geospatial Stream Flow Model")

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

    'Friend Function DownstreamSubbasinIds(ByVal aSubbasinGridFileName As String, ByVal aStreamGridFileName As String) As atcCollection
    '    'given a grid of subbasin ids and a stream grid, figure out the downstream subbasin id
    '    DownstreamSubbasinIds = New atcCollection
    'End Function
End Module
