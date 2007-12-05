Imports System.Collections.ObjectModel
Imports MapWindow.Interfaces
Imports atcUCI
Imports atcUtility

Module CreateUCITest

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lFolder As String = "C:\test\UCICreation\"
        ChDriveDir(lFolder)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        Dim lDataSources As New Collection(Of atcData.atcDataSource)
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        lDataSource.Open("test.wdm")
        lDataSources.Add(lDataSource)
        lDataSource = New atcWDM.atcDataSourceWDM
        lDataSource.Open("md.wdm")
        lDataSources.Add(lDataSource)

        Dim lStarterUciName As String = "starter.uci"

        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.CreateUciFromBASINS(lMsg, "UCICreation.wsd", _
                                     lDataSources, _
                                     "11,1970,1,1,0,0,0,1995,12,31,24,0,0,WDM2", True, _
                                     lStarterUciName)
        lHspfUci.Save()
    End Sub
End Module
