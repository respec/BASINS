Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcSWMM

Module SWMMDriver
    Private pPathName As String = "c:\Program Files (x86)\EPA SWMM 5.0\Examples\"
    Private pFileNames() As String = {"Example1.inp", "Example2.inp", "Example3.inp"}

    Public Function ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SWMMDriver Entry")
        Dim lResultAll As Boolean = True
        For Each lFileName As String In pFileNames
            Logger.Dbg("Test " & lFileName)
            Dim lSWMM As New atcSWMMProject
            lSWMM.Open(pPathName & lFileName)
            Dim lResult = lSWMM.RunSimulation()
            Logger.Dbg("SWMM Simulation Complete " & lResult)
 
            IO.Directory.SetCurrentDirectory(IO.Path.GetDirectoryName(pPathName))
            lSWMM.Save(IO.Path.ChangeExtension(lFileName, ".xxx"))
            lResultAll = lResult And lResultAll
        Next
        Return lResultAll
    End Function
End Module
