Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports System.IO
Imports System.Data

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
    Sub SubMultiSim(ByVal aHSPFEXE As String, ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As Double, ByVal aHSPFEchofilename As String)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm") 'Need to figure out why we need this.
        Dim lExitCode As Integer
        Dim SimID As Integer = 0
        Dim lUci As New atcUCI.HspfUci
        Dim DeleteUCIFiles As Integer = 0
        Dim uciName As String
        uciName = "MultiSim.uci"
        File.Copy(pBaseName & ".uci", uciName, True) 'Saving the original file with MultiSim.uci name
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

        Dim YearsofSimulation As Single
        YearsofSimulation = (aEDateJ - aSDateJ) / JulianYear

        'Generate a default Parameter Sensitivity Table file if it does not exist already
        Dim lParameterListFilesIsAvailable As Boolean = False
        Dim lSpecificationFile As String = "MultiSimSpecFile.csv"

        Dim NumberOfOutputDSN As Integer = 0

        If Not IO.File.Exists(lSpecificationFile) Then
            Dim SensitivityParameterFile As StreamWriter = IO.File.CreateText("MultiSimSpecFile.csv")
            Dim TextToAddForMultiSimFile As String = "***Initial Parameter List For Sensitivity/Uncertainty Analysis And Output DSN from the UCI file"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "***Following Output IDs are the first two ID read from the EXT TARGET block of the UCI file. You can add more."
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)

            TextToAddForMultiSimFile = "DSN,"
            For Each lEXTTARGET As HspfConnection In lUci.Connections

                If lEXTTARGET.Target.VolName.Contains("WDM") AndAlso NumberOfOutputDSN <= 1 Then
                    NumberOfOutputDSN += 1
                    TextToAddForMultiSimFile &= lEXTTARGET.Target.VolId & ","

                End If
            Next

            If NumberOfOutputDSN = 0 Then
                Logger.Msg("The UCI file does not have output datasets specified in the EXT TARGETS Block. HSPEXP+ will quit", vbOKOnly)
                End
            End If

            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = """***The operation number, land use, tied with next, and multiplier can be left blank"""
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "ParmID,OPN_Type,Table_Name,Parm_Name,Occur_Or_MLNumber,Mult_Factor_FG,OPN_Number_Or_Name,Lower_Limit,Upper_Limit"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "1,PERLND,PWAT-PARM2,LZSN,,1,,3,8"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "2,PERLND,PWAT-PARM2,INFILT,,1,,0.01,0.5"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "Delete intermediate UCI files?, 0"
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "***Following lines list the multiplication factor for each parameter for each simulation."
            SensitivityParameterFile.WriteLine(TextToAddForMultiSimFile)
            TextToAddForMultiSimFile = "SimID,Parm1,Parm2,,,,,"
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

        'Dim lOutputFile As StreamWriter = File.CreateText("MultiSimOutput.csv")
        Dim loutputTable As DataTable
        loutputTable = New DataTable("MultiSimOutput")
        loutputTable = AddOutputTableColumns(loutputTable)

        Dim TextToWrite As String = ""
        For Each TableColumn As DataColumn In loutputTable.Columns 'Writing the table headings
            TextToWrite &= TableColumn.Caption & ","
        Next
        'lOutputFile.WriteLine(TextToWrite)

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
        loutputTable = ModelRunandReportAnswers(SimID, lUci, uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, listOfOutputDSN, aHSPFEchofilename, loutputTable)

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
        If File.Exists(Path.Combine(pTestPath, "MultiSim_" & pBaseName & ".wdm")) Then File.Delete(Path.Combine(pTestPath, "MultiSim_" & pBaseName & ".wdm"))

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
                If Not (Double.IsNaN(MFactorOrParmValue) Or (Parm.ParmMultFactor = 0 AndAlso MFactorOrParmValue = 1.0)) Then
                    ChangeUCIParameterAndSave(lUci, Parm, MFactorOrParmValue)
                End If
            Next

            loutputTable = ModelRunandReportAnswers(SimID, lUci, SimID & uciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, YearsofSimulation, listOfOutputDSN, aHSPFEchofilename, loutputTable)

            If DeleteUCIFiles = 1 Then
                File.Delete(SimID & uciName)
            End If

        Loop While lcsvRecordIndex < lSpecificationFileRecordsNew.Count - 1
        Dim lOutputFile2 As StreamWriter = File.CreateText(Path.Combine(pTestPath, "MultiSimOutput.xml"))
        loutputTable.WriteXml(lOutputFile2)
        'For Each TableRow As DataRow In loutputTable.Rows 'Writing the table contents
        '    TextToWrite = ""
        '    For Each TableColumn As DataColumn In loutputTable.Columns
        '        TextToWrite &= TableRow(TableColumn) & ","
        '    Next TableColumn
        '    lOutputFile.WriteLine(TextToWrite)
        'Next TableRow
        'lOutputFile.Close()
        lOutputFile2.Close()

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
                                               ByRef aParm As ModelParameter,
                                               ByVal aMFactorOrParmValue As Double
                                               )
        Dim lOperLowerRange As Integer = -1
        Dim lOperUpperRange As Integer = -1
        Try
            lOperLowerRange = CInt(aParm.ParmOperationName.Split("-")(0))
            lOperUpperRange = CInt(aParm.ParmOperationName.Split("-")(1))
            Logger.Dbg("Range of operaion is provided")
        Catch ex As Exception
            Logger.Dbg("Range of Operations is not provided")
        End Try

        Select Case True
            Case aParm.ParmTable.Contains("MASS-LINK")
                Dim lMassLinkID As Integer
                lMassLinkID = aParm.ParmOccurence
                Dim lMassLinkParts(7) As String
                If aParm.ParmName.Length > 0 Then
                    lMassLinkParts = aParm.ParmName.Split(":")
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

                        If aParm.ParmMultFactor = 1 Then
                            lMasslink.MFact = HspfTable.NumFmtRE(lMasslink.MFact * aMFactorOrParmValue, 5)
                        Else
                            lMasslink.MFact = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                        End If

                    End If
                Next

            Case aParm.ParmTable.Contains("EXTNL")

                If aParm.ParmOperationType.ToLower = "point" Then
                    Dim lConnections(3) As String
                    If aParm.ParmName.Length > 0 Then
                        lConnections = aParm.ParmName.Split(":")

                        For Each lID As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
                            For Each lpointsource As HspfPointSource In lID.PointSources
                                If lpointsource.Target.Group = "INFLOW" AndAlso lpointsource.Target.Member = lConnections(0) AndAlso
                                            lpointsource.Target.MemSub1 = lConnections(1) AndAlso lpointsource.Target.MemSub2 = lConnections(2) AndAlso
                                            (lID.Id = aParm.ParmOperationNumber OrElse lID.Description = aParm.ParmOperationName OrElse
                                            (aParm.ParmOperationNumber = 0 AndAlso aParm.ParmOperationName = "")) Then
                                    If aParm.ParmMultFactor = 1 Then
                                        lpointsource.MFact = HspfTable.NumFmtRE(lpointsource.MFact * aMFactorOrParmValue, 5)
                                    Else
                                        lpointsource.MFact = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                                    End If
                                End If
                            Next
                        Next
                    End If
                ElseIf aParm.ParmOperationType.ToLower = "point_ts" Then
                    Dim lConnections(3) As String
                    If aParm.ParmName.Length > 0 Then
                        lConnections = aParm.ParmName.Split(":")

                        For Each lID As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
                            For Each lpointsource As HspfPointSource In lID.PointSources
                                If lpointsource.Target.Group = "INFLOW" AndAlso lpointsource.Target.Member = lConnections(0) AndAlso
                                            lpointsource.Target.MemSub1 = lConnections(1) AndAlso lpointsource.Target.MemSub2 = lConnections(2) AndAlso
                                            (lID.Id = aParm.ParmOperationNumber OrElse lID.Description = aParm.ParmOperationName OrElse
                                            (aParm.ParmOperationNumber = 0 AndAlso aParm.ParmOperationName = "")) Then
                                    lpointsource.Source.VolId = aMFactorOrParmValue
                                End If
                            Next
                        Next
                    End If
                End If

                Dim MetSegRec As Integer = -1
                For Each lMetSeg As HspfMetSeg In aUCI.MetSegs

                    Select Case aParm.ParmName.ToLower
                        Case "prec"
                            MetSegRec = 0
                        Case "atem"
                            MetSegRec = 2
                        Case "solr"
                            MetSegRec = 5
                        Case Else
                            MetSegRec = -1
                    End Select
                    If MetSegRec = -1 Then Exit For
                    Try
                        If aParm.ParmMultFactor = 1 Then
                            lMetSeg.MetSegRecs(MetSegRec).MFactP = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(MetSegRec).MFactP * aMFactorOrParmValue, 5)

                            lMetSeg.MetSegRecs(MetSegRec).MFactR = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(MetSegRec).MFactR * aMFactorOrParmValue, 5)
                        Else
                            lMetSeg.MetSegRecs(MetSegRec).MFactP = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)

                            lMetSeg.MetSegRecs(MetSegRec).MFactR = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                        End If

                    Catch ex As Exception
                        Logger.Msg("Could not find the Table " & aParm.ParmTable &
                                   " Parameter " & aParm.ParmName & " for " & lMetSeg.Name, MsgBoxStyle.Critical)
                        End
                    End Try
                Next

            Case Else

                For Each lOper As HspfOperation In aUCI.OpnBlks(aParm.ParmOperationType).Ids
                    If (lOper.Id = aParm.ParmOperationNumber OrElse lOper.Description = aParm.ParmOperationName OrElse
                            (lOper.Id >= lOperLowerRange AndAlso lOper.Id <= lOperUpperRange) OrElse
                            (aParm.ParmOperationNumber = 0 AndAlso aParm.ParmOperationName = "")) Then
                        Try
                            Dim lTable As HspfTable

                            If aParm.ParmOccurence = 0 Or aParm.ParmOccurence = 1 Then
                                lTable = lOper.Tables(aParm.ParmTable)
                            Else
                                lTable = lOper.Tables(aParm.ParmTable & ":" & aParm.ParmOccurence)
                            End If


                            If lTable.Name.Contains("MON-") Then
                                For Mon = 0 To 11 'For monthly parameters, only a multiplication factor can be provided

                                    lTable.Parms(Mon).Value = HspfTable.NumFmtRE(lTable.Parms(Mon).Value * aMFactorOrParmValue, 5)
                                    If (lTable.Parms(Mon).Value < aParm.ParmLow) Then
                                        lTable.Parms(Mon).Value = aParm.ParmLow
                                    ElseIf (lTable.Parms(Mon).Value > aParm.ParmHigh) Then
                                        lTable.Parms(Mon).Value = aParm.ParmHigh
                                    End If
                                Next
                            Else
                                If aParm.ParmMultFactor = 1 Then
                                    lTable.ParmValue(aParm.ParmName) = HspfTable.NumFmtRE(lTable.ParmValue(aParm.ParmName) * aMFactorOrParmValue, lTable.Parms(aParm.ParmName).Def.Length)
                                Else
                                    lTable.ParmValue(aParm.ParmName) = HspfTable.NumFmtRE(aMFactorOrParmValue, lTable.Parms(aParm.ParmName).Def.Length)
                                End If
                                If (lTable.ParmValue(aParm.ParmName) < aParm.ParmLow) Then
                                    lTable.ParmValue(aParm.ParmName) = aParm.ParmLow
                                ElseIf (lTable.ParmValue(aParm.ParmName) > aParm.ParmHigh) Then
                                    lTable.ParmValue(aParm.ParmName) = aParm.ParmHigh
                                End If
                            End If
                        Catch ex As Exception
                            Logger.Msg("Could not find the Table " & aParm.ParmTable &
                                       " Parameter " & aParm.ParmName & " for " & lOper.Id, MsgBoxStyle.Critical)
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
                                 ByVal aOutputDSN As atcCollection,
                                 ByRef aHSPFEchofileName As String,
                                 ByRef aOutputTable As DataTable)

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
            OpenFile(aHSPFEchofileName)
            End
        End If

        Dim lOutputline As String = ""

        If HSPFRan Then
            Dim lWdmFileName As String = pBaseName & ".wdm"
            Dim lOutWDMFileName As String = "MultiSim_" & pBaseName & ".wdm"

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

                    Dim AnnSimulatedTS As atcTimeseries = Aggregate(SimulatedTS, atcTimeUnit.TUYear, 1, atcTran.TranMax)

                    Dim row As DataRow
                    row = aOutputTable.NewRow
                    For Each TableColumn As DataColumn In aOutputTable.Columns
                        Dim ColumnName As String = TableColumn.ColumnName
                        Select Case ColumnName
                            Case "SimID"
                                row(ColumnName) = SimID
                            Case "ID"
                                row(ColumnName) = WDMDataset
                            Case "AvAnnPeak"
                                row(ColumnName) = AnnSimulatedTS.Attributes.GetDefinedValue("Mean").Value
                            Case "10%High"
                                row(ColumnName) = SimulatedTS.Attributes.GetDefinedValue("Sum").Value - SimulatedTS.Attributes.GetDefinedValue("%Sum90").Value
                            Case "25%High"
                                row(ColumnName) = SimulatedTS.Attributes.GetDefinedValue("Sum").Value - SimulatedTS.Attributes.GetDefinedValue("%Sum75").Value
                            Case "50%High"
                                row(ColumnName) = SimulatedTS.Attributes.GetDefinedValue("Sum").Value - SimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                            Case "max(30-day GeoMean)"
                                Dim lDailyTs As atcTimeseries = Aggregate(SimulatedTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                                If lDailyTs.Values.Count >= 30 Then
                                    Dim GeoMean As New List(Of Double)
                                    Dim lCount As Integer = 0
                                    Dim Test As Double = 0.0
                                    For i As Integer = 1 To lDailyTs.Values.Count - 1
                                        If lDailyTs.Value(i) > 0 Then
                                            Test += Math.Log10(lDailyTs.Value(i))
                                        End If
                                        If i >= 30 Then
                                            If lDailyTs.Value(i - 30) > 0 Then Test -= Math.Log10(lDailyTs.Value(i - 30))

                                            GeoMean.Add(10 ^ (Test / 30))
                                        End If

                                    Next i
                                    Test = 0.0
                                    For Each lValue As Double In GeoMean
                                        If lValue > Test Then Test = lValue
                                    Next
                                    row(ColumnName) = Test
                                End If

                            Case Else
                                row(ColumnName) = SimulatedTS.Attributes.GetDefinedValue(ColumnName).Value
                        End Select

                    Next TableColumn
                    aOutputTable.Rows.Add(row)

                Catch
                    Logger.Dbg("Could not compute statistics for SimID:" & SimID & " And DatasetID:" & WDMDataset)
                End Try

            Next WDMDataset
            lWdmDataSource.Clear()
            lWdmDataSource2.Clear()
        Else
            Logger.Dbg("The simulation " & SimID & " didn't complete successfully!")
        End If
        Logger.Status("Number of simulations complete: " & SimID)
        Return aOutputTable
    End Function
    Private Function Observed() 'Possible function to save observed values in future.
        Return "Observed"
    End Function

    Private Function AddOutputTableColumns(ByRef aDataTable As Data.DataTable) As DataTable


        Dim column As DataColumn
        column = New DataColumn()
        column.DataType = Type.GetType("System.Int32")
        column.ColumnName = "SimID"
        column.Caption = "Simulation ID"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Int32")
        column.ColumnName = "ID"
        column.Caption = "DataSet ID"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Sum"
        column.Caption = "Sum"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "mean"
        column.Caption = "Mean"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "SumAnnual"
        column.Caption = "Annual Sum"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "geometric mean"
        column.Caption = "Geometric Mean"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "max(30-day GeoMean)"
        column.Caption = "Max(30-day Geometric Mean)"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "AvAnnPeak"
        column.Caption = "Average Annual Peak"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "10%High"
        column.Caption = "Sum of 10% High"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "25%High"
        column.Caption = "Sum of 25% High"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "50%High"
        column.Caption = "Sum of 50% High"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%Sum50"
        column.Caption = "Sum of 50% Low"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%Sum25"
        column.Caption = "Sum of 25% Low"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%Sum10"
        column.Caption = "Sum of 10% Low"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%Sum05"
        column.Caption = "Sum of 5% Low"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%Sum02"
        column.Caption = "Sum of 2% Low"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%0.10"
        column.Caption = "0.1 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%1.00"
        column.Caption = "1 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%2.00"
        column.Caption = "2 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%2.50"
        column.Caption = "2.5 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%5.00"
        column.Caption = "5 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%10.00"
        column.Caption = "10 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%20.00"
        column.Caption = "20 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%30.00"
        column.Caption = "30 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%40.00"
        column.Caption = "40 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%50.00"
        column.Caption = "50 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%60.00"
        column.Caption = "60 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%70.00"
        column.Caption = "70 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%80.00"
        column.Caption = "80 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%90.00"
        column.Caption = "90 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%95.00"
        column.Caption = "95 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%97.5"
        column.Caption = "97.5 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%98.00"
        column.Caption = "98 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%99.00"
        column.Caption = "99 Percentile"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "%99.90"
        column.Caption = "99.9 Percentile"
        aDataTable.Columns.Add(column)

        Return aDataTable
    End Function
End Module
