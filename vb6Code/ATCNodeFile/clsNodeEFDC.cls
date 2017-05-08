VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsNodeEFDC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
Implements ATCclsNodeFile
'This version reads cellnet.inp format

Private pNodes As Collection
Private pFileName As String
Private pFileExt As String
Private pShapeBaseName As String
Private pHelpFileName As String
Private pErrorDescription As String
Private pMonitor As Object
Private pMonitorSet As Boolean

Private nsegs As Long
Private nords As Long
Private ncells As Long
Private nctl As Long
Private nqts As Long
Private ncts As Long
Private nobc As Long

Private isord() As Long
Private isgdn() As Long
Private iscdn() As Long
Private iscup() As Long

Private Sub ATCclsNodeFile_Clear()
  pFileName = "Untitled"
  pFileExt = "Node Files (*.inp)|*.inp"
  pErrorDescription = ""
  
  Set pNodes = Nothing
  Set pNodes = New Collection
End Sub

Private Property Get ATCclsNodeFile_Description() As String
End Property

Private Property Get ATCclsNodeFile_ErrorDescription() As String
End Property

Private Property Get ATCclsNodeFile_FileExtension() As String
  ATCclsNodeFile_FileExtension = "inp"
End Property

Private Property Let ATCclsNodeFile_Filename(ByVal NewValue As String)
  pFileName = NewValue
  ReadFile
End Property
Private Property Get ATCclsNodeFile_Filename() As String
  ATCclsNodeFile_Filename = pFileName
End Property

Private Property Let ATCclsNodeFile_HelpFilename(ByVal NewValue As String)
  pHelpFileName = NewValue
End Property

Private Property Get ATCclsNodeFile_Label() As String
  ATCclsNodeFile_Label = "INP"
End Property

Private Property Set ATCclsNodeFile_Monitor(ByVal NewValue As Object)
  Set pMonitor = NewValue
  pMonitorSet = True
End Property

Private Property Get ATCclsNodeFile_Nodes() As Collection
  Set ATCclsNodeFile_Nodes = pNodes
End Property

Private Sub ATCclsNodeFile_refresh()

End Sub

Private Sub ATCclsNodeFile_WriteMapFile(filename As String)
  Dim fileContents As String
  If Len(Dir(filename)) > 0 Then fileContents = WholeFileString(filename) & vbCrLf
  fileContents = fileContents & "LYR '" & pShapeBaseName & "P',lavender,Keys 'ID',BRN 'BRANCH',DWN 'DOWNID',LEN 'rlen'" & vbCrLf
  fileContents = fileContents & "LYR '" & pShapeBaseName & "L',blue,Keys 'ID',BRN 'BRANCH',DWN 'DOWNID',LEN 'rlen',Animate,Style SolidLine,Animate" & vbCrLf
  fileContents = fileContents & "dbf 'ID'" & vbCrLf
  fileContents = fileContents & "dbf 'BRANCH'" & vbCrLf
  fileContents = fileContents & "dbf 'DOWNID'" & vbCrLf
  SaveFileString filename, fileContents
End Sub

Private Sub ATCclsNodeFile_WriteShapeFile(baseFilename As String)
  Dim shpfile%, shxfile%
  Dim shpfileL%, shxfileL%
  Dim RecordNumber&, FieldNumber&
  Dim FileLength As Long, ShapeType As Long
  Dim FileLengthL As Long
  Dim X#, y#, lowX#, lowY#, uppX#, uppY#
  Dim Xs#(), Ys#()  'For lines
  Dim id As Long, LastID
  Dim DownID As Long, LastDownID As Long
  Dim UpID As Long, LastUpID As Long
  Dim dbf As clsDBF
  Dim testDBF As clsDBF

  Dim vNode As Variant
  Dim nod As ATCclsNode
  Dim shap As ATCclsShape
      
  ReDim Xs(0 To 2)
  ReDim Ys(0 To 2)
  
  If Len(FilenameOnly(baseFilename)) > 7 Then
    MsgBox "Base name of shape file must be seven characters or less."
    Exit Sub
  End If
  
  pShapeBaseName = baseFilename
  
  'Shape data (.dbf)
  Set dbf = New clsDBF
  
