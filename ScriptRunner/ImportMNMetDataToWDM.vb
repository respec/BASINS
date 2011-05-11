Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Imports System.IO
Imports System.Text

Public Module ImportMNMetData
    Private Const pInputPath As String = "D:\BasinsMet\MPCA\Combined\"

    Private Class StationInfo
        Friend Name As String
        Friend Lat As String
        Friend Lng As String
    End Class

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lFile As String = "CrowWingLpRPrecipOKAqTrFiltered.dat"

        Dim lNOAAFile As New atcTimeseriesNOAA.atcDataSourceNOAA
        lNOAAFile.Open(lFile)

        Dim lLat As Double = 0.0
        Dim lLng As Double = 0.0
        Dim lStaName As String = ""

        Dim lStations As New atcCollection

        Dim lTable As New atcTableDelimited
        lTable.OpenFile("MetStationTable.csv")
        Dim lStat As New StationInfo

        Do While Not lTable.EOF
            lStat = New StationInfo
            lStat.Name = lTable.Value(3) : lStat.Lat = lTable.Value(1) : lStat.Lng = lTable.Value(2)
            lStations.Add(lStat.Name, lStat)
            lTable.MoveNext()
        Loop

        lStat = New StationInfo
        If lNOAAFile.DataSets.Count > 0 Then
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            Dim lCurWDM As String = "D:\BasinsMet\MPCA\Combined\test2.wdm"
            lWDMfile.Open(lCurWDM)

            For Each lDS As atcDataSet In lNOAAFile.DataSets
                If lStations.Keys.Contains(lDS.Attributes.GetValue("Location")) Then
                    lStat = lStations.ItemByKey(lDS.Attributes.GetValue("Location"))
                    lDS.Attributes.SetValue("STANAM", lStat.Name)
                    lDS.Attributes.SetValue("LATDEG", lStat.Lat)
                    lDS.Attributes.SetValue("LNGDEG", lStat.Lng)
                    If lWDMfile.AddDataset(lDS) Then
                        Logger.Dbg("ReadSODToWDM: Saved " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                    Else
                        Logger.Dbg("ReadSODToWDM: PROBLEM Saving " & lDS.Attributes.GetValue("Constituent") & " on DSN " & lDS.Attributes.GetValue("ID") & " of WDM file")
                    End If
                End If
            Next
        End If

    End Sub

End Module
