Friend Class clsLookup
    Friend Description As String
    Friend Factor As Double
    Sub New(ByVal _Description As String, ByVal _Factor As Double)
        Description = _Description
        Factor = _Factor
    End Sub
End Class

Friend Module modGrid


    Friend Const PRECISION As Integer = 4 'this is number of decimal places to round real numbers off to when storing in grids

    ''' <summary>
    ''' Given high-resolution source grid, average all values to a destination grid
    ''' The source and destination grids must already exist (the destination should already be filtered for the desired area)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function AverageGrid(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource, gDest As New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then Return False
            If Not gDest.Open(DestFile, , False) Then Return False

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
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
    ''' Create a new grid and return new grid file name; will remove layer and delete grid first
    ''' </summary>
    ''' <param name="GridName">Name of layer to create</param>
    ''' <returns>Grid filename</returns>
    ''' <remarks></remarks>
    Friend Function CreateGrid(ByVal GridFolder As String, ByVal GridName As String, Optional ByVal gTemplate As MapWinGIS.Grid = Nothing, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.FloatDataType) As String
        Dim GridFile As String = String.Format("{0}\{1}.tif", GridFolder, GridName)
        If CreateGrid(GridFile, gTemplate, DataType) Then Return GridFile Else Return ""
    End Function

    ''' <summary>
    ''' Create a new grid and return new grid file name; will remove layer and delete grid first
    ''' </summary>
    ''' <param name="GridFile">Name of grid file to create</param>
    ''' <returns>Grid filename</returns>
    ''' <remarks></remarks>
    Friend Function CreateGrid(ByVal GridFile As String, Optional ByVal gTemplate As MapWinGIS.Grid = Nothing, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.FloatDataType) As Boolean
        Try
            Dim hdr As New MapWinGIS.GridHeader
            With hdr
                If gTemplate Is Nothing Then
                    .dX = Math.Round(Project.GridSize * Project.DistFactor, 2) 'convert meters to project units
                    .dY = .dX
                    .NodataValue = -1 'note: cannot do direct comparison to this value (round-off) instead compare to >=0.0
                    Dim xmax, xmin, ymax, ymin As Double
                    GisUtil.ExtentsOfLayer(GisUtil.LayerIndex(Project.SubbasinLayer), xmax, xmin, ymax, ymin)
                    .NumberCols = (xmax - xmin) / .dX
                    .NumberRows = (ymax - ymin) / .dY
                    .XllCenter = xmin + .dX / 2
                    .YllCenter = ymin + .dY / 2
                Else
                    .CopyFrom(gTemplate.Header)
                End If
            End With

            Do While GisUtil.IsLayerByFileName(GridFile)
                GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
            Loop
            MapWinGeoProc.DataManagement.DeleteGrid(GridFile)

            Dim g As New MapWinGIS.Grid
            Try
                If g.CreateNew(GridFile, hdr, DataType, hdr.NodataValue) Then
                    g.AssignNewProjection(GisUtil.ProjectProjection)
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            Finally
                g.Close()
            End Try
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    '''' <summary>
    '''' Create a new Thiessan polygon grid assigning numeric station ID to grid points; will remove layer and delete grid first if it exists
    '''' The new grid will have the same extents as the subbasins layer
    '''' </summary>
    '''' <param name="GridFile">Name of layer to create</param>
    '''' <param name="StationsLayer">Name of point layer containing stations</param>
    '''' <param name="StationIDField">Field containing Station ID</param>
    '''' <remarks></remarks>
    'Friend Function CreateThiessanGrid(ByVal GridFile As String, ByVal StationsLayer As String, ByVal StationIDField As String) As Boolean
    '    Try
    '        Dim hdr As New MapWinGIS.GridHeader
    '        With hdr
    '            .dX = Math.Round(Project.GridSize * Project.DistFactor, 2) 'convert meters to project units
    '            .dY = .dX
    '            .NodataValue = -1 'note: cannot do direct comparison to this value (round-off) instead compare to >=0.0
    '            Dim xmax, xmin, ymax, ymin As Double
    '            GisUtil.ExtentsOfLayer(GisUtil.LayerIndex(Project.Layers.Subbasins.LayerName), xmax, xmin, ymax, ymin)
    '            .NumberCols = (xmax - xmin) / .dX
    '            .NumberRows = (ymax - ymin) / .dY
    '            .XllCenter = xmin + .dX / 2
    '            .YllCenter = ymin + .dY / 2
    '        End With

    '        Do While GisUtil.IsLayerByFileName(GridFile)
    '            GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
    '        Loop
    '        MapWinGeoProc.DataManagement.DeleteGrid(GridFile)

    '        Dim g As New MapWinGIS.Grid
    '        If Not g.CreateNew(GridFile, hdr, MapWinGIS.GridDataType.ShortDataType, hdr.NodataValue, False, MapWinGIS.GridFileType.GeoTiff) Then Return False
    '        g.AssignNewProjection(GisUtil.ProjectProjection)

    '        Dim sfSub As New MapWinGIS.Shapefile
    '        Dim subidx As Integer = GisUtil.LayerIndex(Project.Layers.Subbasins.LayerName)
    '        sfSub.Open(subidx)

    '        Dim sfSta As New MapWinGIS.Shapefile
    '        sfSta.Open(GisUtil.LayerFileName(StationsLayer))
    '        Dim lyridx As Integer = GisUtil.LayerIndex(StationsLayer)
    '        Dim fldidx As Integer = GisUtil.FieldIndex(lyridx, StationIDField)
    '        Dim dictSta As New Generic.Dictionary(Of Integer, MapWinGIS.Point)
    '        If sfSta.NumShapes = 0 Then
    '            ErrorMsg("There are no station points in CreateTheissanGrid")
    '            Return False
    '        End If
    '        For i As Integer = 0 To sfSta.NumShapes - 1
    '            Dim StaID As Integer = sfSta.CellValue(fldidx, i)
    '            If dictSta.ContainsKey(StaID) Then
    '                ErrorMsg("Station IDs are not unique in CreateTheissanGrid")
    '                Return False
    '            End If
    '            'dictSta.Add(StaID, sfSta.QuickPoint(i, 0))
    '            dictSta.Add(i, sfSta.QuickPoint(i, 0))
    '        Next

    '        'for each grid point, find the closest point
    '        With g.Header
    '            For r As Integer = 0 To .NumberRows - 1
    '                Dim x, y As Double
    '                g.CellToProj(0, r, x, y)
    '                Dim ar(.NumberCols - 1) As Single
    '                For c As Integer = 0 To .NumberCols - 1
    '                    If True Or sfSta.PointInShapefile(x, y) <> -1 Then
    '                        Dim minDist As Double = Double.MaxValue
    '                        Dim minStaID As Integer = Integer.MinValue
    '                        For Each kv As KeyValuePair(Of Integer, MapWinGIS.Point) In dictSta
    '                            With kv.Value
    '                                Dim dist As Double = Math.Sqrt((.x - x) ^ 2 + (.y - y) ^ 2)
    '                                If dist < minDist Then
    '                                    minDist = dist
    '                                    minStaID = kv.Key
    '                                End If
    '                            End With
    '                        Next
    '                        ar(c) = minStaID
    '                    Else
    '                        ar(c) = g.Header.NodataValue
    '                    End If
    '                    x += .dX
    '                Next
    '                g.PutRow(r, ar(0))
    '                'If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
    '            Next
    '        End With
    '        sfSta.Close()
    '        g.Save()
    '        g.Close()
    '        Return True
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' Given a grid over a large extent, set all values that fall outside of the filter layer shapes to NoData
    ''' </summary>
    Friend Function FilterGrid(ByVal GridFile As String, ByVal FilterFile As String) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(FilterFile)))
            sfFilter.BeginPointInShapefile()
            g.Open(GridFile)
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If sfFilter.PointInShapefile(x, y) = -1 Then ar(c) = .NodataValue
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            g.AssignNewProjection(GisUtil.ProjectProjection)
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            sfFilter.EndPointInShapefile()
            sfFilter.Close()
            g.Save()
            g.Close()
            g = Nothing
        End Try
    End Function

    Friend Function GetGridValues(ByVal GridFile As String) As Generic.List(Of Single)
        Dim g As New MapWinGIS.Grid
        Dim lst As New Generic.List(Of Single)
        With g.Header
            For r As Integer = 0 To .NumberRows - 1
                Dim ar(.NumberCols - 1) As Single
                g.GetRow(r, ar(0))
                For c As Integer = 0 To .NumberCols - 1
                    If Not lst.Contains(ar(c)) Then lst.Add(ar(c))
                Next
            Next
        End With
        Return lst
    End Function

    ''' <summary>
    ''' Set values in grid to factor taken from ID in shape file using lookup table
    ''' </summary>
    ''' <param name="SourceFile">Name of source shape file </param>
    ''' <param name="FieldName">Name of field containing lookup value</param>
    ''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    Friend Function LookupGrid(ByVal SourceFile As String, ByVal FieldName As String, ByVal DestFile As String, ByVal dictLookup As Generic.Dictionary(Of String, clsLookup)) As Boolean
        Dim sfSource As MapWinGIS.Shapefile = Nothing
        Dim gDest As MapWinGIS.Grid = Nothing

        Try
            sfSource = New MapWinGIS.Shapefile
            If Not sfSource.Open(SourceFile) Then Return False
            sfSource.BeginPointInShapefile()

            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , True) Then Return False
            Dim Lookup As clsLookup = Nothing
            Dim lyrIndex As Integer = GisUtil.LayerIndex(SourceFile)
            Dim fldIndex As Integer = GisUtil.FieldIndex(lyrIndex, FieldName)
            With gDest.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    gDest.CellToProj(0, r, x, y)
                    Dim arV(.NumberCols - 1) As Single
                    gDest.GetRow(r, arV(0))
                    For c As Integer = 0 To .NumberCols - 1
                        Dim shpIndex As Integer = sfSource.PointInShapefile(x, y)
                        If shpIndex <> -1 Then
                            Dim ID As String = GisUtil.FieldValue(lyrIndex, shpIndex, fldIndex)
                            If dictLookup.TryGetValue(ID, Lookup) Then
                                arV(c) = Lookup.Factor
                            Else
                                arV(c) = .NodataValue
                            End If
                        End If
                        x += .dX
                    Next
                    gDest.PutRow(r, arV(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If sfSource IsNot Nothing Then sfSource.Close()
            If gDest IsNot Nothing Then gDest.Close()
        End Try
    End Function

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
            If Not gSource.Open(SourceFile, , False) Then Return False
            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , False) Then Return False
            Dim Lookup As clsLookup = Nothing
            With gSource.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim arL(.NumberCols - 1), arV(.NumberCols - 1) As Single
                    gSource.GetRow(r, arL(0))
                    For c As Integer = 0 To .NumberCols - 1
                        Dim ID As String = arL(c)
                        If ID <> "" AndAlso dictLookup.TryGetValue(ID, Lookup) Then arV(c) = Lookup.Factor Else arV(c) = .NodataValue
                    Next
                    gDest.PutRow(r, arV(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
                    End If
                    With .Header
                        Dim arS(.NumberCols - 1), arD(.NumberCols - 1) As Single
                        For r As Integer = 0 To .NumberRows - 1
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then arD(c) = Math.Round(arS(c) * Multiplier, PRECISION) Else arD(c) = .NodataValue
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
    ''' <param name="SourceFile">Name of source grid file (slopes)</param>
    ''' <param name="DestFile">Name of destination grid file that will be created</param>
    ''' <returns>True if successful</returns>
    Friend Function MultiplyGrid_LS(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SourceFile) Then
                    gDest = New MapWinGIS.Grid
                    If Not gDest.CreateNew(DestFile, .Header, .DataType, .Header.NodataValue, False) Then
                        Throw New Exception(String.Format("Unable to create new grid file {0} in MultiplyGrid; error message was: {1}", DestFile, gDest.ErrorMsg(gDest.LastErrorCode)))
                        Return False
                    End If
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arS(.NumberCols - 1), arD(.NumberCols - 1), arM(.NumberCols - 1) As Single
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then
                                    Dim LS As Double
                                    Dim Slope As Double = arS(c)
                                    Dim SlopeLength As Double = Project.GridSize 'in meters
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
                                    LS = (0.065 + 4.56 * Slope + 65.41 * Slope ^ 2) * (SlopeLength / 22.1) ^ NN
                                    arD(c) = Math.Round(LS, PRECISION)
                                Else
                                    arD(c) = .NodataValue
                                End If
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
                    Throw New Exception(String.Format("Unable to open {0} grid file in MultiplyGrid_LS.", SourceFile))
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
    ''' The source and destination grids must already exist (the destination should already be filtered for the desired area)
    ''' </summary>
    Friend Function ResampleGrid(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource, gDest As New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then Return False
            If Not gDest.Open(DestFile, , False) Then Return False

            Dim SourceCols, SourceRows As Integer
            With gSource.Header
                SourceCols = .NumberCols
                SourceRows = .NumberRows
            End With
            With gDest.Header
                'get average value within the larger destination grid from the smaller source grid
                Dim ar(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    Dim x As Double, y As Double, cc As Integer, rr As Integer
                    gDest.CellToProj(0, r, x, y)
                    For c As Integer = 0 To .NumberCols - 1
                        gSource.ProjToCell(x, y, cc, rr)
                        'If cc >= 0 AndAlso cc <= gSource.Header.NumberCols - 1 AndAlso rr >= 0 AndAlso rr <= gSource.Header.NumberRows - 1 Then
                        If cc >= 0 AndAlso cc <= SourceCols - 1 AndAlso rr >= 0 AndAlso rr <= SourceRows - 1 Then
                            ar(c) = gSource.Value(cc, rr)
                        Else
                            ar(c) = .NodataValue
                        End If
                        x += .dX
                    Next
                    gDest.PutRow(r, ar(0))
                    'Debug.Print(CInt(My.Computer.Info.AvailablePhysicalMemory / 1000000.0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gSource.Close()
            gDest.Save()
            gDest.Close()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Set all grid values to fixed value 
    ''' </summary>
    Friend Function SetGrid(ByVal DestFile As String, ByVal Value As Double) As Boolean
        Dim gDest As New MapWinGIS.Grid
        Try
            With gDest
                .Open(DestFile, , False)
                With .Header
                    For r As Integer = 0 To .NumberRows - 1
                        Dim ar(.NumberCols - 1) As Single
                        For c As Integer = 0 To .NumberCols - 1
                            ar(c) = Value
                        Next c
                        gDest.PutRow(r, ar(0))
                        If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
    Friend Function SetGrid(ByVal GridFile As String, ByVal FilterFile As String, ByVal ShapeIndex As Integer, ByVal Value As Double) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(FilterFile)))
            sfFilter.BeginPointInShapefile()
            g.Open(GridFile, , False)
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
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
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
            Dim g As New MapWinGIS.Grid
            g.Open(GisUtil.LayerFileName(LayerName), , False)

            Dim scheme As New MapWinGIS.GridColorScheme
            Dim b1 As New MapWinGIS.GridColorBreak
            With b1
                .Caption = Caption1
                .LowValue = g.Minimum
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
End Module
