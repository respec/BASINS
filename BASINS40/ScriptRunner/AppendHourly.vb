Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData
'Imports atcDataTree
'Imports atcEvents

Public Module AppendHourly
    Private Const pInputPath As String = "F:\BASINSMet\WDMFiltered\Subset\"
    Private Const pISHWDMPath As String = "F:\BASINSMet\WDMFiltered\IshShifted\"
    Private Const pSODWDMPath As String = "F:\BASINSMet\WDMFiltered\"
    Private Const pBasinsWDMPath As String = "F:\BASINSMet\Basins31WDMs\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMCompleted\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("AppendHourly:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lMatchingSta As New atcTableDBF
        Dim lISHWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lSODWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lBasinsWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lState As String = ""
        Dim lts As New atcTimeseries(Nothing)

        If lStationDBF.OpenFile("F:\BASINSMet\WDMRaw\StationLocs.dbf") Then
            Logger.Dbg("AppendHourly: Opened Station Location file F:\BASINSMet\WDMRaw\StationLocs.dbf")
        End If
        If lMatchingSta.OpenFile("F:\BASINSMet\MatchingSOD-ISH.dbf") Then
            Logger.Dbg("AppendHourly: Opened Station Location file F:\BASINSMet\MatchingSOD-ISH.dbf")
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("AppendHourly: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("AppendHourly: Opening data file - " & lFile)
            lSODWDMFile = New atcWDM.atcDataSourceWDM
            lSODWDMFile.Open(lFile)
            lStation = FilenameOnly(lFile)
            lStationDBF.CurrentRecord = 1
            If lStationDBF.FindFirst(1, lStation) Then 'found station ID, get it's State name
                lState = lStationDBF.Value(3)
                lMatchingSta.CurrentRecord = 1
                If lMatchingSta.FindFirst(8, lStation) Then 'open corresponding ISH WDM file
                    lISHWDMFile = New atcWDM.atcDataSourceWDM
                    If lISHWDMFile.Open(pISHWDMPath & lState & "\" & lMatchingSta.Value(1) & ".wdm") Then
                        Logger.Dbg("AppendHourly: Opened ISH WDM file - " & pISHWDMPath & lState & "\" & lMatchingSta.Value(1) & ".wdm")
                        lNewWDMFile = New atcWDM.atcDataSourceWDM
                        lNewWDMFile.Open(pOutputPath & lStation & ".wdm")
                        For Each lDS As atcDataSet In lSODWDMFile.DataSets
                            If lDS.Attributes.GetValue("Constituent") = "HPCP" Then
                                CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "HPCP1")
                                Exit For
                            End If
                        Next
                        'now open BASINS 3.1 WDM if possible
                        lBasinsWDMFile = New atcWDM.atcDataSourceWDM
                        If lBasinsWDMFile.Open(pBasinsWDMPath & lState & ".wdm") Then
                            Logger.Dbg("AppendHourly: Opened Basins 3.1 WDM file - " & pBasinsWDMPath & lState & ".wdm")
                            Dim lSta As String = lStation.Substring(2)
                            For Each lDS As atcDataSet In lBasinsWDMFile.DataSets
                                If lDS.Attributes.GetValue("Constituent") = "ATEM" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "ATEMP")
                                ElseIf lDS.Attributes.GetValue("Constituent") = "WIND" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "WIND")
                                ElseIf lDS.Attributes.GetValue("Constituent") = "DEWP" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "DPTEMP")
                                ElseIf lDS.Attributes.GetValue("Constituent") = "CLOU" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "CLOU")
                                ElseIf lDS.Attributes.GetValue("Constituent") = "SOLR" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "SOLR")
                                End If
                            Next
                        Else
                            Logger.Dbg("AppendHourly: PROBLEM - Could not open Basins 3.1 WDM file - " & pBasinsWDMPath & lState & ".wdm")
                        End If
                    Else
                        Logger.Dbg("AppendHourly: PROBLEM - Could not open ISH WDM file - " & pISHWDMPath & lState & "\" & lMatchingSta.Value(1))
                    End If
                Else
                    Logger.Dbg("AppendHourly: No corresponding ISH Station found")
                End If
            Else
                Logger.Dbg("AppendHourly: PROBLEM - station not found in Station Location DBF file")
            End If
        Next

        Logger.Dbg("Append Hourly: Completed Hourly data appending")

        'Application.Exit()

    End Sub

    Private Sub CombineData(ByRef aTS1 As atcTimeseries, ByRef aDatasets As atcDataGroup, ByRef aWDMFile As atcWDM.atcDataSourceWDM, ByVal aCons As String)
        'Writes timeseries from two sources to one new and updated dataset on a WDM file
        'aTS1 - initial timeseries to write to new WDM file
        'aDatasets - datasets containing timeseries to append to aTS1
        'aWDMFile - WDM file to which timeseries are saved
        'aStation - station ID 
        'aCons - constituent name in aDatasets' timeseries for which data are to be appended

        Dim lts As atcTimeseries = Nothing
        Dim lID As String = Nothing
        Dim lDate As Double
        lts = aTS1.Clone
        If lts.Attributes.GetValue("Constituent") = "HPCP" Then 'special case for precip, always 1
            lID = 1
        Else 'use Basins 3.1 constituent index (1-8) for DSN
            lID = Right(lts.Attributes.GetValue("ID").ToString, 1)
        End If
        lts.Attributes.SetValue("ID", lID)
        If aWDMFile.AddDataset(lts) Then
            lDate = lts.Attributes.GetValue("EJDay")
            Logger.Dbg("AppendHourly:CombineData:  Wrote initial TS for " & lts.Attributes.GetValue("Constituent") & _
                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lDate)
            lts = Nothing
            For Each lDS As atcDataSet In aDatasets
                If lDS.Attributes.GetValue("Constituent") = aCons Then
                    If lDS.Attributes.GetValue("SJDay") < lDate AndAlso _
                       lDS.Attributes.GetValue("EJDay") > lDate Then
                        lts = SubsetByDate(lDS, lDate, lDS.Attributes.GetValue("EJDay"), Nothing)
                        lts.Attributes.SetValue("ID", lID)
                        If aWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("AppendHourly:CombineData:  Appended TS for " & lts.Attributes.GetValue("Constituent") & _
                                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lts.Attributes.GetValue("EJDay"))
                        Else
                            Logger.Dbg("AppendHourly:CombineData:  PROBLEM Appending TS for " & lts.Attributes.GetValue("Constituent") & _
                                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lts.Attributes.GetValue("EJDay"))
                        End If
                        lts = Nothing
                    End If
                    Exit For
                End If
            Next
        Else
            Logger.Dbg("AppendHourly:CombineData:  PROBLEM writing initial TS for " & lts.Attributes.GetValue("Constituent") & _
                       " to DSN " & lID & " - Start: " & lts.Attributes.GetValue("SJDay") & " End: " & lDate)
        End If

    End Sub

End Module
