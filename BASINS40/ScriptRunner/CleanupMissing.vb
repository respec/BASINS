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

Public Module CleanupMissing
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFilled2007\"
    Private Const pAlreadyDone As String = ""'01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35" ',36,37,38,39,40,41,42,43,44,45,46" ',47,48,50,51,66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("CleanupMissing:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lTS As atcTimeseries
        Dim lSubTS As atcDataSet = Nothing

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lStation As String = ""
        Dim lCurPath As String = ""
        Dim lUpdate As Boolean = True
        Dim lPctMiss As Double
        Dim lStatePath As String

        Dim lStr As String = ""
        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 2 'DBF parsing detailed output format
        Dim lMissStr As String = ""
        Dim lDateStr As String = ""
        Dim lLenStr As String = ""
        Dim lDate(5) As Integer
        Dim lSJDate As Double
        Dim lEJDate As Double
        Dim lStaCnt As Integer = 0

        Logger.Dbg("CleanupMissing: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("CleanupMissing: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            lStatePath = Right(PathNameOnly(lfile), 2) & "\"
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                FileCopy(lFile, lCurWDM)
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lCurWDM)
                Dim lNewWDMfile As atcWDM.atcDataSourceWDM = Nothing
                lStation = FilenameNoExt(FilenameNoPath(lFile))
                Logger.Dbg("CleanupMissing: For Station - " & lStation)
                lStaCnt += 1
                For Each lTS In lWDMfile.DataSets
                    lUpdate = False
                    lStr = MissingDataSummary(lTS, lMVal, lMAcc, lFMin, lFMax, lRepType)
                    lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                    If lPctMiss > 0 Then
                        Logger.Dbg("CleanupMissing:  For Constituent " & lTS.Attributes.GetValue("Constituent"))
                        'find first instance of missing data
                        lMissStr = lStr.Substring(lStr.IndexOf("DETAIL:DATA") + 13)
                        lDateStr = StrSplit(lMissStr, ",", "")
                        lDateStr = StrSplit(lMissStr, ",", "")
                        If CDbl(lDateStr) - lTS.Attributes.GetValue("SJDay") > _
                           lTS.Attributes.GetValue("EJDay") - CDbl(lDateStr) Then
                            'missing data starts more than halfway through record, trim end
                            lSJDate = lTS.Attributes.GetValue("SJDay")
                            'back up to previous round day interval
                            lEJDate = System.Math.Round(CDbl(lDateStr) - 1)
                            Logger.Dbg("CleanupMissing:  Trim End of data starting at " & DumpDate(lEJDate))
                        Else 'trim data at beginning up to end of missing data
                            lMissStr = lStr.Substring(lStr.LastIndexOf("DETAIL:DATA") + 13)
                            lDateStr = StrSplit(lMissStr, ",", "")
                            lDateStr = StrSplit(lMissStr, ",", "")
                            lLenStr = StrSplit(lMissStr, ",", "")
                            'move ahead to next round day interval
                            If lTS.Attributes.GetValue("tu") = atcTimeUnit.TUDay Then
                                lSJDate = System.Math.Round(CDbl(lDateStr) + CDbl(lLenStr) + 1)
                            ElseIf lTS.Attributes.GetValue("tu") = atcTimeUnit.TUHour Then
                                lSJDate = System.Math.Round(CDbl(lDateStr) + (CDbl(lLenStr) / 24) + 1)
                            End If
                            lEJDate = lTS.Attributes.GetValue("EJDay")
                            Logger.Dbg("CleanupMissing:  Trim Start of data up to " & DumpDate(lSJDate))
                        End If
                        If lSJDate < lEJDate Then
                            lSubTS = SubsetByDate(lTS, lSJDate, lEJDate, Nothing)
                            If Not lSubTS Is Nothing Then
                                lUpdate = True
                            Else
                                Logger.Dbg("CleanupMissing: PROBLEM creating subset timeseries")
                            End If
                        Else
                            Logger.Dbg("CleanupMissing:  PROBLEM with dates for subset period")
                        End If
                    Else
                        Logger.Dbg("CleanupMissing:  For Constituent " & lTS.Attributes.GetValue("Constituent") & " no missing data.")
                    End If
                    If lUpdate Then
                        If lNewWDMfile Is Nothing Then
                            FileCopy(lCurWDM, lNewWDM)
                            lNewWDMfile = New atcWDM.atcDataSourceWDM
                            lNewWDMfile.Open(lNewWDM)
                        End If
                        If lNewWDMfile.AddDataset(lSubTS, atcDataSource.EnumExistAction.ExistReplace) Then
                            Logger.Dbg("CleanupMissing: Saved subset of data from " & DumpDate(lSubTS.Attributes.GetValue("SJDay")) & " to " & _
                                       DumpDate(lSubTS.Attributes.GetValue("EJDay")))
                            lStr = MissingDataSummary(lSubTS, lMVal, lMAcc, lFMin, lFMax, lRepType)
                            lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                            If lPctMiss > 0 Then
                                Logger.Dbg("CleanupMissing: PROBLEM - subset of data still contains missing values!")
                            End If
                        Else
                            Logger.Dbg("CleanupMissing: PROBLEM saving subset of data from " & DumpDate(lSubTS.Attributes.GetValue("SJDay")) & " to " & _
                                       DumpDate(lSubTS.Attributes.GetValue("EJDay")))
                        End If
                    End If
                Next
                If Not lNewWDMfile Is Nothing Then
                    If lNewWDMfile.DataSets.Count = lWDMfile.DataSets.Count Then
                        FileCopy(lNewWDM, lFile)
                        Kill(lNewWDM)
                        lNewWDMfile.DataSets.Clear()
                        lNewWDMfile = Nothing
                        Logger.Dbg("CleanupMissing: Updated WDM file for station " & lStation)
                    Else
                        Logger.Dbg("CleanupMissing: PROBLEM - different number of dataset between original and updated WDM files")
                    End If
                End If
                Kill(lCurWDM)
                lWDMfile.DataSets.Clear()
                lWDMfile = Nothing
            End If
        Next
        Logger.Dbg("CleanupMissing:Completed Missing Cleanup - processed " & lStaCnt & " stations.")

        'Application.Exit()

    End Sub

    Private Function DetermineTimeOffset(ByVal aStation As String, ByVal aState As String, ByVal ats As atcTimeseries, ByVal aISHStaDBF As atcTableDBF, ByVal aSODStaDBF As atcTableDBF) As Integer

        Dim lESTStates As String = "ME,VT,NH,MA,RI,CT,NY,NJ,PA,MD,DE,VA,WV,OH,NC,SC,GA"
        Dim lCSTStates As String = "MN,WI,IA,IL,MO,AR,OK,LA,MS,AL"
        Dim lMSTStates As String = "MT,WY,UT,CO,AZ,NM"
        Dim lPSTStates As String = "WA,NV,CA"
        Dim lWBANID As String = ats.Attributes.GetValue("STAID")

        If lWBANID <> "999999" Then 'try to find match on SOD file
            If aSODStaDBF.FindFirst(2, lWBANID) Then
                Logger.Dbg("Time Offset - used SOD station Time value")
                If IsNumeric(aSODStaDBF.Value(6)) Then
                    Return CInt(aSODStaDBF.Value(6))
                End If
            End If
        End If
        If lESTStates.Contains(aState.ToUpper) Then
            Logger.Dbg("Time Offset - in Eastern States")
            Return 5
        ElseIf lCSTStates.Contains(aState.ToUpper) Then
            Logger.Dbg("Time Offset - in Central States")
            Return 6
        ElseIf lMSTStates.Contains(aState.ToUpper) Then
            Logger.Dbg("Time Offset - in Mountain States")
            Return 7
        ElseIf lPSTStates.Contains(aState.ToUpper) Then
            Logger.Dbg("Time Offset - in Pacific States")
            Return 8
        ElseIf aState.ToUpper = "AK" Then
            Logger.Dbg("Time Offset - in Alaska")
            Return 9
        ElseIf aState.ToUpper = "HI" Then
            Logger.Dbg("Time Offset - in Hawaii")
            Return 10
        ElseIf aState.ToUpper = "PR" Then
            Logger.Dbg("Time Offset - in Puerto Rico")
            Return 4
        End If
        Logger.Dbg("Time Offset - used Lat/Lng rules")
        Dim lLng As Double = ats.Attributes.GetValue("LNGDEG")
        Dim lLat As Double = ats.Attributes.GetValue("LATDEG")
        Select Case aState.ToUpper
            Case "FL"
                If lLng < -85.0 Then
                    Return 6
                Else
                    Return 5
                End If
            Case "TN"
                If lLat > 36 Then 'line is further East in northern portion of state
                    If lLng < -84.75 Then
                        Return 6
                    Else
                        Return 5
                    End If
                Else
                    If lLng < -85.3 Then
                        Return 6
                    Else
                        Return 5
                    End If
                End If
            Case "KY"
                If lLat > 37.25 Then 'further West in Northern portion of state
                    If lLng < -86 Then
                        Return 6
                    Else
                        Return 5
                    End If
                Else
                    If lLng < -85 Then
                        Return 6
                    Else
                        Return 5
                    End If
                End If
            Case "IN"
                If lLat > 41 Then 'Northwest corner of state is central
                    If lLng < -86.75 Then
                        Return 6
                    Else
                        Return 5
                    End If
                ElseIf lLat < 38.25 Then
                    If lLng < -86.75 Then 'Southwest corner of state is central
                        Return 6
                    Else
                        Return 5
                    End If
                Else 'everything else is Eastern
                    Return 5
                End If
            Case "MI"
                If lLng < -87.5 AndAlso lLat < 46.4 Then 'SW part of upper penn. in Central
                    Return 6
                Else
                    Return 5
                End If
            Case "TX"
                If lLat < -104.85 Then 'far West corner is in Mountain
                    Return 7
                Else
                    Return 6
                End If
            Case "KS"
                '2 western sections in Mountain
                If lLat > 37.75 AndAlso lLat < 38.25 AndAlso lLng < -101.1 Then
                    Return 7
                ElseIf lLat >= 38.25 AndAlso lLat < 39.6 AndAlso lLng < -101.5 Then
                    Return 7
                Else 'rest is in central
                    Return 6
                End If
            Case "NE"
                If lLng < -101 Then
                    Return 7
                Else
                    Return 6
                End If
            Case "SD"
                If lLng < -100.7 Then
                    Return 7
                Else
                    Return 6
                End If
            Case "ND"
                If lLng < -101 AndAlso lLat < 47.5 Then 'Southwest corner is in Mountain
                    Return 7
                Else
                    Return 6
                End If
            Case "ID"
                If lLat > 45.5 Then 'northern portion in pacific
                    Return 8
                Else
                    Return 7
                End If
            Case "OR"
                If lLng > -118.2 AndAlso lLat > 42.45 AndAlso lLat < 44.5 Then
                    'small Eastern section in Mountain
                    Return 7
                Else 'rest in pacific
                    Return 8
                End If
        End Select

    End Function

    Private Function ShiftDates(ByVal aTSer As atcTimeseries, ByVal aTU As modDate.atcTimeUnit, ByVal aShift As Integer) As atcDataSet
        Dim lTSer As atcTimeseries = aTSer.Clone
        Dim lShiftInc As Double = 0
        Select Case aTU
            Case atcTimeUnit.TUSecond
                lShiftInc = aShift * modDate.JulianSecond
            Case atcTimeUnit.TUMinute
                lShiftInc = aShift * modDate.JulianMinute
            Case atcTimeUnit.TUHour
                lShiftInc = aShift * modDate.JulianHour
            Case atcTimeUnit.TUDay
                lShiftInc = aShift
            Case atcTimeUnit.TUMonth
                lShiftInc = aShift * modDate.JulianMonth
            Case atcTimeUnit.TUYear
                lShiftInc = aShift * modDate.JulianYear
        End Select
        For i As Integer = 0 To lTSer.numValues
            lTSer.Dates.Value(i) += lShiftInc
        Next
        Return lTSer
    End Function

End Module
