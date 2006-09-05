Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports MapWindow.Interfaces

Public Module PCSFacilityTable
    Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                               ByVal aAreaIDFieldIndex As Integer, _
                               ByVal aAreaNameFieldIndex As Integer, _
                               ByVal aSelectedAreaIndexes As Collection) As Object

        Dim lPCSLayerIndex As Integer
        Dim lNPDESFieldIndex As Integer
        Dim lFacilityFieldIndex As Integer
        Dim lSicFieldIndex As Integer
        Dim lSicdFieldIndex As Integer
        Dim lCityFieldIndex As Integer
        Dim lRecwaterFieldIndex As Integer
        Dim lProblem As String = ""

        'build grid source for results
        Dim lGridSource As New atcGridSource
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

        'find PCS layer and needed fields
        Try
            lPCSLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
        Catch
            lProblem = "Missing Permit Compliance System Layer"
            Logger.Dbg(lProblem)
        End Try

        If Len(lProblem) = 0 Then
            Try
                lNPDESFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "npdes")
                lFacilityFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "fac_name")
                lSicFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "sic2")
                lSicdFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "sic2d")
                lCityFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "city")
                lRecwaterFieldIndex = GisUtil.FieldIndex(lPCSLayerIndex, "rec_water")
            Catch
                lProblem = "Expected field missing from Permit Compliance System Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                Dim i As Integer
                Dim j As Integer
                Dim lPolygonIndex As Integer
                Dim lProgressTotal As Integer = GisUtil.NumFeatures(lPCSLayerIndex)
                Dim lProgressCurrent As Integer = 0
                'loop through each selected polygon and pcs point looking for overlap
                For i = 1 To GisUtil.NumFeatures(lPCSLayerIndex)
                    lPolygonIndex = GisUtil.PointInPolygon(lPCSLayerIndex, i, aAreaLayerIndex)
                    System.Windows.Forms.Application.DoEvents()
                    If lPolygonIndex > -1 Then
                        For j = 1 To aSelectedAreaIndexes.Count
                            If aSelectedAreaIndexes(j) = lPolygonIndex Then
                                'these overlap
                                lGridSource.Rows = lGridSource.Rows + 1
                                lGridSource.CellValue(lGridSource.Rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaIDFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaNameFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 2) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lNPDESFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 3) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lFacilityFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 4) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lSicFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 5) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lSicdFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 6) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lCityFieldIndex)
                                lGridSource.CellValue(lGridSource.Rows - 1, 7) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lRecwaterFieldIndex)
                                Exit For
                            End If
                        Next j
                    End If
                    lProgressCurrent = lProgressCurrent + 1
                    Logger.Progress(lProgressCurrent, lProgressTotal)
                Next i
                Logger.Progress(lProgressTotal, lProgressTotal)
            End If
        End If

        If Len(lProblem) > 0 Then
            Return lProblem
        Else
            Return lGridSource
        End If
    End Function

End Module

