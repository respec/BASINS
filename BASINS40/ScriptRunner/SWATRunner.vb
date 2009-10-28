Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System
Imports System.Data
Imports System.Collections.ObjectModel
Imports Microsoft.VisualBasic

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcTimeseriesBinary
Imports atcList
Imports atcTimeseriesMath.atcTimeseriesMath
Imports SwatObject

Module SWATRunner
    'This is the batch SWAT Run code for the UMRB project.
    'The code currently only works for the SWAT annual run outputs.
    '
    Private pStartYear As Integer = 0 '1960
    Private pNumYears As Integer = 12
    Private pRefreshDB As Boolean = False ' make a copy of the SWATInput database
    Private pUserInteractiveUpdate As Boolean = True 'False - use these defaults
    Private pOutputSummarize As Boolean = True
    Private pInputSummarizeBeforeChange As Boolean = True
    Private pInputSummarizeChanged As Boolean = True
    Private pCropChangeSummarize As Boolean = True
    Private pChangeCropAreas As Boolean = True
    Private pRunModel As Boolean = True
    Private pRunMonthly As Boolean = True
    Private pRunDaily As Boolean = False
    Private pRunYearly As Boolean = False
    Private pScenario As String = "RevCrop"
    Private pDrive As String = "C:"
    Private pBaseFolder As String = pDrive & "\Scratch\UMRB\baseline90"
    Private pSWATGDB As String = "c:\Program Files\SWAT 2005 Editor\Databases\SWAT2005.mdb"
    Private pOutGDB As String = "baseline90.mdb"
    Private pInputFolder As String
    Private pOutGDBFolder As String
    Private pOutputFolder As String
    Private pReportsFolder As String
    Private pLogsFolder As String
    Private pCrpFutureColumn As Integer = 3 'Change to 1 to disable changing CRP to future values
    Private pCrpFuture As String = "CRPFutures.txt"
    Private pParmChangesTextfile As String = "SWATParmChanges.txt"
    Friend pFormat As String = "###,##0.00"
    'Private pSWATExe As String = pOutputFolder & "\swat2005.exe" 'local copy with input data
    Private pSWATExe As String = "C:\Program Files\SWAT 2005 Editor\swat2005.exe"
    Private CanConvertToCRP() As String = {"AGRR", "RNGE", "PAST", "URHD", "URLD", "URMD", "ALFA", "HAY", "FRSD", "FRSE", "FRST"}

    Private pDoCaseStudySummary As Boolean = False
    Private predoCaliSummary As Boolean = False

    Private pDoSA As Boolean = False
    Private pchangePCP As String = String.Empty
    Private pchangeTemp As String = String.Empty
    Private pchangeFert As String = String.Empty
    Private pchangeResRmv As Boolean = False



    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        Dim lContinue As Boolean = True
        'these defaults are overwritten by registry values set by most recent run
        Dim lUserParms As New atcCollection

        With lUserParms
            .Add("Do Sensitivity Analysis?", pDoSA)
            .Add("Summarize Case Study Output", pDoCaseStudySummary)
            .Add("Start Year", pStartYear)
            .Add("Number of Years", pNumYears)
            .Add("Run Model", pRunModel)
            .Add("Run Model Monthly", pRunMonthly)
            .Add("Run Model Yearly", pRunYearly)
            .Add("Run Model Daily", pRunDaily)
            .Add("RefreshDB", pRefreshDB)
            .Add("OutputSummarize", pOutputSummarize)
            .Add("InputSummarizeBeforeChange", pInputSummarizeBeforeChange)
            .Add("InputSummarizeChanged", pInputSummarizeChanged)
            .Add("ChangeCropAreas", pChangeCropAreas)
            .Add("CropChangeSummarize", pCropChangeSummarize)
            .Add("Scenario", pScenario)
            .Add("Drive", pDrive)
            .Add("BaseFolder", pBaseFolder)
            .Add("SWATGDB", pSWATGDB)
            .Add("OutGDB", pOutGDB)
            .Add("ParmChangesTextfile", pParmChangesTextfile)
            .Add("CrpFutureTextfile", pCrpFuture)
            .Add("CrpFutureColumn", pCrpFutureColumn)
            .Add("SWATExe", pSWATExe)

            .Add("Change Rainfall", pchangePCP)
            .Add("Change Temperature", pchangeTemp)
            .Add("Change Fertilization Rate", pchangeFert)
            .Add("Remove Corn Residue", pchangeResRmv)

        End With
        If pUserInteractiveUpdate Then
            Dim lAsk As New frmArgs
            lContinue = lAsk.AskUser("User Specified Parameters", lUserParms)
            If lContinue Then
                With lUserParms
                    pDoSA = .ItemByKey("Do Sensitivity Analysis?")
                    pDoCaseStudySummary = .ItemByKey("Summarize Case Study Output")
                    pStartYear = .ItemByKey("Start Year")
                    pNumYears = .ItemByKey("Number of Years")
                    pRunModel = .ItemByKey("Run Model")
                    pRunMonthly = .ItemByKey("Run Model Monthly")
                    pRunYearly = .ItemByKey("Run Model Yearly")
                    pRunDaily = .ItemByKey("Run Model Daily")

                    pRefreshDB = .ItemByKey("RefreshDB")
                    pOutputSummarize = .ItemByKey("OutputSummarize")
                    pInputSummarizeBeforeChange = .ItemByKey("InputSummarizeBeforeChange")
                    pInputSummarizeChanged = .ItemByKey("InputSummarizeChanged")
                    pCropChangeSummarize = .ItemByKey("CropChangeSummarize")
                    pChangeCropAreas = .ItemByKey("ChangeCropAreas")
                    pScenario = .ItemByKey("Scenario")
                    pDrive = .ItemByKey("Drive")
                    pBaseFolder = .ItemByKey("BaseFolder")
                    pSWATGDB = .ItemByKey("SWATGDB")
                    pOutGDB = .ItemByKey("OutGDB")
                    pParmChangesTextfile = .ItemByKey("ParmChangesTextfile")
                    pCrpFuture = .ItemByKey("CrpFutureTextfile")
                    pCrpFutureColumn = .ItemByKey("CrpFutureColumn")
                    pSWATExe = .ItemByKey("SWATExe")

                    pchangePCP = .ItemByKey("Change Rainfall")
                    pchangeTemp = .ItemByKey("Change Temperature")
                    pchangeFert = .ItemByKey("Change Fertilization Rate")
                    pchangeResRmv = .ItemByKey("Remove Corn Residue")

                End With
            End If
        End If



        Dim lAllScenarios As New List(Of String)
        'Dim lcases As String() = {"RS", "AFDec", "AFInc"}
        'TODO: pDoCaseStudySummary and pDoSA could be used interchangeably here in this program
        If lContinue And pDoCaseStudySummary Then
            'Do summary of all case studies' outputs, then end the SWATRunner program
            'This is assuming all case studies' runs are already done BEFOREHAND!
            'This is essentially a big loop to go through all case studies
            'Specifically, the code would create corresponding RS, AFInc, AFDec
            'report folders under each scenarios' SWAT output directory
            '
            'lAllScenarios.Add("Scen2010RevP8889")
            'lAllScenarios.Add("Scen2015RevP8889")
            'lAllScenarios.Add("Scen2020RevP8889")
            'lAllScenarios.Add("Scen2022RevP8889")
            'lAllScenarios.Add("ScenExistRevP8889")

            'lAllScenarios.Add("Scen2010RevPRaccoon")
            'lAllScenarios.Add("Scen2015RevPRaccoon")
            'lAllScenarios.Add("Scen2020RevPRaccoon")
            'lAllScenarios.Add("Scen2022RevPRaccoon")
            'lAllScenarios.Add("ScenExistRevPRaccoon")
            'lAllScenarios.Add("SA_RS")
            'lAllScenarios.Add("ScenExistRevP")
            'lAllScenarios.Add("MetBase")
            'lAllScenarios.Add("SA_FertInc50")
            lAllScenarios.Add("SA_FertDec10")

            For Each pScenario In lAllScenarios
                pInputFolder = IO.Path.Combine(pBaseFolder, "Scenarios" & IO.Path.DirectorySeparatorChar & pScenario)
                Dim lswatMDB As String = String.Empty
                If pScenario.Contains("Exist") Then
                    lswatMDB = "SWAT2005RevP.mdb"
                ElseIf pScenario.Contains("2010") Then
                    lswatMDB = "SWAT2010RevP.mdb"
                ElseIf pScenario.Contains("2015") Then
                    lswatMDB = "SWAT2015RevP.mdb"
                ElseIf pScenario.Contains("2020") Then
                    lswatMDB = "SWAT2020RevP.mdb"
                ElseIf pScenario.Contains("2022") Then
                    lswatMDB = "SWAT2022RevP.mdb"
                ElseIf pScenario.Contains("SA_") Then
                    lswatMDB = "SWAT2005RevP.mdb"
                End If

                pSWATGDB = IO.Path.Combine(pBaseFolder, lswatMDB)
                'Get a new one
                If IO.File.Exists(pSWATGDB) Then
                    IO.File.Delete(pSWATGDB)
                    IO.File.Copy(IO.Path.Combine(pBaseFolder, "SWAT2005RevPBase\SWAT2005RevP.mdb"), pSWATGDB)
                End If

                'Here define the cases within a given scenario
                'Dim lcases As String() = {"AFInc50", "PcpInc20", "PcpDec20"}
                Dim lcases As String() = {""}
                For Each lzCase As String In lcases
                    If pScenario.Contains("8889") Then ' no case study runs for original scenarios
                        lzCase = ""
                    End If
                    If predoCaliSummary Then
                        If pScenario.Contains("RevPRaccoon") Then 'only for redoing the summary of the calibration runs
                            lzCase = ""
                        End If
                    End If
                    If pDoSA Then 'sensitivity run will be done 'in situ' without creating new folders for everything
                        lzCase = ""
                    End If
                    pOutGDBFolder = IO.Path.Combine(pInputFolder, "TablesIn" & lzCase)
                    pOutputFolder = IO.Path.Combine(pInputFolder, "TxtInOut" & lzCase)
                    pReportsFolder = IO.Path.Combine(pInputFolder, "TablesOut" & lzCase)
                    pLogsFolder = IO.Path.Combine(pInputFolder, "logs" & lzCase)

                    ChDriveDir(pInputFolder)

                    'log for swat runner
                    Logger.StartToFile(IO.Path.Combine(pLogsFolder, "SWATRunner.log"), , , True)

                    For Each lParmKey As String In lUserParms.Keys
                        Logger.Dbg(lParmKey & " = " & lUserParms.ItemByKey(lParmKey))
                    Next
                    Logger.Flush()

                    '****************************************************************************
                    'The below section is only need if one wants to run SWAT/summarize the SWAT inputs
                    'before and after crop area changes, this section is just a replicate of the 
                    'corresponding section down below, if just need to do the outputsummary, then
                    'can bypass this section altogether
                    '****************************************************************************
                    Dim lzOutGDB As String = IO.Path.Combine(pOutGDBFolder, pOutGDB)
                    If pRefreshDB OrElse Not IO.File.Exists(lzOutGDB) Then 'copy the entire input parameter database for this new scenario
                        If IO.File.Exists(lzOutGDB) Then
                            Logger.Dbg("DeleteExisting " & lzOutGDB)
                            IO.File.Delete(lzOutGDB)
                        End If
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lzOutGDB))
                        IO.File.Copy(IO.Path.Combine(pBaseFolder, pOutGDB), lzOutGDB)
                        Logger.Dbg("Copied " & lzOutGDB & " from " & pBaseFolder)
                    End If

                    If Not pRunModel Then
                        GoTo DOSUMMARYONLY
                    End If

                    Logger.Dbg("InitializeSwatInput")
                    Dim lzSwatInput As New SwatInput(pSWATGDB, lzOutGDB, pBaseFolder, pScenario)

                    If pStartYear > 0 Then
                        lzSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IYR", pStartYear)
                    End If

                    If pNumYears > 0 Then
                        lzSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "NBYR", pNumYears)
                    End If

                    If pRunMonthly Then
                        lzSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 0)
                    ElseIf pRunYearly Then
                        lzSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 2)
                    ElseIf pRunDaily Then
                        lzSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 1)
                    End If

                    lzSwatInput.CIO.PrintHru = False

                    If pInputSummarizeBeforeChange Then
                        SummarizeInput(lzSwatInput, "Before")
                    End If

                    Dim lzTotalArea As Double = 0.0
                    Dim lzTotalAreaNotConverted As Double = 0.0
                    Dim lzTotalAreaConverted As Double = 0.0
                    Dim lzTotalAreaCornFut As Double = 0.0
                    Dim lzTotalAreaCornNow As Double = 0.0
                    Dim lzCropChangesSummaryFilename As String = IO.Path.Combine(pLogsFolder, "CropChanges.txt")
                    Dim lzCropChangesHruFilename As String = IO.Path.Combine(pLogsFolder, "CropHruChanges.txt")
                    Dim lzCropConversions As New CropConversions("CORN")

                    If pCropChangeSummarize Then ' This switch has to be set to True in order to set the lTotalAreaCornNow
                        SummarizeCropChange(lzSwatInput, _
                                            lzCropConversions, _
                                            lzCropChangesSummaryFilename, _
                                            lzCropChangesHruFilename, _
                                            lzTotalArea, _
                                            lzTotalAreaNotConverted, _
                                            lzTotalAreaConverted, _
                                            lzTotalAreaCornFut, _
                                            lzTotalAreaCornNow)
                    Else
                        For Each lzString As String In LinesInFile(lzCropChangesSummaryFilename)
                            If lzString.StartsWith("Total") Then
                                Dim lzFields() As String = lzString.Split(vbTab)
                                Double.TryParse(lzFields(2), lzTotalArea)
                                Double.TryParse(lzFields(3), lzTotalAreaCornNow)
                                Double.TryParse(lzFields(4), lzTotalAreaConverted)
                                Double.TryParse(lzFields(5), lzTotalAreaNotConverted)
                                Double.TryParse(lzFields(6), lzTotalAreaCornFut)
                            End If
                        Next
                    End If

                    If pCrpFutureColumn > 1 Then
                        Dim lzCrpChanges As New atcTableDelimited
                        lzCrpChanges.Delimiter = vbTab
                        If lzCrpChanges.OpenFile(pCrpFuture) Then
                            Dim lzTotalAreaCrp As Double
                            Dim lzTotalAreaNotConvertedCrp As Double = 0.0
                            Dim lzTotalAreaConvertedCrp As Double = 0.0
                            Dim lzTotalAreaCornFutCrp As Double = 0.0
                            Dim lzTotalAreaCornNowCrp As Double = 0.0

                            SummarizeCRPChange(lzSwatInput, _
                                        lzCropChangesSummaryFilename & ".crp", _
                                        lzCropChangesHruFilename & ".crp", _
                                        lzTotalAreaCrp, _
                                        lzTotalAreaNotConvertedCrp, _
                                        lzTotalAreaConvertedCrp, _
                                        lzTotalAreaCornFutCrp, _
                                        lzTotalAreaCornNowCrp, _
                                        lzCrpChanges)
                        End If
                    End If

                    If pChangeCropAreas Then

                        'If pChangeCropAreas = True, then pCropChangeSummarize has to be set to True for all except Existing condition
                        'For explanation of the crop area changes, see the corresponding section below.
                        Dim lzDesiredFutureCornArea As Double = lzTotalAreaCornNow

                        If pScenario.Contains("Exist") Then
                            ' No landuse change
                        ElseIf pScenario.Contains("2010") Then
                            lzDesiredFutureCornArea += 1410.26
                        ElseIf pScenario.Contains("2015") Then
                            lzDesiredFutureCornArea += 1730.24
                        ElseIf pScenario.Contains("2020") Then
                            lzDesiredFutureCornArea += 1602.83
                        ElseIf pScenario.Contains("2022") Then
                            lzDesiredFutureCornArea += 1523.6
                        End If

                        Logger.Dbg("DesiredFutureCornArea = " & lzDesiredFutureCornArea)
                        Dim lzConvertFractionOfAvailable As Double = (lzDesiredFutureCornArea - lzTotalAreaCornNow) / (lzTotalAreaCornFut - lzTotalAreaCornNow)
                        Logger.Dbg("ConvertFractionOfAvailable = " & lzConvertFractionOfAvailable)
                        ChangeHRUfractions(lzSwatInput, lzCropConversions, lzCropChangesHruFilename, lzConvertFractionOfAvailable)
                    End If

                    If pInputSummarizeChanged Then
                        SummarizeInput(lzSwatInput, "Changed")
                    End If

                    If IO.File.Exists(pParmChangesTextfile) Then
                        Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
                        For Each lString As String In LinesInFile(pParmChangesTextfile)
                            Dim lzParms() As String = lString.Split(";")
                            lzSwatInput.UpdateInputDB(lzParms(0).Trim, lzParms(1).Trim, lzParms(2).Trim, lzParms(3).Trim, lzParms(4).Trim)
                        Next
                    End If

                    'Do the changes for sensitivity analysis
                    If pDoSA Then
                        'If pchangePCP IsNot String.Empty Then
                        '    changePCP(pOutputFolder & IO.Path.DirectorySeparatorChar & "pcp1.pcp", pchangePCP)
                        'End If
                        'If pchangeTemp IsNot String.Empty Then
                        '    changeTemp(pOutputFolder & IO.Path.DirectorySeparatorChar & "tmp1.tmp", pchangeTemp)
                        'End If
                        If pchangeFert IsNot String.Empty Then
                            'Dim lArgs As Dictionary(Of String, String) = New Dictionary(Of String, String)
                            ''pSWATGDB, , pBaseFolder, pScenario
                            'lArgs.Add("InputGDB", lzOutGDB)
                            'lArgs.Add("FertilizationRate", "")
                            'changeFert(lArgs)

                            Dim lSQL As String = "SELECT AUTO_NSTRS FROM mgt2 WHERE MGT_OP=11 AND CROP='CORN'"
                            Dim ltab As DataTable = lzSwatInput.QueryInputDB(lSQL)
                            Dim lrate As Double = Double.Parse(ltab.Rows(0)("AUTO_NSTRS").ToString)

                            lrate = CType(lrate * (1.0 + pchangeFert / 100.0) * 100.0 + 0.5, Integer) / 100.0
                            lzSwatInput.UpdateInputDB("MGT2", "MGT_OP=11 AND CROP NOT LIKE 'HAY' AND CROP NOT LIKE 'AGRR'", "AUTO_NSTRS", lrate)
                            'lzSwatInput.Mgt.Save()
                            ltab.Clear()
                            ltab = Nothing
                        End If

                        If pchangeResRmv Then
                            ''changeResRmv()
                            ''Change HI_OVR in baseline90jkRaccoon::mgt2
                            '' e.g. Change mgt2::subbasin 2::HRU 64::LANDUSE CSC1::SOIL IA110::SLOPE_CD 2-4::CROP CSC1::YEAR 1::HUSC 1.2::MGT_OP 5 -->
                            ''             mgt2::subbasin 2::HRU 64::LANDUSE CSC1::SOIL IA110::SLOPE_CD 2-4::CROP CSC1::YEAR 1::HUSC 1.2::MGT_OP 7::HI_OVR 0.9
                            ''             add another Kill only operation 8 for the exactly the same crop (insert another record)

                            ''Two approaches, change default HI or HI_OVR on MGT_OP 7, then MGT_OP 8
                            'Dim ltab As DataTable = lzSwatInput.Mgt.Table2
                            'For Each row As DataRow In ltab.Rows
                            '    Dim lcrop As String = row("CROP").ToString
                            '    Dim lop As Integer = Integer.Parse(row("MGT_OP").ToString)
                            '    If lcrop = "CSC1" OrElse lcrop = "CCS1" OrElse lcrop = "CCCC" OrElse lcrop = "CSS1" OrElse lcrop = "CORN" Then
                            '        If lop = 5 Then ' delete it and add another two records
                            '            Dim OID As Long = Long.Parse(row("OID").ToString)
                            '            'Dim SUBBASIN As Double = Double.Parse(row("SUBBASIN").ToString)
                            '            'Dim HRU As Double = Double.Parse(row("HRU").ToString)
                            '            'Dim LANDUSE As String = row("LANDUSE").ToString
                            '            'Dim SOIL As String = row("SOIL").ToString
                            '            'Dim SLOPE_CD As String = row("SLOPE_CD").ToString
                            '            'Dim CROP As String = row("CROP").ToString
                            '            'Dim YEAR As Integer = Integer.Parse(row("YEAR").ToString)
                            '            'Dim MONTH As Integer = Integer.Parse(row("MONTH").ToString)
                            '            'Dim DAY As Integer = Integer.Parse(row("DAY").ToString)
                            '            'Dim HUSC As Single = Single.Parse(row("HUSC").ToString)
                            '            'Dim MGT_OP As Integer = Integer.Parse(row("MGT_OP").ToString)
                            '            'Dim HEATUNITS As Single = Single.Parse(row("HEATUNITS").ToString)
                            '            'Dim PLANT_ID As Integer = Integer.Parse(row("PLANT_ID").ToString)
                            '            'Dim CURYR_MAT As Integer = Integer.Parse(row("CURYR_MAT").ToString)
                            '            'Dim LAI_INIT As Single = Single.Parse(row("LAI_INIT").ToString)
                            '            'Dim BIO_INIT As Single = Single.Parse(row("BIO_INIT").ToString)
                            '            'Dim HI_TARG As Single = Single.Parse(row("HI_TARG").ToString)
                            '            'Dim BIO_TARG As Single = Single.Parse(row("BIO_TARG").ToString)
                            '            'Dim CNOP As Double = Double.Parse(row("CNOP").ToString)
                            '            'Dim IRR_AMT As Single = Single.Parse(row("IRR_AMT").ToString)
                            '            'Dim FERT_ID As Integer = Integer.Parse(row("FERT_ID").ToString)
                            '            'Dim FRT_KG As Single = Single.Parse(row("FRT_KG").ToString)
                            '            'Dim FRT_SURFACE As Single = Single.Parse(row("FRT_SURFACE").ToString)
                            '            'Dim PEST_ID As Integer = Integer.Parse(row("PEST_ID").ToString)
                            '            'Dim PST_KG As Single = Single.Parse(row("PST_KG").ToString)
                            '            'Dim TILLAGE_ID As Integer = Integer.Parse(row("TILLAGE_ID").ToString)
                            '            'Dim HARVEFF As Single = Single.Parse(row("HARVEFF").ToString)
                            '            'Dim HI_OVR As Single = Single.Parse(row("HI_OVR").ToString)
                            '            'Dim GRZ_DAYS As Integer = Integer.Parse(row("GRZ_DAYS").ToString)
                            '            'Dim MANURE_ID As Integer = Integer.Parse(row("MANURE_ID").ToString)
                            '            'Dim BIO_EAT As Single = Single.Parse(row("BIO_EAT").ToString)
                            '            'Dim BIO_TRMP As Single = Single.Parse(row("BIO_TRMP").ToString)
                            '            'Dim MANURE_KG As Single = Single.Parse(row("MANURE_KG").ToString)
                            '            'Dim WSTRS_ID As Integer = Integer.Parse(row("WSTRS_ID").ToString)
                            '            'Dim AUTO_WSTRS As Single = Single.Parse(row("AUTO_WSTRS").ToString)
                            '            'Dim AFERT_ID As Integer = Integer.Parse(row("AFERT_ID").ToString)
                            '            'Dim AUTO_NSTRS As Single = Single.Parse(row("AUTO_NSTRS").ToString)
                            '            'Dim AUTO_NAPP As Single = Single.Parse(row("AUTO_NAPP").ToString)
                            '            'Dim AUTO_NYR As Single = Single.Parse(row("AUTO_NYR").ToString)
                            '            'Dim AUTO_EFF As Single = Single.Parse(row("AUTO_EFF").ToString)
                            '            'Dim AFRT_SURFACE As Single = Single.Parse(row("AFRT_SURFACE").ToString)
                            '            'Dim SWEEPEFF As Single = Single.Parse(row("SWEEPEFF").ToString)
                            '            'Dim FR_CURB As Single = Single.Parse(row("FR_CURB").ToString)
                            '            'Dim IMP_TRIG As Double = Double.Parse(row("IMP_TRIG").ToString)
                            '            'Dim FERT_DAYS As Long = Long.Parse(row("FERT_DAYS").ToString)
                            '            'Dim CFRT_ID As Long = Long.Parse(row("CFRT_ID").ToString)
                            '            'Dim IFRT_FREQ As Long = Long.Parse(row("IFRT_FREQ").ToString)
                            '            'Dim CFRT_KG As Single = Single.Parse(row("CFRT_KG").ToString)

                            '            For i As Integer = 0 To row.ItemArray().Length - 1
                            '                If row.Item(i).ToString = "" Then
                            '                    row.Item(i) = 0
                            '                End If
                            '            Next

                            '            Dim litem2_7 As New SwatObject.SwatInput.clsMgtItem2(row)
                            '            litem2_7.MGT_OP = 7
                            '            litem2_7.HI_OVR = 0.9
                            '            litem2_7.HARVEFF = 0.0
                            '            lzSwatInput.Mgt.Add2(litem2_7)

                            '            row("HUSC") = 1.201
                            '            Dim litem2_8 As New SwatObject.SwatInput.clsMgtItem2(row)
                            '            litem2_8.MGT_OP = 8
                            '            lzSwatInput.Mgt.Add2(litem2_8)

                            '            'ltab.Rows.Remove(row)
                            '            lzSwatInput.DeleteRowInputDB("MGT2", "OID", OID)

                            '        End If
                            '    End If
                            'Next

                            'change harvest index
                            lzSwatInput.UpdateSWATGDB("crop", "ICNUM = 19 OR ICNUM = 98 OR ICNUM = 99 OR ICNUM = 100 OR ICNUM = 101", "HVSTI", "0.9")
                        End If

                        ''Clean up mgt2
                        ''Dim ldt As DataTable = lzSwatInput.QueryInputDB("SELECT COUNT(*) AS Expr1 FROM mgt2 WHERE (((mgt2.MONTH) Is Not Null));")
                        ''Dim ldt1 As DataTable = lzSwatInput.QueryInputDB("SELECT COUNT(*) AS Expr2 FROM mgt2 WHERE (((mgt2.HUSC) Is Not Null));")
                        'Dim ldt As DataTable = lzSwatInput.QueryInputDB("SELECT COUNT(*) AS Expr1 FROM mgt2 WHERE ([MONTH] Is Not Null);")
                        'Dim ldt1 As DataTable = lzSwatInput.QueryInputDB("SELECT COUNT(*) AS Expr2 FROM mgt2 WHERE (HUSC Is Not Null);")

                        'Dim lSQL As String = String.Empty
                        ''lzSwatInput.Close()
                        'If Integer.Parse(ldt.Rows(0).Item(0).ToString) <= Integer.Parse(ldt1.Rows(0).Item(0).ToString) / 2 Then
                        '    'Doesn't seems that setting anything to Null would work through an OleDB connection here
                        '    'lSQL = "UPDATE mgt2 SET MONTH = Null WHERE MONTH Is Null"
                        '    'lzSwatInput.UpdateInputDB("mgt2", "MONTH Is Not Null", "MONTH", "")
                        '    'lzSwatInput.UpdateInputDB("mgt2", "DAY Is Not Null", "DAY", "")

                        '    'lSQL = "UPDATE mgt2 SET MONTH = Null WHERE MONTH Is Not Null;"
                        '    'Try
                        '    '    Dim lConnection As New ADODB.Connection()
                        '    '    lConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lzOutGDB
                        '    '    lConnection.Open()

                        '    '    Dim lcmd As New ADODB.Command()
                        '    '    lcmd.CommandText = lSQL
                        '    '    lcmd.ActiveConnection = lConnection
                        '    '    lcmd.Execute()

                        '    'Catch ex As Exception
                        '    '    Logger.Msg(ex.Message)
                        '    'End Try

                        '    'lzSwatInput.UpdateInputDB("mgt2", "MONTH LIKE ''", "MONTH", "0")
                        '    'lzSwatInput.UpdateInputDB("mgt2", "DAY LIKE ''", "DAY", "0")

                        '    lzSwatInput.UpdateInputDB("mgt2", "[MONTH] Is Not Null", "[MONTH]", "")
                        '    lzSwatInput.UpdateInputDB("mgt2", "[DAY] Is Not Null", "[DAY]", "")

                        'Else
                        '    lSQL = "UPDATE mgt2 SET HUSC = Null WHERE HUSC Is Not Null"
                        '    lzSwatInput.UpdateInputDB("mgt2", "HUSC Is Not Null", "HUSC", "")


                        'End If

                        'ldt.Clear()
                        'ldt1.Clear()
                        'ldt = Nothing
                        'ldt1 = Nothing

                    End If

                    If pRunModel Then
                        lzSwatInput.SaveAllTextInput()
                        Logger.Dbg("Launching " & pSWATExe & " in " & pOutputFolder)
                        Logger.Flush()
                        LaunchProgram(pSWATExe, pOutputFolder) 'Bypass model run here
                    End If

                    lzSwatInput.Close()

                    '*****************************************************************************
                    'End of Summarize input section and SWAT input creation/Run section
                    '*****************************************************************************
