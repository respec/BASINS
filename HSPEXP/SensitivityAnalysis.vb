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

Public Class TimeSeriesStats
    Public SimID As Integer
    Public DSNID As Integer
    Public OverallSum As Double
    Public AnnualSum As Double
    Public Mean As Double
    Public GeometricMean As String
    Public AvAnnPeak As Double
    Public TenPercentHigh As Double
    Public TwentyFivePercentHigh As Double
    Public FiftyPercentHigh As Double
    Public FiftyPercentLow As Double
    Public TwentyFivePercentLow As Double
    Public TenPercentLow As Double
    Public FivePercentLow As Double
    Public TwoPercentLow As Double
    Public Exceed01 As Double
    Public Exceed1 As Double
    Public Exceed2 As Double
    Public Exceed5 As Double
    Public Exceed10 As Double
    Public Exceed20 As Double
    Public Exceed30 As Double
    Public Exceed40 As Double
    Public Exceed50 As Double
    Public Exceed60 As Double
    Public Exceed70 As Double
    Public Exceed80 As Double
    Public Exceed90 As Double
    Public Exceed95 As Double
    Public Exceed98 As Double
    Public Exceed99 As Double
    Public Exceed999 As Double
End Class

Public Class ModelParameter
    Public ParmID As Integer = 0
    Public ParmOperation As String = ""
    Public ParmOperationNumber As Integer = 0
    Public ParmLandUseName As String = ""
    Public ParmTable As String = ""
    Public ParmName As String = ""
    Public ParmLow As Double = -1.0E-30
    Public ParmHigh As Double = 1.0E+30
    Public ParmOccurence As Integer = 1
End Class


