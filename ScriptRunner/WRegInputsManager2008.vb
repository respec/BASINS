Imports atcWDM
Imports atcUtility
Imports System.Collections.Generic
Imports MapWinUtility
Imports MapWindow.Interfaces
Imports WREGLib2008
Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions

Public Module WRegInputsManager
    Private pWDMDownload As String = "DownloadFlow.wdm"
    Private pSTATSConfig As String = "statsconfig.txt"
    Private pSiteInfoDir As String = "G:\Admin\USGS_DO11_WREG\WREGsamplefiles\"
    Private pDefaultProjection As String = "+proj=latlong +datum=NAD83"
    Private pStationList As atcCollection
    Private pLocalDataSource As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        pSiteInfoDir = "C:\test\WREGsamplefiles\"
        Logger.Dbg("Prepare WREG Inputs:CurDir:" & CurDir())
        pWDMDownload = Path.Combine(pSiteInfoDir, "DownloadFlow.wdm")

        Dim lWREGInputs As New WREGInputs
        Dim lLogFilename As String = Path.Combine(pSiteInfoDir, "WREGInputLog.txt")
        Dim lSW As StreamWriter = New StreamWriter(lLogFilename, False)

        Dim lYesNo() As String = {"Yes", "No"}
        If Logger.MsgCustom("Read data from local WDM file?", "Local Data Source", lYesNo) = "Yes" Then
            pLocalDataSource = True
            pWDMDownload = FindFile("Specify Local WDM data file", pWDMDownload, "wdm")
            If Not File.Exists(pWDMDownload) Then
                Logger.Msg("File doesn't exist: " & pWDMDownload, MsgBoxStyle.Critical, "No WDM Flow Data")
                Exit Sub
            End If
        Else
            pLocalDataSource = False
        End If

        Dim lSiteInfoTab As atcTableDelimited = Nothing
        If Not pLocalDataSource Then
            lSiteInfoTab = New atcTableDelimited
            With lSiteInfoTab
                .Delimiter = vbTab
                Dim lSiteInfoFilename As String = FindFile("Specify List of Stations", Path.Combine(pSiteInfoDir, "SiteInfo.txt"), "txt")
                If .OpenFile(lSiteInfoFilename) Then
                    pStationList = New atcCollection
                    .MoveFirst()
                    While Not .EOF
                        pStationList.Add(.Value(1))
                        .MoveNext()
                    End While
                Else
                    Logger.Msg("Open SiteInfo.txt failed. Stop processing.")
                    .Clear()
                    Exit Sub
                End If
            End With
            lSiteInfoTab.Clear()
            lSiteInfoTab = Nothing
        End If

        pSTATSConfig = FindFile("Specify NDay-Freq Statistics Config File", Path.Combine(pSiteInfoDir, "statsconfig.txt"), "txt")
        Dim lSTATSConfigTab As New atcTableDelimited
        With lSTATSConfigTab
            .Delimiter = vbTab
            If .OpenString("HL" & vbTab & "Day" & vbTab & "Year" & vbCrLf & File.ReadAllText(pSTATSConfig)) Then
                lWREGInputs.NDayFreqs = New atcCollection
                .MoveFirst()
                Dim lStatName As String = ""
                While Not .EOF
                    If .Value(1).Trim.ToLower = "low" Then
                        lWREGInputs.NDayFreqs.Add(.Value(2).Trim & "Low" & .Value(3).Trim)
                    ElseIf .Value(1).Trim.ToLower = "high" Then
                        lWREGInputs.NDayFreqs.Add(.Value(2).Trim & "High" & .Value(3).Trim)
                    End If
                    .MoveNext()
                End While
            End If
        End With
        lSTATSConfigTab.Clear()
        lSTATSConfigTab = Nothing

        If lWREGInputs.NDayFreqs Is Nothing OrElse lWREGInputs.NDayFreqs.Count = 0 Then
            Logger.Msg("No Nday Frequency Statistics are specified. Stop preparing WREG inputs.")
            If lSTATSConfigTab IsNot Nothing Then
                lSTATSConfigTab.Clear() : lSTATSConfigTab = Nothing
            End If
            If lSiteInfoTab IsNot Nothing Then
                lSiteInfoTab.Clear() : lSiteInfoTab = Nothing
            End If
            Exit Sub
        End If

        Dim lWREGInputDir As String = Path.Combine(pSiteInfoDir, "WREGInputs")
        MkDirPath(lWREGInputDir)
        With lWREGInputs
            .InputPath = lWREGInputDir
            .SiteInfoDir = pSiteInfoDir
            .WDMDownload = pWDMDownload
        End With

        'Download data from NWIS if no local WDM file as data source
        If Not pLocalDataSource Then
            TryDelete(lWREGInputs.WDMDownload)
            lWREGInputs.DownloadData(pStationList)
        End If

        'Read data from WDM
        Dim lWDM As New atcWDM.atcDataSourceWDM()
        If Not lWDM.Open(lWREGInputs.WDMDownload) Then
            Logger.Msg("Open WDM file, " & pWDMDownload, " failed. Stop processing.")
            Exit Sub
        End If

        Dim lDataSets As atcData.atcTimeseriesGroup = Nothing
        If pLocalDataSource Then
            lDataSets = New atcData.atcTimeseriesGroup()
            For Each lTs As atcData.atcTimeseries In lWDM.DataSets
                If lTs.Attributes.GetValue("Constituent").ToString.ToUpper = "FLOW" Then
                    lDataSets.Add(lTs)
                End If
            Next
        Else
            lDataSets = lWDM.DataSets
        End If

        Dim lDate(5) As Integer
        Logger.Msg("DatasetCount " & lDataSets.Count)
        For Each lDS As atcData.atcDataSet In lDataSets
            Dim lDefVal As atcData.atcDefinedValue = Nothing
            Dim lStationId As String = lDS.Attributes.GetValue("Location").ToString
            Dim lLatitude As String = lDS.Attributes.GetValue("Latitude", 0.0)
            Dim lLongitude As String = lDS.Attributes.GetValue("Longitude", 0.0)
            Dim lTsAnnual As atcData.atcTimeseries = Nothing
            Dim lValueK As Double
            Dim lValueG As Double
            Dim lValueS As Double
            Dim lCompleteStats As Boolean = True
            For Each lStat As String In lWREGInputs.NDayFreqs
                Dim lValueString As String = lDS.Attributes.GetValue(lStat, "?")
                If lValueString <> "NaN" AndAlso lValueString <> "?" AndAlso lCompleteStats Then
                    If Not lWREGInputs.Stations.Keys.Contains(lStationId) Then
                        Dim lNewWREGStation As New WREGStation
                        With lNewWREGStation
                            .StationID = lStationId
                            .Latitude = lLatitude
                            .Longitude = lLongitude
                            .StatFlowCharNdayFreq = New atcCollection
                            .StatFlowCharNdayFreq.Add(lStat, lValueString)
                            .TsAnnual = New atcData.atcTimeseriesGroup
                            lDefVal = lDS.Attributes.GetDefinedValue(lStat)
                            lTsAnnual = lDefVal.Arguments.GetValue("NDayTimeseries")
                            .NumAnnualSeries = lTsAnnual.numValues
                            '.FreqZero = lTsAnnual.Attributes.GetValue("Count Zero")
                            .TsAnnual.Add(lStat, lTsAnnual)
                            lValueK = lTsAnnual.Attributes.GetValue(lStat & " K Value")
                            lValueG = lTsAnnual.Attributes.GetValue("Skew")
                            lValueS = lTsAnnual.Attributes.GetValue("Standard Deviation")
                            .StatLP3G = New atcCollection : .StatLP3G.Add(lStat, lValueG)
                            .StatLP3K = New atcCollection : .StatLP3K.Add(lStat, lValueK)
                            .StatLP3s = New atcCollection : .StatLP3s.Add(lStat, lValueS)
                        End With
                        lWREGInputs.Stations.Add(lStationId, lNewWREGStation)
                    Else
                        With lWREGInputs.Stations.ItemByKey(lStationId)
                            .StatFlowCharNdayFreq.Add(lStat, lValueString)
                            lDefVal = lDS.Attributes.GetDefinedValue(lStat)
                            lTsAnnual = lDefVal.Arguments.GetValue("NDayTimeseries")
                            '.FreqZero = lTsAnnual.Attributes.GetValue("Count Zero")
                            .TsAnnual.Add(lStat, lTsAnnual)
                            lValueK = lTsAnnual.Attributes.GetValue(lStat & " K Value")
                            lValueG = lTsAnnual.Attributes.GetValue("Skew")
                            lValueS = lTsAnnual.Attributes.GetValue("Standard Deviation")
                            .StatLP3G.Add(lStat, lValueG)
                            .StatLP3K.Add(lStat, lValueK)
                            .StatLP3s.Add(lStat, lValueS)
                        End With
                    End If
                Else
                    If lValueString <> "NaN" AndAlso lValueString <> "?" Then
                        lSW.WriteLine(lStationId & " is excluded due to incomplete set of Nday stats.")
                    Else
                        lSW.WriteLine(lStationId & ":" & lStat & " failed. Not enough years in record.")
                    End If
                    lCompleteStats = False
                End If
            Next 'lStat
        Next 'lDS

        'write WREG inputs
        If lWREGInputs.WriteInputs() Then
            Logger.Msg("WREG input files are created in " & lWREGInputDir, MsgBoxStyle.Information, "WREG Input Files Created Successfully")
        Else
            Logger.Msg("WREG Input files creation failed.", MsgBoxStyle.Exclamation, "WREG Input Files Creation Failed")
        End If

        'Clean up
        lSW.Flush() : lSW.Close() : lSW = Nothing
        If lWREGInputs IsNot Nothing Then
            lWREGInputs.Clear() : lWREGInputs = Nothing
        End If

        Logger.Dbg("WREGInputDone")
    End Sub
End Module