DOSUMMARYONLY:
                    If pOutputSummarize Then
                        'Delete exising .tsbin for sub hru and rch such as to start fresh
                        'Dim di As New System.IO.DirectoryInfo(pReportsFolder)
                        'di.Delete()
                        If IO.Directory.Exists(pReportsFolder) Then
                            For Each lfilename As String In System.IO.Directory.GetFiles(pReportsFolder)
                                If IO.Path.GetExtension(lfilename) = ".tsbin" Then
                                    System.IO.File.Delete(lfilename)
                                End If
                            Next
                        End If
                        SummarizeOutputs()
                    End If

                    If pScenario.Contains("8889") Then ' only do summary once for original scenarios
                        Exit For
                    End If
                    If predoCaliSummary Then
                        If pScenario.Contains("RevPRaccoon") Then 'only for redoing the summary of the calibration runs
                            Exit For
                        End If
                    End If

                Next ' lcase eg RS AFInc AFDec
            Next 'lscen eg ScenExistRevPRaccoon Scen2010RevP8889 etc
            Exit Sub 'Done batch summary, then end the processing here
        End If

        'Below is the same code section for doing individual scenarios one at a time
        If lContinue Then
            Dim lLogFileName As String = Logger.FileName

            pInputFolder = IO.Path.Combine(pBaseFolder, "Scenarios" & IO.Path.DirectorySeparatorChar & pScenario)
            pOutGDBFolder = IO.Path.Combine(pInputFolder, "TablesIn")
            pOutputFolder = IO.Path.Combine(pInputFolder, "TxtInOut")
            pReportsFolder = IO.Path.Combine(pInputFolder, "TablesOut")
            pLogsFolder = IO.Path.Combine(pInputFolder, "logs")

            ChDriveDir(pInputFolder)

            'log for swat runner
            Logger.StartToFile(IO.Path.Combine(pLogsFolder, "SWATRunner.log"), , , True)

            For Each lParmKey As String In lUserParms.Keys
                Logger.Dbg(lParmKey & " = " & lUserParms.ItemByKey(lParmKey))
            Next
            Logger.Flush()

            Dim lOutGDB As String = IO.Path.Combine(pOutGDBFolder, pOutGDB)
            If pRefreshDB OrElse Not IO.File.Exists(lOutGDB) Then 'copy the entire input parameter database for this new scenario
                If IO.File.Exists(lOutGDB) Then
                    Logger.Dbg("DeleteExisting " & lOutGDB)
                    IO.File.Delete(lOutGDB)
                End If
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lOutGDB))
                IO.File.Copy(IO.Path.Combine(pBaseFolder, pOutGDB), lOutGDB)
                Logger.Dbg("Copied " & lOutGDB & " from " & pBaseFolder)
            End If

            Logger.Dbg("InitializeSwatInput")
            Dim lSwatInput As New SwatInput(pSWATGDB, lOutGDB, pBaseFolder, pScenario)

            If pStartYear > 0 Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IYR", pStartYear)
            End If

            If pNumYears > 0 Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "NBYR", pNumYears)
            End If

            If pRunMonthly Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 0)
            ElseIf pRunYearly Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 2)
            ElseIf pRunDaily Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IPRINT", 1)
            End If

            lSwatInput.CIO.PrintHru = False

            If pInputSummarizeBeforeChange Then
                SummarizeInput(lSwatInput, "Before")
            End If

            Dim lTotalArea As Double = 0.0
            Dim lTotalAreaNotConverted As Double = 0.0
            Dim lTotalAreaConverted As Double = 0.0
            Dim lTotalAreaCornFut As Double = 0.0
            Dim lTotalAreaCornNow As Double = 0.0
            Dim lCropChangesSummaryFilename As String = IO.Path.Combine(pLogsFolder, "CropChanges.txt")
            Dim lCropChangesHruFilename As String = IO.Path.Combine(pLogsFolder, "CropHruChanges.txt")
            Dim lCropConversions As New CropConversions("CORNX")

            If pCropChangeSummarize Then ' This switch has to be set to True in order to set the lTotalAreaCornNow
                SummarizeCropChange(lSwatInput, _
                                    lCropConversions, _
                                    lCropChangesSummaryFilename, _
                                    lCropChangesHruFilename, _
                                    lTotalArea, _
                                    lTotalAreaNotConverted, _
                                    lTotalAreaConverted, _
                                    lTotalAreaCornFut, _
                                    lTotalAreaCornNow)
            Else
                For Each lString As String In LinesInFile(lCropChangesSummaryFilename)
                    If lString.StartsWith("Total") Then
                        Dim lFields() As String = lString.Split(vbTab)
                        Double.TryParse(lFields(2), lTotalArea)
                        Double.TryParse(lFields(3), lTotalAreaCornNow)
                        Double.TryParse(lFields(4), lTotalAreaConverted)
                        Double.TryParse(lFields(5), lTotalAreaNotConverted)
                        Double.TryParse(lFields(6), lTotalAreaCornFut)
                    End If
                Next
            End If

            If pCrpFutureColumn > 1 Then
                Dim lCrpChanges As New atcTableDelimited
                lCrpChanges.Delimiter = vbTab
                If lCrpChanges.OpenFile(pCrpFuture) Then
                    Dim lTotalAreaCrp As Double
                    Dim lTotalAreaNotConvertedCrp As Double = 0.0
                    Dim lTotalAreaConvertedCrp As Double = 0.0
                    Dim lTotalAreaCornFutCrp As Double = 0.0
                    Dim lTotalAreaCornNowCrp As Double = 0.0

                    SummarizeCRPChange(lSwatInput, _
                                lCropChangesSummaryFilename & ".crp", _
                                lCropChangesHruFilename & ".crp", _
                                lTotalAreaCrp, _
                                lTotalAreaNotConvertedCrp, _
                                lTotalAreaConvertedCrp, _
                                lTotalAreaCornFutCrp, _
                                lTotalAreaCornNowCrp, _
                                lCrpChanges)
                End If
            End If

            If pChangeCropAreas Then

                'If pChangeCropAreas = True, then pCropChangeSummarize has to be set to True for all except Existing condition
                Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow

                '******************************
                'UMRB: whole basin simulations
                '******************************
                'baseline corn is  ~ 23.64 M Acres
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 10630000 / 247 '2010 12% 247 acres per square kilometer
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 10450000 / 247 '2010 - 12.76% of existing; 247 acres per square kilometer
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 13000000 / 247 '2015 12% 247 acres per square kilometer
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 12640000 / 247 '2015 - 13.55% of existing; 247 acres per square kilometer

                '*****************************************************************
                'UMRB: Raccoon watersheds simulations (sub 88 [2] and 89 [1] only)
                '*****************************************************************

                'ScenExistRevPRaccoon: no landuse change

                '2010: 88 difference in corn before_n_after LU changes: 981.7 sq km 89: 428.56 sq km total is: 1410.26 sq km
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 1410.26 '2010RevP - 12% of existing; already in square kilometer

                '2015: 88 difference in corn before_n_after LU changes: 1204.44 sq km 89: 525.8 sq km total is: 1730.24 sq km
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 1730.24 '2015RevP - 12% of existing; already in square kilometer

                '2020: 88 difference in corn before_n_after LU changes: 1115.75 sq km 89: 487.08 sq km total is: 1602.83 sq km
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 1602.83 '2020RevP - 12% of existing; already in square kilometer

                '2022: 88 difference in corn before_n_after LU changes: 1051.48 sq km 89: 472.12 sq km total is: 1523.6 sq km
                'Dim lDesiredFutureCornArea As Double = lTotalAreaCornNow + 1523.6 '2022RevP - 12% of existing; already in square kilometer

                If pScenario.Contains("Exist") Then
                    ' No landuse change
                ElseIf pScenario.Contains("2010") Then
                    'lDesiredFutureCornArea += 1410.26
                ElseIf pScenario.Contains("2015") Then
                    lDesiredFutureCornArea += 3439.9
                ElseIf pScenario.Contains("2020") Then
                    lDesiredFutureCornArea += 5139.6
                ElseIf pScenario.Contains("2022") Then
                    lDesiredFutureCornArea += 5746.7
                End If

                Logger.Dbg("DesiredFutureCornArea = " & lDesiredFutureCornArea)
                Dim lConvertFractionOfAvailable As Double = (lDesiredFutureCornArea - lTotalAreaCornNow) / (lTotalAreaCornFut - lTotalAreaCornNow)
                Logger.Dbg("ConvertFractionOfAvailable = " & lConvertFractionOfAvailable)
                ChangeHRUfractions(lSwatInput, lCropConversions, lCropChangesHruFilename, lConvertFractionOfAvailable)
            End If

            If pInputSummarizeChanged Then
                SummarizeInput(lSwatInput, "Changed")
            End If

            If IO.File.Exists(pParmChangesTextfile) Then
                Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
                For Each lString As String In LinesInFile(pParmChangesTextfile)
                    Dim lParms() As String = lString.Split(";")
                    lSwatInput.UpdateInputDB(lParms(0).Trim, lParms(1).Trim, lParms(2).Trim, lParms(3).Trim, lParms(4).Trim)
                Next
            End If

            If pRunModel Then
                lSwatInput.SaveAllTextInput()
                Logger.Dbg("Launching " & pSWATExe & " in " & pOutputFolder)
                Logger.Flush()
                LaunchProgram(pSWATExe, pOutputFolder)
            End If

            If pOutputSummarize Then
                'Delete exising .tsbin for sub hru and rch such as to start fresh
                If IO.Directory.Exists(pReportsFolder) Then
                    For Each lfilename As String In System.IO.Directory.GetFiles(pReportsFolder)
                        If IO.Path.GetExtension(lfilename) = ".tsbin" Then
                            System.IO.File.Delete(lfilename)
                        End If
                    Next
                End If
                SummarizeOutputs()
            End If

            'back to basins log
            Logger.StartToFile(lLogFileName, True, False, True)
        End If
        Logger.Msg("SWATRunner finished at " & DateTime.Now, "SWATRunner Message")
    End Sub

    Private Sub SummarizeInput(ByVal aSwatInput As SwatInput, ByVal aSuffix As String)
        Logger.Dbg("SWATSummarizeInput")
        Dim lUniqueLandUses As DataTable = aSwatInput.Hru.UniqueValues("LandUse")
        Dim lStreamWriter As New IO.StreamWriter(IO.Path.Combine(pLogsFolder, "LandUses" & aSuffix & ".txt"))
        For Each lLandUse As DataRow In lUniqueLandUses.Rows
            lStreamWriter.WriteLine(lLandUse.Item(0).ToString)
        Next
        lStreamWriter.Close()

        Dim lLandUSeTable As DataTable = AggregateCrops(aSwatInput.SubBsn.TableWithArea("LandUse"))
        SaveFileString(IO.Path.Combine(pLogsFolder, "AreaLandUseReport" & aSuffix & ".txt"), _
                       SWATArea.Report(lLandUSeTable))
        SaveFileString(IO.Path.Combine(pLogsFolder, "AreaSoilReport" & aSuffix & ".txt"), _
                       SWATArea.Report(aSwatInput.SubBsn.TableWithArea("Soil")))
        SaveFileString(IO.Path.Combine(pLogsFolder, "AreaSlopeCodeReport" & aSuffix & ".txt"), _
                       SWATArea.Report(aSwatInput.SubBsn.TableWithArea("Slope_Cd")))
    End Sub

    Private Sub SummarizeCropChange(ByVal aSwatInput As SwatInput, _
                                    ByVal aCropConversions As CropConversions, _
                                    ByVal aCropChangesSummaryFilename As String, _
                                    ByVal aCropChangesHruFilename As String, _
                                    ByRef aTotalArea As Double, _
                                    ByRef aTotalAreaNotConverted As Double, _
                                    ByRef aTotalAreaConverted As Double, _
                                    ByRef aTotalAreaCornFut As Double, _
                                    ByRef aTotalAreaCornNow As Double)

        Dim lSubBasinToHuc8 As atcCollection = SubBasinToHUC8()

        aTotalArea = 0.0
        aTotalAreaNotConverted = 0.0
        aTotalAreaConverted = 0.0
        aTotalAreaCornFut = 0.0
        aTotalAreaCornNow = 0.0

        Dim lHucOldArea As New atcCollection
        Dim lHucNewArea As New atcCollection

        Dim lSummaryWriter As New IO.StreamWriter(aCropChangesSummaryFilename)
        lSummaryWriter.WriteLine("FrmCrp" & vbTab & "ToCrp" & vbTab _
                               & "Area".PadLeft(12) & vbTab _
                               & "CornNow".PadLeft(12) & vbTab _
                               & "CornChange".PadLeft(12) & vbTab _
                               & "AreaSkip".PadLeft(12) & vbTab _
                               & "CornFuture".PadLeft(12) & vbTab _
                               & "CntPot".PadLeft(8) & vbTab & "CntAct".PadLeft(8))

        Dim lHruWriter As New IO.StreamWriter(aCropChangesHruFilename)
        lHruWriter.WriteLine("HUC8".PadLeft(8) & vbTab _
                           & "SubId" & vbTab _
                           & "HruOID" & vbTab _
                           & "ToHruOID" & vbTab _
                           & "FrmCrp" & vbTab _
                           & "ToCrp" & vbTab _
                           & "Soil" & vbTab _
                           & "Slope" & vbTab _
                           & "FrcChg".PadLeft(12) & vbTab _
                           & "Area".PadLeft(12) & vbTab _
                           & "AreaNow".PadLeft(12) & vbTab _
                           & "AreaChange".PadLeft(12) & vbTab _
                           & "AreaSkip".PadLeft(12) & vbTab _
                           & "AreaFuture".PadLeft(12))

        Dim lTotalPotentialChangeCount As Integer = 0
        Dim lTotalActualChangeCount As Integer = 0
        Dim lUniqueLandUses As DataTable = aSwatInput.Hru.UniqueValues("LandUse")
        For Each lLandUse As DataRow In lUniqueLandUses.Rows
            Dim lLandUseName As String = lLandUse.Item(0).ToString
            Dim lLandUsesConvertedTo As String = ""

            Logger.Dbg("Process " & lLandUseName)
            Dim lPotentialChangedHrus As DataTable = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lLandUseName & "';")
            Dim lChangedHruCount As Integer = 0
            Dim lCropArea As Double = 0.0
            Dim lCropAreaNotConverted As Double = 0.0
            Dim lCropAreaConverted As Double = 0.0
            Dim lCropAreaCornNow As Double = 0.0
            Dim lCropAreaCornFut As Double = 0.0
            For Each lPotentialChangedHru As DataRow In lPotentialChangedHrus.Rows
                Dim lHruItem As New SwatInput.clsHruItem(lPotentialChangedHru)
                With lHruItem
                    Dim lLandUseConvertsTo As String = ""
                    Dim lHruChangeTo As DataTable = Nothing

                    Dim lCornFractionBefore As Double = 0
                    Dim lCornFractionAfter As Double = 0
                    Dim lConvertFractionNet As Double = 0 ' increase in corn fraction
                    Dim lToHruOID As Integer = -1

                    If aCropConversions.Contains(lLandUseName) Then
                        Dim lCropConversion As CropConversion = aCropConversions.Item(lLandUseName)
                        lCornFractionBefore = lCropConversion.Fraction
                        For Each lConvertToName As String In lCropConversion.NameConvertsTo
                            Dim lCornConvertTo As CropConversion = aCropConversions.Item(lConvertToName)
                            If lCornConvertTo.Fraction > lCropConversion.Fraction Then
                                lHruChangeTo = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lConvertToName & "' AND SOIL='" & .SOIL & "' AND SLOPE_CD='" & .SLOPE_CD & "' AND SUBBASIN=" & .SUBBASIN & ";")
                                If lHruChangeTo.Rows.Count > 0 Then
                                    lLandUseConvertsTo = lConvertToName
                                    lToHruOID = lHruChangeTo.Rows(0).Item(0)
                                    lCornFractionAfter = lCornConvertTo.Fraction
                                    lConvertFractionNet = lCornFractionAfter - lCornFractionBefore
                                    If Not lLandUsesConvertedTo.Contains(lLandUseConvertsTo) Then
                                        lLandUsesConvertedTo &= lLandUseConvertsTo & " "
                                    End If

                                    Exit For 'Found first available conversion, don't look for another
                                End If
                            End If
                        Next
                    End If

                    Dim lSubBasinArea As Double = aSwatInput.QueryInputDB("Select SUB_KM FROM(sub) WHERE SUBBASIN=" & .SUBBASIN & ";").Rows(0).Item(0)
                    Dim lHruArea As Double = lSubBasinArea * .HRU_FR
                    Dim lHruAreaPotentialConvert As Double = lHruArea * lConvertFractionNet
                    Dim lHruAreaNotConverted As Double = 0.0
                    Dim lHruAreaConverted As Double = 0.0
                    Dim lHruAreaCornNow As Double = lHruArea * lCornFractionBefore
                    Dim lHruAreaCornFut As Double = 0.0

                    If lHruChangeTo IsNot Nothing AndAlso lHruChangeTo.Rows.Count > 0 Then
                        lHruAreaConverted = lHruAreaPotentialConvert
                        lChangedHruCount += 1
                        lHruAreaCornFut = lCornFractionAfter * lHruArea
                    Else 'no conversion
                        lHruAreaNotConverted = lHruArea * (1 - lCornFractionBefore)
                        lHruAreaCornFut = lHruAreaCornNow
                    End If
                    Dim lHUC As String = lSubBasinToHuc8.ItemByKey(Format(.SUBBASIN, "0"))
                    lHruWriter.WriteLine(lHUC & vbTab _
                                       & .SUBBASIN & vbTab _
                                       & lPotentialChangedHru.Item(0) & vbTab _
                                       & lToHruOID & vbTab _
                                       & lLandUseName & vbTab _
                                       & lLandUseConvertsTo & vbTab _
                                       & .SOIL & vbTab _
                                       & .SLOPE_CD & vbTab _
                                       & DoubleToString(lConvertFractionNet, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lHruArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lHruAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lHruAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lHruAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lHruAreaCornFut, 12, pFormat, , , 10).PadLeft(12))
                    lHucOldArea.Increment(lHUC, lHruAreaCornNow)
                    lHucNewArea.Increment(lHUC, lHruAreaCornFut)
                    lCropArea += lHruArea
                    lCropAreaConverted += lHruAreaConverted
                    lCropAreaNotConverted += lHruAreaNotConverted
                    lCropAreaCornNow += lHruAreaCornNow
                    lCropAreaCornFut += lHruAreaCornFut
                End With
            Next

            lSummaryWriter.WriteLine(lLandUseName & vbTab & lLandUsesConvertedTo & vbTab _
                                   & DoubleToString(lCropArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                   & DoubleToString(lCropAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                   & DoubleToString(lCropAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                   & DoubleToString(lCropAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                   & DoubleToString(lCropAreaCornFut, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                   & lPotentialChangedHrus.Rows.Count.ToString.PadLeft(8) & vbTab _
                                   & lChangedHruCount.ToString.PadLeft(8))
            lSummaryWriter.Flush()
            aTotalArea += lCropArea
            aTotalAreaConverted += lCropAreaConverted
            aTotalAreaNotConverted += lCropAreaNotConverted
            aTotalAreaCornNow += lCropAreaCornNow
            aTotalAreaCornFut += lCropAreaCornFut
            lTotalPotentialChangeCount += lPotentialChangedHrus.Rows.Count
            lTotalActualChangeCount += lChangedHruCount
            lHruWriter.Flush()
        Next
        lHruWriter.Close()
        lSummaryWriter.WriteLine()
        lSummaryWriter.WriteLine("Total" & vbTab & Space(6) & vbTab _
                               & DoubleToString(aTotalArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaCornFut, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & lTotalPotentialChangeCount.ToString.PadLeft(8) & vbTab _
                               & lTotalActualChangeCount.ToString.PadLeft(8))
        lSummaryWriter.Flush()
        lSummaryWriter.Close()

        'lHucOldArea As New atcCollection
        'lHucNewArea As New atcCollection

        Dim lHucSummaryWriter As New IO.StreamWriter(aCropChangesSummaryFilename & ".HUC")
        lHucSummaryWriter.WriteLine("HUC8" & vbTab & "ExistingCornArea" & vbTab & "PotentialCornArea" & vbTab & "Added" & vbTab & "%Increase")
        For Each lHUC As String In lHucOldArea.Keys
            Dim lOld As Double = lHucOldArea.ItemByKey(lHUC)
            Dim lNew As Double = lHucNewArea.ItemByKey(lHUC)
            lHucSummaryWriter.WriteLine(lHUC & vbTab & DoubleToString(lOld) & vbTab & DoubleToString(lNew) & vbTab & DoubleToString(lNew - lOld) & vbTab & DoubleToString((lNew - lOld) / lOld * 100))
        Next
        lHucSummaryWriter.Flush()
        lHucSummaryWriter.Close()

        Logger.Dbg("AreaTotal " & aTotalArea & " Converted " & aTotalAreaConverted & " NotTotal " & aTotalAreaNotConverted & " CornTotal " & aTotalAreaCornFut)
    End Sub

    Private Sub SummarizeCRPChange(ByVal aSwatInput As SwatInput, _
                                    ByVal aCropChangesSummaryFilename As String, _
                                    ByVal aCropChangesHruFilename As String, _
                                    ByRef aTotalArea As Double, _
                                    ByRef aTotalAreaNotConverted As Double, _
                                    ByRef aTotalAreaConverted As Double, _
                                    ByRef aTotalAreaCornFut As Double, _
                                    ByRef aTotalAreaCornNow As Double, _
                                    ByVal aCrpChanges As atcTable)

        aTotalArea = 0.0
        aTotalAreaNotConverted = 0.0
        aTotalAreaConverted = 0.0
        aTotalAreaCornFut = 0.0
        aTotalAreaCornNow = 0.0

        Dim lSummaryWriter As New IO.StreamWriter(aCropChangesSummaryFilename)
        lSummaryWriter.WriteLine("HUC8".PadLeft(8) & vbTab & "FrmCrp" & vbTab & "ToCrp" & vbTab _
                               & "Area".PadLeft(12) & vbTab _
                               & "AreaNow".PadLeft(12) & vbTab _
                               & "NeedChg".PadLeft(12) & vbTab _
                               & "AreaChange".PadLeft(12) & vbTab _
                               & "AreaSkip".PadLeft(12) & vbTab _
                               & "AreaFuture".PadLeft(12) & vbTab _
                               & "CntPot".PadLeft(8) & vbTab & "CntAct".PadLeft(8))

        Dim lHruWriter As New IO.StreamWriter(aCropChangesHruFilename)
        lHruWriter.WriteLine("HUC8".PadLeft(8) & vbTab _
                           & "SubId" & vbTab _
                           & "FrmCrp" & vbTab _
                           & "ToCrp" & vbTab _
                           & "Soil" & vbTab _
                           & "Slope" & vbTab _
                           & "FrcChg".PadLeft(12) & vbTab _
                           & "Area".PadLeft(12) & vbTab _
                           & "AreaNow".PadLeft(12) & vbTab _
                           & "AreaChange".PadLeft(12) & vbTab _
                           & "AreaSkip".PadLeft(12) & vbTab _
                           & "AreaFuture".PadLeft(12))

        Dim lTotalPotentialChangeCount As Integer = 0
        Dim lTotalActualChangeCount As Integer = 0
        Dim lChangedHruCount As Integer = 0
        Dim lCropArea As Double = 0.0
        Dim lCropAreaNotConverted As Double = 0.0
        Dim lCropAreaConverted As Double = 0.0
        Dim lCropAreaCornNow As Double = 0.0
        Dim lCropAreaCornFut As Double = 0.0
        Dim lTotalNeededChange As Double = 0.0

        Dim lSubBasinToHuc8 As atcCollection = SubBasinToHUC8()

        Dim lLandUseName As String = "CRP"
        Logger.Dbg("Process " & lLandUseName)
        Dim lLandUseConvertsTo As String = "CCCC"
        Dim lCornFractionBefore As Double = 0
        Dim lCornFractionAfter As Double = 1

        Dim lAllSubbasins As DataTable = aSwatInput.QueryInputDB("Select * FROM(sub);")

        For Each lSubBasinRow As DataRow In lAllSubbasins.Rows
            Dim lSubItem As New SwatObject.SwatInput.clsSubBsnItem(lSubBasinRow)
            Dim lSubBasinStr As String = Format(lSubItem.SUBBASIN, "0")
            Dim lHuc8 As String = lSubBasinToHuc8.ItemByKey(lSubBasinStr)
            If Not lHuc8 Is Nothing AndAlso aCrpChanges.FindFirst(1, CInt(lHuc8)) Then
                Dim lBaseCRParea As Double 'CRP area in base condition
                Dim lNewCRParea As Double  'CRP area in future condition from pCrpFutureColumn
                If Double.TryParse(aCrpChanges.Value(2), lBaseCRParea) AndAlso _
                   Double.TryParse(aCrpChanges.Value(pCrpFutureColumn), lNewCRParea) Then
                    lBaseCRParea /= 247 'convert acres to square km
                    lNewCRParea /= 247
                    Dim lCropChangeToMake As Double = lNewCRParea - lBaseCRParea
                    lTotalNeededChange += lCropChangeToMake

                    Dim lPotentialChangedHrus As DataTable = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lLandUseName & "' AND SUBBASIN=" & lSubBasinStr & ";")
                    For Each lPotentialChangedHru As DataRow In lPotentialChangedHrus.Rows
                        Dim lHruItem As New SwatInput.clsHruItem(lPotentialChangedHru)
                        With lHruItem
                            Dim lHruArea As Double = lSubItem.SUB_KM * .HRU_FR
                            Dim lHruAreaPotentialConvert As Double = lHruArea '* lConvertFractionNet
                            Dim lHruAreaNotConverted As Double = 0.0
                            Dim lHruAreaConverted As Double = 0.0
                            Dim lHruAreaCornNow As Double = 0 'lHruArea * lCornFractionBefore
                            Dim lHruAreaCornFut As Double = 0.0

                            Dim lHruChangeTo As DataTable = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lLandUseConvertsTo & "' AND SOIL='" & .SOIL & "' AND SLOPE_CD='" & .SLOPE_CD & "' AND SUBBASIN=" & .SUBBASIN & ";")
                            If lHruChangeTo.Rows.Count > 0 Then
                                lHruAreaConverted = lHruAreaPotentialConvert
                                lChangedHruCount += 1
                                lHruAreaCornFut = lCornFractionAfter * lHruArea
                            Else 'no conversion
                                lHruAreaNotConverted = lHruAreaPotentialConvert
                                lHruAreaCornFut = lCornFractionBefore * lHruArea
                            End If
                            lHruWriter.WriteLine(lHuc8 & vbTab _
                                               & .SUBBASIN & vbTab _
                                               & lLandUseName & vbTab _
                                               & lLandUseConvertsTo & vbTab _
                                               & .SOIL & vbTab _
                                               & .SLOPE_CD & vbTab _
                                               & DoubleToString(1, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                               & DoubleToString(lHruArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                               & DoubleToString(lHruAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                               & DoubleToString(lHruAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                               & DoubleToString(lHruAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                               & DoubleToString(lHruAreaCornFut, 12, pFormat, , , 10).PadLeft(12))
                            lCropArea += lHruArea
                            lCropAreaConverted += lHruAreaConverted
                            lCropAreaNotConverted += lHruAreaNotConverted
                            lCropAreaCornNow += lHruAreaCornNow
                            lCropAreaCornFut += lHruAreaCornFut
                        End With
                    Next

                    lTotalPotentialChangeCount += lPotentialChangedHrus.Rows.Count

                    lSummaryWriter.WriteLine(lHuc8 & vbTab & lLandUseName & vbTab & lLandUseConvertsTo & vbTab _
                       & DoubleToString(lCropArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & DoubleToString(lCropAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & DoubleToString(lCropChangeToMake, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & DoubleToString(lCropAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & DoubleToString(lCropAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & DoubleToString(lCropAreaCornFut, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                       & lPotentialChangedHrus.Rows.Count.ToString.PadLeft(8) & vbTab _
                       & lChangedHruCount.ToString.PadLeft(8))


                End If
            End If
        Next
        aTotalArea += lCropArea
        aTotalAreaConverted += lCropAreaConverted
        aTotalAreaNotConverted += lCropAreaNotConverted
        aTotalAreaCornNow += lCropAreaCornNow
        aTotalAreaCornFut += lCropAreaCornFut
        lTotalActualChangeCount += lChangedHruCount

        lHruWriter.Close()
        lSummaryWriter.WriteLine()
        lSummaryWriter.WriteLine("Total" & vbTab & Space(6) & vbTab & Space(12) & vbTab _
                               & DoubleToString(aTotalArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(lTotalNeededChange, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & DoubleToString(aTotalAreaCornFut, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                               & lTotalPotentialChangeCount.ToString.PadLeft(8) & vbTab _
                               & lTotalActualChangeCount.ToString.PadLeft(8))
        lSummaryWriter.Flush()
        lSummaryWriter.Close()
        Logger.Dbg("AreaTotal " & aTotalArea & " Converted " & aTotalAreaConverted & " NotTotal " & aTotalAreaNotConverted & " CornTotal " & aTotalAreaCornFut)
    End Sub

    Private Sub ChangeHRUfractions(ByVal aSwatInput As SwatInput, _
                                   ByVal aCropConversions As CropConversions, _
                                   ByVal aHruChangesFilename As String, _
                                   ByVal aConvertFractionOfAvailable As Double)
        Dim lFractionOfSubbasinToChange As Double
        Dim lNumChangesMade As Integer = 0
        Dim lToOID As Integer
        For Each lString As String In LinesInFile(aHruChangesFilename)
            Dim lFields() As String = lString.Split(vbTab)
            If Integer.TryParse(lFields(3), lToOID) AndAlso lToOID > -1 Then
                Dim lFromOID As Integer = lFields(2)
                Dim lLandUseName As String = lFields(4)
                Dim lCropConvertFrom As CropConversion = aCropConversions.Item(lLandUseName)

                Dim lHruToChangeFrom As DataTable = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE OID=" & lFromOID & ";")
                Dim lCropConvertTo As CropConversion = aCropConversions.Item(lFields(5))
                Dim lHruChangeTo As DataTable = aSwatInput.QueryInputDB("Select * FROM(hru) WHERE OID=" & lToOID & ";")
                If lHruToChangeFrom.Rows.Count > 0 AndAlso lHruChangeTo.Rows.Count > 0 Then
                    With lHruToChangeFrom.Rows(0) 'remove fraction of this land use
                        Dim lHRU_FR_From As Double = .Item("HRU_FR")
                        lFractionOfSubbasinToChange = lHRU_FR_From * aConvertFractionOfAvailable
                        aSwatInput.UpdateInputDB("hru", "OID", lFromOID, "HRU_FR", lHRU_FR_From - lFractionOfSubbasinToChange)
                    End With
                    With lHruChangeTo.Rows(0) 'add fraction of this land use
                        aSwatInput.UpdateInputDB("hru", "OID", lToOID, "HRU_FR", .Item("HRU_FR") + lFractionOfSubbasinToChange)
                    End With
                    lNumChangesMade += 1
                End If
            End If
        Next
        Logger.Dbg("Moved fraction of subbasin from " & lNumChangesMade & " HRUs.")
    End Sub

    Private Sub SummarizeOutputs()
        MkDirPath(IO.Path.GetFullPath(pReportsFolder))

        Dim lOutputRchSwatFileName As String = IO.Path.Combine(pOutputFolder, "output.rch")
        Dim lRunDate As Date = IO.File.GetLastWriteTime(lOutputRchSwatFileName)
        WriteBinaryDatasetsIfNeeded("rch")
        WriteBinaryDatasetsIfNeeded("sub")

        Dim lOutputFields As New atcData.atcDataAttributes
        'TODO:add nutrient fields
        'AREAkm2  PRECIPmm SNOFALLmm SNOMELTmm     IRRmm     PETmm      ETmm SW_INITmm  SW_ENDmm    PERCmm GW_RCHGmm DA_RCHGmm   REVAPmm  SA_IRRmm  DA_IRRmm   SA_STmm   DA_STmmSURQ_GENmmSURQ_CNTmm   TLOSSmm    LATQmm    GW_Qmm    WYLDmm   DAILYCN 
        'TMP_AVdgC TMP_MXdgC TMP_MNdgCSOL_TMPdgCSOLARMJ/m2  
        'SYLDt/ha  USLEt/ha
        'N_APPkg/haP_APPkg/haNAUTOkg/haPAUTOkg/ha NGRZkg/ha PGRZkg/haNCFRTkg/haPCFRTkg/haNRAINkg/ha NFIXkg/ha F-MNkg/ha A-MNkg/ha A-SNkg/ha F-MPkg/haAO-LPkg/ha L-APkg/ha A-SPkg/ha 
        'DNITkg/ha  NUPkg/ha  PUPkg/ha ORGNkg/ha ORGPkg/ha SEDPkg/ha
        'NSURQkg/haNLATQkg/ha NO3Lkg/haNO3GWkg/ha SOLPkg/ha P_GWkg/ha    W_STRS  TMP_STRS    N_STRS    P_STRS  
        'BIOMt/ha       LAI   YLDt/ha   BACTPct  BACTLPct
        lOutputFields.SetValue("FieldName", "AREAkm2;NAUTOkg/ha;PAUTOkg/ha;NUPkg/ha;PUPkg/ha;YLDt/ha") ' from .hru
        WriteBinaryDatasetsIfNeeded("hru", lOutputFields)

        Logger.Dbg("SwatSummaryReports")
        Dim lSubBasinsOutputFileName As String = IO.Path.Combine(pReportsFolder, "SubBasinSummary.txt")
        Dim lReachOutputFileName As String = IO.Path.Combine(pReportsFolder, "ReachSummary.txt")

        'TODO be sure appropriate attributes are written
        Dim lDisplayAttributesSave As ArrayList = atcDataManager.DisplayAttributes.Clone
        Dim lDisplayAttributes As String() = {"Location", "Mean", "Max", "Min", "Sum", "Constituent"}
        With atcDataManager.DisplayAttributes
            .Clear()
            .AddRange(lDisplayAttributes)
        End With

        Dim lSubBasin2HUC8 As atcCollection = SubBasinToHUC8()
        Dim lSubTimseriesGroup As New atcDataSourceTimeseriesBinary
        lSubTimseriesGroup.Open(IO.Path.Combine(pReportsFolder, "sub.tsbin"))
        WriteSubSummary(pReportsFolder, lSubTimseriesGroup.DataSets, lSubBasin2HUC8, lSubBasinsOutputFileName)
        Logger.Dbg("Report Load Done")

        Dim lRchTimseriesGroup As New atcDataSourceTimeseriesBinary
        lRchTimseriesGroup.Open(IO.Path.Combine(pReportsFolder, "rch.tsbin"))
        WriteReachSummary(pReportsFolder, lRchTimseriesGroup.DataSets, lSubBasin2HUC8, lReachOutputFileName)
        Logger.Dbg("Report Reach Done")

        Dim lHuc2Summary As New atcCollection
        Dim lHuc4Summary As New atcCollection
        Dim lHuc6Summary As New atcCollection
        Dim lHuc8Summary As New atcCollection
        Dim lSubBasinOutputTable As New atcTableDelimited
        With lSubBasinOutputTable
            .Delimiter = vbTab
            .OpenFile(lSubBasinsOutputFileName)
        End With

        Dim lReachOutputTable As New atcTableDelimited
        With lReachOutputTable
            .Delimiter = vbTab
            .OpenFile(lReachOutputFileName)
            .MoveNext()
            For lIndex As Integer = 2 To .NumRecords
                .CurrentRecord = lIndex
                Dim lHuc8 As String = .Value(2)
                lSubBasinOutputTable.CurrentRecord = lIndex
                MakeSummary(lHuc8.Substring(0, 2), lReachOutputTable, lSubBasinOutputTable, lHuc2Summary)
                MakeSummary(lHuc8.Substring(0, 4), lReachOutputTable, lSubBasinOutputTable, lHuc4Summary)
                MakeSummary(lHuc8.Substring(0, 6), lReachOutputTable, lSubBasinOutputTable, lHuc6Summary)
                MakeSummary(lHuc8.Substring(0, 8), lReachOutputTable, lSubBasinOutputTable, lHuc8Summary)
            Next
        End With
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc2Summary.txt"), HucSummaryReport(lHuc2Summary, lRunDate))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc4Summary.txt"), HucSummaryReport(lHuc4Summary, lRunDate))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc6Summary.txt"), HucSummaryReport(lHuc6Summary, lRunDate))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc8Summary.txt"), HucSummaryReport(lHuc8Summary, lRunDate))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc2SummaryEng.txt"), HucSummaryReport(lHuc2Summary, lRunDate, True))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc4SummaryEng.txt"), HucSummaryReport(lHuc4Summary, lRunDate, True))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc6SummaryEng.txt"), HucSummaryReport(lHuc6Summary, lRunDate, True))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc8SummaryEng.txt"), HucSummaryReport(lHuc8Summary, lRunDate, True))

        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc4SummaryTony.txt"), HucSummaryTony(lHuc2Summary, lHuc4Summary, lRunDate, False))
        SaveFileString(IO.Path.Combine(pReportsFolder, "Huc4SummaryTonyEng.txt"), HucSummaryTony(lHuc2Summary, lHuc4Summary, lRunDate, True))
        'Dim lCombinedOutputTable As atcTable = CombineTables(lSubBasinOutputTable, lReachOutputTable, _
        '    "1:1", "1:2", "1:3", "2:3", "1:4", "2:4", "1:5", "2:4-1:5", "2:5", "1:6", "2:6", "1:7", "2:6-1:7", "2:7")
        'lCombinedOutputTable.WriteFile(IO.Path.Combine(pReportsFolder, "SubbasinReach.txt"))

        With atcDataManager.DisplayAttributes
            .Clear()
            .AddRange(lDisplayAttributesSave)
        End With

        Dim lHruTimseriesGroup As New atcDataSourceTimeseriesBinary
        lHruTimseriesGroup.Open(IO.Path.Combine(pReportsFolder, "hru.tsbin"))
        WriteYieldSummary(lSubBasin2HUC8, pReportsFolder, lHruTimseriesGroup.DataSets)

        Logger.Dbg("SwatPostProcessingDone")
    End Sub

    Private Class HucSummary
        Public Name As String
        Public Area As Double
        Public AreaCum As Double
        Public NLoad As Double
        Public NOutflow As Double
        Public PLoad As Double
        Public POutflow As Double
        Public SedLoad As Double
        Public SedOutflow As Double
        Public H2OLoad As Double
        Public H2OOutflow As Double
    End Class

    Private Function HucSummaryTony(ByVal aHucSummary2 As atcCollection, ByVal aHucSummary4 As atcCollection, ByVal aDateOfRun As Date, Optional ByVal aEnglish As Boolean = False) As String
        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("Scenario " & vbTab & pScenario & vbTab & "Date " & vbTab & aDateOfRun)
        lSB.AppendLine("")
        lSB.AppendLine("HUC".PadLeft(8) _
             & vbTab & "Area(Local)".PadLeft(12) _
             & vbTab & "N(LandUnit)".PadLeft(12) _
             & vbTab & "N(LocalLandLoad)".PadLeft(16) _
             & vbTab & "P(LandUnit)".PadLeft(12) _
             & vbTab & "P(LocalLandLoad)".PadLeft(16) _
             & vbTab & "Sed(LandUnit)".PadLeft(14) _
             & vbTab & "Sed(LocalLandLoad)".PadLeft(18))
        If aEnglish Then
            lSB.AppendLine(Space(8) _
             & vbTab & "acres*10^6".PadLeft(12) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "lbs*10^6".PadLeft(16) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "lbs*10^6".PadLeft(16) _
             & vbTab & "tons/acre".PadLeft(14) _
             & vbTab & "tons".PadLeft(18))
        Else
            lSB.AppendLine(Space(8) _
             & vbTab & "ha*10^6".PadLeft(12) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "kg*10^6".PadLeft(16) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "kg*10^6".PadLeft(16) _
             & vbTab & "tonnes/ha".PadLeft(14) _
             & vbTab & "tonnes".PadLeft(18))
        End If

        'TODO: move to a function to be shared with HucSummaryReport
        Dim lAreaFactor As Double = 0.0001 'ha -> km2
        Dim lUnitLoadFactor As Double = 0.01 'kg/km2 -> kg/ha
        Dim lMassFactor As Double = 0.000001 'kg -> kg*10^6
        Dim lSedLoadFactor As Double = 1.0 'tonnes/ha - leave as is
        Dim lSedMassFactor As Double = 1.0 'tonnes - leave as is
        If aEnglish Then
            lAreaFactor = 0.000247 'ha -> acre*10^6
            lUnitLoadFactor = 0.00892 'kg/km2 -> lbs/acre
            lMassFactor = 0.0000022046 'kg -> lbs*10^6
            lSedLoadFactor = 0.445 'tonnes/ha -> tons/ac
            lSedMassFactor = 1.1023 'tonnes -> tons
        End If

        For Each lHucSummary4 As HucSummary In aHucSummary4
            With lHucSummary4
                Dim lNLandUnit As Double = .NLoad / .Area
                Dim lPLandUnit As Double = .PLoad / .Area
                Dim lSedLandUnit As Double = .SedLoad / .Area
                lSB.AppendLine(.Name.PadLeft(8) _
                               & vbTab & DecimalAlign(.Area * lAreaFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lNLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(.NLoad * lMassFactor, 16, 1, 12).PadLeft(16) _
                               & vbTab & DecimalAlign(lPLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(.PLoad * lMassFactor, 16, 1, 12).PadLeft(16) _
                               & vbTab & DecimalAlign(lSedLandUnit * lSedLoadFactor, 14, 3, 8).PadLeft(14) _
                               & vbTab & DecimalAlign(.SedLoad * lSedMassFactor, 18, 1, 12).PadLeft(18))
            End With
        Next
        lSB.AppendLine("")
        Dim lhucsummary2 As HucSummary = aHucSummary2.ItemByIndex(0)
        With lhucsummary2
            Dim lNLandUnit As Double = .NLoad / .Area
            Dim lPLandUnit As Double = .PLoad / .Area
            Dim lSedLandUnit As Double = .SedLoad / .Area
            lSB.AppendLine(("Total " & .Name).PadLeft(8) _
                            & vbTab & DecimalAlign(.Area * lAreaFactor, , 3, 8).PadLeft(12) _
                            & vbTab & DecimalAlign(lNLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                            & vbTab & DecimalAlign(.NLoad * lMassFactor, 16, 1, 12).PadLeft(16) _
                            & vbTab & DecimalAlign(lPLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                            & vbTab & DecimalAlign(.PLoad * lMassFactor, 16, 1, 12).PadLeft(16) _
                            & vbTab & DecimalAlign(lSedLandUnit * lSedLoadFactor, 14, 3, 8).PadLeft(14) _
                            & vbTab & DecimalAlign(.SedLoad * lSedMassFactor, 18, 1, 12).PadLeft(18))
            lSB.AppendLine("Outflow".PadLeft(8) _
                                        & vbTab & " ".PadLeft(8) _
                                        & vbTab & " ".PadLeft(12) _
                                        & vbTab & DecimalAlign(.NOutflow * lMassFactor, 16, 1, 12).PadLeft(16) _
                                        & vbTab & " ".PadLeft(12) _
                                        & vbTab & DecimalAlign(.POutflow * lMassFactor, 16, 1, 12).PadLeft(16) _
                                        & vbTab & " ".PadLeft(14) _
                                        & vbTab & DecimalAlign(.SedOutflow * lSedMassFactor, 18, 1, 12).PadLeft(18))
            lSB.AppendLine(" ")
            lSB.AppendLine("% Removed ".PadLeft(8) _
                                        & vbTab & " ".PadLeft(8) _
                                        & vbTab & " ".PadLeft(12) _
                                        & vbTab & DecimalAlign(100 * (lNLandUnit - (.NOutflow / .AreaCum)) / lNLandUnit, 16, 1, 12).PadLeft(16) _
                                        & vbTab & " ".PadLeft(12) _
                                        & vbTab & DecimalAlign(100 * (lPLandUnit - (.POutflow / .AreaCum)) / lPLandUnit, 16, 1, 12).PadLeft(16) _
                                        & vbTab & " ".PadLeft(14) _
                                        & vbTab & DecimalAlign(100 * (lSedLandUnit - (.SedOutflow / .AreaCum)) / lSedLandUnit, 18, 1, 12).PadLeft(18))
        End With
        Return lSB.ToString
    End Function

    Private Function HucSummaryReport(ByVal aHucSummaryCollection As atcCollection, ByVal aDateOfRun As Date, Optional ByVal aEnglish As Boolean = False) As String
        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("Scenario " & vbTab & pScenario & vbTab & "Date " & vbTab & aDateOfRun)
        lSB.AppendLine("")
        lSB.AppendLine("HUC".PadLeft(8) _
             & vbTab & "Area(Local)".PadLeft(12) _
             & vbTab & "Area(Total)".PadLeft(12) _
             & vbTab & "N(LandUnit)".PadLeft(12) _
             & vbTab & "N(OutflowUnit)".PadLeft(12) _
             & vbTab & "N(Removed)".PadLeft(12) _
             & vbTab & "N(LocalLandTotal)".PadLeft(16) _
             & vbTab & "N(OutflowTotal)".PadLeft(16) _
             & vbTab & "P(LandUnit)".PadLeft(12) _
             & vbTab & "P(OutflowUnit)".PadLeft(12) _
             & vbTab & "P(Removed)".PadLeft(12) _
             & vbTab & "P(LocalLandTotal)".PadLeft(16) _
             & vbTab & "P(OutflowTotal)".PadLeft(16)) '_
        '& vbTab & "Sed(LocalUnit)".PadLeft(12) _
        '& vbTab & "Sed(TotalOutflow)".PadLeft(16) _
        '& vbTab & "Sed(TotalUnit)".PadLeft(12) _
        '& vbTab & "H2O(LocalUnit)".PadLeft(12) _
        '& vbTab & "H2O(TotalOutflow)".PadLeft(16) _
        '& vbTab & "H2O(TotalUnit)".PadLeft(12))
        If aEnglish Then
            lSB.AppendLine(Space(8) _
             & vbTab & "acres*10^6".PadLeft(12) _
             & vbTab & "acres*10^6".PadLeft(12) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "%".PadLeft(12) _
             & vbTab & "lbs*10^6".PadLeft(16) _
             & vbTab & "lbs*10^6".PadLeft(16) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "lbs/acre".PadLeft(12) _
             & vbTab & "%".PadLeft(12) _
             & vbTab & "lbs*10^6".PadLeft(16) _
             & vbTab & "lbs*10^6".PadLeft(16)) ' _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(16) _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(16) _
            '& vbTab & "?".PadLeft(12))
        Else
            lSB.AppendLine(Space(8) _
             & vbTab & "ha*10^6".PadLeft(12) _
             & vbTab & "ha*10^6".PadLeft(12) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "%".PadLeft(12) _
             & vbTab & "kg*10^6".PadLeft(16) _
             & vbTab & "kg*10^6".PadLeft(16) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "kg/ha".PadLeft(12) _
             & vbTab & "%".PadLeft(12) _
             & vbTab & "kg*10^6".PadLeft(16) _
             & vbTab & "kg*10^6".PadLeft(16)) ' _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(16) _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(12) _
            '& vbTab & "?".PadLeft(16) _
            '& vbTab & "?".PadLeft(12))
        End If
        For Each lHucSummary As HucSummary In aHucSummaryCollection
            With lHucSummary
                'local loads
                Dim lNLandUnit As Double = .NLoad / .Area
                Dim lNOutflowUnit As Double = .NOutflow / .AreaCum
                Dim lNRemovalPercent As Double = 100 * (lNLandUnit - lNOutflowUnit) / lNLandUnit
                Dim lPLandUnit As Double = .PLoad / .Area
                Dim lPOutflowUnit As Double = .POutflow / .AreaCum
                Dim lPRemovalPercent As Double = 100 * (lPLandUnit - lPOutflowUnit) / lPLandUnit
                'Dim lSedLoadUnit As Double = .SedLoad / .Area
                'Dim lH2OLoadUnit As Double = .H2OLoad / .Area

                'TODO: move to a function to be shared with HucSummaryTony
                Dim lAreaFactor As Double = 0.0001 'ha -> km2
                Dim lUnitLoadFactor As Double = 0.01 'kg/km2 -> kg/ha
                Dim lMassFactor As Double = 0.000001 'kg -> kg*10^6
                If aEnglish Then
                    lAreaFactor = 0.000247 'ha -> acre*10^6
                    lUnitLoadFactor = 0.00892 'kg/km2 -> lbs/acre
                    lMassFactor = 0.0000022046 'kg -> lbs*10^6
                End If

                lSB.AppendLine(.Name.PadLeft(8) _
                               & vbTab & DecimalAlign(.Area * lAreaFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(.AreaCum * lAreaFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lNLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lNOutflowUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lNRemovalPercent, , 1, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(.NLoad * lMassFactor, 16, 3, 12).PadLeft(16) _
                               & vbTab & DecimalAlign(.NOutflow * lMassFactor, 16, 3, 12).PadLeft(16) _
                               & vbTab & DecimalAlign(lPLandUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lPOutflowUnit * lUnitLoadFactor, , 3, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(lPRemovalPercent, , 1, 8).PadLeft(12) _
                               & vbTab & DecimalAlign(.PLoad * lMassFactor, 16, 3, 12).PadLeft(16) _
                               & vbTab & DecimalAlign(.POutflow * lMassFactor, 16, 3, 12).PadLeft(16)) ' _
                '                   & vbTab & DecimalAlign(lSedLoadUnit * 0.00892, , 3, 8).PadLeft(12) _
                '                   & vbTab & DecimalAlign(.SedOutflow / 1000000.0, 16, 3, 12).PadLeft(16) _
                '                   & vbTab & DecimalAlign((.SedOutflow / .AreaCum) * 0.00892, , 3, 8).PadLeft(12) _
                '                   & vbTab & DecimalAlign(lH2OLoadUnit * 0.00892, , 3, 8).PadLeft(12) _
                '                   & vbTab & DecimalAlign(.H2OOutflow / 1000000.0, 16, 3, 12).PadLeft(16) _
                '                   & vbTab & DecimalAlign((.H2OOutflow / .AreaCum) * 0.00892, , 3, 8).PadLeft(12))
            End With
        Next
        Return lSB.ToString
    End Function

    Private Sub MakeSummary(ByVal aHuc As String, _
                            ByVal aReachOutputTable As atcTable, ByVal aSubBasinOutputTable As atcTable, _
                            ByRef aHucSummary As atcCollection)
        Dim lHucSummary As HucSummary
        If aHucSummary.IndexFromKey(aHuc) = -1 Then
            lHucSummary = New HucSummary
            lHucSummary.Name = aHuc
            aHucSummary.Add(aHuc, lHucSummary)
        Else
            lHucSummary = aHucSummary.ItemByKey(aHuc)
        End If
        With lHucSummary
            .Area += aSubBasinOutputTable.Value(3)
            .NLoad += aSubBasinOutputTable.Value(5)
            .PLoad += aSubBasinOutputTable.Value(7)
            .SedLoad += aSubBasinOutputTable.Value(9)
            .H2OLoad += aSubBasinOutputTable.Value(11)
            If .AreaCum < aReachOutputTable.Value(3) Then 'new downstream
                .AreaCum = aReachOutputTable.Value(3)
                .NOutflow = aReachOutputTable.Value(5)
                .POutflow = aReachOutputTable.Value(7)
                .SedOutflow = aReachOutputTable.Value(9)
                .H2OOutflow = aReachOutputTable.Value(11)
            End If
        End With
    End Sub

    Private Function SubBasinToHUC8() As atcCollection
        Dim lSubBasin2HUC8 As New atcCollection
        Dim lSubBasin2HUC8Table As New atcTableDelimited
        With lSubBasin2HUC8Table
            .Delimiter = ","
            .NumHeaderRows = 0
            .OpenFile(IO.Path.Combine(pBaseFolder, "flowfig.csv"))
            For lRowIndex As Integer = 1 To .NumRecords
                lSubBasin2HUC8.Add(.Value(5), .Value(7))
                .MoveNext()
            Next
            .Clear()
        End With
        Return lSubBasin2HUC8
    End Function

    Private Function HUC8toSubBasin() As atcCollection
        Dim lHUC8toSubBasin As New atcCollection
        Dim lTable As New atcTableDelimited
        With lTable
            .Delimiter = ","
            .NumHeaderRows = 0
            .OpenFile(IO.Path.Combine(pBaseFolder, "flowfig.csv"))
            For lRowIndex As Integer = 1 To .NumRecords
                lHUC8toSubBasin.Add(.Value(7), .Value(5))
                .MoveNext()
            Next
            .Clear()
        End With
        Return lHUC8toSubBasin
    End Function

    Private Sub WriteBinaryDatasetsIfNeeded(ByVal aOutputType As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing)
        Dim lSwatOutputFileName As String = IO.Path.Combine(pOutputFolder, "output." & aOutputType)
        Dim lRunDate As Date = IO.File.GetLastWriteTime(lSwatOutputFileName)
        Dim lBinaryOutputFileName As String = IO.Path.Combine(pReportsFolder, aOutputType & ".tsbin")
        If Not FileExists(lBinaryOutputFileName) OrElse _
          lRunDate > IO.File.GetLastWriteTime(lBinaryOutputFileName) Then
            If FileExists(lSwatOutputFileName) Then
                Dim lSwatOutput As New atcTimeseriesSWAT.atcTimeseriesSWAT
                With lSwatOutput
                    If .Open(lSwatOutputFileName, aAttributes) Then
                        Logger.Dbg(aOutputType & "OutputTimserCount " & .DataSets.Count)
                        Dim lDataTarget As New atcDataSourceTimeseriesBinary ' atcDataSourceWDM
                        TryDelete(lBinaryOutputFileName)
                        If lDataTarget.Open(lBinaryOutputFileName, aAttributes) Then
                            lDataTarget.AddDatasets(.DataSets)
                            Logger.Dbg("WroteBinaryDatasetsTo" & lBinaryOutputFileName)
                        End If
                    Else
                        Logger.Dbg("UnableToOpen " & lSwatOutputFileName)
                    End If
                End With
            Else
                Logger.Dbg("Missing " & lSwatOutputFileName & " UnableToWriteBinaryFile")
            End If
        Else
            Logger.Dbg("UsingExisting " & lBinaryOutputFileName)
        End If
    End Sub

    Private Sub WriteSubSummary(ByVal aOutputFolder As String, ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aSubBasin2HUC8 As atcCollection, ByVal aOutputFileName As String)
        Dim lConsNitr As String() = {"ORGN", "NSURQ", "LATNO3", "GWNO3"}
        Dim lConsPhos As String() = {"ORGP", "SOLP", "SEDP"}
        Dim lConsOtherOut As String() = {"AREA", "WYLD", "SYLD"}
        Dim lConsToOutput As New ArrayList
        With lConsToOutput
            .AddRange(lConsNitr)
            .AddRange(lConsPhos)
            .AddRange(lConsOtherOut)
        End With

        Dim lSBHuc8 As New Text.StringBuilder
        lSBHuc8.AppendLine("SubID" & vbTab & "HUC8".PadLeft(8) _
                                   & vbTab & "LocalArea".PadLeft(12) _
                                   & vbTab & "N_Unit_Load".PadLeft(12) & vbTab & "N_HUC_Load".PadLeft(12) _
                                   & vbTab & "P_Unit_Load".PadLeft(12) & vbTab & "P_HUC_Load".PadLeft(12) _
                                   & vbTab & "Sed_Unit_Load".PadLeft(12) & vbTab & "Sed_HUC_Load".PadLeft(16) _
                                   & vbTab & "H2O_Unit_Load".PadLeft(12) & vbTab & "H2O_HUC_Load".PadLeft(16))
        lSBHuc8.AppendLine(" " & vbTab & " ".PadLeft(8) _
                               & vbTab & "km2".PadLeft(12) _
                               & vbTab & "kg/ha".PadLeft(12) & vbTab & "kg".PadLeft(12) _
                               & vbTab & "kg/ha".PadLeft(12) & vbTab & "kg".PadLeft(12) _
                               & vbTab & "tonnes/ha".PadLeft(12) & vbTab & "tonnes".PadLeft(16) _
                               & vbTab & "mm".PadLeft(12) & vbTab & "?".PadLeft(16)) 'mm * ha

        For lIndex As Integer = 1 To aSubBasin2HUC8.Count
            Dim lHuc8 As String = aSubBasin2HUC8.ItemByKey(lIndex.ToString)
            lSBHuc8.Append(lIndex & vbTab & lHuc8)
            Dim lSubData As atcTimeseriesGroup = aTimeseriesGroup.FindData("Location", "BIGSUB" & lIndex.ToString.PadLeft(4))
            Dim lSubDataToList As New atcTimeseriesGroup
            Dim lTimserNitr As New atcTimeseriesGroup
            Dim lTimserPhos As New atcTimeseriesGroup
            Dim lHucAreaFactor As Double = 0.0

            For Each lDataSet As atcTimeseries In lSubData
                Dim lCons As String = lDataSet.Attributes.GetValue("Constituent")
                If lConsToOutput.Contains(lCons) Then
                    lSubDataToList.Add(lDataSet)
                    If lCons = "AREA" Then
                        Dim lHucArea As Double = lDataSet.Attributes.GetDefinedValue("Mean").Value
                        lSBHuc8.Append(vbTab & DecimalAlign(lHucArea))
                        lHucAreaFactor = lHucArea * 100 'km -> ha
                    End If
                    If Array.IndexOf(lConsNitr, lCons) > -1 Then
                        lTimserNitr.Add(lDataSet)
                    End If
                    If Array.IndexOf(lConsPhos, lCons) > -1 Then
                        lTimserPhos.Add(lDataSet)
                    End If
                End If
            Next

            If lTimserNitr.Count > 0 Then
                Dim lTimserNitrUnitLoad As atcTimeseries = Compute("Add", lTimserNitr)
                lTimserNitrUnitLoad.Attributes.SetValue("Constituent", "UnitN_Load")
                lSubDataToList.Add(lTimserNitrUnitLoad)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserNitrUnitLoad.Attributes.GetDefinedValue("Mean").Value))

                Dim lTimserNitrHucLoad As atcTimeseries = Compute("Multiply", lTimserNitrUnitLoad, lHucAreaFactor)
                lTimserNitrHucLoad.Attributes.SetValue("Constituent", "HucN_Load")
                lSubDataToList.Add(lTimserNitrHucLoad)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserNitrHucLoad.Attributes.GetDefinedValue("Mean").Value))
            End If

            If lTimserPhos.Count > 0 Then
                Dim lTimserPhosUnitLoad As atcTimeseries = Compute("Add", lTimserPhos)
                lTimserPhosUnitLoad.Attributes.SetValue("Constituent", "UnitP_Load")
                lSubDataToList.Add(lTimserPhosUnitLoad)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserPhosUnitLoad.Attributes.GetDefinedValue("Mean").Value))

                Dim lTimserPhosHucLoad As atcTimeseries = Compute("Multiply", lTimserPhosUnitLoad, lHucAreaFactor)
                lTimserPhosHucLoad.Attributes.SetValue("Constituent", "HucP_Load")
                lSubDataToList.Add(lTimserPhosHucLoad)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserPhosHucLoad.Attributes.GetDefinedValue("Mean").Value))
            End If

            Dim lFind As atcTimeseriesGroup
            Dim lTimserUnitLoad As atcTimeseries
            Dim lTimserLoad As atcTimeseries
            lFind = lSubDataToList.FindData("Constituent", "SYLD")
            If lFind.Count > 0 Then
                lTimserUnitLoad = lFind.ItemByIndex(0)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserUnitLoad.Attributes.GetDefinedValue("Mean").Value))
                lTimserLoad = Compute("Multiply", lTimserUnitLoad, lHucAreaFactor)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserLoad.Attributes.GetDefinedValue("Mean").Value, 16))
            End If

            'H2O
            lFind = lSubDataToList.FindData("Constituent", "WYLD")
            If lFind.Count > 0 Then
                lTimserUnitLoad = lFind.ItemByIndex(0)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserUnitLoad.Attributes.GetDefinedValue("Mean").Value))
                lTimserLoad = Compute("Multiply", lTimserUnitLoad, lHucAreaFactor)
                lSBHuc8.Append(vbTab & DecimalAlign(lTimserLoad.Attributes.GetDefinedValue("Mean").Value, 16))
            End If

            lSBHuc8.AppendLine()

            Dim lOutputFilenameHuc As String = lHuc8 & "_" & lIndex & "_Sub.txt"
            Dim lList As New atcListPlugin
            'TODO: just output year
            lList.Save(lSubDataToList, IO.Path.Combine(aOutputFolder, lOutputFilenameHuc), "DateFormatIncludeYears")
        Next
        SaveFileString(aOutputFileName, lSBHuc8.ToString)
    End Sub

    Private Sub WriteReachSummary(ByVal aOutputFolder As String, ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aSubBasin2HUC8 As atcCollection, ByVal aOutputFileName As String)
        Dim lConsNitrOut As String() = {"NH4_OUT", "NO2_OUT", "NO3_OUT", "ORGN_OUT"}
        Dim lConsNitrIn As String() = {"NH4_IN", "NO2_IN", "NO3_IN", "ORGN_IN"}
        Dim lConsPhosOut As String() = {"ORGP_OUT", "MINP_OUT"}
        Dim lConsPhosIn As String() = {"ORGP_IN", "MINP_IN"}
        Dim lConsOtherIn As String() = {"FLOW_IN", "SED_IN"}
        Dim lConsOtherOut As String() = {"AREA", "FLOW_OUT", "SED_OUT"}
        Dim lConsToOutput As New ArrayList
        With lConsToOutput
            .AddRange(lConsNitrOut)
            .AddRange(lConsNitrIn)
            .AddRange(lConsPhosOut)
            .AddRange(lConsPhosIn)
            .AddRange(lConsOtherOut)
            .AddRange(lConsOtherIn)
        End With

        Dim lSBHuc8 As New Text.StringBuilder
        lSBHuc8.AppendLine("SubID" & vbTab & "HUC8" & vbTab & "CumArea".PadLeft(12) _
                                                    & vbTab & "N_Total_In".PadLeft(16) & vbTab & "N_Total_Out".PadLeft(16) _
                                                    & vbTab & "P_Total_In".PadLeft(16) & vbTab & "P_Total_Out".PadLeft(16) _
                                                    & vbTab & "Sed_Total_In".PadLeft(16) & vbTab & "Sed_Total_Out".PadLeft(16) _
                                                    & vbTab & "H2O_Total_In".PadLeft(16) & vbTab & "H2O_Total_Out".PadLeft(16))
        lSBHuc8.AppendLine(vbTab & vbTab & "km2".PadLeft(12) _
                                 & vbTab & "kg".PadLeft(16) & vbTab & "kg".PadLeft(16) _
                                 & vbTab & "kg".PadLeft(16) & vbTab & "kg".PadLeft(16) _
                                 & vbTab & "tonnes".PadLeft(16) & vbTab & "tonnes".PadLeft(16) _
                                 & vbTab & "m3/s".PadLeft(16) & vbTab & "m3/s".PadLeft(16))

        For lIndex As Integer = 1 To aSubBasin2HUC8.Count
            Dim lHuc8 As String = aSubBasin2HUC8.ItemByKey(lIndex.ToString)
            lSBHuc8.Append(lIndex & vbTab & lHuc8)
            Dim lReachData As atcTimeseriesGroup = aTimeseriesGroup.FindData("Location", "REACH" & lIndex.ToString.PadLeft(5))
            Dim lReachDataToList As New atcTimeseriesGroup
            Dim lTimserNitrOut As New atcTimeseriesGroup
            Dim lTimserNitrIn As New atcTimeseriesGroup
            Dim lTimserPhosOut As New atcTimeseriesGroup
            Dim lTimserPhosIn As New atcTimeseriesGroup
            Dim lAreaFactor As Double = 1.0

            For Each lDataSet As atcTimeseries In lReachData
                Dim lCons As String = lDataSet.Attributes.GetValue("Constituent")
                If lConsToOutput.Contains(lCons) Then
                    lReachDataToList.Add(lDataSet)
                    If lCons = "AREA" Then
                        Dim lHucCumArea As Double = lDataSet.Attributes.GetDefinedValue("Mean").Value
                        lSBHuc8.Append(vbTab & DecimalAlign(lHucCumArea))
                        lAreaFactor = lHucCumArea * 100 'km -> ha
                    End If
                    If Array.IndexOf(lConsNitrOut, lCons) > -1 Then
                        lTimserNitrOut.Add(lDataSet)
                    End If
                    If Array.IndexOf(lConsNitrIn, lCons) > -1 Then
                        lTimserNitrIn.Add(lDataSet)
                    End If
                    If Array.IndexOf(lConsPhosOut, lCons) > -1 Then
                        lTimserPhosOut.Add(lDataSet)
                    End If
                    If Array.IndexOf(lConsPhosIn, lCons) > -1 Then
                        lTimserPhosIn.Add(lDataSet)
                    End If
                End If
            Next

            Dim lTimserNitrTotalIn As atcTimeseries = Compute("Add", lTimserNitrIn)
            lTimserNitrTotalIn.Attributes.SetValue("Constituent", "TotalN_In")
            lReachDataToList.Add(lTimserNitrTotalIn)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimserNitrTotalIn.Attributes.GetDefinedValue("Mean").Value, 16))
            Dim lTimserNitrTotalOut As atcTimeseries = Compute("Add", lTimserNitrOut)
            lTimserNitrTotalOut.Attributes.SetValue("Constituent", "TotalN_Out")
            lReachDataToList.Add(lTimserNitrTotalOut)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimserNitrTotalOut.Attributes.GetDefinedValue("Mean").Value, 16))

            Dim lTimserPhosTotalIn As atcTimeseries = Compute("Add", lTimserPhosIn)
            lTimserPhosTotalIn.Attributes.SetValue("Constituent", "TotalP_In")
            lReachDataToList.Add(lTimserPhosTotalIn)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimserPhosTotalIn.Attributes.GetDefinedValue("Mean").Value, 16))
            Dim lTimserPhosTotalOut As atcTimeseries = Compute("Add", lTimserPhosOut)
            lTimserPhosTotalOut.Attributes.SetValue("Constituent", "TotalP_Out")
            lReachDataToList.Add(lTimserPhosTotalOut)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimserPhosTotalOut.Attributes.GetDefinedValue("Mean").Value, 16))

            'sediment
            Dim lTimser As atcTimeseries = lReachDataToList.FindData("Constituent", "SED_IN").ItemByIndex(0)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimser.Attributes.GetDefinedValue("Mean").Value, 16))
            lTimser = lReachDataToList.FindData("Constituent", "SED_Out").ItemByIndex(0)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimser.Attributes.GetDefinedValue("Mean").Value, 16))

            'H2O
            lTimser = lReachDataToList.FindData("Constituent", "FLOW_IN").ItemByIndex(0)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimser.Attributes.GetDefinedValue("Mean").Value, 16))
            lTimser = lReachDataToList.FindData("Constituent", "FLOW_OUT").ItemByIndex(0)
            lSBHuc8.Append(vbTab & DecimalAlign(lTimser.Attributes.GetDefinedValue("Mean").Value, 16))

            lSBHuc8.AppendLine()

            Dim lOutputFilenameHuc As String = lHuc8 & "_" & lIndex & ".txt"
            Dim lList As New atcListPlugin
            'TODO: just output year
            lList.Save(lReachDataToList, IO.Path.Combine(aOutputFolder, lOutputFilenameHuc), "DateFormatIncludeYears")
        Next
        SaveFileString(aOutputFileName, lSBHuc8.ToString)
    End Sub

    Private Sub WriteYieldSummary(ByVal aSubBasin2huc8 As atcCollection, _
                                  ByVal aOutputFolder As String, _
                                  ByVal aTimeseriesGroup As atcTimeseriesGroup)
        Dim lCropIds As New atcCollection
        With lCropIds
            .Add("CORN") : .Add("CCCC") : .Add("CSC1") : .Add("CSS1") : .Add("CCS1")
        End With

        Dim lTab As String = vbTab
        Dim lFieldWidth As Integer = 12
        Dim lSigDigits As Integer = 8
        Dim lArea As Double = 0.0
        Dim lUnitNUptk As Double = 0.0
        Dim lUnitPUptk As Double = 0.0
        Dim lUnitNApp As Double = 0.0
        Dim lUnitPApp As Double = 0.0
        Dim lUnitYield As Double = 0.0

        Dim lSBAreaDebug As New Text.StringBuilder
        lSBAreaDebug.AppendLine("SubId" & lTab & _
                                "HruId" & lTab & _
                                "Crop" & lTab & _
                                "Area".PadLeft(lFieldWidth) & lTab & _
                                "Fraction".PadLeft(lFieldWidth))
        Dim lAreaGroup As atcTimeseriesGroup = aTimeseriesGroup.FindData("Constituent", "AREA")
        Dim lSubIds As atcCollection = lAreaGroup.SortedAttributeValues("SubId")
        Dim lSubIdAreas As New atcCollection
        For Each lSubId As String In lSubIds
            Dim lAreaSubIdTotal As Double = 0.0
            Dim lSubIdDataGroup As atcTimeseriesGroup = lAreaGroup.FindData("SubId", lSubId)
            Dim lHruIds As atcCollection = lSubIdDataGroup.SortedAttributeValues("HruId")
            Dim lAreaStrings As New atcCollection
            Dim lunitLine As String = Space(5) & lTab & Space(6) & lTab & Space(4) & lTab
            Dim lunit As String = ""
            For Each lHruId As String In lHruIds
                Dim lHruIdDataGroup As atcTimeseriesGroup = lSubIdDataGroup.FindData("HruId", lHruId)
                Dim lAreaUsed As Boolean = False
                For Each lAreaTimeseries As atcTimeseries In lHruIdDataGroup
                    lArea = lAreaTimeseries.Value(1)
                    lunit = lAreaTimeseries.Attributes.GetValue("Units")
                    If Double.IsNaN(lArea) Then
                        'skip
                    ElseIf Not lAreaUsed Then
                        lAreaStrings.Add(lHruId, lSubId & lTab & _
                                                 lAreaTimeseries.Attributes.GetValue("HruId") & lTab & _
                                                 lAreaTimeseries.Attributes.GetValue("CropId") & lTab & _
                                                 DecimalAlign(lArea, , , lSigDigits))
                        lAreaSubIdTotal += lArea
                        lAreaUsed = True
                    Else
                        Logger.Dbg("Problem " & lHruId)
                    End If
                Next
            Next

            lunitLine &= lunit.PadLeft(lFieldWidth) & lTab & Space(8).PadLeft(lFieldWidth)
            lSBAreaDebug.AppendLine(lunitLine)
            For Each lAreaString As String In lAreaStrings
                lArea = lAreaString.Substring(lAreaString.LastIndexOf(lTab))
                lSBAreaDebug.AppendLine(lAreaString & lTab & DecimalAlign(lArea / lAreaSubIdTotal, , 10, lSigDigits))
            Next
            lSubIdAreas.Add(lSubId, lAreaSubIdTotal)
        Next
        SaveFileString(IO.Path.Combine(aOutputFolder, "Area.txt"), lSBAreaDebug.ToString)

        Dim lMatchingDataGroup As atcTimeseriesGroup = aTimeseriesGroup.FindData("CropId", lCropIds)
        'Get the first date from the first matching timeseries, e.g 1961/1/1
        Dim lTimserBase As atcTimeseries = lMatchingDataGroup.Item(0)
        Dim lDateBase(5) As Integer
        J2Date(lTimserBase.Dates.Value(0), lDateBase)
        Dim lNumValues As Integer = lTimserBase.numValues

        Dim lSBDebug As New Text.StringBuilder
        lSBDebug.AppendLine("SubId" & lTab & _
                            "Crop" & lTab & lTab & _
                            "Year" & lTab & _
                            "Area".PadLeft(lFieldWidth) & lTab & _
                            "UnitYield".PadLeft(lFieldWidth) & lTab & _
                            "Yield".PadLeft(lFieldWidth) & lTab & _
                            "UnitNAutoApp".PadLeft(lFieldWidth) & lTab & _
                            "N Auto App".PadLeft(lFieldWidth) & lTab & _
                            "Unit N Uptk".PadLeft(lFieldWidth) & lTab & _
                            "N Uptake".PadLeft(lFieldWidth) & lTab & _
                            "UnitPAutoApp".PadLeft(lFieldWidth) & lTab & _
                            "P Auto App".PadLeft(lFieldWidth) & lTab & _
                            "Unit P Uptk".PadLeft(lFieldWidth) & lTab & _
                            "P Uptake".PadLeft(lFieldWidth))
        'lSBDebug.AppendLine(Space(5) & lTab & _
        '                    Space(4).PadLeft(8) & lTab & _
        '                    Space(4) & lTab & _
        '                    "km2".PadLeft(lFieldWidth) & lTab & _
        '                    "kg/ha".PadLeft(lFieldWidth) & lTab & _
        '                    "kg".PadLeft(lFieldWidth))
        Dim lSBAverage As New Text.StringBuilder
        lSBAverage.AppendLine("SubId" & lTab _
                            & "HUC8" & lTab & lTab _
                            & "Area".PadLeft(lFieldWidth) & lTab _
                            & "CornArea".PadLeft(lFieldWidth) & lTab _
                            & "%".PadLeft(lFieldWidth) & lTab _
                            & "UnitYield".PadLeft(lFieldWidth) & lTab _
                            & "Yield".PadLeft(lFieldWidth) & lTab _
                            & "UnitNAutoApp".PadLeft(lFieldWidth) & lTab _
                            & "N Auto App".PadLeft(lFieldWidth) & lTab _
                            & "Unit N Uptk".PadLeft(lFieldWidth) & lTab _
                            & "N Uptake".PadLeft(lFieldWidth) & lTab _
                            & "UnitPAutoApp".PadLeft(lFieldWidth) & lTab _
                            & "P Auto App".PadLeft(lFieldWidth) & lTab _
                            & "Unit P Uptk".PadLeft(lFieldWidth) & lTab _
                            & "P Uptake".PadLeft(lFieldWidth))
        Dim lSBAnnual As New Text.StringBuilder
        lSBAnnual.AppendLine("SubId" & lTab _
                           & "HUC8" & lTab & lTab _
                           & "Year" & lTab _
                           & "Area".PadLeft(lFieldWidth) & lTab & _
                             "CornArea".PadLeft(lFieldWidth) & lTab & _
                             "%".PadLeft(lFieldWidth) & lTab & _
                             "UnitYield".PadLeft(lFieldWidth) & lTab & _
                             "Yield".PadLeft(lFieldWidth) & lTab & _
                             "UnitNAutoApp".PadLeft(lFieldWidth) & lTab & _
                             "N Auto App".PadLeft(lFieldWidth) & lTab & _
                             "Unit N Uptk".PadLeft(lFieldWidth) & lTab & _
                             "N Uptake".PadLeft(lFieldWidth) & lTab & _
                             "UnitPAutoApp".PadLeft(lFieldWidth) & lTab & _
                             "P Auto App".PadLeft(lFieldWidth) & lTab & _
                             "Unit P Uptk".PadLeft(lFieldWidth) & lTab & _
                             "P Uptake".PadLeft(lFieldWidth))
        Dim lAreaAllTotal As Double = 0.0
        Dim lAreaTotal As Double = 0.0
        Dim lYieldTotal As Double = 0.0
        Dim lNAppTotal As Double = 0.0
        Dim lNUptkTotal As Double = 0.0
        Dim lPAppTotal As Double = 0.0
        Dim lPUptkTotal As Double = 0.0

        'UnitLines
        Dim lSBDebugUnitLine As String = Space(5) & lTab & Space(4).PadLeft(8) & lTab & Space(4) & lTab 'SubID\tCrop\t\Year\t
        Dim lSBAverageUnitLine As String = Space(5) & lTab & Space(4).PadLeft(8) & lTab 'SubID\tHUC8\t
        Dim lSBAnnualUnitLine As String = Space(5) & lTab & Space(4).PadLeft(8) & lTab & Space(4) & lTab 'SubID\tHUC8\t\Year\t

        'Units
        Dim lAreaUnit As String = ""
        Dim lYLDUnit As String = ""
        Dim lNAUTOUnit As String = ""
        Dim lNUPUnit As String = ""
        Dim lPAUTOUnit As String = ""
        Dim lPUPUnit As String = ""

        Dim lYieldSummaryHuc8 As New atcCollection

        Dim ldoneUnitLine As Boolean = False
        For Each lSubId As String In lSubIds
            Dim lHuc8 As String = aSubBasin2huc8.ItemByKey(lSubId.Trim)
            Dim lSubIdDataGroup As atcTimeseriesGroup = lMatchingDataGroup.FindData("SubId", lSubId) 'TSs for all targeted Crops (e.g. CCCC) in a given subbasin
            Dim lLocationIdsInSub As atcCollection = lSubIdDataGroup.SortedAttributeValues("Location")
            Dim lNUptkSum As Double = 0.0
            Dim lNAppSum As Double = 0.0
            Dim lPUptkSum As Double = 0.0
            Dim lPAppSum As Double = 0.0
            Dim lYieldSum As Double = 0.0
            Dim lAreaSum As Double = 0.0

            If Not ldoneUnitLine Then 'Only get the units for the first set of timeseries, assuming they are all the same as raw SWAT outputs' units
                'Find the units
                lAreaUnit = lSubIdDataGroup.FindData("Constituent", "AREA").Item(0).Attributes.GetValue("Units")
                lNAUTOUnit = lSubIdDataGroup.FindData("Constituent", "NAUTO").Item(0).Attributes.GetValue("Units")
                lNUPUnit = lSubIdDataGroup.FindData("Constituent", "NUP").Item(0).Attributes.GetValue("Units")
                lPAUTOUnit = lSubIdDataGroup.FindData("Constituent", "PAUTO").Item(0).Attributes.GetValue("Units")
                lPUPUnit = lSubIdDataGroup.FindData("Constituent", "PUP").Item(0).Attributes.GetValue("Units")
                lYLDUnit = lSubIdDataGroup.FindData("Constituent", "YLD").Item(0).Attributes.GetValue("Units")

                Dim lYLDAmtUnit As String = lYLDUnit.Substring(0, lYLDUnit.LastIndexOf("/"))
                Dim lNAUTOAmtUnit As String = lNAUTOUnit.Substring(0, lNAUTOUnit.LastIndexOf("/"))
                Dim lNUPAmtUnit As String = lNUPUnit.Substring(0, lNUPUnit.LastIndexOf("/"))
                Dim lPAUTOAmtUnit As String = lPAUTOUnit.Substring(0, lPAUTOUnit.LastIndexOf("/"))
                Dim lPUPAmtUnit As String = lPUPUnit.Substring(0, lPUPUnit.LastIndexOf("/"))

                'Construct unit lines
                lSBDebugUnitLine &= lAreaUnit.PadLeft(lFieldWidth) & lTab & _
                                lYLDUnit.PadLeft(lFieldWidth) & lTab & _
                                lYLDAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                lNAUTOUnit.PadLeft(lFieldWidth) & lTab & _
                                lNAUTOAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                lNUPUnit.PadLeft(lFieldWidth) & lTab & _
                                lNUPAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                lPAUTOUnit.PadLeft(lFieldWidth) & lTab & _
                                lPAUTOAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                lPUPUnit.PadLeft(lFieldWidth) & lTab & _
                                lPUPAmtUnit.PadLeft(lFieldWidth)

                lSBAverageUnitLine &= lAreaUnit.PadLeft(lFieldWidth) & lTab _
                                & lAreaUnit.PadLeft(lFieldWidth) & lTab _
                                & "%".PadLeft(lFieldWidth) & lTab _
                                & lYLDUnit.PadLeft(lFieldWidth) & lTab _
                                & lYLDAmtUnit.PadLeft(lFieldWidth) & lTab _
                                & lNAUTOUnit.PadLeft(lFieldWidth) & lTab _
                                & lNAUTOAmtUnit.PadLeft(lFieldWidth) & lTab _
                                & lNUPUnit.PadLeft(lFieldWidth) & lTab _
                                & lNUPAmtUnit.PadLeft(lFieldWidth) & lTab _
                                & lPAUTOUnit.PadLeft(lFieldWidth) & lTab _
                                & lPAUTOAmtUnit.PadLeft(lFieldWidth) & lTab _
                                & lPUPUnit.PadLeft(lFieldWidth) & lTab _
                                & lPUPAmtUnit.PadLeft(lFieldWidth)

                lSBAnnualUnitLine &= lAreaUnit.PadLeft(lFieldWidth) & lTab & _
                                 lAreaUnit.PadLeft(lFieldWidth) & lTab & _
                                 "%".PadLeft(lFieldWidth) & lTab & _
                                 lYLDUnit.PadLeft(lFieldWidth) & lTab & _
                                 lYLDAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                 lNAUTOUnit.PadLeft(lFieldWidth) & lTab & _
                                 lNAUTOAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                 lNUPUnit.PadLeft(lFieldWidth) & lTab & _
                                 lNUPAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                 lPAUTOUnit.PadLeft(lFieldWidth) & lTab & _
                                 lPAUTOAmtUnit.PadLeft(lFieldWidth) & lTab & _
                                 lPUPUnit.PadLeft(lFieldWidth) & lTab & _
                                 lPUPAmtUnit.PadLeft(lFieldWidth)

                ldoneUnitLine = True
            End If

            'Append to various StringBuilders to the beginning of each subbasin printout
            'lSBDebug.AppendLine(lSBDebugUnitLine)
            'lSBAverage.AppendLine(lSBAverageUnitLine)
            'lSBAnnual.AppendLine(lSBAnnualUnitLine)

            Dim lYear As Integer = lDateBase(0)
            Dim lSubIdArea As Double = lSubIdAreas.ItemByKey(lSubId)
            lAreaAllTotal += lSubIdArea
            For lYearIndex As Integer = 1 To lNumValues
                Dim lAreaSub As Double = 0
                Dim lNAppSub As Double = 0
                Dim lNUptkSub As Double = 0
                Dim lPAppSub As Double = 0
                Dim lPUptkSub As Double = 0
                Dim lYieldSub As Double = 0
                For Each lLocationId As String In lLocationIdsInSub
                    Dim lLocationIdDataGroup As atcTimeseriesGroup = lSubIdDataGroup.FindData("Location", lLocationId) 'TSs for One Hru (e.g. CCCC  115) in a given subbasin
                    If lLocationIdDataGroup.Count = 6 Then ' The six chosen constituents, no unit conversion (SWAT output unit)
                        Dim lAreaTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "AREA").Item(0)
                        Dim lNAppliedTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "NAUTO").Item(0)
                        Dim lNUptkTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "NUP").Item(0)
                        Dim lPAppliedTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "PAUTO").Item(0)
                        Dim lPUptkTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "PUP").Item(0)
                        Dim lYieldTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "YLD").Item(0)

                        If lYearIndex <= lAreaTimser.numValues Then
                            lArea = lAreaTimser.Value(lYearIndex)
                            lUnitNApp = lNAppliedTimser.Value(lYearIndex)
                            lUnitNUptk = lNUptkTimser.Value(lYearIndex)
                            lUnitPApp = lPAppliedTimser.Value(lYearIndex)
                            lUnitPUptk = lPUptkTimser.Value(lYearIndex)
                            lUnitYield = lYieldTimser.Value(lYearIndex)
                        Else
                            lArea = GetNaN()
                            lUnitNApp = GetNaN()
                            lUnitNUptk = GetNaN()
                            lUnitPApp = GetNaN()
                            lUnitPUptk = GetNaN()
                            lUnitYield = GetNaN()
                        End If
                        Dim lYield As Double = lUnitYield * lArea
                        Dim lNApp As Double = lUnitNApp * lArea
                        Dim lNUptk As Double = lUnitNUptk * lArea
                        Dim lPApp As Double = lUnitPApp * lArea
                        Dim lPUptk As Double = lUnitPUptk * lArea
                        lSBDebug.AppendLine(lSubId.Trim & lTab & _
                                            lLocationId & lTab & _
                                            lYear & lTab & _
                                            DecimalAlign(lArea) & lTab & _
                                            DecimalAlign(lUnitYield) & lTab & _
                                            DecimalAlign(lYield) & lTab & _
                                            DecimalAlign(lUnitNApp) & lTab & _
                                            DecimalAlign(lNApp) & lTab & _
                                            DecimalAlign(lUnitNUptk) & lTab & _
                                            DecimalAlign(lNUptk) & lTab & _
                                            DecimalAlign(lUnitPApp) & lTab & _
                                            DecimalAlign(lPApp) & lTab & _
                                            DecimalAlign(lUnitPUptk) & lTab & _
                                            DecimalAlign(lPUptk))
                        If Not Double.IsNaN(lArea) Then
                            lYieldSub += lYield
                            lAreaSub += lArea
                            lNAppSub += lNApp
                            lNUptkSub += lNUptk
                            lPAppSub += lPApp
                            lPUptkSub += lPUptk
                        End If
                    Else
                        Logger.Dbg("Problem:" & lLocationIdDataGroup.Count)
                    End If
                Next
                lSBAnnual.AppendLine(lSubId.Trim & lTab _
                                   & lHuc8 & lTab _
                                   & lYear & lTab _
                                   & DecimalAlign(lSubIdArea) & lTab _
                                   & DecimalAlign(lAreaSub) & lTab _
                                   & DecimalAlign(100 * lAreaSub / lSubIdArea, , 1) & lTab _
                                   & DecimalAlign(lYieldSub / lAreaSub) & lTab _
                                   & DecimalAlign(lYieldSub) & lTab _
                                   & DecimalAlign(lNAppSub / lAreaSub) & lTab _
                                   & DecimalAlign(lNAppSub) & lTab _
                                   & DecimalAlign(lNUptkSub / lAreaSub) & lTab _
                                   & DecimalAlign(lNUptkSub) & lTab _
                                   & DecimalAlign(lPAppSub / lAreaSub) & lTab _
                                   & DecimalAlign(lPAppSub) & lTab _
                                   & DecimalAlign(lPUptkSub / lAreaSub) & lTab _
                                   & DecimalAlign(lPUptkSub))
                lNAppSum += lNAppSub
                lNUptkSum += lNUptkSub
                lPAppSum += lPAppSub
                lPUptkSum += lPUptkSub
                lYieldSum += lYieldSub
                lAreaSum += lAreaSub
                lYear += 1
            Next
            Dim lAreaAvg As Double = lAreaSum / lNumValues
            Dim lNAppAvg As Double = lNAppSum / lNumValues
            Dim lNUptkAvg As Double = lNUptkSum / lNumValues
            Dim lPAppAvg As Double = lPAppSum / lNumValues
            Dim lPUptkAvg As Double = lPUptkSum / lNumValues
            Dim lYieldAvg As Double = lYieldSum / lNumValues
            Dim lYieldSummary As New YieldSummary
            With lYieldSummary
                .Huc = lHuc8
                .Area = lSubIdArea
                .AreaCorn = lAreaAvg
                .Yield = lYieldAvg
                .NApp = lNAppAvg
                .NUptk = lNUptkAvg
                .PApp = lPAppAvg
                .PUptk = lPUptkAvg
            End With
            lYieldSummaryHuc8.Add(lHuc8, lYieldSummary)
            lSBAverage.AppendLine(lSubId.Trim & lTab _
                                & lHuc8 & lTab _
                                & DecimalAlign(lSubIdArea) & lTab _
                                & DecimalAlign(lAreaAvg) & lTab _
                                & DecimalAlign(100 * lAreaAvg / lSubIdArea, , 1) & lTab _
                                & DecimalAlign(lYieldAvg / lAreaAvg) & lTab _
                                & DecimalAlign(lYieldAvg) & lTab _
                                & DecimalAlign(lNAppAvg / lAreaAvg) & lTab _
                                & DecimalAlign(lNAppAvg) & lTab _
                                & DecimalAlign(lNUptkAvg / lAreaAvg) & lTab _
                                & DecimalAlign(lNUptkAvg) & lTab _
                                & DecimalAlign(lPAppAvg / lAreaAvg) & lTab _
                                & DecimalAlign(lPAppAvg) & lTab _
                                & DecimalAlign(lPUptkAvg / lAreaAvg) & lTab _
                                & DecimalAlign(lPUptkAvg))
            lAreaTotal += lAreaAvg
            lYieldTotal += lYieldAvg
            lNAppTotal += lNAppAvg
            lNUptkTotal += lNUptkAvg
            lPAppTotal += lPAppAvg
            lPUptkTotal += lPUptkAvg
        Next

        'huc4 report
        Dim lYieldSummaryHuc4 As New atcCollection
        For Each lYieldSummary8 As YieldSummary In lYieldSummaryHuc8
            Dim lHuc4 = lYieldSummary8.Huc.Substring(0, 4)
            Dim lYieldSummary4 As YieldSummary
            If lYieldSummaryHuc4.IndexFromKey(lHuc4) = -1 Then
                lYieldSummary4 = New YieldSummary
                lYieldSummary4.Huc = lHuc4
                lYieldSummaryHuc4.Add(lHuc4, lYieldSummary4)
            Else
                lYieldSummary4 = lYieldSummaryHuc4.ItemByKey(lHuc4)
            End If
            With lYieldSummary4
                .Area += lYieldSummary8.Area
                .AreaCorn += lYieldSummary8.AreaCorn
                .Yield += lYieldSummary8.Yield
                .NApp += lYieldSummary8.NApp
                .NUptk += lYieldSummary8.NUptk
                .PApp += lYieldSummary8.PApp
                .PUptk += lYieldSummary8.PUptk
            End With
        Next
        Dim lSBAverage4 As New Text.StringBuilder
        lSBAverage4.AppendLine("HUC4" & lTab _
                             & "Area".PadLeft(lFieldWidth) & lTab _
                             & "CornArea".PadLeft(lFieldWidth) & lTab _
                             & "%".PadLeft(lFieldWidth) & lTab _
                             & "UnitYield".PadLeft(lFieldWidth) & lTab _
                             & "Yield".PadLeft(lFieldWidth) & lTab & _
                             "UnitNAutoApp".PadLeft(lFieldWidth) & lTab & _
                             "N Auto App".PadLeft(lFieldWidth) & lTab & _
                             "Unit N Uptk".PadLeft(lFieldWidth) & lTab & _
                             "N Uptake".PadLeft(lFieldWidth) & lTab & _
                             "UnitPAutoApp".PadLeft(lFieldWidth) & lTab & _
                             "P Auto App".PadLeft(lFieldWidth) & lTab & _
                             "Unit P Uptk".PadLeft(lFieldWidth) & lTab & _
                             "P Uptake".PadLeft(lFieldWidth))
        For Each lYieldSummary As YieldSummary In lYieldSummaryHuc4
            With lYieldSummary
                lSBAverage4.AppendLine(.Huc _
                             & lTab & DecimalAlign(.Area) _
                             & lTab & DecimalAlign(.AreaCorn) _
                             & lTab & DecimalAlign(100 * .AreaCorn / .Area, , 1) _
                             & lTab & DecimalAlign(.Yield / .AreaCorn) _
                             & lTab & DecimalAlign(.Yield) _
                             & lTab & DecimalAlign(.NApp / .AreaCorn) _
                             & lTab & DecimalAlign(.NApp) _
                             & lTab & DecimalAlign(.NUptk / .AreaCorn) _
                             & lTab & DecimalAlign(.NUptk) _
                             & lTab & DecimalAlign(.PApp / .AreaCorn) _
                             & lTab & DecimalAlign(.PApp) _
                             & lTab & DecimalAlign(.PUptk / .AreaCorn) _
                             & lTab & DecimalAlign(.PUptk))
            End With
        Next
        SaveFileString(IO.Path.Combine(aOutputFolder, "AverageHuc4.txt"), lSBAverage4.ToString)

        Dim lSBTotal As New Text.StringBuilder
        lSBTotal.AppendLine("Area".PadLeft(lFieldWidth) & lTab & _
                            "CornArea".PadLeft(lFieldWidth) & lTab & _
                            "%".PadLeft(lFieldWidth) & lTab & _
                            "UnitYield".PadLeft(lFieldWidth) & lTab & _
                            "Yield".PadLeft(lFieldWidth) & lTab & _
                            "UnitNAutoApp".PadLeft(lFieldWidth) & lTab & _
                            "N Auto App".PadLeft(lFieldWidth) & lTab & _
                            "Unit N Uptk".PadLeft(lFieldWidth) & lTab & _
                            "N Uptake".PadLeft(lFieldWidth) & lTab & _
                            "UnitPAutoApp".PadLeft(lFieldWidth) & lTab & _
                            "P Auto App".PadLeft(lFieldWidth) & lTab & _
                            "Unit P Uptk".PadLeft(lFieldWidth) & lTab & _
                            "P Uptake".PadLeft(lFieldWidth))
        lSBTotal.AppendLine(DecimalAlign(lAreaAllTotal) & lTab & _
                            DecimalAlign(lAreaTotal) & lTab & _
                            DecimalAlign(100 * lAreaTotal / lAreaAllTotal, , 1) & lTab & _
                            DecimalAlign(lYieldTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lYieldTotal) & lTab & _
                            DecimalAlign(lNAppTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lNAppTotal) & lTab & _
                            DecimalAlign(lNUptkTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lNUptkTotal) & lTab & _
                            DecimalAlign(lPAppTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lPAppTotal) & lTab & _
                            DecimalAlign(lPUptkTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lPUptkTotal))
        SaveFileString(IO.Path.Combine(aOutputFolder, "Debug.txt"), lSBDebug.ToString)
        SaveFileString(IO.Path.Combine(aOutputFolder, "Annual.txt"), lSBAnnual.ToString)
        SaveFileString(IO.Path.Combine(aOutputFolder, "Average.txt"), lSBAverage.ToString)
        SaveFileString(IO.Path.Combine(aOutputFolder, "Total.txt"), lSBTotal.ToString)
    End Sub

    Private Class YieldSummary
        Public Huc As String
        Public Area As Double
        Public AreaCorn As Double
        Public NApp As Double
        Public PApp As Double
        Public NUptk As Double
        Public PUptk As Double
        Public Yield As Double
    End Class

