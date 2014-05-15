'this module - originally HSPFOutputReports.vb - was checked out from the BASINS 4 SVN on November 30, 2011
'it is open source and can be found at http://svn.mapwindow.org/svnroot/BASINS40
'the accompanying projects imported below are copied as is from the BASINS 4 checkout
'this particular module was modified by Becky Zeckoski at Virginia Tech beginning December 1, 2011 for the 
'purposes of developing a replacement for HSPEXP for use by the Center for Watershed Studies at Virginia Tech
'I deleted lots of things unneeded for our purposes and indicated where I made additions or changes

Imports System
Imports atcUtility
Imports atcData
Imports atcTimeseriesStatistics
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports atcUCI

'note from Becky: will also need references to atcReport and atcTimeseriesMath projects
'I also installed the MapWinGIS.ocx ActiveX components from http://mapwingis.codeplex.com/releases but I'm still not sure if I actually needed to
'I installed it because I couldn't figure out how to initially initalize the aMapWin variable
'additionally, if you don't have BASINS installed, you'll need to get hass_ent.dll from C:\Windows\System32 on a computer that does have it installed
'and move it to your C:\Windows\System32 folder and register it...I wasn't able to get it to work on Windows 7 though
'you'll also need to get hspfmsg.mdb from C:\BASINS\models\HSPF\bin on a computer that has BASINS installed - this can be anywhere

Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports atcGraph
'Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project

