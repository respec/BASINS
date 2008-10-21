Imports MapWinUtility
Imports MapWindow.Interfaces

Module HSPFRunner
    Private pHSPFExe As String = "D:\Basins\models\HSPF\bin\WinHspfLt.exe"
    Private pOutputFolder As String = "?"
    Private pRunModel As Boolean = True

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lUCINames As New Collection
        lUCINames.Add("?.uci")

        If pRunModel Then
            For Each lUCIName As String In lUCINames
                Logger.Dbg("Launching " & IO.Path.GetFileName(pHSPFExe) & " in " & pOutputFolder & " for " & lUCIName)
                Logger.Flush()
                LaunchProgram(pHSPFExe, pOutputFolder, lUCIName)
                Logger.Dbg("HSPFRun Finished")
            Next
        End If
    End Sub
End Module
