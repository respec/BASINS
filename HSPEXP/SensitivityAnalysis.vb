Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData

Public Class SensitivityStats
    Public SiteName As String
    Public Scenario As String
    Public SimID As String
    Public AverageAnnualcfs As Double
    Public AnnualPeakFlow As Double
    Public AverageAnnual As Double
    Public TenPercentHigh As Double
    Public TwentyFivePercentHigh As Double
    Public FiftyPercentHigh As Double
    Public FiftyPercentLow As Double
    Public TwentyFivePercentLow As Double
    Public TenPercentLow As Double
    Public FivePercentLow As Double
    Public TwoPercentLow As Double
    Public ErrorAverageAnnualcfs As Double
    Public ErrorAnnualPeakFlow As Double
    Public ErrorAverageAnnual As Double
    Public ErrorTenPercentHigh As Double
    Public ErrorTwentyFivePercentHigh As Double
    Public ErrorFiftyPercentHigh As Double
    Public ErrorFiftyPercentLow As Double
    Public ErrorTwentyFivePercentLow As Double
    Public ErrorTenPercentLow As Double
    Public ErrorFivePercentLow As Double
    Public ErrorTwoPercentLow As Double

End Class


Public Module ModSensitivityAnalysis
    Sub SensitivityAnalysis(ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As String)
        Dim YearsofSimulation As Single
        Dim Sensitivity(100, 4) As Object
        Dim ExpertStatsOutputLine As String
        Dim pOper, oTable, oParameter, NewuciName, oLandUse As String
        Dim LowerLimit, UpperLimit, Value As Single
        Dim dateTimeOfUciFile As String = System.IO.File.GetLastWriteTime(pBaseName & ".uci")
        dateTimeOfUciFile = FormatDateTime(dateTimeOfUciFile, DateFormat.LongDate)
        System.IO.File.Copy(pBaseName & ".uci", pBaseName & dateTimeOfUciFile & ".uci", True)
        'Save the original copy of the uci file before altering
        ExpertStatsOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name, Average Annual (cfs), Average Annual (in)," &
            "10% High (in), 25% High (in), 50% High (in), 50% Low (in0.), 25% Low (in), 10% Low (in), 5% Low (in), 2% Low (in)," &
            "Annual Peak Flow(cfs), Error(%) Annual Average, Error(%) 10% High, Error(%) 25% High, Error(%) 50% High, " &
            "Error(%) 50% Low, Error(%) 25% Low, Error(%) 10% Low, Error(%) 5% Low, Error(%) 2% Low, Error (%) Annual Peak Flow" & vbCrLf
        IO.File.WriteAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lExitCode As Integer
        Dim SimID As Integer
        Dim lUci As New atcUCI.HspfUci
        Dim uciName As String
        Dim pParameterFile As String = pBaseName & "ParameterValue_Sensitivity.dbf"
        Dim lDBF As New atcUtility.atcTableDBF
        Dim lStats As New Generic.List(Of SensitivityStats)
        uciName = pBaseName & "temp.uci"
        System.IO.File.Copy(pBaseName & ".uci", uciName, True) 'Saving original uci file as temp uci file
        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file
        'lUci.SaveAs(pBaseName, pBaseName & "temp", 1, 1)

        YearsofSimulation = (lUci.GlobalBlock.EdateJ - lUci.GlobalBlock.SDateJ) / JulianYear

        pOper = "Baseline"
        oTable = "Baseline"
        oParameter = "Baseline"
        Dim ParameterDetails As String = "SIMID, OPERATION, TABLE, OPERATIONNUMBER," &
                                                "PARMATERNAME, MONTH/TYPE, PARAMETERVALUE" & vbCrLf

        If lExitCode = -1 Then
            MsgBox("The original uci file could not run. Program will exit")
        End If
        ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestPath,
                                 YearsofSimulation, aSDateJ, aEDateJ, lStats) 'Baseline simulation and recording the values

        Dim DBFRecords As Integer
        lUci.Save()
        lUci = Nothing
        lDBF.OpenFile(pParameterFile)
        'Opening the dbf file that has the parameter values
        'The parameter table will be read from the dbf file
        Dim pValue() As Single = {0, 0} 'Multipliers to calculate uncertainty


        For DBFRecords = 1 To lDBF.NumRecords 'Going through the record in the dbf file
            lDBF.CurrentRecord = DBFRecords
            pOper = lDBF.Value(lDBF.FieldNumber("OPERATION"))
            oTable = lDBF.Value(lDBF.FieldNumber("TABLE"))
            oParameter = lDBF.Value(lDBF.FieldNumber("PARAMETER"))
            LowerLimit = lDBF.Value(lDBF.FieldNumber("LOWERLIMIT"))
            UpperLimit = lDBF.Value(lDBF.FieldNumber("UPPERLIMIT"))
            pValue(0) = lDBF.Value(lDBF.FieldNumber("FACTOR1"))
            pValue(1) = lDBF.Value(lDBF.FieldNumber("FACTOR2"))
            oLandUse = lDBF.Value(lDBF.FieldNumber("DESCRIPTION"))
            For j = 0 To pValue.GetUpperBound(0) 'This loop goes through all the multipliers defined in the pValue object
                lUci = New atcUCI.HspfUci
                SimID += 1
                NewuciName = SimID & "-" & uciName
                System.IO.File.Copy(uciName, NewuciName, True) 'Saving original uci file as temp uci file
                lUci.ReadUci(lMsg, NewuciName, -1, False, pBaseName & ".ech") ' Reading the uci file
                Value = pValue(j)
                Select Case True
                    Case oTable.Contains("EXTNL")
                        Dim MetSegRec As Integer
                        For Each lMetSeg As HspfMetSeg In lUci.MetSegs
                            Select Case oParameter
                                Case "PREC"
                                    MetSegRec = 0
                                Case "ATEM"
                                    MetSegRec = 2
                                Case "SOLR"
                                    MetSegRec = 5
                            End Select
                            lMetSeg.MetSegRecs(MetSegRec).MFactP = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactP _
                                                                                     * pValue(j), 3)
                            'lMetSeg.MetSegRecs(MetSegRec).MFactP = lMetSeg.MetSegRecs(MetSegRec).MFactP * pValue
                            lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR _
                                                                                     * pValue(j), 3)

                        Next

                    Case oTable.Contains("MON-")
                        For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                            If oLandUse = "" Or lOper.Description = oLandUse Then
                                For Mon = 0 To 11
                                    lOper.Tables(oTable).Parms(Mon).Value = SignificantDigits(lOper.Tables(oTable).Parms(Mon).Value _
                                                                                              * pValue(j), 2)
                                    If (lOper.Tables(oTable).Parms(Mon).Value < LowerLimit) Then
                                        lOper.Tables(oTable).Parms(Mon).Value = LowerLimit
                                    ElseIf (lOper.Tables(oTable).Parms(Mon).Value > UpperLimit) Then
                                        lOper.Tables(oTable).Parms(Mon).Value = UpperLimit
                                    End If
                                    ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," &
                                    oParameter & "," & Mon & "," & lOper.Tables(oTable).Parms(Mon).Value & vbCrLf

                                Next

                            End If
                        Next
                    Case oTable.Contains("SILT-CLAY-PM")
                        For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                            For SedimentType As Integer = 15 To 16

                                lOper.Tables(SedimentType).ParmValue(oParameter) =
                                SignificantDigits(lOper.Tables(SedimentType).ParmValue(oParameter) * pValue(j), 3)
                                If oParameter = "TAUCD" Then
                                    lOper.Tables(SedimentType).ParmValue("TAUCS") =
                                SignificantDigits(lOper.Tables(SedimentType).ParmValue("TAUCS") * pValue(j), 3)
                                End If

                            Next

                        Next
                    Case oTable.Contains("MASS-LINK")
                        Dim lMassLinkID As Integer
                        lMassLinkID = Mid(oTable, 10)

                        For Each lMasslink As HspfMassLink In lUci.MassLinks
                            If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains(oParameter) Then
                                lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue(j), 3)
                            End If
                            If lMassLinkID = 21 Then

                                If lMasslink.MassLinkId = 1 AndAlso lMasslink.Source.Group.Contains("IQUAL") Then
                                    lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue(j), 3)
                                End If
                            End If
                            If oParameter = "NITR" Then
                                If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains("PHOS") Then
                                    lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue(j), 3)
                                End If
                            End If
                        Next

                    Case Else
                        For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                            lOper.Tables(oTable).ParmValue(oParameter) =
                                    SignificantDigits(lOper.Tables(oTable).ParmValue(oParameter) * pValue(j), 3)
                            If (lOper.Tables(oTable).ParmValue(oParameter) < LowerLimit) Then
                                lOper.Tables(oTable).ParmValue(oParameter) = LowerLimit
                            ElseIf (lOper.Tables(oTable).ParmValue(oParameter) > UpperLimit) Then
                                lOper.Tables(oTable).ParmValue(oParameter) = UpperLimit
                            End If
                            ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," &
                                                                            oParameter & ",," &
                                                                            lOper.Tables(oTable).ParmValue(oParameter) & vbCrLf
                        Next
                End Select
                lUci.Save()
                ModelRunandReportAnswers(SimID, lUci, NewuciName, lExitCode, pBaseName, pTestPath,
                                         aSDateJ, aEDateJ, YearsofSimulation, lStats)

                System.IO.File.Copy(pBaseName & ".wdm", SimID & "-" & pBaseName & ".wdm", True)
                lUci = Nothing
            Next
        Next
        'GraphForSensitivty(Sensitivity)
        lUci = Nothing
        IO.File.WriteAllText(pBaseName & "_ParameterDetails.txt", ParameterDetails)


    End Sub

    Private Function ModelRunandReportAnswers(ByVal SimID As Integer,
                                 ByVal lUci As atcUCI.HspfUci,
                                 ByVal uciName As String,
                                 ByVal lExitCode As Integer,
                                 ByVal pBaseName As String,
                                 ByVal pTestPath As String,
                                 ByVal aSDateJ As Double,
                                 ByVal aEDateJ As Double,
                                 ByVal YearsofSimulation As Single,
                                 ByRef lStats As List(Of SensitivityStats))

        'lExitCode = LaunchProgram(pHSPFExe, pTestpath, "-1 -1 " & uciName) 'Run HSPF program
        'If lExitCode = -1 Then
        '    Throw New ApplicationException("winHSPFLt could not run, Analysis cannot continue")
        '    Exit Function
        'End If

        Dim lWdmFileName As String = pBaseName & ".wdm"
        Dim lWdmDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(pTestPath & lWdmFileName)

        If lWdmDataSource Is Nothing Then
            If atcDataManager.OpenDataSource(pTestPath & lWdmFileName) Then
                lWdmDataSource = atcDataManager.DataSourceBySpecification(pTestPath & lWdmFileName)
            End If
        End If
        Dim ExpertStatsOutputLine As String = ""
        Dim lExpertSystem As HspfSupport.atcExpertSystem

        lExpertSystem = New HspfSupport.atcExpertSystem(lUci, pBaseName & ".exs", aSDateJ, aEDateJ)
        Dim lCons As String = "Flow"

        Dim lYearlyAttributes As New atcDataAttributes

        For Each lSite As HspfSupport.HexSite In lExpertSystem.Sites
            Dim lSiteName As String = lSite.Name
            Dim lArea As Double = lSite.Area
            If SimID = 0 Then
                Dim lNewStatObs As New SensitivityStats
                With lNewStatObs
                    .SimID = SimID
                    .Scenario = "Observed"
                    Dim obsTimeSeriescfs As atcData.atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)),
                                                                          lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                    obsTimeSeriescfs = Aggregate(obsTimeSeriescfs, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                    'obsTimeSeriescfs.Attributes.DiscardCalculated()
                    Dim obsTimeSeriesInches As atcTimeseries = HspfSupport.CfsToInches(obsTimeSeriescfs, lArea)

                    .SiteName = lSiteName
                    .AverageAnnualcfs = obsTimeSeriescfs.Attributes.GetValue("SumAnnual")
                    obsTimeSeriescfs = Aggregate(obsTimeSeriescfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                    .AnnualPeakFlow = obsTimeSeriescfs.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                    .AverageAnnual = obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                    .TenPercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                        - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum90").Value) / YearsofSimulation
                    .TwentyFivePercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                        - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum75").Value) / YearsofSimulation
                    .FiftyPercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                           - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% high
                    .FiftyPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% low
                    .TwentyFivePercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum25").Value) / YearsofSimulation '25% low
                    .TenPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum10").Value) / YearsofSimulation '10% low
                    .FivePercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum05").Value) / YearsofSimulation '5% low
                    .TwoPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum02").Value) / YearsofSimulation '2% low

                    ExpertStatsOutputLine = "Observed,Obs,Obs,Obs,Obs, " &
                            lSiteName & ", " & FormatNumber(.AverageAnnualcfs, 3, , , TriState.False) &
                            ", " & FormatNumber(.AverageAnnual, 3) & ", " &
                            FormatNumber(.TenPercentHigh, 3) & ", " & FormatNumber(.TwentyFivePercentHigh, 3) & ", " &
                            FormatNumber(.FiftyPercentHigh, 3) & ", " & FormatNumber(.FiftyPercentLow, 3) & ", " &
                            FormatNumber(.TwentyFivePercentLow, 3) & ", " & FormatNumber(.TenPercentLow, 3) & ", " &
                            FormatNumber(.FivePercentLow, 3) & ", " & FormatNumber(.TwoPercentLow, 3) & ", " &
                            FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & vbCrLf
                End With
                lStats.Add(lNewStatObs)
            End If
            Dim lNewStat As New SensitivityStats
            With lNewStat
                If SimID = 0 Then
                    .Scenario = "Baseline"
                Else
                    .Scenario = "Sensitivity"
                End If
                Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)),
                                                                   lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                Dim lSimTSercfs As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)),
                                                                lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing) * 0.042 * lArea
                Dim MaxSimTSercfs = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)

                'lSeasons.SetSeasonalAttributes(lSimTSercfs, lSeasonalAttributes, lYearlyAttributes)
                .SimID = SimID
                .SiteName = lSiteName
                .AverageAnnualcfs = lSimTSercfs.Attributes.GetValue("SumAnnual")
                lSimTSercfs = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                .AnnualPeakFlow = lSimTSercfs.Attributes.GetValue("SumAnnual")
                .AverageAnnual = lSimTSerInches.Attributes.GetValue("SumAnnual")
                .TenPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
                                  lSimTSerInches.Attributes.GetValue("%Sum90")) _
                                / YearsofSimulation '10% high
                .TwentyFivePercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
                                         lSimTSerInches.Attributes.GetValue("%Sum75")) _
                                         / YearsofSimulation '25% high
                .FiftyPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
                                    lSimTSerInches.Attributes.GetValue("%Sum50")) _
                                / YearsofSimulation '50% high
                .FiftyPercentLow = lSimTSerInches.Attributes.GetValue("%Sum50") / YearsofSimulation '50% low
                .TwentyFivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum25") / YearsofSimulation '25% low
                .TenPercentLow = lSimTSerInches.Attributes.GetValue("%Sum10") / YearsofSimulation '10% low
                .FivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum05") / YearsofSimulation '5% low
                .TwoPercentLow = lSimTSerInches.Attributes.GetValue("%Sum02") / YearsofSimulation '2% low
                'calculating error terms value
                Dim ObsValuesList As List(Of SensitivityStats) = lStats.FindAll(Function(x) (x.Scenario = "Observed" And x.SiteName = lSiteName.ToString))
                If ObsValuesList.Count > 0 Then
                    Dim lObsValues As SensitivityStats = ObsValuesList(0)
                    .ErrorAverageAnnual = (.AverageAnnual - lObsValues.AverageAnnual) * 100 / lObsValues.AverageAnnual
                End If

                '.ErrorTenPercentHigh = (TenPercentHigh - obsTenPercentHigh(lSite.Name)) * 100 / obsTenPercentHigh(lSite.Name)
                '.ErrorTwentyFivePercentHigh = (TwentyFivePercentHigh - obsTwentyFivePercentHigh(lSite.Name)) * 100 / obsTwentyFivePercentHigh(lSite.Name)
                '.ErrorFiftyPercentHigh = (FiftyPercentHigh - obsFiftyPercentHigh(lSite.Name)) * 100 / obsFiftyPercentHigh(lSite.Name)
                '.ErrorFiftyPercentLow = (FiftyPercentLow - obsFiftyPercentLow(lSite.Name)) * 100 / obsFiftyPercentLow(lSite.Name)
                '.ErrorTwentyFivePercentLow = (TwentyFivePercentLow - obsTwentyFivePercentLow(lSite.Name)) * 100 / obsTwentyFivePercentLow(lSite.Name)
                '.ErrorTenPercentLow = (TenPercentLow - obsTenPercentLow(lSite.Name)) * 100 / obsTenPercentLow(lSite.Name)
                '.ErrorFivePercentLow = (FivePercentLow - obsFivePercentLow(lSite.Name)) * 100 / obsFivePercentLow(lSite.Name)
                '.ErrorTwoPercentLow = (TwoPercentLow - obsTwoPercentLow(lSite.Name)) * 100 / obsTwoPercentLow(lSite.Name)
                '.ErrorAnnualPeakFlow = (AnnualPeakFlow - obsAnnualPeakFlow(lSite.Name)) * 100 / obsAnnualPeakFlow(lSite.Name)
                'ExpertStatsOutputLine = ExpertStatsOutputLine & SimID & ", " & pOper & "," & oTable & ", " & _
                '            oParameter & ", " & Value & ", " & lSiteName & ", " & _
                '            FormatNumber(AverageAnnualcfs, 3, , , TriState.False) & ", " & _
                '            FormatNumber(AverageAnnual, 3) & ", " & _
                '            FormatNumber(TenPercentHigh, 3) & ", " & FormatNumber(TwentyFivePercentHigh, 3) & ", " & _
                '            FormatNumber(FiftyPercentHigh, 3) & ", " & FormatNumber(FiftyPercentLow, 3) & ", " & _
                '            FormatNumber(TwentyFivePercentLow, 3) & ", " & FormatNumber(TenPercentLow, 3) & ", " & _
                '            FormatNumber(FivePercentLow, 3) & ", " & FormatNumber(TwoPercentLow, 3) & ", " & _
                '            FormatNumber(AnnualPeakFlow, 3, , , TriState.False) & ", " & _
                '            FormatNumber(ErrorAverageAnnual, 1) & ", " & FormatNumber(ErrorTenPercentHigh, 1) & ", " & _
                '            FormatNumber(ErrorTwentyFivePercentHigh, 1) & ", " & FormatNumber(ErrorFiftyPercentHigh, 1) & ", " & _
                '            FormatNumber(ErrorFiftyPercentLow, 1) & ", " & FormatNumber(ErrorTwentyFivePercentLow, 1) & ", " & _
                '            FormatNumber(ErrorTenPercentLow, 1) & ", " & FormatNumber(ErrorFivePercentLow, 1) & ", " & _
                '            FormatNumber(ErrorTwoPercentLow, 1) & ", " & FormatNumber(ErrorAnnualPeakFlow, 1) & vbCrLf
                'Saving the relevant output in a text string to add it to the text file
                lStats.Add(lNewStat)


                IO.File.AppendAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)

                ExpertStatsOutputLine = ""
            End With
        Next lSite

        Return Nothing
    End Function
    Private Function Observed()
        Return "Observed"
    End Function
End Module
