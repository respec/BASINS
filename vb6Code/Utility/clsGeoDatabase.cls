VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsGeoDatabase"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private pDatabaseFilename As String
Private pDatabase As Database
Private pDatabaseOpenLevel As Long
Private pShapeType As Long
Private pNHD_DBF As Boolean 'True if creating a BASINS NHD flow line shape file DBF
Private pLastError As String
Private pLastDBF As clsDBF

Public Property Get LastDBFsaved() As clsDBF
  Set LastDBFsaved = pLastDBF
End Property

Public Function LastError() As String
  LastError = pLastError
  pLastError = ""
End Function

Private Function GetDB() As Database
  If pDatabase Is Nothing Or pDatabaseOpenLevel = 0 Then
    Set pDatabase = OpenDatabase(pDatabaseFilename, False, vbReadOnly)
    pDatabaseOpenLevel = 1
  End If
  Set pDatabase = pDatabase
End Function
Private Sub ReleaseDB()
  pDatabaseOpenLevel = pDatabaseOpenLevel - 1
  If pDatabaseOpenLevel = 0 Then pDatabase.Close
End Sub

Public Function OpenGeoDatabase(aDatabaseFilename As String) As Boolean
  
  pDatabaseFilename = aDatabaseFilename
  
  On Error GoTo ErrOpeningDatabase
  GetDB
  ReleaseDB
  OpenGeoDatabase = True
  
  Exit Function

ErrOpeningDatabase:
  OpenGeoDatabase = False
  pLastError = "OpenGeoDatabase: " & Err.Description & " (" & Err.Number & ")"
End Function

Public Function GeoLayers() As FastCollection
  Dim HasShapeField As Boolean
  Dim retval As New FastCollection
  Dim iTable As Long
  Dim fieldNum As Long
  Dim fld As Field
  
  GetDB
  
  For iTable = 0 To pDatabase.TableDefs.Count - 1
    With pDatabase.TableDefs(iTable)
      If .RecordCount > 0 Then
        For fieldNum = 0 To .Fields.Count - 1
          Set fld = .Fields(fieldNum)
          If LCase(fld.Name) = "shape" And fld.Type = 11 Then
            If UnderstandShapeType(.Name, fieldNum) Then
              retval.Add .Name, .Name
            End If
          End If
        Next
      End If
    End With
  Next
  
  ReleaseDB
  
  Set GeoLayers = retval
End Function

Private Function UnderstandShapeType(aLayerName As String, aFieldNumber As Long) As Boolean
  Dim rs As Recordset
  Dim lShape() As Byte
  Dim lShapeType As Long
  Set rs = pDatabase.OpenRecordset(aLayerName, dbOpenTable)
  lShape = rs.Fields(aFieldNumber).Value
  lShapeType = lShape(0) + 16 * lShape(1) ' + 256 * lShape(2)
  Select Case lShapeType
    Case 1, 3, 5, 8, 11, 13, 15, 18, 21, 23, 25, 28, 31
      UnderstandShapeType = True
    Case Else
      UnderstandShapeType = False
  End Select
  rs.Close
End Function

