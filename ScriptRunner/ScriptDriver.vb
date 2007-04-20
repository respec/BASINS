Imports MapWindow.Interfaces

Public Module ScriptDriver

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ScriptConstituentBalance.ScriptMain(aMapWin)
        ScriptWatershedSummary.ScriptMain(aMapWin)
        ScriptCatSummary.ScriptMain(aMapWin)
        'BASINSProjectSummary.ScriptMain(pMapWin)
    End Sub

End Module
