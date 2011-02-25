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
                Dim pSwatWeatherStations As New SwatWeatherStations
                Dim lLatitude As Double = 33.6
                Dim lLongitude As Double = -84
                Dim lNearestStations As SortedList(Of Double, PointLocation) = pSwatWeatherStations.Closest(lLatitude, lLongitude, 5)
                Dim lNearestStation As SwatWeatherStation = lNearestStations.Values(0)

                'TODO: get actual elevation of location rather than using station elveation
                Dim lElevation As Double = lNearestStation.Elev / 0.3048
                lElevation = 1010 'from BASINS StationLocs.dbf record 3500
                Dim lPanEvapTimeseries As atcTimeseries = _
                    PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lPrecipitationTS, lAirTemperatureTS, Nothing, lNearestStation)
                lPanEvapTimeseries.Attributes.CalculateAll()
                Logger.Dbg("Attributes " & lPanEvapTimeseries.Attributes.ToString)
            End If
        End If
    End Sub
End Module
