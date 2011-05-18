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
'Imports atcDataTree
'Imports atcEvents

Public Module Append2007
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled2007\"
    Private Const pB31WDMPath As String = "C:\BASINSMet\Basins31WDMs\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pAlreadyDone As String = "" '01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,50,51" ',66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.StartToFile(pOutputPath & "Append2007.log")
        Logger.Dbg("Append2007:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim lCons As String = ""
        Dim lRec As Integer = 0
        Dim i As Integer
        Dim lLat As Double
        Dim lLng As Double
        Dim lElev As Double
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
            Logger.Dbg("Append2007: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        Else
            Logger.Dbg("Append2007: PROBLEM Opening Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If
        Dim lB31DBF As New atcTableDBF
        If lB31DBF.OpenFile(pStationPath & "BASINS31Map.dbf") Then
            Logger.Dbg("Append2007: Opened BASINS 31 Station Map to ISH file " & pStationPath & "BASINS31Map.dbf")
        Else
            Logger.Dbg("Append2007: PROBLEM Opening BASINS 31 Station Map to ISH file " & pStationPath & "BASINS31Map.dbf")
        End If
        Dim lMatchISHSOD As New atcTableDBF
        If lMatchISHSOD.OpenFile(pStationPath & "Matching_ISH+SOD.dbf") Then
            Logger.Dbg("Append2007: Opened Matching ISH + SOD Station file " & pStationPath & "Matching_ISH+SOD.dbf")
        Else
            Logger.Dbg("Append2007: PROBLEM Opening Matching ISH + SOD Station file " & pStationPath & "Matching_ISH+SOD.dbf")
        End If

        Dim lB31IDs As New atcCollection
        For i = 1 To lB31DBF.NumRecords
            lB31DBF.CurrentRecord = i
            lState = lStIds.ItemByKey(lB31DBF.Value(2))
            If lState.Length > 0 Then
                lB31IDs.Add(lB31DBF.CurrentRecord, lState & Format(CInt(lB31DBF.Value(3)), "0000"))
            Else
                Logger.Dbg("Append2007:  PROBLEM - can't find state ID for " & lState)
            End If
        Next i

        Logger.Dbg("Append2007: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        'loop through existing WDMFinal files
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("Append2007: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("Append2007: Opening data file - " & lFile)
            lState = FilenameNoPath(lFile).Substring(2, 2) ' Right(PathNameOnly(lFile), 2)
            If CDbl(lState) > 67 Then 'ISH only station, use State abbreviation
                lStatePath = FilenameNoPath(lFile).Substring(0, 2)
            Else
                lStatePath = lState
            End If
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lNewLocn = FilenameNoExt(FilenameNoPath(lFile))
                lStation = lNewLocn.Substring(2)
                lState = FilenameNoPath(lFile).Substring(0, 2)
                Logger.Dbg("Append2007: For station " & lStation)
                If lStationDBF.FindFirst(1, lStation) Then
                    lLat = lStationDBF.Value(4)
                    lLng = lStationDBF.Value(5)
                    lElev = lStationDBF.Value(6) * 0.3048 'Elevation values in feet, convert to meters
                    lStaName = lStationDBF.Value(2)
                    'merge new data onto existing
                    Dim lNewWDMFile As New atcWDM.atcDataSourceWDM
                    'first see if we need to merge ISH station with COOP station
                    If lMatchISHSOD.FindFirst(1, lStation) Then 'this station has a matching ISH station, try to merge them
                        lState = lStIds.Keys(lStIds.IndexOf(lStatePath))
                        Dim lISHSta As String = pInputPath & lState & "\" & lMatchISHSOD.Value(6) & ".wdm"
                        If FileExists(lISHSta) Then
                            'ISH station exists, open it as WDM file containing new data
                            lNewWDMFile.Open(lISHSta)
                            Logger.Dbg("Append2007:   Appending data from ISH station " & lMatchISHSOD.Value(6) & " (" & lMatchISHSOD.Value(8) & _
                                       ") to station " & lStation & " (" & lMatchISHSOD.Value(3) & ")")
                            WriteFinal(lWDMfile, lNewWDMFile, lStaName, lNewLocn, lLat, lLng, lElev)
                        End If
                    End If

                    'second, update data from WDM file of same station name
                    lNewWDMFile.DataSets.Clear()
                    lNewWDMFile = Nothing
                    lNewWDMFile = New atcWDM.atcDataSourceWDM
                    lNewWDMFile.Open(pInputPath & lStatePath & "\" & lStation & ".wdm")
                    Logger.Dbg("Append2007:   Appending data from file " & pInputPath & lStatePath & "\" & lStation & _
                               " to file " & lFile)
                    WriteFinal(lWDMfile, lNewWDMFile, lStaName, lNewLocn, lLat, lLng, lElev)

                    ''store all constituents in appropriate DSNs
                    'Dim lMerged As Boolean = False
                    'If IsNumeric(lStatePath) Then 'coop (SOD or HPD) data
                    '    If lStIds.IndexOf(lStatePath) >= 0 Then
                    '        lState = lStIds.Keys(lStIds.IndexOf(lStatePath))
                    '    ElseIf lStatePath = "66" Then
                    '        lState = "PR"
                    '    ElseIf lStatePath = "67" Then
                    '        lState = "VI"
                    '    Else
                    '        lState = ""
                    '    End If
                    'Else 'ISH data
                    '    lState = lStatePath.ToUpper
                    '    If lMatchISHSOD.FindFirst(6, lStation) Then 'this station matches a coop station, try to merge them
                    '        Dim lCoopSta As String = pOutputPath & lState & lMatchISHSOD.Value(1) & ".wdm"
                    '        If FileExists(lCoopSta) Then
                    '            'coop station exists, open it as WDM file to write to
                    '            lNewWDMFile.DataSets.Clear()
                    '            lNewWDMFile = Nothing
                    '            lNewWDMFile = New atcWDM.atcDataSourceWDM
                    '            lNewWDMFile.Open(lCoopSta)
                    '            Logger.Dbg("Append2007:  Merging ISH station " & lStation & " (" & lMatchISHSOD.Value(8) & _
                    '                       ") with Coop station " & lMatchISHSOD.Value(1) & " (" & lMatchISHSOD.Value(3) & ")")
                    '            'update location, station name, and lat/lng to use coop station values
                    '            lNewLocn = lState & lMatchISHSOD.Value(1)
                    '            lStaName = lNewWDMFile.DataSets(0).Attributes.GetValue("STANAM")
                    '            lLat = lMatchISHSOD.Value(4)
                    '            lLng = lMatchISHSOD.Value(5)
                    '            lMerged = True
                    '        End If
                    '    End If
                    'End If
                Else
                    Logger.Dbg("Append2007: PROBLEM - couldn't find station on station file!")
                End If
            End If
        Next
        'SaveFileString(pOutputPath & "QACheck.txt", lQAStr)
        Logger.Dbg("Append2007: Completed storing Final WDM files")

        'Application.Exit()

    End Sub

    Private Sub WriteFinal(ByVal aSrcWDM As atcWDM.atcDataSourceWDM, ByRef aNewWDMFile As atcWDM.atcDataSourceWDM, ByVal aStaName As String, ByVal aLocn As String, ByVal aLat As Double, ByVal aLng As Double, ByVal aElev As Double)

        Dim lCons As String
        Dim lScen As String
        Dim lID As Integer
        Dim lNewCons As String
        Dim lAppend As Boolean = False
        Dim llts As atcTimeseries
        Dim lNewTS As atcTimeseries
        Dim lCopied As Boolean = False

        While Not lCopied
            Try
                FileCopy(aLocn & ".wdm", "Temp.wdm")
                lCopied = True
            Catch
                Application.DoEvents()
                System.Threading.Thread.CurrentThread.Sleep(100)
            End Try
        End While
        Dim lSrcWDM As New atcWDM.atcDataSourceWDM
        lSrcWDM.Open("temp.wdm")

        For Each lts As atcTimeseries In lSrcWDM.DataSets
            lAppend = False
            lID = lts.Attributes.GetValue("ID")
            If lID < 2000 Then 'don't bother with old BASINS 3.0 data
                llts = lts.Clone
                llts.EnsureValuesRead()
                'be sure to add elevation to final datasets
                llts.Attributes.SetValue("ELEV", aElev)
                lCons = llts.Attributes.GetValue("Constituent")
                lScen = llts.Attributes.GetValue("Scenario")
                If lCons <> "PREC" OrElse lScen <> "COMPUTED" Then 'skip updating computed precip as Disagg step will be done later
                    'now look to see if any new data available for this constituent
                    For Each lNTS As atcTimeseries In aNewWDMFile.DataSets
                        lNewCons = lNTS.Attributes.GetValue("Constituent")
                        If lCons = lNewCons OrElse _
                            (lCons = "PREC" And (lNewCons = "HPCP" Or lNewCons = "HPCP1")) OrElse _
                            (lCons = "PEVT" And lNewCons = "EVAP") OrElse _
                            (lCons = "ATEM" And lNewCons = "ATEMP") OrElse _
                            (lCons = "DEWP" And lNewCons = "DPTEMP") Then
                            If lNTS.Attributes.GetValue("EJDay") > llts.Attributes.GetValue("EJDay") Then 'new data exsits
                                If lNTS.Attributes.GetValue("SJDay") <= llts.Attributes.GetValue("EJDay") Then 'data overlap, OK to append
                                    lAppend = True
                                    Logger.Dbg("Append2007:    Try to append " & lNTS.ToString & " (ends " & lNTS.Attributes.GetFormattedValue("EJDay") & ") to " _
                                                                               & llts.ToString & " (ends " & llts.Attributes.GetFormattedValue("EJDay") & ")")
                                    lNewTS = SubsetByDate(lNTS, llts.Attributes.GetValue("EJDay"), lNTS.Attributes.GetValue("EJDay"), Nothing)
                                Else 'gap between end of existing and start of new data, can't append
                                    Logger.Dbg("Append2007:    Unable to append new data " & lNTS.ToString & " (starts " & lNTS.Attributes.GetFormattedValue("SJDay") & ") to " _
                                                                               & llts.ToString & " (ends " & llts.Attributes.GetFormattedValue("EJDay") & ")")
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End If
                If lAppend Then
                    'Select Case lNewCons
                    '    Case "HPCP", "HPCP1"
                    '        lNewTS.Attributes.SetValue("ID", 1)
                    '    Case "EVAP"
                    '        lNewTS.Attributes.SetValue("ID", 6)
                    '    Case "ATEM", "ATEMP"
                    '        lNewTS.Attributes.SetValue("ID", 3)
                    '    Case "WIND"
                    '        lNewTS.Attributes.SetValue("ID", 4)
                    '    Case "SOLR"
                    '        lNewTS.Attributes.SetValue("ID", 5)
                    '    Case "DPTEMP"
                    '        lNewTS.Attributes.SetValue("ID", 7)
                    '    Case "CLOU"
                    '        lNewTS.Attributes.SetValue("ID", 8)
                    '    Case Else
                    '        lNewTS.Attributes.SetValue("ID", 1000 + lNewTS.Attributes.GetValue("ID"))
                    'End Select
                    'set new data DSN to existing data DSN
                    lNewTS.Attributes.SetValue("ID", lID)
                    If aSrcWDM.AddDataset(lNewTS, atcDataSource.EnumExistAction.ExistAppend) Then
                        Logger.Dbg("Append2007:     Appended " & lNewTS.ToString & " to " & llts.ToString & " on DSN " & lNewTS.Attributes.GetValue("ID"))
                    Else
                        Logger.Dbg("Append2007:     PROBLEM appending " & lNewTS.ToString & " to " & llts.ToString & " on DSN " & lNewTS.Attributes.GetValue("ID"))
                    End If
                    lNewTS = Nothing
                Else 'just update elevation attribute
                    If aSrcWDM.WriteAttributes(llts) Then
                        Logger.Dbg("Append2007:    Updated ELEV attribute on " & llts.ToString & ", DSN " & llts.Attributes.GetValue("ID"))
                    Else
                        Logger.Dbg("Append2007:    PROBLEM updating ELEV attribute on " & llts.ToString & ", DSN " & llts.Attributes.GetValue("ID"))
                    End If
                End If
            End If
        Next
        lSrcWDM.DataSets.Clear()
        lSrcWDM = Nothing
        While IO.File.Exists("Temp.wdm")
            TryDelete("Temp.wdm")
        End While

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
                        Logger.Dbg("Append2007: Appended " & ats.ToString & " to existing DSN after " & DumpDate(lExistEnd))
                    Else
                        Logger.Dbg("Append2007: PROBLEM appending " & ats.ToString & " to existing DSN after " & DumpDate(lExistEnd))
                    End If
                Else 'for other constituents, use all new data
                    'write subset of original data up to start of new data
                    lSubTs = SubsetByDate(lExistTs, lExistTs.Attributes.GetValue("SJDay"), lNewStart, Nothing)
                    If aNewWDMFile.AddDataset(lSubTs, atcDataSource.EnumExistAction.ExistReplace) Then
                        Logger.Dbg("Append2007: Rewrote subset of existing data " & lSubTs.ToString & " up to " & DumpDate(lNewStart))
                        If aNewWDMFile.AddDataset(ats, atcDataSource.EnumExistAction.ExistAppend) Then
                            Logger.Dbg("Append2007:   Appended " & ats.ToString & " to existing DSN after " & DumpDate(lNewStart))
                        Else
                            Logger.Dbg("Append2007:   PROBLEM appending " & ats.ToString & " to existing DSN after " & DumpDate(lNewStart))
                        End If
                    Else
                        Logger.Dbg("Append2007: PROBLEM rewriting subset of existing data " & lSubTs.ToString & " up to " & DumpDate(lNewStart))
                    End If
                End If
            End If
        Else 'just write it to next available dsn
            If aNewWDMFile.AddDataset(ats, atcDataSource.EnumExistAction.ExistRenumber) Then
                Logger.Dbg("Append2007: Wrote " & ats.ToString & " to WDM file")
            Else
                Logger.Dbg("Append2007: PROBLEM writing " & ats.ToString & " to WDM file")
            End If
        End If
    End Sub

    Private Sub WriteB31(ByVal aB31WDMName As String, ByVal aStation As String, ByRef aNewWDMFile As atcWDM.atcDataSourceWDM)
        Dim lB31WDM As New atcWDM.atcDataSourceWDM
        Dim i As Integer = 0
        lB31WDM.Open(aB31WDMName)
        Logger.Dbg("Append2007: Writing existing Basin 3.1 data")
        For Each lts As atcTimeseries In lB31WDM.DataSets
            If lts.Attributes.GetValue("Location").EndsWith(aStation) Then
                i += 1
                Dim llts As atcTimeseries = lts.Clone
                llts.Attributes.SetValue("ID", 2000 + i)
                llts.Attributes.SetValue("Description", "Original BASINS 3.1 Data")
                If aNewWDMFile.AddDataset(llts) Then
                    Logger.Dbg("Append2007: Wrote " & llts.ToString & " to WDM file.")
                Else
                    Logger.Dbg("Append2007: PROBLEM Writing " & llts.ToString & " to WDM file.")
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
