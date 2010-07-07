Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcSWMM

Module SWMMDriver
    Public Function ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SWMMDriver Entry")
        Dim lSWMM As New SWMMProject
        lSWMM.FileName = "c:\Program Files (x86)\EPA SWMM 5.0\Examples\Example1.inp"
        Dim lResult As Boolean = lSWMM.RunSimulation()
        Logger.Dbg("SWMM Simulation Complete " & lResult)
        Return lResult
    End Function
End Module