'  dbf.OpenDBF "c:\points.dbf"
'  'dbf.WriteDBF "c:\pointtst.dbf"
'  Set testDBF = New clsDBF
'  testDBF.NumRecords = dbf.NumRecords
'  testDBF.NumFields = dbf.NumFields
'  testDBF.Day = dbf.Day
'  testDBF.Month = dbf.Month
'  testDBF.Year = dbf.Year
'
'  For FieldNumber = 1 To dbf.NumFields
'    testDBF.FieldLength(FieldNumber) = dbf.FieldLength(FieldNumber)
'    testDBF.FieldType(FieldNumber) = dbf.FieldType(FieldNumber)
'    testDBF.FieldDecimalCount(FieldNumber) = dbf.FieldDecimalCount(FieldNumber)
'    testDBF.FieldName(FieldNumber) = dbf.FieldName(FieldNumber)
'  Next
'  testDBF.InitData
'  For RecordNumber = 1 To dbf.NumRecords
'    dbf.CurrentRecord = RecordNumber
'    testDBF.CurrentRecord = RecordNumber
'    For FieldNumber = 1 To dbf.NumFields
'      testDBF.Value(FieldNumber) = dbf.Value(FieldNumber)
'    Next
'  Next
'  testDBF.WriteDBF "test"
'  Set testdbf = Nothing
  '(baseFilename & "P" & ".dbf", "DBASE IV")
