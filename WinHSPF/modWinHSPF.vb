Imports MapWinUtility
Imports atcUCI

Public Module WinHSPF
    Friend pUCI As HspfUci
    Friend pMsg As HspfMsg
    Friend pIcon As Icon

    Sub Main()
        Logger.StartToFile("C:\dev\basins40\logs\WinHSPF.log")

        pMsg = New HspfMsg
        pMsg.Open("hspfmsg.mdb")
        Logger.Dbg("WinHSPF:Opened:hspfmsg.mdb")

        Dim lWinHSPF As New frmWinHSPF
        pIcon = lWinHSPF.Icon
        lWinHSPF.ShowDialog()
    End Sub

    Sub OpenUCI()
        'open an existing uci (hard-coded for now)
        Dim lWorkingDir As String = "C:\BASINS\modelout\sediment\"
        ChDir(lWorkingDir)
        Logger.Dbg("WinHSPF:WorkingDir:" & lWorkingDir & ":" & CurDir())

        pUCI = New HspfUci
        Dim lUCIName As String = "sed_riv.uci"
        pUCI.FastReadUciForStarter(pMsg, lUCIName)
        'Dim lFilesOK As Boolean = True
        'Dim lEchoFile As String = ""
        'pUCI.ReadUci(pMsg, lUCIName, 1, lFilesOK, lEchoFile)
        Logger.Dbg("WinHSPF:FastReadUci:Done:" & lUCIName)
    End Sub

    Sub SaveUCI()
        pUCI.Save()
        Logger.Dbg("WinHSPF:SaveUci:Done:" & pUCI.Name)
    End Sub

    Sub ReachEditor()
        If pUCI.OpnBlks("RCHRES").Count > 0 Then
            frmReach.Show()
            'ClearTree()
            'BuildTree()
            'UpdateLegend()
            'UpdateDetails()
        Else
            Logger.Message("The current project contains no reaches.", "Reach Editor Problem", _
                           MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK)
        End If
    End Sub
End Module
