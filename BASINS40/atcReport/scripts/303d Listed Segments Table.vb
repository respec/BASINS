Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module ListedSegmentsTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, _
                        ByVal aSelectedAreaIndexes As Collection, ByVal aOutputPath As String, ByVal afrmOut As Object)

    'set area layer indexes
    Dim lAreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim lAreaIdFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aIDField)
    Dim lAreaNameFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aNameField)

    'find 303d line layer
    Dim lImpairedLayerIndex As Long
    Dim lImpairedIdFieldIndex As Integer
    Dim lImpairedNameFieldIndex As Integer
    Dim lImpairmentFieldIndex As Integer
    lImpairedLayerIndex = GisUtil.LayerIndex("303d List - Lines")
    lImpairedIdFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "RCH_CODE")
    lImpairedNameFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "WBODY_NAME")
    lImpairmentFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "ST_IMPAIR")

    'build grid source for results
    Dim lGridSource = New atcGridSource
    Dim ltitle1 As String
    Dim ltitle2 As String
    ltitle1 = "Watershed Characterization Report"
    ltitle2 = "303d Listed Segments Within " & aAreaLayer
    With lGridSource
      .Rows = 1
      .Columns = 5
      .FixedRows = 1
      .CellValue(0, 0) = "AreaID"
      .CellValue(0, 1) = "AreaName"
      .CellValue(0, 2) = "SegmentID"
      .CellValue(0, 3) = "Waterbody Name"
      .CellValue(0, 4) = "Impairment"
    End With
    
    Dim i As Integer
    Dim j As Integer
    'loop through each selected polygon and each 303d feature looking for overlap
    For j = 1 To aSelectedAreaIndexes.Count
      For i = 1 To GisUtil.NumFeatures(lImpairedLayerIndex)
        If GisUtil.LineInPolygon(lImpairedLayerIndex, i, lAreaLayerIndex, aSelectedAreaIndexes(j)) Then
          'these overlap
          lGridSource.rows = lGridSource.rows + 1
          lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(j), lAreaIdFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(j), lAreaNameFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedIdFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedNameFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairmentFieldIndex)
        End If
      Next i
    Next j

    'now check 303d polygon layer
    lImpairedLayerIndex = GisUtil.LayerIndex("303d List - Areas")
    lImpairedIdFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "RCH_CODE")
    lImpairedNameFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "WBODY_NAME")
    lImpairmentFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "ST_IMPAIR")
    'loop through each selected polygon and each 303d feature looking for overlap
    For j = 1 To aSelectedAreaIndexes.Count
      For i = 1 To GisUtil.NumFeatures(lImpairedLayerIndex)
        If GisUtil.OverlappingPolygons(lImpairedLayerIndex, i - 1, lAreaLayerIndex, aSelectedAreaIndexes(j)) Then
          'these overlap
          lGridSource.rows = lGridSource.rows + 1
          lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(j), lAreaIdFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(j), lAreaNameFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedIdFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedNameFieldIndex)
          lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairmentFieldIndex)
        End If
      Next i
    Next j

    'write file
    SaveFileString(aOutputPath & "303d Listed Segments Table.out", _
       ltitle1 & vbCrLf & "  " & ltitle2 & vbCrLf & vbCrLf & lGridSource.ToString)

    'produce result grid
    If Not afrmOut Is Nothing Then
      afrmOut.InitializeResults(ltitle1, ltitle2, lGridSource)
      afrmOut.Show()
    End If

  End Sub
End Module
