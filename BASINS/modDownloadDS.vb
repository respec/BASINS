Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcData
Imports atcUtility
Imports System.Net
Imports DotSpatial.Controls
Imports DotSpatial.Data
Imports DotSpatial.Symbology
Imports DotSpatial.Projections
Imports atcMwGisUtility
Imports System.Collections.Generic

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Module modDownload

    Private Const XMLappName As String = "BASINS System Application"
#If GISProvider = "DotSpatial" Then
    Public Sub ProcessDownloadResults(ByVal aInstructions As String)
        Dim lXmlInstructions As New Xml.XmlDocument
        lXmlInstructions.LoadXml("<AllInstructions>" & aInstructions & "</AllInstructions>")
        Dim lMessage As String = ""
        Application.DoEvents()
        For Each lInstructionNode As Xml.XmlNode In lXmlInstructions.ChildNodes(0).ChildNodes
            lMessage &= ProcessDownloadResult(lInstructionNode.OuterXml) & vbCrLf
            Application.DoEvents()
        Next
        If lMessage.Length > 2 AndAlso Logger.DisplayMessageBoxes Then
            Logger.Msg(lMessage, "Data Download")
            If Not String.IsNullOrEmpty(g_Project.CurrentProjectFile) AndAlso IO.File.Exists(g_Project.CurrentProjectFile) Then
                g_Project.SaveProject(g_Project.CurrentProjectFile)
            End If
            If lMessage.Contains(" Data file") AndAlso g_AppNameShort <> "GW Toolbox" Then
                atcDataManager.UserManage()
            End If
        End If
    End Sub

    Private Function ProcessDownloadResult(ByVal aInstructions As String) As String
        Logger.Dbg("ProcessDownloadResult: " & aInstructions)
        Dim lProjectorNode As Xml.XmlNode
        Dim lOutputFileName As String
        Dim InputFileList As New NameValueCollection
        Dim lCurFilename As String
        Dim lDefaultsXML As Xml.XmlDocument = Nothing
        Dim lSuccess As Boolean
        Dim lInputProjection As String
        Dim lOutputProjection As String
        Dim lLayersAdded As New ArrayList
        Dim lDataAdded As New ArrayList
        Dim lProjectDir As String = ""

        ProcessDownloadResult = ""

#If GISProvider = "DotSpatial" Then
        Dim lProjectFileName As String = g_MapWin.SerializationManager.CurrentProjectFile
        If g_MapWin IsNot Nothing AndAlso lProjectFileName IsNot Nothing AndAlso lProjectFileName.Contains(g_PathChar) Then
            lProjectDir = IO.Path.GetDirectoryName(lProjectFileName)
            If lProjectDir.Length > 0 AndAlso Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
        End If
        If Not IO.Directory.Exists(lProjectDir) Then
            lProjectDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
#Else
        If g_MapWin IsNot Nothing AndAlso g_MapWin.Project IsNot Nothing AndAlso g_MapWin.Project.FileName IsNot Nothing AndAlso g_MapWin.Project.FileName.Contains(g_PathChar) Then
            lProjectDir = IO.Path.GetDirectoryName(g_MapWin.Project.FileName)
            If lProjectDir.Length > 0 AndAlso Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
        End If
#End If

        If Not aInstructions.StartsWith("<") Then
            If FileExists(aInstructions) Then
                aInstructions = WholeFileString(aInstructions)
            Else
                Logger.Dbg("No instructions to process")
                Exit Function
            End If
        End If

        Dim lInstructionsXML As New Xml.XmlDocument
        lInstructionsXML.LoadXml(aInstructions)
        Dim lInstructionsNode As Xml.XmlNode = lInstructionsXML.FirstChild
        If lInstructionsNode.Name.ToLower = "success" Then
#If GISProvider = "DotSpatial" Then
#Else
            g_MapWin.View.LockLegend()
#End If
            'g_MapWin.View.MapCursor = tkCursor.crsrWait
            For Each lProjectorNode In lInstructionsNode.ChildNodes
                Logger.Dbg("Processing XML: " & lProjectorNode.OuterXml)
                If lProjectorNode.Name <> "message" Then Logger.Status(ReadableFromXML(lProjectorNode.OuterXml))
                Select Case lProjectorNode.Name.ToLower
                    Case "add_data"
                        Dim lDataType As String = lProjectorNode.Attributes.GetNamedItem("type").InnerText
                        lOutputFileName = lProjectorNode.InnerText
#If GISProvider = "DotSpatial" Then
                        If Not FileExists(lOutputFileName) Then
                            lOutputFileName = IO.Path.Combine(lProjectDir, lOutputFileName)
                        End If
#End If
                        If lDataType.Length = 0 Then
                            Logger.Dbg("Could not add data from '" & lOutputFileName & "' - no type specified")
                        Else
                            Dim lNewDataSource As atcData.atcTimeseriesSource = atcData.atcDataManager.DataSourceByName(lDataType)
                            If lNewDataSource Is Nothing Then 'Try loading needed plugin for this data type
                                If atcData.atcDataManager.LoadPlugin("Timeseries::" & lDataType) Then
                                    'Try again with newly loaded plugin
                                    lNewDataSource = atcData.atcDataManager.DataSourceByName(lDataType)
                                    If lNewDataSource IsNot Nothing Then Logger.Dbg("Successfully loaded plugin " & lDataType & " to read " & lOutputFileName)
                                Else
                                    Logger.Dbg("Could not add data from '" & lOutputFileName & "' - unknown type '" & lDataType & "'")
                                End If
                            End If
                            If lNewDataSource IsNot Nothing Then
                                If atcData.atcDataManager.OpenDataSource(lNewDataSource, lOutputFileName, Nothing) Then
                                    lDataAdded.Add(lNewDataSource.Specification)
                                End If
                            End If
                        End If
                    Case "add_shape"
                        lOutputFileName = lProjectorNode.InnerText
#If GISProvider = "DotSpatial" Then
                        Dim lUSGSApplication As Boolean = True
                        If Not FileExists(lOutputFileName) Then
                            lOutputFileName = IO.Path.Combine(lProjectDir, lOutputFileName)
                        End If
#Else
                        Dim lUSGSApplication As Boolean = g_MapWin.ApplicationInfo.ApplicationName.StartsWith("USGS")
#End If
                        If lUSGSApplication Then
                            Select Case IO.Path.GetFileNameWithoutExtension(lOutputFileName).ToLowerInvariant()
                                Case "pcs3", "pcs", "bac_stat", "nawqa", "rf1", "urban", "urban_nm", "epa_reg", "ecoreg", "lulcndx", "mad", "catpt", "cntypt"
                                    TryDeleteShapefile(lOutputFileName)
                                    Continue For  'Skip trying to add this shapefile to the map
                            End Select
                        End If

                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        Dim lLayer As IMapFeatureLayer = AddShapeToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg("Failed add shape layer '" & lOutputFileName & "'")
                        Else
#If GISProvider = "DotSpatial" Then
                            lLayersAdded.Add(lLayer.DataSet.Name)
#Else
                            lLayersAdded.Add(lLayer.Name)
#End If
                        End If
                    Case "add_grid"
                        lOutputFileName = lProjectorNode.InnerText
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
#If GISProvider = "DotSpatial" Then
                        Dim lLayer As IMapRasterLayer = AddGridToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg(lOutputFileName, "Failed add grid layer")
                        Else
                            lLayersAdded.Add(lLayer.DataSet.Name)
                        End If
#Else
                        Dim lLayer As MapWindow.Interfaces.Layer = AddGridToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg(lOutputFileName, "Failed add grid layer")
                        Else
                            lLayersAdded.Add(lLayer.Name)
                        End If
#End If
                        If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                            'create .prj file as work-around for bug
                            SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                        End If
                    Case "remove_data"
                        atcDataManager.RemoveDataSource(lProjectorNode.InnerText.ToLower)
                    Case "remove_layer", "remove_shape", "remove_grid"
                        Try
#If GISProvider = "DotSpatial" Then
#Else
                            atcMwGisUtility.GisUtil.RemoveLayer(atcMwGisUtility.GisUtil.LayerIndex(lProjectorNode.InnerText))
#End If
                        Catch ex As Exception
                            Logger.Dbg("Error removing layer '" & lProjectorNode.InnerText & "': " & ex.Message)
                        End Try
                    Case "clip_grid"
#If GISProvider = "DotSpatial" Then
#Else
                        lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        lCurFilename = lProjectorNode.InnerText
                        'create extents shape to clip to, at the extents of the cat unit
                        Dim lShape As New DotSpatial.Data.Shape
                        Dim lExtentsSf As New DotSpatial.Data.PolygonShapefile
                        'lShape.Create(ShpfileType.SHP_POLYGON)
                        Dim lSf As New DotSpatial.Data.PolygonShapefile
                        lSf.Open(lProjectDir & "cat.shp")
                        Dim lPoint1 As New MapWinGIS.Point
                        lPoint1.x = lSf.Extents.xMin
                        lPoint1.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint1, 0)
                        Dim lPoint2 As New MapWinGIS.Point
                        lPoint2.x = lSf.Extents.xMax
                        lPoint2.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint2, 0)
                        Dim lPoint3 As New MapWinGIS.Point
                        lPoint3.x = lSf.Extents.xMax
                        lPoint3.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint3, 0)
                        Dim lPoint4 As New MapWinGIS.Point
                        lPoint4.x = lSf.Extents.xMin
                        lPoint4.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint4, 0)
                        Dim lPoint5 As New MapWinGIS.Point
                        lPoint5.x = lSf.Extents.xMin
                        lPoint5.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint5, 0)
                        If lOutputFileName.Contains(g_PathChar & "nlcd" & g_PathChar) Then
                            'project the extents into the albers projection for nlcd
                            MkDirPath(lProjectDir & "nlcd")
                            TryDeleteShapefile(lProjectDir & "nlcd\catextent.shp")
                            lSuccess = lExtentsSf.CreateNew(lProjectDir & "nlcd\catextent.shp", ShpfileType.SHP_POLYGON)
                            lSuccess = lExtentsSf.StartEditingShapes(True)
                            lSuccess = lExtentsSf.EditInsertShape(lShape, 0)
                            lSuccess = lExtentsSf.Close()
                            lInputProjection = WholeFileString(lProjectDir & "prj.proj")
                            lInputProjection = CleanUpUserProjString(lInputProjection)
                            lOutputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            If lInputProjection <> lOutputProjection Then
                                lSuccess = MapWinGeoProc.SpatialReference.ProjectShapefile(lInputProjection, lOutputProjection, lExtentsSf)
                                lSuccess = lExtentsSf.Open(lProjectDir & "nlcd\catextent.shp")
                                lShape = lExtentsSf.Shape(0)
                            End If
                        End If
                        'clip to cat extents
                        g_StatusBar(1).Text = "Clipping Grid..."
                        RefreshView()
                        DoEvents()
                        If Not FileExists(FilenameNoExt(lCurFilename) & ".prj") Then
                            'create .prj file as work-around for clipping bug
                            SaveFileString(FilenameNoExt(lCurFilename) & ".prj", "")
                        End If
                        Logger.Dbg("ClipGridWithPolygon: " & lCurFilename & " " & lProjectDir & "nlcd\catextent.shp" & " " & lOutputFileName & " " & "True")
                        lSuccess = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lCurFilename, lShape, lOutputFileName, True)
                        lSuccess = lExtentsSf.Close
                        g_StatusBar(1).Text = ""
#End If
                    Case "project_dir"
                        lProjectDir = lProjectorNode.InnerText
                        If Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
                    Case "message"
                        Logger.Msg(lProjectorNode.InnerText)
                    Case "select_layer"
                        Dim lLayerName As String = lProjectorNode.InnerText
                        Try
                            Dim lLayerIndex As Integer = -1
                            For Each layer As IMapFeatureLayer In g_MapWin.Map.GetLayers()
                                lLayerIndex += 1
                                If layer.DataSet.Name = lLayerName Then
                                    Exit For
                                End If
                            Next
                            'atcMwGisUtility.GisUtil.CurrentLayer = lLayerIndex
                            Logger.Dbg("Selected layer '" & lLayerName & "'")
                        Catch
                            Logger.Dbg("Failed to select layer '" & lLayerName & "'")
                        End Try
                    Case Else
                        Logger.Msg("Cannot follow directive:" & vbCr & lProjectorNode.Name, "ProcessProjectorFile")
                End Select
            Next

            'RemoveEmptyGroups()

            If lLayersAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Layer: " & lLayersAdded(0).ToString
            ElseIf lLayersAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lLayersAdded.Count & " Layers"
            End If

            If lLayersAdded.Count > 0 AndAlso lDataAdded.Count > 0 Then
                ProcessDownloadResult &= vbCrLf
            End If

            If lDataAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Data file: " & lDataAdded(0).ToString
            ElseIf lDataAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lDataAdded.Count & " Data files"
            End If

#If GISProvider = "DotSpatial" Then
#Else
            'g_MapWin.View.MapCursor = tkCursor.crsrMapDefault
            g_MapWin.View.UnlockLegend()
