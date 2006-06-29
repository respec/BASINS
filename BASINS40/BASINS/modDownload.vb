Option Strict Off
Option Explicit On

Imports MapWinGIS
Imports System.Collections.Specialized
Imports System.Windows.Forms.Application
Imports MapWinUtility
Imports atcUtility

Module modDownload

    Private Const XMLappName As String = "BASINS System Application"

    'Returns file name of new project or "" if not built
    Friend Function CreateNewProjectAndDownloadCoreDataInteractive(ByVal aThemeTag As String, ByVal aSelectedFeatures As ArrayList) As String
        Dim lDataPath As String
        Dim lDefaultProjectFileName As String
        Dim lProjectFileName As String
        Dim lNoData As Boolean = False
        Dim lDefDirName As String
        Dim lNewDataDir As String
        Dim lSuffix As Integer
        Dim lMyProjection As String
        Dim lSaveDialog As Windows.Forms.SaveFileDialog

StartOver:
        lDataPath = g_BasinsDrives.Chars(0) & ":\Basins\data\"

        Logger.Dbg("SelectedFeatures" & aSelectedFeatures.Count)
        Select Case aSelectedFeatures.Count
            Case 0
                If lNoData Then
                    'Already came through here, don't ask again
                Else
                    lNoData = True
                    If Logger.Msg("No features have been selected.  Do you wish to create a project with no data?", MsgBoxStyle.YesNo, "BASINS Data Extraction") = MsgBoxResult.No Then
                        Return ""
                    End If
                End If
                lDefDirName = "NewProject"
            Case 1
                lDefDirName = aSelectedFeatures(0)
            Case Else
                lDefDirName = "Multiple"
        End Select

        If FileExists(lDataPath & lDefDirName, True) Then 'Find a suffix that will make name unique
            lSuffix = 0
            Do
                lSuffix += 1
            Loop While FileExists(lDataPath & lDefDirName & "-" & lSuffix, True)
            lDefDirName = lDefDirName & "-" & lSuffix
        End If

        lDefaultProjectFileName = lDataPath & lDefDirName & "\" & lDefDirName & ".mwprj"
        lDefDirName = PathNameOnly(lDefaultProjectFileName)
        Logger.Dbg("CreateNewProjectDirectory:" & lDefDirName)
        System.IO.Directory.CreateDirectory(lDefDirName)

        lSaveDialog = New Windows.Forms.SaveFileDialog
        lSaveDialog.Title = "Save new project as..."
        lSaveDialog.CheckPathExists = False
        lSaveDialog.FileName = lDefaultProjectFileName
        If lSaveDialog.ShowDialog() <> Windows.Forms.DialogResult.OK Then
            lSaveDialog.FileName = "\"
            lSaveDialog.Dispose()
            System.IO.Directory.Delete(PathNameOnly(lDefaultProjectFileName), False) 'Cancelled save dialog
            Logger.Dbg("CreateNewProject:CANCELLED")
            Return ""
        Else
            lProjectFileName = lSaveDialog.FileName
            lNewDataDir = PathNameOnly(lProjectFileName) & "\"
            Logger.Dbg("CreateNewProjectDir:" & lNewDataDir)
            Logger.Dbg("CreateNewProjectName:" & lProjectFileName)

            'Make sure lSaveDialog is not holding a reference to the file so we can delete the dir if needed
            lSaveDialog.FileName = "\"
            lSaveDialog.Dispose()

            'If the user did not choose the default folder or a subfolder of it
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
            Dim lNumFiles As Long = System.IO.Directory.GetFiles(lNewDataDir).LongLength
            Dim lNumDirs As Long = System.IO.Directory.GetDirectories(lNewDataDir).LongLength
            If lNumFiles + lNumDirs > 0 Then
                Logger.Msg("The folder '" & lNewDataDir & "'" & vbCr _
                       & "already contains " & lNumFiles & " files and " & lNumDirs & " folders." & vbCr _
                       & "The folder must be empty before a new project can be created here.", "BASINS Build New")
                GoTo StartOver
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
                    With g_MapWin
                        .Layers.Clear()
                        .PreviewMap.GetPictureFromMap()
                        .Project.Save(lProjectFileName)
                        .Project.Modified = True
                        .Project.Save(lProjectFileName)
                        .Project.Modified = False
                    End With
                Else
                    'download and project core data
                    Logger.Dbg("DownloadData:" & aThemeTag)
                    CreateNewProjectAndDownloadCoreData(aThemeTag, aSelectedFeatures, lDataPath, lNewDataDir, lProjectFileName)
                End If
                Return lProjectFileName
            End If
        End If
    End Function

    'Returns file name of new project or "" if not built
    Public Sub CreateNewProjectAndDownloadCoreData(ByVal aThemeTag As String, _
                                                   ByVal aSelectedFeatures As ArrayList, _
                                                   ByVal aDataPath As String, _
                                                   ByVal aNewDataDir As String, _
                                                   ByVal aProjectFileName As String)
        Dim lDownloadXml As String
        Dim lFeature As Integer
        Dim lDownloadFilename As String = aDataPath & "download.xml"
        Dim lProjectorFilename As String = aDataPath & "ATCProjector.xml"

        lDownloadXml = "<clsWebDataManager>" & vbCrLf & " <status_variables>" & vbCrLf & "  <download_type status=""set by " & XMLappName & """>BasinsInitialSetup</download_type>" & vbCrLf & "  <launched_by>" & XMLappName & "</launched_by>" & vbCrLf & "  <project_dir status=""set by " & XMLappName & """>" & aNewDataDir & "</project_dir>" & vbCrLf

        For lFeature = 0 To aSelectedFeatures.Count - 1
            lDownloadXml &= "  <" & aThemeTag & " status=""set by " & XMLappName & """>" & aSelectedFeatures(lFeature) & "</" & aThemeTag & ">" & vbCrLf
        Next

        lDownloadXml &= " </status_variables>" & vbCrLf & "</clsWebDataManager>" & vbCrLf

        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If

        SaveFileString(lDownloadFilename, lDownloadXml)
        If DataDownload(lDownloadFilename) Then 'Succeeded, as far as we know
            g_MapWin.Layers.Clear()
            g_MapWin.Project.Save(aProjectFileName)
            g_MapWin.Project.Modified = True
            ProcessProjectorFile(lProjectorFilename)
            AddAllShapesInDir(aNewDataDir, aNewDataDir)
            g_MapWin.Project.Save(aProjectFileName)
            'TODO: should save set .Modified?
            g_MapWin.Project.Modified = False
        End If
    End Sub

    'Download new data for an existing project
    Friend Sub DownloadNewData(ByRef aProjectDir As String)
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
            ProcessProjectorFile(lProjectorFilename)
        End If

        If FileExists(lProjectorFilename) Then
            Kill(lProjectorFilename)
        End If
    End Sub

    Private Sub ProcessProjectorFile(ByVal aProjectorFilename As String)
        Dim lProjectorXML As New Chilkat.Xml
        Dim lProjectorNode As Chilkat.Xml
        Dim lInputDirName As String
        Dim lOutputDirName As String
        Dim lOutputFileName As String
        Dim InputFileList As New NameValueCollection
        Dim lFileObject As Object
        Dim lCurFilename As String
        Dim lProjectDir As String = ""
        Dim lDefaultsXML As Chilkat.Xml = Nothing
        Dim lSuccess As Boolean
        Dim lInputProjection As String
        Dim lOutputProjection As String

        g_MapWin.View.MapCursor = tkCursor.crsrWait
        If Not FileExists(aProjectorFilename) Then
            Logger.Dbg("No new ATCProjector.xml to process")
        Else
            lProjectorXML.LoadXml(WholeFileString(aProjectorFilename))
            lProjectorNode = lProjectorXML.FirstChild
            While Not lProjectorNode Is Nothing
                Logger.Dbg("Processing XML: " & lProjectorNode.GetXml)
                Select Case LCase(lProjectorNode.Tag.ToLower)
                    Case "add_shape"
                        lOutputFileName = lProjectorNode.Content
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        AddShapeToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                    Case "add_grid"
                        lOutputFileName = lProjectorNode.Content
                        If lDefaultsXML Is Nothing Then lDefaultsXML = GetDefaultsXML()
                        AddGridToMW(lOutputFileName, GetDefaultsFor(lOutputFileName, lProjectDir, lDefaultsXML))
                        If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                            'create .prj file as work-around for bug
                            SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                        End If
                    Case "add_allshapes"
                        lOutputFileName = lProjectorNode.Content
                        AddAllShapesInDir(lOutputFileName, lProjectDir)
                    Case "clip_grid"
                        lOutputFileName = lProjectorNode.GetAttrValue("output")
                        lCurFilename = lProjectorNode.Content
                        'create extents shape to clip to, at the extents of the cat unit
                        Dim lShape As New MapWinGIS.Shape
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
                        If InStr(lOutputFileName, "\nlcd\") > 0 Then
                            'project the extents into the albers projection for nlcd
                            MkDirPath(lProjectDir & "nlcd")
                            If FileExists(lProjectDir & "nlcd\catextent.shp") Then
                                Try
                                    System.IO.File.Delete(lProjectDir & "nlcd\catextent.shp")
                                    System.IO.File.Delete(lProjectDir & "nlcd\catextent.dbf")
                                    System.IO.File.Delete(lProjectDir & "nlcd\catextent.shx")
                                Catch e As Exception
                                    'Ignore error if files do not exist or can't be removed
                                End Try
                            End If
                            Dim lExtentsSf As New MapWinGIS.Shapefile
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
                        g_MapWin.StatusBar(1).Text = "Clipping Grid..."
                        g_MapWin.Refresh()
                        DoEvents()
                        If Not FileExists(FilenameNoExt(lCurFilename) & ".prj") Then
                            'create .prj file as work-around for clipping bug
                            SaveFileString(FilenameNoExt(lCurFilename) & ".prj", "")
                        End If
                        Logger.Dbg("ClipGridWithPolygon: " & lCurFilename & " " & lProjectDir & "nlcd\catextent.shp" & " " & lOutputFileName & " " & "True")
                        lSuccess = MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(lCurFilename, lShape, lOutputFileName, True)
                        g_MapWin.StatusBar(1).Text = ""
                    Case "project_dir"
                        lProjectDir = lProjectorNode.Content
                        If Right(lProjectDir, 1) <> "\" Then lProjectDir &= "\"
                    Case "convert_shape"
                        lOutputFileName = lProjectorNode.GetAttrValue("output")
                        lCurFilename = lProjectorNode.Content
                        ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                        'if adding to some specific point layers in basins we need to refresh that map layer
                        If Right(lOutputFileName, 8) = "pcs3.shp" Or _
                           Right(lOutputFileName, 8) = "gage.shp" Or _
                           Right(lOutputFileName, 9) = "wqobs.shp" Then
                            'get index of this layer
                            Dim lLayerIndex As Integer = 0
                            Dim i As Integer
                            For i = 1 To g_MapWin.Layers.NumLayers
                                If g_MapWin.Layers(i).FileName = lOutputFileName Then
                                    lLayerIndex = i
                                End If
                            Next
                            If lLayerIndex > 0 Then
                                Dim llayername As String = g_MapWin.Layers(lLayerIndex).Name
                                Dim lRGBcolor As Integer = RGB(g_MapWin.Layers(lLayerIndex).Color.R, g_MapWin.Layers(lLayerIndex).Color.G, g_MapWin.Layers(lLayerIndex).Color.B)
                                Dim lmarkersize As Integer = g_MapWin.Layers(lLayerIndex).LineOrPointSize
                                Dim ltargroup As Integer = g_MapWin.Layers(lLayerIndex).GroupHandle
                                Dim lnewpos As Integer = g_MapWin.Layers(lLayerIndex).GroupPosition
                                Dim MWlay As MapWindow.Interfaces.Layer
                                Dim shpFile As MapWinGIS.Shapefile
                                g_MapWin.Layers.Remove(lLayerIndex)
                                shpFile = New MapWinGIS.Shapefile
                                shpFile.Open(lOutputFileName)
                                MWlay = g_MapWin.Layers.Add(shpFile, llayername, lRGBcolor, lRGBcolor, lmarkersize)
                                g_MapWin.Layers.MoveLayer(MWlay.Handle, lnewpos, ltargroup)
                            End If
                        End If
                    Case "convert_grid"
                        lOutputFileName = lProjectorNode.GetAttrValue("output")
                        lCurFilename = lProjectorNode.Content
                        If FileExists(lOutputFileName) Then
                            'remove output file
                            System.IO.File.Delete(lOutputFileName)
                        End If
                        lOutputProjection = WholeFileString(lProjectDir & "prj.proj")
                        lOutputProjection = CleanUpUserProjString(lOutputProjection)
                        If InStr(lOutputFileName, "\nlcd\") > 0 Then
                            'exception for nlcd data, already in albers
                            lInputProjection = "+proj=aea +ellps=GRS80 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            If lOutputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m" Then
                                'special exception, don't bother to reproject with only this slight datum shift
                                lInputProjection = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
                            End If
                        Else
                            lInputProjection = "+proj=longlat +datum=NAD83"
                        End If
                        If lInputProjection = lOutputProjection Then
                            System.IO.File.Copy(lCurFilename, lOutputFileName)
                        Else
                            'project it
                            g_MapWin.StatusBar(1).Text = "Projecting Grid..."
                            g_MapWin.Refresh()
                            DoEvents()
                            lSuccess = MapWinGeoProc.SpatialReference.ProjectGrid(lInputProjection, lOutputProjection, lCurFilename, lOutputFileName, True)
                            g_MapWin.StatusBar(1).Text = ""
                            If Not FileExists(FilenameNoExt(lOutputFileName) & ".prj") Then
                                'create .prj file as work-around for bug
                                SaveFileString(FilenameNoExt(lOutputFileName) & ".prj", "")
                            End If
                            If Not lSuccess Then
                                Logger.Msg("Failed to project grid" & vbCrLf & MapWinGeoProc.Error.GetLastErrorMsg, "ProcessProjectorFile")
                                System.IO.File.Copy(lCurFilename, lOutputFileName)
                            End If
                        End If
                    Case "convert_dir"
                        'loop through a directory, projecting all files in it
                        lInputDirName = lProjectorNode.Content
                        lOutputDirName = lProjectorNode.GetAttrValue("output")
                        If lOutputDirName Is Nothing OrElse lOutputDirName.Length = 0 Then
                            lOutputDirName = lInputDirName
                        End If
                        If Right(lOutputDirName, 1) <> "\" Then lOutputDirName &= "\"

                        InputFileList.Clear()

                        AddFilesInDir(InputFileList, lInputDirName, False, "*.shp")

                        For Each lFileObject In InputFileList
                            lCurFilename = lFileObject
                            If (FileExt(lCurFilename) = "shp") Then
                                'this is a shapefile
                                lOutputFileName = lOutputDirName & FilenameNoPath(lCurFilename)
                                'change projection and merge
                                If (FileExists(lOutputFileName) And (InStr(1, lOutputFileName, "\landuse\") > 0)) Then
                                    'if the output file exists and it is a landuse shape, dont bother
                                Else
                                    ShapeUtilMerge(lCurFilename, lOutputFileName, lProjectDir & "prj.proj")
                                End If
                            End If
                        Next lFileObject

                    Case Else
                        Logger.Msg("Cannot yet follow directive:" & vbCr & lProjectorNode.Tag, "ProcessProjectorFile")
                End Select

                If Not lProjectorNode.NextSibling2 Then lProjectorNode = Nothing

            End While
        End If
        g_MapWin.View.MapCursor = tkCursor.crsrMapDefault
    End Sub

    Private Function CleanUpUserProjString(ByVal aProjString As String) As String
        Dim lPos As Integer
        Dim lFirst As Boolean

        Do While Mid(aProjString, 1, 1) = "#"
            'eliminate comment lines at beginning
            lPos = InStr(aProjString, vbCrLf)
            aProjString = Mid(aProjString, lPos + 2)
        Loop
        lFirst = True
        Do While InStr(aProjString, vbCrLf) > 0
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
            Else
                aProjString = aProjString & " +datum=NAD83 +units=m"
            End If
        Else
            aProjString = "<none>"
        End If
        CleanUpUserProjString = aProjString
    End Function

    Private Sub ShapeUtilMerge(ByVal aCurFilename As String, ByVal aOutputFilename As String, ByVal aProjectionFilename As String)
        Dim lShapeUtilExe As String = FindFile("Please locate ShapeUtil.exe", "\basins\etc\datadownload\ShapeUtil.exe")
        If lShapeUtilExe.Length > 0 Then
            Dim lLayersDbf As String = GetSetting("ShapeMerge", "files", "layers.dbf")
            If Not FileExists(lLayersDbf) Then
                Logger.Dbg("Did not find layers.dbf in registry " & lLayersDbf)
                lLayersDbf = PathNameOnly(lShapeUtilExe) & "\layers.dbf"
                If FileExists(lLayersDbf) Then
                    Logger.Dbg("Saving layers.dbf location for ShapeUtil: " & lLayersDbf)
                    SaveSetting("ShapeMerge", "files", "layers.dbf", lLayersDbf)
                Else
                    Logger.Dbg("Did not find layers.dbf in same path as ShapeUtil " & lLayersDbf)
                End If
            Else
                Logger.Dbg("Found " & lLayersDbf)
            End If
            'Logger.Msg("MSG2 Merging " & aCurFilename)
            'Logger.Msg("MSG6 into " & aOutputFilename)
            Shell(lShapeUtilExe & " """ & aOutputFilename & """ """ & aCurFilename & """ """ & aProjectionFilename & """", AppWinStyle.NormalNoFocus, True)
        Else
            Logger.Dbg("Failed to find ShapeUtil.exe for merging " & aCurFilename & " into " & aOutputFilename)
        End If
    End Sub

    Private Function DataDownload(ByRef aCommandLine As String) As Boolean
        Dim lDataDownloadExe As String = FindFile("Please locate DataDownload.exe", "\Basins\etc\DataDownload\DataDownload.exe")
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
        '        MsgBox(curStep & vbCr & Err.Description, MsgBoxStyle.OKOnly, "BASINS Data Download Main")
    End Function

    Private Function GetDefaultsXML() As Chilkat.Xml
        Dim lDefaultsXML As Chilkat.Xml = Nothing
        Dim lDefaultsPath As String 'full file name of defaults XML
        lDefaultsPath = FindFile("Please Locate BasinsDefaultLayers.xml", "\basins\etc\BasinsDefaultLayers.xml")
        'lDefaultsPath = "E:\BASINS\etc\BasinsDefaultRenderers.xml"
        If FileExists(lDefaultsPath) Then
            lDefaultsXML = New Chilkat.Xml
            lDefaultsXML.LoadXmlFile(lDefaultsPath)
        End If
        Return lDefaultsXML
    End Function

    'Adds all shape files found in aPath to the current MapWindow project
    Public Sub AddAllShapesInDir(ByVal aPath As String, ByVal aProjectDir As String)
        Dim lLayer As Integer
        Dim Filename As String
        Dim allFiles As New NameValueCollection
        Dim lDefaultsXML As Chilkat.Xml = GetDefaultsXML()

        Logger.Dbg("AddAllShapesInDir: '" & aPath & "'")

        If Right(aPath, 1) <> "\" Then aPath = aPath & "\"
        AddFilesInDir(allFiles, aPath, True, "*.shp")

        For lLayer = 0 To allFiles.Count - 1
            Filename = allFiles.Item(lLayer)
            AddShapeToMW(Filename, GetDefaultsFor(Filename, aProjectDir, lDefaultsXML))
        Next

        'Remove any empty groups (for example, group "Data Layers" should be empty)
        For iGroup As Integer = g_MapWin.Layers.Groups.Count - 1 To 0 Step -1
            If g_MapWin.Layers.Groups.ItemByPosition(iGroup).LayerCount = 0 Then
                g_MapWin.Layers.Groups.Remove(g_MapWin.Layers.Groups.ItemByPosition(iGroup).Handle)
            End If
        Next
        g_MapWin.PreviewMap.GetPictureFromMap()

    End Sub

    'Given a file name and the XML describing how to render it, add a shape layer to MapWindow
    Private Function AddShapeToMW(ByVal aFilename As String, _
                                  ByRef layerXml As Chilkat.Xml) As MapWindow.Interfaces.Layer
        Dim LayerName As String
        Dim Group As String
        Dim Visible As Boolean
        Dim Style As atcRenderStyle = New atcRenderStyle

        Dim MWlay As MapWindow.Interfaces.Layer
        Dim shpFile As MapWinGIS.Shapefile
        Dim RGBcolor As Int32
        Dim RGBoutline As Int32

        'Don't add layer again if we already have it
        For lLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(lLayer))
            If MWlay.FileName.ToLower = aFilename.ToLower Then
                Return MWlay
            End If
        Next
        MWlay = Nothing

        Try
            If layerXml Is Nothing Then
                LayerName = FilenameOnly(aFilename)
                Visible = False 'True
                Group = "Other"
            Else
                LayerName = layerXml.GetAttrValue("Name")
                Style.xml = layerXml.FirstChild
                Group = layerXml.GetAttrValue("Group")
                If Group Is Nothing Then Group = "Other"
                Select Case layerXml.GetAttrValue("Visible").ToLower
                    Case "yes" : Visible = True
                    Case "no" : Visible = False
                    Case Else : Visible = False
                End Select
            End If

            g_MapWin.StatusBar.Item(1).Text = "Opening " & aFilename
            shpFile = New MapWinGIS.Shapefile
            shpFile.Open(aFilename)
            Select Case shpFile.ShapefileType
                Case MapWinGIS.ShpfileType.SHP_POINT, _
                        MapWinGIS.ShpfileType.SHP_POINTM, _
                        MapWinGIS.ShpfileType.SHP_POINTZ, _
                        MapWinGIS.ShpfileType.SHP_MULTIPOINT
                    RGBcolor = RGB(Style.MarkColor.R, Style.MarkColor.G, Style.MarkColor.B)
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.MarkSize)
                    Select Case Style.MarkStyle                        'TODO: translate Cross, X, Bitmap properly
                        Case "Circle" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                        Case "Square" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                        Case "Cross" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleRight
                        Case "X" : MWlay.PointType = MapWinGIS.tkPointType.ptTriangleLeft
                        Case "Diamond" : MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                        Case "Bitmap"
                            Select Case Style.MarkBitsAsHex
                                Case "3C 42 81 99 99 81 42 3C" : MWlay.PointType = MapWinGIS.tkPointType.ptCircle
                                Case "00 7E 7E 7E 7E 7E 7E 00" : MWlay.PointType = MapWinGIS.tkPointType.ptSquare
                                Case "0000 0000 0000 3FF8 3FF8 1FF0 1FF0 0FE0 0FE0 07C0 07C0 0380 0380 0100 0000 0000"
                                    MWlay.PointType = MapWinGIS.tkPointType.ptTriangleDown
                                Case Else
                                    MWlay.PointType = MapWinGIS.tkPointType.ptDiamond
                            End Select
                            'MWlay.PointType = MapWinGIS.tkPointType.ptUserDefined
                            'mwlay.UserPointType = 'TODO: translate bitmap into Image
                    End Select

                Case MapWinGIS.ShpfileType.SHP_POLYLINE, MapWinGIS.ShpfileType.SHP_POLYLINEM, MapWinGIS.ShpfileType.SHP_POLYLINEZ
                    RGBcolor = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBcolor, Style.LineWidth)
                Case MapWinGIS.ShpfileType.SHP_POLYGON, MapWinGIS.ShpfileType.SHP_POLYGONM, MapWinGIS.ShpfileType.SHP_POLYGONZ
                    RGBcolor = RGB(Style.FillColor.R, Style.FillColor.G, Style.FillColor.B)
                    RGBoutline = RGB(Style.LineColor.R, Style.LineColor.G, Style.LineColor.B)
                    MWlay = g_MapWin.Layers.Add(shpFile, LayerName, RGBcolor, RGBoutline, Style.LineWidth)
                    Select Case LCase(Style.FillStyle)
                        Case "none"
                            MWlay.FillStipple = MapWinGIS.tkFillStipple.fsNone
                            If MWlay.Color.Equals(System.Drawing.Color.Black) Then
                                MWlay.Color = System.Drawing.Color.White
                            End If
                            MWlay.DrawFill = False
                        Case "solid" '"Solid"
                        Case "horizontal" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsHorizontalBars
                        Case "vertical" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsVerticalBars
                        Case "down" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownRight
                        Case "up" : MWlay.FillStipple = MapWinGIS.tkFillStipple.fsDiagonalDownLeft
                        Case "cross"
                        Case "diagcross"
                    End Select
            End Select
            Select Case Style.LineStyle.ToLower
                Case "solid" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsNone
                Case "dash" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashed
                Case "dot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDotted
                Case "dashdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
                Case "dashdotdot" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
                    'Case "alternate" : MWlay.LineStipple = MapWinGIS.tkLineStipple.lsCustom
                    '    MWlay.UserLineStipple = 
            End Select

            MWlay.Visible = Visible

            'TODO: replace hard-coded SetLandUseColors and others with full renderer from defaults
            If LCase(aFilename).IndexOf("\landuse\") > 0 Then
                SetLandUseColors(MWlay, shpFile)
            ElseIf LCase(aFilename).IndexOf("\nhd\") > 0 Then
                If InStr(FilenameOnly(shpFile.Filename), "NHD") > 0 Then
                    MWlay.Name = FilenameOnly(shpFile.Filename)
                Else
                    MWlay.Name &= " " & FilenameOnly(shpFile.Filename)
                End If
            ElseIf LCase(aFilename).IndexOf("\census\") > 0 Then
                SetCensusColors(MWlay, shpFile)
            ElseIf LCase(aFilename).IndexOf("\dem\") > 0 Then
                SetDemColors(MWlay, shpFile)
            ElseIf LCase(aFilename).EndsWith("cat.shp") Then
                MWlay.ZoomTo()
            End If
            If Group.Length > 0 Then AddLayerToGroup(MWlay, Group)

            If MWlay.Visible Then
                g_MapWin.View.Redraw()
                DoEvents()
            End If
            g_MapWin.Project.Modified = True
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Shape")
        End Try
        g_MapWin.StatusBar.Item(1).Text = ""

        Return MWlay
    End Function

    'Given a file name and the XML describing how to render it, add a grid layer to MapWindow
    Private Function AddGridToMW(ByVal aFilename As String, _
                                 ByRef layerXml As Chilkat.Xml) As MapWindow.Interfaces.Layer
        Dim LayerName As String
        Dim Group As String
        Dim Visible As Boolean
        Dim Style As atcRenderStyle = New atcRenderStyle

        Dim MWlay As MapWindow.Interfaces.Layer
        Dim g As MapWinGIS.Grid

        'Don't add layer again if we already have it
        For lLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(lLayer))
            If MWlay.FileName.ToLower = aFilename.ToLower Then
                Return MWlay
            End If
        Next
        MWlay = Nothing

        Try
            If layerXml Is Nothing Then
                LayerName = FilenameOnly(aFilename)
                Visible = False 'True
                Group = "Other"
            Else
                LayerName = layerXml.GetAttrValue("Name")
                Style.xml = layerXml.FirstChild
                Group = layerXml.GetAttrValue("Group")
                If Group Is Nothing Then Group = "Other"
                Select Case layerXml.GetAttrValue("Visible").ToLower
                    Case "yes" : Visible = True
                    Case "no" : Visible = False
                    Case Else : Visible = False
                End Select
            End If
            If LayerName = "" Then LayerName = FilenameOnly(aFilename)

            g_MapWin.StatusBar.Item(1).Text = "Opening " & aFilename
            g = New MapWinGIS.Grid
            g.Open(aFilename)

            MWlay = g_MapWin.Layers.Add(g, LayerName)
            MWlay.UseTransparentColor = True

            MWlay.Visible = Visible

            'TODO: replace hard-coded SetLandUseColors and others with full renderer from defaults
            If LCase(aFilename).IndexOf("\demg\") > 0 Then
                SetElevationGridColors(MWlay, g)
            ElseIf LCase(aFilename).IndexOf("\ned\") > 0 Then
                SetElevationGridColors(MWlay, g)
            ElseIf LCase(aFilename).IndexOf("\nlcd\") > 0 Then
                SetLandUseColorsGrid(MWlay, g)
            End If
            If Group.Length > 0 Then AddLayerToGroup(MWlay, Group)

            If MWlay.Visible Then
                g_MapWin.View.Redraw()
                DoEvents()
            End If
            g_MapWin.Project.Modified = True
        Catch ex As Exception
            Logger.Msg("Could not add '" & aFilename & "' to the project. " & ex.ToString & vbCr & ex.StackTrace, "Add Grid")
        End Try
        g_MapWin.StatusBar.Item(1).Text = ""

        Return MWlay
    End Function

    Private Sub AddLayerToGroup(ByVal aLay As MapWindow.Interfaces.Layer, ByVal aGroupName As String)
        Dim GroupHandle As Object
        Dim iExistingGroup As Integer = 0

        While iExistingGroup < g_MapWin.Layers.Groups.Count AndAlso _
              g_MapWin.Layers.Groups.ItemByPosition(iExistingGroup).Text <> aGroupName
            iExistingGroup += 1
        End While

        If iExistingGroup < g_MapWin.Layers.Groups.Count Then
            GroupHandle = g_MapWin.Layers.Groups.ItemByPosition(iExistingGroup).Handle
        Else
            'TODO: read group order from a file rather than hard-coded array
            Dim GroupOrder() As String = {"Data Layers", _
                                          "Hydrology", _
                                          "Observed Data Stations", _
                                          "Point Sources & Withdrawals", _
                                          "Political", _
                                          "Census", _
                                          "Transportation", _
                                          "Soil, Land Use/Cover", _
                                          "Other"}
            Dim iNewGroup As Integer = Array.IndexOf(GroupOrder, aGroupName)
            iExistingGroup = g_MapWin.Layers.Groups.Count
            If iNewGroup > 0 Then
                While iExistingGroup > 0 AndAlso _
                      Array.IndexOf(GroupOrder, _
                                    g_MapWin.Layers.Groups.ItemByPosition(iExistingGroup - 1).Text) < iNewGroup
                    iExistingGroup -= 1
                End While
            End If
            GroupHandle = g_MapWin.Layers.Groups.Add(aGroupName, iExistingGroup)
        End If

        Select Case aLay.LayerType
            Case MapWindow.Interfaces.eLayerType.Grid, MapWindow.Interfaces.eLayerType.Image, MapWindow.Interfaces.eLayerType.PolygonShapefile
                aLay.MoveTo(0, GroupHandle) 'move grid/image/polygon to bottom of group
            Case MapWindow.Interfaces.eLayerType.LineShapefile, MapWindow.Interfaces.eLayerType.PointShapefile
                aLay.MoveTo(99, GroupHandle) 'move line/point layer to top of group
            Case Else
                Logger.Dbg("AddLayerToGroup: Unexpected layer type: " & aLay.LayerType & " for layer " & aLay.Name)
                aLay.MoveTo(0, GroupHandle)
        End Select
    End Sub

    Private Sub SetLandUseColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme

        MWlay.Name &= " " & FilenameOnly(shpFile.Filename).Substring(2)
        colorScheme = New MapWinGIS.ShapefileColorScheme
        colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "lucode")
        If colorScheme.FieldIndex > 0 Then
            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Urban or Built-up Land"
            colorBreak.StartValue = 1
            colorBreak.EndValue = 19
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(120, 120, 120))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Agricultural Land"
            colorBreak.StartValue = 20
            colorBreak.EndValue = 29
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 255, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Rangeland"
            colorBreak.StartValue = 30
            colorBreak.EndValue = 39
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(146, 174, 47))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Forest Land"
            colorBreak.StartValue = 40
            colorBreak.EndValue = 49
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(161, 102, 50))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Water"
            colorBreak.StartValue = 50
            colorBreak.EndValue = 59
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 255))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Wetland"
            colorBreak.StartValue = 60
            colorBreak.EndValue = 69
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 209, 220))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Barren Land"
            colorBreak.StartValue = 70
            colorBreak.EndValue = 79
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(255, 255, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Tundra"
            colorBreak.StartValue = 80
            colorBreak.EndValue = 89
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(60, 105, 0))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            colorBreak = New MapWinGIS.ShapefileColorBreak
            colorBreak.Caption = "Perennial Snow or Ice"
            colorBreak.StartValue = 90
            colorBreak.EndValue = 99
            colorBreak.StartColor = System.Convert.ToUInt32(RGB(210, 210, 210))
            colorBreak.EndColor = colorBreak.StartColor
            colorScheme.Add(colorBreak)

            MWlay.ColoringScheme = colorScheme
            MWlay.DrawFill = True
            MWlay.LineOrPointSize = 0
            MWlay.OutlineColor = System.Drawing.Color.Black
        End If
    End Sub

    Private Sub SetDemColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme
        Dim minelev As Single
        Dim maxelev As Single
        Dim inc As Single
        Dim i As Integer

        colorScheme = New MapWinGIS.ShapefileColorScheme
        colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "elev_m")
        'determine min and max elevations
        minelev = 9999999
        maxelev = -9999999
        For i = 1 To shpFile.NumShapes
            If shpFile.CellValue(colorScheme.FieldIndex, i) > maxelev Then
                maxelev = shpFile.CellValue(colorScheme.FieldIndex, i)
            End If
            If shpFile.CellValue(colorScheme.FieldIndex, i) < minelev Then
                minelev = shpFile.CellValue(colorScheme.FieldIndex, i)
            End If
        Next i
        inc = (maxelev - minelev) / 10

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev) & " - " & CStr(minelev + inc)
        colorBreak.StartValue = minelev
        colorBreak.EndValue = minelev + inc
        colorBreak.StartColor = System.Convert.ToUInt32(32768)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + inc) & " - " & CStr(minelev + (2 * inc))
        colorBreak.StartValue = minelev + inc
        colorBreak.EndValue = minelev + (2 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(888090)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (2 * inc)) & " - " & CStr(minelev + (3 * inc))
        colorBreak.StartValue = minelev + (2 * inc)
        colorBreak.EndValue = minelev + (3 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(1743412)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (3 * inc)) & " - " & CStr(minelev + (4 * inc))
        colorBreak.StartValue = minelev + (3 * inc)
        colorBreak.EndValue = minelev + (4 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(2598734)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (4 * inc)) & " - " & CStr(minelev + (5 * inc))
        colorBreak.StartValue = minelev + (4 * inc)
        colorBreak.EndValue = minelev + (5 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(3454056)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (5 * inc)) & " - " & CStr(minelev + (6 * inc))
        colorBreak.StartValue = minelev + (5 * inc)
        colorBreak.EndValue = minelev + (6 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(4309378)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (6 * inc)) & " - " & CStr(minelev + (7 * inc))
        colorBreak.StartValue = minelev + (6 * inc)
        colorBreak.EndValue = minelev + (7 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(5164700)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (7 * inc)) & " - " & CStr(minelev + (8 * inc))
        colorBreak.StartValue = minelev + (7 * inc)
        colorBreak.EndValue = minelev + (8 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(6020022)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (8 * inc)) & " - " & CStr(minelev + (9 * inc))
        colorBreak.StartValue = minelev + (8 * inc)
        colorBreak.EndValue = minelev + (9 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(6875344)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        colorBreak = New MapWinGIS.ShapefileColorBreak
        colorBreak.Caption = CStr(minelev + (9 * inc)) & " - " & CStr(minelev + (10 * inc))
        colorBreak.StartValue = minelev + (9 * inc)
        colorBreak.EndValue = minelev + (10 * inc)
        colorBreak.StartColor = System.Convert.ToUInt32(7730666)
        colorBreak.EndColor = colorBreak.StartColor
        colorScheme.Add(colorBreak)

        MWlay.ColoringScheme = colorScheme
        MWlay.DrawFill = True
        MWlay.LineOrPointSize = 0
        MWlay.OutlineColor = System.Drawing.Color.Black

    End Sub

    Private Sub SetLandUseColorsGrid(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal g As MapWinGIS.Grid)
        Dim colorBreak As MapWinGIS.GridColorBreak
        Dim colorScheme As MapWinGIS.GridColorScheme

        colorScheme = New MapWinGIS.GridColorScheme
        colorScheme.LightSourceIntensity = 0

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Water"
        colorBreak.LowValue = 10
        colorBreak.HighValue = 12
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Urban"
        colorBreak.LowValue = 20
        colorBreak.HighValue = 23
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 0, 255))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Barren or Mining"
        colorBreak.LowValue = 30
        colorBreak.HighValue = 32
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 0, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Transitional"
        colorBreak.LowValue = 33
        colorBreak.HighValue = 33
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 128, 128))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Forest"
        colorBreak.LowValue = 40
        colorBreak.HighValue = 43
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 128, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Upland Shrub Land"
        colorBreak.LowValue = 50
        colorBreak.HighValue = 53
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 255, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Agriculture - Cropland"
        colorBreak.LowValue = 60
        colorBreak.HighValue = 61
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Grass Land"
        colorBreak.LowValue = 70
        colorBreak.HighValue = 71
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(128, 255, 128))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Agriculture - Cropland"
        colorBreak.LowValue = 80
        colorBreak.HighValue = 80
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Agriculture - Pasture"
        colorBreak.LowValue = 81
        colorBreak.HighValue = 81
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 128, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Agriculture - Cropland"
        colorBreak.LowValue = 82
        colorBreak.HighValue = 85
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        colorBreak = New MapWinGIS.GridColorBreak
        colorBreak.Caption = "Wetlands"
        colorBreak.LowValue = 90
        colorBreak.HighValue = 92
        colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 255, 255))
        colorBreak.HighColor = colorBreak.LowColor
        colorBreak.ColoringType = ColoringType.Random
        colorScheme.InsertBreak(colorBreak)

        MWlay.ColoringScheme = colorScheme

    End Sub

    Private Sub SetElevationGridColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal g As MapWinGIS.Grid)
        Dim colorScheme As MapWinGIS.GridColorScheme

        colorScheme = New MapWinGIS.GridColorScheme
        colorScheme.UsePredefined(g.Minimum, g.Maximum, PredefinedColorScheme.FallLeaves)
        MWlay.ColoringScheme = colorScheme
    End Sub

    Public Sub SetCensusRenderer(ByVal MWlay As MapWindow.Interfaces.Layer, Optional ByVal shpFile As MapWinGIS.Shapefile = Nothing)
        Dim fldCFCC As Integer
        Dim curCFCC As Integer
        Dim iShape As Integer
        Dim nShapes As Integer = MWlay.Shapes.NumShapes
        Dim shp As MapWindow.Interfaces.Shape

        If shpFile Is Nothing Then shpFile = MWlay.GetObject

        fldCFCC = ShpFieldNumFromName(shpFile, "CFCC")

        g_MapWin.View.LockMap() 'keep the map from updating during the loop.

        For iShape = 0 To nShapes - 1
            shp = MWlay.Shapes(iShape)
            curCFCC = shpFile.CellValue(fldCFCC, iShape).ToString.Substring(1)
            Select Case curCFCC
                Case 1, 11 To 18
                    'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(132, 0, 0)))
                    shp.LineOrPointSize = 2
                Case 2, 21 To 28
                    'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(0, 0, 0)))
                    shp.LineOrPointSize = 2
                Case 3, 31 To 38
                    'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(122, 122, 122)))
                    shp.LineOrPointSize = 2
                Case 4, 41 To 48, 63, 64
                    'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(166, 166, 166)))
                Case Else 'A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
                    'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(200, 200, 200)))
                    shp.LineStipple = MapWinGIS.tkLineStipple.lsDotted
            End Select
        Next

        g_MapWin.View.UnlockMap() 'let the map redraw again
    End Sub

    'Public Sub SetCensusRenderer(ByVal MWlay As MapWindow.Interfaces.Layer, Optional ByVal shpFile As MapWinGIS.Shapefile = Nothing)
    '    Dim fldCFCC As Integer
    '    Dim iShape As Integer

    '    If shpFile Is Nothing Then
    '        shpFile = New MapWinGIS.Shapefile
    '        shpFile.Open(MWlay.FileName)
    '    End If

    '    fldCFCC = ShpFieldNumFromName(shpFile, "CFCC")

    '    For iShape = 0 To MWlay.Shapes.NumShapes - 1
    '        Dim shp As MapWindow.Interfaces.Shape = MWlay.Shapes(iShape)
    '        Dim iCFCC As Integer = shpFile.CellValue(fldCFCC, iShape).ToString.Substring(1)
    '        Select Case iCFCC
    '            Case 1, 11 To 18
    '                'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(132, 0, 0)))
    '                shp.LineOrPointSize = 2
    '            Case 2, 21 To 28
    '                'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(0, 0, 0)))
    '                shp.LineOrPointSize = 2
    '            Case 3, 31 To 38
    '                'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(122, 122, 122)))
    '                shp.LineOrPointSize = 2
    '            Case 4, 41 To 48, 63, 64
    '                'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(166, 166, 166)))
    '            Case Else 'A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
    '                'shp.Color = System.Drawing.Color.FromArgb(System.Convert.ToInt32(RGB(200, 200, 200)))
    '                shp.LineStipple = MapWinGIS.tkLineStipple.lsDotted
    '        End Select
    '    Next
    'End Sub

    Private Sub SetCensusColors(ByVal MWlay As MapWindow.Interfaces.Layer, ByVal shpFile As MapWinGIS.Shapefile)
        Dim colorBreak As MapWinGIS.ShapefileColorBreak
        Dim colorScheme As MapWinGIS.ShapefileColorScheme
        Dim prefix As String = (MWlay.FileName.ToUpper.Chars(MWlay.FileName.Length - 5))

        MWlay.Name &= " " & Left(FilenameOnly(shpFile.Filename), 8)

        If MWlay.FileName.ToLower.EndsWith("_tgr_a.shp") Or _
           MWlay.FileName.ToLower.EndsWith("_tgr_p.shp") Then
            'Color the roads
            colorScheme = New MapWinGIS.ShapefileColorScheme
            colorScheme.FieldIndex = ShpFieldNumFromName(shpFile, "CFCC")
            If colorScheme.FieldIndex > 0 Then
                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Primary limited access"
                colorBreak.StartValue = prefix & "1"
                colorBreak.EndValue = prefix & "18"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(132, 0, 0))
                colorBreak.EndColor = colorBreak.StartColor
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Primary non-limited access"
                colorBreak.StartValue = prefix & "2"
                colorBreak.EndValue = prefix & "28"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(0, 0, 0))
                colorBreak.EndColor = colorBreak.StartColor
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Secondary"
                colorBreak.StartValue = prefix & "3"
                colorBreak.EndValue = prefix & "38"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(122, 122, 122))
                'TODO: renderer should be able to change line width: LineWidth = 2
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Local"
                colorBreak.StartValue = prefix & "4" 'TODO: A4, A41, A42, A43, A44, A45, A46, A47, A48, A63, A64
                colorBreak.EndValue = prefix & "48"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(166, 166, 166))
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                colorBreak = New MapWinGIS.ShapefileColorBreak
                colorBreak.Caption = "Other"
                colorBreak.StartValue = prefix & "5" 'TODO: A5, A51, A52, A53, A6, A60, A61, A62, A65, A7, A70, A71, A72, A73, A74
                colorBreak.EndValue = "Z"
                colorBreak.StartColor = System.Convert.ToUInt32(RGB(200, 200, 200))
                'TODO: renderer should be able to change line style: LineStyle = Dashed
                colorBreak.EndColor = colorBreak.StartColor
                colorScheme.Add(colorBreak)

                MWlay.ColoringScheme = colorScheme
                MWlay.LineOrPointSize = 1
                MWlay.OutlineColor = System.Drawing.Color.Black

                SetCensusRenderer(MWlay, shpFile)
            End If
        End If
    End Sub

    Private Function ShpFieldNumFromName(ByVal aShpFile As MapWinGIS.Shapefile, ByVal aFieldName As String) As Integer
        Dim lFieldName As String = LCase(aFieldName)
        Dim iField As Integer
        For iField = 0 To aShpFile.NumFields
            If LCase(aShpFile.Field(iField).Name) = lFieldName Then Return iField
        Next
        Return 0
    End Function

    Private Function GetDefaultsFor(ByVal Filename As String, _
                                    ByVal aProjectDir As String, _
                                    ByRef aDefaultsXml As Chilkat.Xml) As Chilkat.Xml
        Dim lName As String
        Dim layerXml As Chilkat.Xml

        lName = LCase(Filename.Substring(aProjectDir.Length))

        If Not aDefaultsXml Is Nothing Then
            For lLayer As Integer = 0 To aDefaultsXml.NumChildren - 1
                layerXml = aDefaultsXml.GetChild(lLayer)
                If PatternMatch(lName, "*" & layerXml.GetAttrValue("Filename") & "*") Then
                    Return layerXml
                End If
            Next
        End If
        Return Nothing
    End Function
End Module
