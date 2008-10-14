
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWDM
Imports System.Collections.Specialized

Module modUCIEditor
    Private pBaseDrive As String = "d:\"
    Private pBaseDir As String = pBaseDrive & "mono_luChange\"
    Private pOutputDir As String = pBaseDir & "output\"
    Private pScenarioName As String = "base"

    Sub main()
        Logger.StartToFile(pOutputDir & "UCIEditor.log")
        ChDriveDir(pBaseDir)
        Logger.Dbg("BaseDir " & CurDir())

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")
        Dim lUci As New atcUCI.HspfUci
        lUci.FastReadUciForStarter(lMsg, pScenarioName & ".uci")
        Logger.Dbg("UCI " & lUci.Name & " Opened")

        lUci.Name = "LU_Ch.uci"
        lUci.Save()
        Logger.Dbg("UCI " & lUci.Name & " Saved")

    End Sub
End Module
