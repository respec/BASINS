Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports System.IO

Public Class HydrologySensitivityStats
    Public SiteName As String
    Public Scenario As String
    Public SimID As Integer
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

Public Class SensitivityStats
    Public SimID As Integer
    Public DSNID As Integer
    Public OverallSum As Double
    Public AnnualSum As Double
    Public TenPercentHigh As Double
    Public TwentyFivePercentHigh As Double
    Public FiftyPercentHigh As Double
    Public FiftyPercentLow As Double
    Public TwentyFivePercentLow As Double
    Public TenPercentLow As Double
    Public FivePercentLow As Double
    Public TwoPercentLow As Double
End Class

Public Class ModelParameter
    Public ParmID As Integer
    Public ParmOperation As String
    Public ParmOperationNumber As Integer
    Public ParmLandUseName As String = ""
    Public ParmTable As String
    Public ParmName As String
    Public ParmLow As Double = -1.0E-30
    Public ParmHigh As Double = 1.0E+30
End Class


Public Module ModSensitivityAnalysis
    Sub SensitivityAnalysis(ByVal aHSPFEXE As String, ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As Double)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm") 'Need to figure out why we need this.
        Dim lExitCode As Integer
        Dim SimID As Integer = 0
        Dim lUci As New atcUCI.HspfUci
        Dim SaveUCIFiles As Boolean = True
        Dim uciName As String
        uciName = pBaseName & "_UA.uci"
        IO.File.Copy(pBaseName & ".uci", uciName, True) 'Saving the original file with _UA at the end
        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file with _UA

        Dim lStats As New Generic.List(Of SensitivityStats)
        Dim YearsofSimulation As Single
        YearsofSimulation = (aEDateJ - aSDateJ) / JulianYear

        'Generate a default Parameter Sensitivity Table file if it does not exist already
        Dim lSensitiveParameterListFilesIsAvailable As Boolean = False
        Dim lSensitivitySpecificationFile As String = "SensitiveParametersList.csv"
        Dim SensitivityOutputFile As StreamWriter = File.CreateText("SensitivityOutput.csv")
        SensitivityOutputFile.WriteLine("SimID, DSNID, Sum, 10%High, 25%High, 50%High, 50%Low, 25%Low, 10%Low, 5%Low, 2%Low")


        Dim NumberOfOutputDSN As Integer = 0

        If Not IO.File.Exists(lSensitivitySpecificationFile) Then
            Dim SensitivityParameterFile As StreamWriter = IO.File.CreateText("SensitiveParametersList.csv")
            Dim TextToAddForSensitivityFile As String = "***Generic Parameter List For Sensitivity/Uncertainty Analysis And Output DSN from the UCI file"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "***Following Output IDs are the first two ID read from the EXT TARGET block of the UCI file. You can add more if you want to."
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)

            TextToAddForSensitivityFile = "DSN,"
            For Each lEXTTARGET As HspfConnection In lUci.Connections

                If lEXTTARGET.Target.VolName.Contains("WDM") AndAlso NumberOfOutputDSN <= 2 Then
                    NumberOfOutputDSN += 1
                    TextToAddForSensitivityFile &= lEXTTARGET.Target.VolId & ","

                End If
            Next

            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "***The operation number, land use, tied with next, and multiplier can be left blank"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "Parameter,OPERATION, OPERATION NUMBER, LANDUSE, TABLE, PARM, LOWERLIMIT, UPPERLIMIT"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "1,PERLND, , , PWAT-PARM2, LZSN,3,8"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "2,PERLND, , , PWAT-PARM2, INFILT,0.01,0.5"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "***Following lines list the multiplication factor for each parameter for each simulation."
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "Simulation ID,Parameter1,Parameter2,,,,,"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "1,0.9,1,"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "2,1.1,1,"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "3,1,0.9,"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "4,1,1.1,"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)


            MsgBox("A default list of Sensitive parameters was created as SensitiveParameterList.csv. 
                      You can edit this file and add more parameters.", vbOKOnly, "Default List of Sensitive Parameters")
            SensitivityParameterFile.Close()
        End If

        'The file listing the sensitive parameters and the output DSN already exists.
        Dim lSpecificationFileRecordsNew As New ArrayList
        lSpecificationFileRecordsNew = ReadCSVFile(lSensitivitySpecificationFile) 'Reading the UA specification file 

        Dim listOfOutputDSN As New atcCollection
        Dim lcsvRecordIndex As Integer = 0
        Dim lcsvlinerecord() As String = lSpecificationFileRecordsNew(lcsvRecordIndex)
        NumberOfOutputDSN = lcsvlinerecord.Length - 2
        For i = 1 To NumberOfOutputDSN
            If lcsvlinerecord(i) = "" Then
                NumberOfOutputDSN = i - 1
                Exit For
            Else
                listOfOutputDSN.Add(lcsvlinerecord(i))
            End If

        Next




        Dim loutputLine As String = ""
        loutputLine = ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, lStats, listOfOutputDSN)

        SensitivityOutputFile.Write(loutputLine)
        'SensitivityOutputFile.Close()

        Dim ListOfParameters As New List(Of ModelParameter)
        lcsvRecordIndex += 1
        lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
        Do Until lcsvlinerecord(0) = "Simulation ID"
            'Going through the CSV file and getting the list of parameters.
            Dim NewParameter As New ModelParameter
            With NewParameter
                .ParmID = lcsvlinerecord(0)
                .ParmOperation = lcsvlinerecord(1)
                If Not lcsvlinerecord(2) = "" Then
                    .ParmOperationNumber = CInt(lcsvlinerecord(2))
                End If
                If Not lcsvlinerecord(3) = "" Then
                    .ParmLandUseName = lcsvlinerecord(3)
                End If

                .ParmTable = lcsvlinerecord(4)
                .ParmName = lcsvlinerecord(5)
                If lcsvlinerecord.Length > 6 Then
                    .ParmLow = CDbl(lcsvlinerecord(6))
                    .ParmHigh = CDbl(lcsvlinerecord(7))
                End If

            End With

            ListOfParameters.Add(NewParameter)
            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
        Loop

        Dim MultiplicationFactor As Double = 0

        Do
            Dim ParameterSequence As Integer = 1
            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
            SimID = lcsvlinerecord(0)
            For Each Parm As ModelParameter In ListOfParameters
                MultiplicationFactor = lcsvlinerecord(Parm.ParmID)

                With Parm
                    lUci = ChangeUCIParameterAndSave(lUci, .ParmOperation, .ParmOperationNumber, .ParmLandUseName,
                                                     .ParmTable, .ParmName, .ParmLow, .ParmHigh, MultiplicationFactor)

                End With
                ParameterSequence += 1
            Next


            loutputLine = ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, lStats, listOfOutputDSN)
            'lUci = Nothing
            SensitivityOutputFile.Write(loutputLine)

            If SaveUCIFiles Then
                IO.File.Copy(uciName, SimID & uciName, True)
            End If


        Loop While lcsvRecordIndex < lSpecificationFileRecordsNew.Count - 1

        SensitivityOutputFile.Close()

    End Sub
    Private Function ReadCSVFile(aSensitivitySpecificationFile As String)
        Dim lSensitivityRecordsNew As New ArrayList()
        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(aSensitivitySpecificationFile)
            Dim lines() As String = {}
            If System.IO.File.Exists(aSensitivitySpecificationFile) Then
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")
                Dim CurrentRow As String()

                While Not MyReader.EndOfData
                    Try
                        If (MyReader.PeekChars(10000).Contains("***") Or
                                    Trim(MyReader.PeekChars(10000)) = "" Or
                                    Trim(MyReader.PeekChars(10000).ToLower.StartsWith("parameter")) Or
                                    Trim(MyReader.PeekChars(10000).ToLower.StartsWith(","))) Then
                            CurrentRow = MyReader.ReadFields
                        Else
                            CurrentRow = MyReader.ReadFields
                            Dim i As Integer = 0
                            For Each testtring As String In CurrentRow
                                CurrentRow(i) = testtring
                                i += 1
                            Next
                            lSensitivityRecordsNew.Add(CurrentRow)
                        End If

                    Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                        MsgBox("Line " & ex.Message &
                            "is not valid and will be skipped.")
                    End Try
                End While
            End If
        End Using

        Dim lRecordIndex As Integer = 0

        If lSensitivityRecordsNew.Count < 1 Then
            MsgBox("The" & aSensitivitySpecificationFile & " file didn't have any useful data. Reading next CSV file!", vbOKOnly)
            End

        End If


        Return lSensitivityRecordsNew

    End Function

    Private Function ChangeUCIParameterAndSave(ByRef aUCI As atcUCI.HspfUci,
                                               ByVal aParameterOperation As String,
                                               ByVal aOperationNumber As String,
                                               ByVal aLandUsename As String,
                                               ByVal aTableName As String,
                                               ByVal aParameterName As String,
                                               ByVal aLowerLimit As Double,
                                               ByVal aUpperLimit As Double,
                                               ByVal aMultiplicationFactor As Double
                                               )
        Select Case True
            Case aTableName.Contains("EXTNL")
                Dim MetSegRec As Integer
                For Each lMetSeg As HspfMetSeg In aUCI.MetSegs
                    Select Case aParameterName
                        Case "PREC"
                            MetSegRec = 0
                        Case "ATEM"
                            MetSegRec = 2
                        Case "SOLR"
                            MetSegRec = 5
                    End Select
                    lMetSeg.MetSegRecs(MetSegRec).MFactP = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactP _
                                                                                             * aMultiplicationFactor, 3)

                    lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR _
                                                                                             * aMultiplicationFactor, 3)

                Next

            Case aTableName.Contains("MON-")
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    If aLandUsename = "" OrElse lOper.Description = aLandUsename Then
                        For Mon = 0 To 11
                            lOper.Tables(aTableName).Parms(Mon).Value = SignificantDigits(lOper.Tables(aTableName).Parms(Mon).Value _
                                                                                                      * aMultiplicationFactor, 2)
                            If (lOper.Tables(aTableName).Parms(Mon).Value < aLowerLimit) Then
                                lOper.Tables(aTableName).Parms(Mon).Value = aLowerLimit
                            ElseIf (lOper.Tables(aTableName).Parms(Mon).Value > aUpperLimit) Then
                                lOper.Tables(aTableName).Parms(Mon).Value = aUpperLimit
                            End If
                            'ParameterDetailText &= SimID & "," & ParameterOperation & "," & TableName & "," & lOper.Id & "," &
                            '                ParameterName & "," & Mon & "," & lOper.Tables(TableName).Parms(Mon).Value & vbCrLf

                        Next

                    End If
                Next
            Case aTableName.Contains("SILT-CLAY-PM")
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    For SedimentType As Integer = 15 To 16

                        lOper.Tables(SedimentType).ParmValue(aParameterName) =
                                        SignificantDigits(lOper.Tables(SedimentType).ParmValue(aParameterName) * aMultiplicationFactor, 3)
                        If aParameterName = "TAUCD" Then
                            lOper.Tables(SedimentType).ParmValue("TAUCS") =
                                        SignificantDigits(lOper.Tables(SedimentType).ParmValue("TAUCS") * aMultiplicationFactor, 3)
                        End If

                    Next

                Next
            Case aTableName.Contains("MASS-LINK")
                Dim lMassLinkID As Integer
                lMassLinkID = Mid(aTableName, 10)

                For Each lMasslink As HspfMassLink In aUCI.MassLinks
                    If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains(aParameterName) Then
                        lMasslink.MFact = SignificantDigits(lMasslink.MFact * aMultiplicationFactor, 3)
                    End If
                    If lMassLinkID = 21 Then

                        If lMasslink.MassLinkId = 1 AndAlso lMasslink.Source.Group.Contains("IQUAL") Then
                            lMasslink.MFact = SignificantDigits(lMasslink.MFact * aMultiplicationFactor, 3)
                        End If
                    End If
                    If aParameterName = "NITR" Then
                        If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains("PHOS") Then
                            lMasslink.MFact = SignificantDigits(lMasslink.MFact * aMultiplicationFactor, 3)
                        End If
                    End If
                Next

            Case Else
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    lOper.Tables(aTableName).ParmValue(aParameterName) =
                                            SignificantDigits(lOper.Tables(aTableName).ParmValue(aParameterName) * aMultiplicationFactor, 3)
                    If (lOper.Tables(aTableName).ParmValue(aParameterName) < aLowerLimit) Then
                        lOper.Tables(aTableName).ParmValue(aParameterName) = aLowerLimit
                    ElseIf (lOper.Tables(aTableName).ParmValue(aParameterName) > aUpperLimit) Then
                        lOper.Tables(aTableName).ParmValue(aParameterName) = aUpperLimit
                    End If
                    'ParameterDetailText &= SimID & "," & ParameterOperation & "," & TableName & "," & lOper.Id & "," &
                    '                                                                ParameterName & ",," &
                    '                                                                lOper.Tables(TableName).ParmValue(ParameterName) & vbCrLf
                Next
        End Select

        aUCI.Save()

        Return aUCI


    End Function

    Private Function ModelRunandReportAnswers(ByVal SimID As Integer,
                                 ByVal lUci As atcUCI.HspfUci,
                                 ByVal uciName As String,
                                 ByVal lExitCode As Integer,
                                 ByVal pBaseName As String,
                                 ByVal pTestPath As String,
                                 ByVal aSDateJ As Double,
                                 ByVal aEDateJ As Double,
                                 ByVal YearsofSimulation As Single,
                                 ByRef lStats As List(Of SensitivityStats),
                                              ByVal aOutputDSN As atcCollection)

        lExitCode = LaunchProgram(pHSPFExe, pTestPath, "-1 -1 " & uciName) 'Run HSPF program
        If lExitCode = -1 Then
            Throw New ApplicationException("winHSPFLt could not run, Analysis cannot continue")
            Exit Function
        End If

        Dim lWdmFileName As String = pBaseName & ".wdm"
        Dim lWdmDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(pTestPath & lWdmFileName)

        If lWdmDataSource Is Nothing Then

        Else
            atcDataManager.RemoveDataSource(pTestPath & lWdmFileName)
        End If

        If atcDataManager.OpenDataSource(pTestPath & lWdmFileName) Then
            lWdmDataSource = atcDataManager.DataSourceBySpecification(pTestPath & lWdmFileName)
        End If
        'Dim ExpertStatsOutputLine As String = ""
        'Dim lExpertSystem As HspfSupport.atcExpertSystem

        'lExpertSystem = New HspfSupport.atcExpertSystem(lUci, pBaseName & ".exs", aSDateJ, aEDateJ)
        'Dim lCons As String = "Flow"

        Dim lYearlyAttributes As New atcDataAttributes
        Dim lOutputline As String = ""
        For Each WDMDataset As Integer In aOutputDSN
            Dim SimulatedTS As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(WDMDataset), aSDateJ, aEDateJ, Nothing)

            Dim lNewStatDataset As New SensitivityStats
            With lNewStatDataset
                .SimID = SimID
                .DSNID = WDMDataset
                .OverallSum = SimulatedTS.Attributes.GetDefinedValue("Sum").Value
                .TenPercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum90").Value
                .TwentyFivePercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum75").Value
                .FiftyPercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                .FiftyPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                .TwentyFivePercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum25").Value
                .TenPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum10").Value
                .FivePercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum05").Value
                .TwoPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum02").Value
                lOutputline &= .SimID & ", " & .DSNID & ", " & FormatNumber(.OverallSum, 3,, TriState.False, False) & ", " &
                    FormatNumber(.TenPercentHigh, 3,, TriState.False, False) &
                    ", " & FormatNumber(.TwentyFivePercentHigh, 3,, TriState.False, False) &
                    ", " & FormatNumber(.FiftyPercentHigh, 3,, TriState.False, False) &
                    ", " & FormatNumber(.FiftyPercentLow, 3,, TriState.False, False) &
                    ", " & FormatNumber(.TwentyFivePercentLow, 3,, TriState.False, False) &
                    ", " & FormatNumber(.TenPercentLow, 3,, TriState.False, False) & ", " &
                    FormatNumber(.FivePercentLow, 3,, TriState.False, False) & ", " &
                    FormatNumber(.TwoPercentLow, 3,, TriState.False, False) & vbCrLf
            End With
        Next
        lWdmDataSource.Clear()

        'For Each lSite As HspfSupport.HexSite In lExpertSystem.Sites
        '    Dim lSiteName As String = lSite.Name
        '    Dim lArea As Double = lSite.Area
        '    If SimID = 0 Then
        '        Dim lNewStatObs As New HydrologySensitivityStats
        '        With lNewStatObs
        '            .SimID = SimID
        '            .Scenario = "Observed"
        '            Dim obsTimeSeriescfs As atcData.atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(1)),
        '                                                                  lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
        '            obsTimeSeriescfs = Aggregate(obsTimeSeriescfs, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
        '            'obsTimeSeriescfs.Attributes.DiscardCalculated()
        '            Dim obsTimeSeriesInches As atcTimeseries = HspfSupport.CfsToInches(obsTimeSeriescfs, lArea)

        '            .SiteName = lSiteName
        '            .AverageAnnualcfs = obsTimeSeriescfs.Attributes.GetValue("SumAnnual")
        '            obsTimeSeriescfs = Aggregate(obsTimeSeriescfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
        '            .AnnualPeakFlow = obsTimeSeriescfs.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '            .AverageAnnual = obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value / YearsofSimulation
        '            .TenPercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
        '                                - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum90").Value) / YearsofSimulation
        '            .TwentyFivePercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
        '                                - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum75").Value) / YearsofSimulation
        '            .FiftyPercentHigh = (obsTimeSeriesInches.Attributes.GetDefinedValue("Sum").Value _
        '                                   - obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% high
        '            .FiftyPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum50").Value) / YearsofSimulation '50% low
        '            .TwentyFivePercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum25").Value) / YearsofSimulation '25% low
        '            .TenPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum10").Value) / YearsofSimulation '10% low
        '            .FivePercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum05").Value) / YearsofSimulation '5% low
        '            .TwoPercentLow = (obsTimeSeriesInches.Attributes.GetDefinedValue("%Sum02").Value) / YearsofSimulation '2% low

        '            ExpertStatsOutputLine = "Observed,Obs,Obs,Obs,Obs, " &
        '                    lSiteName & ", " & FormatNumber(.AverageAnnualcfs, 3, , , TriState.False) &
        '                    ", " & FormatNumber(.AverageAnnual, 3) & ", " &
        '                    FormatNumber(.TenPercentHigh, 3) & ", " & FormatNumber(.TwentyFivePercentHigh, 3) & ", " &
        '                    FormatNumber(.FiftyPercentHigh, 3) & ", " & FormatNumber(.FiftyPercentLow, 3) & ", " &
        '                    FormatNumber(.TwentyFivePercentLow, 3) & ", " & FormatNumber(.TenPercentLow, 3) & ", " &
        '                    FormatNumber(.FivePercentLow, 3) & ", " & FormatNumber(.TwoPercentLow, 3) & ", " &
        '                    FormatNumber(.AnnualPeakFlow, 3, , , TriState.False) & vbCrLf
        '        End With
        '        lStats.Add(lNewStatObs)
        '    End If
        '    Dim lNewStat As New SensitivityStats
        '    With lNewStat
        '        If SimID = 0 Then
        '            .Scenario = "Baseline"
        '        Else
        '            .Scenario = "Sensitivity"
        '        End If
        '        Dim lSimTSerInches As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)),
        '                                                           lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
        '        Dim lSimTSercfs As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.DSN(0)),
        '                                                        lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing) * 0.042 * lArea
        '        Dim MaxSimTSercfs = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)

        '        'lSeasons.SetSeasonalAttributes(lSimTSercfs, lSeasonalAttributes, lYearlyAttributes)
        '        .SimID = SimID
        '        .SiteName = lSiteName
        '        .AverageAnnualcfs = lSimTSercfs.Attributes.GetValue("SumAnnual")
        '        lSimTSercfs = Aggregate(lSimTSercfs, atcTimeUnit.TUYear, 1, atcTran.TranMax)
        '        .AnnualPeakFlow = lSimTSercfs.Attributes.GetValue("SumAnnual")
        '        .AverageAnnual = lSimTSerInches.Attributes.GetValue("SumAnnual")
        '        .TenPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
        '                          lSimTSerInches.Attributes.GetValue("%Sum90")) _
        '                        / YearsofSimulation '10% high
        '        .TwentyFivePercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
        '                                 lSimTSerInches.Attributes.GetValue("%Sum75")) _
        '                                 / YearsofSimulation '25% high
        '        .FiftyPercentHigh = (lSimTSerInches.Attributes.GetValue("Sum") -
        '                            lSimTSerInches.Attributes.GetValue("%Sum50")) _
        '                        / YearsofSimulation '50% high
        '        .FiftyPercentLow = lSimTSerInches.Attributes.GetValue("%Sum50") / YearsofSimulation '50% low
        '        .TwentyFivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum25") / YearsofSimulation '25% low
        '        .TenPercentLow = lSimTSerInches.Attributes.GetValue("%Sum10") / YearsofSimulation '10% low
        '        .FivePercentLow = lSimTSerInches.Attributes.GetValue("%Sum05") / YearsofSimulation '5% low
        '        .TwoPercentLow = lSimTSerInches.Attributes.GetValue("%Sum02") / YearsofSimulation '2% low
        '        'calculating error terms value
        '        Dim ObsValuesList As List(Of SensitivityStats) = lStats.FindAll(Function(x) (x.Scenario = "Observed" And x.SiteName = lSiteName.ToString))
        '        If ObsValuesList.Count > 0 Then
        '            Dim lObsValues As SensitivityStats = ObsValuesList(0)
        '            .ErrorAverageAnnual = (.AverageAnnual - lObsValues.AverageAnnual) * 100 / lObsValues.AverageAnnual
        '        End If

        '        '.ErrorTenPercentHigh = (TenPercentHigh - obsTenPercentHigh(lSite.Name)) * 100 / obsTenPercentHigh(lSite.Name)
        '        '.ErrorTwentyFivePercentHigh = (TwentyFivePercentHigh - obsTwentyFivePercentHigh(lSite.Name)) * 100 / obsTwentyFivePercentHigh(lSite.Name)
        '        '.ErrorFiftyPercentHigh = (FiftyPercentHigh - obsFiftyPercentHigh(lSite.Name)) * 100 / obsFiftyPercentHigh(lSite.Name)
        '        '.ErrorFiftyPercentLow = (FiftyPercentLow - obsFiftyPercentLow(lSite.Name)) * 100 / obsFiftyPercentLow(lSite.Name)
        '        '.ErrorTwentyFivePercentLow = (TwentyFivePercentLow - obsTwentyFivePercentLow(lSite.Name)) * 100 / obsTwentyFivePercentLow(lSite.Name)
        '        '.ErrorTenPercentLow = (TenPercentLow - obsTenPercentLow(lSite.Name)) * 100 / obsTenPercentLow(lSite.Name)
        '        '.ErrorFivePercentLow = (FivePercentLow - obsFivePercentLow(lSite.Name)) * 100 / obsFivePercentLow(lSite.Name)
        '        '.ErrorTwoPercentLow = (TwoPercentLow - obsTwoPercentLow(lSite.Name)) * 100 / obsTwoPercentLow(lSite.Name)
        '        '.ErrorAnnualPeakFlow = (AnnualPeakFlow - obsAnnualPeakFlow(lSite.Name)) * 100 / obsAnnualPeakFlow(lSite.Name)
        '        'ExpertStatsOutputLine = ExpertStatsOutputLine & SimID & ", " & pOper & "," & oTable & ", " & _
        '        '            oParameter & ", " & Value & ", " & lSiteName & ", " & _
        '        '            FormatNumber(AverageAnnualcfs, 3, , , TriState.False) & ", " & _
        '        '            FormatNumber(AverageAnnual, 3) & ", " & _
        '        '            FormatNumber(TenPercentHigh, 3) & ", " & FormatNumber(TwentyFivePercentHigh, 3) & ", " & _
        '        '            FormatNumber(FiftyPercentHigh, 3) & ", " & FormatNumber(FiftyPercentLow, 3) & ", " & _
        '        '            FormatNumber(TwentyFivePercentLow, 3) & ", " & FormatNumber(TenPercentLow, 3) & ", " & _
        '        '            FormatNumber(FivePercentLow, 3) & ", " & FormatNumber(TwoPercentLow, 3) & ", " & _
        '        '            FormatNumber(AnnualPeakFlow, 3, , , TriState.False) & ", " & _
        '        '            FormatNumber(ErrorAverageAnnual, 1) & ", " & FormatNumber(ErrorTenPercentHigh, 1) & ", " & _
        '        '            FormatNumber(ErrorTwentyFivePercentHigh, 1) & ", " & FormatNumber(ErrorFiftyPercentHigh, 1) & ", " & _
        '        '            FormatNumber(ErrorFiftyPercentLow, 1) & ", " & FormatNumber(ErrorTwentyFivePercentLow, 1) & ", " & _
        '        '            FormatNumber(ErrorTenPercentLow, 1) & ", " & FormatNumber(ErrorFivePercentLow, 1) & ", " & _
        '        '            FormatNumber(ErrorTwoPercentLow, 1) & ", " & FormatNumber(ErrorAnnualPeakFlow, 1) & vbCrLf
        '        'Saving the relevant output in a text string to add it to the text file
        '        lStats.Add(lNewStat)


        '        IO.File.AppendAllText(pBaseName & "_HydrologyOutput.csv", ExpertStatsOutputLine)

        '        ExpertStatsOutputLine = ""
        '    End With
        'Next lSite

        Return lOutputline

    End Function
    Private Function Observed()
        Return "Observed"
    End Function
End Module
