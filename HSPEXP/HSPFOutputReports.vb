Imports atcUtility
Imports atcData
Imports atcGraph
Imports HspfSupport
Imports atcUCI
Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports System.Collections.Specialized
Imports System.IO 'added by Becky to get directory exists function

Module HSPFOutputReports
    Private pTestPath As String
    Private pBaseName As String 'this is the base part of the file name (i.e., without .uci, .wdm, .exs) - it MUST be used to name everything
    Private pOutputLocations As New atcCollection

    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = True
    Private pCurveStepType As String = "NonStep"
    Private pConstituents As New atcCollection
    Private pWaterYears As Boolean = False

    Private pMakeAreaReports As Boolean 'flag to indicate user wants subwatershed & land use reports created
    Friend pHSPFExe As String '= FindFile("Please locate WinHspfLt.exe", IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location), "WinHSPFLt", "WinHspfLt.exe"))
    Private pRunUci As Boolean = False 'Anurag added this option if the user wants this program to run the uci as well
    Private SDateJ, EDateJ As Double
    Private loutfoldername As String
    Private MultiSimulation As Boolean = False
    Private pListModelParameters As Boolean = False
    Private ASDate, AEDate As Date

    Private Sub Initialize()
        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            Dim pStatusMonitor As New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"),
                                            IO.Directory.GetCurrentDirectory,
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                'put our status monitor (StatusMonitor.exe) between the Logger and the default MW status monitor
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE HSPEXP+")
                Logger.Status("PROGRESS TIME OFF") 'Disable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If

        Logger.Status("HSPEXP+ started", True)
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        pGraphSaveWidth = 1300
        pGraphSaveHeight = 768

        pRunUci = StartUp.chkRunHSPF.Checked
        pMakeAreaReports = StartUp.chkAreaReports.Checked

        Dim lTestName As String = IO.Path.GetFileNameWithoutExtension(StartUp.cmbUCIPath.Text)
        Logger.Status("Beginning analysis of " & lTestName, True)

        If StartUp.chkMultiSim.Checked Then
            MultiSimulation = True
        End If
        pConstituents.Clear()
        If StartUp.chkWaterBalance.Checked Then
            pConstituents.Add("Water")
        End If
        If StartUp.chkSedimentBalance.Checked Then
            pConstituents.Add("Sediment")
        End If

        If StartUp.chkTotalNitrogen.Checked Then
            pConstituents.Add("TotalN")
        End If

        If StartUp.chkTotalPhosphorus.Checked Then
            pConstituents.Add("TotalP")
        End If
        If StartUp.chkBODBalance.Checked Then
            pConstituents.Add("BOD-Labile")
        End If

        If StartUp.chkDO.Checked Then
            pConstituents.Add("DO")
        End If

        If StartUp.chkHeat.Checked Then
            pConstituents.Add("Heat")
        End If


        If StartUp.chkFecalColiform.Checked Then
            pConstituents.Add("FColi")
        End If
        'set up the timeseries attributes for statistics
        atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared()

        pTestPath = StartUp.cmbUCIPath.Text
        pBaseName = lTestName
        pTestPath = Mid(pTestPath, 1, Len(pTestPath) - Len(pBaseName) - 4)

        For Each lRCH As String In StartUp.txtRCH.Text.Split(","c)
            If IsNumeric(lRCH) Then
                pOutputLocations.Add("R:" & CInt(lRCH)) ' the Cint should get rid of leading spaces and zeros 
            End If
        Next
        StartUp.Hide()
        Logger.StartToFile(pTestPath & "LogFile.txt", , False)
        Logger.Status("Run characteristics read", True)
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As Object, ByVal aHspfUci As atcUCI.HspfUci)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        Logger.Status("HSPEXP+ is running.")
        Try
            Using lProgress As New ProgressLevel
                SDateJ = StartUp.DateTimePicker1.Value.ToOADate()
                EDateJ = StartUp.DateTimePicker2.Value.ToOADate() + 1

                Dim lEchoFileisinFilesBlock As Boolean = False
                Dim lHspfEchoFileName As String = pTestPath & "hspfecho.out" 'Get the default name of echo file
                Dim echoFileInfo As System.IO.FileInfo
                For i As Integer = 0 To aHspfUci.FilesBlock.Count
                    If aHspfUci.FilesBlock.Value(i).Typ = "MESSU" Then
                        lHspfEchoFileName = AbsolutePath(aHspfUci.FilesBlock.Value(i).Name.Trim, CurDir()) 'Update echo file name if it is referenced in the Files block
                        Exit For
                    End If
                Next

                If MultiSimulation Then
                    SubMultiSim(pHSPFExe, pBaseName, pTestPath, SDateJ, EDateJ, lHspfEchoFileName)
                    Logger.Msg("Multi Simulation Manager Process Complete", vbOKOnly)
                    OpenFile(pTestPath)
                    End

                End If

                pListModelParameters = False
                If pListModelParameters Then
                    ListReachParametersForAllUCIFiles(pTestPath)
                End If