Public Sub SaveShape(aLayerName As String, aShapeFilename As String, _
            Optional ByVal aNewFieldSpec As String = "", _
            Optional manager As Object = Nothing)
  Dim outfile As Integer
  Dim OutFileX As Integer
  Dim DB As Database
  Dim dbf As clsDBF
  Dim rs As Recordset
  Dim rsOther As Recordset
  Dim fld As DAO.Field
  Dim ShapeField As Long
  Dim DBFfield As Long
  Dim DBFmatchingField() As Long
  Dim indx As DAO.index
  Dim i As Long
  Dim IndexNum As Long
  Dim fieldNum As Long
  Dim Value As String
  Dim HasShapeField As Boolean
  Dim FileLengthWords As Long
  Dim PercentDone As Long
  Dim LastPercentDone As Long
  
  Dim NumNewFields As Long
  Dim NewFieldNum As Long
  Dim NewFieldName() As String
  Dim LookupFieldName() As String
  Dim ReferenceTableName() As String
  Dim ReferenceTableSearchFieldName() As String
  Dim ReferenceTableValueFieldName() As String
  
  Dim xMin As Double, xMax As Double
  Dim yMin As Double, yMax As Double
  Dim zMin As Double, zMax As Double
  Dim mMin As Double, mMax As Double
  
  xMin = 1E+300: xMax = -1E+300
  yMin = 1E+300: yMax = -1E+300
  zMin = 1E+300: zMax = -1E+300
  mMin = 1E+300: mMax = -1E+300
  
  'On Error GoTo ErrHandler
  
  NumNewFields = CountString(aNewFieldSpec, ",")
  If NumNewFields > 0 Then
    If NumNewFields Mod 5 = 4 Then
      NumNewFields = NumNewFields / 5
    Else
      MsgBox "Incorrect number of commas in aNewFieldSpec" & vbCrLf _
            & NumNewFields + 1 & " not divisible by 5", vbCritical, "clsGeoDatabase SaveShape"
      NumNewFields = 0
    End If
  End If
        
  GetDB 'Set DB = OpenDatabase(DatabaseFilename, False, vbReadOnly)
  
  Set rs = pDatabase.OpenRecordset(aLayerName, dbOpenTable)
  
  'If CustomDBF Is Nothing Then
    Set dbf = New clsDBF
    dbf.numFields = rs.Fields.Count + NumNewFields - 1 'TODO: add connectivity fields
    dbf.NumRecords = rs.RecordCount
    DBFfield = 0
  'End If
  ReDim DBFmatchingField(rs.Fields.Count - 1)
  For fieldNum = 0 To rs.Fields.Count - 1
    Set fld = rs.Fields(fieldNum)
    If LCase(fld.Name) = "shape" Then
      ShapeField = fieldNum
    Else
      DBFfield = DBFfield + 1
      SetDBFfieldFromMDB dbf, DBFfield, fld
    End If
    DBFmatchingField(fieldNum) = DBFfield
  Next
  If NumNewFields > 0 Then
    ReDim NewFieldName(NumNewFields)
    ReDim LookupFieldName(NumNewFields)
    ReDim ReferenceTableName(NumNewFields)
    ReDim ReferenceTableSearchFieldName(NumNewFields)
    ReDim ReferenceTableValueFieldName(NumNewFields)
    
    For NewFieldNum = 1 To NumNewFields
      NewFieldName(NewFieldNum) = StrSplit(aNewFieldSpec, ",", "")
      LookupFieldName(NewFieldNum) = StrSplit(aNewFieldSpec, ",", "")
      ReferenceTableName(NewFieldNum) = StrSplit(aNewFieldSpec, ",", "")
      ReferenceTableSearchFieldName(NewFieldNum) = StrSplit(aNewFieldSpec, ",", "")
      ReferenceTableValueFieldName(NewFieldNum) = StrSplit(aNewFieldSpec, ",", "")
    
      Set rsOther = pDatabase.OpenRecordset(ReferenceTableName(NewFieldNum), dbOpenDynaset) ' dbOpenTable)
      DBFfield = DBFfield + 1
      SetDBFfieldFromMDB dbf, DBFfield, rsOther.Fields(ReferenceTableValueFieldName(NewFieldNum))
      dbf.fieldName(DBFfield) = NewFieldName(NewFieldNum)
      If NumNewFields > 1 Then rsOther.Close
    Next
  End If
  
  
  dbf.InitData
  
  outfile = FreeFile
  Open aShapeFilename For Binary As outfile
  OutFileX = FreeFile
  Open FilenameNoExt(aShapeFilename) & ".shx" For Binary As OutFileX
  WriteShapeHeader outfile, 0, pShapeType, -100, -100, 100, 100, 0, 0, 0, 0
  WriteShapeHeader OutFileX, 0, pShapeType, -100, -100, 100, 100, 0, 0, 0, 0
  dbf.CurrentRecord = 1
  While Not rs.EOF
    If Not manager Is Nothing Then 'Update progress message
      PercentDone = dbf.CurrentRecord * 100 / rs.RecordCount
      If PercentDone > LastPercentDone Then
        manager.DownloadStatus = "Converting " & aLayerName & " " & PercentDone & "%"
        LastPercentDone = PercentDone
      End If
    End If
    DBFfield = 1
    For fieldNum = 0 To rs.Fields.Count - 1
      With rs.Fields(fieldNum)
        If Not IsNull(.Value) Then
          DBFfield = DBFmatchingField(fieldNum)
          If fieldNum = ShapeField Then
            'Debug.Print "SaveRecordInShapeFile " & dbf.CurrentRecord
            SaveRecordInShapeFile outfile, OutFileX, dbf.CurrentRecord, .Value, xMin, xMax, yMin, yMax, zMin, zMax, mMin, mMax
          ElseIf DBFfield > 0 Then
            If .Type = 11 Then
              dbf.Value(DBFfield) = Binary2Hex(.Value)
            Else
              dbf.Value(DBFfield) = .Value
            End If
          End If
        End If
      End With
    Next
    
    If NumNewFields > 0 Then
      For NewFieldNum = 1 To NumNewFields
        If NumNewFields > 1 Then Set rsOther = pDatabase.OpenRecordset(ReferenceTableName(NewFieldNum), dbOpenTable)
        
        DBFfield = dbf.numFields - NewFieldNum + 1
        
        rsOther.FindFirst ReferenceTableSearchFieldName(NewFieldNum) & "=" & rs.Fields(LookupFieldName(NewFieldNum)).Value
        
        If Not rsOther.NoMatch Then dbf.Value(DBFfield) = rsOther.Fields(ReferenceTableValueFieldName(NewFieldNum))
        
        If NumNewFields > 1 Then rsOther.Close
      Next
    End If
    
    rs.MoveNext
    If Not rs.EOF Then dbf.CurrentRecord = dbf.CurrentRecord + 1
  Wend
            
  dbf.WriteDBF FilenameNoExt(aShapeFilename) & ".dbf"
  Set pLastDBF = dbf
  
  If xMin > xMax Then xMin = 0: xMax = 0
  If yMin > yMax Then yMin = 0: yMax = 0
  If zMin > zMax Then zMin = 0: zMax = 0
  If mMin > mMax Then mMin = 0: mMax = 0
  
  FileLengthWords = (Seek(outfile) - 1) / 2
  WriteShapeHeader outfile, FileLengthWords, pShapeType, xMin, yMin, xMax, yMax, zMin, mMin, zMax, mMax
  FileLengthWords = (Seek(OutFileX) - 1) / 2
  WriteShapeHeader OutFileX, FileLengthWords, pShapeType, xMin, yMin, xMax, yMax, zMin, mMin, zMax, mMax
  Close outfile
  Close OutFileX
  rs.Close
      
  Close #outfile
  
  ReleaseDB
  
  Exit Sub

