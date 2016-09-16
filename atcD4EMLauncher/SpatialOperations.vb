Imports atcUtility
Imports MapWinUtility

Public Class SpatialOperations

    Public Const GeographicProjection As String = "+proj=latlong +datum=NAD83"

    'These all mean the same thing, but we want to check more than one way of writing it so we can avoid unnecessary projection
    Public Shared GeographicProjections() As String = {"+proj=latlong +datum=NAD83", _
                                                       "+proj=longlat +ellps=sphere +lon_0=0 +lat_0=0 +h=0 +datum=NAD83"}

    'These all mean the same thing, but we want to check more than one way of writing it so we can avoid unnecessary projection
    Public Shared AlbersProjections() As String = {"+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m", _
                                                   "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs", _
                                                   "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs", _
                                                   "+proj=aea +datum=NAD83"}

    'Public Shared AlbersProjections() As String = {"+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m", _
    '                                           "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs", _
    '                                           "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs", _
    '                                           "+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs +datum=NAD83", _
    '                                           "+proj=aea +datum=NAD83"}

    'Google Mercator: "+proj=merc +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +units=m +no_defs"

    'Optional: set this object to enable SameProjection to access IsSameProjection test in the OCX
    Public Shared MapOCX As Object 'As AxMapWinGIS.AxMap '= MapWindow.Interfaces.IMapWin.GetOCX

    ''' <summary>
    ''' Add to XML metadata for a layer
    ''' </summary>
    ''' <param name="aProcessStep">contents of new "procstep" entry</param>
    ''' <param name="aLayerFilename">Full path of main layer, metadata is saved in aLayerFilename.xml</param>
    Public Shared Sub AddProcessStepToLayer(ByVal aProcessStep As String, _
                                            ByVal aLayerFilename As String)
        Dim lMetadataFilename As String = aLayerFilename & ".xml"
        Dim lMetadata As New Metadata(lMetadataFilename)
        lMetadata.AddProcessStep(aProcessStep)
        lMetadata.Save()
    End Sub

    '''' <summary>
    '''' Add to XML metadata for all layers matching any given pattern
    '''' </summary>
    '''' <param name="aProcessStep">contents of new "procstep" entry</param>
    '''' <param name="aFolder">Folder to search for matching files</param>
    '''' <param name="aPatterns">Patterns to search for, e.g. "*.shp", "*.tif"</param>
    'Public Shared Sub AddProcessStepToAllLayers(ByVal aProcessStep As String, _
    '                                            ByVal aFolder As String, _
    '                                            ByVal ParamArray aPatterns() As String)
    '    For Each lPattern As String In aPatterns
    '        Dim lMatchingFiles() As String = IO.Directory.GetFiles(aFolder, lPattern, IO.SearchOption.AllDirectories)
    '        For Each lFilename As String In lMatchingFiles
    '            AddProcessStepToLayer(aProcessStep, lFilename)
    '        Next
    '    Next
    'End Sub

    Public Shared Sub CopyProcStepsFromCachedFile(ByVal aCacheFilename As String, ByVal aLayerFilename As String)
        If IO.File.Exists(aCacheFilename) Then
            Dim lMetadata As New Metadata(aLayerFilename & ".xml")
            With lMetadata
                Dim lCacheMetadataFilename As String = aCacheFilename & ".xml"
                If IO.File.Exists(lCacheMetadataFilename) Then
                    Dim lCacheMetadata As New MapWinUtility.Metadata(lCacheMetadataFilename)
                    .AddProcessSteps(lCacheMetadata.GetProcessSteps())
                End If
                .AddProcessStep("Read from cache file created " & Format(IO.File.GetCreationTime(aCacheFilename), "yyyy-MM-dd HH:mm") & " '" & aCacheFilename & "'")
                .Save()
            End With
        End If
    End Sub

    ''' <summary>
    ''' Remove a layer from the map given its file name
    ''' </summary>
    ''' <param name="aLayerFilename">Full path of layer file to remove</param>
    ''' <returns>True if layer was removed, False if it was not on map or could not be removed</returns>
    Public Shared Function RemoveLayerFromMap(ByVal aLayerFilename As String) As Boolean
        If atcMwGisUtility.GisUtil.MappingObjectSet AndAlso FileExists(aLayerFilename) Then
            Try
                For lLayerIndex As Integer = atcMwGisUtility.GisUtil.NumLayers - 1 To 0 Step -1
                    If atcMwGisUtility.GisUtil.LayerFileName(lLayerIndex).ToLower = aLayerFilename.ToLower Then
                        atcMwGisUtility.GisUtil.RemoveLayer(lLayerIndex)
                        Return True
                    End If
                Next
            Catch 'Ignore error if layer could not be removed, probably means it was not on map or there is no map
            End Try
        End If
        Return False
    End Function

    Public Shared Function MergeLayers(ByVal aFromFolder As String, _
                                       ByVal aDestinationFolder As String, _
                              Optional ByVal aShapeKeys As Collections.Hashtable = Nothing) As String
        Dim lFromFolderLength As Integer = aFromFolder.TrimEnd(g_PathChar).Length + 1
        Dim lFilename As String
        MergeLayers = ""

        'Dim AllFilesToMove As New Collections.Specialized.NameValueCollection
        'atcUtility.AddFilesInDir(AllFilesToMove, aFromFolder, True, "*")

        Dim AllFilesToMove() As String = IO.Directory.GetFiles(aFromFolder, "*", IO.SearchOption.AllDirectories)

        For Each lFilename In AllFilesToMove
            Dim lDestinationFilename As String = IO.Path.Combine(aDestinationFolder, lFilename.Substring(lFromFolderLength))
            RemoveLayerFromMap(lDestinationFilename)
            Select Case IO.Path.GetExtension(lFilename).ToLower
                Case ".shp"
                    If Not FileExists(IO.Path.ChangeExtension(lFilename, ".shx")) Then
                        Logger.Dbg("Shape file missing shx, not merging '" & lFilename & "' into '" & lDestinationFilename & "'")
                    ElseIf Not FileExists(IO.Path.ChangeExtension(lFilename, ".dbf")) Then
                        Logger.Dbg("Shape file missing DBF, not merging '" & lFilename & "' into '" & lDestinationFilename & "'")
                    Else
                        MergeLayers &= CopyMoveMergeFile(lFilename, lDestinationFilename, aShapeKeys) & vbCrLf
                    End If
                Case ".tif"
                    MergeLayers &= CopyMoveMergeFile(lFilename, lDestinationFilename, aShapeKeys) & vbCrLf
            End Select
        Next

        Logger.Status("Moving non-layers")

        For Each lFilename In AllFilesToMove
            If FileExists(lFilename) Then 'Only files not already moved above
                Select Case IO.Path.GetExtension(lFilename).ToLower
                    Case ".shp", ".shx", ".spx", ".sbn", ".tif"
                        'These should already be gone after move/merge above, try again to delete if they are still present because they could not be deleted above
                        TryDelete(lFilename)
                    Case Else
                        Dim lDestinationFilename As String = IO.Path.Combine(aDestinationFolder, lFilename.Substring(lFromFolderLength))
                        CopyMoveMergeFile(lFilename, lDestinationFilename, aShapeKeys)
                End Select
            End If
        Next

        Logger.Status("")

    End Function

    Private Shared Function CopyMoveMergeFile(ByVal aFromFilename As String, ByVal aDestinationFilename As String, ByVal aShapeKeys As Collections.Hashtable) As String
        Dim lResult As String = ""
        If FileExists(aFromFilename) Then
            Select Case IO.Path.GetExtension(aFromFilename).ToLower
                Case ".shp"
                    Dim lKey As String = Nothing
                    If Not aShapeKeys Is Nothing Then
                        Try
                            lKey = aShapeKeys.Item(IO.Path.GetFileNameWithoutExtension(aDestinationFilename).ToLower)
                        Catch ex As Exception
                        End Try
                    End If
                    ShapeMerge(aFromFilename, aDestinationFilename, lKey)
                    lResult &= "<add_shape>" & aDestinationFilename & "</add_shape>"
                Case ".tif"
                    If Not FileExists(aDestinationFilename) Then
                        TryMoveGroup(aFromFilename, aDestinationFilename, TifExtensions)
                        lResult &= "<add_grid>" & aDestinationFilename & "</add_grid>"
                    ElseIf TryFilesMatch(aFromFilename, aDestinationFilename) Then
                        Logger.Dbg("Identical files do not need to be merged")
                        TryDeleteGroup(aFromFilename, TifExtensions)
                        lResult &= "<add_grid>" & aDestinationFilename & "</add_grid>"
                    Else
                        Logger.Status("Merging grid " & IO.Path.GetFileNameWithoutExtension(aFromFilename))
                        Logger.Dbg("Merging '" & aFromFilename & "' and existing" & vbCrLf & _
                                   "'" & aDestinationFilename & "'")

                        Dim tGrids(1) As MapWinGIS.Grid
                        Dim newGrid As MapWinGIS.Grid = Nothing
                        Dim mergedFilename As String = IO.Path.ChangeExtension(aDestinationFilename, ".merged" & IO.Path.GetExtension(aDestinationFilename))
                        Dim lMergedIndex As Integer = 1
                        While IO.File.Exists(mergedFilename)
                            lMergedIndex += 1
                            mergedFilename = IO.Path.ChangeExtension(aDestinationFilename, ".merged" & lMergedIndex & IO.Path.GetExtension(aDestinationFilename))
                        End While

                        tGrids(0) = New MapWinGIS.Grid
                        tGrids(1) = New MapWinGIS.Grid

                        If tGrids(0).Open(aFromFilename) Then
                            If tGrids(1).Open(aDestinationFilename) Then
                                Dim merger As New MapWinGIS.Utils
                                newGrid = merger.GridMerge(tGrids, mergedFilename, False, MapWinGIS.GridFileType.GeoTiff)
                                newGrid.Save()
                                tGrids(0).Close()
                                tGrids(1).Close()
                            Else
                                tGrids(0).Close()
                                lResult &= "<error>Could not open '" & aDestinationFilename & "' to merge.</error>"
                            End If
                        Else
                            lResult &= "<error>Could not open '" & aFromFilename & "' to merge.</error>"
                        End If
                        If newGrid Is Nothing OrElse Not FileExists(mergedFilename) OrElse FileLen(mergedFilename) < 10 Then
                            lResult &= "<error>Unable to merge " & IO.Path.GetFileName(aDestinationFilename) & "</error>"
