Imports MapWinUtility
Imports atcUCI
Imports atcUCIForms

Public Module WinHSPF
    Friend pUCI As HspfUci
    Friend pMsg As HspfMsg
    Friend pIcon As Icon
    'Friend pIPC As ATCoIPC

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

    Sub EditBlock(ByVal aParent As Windows.Forms.Form, ByVal aTableName As String)
        If aTableName = "GLOBAL" Then
            UCIForms.Edit(aParent, pUCI.GlobalBlock)
        ElseIf aTableName = "OPN SEQUENCE" Then
            UCIForms.Edit(aParent, pUCI.OpnSeqBlock)
        ElseIf aTableName = "FILES" Then
            UCIForms.Edit(aParent, pUCI.FilesBlock)
        ElseIf aTableName = "CATEGORY" Then
            UCIForms.Edit(aParent, pUCI.CategoryBlock)
        ElseIf aTableName = "FTABLES" Then
            If pUCI.OpnBlks("RCHRES").Count > 0 Then
                UCIForms.Edit(aParent, pUCI.OpnBlks("RCHRES").Ids(0).FTable)
            Else
                Logger.Message("The current project contains no reaches.", "FTable Editor Problem", _
                               MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
            End If
        ElseIf aTableName = "MONTH-DATA" Then

        ElseIf aTableName = "EXT SOURCES" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "NETWORK" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "SCHEMATIC" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "EXT TARGETS" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "MASS-LINK" Then

        ElseIf aTableName = "SPEC-ACTIONS" Then
            UCIForms.Edit(aParent, pUCI.SpecialActionBlk, aTableName)
            'UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        Else
            'DisableAll(True)
            Logger.Msg("Table/Block " & aTableName & " not found.", MsgBoxStyle.OkOnly, "Edit Problem")
            'DisableAll(False)
        End If
    End Sub
End Module
