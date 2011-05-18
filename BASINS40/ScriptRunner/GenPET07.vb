Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinUtility.Strings
'Imports BASINS

Imports atcUtility
Imports atcData
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module GenPET07
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled2007\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenPET07:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewDate() As Integer = {2007, 1, 1, 0, 0, 0}
        Dim lNewJDate As Double = Date2J(lNewDate)
        Dim lStationDBF As New atcTableDBF
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lts As New atcTimeseries(Nothing)
        Dim ltsATmp As atcTimeseries
        Dim lGenCnt As Integer = 0
        Dim lCons As String = ""
        Dim lGo As Boolean = False

        Dim lCTS() As Double = {0, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenPET07: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenPET07: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenPET07: Opening data file - " & lFile)
            lNewWDMFile = New atcWDM.atcDataSourceWDM
            lNewWDMFile.Open(lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            lts = Nothing
            'computed EVAP is on DSN 106 
            lts = lNewWDMFile.DataSets.ItemByKey(106)
            If Not lts Is Nothing Then 'AndAlso lts.Attributes.GetValue("EJDay") <= lNewJDate Then
                'has existing computed temperature record
                lNewJDate = lts.Attributes.GetValue("EJDay")
                'has PEVT data that is not past end of 2006
                lStation = FilenameNoPath(lFile).Substring(0, 6)
                If lStationDBF.FindFirst(1, lStation) Then 'found station ID
                    Dim lLat As Double = lStationDBF.Value(4)
                    For Each lDS As atcDataSet In lNewWDMFile.DataSets
                        lCons = lDS.Attributes.GetValue("Constituent")
                        If (lCons = "ATEM" Or lCons = "ATEMP") AndAlso _
                           lDS.Attributes.GetValue("EJDay") > lNewJDate + 1 Then
                            ltsATmp = SubsetByDate(lDS, lNewJDate, lDS.Attributes.GetValue("EJDay"), Nothing)
                            lts = Nothing
                            lts = DisSolPet(PanEvaporationTimeseriesComputedByHamonX(ltsATmp, Nothing, True, lLat, lCTS), Nothing, 2, lLat)
                            lts.Attributes.SetValue("ID", 106)
                            If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("GenPET07: Appended Hourly PEVT to DSN 106")
                                lGenCnt += 1
                            Else
                                Logger.Dbg("GenPET07: PROBLEM - Appending Hourly PET to DSN 106")
                            End If
                            lts = Nothing
                            Exit For
                        End If
                    Next
                    lNewWDMFile.DataSets.Clear()
                    lNewWDMFile = Nothing
                Else
                    Logger.Dbg("GenPET07: PROBLEM - station not found in Station Location DBF file")
                End If
            Else
                Logger.Dbg("GenPET07:  No PEVT needing update at this station")
            End If
        Next

        Logger.Dbg("GenPET07: Completed PET generation - " & lGenCnt & " datasets generated.")

        'Application.Exit()

    End Sub

End Module
