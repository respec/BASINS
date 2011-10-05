Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMetCmp
Imports atcData
Imports atcUtility

Module MetCmp_PMEt

    Dim pWdmDataPath As String = "C:\Basins\data\SomeWDMs\" '<<<change this to your wdm folder
    Dim pDebug As Boolean = False
    Dim pSingleStation As Boolean = False
    Dim pWriteHourly As Boolean = True

    Dim pDateSubset As Boolean = False
    Dim pSJDate As Double = Date2J(1977, 1, 1)
    Dim pEJDate As Double = Date2J(1978, 1, 1)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("MetCmp_PMEt Start")

        Dim lFileNames As New NameValueCollection
        Dim lSingleStationWDMFilename As String = "AK509739.wdm" '<<<change this to your filename if you want to do just a single WDM
        If pSingleStation Then
            Dim lFileName As String = pWdmDataPath & lSingleStationWDMFilename
            lFileNames.Add(lFileName.ToLower, lFileName)
        Else
            AddFilesInDir(lFileNames, pWdmDataPath, True, "*.wdm")
        End If
        Logger.Dbg("MetCmp_PMEt: Found " & lFileNames.Count & " WDM data files")

        Dim lSW As IO.StreamWriter
        Dim lLogFilename As String = pWdmDataPath & "zMetCmp_PMEt_Log.txt"
        If FileExists(lLogFilename) Then
            TryDelete(lLogFilename)
        End If
        Try
            lSW = New IO.StreamWriter(lLogFilename, False)
            lSW.WriteLine("**************************")
            lSW.WriteLine("***" & Date.Now & "***")
            lSW.WriteLine("**************************")
        Catch ex As Exception
            lSW = Nothing
        End Try

        Dim lCount As Integer = 0
        Dim lSwatWeatherStations As New SwatWeatherStations
        Try
            For Each lFileName As String In lFileNames
                lCount += 1
                Dim lMetStation As String = IO.Path.GetFileNameWithoutExtension(lFileName)
                Logger.Dbg("Process " & lMetStation & "(" & lCount & " of " & lFileNames.Count & ")")
                Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM
                If lWDMFile.Open(lFileName) AndAlso lWDMFile.DataSets.Count > 0 Then
                    Dim lTsPrecGroup As atcTimeseriesGroup = lWDMFile.DataSets.FindData("Constituent", "PREC")
                    Dim lTsAtemGroup As atcTimeseriesGroup = lWDMFile.DataSets.FindData("Constituent", "ATEM")
                    Dim lTsPrec As atcTimeseries = Nothing
                    Dim lTsAtem As atcTimeseries = Nothing

                    Dim lThisLocation As String = ""
                    For Each lTsPrec In lTsPrecGroup
                        lThisLocation = lTsPrec.Attributes.GetValue("Location", "")
                        lTsAtem = Nothing
                        lTsAtem = lTsAtemGroup.FindData("Location", lThisLocation).Item(0)
                        If lTsAtem IsNot Nothing Then
                            Dim lDate(5) As Integer
                            Dim lSJD As Double = Math.Max(lTsPrec.Attributes.GetValue("SJDay"), _
                                                          lTsAtem.Attributes.GetValue("SJDay"))
                            If pDateSubset Then lSJD = pSJDate
                            J2Date(lSJD, lDate)
                            If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                                lDate(0) += 1
                                lDate(1) = 1
                                lDate(2) = 1
                                lDate(3) = 0
                                lDate(4) = 0
                                lDate(5) = 0
                                lSJD = Date2J(lDate)
                            End If
                            Dim lEJD As Double = Math.Min(lTsPrec.Attributes.GetValue("EJDay"), _
                                                          lTsAtem.Attributes.GetValue("EJDay"))
                            If pDateSubset Then lEJD = pEJDate
                            J2Date(lEJD, lDate)
                            If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                                lDate(1) = 1
                                lDate(2) = 1
                                lDate(3) = 0
                                lDate(4) = 0
                                lDate(5) = 0
                                lEJD = Date2J(lDate)
                            End If
                            If lSJD < lEJD Then 'common period found
                                lTsPrec = SubsetByDate(lTsPrec, lSJD, lEJD, Nothing)
                                lTsAtem = SubsetByDate(lTsAtem, lSJD, lEJD, Nothing)
                                Dim lLatitude As Double = lTsAtem.Attributes.GetValue("Latitude")
                                Dim lLongitude As Double = lTsAtem.Attributes.GetValue("Longitude")
                                Dim lNearestStations As SortedList(Of Double, PointLocation) = lSwatWeatherStations.Closest(lLatitude, lLongitude, 5)
                                Dim lNearestStation As SwatWeatherStation = lNearestStations.Values(0)

                                'TODO: get actual elevation of location rather than using station elveation
                                Dim lElevation As Double = lNearestStation.Elev / 0.3048
                                Dim lPanEvapTimeseries As atcTimeseries = _
                                    PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPrec, lTsAtem, Nothing, lNearestStation, 0.0, , , pDebug)
                                lPanEvapTimeseries.SetInterval(atcTimeUnit.TUDay, 1)
                                lPanEvapTimeseries.Attributes.SetValue("ID", lTsAtem.Attributes.GetValue("ID") + 6)
                                lPanEvapTimeseries.Attributes.SetValue("TU", atcTimeUnit.TUDay)
                                lPanEvapTimeseries.Attributes.SetValue("description", "SWAT PM ET inches")

                                'Disaggragate the daily PMET timeseries into hourly timeseries
                                Dim lID As Integer = lPanEvapTimeseries.Attributes.GetValue("ID")
                                Dim ltsPMETHour As atcTimeseries = atcMetCmp.DisSolPet(lPanEvapTimeseries, Nothing, 2, lLatitude)
                                ltsPMETHour.Attributes.SetValue("Constituent", lPanEvapTimeseries.Attributes.GetValue("Constituent"))
                                ltsPMETHour.Attributes.SetValue("TSTYPE", lPanEvapTimeseries.Attributes.GetValue("TSTYPE"))
                                ltsPMETHour.Attributes.SetValue("ID", lID)
                                ltsPMETHour.Attributes.SetValue("Location", lThisLocation)

                                'Add the newly calculated hourly PMET timeseries back into the current WDM, overwrite if already exists.
                                If lWDMFile.AddDataset(ltsPMETHour, atcDataSource.EnumExistAction.ExistReplace) Then
                                    Dim lSumAnnualPET As Double = ltsPMETHour.Attributes.GetValue("SumAnnual")
                                    If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " Penman-Monteith PET at DSN " & lID & " in file " & lWDMFile.Specification)
                                    Logger.Dbg("Wrote Penman-Monteith PET to DSN " & lID & " SumAnnual " & lSumAnnualPET)
                                Else
                                    If lSW IsNot Nothing Then lSW.WriteLine(lThisLocation & " failed writing Penman-Montheith PET to DSN " & lID & " in file " & lWDMFile.Specification)
                                    Logger.Dbg("**** Problem Writing Penman-Monteith PET to DSN " & lID)
                                End If
                                lTsPrec.Clear()
                                lTsPrec = Nothing
                                lTsAtem.Clear()
                                lTsAtem = Nothing
                                lPanEvapTimeseries.Clear()
                                lPanEvapTimeseries = Nothing
                                lNearestStations.Clear()
                                lNearestStations = Nothing
                                ltsPMETHour.Clear()
                                ltsPMETHour = Nothing
                            End If
                        End If
                    Next
                    lTsPrecGroup.Clear()
                    lTsAtemGroup.Clear()
                    lTsPrecGroup = Nothing
                    lTsAtemGroup = Nothing
                    lWDMFile.Clear()
                    lWDMFile = Nothing
                End If
                If lSW IsNot Nothing Then lSW.Flush()
            Next
        Catch ex As Exception
            If lSW IsNot Nothing Then lSW.WriteLine("Failure: " & ex.Message)
            Logger.Dbg("MetCmp_PMEt Failure: " & ex.Message)
        End Try

        If lSW IsNot Nothing Then
            lSW.WriteLine("Done at " & Date.Now)
            lSW.Flush()
            lSW = Nothing
        End If
        If lSwatWeatherStations IsNot Nothing Then
            lSwatWeatherStations.Clear()
            lSwatWeatherStations = Nothing
        End If
        Logger.Dbg("MetCmp_PMEt Finished.")
    End Sub
End Module
