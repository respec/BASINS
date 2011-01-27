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
    Private pExpertPrec As Boolean = False
    Private pIdsPerSeg As Integer = 50

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768

        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "mono"
        'Dim lTestName As String = "Susq_020501"
        'Dim lTestName As String = "tinley"
        'Dim lTestName As String = "hspf"
        'Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "shena"
        'Dim lTestName As String = "mono_lu2030a2_base"
        'Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "housatonic"
        'Dim lTestName As String = "beaver"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        'Dim lTestName As String = "SantaClara"
        'Dim lTestName As String = "NonUpatoi"

        pConstituents.Add("Water")
        'pConstituents.Add("FColi")
        'pConstituents.Add("Sediment")
        pConstituents.Add("N-PQUAL")
        pConstituents.Add("P-PQUAL")
        pConstituents.Add("BOD-PQUAL")
        pConstituents.Add("TotalN")
        pConstituents.Add("TotalP")

        Select Case lTestName
            Case "tutorial"
                pTestPath = "C:\Program Files\FramesV2\MEERT\Data\HSPF\WestBranch"
                pBaseName = "bact_2"
                pOutputLocations.Add("R:5")
                pIdsPerSeg = 5
                pWaterYears = True
            Case "Susq_020501"
                pTestPath = "G:\Projects\TT_GCRP\ProjectsTT\Susq\parms"
                pBaseName = "Susq_020501"
                pOutputLocations.Add("R:69")
                pExpertPrec = True
                pIdsPerSeg = 25
            Case "mono"
                pTestPath = "D:\mono_Tong\lu2090b2_base_bmp"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
                'Add scenario directories
                'pBaseFolders.Clear()
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_10")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono10bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_70")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmp")
                'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmpDbg")
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
                If Not pConstituents.Contains("Sediment") Then
                    pConstituents.Add("Sediment")
                End If
            Case "shena"
                pTestPath = "c:\test\genscn"
                pBaseName = "base"
                pOutputLocations.Add("Lynnwood")
            Case "upatoi"
                'pTestPath = "D:\Basins\modelout\Upatoi"
                'pTestPath = "C:\Basins\data\20710\PollutantModeling\calibration\hydrology"
                'pTestPath = "C:\Basins\data\20710-01\Upatoi"
                pTestPath = "G:\Projects\SERDP\TestCase"
                pBaseName = "Upatoi"
                'pOutputLocations.Add("R:639")
                'pOutputLocations.Add("R:614")
                'pOutputLocations.Add("R:626")
                'pOutputLocations.Add("R:30")
                'pOutputLocations.Add("R:33")
                'pOutputLocations.Add("R:36")
                'pOutputLocations.Add("R:35")
                'pOutputLocations.Add("R:662")
                'pOutputLocations.Add("R:666")
                'pOutputLocations.Add("R:46")
                'pOutputLocations.Add("R:74")
                'pOutputLocations.Add("R:2")
                'pOutputLocations.Add("R:3")
                pOutputLocations.Add("R:614")
                pGraphAnnual = True
                pCurveStepType = "NonStep" 'Tony's convention 
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101} ', 201, 301, 401, 501, 601, 701, 801, 901, 951}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102} ', 202, 302, 402, 502, 602, 702, 802, 902, 952}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
                pWaterYears = True 'TODO: figure this out from run
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
            Case "NonUpatoi"
                pTestPath = "H:"
                pBaseName = "NonUpatoi"
                pOutputLocations.Add("R:109")
                pOutputLocations.Add("R:206")
                pOutputLocations.Add("R:311")
                pOutputLocations.Add("R:509")
                pOutputLocations.Add("R:402")
                pGraphAnnual = True
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101, 201, 301, 401, 501, 601, 701}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102, 202, 302, 402, 502, 602, 702}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
                pCurveStepType = "NonStep" 'Tony's convention 
                pWaterYears = True 'TODO: figure this out from run
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        Logger.Dbg("CurrentFolder " & My.Computer.FileSystem.CurrentDirectory)
        If IO.File.Exists(pBaseName & "Orig.uci") Then
            IO.File.Copy(pBaseName & "Orig.uci", pBaseName & ".uci")
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

        'area report

        Dim lReport As atcReport.ReportText = HspfSupport.AreaReport(lHspfUci, lRunMade, lOperationTypes, pOutputLocations, True, lOutFolderName & "\")
        lReport.MetaData.Insert(lReport.MetaData.ToString.IndexOf("Assembly"), lReport.AssemblyMetadata(System.Reflection.Assembly.GetExecutingAssembly) & vbCrLf)

        SaveFileString(lOutFolderName & "AreaReport.txt", lReport.ToString)

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

            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    lOutFolderName, True)
                'now pivoted version
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lConstituent, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    lOutFolderName, True, True)
            End If
        Next
        Logger.Dbg("Reports Written in " & lOutFolderName, "HSPFOutputReports")
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
