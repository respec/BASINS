Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module Population2000Table
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, ByVal cSelectedAreaIndexes As Collection, ByVal aOutputPath As String)

    Dim AreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim AreaIdFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aIDField)
    Dim AreaNameFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aNameField)

    Dim i As Integer
    Dim j As Integer
    Dim cAreaIds As New Collection
    Dim cAreaNames As New Collection
    Dim cTractNames As New Collection
    Dim cTractPopulation As New Collection
    Dim cTractPercent As New Collection
    Dim TractLayerIndex As Long
    Dim TractNameFieldIndex As Integer
    Dim TractPopulationFieldIndex As Integer
    Dim larea As Double
    Dim lareat As Double

    'find 2000 Census Tract layer
    For i = 1 To GisUtil.NumLayers
      If Right(FilenameNoPath(GisUtil.LayerFileName(i - 1)), 9) = "_tr00.shp" Then
        TractLayerIndex = i - 1
      End If
    Next i
    TractNameFieldIndex = GisUtil.FieldIndex(TractLayerIndex, "NAME")
    TractPopulationFieldIndex = GisUtil.FieldIndex(TractLayerIndex, "population")
    'loop through each selected polygon and each census tract looking for overlap
    For j = 1 To cSelectedAreaIndexes.Count
      For i = 1 To GisUtil.NumFeatures(TractLayerIndex)
        If GisUtil.OverlappingPolygons(TractLayerIndex, i - 1, AreaLayerIndex, cSelectedAreaIndexes(j)) Then
          'these overlap
          cAreaIds.Add(GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaIdFieldIndex))
          cAreaNames.Add((GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaNameFieldIndex)))
          cTractNames.Add(GisUtil.FieldValue(TractLayerIndex, i - 1, TractNameFieldIndex))
          cTractPopulation.Add(GisUtil.FieldValue(TractLayerIndex, i - 1, TractPopulationFieldIndex))
          'what percent of this tract is in this area?
          larea = GisUtil.AreaOverlappingPolygons(TractLayerIndex, i - 1, AreaLayerIndex, cSelectedAreaIndexes(j))
          lareat = GisUtil.FeatureArea(TractLayerIndex, i - 1)
          If lareat > 0 Then
            larea = (larea / lareat) * 100
          Else
            larea = 0.0
          End If
          cTractPercent.Add(Format(larea, "0.0"))
        End If
      Next i
    Next j

    'now write file
    Dim OutFile As Integer
    Dim ctxt As String
    OutFile = FreeFile()
    FileOpen(OutFile, aOutputPath & "2000 Population by Census Tract Table.out", OpenMode.Output)
    PrintLine(OutFile, "Watershed Characterization Report")
    PrintLine(OutFile, "  2000 Population by Census Tract Within " & aAreaLayer)
    PrintLine(OutFile, "")
    PrintLine(OutFile, "AreaID" & vbTab & "AreaName" & vbTab & "TractID" & vbTab & "Population" & vbTab & "%inArea")
    'write area ids and associated descriptions
    For i = 1 To cAreaIds.Count
      ctxt = cAreaIds(i) & vbTab & cAreaNames(i) & vbTab & _
             cTractNames(i) & vbTab & cTractPopulation(i) & vbTab & cTractPercent(i)
      PrintLine(OutFile, ctxt)
    Next i
    FileClose(OutFile)

  End Sub
End Module

