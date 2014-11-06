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
Module SensitivityAndUncertaintyAnalysis
    Private pTestpath, pBaseName, pParameterFile, lstr As String
    'Private lExpertSystem As HspfSupport.atcExpertSystem
    'Private lHSPFBinaryFile As String = pTestpath & "\" & pBaseName & ".hbn"
    'Private lWdmDataSource As New atcWDM.atcDataSourceWDM()
    'Private lBinaryDataSource As New atcTimeseriesFileHspfBinOut()
    Public uciName, ExpertStatsOutputLine, WQOutputLine, BinaryOutputLine As String
    Public pOper, oTable, oParameter, oLandUse As String
    'pOper for Operation, oTable is for operation Table, oParameter is the name of the parameter
    Public pOperNumber As Integer
    Public pValue As Single 'pValue is the parameter value
    Public YearsofSimulation As Integer
    Public pWaterQuality As Boolean = False
    Public pHydrology As Boolean = False
    Public pOutputFromBinary As Boolean = False
    'Public Sensitivity(100, 4) As Object
    Private WQConstituents() As Object = {"TSS", "TW", "TP", "TN"}
    Private WQSites() As Object = {"RCH630", "RCH870"}
    ' "RCH916", "RCH922", "RCH936", "RCH938", "RCH890", "RCH752", "RCH912"}
    'Private TypeOfAnalysis As String = "Sensitivity"
    Private TypeOfAnalysis As String = "Uncertainty"
    Private Value As Single = 0.0
    Private lSeasons As New atcSeasonsYearSubset(aStartMonth:=6, aStartDay:=1, aEndMonth:=8, aEndDay:=31)
    Private Parameters(30, 2) As Object

    Private Sub Initialize()


        pBaseName = "IRW"
        pHydrology = True
        pWaterQuality = True

        Select Case TypeOfAnalysis
            Case "Sensitivity"
                pParameterFile = "C:\BASINS\modelout\IRW\SA\ParameterValue_Sensitivity.dbf"
                pTestpath = "C:\BASINS\modelout\IRW\SA\"
            Case "Uncertainty"
                pParameterFile = "C:\BASINS\modelout\IRW\UA\MFACT_110614.csv"
                pTestpath = "C:\BASINS\modelout\IRW\UA\"
        End Select

            End Sub
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestpath)
        Dim lDBF As New atcTableDBF
        Dim lcsv As New atcTableDelimited

        Dim lUci As New atcUCI.HspfUci
        Dim dateTimeOfUciFile As String = System.IO.File.GetLastWriteTime(pBaseName & ".uci")
        dateTimeOfUciFile = Format(Year(dateTimeOfUciFile), "00") & "_" & Format(Month(dateTimeOfUciFile), "00") & _
                "_" & Format(Day(dateTimeOfUciFile), "00") & "_" & Format(Hour(dateTimeOfUciFile), "00") & _
                "_" & Format(Minute(dateTimeOfUciFile), "00")
        System.IO.File.Copy(pBaseName & ".uci", pBaseName & dateTimeOfUciFile & ".uci", True)
        'Save the original copy of the uci file before altering

        Dim lMsg As New atcUCI.HspfMsg
        Dim lStats As New Generic.List(Of SensitivityStats)
        lMsg.Open("hspfmsg.mdb")

        
        Dim SimID As Integer = 0
        Dim MetSegRec As Integer
        Dim Mon As Integer
        uciName = pBaseName & "temp.uci"
        System.IO.File.Copy(pBaseName & ".uci", uciName, True) 'Saving original uci file as temp uci file
        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file
        'lUci.SaveAs(pBaseName, pBaseName & "temp", 1, 1)
        Dim NewuciName As String

        YearsofSimulation = (lUci.GlobalBlock.EdateJ - lUci.GlobalBlock.SDateJ) / JulianYear
        pValue = 1
        pOper = "Baseline"
        oTable = "Baseline"
        oParameter = "Baseline"
        If pWaterQuality Then
            Dim i As Integer
            IO.File.WriteAllText(pBaseName & "_WaterQualityOutput.csv", WQOutputLine)
            WQOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name "
            For i = 0 To WQConstituents.GetUpperBound(0)
                Select Case WQConstituents(i)
                    Case "TSS"
                        WQOutputLine = WQOutputLine & _
                        ",Mean Ann. TSS Load (tons/year), Mean Daily TSS Load (tons/day), Mean TSS Conc.(mg/l)," & _
                        "Geom. Mean TSS Conc. (mg/l), 10% High TSS Conc.(mg/l), 50% Low TSS Conc.(mg/l)"

                    Case "TP"
                        WQOutputLine = WQOutputLine & _
                        ",Mean Ann. TP Load (lbs/yr), Mean Daily TP Load (lbs/day), Mean TP Conc. (mg/l)," & _
                        "Geom. Mean TP Conc. (mg/l), 10% High TP Conc.(mg/l), 50% Low TP Conc. (mg/l)"
                    Case "TN"
                        WQOutputLine = WQOutputLine & _
                        ",Mean Annual TN Load (lbs/yr), Mean Daily TN Load (lbs/day), Mean TN Conc. (mg/l)," & _
                        "Geom. Mean TN Conc. (mg/l), 10% High TN Conc.(mg/l), 50% Low TN Conc.(mg/l)"
                    Case "TW"
                        WQOutputLine = WQOutputLine & _
                        ",Mean Water Temp. (F), " & "Mean Summer Water Temp. (F)"

                End Select
            Next
            WQOutputLine &= vbCrLf
            IO.File.AppendAllText(pBaseName & "_WaterQualityOutput.csv", WQOutputLine)
        End If

        If pOutputFromBinary Then
            BinaryOutputLine = "Binary Output" & vbCrLf
            IO.File.WriteAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
            BinaryOutputLine = "SimID,Output Location, PET, CEPE, UZET, LZET, AGWET, BASET, TAET" & vbCrLf
            IO.File.AppendAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
        End If
        
        Dim j As Integer
        Dim lExitCode As Integer = 0
        Dim ParameterDetails As String = "SIMID, OPERATION, TABLE, OPERATIONNUMBER," & _
                                                "PARMATERNAME, MONTH/TYPE, PARAMETERVALUE" & vbCrLf
        Select Case TypeOfAnalysis
            Case "Sensitivity"
                ExpertStatsOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name, Mean Annual (cfs), Annual Peak Flow (cfs), Mean Annual runoff(in),10% High (in), 25% High (in), 50% High (in), 50% Low (in), 25% Low (in), 10% Low (in), 5% Low (in), 2% Low (in), Annual Peak Flow(cfs), Error(%) Annual Average, Error(%) 10% High, Error(%) 25% High, Error(%) 50% High, Error(%) 50% Low, Error(%) 25% Low, Error(%) 10% Low, Error(%) 5% Low, Error(%) 2% Low, Error (%) Annual Peak Flow" & vbCrLf
                IO.File.WriteAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)

                'Sensitivity(0, 0) = "Baseline"
                'Sensitivity(0, 1) = (0)
                'Sensitivity(0, 2) = ""
                'Sensitivity(0, 3) = (0)
                'If lExitCode = -1 Then
                '    MsgBox("The original uci file could not run. Program will exit")
                'End If
                ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestpath, YearsofSimulation, lStats)
                'Baseline simulation and recording the values

                Dim DBFRecords As Integer
                lUci.Save()
                lUci = Nothing
                lDBF.OpenFile(pParameterFile)
                'Opening the dbf file that has the parameter values
                'The parameter table will be read from the dbf file
                Dim pValue() As Single = {0, 0} 'Multipliers to calculate uncertainty
                Dim LowerLimit, UpperLimit As Single
                Logger.Dbg("Reading the DBF File")
                For DBFRecords = 1 To lDBF.NumRecords 'Going through the record in the dbf file
                    lDBF.CurrentRecord = DBFRecords
                    pOper = lDBF.Value(lDBF.FieldNumber("OPERATION"))
                    oTable = lDBF.Value(lDBF.FieldNumber("TABLE"))
                    oParameter = lDBF.Value(lDBF.FieldNumber("PARAMETER"))
                    LowerLimit = lDBF.Value(lDBF.FieldNumber("LOWERLIMIT"))
                    UpperLimit = lDBF.Value(lDBF.FieldNumber("UPPERLIMIT"))
                    pValue(0) = lDBF.Value(lDBF.FieldNumber("FACTOR1"))
                    pValue(1) = lDBF.Value(lDBF.FieldNumber("FACTOR2"))
                    For j = 0 To pValue.GetUpperBound(0) 'This loop goes through all the multipliers defined in the pValue object
                        lUci = New atcUCI.HspfUci
                        SimID += 1
                        NewuciName = SimID & "-" & uciName
                        System.IO.File.Copy(uciName, NewuciName, True) 'Saving original uci file as temp uci file
                        lUci.ReadUci(lMsg, NewuciName, -1, False, pBaseName & ".ech") ' Reading the uci file
                        Value = pValue(j)
                        Logger.Dbg("Changing the parameters in the UCI file!")
                        Select Case True
                            Case oTable.Contains("EXTNL")
                                If oParameter = "POINT SOURCES" Then
                                    Dim lRchresOperations As HspfOperations = lUci.OpnBlks("RCHRES").Ids
                                    For Each lReach As HspfOperation In lRchresOperations
                                        For Each lSource As HspfPointSource In lReach.PointSources
                                            If lSource.Target.Group = "INFLOW" Then
                                                lSource.MFact = SignificantDigits(lSource.MFact * pValue(j), 3)
                                            End If
                                        Next
                                    Next
                                Else
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
                                End If

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
                                            ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," & _
                                            oParameter & "," & Mon & "," & lOper.Tables(oTable).Parms(Mon).Value & vbCrLf
                                            IO.File.AppendAllText(pBaseName & "_ParameterDetails.txt", ParameterDetails)
                                            ParameterDetails = ""
                                        Next

                                    End If
                                Next

                            Case oTable.Contains("NUT-BEDCONC")

                                Dim lParameters() As String = {"BNH4(1)", "BNH4(2)", "BNH4(3)", "BPO4(1)", "BPO4(2)", "BPO4(3)"}
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For Each lParameter As String In lParameters
                                        lOper.Tables("NUT-BEDCONC").ParmValue(lParameter) = _
                                        SignificantDigits(lOper.Tables("NUT-BEDCONC").ParmValue(lParameter) * pValue(j), 3)
                                    Next
                                Next

                            Case oTable.Contains("PLNK-PARM2")
                                'If oTable.Contains("PLNK-PARM2") Then Stop

                                Dim lParameters() As String = {"CMMLT", "CMMN", "CMMNP", "CMMP"}
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For Each lParameter As String In lParameters
                                        lOper.Tables("PLNK-PARM2").ParmValue(lParameter) = _
                                        SignificantDigits(lOper.Tables("PLNK-PARM2").ParmValue(lParameter) * pValue(j), 3)
                                    Next
                                Next

                            Case oTable.Contains("SILT-CLAY-PM")
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For SedimentType As Integer = 15 To 16

                                        lOper.Tables(SedimentType).ParmValue(oParameter) = _
                                        SignificantDigits(lOper.Tables(SedimentType).ParmValue(oParameter) * pValue(j), 3)
                                        If oParameter = "TAUCD" Then
                                            lOper.Tables(SedimentType).ParmValue("TAUCS") = _
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
                                    If lMassLinkID = 11 Then

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
                                    If oParameter.Contains("BRBOD(1)") Then oParameter = "BRBOD1"
                                    If oParameter.Contains("BRBOD(2)") Then oParameter = "BRBOD2"
                                    lOper.Tables(oTable).ParmValue(oParameter) = _
                                            SignificantDigits(lOper.Tables(oTable).ParmValue(oParameter) * pValue(j), 3)
                                    If (lOper.Tables(oTable).ParmValue(oParameter) < LowerLimit) Then
                                        lOper.Tables(oTable).ParmValue(oParameter) = LowerLimit
                                    ElseIf (lOper.Tables(oTable).ParmValue(oParameter) > UpperLimit) Then
                                        lOper.Tables(oTable).ParmValue(oParameter) = UpperLimit
                                    End If
                                    ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," & _
                                                                                    oParameter & ",," & _
                                                                                    lOper.Tables(oTable).ParmValue(oParameter) & vbCrLf
                                    IO.File.AppendAllText(pBaseName & "_ParameterDetails.txt", ParameterDetails)
                                    ParameterDetails = ""
                                Next
                        End Select
                        Logger.Dbg("Saved the changes in the UCI file")
                        lUci.Save()
                        ModelRunandReportAnswers(SimID, lUci, NewuciName, lExitCode, pBaseName, pTestpath, YearsofSimulation, lStats)
                        Logger.Dbg("Copied the WDM file to a new file with SimID as " & SimID)
                        System.IO.File.Copy(pBaseName & ".wdm", SimID & "-" & pBaseName & ".wdm", True)
                        lUci = Nothing
                    Next

                Next
                'GraphForSensitivty(Sensitivity)
                lUci = Nothing

                Logger.Dbg("Runs for Sensitivity Analysis finished.")
            Case "Uncertainty"
                ExpertStatsOutputLine = "SimID, OPERATION, PARM-TABLE, PARM, Value, Site Name, Mean Annual (cfs), Annual Peak Flow (cfs), Mean Annual runoff(in),10% High (in), 25% High (in), 50% High (in), 50% Low (in), 25% Low (in), 10% Low (in), 5% Low (in), 2% Low (in), Annual Peak Flow(cfs)" & vbCrLf
                IO.File.WriteAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)
                lcsv.OpenFile(pParameterFile) 'Opening the csv file that has the parameter values
                'lcsv.Header = 3
                Dim TotalNumberOfParameters, ParameterNumber, k As Integer
                TotalNumberOfParameters = lcsv.NumFields - 1
                lcsv.CurrentRecord = 1

                For CurrentRecord As Integer = 1 To 2
                    lcsv.CurrentRecord = CurrentRecord
                    For k = 1 To TotalNumberOfParameters
                        Parameters(k - 1, 0) = lcsv.FieldName(k + 1)
                        Parameters(k - 1, CurrentRecord) = lcsv.Value(k + 1) 'Saving the properties of each parameter in an array
                    Next k
                Next CurrentRecord

                For NumberOfSimulations As Integer = 3 To lcsv.NumRecords
                    'Going through the records in the csv file and changing them in the uci file
                    lcsv.CurrentRecord = NumberOfSimulations
                    SimID = lcsv.Value(1)
                    'MsgBox("HSPF Simulation " & SimID & " of " & lcsv.NumRecords - 3 & " going on!")
                    lUci = New atcUCI.HspfUci
                    lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the uci file
                    'Now the loop starts for changing each parameter at a time
                    For ParameterNumber = 1 To TotalNumberOfParameters
                        pOper = Parameters(ParameterNumber - 1, 0)
                        oTable = Parameters(ParameterNumber - 1, 1)
                        oParameter = Parameters(ParameterNumber - 1, 2)
                        pValue = lcsv.Value(ParameterNumber + 1)

                        Select Case True
                            Case oTable.Contains("EXTNL")
                                If oParameter = "POINT SOURCES" Then
                                    Dim lRchresOperations As HspfOperations = lUci.OpnBlks("RCHRES").Ids
                                    For Each lReach As HspfOperation In lRchresOperations
                                        For Each lSource As HspfPointSource In lReach.PointSources
                                            If lSource.Target.Group = "INFLOW" Then
                                                lSource.MFact = SignificantDigits(lSource.MFact * pValue, 3)
                                            End If
                                        Next
                                    Next
                                Else
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
                                                                                             * pValue, 3)
                                        'lMetSeg.MetSegRecs(MetSegRec).MFactP = lMetSeg.MetSegRecs(MetSegRec).MFactP * pValue
                                        lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR _
                                                                                                 * pValue, 3)
                                    Next
                                End If

                            Case oTable.Contains("MON-")
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    If oLandUse = "" Or lOper.Description = oLandUse Then
                                        For Mon = 0 To 11
                                            lOper.Tables(oTable).Parms(Mon).Value = SignificantDigits(lOper.Tables(oTable).Parms(Mon).Value _
                                                                                                      * pValue, 2)
                                            'If (lOper.Tables(oTable).Parms(Mon).Value < LowerLimit) Then
                                            '    lOper.Tables(oTable).Parms(Mon).Value = LowerLimit
                                            'ElseIf (lOper.Tables(oTable).Parms(Mon).Value > UpperLimit) Then
                                            '    lOper.Tables(oTable).Parms(Mon).Value = UpperLimit
                                            'End If
                                            ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," & _
                                            oParameter & "," & Mon & "," & lOper.Tables(oTable).Parms(Mon).Value & vbCrLf
                                            IO.File.AppendAllText(pBaseName & "_ParameterDetails.txt", ParameterDetails)
                                            ParameterDetails = ""
                                        Next

                                    End If
                                Next

                            Case oTable.Contains("NUT-BEDCONC")

                                Dim lParameters() As String = {"BNH4(1)", "BNH4(2)", "BNH4(3)", "BPO4(1)", "BPO4(2)", "BPO4(3)"}
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For Each lParameter As String In lParameters
                                        lOper.Tables("NUT-BEDCONC").ParmValue(lParameter) = _
                                        SignificantDigits(lOper.Tables("NUT-BEDCONC").ParmValue(lParameter) * pValue, 3)
                                    Next
                                Next

                            Case oTable.Contains("PLNK-PARM2")
                                'If oTable.Contains("PLNK-PARM2") Then Stop

                                Dim lParameters() As String = {"CMMLT", "CMMN", "CMMNP", "CMMP"}
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For Each lParameter As String In lParameters
                                        lOper.Tables("PLNK-PARM2").ParmValue(lParameter) = _
                                        SignificantDigits(lOper.Tables("PLNK-PARM2").ParmValue(lParameter) * pValue, 3)
                                    Next
                                Next

                            Case oTable.Contains("SILT-CLAY-PM")
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    For SedimentType As Integer = 15 To 16

                                        lOper.Tables(SedimentType).ParmValue(oParameter) = _
                                        SignificantDigits(lOper.Tables(SedimentType).ParmValue(oParameter) * pValue, 3)
                                        If oParameter = "TAUCD" Then
                                            lOper.Tables(SedimentType).ParmValue("TAUCS") = _
                                        SignificantDigits(lOper.Tables(SedimentType).ParmValue("TAUCS") * pValue, 3)
                                        End If

                                    Next

                                Next
                            Case oTable.Contains("MASS-LINK")
                                Dim lMassLinkID As Integer
                                lMassLinkID = Mid(oTable, 10)

                                For Each lMasslink As HspfMassLink In lUci.MassLinks
                                    If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains(oParameter) Then
                                        lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue, 3)
                                    End If
                                    If lMassLinkID = 11 Then

                                        If lMasslink.MassLinkId = 1 AndAlso lMasslink.Source.Group.Contains("IQUAL") Then
                                            lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue, 3)
                                        End If
                                    End If
                                    If oParameter = "NITR" Then
                                        If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains("PHOS") Then
                                            lMasslink.MFact = SignificantDigits(lMasslink.MFact * pValue, 3)
                                        End If
                                    End If
                                Next

                            Case Else
                                For Each lOper As HspfOperation In lUci.OpnBlks(pOper).Ids
                                    If oParameter.Contains("BRBOD(1)") Then oParameter = "BRBOD1"
                                    If oParameter.Contains("BRBOD(2)") Then oParameter = "BRBOD2"
                                    lOper.Tables(oTable).ParmValue(oParameter) = _
                                            SignificantDigits(lOper.Tables(oTable).ParmValue(oParameter) * pValue, 3)
                                    'If (lOper.Tables(oTable).ParmValue(oParameter) < LowerLimit) Then
                                    '    lOper.Tables(oTable).ParmValue(oParameter) = LowerLimit
                                    'ElseIf (lOper.Tables(oTable).ParmValue(oParameter) > UpperLimit) Then
                                    '    lOper.Tables(oTable).ParmValue(oParameter) = UpperLimit
                                    'End If
                                    ParameterDetails &= SimID & "," & pOper & "," & oTable & "," & lOper.Id & "," & _
                                                                                    oParameter & ",," & _
                                                                                    lOper.Tables(oTable).ParmValue(oParameter) & vbCrLf
                                    IO.File.AppendAllText(pBaseName & "_ParameterDetails.txt", ParameterDetails)
                                    ParameterDetails = ""
                                Next
                        End Select
                    Next
                    lUci.Save()
                    pOper = ""
                    oTable = ""
                    oParameter = ""
                    pValue = Nothing
                    ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestpath, YearsofSimulation, lStats)
                    'System.IO.File.Copy(uciName, SimID & "-" & uciName, True)

                    System.IO.File.Copy(pBaseName & ".uci", uciName, True)
                    lUci = Nothing
                Next
        End Select
    End Sub

    Sub ModelRunandReportAnswers(ByVal SimID As Integer, _
                                 ByVal lUci As atcUCI.HspfUci, _
                                 ByVal uciName As String, _
                                 ByVal lExitCode As Integer, _
                                 ByVal pBaseName As String, _
                                 ByVal pTestPath As String, _
                                 ByVal YearsofSimulation As Single, _
                                 ByRef lStats As List(Of SensitivityStats))

        Dim pHSPFExe As String = "C:\Basins41\models\HSPF\bin\WinHspfLt.exe"
        Logger.Dbg("Running WinHSPFLt.exe with " & uciName)
        'lExitCode = LaunchProgram(pHSPFExe, pTestPath, "-1 -1 " & uciName) 'Run HSPF program
        'If lExitCode = -1 Then
        '    Throw New ApplicationException("winHSPFLt could not run, Analysis cannot continue")
        '    Exit Sub
        'End If
        Logger.Dbg("Completed WinHSPFLt.exe run with " & uciName)
        Dim lWdmFileName As String
        lWdmFileName = IO.Path.Combine(pTestPath, pBaseName & ".wdm")
        Dim lWdmDataSource As New atcWDM.atcDataSourceWDM
        lWdmDataSource = atcDataManager.DataSourceBySpecification(lWdmFileName)
        If lWdmDataSource IsNot Nothing Then
            atcDataManager.RemoveDataSource(lWdmDataSource)
        End If
        lWdmDataSource = New atcWDM.atcDataSourceWDM()
        atcDataManager.OpenDataSource(lWdmDataSource, lWdmFileName, Nothing)
        Dim lExpertSystem As HspfSupport.atcExpertSystem
        lExpertSystem = New HspfSupport.atcExpertSystem(lUci, "IRW.exs")
        If pHydrology Then
            Logger.Dbg("Calculating Hydrology Statistics for SimID = " & SimID)
            
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
                        .SiteName = lSiteName
                        Dim lobsTSercfs As atcData.atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)), _
                                                                              lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lobsTSercfs = Aggregate(lobsTSercfs, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        Dim lMaxObsTSercfs As atcTimeseries = Aggregate(lobsTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)

                        Dim lobsTimeSeriesInches As atcTimeseries = HspfSupport.CfsToInches(lobsTSercfs, lArea)
                        .AnnualPeakFlow = lMaxObsTSercfs.Attributes.GetValue("Mean")
                        .AverageAnnualcfs = lobsTSercfs.Attributes.GetValue("SumAnnual")

                        lobsTSercfs = Aggregate(lobsTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                        .AnnualPeakFlow = lobsTSercfs.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                        .AverageAnnual = lobsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                        .TenPercentHigh = (lobsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                            - lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum90").Value) / YearsofSimulation
                        .TwentyFivePercentHigh = (lobsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                            - lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum75").Value) / YearsofSimulation
                        .FiftyPercentHigh = (lobsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
                                               - lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% high
                        .FiftyPercentLow = (lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% low
                        .TwentyFivePercentLow = (lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum25").Value) / YearsofSimulation '25% low
                        .TenPercentLow = (lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum10").Value) / YearsofSimulation '10% low
                        .FivePercentLow = (lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum05").Value) / YearsofSimulation '5% low
                        .TwoPercentLow = (lobsTimeSeriesInches.Attributes.GetDefinedValue("%Sum02").Value) / YearsofSimulation '2% low
                        'Annual Peak Flow is being output twice. Fix that. Some numbers have comma when they are output, USe TriState.False in all the output numbers.
                        lobsTSercfs.Clear()
                        lMaxObsTSercfs.Clear()
                        lobsTimeSeriesInches.Clear()

                        ExpertStatsOutputLine = "Observed,Obs,Obs,Obs,Obs, " & _
                                lSiteName & ", " & FormatNumber(.AverageAnnualcfs, 3, , , TriState.False) & _
                                ", " & FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & _
                                ", " & FormatNumber(.AverageAnnual, 3) & ", " & _
                                FormatNumber(.TenPercentHigh, 3) & ", " & FormatNumber(.TwentyFivePercentHigh, 3) & ", " & _
                                FormatNumber(.FiftyPercentHigh, 3) & ", " & FormatNumber(.FiftyPercentLow, 3) & ", " & _
                                FormatNumber(.TwentyFivePercentLow, 3) & ", " & FormatNumber(.TenPercentLow, 3) & ", " & _
                                FormatNumber(.FivePercentLow, 3) & ", " & FormatNumber(.TwoPercentLow, 3) & ", " & _
                                FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & vbCrLf
                    End With
                    lStats.Add(lNewStatObs)
                    'Sensitivity(SimID, 0) = ("Observed") 'SimID,lsiteName, obsAverageAnnualcfs}
                    'Sensitivity(SimID, 1) = (0)
                    'Sensitivity(SimID, 2) = lSite.Name
                    'Sensitivity(SimID, 3) = AverageAnnual
                End If

                Dim lNewStat As New SensitivityStats
                With lNewStat
                    If SimID = 0 Then
                        .Scenario = "Baseline"
                    Else
                        .Scenario = "Sensitivity"
                    End If
                    .SimID = SimID
                    .SiteName = lSiteName
                    Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), _
                                                                       lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                    Dim lSimTSercfs As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)), _
                                                                    lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing) * 0.042 * lArea
                    Dim lMaxSimTSercfs As atcTimeseries = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)

                    .AverageAnnualcfs = lSimTSercfs.Attributes.GetValue("SumAnnual")
                    lSimTSercfs = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                    .AnnualPeakFlow = lMaxSimTSercfs.Attributes.GetValue("Mean")
                    .AverageAnnual = lSimTSerInches.Attributes.GetValue("SumAnnual")
                    .TenPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - _
                                      lSimTSerInches.Attributes.GetValue("%Sum90")) _
                                    / YearsofSimulation '10% high
                    .TwentyFivePercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - _
                                             lSimTSerInches.Attributes.GetValue("%Sum75")) _
                                             / YearsofSimulation '25% high
                    .FiftyPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") - _
                                        lSimTSerInches.Attributes.GetValue("%Sum50")) _
                                    / YearsofSimulation '50% high
                    .FiftyPercentLow = lSimTSerInches.Attributes.GetValue("%Sum50") / YearsofSimulation '50% low
                    .TwentyFivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum25") / YearsofSimulation '25% low
                    .TenPercentLow = lSimTSerInches.Attributes.GetValue("%Sum10") / YearsofSimulation '10% low
                    .FivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum05") / YearsofSimulation '5% low
                    .TwoPercentLow = lSimTSerInches.Attributes.GetValue("%Sum02") / YearsofSimulation '2% low
                    'calculating error terms value
                    lSimTSercfs.Clear()
                    lSimTSerInches.Clear()
                    lMaxSimTSercfs.Clear()
                    ExpertStatsOutputLine = SimID & ", " & pOper & "," & oTable & ", " & _
                                    oParameter & ", " & Value & ", " & lSiteName & ", " & _
                                    FormatNumber(.AverageAnnualcfs, 3, , , TriState.False) & ", " & _
                                    FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & ", " & _
                                    FormatNumber(.AverageAnnual, 3) & ", " & _
                                    FormatNumber(.TenPercentHigh, 3) & ", " & FormatNumber(.TwentyFivePercentHigh, 3) & ", " & _
                                    FormatNumber(.FiftyPercentHigh, 3) & ", " & FormatNumber(.FiftyPercentLow, 3) & ", " & _
                                    FormatNumber(.TwentyFivePercentLow, 3) & ", " & FormatNumber(.TenPercentLow, 3) & ", " & _
                                    FormatNumber(.FivePercentLow, 3) & ", " & FormatNumber(.TwoPercentLow, 3) & ", " & _
                                    FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & ", "
                    Dim ObsValuesList As List(Of SensitivityStats) = lStats.FindAll(Function(x) (x.Scenario = "Observed" _
                                                                                                 And x.SiteName = lSiteName.ToString))
                    If ObsValuesList.Count > 0 Then
                        Dim lObsValues As SensitivityStats = ObsValuesList(0)
                        .ErrorAverageAnnual = (.AverageAnnual - lObsValues.AverageAnnual) * 100 / lObsValues.AverageAnnual
                        'calculating error terms value
                        .ErrorTenPercentHigh = (.TenPercentHigh - lObsValues.TenPercentHigh) * 100 / lObsValues.TenPercentHigh
                        .ErrorTwentyFivePercentHigh = (.TwentyFivePercentHigh - lObsValues.TwentyFivePercentHigh) * 100 / lObsValues.TwentyFivePercentHigh
                        .ErrorFiftyPercentHigh = (.FiftyPercentHigh - lObsValues.FiftyPercentHigh) * 100 / lObsValues.FiftyPercentHigh
                        .ErrorFiftyPercentLow = (.FiftyPercentLow - lObsValues.FiftyPercentLow) * 100 / lObsValues.FiftyPercentLow
                        .ErrorTwentyFivePercentLow = (.TwentyFivePercentLow - lObsValues.TwentyFivePercentLow) * 100 / lObsValues.TwentyFivePercentLow
                        .ErrorTenPercentLow = (.TenPercentLow - lObsValues.TenPercentLow) * 100 / lObsValues.TenPercentLow
                        .ErrorFivePercentLow = (.FivePercentLow - lObsValues.FivePercentLow) * 100 / lObsValues.FivePercentLow
                        .ErrorTwoPercentLow = (.TwoPercentLow - lObsValues.TwoPercentLow) * 100 / lObsValues.TwoPercentLow
                        .ErrorAnnualPeakFlow = (.AnnualPeakFlow - lObsValues.AnnualPeakFlow) * 100 / lObsValues.AnnualPeakFlow
                        ExpertStatsOutputLine &= FormatNumber(.ErrorAverageAnnual, 1) & ", " & FormatNumber(.ErrorTenPercentHigh, 1) & ", " & _
                                    FormatNumber(.ErrorTwentyFivePercentHigh, 1) & ", " & FormatNumber(.ErrorFiftyPercentHigh, 1) & ", " & _
                                    FormatNumber(.ErrorFiftyPercentLow, 1) & ", " & FormatNumber(.ErrorTwentyFivePercentLow, 1) & ", " & _
                                    FormatNumber(.ErrorTenPercentLow, 1) & ", " & FormatNumber(.ErrorFivePercentLow, 1) & ", " & _
                                    FormatNumber(.ErrorTwoPercentLow, 1) & ", " & FormatNumber(.ErrorAnnualPeakFlow, 1) & vbCrLf
                        ''Saving the relevant output in a text string to add it to the text file

                    End If
                    lStats.Add(lNewStat)

                    IO.File.AppendAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)

                    ExpertStatsOutputLine = ""
                End With

                If TypeOfAnalysis = "Sensitivity" AndAlso SimID <> 0 Then
                    'Sensitivity(SimID, 0) = (oParameter) 'SimID,lsiteName, obsAverageAnnualcfs}
                    'Sensitivity(SimID, 1) = ((Value - 1) * 100)
                    'Sensitivity(SimID, 2) = lSite.Name
                    'Sensitivity(SimID, 3) = AverageAnnual
                    'Sensitivity(SimID, 4) = Math.Abs((AverageAnnual - Sensitivity(0, 3)) * 100 / Sensitivity("Observed", 3) / (Value - 1))
                    'Sensitivity(SimID, 5) = (AverageAnnual - Sensitivity("Observed", 3)) * 100 / Sensitivity("Observed", 3)

                End If
                ExpertStatsOutputLine = ""
            Next lSite

        End If
        If pWaterQuality Then
            Logger.Dbg("Calculating Water Quality Statistics for SimID = " & SimID)

            Dim lSeasonsAnnual As New atcSeasonsCalendarYear
            Dim lyearlyTSGroup As New atcTimeseriesGroup
            Dim lTenPercentHigh, lFiftyPercentLow As Double
            Dim WQIndex, Site As Integer
            For Site = 0 To WQSites.GetUpperBound(0)
                ExpertStatsOutputLine = SimID & ", " & pOper & "," & oTable & ", " & _
                oParameter & ", " & Value & ", " & WQSites(Site) & ", "

                For WQIndex = 0 To WQConstituents.GetUpperBound(0)
                    Select Case WQConstituents(WQIndex)
                        Case "TSS"

                            Dim lTser As atcTimeseries = lWdmDataSource.DataSets.FindData("Constituent", "ROSED4"). _
                                        FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                            Dim AverageAnnualSedimentLoad As Single = lTser.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                            Dim AverageDailySedimentLoad As Single = lTser.Attributes.GetValue("Mean")
                            'Assuming sediment load per day per reach in tons in output from all the reaches
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TSS").FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            Dim lMeanConc As Single = lTser.Attributes.GetValue("Mean")
                            Dim lGeoMeanConc As Single = lTser.Attributes.GetValue("Geometric Mean")

                            lyearlyTSGroup = lSeasonsAnnual.Split(lTser, Nothing)
                            lTenPercentHigh = lTser.Attributes.GetValue("%90")
                            lFiftyPercentLow = lTser.Attributes.GetValue("%50")

                            'For Each lyearTS As atcTimeseries In lyearlyTSGroup
                            '    lTenPercentHigh += (lyearTS.Attributes.GetDefinedValue("Sum").Value - lyearTS.Attributes.GetDefinedValue("%Sum90").Value)
                            '    lFiftyPercentLow += lyearTS.Attributes.GetDefinedValue("%Sum50").Value
                            'Next

                            'lTenPercentHigh = lTenPercentHigh / YearsofSimulation
                            'lFiftyPercentLow = lFiftyPercentLow / YearsofSimulation
                            ExpertStatsOutputLine &= AverageAnnualSedimentLoad & "," & AverageDailySedimentLoad & ", " & lMeanConc & _
                                                    "," & lGeoMeanConc & "," & _
                            lTenPercentHigh & "," & lFiftyPercentLow & ","
                            lTser.Clear()
                            lyearlyTSGroup.Clear()
                        Case "TW"
                            Dim lTser As atcTimeseries = lWdmDataSource.DataSets.FindData("Constituent", "TW"). _
                                    FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            Dim AverageWaterTemperature As Single = lTser.Attributes.GetDefinedValue("Mean").Value
                            lSeasons.SeasonSelected(0) = True
                            Dim lSummerTS As atcTimeseries = lSeasons.SplitBySelected(lTser, Nothing).ItemByIndex(1)
                            Dim AverageSummerWaterTemperature As Single = lSummerTS.Attributes.GetDefinedValue("Mean").Value
                            ExpertStatsOutputLine &= AverageWaterTemperature & "," & AverageSummerWaterTemperature & ","
                            lSummerTS.Clear()
                            lTser.Clear()
                        Case "TP"
                            Dim lTser As atcTimeseries = lWdmDataSource.DataSets.FindData("Constituent", "TPLD"). _
                                    FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                            Dim AverageAnnualTPLoad As Single = lTser.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                            Dim AverageDailyTPLoad As Single = lTser.Attributes.GetValue("Mean")
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TP").FindData _
                                    ("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            Dim lMeanConc As Single = lTser.Attributes.GetValue("Mean")
                            Dim lGeoMeanConc As Single = lTser.Attributes.GetValue("Geometric Mean")

                            lyearlyTSGroup = lSeasonsAnnual.Split(lTser, Nothing)
                            lTenPercentHigh = lTser.Attributes.GetValue("%90")
                            lFiftyPercentLow = lTser.Attributes.GetValue("%50")
                            'For Each lyearTS As atcTimeseries In lyearlyTSGroup
                            '    lTenPercentHigh += (lyearTS.Attributes.GetDefinedValue("Sum").Value - _
                            '                        lyearTS.Attributes.GetDefinedValue("%Sum90").Value)
                            '    lFiftyPercentLow += lyearTS.Attributes.GetDefinedValue("%Sum50").Value
                            'Next
                            'lTenPercentHigh = lTenPercentHigh / YearsofSimulation
                            'lFiftyPercentLow = lFiftyPercentLow / YearsofSimulation
                            ExpertStatsOutputLine &= AverageAnnualTPLoad & "," & AverageDailyTPLoad & ", " & lMeanConc & "," & lGeoMeanConc & "," & _
                            lTenPercentHigh & "," & lFiftyPercentLow & ","
                            lTser.Clear()
                            lyearlyTSGroup.Clear()
                        Case "TN"
                            Dim lTser As atcTimeseries = lWdmDataSource.DataSets.FindData("Constituent", "TNLD"). _
                                    FindData("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                            Dim AnnualAverageTNLoad As Single = lTser.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
                            Dim DailyAverageTNLoad As Single = lTser.Attributes.GetValue("Mean")
                            lTser = lWdmDataSource.DataSets.FindData("Constituent", "TN").FindData _
                                    ("Location", WQSites(Site))(0)
                            lTser = SubsetByDate(lTser, lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                            lTser = Aggregate(lTser, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                            Dim lMeanConc As Single = lTser.Attributes.GetValue("Mean")
                            Dim lGeoMeanConc As Single = lTser.Attributes.GetValue("Geometric Mean")

                            lyearlyTSGroup = lSeasonsAnnual.Split(lTser, Nothing)
                            lTenPercentHigh = lTser.Attributes.GetValue("%90")
                            lFiftyPercentLow = lTser.Attributes.GetValue("%50")
                            'For Each lyearTS As atcTimeseries In lyearlyTSGroup
                            '    lTenPercentHigh += (lyearTS.Attributes.GetDefinedValue("Sum").Value - lyearTS.Attributes.GetDefinedValue("%sum90").Value)
                            '    lFiftyPercentLow += lyearTS.Attributes.GetDefinedValue("%Sum50").Value
                            'Next
                            'lTenPercentHigh = lTenPercentHigh / YearsofSimulation
                            'lFiftyPercentLow = lFiftyPercentLow / YearsofSimulation
                            ExpertStatsOutputLine &= AnnualAverageTNLoad & "," & DailyAverageTNLoad & ", " & lMeanConc & "," & lGeoMeanConc & "," & _
                                lTenPercentHigh & "," & lFiftyPercentLow & ","
                            lTser.Clear()
                            lyearlyTSGroup.Clear()
                    End Select

                Next
                ExpertStatsOutputLine &= vbCrLf
                IO.File.AppendAllText(pBaseName & "_WaterQualityOutput.csv", ExpertStatsOutputLine)
                ExpertStatsOutputLine = ""
            Next
        End If
        'lWdmDataSource.Clear()

        'If pOutputFromBinary Then
        '    lBinaryDataSource = New atcTimeseriesFileHspfBinOut()
        '    lHSPFBinaryFile = pBaseName & ".hbn"
        '    lBinaryDataSource.Open(lHSPFBinaryFile)
        '    Dim lEvapT As New atcTimeseriesGroup 'Evapotranspiration Data
        '    Dim lPERLNDOperations As HspfOperations = lUci.OpnBlks("PERLND").Ids

        '    Dim lLuName As String
        '    Dim lPERLNDNumber As Integer
        '    Dim lTempDataset As atcTimeseries
        '    Dim SimulatedET As Single
        '    BinaryOutputLine = ""
        '    For Each lID As HspfOperation In lPERLNDOperations
        '        lLuName = lID.Description
        '        lPERLNDNumber = lID.Id
        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "PET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimID & ",P:" & lID.Id & "," & lLuName & "," & SimulatedET & "," 'PET is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "CEPE").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'CEPE is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "UZET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'UZET is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "LZET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'LZET is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "LZET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'AGWET is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "BASET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & "," 'BASET is added to the list

        '        lEvapT = lBinaryDataSource.DataSets.FindData("Constituent", "TAET").FindData("Location", "P:" & lID.Id)
        '        lTempDataset = SubsetByDate(lEvapT.Item(0), lUci.GlobalBlock.SDateJ, lUci.GlobalBlock.EdateJ, Nothing)
        '        lTempDataset = Aggregate(lTempDataset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '        SimulatedET = lTempDataset.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '        SimulatedET = SignificantDigits(SimulatedET, 2)
        '        BinaryOutputLine = BinaryOutputLine & SimulatedET & vbCrLf 'TAET is added to the list

        '    Next
        '    IO.File.AppendAllText(pBaseName & "_HSPFBinaryOutput.txt", BinaryOutputLine)
        'End If


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

