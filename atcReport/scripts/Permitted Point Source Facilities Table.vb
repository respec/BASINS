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

Public Module PCSFacilityTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, _
                        ByVal aSelectedAreaIndexes As Collection, ByVal aOutputPath As String, ByVal afrmOut As Object)

    'set area layer indexes
    Dim lAreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim lAreaIdFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aIDField)
    Dim lAreaNameFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aNameField)

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
    Dim ltitle1 As String
    Dim ltitle2 As String
    ltitle1 = "Watershed Characterization Report"
    ltitle2 = "Permitted Point Source Facilities Within " & aAreaLayer
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
      lPolygonIndex = GisUtil.PointInPolygon(lPCSLayerIndex, i, lAreaLayerIndex)
      If lPolygonIndex > -1 Then
        For j = 1 To aSelectedAreaIndexes.Count
          If aSelectedAreaIndexes(j) = lPolygonIndex Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaIdFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaNameFieldIndex)
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

    'write file
    SaveFileString(aOutputPath & "Permitted Point Source Facilities Table.out", _
       ltitle1 & vbCrLf & "  " & ltitle2 & vbCrLf & vbCrLf & lGridSource.ToString)

    'produce result grid
    If Not afrmOut Is Nothing Then
      afrmOut.InitializeResults(ltitle1, ltitle2, lGridSource)
      afrmOut.Show()
    End If

  End Sub

End Module

