Attribute VB_Name = "ShapeBrowse"
Option Explicit

Public metaDataString$()
Public dispTypes$()
Public Const noDetailsMsg = "No Details Available"
Private Type FileInfo
   modDate As Date
   recCount As Long
   FileType As FILETYPEENUM
   extension As String
   FileLength As Double
   version As String
   xMin As Double
   xMax As Double
   yMin As Double
   yMax As Double
   zMin As Double
   zMax As Double
   mMin As Double
   mMax As Double
End Type

Private fi() As FileInfo
Private colType&()
Private colName$()

'Must be called once at start of program run
Public Sub InitColName()
  ReDim colName(0 To 13)
  colName(0) = "Name"
  colName(1) = "Type"
  colName(2) = "Records"
  colName(3) = "Version"
  colName(4) = "Bytes"
  colName(5) = "X Min"
  colName(6) = "X Max"
  colName(7) = "Y Min"
  colName(8) = "Y Max"
  colName(9) = "Z Min"
  colName(10) = "Z Max"
  colName(11) = "M Min"
  colName(12) = "M Max"
  colName(13) = "Date"
  
  ReDim colType(0 To 4)
  colType(0) = 0
  colType(1) = 1
  colType(2) = 2
  colType(3) = 4
  colType(4) = 13
  
End Sub

Public Sub SetShapeBrowseCols(newCols&())
  Dim C As Long, minNewCol As Long, maxNewCol As Long
  
  minNewCol = LBound(newCols)
  maxNewCol = UBound(newCols)
  ReDim colType(0 To maxNewCol - minNewCol)
  For C = minNewCol To maxNewCol
    colType(C - minNewCol) = newCols(C)
  Next C
End Sub

Public Sub OpenDirectory(fileBaseDir$, t As TreeView)
  Dim nod As Node
  
  If InStr(fileBaseDir, ":") Then
    ChDrive (Left(fileBaseDir, 2))
  End If
  ChDir (fileBaseDir)
  
  t.Visible = False
  t.Nodes.Clear
  
  ReDim fi(0)
  Set nod = t.Nodes.Add(, , fileBaseDir, fileBaseDir)
  nod.Image = "Folder"
  moveThruDir nod.Text, t
  add2tree fileBaseDir, t
  
  t.Visible = True
  t.Nodes(1).EnsureVisible

End Sub

Public Sub SetDirectoryHeaders(agd As ATCoGrid)
  Dim C&
  With agd
    C = UBound(colType) - LBound(colType) + 1
    If .cols <> C Then .cols = C 'Not doing this If resizes the cols every time
    For C = LBound(colType) To UBound(colType)
      .ColTitle(C) = colName(colType(C))
    Next C
  End With
End Sub

Public Sub DisplayDirectoryDetails(t As TreeView, nod As Node, agd As ATCoGrid)
  Dim n&, chnode As Node
  Dim row&
  
  SetDirectoryHeaders agd
  With agd
    '.header = nod.FullPath
    .rows = 50
    row = 1
    For n = 2 To t.Nodes.count
      Set chnode = t.Nodes(n)
      If chnode.Parent.FullPath = nod.FullPath Then
        If chnode.Image <> "Folder" Then
          DisplayDirectoryRow row, chnode, agd
          row = row + 1
        End If
      End If
    Next n
    .rows = .MaxOccupiedRow
  End With

End Sub

Public Sub DisplayDirectoryRow(r&, chnode As Node, agd As ATCoGrid)
  Dim C&, txt$, val#, lsi As FileInfo
  lsi = fi(chnode.Tag)
  For C = LBound(colType) To UBound(colType)
    val = NONE
    txt = ""
    Select Case colType(C)
      Case 0: txt = chnode.Text
      Case 1:
        If lsi.FileType = typeUnknown Then
          txt = lsi.extension
        Else
          txt = FileTypeName(lsi.FileType)
        End If
      Case 2: val = lsi.recCount
      Case 3: val = lsi.version
      Case 4: val = lsi.FileLength
      Case 5: val = lsi.xMin
      Case 6: val = lsi.xMax
      Case 7: val = lsi.yMin
      Case 8: val = lsi.yMax
      Case 9: val = lsi.zMin
      Case 10: val = lsi.zMax
      Case 11: val = lsi.mMin
      Case 12: val = lsi.mMax
      Case 13: txt = lsi.modDate
    End Select
    If val <> NONE Then txt = val
    agd.TextMatrix(r, C) = txt
  Next C