'  For FieldNumber = 1 To dbf.NumFields
'    Debug.Print FieldNumber & ":" & dbf.FieldName(FieldNumber) & ", " & dbf.FieldType(FieldNumber) & ", " & dbf.FieldLength(FieldNumber) & ", " & dbf.FieldDecimalCount(FieldNumber)
'  Next
'  For RecordNumber = 1 To dbf.NumRecords
'    dbf.CurrentRecord = RecordNumber
'    Debug.Print RecordNumber & ":";
'    For FieldNumber = 1 To dbf.NumFields
'      Debug.Print dbf.Value(FieldNumber) & ", ";
'    Next
'    Debug.Print
'  Next

  dbf.NumRecords = ncells
  dbf.NumFields = 27
  For FieldNumber = 1 To dbf.NumFields
    dbf.FieldLength(FieldNumber) = 8
    dbf.FieldType(FieldNumber) = "N"
    dbf.FieldDecimalCount(FieldNumber) = 0
  Next
  dbf.FieldName(1) = "ID"
  dbf.FieldName(2) = "BRANCH"
  dbf.FieldName(3) = "itype"
  dbf.FieldName(4) = "iupc"
  dbf.FieldName(5) = "idnc"
  dbf.FieldName(6) = "ifru"
  dbf.FieldName(7) = "ifrd"
  dbf.FieldName(8) = "iqcu"
  dbf.FieldName(9) = "iqcd"
  dbf.FieldName(10) = "nqctu"
  dbf.FieldName(11) = "nqctd"
  dbf.FieldName(12) = "nqts"
  dbf.FieldName(13) = "ncts"
  dbf.FieldName(14) = "nobc"
  dbf.FieldName(15) = "nsec"
  dbf.FieldName(16) = "rlen"
  dbf.FieldName(17) = "x"
  dbf.FieldName(18) = "y"
  dbf.FieldName(19) = "angc"
  dbf.FieldName(20) = "qint"
  dbf.FieldName(21) = "hint"
  dbf.FieldName(22) = "bint"
  dbf.FieldName(23) = "rman"
  dbf.FieldName(24) = "ists"
  dbf.FieldName(25) = "idum"
  dbf.FieldName(26) = "jdum"
  dbf.FieldName(27) = "DOWNID"
  dbf.InitData
  
  CreateNewShapeFile baseFilename & "P", 1
  'Point Shape Main file (.shp)
  shpfile = FreeFile(0)
  Open baseFilename & "P.shp" For Binary Access Read Write As shpfile
  Call ReadShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Seek #shpfile, 101
  
  'Point Shape Index file (.shx)
  shxfile = FreeFile(0)
  Open baseFilename & "P.shx" For Binary Access Read Write As shxfile
  Seek #shxfile, 101
  
  CreateNewShapeFile baseFilename & "L", 3
  'Line Shape Main file (.shp)
  shpfileL = FreeFile(0)
  Open baseFilename & "L.shp" For Binary Access Read Write As shpfileL
  Call ReadShapeHeader(shpfileL, FileLengthL, ShapeType, lowX, lowY, uppX, uppY)
  Seek #shpfileL, 101
  
  'Line Shape Index file (.shx)
  shxfileL = FreeFile(0)
  Open baseFilename & "L.shx" For Binary Access Read Write As shxfileL
  Seek #shxfileL, 101

  RecordNumber = 0
  For Each vNode In pNodes
    LastDownID = DownID
    LastUpID = UpID
    LastID = id
    RecordNumber = RecordNumber + 1
    dbf.CurrentRecord = RecordNumber
    With pNodes(RecordNumber).Attributes
      id = .Value("ID")
      dbf.Value(1) = id
      dbf.Value(2) = .Value("BRANCH")
      dbf.Value(3) = .Value("itype")
      dbf.Value(4) = .Value("iupc")
      dbf.Value(5) = .Value("idnc")
      dbf.Value(6) = .Value("ifru")
      dbf.Value(7) = .Value("ifrd")
      dbf.Value(8) = .Value("iqcu")
      dbf.Value(9) = .Value("iqcd")
      dbf.Value(10) = .Value("nqctu")
      dbf.Value(11) = .Value("nqctd")
      dbf.Value(12) = .Value("nqts")
      dbf.Value(13) = .Value("ncts")
      dbf.Value(14) = .Value("nobc")
      dbf.Value(15) = .Value("nsec")
      dbf.Value(16) = .Value("rlen")
      dbf.Value(17) = .Value("x")
      dbf.Value(18) = .Value("y")
      dbf.Value(19) = .Value("angc")
      dbf.Value(20) = .Value("qint")
      dbf.Value(21) = .Value("hint")
      dbf.Value(22) = .Value("bint")
      dbf.Value(23) = .Value("rman")
      dbf.Value(24) = .Value("ists")
      dbf.Value(25) = .Value("idum")
      dbf.Value(26) = .Value("jdum")
      DownID = .Value("idnc")
      If DownID = 0 Then DownID = .Value("ifrd")
      If DownID = 0 Then DownID = .Value("iqcd")
      dbf.Value(27) = DownID
      UpID = .Value("iupc")
      If UpID = 0 Then UpID = .Value("ifru")
      If UpID = 0 Then UpID = .Value("iqcu")
    End With
    Set nod = vNode
    Set shap = nod.Shapes(1)
    X = shap.Vertices(0, 0)
    y = shap.Vertices(0, 1)
    Call WriteShapePointAll(shpfile, RecordNumber, X, y)
    Call WriteShapePointIndex(shxfile, RecordNumber)

    Xs(2) = (Xs(1) + X) / 2
    Ys(2) = (Ys(1) + y) / 2
    If Xs(0) = 0 And Xs(0) = 0 Then
      Xs(0) = X
      Ys(0) = y
    Else
      If LastID <> DownID Then 'discontinuity - don't connect the dots
        Xs(2) = Xs(1)
        Ys(2) = Ys(1)
        Call WriteShapeSingleLine(shpfileL, shxfileL, RecordNumber, Xs, Ys)
        Xs(0) = X
        Ys(0) = y
      Else
        Call WriteShapeSingleLine(shpfileL, shxfileL, RecordNumber, Xs, Ys)
        Xs(0) = Xs(2)
        Ys(0) = Ys(2)
      End If
    End If
    Xs(1) = X
    Ys(1) = y
    
    FileLength = FileLength + 14
    If X > uppX Then uppX = X Else If X < lowX Then lowX = X
    If y > uppY Then uppY = y Else If y < lowY Then lowY = y
  Next
  dbf.WriteDBF baseFilename & "P.dbf"
  FileCopy baseFilename & "P.dbf", baseFilename & "L.dbf"
  
  Xs(2) = X
  Ys(2) = y
  Call WriteShapeSingleLine(shpfileL, shxfileL, RecordNumber, Xs, Ys)
  
  ShapeType = 1
  
  Call WriteShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shpfile
  
  FileLength = 50 + RecordNumber * 4
  Call WriteShapeHeader(shxfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shxfile

  ShapeType = 3

  FileLengthL = 50 + RecordNumber * 52
  Call WriteShapeHeader(shpfileL, FileLengthL, ShapeType, lowX, lowY, uppX, uppY)
  Close shpfileL
  
  FileLengthL = 50 + RecordNumber * 4
  Call WriteShapeHeader(shxfileL, FileLengthL, ShapeType, lowX, lowY, uppX, uppY)
  Close shxfileL
End Sub

Private Sub Class_Initialize()
  pMonitorSet = False
  ATCclsNodeFile_Clear
End Sub

Private Sub ReadFile()
  Dim inFile As Integer
  Dim buf As String
  Dim segNum As Long
  Dim newNode As ATCclsNode
  Dim newShape As ATCclsShape
  Dim Vertices(0, 1) As Double
  
  On Error GoTo ReadError
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(OPEN Reading Node File)"
    pMonitor.SendMonitorMessage "(BUTTOFF CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTOFF PAUSE)"
    pMonitor.SendMonitorMessage "(MSG1 " & pFileName & ")"
  End If
  
  inFile = FreeFile(0)
  Open pFileName For Input As #inFile
  
  GoSub SkipComments
  
  nsegs = StrSplit(buf, " ", "")
  nords = StrSplit(buf, " ", "")
  ncells = StrSplit(buf, " ", "")
  nctl = StrSplit(buf, " ", "")
  nqts = StrSplit(buf, " ", "")
  ncts = StrSplit(buf, " ", "")
  nobc = StrSplit(buf, " ", "")
  
  GoSub SkipComments
  
  ReDim isord(nsegs)
  ReDim isgdn(nsegs)
  ReDim iscdn(nsegs)
  ReDim iscup(nsegs)
  
  While Left(buf, 1) <> "c"
    segNum = StrSplit(buf, " ", "")
    If segNum < 1 Or segNum > nsegs Then
      MsgBox "Suspicious line: " & segNum & " " & buf
    Else
      isord(segNum) = StrSplit(buf, " ", "")
      isgdn(segNum) = StrSplit(buf, " ", "")
      iscdn(segNum) = StrSplit(buf, " ", "")
      iscup(segNum) = StrSplit(buf, " ", "")
    End If
    Line Input #inFile, buf
  Wend
  
  GoSub SkipComments
  
  While Len(Trim(buf)) > 0
    Set newNode = New ATCclsNode
    With newNode.Attributes
      .SetValue "ID", CLng(StrSplit(buf, " ", ""))
      .SetValue "BRANCH", CLng(StrSplit(buf, " ", ""))
      .SetValue "itype", CLng(StrSplit(buf, " ", ""))
      .SetValue "iupc", CLng(StrSplit(buf, " ", ""))
      .SetValue "idnc", CLng(StrSplit(buf, " ", ""))
      .SetValue "ifru", CLng(StrSplit(buf, " ", ""))
      .SetValue "ifrd", CLng(StrSplit(buf, " ", ""))
      .SetValue "iqcu", CLng(StrSplit(buf, " ", ""))
      .SetValue "iqcd", CLng(StrSplit(buf, " ", ""))
      .SetValue "nqctu", CLng(StrSplit(buf, " ", ""))
      .SetValue "nqctd", CLng(StrSplit(buf, " ", ""))
      .SetValue "nqts", CLng(StrSplit(buf, " ", ""))
      .SetValue "ncts", CLng(StrSplit(buf, " ", ""))
      .SetValue "nobc", CLng(StrSplit(buf, " ", ""))
      .SetValue "nsec", CLng(StrSplit(buf, " ", ""))
      .SetValue "rlen", CSng(StrSplit(buf, " ", ""))
      .SetValue "x", CDbl(StrSplit(buf, " ", ""))
      .SetValue "y", CDbl(StrSplit(buf, " ", ""))
      .SetValue "angc", CSng(StrSplit(buf, " ", ""))
      .SetValue "qint", CSng(StrSplit(buf, " ", ""))
      .SetValue "hint", CSng(StrSplit(buf, " ", ""))
      .SetValue "bint", CSng(StrSplit(buf, " ", ""))
      .SetValue "rman", CSng(StrSplit(buf, " ", ""))
      .SetValue "ists", CLng(StrSplit(buf, " ", ""))
      .SetValue "idum", CLng(StrSplit(buf, " ", ""))
      .SetValue "jdum", CLng(StrSplit(buf, " ", ""))
      
      Set newShape = New ATCclsShape
      newShape.IsPoint = True
      Vertices(0, 0) = CDbl(newNode.Attributes.Value("x"))
      Vertices(0, 1) = CDbl(newNode.Attributes.Value("y"))
      newShape.Vertices = Vertices
      newNode.Shapes.Add newShape
      Set newShape = Nothing
      Debug.Print "Read Node #" & .Value("ID") & " (" & Vertices(0, 0) & ", " & Vertices(0, 1) & ")"
    End With
    pNodes.Add newNode
    Set newNode = Nothing
    Line Input #inFile, buf
  Wend
      
  Close #inFile
  
CloseMonitor:
  If pMonitorSet Then
    pMonitor.SendMonitorMessage "(CLOSE)"
    pMonitor.SendMonitorMessage "(BUTTON CANCEL)"
    pMonitor.SendMonitorMessage "(BUTTON PAUSE)"
  End If
  
  Exit Sub

ReadError:
  pErrorDescription = "Error reading '" & pFileName & "' - " & Err.Description
  GoTo CloseMonitor

SkipComments:
  buf = "c"
  While LCase(Left(buf, 1)) = "c"
    Line Input #inFile, buf
  Wend
  Return
End Sub

