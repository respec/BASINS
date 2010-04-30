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

Public Module BuildMetWDM
    Private Const pInputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pDisaggPath As String = "C:\BASINSMet\WDMFilled\subset\04\"
    Private Const pOutputPath As String = "C:\BASINS\Data\FtBenning\Met\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("BuildMetWDM:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lts As New atcTimeseries(Nothing)
        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lDPrecInd As Integer = 0
        Dim lHPrecInd As Integer = 100
        Dim lEvapInd As Integer = 120
        Dim lATempInd As Integer = 140
        Dim lDPTempInd As Integer = 160
        Dim lWindInd As Integer = 180
        Dim lSolrInd As Integer = 200
        Dim lClouInd As Integer = 220
        Dim lStaSumm As String = "DSN" & vbTab & "STAID" & vbTab & "TSTEP" & vbTab & "CONS" & vbTab & "STANAM" & vbTab & "Start" & vbTab & "End" & vbTab & "ANN. AVG." & vbCrLf
        Dim lDateStr As String
        Dim lTUStr As String
        Dim lCons As String = ""

        Dim lStations() As String = {"010427", "012730", "014502", "015397", "016129", "091372", _
                                     "092166", "094033", "095394", "095979", "097087", "097201", _
                                     "098657", "098661", "099291", "099506", "722284", "722250", " 722255"}

        Dim lNewWDMFile As New atcWDM.atcDataSourceWDM
        If lNewWDMFile.Open(pOutputPath & "FtBenMet.wdm") Then
            Logger.Dbg("BuildMetWDM: Opened new wdm file " & pOutputPath & "FtBenMet.wdm")
        Else
            Logger.Dbg("BuildMetWDM: Could not open new wdm file " & pOutputPath & "FtBenMet.wdm")
        End If

        For Each lStation As String In lStations
            Logger.Dbg("BuildMetWDM: Opening WDM file for station " & lStation)
            lWDMFile = New atcWDM.atcDataSourceWDM
            lWDMFile.Open(pInputPath & lStation & ".wdm")
            For Each lDS As atcDataSet In lWDMFile.DataSets
                lts = Nothing
                If lDS.Attributes.GetValue("ID") < 2000 Then 'don't get BASINS 3.1 for now
                    lCons = lDS.Attributes.GetValue("Constituent")
                    Select Case lCons
                        Case "PRCP"  'found daily precip
                            lDPrecInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lDPrecInd)
                        Case "HPCP", "PREC"  'found hourly precip
                            lHPrecInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("Constituent", "HPCP")
                            lts.Attributes.SetValue("TSTYPE", "HPCP")
                            lts.Attributes.SetValue("ID", lHPrecInd)
                        Case "EVAP"
                            lEvapInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lEvapInd)
                        Case "ATEM"
                            lATempInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lATempInd)
                        Case "DEWP"
                            lDPTempInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lDPTempInd)
                        Case "WIND"
                            lWindInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lWindInd)
                        Case "SOLR"
                            lSolrInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lSolrInd)
                        Case "CLOU"
                            lClouInd += 1
                            lts = lDS.Clone
                            lts.Attributes.SetValue("ID", lClouInd)
                    End Select
                End If
                If Not lts Is Nothing Then
                    lDateStr = DumpDate(lts.Attributes.GetValue("SJDay")).Substring(14, 10) & vbTab & _
                               DumpDate(lts.Attributes.GetValue("EJDay")).Substring(14, 10) & vbTab
                    If lts.Attributes.GetValue("TU") = 4 Then
                        lTUStr = "Daily"
                    Else
                        lTUStr = "Hourly"
                    End If
                    lStaSumm &= lts.Attributes.GetValue("ID") & vbTab & _
                                lts.Attributes.GetValue("Location") & vbTab & lTUStr & vbTab & _
                                lts.Attributes.GetValue("Constituent") & vbTab & _
                                lts.Attributes.GetValue("STANAM") & vbTab & lDateStr
                    If lCons = "PRCP" OrElse lCons = "HPCP" OrElse lCons = "PREC" OrElse lCons = "EVAP" Then
                        lStaSumm &= DoubleToString(lts.Attributes.GetValue("SumAnnual"), 6) & vbCrLf
                    Else
                        lStaSumm &= vbCrLf
                    End If
                    If lNewWDMFile.AddDataset(lts) Then
                        Logger.Dbg("BuildMetWDM: Wrote " & lts.ToString & " - " & lts.Attributes.GetValue("Stanam") & " to WDM file.")
                    Else
                        Logger.Dbg("BuildMetWDM: PROBLEM writing " & lts.ToString & " - " & lts.Attributes.GetValue("Stanam") & " to WDM file.")
                    End If
                End If
            Next
            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing
            'If FileExists(pDisaggPath & lStation & "_disagg.wdm") Then
            '    lWDMFile = New atcWDM.atcDataSourceWDM
            '    lWDMFile.Open(pDisaggPath & lStation & "_disagg.wdm")
            '    For Each lDS As atcDataSet In lWDMFile.DataSets
            '        lts = Nothing
            '        If lDS.Attributes.GetValue("Constituent") = "PREC" Then 'found hourly precip
            '            lts = lDS.Clone
            '            lHPrecInd += 1
            '            lts.Attributes.SetValue("ID", lHPrecInd)
            '            lts.Attributes.SetValue("Constituent", "HPCP")
            '        End If
            '        If Not lts Is Nothing Then
            '            'If lNewWDMFile.AddDataset(lts) Then
            '            '    Logger.Dbg("BuildMetWDM: Wrote " & lts.ToString & " - " & lts.Attributes.GetValue("Stanam") & " to WDM file.")
            '            lDateStr = DumpDate(lts.Attributes.GetValue("SJDay")).Substring(14, 10) & vbTab & _
            '                       DumpDate(lts.Attributes.GetValue("EJDay")).Substring(14, 10) & vbTab
            '            lStaSumm &= lts.Attributes.GetValue("ID") & vbTab & _
            '                        lts.Attributes.GetValue("Location") & vbTab & "Hrly-Dis" & vbTab & _
            '                        lts.Attributes.GetValue("STANAM") & vbTab & lDateStr & vbTab & _
            '                        DoubleToString(lts.Attributes.GetValue("SumAnnual"), 6) & vbCrLf
            '            'Else
            '            '    Logger.Dbg("BuildMetWDM: PROBLEM writing " & lts.ToString & " - " & lts.Attributes.GetValue("Stanam") & " to WDM file.")
            '            'End If
            '        End If
            '    Next
            'End If
        Next
        SaveFileString(pOutputPath & "FtBenMetWDM.sum", lStaSumm)
        Logger.Dbg("BuildMetWDM: Completed WDM file creation")

        'Application.Exit()

    End Sub

End Module
