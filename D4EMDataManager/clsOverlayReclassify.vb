Imports atcUtility
Imports MapWinUtility
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

<CLSCompliant(False)> _
Public Class clsOverlayReclassify
    Public Shared Function Overlay(ByVal aGridOutputFileName As String, _
                                   ByVal aGridSlopeValueFileName As String, _
                                   ByVal aResume As Boolean, _
                                   ByVal ParamArray aLayerFileNames() As String) As clsHruTable
        Dim lHruTable As clsHruTable = Nothing
        Try
            Dim lGridSlopeValue As New clsLayer("GridSlopeValue=" & aGridSlopeValueFileName)

            Dim lLayers As New Generic.List(Of clsLayer)
            For Each lLayerFileName As String In aLayerFileNames
                lLayers.Add(New clsLayer(lLayerFileName))
            Next

            lHruTable = Overlay(aGridOutputFileName, lGridSlopeValue, lLayers, aResume)

            For Each lLayer As clsLayer In lLayers
                lLayer.Close()
            Next
        Catch ex As Exception
            Logger.Dbg("OverlayProblem " & ex.Message & vbCrLf & ex.StackTrace)
            lHruTable = Nothing
        End Try

        Return lHruTable
    End Function

    ''' <summary>
    ''' Overlay the grid and/or shape layers in aLayers
    ''' For each unique combination, add an entry to Me.HruTable,
    ''' and put the index of the combination in Me.HruTable into a new grid named aGridOutputFilename
    ''' </summary>
    ''' <param name="aGridOutputFileName">Grid file name to write </param>
    ''' <param name="aGridSlopeValue"></param>
    ''' <param name="aLayers"></param>
    ''' <param name="aResume"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Overlay(ByVal aGridOutputFileName As String, _
                                   ByVal aGridSlopeValue As clsLayer, _
                                   ByVal aLayers As Generic.List(Of clsLayer), _
                          Optional ByVal aResume As Boolean = False) As clsHruTable
        Dim lHruTable As clsHruTable = Nothing

        Dim lDebug As Boolean = False
        Dim lResumed As Boolean = False
        Dim lLastRow As Integer = aGridSlopeValue.Grid.Header.NumberRows
        Dim lLastCol As Integer = aGridSlopeValue.Grid.Header.NumberCols

        Dim lGridOverlay As New MapWinGIS.Grid 'overlay output grid
        Dim lGridOverlayHeader As New MapWinGIS.GridHeader ' output grid header
        Dim lGridOverlayNoDataValue As Integer = -1
        Dim lStartRow As Integer = 0

        If aResume AndAlso IO.File.Exists(aGridOutputFileName) AndAlso lGridOverlay.Open(aGridOutputFileName) Then
            Dim lHruTableFilename As String = ""
            Dim lHruTableBaseFilename As String = IO.Path.ChangeExtension(aGridOutputFileName, ".table")
            For Each lFilename As String In IO.Directory.GetFiles(IO.Path.GetDirectoryName(aGridOutputFileName), _
                                                                  IO.Path.GetFileName(lHruTableBaseFilename) & "*.txt")
                Try
                    Dim lSavedRow As Integer = lFilename.Substring(lHruTableBaseFilename.Length).Replace(".txt", "")
                    If lSavedRow > lStartRow Then
                        lStartRow = lSavedRow
                        lHruTableFilename = lFilename
                    End If
                Catch
                End Try
            Next
            If IO.File.Exists(lHruTableFilename) Then
                lGridOverlayHeader = lGridOverlay.Header
                lHruTable = New clsHruTable(lHruTableFilename)
                lResumed = True
            End If
        End If

        Dim lSlopeReclassIndex As Integer
        Dim lTags As New Generic.List(Of String)
        For Each lLayer As clsLayer In aLayers
            If lLayer.Tag = "SlopeReclass" Then
                lSlopeReclassIndex = lTags.Count
            End If
            lTags.Add(lLayer.Tag)
            lLayer.NeedsProjection = Not lLayer.MatchesGrid(aGridSlopeValue.Grid)
        Next

        If Not lResumed Then
            lHruTable = New clsHruTable(lTags)
            lGridOverlayHeader = New MapWinGIS.GridHeader
            lGridOverlayHeader.CopyFrom(aGridSlopeValue.Grid.Header)
            lGridOverlayHeader.NodataValue = lGridOverlayNoDataValue
            lGridOverlay.CreateNew(aGridOutputFileName, lGridOverlayHeader, MapWinGIS.GridDataType.ShortDataType, lGridOverlayNoDataValue, True, MapWinGIS.GridFileType.Binary)
        End If

        Logger.Dbg("OverlayCreated " & MemUsage())

        Dim lCellSizedX As Double = lGridOverlayHeader.dX
        Dim lCellSizedY As Double = lGridOverlayHeader.dY
        Dim lCellCountToArea As Double = (lCellSizedX * lCellSizedY) / (1000 * 1000)
        'km2
        Dim lAreaTotal As Double = (lLastCol + 1) * _
                                   (lLastRow + 1) * lCellCountToArea
        Dim lOutsideAreaSkipped As Double = 0.0
        Dim lRequiredAreaSkippedBadSlope As Double = 0.0
        Dim lBigSlopeAreaClamped As Double = 0.0
        Dim lRequiredAreaSkippedSlopeCantReclass As Double = 0.0
        Dim lTotalHruArea As Double = 0.0

        Dim lRem As Integer
        Logger.Dbg("OverlayValuesStart " & lLastRow & ":" & lLastCol & MemUsage())
        Dim lCount As Integer = 0
        Dim lMaxCount As Integer = (lLastRow - lStartRow) * lLastCol

        For lRow As Integer = lStartRow To lLastRow
            Math.DivRem(lRow, 500, lRem)
            If lRem = 0 AndAlso lRow > lStartRow Then
                Logger.Dbg("Row " & lRow & " of " & lLastRow & " HRUCount " & lHruTable.Count & " " & MemUsage())
                lHruTable.Save(IO.Path.ChangeExtension(aGridOutputFileName, ".table" & lRow.ToString & ".txt"))
                If lGridOverlay.Save(aGridOutputFileName, MapWinGIS.GridFileType.Binary, Nothing) Then
                    Logger.Dbg("GridOverlaySaved " & MemUsage())
                Else
                    Logger.Dbg("GridOverlaySaveFailed " & lGridOverlay.ErrorMsg(lGridOverlay.LastErrorCode) & " " & MemUsage())
                End If
            End If

            'TODO: look for memory leak as needed here
            'If (System.Diagnostics.Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20)) > 1500 Then 'bigger for win64?
            '    Logger.Dbg("TryToFreeMemory " & MemUsage())
            '    For Each lLayer As clsLayer In aLayers
            '        lLayer.Reopen()
            '    Next
            '    aGridSlopeValue.Reopen()
            'End If

            For lCol As Integer = 0 To lLastCol

                lCount += 1
                Logger.Progress(lCount, lMaxCount)

                Dim lKey As String = ""
                Dim lKeyParts As New Generic.List(Of String)
                Dim lRequired As Boolean = False
                Dim lIncomplete As Boolean = False
                For Each lLayer As clsLayer In aLayers
                    Dim lKeyPart As String = lLayer.Key(aGridSlopeValue.Grid, lRow, lCol)
                    If lKeyPart.Length = 0 Then 'no data for this layer at this row, col
                        lIncomplete = True
                        lKeyPart = "Missing"
                    Else
                        'If we have a good value in a required layer, keep the cell even if incomplete
                        If lLayer.IsRequired Then lRequired = True
                    End If
                    lKey &= lKeyPart & "_"
                    lKeyParts.Add(lKeyPart)
                Next
                lKey = lKey.TrimEnd("_")
                If lIncomplete AndAlso Not lRequired Then
                    lOutsideAreaSkipped += lCellCountToArea
                    lGridOverlay.Value(lCol, lRow) = lGridOverlayNoDataValue
                Else
                    Dim lGridSlopeValue As Double = aGridSlopeValue.Grid.Value(lCol, lRow)
                    If lGridSlopeValue < 0 Then
                        lRequiredAreaSkippedBadSlope += lCellCountToArea
                        lGridOverlay.Value(lCol, lRow) = lGridOverlayNoDataValue
                    ElseIf lKeyParts(lSlopeReclassIndex).Length = 0 AndAlso lGridSlopeValue > 0 Then
                        lRequiredAreaSkippedSlopeCantReclass += lCellCountToArea
                        lGridOverlay.Value(lCol, lRow) = lGridOverlayNoDataValue
                    Else
                        lTotalHruArea += lCellCountToArea
                        If lGridSlopeValue > 100 Then 'Clamp slope values to maximum of 100
                            lBigSlopeAreaClamped += lCellCountToArea
                            lGridSlopeValue = 100
                        End If
                        If lHruTable.Contains(lKey) Then
                            lGridOverlay.Value(lCol, lRow) = CDbl(lHruTable(lKey).Handle)
                            With lHruTable(lKey)
                                .CellCount += 1
                                .Area = .CellCount * lCellSizedX * lCellSizedY
                                If lGridSlopeValue < 100 Then 'Only include reasonable slope values in mean
                                    If .SlopeMean > 99 Then
                                        .SlopeMean = lGridSlopeValue
                                    Else
                                        .SlopeMean = (.SlopeMean * (.CellCount - 1) + lGridSlopeValue) / .CellCount
                                    End If
                                End If
                            End With
                        Else
                            lGridOverlay.Value(lCol, lRow) = CDbl(lHruTable.Count)
                            Dim lOverlayGridTable As New clsHru(lKey, lHruTable.Count, 1, _
                                                                lKeyParts, _
                                                                (lCellSizedX * lCellSizedY), lGridSlopeValue)
                            lHruTable.Add(lOverlayGridTable)
                            lOverlayGridTable = Nothing
                            'If lDebug Then Logger.Dbg("AfterNewGridStructure " & MemUsage())
                        End If
                    End If
                End If
            Next
            If lDebug Then
                Logger.Dbg("Row " & lRow & _
                           " OverlayHashTableCount " & lHruTable.Count & _
                           " Complete " & MemUsage())
                Logger.Flush()
            End If
        Next
        lHruTable.Save(IO.Path.ChangeExtension(aGridOutputFileName, ".table.txt"))

        Logger.Dbg("OverlayValuesSet " & lLastRow * lLastCol & " " & MemUsage())
        Logger.Dbg("HRUCount " & lHruTable.Count)

        Logger.Dbg("Total Overlay Area     " & DoubleToString(lAreaTotal, , "#,###,##0.00"))
        Logger.Dbg("Total Area Outside     " & DoubleToString(lOutsideAreaSkipped, , "#,###,##0.00"))
        Logger.Dbg("Total Area Inside      " & DoubleToString(lAreaTotal - lOutsideAreaSkipped, , "#,###,##0.00"))
        Logger.Dbg("Total Area In HRUs     " & DoubleToString(lTotalHruArea, , "#,###,##0.00"))
        Logger.Dbg("Inside Area Not in HRU " & DoubleToString(lAreaTotal - lOutsideAreaSkipped - lTotalHruArea, , "#,###,##0.00") & " = Inside - HRUs")
        Logger.Dbg("Inside Area Not in HRU " & DoubleToString(lRequiredAreaSkippedBadSlope + lRequiredAreaSkippedSlopeCantReclass, , "#,###,##0.00") & " = Bad Slope + Can't Reclassify")
        Logger.Dbg("Area Clamped Big Slope " & DoubleToString(lBigSlopeAreaClamped, , "#,###,##0.00"))
        Logger.Dbg("Area Skipped Bad Slope " & DoubleToString(lRequiredAreaSkippedBadSlope, , "#,###,##0.00"))
        Logger.Dbg("Area Skipped Reclass   " & DoubleToString(lRequiredAreaSkippedSlopeCantReclass, , "#,###,##0.00"))

        If lGridOverlay.Save(aGridOutputFileName, MapWinGIS.GridFileType.Binary, Nothing) Then
            Logger.Dbg("GridOverlaySaved " & MemUsage())
            'TODO: write a legend file using the keys
            If lGridOverlay.Close() Then
                Logger.Dbg("GridOverlayClosed " & MemUsage())
            Else
                Logger.Dbg("GridOverlayCloseFailed " & lGridOverlay.ErrorMsg(lGridOverlay.LastErrorCode) & " " & MemUsage())
            End If
        Else
            Logger.Dbg("GridOverlaySaveFailed " & lGridOverlay.ErrorMsg(lGridOverlay.LastErrorCode) & " " & MemUsage())
            Return Nothing
        End If
        lGridOverlay = Nothing
        lHruTable.ComputeTotalCellCount()
        Return lHruTable
    End Function

    Public Shared Sub ReportByTag(ByVal aReport As Text.StringBuilder, _
                           ByVal aCollection As atcCollection, _
                           ByVal aDisplayTags As Generic.List(Of String), _
                  Optional ByVal aDisplayFirst As Boolean = True, _
                  Optional ByVal aDisplayAll As Boolean = True, _
                  Optional ByVal aDisplayPredominant As Boolean = False)

        Dim lCountCum As Int64 = 0
        Dim lCountOut As Int64 = 0

        If aReport.Length = 0 Then
            aReport.Append("Index".PadLeft(12) & vbTab)
            For Each lDisplayTag As String In aDisplayTags
                aReport.Append(lDisplayTag.PadLeft(12) & vbTab)
            Next
            aReport.Append("Area".PadLeft(12) & vbTab _
                             & "CntCell".PadLeft(12) & vbTab _
                             & "PctTag".PadLeft(12) & vbTab _
                             & "CumPct".PadLeft(12) & vbTab _
                             & "Slope".PadLeft(12))
            aReport.AppendLine()
        End If

        If aCollection.Count > 0 Then
            If aCollection.ItemByIndex(0).GetType.Name = "atcCollection" Then
                For Each lCollection As atcCollection In aCollection
                    ReportByTag(aReport, lCollection, aDisplayTags, aDisplayFirst, aDisplayAll, aDisplayPredominant)
                Next
            Else
                For Each lHruTableOfTag As clsHruTable In aCollection
                    Dim lTagCellCount As Int64 = lHruTableOfTag.TotalCellCount
                    Dim lHrusSortedByCellCount As clsHruTable = lHruTableOfTag.Sort(True)
                    Dim lTagCellCum As Int64 = 0

                    If aDisplayFirst OrElse aDisplayAll Then
                        For Each lHru As clsHru In lHrusSortedByCellCount
                            With lHru
                                lTagCellCum += .CellCount
                                lCountCum += .CellCount
                                lCountOut += 1

                                aReport.Append(lCountOut.ToString.PadLeft(12) & vbTab)
                                For Each lDisplayTag As String In aDisplayTags
                                    aReport.Append(lHruTableOfTag.Id(lHru, lDisplayTag).PadLeft(12) & vbTab)
                                Next

                                aReport.Append(DoubleToString(.Area, 12, "#,###,##0.").PadLeft(12) & vbTab & _
                                           .CellCount.ToString.PadLeft(12) & vbTab & _
                                           DoubleToString((100 * .CellCount) / lTagCellCount, , "#0.000", "#0.000", , 3).PadLeft(12) & vbTab & _
                                           DoubleToString((100 * lTagCellCum) / lTagCellCount, , "#0.000", "#0.000", , 3).PadLeft(12) & vbTab & _
                                           DoubleToString(.SlopeMean, , "#,##0.00").PadLeft(12))

                                If aDisplayPredominant Then
                                    For Each lDisplayTag As String In aDisplayTags
                                        Dim lPredominantValue As String = lHruTableOfTag.PredominantTagValue(lDisplayTag)
                                        If lPredominantValue <> lHruTableOfTag.Id(lHru, lDisplayTag) Then
                                            aReport.Append(vbTab & "Predominant " & lDisplayTag & " = " & lPredominantValue & " first = " & lHruTableOfTag.Id(lHru, lDisplayTag))
                                        End If
                                    Next
                                    aReport.AppendLine()
                                End If
                                aReport.AppendLine()
                                If Not aDisplayAll Then Exit For
                            End With
                        Next
                    End If

                    If aDisplayPredominant Then
                        aReport.Append("Predominant" & vbTab)
                        For Each lDisplayTag As String In aDisplayTags
                            aReport.Append(lHruTableOfTag.PredominantTagValue(lDisplayTag).PadLeft(12) & vbTab)
                        Next
                        aReport.AppendLine()
                    End If
                Next
            End If
        End If
        Logger.Dbg(aDisplayTags(0) & "Count " & aCollection.Count & " CountCum " & lCountCum)

    End Sub

    Public Shared Function UniqueValuesSummary(ByVal aHruTable As clsHruTable, Optional ByVal aTag As String = Nothing) As String
        Dim lTags As Generic.List(Of String)
        If aTag Is Nothing Then
            lTags = aHruTable.Tags
        Else
            lTags = New Generic.List(Of String)
            lTags.Add(aTag)
        End If

        Dim lSB As New Text.StringBuilder
        For Each lTag As String In lTags
            lSB.AppendLine(lTag & " Summary")
            lSB.AppendLine("Key" & vbTab & "Count" & vbTab & "% of Total" & vbTab & "Cumulative %")
            Dim lTagTotals As atcUtility.atcCollection = aHruTable.CountCellsPerTagValue(lTag)
            Dim lTotalTotal As Int64 = 0
            For lIndex As Integer = lTagTotals.Count - 1 To 0 Step -1
                lTotalTotal += lTagTotals.ItemByIndex(lIndex)
            Next
            Dim lCumCount As Int64 = 0
            For lIndex As Integer = lTagTotals.Count - 1 To 0 Step -1
                Dim lCountThisTagValue As Int64 = lTagTotals.ItemByIndex(lIndex)
                lCumCount += lCountThisTagValue
                lSB.AppendLine(lTagTotals.Keys(lIndex) & vbTab & lCountThisTagValue.ToString.PadLeft(10) & vbTab _
                               & atcUtility.DoubleToString(100 * lCountThisTagValue / lTotalTotal, , "0.0", "0.0").PadLeft(5) & vbTab _
                               & atcUtility.DoubleToString(100 * lCumCount / lTotalTotal, , "0.0", "0.0").PadLeft(5))
            Next
        Next
        Return lSB.ToString
    End Function

    ''' <summary>
    ''' Simplify a collection of HRUs by dissolving those with areas below a threshold into similar HRUs
    ''' </summary>
    ''' <param name="aLayerTags">Tag names used for each field of the HRUs</param>
    ''' <param name="aSubBasinTable">Collection of clsHruTable items, one clsHruTable for each subbasin, obtainable from clsHruTable.SummarizeByTag("SubBasin")</param>
    ''' <param name="aTag">Tag of layer to dissolve, or "Area" to decide by area of HRU</param>
    ''' <param name="aIgnoreBelowFraction">Threshold as a fraction (between zero and 1) of the area of the subbasin</param>
    ''' <param name="aIgnoreBelowAbsolute">Threshold as an absolute area</param>
    ''' <param name="aGridOverlayFileName">Grid containing HRU indexes</param>
    ''' <returns>Table of HRUs from which those representing small [aTag] have been removed and remaining HRUs have been expanded to preserve total area</returns>
    ''' <remarks></remarks>
    Public Shared Function Simplify(ByVal aLayerTags As Generic.List(Of String), _
                                    ByVal aSubBasinTable As atcCollection, _
                                    ByVal aTag As String, _
                                    ByVal aIgnoreBelowFraction As Double, _
                                    ByVal aIgnoreBelowAbsolute As Double, _
                                    ByVal aGridOverlayFileName As String) As clsHruTable

        Dim lHruTableSimplified As New clsHruTable(aLayerTags)
        Dim lTagIsArea As Boolean = False
        If aTag = "Area" Then lTagIsArea = True

        Dim lAreaTotalBefore As Double = 0.0
        Dim lNumHRUsBefore As Integer = 0
        Dim lAreaTotalAfter As Double = 0.0
        For Each lHruTable As clsHruTable In aSubBasinTable
            Dim lSubBasinAreaTotal As Double = 0.0
            Dim lTagAreaMax As Double = 0.0 'Largest area with this tag
            Dim lTagAreaMaxId As String = "" 'ID of largest area
            Dim lTagAreas As New atcCollection
            Dim lTagKey As String
            For Each lHru As clsHru In lHruTable
                lNumHRUsBefore += 1
                lSubBasinAreaTotal += lHru.Area
                If lTagIsArea Then
                    lTagKey = lHru.Key
                Else
                    lTagKey = lHruTable.Id(lHru, aTag)
                End If
                lTagAreas.Increment(lTagKey, lHru.Area)
                Dim lTagAreaIndex As Integer = lTagAreas.IndexFromKey(lTagKey)
                If lTagAreas(lTagAreaIndex) > lTagAreaMax Then
                    lTagAreaMax = lTagAreas(lTagAreaIndex)
                    lTagAreaMaxId = lTagKey
                End If
            Next
            lAreaTotalBefore += lSubBasinAreaTotal

            Dim lAreaRemoved As Double = 0.0
            For Each lTagKey In lTagAreas.Keys
                Dim lTagArea As Double = lTagAreas.ItemByKey(lTagKey)
                If (lTagArea / lSubBasinAreaTotal) < aIgnoreBelowFraction OrElse _
                    lTagArea < aIgnoreBelowAbsolute Then
                    If lTagAreaMaxId <> lTagKey Then
                        lAreaRemoved += lTagArea
                        lTagAreas.Increment(lTagKey, -lTagArea)
                    Else
                        Logger.Dbg("MustKeepSomeArea " & lTagAreaMaxId)
                    End If
                End If
            Next

            Dim lHruCountRemoved As Integer = 0
            Dim lHruCountSaved As Integer = 0
            For Each lHru As clsHru In lHruTable
                If lTagIsArea Then
                    lTagKey = lHru.Key
                Else
                    lTagKey = lHruTable.Id(lHru, aTag)
                End If
                If (lTagAreas.ItemByKey(lTagKey) > 0) Then
                    lHruCountSaved += 1
                Else
                    lHruCountRemoved += 1
                End If
            Next

            Dim lAreaAdjustmentRatio As Double = lSubBasinAreaTotal / (lSubBasinAreaTotal - lAreaRemoved)

            Logger.Dbg("SubBasin " & lHruTable.Id(lHruTable.Item(0), "SubBasin") & _
                       " O " & lHruTable.Count & _
                       " C " & lHruCountSaved & _
                       " R " & lHruCountRemoved & _
                       " A " & lSubBasinAreaTotal & _
                       " F " & lAreaAdjustmentRatio)
            For Each lHru As clsHru In lHruTable
                If lTagIsArea Then
                    lTagKey = lHru.Key
                Else
                    lTagKey = lHruTable.Id(lHru, aTag)
                End If
                If (lTagAreas.ItemByKey(lTagKey) > 0) Then
                    Dim lHruSimplified As New clsHru(lHru)
                    With lHruSimplified
                        .Area *= lAreaAdjustmentRatio
                        lHruTableSimplified.Add(lHruSimplified)
                        lAreaTotalAfter += .Area
                    End With
                End If
            Next
        Next
        lHruTableSimplified.ComputeTotalCellCount()

        Logger.Dbg("BeforeSimplify: HruCount=" & lNumHRUsBefore & " AreaTotal=" & DoubleToString(lAreaTotalBefore / (1000 * 1000), , "#,###,##0.00"))
        Logger.Dbg("AfterSimplify:  HruCount " & lHruTableSimplified.Count & " AreaTotal=" & DoubleToString(lAreaTotalAfter / (1000 * 1000), , "#,###,##0.00"))
        Dim lSB As New System.Text.StringBuilder
        Dim lSortTags As New Generic.List(Of String)
        lSortTags.Add("SubBasin")
        ReportByTag(lSB, lHruTableSimplified.SummarizeByTag(lSortTags), aLayerTags)
        IO.File.WriteAllText(IO.Path.GetDirectoryName(aGridOverlayFileName) & "\HRUsRev" & aIgnoreBelowFraction.ToString & ".txt", lSB.ToString)
        Logger.Flush()

        Return lHruTableSimplified
    End Function

    Private Shared pLastPrivateMemory As Integer = 0
    Private Shared pLastGcMemory As Integer = 0
    Friend Shared Function MemUsage() As String
        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
        Dim lPrivateMemory As Integer = System.Diagnostics.Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20)
        Dim lGcMemory As Integer = System.GC.GetTotalMemory(True) / (2 ^ 20)
        MemUsage = "Megabytes: " & lPrivateMemory & " (" & Format((lPrivateMemory - pLastPrivateMemory), "+0;-0") & ") " _
                   & " GC: " & lGcMemory & " (" & Format((lGcMemory - pLastGcMemory), "+0;-0") & ") "
        pLastPrivateMemory = lPrivateMemory
        pLastGcMemory = lGcMemory
    End Function
