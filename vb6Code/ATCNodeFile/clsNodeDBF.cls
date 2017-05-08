VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsNodeDBF"
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

Private pInputDBF As clsDBF

Private Sub ATCclsNodeFile_Clear()
  pFileName = "Untitled"
  pFileExt = "Node Files (*.dbf)|*.dbf"
  pErrorDescription = ""
  
  Set pNodes = Nothing
  Set pNodes = New Collection
End Sub

Private Property Get ATCclsNodeFile_Description() As String
End Property

Private Property Get ATCclsNodeFile_ErrorDescription() As String
End Property

Private Property Get ATCclsNodeFile_FileExtension() As String
  ATCclsNodeFile_FileExtension = "dbf"
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
  ATCclsNodeFile_Label = "DBF"
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
  Dim FileContents As String
  If Len(Dir(filename)) > 0 Then FileContents = WholeFileString(filename) & vbCrLf
  FileContents = FileContents & "LYR '" & pShapeBaseName & "',lavender,Keys 'LOCATION'" & vbCrLf
  FileContents = FileContents & "dbf 'DSN'" & vbCrLf
  FileContents = FileContents & "dbf 'LOCATION'" & vbCrLf
  SaveFileString filename, FileContents
End Sub

Private Sub ATCclsNodeFile_WriteShapeFile(baseFilename As String)
  Dim shpfile%, shxfile%
  Dim shpfileL%, shxfileL%
  Dim RecordNumber&, FieldNumber&
  Dim FileLength As Long, ShapeType As Long
  Dim FileLengthL As Long
  Dim x#, y#, lowX#, lowY#, uppX#, uppY#
  Dim Xs#(), Ys#()  'For lines
  Dim id As Long, LastID
  Dim DownID As Long, LastDownID As Long
  Dim UpID As Long, LastUpID As Long

  Dim nod As ATCclsNode
  Dim shap As ATCclsShape
        
'  If Len(FilenameOnly(baseFilename)) > 7 Then
'    MsgBox "Base name of shape file must be seven characters or less."
'    Exit Sub
'  End If
  
  pShapeBaseName = baseFilename
  

  
  CreateNewShapeFile baseFilename, 1
  'Point Shape Main file (.shp)
  shpfile = FreeFile(0)
  Open baseFilename & ".shp" For Binary Access Read Write As shpfile
  Call ReadShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Seek #shpfile, 101
  
  'Point Shape Index file (.shx)
  shxfile = FreeFile(0)
  Open baseFilename & ".shx" For Binary Access Read Write As shxfile
  Seek #shxfile, 101
  
  RecordNumber = 0
  While RecordNumber < pInputDBF.NumRecords
    RecordNumber = RecordNumber + 1
    pInputDBF.CurrentRecord = RecordNumber
    x = pInputDBF.Value(12)
    y = pInputDBF.Value(11)

    Call WriteShapePointAll(shpfile, RecordNumber, x, y)
    Call WriteShapePointIndex(shxfile, RecordNumber)

    
    FileLength = FileLength + 14
    If x > uppX Then uppX = x Else If x < lowX Then lowX = x
    If y > uppY Then uppY = y Else If y < lowY Then lowY = y
  Wend
    
  ShapeType = 1
  
  Call WriteShapeHeader(shpfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shpfile
  
  FileLength = 50 + RecordNumber * 4
  Call WriteShapeHeader(shxfile, FileLength, ShapeType, lowX, lowY, uppX, uppY)
  Close shxfile

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
  
  Set pInputDBF = Nothing
  Set pInputDBF = New clsDBF
  pInputDBF.OpenDBF pFileName
  
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


