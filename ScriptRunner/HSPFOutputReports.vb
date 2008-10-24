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

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"

        'Dim lTestName As String = "tinley"
        'Dim lTestName As String = "hspf"
        'Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "shena"
        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "housatonic"
        'Dim lTestName As String = "beaver"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        'Dim lTestName As String = "SantaClara"
        'pSummaryTypes.Add("Water")
        pSummaryTypes.Add("Sediment")

        Select Case lTestName
            Case "housatonic"
                pTestPath = "d:\projects\housatonic\jackBin"
                pBaseName = "base"
                pOutputLocations.Add("R:110")
                pOutputLocations.Add("R:400")
                pOutputLocations.Add("R:820")
                pOutputLocations.Add("R:540")
                pOutputLocations.Add("R:600")
                pOutputLocations.Add("R:900")
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

        If pSummaryTypes.Contains("Water") Then
            'open WDM file
            Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)

            Dim lExpertSystemFileNames As New NameValueCollection
            AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
            Dim lExpertSystem As HspfSupport.ExpertSystem
            For Each lExpertSystemFileName As String In lExpertSystemFileNames
                Try
                    Dim lFileCopied As Boolean = False
                    If IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                        lFileCopied = TryCopy(lExpertSystemFileName, pBaseName & ".exs")
                    End If
                    lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
                    Dim lStr As String = lExpertSystem.Report
                    SaveFileString("outfiles\ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)

                    'lStr = lExpertSystem.AsString 'NOTE:just testing
                    'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                    Dim lCons As String = "Flow"
                    For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                        Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
                        Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
                        Dim lSimTSerInches As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(0))
                        lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                        lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lSimTSer.Attributes.SetValue("YAxis", "Left")
                        lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSer As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lObsTSer.Attributes.SetValue("YAxis", "Left")
                        lObsTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTSer, lArea)
                        lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lPrecTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(5))
                        lPrecTSer.Attributes.SetValue("Units", "inches")

                        lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                             lCons, lSite, _
                                                                             "inches", _
                                                                             lSimTSerInches, lObsTSerInches, _
                                                                             lExpertSystem.SDateJ, _
                                                                             lExpertSystem.EDateJ)
                        Dim lOutFileName As String = "outfiles\MonthlyAverage" & lCons & "Stats" & "-" & lSite & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                     lCons, lSite, _
                                                                     "inches", _
                                                                     lPrecTSer, lSimTSerInches, lObsTSerInches, _
                                                                     lExpertSystem.SDateJ, _
                                                                     lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\Annual" & lCons & "Stats" & "-" & lSite & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                           lCons, lSite, _
                                                                           lSimTSer, lObsTSer, _
                                                                           lExpertSystem.SDateJ, _
                                                                           lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        Dim lTimeSeries As New atcCollection
                        lTimeSeries.Add("Observed", lObsTSer)
                        lTimeSeries.Add("Simulated", lSimTSer)
                        lTimeSeries.Add("Precipitation", lPrecTSer)
                        lTimeSeries.Add("LZS", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(9)))
                        lTimeSeries.Add("UZS", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(8)))
                        lTimeSeries.Add("PotET", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(6)))
                        lTimeSeries.Add("ActET", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(7)))
                        lTimeSeries.Add("Baseflow", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(4)))
                        lTimeSeries.Add("Interflow", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(3)))
                        lTimeSeries.Add("Surface", lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(2)))
                        GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ, _
                                 lCons, lSite, _
                                 lTimeSeries, _
                                 pGraphSaveFormat, pGraphAnnual)
                        lTimeSeries.Dispose()
                    Next lSiteIndex

                    lExpertSystem = Nothing
                    If lFileCopied Then
                        IO.File.Delete(pBaseName & ".exs")
                    End If
                Catch lEx As ApplicationException
                    Logger.Dbg(lEx.Message)
                End Try
            Next lExpertSystemFileName
        End If

        'open HBN file
        'TODO: need to allow additional binary output files!
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)

        For Each lSummaryType As String In pSummaryTypes
            Dim lString As String
            Dim lOutFileName As String
            'build collection of operation types to report
            Dim lOperationTypes As New atcCollection
            lOperationTypes.Add("P:", "PERLND")
            lOperationTypes.Add("I:", "IMPLND")
            lOperationTypes.Add("R:", "RCHRES")

            lString = HspfSupport.ConstituentBudget.Report(lHspfUci, lSummaryType, lOperationTypes, pBaseName, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime).ToString
            lOutFileName = "outfiles\" & pBaseName & "_" & lSummaryType & "_" & "Budget.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            lString = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

            'constituent balance
            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime, True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime, True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            'watershed constituent balance 
            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, , , , True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, , , , True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lHspfBinFileInfo.LastWriteTime, _
                    "outfiles\", True)
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lHspfBinFileInfo.LastWriteTime, _
                    "outfiles\", True, True)
            End If
        Next
        Logger.Dbg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
        'Logger.Msg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
    End Sub
End Module
