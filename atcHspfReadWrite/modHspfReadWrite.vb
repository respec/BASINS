Module modHspfReadWrite
    Dim pTestPath As String
    Dim pBaseName As String

    Sub Initialize()
        Dim lTestName As String
        lTestName = "SFBayColma"
        'lTestName = "mono"

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

        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")

        Dim lHspfUci As New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".uci")
            .Name = pBaseName & ".rev.uci"
            .Save()
        End With
        lHspfUci = Nothing
        lHspfUci = New atcUCI.HspfUci
        With lHspfUci
            .FastReadUciForStarter(lMsg, pBaseName & ".rev.uci")
            .Name = pBaseName & ".rev2.uci"
            .Save()
            .SaveAs(pBaseName, pBaseName & "New", 1, 1)
        End With
    End Sub
End Module
