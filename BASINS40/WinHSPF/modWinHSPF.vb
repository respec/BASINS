Imports MapWinUtility
Imports atcUCIForms

Public Module WinHSPF
    Sub Main()
        Logger.StartToFile("C:\dev\basins40\logs\WinHSPF.log")
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Logger.Dbg("WinHSPF:Opened:hspfmsg.mdb")

        Dim lWorkingDir As String = "C:\test\climate\"
        ChDir(lWorkingDir)
        Dim lUCI As New atcUCI.HspfUci
        Dim lUCIName As String = "modified.uci"
        lUCI.FastReadUci(lMsg, lUCIName)
        Logger.Dbg("WinHSPF:FastReadUci:Done")
        UCIForms.Edit(lUCI.FilesBlock)

        'Dim lWinHSPF As New frmWinHSPF
        'lWinHSPF.Show()
    End Sub
End Module
