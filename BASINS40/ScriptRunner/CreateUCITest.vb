Imports atcUtility
Imports MapWindow.Interfaces

Module CreateUCITest

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci

        Dim lMetWdms(4) As String
        lMetWdms(1) = "C:\BASINS\modelout\UCICreation\md.wdm"

        Dim lWdmIds(4) As String
        lWdmIds(0) = "WDM1"
        lWdmIds(1) = "WDM2"

        atcUCI.CreateUciFromBASINS(lHspfUci, lMsg, "C:\BASINS\modelout\UCICreation\UCICreation.wsd", _
                                   "C:\BASINS\modelout\UCICreation\test.wdm", _
                                   lMetWdms, lWdmIds, _
                                   "11,1970,1,1,0,0,0,1995,12,31,24,0,0,WDM2", True)

    End Sub
End Module
