Imports System.Collections.ObjectModel
Imports MapWindow.Interfaces
Imports atcUCI
Imports atcUtility
Imports atcSegmentation

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
        Dim lPollutantListFileName As String = "Poltnt_2.prn"

        Dim lMetBaseDsn As Integer = 11
        Dim lMetWdmId As String = "WDM2"

        Dim lWatershedName As String = "UCICreation"
        Dim lWatershed As New Watershed
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.Msg = lMsg
            lHspfUci.CreateUciFromBASINS(lWatershed, _
                                         lDataSources, _
                                         lStarterUciName, lPollutantListFileName)
            lHspfUci.Save()
        End If
    End Sub
End Module