End Sub

Public Sub DisplayText(nod As Node)
  If fi(nod.Tag).FileType <= typeMultiPatch Then
    frmText.txt.Text = ShapeDump(nod.FullPath)
  ElseIf fi(nod.Tag).FileType = typeDBF Then
    Dim lDBF As clsDBF
    Set lDBF = New clsDBF
    lDBF.OpenDBF nod.FullPath
    frmText.txt.Text = lDBF.Summary
    lDBF.Clear
  Else
    'Case "txt": frmText.txt.Text = WholeFileString(nod.FullPath)
    frmText.txt.Text = ""
  End If
End Sub

Public Sub DisplayData(nod As Node, agd As ATCoGrid, li As MapLayerInfo, nullString$)
  With agd
    Dim r&, C&, dbCol&
    Dim inFile&, buf As String
    Dim baseFilename As String
    Dim openFilename As String
    Dim dbfName As String
    Dim dbf As clsDBF
    'Static running As Boolean
    'Static pending As Boolean
    'If pending Then Exit Sub
    'If running Then
    '  pending = True
    '  Debug.Print "pending = True"
    'End If
    'running = True
    'Debug.Print "running = True"
    .ColTitle(0) = "Details"
    If fi(nod.Tag).FileType <= typeDBF Then 'shape file of some sort
      .TextMatrix(1, 0) = fi(nod.Tag).recCount & " " & FileTypeName(fi(nod.Tag).FileType) & "s"
      baseFilename = nod.FullPath
      dbfName = baseFilename & ".dbf"
      If Len(Dir(dbfName)) > 0 Then
        On Error GoTo ErrOpenDBF
        Set dbf = New clsDBF
        dbf.OpenDBF dbfName
        .rows = 100
        .cols = dbf.numFields
        For C = 0 To .cols - 1
          .ColTitle(C) = dbf.FieldName(C + 1)
          If dbf.FieldType(C + 1) = "N" Then
            If dbf.FieldDecimalCount(C + 1) > 0 Then
              .colType(C) = ATCoSng
            Else
              .colType(C) = ATCoInt
            End If
          Else
              .colType(C) = ATCoTxt
          End If
        Next C
        r = 0
        For r = 1 To dbf.NumRecords
          dbf.CurrentRecord = r
          If r > .rows Then .rows = r + 50 'over-allocate because changing .rows is expensive
          For C = 0 To .cols - 1
            .TextMatrix(r, C) = dbf.Value(C + 1)
          Next C
        Next
      Else
        .TextMatrix(2, 0) = "No DBF file"
      End If
    Else
      Select Case fi(nod.Tag).FileType
        Case typeWDM
          .TextMatrix(1, 0) = FileTypeName(fi(nod.Tag).FileType) & " file"
        Case typeUCI
          openFilename = nod.FullPath & ".uci"
          If Len(Dir(openFilename)) > 0 Then
            .cols = 1
            inFile = FreeFile(0)
            Open openFilename For Input As inFile
            r = 1
            While Not EOF(inFile)
              Line Input #inFile, buf
              If r > agd.rows Then agd.rows = agd.rows + 100
              agd.TextMatrix(r, 0) = buf
              r = r + 1
            Wend
            agd.rows = r - 1
          End If
        Case Else
          .TextMatrix(1, 0) = "Can't read this file  (" & FileTypeName(fi(nod.Tag).FileType) & ")"
      End Select
    End If
    .Visible = True
  End With
exitsub:
  On Error Resume Next
  dbf.Clear
  Set dbf = Nothing
  'running = False
  Debug.Print "running = False"
  agd.rows = agd.MaxOccupiedRow
  Exit Sub
  
