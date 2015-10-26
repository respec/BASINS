Imports System
Imports atcUtility
Imports atcData
Imports atcTimeseriesStatistics
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports atcUCI
Imports atcBasinsObsWQ
Imports atcList
Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports atcGraph
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports System.Collections.Specialized
Imports System.IO 'added by Becky to get directory exists function


Module HSPFOutputReports
    Private pBaseFolders As New ArrayList
    Private pTestPath As String
    Private pRootPath As String 'if the user is using the VT run number folder naming conventions (Run01, Run02, etc.), this is the root path that holds all the run folders
    Private pBaseName As String 'this is the base part of the file name (i.e., without .uci, .wdm, .exs) - it MUST be used to name everything
    Private pRunNo As Integer = -1 'the current run number - may or may not be provided, if it is then use it to copy to a new folder
    Private pOutputLocations As New atcCollection

    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = True
    Private pCurveStepType As String = "NonStep"
    Private pConstituents As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer
    Private pGraphWQOnly As Boolean = False 'indicates whether ONLY graphing water quality
    Private pGraphWQ As Boolean = False 'indicates whether to graph only water quality
    Private pWaterYears As Boolean = False
    Private pExpertPrec As Boolean = False
    Private pIdsPerSeg As Integer = 50
    'following were added by Becky:
    'Private pProgressBar As New RWZProgress
    Private pMakeStdGraphs As Boolean 'flag to indicate user wants standard graphs (monthly, daily, storms, flow duration)
    Private pMakeLogGraphs As Boolean 'flag to indicate user wants logarithmic graphs (all that are logarithmic)
    Private pMakeSupGraphs As Boolean 'flag to indicate user wants supporting graphs (UZS, LZS, ET, cumulative diff)
    Private pMakeAreaReports As Boolean 'flag to indicate user wants subwatershed & land use reports created
    Friend pHSPFExe As String '= FindFile("Please locate WinHspfLt.exe", IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location), "WinHSPFLt", "WinHspfLt.exe"))
    Private pRunUci As Boolean = False 'Anurag added this option if the user wants this program to run the uci as well
    Private SDateJ, EDateJ As Double
    Private loutfoldername As String
    Private pSensitivity As Boolean = False
    

    Private Sub Initialize()
        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            Dim pStatusMonitor As New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            IO.Directory.GetCurrentDirectory, _
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
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1300
        pGraphSaveHeight = 768
        pMakeStdGraphs = StartUp.chkGraphStandard.Checked
        pMakeLogGraphs = StartUp.chkGraphStandard.Checked
        pMakeSupGraphs = StartUp.chkGraphStandard.Checked
        pRunUci = StartUp.chkRunHSPF.Checked
        pMakeAreaReports = StartUp.chkAreaReports.Checked

        Dim lTestName As String = IO.Path.GetFileNameWithoutExtension(StartUp.cmbUCIPath.Text)
        Logger.Status("Beginning analysis of " & lTestName, True)

        If StartUp.chkHydrologySensitivity.Checked Then
            pSensitivity = True
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
            pConstituents.Add("BOD-PQUAL")
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
        Logger.StartToFile(pTestPath & "LogFile.txt")
        Logger.Status("Run characteristics read", True)
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As Object)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        Logger.Status("HSPEXP+ is running.")
        Try
            Using lProgress As New ProgressLevel
                If pSensitivity Then
                    SensitivityAnalysis(pBaseName, pTestPath)
                End If

                If pRunUci = True Then
                    Logger.Status(Now & " Running HSPF Simulation of " & pBaseName & ".uci", True)
                    Dim lExitCode As Integer
                    ChDriveDir(PathNameOnly(pHSPFExe))
                    lExitCode = LaunchProgram(pHSPFExe, pTestPath, "-1 -1 " & pBaseName & ".uci") 'Run HSPF program
                    ChDriveDir(pTestPath)
                    'Dim WinHSPFLtVersionFound As String = FindFile("Please locate WinHspfLt.exe", IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location), "WinHSPFLt", "WinHspfLt.exe"))
                    'Logger.Dbg(WinHSPFLtVersionFound)
                    If lExitCode = -1 Then
                        Throw New ApplicationException("WinHSPFLt could not run, Analysis cannot continue")
                        Exit Sub
                    End If
                End If
                Logger.Status(Now & " HSPF Simulation of " & pBaseName & ".uci" & " finished.", True)


                Logger.Status(Now & " Opening " & pBaseName & ".uci", True)
                'open uci file
                Logger.Dbg(Now & " Attempting to open hspfmsg.wdm")
                Dim lMsg As New atcUCI.HspfMsg
                lMsg.Open("hspfmsg.wdm") 'Becky: this can be found at C:\BASINS\models\HSPF\bin if you did the typical BASINS install
                Logger.Dbg(Now & " Reading " & pBaseName & ".uci")
                Logger.Progress(10, 100)

                'build collection of operation types to report
                Dim lOperationTypes As New atcCollection
                lOperationTypes.Add("P:", "PERLND")
                lOperationTypes.Add("I:", "IMPLND")
                lOperationTypes.Add("R:", "RCHRES")

                Dim lStr As String = ""
                Dim lRunMade As String = ""

                Dim lHspfUci As New atcUCI.HspfUci
                lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
                SDateJ = lHspfUci.GlobalBlock.SDateJ
                EDateJ = lHspfUci.GlobalBlock.EdateJ
                Dim lEchoFileisinFilesBlock As Boolean = False
                Dim lHspfEchoFileName As String = ""
                Dim echoFileInfo As System.IO.FileInfo
                For i As Integer = 0 To lHspfUci.FilesBlock.Count
                    If lHspfUci.FilesBlock.Value(i).Typ = "MESSU" Then
                        lHspfEchoFileName = AbsolutePath(lHspfUci.FilesBlock.Value(i).Name.Trim, CurDir())
                        echoFileInfo = New System.IO.FileInfo(lHspfEchoFileName)
                        lRunMade = echoFileInfo.LastWriteTime.ToString
                        lEchoFileisinFilesBlock = True
                        Exit For
                    End If
                Next
                If Not lEchoFileisinFilesBlock Then
                    lHspfEchoFileName = pTestPath & "hspfecho.out"
                    echoFileInfo = New System.IO.FileInfo(lHspfEchoFileName)
                    lRunMade = echoFileInfo.LastWriteTime.ToString
                End If

                If lHspfEchoFileName = "" Then
                    Logger.Dbg("There is no echo file available, looks like HSPF didn't run last time!")
                    Dim ans As Integer
                    ans = MsgBox("There is no ECHO file available. Did UCI file run properly last time?")
                    End
                End If
                
                Dim EchoFileReader As StreamReader
                EchoFileReader = System.IO.File.OpenText(lHspfEchoFileName)

                If EchoFileReader.ReadToEnd.Contains("terminated") Then
                    Logger.Dbg("ECHO file says that run was terminated last time. HSPEXP+ will exit!")
                    Dim ans As Integer
                    ans = MsgBox("ECHO File contains a message that the run was terminated last time. HSPEXP+ will quit. Please make sure that UCI" & _
                                 " file runs properly!")
                    End
                End If
                EchoFileReader.Close()

                loutfoldername = pTestPath
                Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") & _
                                    Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")
                loutfoldername = pTestPath & "Reports_" & lDateString & "\"
                System.IO.Directory.CreateDirectory(loutfoldername)
                System.IO.File.Copy(pTestPath & pBaseName & ".uci", loutfoldername & pBaseName & ".uci", overwrite:=True)
                'A folder name is given that has the basename and the time when the run was made.

                Logger.Dbg("ReadUCI " & lHspfUci.Name)
                Logger.Dbg(Now & " Successfully read " & pBaseName & ".uci")
                Logger.Progress(20, 100)

                If StartUp.chkAdditionalgraphs.Checked Then
                    Try
                        ChDriveDir(pTestPath)
                        MakeAutomatedGraphs(SDateJ, EDateJ)
                    Catch exGraph As Exception
                        Logger.Msg("Exception while making graphs: " & exGraph.ToString)
                    End Try
                End If


                If pMakeAreaReports Then
                    Dim alocations As New atcCollection
                    For Each lRCHRES As HspfOperation In lHspfUci.OpnSeqBlock.Opns
                        If lRCHRES.Name = "RCHRES" Then
                            alocations.Add("R:" & lRCHRES.Id)
                        End If
                    Next
                    Logger.Status(Now & " Producing Area Reports.", True)
                    Logger.Dbg(Now & " Producing land use and area reports")
                    'Now the area repotrs are generated for all the reaches in the UCI file.
                    Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(lHspfUci, lRunMade, lOperationTypes, alocations, True, loutfoldername & "/AreaReports/")
                    lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)
                    SaveFileString(loutfoldername & "/AreaReports/AreaReport.txt", lReport.ToString)
                End If

                If StartUp.chkExpertStats.Checked = True Then

                    Dim lExpertSystemFileNames As New NameValueCollection
                    AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
                    If lExpertSystemFileNames.Count < 1 Then 'Becky added this if-then to warn the user if no EXS files exist
                        'In future, at this point, user should get an option if they want to make teir own exs file and this program should
                        'help the user do it.
                        MsgBox("No basins specifications file (*.exs) file found in directory " & IO.Directory.GetCurrentDirectory & "!  Statistics, summaries, and graphs cannot be computed.", vbOKOnly, "No Specification File!")
                        Logger.Dbg(Now & " No basins specifications file found, no statistics computed")
                    End If
                    Dim lExpertSystem As HspfSupport.atcExpertSystem
                    'Dim lWdmDataSource As New atcDataSourceWDM()
                    'Dim lWdmDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lWdmFileName)

                    'If lWdmDataSource Is Nothing Then
                    '    If atcDataManager.OpenDataSource(lWdmFileName) Then
                    '        lWdmDataSource = atcDataManager.DataSourceBySpecification(lWdmFileName)
                    '    End If
                    'End If
                    'Opening the wdm file here, so that it opens only if needed.

                    For Each lExpertSystemFileName As String In lExpertSystemFileNames
                        Logger.Status(Now & " Calculating Expert Statistics for the file " & lExpertSystemFileName, True)
                        Try
                            Logger.Dbg(Now & " Calculating run statistics.")
                            lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lExpertSystemFileName)
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

                                'Becky: as best I understand it, this is saying that the site name in the EXS file should
                                '       read RCH# where # is the reach number of the reach with its outlet at the gaging
                                '       station.  Then the code appears to calculate the area from the SCHEMATIC block and
                                '       compare it to the area reported in the EXS file, throwing a warning if they're 
                                '       different.
                                Dim lRchId() As Integer 'Becky changed this to an array to hold multiple reaches
                                Dim lRchIdStr() As String 'Becky added to receive information from the split function
                                'Becky modified this if-then-else to allow multiple reaches to contribute to a station
                                If lSite.Name.StartsWith("RCH") Then
                                    Dim lRchText As String 'Becky added to hold substring of site name
                                    lRchText = lSiteName.Substring(3)
                                    If InStr(lRchText, ",") > 0 Then
                                        lRchIdStr = Split(lRchText, ",")
                                        ReDim lRchId(lRchIdStr.GetUpperBound(0))
                                        For i As Integer = 0 To lRchIdStr.GetUpperBound(0)
                                            lRchId(i) = CInt(lRchIdStr(i))
                                        Next i
                                    Else
                                        ReDim lRchId(0)
                                        lRchId(0) = lRchText
                                    End If
                                Else
                                    Dim ans As Integer 'Becky added the following if-then to allow a clean stop if the user didn't input RCH01 etc.
                                    'I don't know why the AquaTerra code defaults to just taking the information as is, but I continue to allow that
                                    'just in case...
                                    ans = MsgBox("The site name on line 2 of your basins specification file is not in the format RCH##, where ## is the " & _
                                        "RCHRES number of the outlet reach.  It is likely that the program will abort shortly.  Your *.adv and *.sts files " & _
                                        "have already been created.  Click Cancel now to avoid an unhandled error.", MsgBoxStyle.OkCancel + MsgBoxStyle.Critical, _
                                        "Site Name Invalid")
                                    If ans = vbCancel Then
                                        GoTo RWZProgramEnding
                                    End If
                                    ReDim lRchId(0)
                                    lRchId(0) = lSite.Name
                                End If

                                Dim lPrecDsn As Integer = lSite.DSN(5)
                                Dim lPrecTserOriginal As atcTimeseries = SubsetByDate(lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lPrecDsn), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                                lPrecTserOriginal.Attributes.SetValue("Units", "inches")
                                Dim lPrecTser As atcTimeseries = Aggregate(lPrecTserOriginal, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)

                                RWZSetArgs(lSimTSerInches)
                                RWZSetArgs(lSimTSerInchesOriginal)
                                RWZSetArgs(lObsTSerInches)
                                RWZSetArgs(lPrecTser)
                                RWZSetArgs(lPrecTserOriginal)

                                Logger.Dbg(Now & " Calculating monthly summary for " & lSiteName)
                                'pProgressBar.pbProgress.Increment(5)

                                lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                                     lCons, lSiteName, _
                                                                                     "inches", _
                                                                                     lSimTSerInches, lObsTSerInches, _
                                                                                     lRunMade, _
                                                                                     lExpertSystem.SDateJ, _
                                                                                     lExpertSystem.EDateJ)
                                Dim lOutFileName As String = loutfoldername & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)

                                Logger.Dbg(Now & " Calculating annual summary for " & lSiteName)
                                lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                             lCons, lSiteName, _
                                                                             "inches", _
                                                                             lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                             lRunMade, _
                                                                             lExpertSystem.SDateJ, _
                                                                             lExpertSystem.EDateJ)
                                lOutFileName = loutfoldername & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)

                                Logger.Dbg(Now & " Calculating daily summary for " & lSiteName)
                                'pProgressBar.pbProgress.Increment(6)
                                lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                                   lCons, lSiteName, _
                                                                                   lSimTSer, lObsTSer, _
                                                                                   lRunMade, _
                                                                                   lExpertSystem.SDateJ, _
                                                                                   lExpertSystem.EDateJ)
                                lOutFileName = loutfoldername & "DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                                SaveFileString(lOutFileName, lStr)

                                'Becky's addition: only make graphs if user wants them. 
                                If pMakeLogGraphs Or pMakeStdGraphs Or pMakeSupGraphs Then
                                    Logger.Status(Now & " Preparing Graphs", True)
                                    Dim lTimeSeries As New atcTimeseriesGroup
                                    Logger.Dbg(Now & " Creating nonstorm graphs")
                                    lTimeSeries.Add("Observed", lObsTSer)
                                    lTimeSeries.Add("Simulated", lSimTSer)
                                    lTimeSeries.Add("Precipitation", lPrecTser)
                                    lTimeSeries.Add("LZS", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(9)))
                                    lTimeSeries.Add("UZS", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(8)))
                                    lTimeSeries.Add("PotET", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(6)))
                                    lTimeSeries.Add("ActET", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(7)))
                                    lTimeSeries.Add("Baseflow", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(4)))
                                    lTimeSeries.Add("Interflow", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(3)))
                                    lTimeSeries.Add("Surface", lExpertSystem.ExpertWDMDataSource.DataSets.ItemByKey(lSite.DSN(2)))
                                    GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ, _
                                             lCons, lSiteName, _
                                             lTimeSeries, _
                                             pGraphSaveFormat, _
                                             pGraphSaveWidth, _
                                             pGraphSaveHeight, _
                                             pGraphAnnual, loutfoldername, _
                                             pMakeStdGraphs, pMakeLogGraphs, _
                                             pMakeSupGraphs)
                                    lTimeSeries.Clear()

                                    If pMakeStdGraphs Then 'Becky added, only make storm graphs (log or normal) if we want standard graphs
                                        Logger.Dbg(Now & " Creating storm graphs")
                                        lSimTSer = InchesToCfs(lSimTSerInchesOriginal, lArea)
                                        RWZSetArgs(lSimTSer)
                                        'pProgressBar.pbProgress.Increment(29)
                                        lTimeSeries.Add("Observed", lObsTSerOriginal)
                                        lTimeSeries.Add("Simulated", lSimTSer)
                                        lTimeSeries.Add("Prec", lPrecTserOriginal)

                                        lTimeSeries(0).Attributes.SetValue("Units", "cfs")
                                        lTimeSeries(0).Attributes.SetValue("StepType", pCurveStepType)
                                        lTimeSeries(1).Attributes.SetValue("Units", "cfs")
                                        lTimeSeries(1).Attributes.SetValue("StepType", pCurveStepType)
                                        lTimeSeries(2).Attributes.SetValue("YAxis", "Aux")
                                        IO.Directory.CreateDirectory(loutfoldername & "\Storms\")
                                        GraphStorms(lTimeSeries, 2, loutfoldername & "Storms\" & lSiteName, pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem, pMakeLogGraphs)
                                        lTimeSeries.Dispose()
                                    End If
                                End If

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
                Dim lHspfBinDataSource As New atcDataSource
                If pConstituents.Count > 0 Then
                    Logger.Dbg(Now & " Opening the binary output files.")
                    For i As Integer = 0 To lHspfUci.FilesBlock.Count
                        If lHspfUci.FilesBlock.Value(i).Typ = "BINO" Then
                            Dim lHspfBinFileName As String = AbsolutePath(lHspfUci.FilesBlock.Value(i).Name.Trim, CurDir())
                            Dim lOpenHspfBinDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                            If lOpenHspfBinDataSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                                    lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                End If
                            End If
                            If lOpenHspfBinDataSource.DataSets.Count > 1 Then
                                lHspfBinDataSource.DataSets.AddRange(lOpenHspfBinDataSource.DataSets)
                            End If
                        End If
                    Next


                    If lHspfBinDataSource.DataSets.Count > 0 Then
                        Dim lConstituentName As String = ""

                        For Each lConstituent As String In pConstituents
                            Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")
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
                                Case "FColi"
                                    lConstituentName = "FColi"
                            End Select

                            Dim lReportCons As New atcReport.ReportText
                            Dim lOutFileName As String = ""
                            Dim lConstituentsToOutput As atcCollection = Utility.ConstituentsToOutput(lConstituent)


                            Logger.Dbg(Now & " Calculating Constituent Budget for " & lConstituent)
                            lReportCons = Nothing

                            With HspfSupport.ConstituentBudget.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, pOutputLocations, lRunMade)
                                lReportCons = .Item1
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Ann_Avg_Budget.txt"


                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing
                                lReportCons = .Item2
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Per_LU_Ann_Avg_NPS_Lds.txt"
                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing
                                lReportCons = .Item3

                                'The followng reports are being suppressed until the completion of the 21003-09 project
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation.txt"
                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing

                                lReportCons = .Item4
                                If pOutputLocations.Count > 0 Then
                                    lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation_Locations.txt"
                                    SaveFileString(lOutFileName, lReportCons.ToString)
                                End If
                                lReportCons = Nothing

                            End With
                            Logger.Dbg(Now & " Calculating Annual Constituent Balance for " & lConstituent)

                            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")
                            Dim lScenarioResults As New atcDataSource
                            Dim lConstituentNames As New SortedSet(Of String)
                            For Each lKey As String In lConstituentsToOutput.Keys
                                If lKey.EndsWith("1") Or lKey.EndsWith("2") Then
                                    lKey = Left(lKey, lKey.Length - 1)
                                End If

                                lConstituentNames.Add(lKey.Substring(2).ToUpper)
                            Next
                            For Each lTs As atcTimeseries In lHspfBinDataSource.DataSets
                                If lConstituentNames.Contains(lTs.Attributes.GetValue("Constituent").ToString.ToUpper) Then
                                    lScenarioResults.DataSets.Add(lTs)
                                End If
                            Next
                            lReportCons = HspfSupport.ConstituentBalance.Report _
                               (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                                lScenarioResults, lLocations, lRunMade)
                            lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_OPN_Per_Year.txt"

                            SaveFileString(lOutFileName, lReportCons.ToString)

                            Logger.Dbg("Summary at " & lLocations.Count & " locations")
                            'constituent balance


                            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
                            (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                            lScenarioResults, lRunMade)
                            lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Grp_By_OPN_LU_Ann_Avg.txt"

                            SaveFileString(lOutFileName, lReportCons.ToString)

                            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                                    lScenarioResults, pOutputLocations, lRunMade, _
                                    loutfoldername, True)
                                'now pivoted version
                                'HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                                '    lHspfBinDataSource, pOutputLocations, lRunMade, _
                                '    lOutFolderName, True, True)
                            End If
                        Next


                    Else
                        Logger.Dbg("The HBN file didn't have any data and therefore constituent balance reports were not generated. Make sure that HSPF" & _
                                   "run finished last time.")
                        Dim ans As Integer
                        ans = MsgBox("HBN files do not have any data.  Constituent Balance reports will not be generated. " & _
                                     "Did uci file run properly last time?")
                        End
                    End If
                End If
                Logger.Dbg(Now & " Output Written to " & loutfoldername)
                Logger.Dbg("Reports Written in " & loutfoldername)
