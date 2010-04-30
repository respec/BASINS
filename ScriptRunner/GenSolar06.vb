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

Public Module GenSolar06
    Private Const pInputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenSolar06:Start")
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
        Dim lStatePath As String
        Dim lNSta As Integer = 0

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenSolar06: Opened Station Location file " & lFName)
        Else
            Logger.Dbg("GenSolar06: PROBLEM Opening Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenSolar06: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            Logger.Dbg("GenSolar06: Opening data file - " & lFile)
            lNewWDMFile = New atcWDM.atcDataSourceWDM
            lNewWDMFile.Open(lFile)
            lts = Nothing
            lts = lNewWDMFile.DataSets.ItemByKey(5)
            If Not lts Is Nothing AndAlso lts.Attributes.GetValue("EJDay") <= lNewJDate Then
                'has solar rad data that is not through 2006
                lStation = FilenameOnly(lFile).Substring(2, 6)
                lStatePath = FilenameOnly(lStation).Substring(0, 2)
                lts = Nothing
                For Each lDS As atcDataSet In lNewWDMFile.DataSets
                    lEJD = lDS.Attributes.GetValue("EJDay")
                    If lDS.Attributes.GetValue("Constituent") = "CLOU" AndAlso lEJD > lNewJDate Then
                        lts = Aggregate(SubsetByDate(lDS, lNewJDate, lEJD, Nothing), atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        Exit For
                    End If
                Next
                If Not lts Is Nothing Then
                    Logger.Dbg("GenSolar06:  Generated Daily Cloud Cover from " & DumpDate(lNewJDate) & " to " & DumpDate(lEJD))
                    If lStationDBF.FindFirst(1, lStation) Then 'get Lat from Station location DBF
                        Dim lLat As Double = lStationDBF.Value(4)
                        If lLat > 50 Then 'can't use larger than latitude 51
                            Logger.Dbg("GenSolar06:  NOTE - latitude (" & lLat & ") is greater than allowable for solar generation, use Lat=50.")
                            lLat = 50
                        ElseIf lLat < 25 Then 'can't use lower than latitude 25
                            Logger.Dbg("GenSolar06:  NOTE - latitude (" & lLat & ") is smaller than allowable for solar generation, use Lat=25.")
                            lLat = 25
                        End If
                        Dim ltsDSol As atcTimeseries = CmpSol(lts, Nothing, lLat)
                        ltsDSol.Attributes.SetValue("ID", 1102)
                        If lNewWDMFile.AddDataset(ltsDSol, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("GenSolar06:  Appended DSOL to DSN 1102")
                            lts = Nothing
                            lts = DisSolPet(ltsDSol, Nothing, 1, lLat)
                            lts.Attributes.SetValue("ID", 5)
                            Logger.Dbg("GenSolar06:  Apending SOLR from " & DumpDate(lNewJDate) & " to " & DumpDate(lEJD))
                            If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistAppend) Then
                                Logger.Dbg("GenSolar06:  Appended SOLR to DSN 5")
                                lNSta += 1
                            Else
                                Logger.Dbg("GenSolar06:  PROBLEM - Could not Append SOLR to DSN 5")
                            End If
                        Else
                            Logger.Dbg("GenSolar06:  PROBLEM - Could not AppendbDSOL to DSN 1102")
                        End If
                        ltsDSol = Nothing
                    Else
                        Logger.Dbg("GenSolar06: PROBLEM - could not find Station on Location DBF file")
                    End If
                    lts = Nothing
                Else
                    Logger.Dbg("GenSolar06: Station does not contain CLOU dataset")
                End If
                lNewWDMFile.DataSets.Clear()
                lNewWDMFile = Nothing
            End If
        Next

        Logger.Dbg("GenSolar06: Completed Solar Radiation generation - " & lNSta & " datasets generated.")

        'Application.Exit()

    End Sub

End Module