#Region "HSPF Run"
                If pRunUci = True Then
                    Logger.Status(Now & " Running HSPF Simulation of " & pBaseName & ".uci", True)
                    Dim lExitCode As Integer
                    ChDriveDir(PathNameOnly(pHSPFExe))
                    lExitCode = LaunchProgram(pHSPFExe, pTestPath, "-1 -1 " & pBaseName & ".uci") 'Run HSPF program
                    ChDriveDir(pTestPath)

                    If lExitCode = -1 Then
                        Throw New ApplicationException("WinHSPFLt could not run, Analysis cannot continue")
                        Exit Sub
                    End If
                End If
                Logger.Status(Now & " HSPF Simulation of " & pBaseName & ".uci" & " finished.", True)
#End Region
                'build collection of operation types to report
                Dim lOperationTypes As New atcCollection
                lOperationTypes.Add("P:", "PERLND")
                lOperationTypes.Add("I:", "IMPLND")
                lOperationTypes.Add("R:", "RCHRES")

                Dim lStr As String = ""
                Dim lRunMade As String = ""

                If IO.File.Exists(lHspfEchoFileName) Then
                    echoFileInfo = New System.IO.FileInfo(lHspfEchoFileName)
                    lRunMade = echoFileInfo.LastWriteTime.ToString
                Else
                    Logger.Msg("The ECHO file is not available for this model. Please check if model ran successfully last time", vbCritical)
                    End
                    Return
                End If

#Region "Read Echo File to decide if Model Ran"
                Dim HSPFRan As Boolean = False
                Using echoFileReader As StreamReader = File.OpenText(lHspfEchoFileName)
                    While Not echoFileReader.EndOfStream
                        Dim nextLine As String = echoFileReader.ReadLine()
                        If Not nextLine.ToUpper.Contains("END OF JOB") Then
                            HSPFRan = False
                        Else
                            HSPFRan = True
                        End If
                    End While
                End Using

                If HSPFRan = False Then
                    Logger.Dbg("ECHO file says that run was terminated last time. HSPEXP+ will exit!")
                    Dim ans As Integer
                    ans = MsgBox("ECHO File contains a message that the run was terminated last time. HSPEXP+ will quit. Please make sure that UCI" &
                                 " file runs properly!")
                    OpenFile(lHspfEchoFileName)
                    End
                End If
#End Region
                loutfoldername = pTestPath

                Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") &
                                    Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")
                loutfoldername = pTestPath & "Reports_" & lDateString & "\"
                Directory.CreateDirectory(loutfoldername)
                File.Copy(pTestPath & pBaseName & ".uci", loutfoldername & pBaseName & ".uci", overwrite:=True)
                'A folder name is given that has the basename and the time when the run was made.



                If StartUp.chkAdditionalgraphs.Checked Then
                    Try
                        ChDriveDir(pTestPath)
                        MakeAutomatedGraphs(SDateJ, EDateJ, loutfoldername, pTestPath)
                    Catch exGraph As Exception
                        Logger.Msg("Exception while making graphs: " & exGraph.ToString)
                    End Try
                End If

                If StartUp.chkReganGraphs.Checked Then
                    ReganGraphs(aHspfUci, SDateJ, EDateJ, loutfoldername)
                End If
#Region "Area Report Generation"
                If pMakeAreaReports Then
                    Dim alocations As New atcCollection
                    For Each lRCHRES As HspfOperation In aHspfUci.OpnSeqBlock.Opns
                        If lRCHRES.Name = "RCHRES" Then
                            alocations.Add("R:" & lRCHRES.Id)
                        End If
                    Next
                    Logger.Status(Now & " Producing Area Reports.", True)
                    Logger.Dbg(Now & " Producing land use and area reports")
                    'Now the area reports are generated for all the reaches in the UCI file.
                    Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(aHspfUci, lRunMade, lOperationTypes, alocations, True, loutfoldername & "/AreaReports/")
                    lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)
                    SaveFileString(loutfoldername & "/AreaReports/AreaReport.txt", lReport.ToString)
                End If
