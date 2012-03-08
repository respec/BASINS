Imports atcUtility
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports atcMetCmp
Imports atcData
Imports atcWDM
Imports atcGraph
Imports ZedGraph
Imports Microsoft.Office.Interop

Module Util_HydroFrack
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lTask As Integer = 6
        Select Case lTask
            Case 1 : ConstructHuc8BasedWaterUseFile() ''Task1. get huc8 based water use
            Case 2 : ClassifyWaterYearsForGraph()
            Case 3 : SwapInCBPFlowIntoGCRPRunWDMs() 'turns out the expert system is using the ID 2 as simulated flow
            Case 4 : DurationPlotGCRPvsCBP()
            Case 5 : ClassifyWaterYearsPrecipForGraph()
            Case 6 : ConstructFTableColumnTimeseries()
        End Select
    End Sub

    Private Sub ConstructFTableColumnTimeseries()

        Dim lTsDates As New atcTimeseries(Nothing)
        Dim lDateStart As Double = Date2J(1985, 1, 1, 0, 0, 0)
        Dim lDateEnd As Double = Date2J(2005, 12, 31, 24, 0, 0)
        lTsDates.Values = NewDates(lDateStart, lDateEnd, atcTimeUnit.TUDay, 1)

        Dim lTsDSN1 As New atcTimeseries(Nothing)
        With lTsDSN1
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 1)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lTsDSN2 As New atcTimeseries(Nothing)
        With lTsDSN2
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 2)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lTsDSN3 As New atcTimeseries(Nothing)
        With lTsDSN3
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 3)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lTsDSN4 As New atcTimeseries(Nothing)
        With lTsDSN4
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 4)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lTsDSN5 As New atcTimeseries(Nothing)
        With lTsDSN5
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 5)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lTsDSN6 As New atcTimeseries(Nothing)
        With lTsDSN6
            .Dates = lTsDates
            .numValues = .Dates.numValues
            .SetInterval(atcTimeUnit.TUDay, 1)
            .Attributes.SetValue("ID", 6)
            .Attributes.SetValue("TSTYP", "RCOL")
            .Attributes.SetValue("Constituent", "RCOL")
        End With

        Dim lDate(5) As Integer
        Dim lYear As Integer
        Dim lMonth As Integer
        Dim lDay As Integer
        'Construct DSN1 (SJ4_2060, SJ4_2360, 20503, R3, R6)
        'Start with 4
        'change 4-5 05/14 - 05/15
        'change 5-4 11/14 - 11/15
        'no sliding in transition
        With lTsDSN1
            For I As Integer = 0 To .Dates.numValues
                J2Date(.Dates.Value(I), lDate)
                lMonth = lDate(1) : lDay = lDate(2)
                If lMonth < 5 Then
                    .Value(I + 1) = 4.0
                ElseIf lMonth = 5 Then
                    If lDay <= 14 Then
                        .Value(I + 1) = 4.0
                    Else
                        .Value(I + 1) = 5.0
                    End If
                ElseIf lMonth < 11 Then
                    .Value(I + 1) = 5.0
                ElseIf lMonth = 11 Then
                    If lDay <= 14 Then
                        .Value(I + 1) = 5.0
                    Else
                        .Value(I + 1) = 4.0
                    End If
                Else
                    .Value(I + 1) = 4.0
                End If
            Next
        End With

        'Construct DSN2 (SU2_0741, 20501, R35)
        'Start with 4
        'change 4-5 1989/11/1 - 1990/6/1
        'need to slide scale
        Dim lDate1 As Double = Date2J(1989, 11, 1, 0, 0, 0)
        Dim lDate2 As Double = Date2J(1990, 6, 1, 24, 0, 0)
        With lTsDSN2
            For I As Integer = 0 To .Dates.numValues
                If .Dates.Value(I) < lDate1 Then
                    .Value(I + 1) = 4.0
                ElseIf .Dates.Value(I) >= lDate1 AndAlso .Dates.Value(I) <= lDate2 Then
                    'sliding happens here
                    .Value(I + 1) = (.Dates.Value(I) - lDate1) / (lDate2 - lDate1) + 4.0
                Else
                    .Value(I + 1) = 5.0
                End If
            Next
        End With

        'Construct DSN3 (SU3_0240, 20501, R93)
        'Start with 4
        'change 4-5 04/10 - 05/20 '41 days transition
        'change 5-4 12/01 - 12/10 '10 days transition
        'sliding in transition
        With lTsDSN3
            For I As Integer = 0 To .Dates.numValues
                J2Date(.Dates.Value(I), lDate)
                lMonth = lDate(1) : lDay = lDate(2)
                If lMonth < 4 Then
                    .Value(I + 1) = 4.0
                ElseIf lMonth = 4 Then
                    If lDay <= 10 Then
                        .Value(I + 1) = 4.0
                    Else
                        'start sliding 4-5
                        .Value(I + 1) = 4.0 + (lDay - 10 + 1) / 41.0
                    End If
                ElseIf lMonth = 5 Then
                    If lDay <= 20 Then
                        'sliding 4-5
                        .Value(I + 1) = 4.0 + (lDay + 21.0) / 41.0
                    Else
                        .Value(I + 1) = 5.0
                    End If
                ElseIf lMonth < 12 Then
                    .Value(I + 1) = 5.0
                ElseIf lMonth = 12 Then
                    If lDay >= 1 AndAlso lDay <= 10 Then
                        'sliding 5-4
                        .Value(I + 1) = 5.0 - lDay / 10.0
                    Else
                        .Value(I + 1) = 4.0
                    End If
                End If
            Next
        End With

        'Construct DSN4 (SU2_0030, SU2_0291, 20501, R112, R117)
        'Start with 4
        'change 4-5 04/25 - 05/15 '21 days transition
        'change 5-4 11/28 - 12/12 '15 days transition
        'sliding in transition
        With lTsDSN4
            For I As Integer = 0 To .Dates.numValues
                J2Date(.Dates.Value(I), lDate)
                lMonth = lDate(1) : lDay = lDate(2)
                If lMonth < 4 Then
                    .Value(I + 1) = 4.0
                ElseIf lMonth = 4 Then
                    If lDay <= 25 Then
                        .Value(I + 1) = 4.0
                    Else
                        'start sliding 4-5
                        .Value(I + 1) = 4.0 + (lDay - 25 + 1) / 21.0
                    End If
                ElseIf lMonth = 5 Then
                    If lDay <= 15 Then
                        'sliding 4-5
                        .Value(I + 1) = 4.0 + (lDay + 6.0) / 21.0
                    Else
                        .Value(I + 1) = 5.0
                    End If
                ElseIf lMonth < 11 Then
                    .Value(I + 1) = 5.0
                ElseIf lMonth = 11 Then
                    If lDay <= 28 Then
                        .Value(I + 1) = 5.0
                    Else
                        .Value(I + 1) = 5.0 - (lDay - 28 + 1) / 15.0
                    End If
                ElseIf lMonth = 12 Then
                    If lDay >= 1 AndAlso lDay <= 12 Then
                        'sliding 5-4
                        .Value(I + 1) = 5.0 - (lDay + 3.0) / 15.0
                    Else
                        .Value(I + 1) = 4.0
                    End If
                End If
            Next
        End With

        'Construct DSN6 (SW4_1860, 20502, R47)
        'Start with 4
        'change 4-5 04/23 - 05/30 '38 days transition
        'change 5-4 11/15 - 12/01 '17 days transition
        'sliding in transition
        'only through 1996
        With lTsDSN6
            For I As Integer = 0 To .Dates.numValues
                J2Date(.Dates.Value(I), lDate)
                lYear = lDate(0) : lMonth = lDate(1) : lDay = lDate(2)
                If lYear <= 1996 Then
                    If lMonth < 4 Then
                        .Value(I + 1) = 4.0
                    ElseIf lMonth = 4 Then
                        If lDay <= 23 Then
                            .Value(I + 1) = 4.0
                        Else
                            'start sliding 4-5
                            .Value(I + 1) = 4.0 + (lDay - 23.0 + 1) / 38.0
                        End If
                    ElseIf lMonth = 5 Then
                        If lDay <= 30 Then
                            'sliding 4-5
                            .Value(I + 1) = 4.0 + (lDay + 8.0) / 38.0
                        Else
                            .Value(I + 1) = 5.0
                        End If
                    ElseIf lMonth < 11 Then
                        .Value(I + 1) = 5.0
                    ElseIf lMonth = 11 Then
                        If lDay <= 15 Then
                            .Value(I + 1) = 5.0
                        Else
                            .Value(I + 1) = 5.0 - (lDay - 15.0 + 1) / 17.0
                        End If
                    ElseIf lMonth = 12 Then
                        If lDay = 1 Then
                            'sliding 5-4
                            .Value(I + 1) = 5.0 - (lDay + 16.0) / 17.0
                        Else
                            .Value(I + 1) = 4.0
                        End If
                    End If
                Else 'starting from 1997 and on
                    .Value(I + 1) = 4.0
                End If
            Next
        End With

        'Construct DSN5 (SW3_1690_2222, SW3_1690_1660, 20502, R23)
        'Start with 4
        'change 4-6 04/01 - 07/01 '92 days transition
        'change 6-4 10/01 - 12/01 '62 days transition
        'sliding in transition
        'only through 1993

        'start in 1994
        'start with 5
        'change 5-4 02/15 - 03/01 'var days transition
        'change 4-6 04/01 - 05/15 '45 days transition
        'change 6-5 11/15 - 12/01 '17 days transition
        Dim lDayDiff As Double = 0.0
        With lTsDSN5
            For I As Integer = 0 To .Dates.numValues
                J2Date(.Dates.Value(I), lDate)
                lYear = lDate(0) : lMonth = lDate(1) : lDay = lDate(2)
                If lYear <= 1993 Then
                    If lMonth < 4 Then
                        .Value(I + 1) = 4.0
                    ElseIf lMonth = 4 Then
                        If lDay = 1 Then
                            .Value(I + 1) = 4.0
                        Else
                            'start sliding 4-6
                            .Value(I + 1) = 4.0 + lDay * (6.0 - 4.0) / 92.0
                        End If
                    ElseIf lMonth < 7 Then
                        'sliding 4-6
                        .Value(I + 1) = 4.0 + (DateDiff(DateInterval.Day, New Date(lYear, 4, 1), New Date(lYear, lMonth, lDay)) + 1.0) * (6.0 - 4.0) / 92.0
                    ElseIf lMonth = 7 Then
                        .Value(I + 1) = 6.0
                    ElseIf lMonth < 10 Then
                        .Value(I + 1) = 6.0
                    ElseIf lMonth >= 10 AndAlso lMonth < 12 Then
                        If lMonth = 10 AndAlso lDay = 1 Then
                            .Value(I + 1) = 6.0
                        Else
                            .Value(I + 1) = 6.0 - (DateDiff(DateInterval.Day, New Date(lYear, 10, 1), New Date(lYear, lMonth, lDay)) + 1.0) * (6.0 - 4.0) / 62.0
                        End If
                    ElseIf lMonth = 12 Then
                        .Value(I + 1) = 4.0
                    End If
                Else 'lYear >= 1994
                    If lMonth < 2 Then
                        .Value(I + 1) = 5.0
                    ElseIf lMonth = 2 Then
                        If lDay <= 15 Then
                            .Value(I + 1) = 5.0
                        Else
                            'start sliding 5 - 4
                            .Value(I + 1) = 5.0 - (lDay - 15.0 + 1) / (DayMon(lYear, 2) - 15.0 + 1 + 1)
                        End If
                    ElseIf lMonth = 3 Then
                        .Value(I + 1) = 4.0
                    ElseIf lMonth = 4 Then
                        If lDay = 1 Then
                            .Value(I + 1) = 4.0
                        Else
                            'sliding 4 - 6 till 5/15
                            .Value(I + 1) = 4.0 + lDay * (6.0 - 4.0) / 45.0
                        End If
                    ElseIf lMonth = 5 Then
                        If lDay <= 15 Then
                            'sliding 4-6
                            .Value(I + 1) = 4.0 + (lDay + 30.0) * (6.0 - 4.0) / 45.0
                        Else
                            .Value(I + 1) = 6.0
                        End If
                    ElseIf lMonth < 11 Then
                        .Value(I + 1) = 6.0
                    ElseIf lMonth = 11 Then
                        If lDay <= 15 Then
                            .Value(I + 1) = 6.0
                        Else
                            'start sliding 6 - 5 till 12/1
                            .Value(I + 1) = 6.0 - (lDay - 15.0 + 1) / 17.0
                        End If
                    ElseIf lMonth = 12 Then
                        .Value(I + 1) = 5.0
                    End If
                End If
            Next
        End With

        Dim lSusqTransWDMFilename As String = "G:\Admin\GCRPSusq\RunsWithReservoirs\parms\SusqTrans.wdm"
        Dim lSusqTransWDM As New atcWDM.atcDataSourceWDM()
        With lSusqTransWDM
            If Not .Open(lSusqTransWDMFilename) Then Exit Sub

            'Write Variable FTable column timeseries into this WDM
            If Not .AddDataset(lTsDSN1, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN1 failed.")
            End If
            If Not .AddDataset(lTsDSN2, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN2 failed.")
            End If
            If Not .AddDataset(lTsDSN3, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN3 failed.")
            End If
            If Not .AddDataset(lTsDSN4, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN4 failed.")
            End If
            If Not .AddDataset(lTsDSN5, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN5 failed.")
            End If
            If Not .AddDataset(lTsDSN6, atcDataSource.EnumExistAction.ExistReplace) Then
                Logger.Dbg("Add TSDSN6 failed.")
            End If
            .Clear()
        End With
        lSusqTransWDM = Nothing
    End Sub

    Private Sub ConstructHuc8BasedWaterUseFile()
        '*** Awuds Excel data
        Dim lExcelFipsFieldIndex As Integer = 4
        Dim l2000DataElements() As Integer = {7, 8, 9, 11, 12, 13, 14, 17, 20, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 39, 42, 45, 46, 49, 62, 65, 68}
        Dim l2005DataElements() As Integer = {9, 12, 15, 19, 20, 21, 22, 23, 24, 27, 30, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 46, 47, 48, 49, 53, 54, 55, 56, 57, 58, 59, 60, 63, 66, 69, 72, 75, 99, 102, 105}
        Dim lDataYear() As Integer = {2000, 2005}
        Dim lStates() As String = {"mdco", "nyco", "paco"}
        Dim lAwudsDataDirectory As String = "G:\Admin\EPA_HydroFrac_HSPFEval\WaterUse\"
        Dim lAwudsDataFile As String = ""
        Dim lxlApp As Excel.Application = Nothing
        Dim lxlWorkbook As Excel.Workbook = Nothing
        Dim lxlWorkbookPA As Excel.Workbook = Nothing
        Dim lxlWorkbookMD As Excel.Workbook = Nothing
        Dim lxlWorkbookNY As Excel.Workbook = Nothing
        Dim lxlSheet As Excel.Worksheet = Nothing
        'Dim lStateList As New atcCollection()
        'With lStateList
        '    .Add("24", "md")
        '    .Add("36", "ny")
        '    .Add("42", "pa")
        'End With

        '*** 
        Dim File1 As String = lAwudsDataDirectory & "HUCbyCounty.txt"
        Dim File2 As String = lAwudsDataDirectory & "County.txt"

        Dim lYearToProcess As Integer = lDataYear(1)
        Dim lFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(File1), "HUCbyCountyWaterUse" & lYearToProcess & ".txt")

        Dim lOneLine As String
        Dim lArrCounty() As String
        Dim lArrHucbyCounty() As String
        Dim lFips As String = ""
        Dim lAreaTotal As Double = 0
        Dim lAreaPartial As Double = 0
        Dim lAreaFraction As Double = 0
        Dim lLinebuilder As New Text.StringBuilder

        'construct county fips-area (sq meter) dictionary
        Dim lCountyList As New atcCollection()
        Dim lSRCounty As New StreamReader(File2)
        While Not lSRCounty.EndOfStream
            lOneLine = lSRCounty.ReadLine()
            lArrCounty = Regex.Split(lOneLine, "\s+")
            lFips = lArrCounty(5)
            Dim lArea As Double = Double.Parse(lArrCounty(13))
            If Not lCountyList.Keys.Contains(lFips) Then
                lCountyList.Add(lFips, lArea)
            End If
        End While
        lSRCounty.Close()
        lSRCounty = Nothing

        lxlApp = New Excel.Application()
        lAwudsDataFile = lAwudsDataDirectory & "mdco" & lYearToProcess & ".xls"
        lxlWorkbookMD = lxlApp.Workbooks.Open(lAwudsDataFile)
        lAwudsDataFile = lAwudsDataDirectory & "paco" & lYearToProcess & ".xls"
        lxlWorkbookPA = lxlApp.Workbooks.Open(lAwudsDataFile)
        lAwudsDataFile = lAwudsDataDirectory & "nyco" & lYearToProcess & ".xls"
        lxlWorkbookNY = lxlApp.Workbooks.Open(lAwudsDataFile)

        Dim lSWHucByCountyData As New StreamWriter(lFile, False)
        Dim lSRHucByCounty As New StreamReader(File1)
        While Not lSRHucByCounty.EndOfStream
            lOneLine = lSRHucByCounty.ReadLine
            lArrHucbyCounty = Regex.Split(lOneLine, "\s+")
            lFips = lArrHucbyCounty(3)
            If lCountyList.Keys.Contains(lFips) Then
                lAreaTotal = lCountyList.ItemByKey(lFips)
            Else
                lAreaTotal = -99.9
            End If
            If lAreaTotal < 0 Then Continue While
            If Double.TryParse(lArrHucbyCounty(4), lAreaPartial) Then
                lAreaFraction = lAreaPartial / lAreaTotal
            Else
                Continue While
            End If

            lLinebuilder.Append(lOneLine & " ")
            lLinebuilder.Append(String.Format("{0:0.0}", lAreaTotal) & " ")
            lLinebuilder.Append(String.Format("{0:0.00}", lAreaFraction) & " ")

            'search for data
            Select Case lFips.Substring(0, 2)
                Case "24" : lxlWorkbook = lxlWorkbookMD
                Case "36" : lxlWorkbook = lxlWorkbookNY
                Case "42" : lxlWorkbook = lxlWorkbookPA
            End Select

            Dim lDataElements() As Integer = Nothing
            If lYearToProcess = 2000 Then
                lxlSheet = lxlWorkbook.Worksheets("Data")
                lDataElements = l2000DataElements
            ElseIf lYearToProcess = 2005 Then
                lDataElements = l2005DataElements
                lxlSheet = lxlWorkbook.Worksheets("County")
            End If

            With lxlSheet
                For lRow As Integer = 1 To .UsedRange.Rows.Count
                    If lFips = .Cells(lRow, lExcelFipsFieldIndex).Value Then
                        Dim lValue As Double = 0
                        For I As Integer = 0 To lDataElements.Length - 1
                            lValue = .Cells(lRow, lDataElements(I)).Value * lAreaFraction
                            lLinebuilder.Append(DoubleToString(lValue) & " ")
                        Next
                        Exit For
                    End If
                Next
            End With

            lSWHucByCountyData.WriteLine(lLinebuilder.ToString)
            lLinebuilder.Length = 0
        End While
        lSRHucByCounty.Close()
        lSRHucByCounty = Nothing

        lSWHucByCountyData.Flush()
        lSWHucByCountyData.Close()
        lSWHucByCountyData = Nothing

        lxlWorkbookMD.Close()
        lxlWorkbookNY.Close()
        lxlWorkbookPA.Close()
        Try
            lxlWorkbook.Close()
        Catch ex As Exception

        End Try
        lxlApp.Quit()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlSheet)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlWorkbook)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlWorkbookMD)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlWorkbookNY)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlWorkbookPA)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lxlApp)

        lxlSheet = Nothing
        lxlWorkbook = Nothing
        lxlWorkbookMD = Nothing
        lxlWorkbookNY = Nothing
        lxlWorkbookPA = Nothing
        lxlApp = Nothing
    End Sub

    Private Sub ClassifyCalendarYears()
        Dim lUADepth As Double = 0.03719 'To be multiplied by drainage area in square miles to convert to depth inch
        Dim lWorkDirClassifyYear As String = "G:\Admin\EPA_HydroFrac_HSPFEval\ClassifyYears\"
        Dim lDataDir As String = lWorkDirClassifyYear & "NWIS\"
        Dim lWDMFiles As New atcCollection() 'collection of wdm and its drainage area
        With lWDMFiles
            .Add(lDataDir & "danville.wdm", 11220.0)
            .Add(lDataDir & "marietta.wdm", 25990.0)
            .Add(lDataDir & "raystown.wdm", 796.0)
            .Add(lDataDir & "westbrsusq.wdm", 6847.0)
        End With
        Dim lPercentiles() As Integer = {10, 25, 75, 90}
        Dim lClassifyYearLog As String = lWorkDirClassifyYear & "ClassifyYearLog.txt"
        Dim lSW As New StreamWriter(lClassifyYearLog, False)
        Dim lWDMSource As atcWDM.atcDataSourceWDM = Nothing
        For Each lWDMFile As String In lWDMFiles.Keys
            lWDMSource = New atcWDM.atcDataSourceWDM()
            If Not lWDMSource.Open(lWDMFile) Then Continue For
            Dim lConversionFactor As Double = lUADepth / lWDMFiles.ItemByKey(lWDMFile)
            Dim lTs As atcTimeseries = lWDMSource.DataSets(0)
            Dim lCons As String = lTs.Attributes.GetValue("Constituent")
            Dim lTsAnnual As atcTimeseries = Aggregate(lTs, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
            Dim lTsAnnualDepth As atcTimeseries = Nothing
            lTsAnnualDepth = Aggregate(lTs, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)

            Dim lUnitAverage As String = ""
            Dim lUnitSum As String = ""
            If lCons.ToLower.Contains("flow") Then
                lTsAnnualDepth = lTsAnnualDepth * lConversionFactor
                lUnitAverage = "(cfs)"
                lUnitSum = "(in)"
            ElseIf lCons.ToLower.Contains("prec") Then
                lUnitAverage = "(in)"
                lUnitSum = "(in)"
            End If

            Dim lPercentilesAnnualAverage As New atcCollection()
            With lPercentilesAnnualAverage
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsAnnual.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With
            Dim lPercentilesAnnualSum As New atcCollection()
            With lPercentilesAnnualSum
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsAnnualDepth.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With
            lSW.WriteLine("Processing Data: " & lTs.Attributes.GetValue("History 1"))
            lSW.WriteLine("Classification based on Annual Average " & lUnitAverage & " and Sum " & lUnitSum & " " & lCons & " percentiles")
            lSW.WriteLine("Percentile,Avg,Sum")
            lSW.WriteLine("%," & lUnitAverage & "," & lUnitSum)
            lSW.WriteLine("-------------------")
            For Each lPct As Integer In lPercentilesAnnualAverage.Keys
                lSW.Write(lPct & ",")
                lSW.Write(String.Format("{0:0.00}", lPercentilesAnnualAverage.ItemByKey(lPct)) & ",")
                lSW.WriteLine(String.Format("{0:0.00}", lPercentilesAnnualSum.ItemByKey(lPct)))
            Next
            Dim lDate(5) As Integer
            Dim lCategory As String = ""
            Dim lValue As Double
            lSW.WriteLine(" ")
            lSW.WriteLine("Year,Avg,Class,Sum,Class")
            lSW.WriteLine("    " & "," & lUnitAverage & ", ," & lUnitSum & ", ,")
            lSW.WriteLine("----,---,-----,---,------")
            For lYCount As Integer = 1 To lTsAnnual.numValues
                J2Date(lTsAnnual.Dates.Value(lYCount - 1), lDate)
                lValue = lTsAnnual.Value(lYCount)
                If lValue <= lPercentilesAnnualAverage.ItemByKey(10) Then
                    lCategory = "Drought"
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(10) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(25) Then
                    lCategory = "Dry"
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(25) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(75) Then
                    lCategory = "Normal"
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(75) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(90) Then
                    lCategory = "Wet"
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(90) Then
                    lCategory = "Very Wet"
                End If
                lSW.Write(lDate(0) & "," & String.Format("{0:0.00}", lValue) & "," & lCategory & ",")

                lValue = lTsAnnualDepth.Value(lYCount)
                If lValue <= lPercentilesAnnualSum.ItemByKey(10) Then
                    lCategory = "Drought"
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(10) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(25) Then
                    lCategory = "Dry"
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(25) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(75) Then
                    lCategory = "Normal"
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(75) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(90) Then
                    lCategory = "Wet"
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(90) Then
                    lCategory = "Very Wet"
                End If
                lSW.WriteLine(String.Format("{0:0.00}", lValue) & "," & lCategory)
            Next
            lWDMSource.Clear()
            lWDMSource = Nothing
            lSW.WriteLine(" ")
            lSW.WriteLine(" ")
            lTsAnnual.Clear() : lTsAnnual = Nothing
            lTsAnnualDepth.Clear() : lTsAnnualDepth = Nothing
            lPercentilesAnnualAverage.Clear() : lPercentilesAnnualAverage = Nothing
            lPercentilesAnnualSum.Clear() : lPercentilesAnnualSum = Nothing
            lSW.Flush()
        Next

        lSW.Close()
        lSW = Nothing
    End Sub

    Private Sub ClassifyCalendarYearsForGraph()
        Dim lUADepth As Double = 0.03719 'To be multiplied by drainage area in square miles to convert to depth inch
        Dim lWorkDirClassifyYear As String = "G:\Admin\EPA_HydroFrac_HSPFEval\ClassifyYears\"
        Dim lDataDir As String = lWorkDirClassifyYear & "NWIS\"
        Dim lWDMFiles As New atcCollection() 'collection of wdm and its drainage area
        With lWDMFiles
            .Add(lDataDir & "danville.wdm", 11220.0)
            .Add(lDataDir & "marietta.wdm", 25990.0)
            .Add(lDataDir & "raystown.wdm", 796.0)
            .Add(lDataDir & "westbrsusq.wdm", 6847.0)
        End With
        Dim lPercentiles() As Integer = {10, 25, 75, 90}
        Dim lClassifyYearLog As String = lWorkDirClassifyYear & "ClassifyYearForGraphLog.txt"
        Dim lDelim As String = ","
        Dim lSW As New StreamWriter(lClassifyYearLog, False)
        Dim lWDMSource As atcWDM.atcDataSourceWDM = Nothing
        For Each lWDMFile As String In lWDMFiles.Keys
            lWDMSource = New atcWDM.atcDataSourceWDM()
            If Not lWDMSource.Open(lWDMFile) Then Continue For
            Dim lConversionFactor As Double = lUADepth / lWDMFiles.ItemByKey(lWDMFile)
            Dim lTs As atcTimeseries = lWDMSource.DataSets(0)
            Dim lCons As String = lTs.Attributes.GetValue("Constituent")
            Dim lTsAnnual As atcTimeseries = Aggregate(lTs, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
            Dim lTsAnnualDepth As atcTimeseries = Nothing
            lTsAnnualDepth = Aggregate(lTs, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)

            Dim lUnitAverage As String = ""
            Dim lUnitSum As String = ""
            If lCons.ToLower.Contains("flow") Then
                lTsAnnualDepth = lTsAnnualDepth * lConversionFactor
                lUnitAverage = "(cfs)"
                lUnitSum = "(in)"
            ElseIf lCons.ToLower.Contains("prec") Then
                lUnitAverage = "(in)"
                lUnitSum = "(in)"
            End If

            Dim lPercentilesAnnualAverage As New atcCollection()
            With lPercentilesAnnualAverage
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsAnnual.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With
            Dim lPercentilesAnnualSum As New atcCollection()
            With lPercentilesAnnualSum
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsAnnualDepth.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With
            lSW.WriteLine("Processing Data: " & lTs.Attributes.GetValue("History 1"))
            lSW.WriteLine("Classification based on Annual Average " & lUnitAverage & " and Sum " & lUnitSum & " " & lCons & " percentiles")
            lSW.WriteLine("Percentile,Avg,Sum")
            lSW.WriteLine("%," & lUnitAverage & "," & lUnitSum)
            lSW.WriteLine("-------------------")
            For Each lPct As Integer In lPercentilesAnnualAverage.Keys
                lSW.Write(lPct & ",")
                lSW.Write(String.Format("{0:0.00}", lPercentilesAnnualAverage.ItemByKey(lPct)) & ",")
                lSW.WriteLine(String.Format("{0:0.00}", lPercentilesAnnualSum.ItemByKey(lPct)))
            Next
            Dim lDate(5) As Integer
            Dim lCategory As String = ""
            Dim lValue As Double
            lSW.WriteLine(" ")
            lSW.WriteLine("Listing of Annual Averages " & lUnitAverage & " for different categories")
            lSW.WriteLine("Year,Drought,Dry,Normal,Wet,Very Wet")
            Dim lTsGroupAnnualAvg As New atcTimeseriesGroup()
            Dim lTsAnnAvgDrought As atcTimeseries = lTsAnnual.Clone()
            Dim lTsAnnAvgDry As atcTimeseries = lTsAnnual.Clone()
            Dim lTsAnnAvgNormal As atcTimeseries = lTsAnnual.Clone()
            Dim lTsAnnAvgWet As atcTimeseries = lTsAnnual.Clone()
            Dim lTsAnnAvgVeryWet As atcTimeseries = lTsAnnual.Clone()

            For I As Integer = 1 To lTsAnnual.numValues
                lTsAnnAvgDrought.Value(I) = GetNaN()
                lTsAnnAvgDry.Value(I) = GetNaN()
                lTsAnnAvgNormal.Value(I) = GetNaN()
                lTsAnnAvgWet.Value(I) = GetNaN()
                lTsAnnAvgVeryWet.Value(I) = GetNaN()
            Next
            With lTsGroupAnnualAvg
                .Add(lTsAnnAvgDrought)
                .Add(lTsAnnAvgDry)
                .Add(lTsAnnAvgNormal)
                .Add(lTsAnnAvgWet)
                .Add(lTsAnnAvgVeryWet)
            End With

            lTsAnnAvgDrought.Attributes.SetValue("Point", True)
            lTsAnnAvgDry.Attributes.SetValue("Point", True)
            lTsAnnAvgNormal.Attributes.SetValue("Point", True)
            lTsAnnAvgWet.Attributes.SetValue("Point", True)
            lTsAnnAvgVeryWet.Attributes.SetValue("Point", True)

            lTsAnnAvgDrought.Attributes.SetValue("Class", "Drought")
            lTsAnnAvgDry.Attributes.SetValue("Class", "Dry")
            lTsAnnAvgNormal.Attributes.SetValue("Class", "Normal")
            lTsAnnAvgWet.Attributes.SetValue("Class", "Wet")
            lTsAnnAvgVeryWet.Attributes.SetValue("Class", "Very Wet")

            For lYCount As Integer = 1 To lTsAnnual.numValues
                J2Date(lTsAnnual.Dates.Value(lYCount - 1), lDate)
                lSW.Write(lDate(0) & ",")
                lValue = lTsAnnual.Value(lYCount)
                If lValue <= lPercentilesAnnualAverage.ItemByKey(10) Then
                    lCategory = "Drought"
                    lSW.WriteLine(WriteToColumn(lValue, 1, 5, lDelim))
                    lTsAnnAvgDrought.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(10) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(25) Then
                    lCategory = "Dry"
                    lSW.WriteLine(WriteToColumn(lValue, 2, 5, lDelim))
                    lTsAnnAvgDry.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(25) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(75) Then
                    lCategory = "Normal"
                    lSW.WriteLine(WriteToColumn(lValue, 3, 5, lDelim))
                    lTsAnnAvgNormal.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(75) AndAlso lValue <= lPercentilesAnnualAverage.ItemByKey(90) Then
                    lCategory = "Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 4, 5, lDelim))
                    lTsAnnAvgWet.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesAnnualAverage.ItemByKey(90) Then
                    lCategory = "Very Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 5, 5, lDelim))
                    lTsAnnAvgVeryWet.Value(lYCount) = lValue
                End If
            Next

            'DisplayTsGraph(lTsGroupAnnualAvg)

            lSW.WriteLine(" ")
            lSW.WriteLine(" ")
            lSW.WriteLine("Listing of Annual Sum " & lUnitSum & " for different categories")
            lSW.WriteLine("Year,Drought,Dry,Normal,Wet,Very Wet")
            For lYCount As Integer = 1 To lTsAnnualDepth.numValues
                J2Date(lTsAnnualDepth.Dates.Value(lYCount - 1), lDate)
                lSW.Write(lDate(0) & ",")
                lValue = lTsAnnualDepth.Value(lYCount)
                If lValue <= lPercentilesAnnualSum.ItemByKey(10) Then
                    lCategory = "Drought"
                    lSW.WriteLine(WriteToColumn(lValue, 1, 5, lDelim))
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(10) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(25) Then
                    lCategory = "Dry"
                    lSW.WriteLine(WriteToColumn(lValue, 2, 5, lDelim))
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(25) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(75) Then
                    lCategory = "Normal"
                    lSW.WriteLine(WriteToColumn(lValue, 3, 5, lDelim))
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(75) AndAlso lValue <= lPercentilesAnnualSum.ItemByKey(90) Then
                    lCategory = "Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 4, 5, lDelim))
                ElseIf lValue > lPercentilesAnnualSum.ItemByKey(90) Then
                    lCategory = "Very Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 5, 5, lDelim))
                End If
            Next

            lWDMSource.Clear()
            lWDMSource = Nothing
            lSW.WriteLine(" ")
            lSW.WriteLine(" ")

            lTsAnnAvgDrought.Clear() : lTsAnnAvgDrought = Nothing
            lTsAnnAvgDry.Clear() : lTsAnnAvgDry = Nothing
            lTsAnnAvgNormal.Clear() : lTsAnnAvgNormal = Nothing
            lTsAnnAvgWet.Clear() : lTsAnnAvgWet = Nothing
            lTsAnnAvgVeryWet.Clear() : lTsAnnAvgVeryWet = Nothing
            lTsGroupAnnualAvg.Clear() : lTsGroupAnnualAvg = Nothing

            lTsAnnual.Clear() : lTsAnnual = Nothing
            lTsAnnualDepth.Clear() : lTsAnnualDepth = Nothing
            lPercentilesAnnualAverage.Clear() : lPercentilesAnnualAverage = Nothing
            lPercentilesAnnualSum.Clear() : lPercentilesAnnualSum = Nothing
            lSW.Flush()
        Next

        lSW.Close()
        lSW = Nothing
    End Sub

    Private Sub ClassifyWaterYearsForGraph()
        Dim lUADepth As Double = 0.03719 'To be multiplied by drainage area in square miles to convert to depth inch
        Dim lWorkDirClassifyYear As String = "G:\Admin\EPA_HydroFrac_HSPFEval\ClassifyYears\"
        Dim lDataDir As String = lWorkDirClassifyYear & "NWIS\"
        Dim lWDMFiles As New atcCollection() 'collection of wdm and its drainage area
        With lWDMFiles
            '.Add(lDataDir & "danville.wdm", 11220.0)
            .Add(lDataDir & "marietta.wdm", 25990.0)
            '.Add(lDataDir & "raystown.wdm", 796.0)
            '.Add(lDataDir & "westbrsusq.wdm", 6847.0)
        End With
        Dim lPercentiles() As Integer = {10, 25, 75, 90}
        'Dim lPercentiles() As Integer = {5, 50, 95}
        Dim lClassifyYearLog As String = lWorkDirClassifyYear & "ClassifyWaterYearForGraphLog.txt"
        Dim lDelim As String = ","
        Dim lSW As New StreamWriter(lClassifyYearLog, False)
        Dim lWDMSource As atcWDM.atcDataSourceWDM = Nothing
        Dim lTsWYear As atcTimeseries = Nothing
        For Each lWDMFile As String In lWDMFiles.Keys
            lWDMSource = New atcWDM.atcDataSourceWDM()
            If Not lWDMSource.Open(lWDMFile) Then Continue For
            Dim lConversionFactor As Double = lUADepth / lWDMFiles.ItemByKey(lWDMFile)
            Dim lTs As atcTimeseries = lWDMSource.DataSets(0)
            Dim lCons As String = lTs.Attributes.GetValue("Constituent")

            Dim lDateYearStart As Double = lTs.Dates.Value(0)
            Dim lDateYearEnd As Double = lTs.Dates.Value(lTs.numValues)

            'Should adjust to beginning of a water year (Oct 1)
            '       and    to ending of a water year (Sep 30)
            Dim lDate(5) As Integer
            Dim lSetToYearStart As Boolean = False
            J2Date(lDateYearStart, lDate)
            'If lDate(1) > 1 Then
            '    lSetToYearStart = True
            'Else
            '    If lDate(2) > 1 Then
            '        lSetToYearStart = True
            '    Else
            '        If lDate(3) > 0 Then lSetToYearStart = True
            '    End If
            'End If
            lSetToYearStart = True
            If lSetToYearStart Then
                If lDate(1) < 10 Then
                    lDate(1) = 10
                ElseIf lDate(1) = 10 Then
                    If lDate(2) > 1 Then
                        lDate(0) += 1
                        lDate(1) = 10
                    End If
                Else
                    lDate(0) += 1
                    lDate(1) = 10
                End If

                lDate(2) = 1
                lDate(3) = 0
                lDate(4) = 0
                lDate(5) = 0
                lDateYearStart = Date2J(lDate)
            End If

            'Adjust to end of a year
            Dim lSetToYearEnd As Boolean = False
            J2Date(lDateYearEnd, lDate)
            'If lDate(1) < 12 Then
            '    lSetToYearEnd = True
            'Else
            '    If lDate(2) < 31 Then
            '        lSetToYearEnd = True
            '    Else
            '        If lDate(3) < 24 Then lSetToYearEnd = True
            '    End If
            'End If
            lSetToYearEnd = True
            If lSetToYearEnd Then
                If lDate(1) > 9 Then
                    lDate(1) = 9
                ElseIf lDate(1) = 9 Then
                    If lDate(2) < 30 Then
                        lDate(0) -= 1
                        lDate(1) = 9
                    End If
                Else
                    lDate(0) -= 1
                    lDate(1) = 9
                End If
                lDate(2) = 30
                lDate(3) = 24
                lDate(4) = 0
                lDate(5) = 0
                lDateYearEnd = Date2J(lDate)
            End If

            If lDateYearStart <> lTs.Dates.Value(0) OrElse lDateYearEnd <> lTs.Dates.Value(lTs.numValues) Then
                lTs = SubsetByDate(lTs, lDateYearStart, lDateYearEnd, Nothing)
            End If

            Dim lUnitAverage As String = ""
            Dim lUnitSum As String = ""
            If lCons.ToLower.Contains("flow") Then
                lUnitAverage = "(cfs)"
                lUnitSum = "(in)"
            ElseIf lCons.ToLower.Contains("prec") Then
                lUnitAverage = "(in)"
                lUnitSum = "(in)"
            End If

            'Ask for 7Q10 or 7low10
            Dim l7Q10Flow As Double = lTs.Attributes.GetValue("7Q10")

            Dim lWaterYearsGroup As atcTimeseriesGroup
            Dim lWaterYearSeason As New atcSeasonsWaterYear()
            lWaterYearsGroup = lWaterYearSeason.Split(lTs, Nothing)

            Dim lWYearBeg As Integer
            Dim lWYearEnd As Integer
            Dim lWYearBegDate As Double
            Dim lWYearEndDate As Double
            Dim lTsWaterYearMeans As New atcTimeseries(Nothing)
            lTsWaterYearMeans.Dates = New atcTimeseries(Nothing)
            Dim lWaterYearMeanValues(lWaterYearsGroup.Count) As Double : lWaterYearMeanValues(0) = GetNaN()
            Dim lWaterYearMeansDates(lWaterYearsGroup.Count) As Double : lWaterYearMeansDates(0) = GetNaN()

            For I As Integer = 0 To lWaterYearsGroup.Count - 1
                lTsWYear = lWaterYearsGroup(I)
                lWYearBegDate = lTsWYear.Dates.Value(0)
                lWYearEndDate = lTsWYear.Dates.Value(lTsWYear.numValues)
                J2Date(lWYearBegDate, lDate) : lWYearBeg = lDate(0)
                J2Date(lWYearEndDate, lDate) : lWYearEnd = lDate(0)

                'Set value
                If lWYearBeg = lWYearEnd Then 'not a whole water year
                    lWaterYearMeanValues(I + 1) = GetNaN()
                Else
                    lWaterYearMeanValues(I + 1) = lTsWYear.Attributes.GetValue("Mean")
                End If
                'Set date
                If I = 0 Then
                    lWaterYearMeansDates(I) = Date2J(lWYearEnd, 1, 1, 0, 0, 0)
                Else
                    lWaterYearMeansDates(I) = Date2J(lWYearEnd, 12, 31, 24, 0, 0)
                End If
            Next
            lTsWaterYearMeans.Dates.Values = lWaterYearMeansDates
            lTsWaterYearMeans.Values = lWaterYearMeanValues
            lTsWaterYearMeans.SetInterval(atcTimeUnit.TUYear, 1)

            Dim lPercentilesDaily As New atcCollection()
            Dim lPctStr As String = ""
            With lPercentilesDaily
                For Each lPercentile As Integer In lPercentiles
                    lPctStr = lPercentile.ToString.PadLeft(2, "0")
                    .Add(lPercentile, lTsWaterYearMeans.Attributes.GetValue("%" & lPctStr))
                Next
            End With

            'Dim lExportWaterYearMeansFile As String = "G:\Admin\EPA_HydroFrac_HSPFEval\WaterUse\MeanWaterYearFlowMarietta.txt"
            'lSW = New StreamWriter(lExportWaterYearMeansFile, False)
            'For I As Integer = 0 To lTsWaterYearMeans.numValues
            '    lSW.WriteLine(I & "," & lTsWaterYearMeans.Value(I))
            'Next
            'lSW.Close()

            lSW.WriteLine("Processing Data: " & lTs.Attributes.GetValue("History 1"))
            lSW.WriteLine("Classification based on water year average " & lUnitAverage & " " & lCons & " percentiles")
            lSW.WriteLine("Percentile,Avg")
            lSW.WriteLine("%," & lUnitAverage)
            For Each lPct As Integer In lPercentilesDaily.Keys
                lSW.Write(lPct & ",")
                lSW.WriteLine(String.Format("{0:0.00}", lPercentilesDaily.ItemByKey(lPct)))
            Next

            Dim lCategory As String = ""
            Dim lValue As Double
            lSW.WriteLine(" ")
            lSW.WriteLine("Listing of Water Year Averages " & lUnitAverage & " for different categories")
            lSW.WriteLine("From-To,Drought,Dry,Normal,Wet,Very Wet")
            For Each lTsWYear In lWaterYearsGroup
                J2Date(lTsWYear.Dates.Value(0), lDate) : lWYearBeg = lDate(0)
                J2Date(lTsWYear.Dates.Value(lTsWYear.numValues), lDate) : lWYearEnd = lDate(0)
                lSW.Write(lWYearBeg & "-" & lWYearEnd & lDelim)
                lValue = lTsWYear.Attributes.GetValue("Mean") 'Mean value of the water year
                If lValue <= lPercentilesDaily.ItemByKey(10) Then
                    lCategory = "Drought"
                    lSW.WriteLine(WriteToColumn(lValue, 1, 5, lDelim))
                    'lTsAnnAvgDrought.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(10) AndAlso lValue <= lPercentilesDaily.ItemByKey(25) Then
                    lCategory = "Dry"
                    lSW.WriteLine(WriteToColumn(lValue, 2, 5, lDelim))
                    'lTsAnnAvgDry.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(25) AndAlso lValue <= lPercentilesDaily.ItemByKey(75) Then
                    lCategory = "Normal"
                    lSW.WriteLine(WriteToColumn(lValue, 3, 5, lDelim))
                    'lTsAnnAvgNormal.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(75) AndAlso lValue <= lPercentilesDaily.ItemByKey(90) Then
                    lCategory = "Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 4, 5, lDelim))
                    'lTsAnnAvgWet.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(90) Then
                    lCategory = "Very Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 5, 5, lDelim))
                    'lTsAnnAvgVeryWet.Value(lYCount) = lValue
                End If
            Next

            'DisplayTsGraph(lTsGroupAnnualAvg)

            lWDMSource.Clear()
            lWDMSource = Nothing
            lSW.WriteLine(" ")
            lSW.WriteLine(" ")

            lTsWaterYearMeans.Clear() : lTsWaterYearMeans = Nothing
            lPercentilesDaily.Clear() : lPercentilesDaily = Nothing

            ReDim lWaterYearMeanValues(0)
            ReDim lWaterYearMeansDates(0)
            lWaterYearsGroup.Clear() : lWaterYearsGroup = Nothing
            lWaterYearSeason = Nothing
            lSW.Flush()
        Next

        lSW.Close()
        lSW = Nothing
    End Sub

    Private Sub ClassifyWaterYearsPrecipForGraph()
        'Dim lUADepth As Double = 0.03719 'To be multiplied by drainage area in square miles to convert to depth inch
        Dim lWorkDirClassifyYear As String = "G:\Admin\EPA_HydroFrac_HSPFEval\ClassifyYears\"
        Dim lPrecipWDMFile As String = "G:\Admin\GCRPSusq\Runs\parms\Susq_PMETRev_met.wdm"

        Dim lPrecipStnInfoFiles As New atcCollection()
        With lPrecipStnInfoFiles
            .Add("020501", lWorkDirClassifyYear & "PrecipStations020501.txt")
            '.Add("020502", lWorkDirClassifyYear & "PrecipStations020502.txt")
            '.Add("020503", lWorkDirClassifyYear & "PrecipStations020503.txt")
            '.Add("AllGCRP", lWorkDirClassifyYear & "PrecipStationsAllGCRP.txt")
            '.Add("Raystown", lWorkDirClassifyYear & "PrecipStationsRaystown.txt")
        End With

        'Dim lSite As String = "Raystown"
        'lSite = "020501"
        'lSite = "020502"
        'lSite = "020503"
        'lSite = "AllGCRP"

        Dim lPercentiles() As Integer = {10, 25, 75, 90}

        Dim lDelim As String = ","

        Dim lWDMSource As atcWDM.atcDataSourceWDM = Nothing
        lWDMSource = New atcWDM.atcDataSourceWDM()
        If Not lWDMSource.Open(lPrecipWDMFile) Then Exit Sub

        Dim lTsWYear As atcTimeseries = Nothing
        For Each lSite As String In lPrecipStnInfoFiles.Keys

            'open output file
            Dim lClassifyYearLog As String = lWorkDirClassifyYear & "ClassifyWaterYearPrecip" & lSite & ".txt"
            Dim lSW As New StreamWriter(lClassifyYearLog, False)

            'construct dataset id - area pairs
            Dim lPrecipStnsCollection As New atcCollection()
            Dim lInfoFile As String = lPrecipStnInfoFiles.ItemByKey(lSite)
            Dim lSR As New StreamReader(lInfoFile)
            Dim lOneLine As String = ""
            Dim lArr() As String = Nothing
            Dim lTotalArea As Double = 0
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Trim = "" OrElse lOneLine.StartsWith("#") Then Continue While
                lArr = Regex.Split(lOneLine, "\s+")
                If lPrecipStnsCollection.Keys.Contains(lArr(2)) Then
                    lPrecipStnsCollection.ItemByKey(lArr(2)) += Double.Parse(lArr(3))
                Else
                    lPrecipStnsCollection.Add(lArr(2), Double.Parse(lArr(3)))
                End If
                lTotalArea += Double.Parse(lArr(3))
            End While
            lSR.Close() : lSR = Nothing

            'construct timeseries group
            Dim lTsPrecipGroup As New atcTimeseriesGroup()
            Dim lTs As atcTimeseries = Nothing
            For Each lDsn As String In lPrecipStnsCollection.Keys
                lTs = lWDMSource.DataSets.FindData("ID", Integer.Parse(lDsn))(0)
                lTs = lTs * lPrecipStnsCollection.ItemByKey(lDsn) 'multiply the original rain record with its area
                lTsPrecipGroup.Add(lDsn, lTs)
            Next

            Dim lConversionFactor As Double = 0 'lUADepth / lWDMFiles.ItemByKey(lWDMFile)
            Dim lUnitAverage As String = "(in)"
            Dim lUnitSum As String = "(in)"

            'find common period
            Dim lDateCommonStart As Double = 0
            Dim lDateCommonEnd As Double = 0

            Dim lDateFirstStart As Double = 0
            Dim lDateLastEnd As Double = 0

            If Not CommonDates(lTsPrecipGroup, lDateFirstStart, lDateLastEnd, lDateCommonStart, lDateCommonEnd) Then
                Logger.Dbg("Find common duration problem.")
            End If

            'Should adjust to beginning of a water year (Oct 1)
            '       and    to ending of a water year (Sep 30)
            Dim lDate(5) As Integer
            Dim lSetToYearStart As Boolean = False
            J2Date(lDateCommonStart, lDate)
            'If lDate(1) > 1 Then
            '    lSetToYearStart = True
            'Else
            '    If lDate(2) > 1 Then
            '        lSetToYearStart = True
            '    Else
            '        If lDate(3) > 0 Then lSetToYearStart = True
            '    End If
            'End If
            lSetToYearStart = True
            If lSetToYearStart Then
                If lDate(1) < 10 Then
                    lDate(1) = 10
                ElseIf lDate(1) = 10 Then
                    If lDate(2) > 1 Then
                        lDate(0) += 1
                        lDate(1) = 10
                    End If
                Else
                    lDate(0) += 1
                    lDate(1) = 10
                End If

                lDate(2) = 1
                lDate(3) = 0
                lDate(4) = 0
                lDate(5) = 0
                lDateCommonStart = Date2J(lDate)
            End If

            'Adjust to end of a year
            Dim lSetToYearEnd As Boolean = False
            J2Date(lDateCommonEnd, lDate)
            'If lDate(1) < 12 Then
            '    lSetToYearEnd = True
            'Else
            '    If lDate(2) < 31 Then
            '        lSetToYearEnd = True
            '    Else
            '        If lDate(3) < 24 Then lSetToYearEnd = True
            '    End If
            'End If
            lSetToYearEnd = True
            If lSetToYearEnd Then
                If lDate(1) > 9 Then
                    lDate(1) = 9
                ElseIf lDate(1) = 9 Then
                    If lDate(2) < 30 Then
                        lDate(0) -= 1
                        lDate(1) = 9
                    End If
                Else
                    lDate(0) -= 1
                    lDate(1) = 9
                End If
                lDate(2) = 30
                lDate(3) = 24
                lDate(4) = 0
                lDate(5) = 0
                lDateCommonEnd = Date2J(lDate)
            End If

            Dim lTsPrecipComDurGroup As New atcTimeseriesGroup()
            For Each lTs In lTsPrecipGroup
                lTs = SubsetByDate(lTs, lDateCommonStart, lDateCommonEnd, Nothing)
                lTsPrecipComDurGroup.Add(lTs.Attributes.GetValue("ID"), lTs)
            Next

            'construct one area-weighted average rainfall record
            '  1. sum up
            Dim lTsPrecipAWAvg As atcTimeseries = lTsPrecipComDurGroup(0).Clone
            Dim lTotal As Double
            For H As Integer = 1 To lTsPrecipComDurGroup(0).numValues
                lTotal = 0.0
                For I As Integer = 0 To lTsPrecipComDurGroup.Count - 1
                    lTotal += lTsPrecipComDurGroup(I).Value(H)
                Next
                lTsPrecipAWAvg.Value(H) = lTotal
            Next

            '  2. divide by total area
            'lTsPrecipAWAvg = lTsPrecipAWAvg / lTotalArea
            For H As Integer = 1 To lTsPrecipAWAvg.numValues
                lTsPrecipAWAvg.Value(H) /= lTotalArea
            Next

            'Try free up some memory here
            For Each lTs In lTsPrecipComDurGroup
                lTs.Clear()
            Next
            For Each lTs In lTsPrecipGroup
                lTs.Clear()
            Next
            lTsPrecipGroup.Clear()
            lTsPrecipComDurGroup.Clear()
            System.GC.Collect()
            System.GC.WaitForFullGCComplete()

            Dim lWaterYearsGroup As atcTimeseriesGroup
            Dim lWaterYearSeason As New atcSeasonsWaterYear()
            lWaterYearsGroup = lWaterYearSeason.Split(lTsPrecipAWAvg, Nothing)

            Dim lWYearBeg As Integer
            Dim lWYearEnd As Integer
            Dim lWYearBegDate As Double
            Dim lWYearEndDate As Double
            Dim lTsWaterYearSumRain As New atcTimeseries(Nothing)
            lTsWaterYearSumRain.Dates = New atcTimeseries(Nothing)
            Dim lWaterYearSumValues(lWaterYearsGroup.Count) As Double : lWaterYearSumValues(0) = GetNaN()
            Dim lWaterYearSumDates(lWaterYearsGroup.Count) As Double : lWaterYearSumDates(0) = GetNaN()

            For I As Integer = 0 To lWaterYearsGroup.Count - 1
                lTsWYear = lWaterYearsGroup(I)
                lWYearBegDate = lTsWYear.Dates.Value(0)
                lWYearEndDate = lTsWYear.Dates.Value(lTsWYear.numValues)
                J2Date(lWYearBegDate, lDate) : lWYearBeg = lDate(0)
                J2Date(lWYearEndDate, lDate) : lWYearEnd = lDate(0)

                'Set value
                If lWYearBeg = lWYearEnd Then 'not a whole water year
                    lWaterYearSumValues(I + 1) = GetNaN()
                Else
                    lWaterYearSumValues(I + 1) = lTsWYear.Attributes.GetValue("Sum")
                End If
                'Set date
                If I = 0 Then
                    lWaterYearSumDates(I) = Date2J(lWYearEnd, 1, 1, 0, 0, 0)
                Else
                    lWaterYearSumDates(I) = Date2J(lWYearEnd, 12, 31, 24, 0, 0)
                End If
            Next
            lTsWaterYearSumRain.Dates.Values = lWaterYearSumDates
            lTsWaterYearSumRain.Values = lWaterYearSumValues
            lTsWaterYearSumRain.SetInterval(atcTimeUnit.TUYear, 1)

            Dim lPercentilesDaily As New atcCollection()
            With lPercentilesDaily
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsWaterYearSumRain.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With

            lSW.WriteLine("Processing Data: " & lTs.Attributes.GetValue("History 1"))
            lSW.WriteLine("Classification based on water year sum " & lUnitSum & " precipitation percentiles")
            lSW.WriteLine("Percentile,Sum")
            lSW.WriteLine("%," & lUnitSum)
            For Each lPct As Integer In lPercentilesDaily.Keys
                lSW.Write(lPct & ",")
                lSW.WriteLine(String.Format("{0:0.00}", lPercentilesDaily.ItemByKey(lPct)))
            Next

            Dim lCategory As String = ""
            Dim lValue As Double
            lSW.WriteLine(" ")
            lSW.WriteLine("Listing of Water Year Sum " & lUnitSum & " for different categories")
            lSW.WriteLine("From-To,Drought,Dry,Normal,Wet,Very Wet")
            For Each lTsWYear In lWaterYearsGroup
                J2Date(lTsWYear.Dates.Value(0), lDate) : lWYearBeg = lDate(0)
                J2Date(lTsWYear.Dates.Value(lTsWYear.numValues), lDate) : lWYearEnd = lDate(0)
                lSW.Write(lWYearBeg & "-" & lWYearEnd & lDelim)
                lValue = lTsWYear.Attributes.GetValue("Sum") 'Sum precip value of the water year
                If lValue <= lPercentilesDaily.ItemByKey(10) Then
                    lCategory = "Drought"
                    lSW.WriteLine(WriteToColumn(lValue, 1, 5, lDelim))
                    'lTsAnnAvgDrought.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(10) AndAlso lValue <= lPercentilesDaily.ItemByKey(25) Then
                    lCategory = "Dry"
                    lSW.WriteLine(WriteToColumn(lValue, 2, 5, lDelim))
                    'lTsAnnAvgDry.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(25) AndAlso lValue <= lPercentilesDaily.ItemByKey(75) Then
                    lCategory = "Normal"
                    lSW.WriteLine(WriteToColumn(lValue, 3, 5, lDelim))
                    'lTsAnnAvgNormal.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(75) AndAlso lValue <= lPercentilesDaily.ItemByKey(90) Then
                    lCategory = "Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 4, 5, lDelim))
                    'lTsAnnAvgWet.Value(lYCount) = lValue
                ElseIf lValue > lPercentilesDaily.ItemByKey(90) Then
                    lCategory = "Very Wet"
                    lSW.WriteLine(WriteToColumn(lValue, 5, 5, lDelim))
                    'lTsAnnAvgVeryWet.Value(lYCount) = lValue
                End If
            Next

            'DisplayTsGraph(lTsGroupAnnualAvg)

            lSW.WriteLine(" ")
            lSW.WriteLine(" ")

            lTsWaterYearSumRain.Clear() : lTsWaterYearSumRain = Nothing
            lPercentilesDaily.Clear() : lPercentilesDaily = Nothing

            ReDim lWaterYearSumValues(0)
            ReDim lWaterYearSumDates(0)
            lWaterYearsGroup.Clear() : lWaterYearsGroup = Nothing
            lWaterYearSeason = Nothing
            lTsPrecipGroup.Clear() : lTsPrecipGroup = Nothing

            For Each lTs In lTsPrecipComDurGroup
                lTs.Clear() : lTs = Nothing
            Next
            lTsPrecipComDurGroup.Clear() : lTsPrecipComDurGroup = Nothing

            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        Next 'lSite

        lWDMSource.Clear()
        lWDMSource = Nothing
    End Sub

    Private Function WriteToColumn(ByVal aValue As Double, ByVal aColumn As Integer, ByVal aTotalColumns As Integer, ByVal aDelim As String) As String
        Dim lValue As String = String.Format("{0:0.00}", aValue)
        Dim lOneLine As String = ""
        Dim lThisField As String = ""
        For I As Integer = 1 To aTotalColumns
            lThisField = " "
            If I = aColumn Then lThisField = lValue
            lOneLine &= lThisField & aDelim
        Next
        Return lOneLine
    End Function

    Private Sub DisplayTsGraph(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphTime(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            '.YAxis.Type = AxisType.Log
            'Dim lScaleMin As Double = 10
            '.YAxis.Scale.Min = lScaleMin

            For I As Integer = 0 To aDataGroup.Count - 1
                .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                'With CType(.CurveList.Item(I), LineItem).Symbol
                '    .Type = SymbolType.Circle
                '    .IsVisible = True
                'End With
            Next
            .AxisChange()
        End With
        lGraphForm.Show()
    End Sub

    Private Function GetCurveColor(ByVal aTs As atcTimeseries) As System.Drawing.Color
        Dim lClass As String = aTs.Attributes.GetValue("Class")
        Select Case lClass
            Case "Drought"
                Return Drawing.Color.Magenta
            Case "Dry"
                Return Drawing.Color.DarkOrange
            Case "Normal"
                Return Drawing.Color.Green
            Case "Wet"
                Return Drawing.Color.Cyan
            Case "Very Wet"
                Return Drawing.Color.DarkBlue
        End Select
    End Function

    Private Sub SwapInCBPFlowIntoGCRPRunWDMs()
        Dim lWDMDirCBP As String = "G:\Admin\HF_CBP\CBPResults\"
        Dim lWDMDirGCRP As String = "G:\Admin\HF_CBP\Runs\"

        Dim lWDMFileCBP As String
        'lWDMFileCBP = "SU8_1610_1530.wdm" '020501-R69
        'lWDMFileCBP = "SW7_1640_0003.wdm" '020502-R43
        'lWDMFileCBP = "SL9_2490_2520.wdm" '020503-R86
        'lWDMFileCBP = "SJ4_2660_2360.wdm" '02050303-Calib-R10

        Dim lWDMFilesCBP As New atcCollection()
        With lWDMFilesCBP
            .Add("020501-R69", "SU8_1610_1530.wdm") '020501-R69
            .Add("020502-R43", "SW7_1640_0003.wdm") '020502-R43
            .Add("020503-R86", "SL9_2490_2520.wdm") '020503-R86
            .Add("02050303-Calib-R10", "SJ4_2660_2360.wdm") '02050303-Calib-R10
        End With

        Dim lWDMFileGCRP As String = ""
        Dim lDSNGCRP As Integer = 101
        Dim lDSNCBP As Integer = 111
        Dim lWDMFileHandleCBP As atcWDM.atcDataSourceWDM = Nothing
        Dim lWDMFileHandleGCRP As atcWDM.atcDataSourceWDM = Nothing
        Dim lTSCBP As atcTimeseries = Nothing
        'Dim lTSGCRP As atcTimeseries = Nothing
        For Each lWDMFileCBP In lWDMFilesCBP
            Select Case lWDMFileCBP
                Case "SU8_1610_1530.wdm" '020501-R69
                    lWDMFileGCRP = "Susq020501.wdm"
                    lDSNGCRP = 101
                Case "SW7_1640_0003.wdm" '020502-R43
                    lWDMFileGCRP = "Susq020502.wdm"
                    lDSNGCRP = 102
                Case "SL9_2490_2520.wdm" '020503-R86
                    lWDMFileGCRP = "Susq020503.wdm"
                    lDSNGCRP = 101
                Case "SJ4_2660_2360.wdm" '02050303-Calib-R10
                    lWDMFileGCRP = "SusqCalib.wdm"
                    lDSNGCRP = 101
            End Select

            lWDMFileHandleCBP = New atcWDM.atcDataSourceWDM()
            If lWDMFileHandleCBP.Open(lWDMDirCBP & lWDMFileCBP) Then
                lWDMFileHandleGCRP = New atcWDM.atcDataSourceWDM()
                If lWDMFileHandleGCRP.Open(lWDMDirGCRP & lWDMFileGCRP) Then
                    lTSCBP = lWDMFileHandleCBP.DataSets.FindData("ID", lDSNCBP)(0)
                    If lTSCBP IsNot Nothing Then
                        'lTSGCRP = lWDMFileHandleGCRP.DataSets.FindData("ID", lDSNGCRP)(0)
                        'If lTSGCRP IsNot Nothing Then
                        'End If
                        lTSCBP = Aggregate(lTSCBP, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                        lTSCBP.Attributes.SetValue("ID", lDSNGCRP)
                        If lWDMFileHandleGCRP.AddDataset(lTSCBP, atcDataSource.EnumExistAction.ExistReplace) Then Logger.Dbg("Success, replacing flow in " & lWDMFileGCRP & " @ DSN" & lDSNGCRP)
                    End If
                    lWDMFileHandleGCRP.Clear()
                    lWDMFileHandleGCRP = Nothing
                End If
                lWDMFileHandleCBP.Clear()
                lWDMFileHandleCBP = Nothing
            End If
            If lTSCBP IsNot Nothing Then
                lTSCBP.Clear() : lTSCBP = Nothing
            End If
        Next

        Logger.Dbg("Done swapping in CBP flow results into GCRP output WDM files")
    End Sub

    Private Sub DurationPlotGCRPvsCBP()
        Dim lWDMDirGCRP As String = "G:\Admin\GCRPSusq\Runs\"
        Dim lWDMDirCBP As String = "G:\Admin\HF_CBP\Runs\"
        Dim lGraphDir As String = "G:\Admin\EPA_HydroFrac_HSPFEval\Graphs\"

        Dim lWDMFileName As String = ""
        Dim lRuns As New atcCollection()
        With lRuns
            .Add("Susq020501")
            .Add("Susq020502")
            .Add("Susq020503")
            .Add("SusqCalib")
        End With
        Dim lRunsNickNames As New atcCollection()
        With lRunsNickNames
            .Add("Danville")
            .Add("WestBranchLewisburg")
            .Add("Marietta")
            .Add("Raystown")
        End With

        Dim lWDMGCRP As atcWDM.atcDataSourceWDM
        Dim lWDMCBP As atcWDM.atcDataSourceWDM = Nothing

        Dim lTsObsFlowIn As atcTimeseries
        Dim lTsObsFlowCfs As atcTimeseries
        Dim lTsSimFlowGCRPIn As atcTimeseries
        Dim lTsSimFlowGCRPCfs As atcTimeseries
        Dim lTsSimFlowCBPIn As atcTimeseries
        Dim lTsSimFlowCBPCfs As atcTimeseries
        Dim lDsnObsFlow As Integer = 1 'cfs-daily
        Dim lDsnSimFlow As Integer = 2 'inches-daily
        Dim lDateStart As Double = Date2J(1985, 1, 1, 0, 0, 0)
        Dim lDateEnd As Double = Date2J(2005, 12, 31, 24, 0, 0)

        For Each lRun As String In lRuns
            'open WDM files
            lWDMGCRP = New atcWDM.atcDataSourceWDM()
            If lWDMGCRP.Open(lWDMDirGCRP & lRun & ".wdm") Then
                lWDMCBP = New atcWDM.atcDataSourceWDM()
                If Not lWDMCBP.Open(lWDMDirCBP & lRun & ".wdm") Then
                    lWDMCBP.Clear() : lWDMCBP = Nothing
                    lWDMGCRP.Clear() : lWDMGCRP = Nothing
                    Continue For
                End If
            End If

            Dim lCons As String = "Flow"
            Dim lNickName As String = ""
            Dim lArea As Double = 0.0
            Select Case lRun
                Case "Susq020501" : lArea = 7186006 '@R69
                Case "Susq020502" : lArea = 4384720 '@R43
                Case "Susq020503" : lArea = 16554812 '@R86, but plus 01 and 02 drainage areas as well
                Case "SusqCalib" : lArea = 479638 '@R10
            End Select
            lNickName = lRunsNickNames.ItemByIndex(lRuns.IndexFromKey(lRun))

            lTsObsFlowCfs = SubsetByDate(lWDMGCRP.DataSets.ItemByKey(lDsnObsFlow), lDateStart, lDateEnd, Nothing)
            lTsObsFlowCfs.Attributes.SetValue("Units", "Flow (cfs)")
            lTsObsFlowCfs.Attributes.SetValue("YAxis", "Left")

            'lTsObsFlowIn = CfsToInches(lTsObsFlowCfs, lArea)
            'lTsObsFlowIn.Attributes.SetValue("Units", "Flow (inches)")
            'lTsObsFlowIn.Attributes.SetValue("YAxis", "Left")

            lTsSimFlowGCRPIn = SubsetByDate(lWDMGCRP.DataSets.ItemByKey(lDsnSimFlow), lDateStart, lDateEnd, Nothing)
            lTsSimFlowGCRPIn.Attributes.SetValue("Units", "Flow (inches)")
            lTsSimFlowGCRPIn.Attributes.SetValue("YAxis", "Left")

            lTsSimFlowGCRPCfs = InchesToCfs(lTsSimFlowGCRPIn, lArea)
            With lTsSimFlowGCRPCfs.Attributes
                .SetValue("Units", "Flow (cfs)")
                .SetValue("YAxis", "Left")
                .SetValue("Scenario", "GCRP")
            End With

            lTsSimFlowCBPIn = SubsetByDate(lWDMCBP.DataSets.ItemByKey(lDsnSimFlow), lDateStart, lDateEnd, Nothing)
            lTsSimFlowCBPIn.Attributes.SetValue("Units", "Flow (inches)")
            lTsSimFlowCBPIn.Attributes.SetValue("YAxis", "Left")

            lTsSimFlowCBPCfs = InchesToCfs(lTsSimFlowCBPIn, lArea)
            With lTsSimFlowCBPCfs.Attributes
                .SetValue("Units", "Flow (cfs)")
                .SetValue("YAxis", "Left")
                .SetValue("Scenario", "CBP")
            End With

            Dim lTsGroup As New atcTimeseriesGroup
            lTsGroup.Add(lTsObsFlowCfs)
            lTsGroup.Add(lTsSimFlowGCRPCfs)
            lTsGroup.Add(lTsSimFlowCBPCfs)

            Dim lSaveIn As String = lGraphDir & lRun & "_" & lNickName & ".png"
            If IO.File.Exists(lSaveIn) Then
                TryDelete(lSaveIn)
            End If
            DisplayDurGraph(lTsGroup, lSaveIn)

            'clean up
            lWDMGCRP.Clear() : lWDMGCRP = Nothing
            lWDMCBP.Clear() : lWDMCBP = Nothing
            lTsObsFlowCfs.Clear()
            lTsSimFlowGCRPCfs.Clear()
            lTsSimFlowCBPCfs.Clear()
            lTsSimFlowCBPIn.Clear()
            lTsSimFlowGCRPIn.Clear()
        Next

        Logger.Dbg("Done graphing duration plots of GCRP vs CBP vs observed flow in CFS.")
    End Sub

    Private Sub DisplayDurGraph(ByVal aDataGroup As atcTimeseriesGroup, Optional ByVal aSaveIn As String = "")
        Dim lGraphForm As New atcGraph.atcGraphForm()

        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphProbability(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane

            .AxisChange()
            .CurveList.Item(0).Color = Drawing.Color.Blue
            .CurveList.Item(1).Color = Drawing.Color.Red
            .CurveList.Item(2).Color = Drawing.Color.Cyan
            With .Legend.FontSpec
                .IsBold = False
                .Border.IsVisible = False
                .Size = 12
            End With
            .XAxis.Title.Text = "PERCENTAGE OF TIME FLOW WAS EQUALED OR EXCEEDED"
        End With
        lGraphForm.Grapher.ZedGraphCtrl.Refresh()

        If aSaveIn = "" Then
            lGraphForm.Show()
        Else
            lZgc.SaveIn(aSaveIn)
            lZgc.Dispose()
        End If
    End Sub
End Module
