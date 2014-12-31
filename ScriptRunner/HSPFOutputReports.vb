Imports System
Imports atcUtility
Imports atcData
Imports atcUCI
Imports atcTimeseriesStatistics
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports MapWindow.Interfaces
Imports System.Collections.Specialized



Module HSPFOutputReports
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
    Private pExpertPrec As Boolean = True
    Private RunOffComponents As Boolean = False
    Private pIdsPerSeg As Integer = 50
    Private pAreaReport As Boolean = False
    Private pRunHSPF As Boolean = False
    Private pHSPFExe As String = "C:\Basins41\models\HSPF\bin\WinHspfLt.exe"
    Private pExcelWrite As Boolean = False

    Private Sub Initialize()
        pOutputLocations.Clear()
        Dim lTestName As String
        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1300
        pGraphSaveHeight = 768

        'Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "NonUpatoi"
        'lTestName = "IRW"
        'lTestName = "IRW-test-agchem"
        'lTestName = "LPCal03"
        'lTestName = "LPVal"
        'lTestName = "CWCal03"
        'lTestName = "CWVal"
        lTestName = "RECal03"
        'lTestName = "REVal"
        'lTestName = "HAT"
        'lTestName = "MAN"
        'lTestName = "mainstm"
        'lTestName = "sbranc"
        'lTestName = "MUST"
        'lTestName = "RAB"
        'lTestName = "Prospect"
        'lTestName = "CRY"
        'lTestName = "WIT"
        'lTestName = "Thief_R"
        'pConstituents.Add("Water")
        'pConstituents.Add("FColi")
        'pConstituents.Add("Sediment")
        'pConstituents.Add("N-PQUAL")
        'pConstituents.Add("P-PQUAL")
        'pConstituents.Add("BOD-PQUAL")
        'pConstituents.Add("TotalN")
        pConstituents.Add("TotalP")
        'pConstituents.Add("AGCHEM")

        Select Case lTestName
            Case "CRY"
                pTestPath = "C:\BASINS\modelout\CRY"
                pBaseName = "CRY"
                pOutputLocations.Add("R:40")
                pOutputLocations.Add("R:72")
                pOutputLocations.Add("R:80")
                pOutputLocations.Add("R:108")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                'pRunHSPF = True
            Case "MAN"
                pTestPath = "C:\BASINS\modelout\MAN"
                pBaseName = "MAN"
                pOutputLocations.Add("R:20")
                pOutputLocations.Add("R:42")
                pOutputLocations.Add("R:60")
                pOutputLocations.Add("R:220")
                pOutputLocations.Add("R:261")
                pOutputLocations.Add("R:281")
                pOutputLocations.Add("R:283")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                'pRunHSPF = True
            Case "Prospect"
                pTestPath = "C:\BASINS\modelout\Prospect"
                pBaseName = "RCH45"
                pOutputLocations.Add("R:45")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                pRunHSPF = True

            Case "RAB"
                pTestPath = "C:\BASINS\modelout\BoisdeSioux\BdSPhase2"
                pBaseName = "RAB"
                pOutputLocations.Add("R:205")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                'pRunHSPF = True
            Case "sbranc"
                pTestPath = "C:\BASINS\modelout\Buffalo\BuffaloPhase2"
                pBaseName = "sbranc"
                pOutputLocations.Add("R:127")
                pOutputLocations.Add("R:89")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                'pRunHSPF = True
            Case "WIT"
                pTestPath = "C:\BASINS\modelout\WIT"
                pBaseName = "WIT"
                pOutputLocations.Add("R:40")
                pOutputLocations.Add("R:60")
                pOutputLocations.Add("R:100")
                pOutputLocations.Add("R:102")
                pOutputLocations.Add("R:104")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                'pRunHSPF = True
            Case "HAT"
                pTestPath = "C:\BASINS\modelout\HAT"
                pBaseName = "HAT"
                pOutputLocations.Add("R:32")
                pOutputLocations.Add("R:32")
                pOutputLocations.Add("R:80")
                pOutputLocations.Add("R:314")
                pOutputLocations.Add("R:140")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True
                RunOffComponents = True
                'pRunHSPF = True

            Case "mainstm"
                pTestPath = "C:\BASINS\modelout\Buffalo\BuffaloPhase2"
                pBaseName = "mainstm"
                pOutputLocations.Add("R:107")
                pOutputLocations.Add("R:104")
                pOutputLocations.Add("R:109")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pAreaReport = True

            Case "Thief_R"
                pTestPath = "C:\BASINS\modelout\Thief\Updated_TRW_HSPF_Model\Updated_TRW_HSPF_Model\TRW_updated"
                pBaseName = "Thief_R"
                pOutputLocations.Add("R:41")
                pOutputLocations.Add("R:44")
                pOutputLocations.Add("R:75")
                pOutputLocations.Add("R:64")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                'pAreaReport = True

            Case "RECal03"
                pTestPath = "C:\Basins\modelout\RECal03"
                pBaseName = "RECal03"
                'pOutputLocations.Add("R:121")
                'pOutputLocations.Add("R:131")
                pOutputLocations.Add("R:133")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                'pRunHSPF = True

            Case "REVal"
                pTestPath = "C:\Basins\modelout\REVal"
                pBaseName = "REVal"
                pOutputLocations.Add("R:121")
                pOutputLocations.Add("R:131")
                pOutputLocations.Add("R:133")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                'pRunHSPF = True

            Case "LPCal03"
                pTestPath = "C:\Basins\modelout\LPCal03"
                pBaseName = "LPCal03"
                pOutputLocations.Add("R:400")
                pOutputLocations.Add("R:347")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                'pRunHSPF = True
                'pAreaReport = True
            Case "LPVal"
                pTestPath = "C:\Basins\modelout\LPVal"
                pBaseName = "LPVal"
                pOutputLocations.Add("R:400")
                pOutputLocations.Add("R:347")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pRunHSPF = True
                'pAreaReport = True


            Case "CWCal03"
                pTestPath = "C:\Basins\modelout\CWCal03"
                pBaseName = "CWCal03"
                pOutputLocations.Add("R:515")
                pOutputLocations.Add("R:557")
                pOutputLocations.Add("R:700")
                pCurveStepType = "NonStep" 'Tony's convention
                'pRunHSPF = True
                pGraphAnnual = True
                'pAreaReport = True

            Case "CWVal"
                pTestPath = "C:\Basins\modelout\CWVal"
                pBaseName = "CWVal"
                pOutputLocations.Add("R:515")
                pOutputLocations.Add("R:557")
                pOutputLocations.Add("R:700")
                pCurveStepType = "NonStep" 'Tony's convention
                pRunHSPF = True
                pGraphAnnual = True
                pAreaReport = True

            Case "IRW"
                pTestPath = "C:\Basins\modelout\IRW"
                pBaseName = "IRW"
                'pOutputLocations.Add("R:150")
                'pOutputLocations.Add("R:523")
                'pOutputLocations.Add("R:630")
                'pOutputLocations.Add("R:640")
                pOutputLocations.Add("R:706")
                'pOutputLocations.Add("R:746")
                'pOutputLocations.Add("R:870")
                'pOutputLocations.Add("R:912")
                'pOutputLocations.Add("R:316")
                pOutputLocations.Add("R:516")
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                'pRunHSPF = True

            Case "IRW-test-agchem"
                pTestPath = "S:\BASINS\data\AGCHEM"
                pBaseName = "IRW-test-agchem"
                pCurveStepType = "NonStep" 'Tony's convention
                pGraphAnnual = True
                pRunHSPF = False

            Case "upatoi"
                'pTestPath = "H:\canopy"
                pTestPath = "H:\EB"
                'pTestPath = "H:\EXTRUN"
                'pTestPath = "H:\FTABLEComp\"
                'pTestPath = "H:\AltB\"
                pBaseName = "Upatoi"
                'pOutputLocations.Add("R:639")
                pOutputLocations.Add("R:614")
                'pOutputLocations.Add("R:626")
                'pOutputLocations.Add("R:30")
                'pOutputLocations.Add("R:33")
                'pOutputLocations.Add("R:34")
                'pOutputLocations.Add("R:36")
                'pOutputLocations.Add("R:662")
                'pOutputLocations.Add("R:45")
                'pOutputLocations.Add("R:46")
                'pOutputLocations.Add("R:74")
                'pOutputLocations.Add("R:2")
                'pGraphAnnual = True
                pCurveStepType = "NonStep" 'Tony's convention 
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101} ', 201, 301, 401, 501, 601, 701, 801, 901, 951}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102} ', 202, 302, 402, 502, 602, 702, 802, 902, 952}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
                pWaterYears = True 'TODO: figure this out from run
                'pRunHSPF = True
            Case "hspf"
                pTestPath = "C:\test\HSPF"
                pBaseName = "test10"
            Case "NonUpatoi"
                'pTestPath = "H:\EXTRUN\"
                'pTestPath = "H:\AltB_BMP\"
                pTestPath = "H:\GHMTA_BMP_95\"
                'pTestPath = "H:\EB"
                pBaseName = "NonUpatoi"
                pOutputLocations.Add("R:202")
                pOutputLocations.Add("R:203")
                pOutputLocations.Add("R:204")
                pOutputLocations.Add("R:205")
                pOutputLocations.Add("R:206")
                pOutputLocations.Add("R:301")
                pOutputLocations.Add("R:302")
                pOutputLocations.Add("R:303")
                pOutputLocations.Add("R:509")
                pOutputLocations.Add("R:402")
                pOutputLocations.Add("R:901")
                pOutputLocations.Add("R:902")
                pGraphAnnual = True
                pAreaReport = True
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101, 201, 301, 401, 501, 601, 701}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102, 202, 302, 402, 502, 602, 702}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
                pCurveStepType = "NonStep" 'Tony's convention 
                pWaterYears = True 'TODO: figure this out from run
                'pRunHSPF = True
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)

        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)

        If pRunHSPF Then
            Dim lUCIName As String = pBaseName & ".uci"
            Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & pTestPath & " for " & lUCIName)
            Logger.Flush()
            LaunchProgram(pHSPFExe, pTestPath, lUCIName)
            Logger.Dbg("HSPFRun Finished")
        End If

        If pGraphWQ Then
            GraphWQDuration()
        End If
        If pGraphWQOnly Then Exit Sub ' early end here if just doing it for WQ graphs

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        Logger.Dbg("ReadUCI " & lHspfUci.Name)

        'open HBN file
        'TODO: need to allow additional binary output files!
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        'For Each lTscheck As atcTimeseries In lHspfBinDataSource.DataSets
        '    If lTscheck.Dates.Value(1) < 1 Then Stop
        'Next

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lRunMade As String = lHspfBinFileInfo.LastWriteTime.ToString
        Dim lDateString As String = Format(Year(lRunMade), "00") & "_" & Format(Month(lRunMade), "00") & _
                "_" & Format(Day(lRunMade), "00") & "_" & Format(Hour(lRunMade), "00") & "_" & Format(Minute(lRunMade), "00")

        'A folder name is given that has the basename and the time when the run was made.
        Dim lOutFolderName As String = pTestPath & "\Reports_" & pBaseName & "_" & lDateString & "\"
        TryDelete(lOutFolderName)
        If Not IO.Directory.Exists(lOutFolderName) Then IO.Directory.CreateDirectory(lOutFolderName)
        'dont change name of uci - make it easier to compare with others, foldername contains info about which run
        System.IO.File.Copy(pBaseName & ".uci", lOutFolderName & pBaseName & ".uci", True)
        'todo: should other output files be copied too?

        'build collection of operation types to report
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")

        Dim lStr As String = ""

        'area report
        If pAreaReport Then
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
                    'Dim lFileCopied As Boolean = False
                    'If IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                    '    lFileCopied = TryCopy(lExpertSystemFileName, pBaseName & ".exs")
                    'End If
                    lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lExpertSystemFileName)
                    lStr = pTestPath & vbCrLf & lExpertSystem.Report
                    SaveFileString(lOutFolderName & "ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)
                    'SaveFileString(lOutFolderName & pBaseName & ".exs", lExpertSystem.AsString)

                    Dim lCons As String = "Flow"
                    For Each lSite As HexSite In lExpertSystem.Sites
                        Dim lSiteName As String = lSite.Name
                        Dim lArea As Double = lSite.Area
                        'Getting the observed flow data
                        Dim lObsTser As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1))
                        lObsTser = SubsetByDate(lObsTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lObsTser.Attributes.SetValue("Units", "Flow (cfs)")
                        lObsTser.Attributes.SetValue("YAxis", "Left")
                        lObsTser.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTser, lArea)
                        lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        'Getting the simulated flow data
                        Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                        lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lSimTSer.Attributes.SetValue("YAxis", "Left")
                        lSimTSer.Attributes.SetValue("StepType", pCurveStepType)

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

                            lStr = pTestPath & vbCrLf & HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                                 lCons, lSiteName, _
                                                                                 "inches", _
                                                                                 lSimTSerInches, lObsTSerInches, _
                                                                                 lRunMade, _
                                                                                 lExpertSystem.SDateJ, _
                                                                                 lExpertSystem.EDateJ)
                            Dim lOutFileName As String = lOutFolderName & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            lStr = pTestPath & vbCrLf & HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                         lCons, lSiteName, _
                                                                         "inches", _
                                                                         lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                         lRunMade, _
                                                                         lExpertSystem.SDateJ, _
                                                                         lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            lStr = pTestPath & vbCrLf & HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                               lCons, lSiteName, _
                                                                               lSimTSer, lObsTser, _
                                                                               lRunMade, _
                                                                               lExpertSystem.SDateJ, _
                                                                               lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            Dim lTimeSeries As New atcTimeseriesGroup
                            lTimeSeries.Add("Observed", lObsTser)
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

                            If RunOffComponents Then
                                Dim IFWO As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(3)), _
                                                                        lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                Dim SURO As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(2)), _
                                                                         lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                Dim AGWO As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(4)), _
                                                                         lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                Dim PERO As atcTimeseries = SURO + IFWO + AGWO
                                Dim SimBaseflow As atcTimeseries = AGWO * lSimTSerInches / PERO
                                Dim SimRunoff As atcTimeseries = (IFWO + SURO) * lSimTSerInches / PERO
                                lTimeSeries.Add("Simulated Baseflow", SimBaseflow)
                                lTimeSeries.Add("Simulated Runoff", SimRunoff)
                                'GraphCumDifBatch(lTimeSeries, lOutFolderName & "RCH_" & lSiteName)
                                lTimeSeries.Clear()
                            End If

                            'If pBaseName = "Upatoi" Then
                            '    lObsTser.Clear()
                            '    lSimTSer.Clear()
                            '    lPrecTser.Clear()
                            '    lObsTser = lWdmDataSource.DataSets.ItemByKey(5)
                            '    lSimTSer = lWdmDataSource.DataSets.ItemByKey(3002)
                            '    lPrecTser = lWdmDataSource.DataSets.ItemByKey(9005)

                            'End If
                            lTimeSeries.Add("Observed", lObsTser)
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
                    
                Catch lEx As ApplicationException
                    Logger.Dbg(lEx.Message)
                End Try
            Next lExpertSystemFileName
        End If
        If pExcelWrite Then
            Dim ExcelFile = System.IO.File.Create(lOutFolderName & "Report.xlsx")

        End If
        Dim lConstituentName As String = ""
        For Each lConstituent As String In pConstituents
            Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")

            Dim lReportCons As New atcReport.ReportText
            Dim lOutFileName As String = ""

            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")
            Logger.Dbg("Summary at " & lLocations.Count & " locations")
            Select Case lConstituent
                Case "Water"
                    lConstituentName = "WAT"
                Case "Sediment"
                    lConstituentName = "SED"
                Case "N-PQUAL"
                    lConstituentName = "N"
                Case "P-PQUAL"
                    lConstituentName = "P"
                Case "TotalN"
                    lConstituentName = "TN"
                Case "TotalP"
                    lConstituentName = "TP"
                Case "BOD-PQUAL"
                    lConstituentName = "BOD"
            End Select

            'lReportCons = HspfSupport.atcHspfAGCHEM.Report(lHspfUci, pBaseName, lHspfBinDataSource, lLocations, lRunMade)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_AGCHEM.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString)


            'If lConstituent <> "Sediment" Then
            '    lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, , , , pWaterYears, pIdsPerSeg)
            '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverland.txt"
            '    SaveFileString(lOutFileName, lReportCons.ToString)
            '    lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, True, pWaterYears, pIdsPerSeg)
            '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShortWithMinMax.txt"
            '    SaveFileString(lOutFileName, lReportCons.ToString)
            '    lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, False, pWaterYears, pIdsPerSeg)
            '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShort.txt"
            '    SaveFileString(lOutFileName, lReportCons.ToString)
            'End If

            'lReportCons = HspfSupport.ConstituentBalance.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade)
            lReportCons = HspfSupport.ConstituentBalance.Report(lHspfUci, "", lOperationTypes, pBaseName, lHspfBinDataSource, Nothing, lRunMade)
            lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_RCH_Ann_Avg_Lds.txt"
            '"All_Budget.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lReportCons = Nothing

            lReportCons = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lConstituent)
            lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_OPN_Ann_Avg_Lds.txt"
            '"_All_WatershedSummary.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)
            lReportCons = Nothing

            'constituent balance
            lReportCons = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade)
            lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_OPN_Ann.txt"
            '"_Mult_ConstituentBalance.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            'lReportCons = HspfSupport.ConstituentBalance.Report _
            '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
            '    lHspfBinDataSource, lLocations, lRunMade, True)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivot.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString)

            'lReportCons = HspfSupport.ConstituentBalance.Report _
            '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
            '    lHspfBinDataSource, lLocations, lRunMade, True, 2, 5, 8)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowTab.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowSpace.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))

            'watershed constituent balance 
            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade)
            lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Grp_By_OPN_LU_Ann_Avg.txt"
            '"_All_WatershedConstituentBalance.txt"
            SaveFileString(lOutFileName, lReportCons.ToString)

            'lReportCons = HspfSupport.WatershedConstituentBalance.Report _
            '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
            '    lHspfBinDataSource, lRunMade, , , , True)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivot.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString)

            'lReportCons = HspfSupport.WatershedConstituentBalance.Report _
            '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
            '    lHspfBinDataSource, lRunMade, , , , True, 2, 5, 8)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivotNarrowTab.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString)
            'lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_WatershedConstituentBalancePivotNarrowSpace.txt"
            'SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))

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
        OpenFile(lOutFolderName)
    End Sub

    Private Sub GraphWQDuration()
        'TODO: make not specifc to Monocacy

        '************  Graphing for WQ duration curve plots ******************
        ' The WQ results for SED, N, and P are in the *.output.wdm files
        ' Location: RCHRES 9
        ' Flow Volume: 911, OVOL, -> ac.ft / hour
        ' Sed Load: 921 - 923, Sand + Silt + Clay,  -> tons/hour
        ' Total N: 931-936, N -> lb/hour
        ' Total P: 941-945, P -> lb/hour
        ' The above information is obtained from the .uci file, which is the same across scenarios
        '
        Dim lWDMNameCollection As New atcCollection
        Dim lOutputWdmDataSource As New atcDataSourceWDM()
        Dim lScenName As String = ""
        'build collection of scenarios (uci base names) to report
        Dim lOutputWDMNames As New System.Collections.Specialized.NameValueCollection

        Dim lp As String = ""
        Dim lf As String = "C:\mono_luChange\output\graphWQlog.txt"
        For Each lBaseFolder As String In pBaseFolders ' loop through all land use scenario folders
            lp = ""
            AddFilesInDir(lOutputWDMNames, lBaseFolder, False, "*.output.wdm")
            'lfld = "C:\mono_luChange\output\lu2030b2"
            'AddFilesInDir(lOutputWDMs, lfld, False, "b_10_gfdl_f30.base.output.wdm")
            For Each lOutputWDMName As String In lOutputWDMNames
                lp = ""
                'If foundQResult(lOWDM) Then ' found a problematic result, then bypass it
                '    Continue For
                'End If
                lOutputWdmDataSource.Open(lOutputWDMName)

                'Flow rate (911, ac.ft/hour -> liter/s)
                Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
                Dim lMathArgs As New atcDataAttributes

                Dim lFlow As atcTimeseries = _
                  lOutputWdmDataSource.DataSets.ItemByKey(911) * 342.633844 '1 ((acre foot) per hour) = 342.633844 liter per second

                '****** SEDIMENT *****

                ' Sediment loading (921, sand; 922, silt; 923, clay in tons per hour)
                Dim lLoadSum As atcTimeseries = _
                   lOutputWdmDataSource.DataSets.ItemByKey(921) + _
                   lOutputWdmDataSource.DataSets.ItemByKey(922) + _
                   lOutputWdmDataSource.DataSets.ItemByKey(923)
                'Double checking: sum Annual
                'Logger.Msg("Sum Annual Sediment: " & lLoadSum.Attributes.GetFormattedValue("Sum Annual"))
                '235470 vs 235430    19619.1667 (monthly mean)
                Dim lLoadSumNewUnit As atcTimeseries = lLoadSum * 0.251995761 '1 (ton per hour) = 0.251995761 kilogram per second

                ' Sediment concentration
                Dim lConc As atcTimeseries = lLoadSumNewUnit / lFlow ' Intermediate - kg/l
                Dim lConcNewUnit As atcTimeseries = lConc * (1000 * 1000) 'kg/l to mg/L

                'Rid of Infinitys from TSers
                For i As Integer = 0 To lFlow.numValues - 1
                    If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                    If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                    If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                Next

                Dim lDataGroup As New atcTimeseriesGroup
                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSumNewUnit.Attributes.SetValue("Constituent", "LDC (Sediment)")
                lDataGroup.Add(lLoadSumNewUnit)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Sediment)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph
                Dim lscen As String = IO.Path.GetFileNameWithoutExtension(lOutputWDMName)
                Dim lGraphFilename As String = IO.Path.Combine(lBaseFolder, lscen) & "_dur_sed" & pGraphSaveFormat
                Dim lZgc As ZedGraphControl

                lZgc = CreateZgc()
                Dim lGraphSaveWidth As Integer = 1000
                Dim lGraphSaveHeight As Integer = 600
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane

                    If lscen.StartsWith("base.") Then
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Sediment : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : base"
                    Else
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Sediment : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    End If
                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec"
                        '.Scale.MinAuto = True
                        '.Scale.MaxAuto = True

                        'If .Scale.Min < 1 Then
                        '    .Scale.Min = 0.000001
                        'End If
                        '.Scale.Max = 100000
                        .IsVisible = True
                    End With
                    If .YAxis.Scale.Min < 1 Then
                        .YAxis.Scale.MinAuto = False
                        .YAxis.Scale.Min = 1
                        .YAxis.Scale.Max = 1000000
                        .YAxis.MajorGrid.IsVisible = False
                        .YAxis.MinorGrid.IsVisible = False
                        .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                        .AxisChange()
                    End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Sediment)").Color = Drawing.Color.Brown
                    .CurveList("CDC (Sediment)").Color = Drawing.Color.BurlyWood
                    .CurveList("LDC (Sediment)").IsY2Axis = True
                    .CurveList("CDC (Sediment)").IsY2Axis = True
                End With

                Try
                    lZgc.SaveIn(lGraphFilename)
                Catch ex As Exception
                    lp = "P:Sediment:"
                    'Stop
                End Try
                lGraphDur.Dispose()
                lZgc.Dispose()

                '******* NITROGEN *******
                '******* NITROGEN Begins *******
                '******* NITROGEN *******
                ' Nitrogen loading
                'Get Tot-N
                ' Need 931 - NUCF9, NO3D
                ' Need 932 - NUCF9, NH3D
                ' Need 933 - OSNH4, NH3A
                ' Need 934 - OSNH4, NH3I
                ' Need 935 - OSNH4, NH3C
                ' Need 936 - PKCF2, RORN
                ' Need OXCF2 - 951 - BODA; multiplier: 0.043580459 (i.e. (14*16*49)/(1200*106*1.98))
                ' Need PKCF2 - 953 - PHYT; multiplier: 0.086289308 (i.e. (14*16*49)/(1200*106))
                lLoadSum.Clear()
                'Exit Sub

                Try
                    lLoadSum = lOutputWdmDataSource.DataSets.ItemByKey(931) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(932) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(933) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(933) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(934) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(935) + _
                    lOutputWdmDataSource.DataSets.ItemByKey(936) + _
                    (lOutputWdmDataSource.DataSets.ItemByKey(951) * 0.043580459) + _
                    (lOutputWdmDataSource.DataSets.ItemByKey(953) * 0.086289308)
                Catch ex As Exception
                    Logger.Msg("Adding Nitrogen load problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                lLoadSumNewUnit.Clear()
                Try
                    lLoadSumNewUnit = lLoadSum * 0.000125997881 ' coversion: lb/h -> kg/s,  1 (pound per hour) = 0.000125997881 kilogram per second
                Catch ex As Exception
                    Logger.Msg("Change Nitrogen load unit problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                lConc.Clear()
                Try
                    lConc = lLoadSum / lFlow ' here unit is: number of kg / L /s
                Catch ex As Exception
                    Logger.Msg("Calc Nitrogen concentration problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                'Change Nitrogen concentration to mg/L by multiplying 1000 * 1000
                lConcNewUnit.Clear()
                Try
                    lConcNewUnit = lConc * (1000 * 1000)
                Catch ex As Exception
                    Logger.Msg("Calc final nitrogen concentration problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                'Rid of Infinitys from TSers
                For i As Integer = 0 To lFlow.numValues - 1
                    If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                    If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                    If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                Next

                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSum.Attributes.SetValue("Constituent", "LDC (Nitrogen)")
                lDataGroup.Add(lLoadSum)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Nitrogen)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph for nitrogen
                'lscen = IO.Path.GetFileNameWithoutExtension(lOWDM) ' use from above
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & "_dur_nitro" & pGraphSaveFormat

                lZgc = CreateZgc()
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                lGraphDur = New clsGraphProbability(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane
                    If lscen.StartsWith("base.") Then
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Nitrogen : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : base"
                    Else
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Nitrogen : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    End If

                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False

                        '.Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec"
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) lb/h"
                        '.Scale.MinAuto = True
                        '.Scale.MaxAuto = True

                        'If .Scale.Min < 1 Then
                        '    .Scale.Min = 0.000001
                        'End If
                        '.Scale.Max = 100000
                        .IsVisible = True
                    End With
                    If .YAxis.Scale.Min < 1 Then
                        .YAxis.Scale.MinAuto = False
                        .YAxis.Scale.Min = 1
                        '.YAxis.Scale.Max = 1000000
                        .YAxis.MajorGrid.IsVisible = False
                        .YAxis.MinorGrid.IsVisible = False
                        .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                        .AxisChange()
                    End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Nitrogen)").Color = Drawing.Color.DarkKhaki
                    .CurveList("CDC (Nitrogen)").Color = Drawing.ColorTranslator.FromHtml("#ff0099")
                    .CurveList("LDC (Nitrogen)").IsY2Axis = True
                    .CurveList("CDC (Nitrogen)").IsY2Axis = True
                End With

                Try
                    lZgc.SaveIn(lGraphFilename)
                Catch ex As Exception
                    lp &= "P:Nitrogen:"
                    'Stop
                End Try
                lGraphDur.Dispose()
                lZgc.Dispose()

                '******* PO4 *******
                '******* PO4 Begins *******
                '******* PO4 *******
                ' Phosphorus loading
                'Get Tot-P
                ' Need 941 - 'NUCF9, PO4D
                ' Need 942 - 'OSPO4, PO4A
                ' Need 943 - 'OSPO4, PO4I
                ' Need 944 - 'OSPO4, PO4C
                ' Need 945 - 'PKCF2, RORP
                ' Need OXCF2 - 951 - BODA; multiplier: 0.006031224 (i.e. (31*49)/(1200*106)/1.98)
                ' Need PKCF2 - 953 - PHYT; multiplier: 0.011941824 (i.e. (31*49)/(1200*106))
                lLoadSum.Clear()
                Dim lPO4D As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(941)
                Dim lPO4A As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(942)
                Dim lPO4I As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(943)
                Dim lPO4C As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(944)
                Dim lRORP As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(945)
                Dim lBODA As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(951) * 0.006031224
                Dim lPHYT As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(953) * 0.011941824
                Try
                    lLoadSum = lPO4D
                    lLoadSum = lLoadSum + lPO4A
                    lLoadSum = lLoadSum + lPO4I
                    lLoadSum = lLoadSum + lPO4C
                    lLoadSum = lLoadSum + lRORP
                    lLoadSum = lLoadSum + lBODA
                    lLoadSum = lLoadSum + lPHYT
                Catch ex As Exception
                    Logger.Msg("Adding Phosphorus problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                lLoadSumNewUnit.Clear()
                Try
                    lLoadSumNewUnit = lLoadSum * 0.000125997881 ' coversion: lb/h -> kg/s,  1 (pound per hour) = 0.000125997881 kilogram per second
                Catch ex As Exception
                    Logger.Msg("Changen unit for Phosphorus load problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                lConc.Clear()
                Try
                    lConc = lLoadSumNewUnit / lFlow ' here unit is: number of kg / L
                Catch ex As Exception
                    Logger.Msg("Calc Phosphorus concentration problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                lConcNewUnit.Clear()
                Try
                    lConcNewUnit = lConc * (1000 * 1000) ' convert unit to mg /L
                Catch ex As Exception
                    Logger.Msg("Change Phosphorus concentration unit problem for: " & lOutputWDMName)
                    Exit Sub
                End Try

                'Rid of Infinitys from TSers
                For i As Integer = 0 To lFlow.numValues - 1
                    If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                    If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                    If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                Next

                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSum.Attributes.SetValue("Constituent", "LDC (Phosphorus)") ' graph original as converted units are not working for phos
                lDataGroup.Add(lLoadSum)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Phosphorus)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph for nitrogen
                'lscen = IO.Path.GetFileNameWithoutExtension(lOWDM) ' use from above
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & "_dur_phos" & pGraphSaveFormat

                lZgc = CreateZgc()
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                lGraphDur = New clsGraphProbability(lDataGroup, lZgc)
                'Dim lGraphDuro As New clsGraphTime(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane
                    If lscen.StartsWith("base.") Then
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Phosphorus : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : base"
                    Else
                        .XAxis.Title.Text = "Normal Percentile (% greater than): Phosphorus : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    End If

                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False
                        '.Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec"
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) lb/h"
                        '.Scale.MinAuto = True
                        '.Scale.MaxAuto = True
                        'If .Scale.Min < 1 Then
                        '    .Scale.Min = 0.000001
                        'End If
                        '.Scale.Max = 100000
                        .IsVisible = True
                    End With
                    'If .YAxis.Scale.Min < 1 Then
                    .YAxis.Scale.MinAuto = False
                    .YAxis.Scale.Min = 0.0000001
                    '    .YAxis.Scale.Min = 1
                    '    '.YAxis.Scale.Max = 1000000
                    '    .YAxis.MajorGrid.IsVisible = False
                    '    .YAxis.MinorGrid.IsVisible = False
                    '    .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                    '    .AxisChange()
                    'End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Phosphorus)").Color = Drawing.ColorTranslator.FromHtml("#ff3366")
                    .CurveList("CDC (Phosphorus)").Color = Drawing.ColorTranslator.FromHtml("#cc9933")
                    .CurveList("LDC (Phosphorus)").IsY2Axis = True
                    .CurveList("CDC (Phosphorus)").IsY2Axis = True
                End With

                Try
                    lZgc.SaveIn(lGraphFilename)
                Catch ex As Exception
                    lp &= "P:Phos:"
                    'Stop
                End Try
                lGraphDur.Dispose()
                lZgc.Dispose()

                If lp.StartsWith("P:") Then
                    IO.File.AppendAllText(lf, lp & lGraphFilename & vbCrLf)
                End If

                If lFlow IsNot Nothing Then lFlow.Clear()
                If lLoadSum IsNot Nothing Then lLoadSum.Clear()
                If lLoadSumNewUnit IsNot Nothing Then lLoadSumNewUnit.Clear()
                If lConc IsNot Nothing Then lConc.Clear()
                If lConcNewUnit IsNot Nothing Then lConcNewUnit.Clear()
                If lDataGroup IsNot Nothing Then lDataGroup.Clear()
                If lOutputWDMName = lOutputWDMNames.Item(lOutputWDMNames.Keys.Count - 1).ToString Then
                    Logger.Msg("Done processing last output WDM in this folder")
                End If
            Next ' lOWDM in lOutputWDMS within a given scen lfld
            If lOutputWDMNames IsNot Nothing Then lOutputWDMNames.Clear()
        Next ' lfld in pBaseFolders
    End Sub

    Private Function foundQResult(ByVal aScen As String) As Boolean
        Dim lqList As New ArrayList
        lqList.Add("a_10_gfdl_f30.base.output.wdm")
        lqList.Add("b_10_gfdl_f30.base.output.wdm")
        lqList.Add("a_70_gfdl_f30.base.output.wdm")
        lqList.Add("b_70_gfdl_f30.base.output.wdm")
        lqList.Add("C:\mono_luChange\output\lu2090a2\a_70_cccm_f10.base.output.wdm")
        lqList.Add("C:\mono_luChange\output\lu2090a2\a_70_ccsr_m.base.output.wdm")
        lqList.Add("C:\mono_luChange\output\lu2090a2\a_70_csir_m.base.output.wdm")
        lqList.Add("C:\mono_luChange\output\lu2090a2\a_70_gfdl_f10.base.output")


        foundQResult = False
        For Each ls As String In lqList
            If aScen.Contains(ls) Then
                foundQResult = True
                Exit For
            End If
        Next
    End Function
End Module