#End Region

#Region "Hydrology Calibration"
                If StartUp.chkExpertStats.Checked = True Then
                    Dim lExpertSystemFileNames As New NameValueCollection
                    AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
                    If lExpertSystemFileNames.Count < 1 Then 'Becky added this if-then to warn the user if no EXS files exist
                        MsgBox("No basins specifications file (*.exs) file found in directory " & IO.Directory.GetCurrentDirectory &
                               "!  Statistics, summaries, and graphs cannot be computed. EXS file can be generated using WinHSPF3.1 or later.",
                               vbOKOnly, "No Specification File!")
                        Logger.Dbg(Now & " No basins specifications file found, no statistics computed")
                    End If

                    Dim lExpertSystem As HspfSupport.atcExpertSystem

                    For Each lExpertSystemFileName As String In lExpertSystemFileNames
                        Logger.Status(Now & " Calculating Expert Statistics for the file " & lExpertSystemFileName, True)
                        Try
                            Logger.Dbg(Now & " Calculating run statistics.")
                            lExpertSystem = New HspfSupport.atcExpertSystem(aHspfUci, lExpertSystemFileName, SDateJ, EDateJ)
                            Dim lHydrologyWDMFileName As String = lExpertSystem.ExpertWDMFileName
                            lStr = lExpertSystem.Report

                            SaveFileString(loutfoldername & "ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)

                            'Becky added these to output advice
                            Logger.Dbg(Now & " Creating advice to save in " & pBaseName & ".*.txt")
                            For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                                Dim lAdviceStr As String = "Advice for Calibration Run " & pBaseName & vbCrLf & Now & vbCrLf & vbCrLf
                                lExpertSystem.CalcAdvice(lAdviceStr, lSiteIndex)
                                Dim lSiteNam As String = lExpertSystem.Sites(lSiteIndex - 1).Name
                                SaveFileString(loutfoldername & pBaseName & "." & lSiteNam & "advice.txt", lAdviceStr)
                            Next

                            Dim lCons As String = "Flow"
                            For Each lSite As HexSite In lExpertSystem.Sites
                                Dim lSiteName As String = lSite.Name
                                Dim lArea As Double = lSite.Area
                                Dim lSimTSerInchesOriginal As atcTimeseries = SubsetByDate(lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(0)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                Dim lSimTSerInches As atcTimeseries = Aggregate(lSimTSerInchesOriginal, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                                lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                                Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                                lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                                lSimTSer.Attributes.SetValue("YAxis", "Left")
                                lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                                Dim lObsTSerOriginal As atcTimeseries = SubsetByDate(lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                Dim lObsTSer As atcTimeseries = Aggregate(lObsTSerOriginal, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                                lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                                lObsTSer.Attributes.SetValue("YAxis", "Left")
                                lObsTSer.Attributes.SetValue("StepType", pCurveStepType)
                                Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTSer, lArea)
                                lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                                'Anurag changed the code so that the original time series for observed flow in cfs 
                                'and simulated flow volume in 'inches could be at a smaller time step. Later on 
                                'Anurag did the same for simulated precipitation.  This way shorter time period time series
                                'could be used for storm graphs if the user is interested.

                                Dim lPrecDsn As Integer = lSite.DSN(5)
                                Dim lPrecTserOriginal As atcTimeseries = SubsetByDate(lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lPrecDsn), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                lPrecTserOriginal.Attributes.SetValue("Units", "inches")
                                Dim lPrecTser As atcTimeseries = Aggregate(lPrecTserOriginal, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)

                                Logger.Dbg(Now & " Calculating monthly summary for " & lSiteName)
                                'pProgressBar.pbProgress.Increment(5)
                                Dim lTSerBroken As atcTimeseries = lSimTSer.Clone

                                Dim PercentMissingObservedData As Double = 0.0

                                If lObsTSerInches.Attributes.GetDefinedValue("Count Missing").Value > 0 Then
                                    PercentMissingObservedData = lObsTSerInches.Attributes.GetDefinedValue("Count Missing").Value * 100 / lObsTSerInches.Attributes.GetDefinedValue("Count").Value
                                    For i As Integer = 1 To lObsTSerInches.numValues
                                        If Double.IsNaN(lObsTSerInches.Value(i)) Then
                                            lSimTSerInches.Value(i) = Double.NaN
                                            lTSerBroken.Value(i) = Double.NaN

                                        End If
                                    Next

                                End If

                                lStr = HspfSupport.MonthlyAverageCompareStats.Report(aHspfUci,
                                                                                     lCons, lSiteName,
                                                                                     "inches",
                                                                                     lSimTSerInches, lObsTSerInches,
                                                                                     lRunMade,
                                                                                     lExpertSystem.SDateJ,
                                                                                     lExpertSystem.EDateJ,
                                                                                     PercentMissingObservedData)
                                Dim lOutFileName As String = loutfoldername & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)

                                Logger.Dbg(Now & " Calculating annual summary for " & lSiteName)
                                lStr = HspfSupport.AnnualCompareStats.Report(aHspfUci,
                                                                             lCons, lSiteName,
                                                                             "inches",
                                                                             lPrecTser, lSimTSerInches, lObsTSerInches,
                                                                             lRunMade,
                                                                             lExpertSystem.SDateJ,
                                                                             lExpertSystem.EDateJ,
                                                                             PercentMissingObservedData)
                                lOutFileName = loutfoldername & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)

                                Logger.Dbg(Now & " Calculating daily summary for " & lSiteName)
                                'pProgressBar.pbProgress.Increment(6)
                                lStr = HspfSupport.DailyMonthlyCompareStats.Report(aHspfUci,
                                                                                   lCons, lSiteName,
                                                                                   lTSerBroken, lObsTSer,
                                                                                   lRunMade,
                                                                                   lExpertSystem.SDateJ,
                                                                                   lExpertSystem.EDateJ,
                                                                                   PercentMissingObservedData)
                                lOutFileName = loutfoldername & "DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)


                                Logger.Status(Now & " Preparing Graphs", True)
                                Dim lTimeSeries As New atcTimeseriesGroup
                                Logger.Dbg(Now & " Creating nonstorm graphs")
                                lTimeSeries.Add("Observed", lObsTSer)
                                lTimeSeries.Add("Simulated", lSimTSer)
                                If PercentMissingObservedData > 0 Then
                                    lTimeSeries.Add("SimulatedBroken", lTSerBroken)
                                End If
                                lTimeSeries.Add("Precipitation", lPrecTser)
                                lTimeSeries.Add("LZS", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(9)))
                                lTimeSeries.Add("UZS", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(8)))
                                lTimeSeries.Add("PotET", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(6)))
                                lTimeSeries.Add("ActET", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(7)))
                                lTimeSeries.Add("Baseflow", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(4)))
                                lTimeSeries.Add("Interflow", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(3)))
                                lTimeSeries.Add("Surface", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(2)))
                                GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ,
                                             lCons, lSiteName,
                                             lTimeSeries,
                                             pGraphSaveFormat,
                                             pGraphSaveWidth,
                                             pGraphSaveHeight,
                                             pGraphAnnual, loutfoldername,
                                            True, True,
                                             True, PercentMissingObservedData)
                                lTimeSeries.Clear()


                                Logger.Dbg(Now & " Creating storm graphs")
                                lSimTSer = InchesToCfs(lSimTSerInchesOriginal, lArea)

                                lTimeSeries.Add("Observed", lObsTSerOriginal)
                                lTimeSeries.Add("Simulated", lSimTSer)
                                lTimeSeries.Add("Prec", lPrecTserOriginal)

                                lTimeSeries(0).Attributes.SetValue("Units", "cfs")
                                lTimeSeries(0).Attributes.SetValue("StepType", pCurveStepType)
                                lTimeSeries(1).Attributes.SetValue("Units", "cfs")
                                lTimeSeries(1).Attributes.SetValue("StepType", pCurveStepType)
                                lTimeSeries(2).Attributes.SetValue("YAxis", "Aux")
                                IO.Directory.CreateDirectory(loutfoldername & "\Storms\")
                                GraphStorms(lTimeSeries, 2, loutfoldername & "Storms\" & lSiteName, pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem, True)
                                lTimeSeries.Dispose()



                            Next

                            lExpertSystem = Nothing

                        Catch lEx As ApplicationException
                            If lEx.Message.Contains("rogram will quit") Then
                                Logger.Msg(lEx.Message)
                                End
                            End If
                            Logger.Dbg(lEx.Message)
                        End Try
                    Next lExpertSystemFileName
                End If
#End Region

#Region "Water Quality"
                If pConstituents.Count > 0 Then
                    'Dim lHspfBinDataSource As New atcDataSource
                    Dim lOpenHspfBinDataSource As New atcDataSource

                    Logger.Dbg(Now & " Opening the binary output files.")

                    'Dim lLocations As New atcCollection
                    For i As Integer = 0 To aHspfUci.FilesBlock.Count
                        If aHspfUci.FilesBlock.Value(i).Typ = "BINO" Then
                            Dim lHspfBinFileName As String = AbsolutePath(aHspfUci.FilesBlock.Value(i).Name.Trim, CurDir())
                            lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                            If lOpenHspfBinDataSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                                    lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                End If
                            End If
                        End If
                    Next i

                    For Each lConstituent As String In pConstituents
                        Dim lConstProperties As New List(Of ConstituentProperties)
                        Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")
                        Logger.Status("Begin summary for " & lConstituent)
                        Dim lConstituentName As String = ""
                        Dim lActiveSections As New List(Of String)
                        Dim CheckQUALID As Boolean = False
                        Select Case lConstituent
                            Case "Water"
                                lConstituentName = "WAT"
                                lActiveSections.Add("PWATER")
                                lActiveSections.Add("IWATER")
                                lActiveSections.Add("HYDR")
                            Case "Sediment"
                                lConstituentName = "SED"
                                lActiveSections.Add("SEDMNT")
                                lActiveSections.Add("SOLIDS")
                                lActiveSections.Add("SEDTRN")
                            Case "DO"
                                lConstituentName = "DO"
                                lActiveSections.Add("PWTGAS")
                                lActiveSections.Add("IWTGAS")
                                lActiveSections.Add("OXRX")
                            Case "Heat"
                                lConstituentName = "Heat"
                                lActiveSections.Add("PWTGAS")
                                lActiveSections.Add("IWTGAS")
                                lActiveSections.Add("HTRCH")
                            Case "TotalN"
                                lConstituentName = "TN"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)

                                lActiveSections.Add("NITR")
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")

                            Case "TotalP"
                                lConstituentName = "TP"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)
                                lActiveSections.Add("NITR")
                                lActiveSections.Add("PHOS")
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")

                            Case "BOD-Labile"
                                lConstituentName = "BOD-Labile"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("OXRX")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")
                            Case "FColi"
                                lConstituentName = "FColi"
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("GQUAL")
                        End Select

                        Dim lScenarioResults As New atcDataSource
                        'If lOpenHspfBinDataSource.DataSets.Count > 1 Then
                        '    Dim lConstituentsToOutput As atcCollection = Utility.ConstituentsToOutput(lConstituent, lConstProperties)
                        '    For Each ConstituentForAnalysis As String In lConstituentsToOutput.Keys
                        '        Dim OpnType As String = SafeSubstring(ConstituentForAnalysis, 0, 2)
                        '        ConstituentForAnalysis = SafeSubstring(ConstituentForAnalysis, 2)
                        '        If Not OpnType = "R:" AndAlso (ConstituentForAnalysis.EndsWith("1") Or ConstituentForAnalysis.EndsWith("2")) Then

                        '            ConstituentForAnalysis = Left(ConstituentForAnalysis, ConstituentForAnalysis.Length - 1)
                        '        End If
                        '        lScenarioResults.DataSets.Add(atcDataManager.DataSets.FindData("Constituent", ConstituentForAnalysis))

                        '    Next

                        'End If

                        If lScenarioResults.DataSets.Count = 0 Then
                            For Each activeSection As String In lActiveSections
                                lScenarioResults.DataSets.Add(atcDataManager.DataSets.FindData("Section", activeSection))
                            Next
                        End If


                        If lScenarioResults.DataSets.Count > 0 Then

                            Dim lReportCons As New atcReport.ReportText
                            lReportCons = Nothing
                            Dim lOutFileName As String = ""

                            LandLoadingReports(loutfoldername, lScenarioResults, aHspfUci, pBaseName, lRunMade, lConstituent, lConstProperties, SDateJ, EDateJ)
                            ReachBudgetReports(loutfoldername, lScenarioResults, aHspfUci, pBaseName, lRunMade, lConstituent, lConstProperties, SDateJ, EDateJ)
                            Logger.Status(Now & " Generating Reports for " & lConstituent)
                            Logger.Dbg(Now & " Generating Reports for " & lConstituent)
                            lReportCons = Nothing

                            If Not (lConstituent = "DO" Or lConstituent = "Heat" Or lConstituent = "BOD-Labile") Then
                                With HspfSupport.ConstituentBudget.Report(aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                                                      lScenarioResults, pOutputLocations, lRunMade, SDateJ, EDateJ, lConstProperties)
                                    lReportCons = .Item1
                                    lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Ann_Avg_Budget.txt"
                                    If lReportCons IsNot Nothing Then SaveFileString(lOutFileName, lReportCons.ToString)

                                    'lReportCons = Nothing
                                    'lReportCons = .Item2
                                    'lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Per_LU_Ann_Avg_NPS_Lds.txt"
                                    'SaveFileString(lOutFileName, lReportCons.ToString)
                                    lReportCons = Nothing
                                    lReportCons = .Item3

                                    lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation.txt"
                                    If lReportCons IsNot Nothing Then SaveFileString(lOutFileName, lReportCons.ToString)
                                    lReportCons = Nothing

                                    lReportCons = .Item4
                                    If pOutputLocations.Count > 0 Then
                                        lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation_Locations.txt"
                                        SaveFileString(lOutFileName, lReportCons.ToString)
                                    End If
                                    lReportCons = Nothing
                                    'lReportCons = .Item5
                                    'lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadingRates.txt"
                                    'SaveFileString(lOutFileName, lReportCons.ToString)
                                    'lReportCons = Nothing

                                    If .Item6 IsNot Nothing AndAlso .Item6.Keys.Count > 0 Then
                                        For Each location As String In .Item6.Keys
                                            CreateGraph_BarGraph(.Item6.ItemByKey(location), loutfoldername & lConstituentName & "_" & pBaseName & "_" & location & "_LoadingAllocation.png")
                                        Next location
                                    End If


                                End With
                                'Logger.Dbg(Now & " Calculating Annual Constituent Balance for " & lConstituent)



                                lReportCons = HspfSupport.ConstituentBalance.Report(aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                lScenarioResults, lRunMade, SDateJ, EDateJ, lConstProperties)
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_OPN_Per_Year.txt"

                                SaveFileString(lOutFileName, lReportCons.ToString)

                                'Logger.Dbg("Summary at " & lLocations.Count & " locations")
                                'constituent balance


                                lReportCons = HspfSupport.WatershedConstituentBalance.Report(aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                lScenarioResults, lRunMade, SDateJ, EDateJ, lConstProperties)
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Grp_By_OPN_LU_Ann_Avg.txt"

                                SaveFileString(lOutFileName, lReportCons.ToString)

                                If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                                    HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                       (aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                        lScenarioResults, pOutputLocations, lRunMade, SDateJ, EDateJ,
                                        lConstProperties, loutfoldername, True)
                                    'now pivoted version
                                    'HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                    '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                                    '    lHspfBinDataSource, pOutputLocations, lRunMade, _
                                    '    lOutFolderName, True, True)
                                End If
                            End If


                        Else
                                Logger.Dbg("The HBN file didn't have any data for the constituent " & lConstituent & "  therefore the balance reports for " &
                                lConstituent & " will not be generated. Make sure that HSPF run completed last time.")
                            Dim ans As Integer
                            ans = MsgBox("HBN files do not have any data.  Constituent Balance reports will not be generated. " &
                                         "Did uci file run properly last time?")
                        End If
                        For Each lTimeSeries As atcTimeseries In lScenarioResults.DataSets
                            lTimeSeries.ValuesNeedToBeRead = True
                        Next
                    Next lConstituent

                End If
#End Region

                Logger.Status(Now & " Output Written to " & loutfoldername)
                Logger.Dbg("Reports Written in " & loutfoldername)

                'pProgressBar.pbProgress.Increment(39)

                Logger.Dbg(Now & " HSPEXP+ Complete")
                Logger.Msg("HSPEXP+ is complete")
                OpenFile(loutfoldername)
            End Using
        Catch ex As Exception
            'Skip to the end if Cancel was chosen in felu            

            Logger.Msg(ex.ToString, MsgBoxStyle.Critical, "HSPEXP+ did not complete successfully.")

        End Try

        Logger.Status("")
        atcDataManager.Clear()
        StartUp.Show()
        'Call Application.Exit()

    End Sub





End Module