#End If
        Else
            ProcessDownloadResult &= lInstructionsNode.Name & ": " & lInstructionsNode.InnerXml
        End If
        Logger.Status("")
    End Function

    Public Function CleanUpUserProjString(ByVal aProjString As String) As String
        Dim lPos As Integer
        Dim lFirst As Boolean

        Do While Mid(aProjString, 1, 1) = "#"
            'eliminate comment lines at beginning
            lPos = InStr(aProjString, vbCrLf)
            aProjString = Mid(aProjString, lPos + 2)
        Loop
        lFirst = True
        Do While aProjString.Contains(vbCrLf)
            'strip out unneeded stuff
            lPos = InStr(aProjString, vbCrLf)
            If lFirst Then
                aProjString = Mid(aProjString, lPos + 2)
                lFirst = False
            Else
                aProjString = Mid(aProjString, 1, lPos - 1) & " " & Mid(aProjString, lPos + 2)
            End If
        Loop
        If InStr(aProjString, " end ") > 0 Then
            aProjString = Mid(aProjString, 1, InStr(aProjString, " end ") - 1)
        End If
        If Len(aProjString) > 0 Then
            If Mid(aProjString, 1, 9) = "+proj=dd " Then
                aProjString = "+proj=longlat " & Mid(aProjString, 10)
                aProjString = aProjString & " +datum=NAD83"
            ElseIf Mid(aProjString, 1, 15) = "+proj=merc +lon" Then
                'special exception for google mercator
                aProjString = "+proj=merc +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +units=m +no_defs"
            Else
                aProjString = aProjString & " +datum=NAD83 +units=m"
            End If
        Else
            aProjString = "<none>"
        End If
        Return aProjString
    End Function

    Private Sub CopyFeaturesWithinExtent(ByVal aOldFolder As String, ByVal aNewFolder As String)
        'copy features that are within selected indexes of selected layer to new folder
        'build collection of selected shapes
        Dim lSelectedLayer As IMapFeatureLayer = GisUtilDS.CurrentLayer
        Dim lSelectedShapeCollection As DotSpatial.Symbology.ISelection = lSelectedLayer.Selection

        Dim lResultSf As Shapefile = Nothing
        Dim lNewName As String = ""

        Dim lExtentsSf As Shapefile = New PolygonShapefile()
        Dim lExtentsFileName As String = lSelectedLayer.DataSet.Filename
        Dim lExtentsFeatureSet As IFeatureSet = PolygonShapefile.Open(lExtentsFileName)

        Dim lAllLayers As List(Of IMapFeatureLayer) = GisUtilDS.GetFeatureLayers(Nothing)
        For Each lyr As IMapFeatureLayer In lAllLayers
            If Not (lyr Is lSelectedLayer) Then
                Dim lyrFilename As String = lyr.DataSet.Filename
                If FilenameNoPath(lyrFilename) <> "wdm.shp" And FilenameNoPath(lyrFilename) <> "metpt.shp" Then
                    'want to keep met pts
                    'g_StatusBar(1).Text = "Selecting " & lyrFilename)
                    RefreshView()
                    Logger.Status("Copy Features Within Extent:" & lyrFilename)
                    If lyrFilename.Contains(aOldFolder) Then
                        'this layer is in our project folder
                        lNewName = ReplaceString(lyrFilename, aOldFolder, aNewFolder)
                        Dim lLayerType As FeatureType = lyr.DataSet.FeatureType
                        If lLayerType = FeatureType.Line OrElse lLayerType = FeatureType.Point OrElse lLayerType = FeatureType.Polygon Then
                            Dim lCurrentShpFS As IFeatureSet = Nothing
                            Dim msg As String = ""
                            Select Case lLayerType
                                Case FeatureType.Line
                                    lResultSf = New LineShapefile
                                    lCurrentShpFS = LineShapefile.Open(lExtentsFileName)
                                    msg = "SelectingLines"
                                Case FeatureType.Point
                                    lResultSf = New PointShapefile
                                    lCurrentShpFS = PointShapefile.Open(lExtentsFileName)
                                    msg = "SelectingPoints"
                                Case FeatureType.Polygon
                                    lResultSf = New PolygonShapefile
                                    lCurrentShpFS = PolygonShapefile.Open(lExtentsFileName)
                                    msg = "SelectingPolygons"
                            End Select
                            If lCurrentShpFS IsNot Nothing Then
                                If lCurrentShpFS.Features.Count < 10000 Then  'arbitrary limit to avoid out of memory error
                                    If lResultSf IsNot Nothing Then
                                        Logger.Dbg("  " & msg)
                                        Dim lResultFS As IFeatureSet = GisUtilDS.GP_Intersect(lCurrentShpFS, lExtentsFeatureSet)
                                        Logger.Dbg("  Finished" & msg)
                                        If lResultFS.Features.Count > 0 Then
                                            lResultSf.SaveAs(lNewName, True)
                                        End If
                                        lResultSf.Close()
                                    End If
                                End If
                            End If
                        ElseIf lLayerType = FeatureType.Unspecified Then
                            'grid, need to clip to extents
                            Dim lFileName As String = lyr.DataSet.Filename
                            'If Not MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lFileName, lExtentShape, lNewName, True) Then
                            '    Logger.Dbg("RemoveFeaturesBeyondExtent:ClipGridFailed:" & MapWinGeoProc.Error.GetLastErrorMsg)
                            'End If
                        End If
                    End If
                End If
            End If
        Next
        'g_StatusBar(1).Text = ""
        RefreshView()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BASINSNewMenu()
        'take appropriate actions if the user selects the 'New' menu
        'with a BASINS project open, or just a MapWindow project open

        Dim lResponse As Microsoft.VisualBasic.MsgBoxResult
        Dim lInputProjection As String = ""

        Dim lAllLayers As List(Of IMapFeatureLayer) = GisUtilDS.GetFeatureLayers(Nothing)
        Dim lNumLayers As Integer = lAllLayers.Count
        If IsBASINSProject() Then
            'if currently coming from a BASINS project,
            'ask if the user wants to subset this project
            'or create an entirely new one
            lResponse = Logger.Msg("Do you want to create a new project based on the selected feature(s) in this project?" & vbCrLf & vbCrLf &
                                   "(Answer 'No' to create a new project from scratch)", vbYesNoCancel, "New Project")
            If lResponse = MsgBoxResult.No Then
                LoadNationalProject()
            ElseIf lResponse = MsgBoxResult.Yes Then
                'are any features selected?
                If GisUtilDS.NumSelectedFeatures(GisUtilDS.CurrentLayer) > 0 Then
                    'is this a polygon shapefile?
                    If GisUtilDS.LayerType(GisUtilDS.CurrentLayer.DataSet.Name) = FeatureType.Polygon Then
                        'this is a polygon shapefile

                        'come up with a suggested name for the new project
                        Dim lDataPath As String = DefaultBasinsDataDir()
                        Dim lDefDirName As String = IO.Path.GetFileNameWithoutExtension(GisUtilDS.ProjectFileName)
                        Dim lDefaultProjectFileName As String = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
                        lDefDirName = PathNameOnly(lDefaultProjectFileName)
                        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
                        System.IO.Directory.CreateDirectory(lDefDirName)

                        'prompt user for new name
                        Dim lOldDataDir As String = PathNameOnly(GisUtilDS.ProjectFileName) & g_PathChar
                        Dim lProjectFileName As String = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
                        If lProjectFileName.Length > 0 Then
                            'Check to see if chosen data dir already contains any files
                            Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & g_PathChar
                            Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
                            Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
                            If lNumFiles + lNumDirs > 0 Then
                                Logger.Msg("The folder '" & lNewDataDir & "'" & vbCr _
                                       & "already contains " & lNumFiles & " files and " & lNumDirs & " folders." & vbCr _
                                       & "The folder must be empty before a new project can be created here.", "New Project")
                            Else
                                'got a good name for the new project
                                g_MapWin.Map.Cursor = Cursors.WaitCursor

                                'make dirs from the old project in the new one
                                Dim lTarget As String
                                Dim lDirs As String() = System.IO.Directory.GetDirectories(PathNameOnly(GisUtilDS.ProjectFileName))
                                For Each lDirName As String In lDirs
                                    'make this dir
                                    lTarget = lNewDataDir & Mid(lDirName, Len(PathNameOnly(GisUtilDS.ProjectFileName)) + 2)
                                    System.IO.Directory.CreateDirectory(lTarget)
                                Next

                                CopyFeaturesWithinExtent(lOldDataDir, lNewDataDir)

                                'copy all other files from the old project directory to the new one
                                Dim lFilenames As New NameValueCollection
                                AddFilesInDir(lFilenames, PathNameOnly(GisUtilDS.ProjectFileName), True)
                                For Each lFilename As String In lFilenames
                                    Dim lExtension As String = FileExt(lFilename)
                                    If lExtension <> "mwprj" AndAlso lExtension <> "bmp" Then
                                        lTarget = lNewDataDir & Mid(lFilename, Len(PathNameOnly(GisUtilDS.ProjectFileName)) + 2)
                                        If (lExtension = "prj") OrElse (Not FileExists(lTarget)) Then
                                            'g_StatusBar(1).Text = "Copying " & FilenameNoPath(lFilename.ToString)
                                            RefreshView()
                                            MkDirPath(PathNameOnly(lTarget))
                                            IO.File.Copy(lFilename, lTarget, True)
                                        End If
                                    End If
                                Next
                                'g_StatusBar(1).Text = ""
                                RefreshView()

                                'copy the mapwindow project file
                                IO.File.Copy(GisUtilDS.ProjectFileName, lProjectFileName)
                                'open the new mapwindow project file
                                g_MapWin.SerializationManager.OpenProject(lProjectFileName)
                                g_MapWin.Map.Refresh()
                                Try
                                    g_MapWin.SerializationManager.SaveProject(lProjectFileName)
                                Catch ex As Exception
                                    Logger.Dbg("BASINSNewMenu:Save2Failed:" & ex.InnerException.Message)
                                End Try

                                g_MapWin.Map.Cursor = Cursors.Default
                            End If
                        End If
                    Else
                        Logger.Msg("The selected map feature must be a polygon shapefile to use this option.", "Create Project Problem")
                    End If
                Else
                    Logger.Msg("One or more map features must be selected to use this option.", "Create Project Problem")
                End If
            End If

        Else
            'if not coming from a BASINS project,
            'ask if user wants to make this into a BASINS
            'project or create an entirely new one
            If Not NationalProjectIsOpen() AndAlso lNumLayers > 0 Then
                lResponse = Logger.Msg("Do you want to create a new project based on this MapWindow project?" & vbCrLf & vbCrLf &
                                       "(Answer 'No' to create a new project from scratch)", vbYesNoCancel, "New Project")
                If lResponse = MsgBoxResult.No Then
                    'create entirely new BASINS project
                    LoadNationalProject()
                ElseIf lResponse = MsgBoxResult.Yes Then
                    'create a BASINS project based on this MapWindow project
                    If GisUtilDS.ProjectFileName Is Nothing Then
                        Logger.Msg("The current MapWindow project must be saved before converting it.", "Convert MapWindow Project Problem")
                    Else
                        'set up temporary extents shapefile
                        Dim lProjectDir As String = PathNameOnly(GisUtilDS.ProjectFileName)
                        Dim lTitle As String = ""
                        MkDirPath(IO.Path.Combine(lProjectDir, "temp"))
                        Dim lNewShapeName As String = IO.Path.Combine(lProjectDir, "temp" & g_PathChar & "tempextent.shp")
                        TryDeleteShapefile(lNewShapeName)
                        'are any features selected?
                        Dim lCurrentLayer As IMapFeatureLayer = GisUtilDS.CurrentLayer
                        Dim lCurrentLayerType As FeatureType = FeatureType.Unspecified
                        If lCurrentLayer IsNot Nothing Then
                            lCurrentLayerType = lCurrentLayer.DataSet.FeatureType
                        End If
                        If GisUtilDS.NumSelectedFeatures(lCurrentLayer) > 0 Then
                            'make temp shapefile from selected features
                            GisUtilDS.SaveSelectedFeatures(lCurrentLayer.DataSet.Name, lNewShapeName, False)
                            lInputProjection = lCurrentLayer.DataSet.ProjectionString
                            lTitle = "MapWindow Selected Features"
                        ElseIf lCurrentLayer IsNot Nothing And (lCurrentLayerType = FeatureType.Point Or lCurrentLayerType = FeatureType.Line Or lCurrentLayerType = FeatureType.Polygon) Then
                            'make temp shapefile from current layer
                            Dim lBaseName As String = FilenameNoExt(lCurrentLayer.DataSet.Filename)
                            System.IO.File.Copy(lBaseName & ".shp", lNewShapeName)
                            System.IO.File.Copy(lBaseName & ".dbf", lProjectDir & "\temp\tempextent.dbf")
                            System.IO.File.Copy(lBaseName & ".shx", lProjectDir & "\temp\tempextent.shx")
                            lInputProjection = lCurrentLayer.DataSet.ProjectionString
                            lTitle = "MapWindow Current Layer"
                        Else
                            'make temp shapefile from extents of map
                            GisUtilDS.CreateShapefileOfCurrentMapExtents(lNewShapeName)
                            lTitle = "MapWindow Project Extents"
                        End If

                        'make sure projection is defined
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'dont have a projection yet, try to use the project's projection
                            Dim lProjInfo As ProjectionInfo = g_MapWin.Map.Projection
                            lInputProjection = lProjInfo.ToProj4String
                        End If
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'see if there is a prj.proj
                            If FileExists(lProjectDir & "\prj.proj") Then
                                lInputProjection = WholeFileString(lProjectDir & "\prj.proj")
                                lInputProjection = CleanUpUserProjString(lInputProjection)
                            End If
                        End If
                        If Len(lInputProjection) = 0 Or (lInputProjection Is Nothing) Then
                            'still don't have an projection for this mapwindow project, prompt for it
                            lInputProjection = atcProjector.Methods.AskUser
                        End If

                        If lInputProjection.Length > 0 Then
                            'make prj.proj if it doesn't exist
                            If Not FileExists(lProjectDir & "\prj.proj") Then
                                SaveFileString(lProjectDir & "\prj.proj", lInputProjection)
                            End If
                            'project the extents shapefile
                            Dim lExtentsFS As IFeatureSet = PolygonShapefile.Open(lNewShapeName)
                            If lExtentsFS IsNot Nothing Then
                                Dim lOutputProjection As String = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                                If lInputProjection <> lOutputProjection Then
                                    'If Not MapWinGeoProc.SpatialReference.ProjectShapefile(lInputProjection, lOutputProjection, lExtentsSf) Then
                                    'Logger.Msg("Problem projecting the extents shapefile.", "Convert MapWindow Project Problem")
                                    'End If
                                    Logger.Msg("No Re-projecting the extents shapefile for now.", "Convert MapWindow Project Problem")
                                End If
                                'lExtentsSf.Close()
                            End If

                            'remember the name of this mapwindow project to go back to later
                            pExistingMapWindowProjectName = g_MapWin.SerializationManager.CurrentProjectFile
                            'now open national project with temp shapefile on the map
                            LoadNationalProject()
                            'set symbology for this layer
                            GisUtilDS.AddLayer(lNewShapeName, "")
                            'Also make it transparent
                            'Also zoom near this layer
                            GisUtilDS.ZoomToLayer(GisUtilDS.GetLayerByFileName(lNewShapeName))
                            'select corresponding HUC on the map
                            Dim lCatlayer As IMapPolygonLayer = GisUtilDS.GetLayerByName("Cataloging Units")
                            'Dim lTempLayerIndex As Integer = GisUtilDS.LayerIndex(lTitle)
                            'Dim lPtX As Double
                            'Dim lPtY As Double
                            'For j As Integer = 1 To GisUtilDS.NumFeatures(lTempLayerIndex)
                            '    If (lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON OrElse
                            '        lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM OrElse
                            '        lExtentsSf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                            '        GisUtilDS.ShapeCentroid(lTempLayerIndex, j - 1, lPtX, lPtY)
                            '    Else
                            '        GisUtilDS.PointXY(lTempLayerIndex, j - 1, lPtX, lPtY)
                            '    End If
                            '    GisUtilDS.SetSelectedFeature(lCatLayerIndex, GisUtilDS.PointInPolygonXY(lPtX, lPtY, lCatLayerIndex))
                            'Next j
                            UpdateSelectedFeatures()

                        End If
                    End If
                End If
            Else
                'there are no layers or national project is already open
                LoadNationalProject()
            End If
        End If
    End Sub

    Public Function CreateDefaultNewProjectFileName(ByVal aDataPath As String, ByVal aDefDirName As String) As String
        Dim lSuffix As Integer = 1
        Dim lDirName As String = aDataPath & aDefDirName
        While FileExists(lDirName, True)  'Find a suffix that will make name unique
            If IO.Directory.Exists(lDirName) AndAlso
               IO.Directory.GetFiles(lDirName).Length = 0 AndAlso
               IO.Directory.GetDirectories(lDirName).Length = 0 Then
                Exit While 'Go ahead and use existing empty directory
            End If
            lSuffix += 1
            lDirName = aDataPath & aDefDirName & "-" & lSuffix
        End While
        If lSuffix > 1 Then 'Also add suffix to project file name
            Return IO.Path.Combine(lDirName, aDefDirName & "-" & lSuffix & ".dspx")
        Else
            Return IO.Path.Combine(lDirName, aDefDirName & ".dspx")
        End If
    End Function

    Public Function PromptForNewProjectFileName(ByVal aDefDirName As String, ByVal aDefaultProjectFileName As String) As String
        'prompt user for new name
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        Dim lChosenFileName As String = ""
        With lSaveDialog
            .Title = "Save new project as..."
            .CheckPathExists = False
            .FileName = aDefaultProjectFileName
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            .DefaultExt = ".mwprj"
            If .ShowDialog() <> System.Windows.Forms.DialogResult.OK Then
                TryDelete(aDefDirName, False) 'Cancelled save dialog
                Logger.Dbg("CreateNewProject:CANCELLED")
            Else
                lChosenFileName = .FileName
                Dim lNewDataDir As String = PathNameOnly(lChosenFileName) & g_PathChar
                Logger.Dbg("CreateNewProjectDir:" & lNewDataDir)
                Logger.Dbg("CreateNewProjectName:" & lChosenFileName)
                'Make sure lSaveDialog is not holding a reference to the file so we can delete the dir if needed
                lSaveDialog.FileName = g_PathChar
                lSaveDialog.Dispose()
                'If the user did not choose the default folder or a subfolder of it
                If Not lNewDataDir.ToLower.StartsWith(aDefDirName.ToLower) Then
                    'Remove speculatively created folder since they chose something else
                    Try
                        System.IO.Directory.Delete(aDefDirName, False) 'Cancelled save dialog or changed directory
                        Logger.Dbg("RemoveSpeculativeProjectDir:" & aDefDirName)
                    Catch ex As Exception
                        Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & aDefDirName & vbCr & ex.Message)
                    End Try
                End If
            End If
        End With
        Return lChosenFileName
    End Function

    Public Sub SpecifyAndCreateNewProject()
        pBuildFrm = Nothing
        Dim lProjectName As String = Nothing
        Dim lRegion As String = GetSelectedRegion()

        If lRegion.Length > 0 Then
            If pExistingMapWindowProjectName.Length = 0 Then
                'This is the normal case for building a new project,
                'Save national project as the user has zoomed it
                g_MapWin.Map.ClearSelection()
                Try
                    Dim ldsprojectname As String = g_Project.CurrentProjectFile.Replace("mwprj", "dspx")
                    g_Project.SaveProject(ldsprojectname)
                Catch ex As Exception
                    Logger.Dbg("DotSpatial has no save mwprj")
                End Try
                lProjectName = CreateNewProjectAndDownloadCoreDataInteractive(lRegion)
            Else
                'build new basins project from mapwindow project
                Dim lDataPath As String = DefaultBasinsDataDir()
                Dim lNewDataDir As String = PathNameOnly(pExistingMapWindowProjectName) & g_PathChar
                'download and project core data
                If Not IO.File.Exists(lNewDataDir & "prj.proj") Then
                    IO.File.WriteAllText(lNewDataDir & "prj.proj", g_MapWin.Map.Projection.ToProj4String)
                End If
                CreateNewProjectAndDownloadCoreData(lRegion, lDataPath, lNewDataDir, pExistingMapWindowProjectName, True)
                pExistingMapWindowProjectName = ""
            End If
        Else
            'prompt about creating a project with no data
            lProjectName = CreateNewProjectAndDownloadCoreDataInteractive(lRegion)
        End If

        If Not String.IsNullOrEmpty(lProjectName) Then
            Logger.Msg(lProjectName, "Created Project")
        End If
    End Sub

    Private Function GetSelectedRegion() As String
        Try
            Dim lNumSelected As Integer = GisUtilDS.NumSelectedFeatures(GisUtilDS.CurrentLayer)
            If lNumSelected > 0 Then
                Dim lXML As String = ""
                Dim lThemeTag As String = ""
                Dim lFieldName As String = ""
                Dim lFieldMatch As Integer = -1
                Dim lHUC8s As String = ""
                Dim lCurLayer As IMapFeatureLayer = GisUtilDS.CurrentLayer

                Select Case IO.Path.GetFileNameWithoutExtension(lCurLayer.DataSet.Filename).ToLower
                    Case "cat", "huc", "huc250d3"
                        lThemeTag = "HUC8" '"huc_cd"
                        lFieldName = "CU"
                    Case "cnty"
                        lThemeTag = "county_cd"
                        lFieldName = "FIPS"
                        lHUC8s = CountyStateToHuc("cnty")
                        Logger.Status("HIDE")
                    Case "st"
                        lThemeTag = "state_abbrev"
                        lFieldName = "ST"
                        lHUC8s = CountyStateToHuc("st")
                        Logger.Status("HIDE")
                End Select

                lFieldMatch = GisUtilDS.GetFieldIndexByName(lCurLayer, lFieldName)
                Dim lSelectedExtent As Extent = GisUtilDS.GetSelectedExtentInLayer(lCurLayer)

                If lSelectedExtent IsNot Nothing Then
                    With lSelectedExtent
                        lXML &= "  <northbc>" & .MaxY & "</northbc>" & vbCrLf
                        lXML &= "  <southbc>" & .MinY & "</southbc>" & vbCrLf
                        lXML &= "  <eastbc>" & .MaxX & "</eastbc>" & vbCrLf
                        lXML &= "  <westbc>" & .MinX & "</westbc>" & vbCrLf
                        lXML &= "  <projection>" & lCurLayer.DataSet.ProjectionString & "</projection>" & vbCrLf
                    End With
                End If

                If lFieldMatch >= 0 Then
                    Dim lValues As List(Of String) = GisUtilDS.GetTableValuesAtColumn(lCurLayer, "", lFieldMatch)
                    For Each lValue As String In lValues
                        lXML &= "  <" & lThemeTag & " status=""set by " & XMLappName & """>" & lValue & "</" & lThemeTag & ">" & vbCrLf
                    Next
                End If
                If lHUC8s.Length > 0 AndAlso Not lHUC8s.StartsWith("Error") Then
                    lXML &= lHUC8s
                End If
                If lXML.Length > 0 Then
                    Return "<region>" & vbCrLf & lXML & "</region>" & vbCrLf
                End If
            End If
        Catch e As Exception
            Logger.Dbg("Exception getting selected region: " & e.Message)
        End Try
        Return ""
    End Function

    Private Function CountyStateToHuc(ByVal aLayerName As String) As String
        If g_MapWin Is Nothing Then Return ""
        Dim lLayers As List(Of IMapFeatureLayer) = GisUtilDS.GetFeatureLayers(Nothing)
        Dim lNumLayers As Integer = lLayers.Count
        If g_MapWin.Map.Layers Is Nothing OrElse lNumLayers = 0 Then Return ""
        Dim lHUC8sf As IMapFeatureLayer = Nothing
        Dim lCntysf As IMapFeatureLayer = Nothing
        Dim lStsf As IMapFeatureLayer = Nothing
        Dim lHUC8Tag As String = "HUC8"
        Dim lHUC8Tag1 As String = "status=""set by BASINS System Application"""

        For Each lLayer As IMapFeatureLayer In lLayers
            Select Case IO.Path.GetFileNameWithoutExtension(lLayer.DataSet.Filename).ToLower
                Case "cat", "huc", "huc250d3"
                    If lLayer.DataSet.FeatureType = FeatureType.Polygon Then
                        lHUC8sf = lLayer
                    Else
                        Return "Error: missing HUC8 layer"
                    End If
                Case "cnty"
                    If aLayerName.ToLower() = "cnty" Then
                        If lLayer.DataSet.FeatureType = FeatureType.Polygon Then
                            lCntysf = lLayer
                        Else
                            Return "Error: missing county layer"
                        End If
                    End If
                Case "st"
                    If aLayerName.ToLower() = "st" Then
                        If lLayer.DataSet.FeatureType = FeatureType.Polygon Then
                            lStsf = lLayer
                        Else
                            Return "Error: missing State layer"
                        End If
                    End If
            End Select
        Next

        Dim lHUC8XML As String = ""
        If lHUC8sf IsNot Nothing Then
            Dim lFieldMatch As Integer = GisUtilDS.GetFieldIndexByName(lHUC8sf, "cu")
            If aLayerName.ToLower = "cnty" Then
                If lCntysf IsNot Nothing Then
                    If lFieldMatch >= 0 Then
                        Dim lSelectedHuc8 As List(Of IFeature) = GisUtilDS.SelectByShape(lHUC8sf, lCntysf)
                        If lSelectedHuc8 IsNot Nothing AndAlso lSelectedHuc8.Count > 0 Then
                            For Each lHuc8Feat As IFeature In lSelectedHuc8
                                lHUC8XML &= "<" & lHUC8Tag & " " & lHUC8Tag1 & ">" & lHuc8Feat.DataRow.Item(lFieldMatch).ToString() & "</" & lHUC8Tag & ">" & vbCrLf
                            Next
                        Else
                            lHUC8XML = "None"
                        End If
                    Else
                        lHUC8XML = "Error: county layer missing 'fips' code column"
                    End If
                Else
                    lHUC8XML = "Error: missing county layer."
                End If
            ElseIf aLayerName.ToLower = "st" Then
                If lStsf IsNot Nothing Then
                    If lFieldMatch >= 0 Then
                        Dim lSelectedHuc8 As List(Of IFeature) = GisUtilDS.SelectByShape(lHUC8sf, lStsf)
                        If lSelectedHuc8 IsNot Nothing AndAlso lSelectedHuc8.Count > 0 Then
                            For Each lHuc8Feat As IFeature In lSelectedHuc8
                                lHUC8XML &= "<" & lHUC8Tag & " " & lHUC8Tag1 & ">" & lHuc8Feat.DataRow.Item(lFieldMatch).ToString() & "</" & lHUC8Tag & ">" & vbCrLf
                            Next
                        Else
                            lHUC8XML = "None"
                        End If
                    Else
                        lHUC8XML = "Error: state layer missing 'st' code column"
                    End If
                Else
                    lHUC8XML = "Error: missing state layer."
                End If
            End If
        Else
            lHUC8XML = "Error: missing HUC8 layer"
        End If

        Return lHUC8XML

    End Function

    'Returns file name of new project or "" if not built
    Friend Function CreateNewProjectAndDownloadCoreDataInteractive(ByVal aRegion As String) As String
        Dim lDataPath As String
        Dim lDefaultProjectFileName As String        
        Dim lNoData As Boolean = False
        Dim lDefDirName As String = "NewProject"
        Dim lMyProjection As String
        Dim lCountyCodes As String = ""
        Dim lStateCodes As String = ""

