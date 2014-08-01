﻿Imports atcData
Imports atcUtility
Imports atcTimeseriesBaseflow
Imports MapWinUtility

Public Module modBaseflowUtil
    Public MethodsLastDone As ArrayList
    Public OutputFilenameRoot As String
    Public OutputDir As String
    Private pUADepth As Double = 0.03719

    ''' <summary>
    '''set methods
    '''Args.SetValue("Methods", pMethods)
    '''Set drainage area
    '''Args.SetValue("Drainage Area", lDA)
    '''set duration
    '''Args.SetValue("Start Date", StartDateFromForm)
    '''Args.SetValue("End Date", EndDateFromForm)
    '''Set streamflow
    '''Args.SetValue("Streamflow", pDataGroup)
    '''Set Unit
    '''Args.SetValue("EnglishUnit", True)
    '''Set station.txt
    '''Args.SetValue("Station File", atcUSGSStations.StationInfoFile)
    '''        If pMethods.Contains(BFMethods.BFIStandard) Then
    '''            Args.SetValue("BFIFrac", lFrac)
    '''        End If
    '''        If pMethods.Contains(BFMethods.BFIModified) Then
    '''            Args.SetValue("BFIK1Day", lK1Day)
    '''        End If
    '''        If pMethods.Contains(BFMethods.BFIStandard) OrElse pMethods.Contains(BFMethods.BFIModified) Then
    '''            Args.SetValue("BFINDay", lNDay)
    '''            Args.SetValue("BFIUseSymbol", (chkBFISymbols.Checked))
    '''Dim lBFIYearBasis As String = "Calendar"
    '''            If rdoBFIReportbyWaterYear.Checked Then
    '''                lBFIYearBasis = "Water"
    '''            End If
    '''            Args.SetValue("BFIReportby", lBFIYearBasis)
    '''        End If
    ''' </summary>
    ''' <param name="aArgs"></param>
    ''' <remarks></remarks>
    Public Sub ComputeBaseflow(ByVal aArgs As atcDataAttributes)
        Dim lClsBaseFlowCalculator As New atcTimeseriesBaseflow.atcTimeseriesBaseflow
        Try
            lClsBaseFlowCalculator.Open("Baseflow", aArgs)
            Dim lOldDataSource As atcDataSource = Nothing
            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                If lDataSource.Specification = lClsBaseFlowCalculator.Specification Then
                    lOldDataSource = lDataSource
                    Exit For
                End If
            Next
            If lOldDataSource IsNot Nothing Then
                lOldDataSource.Clear()
                atcDataManager.DataSources.Remove(lOldDataSource)
            End If

            'lClsBaseFlowCalculator.DataSets.Clear()
            'Add new results to Datasets
            atcDataManager.DataSources.Add(lClsBaseFlowCalculator)

        Catch ex As Exception
            Logger.Msg("Baseflow separation failed: " & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Baseflow separation")
        End Try
        'If pDidBFSeparation Then
        '    Logger.Msg("Baseflow separation is successful.", MsgBoxStyle.OkOnly, "Baseflow Separation")
        'End If
    End Sub

    Public Sub ASCIICommon(ByVal aTs As atcTimeseries)

        If Not IO.Directory.Exists(OutputDir) Then
            Exit Sub
        End If

        'Organize data
        Dim lStart As Double = -99.9
        Dim lEnd As Double = -99.9
        Dim lDA As Double = -99.9

        Dim lTsGroupPart As atcCollection = ConstructReportTsGroup(aTs, BFMethods.PART, lStart, lEnd, lDA)
        Dim lTsGroupFixed As atcCollection = ConstructReportTsGroup(aTs, BFMethods.HySEPFixed, lStart, lEnd, lDA)
        Dim lTsGroupLocMin As atcCollection = ConstructReportTsGroup(aTs, BFMethods.HySEPLocMin, lStart, lEnd, lDA)
        Dim lTsGroupSlide As atcCollection = ConstructReportTsGroup(aTs, BFMethods.HySEPSlide, lStart, lEnd, lDA)
        Dim lTsGroupBFIStandard As atcCollection = ConstructReportTsGroup(aTs, BFMethods.BFIStandard, lStart, lEnd, lDA)
        Dim lTsGroupBFIModified As atcCollection = ConstructReportTsGroup(aTs, BFMethods.BFIModified, lStart, lEnd, lDA)

        If (lStart < 0 AndAlso lEnd < 0) OrElse lDA <= 0 Then Exit Sub

        If lTsGroupFixed.Count > 0 Then ASCIIOriginal(aTs, BFMethods.HySEPFixed)
        If lTsGroupLocMin.Count > 0 Then ASCIIOriginal(aTs, BFMethods.HySEPLocMin)
        If lTsGroupSlide.Count > 0 Then ASCIIOriginal(aTs, BFMethods.HySEPSlide)
        If lTsGroupPart.Count > 0 Then ASCIIOriginal(aTs, BFMethods.PART)
        If lTsGroupBFIStandard.Count > 0 Then ASCIIOriginal(aTs, BFMethods.BFIStandard)
        If lTsGroupBFIModified.Count > 0 Then ASCIIOriginal(aTs, BFMethods.BFIModified)

        Dim lConversionFactor As Double = pUADepth / lDA
        Dim lTsFlowDaily As atcTimeseries = SubsetByDate(aTs, lStart, lEnd, Nothing)
        Dim lTsFlowDailyDepth As atcTimeseries = lTsFlowDaily * lConversionFactor

        Dim lTsFlowMonthly As atcTimeseries = Aggregate(lTsFlowDaily, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lTsFlowMonthlySum As atcTimeseries = Aggregate(lTsFlowDaily, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lTsFlowMonthlyDepth As atcTimeseries = lTsFlowMonthlySum * lConversionFactor

        Dim lTsFlowDailyBnd As atcTimeseries = SubsetByDateBoundary(lTsFlowDaily, 1, 1, Nothing)
        Dim lTsFlowYearly As atcTimeseries = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        Dim lTsFlowYearlySum As atcTimeseries = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lTsFlowYearlyDepth As atcTimeseries = lTsFlowYearlySum * lConversionFactor

        Dim lTsGroupStreamFlow As New atcCollection
        With lTsGroupStreamFlow
            .Add("RateDaily", lTsFlowDaily)
            .Add("DepthDaily", lTsFlowDailyDepth)
            .Add("RateMonthly", lTsFlowMonthly)
            .Add("DepthMonthly", lTsFlowMonthlyDepth)
            .Add("RateYearly", lTsFlowYearly)
            .Add("DepthYearly", lTsFlowYearlyDepth)
        End With

        Dim lFileDailySum As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_Daily.csv")
        Dim lFileMonthlySum As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_Monthly.csv")
        Dim lFileYearlySum As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_Yearly.csv")

        Dim lFileDuration As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_Duration.csv")

        Dim lMethodNames As New atcCollection
        With lMethodNames
            .Add(BFMethods.PART, "PART")
            .Add(BFMethods.HySEPFixed, "HySEP-Fixed")
            .Add(BFMethods.HySEPLocMin, "HySEP-LocMin")
            .Add(BFMethods.HySEPSlide, "HySEP-Slide")
            .Add(BFMethods.BFIStandard, "BFIStandard")
            .Add(BFMethods.BFIModified, "BFIModified")
        End With

        'header for data dump file
        Dim lNumColumns As Integer = 4 + MethodsLastDone.Count * 5
        Dim lTableHeader As New atcTableDelimited
        lTableHeader.Delimiter = ","
        lTableHeader.NumFields = lNumColumns

        'Need to rid of lingering results from a previous analysis run
        'based on the BF method(s) that user chose for the analysis JUST BEING DONE
        If Not MethodsLastDone.Contains(BFMethods.HySEPFixed) Then lTsGroupFixed.Clear()
        If Not MethodsLastDone.Contains(BFMethods.HySEPLocMin) Then lTsGroupLocMin.Clear()
        If Not MethodsLastDone.Contains(BFMethods.HySEPSlide) Then lTsGroupSlide.Clear()
        If Not MethodsLastDone.Contains(BFMethods.PART) Then lTsGroupPart.Clear()
        If Not MethodsLastDone.Contains(BFMethods.BFIStandard) Then lTsGroupBFIStandard.Clear()
        If Not MethodsLastDone.Contains(BFMethods.BFIModified) Then lTsGroupBFIModified.Clear()

        Dim lTableToReport As atcTableDelimited = ASCIICommonTable(lTsGroupStreamFlow, _
                                                                   lTsGroupPart, _
                                                                   lTsGroupFixed, _
                                                                   lTsGroupLocMin, _
                                                                   lTsGroupSlide, _
                                                                   lTsGroupBFIStandard, _
                                                                   lTsGroupBFIModified, _
                                                                   "Daily")
        Dim lMethodLabelColumnStart As Integer = 7
        Dim lConsLabelColumnStart As Integer = 5
        Dim lUnitsLabelColumnStarts As Integer = 5
        Dim lColumnsPerMethod As Integer = 5
        With lTableHeader
            For lRow As Integer = 1 To 3
                lTableHeader.CurrentRecord = lRow
                If lRow = 2 Then
                    .Value(3) = "Streamflow"
                ElseIf lRow = 3 Then
                    .Value(1) = "Day"
                    .Value(2) = "Date"
                    .Value(3) = "CFS"
                    .Value(4) = "IN"
                End If
                For Each lMethodKey As BFMethods In MethodsLastDone
                    Select Case lRow
                        Case 1
                            .Value(lMethodLabelColumnStart) = lMethodNames.ItemByKey(lMethodKey)
                            lMethodLabelColumnStart += lColumnsPerMethod
                        Case 2
                            .Value(lConsLabelColumnStart) = "Baseflow"
                            .Value(lConsLabelColumnStart + 2) = "Runoff"
                            .Value(lConsLabelColumnStart + 4) = "BFP"
                            lConsLabelColumnStart += lColumnsPerMethod
                        Case 3
                            .Value(lUnitsLabelColumnStarts) = "CFS"
                            .Value(lUnitsLabelColumnStarts + 1) = "IN"
                            .Value(lUnitsLabelColumnStarts + 2) = "CFS"
                            .Value(lUnitsLabelColumnStarts + 3) = "IN"
                            .Value(lUnitsLabelColumnStarts + 4) = "(%)"
                            lUnitsLabelColumnStarts += lColumnsPerMethod
                    End Select
                Next
            Next
        End With

        Dim lLocation As String = aTs.Attributes.GetValue("Location", "Unknown")
        Dim lStaNam As String = aTs.Attributes.GetValue("STANAM", "Unknown")
        Dim lTitleLine1 As String = "Groundwater Toolbox daily output for hydrograph separation."
        Dim lTitleLine2 As String = "Station: " & lLocation & " " & lStaNam.Replace(",", " ")
        Dim lTitleLine3 As String = "Drainage area: " & DoubleToString(lDA, , "0.0") & " square miles"
        Dim lTitleLine4 As String = "(CFS: cubic feet per second; IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"

        Dim lSW As New IO.StreamWriter(lFileDailySum, False)
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        lSW.WriteLine(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        lTableToReport.ClearData()
        lTableToReport = ASCIICommonTable(lTsGroupStreamFlow, _
                                          lTsGroupPart, _
                                          lTsGroupFixed, _
                                          lTsGroupLocMin, _
                                          lTsGroupSlide, _
                                          lTsGroupBFIStandard, _
                                          lTsGroupBFIModified, _
                                          "Monthly")
        lTableHeader.CurrentRecord = 3
        lTableHeader.Value(1) = "Month"
        lSW = New IO.StreamWriter(lFileMonthlySum, False)
        lTitleLine1 = lTitleLine1.Replace("daily", "monthly")
        lTitleLine4 = "(CFS: average flow for the month (cubic feet per second); IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        lSW.Write(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        lTableToReport.ClearData()
        lTableToReport = ASCIICommonTable(lTsGroupStreamFlow, _
                                          lTsGroupPart, _
                                          lTsGroupFixed, _
                                          lTsGroupLocMin, _
                                          lTsGroupSlide, _
                                          lTsGroupBFIStandard, _
                                          lTsGroupBFIModified, _
                                          "Yearly")
        lTableHeader.CurrentRecord = 3
        lTableHeader.Value(1) = "Year"
        lSW = New IO.StreamWriter(lFileYearlySum, False)
        lTitleLine1 = "Groundwater Toolbox annual output for hydrograph separation (calendar year January 1-December 31)"
        lTitleLine4 = "(CFS: average flow for the year (cubic feet per second); IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        lSW.WriteLine(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        'Write Duration Tables
        'header for duration file
        lNumColumns = 2 + MethodsLastDone.Count * 2
        lMethodLabelColumnStart = 3
        lConsLabelColumnStart = 3
        lColumnsPerMethod = 2
        Dim lTableHeaderDuration As New atcTableDelimited
        With lTableHeaderDuration
            .Delimiter = ","
            .NumFields = lNumColumns
            For lRow As Integer = 1 To 2
                .CurrentRecord = lRow
                If lRow = 1 Then
                    .Value(1) = "Percent"
                ElseIf lRow = 2 Then
                    .Value(1) = "exceedance"
                    .Value(2) = "Streamflow"
                End If
                For Each lMethodKey As BFMethods In MethodsLastDone
                    Select Case lRow
                        Case 1
                            .Value(lMethodLabelColumnStart) = lMethodNames.ItemByKey(lMethodKey)
                            lMethodLabelColumnStart += lColumnsPerMethod
                        Case 2
                            .Value(lConsLabelColumnStart) = "Baseflow"
                            .Value(lConsLabelColumnStart + 1) = "Runoff"
                            lConsLabelColumnStart += lColumnsPerMethod
                    End Select
                Next 'method
            Next 'row
        End With 'lTableHeaderDuration

        lTableToReport.Clear()

        lTitleLine1 = "Groundwater Toolbox daily output for hydrograph separation."
        lTitleLine2 = "Station: " & lLocation & " " & lStaNam.Replace(",", " ")
        lTitleLine3 = "Drainage area: " & DoubleToString(lDA, , "0.0") & " square miles"
        lTitleLine4 = "Period of analysis: " & DumpDate(lStart) & " to " & DumpDate(lEnd)
        Dim lTitleLine5 As String = "Percent exceedence: Percentage of time flow was equaled or exceeded"
        Dim lTitleLine6 As String = "Flow: in cubic feet per second"

        lSW = New System.IO.StreamWriter(lFileDuration, False)
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3)
        lSW.WriteLine(lTitleLine4) : lSW.WriteLine(lTitleLine5) : lSW.WriteLine(lTitleLine6)

        lSW.WriteLine("****   Daily Duration Table  ****")
        lTableToReport = ASCIICommonDurationTable(lTsGroupStreamFlow, _
                                                  lTsGroupPart, _
                                                  lTsGroupFixed, _
                                                  lTsGroupLocMin, _
                                                  lTsGroupSlide, _
                                                  lTsGroupBFIStandard, _
                                                  lTsGroupBFIModified, _
                                                  "Daily")
        lSW.WriteLine(lTableHeaderDuration.ToString)
        lSW.WriteLine(lTableToReport.ToString)

        'lTableToReport.ClearData()
        'lSW.WriteLine("****   Monthly Duration Table  ****")
        'lTableToReport = ASCIICommonDurationTable(lTsGroupStreamFlow, lTsGroupPart, lTsGroupFixed, lTsGroupLocMin, lTsGroupSlide, "Monthly")
        'lSW.WriteLine(lTableHeaderDuration.ToString)
        'lSW.WriteLine(lTableToReport.ToString)

        'lTableToReport.ClearData()
        'lSW.WriteLine("****   Annual Duration Table  ****")
        'lTableToReport = ASCIICommonDurationTable(lTsGroupStreamFlow, lTsGroupPart, lTsGroupFixed, lTsGroupLocMin, lTsGroupSlide, "Yearly")
        'lSW.WriteLine(lTableHeaderDuration.ToString)
        'lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

    End Sub

    Private Function ASCIIOriginal(ByVal aStreamFlowTs As atcTimeseries, ByVal aMethod As BFMethods) As Boolean

        Dim lSpecification As String = ""
        Dim lMethodName As String = ""
        Select Case aMethod
            Case BFMethods.HySEPFixed : lMethodName = "HySEPFixed"
            Case BFMethods.HySEPLocMin : lMethodName = "HySEPLocMin"
            Case BFMethods.HySEPSlide : lMethodName = "HySEPSlide"
            Case BFMethods.PART : lMethodName = "Part"
            Case BFMethods.BFIStandard : lMethodName = "BFIStandard"
            Case BFMethods.BFIModified : lMethodName = "BFIModified"
        End Select
        'Write original HySEP and PART's output files
        If aMethod = BFMethods.HySEPFixed OrElse _
           aMethod = BFMethods.HySEPLocMin OrElse _
           aMethod = BFMethods.HySEPSlide Then
            Dim lFilename As String
            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".SBF")
            ASCIIHySepBSF(aStreamFlowTs, lFilename, aMethod)
            Dim lFilenamePrt As String = IO.Path.ChangeExtension(lFilename, "PRT")
            ASCIIHySepMonthly(aStreamFlowTs, lFilenamePrt, aMethod)
            lSpecification = lFilename

            'If chkTabDelimited.Checked Then
            'lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_tab" & ".SBF")
            'ASCIIHySepDelimited(aBaseFlowTsGroup(0), lFilename)
            'End If

            'With cdlg
            '    lFilename = AbsolutePath(lFilename, CurDir)
            '    .FileName = lFilename
            '    .Filter = ""
            '    '.FilterIndex = 0
            '    .DefaultExt = "SBF"
            'End With
        ElseIf aMethod = BFMethods.PART Then
            'With cdlg
            '    lFilename = AbsolutePath(lFilename, CurDir)
            '    .FileName = lFilename
            '    .Filter = ""
            '    '.FilterIndex = 0
            '    .DefaultExt = "SBF"
            'End With
            Dim lFilename As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partday.txt")
            ASCIIPartDaily(aStreamFlowTs, lFilename)
            'If chkTabDelimited.Checked Then
            '    lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partday_tab.txt")
            '    ASCIIPartDailyDelimited(pDataGroup(0), lFilename)
            'End If

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partmon.txt")
            lSpecification = lFilename
            ASCIIPartMonthly(aStreamFlowTs, lFilename)
            'If chkTabDelimited.Checked Then
            '    lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partmon_tab.txt")
            '    ASCIIPartMonthlyDelimited(pDataGroup(0), lFilename)
            'End If

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partqrt.txt")
            ASCIIPartQuarterly(aStreamFlowTs, lFilename)
            'If chkTabDelimited.Checked Then
            '    lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partqrt_tab.txt")
            '    ASCIIPartQuarterlyDelimited(pDataGroup(0), lFilename)
            'End If

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partWY.txt")
            ASCIIPartWaterYear(aStreamFlowTs, lFilename)
            'If chkTabDelimited.Checked Then
            '    lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partWY_tab.txt")
            '    ASCIIPartWaterYearDelim(pDataGroup(0), lFilename)
            'End If

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_partsum.txt")
            ASCIIPartBFSum(aStreamFlowTs, lFilename)
        ElseIf aMethod = BFMethods.BFIStandard OrElse aMethod = BFMethods.BFIModified Then
            Dim lFilename As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".q")
            ASCIIBFIDaily(aStreamFlowTs, lFilename, lMethodName)

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".bfi")
            ASCIIBFI(aStreamFlowTs, lFilename, lMethodName)
            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".tp")
            ASCIIBFITp(aStreamFlowTs, lFilename, lMethodName)

        End If

        'With cdlg
        '    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        lFilename = AbsolutePath(.FileName, CurDir)
        '        aFilterIndex = .FilterIndex
        '        Logger.Dbg("User specified file '" & lFilename & "'")
        '        Logger.LastDbgText = ""
        '    Else 'Return empty string if user clicked Cancel
        '        lFilename = ""
        '        Logger.Dbg("User Cancelled File Selection Dialog for " & aFileDialogTitle)
        '        Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
        '    End If
        'End With

        'Dim lProcess As New Process
        'With lProcess
        '    .StartInfo.FileName = "Notepad.exe"
        '    .StartInfo.Arguments = lSpecification
        '    Try
        '        .Start()
        '    Catch lException As System.SystemException
        '        'Dim lExtension As String = FileExt(lSpecification)
        '        'lProcess.StartInfo.FileName = "Notepad.exe"
        '        'lProcess.StartInfo.Arguments = lSpecification
        '        'lProcess.Start()
        '        .Dispose()
        '    End Try
        'End With
    End Function

    Public Function ASCIICommonDurationTable(ByVal aTsGroupStreamFlow As atcCollection, _
                                 ByVal aTsGroupPart As atcCollection, _
                                 ByVal aTsGroupFixed As atcCollection, _
                                 ByVal aTsGroupLocMin As atcCollection, _
                                 ByVal aTsGroupSlide As atcCollection, _
                                 ByVal aTsGroupBFIStandard As atcCollection, _
                                 ByVal aTsGroupBFIModified As atcCollection, _
                                 ByVal ATStep As String) As atcTableDelimited

        Dim lTsFlow As atcTimeseries = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep)

        Dim lTsBFPart As atcTimeseries = Nothing
        Dim lTsROPart As atcTimeseries = Nothing
        Dim lExceedanceListing As New atcCollection
        Dim lResult As atcCollection = ConstructExceedanceListing(lTsFlow, "Observed", "StreamFlow")
        lExceedanceListing.AddRange(lResult.Keys, lResult)

        If aTsGroupPart.Count > 0 Then
            lTsBFPart = aTsGroupPart.ItemByKey("Rate" & ATStep)
            lTsROPart = lTsFlow - lTsBFPart
            lResult = ConstructExceedanceListing(lTsBFPart, "Part", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROPart, "Part", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If

        Dim lTsBFFixed As atcTimeseries = Nothing
        Dim lTsROFixed As atcTimeseries = Nothing
        If aTsGroupFixed.Count > 0 Then
            lTsBFFixed = aTsGroupFixed.ItemByKey("Rate" & ATStep)
            lTsROFixed = lTsFlow - lTsBFFixed
            lResult = ConstructExceedanceListing(lTsBFFixed, "Fixed", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROFixed, "Fixed", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If
        Dim lTsBFLocMin As atcTimeseries = Nothing
        Dim lTsROLocMin As atcTimeseries = Nothing
        If aTsGroupLocMin.Count > 0 Then
            lTsBFLocMin = aTsGroupLocMin.ItemByKey("Rate" & ATStep)
            lTsROLocMin = lTsFlow - lTsBFLocMin
            lResult = ConstructExceedanceListing(lTsBFLocMin, "LocMin", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROLocMin, "LocMin", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If
        Dim lTsBFSlide As atcTimeseries = Nothing
        Dim lTsROSlide As atcTimeseries = Nothing
        If aTsGroupSlide.Count > 0 Then
            lTsBFSlide = aTsGroupSlide.ItemByKey("Rate" & ATStep)
            lTsROSlide = lTsFlow - lTsBFSlide
            lResult = ConstructExceedanceListing(lTsBFSlide, "Slide", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROSlide, "Slide", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If

        Dim lTsBFBFIStandard As atcTimeseries = Nothing
        Dim lTsROBFIStandard As atcTimeseries = Nothing
        If aTsGroupBFIStandard.Count > 0 Then
            lTsBFBFIStandard = aTsGroupBFIStandard.ItemByKey("Rate" & ATStep)
            lTsROBFIStandard = lTsFlow - lTsBFBFIStandard
            lResult = ConstructExceedanceListing(lTsBFBFIStandard, "BFIStandard", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROBFIStandard, "BFIStandard", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If
        Dim lTsBFBFIModified As atcTimeseries = Nothing
        Dim lTsROBFIModified As atcTimeseries = Nothing
        If aTsGroupBFIModified.Count > 0 Then
            lTsBFBFIModified = aTsGroupBFIModified.ItemByKey("Rate" & ATStep)
            lTsROBFIModified = lTsFlow - lTsBFBFIModified
            lResult = ConstructExceedanceListing(lTsBFBFIModified, "BFIModified", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROBFIModified, "BFIModified", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If

        'set up table
        Dim lNumColumns As Integer = 2 + MethodsLastDone.Count * 2
        Dim lColumnsPerMethod As Integer = 2
        Dim lTableBody As New atcTableDelimited
        With lTableBody
            .Delimiter = ","
            .NumFields = lNumColumns
            .CurrentRecord = 1
            Dim lNumEntries As Integer = lExceedanceListing.ItemByKey("X_Observed_StreamFlow").Length - 1
            For I As Integer = 0 To lNumEntries
                .Value(1) = DoubleToString(lExceedanceListing.Item(0)(I) * 100)
                .Value(2) = lExceedanceListing.Item(1)(I)
                Dim lLastColumn As Integer = 2
                If aTsGroupPart.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_Part_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_Part_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupFixed.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_Fixed_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_Fixed_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupLocMin.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_LocMin_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_LocMin_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupSlide.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_Slide_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_Slide_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupBFIStandard.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_BFIStandard_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_BFIStandard_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupBFIModified.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_BFIModified_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_BFIModified_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If
                .CurrentRecord += 1
            Next 'exceedance level or probability threshold
        End With

        Return lTableBody
    End Function

    Private Function ConstructExceedanceListing(ByVal aTs As atcTimeseries, ByVal aMethod As String, ByVal aCons As String) As atcCollection
        Dim lNumProbabilityPoints As Integer = 30 '200
        Dim lExceedance As Boolean = False
        Dim lZgc As New ZedGraph.ZedGraphControl
        Dim lX(lNumProbabilityPoints) As Double
        Dim lLastIndex As Integer = lX.GetUpperBound(0)
        Dim lPane As ZedGraph.GraphPane = lZgc.MasterPane.PaneList(0)
        Dim lXScale As ZedGraph.ProbabilityScale
        With lPane.XAxis
            If .Type <> ZedGraph.AxisType.Probability Then
                .Type = ZedGraph.AxisType.Probability
                With .MajorTic
                    .IsInside = True
                    .IsCrossInside = True
                    .IsOutside = False
                    .IsCrossOutside = False
                End With
                lXScale = .Scale
                lXScale.standardDeviations = 3
                'lXScale.IsReverse = True
            End If

            For lXindex As Integer = 0 To lLastIndex
                lX(lXindex) = 100 * .Scale.DeLinearize(lXindex / CDbl(lLastIndex))
            Next
        End With
        Dim lAttributeName As String
        Dim lIndex As Integer
        Dim lXFracExceed() As Double
        Dim lY() As Double

        ReDim lY(lLastIndex)
        ReDim lXFracExceed(lLastIndex)

        For lIndex = 0 To lLastIndex
            If lExceedance Then
                lXFracExceed(lIndex) = (100 - lX(lIndex)) / 100
                lAttributeName = "%" & Format(lX(lIndex), "00.####")
            Else
                lXFracExceed(lIndex) = lX(lIndex) / 100
                lAttributeName = "%" & Format(100 - lX(lIndex), "00.####")
            End If

            'lAttributeName = "%" & Format(lX(lIndex), "00.####")
            lY(lIndex) = aTs.Attributes.GetValue(lAttributeName)
            'Logger.Dbg(lAttributeName & " = " & lY(lIndex) & _
            '                            " : " & lX(lIndex) & _
            '                            " : " & lXFracExceed(lIndex))
        Next
        Dim lExceedanceListing As New atcCollection
        lExceedanceListing.Add("X_" & aMethod & "_" & aCons, lXFracExceed) 'probability
        lExceedanceListing.Add("Y_" & aMethod & "_" & aCons, lY) 'threshold value
        lZgc.Dispose()
        lZgc = Nothing
        Return lExceedanceListing
    End Function

    Private Function ASCIICommonTable(ByVal aTsGroupStreamFlow As atcCollection, _
                                 ByVal aTsGroupPart As atcCollection, _
                                 ByVal aTsGroupFixed As atcCollection, _
                                 ByVal aTsGroupLocMin As atcCollection, _
                                 ByVal aTsGroupSlide As atcCollection, _
                                 ByVal aTsGroupBFIStandard As atcCollection, _
                                 ByVal aTsGroupBFIModified As atcCollection, _
                                 ByVal ATStep As String) As atcTableDelimited
        'set up table
        Dim lNumColumns As Integer = 4 + MethodsLastDone.Count * 5
        Dim lTableBody As New atcTableDelimited
        lTableBody.Delimiter = ","
        lTableBody.NumFields = lNumColumns
        lTableBody.CurrentRecord = 1

        Dim lDate(5) As Integer

        Dim lBF As Double
        Dim lBFDepth As Double
        Dim lBFPct As Double = 0.0

        Dim lTsFlow As atcTimeseries = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep)
        Dim lTsFlowDepth As atcTimeseries = aTsGroupStreamFlow.ItemByKey("Depth" & ATStep)
        Dim lFlowStartDate As Double = lTsFlow.Dates.Value(0)
        Dim lFlowEndDate As Double = lTsFlow.Dates.Value(lTsFlow.numValues)
        If aTsGroupBFIStandard.Count > 0 Then
            AdjustDates(aTsGroupBFIStandard, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate)
        End If
        If aTsGroupBFIModified.Count > 0 Then
            AdjustDates(aTsGroupBFIModified, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate)
        End If
        For I As Integer = 1 To lTsFlow.numValues
            If ATStep = "Monthly" Then
                If ASCIICommonTableSkipOneRow(aTsGroupPart, _
                                 aTsGroupFixed, _
                                 aTsGroupLocMin, _
                                 aTsGroupSlide, _
                                 aTsGroupBFIStandard, _
                                 aTsGroupBFIModified, _
                                 ATStep, _
                                 I) Then
                    Continue For
                End If
            End If
            J2Date(lTsFlow.Dates.Value(I - 1), lDate)
            With lTableBody
                .Value(1) = I
                Select Case ATStep
                    Case "Daily" : .Value(2) = lDate(0) & "-" & lDate(1).ToString.PadLeft(2, "0") & "-" & lDate(2).ToString.PadLeft(2, "0")
                    Case "Monthly" : .Value(2) = lDate(1).ToString.PadLeft(2, "0") & "-" & lDate(0)
                    Case "Yearly" : .Value(2) = lDate(0)
                    Case Else : .Value(2) = lDate(0) & "-" & lDate(1) & "-" & lDate(2)
                End Select

                .Value(3) = DoubleToString(lTsFlow.Value(I), , "0.0")
                .Value(4) = DoubleToString(lTsFlowDepth.Value(I), , "0.00")
                Dim lLastColumn As Integer = 4
                If aTsGroupPart.Count > 0 Then
                    lBF = aTsGroupPart.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupPart.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += 5
                End If
                If aTsGroupFixed.Count > 0 Then
                    lBF = aTsGroupFixed.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupFixed.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += 5
                End If
                If aTsGroupLocMin.Count > 0 Then
                    lBF = aTsGroupLocMin.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupLocMin.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += 5
                End If
                If aTsGroupSlide.Count > 0 Then
                    lBF = aTsGroupSlide.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupSlide.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += 5
                End If
                If aTsGroupBFIStandard.Count > 0 Then
                    lBF = aTsGroupBFIStandard.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupBFIStandard.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += 5 'second to last column to have this jump
                End If
                If aTsGroupBFIModified.Count > 0 Then
                    lBF = aTsGroupBFIModified.ItemByKey("Rate" & ATStep).Value(I)
                    lBFDepth = aTsGroupBFIModified.ItemByKey("Depth" & ATStep).Value(I)
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                End If
                .CurrentRecord += 1
            End With
        Next
        Return lTableBody
    End Function

    Private Function ASCIICommonTableSkipOneRow(ByVal aTsGroupPart As atcCollection, _
                                 ByVal aTsGroupFixed As atcCollection, _
                                 ByVal aTsGroupLocMin As atcCollection, _
                                 ByVal aTsGroupSlide As atcCollection, _
                                 ByVal aTsGroupBFIStandard As atcCollection, _
                                 ByVal aTsGroupBFIModified As atcCollection, _
                                 ByVal ATStep As String, _
                                 ByVal I As Integer) As Boolean
        Dim lBF, lBFDepth As Double
        If aTsGroupPart.Count > 0 Then
            lBF = aTsGroupPart.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupPart.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        If aTsGroupFixed.Count > 0 Then
            lBF = aTsGroupFixed.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupFixed.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        If aTsGroupLocMin.Count > 0 Then
            lBF = aTsGroupLocMin.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupLocMin.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        If aTsGroupSlide.Count > 0 Then
            lBF = aTsGroupSlide.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupSlide.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        If aTsGroupBFIStandard.Count > 0 Then
            lBF = aTsGroupBFIStandard.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupBFIStandard.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        If aTsGroupBFIModified.Count > 0 Then
            lBF = aTsGroupBFIModified.ItemByKey("Rate" & ATStep).Value(I)
            lBFDepth = aTsGroupBFIModified.ItemByKey("Depth" & ATStep).Value(I)
            If lBF < 0 OrElse lBFDepth < 0 Then Return True
        End If
        Return False
    End Function

    Private Sub ASCIICommonTableOneRow(ByVal aTable As atcTableDelimited, _
                                       ByVal aTSFlow As atcTimeseries, _
                                       ByVal aTSFlowDepth As atcTimeseries, _
                                       ByVal aInd As Integer, _
                                       ByVal ATStep As String, _
                                       ByVal aBF As Double, _
                                       ByVal aBFDepth As Double, _
                                       ByVal aLastColumn As Integer)
        Dim lRO, lRODepth, lBFPct As Double

        If aBF < 0 OrElse aBFDepth < 0 Then
            aBF = -99 : aBFDepth = -99
            lRO = -99 : lRODepth = -99
            lBFPct = -99
        Else
            lRO = aTSFlow.Value(aInd) - aBF
            lRODepth = aTSFlowDepth.Value(aInd) - aBFDepth
            If aBF > 0 AndAlso aTSFlow.Value(aInd) > 0 Then
                lBFPct = aBF / aTSFlow.Value(aInd) * 100
            Else
                lBFPct = 0
            End If
        End If
        With aTable
            If (ATStep = "Monthly" OrElse ATStep = "Yearly") AndAlso (aBF < 0 OrElse aBFDepth < 0) Then
                'For incomplete Monthly timestep, simply put up blank cells
                .Value(aLastColumn + 1) = ""
                .Value(aLastColumn + 2) = ""
                .Value(aLastColumn + 3) = ""
                .Value(aLastColumn + 4) = ""
                .Value(aLastColumn + 5) = ""
            Else
                .Value(aLastColumn + 1) = DoubleToString(aBF, , "0.00")
                .Value(aLastColumn + 2) = DoubleToString(aBFDepth, , "0.00")
                .Value(aLastColumn + 3) = DoubleToString(lRO, , "0.00")
                .Value(aLastColumn + 4) = DoubleToString(lRODepth, , "0.00")
                .Value(aLastColumn + 5) = DoubleToString(lBFPct, , "0.0")
            End If
        End With
    End Sub
    ''' <summary>
    ''' This routine is to make date range adjustment to BFI analysis.
    ''' The only adjustment is to make sure the BFI baseflow and depth Tsers
    ''' are of the same duration as the flow data going into those analysis.
    ''' The BFI method only back fill at the beginning of the flow Tsers
    ''' to either Jan 1 or Oct 1 for calendar- or water-year based analysis.
    ''' The end date is not adjusted. Hence, a simple 'subsetbydate' would work here.
    ''' </summary>
    ''' <param name="aTsGroupPerMethod">A collection of the BFI related timeseries</param>
    ''' <param name="aTsFlow">The streamflow Tser based on which BFI is carried out</param>
    ''' <param name="aTStep">A string that signifies the time step for a output, ie. Daily, Monthly, or Yearly</param>
    ''' <param name="aFlowStartDate">Start date of the streamflow Tser</param>
    ''' <param name="aFlowEndDate">End date of the streamflow Tser</param>
    ''' <remarks></remarks>
    Private Sub AdjustDates(ByRef aTsGroupPerMethod As atcCollection, ByVal aTsFlow As atcTimeseries, ByVal aTStep As String, ByVal aFlowStartDate As Double, ByVal aFlowEndDate As Double)
        Dim lTsBFTemp As atcTimeseries = aTsGroupPerMethod.ItemByKey("Rate" & aTStep)
        Dim lTsBFDepthTemp As atcTimeseries = aTsGroupPerMethod.ItemByKey("Depth" & aTStep)
        If lTsBFTemp.Dates.Value(0) <> aFlowStartDate OrElse lTsBFTemp.Dates.Value(lTsBFTemp.numValues) <> aFlowEndDate Then
            aTsGroupPerMethod.ItemByKey("Rate" & aTStep) = SubsetByDate(lTsBFTemp, aFlowStartDate, aFlowEndDate, Nothing)
        End If
        If lTsBFDepthTemp.Dates.Value(0) <> aFlowStartDate OrElse lTsBFDepthTemp.Dates.Value(lTsBFDepthTemp.numValues) <> aFlowEndDate Then
            aTsGroupPerMethod.ItemByKey("Depth" & aTStep) = SubsetByDate(lTsBFDepthTemp, aFlowStartDate, aFlowEndDate, Nothing)
        End If
        'If lTsBFTemp.Dates.Value(0) <> aFlowStartDate OrElse lTsBFTemp.Dates.Value(lTsBFTemp.numValues) <> aFlowEndDate Then
        '    Dim lTsG As New atcTimeseriesGroup()
        '    With lTsG
        '        .Add(aTsFlow)
        '        .Add(lTsBFTemp)
        '        .Add(lTsBFDepthTemp)
        '    End With
        '    Dim lFirstStart As Double
        '    Dim lLastEnd As Double
        '    Dim lComStart As Double
        '    Dim lComEnd As Double
        '    CommonDates(lTsG, lFirstStart, lLastEnd, lComStart, lComEnd)
        '    Dim lnewts As atcTimeseries = NewTimeseries(lFirstStart, lLastEnd, aTsFlow.Attributes.GetValue("tu"), 1, Nothing, -99.0)

        '    If lFirstStart <> lTsBFTemp.Dates.Value(0) OrElse lLastEnd <> lTsBFTemp.Dates.Value(lTsBFTemp.numValues) Then
        '        lTsG.Clear()
        '        lTsG.Add(lTsBFTemp)
        '        lTsG.Add(lnewts)
        '        lTsBFTemp = MergeTimeseries(lTsG)
        '        aTsGroupPerMethod.ItemByKey("Rate" & aTStep) = SubsetByDate(lTsBFTemp, aFlowStartDate, aFlowEndDate, Nothing)
        '    End If
        '    If lFirstStart <> lTsBFDepthTemp.Dates.Value(0) OrElse lLastEnd <> lTsBFDepthTemp.Dates.Value(lTsBFDepthTemp.numValues) Then
        '        lTsG.Clear()
        '        lTsG.Add(lTsBFDepthTemp)
        '        lTsG.Add(lnewts)
        '        lTsBFDepthTemp = MergeTimeseries(lTsG)
        '        aTsGroupPerMethod.ItemByKey("Depth" & aTStep) = SubsetByDate(lTsBFDepthTemp, aFlowStartDate, aFlowEndDate, Nothing)
        '    End If
        '    'lnewts.Clear()
        '    lTsG.Clear()
        'End If
    End Sub
    Private Function ConstructReportTsGroup(ByVal aTs As atcTimeseries, ByVal aMethod As BFMethods, _
                                            Optional ByRef aStart As Double = 0.0, _
                                            Optional ByRef aEnd As Double = 0.0, _
                                            Optional ByRef aDA As Double = 0.0) As atcCollection

        'use a new ts group to hold the final ts for report

        Dim lDA As Double
        Dim lConversionFactor As Double
        Dim lTsGroupToReport = New atcCollection

        Dim lObj As Object = aTs.Attributes.GetDefinedValue("Baseflow")
        If lObj Is Nothing Then Return Nothing
        Dim lTsBFGroup As atcTimeseriesGroup = lObj.Value
        If lTsBFGroup Is Nothing OrElse lTsBFGroup.Count = 0 Then Return lTsGroupToReport

        Dim lMethodConsUnit As String = ""
        Dim lReportColumnAttributeName As String = "ReportColumn"

        Dim lMatchBFTsGroup As New atcTimeseriesGroup
        For Each lTs As atcTimeseries In lTsBFGroup
            If lTs.Attributes.GetValue("Method") = aMethod Then
                lMatchBFTsGroup.Add(lTs)
            End If
        Next
        If lMatchBFTsGroup.Count > 0 Then
            If aMethod = BFMethods.PART Then
                Dim lTsBFToReportPartDaily1 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily1")(0)
                Dim lTsBFToReportPartDaily2 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily2")(0)
                Dim lTsBFToReportPartMonthly As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartMonthlyInterpolated")(0)
                Dim lTsBFToReportPartMonthlyDepth As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartMonthlyDepth")(0)

                Dim linearSlope As Double = lTsBFToReportPartMonthly.Attributes.GetValue("LinearSlope")
                Dim lTsBFToReportPartDaily As atcTimeseries = lTsBFToReportPartDaily1.Clone
                For I As Integer = 1 To lTsBFToReportPartDaily1.numValues
                    lTsBFToReportPartDaily.Value(I) = lTsBFToReportPartDaily1.Value(I) + linearSlope * (lTsBFToReportPartDaily2.Value(I) - lTsBFToReportPartDaily1.Value(I))
                Next
                lDA = lTsBFToReportPartDaily1.Attributes.GetValue("Drainage Area")
                lConversionFactor = pUADepth / lDA
                Dim lTsBFToReportPartDailyDepth As atcTimeseries = lTsBFToReportPartDaily * lConversionFactor

                Dim lTsBFToReportPartDailyBnd As atcTimeseries = SubsetByDateBoundary(lTsBFToReportPartDaily, 1, 1, Nothing)
                Dim lTsBFToReportPartYearly As atcTimeseries = Aggregate(lTsBFToReportPartDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                Dim lTsBFToReportPartYearlySum As atcTimeseries = Aggregate(lTsBFToReportPartDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                Dim lTsBFToReportPartYearlyDepth As atcTimeseries = lTsBFToReportPartYearlySum * lConversionFactor

                'lTsBFToReportPartDaily.Attributes.SetValue(lReportColumnAttributeName, "RateDaily")
                'lTsBFToReportPartDailyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthDaily")
                'lTsBFToReportPartMonthly.Attributes.SetValue(lReportColumnAttributeName, "RateMonthly")
                'lTsBFToReportPartMonthlyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthMonthly")
                'lTsBFToReportPartYearly.Attributes.SetValue(lReportColumnAttributeName, "RateYearly")
                'lTsBFToReportPartYearlyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthYearly")
                With lTsGroupToReport
                    .Add("RateDaily", lTsBFToReportPartDaily)
                    .Add("DepthDaily", lTsBFToReportPartDailyDepth)
                    .Add("RateMonthly", lTsBFToReportPartMonthly)
                    .Add("DepthMonthly", lTsBFToReportPartMonthlyDepth)
                    .Add("RateYearly", lTsBFToReportPartYearly)
                    .Add("DepthYearly", lTsBFToReportPartYearlyDepth)
                End With
            Else
                Dim lTsDaily As atcTimeseries = lMatchBFTsGroup(0)
                lDA = lTsDaily.Attributes.GetValue("Drainage Area", 0.0)

                'lConversionFactor = pUADepth / lDA
                'Dim lTsDailyDepth As atcTimeseries = lTsDaily * lConversionFactor
                'Dim lTsMon As atcTimeseries = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                'Dim lTsMonSum As atcTimeseries = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                'Dim lTsMonDepth As atcTimeseries = lTsMonSum * lConversionFactor
                'Dim lTsYear As atcTimeseries = Aggregate(lTsDaily, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                'Dim lTsYearSum As atcTimeseries = Aggregate(lTsDaily, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                'Dim lTsYearDepth As atcTimeseries = lTsYearSum * lConversionFactor

                Dim lTsDailyDepth As atcTimeseries = Nothing
                Dim lTsMon As atcTimeseries = Nothing
                Dim lTsMonSum As atcTimeseries = Nothing
                Dim lTsMonDepth As atcTimeseries = Nothing
                Dim lTsYear As atcTimeseries = Nothing
                Dim lTsYearSum As atcTimeseries = Nothing
                Dim lTsYearDepth As atcTimeseries = Nothing
                Dim lTsDailyBnd As atcTimeseries = SubsetByDateBoundary(lTsDaily, 1, 1, Nothing)

                If lDA > 0 Then
                    lConversionFactor = pUADepth / lDA
                    lTsDailyDepth = lTsDaily * lConversionFactor
                    lTsMon = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    lTsMonSum = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    lTsMonDepth = lTsMonSum * lConversionFactor

                    lTsYear = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                    lTsYearSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                    lTsYearDepth = lTsYearSum * lConversionFactor
                Else
                    lTsMon = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    lTsMonSum = Aggregate(lTsDaily, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)

                    lTsYear = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                    lTsYearSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                End If

                'lTsDaily.Attributes.SetValue(lReportColumnAttributeName, "RateDaily")
                'lTsDailyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthDaily")
                'lTsMon.Attributes.SetValue(lReportColumnAttributeName, "RateMonthly")
                'lTsMonDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthMonthly")
                'lTsYear.Attributes.SetValue(lReportColumnAttributeName, "RateYearly")
                'lTsYearDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthYearly")

                'Process HySep method's monthly values to set incomplete months value to -99.99
                If aMethod = BFMethods.HySEPFixed OrElse aMethod = BFMethods.HySEPLocMin OrElse aMethod = BFMethods.HySEPSlide Then
                    Dim lAnalysisStart As Double = lTsDaily.Attributes.GetValue("AnalysisStart", -99)
                    Dim lAnalysisEnd As Double = lTsDaily.Attributes.GetValue("AnalysisEnd", -99)
                    If lAnalysisStart > 0 AndAlso lAnalysisEnd > 0 AndAlso lAnalysisEnd > lAnalysisStart Then
                        Dim lDateStart(5) As Integer
                        Dim lDateEnd(5) As Integer
                        Dim lDateMon(5) As Integer
                        J2Date(lAnalysisStart, lDateStart)
                        J2Date(lAnalysisEnd, lDateEnd)
                        Dim lWholeMonthStart As Double = Date2J(lDateStart(0), lDateStart(1), 1, 0, 0, 0)
                        Dim lWholeMonthEnd As Double = Date2J(lDateEnd(0), lDateEnd(1), 24, 0, 0, 0)
                        Dim lSkipStartMonth As Boolean = False
                        Dim lSkipEndMonth As Boolean = False
                        If lAnalysisStart > lWholeMonthStart Then lSkipStartMonth = True
                        If lAnalysisEnd < lWholeMonthEnd Then lSkipEndMonth = True
                        If lSkipStartMonth OrElse lSkipEndMonth Then
                            Dim lDate As Double
                            For I As Integer = 1 To lTsMon.numValues
                                lDate = lTsMon.Dates.Value(I)
                                If lDate < lWholeMonthStart OrElse lDate > lWholeMonthEnd Then
                                    lTsMon.Value(I) = -99.99
                                    lTsMonDepth.Value(I) = -99.99
                                Else
                                    J2Date(lDate, lDateMon)
                                    timcnv(lDateMon)
                                    If lDateMon(0) = lDateStart(0) AndAlso lDateMon(1) = lDateStart(1) Then
                                        If lSkipStartMonth Then
                                            lTsMon.Value(I) = -99.99
                                            lTsMonDepth.Value(I) = -99.99
                                        End If
                                    ElseIf lDateMon(0) = lDateEnd(0) AndAlso lDateMon(1) = lDateEnd(1) Then
                                        If lSkipEndMonth Then
                                            lTsMon.Value(I) = -99.99
                                            lTsMonDepth.Value(I) = -99.99
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
                With lTsGroupToReport
                    .Add("RateDaily", lTsDaily)
                    .Add("DepthDaily", lTsDailyDepth)
                    .Add("RateMonthly", lTsMon)
                    .Add("DepthMonthly", lTsMonDepth)
                    .Add("RateYearly", lTsYear)
                    .Add("DepthYearly", lTsYearDepth)
                End With
            End If
            If aStart < 0 AndAlso aEnd < 0 Then
                aStart = lTsGroupToReport(0).Dates.Value(0)
                aEnd = lTsGroupToReport(0).Dates.Value(lTsGroupToReport(0).numValues)
            End If
            If aDA < 0 Then
                aDA = lTsGroupToReport(0).Attributes.GetValue("Drainage Area")
            End If
        End If

        Return lTsGroupToReport
    End Function

    Public Function ConstructGraphTsGroup(ByVal aTs As atcTimeseries, ByVal aMethod As BFMethods, _
                                            Optional ByRef aStart As Double = 0.0, _
                                            Optional ByRef aEnd As Double = 0.0, _
                                            Optional ByRef aDA As Double = 0.0) As atcCollection
        'use a new ts group to hold the final ts for graph

        Dim lTsGroupToGraph = New atcCollection

        Dim lTsBFGroup As atcTimeseriesGroup = aTs.Attributes.GetDefinedValue("Baseflow").Value
        If lTsBFGroup Is Nothing OrElse lTsBFGroup.Count = 0 Then Return lTsGroupToGraph

        Dim lMatchBFTsGroup As New atcTimeseriesGroup
        For Each lTs As atcTimeseries In lTsBFGroup
            If lTs.Attributes.GetValue("Method") = aMethod Then
                lMatchBFTsGroup.Add(lTs)
            End If
        Next
        If lMatchBFTsGroup.Count > 0 Then
            If aMethod = BFMethods.PART Then
                Dim lTsBFToReportPartDaily1 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily1")(0)
                Dim lTsBFToReportPartDaily2 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily2")(0)
                Dim lTsBFToReportPartMonthly As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartMonthlyInterpolated")(0)

                Dim linearSlope As Double = lTsBFToReportPartMonthly.Attributes.GetValue("LinearSlope")
                Dim lTsBFToReportPartDaily As atcTimeseries = lTsBFToReportPartDaily1.Clone
                For I As Integer = 1 To lTsBFToReportPartDaily1.numValues
                    lTsBFToReportPartDaily.Value(I) = lTsBFToReportPartDaily1.Value(I) + linearSlope * (lTsBFToReportPartDaily2.Value(I) - lTsBFToReportPartDaily1.Value(I))
                Next
                With lTsGroupToGraph
                    .Add("RateDaily", lTsBFToReportPartDaily)
                End With
            Else
                Dim lTsDaily As atcTimeseries = lMatchBFTsGroup(0)
                With lTsGroupToGraph
                    .Add("RateDaily", lTsDaily)
                End With
            End If
            If aStart < 0 AndAlso aEnd < 0 Then
                aStart = lTsGroupToGraph(0).Dates.Value(0)
                aEnd = lTsGroupToGraph(0).Dates.Value(lTsGroupToGraph(0).numValues)
            End If
            If aDA < 0 Then
                aDA = lTsGroupToGraph(0).Attributes.GetValue("Drainage Area")
            End If
        End If

        Return lTsGroupToGraph
    End Function

    ''' <summary>
    ''' this is the .BSF WatStore format ASCII output
    ''' </summary>
    ''' <param name="aTs">Streamflow timeseries with baseflow group as an attribute</param>
    ''' <param name="aFilename">.BSF output filename</param>
    ''' <remarks></remarks>
    Public Sub ASCIIHySepBSF(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aBFMethod As BFMethods = BFMethods.HySEPFixed)

        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", "12345678")
        Dim lColumnId1 As String = "2" & lSTAID & "   60    3"
        Dim lColumnId2 As String = ("3" & lSTAID).PadRight(16, " ")

        Dim lTsBF As atcTimeseries = Nothing
        'If aBFName = "" Then aBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Method") = aBFMethod Then
                lTsBF = lTs
                Exit For
            End If
        Next

        If lTsBF Is Nothing Then
            Logger.Dbg("No baseflow data found.")
            Exit Sub
        End If

        Dim lOutputFile As String = aFilename
        Dim lSW As New IO.StreamWriter(lOutputFile, False)
        lSW.WriteLine(lColumnId1)

        Dim lDate(5) As Integer
        Dim lMonthQuarter As Integer = 1
        J2Date(lTsBF.Attributes.GetValue("SJDay"), lDate)
        Dim lStartYear As Integer = lDate(0)
        Dim lStartMonth As Integer = lDate(1)
        Dim lStartDay As Integer = lDate(2)
        'J2Date(lTsBF.Dates.Value(lTsBF.numValues - 1), lDate)
        J2Date(lTsBF.Attributes.GetValue("EJDay"), lDate)
        Dim lEndYear As Integer = lDate(0)
        Dim lEndMonth As Integer = lDate(1)
        Dim lEndDay As Integer = lDate(2)
        Dim lBFTsEndDate As Double = lTsBF.Dates.Value(lTsBF.numValues)

        Dim lStarting As Boolean = True
        Dim lEnded As Boolean = False

        For I As Integer = 0 To lTsBF.numValues - 1
            J2Date(lTsBF.Dates.Value(I), lDate)
            lMonthQuarter = 1
            'write one month at a time
            Dim lcurrentMonth As Integer = lDate(1)
            Dim lcurrentYear As Integer = lDate(0)
            Dim lfinalIndexInMonth As Integer = 0
            Dim lmonthQuarterStart As Integer = 0
            While lDate(2) <= DayMon(lcurrentYear, lcurrentMonth)

                If lDate(2) <= 8 Then
                    lMonthQuarter = 1
                    lmonthQuarterStart = lDate(2) - 1
                ElseIf lDate(2) <= 16 Then
                    lMonthQuarter = 2
                    lmonthQuarterStart = lDate(2) - 8 - 1
                ElseIf lDate(2) <= 24 Then
                    lMonthQuarter = 3
                    lmonthQuarterStart = lDate(2) - 16 - 1
                Else
                    lMonthQuarter = 4
                    lmonthQuarterStart = lDate(2) - 24 - 1
                End If

                lSW.Write(lColumnId2)
                lSW.Write(lDate(0).ToString) 'year
                lSW.Write(lDate(1).ToString.PadLeft(2, " ")) 'month
                lSW.Write(lMonthQuarter.ToString.PadLeft(2, " ")) 'month quarter

                For J As Integer = 0 To 7

                    If lStarting And lDate(2) < lStartDay Then
                        lSW.Write("       ")
                    ElseIf J < lmonthQuarterStart Then
                        lSW.Write("       ")
                    Else
                        'need to determine end of month
                        J2Date(lTsBF.Dates.Value(I + J - lmonthQuarterStart), lDate)

                        'check if reaching the end of the BF timeseries
                        If I + J - lmonthQuarterStart >= lTsBF.numValues Then
                            lEnded = True
                            lSW.WriteLine()
                            Exit While
                        End If
                        'check if within the current month
                        If lDate(2) <= DayMon(lDate(0), lcurrentMonth) Then
                            lSW.Write(String.Format("{0:#.00}", lTsBF.Value(I + J - lmonthQuarterStart + 1)).PadLeft(7, " "))
                            lStarting = False
                            If lDate(2) = DayMon(lDate(0), lcurrentMonth) Then
                                lSW.WriteLine()
                                lfinalIndexInMonth = I + J
                                Exit While
                            End If
                        Else
                            'don't think need to fill out the blanks with the blanks,
                            'so just quit the loop here
                            lSW.WriteLine()
                            lfinalIndexInMonth = I + J
                            Exit While
                        End If

                    End If

                Next
                lSW.WriteLine()
                I += 8 - lmonthQuarterStart
                J2Date(lTsBF.Dates.Value(I), lDate)
            End While

            'end it if already reached the end of the timeseries
            If lEnded Then
                Exit For
            Else
                I = lfinalIndexInMonth
            End If
            'move I forward to the end of the month

        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        ReDim lDate(0)
        lDate = Nothing
    End Sub

    ''' <summary>
    ''' this is the HySEP monthly ASCII output
    ''' </summary>
    ''' <param name="aTs">Streamflow timeseries with baseflow group as an attribute</param>
    ''' <param name="aFilename">.BSF output filename</param>
    ''' <remarks></remarks>
    Public Sub ASCIIHySepMonthly(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aBFMethod As BFMethods = BFMethods.HySEPFixed)
        Dim lSW As IO.StreamWriter = Nothing
        Try
            lSW = New IO.StreamWriter(aFilename, False)
        Catch ex As Exception
            Logger.Dbg("Problem opening file: " & aFilename)
            Exit Sub
        End Try

        Dim lTsBF As atcTimeseries = Nothing
        Dim lTsFlow As atcTimeseries = Nothing
        'If aBFName = "" Then aBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Method") = aBFMethod Then
                lTsBF = lTs
                lTsFlow = SubsetByDate(aTs, lTs.Dates.Value(0), lTs.Dates.Value(lTs.numValues), Nothing)
                Exit For
            End If
        Next

        If lTsBF Is Nothing Then
            Logger.Dbg("No baseflow data found.")
            Exit Sub
        End If

        Dim lEnglishUnit As Boolean = True
        Dim lDate(5) As Integer
        'English Unit: flow in cfs, depth in inch, drainage area in square miles
        'Metric Unit: flow in m3/s, depth in centimeter, drainage area in square km
        '1 second-foot for one day covers 1 square mile 0.03719 inch deep (Water Supply Paper by USGS)
        '1 cfs = 0.6462 M gal/d (flow rate conversion)
        Dim lDA As Double = lTsBF.Attributes.GetValue("Drainage Area", 1.0)
        Dim lTsFlowDepthPUA As atcTimeseries = Nothing
        Dim lTsFlowVolumePUA As atcTimeseries = Nothing
        Dim lTsFlowRatePUA As atcTimeseries = Nothing
        'Dim lTsFlowVolume As atcTimeseries = Nothing

        If lEnglishUnit Then
            lTsFlowDepthPUA = lTsFlow * (0.03719 / lDA) '-> inch
            lTsFlowVolumePUA = lTsFlow * (0.6462 / lDA) '-> Mgal/d/mi2
            'lTsFlowVolume = aTs * 0.6462
        Else
            lTsFlowDepthPUA = lTsFlow * (8.64 / lDA) '-> centimeter
        End If
        lTsFlowRatePUA = lTsFlow / lDA

        Dim lTsBFDepthPUA As atcTimeseries = Nothing
        Dim lTsBFVolumePUA As atcTimeseries = Nothing
        Dim lTsBFRatePUA As atcTimeseries = Nothing
        Dim lTsBFRateVolume As atcTimeseries = Nothing
        If lEnglishUnit Then
            lTsBFDepthPUA = lTsBF * (0.03719 / lDA) '-> inch
            lTsBFVolumePUA = lTsBF * (0.6462 / lDA) '-> Mgal/d/mi2
            lTsBFRateVolume = lTsBF * 0.6462
        Else
            lTsBFDepthPUA = lTsBF * (8.64 / lDA) '-> centimeter
        End If
        lTsBFRatePUA = lTsBF / lDA

        'Dim lTsBFPct As atcTimeseries = (lTsBF * 100.0) / aTs

        'aggregate into monthly values
        Dim lMonthlyCo1RateFlow As atcTimeseries = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo2RateBF As atcTimeseries = Aggregate(lTsBF, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo3RateRO As atcTimeseries = Aggregate(lTsFlow - lTsBF, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo4DepthFlowPUA As atcTimeseries = Aggregate(lTsFlowDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo5DepthBFPUA As atcTimeseries = Aggregate(lTsBFDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo6DepthROPUA As atcTimeseries = Aggregate(lTsFlowDepthPUA - lTsBFDepthPUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        'Dim lMonthlyVolumeFlowPUA As atcTimeseries = Aggregate(lTsFlowVolumePUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo8RateBFPUA As atcTimeseries = Aggregate(lTsBFRatePUA, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lMonthlyCo9RateBFVolumePUA As atcTimeseries = Aggregate(lTsBFVolumePUA, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lMonthlyCo10RateBFVolume As atcTimeseries = Aggregate(lTsBFRateVolume, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)

        Dim lSnMonth As New atcSeasonsMonth
        Dim lSnCo1RateFlow As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo1RateFlow, Nothing)
        Dim lSnCo2RateBF As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo2RateBF, Nothing)
        Dim lSnCo3RateRO As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo3RateRO, Nothing)
        Dim lSnCo4DepthFlowPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo4DepthFlowPUA, Nothing)
        Dim lSnCo5DepthBFPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo5DepthBFPUA, Nothing)
        Dim lSnCo6DepthROPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo6DepthROPUA, Nothing)
        Dim lSnCo8RateBFPUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo8RateBFPUA, Nothing)
        Dim lSnCo9RateBFVolumePUA As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo9RateBFVolumePUA, Nothing)
        Dim lSnCo10RateBFVolume As atcTimeseriesGroup = lSnMonth.Split(lMonthlyCo10RateBFVolume, Nothing)

        '2100
        Dim lHeader2100 As String = "" & vbCrLf & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow   (Mgal/d/" & vbCrLf & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & vbCrLf & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2110
        Dim lHeader2110 As String = "" & vbCrLf & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/" & vbCrLf & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   Base" & vbCrLf & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow     flow" & vbCrLf & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)   (Mgal/d)" & vbCrLf & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2120
        Dim lHeader2120 As String = "" & vbCrLf & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/" & vbCrLf & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & vbCrLf & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2130
        Dim lHeader2130 As String = "" & vbCrLf & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (m3/s/" & vbCrLf & _
     Space(6) & "  (m3/s)     (m3/s)     (m3/s)     (cm)    (cm)    (cm)    (%)      km2)" & vbCrLf & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        Dim lHeaderEngAll As String = "" & vbCrLf & _
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base       Base" & vbCrLf & _
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow       flow       Base" & vbCrLf & _
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/   (Mgal/d/     flow" & vbCrLf & _
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)       mi2)    (Mgal/d)" & vbCrLf & _
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ---------- ---------- ----------"

        '2150 FORMAT (A5,3F11.2,3F8.3,F7.2,F11.3)
        Dim lMonthNames() As String = {"Dummy", "Jan. ", "Feb. ", "Mar. ", "Apr. ", _
                                                "May  ", "June ", "July ", "Aug. ", _
                                                "Sept.", "Oct. ", "Nov. ", "Dec. "}

        'construct table
        Dim lCo0MonthName As String = ""
        Dim lCo1RateFlow As String = ""
        Dim lCo2RateBF As String = ""
        Dim lCo3RateRO As String = ""
        Dim lCo4DepthTotalflow As String = ""
        Dim lCo5DepthTotalBF As String = ""
        Dim lCo6DepthTotalRO As String = ""
        Dim lCo7BFPct As String = ""
        Dim lCo8RateBFPUA As String = ""
        Dim lCo9RateBFVolumePUA As String = ""
        Dim lCo10RateBFVolume As String = ""
        Dim lTable As New atcTableDelimited
        With lTable
            .Delimiter = ","
            .NumFields = 11
            .FieldLength(1) = 5
            .FieldName(1) = "Month"
            For I As Integer = 2 To 4
                .FieldLength(I) = 11
                Select Case I
                    Case 2 : .FieldName(I) = "FlowRate"
                    Case 3 : .FieldName(I) = "BaseflowRate"
                    Case 4 : .FieldName(I) = "RunoffRate"
                End Select
            Next
            For I As Integer = 5 To 7
                .FieldLength(I) = 8
                Select Case I
                    Case 5 : .FieldName(I) = "TotalFlowDepth"
                    Case 6 : .FieldName(I) = "TotalBaseflowDepth"
                    Case 7 : .FieldName(I) = "TotalRunoffDepth"
                End Select
            Next
            .FieldLength(8) = 7
            .FieldName(8) = "Baseflow%"

            For I As Integer = 9 To 11
                .FieldLength(I) = 11
                Select Case I
                    Case 9 : .FieldName(I) = "BaseflowRatePUA"
                    Case 10 : .FieldName(I) = "BaseflowRateVolumePUA"
                    Case 11 : .FieldName(I) = "BaseflowRateVolume"
                End Select
            Next

            'Start write out

            Dim lCo1Sum12MonthFlow As Double
            Dim lCo2Sum12MonthBF As Double
            Dim lCo3Sum12MonthRO As Double
            Dim lCo4Sum12MonthDepthFlow As Double
            Dim lCo5Sum12MonthDepthBF As Double
            Dim lCo6Sum12MonthDepthRO As Double
            Dim lCo8Sum12MonthRateBFPUA As Double
            Dim lCo9Sum12MonthRateBFVolumePUA As Double
            Dim lCo10Sum12MonthRateBFVolume As Double

            .CurrentRecord = 1
            For I As Integer = 0 To lMonthlyCo1RateFlow.numValues - 1

                If I + 1 Mod 12 = 0 Then
                    lCo1Sum12MonthFlow /= 12.0
                    lCo2Sum12MonthBF /= 12.0
                    lCo3Sum12MonthRO /= 12.0
                    Dim lBFPct As Double = lCo5Sum12MonthDepthBF / lCo4Sum12MonthDepthFlow * 100.0
                    lCo8Sum12MonthRateBFPUA /= 12.0
                    lCo9Sum12MonthRateBFVolumePUA /= 12.0
                    lCo10Sum12MonthRateBFVolume /= 12.0
                    J2Date(lMonthlyCo1RateFlow.Dates.Value(I), lDate)

                    .Value(1) = "E" & lDate(0)
                    .Value(2) = String.Format("{0:0.00}", lCo1Sum12MonthFlow)
                    .Value(3) = String.Format("{0:0.00}", lCo2Sum12MonthBF)
                    .Value(4) = String.Format("{0:0.00}", lCo3Sum12MonthRO)
                    .Value(5) = String.Format("{0:0.000}", lCo4Sum12MonthDepthFlow)
                    .Value(6) = String.Format("{0:0.000}", lCo5Sum12MonthDepthBF)
                    .Value(7) = String.Format("{0:0.000}", lCo6Sum12MonthDepthRO)
                    .Value(8) = String.Format("{0:0.00}", lBFPct)
                    .Value(9) = String.Format("{0:0.000}", lCo8Sum12MonthRateBFPUA)
                    .Value(10) = String.Format("{0:0.000}", lCo9Sum12MonthRateBFVolumePUA)
                    .Value(11) = String.Format("{0:0.000}", lCo10Sum12MonthRateBFVolume)

                    lCo1Sum12MonthFlow = 0.0
                    lCo2Sum12MonthBF = 0.0
                    lCo3Sum12MonthRO = 0.0
                    lCo4Sum12MonthDepthFlow = 0.0
                    lCo5Sum12MonthDepthBF = 0.0
                    lCo6Sum12MonthDepthRO = 0.0
                    lCo8Sum12MonthRateBFPUA = 0.0
                    lCo9Sum12MonthRateBFVolumePUA = 0.0
                    lCo10Sum12MonthRateBFVolume = 0.0

                    .CurrentRecord += 1
                End If

                J2Date(lMonthlyCo1RateFlow.Dates.Value(I), lDate)
                .Value(1) = lMonthNames(lDate(1))
                .Value(2) = String.Format("{0:0.00}", lMonthlyCo1RateFlow.Value(I + 1))
                .Value(3) = String.Format("{0:0.00}", lMonthlyCo2RateBF.Value(I + 1))
                .Value(4) = String.Format("{0:0.00}", lMonthlyCo3RateRO.Value(I + 1))
                .Value(5) = String.Format("{0:0.000}", lMonthlyCo4DepthFlowPUA.Value(I + 1))
                .Value(6) = String.Format("{0:0.000}", lMonthlyCo5DepthBFPUA.Value(I + 1))
                .Value(7) = String.Format("{0:0.000}", lMonthlyCo6DepthROPUA.Value(I + 1))
                .Value(8) = String.Format("{0:0.00}", CDbl(.Value(3)) / CDbl(.Value(2)) * 100.0)
                .Value(9) = String.Format("{0:0.000}", lMonthlyCo8RateBFPUA.Value(I + 1))
                .Value(10) = String.Format("{0:0.000}", lMonthlyCo9RateBFVolumePUA.Value(I + 1))
                .Value(11) = String.Format("{0:0.000}", lMonthlyCo10RateBFVolume.Value(I + 1))

                lCo1Sum12MonthFlow += lMonthlyCo1RateFlow.Value(I + 1)
                lCo2Sum12MonthBF += lMonthlyCo2RateBF.Value(I + 1)
                lCo3Sum12MonthRO += lMonthlyCo3RateRO.Value(I + 1)
                lCo4Sum12MonthDepthFlow += lMonthlyCo4DepthFlowPUA.Value(I + 1)
                lCo5Sum12MonthDepthBF += lMonthlyCo5DepthBFPUA.Value(I + 1)
                lCo6Sum12MonthDepthRO += lMonthlyCo6DepthROPUA.Value(I + 1)
                lCo8Sum12MonthRateBFPUA += lMonthlyCo8RateBFPUA.Value(I + 1)
                lCo9Sum12MonthRateBFVolumePUA += lMonthlyCo9RateBFVolumePUA.Value(I + 1)
                lCo10Sum12MonthRateBFVolume += lMonthlyCo10RateBFVolume.Value(I + 1)

                .CurrentRecord += 1
            Next
            'For I As Integer = 1 To 12
            '    .CurrentRecord = I
            '    .Value(1) = lMonthNames(I)
            '    .Value(2) = lSnCo1RateFlow(I - 1).Attributes.GetValue("Mean")
            '    .Value(3) = lSnCo2RateBF(I - 1).Attributes.GetValue("Mean")
            '    .Value(4) = lSnCo3RateRO(I - 1).Attributes.GetValue("Mean")
            '    .Value(5) = lSnCo4DepthFlowPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(6) = lSnCo5DepthBFPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(7) = lSnCo6DepthROPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(8) = CDbl(.Value(3)) / CDbl(.Value(2)) * 100.0
            '    .Value(9) = lSnCo8RateBFPUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(10) = lSnCo9RateBFVolumePUA(I - 1).Attributes.GetValue("Mean")
            '    .Value(11) = lSnCo10RateBFVolume(I - 1).Attributes.GetValue("Mean")
            'Next
        End With

        Dim lStation As String = aTs.Attributes.GetValue("STAID", "")
        If lStation = "" Then lStation = aTs.Attributes.GetValue("Location", "")
        Dim lUnitArea As String = "square miles"
        If Not lEnglishUnit Then lUnitArea = "square kilometers"

        J2Date(lTsBF.Dates.Value(0), lDate)
        Dim lStartYear As String = lDate(0)
        Dim lStartMonth As Integer = lDate(1)
        J2Date(lTsBF.Dates.Value(lTsBF.numValues - 1), lDate)
        Dim lEndYear As String = lDate(0)
        Dim lEndMonth As Integer = lDate(1)
        Dim lBFInterval As Double = lTsBF.Attributes.GetValue("BFInterval", 0.0)

        Dim lOneLine As String = lTsBF.Attributes.GetValue("Scenario", "")
        If lOneLine.Length > 0 Then lOneLine = lOneLine.Substring("HySep".Length)
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("Hydrograph separation by the " & lOneLine.PadRight(23, " "))
        lSW.WriteLine("Station ID = " & lStation & " Drainage Area = " & String.Format("{0:0.00}", lDA) & " " & lUnitArea)
        lSW.WriteLine("Period from " & lStartYear & " to " & lEndYear & "   interval = " & String.Format("{0:0.0}", lBFInterval) & " days")
        lSW.WriteLine(Space(80).Replace(" ", "-"))

        lSW.WriteLine(vbCrLf & vbCrLf)
        lSW.WriteLine(lHeaderEngAll)
        lTable.MoveFirst()
        While Not lTable.EOF
            For I As Integer = 1 To lTable.NumFields
                lSW.Write(lTable.Value(I).PadLeft(lTable.FieldLength(I)))
            Next
            lSW.WriteLine("")
            lTable.MoveNext()
        End While
        'lSW.WriteLine(lTable.ToString.Replace(",", "   "))

        'Seasonal-distribution table
        lSW.WriteLine(vbCrLf & vbCrLf)
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("                 Seasonal-distribution table " & vbCrLf)
        lSW.WriteLine("Hydrograph separation by the " & lOneLine.PadRight(23, " "))
        lSW.WriteLine("Station ID = " & lStation & " Drainage Area = " & String.Format("{0:0.00}", lDA) & " " & lUnitArea)
        lSW.WriteLine("Period from " & lStartYear & " to " & lEndYear & "   interval = " & String.Format("{0:0.0}", lBFInterval) & " days")
        lSW.WriteLine(Space(80).Replace(" ", "-"))
        lSW.WriteLine("                Year starts in " & lMonthNames(lStartMonth))
        lSW.WriteLine("                 Year ends in " & lMonthNames(lEndMonth))

        lSW.WriteLine("                        Base flow     Runoff")
        lSW.WriteLine("           Month           (in)         (in)")
        lSW.WriteLine("           ---------     ---------     ------")
        Dim lBFinch As String
        Dim lROinch As String
        'For I As Integer = lStartMonth To lEndMonth
        '    lBFinch = String.Format("{0:0.000}", lSnCo5DepthBFPUA(I - 1).Attributes.GetValue("Mean")).PadLeft(13, " ")
        '    lROinch = String.Format("{0:0.000}", lSnCo6DepthROPUA(I - 1).Attributes.GetValue("Mean")).PadLeft(13, " ")
        '    lSW.WriteLine(Space(11) & lMonthNames(I).Trim().PadRight(9, " ") & lBFinch & lROinch)
        'Next
        For I As Integer = 0 To lSnCo5DepthBFPUA.Count - 1
            lBFinch = String.Format("{0:0.000}", lSnCo5DepthBFPUA(I).Attributes.GetValue("Mean")).PadLeft(13, " ")
            lROinch = String.Format("{0:0.000}", lSnCo6DepthROPUA(I).Attributes.GetValue("Mean")).PadLeft(13, " ")
            J2Date(lSnCo5DepthBFPUA(I).Dates.Value(0), lDate)
            lSW.WriteLine(Space(11) & lMonthNames(lDate(1)).Trim().PadRight(9, " ") & lBFinch & lROinch)
        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTable.Clear()
        lTable = Nothing

        lTsFlowDepthPUA.Clear()
        lTsFlowVolumePUA.Clear()
        lTsFlowRatePUA.Clear()
        'lTsBFDepthPUA.Clear()
        'lTsBFVolumePUA.Clear()
        'lTsBFRatePUA.Clear()
        'lTsBFRateVolume.Clear()

        lMonthlyCo1RateFlow.Clear()
        lMonthlyCo2RateBF.Clear()
        lMonthlyCo3RateRO.Clear()
        lMonthlyCo4DepthFlowPUA.Clear()
        lMonthlyCo5DepthBFPUA.Clear()
        lMonthlyCo6DepthROPUA.Clear()

        lMonthlyCo8RateBFPUA.Clear()
        lMonthlyCo9RateBFVolumePUA.Clear()
        lMonthlyCo10RateBFVolume.Clear()

    End Sub

    Public Sub ASCIIHySepDelimited(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", "12345678")
        Dim lColumnId1 As String = "2" & lSTAID & "   60    3"
        Dim lColumnId2 As String = ("3" & lSTAID).PadRight(16, " ")

        Dim lTsBF As atcTimeseries = Nothing
        Dim lBFName As String = ""
        If lBFName = "" Then lBFName = "HySep"
        For Each lTs As atcTimeseries In aTs.Attributes.GetDefinedValue("Baseflow").Value
            If lTs.Attributes.GetValue("Scenario").ToString.StartsWith(lBFName) Then
                lTsBF = lTs
                Exit For
            End If
        Next

        If lTsBF Is Nothing Then
            Logger.Dbg("No baseflow data found.")
            Exit Sub
        End If

        Dim lOutputFile As String = aFilename
        Dim lSW As New IO.StreamWriter(lOutputFile, False)
        lSW.WriteLine(lColumnId1)
        Dim lDate(5) As Integer
        Dim lStarting As Boolean = True
        Dim lEnded As Boolean = False
        lSW.WriteLine("Baseflow at lSTAID")
        For I As Integer = 0 To lTsBF.numValues - 1
            J2Date(lTsBF.Dates.Value(I), lDate)
            lSW.WriteLine(lDate(0) & "/" & lDate(1) & "/" & lDate(2) & aDelim & String.Format("{0:#.00}", lTsBF.Value(I + 1)))
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub


    ''' <summary>
    ''' PART ASCII output, partday.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partday file name</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartDaily(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "PartDaily1"
                        lTsBaseflow1 = lTsBF
                    Case "PartDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "PartDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIPartDaily: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartDaily: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        lSW.WriteLine(" THIS IS FILE PARTDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM PART. ")
        lSW.WriteLine(" NOTE -- RESULTS AT THIS SMALL TIME SCALE ARE PROVIDED FOR ")
        lSW.WriteLine(" THE PURPOSES OF PROGRAM SCREENING AND FOR GRAPHICS, BUT ")
        lSW.WriteLine(" SHOULD NOT BE REPORTED OR USED QUANTITATIVELY ")
        lSW.WriteLine("  INPUT FILE = " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine("  STARTING YEAR =" & lStartingYear.PadLeft(6, " "))
        lSW.WriteLine("  ENDING YEAR =" & lEndingYear.PadLeft(8, " "))
        lSW.WriteLine("                          BASE FLOW FOR EACH")
        lSW.WriteLine("                             REQUIREMENT OF  ")
        lSW.WriteLine("           STREAM         ANTECEDENT RECESSION ")
        lSW.WriteLine("  DAY #     FLOW        #1         #2         #3          DATE ")
        Dim lDayCount As String
        Dim lStreamFlow As String
        Dim lBF1 As String
        Dim lBF2 As String
        Dim lBF3 As String
        Dim lDateStr As String

        For I As Integer = 0 To lTsFlow.numValues - 1
            lDayCount = (I + 1).ToString.PadLeft(5, " ")
            lStreamFlow = String.Format("{0:0.00}", lTsFlow.Value(I + 1)).PadLeft(11, " ")
            lBF1 = String.Format("{0:0.00}", lTsBaseflow1.Value(I + 1)).PadLeft(11, " ")
            lBF2 = String.Format("{0:0.00}", lTsBaseflow2.Value(I + 1)).PadLeft(11, " ")
            lBF3 = String.Format("{0:0.00}", lTsBaseflow3.Value(I + 1)).PadLeft(11, " ")
            J2Date(lTsFlow.Dates.Value(I), lDate)
            lDateStr = lDate(0).ToString.PadLeft(9, " ") & _
                       lDate(1).ToString.PadLeft(4, " ") & _
                       lDate(2).ToString.PadLeft(4, " ")
            lSW.WriteLine(lDayCount & lStreamFlow & lBF1 & lBF2 & lBF3 & lDateStr)
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    Public Sub ASCIIPartDailyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "PartDaily1"
                        lTsBaseflow1 = lTsBF
                    Case "PartDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "PartDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIPartDailyTabDelimited: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartDailyTabDelimited: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        lSW.WriteLine(" THIS IS FILE PARTDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM PART. ")
        lSW.WriteLine(" NOTE -- RESULTS AT THIS SMALL TIME SCALE ARE PROVIDED FOR ")
        lSW.WriteLine(" THE PURPOSES OF PROGRAM SCREENING AND FOR GRAPHICS, BUT ")
        lSW.WriteLine(" SHOULD NOT BE REPORTED OR USED QUANTITATIVELY ")
        lSW.WriteLine("  INPUT FILE = " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine("  STARTING YEAR =" & lStartingYear.PadLeft(6, " "))
        lSW.WriteLine("  ENDING YEAR =" & lEndingYear.PadLeft(8, " "))
        lSW.WriteLine("                          BASE FLOW FOR EACH")
        lSW.WriteLine("                             REQUIREMENT OF  ")
        lSW.WriteLine("           STREAM         ANTECEDENT RECESSION ")
        lSW.WriteLine("DAY #" & aDelim & "FLOW" & aDelim & "#1" & aDelim & "#2" & aDelim & "#3" & aDelim & "DATE")
        Dim lDayCount As String
        Dim lStreamFlow As String
        Dim lBF1 As String
        Dim lBF2 As String
        Dim lBF3 As String
        Dim lDateStr As String

        For I As Integer = 0 To lTsFlow.numValues - 1
            lDayCount = (I + 1).ToString & aDelim
            lStreamFlow = String.Format("{0:0.00}", lTsFlow.Value(I + 1)) & aDelim
            lBF1 = String.Format("{0:0.00}", lTsBaseflow1.Value(I + 1)) & aDelim
            lBF2 = String.Format("{0:0.00}", lTsBaseflow2.Value(I + 1)) & aDelim
            lBF3 = String.Format("{0:0.00}", lTsBaseflow3.Value(I + 1)) & aDelim
            J2Date(lTsFlow.Dates.Value(I), lDate)
            lDateStr = lDate(0) & "/" & lDate(1) & "/" & lDate(2)
            lSW.WriteLine(lDayCount & lStreamFlow & lBF1 & lBF2 & lBF3 & lDateStr)
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partmon.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partmon filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartMonthly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        'PrintDataSummary(aTS) 'repopulate the missing-month collection

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(lstart + JulianHour * 24, lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(lend - JulianHour * 24, lDate)
        Dim lEndingYear As String = lDate(0).ToString

        'Monthly stream flow in cfs, then, turn into inches
        Dim lTotXX As Double = 0.0 ' in inches
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            'For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
            '    If lTsMonthlyFlowDepth.Value(M) = 0 Then
            '        lTsMonthlyFlowDepth.Value(M) = -99.99
            '    End If
            'Next

            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                    lTotXX += lTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

        lSW.WriteLine("  ")
        lSW.WriteLine("  THIS IS FILE PARTMON.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("                        MONTHLY STREAMFLOW (INCHES):")
        lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")
        lSW.Flush()
        Dim lFieldWidth As Integer = 6

        'Create a full calendar year range that is inclusive of the original time span
        J2Date(lTsMonthlyFlowDepth.Dates.Value(0), lDate)
        Dim lNewStartDate As Double = Date2J(lDate(0), 1, 1, 0, 0, 0)
        J2Date(lTsMonthlyFlowDepth.Dates.Value(lTsMonthlyFlowDepth.numValues), lDate)
        timcnv(lDate)
        Dim lNewEndDate As Double = Date2J(lDate(0), 12, 31, 24, 0, 0)
        Dim lTsMonthlyFlowDepthExt As atcTimeseries = NewTimeseries(lNewStartDate, lNewEndDate, atcTimeUnit.TUMonth, 1, Nothing, GetNaN())
        Dim lTsGroupMrg As New atcTimeseriesGroup()
        lTsGroupMrg.Add(lTsMonthlyFlowDepth)
        lTsGroupMrg.Add(lTsMonthlyFlowDepthExt)
        lTsMonthlyFlowDepthExt = MergeTimeseries(lTsGroupMrg)
        Dim lTsYearly As atcTimeseries = Aggregate(lTsMonthlyFlowDepthExt, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lYearCount As Integer = 1
        Dim lYearHasMiss As Boolean = False
        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
            Dim lCurrentYear As Integer = lDate(0)
            lYearHasMiss = False
            For M As Integer = 1 To 12
                If lDate(1) = M Then
                    If lDate(0) = lCurrentYear Then
                        Dim lMonthlyValue As Double
                        If I > lTsMonthlyFlowDepth.numValues Then
                            lMonthlyValue = -99.99
                            TIMADD(lDate, atcTimeUnit.TUMonth, 1, 1, lDate)
                        Else
                            lMonthlyValue = lTsMonthlyFlowDepth.Value(I)
                            J2Date(lTsMonthlyFlowDepth.Dates.Value(I), lDate)
                        End If
                        If lMonthlyValue < -99.0 Then lYearHasMiss = True
                        lSW.Write(String.Format("{0:0.00}", lMonthlyValue).PadLeft(lFieldWidth, " "))
                        I += 1
                    Else
                        Exit For
                    End If

                Else
                    lSW.Write(Space(lFieldWidth))
                End If
            Next
            I -= 1

            'print yearly sum
            If lYearHasMiss OrElse lYearCount > lTsYearly.numValues OrElse Double.IsNaN(lTsYearly.Value(lYearCount)) Then
                lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
            Else
                lSW.WriteLine(String.Format("{0:0.00}", lTsYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
            End If
            lYearCount += 1
        Next
        lSW.WriteLine(" ")
        lSW.WriteLine("                 TOTAL OF MONTHLY AMOUNTS = " & DoubleToString(lTotXX))
        lSW.Flush()

        'print baseflow monthly values
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("                         MONTHLY BASE FLOW (INCHES):")
        lSW.WriteLine("          J     F     M     A     M     J     J     A     S     O     N     D   YEAR")

        'Create a full calendar year range that is inclusive of the original time span
        'J2Date(lTsMonthlyFlowDepth.Dates.Value(0), lDate)
        'Dim lNewStartDate As Double = Date2J(lDate(0), 1, 1, 0, 0, 0)
        'J2Date(lTsMonthlyFlowDepth.Dates.Value(lTsMonthlyFlowDepth.numValues), lDate)
        'timcnv(lDate)
        'Dim lNewEndDate As Double = Date2J(lDate(0), 12, 31, 24, 0, 0)
        Dim lTsBaseflowMonthlyDepthExt As atcTimeseries = NewTimeseries(lNewStartDate, lNewEndDate, atcTimeUnit.TUMonth, 1, Nothing, GetNaN())
        lTsGroupMrg.Clear()
        lTsGroupMrg.Add(lTsBaseflowMonthlyDepth)
        lTsGroupMrg.Add(lTsBaseflowMonthlyDepthExt)
        lTsBaseflowMonthlyDepthExt = MergeTimeseries(lTsGroupMrg)
        Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepthExt, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lYearCount = 1
        For I As Integer = 1 To lTsBaseflowMonthlyDepth.numValues
            J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0).ToString.PadLeft(lFieldWidth, " ")) 'begining of a year
            Dim lCurrentYear As Integer = lDate(0)
            lYearHasMiss = False
            For M As Integer = 1 To 12
                If lDate(1) = M Then
                    If lDate(0) = lCurrentYear Then
                        Dim lMonthlyValue As Double
                        If I > lTsMonthlyFlowDepth.numValues Then
                            lMonthlyValue = -99.99
                            TIMADD(lDate, atcTimeUnit.TUMonth, 1, 1, lDate)
                        Else
                            lMonthlyValue = lTsBaseflowMonthlyDepth.Value(I)
                            J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I), lDate)
                        End If
                        If lMonthlyValue < -99.0 Then lYearHasMiss = True
                        lSW.Write(String.Format("{0:0.00}", lMonthlyValue).PadLeft(lFieldWidth, " "))

                        I += 1
                    Else
                        Exit For
                    End If

                Else
                    lSW.Write(Space(lFieldWidth))
                End If

            Next

            I -= 1
            'print yearly sum
            If lYearHasMiss OrElse lYearCount > lTsBFYearly.numValues OrElse Double.IsNaN(lTsBFYearly.Value(lYearCount)) Then
                lSW.WriteLine(String.Format("{0:0.00}", -99.99).PadLeft(lFieldWidth, " "))
            Else
                lSW.WriteLine(String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount)).PadLeft(lFieldWidth, " "))
            End If
            lYearCount += 1
        Next

        lSW.WriteLine(" ")
        lSW.WriteLine("                  TOTAL OF MONTHLY AMOUNTS = " & DoubleToString(lTotalBaseflowDepth))
        lSW.WriteLine(" ")
        lSW.WriteLine(" RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION. ")
        lSW.WriteLine(" FILES PARTQRT.TXT AND PARTSUM.TXT GIVE RESULT AT THE")
        lSW.WriteLine(" CORRECT TIME SCALES (QUARTER YEAR, YEAR, OR MORE). ")

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTsYearly.Clear() : lTsYearly = Nothing
    End Sub

    Public Sub ASCIIPartMonthlyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartMonthly: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        'Monthly stream flow in cfs, then, turn into inches
        Dim lTotXX As Double = 0.0 ' in inches
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            'For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
            '    If lTsMonthlyFlowDepth.Value(M) = 0 Then
            '        lTsMonthlyFlowDepth.Value(M) = -99.99
            '    End If
            'Next

            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                'If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                '    lTsMonthlyFlowDepth.Value(M) = -99.99
                'Else
                lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                lTotXX += lTsMonthlyFlowDepth.Value(M)
                'End If
            Next
        End If

        lSW.WriteLine(" ")
        lSW.WriteLine("  THIS IS FILE PARTMON.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("  MONTHLY STREAMFLOW AND BASEFLOW (INCHES):")
        lSW.WriteLine("Date" & aDelim & "Flow" & aDelim & "Baseflow")
        lSW.Flush()

        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            lSW.Write(lDate(0) & "/" & lDate(1) & aDelim)
            lSW.Write(String.Format("{0:0.00}", lTsMonthlyFlowDepth.Value(I)) & aDelim)
            lSW.WriteLine(String.Format("{0:0.00}", lTsBaseflowMonthlyDepth.Value(I)))
        Next
        lSW.WriteLine(" ")
        lSW.WriteLine("     TOTAL OF MONTHLY Flow AMOUNTS = " & lTotXX)
        lSW.WriteLine("     TOTAL OF MONTHLY Baseflow AMOUNTS = " & String.Format("{0:0.0000000}", lTotalBaseflowDepth))
        lSW.WriteLine(" ")
        lSW.WriteLine(" RESULTS ON THE MONTHLY TIME SCALE SHOULD BE USED WITH CAUTION. ")
        lSW.WriteLine(" FILES PARTQRT.TXT AND PARTSUM.TXT GIVE RESULT AT THE")
        lSW.WriteLine(" CORRECT TIME SCALES (QUARTER YEAR, YEAR, OR MORE). ")
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partqrt.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partqrt filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartQuarterly(ByVal aTS As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)


        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer

        'Monthly stream flow in cfs
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        Dim lTotXX As Double = 0.0
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            'For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
            '    If lTsMonthlyFlowDepth.Value(M) = 0 Then
            '        lTsMonthlyFlowDepth.Value(M) = -99.99
            '    End If
            'Next
            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                    lTsMonthlyFlowDepth.Value(M) = -99.99
                Else
                    lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                    lTotXX += lTsMonthlyFlowDepth.Value(M)
                End If
            Next
        End If

        lSW.WriteLine("  ")
        lSW.WriteLine("  THIS IS FILE PARTQRT.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine("  ")
        lSW.WriteLine("        QUARTER-YEAR STREAMFLOW IN INCHES         ")
        lSW.WriteLine("        --------------------------------          ")
        lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")
        lSW.Flush()

        ' 1053 FORMAT (1I6, 5F8.2)
        Dim lFieldWidth1 As Integer = 6
        Dim lFieldWidthO As Integer = 8

        'Create a full calendar year range that is inclusive of the original time span
        J2Date(lTsMonthlyFlowDepth.Dates.Value(0), lDate)
        Dim lNewStartDate As Double = Date2J(lDate(0), 1, 1, 0, 0, 0)
        J2Date(lTsMonthlyFlowDepth.Dates.Value(lTsMonthlyFlowDepth.numValues), lDate)
        timcnv(lDate)
        Dim lNewEndDate As Double = Date2J(lDate(0), 12, 31, 24, 0, 0)
        Dim lTsMonthlyFlowDepthExt As atcTimeseries = NewTimeseries(lNewStartDate, lNewEndDate, atcTimeUnit.TUMonth, 1, Nothing, GetNaN())
        Dim lTsGroupMrg As New atcTimeseriesGroup()
        lTsGroupMrg.Add(lTsMonthlyFlowDepth)
        lTsGroupMrg.Add(lTsMonthlyFlowDepthExt)
        lTsMonthlyFlowDepthExt = MergeTimeseries(lTsGroupMrg)
        Dim lTsYearly As atcTimeseries = Aggregate(lTsMonthlyFlowDepthExt, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)

        Dim lYearCount As Integer = 1
        Dim lQuarter1 As Double = 0
        Dim lQuarter2 As Double = 0
        Dim lQuarter3 As Double = 0
        Dim lQuarter4 As Double = 0

        Dim lQuarter1Negative As Boolean = False
        Dim lQuarter2Negative As Boolean = False
        Dim lQuarter3Negative As Boolean = False
        Dim lQuarter4Negative As Boolean = False

        For I As Integer = 1 To lTsMonthlyFlowDepthExt.numValues
            J2Date(lTsMonthlyFlowDepthExt.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 4, 5, 6
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 7, 8, 9
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 10, 11, 12
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                    End Select

                    If I > lTsMonthlyFlowDepthExt.numValues Then
                        TIMADD(lDate, atcTimeUnit.TUMonth, 1, 1, lDate)
                    Else
                        J2Date(lTsMonthlyFlowDepthExt.Dates.Value(I), lDate)
                    End If

                    I += 1
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")

            Dim lYearlyValue As Double = -99.99
            If lYearCount <= lTsYearly.numValues Then
                If Not Double.IsNaN(lTsYearly.Value(lYearCount)) AndAlso _
                Not lQuarter1Negative AndAlso Not lQuarter2Negative AndAlso Not lQuarter3Negative AndAlso Not lQuarter4Negative Then
                    lYearlyValue = lTsYearly.Value(lYearCount)
                End If
            End If
            Dim lStrQYear As String = String.Format("{0:0.00}", lYearlyValue).PadLeft(lFieldWidthO, " ")
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        'print quarterly baseflow values
        lSW.WriteLine("  ")
        lSW.WriteLine("  ")
        lSW.WriteLine("        QUARTER-YEAR BASE FLOW IN INCHES          ")
        lSW.WriteLine("        --------------------------------          ")
        lSW.WriteLine("          JAN-    APR-    JULY-   OCT-    YEAR    ")
        lSW.WriteLine("          MAR     JUNE    SEPT    DEC     TOTAL   ")

        'Create a full calendar year range that is inclusive of the original time span
        'J2Date(lTsMonthlyFlowDepth.Dates.Value(0), lDate)
        'lNewStartDate = Date2J(lDate(0), 1, 1, 0, 0, 0)
        'J2Date(lTsMonthlyFlowDepth.Dates.Value(lTsMonthlyFlowDepth.numValues), lDate)
        'timcnv(lDate)
        'lNewEndDate = Date2J(lDate(0), 12, 31, 24, 0, 0)
        Dim lTsBaseflowMonthlyDepthExt As atcTimeseries = NewTimeseries(lNewStartDate, lNewEndDate, atcTimeUnit.TUMonth, 1, Nothing, GetNaN())
        lTsGroupMrg.Clear()
        lTsGroupMrg.Add(lTsBaseflowMonthlyDepth)
        lTsGroupMrg.Add(lTsBaseflowMonthlyDepthExt)
        lTsBaseflowMonthlyDepthExt = MergeTimeseries(lTsGroupMrg)
        Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepthExt, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)

        lYearCount = 1
        For I As Integer = 1 To lTsBaseflowMonthlyDepthExt.numValues
            J2Date(lTsBaseflowMonthlyDepthExt.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 4, 5, 6
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 7, 8, 9
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 10, 11, 12
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse _
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse _
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                    End Select

                    If I > lTsMonthlyFlowDepthExt.numValues Then
                        TIMADD(lDate, atcTimeUnit.TUMonth, 1, 1, lDate)
                    Else
                        J2Date(lTsMonthlyFlowDepthExt.Dates.Value(I), lDate)
                    End If

                    I += 1
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString.PadLeft(lFieldWidth1, " ")
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1).PadLeft(lFieldWidthO, " ")
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2).PadLeft(lFieldWidthO, " ")
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3).PadLeft(lFieldWidthO, " ")
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4).PadLeft(lFieldWidthO, " ")

            Dim lYearlyValue As Double = -99.99
            If lYearCount <= lTsBFYearly.numValues Then
                If Not Double.IsNaN(lTsBFYearly.Value(lYearCount)) AndAlso _
                Not lQuarter1Negative AndAlso Not lQuarter2Negative AndAlso Not lQuarter3Negative AndAlso Not lQuarter4Negative Then
                    lYearlyValue = lTsBFYearly.Value(lYearCount)
                End If
            End If

            Dim lStrQYear As String = String.Format("{0:0.00}", lYearlyValue).PadLeft(lFieldWidthO, " ")
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTsYearly.Clear() : lTsYearly = Nothing

    End Sub

    Public Sub ASCIIPartQuarterlyDelimited(ByVal aTS As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lTsBaseflowDaily As atcTimeseries = Nothing
        Dim lTotalBaseflowDepth As Double = 0.0
        Dim lMissingDataMonth As atcCollection = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                    lTotalBaseflowDepth = lTsBF.Attributes.GetValue("SumDepth")
                    lMissingDataMonth = lTsBF.Attributes.GetValue("MissingMonths")
                    If lTsBaseflowDaily IsNot Nothing Then Exit For
                ElseIf lTsBF.Attributes.GetValue("Scenario") = "PartDaily1" Then
                    lTsBaseflowDaily = lTsBF
                    If lTsBaseflowMonthlyDepth IsNot Nothing Then Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartQuarterly: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflowDaily.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflowDaily.Attributes.GetValue("EJday")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)


        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer

        'Monthly stream flow in cfs
        Dim lTsMonthlyFlowDepth As atcTimeseries = Nothing
        Dim lTotXX As Double = 0.0
        If lTsMonthlyFlowDepth Is Nothing Then
            lTsMonthlyFlowDepth = Aggregate(lTsFlow, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame, Nothing)
            'For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
            '    If lTsMonthlyFlowDepth.Value(M) = 0 Then
            '        lTsMonthlyFlowDepth.Value(M) = -99.99
            '    End If
            'Next
            'Monthly stream flow in inches and flag as -99.99 if month is incomplete
            'Also determine total of monthly amounts
            For M As Integer = 1 To lTsMonthlyFlowDepth.numValues
                J2Date(lTsMonthlyFlowDepth.Dates.Value(M - 1), lDate)
                'If lMissingDataMonth.Keys.Contains(lDate(0).ToString & "_" & lDate(1).ToString.PadLeft(2, "0")) Then
                '    lTsMonthlyFlowDepth.Value(M) = -99.99
                'Else
                lTsMonthlyFlowDepth.Value(M) *= DayMon(lDate(0), lDate(1)) / (26.888889 * lDrainageArea)
                lTotXX += lTsMonthlyFlowDepth.Value(M)
                'End If
            Next
        End If

        lSW.WriteLine(" ")
        lSW.WriteLine("  THIS IS FILE PARTQRT.TXT FOR INPUT FILE: " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
        lSW.WriteLine(" ")
        lSW.WriteLine("  PROGRAM VERSION DATE = JANUARY 2007  ")
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("    " & aDelim & "QUARTER-YEAR STREAMFLOW IN INCHES         ")
        lSW.WriteLine("    " & aDelim & "---------------------------------          ")
        lSW.WriteLine("    " & aDelim & "JAN-" & aDelim & "APR-" & aDelim & "JUL-" & aDelim & "OCT-" & aDelim & "YEAR")
        lSW.WriteLine("Year" & aDelim & "MAR " & aDelim & "JUN " & aDelim & "SEP " & aDelim & "DEC " & aDelim & "TOTAL")
        lSW.Flush()

        ' 1053 FORMAT (1I6, 5F8.2)
        Dim lFieldWidth1 As Integer = 6
        Dim lFieldWidthO As Integer = 8
        Dim lTsYearly As atcTimeseries = Aggregate(lTsMonthlyFlowDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lYearCount As Integer = 1
        Dim lQuarter1 As Double = 0
        Dim lQuarter2 As Double = 0
        Dim lQuarter3 As Double = 0
        Dim lQuarter4 As Double = 0

        Dim lQuarter1Negative As Boolean = False
        Dim lQuarter2Negative As Boolean = False
        Dim lQuarter3Negative As Boolean = False
        Dim lQuarter4Negative As Boolean = False

        For I As Integer = 1 To lTsMonthlyFlowDepth.numValues
            J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 4, 5, 6
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 7, 8, 9
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsMonthlyFlowDepth.Value(I)
                            End If
                        Case 10, 11, 12
                            If lTsMonthlyFlowDepth.Value(I) < -99.0 Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsMonthlyFlowDepth.Value(I)
                            End If
                    End Select
                    I += 1
                    J2Date(lTsMonthlyFlowDepth.Dates.Value(I - 1), lDate)
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString & aDelim
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1) & aDelim
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2) & aDelim
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3) & aDelim
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4) & aDelim
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsYearly.Value(lYearCount))
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        'print quarterly baseflow values
        lSW.WriteLine(" ")
        lSW.WriteLine(" ")
        lSW.WriteLine("    " & aDelim & "QUARTER-YEAR BASE FLOW IN INCHES          ")
        lSW.WriteLine("    " & aDelim & "--------------------------------          ")
        lSW.WriteLine("    " & aDelim & " JAN-" & aDelim & "APR-" & aDelim & "JUL-" & aDelim & "OCT-" & aDelim & "YEAR")
        lSW.WriteLine("Year" & aDelim & " MAR " & aDelim & "JUN " & aDelim & "SEP " & aDelim & "DEC " & aDelim & "TOTAL")

        Dim lTsBFYearly As atcTimeseries = Aggregate(lTsBaseflowMonthlyDepth, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lYearCount = 1
        For I As Integer = 1 To lTsBaseflowMonthlyDepth.numValues
            J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
            Dim lCurrentYear As Integer = lDate(0)

            lQuarter1 = 0
            lQuarter2 = 0
            lQuarter3 = 0
            lQuarter4 = 0

            lQuarter1Negative = False
            lQuarter2Negative = False
            lQuarter3Negative = False
            lQuarter4Negative = False

            For M As Integer = 1 To 12
                If lDate(1) = M And lDate(0) = lCurrentYear Then 'within a year
                    Select Case M
                        Case 1, 2, 3
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 4, 5, 6
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 7, 8, 9
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                        Case 10, 11, 12
                            If lTsBaseflowMonthlyDepth.Value(I) < -99.0 Then
                                lQuarter4Negative = True
                            Else
                                lQuarter4 += lTsBaseflowMonthlyDepth.Value(I)
                            End If
                    End Select
                    I += 1
                    J2Date(lTsBaseflowMonthlyDepth.Dates.Value(I - 1), lDate)
                End If
            Next ' month

            I -= 1

            If lQuarter1Negative Then lQuarter1 = -99.99
            If lQuarter2Negative Then lQuarter2 = -99.99
            If lQuarter3Negative Then lQuarter3 = -99.99
            If lQuarter4Negative Then lQuarter4 = -99.99

            Dim lStrYear As String = lCurrentYear.ToString & aDelim
            Dim lStrQ1 As String = String.Format("{0:0.00}", lQuarter1) & aDelim
            Dim lStrQ2 As String = String.Format("{0:0.00}", lQuarter2) & aDelim
            Dim lStrQ3 As String = String.Format("{0:0.00}", lQuarter3) & aDelim
            Dim lStrQ4 As String = String.Format("{0:0.00}", lQuarter4) & aDelim
            Dim lStrQYear As String = String.Format("{0:0.00}", lTsBFYearly.Value(lYearCount))
            lSW.WriteLine(lStrYear & lStrQ1 & lStrQ2 & lStrQ3 & lStrQ4 & lStrQYear)

            lYearCount += 1
        Next 'monthly streamflow in inches

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lTsYearly.Clear() : lTsYearly = Nothing

    End Sub

    ''' <summary>
    ''' PART ASCII output, partsum.txt
    ''' </summary>
    ''' <param name="aTsSF">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partsum filename</param>
    ''' <remarks>This summary file is supposed to be appended</remarks>
    Public Sub ASCIIPartBFSum(ByVal aTsSF As atcTimeseries, ByVal aFilename As String)

        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing
        Dim lDrainageArea As Double = 0.0
        Dim lTBase As Double = 0.0
        Dim lBFDatagroup As atcTimeseriesGroup = aTsSF.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "PartDaily1"
                        lTsBaseflow1 = lTsBF
                        lDrainageArea = lTsBF.Attributes.GetValue("Drainage Area")
                        lTBase = lTsBF.Attributes.GetValue("TBase")
                    Case "PartDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "PartDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIPartBFSum: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIPartBFSum: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTsSF, lstart, lend, Nothing)

        Dim lWriteHeader As Boolean = False
        If Not IO.File.Exists(aFilename) Then
            lWriteHeader = True
        End If

        Dim lSW As New IO.StreamWriter(aFilename, True)
        Dim lDate(5) As Integer

        If lWriteHeader Then
            lSW.WriteLine("File ""partsum.txt""                    Program version -- Jan 2007")
            lSW.WriteLine("-------------------------------------------------------------------")
            lSW.WriteLine("Each time the PART program is run, a new line is written to the end")
            lSW.WriteLine("of this file.")
            lSW.WriteLine(" ")
            lSW.WriteLine("            Drainage                                           Base-")
            lSW.WriteLine("              area                  Mean           Mean        flow")
            lSW.WriteLine("File name     (Sq.   Time         streamflow      baseflow     index")
            lSW.WriteLine("             miles)  period     (cfs)  (in/yr)  (cfs)  (in/yr)  (%)")
            lSW.WriteLine("--------------------------------------------------------------------")
        End If

        Dim lFieldWidthFilename As Integer = 11
        Dim lDataFilename As String = IO.Path.GetFileName(aTsSF.Attributes.GetValue("History 1"))
        If lDataFilename.Length > 10 Then lDataFilename = lDataFilename.Substring(0, lFieldWidthFilename - 1)
        lDataFilename = lDataFilename.PadRight(lFieldWidthFilename, " ")

        Dim lPadWidth As Integer = 19 - lFieldWidthFilename
        Dim lDrainageAreaStr As String = String.Format("{0:0.00}", lDrainageArea)
        lDrainageAreaStr = lDrainageAreaStr.PadLeft(lPadWidth, " ")
        Dim lSFMean As Double = lTsFlow.Attributes.GetValue("Mean")
        Dim lBFMean1 As Double = lTsBaseflow1.Attributes.GetValue("Mean")
        Dim lBFMean2 As Double = lTsBaseflow2.Attributes.GetValue("Mean")
        Dim lBFMean3 As Double = lTsBaseflow3.Attributes.GetValue("Mean")
        Dim lMsg As String = ""
        If lBFMean1 <> lBFMean2 Then
            lMsg &= "STREAMFLOW VARIES BETWEEN DIFFERENT " & vbCrLf
            lMsg &= "VALUES OF THE REQMT ANT. RECESSION !!!"
        End If
        Dim lBFMeanArithmetic As Double = (lBFMean1 + lBFMean2 + lBFMean3) / 3.0

        Dim lA As Double = (lBFMean1 - lBFMean2 - lBFMean2 + lBFMean3) / 2.0
        Dim lB As Double = lBFMean2 - lBFMean1 - 3.0 * lA
        Dim lC As Double = lBFMean1 - lA - lB
        Dim lX As Double = lDrainageArea ^ 0.2 - lTBase + 1
        Dim lBFInterpolatedCFS As Double = lA * lX ^ 2.0 + lB * lX + lC 'interpolated mean base flow (cfs)
        Dim lBFInterpolatedInch As Double = lBFInterpolatedCFS * 13.5837 / lDrainageArea 'interpolated mean base flow (IN/YR)

        '   LINEAR INTERPOLATION BETWEEN RESULTS FOR THE FIRST AND SECOND VALUES
        '   OF THE REQUIREMENT OF ANTECEDENT RECESSION.....
        'Dim lBFLine As Double = lBFMean1 + (lX - 1) * (lBFMean2 - lBFMean1)
        J2Date(lstart + JulianHour * 24, lDate)
        Dim lYearStart As Integer = lDate(0)
        J2Date(lend - JulianHour * 24, lDate)
        Dim lYearEnd As Integer = lDate(0)
        Dim lDurationString As String = lYearStart.ToString & "-" & lYearEnd.ToString
        lDurationString = lDurationString.PadLeft(11, " ")
        If lTsFlow.Attributes.GetValue("Count Missing") > 1 Then
            lMsg = " ******** record incomplete ********"
        Else
            lMsg = ""
        End If
        Dim lSFMeanCfs As String = String.Format("{0:0.00}", lSFMean).PadLeft(8, " ")
        Dim lSFMeanInch As String = String.Format("{0:0.00}", lSFMean * 13.5837 / lDrainageArea).PadLeft(8, " ")

        Dim lBFMeanCfs As String = String.Format("{0:0.00}", lBFInterpolatedCFS).PadLeft(8, " ")
        Dim lBFMeanInch As String = String.Format("{0:0.00}", lBFInterpolatedInch).PadLeft(8, " ")

        Dim lBFIndex As String = String.Format("{0:0.00}", 100 * lBFInterpolatedCFS / lSFMean).PadLeft(8, " ")

        lSW.Write(lDataFilename & lDrainageAreaStr & lDurationString)
        If lMsg.Length = 0 Then
            lSW.WriteLine(lSFMeanCfs & lSFMeanInch & lBFMeanCfs & lBFMeanInch & lBFIndex)
        Else
            lSW.WriteLine(lMsg)
        End If

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    ''' <summary>
    ''' PART ASCII output, partWY.txt
    ''' </summary>
    ''' <param name="aTS">Daily streamflow Tser with baseflow group</param>
    ''' <param name="aFilename">partWY filename</param>
    ''' <remarks>This summary file is supposed to be overwritten</remarks>
    Public Sub ASCIIPartWaterYear(ByVal aTs As atcTimeseries, ByVal aFilename As String)
        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTs.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        Dim lSW As New IO.StreamWriter(aFilename, False)
        Dim lWaterYear As New atcSeasonsWaterYear
        Dim lWaterYearCollection As atcTimeseriesGroup = lWaterYear.Split(lTsBaseflowMonthlyDepth, Nothing)

        'write file header
        lSW.WriteLine(" Results on the basis of the ")
        lSW.WriteLine(" water year (Oct 1 to Sept 30) ")
        lSW.WriteLine("  ")
        lSW.WriteLine("         Year              Total ")
        lSW.WriteLine(" --------------------      ----- ")

        'write results
        Dim lDate(5) As Integer
        For Each lTsWaterYear As atcTimeseries In lWaterYearCollection
            If lTsWaterYear.Attributes.GetValue("Count") = 12 Then
                'a full water year, then write out
                J2Date(lTsWaterYear.Dates.Value(0), lDate)
                lSW.Write("Oct " & lDate(0))
                J2Date(lTsWaterYear.Dates.Value(lTsWaterYear.numValues), lDate)
                lSW.Write(" to Sept " & lDate(0))
                lSW.WriteLine(String.Format("{0:0.00}", lTsWaterYear.Attributes.GetValue("Sum")).PadLeft(11, " "))
            Else
                'not a full water year, ignore
            End If
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lWaterYearCollection.Clear() : lWaterYearCollection = Nothing
        lWaterYear = Nothing
    End Sub

    Public Sub ASCIIPartWaterYearDelim(ByVal aTs As atcTimeseries, ByVal aFilename As String, Optional ByVal aDelim As String = vbTab)
        Dim lTsBaseflowMonthlyDepth As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTs.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If lTsBF.Attributes.GetValue("Scenario") = "PartMonthlyDepth" Then
                    lTsBaseflowMonthlyDepth = lTsBF
                    Exit For
                End If
            Next
        Else
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflowMonthlyDepth Is Nothing Then
            Logger.Dbg("ASCIIPartWaterYear: no baseflow data found.")
            Exit Sub
        End If

        Dim lSW As New IO.StreamWriter(aFilename, False)
        Dim lWaterYear As New atcSeasonsWaterYear
        Dim lWaterYearCollection As atcTimeseriesGroup = lWaterYear.Split(lTsBaseflowMonthlyDepth, Nothing)

        'write file header
        lSW.WriteLine(" Results on the basis of the ")
        lSW.WriteLine(" water year (Oct 1 to Sept 30) ")
        lSW.WriteLine(" ")
        lSW.WriteLine("  Year " & aDelim & " Total ")
        lSW.WriteLine(" ------" & aDelim & "----- ")

        'write results
        Dim lDate(5) As Integer
        For Each lTsWaterYear As atcTimeseries In lWaterYearCollection
            If lTsWaterYear.Attributes.GetValue("Count") = 12 Then
                'a full water year, then write out
                J2Date(lTsWaterYear.Dates.Value(0), lDate)
                lSW.Write("Oct " & lDate(0))
                J2Date(lTsWaterYear.Dates.Value(lTsWaterYear.numValues), lDate)
                lSW.Write(" to Sept " & lDate(0) & aDelim)
                lSW.WriteLine(String.Format("{0:0.00}", lTsWaterYear.Attributes.GetValue("Sum")))
            Else
                'not a full water year, ignore
            End If
        Next
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        lWaterYearCollection.Clear() : lWaterYearCollection = Nothing
        lWaterYear = Nothing
    End Sub

    Public Sub ASCIIBFIDaily(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethodName As String)

        Dim lTsBaseflow As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case aMethodName
                        lTsBaseflow = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFIDaily: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow Is Nothing Then
            Logger.Dbg("ASCIIBFIDaily: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        Dim lblMethodStd As String = "   "
        Dim lblMethodMod As String = "   "
        Dim lblMethodParm As String = ""
        Dim lBFINDay As Integer = lTsBaseflow.Attributes.GetValue("BFINDay")
        Dim lStationID As String = lTsBaseflow.Attributes.GetValue("Location")
        If aMethodName.StartsWith("BFIStandard") Then
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethodName.StartsWith("BFIModified") Then
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   K      =" & String.Format("{0:.000000}", lBFIK1Day).PadLeft(10, " ")
        End If

        lSW.WriteLine(" Program Version = BFI 4.15 (.Net)")
        lSW.WriteLine("")
        lSW.WriteLine(" AVAILABLE SEPARATION METHODS:")
        lSW.WriteLine(lblMethodStd & "1 = STANDARD Institute of Hydrology method")
        lSW.WriteLine("       (N-day avg. recession test; uses ""N"" and ""f"")")
        lSW.WriteLine(lblMethodMod & "2 = MODIFIED method")
        lSW.WriteLine("       (1-day recession constant adjusted for number of days")
        lSW.WriteLine("        between points; uses ""N"" and ""K"")")
        lSW.WriteLine("")
        lSW.WriteLine(" BASE-FLOW SEPARATION PARAMETERS")
        lSW.WriteLine(lblMethodParm)
        lSW.WriteLine("")
        lSW.WriteLine("=================================================")
        lSW.WriteLine("")
        lSW.WriteLine("Gage " & lStationID)
        lSW.WriteLine("")
        lSW.WriteLine("<-- Calendar -->  Base Flow  Total Flow")
        lSW.WriteLine("Year  Month  Day    (cfs)       (cfs)  ")
        lSW.WriteLine("---------------------------------------")
        Dim lStrYear As String
        Dim lStrMonth As String
        Dim lStrDay As String
        Dim lStrBFFlow As String
        Dim lStrFlow As String
        Dim lTpStartIndex As Integer = lTsBaseflow.Attributes.GetValue("TPStart", 1)
        If lTsFlow.numValues < lTsBaseflow.numValues Then
            Dim lTsFlowNew As atcTimeseries = NewTimeseries(lTsBaseflow.Dates.Value(0), lTsBaseflow.Dates.Value(lTsBaseflow.numValues), atcTimeUnit.TUDay, 1, Nothing, -99.0)
            Dim lTsGroup As New atcTimeseriesGroup
            lTsGroup.Add(lTsFlow)
            lTsGroup.Add(lTsFlowNew)
            lTsFlow = MergeTimeseries(lTsGroup)
        End If
        For Z As Integer = lTpStartIndex To lTsBaseflow.numValues - 1
            J2Date(lTsBaseflow.Dates.Value(Z - 1), lDate)
            lStrYear = lDate(0).ToString
            lStrMonth = lDate(1).ToString.PadLeft(6, " ")
            lStrDay = lDate(2).ToString.PadLeft(6, " ")
            lStrBFFlow = String.Format("{0:0.00}", lTsBaseflow.Value(Z)).PadLeft(10, " ")
            lStrFlow = String.Format("{0:0.00}", lTsFlow.Value(Z)).PadLeft(12, " ")
            lSW.WriteLine(lStrYear & lStrMonth & lStrDay & lStrBFFlow & lStrFlow)
        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    Public Sub ASCIIBFI(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethodName As String)
        Dim lTsBaseflow As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case aMethodName
                        lTsBaseflow = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFI: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow Is Nothing Then
            Logger.Dbg("ASCIIBFI: no baseflow data found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)
        Dim lYearBasis As String = lTsBaseflow.Attributes.GetValue("YearBasis", "Calendar")

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        Dim lblMethodStd As String = "   "
        Dim lblMethodMod As String = "   "
        Dim lblMethodParm As String = ""
        Dim lBFINDay As Integer = lTsBaseflow.Attributes.GetValue("BFINDay")
        Dim lStationID As String = aTS.Attributes.GetValue("Location")
        Dim lFilenameOnly As String = IO.Path.GetFileNameWithoutExtension(aFilename)
        If aMethodName.StartsWith("BFIStandard") Then
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethodName.StartsWith("BFIModified") Then
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   K      =" & String.Format("{0:.000000}", lBFIK1Day).PadLeft(10, " ")
        End If

        'Writing annual results
        lSW.WriteLine("                                 Input file = " & aTS.Attributes.GetValue("History 1"))
        lSW.WriteLine("                                File format = Web/rdb (NWIS-W)")
        lSW.WriteLine("                      Base-flow output file = " & lFilenameOnly & ".bfi")
        lSW.WriteLine("                  Turning point output file = " & lFilenameOnly & ".tp")
        lSW.WriteLine(" Daily base flow and total flow output file = " & lFilenameOnly & ".q")
        lSW.WriteLine("")
        lSW.WriteLine(" Program Version = BFI 4.15 (.Net)")
        lSW.WriteLine("")
        lSW.WriteLine(" AVAILABLE SEPARATION METHODS:")
        lSW.WriteLine(lblMethodStd & "1 = STANDARD Institute of Hydrology method")
        lSW.WriteLine("       (N-day avg. recession test; uses ""N"" and ""f"")")
        lSW.WriteLine(lblMethodMod & "2 = MODIFIED method")
        lSW.WriteLine("       (1-day recession constant adjusted for number of days")
        lSW.WriteLine("        between points; uses ""N"" and ""K"")")
        lSW.WriteLine("")
        lSW.WriteLine(" BASE-FLOW SEPARATION PARAMETERS")
        lSW.WriteLine(lblMethodParm)
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine("")
        lSW.WriteLine(" =============================================================================")
        lSW.WriteLine(" Base-Flow Index for gage " & lStationID)
        lSW.WriteLine(" agency " & lStationID & " sample data					                                              ")
        lSW.WriteLine(" " & lYearBasis & "   Base-Flow      Base Flow     Total Runoff | Day of Turning Point |")
        lSW.WriteLine(" Year         Index        (acre-ft)       (acre-ft)  |  [First]     [Last]  |")
        lSW.WriteLine(" -----------------------------------------------------------------------------")
        lSW.WriteLine(lTsBaseflow.Attributes.GetValue("BFIAnnualSummary"))

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub

    Public Sub ASCIIBFITp(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethodName As String)
        Dim lTsBaseflow As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case aMethodName
                        lTsBaseflow = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFITp: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow Is Nothing Then
            Logger.Dbg("ASCIIBFITp: no baseflow data found.")
            Exit Sub
        End If

        Dim lTsQMinsTp As atcTimeseries = lTsBaseflow.Attributes.GetValue("TsQMINSTp")
        If lTsQMinsTp Is Nothing Then
            Logger.Dbg("ASCIIBFITp: no QMINS timeseries found.")
            Exit Sub
        End If

        Dim lstart As Double = lTsBaseflow.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(aTS, lstart, lend, Nothing)

        Dim lSW As New IO.StreamWriter(aFilename, False)

        Dim lDate(5) As Integer
        J2Date(aTS.Dates.Value(0), lDate)
        Dim lStartingYear As String = lDate(0).ToString
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        Dim lEndingYear As String = lDate(0).ToString

        Dim lblMethodStd As String = "   "
        Dim lblMethodMod As String = "   "
        Dim lblMethodParm As String = ""
        Dim lBFINDay As Integer = lTsBaseflow.Attributes.GetValue("BFINDay")
        Dim lStationID As String = lTsBaseflow.Attributes.GetValue("Location")
        Dim lFilenameOnly As String = IO.Path.GetFileNameWithoutExtension(aFilename)
        If aMethodName.StartsWith("BFIStandard") Then
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethodName.StartsWith("BFIModified") Then
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf & _
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf & _
                            "   K      =" & String.Format("{0:.000000}", lBFIK1Day).PadLeft(10, " ")
        End If

        lSW.WriteLine(" Program Version = BFI 4.15 (.Net)")
        lSW.WriteLine("")
        lSW.WriteLine(" AVAILABLE SEPARATION METHODS:")
        lSW.WriteLine(lblMethodStd & "1 = STANDARD Institute of Hydrology method")
        lSW.WriteLine("       (N-day avg. recession test; uses ""N"" and ""f"")")
        lSW.WriteLine(lblMethodMod & "2 = MODIFIED method")
        lSW.WriteLine("       (1-day recession constant adjusted for number of days")
        lSW.WriteLine("        between points; uses ""N"" and ""K"")")
        lSW.WriteLine("")
        lSW.WriteLine(" BASE-FLOW SEPARATION PARAMETERS")
        lSW.WriteLine(lblMethodParm)
        lSW.WriteLine("")
        lSW.WriteLine("===============================================")
        lSW.WriteLine("")
        lSW.WriteLine("Gage " & lStationID)
        'lSW.WriteLine("(^ indicates interpolated turning point)")
        lSW.WriteLine("")
        lSW.WriteLine("<-- Calendar -->  Base Flow")
        lSW.WriteLine("Year  Month  Day    (cfs)  ")
        lSW.WriteLine("---------------------------")

        Dim lStrYear, lStrMonth, lStrDay, lStrBF As String
        Dim lBFVal As Double
        For I As Integer = 1 To lTsQMinsTp.numValues
            If lTsQMinsTp.Value(I) >= 0 Then
                J2Date(lTsQMinsTp.Dates.Value(I - 1), lDate)
                lStrYear = lDate(0).ToString()
                lStrMonth = lDate(1).ToString.PadLeft(6, " ")
                lStrDay = lDate(2).ToString.PadLeft(6, " ")
                lBFVal = lTsBaseflow.Value(I)
                lStrBF = String.Format("{0:0.00}", lBFVal).PadLeft(10, " ")
                lSW.WriteLine(lStrYear & lStrMonth & lStrDay & lStrBF)
            End If
        Next

        lSW.Flush()
        lSW.Close()
        lSW = Nothing
    End Sub
End Module
