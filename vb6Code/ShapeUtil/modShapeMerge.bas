Attribute VB_Name = "modShapeMerge"
Option Explicit

'keyField: the field name or number in the DBFs that is unique.
'   Duplicate values in this field will cause DBF records and associated
'   shapes that occur later in shpFileNames to be discarded.
'   If keyField is 0, duplicate checking will use all fields
'   If keyField is -1, no duplicate checking will occur
'newBaseFilename: the path and filename without extension of the
'   destination merged dbf, shp, and shx. If the file already exists, its
'   current contents will be merged with the other file(s) in shpFileNames
'shpFileNames: array of full path names of .shp files to merge
Public Sub ShapeMerge(keyField As String, newBaseFilename As String, shpFileNames() As String)
  Dim duplicate As Boolean
  Dim newBaseCopied As Boolean
  Dim successful As Boolean
  Dim firstInput As Long
  Dim lastInput As Long
  Dim curInput As Long
  Dim dbfIn() As clsDBF
  Dim shpIn() As CShape_IO
  Dim dbfOut As clsDBF
  Dim shpOut As CShape_IO
  Dim baseFileName As String
  Dim fieldNum As Integer
  Dim minFields As Integer
  Dim keyFieldNum As Integer
  Dim outRecsBeforeThisInput As Long 'Skip searching for matches in records just added from the same file
  Dim outRecsBeforeAllInput As Long 'Skip writing if we never added any records

  Dim FileLength As Long
  Dim ShapeType As Long
  Dim lowX#, lowY#, uppX#, uppY#
  
  Dim NewFileLength As Long
  Dim NewShapeType As Long
  Dim NewHeader As T_MainFileHeader
  Dim shpInHeader As T_MainFileHeader
  Dim starttime As Single
  
  Dim i As Long, j As Long, mergeRecordCount As Long
  Dim canCopyRecords As Boolean
  
  'On Error GoTo ErrHand

  newBaseFilename = FilenameNoExt(newBaseFilename)
  
  firstInput = LBound(shpFileNames)
  lastInput = UBound(shpFileNames)
  
  'Array may be dimensioned 0 to UBound, but only populated 1 to UBound
  If Len(shpFileNames(firstInput)) = 0 Then firstInput = firstInput + 1
  
  ReDim dbfIn(firstInput To lastInput)
  ReDim shpIn(firstInput To lastInput)

  For curInput = firstInput To lastInput
    'TODO check to see if sets of files are identical before any compares by record
    baseFileName = FilenameNoExt(shpFileNames(curInput))
    If Len(baseFileName) = 0 Then
      'skip blank entries in shpFileNames
    ElseIf LCase(baseFileName) = LCase(newBaseFilename) Then
      'skip the destination if it is named in shpFileNames
    ElseIf Not FileExists(baseFileName & ".dbf") Then
      LogMsg "Could not find " & baseFileName & ".dbf, so could not merge"
    ElseIf Not FileExists(baseFileName & ".shp") Then
      LogMsg "Could not find " & baseFileName & ".shp, so could not merge"
      
    'If we are merging an empty file into an existing file, might as well skip then empty file
    ElseIf FileLen(baseFileName & ".shp") <= 100 And FileExists(newBaseFilename & ".shp") Then
      LogDbg "Skipping empty shape file '" & baseFileName & ".shp'"
    
    ElseIf Not FileExists(newBaseFilename & ".shp") Then
      'If destination shape file does not exist, copy first shape file to merge
