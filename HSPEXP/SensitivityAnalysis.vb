Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports System.IO

Public Class TimeSeriesStats
    'These statistics are calculated for each individual time series for each individual run
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
    'This class manages the property of each individual parameter
    Public ParmID As Integer = 0
    Public ParmOperationType As String = ""
    Public ParmOperationNumber As Integer = 0
    Public ParmOperationName As String = ""
    Public ParmTable As String = ""
    Public ParmName As String = ""
    Public ParmMultFactor As Integer = 0
    Public ParmLow As Double = -1.0E-30
    Public ParmHigh As Double = 1.0E+30
    Public ParmOccurence As Integer = 0
End Class


Public Module MultiSimAnalysis
    Sub SensitivityAnalysis(ByVal aHSPFEXE As String, ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As Double, ByVal aHSPFEchofilename As String)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm") 'Need to figure out why we need this.
        Dim lExitCode As Integer
        Dim SimID As Integer = 0
        Dim lUci As New atcUCI.HspfUci
        Dim DeleteUCIFiles As Integer = 0
        Dim uciName As String
        uciName = "MultiSim.uci"
        IO.File.Copy(pBaseName & ".uci", uciName, True) 'Saving the original file with MultiSim.uci name
        lUci.ReadUci(lMsg, uciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file
        lUci.GlobalBlock.RunFg = 1
        lUci.Save()

        Dim loutWDMFileName As String = Path.Combine(pTestPath, "MultiSim.wdm")
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
        Dim lSpecificationFile As String = "MultiSimSpecFile.csv"
        Dim lOutputFile As StreamWriter = File.CreateText("MultiSimOutput.csv")
        lOutputFile.WriteLine("SimID,DSNID,Sum,AnnualSum,Mean,GeometricMean,Av.AnnualPeak,10%High,25%High,50%High,50%Low,25%Low,10%Low,5%Low,2%Low,99.9%,99%,98%,95%,90%,80%,70%,60%,50%,40%,30%,20%,10%,5%,2%,1%,0.1%")


        Dim NumberOfOutputDSN As Integer = 0

        If Not IO.File.Exists(lSpecificationFile) Then
            Dim SensitivityParameterFile As StreamWriter = IO.File.CreateText("MultiSimSpecFile.csv")
            Dim TextToAddForMultiSimFile As String = "***Generic Parameter List For Sensitivity/Uncertainty Analysis And Output DSN from the UCI file"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "***Following Output IDs are the first two ID read from the EXT TARGET block of the UCI file. You can add more if you want to."
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)

            TextToAddForMultiSimFile = "DSN,"
            For Each lEXTTARGET As HspfConnection In lUci.Connections

                If lEXTTARGET.Target.VolName.Contains("WDM") AndAlso NumberOfOutputDSN <= 2 Then
                    NumberOfOutputDSN += 1
                    TextToAddForMultiSimFile &= lEXTTARGET.Target.VolId & ","

                End If
            Next

            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = """***The operation number, land use, tied with next, and multiplier can be left blank"""
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "PARMID,OPN_TYPE,TABLE_NAME,PARM_NAME,OCCURENCE,MULT_FACTOR?,OPN_NUMBER_NAME,LOWERLIMIT,UPPERLIMIT"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "1,PERLND,PWAT-PARM2,LZSN,,1,,3,8"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "2,PERLND,PWAT-PARM2,INFILT,,1,,0.01,0.5"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "Delete intermediate UCI files?, 0"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "***Following lines list the multiplication factor for each parameter for each simulation."
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "Simulation ID,Parameter1,Parameter2,,,,,"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "1,0.9,,"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "2,1.1,,"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "3,,0.9,"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "4,,1.1,"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)


            MsgBox("A default list of Sensitive parameters was created as MultiSimSpecFile.csv. Additionally two output DSN from the EXT TARGETS blocks were selected for analyzing outputs.
You can edit this specification file and add more parameters and outputs.", vbOKOnly, "Default List of Outputs & Parameters")
            SensitivityParameterFile.Close()
        End If

        'The file listing the sensitive parameters and the output DSN already exists.
        Dim lSpecificationFileRecordsNew As New ArrayList
        lSpecificationFileRecordsNew = ReadCSVFile(lSpecificationFile) 'Reading the MultiSim specification file 

        Dim listOfOutputDSN As New atcCollection
        Dim lcsvRecordIndex As Integer = 0
        Dim lcsvlinerecord() As String = lSpecificationFileRecordsNew(lcsvRecordIndex)
        NumberOfOutputDSN = lcsvlinerecord.Length - 1
        For i = 1 To NumberOfOutputDSN
            If lcsvlinerecord(i) = "" Then
                NumberOfOutputDSN = i - 1
                Exit For
            Else

                listOfOutputDSN.Add(Int(lcsvlinerecord(i)))
                'If DSN is not an integer, it will cause an error here. Need to figure out the best way to handle it.
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
        Do Until lcsvlinerecord(0) = "Simulation ID" Or lcsvlinerecord(0).ToLower.Contains("delete")
            'Going through the CSV file and getting the list of parameters.
            Dim NewParameter As New ModelParameter
            With NewParameter
                Try
                    .ParmID = Trim(Int(lcsvlinerecord(0))) 'Need to figure out better error messages if the values are not correct.
                Catch ex As Exception

                End Try
                If Not Trim(lcsvlinerecord(1)).Length = 0 Then
                    .ParmOperationType = Trim(lcsvlinerecord(1))
                End If

                .ParmTable = Trim(lcsvlinerecord(2))

                If lcsvlinerecord.Length > 3 AndAlso Not Trim(lcsvlinerecord(3)).Length = 0 Then
                    .ParmName = Trim(lcsvlinerecord(3))
                End If

                Try
                    .ParmOccurence = CInt(Trim(lcsvlinerecord(4)))
                Catch
                    .ParmOccurence = 0
                End Try

                .ParmMultFactor = 0

                Try
                    .ParmMultFactor = CInt(Trim(lcsvlinerecord(5)))
                Catch
                    .ParmMultFactor = 0
                End Try

                'Assume that the fixed value for parameters will be provided for a MultiSim analysis for each individual simulation.

                If .ParmTable.ToLower.StartsWith("mon-") Then .ParmMultFactor = 1 'If a monthly table is provided, a multiplication factor has to be provided.

                If Not Trim(lcsvlinerecord(6)).Length = 0 Then
                    Try
                        .ParmOperationNumber = CInt(Trim(lcsvlinerecord(6)))
                        .ParmOperationName = ""
                    Catch
                        .ParmOperationName = Trim(lcsvlinerecord(6))
                        .ParmOperationNumber = 0
                    End Try

                End If

                Try
                    .ParmLow = CDbl(Trim(lcsvlinerecord(7)))
                Catch
                End Try

                Try
                    .ParmHigh = CDbl(Trim(lcsvlinerecord(8)))
                Catch
                End Try

            End With

            ListOfParameters.Add(NewParameter)
            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
        Loop

        Do
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
            If lcsvlinerecord(0).ToLower.Contains("delete") Then
                DeleteUCIFiles = CInt(Trim(lcsvlinerecord(1)))
                lcsvRecordIndex += 1

            End If
            lcsvRecordIndex += 1
            lcsvlinerecord = lSpecificationFileRecordsNew(lcsvRecordIndex)
            SimID = lcsvlinerecord(0)

            IO.File.Copy(Path.Combine(pTestPath, uciName), Path.Combine(pTestPath, SimID & uciName), True)
            lUci = New HspfUci
            lUci.ReadUci(lMsg, Path.Combine(pTestPath, SimID & uciName), -1, False, Path.Combine(pTestPath, pBaseName & ".ech"))

            Dim MFactorOrParmValue As Double = 0.0

            For Each Parm As ModelParameter In ListOfParameters

                Try
                    MFactorOrParmValue = CDbl(lcsvlinerecord((Parm.ParmID)))
                Catch
                    MFactorOrParmValue = Double.NaN
                End Try
                'If SimID = 8 Then Stop

                If Not (Double.IsNaN(MFactorOrParmValue) Or (Parm.ParmMultFactor = 0 AndAlso MFactorOrParmValue = 1.0)) Then
                    With Parm
                        ChangeUCIParameterAndSave(lUci, .ParmOperationType, .ParmOperationNumber, .ParmOperationName,
                                                         .ParmTable, .ParmName, .ParmOccurence, .ParmLow, .ParmHigh, .ParmMultFactor, MFactorOrParmValue)

                    End With
                End If

            Next

            loutputLine = ModelRunandReportAnswers(SimID, lUci, SimID & uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, lStats, listOfOutputDSN, aHSPFEchofilename)

            lOutputFile.Write(loutputLine)

            If DeleteUCIFiles = 1 Then
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
                                    Trim(MyReader.PeekChars(10000).ToLower.StartsWith("parmid")) Or
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
                                               ByVal aOperationNumber As Integer,
                                               ByVal aOperationName As String,
                                               ByVal aTableName As String,
                                               ByVal aParameterName As String,
                                               ByVal aParameterOccurrence As Integer,
                                               ByVal aLowerLimit As Double,
                                               ByVal aUpperLimit As Double,
                                               ByVal aParmAbsolute As Int16,
                                               ByVal aMFactorOrParmValue As Double
                                               )
        Select Case True
            Case aTableName.Contains("MASS-LINK")
                Dim lMassLinkID As Integer
                lMassLinkID = aParameterOccurrence
                Dim lMassLinkParts(7) As String
                If aOperationName.Length > 0 Then
                    lMassLinkParts = aOperationName.Split(":")
                    ReDim Preserve lMassLinkParts(7)

                End If

                For Each lMasslink As HspfMassLink In aUCI.MassLinks

                    If lMasslink.MassLinkId = lMassLinkID AndAlso
                        (lMasslink.Source.Group = lMassLinkParts(0) Or IsNothing(lMassLinkParts(0))) AndAlso
                        (lMasslink.Source.Member = lMassLinkParts(1) Or IsNothing(lMassLinkParts(1))) AndAlso
                        (lMasslink.Source.MemSub1 = lMassLinkParts(2) Or IsNothing(lMassLinkParts(2))) AndAlso
                        (lMasslink.Source.MemSub2 = lMassLinkParts(3) Or IsNothing(lMassLinkParts(3))) AndAlso
                        (lMasslink.Target.Group = lMassLinkParts(4) Or IsNothing(lMassLinkParts(4))) AndAlso
                        (lMasslink.Target.Member = lMassLinkParts(5) Or IsNothing(lMassLinkParts(5))) AndAlso
                        (lMasslink.Target.MemSub1 = lMassLinkParts(6) Or IsNothing(lMassLinkParts(6))) AndAlso
                        (lMasslink.Target.MemSub2 = lMassLinkParts(7) Or IsNothing(lMassLinkParts(7))) Then


                        lMasslink.MFact = HspfTable.NumFmtRE(lMasslink.MFact * aMFactorOrParmValue, 5)
                    End If
                Next

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
                        Case ""

                    End Select
                    Try
                        If aParmAbsolute = 0 Then
                            lMetSeg.MetSegRecs(MetSegRec).MFactP = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(MetSegRec).MFactP * aMFactorOrParmValue, 5)

                            lMetSeg.MetSegRecs(MetSegRec).MFactR = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(MetSegRec).MFactR * aMFactorOrParmValue, 5)
                        Else
                            lMetSeg.MetSegRecs(MetSegRec).MFactP = HspfTable.NumFmtRE(aMFactorOrParmValue, 3)

                            lMetSeg.MetSegRecs(MetSegRec).MFactR = HspfTable.NumFmtRE(aMFactorOrParmValue, 3)
                        End If

                    Catch ex As Exception
                        Logger.Msg("Could not find the Table " & aTableName &
                                   " Parameter " & aParameterName & " for " & lMetSeg.Name, MsgBoxStyle.Critical)
                        End
                    End Try
                Next

            Case Else
                For Each lOper As HspfOperation In aUCI.OpnBlks(aParameterOperation).Ids
                    If (lOper.Id = aOperationNumber OrElse lOper.Description = aOperationName OrElse (aOperationNumber = 0 AndAlso aOperationName = "")) Then
                        Try
                            Dim lTable As HspfTable

                            If aParameterOccurrence = 0 Or aParameterOccurrence = 1 Then
                                lTable = lOper.Tables(aTableName)
                            Else
                                lTable = lOper.Tables(aTableName & ":" & aParameterOccurrence)
                            End If


                            If lTable.Name.Contains("MON-") Then
                                For Mon = 0 To 11 'For monthly parameters, only a multiplication factor can be provided

                                    lTable.Parms(Mon).Value = HspfTable.NumFmtRE(lTable.Parms(Mon).Value * aMFactorOrParmValue, 5)
                                    If (lTable.Parms(Mon).Value < aLowerLimit) Then
                                        lTable.Parms(Mon).Value = aLowerLimit
                                    ElseIf (lTable.Parms(Mon).Value > aUpperLimit) Then
                                        lTable.Parms(Mon).Value = aUpperLimit
                                    End If
                                Next
                            Else
                                If aParmAbsolute = 0 Then
                                    lTable.ParmValue(aParameterName) = HspfTable.NumFmtRE(lTable.ParmValue(aParameterName) * aMFactorOrParmValue, lTable.Parms(aParameterName).Def.Length)
                                Else
                                    lTable.ParmValue(aParameterName) = HspfTable.NumFmtRE(aMFactorOrParmValue, lTable.Parms(aParameterName).Def.Length)
                                    If (lTable.ParmValue(aParameterName) < aLowerLimit) Then
                                        lTable.ParmValue(aParameterName) = aLowerLimit
                                    ElseIf (lTable.ParmValue(aParameterName) > aUpperLimit) Then
                                        lTable.ParmValue(aParameterName) = aUpperLimit
                                    End If
                                End If

                            End If
                        Catch ex As Exception
                            Logger.Msg("Could not find the Table " & aTableName &
                                       " Parameter " & aParameterName & " for " & lOper.Id, MsgBoxStyle.Critical)
                            End
                        End Try

                    End If
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
                Try
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
                Catch
                    Logger.Dbg("Could not compute statistics for ID" & WDMDataset)
                End Try

            Next WDMDataset
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
