Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module ListedSegmentsTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, ByVal cSelectedAreaIndexes As Collection, ByVal aOutputPath As String)

    Dim AreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim AreaIdFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aIDField)
    Dim AreaNameFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aNameField)

    Dim i As Integer
    Dim j As Integer
    Dim cAreaIds As New Collection
    Dim cAreaNames As New Collection
    Dim cImpairedIds As New Collection
    Dim cImpairedNames As New Collection
    Dim cImpairment As New Collection
    Dim ImpairedLayerIndex As Long
    Dim ImpairedIdFieldIndex As Integer
    Dim ImpairedNameFieldIndex As Integer
    Dim ImpairmentFieldIndex As Integer

    'first check 303d line layer
    ImpairedLayerIndex = GisUtil.LayerIndex("303d List - Lines")
    ImpairedIdFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "RCH_CODE")
    ImpairedNameFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "WBODY_NAME")
    ImpairmentFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "ST_IMPAIR")
    'loop through each selected polygon and each 303d feature looking for overlap
    For j = 1 To cSelectedAreaIndexes.Count
      For i = 1 To GisUtil.NumFeatures(ImpairedLayerIndex)
        If GisUtil.LineInPolygon(ImpairedLayerIndex, i, AreaLayerIndex, cSelectedAreaIndexes(j)) Then
          'these overlap
          cAreaIds.Add(GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaIdFieldIndex))
          cAreaNames.Add((GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaNameFieldIndex)))
          cImpairedIds.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairedIdFieldIndex))
          cImpairedNames.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairedNameFieldIndex))
          cImpairment.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairmentFieldIndex))
        End If
      Next i
    Next j

    'now check 303d polygon layer
    ImpairedLayerIndex = GisUtil.LayerIndex("303d List - Areas")
    ImpairedIdFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "RCH_CODE")
    ImpairedNameFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "WBODY_NAME")
    ImpairmentFieldIndex = GisUtil.FieldIndex(ImpairedLayerIndex, "ST_IMPAIR")
    'loop through each selected polygon and each 303d feature looking for overlap
    For j = 1 To cSelectedAreaIndexes.Count
      For i = 1 To GisUtil.NumFeatures(ImpairedLayerIndex)
        If GisUtil.OverlappingPolygons(ImpairedLayerIndex, i - 1, AreaLayerIndex, cSelectedAreaIndexes(j)) Then
          'these overlap
          cAreaIds.Add(GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaIdFieldIndex))
          cAreaNames.Add((GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), AreaNameFieldIndex)))
          cImpairedIds.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairedIdFieldIndex))
          cImpairedNames.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairedNameFieldIndex))
          cImpairment.Add(GisUtil.FieldValue(ImpairedLayerIndex, i - 1, ImpairmentFieldIndex))
        End If
      Next i
    Next j

    'now write file
    Dim OutFile As Integer
    Dim ctxt As String
    OutFile = FreeFile()
    FileOpen(OutFile, aOutputPath & "303d Listed Segments Table.out", OpenMode.Output)
    PrintLine(OutFile, "Watershed Characterization Report")
    PrintLine(OutFile, "  303d Listed Segments Within " & aAreaLayer)
    PrintLine(OutFile, "")
    PrintLine(OutFile, "AreaID" & vbTab & "AreaName" & vbTab & "SegmentID" & vbTab & "Waterbody Name" & vbTab & "Impairment")
    'write area ids and associated descriptions
    For i = 1 To cAreaIds.Count
      ctxt = cAreaIds(i) & vbTab & cAreaNames(i) & vbTab & _
             cImpairedIds(i) & vbTab & cImpairedNames(i) & vbTab & cImpairment(i)
      PrintLine(OutFile, ctxt)
    Next i
    FileClose(OutFile)

  End Sub
End Module