CopyOverNew:
      LogDbg "Copying " & baseFileName & " to " & newBaseFilename
      FileCopy baseFileName & ".dbf", newBaseFilename & ".dbf"
      FileCopy baseFileName & ".shp", newBaseFilename & ".shp"
      FileCopy baseFileName & ".shx", newBaseFilename & ".shx"
      newBaseCopied = True
    Else
      If dbfOut Is Nothing Then
        Set dbfOut = New clsDBF
        dbfOut.OpenDBF newBaseFilename & ".dbf"
        If dbfOut.NumRecords = 0 Then 'Destination layer is empty, delete it and copy over it
          LogDbg "Destination " & newBaseFilename & " empty, deleting and copying over it"
          Set dbfOut = Nothing
          If FileExists(newBaseFilename & ".shp") Then Kill newBaseFilename & ".shp"
          If FileExists(newBaseFilename & ".shx") Then Kill newBaseFilename & ".shx"
          If FileExists(newBaseFilename & ".dbf") Then Kill newBaseFilename & ".dbf"
          GoTo CopyOverNew
        ElseIf IsNumeric(keyField) Then
          keyFieldNum = CInt(keyField)
        Else
          keyFieldNum = dbfOut.FieldNumber(keyField)
        End If
        dbfOut.CurrentRecord = dbfOut.NumRecords
        outRecsBeforeAllInput = dbfOut.NumRecords
        minFields = dbfOut.NumFields
        
        Set shpOut = New CShape_IO
        shpOut.ShapeFileOpen newBaseFilename & ".shp", READWRITEFLAG.Readwrite
        NewHeader = shpOut.getShapeHeader
        NewShapeType = NewHeader.ShapeType
      End If
      
      Set dbfIn(curInput) = New clsDBF
      With dbfIn(curInput)
        Set .Logger = gLogger
        .OpenDBF baseFileName & ".dbf"
        'TODO check to make sure fields are exactly the same as dbfOut
        If .NumFields <> dbfOut.NumFields Then
          LogDbg "Different number of fields:" & vbCr _
                 & newBaseFilename & ".dbf = " & dbfOut.NumFields & vbCr _
                 & baseFileName & ".dbf = " & .NumFields & vbCr & vbCr _
                 & "Skipping last " & Abs(.NumFields - dbfOut.NumFields) & " field(s)"
          If .NumFields < minFields Then minFields = .NumFields
'          LogMsg "Different number of fields:" & vbCr _
'                 & newBaseFilename & ".dbf = " & dbfOut.numFields & vbCr _
'                 & baseFileName & ".dbf = " & .numFields & vbCr & vbCr _
'                 & "Cannot merge shape files"
'          Exit Sub
        End If
      End With
      
      Set shpIn(curInput) = New CShape_IO
      With shpIn(curInput)
        .ShapeFileOpen baseFileName & ".shp", READWRITEFLAG.ReadOnly
        If .getRecordCount <> dbfIn(curInput).NumRecords Then
          LogMsg "Unequal number of records:" & vbCr _
                & baseFileName & ".shp = " & .getRecordCount & vbCr _
                & baseFileName & ".dbf = " & dbfIn(curInput).NumRecords & vbCr & vbCr _
                & "Cannot merge shape files"
          Exit Sub
        End If
        shpInHeader = .getShapeHeader
        If shpInHeader.ShapeType <> NewShapeType Then
          LogMsg "Different type of shapes in files: " & vbCr _
                       & newBaseFilename & ".shp = " & NewShapeType & vbCr & vbCr _
                       & baseFileName & ".shp = " & shpInHeader.ShapeType & vbCr & vbCr _
                       & "Cannot merge shape files"
          Exit Sub
        End If
      End With
    End If
  Next
  
  If Not dbfOut Is Nothing Then
    For curInput = firstInput To lastInput
      If Not dbfIn(curInput) Is Nothing And Not shpIn(curInput) Is Nothing Then
        starttime = Timer
        With dbfIn(curInput)
          If .NumRecords < 1 Then
            LogDbg "No records available in " & FilenameNoExt(dbfIn(curInput).Filename)
          ElseIf .NumRecords <> shpIn(curInput).getRecordCount Then
              baseFileName = FilenameNoExt(shpFileNames(curInput))
              LogMsg "Cannot merge - unequal number of records in " & vbCr _
                    & baseFileName & ".shp (" & shpIn(curInput).getRecordCount & ") and " & vbCr _
                    & baseFileName & ".dbf (" & .NumRecords & ")"
          Else
            mergeRecordCount = 0
            LogDbg "Merging " & FilenameNoExt(dbfIn(curInput).Filename) & " into " & newBaseFilename
            outRecsBeforeThisInput = dbfOut.NumRecords
            If UBound(.record) = UBound(dbfOut.record) Then
              canCopyRecords = True
            Else
              canCopyRecords = False
            End If
            For i = 1 To .NumRecords
              .CurrentRecord = i
              Select Case keyFieldNum
                Case -1:   duplicate = False
                Case 0:    duplicate = dbfOut.FindRecord(.record, 1, outRecsBeforeThisInput)
                Case Else: duplicate = dbfOut.FindFirst(keyFieldNum, .Value(keyFieldNum), 1, outRecsBeforeThisInput)
              End Select
              If duplicate Then
                'LogDbg "Merge:Skipping matching record in " & shpFileNames(curInput) & " - " & I
                'For j = 1 To .NumFields
                '  Debug.Print .FieldName(j), .Value(j), dbfOut.Value(j)
                'Next
              Else
                dbfOut.CurrentRecord = dbfOut.NumRecords + 1
                If canCopyRecords Then
                  dbfOut.record = .record 'Copy this record in the DBF
                Else
                  For fieldNum = 1 To minFields
                    dbfOut.Value(fieldNum) = .Value(fieldNum)
                  Next
                End If
                'Append current shape from shpIn(curInput) to shpOut
                'Debug.Print ".";
                CopyShape NewShapeType, .CurrentRecord, shpIn(curInput), shpOut
                mergeRecordCount = mergeRecordCount + 1
              End If
              DoEvents
            Next
                        
            LogDbg "Added " & mergeRecordCount & _
                  " records of " & .NumRecords _
                        & " to " & outRecsBeforeThisInput _
                  & " yielding " & dbfOut.NumRecords _
                        & " in " & Format(Timer - starttime, "#.### sec")
          End If
        End With
      End If
    Next
    If dbfOut.NumRecords > outRecsBeforeAllInput Then
      dbfOut.WriteDBF newBaseFilename & ".dbf"
    End If
  End If
  successful = True
  
  GoSub CleanUp
  Exit Sub

