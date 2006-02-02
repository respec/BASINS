Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports MapWindow.Interfaces

Public Module PCSFacilityTable
  Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                             ByVal aAreaIDFieldIndex As Integer, _
                             ByVal aAreaNameFieldIndex As Integer, _
                             ByVal aSelectedAreaIndexes As Collection)

    'find PCS layer and needed fields
    Dim lPCSLayerIndex As Integer = GisUtil.LayerIndex("Permit Compliance System")
    Dim lNPDESFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "npdes")
    Dim lFacilityFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "fac_name")
    Dim lSicFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "sic2")
    Dim lSicdFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "sic2d")
    Dim lCityFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "city")
    Dim lRecwaterFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "rec_water")

    'build grid source for results
    Dim lGridSource = New atcGridSource
    With lGridSource
      .Rows = 1
      .Columns = 8
      .FixedRows = 1
      .CellValue(0, 0) = "AreaID"
      .CellValue(0, 1) = "AreaName"
      .CellValue(0, 2) = "NPDES"
      .CellValue(0, 3) = "Facility Name"
      .CellValue(0, 4) = "SIC"
      .CellValue(0, 5) = "SIC Name"
      .CellValue(0, 6) = "City"
      .CellValue(0, 7) = "Waterbody"
    End With

    Dim i As Integer
    Dim j As Integer
    Dim lPolygonIndex As Integer
    'loop through each selected polygon and pcs point looking for overlap
    For i = 1 To GisUtil.NumFeatures(lPCSLayerIndex)
      lPolygonIndex = GisUtil.PointInPolygon(lPCSLayerIndex, i, aAreaLayerIndex)
      If lPolygonIndex > -1 Then
        For j = 1 To aSelectedAreaIndexes.Count
          If aSelectedAreaIndexes(j) = lPolygonIndex Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaIDFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lNPDESFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lFacilityFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lSicFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 5) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lSicdFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 6) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lCityFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 7) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lRecwaterFieldIndex)
            Exit For
          End If
        Next j
      End If
    Next i

    Return lGridSource
  End Function

End Module

