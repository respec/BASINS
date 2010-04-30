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
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module GenPET
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("QACheck:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lts As New atcTimeseries(Nothing)
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lGenCnt As Integer = 0

        Dim lCTS() As Double = {0, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenPET: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenPET: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenPET: Opening data file - " & lFile)
            lStation = FilenameOnly(lFile)
            'lStationDBF.CurrentRecord = 1
            If lStationDBF.FindFirst(1, lStation) Then 'found station ID
                FileCopy(lFile, lCurWDM)
                lNewWDMFile = New atcWDM.atcDataSourceWDM
                lNewWDMFile.Open(lCurWDM)
                Dim lLat As Double = lStationDBF.Value(4)
                For Each lDS As atcDataSet In lNewWDMFile.DataSets
                    If CStr(lDS.Attributes.GetValue("Constituent")).StartsWith("ATEM") Then
                        lts = DisSolPet(CmpHamX(lDS, Nothing, True, lLat, lCTS), Nothing, 2, lLat)
                        lts.Attributes.SetValue("ID", 106)
                        lts.Attributes.SetValue("Location", lStation)
                        'Latitude is put on dataset within Hamon Computation module,
                        'be sure Longitude is put on dataset also
                        lts.Attributes.SetValue("LngDeg", lStationDBF.Value(5))
                        If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace) Then
                            Logger.Dbg("GenPET: Wrote Hourly PET to DSN 106")
                            lGenCnt += 1
                            'delete existing file and copy current working WDM to it
                            Kill(lFile)
                            FileCopy(lCurWDM, lFile)
                        Else
                            Logger.Dbg("GenPET: PROBLEM - Writing Hourly PET to DSN 106")
                        End If
                        lts = Nothing
                        Exit For
                    End If
                Next
                Kill(lCurWDM)
                lNewWDMFile.DataSets.Clear()
                lNewWDMFile = Nothing
            Else
                Logger.Dbg("GenPET: PROBLEM - station not found in Station Location DBF file")
            End If
        Next

        Logger.Dbg("GenPET: Completed PET generation - " & lGenCnt & " datasets generated.")

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
