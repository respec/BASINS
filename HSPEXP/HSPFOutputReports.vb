Imports atcUtility
Imports atcData
Imports atcGraph
Imports HspfSupport
Imports atcUCI
Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports System.Collections.Specialized
Imports System.IO 'added by Becky to get directory exists function
Imports System.Xml
Imports System.Data

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
    Private pModelQAQC As Boolean = False
    Private SDateJ, EDateJ As Double
    Private loutfoldername As String
    Private MultiSimulation As Boolean = False
    Private pListModelParameters As Boolean = False
    Private ASDate, AEDate As Date
    Private pBATHTUB As Boolean = True
    Private pWASP As Boolean = True

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
        pModelQAQC = StartUp.chkModelQAQC.Checked
        pBATHTUB = StartUp.chkBathtub.Checked
        pWASP = StartUp.chkWASP.Checked


        pRunUci = StartUp.chkRunHSPF.Checked
        'pMakeAreaReports = StartUp.chkAreaReports.Checked

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
            pConstituents.Add("TN")
        End If

        If StartUp.chkTotalPhosphorus.Checked Then
            pConstituents.Add("TP")
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

        If StartUp.chkGQUAL1.Checked Then
            pConstituents.Add(StartUp.chkGQUAL1.Text & "-1")
        End If
        If StartUp.chkGQUAL2.Checked Then
            pConstituents.Add(StartUp.chkGQUAL2.Text & "-2")
        End If
        If StartUp.chkGQUAL3.Checked Then
            pConstituents.Add(StartUp.chkGQUAL3.Text & "-3")
        End If
        If StartUp.chkGQUAL4.Checked Then
            pConstituents.Add(StartUp.chkGQUAL4.Text & "-4")
        End If
        If StartUp.chkGQUAL5.Checked Then
            pConstituents.Add(StartUp.chkGQUAL5.Text & "-5")
        End If
        If StartUp.chkGQUAL6.Checked Then
            pConstituents.Add(StartUp.chkGQUAL6.Text & "-6")
        End If
        If StartUp.chkGQUAL7.Checked Then
            pConstituents.Add(StartUp.chkGQUAL7.Text & "-7")
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aMapWin"></param>
    ''' <param name="aHspfUci"></param>
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
                lOperationTypes.Add("B:", "BMPRAC")

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




#Region "Start QA/QC Report"

                Dim QAQCReportFile As New Text.StringBuilder

                'QAQCReportFile = My.Computer.FileSystem.OpenTextFileWriter(loutfoldername & "\ModelQAQCReport.htm", False)
                If pModelQAQC Then
                    QAQCReportFile.AppendLine("<html>")
                    QAQCReportFile.AppendLine("<head>")
                    QAQCReportFile.AppendLine("<style>
                                                  table, th {
                                                  border:2px solid #DEDFE0;
                                                  border-collapse:collapse;
                                                  border-top-color: #ef3e32;
                                                  border-bottom-color: #ef3e32;
                                                  font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 12px;}
                                                  td {
                                                  border:2px solid #DEDFE0;
                                                  border-collapse:collapse;
                                                  font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 12px;}
                                                    
                                                  h1 { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 24px; font-style: normal; font-variant: normal; font-weight: 700; line-height: 26.4px; color: black; text-transform: uppercase;} 
                                                  h2 { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 18px; font-style: normal; font-variant: normal; font-weight: 700; line-height: 15.4px; color: #ef3e32;text-transform: uppercase;} 
                                                  h3 { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 14px; font-style: normal; font-variant: normal; font-weight: 700; line-height: 15.4px; } 
                                                  p { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 14px; font-style: normal; font-variant: normal; font-weight: 400; line-height: 20px; } 
                                                  li { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 12px; font-style: normal; font-variant: normal; font-weight: 400; line-height: 20px; }
                                                  blockquote { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 21px; font-style: normal; font-variant: normal; font-weight: 400; line-height: 30px; } 
                                                  pre { font-family: ""Arial Narrow"", Arial, sans-serif; font-size: 13px; font-style: normal; font-variant: normal; font-weight: 400; line-height: 18.5714px; }
                                                </style>")

                    QAQCReportFile.AppendLine("</head>")
                    QAQCReportFile.AppendLine("<body>")
                    QAQCReportFile.AppendLine(GeneralModelInfo(aHspfUci, lRunMade))
                    QAQCReportFile.AppendLine(CheckHSPFParmValues(aHspfUci, lRunMade))

                    'Dim alocations As New atcCollection
                    'For Each lRCHRES As HspfOperation In aHspfUci.OpnBlks("RCHRES").Ids
                    '    Dim lDownstreamReachID As Integer = lRCHRES.DownOper("RCHRES")
                    '    If lDownstreamReachID = 0 Then
                    '        alocations.Add("R:" & lRCHRES.Id)
                    '    End If
                    'Next
                    'Dim lReport As atcReport.ReportText = AreaReport(aHspfUci, lRunMade, lOperationTypes, alocations, True, loutfoldername & "/AreaReports/")



                End If

#End Region

#Region "AutomatedGraph and Regan Graph Generation"

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

#End Region
#Region "Area Report Generation"
                'Area reporting is included in QA/QC Reports
                'If pMakeAreaReports Then
                '    Dim alocations As New atcCollection
                '    For Each lRCHRES As HspfOperation In aHspfUci.OpnBlks("RCHRES").Ids

                '        alocations.Add("R:" & lRCHRES.Id)

                '    Next
                '    Logger.Status(Now & " Producing Area Reports.", True)
                '    Logger.Dbg(Now & " Producing land use and area reports")
                '    'Now the area reports are generated for all the reaches in the UCI file.
                '    Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(aHspfUci, lRunMade, lOperationTypes, alocations, True, loutfoldername & "/AreaReports/")
                '    lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)
                '    SaveFileString(loutfoldername & "/AreaReports/AreaReport.txt", lReport.ToString)
                'End If
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
                            lStr = lExpertSystem.Report(lRunMade)

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
                If pConstituents.Count > 0 OrElse pBATHTUB Then
                    Dim lOpenHspfBinDataSource As New atcDataSource
                    Logger.Dbg(Now & " Opening the binary output files.")
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

#Region "Reading data from HBN Files for BATHTUB"
                    If pBATHTUB Then
                        If pOutputLocations.Count > 0 Then
                            Dim lBATHTUBDataSource As New atcDataSource
                            For Each location As String In pOutputLocations
                                lBATHTUBDataSource.DataSets.Add(atcDataManager.DataSets.FindData("Location", location))
                                Dim locationID As Integer = location.Substring(2)
                                Dim lRCHRESOperation As HspfOperation = aHspfUci.OpnBlks("RCHRES").OperFromID(locationID)
                                For Each lSource As HspfConnection In lRCHRESOperation.Sources
                                    If lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND" OrElse lSource.Source.VolName = "RCHRES" Then
                                        Dim lSourceOperation As String = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.VolId
                                        lBATHTUBDataSource.DataSets.Add(atcDataManager.DataSets.FindData("Location", lSourceOperation))
                                    End If
                                Next
                                BATHTUBInputFile(aHspfUci, lBATHTUBDataSource, SDateJ, EDateJ, locationID, pTestPath)
                            Next
                        Else
                        End If
                    End If
#End Region

                    For Each lConstituent As String In pConstituents
                        Dim lConstProperties As New List(Of ConstituentProperties)
                        Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")
                        Logger.Status("Begin summary for " & lConstituent)
                        Dim lConstituentName As String = ""
                        Dim lActiveSections As New List(Of String)
                        Dim CheckQUALID As Boolean = False
                        Dim lGQALID As Integer = 0
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
                            Case "TN"
                                lConstituentName = "TN"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)
                                If lConstProperties Is Nothing Then
                                    End
                                End If
                                lActiveSections.Add("NITR")
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")

                            Case "TP"
                                lConstituentName = "TP"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)
                                If lConstProperties Is Nothing Then
                                    End
                                End If
                                lActiveSections.Add("NITR")
                                lActiveSections.Add("PHOS")
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")

                            Case "BOD-Labile"
                                lConstituentName = "BOD-Labile"
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituent)
                                If lConstProperties Is Nothing Then
                                    End
                                End If
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("OXRX")
                                lActiveSections.Add("NUTRX")
                                lActiveSections.Add("PLANK")
                            Case Else
                                lGQALID = Right(lConstituent, 1)
                                lConstituentName = SafeSubstring(lConstituent, 0, lConstituent.Length - 2)
                                lConstProperties = Utility.LocateConstituentNames(aHspfUci, lConstituentName, lGQALID)
                                If lConstProperties Is Nothing Then
                                    End
                                End If
                                lActiveSections.Add("PQUAL")
                                lActiveSections.Add("IQUAL")
                                lActiveSections.Add("GQUAL")

                        End Select

                        Dim lScenarioResults As New atcDataSource

                        If lScenarioResults.DataSets.Count = 0 Then
                            For Each activeSection As String In lActiveSections
                                lScenarioResults.DataSets.Add(atcDataManager.DataSets.FindData("Section", activeSection))
                            Next
                        End If


                        If lScenarioResults.DataSets.Count > 0 Then

                            Dim lReportCons As New atcReport.ReportText
                            lReportCons = Nothing
                            Dim lOutFileName As String = ""

                            Dim LandLoadingReportForConstituents As DataTable = LandLoadingReports(loutfoldername, lScenarioResults, aHspfUci, pBaseName, lRunMade, lConstituentName, lConstProperties, SDateJ, EDateJ, lGQALID)

                            If pModelQAQC Then
                                QAQCReportFile.AppendLine("<h2>" & lConstituent & " Loading Rate Analysis</h2>")
                                QAQCReportFile.AppendLine(LoadingRateComparison(lConstituentName, LandLoadingReportForConstituents, lDateString))
                            End If

                            ReachBudgetReports(loutfoldername, lScenarioResults, aHspfUci, pBaseName, lRunMade, lConstituentName, lConstProperties, SDateJ, EDateJ, lGQALID)
                            If pModelQAQC Then
                                QAQCReportFile.AppendLine(VerifyStorageTrend(aHspfUci, lScenarioResults, lConstituentName))
                            End If

                            Logger.Status(Now & " Generating Reports for " & lConstituent)
                            Logger.Dbg(Now & " Generating Reports for " & lConstituent)
                            lReportCons = Nothing

                            If lConstituent = "TN" OrElse lConstituent = "TP" OrElse
                                lConstituent = "Sediment" OrElse lConstituent = "Water" Then

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
                If pModelQAQC Then
                    QAQCReportFile.AppendLine(ModelAreaReport(aHspfUci, lOperationTypes))
                    QAQCReportFile.AppendLine("</body>")
                    QAQCReportFile.AppendLine("</html>")
                    File.WriteAllText(pTestPath & "\ModelQAQC.htm", QAQCReportFile.ToString())

                End If
                Logger.Status(Now & " Output Written to " & loutfoldername)
                Logger.Dbg("Reports Written in " & loutfoldername)
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
    End Sub
    ''' <summary>
    ''' This function looks at each parameter limit in the XML list of parameters and compares them to the values in the UCI file. If the values are not within the 
    ''' limits, it is mentioned in the report.
    ''' </summary>
    ''' <param name="aUCI"></param>
    ''' <param name="aRunMade"></param>
    ''' <returns></returns>
    Private Function CheckHSPFParmValues(ByVal aUCI As HspfUci, ByVal aRunMade As String) As String ' , ByVal ParameterValues As DataTable)
        Dim HSPFParmTable As XmlDocument = New XmlDocument()
        Dim TableName As String = ""
        Dim ParameterName As String
        Dim MaxValue As Double = 0
        Dim MinValue As Double = 0
        Dim OperationType As String = ""
        Dim ParameterInfo As New Text.StringBuilder
        Dim lTotalParmIssues As Integer = 0
        HSPFParmTable.LoadXml(My.Resources.HSPFParmValues) '    
        ParameterInfo.AppendLine("<h2>Model Parameter Value Analysis</h2>")

        Dim nodes As XmlNodeList = HSPFParmTable.DocumentElement.SelectNodes("Parm")
        Dim lParameterViolationTable As DataTable
        lParameterViolationTable = New DataTable("ModelParameterInfo")
        Dim lColumn As DataColumn

        lColumn = New DataColumn()
        lColumn.ColumnName = "OPN_TYPE"
        lColumn.Caption = "Operation Type"
        lColumn.DataType = Type.GetType("System.String")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "OPN_NUM"
        lColumn.Caption = "Operation Number"
        lColumn.DataType = Type.GetType("System.Int16")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "OPN_INFO"
        lColumn.Caption = "Operation Information"
        lColumn.DataType = Type.GetType("System.String")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "PARM_Table"
        lColumn.Caption = "Parameter Table"
        lColumn.DataType = Type.GetType("System.String")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "PARM_Name"
        lColumn.Caption = "Parameter Name"
        lColumn.DataType = Type.GetType("System.String")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "ConstName"
        lColumn.Caption = "Comment/Name"
        lColumn.DataType = Type.GetType("System.String")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()

        lColumn.ColumnName = "ModelValue"
        lColumn.Caption = "Model Value"
        lColumn.DataType = Type.GetType("System.Double")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "TypicalMin"
        lColumn.Caption = "Typical Min"
        lColumn.DataType = Type.GetType("System.Double")
        lParameterViolationTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "TypicalMax"
        lColumn.Caption = "Typical Max"
        lColumn.DataType = Type.GetType("System.Double")
        lParameterViolationTable.Columns.Add(lColumn)

        Dim lrow As DataRow

        For Each node As XmlNode In nodes
            OperationType = node.SelectSingleNode("OPNTYPE").InnerText
            TableName = node.SelectSingleNode("TABLE").InnerText
            ParameterName = node.SelectSingleNode("ParameterName").InnerText
            'If ParameterName = "TAUCS" Then Stop
            Dim IsMonthlyValuePossible As Integer
            Try
                IsMonthlyValuePossible = CInt(node.SelectSingleNode("IsMonthlyPossible").InnerText)
            Catch
                IsMonthlyValuePossible = 0
            End Try
            Dim lActivityFlag As String = ""

            lActivityFlag = node.SelectSingleNode("ACTIVITYFLAG").InnerText

            If lActivityFlag.Length = 0 Then Continue For

            Dim MonthlyFlagTable As String = node.SelectSingleNode("FlagTable").InnerText
            Dim MonthlyFlagParm As String = node.SelectSingleNode("FlagParm").InnerText
            Dim MonthlyParmTable As String = node.SelectSingleNode("MonthlyParmTable").InnerText
            Dim lQualConstituentName As String = ""
            Logger.Dbg("Looking at Table " & TableName & "Parameter " & ParameterName)
            lQualConstituentName = node.SelectSingleNode("ConstituentName").InnerText
            Dim lQUALID As Integer = 0
            If lQualConstituentName.Length > 0 Then
                Select Case lQualConstituentName 'Can look for QUALID fronm the UCI file also.
                    Case "NH3+NH4"
                        lQUALID = 1
                    Case "NO3"
                        lQUALID = 2
                    Case "ORTHO P"
                        lQUALID = 3
                    Case "BOD"
                        lQUALID = 4
                End Select
            End If

            Try
                MaxValue = CDbl(node.SelectSingleNode("Max").InnerText)
                MinValue = CDbl(node.SelectSingleNode("Min").InnerText)
            Catch
                Continue For
            End Try

            Dim lMessageCountPerParameter As Integer = 0
            For Each loperation As HspfOperation In aUCI.OpnBlks(OperationType).Ids
                'Looping through each operation for a specific parameter 
                Dim lActivityFlagValue As Integer = 0
                lActivityFlagValue = loperation.Tables("ACTIVITY").Parms(lActivityFlag).Value
                If lActivityFlagValue = 0 Then Continue For
                Dim lMonthlyFlagValue As Integer = 0
                If IsMonthlyValuePossible = 1 Then
                    Try
                        lMonthlyFlagValue = loperation.Tables(MonthlyFlagTable).Parms(MonthlyFlagParm).Value
                    Catch
                        lMonthlyFlagValue = 0
                    End Try
                End If
                Try
                    If lQUALID = 0 Then
                        If lMonthlyFlagValue = 0 Then
                            Dim lParmValue As Double = loperation.Tables(TableName).Parms(ParameterName).Value
                            Dim lComment As String = ""
                            'If ParameterName = "TAUCD" Then Stop
                            If ParameterName = "TAUCD" AndAlso loperation.Tables(TableName).Parms("TAUCS").Value < lParmValue Then
                                lComment = "TAUCD is greater than TAUCS."
                            End If
                            If (ParameterName = "TAUCS" OrElse ParameterName = "TAUCD") AndAlso TableName = "SILT-CLAY-PM" AndAlso
                                    loperation.Tables("SILT-CLAY-PM:2").Parms(ParameterName).Value > lParmValue Then
                                lComment &= ParameterName & " for clay is greater than " & ParameterName & " for silt."
                            End If
                            If lParmValue > MaxValue OrElse lParmValue < MinValue OrElse lComment.Length > 0 Then
                                lrow = lParameterViolationTable.NewRow
                                lrow("OPN_TYPE") = OperationType
                                lrow("OPN_NUM") = loperation.Id
                                lrow("OPN_INFO") = loperation.Description
                                lrow("PARM_TABLE") = TableName
                                lrow("PARM_NAME") = ParameterName
                                lrow("ConstName") = lComment
                                lrow("ModelValue") = lParmValue
                                lrow("TypicalMin") = MinValue
                                lrow("TypicalMax") = MaxValue
                                lParameterViolationTable.Rows.Add(lrow)
                            End If
                        Else
                            If MonthlyParmTable = "MON-LGTP2" AndAlso loperation.Tables("PSTEMP-PARM1").Parms("TSOPFG").Value = 1 Then
                                Exit For
                            End If

                            For Each MonthlyParm As HspfParm In loperation.Tables(MonthlyParmTable).Parms
                                If MonthlyParm.Value < MinValue OrElse MonthlyParm.Value > MaxValue Then
                                    lrow = lParameterViolationTable.NewRow
                                    lrow("OPN_TYPE") = OperationType
                                    lrow("OPN_NUM") = loperation.Id
                                    lrow("OPN_INFO") = loperation.Description
                                    lrow("PARM_TABLE") = MonthlyParmTable
                                    lrow("PARM_NAME") = MonthlyParm.Name
                                    lrow("ConstName") = ""
                                    lrow("ModelValue") = MonthlyParm.Value
                                    lrow("TypicalMin") = MinValue
                                    lrow("TypicalMax") = MaxValue
                                    lParameterViolationTable.Rows.Add(lrow)
                                End If
                            Next

                        End If

                    Else
                        Dim lTempTABLEName As String = TableName
                        If lQUALID > 1 Then lTempTABLEName = TableName & ":" & lQUALID
                        If lMonthlyFlagValue = 0 Then
                            Dim lParmValue As Double = loperation.Tables(lTempTABLEName).Parms(ParameterName).Value
                            If lParmValue > MaxValue OrElse lParmValue < MinValue Then
                                lrow = lParameterViolationTable.NewRow
                                lrow("OPN_TYPE") = OperationType
                                lrow("OPN_NUM") = loperation.Id
                                lrow("OPN_INFO") = loperation.Description
                                lrow("PARM_TABLE") = lTempTABLEName
                                lrow("PARM_NAME") = ParameterName
                                lrow("ConstName") = lQualConstituentName
                                lrow("ModelValue") = lParmValue
                                lrow("TypicalMin") = MinValue
                                lrow("TypicalMax") = MaxValue
                                lParameterViolationTable.Rows.Add(lrow)
                            End If
                        Else
                            Dim lTempMonthlyTable As String = MonthlyParmTable
                            If lQUALID > 1 Then lTempMonthlyTable = MonthlyParmTable & ":" & lQUALID
                            For Each MonthlyParm As HspfParm In loperation.Tables(lTempMonthlyTable).Parms
                                If MonthlyParm.Value < MinValue OrElse MonthlyParm.Value > MaxValue Then
                                    'lMonthlyParmCount += 1
                                    lrow = lParameterViolationTable.NewRow
                                    lrow("OPN_TYPE") = OperationType
                                    lrow("OPN_NUM") = loperation.Id
                                    lrow("OPN_INFO") = loperation.Description
                                    lrow("PARM_TABLE") = lTempMonthlyTable
                                    lrow("PARM_NAME") = MonthlyParm.Name
                                    lrow("ConstName") = lQualConstituentName
                                    lrow("ModelValue") = MonthlyParm.Value
                                    lrow("TypicalMin") = MinValue
                                    lrow("TypicalMax") = MaxValue
                                    lParameterViolationTable.Rows.Add(lrow)
                                End If
                            Next

                        End If

                    End If
                Catch ex As Exception

                End Try
            Next loperation

        Next
        Dim lReachesWithAdsDepoIssue As Integer = 0
        For Each lRCHRES As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
            If lRCHRES.Tables("ACTIVITY").Parms("NUTFG").Value = 1 AndAlso (lRCHRES.Tables("NUT-FLAGS").Parms("ADNHFG").Value = 0 OrElse
                lRCHRES.Tables("NUT-FLAGS").Parms("ADPOFG").Value = 0) Then
                lReachesWithAdsDepoIssue += 1
                If lReachesWithAdsDepoIssue > 1 Then
                    ParameterInfo.AppendLine("<p>At least one reach simulates nutrients but the adsoprtion and desorption from bed sediment is not simulated.</p>")
                    Exit For
                End If
            End If
        Next

        If lParameterViolationTable.Rows.Count = 0 AndAlso lReachesWithAdsDepoIssue = 0 Then

            ParameterInfo.AppendLine("<p>All the tested parameters are within the typical range.</p>")

        End If
        If lParameterViolationTable.Rows.Count > 0 And lParameterViolationTable.Rows.Count <= 10 Then

            ParameterInfo.AppendLine("<p>Following parameters were beyond the typical range.")
            ParameterInfo.Append(ConvertToHtmlFile(lParameterViolationTable))

        End If
        If lParameterViolationTable.Rows.Count > 10 Then
            Dim lXSLFile As StreamWriter = File.CreateText(Path.Combine(pTestPath, "ParameterTableTemplate.xsl"))
            lXSLFile.Write(My.Resources.LimitViolatingParameterTemplate)
            lXSLFile.Close()
            Dim lParameterInfo As StreamWriter = File.CreateText(Path.Combine(pTestPath, "ParameterReport.xml"))
            lParameterInfo.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
            lParameterInfo.WriteLine("<?xml-stylesheet version=""1.0"" type=""text/xsl"" href=""ParameterTableTemplate.xsl""?>")
            lParameterViolationTable.WriteXml(lParameterInfo, XmlWriteMode.IgnoreSchema)
            lParameterInfo.Close()

            ParameterInfo.AppendLine("<p> More than 10 cases were found where the model parameter values were outside the typical range. First 10 cases are listed below and all the 
                                        range violations are listed in a <a href=./ParameterReport.xml>separate xml table.</a></p>")

            ParameterInfo.Append(ConvertToHtmlFile(lParameterViolationTable, 10))
        End If

        Return ParameterInfo.ToString
    End Function
    ''' <summary>
    ''' This function outputs heading of the QA/QC report and some general information.
    ''' </summary>
    ''' <param name="aUCI"></param>
    ''' <returns></returns>
    Private Function GeneralModelInfo(ByVal aUCI As HspfUci, ByVal aRunMade As String) As String
        Dim GeneralModelInfoText As New Text.StringBuilder
        GeneralModelInfoText.AppendLine("<h1>HSPF Model QA QC Report</h1>")
        GeneralModelInfoText.AppendLine("<h2>Disclaimer</h2>")
        GeneralModelInfoText.AppendLine("<p>The QA/QC module assumes that the model is run in English Units and the units of nutrients 
                                        are in lbs. This report provides a set of checks for HSPF hydrology and water quality models 
                                        that are based on generalized professional judgment.  Unique settings and conditions can 
                                        lead to hydrologic and water quality complexities that do not conform to the judgments that 
                                        are embedded in this module.  Furthermore, the set of aspects for which checks have been 
                                        included is not all-inclusive.</p>")
        GeneralModelInfoText.AppendLine("<p>This module also assumes that first four water quality constituents simulated on the land are  
                                        ammonia (NH3+NH4), nitrate as nitrogen (NO3), orthophosphorus as phosphorus (ORTHO P), and biochemical 
                                        oxygen demand (BOD) in the order that they are listed here with the QUALID as it is listed in the parenthesis.</p>")
        GeneralModelInfoText.AppendLine("<h2>How to use the QA/QC Report</h2>")
        GeneralModelInfoText.AppendLine("<p>The sequence of messages that are contained in the QA/QC Report for any UCI and its 
                                        resulting output will flag aspects of the model input and output that are considered 
                                        nontypical, thereby providing the modeler with a checklist of items that warrant either
                                        re-affirmation or modification, either by refining aspects of the model input or 
                                        undertaking additional calibration. </p>")
        GeneralModelInfoText.AppendLine("<p>Iterative use of the QA/QC module should result in decreasing the number of instances 
                                        of nontypical designations or results.  The aspects that remain flagged in a model run 
                                        that is considered a final calibration will document nuances of the model that should be 
                                        explained in the modeling application report. </p>")
        GeneralModelInfoText.AppendLine("<h2>General Model Information</h2>")
        GeneralModelInfoText.AppendLine("<table>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <th align=left>Model File Name</th>")
        GeneralModelInfoText.AppendLine("    <th align=center>" & aUCI.Name & "</th>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <td>Model Span</td>")
        GeneralModelInfoText.AppendLine("    <td align=center>" & aUCI.GlobalBlock.SDate(0) & "/" & aUCI.GlobalBlock.SDate(1) & "/" & aUCI.GlobalBlock.SDate(2) & " - " &
                                         aUCI.GlobalBlock.EDate(0) & "/" & aUCI.GlobalBlock.EDate(1) & "/" & aUCI.GlobalBlock.EDate(2) & "</td>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <td>Model Last Run time </td>")
        GeneralModelInfoText.AppendLine("    <td align=center>" & aRunMade & "</td>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <td>QA QC Report generated on </td>")
        GeneralModelInfoText.AppendLine("    <td align=center>" & DateTime.Now & "</td>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <td>HSPEXP+ Version </td>")
        GeneralModelInfoText.AppendLine("    <td align=center>3.0 beta</td>")
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("  <tr>")
        GeneralModelInfoText.AppendLine("    <td>Sections listed in this report</td>")
        Dim QAQCAnalysis As String = "    <td align=center>Parameter Values"
        If pConstituents.Contains("Water") Then
            QAQCAnalysis &= ", Water"
        End If
        If pConstituents.Contains("Sediment") Then
            QAQCAnalysis &= ", Sediment"
        End If
        If pConstituents.Contains("TN") Then
            QAQCAnalysis &= ", Total Nitrogen"
        End If
        If pConstituents.Contains("TP") Then
            QAQCAnalysis &= ", Total Phosphorus"
        End If
        If pConstituents.Contains("BOD-Labile") Then
            QAQCAnalysis &= ", BOD-Labile"
        End If
        QAQCAnalysis &= "</td>"
        GeneralModelInfoText.AppendLine(QAQCAnalysis)
        GeneralModelInfoText.AppendLine("  </tr>")
        GeneralModelInfoText.AppendLine("</table>")
        Return GeneralModelInfoText.ToString
    End Function

    ''' <summary>
    ''' This function outputs area report of terminal reaches and model calibration reaches.
    ''' </summary>
    ''' <param name="aUCI"></param>
    ''' <param name="aOperationTypes"></param>
    ''' <returns></returns>
    Private Function ModelAreaReport(ByVal aUCI As HspfUci, ByVal aOperationTypes As atcCollection) As String
        Dim ModelAreaReportTable As String = ""
        Dim lOutletLocations As New atcCollection
        Dim lCalibrationLocations As New atcCollection
        Dim lLocationsToOutput As New atcCollection

        'First add calibration reaches by looking at contents of expert system .exs files
        Dim lExpertSystemFileNames As New NameValueCollection
        AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
        Dim lExpertSystem As HspfSupport.atcExpertSystem
        For Each lExpertSystemFileName As String In lExpertSystemFileNames
            Try
                lExpertSystem = New HspfSupport.atcExpertSystem(aUCI, lExpertSystemFileName, SDateJ, EDateJ)
                For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                    Dim lSiteNam As String = lExpertSystem.Sites(lSiteIndex - 1).Name
                    If lSiteNam.StartsWith("RCH:") Then
                        If IsInteger(lSiteNam.Substring(4)) Then
                            lSiteNam = "R:" & lSiteNam.Substring(4)
                            lCalibrationLocations.Add(lSiteNam)
                            lLocationsToOutput.Add(lSiteNam)
                        End If
                    ElseIf lSiteNam.StartsWith("RCH") Then
                        If IsInteger(lSiteNam.Substring(3)) Then
                            lSiteNam = "R:" & lSiteNam.Substring(3)
                            lCalibrationLocations.Add(lSiteNam)
                            lLocationsToOutput.Add(lSiteNam)
                        End If
                    ElseIf lSiteNam.StartsWith("R:") Then
                        If IsInteger(lSiteNam.Substring(2)) Then
                            lSiteNam = "R:" & lSiteNam.Substring(2)
                            lCalibrationLocations.Add(lSiteNam)
                            lLocationsToOutput.Add(lSiteNam)
                        End If
                    ElseIf lSiteNam.StartsWith("R") Then
                        If IsInteger(lSiteNam.Substring(1)) Then
                            lSiteNam = "R:" & lSiteNam.Substring(1)
                            lCalibrationLocations.Add(lSiteNam)
                            lLocationsToOutput.Add(lSiteNam)
                        End If
                    End If
                Next
            Catch
            End Try
        Next

        'Then add outlet reaches
        For Each lRCHRES As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
            Dim lDownstreamReachID As Integer = lRCHRES.DownOper("RCHRES")
            If lDownstreamReachID = 0 Then
                lOutletLocations.Add("R:" & lRCHRES.Id)
                lLocationsToOutput.Add("R:" & lRCHRES.Id)
            End If
        Next

        Dim AreaInfo As New Text.StringBuilder
        AreaInfo.AppendLine("<h2>Model Area Table</h2>")
        If lCalibrationLocations.Count > 0 Then
            AreaInfo.AppendLine("<p>" & lCalibrationLocations.Count & " Calibration Locations and " & lOutletLocations.Count & " Outlet Locations</p>")
        Else
            If lOutletLocations.Count = 1 Then
                AreaInfo.AppendLine("<p>One Outlet Location</p>")
            Else
                AreaInfo.AppendLine("<p>" & lOutletLocations.Count & " Outlet Locations</p>")
            End If
        End If

        'lLocationsToOutput.Add("R:1")   'for debug

        For Each lLocation In lLocationsToOutput
            AreaInfo.AppendLine("<p>")
            If lCalibrationLocations.Contains(lLocation) Then
                AreaInfo.AppendLine("<p>Calibration Location " & lLocation)
            Else
                AreaInfo.AppendLine("<p>Outlet Location " & lLocation)
            End If

            Dim lAreaTable As DataTable = AreaReportInTableFormat(aUCI, aOperationTypes, lLocation)

            AreaInfo.Append(ConvertToHtmlFile(lAreaTable))
        Next

        ModelAreaReportTable = AreaInfo.ToString

        Return ModelAreaReportTable
    End Function

    Private Function LoadingRateComparison(ByVal aConstituentName As String, ByVal aLandLoadingConstReport As DataTable,
                                           ByVal aDateString As String) As String
        Dim OverAllComments As New Text.StringBuilder
        Dim LoadingRateComments As New Text.StringBuilder
        Dim newColumn As DataColumn
        newColumn = New DataColumn()
        newColumn.DataType = Type.GetType("System.String")
        newColumn.ColumnName = "genLandUse"
        newColumn.Caption = "Generalized Land Use"
        aLandLoadingConstReport.Columns.Add(newColumn)
        Dim UCILandUse As String = ""
        Dim ListofLandUsesInUCI As New List(Of String)
        Dim OpTypeNumber As String = ""
        For Each row As DataRow In aLandLoadingConstReport.Rows
            UCILandUse = row("OpDesc")
            OpTypeNumber = row("OpTypeNumber")
            If OpTypeNumber.StartsWith("I:") Then
                row("genLandUse") = "Impervious"
                If Not ListofLandUsesInUCI.Contains(row("genLandUse")) And Not row("genLandUse") = "Unknown" Then
                    ListofLandUsesInUCI.Add(row("genLandUse"))
                End If
                Continue For
            End If
            row("genLandUse") = FindGeneralLandUse(UCILandUse)

            If Not ListofLandUsesInUCI.Contains(row("genLandUse")) And Not row("genLandUse") = "Unknown" Then
                ListofLandUsesInUCI.Add(row("genLandUse"))
            End If
        Next
        Dim lSelectExpression As String = ""

        'LoadingRateComments.AppendLine("<ul>")
        Dim IsWetlandALanduse As Boolean = False
        If ListofLandUsesInUCI.Contains("Wetland") Then IsWetlandALanduse = True

        For Each landuse As String In ListofLandUsesInUCI
            Dim TextFromFunction As String = String.Empty
            Select Case aConstituentName
                Case "WAT"
                    TextFromFunction = CheckIrrigation(landuse, aLandLoadingConstReport)
                    TextFromFunction &= CheckETIssues(landuse, aLandLoadingConstReport, ListofLandUsesInUCI, IsWetlandALanduse)
                    TextFromFunction &= CheckRunoff(landuse, aLandLoadingConstReport, ListofLandUsesInUCI)
                Case "TN"
                    TextFromFunction &= CheckNutrientLoading(landuse, "NO3", ListofLandUsesInUCI, aLandLoadingConstReport)
                    TextFromFunction &= CheckNutrientLoading(landuse, "TAM", ListofLandUsesInUCI, aLandLoadingConstReport)
                    TextFromFunction &= CheckNutrientLoading(landuse, "TN", ListofLandUsesInUCI, aLandLoadingConstReport)
                Case "TP"
                    TextFromFunction &= CheckNutrientLoading(landuse, "PO4", ListofLandUsesInUCI, aLandLoadingConstReport)
                    TextFromFunction &= CheckNutrientLoading(landuse, "TP", ListofLandUsesInUCI, aLandLoadingConstReport)
                Case "BOD-Labile"
                    TextFromFunction &= CheckNutrientLoading(landuse, "BOD-Labile", ListofLandUsesInUCI, aLandLoadingConstReport)
                Case "SED"
                    TextFromFunction &= CheckTotalSedimentErosion(landuse, ListofLandUsesInUCI, aLandLoadingConstReport)

            End Select
            If TextFromFunction.Length > 0 Then
                LoadingRateComments.AppendLine(TextFromFunction)
            End If
        Next


        If aConstituentName = "BOD-Labile" Then
            OverAllComments.AppendLine("<p>BOD-Labile only includes labile fraction of total organic that enters the RCHRES as OXIF 2 Member of Group INFLOW.</p>")
        End If

        If LoadingRateComments.Length > 0 Then
            OverAllComments.AppendLine("<h3>Non-typical behaviors that were noticed in the model.</h3>")
            OverAllComments.AppendLine("<ul>")
            OverAllComments.Append(LoadingRateComments)
            OverAllComments.AppendLine("</ul>")
        End If

        Select Case aConstituentName
            Case "WAT"
                OverAllComments.AppendLine("<p>Refer to the following Box-Whisker plot for more details on annual runoff from each land use.</p>")
                OverAllComments.AppendLine("<img src=""Reports_" & aDateString & "/WAT_BoxWhisker.png"" alt=""Annual runoff from each land use."" height=""400"" width=""600""></img>")
            Case "SED"
                OverAllComments.AppendLine("<p>Refer to the following Box-Whisker plot for more details on sediment loading rate from each land use.</p>")
                OverAllComments.AppendLine("<img src=""Reports_" & aDateString & "/SED_BoxWhisker.png"" alt=""Sediment loading from each land use."" height=""400"" width=""600""></img>")
            Case "TN"
                OverAllComments.AppendLine("<p>Refer to the following Box-Whisker plot for more details on total nitrogen loading rate from each land use.</p>")
                OverAllComments.AppendLine("<img src=""Reports_" & aDateString & "/TN_BoxWhisker.png"" alt=""Total nitrogen loading from each land use."" height=""400"" width=""600""></img>")
            Case "TP"
                OverAllComments.AppendLine("<p>Refer to the following Box-Whisker plot for more details on total phosphorus loading rate from each land use.</p>")
                OverAllComments.AppendLine("<img src=""Reports_" & aDateString & "/TP_BoxWhisker.png"" alt=""Total phosphorus loading from each land use."" height=""400"" width=""600""></img>")
            Case "BOD-Labile"
                OverAllComments.AppendLine("<p>Refer to the following Box-Whisker plot for more details on biochemical oxygen demand (labile only) loading rate from each land use.</p>")
                OverAllComments.AppendLine("<img src=""Reports_" & aDateString & "/BOD-Labile_BoxWhisker.png"" alt=""Total BOD-Labile loading from each land use."" height=""400"" width=""600""></img>")
        End Select



        Return OverAllComments.ToString
    End Function
    ''' <summary>
    ''' This Function takes the UCI Landuse and finds a corresponding general land use based on a CSV file.
    ''' </summary>
    ''' <param name="aUCILandUse"></param>
    ''' <returns></returns>
    Private Function FindGeneralLandUse(ByVal aUCILandUse As String) As String
        Dim GeneralLandUse As String = "Unknown"

        If aUCILandUse.ToLower.Contains("forest") Then
            GeneralLandUse = "Forest"
            Return GeneralLandUse
        ElseIf aUCILandUse.ToLower.Contains("crop") OrElse aUCILandUse.ToLower.Contains("agric") Then
            GeneralLandUse = "Ag/Other"
            Return GeneralLandUse
        ElseIf aUCILandUse.ToLower.Contains("urban") OrElse aUCILandUse.ToLower.Contains("develop") Then
            GeneralLandUse = "Urban"
            Return GeneralLandUse
        End If

        Dim LanduseMappings As XmlDocument = New XmlDocument
        LanduseMappings.LoadXml(My.Resources.LandUseNames_Mappings)
        Dim nodes As XmlNodeList = LanduseMappings.DocumentElement.SelectNodes("landusename")
        For Each node As XmlNode In nodes
            If node.SelectSingleNode("UCILandUse").InnerText = aUCILandUse Then
                Return node.SelectSingleNode("GeneralLandUse").InnerText
            End If
        Next

        Return GeneralLandUse
    End Function
    Private Function CheckIrrigation(ByVal aLanduse As String, ByVal aLandLoadingConstReport As DataTable) As String
        If aLanduse = "Ag/Other" OrElse aLanduse = "Impervious" Then Return String.Empty
        Dim IrrigationStatement As String = String.Empty
        Dim IrrigationApp As Double = 0.0
        Dim lSelectExpression As String = "genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
        Try
            IrrigationApp = aLandLoadingConstReport.Compute("AVG(IRRAPP6)", lSelectExpression)
        Catch

        End Try
        If IrrigationApp > 0 Then
            Return "<li>" & aLanduse & " land use has irrigation application.</li>"
        Else
            Return String.Empty
        End If

    End Function

    ''' <summary>
    ''' This function checks if the simulated runoff among land uses is inconsistent
    ''' </summary>
    ''' <param name="aLanduse"></param>
    ''' <param name="aLandLoadingConstReport"></param>
    ''' <param name="aListofLandUsesinUCI"></param>
    ''' <returns></returns>
    Private Function CheckRunoff(ByVal aLanduse As String, ByVal aLandLoadingConstReport As DataTable, ByVal aListofLandUsesinUCI As List(Of String)) As String
        Dim lSelectExpression As String = "genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
        Dim TotalOutFlow As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression)
        Dim TotalSurfaceRunoff As Double = aLandLoadingConstReport.Compute("AVG(SURO)", lSelectExpression)
        Dim CheckRunoffStatement As New Text.StringBuilder
        If aLanduse = "Impervious" Then
            For Each landuse2 As String In aListofLandUsesinUCI
                If landuse2 = "Impervious" Then Continue For
                Dim SelectExpression2 As String = "genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                Dim TotalOutflow2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", SelectExpression2)
                If TotalOutflow2 > TotalOutFlow Then
                    CheckRunoffStatement.AppendLine("<li>" & landuse2 & " has greater runoff than Impervious area.</li>")
                End If
            Next
        Else
            Dim TotalBaseFlow As Double = aLandLoadingConstReport.Compute("AVG(IFWO)", lSelectExpression) + aLandLoadingConstReport.Compute("AVG(AGWO)", lSelectExpression)
            Dim TotalLZET As Double = aLandLoadingConstReport.Compute("AVG(LZET)", lSelectExpression) + aLandLoadingConstReport.Compute("AVG(AGWO)", lSelectExpression)
            Dim TotalUZET As Double = aLandLoadingConstReport.Compute("AVG(UZET)", lSelectExpression) + aLandLoadingConstReport.Compute("AVG(AGWO)", lSelectExpression)
            Dim TotalAGWET As Double = aLandLoadingConstReport.Compute("AVG(AGWET)", lSelectExpression) + aLandLoadingConstReport.Compute("AVG(AGWO)", lSelectExpression)

            If TotalSurfaceRunoff > TotalBaseFlow Then
                CheckRunoffStatement.AppendLine("<li>Surface runoff is greater than baseflow for " & aLanduse & ".</li>")
            End If

            Select Case aLanduse
                Case "Forest"
                    For Each landuse2 As String In aListofLandUsesinUCI
                        If landuse2 = "Forest" OrElse landuse2 = "Impervious" Then Continue For
                        Dim SelectExpression2 As String = "genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                        Dim TotalOutflow2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", SelectExpression2)
                        Dim TotalSurfaceRunoff2 As Double = aLandLoadingConstReport.Compute("AVG(SURO)", SelectExpression2)
                        Dim TotalBaseFlow2 As Double = aLandLoadingConstReport.Compute("AVG(IFWO)", SelectExpression2) + aLandLoadingConstReport.Compute("AVG(AGWO)", SelectExpression2)
                        Select Case True
                            Case landuse2 = "Wetland" AndAlso TotalOutflow2 > TotalOutFlow
                                CheckRunoffStatement.AppendLine("<li>Wetland has greater total outflow than Forest.</li>")
                            Case landuse2 <> "Wetland" AndAlso TotalOutflow2 < TotalOutFlow
                                CheckRunoffStatement.AppendLine("<li>Forest has greater total outflow than " & landuse2 & ".</li>")
                            Case landuse2 = "Wetland" AndAlso TotalSurfaceRunoff2 > TotalSurfaceRunoff
                                CheckRunoffStatement.AppendLine("<li>Wetland has greater surface runoff than Forest.</li>")
                            Case landuse2 <> "Wetland" AndAlso TotalSurfaceRunoff2 < TotalSurfaceRunoff
                                CheckRunoffStatement.AppendLine("<li>Forest has greater surface runoff than " & landuse2 & ".</li>")
                        End Select
                    Next

                Case "Wetland"
                    For Each landuse2 As String In aListofLandUsesinUCI
                        If landuse2 = "Forest" OrElse landuse2 = "Wetland" OrElse landuse2 = "Impervious" Then Continue For
                        Dim SelectExpression2 As String = "genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                        Dim TotalOutflow2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", SelectExpression2)
                        Dim TotalSurfaceRunoff2 As Double = aLandLoadingConstReport.Compute("AVG(SURO)", SelectExpression2)
                        Dim TotalBaseFlow2 As Double = aLandLoadingConstReport.Compute("AVG(IFWO)", SelectExpression2) + aLandLoadingConstReport.Compute("AVG(AGWO)", SelectExpression2)
                        If TotalOutFlow > TotalOutflow2 Then
                            CheckRunoffStatement.AppendLine("<li>Wetland has greater total outflow than " & landuse2 & ".</li>")
                        End If
                    Next

            End Select
        End If



        Return CheckRunoffStatement.ToString
    End Function

    ''' <summary>
    ''' This function checks if simulated ET is inconsistent among the land uses.
    ''' </summary>
    ''' <param name="aLanduse"></param>
    ''' <param name="aLandLoadingConstReport"></param>
    ''' <param name="aListofLandUsesinUCI"></param>
    ''' <param name="WetlandLUExists"></param>
    ''' <returns></returns>
    Private Function CheckETIssues(ByVal aLanduse As String, ByVal aLandLoadingConstReport As DataTable,
                                   ByVal aListofLandUsesinUCI As List(Of String),
                                   ByVal WetlandLUExists As Boolean) As String
        Dim CheckETIssuesStatement As New Text.StringBuilder
        Dim lSelectExpression As String = ""
        Dim TotalET As Double = 0
        If aLanduse = "Impervious" Then
            lSelectExpression = "genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
            TotalET = aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression)
            For Each landuse2 As String In aListofLandUsesinUCI
                If landuse2 = "Impervious" Then Continue For
                Dim lSelectExpression2 As String = "genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                If TotalET > aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression2) Then
                    CheckETIssuesStatement.AppendLine("<li>Impervious areas have greater ET loss than " & landuse2 & ".</li>")
                End If
            Next

        Else
            lSelectExpression = "OpTypeNumber Like 'P:%' And genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
            TotalET = aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression)
            Dim PotET As Double = aLandLoadingConstReport.Compute("AVG(PET)", lSelectExpression)

            Dim InterceptionET As Double = aLandLoadingConstReport.Compute("AVG(CEPE)", lSelectExpression)
            Dim UpperZoneET As Double = aLandLoadingConstReport.Compute("AVG(UZET)", lSelectExpression)
            Dim LowerZoneET As Double = aLandLoadingConstReport.Compute("AVG(LZET)", lSelectExpression)
            Dim GroundWaterET As Double = aLandLoadingConstReport.Compute("AVG(AGWET)", lSelectExpression)
            Dim BaseflowET As Double = aLandLoadingConstReport.Compute("AVG(BASET)", lSelectExpression)

            If aLanduse <> "Wetland" AndAlso WetlandLUExists AndAlso GroundWaterET > 0 Then
                CheckETIssuesStatement.AppendLine("<li>Groundwater is being lost through evapotranspiration in " & aLanduse & " even though there is a separate Wetland land use.</li>")
            End If

            If UpperZoneET > LowerZoneET Then
                CheckETIssuesStatement.AppendLine("<li>Evaporation from upper zone is greater than evaporation from lower zone for " & aLanduse & ".</li>")
            End If


            Select Case aLanduse

                Case "Forest"
                    For Each landuse2 As String In aListofLandUsesinUCI
                        If landuse2 = "Forest" OrElse landuse2 = "Impervious" Then Continue For
                        Dim lSelectExpression2 As String = "OpTypeNumber Like 'P:%' And genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                        'Comparing Total ET
                        If landuse2 = "Wetland" AndAlso TotalET > aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression2) Then
                            CheckETIssuesStatement.AppendLine("<li>Forest has more ET than Wetland.</li>")
                        ElseIf landuse2 <> "Wetland" AndAlso TotalET < aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression2) Then
                            CheckETIssuesStatement.AppendLine("<li>" & landuse2 & " has more ET than Forest.</li>")
                        End If
                        'Comparing Interception ET loss
                        If landuse2 <> "Wetland" AndAlso InterceptionET < aLandLoadingConstReport.Compute("AVG(CEPE)", lSelectExpression2) Then
                            CheckETIssuesStatement.AppendLine("<li>" & landuse2 & " has more interception loss than Forest.</li>")
                        End If
                    Next landuse2


                Case "Wetland"
                    For Each landuse2 As String In aListofLandUsesinUCI
                        If landuse2 = "Forest" OrElse landuse2 = "Wetland" OrElse landuse2 = "Impervious" Then Continue For
                        Dim lSelectExpression2 As String = "OpTypeNumber Like 'P:%' And genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                        If TotalET < aLandLoadingConstReport.Compute("AVG(TAET)", lSelectExpression2) Then
                            CheckETIssuesStatement.AppendLine("<li>" & landuse2 & " has more ET than Wetland.</li>")
                        End If
                        If InterceptionET < aLandLoadingConstReport.Compute("AVG(CEPE)", lSelectExpression2) Then
                            CheckETIssuesStatement.AppendLine("<li>" & landuse2 & " has more interception loss than Wetland.</li>")
                        End If
                    Next landuse2
            End Select
        End If

        Return CheckETIssuesStatement.ToString

    End Function
    ''' <summary>
    ''' This function checks if simulated sediment outflow is inconsistent among land uses.
    ''' </summary>
    ''' <param name="aLanduse"></param>
    ''' <param name="aListofLandUsesinUCI"></param>
    ''' <param name="aLandLoadingConstReport"></param>
    ''' <returns></returns>
    Private Function CheckTotalSedimentErosion(ByVal aLanduse As String, ByVal aListofLandUsesinUCI As List(Of String),
                                               ByVal aLandLoadingConstReport As DataTable) As String
        Dim lUnits As String = " lb/ac/yr"
        Dim CheckTotalSedErosion As New Text.StringBuilder
        Dim lSelectExpression As String = "genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
        Dim TotalSedRunoff As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression) 'Need to convert sediment erosion rate to lbs/ac
        Dim TotalGullyErosion As Double = 0
        Try
            TotalGullyErosion = aLandLoadingConstReport.Compute("AVG(SCRSD)", lSelectExpression)
        Catch ex As Exception

        End Try
        If TotalGullyErosion > 0 And aLanduse <> "Forest" Then
            CheckTotalSedErosion.AppendLine("<li>Gully erosion is being simulated on " & aLanduse & ".</li>")
        End If

        Dim LoadingRate As List(Of Double) = GetMinMaxLoadingRates(aLanduse, "SED")
        If TotalSedRunoff > LoadingRate(1) OrElse TotalSedRunoff < LoadingRate(0) Then
            CheckTotalSedErosion.AppendLine("<li>Sediment loading rate of <b>" & Format(TotalSedRunoff, "0.00") & lUnits & "</b> is outside the typical limit of <b>" &
                                            LoadingRate(0) & " - " & LoadingRate(1) & lUnits & "</b> for " & aLanduse & ".</li>")
        End If

        Select Case aLanduse
            Case "Impervious"
                For Each landuse2 As String In aListofLandUsesinUCI
                    If landuse2 = "Impervious" Then Continue For
                    Dim lSelectExpression2 As String = "genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                    Dim TotalSedRunoff2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression2)
                    If TotalSedRunoff2 > TotalSedRunoff Then
                        CheckTotalSedErosion.AppendLine("<li>Impervious area has greater sediment runoff than " & landuse2 & ".</li>")
                    End If
                Next

            Case "Forest"
                For Each landuse2 As String In aListofLandUsesinUCI
                    If landuse2 = "Forest" OrElse landuse2 = "Impervious" Then Continue For
                    Dim lSelectExpression2 As String = "OpTypeNumber Like 'P:%' And genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                    Dim TotalSedRunoff2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression2)
                    If landuse2 = "Wetland" AndAlso TotalSedRunoff2 > TotalSedRunoff Then
                        CheckTotalSedErosion.AppendLine("<li>Wetland has greater sediment runoff than Forest.</li>")
                    ElseIf landuse2 <> "Wetland" AndAlso TotalSedRunoff2 < TotalSedRunoff Then
                        CheckTotalSedErosion.AppendLine("<li>Forest has greater sediment runoff than " & landuse2 & ".</li>")
                    End If
                Next
            Case "Wetland"
                For Each landuse2 As String In aListofLandUsesinUCI
                    If landuse2 = "Forest" OrElse landuse2 = "Wetland" OrElse landuse2 = "Impervious" Then Continue For
                    Dim lSelectExpression2 As String = "OpTypeNumber Like 'P:%' And genLandUse = '" & landuse2 & "' And Year = 'SumAnnual'"
                    Dim TotalSedRunoff2 As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression2)
                    If TotalSedRunoff2 < TotalSedRunoff Then
                        CheckTotalSedErosion.AppendLine("<li>Wetland has greater sediment runoff than " & landuse2 & ".</li>")
                    End If
                Next
        End Select
        Return CheckTotalSedErosion.ToString
    End Function
    ''' <summary>
    ''' Calculating Nutrient Loading Rate Range for all landuses 
    ''' </summary>
    ''' <param name="aLanduse"></param>
    ''' <param name="aConstituentName"></param>
    ''' <param name="aListofLandUsesinUCI"></param>
    ''' <param name="aLandLoadingConstReport"></param>
    ''' <returns></returns>
    Private Function CheckNutrientLoading(ByVal aLanduse As String, ByVal aConstituentName As String, ByVal aListofLandUsesinUCI As List(Of String),
                                               ByVal aLandLoadingConstReport As DataTable) As String
        Dim CheckNutrientLoadingText As New Text.StringBuilder
        Dim lUnits As String = " lb/ac/yr"
        Dim lSelectExpression As String = "ConstNameEXP = '" & aConstituentName &
                                            "' AND genLandUse = '" & aLanduse & "' And Year = 'SumAnnual'"
        Dim lconversionfactor As Double = 1.0
        'If aConstituentName = "SED" Then lconversionfactor = 2000
        Dim TotalRunoff As Double = aLandLoadingConstReport.Compute("AVG(TotalOutflow)", lSelectExpression) * lconversionfactor 'Need to convert sediment erosion rate to lbs/ac
        Dim LoadingRate As List(Of Double) = GetMinMaxLoadingRates(aLanduse, aConstituentName)
        If TotalRunoff > LoadingRate(1) OrElse TotalRunoff < LoadingRate(0) Then
            CheckNutrientLoadingText.AppendLine("<li>" & aConstituentName & " loading rate of <b>" & Format(TotalRunoff, "0.00") & lUnits &
                                                "</b> is outside the typical range of <b>" & Format(LoadingRate(0), "0.00") & " - " &
                Format(LoadingRate(1), "0.00") & lUnits & "</b> for " & aLanduse & ".</li>")
        End If

        Return CheckNutrientLoadingText.ToString
    End Function
    Private Function GetMinMaxLoadingRates(ByVal aLanduse As String, ByVal aConstituent As String) As List(Of Double)
        Dim MinMaxRange As New List(Of Double)
        Dim LoadingRateRange As XmlDocument = New XmlDocument()
        LoadingRateRange.LoadXml(My.Resources.LoadingRates) '    
        Dim SelectStatementFromXML As String = "/LoadingRates/Nutrient[@type='" & aConstituent & "']/Landuse[@name='" & aLanduse & "']"
        Dim nodes As XmlNodeList = LoadingRateRange.DocumentElement.SelectNodes(SelectStatementFromXML)

        MinMaxRange.Add(nodes(0).SelectSingleNode("Min").InnerText)
        MinMaxRange.Add(nodes(0).SelectSingleNode("Max").InnerText)

        Return MinMaxRange
    End Function
    'Private Function CheckIfMonthlyFlagisOn(ByVal aTableParm As String, ByVal aParmName As String, Optional ByVal aConst As String = "") As IList(Of String)
    '    Dim MonthlyFlagisOn As New List(Of String)
    '    'Based on the aTableParm and aParmName locate the relevant TableName and Parametervalue that describes if the monthly flag is on
    '    Dim MonthlyParmName As XmlDocument = New XmlDocument
    '    MonthlyParmName.LoadXml(My.Resources.MonthlyParmForMonthlyParm)

    '    Dim SelectStarementFromXML As String = "/MONTHLYTABLE/Parm"
    '    Dim nodes As XmlNodeList = MonthlyParmName.DocumentElement.SelectNodes(SelectStarementFromXML)
    '    For Each node As XmlNode In nodes
    '        If aTableParm = node.SelectSingleNode("ParmTable").InnerText AndAlso aParmName = node.SelectSingleNode("ParmName").InnerText Then
    '            Dim FLAGTable As String = node.SelectSingleNode("FlagTable").InnerText
    '            Dim FLAGParm As String = node.SelectSingleNode("FlagName").InnerText

    '        End If

    '    Next

    '    Return MonthlyFlagisOn
    'End Function

    Private Function VerifyStorageTrend(ByVal aUCI As HspfUci, ByVal aBinaryData As atcDataSource, ByVal aConstituent As String) As String
        Dim StorageTrend As New Text.StringBuilder
        Dim OverAllStorageTrend As New Text.StringBuilder
        Dim lSimSpan As Double = aUCI.GlobalBlock.EdateJ - aUCI.GlobalBlock.SDateJ
        Dim lNumberOfTrendIssues As Integer = 0
        Logger.Dbg("Constituent Storage Analysis for " & aConstituent)
        Dim lListOfStorageVariables As New List(Of String)
        Select Case aConstituent
            Case "WAT"
                lListOfStorageVariables.Add("R:VOL")
                OverAllStorageTrend.AppendLine("<h2>Reach water volume analysis</h2>")
            Case "TN"
                lListOfStorageVariables.Add("R:NO3-STOR")
                lListOfStorageVariables.Add("R:TAM-STOR")
                lListOfStorageVariables.Add("R:N-TOT-CONC")
                lListOfStorageVariables.Add("P:SQO-NO3")
                lListOfStorageVariables.Add("P:SQO-NH3+NH4")
                lListOfStorageVariables.Add("I:SQO-NO3")
                lListOfStorageVariables.Add("I:SQO-NH3+NH4")
                OverAllStorageTrend.AppendLine("<h2>Nitrate, ammonia, and total nitogen storage and concentration analysis</h2>")
            Case "TP"
                lListOfStorageVariables.Add("R:PO4-STOR")
                lListOfStorageVariables.Add("R:P-TOT-CONC")
                lListOfStorageVariables.Add("P:SQO-ORTHO P")
                lListOfStorageVariables.Add("I:SQO-ORTHO P")
                OverAllStorageTrend.AppendLine("<h2>Orthophosphorus and total phosphorus storage and concentration analysis</h2>")
            Case "SED"
                lListOfStorageVariables.Add("R:BEDDEP")
                lListOfStorageVariables.Add("P:DETS")
                lListOfStorageVariables.Add("I:SLDS")
                OverAllStorageTrend.AppendLine("<h2>Reach Bed Depth and sediment storage analysis</h2>")
            Case "BOD-Labile"
                lListOfStorageVariables.Add("P:SQO-BOD")
                lListOfStorageVariables.Add("I:SQO-BOD")
                OverAllStorageTrend.AppendLine("<h2>BOD-labile storage analysis</h2>")
        End Select
        If lSimSpan < 1827 Then
            OverAllStorageTrend.AppendLine("<p>The time span of simulation is shorter than 5 years. The long term trend analysis may not be accurate.</p>")
        End If
        StorageTrend.AppendLine("<ul>")
        For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
            Dim lLocationName As String = ""
            Select Case lOperation.Name
                Case "PERLND"
                    lLocationName = "P:" & lOperation.Id
                Case "IMPLND"
                    lLocationName = "I:" & lOperation.Id
                Case "RCHRES"
                    lLocationName = "R:" & lOperation.Id
                Case Else
                    Continue For
            End Select

            For Each StorageVariable As String In lListOfStorageVariables
                If Not StorageVariable.StartsWith(lLocationName.Substring(0, 2)) Then Continue For
                StorageVariable = StorageVariable.Split(":")(1)
                Logger.Dbg("Operation ID= " & lOperation.Id & ", Storage Variable = " & StorageVariable)
                Dim lSlope As Double = 0
                Dim lIntercept As Double = 0
                Dim lRCoeff As Double = 0
                Dim lStorageTimeSeries As atcTimeseries = aBinaryData.DataSets.FindData("Location", lLocationName).FindData("Constituent", StorageVariable)(0)
                If lStorageTimeSeries Is Nothing Then Continue For
                Dim lTempTimeSeries As New atcTimeseries(Nothing)
                lTempTimeSeries = lStorageTimeSeries.Clone
                For i As Integer = 0 To lStorageTimeSeries.numValues
                    lTempTimeSeries.Value(i) = lStorageTimeSeries.Dates.Value(i)
                Next
                Dim lArgs As New atcDataAttributes()
                Dim lTSerAverage As Double = 0
                Dim lTSerStdev As Double = 0
                Dim lCoeffVariation As Double = 0
                Try
                    lTSerAverage = lStorageTimeSeries.Attributes.GetDefinedValue("Mean").Value
                    lTSerStdev = lStorageTimeSeries.Attributes.GetDefinedValue("Standard Deviation").Value
                    lCoeffVariation = lTSerStdev / lTSerAverage
                Catch
                    StorageTrend.AppendLine("Could not estimate trend for Operation ID= " & lOperation.Id & ", Storage Variable = " & StorageVariable)

                    Continue For
                End Try
                If lCoeffVariation < 0.1 Then Continue For
                lArgs.SetValue("Timeseries", lStorageTimeSeries)
                lArgs.SetValue("Number", lTSerAverage)
                lStorageTimeSeries = DoMath("subtract", lArgs)
                lArgs.SetValue("Timeseries", lStorageTimeSeries)
                lArgs.SetValue("Number", lTSerStdev)
                lStorageTimeSeries = DoMath("divide", lArgs)
                'Now find the slope of this timeseries
                FitLine(lStorageTimeSeries, lTempTimeSeries, lSlope, lIntercept, lRCoeff, "")
                If lSlope > 0.002 Then
                    StorageTrend.AppendLine("<li>The " & StorageVariable & " for " & lLocationName & " is increasing.</li>")
                    lNumberOfTrendIssues += 1
                ElseIf lSlope < -0.002 Then
                    StorageTrend.AppendLine("<li>The " & StorageVariable & " for " & lLocationName & " is decreasing.</li>")
                    lNumberOfTrendIssues += 1
                End If
            Next
        Next
        StorageTrend.AppendLine("</ul>")

        If lNumberOfTrendIssues > 0 Then
            OverAllStorageTrend.AppendLine("<p>Following non-typical long term trend issues were noticed in the model.</p>")
            OverAllStorageTrend.Append(StorageTrend)
        Else
            OverAllStorageTrend.AppendLine("<p>No long term storage or concentration issues were noticed in the model.</p>")
        End If

        Return OverAllStorageTrend.ToString
    End Function
    Private Function ConvertToHtmlFile(ByVal myTable As DataTable, ByVal Optional NumberOfRows As Integer = 0) As String
        Dim myHtmlFile As String = ""
        Dim myBuilder As New Text.StringBuilder
        Dim ColumnCounter As Integer = 0
        If myTable Is Nothing Then
            Throw New System.ArgumentNullException("myTable")
        Else
            myBuilder.AppendLine("<table>")
            myBuilder.AppendLine("<tr>")
            For Each myColumn As DataColumn In myTable.Columns
                ColumnCounter += 1
                If ColumnCounter = 1 Then
                    myBuilder.Append("<th align = ""left"">")
                Else
                    myBuilder.Append("<th>")
                End If
                myBuilder.Append(myColumn.Caption)
                myBuilder.AppendLine("</th>")
            Next
            myBuilder.AppendLine("</tr>")

            Dim MyRowCounter As Integer = 0
            ColumnCounter = 0
            For Each myRow As DataRow In myTable.Rows
                MyRowCounter += 1
                myBuilder.AppendLine("<tr>")
                For Each myColumn As DataColumn In myTable.Columns
                    ColumnCounter += 1
                    If ColumnCounter = 1 Then
                        myBuilder.Append("<td align = ""left"">")
                    Else
                        myBuilder.Append("<td align=""center"">")
                    End If

                    myBuilder.Append(myRow(myColumn.ColumnName).ToString())
                    myBuilder.AppendLine("</td>")
                Next
                ColumnCounter = 0
                If NumberOfRows <> 0 AndAlso MyRowCounter = NumberOfRows Then Exit For
            Next
            myBuilder.AppendLine("</tr>")

        End If

        'Close tags. 
        myBuilder.AppendLine("</table>")

        myHtmlFile = myBuilder.ToString()
        Return myHtmlFile
    End Function
    ''' <summary>
    ''' This function checks the diurnal pattern of the hourly time series. 
    ''' </summary>
    ''' <param name="aBinaryData"></param>
    ''' <param name="aUCI"></param>
    ''' <param name="aConstituent"></param>
    ''' <returns></returns>
    Private Function CheckDiurnalPattern(ByVal aBinaryData As atcDataSource, ByVal aUCI As HspfUci, ByVal aConstituent As String) As String
        Dim lDiurnalPattern As New Text.StringBuilder
        Dim lFoundTheTS As Boolean = False
        Dim lGroupName As String = ""
        Dim lMemberName As String = ""
        Dim lMemSub1 As Integer = 0
        Dim lMemSub2 As Integer = 0
        Dim lConstituent As String = ""
        Select Case aConstituent
            Case "DO"
                lGroupName = "OXRX"
                lMemberName = "DOX"
                lMemSub1 = 1
                lMemSub2 = 1
                lConstituent = "DOXCONC"
            Case "Water Temperature"
                lGroupName = "OXRX"
                lMemberName = "DOX"
                lMemSub1 = 1
                lMemSub2 = 1
                lConstituent = "TW"
        End Select
        For Each lRCHRES As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
            Dim lTS As atcTimeseries = LocateTheTimeSeries(aUCI, lRCHRES.Id, lGroupName, lMemberName, lMemSub1, lMemSub2, lFoundTheTS) 'Look for the timeseries in the WDM file

            If (Not lTS Is Nothing) OrElse (lTS.Attributes.GetDefinedValue("Time Unit").Value < 3) Then 'This means that timeseries is hourly or minute
                lTS = Aggregate(lTS, atcTimeUnit.TUHour, 1, atcTran.TranAverSame) 'In case the timeseries is shorter than hour, then aggregate it to hourly
            End If
            If lTS Is Nothing Then
                lTS = aBinaryData.DataSets.FindData("Constituent", lConstituent).FindData("Location", "R:" & lRCHRES.Id)(0)
            End If
            If (Not lTS Is Nothing) OrElse (lTS.Attributes.GetDefinedValue("Time Unit").Value < 3) Then 'This means that timeseries is hourly or minute
                lTS = Aggregate(lTS, atcTimeUnit.TUHour, 1, atcTran.TranAverSame) 'In case the timeseries is shorter than hour, then aggregate it to hourly
            End If
            If lTS.Attributes.GetDefinedValue("Time Unit").Value = 2 Then 'Getting to this loop only if timestep is hourly
                'Find the 12 to 4 am timeseries, and 12 to 4pm timeseries
                Dim lSeasonMorning As New atcSeasonsHour
                'hours ending 1am - 4am
                lSeasonMorning.SeasonSelected(0) = True
                lSeasonMorning.SeasonSelected(1) = True
                lSeasonMorning.SeasonSelected(2) = True
                lSeasonMorning.SeasonSelected(3) = True
                Dim lSeasonTimeseries As atcTimeseries = lSeasonMorning.SplitBySelected(lTS, Nothing)(0)
                Dim lMorningMean As Double = lSeasonTimeseries.Attributes.GetValue("Mean")
                Dim lSeasonAfternoon As New atcSeasonsHour
                'hours ending 1pm - 4pm
                lSeasonAfternoon.SeasonSelected(12) = True
                lSeasonAfternoon.SeasonSelected(13) = True
                lSeasonAfternoon.SeasonSelected(14) = True
                lSeasonAfternoon.SeasonSelected(15) = True
                lSeasonTimeseries = lSeasonAfternoon.SplitBySelected(lTS, Nothing)(0)
                Dim lAfternoonMean As Double = lSeasonTimeseries.Attributes.GetValue("Mean")
            End If

        Next




        Return lDiurnalPattern.ToString
    End Function
    Private Function CheckIfAdsDesIsSimulated() As String
        Return ""
    End Function
End Module




