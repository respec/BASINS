Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized

Module ScriptWdmCreateFromGrids
    Private pTestPath As String = "D:\GisData\Illinois Snow\Cook_04-05\200711817383"
    Private pPolyIdName As String = "MidlothianTinley"
    Private pPolyIdFieldName As String = "FIPS"
    Private pUTCAdj As Double = -5
    Private pWdmName As String = "GridData.wdm"
    Private pAggregation As String = "Aver" 'Min, Max
    Private pDebug As Boolean = False

    Private Structure SnodasData
        Dim DateObs As Double
        Dim Constituent As String
        Dim Id As Integer
        Dim FileName As String
    End Structure

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())
        GisUtil.MappingObject = aMapWin

        Dim lGridLayersToProcess As New NameValueCollection
        AddFilesInDir(lGridLayersToProcess, pTestPath, False, "*.bil")
        Logger.Dbg("Process " & lGridLayersToProcess.Count & " files")
        Dim lSnodasData As New atcCollection 'of SnodasData
        For lIndex As Integer = 0 To lGridLayersToProcess.Count - 1
            ParseFileName(lGridLayersToProcess(lIndex), lSnodasData)
        Next

        Dim lConstituents As New atcCollection
        For lIndex As Integer = 0 To lSnodasData.Count - 1
            With lSnodasData(lIndex)
                If Not .Constituent = Nothing Then
                    lConstituents.Increment(.Constituent, 1)
                End If
            End With
        Next
        Logger.Dbg("ConstituentCount:" & lConstituents.Count)

        Dim lBaseGrid As New MapWinGIS.Grid

        Dim lPolyIdGridName As String = pPolyIdName & ".tif"
        If FileExists(lPolyIdGridName) Then
            Logger.Dbg("UsingExistingPolyIdGridFile:" & lPolyIdGridName)
        Else 'create it
            Dim lPolyLayer As New MapWinGIS.Shapefile
            lPolyLayer.Open(pPolyIdName & ".shp")
            Dim lPolyFieldIndex As Integer = 0 'TODO:get index from a fieldname name

            lBaseGrid.Open(lSnodasData(0).FileName)
            lBaseGrid.Header.NodataValue = 65535
            Dim lVals(lBaseGrid.Header.NumberCols) As Single
            lPolyLayer.BeginPointInShapefile()
            For lRow As Integer = 0 To lBaseGrid.Header.NumberRows
                For lCol As Integer = 0 To lBaseGrid.Header.NumberCols
                    Dim lX, ly As Double
                    lBaseGrid.CellToProj(lCol, lRow, lX, ly)
                    Dim lPolyIndex As Integer = lPolyLayer.PointInShapefile(lX, ly)
                    If lPolyIndex = -1 Then
                        lVals(lCol) = lBaseGrid.Header.NodataValue
                    Else
                        lVals(lCol) = lPolyIndex
                    End If
                Next
                lBaseGrid.PutRow(lRow, lVals(0))
            Next
            lPolyLayer.EndPointInShapefile()
            lBaseGrid.Save(lPolyIdGridName, MapWinGIS.GridFileType.GeoTiff)
            Logger.Dbg("DoneGridCreateFromPoly:Name:" & lPolyIdGridName)
            lBaseGrid.Close()
        End If

        Dim lPolyIdGrid As New MapWinGIS.Grid
        lPolyIdGrid.Open(lPolyIdGridName)
        Dim lPolyIdGridNoData As Double = lPolyIdGrid.Header.NodataValue

        'process the data into timeseries
        Dim lDataSource As New atcDataSource

        For lIndexConstituent As Integer = 0 To lConstituents.Count - 1
            Dim lKey As String = lConstituents.Keys(lIndexConstituent)
            Logger.Dbg("Process:" & lKey)
            If lkey = "SnoSubBl" Then
                Logger.Dbg("LookForProblems")
            End If
            Dim lDates As New atcTimeseries(lDataSource)
            lDates.numValues = lConstituents.ItemByIndex(lIndexConstituent)
            Dim lDateCount As Integer = 0
            For lIndexFile As Integer = 0 To lSnodasData.Count - 1
                If lSnodasData(lIndexFile).Constituent = lKey Then
                    Logger.Dbg(" Date:" & DumpDate(lSnodasData(lIndexFile).DateObs))
                    lDateCount += 1
                    If lDateCount = 1 Then 'set first date
                        lDates.Value(0) = lSnodasData(lIndexFile).DateObs - 1 'assumes daily data
                    End If
                    Dim lDateIndex As Integer = timdifJ(lDates.Value(0), lSnodasData(lIndexFile).dateobs, 4, 1)  'assumes daily data
                    If lDateCount < lDateIndex Then
                        Logger.Dbg("DateCountProblem:" & lDateIndex & ":" & lDateCount)
                        For lDateFillIndex As Integer = lDateCount To lDateIndex - 1
                            lDates.Value(lDateCount) = TimAddJ(lDates.Value(0), 4, 1, lDateCount) 'assume daily date
                            lDateCount += 1
                        Next
                    ElseIf lDateCount > lDateIndex Then
                        Logger.Dbg("BigDateCountProblem:" & lDateIndex & ":" & lDateCount)
                    End If
                    lDates.Value(lDateCount) = lSnodasData(lIndexFile).DateObs

                    Dim lSum(lPolyIdGrid.Maximum) As Double
                    Dim lMin(lPolyIdGrid.Maximum) As Double
                    Dim lMax(lPolyIdGrid.Maximum) As Double
                    For lIndex As Integer = 0 To lPolyIdGrid.Maximum
                        lMin(lIndex) = Double.PositiveInfinity
                        lMax(lIndex) = Double.NegativeInfinity
                    Next
                    Dim lCount(lPolyIdGrid.Maximum) As Integer
                    Dim lCountBaseNoData As Integer = 0
                    Dim lCountPolyNoData As Integer = 0

                    lBaseGrid.Open(lSnodasData(lIndexFile).FileName)
                    Dim lBaseGridNoData As Double = lBaseGrid.Header.NodataValue

                    For lRow As Integer = 1 To lPolyIdGrid.Header.NumberRows
                        For lCol As Integer = 1 To lPolyIdGrid.Header.NumberCols
                            Dim lPolyIndex As Double = lPolyIdGrid.Value(lCol, lRow)
                            If lPolyIndex <> lPolyIdGridNoData Then
                                Dim lValue As Double = lBaseGrid.Value(lCol, lRow)
                                If lValue <> lBaseGridNoData Then
                                    If lValue > 65000 Then
                                        lValue -= 65535
                                    End If
                                    If Math.Abs(lValue - 55537) < 0.001 Then
                                        lCountBaseNoData += 1
                                    Else
                                        If lKey = "SnoTemp" And lValue > 0.0 Then 'convert to degC
                                            lValue -= 273
                                        End If
                                        lSum(lPolyIndex) += lValue
                                        lCount(lPolyIndex) += 1
                                        If lMin(lPolyIndex) > lValue Then
                                            lMin(lPolyIndex) = lValue
                                        End If
                                        If lMax(lPolyIndex) < lValue Then
                                            lMax(lPolyIndex) = lValue
                                        End If
                                    End If
                                Else
                                    lCountBaseNoData += 1
                                End If
                            Else
                                lCountPolyNoData += 1
                            End If
                        Next
                    Next
                    Logger.Dbg("  Base:Min:" & DoubleToString(lBaseGrid.Minimum) & _
                                     " Max:" & DoubleToString(lBaseGrid.Maximum) & _
                                     " NoDataCount: " & lCountBaseNoData)
                    lBaseGrid.Close()

                    Dim lAver As Double
                    For lIndex As Integer = 0 To lPolyIdGrid.Maximum
                        Dim lId As Integer = lSnodasData(lIndexFile).Id * 1000 + lIndex
                        Dim lTimser As atcTimeseries = lDataSource.DataSets.ItemByKey(lId)
                        If lTimser Is Nothing Then
                            lTimser = New atcTimeseries(lDataSource)
                            lTimser.Dates = lDates
                            lTimser.numValues = lDates.numValues
                            lTimser.Attributes.SetValue("Id", lId)
                            lTimser.Attributes.SetValue("Locn", lIndex)
                            lTimser.Attributes.SetValue("Scen", "SNODAS")
                            lTimser.Attributes.SetValue("TSFILL", -999)
                            lTimser.Attributes.SetValue("Cons", lConstituents.Keys(lIndexConstituent))
                            lTimser.Attributes.SetValue("ts", 1)
                            lTimser.Attributes.SetValue("tu", 4) 'todo: check this, might be hourly
                            lDataSource.DataSets.Add(lId, lTimser)
                            'TODO: set any other attributes
                        End If
                        If lCount(lIndex) > 0 Then
                            lAver = lSum(lIndex) / lCount(lIndex)
                            If pDebug Then
                                Logger.Dbg("   Id:" & lIndex & _
                                           " Aver:" & DoubleToString(lAver) & _
                                           " Min:" & lMin(lIndex) & _
                                           " Max:" & lMax(lIndex) & _
                                           " Sum:" & lSum(lIndex) & _
                                           " Count:" & lCount(lIndex))
                            End If
                        Else 'no data
                            lAver = Double.NaN
                            lMin(lIndex) = Double.NaN
                            lMax(lIndex) = Double.NaN
                        End If
                        Select Case pAggregation.ToLower
                            Case "aver" : lTimser.Value(lDateCount) = lAver
                            Case "min" : lTimser.Value(lDateCount) = lMin(lIndex)
                            Case "max" : lTimser.Value(lDateCount) = lMax(lIndex)
                        End Select
                    Next
                    If pDebug Then Logger.Dbg("  Skip:PolyNoData:" & lCountPolyNoData & " BaseNoData:" & lCountBaseNoData)
                End If
            Next
        Next
        Logger.Dbg("DataSetCount:" & lDataSource.DataSets.Count)
        Dim lWdm As New atcDataSourceWDM
        If lWdm.Open(pWdmName) Then
            For Each lDataset As atcDataSet In lDataSource.DataSets
                lWdm.AddDataset(lDataset, atcDataSource.EnumExistAction.ExistAppend)
            Next
        End If
        Logger.Dbg("AllDone")
    End Sub

    Private Sub ParseFileName(ByVal aFileName As String, ByRef aSnodasData As atcCollection)
        Dim lPos As Integer = aFileName.IndexOf("ssmv") + 5
        If lPos > 4 Then
            Dim lParmCode As Integer = aFileName.Substring(lPos, 4)
            Dim lSnodasDatum As SnodasData = Nothing
            With lSnodasDatum
                Select Case lParmCode
                    Case 1034 : .Constituent = "SnoWatEq"
                        .Id = 1
                    Case 1036 : .Constituent = "SnoDep"
                        .Id = 2
                    Case 1044 : .Constituent = "SnoMltB"
                        .Id = 3
                    Case 1050 : .Constituent = "SnoSubPk"
                        .Id = 4
                    Case 1039 : .Constituent = "SnoSubBl"
                        .Id = 5
                    Case 1025
                        lPos = aFileName.IndexOf("sll") + 4
                        If aFileName.Substring(lPos, 1) = 1 Then
                            .Constituent = "Snow" : .Id = 6
                        Else
                            .Constituent = "Rain" : .Id = 7
                        End If
                    Case 1038 : .Constituent = "SnoTemp" : .Id = 8
                End Select
                lPos = aFileName.ToUpper.IndexOf("NATS") + 4
                Dim lDateStr As String = aFileName.Substring(lPos, 10)
                Dim lDateJ As Double = Jday(lDateStr.Substring(0, 4), _
                                            lDateStr.Substring(4, 2), _
                                            lDateStr.Substring(6, 2), _
                                            lDateStr.Substring(8, 2) + pUTCAdj, 0, 0)
                .DateObs = lDateJ
                .FileName = aFileName
            End With
            aSnodasData.Add(lSnodasDatum)
        End If
    End Sub
End Module
