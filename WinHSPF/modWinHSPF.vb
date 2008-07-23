Imports MapWinUtility
Imports atcUCI

Public Module WinHSPF
    Friend pUCI As HspfUci

    Sub Main()
        Logger.StartToFile("C:\dev\basins40\logs\WinHSPF.log")

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Logger.Dbg("WinHSPF:Opened:hspfmsg.mdb")

        Dim lWorkingDir As String = "C:\BASINS\modelout\sediment\"
        ChDir(lWorkingDir)
        Logger.Dbg("WinHSPF:WorkingDir:" & lWorkingDir & ":" & CurDir())

        pUCI = New HspfUci
        Dim lUCIName As String = "sed_riv.uci"
        pUCI.FastReadUci(lMsg, lUCIName)
        Logger.Dbg("WinHSPF:FastReadUci:Done:" & lUCIName)

        Dim lWinHSPF As New frmWinHSPF
        lWinHSPF.ShowDialog()
    End Sub
End Module
