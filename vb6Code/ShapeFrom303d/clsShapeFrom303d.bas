Attribute VB_Name = "clsShapeFrom303d"
Option Explicit

Sub main()
  Dim vShpTypes As Variant, vShpType As Variant
  Dim lShpType As String
  
  Dim lShp303d As CShape_IO
  Dim lShp303dFile As String
  Dim lShp303dPath As String
  Dim lShp303dTable As clsDBF
  Dim lShp303dRecord As Long
  
  Dim lHucProc As FastCollection
  Dim lHucCurr As String
  
  Dim lRchFld As Long
  Dim i As Long
  
  On Error GoTo reportError:

  lShp303dPath = "d:\GisData\EPA 303d\Shapes"
  ChDriveDir lShp303dPath
  
  Dim lLogger As clsATCoLogger
  Set lLogger = New clsATCoLogger
  lLogger.SetFileName "CreateShapes.log"
  lLogger.DateTime = True
  lLogger.Log "StartCreate303d:Curdir:" & CurDir
  
  vShpTypes = Array("area", "point", "line")
  For Each vShpType In vShpTypes
    lLogger.Log "  Process:" & vShpType
    lShpType = vShpType
    Set lHucProc = New FastCollection
    lShp303dFile = lShp303dPath & "\rad_303d_" & Left(lShpType, 1) & "_sde"
    Set lShp303d = New CShape_IO
    lLogger.Log "  openBoundary:" & lShp303d.ShapeFileOpen(lShp303dFile & ".shp", 0) & ":RecCnt:" & lShp303d.getRecordCount
    Set lShp303dTable = New clsDBF
    lShp303dTable.OpenDBF lShp303dFile & ".dbf"
    lRchFld = lShp303dTable.FieldNumber("Rch_code")
    lLogger.Log "  RchFld:" & lRchFld
  
    For i = 1 To lShp303dTable.NumRecords
      lShp303dTable.CurrentRecord = i
      lHucCurr = Left(lShp303dTable.Value(lRchFld), 8)
      If Not (lHucProc.KeyExists(lHucCurr)) Then 'new buc, process it
        lHucProc.Add lHucCurr, lHucCurr
        ' lots more here!
      End If
    Next
    lLogger.Log "  foundHucsCnt:" & lHucProc.count
    'close files
    lShp303d.FileShutDown
    Set lShp303d = Nothing
    lShp303dTable.Clear
    Set lShp303dTable = Nothing
  Next
  Exit Sub
reportError:
  lLogger.Log "*** ERROR " & Err.Description
  Err.Clear
  Resume Next
  
End Sub
