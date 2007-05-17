Imports MapWindow.Interfaces

Public Module ScriptDriver

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ScriptGridValues.scriptMain(aMapWin)
        'ScriptConstituentBalance.ScriptMain(aMapWin)
        'ScriptWatershedSummary.ScriptMain(aMapWin)
        'ScriptCatSummary.ScriptMain(aMapWin)
        'BASINSProjectSummary.ScriptMain(aMapWin)
        'ScriptShapefileFromWDM.ScriptMain(aMapWin)
    End Sub

End Module