Imports MapWindow.Interfaces 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
'note that for whatever reason, the MapWindow.Interfaces DLL from that site doesn't
'have everything needed (no IMapWin) - if you also add a reference to MapWinInterfaces to the project, it compiles
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
    Private pHSPFExe As String = FindFile("Please locate WinHspfLt.exe", "WinHspfLt.exe")
    Private pRunUci As Boolean = False 'Anurag added this option if the user wants this program to run the uci as well

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
        pMakeLogGraphs = StartUp.chkLogGraphs.Checked
        pMakeSupGraphs = StartUp.chkSupportingGraphs.Checked
        pRunUci = StartUp.chkRunHSPF.Checked
        pMakeAreaReports = StartUp.chkAreaReports.Checked

        Dim lTestName As String = IO.Path.GetFileNameWithoutExtension(StartUp.txtUCIPath.Text)
        Logger.Status("Beginning analysis of " & lTestName, True)

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

        'Becky added the following to be dependent upon user input:
        pTestPath = StartUp.txtUCIPath.Text
        pBaseName = lTestName
        pTestPath = Mid(pTestPath, 1, Len(pTestPath) - Len(pBaseName) - 4)


        'as best I can tell, the output location should include R for reach and the outlet reach number
        'Becky added the following if-then-else to allow multiple output locations
        For Each lRCH As String In StartUp.txtRCH.Text.Split(","c)
            If IsNumeric(lRCH) Then
                pOutputLocations.Add("R:" & CInt(lRCH)) ' the Cint should get rid of leading spaces and zeros 
            End If
        Next
        StartUp.Hide()
        Logger.StartToFile(pTestPath & "LogFile.txt")
        Logger.Status("Run characteristics read", True)
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        Logger.Status("HSPEXP+ is running.")
        Using lProgress As New ProgressLevel

            If pRunUci = True Then
                Logger.Status(Now & " Running HSPF Simulation of " & pBaseName & ".uci", True)
                Dim lExitCode As Integer
                lExitCode = LaunchProgram(pHSPFExe, pTestPath, pBaseName & ".uci") 'Run HSPF program
                If lExitCode <> 0 Then
                    'Throw New ApplicationException("WinHSPFLt could not run, Analysis cannot continue")
                    Dim ans As Integer
                    ans = MsgBox("WinHSPFLt could not run, Analysis cannot continue")
                    End
                End If
            End If
            Logger.Status(Now & " HSPF Simulation of " & pBaseName & ".uci" & " finished.", True)
            'set up the timeseries attributes for statistics
            Dim lStat As New atcTimeseriesStatistics.atcTimeseriesStatistics
            For Each lOperation As atcDefinedValue In lStat.AvailableOperations
                atcDataAttributes.AddDefinition(lOperation.Definition)
            Next

            Logger.Status(Now & " Opening " & pBaseName & ".uci", True)
            'open uci file
            Logger.Dbg(Now & " Attempting to open hspfmsg.wdm")
            Dim lMsg As New atcUCI.HspfMsg
            lMsg.Open("hspfmsg.wdm") 'Becky: this can be found at C:\BASINS\models\HSPF\bin if you did the typical BASINS install
            Logger.Dbg(Now & " Reading " & pBaseName & ".uci")
            Logger.Progress(10, 100)
            

            'Becky: change the lOutFolderName to include pTestPath (changing directory several lines above didn't seem to work right)
            'Dim lOutFolderName As String = pTestPath & "\Reports_" & pBaseName & "_" & lDateString & "\"
            'Becky - remove copy to new folder to run reports, assume user has already copied files to current folder and reports should be saved there
            'Anurag did some additional changes and made it conditional so as to follow VT and ATC pattern both.

            'TryDelete(lOutFolderName) 'Becky removed
            'IO.Directory.CreateDirectory(lOutFolderName) 'Becky removed
            'dont change name of uci - make it easier to compare with others, foldername contains info about which run
            'Becky: change the copy from to include pTestPath (changing directory several lines above didn't seem to work right)
            'Becky: remove file copy below, replace with a copy to a new folder with a new run number if the user specified run number
            'System.IO.File.Copy(pTestPath & "\" & pBaseName & ".uci", lOutFolderName & pBaseName & ".uci")
            'the following if then complex was generated by Becky


            'build collection of operation types to report
            Dim lOperationTypes As New atcCollection
            lOperationTypes.Add("P:", "PERLND")
            lOperationTypes.Add("I:", "IMPLND")
            lOperationTypes.Add("R:", "RCHRES")

            Dim lStr As String = ""

            Dim lRunMade As String = ""
            Dim lWdmFileName As String = pTestPath & pBaseName & ".wdm" 'Becky fixed to remove extra "\"

            Dim lWdmDataSource As New atcDataSourceWDM()
            If System.IO.File.Exists(lWdmFileName) Then
                Dim wdmFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lWdmFileName)
                lRunMade = wdmFileInfo.LastWriteTime.ToString
                'TODO: allow observed flow to come from a different fileEXPE
            Else
                'Becky added this if-then-else to catch the case below if the WDM file does not exist
                Dim ans As Integer
                ans = MsgBox("WDM file " & lWdmFileName & " does not exist.  This program will end now.")
                End
            End If
            Dim lOutFolderName As String = pTestPath
            If pRunNo >= 0 Then 'set to -1 if user didn't enter a run number
                Logger.Dbg(Now & " Incrementing run number and copying files")
                Dim lCurRun As String
                Dim lNextRun As String
                Dim lNextName As String
                If pRunNo < 9 Then
                    lNextRun = "0" & CStr(pRunNo + 1)
                    lCurRun = "0" & CStr(pRunNo)
                ElseIf pRunNo = 9 Then
                    lNextRun = "10"
                    lCurRun = "09"
                Else
                    lNextRun = CStr(pRunNo + 1)
                    lCurRun = CStr(pRunNo)
                End If
                Dim lNextFolder As String = pRootPath & "Run" & lNextRun & "\"
                If Microsoft.VisualBasic.Strings.Right(pBaseName, Len(lCurRun)) = lCurRun Then
                    'the file name is the XXX##.uci format, where XXX is the watershed abbreviation and ## is the run number
                    lNextName = Microsoft.VisualBasic.Strings.Left(pBaseName, Len(pBaseName) - Len(lCurRun)) & lNextRun
                Else
                    'the file name is in the XXX.uci or other format, do not change the file name to include the run number
                    lNextName = pBaseName
                End If
                TryDelete(lNextFolder)
                IO.Directory.CreateDirectory(lNextFolder)
                'the following assumes that the EXS, WDM, and UCI all have the same name
                System.IO.File.Copy(pTestPath & pBaseName & ".uci", lNextFolder & lNextName & ".uci")
                If System.IO.File.Exists(pTestPath & pBaseName & ".wdm") Then
                    System.IO.File.Copy(pTestPath & pBaseName & ".wdm", lNextFolder & lNextName & ".wdm")
                End If
                If System.IO.File.Exists(pTestPath & pBaseName & ".exs") Then
                    System.IO.File.Copy(pTestPath & pBaseName & ".exs", lNextFolder & lNextName & ".exs")
                End If
            Else
                Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") & _
                                Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")
                lOutFolderName = pTestPath & "Reports_" & lDateString & "\"
                System.IO.Directory.CreateDirectory(lOutFolderName)
                System.IO.File.Copy(pTestPath & pBaseName & ".uci", lOutFolderName & pBaseName & ".uci", overwrite:=True)
            End If
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
            Logger.Dbg("ReadUCI " & lHspfUci.Name)
            Logger.Dbg(Now & " Successfully read " & pBaseName & ".uci")
            Logger.Progress(20, 100)

            'A folder name is given that has the basename and the time when the run was made.
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
                Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(lHspfUci, lRunMade, lOperationTypes, alocations, True, lOutFolderName & "/AreaReports/")
                lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)

                SaveFileString(lOutFolderName & "/AreaReports/AreaReport.txt", lReport.ToString)
            End If


            If StartUp.chkExpertStats.Checked = True Then

                Dim lExpertSystemFileNames As New NameValueCollection
                AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
                If lExpertSystemFileNames.Count < 1 Then 'Becky added this if-then to warn the user if no EXS files exist
                    'in directory - without an EXS, nothing else will happen (For Each finds nothing), but previously
                    'the user received no notification of this
                    'In future, at this point, user should get an option if they want to make teir own exs file and this program should
                    'help the user do it.
                    MsgBox("No basins specifications file (*.exs) file found in directory " & IO.Directory.GetCurrentDirectory & "!  Statistics, summaries, and graphs cannot be computed.", vbOKOnly, "No Specification File!")
                    Logger.Dbg(Now & " No basins specifications file found, no statistics computed")
                End If
                Dim lExpertSystem As HspfSupport.atcExpertSystem
                lWdmDataSource.Open(lWdmFileName) 'Opening the wdm file here, so that it opens only if needed.
                For Each lExpertSystemFileName As String In lExpertSystemFileNames
                    Logger.Status(Now & " Calculating Expert Statistics for the file " & lExpertSystemFileName, True)
                    Try

                        Logger.Dbg(Now & " Calculating run statistics to save in " & pBaseName & ".sts")
                        lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lWdmDataSource, lExpertSystemFileName)
                        lStr = lExpertSystem.Report
                        'Becky changed file name to match our typical file structure
                        If pRunNo >= 0 Then
                            SaveFileString(lOutFolderName & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".sts", lStr)
                            'Save as *.sts file if using VT format. 
                        Else
                            SaveFileString(lOutFolderName & "ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)
                            'save as text file, if using default format.
                        End If
                        'Becky commented the following out - no need to exactly reproduce the EXS file we already have
                        'SaveFileString(lOutFolderName & pBaseName & "out.exs", lExpertSystem.AsString) 'Becky added "out" so as not to write over the original

                        'Becky added these to output advice
                        Logger.Dbg(Now & " Creating advice to save in " & pBaseName & ".adv")
                        For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                            Dim lAdviceStr As String = "Advice for Calibration Run " & pBaseName & vbCrLf & Now & vbCrLf & vbCrLf
                            lExpertSystem.CalcAdvice(lAdviceStr, lSiteIndex)
                            Dim lSiteNam As String = lExpertSystem.Sites(lSiteIndex - 1).Name
                            SaveFileString(lOutFolderName & pBaseName & "." & lSiteNam & ".adv", lAdviceStr)
                        Next

                        Dim lCons As String = "Flow"
                        For Each lSite As HexSite In lExpertSystem.Sites
                            Dim lSiteName As String = lSite.Name
                            Dim lArea As Double = lSite.Area
                            Dim lSimTSerInchesOriginal As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            Dim lSimTSerInches As atcTimeseries = Aggregate(lSimTSerInchesOriginal, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                            lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                            Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                            lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                            lSimTSer.Attributes.SetValue("YAxis", "Left")
                            lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                            Dim lObsTSerOriginal As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
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
                                    "have already been created.  Click Abort now to avoid an unhandled error.", MsgBoxStyle.AbortRetryIgnore + MsgBoxStyle.Critical, _
                                    "Site Name Invalid")
                                If ans = vbAbort Then
                                    GoTo RWZProgramEnding
                                End If
                                ReDim lRchId(0)
                                lRchId(0) = lSite.Name
                            End If

                            Dim lPrecDsn As Integer = lSite.DSN(5)
                            Dim lPrecTserOriginal As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lPrecDsn), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
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
                            Dim lOutFileName As String = lOutFolderName & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            Logger.Dbg(Now & " Calculating annual summary for " & lSiteName)
                            lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                         lCons, lSiteName, _
                                                                         "inches", _
                                                                         lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                         lRunMade, _
                                                                         lExpertSystem.SDateJ, _
                                                                         lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            Logger.Dbg(Now & " Calculating daily summary for " & lSiteName)
                            'pProgressBar.pbProgress.Increment(6)
                            lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                               lCons, lSiteName, _
                                                                               lSimTSer, lObsTSer, _
                                                                               lRunMade, _
                                                                               lExpertSystem.SDateJ, _
                                                                               lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            'Becky's addition: only make graphs if user wants them. 
                            If pMakeLogGraphs Or pMakeStdGraphs Or pMakeSupGraphs Then
                                Logger.Status(Now & " Preparing Graphs", True)
                                Dim lTimeSeries As New atcTimeseriesGroup
                                Logger.Dbg(Now & " Creating nonstorm graphs")
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
                                         pGraphAnnual, lOutFolderName, _
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
                                    IO.Directory.CreateDirectory(lOutFolderName & "\Storms\")
                                    GraphStorms(lTimeSeries, 2, lOutFolderName & "Storms\" & lSiteName, pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem, pMakeLogGraphs)
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

            Logger.Dbg(Now & " Opening " & pBaseName & ".hbn")
            Dim lHspfBinFileName As String = pTestPath & pBaseName & ".hbn"
            Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
            If System.IO.File.Exists(lHspfBinFileName) Then
                lHspfBinDataSource.Open(lHspfBinFileName)
                If lHspfBinDataSource.DataSets.Count > 1 Then

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
                        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

                        Logger.Dbg(Now & " Calculating Constituent Budget for " & lConstituent)
                        lReportCons = Nothing
                        lReportCons = HspfSupport.ConstituentBudget.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade)
                        lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_RCH_Ann_Avg_Lds.txt"
                        '"All_Budget.txt"
                        SaveFileString(lOutFileName, lReportCons.ToString)
                        lReportCons = Nothing

                        Logger.Dbg(Now & " Calculating Watershed Summary Reports for " & lConstituent)
                        lReportCons = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lConstituent)
                        lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_OPN_Ann_Avg_Lds.txt"
                        '"_All_WatershedSummary.txt"
                        SaveFileString(lOutFileName, lReportCons.ToString)
                        lReportCons = Nothing

                        Logger.Dbg(Now & " Calculating Annual Constituent Balance for " & lConstituent)

                        lReportCons = HspfSupport.ConstituentBalance.Report _
                           (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                            lHspfBinDataSource, lLocations, lRunMade)
                        lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Per_OPN_Ann.txt"
                        '"_Mult_ConstituentBalance.txt"
                        SaveFileString(lOutFileName, lReportCons.ToString)

                        Logger.Dbg("Summary at " & lLocations.Count & " locations")
                        'constituent balance


                        lReportCons = HspfSupport.WatershedConstituentBalance.Report _
                        (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                        lHspfBinDataSource, lRunMade)
                        lOutFileName = lOutFolderName & lConstituentName & "_" & pBaseName & "_Grp_By_OPN_LU_Ann_Avg.txt"
                        '"_All_WatershedConstituentBalance.txt"
                        SaveFileString(lOutFileName, lReportCons.ToString)


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
                Else
                    Logger.Dbg("The HBN file didn't have any data and therefore constituent balance reports were not generated. Make sure that HSPF" & _
                               "run finished last time.")
                    Dim ans As Integer
                    ans = MsgBox("HBN file " & lHspfBinFileName & " does not have any data.  Constituent Balance reports will not be generated. " & _
                                 "Did uci file run properly last time?")
                    End
                End If
            Else
                'give message if hbn file does not exist, but continue without it
                Dim ans As Integer
                ans = MsgBox("HBN file " & lHspfBinFileName & " does not exist.  Constituent Balance reports will not be generated.")
                End
            End If

            Logger.Dbg(Now & " Output Written to " & lOutFolderName)
            Logger.Dbg("Reports Written in " & lOutFolderName)
RWZProgramEnding:
            'pProgressBar.pbProgress.Increment(39)
            Logger.Dbg(Now & " HSPEXP+ Complete")
            Logger.Msg("HSPEXP+ is complete")
            OpenFile(lOutFolderName)
        End Using
        Logger.Status("")
        Call Application.Exit()

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

End Module
