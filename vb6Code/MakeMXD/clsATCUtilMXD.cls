VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCUtilMXD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Dim pApp As Application
'Dim pIpc As ATCoIPC

Public Function StartMXD(aApp As Application) As Boolean
  Dim lDirective As String
  Dim lInstructionFileString As String
  Dim lFilename As String
  
  'Set pIpc = New ATCoIPC
  'pIpc.SendMonitorMessage "(OPEN StartMxd " & aApp.Document.title & ")"
  
  Dim buildmxdFilename As String
  Dim ff As ATCoFindFile
  Set ff = New ATCoFindFile
  ff.SetDialogProperties "Please Locate build.mxd", "\BASINS\etc\build.mxd"
  ff.SetRegistryInfo "FindFile", "FoundFiles", "build.mxd"
  buildmxdFilename = ff.GetName
  
  Set pApp = aApp
  
  Dim lLogFilename As String
  lLogFilename = PathNameOnly(buildmxdFilename) & "\ArcMapBuildLogger.txt"
  If FileExists(lLogFilename) Then
    lLogFilename = PathNameOnly(buildmxdFilename) & "\ArcMapOpenLogger.txt"
  End If
  Dim lLogunit As Integer
  lLogunit = FreeFile(0)
  Open lLogFilename For Output As lLogunit
  
  lFilename = PathNameOnly(buildmxdFilename) & "\ArcMapInstructions.txt"
  If FileExists(lFilename) Then
    lInstructionFileString = WholeFileString(lFilename)
    'pIpc.SendMonitorMessage "Instructions:" & lInstructionFileString
    Print #lLogunit, "Instructions:" & lInstructionFileString
  
    Do While Len(lInstructionFileString) > 0
      lDirective = StrSplit(lInstructionFileString, ",", """")
      'pIpc.SendMonitorMessage "Directive:" & lDirective
      Print #lLogunit, "DirectiveX:" & lDirective
      If UCase(lDirective) = "BUILD" Then
        Print #lLogunit, "In conditional build " & pApp.Document.title
        If UCase(pApp.Document.title) = "BUILD.MXD" Or UCase(pApp.Document.title) = "BUILD" Then
          'pApp.Visible = False
          Print #lLogunit, "About to BuildNewProject"
          BuildNewProject lInstructionFileString, lLogunit
        End If
      ElseIf UCase(lDirective) = "ARCHIVE" Then
        ArchiveProject lInstructionFileString
      ElseIf UCase(lDirective) = "RESTORE" Then
        RestoreProject lInstructionFileString
      End If
    Loop
  Else
    'pIpc.SendMonitorMessage "File Not Found:" & lFilename
    Print #lLogunit, "File Not Found:" & lFilename
  End If
  
  'pIpc.SendMonitorMessage "StartMxd done"
  Print #lLogunit, "StartMxd done"
  Close lLogunit
  pApp.Caption = "BASINS 4.0"
End Function

Private Sub BuildNewProject(aInstructions As String, aLogunit As Integer)

  Dim lMapWindowProjectName As String
  lMapWindowProjectName = StrSplit(aInstructions, ",", """")
  
  Dim lMW As New ChilkatXml
  lMW.LoadXmlFile lMapWindowProjectName
  
  Dim lOverwrite As Boolean
  Dim lProjectName As String
  lProjectName = FilenameNoExt(lMapWindowProjectName) & ".mxd"
  If FileExists(lProjectName) Then
    lOverwrite = MsgBox("Overwrite Existing " & lProjectName, vbYesNo, "Build BASINS Mxd")
  Else
    lOverwrite = True
  End If
  
  Dim lDoc As IMxDocument
  Dim lMap As IMap
  
  If lOverwrite Then
  
    Set lDoc = pApp.Document
    Set lMap = lDoc.FocusMap

    Dim lMWGroups As New ChilkatXml
    Set lMWGroups = lMW.FindChild("Groups")
  
    Dim lMWGroup As New ChilkatXml
    Dim lMWLayers As New ChilkatXml
    Dim lMWLayer As New ChilkatXml
    Dim lMWShapeProps As New ChilkatXml
    Dim i As Long
    Dim j As Long
    Dim layername As String
    Dim layerpath As String
    Dim layervisible As Boolean
    Dim LineRgb As Long
    Dim LineStyle As Long
    Dim LineSize As Long
    Dim FillRgb As Long
    Dim FillStyle As Long
    Dim MarkRgb As Long
    Dim MarkStyle As Long
    Dim MarkSize As Long
          
    For i = 0 To lMWGroups.NumChildren - 1
      Set lMWGroup = lMWGroups.GetChild(i)
      Set lMWLayers = lMWGroup.FindChild("Layers")
      For j = 0 To lMWLayers.NumChildren - 1
        Set lMWLayer = lMWLayers.GetChild(j)
        layername = lMWLayer.GetAttrValue("Name")
        layerpath = PathNameOnly(lProjectName) & "\" & lMWLayer.GetAttrValue("Path")
        layervisible = lMWLayer.GetAttrValue("Visible")
        If FileExt(layerpath) = "shp" Then
          Print #aLogunit, "adding layer " & layerpath & " " & layername
          AddShapefile lMap, layerpath, FilenameOnly(layerpath), layername, layervisible
          
          Set lMWShapeProps = lMWLayer.FindChild("ShapeFileProperties")

          'PointType = "0"
          'DrawFill = "False"
          'UseTransparency = "False"
          'TransparencyColor = "0"
          LineRgb = lMWShapeProps.GetAttrValue("OutLineColor")
          LineStyle = lMWShapeProps.GetAttrValue("LineStipple")
          LineSize = lMWShapeProps.GetAttrValue("LineOrPointSize")
          FillRgb = lMWShapeProps.GetAttrValue("Color")
          FillStyle = lMWShapeProps.GetAttrValue("FillStipple")
          If FillStyle = 0 And lMWShapeProps.GetAttrValue("DrawFill") = False Then
            FillStyle = 1
          End If
          MarkRgb = lMWShapeProps.GetAttrValue("Color")
          MarkStyle = 1
          MarkSize = lMWShapeProps.GetAttrValue("LineOrPointSize")
          
          SetThemeDisplay lMap, LineRgb, LineStyle, LineSize, FillRgb, FillStyle, MarkRgb, MarkStyle, MarkSize
        ElseIf FileExt(layerpath) = "tif" Then
          Print #aLogunit, "adding grid  " & layerpath & " " & layername
          AddGrid lMap, layerpath, FilenameOnly(layerpath), layername, layervisible
        End If
      Next j
    Next i
    
    lDoc.ActiveView.Refresh
    pApp.Visible = True
    pApp.SaveAsDocument lProjectName, True
    pApp.OpenDocument lProjectName
  End If

'      'TABLES
'      'longname = "STORET Agency Codes"
'      'tablename = "storetag.dbf"
'      'AddDBASEFile shapepath, tablename, longname
'
'      'following does not work!!!
'      Set lExtent = New clsExtent
'      If .Extents.Count = 0 Then
'        pIpc.SendMonitorMessage "Extent not available"
'      Else
'        pIpc.SendMonitorMessage "Setting Extent"
'        Set lExtent = .Extents(1)
'        With lDoc.ActiveView.Extent.Envelope
'          .xMin = lExtent.Left
'          .yMax = lExtent.Top
'          .Height = lExtent.Height
'          .Width = lExtent.Width
'          pIpc.SendMonitorMessage "ExtentDecodedX:" & .xMin & "," & .xMax & "," & .UpperLeft.X & "," & .Width
'          pIpc.SendMonitorMessage "ExtentDecodedY:" & .yMin & "," & .yMax & "," & .UpperRight.Y & "," & .Height
'        End With
'      End If
'    End With
'
End Sub

Private Sub AddShapefile(aMap As IMap, aShapePath As String, aShapeName As String, aLongName As String, aDisplay As Boolean)
  Dim lWorkspaceFactory As IWorkspaceFactory
  Dim lFeatureWorkspace As IFeatureWorkspace
  Dim lFeatureLayer As IFeatureLayer
  Dim lLayer As ILayer
  Dim lGFLayer As IGeoFeatureLayer

  'Create a new ShapefileWorkspaceFactory object and open a shapefile folder
  Set lWorkspaceFactory = New ShapefileWorkspaceFactory
  Set lFeatureWorkspace = lWorkspaceFactory.OpenFromFile(PathNameOnly(aShapePath), 0)
  'pIpc.SendMonitorMessage "FeatureWorkspaceOpened, addClass " & aShapeName
  'Create a new FeatureLayer and assign a shapefile to it
  Set lFeatureLayer = New FeatureLayer
  Set lFeatureLayer.FeatureClass = lFeatureWorkspace.OpenFeatureClass(aShapeName)
  'pIpc.SendMonitorMessage "FeatureClassOpened for " & aShapeName
  'pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName
  lFeatureLayer.Name = aLongName
  lFeatureLayer.Visible = aDisplay

  'Add the FeatureLayer to the focus map
  Set lLayer = lFeatureLayer
  aMap.AddLayer lLayer
End Sub

Private Sub AddGrid(aMap As IMap, aGridPath As String, aGridName As String, aLongName As String, aDisplay As Boolean)
  Dim lWorkspaceFactory As IWorkspaceFactory
  Dim lRasterWorkspace As IRasterWorkspace
  Dim lRasterLayer As IRasterLayer
  Dim lRasterDataset As IRasterDataset
  Dim lLayer As ILayer
  Dim lGFLayer As IGeoFeatureLayer

  'Create a new RasterWorkspaceFactory object and open a raster folder
  Set lWorkspaceFactory = New RasterWorkspaceFactory
  Set lRasterWorkspace = lWorkspaceFactory.OpenFromFile(PathNameOnly(aGridPath), 0)
  'pIpc.SendMonitorMessage "RasterWorkspaceOpened, addClass " & aGridName
  'Create a new RasterLayer and assign a raster to it
  'Set lRasterDataset = New RasterDataset
  'Set lRasterDataset = lRasterWorkspace.OpenRasterDataset(aGridName)
  Set lRasterLayer = New RasterLayer
  lRasterLayer.CreateFromFilePath aGridPath
  'pIpc.SendMonitorMessage "FeatureClassOpened for " & aGridName
  lRasterLayer.Name = aLongName
  lRasterLayer.Visible = aDisplay

  'Add the RasterLayer to the focus map
  Set lLayer = lRasterLayer
  aMap.AddLayer lLayer
End Sub

Private Sub AddDBASEFile(tablepath$, tableName$, longName$)
'  Dim pFact As IWorkspaceFactory
'  Dim pWorkspace As IWorkspace
'  Dim pFeatws As IFeatureWorkspace
'  Dim pTable As ITable
'
'  ' Get the ITable from the geodatabase
'  Set pFact = New ShapefileWorkspaceFactory
'  Set pWorkspace = pFact.OpenFromFile(tablepath, 0)
'  Set pFeatws = pWorkspace
'  Set pTable = pFeatws.OpenTable(tableName)
'
'  ' add the table
'  Add_Table_TOC pTable, longName

End Sub

'Private Sub Add_Table_TOC(pTable As ITable, longName$)
'
'  Dim pDoc As IMxDocument
'  Dim pMap As iMap
'
'  Set pDoc = pApp.Document
'  Set pMap = pDoc.FocusMap
'
'  ' Create a new standalone table and add it
'  ' to the collection of the focus map
'  Dim pStTab As IStandaloneTable
'  Set pStTab = New StandaloneTable
'  Set pStTab.Table = pTable
'  pStTab.Name = longName
'  Dim pStTabColl As IStandaloneTableCollection
'  Set pStTabColl = pMap
'  pStTabColl.AddStandaloneTable pStTab
'
'  ' Refresh the TOC
'  pDoc.UpdateContents
'
'End Sub

Private Sub ArchiveProject(aInstruction As String)
'  Dim pDoc As IMxDocument
'  Dim pMap As iMap
'  Dim pLayer As iLayer
'  Dim pFLayer As IFeatureLayer
'  Dim pFClass As IFeatureClass
'  Dim pData As IDataLayer
'  Dim pDataSet As IDataset
'  Dim pTable As ITable
'  Dim I As Long
'  Dim cname As String
'  Dim cpath As String
'  Dim lXml As ChilkatXml, lXmlMap As ChilkatXml, lXmlLayer As ChilkatXml
'  Dim lXmlLayerData As ChilkatXml
'
'  Set lXml = New ChilkatXml
'  Set lXmlMap = New ChilkatXml
'
'  Set pDoc = pApp.Document
'  Set pMap = pDoc.FocusMap 'how do we open the correct map (whose name is in the aInstruction)?
'
'  For I = 1 To pMap.LayerCount
'    Set pLayer = pMap.Layer(I - 1)
'    Set pFLayer = pLayer
'    Set pFClass = pFLayer.FeatureClass
'    Set pDataSet = pFClass
'    cpath = pDataSet.Workspace.PathName
'    cname = pFClass.AliasName
'    Set lXmlLayer = New ChilkatXml
'    lXmlLayer.Tag = "MapLayer"
'    lXmlMap.AddChildTree lXmlLayer
'  Next I
'
'  Dim pStTabColl As IStandaloneTableCollection
'  Set pStTabColl = pMap
'
'  For I = 1 To pStTabColl.StandaloneTableCount
'    Set pTable = pStTabColl.StandaloneTable(I - 1)
'    Set pDataSet = pTable
'    cpath = pDataSet.Workspace.PathName
'    cname = pDataSet.BrowseName
'    'need some xml here
'  Next I
'
'  lXml.AddChildTree lXmlMap
'  'extent ...
'
'  lXml.SaveXml aInstruction  'or the portion thats the xml name
'
'  pApp.Shutdown '???
'
End Sub

Private Sub RestoreProject(aInstruction As String)
  'not yet working, how is it different from Build?
'  Dim pDoc As IMxDocument
'  Dim pMap As iMap
'  Dim pMapAdmin2 As IMapAdmin2
'  Dim pLayer As iLayer
'  Dim pFLayer As IFeatureLayer
'  Dim pOldFC As IFeatureClass
'  Dim pDataSet As IDataset
'  Dim pWorkspaceFactory As IWorkspaceFactory
'  Dim pFWorkspace As IFeatureWorkspace
'  Dim pNewFC As IFeatureClass
'
'  Dim pData As IDataLayer
'  Dim pWorkspace As IWorkspace
'  Dim pTable As ITable
'
'  Dim I As Long, j As Long
'  Dim cname As String
'  Dim ctxt As String
'  Dim cpath As String
'  Dim oldpath As String
'
'  Dim fu As Long
'  'need to open file based on name in instruction
'
'  Set pDoc = pApp.Document
'  Set pMap = pDoc.FocusMap
'  Set pMapAdmin2 = pMap
'
'  For I = 1 To pMap.LayerCount
'    Line Input #fu, ctxt
'    Set pLayer = pMap.Layer(I - 1)
'    Set pFLayer = pLayer
'    Set pOldFC = pFLayer.FeatureClass
'    oldpath = ""
'    If Not pOldFC Is Nothing Then
'      'see what current path is
'      Set pDataSet = pOldFC
'      oldpath = pDataSet.Workspace.PathName
'      Set pOldFC = Nothing
'      Set pFLayer.FeatureClass = Nothing
'    End If
'    For j = Len(ctxt) To 1 Step -1
'      If Mid(ctxt, j, 1) = "\" Then Exit For
'    Next j
'    cpath = Mid(ctxt, 7, j - 6)
'    cname = Mid(ctxt, j + 1)
'    If oldpath <> cpath Then
'      'need to update, can assume shapefile???
'
'      'based on esri sample SetDataSource
'      Set pWorkspaceFactory = New ShapefileWorkspaceFactory
'      Set pFWorkspace = pWorkspaceFactory.OpenFromFile(cpath, 0)
'      Set pNewFC = pFWorkspace.OpenFeatureClass(cname)
'      Set pFLayer.FeatureClass = pNewFC
'      pMapAdmin2.FireChangeFeatureClass pOldFC, pNewFC
'
'    End If
'  Next I
'  pDoc.CurrentContentsView.Refresh 0
'
'  Dim pStTabColl As IStandaloneTableCollection
'  Set pStTabColl = pMap
'  'need to restore tables too
'  'For i = 1 To pStTabColl.StandaloneTableCount
'  '  Line Input #fu, ctxt
'  '  Set pTable = pStTabColl.StandaloneTable(i - 1)
'  '  Set pDataSet = pTable
'  '  For j = Len(ctxt) To 1
'  '    If Mid(ctxt, j, 1) = "\" Then Exit For
'  '  Next j
'  '  cpath = Mid(ctxt, 7, j - 7)
'  '  cname = Mid(ctxt, j)
'  '
'  '  pDataSet.Workspace.PathName = Mid(ctxt, 7, j - 7)
'  'Next i
'
'  Close fu
'  'pApp.SaveDocument
'  'pApp.Shutdown
End Sub
  
Private Sub SetThemeDisplay(aMap As IMap, aLineRgb As Long, aLineStyle As Long, aLineSize As Long, aFillRgb As Long, aFillStyle As Long, _
                            aMarkRgb As Long, aMarkStyle As Long, aMarkSize As Long)
  Dim lFeatureLayer As IFeatureLayer
  Dim lLayer As ILayer
  Dim lGFLayer As IGeoFeatureLayer
  Dim lLineColor As iColor, lFillColor As iColor, lMarkColor As iColor
  Dim lSimpleRenderer As ISimpleRenderer
  Dim lSimpleMarker As ISimpleMarkerSymbol
  Dim lPictureMarker As IPictureMarkerSymbol
  'Dim lMarkerSymbol As IMarkerSymbol
  Dim lFillS As ISimpleFillSymbol
  Dim lLineS As ISimpleLineSymbol

  'pIpc.SendMonitorMessage "In Set Theme Display"

  Set lLayer = aMap.Layer(0)
  Set lFeatureLayer = lLayer
  Set lGFLayer = lFeatureLayer
  Set lSimpleRenderer = lGFLayer.Renderer

  'set color of new layer
  If lFeatureLayer.FeatureClass.ShapeType = 1 Then 'point
    If aMarkStyle = 100 Then
      'pIpc.SendMonitorMessage "Set Theme Display for Marker Style" & CStr(aMarkStyle)
      'Set MarkerSymbol = lSimpleRenderer.Symbol
      'Set lPictureMarker = New PictureMarkerSymbol
      'pIpc.SendMonitorMessage "about to create picture marker"
      'Set lPictureMarker.Picture = aMarkerPicture
      'pIpc.SendMonitorMessage "picturemarker size " & CStr(lPictureMarker.Size)
      'lPictureMarker.Size = aMarkSize
      'pIpc.SendMonitorMessage "picturemarker size " & CStr(lPictureMarker.Size)
      'lPictureMarker.CreateMarkerSymbolFromFile 1, "C:\WINNT\SYSTEM32\Samples\BitMaps\del.bmp"
      'pIpc.SendMonitorMessage "Just created picture marker"
      'Set lSimpleRenderer.Symbol = lPictureMarker
    Else
      'pIpc.SendMonitorMessage "Set Theme Display for Marker Style" & CStr(aMarkStyle)
      Set lSimpleMarker = lSimpleRenderer.Symbol
      'lSimpleMarker.Style = aMarkStyle
      Set lMarkColor = lSimpleMarker.Color
      lMarkColor.rgb = aMarkRgb
      lSimpleMarker.Color = lMarkColor
      'lSimpleMarker.Size = aLineSize
    End If
  ElseIf lFeatureLayer.FeatureClass.ShapeType = 3 Then 'line
    Set lLineS = lSimpleRenderer.Symbol
    Set lLineColor = lLineS.Color
    lLineColor.rgb = aLineRgb
    lLineS.Color = lLineColor
    lLineS.Style = aLineStyle
    If aLineSize > 1# Then
      lLineS.Width = 2#
    Else
      lLineS.Width = aLineSize
    End If
  Else 'polygon
    Set lFillS = lSimpleRenderer.Symbol
    Set lLineS = lFillS.Outline
    Set lLineColor = lLineS.Color
    lLineColor.rgb = aLineRgb
    lLineS.Color = lLineColor
    lLineS.Style = aLineStyle
    If aLineSize > 1# Then
      lLineS.Width = 2#
    Else
      lLineS.Width = aLineSize
    End If
    lFillS.Outline = lLineS
    Set lFillColor = lFillS.Color
    lFillColor.rgb = aFillRgb
    lFillS.Color = lFillColor
    lFillS.Style = aFillStyle
  End If
End Sub