ErrOpenDBF:
  agd.TextMatrix(2, 0) = "Error opening DBF: "
  agd.TextMatrix(3, 0) = Err.Description
  GoTo exitsub
End Sub

Private Sub InitBBox(Box As ShapeDefines.T_BoundingBox)
  With Box
    .xMin = 1E+300
    .yMin = 1E+300
    .xMax = -1E+300
    .yMax = -1E+300
  End With
End Sub

Private Sub updateBBox(x#, y#, Box As ShapeDefines.T_BoundingBox)
  If x < Box.xMin Then Box.xMin = x
  If x > Box.xMax Then Box.xMax = x
  If y < Box.yMin Then Box.yMin = y
  If y > Box.yMax Then Box.yMax = y
End Sub

Public Sub ConvertToPoints(nod As Node, justEndpoints As Boolean)
  Dim x#, y#
  Dim minx#, maxx#, miny#, maxy#
  Dim rec&, pt&, lsi As FileInfo
  Dim baseFilename$
  Dim ShapeIn As New CShape_IO
  Dim ShapeOut As New CShape_IO
  Dim XYPoint As ShapeDefines.T_shpXYPoint
  Dim XYMultiPoint As ShapeDefines.T_shpXYMultiPoint
  Dim PolyLine As ShapeDefines.T_shpPolyLine
  Dim Polygon As ShapeDefines.T_shpPolygon
  baseFilename = nod.FullPath
  lsi = fi(nod.Tag)
  
  ShapeIn.ShapeFileOpen baseFilename & ".shp", ReadOnly
  
  If justEndpoints Then
    ShapeOut.CreateNewShape baseFilename & "Points.shp", typePoint
    XYPoint.ShapeType = typePoint
  Else
    ShapeOut.CreateNewShape baseFilename & "Points.shp", typeMultiPoint
    XYMultiPoint.ShapeType = typeMultiPoint
  End If
  For rec = 1 To lsi.recCount
    Select Case lsi.FileType
      Case typePoint
      Case typeMultiPoint
      Case typepolyline
        PolyLine = ShapeIn.getPolyLine(rec)
        If justEndpoints Then
          For pt = 1 To PolyLine.NumParts - 1 'change to start at part 0 to get both ends of each line
            XYPoint.thePoint = PolyLine.thePoints(PolyLine.Parts(pt))
            ShapeOut.putXYPoint 0, XYPoint
          Next pt
          XYPoint.thePoint = PolyLine.thePoints(PolyLine.NumPoints - 1)
          ShapeOut.putXYPoint 0, XYPoint
        Else
          XYMultiPoint.Box = PolyLine.Box
          XYMultiPoint.NumPoints = PolyLine.NumPoints
          ReDim XYMultiPoint.thePoints(0 To XYMultiPoint.NumPoints - 1)
          For pt = 0 To PolyLine.NumPoints - 1
            XYMultiPoint.thePoints(pt) = PolyLine.thePoints(pt)
          Next pt
          ShapeOut.putXYMultiPoint 0, XYMultiPoint
        End If
      Case typepolygon
        Polygon = ShapeIn.getPolygon(rec)
        If justEndpoints Then
          For pt = 0 To Polygon.NumParts - 1
            XYPoint.thePoint = Polygon.thePoints(Polygon.Parts(pt))
            ShapeOut.putXYPoint 0, XYPoint
          Next pt
          XYPoint.thePoint = Polygon.thePoints(Polygon.NumPoints - 1)
          ShapeOut.putXYPoint 0, XYPoint
        Else
          XYMultiPoint.Box = Polygon.Box
          XYMultiPoint.NumPoints = Polygon.NumPoints
          ReDim XYMultiPoint.thePoints(0 To XYMultiPoint.NumPoints - 1)
          For pt = 0 To Polygon.NumPoints - 1
            XYMultiPoint.thePoints(pt) = Polygon.thePoints(pt)
          Next pt
          ShapeOut.putXYMultiPoint 0, XYMultiPoint
        End If
    End Select
  Next rec
  ShapeIn.FileShutDown
  ShapeOut.FileShutDown

  'copy file header from original shape file to get proper min/max
'  Dim shpfile%, shxfile%, RecordNumber& ', c&
'  Dim FileLength&, ShapeType&, lowX#, lowY#, uppX#, uppY#
'  shpfile = FreeFile(0)
    
'  Open baseFilename & ".shp" For Binary Access Read As shpfile
'  Call ReadShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
'  Close shpfile
'  ShapeType = typeMultiPoint
'  Open baseFilename & "Points.shp" For Binary Access Read Write As shpfile
'  Call WriteShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
'  Close shpfile
  
  'Shape Index file (.shx)
'  shxfile = FreeFile(0)
'  Open baseFilename & "Points.shx" For Binary Access Read Write As shxfile
'  Call WriteShapeHeader(shxfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
'  Close shxfile

End Sub
'conversion = 0:None
'             1:Albers to Lat/Long
'             2:Lat/Long to Albers
'             3:Mass. State Plane to Lat/Long
'             4:Lat/Long to Mass. State Plane
Public Sub ConvertFromTo(nod As Node, ByVal conversion&)
  Dim x#, y#, newx#, newy#
  'Dim minx#, maxx#, miny#, maxy#
  Dim rec&, pt&, lsi As FileInfo
  Dim baseFilename$
  Dim ShapeIn As New CShape_IO
  Dim ShapeOut As New CShape_IO
  Dim XYPoint As ShapeDefines.T_shpXYPoint
  Dim XYMultiPoint As ShapeDefines.T_shpXYMultiPoint
  Dim PolyLine As ShapeDefines.T_shpPolyLine
  Dim Polygon As ShapeDefines.T_shpPolygon
  baseFilename = nod.FullPath
  lsi = fi(nod.Tag)
  
  'We don't use these min and max yet because I don't know how to update the bounding box in the header
  'minx = 1E+30
  'maxx = -1E+30
  'miny = 1E+30
  'maxy = -1E+30

  ShapeIn.ShapeFileOpen baseFilename & ".shp", ReadOnly
  ShapeOut.CreateNewShape baseFilename & "LatLon.shp", lsi.FileType
  'ShapeOut.ShapeFileOpen baseFilename & "LatLon.shp", Readwrite
  For rec = 1 To lsi.recCount
    Select Case lsi.FileType
      Case typePoint
        XYPoint = ShapeIn.getXYPoint(rec)
        x = XYPoint.thePoint.x
        y = XYPoint.thePoint.y
        
        Select Case conversion
          Case 1: AL_2_LL x, y, -96, newy, newx
          Case 2: LL_2_AL y, x, -96, newx, newy
          Case 3: MASP_2_LL x, y, newy, newx
          Case 4: LL_2_MASP y, x, newx, newy
        End Select
        'GoSub updateMinMax
        
        XYPoint.thePoint.x = newx
        XYPoint.thePoint.y = newy
        ShapeOut.putXYPoint 0, XYPoint 'record number 0 means append
      Case typeMultiPoint
        XYMultiPoint = ShapeIn.getXYMultiPoint(rec)
        InitBBox XYMultiPoint.Box
        For pt = 0 To XYMultiPoint.NumPoints - 1
          x = XYMultiPoint.thePoints(pt).x
          y = XYMultiPoint.thePoints(pt).y
          
          Select Case conversion
            Case 1: AL_2_LL x, y, -96, newy, newx
            Case 2: LL_2_AL y, x, -96, newx, newy
            Case 3: MASP_2_LL x, y, newy, newx
            Case 4: LL_2_MASP y, x, newx, newy
          End Select
          updateBBox newx, newy, XYMultiPoint.Box
          'GoSub updateMinMax
          
          XYMultiPoint.thePoints(pt).x = newx
          XYMultiPoint.thePoints(pt).y = newy
        Next pt
        ShapeOut.putXYMultiPoint 0, XYMultiPoint
      Case typepolyline
        PolyLine = ShapeIn.getPolyLine(rec)
        InitBBox PolyLine.Box
        For pt = 0 To PolyLine.NumPoints - 1
          x = PolyLine.thePoints(pt).x
          y = PolyLine.thePoints(pt).y
          
          Select Case conversion
            Case 1: AL_2_LL x, y, -96, newy, newx
            Case 2: LL_2_AL y, x, -96, newx, newy
            Case 3: MASP_2_LL x, y, newy, newx
            Case 4: LL_2_MASP y, x, newx, newy
          End Select
          updateBBox newx, newy, PolyLine.Box
          'GoSub updateMinMax
          
          PolyLine.thePoints(pt).x = newx
          PolyLine.thePoints(pt).y = newy
        Next pt
        ShapeOut.putPolyLine 0, PolyLine
      Case typepolygon
        Polygon = ShapeIn.getPolygon(rec)
        InitBBox Polygon.Box
        For pt = 0 To Polygon.NumPoints - 1
          x = Polygon.thePoints(pt).x
          y = Polygon.thePoints(pt).y
          
          Select Case conversion
            Case 1: AL_2_LL x, y, -96, newy, newx
            Case 2: LL_2_AL y, x, -96, newx, newy
            Case 3: MASP_2_LL x, y, newy, newx
            Case 4: LL_2_MASP y, x, newx, newy
          End Select
          updateBBox newx, newy, Polygon.Box
          'GoSub updateMinMax
          
          Polygon.thePoints(pt).x = newx
          Polygon.thePoints(pt).y = newy
        Next pt
        ShapeOut.putPolygon 0, Polygon
        
    End Select
  Next rec
  ShapeIn.FileShutDown
  ShapeOut.FileShutDown
'  Exit Sub
'updateMinMax:
'  If x > maxx Then maxx = x
'  If x < minx Then minx = x
'  If y > maxy Then maxy = y
'  If y < miny Then miny = y
'  Return
End Sub

Public Sub DisplayDataHeaders(nod As Node, agd As ATCoGrid)
  With agd
    Dim r&
    Dim baseFilename$
    Dim dbfName$
    Dim dbf As Recordset
    
    .ColTitle(0) = noDetailsMsg
    baseFilename = nod.FullPath
    Select Case fi(nod.Tag).FileType
    Case typeUCI, typeWDM:
      .TextMatrix(1, 0) = "Can't read this type of file"
    Case Else
      dbfName = baseFilename & ".dbf"
      If Len(Dir(dbfName)) > 0 Then
        On Error GoTo ErrOpenDBF
        Set dbf = OpenDBF(dbfName, True)
        .rows = dbf.Fields.count
        .cols = 2
        .ColTitle(0) = "Field in DBF"
        .ColTitle(1) = "Data Type"
        For r = 1 To .rows
          .TextMatrix(r, 0) = dbf.Fields(r - 1).Name
          .TextMatrix(r, 1) = DataTypeName(dbf.Fields(r - 1).Type)
        Next r
      Else
        .TextMatrix(1, 0) = "No DBF file"
      End If
    End Select
    .Visible = True
  End With
  Exit Sub
  
ErrOpenDBF:
  agd.TextMatrix(2, 0) = "Error opening DBF: "
  agd.TextMatrix(3, 0) = Err.Description
End Sub

Public Sub DisplayMetadata(t As TreeView, tryFilename$, cdlg As CommonDialog)
  Dim baseFilename$, metaFilename$, dotpos&
  On Error GoTo NeverMind
  t.Nodes.Clear
  ReDim metaDataString(1000)
  If Len(tryFilename) = 0 Then GoTo AskForFilename
  cdlg.Filename = tryFilename
  If Len(Dir(tryFilename)) = 0 Then
AskForFilename:
    'cdlg.FileTitle = "Open Metadata File"
    cdlg.ShowOpen
  End If
  metaFilename = cdlg.Filename
  If Len(Dir(metaFilename)) > 0 Then
    Dim inFile%
    Dim lineBuf$, lenBuf&
    Dim noSpace$, lenNoSpace&
    Dim delimPos&, spacePos&
    Dim nod As Node
    Dim nodeName$, nodeNumber&
    Dim metaLevelName$(), metaLevelOffset%()
    Dim parentName$, metaLevel&, lastLevel&, nLevels&, lev&, curOffset%
    Dim n As Node
    nLevels = 0
    lastLevel = -1
    ReDim metaLevelName(80), metaLevelOffset(-1 To 80)
    metaLevelOffset(-1) = -1
    inFile = FreeFile()
    Open metaFilename For Input As inFile
    t.Nodes.Add , , metaFilename, metaFilename
    While Not EOF(inFile)
      Line Input #inFile, lineBuf
      delimPos = InStr(lineBuf, ":")
      If delimPos < 1 Then GoTo NotNode
      'if the colon is part of a URL (i.e. http:// or ftp://), it is not important
      If Mid(lineBuf, delimPos + 1, 2) = "//" Then GoTo NotNode
      noSpace = LTrim(lineBuf)
      lenBuf = Len(lineBuf)
      lenNoSpace = Len(noSpace)
      spacePos = InStr(lenBuf - lenNoSpace + 1, lineBuf, " ")
      'if there is a space in the line after some text but before the colon, the colon is important
      If spacePos > 0 And spacePos < delimPos Then GoTo NotNode
      curOffset = (lenBuf - lenNoSpace)
      If curOffset > metaLevelOffset(lastLevel) Then
        metaLevel = lastLevel + 1
        metaLevelOffset(metaLevel) = curOffset
      ElseIf curOffset < metaLevelOffset(lastLevel) Then
        metaLevel = lastLevel
        While curOffset < metaLevelOffset(metaLevel)
          metaLevel = metaLevel - 1
        Wend
      End If
      parentName = metaFilename
      For lev = 0 To metaLevel - 1
        parentName = parentName & metaLevelName(lev)
      Next lev
      If delimPos = lenNoSpace Then noSpace = Left(noSpace, lenNoSpace - 1)
      nodeNumber = 1
      nodeName = noSpace
      If t.Nodes(parentName).Children > 0 Then
        Set n = t.Nodes(parentName).Child
        Do
          If parentName & nodeName = n.Key Then
            nodeNumber = nodeNumber + 1
            nodeName = noSpace & "(" & nodeNumber & ")"
          End If
          If n = n.LastSibling Then
            Exit Do
          Else
            Set n = n.Next
          End If
        Loop
      End If
      metaLevelName(metaLevel) = nodeName
      t.Nodes.Add parentName, tvwChild, parentName & nodeName, nodeName
      On Error GoTo NeverMind
      If metaLevel < 1 Then t.Nodes(t.Nodes.count).EnsureVisible
      lastLevel = metaLevel
      If UBound(metaDataString) <= t.Nodes.count Then
        ReDim Preserve metaDataString(t.Nodes.count + 100)
      End If
NotNode:
      metaDataString(t.Nodes.count) = metaDataString(t.Nodes.count) & lineBuf & vbCrLf
    Wend
    Close inFile
  End If

NeverMind:

End Sub

Public Sub EditColumns(agdDetails As ATCoGrid, t As TreeView)
  Dim col&, rcol&, minNameCol&, maxNameCol&, minTypeCol&, maxTypeCol&
  minNameCol = LBound(colName)
  maxNameCol = UBound(colName)
  minTypeCol = LBound(colType)
  maxTypeCol = UBound(colType)
  With frmEditCols
    .Hide
    .asl.ClearLeft
    .asl.ClearRight
    For col = minNameCol To maxNameCol
      .asl.LeftItem(col - minNameCol) = colName(col)
      .asl.LeftItemData(col - minNameCol) = col
    Next col
    For rcol = minTypeCol To maxTypeCol
      col = minNameCol
      While .asl.LeftItemData(col) <> colType(rcol)
        col = col + 1
      Wend
      .asl.MoveRight col
    Next rcol
    .Show
  End With
End Sub

Private Sub getFileInfo(baseFilename$, ext$, lsi As FileInfo)
  Dim ShapeStuff As New CShape_IO                     'object var for instance to CShape_IO
  Dim ShpfileHeader As ShapeDefines.T_MainFileHeader  'Var to hold the shape header
  Dim result As Long                                  'for boolean func returns
                                                      'in the shapefile
  On Error GoTo NeverMind
  lsi.recCount = NONE
  lsi.version = NONE
  lsi.xMin = NONE
  lsi.xMax = NONE
  lsi.yMin = NONE
  lsi.yMax = NONE
  lsi.mMin = NONE
  lsi.mMax = NONE
  lsi.modDate = FileDateTime(baseFilename & "." & ext)
  lsi.FileLength = FileLen(baseFilename & "." & ext)
  lsi.extension = ext
  lsi.FileType = typeUnknown
  Select Case ext
    Case "dbf":  lsi.FileType = typeDBF
      Dim lDBF As clsDBF
      Set lDBF = New clsDBF
      lDBF.OpenDBF baseFilename & "." & ext
      lsi.recCount = lDBF.NumRecords
      Set lDBF = Nothing
    Case "txt":  lsi.FileType = typeTXT
    Case "rtf":  lsi.FileType = typeRTF
    Case "rdb":  lsi.FileType = typeRDB
    Case "wdm": lsi.FileType = typeWDM
                lsi.recCount = lsi.FileLength / 2048
    Case "uci": lsi.FileType = typeUCI
                lsi.recCount = CountString(WholeFileString(baseFilename & "." & ext), vbCr)
    Case "shp":
      result = ShapeStuff.ShapeFileOpen(baseFilename & ".shp", ReadOnly)
      lsi.recCount = ShapeStuff.getRecordCount
      ShpfileHeader = ShapeStuff.getShapeHeader
      lsi.FileType = ShpfileHeader.ShapeType
      lsi.FileLength = ShpfileHeader.FileLength
      lsi.version = ShpfileHeader.version
      lsi.xMin = ShpfileHeader.BndBoxXmin
      lsi.xMax = ShpfileHeader.BndBoxXmax
      lsi.yMin = ShpfileHeader.BndBoxYmin
      lsi.yMax = ShpfileHeader.BndBoxYmax
      lsi.mMin = ShpfileHeader.BndBoxMmin
      lsi.mMax = ShpfileHeader.BndBoxMmax
      ShapeStuff.FileShutDown
  End Select
NeverMind:
End Sub

Private Sub moveThruDir(myPar$, t As TreeView)
  Dim newDir$, subdir$(), i%
  Dim nod As Node
  
  t.Refresh
  i = 0
  ReDim subdir(i)
  newDir = Dir("*", vbDirectory)
  While newDir <> ""
    If Left(newDir, 1) <> "." Then
      If GetAttr(newDir) = vbDirectory Then
        i = i + 1
        ReDim Preserve subdir(i)
        subdir(i) = newDir
      End If
    End If
    newDir = Dir
  Wend
  
  For i = 1 To UBound(subdir)
    ChDir subdir(i)
    Set nod = t.Nodes.Add(myPar, tvwChild, myPar & "\" & subdir(i), subdir(i))
    nod.Image = "Folder"
    nod.Parent.Expanded = True
    moveThruDir nod.FullPath, t
    add2tree nod.FullPath, t
    ChDir ("..\")
  Next i
    
End Sub

Private Sub add2tree(myPar$, t As TreeView)
  Dim myFile$
  Dim nod As Node
  Dim siIndex&, nodIndex&, typeIndex&

  nodIndex = t.Nodes.count + 1
  
  For typeIndex = LBound(dispTypes) To UBound(dispTypes)
    myFile = Dir("*." & dispTypes(typeIndex))
    While myFile <> "" 'dir loses its position if much else goes on
      If dispTypes(typeIndex) = "*" Then 'have to figure out length of extension
        Dim ext$, noExt$
        noExt = FilenameOnly(myFile)
        ext = Mid(myFile, Len(noExt) + 2)
        Set nod = t.Nodes.Add(myPar, tvwChild, , noExt)
        nod.Tag = LCase(ext)
      Else
        Set nod = t.Nodes.Add(myPar, tvwChild, , Left(myFile, Len(myFile) - Len(dispTypes(typeIndex)) - 1))
        nod.Tag = dispTypes(typeIndex)
      End If
      myFile = Dir
    Wend
  Next typeIndex
  
  While nodIndex <= t.Nodes.count
    Set nod = t.Nodes(nodIndex)
    siIndex = UBound(fi) + 1
    ReDim Preserve fi(siIndex)
    Call getFileInfo(nod.FullPath, nod.Tag, fi(siIndex))
    'We temporarily used nod.tag to hold the file extension, but that has been copied into fi(siIndex)
    'From now on we use the tag to hold siIndex so we can easily find the node's info later
    nod.Tag = siIndex
    On Error GoTo DefaultShape
    nod.Image = FileTypeName(fi(siIndex).FileType)
    On Error GoTo 0
    nod.Parent.Expanded = True
    nodIndex = nodIndex + 1
  Wend
  
  Exit Sub
  
DefaultShape:
  nod.Image = "Null"
  Resume Next
End Sub

'Reads main header at beginning of shp or shx file
'Public Sub ReadShapeHeader(infile%, ByRef FileLength&, ByRef ShapeType&, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#)
'  Dim FileCode&, version&
'  Seek #infile, 1
'  FileCode = ReadBigInt(infile)
'  If FileCode <> 9994 Then If MsgBox("Warning: File Code of " & FileCode & " found. Expected 9994.", vbOKCancel, "ReadShapeHeader") = vbCancel Then Exit Sub
'  Seek #infile, 25
'  FileLength = ReadBigInt(infile)
'  Get #infile, , version
'  If version <> 1000 Then If MsgBox("Warning: Version of " & version & " found. Expected 1000.", vbOKCancel, "ReadShapeHeader") = vbCancel Then Exit Sub
'  Get #infile, , ShapeType '0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
'  ReadBoundingBox infile, lowX, lowY, uppX, uppY
  'Dim s$
  's = "Code = " & FileCode & " (9994 expected), Version = " & version & " (1000 expected)" & vbCr
  's = s & "Length = " & FileLength & vbCr
  'If ShapeType = 1 Then s = s & "Points (Length implies " & (FileLength - 50) / 14 & " points in shp or " & (FileLength - 50) / 4 & " points in shx)" & vbCr Else s = s & "Shape Type = " & ShapeType
  's = s & ", Bounding box: (" & lowX & ", " & lowY & "), (" & uppX & ", " & uppY & ")" & vbCr
  'MsgBox s, vbOKOnly, "ReadShapeHeader"
  
'End Sub

'Public Sub WriteShapeHeader(outfile%, FileLength&, ShapeType&, lowX#, lowY#, uppX#, uppY#)
'  Dim i&, bigzero&, version&
'  Seek #outfile, 1
'  Put outfile, , SwapBytes(9994)
'  For i = 1 To 5
'    Put outfile, , 0
'  Next i
'  Put outfile, , SwapBytes(FileLength)
'  version = 1000
'  Put #outfile, , version
'  Put #outfile, , ShapeType '0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
'  Put #outfile, , lowX      'bounding box
'  Put #outfile, , lowY
'  Put #outfile, , uppX
'  Put #outfile, , uppY
'  For i = 1 To 8
'    Put #outfile, , bigzero
'  Next i

  'Dim s$
  's = "Code = 9994 expected, Version = " & version & " (1000 expected)" & vbCr
  's = s & "Length = " & FileLength & vbCr
  'If ShapeType = 1 Then s = s & "Points (Length implies " & (FileLength - 50) / 14 & " points in shp or " & (FileLength - 50) / 4 & " points in shx)" & vbCr Else s = s & "Shape Type = " & ShapeType
  's = s & ", Bounding box: (" & lowX & ", " & lowY & "), (" & uppX & ", " & uppY & ")" & vbCr
  'MsgBox s, vbOKOnly, "WriteShapeHeader"

'End Sub

'Private Sub ReadBoundingBox(infile, ByRef lowX#, ByRef lowY#, ByRef uppX#, ByRef uppY#)
'  Get #infile, , lowX
'  Get #infile, , lowY
'  Get #infile, , uppX
'  Get #infile, , uppY
'End Sub