MoveToNewDestination:
                            Dim lAppendNumber As Integer = 2
                            Dim lNewDestinationFilename As String = FilenameNoExt(aDestinationFilename) & "-" & lAppendNumber & ".tif"
                            While FileExists(lNewDestinationFilename)
                                lAppendNumber += 1
                                lNewDestinationFilename = FilenameNoExt(aDestinationFilename) & "-" & lAppendNumber & ".tif"
                            End While
                            Logger.Dbg("Unable to merge, adding as separate layer: " & lNewDestinationFilename)
                            TryMoveGroup(aFromFilename, lNewDestinationFilename, TifExtensions)
                            lResult &= "<add_grid>" & lNewDestinationFilename & "</add_grid>"
                        Else
                            newGrid.Close()
                            Logger.Dbg("Merge Complete")
                            If Not TryMoveGroup(mergedFilename, aDestinationFilename, TifExtensions) Then
                                Logger.Dbg("Could not move merged file '" & mergedFilename & "' to '" & aDestinationFilename & "'")
                                GoTo MoveToNewDestination
                            Else
                                'Since merged grid will have correct world file information inside, discard world file from original grid
                                TryDelete(IO.Path.ChangeExtension(aDestinationFilename, "tfw"), False)
                                TryDeleteGroup(aFromFilename, TifExtensions, False)
                                lResult &= "<add_grid>" & aDestinationFilename & "</add_grid>"
                            End If
                        End If
                    End If
                Case ".dbf"
                    If Not FileExists(aDestinationFilename) Then
                        TryMove(aFromFilename, aDestinationFilename, True)
                    ElseIf TryFilesMatch(aFromFilename, aDestinationFilename) Then
                        Logger.Dbg("Identical files do not need to be merged")
                        TryDelete(aFromFilename)
                    Else
                        Logger.Status("Merging " & IO.Path.GetFileNameWithoutExtension(aFromFilename))
                        Dim lExisting As New atcTableDBF
                        If Not lExisting.OpenFile(aDestinationFilename) Then
                            Logger.Dbg("Could not open '" & aDestinationFilename & "' for merge")
                            GoTo UnknownFileType
                        Else
                            Dim lMergeIn As New atcTableDBF
                            If Not lMergeIn.OpenFile(aFromFilename) Then
                                Logger.Dbg("Could not open '" & aDestinationFilename & "' for merge")
                            Else
                                lExisting.Merge(lMergeIn, Nothing, 1)
                                lExisting.WriteFile(aDestinationFilename)
                            End If
                            lMergeIn.Clear()
                        End If
                        lExisting.Clear()
                    End If
                Case ".prj", ".tfw"
                    If FileExists(aDestinationFilename) Then 'Already have a version of this file.
                        Logger.Dbg("Keeping existing file " & aDestinationFilename)
                    Else
                        TryMove(aFromFilename, aDestinationFilename, True)
                    End If
                Case Else
