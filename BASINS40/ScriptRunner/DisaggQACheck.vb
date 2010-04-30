Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports System.Math
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcMetCmp
Imports atcSeasons

Public Module ScriptDisaggQACheck
    Private Const pInputPath As String = "C:\BASINSMet\DisaggPrec\"
    Private Const pFilledPath As String = "C:\BASINSMet\WDMFilled\"
    Private Const pOutputPath As String = "C:\BASINSMet\QAResults\"
    Private Const pFormat As String = "#,##0.00"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("DisaggQACheck:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim ltssub As atcTimeseries
        Dim lHrTs As atcTimeseries
        Dim lDyTs As atcTimeseries = Nothing
        Dim lSeasonsMonth As atcSeasonsMonth = Nothing
        Dim lMonthData As atcDataGroup = Nothing
        Dim lSJDay As Double
        Dim lEJDay As Double
        Dim lMonStr As String = ""
        Dim i As Integer

        Dim lCurWDM As String = pInputPath & "current.wdm"
        Dim lPrismDBF As New atcTableDBF
        Dim lStation As String = ""
        Dim lStatePath As String = ""
        Dim lCons As String = ""
        Dim lFileStr As String = ""
        Dim lMonFile As String = ""
        Dim lFldVal As String = ""
        Dim lInd As Integer = 0
        Dim lMin As Double = 0
        Dim lMax As Double = 0
        Dim lCenter As Double = 0
        Dim lVal As Double = 0

        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lDBF As New atcTableDBF
        With lDBF
            .Year = CInt(Format(Now, "yyyy")) - 1900
            .Month = CByte(Format(Now, "mm"))
            .Day = CByte(Format(Now, "dd"))
            .NumFields = 14
            .FieldName(1) = "LOCATION"
            .FieldType(1) = "C"
            .FieldLength(1) = 10
            .FieldDecimalCount(1) = 0
            .FieldName(2) = "STANAM"
            .FieldType(2) = "C"
            .FieldLength(2) = 48
            .FieldDecimalCount(2) = 0
            .FieldName(3) = "STARTDATE"
            .FieldType(3) = "D"
            .FieldLength(3) = 9
            .FieldDecimalCount(3) = 0
            .FieldName(4) = "ENDDATE"
            .FieldType(4) = "D"
            .FieldLength(4) = 9
            .FieldDecimalCount(4) = 0
            .FieldName(5) = "DLY SUM"
            .FieldType(5) = "N"
            .FieldLength(5) = 9
            .FieldDecimalCount(5) = 0
            .FieldName(6) = "DLYANNAV"
            .FieldType(6) = "N"
            .FieldLength(6) = 9
            .FieldDecimalCount(6) = 0
            .FieldName(7) = "DIS SUM"
            .FieldType(7) = "N"
            .FieldLength(7) = 9
            .FieldDecimalCount(7) = 0
            .FieldName(8) = "DISANNAV"
            .FieldType(8) = "N"
            .FieldLength(8) = 9
            .FieldDecimalCount(8) = 0
            .FieldName(9) = "%DIFFSUM"
            .FieldType(9) = "N"
            .FieldLength(9) = 9
            .FieldDecimalCount(9) = 0
            .FieldName(10) = "%DIFFAVE"
            .FieldType(10) = "N"
            .FieldLength(10) = 9
            .FieldDecimalCount(10) = 0
            .FieldName(11) = "HLY SUM"
            .FieldType(11) = "N"
            .FieldLength(11) = 9
            .FieldDecimalCount(11) = 0
            .FieldName(12) = "HLYANNAV"
            .FieldType(12) = "N"
            .FieldLength(12) = 9
            .FieldDecimalCount(12) = 0
            .FieldName(13) = "%DIFFSUM"
            .FieldType(13) = "N"
            .FieldLength(13) = 9
            .FieldDecimalCount(13) = 0
            .FieldName(14) = "%DIFFAVE"
            .FieldType(14) = "N"
            .FieldLength(14) = 9
            .FieldDecimalCount(14) = 0
            .CurrentRecord = 1
        End With

        Logger.Dbg("DisaggQACheck: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("DisaggQACheck: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lFile)
            lStation = FilenameNoExt(FilenameNoPath(lFile))
            lStatePath = Right(PathNameOnly(lFile), 2) & "\"

            lts = lWDMfile.DataSets(0)
            lSJDay = lts.Attributes.GetValue("SJDay")
            lEJDay = lts.Attributes.GetValue("EJDay")
            lCons = lts.Attributes.GetValue("Constituent")
            Dim lWDMFilled As New atcWDM.atcDataSourceWDM
            lWDMFilled.Open(pFilledPath & lStatePath & lStation.Substring(0, 6) & ".wdm")
            lHrTs = Nothing
            If lWDMFilled.DataSets.IndexFromKey(100) >= 0 Then
                'hourly precip exists, try to find common date period for comparison
                lHrTs = lWDMFilled.DataSets.ItemByKey(100)
                If lHrTs.Attributes.GetValue("SJDay") < lSJDay AndAlso _
                   lHrTs.Attributes.GetValue("EJDay") > lEJDay Then
                    'find common period
                    If lHrTs.Attributes.GetValue("SJDay") > lSJDay Then
                        lSJDay = lHrTs.Attributes.GetValue("SJDay")
                    End If
                    If lHrTs.Attributes.GetValue("EJDay") < lEJDay Then
                        lEJDay = lHrTs.Attributes.GetValue("EJDay")
                    End If
                Else
                    lHrTs = Nothing
                End If
            End If
            For Each lDS As atcDataSet In lWDMFilled.DataSets
                If lDS.Attributes.GetValue("Constituent") = "PRCP" AndAlso lDS.Attributes.GetValue("tu") = atcTimeUnit.TUDay Then
                    lDyTs = SubsetByDate(lDS, lSJDay, lEJDay, Nothing)
                End If
            Next
            ltssub = SubsetByDate(lts, lSJDay, lEJDay, Nothing)
            lDBF.Value(1) = lts.Attributes.GetValue("Location")
            lDBF.Value(2) = lts.Attributes.GetValue("Stanam")
            lD2SStart.DateSeparator = ""
            lDBF.Value(3) = lD2SStart.JDateToString(lSJDay)
            lD2SEnd.DateSeparator = ""
            lDBF.Value(4) = lD2SEnd.JDateToString(lEJDay)
            lDBF.Value(5) = lDyTs.Attributes.GetValue("Sum")
            lDBF.Value(6) = lDyTs.Attributes.GetValue("SumAnnual")
            lDBF.Value(7) = ltssub.Attributes.GetValue("Sum")
            lDBF.Value(8) = ltssub.Attributes.GetValue("SumAnnual")
            lDBF.Value(9) = (lDBF.Value(5) - lDBF.Value(7)) / lDBF.Value(5) * 100
            lDBF.Value(10) = (lDBF.Value(6) - lDBF.Value(8)) / lDBF.Value(6) * 100
            If Not lHrTs Is Nothing Then
                lDBF.Value(11) = lHrTs.Attributes.GetValue("Sum")
                lDBF.Value(12) = lHrTs.Attributes.GetValue("SumAnnual")
                lDBF.Value(13) = (lDBF.Value(11) - lDBF.Value(7)) / lDBF.Value(11) * 100
                lDBF.Value(14) = (lDBF.Value(12) - lDBF.Value(8)) / lDBF.Value(12) * 100
            End If
            lWDMfile.DataSets.Clear()
            lWDMfile = Nothing
            lWDMFilled.DataSets.Clear()
            lWDMFilled = Nothing
            lDBF.CurrentRecord += 1
            Dim lPercent As String = "(" & DoubleToString((100 * lDBF.CurrentRecord) / lFiles.Count, , pFormat) & "%)"
            Logger.Dbg("DisaggQACheck:  Done " & lDBF.CurrentRecord & lPercent & lFile & " MemUsage " & MemUsage())
        Next
        Logger.Dbg("DisaggQACheck: Completed Disagg QA Checking")
        lDBF.WriteFile(pOutputPath & "DisaggQA.dbf")

        'Application.Exit()

    End Sub

    Private Function MemUsage() As String
        System.GC.WaitForPendingFinalizers()
        Return "MemoryUsage(MB):" & DoubleToString(Process.GetCurrentProcess.PrivateMemorySize64 / (2 ^ 20), , pFormat) & _
                    " Local(MB):" & DoubleToString(System.GC.GetTotalMemory(True) / (2 ^ 20), , pFormat)
    End Function
End Module
