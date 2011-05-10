Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Imports System.IO
Imports System.Text

Public Module ImportMNMetData
    Private Const pInputPath As String = "D:\BasinsMet\MPCA\Combined\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lFile As String = "CrowWingLpRPrecipOKAqTrFiltered.dat"

        Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
        lNOAAFile.Open(lFile)

        Dim lLat As Double = 0.0
        Dim lLng As Double = 0.0
        Dim lStaName As String = ""

        '        292021_5176418	46.70896	95.72096
        '295088_5119937	46.20215	95.65607
        '297698_5102077	46.04235	95.61466
        '298010_5203786	46.95681	95.6548
        '301425_5145540	46.43421	95.58483
        '302587_5226119	47.15895	95.60446
        '310699_5083868	45.88233	95.43959
        '312370_5109747	46.1155	95.42824
        '313919_5083762	45.88226	95.39809
        '316733_5090204	45.94094	95.36431
        '317864_5154677	46.52101	95.37462
        '318682_5102963	46.0562	95.34403
        '318874_5088465	45.92587	95.33606
        '331413_5202750	46.95683	95.21576
        '333317_5088051	45.92581	95.14978
        '335098_5141333	46.40544	95.14537
        '335880_5209072	47.01481	95.15938
        '337593_5220316	47.11634	95.1409
        '339275_5202631	46.95772	95.11246
        '340700_5191306	46.85621	95.08978
        '342274_5191297	46.85651	95.06915
        '342451_5197742	46.91451	95.06905
        '342586_5095815	45.99783	95.03286
        '342654_5081464	45.86876	95.02727
        '343309_5120049	46.21597	95.03152
        '345149_5157169	46.55025	95.01996
        '345619_5196067	46.90019	95.02691
        '345771_5228139	47.18865	95.03585
        '346644_5176618	46.72551	95.00693
        '352266_5203900	46.97215	94.94223
        '355443_5093948	45.98386	94.86635
        '355926_5111659	46.14329	94.86548
        '356101_5168224	46.65212	94.88061
        '356385_5189210	46.84093	94.88348
        '360706_5134239	46.3474	94.81029
        '361465_5203666	46.97203	94.82129
        '362000_5077750	45.8395	94.77708
        '362255_5087375	45.92614	94.77656
        '362804_5159986	46.57942	94.79061
        '368191_5222575	47.14348	94.73842
        '380943_5214747	47.0755	94.56824
        '385350_5185475	46.81295	94.50282
        '394320_5101971	46.06315	94.36636
        '395338_5194950	46.89984	94.37412
        '395494_5140002	46.4055	94.35961
        '399490_5169071	46.66765	94.31394
        '406639_5133393	46.34768	94.21333
        '407697_5132326	46.33822	94.19938
        '410797_5170500	46.68211	94.16643
        '412583_5138623	46.39553	94.13707
        '415559_5168831	46.66771	94.10387


        If lNOAAFile.DataSets.Count > 0 Then
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            Dim lCurWDM As String = "D:\BasinsMet\MPCA\Combined\test.wdm"
            lWDMfile.Open(lCurWDM)

            For Each lDS As atcDataSet In lNOAAFile.DataSets
                lDS.Attributes.SetValue("STANAM", lStaName)
                lDS.Attributes.SetValue("LATDEG", lLat)
                lDS.Attributes.SetValue("LNGDEG", lLng)
                If lWDMfile.AddDataset(lDS) Then
                    Logger.Dbg("ReadSODToWDM: Saved " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                Else
                    Logger.Dbg("ReadSODToWDM: PROBLEM Saving " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                End If
            Next
        End If

    End Sub

End Module
