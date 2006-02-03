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

Public Module WQObservationStationsTable
  Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                         ByVal aAreaIDFieldIndex As Integer, _
                         ByVal aAreaNameFieldIndex As Integer, _
                         ByVal aSelectedAreaIndexes As Collection)

    'find WQObs layer and needed fields
    Dim lWQLayerIndex As Integer = GisUtil.LayerIndex("Water Quality Observation")
    Dim lStationFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "bstat_id")
    Dim lNameFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "location")
    Dim lAgencyFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "agency")

    'build grid source for results
    Dim lGridSource = New atcGridSource
    With lGridSource
      .Rows = 1
      .Columns = 5
      .FixedRows = 1
      .CellValue(0, 0) = "AreaID"
      .CellValue(0, 1) = "AreaName"
      .CellValue(0, 2) = "StationID"
      .CellValue(0, 3) = "StationName"
      .CellValue(0, 4) = "Agency"
    End With

    Dim i As Integer
    Dim j As Integer
    Dim lPolygonIndex As Integer
    'loop through each selected polygon and pcs point looking for overlap
    For i = 1 To GisUtil.NumFeatures(lWQLayerIndex)
      lPolygonIndex = GisUtil.PointInPolygon(lWQLayerIndex, i, aAreaLayerIndex)
      If lPolygonIndex > -1 Then
        For j = 1 To aSelectedAreaIndexes.Count
          If aSelectedAreaIndexes(j) = lPolygonIndex Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaIDFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lStationFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lAgencyFieldIndex)
            Exit For
          End If
        Next j
      End If
    Next i

    Return lGridSource
  End Function

End Module

