Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Public Module PCSDischargeTable
    Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                             ByVal aAreaIDFieldIndex As Integer, _
                             ByVal aAreaNameFieldIndex As Integer, _
                             ByVal aSelectedAreaIndexes As Collection) As Object

        Dim lProblem As String = ""
        Dim lPCSLayerIndex As Integer
        Dim lNPDESFieldIndex As Integer
        Dim lFacilityFieldIndex As Integer

        'build grid source for results
        Dim lGridSource As New atcGridSource
        With lGridSource
            .Rows = 1
            .Columns = 9
            .FixedRows = 1
            .CellValue(0, 0) = "AreaID"
            .CellValue(0, 1) = "AreaName"
            .CellValue(0, 2) = "NPDES"
            .CellValue(0, 3) = "Facility Name"
            .CellValue(0, 4) = "Parameter"
            .CellValue(0, 5) = "Year"
            .CellValue(0, 6) = "Flow (MGD)"
            .CellValue(0, 7) = "Avg. Conc. (mg/l)"
            .CellValue(0, 8) = "Load (lbs/yr)"
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
            Catch
                lProblem = "Expected field missing from Permit Compliance System Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                'open PCS loading dbfs
                Dim lpcspath As String = PathNameOnly(GisUtil.LayerFileName(lPCSLayerIndex)) & "\pcs\"
                Dim lallFiles As New NameValueCollection
                Dim lcNpdes As New Collection
                Dim lcParm As New Collection
                Dim lcYear As New Collection
                Dim lcFlow As New Collection
                Dim lcConc As New Collection
                Dim lcLoad As New Collection
                Dim ltmpDbf As IatcTable
                Dim i As Integer
                AddFilesInDir(lallFiles, lpcspath, False, "*.dbf")
                For Each lFilename As String In lallFiles
                    'open dbf file
                    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lFilename)
                    If ltmpDbf.NumFields > 5 Then
                        For i = 1 To ltmpDbf.NumRecords
                            ltmpDbf.CurrentRecord = i
                            lcParm.Add(ltmpDbf.Value(1))
                            lcNpdes.Add(ltmpDbf.Value(2))
                            lcYear.Add(ltmpDbf.Value(3))
                            lcConc.Add(ltmpDbf.Value(4))
                            lcFlow.Add(ltmpDbf.Value(5))
                            lcLoad.Add(ltmpDbf.Value(6))
                        Next i
                    End If
                Next
                If lcParm.Count = 0 Then
                    lProblem = "No PCS Loading Dbfs found."
                    Logger.Dbg(lProblem)
                End If

                If Len(lProblem) = 0 Then
                    'read in parm name table
                    lpcspath = PathNameOnly(GisUtil.LayerFileName(lPCSLayerIndex)) & "\pcs3_prm.dbf"
                    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lpcspath)
                    Dim lcParmCode As New Collection
                    Dim lcParmName As New Collection
                    For i = 1 To ltmpDbf.NumRecords
                        ltmpDbf.CurrentRecord = i
                        lcParmCode.Add(ltmpDbf.Value(1))
                        lcParmName.Add(ltmpDbf.Value(2))
                    Next i
                    If lcParmCode.Count = 0 Then
                        lProblem = "No PCS Parameter Name table found."
                        Logger.Dbg(lProblem)
                    End If

                    Dim j As Integer
                    Dim k As Integer
                    Dim l As Integer
                    Dim lPolygonIndex As Integer
                    Dim lnpdes As String
                    GisUtil.ShowProgressBar(True)
                    GisUtil.ProgressBarValue(0)
                    Dim lProgressTotal As Integer = GisUtil.NumFeatures(lPCSLayerIndex)
                    Dim lProgressCurrent As Integer = 0
                    Dim lProgressPercent As Integer = 0
                    Dim lProgressLastDisplayed As Integer = 0
                    'loop through each selected polygon and pcs point looking for overlap
                    For i = 1 To GisUtil.NumFeatures(lPCSLayerIndex)
                        lPolygonIndex = GisUtil.PointInPolygon(lPCSLayerIndex, i, aAreaLayerIndex)
                        If lPolygonIndex > -1 Then
                            For j = 1 To aSelectedAreaIndexes.Count
                                System.Windows.Forms.Application.DoEvents()
                                If aSelectedAreaIndexes(j) = lPolygonIndex Then
                                    'these overlap
                                    lnpdes = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lNPDESFieldIndex)
                                    For k = 1 To lcNpdes.Count
                                        System.Windows.Forms.Application.DoEvents()
                                        If lnpdes = lcNpdes(k) Then
                                            'want to add this record
                                            lGridSource.Rows = lGridSource.Rows + 1
                                            lGridSource.CellValue(lGridSource.Rows - 1, 0) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaIDFieldIndex)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 1) = GisUtil.FieldValue(aAreaLayerIndex, lPolygonIndex, aAreaNameFieldIndex)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 2) = lnpdes
                                            lGridSource.CellValue(lGridSource.Rows - 1, 3) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lFacilityFieldIndex)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 4) = lcParm(k)
                                            'convert parm code to parm name
                                            For l = 1 To lcParmCode.Count
                                                If Trim(lcParm(k)) = Trim(lcParmCode(l)) Then
                                                    lGridSource.CellValue(lGridSource.Rows - 1, 4) = lcParmName(l)
                                                    Exit For
                                                End If
                                            Next l
                                            lGridSource.CellValue(lGridSource.Rows - 1, 5) = lcYear(k)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 6) = lcFlow(k)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 7) = lcConc(k)
                                            lGridSource.CellValue(lGridSource.Rows - 1, 8) = lcLoad(k)
                                        End If
                                    Next k
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
        End If

        If Len(lProblem) > 0 Then
            Return lProblem
        Else
            Return lGridSource
        End If
    End Function

End Module