RWZProgramEnding:
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

    Public Sub RWZSetArgs(ByVal aTimeseries As atcTimeseries)
        Dim lTSerStats As New atcTimeseriesStatistics.atcTimeseriesStatistics 'define new statistics object so can access statistics methods
        Dim lAttrArgs As New atcDataAttributes 'needed argument for open below
        lAttrArgs = lTSerStats.AvailableOperations 'this sets up all the different calculations with the correct properties (copiesinherit etc.)
        'this is necessary because otherwise the properties from the main timeseries get copied to the 
        'aggregated timeseries and the monthly attributes aren't calculated correctly with the seasonal Aggregate and Split functions
        lAttrArgs.SetValue("timeseries", aTimeseries) 'tie the DataAttributes to the source timeseries
        With lTSerStats
            .Open("Sum", lAttrArgs) 'sending anything that doesn't start with "%" or "bins" computes everything in atcTimerseriesStatistics.ComputeStatistics (including Sum)
            .Open("bins", lAttrArgs) 'set the bins needed for the remaining calculations, add as attribute
            .Open("%Sum90", lAttrArgs) 'calculate the sum of values for the 90th percentile, add as attribute
            .Open("%Sum75", lAttrArgs) 'calculate the sum of values for the 75th percentile, add as attribute
            .Open("%Sum50", lAttrArgs) 'calculate the sum of values for the 50th percentile, add as attribute
            .Open("%Sum25", lAttrArgs) 'calculate the sum of values for the 25th percentile, add as attribute
            .Open("%Sum10", lAttrArgs) 'calculate the sum of values for the 10th percentile, add as attribute
            .Open("%50", lAttrArgs) 'calculate the 50th percentile value, add as attribute
        End With
    End Sub
    Public Sub MakeAutomatedGraphs(ByVal lGraphStartJ As Double, ByVal lGraphEndJ As Double)

        'Expect a comma separated file called *.csv
        Dim lFileName As String = "AutomatedGraphs.csv"
        Dim lines() As String
        If System.IO.File.Exists(lFileName) Then
            lines = IO.File.ReadAllLines(pTestPath & lFileName)
        Else
            Throw New ApplicationException("The AutomatedGraphs.csv didn't exist or was blank. Program will quit!")
        End If

        Logger.Dbg("AutomatedGraphs.csv was used as the Graph Specification File")
        Dim lGraphRecords As New ArrayList()
        Dim lRecordIndex As Integer = 1


        For Each line As String In lines
            If Not (line.Contains("***") Or Trim(line) = "" Or Trim(line).StartsWith(",")) Then
                line = line.Replace("deg-", ChrW(186))
                line = line.Replace("mu-", ChrW(956))
                lGraphRecords.Add(line)
            End If
        Next

        Dim lDBFdatasource As New atcDataSourceBasinsObsWQ

        Do
            Dim lTimeseriesGroup As New atcTimeseriesGroup
            lRecordIndex += 1
            Dim lGraphInit() As String = lGraphRecords(lRecordIndex).split(",")
            Dim lNumberOfCurves As Integer = Trim(lGraphInit(2))
            Dim lOutFileName As String = loutfoldername & Trim(lGraphInit(1))
            
            Dim lGraphStartDateJ, lGraphEndDateJ As Double
            'GraphInit has information about number of datasets, their color, symbol etc.
            lRecordIndex += 1

            If Not Trim(lGraphInit(7)) = "" Then
                Dim SDate As String() = Trim(lGraphInit(7)).Split("/")
                lGraphStartDateJ = Date2J(SDate(2), SDate(0), SDate(1))

            Else
                lGraphStartDateJ = lGraphStartJ
            End If
            If Not Trim(lGraphInit(8)) = "" Then
                Dim EDate As String() = Trim(lGraphInit(8)).Split("/")
                lGraphEndDateJ = Date2J(EDate(2), EDate(0), EDate(1), 24, 0)

            Else
                lGraphEndDateJ = lGraphEndJ
            End If

            If Trim(lGraphInit(0)).ToLower = "scatter" Then lNumberOfCurves = 2
            
            For CurveNumber As Integer = 1 To lNumberOfCurves
                Dim lGraphDataset() As String = lGraphRecords(lRecordIndex).split(",")
                Dim lTimeSeries As atcTimeseries = Nothing
                Dim lDataSourceFilename As String = AbsolutePath(Trim(lGraphDataset(1)), pTestPath)
                If IO.File.Exists(lDataSourceFilename) Then
                    Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lDataSourceFilename)

                    If lDataSource Is Nothing Then
                        If atcDataManager.OpenDataSource(lDataSourceFilename) Then
                            lDataSource = atcDataManager.DataSourceBySpecification(lDataSourceFilename)
                        End If
                    End If
                    If lDataSource Is Nothing Then
                        Throw New ApplicationException("Could not open '" & lDataSourceFilename & "'")
                    End If
                    Select Case IO.Path.GetExtension(lDataSourceFilename)
                        Case ".wdm"
                            lTimeSeries = lDataSource.DataSets.FindData("ID", Trim(lGraphDataset(2)))(0)
                            lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                            If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                               " DSN " & Trim(lGraphDataset(2)) & ". Program will quit!")
                            End If

                        Case ".hbn", ".dbf"
                            lTimeSeries = lDataSource.DataSets.FindData("Location", Trim(lGraphDataset(2))) _
                                                              .FindData("Constituent", Trim(lGraphDataset(3)))(0)
                            lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                            If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                                                      " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Program will quit!")
                            End If

                        Case ".rdb"
                            lTimeSeries = lDataSource.DataSets.FindData("ParmCode", Trim(lGraphDataset(2)))(0)
                            lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                            If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                                                      " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Program will quit!")
                            End If
                    End Select


                    Dim aTu As Integer = lTimeSeries.Attributes.GetValue("TimeUnit")
                    lTimeSeries.Attributes.SetValue("YAxis", Trim(lGraphDataset(0)))

                    lTimeSeries = AggregateTS(lTimeSeries, Trim(lGraphDataset(10)).ToLower, Trim(lGraphDataset(11)).ToLower)

                    Dim Transformation As String = Trim(lGraphDataset(12)).ToLower
                    Select Case True
                        Case Transformation.Contains("c to f")
                            lTimeSeries = lTimeSeries * 1.8 + 32

                        Case Transformation.Contains("f to c")
                            lTimeSeries = (lTimeSeries - 32) * 0.56

                        Case Transformation.Contains("sum")
                            Dim Sum As Double = Convert.ToDouble(Transformation.Split(" ")(1))
                            lTimeSeries = lTimeSeries + Sum

                        Case Transformation.Contains("product")
                            Dim Product As Double = Convert.ToDouble(Transformation.Split(" ")(1))
                            lTimeSeries = lTimeSeries * Product
                    End Select



                    If Not Trim(lGraphInit(18)) = "" Then

                        Dim SeasonStart() As Integer = Array.ConvertAll(lGraphInit(18).Split("/"), Function(str) Int32.Parse(str))
                        Dim SeasonEnd() As Integer = Array.ConvertAll(lGraphInit(19).Split("/"), Function(str) Int32.Parse(str))
                        Dim lseasons As New atcSeasonsYearSubset(SeasonStart(0), SeasonStart(1), SeasonEnd(0), SeasonEnd(1))
                        lseasons.SeasonSelected(0) = True
                        lTimeSeries = lseasons.SplitBySelected(lTimeSeries, Nothing).ItemByIndex(1)
                    End If

                    lTimeseriesGroup.Add(lTimeSeries)
                Else
                    Logger.Msg("Could not open '" & lDataSourceFilename & "' Aborting Graphing.", MsgBoxStyle.OkOnly, "HSPEXP+")
                    Exit Do
                End If
                lRecordIndex += 1
            Next CurveNumber

            Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
            Select Case Trim(lGraphInit(0)).ToLower
                Case "timeseries"
                    TimeSeriesgraph(lTimeseriesGroup, lZgc, lGraphInit, lGraphRecords, lRecordIndex)
                Case "frequency"
                    FrequencyGraph(lTimeseriesGroup, lZgc, lGraphInit, lGraphRecords, lRecordIndex)
                Case "scatter"
                    lRecordIndex += 2
                    ScatterPlotGraph(lTimeseriesGroup, lZgc, lGraphInit, lGraphRecords, lRecordIndex)
                Case "cumulative probability"
                    CumulativeProbability(lTimeseriesGroup, lZgc, lGraphInit, lGraphRecords, lRecordIndex)
            End Select

            Dim GraphDirectory As String = System.IO.Path.GetDirectoryName(lOutFileName)
            If Not System.IO.Directory.Exists(GraphDirectory) Then
                System.IO.Directory.CreateDirectory(GraphDirectory)
            End If
            lZgc.SaveIn(lOutFileName)
            Dim newlistofattributes() As String = {"Location", "Constituent"}
            atcData.atcDataManager.DisplayAttributesSet(newlistofattributes)
            Dim lList As New atcList.atcListPlugin
            lList.Save(lTimeseriesGroup, lOutFileName.Substring(0, Len(lOutFileName) - 4) & ".txt")
            lZgc.Dispose()
            lRecordIndex -= 1
            lTimeseriesGroup = Nothing
        Loop While (lRecordIndex + 1) < lGraphRecords.Count
        'atcDataManager.Clear()
    End Sub

    Private Function AggregateTS(ByVal aTimeseries As atcTimeseries, ByVal aTimeUnit As String, ByVal aTran As String) As atcTimeseries
        Select Case aTimeUnit & "_" & aTran
            Case "daily_average"
                Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            Case "daily_min"
                Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMin)
            Case "daily_max"
                Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMax)
            Case "daily_sum"
                Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            Case "monthly_average"
                Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
            Case "monthly_sum"
                Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
            Case "monthly_min"
                Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMin)
            Case "monthly_max"
                Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMax)
            Case "yearly_average"
                Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
            Case "yearly_sum"
                Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
            Case "yearly_min"
                Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMin)
            Case "yearly_max"
                Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMax)
        End Select

        Return aTimeseries
    End Function
    Private Function TimeSeriesgraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object, _
                                     ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer)
        Dim lGrapher As New clsGraphTime(aTimeseriesgroup, aZgc)
        Dim lNumberofCurves = Trim(aGraphInit(2))
        aRecordIndex -= lNumberOfCurves
        Dim lNumberofAuxPaneCurves As Integer = 0
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing
        Dim lAuxPane As GraphPane = Nothing
        Dim lCurve As ZedGraph.LineItem = Nothing


        If aZgc.MasterPane.PaneList.Count > 1 Then
            lPaneMain = aZgc.MasterPane.PaneList(1)
            lAuxPane = aZgc.MasterPane.PaneList(0)
            lAuxPane.YAxis.Title.Text = aGraphInit(5)
            If Not Trim(aGraphInit(11)) = "" Then
                lAuxPane.YAxis.Scale.Min = Trim(aGraphInit(11))
            End If
            If Not Trim(aGraphInit(12)) = "" Then
                lAuxPane.YAxis.Scale.Max = Trim(aGraphInit(12))
            End If
            If Trim(aGraphInit(16)).ToLower = "yes" Then
                lAuxPane.YAxis.Type = AxisType.Log
            End If
        Else
            lPaneMain = aZgc.MasterPane.PaneList(0)
        End If
        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))
        lPaneMain.Y2Axis.Title.Text = Trim(aGraphInit(6))

        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        If Not Trim(aGraphInit(13)) = "" Then
            lPaneMain.Y2Axis.Scale.Min = Trim(aGraphInit(13))
        End If
        If Not Trim(aGraphInit(14)) = "" Then
            lPaneMain.Y2Axis.Scale.Max = Trim(aGraphInit(14))
        End If
        If Trim(aGraphInit(15)) = "yes" Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If
        If Trim(aGraphInit(17)) = "yes" Then
            lPaneMain.Y2Axis.Type = AxisType.Log
        End If

        For CurveNumber As Integer = 1 To lNumberOfCurves
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex).split(",")
            If Trim(lGraphDataset(0)).ToLower = "aux" Then
                lCurve = lAuxPane.CurveList.Item(lNumberofAuxPaneCurves)
                lNumberofAuxPaneCurves += 1
            ElseIf (Trim(lGraphDataset(0)).ToLower = "left" Or _
                        Trim(lGraphDataset(0)).ToLower = "right") Then
                lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
                lNumberOfMainPaneCurves += 1
            End If

            If Trim(lGraphDataset(4)).ToLower = "line" Then
                lCurve.Symbol.Type = SymbolType.None
                lCurve.Line.IsVisible = True
                lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid
                lCurve.Line.Width = Math.Max(Convert.ToInt32(Trim(lGraphDataset(8))), 1)

            Else
                lCurve.Line.IsVisible = False
                Select Case Trim(lGraphDataset(7)).ToLower
                    Case "circle"
                        lCurve.Symbol.Type = SymbolType.Circle

                    Case "square"
                        lCurve.Symbol.Type = SymbolType.Square

                    Case "plus"
                        lCurve.Symbol.Type = SymbolType.Plus
                    Case "diamond"
                        lCurve.Symbol.Type = SymbolType.Diamond

                    Case "hdash"
                        lCurve.Symbol.Type = SymbolType.HDash
                    Case "triangle"
                        lCurve.Symbol.Type = SymbolType.Triangle
                    Case "triangledown"
                        lCurve.Symbol.Type = SymbolType.TriangleDown

                    Case "vdash"
                        lCurve.Symbol.Type = SymbolType.VDash
                    Case "xcross"
                        lCurve.Symbol.Type = SymbolType.XCross
                    Case "star"
                        lCurve.Symbol.Type = SymbolType.Star

                    Case Else
                        lCurve.Symbol.Type = SymbolType.Circle


                End Select
                lCurve.Symbol.Fill.IsVisible = True
                lCurve.Symbol.Size = Math.Max(Convert.ToInt32(Trim(lGraphDataset(8))), 1)
            End If

            
            lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

            If Trim(lGraphDataset(6)).ToLower.Contains("forward") Then
                lCurve.Line.StepType = StepType.ForwardStep
            ElseIf Trim(lGraphDataset(6)).ToLower.Contains("rear") Then
                lCurve.Line.StepType = StepType.RearwardStep
            Else
                lCurve.Line.StepType = StepType.NonStep
            End If

            If Not Trim(lGraphDataset(9)) = "" Then
                If Trim(lGraphDataset(9)).ToLower = "don't show" Then
                    lCurve.Label.IsVisible = False
                End If
                lCurve.Label.Text = Trim(lGraphDataset(9))
            End If


         



                'End If
            aRecordIndex += 1
        Next CurveNumber
        aTimeseriesgroup = Nothing
        Return aZgc
    End Function
    Private Function FrequencyGraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object, _
                                     ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer)
        Dim lGrapher As New clsGraphProbability(aTimeseriesgroup, aZgc)

        Dim lNumberofCurves = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))
        lPaneMain.Legend.FontSpec.Size = 12

        If Trim(aGraphInit(15)) = "yes" Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If

        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.MinAuto = False
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.MaxAuto = False
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        lPaneMain.AxisChange()
        For CurveNumber As Integer = 1 To lNumberofCurves
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex).split(",")


            lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
            If Trim(lGraphDataset(4)).ToLower = "line" Then
                lCurve.Symbol.Type = SymbolType.None
                lCurve.Line.IsVisible = True
                lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid
                lCurve.Line.Width = Trim(lGraphDataset(8))
            End If

            If Trim(lGraphDataset(5)).ToLower = "nonstep" Then
                lCurve.Line.StepType = StepType.NonStep
            Else
                lCurve.Line.StepType = StepType.ForwardStep
            End If


            lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

            If Not Trim(lGraphDataset(9)) = "" Then
                If Trim(lGraphDataset(9)).ToLower = "don't show" Then
                    lCurve.Label.IsVisible = False
                End If
                lCurve.Label.Text = Trim(lGraphDataset(9))

            End If

            'End If
            aRecordIndex += 1
            lNumberOfMainPaneCurves += 1
        Next CurveNumber
        Return aZgc
    End Function
    Private Function ScatterPlotGraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object, ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer)
        Dim lGrapher As New clsGraphScatter(aTimeseriesgroup, aZgc)

        Dim lNumberofCurves = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))


        lPaneMain.XAxis.Scale.Min = aTimeseriesgroup(0).Attributes.GetValue("Minimum")

        lPaneMain.XAxis.Scale.Max = aTimeseriesgroup(0).Attributes.GetValue("Maximum")
        lPaneMain.YAxis.Scale.Min = aTimeseriesgroup(1).Attributes.GetValue("Minimum")
        lPaneMain.YAxis.Scale.Max = aTimeseriesgroup(1).Attributes.GetValue("Maximum")
        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        lPaneMain.AxisChange()

        If Trim(aGraphInit(15)) = "yes" Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If


        Dim lGraphDataset() As String = aGraphRecords(aRecordIndex).split(",")

        lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
        If Trim(lGraphDataset(4)).ToLower = "line" Then
            lCurve.Symbol.Type = SymbolType.None
            lCurve.Line.IsVisible = True
            lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid
            lCurve.Line.Width = Trim(lGraphDataset(8))
        Else
            lCurve.Line.IsVisible = False
            Select Case Trim(lGraphDataset(7)).ToLower
                Case "circle"
                    lCurve.Symbol.Type = SymbolType.Circle
                    lCurve.Symbol.Fill.IsVisible = True
                Case "square"
                    lCurve.Symbol.Type = SymbolType.Square
                    lCurve.Symbol.Fill.IsVisible = True
                Case "plus"
                    lCurve.Symbol.Type = SymbolType.Plus
                Case "diamond"
                    lCurve.Symbol.Type = SymbolType.Diamond
                    lCurve.Symbol.Fill.IsVisible = True
                Case "hdash"
                    lCurve.Symbol.Type = SymbolType.HDash
                Case "triangle"
                    lCurve.Symbol.Type = SymbolType.Triangle
                    lCurve.Symbol.Fill.IsVisible = True
                Case "triangledown"
                    lCurve.Symbol.Type = SymbolType.TriangleDown
                    lCurve.Symbol.Fill.IsVisible = True
                Case "vdash"
                    lCurve.Symbol.Type = SymbolType.VDash
                Case "xcross"
                    lCurve.Symbol.Type = SymbolType.XCross
                Case "star"
                    lCurve.Symbol.Type = SymbolType.Star
                    lCurve.Symbol.Fill.IsVisible = True
            End Select
            lCurve.Symbol.Size = Trim(lGraphDataset(8))
        End If

        

        If Trim(lGraphDataset(5)).ToLower = "nonstep" Then
            lCurve.Line.StepType = StepType.NonStep
        Else
            lCurve.Line.StepType = StepType.ForwardStep
        End If


        lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

        If Not Trim(lGraphDataset(9)) = "" Then
            lCurve.Label.Text = Trim(lGraphDataset(9))
        End If
        aRecordIndex += 2
       
        For RecordIndex As Integer = 3 To lNumberofCurves
            lGraphDataset = aGraphRecords(aRecordIndex).split(",")
            Select Case Trim(lGraphDataset(0)).ToLower
                Case "regression"
                    Dim lACoef As Double
                    Dim lBCoef As Double
                    Dim lRSquare As Double
                    Dim lSJDay As Double
                    Dim lEJDay As Double
                    Dim lTimeseriesX As atcTimeseries = aTimeseriesgroup(0)
                    Dim lTimeseriesY As atcTimeseries = aTimeseriesgroup(1)
                    If lTimeseriesX.Dates.Values(0) < lTimeseriesY.Dates.Values(0) Then
                        'y starts after x, use y start date
                        lSJDay = lTimeseriesY.Dates.Values(0)
                    Else 'use x start date
                        lSJDay = lTimeseriesX.Dates.Values(0)
                    End If
                    If lTimeseriesX.Dates.Values(lTimeseriesX.Dates.numValues) < lTimeseriesY.Dates.Values(lTimeseriesY.Dates.numValues) Then
                        'x ends before y, use x end date
                        lEJDay = lTimeseriesX.Dates.Values(lTimeseriesX.Dates.numValues)
                    Else 'use y end date
                        lEJDay = lTimeseriesY.Dates.Values(lTimeseriesY.Dates.numValues)
                    End If

                    Dim lSubsetTimeseriesX As atcTimeseries = SubsetByDate(lTimeseriesX, lSJDay, lEJDay, Nothing)
                    Dim lSubsetTimeseriesY As atcTimeseries = SubsetByDate(lTimeseriesY, lSJDay, lEJDay, Nothing)

                    FitLine(lSubsetTimeseriesX, lSubsetTimeseriesY, lACoef, lBCoef, lRSquare, "")
                    Dim lLine As ZedGraph.LineItem = AddLine(lPaneMain, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid)
                    lLine.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)
                    lLine.Line.Width = Trim(lGraphDataset(8))
                    lLine.Label.Text = Trim(lGraphDataset(9))
                    Dim lText As New TextObj
                    Dim lFmt As String = "###,##0.###"
                    lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & Environment.NewLine & _
                                 "R = " & DoubleToString(Math.Sqrt(lRSquare), , lFmt) & vbCrLf & _
                                 "R Squared = " & DoubleToString(lRSquare, , lFmt)
                    lText.FontSpec.StringAlignment = Drawing.StringAlignment.Near

                    lText.Location = New Location(0.05, 0.15, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                    lText.FontSpec.Border.IsVisible = False
                    lPaneMain.GraphObjList.Add(lText)
                    lPaneMain.XAxis.Title.Text &= vbCrLf & vbCrLf & "Scatter Plot"

                Case "45-deg line"
                    Dim lLine As ZedGraph.LineItem = AddLine(lPaneMain, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
                    lLine.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)
                    lLine.Line.Width = Trim(lGraphDataset(8))
                    lLine.Label.Text = Trim(lGraphDataset(9))
            End Select
            lPaneMain.Legend.IsVisible = True

            aRecordIndex += 1

        Next
        Return aZgc
    End Function
    Private Function CumulativeProbability(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object, _
                                     ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer)

        Dim lNumberofCurves = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))

        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If

        Return aZgc

    End Function
   


End Module