UnknownFileType:
                    If FileExists(aDestinationFilename) Then 'Already have a version of this file.
                        If Not TryDelete(aDestinationFilename) Then
                            Logger.Dbg("Could not remove existing file '" & aDestinationFilename & "' to replace with new file.")
                            Return lResult
                        End If
                    Else
                        TryMove(aFromFilename, aDestinationFilename, True)
                    End If
            End Select
        End If
        Return lResult
    End Function

    Public Shared Function SameProjection(ByVal aProj1 As String, ByVal aProj2 As String) As Boolean
        Dim lProj1 As String = aProj1.ToLower.Trim
        Dim lProj2 As String = aProj2.ToLower.Trim

        If lProj1.Equals(lProj2) Then Return True

        Dim lMatch1 As Boolean = False
        Dim lMatch2 As Boolean = False
        For Each lProjection As String In GeographicProjections
            If lProjection.ToLower.Equals(lProj1) Then lMatch1 = True
            If lProjection.ToLower.Equals(lProj2) Then lMatch2 = True
        Next

        If lMatch1 AndAlso lMatch2 Then Return True

        lMatch1 = False : lMatch2 = False

        For Each lProjection As String In AlbersProjections
            If lProjection.ToLower.Equals(lProj1) Then lMatch1 = True
            If lProjection.ToLower.Equals(lProj2) Then lMatch2 = True
        Next

        If lMatch1 AndAlso lMatch2 Then Return True

        If MapOCX IsNot Nothing Then Return MapOCX.IsSameProjection(aProj1, aProj2)

        Return False
    End Function

    'Public Shared Sub RepairProjection(ByRef aProjectionString As String)
    '    aProjectionString = aProjectionString.Trim

    '    'For Each lProjection As String In GeographicProjections
    '    '    If lProjection.ToLower.Equals(aProjectionString.ToLower) Then aProjectionString = GeographicProjections(0)
    '    'Next

    '    For Each lProjection As String In AlbersProjections
    '        If lProjection.ToLower.Equals(aProjectionString.ToLower) Then aProjectionString = AlbersProjections(0)
    '    Next
    'End Sub

    Public Shared Function ProjectShapefile(ByVal aNativeProjection As String, ByVal aDesiredProjection As String, ByVal aShapeFilename As String) As Boolean
        Dim lSuccess As Boolean = False
        If SameProjection(aNativeProjection, aDesiredProjection) Then
            Logger.Status("ProjectShapefile: already in desired projection: " & aShapeFilename)
            lSuccess = True
        Else
            Dim lShapeUtilExe As String = ShapeUtilExeFullPath()
            If FileExists(lShapeUtilExe) Then
                Logger.Status("ShapeUtil: Projecting shape " & IO.Path.GetFileNameWithoutExtension(aShapeFilename))
                Logger.Dbg(lShapeUtilExe & " """ & aShapeFilename & """ """ & aDesiredProjection & """") ' """ & aNativeProjection & """")
                'TODO: insert as last arg to ShapeUtil, without it will assume decimal degrees """ & aNativeProjection & """
                'Example test might be: If Not SameProjection(aNativeProjection, GeographicProjection)
                Dim lReturnCode As Integer = Shell(lShapeUtilExe & " """ & aShapeFilename & """ """ & aDesiredProjection & """", AppWinStyle.NormalNoFocus, True)
                lSuccess = True 'TODO: check for success somehow? Return code from Shell? Check modification time of aShapeFilename?
            Else
                Logger.Dbg("MapWinGeoProc.SpatialReference.ProjectShapefile: Projecting " & aShapeFilename)
                Dim lProjectedShapeFilename As String = GetTemporaryFileName("", ".shp")
                lSuccess = MapWinGeoProc.SpatialReference.ProjectShapefile(aNativeProjection, aDesiredProjection, aShapeFilename, lProjectedShapeFilename, New MapWinCallback)
                If lSuccess Then
                    If Not TryMoveShapefile(lProjectedShapeFilename, aShapeFilename) Then
                        Logger.Dbg("FailedToMoveShapeFile " & lProjectedShapeFilename)
                    End If
                Else
                    Logger.Dbg("Failed to project " & aShapeFilename & ": " & MapWinGeoProc.Error.GetLastErrorMsg)
                End If
        End If
        If lSuccess Then
            Logger.Dbg("Projected, writing metadata")
            'TODO: move this metadata handling into MapWinGeoProc.SpatialReference.ProjectShapefile
            Dim lMetadataFilename As String = aShapeFilename & ".xml"
            Dim lMetadata As New Metadata(lMetadataFilename)
            lMetadata.AddProcessStep("Projected from '" & aNativeProjection & "' to '" & aDesiredProjection & "'")
            Dim lShapefile As New MapWinGIS.Shapefile
            If lShapefile.Open(aShapeFilename) Then
                lShapefile.Projection = aDesiredProjection
                Dim lExtents As New Region(lShapefile.Extents.yMax, lShapefile.Extents.yMin, lShapefile.Extents.xMin, lShapefile.Extents.xMax, aDesiredProjection)
                Dim lNorth, lSouth, lEast, lWest As Double
                lExtents.GetBounds(lNorth, lSouth, lWest, lEast, GeographicProjection)
                lMetadata.SetBoundingBox(DoubleToString(lWest), DoubleToString(lEast), DoubleToString(lNorth), DoubleToString(lSouth))
                lShapefile.Close()
            End If
            lMetadata.Save()
        End If
        End If
        Return lSuccess
    End Function

    ''' <summary>
    ''' Use ShapeUtil.exe to merge shapes in aFromFilename into aDestinationFilename.
    ''' </summary>
    ''' <param name="aFromFilename">New shapes to add</param>
    ''' <param name="aDestinationFilename">Existing shape file to add to</param>
    ''' <param name="aKeyField">Ignore any new shapes in aFromFilename if an existing shape in aDestinationFilename already has a matching key in this field</param>
    ''' <remarks></remarks>
    Private Shared Sub ShapeMerge(ByVal aFromFilename As String, ByVal aDestinationFilename As String, ByVal aKeyField As String)
        If FileExists(aFromFilename) Then
            If Not FileExists(aDestinationFilename) Then
                TryMoveShapefile(aFromFilename, aDestinationFilename)
                TryDelete(IO.Path.ChangeExtension(aDestinationFilename, ".sbn"))
                TryDelete(IO.Path.ChangeExtension(aDestinationFilename, ".sbx"))
            ElseIf TryFilesMatch(aFromFilename, aDestinationFilename) Then
                Logger.Dbg("Identical files do not need to be merged")
                TryDeleteShapefile(aFromFilename)
            ElseIf Not FileExists(IO.Path.ChangeExtension(aFromFilename, ".shx")) Then
                Logger.Dbg("Shape file missing shx, not merging '" & aFromFilename & "' into '" & aDestinationFilename & "'")
            ElseIf Not FileExists(IO.Path.ChangeExtension(aFromFilename, ".dbf")) Then
                Logger.Dbg("Shape file missing DBF, not merging '" & aFromFilename & "' into '" & aDestinationFilename & "'")
            Else
                Dim lShapeUtilExe As String = ShapeUtilExeFullPath
                If FileExists(lShapeUtilExe) Then
                    Logger.Status("Merging shape " & IO.Path.GetFileNameWithoutExtension(aFromFilename))
                    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(aDestinationFilename))
                    Logger.Dbg(lShapeUtilExe & " """ & aDestinationFilename & """ key=" & aKeyField & " """ & aFromFilename & """") ' """ & aProjectionFilename & """")
                    'Shell(lShapeUtilExe & " """ & aOutputFilename & """ key=" & aKeyField & " """ & aCurFilename & """ """ & aProjectionFilename & """", AppWinStyle.NormalNoFocus, True)
                    If aKeyField Is Nothing Then
                        Shell(lShapeUtilExe & " """ & aDestinationFilename & """ """ & aFromFilename & """", AppWinStyle.NormalNoFocus, True)
                    Else
                        Shell(lShapeUtilExe & " """ & aDestinationFilename & """ key=" & aKeyField & " """ & aFromFilename & """", AppWinStyle.NormalNoFocus, True)
                    End If
                Else
                    Logger.Dbg("Failed to find ShapeUtil.exe for merging " & aFromFilename & " into " & aDestinationFilename)
                End If
            End If
        End If
    End Sub

    Private Shared Function ShapeUtilExeFullPath() As String
        Dim lShapeUtilExe As String = IO.Path.Combine(PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)), "etc" & g_PathChar & "DataDownload" & g_PathChar & "ShapeUtil.exe")
        If Not FileExists(lShapeUtilExe) Then
            lShapeUtilExe = FindFile("Please locate ShapeUtil.exe (installed in etc\DataDownload)", lShapeUtilExe)
        End If
        Return lShapeUtilExe
    End Function

    Public Shared Sub ProjectAndClipGridLayers(ByVal aFolder As String, _
                                               ByVal aNativeProjection As String, _
                                               ByVal aDesiredProjection As String, _
                                      Optional ByVal aClipRegion As Region = Nothing)
        ProjectAndClipGridLayers(aFolder, aNativeProjection, aDesiredProjection, aClipRegion, "*.tif")
    End Sub

    Public Shared Sub ProjectAndClipGridLayers(ByVal aFolder As String, _
                                               ByVal aNativeProjection As String, _
                                               ByVal aDesiredProjection As String, _
                                               ByVal aClipRegion As Region, _
                                               ByVal aFilter As String)
        'Project geotiff layers
        Dim lAllFilesToProject As New Collections.Specialized.NameValueCollection
        atcUtility.AddFilesInDir(lAllFilesToProject, aFolder, True, aFilter)
        Dim lLayerCount As Integer = lAllFilesToProject.Count
        If lLayerCount > 0 Then
            Dim lFolderLength As Integer = aFolder.TrimEnd(g_PathChar).Length + 1
            Dim lProjectToFolder As String = NewTempDir("Projecting")
            Dim lClipToFolder As String = ""
            If Not aClipRegion Is Nothing Then lClipToFolder = NewTempDir("Clipping")

            Dim lLayerProgressEnd As Integer = lLayerCount
            Dim lProgressComplete As Integer = 0
            If aClipRegion IsNot Nothing Then lLayerProgressEnd *= 2
            If lLayerProgressEnd > 1 Then Logger.Progress(0, lLayerProgressEnd)
            For Each lLayerFilename As String In lAllFilesToProject
                Logger.Status("Processing " & IO.Path.GetFileName(lLayerFilename))
                If lLayerProgressEnd > 1 Then Logger.Status("Label Middle Layer " & lProgressComplete + 1 & " of " & lLayerCount)

                Dim lUnClippedFilename As String = ""
                Dim lClipFilename As String = ""

                If aClipRegion IsNot Nothing Then
                    lClipFilename = IO.Path.Combine(lClipToFolder, IO.Path.GetFileName(lLayerFilename))
                    Using lLevel As New ProgressLevel(True)
                        Try
                            If aClipRegion.ClipGrid(lLayerFilename, lClipFilename, aNativeProjection) Then
                                lUnClippedFilename = lLayerFilename
                                lLayerFilename = lClipFilename
                            Else 'TODO: determine whether lLevel.Dispose is called when we throw an exception
                                Throw New ApplicationException("SpatialOperations.ClipGridWithPolygon:" & MapWinGeoProc.Error.GetLastErrorMsg)
                            End If
                        Catch exClip As Exception
                            Logger.Dbg("Grid could not be clipped: " & exClip.ToString)
                            lClipFilename = ""
                        End Try
                    End Using
                End If

                Dim lProjectedFilename As String = IO.Path.Combine(lProjectToFolder, lLayerFilename.Substring(lFolderLength))
                TryProjectGrid(aNativeProjection, aDesiredProjection, lLayerFilename, lProjectedFilename)

                If lUnClippedFilename.Length > 0 Then
                    TryDelete(lClipFilename)
                    lLayerFilename = lUnClippedFilename
                End If
                'Since projected grid will have correct world file information inside, discard world file from original grid
                TryDelete(IO.Path.ChangeExtension(lLayerFilename, "tfw"), False)
                TryMove(lProjectedFilename, lLayerFilename)
                TryMove(IO.Path.ChangeExtension(lProjectedFilename, "prj"), IO.Path.ChangeExtension(lLayerFilename, "prj"))
                TryMove(lProjectedFilename & ".xml", lLayerFilename & ".xml")

                lProgressComplete += 1
                If lLayerProgressEnd > 1 Then Logger.Progress(lProgressComplete, lLayerProgressEnd)
            Next
            If lClipToFolder.Length > 0 Then TryDelete(lClipToFolder)
            TryDelete(lProjectToFolder)
            Logger.Status("")
        End If
    End Sub

    Public Shared Sub TryProjectGrid(ByVal aNativeProjection As String, ByVal aDesiredProjection As String, _
                                     ByVal aLayerFilename As String, ByVal aProjectedFilename As String)
        'RepairProjection(aNativeProjection)
        'RepairProjection(aDesiredProjection)
        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(aProjectedFilename))
        If IO.Path.GetExtension(aLayerFilename).ToLower.Equals(".tif") AndAlso SameProjection(aNativeProjection, aDesiredProjection) Then
            TryCopyGroup(IO.Path.GetFileNameWithoutExtension(aLayerFilename), IO.Path.GetFileNameWithoutExtension(aProjectedFilename), TifExtensions)
        Else
            Dim lNumTries As Integer = 0
            Dim lFirstTryTime As Date = Date.Now
            Logger.Status("Projecting " & IO.Path.GetFileName(aLayerFilename))
            System.Threading.Thread.Sleep(5000)
ProjectIt:
            'Dim lProjectFromFolder As String = NewTempDir("ProjectFrom")
            'For Each lFilename As String In IO.Directory.GetFiles(IO.Path.GetDirectoryName(aLayerFilename))
            '    If Not TryCopy(lFilename, IO.Path.Combine(lProjectFromFolder, IO.Path.GetFileName(lFilename))) Then
            '        If Now.Subtract(lFirstTryTime).TotalSeconds > 120 Then
            '            Throw New ApplicationException("CopyGrid Failed")
            '        Else
            '            System.Threading.Thread.Sleep(10000)
            '            Logger.Status("Retrying grid copy " & aLayerFilename)
            '            GoTo ProjectIt
            '        End If
            '    End If
            'Next
            'aLayerFilename = IO.Path.Combine(lProjectFromFolder, IO.Path.GetFileName(aLayerFilename))

            Dim lProjectToFolder As String = NewTempDir("Projecting")
            Dim lProjectedFilename As String = IO.Path.Combine(lProjectToFolder, IO.Path.GetFileName(aProjectedFilename))
            If ProjectGrid(aNativeProjection, aDesiredProjection, aLayerFilename, lProjectedFilename) Then
                TryCopyGroup(lProjectedFilename, IO.Path.GetDirectoryName(aProjectedFilename), TifExtensions)

                Dim lProjectedProjectionFileName As String = IO.Path.ChangeExtension(aProjectedFilename, "prj")
                If Not IO.File.Exists(lProjectedProjectionFileName) OrElse IO.File.ReadAllText(IO.Path.ChangeExtension(aProjectedFilename, "prj")).Length = 0 Then
                    'Did not get a destination projection file written, work around bug by tricking it into writing projection file for dummy shape file
                    Dim lShp As New MapWinGIS.Shapefile
                    lShp.CreateNew(IO.Path.ChangeExtension(aProjectedFilename, "shp"), MapWinGIS.ShpfileType.SHP_POINT)
                    lShp.SaveAs(lShp.Filename)
                    lShp.Projection = aDesiredProjection
                    lShp.Close()
                    lShp = Nothing
                    IO.File.Delete(IO.Path.ChangeExtension(aProjectedFilename, "shp"))
                    IO.File.Delete(IO.Path.ChangeExtension(aProjectedFilename, "shx"))
                    IO.File.Delete(IO.Path.ChangeExtension(aProjectedFilename, "dbf"))
                End If

                'TODO: move this metadata handling into ProjectGrid
                Dim lMetadataFilename As String = aLayerFilename & ".xml"
                Dim lMetadata As New Metadata(lMetadataFilename)
                lMetadata.AddProcessStep("Projected from '" & aNativeProjection & "' to '" & aDesiredProjection & "'")
                'TODO: lMetadata.SetBoundingBox(.Extents.xMin, .Extents.xMax, .Extents.yMax, .Extents.yMin)
                lMetadata.Save()
            Else
                lNumTries += 1
                If lNumTries > 2 AndAlso Now.Subtract(lFirstTryTime).TotalSeconds > 240 Then
                    Throw New ApplicationException("ProjectGrid Failed, attempts=" & lNumTries)
                Else
                    Logger.Status("Waiting for grid projection, attempt " & lNumTries & " for " & IO.Path.GetFileName(aLayerFilename), 1)
                    Dim lDelaySeconds As Integer = 8 + 2 * lNumTries
                    For lDelay As Integer = 1 To lDelaySeconds
                        System.Threading.Thread.Sleep(1000)
                        Logger.Progress(lDelay, lDelaySeconds)
                    Next
                    Logger.Status("Retrying projection " & aLayerFilename, 1)
                    GoTo ProjectIt
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Project (and optionally clip) all the shape files in aFolder or its subfolders
    ''' </summary>
    ''' <param name="aShapeFilename">Folder containing shape files to process</param>
    ''' <param name="aNativeProjection">Projection of shape files before calling</param>
    ''' <param name="aDesiredProjection">New projection desired for shape files</param>
    ''' <param name="aClipRegion">Optional area to clip to before projecting</param>
    <CLSCompliant(False)> _
    Public Shared Sub ProjectAndClipShapeLayer(ByVal aShapeFilename As String, _
                                               ByVal aNativeProjection As String, _
                                               ByVal aDesiredProjection As String, _
                                      Optional ByVal aClipRegion As Region = Nothing, _
                                      Optional ByVal aClipFolder As String = "")
        Logger.Status("Project " & IO.Path.GetFileName(aShapeFilename))
        If FileLen(aShapeFilename) <= 100 Then
            Logger.Dbg("Empty shape file, deleting '" & aShapeFilename & "'")
            TryDeleteShapefile(aShapeFilename)
        ElseIf Not FileExists(IO.Path.ChangeExtension(aShapeFilename, ".shx")) Then
            Logger.Dbg("Shape file missing shx, deleting '" & aShapeFilename & "'")
            TryDeleteShapefile(aShapeFilename)
        ElseIf Not FileExists(IO.Path.ChangeExtension(aShapeFilename, ".dbf")) Then
            Logger.Dbg("Shape file missing DBF, deleting '" & aShapeFilename & "'")
            TryDeleteShapefile(aShapeFilename)
        Else
            Dim lUnClippedFilename As String = ""
            If aClipRegion IsNot Nothing Then
                Dim lClipFilename As String = IO.Path.Combine(aClipFolder, IO.Path.GetFileName(aShapeFilename))
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lClipFilename))
                Logger.Status("Clipping " & IO.Path.GetFileNameWithoutExtension(aShapeFilename), True)
                If aClipRegion.SelectShapes(aShapeFilename, lClipFilename, aNativeProjection) Then
                    lUnClippedFilename = aShapeFilename
                    aShapeFilename = lClipFilename
                Else
                    Logger.Status("Failed to clip " & aShapeFilename, True)
                End If
                If Not IO.File.Exists(aShapeFilename) Then
                    Logger.Dbg("Removing original layer that did not produce a clipped file: " & IO.Path.GetFileName(lUnClippedFilename))
                    TryDeleteShapefile(lUnClippedFilename)
                End If
            End If

            'Dim lShapeFile As New MapWinGIS.Shapefile
            If IO.File.Exists(aShapeFilename) Then
                If FileLen(aShapeFilename) < 102 Then
                    'lShapeFile.Projection = aDesiredProjection
                    Logger.Dbg("Skipped projecting empty layer " & aShapeFilename)
                ElseIf aNativeProjection.ToLower.Equals(aDesiredProjection.ToLower) Then
                    'skip projecting because already in desired projection
                Else
                    ProjectShapefile(aNativeProjection, aDesiredProjection, aShapeFilename)
                End If
                If lUnClippedFilename.Length > 0 Then 'Move clipped to replace original
                    TryDeleteShapefile(lUnClippedFilename)
                    TryMoveShapefile(aShapeFilename, lUnClippedFilename)
                End If
            Else
                Logger.Dbg("Failed to open " & aShapeFilename) '& ": " & lUtil.ErrorMsg(lShapeFile.LastErrorCode))
            End If
        End If
        Logger.Status("")
    End Sub

    ''' <summary>
    ''' Project (and optionally clip) all the shape files in aFolder or its subfolders
    ''' </summary>
    ''' <param name="aFolder">Folder containing shape files to process</param>
    ''' <param name="aNativeProjection">Projection of shape files before calling</param>
    ''' <param name="aDesiredProjection">New projection desired for shape files</param>
    ''' <param name="aClipRegion">Optional area to clip to before projecting</param>
    <CLSCompliant(False)> _
    Public Shared Sub ProjectAndClipShapeLayers(ByVal aFolder As String, _
                                                ByVal aNativeProjection As String, _
                                                ByVal aDesiredProjection As String, _
                                       Optional ByVal aClipRegion As Region = Nothing)
        Dim lFolderLength As Integer = aFolder.TrimEnd(g_PathChar).Length + 1
        Dim lAllFilesToProject As New Collections.Specialized.NameValueCollection
        Dim lClipToFolder As String = ""
        Dim lClipToLocalFolder As String = ""
        If Not aClipRegion Is Nothing Then lClipToFolder = NewTempDir("Clipping")

        atcUtility.AddFilesInDir(lAllFilesToProject, aFolder, True, "*.shp")
        Dim lLayerCount As Integer = lAllFilesToProject.Count
        Dim lProgressComplete As Integer = 0
        Logger.Progress(0, lLayerCount)
        For Each lLayerFilename As String In lAllFilesToProject
            Logger.Status("Processing " & IO.Path.GetFileName(lLayerFilename))
            Logger.Status("Label Middle Layer " & lProgressComplete + 1 & " of " & lLayerCount)
            If lClipToFolder.Length > 0 Then
                lClipToLocalFolder = IO.Path.GetDirectoryName(lLayerFilename)
                If lClipToLocalFolder.Length > lFolderLength Then
                    lClipToLocalFolder = lClipToLocalFolder.Substring(lFolderLength)
                Else
                    lClipToLocalFolder = ""
                End If
            End If
            Using lLevel As New ProgressLevel((lLayerCount > 1), (lLayerCount = 1))
                ProjectAndClipShapeLayer(lLayerFilename, aNativeProjection, aDesiredProjection, aClipRegion, IO.Path.Combine(lClipToFolder, lClipToLocalFolder))
            End Using
            lProgressComplete += 1
        Next
        If lClipToFolder.Length > 0 Then TryDelete(lClipToFolder)
    End Sub

    ''' <summary>
    ''' Project a grid using a new progress level
    ''' </summary>
    Private Shared Function ProjectGrid(ByVal aNativeProjection As String, _
                                       ByVal aDesiredProjection As String, _
                                       ByVal aLayerFilename As String, _
                                       ByVal aProjectedFilename As String, _
                              Optional ByVal aTrimResult As Boolean = False, _
                              Optional ByVal aIncrementProgressAfter As Boolean = False, _
                              Optional ByVal aProgressSameLevel As Boolean = False) As Boolean
        Using lLevel As New ProgressLevel(aIncrementProgressAfter, aProgressSameLevel)
            Try
                GetDefaultRenderer(aLayerFilename)
                Logger.Status("Projecting " & IO.Path.GetFileName(aLayerFilename))
                Logger.Dbg("MapWinGeoProc.SpatialReference.ProjectGrid: " & vbCrLf _
                         & "from " & aNativeProjection & vbCrLf & "to " & aDesiredProjection & vbCrLf _
                         & "from " & aLayerFilename & vbCrLf & "to " & aProjectedFilename & vbCrLf _
                         & "aTrimResult=" & aTrimResult)
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(aProjectedFilename))
                If Not MapWinGeoProc.SpatialReference.ProjectGrid(aNativeProjection, aDesiredProjection, _
                                             aLayerFilename, aProjectedFilename, aTrimResult) Then
                    Dim lLastErrorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg
                    Logger.Dbg("ProjectGrid Failed: " & lLastErrorMsg)
                    Return False
                End If
                Return True
            Catch e As Exception
                Logger.Dbg("ProjectGrid Exception: " & e.Message & vbCrLf & e.StackTrace)
                Return False
            End Try
        End Using
    End Function

    Public Shared Sub ProjectImage(ByVal aCurrentProjection As String, ByVal aDesiredProjection As String, _
                                   ByVal aSourceFilename As String, ByVal aDestinationFilename As String, _
                          Optional ByVal aIncrementProgressAfter As Boolean = False, _
                          Optional ByVal aProgressSameLevel As Boolean = False)
        'TODO: try the following code when we start using this method again
        'If SameProjection(aCurrentProjection, aDesiredProjection) Then
        '    TryCopyGroup(IO.Path.GetFileNameWithoutExtension(aSourceFilename), IO.Path.GetFileNameWithoutExtension(aDestinationFilename), atcUtility.TifExtensions)
        'Else
        Using lLevel As New ProgressLevel(aIncrementProgressAfter, aProgressSameLevel)
            MapWinGeoProc.SpatialReference.ProjectImage(aCurrentProjection, aDesiredProjection, aSourceFilename, aDestinationFilename, New MapWinCallback)
        End Using
    End Sub

    Private Shared RenderersPath As String = Nothing

    ''' <summary>
    ''' If the named layer does not have a .mwsr (for shape) or .mwleg (for grid) then look for the default file
    ''' and put a copy of the default renderer with the layer
    ''' </summary>
    Public Shared Function GetDefaultRenderer(ByVal aLayerFilename As String) As String
        Dim lReturnValue As String = ""

        If aLayerFilename IsNot Nothing AndAlso aLayerFilename.Length > 0 Then
            Dim lRendererExtensions As New Generic.List(Of String)
            If IO.Path.GetExtension(aLayerFilename).ToLower = ".shp" Then
                lRendererExtensions.Add(".shp.mwsymb") 'also copy new symbology file (MapWindow 4.8.6) if there is one
                lRendererExtensions.Add(".mwsr") 'copy old-style symbology
            Else
                lRendererExtensions.Add(".mwleg")
            End If

            For Each lRendererExt As String In lRendererExtensions

                Dim lRendererFilename As String = IO.Path.ChangeExtension(aLayerFilename, lRendererExt)
                If lRendererFilename.Length > 0 AndAlso Not IO.File.Exists(lRendererFilename) Then
                    Dim lRendererFilenameNoPath As String = IO.Path.GetFileName(lRendererFilename)
                    If aLayerFilename.ToLower.Contains(g_PathChar & "landuse" & g_PathChar) Then
                        lRendererFilenameNoPath = "giras" & lRendererExt
                    End If

                    If RenderersPath Is Nothing Then
                        RenderersPath = IO.Path.Combine(PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)), "etc") & g_PathChar & "renderers" & g_PathChar
                    End If
                    Dim lDefaultRendererFilename As String = FindFile("", RenderersPath & lRendererFilenameNoPath)
                    If Not FileExists(lDefaultRendererFilename) Then
                        If lRendererFilenameNoPath.Contains("_") Then 'Some layers are named huc8_xxx.shp, renderer is named _xxx & lRendererExt
                            lDefaultRendererFilename = FindFile("", RenderersPath & lRendererFilenameNoPath.Substring(lRendererFilenameNoPath.IndexOf("_")))
                        End If
                        If Not FileExists(lDefaultRendererFilename) Then 'Try match without leading or trailing numbers
                            lDefaultRendererFilename = FindFile("", RenderersPath & IO.Path.GetFileNameWithoutExtension(aLayerFilename).Trim("0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c) & lRendererExt)
                        End If
                    End If
                    If FileExists(lDefaultRendererFilename) Then
                        IO.File.Copy(lDefaultRendererFilename, lRendererFilename)
                        lReturnValue = lRendererFilename
                    End If
                End If
            Next
        End If
        Return lReturnValue
    End Function

    Private Shared Function SelectShapesWithBoxArgs(ByVal aArgs As Xml.XmlDocument, _
                                                    ByRef aSelectFromShapeFilename As String, _
                                                    ByRef aSelectFromShapeProjection As String, _
                                                    ByRef aKeyFieldName As String) As Boolean

        Dim lArg As Xml.XmlNode = aArgs.FirstChild
        aSelectFromShapeProjection = ""

        While Not lArg Is Nothing
            Dim lArgName As String = lArg.Name
            Dim lNameAttribute As Xml.XmlAttribute = lArg.Attributes.GetNamedItem("name")
            If Not lNameAttribute Is Nothing Then lArgName = lNameAttribute.Value
            Select Case lArgName.ToLower
                Case "selectfromshapefilename" : aSelectFromShapeFilename = lArg.InnerText
                Case "selectfromshapeprojection" : aSelectFromShapeProjection = lArg.InnerText
                Case "keyfield" : aKeyFieldName = lArg.InnerText
            End Select
            lArg = lArg.NextSibling
        End While

        'TODO: see if this can work as is or if it needs conversion from .prj format to proj4
        'If projection was not specified as an argument, see if it exists as a file
        'If aSelectFromShapeProjection.Length = 0 Then
        '    If FileExists(aSelectFromShapeFilename & ".prj") Then
        '        aSelectFromShapeProjection = WholeFileString(aSelectFromShapeFilename & ".prj")
        '    End If
        'End If

        If Not aSelectFromShapeFilename Is Nothing AndAlso _
           FileExists(aSelectFromShapeFilename) AndAlso _
           aSelectFromShapeProjection.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns the list of 8-digit HUCs in the given 2, 4, or 6-digit HUC
    ''' </summary>
    ''' <param name="aHUC">2, 4, or 6-digit HUC</param>
    ''' <returns>list of 8-digit HUCs as List of strings</returns>
    Public Shared Function HUC8List(ByVal aHUC As String) As Generic.List(Of String)
        Dim lHUCS As New Generic.List(Of String)
        Dim lDBFfilename As String = FindFile("Please locate the 8-digit HUC DBF", "huc250d3.dbf")
        If Not IO.File.Exists(lDBFfilename) Then
            Throw New ApplicationException("HUC DBF not found")
        Else
            Dim lDBF As New atcTableDBF
            lDBF.OpenFile(lDBFfilename)
            Dim lHUCfield As Integer = lDBF.FieldNumber("CU")
            If lHUCfield = 0 Then
                Throw New ApplicationException("CU field not found in HUC DBF '" & lDBFfilename & "'")
            Else
                For lRecord As Integer = 1 To lDBF.NumRecords
                    Dim lRecordHUC As String = lDBF.Value(lHUCfield)
                    If lRecordHUC.StartsWith(aHUC) Then
                        lHUCS.Add(lRecordHUC)
                    End If
                Next
            End If
        End If
        Return lHUCS
    End Function

    '<CLSCompliant(False)> _
    'Public Shared Function GetKeysOfShapesOverlappingBox(ByVal aArgs As Chilkat.Xml, _
    '                                                     ByRef aTop As Double, _
    '                                                     ByRef aBottom As Double, _
    '                                                     ByRef aLeft As Double, _
    '                                                     ByRef aRight As Double) As ArrayList
    '    Dim lBoxProjection As String = ""
    '    Dim lSelectFromShapeFilename As String = "" '"C:\dev\BASINS40\Data\national\huc250d3.shp"
    '    Dim lSelectFromShapeProjection As String = ""
    '    Dim lKeyFieldName As String = ""
    '    If SelectShapesWithBoxArgs(aArgs, _
    '                               aTop, _
    '                               aBottom, _
    '                               aLeft, _
    '                               aRight, _
    '                               lBoxProjection, _
    '                               lSelectFromShapeFilename, _
    '                               lSelectFromShapeProjection, _
    '                               lKeyFieldName) Then

    '        MapWinGeoProc.SpatialReference.ProjectPoint(aLeft, aTop, lBoxProjection, lSelectFromShapeProjection)
    '        MapWinGeoProc.SpatialReference.ProjectPoint(aRight, aBottom, lBoxProjection, lSelectFromShapeProjection)

    '        Return SpatialOperations.GetKeysOfOverlappingShapes(lSelectFromShapeFilename, aTop, aBottom, aLeft, aRight, lKeyFieldName)
    '    End If
    '    Return Nothing
    'End Function

    '<CLSCompliant(False)> _
    'Public Shared Function ProjectedBox(ByVal aTop As Double, _
    '                                    ByVal aBottom As Double, _
    '                                    ByVal aLeft As Double, _
    '                                    ByVal aRight As Double, _
    '                                    ByVal aBoxProjection As String, _
    '                                    ByVal aNewProjection As String) As MapWinGIS.Shape
    '    Dim lBox As New MapWinGIS.Shape
    '    Dim lPoint As MapWinGIS.Point
    '    lBox.Create(MapWinGIS.ShpfileType.SHP_POLYGON)
    '    lPoint = New MapWinGIS.Point
    '    lPoint.x = aLeft
    '    lPoint.y = aTop
    '    MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, aBoxProjection, aNewProjection)
    '    lBox.InsertPoint(lPoint, 0)

    '    lPoint = New MapWinGIS.Point
    '    lPoint.x = aRight
    '    lPoint.y = aTop
    '    MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, aBoxProjection, aNewProjection)
    '    lBox.InsertPoint(lPoint, 0)

    '    lPoint = New MapWinGIS.Point
    '    lPoint.x = aRight
    '    lPoint.y = aBottom
    '    MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, aBoxProjection, aNewProjection)
    '    lBox.InsertPoint(lPoint, 0)

    '    lPoint = New MapWinGIS.Point
    '    lPoint.x = aLeft
    '    lPoint.y = aBottom
    '    MapWinGeoProc.SpatialReference.ProjectPoint(lPoint.x, lPoint.y, aBoxProjection, aNewProjection)
    '    lBox.InsertPoint(lPoint, 0)

    '    Return lBox
    'End Function

    'Public Shared Sub ProjectBox(ByRef aTop As Double, _
    '                               ByRef aBottom As Double, _
    '                               ByRef aLeft As Double, _
    '                               ByRef aRight As Double, _
    '                               ByVal aOldProjection As String, _
    '                               ByVal aNewProjection As String)
    '    'TODO: this does not properly project the other two corners
    '    MapWinGeoProc.SpatialReference.ProjectPoint(aLeft, aTop, aOldProjection, aNewProjection)
    '    MapWinGeoProc.SpatialReference.ProjectPoint(aRight, aBottom, aOldProjection, aNewProjection)
    'End Sub

    ''' <summary>
    ''' Check for and process a new copy of concise place names from USGS topical gazetteer
    ''' </summary>
    ''' <param name="aFolder">Folder containing concise place names and created shape files</param>
    ''' <returns>instructions for adding any newly created layers</returns>
    ''' <remarks>download new place names from http://geonames.usgs.gov/domestic/download_data.htm </remarks>
    Public Shared Function CheckPlaceNames(ByVal aFolder As String, ByVal aProjection As String) As String
        Dim lInstructions As String = ""
        Dim lDownloadedFilename As String = IO.Path.Combine(aFolder, "US_CONCISE.txt")
        'Dim lPlacesFolder As String = IO.Path.Combine(aFolder, "places") & g_PathChar
        If IO.File.Exists(lDownloadedFilename) Then
            Logger.Status("Processing Places", True)
            Dim lSourceTable As New atcTableDelimited
            lSourceTable.Delimiter = vbTab
            If lSourceTable.OpenFile(lDownloadedFilename) Then
                With lSourceTable
                    Dim lNameField As Integer = .FieldNumber("Feature_Name")
                    If lNameField < 1 Then lNameField = .FieldNumber("Name")
                    Dim lClassField As Integer = .FieldNumber("Class")
                    Dim lLatitudeField As Integer = .FieldNumber("Source_lat_dec")
                    Dim lLongitudeField As Integer = .FieldNumber("Source_lon_dec")
                    Dim lNewDBFs As New atcCollection
                    Dim lNewDBF As atcTableDBF = Nothing
                    Dim lNewFieldNums As New ArrayList
                    Dim lNewFieldNames As New ArrayList
                    lNewFieldNames.Add("") 'Skip position 0
                    Dim lOldFieldNum As Integer
                    Dim lNewFieldNum As Integer
                    Dim lSkippedCivil As Integer = 0
                    Dim lSkippedOther As Integer = 0
                    For lRecord As Integer = 1 To .NumRecords
                        .CurrentRecord = lRecord
                        Dim lClass As String = .Value(lClassField)
                        Select Case lClass
                            Case "Populated Place" : lClass = "Populated"
                            Case "Civil"
                                Dim lName As String = .Value(lNameField)
                                If lName.StartsWith("State of") _
                                   OrElse lName.StartsWith("Commonwealth of ") _
                                   OrElse lName.EndsWith(" County") _
                                   OrElse lName.EndsWith(" Parish") Then
                                    lSkippedCivil += 1
                                    GoTo NextRecord
                                End If
                            Case "Cemetery", "Church", "Bend", "Trail", "School"
                                lSkippedOther += 1
                                GoTo NextRecord 'Ignore the few of these in concise file
                        End Select
                        'lNewDBF = lNewDBFs.ItemByKey(lClass)
                        lClass = "All"
                        If lNewDBF Is Nothing Then 'Create new DBF
                            If lNewDBFs.Count = 0 Then 'Create first DBF
                                lNewFieldNum = 1
                                For lOldFieldNum = 1 To .NumFields
                                    lNewFieldNames.Add(NewPlaceFieldName(.FieldName(lOldFieldNum)))
                                    If lNewFieldNames(lNewFieldNames.Count - 1).Length > 0 Then
                                        lNewFieldNums.Add(lNewFieldNum)
                                        lNewFieldNum += 1
                                    Else
                                        lNewFieldNums.Add(-1)
                                    End If
                                Next
                                Dim lOldFieldWidths() As Integer = .ComputeFieldLengths
                                lNewDBF = New atcTableDBF
                                lNewDBF.NumFields = lNewFieldNum - 1
                                For lOldFieldNum = 1 To .NumFields
                                    lNewFieldNum = lNewFieldNums(lOldFieldNum - 1)
                                    If lNewFieldNum > 0 Then
                                        lNewDBF.FieldName(lNewFieldNum) = lNewFieldNames(lOldFieldNum)
                                        If .FieldName(lOldFieldNum) = "Class" Then
                                            lNewDBF.FieldLength(lNewFieldNum) = 9 'Shortening this field
                                        Else
                                            lNewDBF.FieldLength(lNewFieldNum) = lOldFieldWidths(lOldFieldNum)
                                        End If
                                        If lNewDBF.FieldName(lNewFieldNum).EndsWith("itude") Then
                                            lNewDBF.FieldDecimalCount(lNewFieldNum) = 6
                                        End If
                                    End If
                                Next
                            Else 'Copy fields from first DBF
                                lNewDBF = lNewDBFs.ItemByIndex(0).Cousin
                            End If
                            lNewDBFs.Add(lClass, lNewDBF)
                        End If

                        lNewDBF.CurrentRecord = lNewDBF.NumRecords + 1
                        For lOldFieldNum = 1 To .NumFields
                            lNewFieldNum = lNewFieldNums(lOldFieldNum - 1)
                            If lNewFieldNum > 0 Then
                                lNewDBF.Value(lNewFieldNum) = .Value(lOldFieldNum)
                            End If
                        Next
NextRecord:
                    Next
                    Logger.Dbg("Skipped " & lSkippedCivil & " Civil, " & lSkippedOther & " others")

                    For lDBFIndex As Integer = 0 To lNewDBFs.Count - 1
                        lNewDBF = lNewDBFs.ItemByIndex(lDBFIndex)
                        If lNewDBF.NumRecords > 0 Then
                            'Dim lFilename As String = lPlacesFolder & "Place-" & lNewDBFs.Keys(lDBFIndex) & ".dbf"
                            Dim lShapeFilename As String = IO.Path.Combine(aFolder, "places.shp")
                            TryDeleteShapefile(lShapeFilename)
                            lNewDBF.WriteFile(IO.Path.ChangeExtension(lShapeFilename, ".dbf"))
                            ProjectShapefile(GeographicProjection, aProjection, lShapeFilename)
                            lInstructions &= "<add_shape>" & lShapeFilename & "</add_shape>" & vbCrLf
                        End If
                    Next
                End With
                Logger.Status("")
            End If

            'Move newly imported file so we don't try to import it again next time
            TryDelete(lDownloadedFilename & ".bak")
            TryMove(lDownloadedFilename, lDownloadedFilename & ".imported")

            If lInstructions.Length > 0 Then 'Add newly created layers to project
                lInstructions = "<success>" & lInstructions & "</success>"
            End If
        End If
        Return lInstructions
    End Function

    Private Shared Function NewPlaceFieldName(ByVal aOldFieldName As String) As String
        NewPlaceFieldName = ""
        Select Case aOldFieldName
            Case "Feature_ID" : NewPlaceFieldName = "ID"
            Case "Feature_Name" : NewPlaceFieldName = "Name"
            Case "Class" : NewPlaceFieldName = aOldFieldName
                'Case "ST_alpha" : NewPlaceFieldName = "State"
                'Case "ST_num"
                'Case "County"
                'Case "County_num" : NewPlaceFieldName = "County_num"
                'Case "Primary_lat_DMS", "Primary_lon_DMS"
            Case "Primary_lat_dec" : NewPlaceFieldName = "Latitude"
            Case "Primary_lon_dec" : NewPlaceFieldName = "Longitude"
                'Case "Source_lat_DMS", "Source_lon_DMS", "Source_lat_dec", "Source_lon_dec", "Elev(Meters)", "Map_Name"
                'Case "ID", "Name", "State", "County", "Latitude", "Longitude": NewPlaceFieldName = aOldFieldName
        End Select
    End Function

    'Public Sub UniqueLabels(ByVal aLabelsFilename As String)
    '    If IO.File.Exists(aLabelsFilename) Then

    '    End If
    'End Sub

    Private Shared Function TryFilesMatch(ByVal aFileName1 As String, ByVal aFileName2 As String) As Boolean
        Try
            Return FilesMatch(aFileName1, aFileName2)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ChangeGridFormat(ByVal aSourceFilename As String, ByVal aDestinationFilename As String) As Boolean
        MapWinGeoProc.DataManagement.ChangeGridFormat(aSourceFilename, aDestinationFilename, MapWinGIS.GridFileType.UseExtension, MapWinGIS.GridDataType.UnknownDataType, 1)
    End Function

    'Public Shared Sub SetGridProjection(ByVal aGridFilename As String, ByVal aProjection As String)
    '    Dim lGrid As New MapWinGIS.Grid

    '    If lGrid.Open(aGridFilename, MapWinGIS.GridDataType.UnknownDataType, True, MapWinGIS.GridFileType.UseExtension, Nothing) Then
    '        lGrid.AssignNewProjection(aProjection)
    '        lGrid.Close()
    '    End If

    'End Sub
End Class

Public Class MapWinCallback
    Implements MapWinGIS.ICallback

    Public Sub myError(ByVal KeyOfSender As String, ByVal ErrorMsg As String) Implements MapWinGIS.ICallback.Error
        Logger.Msg(ErrorMsg, "Error - MapWinGIS.ICallback " & KeyOfSender)
    End Sub
    Public Sub Progress(ByVal KeyOfSender As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
        Logger.Progress(Percent, 100)
        If Not String.IsNullOrEmpty(Message) Then Logger.Status("LABEL MIDDLE " & Message)
    End Sub
End Class

'Public Module FeatureSetShapefileExtensions
'    <System.Runtime.CompilerServices.Extension()> _
'    Public Function NumShapes(ByVal aShapefile As DotSpatial.Data.FeatureSet) As Integer
'        Return aShapefile.Features.Count
'    End Function

'    <System.Runtime.CompilerServices.Extension()> _
'    Public Function CellValue(ByVal aShapefile As DotSpatial.Data.FeatureSet, ByVal aFieldIndex As Integer, ByVal aShapeIndex As Integer) As Object
'        Return aShapefile.Features.Item(aShapeIndex).DataRow.Item(aFieldIndex)
'    End Function

'End Module

