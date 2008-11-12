Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFOutputReports
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String
    Private pGraphAnnual As Boolean = False
    Private pCurveStepType As String = "RearwardStep"
    Private pSummaryTypes As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"

        'Dim lTestName As String = "tinley"
        'Dim lTestName As String = "hspf"
        'Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "shena"
        'Dim lTestName As String = "mono_lu2030a2_base"
        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "housatonic"
        'Dim lTestName As String = "beaver"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        'Dim lTestName As String = "SantaClara"

        pSummaryTypes.Add("Water")
        pSummaryTypes.Add("Sediment")

        Select Case lTestName
            Case "mono"
                pTestPath = "d:\mono_base"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
            Case "mono_lu2030a2_base"
                pTestPath = "D:\mono_luChange\output\lu2030a2"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
            Case "mono_lu2030a2_a_10_cccm_F10"
                pTestPath = "D:\mono_luChange\output\lu2030a2"
                pBaseName = "a_10_cccm_F10.base"
                pOutputLocations.Add("R:9")
            Case "housatonic"
                pTestPath = "d:\projects\housatonic\jackBin"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
                'pOutputLocations.Add("R:110")
                'pOutputLocations.Add("R:400")
                'pOutputLocations.Add("R:820")
                'pOutputLocations.Add("R:540")
                'pOutputLocations.Add("R:600")
                'pOutputLocations.Add("R:900")
            Case "beaver"
                pTestPath = "g:\projects\beaver\tds"
                pBaseName = "beaver-TDS-Run01"
                pOutputLocations.Add("R:360")
                pCurveStepType = "NonStep"
                If Not pSummaryTypes.Contains("Sediment") Then
                    pSummaryTypes.Add("Sediment")
                End If
            Case "shena"
                pTestPath = "c:\test\genscn"
                pBaseName = "base"
                pOutputLocations.Add("Lynnwood")
            Case "upatoi"
                pTestPath = "D:\Basins\modelout\Upatoi"
                'pTestPath = "C:\Basins\data\20710-01\Upatoi"
                pBaseName = "Upatoi"
                pOutputLocations.Add("R:14")
                pOutputLocations.Add("R:34")
                pOutputLocations.Add("R:46")
                pOutputLocations.Add("R:62")
                pOutputLocations.Add("R:74")
                pGraphAnnual = True
                pCurveStepType = "NonStep" 'Tony's convention 
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101, 201, 301, 401, 501, 601, 701, 801, 901, 951} '{101, 201, 301, 401, 501, 601, 701, 801, 901, 924}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102, 202, 302, 402, 502, 602, 702, 802, 902, 952} '{101, 201, 301, 401, 501, 601, 701, 801, 901, 905}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
            Case "tinley"
                pTestPath = "c:\test\tinley"
                pBaseName = "tinley"
                pOutputLocations.Add("R:850")
            Case "calleguas_cat"
                pTestPath = "D:\MountainViewData\Calleguas\cat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "calleguas_nocat"
                pTestPath = "D:\MountainViewData\Calleguas\nocat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "hyd_man"
                pTestPath = "C:\test\EXP_CAL\hyd_man.net"
                pBaseName = "hyd_man"
                pOutputLocations.Add("R:5")
                pOutputLocations.Add("R:4")
            Case "hspf"
                pTestPath = "C:\test\HSPF"
                pBaseName = "test10"
            Case "SantaClara"
                pTestPath = "D:\MountainViewData\SantaClara\nocat"
                pBaseName = "SCR10"
                pOutputLocations.Add("R:70")
                pOutputLocations.Add("R:180")
                pOutputLocations.Add("R:320")
                pOutputLocations.Add("R:410")
                pOutputLocations.Add("R:880")
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        If FileExists(pBaseName & "Orig.uci") Then
            IO.File.Copy(pBaseName & "Orig.uci", pBaseName & ".uci")
        End If

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        If pOutputLocations.Contains("Lynnwood") Then 'special case to check GenScn examples
            With lHspfUci.GlobalBlock
                .SDate(0) = 1986
                .SDate(1) = 10
                .SDate(2) = 1
                .EDate(0) = 1987
                .EDate(1) = 10
                .EDate(2) = 1
            End With
        End If
        'lHspfUci.Save()

        'open HBN file
        'TODO: need to allow additional binary output files!
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lRunMade As String = lHspfBinFileInfo.LastWriteTime.ToString

        If pSummaryTypes.Contains("Water") Then
            'open WDM file
            Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)

            Dim lExpertSystemFileNames As New NameValueCollection
            AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
            Dim lExpertSystem As HspfSupport.atcExpertSystem
            For Each lExpertSystemFileName As String In lExpertSystemFileNames
                Try
                    Dim lFileCopied As Boolean = False
                    If IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                        lFileCopied = TryCopy(lExpertSystemFileName, pBaseName & ".exs")
                    End If
                    lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lWdmDataSource)
                    Dim lStr As String = lExpertSystem.Report
                    SaveFileString("outfiles\ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)

                    'lStr = lExpertSystem.AsString 'NOTE:just testing
                    'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                    Dim lCons As String = "Flow"
                    For Each lSite As hexSite In lExpertSystem.Sites
                        Dim lSiteName As String = lSite.Name
                        Dim lArea As Double = lSite.Area
                        Dim lSimTSerInches As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(0))
                        lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                        lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lSimTSer.Attributes.SetValue("YAxis", "Left")
                        lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSer As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lObsTSer.Attributes.SetValue("YAxis", "Left")
                        lObsTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTSer, lArea)
                        lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lPrecTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(5))
                        lPrecTSer.Attributes.SetValue("Units", "inches")

                        lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                             lCons, lSiteName, _
                                                                             "inches", _
                                                                             lSimTSerInches, lObsTSerInches, _
                                                                             lRunMade, _
                                                                             lExpertSystem.SDateJ, _
                                                                             lExpertSystem.EDateJ)
                        Dim lOutFileName As String = "outfiles\MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                     lCons, lSiteName, _
                                                                     "inches", _
                                                                     lPrecTSer, lSimTSerInches, lObsTSerInches, _
                                                                     lRunMade, _
                                                                     lExpertSystem.SDateJ, _
                                                                     lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\Annual" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                           lCons, lSiteName, _
                                                                           lSimTSer, lObsTSer, _
                                                                           lRunMade, _
                                                                           lExpertSystem.SDateJ, _
                                                                           lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        Dim lTimeSeries As New atcCollection
                        lTimeSeries.Add("Observed", lObsTSer)
                        lTimeSeries.Add("Simulated", lSimTSer)
                        lTimeSeries.Add("Precipitation", lPrecTSer)
                        lTimeSeries.Add("LZS", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(9)))
                        lTimeSeries.Add("UZS", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(8)))
                        lTimeSeries.Add("PotET", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(6)))
                        lTimeSeries.Add("ActET", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(7)))
                        lTimeSeries.Add("Baseflow", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(4)))
                        lTimeSeries.Add("Interflow", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(3)))
                        lTimeSeries.Add("Surface", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(2)))
                        GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ, _
                                 lCons, lSiteName, _
                                 lTimeSeries, _
                                 pGraphSaveFormat, pGraphAnnual)
                        lTimeSeries.Dispose()
                    Next

                    lExpertSystem = Nothing
                    If lFileCopied Then
                        IO.File.Delete(pBaseName & ".exs")
                    End If
                Catch lEx As ApplicationException
                    Logger.Dbg(lEx.Message)
                End Try
            Next lExpertSystemFileName
        End If

        For Each lSummaryType As String In pSummaryTypes
            Dim lString As String
            Dim lOutFileName As String
            'build collection of operation types to report
            Dim lOperationTypes As New atcCollection
            lOperationTypes.Add("P:", "PERLND")
            lOperationTypes.Add("I:", "IMPLND")
            lOperationTypes.Add("R:", "RCHRES")

            lString = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lSummaryType, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts).ToString
            lOutFileName = "outfiles\" & pBaseName & "_" & lSummaryType & "_WatershedOverland.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            lString = HspfSupport.ConstituentBudget.Report(lHspfUci, lSummaryType, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade).ToString
            lOutFileName = "outfiles\" & pBaseName & "_" & lSummaryType & "_Budget.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            lString = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lSummaryType).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedSummary.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

            'constituent balance
            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            'watershed constituent balance 
            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    "outfiles\", True)
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    "outfiles\", True, True)
            End If
        Next
        Logger.Dbg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
        'Logger.Msg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
    End Sub
End Module