Public Module ModSensitivityAnalysis
    Sub SensitivityAnalysis(ByVal aHSPFEXE As String, ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As Double, ByVal aHSPFEchofilename As String)

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
        lUci.GlobalBlock.RunFg = 1
        lUci.Save()

        Dim loutWDMFileName As String = Path.Combine(pTestPath, pBaseName & "_UA.wdm")
        If File.Exists(loutWDMFileName) Then
            Try
                File.Delete(loutWDMFileName)
            Catch ex As Exception
            End Try
        End If


        Dim lStats As New Generic.List(Of TimeSeriesStats)
        Dim YearsofSimulation As Single
        YearsofSimulation = (aEDateJ - aSDateJ) / JulianYear

        'Generate a default Parameter Sensitivity Table file if it does not exist already
        Dim lParameterListFilesIsAvailable As Boolean = False
        Dim lSpecificationFile As String = "SensitiveParametersList.csv"
        Dim lOutputFile As StreamWriter = File.CreateText("SensitivityOutput.csv")
        lOutputFile.WriteLine("SimID,DSNID,Sum,AnnualSum,Mean,GeometricMean,Av.AnnualPeak,10%High,25%High,50%High,50%Low,25%Low,10%Low,5%Low,2%Low,99.9%,99%,98%,95%,90%,80%,70%,60%,50%,40%,30%,20%,10%,5%,2%,1%,0.1%")


        Dim NumberOfOutputDSN As Integer = 0

        If Not IO.File.Exists(lSpecificationFile) Then
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
            TextToAddForSensitivityFile = "Parameter,OPERATION,OPERATION NUMBER,LANDUSE,TABLE,PARM,OCCURENCE,LOWERLIMIT,UPPERLIMIT"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "1,PERLND,,,PWAT-PARM2,LZSN,,3,8"
            SensitivityParameterFile.WriteLine(TextToAddForSensitivityFile)
            TextToAddForSensitivityFile = "2,PERLND,,,PWAT-PARM2,INFILT,,0.01,0.5"
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
        lSpecificationFileRecordsNew = ReadCSVFile(lSpecificationFile) 'Reading the UA specification file 

        Dim listOfOutputDSN As New atcCollection
        Dim lcsvRecordIndex As Integer = 0
        Dim lcsvlinerecord() As String = lSpecificationFileRecordsNew(lcsvRecordIndex)
        NumberOfOutputDSN = lcsvlinerecord.Length - 1
        For i = 1 To NumberOfOutputDSN
            If lcsvlinerecord(i) = "" Then
                NumberOfOutputDSN = i - 1 'Should edit the code so that it add DSN only if they are integers
                Exit For
            Else
                listOfOutputDSN.Add(lcsvlinerecord(i))
            End If

        Next

        Dim loutputLine As String = ""
        ChDriveDir(PathNameOnly(pHSPFExe))
        loutputLine = ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, lStats, listOfOutputDSN, aHSPFEchofilename)
        lOutputFile.Write(loutputLine)
        Dim ListOfParameters As New List(Of ModelParameter)
        lcsvRecordIndex += 1
        lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
        Do Until lcsvlinerecord(0) = "Simulation ID"
            'Going through the CSV file and getting the list of parameters.
            Dim NewParameter As New ModelParameter
            With NewParameter
                .ParmID = Trim(lcsvlinerecord(0)) 'Need to figure out better error messages if the values are not correct.

                If Not Trim(lcsvlinerecord(1)).Length = 0 Then
                    .ParmOperation = Trim(lcsvlinerecord(1))
                End If

                If Not Trim(lcsvlinerecord(2)).Length = 0 Then
                    .ParmOperationNumber = CInt(Trim(lcsvlinerecord(2)))
                End If

                If Not Trim(lcsvlinerecord(3)).Length = 0 Then
                    .ParmLandUseName = Trim(lcsvlinerecord(3))
                End If

                .ParmTable = lcsvlinerecord(4)

                If lcsvlinerecord.Length > 5 AndAlso Not Trim(lcsvlinerecord(5)).Length = 0 Then
                    .ParmName = Trim(lcsvlinerecord(5))
                End If

                If lcsvlinerecord.Length > 6 AndAlso Not Trim(lcsvlinerecord(6)).Length = 0 Then
                    .ParmOccurence = CInt(Trim(lcsvlinerecord(6)))
                End If

                If lcsvlinerecord.Length > 7 AndAlso Not Trim(lcsvlinerecord(7)).Length = 0 AndAlso Not Trim(lcsvlinerecord(8)).Length = 0 Then
                    .ParmLow = CDbl(Trim(lcsvlinerecord(7)))
                    .ParmHigh = CDbl(Trim(lcsvlinerecord(8)))
                End If

            End With

            ListOfParameters.Add(NewParameter)
            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
        Loop

        Dim MultiplicationFactor As Double = 0

        Do

            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
            SimID = lcsvlinerecord(0)

            IO.File.Copy(Path.Combine(pTestPath, uciName), Path.Combine(pTestPath, SimID & uciName), True)
            lUci = New HspfUci
            lUci.ReadUci(lMsg, Path.Combine(pTestPath, SimID & uciName), -1, False, Path.Combine(pTestPath, pBaseName & ".ech"))

            For Each Parm As ModelParameter In ListOfParameters
                Try
                    MultiplicationFactor = lcsvlinerecord(Parm.ParmID)
                Catch
                    MultiplicationFactor = 1.0
                End Try

                If MultiplicationFactor <> 1.0 Then
                    With Parm
                        ChangeUCIParameterAndSave(lUci, .ParmOperation, .ParmOperationNumber, .ParmLandUseName,
                                                         .ParmTable, .ParmName, .ParmOccurence, .ParmLow, .ParmHigh, MultiplicationFactor)

                    End With
                End If

            Next

            loutputLine = ModelRunandReportAnswers(SimID, lUci, SimID & uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, lStats, listOfOutputDSN, aHSPFEchofilename)
            'lUci = Nothing


            lOutputFile.Write(loutputLine)


            If Not SaveUCIFiles Then
                File.Delete(SimID & uciName)
            End If



        Loop While lcsvRecordIndex < lSpecificationFileRecordsNew.Count - 1

        lOutputFile.Close()

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

    Private Sub ChangeUCIParameterAndSave(ByRef aUCI As atcUCI.HspfUci,
                                               ByVal aParameterOperation As String,
                                               ByVal aOperationNumber As String,
                                               ByVal aLandUsename As String,
                                               ByVal aTableName As String,
                                               ByVal aParameterName As String,
                                               ByVal aParameterOccurrence As Integer,
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
                    Try
                        lMetSeg.MetSegRecs(MetSegRec).MFactP = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactP _
                                                                                             * aMultiplicationFactor, 3)

                        lMetSeg.MetSegRecs(MetSegRec).MFactR = SignificantDigits(lMetSeg.MetSegRecs(MetSegRec).MFactR _
                                                                                             * aMultiplicationFactor, 3)
                    Catch ex As Exception
                        Logger.Msg("Could not find the Table " & aTableName &
                                   " Parameter " & aParameterName & " for " & lMetSeg.Name, MsgBoxStyle.Critical)
                        End

                    End Try

                Next

            Case aTableName.Contains("MON-")
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    If aLandUsename = "" OrElse lOper.Description = aLandUsename Then
                        For Mon = 0 To 11
                            Try
                                lOper.Tables(aTableName).Parms(Mon).Value = SignificantDigits(lOper.Tables(aTableName).Parms(Mon).Value _
                                                                                                      * aMultiplicationFactor, 2)
                                If (lOper.Tables(aTableName).Parms(Mon).Value < aLowerLimit) Then
                                    lOper.Tables(aTableName).Parms(Mon).Value = aLowerLimit
                                ElseIf (lOper.Tables(aTableName).Parms(Mon).Value > aUpperLimit) Then
                                    lOper.Tables(aTableName).Parms(Mon).Value = aUpperLimit
                                End If

                            Catch ex As Exception
                                Logger.Msg("Could not find the Table " & aTableName &
                                   " Parameter " & aParameterName & " for " & lOper.Id, MsgBoxStyle.Critical)

                                End
                            End Try


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
                    'If lMassLinkID = 21 Then

                    '    If lMasslink.MassLinkId = 1 AndAlso lMasslink.Source.Group.Contains("IQUAL") Then
                    '        lMasslink.MFact = SignificantDigits(lMasslink.MFact * aMultiplicationFactor, 3)
                    '    End If
                    'End If
                    'If aParameterName = "NITR" Then
                    '    If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Source.Group.Contains("PHOS") Then
                    '        lMasslink.MFact = SignificantDigits(lMasslink.MFact * aMultiplicationFactor, 3)
                    '    End If
                    'End If
                Next

            Case Else
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    Try
                        lOper.Tables(aTableName).ParmValue(aParameterName) =
                                            SignificantDigits(lOper.Tables(aTableName).ParmValue(aParameterName) * aMultiplicationFactor, 3)
                        If (lOper.Tables(aTableName).ParmValue(aParameterName) < aLowerLimit) Then
                            lOper.Tables(aTableName).ParmValue(aParameterName) = aLowerLimit
                        ElseIf (lOper.Tables(aTableName).ParmValue(aParameterName) > aUpperLimit) Then
                            lOper.Tables(aTableName).ParmValue(aParameterName) = aUpperLimit
                        End If
                    Catch ex As Exception
                        Logger.Msg("Could not find the Table " & aTableName &
                                   " Parameter " & aParameterName & " for " & lOper.Id, MsgBoxStyle.Critical)
                        End
                    End Try

                    'ParameterDetailText &= SimID & "," & ParameterOperation & "," & TableName & "," & lOper.Id & "," &
                    '                                                                ParameterName & ",," &
                    '                                                                lOper.Tables(TableName).ParmValue(ParameterName) & vbCrLf
                Next
        End Select
        aUCI.GlobalBlock.RunFg = 1

        aUCI.Save()




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
                                 ByRef lStats As List(Of TimeSeriesStats),
                                 ByVal aOutputDSN As atcCollection,
                                 ByRef aHSPFEchofileName As String)

        lExitCode = LaunchProgram(pHSPFExe, pTestPath, "-1 -1 " & uciName) 'Run HSPF program
        'If lExitCode = -1 Then
        '    Throw New ApplicationException("winHSPFLt could not run, Analysis cannot continue")
        '    End
        'End If

        Dim HSPFRan As Boolean = False
        Dim lRunMade As String
        Dim echoFileInfo As System.IO.FileInfo
        If IO.File.Exists(aHSPFEchofileName) Then
            echoFileInfo = New System.IO.FileInfo(aHSPFEchofileName)
            lRunMade = echoFileInfo.LastWriteTime.ToString
            Using echoFileReader As StreamReader = File.OpenText(aHSPFEchofileName)
                While Not echoFileReader.EndOfStream
                    Dim nextLine As String = echoFileReader.ReadLine()
                    If Not nextLine.ToUpper.Contains("END OF JOB") Then
                        HSPFRan = False
                    Else
                        HSPFRan = True
                    End If
                End While
            End Using
        Else
            Logger.Msg("The ECHO file is not available for this model. Please check if model ran successfully last time", vbCritical)
            End
        End If

        Dim lOutputline As String = ""

        If HSPFRan Then
            Dim lWdmFileName As String = pBaseName & ".wdm"
            Dim lOutWDMFileName As String = pBaseName & "_UA.wdm"

            Dim lWdmDataSource As atcWDM.atcDataSourceWDM
            Dim lWdmDataSource2 As atcWDM.atcDataSourceWDM

            If atcDataManager.DataSources.Contains(pTestPath & lWdmFileName) Then
                atcDataManager.RemoveDataSource(pTestPath & lWdmFileName)
            End If
            If atcDataManager.DataSources.Contains(pTestPath & lOutWDMFileName) Then
                atcDataManager.RemoveDataSource(pTestPath & lOutWDMFileName)
            End If

            atcDataManager.OpenDataSource(pTestPath & lWdmFileName)
            lWdmDataSource = atcDataManager.DataSourceBySpecification(pTestPath & lWdmFileName)
            atcDataManager.OpenDataSource(pTestPath & lOutWDMFileName)
            lWdmDataSource2 = atcDataManager.DataSourceBySpecification(pTestPath & lOutWDMFileName)


            If lWdmDataSource2 Is Nothing Then
                lWdmDataSource2 = New atcWDM.atcDataSourceWDM
                lWdmDataSource2.Open(pTestPath & lOutWDMFileName)
            End If

            Dim lYearlyAttributes As New atcDataAttributes

            Dim DatasetID As Integer = lWdmDataSource2.DataSets.Count
            For Each WDMDataset As Integer In aOutputDSN
                Dim SimulatedTS As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(WDMDataset), aSDateJ, aEDateJ, Nothing)
                DatasetID += 1
                If DatasetID < 10000 Then
                    SimulatedTS.Attributes.SetValue("ID", DatasetID)
                    SimulatedTS.Attributes.SetValue("Scenario", "SIMID" & SimID)

                    lWdmDataSource2.AddDataset(SimulatedTS)
                End If

                Dim lNewStatDataset As New TimeSeriesStats
                Dim AnnSimulatedTS As atcTimeseries = Aggregate(SimulatedTS, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                With lNewStatDataset

                    .SimID = SimID
                    .DSNID = WDMDataset
                    .OverallSum = SimulatedTS.Attributes.GetDefinedValue("Sum").Value
                    .AnnualSum = SimulatedTS.Attributes.GetDefinedValue("SumAnnual").Value
                    .Mean = SimulatedTS.Attributes.GetDefinedValue("Mean").Value
                    .GeometricMean = "Not Calculated"
                    .AvAnnPeak = AnnSimulatedTS.Attributes.GetDefinedValue("Mean").Value
                    .TenPercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum90").Value
                    .TwentyFivePercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum75").Value
                    .FiftyPercentHigh = .OverallSum - SimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                    .FiftyPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                    .TwentyFivePercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum25").Value
                    .TenPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum10").Value
                    .FivePercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum05").Value
                    .TwoPercentLow = SimulatedTS.Attributes.GetDefinedValue("%Sum02").Value
                    .Exceed01 = SimulatedTS.Attributes.GetDefinedValue("%0.10").Value
                    .Exceed1 = SimulatedTS.Attributes.GetDefinedValue("%1.00").Value
                    .Exceed2 = SimulatedTS.Attributes.GetDefinedValue("%2.00").Value
                    .Exceed5 = SimulatedTS.Attributes.GetDefinedValue("%5.00").Value
                    .Exceed10 = SimulatedTS.Attributes.GetDefinedValue("%10.00").Value
                    .Exceed20 = SimulatedTS.Attributes.GetDefinedValue("%20.00").Value
                    .Exceed30 = SimulatedTS.Attributes.GetDefinedValue("%30.00").Value
                    .Exceed40 = SimulatedTS.Attributes.GetDefinedValue("%40.00").Value
                    .Exceed50 = SimulatedTS.Attributes.GetDefinedValue("%50.00").Value
                    .Exceed60 = SimulatedTS.Attributes.GetDefinedValue("%60.00").Value
                    .Exceed70 = SimulatedTS.Attributes.GetDefinedValue("%70.00").Value
                    .Exceed80 = SimulatedTS.Attributes.GetDefinedValue("%80.00").Value
                    .Exceed90 = SimulatedTS.Attributes.GetDefinedValue("%90.00").Value
                    .Exceed95 = SimulatedTS.Attributes.GetDefinedValue("%95.00").Value
                    .Exceed98 = SimulatedTS.Attributes.GetDefinedValue("%98.00").Value
                    .Exceed99 = SimulatedTS.Attributes.GetDefinedValue("%99.00").Value
                    .Exceed999 = SimulatedTS.Attributes.GetDefinedValue("%99.90").Value

                    lOutputline &= .SimID & ", " & .DSNID & ", " & FormatNumber(.OverallSum, 3,, TriState.False, False) &
                        ", " & FormatNumber(.AnnualSum, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Mean, 4,, TriState.False, False) &
                        ", " & .GeometricMean &
                        ", " & FormatNumber(.AvAnnPeak, 4,, TriState.False, False) &
                        ", " & FormatNumber(.TenPercentHigh, 4,, TriState.False, False) &
                        ", " & FormatNumber(.TwentyFivePercentHigh, 4,, TriState.False, False) &
                        ", " & FormatNumber(.FiftyPercentHigh, 4,, TriState.False, False) &
                        ", " & FormatNumber(.FiftyPercentLow, 4,, TriState.False, False) &
                        ", " & FormatNumber(.TwentyFivePercentLow, 4,, TriState.False, False) &
                        ", " & FormatNumber(.TenPercentLow, 4,, TriState.False, False) &
                        ", " & FormatNumber(.FivePercentLow, 4,, TriState.False, False) &
                        ", " & FormatNumber(.TwoPercentLow, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed01, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed1, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed2, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed5, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed10, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed20, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed30, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed40, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed50, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed60, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed70, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed80, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed90, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed95, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed98, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed99, 4,, TriState.False, False) &
                        ", " & FormatNumber(.Exceed999, 4,, TriState.False, False) & vbCrLf

                End With
            Next
            lWdmDataSource.Clear()
            lWdmDataSource2.Clear()
        Else

            lOutputline = SimID & ", This simulation didn't compelete successfully" & vbCrLf

        End If

        Logger.Status("Number of simulations complete: " & SimID)
        Return lOutputline


    End Function
    Private Function Observed()
        Return "Observed"
    End Function
End Module
