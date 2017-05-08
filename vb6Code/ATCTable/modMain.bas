Attribute VB_Name = "modMain"
Option Explicit

Private Const TLIDfield As Long = 3
Private Const RTSQfield As Long = 4
Private Const LatLongMultiplier As Double = 1 / 1000000 'Lat/Long are stored in TIGER multiplied by 100,000

Public Sub Main()
  frmTIGER.Show
'  If Len(Command) > 0 Then
'    Dim Filename As String
'    Filename = ReplaceString(Command, """", "")
'    If FileExists(Filename) Then frmTIGER.OpenTable Filename
'  End If
End Sub

Public Sub SaveAsNewBin(aTable As clsATCTable)
  Dim iField As Long
  Dim iRecord As Long
  Dim newTable As clsATCTable
  Set newTable = New clsATCTableBin
  With newTable
    .NumFields = aTable.NumFields
  
    For iField = 1 To .NumFields
      .FieldName(iField) = aTable.FieldName(iField)
    Next
    '"BSTAT_ID"
    .FieldType(1) = "C":    .FieldLength(1) = 28
    '"ID_SAMP"
    .FieldType(2) = "C":    .FieldLength(2) = 10
    '"ID"
    .FieldType(3) = "C":    .FieldLength(3) = 5
    '"TIME"
    .FieldType(4) = 2:      .FieldLength(4) = 2
    '"DEPTH"
    .FieldType(5) = 32:     .FieldLength(5) = 4
    '"PARM"
    .FieldType(6) = "C":    .FieldLength(6) = 5
    '"REMARK"
    .FieldType(7) = "C":    .FieldLength(7) = 1
    '"VALUE"
    .FieldType(8) = 32:     .FieldLength(8) = 4
    '"BCU"
    .FieldType(9) = "C":    .FieldLength(9) = 8
    '"DATE"
    .FieldType(10) = 4:     .FieldLength(10) = 4
  
    .NumRecords = aTable.NumRecords
    For iRecord = 1 To .NumRecords
      .CurrentRecord = iRecord
      aTable.CurrentRecord = iRecord
      For iField = 1 To .NumFields
        .Value(iField) = aTable.Value(iField)
      Next
    Next
    .WriteFile FilenameNoExt(aTable.Filename) & ".abt"
  End With
End Sub

Public Sub WriteShpFromTiger(rt1 As clsATCTable, rt2 As clsATCTable, SaveAs As String)
  Dim iField As Long
  Dim iFromLonField As Long
  Dim iRecord As Long
  Dim usedRT2 As Boolean
  Dim newSHP As CShape_IO
  Dim newTable As clsATCTable
  Set newTable = New clsATCTableDBF
  With newTable
    .NumFields = rt1.NumFields - 4 'Omit start/end lat/long from table
      
    For iField = 1 To .NumFields
      .FieldName(iField) = rt1.FieldName(iField)
      .FieldType(iField) = rt1.FieldType(iField)
      .FieldLength(iField) = rt1.FieldLength(iField)
    Next
    iFromLonField = rt1.FieldNumber("FRLONG")
    
    .NumRecords = rt1.NumRecords

    Set newSHP = New CShape_IO
    newSHP.CreateNewShape SaveAs, 3
    
    For iRecord = 1 To .NumRecords
      .CurrentRecord = iRecord
      rt1.CurrentRecord = iRecord
      For iField = 1 To .NumFields
        .Value(iField) = rt1.Value(iField)
      Next
            
      newSHP.putPoly 0, OneShpFromTiger(rt1, iFromLonField, rt2)
      
      If Not usedRT2 Then
        If rt2.CurrentRecord > 1 Then usedRT2 = True
      End If
      
    Next
    If rt2.CurrentRecord <> 1 Then MsgBox rt2.Filename & " CurrentRecord = " & rt2.CurrentRecord & " of " & rt2.NumRecords
    If Not usedRT2 Then
      If rt2.NumRecords > 1 Then
        MsgBox rt2.Filename & " either contains only one shape or was not used"
      End If
    End If
    newSHP.FileShutDown
    .WriteFile FilenameNoExt(SaveAs) & ".dbf"
  End With

End Sub

