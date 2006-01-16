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
  Public Function CreateNewProjectAndDownloadCoreDataInteractive(ByVal aThemeTag As String, ByVal aSelectedFeatures As ArrayList) As String
    Dim dataPath As String
    Dim DefaultProjectFileName As String
    Dim ProjectFileName As String
    Dim NoData As Boolean = False
    Dim defDirName As String
    Dim newDataDir As String
    'Dim aprStr As String
    'Dim wholeaprStr As String
    'Dim fdn As String
    Dim iSuffix As Integer
    Dim myProjection As String
    Dim cdlg As Windows.Forms.SaveFileDialog

StartOver:

    dataPath = g_BasinsDrives.Chars(0) & ":\Basins\data\"

    Select Case aSelectedFeatures.Count
      Case 0
        If NoData Then
          'Already came through here, don't ask again
        Else
          NoData = True
          If MsgBox("No features have been selected.  Do you wish to create a project with no data?", MsgBoxStyle.YesNo, "BASINS Data Extraction") = MsgBoxResult.No Then
            Return ""
          End If
        End If
        defDirName = "NewProject"
      Case 1
        defDirName = aSelectedFeatures(0)
      Case Else : defDirName = "Multiple"
    End Select

    If FileExists(dataPath & defDirName, True) Then 'Find a suffix that will make name unique
      iSuffix = 1
      Do
        iSuffix = iSuffix + 1
      Loop While FileExists(dataPath & defDirName & "-" & iSuffix, True)
      defDirName = defDirName & "-" & iSuffix
    End If

    DefaultProjectFileName = dataPath & defDirName & "\" & defDirName & ".mwprj"
    System.IO.Directory.CreateDirectory(PathNameOnly(DefaultProjectFileName))

    cdlg = New Windows.Forms.SaveFileDialog
    cdlg.Title = "Save new project as..."
    cdlg.CheckPathExists = False
    cdlg.FileName = DefaultProjectFileName
    If cdlg.ShowDialog() <> Windows.Forms.DialogResult.OK Then
      cdlg.FileName = "\"
      cdlg.Dispose()
      System.IO.Directory.Delete(PathNameOnly(DefaultProjectFileName), False) 'Cancelled save dialog
      Return ""
    Else
      ProjectFileName = cdlg.FileName
      newDataDir = PathNameOnly(ProjectFileName) & "\"

      'Make sure cdlg is not holding a reference to the file so we can delete the dir if needed
      cdlg.FileName = "\"
      cdlg.Dispose()

      'If the user did not choose the default folder or a subfolder of it
      If Not newDataDir.ToLower.StartsWith(PathNameOnly(DefaultProjectFileName).ToLower) Then
        'Remove speculatively created folder since they chose something else
        Try
          System.IO.Directory.Delete(PathNameOnly(DefaultProjectFileName), False) 'Cancelled save dialog
        Catch ex As Exception
          Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & PathNameOnly(DefaultProjectFileName) & vbCr & ex.Message)
        End Try
      End If

      'Check to see if chosen data dir already contains any files
      Dim numFiles As Long = System.IO.Directory.GetFiles(newDataDir).LongLength
      Dim numDirs As Long = System.IO.Directory.GetDirectories(newDataDir).LongLength
      If numFiles + numDirs > 0 Then
        Logger.Msg("The folder '" & newDataDir & "'" & vbCr _
               & "already contains " & numFiles & " files and " & numDirs & " folders." & vbCr _
               & "The folder must be empty before a new project can be created here.", "BASINS Build New")
        GoTo StartOver
      End If

      myProjection = atcProjector.Methods.AskUser
      If myProjection.Length = 0 Then 'Cancelled projection specification dialog
        Try 'remove already created data directory
          System.IO.Directory.Delete(PathNameOnly(DefaultProjectFileName), False)
        Catch ex As Exception
          Logger.Dbg("CreateNewProjectAndDownloadCoreDataInteractive: Could not delete " & PathNameOnly(DefaultProjectFileName) & vbCr & ex.Message)
        End Try
        Return ""
      Else
        SaveFileString(newDataDir & "prj.proj", myProjection) 'Side effect: makes data directory

        'TODO: test this with no area selected
        If NoData Then
          g_MapWin.Layers.Clear()
          g_MapWin.Project.Save(ProjectFileName)
          g_MapWin.Project.Modified = True
          g_MapWin.Project.Save(ProjectFileName)
          g_MapWin.Project.Modified = False
        Else
          'download and project core data
          CreateNewProjectAndDownloadCoreData(aThemeTag, aSelectedFeatures, dataPath, newDataDir, ProjectFileName)
        End If
        Return ProjectFileName
      End If
    End If
  End Function

  'Returns file name of new project or "" if not built
  Public Sub CreateNewProjectAndDownloadCoreData(ByVal aThemeTag As String, _
                                                 ByVal aSelectedFeatures As ArrayList, _
                                                 ByVal aDataPath As String, _
                                                 ByVal aNewDataDir As String, _
                                                 ByVal aProjectFileName As String)
    Dim downloadxml As String
    Dim iFeature As Integer
    Dim downloadFilename As String = aDataPath & "download.xml"
    Dim projectorFilename As String = aDataPath & "ATCProjector.xml"

    downloadxml = "<clsWebDataManager>" & vbCrLf & " <status_variables>" & vbCrLf & "  <download_type status=""set by " & XMLappName & """>BasinsInitialSetup</download_type>" & vbCrLf & "  <launched_by>" & XMLappName & "</launched_by>" & vbCrLf & "  <project_dir status=""set by " & XMLappName & """>" & aNewDataDir & "</project_dir>" & vbCrLf

    For iFeature = 0 To aSelectedFeatures.Count - 1
      downloadxml &= "  <" & aThemeTag & " status=""set by " & XMLappName & """>" & aSelectedFeatures(iFeature) & "</" & aThemeTag & ">" & vbCrLf
    Next

    downloadxml &= " </status_variables>" & vbCrLf & "</clsWebDataManager>" & vbCrLf

    If FileExists(projectorFilename) Then Kill(projectorFilename)

    SaveFileString(downloadFilename, downloadxml)
    If DataDownload(downloadFilename) Then 'Succeeded, as far as we know
      g_MapWin.Layers.Clear()
      g_MapWin.Project.Save(aProjectFileName)
      g_MapWin.Project.Modified = True
      ProcessProjectorFile(projectorFilename)
      AddAllShapesInDir(aNewDataDir, aNewDataDir)
      g_MapWin.Project.Save(aProjectFileName)
      g_MapWin.Project.Modified = False
    End If
  End Sub

  'Download new data for an existing project
  Public Sub DownloadNewData(ByRef project_dir As String)
    Dim downloadFilename As String = project_dir & "download.xml"
    Dim projectorFilename As String = PathNameOnly(project_dir.Substring(0, project_dir.Length - 1)) & "\ATCProjector.xml"
    SaveFileString(downloadFilename, "<clsWebDataManager>" & vbCrLf & " <status_variables>" & vbCrLf & "  <launched_by>" & XMLappName & "</launched_by>" & vbCrLf & "  <project_dir status=""set by " & XMLappName & """>" & project_dir & "</project_dir>" & vbCrLf & " </status_variables>" & vbCrLf & "</clsWebDataManager>")
    If FileExists(projectorFilename) Then Kill(projectorFilename)
    If DataDownload(downloadFilename) Then
      ProcessProjectorFile(projectorFilename)
    End If
    Kill(downloadFilename)
  End Sub

  Private Sub ProcessProjectorFile(ByVal aProjectorFilename As String)
    Dim lProjectorXML As New Chilkat.Xml
    Dim lProjectorNode As Chilkat.Xml
    Dim theInputDirName As String
    Dim theOutputDirName As String
    Dim theOutputFileName As String
    Dim InputFileList As New NameValueCollection
    Dim vFilename As Object
    Dim curFilename As String
    Dim project_dir As String
    Dim defaultsXML As Chilkat.Xml
    Dim success As Boolean
    Dim iproj As String
    Dim oproj As String
    Dim layername As String

    If Not FileExists(aProjectorFilename) Then
      Logger.Dbg("No new ATCProjector.xml to process")
    Else
      lProjectorXML.LoadXml(WholeFileString(aProjectorFilename))
      lProjectorNode = lProjectorXML.FirstChild
      While Not lProjectorNode Is Nothing
        Logger.Dbg("Processing XML: " & lProjectorNode.GetXml)
        Select Case LCase(lProjectorNode.Tag.ToLower)
          Case "add_shape"
            theOutputFileName = lProjectorNode.Content
            If defaultsXML Is Nothing Then defaultsXML = GetDefaultsXML()
            AddShapeToMW(theOutputFileName, GetDefaultsFor(theOutputFileName, project_dir, defaultsXML))
          Case "add_grid"
            theOutputFileName = lProjectorNode.Content
            If defaultsXML Is Nothing Then defaultsXML = GetDefaultsXML()
            AddGridToMW(theOutputFileName, GetDefaultsFor(theOutputFileName, project_dir, defaultsXML))
          Case "add_allshapes"
            theOutputFileName = lProjectorNode.Content
            AddAllShapesInDir(theOutputFileName, project_dir)
          Case "project_dir"
            project_dir = lProjectorNode.Content
            If Right(project_dir, 1) <> "\" Then project_dir &= "\"
          Case "convert_shape"
            theOutputFileName = lProjectorNode.GetAttrValue("output")
            curFilename = lProjectorNode.Content
            ShapeUtilMerge(curFilename, theOutputFileName, project_dir & "prj.proj")
          Case "convert_grid"
            theOutputFileName = lProjectorNode.GetAttrValue("output")
            curFilename = lProjectorNode.Content
            If FileExists(theOutputFileName) Then
              'remove output file
              System.IO.File.Delete(theOutputFileName)
            End If
            If InStr(theOutputFileName, "\nlcd\") > 0 Then
              'exception for nlcd data, already in albers
              iproj = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m"
            Else
              iproj = "+proj=longlat +datum=NAD83"
            End If
            oproj = WholeFileString(project_dir & "prj.proj")
            oproj = CleanUpUserProjString(oproj)
            If iproj = oproj Then
              System.IO.File.Copy(curFilename, theOutputFileName)
            Else
              'project it
              g_MapWin.StatusBar(1).Text = "Projecting Grid..."
              'g_MapWin.View.MapCursor = tkCursor.crsrWait
              g_MapWin.Refresh()
              DoEvents()
              success = MapWinX.SpatialReference.ProjectGrid(iproj, oproj, curFilename, theOutputFileName, True)
              g_MapWin.StatusBar(1).Text = ""
              If Not success Then
                Logger.Msg("Failed to project grid" & vbCrLf & MapWinX.Error.GetLastErrorMsg, "ProcessProjectorFile")
                System.IO.File.Copy(curFilename, theOutputFileName)
              End If
              'g_MapWin.View.MapCursor = tkCursor.crsrMapDefault
            End If
          Case "convert_dir"
              'loop through a directory, projecting all files in it
              theInputDirName = lProjectorNode.Content
              theOutputDirName = lProjectorNode.GetAttrValue("output")
              If theOutputDirName Is Nothing OrElse theOutputDirName.Length = 0 Then
                theOutputDirName = theInputDirName
              End If
              If Right(theOutputDirName, 1) <> "\" Then theOutputDirName &= "\"

              InputFileList.Clear()

              AddFilesInDir(InputFileList, theInputDirName, False, "*.shp")

              For Each vFilename In InputFileList
                curFilename = vFilename
                If (FileExt(curFilename) = "shp") Then
                  'this is a shapefile
                  theOutputFileName = theOutputDirName & FilenameNoPath(curFilename)
                  'change projection and merge
                  If (FileExists(theOutputFileName) And (InStr(1, theOutputFileName, "\landuse\") > 0)) Then
                    'if the output file exists and it is a landuse shape, dont bother
                  Else
                    ShapeUtilMerge(curFilename, theOutputFileName, project_dir & "prj.proj")
                  End If
                End If
              Next vFilename

          Case Else
            Logger.Msg("Cannot yet follow directive:" & vbCr & lProjectorNode.Tag, "ProcessProjectorFile")
        End Select

        If Not lProjectorNode.NextSibling2 Then lProjectorNode = Nothing
      End While
    End If
  End Sub

  Private Function CleanUpUserProjString(ByVal ctemp As String) As String
    Dim ipos As Integer
    Dim first As Boolean

    Do While Mid(ctemp, 1, 1) = "#"
      'eliminate comment lines at beginning
      ipos = InStr(ctemp, vbCrLf)
      ctemp = Mid(ctemp, ipos + 2)
    Loop
    first = True
    Do While InStr(ctemp, vbCrLf) > 0
      'strip out unneeded stuff
      ipos = InStr(ctemp, vbCrLf)
      If first Then
        ctemp = Mid(ctemp, ipos + 2)
        first = False
      Else
        ctemp = Mid(ctemp, 1, ipos - 1) & " " & Mid(ctemp, ipos + 2)
      End If
    Loop
    If InStr(ctemp, " end ") > 0 Then
      ctemp = Mid(ctemp, 1, InStr(ctemp, " end ") - 1)
    End If
    If Len(ctemp) > 0 Then
      If Mid(ctemp, 1, 9) = "+proj=dd " Then
        ctemp = "+proj=longlat " & Mid(ctemp, 10)
        ctemp = ctemp & " +datum=NAD83"
      Else
        ctemp = ctemp & " +datum=NAD83 +units=m"
      End If
    Else
      ctemp = "<none>"
    End If
    CleanUpUserProjString = ctemp
  End Function

  Private Sub ShapeUtilMerge(ByVal aCurFilename As String, ByVal aOutputFilename As String, ByVal aProjectionFilename As String)
    Dim exe As String = FindFile("Please locate ShapeUtil.exe", "\basins\etc\datadownload\ShapeUtil.exe")
    If exe.Length > 0 Then
      Dim layersDBF As String = GetSetting("ShapeMerge", "files", "layers.dbf")
      If Not FileExists(layersDBF) Then
        Logger.Dbg("Did not find layers.dbf in registry " & layersDBF)
        layersDBF = PathNameOnly(exe) & "\layers.dbf"
        If FileExists(layersDBF) Then
          Logger.Dbg("Saving layers.dbf location for ShapeUtil: " & layersDBF)
          SaveSetting("ShapeMerge", "files", "layers.dbf", layersDBF)
        Else
          Logger.Dbg("Did not find layers.dbf in same path as ShapeUtil " & layersDBF)
        End If
      Else
        Logger.Dbg("Found " & layersDBF)
      End If
      'LogCmd("MSG2 Merging " & aCurFilename)
      'LogCmd("MSG6 into " & aOutputFilename)
      Shell(exe & " """ & aOutputFilename & """ """ & aCurFilename & """ """ & aProjectionFilename & """", AppWinStyle.NormalNoFocus, True)
    Else
      Logger.Dbg("Failed to find ShapeUtil.exe for merging " & aCurFilename & " into " & aOutputFilename)
    End If
  End Sub

  Private Function DataDownload(ByRef aCommandLine As String) As Boolean
    Dim exe As String = FindFile("Please locate DataDownload.exe", "\Basins\etc\DataDownload\DataDownload.exe")
    If exe.Length > 0 Then
      If Shell(exe & " " & aCommandLine, AppWinStyle.NormalFocus, True) = 0 Then
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
    Dim defaultsXml As Chilkat.Xml
    Dim defaultsPath As String 'full file name of defaults XML
    defaultsPath = FindFile("Please Locate BasinsDefaultLayers.xml", "\basins\etc\BasinsDefaultLayers.xml")
    'defaultsPath = "E:\BASINS\etc\BasinsDefaultRenderers.xml"
    If FileExists(defaultsPath) Then
      defaultsXml = New Chilkat.Xml
      defaultsXml.LoadXmlFile(defaultsPath)
    End If
    Return defaultsXml
  End Function

  'Adds all shape files found in aPath to the current MapWindow project
  Public Sub AddAllShapesInDir(ByVal aPath As String, ByVal project_dir As String)
    Dim iLayer As Integer
    Dim Filename As String
    Dim allFiles As New NameValueCollection
    Dim defaultsXML As Chilkat.Xml = GetDefaultsXML()

    Logger.Dbg("AddAllShapesInDir: '" & aPath & "'")

    If Right(aPath, 1) <> "\" Then aPath = aPath & "\"
    AddFilesInDir(allFiles, aPath, True, "*.shp")

    For iLayer = 0 To allFiles.Count - 1
      Filename = allFiles.Item(iLayer)
      AddShapeToMW(Filename, GetDefaultsFor(Filename, project_dir, defaultsXML))
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
    For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
      MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(iLayer))
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
    Dim RGBcolor As Int32
    Dim RGBoutline As Int32

    'Don't add layer again if we already have it
    For iLayer As Integer = 0 To g_MapWin.Layers.NumLayers - 1
      MWlay = g_MapWin.Layers(g_MapWin.Layers.GetHandle(iLayer))
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
        'SetDEMGColors(MWlay, g)
      ElseIf LCase(aFilename).IndexOf("\ned\") > 0 Then
        'SetNEDColors(MWlay, g)
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
      'MWlay.Color = System.Drawing.Color.White
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

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Water"
    colorBreak.LowValue = 10
    colorBreak.HighValue = 12
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Urban"
    colorBreak.LowValue = 20
    colorBreak.HighValue = 23
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 0, 255))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Barren or Mining"
    colorBreak.LowValue = 30
    colorBreak.HighValue = 32
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 0, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Transitional"
    colorBreak.LowValue = 33
    colorBreak.HighValue = 33
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 128, 128))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Forest"
    colorBreak.LowValue = 40
    colorBreak.HighValue = 43
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 128, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Upland Shrub Land"
    colorBreak.LowValue = 50
    colorBreak.HighValue = 53
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 255, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Agriculture - Cropland"
    colorBreak.LowValue = 60
    colorBreak.HighValue = 61
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Grass Land"
    colorBreak.LowValue = 70
    colorBreak.HighValue = 71
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(128, 255, 128))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Agriculture - Cropland"
    colorBreak.LowValue = 80
    colorBreak.HighValue = 80
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Agriculture - Pasture"
    colorBreak.LowValue = 81
    colorBreak.HighValue = 81
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 128, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Agriculture - Cropland"
    colorBreak.LowValue = 82
    colorBreak.HighValue = 85
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

    colorBreak = New MapWinGIS.GridColorBreak
    colorBreak.Caption = "Wetlands"
    colorBreak.LowValue = 90
    colorBreak.HighValue = 92
    colorBreak.LowColor = System.Convert.ToUInt32(RGB(0, 255, 255))
    colorBreak.HighColor = colorBreak.LowColor
    colorScheme.InsertBreak(colorBreak)

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
                                  ByVal project_dir As String, _
                                  ByRef aDefaultsXml As Chilkat.Xml) As Chilkat.Xml
    Dim lName As String
    Dim layerXml As Chilkat.Xml

    lName = LCase(Filename.Substring(project_dir.Length))

    If Not aDefaultsXml Is Nothing Then
      For iLayer As Integer = 0 To aDefaultsXml.NumChildren - 1
        layerXml = aDefaultsXml.GetChild(iLayer)
        If PatternMatch(lName, "*" & layerXml.GetAttrValue("Filename") & "*") Then
          Return layerXml
        End If
      Next
    End If
    Return Nothing
  End Function
End Module
