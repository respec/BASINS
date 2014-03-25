Imports System
Imports atcUtility
Imports atcData
Imports atcUCI
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph
Imports System.Diagnostics.Process
Imports MapWindow.Interfaces
Imports System.Collections.Specialized
Imports atcManDelin
Imports System.IO
Imports System.Text
Imports atcHspfBinOut
Module SensitivityAndUncertaintyAnalysis
    Private pTestpath, pBaseName, pParameterFile, lstr As String
    Private pHSPFExe As String = "C:\Basins41\models\HSPF\bin\WinHspfLt.exe"
    Private lExpertSystem As HspfSupport.atcExpertSystem
    Private lWdmFileName As String = pTestpath & "\" & pBaseName & ".wdm"
    Private lHSPFBinaryFile As String = pTestpath & "\" & pBaseName & ".hbn"
    Private lWdmDataSource As New atcWDM.atcDataSourceWDM()
    Private lBinaryDataSource As New atcTimeseriesFileHspfBinOut()
    Public uciName, ExpertStatsOutputLine, WQOutputLine, BinaryOutputLine As String
    Public pOper, oTable, oParameter As String 'pOper for Operation, oTable is for operation Table, oParameter is the name of the parameter
    Public pOperNumber As Integer
    Public pValue As Single 'pValue is the parameter value
    Public YearsofSimulation As Integer
    Dim obsAverageAnnualcfs, obsAverageAnnual, obsTenPercentHigh, obsTwentyFivePercentHigh, obsFiftyPercentHigh, obsFiftyPercentLow As Single
    Dim obsTwentyFivePercentLow, obsTenPercentLow, obsFivePercentLow, obsTwoPercentLow As Single
    Public pWaterQuality As Boolean = False
    Public pHydrology As Boolean = False
    Public pOutputFromBinary As Boolean = False
    Public Sensitivity(100, 4) As Object
    Private WQConstituents() As Object = {"TSS", "TW", "TN", "TP"}
    Private WQSites() As Object = {"RCH630", "RCH1000", "RCH635"}
    Public WQConstituent As String
    Private TypeOfAnalysis As String = "Sensitivity"
    'Private TypeOfAnalysis As String = "Uncertainty"
    Private Value As Single = 0.0
    Private lSeasons As New atcSeasonsYearSubset(aStartMonth:=6, aStartDay:=1, aEndMonth:=8, aEndDay:=31)
    Private Parameters(30, 2) As Object

    Private Sub Initialize()

        pTestpath = "C:\BASINS\modelout\IRW\SA\"
        pBaseName = "IRW"
        pHydrology = True
        pWaterQuality = True

        Select Case TypeOfAnalysis
            Case "Sensitivity"
                pParameterFile = "C:\BASINS\modelout\IRW\SA\ParameterValue_Sensitivity.dbf"

            Case "Uncertainty"
                pParameterFile = "C:\BASINS\modelout\IRW\SA\ParameterValues_Uncertainty.dbf"
        End Select

            End Sub
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestpath)
        Dim lDBF As New atcTableDBF

        Dim i As Integer

        Dim lUci As New atcUCI.HspfUci
        Dim dateTimeOfUciFile As String = System.IO.File.GetLastWriteTime(pBaseName & ".uci")
        dateTimeOfUciFile = Format(Year(dateTimeOfUciFile), "00") & "_" & Format(Month(dateTimeOfUciFile), "00") & _
                "_" & Format(Day(dateTimeOfUciFile), "00") & "_" & Format(Hour(dateTimeOfUciFile), "00") & "_" & Format(Minute(dateTimeOfUciFile), "00")
        System.IO.File.Copy(pBaseName & ".uci", pBaseName & dateTimeOfUciFile & ".uci", True) 'Save the original copy of the uci file before altering
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        ExpertStatsOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name, Average Annual (cfs), Average Annual (in),10% High (in), 25% High (in), 50% High (in), 50% Low (in0.), 25% Low (in), 10% Low (in), 5% Low (in), 2% Low (in), Error(%) Annual Average, Error(%) 10% High, Error(%) 25% High, Error(%) 50% High, Error(%) 50% Low, Error(%) 25% Low, Error(%) 10% Low, Error(%) 5% Low, Error(%) 2% Low" & vbCrLf
        IO.File.WriteAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)
        Dim SimID As Integer = 0
        Dim MetSegRec As Integer
        Dim Mon As Integer
        uciName = pBaseName & "temp.uci"
        System.IO.File.Copy(pBaseName & ".uci", uciName, True) 'Saving original uci file as temp uci file
        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file
        YearsofSimulation = lUci.GlobalBlock.YearCount
        pValue = 1
        oTable = "Baseline"
        oParameter = ""
        If pWaterQuality Then
            WQOutputLine = "Water Quality" & vbCrLf
            IO.File.WriteAllText(pBaseName & "_WaterQualityOutput.csv", WQOutputLine)
            
            WQOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name, "

            For i = 0 To WQConstituents.GetUpperBound(0)
                Select Case WQConstituents(i)
                    Case "TSS"
                        WQOutputLine = WQOutputLine & "Sediment (tons/year),"
                    Case "TW"
                        WQOutputLine = WQOutputLine & "Mean Water Temperature (C), " & "Mean Summer Water Temperature (C),"
                    Case "TP"
                        WQOutputLine = WQOutputLine & "Mean Annual TP Load (lbs/yr),"
                    Case "TN"
                        WQOutputLine = WQOutputLine & "Mean Annual TN Load (lbs/yr),"
                End Select
            Next
            WQOutputLine &= vbCrLf
            IO.File.AppendAllText(pBaseName & "_WaterQualityOutput.csv", WQOutputLine)
        End If

        Sensitivity(0, 0) = "Baseline"
        Sensitivity(0, 1) = (0)
        Sensitivity(0, 2) = ""
        Sensitivity(0, 3) = (0)

        If pOutputFromBinary Then
            BinaryOutputLine = "Binary Output" & vbCrLf
            IO.File.WriteAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
            BinaryOutputLine = "SimID,Output Location, PET, CEPE, UZET, LZET, AGWET, BASET, TAET" & vbCrLf
            IO.File.AppendAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
        End If

        ModelRunandReportAnswers(SimID, lUci, uciName) 'Baseline simulation and recording the values
        lUci = Nothing
        lWdmDataSource = Nothing
        Dim j As Integer

        Select Case TypeOfAnalysis
            Case "Sensitivity"
                lDBF.OpenFile(pParameterFile) 'Opening the dbf file that has the parameter values
                'The parameter table will be read from the dbf file
                Dim pValue() As Single = {0.9, 1.1} 'Multipliers to calculate uncertainty
                For i = 1 To lDBF.NumRecords 'Going through the record in the dbf file
                    lDBF.CurrentRecord = i
                    pOper = lDBF.Value(lDBF.FieldNumber("OPERATION"))
                    oTable = lDBF.Value(lDBF.FieldNumber("TABLE"))
                    oParameter = lDBF.Value(lDBF.FieldNumber("PARAMETER"))
                    pValue(0) = lDBF.Value(lDBF.FieldNumber("FACTOR1"))
                    pValue(1) = lDBF.Value(lDBF.FieldNumber("FACTOR2"))
                    For j = 0 To pValue.GetUpperBound(0) 'This loop goes through all the multipliers defined in the pValue object
                        lUci = New atcUCI.HspfUci
                        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the uci file
                        Value = pValue(j)
                        If oTable = "EXTNL" Then
                            For Each lMetSeg As HspfMetSeg In lUci.MetSegs
                                Select Case oParameter
                                    Case "PREC"
                                        MetSegRec = 0
                                    Case "ATEM"
                                        MetSegRec = 2
                                    Case "SOLR"
                                        MetSegRec = 5
                                End Select
                                lMetSeg.MetSegRecs(MetSegRec).MFactP = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactP * pValue(j), 3)
                                'lMetSeg.MetSegRecs(MetSegRec).MFactP = lMetSeg.MetSegRecs(MetSegRec).MFactP * pValue
                                lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR * pValue(j), 3)
                            Next
                        Else
                            For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                If oTable.Contains("MON") And oParameter = "" Then
                                    'When the monthly tables are provided, the value in each month needs to be provided
                                    For Mon = 0 To 11
                                        lOper.Tables(oTable).Parms(Mon).Value = SignificantDigits(lOper.Tables(oTable).Parms(Mon).Value * pValue(j), 2)
                                        'lOper.Tables(oTable).Parms(Mon).Value = lOper.Tables(oTable).Parms(Mon).Value * pValue
                                    Next

                                ElseIf oTable.Contains("SILT-CLAY-PM") Then
                                    For SedimentType As Integer = 15 To 16
                                        'oTable = oTable & "," & SedimentType
                                        lOper.Tables(SedimentType).ParmValue(oParameter) = SignificantDigits(lOper.Tables(SedimentType).ParmValue(oParameter) * pValue(j), 3)
                                    Next
                                Else
                                    lOper.Tables(oTable).ParmValue(oParameter) = SignificantDigits(lOper.Tables(oTable).ParmValue(oParameter) * pValue(j), 3)
                                End If
                            Next
                        End If
                        lUci.Save()
                        SimID += 1
                        'System.IO.File.Copy(uciName, SimID & "-" & uciName, True)
                        ModelRunandReportAnswers(SimID, lUci, uciName)

                        System.IO.File.Copy(pBaseName & ".uci", uciName, True)
                        lUci = Nothing
                        lWdmDataSource = Nothing
                    Next
                Next
                GraphForSensitivty(Sensitivity)
                lUci = Nothing
                lWdmDataSource = Nothing
                lBinaryDataSource = Nothing

            Case "Uncertainty"
                lDBF.OpenFile(pParameterFile) 'Opening the dbf file that has the parameter values
                Dim TotalNumberOfParameters, ParameterNumber, k As Integer
                TotalNumberOfParameters = lDBF.NumFields - 1
                lDBF.CurrentRecord = 1

                For CurrentRecord As Integer = 1 To 3
                    lDBF.CurrentRecord = CurrentRecord
                    For k = 1 To TotalNumberOfParameters

                        Parameters(k - 1, CurrentRecord - 1) = lDBF.Value(k + 1) 'Saving the properties of each parameter in an array

                    Next k
                Next CurrentRecord
                For NumberOfSimulations As Integer = 4 To lDBF.NumRecords 'Going through the records in dbf file and changing them in the uci file
                    lDBF.CurrentRecord = NumberOfSimulations
                    SimID = NumberOfSimulations - 3
                    lUci = New atcUCI.HspfUci
                    lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the uci file
                    'Now the loop starts for changing each parameter at a time
                    For ParameterNumber = 1 To TotalNumberOfParameters
                        pOper = Parameters(ParameterNumber - 1, 0)
                        oTable = Parameters(ParameterNumber - 1, 1)
                        oParameter = Parameters(ParameterNumber - 1, 2)
                        pValue = lDBF.Value(ParameterNumber + 1)

                        If oTable = "EXTNL" Then
                            For Each lMetSeg As HspfMetSeg In lUci.MetSegs
                                Select Case oParameter
                                    Case "PREC"
                                        MetSegRec = 0
                                    Case "ATEM"
                                        MetSegRec = 2
                                    Case "SOLR"
                                        MetSegRec = 5
                                End Select
                                lMetSeg.MetSegRecs(MetSegRec).MFactP = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactP * pValue, 3)
                                lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR * pValue, 3)
                            Next
                        Else
                            For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                If oTable.Contains("MON") And oParameter = "" Then
                                    'When the monthly tables are provided, the value in each month will be updated with the MultiplicationFactor
                                    For Mon = 0 To 11
                                        lOper.Tables(oTable).Parms(Mon).Value = SignificantDigits(lOper.Tables(oTable).Parms(Mon).Value * pValue, 2)
                                        'lOper.Tables(oTable).Parms(Mon).Value = lOper.Tables(oTable).Parms(Mon).Value * pValue
                                    Next
                                Else
                                    lOper.Tables(oTable).ParmValue(oParameter) = SignificantDigits(lOper.Tables(oTable).ParmValue(oParameter) * pValue, 3)
                                End If
                            Next
                        End If

                Next
                    lUci.Save()
                    pOper = ""
                    oTable = ""
                    oParameter = ""
                    pValue = Nothing
                    ModelRunandReportAnswers(SimID, lUci, uciName)
                    'System.IO.File.Copy(uciName, SimID & "-" & uciName, True)
                    System.IO.File.Copy(pBaseName & ".uci", uciName, True)
                    lUci = Nothing
                    lWdmDataSource = Nothing
                    lBinaryDataSource = Nothing
                Next
        End Select
    End Sub

    Sub ModelRunandReportAnswers(ByVal SimID As Integer, ByVal lUci As atcUCI.HspfUci, ByVal uciName As String)

        Dim lExitCode As Integer
        lExitCode = LaunchProgram(pHSPFExe, pTestpath, "-1 -1 " & uciName) 'Run HSPF program
        If lExitCode = -1 Then
            Throw New ApplicationException("winHSPFLt could not run, Analysis cannot continue")

        End If

        lWdmDataSource = New atcWDM.atcDataSourceWDM()
        lWdmFileName = pBaseName & ".wdm"
        lWdmDataSource.Open(lWdmFileName)      'Open the wdm file

        lBinaryDataSource = New atcTimeseriesFileHspfBinOut()
        lHSPFBinaryFile = pBaseName & ".hbn"
        lBinaryDataSource.Open(lHSPFBinaryFile)
        Dim lEvapT As New atcTimeseriesGroup 'Evapotranspiration Data
        Dim lPERLNDOperations As HspfOperations = lUci.OpnBlks("PERLND").Ids

        Dim lRunMade = lWdmDataSource.BuildDate
        

        If pHydrology Then
            lExpertSystem = New HspfSupport.atcExpertSystem(lUci, lWdmDataSource, "IRW.exs")
            Dim lCons As String = "Flow"
            Dim AverageAnnual, AverageAnnualcfs, TenPercentHigh, TwentyFivePercentHigh, FiftyPercentHigh, FiftyPercentLow, TwentyFivePercentLow As Single
            'Dim AverageStormPeakcfs As Single
            Dim TenPercentLow, FivePercentLow, TwoPercentLow As Single

            Dim ErrorAverageAnnual, ErrorTenPercentHigh, ErrorTwentyFivePercentHigh, ErrorFiftyPercentHigh, _
                ErrorFiftyPercentLow, ErrorFivePercentLow, ErrorTwoPercentLow As Single
            Dim ErrorTwentyFivePercentLow, ErrorTenPercentLow As Single

            For Each lSite As HspfSupport.HexSite In lExpertSystem.Sites 'the code below is copied form HSPFOutputReports
                Dim lSiteName As String = lSite.Name

                Dim lArea As Double = lSite.Area
                If SimID = 0 Then
                    Dim obsTimeSeries As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)), _
                                                                          lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                    obsTimeSeries = Aggregate(obsTimeSeries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                    obsAverageAnnualcfs = obsTimeSeries.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                    obsTimeSeries = CfsToInches(obsTimeSeries, lArea)
                    obsAverageAnnual = obsTimeSeries.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                    obsTenPercentHigh = (obsTimeSeries.Attributes.GetValue("Sum") - obsTimeSeries.Attributes.GetValue("%Sum90")) / _
                                                                                                            YearsofSimulation '10% high
                    obsTwentyFivePercentHigh = (obsTimeSeries.Attributes.GetValue("Sum") - obsTimeSeries.Attributes.GetValue("%Sum75")) / _
                                                                                                            YearsofSimulation '25% high
                    obsFiftyPercentHigh = (obsTimeSeries.Attributes.GetValue("Sum") - obsTimeSeries.Attributes.GetValue("%Sum50")) / YearsofSimulation '50% high
                    obsFiftyPercentLow = (obsTimeSeries.Attributes.GetValue("%Sum50")) / YearsofSimulation '50% low
                    obsTwentyFivePercentLow = (obsTimeSeries.Attributes.GetValue("%Sum25")) / YearsofSimulation '25% low
                    obsTenPercentLow = (obsTimeSeries.Attributes.GetValue("%Sum10")) / YearsofSimulation '10% low
                    obsFivePercentLow = (obsTimeSeries.Attributes.GetValue("%Sum05")) / YearsofSimulation '5% low
                    obsTwoPercentLow = (obsTimeSeries.Attributes.GetValue("%Sum02")) / YearsofSimulation '2% low

                    ExpertStatsOutputLine = "Observed,,,,, " & _
                lSiteName & ", " & FormatNumber(obsAverageAnnualcfs, 1, , , TriState.False) & ", " & FormatNumber(obsAverageAnnual, 2) & ", " & _
                FormatNumber(obsTenPercentHigh, 2) & ", " & FormatNumber(obsTwentyFivePercentHigh, 2) & ", " & _
                FormatNumber(obsFiftyPercentHigh, 2) & ", " & FormatNumber(obsFiftyPercentLow, 2) & ", " & _
                FormatNumber(obsTwentyFivePercentLow, 2) & ", " & FormatNumber(obsTenPercentLow, 3) & ", " & _
                FormatNumber(obsFivePercentLow, 2) & ", " & FormatNumber(obsTwoPercentLow, 2) & vbCrLf
                    Sensitivity(SimID, 0) = ("Baseline") 'SimID,lsiteName, obsAverageAnnualcfs}
                    Sensitivity(SimID, 1) = (0)
                    Sensitivity(SimID, 2) = lSite.Name
                    Sensitivity(SimID, 3) = AverageAnnual
                End If


                Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), _
                                                                   lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                Dim lSimTSercfs As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), _
                                                                lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing) * 0.042 * lArea

                AverageAnnualcfs = lSimTSercfs.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                AverageAnnual = lSimTSerInches.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                TenPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - lSimTSerInches.Attributes.GetValue("%Sum90")) / YearsofSimulation '10% high
                TwentyFivePercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - lSimTSerInches.Attributes.GetValue("%Sum75")) / YearsofSimulation '25% high
                FiftyPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - lSimTSerInches.Attributes.GetValue("%Sum50")) / YearsofSimulation '50% high
                FiftyPercentLow = (lSimTSerInches.Attributes.GetValue("%Sum50")) / YearsofSimulation '50% low
                TwentyFivePercentLow = (lSimTSerInches.Attributes.GetValue("%Sum25")) / YearsofSimulation '25% low
                TenPercentLow = (lSimTSerInches.Attributes.GetValue("%Sum10")) / YearsofSimulation '10% low
                FivePercentLow = (lSimTSerInches.Attributes.GetValue("%Sum05")) / YearsofSimulation '5% low
                TwoPercentLow = (lSimTSerInches.Attributes.GetValue("%Sum02")) / YearsofSimulation '2% low
                'calculating error terms value
                ErrorAverageAnnual = (AverageAnnual - obsAverageAnnual) * 100 / obsAverageAnnual
                ErrorTenPercentHigh = (TenPercentHigh - obsTenPercentHigh) * 100 / obsTenPercentHigh
                ErrorTwentyFivePercentHigh = (TwentyFivePercentHigh - obsTwentyFivePercentHigh) * 100 / obsTwentyFivePercentHigh
                ErrorFiftyPercentHigh = (FiftyPercentHigh - obsFiftyPercentHigh) * 100 / obsFiftyPercentHigh
                ErrorFiftyPercentLow = (FiftyPercentLow - obsFiftyPercentLow) * 100 / obsFiftyPercentLow
                ErrorTwentyFivePercentLow = (TwentyFivePercentLow - obsTwentyFivePercentLow) * 100 / obsTwentyFivePercentLow
                ErrorTenPercentLow = (TenPercentLow - obsTenPercentLow) * 100 / obsTenPercentLow
                ErrorFivePercentLow = (FivePercentLow - obsFivePercentLow) * 100 / obsFivePercentLow
                ErrorTwoPercentLow = (TwoPercentLow - obsTwoPercentLow) * 100 / obsTwoPercentLow
                'AverageStormPeakcfs = Need to figure out how to calculate it.
                ExpertStatsOutputLine = ExpertStatsOutputLine & SimID & ", " & pOper & "," & oTable & ", " & oParameter & ", " & Value & ", " & _
                lSiteName & ", " & FormatNumber(AverageAnnualcfs, 1, , , TriState.False) & ", " & FormatNumber(AverageAnnual, 2) & ", " & _
                FormatNumber(TenPercentHigh, 2) & ", " & FormatNumber(TwentyFivePercentHigh, 2) & ", " & _
                FormatNumber(FiftyPercentHigh, 2) & ", " & FormatNumber(FiftyPercentLow, 2) & ", " & _
                FormatNumber(TwentyFivePercentLow, 2) & ", " & FormatNumber(TenPercentLow, 3) & ", " & _
                FormatNumber(FivePercentLow, 3) & ", " & FormatNumber(TwoPercentLow, 3) & ", " & _
                FormatNumber(ErrorAverageAnnual, 1) & ", " & FormatNumber(ErrorTenPercentHigh, 1) & ", " & _
                FormatNumber(ErrorTwentyFivePercentHigh, 1) & ", " & FormatNumber(ErrorFiftyPercentHigh, 1) & ", " & _
                FormatNumber(ErrorFiftyPercentLow, 1) & ", " & FormatNumber(ErrorTwentyFivePercentLow, 1) & ", " & _
                FormatNumber(ErrorTenPercentLow, 1) & ", " & FormatNumber(ErrorFivePercentLow, 1) & ", " & _
                FormatNumber(ErrorTwoPercentLow, 1) & vbCrLf
                
                'Saving the relevant output in a text string to add it to the text file
                IO.File.AppendAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)
                If SimID <> 0 Then
                    Sensitivity(SimID, 0) = (oParameter) 'SimID,lsiteName, obsAverageAnnualcfs}
                    Sensitivity(SimID, 1) = ((Value - 1) * 100)
                    Sensitivity(SimID, 2) = lSite.Name
                    Sensitivity(SimID, 3) = AverageAnnual
                    'Sensitivity(SimID, 4) = Math.Abs((AverageAnnual - Sensitivity(0, 3)) * 100 / Sensitivity("Observed", 3) / (Value - 1))
                    'Sensitivity(SimID, 5) = (AverageAnnual - Sensitivity("Observed", 3)) * 100 / Sensitivity("Observed", 3)

                End If
                ExpertStatsOutputLine = ""
            Next lSite

        End If
        If pWaterQuality Then
            Dim AverageSedimentLoad, AverageWaterTemperature, AverageSummerWaterTemperature, AverageTPLoad, AverageTNLoad As Single
            Dim lTser, lSummerTS As atcTimeseries
            Dim WQIndex, Site As Integer
            For Site = 0 To WQSites.GetUpperBound(0)
                ExpertStatsOutputLine = SimID & ", " & pOper & "," & oTable & ", " & oParameter & ", " & Value & ", " & WQSites(Site) & ", "
                For WQIndex = 0 To WQConstituents.GetUpperBound(0)

                    Select Case WQConstituents(WQIndex)
                        Case "TSS"
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "ROSED4"). _
                            FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            AverageSedimentLoad = lTser.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                            'Assuming sediment load per day per reach in tons in output from all the reaches

                        Case "TW"
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TW"). _
                            FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            AverageWaterTemperature = lTser.Attributes.GetDefinedValue("Mean").Value
                            lSeasons.SeasonSelected(0) = True
                            lSummerTS = lSeasons.SplitBySelected(lTser, Nothing).ItemByIndex(1)
                            AverageSummerWaterTemperature = lSummerTS.Attributes.GetDefinedValue("Mean").Value
                        Case "TP"
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TPLD"). _
                            FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            AverageTPLoad = lTser.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                        Case "TN"
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TNLD"). _
                            FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            AverageTNLoad = lTser.Attributes.GetDefinedValue("Mean").Value / YearsofSimulation
                    End Select
                    
                Next
                ExpertStatsOutputLine = ExpertStatsOutputLine & AverageSedimentLoad & "," & _
                        AverageWaterTemperature & "," & AverageSummerWaterTemperature & "," & AverageTNLoad & "," & AverageTPLoad & vbCrLf
                IO.File.AppendAllText(pBaseName & "_WaterQualityOutput.csv", ExpertStatsOutputLine)
                ExpertStatsOutputLine = ""
            Next
        End If
        lWdmDataSource = Nothing

        If pOutputFromBinary Then
            Dim lLuName As String
            Dim lPERLNDNumber As Integer
            Dim lTempDataset As atcTimeseries
            Dim SimulatedET As Single
            BinaryOutputLine = ""
            For Each lID As HspfOperation In lPERLNDOperations
                lLuName = lID.Description
                lPERLNDNumber = lID.Id
                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "PET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimID & ",P:" & lID.Id & "," & lLuName & "," & SimulatedET & "," 'PET is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "CEPE").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'CEPE is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "UZET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'UZET is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "LZET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'LZET is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "LZET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'AGWET is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "BASET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'BASET is added to the list

                lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "TAET").FindData("Location", "P:" & lID.Id)
                lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
                lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                SimulatedET = SignificantDigits(SimulatedET, 2)
                BinaryOutputLine = BinaryOutputLine & SimulatedET & vbCrLf 'TAET is added to the list

            Next
            IO.File.AppendAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
        End If



    End Sub
    Sub GraphForSensitivty(ByVal Sensivity)
        Dim i As Integer
        For i = 1 To 100
            If Sensivity(i, 0) <> "Baseline" Then

                'Dim lGraphScatter As clsGraphScatter = New clsGraphScatter(lDataGroupError, aZgc)



            End If
        Next
    End Sub
End Module

