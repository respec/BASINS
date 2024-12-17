Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports System.IO
Imports System.Data

Public Class ModelParameter
    'This class manages the property of each individual parameter
    Public pParmID As Integer = 0
    Public pParmOperationType As String = ""
    Public pParmOperationNumber As Integer = 0
    Public pParmOperationName As String = ""
    Public pParmTable As String = ""
    Public pParmName As String = ""
    Public pParmMultFactor As Integer = 0
    Public pParmLow As Double = -1.0E-30
    Public pParmHigh As Double = 1.0E+30
    Public pParmOccurence As Integer = 0
End Class


Public Module MultiSimAnalysis
    Sub SubMultiSim(ByVal aHSPFEXE As String, ByVal pBaseName As String, ByVal pTestPath As String,
                            aSDateJ As Double, aEDateJ As Double, ByVal aHSPFEchofilename As String)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm") 'Need to figure out why we need this.
        Dim lExitCode As Integer
        Dim lSimID As Integer = 0
        Dim lUci As New atcUCI.HspfUci
        Dim lDeleteUCIFiles As Integer = 0
        Dim lUciName As String
        lUciName = "MultiSim.uci"
        File.Copy(pBaseName & ".uci", lUciName, True) 'Saving the original file with MultiSim.uci name
        lUci.ReadUci(lMsg, lUciName, -1, False, pBaseName & ".ech") ' Reading the temp uci file
        lUci.GlobalBlock.RunFg = 1
        lUci.Save()

        Dim lOutWDMFileName As String = Path.Combine(pTestPath, "MultiSim.wdm")
        If File.Exists(lOutWDMFileName) Then
            Try
                File.Delete(lOutWDMFileName)
            Catch ex As Exception
            End Try
        End If

        Dim lYearsofSimulation As Single
        lYearsofSimulation = (aEDateJ - aSDateJ) / JulianYear

        'Generate a default Parameter Sensitivity Table file if it does not exist already
        Dim lParameterListFilesIsAvailable As Boolean = False
        Dim lSpecificationFile As String = "MultiSimSpecFile.csv"

        Dim lNumberOfOutputDSN As Integer = 0

        If Not IO.File.Exists(lSpecificationFile) Then
            Dim lSensitivityParameterFile As StreamWriter = IO.File.CreateText("MultiSimSpecFile.csv")
            Dim lTextToAddForMultiSimFile As String = "***Initial Parameter List For Sensitivity/Uncertainty Analysis And Output DSN from the UCI file"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "***Following Output IDs are the first two ID read from the EXT TARGET block of the UCI file. You can add more."
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)

            lTextToAddForMultiSimFile = "DSN,"
            For Each lEXTTARGET As HspfConnection In lUci.Connections

                If lEXTTARGET.Target.VolName.Contains("WDM") AndAlso lNumberOfOutputDSN <= 1 Then
                    lNumberOfOutputDSN += 1
                    lTextToAddForMultiSimFile &= lEXTTARGET.Target.VolId & ","

                End If
            Next

            If lNumberOfOutputDSN = 0 Then
                Logger.Msg("The UCI file does not have output datasets specified in the EXT TARGETS Block. HSPEXP+ will quit", vbOKOnly)
                End
            End If

            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = """***The operation number, land use, tied with next, and multiplier can be left blank"""
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "ParmID,OPN_Type,Table_Name,Parm_Name,Occur_Or_MLNumber,Mult_Factor_FG,OPN_Number_Or_Name,Lower_Limit,Upper_Limit"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "1,PERLND,PWAT-PARM2,LZSN,,1,,3,8"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "2,PERLND,PWAT-PARM2,INFILT,,1,,0.01,0.5"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "Delete intermediate UCI files?, 0"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "***Following lines list the multiplication factor for each parameter for each simulation."
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "SimID,Parm1,Parm2,,,,,"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "1,0.9,,"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "2,1.1,,"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "3,,0.9,"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)
            lTextToAddForMultiSimFile = "4,,1.1,"
            lSensitivityParameterFile.WriteLine(lTextToAddForMultiSimFile)


            MsgBox("A default list of Sensitive parameters was created as MultiSimSpecFile.csv. Additionally two output DSN from the EXT TARGETS blocks were selected for analyzing outputs.
You can edit this specification file and add more parameters and outputs.", vbOKOnly, "Default List of Outputs & Parameters")
            lSensitivityParameterFile.Close()
        End If

        'Dim lOutputFile As StreamWriter = File.CreateText("MultiSimOutput.csv")
        Dim lOutputTable As DataTable
        lOutputTable = New DataTable("MultiSimOutput")
        lOutputTable = AddOutputTableColumns(lOutputTable)

        Dim lTextToWrite As String = ""
        For Each lTableColumn As DataColumn In lOutputTable.Columns 'Writing the table headings
            lTextToWrite &= lTableColumn.Caption & ","
        Next
        'lOutputFile.WriteLine(TextToWrite)

        'The file listing the sensitive parameters and the output DSN already exists.
        Dim lSpecificationFileRecordsNew As New ArrayList
        lSpecificationFileRecordsNew = ReadCSVFile(lSpecificationFile) 'Reading the MultiSim specification file 

        Dim lListOfOutputDSN As New atcCollection
        Dim lCsvRecordIndex As Integer = 0
        Dim lCsvLineRecord() As String = lSpecificationFileRecordsNew(lCsvRecordIndex)
        lNumberOfOutputDSN = lCsvLineRecord.Length - 1
        For i = 1 To lNumberOfOutputDSN
            If lcsvlinerecord(i) = "" Then
                lNumberOfOutputDSN = i - 1
                Exit For
            Else
                lListOfOutputDSN.Add(Int(lCsvLineRecord(i)))
                'If DSN is not an integer, it will cause an error here. Need to figure out the best way to handle it.
            End If
        Next

        Dim lOutputLine As String = ""
        ChDriveDir(PathNameOnly(pHSPFExe))
        lOutputTable = ModelRunandReportAnswers(lSimID, lUci, lUciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, lYearsofSimulation, lListOfOutputDSN, aHSPFEchofilename, lOutputTable)

        Dim lListOfParameters As New List(Of ModelParameter)
        lCsvRecordIndex += 1
        lCsvLineRecord = lSpecificationFileRecordsNew(lCsvRecordIndex)
        Do Until lCsvLineRecord(0) = "Simulation ID" Or lCsvLineRecord(0).ToLower.Contains("delete")
            'Going through the CSV file and getting the list of parameters.
            Dim lNewParameter As New ModelParameter
            With lNewParameter
                Try
                    .pParmID = Trim(Int(lCsvLineRecord(0))) 'Need to figure out better error messages if the values are not correct.
                Catch ex As Exception

                End Try
                If Not Trim(lCsvLineRecord(1)).Length = 0 Then
                    .pParmOperationType = Trim(lCsvLineRecord(1))
                End If

                .pParmTable = Trim(lCsvLineRecord(2))

                If lCsvLineRecord.Length > 3 AndAlso Not Trim(lCsvLineRecord(3)).Length = 0 Then
                    .pParmName = Trim(lCsvLineRecord(3))
                End If

                Try
                    .pParmOccurence = CInt(Trim(lCsvLineRecord(4)))
                Catch
                    .pParmOccurence = 0
                End Try

                .pParmMultFactor = 0

                Try
                    .pParmMultFactor = CInt(Trim(lCsvLineRecord(5)))
                Catch
                    .pParmMultFactor = 0
                End Try

                'Assume that the fixed value for parameters will be provided for a MultiSim analysis for each individual simulation.

                If .pParmTable.ToLower.StartsWith("mon-") Then .pParmMultFactor = 1 'If a monthly table is provided, a multiplication factor has to be provided.

                If Not Trim(lCsvLineRecord(6)).Length = 0 Then
                    Try
                        .pParmOperationNumber = CInt(Trim(lCsvLineRecord(6)))
                        .pParmOperationName = ""
                    Catch
                        .pParmOperationName = Trim(lCsvLineRecord(6))
                        .pParmOperationNumber = 0
                    End Try
                End If

                Try
                    .pParmLow = CDbl(Trim(lCsvLineRecord(7)))
                Catch
                End Try

                Try
                    .pParmHigh = CDbl(Trim(lCsvLineRecord(8)))
                Catch
                End Try

            End With

            lListOfParameters.Add(lNewParameter)
            lCsvRecordIndex += 1
            lCsvLineRecord = lSpecificationFileRecordsNew(lCsvRecordIndex)
        Loop
        If File.Exists(Path.Combine(pTestPath, "MultiSim_" & pBaseName & ".wdm")) Then File.Delete(Path.Combine(pTestPath, "MultiSim_" & pBaseName & ".wdm"))

        Do
            lCsvLineRecord = lSpecificationFileRecordsNew(lCsvRecordIndex)
            If lCsvLineRecord(0).ToLower.Contains("delete") Then
                lDeleteUCIFiles = CInt(Trim(lCsvLineRecord(1)))
                lCsvRecordIndex += 1
            End If
            lCsvRecordIndex += 1
            lCsvLineRecord = lSpecificationFileRecordsNew(lCsvRecordIndex)
            lSimID = lCsvLineRecord(0)

            IO.File.Copy(Path.Combine(pTestPath, lUciName), Path.Combine(pTestPath, lSimID & lUciName), True)
            lUci = New HspfUci
            lUci.ReadUci(lMsg, Path.Combine(pTestPath, lSimID & lUciName), -1, False, Path.Combine(pTestPath, pBaseName & ".ech"))

            Dim lMFactorOrParmValue As Double = 0.0

            For Each lParm As ModelParameter In lListOfParameters
                Try
                    lMFactorOrParmValue = CDbl(lCsvLineRecord((lParm.pParmID)))
                Catch
                    lMFactorOrParmValue = Double.NaN
                End Try
                If Not (Double.IsNaN(lMFactorOrParmValue) Or (lParm.pParmMultFactor = 0 AndAlso lMFactorOrParmValue = 1.0)) Then
                    ChangeUCIParameterAndSave(lUci, lParm, lMFactorOrParmValue)
                End If
            Next

            lOutputTable = ModelRunandReportAnswers(lSimID, lUci, lSimID & lUciName, lExitCode, pBaseName, pTestPath,
                                 aSDateJ, aEDateJ, lYearsofSimulation, lListOfOutputDSN, aHSPFEchofilename, lOutputTable)

            If lDeleteUCIFiles = 1 Then
                File.Delete(lSimID & lUciName)
            End If

        Loop While lCsvRecordIndex < lSpecificationFileRecordsNew.Count - 1
        Dim lOutputFile2 As StreamWriter = File.CreateText(Path.Combine(pTestPath, "MultiSimOutput.xml"))
        lOutputTable.WriteXml(lOutputFile2)
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
        Using lMyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(aSensitivitySpecificationFile)
            Dim lLines() As String = {}
            If File.Exists(aSensitivitySpecificationFile) Then
                lMyReader.TextFieldType = FileIO.FieldType.Delimited
                lMyReader.SetDelimiters(",")
                Dim lCurrentRow As String()

                While Not lMyReader.EndOfData
                    Try
                        If (lMyReader.PeekChars(10000).Contains("***") Or
                                    Trim(lMyReader.PeekChars(10000)) = "" Or
                                    Trim(lMyReader.PeekChars(10000).ToLower.StartsWith("parameter")) Or
                                    Trim(lMyReader.PeekChars(10000).ToLower.StartsWith("parmid")) Or
                                    Trim(lMyReader.PeekChars(10000).ToLower.StartsWith(","))) Then
                            lCurrentRow = lMyReader.ReadFields
                        Else
                            lCurrentRow = lMyReader.ReadFields
                            Dim i As Integer = 0
                            For Each testtring As String In lCurrentRow
                                lCurrentRow(i) = testtring
                                i += 1
                            Next
                            lSensitivityRecordsNew.Add(lCurrentRow)
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
            lOperLowerRange = CInt(aParm.pParmOperationName.Split("-")(0))
            lOperUpperRange = CInt(aParm.pParmOperationName.Split("-")(1))
            Logger.Dbg("Range of operaion is provided")
        Catch ex As Exception
            Logger.Dbg("Range of Operations is not provided")
        End Try

        Select Case True
            Case aParm.pParmTable.Contains("MASS-LINK")
                Dim lMassLinkID As Integer
                lMassLinkID = aParm.pParmOccurence
                Dim lMassLinkParts(7) As String
                If aParm.pParmName.Length > 0 Then
                    lMassLinkParts = aParm.pParmName.Split(":")
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

                        If aParm.pParmMultFactor = 1 Then
                            lMasslink.MFact = HspfTable.NumFmtRE(lMasslink.MFact * aMFactorOrParmValue, 5)
                        Else
                            lMasslink.MFact = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                        End If

                    End If
                Next

            Case aParm.pParmTable.Contains("EXTNL")

                If aParm.pParmOperationType.ToLower = "point" Then
                    Dim lConnections(3) As String
                    If aParm.pParmName.Length > 0 Then
                        lConnections = aParm.pParmName.Split(":")

                        For Each lID As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
                            For Each lPointSource As HspfPointSource In lID.PointSources
                                If lPointSource.Target.Group = "INFLOW" AndAlso lPointSource.Target.Member = lConnections(0) AndAlso
                                            lPointSource.Target.MemSub1 = lConnections(1) AndAlso lPointSource.Target.MemSub2 = lConnections(2) AndAlso
                                            (lID.Id = aParm.pParmOperationNumber OrElse lID.Description = aParm.pParmOperationName OrElse
                                            (lID.Id >= lOperLowerRange AndAlso lID.Id <= lOperUpperRange) OrElse
                                            (aParm.pParmOperationNumber = 0 AndAlso aParm.pParmOperationName = "")) Then
                                    If aParm.pParmMultFactor = 1 Then
                                        lPointSource.MFact = HspfTable.NumFmtRE(lPointSource.MFact * aMFactorOrParmValue, 5)
                                    Else
                                        lPointSource.MFact = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                                    End If
                                End If
                            Next
                        Next
                    End If
                ElseIf aParm.pParmOperationType.ToLower = "point_ts" Then
                    Dim lConnections(3) As String
                    If aParm.pParmName.Length > 0 Then
                        lConnections = aParm.pParmName.Split(":")

                        For Each lID As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
                            For Each lPointSource As HspfPointSource In lID.PointSources
                                If lPointSource.Target.Group = "INFLOW" AndAlso lPointSource.Target.Member = lConnections(0) AndAlso
                                            lPointSource.Target.MemSub1 = lConnections(1) AndAlso lPointSource.Target.MemSub2 = lConnections(2) AndAlso
                                            (lID.Id = aParm.pParmOperationNumber OrElse lID.Description = aParm.pParmOperationName OrElse
                                            (aParm.pParmOperationNumber = 0 AndAlso aParm.pParmOperationName = "")) Then
                                    lPointSource.Source.VolId = aMFactorOrParmValue
                                End If
                            Next
                        Next
                    End If
                End If

                Dim lMetSegRec As Integer = -1
                For Each lMetSeg As HspfMetSeg In aUCI.MetSegs

                    Select Case aParm.pParmName.ToLower
                        Case "prec"
                            lMetSegRec = 0
                        Case "atem"
                            lMetSegRec = 2
                        Case "solr"
                            lMetSegRec = 5
                        Case Else
                            lMetSegRec = -1
                    End Select
                    If lMetSegRec = -1 Then Exit For
                    Try
                        If aParm.pParmMultFactor = 1 Then
                            lMetSeg.MetSegRecs(lMetSegRec).MFactP = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(lMetSegRec).MFactP * aMFactorOrParmValue, 5)

                            lMetSeg.MetSegRecs(lMetSegRec).MFactR = HspfTable.NumFmtRE(lMetSeg.MetSegRecs(lMetSegRec).MFactR * aMFactorOrParmValue, 5)
                        Else
                            lMetSeg.MetSegRecs(lMetSegRec).MFactP = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)

                            lMetSeg.MetSegRecs(lMetSegRec).MFactR = HspfTable.NumFmtRE(aMFactorOrParmValue, 5)
                        End If

                    Catch ex As Exception
                        Logger.Msg("Could not find the Table " & aParm.pParmTable &
                                   " Parameter " & aParm.pParmName & " for " & lMetSeg.Name, MsgBoxStyle.Critical)
                        End
                    End Try
                Next

            Case Else

                For Each lOper As HspfOperation In aUCI.OpnBlks(aParm.pParmOperationType).Ids
                    If (lOper.Id = aParm.pParmOperationNumber OrElse lOper.Description = aParm.pParmOperationName OrElse
                            (lOper.Id >= lOperLowerRange AndAlso lOper.Id <= lOperUpperRange) OrElse
                            (aParm.pParmOperationNumber = 0 AndAlso aParm.pParmOperationName = "")) Then
                        Try
                            Dim lTable As HspfTable

                            If aParm.pParmOccurence = 0 Or aParm.pParmOccurence = 1 Then
                                lTable = lOper.Tables(aParm.pParmTable)
                            Else
                                lTable = lOper.Tables(aParm.pParmTable & ":" & aParm.pParmOccurence)
                            End If


                            If lTable.Name.Contains("MON-") Then
                                For Mon = 0 To 11 'For monthly parameters, only a multiplication factor can be provided

                                    lTable.Parms(Mon).Value = HspfTable.NumFmtRE(lTable.Parms(Mon).Value * aMFactorOrParmValue, 5)
                                    If (lTable.Parms(Mon).Value < aParm.pParmLow) Then
                                        lTable.Parms(Mon).Value = aParm.pParmLow
                                    ElseIf (lTable.Parms(Mon).Value > aParm.pParmHigh) Then
                                        lTable.Parms(Mon).Value = aParm.pParmHigh
                                    End If
                                Next
                            Else
                                If aParm.pParmMultFactor = 1 Then
                                    lTable.ParmValue(aParm.pParmName) = HspfTable.NumFmtRE(lTable.ParmValue(aParm.pParmName) * aMFactorOrParmValue, lTable.Parms(aParm.pParmName).Def.Length)
                                Else
                                    lTable.ParmValue(aParm.pParmName) = HspfTable.NumFmtRE(aMFactorOrParmValue, lTable.Parms(aParm.pParmName).Def.Length)
                                End If
                                If (lTable.ParmValue(aParm.pParmName) < aParm.pParmLow) Then
                                    lTable.ParmValue(aParm.pParmName) = aParm.pParmLow
                                ElseIf (lTable.ParmValue(aParm.pParmName) > aParm.pParmHigh) Then
                                    lTable.ParmValue(aParm.pParmName) = aParm.pParmHigh
                                End If
                            End If
                        Catch ex As Exception
                            Logger.Msg("Could not find the Table " & aParm.pParmTable &
                                       " Parameter " & aParm.pParmName & " for " & lOper.Id, MsgBoxStyle.Critical)
                            End
                        End Try

                    End If
                Next
        End Select
        aUCI.GlobalBlock.RunFg = 1

        aUCI.Save()
    End Sub

    Private Function ModelRunandReportAnswers(ByVal aSimID As Integer,
                                 ByVal aUci As atcUCI.HspfUci,
                                 ByVal aUciName As String,
                                 ByVal aExitCode As Integer,
                                 ByVal aBaseName As String,
                                 ByVal aTestPath As String,
                                 ByVal aSDateJ As Double,
                                 ByVal aEDateJ As Double,
                                 ByVal aYearsofSimulation As Single,
                                 ByVal aOutputDSN As atcCollection,
                                 ByRef aHSPFEchofileName As String,
                                 ByRef aOutputTable As DataTable)

        aExitCode = LaunchProgram(pHSPFExe, aTestPath, "-1 -1 " & aUciName) 'Run HSPF program

        Dim lHSPFRan As Boolean = False
        Dim lRunMade As String
        Dim lEchoFileInfo As System.IO.FileInfo
        If IO.File.Exists(aHSPFEchofileName) Then
            lEchoFileInfo = New System.IO.FileInfo(aHSPFEchofileName)
            lRunMade = lEchoFileInfo.LastWriteTime.ToString
            Using lEchoFileReader As StreamReader = File.OpenText(aHSPFEchofileName)
                While Not lEchoFileReader.EndOfStream
                    Dim lNextLine As String = lEchoFileReader.ReadLine()
                    If Not lNextLine.ToUpper.Contains("END OF JOB") Then
                        lHSPFRan = False
                    Else
                        lHSPFRan = True
                    End If
                End While
            End Using
        Else
            Logger.Msg("The ECHO file is not available for this model. Please check if model ran successfully last time", vbCritical)
            OpenFile(aHSPFEchofileName)
            End
        End If

        Dim lOutputline As String = ""

        If lHSPFRan Then
            Dim lWdmFileName As String = aBaseName & ".wdm"
            Dim lOutWDMFileName As String = "MultiSim_" & aBaseName & ".wdm"

            Dim lWdmDataSource As atcWDM.atcDataSourceWDM
            Dim lWdmDataSource2 As atcWDM.atcDataSourceWDM

            If atcDataManager.DataSources.Contains(aTestPath & lWdmFileName) Then
                atcDataManager.RemoveDataSource(aTestPath & lWdmFileName)
            End If
            If atcDataManager.DataSources.Contains(aTestPath & lOutWDMFileName) Then
                atcDataManager.RemoveDataSource(aTestPath & lOutWDMFileName)
            End If

            atcDataManager.OpenDataSource(aTestPath & lWdmFileName)
            lWdmDataSource = atcDataManager.DataSourceBySpecification(aTestPath & lWdmFileName)
            atcDataManager.OpenDataSource(aTestPath & lOutWDMFileName)
            lWdmDataSource2 = atcDataManager.DataSourceBySpecification(aTestPath & lOutWDMFileName)


            If lWdmDataSource2 Is Nothing Then
                lWdmDataSource2 = New atcWDM.atcDataSourceWDM
                lWdmDataSource2.Open(aTestPath & lOutWDMFileName)
            End If

            Dim lYearlyAttributes As New atcDataAttributes

            Dim lDatasetID As Integer = lWdmDataSource2.DataSets.Count
            For Each lWDMDataset As Integer In aOutputDSN
                Try
                    Dim lSimulatedTS As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lWDMDataset), aSDateJ, aEDateJ, Nothing)

                    lDatasetID += 1
                    If lDatasetID < 10000 Then
                        lSimulatedTS.Attributes.SetValue("ID", lDatasetID)
                        lSimulatedTS.Attributes.SetValue("Scenario", "SIMID" & aSimID)

                        lWdmDataSource2.AddDataset(lSimulatedTS)
                    End If

                    Dim lAnnSimulatedTS As atcTimeseries = Aggregate(lSimulatedTS, atcTimeUnit.TUYear, 1, atcTran.TranMax)

                    Dim lRow As DataRow
                    lRow = aOutputTable.NewRow
                    For Each lTableColumn As DataColumn In aOutputTable.Columns
                        Dim lColumnName As String = lTableColumn.ColumnName
                        Select Case lColumnName
                            Case "SimID"
                                lRow(lColumnName) = aSimID
                            Case "ID"
                                lRow(lColumnName) = lWDMDataset
                            Case "AvAnnPeak"
                                lRow(lColumnName) = lAnnSimulatedTS.Attributes.GetDefinedValue("Mean").Value
                            Case "10%High"
                                lRow(lColumnName) = lSimulatedTS.Attributes.GetDefinedValue("Sum").Value - lSimulatedTS.Attributes.GetDefinedValue("%Sum90").Value
                            Case "25%High"
                                lRow(lColumnName) = lSimulatedTS.Attributes.GetDefinedValue("Sum").Value - lSimulatedTS.Attributes.GetDefinedValue("%Sum75").Value
                            Case "50%High"
                                lRow(lColumnName) = lSimulatedTS.Attributes.GetDefinedValue("Sum").Value - lSimulatedTS.Attributes.GetDefinedValue("%Sum50").Value
                            Case "max(30-day GeoMean)"
                                Dim lDailyTs As atcTimeseries = Aggregate(lSimulatedTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                                If lDailyTs.Values.Count >= 30 Then
                                    Dim lGeoMean As New List(Of Double)
                                    Dim lCount As Integer = 0
                                    Dim lTest As Double = 0.0
                                    For i As Integer = 1 To lDailyTs.Values.Count - 1
                                        If lDailyTs.Value(i) > 0 Then
                                            lTest += Math.Log10(lDailyTs.Value(i))
                                        End If
                                        If i >= 30 Then
                                            If lDailyTs.Value(i - 30) > 0 Then lTest -= Math.Log10(lDailyTs.Value(i - 30))

                                            lGeoMean.Add(10 ^ (lTest / 30))
                                        End If

                                    Next i
                                    lTest = 0.0
                                    For Each lValue As Double In lGeoMean
                                        If lValue > lTest Then lTest = lValue
                                    Next
                                    lRow(lColumnName) = lTest
                                End If

                            Case Else
                                Try
                                    lRow(lColumnName) = lSimulatedTS.Attributes.GetDefinedValue(lColumnName).Value
                                Catch ex As Exception
                                    Logger.Dbg("The " & lColumnName & " could not be calculated for DSN " & lWDMDataset)
                                End Try

                        End Select

                    Next lTableColumn
                    aOutputTable.Rows.Add(lRow)

                Catch
                    Logger.Dbg("Could not compute statistics for SimID:" & aSimID & " And DatasetID:" & lWDMDataset)
                End Try

            Next lWDMDataset
            lWdmDataSource.Clear()
            lWdmDataSource2.Clear()
        Else
            Logger.Dbg("The simulation " & aSimID & " didn't complete successfully!")
        End If
        Logger.Status("Number of simulations complete: " & aSimID)
        Return aOutputTable
    End Function

    Private Function Observed() 'Possible function to save observed values in future.
        Return "Observed"
    End Function

    Private Function AddOutputTableColumns(ByRef aDataTable As Data.DataTable) As DataTable

        Dim lColumn As DataColumn
        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Int32")
        lColumn.ColumnName = "SimID"
        lColumn.Caption = "Simulation ID"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Int32")
        lColumn.ColumnName = "ID"
        lColumn.Caption = "DataSet ID"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Sum"
        lColumn.Caption = "Sum"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "mean"
        lColumn.Caption = "Mean"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "SumAnnual"
        lColumn.Caption = "Annual Sum"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "geometric mean"
        lColumn.Caption = "Geometric Mean"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "max(30-day GeoMean)"
        lColumn.Caption = "Max(30-day Geometric Mean)"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "AvAnnPeak"
        lColumn.Caption = "Average Annual Peak"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "10%High"
        lColumn.Caption = "Sum of 10% High"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "25%High"
        lColumn.Caption = "Sum of 25% High"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "50%High"
        lColumn.Caption = "Sum of 50% High"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%Sum50"
        lColumn.Caption = "Sum of 50% Low"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%Sum25"
        lColumn.Caption = "Sum of 25% Low"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%Sum10"
        lColumn.Caption = "Sum of 10% Low"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%Sum05"
        lColumn.Caption = "Sum of 5% Low"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%Sum02"
        lColumn.Caption = "Sum of 2% Low"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%0.10"
        lColumn.Caption = "0.1 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%1.00"
        lColumn.Caption = "1 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%2.00"
        lColumn.Caption = "2 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%2.50"
        lColumn.Caption = "2.5 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%5.00"
        lColumn.Caption = "5 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%10.00"
        lColumn.Caption = "10 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%20.00"
        lColumn.Caption = "20 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%30.00"
        lColumn.Caption = "30 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%40.00"
        lColumn.Caption = "40 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%50.00"
        lColumn.Caption = "50 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%60.00"
        lColumn.Caption = "60 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%70.00"
        lColumn.Caption = "70 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%80.00"
        lColumn.Caption = "80 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%90.00"
        lColumn.Caption = "90 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%95.00"
        lColumn.Caption = "95 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%97.5"
        lColumn.Caption = "97.5 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%98.00"
        lColumn.Caption = "98 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%99.00"
        lColumn.Caption = "99 Percentile"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "%99.90"
        lColumn.Caption = "99.9 Percentile"
        aDataTable.Columns.Add(lColumn)

        Return aDataTable
    End Function
End Module
