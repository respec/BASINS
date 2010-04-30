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

Public Module QACheck
    Private Const pInputPath As String = "C:\BASINSMet\WDMFilled\Subset\"
    Private Const pPRISMPath As String = "C:\BASINSMet\PRISM\"
    Private Const pOutputPath As String = "C:\BASINSMet\QAResults\"
    'Private Const pDBFType As String = "mon" 'use "mon" for monthly normals and ave annual
    Private Const pDBFType As String = "all" 'use "all" for each monthly and annual value as well as norms

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("QACheck:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim ltssub As atcTimeseries
        Dim lSeasonsMonth As atcSeasonsMonth = Nothing
        Dim lMonthData As atcDataGroup = Nothing
        Dim lSDate() As Integer = {0, 1, 1, 0, 0, 0}
        Dim lEDate() As Integer = {0, 12, 31, 24, 0, 0}
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lMonStr As String = ""
        Dim i As Integer

        'Dim lCurWDM As String = pInputPath & "current.wdm"
        Dim lStation As String = ""
        Dim lCons As String = ""
        Dim lStr As String = ""
        Dim lFileStr As String = ""
        Dim lMonFile As String = ""
        Dim lFldVal As String = ""
        Dim lInd As Integer = 0
        Dim lMin As Double = 0
        Dim lMax As Double = 0
        Dim lCenter As Double = 0
        Dim lVal As Double = 0
        Dim lPctDiff As Double = 0
        Dim lAbsDiff As Double = 0
        Dim lNumNoted As Integer = 0
        Dim lNumChecked As Integer = 0
        Dim lTotals As Boolean = True

        Dim lyr As Integer = 1970
        Dim lsyr As Integer = 1970
        Dim llsyr As Integer = 1970
        Dim lspan As Integer = 36
        lSDate(0) = lyr
        lSJDay = Date2J(lSDate)
        lEDate(0) = lyr + lspan - 1
        lEJDay = Date2J(lEDate)

        Logger.Dbg("QACheck: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("QACheck: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            'FileCopy(lFile, lCurWDM)
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lFile)
            lStation = FilenameNoExt(FilenameNoPath(lFile))
            lStr = vbCrLf & "For Station - " & lStation & " - comparing for period " & DumpDate(lSJDay).Substring(14) & " to " & DumpDate(lEJDay).Substring(14) & vbCrLf
            Logger.Dbg("QACheck: " & lStr)

            'these assignments were for generating running averages for Atlanta & Richmond test stations
            'If lStation = "090451" Then
            '    lsyr = 1930
            '    llsyr = 1950
            '    lspan = 62
            'ElseIf lStation = "447201" Then
            '    lsyr = 1949
            '    llsyr = 1955
            '    lspan = 44
            'End If

            For Each lDS As atcDataSet In lWDMfile.DataSets
                lNumNoted = 0
                lNumChecked = 0
                lCons = lDS.Attributes.GetValue("Constituent")
                'If lCons = "PREC" OrElse _
                Dim lPrismDBF As New atcTableDBF
                If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") OrElse _
                   lCons = "TMAX" OrElse lCons = "TMIN" Then
                    If lCons = "TMAX" Then
                        lPrismDBF.OpenFile(pPRISMPath & "us_tmax_" & pDBFType & ".dbf")
                    ElseIf lCons = "TMIN" Then
                        lPrismDBF.OpenFile(pPRISMPath & "us_tmin_" & pDBFType & ".dbf")
                    Else
                        lPrismDBF.OpenFile(pPRISMPath & "us_ppt_" & pDBFType & ".dbf")
                    End If
                    'For lyr As Integer = lsyr To llsyr
                    If lDS.Attributes.GetValue("SJDay") <= lSJDay AndAlso _
                       lDS.Attributes.GetValue("EJDay") >= lEJDay Then
                        ltssub = SubsetByDate(lDS, lSJDay, lEJDay, Nothing)
                        lStr &= vbCrLf & "Comparing " & lCons
                        Logger.Dbg("QACheck:  Comparing " & lCons)
                        'If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                        If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                            lts = Aggregate(ltssub, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                        Else 'average temperature values
                            lts = Aggregate(ltssub, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                        End If

                        If lTotals Then
                            lFileStr &= vbCrLf & lStr & vbCrLf & "Year/Mon" & vbTab & "Min" & vbTab & "Ctr" & vbTab & "Max" & _
                                        vbTab & "Value" & vbTab & "%Diff" & vbTab & "Abs Diff" & vbCrLf
                            lMonFile &= vbCrLf & lStr & vbCrLf & "Year" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & _
                                       "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbCrLf
                            For i = 1 To lspan
                                lMonFile &= lsyr + i - 1
                                For lMon As Integer = 1 To 12
                                    lStr = lsyr + i - 1
                                    If lMon < 10 Then
                                        lMonStr = "0" & lMon
                                    Else
                                        lMonStr = lMon
                                    End If
                                    If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                                        lFldVal = "us_ppt_" & lStr & "." & lMonStr
                                    ElseIf lCons = "TMIN" Then
                                        lFldVal = "us_tmin_" & lStr & "." & lMonStr
                                    ElseIf lCons = "TMAX" Then
                                        lFldVal = "us_tmax_" & lStr & "." & lMonStr
                                    End If
                                    lStr &= "/" & lMonStr & " " & vbTab
                                    If lPrismDBF.FindFirst(2, lStation) Then
                                        While lPrismDBF.Value(5) <> lFldVal
                                            lPrismDBF.FindNext(2, lStation)
                                        End While
                                        'lStr &= lPrismDBF.Value(6).Substring(0, 5) & vbTab & lPrismDBF.Value(7).Substring(0, 5) & vbTab & lPrismDBF.Value(8).Substring(0, 5) & vbTab
                                        lStr &= DoubleToString(CDbl(lPrismDBF.Value(6)), , "#0.00", , , 4) & vbTab & DoubleToString(CDbl(lPrismDBF.Value(7)), , "#0.00", , , 4) & vbTab & DoubleToString(CDbl(lPrismDBF.Value(8)), , "#0.00", , , 4) & vbTab
                                        lInd = 12 * (i - 1) + lMon
                                        lVal = lts.Value(lInd)
                                        lMonFile &= vbTab & DoubleToString(lVal, , "#0.00", , , 4)
                                        lStr &= DoubleToString(lVal, , "#0.00", , , 4) & vbTab
                                        lPctDiff = 100 * ((lVal - lPrismDBF.Value(7)) / lPrismDBF.Value(7))
                                        lAbsDiff = lVal - lPrismDBF.Value(7)
                                        lStr &= DoubleToString(lPctDiff, , "#0.0", , , 3) & vbTab & DoubleToString(lAbsDiff, , "#0.0", , , 3)
                                        If System.Math.Abs(lPctDiff) > 10 AndAlso System.Math.Abs(lAbsDiff) > 1 Then
                                            lStr &= vbTab & "Check Difference"
                                            lNumNoted += 1
                                        End If
                                        lNumChecked += 1
                                    End If
                                    lFileStr &= lStr & vbCrLf
                                Next
                                lMonFile &= vbCrLf
                            Next
                        Else
                            lFileStr &= vbCrLf & lStr & vbCrLf & "Year/Mon" & vbTab & "Min" & vbTab & "Ctr" & vbTab & "Max" & _
                                        vbTab & "Value" & vbTab & "%Diff" & vbTab & "Abs Diff" & vbCrLf
                        End If

                        lFileStr &= vbCrLf & "Monthly Normals" & vbCrLf
                        lSeasonsMonth = New atcSeasonsMonth
                        lMonthData = lSeasonsMonth.Split(lts, Nothing)
                        lInd = 1
                        If lPrismDBF.FindFirst(2, lStation) Then
                            For Each lMonTS As atcDataSet In lMonthData
                                If lInd < 10 Then
                                    lMonStr = "0" & lInd
                                Else
                                    lMonStr = lInd
                                End If
                                lStr = "     " & lMonStr & " " & vbTab
                                'If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                                If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                                    lFldVal = "us_ppt." & lMonStr
                                ElseIf lCons = "TMIN" Then
                                    lFldVal = "us_tmin." & lMonStr
                                ElseIf lCons = "TMAX" Then
                                    lFldVal = "us_tmax." & lMonStr
                                End If
                                While lPrismDBF.Value(5) <> lFldVal
                                    lPrismDBF.FindNext(2, lStation)
                                End While
                                lStr &= lPrismDBF.Value(6).Substring(0, 5) & vbTab & lPrismDBF.Value(7).Substring(0, 5) & vbTab & lPrismDBF.Value(8).Substring(0, 5) & vbTab
                                lVal = lMonTS.Attributes.GetValue("Mean")
                                lStr &= DoubleToString(lVal, , "#0.00", , , 4) & vbTab
                                If lPrismDBF.Value(7) > 0 Then
                                    lPctDiff = 100 * ((lVal - lPrismDBF.Value(7)) / lPrismDBF.Value(7))
                                    lAbsDiff = lVal - lPrismDBF.Value(7)
                                    lStr &= DoubleToString(lPctDiff, , "#0.0", , , 3) & vbTab & DoubleToString(lAbsDiff, , "#0.0", , , 3)
                                    If System.Math.Abs(lPctDiff) > 10 AndAlso System.Math.Abs(lAbsDiff) > 1 Then
                                        lStr &= vbTab & "Check Difference"
                                        lNumNoted += 1
                                    End If
                                    lNumChecked += 1
                                End If
                                lFileStr &= lStr & vbCrLf
                                lInd += 1
                            Next
                        Else
                            Logger.Dbg("QACheck:  PROBLEM - could not find station " & lStation & " on PRISM file.")
                        End If

                        If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then
                            lts = Aggregate(ltssub, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Else
                            lts = Aggregate(ltssub, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                        End If
                        If lTotals Then
                            lFileStr &= vbCrLf & "Annual Totals" & vbCrLf
                            For i = 1 To lspan
                                lStr = lsyr + i - 1
                                lFldVal = "us_ppt_" & lStr & ".14"
                                'If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                                If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                                    lFldVal = "us_ppt_" & lStr & ".14"
                                ElseIf lCons = "TMIN" Then
                                    lFldVal = "us_tmin_" & lStr & ".14"
                                ElseIf lCons = "TMAX" Then
                                    lFldVal = "us_tmax_" & lStr & ".14"
                                End If
                                lStr &= vbTab & vbTab
                                If lPrismDBF.FindFirst(2, lStation) Then
                                    While lPrismDBF.Value(5) <> lFldVal
                                        lPrismDBF.FindNext(2, lStation)
                                    End While
                                    'lStr &= lPrismDBF.Value(6).Substring(0, 5) & vbTab & lPrismDBF.Value(7).Substring(0, 5) & vbTab & lPrismDBF.Value(8).Substring(0, 5) & vbTab
                                    lStr &= DoubleToString(CDbl(lPrismDBF.Value(6)), , "#0.00", , , 4) & vbTab & DoubleToString(CDbl(lPrismDBF.Value(7)), , "#0.00", , , 4) & vbTab & DoubleToString(CDbl(lPrismDBF.Value(8)), , "#0.00", , , 4) & vbTab
                                    lVal = lts.Value(i)
                                    lStr &= DoubleToString(lVal, , "#0.00", , , 4) & vbTab
                                    lPctDiff = 100 * ((lVal - lPrismDBF.Value(7)) / lPrismDBF.Value(7))
                                    lAbsDiff = lVal - lPrismDBF.Value(7)
                                    lStr &= DoubleToString(lPctDiff, , "#0.0", , , 3) & vbTab & DoubleToString(lAbsDiff, , "#0.0", , , 3)
                                    If System.Math.Abs(lPctDiff) > 10 AndAlso System.Math.Abs(lAbsDiff) > 1 Then
                                        lStr &= vbTab & "Check Difference"
                                        lNumNoted += 1
                                    End If
                                    lNumChecked += 1
                                End If
                                lFileStr &= lStr & vbCrLf
                            Next
                        End If
                        lStr = "Annual Ave" & vbTab
                        'If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                        If lCons = "PRCP" OrElse lCons.StartsWith("HPCP") Then 'sum precip values
                            lFldVal = "us_ppt.14"
                        ElseIf lCons = "TMIN" Then
                            lFldVal = "us_tmin.14"
                        ElseIf lCons = "TMAX" Then
                            lFldVal = "us_tmax.14"
                        End If
                        If lPrismDBF.FindFirst(2, lStation) Then
                            While lPrismDBF.Value(5) <> lFldVal
                                lPrismDBF.FindNext(2, lStation)
                            End While
                            lStr &= lPrismDBF.Value(6).Substring(0, 5) & vbTab & lPrismDBF.Value(7).Substring(0, 5) & vbTab & lPrismDBF.Value(8).Substring(0, 5) & vbTab
                            lVal = lts.Attributes.GetValue("Mean")
                            lStr &= DoubleToString(lVal, , "#0.00", , , 4) & vbTab
                            lPctDiff = 100 * ((lVal - lPrismDBF.Value(7)) / lPrismDBF.Value(7))
                            lAbsDiff = lVal - lPrismDBF.Value(7)
                            lStr &= DoubleToString(lPctDiff, , "#0.0", , , 3) & vbTab & DoubleToString(lAbsDiff, , "#0.0", , , 3)
                            If System.Math.Abs(lPctDiff) > 10 AndAlso System.Math.Abs(lAbsDiff) > 1 Then
                                lStr &= vbTab & "Check Difference"
                                lNumNoted += 1
                            End If
                            lNumChecked += 1
                        End If
                        lFileStr &= vbCrLf & lStr & vbCrLf

                        If lTotals Then
                            lStr = lNumNoted & " Noted Differences out of " & lNumChecked & " comparisons" & vbCrLf
                            If lNumNoted / lNumChecked < 0.1 Then
                                lStr &= "Less than 10% Noted Differences, Quality Check OK"
                            Else
                                lStr &= "More than 10% Noted Differences, Quality Check NOT OK!!!"
                            End If
                            lFileStr &= vbCrLf & lStr & vbCrLf
                        End If

                        lMonthData.Clear()
                        lMonthData = Nothing
                        ltssub = Nothing
                        lts = Nothing
                    Else
                        lStr &= vbCrLf & "Period of record shorter than PRISM - no comparison made "
                        Logger.Dbg("QACheck:  Period of record shorter than PRISM - no comparison made ")
                        lFileStr &= vbCrLf & lStr & vbCrLf
                    End If
                    'Next
                    lPrismDBF.Clear() ' = Nothing
                    lPrismDBF = Nothing
                    lStr = ""
                End If
            Next
            'Kill(lCurWDM)
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
        Next
        Logger.Dbg("QACheck:Completed QA Checking")
        If pDBFType = "mon" Then
            SaveFileString(pOutputPath & "QACheck_MonAnn_Normals.txt", lFileStr)
        Else
            SaveFileString(pOutputPath & "QACheck_MonAnn_All.txt", lFileStr)
        End If
        If lTotals Then SaveFileString(pOutputPath & "QACheck_Months.txt", lMonFile)

        'Application.Exit()

    End Sub

End Module
