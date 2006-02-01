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
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, _
                        ByVal aSelectedAreaIndexes As Collection, ByVal aOutputPath As String, ByVal afrmOut As Object)

    'set area layer indexes
    Dim lAreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim lAreaIdFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aIDField)
    Dim lAreaNameFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aNameField)

    'find WQObs layer and needed fields
    Dim lWQLayerIndex As Integer = GisUtil.LayerIndex("Water Quality Observation")
    Dim lStationFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "bstat_id")
    Dim lNameFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "location")
    Dim lAgencyFieldIndex As Integer = GisUtil.FieldIndex(lWQLayerIndex, "agency")
        
    'build grid source for results
    Dim lGridSource = New atcGridSource
    Dim ltitle1 As String
    Dim ltitle2 As String
    ltitle1 = "Watershed Characterization Report"
    ltitle2 = "Water Quality Observation Stations Within " & aAreaLayer
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
      lPolygonIndex = GisUtil.PointInPolygon(lWQLayerIndex, i, lAreaLayerIndex)
      If lPolygonIndex > -1 Then
        For j = 1 To aSelectedAreaIndexes.Count
          If aSelectedAreaIndexes(j) = lPolygonIndex Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaIdFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lStationFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lAgencyFieldIndex)
            Exit For
          End If
        Next j
      End If
    Next i

    'write file
    SaveFileString(aOutputPath & "Water Quality Observation Stations Table.out", _
       ltitle1 & vbCrLf & "  " & ltitle2 & vbCrLf & vbCrLf & lGridSource.ToString)

    'produce result grid
    If Not afrmOut Is Nothing Then
      afrmOut.InitializeResults(ltitle1, ltitle2, lGridSource)
      afrmOut.Show()
    End If

  End Sub

End Module

