Imports System
Imports atcUtility
Imports atcData
Imports atcTimeseriesStatistics
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports atcUCI

Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized
Module Util_HydroFrack_Report

    Private pBaseFolders As New ArrayList
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = False
    Private pCurveStepType As String = "RearwardStep"
    Private pConstituents As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer
    Private pGraphWQOnly As Boolean = False
    Private pGraphWQ As Boolean = False
    Private pWaterYears As Boolean = False
    Private pExpertPrec As Boolean = False
    Private pIdsPerSeg As Integer = 50

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768

        Dim lTestName As String = "Susq020501"
        'lTestName = "Susq020502"
        'lTestName = "Susq020503"

        pConstituents.Add("Water")
        pTestPath = "G:\Admin\GCRPSusq"

        Select Case lTestName
            Case "Susq020501"
                pBaseName = "Susq020501"
                pOutputLocations.Add("R:69")
                'pOutputLocations.Add("R:110")
                pExpertPrec = True
                'pWaterYears = True 'TODO: figure this out from run
                pIdsPerSeg = 25

                'Add scenario directories
                'pBaseFolders.Clear()
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2bmp")

                'pCurveStepType = "NonStep"
                'pConstituents.Add("Sediment")

                'pGraphAnnual = True
                'pCurveStepType = "NonStep" 'Tony's convention 
                'Dim pUpatoiPerlndSegmentStarts() As Integer = {101} ', 201, 301, 401, 501, 601, 701, 801, 901, 951}
                'pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                'Dim pUpatoiImplndSegmentStarts() As Integer = {102} ', 202, 302, 402, 502, 602, 702, 802, 902, 952}
                'pImplndSegmentStarts = pUpatoiImplndSegmentStarts
            Case "Susq020502"
                pBaseName = "Susq020502"
                pOutputLocations.Add("R:43")
                pExpertPrec = True
                pIdsPerSeg = 25
            Case "Susq020503"
                pBaseName = "Susq020503"
                pOutputLocations.Add("R:86")
                pExpertPrec = True
                pIdsPerSeg = 25
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        Logger.Dbg("ReadUCI " & lHspfUci.Name)
        If pOutputLocations.Contains("Lynnwood") Then 'special case to check GenScn examples
            With lHspfUci.GlobalBlock
                .SDate(0) = 1986
                .SDate(1) = 10
                .SDate(2) = 1
                .EDate(0) = 1987
                .EDate(1) = 10
                .EDate(2) = 1
            End With
        ElseIf pTestPath.Contains("mono") Then
            'CBP - adjust description to remove segment id
            Dim lOperationsNames() As String = {"PERLND", "IMPLND"}
            For Each lOperationName As String In lOperationsNames
                Dim lOperations As HspfOperations = lHspfUci.OpnBlks(lOperationName).Ids
                For Each lOperation As HspfOperation In lOperations
                    With lOperation
                        .Description = .Description.Substring(.Description.LastIndexOf(" ") + 1)
                    End With
                Next
            Next
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
        Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") & _
                Format(Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")

        'A folder name is given that has the basename and the time when the run was made.
        Dim lOutFolderName As String = "Reports_" & pBaseName & "_" & lDateString & "\"
        TryDelete(lOutFolderName)
        IO.Directory.CreateDirectory(lOutFolderName)
        'dont change name of uci - make it easier to compare with others, foldername contains info about which run
        System.IO.File.Copy(pBaseName & ".uci", lOutFolderName & pBaseName & ".uci")
        'todo: should other output files be copied too?

        'build collection of operation types to report
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")

        Dim lStr As String = ""
        Dim lBalanceBasinOnly As Boolean = True

        If Not lBalanceBasinOnly Then
            'area report
            Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(lHspfUci, lRunMade, lOperationTypes, pOutputLocations, True, lOutFolderName & "\")
            lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)
            SaveFileString(lOutFolderName & "AreaReport.txt", lReport.ToString)
        End If

        If pConstituents.Contains("Water") Then
            'open WDM file
            Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)
            'TODO: allow observed flow to come from a different fileEXPE

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
                    lStr = lExpertSystem.Report
                    SaveFileString(lOutFolderName & "ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)
                    SaveFileString(lOutFolderName & pBaseName & ".exs", lExpertSystem.AsString)

                    'lStr = lExpertSystem.AsString 'NOTE:just testing
                    'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                    Dim lCons As String = "Flow"
                    For Each lSite As HexSite In lExpertSystem.Sites
                        Dim lSiteName As String = lSite.Name
                        Dim lArea As Double = lSite.Area
                        Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(0)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
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

                        Dim lRchId As Integer
                        If lSite.Name.StartsWith("RCH") Then
                            lRchId = lSiteName.Substring(3)
                        Else
                            lRchId = lSite.Name
                        End If
                        Dim lOperation As atcUCI.HspfOperation = lHspfUci.OpnBlks("RCHRES").OperFromID(lRchId)
                        If lOperation Is Nothing Then
                            Logger.Dbg("MissingOperationInUCI for " & lRchId)
                        Else
                            Dim lAreaOriginal As Double
                            Dim lPrecSourceCollection As New atcCollection
                            Dim lAreaFromWeight As Double = lHspfUci.WeightedSourceArea(lOperation, "PREC", lPrecSourceCollection, lAreaOriginal)
                            Logger.Dbg("AreaFromWeight " & lAreaFromWeight & " AreaOriginal " & lAreaOriginal)
                            If (lAreaFromWeight - lAreaOriginal) > 1 Then
                                Logger.Dbg("**** AREA PROBLEM ****")
                            End If

                            Dim lPrecTser As atcTimeseries = Nothing
                            If pExpertPrec Then
                                Dim lPrecDsn As Integer = lSite.DSN(5)
                                lPrecTser = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lPrecDsn), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            Else
                                Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
                                Dim lMathArgs As New atcDataAttributes
                                For lSourceIndex As Integer = 0 To lPrecSourceCollection.Count - 1
                                    Dim lPrecDataGroup As atcTimeseriesGroup = lWdmDataSource.DataSets.FindData("ID", lPrecSourceCollection.Keys(lSourceIndex))
                                    If lPrecDataGroup.Count = 0 Then
                                        Dim lPrecWdmDataSource As New atcDataSourceWDM()
                                        lPrecWdmDataSource.Open(pTestPath & "\FBMet.wdm")
                                        lPrecDataGroup = lPrecWdmDataSource.DataSets.FindData("ID", lPrecSourceCollection.Keys(lSourceIndex))
                                        Logger.Dbg("PrecDataGroupFrom FBMet.wdm " & lPrecDataGroup.Count)
                                    Else
                                        Logger.Dbg("PrecDataGroupFrom " & lWdmFileName & " " & lPrecDataGroup.Count)
                                    End If

                                    Dim lPrecSubsetDataGroup As New atcTimeseriesGroup
                                    For lIndex As Integer = 0 To lPrecDataGroup.Count - 1
                                        lPrecSubsetDataGroup.Add(SubsetByDate(lPrecDataGroup(lIndex), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing))
                                    Next

                                    lMathArgs.SetValue("Timeseries", lPrecSubsetDataGroup)
                                    Dim lPrecMultiply As Double = lPrecSourceCollection.Item(lSourceIndex)
                                    lMathArgs.SetValue("Number", lPrecMultiply)
                                    If lMath.Open("Multiply", lMathArgs) Then
                                        Logger.Dbg("SourceIndex " & lSourceIndex & _
                                                   " DSN " & lPrecSubsetDataGroup.Item(0).Attributes.GetDefinedValue("ID").Value & _
                                                   " MultBy " & lPrecMultiply)
                                        If lSourceIndex = 0 Then
                                            lPrecTser = lMath.DataSets(0).Clone
                                        Else
                                            Dim lMathAdd As New atcTimeseriesMath.atcTimeseriesMath
                                            Dim lMathAddArgs As New atcDataAttributes
                                            Dim lDataGroup As New atcTimeseriesGroup
                                            lDataGroup.Add(lPrecTser)
                                            lDataGroup.Add(lMath.DataSets(0))
                                            lMathAddArgs.SetValue("Timeseries", lDataGroup)
                                            If lMathAdd.Open("Add", lMathAddArgs) Then
                                                lPrecTser = lMathAdd.DataSets(0).Clone
                                                Logger.Dbg(" AfterAdd " & lPrecTser.Attributes.GetValue("SumAnnual"))
                                            Else
                                                Logger.Dbg("ProblemWithAdd")
                                            End If
                                        End If
                                    Else
                                        Logger.Dbg("ProblemWithMultiply ")
                                    End If
                                    lMath.Clear()
                                    lMathArgs.Clear()
                                Next
                                lMathArgs.SetValue("Timeseries", lPrecTser)
                                If Math.Abs(lSite.Area - lAreaOriginal) < 0.01 Then
                                    Logger.Dbg("AreaDiscrepancy " & lSite.Area & " " & lAreaOriginal)
                                End If
                                lMathArgs.SetValue("Number", lAreaOriginal)
                                If Not lMath.Open("Divide", lMathArgs) Then
                                    Logger.Dbg("ProblemWithDivide")
                                End If
                                lPrecTser = lMath.DataSets(0)
                                Logger.Dbg(" AfterDivide " & lPrecTser.Attributes.GetValue("SumAnnual"))
                                lPrecTser.Attributes.SetValue("Location", "Weighted Average")
                            End If
                            lPrecTser.Attributes.SetValue("Units", "inches")

                            lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                                 lCons, lSiteName, _
                                                                                 "inches", _
                                                                                 lSimTSerInches, lObsTSerInches, _
                                                                                 lRunMade, _
                                                                                 lExpertSystem.SDateJ, _
                                                                                 lExpertSystem.EDateJ)
                            Dim lOutFileName As String = lOutFolderName & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                         lCons, lSiteName, _
                                                                         "inches", _
                                                                         lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                         lRunMade, _
                                                                         lExpertSystem.SDateJ, _
                                                                         lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                               lCons, lSiteName, _
                                                                               lSimTSer, lObsTSer, _
                                                                               lRunMade, _
                                                                               lExpertSystem.SDateJ, _
                                                                               lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            Dim lTimeSeries As New atcTimeseriesGroup
                            lTimeSeries.Add("Observed", lObsTSer)
                            lTimeSeries.Add("Simulated", lSimTSer)
                            lTimeSeries.Add("Precipitation", lPrecTser)
                            lTimeSeries.Add("LZS", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(9)))
                            lTimeSeries.Add("UZS", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(8)))
                            lTimeSeries.Add("PotET", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(6)))
                            lTimeSeries.Add("ActET", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(7)))
                            lTimeSeries.Add("Baseflow", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(4)))
                            lTimeSeries.Add("Interflow", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(3)))
                            lTimeSeries.Add("Surface", lWdmDataSource.DataSets.ItemByKey(lSite.DSN(2)))
                            GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ, _
                                     lCons, lSiteName, _
                                     lTimeSeries, _
                                     pGraphSaveFormat, _
                                     pGraphSaveWidth, _
                                     pGraphSaveHeight, _
                                     pGraphAnnual, lOutFolderName)
                            lTimeSeries.Clear()

                            lTimeSeries.Add("Observed", lObsTSer)
                            lTimeSeries.Add("Simulated", lSimTSer)
                            lTimeSeries.Add("Prec", lPrecTser)

                            lTimeSeries(0).Attributes.SetValue("Units", "cfs")
                            lTimeSeries(0).Attributes.SetValue("StepType", pCurveStepType)
                            lTimeSeries(1).Attributes.SetValue("Units", "cfs")
                            lTimeSeries(1).Attributes.SetValue("StepType", pCurveStepType)
                            GraphStorms(lTimeSeries, 2, lOutFolderName & "Storm", pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem)
                            lTimeSeries.Dispose()
                        End If
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

        For Each lConstituent As String In pConstituents
            Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")
            If lBalanceBasinOnly Then GoTo BalanceBasin
            Dim lReportCons As New atcReport.ReportText
            Dim lOutFileName As String = ""
            If lConstituent <> "Sediment" Then
                HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, , , , pWaterYears, pIdsPerSeg).ToString()
                lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverland.txt"
                SaveFileString(lOutFileName, lReportCons.ToString)
                lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, True, pWaterYears, pIdsPerSeg)
                lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShortWithMinMax.txt"
                SaveFileString(lOutFileName, lReportCons.ToString)
                lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, False, pWaterYears, pIdsPerSeg)
                lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShort.txt"
                SaveFileString(lOutFileName, lReportCons.ToString)
            End If

            lReportCons = Nothing
            lReportCons = HspfSupport.ConstituentBudget.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_Budget.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lReportCons = Nothing

            lReportCons = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lConstituent)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedSummary.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lReportCons = Nothing

            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")
            Logger.Dbg("Summary at " & lLocations.Count & " locations")
            'constituent balance
            lReportCons = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalance.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            lReportCons = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            lReportCons = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True, 2, 5, 8)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))

            'watershed constituent balance 
            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalance.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True, 2, 5, 8)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_WatershedConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))
BalanceBasin:
            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    lOutFolderName, True)
                'now pivoted version
                'HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                '    lHspfBinDataSource, pOutputLocations, lRunMade, _
                '    lOutFolderName, True, True)
            End If
        Next
        Logger.Dbg("Reports Written in " & lOutFolderName, "HSPFOutputReports")
    End Sub

End Module
