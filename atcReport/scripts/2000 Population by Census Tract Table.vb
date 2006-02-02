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

Public Module Population2000Table
  Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                             ByVal aAreaIDFieldIndex As Integer, _
                             ByVal aAreaNameFieldIndex As Integer, _
                             ByVal aSelectedAreaIndexes As Collection)
    'find 2000 Census Tract layer
    Dim i As Integer
    Dim lTractLayerIndex As Long
    Dim lTractNameFieldIndex As Integer
    Dim lTractPopulationFieldIndex As Integer
    For i = 1 To GisUtil.NumLayers
      If Right(FilenameNoPath(GisUtil.LayerFileName(i - 1)), 9) = "_tr00.shp" Then
        lTractLayerIndex = i - 1
      End If
    Next i

    If lTractLayerIndex > 0 Then
      lTractNameFieldIndex = GisUtil.FieldIndex(lTractLayerIndex, "NAME")
      lTractPopulationFieldIndex = GisUtil.FieldIndex(lTractLayerIndex, "population")

      'build grid source for results
      Dim lGridSource = New atcGridSource
      With lGridSource
        .Rows = 1
        .Columns = 5
        .FixedRows = 1
        .CellValue(0, 0) = "AreaID"
        .CellValue(0, 1) = "AreaName"
        .CellValue(0, 2) = "TractID"
        .CellValue(0, 3) = "Population"
        .CellValue(0, 4) = "%inArea"
      End With

      Dim larea As Double
      Dim lareat As Double
      Dim j As Integer
      'loop through each selected polygon and each census tract looking for overlap
      For j = 1 To aSelectedAreaIndexes.Count
        For i = 1 To GisUtil.NumFeatures(lTractLayerIndex)
          If GisUtil.OverlappingPolygons(lTractLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaIDFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lTractLayerIndex, i - 1, lTractNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lTractLayerIndex, i - 1, lTractPopulationFieldIndex)
            'what percent of this tract is in this area?
            larea = GisUtil.AreaOverlappingPolygons(lTractLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j))
            lareat = GisUtil.FeatureArea(lTractLayerIndex, i - 1)
            If lareat > 0 Then
              larea = (larea / lareat) * 100
            Else
              larea = 0.0
            End If
            lGridSource.CellValue(lGridSource.rows - 1, 4) = Format(larea, "0.0")
          End If
        Next i
      Next j
      Return lGridSource
    Else
      Logger.Dbg("No Tract Layer Found")
    End If

  End Function

End Module

