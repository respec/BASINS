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

Public Module GenCloud
    Private Const pInputPath As String = "F:\BASINSMet\WDMFiltered\Ishshifted\Subset\"
    Private Const pISHWDMPath As String = "F:\BASINSMet\WDMFiltered\IshShifted\"
    Private Const pSODWDMPath As String = "F:\BASINSMet\WDMFiltered\"
    Private Const pBasinsWDMPath As String = "F:\BASINSMet\Basins31WDMs\"
    Private Const pOutputPath As String = "F:\BASINSMet\WDMCompleted\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenCloud:Start")
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
            Logger.Dbg("GenCloud: Opened Station Location file F:\BASINSMet\WDMRaw\StationLocs.dbf")
        End If
        If lMatchingSta.OpenFile("F:\BASINSMet\MatchingSOD-ISH.dbf") Then
            Logger.Dbg("GenCloud: Opened Station Location file F:\BASINSMet\MatchingSOD-ISH.dbf")
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pInputPath, False)
        Logger.Dbg("GenCloud: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenCloud: Opening data file - " & lFile)
            lISHWDMFile = New atcWDM.atcDataSourceWDM
            lISHWDMFile.Open(lFile)
            lStation = FilenameOnly(lFile)
            If lStationDBF.FindFirst(1, lStation) Then 'found station ID, get it's State name
                lState = lStationDBF.Value(3)
                If lMatchingSta.FindFirst(1, lStation) Then 'found corresponding SOD Station ID
                    lStation = lMatchingSta.Value(8)
                    lNewWDMFile = New atcWDM.atcDataSourceWDM
                    If lNewWDMFile.Open(pOutputPath & lStation & ".wdm") Then
                        Logger.Dbg("GenCloud: Opened Completed WDM file - " & pOutputPath & lStation & ".wdm")
                        'now open BASINS 3.1 WDM if possible
                        lBasinsWDMFile = New atcWDM.atcDataSourceWDM
                        If lBasinsWDMFile.Open(pBasinsWDMPath & lState & ".wdm") Then
                            Logger.Dbg("GenCloud: Opened Basins 3.1 WDM file - " & pBasinsWDMPath & lState & ".wdm")
                            Dim lSta As String = lStation.Substring(2)
                            For Each lDS As atcDataSet In lBasinsWDMFile.DataSets
                                If lDS.Attributes.GetValue("Constituent") = "CLOU" AndAlso _
                                   lDS.Attributes.GetValue("Location").ToString.EndsWith(lSta) Then
                                    CombineData(lDS, lISHWDMFile.DataSets, lNewWDMFile, "ATEMP")
                                End If
                            Next
                        Else
                            Logger.Dbg("AppendHourly: PROBLEM - Could not open Basins 3.1 WDM file - " & pBasinsWDMPath & lState & ".wdm")
                        End If
                    Else
                        Logger.Dbg("GenCloud: PROBLEM - Could not open Completed WDM file - " & pOutputPath & lStation & ".wdm")
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
        If aWDMFile.AddDataSet(lts) Then
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
                        If aWDMFile.AddDataSet(lts, atcDataSource.EnumExistAction.ExistAppend) Then
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

    Private Function SkyCondOktas2CloudTenths(ByRef aHrlySky As atcTimeseries) As atcTimeseries
        'convert ISH hourly Sky Condition timeseries (recorded in Oktas)
        'to Cloud Cover timeseries in tenths

        Dim lts As atcTimeseries = aHrlySky.Clone
        Dim lVal As Double = 0

        For i As Integer = 1 To lts.numValues
            lVal = lts.Value(i)
            If lVal >= 0 AndAlso lVal <= 10 Then
                Select Case lVal
                    Case 0, 1 'same as recorded okta value
                        lts.Value(i) = lVal
                    Case 2 '2/10 - 3/10
                        lts.Value(i) = 2.5
                    Case 3, 4, 5 '1/10 higher than recorded okta value
                        lts.Value(i) = lVal + 1
                    Case 6 '7/10 - 8/10
                        lts.Value(i) = 7.5
                    Case 7, 8 '2/10 higher than recorded okta value
                        lts.Value(i) = lVal + 2
                    Case 9, 10 'assume fully covered
                        lts.Value(i) = 10
                End Select
            End If
        Next
        Return lts
    End Function

End Module
