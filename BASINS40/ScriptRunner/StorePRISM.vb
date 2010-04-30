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
Imports atcSeasons
'Imports atcDataTree
'Imports atcEvents

Public Module StorePRISM
    Private Const pInputPath As String = "C:\BASINSMet\WDMFiltered\"
    Private Const pPRISMPath As String = "C:\BASINSMet\PRISM\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("QACheck:Start")
        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lCurWDM As String = "current.wdm"
        Dim lStation As String = ""
        Dim lCons As String = ""
        Dim lVal As Double
        Dim lRewrite As Boolean
        Dim lStaCnt As Integer = 0

        Dim lPrismPrecip As New atcTableDBF
        Dim lPrismTMin As New atcTableDBF
        Dim lPrismTMax As New atcTableDBF

        If lPrismPrecip.OpenFile(pPRISMPath & "us_ppt_ann.dbf") Then
            Logger.Dbg("StorePRISM: Opened PRISM Precip file " & lPrismPrecip.FileName)
        End If
        If lPrismTMin.OpenFile(pPRISMPath & "us_tmin_ann.dbf") Then
            Logger.Dbg("StorePRISM: Opened PRISM TMin file " & lPrismTMin.FileName)
        End If
        If lPrismTMax.OpenFile(pPRISMPath & "us_tmax_ann.dbf") Then
            Logger.Dbg("StorePRISM: Opened PRISM TMax file " & lPrismTMax.FileName)
        End If

        Logger.Dbg("StorePRISM: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("StorePRISM: Found " & lFiles.Count & " data files")
        For Each lFile As String In lFiles
            lStaCnt += 1
            FileCopy(lFile, lCurWDM)
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lCurWDM)
            Dim lDataSets As New atcDataGroup
            lDataSets = lWDMfile.DataSets.Clone
            lStation = FilenameNoExt(FilenameNoPath(lFile))
            Logger.Dbg("StorePRISM: For Station - " & lStation)
            For Each lDS As atcDataSet In lDataSets
                lRewrite = False
                lCons = lDS.Attributes.GetValue("Constituent")
                Select Case lCons
                    Case "PRCP", "PREC", "HPCP", "HPCP1"
                        If lPrismPrecip.FindFirst(1, lStation) Then
                            lVal = CDbl(lPrismPrecip.Value(6))
                            If lVal > 0 AndAlso lVal < 200 Then
                                lDS.Attributes.SetValue("PRECIP", lVal)
                                lRewrite = True
                            Else
                                Logger.Dbg("StorePRISM:  NOTE - No valid value found on PRISM Precip DBF file - no attribute stored")
                            End If
                        Else
                            Logger.Dbg("StorePRISM:  PROBLEM - could not find station " & lStation & " on PRISM Precip DBF file")
                        End If
                    Case "TMIN", "TMAX", "DPTP", "ATEMP", "DPTEMP"
                        If lPrismTMin.FindFirst(1, lStation) AndAlso lPrismTMax.FindFirst(1, lStation) Then
                            lVal = (CDbl(lPrismTMin.Value(6)) + CDbl(lPrismTMax.Value(6))) / 2
                            If lVal > 0 AndAlso lVal < 200 Then
                                lDS.Attributes.SetValue("UBC190", lVal)
                                lRewrite = True
                            Else
                                Logger.Dbg("StorePRISM:  NOTE - No valid value found on PRISM TMin/TMax DBF file - no attribute stored")
                            End If
                        Else
                            Logger.Dbg("StorePRISM:  PROBLEM - could not find station " & lStation & " on PRISM TMin/TMax DBF file")
                        End If
                End Select
                If lRewrite Then
                    If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistReplace) Then
                        Logger.Dbg("StorePRISM: Updated timeseries " & lDS.ToString & " with PRISM value")
                    Else
                        Logger.Dbg("StorePRISM: PROBLEM Updating timeseries " & lDS.ToString & " with PRISM value")
                    End If
                Else
                    Logger.Dbg("StorePRISM: No PRISM data available for timeseries " & lDS.ToString)
                End If
            Next
            FileCopy(lCurWDM, lFile)
            lDataSets.Clear()
            lDataSets = Nothing
            Kill(lCurWDM)
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
        Next
        Logger.Dbg("StorePRISM: Reviewed " & lStaCnt & " stations")
        Logger.Dbg("StorePRISM:Completed PRISM Storing")

    End Sub

    'Private Sub ProcessFiles(ByVal aFiles As NameValueCollection, ByVal aPrismPrec As atcTableDBF, ByVal aPrismTMin As atcTableDBF, ByVal aPrismTMax As atcTableDBF)
    '    Dim lts As atcTimeseries
    '    Dim ltssub As atcTimeseries
    '    Dim lSeasonsMonth As atcSeasonsMonth = Nothing
    '    Dim lMonthData As atcDataGroup = Nothing
    '    Dim lSDate() As Integer = {0, 1, 1, 0, 0, 0}
    '    Dim lEDate() As Integer = {0, 12, 31, 24, 0, 0}
    '    Dim lSJDay As Double
    '    Dim lEJDay As Double
    '    Dim lMonStr As String = ""
    '    Dim i As Integer

    '    Dim lCurWDM As String = "current.wdm"
    '    Dim lStation As String = ""
    '    Dim lCons As String = ""
    '    Dim lStr As String = ""
    '    Dim lFileStr As String = ""
    '    Dim lMonFile As String = ""
    '    Dim lFldVal As String = ""
    '    Dim lInd As Integer = 0
    '    Dim lMin As Double = 0
    '    Dim lMax As Double = 0
    '    Dim lCenter As Double = 0
    '    Dim lVal As Double = 0
    '    Dim lPctDiff As Double = 0
    '    Dim lAbsDiff As Double = 0
    '    Dim lRewrite As Boolean

    '    For Each lFile As String In aFiles
    '        FileCopy(lFile, lCurWDM)
    '        Dim lWDMfile As New atcWDM.atcDataSourceWDM
    '        lWDMfile.Open(lCurWDM)
    '        lStation = FilenameNoExt(FilenameNoPath(lFile))
    '        Logger.Dbg("StorePRISM: For Station - " & lStation)
    '        For Each lDS As atcDataSet In lWDMfile.DataSets
    '            lRewrite = False
    '            lCons = lDS.Attributes.GetValue("Constituent")
    '            Select Case lCons
    '                Case "PRCP", "PREC", "HPCP", "HPCP1"
    '                    If aPrismPrec.FindFirst(1, lStation) Then
    '                        lDS.Attributes.SetValue("PRECIP", aPrismPrec.Value(6))
    '                        lRewrite = True
    '                    Else
    '                        Logger.Dbg("StorePRISM:  PROBLEM - could not find station " & lStation & " on PRISM Precip DBF file")
    '                    End If
    '                Case "TMIN", "TMAX", "DPTP", "ATEMP", "DPTEMP"
    '                    If aPrismTMin.FindFirst(1, lStation) AndAlso aPrismTMax.FindFirst(1, lStation) Then
    '                        lVal = (aPrismTMin.Value(6) + aPrismTMax.Value(6)) / 2
    '                        lDS.Attributes.SetValue("UBC190", lVal)
    '                        lRewrite = True
    '                    Else
    '                        Logger.Dbg("StorePRISM:  PROBLEM - could not find station " & lStation & " on PRISM TMin/TMax DBF file")
    '                    End If
    '            End Select
    '            If lRewrite Then
    '                If lWDMfile.AddDataset(lDS, atcDataSource.EnumExistAction.ExistReplace) Then
    '                    Logger.Dbg("StorePRISM: Updated timeseries " & lDS.ToString & " with PRISM value")
    '                Else
    '                    Logger.Dbg("StorePRISM: PROBLEM Updating timeseries " & lDS.ToString & " with PRISM value")
    '                End If
    '            End If
    '        Next
    '        Kill(lCurWDM)
    '        lWDMfile.DataSets.Clear()
    '        lWDMfile = Nothing
    '    Next
    '    Logger.Dbg("StorePRISM:Completed PRISM Storing")

    '    'Application.Exit()

    'End Sub

End Module
