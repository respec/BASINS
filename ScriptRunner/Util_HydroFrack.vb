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
        Dim lTask As Integer = 9
        Select Case lTask
            Case 1 : ConstructHuc8BasedWaterUseFile() ''Task1. get huc8 based water use
            Case 2 : ClassifyWaterYearsForGraph()
            Case 3 : SwapInCBPFlowIntoGCRPRunWDMs() 'turns out the expert system is using the ID 2 as simulated flow
            Case 4 : DurationPlotGCRPvsCBP()
            Case 5 : ClassifyWaterYearsPrecipForGraph()
            Case 6 : ConstructFTableColumnTimeseries()
            Case 7 : ConstructErrorTable()
            Case 8 : ConstructGCRPHspfSubbasinBasedWaterUseFile()
            Case 9 : SumGCRPHspfSubbasinWateruse()
                'Case 10 : BuildGCRPHspfSubbasinWaterUseTimeseries()
        End Select
    End Sub

    Private Sub BuildGCRPHspfSubbasinWaterUseTimeseries()
        'This routine is to be run after task 9 (SumGCRPHspfSubbasinWateruse) whose output is like, GCRP020503SubbasinWaterUse2000.txt
        'that file is a comma-delimited subbasin and its water use in cfs listing

        'In this routine, we will build two timeseries for each subbasin, one for PS, the other for the rest
        'from 1/1/1985 to 12/31/2005, monthly timestep

    End Sub

    Private Sub SumGCRPHspfSubbasinWateruse()
        'This routine is to be run after task task 8 (whose output file is white-space delimited, e.g. GCRP020501byCountyWaterUse2000.txt)
        'basically, to sum up the various categories of wateruse for each subbasin
        'the output file is comma-delimited (e.g. GCRP020501SubbasinWaterUse2000.txt)
        'water use categories and column order are the same as those in the excel files used in
        'task 8 (ConstructGCRPHspfSubbasinBasedWaterUseFile)

        Dim lDirGCRPHspfSubbasinWaterUse As String = "G:\Admin\EPA_HydroFrac_HSPFEval\WaterUse\"
        Dim lYears() As Integer = {2000, 2005}
        Dim lGCRPRunNames() As String = {"020501", "020502", "020503"}
        '1 million (US gallons per day) = 1.54722865 (cubic foot) per second
        Dim lMgdToCfsFactor As Double = 1.54722865
        Dim lHeaderText As String = ""
        Dim lSR As StreamReader
        For Each lYearToProcess As Integer In lYears
            For Each lGCRPRunName As String In lGCRPRunNames
                Dim lGCRPRun As New GCRPRun()
                Dim lFileSBbyCountyWU As String = lDirGCRPHspfSubbasinWaterUse & "GCRP" & lGCRPRunName & "byCountyWaterUse" & lYearToProcess & ".txt"
                Dim lFileSBWU As String = lDirGCRPHspfSubbasinWaterUse & "GCRP" & lGCRPRunName & "SubbasinWaterUse" & lYearToProcess & ".txt"
                lSR = New StreamReader(lFileSBbyCountyWU)
                Dim line As String
                Dim lArr() As String
                Dim lWUValueStartingColIndex As Integer = 7
                Dim lSubbasin As GCRPSubbasin
                While Not lSR.EndOfStream
                    line = lSR.ReadLine().Trim() 'The trim is important so as to remove the last delim char
                    lArr = Regex.Split(line, "\s+")

                    If Not IsNumeric(lArr(2)) Then
                        lHeaderText = "Subbasins,"
                        For H As Integer = lWUValueStartingColIndex To lArr.Length - 1
                            lHeaderText &= lArr(H) & ","
                        Next
                        lHeaderText = lHeaderText.TrimEnd(",")
                        Continue While
                    End If

                    lSubbasin = lGCRPRun.Subbasins.ItemByKey(lArr(2)) '2: subbasin id

                    If lSubbasin Is Nothing Then
                        lSubbasin = New GCRPSubbasin()
                        lGCRPRun.Subbasins.Add(lArr(2), lSubbasin)
                    End If
                    With lSubbasin
                        lSubbasin.SubbasinId = Integer.Parse(lArr(2))
                        If .WUYear = 0 Then .WUYear = lYearToProcess 'set the year, which decide which year it is collating
                        If .NumWUs = 0 Then .NumWUs = lArr.Length - lWUValueStartingColIndex 'set the size and initialize value to zeros
                        If Not .WUAreaList.Contains(lArr(3)) Then .WUAreaList.Add(lArr(3)) '3: county fibs code

                        For I As Integer = lWUValueStartingColIndex To lArr.Length - 1
                            If .WUYear = 2000 Then
                                .WaterUses2000.ItemByIndex(I - lWUValueStartingColIndex) += Double.Parse(lArr(I)) * lMgdToCfsFactor
                            ElseIf .WUYear = 2005 Then
                                .WaterUses2005.ItemByIndex(I - lWUValueStartingColIndex) += Double.Parse(lArr(I)) * lMgdToCfsFactor
                            End If
                        Next
                    End With
                End While
                lSR.Close()
                lSR = Nothing

                'write out result
                lGCRPRun.WriteWaterUse(lYearToProcess, lFileSBWU, lHeaderText)
                lGCRPRun.Clear()
                lGCRPRun = Nothing
            Next 'lGCRPRun
        Next 'lYearToProcess
    End Sub

    Private Sub ConstructGCRPHspfSubbasinBasedWaterUseFile()
        '*** distribute Awuds Excel data into GCRP subbasins
        ' output file is white space delimited
        'files involved example:
        'GCRP020501byCounty.txt 'clip county by GCRP subbasin shapefile
        'County.txt 'for querying county area
        'mdco95_CUPct.csv 'consumptive fraction calculated from 1995 data
        'nyco95_CUPct.csv
        'paco95_CUPct.csv
        'DataDictionaryCompare.txt 'matching wateruse categories among 1995, 2000, 2005 to apply the consumptive fraction

        'The wateruse excel files are the source of raw water use data
        'they MUST have the SAME set of wateruse categories in the SAME COLUMN ORDER
        'this same set in the same order is going to be used by task 9 (SumGCRPHspfSubbasinWateruse)
        'mdco2000SelectedWU.xls 'selected original wateruse data 
        'mdco2005SelectedWU.xls

        Dim lFipsFieldIndexExcel As Integer = 4
        Dim lWUStartColIndexExcel As Integer = 5
        Dim lFipsFieldIndexCSV1995 As Integer = 4

        Dim lAreaType As String = "co" 'county, if huc8, then h8
        'Dim l2000DataElements() As Integer = {7, 8, 9, 11, 12, 13, 14, 17, 20, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 39, 42, 45, 46, 49, 62, 65, 68}
        'Dim l2005DataElements() As Integer = {9, 12, 15, 19, 20, 21, 22, 23, 24, 27, 30, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 46, 47, 48, 49, 53, 54, 55, 56, 57, 58, 59, 60, 63, 66, 69, 72, 75, 99, 102, 105}
        Dim l2000DataElements() As Integer = {5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17} 'only selected fields
        Dim l2005DataElements() As Integer = {5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19} 'only selected fields
        Dim lDataYear() As Integer = {2000, 2005}
        'Dim lStates() As String = {"mdco", "nyco", "paco"}
        Dim lAwudsDataDirectory As String = "G:\Admin\EPA_HydroFrac_HSPFEval\WaterUse\"


        Dim lSubbasinByCountyFiles As New atcCollection
        With lSubbasinByCountyFiles
            .Add("020501", lAwudsDataDirectory & "GCRP020501byCounty.txt")
            .Add("020502", lAwudsDataDirectory & "GCRP020502byCounty.txt")
            .Add("020503", lAwudsDataDirectory & "GCRP020503byCounty.txt")
        End With

        Dim lAwudsDataFile As String = ""
        Dim lxlApp As Excel.Application = Nothing
        Dim lxlWorkbook As Excel.Workbook = Nothing
        Dim lxlWorkbookPA As Excel.Workbook = Nothing
        Dim lxlWorkbookMD As Excel.Workbook = Nothing
        Dim lxlWorkbookNY As Excel.Workbook = Nothing
        Dim lxlSheet As Excel.Worksheet = Nothing

        Dim lStates As New atcCollection
        'With lStates
        '    Dim lNewState As New WUState
        '    lNewState.Name = "Maryland"
        '    lNewState.Abbreviation = "md"
        '    lNewState.Code = "24"
        '    lStates.Add(lNewState.Code, lNewState)

        '    lNewState = New WUState
        '    lNewState.Name = "New York"
        '    lNewState.Abbreviation = "ny"
        '    lNewState.Code = "36"
        '    lStates.Add(lNewState.Code, lNewState)

        '    lNewState = New WUState
        '    lNewState.Name = "Pennsylvania"
        '    lNewState.Abbreviation = "pa"
        '    lNewState.Code = "42"
        '    lStates.Add(lNewState.Code, lNewState)
        'End With

        '*** 
        Dim File1 As String = ""
        Dim File2 As String = lAwudsDataDirectory & "County.txt"
        'construct county fips-area (sq meter) dictionary
        Dim lOneLine As String
        Dim lArrCounty() As String
        Dim lFips As String = ""
        Dim lStateAbbrev As String = ""

        Dim lLinebuilder As New Text.StringBuilder
        Dim lCountyList As New atcCollection()
        Dim lSRCounty As New StreamReader(File2)
        While Not lSRCounty.EndOfStream
            lOneLine = lSRCounty.ReadLine()
            lArrCounty = Regex.Split(lOneLine, "\s+")
            Dim lState As WUState = lStates.ItemByKey(lArrCounty(5).Substring(0, 2))
            If lState Is Nothing Then
                lState = New WUState
                With lState
                    .Code = lArrCounty(5).Substring(0, 2)
                    .Abbreviation = lArrCounty(6)
                End With
                lStates.Add(lState.Code, lState)
            End If
            If Not lState.Counties.Keys.Contains(lArrCounty(5)) Then
                Dim lCounty As New WUCounty
                With lCounty
                    .Fips = lArrCounty(5)
                    .Code = lArrCounty(5).Substring(2) 'the remaining 3 digits
                    .Area = Double.Parse(lArrCounty(13))
                    .State = lState
                End With
                lState.Counties.Add(lCounty.Fips, lCounty)
            End If

        End While
        lSRCounty.Close()
        lSRCounty = Nothing

        'open the three Consumptive percentage files
        'these are preformatted to have the same column headings
        'please note that in 1995, LA means livestock animal specialty, but in 2000 and on, it means aquaculture!!!
        'in 2000 and on, LS = 1995's LV
        For Each lState As WUState In lStates
            Dim lCuPctFilename As String = lAwudsDataDirectory & lState.Abbreviation.ToLower & lAreaType & "95_CUPct.csv"
            Dim lCuPctTable As New atcTableDelimited
            With lCuPctTable
                .Delimiter = ","
                .OpenFile(lCuPctFilename)
                .CurrentRecord = 1
                Dim lFibsFieldIndex As Integer
                Dim lAttStartFieldIndex As Integer
                For I As Integer = 1 To .NumFields
                    If .FieldName(I).ToLower = "fibs" Then lFibsFieldIndex = I
                    If .FieldName(I).ToLower.StartsWith("cupct_") Then
                        lAttStartFieldIndex = I
                        Exit For
                    End If
                Next
                Dim lValue As Double
                While Not .EOF
                    Dim lCounty As WUCounty = lState.Counties.ItemByKey(.Value(lFibsFieldIndex))
                    If lCounty Is Nothing Then
                        lCounty = New WUCounty
                        With lCounty
                            .Fips = lCuPctTable.Value(lFibsFieldIndex)
                            .State = lState
                            .Code = lCuPctTable.Value(lFibsFieldIndex).Substring(2)
                        End With
                        lState.Counties.Add(lCounty.Fips, lCounty)
                    End If
                    For I As Integer = lAttStartFieldIndex To .NumFields
                        If Not lCounty.CUPcts.ContainsAttribute(.FieldName(I).ToUpper) Then
                            If Not Double.TryParse(.Value(I), lValue) Then
                                lValue = GetNaN()
                            End If
                            lCounty.CUPcts.Add(.FieldName(I), lValue)
                        End If
                    Next
                    .MoveNext()
                End While
                .Clear()
            End With 'lCuPctTable
        Next 'lState

        'Set cross-reference of data dictionary for the chosen categories
        Dim lDDTableFilename As String = lAwudsDataDirectory & "DataDictionaryCompare.txt"
        Dim lDD As New DataDictionaries(lDDTableFilename)

        Dim lYearToProcess As Integer = lDataYear(1) 'pick a year to do
        Dim lFile As String = ""

        lxlApp = New Excel.Application()
        lAwudsDataFile = lAwudsDataDirectory & "mdco" & lYearToProcess & "SelectedWU.xls"
        lxlWorkbookMD = lxlApp.Workbooks.Open(lAwudsDataFile)
        lAwudsDataFile = lAwudsDataDirectory & "paco" & lYearToProcess & "SelectedWU.xls"
        lxlWorkbookPA = lxlApp.Workbooks.Open(lAwudsDataFile)
        lAwudsDataFile = lAwudsDataDirectory & "nyco" & lYearToProcess & "SelectedWU.xls"
        lxlWorkbookNY = lxlApp.Workbooks.Open(lAwudsDataFile)

        Dim lNeedToSetHeader As Boolean = True
        For Each lGCRPRun As String In lSubbasinByCountyFiles.Keys
            File1 = lSubbasinByCountyFiles.ItemByKey(lGCRPRun)
            lFile = IO.Path.Combine(IO.Path.GetDirectoryName(File1), "GCRP" & lGCRPRun & "byCountyWaterUse" & lYearToProcess & ".txt")

            lOneLine = ""
            ReDim lArrCounty(0)
            Dim lArrGCRPSubbasinbyCounty() As String
            lFips = ""
            Dim lCountyArea As Double = 0
            Dim lAreaPartial As Double = 0
            Dim lAreaFraction As Double = 0
            lLinebuilder.Length = 0

            Dim lSWGCRPSubbasinByCountyData As New StreamWriter(lFile, False)
            Dim lSRGCRPSubbasinByCounty As New StreamReader(File1)
            Dim lStateCode As String = ""
            Dim lState As WUState
            Dim lCounty As WUCounty = Nothing
            Dim lCat As String
            Dim lNeedToWriteHeader As Boolean = True

            While Not lSRGCRPSubbasinByCounty.EndOfStream
                lOneLine = lSRGCRPSubbasinByCounty.ReadLine
                lArrGCRPSubbasinbyCounty = Regex.Split(lOneLine, "\s+")

                lFips = lArrGCRPSubbasinbyCounty(3)
                lStateCode = lFips.Substring(0, 2)
                lState = lStates.ItemByKey(lStateCode)
                If lState IsNot Nothing Then
                    lCounty = lState.Counties.ItemByKey(lFips)
                    If lCounty IsNot Nothing Then
                        lCountyArea = lCounty.Area
                    Else
                        lCountyArea = -99.9
                    End If
                End If

                If lCountyArea < 0 Then Continue While
                If Double.TryParse(lArrGCRPSubbasinbyCounty(4), lAreaPartial) Then
                    lAreaFraction = lAreaPartial / lCountyArea
                Else
                    Continue While
                End If

                lLinebuilder.Append(lOneLine & " ")
                lLinebuilder.Append(String.Format("{0:0.0}", lCountyArea) & " ")
                lLinebuilder.Append(String.Format("{0:0.00}", lAreaFraction) & " ")

                'search for data
                Select Case lStateCode
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
                    'Get header for WU categories
                    If lNeedToSetHeader Then
                        If lYearToProcess = 2000 AndAlso WUState.WaterUseCategories2000.Count = 0 Then
                            For C As Integer = lWUStartColIndexExcel To .UsedRange.Columns.Count
                                Dim lHeaderCellValue As String = .Cells(1, C).Value
                                If lHeaderCellValue IsNot Nothing AndAlso lHeaderCellValue <> "" Then
                                    If Not WUState.WaterUseCategories2000.Contains(lHeaderCellValue) Then
                                        WUState.WaterUseCategories2000.Add(lHeaderCellValue)
                                    End If
                                End If
                            Next
                        ElseIf lYearToProcess = 2005 AndAlso WUState.WaterUseCategories2005.Count = 0 Then
                            For C As Integer = lWUStartColIndexExcel To .UsedRange.Columns.Count
                                Dim lHeaderCellValue As String = .Cells(1, C).Value
                                If lHeaderCellValue IsNot Nothing AndAlso lHeaderCellValue <> "" Then
                                    If Not WUState.WaterUseCategories2005.Contains(lHeaderCellValue) Then
                                        WUState.WaterUseCategories2005.Add(lHeaderCellValue)
                                    End If
                                End If
                            Next
                        End If

                        lNeedToSetHeader = False
                    End If
                    If lNeedToWriteHeader Then
                        lSWGCRPSubbasinByCountyData.Write("ShapeId MWShapeId Subbasin Fibs Area CArea AreaFrac ")
                        Dim lHeaderArraylist As ArrayList = Nothing
                        If lYearToProcess = 2000 Then
                            lHeaderArraylist = WUState.WaterUseCategories2000
                        ElseIf lYearToProcess = 2005 Then
                            lHeaderArraylist = WUState.WaterUseCategories2005
                        End If
                        If lHeaderArraylist IsNot Nothing Then
                            Dim lHeaderLine As String = ""
                            For Each lWUCat As String In lHeaderArraylist
                                lHeaderLine &= lWUCat & " "
                            Next
                            lSWGCRPSubbasinByCountyData.WriteLine(lHeaderLine.Trim())
                        End If
                        lNeedToWriteHeader = False
                    End If

                    For lRow As Integer = 1 To .UsedRange.Rows.Count
                        If lFips = .Cells(lRow, lFipsFieldIndexExcel).Value Then
                            Dim lValue As Double = 0
                            Dim lCuPctValue As Double = 0
                            Dim lHas1995EquivlentCuPctValue As Boolean
                            For I As Integer = 0 To lDataElements.Length - 1
                                'For I As Integer = 1 To .UsedRange.Columns.Count
                                lCat = .Cells(1, lDataElements(I)).Value
                                'lCat = .Cells(1, I).Value
                                lDD.MatchCategory(lYearToProcess, lCat)
                                lHas1995EquivlentCuPctValue = False
                                If lYearToProcess = 2000 Then
                                    If lDD.Cat1995 <> "None" AndAlso lDD.Cat2000 <> "None" Then
                                        lHas1995EquivlentCuPctValue = True
                                    End If
                                ElseIf lYearToProcess = 2005 Then
                                    If lDD.Cat1995 <> "None" AndAlso lDD.Cat2005 <> "None" Then
                                        lHas1995EquivlentCuPctValue = True
                                    End If
                                End If

                                If lHas1995EquivlentCuPctValue Then
                                    If lCounty.CUPcts.GetDefinedValue(lDD.Cat1995) Is Nothing OrElse Not Double.TryParse(lCounty.CUPcts.GetDefinedValue(lDD.Cat1995).Value.ToString, lCuPctValue) Then
                                        lCuPctValue = 100.0
                                    Else
                                        If Double.IsNaN(lCuPctValue) OrElse lCuPctValue > 100.0 Then
                                            lCuPctValue = 100.0
                                        End If
                                    End If
                                Else
                                    lCuPctValue = 100.0
                                End If

                                lValue = .Cells(lRow, lDataElements(I)).Value * lAreaFraction * lCuPctValue / 100.0
                                lLinebuilder.Append(DoubleToString(lValue) & " ")
                            Next
                            Exit For
                        End If
                    Next
                End With

                lSWGCRPSubbasinByCountyData.WriteLine(lLinebuilder.ToString)
                'output file columns
                'first, original columns from the GCRP02050xbyCounty.txt
                'then, followed by county area in sq_m and area fraction of intersection of subbasin and county
                'then, followed by the wateruse categories from the excel files
                'the file is white-space delimited
                lLinebuilder.Length = 0
            End While
            lSRGCRPSubbasinByCounty.Close()
            lSRGCRPSubbasinByCounty = Nothing

            lSWGCRPSubbasinByCountyData.Flush()
            lSWGCRPSubbasinByCountyData.Close()
            lSWGCRPSubbasinByCountyData = Nothing

        Next 'lGCRPRun
        lDD.Clear()
        lxlWorkbookMD.Close()
        lxlWorkbookNY.Close()
        lxlWorkbookPA.Close()
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

    Private Sub ConstructErrorTable()
        Dim lRunDir As String = "G:\Admin\GCRPSusq\ReportsWithReservoirs\"
        Dim lReports As New atcCollection()
        With lReports
            .Add("_Susq020501_", "R69 (Susq020501),Susquehanna River at Danville PA,USGS01540500")
            .Add("_Susq020502_", "R43 (Susq020502),West Branch Susquehanna River at Lewisburg PA,USGS01553500")
            .Add("_Susq020503_", "R86 (Susq020503),Susquehanna River at Marietta PA,USGS01576000")
        End With

        Dim lAreas As New atcCollection() 'acres
        With lAreas
            .Add("_Susq020501_", 7186006)
            .Add("_Susq020502_", 4384719)
            .Add("_Susq020503_", 16554811)
        End With

        Dim lFlowsDepthObs As New atcCollection() 'inch
        With lFlowsDepthObs
            .Add("_Susq020501_", 19.68)
            .Add("_Susq020502_", 21.74)
            .Add("_Susq020503_", 20.23)
        End With

        Dim lRoot As New DirectoryInfo("G:\Admin\GCRPSusq\ReportsWithReservoirs\")

        Dim lFiles As FileInfo() = lRoot.GetFiles("*.*")
        Dim lDirs As DirectoryInfo() = lRoot.GetDirectories("*.*")

        'Console.WriteLine("Root Directories")
        Dim lErrorFileName As String = lRunDir & "ErrorTable.txt"
        Dim lSW As New StreamWriter(lErrorFileName, False)

        Dim lDirectoryName As DirectoryInfo
        Dim lReportMultBalanceBasinsPath As String = ""
        Dim lReportDailyMonthly As String = ""
        Dim lReportExpertSys As String = ""
        Dim lKey As String = ""
        For Each lDirectoryName In lDirs
            If lDirectoryName.FullName.Contains("_Susq020501_") Then
                lKey = "_Susq020501_"
                lReportMultBalanceBasinsPath = "Water_Susq020501_Mult_BalanceBasin.txt"
                lReportDailyMonthly = "DailyMonthlyFlowStats-RCH69.txt"
                lReportExpertSys = "ExpertSysStats-susq020501.txt"
            ElseIf lDirectoryName.FullName.Contains("_Susq020502_") Then
                lKey = "_Susq020502_"
                lReportMultBalanceBasinsPath = "Water_Susq020502_Mult_BalanceBasin.txt"
                lReportDailyMonthly = "DailyMonthlyFlowStats-RCH43.txt"
                lReportExpertSys = "ExpertSysStats-susq020502.txt"
            ElseIf lDirectoryName.FullName.Contains("_Susq020503_") Then
                lKey = "_Susq020503_"
                lReportMultBalanceBasinsPath = "Water_Susq020503_Mult_BalanceBasin.txt"
                lReportDailyMonthly = "DailyMonthlyFlowStats-RCH86.txt"
                lReportExpertSys = "ExpertSysStats-susq020503.txt"
            End If

            lSW.Write(lReports.ItemByKey(lKey) & ",")
            Dim lSR As New StreamReader(IO.Path.Combine(lDirectoryName.FullName, lReportMultBalanceBasinsPath))
            Dim lVolume As Double 'ac-ft
            Dim lDepth As Double 'in
            While Not lSR.EndOfStream
                Dim line As String = lSR.ReadLine()
                If Not line.StartsWith("  OutVolume") Then Continue While
                Dim lArr() As String = line.Split(vbTab)
                If Double.TryParse(lArr(lArr.Length - 1), lVolume) Then
                    lDepth = lVolume / lAreas.ItemByKey(lKey) * 12.0
                    Exit While
                End If
            End While
            lSR.Close()
            lSR = Nothing

            lSW.Write(String.Format("{0:0.00}", lDepth) & "," & lFlowsDepthObs.ItemByKey(lKey) & ",")
            Dim lPctChange As Double = (lDepth - lFlowsDepthObs.ItemByKey(lKey)) / lFlowsDepthObs.ItemByKey(lKey) * 100
            lSW.Write(String.Format("{0:0.00}", lPctChange) & ",")

            lSR = New StreamReader(IO.Path.Combine(lDirectoryName.FullName, lReportDailyMonthly))
            Dim lDailyR As Double
            Dim lDailyR2 As Double
            Dim lMonthlyR As Double
            Dim lMonthlyR2 As Double
            Dim lDailyNSE As Double
            Dim lMonthlyNSE As Double
            Dim lTimes As Integer = 0
            While Not lSR.EndOfStream
                Dim line As String = lSR.ReadLine()
                If line.StartsWith("             Correlation Coefficient") AndAlso lTimes = 0 Then
                    lTimes += 1
                    Double.TryParse(line.Substring("             Correlation Coefficient".Length), lDailyR)
                End If
                If line.StartsWith("        Coefficient of Determination") AndAlso lTimes = 1 Then

                    Double.TryParse(line.Substring("        Coefficient of Determination".Length), lDailyR2)
                End If
                If line.StartsWith("                Model Fit Efficiency") AndAlso lTimes = 1 Then
                    lTimes += 1
                    Double.TryParse(line.Substring("                Model Fit Efficiency".Length), lDailyNSE)
                End If

                If line.StartsWith("             Correlation Coefficient") AndAlso lTimes = 2 Then

                    Double.TryParse(line.Substring("             Correlation Coefficient".Length), lMonthlyR)
                End If
                If line.StartsWith("        Coefficient of Determination") AndAlso lTimes = 2 Then

                    Double.TryParse(line.Substring("        Coefficient of Determination".Length), lMonthlyR2)
                End If
                If line.StartsWith("                Model Fit Efficiency") AndAlso lTimes = 2 Then

                    Double.TryParse(line.Substring("                Model Fit Efficiency".Length), lMonthlyNSE)

                End If
            End While
            lSR.Close()
            lSR = Nothing

            lSW.Write(String.Format("{0:0.00}", lDailyR) & "," & String.Format("{0:0.00}", lDailyR2) & ",")
            lSW.Write(String.Format("{0:0.00}", lMonthlyR) & "," & String.Format("{0:0.00}", lMonthlyR2) & ",")

            lSR = New StreamReader(IO.Path.Combine(lDirectoryName.FullName, lReportExpertSys))
            Dim lPctPeakDiff As Double
            While Not lSR.EndOfStream
                Dim line As String = lSR.ReadLine()
                If line.StartsWith("  Error in average storm peak (%) =") Then
                    line = line.Substring("  Error in average storm peak (%) =".Length)
                    Dim lArr() As String = Regex.Split(line, "\s+")
                    Double.TryParse(lArr(1), lPctPeakDiff)
                    Exit While
                End If
            End While
            lSR.Close()
            lSR = Nothing

            lSW.Write(String.Format("{0:0.00}", lPctPeakDiff) & ",")
            lSW.WriteLine(String.Format("{0:0.00}", lDailyNSE) & "," & String.Format("{0:0.00}", lMonthlyNSE))
            lSW.Flush()
        Next

        lSW.Close()
        lSW = Nothing
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
        Dim lWDMDirGCRPWithReservoir As String = "G:\Admin\GCRPSusq\RunsWithReservoirs\"
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

        Dim lWDMGCRP As atcWDM.atcDataSourceWDM = Nothing
        Dim lWDMGCRPWithResv As atcWDM.atcDataSourceWDM = Nothing
        Dim lWDMCBP As atcWDM.atcDataSourceWDM = Nothing

        Dim lTsObsFlowIn As atcTimeseries
        Dim lTsObsFlowCfs As atcTimeseries
        Dim lTsSimFlowGCRPIn As atcTimeseries
        Dim lTsSimFlowGCRPCfs As atcTimeseries
        Dim lTsSimFlowGCRPWithResvIn As atcTimeseries
        Dim lTsSimFlowGCRPWithResvCfs As atcTimeseries
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
                lWDMGCRPWithResv = New atcWDM.atcDataSourceWDM()
                If Not lWDMGCRPWithResv.Open(lWDMDirGCRPWithReservoir & lRun & ".wdm") Then
                    lWDMCBP.Clear() : lWDMCBP = Nothing
                    lWDMGCRP.Clear() : lWDMGCRP = Nothing
                    lWDMGCRPWithResv.Clear() : lWDMGCRPWithResv = Nothing
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

            lTsSimFlowGCRPWithResvIn = SubsetByDate(lWDMGCRPWithResv.DataSets.ItemByKey(lDsnSimFlow), lDateStart, lDateEnd, Nothing)
            lTsSimFlowGCRPWithResvIn.Attributes.SetValue("Units", "Flow (inches)")
            lTsSimFlowGCRPWithResvIn.Attributes.SetValue("YAxis", "Left")

            lTsSimFlowGCRPWithResvCfs = InchesToCfs(lTsSimFlowGCRPWithResvIn, lArea)
            With lTsSimFlowGCRPWithResvCfs.Attributes
                .SetValue("Units", "Flow (cfs)")
                .SetValue("YAxis", "Left")
                .SetValue("Scenario", "GCRPWithReservoir")
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
            lTsGroup.Add(lTsSimFlowGCRPWithResvCfs)
            lTsGroup.Add(lTsSimFlowCBPCfs)

            Dim lSaveIn As String = lGraphDir & lRun & "_" & lNickName & ".png"
            If IO.File.Exists(lSaveIn) Then
                TryDelete(lSaveIn)
            End If
            DisplayDurGraph(lTsGroup, lSaveIn)

            'clean up
            lWDMGCRP.Clear() : lWDMGCRP = Nothing
            lWDMGCRPWithResv.Clear() : lWDMGCRPWithResv = Nothing
            lWDMCBP.Clear() : lWDMCBP = Nothing
            lTsObsFlowCfs.Clear()
            lTsSimFlowGCRPCfs.Clear()
            lTsSimFlowGCRPWithResvCfs.Clear()
            lTsSimFlowCBPCfs.Clear()
            lTsSimFlowCBPIn.Clear()
            lTsSimFlowGCRPIn.Clear()
            lTsSimFlowGCRPWithResvIn.Clear()
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

    Public Class WUState
        Public Name As String
        Public Code As String
        Public Abbreviation As String
        Public Shared WaterUseCategories2000 As ArrayList
        Public Shared WaterUseCategories2005 As ArrayList

        Public Counties As atcCollection
        Public Sub New()
            Counties = New atcCollection()
            WaterUseCategories2000 = New ArrayList()
            WaterUseCategories2005 = New ArrayList()
        End Sub

    End Class

    Public Class WUCounty
        Public State As WUState
        Public Name As String
        Public Fips As String
        Public Code As String
        Public Area As Double
        Public CUPcts As atcDataAttributes
        Public WaterUses2000 As atcDataAttributes
        Public WaterUses2005 As atcDataAttributes
        Public Sub New()
            CUPcts = New atcDataAttributes()
            WaterUses2000 = New atcDataAttributes()
            WaterUses2005 = New atcDataAttributes()
        End Sub
    End Class

    Public Class DataDictionaries
        Public Specification As String
        Public DDTable As atcTableDelimited
        Public Col1995 As Integer = -99
        Public Col2000 As Integer = -99
        Public Col2005 As Integer = -99
        Public Cat1995 As String
        Public Cat2000 As String
        Public Cat2005 As String
        Public Sub New(Optional ByVal aSpec As String = "", Optional ByVal aDelim As String = ",")
            Specification = aSpec
            If File.Exists(Specification) Then
                DDTable = New atcTableDelimited
                With DDTable
                    .Delimiter = aDelim
                    If Not .OpenFile(Specification) Then
                        Specification = ""
                        DDTable = Nothing
                    Else
                        For I As Integer = 1 To .NumFields
                            Select Case .FieldName(I)
                                Case "1995" : Col1995 = I
                                Case "2000" : Col2000 = I
                                Case "2005" : Col2005 = I
                            End Select
                        Next
                    End If
                End With
            End If
        End Sub
        Public Sub MatchCategory(ByVal aYear As Integer, ByVal aCat As String)
            If DDTable Is Nothing Then
                Cat1995 = "" : Cat2000 = "" : Cat2005 = ""
                Exit Sub
            ElseIf Col1995 < 0 OrElse Col2000 < 0 OrElse Col2005 < 0 Then
                Exit Sub
            End If
            With DDTable
                .CurrentRecord = 1
                Dim lFoundMatch As Boolean = False
                While Not .EOF
                    Select Case aYear
                        Case 1995
                            If .Value(Col1995).ToLower = aCat.ToLower Then
                                Cat1995 = aCat
                                Cat2000 = .Value(Col2000)
                                Cat2005 = .Value(Col2005)
                                lFoundMatch = True
                            End If
                        Case 2000
                            If .Value(Col2000).ToLower = aCat.ToLower Then
                                Cat1995 = .Value(Col1995)
                                Cat2000 = aCat
                                Cat2005 = .Value(Col2005)
                                lFoundMatch = True
                            End If
                        Case 2005
                            If .Value(Col2005).ToLower = aCat.ToLower Then
                                Cat1995 = .Value(Col1995)
                                Cat2000 = .Value(Col2000)
                                Cat2005 = aCat
                                lFoundMatch = True
                            End If
                    End Select
                    If lFoundMatch Then
                        Exit Sub
                    Else
                        Cat1995 = ""
                        Cat2000 = ""
                        Cat2005 = ""
                    End If
                    .MoveNext()
                End While
            End With
        End Sub
        Public Sub Clear()
            If DDTable IsNot Nothing Then
                DDTable.Clear()
                DDTable = Nothing
            End If
        End Sub
    End Class

    Public Class GCRPRun
        Public Subbasins As atcCollection
        Public Sub New()
            Subbasins = New atcCollection()
        End Sub
        Public Sub WriteWaterUse(ByVal aYear As Integer, ByVal aSpec As String, Optional ByVal aHeader As String = "") 'aYear is either 2000 or 2005
            Dim lSW As New StreamWriter(aSpec, False)
            If Not aHeader = "" Then
                lSW.WriteLine(aHeader)
            End If
            For Each lSubbasin As GCRPSubbasin In Subbasins
                lSubbasin.WUYear = aYear
                lSW.WriteLine(lSubbasin.ToString())
            Next
            lSW.Flush()
            lSW.Close()
            lSW = Nothing
        End Sub
        Public Sub Clear()
            For Each lSub As GCRPSubbasin In Subbasins
                lSub.Clear()
            Next
            Subbasins.Clear()
        End Sub
    End Class
    Public Class GCRPSubbasin
        Public SubbasinId As Integer
        Public WUYear As Integer
        Private pNumWUs As Integer
        Public WUAreaList As atcCollection
        Public WaterUses2000 As atcCollection
        Public WaterUses2005 As atcCollection

        Public Property NumWUs() As Integer
            Get
                Return pNumWUs
            End Get
            Set(ByVal value As Integer)
                pNumWUs = value
                If WUYear = 2000 Then
                    WaterUses2000.Clear()
                ElseIf WUYear = 2005 Then
                    WaterUses2005.Clear()
                End If
                For I As Integer = 0 To pNumWUs - 1
                    If WUYear = 2000 Then
                        WaterUses2000.Add(I.ToString, 0.0)
                    ElseIf WUYear = 2005 Then
                        WaterUses2005.Add(I.ToString, 0.0)
                    End If
                Next
            End Set
        End Property
        Public Sub New()
            WUAreaList = New atcCollection()
            WaterUses2000 = New atcCollection() 'keyed on wateruse cat name
            WaterUses2005 = New atcCollection() 'keyed on wateruse cat name
        End Sub

        Public Overrides Function ToString() As String
            Dim lWUs As atcCollection = Nothing
            If WUYear = 2000 Then
                lWUs = WaterUses2000
            ElseIf WUYear = 2005 Then
                lWUs = WaterUses2005
            End If
            If lWUs Is Nothing Then Return ""

            Dim lStr As String = SubbasinId.ToString & ","
            For I As Integer = 0 To lWUs.Count - 1
                lStr &= DoubleToString(lWUs.ItemByIndex(I)) & ","
            Next

            Return lStr.TrimEnd(",")
        End Function

        Public Sub Clear()
            WUAreaList.Clear()
            WaterUses2000.Clear()
            WaterUses2005.Clear()
            WUAreaList = Nothing
            WaterUses2000 = Nothing
            WaterUses2005 = Nothing
        End Sub
    End Class

End Module
