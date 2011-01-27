Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Public Module QACheck
    Private Const pBasePath As String = "G:\Projects\TT_GCRP\MetDataFinalWithManualZip\"
    Private Const pOutputPath As String = pBasePath & "QA\"
    Private Const pInputPath As String = pBasePath & "Projects\"
    Private Const pPRISMPath As String = pOutputPath & "PRISM\"

    Private Const pDBFType As String = "ann" 'use "all" for each monthly and annual value as well as norms

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName
        Dim lOriginalDisplayMessages As Boolean = Logger.DisplayMessageBoxes

        ChDriveDir(pOutputPath)
        Logger.StartToFile("QACheck.log", False, False, True)
        Logger.Dbg("QACheck: Start")

        Dim lMin As Double = 0
        Dim lMax As Double = 0
        Dim lCenter As Double = 0
        Dim lVal As Double = 0
        Dim lPctDiff As Double = 0
        Dim lAbsDiff As Double = 0

        Dim lSpan As Integer = 36
        Dim lSDate() As Integer = {0, 1, 1, 0, 0, 0}
        lSDate(0) = 1970
        Dim lSJDay As Double = Date2J(lSDate)
        Dim lEDate() As Integer = {0, 12, 31, 24, 0, 0}
        lEDate(0) = lSDate(0) + lSpan - 1
        Dim lEJDay As Double = Date2J(lEDate)

        Logger.Dbg("QACheck: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("QACheck: Found " & lFiles.Count & " data files")

        Dim lFileStr As String = ""
        Dim lMonFile As String = ""
        Dim lPrismPptDbf As New atcTableDBF
        lPrismPptDbf.OpenFile(pPRISMPath & "us_ppt_ann.dbf")
        Logger.Dbg("PPT:RecordCount " & lPrismPptDbf.NumRecords & " in " & lPrismPptDbf.FileName)
        Dim lPrismTmaxDbf As New atcTableDBF
        lPrismTmaxDbf.OpenFile(pPRISMPath & "us_tmax_ann.dbf")
        Logger.Dbg("TMAX:RecordCount " & lPrismTmaxDbf.NumRecords & " in " & lPrismTmaxDbf.FileName)
        Dim lPrismTminDbf As New atcTableDBF
        lPrismTminDbf.OpenFile(pPRISMPath & "us_tmin_ann.dbf")
        Logger.Dbg("TMIN:RecordCount " & lPrismTminDbf.NumRecords & " in " & lPrismTminDbf.FileName)

        Dim lNumNoted As Integer = 0
        Dim lNumChecked As Integer = 0
        Dim lNumSkipped As Integer = 0

        For Each lFile As String In lFiles
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            If lWDMfile.Open(lFile) Then
                Logger.Dbg("Process " & lFile & " with " & lWDMfile.DataSets.Count & " datasets")

                For Each lDS As atcDataSet In lWDMfile.DataSets
                    Dim lStation As String = lDS.Attributes.GetValue("Location").ToString.Substring(2)
                    Dim lCons As String = lDS.Attributes.GetValue("Constituent")
                    If lCons = "PREC" Then ' OrElse lCons.StartsWith("ATEM") Then
                        Dim lStr As String = "Station " & lStation & " comparing " & lCons & " for period " & DumpDate(lSJDay).Substring(0, 10) & " to " & DumpDate(lEJDay).Substring(0, 10) & vbCrLf
                        Logger.Dbg("QACheck: " & lStr.Trim(vbCrLf))
                        Dim lPrismDBF As atcTableDBF
                        If lCons = "TMAX" Then
                            lPrismDBF = lPrismTmaxDbf
                        ElseIf lCons = "TMIN" Then
                            lPrismDBF = lPrismTminDbf
                        Else
                            lPrismDBF = lPrismPptDbf
                        End If
                        If lDS.Attributes.GetValue("SJDay") <= lSJDay AndAlso _
                           lDS.Attributes.GetValue("EJDay") >= lEJDay Then
                            Dim lTsSubset As atcTimeseries = SubsetByDate(lDS, lSJDay, lEJDay, Nothing)
                            Dim lTs As atcTimeseries
                            If lCons = "PREC" Then 'sum precip values
                                lTs = Aggregate(lTsSubset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                            Else 'average temperature values
                                lTs = Aggregate(lTsSubset, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                            End If

                            Dim lFldVal As String = ""
                            Dim lInd As Integer = 0

                            lStr &= "  Annual Ave" & vbTab
                            If lCons = "PREC" Then ' precip values
                                lFldVal = "us_ppt.14"
                            ElseIf lCons = "TMIN" Then
                                lFldVal = "us_tmin.14"
                            ElseIf lCons = "TMAX" Then
                                lFldVal = "us_tmax.14"
                            End If
                            If lPrismDBF.FindFirst(1, lStation) Then
                                While lPrismDBF.Value(4) <> lFldVal
                                    lPrismDBF.FindNext(1, lStation)
                                End While
                                Try
                                    For lIndex As Integer = 5 To 7
                                        lStr &= DoubleToString(lPrismDBF.Value(lIndex), , "##0.00", , , 5) & vbTab
                                    Next
                                    lVal = lTs.Attributes.GetValue("Mean")
                                    lStr &= DoubleToString(lVal, , "##0.00", , , 4) & vbTab
                                    lPctDiff = 100 * ((lVal - lPrismDBF.Value(6)) / lPrismDBF.Value(6))
                                    lAbsDiff = lVal - lPrismDBF.Value(6)
                                    lStr &= DoubleToString(lPctDiff, , "#0.0", , , 3) & vbTab & DoubleToString(lAbsDiff, , "#0.0", , , 3)
                                    If System.Math.Abs(lPctDiff) > 10 AndAlso System.Math.Abs(lAbsDiff) > 1 Then
                                        lStr &= vbTab & "Check Difference"
                                        lNumNoted += 1
                                    Else
                                        lStr &= vbTab & "OK"
                                    End If
                                    Logger.Dbg("  " & lStr)
                                    lNumChecked += 1
                                Catch lEx As Exception
                                    Logger.Dbg("  Problem" & lEx.ToString)
                                End Try
                            End If
                            lFileStr &= lStr & vbCrLf

                            lTsSubset = Nothing
                            lTs = Nothing
                        Else
                            lStr &= vbCrLf & "Period of record shorter than PRISM - no comparison made "
                            Logger.Dbg("QACheck:  Period of record shorter than PRISM - no comparison made ")
                            lFileStr &= lStr & vbCrLf
                            lNumSkipped += 1
                        End If
                        lStr = ""
                    End If
                Next
                lWDMfile.DataSets.Clear()
                lWDMfile = Nothing
            End If
        Next
        Logger.Dbg("Done,Checked " & lNumChecked & " Noted " & lNumNoted & " Skipped " & lNumSkipped)
        SaveFileString(pOutputPath & "QACheck_Ann_All.txt", lFileStr)

        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("QACheckDone")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("QACheckDone")
        Logger.DisplayMessageBoxes = lOriginalDisplayMessages
    End Sub

End Module