ErrHandler:
  MsgBox Err.Description, vbOKOnly, "Error in GeoDatabaseToShape"
  ReleaseDB
  Exit Sub

End Sub

Private Sub SetDBFfieldFromMDB(aDBF As clsDBF, aDBFField As Long, aField As Field)
  aDBF.fieldName(aDBFField) = aField.Name
  Select Case aField.Type
    Case dbInteger, dbLong
      Select Case aField.size
        Case 1: aDBF.FieldLength(aDBFField) = 4
        Case 2: aDBF.FieldLength(aDBFField) = 6
        Case 4: aDBF.FieldLength(aDBFField) = 11
        Case Else: aDBF.FieldLength(aDBFField) = 11
      End Select
    Case dbSingle, dbDouble, dbNumeric
      aDBF.FieldLength(aDBFField) = 22
    Case dbDate
      aDBF.FieldLength(aDBFField) = 10 '01/01/1900
    Case Else
      aDBF.FieldLength(aDBFField) = aField.size
  End Select
End Sub


Private Sub SaveRecordInShapeFile(ShapeFile As Integer, _
                                  ShxFile As Integer, _
                                  index As Long, _
                                  shape() As Byte, _
                                  ByRef xMin As Double, ByRef xMax As Double, _
                                  ByRef yMin As Double, ByRef yMax As Double, _
                                  ByRef zMin As Double, ByRef zMax As Double, _
                                  ByRef mMin As Double, ByRef mMax As Double)
  
  Dim ContentLength As Long
    
  ContentLength = ParseContent(shape, xMin, xMax, yMin, yMax, zMin, zMax, mMin, mMax)
  
  WriteShapeIndex ShxFile, ContentLength, Seek(ShapeFile) / 2
  
  WriteBigInt ShapeFile, index
  WriteBigInt ShapeFile, ContentLength
  ReDim Preserve shape(ContentLength * 2 - 1)
  Put #ShapeFile, , shape
