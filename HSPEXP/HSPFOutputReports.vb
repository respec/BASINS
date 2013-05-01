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
Imports BeckyUtilities 'added by Becky

'note from Becky: will also need references to atcReport and atcTimeseriesMath projects
'I also installed the MapWinGIS.ocx ActiveX components from http://mapwingis.codeplex.com/releases but I'm still not sure if I actually needed to
'I installed it because I couldn't figure out how to initially initalize the aMapWin variable
'additionally, if you don't have BASINS installed, you'll need to get hass_ent.dll from C:\Windows\System32 on a computer that does have it installed
'and move it to your C:\Windows\System32 folder and register it...I wasn't able to get it to work on Windows 7 though
'you'll also need to get hspfmsg.mdb from C:\BASINS\models\HSPF\bin on a computer that has BASINS installed - this can be anywhere

Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports atcGraph
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project

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
    Private pRunNo As Integer 'the current run number - may or may not be provided, if it is then use it to copy to a new folder
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = False
    Private pCurveStepType As String = "RearwardStep"
    Private pConstituents As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer
    Private pGraphWQOnly As Boolean = False 'indicates whether ONLY graphing water quality
    Private pGraphWQ As Boolean = False 'indicates whether to graph only water quality
    Private pWaterYears As Boolean = False
    Private pExpertPrec As Boolean = False
    Private pIdsPerSeg As Integer = 50
    'following were added by Becky:
    Private pProgressBar As New RWZProgress
    Private pMakeStdGraphs As Boolean 'flag to indicate user wants standard graphs (monthly, daily, storms, flow duration)
    Private pMakeLogGraphs As Boolean 'flag to indicate user wants logarithmic graphs (all that are logarithmic)
    Private pMakeSupGraphs As Boolean 'flag to indicate user wants supporting graphs (UZS, LZS, ET, cumulative diff)
    Private pMakeAreaReports As Boolean 'flag to indicate user wants subwatershed & land use reports created

    Private Sub Initialize()
        pProgressBar.Show()
        pProgressBar.txtProgress.Text = Now & " Hydrologic statistics program started" & vbCrLf
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768
        pMakeStdGraphs = StartUp.chkGraphStandard.Checked
        pMakeLogGraphs = StartUp.chkLogGraphs.Checked
        pMakeSupGraphs = StartUp.chkSupportingGraphs.Checked

        pMakeAreaReports = StartUp.chkAreaReports.Checked

        Dim lTestName As String = StartUp.txtPrefix.Text
        pProgressBar.txtProgress.Text &= Now & " Beginning analysis of " & lTestName & vbCrLf

        pConstituents.Add("Water")
        pExpertPrec = True 'Becky: this tells the program to read the DSN from the EXS file
        'Becky: remove the other constituents, only interested in water
        'Becky removed all the test cases
        'Becky added the following to be dependent upon user input:
        pTestPath = StartUp.txtUCIPath.Text
        pBaseName = lTestName
        If IsNumeric(StartUp.txtRunNo.Text) Then
            pRunNo = CInt(StartUp.txtRunNo.Text)
            If Directory.Exists(StartUp.txtRootPath.Text) Then
                pRootPath = StartUp.txtRootPath.Text
            Else
                pRootPath = pTestPath
            End If
        Else
            pRunNo = -1
        End If
        'as best I can tell, the output location should include R for reach and the outlet reach number
        'Becky added the following if-then-else to allow multiple output locations
        Dim ltxtRCH As String
        ltxtRCH = StartUp.txtRCH.Text
        If InStr(1, ltxtRCH, ",") > 0 Then
            'multiple reaches
            Dim lRCHRES() As String
            lRCHRES = Split(ltxtRCH, ",")
            For Each lRCH As String In lRCHRES
                pOutputLocations.Add("R:" & CInt(lRCH)) ' the Cint should get rid of leading spaces and zeros 
            Next
        Else
            'only one reach
            pOutputLocations.Add("R:" & CInt(StartUp.txtRCH.Text))
        End If
        StartUp.Hide()
        Logger.StartToFile(pTestPath & "LogFile.txt")
        pProgressBar.txtProgress.Text &= Now & " Run characteristics read" & vbCrLf
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        If IO.File.Exists(pBaseName & "Orig.uci") Then
            IO.File.Copy(pBaseName & "Orig.uci", pBaseName & ".uci")
        End If

        'If pGraphWQ Then
        '    GraphWQDuration()
        'End If
        'If pGraphWQOnly Then Exit Sub ' early end here if just doing it for WQ graphs


        'open uci file
        pProgressBar.txtProgress.Text &= Now & " Attempting to open hspfmsg.mdb" & vbCrLf
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb") 'Becky: this can be found at C:\BASINS\models\HSPF\bin if you did the typical BASINS install
        pProgressBar.txtProgress.Text &= Now & " Reading " & pBaseName & ".uci" & vbCrLf
        pProgressBar.pbProgress.Increment(11)
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        Logger.Dbg("ReadUCI " & lHspfUci.Name)
        pProgressBar.txtProgress.Text &= Now & " Successfully read " & pBaseName & ".uci" & vbCrLf
        pProgressBar.pbProgress.Increment(10)
        'Becky: removed special cases Lynnwood and mono as they should not be relevant


        'open HBN file
        'TODO: need to allow additional binary output files!
        'Becky: this appears to allow summary tables that aren't too useful for water and selection of run time, comment out
        'Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        'Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        'lHspfBinDataSource.Open(lHspfBinFileName)

        'watershed summary
        'Becky: next two lines commented out 'cause we didn't read the binary file
        'Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        'Dim lRunMade As String = lHspfBinFileInfo.LastWriteTime.ToString
        'Becky: added new lRunMade below to be dependent on today rather than file creation time
        Dim lRunMade As String = Now.ToString
        Dim lDateString As String = Format(Year(lRunMade), "00") & Format(Month(lRunMade), "00") & _
                Format(Microsoft.VisualBasic.DateAndTime.Day(lRunMade), "00") & Format(Hour(lRunMade), "00") & Format(Minute(lRunMade), "00")

        'A folder name is given that has the basename and the time when the run was made.
        'Becky: change the lOutFolderName to include pTestPath (changing directory several lines above didn't seem to work right)
        'Dim lOutFolderName As String = pTestPath & "\Reports_" & pBaseName & "_" & lDateString & "\"
        'Becky - remove copy to new folder to run reports, assume user has already copied files to current folder and reports should be saved there
        Dim lOutFolderName As String = pTestPath
        'TryDelete(lOutFolderName) 'Becky removed
        'IO.Directory.CreateDirectory(lOutFolderName) 'Becky removed
        'dont change name of uci - make it easier to compare with others, foldername contains info about which run
        'Becky: change the copy from to include pTestPath (changing directory several lines above didn't seem to work right)
        'Becky: remove file copy below, replace with a copy to a new folder with a new run number if the user specified run number
        'System.IO.File.Copy(pTestPath & "\" & pBaseName & ".uci", lOutFolderName & pBaseName & ".uci")
        'the following if then complex was generated by Becky
        If pRunNo >= 0 Then 'set to -1 if user didn't enter a run number
            pProgressBar.txtProgress.Text &= Now & " Incrementing run number and copying files" & vbCrLf
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
        End If
     
        'build collection of operation types to report
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")

        Dim lStr As String = ""

        'area report
        'Becky's addition: only do this if user wants it
        If pMakeAreaReports Then
            pProgressBar.txtProgress.Text &= Now & " Producing land use and area reports" & vbCrLf
            'Becky's note: I changed modUtility so this will actually do this for all locations in pOutputLocations
            Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(lHspfUci, lRunMade, lOperationTypes, pOutputLocations, True, lOutFolderName & "\")
            lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)

            SaveFileString(lOutFolderName & "AreaReport.txt", lReport.ToString)
        End If
        If pConstituents.Contains("Water") Then
            'open WDM file
            pProgressBar.txtProgress.Text &= Now & " Opening " & pBaseName & ".wdm" & vbCrLf
            Dim lWdmFileName As String = pTestPath & pBaseName & ".wdm" 'Becky fixed to remove extra "\"
            Dim lWdmDataSource As New atcDataSourceWDM()
            If System.IO.File.Exists(lWdmFileName) Then
                lWdmDataSource.Open(lWdmFileName)
                'TODO: allow observed flow to come from a different fileEXPE
            Else
                'Becky added this if-then-else to catch the case below if the WDM file does not exist
                Dim ans As Integer
                ans = MsgBox("WDM file " & lWdmFileName & " does not exist.  This program will end now.")
                End
            End If

            Dim lExpertSystemFileNames As New NameValueCollection
            AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
            If lExpertSystemFileNames.Count < 1 Then 'Becky added this if-then to warn the user if no EXS files exist
                'in directory - without an EXS, nothing else will happen (For Each finds nothing), but previously
                'the user received no notification of this
                MsgBox("No basins specifications file (*.exs) file found in directory " & IO.Directory.GetCurrentDirectory & "!  Statistics, summaries, and graphs cannot be computed.", vbOKOnly, "No Specification File!")
                pProgressBar.txtProgress.Text &= Now & " No basins specifications file found, no statistics computed" & vbCrLf
            End If
            Dim lExpertSystem As HspfSupport.atcExpertSystem
            For Each lExpertSystemFileName As String In lExpertSystemFileNames
                Try
                    Dim lFileCopied As Boolean = False
                    If IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                        'Becky: I believe all this is doing is copying the existing EXS file to one named the same as the UCI file
                        lFileCopied = TryCopy(lExpertSystemFileName, pBaseName & ".exs")
                    End If
                    pProgressBar.txtProgress.Text &= Now & " Calculating run statistics to save in " & pBaseName & ".sts" & vbCrLf
                    lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lWdmDataSource)
                    lStr = lExpertSystem.Report
                    'Becky changed file name to match our typical file structure
                    'SaveFileString(lOutFolderName & "ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)
                    SaveFileString(lOutFolderName & pBaseName & ".sts", lStr)
                    'Becky commented the following out - no need to exactly reproduce the EXS file we already have
                    'SaveFileString(lOutFolderName & pBaseName & "out.exs", lExpertSystem.AsString) 'Becky added "out" so as not to write over the original

                    'Becky added these to output advice
                    pProgressBar.txtProgress.Text &= Now & " Creating advice to save in " & pBaseName & ".adv" & vbCrLf
                    Dim lAdviceStr As String = "Advice for Calibration Run " & pBaseName & vbCrLf & Now & vbCrLf & vbCrLf
                    lExpertSystem.CalcAdvice(lAdviceStr)
                    SaveFileString(lOutFolderName & pBaseName & ".adv", lAdviceStr)

                    'lStr = lExpertSystem.AsString 'NOTE:just testing
                    'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                    Dim lCons As String = "Flow"
                    For Each lSite As HexSite In lExpertSystem.Sites
                        Dim lSiteName As String = lSite.Name
                        Dim lArea As Double = lSite.Area
                        Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                        lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lSimTSer.Attributes.SetValue("YAxis", "Left")
                        lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSer As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lObsTSer.Attributes.SetValue("YAxis", "Left")
                        lObsTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTSer, lArea)
                        lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")

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
                        'Becky modified lRchId to read the first array value
                        Dim lOperation As atcUCI.HspfOperation = lHspfUci.OpnBlks("RCHRES").OperFromID(lRchId(0))
                        If lOperation Is Nothing Then
                            Logger.Dbg("MissingOperationInUCI for " & lRchId(0))
                        Else
                            pProgressBar.txtProgress.Text &= Now & " Calculating watershed and land use areas from SCHEMATIC" & vbCrLf
                            Dim lAreaOriginal As Double = 0 '=0 added by Becky
                            Dim lAreaOrigTemp As Double = 0 'added by Becky to accumulate the lAreaOriginals generated by WeightedSourceArea
                            Dim lPrecSourceCollection As New atcCollection
                            'Becky's note: all that the following does is calculate the total area from the SCHEMATIC
                            'block contributing to each RCHRES above the outlet and compare it to a calculation
                            'of these same areas multiplied by any multiplication factors in the PREC input in the
                            'EXT SOURCES block.  I'm really not sure why this is done. However, lAreaOriginal is
                            'used later on to compare to the area in the EXS file (though this was originally implemented wrong).
                            Dim lAreaFromWeight As Double = 0 'shortened this to = 0 so that I could accumulate later
                            For Each lRch As Integer In lRchId
                                lOperation = lHspfUci.OpnBlks("RCHRES").OperFromID(lRch) 'Becky changed to use the argument in the new loop
                                lAreaFromWeight += lHspfUci.WeightedSourceArea(lOperation, "PREC", lPrecSourceCollection, lAreaOrigTemp)
                                'Becky (above) changed lAreaOriginal to lAreaOrigTemp so I can accumulate; also set to lAreaWeighted instead of doing it on the dim line
                                lAreaOriginal += lAreaOrigTemp
                                lAreaOrigTemp = 0
                            Next lRch
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
                                        'Becky added the content from here to the 'else' for the above if to allow the user to specify
                                        'a PREC DSN to find in the output WDM if it does not match what is in the UCI file
                                        Dim lReplaceDSNStr As String
                                        Dim lReplaceDSNInt As Integer
                                        lReplaceDSNStr = InputBox("Your UCI calls for a PREC timeseries with DSN " & lPrecSourceCollection.Keys(lSourceIndex) & " but this does not " & _
                                               "exist in the output WDM " & lWdmFileName & ".  Please enter the DSN for the PREC timeseries held in your " & _
                                               "output WDM file.   If the PREC timeseries does not exist in your output WDM file or you wish to close this program " & _
                                               "for any other reason, enter a -1 in the input box.  In this case your graphs will not be created, your annual and " & _
                                               "monthly statistics will not be calculated, and the program will not compare " & _
                                               "the total areas in the SCHEMATIC vs. what you specified in the EXS file.", "PREC not found in output WDM.", -1)
                                        If IsInteger(lReplaceDSNStr) Then
                                            lReplaceDSNInt = CInt(lReplaceDSNStr)
                                            If lReplaceDSNInt > 0 Then
                                                lPrecDataGroup = lWdmDataSource.DataSets.FindData("ID", lReplaceDSNInt)
                                                If lPrecDataGroup.Count = 0 Then
                                                    MsgBox("The value you entered does not represent an existing dataset number in the WDM File " & _
                                                        lWdmDataSource.Name & ".  You were warned.  This program is terminating now.", vbOKOnly, "PREC DSN still not found")
                                                    Logger.Dbg("Precipitation timeseries with DSN " & lReplaceDSNInt & "not found. Program closing.")
                                                    GoTo RWZProgramEnding
                                                Else
                                                    Logger.Dbg("PrecDataGroupFrom " & lWdmFileName & " " & lPrecDataGroup.Count)
                                                End If
                                            Else 'user did not give a new DSN
                                                Logger.Dbg("Precipitation timeseries with DSN " & lReplaceDSNInt & "not found. Program closing.")
                                                GoTo RWZProgramEnding
                                            End If
                                        Else
                                            Logger.Dbg("Precipitation timeseries with DSN " & lReplaceDSNInt & "not found. Program closing.")
                                            GoTo RWZProgramEnding
                                        End If
                                        'Becky commented out the next four lines because they don't work as a solution to the problem 
                                        '(the problem being that the program can't find the EXT SOURCES PREC timeseries in the HSPEXP output WDM - 
                                        'this would happen if the DSN for PREC in the meteorological WDM doesn't match that in the output WDM
                                        'used with HSPEXP)
                                        'Dim lPrecWdmDataSource As New atcDataSourceWDM()
                                        'lPrecWdmDataSource.Open(pTestPath & "FBMet.wdm") 'Becky fixed to remove extra "\"
                                        'lPrecDataGroup = lPrecWdmDataSource.DataSets.FindData("ID", lPrecSourceCollection.Keys(lSourceIndex))
                                        'Logger.Dbg("PrecDataGroupFrom FBMet.wdm " & lPrecDataGroup.Count)
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
                                If Math.Abs(lSite.Area - lAreaOriginal) > 0.01 Then 'Becky changed < to > as this was actually checking the opposite of what it intended to
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

                            RWZSetArgs(lSimTSerInches)
                            RWZSetArgs(lObsTSerInches)
                            RWZSetArgs(lPrecTser)

                            pProgressBar.txtProgress.Text &= Now & " Calculating monthly summary" & vbCrLf
                            pProgressBar.pbProgress.Increment(5)

                            lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                                 lCons, lSiteName, _
                                                                                 "inches", _
                                                                                 lSimTSerInches, lObsTSerInches, _
                                                                                 lRunMade, _
                                                                                 lExpertSystem.SDateJ, _
                                                                                 lExpertSystem.EDateJ)
                            Dim lOutFileName As String = lOutFolderName & "MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            pProgressBar.txtProgress.Text &= Now & " Calculating annual summary" & vbCrLf
                            lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                         lCons, lSiteName, _
                                                                         "inches", _
                                                                         lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                         lRunMade, _
                                                                         lExpertSystem.SDateJ, _
                                                                         lExpertSystem.EDateJ)
                            lOutFileName = lOutFolderName & "Annual" & lCons & "Stats-" & lSiteName & ".txt"
                            SaveFileString(lOutFileName, lStr)

                            pProgressBar.txtProgress.Text &= Now & " Calculating daily summary" & vbCrLf
                            pProgressBar.pbProgress.Increment(6)
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
                                Dim lTimeSeries As New atcTimeseriesGroup
                                pProgressBar.txtProgress.Text &= Now & " Creating nonstorm graphs" & vbCrLf
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
                                    pProgressBar.txtProgress.Text &= Now & " Creating storm graphs" & vbCrLf
                                    pProgressBar.pbProgress.Increment(29)
                                    lTimeSeries.Add("Observed", lObsTSer)
                                    lTimeSeries.Add("Simulated", lSimTSer)
                                    lTimeSeries.Add("Prec", lPrecTser)

                                    lTimeSeries(0).Attributes.SetValue("Units", "cfs")
                                    lTimeSeries(0).Attributes.SetValue("StepType", pCurveStepType)
                                    lTimeSeries(1).Attributes.SetValue("Units", "cfs")
                                    lTimeSeries(1).Attributes.SetValue("StepType", pCurveStepType)
                                    GraphStorms(lTimeSeries, 2, lOutFolderName & "Storm", pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem, pMakeLogGraphs)
                                    lTimeSeries.Dispose()
                                End If
                            End If
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

        'For Each lConstituent As String In pConstituents
        '    Logger.Dbg("------ Begin summary for " & lConstituent & " -----------------")

        '    Dim lReportCons As New atcReport.ReportText
        '    Dim lOutFileName As String = ""
        '    If lConstituent <> "Sediment" Then
        '        HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, , , , pWaterYears, pIdsPerSeg).ToString()
        '        lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverland.txt"
        '        SaveFileString(lOutFileName, lReportCons.ToString)
        '        lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, True, pWaterYears, pIdsPerSeg)
        '        lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShortWithMinMax.txt"
        '        SaveFileString(lOutFileName, lReportCons.ToString)
        '        lReportCons = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts, False, True, False, pWaterYears, pIdsPerSeg)
        '        lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedOverlandShort.txt"
        '        SaveFileString(lOutFileName, lReportCons.ToString)
        '    End If

        '    lReportCons = Nothing
        '    lReportCons = HspfSupport.ConstituentBudget.Report(lHspfUci, lConstituent, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_Budget.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)
        '    lReportCons = Nothing

        '    lReportCons = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lConstituent)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedSummary.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)
        '    lReportCons = Nothing

        '    Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")
        '    Logger.Dbg("Summary at " & lLocations.Count & " locations")
        '    'constituent balance
        '    lReportCons = HspfSupport.ConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lLocations, lRunMade)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalance.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)

        '    lReportCons = HspfSupport.ConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lLocations, lRunMade, True)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivot.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)

        '    lReportCons = HspfSupport.ConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lLocations, lRunMade, True, 2, 5, 8)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowTab.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_ConstituentBalancePivotNarrowSpace.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))

        '    'watershed constituent balance 
        '    lReportCons = HspfSupport.WatershedConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lRunMade)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalance.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)

        '    lReportCons = HspfSupport.WatershedConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lRunMade, , , , True)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivot.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)

        '    lReportCons = HspfSupport.WatershedConstituentBalance.Report _
        '       (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '        lHspfBinDataSource, lRunMade, , , , True, 2, 5, 8)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_All_WatershedConstituentBalancePivotNarrowTab.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString)
        '    lOutFileName = lOutFolderName & lConstituent & "_" & pBaseName & "_Mult_WatershedConstituentBalancePivotNarrowSpace.txt"
        '    SaveFileString(lOutFileName, lReportCons.ToString.Replace(vbTab, " "))

        '    If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
        '        HspfSupport.WatershedConstituentBalance.ReportsToFiles _
        '           (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '            lHspfBinDataSource, pOutputLocations, lRunMade, _
        '            lOutFolderName, True)
        '        'now pivoted version
        '        HspfSupport.WatershedConstituentBalance.ReportsToFiles _
        '           (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
        '            lHspfBinDataSource, pOutputLocations, lRunMade, _
        '            lOutFolderName, True, True)
        '    End If
        'Next
        pProgressBar.txtProgress.Text &= Now & " Output Written to " & lOutFolderName & vbCrLf
        Logger.Dbg("Reports Written in " & lOutFolderName, "HSPFOutputReports")
RWZProgramEnding:
        pProgressBar.pbProgress.Increment(39)
        pProgressBar.txtProgress.Text &= Now & " Statistics Program Complete"
        pProgressBar.lblProgressTitle.Text = "Program is complete.  Please ignore the timer cursor and click Exit."
        pProgressBar.txtProgress.DeselectAll()
        pProgressBar.Cursor = Cursors.Default
        pProgressBar.cmdExit.Visible = True
        pProgressBar.Focus()
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

                InitMatchingColors(FindFile("", "GraphColors.txt")) 'Becky moved this here from atcGraph10/CreateZgc so that
                'the file is found ONCE instead of a gazillion times and the colors are initialized ONCE rather than a gazillion
                'times

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
