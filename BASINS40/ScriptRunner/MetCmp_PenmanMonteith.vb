Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp
Imports atcData
Imports atcUtility

Module MetCmp_PenmanMonteith
    Dim pWdmDataPath As String = "G:\Data\BasinsMet\WDMFinal\"
    Dim pMetStation As String = "GA090451" 'Hartsfield
    Dim pMetId As Integer = 203 'GAATLANTAWBAIRPORT

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MetCmp_PenmanMonteith Start")

        Dim lElevation As Double = 1000.0

        Dim lFile As String = pWdmDataPath & pMetStation & ".wdm"
        Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM
        If lWDMFile.Open(lFile) AndAlso lWDMFile.DataSets.Count > 0 Then
            Dim lPrecipitationTS As atcTimeseries = lWDMFile.DataSets.FindData("Constituent", "PREC", 1).Item(0)
            Dim lAirTemperatureTS As atcTimeseries = lWDMFile.DataSets.FindData("Constituent", "ATEM", 1).Item(0)
            'Find common period for precip/temp
            Dim lSJD As Double = Math.Max(lPrecipitationTS.Attributes.GetValue("SJDay"), _
                                          lAirTemperatureTS.Attributes.GetValue("SJDay"))
            Dim lEJD As Double = Math.Min(lPrecipitationTS.Attributes.GetValue("EJDay"), _
                                          lAirTemperatureTS.Attributes.GetValue("EJDay"))
            If lSJD < lEJD Then 'common period found
                lPrecipitationTS = SubsetByDate(lPrecipitationTS, lSJD, lEJD, Nothing)
                lAirTemperatureTS = SubsetByDate(lAirTemperatureTS, lSJD, lEJD, Nothing)
                Dim lPanEvapTimeseries As atcTimeseries = _
                    PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lPrecipitationTS, lAirTemperatureTS, Nothing, pMetId)
                lPanEvapTimeseries.Attributes.CalculateAll()
                Logger.Dbg("Attributes " & lPanEvapTimeseries.Attributes.ToString)
            End If
        End If
    End Sub
End Module