End Class

Public Class clsHru
    Public Handle As Long
    Public Key As String
    Public CellCount As Integer
    Public Ids As New Generic.List(Of String)
    Public Area As Double
    Public SlopeMean As Double

    Public Sub New(ByVal aHru As clsHru)
        With aHru
            Key = .Key
            Handle = .Handle
            CellCount = .CellCount
            Ids.AddRange(.Ids)
            Area = .Area
            SlopeMean = .SlopeMean
        End With
    End Sub

    Public Sub New(ByVal aKey As String, ByVal aHandle As Integer, ByVal aCellCount As Integer, _
                   ByVal aIds As Generic.List(Of String), _
                   ByVal aArea As Double, ByVal aSlopeMean As Double)
        Key = aKey
        Handle = aHandle
        CellCount = aCellCount
        Ids.AddRange(aIds)
        Area = aArea
        SlopeMean = aSlopeMean
    End Sub

    Public Sub SetKeyFromIds()
        Key = ""
        For Each lValue As String In Ids
            Key &= lValue & "_"
        Next
        Key = Key.TrimEnd("_")
    End Sub
End Class

Public Class clsHruComparer
    Implements Collections.IComparer

    Public TagIndex As Integer = -1

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        If TagIndex < 0 Then
            Return x.CellCount.CompareTo(y.CellCount)
        Else
            Return x.Ids(TagIndex).CompareTo(y.Ids(TagIndex))
        End If
    End Function
