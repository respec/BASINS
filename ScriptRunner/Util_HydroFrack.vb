Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports atcMetCmp
Imports atcData
Imports atcGraph
Imports ZedGraph
Imports Microsoft.Office.Interop

Module Util_HydroFrack
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lTask As Integer = 2
        Select Case lTask
            Case 1 : ConstructHuc8BasedWaterUseFile() ''Task1. get huc8 based water use
            Case 2 : ClassifyWaterYearsForGraph()
        End Select
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
            .Add(lDataDir & "danville.wdm", 11220.0)
            .Add(lDataDir & "marietta.wdm", 25990.0)
            .Add(lDataDir & "raystown.wdm", 796.0)
            .Add(lDataDir & "westbrsusq.wdm", 6847.0)
        End With
        Dim lPercentiles() As Integer = {10, 25, 75, 90}
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

            Dim lUnitAverage As String = ""
            Dim lUnitSum As String = ""
            If lCons.ToLower.Contains("flow") Then
                lUnitAverage = "(cfs)"
                lUnitSum = "(in)"
            ElseIf lCons.ToLower.Contains("prec") Then
                lUnitAverage = "(in)"
                lUnitSum = "(in)"
            End If

            Dim lWaterYearsGroup As atcTimeseriesGroup
            Dim lWaterYearSeason As New atcSeasonsWaterYear()
            lWaterYearsGroup = lWaterYearSeason.Split(lTs, Nothing)

            Dim lDate(5) As Integer
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
            With lPercentilesDaily
                For Each lPercentile As Integer In lPercentiles
                    .Add(lPercentile, lTsWaterYearMeans.Attributes.GetValue("%" & lPercentile.ToString))
                Next
            End With

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
End Module
