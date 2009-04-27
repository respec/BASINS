Imports atcMwGisUtility

Friend Class clsLookup
    Friend Description As String
    Friend Factor As Single
    Sub New(ByVal _Description As String, ByVal _Factor As Single)
        Description = _Description
        Factor = _Factor
    End Sub
End Class

Friend Module modGrid


    '''' <summary>
    '''' Set values in grid to NoValue if they are less than threshold
    '''' </summary>
    '''' <param name="SourceFile">Name of source grid file </param>
    '''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    '''' <param name="Threshold">Threshold value</param>
    'Friend Function LookupGrid(ByVal SourceFile As String, ByVal DestFile As String, ByVal Threshold As Double) As Boolean
    '    Dim gSource As MapWinGIS.Grid = Nothing
    '    Dim gDest As MapWinGIS.Grid = Nothing

    '    Try
    '        gSource = New MapWinGIS.Grid
    '        If Not gSource.Open(SourceFile, , False) Then Return False
    '        gDest = New MapWinGIS.Grid
    '        If Not gDest.Open(DestFile, , False) Then Return False
    '        Dim Lookup As clsLookup = Nothing
    '        With gSource.Header
    '            For r As Integer = 0 To .NumberRows - 1
    '                Dim arV(.NumberCols - 1) As Single
    '                gSource.GetRow(r, arV(0))
    '                For c As Integer = 0 To .NumberCols - 1
    '                    If arV(c) < Threshold Then arV(c) = gDest.Header.NodataValue
    '                Next
    '                gDest.PutRow(r, arV(0))
    '                If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
    '            Next
    '        End With
    '        gDest.Save()
    '        Return True
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '        Return False
    '    Finally
    '        If gSource IsNot Nothing Then gSource.Close()
    '        If gDest IsNot Nothing Then gDest.Close()
    '    End Try
    'End Function

    '''' <summary>
    '''' Convert ArcGIS/NHD flow direction grid system to Taudem system
    '''' </summary>
    '''' <param name="SourceFile">Name of grid file </param>
    'Friend Function LookupGrid(ByVal SourceFile As String) As Boolean
    '    'NHD (ArcGIS):
    '    '
    '    '32    64    128
    '    '16     x      1
    '    ' 8     4      2

    '    'Taudem:
    '    '
    '    ' 4     3      2
    '    ' 5     x      1
    '    ' 6     7      8
    '    Dim g As MapWinGIS.Grid = Nothing

    '    Try
    '        g = New MapWinGIS.Grid
    '        If Not g.Open(SourceFile, , False) Then Return False
    '        With g.Header
    '            For r As Integer = 0 To .NumberRows - 1
    '                Dim arV(.NumberCols - 1) As Single
    '                g.GetRow(r, arV(0))
    '                For c As Integer = 0 To .NumberCols - 1
    '                    Select Case arV(c)
    '                        Case 1 : arV(c) = 1
    '                        Case 2 : arV(c) = 8
    '                        Case 4 : arV(c) = 7
    '                        Case 8 : arV(c) = 6
    '                        Case 16 : arV(c) = 5
    '                        Case 32 : arV(c) = 4
    '                        Case 64 : arV(c) = 3
    '                        Case 128 : arV(c) = 2
    '                        Case g.Header.NodataValue : arV(c) = g.Header.NodataValue
    '                        Case Else
    '                            WarningMsg("Invalid value found in NHD/ArcGIS flow direction grid: {0}; expected only 1, 2, 4, 8, 16, 32, 64, or 128.", arV(c))
    '                            Return False
    '                    End Select
    '                Next
    '                g.PutRow(r, arV(0))
    '                If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
    '            Next
    '        End With
    '        g.Save()
    '        Return True
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '        Return False
    '    Finally
    '        If g IsNot Nothing Then g.Close()
    '    End Try
    'End Function

    Friend Delegate Function LookupDelegate(ByVal SourceValue As Object) As Object

    Friend Enum enumCalcOption
        Average
        Sum
        Distance
        TravelTime
    End Enum

    Friend Enum enumFlowOption
        Overland
        Stream
        Both
    End Enum

    Friend Const PRECISION As Integer = 4 'this is number of decimal places to round real numbers off to when storing in grids

    Private _LastErrorMsg As String = ""

    ''' <summary>
    ''' Determine if two grids are same size and located at same place
    ''' </summary>
    ''' <param name="g1">First Grid</param>
    ''' <param name="g2">Second Grid</param>
    ''' <param name="MustMatchExactly">If true, lower left corner must be at exact same location; otherwise may be offset by up to dx/2</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AreGridsCompatible(ByVal g1 As MapWinGIS.Grid, ByVal g2 As MapWinGIS.Grid, Optional ByVal MustMatchExactly As Boolean = False) As Boolean
        With g1.Header
            If .dX <> g2.Header.dY OrElse .dY <> g2.Header.dY OrElse .NumberCols <> g2.Header.NumberCols OrElse .NumberRows <> g2.Header.NumberRows Then Return False
            Dim Offset As Double = .dX / 2
            If MustMatchExactly Then Offset = 0
            If Math.Abs(.XllCenter - g2.Header.XllCenter) > Offset OrElse Math.Abs(.YllCenter - g2.Header.YllCenter) > Offset Then Return False
        End With
        Return True
    End Function

    ''' <summary>
    ''' Given high-resolution source grid, average all values to a destination grid
    ''' The source and destination grids must already exist (the destination should already be filtered for the desired area)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AverageGrid(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        _LastErrorMsg = ""
        Try
            Dim gSource, gDest As New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then _LastErrorMsg = "Unable to open grid: " & SourceFile : Return False
            If Not gDest.Open(DestFile, , False) Then _LastErrorMsg = "Unable to open grid: " & DestFile : Return False

            If Not GridsCompatible(gSource, gDest) Then Return False

            'create temp array so can average slope values (will initially be populated with zeroes)
            Dim arSum(,) As Double, arNum(,) As Integer
            With gDest.Header
                ReDim arSum(.NumberCols - 1, .NumberRows - 1)
                ReDim arNum(.NumberCols - 1, .NumberRows - 1)
            End With

            With gSource.Header
                'get average value within the larger destination grid from the smaller source grid
                For r As Integer = 0 To .NumberRows - 1
                    Dim x As Double, y As Double, cc As Integer, rr As Integer
                    gSource.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    gSource.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If ar(c) <> .NodataValue Then
                            gDest.ProjToCell(x, y, cc, rr)
                            If cc >= 0 AndAlso cc <= arSum.GetUpperBound(0) AndAlso rr >= 0 AndAlso rr <= arSum.GetUpperBound(1) Then
                                arSum(cc, rr) += ar(c)
                                arNum(cc, rr) += 1
                            End If
                        End If
                        x += .dX
                    Next
                    If Not ProgressForm.SetProgress("Averaging grid...", r, .NumberRows - 1) Then Return False
                Next
            End With
            gSource.Close()

            With gDest.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim ar(.NumberCols - 1) As Single
                    gDest.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If arNum(c, r) > 0 Then ar(c) = Math.Round(arSum(c, r) / arNum(c, r), PRECISION)
                    Next
                    gDest.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress("Averaging grid...", r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            gDest.Close()
            Return True

        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Given a flow direction grid, average or sum all values in the WeightGrid along the downstream path
    ''' </summary>
    ''' <param name="FlowDirGrid">Grid containing D8 flow directions</param>
    ''' <param name="SourceGrid">Grid containing values to average or sum (ignored if CalcOption is Distance)</param>
    ''' <param name="FlowPathGrid">Grid containing flow paths (NoDataValue for all overland flow cells, flow directions otherwise)</param>
    ''' <param name="ResultGrid">Resulting downstream averages</param>
    ''' <param name="FlowOption">Flag indicating whether averages are for overland or stream cells; if Both, then will find where overland connects to stream, and add stream travel time to all overland time in path</param>
    ''' <param name="CalcOption">Flag indicating whether weight grid values should be averaged or totalled, or distance totalled, or travel times totalled</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function CalcDownstream(ByVal FlowDirGrid As String, ByVal SourceGrid As String, ByVal FlowPathGrid As String, ByVal ResultGrid As String, ByVal FlowOption As enumFlowOption, ByVal CalcOption As enumCalcOption) As Boolean
        Dim gFlowDir, gSource, gFlowPath, gResult As New MapWinGIS.Grid
        Try
            If CalcOption = enumCalcOption.TravelTime And FlowOption <> enumFlowOption.Both Then
                _LastErrorMsg = "Invalid combination of Flow and Calc options."
                Return False
            End If

            If Not DeleteGrid(ResultGrid) Then Return False
            If Not gFlowDir.Open(FlowDirGrid) Then _LastErrorMsg = "Unable to open grid: " & FlowDirGrid : Return False
            If CalcOption <> enumCalcOption.Distance AndAlso Not gSource.Open(SourceGrid) Then _LastErrorMsg = "Unable to open grid: " & SourceGrid : Return False
            If Not gFlowPath.Open(FlowPathGrid) Then _LastErrorMsg = "Unable to open grid: " & FlowPathGrid : Return False
            If Not gSource.Open(SourceGrid) Then _LastErrorMsg = "Unable to open grid: " & SourceGrid : Return False
            If Not gResult.CreateNew(ResultGrid, gSource.Header, gSource.DataType, gSource.Header.NodataValue) Then _LastErrorMsg = "Unable to create grid: " & ResultGrid : Return False

            If Not GridsIdentical(gFlowDir, gSource, gFlowPath) Then _LastErrorMsg = "Grids are not identical in structure." : Return False

            With gFlowDir.Header
                'store all grid data in 2-D arrays
                Dim arSum(.NumberRows - 1, .NumberCols - 1) As Single
                Dim arNum(.NumberRows - 1, .NumberCols - 1) As Integer
                Dim arFlowDir(.NumberRows - 1, .NumberCols - 1) As Integer
                Dim arSource(.NumberRows - 1, .NumberCols - 1) As Single
                Dim arFlowPath(.NumberRows - 1, .NumberCols - 1) As Integer
                Dim arD(.NumberCols - 1) As Single
                Dim arS(.NumberCols - 1) As Single
                Dim arP(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    If Not gFlowDir.GetRow(r, arD(0)) Then _LastErrorMsg = "Unable to retrieve grid data: " & FlowDirGrid : Return False
                    If CalcOption <> enumCalcOption.Distance AndAlso Not gSource.GetRow(r, arS(0)) Then _LastErrorMsg = "Unable to retrieve grid data: " & SourceGrid : Return False
                    If Not gFlowPath.GetRow(r, arP(0)) Then _LastErrorMsg = "Unable to retrieve grid data: " & FlowPathGrid : Return False
                    For c As Integer = 0 To .NumberCols - 1
                        arFlowDir(r, c) = CInt(arD(c))
                        arSource(r, c) = arS(c)
                        arFlowPath(r, c) = CInt(arP(c))
                    Next
                Next

                For r As Integer = 0 To .NumberRows - 1
                    For c As Integer = 0 To .NumberCols - 1
                        If arFlowDir(r, c) <> gFlowDir.Header.NodataValue And arNum(r, c) = 0 AndAlso Not (FlowOption = enumFlowOption.Stream And arFlowPath(r, c) = gFlowPath.Header.NodataValue) Then 'hasn't been previously calculated so go ahead
                            'walk downstream, building list of cells touched until get to previously computed cell or d.s. boundary
                            Dim lstCells As New Generic.List(Of Drawing.Point) 'list of downstream cells stepped on (will need to walk back up to get sum)
                            Dim dsr As Integer = r, dsc As Integer = c 'downstream row & column
                            lstCells.Add(New Drawing.Point(dsc, dsr))
                            Do
                                'next downstream cell coordinates (note: rows count from top to bottom, cols from left to right)

                                Dim dir As Integer = arFlowDir(dsr, dsc)
                                dsr += Choose(dir, 0, -1, -1, -1, 0, 1, 1, 1)
                                dsc += Choose(dir, 1, 1, 0, -1, -1, -1, 0, 1)

                                'stop when previously calced cell, no downstream cell (or, if overland or total, stream cell encountered)

                                If arNum(dsr, dsc) <> 0 Then Exit Do 'previously calced cell
                                If arFlowDir(dsr, dsc) = gFlowDir.Header.NodataValue Then Exit Do 'outlet
                                If FlowOption = enumFlowOption.Overland AndAlso arFlowPath(dsr, dsc) <> gFlowPath.Header.NodataValue Then Exit Do 'stream grid

                                'add this new cell and keep going...

                                lstCells.Add(New Drawing.Point(dsc, dsr))
                            Loop

                            'now have list of complete path; walk back upstream, accumulating values

                            'note that computed average is based on number of cells and doesn't take cumulative
                            '     flow path into account (i.e., diagonal slopes given same weight as horiz/vert)

                            For i As Integer = lstCells.Count - 1 To 0 Step -1
                                Dim rr As Integer = lstCells(i).Y
                                Dim cc As Integer = lstCells(i).X
                                If i = lstCells.Count - 1 Then 'initialize downstream end
                                    If arNum(dsr, dsc) <> 0 Then 'set previously computed d.s. cumulative to current (will add below)
                                        arNum(rr, cc) = arNum(dsr, dsc)
                                        arSum(rr, cc) = arSum(dsr, dsc)
                                    Else
                                        arNum(rr, cc) = 0
                                        arSum(rr, cc) = 0
                                    End If
                                Else 'set to next downstream sum
                                    dsr = lstCells(i + 1).Y
                                    dsc = lstCells(i + 1).X
                                    arNum(rr, cc) = arNum(dsr, dsc)
                                    arSum(rr, cc) = arSum(dsr, dsc)
                                End If

                                'now add current cell
                                arNum(rr, cc) += 1
                                Select Case CalcOption
                                    Case enumCalcOption.Distance
                                        Dim dist As Single = Project.GridSize / Project.DistFactor
                                        Select Case arFlowDir(rr, cc)
                                            Case 2, 4, 6, 8 : dist *= 1.414214 'is on diagonal; mult by sqrt(2)
                                        End Select
                                        arSum(rr, cc) += dist
                                    Case enumCalcOption.Sum, enumCalcOption.Average
                                        arSum(rr, cc) += arSource(rr, cc) 'accumulate values from downstream
                                    Case enumCalcOption.TravelTime
                                        If arFlowPath(rr, cc) <> gFlowPath.Header.NodataValue Then
                                            arSum(rr, cc) = arSource(rr, cc) 'is stream, so save stream travel time
                                        Else
                                            arSum(rr, cc) = arSum(dsr, dsc) 'just save the downstream stream travel time (will add later)
                                        End If
                                End Select
                            Next
                        End If
                    Next
                    If Not ProgressForm.SetProgress("Computing averages or totals along downstream flowpaths...", r, .NumberRows - 1) Then Return False
                Next

                'store back into result grid
                Dim ar(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    For c As Integer = 0 To .NumberCols - 1
                        If arNum(r, c) = 0 OrElse (FlowOption = enumFlowOption.Stream And arFlowPath(r, c) = gFlowPath.Header.NodataValue) Then
                            ar(c) = gResult.Header.NodataValue
                        Else
                            Select Case CalcOption
                                Case enumCalcOption.Average
                                    ar(c) = arSum(r, c) / arNum(r, c) 'store average value
                                Case enumCalcOption.Sum, enumCalcOption.Distance
                                    ar(c) = arSum(r, c) 'store accumulated value
                                Case enumCalcOption.TravelTime
                                    If arFlowPath(r, c) <> gFlowPath.Header.NodataValue Then
                                        ar(c) = arSource(r, c)
                                    Else
                                        ar(c) = arSource(r, c) + arSum(r, c) 'if not stream, add overland travel time value and stream travel time
                                    End If
                            End Select
                        End If
                    Next
                    If Not gResult.PutRow(r, ar(0)) Then _LastErrorMsg = "Unable to put grid data: " & ResultGrid : Return False
                Next
                If Not gResult.Save() Then _LastErrorMsg = "Unable to save to grid: " & ResultGrid : Return False
            End With

            Return True

        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gFlowDir.Close()
            If CalcOption <> enumCalcOption.Distance Then gSource.Close()
            gFlowPath.Close()
            gResult.Close()
        End Try

    End Function

    '''' <summary>
    '''' Create a new grid and return new grid file name; will remove layer and delete grid first
    '''' </summary>
    '''' <param name="GridName">Name of layer to create</param>
    '''' <returns>Grid filename</returns>
    '''' <remarks></remarks>
    'Friend Function CreateGrid(ByVal GridFolder As String, ByVal GridName As String, Optional ByVal gTemplate As MapWinGIS.Grid = Nothing, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.FloatDataType) As String
    '    Dim GridFile As String = String.Format("{0}\{1}.tif", GridFolder, GridName)
    '    If CreateGrid(GridFile) Then Return GridFile Else Return ""
    'End Function

    ''' <summary>
    ''' Create a new grid and return new grid file name; will remove layer and delete grid first; extents of layer based on Subbasins layer
    ''' </summary>
    ''' <param name="GridFile">Name of grid file to create</param>
    ''' <returns>Grid filename</returns>
    ''' <remarks></remarks>
    Friend Function CreateGrid(ByVal GridFile As String, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.FloatDataType) As Boolean
        Try
            Dim hdr As New MapWinGIS.GridHeader
            With hdr
                .dX = Math.Round(Project.GridSize * Project.DistFactor, 2) 'convert meters to project units
                .dY = .dX
                .NodataValue = -1  'note: if float type, cannot do direct comparison to this value (round-off) instead compare to >=0.0
                Dim xmax, xmin, ymax, ymin As Double
                GisUtil.ExtentsOfLayer(GisUtil.LayerIndex(Project.Layers.Subbasins.LayerName), xmax, xmin, ymax, ymin)
                .NumberCols = (xmax - xmin) / .dX
                .NumberRows = (ymax - ymin) / .dY
                .XllCenter = xmin + .dX / 2
                .YllCenter = ymin + .dY / 2
            End With

            Do While GisUtil.IsLayerByFileName(GridFile)
                GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
            Loop
            DeleteGrid(GridFile)

            Dim g As New MapWinGIS.Grid
            Try
                If g.CreateNew(GridFile, hdr, DataType, hdr.NodataValue, False, MapWinGIS.GridFileType.UseExtension) Then
                    If g.AssignNewProjection(GisUtil.ProjectProjection) Then
                        Return True
                    Else
                        _LastErrorMsg = "Unable to assign projection: " & GridFile
                    End If
                Else
                    _LastErrorMsg = "Unable to create grid: " & GridFile
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            Finally
                g.Save()
                g.Close()
            End Try
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Create a new Thiessan polygon grid assigning numeric station ID to grid points; will remove layer and delete grid first if it exists
    ''' The new grid will have the same extents as the subbasins layer
    ''' </summary>
    ''' <param name="GridFile">Name of layer to create</param>
    ''' <param name="StationsLayer">Name of point layer containing stations</param>
    ''' <param name="StationIDField">Field containing Station ID</param>
    ''' <remarks></remarks>
    Friend Function CreateThiessanGrid(ByVal GridFile As String, ByVal StationsLayer As String, ByVal StationIDField As String) As Boolean
        Try
            Dim xmax, xmin, ymax, ymin As Double
            Dim hdr As New MapWinGIS.GridHeader
            With hdr
                .dX = Math.Round(Project.GridSize * Project.DistFactor, 2) 'convert meters to project units
                .dY = .dX
                .NodataValue = Integer.MinValue  'note: cannot do direct comparison to this value (round-off) instead compare to >=0.0
                GisUtil.ExtentsOfLayer(GisUtil.LayerIndex(Project.Layers.Subbasins.LayerName), xmax, xmin, ymax, ymin)
                .NumberCols = (xmax - xmin) / .dX
                .NumberRows = (ymax - ymin) / .dY
                .XllCenter = xmin + .dX / 2
                .YllCenter = ymin + .dY / 2
            End With

            Do While GisUtil.IsLayerByFileName(GridFile)
                GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
            Loop
            MapWinGeoProc.DataManagement.DeleteGrid(GridFile)

            Dim g As New MapWinGIS.Grid
            If Not g.CreateNew(GridFile, hdr, MapWinGIS.GridDataType.LongDataType, hdr.NodataValue, False, MapWinGIS.GridFileType.GeoTiff) Then _LastErrorMsg = "Unable to create grid: " & GridFile : Return False
            g.AssignNewProjection(GisUtil.ProjectProjection)

            Dim sfSub As New MapWinGIS.Shapefile
            sfSub.Open(Project.Layers.Subbasins.Filename)
            'to speed processing time, only consider points near boundaries of subbasin (+/- 2x extents size)
            With sfSub.Extents
                xmin = .xMin - (.xMax - .xMin) * 2
                xmax = .xMax + (.xMax - .xMin) * 2
                ymin = .yMin - (.yMax - .yMin) * 2
                ymax = .yMax + (.yMax - .yMin) * 2
            End With
            sfSub.Close()
            Dim CloseOnly As Boolean = True

loopagain:
            Dim sfSta As New MapWinGIS.Shapefile
            sfSta.Open(GisUtil.LayerFileName(StationsLayer))
            Dim lyridx As Integer = GisUtil.LayerIndex(StationsLayer)
            Dim fldidx As Integer = GisUtil.FieldIndex(lyridx, StationIDField)
            Dim dictSta As New Generic.Dictionary(Of Integer, MapWinGIS.Point)
            If sfSta.NumShapes = 0 Then
                _LastErrorMsg = "There are no station points in: " & StationsLayer
                Return False
            End If

            For i As Integer = 0 To sfSta.NumShapes - 1
                Dim StaID As String = sfSta.CellValue(fldidx, i)
                Dim StaNum As Integer = i + 1
                'For j As Integer = StaID.Length - 1 To 0 Step -1
                '    If Not IsNumeric(StaID.Substring(j, 1)) Then StaID = StaID.Remove(j, 1)
                'Next
                'If IsNumeric(StaID) Then
                '    StaNum = Val(StaID)
                'Else
                '    _LastErrorMsg = "A non-numeric station ID was encountered in CreateThiessanGrid."
                '    Return False
                'End If
                If dictSta.ContainsKey(StaNum) Then
                    _LastErrorMsg = "Station IDs are not unique in CreateTheissanGrid"
                    Return False
                End If
                Dim pt As MapWinGIS.Point = sfSta.QuickPoint(i, 0)
                If Not CloseOnly OrElse (pt.x > xmin AndAlso pt.x < xmax AndAlso pt.y > ymin And pt.y < ymax) Then dictSta.Add(StaNum, pt)
            Next

            If dictSta.Count = 0 Then
                If CloseOnly Then 'didn't find any close to subbasin--extend search area
                    CloseOnly = False
                    GoTo loopagain
                Else
                    _LastErrorMsg = "No Station IDs found in CreateTheissanGrid"
                    Return False
                End If
            End If

            'for each grid point, find the closest point
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    For c As Integer = 0 To .NumberCols - 1
                        Dim minDist As Double = Double.MaxValue
                        Dim minStaID As Integer = Integer.MinValue
                        For Each kv As KeyValuePair(Of Integer, MapWinGIS.Point) In dictSta
                            With kv.Value
                                Dim dist As Double = Math.Sqrt((.x - x) ^ 2 + (.y - y) ^ 2)
                                If dist < minDist Then
                                    minDist = dist
                                    minStaID = kv.Key
                                End If
                            End With
                        Next
                        ar(c) = minStaID
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress("Computing Thiessan grid...", r, .NumberRows - 1) Then Return False
                Next
            End With
            sfSta.Close()
            g.Save()
            g.Close()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function

    '''' <summary>
    '''' Given a stream shapefile, compute flow path grid where on-stream values have flowdirections and off-stream have no-data value
    '''' </summary>
    '''' <param name="StreamShapeFile"></param>
    '''' <param name="FlowDirectionGridFile"></param>
    '''' <param name="FlowPathGridFile"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Friend Function CreateFlowPathGrid(ByVal StreamShapeFile As String, ByVal FlowDirectionGridFile As String, ByVal FlowPathGridFile As String) As Boolean
    '    Dim sfStream As New MapWinGIS.Shapefile
    '    Dim gFlowDir As New MapWinGIS.Grid
    '    Dim gFlowPath As New MapWinGIS.Grid
    '    Try
    '        If Not sfStream.Open(StreamShapeFile) Then Return False
    '        If Not gFlowDir.Open(FlowDirectionGridFile) Then Return False
    '        If Not gFlowPath.Open(FlowPathGridFile) Then Return False
    '        'If Not sfStream.BeginPointInShapefile Then Return False
    '        If Not GridsIdentical(gFlowDir, gFlowPath) Then Return False
    '        With gFlowPath.Header
    '            Dim arFlowPath(.NumberCols - 1) As Single
    '            Dim arFlowDir(.NumberCols - 1) As Single
    '            For r As Integer = 0 To .NumberRows - 1
    '                Dim x, y As Double
    '                gFlowDir.CellToProj(0, r, x, y)
    '                If Not gFlowDir.GetRow(r, arFlowDir(0)) Then Return False
    '                For c As Integer = 0 To .NumberCols - 1
    '                    'see if any part of any stream polyline falls within this cell; if it does, set cell to flow direction; otherwise is nodatavalue
    '                    arFlowPath(c) = .NodataValue

    '                    Dim gridCellShape As New MapWinGIS.Shape
    '                    gridCellShape.Create(MapWinGIS.ShpfileType.SHP_POLYGON)
    '                    For i As Integer = 1 To 4
    '                        Dim pt As New MapWinGIS.Point
    '                        pt.x = x + .dX / 2 * Choose(i, -1, 1, 1, -1)
    '                        pt.y = y + .dY / 2 * Choose(i, -1, -1, 1, 1)
    '                        If Not gridCellShape.InsertPoint(pt, i = 1) Then Return False
    '                    Next

    '                    Dim gridCell As MapWinGeoProc.NTS.Topology.Geometries.Geometry = MapWinGeoProc.NTS_Adapter.ShapeToGeometry(gridCellShape)

    '                    For s As Integer = 0 To sfStream.NumShapes - 1
    '                        Dim polyline As MapWinGeoProc.NTS.Topology.Geometries.Geometry = MapWinGeoProc.NTS_Adapter.ShapeToGeometry(sfStream.Shape(s))
    '                        If polyline.Intersects(gridCell) Then
    '                            arFlowPath(c) = arFlowDir(c)
    '                            Exit For
    '                        End If
    '                    Next
    '                    x += .dX
    '                Next
    '                gFlowPath.PutRow(r, arFlowPath(0))
    '                If Not ProgressForm.SetProgress("Creating flow path grid...", r, .NumberRows - 1) Then Return False
    '            Next
    '        End With
    '        gFlowPath.AssignNewProjection(GisUtil.ProjectProjection)
    '        Return True
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '        Return False
    '    Finally
    '        sfStream.EndPointInShapefile()
    '        sfStream.Close()
    '        sfStream = Nothing
    '        gFlowDir.Close()
    '        gFlowDir = Nothing
    '        gFlowPath.Save()
    '        gFlowPath.Close()
    '        gFlowPath = Nothing
    '    End Try
    '    Return True
    'End Function

    ''' <summary>
    ''' Given a grid over a large extent, set all values that fall outside of the filter layer shapes to NoData
    ''' If filter layer is grid, will act as mask, setting all grid values to NoData if filter grid is NoData
    ''' </summary>
    ''' <param name="GridFile">Grid file that is to be filtered</param>
    ''' <param name="FilterFile">Grid or shape file to act as filter; if grid, uses NoData values as filter; if shapefile uses all shapes as filter </param>
    Friend Function FilterGrid(ByVal GridFile As String, ByVal FilterFile As String) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g, gFilter As New MapWinGIS.Grid
        Dim isShapeFile As Boolean = IO.Path.GetExtension(FilterFile).ToLower = ".shp"
        Try
            If Not g.Open(GridFile) Then _LastErrorMsg = "Unable to open grid: " & GridFile : Return False
            If isShapeFile Then
                If Not sfFilter.Open(FilterFile) Then _LastErrorMsg = "Unable to open shape file: " & FilterFile : Return False
                sfFilter.BeginPointInShapefile()
            Else
                If Not gFilter.Open(FilterFile) Then _LastErrorMsg = "Unable to open grid: " & FilterFile : Return False
                If Not GridsIdentical(g, gFilter) Then _LastErrorMsg = "Grids are not identical: " & GridFile & " & " & FilterFile : Return False
            End If
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1), arFilter(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return False
                    If Not isShapeFile AndAlso Not gFilter.GetRow(r, arFilter(0)) Then Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If isShapeFile Then
                            If sfFilter.PointInShapefile(x, y) = -1 Then ar(c) = .NodataValue
                        Else
                            If arFilter(c) = gFilter.Header.NodataValue Then ar(c) = .NodataValue
                        End If
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress("Filtering grid...", r, .NumberRows - 1) Then Return False
                Next
            End With
            g.AssignNewProjection(GisUtil.ProjectProjection)
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If isShapeFile Then
                sfFilter.EndPointInShapefile()
                sfFilter.Close()
            Else
                gFilter.Close()
            End If
            g.Save()
            g.Close()
            g = Nothing
        End Try
    End Function

    Friend Function GetGridValues(ByVal GridFile As String) As Generic.List(Of Single)
        Dim g As New MapWinGIS.Grid
        g.Open(GridFile)
        Dim lst As New Generic.List(Of Single)
        With g.Header
            For r As Integer = 0 To .NumberRows - 1
                Dim ar(.NumberCols - 1) As Single
                g.GetRow(r, ar(0))
                For c As Integer = 0 To .NumberCols - 1
                    If Not lst.Contains(ar(c)) And ar(c) <> .NodataValue Then lst.Add(ar(c))
                Next
            Next
        End With
        g.Close()
        Return lst
    End Function

    ''' <summary>
    ''' Determine if all grids are same size and located nearly at same place
    ''' </summary>
    ''' <param name="Grids">List of grids to check</param>
    Friend Function GridsCompatible(ByVal ParamArray Grids() As MapWinGIS.Grid) As Boolean
        If Grids.Length < 2 Then Return False
        For i As Integer = 1 To Grids.Length - 1
            If Not AreGridsCompatible(Grids(0), Grids(i), False) Then
                _LastErrorMsg = "The grid size, extents, or location are imcompatible: " & Grids(i).Filename
                Return False
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Determine if all grids are same size and located at exactly same place
    ''' </summary>
    ''' <param name="Grids">List of grids to check</param>
    Friend Function GridsIdentical(ByVal ParamArray Grids() As MapWinGIS.Grid) As Boolean
        If Grids.Length < 2 Then Return False
        For i As Integer = 1 To Grids.Length - 1
            If Not AreGridsCompatible(Grids(0), Grids(i), True) Then
                _LastErrorMsg = "The grid size, extents, or location are imcompatible: " & Grids(i).Filename
                Return False
            End If
        Next
        Return True
    End Function

    '''' <summary>
    '''' Set values in grid to factor taken from ID in shape file using lookup table
    '''' </summary>
    '''' <param name="SourceFile">Name of source shape file </param>
    '''' <param name="FieldName">Name of field containing lookup value</param>
    '''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    'Friend Function LookupGrid(ByVal SourceFile As String, ByVal FieldName As String, ByVal DestFile As String, ByVal dictLookup As Generic.Dictionary(Of String, clsLookup)) As Boolean
    '    Dim sfSource As MapWinGIS.Shapefile = Nothing
    '    Dim gDest As MapWinGIS.Grid = Nothing

    '    Try
    '        sfSource = New MapWinGIS.Shapefile
    '        If Not sfSource.Open(SourceFile) Then Return False
    '        sfSource.BeginPointInShapefile()

    '        gDest = New MapWinGIS.Grid
    '        If Not gDest.Open(DestFile, , True) Then Return False
    '        Dim Lookup As clsLookup = Nothing
    '        Dim lyrIndex As Integer = GisUtil.LayerIndex(SourceFile)
    '        Dim fldIndex As Integer = GisUtil.FieldIndex(lyrIndex, FieldName)
    '        With gDest.Header
    '            For r As Integer = 0 To .NumberRows - 1
    '                Dim x, y As Double
    '                gDest.CellToProj(0, r, x, y)
    '                Dim arV(.NumberCols - 1) As Single
    '                gDest.GetRow(r, arV(0))
    '                For c As Integer = 0 To .NumberCols - 1
    '                    Dim shpIndex As Integer = sfSource.PointInShapefile(x, y)
    '                    If shpIndex <> -1 Then
    '                        Dim ID As String = GisUtil.FieldValue(lyrIndex, shpIndex, fldIndex)
    '                        If dictLookup.TryGetValue(ID, Lookup) Then
    '                            arV(c) = Lookup.Factor
    '                        Else
    '                            arV(c) = .NodataValue
    '                        End If
    '                    End If
    '                    x += .dX
    '                Next
    '                gDest.PutRow(r, arV(0))
    '                If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
    '            Next
    '        End With
    '        gDest.Save()
    '        Return True
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '        Return False
    '    Finally
    '        If sfSource IsNot Nothing Then sfSource.Close()
    '        If gDest IsNot Nothing Then gDest.Close()
    '    End Try
    'End Function

    ''' <summary>
    ''' Set values in grid to factor taken from ID in grid file using lookup table
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file </param>
    ''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    Friend Function LookupGrid(ByVal SourceFile As String, ByVal DestFile As String, ByVal dictLookup As Generic.Dictionary(Of String, clsLookup)) As Boolean
        Dim gSource As MapWinGIS.Grid = Nothing
        Dim gDest As MapWinGIS.Grid = Nothing

        Try
            gSource = New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then LastErrorMsg = "Unable to open grid: " & SourceFile : Return False
            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , False) Then LastErrorMsg = "Unable to open grid: " & DestFile : Return False
            Dim Lookup As clsLookup = Nothing
            With gSource.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim arL(.NumberCols - 1), arV(.NumberCols - 1) As Single
                    gSource.GetRow(r, arL(0))
                    For c As Integer = 0 To .NumberCols - 1
                        Dim ID As String = arL(c)
                        If ID <> "" AndAlso dictLookup.TryGetValue(ID, Lookup) Then arV(c) = Lookup.Factor Else arV(c) = gDest.Header.NodataValue
                    Next
                    gDest.PutRow(r, arV(0))
                    If Not ProgressForm.SetProgress("Performing grid lookup...", r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If gSource IsNot Nothing Then gSource.Close()
            If gDest IsNot Nothing Then gDest.Close()
        End Try
    End Function

    ''' <summary>
    ''' Set values in destination grid based on delegate function
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file containing key values that will be looked up</param>
    ''' <param name="DestFile">Name of destination grid file (if doesn't exist, will be created)</param>
    ''' <param name="LookupFunction">Delegate function that takes source grid value and computes destination grid value</param>
    ''' <param name="DataType">Optional grid data type for grid to be created</param>
    Friend Function LookupGrid(ByVal SourceFile As String, ByVal DestFile As String, ByVal LookupFunction As LookupDelegate, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.UnknownDataType) As Boolean
        Dim gSource As MapWinGIS.Grid = Nothing
        Dim gDest As MapWinGIS.Grid = Nothing

        Try
            gSource = New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then _LastErrorMsg = "Unable to open grid: " & SourceFile : Return False

            'if caller wants grid created (type supplied), delete grid if it exists
            If DataType <> MapWinGIS.GridDataType.UnknownDataType AndAlso Not DeleteGrid(DestFile) Then Return False

            'if not found, create
            If Not My.Computer.FileSystem.FileExists(DestFile) Then
                If DataType = MapWinGIS.GridDataType.UnknownDataType Then _LastErrorMsg = "Destination grid not found and/or invalid grid datatype specified." : Return False
                If Not CreateGrid(DestFile, DataType) Then _LastErrorMsg = "Unable to create grid: " & DestFile : Return False
            End If

            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , True) Then _LastErrorMsg = "Unable to open grid: " & DestFile : Return False

            If Not GridsIdentical(gSource, gDest) Then _LastErrorMsg = "Source and destination grids are not identical structure." : Return False

            With gSource.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim arV(.NumberCols - 1) As Single
                    If Not gSource.GetRow(r, arV(0)) Then _LastErrorMsg = "Unable to get row from source grid." : Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If arV(c) = .NodataValue Then
                            arV(c) = gDest.Header.NodataValue
                        Else
                            arV(c) = LookupFunction(arV(c))
                        End If
                    Next
                    If Not gDest.PutRow(r, arV(0)) Then _LastErrorMsg = "Unable to put row to destination grid." : Return False
                    If Not ProgressForm.SetProgress("Performing grid lookup...", r, .NumberRows - 1) Then Return False
                Next
            End With
            If Not gSource.Close() Then _LastErrorMsg = "Unable to close grid: " & SourceFile : Return False
            gSource = Nothing
            If Not gDest.Save() Then _LastErrorMsg = "Unable to save grid: " & DestFile : Return False
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If gSource IsNot Nothing AndAlso Not gSource.Close() Then _LastErrorMsg = "Unable to close grid: " & SourceFile
            If gDest IsNot Nothing AndAlso Not gDest.Close() Then _LastErrorMsg = "Unable to close grid: " & DestFile
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid)
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file</param>
    ''' <param name="DestFile">Name of destination grid file that will be created (if blank, will put results back in source grid)</param>
    ''' <returns>True if successful</returns>
    Friend Function MultiplyGrid(ByVal SourceFile As String, ByVal Multiplier As Double, Optional ByVal DestFile As String = "") As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing

            With gSource
                If .Open(SourceFile) Then
                    If DestFile <> "" Then
                        gDest = New MapWinGIS.Grid
                        If Not gDest.Open(DestFile) Then Return False
                        Debug.Assert(gSource.Header.NumberCols = gDest.Header.NumberCols AndAlso gSource.Header.NumberRows = gDest.Header.NumberRows)
                    End If
                    With .Header
                        Dim arS(.NumberCols - 1), arD(.NumberCols - 1) As Single
                        For r As Integer = 0 To .NumberRows - 1
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then arD(c) = Math.Round(arS(c) * Multiplier, PRECISION) Else arD(c) = .NodataValue
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress("Multiplying grid...", r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} grid file in MultiplyGrid; error message was: {1}", SourceFile, .ErrorMsg(.LastErrorCode)))
                    Return False
                End If
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid)
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file</param>
    ''' <param name="MultFile">Name of grid file containing multipliers</param>
    ''' <param name="DestFile">Name of destination grid file that will be created (if blank, will put results back in source grid)</param>
    ''' <returns>True if successful</returns>
    Friend Function MultiplyGrid(ByVal SourceFile As String, ByVal MultFile As String, Optional ByVal DestFile As String = "") As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gMult As New MapWinGIS.Grid

            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SourceFile) AndAlso gMult.Open(MultFile) Then
                    If DestFile <> "" Then
                        gDest = New MapWinGIS.Grid
                        If Not gDest.Open(DestFile) Then Return False
                        Debug.Assert(gSource.Header.NumberCols = gMult.Header.NumberCols AndAlso gSource.Header.NumberRows = gMult.Header.NumberRows)
                    End If
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arS(.NumberCols - 1), arD(.NumberCols - 1), arM(.NumberCols - 1) As Single
                            gSource.GetRow(r, arS(0))
                            gMult.GetRow(r, arM(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue And arM(c) <> .NodataValue Then arD(c) = Math.Round(arS(c) * arM(c), PRECISION) Else arD(c) = .NodataValue
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress("Multiplying grid...", r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                    gMult.Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} and/or {1} grid files in MultiplyGrid.", SourceFile, MultFile))
                    Return False
                End If
                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid) (Special version to compute LS factor)
    ''' </summary>
    ''' <param name="SlopeGridFile">Name of source grid file (slopes)</param>
    ''' <param name="LSGridFile">Name of destination LS grid file that will be created</param>
    ''' <returns>True if successful</returns>
    Friend Function MultiplyGrid_LS(ByVal SlopeGridFile As String, ByVal LSGridFile As String, ByVal SlopeLength As Double) As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SlopeGridFile) Then
                    gDest = New MapWinGIS.Grid
                    If Not gDest.CreateNew(LSGridFile, .Header, .DataType, .Header.NodataValue, False) Then
                        Throw New Exception(String.Format("Unable to create new grid file {0} in MultiplyGrid; error message was: {1}", LSGridFile, gDest.ErrorMsg(gDest.LastErrorCode)))
                        Return False
                    End If
                    Debug.Assert(gSource.Header.NumberCols = gDest.Header.NumberCols AndAlso gSource.Header.NumberRows = gDest.Header.NumberRows)
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arS(.NumberCols - 1), arD(.NumberCols - 1), arM(.NumberCols - 1) As Single
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then
                                    Dim LS As Double
                                    Dim Slope As Double = arS(c)
                                    'Dim SlopeLength As Double = Project.GridSize 'in meters
                                    Dim NN As Double
                                    If Slope < 0.01 Then
                                        NN = 0.2
                                    ElseIf Slope < 0.03 Then
                                        NN = 0.3
                                    ElseIf Slope < 0.05 Then
                                        NN = 0.4
                                    Else
                                        NN = 0.5
                                    End If
                                    'note: formula taken from http://www.omafra.gov.on.ca/english/engineer/facts/00-001.htm
                                    'this is equivalent to equation used in WCS, except WCS used constant value of 0.5 for NN and used English units
                                    'note: this is slightly different from equation in GBMM guide which uses sin(theta) where theta = arctan(slope)
                                    '      in place of slope. These two values are computationally nearly the same for the range of slopes
                                    '      the equation is valid for (for slope = 5%, sin(arctan(0.05))=.04994)
                                    LS = (0.065 + 4.56 * Slope + 65.41 * Slope ^ 2) * (SlopeLength / 22.1) ^ NN
                                    'note: this equation is most likely invalid for very large or small slopes; don't let computed LS exceed a range taken by examining the table in the above reference
                                    '
                                    LS = Math.Max(LS, 0.0691)
                                    LS = Math.Min(LS, 10)
                                    arD(c) = Math.Round(LS, PRECISION)
                                Else
                                    arD(c) = .NodataValue
                                End If
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress("Computing LS factors...", r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} grid file in MultiplyGrid_LS.", SlopeGridFile))
                    Return False
                End If
                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Take high-resolution source grid and create lower resolution destination grid
    ''' The source and destination grids must already exist
    ''' </summary>
    ''' <param name="SourceGridFile">Name of high-resolution grid file</param>
    ''' <param name="DestGridFile">Name of existing lower-resolution grid file that will be written to</param>
    ''' <param name="MaskShapeFile">If non-blank, is polygon shapefile name used to mask output file</param>
    Friend Function ResampleGrid(ByVal SourceGridFile As String, ByVal DestGridFile As String, Optional ByVal MaskShapeFile As String = "") As Boolean
        Dim gSource, gDest As New MapWinGIS.Grid
        Dim sfMask As New MapWinGIS.Shapefile
        Try
            If Not gSource.Open(SourceGridFile, , False) Then LastErrorMsg = "Unable to open: " & SourceGridFile : Return False
            If Not gDest.Open(DestGridFile, , False) Then LastErrorMsg = "Unable to open: " & DestGridFile : Return False

            If MaskShapeFile <> "" Then
                sfMask.Open(MaskShapeFile)
                sfMask.BeginPointInShapefile()
            End If

            Dim SourceCols, SourceRows As Integer
            With gSource.Header
                SourceCols = .NumberCols
                SourceRows = .NumberRows
            End With
            With gDest.Header
                'get source grid value at center of destination grid
                Dim ar(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    Dim x As Double, y As Double, cc As Integer, rr As Integer
                    gDest.CellToProj(0, r, x, y)
                    For c As Integer = 0 To .NumberCols - 1
                        ar(c) = .NodataValue
                        gSource.ProjToCell(x, y, cc, rr)
                        If cc >= 0 AndAlso cc <= SourceCols - 1 AndAlso rr >= 0 AndAlso rr <= SourceRows - 1 Then
                            If gSource.Value(cc, rr) <> gSource.Header.NodataValue Then
                                If MaskShapeFile = "" OrElse sfMask.PointInShapefile(x, y) <> -1 Then
                                    ar(c) = gSource.Value(cc, rr)
                                End If
                            End If
                        End If
                        x += .dX
                    Next
                    gDest.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress("Resampling grid...", r, .NumberRows - 1) Then Return False
                Next
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If gSource IsNot Nothing Then gSource.Close()
            If gDest IsNot Nothing Then gDest.Save() : gDest.Close()
            If sfMask IsNot Nothing Then sfMask.EndPointInShapefile() : sfMask.Close()
        End Try
    End Function

    ''' <summary>
    ''' Set all grid values to fixed value 
    ''' </summary>
    ''' <param name="DestGridFile">Destination grid file (must already exist)</param>
    ''' <param name="Value">Constant value to set entire grid to</param>
    Friend Function SetGrid(ByVal DestGridFile As String, ByVal Value As Double) As Boolean
        Dim gDest As New MapWinGIS.Grid
        Try
            With gDest
                .Open(DestGridFile, , False)
                With .Header
                    For r As Integer = 0 To .NumberRows - 1
                        Dim ar(.NumberCols - 1) As Single
                        For c As Integer = 0 To .NumberCols - 1
                            ar(c) = Value
                        Next c
                        gDest.PutRow(r, ar(0))
                        If Not ProgressForm.SetProgress("Setting grid values...", r, .NumberRows - 1) Then Return False
                    Next
                End With
                .Save()
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gDest.Close()
            gDest = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Given a grid over a large extent, set all values that fall inside a filter layer shape to specified value
    ''' </summary>
    ''' <param name="DestGridFile">Destination grid file (must already exist)</param>
    ''' <param name="FilterShapeFile">Name of shape file to use as filter</param>
    ''' <param name="ShapeIndex">Index of shape in shape file to use as filter</param>
    ''' <param name="Value">Constant value to set entire grid to</param>
    Friend Function SetGrid(ByVal DestGridFile As String, ByVal FilterShapeFile As String, ByVal ShapeIndex As Integer, ByVal Value As Double) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(FilterShapeFile)))
            sfFilter.BeginPointInShapefile()
            g.Open(DestGridFile, , False)
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If sfFilter.PointInShapefile(x, y) = ShapeIndex Then ar(c) = Value
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress("Setting grid values...", r, .NumberRows - 1) Then Return False
                Next
            End With
            sfFilter.EndPointInShapefile()
            g.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            sfFilter.Close()
            g.Close()
        End Try
    End Function

    ''' <summary>
    ''' Given an integer grid where the value represents a station ID (like a theissan), tabulate all areas within the selected shape
    ''' Returned dictionary key is the integer grid value, value is area in sq km
    ''' </summary>
    Friend Function TabulateAreasInShape(ByVal GridFile As String, ByVal ShapeFile As String, ByVal ShapeIndex As Integer) As Generic.Dictionary(Of Integer, Double)
        Dim dict As New Generic.Dictionary(Of Integer, Double)
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(ShapeFile)
            sfFilter.BeginPointInShapefile()
            g.Open(GridFile)
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return Nothing
                    For c As Integer = 0 To .NumberCols - 1
                        If sfFilter.PointInShape(ShapeIndex, x, y) Then
                            Dim priorarea As Double = 0
                            If dict.ContainsKey(ar(c)) Then
                                priorarea = dict(ar(c))
                                dict.Remove(ar(c))
                            End If
                            dict.Add(ar(c), priorarea + Project.CellAreaKm)
                        End If
                        x += .dX
                    Next
                    If Not ProgressForm.SetProgress("Tabulating areas...", r, .NumberRows - 1) Then Return Nothing
                Next
            End With
            Return dict
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        Finally
            If sfFilter IsNot Nothing Then
                sfFilter.EndPointInShapefile()
                sfFilter.Close()
            End If
            If g IsNot Nothing Then g.Close()
        End Try
    End Function

    ''' <summary>
    ''' Apply predefined coloring scheme to grid layer
    ''' </summary>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="ColoringScheme">Predefined coloring scheme</param>
    Friend Sub ApplyColoringScheme(ByVal LayerName As String, ByVal ColoringScheme As MapWinGIS.PredefinedColorScheme)
        Try
            Dim g As New MapWinGIS.Grid
            If Not g.Open(GisUtil.LayerFileName(LayerName), , False) Then Exit Sub

            Dim scheme As New MapWinGIS.GridColorScheme
            With scheme
                .UsePredefined(g.Minimum * 0.999, g.Maximum * 1.001, ColoringScheme) 'prevent round-off errors from causing empty cells
            End With
            g.Close()

            GisUtil.ColoringScheme(GisUtil.LayerIndex(LayerName)) = scheme
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Apply custom coloring scheme to grid layer (two ranges, each with separate color)
    ''' </summary>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="Caption1">Caption assigned to lower range</param>
    ''' <param name="Color1">Color assigned to lower range</param>
    ''' <param name="Caption2">Caption assigned to upper range</param>
    ''' <param name="Color2">Color assigned to upper range</param>
    ''' <param name="BreakValue">Grid value that separates two ranges</param>
    Friend Sub ApplyColoringScheme(ByVal LayerName As String, ByVal Caption1 As String, ByVal Color1 As System.Drawing.Color, ByVal Caption2 As String, ByVal Color2 As System.Drawing.Color, ByVal BreakValue As Double)
        Try
            If Not GisUtil.IsLayer(LayerName) Then Return

            Dim g As New MapWinGIS.Grid
            g.Open(GisUtil.LayerFileName(LayerName), , False)

            Dim scheme As New MapWinGIS.GridColorScheme
            Dim b1 As New MapWinGIS.GridColorBreak
            With b1
                .Caption = Caption1
                .LowValue = Math.Min(g.Minimum, g.Header.NodataValue)
                .HighValue = BreakValue
                .ColoringType = MapWinGIS.ColoringType.Gradient
                .LowColor = System.Convert.ToUInt32(RGB(Color1.R, Color1.G, Color1.B))
                .HighColor = .LowColor
            End With
            scheme.InsertBreak(b1)

            Dim b2 As New MapWinGIS.GridColorBreak
            With b2
                .Caption = Caption2
                .LowValue = BreakValue * 1.0001
                .HighValue = g.Maximum * 1.0001
                .ColoringType = MapWinGIS.ColoringType.Gradient
                .LowColor = System.Convert.ToUInt32(RGB(Color2.R, Color2.G, Color2.B))
                .HighColor = .LowColor
            End With
            scheme.InsertBreak(b2)

            g.Close()

            GisUtil.ColoringScheme(GisUtil.LayerIndex(LayerName)) = scheme
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Remove grid layer if it exists, and delete all files associated with grid
    ''' </summary>
    ''' <param name="GridFile">Name of grid file to delete</param>
    Friend Function DeleteGrid(ByVal GridFile As String) As Boolean
        If GisUtil.IsLayerByFileName(GridFile) Then GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
        For Each fn As String In My.Computer.FileSystem.GetFiles(IO.Path.GetDirectoryName(GridFile), FileIO.SearchOption.SearchTopLevelOnly, IO.Path.GetFileNameWithoutExtension(GridFile) & ".*")
            Try
                My.Computer.FileSystem.DeleteFile(fn)
            Catch ex As Exception
                LastErrorMsg = "Unable to delete file: " & fn & vbCr & vbCr & "Note that MapWindow has a bug which will lock files if you use the Remove Group feature; instead, remove layers individually."
                Return False
            End Try
        Next
        Return True
    End Function

    ''' <summary>
    ''' Store error messages from deep within routines so caller can handle later; if string is empty, no error occurred
    ''' </summary>
    Friend Property LastErrorMsg() As String
        Get
            Dim u As New MapWinGIS.Utils
            If u.LastErrorCode = 0 Then
                Return _LastErrorMsg
            Else
                Return _LastErrorMsg & vbCr & vbCr & "Detailed error message was: " & vbCr & vbCr & u.ErrorMsg(u.LastErrorCode)
            End If
        End Get
        Set(ByVal value As String)
            _LastErrorMsg = value
        End Set
    End Property

    ''' <summary>
    ''' Given shape file with numeric field, create or overwrite grid values
    ''' </summary>
    ''' <param name="ShapeFile">Name of shape file</param>
    ''' <param name="FieldName">Name of field with numeric values (if blank, will use shape index plus 1)</param>
    ''' <param name="GridFile">Name of new or existing grid file; if not found will be created</param>
    Friend Function ShapefileToGrid(ByVal ShapeFile As String, ByVal FieldName As String, ByVal GridFile As String) As Boolean
        Dim sf As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid

        Try
            If Not GisUtil.IsLayerByFileName(ShapeFile) Then LastErrorMsg = "Shape file is not a layer: " & ShapeFile : Return False
            If Not sf.Open(ShapeFile) Then LastErrorMsg = "Unable to open shape file: " & ShapeFile : Return False
            sf.BeginPointInShapefile()
            Dim fldidx As Integer = -1
            If FieldName <> "" Then
                If Not GisUtil.IsField(GisUtil.LayerIndex(ShapeFile), FieldName) Then LastErrorMsg = "Invalid field specified: " & FieldName : Return False
                fldidx = GisUtil.FieldIndex(GisUtil.LayerIndex(ShapeFile), FieldName)
            End If
            If Not g.Open(GridFile) Then LastErrorMsg = "Unable to open grid file: " & GridFile : Return False

            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return Nothing
                    For c As Integer = 0 To .NumberCols - 1
                        Dim shapenum As Integer = sf.PointInShapefile(x, y)
                        If shapenum <> -1 Then
                            If FieldName = "" Then
                                ar(c) = shapenum + 1
                            Else
                                ar(c) = Val(sf.CellValue(fldidx, shapenum))
                            End If
                        End If
                        x += .dX
                    Next
                    If Not g.PutRow(r, ar(0)) Then LastErrorMsg = "Unable to put data in grid." : Return False
                    If Not ProgressForm.SetProgress("Converting shape file to grid...", r, .NumberRows - 1) Then Return Nothing
                Next
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            sf.EndPointInShapefile()
            sf.Close()
            g.Save()
            g.Close()
        End Try
    End Function
End Module
