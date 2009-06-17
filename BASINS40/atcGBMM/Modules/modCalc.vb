Imports atcMwGisUtility

Module modCalc

    ''' <summary>
    ''' Compute required grids prior to running computational program
    ''' </summary>
    ''' <remarks></remarks>
    Friend Function SetupMercuryModel() As Boolean
        Try
            GisUtil.AddGroup(GroupName)
            LastErrorMsg = ""

            Dim TaskNum As Integer = 0
            Dim NumTasks As Integer = 16

            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Removing old layers...", TaskNum - 1, NumTasks) AndAlso RemoveLayers()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing flow paths...", TaskNum - 1, NumTasks) AndAlso CalcFlowPaths()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing land use grid...", TaskNum - 1, NumTasks) AndAlso CalcLandUse()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing curve numbers...", TaskNum - 1, NumTasks) AndAlso CalcCN()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing LS factors...", TaskNum - 1, NumTasks) AndAlso CalcLSFactor()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing roughness coefficients...", TaskNum - 1, NumTasks) AndAlso CalcRoughness()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing hydraulic radius...", TaskNum - 1, NumTasks) AndAlso CalcHydRad()) Then Return False
            'TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing lakes...", TaskNum - 1, NumTasks) AndAlso CalcLakeParms()) Then Return False
            'TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing lake raster...", TaskNum - 1, NumTasks) AndAlso CalcLakeRaster()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing travel times...", TaskNum - 1, NumTasks) AndAlso CalcTravelTimes()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing subbasin grid...", TaskNum - 1, NumTasks) AndAlso CalcSubbasin()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing theissan polygons...", TaskNum - 1, NumTasks) AndAlso CalcTheissan()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing point sources...", TaskNum - 1, NumTasks) AndAlso CalcPointSource()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Computing litter decomposition rates...", TaskNum - 1, NumTasks) AndAlso CalcLitter()) Then Return False
            TaskNum += 1 : If Not (ProgressForm.SetProgressOverall("Saving grids in ASCII format...", TaskNum - 1, NumTasks) AndAlso SaveGrids()) Then Return False

            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If LastErrorMsg() <> "" Then ErrorMsg(LastErrorMsg)
        End Try
    End Function

    Private Function RemoveLayers() As Boolean
        With Project.Grids
            If Not Project.ForceRebuild Then Return True
            .DEM.RemoveLayer()
            .DEMFilled.RemoveLayer()
            .FlowDir.RemoveLayer()
            .Slope.RemoveLayer()
            .FlowAccum.RemoveLayer()
            .FlowPath.RemoveLayer()
            .DistToStream.RemoveLayer()
            .DistToOutlet.RemoveLayer()
            .AvgSlopeOverland.RemoveLayer()
            .AvgSlopeStream.RemoveLayer()
            .LandUse.RemoveLayer()
            .Soils.RemoveLayer()
            .CNPerv.RemoveLayer()
            .CNImperv.RemoveLayer()
            .Roughness.RemoveLayer()
            .AvgRoughness.RemoveLayer()
            .HydRadius.RemoveLayer()
            .AvgHydRadius.RemoveLayer()
            .TravelOverland.RemoveLayer()
            .TravelStream.RemoveLayer()
            .TravelTotal.RemoveLayer()
            .Subbasins.RemoveLayer()
            .ClimateThiessan.RemoveLayer()
            .MercuryThiessan.RemoveLayer()
            '.MercuryWetDepo.RemoveLayer()
            '.MercuryDryDepo.RemoveLayer()
            '.MercurySoil.RemoveLayer()
            .MercuryLitter.RemoveLayer()
        End With
        With Project.Layers
            While GisUtil.IsLayerByFileName(.AssessPt.Filename)
                GisUtil.RemoveLayer(GisUtil.LayerIndex(.AssessPt.Filename))
            End While
        End With
        Return True
    End Function

    Private LookupThreshold As LookupDelegate = AddressOf Threshold

    ''' <summary>
    ''' Given accumulated area grid, set cell to NoDataValue (assumed at -1) if number of cells is less than threshold 
    ''' </summary>
    ''' <param name="NumCells">Number of upstream cells</param>
    Private Function Threshold(ByVal NumCells As Integer) As Double
        With Project
            If NumCells < .StreamThreshold / .CellAreaKm Then
                Return -1
            Else
                Return NumCells
            End If
        End With
    End Function

    'Private LookupConvertD8 As LookupDelegate = AddressOf ConvertD8

    '''' <summary>
    '''' Given flow direction grid, change from NHD/ArcGIS designation to that used by MapWinGeoProc
    '''' </summary>
    '''' <param name="FlowDir">Direction of flow out of grid cell</param>
    'Private Function ConvertD8(ByVal FlowDir As Integer) As Double
    '    '    'NHD (ArcGIS):
    '    '    '
    '    '    '32    64    128
    '    '    '16     x      1
    '    '    ' 8     4      2

    '    '    'Taudem:
    '    '    '
    '    '    ' 4     3      2
    '    '    ' 5     x      1
    '    '    ' 6     7      8
    '    Static AlreadyWarned As Boolean = False
    '    Select Case FlowDir
    '        Case 1 : Return 1
    '        Case 2 : Return 8
    '        Case 4 : Return 7
    '        Case 8 : Return 6
    '        Case 16 : Return 5
    '        Case 32 : Return 4
    '        Case 64 : Return 3
    '        Case 128 : Return 2
    '        Case Else
    '            If Not AlreadyWarned Then
    '                WarningMsg("Invalid value found in NHD/ArcGIS flow direction grid: {0}; expected only 1, 2, 4, 8, 16, 32, 64, or 128.", FlowDir)
    '                AlreadyWarned = True
    '            End If
    '            Return -1
    '    End Select
    'End Function

    Private Function MakeErrorMsg(ByVal GeneralMsg As String, ByVal tk As TKTAUDEMLib.TauDEM, ByVal Result As Integer) As String
        If Result = 0 Then
            Return GeneralMsg
        Else
            Return GeneralMsg & vbCr & vbCr & "Detailed error message was:" & vbCr & vbCr & tk.getErrorMsg(Result)
        End If
    End Function

    ''' <summary>
    ''' Calculate flow path and other DEM grids
    ''' </summary>
    Private Function CalcFlowPaths() As Boolean
        With Project.Grids

            If .DEM.IsUpToDate() AndAlso _
                .DEMFilled.IsUpToDate(.DEM.FileName) AndAlso _
                .FlowDir.IsUpToDate(.DEMFilled.FileName) AndAlso _
                .Slope.IsUpToDate(.DEMFilled.FileName) AndAlso _
                .FlowAccum.IsUpToDate(.DEM.FileName) AndAlso _
                .FlowPath.IsUpToDate(.DEM.FileName, .FlowAccum.FileName) AndAlso _
                .DistToStream.IsUpToDate(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName) AndAlso _
                .DistToOutlet.IsUpToDate(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName) AndAlso _
                .AvgSlopeOverland.IsUpToDate(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName) AndAlso _
                .AvgSlopeStream.IsUpToDate(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName) Then Return True

            Dim tk As New TKTAUDEMLib.TauDEM

            'delete all files that are about to be created
            'NOTE: bug in MapWindow: if you remove group, all layers stay locked and will get error messages

            If Not .DEM.Delete() AndAlso .DEMFilled.Delete() AndAlso .FlowDir.Delete() AndAlso .Slope.Delete() AndAlso .FlowAccum.Delete() AndAlso .FlowPath.Delete() AndAlso .DistToStream.Delete() AndAlso .DistToOutlet.Delete() AndAlso .AvgSlopeOverland.Delete() AndAlso .AvgSlopeStream.Delete() Then Return False

            Dim res As Integer 'result code

            'clip and filter DEM layer to make DEM grid used for computations

            If Not ProgressForm.SetProgress("Clipping and filtering DEM grid...", ProgressBarStyle.Marquee) Then Return False
            If Not CreateGrid(.DEM.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False
            If Not ResampleGrid(Project.Layers.DEM.Filename, .DEM.FileName, Project.Layers.Subbasins.Filename) Then Return False
            If Not .DEM.AddLayer Then Return False

            'create a pit-filled DEM (don't use flow path grid)

            If Not ProgressForm.SetProgress("Creating filled DEM...", ProgressBarStyle.Marquee) Then Return False
            If Not .DEMFilled.Delete Then Return False
            res = tk.Flood(.DEM.FileName, .DEMFilled.FileName, "", "", 0)
            If res <> 0 Then LastErrorMsg = MakeErrorMsg("Unable to create pit-filled DEM.", tk, res) : Return False
            If Not .DEMFilled.AddLayer Then Return False

            'create flow direction and slope grids

            If Not ProgressForm.SetProgress("Creating flow direction and slope grids...", ProgressBarStyle.Marquee) Then Return False

            If Not (.FlowDir.Delete AndAlso .Slope.Delete) Then Return False
            res = tk.D8(.DEMFilled.FileName, .FlowDir.FileName, .Slope.FileName, "", 0, True)
            If res <> 0 Then LastErrorMsg = MakeErrorMsg("Unable to create flow direction & slope grids.", tk, res) : Return False

            If Not .FlowDir.AddLayer() Then Return False
            If Not .Slope.AddLayer() Then Return False

            'note: slope will only be correct if horizontal distances the same as vertical distances; if not, multiply by correct factors

            If Not ProgressForm.SetProgress("Applying scale factors to slope grids...", ProgressBarStyle.Marquee) Then Return False
            If Not MultiplyGrid(.Slope.FileName, Project.DistFactor / Project.ElevFactor) Then Return False

            'create flow accumulation grid (number of upstream cells flowing to each cell)

            If Not ProgressForm.SetProgress("Computing flow accumulation grid...", ProgressBarStyle.Marquee) Then Return False
            If Not .FlowAccum.Delete Then Return False
            res = tk.Aread8(.FlowDir.FileName, .FlowAccum.FileName, 0, 0, 0, 1, "", 0, 0)
            If res <> 0 Then LastErrorMsg = MakeErrorMsg("Unable to create cumulative area grid.", tk, res) : Return False

            If Not .FlowAccum.AddLayer() Then Return False

            'create flow path grid by setting all cells having less than threshold accumulated area to NoData

            If Not ProgressForm.SetProgress("Creating stream flow path grids...", ProgressBarStyle.Marquee) Then Return False
            If Not LookupGrid(.FlowAccum.FileName, .FlowPath.FileName, LookupThreshold, MapWinGIS.GridDataType.LongDataType) Then Return False
            If Not .FlowPath.AddLayer(True) Then Return False
            ApplyColoringScheme(.FlowPath.LayerName, "Upland", System.Drawing.Color.Black, "Stream", System.Drawing.Color.Blue, Project.StreamThreshold)

            'create distance to streams grid (note: could also call following to flowaccum grid and specify threshold or use my own routine)

            If Not ProgressForm.SetProgress("Computing overland flow distance to streams...", ProgressBarStyle.Marquee) Then Return False
            If Not CreateGrid(.DistToStream.FileName) Then Return False
            res = tk.Distance(.FlowDir.FileName, .FlowAccum.FileName, .DistToStream.FileName, Project.StreamThreshold / Project.CellAreaKm, 0)
            If res <> 0 Then LastErrorMsg = MakeErrorMsg("Unable to compute distance to stream grid.", tk, res) : Return False

            'GBMM needs overland flow length; this MAY be the same as DistToStream
            Dim g As New MapWinGIS.Grid
            If Not g.Open(.DistToStream.FileName) Then Return False
            If Not g.Save(.FlowLength.FileName) Then Return False
            g.Close()

            If Not .DistToStream.AddLayer Then Return False

            'create stream distance to outlet grid 

            If Not ProgressForm.SetProgress("Computing stream flow distance to outlet...", ProgressBarStyle.Marquee) Then Return False
            If Not CalcDownstream(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName, .DistToOutlet.FileName, enumFlowOption.Stream, enumCalcOption.Distance) Then Return False
            If Not .DistToOutlet.AddLayer Then Return False

            'compute average slope to stream by summing slopes along paths and dividing by number of cells

            If Not ProgressForm.SetProgress("Computing average downstream overland slopes...", ProgressBarStyle.Marquee) Then Return False
            If Not CalcDownstream(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName, .AvgSlopeOverland.FileName, enumFlowOption.Overland, enumCalcOption.Average) Then Return False
            If Not .AvgSlopeOverland.AddLayer Then Return False

            'compute average slope on stream to outlet by summing slopes along paths and dividing by number of cells

            If Not ProgressForm.SetProgress("Computing average downstream stream slopes...", ProgressBarStyle.Marquee) Then Return False
            If Not CalcDownstream(.FlowDir.FileName, .Slope.FileName, .FlowPath.FileName, .AvgSlopeStream.FileName, enumFlowOption.Stream, enumCalcOption.Average) Then Return False
            If Not .AvgSlopeStream.AddLayer Then Return False

            Return True
        End With
    End Function

    '''' <summary>
    '''' Calculate overland flow distance to streams (and slopes)
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks>"Streams" are defined as cells with upstream contributing areas greater than specified threshold</remarks>
    'Private Function CalcOverlandDist() As Boolean
    '    With Project

    '        'validate input
    '        If .StreamThreshold = 0 Then WarningMsg("Stream Threshold must be set to a non-zero number.") : Return False
    '        If .Layers.DEM.Filename = "" Then WarningMsg("You must specify a valid DEM layer.") : Return False
    '        If .Layers.FlowAcc.Filename = "" Then WarningMsg("You must specify a valid Flow Accumulation layer.") : Return False
    '        If .Layers.FlowDir.Filename = "" Then WarningMsg("You must specify a valid Flow Direction layer.") : Return False

    '        Dim ThresholdCells As Integer = .StreamThreshold / .CellAreaKm

    '        'since I have DEM, I *should* be able to do all calcs I need to do using TauDEM, but it turns out that
    '        'in my testing that the flow accumulation grid has NoData values scattered about (even after using pit-filled DEM)
    '        'and as a result, the accumulated area function gives incorrect results

    '        'therefore, REQUIRE that the NHD flow direction and accumulated area grids be downloaded and used; only use
    '        'TauDEM functions to compute stream grid and overland slopes

    '        'create flow path grid; all cells are empty except those with areas greater than the specified threshold

    '        'must use DEM as template (when use subbasin layer, grids may not align perfectly

    '        'create flow path grid by setting all cells having less than threshold accumulated area to NoData

    '        If Not CreateGrid(.Grids.FlowPath.FileName, .Layers.DEM.Filename, MapWinGIS.GridDataType.LongDataType) Then Return False
    '        If Not LookupGrid(.Layers.FlowAcc.Filename, .Grids.FlowPath.FileName, LookupThreshold) Then Return False

    '        'see if flow direction grid is compatible with taudem distance routine (has 8 values); if not convert from NHD format

    '        Dim g As New MapWinGIS.Grid
    '        If Not g.Open(.Layers.FlowDir.Filename) Then Return False
    '        Dim NoDataValue As Object = g.Header.NodataValue
    '        g.Close()

    '        Select Case GisUtil.GridLayerMaximum(GisUtil.LayerIndex(.Layers.FlowDir.LayerName))
    '            Case 1 To 8, NoDataValue
    '                'probably OK
    '            Case 1, 2, 4, 8, 16, 32, 64, 128, NoDataValue
    '                'If Not LookupGrid(.Layers.FlowDir.Filename) Then Return False
    '                If Not LookupGrid(.Layers.FlowDir.Filename, .Layers.FlowDir.Filename, LookupConvertD8) Then Return False
    '                gMapWin.Refresh()
    '            Case Else
    '                WarningMsg("Unexpected values found in flow direction grid.")
    '                Exit Function
    '        End Select

    '        'create distance to streams grid

    '        If Not CreateGrid(.Grids.DistToStream.FileName, .Layers.DEM.Filename, MapWinGIS.GridDataType.FloatDataType) Then Return False
    '        Dim tk As New TKTAUDEMLib.TauDEM
    '        'note: could also call following to flowaccum grid and specify threshold
    '        If tk.Distance(.Layers.FlowDir.Filename, .Grids.FlowPath.FileName, .Grids.DistToStream.FileName, 0, 0) <> 0 Then Return False

    '        'create slope grid (will ignore flow direction grid since I'm already using the flow direction NHD layer

    '        If Not CreateGrid(.Grids.FlowDir.FileName, .Layers.DEM.Filename, MapWinGIS.GridDataType.LongDataType) Then Return False
    '        If Not CreateGrid(.Grids.Slope.FileName, .Layers.DEM.Filename, MapWinGIS.GridDataType.FloatDataType) Then Return False
    '        If tk.D8(.Grids.DEMFilled.FileName, .Grids.FlowDir.FileName, .Grids.Slope.FileName, "", 0, False) <> 0 Then Return False

    '        'note: slope will only be correct if horizontal distances the same as vertical distances; if not, multiply by correct factors

    '        If Not MultiplyGrid(.Grids.Slope.FileName, .DistFactor / .ElevFactor) Then Return False

    '        'create downstream flow lengths



    '        'add layers to project

    '        With .Grids
    '            If Not GisUtil.AddLayerToGroup(.Slope.FileName, .Slope.LayerName, GroupName) Then Return False
    '            If Not GisUtil.AddLayerToGroup(.FlowAccum.FileName, .FlowAccum.LayerName, GroupName) Then Return False
    '            If Not GisUtil.AddLayerToGroup(.DistToStream.FileName, .DistToStream.LayerName, GroupName) Then Return False
    '            If Not GisUtil.AddLayerToGroup(.FlowPath.FileName, .FlowPath.LayerName, GroupName) Then Return False
    '            ApplyColoringScheme(.FlowPath.LayerName, "Upland", System.Drawing.Color.LightYellow, "Stream", System.Drawing.Color.Blue, Project.StreamThreshold)
    '            GisUtil.LayerVisible(.FlowPath.LayerName) = True
    '        End With

    '        Return True
    '    End With
    'End Function

    ''' <summary>
    ''' Calculate landuse grid based on shapefile or grid selected for input
    ''' </summary>
    Private Function CalcLandUse() As Boolean
        Try
            With Project
                If .LanduseType = enumLandUseType.GIRAS Then
                    For Each idx As Integer In .GIRASLayers(GisUtil.LayerIndex(.Layers.Subbasins.LayerName))
                        If .Grids.LandUse.IsUpToDate(GisUtil.LayerFileName(idx)) Then Return True
                    Next
                Else
                    If .Grids.LandUse.IsUpToDate(.Layers.Landuse.Filename) Then Return True
                End If

                If Not CreateGrid(.Grids.LandUse.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False
                If .LanduseType = enumLandUseType.GIRAS Then
                    For Each idx As Integer In .GIRASLayers(GisUtil.LayerIndex(.Layers.Subbasins.LayerName))
                        If Not ShapefileToGrid(GisUtil.LayerFileName(idx), "LUCODE", .Grids.LandUse.FileName) Then Return False
                    Next
                    If Not FilterGrid(.Grids.LandUse.FileName, .Layers.Subbasins.Filename) Then Return False
                Else
                    If GisUtil.LayerType(.Layers.Landuse.LayerName) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                        If Not ShapefileToGrid(.Layers.Landuse.Filename, .Layers.Landuse.FieldName, .Grids.LandUse.FileName) Then Return False
                        If Not FilterGrid(.Grids.LandUse.FileName, .Layers.Subbasins.Filename) Then Return False
                    Else
                        If Not ResampleGrid(.Layers.Landuse.Filename, .Grids.LandUse.FileName, .Layers.Subbasins.Filename) Then Return False
                    End If
                End If
                If Not .Grids.LandUse.AddLayer() Then Return False
            End With

            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function

    ''' <summary>
    ''' Compute curve numbers from soils and land use layers and assign to grid
    ''' </summary>
    Private Function CalcCN() As Boolean

        With Project

            If .Grids.CNImperv.IsUpToDate(.Grids.DEM.FileName, .Grids.LandUse.FileName, .Layers.Soils.Filename, .Tables.Soils.Filename, .Tables.LandUse.Filename, .Tables.LandUseCN.Filename) AndAlso _
               .Grids.CNPerv.IsUpToDate(.Grids.DEM.FileName, .Grids.LandUse.FileName, .Layers.Soils.Filename, .Tables.Soils.Filename, .Tables.LandUse.Filename, .Tables.LandUseCN.Filename) AndAlso _
               .Grids.Soils.IsUpToDate(.Layers.Soils.Filename) AndAlso _
               .Soils.Count > 0 AndAlso _
               .Landuses.Count > 0 Then Return True

            'validate input
            If .Grids.DEM.FileName = "" Then WarningMsg("You must specify a valid DEM layer.") : Return False
            If .Layers.Subbasins.Filename = "" Then WarningMsg("You must specify a valid Subbasins layer.") : Return False
            If .Layers.Soils.Filename = "" Then WarningMsg("You must specify a valid Soils layer.") : Return False
            If .Tables.Soils.Filename = "" Then WarningMsg("You must specify a valid Soil Properties table.") : Return False
            If .Tables.LandUse.Filename = "" Then WarningMsg("You must specify a valid Land Use Properties table.") : Return False
            If .Tables.LandUseCN.Filename = "" Then WarningMsg("You must specify a valid Land Use Curve Number table.") : Return False


            'create destination grid, which will contain integer curve numbers (must be at same location and resolution as landuse grid)

            Dim sfSoils As MapWinGIS.Shapefile = Nothing
            Dim gSoils As MapWinGIS.Grid = Nothing
            Dim gLandUse As MapWinGIS.Grid = Nothing
            Dim gCNPerv As MapWinGIS.Grid = Nothing
            Dim gCnImperv As MapWinGIS.Grid = Nothing

            'delete grids about to be created
            If Not .Grids.CNImperv.Delete AndAlso .Grids.CNPerv.Delete() Then Return False

            Try

                'save soils shapefile as grid (must do in multiple steps because Statsgo shapefile has MUID string, need Soil ID integer from Soil Properties table)

                If Not CreateGrid(.Grids.Soils.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False
                gSoils = New MapWinGIS.Grid
                If Not gSoils.Open(.Grids.Soils.FileName) Then LastErrorMsg = "Unable to open grid: " & .Grids.Soils.FileName : Return False

                'open soils shapefile and landuse grid

                sfSoils = New MapWinGIS.Shapefile
                If Not sfSoils.Open(.Layers.Soils.Filename) Then LastErrorMsg = "Unable to open shapefile: " & .Layers.Soils.Filename : Return False

                ProgressForm.SetProgress("Creating CN grids...")

                If Not CreateGrid(.Grids.CNPerv.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False

                gCNPerv = New MapWinGIS.Grid
                If Not gCNPerv.Open(.Grids.CNPerv.FileName, , True) Then LastErrorMsg = "Unable to open grid: " & .Grids.CNPerv.FileName : Return False

                If Not CreateGrid(.Grids.CNImperv.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False

                gCnImperv = New MapWinGIS.Grid
                If Not gCnImperv.Open(.Grids.CNImperv.FileName, , True) Then LastErrorMsg = "Unable to open grid: " & .Grids.CNPerv.FileName : Return False

                gLandUse = New MapWinGIS.Grid
                If Not gLandUse.Open(.Grids.LandUse.FileName, , True) Then LastErrorMsg = "Unable to open grid: " & .Grids.CNPerv.FileName : Return False

                'ASSUMPTIONS:
                'soil layer has MUID field containing soil type like GA123 (or can be user-defined fieldname);
                'soil table has MUID2 field with same, and GROUPVALUE field with numeric value for Hyd. Soil Group (A=100, B=200, C=300, D=400)
                'landuse grid has 2 digit LUCODE field associated with each land use type (11-92)
                'landuse-cn table has LU_CNCODE field with sum of GROUPVALUE and LUCODE values (e.g., 340 is C soil, 40 landuse) and associated CN value

                ProgressForm.SetProgress("Computing CN grids...")

                Dim dt As DataTable

                .Soils.Clear()
                dt = LoadTable(.Tables.Soils.Filename)
                Try
                    For Each dr As DataRow In dt.Rows
                        Dim soil As New clsSoil(dr("VALUE"), dr("MUID2"), dr("AWC"), dr("BD"), dr("CLAYPERC"), dr("PERM"), dr("KFACT"), dr("GROUPVALUE"))
                        .Soils.Add(dr("MUID2"), soil)
                    Next
                Catch ex As Exception
                    ErrorMsg("Some required fields were missing from the Soil Properties table.")
                    Return False
                End Try
                dt.Dispose()

                'only want to save land uses that are actually encountered...
                Dim lstLandUses As Generic.List(Of Single) = GetGridValues(.Grids.LandUse.FileName)
                .Landuses.Clear()
                dt = LoadTable(.Tables.LandUse.Filename)
                Try
                    For Each dr As DataRow In dt.Rows
                        Dim landuse As New clsLandUse(dr("LUCODE"), dr("TYPE"), dr("GETCOVER"), dr("NGETCOVER"), dr("LUC"), dr("LUP"), dr("IMP_DCON"), dr("IMP_TOT"), dr("LUNAME"))
                        If lstLandUses.Contains(landuse.LuID) Then .Landuses.Add(dr("LUCODE"), landuse)
                    Next
                Catch ex As Exception
                    LastErrorMsg = "Landuse data table must contain the fields LUCODE, IMP_TOT, and IMP_DCON."
                    Return False
                End Try
                dt.Dispose()

                Dim dictCN As New Dictionary(Of Integer, Integer) 'LU_CNCODE,CN
                dt = LoadTable(.Tables.LandUseCN.Filename)
                If dt.Columns.Contains("LU_CNCODE") And dt.Columns.Contains("CN") Then
                    For Each dr As DataRow In dt.Rows
                        dictCN.Add(dr("LU_CNCODE"), dr("CN"))
                    Next
                Else
                    LastErrorMsg = "Landuse-CN data table must contain the fields LU_CNCODE and CN."
                    Return False
                End If
                dt.Dispose()

                Dim t1 As Date = Now
                Dim Lookup As clsLookup = Nothing
                Dim lyrIndex As Integer = GisUtil.LayerIndex(.Layers.Soils.LayerName)
                Dim fldIndex As Integer = GisUtil.FieldIndex(lyrIndex, .Layers.Soils.FieldName)
                Dim DestNoDataValue As Integer = gCNPerv.Header.NodataValue
                Dim LanduseNoDataValue As Integer = gLandUse.Header.NodataValue
                With gLandUse.Header
                    Dim NumRows As Integer = .NumberRows
                    Dim NumCols As Integer = .NumberCols
                    Dim dX As Single = .dX, dY As Single = .dY
                    Dim x, y, x0 As Double
                    gLandUse.CellToProj(0, 0, x0, y)
                    Dim arSoil(NumCols - 1) As Single 'row of soil IDs to be written
                    Dim arLU(NumCols - 1) As Single 'row of landuse codes read
                    Dim arCNPerv(NumCols - 1) As Single 'row of cns to be written
                    Dim arCNImperv(NumCols - 1) As Single 'row of cns to be written-impervious
                    Dim LU As clsLandUse = Nothing
                    sfSoils.BeginPointInShapefile()
                    Dim lstMissing As New Generic.List(Of String)

                    For r As Integer = 0 To NumRows - 1
                        gLandUse.GetRow(r, arLU(0))
                        x = x0
                        For c As Integer = 0 To NumCols - 1
                            Dim xx, yy As Double
                            gCNPerv.CellToProj(c, r, xx, yy)
                            arSoil(c) = DestNoDataValue
                            arCNPerv(c) = DestNoDataValue
                            arCNImperv(c) = DestNoDataValue
                            If arLU(c) <> LanduseNoDataValue Then
                                Dim shpIndex As Integer = sfSoils.PointInShapefile(x, y)
                                If shpIndex <> -1 Then
                                    Dim MUID As String = sfSoils.CellValue(fldIndex, shpIndex)
                                    Dim CN As Integer, Soil As clsSoil = Nothing
                                    If Project.Soils.TryGetValue(MUID, Soil) AndAlso _
                                       dictCN.TryGetValue(CInt(Soil.GroupValue + arLU(c)), CN) AndAlso _
                                       CN <> 0 AndAlso _
                                       Project.Landuses.TryGetValue(arLU(c), LU) Then 'found soil type, combined with lucode, got CN, and imperv ratios
                                        arSoil(c) = Soil.SoilID
                                        arCNPerv(c) = CN
                                        With LU
                                            If .Imp_Tot < 0.3 Then
                                                arCNImperv(c) = (CN * (1 - .Imp_Tot + .Imp_DCon / 2) + 98 * .Imp_DCon / 2) / (1 - (.Imp_Tot - .Imp_DCon))
                                            Else
                                                arCNImperv(c) = (CN * (1 - .Imp_Tot) + 98 * .Imp_DCon) / (1 - (.Imp_Tot - .Imp_DCon))
                                            End If
                                            arCNImperv(c) = Math.Max(Math.Min(arCNImperv(c), 99.99), 0.01)
                                        End With
                                    Else
                                        Dim s As String = String.Format("Land Use: {0}; Soil Type: {1}; Land Use/CN Lookup: {2}; CN: {3}", arLU(c), MUID, CInt(Soil.GroupValue + arLU(c)), CN)
                                        If Not lstMissing.Contains(s) Then lstMissing.Add(s)
                                    End If
                                End If
                            End If
                            x += dX
                        Next
                        gSoils.PutRow(r, arSoil(0))
                        gCNPerv.PutRow(r, arCNPerv(0))
                        gCnImperv.PutRow(r, arCNImperv(0))
                        If Not ProgressForm.SetProgress(r, NumRows - 1) Then Return False
                        y -= dY
                    Next
                    If lstMissing.Count <> 0 Then
                        lstMissing.Sort()
                        Dim err As String = ""
                        For Each LUCode As String In lstMissing
                            err &= LUCode & "\n"
                        Next
                        If Logger.Message(String.Format("The following landuse and soil IDs were missing from the LandUse Lookup and/or LandUse_CN and/or Soil tables, or the lookup value for the CN was 0:\n\n{0}\n\nThe computed CN grids were assigned 'NoDataValue' values at the affected locations.", err).Replace("\n", vbCr), "GBMM Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, DialogResult.Cancel) = DialogResult.Cancel Then Return False
                    End If
                End With
                gSoils.Save()
                gLandUse.Save()
                gCNPerv.Save()
                gCnImperv.Save()
                gSoils.Close()
                gLandUse.Close()
                gCNPerv.Close()
                gCnImperv.Close()

                'only want to save soils that are actually encountered so remove unreferenced ones...
                Dim lstSoils As Generic.List(Of Single) = GetGridValues(.Grids.Soils.FileName)
                Dim lstRemove As New Generic.List(Of String)
                For Each s As clsSoil In .Soils.Values
                    If Not lstSoils.Contains(s.SoilID) Then lstRemove.Add(s.MUID)
                Next
                For Each s As String In lstRemove
                    .Soils.Remove(s)
                Next

                If Not .Grids.Soils.AddLayer() Then Return False
                If Not .Grids.CNPerv.AddLayer() Then Return False
                If Not .Grids.CNImperv.AddLayer() Then Return False

                Return True
            Catch ex As Exception
                ErrorMsg(, ex)
                Return False
            Finally
                If sfSoils IsNot Nothing Then sfSoils.Close()
                If gSoils IsNot Nothing Then gSoils.Close()
                If gLandUse IsNot Nothing Then gLandUse.Close()
                If gCNPerv IsNot Nothing Then gCNPerv.Close()
                If gCnImperv IsNot Nothing Then gCnImperv.Close()
            End Try
        End With
    End Function

    Private Function CalcLSFactor() As Boolean
        With Project.Grids
            Select Case Project.LSFactor.LSFlag
                Case enumLSFlag.DEM
                    WarningMsg("Computation of LS Factors using DEM is not supported in this version of GBMM.")
                    Return False
                Case enumLSFlag.Constant
                    If .LSFactor.IsUpToDate(.Slope.FileName) Then Return True
                    'use previously created slope grid
                    If .Slope.FileName = "" Then WarningMsg("You must have computed a valid slope grid.") : Return False
                    If Not .LSFactor.Delete() Then Return False
                    If Not MultiplyGrid_LS(.Slope.FileName, .LSFactor.FileName, Project.LSFactor.ConstantSlopeLength) Then Return False
                Case enumLSFlag.Existing
                    If .LSFactor.FileName = "" Then
                        WarningMsg("You indicated that an existing LS Factor grid should be used, but no such layer was found.")
                        Return False
                    End If
            End Select
            Return True
        End With
    End Function

    Private Function CalcRoughness() As Boolean
        Try
            With Project

                If .Grids.Roughness.IsUpToDate(.Grids.LandUse.FileName, .Tables.LandUse.Filename) AndAlso _
                   .Grids.AvgRoughness.IsUpToDate(.Grids.Roughness.FileName) Then Return True

                If .Tables.LandUse.Filename = "" Then WarningMsg("You must specify a valid Land Use Properties table.") : Return False
                If .Grids.DEM.FileName = "" Then WarningMsg("You must specify a valid DEM layer.") : Return False

                'create dictionary for roughness lookup
                Dim dictRoughness As New Generic.Dictionary(Of String, clsLookup)
                Dim dt As DataTable = LoadTable(.Tables.LandUse.Filename)
                For Each dr As DataRow In dt.Rows
                    Try
                        dictRoughness.Add(dr("LUCODE"), New clsLookup(dr("LUCODE"), dr("N")))
                    Catch ex As Exception
                        WarningMsg("Landuse data table must contain the fields LUCODE, LUNAME, and N.")
                        Return False
                    End Try
                Next
                dt.Dispose()

                If Not .Grids.Roughness.Delete() Then Return False
                If Not .Grids.AvgRoughness.Delete() Then Return False

                If Not CreateGrid(.Grids.Roughness.FileName) Then Return False
                If Not LookupGrid(.Grids.LandUse.FileName, .Grids.Roughness.FileName, dictRoughness) Then Return False

                If Not CalcDownstream(.Grids.FlowDir.FileName, .Grids.Roughness.FileName, .Grids.FlowPath.FileName, .Grids.AvgRoughness.FileName, enumFlowOption.Overland, enumCalcOption.Average) Then Return False

                If Not .Grids.Roughness.AddLayer() Then Return False
                If GisUtil.IsLayer(.Grids.Roughness.LayerName) Then GisUtil.UniqueValuesRenderer(GisUtil.LayerIndex(.Grids.Roughness.FileName)) '...because default renderer doesn't work correctly for single-precision numbers

                If Not .Grids.AvgRoughness.AddLayer() Then Return False

                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    Private LookupHydRad As LookupDelegate = AddressOf HydRad

    ''' <summary>
    ''' Given accumulated area grid, use Rosgen formula to compute regional channel depths and widths 
    ''' </summary>
    ''' <param name="NumCells">Number of upstream cells</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function HydRad(ByVal NumCells As Integer) As Double
        With Project
            If NumCells = 0 Then Return 0
            Dim DrainageArea As Double = NumCells * .CellAreaKm
            With .StreamParms
                Dim Depth As Double = .AlphaDepth * DrainageArea ^ .BetaDepth
                Dim Width As Double = .AlphaWidth * DrainageArea ^ .BetaWidth
                Dim Area As Double = Depth * (Width + Depth / 2 * (.Z1 + .Z2))
                Dim Perim As Double = Width + Depth * (Math.Sqrt(1 + .Z1 ^ 2) + Math.Sqrt(1 + .Z2 ^ 2))
                If Perim = 0 Then Return 0 Else Return Area / Perim
            End With
        End With
    End Function

    ''' <summary>
    ''' Compute hydraulic radius grid
    ''' </summary>
    Private Function CalcHydRad() As Boolean
        With Project.Grids
            If .HydRadius.IsUpToDate(.FlowAccum.FileName, .FlowPath.FileName, .AvgHydRadius.FileName) Then Return True

            If .DEM.FileName = "" Then WarningMsg("You must specify a valid DEM layer.") : Return False
            If .FlowAccum.FileName = "" Then WarningMsg("You must specify a valid Flow Accumulation layer.") : Return False

            If Not .HydRadius.Delete() Then Return False

            If Not CreateGrid(.HydRadius.FileName) Then Return False
            If Not LookupGrid(.FlowAccum.FileName, .HydRadius.FileName, LookupHydRad) Then Return False
            If Not FilterGrid(.HydRadius.FileName, .FlowPath.FileName) Then Return False
            If Not CalcDownstream(.FlowDir.FileName, .HydRadius.FileName, .FlowPath.FileName, .AvgHydRadius.FileName, enumFlowOption.Stream, enumCalcOption.Average) Then Return False
            If Not .HydRadius.AddLayer Then Return False
            If Not .AvgHydRadius.AddLayer Then Return False
            Return True
        End With
    End Function

    ''' <summary>
    ''' Compute parameters for each lake in watershed
    ''' </summary>
    ''' <remarks>Assume that each lake is entirely contained within a subbasin (or not in any subbasins)</remarks>
    Private Function CalcLakeParms() As Boolean
        With Project
            If .Layers.Lakes.LayerName <> "" Then

                If .Lakes.Count > 0 Then Return True

                If Not GisUtil.IsLayer(.Layers.Lakes.LayerName) Then WarningMsg("You must specify a valid Lake layer.") : Return False
                If .Layers.Subbasins.Filename = "" Then WarningMsg("You must specify a valid Subbasins layer.") : Return False

                .Lakes.Clear()
                Dim LakeLayerIdx As Integer = GisUtil.LayerIndex(.Layers.Lakes.LayerName)
                Dim SubbasinLayerIdx As Integer = GisUtil.LayerIndex(.Layers.Subbasins.LayerName)
                If LakeLayerIdx = -1 Or SubbasinLayerIdx = -1 Then Return False

                Dim LakeFieldIdx As Integer = GisUtil.FieldIndex(LakeLayerIdx, .Layers.Lakes.FieldName)
                For i As Integer = 0 To GisUtil.NumFeatures(LakeLayerIdx) - 1
                    Dim Area As Double = GisUtil.FeatureArea(LakeLayerIdx, i) / .DistFactor ^ 2
                    If Area >= .LakeParms.AreaThreshold Then
                        Dim x, y As Double
                        GisUtil.ShapeCentroid(LakeLayerIdx, i, x, y)
                        Dim ShapeIdx As Integer = GisUtil.PointInPolygonXY(x, y, SubbasinLayerIdx)
                        If ShapeIdx <> -1 Then
                            Dim SubID As Integer = ShapeIdx + 1
                            Dim LakeID As String = GisUtil.FieldValue(LakeLayerIdx, i, LakeFieldIdx)
                            .Lakes.Add(New clsLake(LakeID, SubID, Area))
                            'get list of climate stations whose theissan polygons touch this lake
                            Dim dictAreas As Generic.Dictionary(Of Integer, Double) = TabulateAreasInShape(.Grids.ClimateThiessan.FileName, .Layers.Lakes.Filename, i)
                            With .Lakes(.Lakes.Count - 1).ClimStaFrac
                                For Each kv As KeyValuePair(Of Integer, Double) In dictAreas
                                    .Add(kv.Key, New clsClimStaFrac(kv.Key, kv.Value / Area))
                                Next
                            End With
                        End If
                    End If
                    ProgressForm.SetProgress("Tabulating areas...", i, GisUtil.NumFeatures(LakeLayerIdx) - 1)
                Next
            End If
            Return True
        End With
    End Function

    Private Function CalcLakeRaster() As Boolean
        'note: need to investigate how lake raster is used in GBMM!
        Return True
    End Function

    ''' <summary>
    ''' Calculate travel time grid, where each grid contains the cumulative travel time from that point to the downstream outlet (in hrs) 
    ''' </summary>
    Private Function CalcTravelTimes() As Boolean
        Dim gAvgSlopeOverland As New MapWinGIS.Grid
        Dim gAvgSlopeStream As New MapWinGIS.Grid
        Dim gAvgRoughness As New MapWinGIS.Grid
        Dim gAvgHydRad As New MapWinGIS.Grid
        Dim gDistToStream As New MapWinGIS.Grid
        Dim gDistToOutlet As New MapWinGIS.Grid
        Dim gTravelOverland As New MapWinGIS.Grid
        Dim gTravelStream As New MapWinGIS.Grid

        Try
            'overland travel time:

            'Tov = 0.0289*(N*L)^0.8/(P2^0.5*S^0.4)
            'where T is travel time (hr), N is Mannings roughness, L is flowlength (m), P2 is 2-yr, 24hr rainfall (cm), S is slope (m/m)

            'note: use downstream average values for roughness & slope and distance to channel for length to compute cumulative downstream travel time to channel

            'stream travel time:

            'Tch = L/(3600*V), where T is travel time (hr), L is flow length (m), V is channel velocity (m/s)
            'V = 1/n * R^2/3 * S^1/2, where V is velocity (m/s), n is roughness, R is hyd rad (m), S is slope (m/m)
            'R, and S grids already calculated; for n use constant channel Manning's n input by user
            'Flow length determined as total downstream flow to outlet

            'total travel time:

            'channel travel time plus overland travel time

            With Project.Grids
                If .TravelOverland.IsUpToDate(.AvgSlopeOverland.FileName, .AvgRoughness.FileName, .DistToStream.FileName) AndAlso _
                   .TravelStream.IsUpToDate(.AvgSlopeStream.FileName, .HydRadius.FileName, .DistToOutlet.FileName) Then Return True

                If Project.WatershedParms.P2Rainfall <= 0 Then LastErrorMsg = "You must specify the 2-yr, 24-hr rainfall depth." : Return False
                If Not gAvgSlopeOverland.Open(.AvgSlopeOverland.FileName) Then Return False
                If Not gAvgSlopeStream.Open(.AvgSlopeStream.FileName) Then Return False
                If Not gAvgRoughness.Open(.AvgRoughness.FileName) Then Return False
                If Not gAvgHydRad.Open(.AvgHydRadius.FileName) Then Return False
                If Not gDistToStream.Open(.DistToStream.FileName) Then Return False
                If Not gDistToOutlet.Open(.DistToOutlet.FileName) Then Return False
                If Not .TravelOverland.Delete() Then Return False
                If Not .TravelStream.Delete() Then Return False

                'create new grids
                If Not CreateGrid(.TravelOverland.FileName) Then Return False
                If Not gTravelOverland.Open(.TravelOverland.FileName) Then Return False

                If Not CreateGrid(.TravelStream.FileName) Then Return False
                If Not gTravelStream.Open(.TravelStream.FileName) Then Return False

                If Not GridsCompatible(gAvgRoughness, gAvgHydRad, gAvgSlopeOverland, gTravelStream, gTravelOverland) Then Return False

                'loop through each cell and compute travel time for overland and stream
                With gTravelStream.Header
                    Dim NoDataValueTS As Single = .NodataValue
                    Dim NoDataValueAR As Single = gAvgRoughness.Header.NodataValue
                    Dim NoDataValueAH As Single = gAvgHydRad.Header.NodataValue
                    Dim NoDataValueSS As Single = gAvgSlopeStream.Header.NodataValue
                    Dim NoDataValueSO As Single = gAvgSlopeOverland.Header.NodataValue
                    Dim NoDataValueDS As Single = gDistToStream.Header.NodataValue
                    Dim NoDataValueDO As Single = gDistToOutlet.Header.NodataValue
                    Dim arAvgSlopeOverland(.NumberCols - 1) As Single
                    Dim arAvgSlopeStream(.NumberCols - 1) As Single
                    Dim arAvgRoughness(.NumberCols - 1) As Single
                    Dim arAvgHydRad(.NumberCols - 1) As Single
                    Dim arDistToStream(.NumberCols - 1) As Single
                    Dim arDistToOutlet(.NumberCols - 1) As Single
                    Dim arTravelOverland(.NumberCols - 1) As Single
                    Dim arTravelStream(.NumberCols - 1) As Single
                    For r As Integer = 0 To .NumberRows - 1
                        If Not gAvgRoughness.GetRow(r, arAvgRoughness(0)) Then Return False
                        If Not gAvgHydRad.GetRow(r, arAvgHydRad(0)) Then Return False
                        If Not gAvgSlopeOverland.GetRow(r, arAvgSlopeOverland(0)) Then Return False
                        If Not gAvgSlopeStream.GetRow(r, arAvgSlopeStream(0)) Then Return False
                        If Not gDistToStream.GetRow(r, arDistToStream(0)) Then Return False
                        If Not gDistToOutlet.GetRow(r, arDistToOutlet(0)) Then Return False
                        For c As Integer = 0 To .NumberCols - 1
                            arTravelOverland(c) = NoDataValueTS
                            If arDistToStream(c) <> NoDataValueDS AndAlso arAvgRoughness(c) <> NoDataValueAR AndAlso arAvgRoughness(c) <> 0 Then
                                Dim L As Double = arDistToStream(c)
                                Dim S As Double = Math.Max(arAvgSlopeOverland(c), 0.001)
                                arTravelOverland(c) = 0.0289 * (arAvgRoughness(c) * L) ^ 0.8 / (Project.WatershedParms.P2Rainfall ^ 0.4 * S ^ 0.4)
                            End If
                            arTravelStream(c) = NoDataValueTS
                            If arDistToOutlet(c) <> NoDataValueDO AndAlso arAvgHydRad(c) <> NoDataValueAH AndAlso arAvgSlopeStream(c) <> NoDataValueSS Then
                                Dim S As Double = Math.Max(arAvgSlopeStream(c), 0.0001)
                                Dim V As Double = 1 / Project.StreamParms.Manning * arAvgHydRad(c) ^ 0.6667 * Math.Sqrt(S)
                                Dim L As Double = arDistToOutlet(c)
                                If V <> 0 Then
                                    arTravelStream(c) = L / (3600 * V)
                                    arTravelOverland(c) = arTravelStream(c) 'also overlay on top of overland grid so later can get total
                                End If
                            End If
                        Next
                        gTravelOverland.PutRow(r, arTravelOverland(0))
                        gTravelStream.PutRow(r, arTravelStream(0))
                        If Not ProgressForm.SetProgress("Calculating travel times...", r, .NumberRows - 1) Then Return False
                    Next
                End With

                gTravelOverland.Save()
                gTravelStream.Save()

                gTravelOverland.Close()
                gTravelStream.Close()

                'now I have grid containing  travel time to stream, & stream travel times to outlet; need to compute total travel time

                If Not CalcDownstream(.FlowDir.FileName, .TravelOverland.FileName, .FlowPath.FileName, .TravelTotal.FileName, enumFlowOption.Both, enumCalcOption.TravelTime) Then Return False

                'add layers to project

                If Not .TravelOverland.AddLayer Then Return False
                If Not .TravelStream.AddLayer Then Return False
                If Not .TravelTotal.AddLayer Then Return False

            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gAvgRoughness.Close()
            gAvgHydRad.Close()
            gAvgSlopeOverland.Close()
            gDistToStream.Close()
            gDistToOutlet.Close()
        End Try
    End Function

    ''' <summary>
    ''' Create subbasin grid and compute travel times from outlet to outlet
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalcSubbasin() As Boolean
        Dim gDEM As New MapWinGIS.Grid
        Dim gSub As New MapWinGIS.Grid
        Dim gDir As New MapWinGIS.Grid
        Dim gTTT As New MapWinGIS.Grid
        Dim sf As New MapWinGIS.Shapefile
        Dim arDir(,) As Integer
        Try
            With Project.Grids

                If .Subbasins.IsUpToDate(Project.Layers.Subbasins.Filename, .DEM.FileName, .TravelTotal.FileName) AndAlso _
                    My.Computer.FileSystem.FileExists(Project.Layers.AssessPt.Filename) AndAlso _
                    Project.AssessmentPoints.Count > 0 AndAlso _
                    Project.Watersheds.Count > 0 AndAlso _
                    Project.TravelTimes.Count > 0 Then Return True

                If Not .Subbasins.IsUpToDate(Project.Layers.Subbasins.Filename) Then
                    If Not CreateGrid(.Subbasins.FileName, MapWinGIS.GridDataType.LongDataType) Then Return False
                    If Not ShapefileToGrid(Project.Layers.Subbasins.Filename, "", .Subbasins.FileName) Then Return False
                    .Subbasins.AddLayer()
                End If

                'compute travel times from outlet to outlet; outlets will be defined as lowest grid point in the subbasin
                If Not gDEM.Open(.DEM.FileName) Then LastErrorMsg = "Unable to open: " & .DEM.FileName : Return False
                If Not gSub.Open(.Subbasins.FileName) Then LastErrorMsg = "Unable to open: " & .Subbasins.FileName : Return False

                'store all direction grid data in 2-D arrays to speed processing

                If Not gDir.Open(.FlowDir.FileName) Then LastErrorMsg = "Unable to open: " & .FlowDir.FileName : Return False
                With gDir.Header
                    ReDim arDir(.NumberRows - 1, .NumberCols - 1)
                    Dim arD(.NumberCols - 1) As Single
                    For r As Integer = 0 To .NumberRows - 1
                        gDir.GetRow(r, arD(0))
                        For c As Integer = 0 To .NumberCols - 1
                            arDir(r, c) = CInt(arD(c))
                        Next
                        If Not ProgressForm.SetProgress("Compiling flow direction data...", r, .NumberRows - 1) Then Return False
                    Next
                End With
            End With

            Project.AssessmentPoints.Clear()

            Dim NoDataValueDir As Single = gDir.Header.NodataValue

            With gDEM.Header
                Dim NoDataValueDEM As Single = .NodataValue
                Dim arDEM(.NumberCols - 1) As Single
                Dim arSub(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    gDEM.GetRow(r, arDEM(0))
                    gSub.GetRow(r, arSub(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If arDEM(c) <> NoDataValueDEM Then
                            Dim AssessPt As clsAssessPt = Nothing
                            If Not Project.AssessmentPoints.TryGetValue(arSub(c), AssessPt) Then AssessPt = New clsAssessPt(arSub(c), Integer.MaxValue, r, c)
                            If arDEM(c) < AssessPt.MinElev And arDir(r, c) <> NoDataValueDir Then
                                Project.AssessmentPoints.Remove(arSub(c))
                                Project.AssessmentPoints.Add(arSub(c), New clsAssessPt(arSub(c), arDEM(c), r, c))
                            End If
                        End If
                    Next
                    If Not ProgressForm.SetProgress("Finding assessment points...", r, .NumberRows - 1) Then Return False
                Next
            End With

            'now have a list of min elevations nodes for each subbasin (assessment points); for grins, create assessment point layer

            ProgressForm.SetProgress("Creating assessment point layer...", ProgressBarStyle.Marquee)

            With Project.Layers.AssessPt
                If GisUtil.IsLayerByFileName(.Filename) Then GisUtil.RemoveLayer(GisUtil.LayerIndex(.Filename))
                MapWinGeoProc.DataManagement.DeleteShapefile(.Filename)
                If Not sf.CreateNew(.Filename, MapWinGIS.ShpfileType.SHP_POINT) Then LastErrorMsg = "Unable to create: " & .Filename : Return False
                sf.StartEditingShapes(True)
                Dim fld As New MapWinGIS.Field
                fld.Name = .FieldName
                fld.Type = MapWinGIS.FieldType.INTEGER_FIELD
                sf.EditInsertField(fld, 0)
                For Each AssessPt As clsAssessPt In Project.AssessmentPoints.Values
                    Dim shp As New MapWinGIS.Shape
                    shp.Create(MapWinGIS.ShpfileType.SHP_POINT)
                    Dim pt As New MapWinGIS.Point
                    gDEM.CellToProj(AssessPt.Col, AssessPt.Row, pt.x, pt.y)
                    shp.InsertPoint(pt, 0)
                    sf.EditInsertShape(shp, sf.NumShapes)
                    sf.EditCellValue(0, sf.NumShapes - 1, AssessPt.SubID)
                    Application.DoEvents()
                Next
                sf.StopEditingShapes(True, True)
                sf.Projection = GisUtil.ProjectProjection
                sf.SaveAs(.Filename)
                sf.Close()
                If Project.AddLayers Then
                    GisUtil.AddLayerToGroup(.Filename, .LayerName, GroupName)
                    Dim lyr As MapWindow.Interfaces.Layer = gMapWin.Layers(gMapWin.Layers.GetHandle(GisUtil.LayerIndex(.LayerName)))
                    lyr.Color = Drawing.Color.Red
                    lyr.OutlineColor = Drawing.Color.Red
                    lyr.PointType = MapWinGIS.tkPointType.ptCircle
                    lyr.LineOrPointSize = 12
                    lyr.Visible = True
                    gMapWin.Refresh()
                End If
            End With
            gDEM.Close()
            gSub.Close()

            With Project

                'get area for each subbasin

                .Watersheds.Clear()
                With .Layers.Subbasins
                    If .LayerName <> "" And GisUtil.IsLayer(.LayerName) Then
                        Dim LayerIdx As Integer = GisUtil.LayerIndex(.LayerName)
                        For i As Integer = 0 To GisUtil.NumFeatures(LayerIdx) - 1
                            Dim ID As Integer = i + 1
                            Dim Area As Double = GisUtil.FeatureArea(LayerIdx, i)
                            Project.Watersheds.Add(New clsWatershed(ID, Area))
                            If Not ProgressForm.SetProgress("Compiling subbasin data...", i, GisUtil.NumFeatures(LayerIdx) - 1) Then Return False
                        Next
                    End If
                End With

                'for each assessment point, determine downstream assessment point and travel time between; must walk down flow direction grid until next one is found

                ProgressForm.SetProgress("Determining travel times...", ProgressBarStyle.Marquee)

                If Not gTTT.Open(.Grids.TravelTotal.FileName) Then LastErrorMsg = "Unable to open: " & .Grids.TravelTotal.FileName : Return False

                .TravelTimes.Clear()

                For Each ptSource As clsAssessPt In Project.AssessmentPoints.Values
                    Dim dsr As Integer = ptSource.Row, dsc As Integer = ptSource.Col
                    Do
                        'next downstream cell coordinates (note: rows count from top to bottom, cols from left to right)

                        Dim dir As Integer = arDir(dsr, dsc)
                        dsr += Choose(dir, 0, -1, -1, -1, 0, 1, 1, 1)
                        dsc += Choose(dir, 1, 1, 0, -1, -1, -1, 0, 1)

                        'stop when no downstream cell get to another assessment point

                        If arDir(dsr, dsc) = NodataValuedir Then 'is ds-most outlet, must save special link
                            .TravelTimes.Add(New clsTravelTime(ptSource.SubID, 0, 0.0))
                            Exit Do
                        End If

                        For Each ptDest As clsAssessPt In Project.AssessmentPoints.Values
                            If ptDest.SubID <> ptSource.SubID AndAlso ptDest.Row = dsr AndAlso ptDest.Col = dsc Then
                                'travel time is difference btwn total travel times for two points
                                Dim tt As Double = gTTT.Value(ptSource.Col, ptSource.Row) - gTTT.Value(ptDest.Col, ptDest.Row)
                                .TravelTimes.Add(New clsTravelTime(ptSource.SubID, ptDest.SubID, tt))
                                Exit Do
                            End If
                            Application.DoEvents()
                        Next
                    Loop
                Next

            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            gDEM.Close()
            gSub.Close()
            sf.Close()
            gDir.Close()
            gTTT.Close()
        End Try
    End Function

    Private Function CalcTheissan() As Boolean
        With Project
            If .Grids.ClimateThiessan.IsUpToDate(.Layers.ClimateSta.Filename) AndAlso _
               .ClimateStations.Count > 0 Then Return True

            If .Layers.MercurySta.LayerName = "" Then
                .MercuryStations.Clear()
            Else
                If .Grids.MercuryThiessan.IsUpToDate(.Layers.MercurySta.Filename) AndAlso .MercuryStations.Count > 0 Then Return True
            End If

            With .Grids
                If Not .ClimateThiessan.Delete() Then Return False
                If Not .MercuryThiessan.Delete() Then Return False
            End With

            With .Layers.ClimateSta
                'create climate station theissan polygons; note that FieldName is for field with StationName string; 
                'grid will use sequentially assigned integer index (item number in shape file)
                If Not GisUtil.IsLayer(.LayerName) Then WarningMsg("Climate station grid was not found.") : Return False
                If Not CreateThiessanGrid(Project.Grids.ClimateThiessan.FileName, .LayerName, .FieldName) Then Return False
            End With
            With .Grids.ClimateThiessan
                FilterGrid(.FileName, Project.Layers.Subbasins.Filename)
                .AddLayer()
            End With

            Dim lyrIdx As Integer = GisUtil.LayerIndex(.Layers.ClimateSta.LayerName)
            Dim fldIdxID As Integer = GisUtil.FieldIndex(lyrIdx, .Layers.ClimateSta.FieldName)
            If Not GisUtil.IsField(lyrIdx, "LAT") Then LastErrorMsg = "The climate stations layer must contain a field called LAT!" : Return False
            Dim fldIdxLat As Integer = GisUtil.FieldIndex(lyrIdx, "LAT")

            With .ClimateStations
                .Clear()
                For Each StaNum As Integer In GetGridValues(Project.Grids.ClimateThiessan.FileName)
                    .Add(StaNum, New clsStation(StaNum, GisUtil.FieldValue(lyrIdx, StaNum - 1, fldIdxID), GisUtil.FieldValue(lyrIdx, StaNum - 1, fldIdxLat)))
                Next
            End With

            With .Tables.Climate
                If Not .Export Then LastErrorMsg = "Unable to copy table: " & .Filename : Return False
            End With

            If .Layers.MercurySta.LayerName = "" Then Return True

            With .Layers.MercurySta
                'create mercury station theissan polygons
                If Not GisUtil.IsLayer(.LayerName) Then WarningMsg("Mercury station grid was not found.") : Return False
                If Not CreateThiessanGrid(Project.Grids.MercuryThiessan.FileName, .LayerName, .FieldName) Then Return False
            End With
            With .Grids.MercuryThiessan
                FilterGrid(.FileName, Project.Layers.Subbasins.Filename)
                .AddLayer()
            End With

            lyrIdx = GisUtil.LayerIndex(.Layers.MercurySta.LayerName)
            fldIdxID = GisUtil.FieldIndex(lyrIdx, .Layers.MercurySta.FieldName)

            With .MercuryStations
                .Clear()
                For Each StaNum As Integer In GetGridValues(Project.Grids.MercuryThiessan.FileName)
                    .Add(StaNum, New clsStation(StaNum, GisUtil.FieldValue(lyrIdx, StaNum - 1, fldIdxID)))
                Next
            End With

            With .Tables.HgDryDep
                If Not .Export Then LastErrorMsg = "Unable to copy table: " & .Filename : Return False
            End With
            With .Tables.HgWetDep
                If Not .Export Then LastErrorMsg = "Unable to copy table: " & .Filename : Return False
            End With

            Return True
        End With
    End Function

    Private Function CalcPointSource() As Boolean
        Dim gTTT As New MapWinGIS.Grid
        Try
            With Project
                If .Layers.PointSources.LayerName = "" Then .PointSources.Clear() : Return True
                If .PointSources.Count > 0 Then Return True
                Dim lyrIdx As Integer = GisUtil.LayerIndex(.Layers.PointSources.LayerName)
                Dim fldIdx As Integer = GisUtil.FieldIndex(lyrIdx, .Layers.PointSources.FieldName)
                If Not gTTT.Open(.Grids.TravelTotal.FileName) Then LastErrorMsg = "Unable to open: " & .Grids.TravelTotal.FileName : Return False

                With .PointSources
                    .Clear()
                    For i As Integer = 0 To GisUtil.NumFeatures(lyrIdx) - 1
                        Dim shpNum As Integer = GisUtil.PointInPolygon(lyrIdx, i, GisUtil.LayerIndex(Project.Layers.Subbasins.LayerName))
                        If shpNum <> -1 Then 'point is in a subbasin, assign shape number as subbasin ID and compute travel time to outlet
                            Dim SubID As Integer = i + 1
                            'get grid location for point sources, then total travel time to outlet
                            Dim x, y As Double, r, c As Integer
                            GisUtil.PointXY(lyrIdx, i, x, y)
                            gTTT.ProjToCell(x, y, c, r)
                            Dim travTimePS As Double = gTTT.Value(c, r)
                            'get assessment point for this subbasin and its travel time
                            Dim ap As clsAssessPt = Project.AssessmentPoints(SubID)
                            Dim travTimeAP As Double = gTTT.Value(ap.Col, ap.Row)
                            'assign travel time to outlet
                            .Add(SubID, New clsStation(SubID, GisUtil.FieldValue(lyrIdx, i, fldIdx), , travTimePS - travTimeAP))
                        End If
                    Next
                End With
                With .Tables.PointSource
                    If Not .Export Then LastErrorMsg = "Unable to copy table: " & .Filename : Return False
                End With
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gTTT.Close()
        End Try
    End Function

    Private LookupKdComp As LookupDelegate = AddressOf KdComp

    ''' <summary>
    ''' Given land use code, assign decomposition rates
    ''' </summary>
    ''' <param name="LuCode">Land use code</param>
    ''' <returns></returns>
    ''' <remarks>Note: even though in other part of the program a landuse lookup table is used, here the landuse codes are hardwired</remarks>
    Private Function KdComp(ByVal LuCode As Integer) As Double
        With Project.ForestMercuryParms
            Select Case LuCode
                Case 41, 51 : Return .LitterDecomp1
                Case 42, 52 : Return .LitterDecomp2
                Case 43, 53, 91 : Return .LitterDecomp3
                Case Else
                    Return 0
            End Select
        End With
    End Function

    Private Function CalcLitter() As Boolean
        With Project.Grids
            If .MercuryLitter.IsUpToDate(.LandUse.FileName) Then Return True
            If Not CreateGrid(.MercuryLitter.FileName) Then Return False
            If Not LookupGrid(.LandUse.FileName, .MercuryLitter.FileName, LookupKdComp) Then Return False
            .MercuryLitter.AddLayer()
            If GisUtil.IsLayer(.MercuryLitter.LayerName) Then GisUtil.UniqueValuesRenderer(GisUtil.LayerIndex(.MercuryLitter.LayerName))
        End With
        Return True
    End Function

    Private Function ExportGrid(ByVal Grid As clsGrid, Optional ByVal MaxGrids As Integer = 0) As Boolean
        Static GridNum As Integer = 0, NumGrids As Integer = 0
        If MaxGrids <> 0 Then
            GridNum = 0
            NumGrids = MaxGrids
        End If
        With Grid
            If Not ProgressForm.SetProgress("Exporting grid: " & .LayerName, GridNum, NumGrids) Then Return False
            If Not .Export Then LastErrorMsg = "Unable to export grid to Input folder: " & .LayerName : Return False
            GridNum += 1
        End With
        Return True
    End Function

    Private Function SaveGrids() As Boolean
        'delete all existing .asc files after removing them if loaded in map
        For Each fn As String In My.Computer.FileSystem.GetFiles(Project.Folders.InputFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.asc")
            If GisUtil.IsLayerByFileName(fn) Then GisUtil.RemoveLayer(GisUtil.LayerIndex(fn))
            Try
                My.Computer.FileSystem.DeleteFile(fn)
            Catch ex As Exception
            End Try
        Next
        With Project.Grids
            If Not ExportGrid(.ClimateThiessan, 18) Then Return False
            If Not ExportGrid(.LandUse) Then Return False
            If Not ExportGrid(.Soils) Then Return False
            If Not ExportGrid(.Subbasins) Then Return False
            If Not ExportGrid(.CNPerv) Then Return False
            If Not ExportGrid(.CNImperv) Then Return False
            If Not ExportGrid(.TravelTotal) Then Return False
            If Not ExportGrid(.TravelStream) Then Return False
            If Not ExportGrid(.SoilWater) Then Return False
            If Not ExportGrid(.FlowLength) Then Return False
            If Not ExportGrid(.AvgRoughness) Then Return False
            If Not ExportGrid(.AvgSlopeOverland) Then Return False
            If Not ExportGrid(.LSFactor) Then Return False
            If Not ExportGrid(.MercuryLitter) Then Return False
            If Project.MercuryDepo.AirDepoFlag = enumDepoFlag.TimeSeries AndAlso Not ExportGrid(.MercuryThiessan) Then Return False
            If Not .MercuryDryDepo.LayerName <> "" AndAlso Not ExportGrid(.MercuryDryDepo) Then Return False
            If Not .MercuryWetDepo.LayerName <> "" AndAlso Not ExportGrid(.MercuryWetDepo) Then Return False
            If Not .MercurySoil.LayerName <> "" AndAlso Not ExportGrid(.MercurySoil) Then Return False
        End With
        Return True
    End Function

End Module
