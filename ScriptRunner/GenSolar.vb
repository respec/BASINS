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

Public Module GenSolar
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenSolar:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lISHWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lStation As String = ""
        Dim lts As atcTimeseries = Nothing
        Dim lSJD As Double
        Dim lEJD As Double
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lStatePath As String
        Dim lNSta As Integer = 0

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenSolar: Opened Station Location file " & lFName)
        Else
            Logger.Dbg("GenSolar: PROBLEM Opening Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenSolar: Found " & lFiles.Count & " WDM data files")
        For Each lFile As String In lFiles
            lStatePath = Right(PathNameOnly(lfile), 2)
            If Not IsNumeric(lStatePath) Then
                Logger.Dbg("GenSolar: Opening data file - " & lFile)
                FileCopy(lFile, lCurWDM)
                lISHWDMFile = New atcWDM.atcDataSourceWDM
                lISHWDMFile.Open(lCurWDM)
                For Each lDS As atcDataSet In lISHWDMFile.DataSets
                    If lDS.Attributes.GetValue("Constituent") = "CLOU" Then 'generate daily cloud cover TSER
                        lSJD = lDS.Attributes.GetValue("SJDay")
                        lEJD = lDS.Attributes.GetValue("EJDay")
                        If lSJD - Fix(lSJD) > JulianSecond Then 'get on whole day boundaries
                            'then subset by date on those boundaries for aggregating to daily
                            lSJD = Fix(lSJD) + 1
                            If lEJD - Fix(lEJD) > JulianSecond Then
                                lEJD = Fix(lEJD)
                            End If
                            lts = Aggregate(SubsetByDate(lDS, lSJD, lEJD, Nothing), atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        Else 'already on whole day boundary
                            lts = Aggregate(lDS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        End If
                        Exit For
                    End If
                Next
                If Not lts Is Nothing Then
                    Logger.Dbg("GenSolar:  Generated Daily Cloud Cover from " & dumpdate(lSJD) & " to " & dumpdate(lEJD))
                    lStation = FilenameOnly(lFile)
                    If lStationDBF.FindFirst(1, lStation) Then 'get Lat from Station location DBF
                        Dim lLat As Double = lStationDBF.Value(4)
                        If lLat > 50 Then 'can't use larger than latitude 51
                            Logger.Dbg("GenSolar:  NOTE - latitude (" & lLat & ") is greater than allowable for solar generation, use Lat=50.")
                            lLat = 50
                        ElseIf lLat < 25 Then 'can't use lower than latitude 25
                            Logger.Dbg("GenSolar:  NOTE - latitude (" & lLat & ") is smaller than allowable for solar generation, use Lat=25.")
                            lLat = 25
                        End If
                        Dim ltsDSol As atcTimeseries = CmpSol(lts, Nothing, lLat)
                        ltsDSol.Attributes.SetValue("ID", 102)
                        If lISHWDMFile.AddDataset(ltsDSol) Then
                            Logger.Dbg("GenSolar:  Wrote DSOL to DSN 102")
                            lts = Nothing
                            lts = DisSolPet(ltsDSol, Nothing, 1, lLat)
                            lts.Attributes.SetValue("ID", 103)
                            If lISHWDMFile.AddDataset(lts) Then
                                Logger.Dbg("GenSolar:  Wrote SOLR to DSN 103")
                                lNSta += 1
                            Else
                                Logger.Dbg("GenSolar:  PROBLEM - Could not write SOLR to DSN 103")
                            End If
                        Else
                            Logger.Dbg("GenSolar:  PROBLEM - Could not write DSOL to DSN 102")
                        End If
                        ltsDSol = Nothing
                    Else
                        Logger.Dbg("GenSolar: PROBLEM - could not find Station on Location DBF file")
                    End If
                    lts = Nothing
                Else
                    Logger.Dbg("GenSolar: Station does not contain CLOU dataset")
                End If
                lISHWDMFile.DataSets.Clear()
                FileCopy(lCurWDM, lFile)
                Kill(lCurWDM)
                lISHWDMFile = Nothing
            End If
        Next

        Logger.Dbg("GenSolar: Completed Solar Radiation generation - " & lNSta & " datasets generated.")

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
                    Case Else
                        lts.Value(i) = lVal
                End Select
            End If
        Next
        Return lts
    End Function

End Module