StartOver:
        lDataPath = DefaultBasinsDataDir()

        If aRegion.Length > 0 Then
            Dim lRegionXML As New Xml.XmlDocument

            With lRegionXML
                .LoadXml(aRegion)
                For Each lChild As Xml.XmlNode In .ChildNodes(0).ChildNodes
                    If lChild.InnerText.Length > 0 Then
                        Select Case lChild.Name.ToLower
                            Case "northbc", "top"
                            Case "southbc", "bottom"
                            Case "westbc", "left"
                            Case "eastbc", "right"
                            Case "projection", "boxprojection"
                            Case "county_cd"
                                lCountyCodes &= lChild.InnerText & "_"
                            Case "state_abbrev"
                                lStateCodes &= lChild.InnerText & "_"
                            Case Else
                                If lDefDirName = "NewProject" Then
                                    lDefDirName = lChild.InnerText
                                Else
                                    lDefDirName = "Multiple"
                                End If
                        End Select
                    End If
                Next
            End With
        End If

        If lDefDirName = "NewProject" Then
            If lNoData Then
                'Already came through here, don't ask again
            Else
                lNoData = True
                If Logger.Msg("No features have been selected.  Do you wish to create a project with no data?", MsgBoxStyle.YesNo, "Data Extraction") = MsgBoxResult.No Then
                    Return ""
                End If
            End If
        End If
        If lCountyCodes.Length > 0 Then
            Dim lFirstCountyCd As String = lCountyCodes.Substring(0, lCountyCodes.IndexOf("_"))
            lDefDirName = lFirstCountyCd & "_" & lDefDirName
        ElseIf lStateCodes.Length > 0 Then
            Dim lFirstState As String = lStateCodes.Substring(0, lStateCodes.IndexOf("_"))
            lDefDirName = lFirstState & "_" & lDefDirName
        End If
        lDefaultProjectFileName = CreateDefaultNewProjectFileName(lDataPath, lDefDirName)
        lDefDirName = PathNameOnly(lDefaultProjectFileName)
        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
        System.IO.Directory.CreateDirectory(lDefDirName)

        Dim lProjectFileName As String = PromptForNewProjectFileName(lDefDirName, lDefaultProjectFileName)
        If lProjectFileName.Length = 0 Then
            Return ""
        Else
            'If the user did not choose the default folder or a subfolder of it
            Dim lNewDataDir As String = PathNameOnly(lProjectFileName) & g_PathChar
            If Not lNewDataDir.ToLower.StartsWith(lDefDirName.ToLower) Then
                'Remove speculatively created folder since they chose something else
                Try
                    System.IO.Directory.Delete(lDefDirName, False) 'Cancelled save dialog or changed directory
                    Logger.Dbg("RemoveSpeculativeProjectDir:" & lDefDirName)
                Catch ex As Exception
                    Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & lDefDirName & vbCr & ex.Message)
                End Try
            End If

            'Check to see if chosen data dir already contains any files
            If IO.Directory.Exists(lNewDataDir) Then
                Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
                Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
                If lNumFiles + lNumDirs > 0 Then
                    Dim lMessage As String = "The folder '" & lNewDataDir & "'" & vbCr & "already contains "
                    If lNumFiles > 0 Then
                        lMessage &= lNumFiles & " files"
                        If lNumDirs > 0 Then lMessage &= " and "
                    End If
                    If lNumDirs > 0 Then lMessage &= lNumDirs & " folders"
                    Logger.Msg(lMessage & "." & vbCr & "The folder must be empty before a new project can be created here.", "New Project")
                    GoTo StartOver
                End If
            End If

            lMyProjection = atcProjector.Methods.AskUser
            If lMyProjection.Length = 0 Then 'Cancelled projection specification dialog
                Try 'remove already created data directory
                    System.IO.Directory.Delete(lNewDataDir, False)
                    Logger.Dbg("RemoveProjectDir:" & lNewDataDir & " dueToProjectionCancel")
                Catch ex As Exception
                    Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & lNewDataDir & vbCr & ex.Message)
                End Try
                Return ""
            Else
                Logger.Dbg("Projection:" & lMyProjection)
                SaveFileString(lNewDataDir & "prj.proj", lMyProjection) 'Side effect: makes data directory

                If lNoData Then
                    Logger.Dbg("EmptyProjectCreated")
                    ClearLayers()
                    'g_MapWin.PreviewMap.GetPictureFromMap()
                    g_Project.SaveProject(lProjectFileName)
                Else
                    'new check to see if the core data is available before attempting to download it
                    Dim lHUC8Status As Integer = CheckCore(aRegion, lNewDataDir, lDataPath, lProjectFileName)
                    If lHUC8Status = 0 Then
                        'download and project core data
                        CreateNewProjectAndDownloadCoreData(aRegion, lDataPath, lNewDataDir, lProjectFileName)
                    ElseIf lHUC8Status = 2 Then
                        lProjectFileName = ""    'cancel
                    End If
                End If
                Return lProjectFileName
            End If
        End If
    End Function

    Private Sub CopyFromIfNeeded(ByVal aFilename As String, ByVal aFromDir As String, ByVal aToDir As String)
        If Not IO.File.Exists(aToDir & aFilename) Then
            If IO.File.Exists(aFromDir & aFilename) Then
                IO.File.Copy(aFromDir & aFilename, aToDir & aFilename)
            Else
                Logger.Dbg("Could not copy " & aFilename & " from " & aFromDir & " to " & aToDir)
            End If
        End If
    End Sub


    'Returns file name of new project or "" if not built
    Public Sub CreateNewProjectAndDownloadCoreData(ByVal aRegion As String,
                                                   ByVal aDataPath As String,
                                                   ByVal aNewDataDir As String,
                                                   ByVal aProjectFileName As String,
                                                   Optional ByVal aExistingMapWindowProject As Boolean = False,
                                                   Optional ByVal aCacheFolder As String = "")

        'the usual case, download the core data sets
        Dim lQuery As String
        Dim lProjection As String = CleanUpUserProjString(IO.File.ReadAllText(aNewDataDir & "prj.proj"))
        Dim lNationalDir As String = IO.Path.Combine(g_ProgramDir, "Data\national" & g_PathChar)
        If Not IO.Directory.Exists(lNationalDir) Then
            lNationalDir = IO.Path.Combine(aDataPath, "national" & g_PathChar)
        End If
        If IO.Directory.Exists(lNationalDir) Then
            CopyFromIfNeeded("sic.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("storetag.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("wqcriter.dbf", lNationalDir, aNewDataDir)
            CopyFromIfNeeded("wqobs_prm.dbf", lNationalDir, aNewDataDir)
        End If

        Dim lCacheFolder As String = g_CacheDir
        If aCacheFolder.Length > 0 Then
            lCacheFolder = aCacheFolder
        End If

        Dim lBasinsDataTypes As String = "<DataType>core31</DataType>"
        Select Case g_AppNameShort
            Case "SW Toolbox", "GW Toolbox", "Hydro Toolbox"
                lBasinsDataTypes &= "<DataType>nhd</DataType>"
        End Select

        lQuery = "<function name='GetBASINS'>" _
                  & "<arguments>" _
                  & lBasinsDataTypes _
                  & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
                  & "<CacheFolder>" & lCacheFolder & "</CacheFolder>" _
                  & "<DesiredProjection>" & lProjection & "</DesiredProjection>" _
                  & aRegion _
                  & "<clip>False</clip>" _
                  & "<merge>True</merge>" _
                  & "<joinattributes>true</joinattributes>" _
                  & "</arguments>" _
                  & "</function>"

        Logger.Status("Building new project")
        Using lLevel As New ProgressLevel()
            Dim lResult As String = atcD4EMLauncher.Execute(lQuery)
            'Logger.Msg(lResult, "Result of Query from DataManager")

            Dim lDisplayMessageBoxes As Boolean = Logger.DisplayMessageBoxes

            If lResult IsNot Nothing AndAlso lResult.Length > 0 AndAlso lResult.StartsWith("<success>") Then
                If Not aExistingMapWindowProject Then
                    'regular case, not coming from existing mapwindow project
                    ClearLayers()
                    Try
                        Dim ldsprojectname As String = aProjectFileName.Replace("mwprj", "dspx")
                        g_Project.SaveProject(ldsprojectname)
                    Catch ex As Exception
                        Logger.Dbg("CreateNewProjectAndDownloadCoreData:Save1Failed:" & ex.InnerException.Message)
                    End Try
                Else
                    'open existing mapwindow project again
                    g_Project.OpenProject(aProjectFileName)
                    Dim lProjectDir As String = PathNameOnly(aProjectFileName)
                    Dim lNewShapeName As String = lProjectDir & "\temp\tempextent.shp"
                    TryDeleteShapefile(lNewShapeName)
                End If
                'g_Project.Modified = True

                Logger.DisplayMessageBoxes = False
                ProcessDownloadResults(lResult)
                Logger.DisplayMessageBoxes = lDisplayMessageBoxes

                AddAllShapesInDir(aNewDataDir, aNewDataDir)

                Try 'If we retrieved the more detailed NHD, hide the rougher RF1
                    If lBasinsDataTypes.ToLowerInvariant().Contains(">nhd<") Then
                        Dim lnhdlayer As IMapFeatureLayer = GisUtilDS.GetLayerByName("Reach File, V1")
                        If lnhdlayer IsNot Nothing Then
                            lnhdlayer.IsVisible = False
                        End If
                    End If
                Catch 'Don't worry if we could not do this, it was not critical.
                End Try

                'g_MapWin.PreviewMap.Update(MapWindow.Interfaces.ePreviewUpdateExtents.CurrentMapView)
                If Not aExistingMapWindowProject Then
                    'regular case, not coming from existing mapwindow project
                    'set mapwindow project projection to projection of first layer
                    'g_Project.ProjectProjection = lProjection

                    'Dim lKey As String = g_MapWin.Plugins.GetPluginKey("Tiled Map")
                    'If Not String.IsNullOrEmpty(lKey) Then g_MapWin.Plugins.StopPlugin(lKey)

                    Try
                        Dim lDsprojfilename As String = aProjectFileName.Replace("mwprj", "dspx")
                        g_Project.SaveProject(lDsprojfilename)
                    Catch ex As Exception
                        Logger.Dbg("CreateNewProjectAndDownloadCoreData:Save2Failed:" & ex.InnerException.Message)
                    End Try
                End If

                'Get additional layers as desired by variants
                lQuery = ""
                Select Case g_AppNameShort
                    Case "SW Toolbox"
                        lQuery = "<function name='GetNWISStations'><arguments>" _
                           & "<DataType>discharge</DataType>" _
                           & "<MinCount>10</MinCount>" _
                           & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
                           & "<CacheFolder>" & lCacheFolder & "</CacheFolder>" _
                           & "<DesiredProjection>" & lProjection & "</DesiredProjection>" _
                           & aRegion _
                           & "<clip>False</clip> <merge>False</merge>" _
                           & "</arguments></function>"
                    Case "GW Toolbox", "Hydro Toolbox"
                        'Dim lRegion As D4EMDataManager.Region 'Use the view extents to include some stations in a buffer outside the original region
                        'With g_MapWin.View.Extents
                        '    lRegion = New D4EMDataManager.Region(.yMax, .yMin, .xMin, .xMax, g_MapWin.Project.ProjectProjection)
                        'End With
                        lQuery = "<function name='GetNWISStations'><arguments>" _
                           & "<DataType>gw_daily</DataType>" _
                           & "<DataType>gw_periodic</DataType>" _
                           & "<DataType>discharge</DataType>" _
                           & "<MinCount>10</MinCount>" _
                           & "<SaveIn>" & aNewDataDir & "</SaveIn>" _
                           & "<CacheFolder>" & lCacheFolder & "</CacheFolder>" _
                           & "<DesiredProjection>" & lProjection & "</DesiredProjection>" _
                           & aRegion _
                           & "<clip>False</clip> <merge>False</merge>" _
                           & "</arguments></function>"
                End Select
                If lQuery.Length > 0 Then
                    lResult = atcD4EMLauncher.Execute(lQuery)
                    If Not lResult Is Nothing AndAlso lResult.Length > 0 AndAlso lResult.StartsWith("<success>") Then
                        Logger.DisplayMessageBoxes = False
                        ProcessDownloadResults(lResult)
                        Logger.DisplayMessageBoxes = lDisplayMessageBoxes
                        'Save again after successfully adding more layers
                        Dim lProjectSaved As Boolean
                        Dim lErrMsg As String = ""
                        Try
                            Dim lDsprojfilename As String = aProjectFileName.Replace("mwprj", "dspx")
                            g_Project.SaveProject(lDsprojfilename)
                            lProjectSaved = True
                        Catch ex As Exception
                            lProjectSaved = False
                            lErrMsg = ex.InnerException.Message
                        End Try
                        If Not aExistingMapWindowProject AndAlso Not lProjectSaved Then
                            Logger.Dbg("CreateNewProjectAndDownloadCoreData:Save3Failed:" & lErrMsg)
                        End If
                    End If
                End If
            End If
        End Using

        Logger.Status("")
    End Sub

#Else
    'Download new data for an existing project
    Public Sub DownloadNewData(ByRef aProjectDir As String)
        Dim lDownloadFilename As String = aProjectDir & "download.xml"
        SaveFileString(lDownloadFilename, "<clsWebDataManager>" & vbCrLf & _
                                          " <status_variables>" & vbCrLf & _
                                          "  <launched_by>" & XMLappName & "</launched_by>" & vbCrLf & _
                                          "  <project_dir status=""set by " & XMLappName & """>" & aProjectDir & "</project_dir>" & vbCrLf & _
                                          " </status_variables>" & vbCrLf & _
                                          "</clsWebDataManager>")

        Dim lProjectorFilename As String = PathNameOnly(aProjectDir.Substring(0, aProjectDir.Length - 1)) & "\ATCProjector.xml"
        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If

        If DataDownload(lDownloadFilename) Then
            ProcessDownloadResults(lProjectorFilename)
        End If

        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If
    End Sub

    Public Sub ProcessDownloadResults(ByVal aInstructions As String)
        Dim lXmlInstructions As New Xml.XmlDocument
        lXmlInstructions.LoadXml("<AllInstructions>" & aInstructions & "</AllInstructions>")
        Dim lMessage As String = ""
        Application.DoEvents()
        For Each lInstructionNode As Xml.XmlNode In lXmlInstructions.ChildNodes(0).ChildNodes
            lMessage &= ProcessDownloadResult(lInstructionNode.OuterXml) & vbCrLf
            Application.DoEvents()
        Next
        If lMessage.Length > 2 AndAlso Logger.DisplayMessageBoxes Then
            Logger.Msg(lMessage, "Data Download")
            If lMessage.Contains(" Data file") AndAlso g_AppNameShort <> "GW Toolbox" Then
                atcDataManager.UserManage()
            End If
        End If
    End Sub

    Private Function ProcessDownloadResult(ByVal aInstructions As String) As String
        Logger.Dbg("ProcessDownloadResult: " & aInstructions)
        Dim lProjectorNode As Xml.XmlNode
        Dim lOutputFileName As String
        Dim InputFileList As New NameValueCollection
        Dim lCurFilename As String
        Dim lDefaultsXML As Xml.XmlDocument = Nothing
        Dim lSuccess As Boolean
        Dim lInputProjection As String
        Dim lOutputProjection As String
        Dim lLayersAdded As New ArrayList
        Dim lDataAdded As New ArrayList
        Dim lProjectDir As String = ""

        ProcessDownloadResult = ""

        If g_MapWin IsNot Nothing AndAlso g_MapWin.Project IsNot Nothing AndAlso g_MapWin.Project.FileName IsNot Nothing AndAlso g_MapWin.Project.FileName.Contains(g_PathChar) Then
            lProjectDir = IO.Path.GetDirectoryName(g_MapWin.Project.FileName)
            If lProjectDir.Length > 0 AndAlso Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
        End If

        If Not aInstructions.StartsWith("<") Then
            If FileExists(aInstructions) Then
                aInstructions = WholeFileString(aInstructions)
            Else
                Logger.Dbg("No instructions to process")
                Exit Function
            End If
        End If

        Dim lInstructionsXML As New Xml.XmlDocument
        lInstructionsXML.LoadXml(aInstructions)
        Dim lInstructionsNode As Xml.XmlNode = lInstructionsXML.FirstChild
        If lInstructionsNode.Name.ToLower = "success" Then
            g_MapWin.View.LockLegend()
            g_MapWin.View.MapCursor = tkCursor.crsrWait
            For Each lProjectorNode In lInstructionsNode.ChildNodes
                Logger.Dbg("Processing XML: " & lProjectorNode.OuterXml)
                If lProjectorNode.Name <> "message" Then Logger.Status(ReadableFromXML(lProjectorNode.OuterXml))
                Select Case lProjectorNode.Name.ToLower
                    Case "add_data"
                        Dim lDataType As String = lProjectorNode.Attributes.GetNamedItem("type").InnerText
                        lOutputFileName = lProjectorNode.InnerText
                        If lDataType.Length = 0 Then
                            Logger.Dbg("Could not add data from '" & lOutputFileName & "' - no type specified")
                        Else
                            Dim lNewDataSource As atcData.atcTimeseriesSource = atcData.atcDataManager.DataSourceByName(lDataType)
                            If lNewDataSource Is Nothing Then 'Try loading needed plugin for this data type
                                If atcData.atcDataManager.LoadPlugin("Timeseries::" & lDataType) Then
                                    'Try again with newly loaded plugin
                                    lNewDataSource = atcData.atcDataManager.DataSourceByName(lDataType)
                                    If lNewDataSource IsNot Nothing Then Logger.Dbg("Successfully loaded plugin " & lDataType & " to read " & lOutputFileName)
                                Else
                                    Logger.Dbg("Could not add data from '" & lOutputFileName & "' - unknown type '" & lDataType & "'")
                                End If
                            End If
                            If lNewDataSource IsNot Nothing Then
                                If atcData.atcDataManager.OpenDataSource(lNewDataSource, lOutputFileName, Nothing) Then
                                    lDataAdded.Add(lNewDataSource.Specification)
                                End If
                            End If
                        End If
                    Case "add_shape"
                        lOutputFileName = lProjectorNode.InnerText
                        If g_MapWin.ApplicationInfo.ApplicationName.StartsWith("USGS") Then
                            Select Case IO.Path.GetFileNameWithoutExtension(lOutputFileName).ToLowerInvariant()
                                Case "pcs3", "pcs", "bac_stat", "nawqa", "rf1", "urban", "urban_nm", "epa_reg", "ecoreg", "lulcndx", "mad"
                                    TryDeleteShapefile(lOutputFileName)
                                    Continue For  'Skip trying to add this shapefile to the map
                            End Select
                        End If

                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        Dim lLayer As MapWindow.Interfaces.Layer = AddShapeToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg("Failed add shape layer '" & lOutputFileName & "'")
                        Else
                            lLayersAdded.Add(lLayer.Name)
                        End If
                    Case "add_grid"
                        lOutputFileName = lProjectorNode.InnerText
                        '    Select Case IO.Path.GetFileName(lOutputFileName).ToLower
                        '        Case "fac.tif", "fdr.tif", "cat.tif"
                        '            Logger.Dbg("Skipping adding grid to MapWindow: " & lOutputFileName)
                        '            GetDefaultRenderer(lOutputFileName)
                        '            lMessage &= IO.Path.GetFileName(lOutputFileName) & " available, not added to map" & vbCrLf
                        '        Case Else
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        Dim lLayer As MapWindow.Interfaces.Layer = AddGridToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If lLayer Is Nothing Then
                            Logger.Msg(lOutputFileName, "Failed add grid layer")
                        Else
                            lLayersAdded.Add(lLayer.Name)
                        End If
                        If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                            'create .prj file as work-around for bug
                            SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                        End If
                        '    End Select
                        'Case "add_allshapes"
                        '    lOutputFileName = lProjectorNode.InnerText
                        '    lLayersAdded.AddRange(AddAllShapesInDir(lOutputFileName, lProjectDir))
                    Case "remove_data"
                        atcDataManager.RemoveDataSource(lProjectorNode.InnerText.ToLower)
                    Case "remove_layer", "remove_shape", "remove_grid"
                        Try
                            atcMwGisUtility.GisUtil.RemoveLayer(atcMwGisUtility.GisUtil.LayerIndex(lProjectorNode.InnerText))
                        Catch ex As Exception
                            Logger.Dbg("Error removing layer '" & lProjectorNode.InnerText & "': " & ex.Message)
                        End Try
                    Case "clip_grid"
                        lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        lCurFilename = lProjectorNode.InnerText
                        'create extents shape to clip to, at the extents of the cat unit
                        Dim lShape As New MapWinGIS.Shape
                        Dim lExtentsSf As New MapWinGIS.Shapefile
                        lShape.Create(ShpfileType.SHP_POLYGON)
                        Dim lSf As New MapWinGIS.Shapefile
                        lSf.Open(lProjectDir & "cat.shp")
                        Dim lPoint1 As New MapWinGIS.Point
                        lPoint1.x = lSf.Extents.xMin
                        lPoint1.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint1, 0)
                        Dim lPoint2 As New MapWinGIS.Point
                        lPoint2.x = lSf.Extents.xMax
                        lPoint2.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint2, 0)
                        Dim lPoint3 As New MapWinGIS.Point
                        lPoint3.x = lSf.Extents.xMax
                        lPoint3.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint3, 0)
                        Dim lPoint4 As New MapWinGIS.Point
                        lPoint4.x = lSf.Extents.xMin
                        lPoint4.y = lSf.Extents.yMax
                        lSuccess = lShape.InsertPoint(lPoint4, 0)
                        Dim lPoint5 As New MapWinGIS.Point
                        lPoint5.x = lSf.Extents.xMin
                        lPoint5.y = lSf.Extents.yMin
                        lSuccess = lShape.InsertPoint(lPoint5, 0)
                        If lOutputFileName.Contains(g_PathChar & "nlcd" & g_PathChar) Then
                            'project the extents into the albers projection for nlcd
                            MkDirPath(lProjectDir & "nlcd")
                            TryDeleteShapefile(lProjectDir & "nlcd\catextent.shp")
                            lSuccess = lExtentsSf.CreateNew(lProjectDir & "nlcd\catextent.shp", ShpfileType.SHP_POLYGON)
                            lSuccess = lExtentsSf.StartEditingShapes(True)
                            lSuccess = lExtentsSf.EditInsertShape(lShape, 0)
                            lSuccess = lExtentsSf.Close()
                            lInputProjection = WholeFileString(lProjectDir & "prj.proj")
                            lInputProjection = CleanUpUserProjString(lInputProjection)
                            lOutputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            If lInputProjection <> lOutputProjection Then
                                lSuccess = MapWinGeoProc.SpatialReference.ProjectShapefile(lInputProjection, lOutputProjection, lExtentsSf)
                                lSuccess = lExtentsSf.Open(lProjectDir & "nlcd\catextent.shp")
                                lShape = lExtentsSf.Shape(0)
                            End If
                        End If
                        'clip to cat extents
                        g_StatusBar(1).Text = "Clipping Grid..."
                        RefreshView()
                        DoEvents()
                        If Not FileExists(FilenameNoExt(lCurFilename) & ".prj") Then
                            'create .prj file as work-around for clipping bug
                            SaveFileString(FilenameNoExt(lCurFilename) & ".prj", "")
                        End If
                        Logger.Dbg("ClipGridWithPolygon: " & lCurFilename & " " & lProjectDir & "nlcd\catextent.shp" & " " & lOutputFileName & " " & "True")
                        lSuccess = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lCurFilename, lShape, lOutputFileName, True)
                        lSuccess = lExtentsSf.Close
                        g_StatusBar(1).Text = ""
                    Case "project_dir"
                        lProjectDir = lProjectorNode.InnerText
                        If Not lProjectDir.EndsWith(g_PathChar) Then lProjectDir &= g_PathChar
                        'Case "convert_shape"
                        '    lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        '    lCurFilename = lProjectorNode.InnerText
                        '    ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                        '    'attempt to assign prj file
                        '    lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        '    lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        '    Dim sf As New MapWinGIS.Shapefile
                        '    sf.Open(lOutputFileName, Nothing)
                        '    sf.Projection = lOutputProjection
                        '    sf.Close()
                        '    'if adding to some specific point layers in basins we need to refresh that map layer
                        '    If Right(lOutputFileName, 8) = "pcs3.shp" Or _
                        '       Right(lOutputFileName, 8) = "gage.shp" Or _
                        '       Right(lOutputFileName, 9) = "wqobs.shp" Then
                        '        'get handle of this layer
                        '        Dim lLayerHandle As Integer = -1
                        '        For i As Integer = 0 To g_MapWin.Layers.NumLayers
                        '            Dim lLayer As Layer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(i))
                        '            If Not (lLayer Is Nothing) AndAlso lLayer.FileName = lOutputFileName Then
                        '                lLayerHandle = g_MapWin.Layers.GetHandle(i)
                        '            End If
                        '        Next
                        '        If lLayerHandle > -1 Then
                        '            Dim llayername As String = g_MapWin.Layers(lLayerHandle).Name
                        '            Dim lRGBcolor As Integer = RGB(g_MapWin.Layers(lLayerHandle).Color.R, g_MapWin.Layers(lLayerHandle).Color.G, g_MapWin.Layers(lLayerHandle).Color.B)
                        '            Dim lmarkersize As Integer = g_MapWin.Layers(lLayerHandle).LineOrPointSize
                        '            Dim ltargroup As Integer = g_MapWin.Layers(lLayerHandle).GroupHandle
                        '            Dim lnewpos As Integer = g_MapWin.Layers(lLayerHandle).GroupPosition
                        '            Dim MWlay As MapWindow.Interfaces.Layer
                        '            Dim shpFile As MapWinGIS.Shapefile
                        '            g_MapWin.Layers.Remove(lLayerHandle)
                        '            shpFile = New MapWinGIS.Shapefile
                        '            shpFile.Open(lOutputFileName)
                        '            MWlay = g_MapWin.Layers.Add(shpFile, llayername, lRGBcolor, lRGBcolor, lmarkersize)
                        '            g_MapWin.Layers.MoveLayer(MWlay.Handle, lnewpos, ltargroup)
                        '        End If
                        '    End If
                        'Case "convert_grid"
                        '    lOutputFileName = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        '    lCurFilename = lProjectorNode.InnerText
                        '    'remove output file
                        '    TryDelete(lOutputFileName)
                        '    lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        '    lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        '    If InStr(lOutputFileName, "\nlcd" & g_PathChar) > 0 Then
                        '        'exception for nlcd data, already in albers
                        '        lInputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                        '        If lOutputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m" Then
                        '            'special exception, don't bother to reproject with only this slight datum shift
                        '            lInputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                        '        End If
                        '    Else
                        '        lInputProjection = "+proj=longlat +datum=NAD83"
                        '    End If
                        '    If lInputProjection = lOutputProjection Then
                        '        System.IO.File.Copy(lCurFilename, lOutputFileName)
                        '    Else
                        '        'project it
                        '        g_StatusBar(1).Text = "Projecting Grid..."
                        '        RefreshView()
                        '        DoEvents()
                        '        lSuccess = MapWinGeoProc.SpatialReference.ProjectGrid(lInputProjection, lOutputProjection, lCurFilename, lOutputFileName, True)
                        '        g_StatusBar(1).Text = ""
                        '        If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                        '            'create .prj file as work-around for bug
                        '            SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                        '        End If
                        '        If Not lSuccess Then
                        '            Logger.Msg("Failed to project grid" & vbCrLf & MapWinGeoProc.Error.GetLastErrorMsg, "ProcessProjectorFile")
                        '            System.IO.File.Copy(lCurFilename, lOutputFileName)
                        '        End If
                        '    End If
                        'Case "convert_dir"
                        ''loop through a directory, projecting all files in it
                        'Dim lInputDirName As String = lProjectorNode.InnerText
                        'Dim lOutputDirName As String = lProjectorNode.Attributes.GetNamedItem("output").InnerText
                        'If lOutputDirName Is Nothing OrElse lOutputDirName.Length = 0 Then
                        '    lOutputDirName = lInputDirName
                        'End If
                        'If Right(lOutputDirName, 1) <> g_PathChar Then lOutputDirName &= g_PathChar

                        'InputFileList.Clear()

                        'AddFilesInDir(InputFileList, lInputDirName, False, "*.shp")

                        'lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        'lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        'Dim sf As New MapWinGIS.Shapefile

                        'For Each lFileObject As Object In InputFileList
                        '    lCurFilename = lFileObject
                        '    If (FileExt(lCurFilename) = "shp") Then
                        '        'this is a shapefile
                        '        lOutputFileName = lOutputDirName & FilenameNoPath(lCurFilename)
                        '        'change projection and merge
                        '        If (FileExists(lOutputFileName) And (InStr(1, lOutputFileName, "\landuse" & g_PathChar) > 0)) Then
                        '            'if the output file exists and it is a landuse shape, dont bother
                        '        Else
                        '            ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                        '            'attempt to assign prj file
                        '            sf.Open(lOutputFileName, Nothing)
                        '            sf.Projection = lOutputProjection
                        '            sf.Close()
                        '        End If
                        '    End If
                        'Next lFileObject
                    Case "message"
                        Logger.Msg(lProjectorNode.InnerText)
                    Case "select_layer"
                        Dim lLayerName As String = lProjectorNode.InnerText
                        Try
                            Dim lLayerIndex As Integer = atcMwGisUtility.GisUtil.LayerIndex(lLayerName)
                            atcMwGisUtility.GisUtil.CurrentLayer = lLayerIndex
                            Logger.Dbg("Selected layer '" & lLayerName & "'")
                        Catch
                            Logger.Dbg("Failed to select layer '" & lLayerName & "'")
                        End Try
                    Case Else
                        Logger.Msg("Cannot follow directive:" & vbCr & lProjectorNode.Name, "ProcessProjectorFile")
                End Select
            Next

            RemoveEmptyGroups()

            If lLayersAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Layer: " & lLayersAdded(0).ToString
            ElseIf lLayersAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lLayersAdded.Count & " Layers"
            End If

            If lLayersAdded.Count > 0 AndAlso lDataAdded.Count > 0 Then
                ProcessDownloadResult &= vbCrLf
            End If

            If lDataAdded.Count = 1 Then
                ProcessDownloadResult &= "Downloaded Data file: " & lDataAdded(0).ToString
            ElseIf lDataAdded.Count > 1 Then
                ProcessDownloadResult &= "Downloaded " & lDataAdded.Count & " Data files"
            End If

            g_MapWin.View.MapCursor = tkCursor.crsrMapDefault
            g_MapWin.View.UnlockLegend()
        Else
            ProcessDownloadResult &= lInstructionsNode.Name & ": " & lInstructionsNode.InnerXml
        End If
        Logger.Status("")
    End Function

    '''' <summary>
    '''' Merge two shapefiles, depend on ShapeUtil.exe to find key field in layers.dbf
    '''' </summary>
    '''' <param name="aCurFilename">New layer to merge in</param>
    '''' <param name="aOutputFilename">Existing layer to add to</param>
    '''' <param name="aProjectionFilename">file containing projection information</param>
    '''' <remarks></remarks>
    'Private Sub ShapeUtilMerge(ByVal aCurFilename As String, ByVal aOutputFilename As String, ByVal aProjectionFilename As String)
    '    Dim lShapeUtilExe As String = D4EMDataManager.SpatialOperations.ShapeUtilExeFullPath()
    '    If FileExists(lShapeUtilExe) Then
    '        Dim lLayersDbf As String = GetSetting("ShapeMerge", "files", "layers.dbf")
    '        If Not FileExists(lLayersDbf) Then
    '            Logger.Dbg("Did not find layers.dbf in registry " & lLayersDbf)
    '            lLayersDbf = IO.Path.Combine(PathNameOnly(lShapeUtilExe), "layers.dbf")
    '            If FileExists(lLayersDbf) Then
    '                Logger.Dbg("Saving layers.dbf location for ShapeUtil: " & lLayersDbf)
    '                SaveSetting("ShapeMerge", "files", "layers.dbf", lLayersDbf)
    '            Else
    '                Logger.Dbg("Did not find layers.dbf in same path as ShapeUtil.exe " & lLayersDbf)
    '            End If
    '            'Else
    '            '   Logger.Dbg("Found " & lLayersDbf)
    '        End If
    '        'Logger.Msg("MSG2 Merging " & aCurFilename)
    '        'Logger.Msg("MSG6 into " & aOutputFilename)
    '        Shell(lShapeUtilExe & " """ & aOutputFilename & """ """ & aCurFilename & """ """ & aProjectionFilename & """", AppWinStyle.NormalNoFocus, True)
    '    Else
    '        Logger.Dbg("Failed to find ShapeUtil.exe for merging " & aCurFilename & " into " & aOutputFilename)
    '    End If
    'End Sub

    Private Function DataDownload(ByRef aCommandLine As String) As Boolean
        Dim lProgramFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\" & g_AppNameRegistry, "Base Directory", "C:\" & g_AppNameRegistry)
        Dim lDataDownloadExe As String = FindFile("Please locate DataDownload.exe", lProgramFolder & "\etc\DataDownload\DataDownload.exe")
        If lDataDownloadExe.Length > 0 Then
            If Shell(lDataDownloadExe & " " & aCommandLine, AppWinStyle.NormalFocus, True) = 0 Then
                Return True
            Else
                Return False
            End If
        End If
        '        Dim pManager As WebDataManager.clsWebDataManager
        '        Dim curStep As String

        '        On Error GoTo ErrHand

        '        curStep = "Set pManager = New clsWebDataManager"
        '        pManager = New WebDataManager.clsWebDataManager
        '        'Set pManager.Logger = glogger

        '        curStep = "CurrentStatusFromFile '" & aCommandLine & "'"
        '        If Len(aCommandLine) > 0 Then pManager.CurrentStatusFromFile(aCommandLine)

        '        curStep = "Manager.SelectDataType"
        '        pManager.SelectDataType()
        '        While pManager.State < 1000
        '            System.Windows.Forms.Application.DoEvents()
        '            System.Threading.Thread.Sleep(50)
        '        End While

        '        Exit Sub

        'ErrHand:
        '        Logger.Msg(curStep & vbCr & Err.Description, MsgBoxStyle.OKOnly, "BASINS Data Download Main")
    End Function

#End If
    Private Function GetDefaultsXML() As Xml.XmlDocument
        Dim lDefaultsXML As Xml.XmlDocument = Nothing
        Dim lDefaultsPath As String 'full file name of defaults XML
        Dim lProgramFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\" & g_AppNameRegistry, "Base Directory", "C:\" & g_AppNameRegistry)
        lDefaultsPath = FindFile("Please Locate BasinsDefaultLayers.xml", lProgramFolder & "\etc\BasinsDefaultLayers.xml")
        If FileExists(lDefaultsPath) Then
            lDefaultsXML = New Xml.XmlDocument
            lDefaultsXML.Load(lDefaultsPath)
        End If
        Return lDefaultsXML
    End Function

    Private Function GetDefaultsFor(ByVal Filename As String,
                                    ByVal aProjectDir As String,
                                    ByRef aDefaultsXml As Xml.XmlDocument) As Xml.XmlNode
        Dim lName As String = Filename.ToLower
        If lName.StartsWith(aProjectDir.ToLower) Then
            lName = lName.Substring(aProjectDir.Length)
        End If

        If Not aDefaultsXml Is Nothing Then
            For Each lChild As Xml.XmlNode In aDefaultsXml.FirstChild.ChildNodes
                'Debug.Print("Testing " & lName & " Like " & "*" & lChild.Attributes.GetNamedItem("Filename").InnerText & "*")
                If lName Like "*" & lChild.Attributes.GetNamedItem("Filename").InnerText.ToLower & "*" Then
                    Return lChild
                End If
            Next
        End If
        Return Nothing
    End Function

    'Adds all shape files found in aPath to the current MapWindow project
    Public Function AddAllShapesInDir(ByVal aPath As String, ByVal aProjectDir As String) As atcCollection
        Dim lLayer As Integer
        Dim Filename As String
        Dim allFiles As New NameValueCollection
        Dim lDefaultsXML As Xml.XmlDocument = GetDefaultsXML()

        Logger.Dbg("AddAllShapesInDir: '" & aPath & "'")

        If Right(aPath, 1) <> g_PathChar Then aPath = aPath & g_PathChar
        AddFilesInDir(allFiles, aPath, True, "*.shp")

        AddAllShapesInDir = New atcCollection()
        For lLayer = 0 To allFiles.Count - 1
            Filename = allFiles.Item(lLayer)
            AddAllShapesInDir.Add(Filename, AddShapeToMW(Filename, GetDefaultsFor(Filename, aProjectDir, lDefaultsXML)))
        Next

        RemoveEmptyGroups()
        'g_MapWin.PreviewMap.GetPictureFromMap()

    End Function

    'Given a file name and the XML describing how to render it, add a shape layer to MapWindow
    Private Function AddShapeToMW(ByVal aFilename As String, ByRef layerXml As Xml.XmlNode) As IMapFeatureLayer
        Dim LayerName As String
        Dim Group As String = ""
        Dim Visible As Boolean
        'Dim Style As atcRenderStyle = Nothing

        Dim MWlay As IMapFeatureLayer
        Dim shpFile As Shapefile
        Dim shpFileFS As IFeatureSet = Nothing

        'Don't add layer again if we already have it
        MWlay = GisUtilDS.GetLayerByFileName(aFilename)
        If MWlay IsNot Nothing Then Return MWlay
        MWlay = Nothing

        Try
            Dim lRendererName As String = atcD4EMLauncher.SpatialOperations.GetDefaultRenderer(aFilename)

            LayerName = IO.Path.GetFileNameWithoutExtension(aFilename)
            If layerXml Is Nothing Then
                Visible = True
                If lRendererName.Length = 0 Then
                    Group = "Other"
                End If
            Else
                Group = "Other"
                Visible = False
                For Each lAttribute As Xml.XmlAttribute In layerXml.Attributes
                    Select Case lAttribute.Name.ToLower
                        Case "name" : LayerName = lAttribute.InnerText
                        Case "group" : Group = lAttribute.InnerText
                        Case "visible" : Visible = (lAttribute.InnerText.ToLower = "yes") OrElse (lAttribute.InnerText.ToLower = "true")
                    End Select
                Next
            End If

            'g_StatusBar.Item(1).Text = "Opening " & aFilename


            shpFileFS = FeatureSet.Open(aFilename)
            Select Case shpFileFS.FeatureType
                Case FeatureType.Point, FeatureType.MultiPoint
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Map.Layers.Add(shpFileFS)
                    MWlay.LegendText = LayerName
                    'Else
                    'RGBcolor = RGB(Style.MarkColor.R, Style.MarkColor.G, Style.MarkColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.MarkSize)
                    'Select Case Style.MarkStyle                        'TODO: translate Cross, X, Bitmap properly
                    '    Case "Circle" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                    '    Case "Square" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                    '    Case "Cross" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleRight
                    '    Case "X" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleLeft
                    '    Case "Diamond" : MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                    '    Case "Bitmap"
                    '        Select Case Style.MarkBitsAsHex
                    '            Case "3C 42 81 99 99 81 42 3C" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                    '            Case "00 7E 7E 7E 7E 7E 7E 00" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                    '            Case "0000 0000 0000 3FF8 3FF8 1FF0 1FF0 0FE0 0FE0 07C0 07C0 0380 0380 0100 0000 0000"
                    '                MWlay.PointType = MapWinGIS.tkPointType.ptTriangleDown
                    '            Case Else
                    '                MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                    '        End Select
                    '        'MWlay.PointType = MapWinGIS.tkPointType.ptUserDefined
                    '        'mwlay.UserPointType = 'TODO: translate bitmap into Image
                    'End Select
                    'End If

                Case FeatureType.Line
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Map.Layers.Add(shpFileFS)
                    MWlay.LegendText = LayerName
                    'Else
                    'RGBcolor = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.LineWidth)
                    'End If
                Case FeatureType.Polygon
                    'If Style Is Nothing Then
                    MWlay = g_MapWin.Map.Layers.Add(shpFileFS)
                    MWlay.LegendText = LayerName
                    'Else
                    'RGBcolor = RGB(Style.FillColor.R, Style.FillColor.G, Style.FillColor.B)
                    'RGBoutline = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    'MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBoutline, Style.LineWidth)
                    'Select Case LCase(Style.FillStyle)
                    '    Case "none"
                    '        MWlay.FillStipple = MapWinGIS.tkFillStipple.fsNone
                    '        If MWlay.Color.Equals(System.Drawing.Color.Black) Then
                    '            MWlay.Color = System.Drawing.Color.White
                    '        End If
                    '        MWlay.DrawFill = False
                    '    Case "solid" '"Solid"
                    '    Case "horizontal" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsHorizontalBars
                    '    Case "vertical" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsVerticalBars
                    '    Case "down" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownRight
                    '    Case "up" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownLeft
                    '    Case "cross"
                    '    Case "diagcross"
                    'End Select
                    'End If
            End Select
            'If Not Style Is Nothing Then
            '    Select Case Style.LineStyle.ToLower
            '        Case "solid" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsNone
            '        Case "dash" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashed
            '        Case "dot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDotted
            '        Case "dashdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
            '        Case "dashdotdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
            '            'Case "alternate" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsCustom
            '            '    MWlay.UserLineStipple = 
            '    End Select
            'End If

            If MWlay IsNot Nothing Then
                Try 'Try to get Group and Visible from renderer
                    If lRendererName.Length > 0 Then
                        'MWlay.LoadShapeLayerProps(lRendererName)
                        Dim lRendererXML As New Xml.XmlDocument
                        lRendererXML.Load(lRendererName)
                        Dim lElements As System.Xml.XmlNodeList = lRendererXML.GetElementsByTagName("SFRendering")
                        If lElements.Count > 0 AndAlso lElements(0).ChildNodes IsNot Nothing AndAlso lElements(0).ChildNodes.Count > 0 Then
                            Dim lLayerXML As Xml.XmlNode = lElements(0).ChildNodes(0)
                            If lLayerXML.Attributes("GroupName") IsNot Nothing Then
                                Group = lLayerXML.Attributes("GroupName").InnerText
                            End If
                            If lLayerXML.Attributes("Visible") IsNot Nothing Then
                                Visible = CBool(lLayerXML.Attributes("Visible").InnerText)
                            End If
                        End If
                    End If
                Catch
                End Try
                MWlay.IsVisible = Visible

                'TODO: replace hard-coded SetLandUseColors and others with full renderer from defaults
                If aFilename.ToLower.Contains(g_PathChar & "landuse" & g_PathChar) Then
                    'SetLandUseColors(MWlay, shpFile)
                ElseIf aFilename.ToLower.Contains(g_PathChar & "nhd" & g_PathChar) Then
                    If IO.Path.GetFileNameWithoutExtension(aFilename).ToUpper.Contains("NHD") Then
                        MWlay.DataSet.Name = IO.Path.GetFileNameWithoutExtension(aFilename)
                    Else
                        MWlay.DataSet.Name &= " " & IO.Path.GetFileNameWithoutExtension(aFilename)
                    End If
                    MWlay.Symbolizer.SetOutline(Drawing.Color.Navy, 1)
                    'MWlay.SaveShapeLayerProps()
                    'ElseIf aFilename.ToLower.Contains(g_PathChar & "census" & g_PathChar) Then
                    'SetCensusColors(MWlay, shpFile)
                ElseIf aFilename.ToLower.Contains(g_PathChar & "dem" & g_PathChar) Then
                    'SetDemColors(MWlay, shpFile)
                ElseIf IO.Path.GetFileName(aFilename).ToLower = "met.shp" Then
                    'SetMetIcons(MWlay, shpFile)
                ElseIf IO.Path.GetFileName(aFilename).ToLower = "cat.shp" Then
                    GisUtilDS.ZoomToLayer(MWlay)
                End If
                If Group.Length > 0 Then
                    If Not String.IsNullOrEmpty(Group) Then AddLayerToGroup(MWlay, Group)
                    'If Not GisUtilDS.IsLayer(MWlay.LegendText) Then
                    '    AddLayerToGroup(MWlay, Group)
                    'End If
                End If
                If MWlay.IsVisible Then
                    g_MapWin.Map.Refresh()
                    DoEvents()
                End If
            End If
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Shape")
        End Try
        'g_StatusBar.Item(1).Text = ""

        Return MWlay
    End Function

    Private Sub AddLayerToGroup(ByVal aLay As IMapLayer, ByVal aGroupName As String)
        Dim lExistingGroupIndex As Integer = 0
        While lExistingGroupIndex < g_MapWin.Map.Layers.Count AndAlso
              g_MapWin.Map.Layers.Item(lExistingGroupIndex).LegendText <> aGroupName
            lExistingGroupIndex += 1
        End While

        Dim lGroupHandle As Integer
        If lExistingGroupIndex < g_MapWin.Map.Layers.Count Then
            lGroupHandle = lExistingGroupIndex
        Else
            'TODO: read group order from a file rather than hard-coded array
            Dim lGroupOrder() As String = {"Data Layers",
                                           "Observed Data Stations",
                                           "Point Sources & Withdrawals",
                                           "Hydrology",
                                           "Political",
                                           "Census",
                                           "Transportation",
                                           "Soil, Land Use/Cover",
                                           "Elevation",
                                           "Other"}
            Dim lNewGroupIndex As Integer = Array.IndexOf(lGroupOrder, aGroupName)
            lExistingGroupIndex = g_MapWin.Map.Layers.Count
            If lNewGroupIndex > 0 Then
                While lExistingGroupIndex > 0 AndAlso
                      Array.IndexOf(lGroupOrder, g_MapWin.Map.Layers.Item(lExistingGroupIndex - 1).LegendText) < lNewGroupIndex
                    lExistingGroupIndex -= 1
                End While
            End If
            Dim lnewMapGroup As IMapGroup = New MapGroup()
            lnewMapGroup.LegendText = aGroupName
            g_MapWin.Map.Layers.Insert(lExistingGroupIndex, lnewMapGroup)
            lGroupHandle = lExistingGroupIndex
        End If

        If lGroupHandle >= 0 AndAlso lGroupHandle < g_MapWin.Map.Layers.Count Then
            Dim lMapGroup As IMapGroup = g_MapWin.Map.Layers.Item(lGroupHandle)
            lMapGroup.Layers.Add(aLay)
            g_MapWin.Map.Layers.Remove(aLay)
        Else
            'g_MapWin.Map.AddLayer(aLay.DataSet.Filename)
        End If

        'Select Case aLay.DataSet.FeatureType
        '    Case MapWindow.Interfaces.eLayerType.Grid, MapWindow.Interfaces.eLayerType.Image, MapWindow.Interfaces.eLayerType.PolygonShapefile
        '        aLay.MoveTo(0, lGroupHandle) 'move grid/image/polygon to bottom of group
        '    Case MapWindow.Interfaces.eLayerType.LineShapefile, MapWindow.Interfaces.eLayerType.PointShapefile
        '        aLay.MoveTo(99, lGroupHandle) 'move line/point layer to top of group
        '    Case Else
        '        Logger.Dbg("AddLayerToGroup: Unexpected layer type: " & aLay.LayerType & " for layer " & aLay.Name)
        '        aLay.MoveTo(0, lGroupHandle)
        'End Select
    End Sub


    ''' <summary>
    ''' Remove any groups from the legend that do not contain any layers 
    ''' for example, groups "Data Layers" and "New Group" are often created but then no layers end up in them
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RemoveEmptyGroups()
        Dim lGroupToRemove As New List(Of IMapGroup)
        For lGroupIndex As Integer = g_MapWin.Map.Layers.Count - 1 To 0 Step -1
            Dim lGroup As MapGroup = TryCast(g_MapWin.Map.Layers.Item(lGroupIndex), MapGroup)
            If lGroup IsNot Nothing Then
                Try
                    If lGroup IsNot Nothing AndAlso lGroup.Layers.Count = 0 Then
                        lGroupToRemove.Add(lGroup)
                    End If
                Catch ex As Exception
                End Try
            End If
        Next
        For Each lGroup As IMapGroup In lGroupToRemove
            Try
                g_MapWin.Map.Layers.Remove(lGroup)
            Catch ex As Exception
                Logger.Dbg("Could not remove group", ex.Message, ex.StackTrace)
            End Try
        Next
    End Sub

    Public Function CheckCore(ByVal aRegion As String, ByVal aNewDataDir As String, ByVal aDataPath As String, ByVal aProjectFileName As String) As Integer
        'new check to see if the core data is available before attempting to download it
        'Dim lBaseURL As String = "http://www3.epa.gov/ceampubl/basins/gis_data/huc/"
        'Dim lBaseURL As String = "ftp://newftp.epa.gov/exposure/BasinsData/BasinsCoreData/"
        Dim lBaseURL As String = "https://gaftp.epa.gov/Exposure/BasinsData/BasinsCoreData/"
        Dim lHUC8s As New atcCollection
        Dim lMissingHuc8s As String = ""
        'Dim lHUC8BoundaryOnly As Boolean = False
        Dim lHUC8Status As Integer = 0              '0 - ok (download or cache), 1 - use only huc8 boundary, 2- cancel
        'get huc8s in this region
        Dim lXDoc As New Xml.XmlDocument
        lXDoc.LoadXml(aRegion)
        Dim lNodeList As Xml.XmlNodeList
        lNodeList = lXDoc.SelectNodes("/region/HUC8")
        For Each lNode As Xml.XmlNode In lNodeList
            Dim lHUC8 As String = lNode.InnerText
            lHUC8s.Add(lHUC8)
            If lHUC8Status = 0 Then
                If Not CheckAddress(lBaseURL & lHUC8 & "/" & lHUC8 & "_core31.exe") Then
                    'problem, this file does not exist
                    'just build project using selected HUC8s without any core data
                    lHUC8Status = 1
                    lMissingHuc8s &= " " & lHUC8
                End If
            End If
        Next

        If lHUC8Status = 1 Then
            Dim lStr As String = Logger.MsgCustom("One (or more) of the core data sets is temporarily unavailable for download." & vbCrLf & vbCrLf &
                                                    lMissingHuc8s & vbCrLf & vbCrLf &
                                                    "Select 'Use HUC8' to build project using only the HUC8 boundary." & vbCrLf &
                                                    "Select 'Use Cache' to build project using data from the program cache." & vbCrLf &
                                                    "Select 'Cancel' to stop Build New Project and return to Main dialog box.",
                                                    "New Project", "Use HUC8", "Use Cache", "Cancel")
            If lStr = "Use HUC8" Then
                lHUC8Status = 1
            ElseIf lStr = "Use Cache" Then
                lHUC8Status = 0
            Else
                lHUC8Status = 2  'cancel
            End If
            If lHUC8Status = 1 Then
                'If Logger.Msg("One (or more) of the core data sets is temporarily unavailable for download." & vbCrLf & vbCrLf &
                '        lMissingHuc8s & vbCrLf & vbCrLf &
                '        "Do you want to build a project using only the HUC8 boundary?" &
                '         vbCr & vbCr & "(Cancel will attempt to build using data from the program cache.)",
                '        MsgBoxStyle.OkCancel, "New Project") = MsgBoxResult.Ok Then
                'save the HUC8s as a new shapefile
                Dim lHUC8ShapefileName As String = GisUtilDS.LayerFileName("Cataloging Units")
                Dim lNewShapefileName As String = aNewDataDir & FilenameNoPath(lHUC8ShapefileName)
                'Dim lLayerIndex As Integer = GisUtil.LayerIndex("Cataloging Units")
                Dim lHuc8Layer As IMapFeatureLayer = GisUtilDS.GetLayerByName("Cataloging Units")
                Dim lNumSelectedFeatures As Integer = lHuc8Layer.Selection.Count
                Dim lInputProjection As String = lHuc8Layer.DataSet.ProjectionString
                If lNumSelectedFeatures = 0 Then
                    'are any features selected in this cat shapefile?  it could be the another layer that was selected
                    'select the lhuc8 features
                    Dim lFieldIndex As Integer = GisUtilDS.GetFieldIndexByName(lHuc8Layer, "CU")
                    Dim lExpression As String = ""
                    For Each lHuc8 As String In lHUC8s
                        lExpression &= "[CU] like '" & lHuc8 & "' Or "
                    Next
                    lHuc8Layer.ClearSelection()
                    lExpression = lExpression.Substring(0, lExpression.Length - 4)
                    lHuc8Layer.SelectByAttribute(lExpression)
                End If
                GisUtilDS.SaveSelectedFeatures(lHUC8ShapefileName, lNewShapefileName, False)
                'change projection
                Dim lOutputProjection As String = CleanUpUserProjString(IO.File.ReadAllText(aNewDataDir & "prj.proj"))
                Dim lNewProjectionInfo As New ProjectionInfo()
                If lNewProjectionInfo.TryParseEsriString(lOutputProjection) Then
                    Try
                        lHuc8Layer.Reproject(lNewProjectionInfo)
                        lHuc8Layer.DataSet.SaveAs(lNewShapefileName, True)
                    Catch ex As Exception
                        Logger.Msg("Problem projecting the HUC8 shapefile.", "Projection Problem")
                    End Try
                Else
                    Logger.Msg("Problem reading output projection file.", "ReProject HUC8 Layer Problem")
                End If

                'put default files in project
                Dim lNationalDir As String = IO.Path.Combine(g_ProgramDir, "Data\national" & g_PathChar)
                If Not IO.Directory.Exists(lNationalDir) Then
                    lNationalDir = IO.Path.Combine(aDataPath, "national" & g_PathChar)
                End If
                If IO.Directory.Exists(lNationalDir) Then
                    CopyFromIfNeeded("sic.dbf", lNationalDir, aNewDataDir)
                    CopyFromIfNeeded("storetag.dbf", lNationalDir, aNewDataDir)
                    CopyFromIfNeeded("wqcriter.dbf", lNationalDir, aNewDataDir)
                    CopyFromIfNeeded("wqobs_prm.dbf", lNationalDir, aNewDataDir)
                End If
                'g_Project.ProjectProjection = lOutputProjection
                'save the new project with just this layer
                ClearLayers()
                AddAllShapesInDir(aNewDataDir, aNewDataDir)
                'g_MapWin.PreviewMap.GetPictureFromMap()
                Dim lProjectFileName As String = IO.Path.ChangeExtension(aProjectFileName, "dspx")
                g_Project.SaveProject(lProjectFileName)
            End If
        End If
        Return lHUC8Status
    End Function

    'Given a file name and the XML describing how to render it, add a grid layer to MapWindow
    Private Function AddGridToMW(ByVal aFilename As String,
                                 ByRef layerXml As Xml.XmlNode) As IMapRasterLayer
        Dim LayerName As String
        Dim Group As String = "Other"
        Dim Visible As Boolean
        'Dim Style As atcRenderStyle = New atcRenderStyle

        Dim MWlay As IMapRasterLayer = Nothing
        Dim g As IMapRasterLayer

        'Don't add layer again if we already have it
        If GisUtilDS.IsLayerByFileName(aFilename, MWlay) Then
            Return MWlay
        End If

        MWlay = Nothing

        Try
            atcD4EMLauncher.SpatialOperations.GetDefaultRenderer(aFilename)

            'g_StatusBar.Item(1).Text = "Opening " & aFilename

            If layerXml Is Nothing Then
                Dim lr As Raster = Raster.OpenFile(aFilename)
                Dim lIr As IRaster = lr
                MWlay = g_MapWin.Map.Layers.Add(lr)
                MWlay.IsVisible = True
            Else
                LayerName = layerXml.Attributes.GetNamedItem("Name").InnerText
                'Style.xml = layerXml.FirstChild
                Group = layerXml.Attributes.GetNamedItem("Group").InnerText
                If Group Is Nothing Then Group = "Other"
                Select Case layerXml.Attributes.GetNamedItem("Visible").InnerText.ToLower
                    Case "yes" : Visible = True
                    Case "no" : Visible = False
                    Case Else : Visible = False
                End Select
                If LayerName = "" Then LayerName = IO.Path.GetFileNameWithoutExtension(aFilename)
                Select Case LayerName
                    Case "National Elevation Dataset", "DEM Elevation Model", "Digital Elevation Model"
                        LayerName &= " (" & IO.Path.GetFileNameWithoutExtension(aFilename) & ")"
                End Select

                'g = RasterLayer.OpenFile(aFilename)
                Dim lr As Raster = Raster.OpenFile(aFilename)
                Dim lIr As IRaster = lr
                'ensure correct Nodata value is set in NLCD grids
                If aFilename.ToLower.Contains(g_PathChar & "nlcd" & g_PathChar) Then
                    If lr.NoDataValue <> 0 Then
                        lr.NoDataValue = 0
                        Try
                            lr.Save()
                            lr.Close()
                            lr = Raster.OpenFile(aFilename)
                        Catch e As Exception
                            Logger.Msg(aFilename & vbLf & e.Message & vbLf & "This may cause unwanted display of areas with missing data", "Unable to set zero as NoData value")
                        End Try
                    End If
                End If
                Dim lProjectProjection As String = g_MapWin.Map.Projection.ToProj4String
                If lProjectProjection IsNot Nothing AndAlso lProjectProjection.Length > 1 Then
                    Dim lLayerProjection4 As String = lr.ProjectionString
                    If lLayerProjection4 Is Nothing OrElse lLayerProjection4.Trim().Length = 0 Then
                        Dim lPrjFileName As String = IO.Path.ChangeExtension(lr.Filename, ".proj4")
                        If IO.File.Exists(lPrjFileName) Then
                            lLayerProjection4 = WholeFileString(lPrjFileName)
                            'Else
                            '    lPrjFileName = IO.Path.ChangeExtension(g.Filename, ".prj")
                            '    If IO.File.Exists(lPrjFileName) Then

                            '    End If
                        End If
                    End If
                    If lLayerProjection4 IsNot Nothing AndAlso lLayerProjection4.Trim().Length > 0 AndAlso lLayerProjection4 <> lProjectProjection Then
                        'g_MapWin.Map.Layers.Remove(lr)
                        Logger.Status("Reprojecting Grid, this may take several minutes: " & aFilename)
                        'the problem with GetTemporaryFileName is that we want this file to be permanent, so don't use this
                        'Dim lOutputFileName As String = GetTemporaryFileName(IO.Path.Combine(PathNameOnly(aFilename), FilenameNoExt(aFilename)), IO.Path.GetExtension(aFilename))
                        Dim lOutputFileName As String = IO.Path.Combine(PathNameOnly(aFilename), FilenameNoExt(aFilename)) & "_reproj" & IO.Path.GetExtension(aFilename)
                        'ToDo: how to reproject grid in DotSpatial
                        'MapWinGeoProc.SpatialReference.ProjectGrid(lLayerProjection4, g_Project.ProjectProjection, aFilename, lOutputFileName, True)
                        Logger.Status("Opening " & aFilename)
                        If IO.File.Exists(lOutputFileName) Then
                            'g = RasterLayer.OpenFile(lOutputFileName)
                            lr = Raster.OpenFile(aFilename)
                            lIr = lr
                        Else
                            'g = RasterLayer.OpenFile(aFilename)
                            lr = Raster.OpenFile(aFilename)
                            lIr = lr
                        End If
                        Logger.Status("")
                    End If
                End If

                'g = g_MapWin.Map.Layers.Add(LayerName)
                'g.IsVisible = Visible
                MWlay = g_MapWin.Map.Layers.Add(lr)
                MWlay.IsVisible = Visible
                'g.UseTransparentColor = True

                'TODO: replace hard-coded SetElevationGridColors with full renderer from defaults
                If aFilename.ToLower.EndsWith("demg.tif") Then
                    SetElevationGridColors(MWlay, g)
                ElseIf aFilename.ToLower.Contains(g_PathChar & "ned" & g_PathChar) Then
                    SetElevationGridColors(MWlay, g)
                ElseIf aFilename.ToLower.Contains(g_PathChar & "nlcd" & g_PathChar) Then
                    SetLandUseLegend(MWlay)
                End If
            End If

            If Not String.IsNullOrEmpty(Group) Then AddLayerToGroup(MWlay, Group)

            If MWlay.IsVisible Then
                g_MapWin.Map.Refresh()
                DoEvents()
            End If
        Catch ex As Exception
            Logger.Msg("Could Not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Grid")
        End Try
        'g_StatusBar.Item(1).Text = ""

        Return MWlay
    End Function

    Private Sub SetLandUseLegend(ByVal aMWlay As IMapRasterLayer)
        Dim lLegItems As DotSpatial.Symbology.ColorCategoryCollection = aMWlay.LegendItems
        For Each lLegItem As ILegendItem In lLegItems
            If lLegItem.LegendText = "11" Then
                lLegItem.LegendText = "Water-Open"
            ElseIf lLegItem.LegendText = "12" Then
                lLegItem.LegendText = "IceSnow-Perennial"
            ElseIf lLegItem.LegendText = "21" Then
                lLegItem.LegendText = "Developed-Open Space"
            ElseIf lLegItem.LegendText = "22" Then
                lLegItem.LegendText = "Developed-Low Intensity"
            ElseIf lLegItem.LegendText = "23" Then
                lLegItem.LegendText = "Developed-Medium Intensity"
            ElseIf lLegItem.LegendText = "24" Then
                lLegItem.LegendText = "Developed-High Intensity"
            ElseIf lLegItem.LegendText = "31" Then
                lLegItem.LegendText = "Barren Land"
            ElseIf lLegItem.LegendText = "32" Then
                lLegItem.LegendText = "Unconsolidated Shore"
            ElseIf lLegItem.LegendText = "33" Then
                lLegItem.LegendText = "Transitional"
            ElseIf lLegItem.LegendText = "41" Then
                lLegItem.LegendText = "Forest-Deciduous"
            ElseIf lLegItem.LegendText = "42" Then
                lLegItem.LegendText = "Forest-Evergreen"
            ElseIf lLegItem.LegendText = "43" Then
                lLegItem.LegendText = "Forest-Mixed"
            ElseIf lLegItem.LegendText = "51" Then
                lLegItem.LegendText = "Scrub-Dwarf"
            ElseIf lLegItem.LegendText = "52" Then
                lLegItem.LegendText = "Scrub-Scrub"
            ElseIf lLegItem.LegendText = "71" Then
                lLegItem.LegendText = "Grassland"
            ElseIf lLegItem.LegendText = "81" Then
                lLegItem.LegendText = "Agriculture-Pasture"
            ElseIf lLegItem.LegendText = "82" Then
                lLegItem.LegendText = "Agriculture-Cultivated Crops"
            ElseIf lLegItem.LegendText = "90" Then
                lLegItem.LegendText = "Wetlands-Woody"
            ElseIf lLegItem.LegendText = "91" Then
                lLegItem.LegendText = "Wetlands-Palustrine Forested"
            ElseIf lLegItem.LegendText = "92" Then
                lLegItem.LegendText = "Wetlands-Palustrine Scrub"
            ElseIf lLegItem.LegendText = "93" Then
                lLegItem.LegendText = "Wetlands-Estuarine Forested"
            ElseIf lLegItem.LegendText = "94" Then
                lLegItem.LegendText = "Wetlands-Estuarine Scrub"
            ElseIf lLegItem.LegendText = "95" Then
                lLegItem.LegendText = "Wetlands-Emergent Herbaceous"
            ElseIf lLegItem.LegendText = "96" Then
                lLegItem.LegendText = "Wetlands-Palustrine Emergent"
            ElseIf lLegItem.LegendText = "97" Then
                lLegItem.LegendText = "Wetlands-Estuarine Emergent"
            ElseIf lLegItem.LegendText = "98" Then
                lLegItem.LegendText = "Aquatic Bed-Paustrine"
            ElseIf lLegItem.LegendText = "99" Then
                lLegItem.LegendText = "Aquatic Bed-Esturarine"
            Else
                lLegItem.LegendItemVisible = False
            End If
        Next
        Dim lIndex As Integer = 0
        Do Until lIndex > lLegItems.Count - 1
            If lLegItems(lIndex).LegendItemVisible = False Then
                lLegItems.RemoveAt(lIndex)
            Else
                lIndex += 1
            End If
        Loop
    End Sub

    'Private Sub SetLandUseColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
    '    Dim colorBreak As MapWinGIS.ShapefileColorBreak
    '    Dim colorScheme As MapWinGIS.ShapefileColorScheme

    '    MWlay.Name &= " " & IO.Path.GetFileNameWithoutExtension(shpFile.Filename).Substring(2)
    '    colorScheme = New MapWinGIS.ShapefileColorScheme
    '    colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "lucode")
    '    If colorScheme.FieldIndex > 0 Then
    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Urban or Built-up Land"
    '        colorBreak.StartValue = 1
    '        colorBreak.EndValue = 19
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(120, 120, 120))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Agricultural Land"
    '        colorBreak.StartValue = 20
    '        colorBreak.EndValue = 29
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 255, 0))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Rangeland"
    '        colorBreak.StartValue = 30
    '        colorBreak.EndValue = 39
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(146, 174, 47))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Forest Land"
    '        colorBreak.StartValue = 40
    '        colorBreak.EndValue = 49
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(161, 102, 50))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Water"
    '        colorBreak.StartValue = 50
    '        colorBreak.EndValue = 59
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Wetland"
    '        colorBreak.StartValue = 60
    '        colorBreak.EndValue = 69
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 209, 220))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Barren Land"
    '        colorBreak.StartValue = 70
    '        colorBreak.EndValue = 79
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Tundra"
    '        colorBreak.StartValue = 80
    '        colorBreak.EndValue = 89
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(60, 105, 0))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        colorBreak = New MapWinGIS.ShapefileColorBreak
    '        colorBreak.Caption = "Perennial Snow or Ice"
    '        colorBreak.StartValue = 90
    '        colorBreak.EndValue = 99
    '        colorBreak.StartColor = System.Convert.ToUInt32(RGB(210, 210, 210))
    '        colorBreak.EndColor = colorBreak.StartColor
    '        colorScheme.Add(colorBreak)

    '        MWlay.ColoringScheme = colorScheme
    '        MWlay.DrawFill = True
    '        MWlay.LineOrPointSize = 0
    '        MWlay.OutlineColor = System.Drawing.Color.Black
    '    End If
    'End Sub

    'Private Sub SetDemColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
    '    Dim colorBreak As MapWinGIS.ShapefileColorBreak
    '    Dim colorScheme As MapWinGIS.ShapefileColorScheme
    '    Dim minelev As Single
    '    Dim maxelev As Single
    '    Dim inc As Single
    '    Dim i As Integer

    '    colorScheme = New MapWinGIS.ShapefileColorScheme
    '    colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "elev_m")
    '    'determine min and max elevations
    '    minelev = 9999999
    '    maxelev = -9999999
    '    For i = 1 To shpFile.NumShapes
    '        If shpFile.CellValue(colorScheme.FieldIndex, i) > maxelev Then
    '            maxelev = shpFile.CellValue(colorScheme.FieldIndex, i)
    '        End If
    '        If shpFile.CellValue(colorScheme.FieldIndex, i) < minelev Then
    '            minelev = shpFile.CellValue(colorScheme.FieldIndex, i)
    '        End If
    '    Next i
    '    inc = (maxelev - minelev) / 10

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev) & " - " & CStr(minelev + inc)
    '    colorBreak.StartValue = minelev
    '    colorBreak.EndValue = minelev + inc
    '    colorBreak.StartColor = System.Convert.ToUInt32(32768)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + inc) & " - " & CStr(minelev + (2 * inc))
    '    colorBreak.StartValue = minelev + inc
    '    colorBreak.EndValue = minelev + (2 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(888090)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (2 * inc)) & " - " & CStr(minelev + (3 * inc))
    '    colorBreak.StartValue = minelev + (2 * inc)
    '    colorBreak.EndValue = minelev + (3 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(1743412)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (3 * inc)) & " - " & CStr(minelev + (4 * inc))
    '    colorBreak.StartValue = minelev + (3 * inc)
    '    colorBreak.EndValue = minelev + (4 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(2598734)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (4 * inc)) & " - " & CStr(minelev + (5 * inc))
    '    colorBreak.StartValue = minelev + (4 * inc)
    '    colorBreak.EndValue = minelev + (5 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(3454056)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (5 * inc)) & " - " & CStr(minelev + (6 * inc))
    '    colorBreak.StartValue = minelev + (5 * inc)
    '    colorBreak.EndValue = minelev + (6 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(4309378)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (6 * inc)) & " - " & CStr(minelev + (7 * inc))
    '    colorBreak.StartValue = minelev + (6 * inc)
    '    colorBreak.EndValue = minelev + (7 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(5164700)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (7 * inc)) & " - " & CStr(minelev + (8 * inc))
    '    colorBreak.StartValue = minelev + (7 * inc)
    '    colorBreak.EndValue = minelev + (8 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(6020022)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (8 * inc)) & " - " & CStr(minelev + (9 * inc))
    '    colorBreak.StartValue = minelev + (8 * inc)
    '    colorBreak.EndValue = minelev + (9 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(6875344)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    colorBreak = New MapWinGIS.ShapefileColorBreak
    '    colorBreak.Caption = CStr(minelev + (9 * inc)) & " - " & CStr(minelev + (10 * inc))
    '    colorBreak.StartValue = minelev + (9 * inc)
    '    colorBreak.EndValue = minelev + (10 * inc)
    '    colorBreak.StartColor = System.Convert.ToUInt32(7730666)
    '    colorBreak.EndColor = colorBreak.StartColor
    '    colorScheme.Add(colorBreak)

    '    MWlay.ColoringScheme = colorScheme
    '    MWlay.DrawFill = True
    '    MWlay.LineOrPointSize = 0
    '    MWlay.OutlineColor = System.Drawing.Color.Black
    'End Sub

    ''' <summary>
    ''' Set colors for DEM as Shapefile
    ''' </summary>
    ''' <remarks>New MW 4.8 symbology</remarks>
    Private Sub SetDemColors(ByVal MWlay As ILayer, ByVal shpFile As IMapFeatureLayer)
        'Dim options As MapWinGIS.ShapeDrawingOptions = shpFile.DefaultDrawingOptions
        'options.FillVisible = True
        'options.LineVisible = False
        'options.LineColor = 0
        'options.FillColor = Convert.ToUInt32(32768)

        'Dim lFieldIndex As Integer = ShpFieldNumFromName(shpFile, "elev_m")

        'shpFile.Categories.Generate(lFieldIndex, MapWinGIS.tkClassificationType.ctEqualIntervals, 10) ', minelev, maxelev)
        'shpFile.Categories.ApplyExpressions()
        'shpFile.Categories.Item(0).DrawingOptions.FillColor = System.Convert.ToUInt32(32768)
        'shpFile.Categories.Item(1).DrawingOptions.FillColor = System.Convert.ToUInt32(888090)
        'shpFile.Categories.Item(2).DrawingOptions.FillColor = System.Convert.ToUInt32(1743412)
        'shpFile.Categories.Item(3).DrawingOptions.FillColor = System.Convert.ToUInt32(2598734)
        'shpFile.Categories.Item(4).DrawingOptions.FillColor = System.Convert.ToUInt32(3454056)
        'shpFile.Categories.Item(5).DrawingOptions.FillColor = System.Convert.ToUInt32(4309378)
        'shpFile.Categories.Item(6).DrawingOptions.FillColor = System.Convert.ToUInt32(5164700)
        'shpFile.Categories.Item(7).DrawingOptions.FillColor = System.Convert.ToUInt32(6020022)
        'shpFile.Categories.Item(8).DrawingOptions.FillColor = System.Convert.ToUInt32(6875344)
        'shpFile.Categories.Item(9).DrawingOptions.FillColor = System.Convert.ToUInt32(7730666)

        Dim lScheme As New PolygonScheme
        lScheme.Categories.Clear()
        lScheme.EditorSettings.ClassificationType = ClassificationType.Quantities
        lScheme.EditorSettings.IntervalMethod = IntervalMethod.EqualInterval
        lScheme.EditorSettings.IntervalSnapMethod = IntervalSnapMethod.Rounding
        lScheme.EditorSettings.IntervalRoundingDigits = 5
        lScheme.EditorSettings.TemplateSymbolizer = New PolygonSymbolizer(System.Drawing.Color.Yellow, System.Drawing.Color.Gray)
        lScheme.EditorSettings.FieldName = "Area"
        lScheme.CreateCategories(shpFile.DataSet.DataTable)
        shpFile.Symbology = lScheme
    End Sub

    ''' <summary>
    ''' Set icons for met stations
    ''' </summary>
    ''' <remarks>New MW 4.8 symbology</remarks>
    Friend Sub SetMetIcons(ByVal MWlay As IMapFeatureLayer, ByVal shpFile As IMapFeatureLayer)
        Try
            '            Dim lRenderersPath As String = IO.Path.Combine(PathNameOnly(PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)), "etc") & g_PathChar & "renderers" & g_PathChar
            '            If shpFile.Field(5).Name = "CONSTITUEN" Then
            '                shpFile.Categories.Generate(5, MapWinGIS.tkClassificationType.ctUniqueValues, 7)
            '                shpFile.Categories.ApplyExpressions()
            '                Dim lLastShape As Integer = Math.Min(6, shpFile.Categories.Count - 1)
            '                For iShape As Integer = 0 To lLastShape
            '                    Try
            '                        With shpFile.Categories.Item(iShape)
            '                            Logger.Dbg("Met Station Category: " & .Expression)
            '                            Dim lConstituent As String = .Expression.Split("""")(1)
            '                            Dim lIconFilename As String = IO.Path.Combine(lRenderersPath, "met-" & lConstituent & ".png")
            '                            With .DrawingOptions
            '                                If IO.File.Exists(lIconFilename) Then
            '                                    Logger.Dbg("Set met station icon for " & lConstituent)
            '                                    Dim img As New MapWinGIS.Image
            '                                    If Not img.Open(lIconFilename) Then GoTo NoIcon
            '                                    .PointType = tkPointSymbolType.ptSymbolPicture
            '                                    .PointRotation = 0
            '                                    .Picture = img
            '                                    .FillBgTransparent = True
            '                                Else
            'NoIcon:                             Logger.Dbg("Icon not found for met station at " & lIconFilename)
            '                                    .PointType = tkPointSymbolType.ptSymbolStandard
            '                                    .PointRotation = 45
            '                                    .PointShape = tkPointShapeType.ptShapeRegular
            '                                    .PointSidesCount = 4
            '                                    .PointSize = 10
            '                                End If
            '                            End With
            '                        End With
            '                    Catch e As Exception
            '                        Logger.Dbg("Set Met Icon " & iShape & ": " & e.ToString)
            '                    End Try
            '                Next
            '                shpFile.Save()
            '                shpFile.CollisionMode = tkCollisionMode.AllowCollisions
            '            End If
        Catch e2 As Exception
            Logger.Dbg("SetMetIcons: " & e2.ToString)
        End Try
    End Sub

    Private Sub SetElevationGridColors(ByVal MWlay As IMapFeatureLayer, ByVal g As IRasterLayer)
        'Dim colorScheme As MapWinGIS.GridColorScheme

        'ColorScheme = New MapWinGIS.GridColorScheme
        'ColorScheme.UsePredefined(g.Minimum, g.Maximum, PredefinedColorScheme.FallLeaves)
        'MWlay.ColoringScheme = colorScheme
    End Sub

    'Friend Sub SetCensusRenderer(ByVal MWlay As MapWindow.Interfaces.Layer, Optional ByVal shpFile As MapWinGIS.Shapefile = Nothing)
    '    If shpFile Is Nothing Then shpFile = MWlay.GetObject

    '    g_MapWin.View.LockMap() 'keep the map from updating during the loop.

    '    'NOTE: using atcTableDBF - with 80K records - processing reduced from 3.5 to 2.5 seconds
    '    Dim lShapeDbf As New atcTableDBF
    '    lShapeDbf.OpenFile(IO.Path.ChangeExtension(shpFile.Filename, ".dbf"))
    '    Dim lFieldCFCC As Integer = lShapeDbf.FieldNumber("CFCC")
    '    'Dim lFieldCFCC As Integer = ShpFieldNumFromName(shpFile, "CFCC")

    '    Dim lCurrentCFCC As Integer
    '    Dim lLastShapeIndex As Integer = shpFile.NumShapes - 1
    '    For iShape As Integer = 0 To lLastShapeIndex
    '        'lCurrentCFCC = shpFile.CellValue(lFieldCFCC, iShape).ToString.Substring(1)
    '        lCurrentCFCC = lShapeDbf.Value(lFieldCFCC).Substring(1)
    '        lShapeDbf.MoveNext()
    '        Select Case lCurrentCFCC
    '            Case 4, 41 To 48, 63, 64 'most likely
    '            Case 1 To 3, 11 To 38
    '                MWlay.Shapes(iShape).LineOrPointSize = 2
    '            Case 2, 21 To 28
    '                MWlay.Shapes(iShape).LineOrPointSize = 2
    '            Case 3, 31 To 38
    '                MWlay.Shapes(iShape).LineOrPointSize = 2
    '            Case Else 'A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
    '                MWlay.Shapes(iShape).LineStipple = MapWinGIS.tkLineStipple.lsDotted
    '        End Select
    '    Next

    '    g_MapWin.View.UnlockMap() 'let the map redraw again
    'End Sub

    'Private Sub SetCensusColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
    '    Dim colorBreak As MapWinGIS.ShapefileColorBreak
    '    Dim colorScheme As MapWinGIS.ShapefileColorScheme
    '    Dim prefix As String = (MWlay.FileName.ToUpper.Chars(MWlay.FileName.Length - 5))

    '    MWlay.Name &= " " & Left(IO.Path.GetFileNameWithoutExtension(shpFile.Filename), 8)

    '    If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") OrElse _
    '       MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
    '        'Color the roads
    '        colorScheme = New MapWinGIS.ShapefileColorScheme
    '        colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "CFCC")
    '        If colorScheme.FieldIndex > 0 Then
    '            colorBreak = New MapWinGIS.ShapefileColorBreak
    '            colorBreak.Caption = "Primary limited access"
    '            colorBreak.StartValue = prefix & "1"
    '            colorBreak.EndValue = prefix & "18"
    '            colorBreak.StartColor = System.Convert.ToUInt32(RGB(132, 0, 0))
    '            colorBreak.EndColor = colorBreak.StartColor
    '            'TODO: renderer should be able to change line width: LineWidth = 2
    '            colorScheme.Add(colorBreak)

    '            colorBreak = New MapWinGIS.ShapefileColorBreak
    '            colorBreak.Caption = "Primary non-limited access"
    '            colorBreak.StartValue = prefix & "2"
    '            colorBreak.EndValue = prefix & "28"
    '            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 0))
    '            colorBreak.EndColor = colorBreak.StartColor
    '            'TODO: renderer should be able to change line width: LineWidth = 2
    '            colorScheme.Add(colorBreak)

    '            colorBreak = New MapWinGIS.ShapefileColorBreak
    '            colorBreak.Caption = "Secondary"
    '            colorBreak.StartValue = prefix & "3"
    '            colorBreak.EndValue = prefix & "38"
    '            colorBreak.StartColor = System.Convert.ToUInt32(RGB(122, 122, 122))
    '            'TODO: renderer should be able to change line width: LineWidth = 2
    '            colorBreak.EndColor = colorBreak.StartColor
    '            colorScheme.Add(colorBreak)

    '            colorBreak = New MapWinGIS.ShapefileColorBreak
    '            colorBreak.Caption = "Local"
    '            colorBreak.StartValue = prefix & "4" 'TODO: A4, A41, A42, A43, A44, A45, A46, A47, A48, A63, A64
    '            colorBreak.EndValue = prefix & "48"
    '            colorBreak.StartColor = System.Convert.ToUInt32(RGB(166, 166, 166))
    '            colorBreak.EndColor = colorBreak.StartColor
    '            colorScheme.Add(colorBreak)

    '            colorBreak = New MapWinGIS.ShapefileColorBreak
    '            colorBreak.Caption = "Other"
    '            colorBreak.StartValue = prefix & "5" 'TODO: A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
    '            colorBreak.EndValue = "Z"
    '            colorBreak.StartColor = System.Convert.ToUInt32(RGB(200, 200, 200))
    '            'TODO: renderer should be able to change line style: LineStyle = Dashed
    '            colorBreak.EndColor = colorBreak.StartColor
    '            colorScheme.Add(colorBreak)

    '            MWlay.ColoringScheme = colorScheme
    '            MWlay.LineOrPointSize = 1
    '            MWlay.OutlineColor = System.Drawing.Color.Black

    '            SetCensusRenderer(MWlay, shpFile)
    '        End If
    '    End If
    'End Sub

    Private Function ShpFieldNumFromName(ByVal aShpFile As IMapFeatureLayer, ByVal aFieldName As String) As Integer
        Return GisUtilDS.GetFieldIndexByName(aShpFile, aFieldName)
    End Function

    Public Function CheckAddress(ByVal URL As String) As Boolean
        Try
            Logger.Dbg("CheckAddress " & URL)
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
            response.Close()
        Catch ex As Exception
            Logger.Dbg("CheckAddress Failed " & ex.ToString)
            Return False
        End Try
        Return True
    End Function
End Module
