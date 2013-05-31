Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcWdmVb
Imports System
Imports System.Collections.Specialized
Imports MapWindow.Interfaces

Module modMetData
    Private Const pTestMode As Boolean = False
    Private Const pReportNanAsZero As Boolean = True
    Private Const pFormat As String = "##,###,##0.000"
    Private Const pFormatLL As String = "###0.00###"
    Private Const pStartYear As Integer = 1970
    Private Const pEndYear As Integer = 2010
    ''' <summary>
    ''' This module was a copy of the modMetData.vb in c:\Dev\StormWaterCalculator\MetData
    ''' That project was done in VS2010 with variant versions of atcData, atcControl, and atcUtility etc
    ''' but can't get them to compile, so moved this core program here under the BASINS ScriptRunner's fold
    ''' </summary>
    ''' <param name="aMapWin"></param>
    ''' <remarks></remarks>
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ' check reading and writing of SWMM input sequences
        Dim lSWMMCheck As Boolean = False
        If lSWMMCheck Then
            IO.Directory.SetCurrentDirectory("..\..\..\Test\SWMM")
            Dim lSWMMINPs() As String = {"Example1.inp", "Example2.inp", "Example3.inp", "PatSWMM05052010.inp", "Patuxent.inp"}
            Logger.StartToFile("SWMM.log")
            For Each lFileName As String In lSWMMINPs
                Dim lSWMMProject As New atcSWMM.atcSWMMProject
                IO.Directory.SetCurrentDirectory("base")
                Try
                    lSWMMProject.Open(lFileName)
                Catch lEx As Exception
                    Logger.Dbg("Problem reading " & lFileName & " " & lEx.ToString)
                End Try
                IO.Directory.SetCurrentDirectory("..\current")
                Try
                    lSWMMProject.Save(lFileName)
                Catch lEx As Exception
                    Logger.Dbg("Problem writing " & lFileName & " " & lEx.ToString)
                End Try
                IO.Directory.SetCurrentDirectory("..")
            Next
        End If

        Dim lMetDataBase As String = "G:\Data\BasinsMet\"
        Dim lDataSources() As String = {"ATest"} '"WdmFinal", "SamplePrec", "WDMRaw" }
        IO.Directory.SetCurrentDirectory(lMetDataBase & lDataSources(0)) '"..\Test\MetData")
        Logger.StartToFile("MetData.log")
        'this teaches atcData how to calculate basic statistics
        Dim lStatistics As New atcTimeseriesStatistics.atcTimeseriesStatistics
        For Each lOperation As atcDefinedValue In lStatistics.AvailableOperations
            atcDataAttributes.AddDefinition(lOperation.Definition)
        Next

        For Each lDataSource As String In lDataSources
            Dim lWdmPath As String = lMetDataBase & lDataSource
            Dim lWdmFileNames As NameValueCollection = Nothing
            AddFilesInDir(lWdmFileNames, lWdmPath, True, "*.wdm")
            Logger.Dbg("WdmCountIn " & lWdmPath & " is " & lWdmFileNames.Count)
            If pTestMode Then
                Dim lWdmFileName As String = lWdmFileNames(0)
                lWdmFileNames.Clear()
                lWdmFileNames.Add(lWdmFileName.ToLower, lWdmFileName)
            End If

            TryDelete(lDataSource & "summary.txt")
            TryDelete(lDataSource & "constituentCounts.txt")

            If Not IO.File.Exists(lDataSource & "summary.txt") OrElse _
               Not IO.File.Exists(lDataSource & "constituentCounts.txt") Then
                SummarizeData(lWdmFileNames, lDataSource)
            End If

            Dim lConstituentNames() As String = {"EVAP", "PEVT", "PMET"} ', "HPCP", "PREC", "PRCP", "ATEMP"}
            TryDelete(lDataSource & "_" & lConstituentNames(0) & "_Summary.txt")
            If Not IO.File.Exists(lDataSource & "_" & lConstituentNames(0) & "_Summary.txt") Then
                SummarizeDataByConstituent(lDataSource, lConstituentNames)
            End If
            TryDelete(lDataSource & "_" & lConstituentNames(0) & "_Details.txt")
            If Not IO.File.Exists(lDataSource & "_" & lConstituentNames(0) & "_Details.txt") Then
                SummarizeDataByConstituentDetails(lDataSource, lConstituentNames, lWdmFileNames)
            End If
        Next
        Dim lETDetailFiles As NameValueCollection = Nothing
        AddFilesInDir(lETDetailFiles, CurDir, False, "*EV*_Details.txt")
        AddFilesInDir(lETDetailFiles, CurDir, False, "*PMET_Details.txt")
        If lETDetailFiles.Count > 0 Then CombineTables("EVAP", lETDetailFiles, 5, 2000)
        Dim lPrecDetailFiles As NameValueCollection = Nothing
        AddFilesInDir(lPrecDetailFiles, CurDir, False, "*PREC*_Summary.txt")
        If lPrecDetailFiles.Count > 0 Then CombineTables("PREC", lPrecDetailFiles, 5, 2000)
    End Sub

    ''' <summary></summary>
    ''' <param name="aDataType">Type of data</param>
    ''' <param name="aDetailDataFileNames">Names of files to combine</param>
    ''' <param name="aMinYears">Mininum number of years required to include a station</param>
    ''' <param name="aYearEndRequired">Data must end after this year to be included</param>
    ''' <remarks></remarks>
    Sub CombineTables(ByVal aDataType As String, _
                      ByVal aDetailDataFileNames As NameValueCollection, _
                      ByVal aMinYears As Double, _
                      ByVal aYearEndRequired As Integer)
        Logger.Dbg("BuildDatabase " & aDataType & " from " & aDetailDataFileNames.Count)
        Dim lFirst As Boolean = True
        Dim lCombined As New Text.StringBuilder
        Dim lObserved As New Text.StringBuilder
        Dim lHourlyPrec As New Text.StringBuilder
        Dim lHeaderLength As Integer = 0
        Dim lStationDetails As New SortedList
        Dim lRecordCount As Integer = 0
        For Each lDetailDataFileName As String In aDetailDataFileNames
            Dim lDatasetDetailContents() As String = IO.File.ReadAllLines(lDetailDataFileName)
            If lFirst Then
                lCombined.AppendLine(lDatasetDetailContents(0))
                lObserved.AppendLine(lDatasetDetailContents(0))
                lHourlyPrec.AppendLine(lDatasetDetailContents(0))
                lHeaderLength = lHourlyPrec.Length
                lFirst = False
            End If
            lRecordCount += lDatasetDetailContents.GetUpperBound(0)
            For lRecordIndex As Integer = 1 To lDatasetDetailContents.GetUpperBound(0)
                Dim lRecordDetail() As String = lDatasetDetailContents(lRecordIndex).Split(vbTab)
                If lRecordDetail.GetUpperBound(0) >= 10 AndAlso _
                    lRecordDetail(10) >= aMinYears AndAlso _
                    lRecordDetail(9).Substring(1, 4) > aYearEndRequired Then
                    Dim lStationId As String = lRecordDetail(0)
                    Dim lStationDetail As ArrayList
                    If lStationDetails.Contains(lStationId) Then
                        lStationDetail = lStationDetails.Item(lStationId)
                    Else
                        lStationDetail = New ArrayList
                        lStationDetails.Add(lStationId, lStationDetail)
                    End If
                    lStationDetail.Add(lDatasetDetailContents(lRecordIndex))
                End If
            Next
        Next
        Logger.Dbg("StationCount " & lStationDetails.Count & " RecordCount " & lRecordCount)

        For lStationIndex As Integer = 0 To lStationDetails.Count - 1
            Dim lStation As ArrayList = lStationDetails.GetByIndex(lStationIndex)
            For Each lStationDetail As String In lStation
                If lStationDetail.Length > 0 Then
                    If lStationDetail.Contains("WDMRaw") Then
                        'kluge to make field widths look better
                        lStationDetail = lStationDetail.Replace("WDMRaw", "WDMRaw  ")
                        lObserved.AppendLine(lStationDetail)
                    ElseIf lStationDetail.Contains(vbTab & "1" & vbTab) Then
                        lHourlyPrec.AppendLine(lStationDetail)
                    End If
                    lCombined.AppendLine(lStationDetail)
                End If
            Next
        Next
        SaveFileString(aDataType & "_AllDetails.txt", lCombined.ToString)
        If lObserved.Length > lHeaderLength Then
            SaveFileString(aDataType & "_ObsDetails.txt", lObserved.ToString)
            SaveFileString(aDataType & "_ObsDetailsDelimV.txt", lObserved.ToString.Replace(vbTab, "|"))
        End If
        If lHourlyPrec.Length > lHeaderLength Then
            SaveFileString(aDataType & "_Details.txt", lHourlyPrec.ToString)
            SaveFileString(aDataType & "_DetailsDelimV.txt", lHourlyPrec.ToString.Replace(vbTab, "|"))
        End If
    End Sub

    ''' <summary>Make a summary of data found in a collection of WDM files</summary>
    ''' <param name="aWdmFileNames">Collection of WDM file names to summarize</param>
    ''' <param name="aDataSource">Name of collection of WDM files</param>
    ''' <remarks></remarks>
    Sub SummarizeData(ByVal aWdmFileNames As NameValueCollection, ByVal aDataSource As String)
        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lConstituentCounts As New atcCollection
        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("StationId" & vbTab & "DataType" & vbTab & "FileName" & vbTab & "Id" & vbTab & "Lat" & vbTab & "Long" & vbTab & _
                       "Scenario" & vbTab & "Constituent" & vbTab & _
                       "SDate" & vbTab & "EDate" & vbTab & "YrCount" & vbTab & "Value" & vbTab & "StaNam")
        Dim lWdmCnt As Integer = 0
        For Each lWdmFileName As String In aWdmFileNames
            lWdmCnt += 1
            Dim lWDMFile As New atcWDMfile
            If lWDMFile.Open(lWdmFileName) Then
                Dim lWDMName As String = IO.Path.GetFileName(lWdmFileName)
                Dim lLatitudeBase As Double = lWDMFile.DataSets(0).Attributes.GetValue("Latitude")
                Dim lLongitudeBase As Double = lWDMFile.DataSets(0).Attributes.GetValue("Longitude")
                Dim lStationId As String = lWDMName.Substring(0, 6)
                If Not IsNumeric(lStationId.Substring(0, 1)) Then
                    lStationId = lWDMName.Substring(2, 6)
                End If

                For Each lTimeseries As atcTimeseries In lWDMFile.DataSets
                    lTimeseries = SubsetByDate(lTimeseries, Date2J(pStartYear, 1, 1), Date2J(pEndYear, 12, 31), Nothing)
                    Dim lCons As String = lTimeseries.Attributes.GetValue("Constituent")
                    lConstituentCounts.Increment(lCons)
                    Dim lScenario As String = lTimeseries.Attributes.GetValue("Scenario")
                    If lTimeseries.Attributes.GetValue("Min") = -998.0 Then
                        UpdateAccumulated(lTimeseries)
                    End If
                    Dim lValue As String
                    Select Case lCons 'dont include constituents dont want to summarize
                        Case "EVAP", "PEVT", "PMET", "HPRC", "PREC", "PRCP", "NO23", "NH4A", "NH4D", "ORGN", "PO4A", "ORGP"
                            lValue = DoubleToString(lTimeseries.Attributes.GetValue("SumAnnual"), , pFormat)
                        Case "DEWP", "WNDH", "RADH", "ATMP", "CLDC"
                            lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), , pFormat)
                        Case Else
                            lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), 16, pFormat) '"?"
                    End Select
                    If lValue <> "?" Then
                        With lTimeseries.Attributes
                            Dim lSJDay As Double = .GetValue("SJDay")
                            Dim lEJDay As Double = .GetValue("EJDay")
                            lSB.AppendLine(lStationId.PadLeft(8) & vbTab & _
                                           aDataSource.PadLeft(8) & vbTab & _
                                           lWDMName.PadLeft(12) & vbTab & _
                                           .GetValue("ID").ToString.PadLeft(12) & vbTab & _
                                           DoubleToString(.GetValue("Latitude", lLatitudeBase), , pFormatLL).PadLeft(12) & vbTab & _
                                           DoubleToString(.GetValue("Longitude", lLongitudeBase), , pFormatLL).PadLeft(12) & vbTab & _
                                           lScenario.PadLeft(12) & vbTab & _
                                           lCons.PadLeft(10) & vbTab & _
                                           ("'" & lD2SStart.JDateToString(lSJDay) & "'").PadLeft(12) & vbTab & _
                                           ("'" & lD2SEnd.JDateToString(lEJDay) & "'").PadLeft(12) & vbTab & _
                                           DoubleToString((lEJDay - lSJDay) / 365.25, , pFormat).PadLeft(6) & vbTab & _
                                           lValue.PadLeft(10) & vbTab & _
                                           .GetValue("StaNam".ToString))
                        End With
                    Else
                        'TODO: first time skip message
                    End If
                Next
                lWDMFile.DataSets.Clear()
                lWDMFile = Nothing
            Else
                Logger.Dbg("ProblemOpening " & lWdmFileName)
            End If
            Dim lPercent As String = "(" & DoubleToString((100 * lWdmCnt) / aWdmFileNames.Count, , pFormat) & "%)"
            Logger.Dbg("Done " & lWdmCnt & lPercent & lWdmFileName & " MemUsage " & MemUsage())
        Next
        SaveFileString(aDataSource & "Summary.txt", lSB.ToString)
        lSB = New Text.StringBuilder 'use .Clear in 4.0
        For lConstituentIndex As Integer = 0 To lConstituentCounts.Count - 1
            lSB.AppendLine(lConstituentCounts.Keys(lConstituentIndex).ToString.PadLeft(12) & vbTab & lConstituentCounts.Item(lConstituentIndex))
        Next
        SaveFileString(aDataSource & "ConstituentCounts.txt", lSB.ToString)
        Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    ''' <summary>Make a summary of data for a specified set of constituents found in an existing summary file</summary>
    ''' <param name="aDataSource">Name of original collection of WDM files</param>
    ''' <param name="aConstituentNames">Constituent names to summarize</param>
    ''' <remarks></remarks>
    Sub SummarizeDataByConstituent(ByVal aDataSource As String, ByVal aConstituentNames() As String)
        Dim lDatasetTable As New atcTableDelimited
        With lDatasetTable
            .Delimiter = vbTab
            If Not .OpenFile(aDataSource & "summary.txt") Then Exit Sub
            Logger.Dbg("Fields " & .NumFields & " Records " & .NumRecords)

            Dim lConstituentFieldIndex As Integer = .FieldNumber("Constituent")
            Dim lIdFieldIndex As Integer = .FieldNumber("Id")
            Dim lConstituentTable As atcTableDelimited = .Cousin
            For Each lConstituentName As String In aConstituentNames
                .MoveFirst()
                While Not .EOF
                    If .Value(lConstituentFieldIndex) = "PMET" AndAlso .Value(lIdFieldIndex).Contains("1003") Then
                        'do nothing, by pass this record
                    ElseIf .Value(lConstituentFieldIndex) = lConstituentName Then
                        lConstituentTable.NumRecords += 1
                        lConstituentTable.MoveNext()
                        For lFieldIndex As Integer = 1 To .NumFields
                            lConstituentTable.Value(lFieldIndex) = .Value(lFieldIndex)
                        Next
                    End If
                    .MoveNext()
                End While
                If lConstituentTable.NumRecords > 0 Then
                    lConstituentTable.WriteFile(aDataSource & "_" & lConstituentName & "_Summary.txt")
                    lConstituentTable.ClearData()
                End If
            Next
        End With
    End Sub

    ''' <summary>Make a summary of data (with monthly values) in a collection of WDM files for a specified set of constituents</summary>
    ''' <param name="aDataSource">Name of original collection of WDM files</param>
    ''' <param name="aConstituentNames">Constituent names to summarize</param>
    ''' <param name="aWdmFileNames">Collection of WDM file names to summarize</param>
    ''' <remarks></remarks>
    Sub SummarizeDataByConstituentDetails(ByVal aDataSource As String, _
                                          ByVal aConstituentNames() As String, _
                                          ByVal aWdmFileNames As NameValueCollection)
        Dim lWdmFileNames As New NameValueCollection
        For Each lWdmFileName As String In aWdmFileNames
            Dim lWdmFileNameShort As String = IO.Path.GetFileName(lWdmFileName)
            If Array.IndexOf(lWdmFileNames.AllKeys, lWdmFileName) >= 0 Then
                Logger.Dbg("DuplicateKeyFor " & lWdmFileName)
            Else
                lWdmFileNames.Add(lWdmFileNameShort, lWdmFileName)
            End If
        Next

        Dim lSeasonsMonth As New atcSeasonsMonth
        Dim lDatasetTable As New atcTableDelimited

        For Each lConstituentName As String In aConstituentNames
            Logger.Dbg("Process " & lConstituentName & " in " & aDataSource)
            With lDatasetTable
                .Delimiter = vbTab
                If .OpenFile(aDataSource & "_" & lConstituentName & "_Summary.txt") Then
                    Logger.Dbg("Fields " & .NumFields & " Records " & .NumRecords)
                    Dim lDatasetTableWithDetails As atcTableDelimited = .Cousin
                    With lDatasetTableWithDetails
                        .NumFields += 13
                        .FieldName(.NumFields - 13) = "Sum"
                        Dim lMonthIndex As Integer = 1
                        For lFieldNameIndex As Integer = .NumFields - 12 To .NumFields - 1
                            .FieldName(lFieldNameIndex) = MonthName(lMonthIndex, True)
                            lMonthIndex += 1
                        Next
                        .NumRecords = 1
                    End With
                    'need to do the next 4 lines because changing the number of fields clears all fieldnames
                    For lFieldIndex As Integer = 1 To .NumFields - 1
                        lDatasetTableWithDetails.FieldName(lFieldIndex) = .FieldName(lFieldIndex)
                    Next
                    lDatasetTableWithDetails.FieldName(lDatasetTableWithDetails.NumFields) = .FieldName(.NumFields)

                    Dim lWdmFileNameFieldIndex As Integer = .FieldNumber("FileName")
                    Dim lIdFieldIndex As Integer = .FieldNumber("Id")
                    While Not .EOF
                        Dim lWdmFileNameWanted As String = .Value(lWdmFileNameFieldIndex)
                        Dim lWdmFileName As String = lWdmFileNames.GetValues(lWdmFileNameWanted)(0) 'lWdmFileNames.Item(lWdmFileNameWanted)
                        Dim lWDMFile As New atcWDMfile
                        If lWDMFile.Open(lWdmFileName) Then
                            Dim lTimeseries As atcTimeseries = lWDMFile.DataSets.FindData("Id", .Value(lIdFieldIndex)).Item(0)
                            If lTimeseries.Attributes.GetValue("Min") = -998.0 Then
                                UpdateAccumulated(lTimeseries)
                            End If
                            Dim lMonthValues(12) As Double
                            For Each lTimeseriesMonth As atcTimeseries In lSeasonsMonth.Split(lTimeseries, Nothing)
                                Dim lMonthValue As Double = lTimeseriesMonth.Attributes.GetValue("SumAnnual")
                                If pReportNanAsZero AndAlso Double.IsNaN(lMonthValue) Then
                                    lMonthValue = 0.0
                                ElseIf lMonthValue < 0.01 Then
                                    lMonthValue = 0.0
                                End If
                                Dim lSeasonIndex As Integer = lTimeseriesMonth.Attributes.GetValue("SeasonIndex")
                                Select Case lSeasonIndex
                                    Case 1, 3, 5, 7, 8, 10, 12
                                        lMonthValues(lSeasonIndex) = lMonthValue / 31.0
                                    Case 2
                                        lMonthValues(lSeasonIndex) = lMonthValue / 28.25
                                    Case Else
                                        lMonthValues(lSeasonIndex) = lMonthValue / 30.0
                                End Select
                            Next
                            lDatasetTableWithDetails.MoveNext()
                            For lFieldIndex As Integer = 1 To .NumFields - 1
                                lDatasetTableWithDetails.Value(lFieldIndex) = .Value(lFieldIndex)
                            Next
                            Dim lSumMonths As Double = 0.0
                            Dim lMonthIndex As Integer = 1
                            For lFieldIndex As Integer = .NumFields + 1 To .NumFields + 12
                                lSumMonths += lMonthValues(lMonthIndex)
                                lDatasetTableWithDetails.Value(lFieldIndex) = DoubleToString(lMonthValues(lMonthIndex), , pFormat)
                                lMonthIndex += 1
                            Next
                            lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields - 13) = DoubleToString(lSumMonths, , pFormat)
                            lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields) = .Value(.NumFields)
                            lDatasetTableWithDetails.NumRecords += 1
                            Logger.Dbg("Found " & lTimeseries.Attributes.GetValue("Id").ToString.PadLeft(4) & " value " & DoubleToString(lTimeseries.Attributes.GetValue("SumAnnual"), , pFormat).PadLeft(10) & _
                                       DoubleToString(lSumMonths, , pFormat).PadLeft(10) & " in " & lWdmFileName)
                            lWDMFile.DataSets.Clear()
                            lWDMFile = Nothing
                        Else
                            Logger.Dbg("ProblemOpening " & lWdmFileName)
                        End If

                        .MoveNext()
                    End While
                    lDatasetTableWithDetails.WriteFile(aDataSource & "_" & lConstituentName & "_Details.txt")
                    .Clear()
                Else
                    Logger.Dbg(lConstituentName & " NotFound")
                End If
            End With
        Next
    End Sub

    ''' <summary>Distributes accumulated data within a timeseries</summary>
    ''' <param name="aTimeseries">Timeseries to distribute accumulated data in</param>
    ''' <param name="aAccumulatedValue">Accumulated data value</param>
    ''' <remarks></remarks>
    Public Sub UpdateAccumulated(ByVal aTimeseries As atcTimeseries, Optional ByVal aAccumulatedValue As Double = -998.0)
        With aTimeseries
            Dim lCons As String = .Attributes.GetValue("Constituent")
            Dim lFunction As String = "Div"
            If lCons = "ATMP" Then  'TODo - add more as needed
                lFunction = "Same"
            End If

            Dim lAccumIndexStart As Integer = 0
            Dim lAccum As Boolean = False
            For lValueIndex As Integer = 1 To .numValues
                If .Value(lValueIndex) = aAccumulatedValue Then
                    If Not lAccum Then
                        lAccumIndexStart = lValueIndex
                        lAccum = True
                    End If
                ElseIf lAccum Then
                    Dim lNewValue As Double = .Value(lValueIndex)
                    If lFunction = "Div" Then
                        lNewValue /= CDbl(1 + lValueIndex - lAccumIndexStart)
                    End If
                    For lTempIndex As Integer = lAccumIndexStart To lValueIndex
                        .Value(lTempIndex) = lNewValue
                    Next
                    lAccum = False
                End If
            Next
            .Attributes.CalculateAll()
        End With
    End Sub
End Module