End Sub

'Returns ContentLength in 16-bit words
Private Function ParseContent(shape() As Byte, _
                              ByRef xMin As Double, ByRef xMax As Double, _
                              ByRef yMin As Double, ByRef yMax As Double, _
                              ByRef zMin As Double, ByRef zMax As Double, _
                              ByRef mMin As Double, ByRef mMax As Double)
  Dim NumParts As Long
  Dim NumPoints As Long
  Dim ContentLength As Long
    
  'First two bytes of shape contain shape type
  pShapeType = shape(0) + 16 * shape(1)
  Select Case pShapeType
    Case 1: 'Point
      UpdateBounds shape, 4, xMin, xMax
      UpdateBounds shape, 12, yMin, yMax
    Case 11: 'PointZ
      UpdateBounds shape, 4, xMin, xMax
      UpdateBounds shape, 12, yMin, yMax
      UpdateBounds shape, 20, zMin, zMax
      UpdateBounds shape, 28, mMin, mMax
    Case 21: 'PointM
      UpdateBounds shape, 4, xMin, xMax
      UpdateBounds shape, 12, yMin, yMax
      UpdateBounds shape, 20, mMin, mMax
    Case 3, 5, 8, 13, 15, 18, 23, 25, 28, 31: 'All with bounding box
      UpdateMin shape, 4, xMin
      UpdateMin shape, 12, yMin
      UpdateMax shape, 20, xMax
      UpdateMax shape, 28, yMax
  End Select
  'TODO: update Z and/or M bounds for 13,15,18,23,25,28,31
  
  Select Case pShapeType
    Case 1: 'Point
      ContentLength = 10
    Case 11: 'PointZ
      ContentLength = 18
    Case 21: 'PointM
      ContentLength = 14
    
    Case 8: 'Multipoint
      CopyMemory ByVal VarPtr(NumParts), shape(36), 4
      ContentLength = (40 + NumPoints * 16) / 2
    Case 18: 'MultiPointZ
      CopyMemory ByVal VarPtr(NumPoints), shape(36), 4
      ContentLength = (72 + NumPoints * 32) / 2
    Case 28: 'MultiPointM
      CopyMemory ByVal VarPtr(NumPoints), shape(36), 4
      ContentLength = (56 + NumPoints * 24) / 2
    
    Case 3, 5: 'Polyline, Polygon
      CopyMemory ByVal VarPtr(NumParts), shape(36), 4
      CopyMemory ByVal VarPtr(NumPoints), shape(40), 4
      ContentLength = (44 + NumParts * 4 + NumPoints * 16) / 2
    Case 13, 15: 'PolyLineZ, PolygonZ
      CopyMemory ByVal VarPtr(NumParts), shape(36), 4
      CopyMemory ByVal VarPtr(NumPoints), shape(40), 4
      ContentLength = (76 + NumParts * 4 + NumPoints * 32) / 2
    Case 23, 25: 'PolyLineM, PolygonM
      CopyMemory ByVal VarPtr(NumParts), shape(36), 4
      CopyMemory ByVal VarPtr(NumPoints), shape(40), 4
      ContentLength = (60 + NumParts * 4 + NumPoints * 24) / 2
    
    Case Else:
      'This should work for all cases, but some records in database have trailing zeroes
      ContentLength = (UBound(shape) - LBound(shape) + 1) / 2
  End Select
  ParseContent = ContentLength
