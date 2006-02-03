Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module ListedSegmentsTable
  Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                             ByVal aAreaIDFieldIndex As Integer, _
                             ByVal aAreaNameFieldIndex As Integer, _
                             ByVal aSelectedAreaIndexes As Collection)
    Dim i As Integer
    Dim j As Integer
    Dim lImpairedLayerIndex As Integer
    Dim lImpairedIdFieldIndex As Integer
    Dim lImpairedNameFieldIndex As Integer
    Dim lImpairmentFieldIndex As Integer
    Dim lProblem As String = ""

    'build grid source for results
    Dim lGridSource = New atcGridSource
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

    'find 303d line layer
    Try
      lImpairedLayerIndex = GisUtil.LayerIndex("303d List - Lines")
    Catch
      lProblem = "No 303d Line Layer Found"
      Logger.Dbg(lProblem)
    End Try

    If Len(lProblem) = 0 Then
      Try
        lImpairedIdFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "RCH_CODE")
        lImpairedNameFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "WBODY_NAME")
        lImpairmentFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "ST_IMPAIR")
      Catch
        lProblem = "Expected field missing from 303d Line Layer"
        Logger.Dbg(lProblem)
      End Try
    End If

    If Len(lProblem) = 0 Then
      'loop through each selected polygon and each 303d feature looking for overlap
      For j = 1 To aSelectedAreaIndexes.Count
        For i = 1 To GisUtil.NumFeatures(lImpairedLayerIndex)
          If GisUtil.LineInPolygon(lImpairedLayerIndex, i, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaIDFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedIdFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairmentFieldIndex)
          End If
        Next i
      Next j
    End If

    'now check 303d polygon layer
    If Len(lProblem) = 0 Then
      Try
        lImpairedLayerIndex = GisUtil.LayerIndex("303d List - Areas")
      Catch
        lProblem = "No 303d Area Layer Found"
        Logger.Dbg(lProblem)
      End Try
    End If

    If Len(lProblem) = 0 Then
      Try
        lImpairedIdFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "RCH_CODE")
        lImpairedNameFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "WBODY_NAME")
        lImpairmentFieldIndex = GisUtil.FieldIndex(lImpairedLayerIndex, "ST_IMPAIR")
      Catch
        lProblem = "Expected field missing from 303d Area Layer"
        Logger.Dbg(lProblem)
      End Try
    End If

    If Len(lProblem) = 0 Then
      'loop through each selected polygon and each 303d feature looking for overlap
      For j = 1 To aSelectedAreaIndexes.Count
        For i = 1 To GisUtil.NumFeatures(lImpairedLayerIndex)
          If GisUtil.OverlappingPolygons(lImpairedLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
            'these overlap
            lGridSource.rows = lGridSource.rows + 1
            lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaIDFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 2) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedIdFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairedNameFieldIndex)
            lGridSource.CellValue(lGridSource.rows - 1, 4) = GisUtil.FieldValue(lImpairedLayerIndex, i - 1, lImpairmentFieldIndex)
          End If
        Next i
      Next j
    End If

    Return lGridSource
  End Function
End Module
