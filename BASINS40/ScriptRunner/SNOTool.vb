Imports ICSharpCode.SharpZipLib
Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized
Imports System.IO
Imports System.Math


Module ScriptSNOTool

    Private pInputPath As String = "C:\GisData\Illinois Snow\FromTom\" 'location of downloaded tar files
    Private pOutputPath As String = "C:\GisData\Illinois Snow\FromTom\Output\" 'location of extracted data and resulting WDM file
    Private pFileFilter As String = "*.tar" 'indicates files to extract data from
    Private pGISDataPath As String = "C:\GisData\Illinois Snow\Coverages\"
    Private pPolyIdName As String = "coop_poly2" 'name of shape file to use for aggregating grid values
    Private pPolyIdFieldName As String = "FIPS" 'field name in grid on which aggregations will be based
    Private pUTCAdj As Double = -5
    Private pWdmName As String = "GridData.wdm" 'output WDM file on which data will be saved
    Private pAggregation As String = "Aver" 'Min, Max
    Private pDebug As Boolean = False

    Private Structure SnodasData
        Dim DateObs As Double
        Dim Constituent As String
        Dim Id As Integer
        Dim FileName As String
    End Structure

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg(" Start")
        'Logger.Dbg(" CurDir:" & CurDir())
        GisUtil.MappingObject = aMapWin

        'extract and uncompress data 
        Logger.Status("Unpacking data from tar files", True)
        UnpackData(pInputPath, pOutputPath, pFileFilter)

        ChDriveDir(pOutputPath)  'change to the output data directory

        Dim lGroupBuilder As New atcTimeseriesGroupBuilder(Nothing)
        Dim lTSBuilder As atcTimeseriesBuilder
        Dim lTSValue As Double
        Dim lDate As Double

        Dim lGridLayersToProcess As New NameValueCollection
        AddFilesInDir(lGridLayersToProcess, pOutputPath, False, "*.bil")
        Logger.Status(" Process " & lGridLayersToProcess.Count & " files", True)
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
        Logger.Status(" ConstituentCount:" & lConstituents.Count, True)

        Dim lBaseGrid As New MapWinGIS.Grid

        Dim lPolyIdGridName As String = pPolyIdName & ".tif"
        If FileExists(lPolyIdGridName) Then
            Logger.Dbg(" UsingExistingPolyIdGridFile:" & lPolyIdGridName)
        Else 'create it
            Dim lPolyLayer As New MapWinGIS.Shapefile
            If lPolyLayer.Open(pGISDataPath & pPolyIdName & ".shp") Then
                Dim lPolyFieldIndex As Integer = 0 'TODO:get index from a fieldname name

                If lBaseGrid.Open(lSnodasData(0).FileName) Then
                    lBaseGrid.Header.NodataValue = 65535
                    Dim lVals(lBaseGrid.Header.NumberCols) As Single
                    lPolyLayer.BeginPointInShapefile()
                    For lRow As Integer = 0 To lBaseGrid.Header.NumberRows
                        For lCol As Integer = 0 To lBaseGrid.Header.NumberCols
                            Dim lX, ly As Double
                            lBaseGrid.CellToProj(lCol, lRow, lX, ly)
                            If lRow = 0 And lCol = 0 Then
                                Logger.Dbg(" Initial X:Y coordinates: " & lX & " : " & ly)
                            End If
                            Dim lPolyIndex As Integer = lPolyLayer.PointInShapefile(lX, ly)
                            If lPolyIndex = -1 Then
                                lVals(lCol) = lBaseGrid.Header.NodataValue
                            Else
                                Logger.Dbg(" NOTE: Matched grid at Row/Col " & lRow & "/" & lCol)
                                lVals(lCol) = lPolyIndex
                            End If
                        Next
                        lBaseGrid.PutRow(lRow, lVals(0))
                    Next
                    lPolyLayer.EndPointInShapefile()
                    lBaseGrid.Save(lPolyIdGridName, MapWinGIS.GridFileType.GeoTiff)
                    Logger.Dbg(" DoneGridCreateFromPoly:Name:" & lPolyIdGridName)
                    lBaseGrid.Close()
                Else 'couldn't find SNODAS grid file, big problem
                    Logger.Status(" STOP - Could not open SNODAS grid file " & lSnodasData(0).Filename)
                    Exit Sub
                End If
            Else 'couldn't find shape file, BIG problem (right Tom?!)
                Logger.Status(" STOP - Could not open shape file " & pGISDataPath & pPolyIdName & ".shp")
                Exit Sub
            End If
        End If

        Dim lPolyIdGrid As New MapWinGIS.Grid
        lPolyIdGrid.Open(lPolyIdGridName)
        Dim lPolyIdGridNoData As Double = lPolyIdGrid.Header.NodataValue

        For lIndexConstituent As Integer = 0 To lConstituents.Count - 1
            Dim lKey As String = lConstituents.Keys(lIndexConstituent)
            Logger.Status(" Processing:" & lKey, True)
            If lKey = "SnoSubBl" Then
                Logger.Dbg(" LookForProblems")
            End If
            Dim lDateCount As Integer = 0
            For lIndexFile As Integer = 0 To lSnodasData.Count - 1
                If lSnodasData(lIndexFile).Constituent = lKey Then
                    lDate = lSnodasData(lIndexFile).DateObs
                    Logger.Dbg(" Date:" & DumpDate(lSnodasData(lIndexFile).DateObs))
                    lDateCount += 1

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
                                Dim lGridValue As Double = lBaseGrid.Value(lCol, lRow)
                                If lGridValue <> lBaseGridNoData Then
                                    If lGridValue > 65000 Then
                                        lGridValue -= 65535
                                    End If
                                    If Abs(lGridValue - 55537) < 0.001 Then
                                        lCountBaseNoData += 1
                                    Else
                                        If lKey = "SnoTemp" And lGridValue > 0.0 Then 'convert to degC
                                            lGridValue -= 273
                                        End If
                                        lSum(lPolyIndex) += lGridValue
                                        lCount(lPolyIndex) += 1
                                        If lMin(lPolyIndex) > lGridValue Then
                                            lMin(lPolyIndex) = lGridValue
                                        End If
                                        If lMax(lPolyIndex) < lGridValue Then
                                            lMax(lPolyIndex) = lGridValue
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
                        lTSBuilder = lGroupBuilder.Builder(lIndex & ":" & lConstituents.Keys(lIndexConstituent))
                        With lTSBuilder.Attributes
                            'Set attributes of newly created builder
                            If Not .ContainsAttribute("Location") Then
                                .SetValue("Id", lId)
                                .SetValue("Locn", lIndex)
                                .SetValue("Scen", "SNODAS")
                                .SetValue("TSFILL", -999)
                                .SetValue("Cons", lConstituents.Keys(lIndexConstituent))
                                .SetValue("ts", 1)
                                .SetValue("tu", 4) 'todo: check this, might be hourly
                            End If
                        End With
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
                            Case "aver" : lTSValue = lAver
                            Case "min" : lTSValue = lMin(lIndex)
                            Case "max" : lTSValue = lMax(lIndex)
                        End Select
                        lTSBuilder.AddValue(lDate, lTSValue)
                    Next
                    If pDebug Then Logger.Dbg("   Skip:PolyNoData:" & lCountPolyNoData & " BaseNoData:" & lCountBaseNoData)
                End If
            Next
            Logger.Progress(lIndexConstituent + 1, lConstituents.Count)
        Next
            Dim lWdm As New atcDataSourceWDM
            If lWdm.Open(pWdmName) Then
                Dim lDatasets As atcDataGroup = lGroupBuilder.CreateTimeseriesGroup
                Logger.Dbg(" DataSetCount:" & lDatasets.Count)
                For Each lDataset As atcDataSet In lDatasets
                    Dim lCITSer As atcTimeseries = FillValues(lDataset, atcTimeUnit.TUDay, 1, -999)
                    lWdm.AddDataset(lCITSer, atcDataSource.EnumExistAction.ExistReplace)
                Next
            End If
            Logger.Status(" SNOTool Processing Complete", True)
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

    Private Sub UnpackData(ByVal aInputPath As String, ByVal aOutputPath As String, ByVal aFileFilter As String)

        Dim lUnzipper As GZip.GZipInputStream
        Dim lTar As Tar.TarArchive
        Dim ios As Stream
        Dim lFileStream As FileStream
        Dim lData(2048) As Byte
        Dim lSize As Integer
        Dim lFileCnt As Integer = 0

        Dim lFName As String
        Dim lZipFiles As NameValueCollection = Nothing
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, aInputPath, True, aFileFilter)
        Logger.Dbg(" Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Status("  Unpacking " & lFile, True)
            ios = File.OpenRead(lFile)
            lTar = Tar.TarArchive.CreateInputTarArchive(ios)
            lFName = FilenameNoPath(lFile)
            lTar.ExtractContents(aOutputPath)
            lTar.CloseArchive()
            ios.Close()
            AddFilesInDir(lZipFiles, aOutputPath, False, "*.gz")
            For Each lZipFile As String In lZipFiles
                Logger.Dbg("    Unzipping " & lZipFile)
                lUnzipper = New GZip.GZipInputStream(File.OpenRead(lZipFile))
                lFileStream = File.Create(aOutputPath & "\temp.dat")
                While (True)
                    lSize = lUnzipper.Read(lData, 0, 1024)
                    If lSize > 0 Then
                        lFileStream.Write(lData, 0, lSize)
                    Else
                        Exit While
                    End If
                End While
                lUnzipper.Close()
                lFileStream.Close()
                'save in original compressed file name without ".gz" ending
                TryDelete(lZipFile.Substring(0, lZipFile.Length - 3))
                File.Copy(aOutputPath & "\temp.dat", lZipFile.Substring(0, lZipFile.Length - 3))
                File.Delete(lZipFile)
            Next
            File.Delete(aOutputPath & "\temp.dat")
            lZipFiles.Clear()
            lFileCnt += 1
            Logger.Progress(lFileCnt, lFiles.Count)
        Next

    End Sub
End Module
