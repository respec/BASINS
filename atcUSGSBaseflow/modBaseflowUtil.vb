Imports atcData
Imports atcUtility
Imports atcTimeseriesBaseflow
Imports MapWinUtility
Imports System.Text.RegularExpressions

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
    Public Sub ComputeBaseflow(ByVal aArgs As atcDataAttributes, Optional ByVal aMakeAvailable As Boolean = False)
        Dim lClsBaseFlowCalculator As New atcTimeseriesBaseflow.atcTimeseriesBaseflow
        Try
            lClsBaseFlowCalculator.Calculate("Baseflow", aArgs)

            If aMakeAvailable Then
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
                'atcDataManager.DataSources.Add(lClsBaseFlowCalculator)

                Dim lBFTsersGroup As New atcTimeseriesGroup()
                Dim lPartDaily1 As atcTimeseries = Nothing
                Dim lPartDaily2 As atcTimeseries = Nothing
                Dim linearSlope As Double = Double.NaN
                For Each lTS As atcTimeseries In lClsBaseFlowCalculator.DataSets
                    Dim lConName As String = lTS.Attributes.GetValue("Constituent", "").ToLower()
                    If lConName.Contains("partmonthlyinter") Then
                        linearSlope = lTS.Attributes.GetValue("LinearSlope", Double.NaN)
                        Continue For
                    ElseIf lConName.Contains("monthly") OrElse lConName.Contains("bf_partdaily3") Then
                        Continue For
                    ElseIf lConName.Contains("bf_partdaily1") Then
                        lPartDaily1 = lTS
                    ElseIf lConName.Contains("bf_partdaily2") Then
                        lPartDaily2 = lTS
                    ElseIf lConName.Contains("yearly") Then
                        Continue For
                    Else
                        lBFTsersGroup.Add(lTS)
                    End If
                Next
                If lPartDaily1 IsNot Nothing AndAlso lPartDaily2 IsNot Nothing AndAlso Not Double.IsNaN(linearSlope) Then
                    Dim lPartDailyDiff As atcTimeseries = lPartDaily2 - lPartDaily1
                    Dim lPartDailyDiffSlope As atcTimeseries = lPartDailyDiff * linearSlope
                    Dim lPartDaily As atcTimeseries = lPartDaily1 + lPartDailyDiffSlope
                    lPartDaily.Attributes.SetValue("Constituent", "BF_PartDaily")
                    lBFTsersGroup.Add(lPartDaily)
                End If
                Dim lTsFlowG As atcTimeseriesGroup = aArgs.GetValue("streamflow", Nothing)
                If lTsFlowG IsNot Nothing AndAlso lTsFlowG.Count > 0 Then
                    Dim lSDate As Double = aArgs.GetValue("StartDate", -99)
                    Dim lEDate As Double = aArgs.GetValue("EndDate", -99)
                    If lSDate > 0 AndAlso lEDate > 0 AndAlso lEDate > lSDate Then
                        Dim lTsFlow As atcTimeseries = SubsetByDate(lTsFlowG(0), lSDate, lEDate, Nothing)
                        Dim lBFConsName As String = ""
                        Dim lROConsName As String = ""
                        Dim lBFPConsName As String = ""
                        Dim lROBFPGroup As New atcTimeseriesGroup()
                        For Each lBFTs As atcTimeseries In lBFTsersGroup
                            lBFConsName = lBFTs.Attributes.GetValue("Constituent")
                            Select Case lBFConsName.ToUpper()
                                Case "BF_HYSEPFIXED"
                                    lROConsName = "BF_HYSEPFIXED_RO"
                                    lBFPConsName = "BF_HYSEPFIXED_BFP"
                                Case "BF_HYSEPLOCMIN"
                                    lROConsName = "BF_HYSEPLOCMIN_RO"
                                    lBFPConsName = "BF_HYSEPLOCMIN_BFP"
                                Case "BF_HYSEPSLIDE"
                                    lROConsName = "BF_HYSEPSLIDE_RO"
                                    lBFPConsName = "BF_HYSEPSLIDE_BFP"
                                Case "BF_BFISTANDARD"
                                    lROConsName = "BF_BFISTANDARD_RO"
                                    lBFPConsName = "BF_BFISTANDARD_BFP"
                                Case "BF_BFIMODIFIED"
                                    lROConsName = "BF_BFIMODIFIED_RO"
                                    lBFPConsName = "BF_BFIMODIFIED_BFP"
                                Case "BF_PARTDAILY"
                                    lROConsName = "BF_PARTDAILY_RO"
                                    lBFPConsName = "BF_PARTDAILY_BFP"
                            End Select
                            Dim lTsRO As atcTimeseries = lTsFlow - lBFTs
                            Dim lTsBFP As atcTimeseries = Nothing
                            If lBFConsName.ToUpper().Contains("BFI") Then
                                Dim lTsBFP_Temp As atcTimeseries = lTsFlow.Clone() 'starts out with correct date range
                                Dim lDateBF As Double = 0
                                Dim lDateBFPct As Double = 0
                                Dim lSearchIndex As Integer = 0
                                Dim lFlowEndDate As Double = lTsFlow.Dates.Value(lTsFlow.numValues)
                                For I As Integer = 0 To lBFTs.numValues - 1
                                    lDateBF = lBFTs.Dates.Value(I)
                                    If lDateBF > lFlowEndDate Then Continue For
                                    For J As Integer = lSearchIndex To lTsBFP_Temp.numValues - 1
                                        lDateBFPct = lTsBFP_Temp.Dates.Value(J)
                                        If lDateBF < lDateBFPct Then
                                            lSearchIndex = J
                                            Exit For
                                        ElseIf lDateBF > lDateBFPct Then
                                            Continue For
                                        Else
                                            lTsBFP_Temp.Value(J + 1) = lBFTs.Value(I + 1) / lTsFlow.Value(J + 1) * 100
                                            lSearchIndex = J
                                            Exit For
                                        End If
                                    Next
                                Next
                                lTsBFP = SubsetByDate(lTsBFP_Temp, lTsFlow.Dates.Value(0), lTsFlow.Dates.Value(lTsFlow.numValues), Nothing)
                                lTsBFP_Temp.Clear()
                                'lTsBFP_Temp.Dates.Clear()
                                'Dim lNewVals(1) As Double
                                'lTsBFP_Temp.Values = lNewVals
                            Else
                                lTsBFP = lBFTs / lTsFlow * 100
                            End If

                            lTsRO.Attributes.SetValue("Constituent", lROConsName)
                            lTsBFP.Attributes.SetValue("Constituent", lBFPConsName)
                            lROBFPGroup.Add(lTsRO)
                            lROBFPGroup.Add(lTsBFP)
                        Next
                        lBFTsersGroup.Add(lROBFPGroup)
                    End If
                End If
                Dim lNewTSerSource As New atcTimeseriesSource()
                For Each lDS As atcDataSet In lBFTsersGroup
                    lNewTSerSource.AddDataSet(lDS)
                Next
                lNewTSerSource.Specification = lClsBaseFlowCalculator.Specification
                atcDataManager.DataSources.Add(lNewTSerSource)

                'Dim lStart As Double = -99.9
                'Dim lEnd As Double = -99.9
                'Dim lDA As Double = -99.9
                'Dim lSFTser As atcTimeseries = aArgs.GetValue("streamflow", Nothing)
                'If lSFTser IsNot Nothing Then
                '    Dim lTsGroupPart As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.PART, lStart, lEnd, lDA)
                '    Dim lTsGroupFixed As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.HySEPFixed, lStart, lEnd, lDA)
                '    Dim lTsGroupLocMin As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.HySEPLocMin, lStart, lEnd, lDA)
                '    Dim lTsGroupSlide As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.HySEPSlide, lStart, lEnd, lDA)
                '    Dim lTsGroupBFIStandard As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.BFIStandard, lStart, lEnd, lDA)
                '    Dim lTsGroupBFIModified As atcCollection = ConstructReportTsGroup(lSFTser, BFMethods.BFIModified, lStart, lEnd, lDA)

                'End If

            End If
        Catch ex As Exception
            Logger.Msg("Baseflow separation failed: " & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Baseflow separation")
        End Try
        'If pDidBFSeparation Then
        '    Logger.Msg("Baseflow separation is successful.", MsgBoxStyle.OkOnly, "Baseflow Separation")
        'End If
    End Sub

    ''' <summary>
    ''' Custom routine to match all time series' date duration such that they can be reported on even footing
    ''' currently focus on yearly bf result time series
    ''' </summary>
    ''' <param name="aTsFlowFullRange">The streamflow record at full date range for the batch analysis</param>
    ''' <param name="aBFReportGroup">The atcAttributes that holds all base-flow resulting time series from all methods</param>
    Public Sub AdjustDatesOfReportingTimeseries(ByRef aTsFlowFullRange As atcTimeseries, ByVal aBFReportGroup As atcDataAttributes)
        Dim lTsGroupPart As atcCollection = aBFReportGroup.GetValue("GroupPart", Nothing)
        Dim lTsGroupFixed As atcCollection = aBFReportGroup.GetValue("GroupFixed", Nothing)
        Dim lTsGroupLocMin As atcCollection = aBFReportGroup.GetValue("GroupLocMin", Nothing)
        Dim lTsGroupSlide As atcCollection = aBFReportGroup.GetValue("GroupSlide", Nothing)
        Dim lTsGroupBFIStandard As atcCollection = aBFReportGroup.GetValue("GroupBFIStandard", Nothing)
        Dim lTsGroupBFIModified As atcCollection = aBFReportGroup.GetValue("GroupBFIModified", Nothing)
        Dim lTsGroupBFLOW As atcCollection = aBFReportGroup.GetValue("GroupBFLOW", Nothing)
        Dim lTsGroupTwoPRDF As atcCollection = aBFReportGroup.GetValue("GroupTwoPRDF", Nothing)
        'lStart = lBFReportGroup.GetValue("AnalysisStart", -99)
        'lEnd = lBFReportGroup.GetValue("AnalysisEnd", -99)
        'lDA = lBFReportGroup.GetValue("Drainage Area", -99)
        Dim lReporBy As String = aBFReportGroup.GetValue(BFInputNames.Reportby, "Calendar")

        'Monthly Template
        'Dim lTsFlowMonthly As atcTimeseries = Aggregate(lTsFlowFullRange, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)

        Dim lTsFlowDailyBnd As atcTimeseries = Nothing
        Dim lTsFlowYearly As atcTimeseries = Nothing
        'Dim lTsFlowYearlySum As atcTimeseries = Nothing
        'Dim lTsFlowYearlyDepth As atcTimeseries = Nothing
        'Yearly Template
        If aTsFlowFullRange.numValues > JulianYear Then
            If lReporBy.ToLower() = "water" Then
                lTsFlowDailyBnd = SubsetByDateBoundary(aTsFlowFullRange, 10, 1, Nothing)
            Else
                lTsFlowDailyBnd = SubsetByDateBoundary(aTsFlowFullRange, 1, 1, Nothing)
            End If
            If lTsFlowDailyBnd Is Nothing OrElse lTsFlowDailyBnd.Values Is Nothing Then Exit Sub
            lTsFlowYearly = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
            If lTsFlowYearly Is Nothing Then Exit Sub
            If lTsGroupPart IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupPart, lReporBy)
            If lTsGroupFixed IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupFixed, lReporBy)
            If lTsGroupLocMin IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupLocMin, lReporBy)
            If lTsGroupSlide IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupSlide, lReporBy)
            If lTsGroupBFIStandard IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupBFIStandard, lReporBy)
            If lTsGroupBFIModified IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupBFIModified, lReporBy)
            If lTsGroupBFLOW IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupBFLOW, lReporBy)
            If lTsGroupTwoPRDF IsNot Nothing Then AdjustYearlyBFTserDates(lTsFlowYearly, lTsGroupTwoPRDF, lReporBy)
        End If
    End Sub

    ''' <summary>
    ''' Adjust the yearly base-flow time series to match the stream flow yearly time step and date range so 
    ''' they can be reported in synch
    ''' </summary>
    ''' <param name="aTsFlowYearly"></param>
    ''' <param name="aBFTserGroup"></param>
    Private Sub AdjustYearlyBFTserDates(ByVal aTsFlowYearly As atcTimeseries, ByVal aBFTserGroup As atcCollection,
                                        Optional aReportBy As String = "calendar")
        Dim lCommonStart As Double
        Dim lCommonEnd As Double
        If aBFTserGroup IsNot Nothing AndAlso aBFTserGroup.Count > 0 Then
            If aBFTserGroup.IndexFromKey("RateYearly") >= 0 AndAlso aBFTserGroup.IndexFromKey("DepthYearly") >= 0 Then
                If aReportBy.ToLower() = "water" Then
                    Dim ldailyBF As atcTimeseries = aBFTserGroup.ItemByKey("RateDaily")
                    Dim ldailyBFWateryear As atcTimeseries = SubsetByDateBoundary(ldailyBF, 10, 1, Nothing)
                    aBFTserGroup.RemoveByKey("RateYearly")
                    Dim lTsRateYearly = Aggregate(ldailyBFWateryear, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                    aBFTserGroup.Add("RateYearly", lTsRateYearly)
                    aBFTserGroup.RemoveByKey("DepthYearly")
                    Dim lTsDepthYearly = Aggregate(ldailyBFWateryear, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                    Dim lconversion_factor = pUADepth / aTsFlowYearly.Attributes.GetValue("Drainage Area", 1.0)
                    lTsDepthYearly = lTsDepthYearly * lconversion_factor
                    aBFTserGroup.Add("DepthYearly", lTsDepthYearly)
                End If
            Else
                Exit Sub
            End If
            For I As Integer = 0 To aBFTserGroup.Count - 1
                Dim lTs As atcTimeseries = aBFTserGroup.ItemByIndex(I)
                If lTs IsNot Nothing AndAlso lTs.Attributes.GetValue("tu") = atcTimeUnit.TUYear Then
                    lCommonStart = aTsFlowYearly.Attributes.GetValue("SJDay")
                    lCommonEnd = aTsFlowYearly.Attributes.GetValue("EJDay")

                    If lTs.Attributes.GetValue("SJDay") > lCommonStart Then
                        Dim lTsNewBFYearly As atcTimeseries = NewTimeseries(lCommonStart, lCommonEnd, atcTimeUnit.TUYear, 1, , -99)
                        aBFTserGroup.ItemByIndex(I) = MergeBaseflowTimeseries(lTsNewBFYearly, lTs, False, True)
                        lTs.Clear()
                        lTs = Nothing
                    ElseIf lTs.Attributes.GetValue("SJDay") < lCommonStart Then
                        aBFTserGroup.ItemByIndex(I) = SubsetByDate(lTs, lCommonStart, lCommonEnd, Nothing)
                        lTs.Clear()
                        lTs = Nothing
                    End If
                End If
            Next
        End If
    End Sub

    '{
    Public Sub ComputeBaseflowIntermittent(ByVal aArgs As atcDataAttributes, Optional ByVal aMakeAvailable As Boolean = False)
        'get the streamflow records
        Dim lTsFlowOri As atcTimeseries = aArgs.GetValue(BFInputNames.Streamflow)(0) 'original timeseries group that contains the original flow record
        'Me.Specification = "Base-Flow-" & lTsStreamflow.Attributes.GetValue("Location")
        'lEnglishFlg = aArgs.GetValue(BFInputNames.EnglishUnit, lEnglishFlg)
        'lStartDate = aArgs.GetValue(BFInputNames.StartDate)
        Dim lAnalysis_Start As Double = aArgs.GetValue(BFInputNames.StartDate)
        'lEndDate = aArgs.GetValue(BFInputNames.EndDate)
        Dim lAnalysis_End As Double = aArgs.GetValue(BFInputNames.EndDate)
        'lMethod = aArgs.GetValue("Method")
        Dim lMethods As ArrayList = aArgs.GetValue(BFInputNames.BFMethods)
        Dim lDrainageArea As Double = aArgs.GetValue(BFInputNames.DrainageArea)
        Dim lTsFlow As atcTimeseries = lTsFlowOri
        If Not Double.IsNaN(lAnalysis_Start) AndAlso
            Not Double.IsNaN(lAnalysis_End) AndAlso
            lAnalysis_End > lAnalysis_Start Then
            lTsFlow = SubsetByDate(lTsFlow, lAnalysis_Start, lAnalysis_End, Nothing)
        End If
        Dim lSaveWithProject As Boolean = aArgs.GetValue("SaveWithProject", True)
        Dim lMsgTitle As String = "Interactive Base-flow Analysis"
        'break up into continuous periods
        Dim lTserFullDateRange As New atcTimeseries(Nothing)
        Logger.Status("Creating Full Date Range Flow Record.")
        Try
            With lTserFullDateRange
                .Dates = New atcTimeseries(Nothing)
                .Dates.Values = NewDates(lAnalysis_Start, lAnalysis_End, atcTimeUnit.TUDay, 1)
                .numValues = lTserFullDateRange.Dates.numValues
                .SetInterval(atcTimeUnit.TUDay, 1)
                For I As Integer = 1 To .numValues
                    .Value(I) = -99.0
                Next
            End With
        Catch ex As Exception
            Logger.Msg("Creating Full Date Range Flow Record Failed.", MsgBoxStyle.Exclamation, lMsgTitle)
            Exit Sub
        End Try

        Dim lFlowStart As Double = lTsFlow.Dates.Value(0)
        Dim lFlowEnd As Double = lTsFlow.Dates.Value(lTsFlow.numValues)

        Logger.Status("Construct continuous streamflow records for analysis.")
        Dim lTsAnalysisGroup As New atcTimeseriesGroup()
        Try
            If lTsFlow.Attributes.GetValue("Count missing") > 0 Then
                If lTsFlow.Attributes.GetValue("Point") Then
                    lTsFlow.Attributes.SetValue("Point", False)
                End If
                Dim ctr As Integer = 1
                For I As Integer = 1 To lTsFlow.numValues
                    If Not Double.IsNaN(lTsFlow.Value(I)) Then
                        lFlowStart = lTsFlow.Dates.Value(I - 1)
                        While (I <= lTsFlow.numValues AndAlso Not Double.IsNaN(lTsFlow.Value(I)))
                            I = I + 1
                        End While
                        lFlowEnd = lTsFlow.Dates.Value(I - 1) 'need to record the end of the last time step
                        If (lFlowEnd - lFlowStart) >= 31 Then
                            Dim lTs As atcTimeseries = SubsetByDate(lTsFlow, lFlowStart, lFlowEnd, Nothing)
                            lTs.Attributes.SetValue("period", ctr)
                            lTsAnalysisGroup.Add(ctr, lTs)
                            ctr += 1
                        End If
                    End If
                Next
            Else
                'cuz lTsFlow for a station has to be used later, the clone will be cleared after batch bf
                If lTsFlow.Attributes.GetValue("Point") Then
                    lTsFlow.Attributes.SetValue("Point", False)
                End If
                lTsAnalysisGroup.Add(lTsFlow.Clone())
            End If
        Catch ex As Exception
            Logger.Msg("Construct continuous streamflow records for analysis failed.", MsgBoxStyle.Exclamation, lMsgTitle)
            Exit Sub
        End Try

        'Dim lTsChunkAttributes As New atcDataAttributes()
        Dim lTsFlowGroup As New atcTimeseriesGroup()
        Dim CalcBF As atcTimeseriesBaseflow.atcTimeseriesBaseflow = New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
        Dim lTotalGroupCt As Integer = lTsAnalysisGroup.Count
        Dim lGroupCtr As Integer = 1
        Dim lPattern As String = "_period_(\d)+"
        Dim rgx = New Regex(lPattern)
        Dim replacement = ""
        Dim lTsDailyCheck As atcTimeseries = Nothing
        Dim lActualStart As Double
        Dim lActualEnd As Double
        For Each lTsChunk As atcTimeseries In lTsAnalysisGroup
            Logger.Progress("Base-flow Analysis for continuous stream records", lGroupCtr, lTotalGroupCt)
            lActualStart = lTsChunk.Dates.Value(0)
            lActualEnd = lTsChunk.Dates.Value(lTsChunk.numValues)
            lTsDailyCheck = SubsetByDate(lTsChunk, lAnalysis_Start, lAnalysis_End, Nothing)
            If lTsDailyCheck Is Nothing OrElse lTsDailyCheck.Values Is Nothing OrElse lTsDailyCheck.numValues = 0 Then
                Logger.Dbg("Period " & lGroupCtr & " duration (" & DumpDate(lActualStart) & "~" & DumpDate(lActualEnd) & ") mismatch analysis duration (" & DumpDate(lAnalysis_Start) & "~" & DumpDate(lAnalysis_End) & "). Skipped.")
                lGroupCtr += 1
                Continue For
            Else
                lTsDailyCheck.Clear()
                lTsDailyCheck = Nothing
            End If
            lTsFlowGroup.Add(lTsChunk)
            'With lTsChunkAttributes
            '    .Clear()
            '    .SetValue(BFInputNames.Streamflow, lTsFlowGroup)
            '    lFlowStart = lTsChunk.Dates.Value(0)
            '    lFlowEnd = lTsChunk.Dates.Value(lTsChunk.numValues)
            '    .SetValue(BFInputNames.StartDate, lFlowStart) 'lAnalysis_Start) 'Date2J(lDates))
            '    .SetValue(BFInputNames.EndDate, lFlowEnd) 'lAnalysis_End) 'Date2J(lDates))
            '    .SetValue(BFInputNames.DrainageArea, lDrainageArea)
            '    .SetValue(BFInputNames.BFMethods, lMethods)
            '    .SetValue("BatchRun", False)
            'End With
            aArgs.SetValue("BatchRun", False)
            aArgs.SetValue(BFInputNames.Streamflow, lTsFlowGroup)
            'Below is for running BFLOW using original full time series including gaps
            'aArgs.SetValue("OriginalFlow", lTsFlow)
            If CalcBF.Calculate("baseflow", aArgs) Then
                'OutputDir = lStationOutDir
                OutputDir = aArgs.GetValue("OutputDir", "")
                'OutputFilenameRoot = lStation.BFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                Dim lCtr As Integer = lTsChunk.Attributes.GetValue("period", 0)
                If String.IsNullOrEmpty(OutputFilenameRoot) Then
                    OutputFilenameRoot = "BF_period_" & lCtr.ToString()
                Else
                    'Adjust to remove period from root name for the full span report files
                    OutputFilenameRoot = rgx.Replace(OutputFilenameRoot, replacement)
                    OutputFilenameRoot = OutputFilenameRoot.Replace("_period_", "")
                    OutputFilenameRoot &= "_period_" & lCtr.ToString()
                End If
                MethodsLastDone = lMethods
                For Each lAttr As atcDefinedValue In lTsFlowGroup(0).Attributes
                    If lTsFlowOri.Attributes.ContainsAttribute(lAttr.Definition.Name) Then
                        lTsFlowOri.Attributes.RemoveByKey(lAttr.Definition.Name)
                    End If
                    lTsFlowOri.Attributes.Add(lAttr.Clone())
                Next
                If IO.Directory.Exists(OutputDir) Then
                    For Each lMethod As BFMethods In lMethods
                        Logger.Status("writing legacy format text output files for stream record " & lGroupCtr & " for method: " & lMethod.ToString())
                        ASCIIOriginal(lTsChunk, lMethod)
                    Next
                    Logger.Status("writing common format text output files for stream record " & lGroupCtr)
                    ASCIICommon(lTsChunk, aArgs)
                End If
            End If
            'lStation.Message &= CalcBF.BF_Message.Trim()
            lTsFlowGroup.Clear()
            lGroupCtr += 1
        Next 'period
        'after the results are written, then merge, the intermittent time series can be cleared
        'the lTsAnalysisGroup contains the chuncky or original time series' clone (if only 1 period), these will be cleared

        Dim lBFReportGroups As atcDataAttributes = Nothing
        Try
            Logger.Status("Construct full span base-flow analysis time series.")
            lBFReportGroups = MergeBaseflowResults(lTserFullDateRange, lTsAnalysisGroup, "Daily", True)
        Catch ex As Exception
            Logger.Msg("Construct full span base-flow analysis time series failed.", MsgBoxStyle.Exclamation, lMsgTitle)
            Exit Sub
        End Try
        With lBFReportGroups
            .SetValue("AnalysisStart", lAnalysis_Start)
            .SetValue("AnalysisEnd", lAnalysis_End)
            .SetValue("Drainage Area", lDrainageArea)
            .SetValue("ReportGroupsAvailable", True)
            .SetValue("ReportFileSuffix", "fullspan")
            .SetValue("ForFullSpan", True)
            .SetValue("Specification", "")
            lMethods = aArgs.GetValue(BFInputNames.BFMethods, Nothing)
            .SetValue(BFInputNames.BFMethods, lMethods)
            If lMethods.Contains(BFMethods.BFIStandard) Then
                Dim lFrac = aArgs.GetValue(BFInputNames.BFITurnPtFrac, Double.NaN) '"BFIFrac"
                .SetValue(BFInputNames.BFITurnPtFrac, lFrac)
            End If
            If lMethods.Contains(BFMethods.BFIModified) Then
                Dim lK1Day = aArgs.GetValue(BFInputNames.BFIRecessConst, Double.NaN) '"BFIK1Day"
                .SetValue(BFInputNames.BFIRecessConst, lK1Day) '"BFIK1Day"
            End If
            If lMethods.Contains(BFMethods.BFIStandard) OrElse lMethods.Contains(BFMethods.BFIModified) Then
                Dim lNDay = aArgs.GetValue(BFInputNames.BFINDayScreen, Double.NaN) '"BFINDay"
                .SetValue(BFInputNames.BFINDayScreen, lNDay) '"BFINDay"
                Dim lBFIYearBasis As String = aArgs.GetValue(BFInputNames.BFIReportby, "") '"BFIReportby"
                .SetValue(BFInputNames.BFIReportby, lBFIYearBasis) '"BFIReportby"
            End If
            If lMethods.Contains(BFMethods.BFLOW) Then
                Dim lalpha = aArgs.GetValue(BFInputNames.BFLOWFilter, Double.NaN)
                .SetValue(BFInputNames.BFLOWFilter, lalpha)
            End If
            If lMethods.Contains(BFMethods.TwoPRDF) Then
                Dim lRC = aArgs.GetValue(BFInputNames.TwoPRDFRC, Double.NaN)
                .SetValue(BFInputNames.TwoPRDFRC, lRC)
                Dim lBFImax = aArgs.GetValue(BFInputNames.TwoPRDFBFImax, Double.NaN)
                .SetValue(BFInputNames.TwoPRDFBFImax, lBFImax)
                Dim lDF2PMethod = aArgs.GetValue(BFInputNames.TwoParamEstMethod, clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE)
                .SetValue(BFInputNames.TwoParamEstMethod, lDF2PMethod)
            End If
            .SetValue(BFInputNames.Reportby, aArgs.GetValue(BFInputNames.Reportby, BFInputNames.ReportbyCY))
            .SetValue(BFInputNames.FullSpanDuration, aArgs.GetValue(BFInputNames.FullSpanDuration, False))
        End With

        Dim lTsFlowFullRange As atcTimeseries = Nothing
        Try
            Logger.Status("Construct full span streamflow time series for report.")
            lTsFlowFullRange = MergeBaseflowTimeseries(lTserFullDateRange, lTsFlow, False, True)  'MergeTimeseries(lTmpGroup, True)
            AdjustDatesOfReportingTimeseries(lTsFlowFullRange, lBFReportGroups)
            'lTsFlowFullRange.Clear()
            'lTsFlowFullRange = Nothing
        Catch ex As Exception
            Logger.Msg("Construct full span streamflow time series for report failed.", MsgBoxStyle.Exclamation, lMsgTitle)
            Exit Sub
        End Try

        Try
            Logger.Status("Writing full span base-flow analysis result in common text format.")
            If IO.Directory.Exists(OutputDir) Then
                ASCIICommon(lTsFlowFullRange, lBFReportGroups)
                Dim lSpec As String = lBFReportGroups.GetValue("Specification")
                If lSaveWithProject AndAlso Not String.IsNullOrEmpty(lSpec) Then
                    CalcBF.Specification = lSpec
                End If
            End If
        Catch ex As Exception
            Logger.Msg("Writing full span base-flow analysis result in common text format failed.", MsgBoxStyle.Exclamation, lMsgTitle)
            'don't have to exit
        End Try

        If aMakeAvailable Then
            Logger.Status("Construct full time span base-flow analysis result.")
            'reconstruct the bf time series as attribute to pDataGroup(0) of the analysis window
            'first get rid of old method's group
            lTsFlow.Attributes.SetValue("Baseflow", Nothing)
            Dim lNewBFTserGroup As atcTimeseriesGroup = New atcTimeseriesGroup()

            lTsFlow.Attributes.SetValue("Baseflow", lNewBFTserGroup)
            Dim lTsGroupPart As atcCollection = Nothing
            Dim lTsGroupFixed As atcCollection = Nothing
            Dim lTsGroupLocMin As atcCollection = Nothing
            Dim lTsGroupSlide As atcCollection = Nothing
            Dim lTsGroupBFIStandard As atcCollection = Nothing
            Dim lTsGroupBFIModified As atcCollection = Nothing
            Dim lTsGroupBFLOW As atcCollection = Nothing
            Dim lTsGroupTwoPRDF As atcCollection = Nothing
            With lBFReportGroups
                lTsGroupPart = .GetValue("GroupPart", Nothing)
                lTsGroupFixed = .GetValue("GroupFixed", Nothing)
                lTsGroupLocMin = .GetValue("GroupLocMin", Nothing)
                lTsGroupSlide = .GetValue("GroupSlide", Nothing)
                lTsGroupBFIStandard = .GetValue("GroupBFIStandard", Nothing)
                lTsGroupBFIModified = .GetValue("GroupBFIModified", Nothing)
                lTsGroupBFLOW = .GetValue("GroupBFLOW", Nothing)
                lTsGroupTwoPRDF = .GetValue("GroupTwoPRDF", Nothing)
            End With
            If lTsGroupPart IsNot Nothing AndAlso lTsGroupPart.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupPart.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.PART)
                lTs.Attributes.SetValue("Scenario", BFMethods.PART.ToString())
                lTs.Attributes.SetValue("Constituent", "BF_Part")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_Part")
                        .SetValue("Method", BFMethods.PART)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_Part")
                        .SetValue("Method", BFMethods.PART)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If
            If lTsGroupFixed IsNot Nothing AndAlso lTsGroupFixed.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupFixed.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.HySEPFixed)
                lTs.Attributes.SetValue("Scenario", BFMethods.HySEPFixed.ToString())
                lTs.Attributes.SetValue("Constituent", "BF_HySEPFixed")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_HySEPFixed")
                        .SetValue("Method", BFMethods.HySEPFixed)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_HySEPFixed")
                        .SetValue("Method", BFMethods.HySEPFixed)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If

            If lTsGroupLocMin IsNot Nothing AndAlso lTsGroupLocMin.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupLocMin.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.HySEPLocMin)
                lTs.Attributes.SetValue("Constituent", "BF_HySEPLocMin")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_HySEPLocMin")
                        .SetValue("Method", BFMethods.HySEPLocMin)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_HySEPLocMin")
                        .SetValue("Method", BFMethods.HySEPLocMin)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If

            If lTsGroupSlide IsNot Nothing AndAlso lTsGroupSlide.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupSlide.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.HySEPSlide)
                lTs.Attributes.SetValue("Constituent", "BF_HySEPSlide")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_HySEPSlide")
                        .SetValue("Method", BFMethods.HySEPSlide)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_HySEPSlide")
                        .SetValue("Method", BFMethods.HySEPSlide)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If
            If lTsGroupBFIStandard IsNot Nothing AndAlso lTsGroupBFIStandard.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupBFIStandard.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.BFIStandard)
                lTs.Attributes.SetValue("Constituent", "BF_BFIStandard")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    'Dim lTsFlowSameSpan As atcTimeseries = Nothing
                    'If lTsFlowFullRange.numValues > lTs.numValues Then
                    '    lTsFlowSameSpan = SubsetByDate(lTsFlowFullRange, lTs.Dates.Value(0), lTs.Dates.Value(lTs.numValues), Nothing)
                    'End If
                    'Dim lTsBFP As atcTimeseries = Nothing
                    'If lTsFlowSameSpan IsNot Nothing Then
                    '    Dim lTsBFPSameSpan As atcTimeseries = lTs / lTsFlowSameSpan * 100
                    '    lTsBFP = lTsFlowFullRange.Clone()
                    '    For I As Integer = 0 To lTsBFP.numValues
                    '        lTsBFP.Value(I) = Double.NaN
                    '    Next
                    '    'back fill to full date range
                    '    Dim lStartIndex As Integer = Array.IndexOf(lTsBFP.Dates.Values, lTs.Dates.Value(0))
                    '    Dim lEndIndex As Integer = Array.IndexOf(lTsBFP.Dates.Values, lTs.Dates.Value(lTs.Dates.numValues - 1))
                    '    For J As Integer = lStartIndex To lEndIndex
                    '        lTsBFP.Value(lStartIndex + 1) = lTsBFPSameSpan.Value(J - lStartIndex + 1)
                    '    Next
                    'Else
                    '    lTsBFP = lTs / lTsFlowFullRange * 100
                    'End If

                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs)
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_BFIStandard")
                        .SetValue("Method", BFMethods.BFIStandard)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    'Dim lTsRO As atcTimeseries = lTsFlowFullRange - lTs
                    'Dim lTsRO As atcTimeseries = Nothing
                    'If lTsFlowSameSpan IsNot Nothing Then
                    '    Dim lTsROSameSpan As atcTimeseries = lTsFlowSameSpan - lTs
                    '    'back fill to full date range
                    '    lTsRO = lTsFlowFullRange.Clone()
                    '    For I As Integer = 0 To lTsRO.numValues
                    '        lTsRO.Value(I) = Double.NaN
                    '    Next
                    '    Dim lStartIndex As Integer = Array.IndexOf(lTsRO.Dates.Values, lTs.Dates.Value(0))
                    '    Dim lEndIndex As Integer = Array.IndexOf(lTsRO.Dates.Values, lTs.Dates.Value(lTs.Dates.numValues - 1))
                    '    For J As Integer = lStartIndex To lEndIndex
                    '        lTsRO.Value(lStartIndex + 1) = lTsROSameSpan.Value(J - lStartIndex + 1)
                    '    Next
                    'Else
                    '    lTsRO = lTsFlowFullRange - lTs
                    'End If
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs)
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_BFIStandard")
                        .SetValue("Method", BFMethods.BFIStandard)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If
            If lTsGroupBFIModified IsNot Nothing AndAlso lTsGroupBFIModified.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupBFIModified.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.BFIModified)
                lTs.Attributes.SetValue("Constituent", "BF_BFIModified")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs)
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_BFIModified")
                        .SetValue("Method", BFMethods.BFIModified)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)

                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs)
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_BFIModified")
                        .SetValue("Method", BFMethods.BFIModified)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If

            If lTsGroupBFLOW IsNot Nothing AndAlso lTsGroupBFLOW.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupBFLOW.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.BFLOW)
                lTs.Attributes.SetValue("Constituent", "BF_BFLOW")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_BFLOW")
                        .SetValue("Method", BFMethods.BFLOW)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_BFLOW")
                        .SetValue("Method", BFMethods.BFLOW)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If

            If lTsGroupTwoPRDF IsNot Nothing AndAlso lTsGroupTwoPRDF.Count > 0 Then
                Dim lTs As atcTimeseries = lTsGroupTwoPRDF.ItemByKey("RateDaily")
                For I As Integer = 1 To lTs.numValues
                    If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
                Next
                lTs.Attributes.SetValue("Method", BFMethods.TwoPRDF)
                lTs.Attributes.SetValue("Constituent", "BF_TwoPRDF")
                lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
                lTs.Attributes.SetValue("Units", "cubic feet per second")
                If lDrainageArea > 0 Then
                    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
                Else
                    lTs.Attributes.SetValue("Drainage Area", -99)
                End If
                lNewBFTserGroup.Add(lTs)
                If lTsFlowFullRange IsNot Nothing Then
                    Dim lTsBFP As atcTimeseries = CalculateBFP_RO("bfp", lTsFlowFullRange, lTs) 'lTs / lTsFlowFullRange * 100
                    With lTsBFP.Attributes
                        .SetValue("Constituent", "BFPct_TwoPRDF")
                        .SetValue("Method", BFMethods.TwoPRDF)
                    End With
                    lNewBFTserGroup.Add(lTsBFP)
                    Dim lTsRO As atcTimeseries = CalculateBFP_RO("ro", lTsFlowFullRange, lTs) 'lTsFlowFullRange - lTs
                    With lTsRO.Attributes
                        .SetValue("Constituent", "RO_TwoPRDF")
                        .SetValue("Method", BFMethods.TwoPRDF)
                        .SetValue("Units", "cubic feet per second")
                    End With
                    lNewBFTserGroup.Add(lTsRO)
                End If
            End If

            Logger.Status("Add full time span base-flow analysis result to data manager.")
            Dim lOldDataSource As atcDataSource = Nothing
            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                If lDataSource.Specification = CalcBF.Specification Then
                    lOldDataSource = lDataSource
                    Exit For
                End If
            Next
            If lOldDataSource IsNot Nothing Then
                lOldDataSource.Clear()
                atcDataManager.DataSources.Remove(lOldDataSource)
            End If

            Dim lNewTSerSource As New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
            For Each lDS As atcDataSet In lNewBFTserGroup
                lDS.Attributes.SetValue("Data Source", CalcBF.Specification)
                lNewTSerSource.AddDataSet(lDS)
            Next
            lNewTSerSource.Specification = CalcBF.Specification
            atcDataManager.DataSources.Add(lNewTSerSource)
        End If

        'Catch ex As Exception
        '    Logger.Msg("Baseflow separation failed: " & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Baseflow separation")
        'End Try
        'If pDidBFSeparation Then
        '    Logger.Msg("Baseflow separation is successful.", MsgBoxStyle.OkOnly, "Baseflow Separation")
        'End If

        'return the global 'OutputFilenameRoot' to its original form
        OutputFilenameRoot = rgx.Replace(OutputFilenameRoot, replacement)
        OutputFilenameRoot = OutputFilenameRoot.Replace("_period_", "")

        Logger.Status("Hide")
    End Sub '}

    ''' <summary>
    ''' A convenient function for calculating BFP and RO from streamflow and base-flow
    ''' </summary>
    ''' <param name="aOpn">type of operation, either 'bfp' (base-flow %) or 'ro' (runoff)</param>
    ''' <param name="aTsFlowFullRange">streamflow of full date range</param>
    ''' <param name="aTs">base-flow timeseries as estimated by a method</param>
    ''' <returns></returns>
    Private Function CalculateBFP_RO(ByVal aOpn As String, ByVal aTsFlowFullRange As atcTimeseries, ByVal aTs As atcTimeseries) As atcTimeseries
        If aTsFlowFullRange Is Nothing OrElse aTs Is Nothing Then
            Return Nothing
        End If
        Dim lTsFlowSameSpan As atcTimeseries = Nothing
        If aTsFlowFullRange.numValues > aTs.numValues Then
            lTsFlowSameSpan = SubsetByDate(aTsFlowFullRange, aTs.Dates.Value(0), aTs.Dates.Value(aTs.numValues), Nothing)
        End If
        Dim lTsResult As atcTimeseries = Nothing
        If lTsFlowSameSpan IsNot Nothing Then
            Dim lTsResultSameSpan As atcTimeseries = Nothing
            If aOpn = "bfp" Then
                lTsResultSameSpan = aTs / lTsFlowSameSpan * 100
            ElseIf aOpn = "ro" Then
                lTsResultSameSpan = lTsFlowSameSpan - aTs
            End If
            lTsResult = aTsFlowFullRange.Clone()
            For I As Integer = 0 To lTsResult.numValues
                lTsResult.Value(I) = Double.NaN
            Next
            'back fill to full date range
            Dim lStartIndex As Integer = Array.IndexOf(lTsResult.Dates.Values, aTs.Dates.Value(0))
            Dim lEndIndex As Integer = Array.IndexOf(lTsResult.Dates.Values, aTs.Dates.Value(aTs.Dates.numValues - 1))
            For J As Integer = lStartIndex To lEndIndex
                lTsResult.Value(lStartIndex + 1) = lTsResultSameSpan.Value(J - lStartIndex + 1)
            Next
        Else
            'the streamflow and baseflow time series are of equal length
            If aOpn = "bfp" Then
                lTsResult = aTs / aTsFlowFullRange * 100
            ElseIf aOpn = "ro" Then
                lTsResult = aTsFlowFullRange - aTs
            End If
        End If
        Return lTsResult
    End Function

    '{
    ''' <summary>
    ''' Calculate Runoff from Streamflow using the HySEP_LocMin method (for WinHSPF)
    ''' </summary>
    ''' <param name="aArgs">Base-flow separation inputs (e.g. streamflow and parameters)</param>
    ''' <return>Error message, start with "ERROR". If successful, then it will begin with "SUCCESS,Key" string</return>
    Public Function ComputeRunoffIntermittent(ByVal aArgs As atcDataAttributes) As String
        Dim lMessage As String = ""
        Dim lTsFlow As atcTimeseries = aArgs.GetValue(BFInputNames.Streamflow)(0) 'original timeseries group that contains the original flow record
        Dim lAnalysis_Start As Double = aArgs.GetValue(BFInputNames.StartDate)
        Dim lAnalysis_End As Double = aArgs.GetValue(BFInputNames.EndDate)
        Dim lMethods As ArrayList = aArgs.GetValue(BFInputNames.BFMethods)
        Dim lDrainageArea As Double = aArgs.GetValue(BFInputNames.DrainageArea)

        Dim lMsgTitle As String = "Interactive Base-flow Analysis"
        'break up into continuous periods
        Dim lTserFullDateRange As New atcTimeseries(Nothing)
        Logger.Dbg("Creating Full Date Range Flow Record.")
        Try
            With lTserFullDateRange
                .Dates = New atcTimeseries(Nothing)
                .Dates.Values = NewDates(lAnalysis_Start, lAnalysis_End, atcTimeUnit.TUDay, 1)
                .numValues = lTserFullDateRange.Dates.numValues
                .SetInterval(atcTimeUnit.TUDay, 1)
                For I As Integer = 1 To .numValues
                    .Value(I) = -99.0
                Next
            End With
        Catch ex As Exception
            lMessage = "ERROR:Creating Full Date Range Flow Record Failed"
            Return lMessage
        End Try

        Dim lFlowStart As Double = lTsFlow.Dates.Value(0)
        Dim lFlowEnd As Double = lTsFlow.Dates.Value(lTsFlow.numValues)

        Logger.Dbg("Construct continuous streamflow records for analysis.")
        Dim lTsAnalysisGroup As New atcTimeseriesGroup()
        Try
            If lTsFlow.Attributes.GetValue("Count missing") > 0 Then
                If lTsFlow.Attributes.GetValue("Point") Then
                    lTsFlow.Attributes.SetValue("Point", False)
                End If
                Dim ctr As Integer = 1
                For I As Integer = 1 To lTsFlow.numValues
                    If Not Double.IsNaN(lTsFlow.Value(I)) Then
                        lFlowStart = lTsFlow.Dates.Value(I - 1)
                        While (I <= lTsFlow.numValues AndAlso Not Double.IsNaN(lTsFlow.Value(I)))
                            I = I + 1
                        End While
                        lFlowEnd = lTsFlow.Dates.Value(I - 1) 'need to record the end of the last time step
                        If (lFlowEnd - lFlowStart) >= 31 Then
                            Dim lTs As atcTimeseries = SubsetByDate(lTsFlow, lFlowStart, lFlowEnd, Nothing)
                            lTs.Attributes.SetValue("period", ctr)
                            lTsAnalysisGroup.Add(ctr, lTs)
                            ctr += 1
                        End If
                    End If
                Next
            Else
                'cuz lTsFlow for a station has to be used later, the clone will be cleared after batch bf
                If lTsFlow.Attributes.GetValue("Point") Then
                    lTsFlow.Attributes.SetValue("Point", False)
                End If
                lTsAnalysisGroup.Add(lTsFlow.Clone())
            End If
        Catch ex As Exception
            lMessage = "ERROR:Construct continuous streamflow records for analysis failed."
            Return lMessage
        End Try

        Dim lTsFlowGroup As New atcTimeseriesGroup()
        Dim CalcBF As atcTimeseriesBaseflow.atcTimeseriesBaseflow = New atcTimeseriesBaseflow.atcTimeseriesBaseflow()
        Dim lTotalGroupCt As Integer = lTsAnalysisGroup.Count
        Dim lGroupCtr As Integer = 1
        Dim lPattern As String = "_period_(\d)+"
        Dim rgx = New Regex(lPattern)
        Dim replacement = ""
        Dim lTsDailyCheck As atcTimeseries = Nothing
        Dim lActualStart As Double
        Dim lActualEnd As Double
        For Each lTsChunk As atcTimeseries In lTsAnalysisGroup
            'Logger.Progress("Base-flow Analysis for continuous stream records", lGroupCtr, lTotalGroupCt)
            lActualStart = lTsChunk.Dates.Value(0)
            lActualEnd = lTsChunk.Dates.Value(lTsChunk.numValues)
            lTsDailyCheck = SubsetByDate(lTsChunk, lAnalysis_Start, lAnalysis_End, Nothing)
            If lTsDailyCheck Is Nothing OrElse lTsDailyCheck.Values Is Nothing OrElse lTsDailyCheck.numValues = 0 Then
                Logger.Dbg("Period " & lGroupCtr & " duration (" & DumpDate(lActualStart) & "~" & DumpDate(lActualEnd) & ") mismatch analysis duration (" & DumpDate(lAnalysis_Start) & "~" & DumpDate(lAnalysis_End) & "). Skipped.")
                lGroupCtr += 1
                Continue For
            Else
                lTsDailyCheck.Clear()
                lTsDailyCheck = Nothing
            End If
            lTsFlowGroup.Add(lTsChunk)
            aArgs.SetValue("BatchRun", False)
            aArgs.SetValue(BFInputNames.Streamflow, lTsFlowGroup)
            'Below is for running BFLOW using original full time series including gaps
            'aArgs.SetValue("OriginalFlow", lTsFlow)
            If CalcBF.Calculate("baseflow", aArgs) Then
                MethodsLastDone = lMethods
            End If
            'lStation.Message &= CalcBF.BF_Message.Trim()
            lTsFlowGroup.Clear()
            lGroupCtr += 1
        Next 'period
        'after the results are written, then merge, the intermittent time series can be cleared
        'the lTsAnalysisGroup contains the chuncky or original time series' clone (if only 1 period), these will be cleared
        Dim lBFReportGroups As atcDataAttributes = Nothing
        Try
            Logger.Dbg("Construct full span base-flow analysis time series.")
            lBFReportGroups = MergeBaseflowResults(lTserFullDateRange, lTsAnalysisGroup, "Daily", True)
        Catch ex As Exception
            lMessage = "ERROR:Construct full span base-flow analysis time series failed."
            Return lMessage
        End Try
        With lBFReportGroups
            .SetValue("AnalysisStart", lAnalysis_Start)
            .SetValue("AnalysisEnd", lAnalysis_End)
            .SetValue("Drainage Area", lDrainageArea)
            .SetValue("ReportGroupsAvailable", True)
            .SetValue("ReportFileSuffix", "fullspan")
            .SetValue("ForFullSpan", True)
            lMethods = aArgs.GetValue(BFInputNames.BFMethods, Nothing)
            .SetValue(BFInputNames.BFMethods, lMethods)
            If lMethods.Contains(BFMethods.BFIStandard) Then
                Dim lFrac = aArgs.GetValue(BFInputNames.BFITurnPtFrac, Double.NaN) '"BFIFrac"
                .SetValue(BFInputNames.BFITurnPtFrac, lFrac)
            End If
            If lMethods.Contains(BFMethods.BFIModified) Then
                Dim lK1Day = aArgs.GetValue(BFInputNames.BFIRecessConst, Double.NaN) '"BFIK1Day"
                .SetValue(BFInputNames.BFIRecessConst, lK1Day) '"BFIK1Day"
            End If
            If lMethods.Contains(BFMethods.BFIStandard) OrElse lMethods.Contains(BFMethods.BFIModified) Then
                Dim lNDay = aArgs.GetValue(BFInputNames.BFINDayScreen, Double.NaN) '"BFINDay"
                .SetValue(BFInputNames.BFINDayScreen, lNDay) '"BFINDay"
                Dim lBFIYearBasis As String = aArgs.GetValue(BFInputNames.BFIReportby, "") '"BFIReportby"
                .SetValue(BFInputNames.BFIReportby, lBFIYearBasis) '"BFIReportby"
            End If
            If lMethods.Contains(BFMethods.BFLOW) Then
                Dim lalpha = aArgs.GetValue(BFInputNames.BFLOWFilter, Double.NaN)
                .SetValue(BFInputNames.BFLOWFilter, lalpha)
            End If
            If lMethods.Contains(BFMethods.TwoPRDF) Then
                Dim lRC = aArgs.GetValue(BFInputNames.TwoPRDFRC, Double.NaN)
                .SetValue(BFInputNames.TwoPRDFRC, lRC)
                Dim lBFImax = aArgs.GetValue(BFInputNames.TwoPRDFBFImax, Double.NaN)
                .SetValue(BFInputNames.TwoPRDFBFImax, lBFImax)
                Dim lDF2PMethod = aArgs.GetValue(BFInputNames.TwoParamEstMethod, clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE)
                .SetValue(BFInputNames.TwoParamEstMethod, lDF2PMethod)
            End If
        End With

        Dim lTsFlowFullRange As atcTimeseries = Nothing
        Try
            Logger.Dbg("Construct full span streamflow time series for report.")
            lTsFlowFullRange = MergeBaseflowTimeseries(lTserFullDateRange, lTsFlow, False, True)  'MergeTimeseries(lTmpGroup, True)
            AdjustDatesOfReportingTimeseries(lTsFlowFullRange, lBFReportGroups)
            'lTsFlowFullRange.Clear()
            'lTsFlowFullRange = Nothing
        Catch ex As Exception
            lMessage = "ERROR:Construct full span streamflow time series for report failed."
            Return lMessage
        End Try

        Dim lKey As String = "RO_HySEPLocMin" 'I think this should be read from the lMethods. We should not hard code it here.
        Logger.Dbg("Construct full time span base-flow analysis result.")
        'reconstruct the bf time series as attribute to pDataGroup(0) of the analysis window
        'first get rid of old method's group
        Dim lTsGroupLocMin As atcCollection = Nothing
        With lBFReportGroups
            lTsGroupLocMin = .GetValue("GroupLocMin", Nothing)
        End With
        If lTsGroupLocMin IsNot Nothing AndAlso lTsGroupLocMin.Count > 0 Then
            Dim lTs As atcTimeseries = lTsGroupLocMin.ItemByKey("RateDaily")
            For I As Integer = 1 To lTs.numValues
                If lTs.Value(I) < 0 Then lTs.Value(I) = Double.NaN
            Next
            'lTs.Attributes.SetValue("Method", BFMethods.HySEPFixed)
            'lTs.Attributes.SetValue("Scenario", BFMethods.HySEPFixed.ToString())
            'lTs.Attributes.SetValue("Constituent", "BF_HySEPFixed")
            'lTs.Attributes.SetValue("Location", lTsFlow.Attributes.GetValue("Location"))
            'lTs.Attributes.SetValue("Units", "cubic feet per second")
            'If lDrainageArea > 0 Then
            '    lTs.Attributes.SetValue("Drainage Area", lDrainageArea)
            'Else
            '    lTs.Attributes.SetValue("Drainage Area", -99)
            'End If
            'lNewBFTserGroup.Add(lTs)
            If lTsFlowFullRange IsNot Nothing Then
                Dim lTsRO As atcTimeseries = lTsFlowFullRange - lTs
                With lTsRO.Attributes
                    .SetValue("Constituent", lKey)
                    .SetValue("Method", BFMethods.HySEPLocMin) 'This should also be read from the lMethods
                    .SetValue("Units", "cubic feet per second")
                    .SetValue("Specification", CalcBF.Specification)
                End With
                Dim lTSRo0 As atcTimeseries = lTsFlow.Attributes.GetValue(lKey, Nothing)
                If lTSRo0 IsNot Nothing Then
                    lTSRo0.Clear()
                    lTSRo0 = Nothing
                    lTsFlow.Attributes.RemoveByKey(lKey)
                End If
                lTsFlow.Attributes.SetValue(lKey, lTsRO)
                lMessage = "SUCCESS," + lKey
            End If
        Else
            lMessage = "ERROR:Base-flow separation failed."
        End If
        Return lMessage
    End Function '}

    Public Sub ASCIICommon(ByVal aTs As atcTimeseries, Optional ByVal args As atcDataAttributes = Nothing)

        If Not IO.Directory.Exists(OutputDir) Then
            Exit Sub
        End If

        'Organize data
        Dim lStart As Double = -99.9
        Dim lEnd As Double = -99.9
        Dim lDA As Double = -99.9

        Dim lReportGroupsAvailable As Boolean = False
        Dim lReportFileSuffix As String = ""
        Dim lReportBy As String = ""
        If args IsNot Nothing Then
            lReportGroupsAvailable = args.GetValue("ReportGroupsAvailable", False)
            lReportFileSuffix = args.GetValue("ReportFileSuffix", "")
            lReportBy = args.GetValue("ReportBy", "Calendar")
        End If

        Dim lTsGroupPart As atcCollection = Nothing
        Dim lTsGroupFixed As atcCollection = Nothing
        Dim lTsGroupLocMin As atcCollection = Nothing
        Dim lTsGroupSlide As atcCollection = Nothing
        Dim lTsGroupBFIStandard As atcCollection = Nothing
        Dim lTsGroupBFIModified As atcCollection = Nothing
        Dim lTsGroupBFLOW As atcCollection = Nothing
        Dim lTsGroupTwoPRDF As atcCollection = Nothing
        If lReportGroupsAvailable Then
            lTsGroupPart = args.GetValue("GroupPart", Nothing)
            lTsGroupFixed = args.GetValue("GroupFixed", Nothing)
            lTsGroupLocMin = args.GetValue("GroupLocMin", Nothing)
            lTsGroupSlide = args.GetValue("GroupSlide", Nothing)
            lTsGroupBFIStandard = args.GetValue("GroupBFIStandard", Nothing)
            lTsGroupBFIModified = args.GetValue("GroupBFIModified", Nothing)
            lTsGroupBFLOW = args.GetValue("GroupBFLOW", Nothing)
            lTsGroupTwoPRDF = args.GetValue("GroupTwoPRDF", Nothing)
            lStart = args.GetValue("AnalysisStart", -99)
            lEnd = args.GetValue("AnalysisEnd", -99)
            lDA = args.GetValue("Drainage Area", -99)
        Else
            lTsGroupPart = ConstructReportTsGroup(aTs, BFMethods.PART, lStart, lEnd, lDA, lReportBy)
            lTsGroupFixed = ConstructReportTsGroup(aTs, BFMethods.HySEPFixed, lStart, lEnd, lDA, lReportBy)
            lTsGroupLocMin = ConstructReportTsGroup(aTs, BFMethods.HySEPLocMin, lStart, lEnd, lDA, lReportBy)
            lTsGroupSlide = ConstructReportTsGroup(aTs, BFMethods.HySEPSlide, lStart, lEnd, lDA, lReportBy)
            lTsGroupBFIStandard = ConstructReportTsGroup(aTs, BFMethods.BFIStandard, lStart, lEnd, lDA, lReportBy)
            lTsGroupBFIModified = ConstructReportTsGroup(aTs, BFMethods.BFIModified, lStart, lEnd, lDA, lReportBy)
            lTsGroupBFLOW = ConstructReportTsGroup(aTs, BFMethods.BFLOW, lStart, lEnd, lDA, lReportBy)
            lTsGroupTwoPRDF = ConstructReportTsGroup(aTs, BFMethods.TwoPRDF, lStart, lEnd, lDA, lReportBy)
        End If

        If (lStart < 0 AndAlso lEnd < 0) OrElse lDA <= 0 Then Exit Sub

        If Not lReportGroupsAvailable Then
            If lTsGroupPart Is Nothing Then lTsGroupPart = ConstructReportTsGroup(aTs, BFMethods.PART, lStart, lEnd, lDA, lReportBy)
            If lTsGroupFixed Is Nothing Then lTsGroupFixed = ConstructReportTsGroup(aTs, BFMethods.HySEPFixed, lStart, lEnd, lDA, lReportBy)
            If lTsGroupLocMin Is Nothing Then lTsGroupLocMin = ConstructReportTsGroup(aTs, BFMethods.HySEPLocMin, lStart, lEnd, lDA, lReportBy)
            If lTsGroupSlide Is Nothing Then lTsGroupSlide = ConstructReportTsGroup(aTs, BFMethods.HySEPSlide, lStart, lEnd, lDA, lReportBy)
            If lTsGroupBFIStandard Is Nothing Then lTsGroupBFIStandard = ConstructReportTsGroup(aTs, BFMethods.BFIStandard, lStart, lEnd, lDA, lReportBy)
            If lTsGroupBFIModified Is Nothing Then lTsGroupBFIModified = ConstructReportTsGroup(aTs, BFMethods.BFIModified, lStart, lEnd, lDA, lReportBy)
            If lTsGroupBFLOW Is Nothing Then lTsGroupBFLOW = ConstructReportTsGroup(aTs, BFMethods.BFLOW, lStart, lEnd, lDA, lReportBy)
            If lTsGroupTwoPRDF Is Nothing Then lTsGroupTwoPRDF = ConstructReportTsGroup(aTs, BFMethods.TwoPRDF, lStart, lEnd, lDA, lReportBy)
        End If

        Dim lConversionFactor As Double = pUADepth / lDA

        Dim lTsFlowDaily As atcTimeseries = Nothing
        If args IsNot Nothing Then
            lTsFlowDaily = aTs
        Else
            lTsFlowDaily = SubsetByDate(aTs, lStart, lEnd, Nothing)
        End If

        Dim lTsFlowDailyDepth As atcTimeseries = lTsFlowDaily * lConversionFactor

        Dim lTsFlowMonthly As atcTimeseries = Aggregate(lTsFlowDaily, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lTsFlowMonthlySum As atcTimeseries = Aggregate(lTsFlowDaily, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
        Dim lTsFlowMonthlyDepth As atcTimeseries = lTsFlowMonthlySum * lConversionFactor

        Dim lTsFlowDailyBnd As atcTimeseries = Nothing
        Dim lTsFlowYearly As atcTimeseries = Nothing
        Dim lTsFlowYearlySum As atcTimeseries = Nothing
        Dim lTsFlowYearlyDepth As atcTimeseries = Nothing
        If lTsFlowDaily.numValues >= 365 Then
            If Not String.IsNullOrEmpty(lReportBy) AndAlso lReportBy.ToLower() = "water" Then
                lTsFlowDailyBnd = SubsetByDateBoundary(lTsFlowDaily, 10, 1, Nothing)
                If lTsFlowDailyBnd.Values IsNot Nothing Then
                    ' here test if user tries to force water year bound on one exact calendar year
                    lTsFlowMonthly = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    lTsFlowMonthlySum = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    lTsFlowMonthlyDepth = lTsFlowMonthlySum * lConversionFactor
                Else
                    ' cannot report a calendar year dataset in water year
                    ' don't try to re-calculate monthly sum and depth here
                    ' try to use the monthly values over original period of record
                    ' Exit Sub
                End If
            Else
                lTsFlowDailyBnd = SubsetByDateBoundary(lTsFlowDaily, 1, 1, Nothing)
            End If
            If lTsFlowDailyBnd IsNot Nothing AndAlso lTsFlowDailyBnd.Values IsNot Nothing Then
                lTsFlowYearly = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                lTsFlowYearlySum = Aggregate(lTsFlowDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                lTsFlowYearlyDepth = lTsFlowYearlySum * lConversionFactor
            End If
        End If

        Dim lTsGroupStreamFlow As New atcCollection
        With lTsGroupStreamFlow
            'If Not String.IsNullOrEmpty(lReportBy) AndAlso lReportBy.ToLower() = "water" Then
            '    lTsFlowDaily = SubsetByDateBoundary(lTsFlowDaily, 10, 1, Nothing)
            '    If lTsFlowDaily.Values IsNot Nothing Then
            '        lTsFlowDailyDepth = lTsFlowDaily * lConversionFactor
            '    Else
            '        lTsFlowDailyDepth = Nothing
            '    End If
            'End If
            If lTsFlowDailyBnd IsNot Nothing AndAlso lTsFlowDailyBnd.Values IsNot Nothing Then
                .Add("RateDaily", lTsFlowDailyBnd)
                lTsFlowDailyDepth = lTsFlowDailyBnd * lConversionFactor
                .Add("DepthDaily", lTsFlowDailyDepth)
                If lTsFlowDaily.Dates.Value(1) <> lTsFlowDailyBnd.Dates.Value(1) Then
                    .Add("RateDailyOriginal", lTsFlowDaily)
                    .Add("DepthDailyOriginal", lTsFlowDaily * lConversionFactor)
                End If
            Else
                .Add("RateDaily", lTsFlowDaily)
                .Add("DepthDaily", lTsFlowDailyDepth)
            End If
            .Add("RateMonthly", lTsFlowMonthly)
            .Add("DepthMonthly", lTsFlowMonthlyDepth)
            .Add("RateYearly", lTsFlowYearly)
            .Add("DepthYearly", lTsFlowYearlyDepth)
        End With

        Dim lfnRoot As String = OutputFilenameRoot
        If args IsNot Nothing AndAlso Not String.IsNullOrEmpty(lReportFileSuffix) Then
            Dim lForFullSpan As Boolean = args.GetValue("ForFullSpan", False)
            If lForFullSpan Then
                'Adjust to remove period from root name for the full span report files
                Dim lPattern As String = "_period_(\d)+"
                Dim rgx = New Regex(lPattern)
                Dim replacement = ""
                lfnRoot = rgx.Replace(lfnRoot, replacement)
                lfnRoot = lfnRoot.Replace("_period_", "")
            End If
            lfnRoot &= "_" & lReportFileSuffix
        End If
        Dim lFileDailySum As String = IO.Path.Combine(OutputDir, lfnRoot & "_Daily.csv")
        Dim lFileMonthlySum As String = IO.Path.Combine(OutputDir, lfnRoot & "_Monthly.csv")
        Dim lFileYearlySum As String = IO.Path.Combine(OutputDir, lfnRoot & "_Yearly.csv")
        Dim lFileDuration As String = IO.Path.Combine(OutputDir, lfnRoot & "_Duration.csv")

        Dim lMethodNames As New atcCollection()
        With lMethodNames
            .Add(BFMethods.PART, "PART")
            .Add(BFMethods.HySEPFixed, "HySEP-Fixed")
            .Add(BFMethods.HySEPLocMin, "HySEP-LocMin")
            .Add(BFMethods.HySEPSlide, "HySEP-Slide")
            .Add(BFMethods.BFIStandard, "BFIStandard")
            .Add(BFMethods.BFIModified, "BFIModified")
            .Add(BFMethods.BFLOW, "BFLOW")
            .Add(BFMethods.TwoPRDF, "TwoPRDF")
        End With

        'header for data dump file
        Dim lColumnsPerMethod As Integer = 6
        Dim lNumColumns As Integer = 4 + MethodsLastDone.Count * lColumnsPerMethod
        Dim lTableHeader As New atcTableDelimited
        lTableHeader.Delimiter = ", "
        lTableHeader.NumFields = lNumColumns

        'Need to rid of lingering results from a previous analysis run
        'based on the BF method(s) that user chose for the analysis JUST BEING DONE
        If args Is Nothing Then
            If Not MethodsLastDone.Contains(BFMethods.HySEPFixed) Then lTsGroupFixed.Clear()
            If Not MethodsLastDone.Contains(BFMethods.HySEPLocMin) Then lTsGroupLocMin.Clear()
            If Not MethodsLastDone.Contains(BFMethods.HySEPSlide) Then lTsGroupSlide.Clear()
            If Not MethodsLastDone.Contains(BFMethods.PART) Then lTsGroupPart.Clear()
            If Not MethodsLastDone.Contains(BFMethods.BFIStandard) Then lTsGroupBFIStandard.Clear()
            If Not MethodsLastDone.Contains(BFMethods.BFIModified) Then lTsGroupBFIModified.Clear()
            If Not MethodsLastDone.Contains(BFMethods.BFLOW) Then lTsGroupBFLOW.Clear()
            If Not MethodsLastDone.Contains(BFMethods.TwoPRDF) Then lTsGroupTwoPRDF.Clear()
        End If

        Dim lTableToReport As atcTableDelimited = ASCIICommonTable(lTsGroupStreamFlow,
                                                                   lTsGroupPart,
                                                                   lTsGroupFixed,
                                                                   lTsGroupLocMin,
                                                                   lTsGroupSlide,
                                                                   lTsGroupBFIStandard,
                                                                   lTsGroupBFIModified,
                                                                   lTsGroupBFLOW,
                                                                   lTsGroupTwoPRDF,
                                                                   "Daily")
        Dim lMethodLabelColumnStart As Integer = 7
        Dim lConsLabelColumnStart As Integer = 5
        Dim lUnitsLabelColumnStarts As Integer = 5
        With lTableHeader
            For lRow As Integer = 1 To 3
                lTableHeader.CurrentRecord = lRow
                If lRow = 2 Then
                    .Value(3) = "Streamflow"
                ElseIf lRow = 3 Then
                    .Value(1) = "Day"
                    .Value(2) = "Date"
                    .Value(3) = "CFS"
                    .Value(4) = "In"
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
                            .Value(lConsLabelColumnStart + 5) = "BFI"
                            lConsLabelColumnStart += lColumnsPerMethod
                        Case 3
                            .Value(lUnitsLabelColumnStarts) = "CFS"
                            .Value(lUnitsLabelColumnStarts + 1) = "In"
                            .Value(lUnitsLabelColumnStarts + 2) = "CFS"
                            .Value(lUnitsLabelColumnStarts + 3) = "In"
                            .Value(lUnitsLabelColumnStarts + 4) = "(%)"
                            .Value(lUnitsLabelColumnStarts + 5) = "(--)"
                            lUnitsLabelColumnStarts += lColumnsPerMethod
                    End Select
                Next
            Next
        End With

        Dim lLocation As String = aTs.Attributes.GetValue("Location", "Unknown")
        Dim lStaNam As String = aTs.Attributes.GetValue("STANAM", "Unknown")
        Dim lTitleLine1 As String = "Groundwater Toolbox daily output For hydrograph separation."
        Dim lTitleLine2 As String = "Station: " & lLocation & " " & lStaNam.Replace(",", " ")
        Dim lTitleLine3 As String = "Drainage area: " & DoubleToString(lDA, , "0.0") & " square miles"
        Dim lTitleLine4 As String = "(CFS: cubic feet per second; IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"
        Dim lParameterNotes As String = ""
        If args IsNot Nothing Then
            Dim lFact As Double = Double.NaN
            Dim lK1Day As Double = Double.NaN
            Dim lNDay As Double = Double.NaN
            Dim lalpha As Double = Double.NaN
            Dim lRC As Double = Double.NaN
            Dim lBFImax As Double = Double.NaN
            Dim lDF2PMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE
            Dim lBFI_Notes As String = ""
            Dim lDF1P_Notes As String = ""
            Dim lDF2P_Notes As String = ""
            With args
                Dim lMethods = .GetValue(BFInputNames.BFMethods, Nothing)
                If lMethods.Contains(BFMethods.BFIStandard) OrElse lMethods.Contains(BFMethods.BFIModified) Then
                    lNDay = .GetValue(BFInputNames.BFINDayScreen, Double.NaN) '"BFINDay"
                    'Dim lBFIYearBasis As String = aArgs.GetValue(BFInputNames.BFIReportby, "") '"BFIReportby"
                    lBFI_Notes = " " & "," & "BFI:" & ", ," & "Partition Length (N days):" & lNDay & ";"
                End If
                If lMethods.Contains(BFMethods.BFIStandard) Then
                    lFact = .GetValue(BFInputNames.BFITurnPtFrac, Double.NaN) '"BFIFrac"
                    lBFI_Notes &= " Turning Point Test Factor (F):" & lFact & ";"
                End If
                If lMethods.Contains(BFMethods.BFIModified) Then
                    lK1Day = .GetValue(BFInputNames.BFIRecessConst, Double.NaN) '"BFIK1Day"
                    lBFI_Notes &= " Daily Recession Index (K):" & lK1Day
                End If
                lBFI_Notes = lBFI_Notes.TrimEnd(";")
                If lMethods.Contains(BFMethods.BFLOW) Then
                    lalpha = .GetValue(BFInputNames.BFLOWFilter, Double.NaN)
                    lDF1P_Notes = " " & "," & "DF1Param:" & ", ," & "Filter Constant (alpha):" & lalpha
                End If
                If lMethods.Contains(BFMethods.TwoPRDF) Then
                    lRC = .GetValue(BFInputNames.TwoPRDFRC, Double.NaN)
                    lBFImax = .GetValue(BFInputNames.TwoPRDFBFImax, Double.NaN)
                    lDF2PMethod = .GetValue(BFInputNames.TwoParamEstMethod, clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE)
                    lDF2P_Notes = " " & "," & "DF2Param:" & ", ," & "Recession Constant (a):" & lRC & "; BFImax:" & lBFImax
                End If
            End With
            If Not String.IsNullOrEmpty(lBFI_Notes) OrElse Not String.IsNullOrEmpty(lDF1P_Notes) OrElse Not String.IsNullOrEmpty(lDF2P_Notes) Then
                lParameterNotes = "Parameters Specified:" & vbCrLf
                If Not String.IsNullOrEmpty(lBFI_Notes) Then
                    lParameterNotes &= lBFI_Notes & vbCrLf
                End If
                If Not String.IsNullOrEmpty(lDF1P_Notes) Then
                    lParameterNotes &= lDF1P_Notes & vbCrLf
                End If
                If Not String.IsNullOrEmpty(lDF2P_Notes) Then
                    lParameterNotes &= lDF2P_Notes & vbCrLf
                End If
            End If
        End If

        Dim lSW As New IO.StreamWriter(lFileDailySum, False)
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        If Not String.IsNullOrEmpty(lParameterNotes) Then
            lSW.Write(lParameterNotes)
        End If
        lSW.WriteLine(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing
        If args IsNot Nothing AndAlso args.ContainsAttribute("Specification") Then
            args.SetValue("Specification", lFileDailySum)
        End If

        lTableToReport.ClearData()
        lTableToReport = ASCIICommonTable(lTsGroupStreamFlow,
                                          lTsGroupPart,
                                          lTsGroupFixed,
                                          lTsGroupLocMin,
                                          lTsGroupSlide,
                                          lTsGroupBFIStandard,
                                          lTsGroupBFIModified,
                                          lTsGroupBFLOW,
                                          lTsGroupTwoPRDF,
                                          "Monthly")
        lTableHeader.CurrentRecord = 3
        lTableHeader.Value(1) = "Month"
        lSW = New IO.StreamWriter(lFileMonthlySum, False)
        lTitleLine1 = lTitleLine1.Replace("daily", "monthly")
        lTitleLine4 = "(CFS: average flow for the month (cubic feet per second); IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        If Not String.IsNullOrEmpty(lParameterNotes) Then
            lSW.Write(lParameterNotes)
        End If
        lSW.Write(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        lTableToReport.ClearData()
        lTableToReport = ASCIICommonTable(lTsGroupStreamFlow,
                                          lTsGroupPart,
                                          lTsGroupFixed,
                                          lTsGroupLocMin,
                                          lTsGroupSlide,
                                          lTsGroupBFIStandard,
                                          lTsGroupBFIModified,
                                          lTsGroupBFLOW,
                                          lTsGroupTwoPRDF,
                                          "Yearly",
                                          lReportBy)
        lTableHeader.CurrentRecord = 3
        lTableHeader.Value(1) = "Year"
        If Not String.IsNullOrEmpty(lReportBy) AndAlso lReportBy.ToLower() = "water" Then
            lTableHeader.Value(2) = "Water Year"
        Else
            lTableHeader.Value(2) = "Calendar Year"
        End If
        lSW = New IO.StreamWriter(lFileYearlySum, False)
        If Not String.IsNullOrEmpty(lReportBy) AndAlso lReportBy.ToLower() = "water" Then
            lTitleLine1 = "Groundwater Toolbox annual output for hydrograph separation (water year October 1-September 30)"
        Else
            lTitleLine1 = "Groundwater Toolbox annual output for hydrograph separation (calendar year January 1-December 31)"
        End If
        lTitleLine4 = "(CFS: average flow for the year (cubic feet per second); IN: flow per drainage area (inches); BFP: Base-Flow Percentage (ratio of base-flow to streamflow multiplied by 100)"
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3) : lSW.WriteLine(lTitleLine4)
        If Not String.IsNullOrEmpty(lParameterNotes) Then
            lSW.Write(lParameterNotes)
        End If
        lSW.WriteLine(lTableHeader.ToString)
        lSW.WriteLine(lTableToReport.ToString)
        lSW.Flush()
        lSW.Close()
        lSW = Nothing

        If args IsNot Nothing AndAlso args.GetValue(BFInputNames.FullSpanDuration, False) Then
            'new behavior: if continuous full time span dataset or if user chooses to do it, then allow it
        Else
            'new behavior above
            'It is fine to keep the Duration.csv files for the individual periods (chunks) of the record, however.
            'this change only applies to dataset that has gaps
            Exit Sub
        End If
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

        Dim lDates(5) As Integer
        lTitleLine1 = "Groundwater Toolbox daily output for hydrograph separation."
        lTitleLine2 = "Station: " & lLocation & " " & lStaNam.Replace(",", " ")
        lTitleLine3 = "Drainage area: " & DoubleToString(lDA, , "0.0") & " square miles"
        J2Date(lStart, lDates)
        Dim lStartStr As String = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
        J2Date(lEnd, lDates)
        timcnv(lDates)
        Dim lEndStr As String = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
        'lTitleLine4 = "Period of analysis: " & DumpDate(lStart) & " to " & DumpDate(lEnd)
        lTitleLine4 = "Period of analysis: " & lStartStr & " to " & lEndStr
        Dim lTitleLine5 As String = "Percent exceedence: Percentage of time flow was equaled or exceeded"
        Dim lTitleLine6 As String = "Flow: in cubic feet per second"

        lSW = New System.IO.StreamWriter(lFileDuration, False)
        lSW.WriteLine(lTitleLine1) : lSW.WriteLine(lTitleLine2) : lSW.WriteLine(lTitleLine3)
        lSW.WriteLine(lTitleLine4) : lSW.WriteLine(lTitleLine5) : lSW.WriteLine(lTitleLine6)
        If Not String.IsNullOrEmpty(lParameterNotes) Then
            lSW.Write(lParameterNotes)
        End If

        lSW.WriteLine("****   Daily Duration Table  ****")
        lTableToReport = ASCIICommonDurationTable(lTsGroupStreamFlow,
                                                  lTsGroupPart,
                                                  lTsGroupFixed,
                                                  lTsGroupLocMin,
                                                  lTsGroupSlide,
                                                  lTsGroupBFIStandard,
                                                  lTsGroupBFIModified,
                                                  lTsGroupBFLOW,
                                                  lTsGroupTwoPRDF,
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

    Public Function ASCIIOriginal(ByVal aStreamFlowTs As atcTimeseries, ByVal aMethod As BFMethods) As Boolean

        Dim lSpecification As String = ""
        Dim lMethodName As String = ""
        Select Case aMethod
            Case BFMethods.HySEPFixed : lMethodName = "HySEPFixed"
            Case BFMethods.HySEPLocMin : lMethodName = "HySEPLocMin"
            Case BFMethods.HySEPSlide : lMethodName = "HySEPSlide"
            Case BFMethods.PART : lMethodName = "Part"
            Case BFMethods.BFIStandard : lMethodName = "BFIStandard"
            Case BFMethods.BFIModified : lMethodName = "BFIModified"
            Case BFMethods.BFLOW : lMethodName = "BFLOW"
            Case BFMethods.TwoPRDF : lMethodName = "TwoPRDF"
        End Select
        'Write original HySEP and PART's output files
        If aMethod = BFMethods.HySEPFixed OrElse
           aMethod = BFMethods.HySEPLocMin OrElse
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
            ASCIIBFIDaily(aStreamFlowTs, lFilename, aMethod) 'lMethodName

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".bfi")
            ASCIIBFI(aStreamFlowTs, lFilename, aMethod) 'lMethodName
            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & ".tp")
            ASCIIBFITp(aStreamFlowTs, lFilename, aMethod) 'lMethodName
        ElseIf aMethod = BFMethods.BFLOW Then
            Dim loc As String = aStreamFlowTs.Attributes.GetValue("Location", "")
            Dim lFilename As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & "_" & loc & ".dat")
            ASCIIBFLOWDat(aStreamFlowTs, lFilename, aMethod) 'lMethodName

            lFilename = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_" & lMethodName & "_" & loc & ".out")
            ASCIIBFLOWDailyOut(aStreamFlowTs, lFilename) 'lMethodName
        ElseIf aMethod = BFMethods.TwoPRDF Then
            Dim loc As String = aStreamFlowTs.Attributes.GetValue("Location", "")
            Dim lBFITXTFile As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_TwoPRDF_bfi_" & loc & ".txt")
            ASCIITwoPRDFDat(lBFITXTFile, aStreamFlowTs)
            Dim lbaseflowTXTFile As String = IO.Path.Combine(OutputDir, OutputFilenameRoot & "_TwoPRDF_baseflow_" & loc & ".txt")
            ASCIITwoPRDFDailyOut(lbaseflowTXTFile, aStreamFlowTs)
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

    Private Sub SetNaNTimeseries(ByVal aTser As atcTimeseries)
        If aTser Is Nothing Then Exit Sub
        For I As Integer = 1 To aTser.numValues
            If aTser.Value(I) < 0 Then
                aTser.Value(I) = Double.NaN
            End If
        Next
    End Sub

    Public Function ASCIICommonDurationTable(ByVal aTsGroupStreamFlow As atcCollection,
                                 ByVal aTsGroupPart As atcCollection,
                                 ByVal aTsGroupFixed As atcCollection,
                                 ByVal aTsGroupLocMin As atcCollection,
                                 ByVal aTsGroupSlide As atcCollection,
                                 ByVal aTsGroupBFIStandard As atcCollection,
                                 ByVal aTsGroupBFIModified As atcCollection,
                                 ByVal aTsGroupBFLOW As atcCollection,
                                 ByVal aTsGroupTwoPRDF As atcCollection,
                                 ByVal ATStep As String) As atcTableDelimited

        Dim lTsFlow As atcTimeseries = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep)
        SetNaNTimeseries(lTsFlow)
        Dim lTsBFPart As atcTimeseries = Nothing
        Dim lTsROPart As atcTimeseries = Nothing
        Dim lExceedanceListing As New atcCollection
        Dim lResult As atcCollection = ConstructExceedanceListing(lTsFlow, "Observed", "StreamFlow")
        lExceedanceListing.AddRange(lResult.Keys, lResult)

        If aTsGroupPart.Count > 0 Then
            lTsBFPart = aTsGroupPart.ItemByKey("Rate" & ATStep)
            lTsROPart = lTsFlow - lTsBFPart
            SetNaNTimeseries(lTsBFPart)
            SetNaNTimeseries(lTsROPart)
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
            SetNaNTimeseries(lTsBFFixed)
            SetNaNTimeseries(lTsROFixed)
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
            SetNaNTimeseries(lTsBFLocMin)
            SetNaNTimeseries(lTsROLocMin)
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
            SetNaNTimeseries(lTsBFSlide)
            SetNaNTimeseries(lTsROSlide)
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
            SetNaNTimeseries(lTsBFBFIStandard)
            SetNaNTimeseries(lTsROBFIStandard)
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
            SetNaNTimeseries(lTsBFBFIModified)
            SetNaNTimeseries(lTsROBFIModified)
            lResult = ConstructExceedanceListing(lTsBFBFIModified, "BFIModified", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROBFIModified, "BFIModified", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If
        Dim lTsBFBFLOW As atcTimeseries = Nothing
        Dim lTsROBFLOW As atcTimeseries = Nothing
        If aTsGroupBFLOW.Count > 0 Then
            lTsBFBFLOW = aTsGroupBFLOW.ItemByKey("Rate" & ATStep)
            lTsROBFLOW = lTsFlow - lTsBFBFLOW
            SetNaNTimeseries(lTsBFBFLOW)
            SetNaNTimeseries(lTsROBFLOW)
            lResult = ConstructExceedanceListing(lTsBFBFLOW, "BFLOW", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROBFLOW, "BFLOW", "Runoff")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
        End If
        Dim lTsBFTwoPRDF As atcTimeseries = Nothing
        Dim lTsROTwoPRDF As atcTimeseries = Nothing
        If aTsGroupTwoPRDF.Count > 0 Then
            lTsBFTwoPRDF = aTsGroupTwoPRDF.ItemByKey("Rate" & ATStep)
            lTsROTwoPRDF = lTsFlow - lTsBFTwoPRDF
            SetNaNTimeseries(lTsBFTwoPRDF)
            SetNaNTimeseries(lTsROTwoPRDF)
            lResult = ConstructExceedanceListing(lTsBFTwoPRDF, "TwoPRDF", "Baseflow")
            lExceedanceListing.AddRange(lResult.Keys, lResult)
            lResult = ConstructExceedanceListing(lTsROTwoPRDF, "TwoPRDF", "Runoff")
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

                If aTsGroupBFLOW.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_BFLOW_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_BFLOW_Runoff")(I)
                    lLastColumn += lColumnsPerMethod
                End If

                If aTsGroupTwoPRDF.Count > 0 Then
                    .Value(lLastColumn + 1) = lExceedanceListing.ItemByKey("Y_TwoPRDF_Baseflow")(I)
                    .Value(lLastColumn + 2) = lExceedanceListing.ItemByKey("Y_TwoPRDF_Runoff")(I)
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

    Private Function ASCIICommonTable(ByVal aTsGroupStreamFlow As atcCollection,
                                 ByVal aTsGroupPart As atcCollection,
                                 ByVal aTsGroupFixed As atcCollection,
                                 ByVal aTsGroupLocMin As atcCollection,
                                 ByVal aTsGroupSlide As atcCollection,
                                 ByVal aTsGroupBFIStandard As atcCollection,
                                 ByVal aTsGroupBFIModified As atcCollection,
                                 ByVal aTsGroupBFLOW As atcCollection,
                                 ByVal aTsGroupTwoPRDF As atcCollection,
                                 ByVal ATStep As String,
                                 Optional aReportBy As String = "calendar") As atcTableDelimited
        'set up table
        Dim lNumColumnsPerMethod As Integer = 6
        Dim lNumColumns As Integer = 4 + MethodsLastDone.Count * lNumColumnsPerMethod
        Dim lTableBody As New atcTableDelimited
        lTableBody.Delimiter = ","
        lTableBody.NumFields = lNumColumns
        lTableBody.CurrentRecord = 1

        Dim lDate(5) As Integer

        Dim lFlowVal As Double
        Dim lBF As Double
        Dim lBFDepth As Double
        Dim lBFPct As Double = 0.0
        Dim lBFTser As atcTimeseries = Nothing
        Dim lBFDepthTser As atcTimeseries = Nothing

        Dim lTsFlow As atcTimeseries = Nothing
        Dim lTsFlowDepth As atcTimeseries = Nothing
        If ATStep = "Daily" Then
            If aTsGroupStreamFlow.Keys.Contains("Rate" & ATStep & "Original") Then
                lTsFlow = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep & "Original")
                lTsFlowDepth = aTsGroupStreamFlow.ItemByKey("Depth" & ATStep & "Original")
            Else
                lTsFlow = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep)
                lTsFlowDepth = aTsGroupStreamFlow.ItemByKey("Depth" & ATStep)
            End If
        Else
            lTsFlow = aTsGroupStreamFlow.ItemByKey("Rate" & ATStep)
            lTsFlowDepth = aTsGroupStreamFlow.ItemByKey("Depth" & ATStep)
        End If
        Dim lFlowStartDate As Double = -99
        Dim lFlowEndDate As Double = -99
        If lTsFlow IsNot Nothing Then
            lFlowStartDate = lTsFlow.Dates.Value(0)
            lFlowEndDate = lTsFlow.Dates.Value(lTsFlow.numValues)
        Else
            Return lTableBody
        End If
        Dim lDA As Double = lTsFlow.Attributes.GetValue("Drainage Area", -1.0)
        If aTsGroupBFIStandard.Count > 0 Then
            AdjustDates(aTsGroupBFIStandard, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupBFIModified.Count > 0 Then
            AdjustDates(aTsGroupBFIModified, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupPart.Count > 0 Then
            AdjustDates(aTsGroupPart, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupFixed.Count > 0 Then
            AdjustDates(aTsGroupFixed, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupSlide.Count > 0 Then
            AdjustDates(aTsGroupSlide, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupLocMin.Count > 0 Then
            AdjustDates(aTsGroupLocMin, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupBFLOW.Count > 0 Then
            AdjustDates(aTsGroupBFLOW, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If aTsGroupTwoPRDF.Count > 0 Then
            AdjustDates(aTsGroupTwoPRDF, lTsFlow, ATStep, lFlowStartDate, lFlowEndDate, lDA)
        End If
        If lTsFlow Is Nothing Then
            Return lTableBody
        End If
        For I As Integer = 1 To lTsFlow.numValues
            ''New directive, 4/6/2016, do show all months, incomplete or not
            'If ATStep = "Monthly" Then
            '    If ASCIICommonTableSkipOneRow(aTsGroupPart,
            '                     aTsGroupFixed,
            '                     aTsGroupLocMin,
            '                     aTsGroupSlide,
            '                     aTsGroupBFIStandard,
            '                     aTsGroupBFIModified,
            '                     ATStep,
            '                     I) Then
            '        Continue For
            '    End If
            'End If
            J2Date(lTsFlow.Dates.Value(I - 1), lDate)
            With lTableBody
                .Value(1) = I
                Select Case ATStep
                    Case "Daily" : .Value(2) = lDate(0) & "-" & lDate(1).ToString.PadLeft(2, "0") & "-" & lDate(2).ToString.PadLeft(2, "0")
                    Case "Monthly" : .Value(2) = lDate(1).ToString.PadLeft(2, "0") & "-" & lDate(0)
                    Case "Yearly"
                        If Not String.IsNullOrEmpty(aReportBy) AndAlso aReportBy.ToLower() = "water" Then
                            .Value(2) = lDate(0) + 1
                        Else
                            .Value(2) = lDate(0)
                        End If
                    Case Else : .Value(2) = lDate(0) & "-" & lDate(1) & "-" & lDate(2)
                End Select

                lFlowVal = lTsFlow.Value(I)
                If Double.IsNaN(lFlowVal) OrElse lFlowVal < 0 Then
                    .Value(3) = "NA"
                    .Value(4) = "NA"
                Else
                    .Value(3) = DoubleToString(lFlowVal, , "0.00", "0.00E00")
                    .Value(4) = DoubleToString(lTsFlowDepth.Value(I), , "0.00", "0.00E00")
                End If
                Dim lLastColumn As Integer = 4
                If aTsGroupPart.Count > 0 Then
                    lBFTser = aTsGroupPart.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupPart.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If
                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod
                End If
                If aTsGroupFixed.Count > 0 Then
                    lBFTser = aTsGroupFixed.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupFixed.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod
                End If
                If aTsGroupLocMin.Count > 0 Then
                    lBFTser = aTsGroupLocMin.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupLocMin.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod
                End If
                If aTsGroupSlide.Count > 0 Then
                    lBFTser = aTsGroupSlide.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupSlide.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod
                End If
                If aTsGroupBFIStandard.Count > 0 Then
                    lBFTser = aTsGroupBFIStandard.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupBFIStandard.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod 'second to last column to have this jump
                End If
                If aTsGroupBFIModified.Count > 0 Then
                    lBFTser = aTsGroupBFIModified.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupBFIModified.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod 'second to last column to have this jump
                End If

                If aTsGroupBFLOW.Count > 0 Then
                    lBFTser = aTsGroupBFLOW.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupBFLOW.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                    lLastColumn += lNumColumnsPerMethod 'second to last column to have this jump
                End If

                If aTsGroupTwoPRDF.Count > 0 Then
                    lBFTser = aTsGroupTwoPRDF.ItemByKey("Rate" & ATStep)
                    lBFDepthTser = aTsGroupTwoPRDF.ItemByKey("Depth" & ATStep)
                    If lBFTser IsNot Nothing Then
                        lBF = lBFTser.Value(I)
                    Else
                        lBF = -99
                    End If
                    If lBFDepthTser IsNot Nothing Then
                        lBFDepth = lBFDepthTser.Value(I)
                    Else
                        lBFDepth = -99
                    End If

                    ASCIICommonTableOneRow(lTableBody, lTsFlow, lTsFlowDepth, I, ATStep, lBF, lBFDepth, lLastColumn)
                End If

                .CurrentRecord += 1
            End With
        Next
        Return lTableBody
    End Function

    Private Function ASCIICommonTableSkipOneRow(ByVal aTsGroupPart As atcCollection,
                                 ByVal aTsGroupFixed As atcCollection,
                                 ByVal aTsGroupLocMin As atcCollection,
                                 ByVal aTsGroupSlide As atcCollection,
                                 ByVal aTsGroupBFIStandard As atcCollection,
                                 ByVal aTsGroupBFIModified As atcCollection,
                                 ByVal ATStep As String,
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

    Private Sub ASCIICommonTableOneRow(ByVal aTable As atcTableDelimited,
                                       ByVal aTSFlow As atcTimeseries,
                                       ByVal aTSFlowDepth As atcTimeseries,
                                       ByVal aInd As Integer,
                                       ByVal ATStep As String,
                                       ByVal aBF As Double,
                                       ByVal aBFDepth As Double,
                                       ByVal aLastColumn As Integer)
        Dim lRO, lRODepth, lBFPct As Double
        Dim lValueNA As String = "NA"
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
                ''For incomplete Monthly timestep, simply put up blank cells
                '.Value(aLastColumn + 1) = ""
                '.Value(aLastColumn + 2) = ""
                '.Value(aLastColumn + 3) = ""
                '.Value(aLastColumn + 4) = ""
                '.Value(aLastColumn + 5) = ""
                '.Value(aLastColumn + 6) = ""
                'For incomplete Monthly timestep, new order is to put up just "NA"
                .Value(aLastColumn + 1) = lValueNA
                .Value(aLastColumn + 2) = lValueNA
                .Value(aLastColumn + 3) = lValueNA
                .Value(aLastColumn + 4) = lValueNA
                .Value(aLastColumn + 5) = lValueNA
                .Value(aLastColumn + 6) = lValueNA
            Else
                If Double.IsNaN(aBF) OrElse Double.IsNaN(aBFDepth) OrElse aBF < 0 OrElse aBFDepth < 0 Then
                    .Value(aLastColumn + 1) = lValueNA
                    .Value(aLastColumn + 2) = lValueNA
                    .Value(aLastColumn + 3) = lValueNA
                    .Value(aLastColumn + 4) = lValueNA
                    .Value(aLastColumn + 5) = lValueNA
                    .Value(aLastColumn + 6) = lValueNA
                Else
                    .Value(aLastColumn + 1) = DoubleToString(aBF, , "0.00", "0.00E00")
                    .Value(aLastColumn + 2) = DoubleToString(aBFDepth, , "0.00", "0.00E00")
                    .Value(aLastColumn + 3) = DoubleToString(lRO, , "0.00", "0.00E00")
                    .Value(aLastColumn + 4) = DoubleToString(lRODepth, , "0.00", "0.00E00")
                    .Value(aLastColumn + 5) = DoubleToString(lBFPct, , "0.0")
                    .Value(aLastColumn + 6) = DoubleToString(lBFPct / 100, , "0.0000")
                End If
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
    Private Sub AdjustDates(ByRef aTsGroupPerMethod As atcCollection, ByVal aTsFlow As atcTimeseries, ByVal aTStep As String, ByVal aFlowStartDate As Double, ByVal aFlowEndDate As Double, ByVal aDA As Double)
        Dim lTsBFTemp As atcTimeseries = aTsGroupPerMethod.ItemByKey("Rate" & aTStep)
        Dim lTsBFDepthTemp As atcTimeseries = aTsGroupPerMethod.ItemByKey("Depth" & aTStep)
        If lTsBFTemp IsNot Nothing AndAlso (lTsBFTemp.Dates.Value(0) <> aFlowStartDate OrElse lTsBFTemp.Dates.Value(lTsBFTemp.numValues) <> aFlowEndDate) Then
            If aTStep.ToLower() = "yearly" Then
                Dim lTsDaily As atcTimeseries = aTsGroupPerMethod.ItemByKey("RateDaily")
                Dim lTsDailySubset As atcTimeseries = SubsetByDate(lTsDaily, aFlowStartDate, aFlowEndDate, Nothing)
                aTsGroupPerMethod.ItemByKey("Rate" & aTStep) = Aggregate(lTsDailySubset, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                Dim lTsDailySubsetSumDiv As atcTimeseries = Aggregate(lTsDailySubset, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                Dim lCF As Double = 1.0
                If aDA > 0 Then
                    lCF = pUADepth / aDA
                Else
                    aDA = lTsDaily.Attributes.GetValue("Drainage Area", -1.0)
                    If aDA > 0 Then
                        lCF = pUADepth / aDA
                    Else
                        lCF = 1.0
                    End If
                End If
                aTsGroupPerMethod.ItemByKey("Depth" & aTStep) = lTsDailySubsetSumDiv * lCF
                lTsDailySubset.Clear()
                lTsDailySubset = Nothing
            Else
                aTsGroupPerMethod.ItemByKey("Rate" & aTStep) = SubsetByDate(lTsBFTemp, aFlowStartDate, aFlowEndDate, Nothing)
                aTsGroupPerMethod.ItemByKey("Depth" & aTStep) = SubsetByDate(lTsBFDepthTemp, aFlowStartDate, aFlowEndDate, Nothing)
            End If
        End If
        If lTsBFDepthTemp IsNot Nothing AndAlso (lTsBFDepthTemp.Dates.Value(0) <> aFlowStartDate OrElse lTsBFDepthTemp.Dates.Value(lTsBFDepthTemp.numValues) <> aFlowEndDate) Then
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
    Private Function ConstructReportTsGroup(ByVal aTs As atcTimeseries, ByVal aMethod As BFMethods,
                                            Optional ByRef aStart As Double = 0.0,
                                            Optional ByRef aEnd As Double = 0.0,
                                            Optional ByRef aDA As Double = 0.0,
                                            Optional ByVal aReportBy As String = "Calendar") As atcCollection

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

                Dim lNumOfDays As Integer = lTsBFToReportPartDaily.numValues

                Dim lTsBFToReportPartDailyBnd As atcTimeseries = Nothing
                Dim lTsBFToReportPartYearly As atcTimeseries = Nothing
                Dim lTsBFToReportPartYearlySum As atcTimeseries = Nothing
                Dim lTsBFToReportPartYearlyDepth As atcTimeseries = Nothing
                If lNumOfDays > JulianYear Then
                    If Not String.IsNullOrEmpty(aReportBy) AndAlso aReportBy.ToLower() = "water" Then
                        lTsBFToReportPartDailyBnd = SubsetByDateBoundary(lTsBFToReportPartDaily, 10, 1, Nothing)
                        lTsBFToReportPartMonthly = SubsetByDateBoundary(lTsBFToReportPartMonthly, 10, 1, Nothing)
                        lTsBFToReportPartMonthlyDepth = SubsetByDateBoundary(lTsBFToReportPartMonthlyDepth, 10, 1, Nothing)
                    Else
                        lTsBFToReportPartDailyBnd = SubsetByDateBoundary(lTsBFToReportPartDaily, 1, 1, Nothing)
                        'don't touch monthly if not report by water year
                        'lTsBFToReportPartMonthly = SubsetByDateBoundary(lTsBFToReportPartMonthly, 1, 1, Nothing)
                        'lTsBFToReportPartMonthlyDepth = SubsetByDateBoundary(lTsBFToReportPartMonthlyDepth, 1, 1, Nothing)
                    End If
                    If lTsBFToReportPartDailyBnd IsNot Nothing AndAlso lTsBFToReportPartDailyBnd.Values IsNot Nothing Then
                        lTsBFToReportPartYearly = Aggregate(lTsBFToReportPartDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                        lTsBFToReportPartYearlySum = Aggregate(lTsBFToReportPartDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        lTsBFToReportPartYearlyDepth = lTsBFToReportPartYearlySum * lConversionFactor
                    End If
                End If

                'lTsBFToReportPartDaily.Attributes.SetValue(lReportColumnAttributeName, "RateDaily")
                'lTsBFToReportPartDailyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthDaily")
                'lTsBFToReportPartMonthly.Attributes.SetValue(lReportColumnAttributeName, "RateMonthly")
                'lTsBFToReportPartMonthlyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthMonthly")
                'lTsBFToReportPartYearly.Attributes.SetValue(lReportColumnAttributeName, "RateYearly")
                'lTsBFToReportPartYearlyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthYearly")
                With lTsGroupToReport
                    'If Not String.IsNullOrEmpty(aReportBy) AndAlso aReportBy.ToLower() = "water" AndAlso lNumOfDays > JulianYear Then
                    '    .Add("RateDaily", lTsBFToReportPartDailyBnd)
                    'Else
                    .Add("RateDaily", lTsBFToReportPartDaily)
                    'End If
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

                If aMethod = BFMethods.BFIStandard OrElse aMethod = BFMethods.BFIModified Then
                    Dim lStartOrig As Double = lTsDaily.Attributes.GetValue("OriginalStart", -1)
                    Dim lEndOrig As Double = lTsDaily.Attributes.GetValue("OriginalEnd", -1)
                    If lStartOrig > 0 AndAlso lEndOrig > 0 Then
                        If lStartOrig <> lTsDaily.Dates.Value(0) AndAlso lStartOrig > lTsDaily.Dates.Value(0) Then
                            lTsDaily = SubsetByDate(lTsDaily, lStartOrig, lEndOrig, Nothing)
                        End If
                    End If
                End If

                Dim lTsDailyDepth As atcTimeseries = Nothing
                Dim lTsMon As atcTimeseries = Nothing
                Dim lTsMonSum As atcTimeseries = Nothing
                Dim lTsMonDepth As atcTimeseries = Nothing
                Dim lTsYear As atcTimeseries = Nothing
                Dim lTsYearSum As atcTimeseries = Nothing
                Dim lTsYearDepth As atcTimeseries = Nothing
                Dim lNumOfDays As Integer = lTsDaily.numValues
                Dim lTsDailyBnd As atcTimeseries = Nothing 'SubsetByDateBoundary(lTsDaily, 1, 1, Nothing)
                If lNumOfDays >= 365 Then
                    If Not String.IsNullOrEmpty(aReportBy) AndAlso aReportBy.ToLower() = "water" Then
                        lTsDailyBnd = SubsetByDateBoundary(lTsDaily, 10, 1, Nothing)
                    Else
                        'lTsDailyBnd = SubsetByDateBoundary(lTsDaily, 1, 1, Nothing)
                        lTsDailyBnd = lTsDaily
                    End If
                Else
                    lTsDailyBnd = lTsDaily
                End If

                If lDA > 0 Then
                    lConversionFactor = pUADepth / lDA
                    lTsDailyDepth = lTsDailyBnd * lConversionFactor
                    lTsMon = Aggregate(lTsDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    lTsMonSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    lTsMonDepth = lTsMonSum * lConversionFactor

                    'Adjust for partial months based on the beginning and end of daily time series for HySEP methods
                    If aMethod = BFMethods.HySEPFixed OrElse aMethod = BFMethods.HySEPLocMin OrElse aMethod = BFMethods.HySEPSlide Then
                        If lTsMon IsNot Nothing AndAlso lTsMon.Values IsNot Nothing AndAlso lTsMon.numValues > 0 Then
                            Dim lDailyDate(5) As Integer
                            J2Date(lTsDailyBnd.Dates.Value(0), lDailyDate)
                            If lDailyDate(2) <> 1 Then
                                lTsMon.Value(1) = -99.99
                                lTsMonDepth.Value(1) = -99.99
                            End If

                            ''doesn't seem to need adjusting the end
                            'J2Date(lTsDaily.Dates.Value(lTsDaily.numValues), lDailyDate)
                            'timcnv(lDailyDate)
                            'If lDailyDate(2) <> modDate.DayMon(lDailyDate(0), lDailyDate(1)) Then
                            '    lTsMon.Value(lTsMon.numValues) = -99.99
                            '    lTsMonDepth.Value(lTsMonDepth.numValues) = -99.99
                            'End If
                        End If
                    End If

                    If lNumOfDays >= 365 Then
                        If lTsDailyBnd IsNot Nothing AndAlso lTsDailyBnd.Values IsNot Nothing Then
                            For I As Integer = 1 To lTsDailyBnd.numValues
                                If lTsDailyBnd.Value(I) < 0 Then
                                    'ensure even if 1 value out of a year is negative, then whole year is negative
                                    'can't use NaN as Aggregate will fail
                                    lTsDailyBnd.Value(I) = Double.MinValue '-999 'Double.NaN
                                End If
                            Next

                            lTsYear = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                            For I As Integer = 1 To lTsYear.numValues
                                If Double.IsNaN(lTsYear.Value(I)) OrElse lTsYear.Value(I) < 0 Then
                                    lTsYear.Value(I) = -99
                                End If
                            Next
                            lTsYearSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                            lTsYearDepth = lTsYearSum * lConversionFactor
                            For I As Integer = 1 To lTsYearDepth.numValues
                                If Double.IsNaN(lTsYearDepth.Value(I)) OrElse lTsYearDepth.Value(I) < 0 Then
                                    lTsYearDepth.Value(I) = -99
                                End If
                            Next
                        End If
                    End If
                Else
                    lTsMon = Aggregate(lTsDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    lTsMonSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    lTsMonDepth = lTsMonSum.Clone()
                    For I As Integer = 1 To lTsMonDepth.numValues
                        lTsMonDepth.Value(I) = -99
                    Next

                    If lNumOfDays >= 365 Then
                        If lTsDailyBnd IsNot Nothing AndAlso lTsDailyBnd.Values IsNot Nothing Then
                            For I As Integer = 1 To lTsDailyBnd.numValues
                                If lTsDailyBnd.Value(I) < 0 Then
                                    'ensure even if 1 value out of a year is negative, then whole year is negative
                                    'can't use NaN as Aggregate will fail
                                    lTsDailyBnd.Value(I) = Double.MinValue 'Double.NaN
                                End If
                            Next
                            lTsYear = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                            For I As Integer = 1 To lTsYear.numValues
                                If Double.IsNaN(lTsYear.Value(I)) OrElse lTsYear.Value(I) < 0 Then
                                    lTsYear.Value(I) = -99
                                End If
                            Next
                            lTsYearSum = Aggregate(lTsDailyBnd, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                            lTsYearDepth = lTsYearSum.Clone()
                            For I As Integer = 1 To lTsYearDepth.numValues
                                lTsYearDepth.Value(I) = -99
                            Next
                        End If
                    End If
                End If

                'lTsDaily.Attributes.SetValue(lReportColumnAttributeName, "RateDaily")
                'lTsDailyDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthDaily")
                'lTsMon.Attributes.SetValue(lReportColumnAttributeName, "RateMonthly")
                'lTsMonDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthMonthly")
                'lTsYear.Attributes.SetValue(lReportColumnAttributeName, "RateYearly")
                'lTsYearDepth.Attributes.SetValue(lReportColumnAttributeName, "DepthYearly")

                'Process HySep method's monthly values to set incomplete months value to -99.99
                If aMethod = BFMethods.HySEPFixed OrElse aMethod = BFMethods.HySEPLocMin OrElse aMethod = BFMethods.HySEPSlide OrElse aMethod = BFMethods.BFIModified OrElse aMethod = BFMethods.BFIStandard Then
                    Dim lAnalysisStart As Double = lTsDailyBnd.Attributes.GetValue("AnalysisStart", -99)
                    Dim lAnalysisEnd As Double = lTsDailyBnd.Attributes.GetValue("AnalysisEnd", -99)
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
                    If lTsDaily.numValues = lTsDailyDepth.numValues Then
                        .Add("DepthDaily", lTsDailyDepth)
                    Else
                        .Add("DepthDaily", lTsDaily * lConversionFactor)
                    End If
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

    Public Function ConstructGraphTsGroup(ByVal aTs As atcTimeseries, ByVal aMethod As BFMethods,
                                            Optional ByRef aStart As Double = 0.0,
                                            Optional ByRef aEnd As Double = 0.0,
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
                Dim lTsBFToReportPartMonthlyGroup As atcTimeseriesGroup = lMatchBFTsGroup.FindData("Scenario", "PartMonthlyInterpolated")
                If lTsBFToReportPartMonthlyGroup Is Nothing OrElse lTsBFToReportPartMonthlyGroup.Count = 0 Then
                    'this means the base-flow timeseries are constructed for the intermittent 
                    Dim lTsDaily As atcTimeseries = lMatchBFTsGroup(0)
                    With lTsGroupToGraph
                        .Add("RateDaily", lTsDaily)
                    End With
                Else
                    Dim lTsBFToReportPartMonthly As atcTimeseries = lTsBFToReportPartMonthlyGroup(0)
                    Dim lTsBFToReportPartDaily1 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily1")(0)
                    Dim lTsBFToReportPartDaily2 As atcTimeseries = lMatchBFTsGroup.FindData("Scenario", "PartDaily2")(0)

                    Dim linearSlope As Double = lTsBFToReportPartMonthly.Attributes.GetValue("LinearSlope")
                    Dim lTsBFToReportPartDaily As atcTimeseries = lTsBFToReportPartDaily1.Clone
                    For I As Integer = 1 To lTsBFToReportPartDaily1.numValues
                        lTsBFToReportPartDaily.Value(I) = lTsBFToReportPartDaily1.Value(I) + linearSlope * (lTsBFToReportPartDaily2.Value(I) - lTsBFToReportPartDaily1.Value(I))
                    Next
                    With lTsGroupToGraph
                        .Add("RateDaily", lTsBFToReportPartDaily)
                    End With
                End If
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
                Dim lStartDate As Double = -99
                For I As Integer = 0 To lTs.Dates.numValues
                    If Not Double.IsNaN(lTs.Dates.Value(I)) Then
                        lStartDate = lTs.Dates.Value(I)
                        Exit For
                    End If
                Next
                'lTsFlow = SubsetByDate(aTs, lTs.Dates.Value(0), lTs.Dates.Value(lTs.numValues), Nothing)
                lTsFlow = SubsetByDate(aTs, lStartDate, lTs.Dates.Value(lTs.numValues), Nothing)
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
        Dim lHeader2100 As String = "" & vbCrLf &
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf &
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf &
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow   (Mgal/d/" & vbCrLf &
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & vbCrLf &
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2110
        Dim lHeader2110 As String = "" & vbCrLf &
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/" & vbCrLf &
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   Base" & vbCrLf &
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow     flow" & vbCrLf &
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)   (Mgal/d)" & vbCrLf &
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2120
        Dim lHeader2120 As String = "" & vbCrLf &
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf &
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf &
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/" & vbCrLf &
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)" & vbCrLf &
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        '2130
        Dim lHeader2130 As String = "" & vbCrLf &
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base" & vbCrLf &
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow" & vbCrLf &
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (m3/s/" & vbCrLf &
     Space(6) & "  (m3/s)     (m3/s)     (m3/s)     (cm)    (cm)    (cm)    (%)      km2)" & vbCrLf &
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ----------"

        Dim lHeaderEngAll As String = "" & vbCrLf &
     Space(6) & "   Mean       Mean       Mean     Total   Total   Total    BF/     Base       Base" & vbCrLf &
     Space(6) & "  stream-     base      surface  stream-   base  surface stream-   flow       flow       Base" & vbCrLf &
     Space(6) & "   flow       flow      runoff    flow     flow   runoff  flow    (ft3/s/   (Mgal/d/     flow" & vbCrLf &
     Space(6) & "  (ft3/s)    (ft3/s)    (ft3/s)    (in)    (in)    (in)    (%)      mi2)       mi2)    (Mgal/d)" & vbCrLf &
     Space(6) & "---------- ---------- ---------- ------- ------- -------  ----- ---------- ---------- ----------"

        '2150 FORMAT (A5,3F11.2,3F8.3,F7.2,F11.3)
        Dim lMonthNames() As String = {"Dummy", "Jan. ", "Feb. ", "Mar. ", "Apr. ",
                                                "May  ", "June ", "July ", "Aug. ",
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
            lDateStr = lDate(0).ToString.PadLeft(9, " ") &
                       lDate(1).ToString.PadLeft(4, " ") &
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
        If lTsYearly IsNot Nothing Then
            lTsYearly.Clear() : lTsYearly = Nothing
        End If
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
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 4, 5, 6
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 7, 8, 9
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsMonthlyFlowDepthExt.Value(I)) Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsMonthlyFlowDepthExt.Value(I)
                            End If
                        Case 10, 11, 12
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsMonthlyFlowDepthExt.Value(I) < -99.0 OrElse
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
            If lTsYearly IsNot Nothing AndAlso lYearCount <= lTsYearly.numValues Then
                If Not Double.IsNaN(lTsYearly.Value(lYearCount)) AndAlso
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
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter1Negative = True
                            Else
                                lQuarter1 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 4, 5, 6
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter2Negative = True
                            Else
                                lQuarter2 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 7, 8, 9
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse
                               Double.IsNaN(lTsBaseflowMonthlyDepthExt.Value(I)) Then
                                lQuarter3Negative = True
                            Else
                                lQuarter3 += lTsBaseflowMonthlyDepthExt.Value(I)
                            End If
                        Case 10, 11, 12
                            If I > lTsMonthlyFlowDepthExt.numValues OrElse
                               lTsBaseflowMonthlyDepthExt.Value(I) < -99.0 OrElse
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
            If lTsBFYearly IsNot Nothing AndAlso lYearCount <= lTsBFYearly.numValues Then
                If Not Double.IsNaN(lTsBFYearly.Value(lYearCount)) AndAlso
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
        If lTsYearly IsNot Nothing Then
            lTsYearly.Clear() : lTsYearly = Nothing
        End If
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

    Public Sub ASCIIBFIDaily(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethod As BFMethods)

        Dim lTsBaseflow As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Method")
                    Case aMethod
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
        If aMethod = BFMethods.BFIStandard Then 'aMethodame.StartsWith("BFIStandard")
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethod = BFMethods.BFIModified Then 'aMethodame.StartsWith("BFIModified") 
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
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

    Public Sub ASCIIBFI(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethod As BFMethods)
        Dim lTsBaseflow As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Method")
                    Case aMethod
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
        If aMethod = BFMethods.BFIStandard Then 'aMethodName.StartsWith("BFIStandard") 
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethod = BFMethods.BFIModified Then 'aMethodName.StartsWith("BFIModified") 
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
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

    Public Sub ASCIIBFITp(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethod As BFMethods)
        Dim lTsBaseflow As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Method")
                    Case aMethod
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
        If aMethod = BFMethods.BFIStandard Then 'aMethodName.StartsWith("BFIStandard") 
            lblMethodStd = " * "
            Dim lBFIFrac As Double = lTsBaseflow.Attributes.GetValue("BFIFrac")
            lblMethodParm = "   METHOD =   1" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
                            "   f      =" & String.Format("{0:.000000}", lBFIFrac).PadLeft(10, " ")
        ElseIf aMethod = BFMethods.BFIModified Then 'aMethodName.StartsWith("BFIModified")
            lblMethodMod = " * "
            Dim lBFIK1Day As Double = lTsBaseflow.Attributes.GetValue("BFIK1Day")
            lblMethodParm = "   METHOD =   2" & vbCrLf &
                            "   N      =" & lBFINDay.ToString.PadLeft(4, " ") & vbCrLf &
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

    Public Sub ASCIIBFLOWDat(ByVal aTS As atcTimeseries, ByVal aFilename As String, ByVal aMethod As BFMethods)
        Dim lTsBaseflow As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Method")
                    Case aMethod
                        If lTsBF.Attributes.GetValue("Scenario").ToString() = "BFLOWDaily1" Then
                            lTsBaseflow = lTsBF
                        End If
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFLOWDat: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow Is Nothing Then
            Logger.Dbg("ASCIIBFLOWDat: no baseflow data found.")
            Exit Sub
        End If

        Dim lResults As atcDataAttributes = lTsBaseflow.Attributes
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            Dim lWriteHeader As Boolean = True
            If IO.File.Exists(aFilename) Then
                lWriteHeader = False
            End If
            lSW = New System.IO.StreamWriter(aFilename, True)
            Dim lDatasetName As String = lResults.GetValue("Location", "")
            If lWriteHeader Then
                lSW.WriteLine("Baseflow data file: this file summarizes the fraction " &
                              "of streamflow that is contributed by baseflow for each " &
                              "of the 3 passes made by the program")
                lSW.WriteLine("Gage_file      " & " Baseflow_Fr1" & " Baseflow_Fr2" &
                              " Baseflow_Fr3" & "    NPR" & " Alpha_Factor" &
                              " Baseflow_Days")
            End If

            '5002 format(a15,1x,f12.2,1x,f12.2,1x,f12.2,1x,i6,1x,f12.4,1x,f13.4)

            Dim lstrfwfile As String = " ".PadLeft(15, " ")
            If Not String.IsNullOrEmpty(lDatasetName) Then
                Dim lFilenameOnly As String = IO.Path.GetFileName(lDatasetName)
                If lFilenameOnly.Length >= 15 Then
                    lstrfwfile = lFilenameOnly.Substring(0, 15)
                Else
                    lstrfwfile = lFilenameOnly.PadLeft(15, " ")
                End If
            End If
            Dim lstrbflw_fr1 As String = DoubleToString(lResults.GetValue("fr1", -99), 12, "0.00").PadLeft(12, " ")
            Dim lstrbflw_fr2 As String = DoubleToString(lResults.GetValue("fr2", -99), 12, "0.00").PadLeft(12, " ")
            Dim lstrbflw_fr3 As String = DoubleToString(lResults.GetValue("fr3", -99), 12, "0.00").PadLeft(12, " ")
            Dim lstrnpr As String = ""
            Dim lstralf As String = ""
            Dim lstrbfd As String = ""
            Dim lnpr As Integer = 0
            If Integer.TryParse(lResults.GetValue("npr", -1), lnpr) AndAlso lnpr > 1 Then
                Dim lalf As Double = lResults.GetValue("alf", -99)
                Dim lbfd As Double = lResults.GetValue("bfd", -99)
                lstrnpr = lnpr.ToString().PadLeft(6, " ")
                'lstralf = DoubleToString(lalf, 12, "#.0000").PadLeft(12, " ")
                'lstrbfd = DoubleToString(lbfd, 13, "#.00000").PadLeft(13, " ")
                lstralf = String.Format("{0:0.0000}", lalf).PadLeft(12, " ")
                lstrbfd = String.Format("{0:0.0000}", lbfd).PadLeft(13, " ")
            End If
            If String.IsNullOrEmpty(lstrnpr) Then
                lSW.WriteLine(lstrfwfile & " " & lstrbflw_fr1 & " " & lstrbflw_fr2 & " " & lstrbflw_fr3)
            Else
                lSW.WriteLine(lstrfwfile & " " & lstrbflw_fr1 & " " & lstrbflw_fr2 & " " & lstrbflw_fr3 & " " &
                              lstrnpr & " " & lstralf & " " & lstrbfd)
            End If
        Catch ex As Exception
            'lOutputGood = False
        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
    End Sub

    Public Sub ASCIIBFLOWDailyOut0(ByVal aTS As atcTimeseries, ByVal aFilename As String)
        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "BFLOWDaily1"
                        lTsBaseflow1 = lTsBF
                    Case "BFLOWDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "BFLOWDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFLOWDaily: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIBFLOWDaily: no baseflow data found.")
            Exit Sub
        End If
        Dim lSW As IO.StreamWriter = Nothing
        Try
            lSW = New IO.StreamWriter(aFilename, False)
            Dim lDate(5) As Integer
            J2Date(aTS.Dates.Value(0), lDate)
            Dim lStartingYear As String = lDate(0).ToString
            J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
            Dim lEndingYear As String = lDate(0).ToString

            lSW.WriteLine(" THIS IS FILE BFLOWDAY.TXT WHICH GIVES DAILY OUTPUT OF PROGRAM BFLOW. ")
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

            For I As Integer = 0 To aTS.numValues - 1
                lDayCount = (I + 1).ToString.PadLeft(5, " ")
                lStreamFlow = String.Format("{0:0.00}", aTS.Value(I + 1)).PadLeft(11, " ")
                lBF1 = String.Format("{0:0.00}", lTsBaseflow1.Value(I + 1)).PadLeft(11, " ")
                lBF2 = String.Format("{0:0.00}", lTsBaseflow2.Value(I + 1)).PadLeft(11, " ")
                lBF3 = String.Format("{0:0.00}", lTsBaseflow3.Value(I + 1)).PadLeft(11, " ")
                J2Date(aTS.Dates.Value(I), lDate)
                lDateStr = lDate(0).ToString.PadLeft(9, " ") &
                           lDate(1).ToString.PadLeft(4, " ") &
                           lDate(2).ToString.PadLeft(4, " ")
                lSW.WriteLine(lDayCount & lStreamFlow & lBF1 & lBF2 & lBF3 & lDateStr)
            Next
            lSW.Flush()
        Catch ex As Exception

        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
    End Sub

    Public Sub ASCIIBFLOWDailyOut(ByVal aTS As atcTimeseries, ByVal aFilename As String)
        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "BFLOWDaily1"
                        lTsBaseflow1 = lTsBF
                    Case "BFLOWDaily2"
                        lTsBaseflow2 = lTsBF
                    Case "BFLOWDaily3"
                        lTsBaseflow3 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIIBFLOWDaily: no baseflow data found.")
            Exit Sub
        End If

        If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
            Logger.Dbg("ASCIIBFLOWDaily: no baseflow data found.")
            Exit Sub
        End If
        Dim lSW As IO.StreamWriter = Nothing
        Try
            Dim lDateFormat As New atcDateFormat()
            With lDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
                .Midnight24 = True
            End With
            lSW = New IO.StreamWriter(aFilename, False)
            Dim lDate(5) As Integer
            J2Date(aTS.Dates.Value(0), lDate)
            Dim lStartingYear As String = lDateFormat.JDateToString(aTS.Dates.Value(0))
            J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
            Dim lEndingYear As String = lDateFormat.JDateToString(aTS.Dates.Value(aTS.numValues))

            lSW.WriteLine("BFLOW Daily baseflow filters values for data from: ")
            lSW.WriteLine("  INPUT FILE = " & IO.Path.GetFileName(aTS.Attributes.GetValue("History 1")))
            lSW.WriteLine("  STARTING Date =" & lStartingYear.PadLeft(15, " "))
            lSW.WriteLine("  ENDING Date =" & lEndingYear.PadLeft(15, " "))
            lSW.WriteLine("YEARMNDY   Streamflow  Bflow Pass1  Bflow Pass2  Bflow Pass3")
            Dim lStreamFlow As String
            Dim lBF1 As String
            Dim lBF2 As String
            Dim lBF3 As String
            Dim lDateStr As String

            'write(4, 6002) iyr(i), mon(i), iday(i), strflow(i), baseq(1,i), baseq(2,i), baseq(3,i)
            ' 6002 format (i4,i2,i2,1x,e12.6,1x,e12.6,1x,e12.6,1x,e12.6)

            For I As Integer = 0 To aTS.numValues - 1
                lStreamFlow = String.Format("{0:0.000000}", aTS.Value(I + 1)).PadLeft(13, " ")
                lBF1 = String.Format("{0:0.000000}", lTsBaseflow1.Value(I + 1)).PadLeft(13, " ")
                lBF2 = String.Format("{0:0.000000}", lTsBaseflow2.Value(I + 1)).PadLeft(13, " ")
                lBF3 = String.Format("{0:0.000000}", lTsBaseflow3.Value(I + 1)).PadLeft(13, " ")
                J2Date(aTS.Dates.Value(I), lDate)
                lDateStr = lDate(0) &
                           lDate(1).ToString.PadLeft(2, "0") &
                           lDate(2).ToString.PadLeft(2, "0")
                'lDateStr = lDateFormat.JDateToString(aTS.Dates.Value(I))
                lSW.WriteLine(lDateStr & lStreamFlow & lBF1 & lBF2 & lBF3)
            Next
            lSW.Flush()
        Catch ex As Exception

        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
    End Sub

    Private Function ASCIITwoPRDFDat(ByVal aFilename As String, ByVal aTS As atcTimeseries) As Boolean
        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "TwoPRDFDaily"
                        lTsBaseflow1 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIITwoPRDFDat: no baseflow data found.")
            Return False
        End If
        'aFilename = "baseflow_" & TargetTS.Attribute.GetValue("Location") & ".txt"
        Dim lRC As Double = Double.NaN
        Dim lBFI As Double = Double.NaN
        Dim lSRC As Double = Double.NaN
        Dim lSBFImax As Double = Double.NaN
        With lTsBaseflow1.Attributes
            lRC = lTsBaseflow1.Attributes.GetValue("RC")
            lBFI = lTsBaseflow1.Attributes.GetValue("BFI")
            lSRC = lTsBaseflow1.Attributes.GetValue("SRC")
            lSBFImax = lTsBaseflow1.Attributes.GetValue("SBFImax")

        End With
        'aFilename = "bfi.txt" -- unit 12
        'write(12,'(I11,2X,F5.3,3X,F4.2,5X,F5.2,10X,F5.2)') gageno, a, BFI, sa, SBFImax
        Dim lOutputGood As Boolean = True
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            Dim lWriteHeader As Boolean = True
            If IO.File.Exists(aFilename) Then
                lWriteHeader = False
            End If
            lSW = New IO.StreamWriter(aFilename, True)
            If lWriteHeader Then
                lSW.WriteLine("gage number      a    BFI  S(BFI|a)  S(BFI|BFImax)")
                lSW.WriteLine("--------------------------------------------------")
            End If
            Dim lstrgageno As String = aTS.Attributes.GetValue("Location", "").ToString().PadLeft(11, " ")
            Dim lstrRC As String = DoubleToString(lRC, 5, "0.000").PadLeft(5, " ")
            Dim lstrBFI As String = DoubleToString(lBFI, 4, "0.00").PadLeft(4, " ")
            Dim lstrSRC As String = DoubleToString(lSRC, 5, "0.00").PadLeft(5, " ")
            Dim lstrSBFI As String = DoubleToString(lSBFImax, 5, "0.00").PadLeft(5, " ")
            lSW.WriteLine(lstrgageno & "  " & lstrRC & "   " & lstrBFI & "     " & lstrSRC & "          " & lstrSBFI)
        Catch ex As Exception
            lOutputGood = False
        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
        If Not lOutputGood Then

        End If
        Return lOutputGood
    End Function

    Private Function ASCIITwoPRDFDailyOut(ByVal aFilename As String, ByVal aTS As atcTimeseries) As Boolean
        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lBFDatagroup As atcTimeseriesGroup = aTS.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                Select Case lTsBF.Attributes.GetValue("Scenario")
                    Case "TwoPRDFDaily"
                        lTsBaseflow1 = lTsBF
                End Select
            Next
        Else
            Logger.Dbg("ASCIITwoPRDFDaily: no baseflow data found.")
            Return False
        End If
        'aFilename = "baseflow_" & TargetTS.Attribute.GetValue("Location") & ".txt"
        Dim lRC As Double = lTsBaseflow1.Attributes.GetValue("RC")
        Dim lBFI As Double = lTsBaseflow1.Attributes.GetValue("BFI")
        Dim lOutputGood As Boolean = True
        Dim lSW As System.IO.StreamWriter = Nothing
        Try
            lSW = New IO.StreamWriter(aFilename, False)
            Dim lstrgageno As String = aTS.Attributes.GetValue("Location", "")
            lSW.WriteLine("Stream- and base-flow for gage number " & lstrgageno)
            lSW.WriteLine("recession constant= " & String.Format("{0:0.000}", lRC) & ", BFI= " & String.Format("{0:0.00}", lBFI))
            Dim lDateFormat As New atcDateFormat()
            With lDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
            Dim lstrDate As String = ""
            Dim lstrFlow As String = ""
            Dim lstrBaseflow As String = ""
            For I As Integer = 1 To aTS.numValues
                lstrDate = lDateFormat.JDateToString(aTS.Dates.Value(I)).PadLeft(10, " ")
                lstrFlow = DoubleToString(aTS.Value(I), 8, "0.0").PadLeft(8, " ")
                lstrBaseflow = DoubleToString(lTsBaseflow1.Value(I), 8, "0.0").PadLeft(8, " ")
                lSW.WriteLine(lstrDate & lstrFlow & lstrBaseflow)
            Next
        Catch ex As Exception
            lOutputGood = False
        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
        If Not lOutputGood Then

        End If
        Return lOutputGood
    End Function

    ''' <summary>
    ''' chainning base-flow analysis results on different continuous periods of data into complete time series
    ''' will put the complete base-flow record as an attribute to the original flow time series
    ''' return the daily base-flow time series for all methods in a new time series group, keyed on BFMethod
    ''' </summary>
    ''' <param name="aTserFullDateRange">The flow data time series with full date range, daily</param>
    ''' <param name="aTsAnalysisGroup">The group of continuous period of flow data, bf results are as their attributes</param>
    ''' <param name="aSTEP">a time step, Daily, Monthly, Yearly</param>
    ''' <param name="aClearChunks">Whether to clear the group of continuous period of flow data, and their bf results are as their attributes</param>
    Public Function MergeBaseflowResults(ByVal aTserFullDateRange As atcTimeseries,
                                         ByVal aTsAnalysisGroup As atcTimeseriesGroup,
                                         ByVal aSTEP As String,
                                         Optional ByVal aClearChunks As Boolean = False) As atcDataAttributes
        Dim lStart As Double = -99.9
        Dim lEnd As Double = -99.9
        Dim lDA As Double = -99.9
        'Dim lTserFullDateRange As New atcTimeseries(Nothing)
        'With lTserFullDateRange
        '    .Dates = New atcTimeseries(Nothing)
        '    .Dates.Values = NewDates(aFirstStart, aLastEnd, atcTimeUnit.TUDay, 1)
        '    .numValues = lTserFullDateRange.Dates.numValues
        '    .SetInterval(atcTimeUnit.TUDay, 1)
        '    For I As Integer = 1 To .numValues
        '        .Value(I) = -99.0
        '    Next
        'End With

        Dim lTserFullDateRangeMonthly As atcTimeseries = Aggregate(aTserFullDateRange, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
        Dim lTserFullDateRangeYearly As atcTimeseries = Aggregate(aTserFullDateRange, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)

        Dim lTsGroupPartAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupFixedAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupLocMinAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupSlideAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFAllDaily As New atcTimeseriesGroup()

        Dim lTsGroupPartAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupFixedAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupLocMinAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupSlideAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFAllMonthly As New atcTimeseriesGroup()

        Dim lTsGroupPartAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupFixedAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupLocMinAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupSlideAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFAllYearly As New atcTimeseriesGroup()

        Dim lTsGroupPartDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupFixedDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupLocMinDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupSlideDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWDepthAllDaily As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFDepthAllDaily As New atcTimeseriesGroup()

        Dim lTsGroupPartDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupFixedDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupLocMinDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupSlideDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWDepthAllMonthly As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFDepthAllMonthly As New atcTimeseriesGroup()

        Dim lTsGroupPartDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupFixedDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupLocMinDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupSlideDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFIStandardDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFIModifiedDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupBFLOWDepthAllYearly As New atcTimeseriesGroup()
        Dim lTsGroupTwoPRDFDepthAllYearly As New atcTimeseriesGroup()

        Dim lTsPartAllDaily As atcTimeseries = Nothing
        Dim lTsFixedAllDaily As atcTimeseries = Nothing
        Dim lTsLocMinAllDaily As atcTimeseries = Nothing
        Dim lTsSlideAllDaily As atcTimeseries = Nothing
        Dim lTsBFIStandardAllDaily As atcTimeseries = Nothing
        Dim lTsBFIModifiedAllDaily As atcTimeseries = Nothing
        Dim lTsBFLOWAllDaily As atcTimeseries = Nothing
        Dim lTsTwoPRDFAllDaily As atcTimeseries = Nothing

        Dim lTsPartAllMonthly As atcTimeseries = Nothing
        Dim lTsFixedAllMonthly As atcTimeseries = Nothing
        Dim lTsLocMinAllMonthly As atcTimeseries = Nothing
        Dim lTsSlideAllMonthly As atcTimeseries = Nothing
        Dim lTsBFIStandardAllMonthly As atcTimeseries = Nothing
        Dim lTsBFIModifiedAllMonthly As atcTimeseries = Nothing
        Dim lTsBFLOWAllMonthly As atcTimeseries = Nothing
        Dim lTsTwoPRDFAllMonthly As atcTimeseries = Nothing

        Dim lTsPartAllYearly As atcTimeseries = Nothing
        Dim lTsFixedAllYearly As atcTimeseries = Nothing
        Dim lTsLocMinAllYearly As atcTimeseries = Nothing
        Dim lTsSlideAllYearly As atcTimeseries = Nothing
        Dim lTsBFIStandardAllYearly As atcTimeseries = Nothing
        Dim lTsBFIModifiedAllYearly As atcTimeseries = Nothing
        Dim lTsBFLOWAllYearly As atcTimeseries = Nothing
        Dim lTsTwoPRDFAllYearly As atcTimeseries = Nothing

        Dim lTsPartDepthAllDaily As atcTimeseries = Nothing
        Dim lTsFixedDepthAllDaily As atcTimeseries = Nothing
        Dim lTsLocMinDepthAllDaily As atcTimeseries = Nothing
        Dim lTsSlideDepthAllDaily As atcTimeseries = Nothing
        Dim lTsBFIStandardDepthAllDaily As atcTimeseries = Nothing
        Dim lTsBFIModifiedDepthAllDaily As atcTimeseries = Nothing
        Dim lTsBFLOWDepthAllDaily As atcTimeseries = Nothing
        Dim lTsTwoPRDFDepthAllDaily As atcTimeseries = Nothing

        Dim lTsPartDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsFixedDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsLocMinDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsSlideDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsBFIStandardDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsBFIModifiedDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsBFLOWDepthAllMonthly As atcTimeseries = Nothing
        Dim lTsTwoPRDFDepthAllMonthly As atcTimeseries = Nothing

        Dim lTsPartDepthAllYearly As atcTimeseries = Nothing
        Dim lTsFixedDepthAllYearly As atcTimeseries = Nothing
        Dim lTsLocMinDepthAllYearly As atcTimeseries = Nothing
        Dim lTsSlideDepthAllYearly As atcTimeseries = Nothing
        Dim lTsBFIStandardDepthAllYearly As atcTimeseries = Nothing
        Dim lTsBFIModifiedDepthAllYearly As atcTimeseries = Nothing
        Dim lTsBFLOWDepthAllYearly As atcTimeseries = Nothing
        Dim lTsTwoPRDFDepthAllYearly As atcTimeseries = Nothing


        Dim lArgs As New atcDataAttributes()

        For Each lTsChunk As atcTimeseries In aTsAnalysisGroup
            'Organize data
            lStart = -99.9
            lEnd = -99.9
            lDA = -99.9
            Dim lTsGroupPart As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.PART, lStart, lEnd, lDA)
            Dim lTsGroupFixed As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.HySEPFixed, lStart, lEnd, lDA)
            Dim lTsGroupLocMin As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.HySEPLocMin, lStart, lEnd, lDA)
            Dim lTsGroupSlide As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.HySEPSlide, lStart, lEnd, lDA)
            Dim lTsGroupBFIStandard As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.BFIStandard, lStart, lEnd, lDA)
            Dim lTsGroupBFIModified As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.BFIModified, lStart, lEnd, lDA)
            Dim lTsGroupBFLOW As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.BFLOW, lStart, lEnd, lDA)
            Dim lTsGroupTwoPRDF As atcCollection = ConstructReportTsGroup(lTsChunk, BFMethods.TwoPRDF, lStart, lEnd, lDA)
            If (lStart < 0 AndAlso lEnd < 0) OrElse lDA <= 0 Then Continue For

            Dim lCtr As Integer = lTsChunk.Attributes.GetValue("period")
            aSTEP = "Daily"
            If lTsGroupPart.Count > 0 Then lTsGroupPartAllDaily.Add(lCtr, lTsGroupPart.ItemByKey("Rate" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedAllDaily.Add(lCtr, lTsGroupFixed.ItemByKey("Rate" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinAllDaily.Add(lCtr, lTsGroupLocMin.ItemByKey("Rate" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideAllDaily.Add(lCtr, lTsGroupSlide.ItemByKey("Rate" & aSTEP))

            If lTsGroupPart.Count > 0 Then lTsGroupPartDepthAllDaily.Add(lCtr, lTsGroupPart.ItemByKey("Depth" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedDepthAllDaily.Add(lCtr, lTsGroupFixed.ItemByKey("Depth" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinDepthAllDaily.Add(lCtr, lTsGroupLocMin.ItemByKey("Depth" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideDepthAllDaily.Add(lCtr, lTsGroupSlide.ItemByKey("Depth" & aSTEP))

            'need to break up the BFI resulting time series into periods
            If lTsGroupBFIStandard.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    lTsGroupBFIStandardAllDaily.Add(lCtr, SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing))
                End If
                If lBFIDepthTser IsNot Nothing Then
                    lTsGroupBFIStandardDepthAllDaily.Add(lCtr, SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing))
                End If
            End If

            If lTsGroupBFIModified.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    lTsGroupBFIModifiedAllDaily.Add(lCtr, SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing))
                End If
                If lBFIDepthTser IsNot Nothing Then
                    lTsGroupBFIModifiedDepthAllDaily.Add(lCtr, SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing))
                End If
                'lTsGroupBFIModifiedAllDaily.Add(lCtr, lTsGroupBFIModified.ItemByKey("Rate" & aSTEP))
                'lTsGroupBFIModifiedDepthAllDaily.Add(lCtr, lTsGroupBFIModified.ItemByKey("Depth" & aSTEP))
            End If

            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWAllDaily.Add(lCtr, lTsGroupBFLOW.ItemByKey("Rate" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFAllDaily.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Rate" & aSTEP))
            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWDepthAllDaily.Add(lCtr, lTsGroupBFLOW.ItemByKey("Depth" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFDepthAllDaily.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Depth" & aSTEP))

            aSTEP = "Monthly"
            If lTsGroupPart.Count > 0 Then lTsGroupPartAllMonthly.Add(lCtr, lTsGroupPart.ItemByKey("Rate" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedAllMonthly.Add(lCtr, lTsGroupFixed.ItemByKey("Rate" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinAllMonthly.Add(lCtr, lTsGroupLocMin.ItemByKey("Rate" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideAllMonthly.Add(lCtr, lTsGroupSlide.ItemByKey("Rate" & aSTEP))

            If lTsGroupPart.Count > 0 Then lTsGroupPartDepthAllMonthly.Add(lCtr, lTsGroupPart.ItemByKey("Depth" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedDepthAllMonthly.Add(lCtr, lTsGroupFixed.ItemByKey("Depth" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinDepthAllMonthly.Add(lCtr, lTsGroupLocMin.ItemByKey("Depth" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideDepthAllMonthly.Add(lCtr, lTsGroupSlide.ItemByKey("Depth" & aSTEP))

            If lTsGroupBFIStandard.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    lTsGroupBFIStandardAllMonthly.Add(lCtr, SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing))
                End If
                If lBFIDepthTser IsNot Nothing Then
                    lTsGroupBFIStandardDepthAllMonthly.Add(lCtr, SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing))
                End If

                'lTsGroupBFIStandardAllMonthly.Add(lCtr, lTsGroupBFIStandard.ItemByKey("Rate" & aSTEP))
                'lTsGroupBFIStandardDepthAllMonthly.Add(lCtr, lTsGroupBFIStandard.ItemByKey("Depth" & aSTEP))
            End If

            If lTsGroupBFIModified.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    lTsGroupBFIModifiedAllMonthly.Add(lCtr, SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing))
                End If
                If lBFIDepthTser IsNot Nothing Then
                    lTsGroupBFIModifiedDepthAllMonthly.Add(lCtr, SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing))
                End If
                'lTsGroupBFIModifiedAllMonthly.Add(lCtr, lTsGroupBFIModified.ItemByKey("Rate" & aSTEP))
                'lTsGroupBFIModifiedDepthAllMonthly.Add(lCtr, lTsGroupBFIModified.ItemByKey("Depth" & aSTEP))
            End If

            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWAllMonthly.Add(lCtr, lTsGroupBFLOW.ItemByKey("Rate" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFAllMonthly.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Rate" & aSTEP))
            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWDepthAllMonthly.Add(lCtr, lTsGroupBFLOW.ItemByKey("Depth" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFDepthAllMonthly.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Depth" & aSTEP))

            aSTEP = "Yearly"
            If lTsGroupPart.Count > 0 Then lTsGroupPartAllYearly.Add(lCtr, lTsGroupPart.ItemByKey("Rate" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedAllYearly.Add(lCtr, lTsGroupFixed.ItemByKey("Rate" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinAllYearly.Add(lCtr, lTsGroupLocMin.ItemByKey("Rate" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideAllYearly.Add(lCtr, lTsGroupSlide.ItemByKey("Rate" & aSTEP))

            If lTsGroupPart.Count > 0 Then lTsGroupPartDepthAllYearly.Add(lCtr, lTsGroupPart.ItemByKey("Depth" & aSTEP))
            If lTsGroupFixed.Count > 0 Then lTsGroupFixedDepthAllYearly.Add(lCtr, lTsGroupFixed.ItemByKey("Depth" & aSTEP))
            If lTsGroupLocMin.Count > 0 Then lTsGroupLocMinDepthAllYearly.Add(lCtr, lTsGroupLocMin.ItemByKey("Depth" & aSTEP))
            If lTsGroupSlide.Count > 0 Then lTsGroupSlideDepthAllYearly.Add(lCtr, lTsGroupSlide.ItemByKey("Depth" & aSTEP))

            'For BFI yearly time series, some times subsetbydate using chunk's time span will result in null dates/values due to mismatch time span
            'so it is important to put in try block and test for null dates to see if yearly subset is sensible
            If lTsGroupBFIStandard.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIStandard.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    Try
                        Dim lTsBnd As atcTimeseries = SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing)
                        If lTsBnd IsNot Nothing AndAlso lTsBnd.Dates IsNot Nothing AndAlso lTsBnd.Values IsNot Nothing Then
                            lTsGroupBFIStandardAllYearly.Add(lCtr, lTsBnd)
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If lBFIDepthTser IsNot Nothing Then
                    Try
                        Dim lTsBnd As atcTimeseries = SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing)
                        If lTsBnd IsNot Nothing AndAlso lTsBnd.Dates IsNot Nothing AndAlso lTsBnd.Values IsNot Nothing Then
                            lTsGroupBFIStandardDepthAllYearly.Add(lCtr, lTsBnd)
                        End If
                    Catch ex As Exception
                    End Try
                End If
                'If lTsGroupBFIStandard.Count > 0 Then lTsGroupBFIStandardAllYearly.Add(lCtr, lTsGroupBFIStandard.ItemByKey("Rate" & aSTEP))
                'If lTsGroupBFIStandard.Count > 0 Then lTsGroupBFIStandardDepthAllYearly.Add(lCtr, lTsGroupBFIStandard.ItemByKey("Depth" & aSTEP))
            End If

            If lTsGroupBFIModified.Count > 0 Then
                Dim lBFIRateTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Rate" & aSTEP)
                Dim lBFIDepthTser As atcTimeseries = lTsGroupBFIModified.ItemByKey("Depth" & aSTEP)
                Dim lChunkStart As Double = lTsChunk.Dates.Value(0)
                Dim lChunkEnd As Double = lTsChunk.Dates.Value(lTsChunk.numValues)
                If lBFIRateTser IsNot Nothing Then
                    Try
                        Dim lTsBnd As atcTimeseries = SubsetByDate(lBFIRateTser, lChunkStart, lChunkEnd, Nothing)
                        If lTsBnd IsNot Nothing AndAlso lTsBnd.Dates IsNot Nothing AndAlso lTsBnd.Values IsNot Nothing Then
                            lTsGroupBFIModifiedAllYearly.Add(lCtr, lTsBnd)
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If lBFIDepthTser IsNot Nothing Then
                    Try
                        Dim lTsBnd As atcTimeseries = SubsetByDate(lBFIDepthTser, lChunkStart, lChunkEnd, Nothing)
                        If lTsBnd IsNot Nothing AndAlso lTsBnd.Dates IsNot Nothing AndAlso lTsBnd.Values IsNot Nothing Then
                            lTsGroupBFIModifiedDepthAllYearly.Add(lCtr, lTsBnd)
                        End If
                    Catch ex As Exception

                    End Try
                End If
                'If lTsGroupBFIModified.Count > 0 Then lTsGroupBFIModifiedAllYearly.Add(lCtr, lTsGroupBFIModified.ItemByKey("Rate" & aSTEP))
                'If lTsGroupBFIModified.Count > 0 Then lTsGroupBFIModifiedDepthAllYearly.Add(lCtr, lTsGroupBFIModified.ItemByKey("Depth" & aSTEP))
            End If
            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWAllYearly.Add(lCtr, lTsGroupBFLOW.ItemByKey("Rate" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFAllYearly.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Rate" & aSTEP))
            If lTsGroupBFLOW.Count > 0 Then lTsGroupBFLOWDepthAllYearly.Add(lCtr, lTsGroupBFLOW.ItemByKey("Depth" & aSTEP))
            If lTsGroupTwoPRDF.Count > 0 Then lTsGroupTwoPRDFDepthAllYearly.Add(lCtr, lTsGroupTwoPRDF.ItemByKey("Depth" & aSTEP))
        Next

        'Daily
        If lTsGroupPartAllDaily.Count > 0 Then lTsPartAllDaily = MergeTimeseriesInSequence(lTsGroupPartAllDaily) 'MergeTimeseries(lTsGroupPartAllDaily, True)
        If lTsGroupPartDepthAllDaily.Count > 0 Then lTsPartDepthAllDaily = MergeTimeseriesInSequence(lTsGroupPartDepthAllDaily) 'MergeTimeseries(lTsGroupPartDepthAllDaily, True)

        If lTsGroupFixedAllDaily.Count > 0 Then lTsFixedAllDaily = MergeTimeseriesInSequence(lTsGroupFixedAllDaily)
        If lTsGroupFixedDepthAllDaily.Count > 0 Then lTsFixedDepthAllDaily = MergeTimeseriesInSequence(lTsGroupFixedDepthAllDaily)

        If lTsGroupLocMinAllDaily.Count > 0 Then lTsLocMinAllDaily = MergeTimeseriesInSequence(lTsGroupLocMinAllDaily)
        If lTsGroupLocMinDepthAllDaily.Count > 0 Then lTsLocMinDepthAllDaily = MergeTimeseriesInSequence(lTsGroupLocMinDepthAllDaily)

        If lTsGroupSlideAllDaily.Count > 0 Then lTsSlideAllDaily = MergeTimeseriesInSequence(lTsGroupSlideAllDaily)
        If lTsGroupSlideDepthAllDaily.Count > 0 Then lTsSlideDepthAllDaily = MergeTimeseriesInSequence(lTsGroupSlideDepthAllDaily)

        If lTsGroupBFIStandardAllDaily.Count > 0 Then lTsBFIStandardAllDaily = MergeTimeseriesInSequence(lTsGroupBFIStandardAllDaily)
        If lTsGroupBFIStandardDepthAllDaily.Count > 0 Then lTsBFIStandardDepthAllDaily = MergeTimeseriesInSequence(lTsGroupBFIStandardDepthAllDaily)

        If lTsGroupBFIModifiedAllDaily.Count > 0 Then lTsBFIModifiedAllDaily = MergeTimeseriesInSequence(lTsGroupBFIModifiedAllDaily)
        If lTsGroupBFIModifiedDepthAllDaily.Count > 0 Then lTsBFIModifiedDepthAllDaily = MergeTimeseriesInSequence(lTsGroupBFIModifiedDepthAllDaily)

        If lTsGroupBFLOWAllDaily.Count > 0 Then lTsBFLOWAllDaily = MergeTimeseriesInSequence(lTsGroupBFLOWAllDaily)
        If lTsGroupBFLOWDepthAllDaily.Count > 0 Then lTsBFLOWDepthAllDaily = MergeTimeseriesInSequence(lTsGroupBFLOWDepthAllDaily)

        If lTsGroupTwoPRDFAllDaily.Count > 0 Then lTsTwoPRDFAllDaily = MergeTimeseriesInSequence(lTsGroupTwoPRDFAllDaily)
        If lTsGroupTwoPRDFDepthAllDaily.Count > 0 Then lTsTwoPRDFDepthAllDaily = MergeTimeseriesInSequence(lTsGroupTwoPRDFDepthAllDaily)

        'Monthly
        If Not IsGroupEmpty(lTsGroupPartAllMonthly) Then lTsPartAllMonthly = MergeTimeseriesInSequence(lTsGroupPartAllMonthly) 'MergeTimeseries(lTsGroupPartAllMonthly, True)
        If Not IsGroupEmpty(lTsGroupPartDepthAllMonthly) Then lTsPartDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupPartDepthAllMonthly) 'MergeTimeseries(lTsGroupPartDepthAllMonthly, True)

        If Not IsGroupEmpty(lTsGroupFixedAllMonthly) Then lTsFixedAllMonthly = MergeTimeseriesInSequence(lTsGroupFixedAllMonthly)
        If Not IsGroupEmpty(lTsGroupFixedDepthAllMonthly) Then lTsFixedDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupFixedDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupLocMinAllMonthly) Then lTsLocMinAllMonthly = MergeTimeseriesInSequence(lTsGroupLocMinAllMonthly)
        If Not IsGroupEmpty(lTsGroupLocMinDepthAllMonthly) Then lTsLocMinDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupLocMinDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupSlideAllMonthly) Then lTsSlideAllMonthly = MergeTimeseriesInSequence(lTsGroupSlideAllMonthly)
        If Not IsGroupEmpty(lTsGroupSlideDepthAllMonthly) Then lTsSlideDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupSlideDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupBFIStandardAllMonthly) Then lTsBFIStandardAllMonthly = MergeTimeseriesInSequence(lTsGroupBFIStandardAllMonthly)
        If Not IsGroupEmpty(lTsGroupBFIStandardDepthAllMonthly) Then lTsBFIStandardDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupBFIStandardDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupBFIModifiedAllMonthly) Then lTsBFIModifiedAllMonthly = MergeTimeseriesInSequence(lTsGroupBFIModifiedAllMonthly)
        If Not IsGroupEmpty(lTsGroupBFIModifiedDepthAllMonthly) Then lTsBFIModifiedDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupBFIModifiedDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupBFLOWAllMonthly) Then lTsBFLOWAllMonthly = MergeTimeseriesInSequence(lTsGroupBFLOWAllMonthly)
        If Not IsGroupEmpty(lTsGroupBFLOWDepthAllMonthly) Then lTsBFLOWDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupBFLOWDepthAllMonthly)

        If Not IsGroupEmpty(lTsGroupTwoPRDFAllMonthly) Then lTsTwoPRDFAllMonthly = MergeTimeseriesInSequence(lTsGroupTwoPRDFAllMonthly)
        If Not IsGroupEmpty(lTsGroupTwoPRDFDepthAllMonthly) Then lTsTwoPRDFDepthAllMonthly = MergeTimeseriesInSequence(lTsGroupTwoPRDFDepthAllMonthly)

        'Yearly
        If Not IsGroupEmpty(lTsGroupPartAllYearly) Then lTsPartAllYearly = MergeTimeseriesInSequence(lTsGroupPartAllYearly)
        If Not IsGroupEmpty(lTsGroupPartDepthAllYearly) Then lTsPartDepthAllYearly = MergeTimeseriesInSequence(lTsGroupPartDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupFixedAllYearly) Then lTsFixedAllYearly = MergeTimeseriesInSequence(lTsGroupFixedAllYearly)
        If Not IsGroupEmpty(lTsGroupFixedDepthAllYearly) Then lTsFixedDepthAllYearly = MergeTimeseriesInSequence(lTsGroupFixedDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupLocMinAllYearly) Then lTsLocMinAllYearly = MergeTimeseriesInSequence(lTsGroupLocMinAllYearly)
        If Not IsGroupEmpty(lTsGroupLocMinDepthAllYearly) Then lTsLocMinDepthAllYearly = MergeTimeseriesInSequence(lTsGroupLocMinDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupSlideAllYearly) Then lTsSlideAllYearly = MergeTimeseriesInSequence(lTsGroupSlideAllYearly)
        If Not IsGroupEmpty(lTsGroupSlideDepthAllYearly) Then lTsSlideDepthAllYearly = MergeTimeseriesInSequence(lTsGroupSlideDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupBFIStandardAllYearly) Then lTsBFIStandardAllYearly = MergeTimeseriesInSequence(lTsGroupBFIStandardAllYearly)
        If Not IsGroupEmpty(lTsGroupBFIStandardDepthAllYearly) Then lTsBFIStandardDepthAllYearly = MergeTimeseriesInSequence(lTsGroupBFIStandardDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupBFIModifiedAllYearly) Then lTsBFIModifiedAllYearly = MergeTimeseriesInSequence(lTsGroupBFIModifiedAllYearly)
        If Not IsGroupEmpty(lTsGroupBFIModifiedDepthAllYearly) Then lTsBFIModifiedDepthAllYearly = MergeTimeseriesInSequence(lTsGroupBFIModifiedDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupBFLOWAllYearly) Then lTsBFLOWAllYearly = MergeTimeseriesInSequence(lTsGroupBFLOWAllYearly)
        If Not IsGroupEmpty(lTsGroupBFLOWDepthAllYearly) Then lTsBFLOWDepthAllYearly = MergeTimeseriesInSequence(lTsGroupBFLOWDepthAllYearly)

        If Not IsGroupEmpty(lTsGroupTwoPRDFAllYearly) Then lTsTwoPRDFAllYearly = MergeTimeseriesInSequence(lTsGroupTwoPRDFAllYearly)
        If Not IsGroupEmpty(lTsGroupTwoPRDFDepthAllYearly) Then lTsTwoPRDFDepthAllYearly = MergeTimeseriesInSequence(lTsGroupTwoPRDFDepthAllYearly)

        Dim lTmpGroup As New atcTimeseriesGroup()
        'lTmpGroup.Add(aFlowTser)
        'lTmpGroup.Add(lTserFullDateRange)
        'Dim lFlowTserAll As atcTimeseries = MergeTimeseries(lTmpGroup, True)
        If lTsPartAllDaily IsNot Nothing Then lTsPartAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsPartAllDaily)
        If lTsPartDepthAllDaily IsNot Nothing Then lTsPartDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsPartDepthAllDaily)
        If lTsFixedAllDaily IsNot Nothing Then lTsFixedAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsFixedAllDaily)
        If lTsFixedDepthAllDaily IsNot Nothing Then lTsFixedDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsFixedDepthAllDaily)
        If lTsLocMinAllDaily IsNot Nothing Then lTsLocMinAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsLocMinAllDaily)
        If lTsLocMinDepthAllDaily IsNot Nothing Then lTsLocMinDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsLocMinDepthAllDaily)
        If lTsSlideAllDaily IsNot Nothing Then lTsSlideAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsSlideAllDaily)
        If lTsSlideDepthAllDaily IsNot Nothing Then lTsSlideDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsSlideDepthAllDaily)
        If lTsBFIStandardAllDaily IsNot Nothing Then lTsBFIStandardAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFIStandardAllDaily)
        If lTsBFIStandardDepthAllDaily IsNot Nothing Then lTsBFIStandardDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFIStandardDepthAllDaily)
        If lTsBFIModifiedAllDaily IsNot Nothing Then lTsBFIModifiedAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFIModifiedAllDaily)
        If lTsBFIModifiedDepthAllDaily IsNot Nothing Then lTsBFIModifiedDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFIModifiedDepthAllDaily)
        If lTsBFLOWAllDaily IsNot Nothing Then lTsBFLOWAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFLOWAllDaily)
        If lTsBFLOWDepthAllDaily IsNot Nothing Then lTsBFLOWDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsBFLOWDepthAllDaily)
        If lTsTwoPRDFAllDaily IsNot Nothing Then lTsTwoPRDFAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsTwoPRDFAllDaily)
        If lTsTwoPRDFDepthAllDaily IsNot Nothing Then lTsTwoPRDFDepthAllDaily = MergeBaseflowTimeseries(aTserFullDateRange, lTsTwoPRDFDepthAllDaily)

        If lTsPartAllMonthly IsNot Nothing Then lTsPartAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsPartAllMonthly)
        If lTsPartDepthAllMonthly IsNot Nothing Then lTsPartDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsPartDepthAllMonthly)
        If lTsFixedAllMonthly IsNot Nothing Then lTsFixedAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsFixedAllMonthly)
        If lTsFixedDepthAllMonthly IsNot Nothing Then lTsFixedDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsFixedDepthAllMonthly)
        If lTsLocMinAllMonthly IsNot Nothing Then lTsLocMinAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsLocMinAllMonthly)
        If lTsLocMinDepthAllMonthly IsNot Nothing Then lTsLocMinDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsLocMinDepthAllMonthly)
        If lTsSlideAllMonthly IsNot Nothing Then lTsSlideAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsSlideAllMonthly)
        If lTsSlideDepthAllMonthly IsNot Nothing Then lTsSlideDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsSlideDepthAllMonthly)
        If lTsBFIStandardAllMonthly IsNot Nothing Then lTsBFIStandardAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFIStandardAllMonthly)
        If lTsBFIStandardDepthAllMonthly IsNot Nothing Then lTsBFIStandardDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFIStandardDepthAllMonthly)
        If lTsBFIModifiedAllMonthly IsNot Nothing Then lTsBFIModifiedAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFIModifiedAllMonthly)
        If lTsBFIModifiedDepthAllMonthly IsNot Nothing Then lTsBFIModifiedDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFIModifiedDepthAllMonthly)
        If lTsBFLOWAllMonthly IsNot Nothing Then lTsBFLOWAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFLOWAllMonthly)
        If lTsBFLOWDepthAllMonthly IsNot Nothing Then lTsBFLOWDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsBFLOWDepthAllMonthly)
        If lTsTwoPRDFAllMonthly IsNot Nothing Then lTsTwoPRDFAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsTwoPRDFAllMonthly)
        If lTsTwoPRDFDepthAllMonthly IsNot Nothing Then lTsTwoPRDFDepthAllMonthly = MergeBaseflowTimeseries(lTserFullDateRangeMonthly, lTsTwoPRDFDepthAllMonthly)

        If lTsPartAllYearly IsNot Nothing Then lTsPartAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsPartAllYearly)
        If lTsPartDepthAllYearly IsNot Nothing Then lTsPartDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsPartDepthAllYearly)
        If lTsFixedAllYearly IsNot Nothing Then lTsFixedAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsFixedAllYearly)
        If lTsFixedDepthAllYearly IsNot Nothing Then lTsFixedDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsFixedDepthAllYearly)
        If lTsLocMinAllYearly IsNot Nothing Then lTsLocMinAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsLocMinAllYearly)
        If lTsLocMinDepthAllYearly IsNot Nothing Then lTsLocMinDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsLocMinDepthAllYearly)
        If lTsSlideAllYearly IsNot Nothing Then lTsSlideAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsSlideAllYearly)
        If lTsSlideDepthAllYearly IsNot Nothing Then lTsSlideDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsSlideDepthAllYearly)
        If lTsBFIStandardAllYearly IsNot Nothing Then lTsBFIStandardAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFIStandardAllYearly)
        If lTsBFIStandardDepthAllYearly IsNot Nothing Then lTsBFIStandardDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFIStandardDepthAllYearly)
        If lTsBFIModifiedAllYearly IsNot Nothing Then lTsBFIModifiedAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFIModifiedAllYearly)
        If lTsBFIModifiedDepthAllYearly IsNot Nothing Then lTsBFIModifiedDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFIModifiedDepthAllYearly)
        If lTsBFLOWAllYearly IsNot Nothing Then lTsBFLOWAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFLOWAllYearly)
        If lTsBFLOWDepthAllYearly IsNot Nothing Then lTsBFLOWDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsBFLOWDepthAllYearly)
        If lTsTwoPRDFAllYearly IsNot Nothing Then lTsTwoPRDFAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsTwoPRDFAllYearly)
        If lTsTwoPRDFDepthAllYearly IsNot Nothing Then lTsTwoPRDFDepthAllYearly = MergeBaseflowTimeseries(lTserFullDateRangeYearly, lTsTwoPRDFDepthAllYearly)

        'Dim lNewGroup As New atcTimeseriesGroup()
        'With lNewGroup
        '    '.Add("streamflowfull", lFlowTserAll)
        '    If lTsPartAll IsNot Nothing Then .Add(BFMethods.PART, lTsPartAll)
        '    If lTsFixedAll IsNot Nothing Then .Add(BFMethods.HySEPFixed, lTsFixedAll)
        '    If lTsLocMinAll IsNot Nothing Then .Add(BFMethods.HySEPLocMin, lTsLocMinAll)
        '    If lTsSlideAll IsNot Nothing Then .Add(BFMethods.HySEPSlide, lTsSlideAll)
        '    If lTsBFIStandardAll IsNot Nothing Then .Add(BFMethods.BFIStandard, lTsBFIStandardAll)
        '    If lTsBFIModifiedAll IsNot Nothing Then .Add(BFMethods.BFIModified, lTsBFIModifiedAll)
        'End With

        If aClearChunks Then
            'Clear all chunky data
            For Each lTsChunk As atcTimeseries In aTsAnalysisGroup
                lTsChunk.Clear()
                lTsChunk = Nothing
            Next
        End If


        Dim lNewAttribs As New atcDataAttributes()
        With lNewAttribs
            Dim lTsGroupPart As New atcCollection()
            Dim lTsGroupFixed As New atcCollection()
            Dim lTsGroupLocMin As New atcCollection()
            Dim lTsGroupSlide As New atcCollection()
            Dim lTsGroupBFIStandard As New atcCollection()
            Dim lTsGroupBFIModified As New atcCollection()
            Dim lTsGroupBFLOW As New atcCollection()
            Dim lTsGroupTwoPRDF As New atcCollection()

            If lTsPartAllDaily IsNot Nothing Then
                lTsGroupPart.Add("RateDaily", lTsPartAllDaily)
                lTsGroupPart.Add("RateMonthly", lTsPartAllMonthly)
                lTsGroupPart.Add("RateYearly", lTsPartAllYearly)
                lTsGroupPart.Add("DepthDaily", lTsPartDepthAllDaily)
                lTsGroupPart.Add("DepthMonthly", lTsPartDepthAllMonthly)
                lTsGroupPart.Add("DepthYearly", lTsPartDepthAllYearly)
            End If
            If lTsFixedAllDaily IsNot Nothing Then
                lTsGroupFixed.Add("RateDaily", lTsFixedAllDaily)
                lTsGroupFixed.Add("RateMonthly", lTsFixedAllMonthly)
                lTsGroupFixed.Add("RateYearly", lTsFixedAllYearly)
                lTsGroupFixed.Add("DepthDaily", lTsFixedDepthAllDaily)
                lTsGroupFixed.Add("DepthMonthly", lTsFixedDepthAllMonthly)
                lTsGroupFixed.Add("DepthYearly", lTsFixedDepthAllYearly)
            End If

            If lTsLocMinAllDaily IsNot Nothing Then
                lTsGroupLocMin.Add("RateDaily", lTsLocMinAllDaily)
                lTsGroupLocMin.Add("RateMonthly", lTsLocMinAllMonthly)
                lTsGroupLocMin.Add("RateYearly", lTsLocMinAllYearly)
                lTsGroupLocMin.Add("DepthDaily", lTsLocMinDepthAllDaily)
                lTsGroupLocMin.Add("DepthMonthly", lTsLocMinDepthAllMonthly)
                lTsGroupLocMin.Add("DepthYearly", lTsLocMinDepthAllYearly)
            End If

            If lTsSlideAllDaily IsNot Nothing Then
                lTsGroupSlide.Add("RateDaily", lTsSlideAllDaily)
                lTsGroupSlide.Add("RateMonthly", lTsSlideAllMonthly)
                lTsGroupSlide.Add("RateYearly", lTsSlideAllYearly)
                lTsGroupSlide.Add("DepthDaily", lTsSlideDepthAllDaily)
                lTsGroupSlide.Add("DepthMonthly", lTsSlideDepthAllMonthly)
                lTsGroupSlide.Add("DepthYearly", lTsSlideDepthAllYearly)
            End If

            If lTsBFIStandardAllDaily IsNot Nothing Then
                lTsGroupBFIStandard.Add("RateDaily", lTsBFIStandardAllDaily)
                lTsGroupBFIStandard.Add("RateMonthly", lTsBFIStandardAllMonthly)
                lTsGroupBFIStandard.Add("RateYearly", lTsBFIStandardAllYearly)
                lTsGroupBFIStandard.Add("DepthDaily", lTsBFIStandardDepthAllDaily)
                lTsGroupBFIStandard.Add("DepthMonthly", lTsBFIStandardDepthAllMonthly)
                lTsGroupBFIStandard.Add("DepthYearly", lTsBFIStandardDepthAllYearly)
            End If

            If lTsBFIModifiedAllDaily IsNot Nothing Then
                lTsGroupBFIModified.Add("RateDaily", lTsBFIModifiedAllDaily)
                lTsGroupBFIModified.Add("RateMonthly", lTsBFIModifiedAllMonthly)
                lTsGroupBFIModified.Add("RateYearly", lTsBFIModifiedAllYearly)
                lTsGroupBFIModified.Add("DepthDaily", lTsBFIModifiedDepthAllDaily)
                lTsGroupBFIModified.Add("DepthMonthly", lTsBFIModifiedDepthAllMonthly)
                lTsGroupBFIModified.Add("DepthYearly", lTsBFIModifiedDepthAllYearly)
            End If

            If lTsBFLOWAllDaily IsNot Nothing Then
                lTsGroupBFLOW.Add("RateDaily", lTsBFLOWAllDaily)
                lTsGroupBFLOW.Add("RateMonthly", lTsBFLOWAllMonthly)
                lTsGroupBFLOW.Add("RateYearly", lTsBFLOWAllYearly)
                lTsGroupBFLOW.Add("DepthDaily", lTsBFLOWDepthAllDaily)
                lTsGroupBFLOW.Add("DepthMonthly", lTsBFLOWDepthAllMonthly)
                lTsGroupBFLOW.Add("DepthYearly", lTsBFLOWDepthAllYearly)
            End If

            If lTsTwoPRDFAllDaily IsNot Nothing Then
                lTsGroupTwoPRDF.Add("RateDaily", lTsTwoPRDFAllDaily)
                lTsGroupTwoPRDF.Add("RateMonthly", lTsTwoPRDFAllMonthly)
                lTsGroupTwoPRDF.Add("RateYearly", lTsTwoPRDFAllYearly)
                lTsGroupTwoPRDF.Add("DepthDaily", lTsTwoPRDFDepthAllDaily)
                lTsGroupTwoPRDF.Add("DepthMonthly", lTsTwoPRDFDepthAllMonthly)
                lTsGroupTwoPRDF.Add("DepthYearly", lTsTwoPRDFDepthAllYearly)
            End If

            .SetValue("GroupPart", lTsGroupPart)
            .SetValue("GroupFixed", lTsGroupFixed)
            .SetValue("GroupLocMin", lTsGroupLocMin)
            .SetValue("GroupSlide", lTsGroupSlide)
            .SetValue("GroupBFIStandard", lTsGroupBFIStandard)
            .SetValue("GroupBFIModified", lTsGroupBFIModified)
            .SetValue("GroupBFLOW", lTsGroupBFLOW)
            .SetValue("GroupTwoPRDF", lTsGroupTwoPRDF)
        End With

        Return lNewAttribs
        'Return lNewGroup
    End Function

    ''' <summary>
    ''' Merge a group of time series that are already in order
    ''' All time series need to have the same time unit and time step
    ''' </summary>
    ''' <param name="aTsGroup"></param>
    ''' <returns>A brand new merged time series</returns>
    Public Function MergeTimeseriesInSequence(ByVal aTsGroup As atcTimeseriesGroup) As atcTimeseries
        If aTsGroup Is Nothing OrElse aTsGroup.Count = 0 Then Return Nothing

        Dim lTu As atcTimeUnit = aTsGroup(0).Attributes.GetValue("Tu")
        Dim lTs As Integer = aTsGroup(0).Attributes.GetValue("ts")
        For I As Integer = 1 To aTsGroup.Count - 1
            If aTsGroup(I).Attributes.GetValue("Tu") <> lTu Then
                Return Nothing
            End If
            If aTsGroup(I).Attributes.GetValue("ts") <> lTs Then
                Return Nothing
            End If
        Next

        Dim lStartTime As Double = aTsGroup(0).Dates.Value(0)
        Dim lEndTime As Double = aTsGroup(aTsGroup.Count - 1).Dates.Value(aTsGroup(aTsGroup.Count - 1).numValues)
        Dim lNewTser As atcTimeseries = NewTimeseries(lStartTime, lEndTime, lTu, lTs, Nothing, Double.NaN)
        MergeAttributes(aTsGroup, lNewTser)
        Dim lDate1 As Double = 0
        Dim lDateg As Double = 0
        Dim lSearchIndex As Integer = 1
        For J As Integer = 0 To aTsGroup.Count - 1
            Dim lTsChunk As atcTimeseries = aTsGroup(J)
            For K As Integer = 1 To lTsChunk.numValues
                lDate1 = lTsChunk.Dates.Value(K - 1)
                For I As Integer = lSearchIndex To lNewTser.numValues
                    lDateg = lNewTser.Dates.Value(I - 1)
                    If lDate1 > lDateg Then
                        Continue For
                    ElseIf lDate1 < lDateg Then
                        lSearchIndex = I
                        Exit For
                    Else
                        lNewTser.Value(I) = lTsChunk.Value(K)
                        lSearchIndex = I
                        Exit For
                    End If
                Next
            Next
        Next

        Return lNewTser
    End Function
    ''' <summary>
    ''' Merge the partial base-flow time series with the full range of time series
    ''' Assumption is that both time series are of the same time unit and time step
    ''' </summary>
    ''' <param name="aFullDateTser">the full date range time series</param>
    ''' <param name="aPartialDateTser">partial date range time series, within the date range of the full date range</param>
    ''' <returns></returns>
    Public Function MergeBaseflowTimeseries(ByVal aFullDateTser As atcTimeseries,
                                            ByVal aPartialDateTser As atcTimeseries,
                                            Optional ByVal aClearPartialTser As Boolean = True,
                                            Optional ByVal aMergeAttribues As Boolean = False) As atcTimeseries
        Dim lDates(5) As Integer
        J2Date(aPartialDateTser.Dates.Value(0), lDates)
        Dim lFullMergedTser As atcTimeseries = aFullDateTser.Clone()
        Dim lTu As atcTimeUnit = aFullDateTser.Attributes.GetValue("tu")
        If lTu = atcTimeUnit.TUDay Then
            If aFullDateTser.Dates.Value(0) <> aPartialDateTser.Dates.Value(0) Then
                Dim lStop = "Stop"
            End If
        ElseIf lTu = atcTimeUnit.TUMonth Then
            If aFullDateTser.Dates.Value(0) <> aPartialDateTser.Dates.Value(0) Then
                Dim lStop = "Stop"
            End If
        ElseIf lTu = atcTimeUnit.TUYear Then
            If aFullDateTser.Dates.Value(0) <> aPartialDateTser.Dates.Value(0) Then
                Dim lStop = "Stop"
                'lFullMergedTser = SubsetByDateBoundary(aFullDateTser, lDates(1), lDates(2), Nothing)
                Dim lDatesBeg(5) As Integer
                J2Date(aFullDateTser.Dates.Value(0), lDatesBeg)
                Dim lDatesEnd(5) As Integer
                J2Date(aFullDateTser.Dates.Value(aFullDateTser.numValues), lDatesEnd)
                Dim lYearBeg As Integer = lDatesBeg(0)
                If lYearBeg > lDates(0) Then lYearBeg = lDates(0)
                Dim lMonthBeg As Integer = lDates(1)
                Dim lDayBeg As Integer = lDates(2)
                Dim lYearEnd As Integer = lDatesEnd(0)
                If lDatesEnd(1) > 1 OrElse lDatesEnd(2) > 1 Then
                    lYearEnd += 1
                End If
                Dim lMonthEnd As Integer = lDates(1)
                Dim lDayEnd As Integer = lDates(2)
                lFullMergedTser = NewTimeseries(Date2J(lYearBeg, lMonthBeg, lDayBeg, 0, 0, 0), Date2J(lYearEnd, lMonthEnd, lDayEnd, 24, 0, 0), atcTimeUnit.TUYear, 1, Nothing, -99.0)
            End If
        End If

        If aMergeAttribues Then
            Dim lTmpGroup As New atcTimeseriesGroup()
            lTmpGroup.Add(aPartialDateTser)
            MergeAttributes(lTmpGroup, lFullMergedTser)
        End If
        Dim lSearchIndex As Integer = 0
        Dim lDateFull As Double
        Dim lDatePart As Double
        For I As Integer = 0 To lFullMergedTser.numValues - 1
            lDateFull = lFullMergedTser.Dates.Value(I)
            For J As Integer = lSearchIndex To aPartialDateTser.numValues - 1
                lDatePart = aPartialDateTser.Dates.Value(J)
                If lDatePart > lDateFull Then
                    lSearchIndex = J
                    Exit For 'partial time serie loop for full range time series to catch up
                ElseIf lDatePart < lDateFull Then
                    'Continue For 'let partial time series loop continue to catch up with full range time series
                Else
                    lFullMergedTser.Value(I + 1) = aPartialDateTser.Value(J + 1)
                    lSearchIndex = J
                    Exit For
                End If
            Next
        Next
        'Dim lTmpGroup As New atcTimeseriesGroup()
        'lTmpGroup.Add(aPartialDateTser)
        'lTmpGroup.Add(aFullDateTser)
        'Dim lFullMergedTser As atcTimeseries = MergeTimeseries(lTmpGroup, True)
        'lTmpGroup.Clear()
        If aClearPartialTser Then
            aPartialDateTser.Clear()
            aPartialDateTser = Nothing
        End If
        Return lFullMergedTser
    End Function

    ''' <summary>
    ''' Clean the group to ensure the first element is not nothing or otherwise say it is empty (all nothing) or not
    ''' </summary>
    ''' <param name="aTserGroup"></param>
    ''' <returns></returns>
    Private Function IsGroupEmpty(ByRef aTserGroup As atcTimeseriesGroup) As Boolean
        If aTserGroup Is Nothing Then Return True
        While CountNothing(aTserGroup) > 0
            For I As Integer = 0 To aTserGroup.Count - 1
                If aTserGroup.ItemByIndex(I) Is Nothing Then
                    aTserGroup.RemoveAt(I)
                    Exit For
                End If
            Next
        End While
        If aTserGroup Is Nothing OrElse aTserGroup.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CountNothing(ByVal aTserGroup As atcTimeseriesGroup) As Integer
        Dim lCtr As Integer = 0
        If aTserGroup Is Nothing OrElse aTserGroup.Count = 0 Then
            Return lCtr
        End If
        For Each lTs As atcTimeseries In aTserGroup
            If lTs Is Nothing Then lCtr += 1
        Next
        Return lCtr
    End Function
End Module
