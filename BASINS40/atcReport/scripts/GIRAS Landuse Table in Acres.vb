Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports MapWindow.Interfaces

Public Module GIRASLanduseTable
    Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                               ByVal aAreaIDFieldIndex As Integer, _
                               ByVal aAreaNameFieldIndex As Integer, _
                               ByVal aSelectedAreaIndexes As Collection) As Object

        Dim lProblem As String = ""
        Dim lLanduseLayerIndex As Integer
        Dim lLandUseFieldIndex As Long
        Dim lGridSource As New atcGridSource
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'set land use index layer
        Try
            lLanduseLayerIndex = GisUtil.LayerIndex("Land Use Index")
        Catch
            lProblem = "Missing Land Use Index Layer"
            Logger.Dbg(lProblem)
        End Try

        If Len(lProblem) = 0 Then

            Try
                lLandUseFieldIndex = GisUtil.FieldIndex(lLanduseLayerIndex, "COVNAME")
            Catch
                lProblem = "Expected field missing from Land Use Index Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                'figure out which land use tiles to list
                Dim lcluTiles As New Collection
                Dim lNewFileName As String
                Dim j As Integer
                Dim i As Integer
                For i = 1 To GisUtil.NumFeatures(lLanduseLayerIndex)
                    'loop thru each shape of land use index shapefile
                    lNewFileName = GisUtil.FieldValue(lLanduseLayerIndex, i - 1, lLandUseFieldIndex)
                    For j = 1 To aSelectedAreaIndexes.Count
                        If GisUtil.OverlappingPolygons(lLanduseLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
                            'yes, add it
                            lcluTiles.Add(lNewFileName)
                            Exit For
                        End If
                    Next j
                Next i

                Dim lPathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex)) & g_PathChar & "landuse"
                Dim lLandUseFieldName As String
                Dim lbfirst As Boolean = True
                For j = 1 To lcluTiles.Count
                    'loop thru each land use tile
                    lNewFileName = lPathName & g_PathChar & lcluTiles(j) & ".shp"
                    If Not GisUtil.AddLayer(lNewFileName, lcluTiles(j)) Then
                        lProblem = "Missing GIRAS Land Use Layer " & lcluTiles(j)
                        Logger.Dbg(lProblem)
                    End If

                    If Len(lProblem) = 0 Then
                        lLandUseFieldName = "LUCODE"
                        If GisUtil.LayerName(aAreaLayerIndex) <> "<none>" Then
                            'do overlay
                            GisUtil.Overlay(lcluTiles(j), lLandUseFieldName, _
                                            GisUtil.LayerName(aAreaLayerIndex), _
                                            GisUtil.FieldName(aAreaIDFieldIndex, aAreaLayerIndex), _
                                            lPathName & g_PathChar & "overlay.shp", lbfirst)
                            lbfirst = False
                        End If
                    End If
                Next j

                If Len(lProblem) = 0 Then
                    'build collection of selected area ids
                    Dim lSelectedAreaIds As New Collection
                    For i = 1 To aSelectedAreaIndexes.Count
                        lSelectedAreaIds.Add(GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(i), aAreaIDFieldIndex))
                    Next i

                    'if simple reclassifyfile exists, read it in
                    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                    Dim lReclassifyFile As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc" & g_PathChar
                    Dim ltmpDbf As IatcTable
                    If FileExists(lReclassifyFile) Then
                        lReclassifyFile = lReclassifyFile & "giras.dbf"
                    Else
                        lReclassifyFile = lBasinsFolder & "\etc\giras.dbf"
                    End If
                    Dim lcRcode As New Collection
                    Dim lcRname As New Collection
                    'open dbf file
                    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lReclassifyFile)
                    For i = 1 To ltmpDbf.NumRecords
                        ltmpDbf.CurrentRecord = i
                        lcRcode.Add(ltmpDbf.Value(1))
                        lcRname.Add(ltmpDbf.Value(2))
                    Next i

                    'read landuse, subbasin (aoi), and area combinations for selected areas from overlay.dbf
                    Dim lcLugroup As New Collection
                    Dim lcSubid As New Collection
                    Dim lcArea As New Collection
                    Dim lincollection As Boolean
                    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lPathName & g_PathChar & "overlay.dbf")
                    For i = 1 To ltmpDbf.NumRecords
                        ltmpDbf.CurrentRecord = i
                        lincollection = False
                        For j = 1 To lSelectedAreaIds.Count
                            If ltmpDbf.Value(2) = lSelectedAreaIds(j) Then
                                lincollection = True
                                Exit For
                            End If
                        Next j
                        If lincollection Then
                            'find lugroup that corresponds to this lucode
                            Dim lugroup As String = ""
                            For j = 1 To lcRcode.Count
                                If ltmpDbf.Value(1) = lcRcode(j) Then
                                    lugroup = lcRname(j)
                                    Exit For
                                End If
                            Next j
                            If Len(lugroup) > 0 Then
                                lcLugroup.Add(lugroup)
                            Else
                                lcLugroup.Add("<No Data>")
                            End If
                            lcSubid.Add(ltmpDbf.Value(2))
                            lcArea.Add(ltmpDbf.Value(3))
                        End If
                    Next i

                    'build collection of unique subbasin ids
                    Dim lcUniqueSubids As New Collection
                    For i = 1 To lcSubid.Count
                        lincollection = False
                        For j = 1 To lcUniqueSubids.Count
                            If lcUniqueSubids(j) = lcSubid(i) Then
                                lincollection = True
                                Exit For
                            End If
                        Next j
                        If Not lincollection Then
                            lcUniqueSubids.Add(lcSubid(i))
                        End If
                    Next i

                    'build collection of unique landuse groups
                    Dim lcUniqueLugroups As New atcCollection
                    For i = 1 To lcLugroup.Count
                        lincollection = False
                        For j = 0 To lcUniqueLugroups.Count - 1
                            If lcUniqueLugroups(j) = lcLugroup(i) Then
                                lincollection = True
                                Exit For
                            End If
                        Next j
                        If Not lincollection Then
                            lcUniqueLugroups.Add(lcLugroup(i))
                        End If
                    Next i
                    'sort landuse groups
                    lcUniqueLugroups.Sort()

                    'create summary array, area of each land use group in each subarea
                    Dim lArea(lcUniqueSubids.Count, lcUniqueLugroups.Count) As Single
                    Dim lspos As Integer
                    Dim llpos As Integer
                    'loop through each polygon (or grid subid/lucode combination)
                    'and populate array with area values
                    For i = 1 To lcSubid.Count
                        'find subbasin position in the area array
                        For j = 1 To lcUniqueSubids.Count
                            If lcSubid(i) = lcUniqueSubids(j) Then
                                lspos = j
                                Exit For
                            End If
                        Next j
                        'find lugroup that corresponds to this lucode
                        For j = 1 To lcUniqueLugroups.Count
                            If lcLugroup(i) = lcUniqueLugroups(j - 1) Then
                                llpos = j
                                Exit For
                            End If
                        Next j
                        lArea(lspos, llpos) = lArea(lspos, llpos) + lcArea(i)
                    Next i

                    'build grid source for results
                    With lGridSource
                        .Rows = 2
                        .Columns = lcUniqueSubids.Count + 1
                        .FixedRows = 1
                        'write area ids
                        For i = 1 To lcUniqueSubids.Count
                            .CellValue(0, i) = lcUniqueSubids(i)
                        Next i
                        'write associated descriptions
                        For i = 1 To lcUniqueSubids.Count
                            For j = 1 To aSelectedAreaIndexes.Count
                                If lSelectedAreaIds(j) = lcUniqueSubids(i) Then
                                    .CellValue(1, i) = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(j), aAreaNameFieldIndex)
                                    Exit For
                                End If
                            Next j
                        Next i
                        'now write data
                        For j = 1 To lcUniqueLugroups.Count
                            .Rows = .Rows + 1
                            .CellValue(.Rows - 1, 0) = lcUniqueLugroups(j - 1)
                            For i = 1 To lcUniqueSubids.Count
                                .CellValue(.Rows - 1, i) = Format((lArea(i, j) / 4046.8564), "0.")   'converting to acres
                            Next i
                        Next j
                    End With
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
