
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcUCI
Imports atcWDM
Imports System.Collections.Specialized

Module modUCIEditor
    Private pBaseDrive As String
    Private pScenario As String = "mono"
    'Private pScenario As String = "hspf10"
    Private pBaseDir As String
    Private pOutputDir As String
    Private pScenarioName As String
    Private pScenarioNameNew As String
    Private pUpdateFileName As String = "C:\test\HSPF\Updates.txt"
    Private pdoVerify As Boolean = False
    Private pscenList As New ArrayList()

    Private pHSPFExe As String = "C:\Basins\models\HSPF\bin\WinHspfLt.exe"
    Private pHspfOutputFolder As String = "?"
    Private pRunModel As Boolean = True

    Sub setUpScenDirs(ByVal scenList As ArrayList)
        For Each scen As String In scenList
            If IO.Directory.Exists(pOutputDir & scen) Then
                Dim di As New System.IO.DirectoryInfo(pOutputDir & scen)
                di.Delete(True)
            End If
            IO.Directory.CreateDirectory(pOutputDir & scen)
            FileCopy(pBaseDir & "base.wdm", pOutputDir & scen & "\" & scen & ".wdm")
            FileCopy(pBaseDir & "base.output.wdm", pOutputDir & scen & "\" & scen & ".output.wdm")
            FileCopy(pBaseDir & "base.ptsrc.wdm", pOutputDir & scen & "\" & scen & ".ptsrc.wdm")
        Next
    End Sub
    Sub Initialize()
        Select Case pScenario
            Case "mono"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "mono_luChange\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "base"
                'pScenarioNameNew = "LU_Ch"
                'pUpdateFileName = pBaseDir & "lu2030a2.csv"
                'pUpdateFileName = pBaseDir & "lu2030b2.csv"
                'pUpdateFileName = pBaseDir & "lu2090a2.csv"
                'pUpdateFileName = pBaseDir & "lu2090b2.csv"
                pscenList.Add("lu2030a2")
                pscenList.Add("lu2030b2")
                pscenList.Add("lu2090a2")
                pscenList.Add("lu2090b2")
                setUpScenDirs(pscenList)
            Case "hspf10"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "test\HSPF\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "test10"
                pScenarioNameNew = "test10Rev"
                pUpdateFileName = "updates.txt"
            Case "hspf12"
                pBaseDrive = "c:\"
                pBaseDir = pBaseDrive & "test\HSPF\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "test12"
                pScenarioNameNew = "test12Rev"
        End Select
    End Sub

    Sub main()
        Initialize()
        Logger.StartToFile(pOutputDir & "UCIEditor.log")
        ChDriveDir(pBaseDir)
        Logger.Dbg("BaseDir " & CurDir())


        'Below is for comparing the values contained in the update file vs those used to populate the new SCHEMATIC block
        'Right now has to format the entries saved in the log file to a special format before this can be done, later can remove this
        If pdoVerify Then
            If zcompare("C:/mono_luChange/output/lu2090b2_readin.txt", pUpdateFileName) Then
                Logger.Msg("Compare OK")
            Else
                Logger.Msg("Compare found mismatch")
            End If
            Return
        End If

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lUci As New atcUCI.HspfUci

        Try
            lUci.FastReadUciForStarter(lMsg, pScenarioName & ".uci")
            Dim lError As String = lUci.ErrorDescription
            If lError.Length > 0 Then
                Logger.Dbg("Error " & lError)
            Else
                Logger.Dbg("UCI " & lUci.Name & " Opened")

                'add changes to scematic block here
                Logger.Dbg("ConnectionCount " & lUci.Connections.Count)
                'For Each lConnection As atcUCI.HspfConnection In lUci.Connections
                '    Logger.Dbg(lConnection.Source.VolName & " " & _
                '               lConnection.Source.VolId & " " & _
                '               lConnection.MFact & " " & _
                '               lConnection.Target.VolName & " " & _
                '               lConnection.Target.VolId)
                'Next
                'lUci.Name = pScenarioNameNew & ".uci"
                'lUci.Save()
                'Logger.Dbg("UCI " & lUci.Name & " Saved")
            End If
            Logger.Flush()

            'Loop through all scenarios and save their own set up in their own folders
            '

            Dim oldScen As String = ""
            For Each scen As String In pscenList
                If oldScen = "" Then oldScen = "base" 'first round, the old scen name is 'base.*'
                pUpdateFileName = scen & ".csv"
                pScenarioNameNew = pOutputDir & scen & "\" & scen & ".uci"
                pHspfOutputFolder = pOutputDir & scen
                If ModifyAreasInSchematicBlock(lUci) Then
                    Logger.Dbg("update loaded from " & pUpdateFileName)

                    'Write out the sum of total area
                    '
                    Dim ltotalArea As Double = 0.0
                    For Each lConnection As atcUCI.HspfConnection In lUci.Connections
                        ltotalArea += lConnection.MFact
                    Next
                    'Logger.Msg("UCI " & lUci.Name & " covers total areage:" & ltotalArea.ToString)
                    Logger.Dbg("UCI " & lUci.Name & " covers total areage:" & ltotalArea.ToString)
                    Logger.Flush()

                    ModifyFilesInFilesBlock(lUci, oldScen, scen)
                    lUci.Name = pScenarioNameNew
                    lUci.Save()
                    Logger.Dbg("UCI " & lUci.Name & " Saved")
                    Logger.Flush()

                End If
                oldScen = scen 'save the previous scen name for the file block for the replacement due to using the same luci object
            Next
            Logger.Msg("Done Setting up Hspf Land use change inputs.")

            'Run Hspf
            If pRunModel Then
                For Each scen As String In pscenList
                    pScenarioNameNew = pOutputDir & scen & "\" & scen & ".uci"
                    pHspfOutputFolder = pOutputDir & scen

                    Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & pHspfOutputFolder & " for " & pScenarioNameNew)
                    Logger.Flush()
                    LaunchProgram(pHSPFExe, pHspfOutputFolder, pScenarioNameNew)
                    Logger.Dbg("HSPFRun Finished")
                Next
            End If
        Catch ex As Exception
            Logger.Msg("Error: " & ex.ToString)
        End Try
    End Sub

    Private Function ModifyFilesInFilesBlock(ByRef aUciOriginal As atcUCI.HspfUci, ByVal oldScen As String, ByVal newScen As String) As Boolean
        'Change the file names to corresponding scenario name

        'aUciOriginal.FilesBlock.newNameAll("base.wdm", newScen & ".wdm")
        'aUciOriginal.FilesBlock.newNameAll("base.output.wdm", newScen & ".output.wdm")
        'aUciOriginal.FilesBlock.newNameAll("base.ptsrc.wdm", newScen & ".ptsrc.wdm")
        'aUciOriginal.FilesBlock.newNameAll("base", newScen)
        aUciOriginal.FilesBlock.newNameAll(oldScen, newScen)

    End Function

    Private Function ModifyAreasInSchematicBlock(ByRef aUciOriginal As atcUCI.HspfUci) As Boolean
        'Replace area for each schematic source-2-target combination
        Dim lAreaTable As New atcTableDelimited
        lAreaTable.Delimiter = ","
        If Not lAreaTable.OpenFile(pUpdateFileName) Then
            Throw New Exception("Failed to Open LandUse in '" & pUpdateFileName & "'")
            Return False
        Else  'look through each rchres
            Dim lFieldVal(1) As String
            Dim lFieldNum(1) As Integer
            lFieldNum(0) = 1
            lFieldNum(1) = 2
            Dim lTableOper(1) As String
            lTableOper(0) = "="
            lTableOper(1) = "="

            Dim lRchIDs As New Collection
            Dim lDownRchIDs As New Collection

            Dim lArea As Double
            Dim lLandUse As String

            'For verification purpose:
            Dim ldbgStr As String
            For Each lRchOper As atcUCI.HspfOperation In aUciOriginal.OpnBlks("RCHRES").Ids
                lFieldVal(0) = lRchOper.Tables("GEN-INFO").Parms("RCHID").Value
                For Each lOperType As String In New String() {"PERLND", "IMPLND"}
                    For Each lLandOper As atcUCI.HspfOperation In aUciOriginal.OpnBlks(lOperType).Ids
                        lFieldVal(1) = Mid(lLandOper.Tables("GEN-INFO").Parms("LSID").Value, 1, 6)
                        lLandUse = Trim(Mid(lLandOper.Tables("GEN-INFO").Parms("LSID").Value, 7))
                        If lAreaTable.FindMatch(lFieldNum, lTableOper, lFieldVal) Then
                            'found this land series contributing to this reach
                            lArea = lAreaTable.Value(lAreaTable.FieldNumber(lLandUse.ToUpper))
                            If lArea > 0 Then
                                For Each lConn As atcUCI.HspfConnection In aUciOriginal.Connections
                                    If lConn.Source.VolName = lLandOper.Name And lConn.Source.VolId = lLandOper.Id And _
                                       lConn.Target.VolName = lRchOper.Name And lConn.Target.VolId = lRchOper.Id Then
                                        'found it
                                        lConn.MFact = lArea

                                        'For Verification printout
                                        ldbgStr = lFieldVal(0) & ";" & lFieldVal(1) & ";" & lLandUse & ";->" & lConn.Source.VolName & " " & lConn.Source.VolId & lArea.ToString.PadLeft(10) & "   RCHRES  " & lConn.Target.VolId
                                        Logger.Dbg(ldbgStr)
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Next lLandOper
                Next lOperType
                'also get reach to reach connections out of the reach id
                lRchIDs.Add(Mid(lFieldVal(0), 5, 4))
                lDownRchIDs.Add(Mid(lFieldVal(0), 10, 4))
                Logger.Dbg("LandInputConnectionsCompleteFor " & lRchOper.Name)
            Next lRchOper
        End If
        Return True
    End Function
End Module
