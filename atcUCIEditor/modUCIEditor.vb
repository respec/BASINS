
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWDM
Imports System.Collections.Specialized

Module modUCIEditor
    Private pBaseDrive As String = "c:\"
    'Private pScenario As String = "base"
    Private pScenario As String = "hspf12"
    Private pBaseDir As String
    Private pOutputDir As String
    Private pScenarioName As String
    Private pScenarioNameNew As String

    Sub Initialize()
        Select Case pScenario
            Case "mono"
                pBaseDir = pBaseDrive & "mono_luChange\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "base"
                pScenarioNameNew = "LU_Ch"
            Case "hspf10"
                pBaseDir = pBaseDrive & "test\HSPF\"
                pOutputDir = pBaseDir & "output\"
                pScenarioName = "test10"
                pScenarioNameNew = "test10Rev"
            Case "hspf12"
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

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lUci As New atcUCI.HspfUci
        lUci.FastReadUciForStarter(lMsg, pScenarioName & ".uci")
        Logger.Dbg("UCI " & lUci.Name & " Opened")

        'add changes to scematic block here

        lUci.Name = pScenarioNameNew & ".uci"
        lUci.Save()
        Logger.Dbg("UCI " & lUci.Name & " Saved")

    End Sub
End Module
