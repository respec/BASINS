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
    Private ASDate, AEDate As Date



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
        pMakeStdGraphs = True
        pMakeLogGraphs = True
        pMakeSupGraphs = True
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
            pConstituents.Add("BOD-Labile")
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

    Public Sub ScriptMain(ByRef aMapWin As Object, ByVal aHspfUci As atcUCI.HspfUci)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        Logger.Status("HSPEXP+ is running.")
        Try
            Using lProgress As New ProgressLevel
                SDateJ = StartUp.DateTimePicker1.Value.ToOADate()
                EDateJ = StartUp.DateTimePicker2.Value.ToOADate() + 1

                If pSensitivity Then
                    SensitivityAnalysis(pBaseName, pTestPath, SDateJ, EDateJ)
                End If

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

                'build collection of operation types to report
                Dim lOperationTypes As New atcCollection
                lOperationTypes.Add("P:", "PERLND")
                lOperationTypes.Add("I:", "IMPLND")
                lOperationTypes.Add("R:", "RCHRES")

                Dim lStr As String = ""
                Dim lRunMade As String = ""

                Dim lEchoFileisinFilesBlock As Boolean = False
                Dim lHspfEchoFileName As String = ""
                Dim echoFileInfo As System.IO.FileInfo
                For i As Integer = 0 To aHspfUci.FilesBlock.Count
                    If aHspfUci.FilesBlock.Value(i).Typ = "MESSU" Then
                        lHspfEchoFileName = AbsolutePath(aHspfUci.FilesBlock.Value(i).Name.Trim, CurDir()) 'Should check if the echo file is present
                        If IO.File.Exists(lHspfEchoFileName) Then
                            echoFileInfo = New System.IO.FileInfo(lHspfEchoFileName)
                            lRunMade = echoFileInfo.LastWriteTime.ToString
                            lEchoFileisinFilesBlock = True
                        Else
                            Logger.Msg("The ECHO file is not available for this model. Please check if model ran successfully last time", vbCritical)
                            Return
                        End If
                        Exit For
                    End If
                Next
                If Not lEchoFileisinFilesBlock Then
                    lHspfEchoFileName = pTestPath & "hspfecho.out"
                    If IO.File.Exists(lHspfEchoFileName) Then
                        echoFileInfo = New System.IO.FileInfo(lHspfEchoFileName)
                        lRunMade = echoFileInfo.LastWriteTime.ToString
                        lEchoFileisinFilesBlock = True
                    Else
                        Logger.Msg("The ECHO file is not available for this model. Please check if model ran successfully last time", vbCritical)
                        Return
                    End If
                End If



                Dim HSPFRan As Boolean = True
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
                    End
                End If

                loutfoldername = pTestPath
                Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") & _
                                    Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")
                loutfoldername = pTestPath & "Reports_" & lDateString & "\"
                System.IO.Directory.CreateDirectory(loutfoldername)
                System.IO.File.Copy(pTestPath & pBaseName & ".uci", loutfoldername & pBaseName & ".uci", overwrite:=True)
                'A folder name is given that has the basename and the time when the run was made.


                If StartUp.chkAdditionalgraphs.Checked Then
                    Try
                        ChDriveDir(pTestPath)
                        MakeAutomatedGraphs(SDateJ, EDateJ)
                    Catch exGraph As Exception
                        Logger.Msg("Exception while making graphs: " & exGraph.ToString)
                    End Try
                End If

                If StartUp.chkReganGraphs.Checked Then
                    ReganGraphs(aHspfUci, SDateJ, EDateJ, loutfoldername)
                End If

                If pMakeAreaReports Then
                    Dim alocations As New atcCollection
                    For Each lRCHRES As HspfOperation In aHspfUci.OpnSeqBlock.Opns
                        If lRCHRES.Name = "RCHRES" Then
                            alocations.Add("R:" & lRCHRES.Id)
                        End If
                    Next
                    Logger.Status(Now & " Producing Area Reports.", True)
                    Logger.Dbg(Now & " Producing land use and area reports")
                    'Now the area repotrs are generated for all the reaches in the UCI file.
                    Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(aHspfUci, lRunMade, lOperationTypes, alocations, True, loutfoldername & "/AreaReports/")
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

                                If pMakeLogGraphs Or pMakeStdGraphs Or pMakeSupGraphs Then
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
                                             pMakeStdGraphs, pMakeLogGraphs,
                                             pMakeSupGraphs, PercentMissingObservedData)
                                    lTimeSeries.Clear()

                                    If pMakeStdGraphs Then 'Becky added, only make storm graphs (log or normal) if we want standard graphs
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

                If pConstituents.Count > 0 Then

                    For Each lConstituent As String In pConstituents
                        Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")
                        Dim AcceptableQUALNames As New List(Of String)
                        Dim lConstituentName As String = ""
                        Dim CheckQUALID As Boolean = False
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
                                CheckQUALID = True
                                AcceptableQUALNames.Add("NO3")
                                AcceptableQUALNames.Add("NH3+NH4")
                                AcceptableQUALNames.Add("BOD")
                            Case "TotalP"
                                lConstituentName = "TP"
                                CheckQUALID = True
                                AcceptableQUALNames.Add("ORTHO P")
                                AcceptableQUALNames.Add("BOD")
                            Case "BOD-Labile"
                                lConstituentName = "BOD-Labile"
                                CheckQUALID = True
                                AcceptableQUALNames.Add("BOD")
                            Case "FColi"
                                lConstituentName = "FColi"
                        End Select

                        'Following part of code checks if UCI file contains proper QUALID name before going any further for all operations. May be time consuming for lot of operations.
                        Dim NQUALS As Integer = 0
                        Dim QUALID As String = ""

                        If CheckQUALID Then 'Model had issues when PQUAL was not active
                            Dim QUALIDS As New List(Of String)
                            For i As Integer = 0 To aHspfUci.OpnSeqBlock.Opns.Count - 1
                                Dim lOperationType As String = aHspfUci.OpnSeqBlock.Opns(i).Name
                                If lOperationType = "PERLND" Or lOperationType = "IMPLND" Then
                                    Dim QUAL As String = lOperationType.Substring(0, 1) & "QALFG"
                                    Dim QUALActivity As Integer = aHspfUci.OpnSeqBlock.Opn(i).Tables("ACTIVITY").Parms(QUAL).Value
                                    If QUALActivity = 0 Then 'Make sure to double check code for AGCHEM cases.

                                        Logger.Dbg("The operation " & lOperationType & " " & aHspfUci.OpnSeqBlock.Opns(i).Id & _
                                                     "does not have PQUAL section active. Please check output for consistency!")
                                    Else
                                        NQUALS = aHspfUci.OpnSeqBlock.Opn(i).Tables("QUAL-PROPS").OccurCount
                                        For k As Integer = 0 To aHspfUci.OpnSeqBlock.Opn(i).Tables.Count - 1
                                            If aHspfUci.OpnSeqBlock.Opn(i).Tables(k).Name = "QUAL-PROPS" Then

                                                QUALID = aHspfUci.OpnSeqBlock.Opn(i).Tables(k).Parms(0).Value
                                                If Not QUALIDS.Contains(QUALID) Then
                                                    QUALIDS.Add(QUALID)
                                                End If

                                                If Not AcceptableQUALNames.Contains(QUALID) Then

                                                End If
                                            End If
                                        Next k
                                    End If
                                End If
                            Next i
                            For Each QUALIDItem In AcceptableQUALNames
                                If Not QUALIDS.Contains(QUALIDItem) Then
                                    Dim ans As Integer
                                    ans = MsgBox("The QUALID " & QUALIDItem & "has not been used for all the operations. The nutrient balance reports will not continue" & _
                                                 " and the program will exit!")
                                    End

                                End If
                            Next QUALIDItem
                        End If


                        'Done checking QUALID
                        'Dim lHspfBinDataSource As New atcDataSource
                        Dim lConstituentsToOutput As atcCollection = Utility.ConstituentsToOutput(lConstituent)
                        Logger.Dbg(Now & " Opening the binary output files.")
                        Dim lScenarioResults As New atcDataSource
                        Dim lLocations As New atcCollection
                        For i As Integer = 0 To aHspfUci.FilesBlock.Count
                            If aHspfUci.FilesBlock.Value(i).Typ = "BINO" Then
                                Dim lHspfBinFileName As String = AbsolutePath(aHspfUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lOpenHspfBinDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                If lOpenHspfBinDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lHspfBinFileName) Then
                                        lOpenHspfBinDataSource = atcDataManager.DataSourceBySpecification(lHspfBinFileName)
                                    End If
                                End If
                                If lOpenHspfBinDataSource.DataSets.Count > 1 Then

                                    lLocations.AddRange(lOpenHspfBinDataSource.DataSets.SortedAttributeValues("Location"))
                                    Dim lConstituentNames As New SortedSet(Of String)
                                    For Each lKey As String In lConstituentsToOutput.Keys
                                        If (lKey.EndsWith("1") Or lKey.EndsWith("2")) And Not (lKey.ToUpper.Contains("EXIT") Or lKey.ToUpper.Contains("OVOL")) Then
                                            lKey = Left(lKey, lKey.Length - 1)
                                        End If

                                        lConstituentNames.Add(lKey.Substring(2).ToUpper)
                                    Next
                                    For Each lTs As atcTimeseries In lOpenHspfBinDataSource.DataSets
                                        Dim ConstituentFromTS = lTs.Attributes.GetValue("Constituent").ToString.ToUpper
                                        If lConstituentNames.Contains(ConstituentFromTS) Then
                                            'If ConstituentsThatUseLast.Contains(ConstituentFromTS) Then
                                            lTs = SubsetByDate(lTs, SDateJ, EDateJ, Nothing)
                                            lScenarioResults.DataSets.Add(lTs)
                                            'Else
                                            'Should be able to aggregate here, but need a better definition of TS that needs to be
                                            'summed, averaged, or for the ones that need last.
                                            'lTs = Aggregate(lTs, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                                            'lScenarioResults.DataSets.Add(lTs)
                                            'End If

                                        End If
                                    Next lTs
                                End If

                            End If


                        Next i

                        If lScenarioResults.DataSets.Count > 0 Then
                            Dim lReportCons As New atcReport.ReportText

                            Dim lOutFileName As String = ""

                            Logger.Dbg(Now & " Calculating Constituent Budget for " & lConstituent)
                            lReportCons = Nothing

                            With HspfSupport.ConstituentBudget.Report(aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                                                      lScenarioResults, pOutputLocations, lRunMade, SDateJ, EDateJ)
                                lReportCons = .Item1
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Ann_Avg_Budget.txt"

                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing
                                lReportCons = .Item2
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_RCH_Per_LU_Ann_Avg_NPS_Lds.txt"
                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing
                                lReportCons = .Item3

                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation.txt"
                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing

                                lReportCons = .Item4
                                If pOutputLocations.Count > 0 Then
                                    lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadAllocation_Locations.txt"
                                    SaveFileString(lOutFileName, lReportCons.ToString)
                                End If
                                lReportCons = Nothing
                                lReportCons = .Item5
                                lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_LoadingRates.txt"
                                SaveFileString(lOutFileName, lReportCons.ToString)
                                lReportCons = Nothing


                                CreateGraph_BoxAndWhisker(.Item6, loutfoldername & lConstituentName & "_" & pBaseName & "_LoadingRates.png")


                                For Each location As String In .Item7.Keys
                                    CreateGraph_BarGraph(.Item7.ItemByKey(location), loutfoldername & lConstituentName & "_" & pBaseName & "_" & location & "_LoadingAllocation.png")


                                Next location




                            End With
                            Logger.Dbg(Now & " Calculating Annual Constituent Balance for " & lConstituent)


                            lReportCons = HspfSupport.ConstituentBalance.Report _
                               (aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                lScenarioResults, lLocations, lRunMade, SDateJ, EDateJ)
                            lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Per_OPN_Per_Year.txt"

                            SaveFileString(lOutFileName, lReportCons.ToString)

                            Logger.Dbg("Summary at " & lLocations.Count & " locations")
                            'constituent balance


                            lReportCons = HspfSupport.WatershedConstituentBalance.Report _
                            (aHspfUci, lConstituent, lOperationTypes, pBaseName,
                            lScenarioResults, lRunMade, SDateJ, EDateJ)
                            lOutFileName = loutfoldername & lConstituentName & "_" & pBaseName & "_Grp_By_OPN_LU_Ann_Avg.txt"

                            SaveFileString(lOutFileName, lReportCons.ToString)

                            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                   (aHspfUci, lConstituent, lOperationTypes, pBaseName,
                                    lScenarioResults, pOutputLocations, lRunMade, SDateJ, EDateJ,
                                    loutfoldername, True)
                                'now pivoted version
                                'HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                                '   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                                '    lHspfBinDataSource, pOutputLocations, lRunMade, _
                                '    lOutFolderName, True, True)
                            End If
                        Else
                            Logger.Dbg("The HBN file didn't have any data for the constituent " & lConstituent & "  therefore the balance reports for " & _
                                lConstituent & " will not be generated. Make sure that HSPF run completed last time.")
                            Dim ans As Integer
                            ans = MsgBox("HBN files do not have any data.  Constituent Balance reports will not be generated. " & _
                                         "Did uci file run properly last time?")
                        End If
                        For Each lTimeSeries As atcTimeseries In lScenarioResults.DataSets
                            lTimeSeries.ValuesNeedToBeRead = True
                        Next
                    Next lConstituent

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

    
    Public Sub MakeAutomatedGraphs(ByVal lGraphStartJ As Double, ByVal lGraphEndJ As Double)



        'Expect a comma separated file called *.csv
        'On 11/13/2015, Anurag decided that aany number of CSV file with *.csv extension could be added. 
        '
        Dim lGraphSpecificationFileNames As New NameValueCollection
        AddFilesInDir(lGraphSpecificationFileNames, IO.Directory.GetCurrentDirectory, False, "*.csv")
        If lGraphSpecificationFileNames.Count < 1 Then '
            '
            Throw New ApplicationException("No CSV (*.csv) file found in directory " & IO.Directory.GetCurrentDirectory)
            Logger.Dbg(Now & " Custom graphs will not be produced.")
        End If
        Dim lGraphFilesCount = 0
        For Each lGraphSpecificationFile As String In lGraphSpecificationFileNames
            lGraphFilesCount += 1
            Dim lgraphRecordsNew As New ArrayList()
            Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lGraphSpecificationFile)
                Dim lines() As String = {}
                If System.IO.File.Exists(lGraphSpecificationFile) Then

                    MyReader.TextFieldType = FileIO.FieldType.Delimited
                    MyReader.SetDelimiters(",")
                    Dim CurrentRow As String()

                    While Not MyReader.EndOfData
                        Try
                            If (MyReader.PeekChars(10000).Contains("***") Or
                                    Trim(MyReader.PeekChars(10000)) = "" Or
                                    Trim(MyReader.PeekChars(10000).ToLower.Contains("type of graph")) Or
                                    Trim(MyReader.PeekChars(10000).ToLower.Contains("axis for the curve")) Or
                                    Trim(MyReader.PeekChars(10000).ToLower.StartsWith(","))) Then
                                CurrentRow = MyReader.ReadFields
                            Else

                                CurrentRow = MyReader.ReadFields
                                Dim i As Integer = 0
                                For Each testtring As String In CurrentRow
                                    testtring = testtring.Replace("deg-", ChrW(186))
                                    testtring = testtring.Replace("mu-", ChrW(956))
                                    CurrentRow(i) = testtring
                                    i += 1
                                Next

                                lgraphRecordsNew.Add(CurrentRow)

                            End If


                        Catch ex As Microsoft.VisualBasic.
                                    FileIO.MalformedLineException
                            MsgBox("Line " & ex.Message &
                            "is not valid and will be skipped.")
                        End Try
                    End While
                    Logger.Dbg(lGraphSpecificationFile & " was used as the Graph Specification File")
                ElseIf (lGraphSpecificationFileNames.Count = 0) Then
                    MsgBox("The Graph specification files were not found.", vbOKOnly)
                    Exit For
                Else
                    MsgBox("The" & lGraphSpecificationFile & " file didn't exist or was blank. Reading next CSV file!", vbOKOnly)
                    Continue For
                End If

                Dim lRecordIndex As Integer = 0

                If lGraphRecordsNew.Count < 1 Then
                    MsgBox("The" & lGraphSpecificationFile & " file didn't have any useful data. Reading next CSV file!", vbOKOnly)
                    Continue For
                End If

                Dim lDBFdatasource As New atcDataSourceBasinsObsWQ
                Do
                    Dim lTimeseriesGroup As New atcTimeseriesGroup
                    Dim lGraphInit() As String = lgraphRecordsNew(lRecordIndex) 'MyReader.ReadFields 'lGraphRecords(lRecordIndex).split(",")
                    Dim TypeOfGraph As String = Trim(lGraphInit(0)).ToLower
                    If Not (TypeOfGraph = "timeseries" Or TypeOfGraph = "frequency" Or TypeOfGraph = "scatter") Then
                        MsgBox("Wrong type of graph specified. Aborting graphing from file " & lGraphSpecificationFile & " Reading next CSV file!", vbOKOnly)
                        Continue For
                    End If
                    Dim lNumberOfCurves As Integer = Trim(lGraphInit(2))
                    Dim lOutFileName As String = loutfoldername & Trim(lGraphInit(1))
                    If lNumberOfCurves < 1 Then
                        MsgBox("The " & lOutFileName & " graph in " & lGraphSpecificationFile & " file didn't have any useful data. Reading next CSV file!", vbOKOnly)
                        Continue For
                    End If
                    Logger.Dbg("Started preparing graph " & lOutFileName)
                    Dim lGraphStartDateJ, lGraphEndDateJ As Double
                    'GraphInit has information about number of datasets, their color, symbol etc.


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

                    If TypeOfGraph = "scatter" Then lNumberOfCurves = 2
                    lRecordIndex += 1
                    Dim skipGraph As Boolean = False
                    For CurveNumber As Integer = 1 To lNumberOfCurves
                        Dim lGraphDataset() As String = lgraphRecordsNew(lRecordIndex)
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
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Moving to next graph!", vbOKOnly)
                                        lRecordIndex += 1
                                        Do Until (Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("scatter") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("timeseries") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("frequency") Or _
                                                    lRecordIndex + 1 < lgraphRecordsNew.Count)
                                            lRecordIndex += 1
                                        Loop

                                        skipGraph = True
                                        Exit For
                                        'Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                        '                               " DSN " & Trim(lGraphDataset(2)) & ". Program will quit!")
                                    End If

                                Case ".hbn", ".dbf"
                                    lTimeSeries = lDataSource.DataSets.FindData("Location", Trim(lGraphDataset(2))) _
                                                                      .FindData("Constituent", Trim(lGraphDataset(3)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Moving to next graph!", vbOKOnly)
                                        lRecordIndex += 1
                                        Do Until (Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("scatter") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("timeseries") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("frequency") Or _
                                                    lRecordIndex + 1 < lgraphRecordsNew.Count)
                                            lRecordIndex += 1
                                        Loop

                                        skipGraph = True
                                        Exit For
                                        'Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                        '                                                      " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Program will quit!")
                                    End If

                                Case ".rdb"
                                    lTimeSeries = lDataSource.DataSets.FindData("ParmCode", Trim(lGraphDataset(2)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " & _
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Moving to next graph!", vbOKOnly)
                                        lRecordIndex += 1
                                        Do Until (Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("scatter") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("timeseries") Or _
                                                    Trim(lgraphRecordsNew(lRecordIndex)).ToLower.StartsWith("frequency") Or _
                                                    lRecordIndex + 1 < lgraphRecordsNew.Count)
                                            lRecordIndex += 1
                                        Loop

                                        skipGraph = True
                                        Exit For
                                        'Throw New ApplicationException("No timeseries was available from " & lDataSourceFilename & " for " & _
                                        '                                                      " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) & ". Program will quit!")
                                    End If
                            End Select


                            Dim aTu As Integer = lTimeSeries.Attributes.GetValue("TimeUnit")
                            lTimeSeries.Attributes.SetValue("YAxis", Trim(lGraphDataset(0)))

                            If (lGraphDataset.GetUpperBound(0) > 10 AndAlso Not String.IsNullOrEmpty(Trim(lGraphDataset(10)))) Then
                                lTimeSeries = AggregateTS(lTimeSeries, Trim(lGraphDataset(10)).ToLower, Trim(lGraphDataset(11)).ToLower)
                            End If
                            If (lGraphDataset.GetUpperBound(0) > 11 AndAlso Not String.IsNullOrEmpty(Trim(lGraphDataset(12)))) Then
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
                            End If

                            If (lGraphInit.GetUpperBound(0) > 17 AndAlso Not String.IsNullOrEmpty(lGraphInit(18))) Then

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
                    If Not skipGraph Then


                        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                        Select Case Trim(lGraphInit(0)).ToLower
                            Case "timeseries"
                                TimeSeriesgraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "frequency"
                                FrequencyGraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "scatter"
                                lRecordIndex += 2
                                ScatterPlotGraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "cumulative probability"
                                CumulativeProbability(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
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
                        lRecordIndex += 1
                    End If

                Loop While (lRecordIndex + 1) < lgraphRecordsNew.Count

                atcDataManager.DataSets.Clear()
                atcDataManager.Clear()
            End Using
        Next

    End Sub

    Private Function AggregateTS(ByVal aTimeseries As atcTimeseries, ByVal aTimeUnit As String, ByVal aTran As String) As atcTimeseries
        Select Case aTimeUnit
            Case "hourly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select

            Case "daily"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select

            Case "monthly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select
            Case "yearly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select
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
            lAuxPane = aZgc.MasterPane.PaneList(0)
            lPaneMain = aZgc.MasterPane.PaneList(1)
            If (aGraphInit.length > 11 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(11)))) Then
                lAuxPane.YAxis.Scale.Min = Trim(aGraphInit(11))
            End If
            If (aGraphInit.length > 12 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(12)))) Then
                lAuxPane.YAxis.Scale.Max = Trim(aGraphInit(12))
            End If
            If (aGraphInit.length > 16 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(16))) AndAlso Trim(aGraphInit(16)).ToLower = "yes") Then
                lAuxPane.YAxis.Type = AxisType.Log
            End If
            lAuxPane.YAxis.Title.Text = Trim(aGraphInit(5))
        Else
            lPaneMain = aZgc.MasterPane.PaneList(0)
        End If
        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))
        lPaneMain.Y2Axis.Title.Text = Trim(aGraphInit(6))
        lPaneMain.YAxis.Scale.Min = 0
        lPaneMain.Y2Axis.Scale.Min = 0


        If (aGraphInit.length > 9 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(9)))) Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If (aGraphInit.length > 10 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(10)))) Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        If (aGraphInit.length > 13 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(13)))) Then
            lPaneMain.Y2Axis.Scale.Min = Trim(aGraphInit(13))
        End If
        If (aGraphInit.length > 14 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(14)))) Then
            lPaneMain.Y2Axis.Scale.Max = Trim(aGraphInit(14))
        End If
        If (aGraphInit.length > 15 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(15))) AndAlso Trim(aGraphInit(15)).ToLower = "yes") Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If
        If (aGraphInit.length > 15 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(17))) AndAlso Trim(aGraphInit(17)).ToLower = "yes") Then
            lPaneMain.Y2Axis.Type = AxisType.Log
        End If

        For CurveNumber As Integer = 1 To lNumberOfCurves
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex)
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
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex)


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
