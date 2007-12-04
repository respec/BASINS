Imports atcUtility
Imports MapWindow.Interfaces
Imports atcUCI

Module CreateUCITest

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lFolder As String = "C:\test\UCICreation\"
        ChDriveDir(lFolder)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lMetWdms(4) As String
        lMetWdms(1) = "md.wdm"
        Dim lWdmIds(4) As String
        lWdmIds(0) = "WDM1"
        lWdmIds(1) = "WDM2"

        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.CreateUciFromBASINS(lMsg, "UCICreation.wsd", _
                                     "test.wdm", _
                                     lMetWdms, lWdmIds, _
                                     "11,1970,1,1,0,0,0,1995,12,31,24,0,0,WDM2", True)
        lHspfUci.Save()
    End Sub
End Module