End Class

Public Class clsHruComparerGeneric
    Implements Collections.Generic.IComparer(Of clsHru)

    Public TagIndex As Integer = -1

    Public Function Compare(ByVal x As clsHru, ByVal y As clsHru) As Integer Implements System.Collections.Generic.IComparer(Of clsHru).Compare
        If TagIndex < 0 Then
            Return x.CellCount.CompareTo(y.CellCount)
        Else
            Return x.Ids(TagIndex).CompareTo(y.Ids(TagIndex))
        End If
    End Function
End Class

Public Class clsHruTable
    Inherits KeyedCollection(Of String, clsHru)
    Implements IComparable

    Public TotalCellCount As Int64 = 0

    ''' <summary>
    ''' One tag for each layer contributing to HRU key, in same order as clsHru.Key and clsHru.Ids
    ''' </summary>
    ''' <remarks></remarks>
    Public Tags As New Generic.List(Of String)

    ''' <summary>
    ''' Create empty HRU table with the given set of layer tags
    ''' </summary>
    ''' <param name="aTags">One tag for each layer contributing to HRU key, in same order as clsHru.Key and clsHru.Ids</param>
    ''' <remarks></remarks>
    Sub New(ByVal aTags As Generic.List(Of String))
        Tags.AddRange(aTags)
    End Sub

    ''' <summary>
    ''' Create HRU table by reading saved table from file
    ''' </summary>
    ''' <param name="aFilename">full path of file to read</param>
    Sub New(ByVal aFilename As String)
        Dim lFile As New IO.StreamReader(aFilename)
        Dim lTagsToRead As Integer = lFile.ReadLine().Substring("Tags ".Length)
        While lTagsToRead > 0
            Tags.Add(lFile.ReadLine)
            lTagsToRead -= 1
        End While
        TotalCellCount = 0
        Dim lHrusToRead As Integer = lFile.ReadLine().Substring("HRUs ".Length)
        While lHrusToRead > 0
            Dim lHandle As Integer = lFile.ReadLine()
            Dim lCellCount As Integer = lFile.ReadLine()
            TotalCellCount += lCellCount
            Dim lArea As Double = lFile.ReadLine()
            Dim lSlopeMean As Double = lFile.ReadLine()
            Dim lKey As String = lFile.ReadLine()
            If lKey.Contains(":") Then
                lKey = lKey.Substring(0, lKey.IndexOf(":"))
            End If
            Dim lIds As New Generic.List(Of String)
            lIds.AddRange(lKey.Split("_"))
            Me.Add(New clsHru(lKey, lHandle, lCellCount, lIds, lArea, lSlopeMean))
            lHrusToRead -= 1
        End While
        lFile.Close()
    End Sub

    ''' <summary>
    ''' Get the key for the KeyedCollection base class
    ''' </summary>
    ''' <param name="aHru">item to get key of</param>
    Protected Overrides Function GetKeyForItem(ByVal aHru As clsHru) As String
        Return aHru.Key
    End Function

    ''' <summary>
    ''' Return the part of the key for the given HRU corresponding to the given layer tag
    ''' </summary>
    ''' <param name="aHru">HRU whose layer value is requested</param>
    ''' <param name="aTag">Tag of layer whose value for this HRU is requested</param>
    Function Id(ByVal aHru As clsHru, ByVal aTag As String) As String
        Return (aHru.Ids(Tags.IndexOf(aTag)))
    End Function

    ''' <summary>
    ''' Save contents of this table to a text file
    ''' </summary>
    ''' <param name="aFilename">Full path of file to save in</param>
    Public Sub Save(ByVal aFilename As String)
        Dim lFile As New IO.StreamWriter(aFilename)
        lFile.WriteLine("Tags " & Tags.Count)
        For Each lTag As String In Tags
            lFile.WriteLine(lTag)
        Next
        lFile.WriteLine("HRUs " & Me.Count)
        For Each lHru As clsHru In Me
            With lHru
                lFile.WriteLine(.Handle)
                lFile.WriteLine(.CellCount)
                lFile.WriteLine(.Area)
                lFile.WriteLine(.SlopeMean)
                lFile.WriteLine(.Key)
            End With
        Next
        lFile.Close()
    End Sub

    Public Sub ComputeTotalCellCount()
        TotalCellCount = 0
        For Each lHru As clsHru In Me
            TotalCellCount += lHru.CellCount
        Next
    End Sub

    Public Function CountCellsPerTagValue(ByVal aTag As String) As atcCollection
        Dim lTagIndex As Integer = Tags.IndexOf(aTag)
        Dim lValueTotals As New atcCollection
        For Each lHru As clsHru In Me
            lValueTotals.Increment(lHru.Ids(lTagIndex), lHru.CellCount)
        Next
        lValueTotals.SortByValue()
        Return lValueTotals
    End Function

    Public Function PredominantTagValue(ByVal aTag As String) As String
        Dim lMaxIndex As Integer = -1
        Dim lMaxCellCount As Int64 = 0
        Dim lSplit As atcCollection = SplitByTag(aTag)
        Dim lSplitIndex As Integer = 0
        For Each lTable As clsHruTable In lSplit
            If lTable.TotalCellCount > lMaxCellCount Then
                lMaxIndex = lSplitIndex
                lMaxCellCount = lSplit.ItemByIndex(lSplitIndex).TotalCellCount
            End If
            lSplitIndex += 1
        Next
        If lMaxIndex > -1 Then
            Return lSplit.Keys(lMaxIndex)
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aDescending"></param>
    ''' <param name="aTag">Tag to sort by, if omitted sort by cell count</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Sort(ByVal aDescending As Boolean, Optional ByVal aTag As String = "") As clsHruTable
        Dim lSortedTable As New clsHruTable(Tags)

        Dim lComparer As New clsHruComparerGeneric
        lComparer.TagIndex = Tags.IndexOf(aTag)
        Dim lSorter As New Generic.List(Of clsHru)
        lSorter.AddRange(Me.Items)
        lSorter.Sort(lComparer)

        If aDescending Then
            For lIndex As Integer = lSorter.Count - 1 To 0 Step -1
                lSortedTable.Add(lSorter(lIndex))
            Next
        Else
            For Each lHru As clsHru In lSorter
                lSortedTable.Add(lHru)
            Next
        End If

        lSortedTable.TotalCellCount = TotalCellCount
        Return lSortedTable
    End Function

    ''' <summary>
    ''' Split this table into a collection of smaller tables.
    ''' One new table is created for each unique value of aTag.
    ''' All elements sharing the same value of aTag are placed in the same resulting table.
    ''' </summary>
    ''' <param name="aTag">Name of tag to split by</param>
    ''' <remarks>returned collection is keyed by the unique tag value of each table</remarks>
    Public Function SplitByTag(ByVal aTag As String) As atcCollection
        Dim lTagIndex As Integer = Tags.IndexOf(aTag)
        Dim lHruTableOfTag As clsHruTable
        Dim lSplitTables As New atcCollection
        For Each lHru As clsHru In Me
            Dim lId As String = lHru.Ids(lTagIndex)
            Dim lIndex As Integer = lSplitTables.IndexFromKey(lId)
            If lIndex >= 0 Then
                lHruTableOfTag = lSplitTables.ItemByIndex(lIndex)
            Else
                lHruTableOfTag = New clsHruTable(Tags)
                lSplitTables.Add(lId, lHruTableOfTag)
            End If
            lHruTableOfTag.Add(lHru)
        Next
        For Each lHruTableOfTag In lSplitTables
            lHruTableOfTag.ComputeTotalCellCount()
        Next
        lSplitTables.SortByValue()
        Return lSplitTables
    End Function

    ''' <summary>
    ''' Recursively split and sort by each given tag
    ''' If aSortTags contains only one tag, returns collection of tables from SplitByTag.
    ''' If aSortTags contains more than one tag, split by the first one then return a collection of collections
    ''' from recursive calls on tags after the first one.
    ''' </summary>
    ''' <param name="aSortTags">Tags to group by</param>
    ''' <returns>collection of resulting tables or collections for each value of the first aSortTag</returns>
    ''' <remarks></remarks>
    Public Function SummarizeByTag(ByVal aSortTags As Generic.List(Of String)) As atcCollection
        Dim lCountGood As Int64 = TotalCellCount
        Dim lSplitTables As atcCollection = SplitByTag(aSortTags(0))
        Dim lResult As atcCollection

        If aSortTags.Count > 1 Then
            Dim lSortTagsRemaining As Generic.List(Of String) = aSortTags.GetRange(1, aSortTags.Count - 1)
            lResult = New atcCollection
            For lIndex As Integer = 0 To lSplitTables.Count - 1
                Dim lSplitTable As clsHruTable = lSplitTables.ItemByIndex(lIndex)
                lResult.Add(lSplitTables.Keys(lIndex), lSplitTable.SummarizeByTag(lSortTagsRemaining))
            Next
        Else
            lResult = lSplitTables
        End If

        Logger.Dbg(aSortTags(0) & "Count " & lSplitTables.Count & " CountGood " & lCountGood)
        Return lResult

    End Function

    ''' <summary>
    ''' Read a comma-separated values text file of IDs to change from and to
    ''' First column must contain values to change from and values must be unique.
    ''' Second column must contain values to change to and may contain duplicates.
    ''' </summary>
    ''' <param name="aCsvFileName">Full path of file to read</param>
    ''' <param name="aOriginalIDs">return parameter containing values to change from</param>
    ''' <param name="aNewIds">return parameter containing values to change to</param>
    ''' <remarks>useful before calling Reclassify</remarks>
    Public Sub ReadReclassifyCSV(ByVal aCsvFileName As String, _
                    ByRef aOriginalIDs As Generic.List(Of String), _
                    ByRef aNewIds As Generic.List(Of String), _
                    Optional ByVal aRemoveAfter As String = "")
        aOriginalIDs = New Generic.List(Of String)
        aNewIds = New Generic.List(Of String)
        For Each lLine As String In IO.File.ReadAllLines(aCsvFileName)
            Dim lFields() As String = lLine.Split(",")
            If lFields.Length = 2 Then
                aOriginalIDs.Add(lFields(0))
                If aRemoveAfter.Length > 0 AndAlso lFields(1).Contains(aRemoveAfter) Then
                    lFields(1) = lFields(1).Remove(lFields(1).IndexOf(aRemoveAfter))
                End If
                aNewIds.Add(lFields(1))
            End If
        Next
    End Sub

    ''' <summary>
    ''' Change values in all HRUs with tag value
    ''' aTag=[a value in aChangeFromValues]
    ''' to 
    ''' aTag=[value at same index in aChangeToValues]
    ''' </summary>
    ''' <param name="aTag"></param>
    ''' <param name="aChangeFromValues"></param>
    ''' <param name="aChangeToValues"></param>
    ''' <remarks></remarks>
    Public Sub Reclassify(ByVal aTag As String, _
                          ByVal aChangeFromValues As Generic.List(Of String), _
                          ByVal aChangeToValues As Generic.List(Of String))
        Dim lTagIndex As Integer = Tags.IndexOf(aTag)
        Dim lNumChanged As Integer = 0
        If lTagIndex < 0 Then
            Logger.Dbg("ChangeTag:TagNotFound:" & aTag)
        Else
            For Each lHru As clsHru In Me
                Dim lReplacmentIndex As Integer = aChangeFromValues.IndexOf(lHru.Ids(lTagIndex))
                If lReplacmentIndex >= 0 Then
                    lHru.Ids(lTagIndex) = aChangeToValues(lReplacmentIndex)
                    lHru.SetKeyFromIds()
                    lNumChanged += 1
                End If
            Next
            If lNumChanged > 0 Then
                Logger.Dbg("Reclassify changed " & aTag & " for " & lNumChanged & " of " & Me.Count)
                EnsureUnique()
            Else
                Logger.Dbg("Reclassify made no changes to " & aTag)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Scan all existing HRUs and consolidate any with matching keys
    ''' </summary>
    ''' <remarks>
    ''' When combining two HRUs:
    ''' Slope mean is recomputed as area-weighted average
    ''' Cell count and area are sums of existing values
    ''' </remarks>
    Public Sub EnsureUnique()
        Dim lCountBefore As Integer = Me.Count
        For lCheckIndex1 As Integer = Me.Count - 1 To 0 Step -1
            Dim lHru1 As clsHru = Me.Item(lCheckIndex1)
            For lCheckIndex2 As Integer = lCheckIndex1 - 1 To 0 Step -1
                If lHru1.Key = Me.Item(lCheckIndex2).Key Then
                    With Me.Item(lCheckIndex2)
                        'TODO: keep track of which indexes are mapped to which other ones so we can interpret HRU grid, .Handles.Add?
                        'Logger.Dbg("Merging duplicate HRU " & .Key)
                        .Area += lHru1.Area
                        Dim lNewCellCount As Int64 = .CellCount + lHru1.CellCount
                        .SlopeMean = (.SlopeMean * .CellCount + lHru1.SlopeMean * lHru1.CellCount) / lNewCellCount
                        .CellCount = lNewCellCount
                    End With
                    Me.RemoveAt(lCheckIndex1)
                    Exit For
                End If
            Next
        Next
        Logger.Dbg("EnsureUnique number of HRUs changed from " & lCountBefore & " to " & Me.Count)
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return TotalCellCount.CompareTo(obj.TotalCellCount)
    End Function
End Class
