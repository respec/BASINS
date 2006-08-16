Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports MapWindow.Interfaces

Public Module WQObservationStationsTable
    Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                           ByVal aAreaIDFieldIndex As Integer, _
                           ByVal aAreaNameFieldIndex As Integer, _
                           ByVal aSelectedAreaIndexes As Collection) As Object

        Dim lWQLayerIndex As Integer
        Dim lStationFieldIndex As Integer
        Dim lNameFieldIndex As Integer
        Dim lAgencyFieldIndex As Integer
        Dim lProblem As String = ""

        'build grid source for results
        Dim lGridSource As New atcGridSource
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

        'find WQObs layer and needed fields
        Try
            lWQLayerIndex = GisUtil.LayerIndex("Water Quality Observation")
        Catch
            lProblem = "Missing Water Quality Observation Layer"
            Logger.Dbg(lProblem)
        End Try

        If Len(lProblem) = 0 Then
            Try
                lStationFieldIndex = GisUtil.FieldIndex(lWQLayerIndex, "bstat_id")
                lNameFieldIndex = GisUtil.FieldIndex(lWQLayerIndex, "location")
                lAgencyFieldIndex = GisUtil.FieldIndex(lWQLayerIndex, "agency")
            Catch
                lProblem = "Expected field missing from Water Quality Observation Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                Dim i As Integer
                Dim j As Integer
                Dim lPolygonIndex As Integer
                GisUtil.ShowProgressBar(True)
                GisUtil.ProgressBarValue(0)
                Dim lProgressTotal As Integer = GisUtil.NumFeatures(lWQLayerIndex)
                Dim lProgressCurrent As Integer = 0
                Dim lProgressPercent As Integer = 0
                Dim lProgressLastDisplayed As Integer = 0
                'loop through each selected polygon and wq point looking for overlap
                For i = 1 To GisUtil.NumFeatures(lWQLayerIndex)
                    System.Windows.Forms.Application.DoEvents()
                    lPolygonIndex = GisUtil.PointInPolygon(lWQLayerIndex, i, aAreaLayerIndex)
                    If lPolygonIndex > -1 Then
                        For j = 1 To aSelectedAreaIndexes.Count
                            If aSelectedAreaIndexes(j) = lPolygonIndex Then
                                'these overlap
                                lGridSource.Rows = lGridSource.Rows + 1
                                lGridSource.CellValue(lGridSource.Rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaIDFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaNameFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 2) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lStationFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 3) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lNameFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 4) = GisUtil.FieldValue(lWQLayerIndex, i - 1, lAgencyFieldIndex)
                                Exit For
                            End If
                        Next j
                    End If
                    lProgressCurrent = lProgressCurrent + 1
                    lProgressPercent = Int(lProgressCurrent / lProgressTotal * 100)
                    If lProgressPercent > lProgressLastDisplayed Then
                        GisUtil.ProgressBarValue(lProgressPercent)
                        lProgressLastDisplayed = lProgressPercent
                    End If
                Next i
                GisUtil.ShowProgressBar(False)
            End If
        End If

        If Len(lProblem) > 0 Then
            Return lProblem
        Else
            Return lGridSource
        End If
    End Function

End Module

