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

Public Module GenSolar07
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled2007\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenSolar07:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNewDate() As Integer = {2006, 1, 1, 0, 0, 0}
        Dim lNewJDate As Double = Date2J(lNewDate)
        Dim lStationDBF As New atcTableDBF
        Dim lNewWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lts As atcTimeseries = Nothing
        Dim lSJD As Double
        Dim lEJD As Double
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lNSta As Integer = 0

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenSolar07: Opened Station Location file " & lFName)
        Else
            Logger.Dbg("GenSolar07: PROBLEM Opening Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenSolar07: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenSolar07: Opening data file - " & lFile)
            lNewWDMFile = New atcWDM.atcDataSourceWDM
            lNewWDMFile.Open(lFile)
            lts = Nothing
            'WDMFilled2007 has Solar on DSN 103
            lts = lNewWDMFile.DataSets.ItemByKey(103)
            If Not lts Is Nothing Then 'AndAlso lts.Attributes.GetValue("EJDay") <= lNewJDate Then
                'has existing computed temperature record
                lNewJDate = lts.Attributes.GetValue("EJDay")
                'has solar rad data that is not through 2006
                lStation = FilenameNoPath(lFile).Substring(0, 6)
                lts = Nothing
                For Each lDS As atcDataSet In lNewWDMFile.DataSets
                    lEJD = lDS.Attributes.GetValue("EJDay")
                    If lDS.Attributes.GetValue("Constituent") = "CLOU" AndAlso lEJD > lNewJDate + 1 Then
                        'create daily cloud record for new time period
                        lts = Aggregate(SubsetByDate(lDS, lNewJDate, lEJD, Nothing), atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        Exit For
                    End If
                Next
                If Not lts Is Nothing Then
                    Logger.Dbg("GenSolar07:  Generated Daily Cloud Cover from " & DumpDate(lNewJDate) & " to " & DumpDate(lEJD))
                    If lStationDBF.FindFirst(1, lStation) Then 'get Lat from Station location DBF
                        Dim lLat As Double = lStationDBF.Value(4)
                        If lLat > 50 Then 'can't use larger than latitude 51
                            Logger.Dbg("GenSolar07:  NOTE - latitude (" & lLat & ") is greater than allowable for solar generation, use Lat=50.")
                            lLat = 50
                        ElseIf lLat < 25 Then 'can't use lower than latitude 25
                            Logger.Dbg("GenSolar07:  NOTE - latitude (" & lLat & ") is smaller than allowable for solar generation, use Lat=25.")
                            lLat = 25
                        End If
                        Dim ltsDSol As atcTimeseries = CmpSol(lts, Nothing, lLat)
                        ltsDSol.Attributes.SetValue("ID", 102)
                        If lNewWDMFile.AddDataset(ltsDSol, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("GenSolar07:  Appended DSOL to DSN 102")
                            lts = Nothing
                            lts = DisSolPet(ltsDSol, Nothing, 1, lLat)
                            lts.Attributes.SetValue("ID", 103)
                            Logger.Dbg("GenSolar07:  Apending SOLR from " & DumpDate(lNewJDate) & " to " & DumpDate(lEJD))
                            If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("GenSolar07:  Appended SOLR to DSN 5")
                                lNSta += 1
                            Else
                                Logger.Dbg("GenSolar07:  PROBLEM - Could not Append SOLR to DSN 103")
                            End If
                        Else
                            Logger.Dbg("GenSolar07:  PROBLEM - Could not AppendbDSOL to DSN 102")
                        End If
                        ltsDSol = Nothing
                    Else
                        Logger.Dbg("GenSolar07: PROBLEM - could not find Station on Location DBF file")
                    End If
                    lts = Nothing
                Else
                    Logger.Dbg("GenSolar07: Station does not contain CLOU dataset")
                End If
                lNewWDMFile.DataSets.Clear()
                lNewWDMFile = Nothing
            End If
        Next

        Logger.Dbg("GenSolar07: Completed Solar Radiation generation - " & lNSta & " datasets generated.")

        'Application.Exit()

    End Sub

End Module
