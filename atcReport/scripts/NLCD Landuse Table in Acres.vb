Imports atcMwGisUtility
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports MapWindow.Interfaces

Public Module NLCDLanduseTable
    Public Function ScriptMain(ByVal aAreaLayerIndex As Integer, _
                               ByVal aAreaIDFieldIndex As Integer, _
                               ByVal aAreaNameFieldIndex As Integer, _
                               ByVal aSelectedAreaIndexes As Collection) As Object

        Dim i As Integer
        Dim lLanduseLayerIndex As Integer
        Dim lProblem As String = ""
        Dim lGridSource As New atcGridSource
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'find appropriate lu grid layer
        For i = 0 To GisUtil.NumLayers() - 1
            If GisUtil.LayerType(i) = 4 Then
                'this is a grid 
                If InStr(GisUtil.LayerFileName(i), "\nlcd\") > 0 Then
                    'looks like an nlcd grid, lets use it
                    lLanduseLayerIndex = i
                End If
            End If
        Next
        Logger.Dbg("NLCDLanduseTable:" & lLanduseLayerIndex & ":" & aAreaLayerIndex)

        If lLanduseLayerIndex > -1 Then
            'tabulate the areas 
            Dim k As Integer = System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
            Dim laAreaLS(k, GisUtil.NumFeatures(aAreaLayerIndex)) As Double
            GisUtil.TabulateAreas(lLanduseLayerIndex, aAreaLayerIndex, laAreaLS)

            'build collection of selected area ids
            Dim lSelectedAreaIds As New Collection
            For i = 1 To aSelectedAreaIndexes.Count
                lSelectedAreaIds.Add(GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(i), aAreaIDFieldIndex))
            Next i

            'if simple reclassifyfile exists, read it in
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            Dim lReclassifyFile As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
            Dim ltmpDbf As IatcTable
            If FileExists(lReclassifyFile) Then
                lReclassifyFile = lReclassifyFile & "nlcd.dbf"
            Else
                lReclassifyFile = lBasinsFolder & "\etc\nlcd.dbf"
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

            'read landuse, subbasin (aoi), and area combinations for selected areas 
            Dim lcLugroup As New Collection
            Dim lcSubid As New Collection
            Dim lcArea As New Collection
            Dim lincollection As Boolean
            Dim lsubid As Integer
            Dim llugroup As String
            Dim j As Integer
            For k = 1 To aSelectedAreaIndexes.Count
                'loop thru each selected subbasin (or all if none selected)
                lsubid = GisUtil.FieldValue(aAreaLayerIndex, aSelectedAreaIndexes(k), aAreaIDFieldIndex)
                For i = 1 To System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
                    If laAreaLS(i, aSelectedAreaIndexes(k)) > 0 Then
                        'find lugroup that corresponds to code i
                        llugroup = ""
                        For j = 1 To lcRcode.Count
                            If i = lcRcode(j) Then
                                llugroup = lcRname(j)
                                Exit For
                            End If
                        Next j
                        If Len(llugroup) > 0 Then
                            lcLugroup.Add(llugroup)
                        Else
                            lcLugroup.Add("<No Data>")
                        End If
                        lcArea.Add(laAreaLS(i, aSelectedAreaIndexes(k)))
                        lcSubid.Add(lsubid)
                    End If
                Next i
            Next k

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
        Else
            lProblem = "No NLCD Layer Found"
            Logger.Dbg(lProblem)
        End If

        If Len(lProblem) > 0 Then
            Return lProblem
        Else
            Return lGridSource
        End If
    End Function
End Module