End Module

Module SWATArea
    Public Function Report(ByVal aReportTable As DataTable) As String

        With aReportTable
            Dim lAreaTotals(.Columns.Count) As Double
            Dim lSb As New Text.StringBuilder
            Dim lStr As String = ""
            For lColumnIndex As Integer = 0 To .Columns.Count - 1
                lStr &= .Columns(lColumnIndex).ColumnName.PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            For lRowIndex As Integer = 0 To .Rows.Count - 1
                Dim lReportRow As DataRow = .Rows(lRowIndex)
                With lReportRow
                    lStr = .Item(0).ToString.PadLeft(12) & vbTab
                    For lColumnIndex As Integer = 1 To .ItemArray.GetUpperBound(0)
                        lStr &= DoubleToString(.Item(lColumnIndex), 12, pFormat, , , 10).PadLeft(12) & vbTab
                        lAreaTotals(lColumnIndex) += .Item(lColumnIndex)
                    Next
                End With
                lSb.AppendLine(lStr.TrimEnd(vbTab))
            Next
            lStr = "Totals".PadLeft(12) & vbTab
            For lColumnIndex As Integer = 1 To .Columns.Count - 1
                lStr &= DoubleToString(lAreaTotals(lColumnIndex), 12, pFormat, , , 10).PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            Logger.Dbg("AreaTotalReportComplete " & lAreaTotals(1))
            Return lSb.ToString
        End With
    End Function

    'Public Function AggregateCrops(ByVal aInputTable As DataTable) As DataTable
    '    Dim lCornConversions As New CropConversions("CORN")
    '    Dim lSoybConversions As New CropConversions("SOYB")
    '    Dim lArea As Double = 0.0

    '    Dim lOutputTable As DataTable = aInputTable.Copy
    '    Dim lCornColumnIndex As Integer = lOutputTable.Columns.Count
    '    lOutputTable.Columns.Add("CORN")
    '    Dim lSoybColumnIndex As Integer = lOutputTable.Columns.Count
    '    lOutputTable.Columns.Add("SOYB")

    '    For Each lRow As DataRow In lOutputTable.Rows
    '        lRow(lCornColumnIndex) = 0.0
    '        lRow(lSoybColumnIndex) = 0.0
    '        For lColumnIndex As Integer = 2 To lOutputTable.Columns.Count - 2
    '            Dim lColumnName As String = lOutputTable.Columns(lColumnIndex).ColumnName
    '            If lCornConversions.Contains(lColumnName) Then
    '                lRow(lCornColumnIndex) += CDbl(lRow(lColumnIndex)) * lCornConversions.Item(lColumnName).Fraction
    '            End If
    '            If lSoybConversions.Contains(lColumnName) Then
    '                lRow(lSoybColumnIndex) += CDbl(lRow(lColumnIndex)) * lSoybConversions.Item(lColumnName).Fraction
    '            End If
    '        Next
    '    Next
    '    Return lOutputTable
    'End Function

    Public Function AggregateCrops(ByVal aInputTable As DataTable) As DataTable
        Dim lCropConversions As New CropConversions("CORN")
        Dim lArea As Double = 0.0

        Dim lOutputTable As DataTable = aInputTable.Copy
        Dim lCornColumnIndex As Integer = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("CORN")
        Dim lSoybColumnIndex As Integer = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("SOYB")

        For Each lRow As DataRow In lOutputTable.Rows
            lRow(lCornColumnIndex) = 0.0
            lRow(lSoybColumnIndex) = 0.0
            For lColumnIndex As Integer = 2 To lOutputTable.Columns.Count - 2
                Dim lColumnName As String = lOutputTable.Columns(lColumnIndex).ColumnName
                If lCropConversions.Contains(lColumnName) Then
                    Dim lCropConversion As CropConversion = lCropConversions.Item(lColumnName)
                    lArea = lRow(lColumnIndex)
                    lRow(lCornColumnIndex) += lArea * lCropConversion.Fraction
                    lRow(lSoybColumnIndex) += lArea * (1 - lCropConversion.Fraction)
                End If
            Next
        Next
        Return lOutputTable
    End Function

    'TODO: fix soybeans!

    'Friend Class CropConversions
    '    Inherits KeyedCollection(Of String, CropConversion)
    '    Protected Overrides Function GetKeyForItem(ByVal aParm As CropConversion) As String
    '        Return aParm.Name
    '    End Function

    '    Public Sub New(ByVal aCropToName As String)
    '        Select Case aCropToName
    '            Case "CRP"
    '                Me.Add(New CropConversion("AGRR", 0.0, "CRP"))
    '                Me.Add(New CropConversion("ALFA", 0.0, "CRP"))
    '                Me.Add(New CropConversion("HAY", 0.0, "CRP"))
    '                Me.Add(New CropConversion("PAST", 0.0, "CRP"))
    '                Me.Add(New CropConversion("RNGE", 0.0, "CRP"))
    '                Me.Add(New CropConversion("CRP", 1.0, "CRP"))
    '            Case "CORN"
    '                'NOTE: do not remove or comment out lines, just edit the list of what can be converted to
    '                'All crops that include corn must be included here for area summary report to work
    '                Me.Add(New CropConversion("CCCC", 1.0, "CCCC"))
    '                Me.Add(New CropConversion("CCS1", 0.66667, "CCCC"))
    '                Me.Add(New CropConversion("CSC1", 0.5, "CCCC"))
    '                Me.Add(New CropConversion("CSS1", 0.33333, "CCCC"))
    '                Me.Add(New CropConversion("SCC1", 0.66667, "CCCC"))
    '                Me.Add(New CropConversion("SCS1", 0.5, "CCCC"))
    '                Me.Add(New CropConversion("SSC1", 0.33333, "CCCC"))
    '                Me.Add(New CropConversion("SSSC", 0.0))
    '                Me.Add(New CropConversion("AGRR", 0.0, "CCCC", "CSC1", "SCS1"))
    '                Me.Add(New CropConversion("CRP", 0.0, "CCCC", "CSC1", "SCS1"))
    '                Me.Add(New CropConversion("HAY", 0.0, "CCCC", "CSC1", "SCS1"))
    '            Case "SOYB"
    '                'All crops that include soybeans must be included here for area summary report to work
    '                Me.Add(New CropConversion("CCS1", 0.33333))
    '                Me.Add(New CropConversion("CSC1", 0.5))
    '                Me.Add(New CropConversion("CSS1", 0.66667))
    '                Me.Add(New CropConversion("SCC1", 0.33333))
    '                Me.Add(New CropConversion("SCS1", 0.5))
    '                Me.Add(New CropConversion("SSC1", 0.66667))
    '                Me.Add(New CropConversion("SSSC", 1.0))
    '        End Select
    '    End Sub
    'End Class

    Friend Class CropConversions
        Inherits KeyedCollection(Of String, CropConversion)
        Protected Overrides Function GetKeyForItem(ByVal aParm As CropConversion) As String
            Return aParm.Name
        End Function

        Public Sub New(ByVal aCropToName As String)
            If aCropToName = "CRP" Then
                Me.Add(New CropConversion("AGRR", 0.0, "CRP"))
                Me.Add(New CropConversion("ALFA", 0.0, "CRP"))
                Me.Add(New CropConversion("HAY", 0.0, "CRP"))
                Me.Add(New CropConversion("PAST", 0.0, "CRP"))
                Me.Add(New CropConversion("RNGE", 0.0, "CRP"))
                Me.Add(New CropConversion("CRP", 1.0, "CRP"))
            ElseIf aCropToName = "CORN" Then
                '"CCCC", "CCS1", "SCC1", "CSC1", "SCS1", "CSS1", "SSC1"
                Me.Add(New CropConversion("CCCC", 1.0, "CCCC"))
                Me.Add(New CropConversion("CCS1", 0.66667, "CCCC"))
                Me.Add(New CropConversion("CSC1", 0.5, "CCCC")) 'TODO: check
                Me.Add(New CropConversion("CSS1", 0.33333, "CCCC"))
                Me.Add(New CropConversion("SCC1", 0.66667, "CCCC"))
                Me.Add(New CropConversion("SCS1", 0.5, "CCCC"))  'TODO: check
                Me.Add(New CropConversion("SSC1", 0.33333, "CCCC"))
                'Me.Add(New CropConversion("SSSC", 0.0, "CCCC"))
                Me.Add(New CropConversion("AGRR", 0.0, "CCCC", "CSC1", "SCS1"))
                Me.Add(New CropConversion("CRP", 0.0, "CCCC", "CSC1", "SCS1"))
                Me.Add(New CropConversion("HAY", 0.0, "CCCC", "CSC1", "SCS1"))
            ElseIf aCropToName = "CORNX" Then
                '"CCCC", "CCS1", "SCC1", "CSC1", "SCS1", "CSS1", "SSC1"
                Me.Add(New CropConversion("CCCC", 0.0, "CSC1", "SCS1"))
                Me.Add(New CropConversion("CSC1", 0.5, "CSC1"))
                Me.Add(New CropConversion("SCS1", 0.5, "SCS1"))
                Me.Add(New CropConversion("CCS1", 0.66667, "CCS1"))
                Me.Add(New CropConversion("CSS1", 0.33333, "CSS1"))
                Me.Add(New CropConversion("SCC1", 0.66667, "SCC1"))
                Me.Add(New CropConversion("SSC1", 0.33333, "SSC1"))
            End If
        End Sub
    End Class

    Friend Class CropConversion
        Public Name As String
        Public Fraction As Double
        Public NameConvertsTo As Generic.List(Of String)

        Public Sub New(ByVal aName As String, ByVal aFraction As Double, ByVal ParamArray aNameConvertsTo() As String)
            Name = aName
            Fraction = aFraction
            NameConvertsTo = New Generic.List(Of String)
            NameConvertsTo.AddRange(aNameConvertsTo)
        End Sub
    End Class

    Function CombineTables(ByVal ParamArray aArgs() As Object) As atcTable
        Dim lNewTable As New atcTableArray
        Dim lExistingTables As New Generic.List(Of IatcTable)
        Dim lNewColumnSpecs As New Generic.List(Of String)
        For Each lArg As Object In aArgs
            Dim lArgType As Type = lArg.GetType
            If lArgType.GetInterface("IatcTable") IsNot Nothing Then
                lExistingTables.Add(lArg)
            Else
                lNewColumnSpecs.Add(CStr(lArg))
            End If
        Next
        lNewTable.NumFields = lNewColumnSpecs.Count
        Dim lColumnSpec As String
        Dim lOldTableIndex As Integer
        Dim lOldColumnIndex As Integer
        Dim lNewColumnIndex As Integer = 0

        'populate field names
        For Each lColumnSpec In lNewColumnSpecs
            lNewColumnIndex += 1
            lNewTable.FieldName(lNewColumnIndex) = ""
            While lColumnSpec.Length > 0
                lOldTableIndex = StrFirstInt(lColumnSpec)
                If lColumnSpec.StartsWith(":") Then
                    lColumnSpec = lColumnSpec.Substring(1)
                    lOldColumnIndex = StrFirstInt(lColumnSpec)
                    lNewTable.FieldName(lNewColumnIndex) &= lExistingTables(lOldTableIndex - 1).FieldName(lOldColumnIndex)

                    If lColumnSpec.Length > 0 Then 'math with next part of column spec
                        lNewTable.FieldName(lNewColumnIndex) &= lColumnSpec.Substring(0, 1)
                        lColumnSpec = lColumnSpec.Substring(1)
                    End If
                End If
            End While
        Next

        'populate values
        For lRecord As Integer = 1 To lExistingTables(0).NumRecords
            lNewTable.CurrentRecord = lRecord
            For Each lOldTable As atcTable In lExistingTables
                lOldTable.CurrentRecord = lRecord
            Next

            For Each lColumnSpec In lNewColumnSpecs
                lNewColumnIndex += 1
                While lColumnSpec.Length > 0
                    Dim lOperator As String = lColumnSpec.Substring(0, 1)
                    If Not IsNumeric(lOperator) Then lColumnSpec = lColumnSpec.Substring(1)
                    lOldTableIndex = StrFirstInt(lColumnSpec)
                    If lColumnSpec.StartsWith(":") Then
                        lColumnSpec = lColumnSpec.Substring(1)
                        lOldColumnIndex = StrFirstInt(lColumnSpec)
                        Dim lOldValue As Double = lExistingTables(lOldTableIndex - 1).Value(lOldColumnIndex)
                        Select Case lColumnSpec.Substring(0, 1)
                            Case "+"
                                lNewTable.Value(lNewColumnIndex) = CDbl(lNewTable.Value(lNewColumnIndex)) + lOldValue
                            Case "-"
                                lNewTable.Value(lNewColumnIndex) = CDbl(lNewTable.Value(lNewColumnIndex)) - lOldValue
                            Case "*"
                                lNewTable.Value(lNewColumnIndex) = CDbl(lNewTable.Value(lNewColumnIndex)) * lOldValue
                            Case "/"
                                lNewTable.Value(lNewColumnIndex) = CDbl(lNewTable.Value(lNewColumnIndex)) / lOldValue
                            Case Else
                                lNewTable.Value(lNewColumnIndex) = lOldValue
                        End Select
                    End If
                End While
            Next
        Next

        Return lNewTable
    End Function



End Module
