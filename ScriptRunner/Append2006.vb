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

Public Module Append2006
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"
    Private Const pInputPath As String = "C:\BASINSMet\WDMRaw\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFiltered\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Append2006:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lDate() As Integer = {2005, 1, 1, 0, 0, 0}
        Dim lMinDate As Double = Date2J(lDate)

        Dim lTS As atcTimeseries
        Dim lNewTS As atcDataSet = Nothing

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        'Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lSODStaDBF As New atcTableDBF
        Dim lHPDStaDBF As New atcTableDBF
        Dim lISHStaDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lState As String = ""
        Dim lCurPath As String = ""
        Dim lCons As String = ""
        Dim lAddMe As Boolean = True
        Dim lCnt As Integer = 0
        Dim lCurEJDay As Double
        Dim lUpdate As Boolean

        If lSODStaDBF.OpenFile(pStationPath & "coop_Summ.dbf") Then
            Logger.Dbg("Append2006: Opened SOD Station Summary file " & pStationPath & "coop_Summ.dbf")
        Else
            Logger.Dbg("Append2006: PROBLEM Opening SOD Station Summary file " & pStationPath & "coop_Summ.dbf")
        End If

        If lISHStaDBF.OpenFile(pStationPath & "ISH_Stations.dbf") Then
            Logger.Dbg("Append2006: Opened ISH Station Summary file " & pStationPath & "ISH_Stations.dbf")
        Else
            Logger.Dbg("Append2006: PROBLEM Opening ISH Station Summary file " & pStationPath & "ISH_Stations.dbf")
        End If

        Logger.Dbg("Append2006: Get all files in data directory " & pOutputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("Append2006: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            lUpdate = False
            lState = Right(PathNameOnly(lFile), 2)
            lCnt += 1
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lFile)
            FileCopy(lFile, lCurWDM)
            Dim lCurWDMfile As New atcWDM.atcDataSourceWDM
            lCurWDMfile.Open(lCurWDM)
            lStation = FilenameNoExt(FilenameNoPath(lFile))
            Dim lOffset As Integer = 0
            Logger.Dbg("Append2006: For Station - " & lStation)
            For Each lTS In lWDMfile.DataSets
                lCurEJDay = lTS.Attributes.GetValue("EJDay")
                If lCurEJDay > lMinDate Then 'existing data is current, find update
                    Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
                    If FileExists(pInputPath & "\" & lState & "\" & FilenameNoPath(lFile)) Then
                        lNewWDMfile.Open(pInputPath & "\" & lState & "\" & FilenameNoPath(lFile))
                        For Each lTS06 As atcTimeseries In lNewWDMfile.DataSets
                            If lTS06.Attributes.GetValue("Constituent") = lTS.Attributes.GetValue("Constituent") Then
                                'same constituent, see if there's new data
                                If lTS06.Attributes.GetValue("EJDay") > lCurEJDay Then 'yes, new data to add
                                    If Not IsNumeric(lState) Then
                                        If lOffset = 0 Then
                                            lOffset = DetermineTimeOffset(lStation, lState, lTS, lISHStaDBF, lSODStaDBF)
                                            Logger.Dbg("Append2006: Shifting data, Time Offset set to " & lOffset)
                                        End If
                                        lNewTS = ShiftDates(lTS06, atcTimeUnit.TUHour, -lOffset)
                                    Else
                                        lNewTS = lTS06.Clone
                                    End If
                                    'preserve attributes from existing timeseries
                                    lNewTS.Attributes.SetValue("UBC200", lTS.Attributes.GetValue("UBC200"))
                                    If lTS.Attributes.ContainsAttribute("UBC190") Then
                                        lNewTS.Attributes.SetValue("UBC190", lTS.Attributes.GetValue("UBC190"))
                                    End If
                                    If lTS.Attributes.ContainsAttribute("PRECIP") Then
                                        lNewTS.Attributes.SetValue("PRECIP", lTS.Attributes.GetValue("PRECIP"))
                                    End If
                                    If lCurWDMfile.AddDataset(lNewTS, atcDataSource.EnumExistAction.ExistReplace) Then
                                        lUpdate = True
                                        Logger.Dbg("Append2006: For " & lNewTS.Attributes.GetValue("Constituent") & ": data from " & _
                                                   lTS.Attributes.GetValue("SJDay") & " to " & lTS.Attributes.GetValue("EJDay") & vbCrLf & _
                                                   "                                   replaced with data from " & _
                                                   lTS06.Attributes.GetValue("SJDay") & " to " & lTS06.Attributes.GetValue("EJDay"))
                                    Else
                                        Logger.Dbg("Append2006: PROBLEM updating " & lNewTS.Attributes.GetValue("Constituent") & _
                                                   " on DSN " & lNewTS.Attributes.GetValue("ID"))
                                    End If
                                End If
                            End If
                        Next
                        lNewWDMfile.DataSets.Clear()
                        lNewWDMfile = Nothing
                    Else
                        Logger.Dbg("Append2006: PROBLEM - could not find file " & pInputPath & "\" & lState & "\" & FilenameNoPath(lFile))
                    End If
                Else
                    Logger.Dbg("Append2006: " & lTS.Attributes.GetValue("Constituent") & " not current (through 2005)")
                End If
            Next
            If lUpdate Then
                Kill(lFile)
                FileCopy(lCurWDM, lFile)
                Logger.Dbg("Append2006: Wrote updated WDM file " & lFile)
            Else
                Logger.Dbg("Append2006: No Datasets updated for station " & lStation)
            End If
            'Kill(lNewWDM)
            Kill(lCurWDM)
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
            lCurWDMfile.DataSets.Clear()
            lCurWDMfile = Nothing
        Next
        Logger.Dbg("Append2006: Completed 2006 Updating - processed " & lCnt & " Stations")

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
