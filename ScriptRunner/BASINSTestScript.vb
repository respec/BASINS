Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinGeoProc
Imports atcUtility

Module BASINSTestScript
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lScriptName As String = "BASINSTestScript"
        Dim lTestSuccessful As Boolean = False

        Dim lTestMEthods As New TestMethods.TestMethods(lScriptName, aMapWin, True)
    End Sub
End Module
