Imports atcData
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGIS

Module UpdatePointsFromData

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalShapeFileName As String = "NWIS_Stations_discharge.shp"
        Dim lOpenedOriginal As Boolean = False
        Dim lNewShapeFileName As String = "NWIS_Stations_Updated.shp"
        Dim lSplitConstituents As Boolean = True
        Dim lDataType As String = "q"
        Dim lCountStartDate As Double = 0
        Dim lCountEndDate As Double = 0
        Dim lDF As New atcDateFormat
        lDF.DateSeparator = "-"
        lDF.IncludeHours = False
        lDF.IncludeMinutes = False
        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Original shape file name", lOriginalShapeFileName)
            .Add("New shape file name", lNewShapeFileName)
            .Add("Split Constituents", lSplitConstituents)
            .Add("Data Type (q, qw, gw, sv, peak)", lDataType)
            .Add("Count Start Date", "1800-01-01")
            .Add("Count End Date", "2100-01-01")
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("About to create new station shape file(s) based on currently loaded data", lUserParms) Then
            With lUserParms
                lOriginalShapeFileName = .ItemByKey("Original shape file name")
                lNewShapeFileName = .ItemByKey("New shape file name")
                lSplitConstituents = .ItemByKey("Split Constituents")
                lDataType = .ItemByKey("Data Type (q, qw, gw, sv, peak)")
                lCountStartDate = Date.Parse(.ItemByKey("Count Start Date")).ToOADate
                lCountEndDate = Date.Parse(.ItemByKey("Count End Date")).ToOADate
            End With

            If Not IO.File.Exists(lOriginalShapeFileName) Then
                lOriginalShapeFileName = FindFile("Find station shape file", lOriginalShapeFileName, "shp", "*.shp|*.shp", True)
            End If

            If Not IO.File.Exists(lOriginalShapeFileName) Then
                Logger.Msg(lOriginalShapeFileName, "Could not find station shape file")
            Else
                Dim lOriginalShapeFile As Shapefile = Nothing
                For Each lLayer As Layer In aMapWin.Layers
                    If lLayer.FileName.ToLower = lOriginalShapeFileName.ToLower Then
                        lOriginalShapeFile = lLayer.GetObject
                    End If
                Next
                If lOriginalShapeFile Is Nothing Then
                    lOriginalShapeFile = New Shapefile
                    If lOriginalShapeFile.Open(lOriginalShapeFileName) Then
                        lOpenedOriginal = True
                    Else
                        Logger.Msg(lOriginalShapeFileName, "Could not open station shape file")
                        lOriginalShapeFile = Nothing
                    End If
                End If

                If lOriginalShapeFile IsNot Nothing Then
                    Dim lAllData As atcDataGroup = atcDataManager.UserSelectData("Select data to search for station information")
                    Dim lShapeFileNew As Shapefile = Nothing

                    Dim lFieldIndex As Integer
                    Dim lLastFieldIndex As Integer = lOriginalShapeFile.NumFields - 1
                    Dim lLastOriginalShape As Integer = lOriginalShapeFile.NumShapes - 1

                    'Dim lCountFieldIndex As Field = lOriginalShapeFile.FieldByName((lDataType & "_count_nu").Substring(0, 10))
                    'Dim lBeginField As Field = lOriginalShapeFile.FieldByName((lDataType & "_begin_date").Substring(0, 10))
                    'Dim lEndField As Field = lOriginalShapeFile.FieldByName((lDataType & "_end_date").Substring(0, 10))

                    Dim lCountFieldName As String = (lDataType & "_count_nu").Substring(0, 10)
                    Dim lBeginFieldName As String = (lDataType & "_begin_date").Substring(0, 10)
                    Dim lEndFieldName As String = (lDataType & "_end_date").Substring(0, 10)

                    Dim lCountFieldIndex As Integer = -1
                    Dim lBeginFieldIndex As Integer = -1
                    Dim lEndFieldIndex As Integer = -1

                    For lFieldIndex = 0 To lLastFieldIndex
                        Select Case lOriginalShapeFile.Field(lFieldIndex).Name
                            Case lCountFieldName : lCountFieldIndex = lFieldIndex
                            Case lBeginFieldName : lBeginFieldIndex = lFieldIndex
                            Case lEndFieldName : lEndFieldIndex = lFieldIndex
                        End Select
                    Next

                    Dim lNewShapefiles As New atcCollection ' (Of Shapefile)

                    If Not lSplitConstituents Then
                        lShapeFileNew = NewShapeFile(lOriginalShapeFile, lNewShapeFileName)
                        lNewShapefiles.Add(lNewShapeFileName, lShapeFileNew)
                    End If

                    Dim lConstituent As String
                    Dim lTs As atcTimeseries

                    For lOriginalShapeIndex As Integer = 0 To lLastOriginalShape
                        Dim lLocation As String = lOriginalShapeFile.CellValue(1, lOriginalShapeIndex)
                        Dim lDataSetsHere As atcDataGroup = lAllData.FindData("Location", lLocation)
                        For Each lTs In lDataSetsHere
                            Dim lCountTs As atcTimeseries = SubsetByDate(lTs, lCountStartDate, lCountEndDate, Nothing)
                            Dim lCount As Integer = lCountTs.Attributes.GetValue("Count", 0)
                            If lCount > 0 Then
                                If lSplitConstituents Then
                                    lConstituent = lTs.Attributes.GetValue("Constituent")
                                    lShapeFileNew = lNewShapefiles.ItemByKey(lConstituent)
                                    If lShapeFileNew Is Nothing Then
                                        lShapeFileNew = NewShapeFile(lOriginalShapeFile, IO.Path.ChangeExtension(lNewShapeFileName, lConstituent & ".shp"))
                                        lNewShapefiles.Add(lConstituent, lShapeFileNew)
                                    End If
                                End If
                                Dim lShapeFileNewIndex As Integer = lShapeFileNew.NumShapes
                                lShapeFileNew.EditInsertShape(lOriginalShapeFile.Shape(lOriginalShapeIndex), lShapeFileNewIndex)
                                For lFieldIndex = 0 To lLastFieldIndex
                                    Select Case lFieldIndex
                                        Case lCountFieldIndex : lShapeFileNew.EditCellValue(lCountFieldIndex, lShapeFileNewIndex, lCount)
                                        Case lBeginFieldIndex : lShapeFileNew.EditCellValue(lBeginFieldIndex, lShapeFileNewIndex, lDF.JDateToString(lCountTs.Dates.Value(1)))
                                        Case lEndFieldIndex : lShapeFileNew.EditCellValue(lEndFieldIndex, lShapeFileNewIndex, lDF.JDateToString(lCountTs.Dates.Value(lCountTs.Dates.numValues)))
                                        Case Else : lShapeFileNew.EditCellValue(lFieldIndex, lShapeFileNewIndex, lOriginalShapeFile.CellValue(lFieldIndex, lOriginalShapeIndex))
                                    End Select
                                Next
                            End If
                        Next
                    Next

                    If lOpenedOriginal Then lOriginalShapeFile.Close()
                    For Each lShapeFileNew In lNewShapefiles
                        Dim lShapeFilename As String = lShapeFileNew.Filename
                        lShapeFileNew.StopEditingShapes()
                        lShapeFileNew.StopEditingTable()
                        lShapeFileNew.Close()
                        Threading.Thread.Sleep(500) 'Give shape file plenty of time to be finished closing before opening again
                        aMapWin.Layers.Add(lShapeFilename)
                    Next
                End If
            End If
        End If

    End Sub

    Private Function NewShapeFile(ByVal aOriginalShapeFile As Shapefile, ByVal aNewFileName As String) As Shapefile
        If Not aNewFileName.Contains(IO.Path.DirectorySeparatorChar) Then
            aNewFileName = IO.Path.Combine(IO.Path.GetDirectoryName(aOriginalShapeFile.Filename), aNewFileName)
        End If
        Dim lShapeFileNew As Shapefile = New Shapefile
        lShapeFileNew.CreateNew(aNewFileName, aOriginalShapeFile.ShapefileType)
        TryCopy(IO.Path.ChangeExtension(aOriginalShapeFile.Filename, "prj"), IO.Path.ChangeExtension(aNewFileName, "prj"))
        TryCopy(IO.Path.ChangeExtension(aOriginalShapeFile.Filename, "mwsr"), IO.Path.ChangeExtension(aNewFileName, "mwsr"))
        Dim lFieldIndex As Integer
        Dim lLastFieldIndex As Integer = aOriginalShapeFile.NumFields - 1
        lShapeFileNew.StartEditingShapes()
        lShapeFileNew.StartEditingTable()
        For lFieldIndex = 0 To lLastFieldIndex
            lShapeFileNew.EditInsertField(aOriginalShapeFile.Field(lFieldIndex), lFieldIndex)
        Next
        Return lShapeFileNew
    End Function


    Private Sub CreatePointShapefile(ByVal aShapefileName As String, _
                                       ByVal aXPositions As Collection, ByVal aYPositions As Collection, _
                                       ByVal aAttributeNames As Collection, ByVal aAttributeValues As Collection, _
                                       ByVal aOutputProjection As String)
        Dim lShapefile As New MapWinGIS.Shapefile
        Dim lInputProjection As String = "+proj=longlat +datum=NAD83"

        If Not lShapefile.CreateNew(aShapefileName, MapWinGIS.ShpfileType.SHP_POINT) Then
            Logger.Dbg("Failed to create new point shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
        End If
        If Not lShapefile.StartEditingShapes(True) Then
            Logger.Dbg("Failed to start editing new point shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
        End If

        Dim lAttributeIdIndex As Integer = -1
        Dim lAttributeIndex As Integer = 0
        For Each lAttributeName As String In aAttributeNames
            lAttributeIndex += 1
            Dim lField As New MapWinGIS.Field
            lField.Name = lAttributeName
            If lAttributeName.ToLower = "id" Then
                lAttributeIdIndex = lAttributeIndex
                Logger.Dbg("Id field " & lAttributeIndex)
            End If
            lField.Type = MapWinGIS.FieldType.STRING_FIELD
            lField.Width = 10
            If Not lShapefile.EditInsertField(lField, lShapefile.NumFields) Then
                Logger.Dbg("Failed to add new field " & lField.Name & " to shapefile " & aShapefileName & " ErrorCode " & lShapefile.LastErrorCode)
            End If
            lField = Nothing
        Next

        Dim lAttributeCount As Integer = aAttributeNames.Count
        For lIndex As Integer = 0 To aXPositions.Count - 1
            Dim lShape As New MapWinGIS.Shape
            Dim lPoint As New MapWinGIS.Point
            If Not lShape.Create(MapWinGIS.ShpfileType.SHP_POINT) Then
                Logger.Dbg("Failed to create new point shape, ErrorCode " & lShape.LastErrorCode)
            End If

            If IsNumeric(aXPositions(lIndex + 1)) And IsNumeric(aYPositions(lIndex + 1)) Then
                Dim lXpos As Double = aXPositions(lIndex + 1)
                Dim lYpos As Double = aYPositions(lIndex + 1)
                If aOutputProjection.Length > 0 Then
                    MapWinGeoProc.SpatialReference.ProjectPoint(lXpos, lYpos, lInputProjection, aOutputProjection)
                End If
                lPoint.x = lXpos
                lPoint.y = lYpos
                Dim lTempIndex As Integer = lIndex '2nd argument in InsertPoint set to 0 for some reason
                If Not lShape.InsertPoint(lPoint, lTempIndex) Then
                    Logger.Dbg("Failed to insert point into shape.")
                End If
                If Not lShapefile.EditInsertShape(lShape, lShapefile.NumShapes) Then
                    Logger.Dbg("Failed to insert point shape into shapefile.")
                End If

                For lAttributeIndex = 1 To lAttributeCount
                    If Not lShapefile.EditCellValue(lAttributeIndex - 1, lShapefile.NumShapes - 1, aAttributeValues((lIndex * lAttributeCount) + lAttributeIndex)) Then
                        Logger.Dbg("Failed to edit cell value.")
                    End If
                Next
            Else
                Logger.Dbg("No Latitude (" & aYPositions(lIndex + 1) & ") or Longitude (" & aXPositions(lIndex + 1) & _
                           ") set for point with id " & aAttributeValues((lIndex * lAttributeCount) + lAttributeIdIndex))
            End If

            lPoint = Nothing
            lShape = Nothing
        Next

        If Not lShapefile.StopEditingShapes(True, True) Then
            Logger.Dbg("Failed to stop editing shapes in " & lShapefile.Filename & " ErrorCode " & lShapefile.LastErrorCode)
        End If
        If aOutputProjection.Length > 0 Then
            lShapefile.Projection = aOutputProjection
        Else
            lShapefile.Projection = lInputProjection
        End If
        If Not lShapefile.Close() Then
            Logger.Dbg("Failed to close shapefile " & lShapefile.Filename & " ErrorCode " & lShapefile.LastErrorCode)
        End If

    End Sub

End Module