Private Function OneShpFromTiger(aRT1 As clsATCTable, aFromLonField As Long, aRT2 As clsATCTable) As T_shpPoly
  Dim retval As T_shpPoly
  Dim TLID As String
  Dim startSearch As Long
  Dim numNewPoints As Long
  Dim iField As Long
  Dim iPoint As Long
  Dim iNewPoint As Long
  Dim x As Double
  Dim y As Double
  
  TLID = aRT1.Value(TLIDfield)

  retval.ShapeType = 3 'Assume all are polyline
  
  retval.NumParts = 1
  ReDim retval.Parts(0)
  
  retval.NumPoints = 2
  ReDim retval.thePoints(0 To retval.NumPoints - 1)
  x = aRT1.Value(aFromLonField) * LatLongMultiplier
  y = aRT1.Value(aFromLonField + 1) * LatLongMultiplier

  retval.thePoints(0).x = x
  retval.thePoints(0).y = y

  retval.Box.xMax = x
  retval.Box.xMin = x
  retval.Box.yMax = y
  retval.Box.yMin = y

  iPoint = 1
  
  startSearch = 1
  'Find is needed if we can't count on RT2 being in the same order as RT1
  'If aRT2.FindFirst(TLIDfield, TLID) Then
  '  Do
  While aRT2.Value(TLIDfield) = TLID
      For numNewPoints = 2 To 10
        Select Case aRT2.Value(RTSQfield + numNewPoints * 2)
          Case "+00000000", "": Exit For
        End Select
      Next
      numNewPoints = numNewPoints - 1
      retval.NumPoints = retval.NumPoints + numNewPoints
      ReDim Preserve retval.thePoints(0 To retval.NumPoints - 1)
      
      iField = RTSQfield
      For iNewPoint = 1 To numNewPoints
        iField = iField + 1
        x = aRT2.Value(iField) * LatLongMultiplier
        iField = iField + 1
        y = aRT2.Value(iField) * LatLongMultiplier
        GoSub UpdateMinMax
      Next
      aRT2.MoveNext
  Wend
  '  Loop While aRT2.Value(TLIDfield) = TLID
  'End If

  If iPoint <> retval.NumPoints - 1 Then
    MsgBox "Mismatched number of points - iPoint = " & iPoint & ", retval.NumPoints = " & retval.NumPoints
  End If
  
  x = aRT1.Value(aFromLonField + 2) * LatLongMultiplier
  y = aRT1.Value(aFromLonField + 3) * LatLongMultiplier
  
  GoSub UpdateMinMax
  
  OneShpFromTiger = retval
  
  Exit Function
  
UpdateMinMax:
  retval.thePoints(iPoint).x = x
  retval.thePoints(iPoint).y = y
  iPoint = iPoint + 1
  
  If x > retval.Box.xMax Then
    retval.Box.xMax = x
  ElseIf x < retval.Box.xMin Then
    retval.Box.xMin = x
  End If

  If y > retval.Box.yMax Then
    retval.Box.yMax = y
  ElseIf y < retval.Box.yMin Then
    retval.Box.yMin = y
  End If
  Return
End Function

Public Sub SaveAsNewDBF(aTable As clsATCTable)
  Dim iField As Long
  Dim iRecord As Long
  Dim newTable As clsATCTable
  Set newTable = New clsATCTableDBF
  With newTable
    .NumFields = aTable.NumFields
      
    For iField = 1 To .NumFields
      .FieldName(iField) = aTable.FieldName(iField)
      .FieldType(iField) = aTable.FieldType(iField)
      .FieldLength(iField) = aTable.FieldLength(iField)
    Next
      
'    .FieldName(1) = "BSTAT_ID"
'    .FieldType(1) = "C"
'    .FieldLength(1) = 28
'
'    .FieldName(2) = "ID_SAMP"
'    .FieldType(2) = "C"
'    .FieldLength(2) = 10
'
'    .FieldName(3) = "ID"
'    .FieldType(3) = "C"
'    .FieldLength(3) = 5
'
'    .FieldName(4) = "TIME"
'    .FieldType(4) = "C"
'    .FieldLength(4) = 4
'
'    .FieldName(5) = "DEPTH"
'    .FieldType(5) = "N"
'    .FieldLength(5) = 20
'
'    .FieldName(6) = "PARM"
'    .FieldType(6) = "C"
'    .FieldLength(6) = 5
'
'    .FieldName(7) = "REMARK"
'    .FieldType(7) = "C"
'    .FieldLength(7) = 1
'
'    .FieldName(8) = "VALUE"
'    .FieldType(8) = "N"
'    .FieldLength(8) = 20
'
'    .FieldName(9) = "BCU"
'    .FieldType(9) = "C"
'    .FieldLength(9) = 8
'
'    .FieldName(10) = "DATE"
'    .FieldType(10) = "C"
'    .FieldLength(10) = 8
  
    .NumRecords = aTable.NumRecords

    For iRecord = 1 To .NumRecords
      .CurrentRecord = iRecord
      aTable.CurrentRecord = iRecord
      For iField = 1 To .NumFields
        .Value(iField) = aTable.Value(iField)
      Next
    Next
    .WriteFile FilenameNoExt(aTable.Filename) & ".new.dbf"
  End With
End Sub


