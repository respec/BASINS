Attribute VB_Name = "mod"
Option Explicit

'keyFieldName is the field name in the DBFs that is unique.
'merge discards DBF records and associated shapes that
'have duplicate values in this field.
'If keyFieldName is blank, no duplicate checking will occur
Public Sub merge(shpFileNames() As String, _
                 ByVal keyFieldName As String, _
                 ByVal newBaseFilename As String)
  Dim firstInput As Long
  Dim lastInput As Long
  Dim curInput As Long
  Dim dbfIn() As clsDBF
  Dim shpIn() As Integer 'file handles
  Dim shxIn() As Integer 'file handles
  Dim dbfOut As clsDBF
  Dim shpOut As Integer
  Dim shxOut As Integer
  Dim baseFilename As String
  Dim keyField As Integer

  Dim FileLength As Long
  Dim ShapeType As Long
  Dim lowX#, lowY#, uppX#, uppY#
  
  Dim NewFileLength As Long
  Dim NewShapeType As Long
  Dim NewlowX#, NewlowY#, NewuppX#, NewuppY#
  
  Dim i As Long

  newBaseFilename = FilenameNoExt(newBaseFilename)
  firstInput = LBound(shpFileNames)
  lastInput = UBound(shpFileNames)
  ReDim dbfIn(firstInput To lastInput)
  ReDim shpIn(firstInput To lastInput)
  ReDim shxIn(firstInput To lastInput)
  
  baseFilename = FilenameNoExt(shpFileNames(firstInput))
  FileCopy baseFilename & ".dbf", newBaseFilename & ".dbf"
  FileCopy baseFilename & ".shp", newBaseFilename & ".shp"
  FileCopy baseFilename & ".shx", newBaseFilename & ".shx"
  
  Set dbfOut = New clsDBF:
  dbfOut.OpenDBF newBaseFilename & ".dbf"
  keyField = dbfOut.FieldNumber(keyFieldName)
  dbfOut.CurrentRecord = dbfOut.NumRecords
  
  shpOut = FreeFile(0): Open newBaseFilename & ".shp" For Binary As shpOut
  shxOut = FreeFile(0): Open newBaseFilename & ".shx" For Binary As shxOut
  
  ReadShapeHeader shpOut, NewFileLength, NewShapeType, NewlowX, NewlowY, NewuppX, NewuppY
  Seek #shpOut, LOF(shpOut) + 1 'Skip past data already in this file
  Seek #shxOut, LOF(shxOut) + 1
  
  For curInput = firstInput + 1 To lastInput
    
    baseFilename = FilenameNoExt(shpFileNames(curInput))
    
    Set dbfIn(curInput) = New clsDBF: dbfIn(curInput).OpenDBF baseFilename & ".dbf"
    shpIn(curInput) = FreeFile(0): Open baseFilename & ".shp" For Binary As shpIn(curInput)
    shxIn(curInput) = FreeFile(0): Open baseFilename & ".shx" For Binary As shxIn(curInput)
    
    ReadShapeHeader shpIn(curInput), FileLength, ShapeType, lowX, lowY, uppX, uppY
    If ShapeType <> NewShapeType Then
      MsgBox "Different type of shapes in files " & vbCr _
           & baseFilename & ".shp" & vbCr _
           & "and" & vbCr _
           & FilenameNoExt(shpFileNames(firstInput)) & ".shp'", vbCritical, "Cannot merge shape files"
      Close shpOut
      Close shxOut
      Set dbfOut = Nothing
      Kill newBaseFilename & ".shp"
      Kill newBaseFilename & ".shx"
      Kill newBaseFilename & ".dbf"
      Exit Sub
    End If
  Next
  
  For curInput = firstInput + 1 To lastInput
    With dbfIn(curInput)
      For i = 1 To .NumRecords
        .CurrentRecord = i
        If keyField > 0 Then 'Check for a duplicate record already in dbfOut
          If dbfOut.FindFirst(keyField, .Value(keyField)) Then GoTo NextRecord
        End If
        dbfOut.CurrentRecord = dbfOut.CurrentRecord + 1
        dbfOut.Record = .Record 'Copy this record in the DBF
        'Append current shape from shpIn(curInput) to shpOut and shxOut
        NewFileLength = NewFileLength + CopyShape(ShapeType, .CurrentRecord, shpIn(curInput), shxIn(curInput), _
                                                       dbfOut.CurrentRecord, shpOut, shxOut, _
                                                       NewlowX, NewlowY, NewuppX, NewuppY)
        
NextRecord:
      Next
    End With
  Next
  Debug.Print NewFileLength, LOF(shpOut) / 2
  WriteShapeHeader shpOut, LOF(shpOut) / 2, NewShapeType, NewlowX, NewlowY, NewuppX, NewuppY
  WriteShapeHeader shxOut, LOF(shxOut) / 2, NewShapeType, NewlowX, NewlowY, NewuppX, NewuppY
  dbfOut.WriteDBF newBaseFilename & ".dbf"
  Close shpOut
  Close shxOut
  For curInput = firstInput + 1 To lastInput
    Close shpIn(curInput)
    Close shxIn(curInput)
  Next
  'TODO: update shape file header for shpOut and shxOut to reflect increased number of shapes
End Sub

'Returns number of 2-byte words written to ToShp
Private Function CopyShape(ShapeType As Long, FromIndex As Long, FromShp As Integer, FromShx As Integer, _
                                           ToIndex As Long, ToShp As Integer, ToShx As Integer, _
                                           ByRef NewlowX#, ByRef NewlowY#, ByRef NewuppX#, ByRef NewuppY#)
  Dim X As Double, Y As Double
  Select Case ShapeType 'ShapeType: 0=null, 1=point, 3=arc, 5=polygon, 8=multipoint
    Case 1
      Seek #FromShp, 101 + (FromIndex - 1) * 28 + 12
      ReadShapePoint FromShp, X, Y
      Debug.Print X, Y
      WriteShapePointAll ToShp, ToIndex, X, Y
      WriteShapePointIndex ToShx, ToIndex
      CopyShape = 14
      If X < NewlowX Then NewlowX = X
      If Y < NewlowY Then NewlowY = Y
      If X > NewuppX Then NewuppX = X
      If Y > NewuppY Then NewuppY = Y
    Case Else
      Debug.Print "Unsupported shape type " & ShapeType & " not copied"
  End Select
End Function
