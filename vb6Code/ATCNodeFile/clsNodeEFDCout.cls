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
  pFileExt = "EFDC Output Files (*.out)|*.out"
  pErrorDescription = ""
  
  Set pNodes = Nothing
  Set pNodes = New Collection
End Sub

Private Property Get ATCclsNodeFile_Description() As String
End Property

Private Property Get ATCclsNodeFile_ErrorDescription() As String
End Property

Private Property Get ATCclsNodeFile_FileExtension() As String
  ATCclsNodeFile_FileExtension = "out"
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
  ATCclsNodeFile_Label = "EFDC"
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

Private Sub ATCclsNodeFile_WriteMapFile(Filename As String)
  Dim fileContents As String
  If Len(Dir(Filename)) > 0 Then fileContents = WholeFileString(Filename) & vbCrLf
  fileContents = fileContents & "LYR '" & pShapeBaseName & "P',Keys 'ID'" & vbCrLf
  fileContents = fileContents & "LYR '" & pShapeBaseName & "L',Keys 'ID'" & vbCrLf
  SaveFileString Filename, fileContents
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

  dbf.NumRecords = pNodes.Count
  dbf.NumFields = 10
  For FieldNumber = 1 To dbf.NumFields
    dbf.FieldLength(FieldNumber) = 8
    dbf.FieldType(FieldNumber) = "N"
    dbf.FieldDecimalCount(FieldNumber) = 0
  Next
  dbf.FieldName(1) = "ID"
  dbf.FieldName(2) = "UpstreamID"
  dbf.FieldName(3) = "DOWNID"
  dbf.FieldName(4) = "LFU"
  dbf.FieldName(5) = "LFD"
  dbf.FieldName(6) = "LCU"
  dbf.FieldName(7) = "LCD"
  dbf.FieldName(8) = "X"
  dbf.FieldName(9) = "Y"
  dbf.FieldName(10) = "BEL"
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
      dbf.Value(2) = .Value("UpstreamID")
      dbf.Value(3) = .Value("DOWNID")
      DownID = dbf.Value(3)
      dbf.Value(4) = .Value("LFU")
      dbf.Value(5) = .Value("LFD")
      dbf.Value(6) = .Value("LCU")
      dbf.Value(7) = .Value("LCD")
      dbf.Value(8) = .Value("X")
      dbf.Value(9) = .Value("Y")
      dbf.Value(10) = .Value("BEL")
      'dbf.Value(11) = .Value("I")
      'dbf.Value(12) = .Value("J")
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
  Line Input #inFile, buf 'skip    465.5000 TIME IN DAYS
  Line Input #inFile, buf 'skip  L1D   LSU  LSD  LFU LFD  LCU  LCD      X         Y        BEL     DEP     AREA     WPER    BSRF    VEL        Q       SAL     TEM       SED      SND1      SND2     SEDB        SNDB1       SNDB2      TAUBED
                          'was   L1D    L    I    J      X         Y        BEL     DEP     AREA     WPER    BSRF    VEL        Q       SAL     TEM       SED      SND1      SND2     SEDB        SNDB1       SNDB2      TAUBED
  Line Input #inFile, buf 'read first line of data

  While Mid(buf, 14, 1) <> "T"
    'Location = StrSplit(buf, " ", "")
    Set newNode = New ATCclsNode
    With newNode.Attributes
      .SetValue "ID", CLng(StrSplit(buf, " ", ""))
      .SetValue "UpstreamID", CLng(StrSplit(buf, " ", ""))
      .SetValue "DOWNID", CLng(StrSplit(buf, " ", ""))
      .SetValue "LFU", CLng(StrSplit(buf, " ", ""))
      .SetValue "LFD", CLng(StrSplit(buf, " ", ""))
      .SetValue "LCU", CLng(StrSplit(buf, " ", ""))
      .SetValue "LCD", CLng(StrSplit(buf, " ", ""))
      .SetValue "X", CDbl(StrSplit(buf, " ", ""))
      .SetValue "Y", CDbl(StrSplit(buf, " ", ""))
      .SetValue "BEL", CSng(StrSplit(buf, " ", ""))
      '.SetValue "I", CLng(StrSplit(buf, " ", ""))
      '.SetValue "J", CLng(StrSplit(buf, " ", ""))
      
      Set newShape = New ATCclsShape
      newShape.IsPoint = True
      Vertices(0, 0) = CDbl(newNode.Attributes.Value("X"))
      Vertices(0, 1) = CDbl(newNode.Attributes.Value("Y"))
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

