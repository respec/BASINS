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

Public Module FirstFilter
    Private Const pInputPath As String = "C:\BASINSMet\WDMRaw\"
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFiltered\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("FirstFilter:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries

        Dim lStr As String = ""
        Dim lFileStr As String = ""

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1
        Dim lSkipped As Integer = 0

        Dim lCurWDM As String = pOutputPath & "current.wdm"
        Dim lNewWDM As String = pOutputPath & "new.wdm"
        Dim lMissDBF As New atcTableDBF
        Dim lSODStaDBF As New atcTableDBF
        Dim lHPDStaDBF As New atcTableDBF
        Dim lISHStaDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lCurPath As String = ""
        Dim lCons As String = ""
        Dim lAddMe As Boolean = True
        Dim lStaCount As Integer = 0

        If lMissDBF.OpenFile(pStationPath & "MissingSummary.dbf") Then
            Logger.Dbg("FirstFilter: Opened Missing Summary file " & pStationPath & "MissingSummary.dbf")
        Else
            Logger.Dbg("FirstFilter: PROBLEM Opening Summary file " & pStationPath & "MissingSummary.dbf")
        End If

        If lSODStaDBF.OpenFile(pStationPath & "coop_Summ.dbf") Then
            Logger.Dbg("FirstFilter: Opened SOD Station Summary file " & pStationPath & "coop_Summ.dbf")
        Else
            Logger.Dbg("FirstFilter: PROBLEM Opening SOD Station Summary file " & pStationPath & "coop_Summ.dbf")
        End If

        If lHPDStaDBF.OpenFile(pStationPath & "HPD_Stations.dbf") Then
            Logger.Dbg("FirstFilter: Opened HPD Station Summary file " & pStationPath & "HPD_Stations.dbf")
        Else
            Logger.Dbg("FirstFilter: PROBLEM Opening HPD Station Summary file " & pStationPath & "HPD_Stations.dbf")
        End If

        If lISHStaDBF.OpenFile(pStationPath & "ISH_Stations.dbf") Then
            Logger.Dbg("FirstFilter: Opened ISH Station Summary file " & pStationPath & "ISH_Stations.dbf")
        Else
            Logger.Dbg("FirstFilter: PROBLEM Opening ISH Station Summary file " & pStationPath & "ISH_Stations.dbf")
        End If

        Logger.Dbg("FirstFilter: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("FirstFilter: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            lStatePath = Right(PathNameOnly(lFile), 2) & "\"
            FileCopy(lFile, lCurWDM)
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lCurWDM)
            Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
            lNewWDMfile.Open(lNewWDM)
            lStation = FilenameNoExt(FilenameNoPath(lFile))
            Logger.Dbg("FirstFilter: For Station - " & lStation)
            For Each lts In lWDMfile.DataSets
                lMissDBF.FindFirst(3, lStation)
                lts.EnsureValuesRead()
                lCons = lts.Attributes.GetValue("Constituent")
                If Not lCons.EndsWith("-OBS") Then
                    'find missing data summary record for this station/constituent
                    While lCons <> lMissDBF.Value(4)
                        If Not lMissDBF.FindNext(3, lStation) Then
                            Logger.Dbg("FirstFilter: PROBLEM - Could not find constituent " & lCons & " for station " & lStation)
                            Exit While
                        End If
                    End While
                End If
                Select Case lCons
                    Case "TMIN", "TMAX", "PRCP", "EVAP", "WDMV", "HPCP", _
                         "WIND", "ATEMP", "DPTEMP", "HPCP1", "HPCP1-TM", _
                         "SKY-SST1", "SKY-SUM1", "SKYCOND"
                        lts.Attributes.SetValue("UBC200", lMissDBF.Value(19)) 'store %missing as attribute
                        lAddMe = True
                    Case "TMIN-OBS", "TMAX-OBS", "PRCP-OBS", "EVAP-OBS", "WDMV-OBS" 'save Obs Time timeseries for daily data
                        lAddMe = True
                    Case Else
                        Logger.Dbg("FirstFilter: Removed constituent " & lCons)
                        lAddMe = False
                End Select
                If lAddMe Then
                    If lNewWDMfile.AddDataset(lts) Then
                        Logger.Dbg("FirstFilter: Added Constituent " & lts.Attributes.GetValue("Constituent") & _
                                   " to DSN " & lts.Attributes.GetValue("ID"))
                    Else
                        Logger.Dbg("FirstFilter: PROBLEM adding constituent " & lts.Attributes.GetValue("Constituent") & _
                                   " to DSN " & lts.Attributes.GetValue("ID"))
                    End If
                End If
            Next
            If lNewWDMfile.DataSets.Count > 0 Then
                lStaCount += 1
                MkDirPath(pOutputPath & lStatePath)
                Logger.Dbg("FirstFilter: Writing new WDM file " & pOutputPath & lStatePath & lStation & ".wdm")
                FileCopy(lNewWDM, pOutputPath & lStatePath & lStation & ".wdm")
            Else
                Logger.Dbg("FirstFilter: No Datasets saved for station " & lStation & " - WDM file removed")
                lSkipped += 1
            End If
            Kill(lNewWDM)
            lNewWDMfile.DataSets.Clear()
            lNewWDMfile = Nothing
            Kill(lCurWDM)
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
        Next
        Logger.Dbg("FirstFilter: Saved " & lStaCount & " stations to WDM files" & vbcrlf & _
                   "             Removed " & lSkipped & " stations")
        Logger.Dbg("FirstFilter: Completed Filtering")

        'Application.Exit()

    End Sub

End Module
