Imports atcUtility.modFile
Imports MapWindow.Interfaces

Module HSPFOutputReports
    Private Const pTestPath As String = "C:\test\EXP_CAL\hyd_man.net"
    Private Const pBaseName As String = "hyd_man"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pTestPath)
        Dim lExpSysStats As String
        lExpSysStats = GetExpSysStats(pBaseName & ".uci", pbasename & ".exs", CurDir)
        SaveFileString("outfiles\ExpertSysStats.txt", lExpSysStats)
    End Sub
End Module
