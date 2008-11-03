Module modHspfReadWrite
    Dim pTestPath As String
    Dim pBaseName As String

    Sub Initialize()
        Dim lTestName As String
        lTestName = "SFBayColma"

        Select Case lTestName
            Case "mono"
                pTestPath = "d:\mono_base"
                pBaseName = "base"
            Case "SFBayColma"
                pTestPath = "G:\SFBay\Task1\UCOLMAnew"
                pBaseName = "UCOLMA"
        End Select
    End Sub

    Sub main()
        Initialize()
        atcUtility.ChDriveDir(pTestPath)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".uci")
            .Name = pBaseName & ".rev.uci"
            .Save()
        End With
    End Sub
End Module
