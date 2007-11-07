Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized

Module ScriptWdmCreateFromGrids
    Private pTestPath As String = "D:\GisData\Illinois Snow\Excerpt"
    Private pPolyIdFieldName As String = "FIPS"
    Private pPolyName = "County"
    Private pUTCAdj As Double = -5
    Private pWdmName As String = "GridData.wdm"

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
        Dim lSnodasData As New atcCollection 'of SnodasData
        For lIndex As Integer = 0 To lGridLayersToProcess.Count - 1
            ParseFileName(lGridLayersToProcess(lIndex), lSnodasData)
        Next

        Dim lConstituents As New atcCollection
        For lIndex As Integer = 0 To lSnodasData.Count - 1
            With lSnodasData(lIndex)
                If Not .Constituent = Nothing AndAlso lConstituents.ItemByKey(.Constituent) = Nothing AndAlso .Constituent.Length > 0 Then
                    lConstituents.Add(.Constituent)
                End If
            End With
        Next

        Dim lBaseGrid As New MapWinGIS.Grid
        lBaseGrid.Open(lSnodasData(0).FileName)

        Dim lPolyIdGridName As String = "PolyIdGrid.tif"
        If Not FileExists(lPolyIdGridName) Then 'create it
            Dim lPolyLayer As New MapWinGIS.Shapefile
            lPolyLayer.Open(pPolyName & ".shp")
            Dim lPolyFieldIndex As Integer = 0 'TODO:get index from a fieldname name

            Dim lVals(lBaseGrid.Header.NumberCols) As Single
            lPolyLayer.BeginPointInShapefile()
            For lRow As Integer = 1 To lBaseGrid.Header.NumberRows
                For lCol As Integer = 1 To lBaseGrid.Header.NumberCols
                    Dim lX, ly As Double
                    lBaseGrid.CellToProj(lCol, lRow, lX, ly)
                    Dim lPolyIndex As Integer = lPolyLayer.PointInShapefile(lX, ly)
                    lVals(lCol - 1) = lPolyIndex 'lPolyLayer.CellValue(lPolyFieldIndex, lPolyIndex)
                Next
                lBaseGrid.PutRow(lRow, lVals(0))
            Next
            lPolyLayer.EndPointInShapefile()
            lBaseGrid.Save(lPolyIdGridName, MapWinGIS.GridFileType.GeoTiff)
            lBaseGrid.Close()
            Logger.Dbg("DoneGridCreateFromPoly")
        End If

        Dim lPolyIdGrid As New MapWinGIS.Grid
        lPolyIdGrid.Open(lPolyIdGridName)
        Dim lPolyIdGridNoData As Integer = lPolyIdGrid.Header.NodataValue

        'process the data into timeseries
        Dim lDataSource As New atcDataSource

        For lIndexConstituent As Integer = 0 To lConstituents.Count - 1
            Logger.Dbg("Process:" & lConstituents.ItemByIndex(lIndexConstituent))
            Dim lDates As New atcTimeseries(lDataSource)
            For lIndexFile As Integer = 0 To lSnodasData.Count - 1
                If lSnodasData(lIndexFile).Constituent = lConstituents.ItemByIndex(lIndexConstituent) Then
                    Logger.Dbg(" Date:" & DumpDate(lSnodasData(lIndexFile).DateObs))
                    lDates.numValues += 1
                    lDates.Value(lDates.numValues) = lSnodasData(lIndexFile).DateObs
                    If lDates.numValues = 1 Then 'set first date
                        lDates.Value(0) = lSnodasData(lIndexFile).DateObs - 1 'assumes daily data
                    End If

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
                    Dim lBaseGridNoData As Integer = lBaseGrid.Header.NodataValue
                    Logger.Dbg("Base:Min:" & DoubleToString(lBaseGrid.Minimum) & " Max: " & DoubleToString(lBaseGrid.Maximum))

                    For lRow As Integer = 1 To lPolyIdGrid.Header.NumberRows
                        For lCol As Integer = 1 To lPolyIdGrid.Header.NumberCols
                            Dim lPolyIndex As Integer = lPolyIdGrid.Value(lCol, lRow)
                            If lPolyIndex <> lPolyIdGridNoData Then
                                Dim lValue As Double = lBaseGrid.Value(lCol, lRow)
                                If lValue <> lBaseGridNoData Then
                                    lSum(lPolyIndex) += lValue
                                    lCount(lPolyIndex) += 1
                                    If lMin(lPolyIndex) > lValue Then lMin(lPolyIndex) = lValue
                                    If lMax(lPolyIndex) < lValue Then lMax(lPolyIndex) = lValue
                                Else
                                    lCountBaseNoData += 1
                                End If
                            Else
                                lCountPolyNoData += 1
                            End If
                        Next
                    Next
                    lBaseGrid.Close()

                    Dim lAver As Double
                    For lIndex As Integer = 0 To lPolyIdGrid.Maximum
                        If lCount(lIndex) > 0 Then
                            lAver = lSum(lIndex) / lCount(lIndex)
                            Logger.Dbg("Id:" & lIndex & " Aver:" & DoubleToString(lAver) & _
                                                        " Min:" & lMin(lIndex) & _
                                                        " Max:" & lMax(lIndex) & _
                                                        " Sum:" & lSum(lIndex) & _
                                                        " Count:" & lCount(lIndex))
                            Dim lId As Integer = lSnodasData(lIndexFile).Id * 1000 + lIndex
                            Dim lTimser As atcTimeseries = lDataSource.DataSets.ItemByKey(lId)
                            If lTimser Is Nothing Then
                                lTimser = New atcTimeseries(lDataSource)
                                lTimser.Dates = lDates
                                lTimser.Attributes.SetValue("Id", lId)
                                lTimser.Attributes.SetValue("Locn", lIndex)
                                lTimser.Attributes.SetValue("Cons", lConstituents.ItemByIndex(lIndexConstituent))
                                lTimser.Attributes.SetValue("ts", 1)
                                lTimser.Attributes.SetValue("tu", 4) 'todo: check this
                                lDataSource.DataSets.Add(lId, lTimser)
                                'TODO: set attributes
                            End If
                            lTimser.numValues += 1
                            lTimser.Value(lTimser.numValues) = lAver
                        Else
                            lAver = 0
                            lMin(lIndex) = 0
                            lMax(lIndex) = 0
                        End If
                    Next
                    Logger.Dbg("Skip:PolyNoData:" & lCountPolyNoData & " BaseNoData:" & lCountBaseNoData)
                End If
            Next
        Next
        Logger.Dbg("DataSetCount:" & lDataSource.DataSets.Count)
        Dim lWdm As New atcDataSourceWDM
        lWdm.Open(pWdmName)
        lWdm.AddDatasets(lDataSource.DataSets) 'TODO: add quiet parm to AddDatasets
        lWdm.Save(pWdmName)
    End Sub

    Private Sub ParseFileName(ByVal aFileName As String, ByRef aSnodasData As atcCollection)
        Dim lPos As Integer = aFileName.IndexOf("ssmv") + 5
        If lPos > 4 Then
            Dim lParmCode As Integer = aFileName.Substring(lPos, 4)
            Dim lSnodasDatum As SnodasData = Nothing
            With lSnodasDatum
                Select Case lParmCode
                    Case 1034 : .Constituent = "SnoWatEq" : .Id = 1
                    Case 1036 : .Constituent = "SnoDep" : .Id = 2
                    Case 1044 : .Constituent = "SnoMltB" : .Id = 3
                    Case 1050 : .Constituent = "SnoSubPk" : .Id = 4
                    Case 1039 : .Constituent = "SnoSubBl" : .Id = 5
                    Case 1025
                        lPos = aFileName.IndexOf("sll") + 5
                        If aFileName.Substring(lPos, 1) = 1 Then
                            .Constituent = "Snow" : .Id = 6
                        Else
                            .Constituent = "Rain" : .Id = 7
                        End If
                    Case 1038 : .Constituent = "SnoTemp" : .Id = 8
                End Select
                lPos = aFileName.IndexOf("NATS") + 4
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
