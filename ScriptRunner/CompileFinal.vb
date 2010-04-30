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

Public Module CompileFinal
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pB31WDMPath As String = "C:\BASINSMet\Basins31WDMs\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pAlreadyDone As String = "" '01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,50,51" ',66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("CompileFinal:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""
        Dim lRec As Integer = 0
        Dim i As Integer
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String

        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lDBF As atcTableDBF = Nothing
        Dim lStation As String
        Dim lStatePath As String = ""
        Dim lNewLocn As String = ""
        Dim lCurPath As String = ""
        Dim lCurrEDate() As Integer = {2005, 12, 31, 0, 0, 0}
        Dim lCurrEnd As Double = Date2J(lCurrEDate)
        Dim lB31SDate() As Integer = {1970, 1, 1, 0, 0, 0}
        Dim lB31Start As Double = Date2J(lB31SDate)
        Dim lB31EDate() As Integer = {1995, 12, 31, 24, 0, 0}
        Dim lB31End As Double = Date2J(lB31EDate)
        Dim AlreadySaved As New atcCollection
        Dim lStr As String = ""
        Dim lQAStr As String = "Cons" & vbTab & "NVal" & vbTab & "B31 Loc" & vbTab & "B31 Val" & vbTab & _
                               "New Loc" & vbTab & "New Val" & vbTab & "% Diff" & vbCrLf

        Dim lStates() As String = {"AL", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "ID", _
                                   "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", _
                                   "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", _
                                   "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", _
                                   "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "AK", "HI", "PR", "VI"}
        Dim lState As String = ""
        Dim lStIds As New atcCollection
        i = 1
        For Each lState In lStates
            If i < 10 Then
                lStIds.Add(lState, "0" & CStr(i))
            Else
                lStIds.Add(lState, CStr(i))
                If i = 48 Then i += 1 'skip unused 49
                If i = 51 Then i = 65 'skip to Puerto Rico (66)
            End If
            i += 1
        Next

        Dim lStationDBF As New atcTableDBF
        If lStationDBF.OpenFile(pStationPath & "StationLocs.dbf") Then
            Logger.Dbg("CompileFinal: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        Else
            Logger.Dbg("CompileFinal: PROBLEM Opening Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If
        Dim lB31DBF As New atcTableDBF
        If lB31DBF.OpenFile(pStationPath & "BASINS31Map.dbf") Then
            Logger.Dbg("CompileFinal: Opened BASINS 31 Station Map to ISH file " & pStationPath & "BASINS31Map.dbf")
        Else
            Logger.Dbg("CompileFinal: PROBLEM Opening BASINS 31 Station Map to ISH file " & pStationPath & "BASINS31Map.dbf")
        End If
        Dim lMatchISHSOD As New atcTableDBF
        If lMatchISHSOD.OpenFile(pStationPath & "Matching_ISH+SOD.dbf") Then
            Logger.Dbg("CompileFinal: Opened Matching ISH + SOD Station file " & pStationPath & "Matching_ISH+SOD.dbf")
        Else
            Logger.Dbg("CompileFinal: PROBLEM Opening Matching ISH + SOD Station file " & pStationPath & "Matching_ISH+SOD.dbf")
        End If

        Dim lB31IDs As New atcCollection
        For i = 1 To lB31DBF.NumRecords
            lB31DBF.CurrentRecord = i
            lState = lStIds.ItemByKey(lB31DBF.Value(2))
            If lState.Length > 0 Then
                lB31IDs.Add(lB31DBF.CurrentRecord, lState & Format(CInt(lB31DBF.Value(3)), "0000"))
            Else
                Logger.Dbg("CompileFinal:  PROBLEM - can't find state ID for " & lState)
            End If
        Next i

        Logger.Dbg("CompileFinal: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("CompileFinal: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("CompileFinal: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lStation = FilenameNoExt(FilenameNoPath(lFile))
                If IsNumeric(lStatePath) Then
                    lState = lStIds.Keys(lStIds.IndexOf(lStatePath))
                Else 'just use state path for start of new location
                    lState = lStatePath
                End If
                lNewLocn = lState.ToUpper & lStation
                Logger.Dbg("CompileFinal: For station " & lStation)
                If lStationDBF.FindFirst(1, lStation) Then
                    lLat = lStationDBF.Value(4)
                    lLng = lStationDBF.Value(5)
                    lStaName = lStationDBF.Value(2)
                    Dim lNewWDMFile As New atcWDM.atcDataSourceWDM
                    lNewWDMFile.Open(lNewWDM)
                    lRec = lB31IDs.IndexOf(lStation) + 1
                    If lRec > 0 Then 'it's a BASINS 3.1 station
                        'only pass last 4 characters of 6-character ID
                        WriteB31(pB31WDMPath & lState & ".wdm", lStation.Substring(2), lNewWDMFile)
                        lB31DBF.CurrentRecord = lRec
                        Dim lWroteHPCP As Boolean = False
                        If lWDMfile.DataSets.Keys.Contains(100) Then 'hourly precip exists
                            lts = lWDMfile.DataSets.ItemByKey(100).Clone
                            If lts.Attributes.GetValue("JSDay") <= lB31Start AndAlso _
                               lts.Attributes.GetValue("EJDay") >= lB31End Then
                                'data spans Basins 3.1 period 
                                If lts.Attributes.GetValue("EJDay") < lCurrEnd Then 'data are not current, NOTE it
                                    Logger.Dbg("CompileFinal:  NOTE - Hourly Precip for BASINS 3.1 station (" & lStation & ") not current; ends " & DumpDate(lts.Attributes.GetValue("EJDay")).Substring(14, 16))
                                End If
                                'do some QA first
                                Dim ltSub As atcTimeseries
                                ltSub = SubsetByDate(lts, lB31Start, lCurrEnd, Nothing)
                                Dim lSubSum As Double = ltSub.Attributes.GetValue("Sum")
                                Dim lB31SubTs As atcTimeseries
                                lB31SubTs = SubsetByDate(lNewWDMFile.DataSets.ItemByKey(2001), lB31Start, lCurrEnd, Nothing)
                                Dim lB31Sum As Double = lB31SubTs.Attributes.GetValue("Sum")
                                If lB31Sum > 0 AndAlso lSubSum > 0 Then
                                    Dim lDiff As Double = System.Math.Abs(((lB31Sum - lSubSum) / (lB31Sum)) * 100)
                                    If lDiff < 20 Then
                                        Logger.Dbg("CompileFinal: QACHECK - For PREC from " & DumpDate(lB31Start).Substring(14, 16) & " to " & DumpDate(lCurrEnd).Substring(14, 16) & _
                                                   " % diff between sum for B31 (" & DoubleToString(lB31Sum, 10, , , , 4) & ") and new data (" & DoubleToString(lSubSum, 10, , , , 4) & ") is <20 (" & lDiff & ")")
                                    Else
                                        Logger.Dbg("CompileFinal: QACHECK - For PREC from " & DumpDate(lB31Start).Substring(14, 16) & " to " & DumpDate(lCurrEnd).Substring(14, 16) & _
                                                   " % diff between sum for B31 (" & DoubleToString(lB31Sum, 10, , , , 4) & ") and new data (" & DoubleToString(lSubSum, 10, , , , 4) & ") is >20 (" & lDiff & ")")
                                        lQAStr = "PREC" & vbTab & ltSub.numValues & vbTab & lB31SubTs.Attributes.GetValue("Location") & vbTab & DoubleToString(lB31Sum, 10, , , , 4) & vbTab & _
                                                 ltSub.Attributes.GetValue("Location") & vbTab & DoubleToString(lSubSum, 10, , , , 4) & vbTab & DoubleToString(lDiff, 8, , , , 4) & vbCrLf
                                    End If
                                Else
                                    Logger.Dbg("CompileFinal: QACHECK - NOTE: For PREC from " & DumpDate(lB31Start).Substring(14, 16) & " to " & DumpDate(lCurrEnd).Substring(14, 16) & _
                                               " B31 (" & DoubleToString(lB31Sum, 10, , , , 4) & ") or new data (" & DoubleToString(lSubSum, 10, , , , 4) & ") sum is 0")
                                End If
                                ltSub = Nothing
                                lB31SubTs = Nothing
                                lts.EnsureValuesRead()
                                'now write hourly precip dataset to WDM file
                                lts.Attributes.SetValue("ID", 1)
                                'match Basins 3.1 location format
                                lts.Attributes.SetValue("Location", lNewLocn)
                                'use consistent lat/lng and station name values from station location file
                                lts.Attributes.SetValue("LatDeg", lLat)
                                lts.Attributes.SetValue("LngDeg", lLng)
                                lts.Attributes.SetValue("STANAM", lStaName)
                                lts.Attributes.SetValue("Constituent", "PREC")
                                lts.Attributes.SetValue("TSTYPE", "PREC")
                                If lNewWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace) Then
                                    Logger.Dbg("CompileFinal: Wrote " & lts.ToString & " to WDM file")
                                    lWroteHPCP = True
                                Else
                                    Logger.Dbg("CompileFinal: PROBLEM writing " & lts.ToString & " to WDM file")
                                End If
                            Else 'data does not span existing B31 data
                                Logger.Dbg("CompileFinal:  PROBLEM - Hourly Precip for BASINS 3.1 station (" & lStation & ") not current; ends " & dumpdate(lts.Attributes.GetValue("EJDay")).Substring(14, 16))
                            End If
                        End If
                        For Each lts In lWDMfile.DataSets
                            lts.EnsureValuesRead()
                            lts.Attributes.SetValue("Location", lNewLocn)
                            'use consistent lat/lng and station name values from station location file
                            lts.Attributes.SetValue("LatDeg", lLat)
                            lts.Attributes.SetValue("LngDeg", lLng)
                            lts.Attributes.SetValue("STANAM", lStaName)
                            If lB31DBF.Value(6).Length = 0 AndAlso _
                               lts.Attributes.GetValue("SJDay") <= lB31End AndAlso _
                               lts.Attributes.GetValue("EJDay") >= lCurrEnd AndAlso _
                               (lts.Attributes.GetValue("Constituent") <> "HPCP" Or Not lWroteHPCP) Then
                                'not a match with an ISH station, merge all local current data possible
                                MergeTSers(lNewWDMFile, lts, lStr)
                                If lStr.Length > 0 Then
                                    lQAStr &= lStr
                                End If
                            Else 'write local data as ancilliary data
                                'since this ISH station matches the coop station, 
                                'the ISH data will update the existing Basins 3.1 data below
                                Dim llts As atcTimeseries = lts.Clone
                                If llts.Attributes.GetValue("Constituent") <> "HPCP" OrElse Not lWroteHPCP Then
                                    llts.Attributes.SetValue("ID", lts.Attributes.GetValue("ID") + 1000)
                                    If lNewWDMFile.AddDataset(llts) Then
                                        Logger.Dbg("CompileFinal:   Wrote ancillary dataset " & llts.ToString & " to WDM file.")
                                    Else
                                        Logger.Dbg("CompileFinal:   PROBLEM writing ancillary dataset " & llts.ToString & " to WDM file.")
                                    End If
                                End If
                            End If
                            If lB31DBF.Value(6).Length > 0 AndAlso AlreadySaved.IndexOf(lB31DBF.Value(5)) < 0 Then
                                'since this ISH station matches the coop and all its data are saved, don't save it again
                                AlreadySaved.Add(lStation & " (" & lB31DBF.Value(1) & ")", lB31DBF.Value(5))
                            End If
                        Next
                        'now merge ISH data in to finish out constituents
                        Dim lISHFiles As NameValueCollection = Nothing
                        AddFilesInDir(lISHFiles, pInputPath, True, lB31DBF.Value(5) & ".wdm")
                        If lISHFiles.Count = 1 Then 'found the ISH WDM file
                            Dim lISHWDM As New atcWDM.atcDataSourceWDM
                            lISHWDM.Open(lISHFiles(0))
                            For Each lts In lISHWDM.DataSets
                                lts.EnsureValuesRead()
                                lts.Attributes.SetValue("Location", lNewLocn)
                                'use consistent lat/lng and station name values from station location file
                                lts.Attributes.SetValue("LatDeg", lLat)
                                lts.Attributes.SetValue("LngDeg", lLng)
                                lts.Attributes.SetValue("STANAM", lStaName)
                                MergeTSers(lNewWDMFile, lts, lStr)
                                If lStr.Length > 0 Then
                                    lQAStr &= lStr
                                End If
                            Next
                        ElseIf lISHFiles.Count = 0 Then
                            Logger.Dbg("CompileFinal:  PROBLEM locating ISH WDM file " & lB31DBF.Value(5) & ".wdm")
                        ElseIf lISHFiles.Count > 1 Then
                            Logger.Dbg("CompileFinal:  PROBLEM - found multiple ISH WDM files for " & lB31DBF.Value(5) & ".wdm")
                        End If
                    ElseIf AlreadySaved.IndexOf(lStation) >= 0 Then
                        Logger.Dbg("CompileFinal: Already saved station " & lStation & " (" & lStationDBF.Value(2) & ") on WDM file for station " & AlreadySaved.Keys(AlreadySaved.IndexOf(lStation)))
                    Else 'store all constituents in appropriate DSNs
                        Dim lMerged As Boolean = False
                        If IsNumeric(lStatePath) Then 'coop (SOD or HPD) data
                            If lStIds.IndexOf(lStatePath) >= 0 Then
                                lState = lStIds.Keys(lStIds.IndexOf(lStatePath))
                            ElseIf lStatePath = "66" Then
                                lState = "PR"
                            ElseIf lStatePath = "67" Then
                                lState = "VI"
                            Else
                                lState = ""
                            End If
                        Else 'ISH data
                            lState = lStatePath.ToUpper
                            If lMatchISHSOD.FindFirst(6, lStation) Then 'this station matches a coop station, try to merge them
                                Dim lCoopSta As String = pOutputPath & lState & lMatchISHSOD.Value(1) & ".wdm"
                                If FileExists(lCoopSta) Then
                                    'coop station exists, open it as WDM file to write to
                                    lNewWDMFile.DataSets.Clear()
                                    lNewWDMFile = Nothing
                                    lNewWDMFile = New atcWDM.atcDataSourceWDM
                                    lNewWDMFile.Open(lCoopSta)
                                    Logger.Dbg("CompileFinal:  Merging ISH station " & lStation & " (" & lMatchISHSOD.Value(8) & _
                                               ") with Coop station " & lMatchISHSOD.Value(1) & " (" & lMatchISHSOD.Value(3) & ")")
                                    'update location, station name, and lat/lng to use coop station values
                                    lNewLocn = lState & lMatchISHSOD.Value(1)
                                    lStaName = lNewWDMFile.DataSets(0).Attributes.GetValue("STANAM")
                                    lLat = lMatchISHSOD.Value(4)
                                    lLng = lMatchISHSOD.Value(5)
                                    lMerged = True
                                End If
                            End If
                        End If
                        WriteFinal(lWDMfile, lNewWDMFile, lStaName, lNewLocn, lLat, lLng)
                        If lMerged Then 'station data saved on existing WDM, don't write new WDM file
                            lNewWDMFile.DataSets.Clear()
                        End If
                    End If
                    If lNewWDMFile.DataSets.Count > 0 Then 'save final WDM file for station
                        FileCopy(lNewWDM, pOutputPath & lState & lStation & ".wdm")
                        Logger.Dbg("CompileFinal:  Wrote final WDM file for station " & lStation)
                        lNewWDMFile.DataSets.Clear()
                    Else
                        Logger.Dbg("CompileFinal:  No datasets saved in final WDM file for station " & lStation)
                    End If
                    lNewWDMFile = Nothing
                    Kill(lNewWDM)
                    lWDMfile.DataSets.Clear()
                    lWDMfile = Nothing
                Else
                    Logger.Dbg("CompileFinal: PROBLEM - couldn't find station on station file!")
                End If
            End If
        Next
        SaveFileString(pOutputPath & "QACheck.txt", lQAStr)
        Logger.Dbg("CompileFinal: Completed storing Final WDM files")

        'Application.Exit()

    End Sub

    'merge existing Basins 3.1 data with updated data
    'aWDMFile is output WDM file that already has Basins 3.1 stored on DSNs 2001 - 2008
    'aTSer is the updated data to merge with existing
    'if aTSer is not for a needed final constituent, store it as reference in the DSN 1000 group
    Private Sub MergeTSers(ByRef aWDMFile As atcWDM.atcDataSourceWDM, ByVal aTSer As atcTimeseries, ByRef aQAStr As String)
        Dim lTSer As atcTimeseries = aTSer.Clone
        Dim lCons As String = lTSer.Attributes.GetValue("Constituent")
        Dim lts As atcTimeseries
        Dim lB31TS As atcTimeseries
        'Basins 3.1 contains some 1996 data, but values are often suspect (many containing just 0.0)
        Dim lEDate() As Integer = {1995, 12, 31, 24, 0, 0} 'always end Basins 3.1 data in 1995
        Dim lEJD As Double = Date2J(lEDate)
        Dim lID As Integer

        aQAStr = ""

        Select Case lCons
            Case "HPCP", "HPCP1" : lts = aWDMFile.DataSets.ItemByKey(2001).Clone
            Case "EVAP" : lts = aWDMFile.DataSets.ItemByKey(2006).Clone
            Case "ATEM", "ATEMP" : lts = aWDMFile.DataSets.ItemByKey(2003).Clone
            Case "WIND" : lts = aWDMFile.DataSets.ItemByKey(2004).Clone
            Case "SOLR" : lts = aWDMFile.DataSets.ItemByKey(2005).Clone
            Case "DEWP", "DPTEMP" : lts = aWDMFile.DataSets.ItemByKey(2007).Clone
            Case "CLOU" : lts = aWDMFile.DataSets.ItemByKey(2008).Clone
            Case Else 'not a needed timeseries, write it at end of WDM
                lTSer.Attributes.SetValue("ID", lTSer.Attributes.GetValue("ID") + 1000)
                If aWDMFile.AddDataset(lTSer, atcDataSource.EnumExistAction.ExistRenumber) Then
                    Logger.Dbg("CompileFinal:   Wrote ancillary dataset " & lTSer.ToString & " to WDM file")
                Else
                    Logger.Dbg("CompileFinal:   PROBLEM writing ancillary dataset " & lTSer.ToString & " to WDM file")
                End If
                Exit Sub
        End Select
        lts.Attributes.SetValue("Description", GetDescription(lCons))
        'only use up through 1995
        lB31TS = SubsetByDate(lts, lts.Attributes.GetValue("SJDay"), lEJD, Nothing)
        If lCons = "EVAP" Then 'save as Hamon generated PET
            lB31TS.Attributes.SetValue("Constituent", "PEVT")
            lB31TS.Attributes.SetValue("TSTYPE", "PEVT")
        End If
        lID = lB31TS.Attributes.GetValue("ID") - 2000
        'be sure lat/lng and Station Names get on final dataset, use updated data's coordinates
        lB31TS.Attributes.SetValue("LatDeg", lTSer.Attributes.GetValue("LatDeg"))
        lB31TS.Attributes.SetValue("LngDeg", lTSer.Attributes.GetValue("LngDeg"))
        lB31TS.Attributes.SetValue("STANAM", lTSer.Attributes.GetValue("STANAM"))
        If Not aWDMFile.DataSets.Keys.Contains(lID) Then
            Dim lSJD As Double = lTSer.Attributes.GetValue("SJDay")
            If lSJD <= lEJD Then
                'start by comparing common period
                If lSJD < lB31TS.Attributes.GetValue("SJDay") Then lSJD = lB31TS.Attributes.GetValue("SJDay")
                Dim lSubB31 As atcTimeseries = SubsetByDate(lB31TS, lSJD, lEJD, Nothing)
                Dim lSumB31 As Double = lSubB31.Attributes.GetValue("Sum")
                Dim lSubTS As atcTimeseries = SubsetByDate(lTSer, lSJD, lEJD, Nothing)
                Dim lSumTS As Double = lSubTS.Attributes.GetValue("Sum")
                If lSumB31 > 0 AndAlso lSumTS > 0 Then
                    Dim lDiff As Double = System.Math.Abs(((lSumB31 - lSumTS) / (lSumB31)) * 100)
                    If lDiff < 20 Then
                        Logger.Dbg("CompileFinal: QACHECK - For " & lCons & " from " & DumpDate(lSJD).Substring(14, 16) & " to " & DumpDate(lEJD).Substring(14, 16) & _
                                   " % diff between sum for B31 (" & DoubleToString(lSumB31, 10, , , , 4) & ") and new data (" & DoubleToString(lSumTS, 10, , , , 4) & ") is <20 (" & lDiff & ")")
                    Else
                        Logger.Dbg("CompileFinal: QACHECK - For " & lCons & " from " & DumpDate(lSJD).Substring(14, 16) & " to " & DumpDate(lEJD).Substring(14, 16) & _
                                   " % diff between sum for B31 (" & DoubleToString(lSumB31, 10, , , , 4) & ") and new data (" & DoubleToString(lSumTS, 10, , , , 4) & ") is >20 (" & lDiff & ")")
                        aQAStr = lCons & vbTab & lSubTS.numValues & vbTab & lB31TS.Attributes.GetValue("Location") & vbTab & DoubleToString(lSumB31, 10, , , , 4) & vbTab & _
                                 lTSer.Attributes.GetValue("Location") & vbTab & DoubleToString(lSumTS, 10, , , , 4) & vbTab & DoubleToString(lDiff, 8, , , , 4) & vbCrLf
                    End If
                Else
                    Logger.Dbg("CompileFinal: QACHECK - NOTE: For " & lCons & " from " & DumpDate(lSJD).Substring(14, 16) & " to " & DumpDate(lEJD).Substring(14, 16) & _
                               " B31 (" & DoubleToString(lSumB31, 10, , , , 4) & ") or new data (" & DoubleToString(lSumTS, 10, , , , 4) & ") sum is 0")
                End If
                'now merge timeseries
                lSubTS = Nothing
                lB31TS.Attributes.SetValue("ID", lID)
                'use same location as dataset being appended
                lB31TS.Attributes.SetValue("Location", lTSer.Attributes.GetValue("Location"))
                lB31TS.Attributes.SetValue("Description", lts.Attributes.GetValue("Description"))
                If aWDMFile.AddDataset(lB31TS) Then
                    Logger.Dbg("CompileFinal: Wrote Basins 3.1 data (" & lB31TS.ToString & ") to DSN " & lB31TS.Attributes.GetValue("ID"))
                    lSubTS = SubsetByDate(lTSer, lEJD, lTSer.Attributes.GetValue("EJDay"), Nothing)
                    lSubTS.Attributes.SetValue("ID", lB31TS.Attributes.GetValue("ID"))
                    If aWDMFile.AddDataset(lSubTS, atcDataSource.EnumExistAction.ExistAppend) Then
                        Logger.Dbg("CompileFinal:   Appended " & lTSer.ToString & " to Basins 3.1 data, DSN " & lSubTS.Attributes.GetValue("ID"))
                    Else
                        Logger.Dbg("CompileFinal:   PROBLEM Appending " & lTSer.ToString & " to Basins 3.1 data")
                    End If
                Else
                    Logger.Dbg("CompileFinal: PROBLEM writing Basins 3.1 data to DSN " & lB31TS.Attributes.GetValue("ID"))
                End If
                lSubB31 = Nothing
                lSubTS = Nothing
            Else
                Logger.Dbg("CompileFinal:  PROBLEM - TS to merge with BASINS31 data (" & lTSer.ToString & _
                           ") starts after 1995 (" & DumpDate(lSJD) & ")")
            End If
        Else
            Logger.Dbg("CompileFinal: Dataset already saved for " & lCons)
            lID = lTSer.Attributes.GetValue("ID") + 1000
            lTSer.Attributes.SetValue("ID", lID)
            If aWDMFile.AddDataset(lTSer) Then
                Logger.Dbg("CompileFinal:   Wrote timeseries " & lTSer.ToString & " to DSN " & lID)
            Else
                Logger.Dbg("CompileFinal:   PROBLEM writing timeseries " & lTSer.ToString & " to DSN " & lID)
            End If
        End If
        lTSer = Nothing
    End Sub

    Private Sub WriteFinal(ByVal aSrcWDM As atcWDM.atcDataSourceWDM, ByRef aNewWDMFile As atcWDM.atcDataSourceWDM, ByVal aStaName As String, ByVal aLocn As String, ByVal aLat As Double, ByVal aLng As Double)

        Dim lCons As String
        Dim llts As atcTimeseries
        For Each lts As atcTimeseries In aSrcWDM.DataSets
            llts = lts.Clone
            llts.EnsureValuesRead()
            llts.Attributes.SetValue("Location", aLocn)
            llts.Attributes.SetValue("STANAM", aStaName)
            llts.Attributes.SetValue("LatDeg", aLat)
            llts.Attributes.SetValue("LngDeg", aLng)
            lCons = llts.Attributes.GetValue("Constituent")
            Select Case lCons
                Case "HPCP", "HPCP1"
                    llts.Attributes.SetValue("ID", 1)
                    llts.Attributes.SetValue("Constituent", "PREC")
                Case "EVAP" 'save as Hamon PET
                    llts.Attributes.SetValue("ID", 6)
                    llts.Attributes.SetValue("Constituent", "PEVT")
                    'fix missing STANAM problem for EVAP
                    llts.Attributes.SetValue("STANAM", aStaName)
                Case "ATEM", "ATEMP"
                    llts.Attributes.SetValue("ID", 3)
                    llts.Attributes.SetValue("Constituent", "ATEM")
                Case "WIND"
                    llts.Attributes.SetValue("ID", 4)
                Case "SOLR"
                    llts.Attributes.SetValue("ID", 5)
                Case "DPTEMP"
                    llts.Attributes.SetValue("ID", 7)
                    llts.Attributes.SetValue("Constituent", "DEWP")
                Case "CLOU"
                    llts.Attributes.SetValue("ID", 8)
                Case Else
                    llts.Attributes.SetValue("ID", 1000 + lts.Attributes.GetValue("ID"))
            End Select
            'be sure TSTYPE matches Constituent name
            llts.Attributes.SetValue("TSTYPE", llts.Attributes.GetValue("Constituent"))
            WriteTS(aNewWDMFile, llts)
        Next

    End Sub

    Private Sub WriteTS(ByRef aNewWDMFile As atcWDM.atcDataSourceWDM, ByRef ats As atcTimeseries)
        'Write data to final WDM files - used for non Basins 3.1 sites.
        'If a final (i.e. non-ancilliary) dataset with the same ID exists, 
        'assume it is ISH data at the same location as existing SOD/HPD data.
        'For Precip (ID=1), assume HPD data is better and append ONLY newer ISH data.
        'For all other constituents, data were generated/disaggregated SOD values,
        'so use all observed ISH hourly values as they should be more accurate.
        Dim lID As Integer = ats.Attributes.GetValue("ID")
        Dim lCons As String = ats.Attributes.GetValue("Constituent")
        If lID < 9 Then ats.Attributes.SetValue("Description", GetDescription(lCons))
        If lID < 9 AndAlso aNewWDMFile.DataSets.Keys.Contains(lID) Then 'dataset already exists
            Dim lExistTs As atcTimeseries = aNewWDMFile.DataSets.ItemByKey(lID)
            Dim lExistEnd As Double = lExistTs.Attributes.GetValue("EJDay")
            Dim lNewStart As Double = ats.Attributes.GetValue("SJDay")
            Dim lNewEnd As Double = ats.Attributes.GetValue("EJDay")
            If lNewEnd >= lExistEnd AndAlso lNewStart < lExistEnd Then
                Dim lSubTs As New atcTimeseries(Nothing)
                If lCons = "PREC" Then 'keep all existing precip, append any additional
                    lSubTs = SubsetByDate(ats, lExistEnd, lNewEnd, Nothing)
                    If aNewWDMFile.AddDataset(lSubTs, atcDataSource.EnumExistAction.ExistAppend) Then
                        Logger.Dbg("CompileFinal: Appended " & ats.ToString & " to existing DSN after " & DumpDate(lExistEnd))
                    Else
                        Logger.Dbg("CompileFinal: PROBLEM appending " & ats.ToString & " to existing DSN after " & DumpDate(lExistEnd))
                    End If
                Else 'for other constituents, use all new data
                    'write subset of original data up to start of new data
                    lSubTs = SubsetByDate(lExistTs, lExistTs.Attributes.GetValue("SJDay"), lNewStart, Nothing)
                    If aNewWDMFile.AddDataset(lSubTs, atcDataSource.EnumExistAction.ExistReplace) Then
                        Logger.Dbg("CompileFinal: Rewrote subset of existing data " & lSubTs.ToString & " up to " & DumpDate(lNewStart))
                        If aNewWDMFile.AddDataset(ats, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("CompileFinal:   Appended " & ats.ToString & " to existing DSN after " & DumpDate(lNewStart))
                        Else
                            Logger.Dbg("CompileFinal:   PROBLEM appending " & ats.ToString & " to existing DSN after " & DumpDate(lNewStart))
                        End If
                    Else
                        Logger.Dbg("CompileFinal: PROBLEM rewriting subset of existing data " & lSubTs.ToString & " up to " & DumpDate(lNewStart))
                    End If
                End If
            End If
        Else 'just write it to next available dsn
            If aNewWDMFile.AddDataset(ats, atcDataSource.EnumExistAction.ExistRenumber) Then
                Logger.Dbg("CompileFinal: Wrote " & ats.ToString & " to WDM file")
            Else
                Logger.Dbg("CompileFinal: PROBLEM writing " & ats.ToString & " to WDM file")
            End If
        End If
    End Sub

    Private Sub WriteB31(ByVal aB31WDMName As String, ByVal aStation As String, ByRef aNewWDMFile As atcWDM.atcDataSourceWDM)
        Dim lB31WDM As New atcWDM.atcDataSourceWDM
        Dim i As Integer = 0
        lB31WDM.Open(aB31WDMName)
        Logger.Dbg("CompileFinal: Writing existing Basin 3.1 data")
        For Each lts As atcTimeseries In lB31WDM.DataSets
            If lts.Attributes.GetValue("Location").EndsWith(aStation) Then
                i += 1
                Dim llts As atcTimeseries = lts.Clone
                llts.Attributes.SetValue("ID", 2000 + i)
                llts.Attributes.SetValue("Description", "Original BASINS 3.1 Data")
                If aNewWDMFile.AddDataset(llts) Then
                    Logger.Dbg("CompileFinal: Wrote " & llts.ToString & " to WDM file.")
                Else
                    Logger.Dbg("CompileFinal: PROBLEM Writing " & llts.ToString & " to WDM file.")
                End If
                llts = Nothing
            End If
        Next
    End Sub

    Private Function GetDescription(ByVal aCons As String) As String
        Select Case aCons
            Case "HPCP", "HPCP1", "PREC" : Return "Hourly Precip in Inches"
            Case "EVAP" : Return "Hourly Potential ET in Inches"
            Case "ATEM", "ATEMP" : Return "Hourly Air Temperature in Degrees F"
            Case "WIND" : Return "Hourly Wind Speed in MPH"
            Case "SOLR" : Return "Hourly Solar Radiation in Langleys"
            Case "DEWP", "DPTEMP" : Return "Hourly Dewpoint Temperature in Degrees F"
            Case "CLOU" : Return "Hourly Cloud Cover in Tenths"
            Case Else : Return ""
        End Select
    End Function

End Module
