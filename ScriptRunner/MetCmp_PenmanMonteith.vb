Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp

Module MetCmp_PenmanMonteith
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MetCmp_PenmanMonteith Start")
        Dim lStations As New SwatWeatherStations
        Logger.Dbg("StationCount " & lStations.Count)
    End Sub
End Module