End Function

Private Sub UpdateBounds(shape() As Byte, _
                         offset As Long, _
                   ByRef currentMin As Double, _
                   ByRef currentMax As Double)
  Dim d As Double
  CopyMemory ByVal VarPtr(d), shape(offset), 8
  If d < currentMin Then currentMin = d
  If d > currentMax Then currentMax = d
End Sub
Private Sub UpdateMax(shape() As Byte, _
                      offset As Long, _
                   ByRef currentMax As Double)
  Dim d As Double
  CopyMemory ByVal VarPtr(d), shape(offset), 8
  If d > currentMax Then currentMax = d
End Sub
Private Sub UpdateMin(shape() As Byte, _
                      offset As Long, _
                  ByRef currentMin As Double)
  Dim d As Double
  CopyMemory ByVal VarPtr(d), shape(offset), 8
  If d < currentMin Then currentMin = d
End Sub

Private Function Binary2Hex(bytes() As Byte) As String
  Dim i As Long
  For i = LBound(bytes) To UBound(bytes)
    If bytes(i) < 16 Then Binary2Hex = Binary2Hex & "0"
    Binary2Hex = Binary2Hex & Hex(bytes(i))
  Next
End Function

Private Function NewNHDtable() As clsDBF
  Set NewNHDtable = New clsDBF
  With NewNHDtable
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .numFields = 11
  
'    .FieldName(1) = "RCH_"
'    .FieldType(1) = "N"
'    .FieldLength(1) = 11
'    .FieldDecimalCount(1) = 0
'
'    .FieldName(2) = "RCH_ID"
'    .FieldType(2) = "N"
'    .FieldLength(2) = 11
'    .FieldDecimalCount(2) = 0
  
    .fieldName(1) = "COM_ID"
    .FieldType(1) = "N"
    .FieldLength(1) = 11
    .FieldDecimalCount(1) = 0
  
    .fieldName(2) = "RCH_CODE"
    .FieldType(2) = "C"
    .FieldLength(2) = 14
    .FieldDecimalCount(2) = 0
  
    .fieldName(3) = "RCH_DATE"
    .FieldType(3) = "C"
    .FieldLength(3) = 8
    .FieldDecimalCount(3) = 0
  
    .fieldName(4) = "LEVEL"
    .FieldType(4) = "N"
    .FieldLength(4) = 5
    .FieldDecimalCount(4) = 0
  
    .fieldName(5) = "METERS"
    .FieldType(5) = "N"
    .FieldLength(5) = 12
    .FieldDecimalCount(5) = 0
  
    .fieldName(6) = "GNIS_ID"
    .FieldType(6) = "C"
    .FieldLength(6) = 8
    .FieldDecimalCount(6) = 0
  
    .fieldName(7) = "NAME"
    .FieldType(7) = "C"
    .FieldLength(7) = 99
    .FieldDecimalCount(7) = 0
  
    .fieldName(8) = "RCHID"
    .FieldType(8) = "C"
    .FieldLength(8) = 10
    .FieldDecimalCount(8) = 0
  
    .fieldName(9) = "DSRCHID"
    .FieldType(9) = "C"
    .FieldLength(9) = 10
    .FieldDecimalCount(9) = 0
  End With
End Function