ErrHand:
  GoSub CleanUp
  If Err.Source = "Merge" Then
    Err.Raise Err.Number, "ShapeUtil Merge", Err.Description
  Else
    Err.Raise Err.Number, "ShapeUtil Merge", Err.Description & " (" & Err.Source & ")"
  End If
  Exit Sub
  
CleanUp:
  On Error Resume Next 'If there is trouble killing files, just leave them
  If Not shpOut Is Nothing Then shpOut.FileShutDown
  For curInput = firstInput To lastInput
    If Not shpIn(curInput) Is Nothing Then shpIn(curInput).FileShutDown
    Set shpIn(curInput) = Nothing
    Set dbfIn(curInput) = Nothing
    baseFileName = FilenameNoExt(shpFileNames(curInput))
    If FileExists(baseFileName & ".shp") Then Kill baseFileName & ".shp"
    If FileExists(baseFileName & ".shx") Then Kill baseFileName & ".shx"
    If FileExists(baseFileName & ".dbf") Then Kill baseFileName & ".dbf"
  Next
  If Not successful And newBaseCopied Then
    Kill newBaseFilename & ".dbf"
    Kill newBaseFilename & ".shp"
    Kill newBaseFilename & ".shx"
  End If
  Return

End Sub

Private Sub CopyShape(ShapeType As Long, FromIndex As Long, _
                      FromShp As CShape_IO, ToShp As CShape_IO)
  Select Case ShapeType 'ShapeType: 0=null, 1=Point, 3=PolyLine, 5=Polygon, 8=MultiPoint
    Case typePoint:
      ToShp.putXYPoint 0, FromShp.getXYPoint(FromIndex)
    'TODO: typePointZ, typePointM
    Case typePolyline, typePolygon, typeMultipoint, _
         typePolyLineZ, typePolygonZ, typeMultiPointZ, _
         typePolyLineM, typePolygonM, typeMultiPointM, typeMultiPatch
      ToShp.putPoly 0, FromShp.getPoly(FromIndex)
    Case Else
      LogDbg "CopyShape:Unsupported shape type " & ShapeType & " not copied"
  End Select
End Sub

Private Sub LogDbg(msg As String)
  If gLogger Is Nothing Then
    Debug.Print msg
  Else
    gLogger.Log msg
  End If
End Sub

Private Sub LogMsg(msg As String)
  If gLogger Is Nothing Then
    MsgBox msg, vbCritical, "Shape Merge"
  Else
    gLogger.LogMsg msg, "Shape Merge"
  End If
End Sub
