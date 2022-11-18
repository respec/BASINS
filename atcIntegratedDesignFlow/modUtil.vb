Imports atcData
Imports atcControls
Imports atcBatchProcessing
Imports atcUtility

Public Module modUtil

    Public RScriptFilename As String = "fnScreeningTests.R"
    Public RunRProg As String = "RunR.exe"
    Public Enum RTests
        BestFitDistribution
        Spearman
        PPCC
        KSFit
        KRatioStations
        KRatioOutliers
    End Enum

    ''' <summary>
    ''' Find a station ID by looking at STAID, site_no, Location, STANAM attributes, set STANAM attribute if found and return it
    ''' </summary>
    ''' <param name="aTs">Timeseries to search attributes of and set STANAM</param>
    ''' <returns></returns>
    Public Function STAID(ByVal aTs As atcTimeseries) As String
        Dim lSTAID As String = aTs.Attributes.GetValue("STAID", Nothing)
        If lSTAID Is Nothing Then
            lSTAID = aTs.Attributes.GetValue("site_no", Nothing)
            If lSTAID Is Nothing Then
                lSTAID = aTs.Attributes.GetValue("Location", Nothing)
                If lSTAID Is Nothing Then
                    lSTAID = aTs.Attributes.GetValue("STANAM", Nothing)
                    If lSTAID Is Nothing Then
                        lSTAID = "N/A"
                    End If
                End If
            End If
            aTs.Attributes.SetValue("STAID", lSTAID)
        End If
        Return lSTAID
    End Function

    Public Sub AddSeasonNameIfNeeded(aAttributes As List(Of String), aDataSets As atcTimeseriesGroup)
        If Not aAttributes.Contains("SeasonName") Then
            Dim lSeasons As atcCollection = aDataSets.SortedAttributeValues("SeasonName")
            If lSeasons.Count > 1 OrElse lSeasons.Count = 1 AndAlso lSeasons(0) IsNot Nothing Then
                aAttributes.Add("SeasonName")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Holds a set of inputs' names for batch run
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InputNames
        Inherits Inputs
        Public Shared HighLow As String = "HighOrLow"
        Public Shared HighLowText As String = "FLOWCONDITION"
        Public Shared Logarithmic As String = "LOGARITHMIC"
        Public Shared ReturnPeriod As String = "Return Period"
        Public Shared ReturnPeriods As String = "Return Periods"
        Public Shared ReturnPeriodText As String = "RECURRENCEINTERVAL"
        Public Shared NDay As String = "NDay"
        Public Shared NDays As String = "NDays"

        Public Shared MultiNDayPlot As String = "MultipleNDayPlots"
        Public Shared MultiStationPlot As String = "MultipleStationPlots"

        Public Enum ITAMethod
            NDAYTIMESERIES = 1
            FREQUECYGRID
            FREQUENCYGRAPH
            TRENDLIST
        End Enum
        Public Shared MethodNdayTser As String = ITAMethod.NDAYTIMESERIES.ToString() '"NDAYTIMESERIES"
        Public Shared MethodFreqGrid As String = ITAMethod.FREQUECYGRID.ToString()
        Public Shared MethodFreqPlot As String = ITAMethod.FREQUENCYGRAPH.ToString()
        Public Shared MethodTrendLst As String = ITAMethod.TRENDLIST.ToString()

        Public Shared Sub BuildInputSet(ByRef aSpecialSet As atcDataAttributes, ByVal aCommonSet As atcDataAttributes)
            If aSpecialSet Is Nothing Then
                aSpecialSet = New atcDataAttributes()
            End If
            Dim lstNDayDefault() As Double = clsIDFPlugin.ListDefaultArray(NDay)
            Dim lstRPsDefault() As Double = clsIDFPlugin.ListDefaultArray(ReturnPeriod)
            Dim lNDays As atcUtility.atcCollection
            Dim lRPs As atcUtility.atcCollection
            If aCommonSet Is Nothing Then
                With aSpecialSet
                    If .GetValue(HighLow) Is Nothing Then .SetValue(HighLow, "Low")
                    If .GetValue(Logarithmic) Is Nothing Then .SetValue(Logarithmic, True)
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, "")
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, "")
                    If .GetValue(MultiNDayPlot) Is Nothing Then .SetValue(MultiNDayPlot, False)
                    If .GetValue(MultiStationPlot) Is Nothing Then .SetValue(MultiStationPlot, False)
                    If .GetValue(HighFlowSeasonStart) Is Nothing Then .SetValue(HighFlowSeasonStart, "10/01")
                    If .GetValue(HighFlowSeasonEnd) Is Nothing Then .SetValue(HighFlowSeasonEnd, "09/30")
                    If .GetValue(LowFlowSeasonStart) Is Nothing Then .SetValue(LowFlowSeasonStart, "04/01")
                    If .GetValue(LowFlowSeasonEnd) Is Nothing Then .SetValue(LowFlowSeasonEnd, "03/31")
                    If .GetValue(NDay) Is Nothing Then
                        .SetValue(NDay, lstNDayDefault)
                    End If
                    Dim lNDaysOriginal As atcUtility.atcCollection = .GetValue(NDays)
                    lNDays = New atcUtility.atcCollection()
                    If .GetValue(NDays) IsNot Nothing Then
                        'detach from original attribute set
                        .RemoveByKey(NDays)
                    End If
                    If lNDaysOriginal IsNot Nothing Then
                        For Each lNDayKey As Double In lNDaysOriginal.Keys
                            lNDays.Add(lNDayKey, lNDaysOriginal.ItemByKey(lNDayKey))
                        Next
                        lNDaysOriginal.Clear()
                        lNDaysOriginal = Nothing
                    End If

                    For Each lNDay As Double In .GetValue(NDay)
                        If Not lNDays.Keys.Contains(lNDay) Then
                            lNDays.Add(lNDay, False)
                        End If
                    Next
                    .SetValue(NDays, lNDays)

                    If .GetValue(ReturnPeriod) Is Nothing Then
                        .SetValue(ReturnPeriod, lstRPsDefault)
                    End If
                    Dim lRPsOriginal As atcUtility.atcCollection = .GetValue(ReturnPeriods)
                    If .GetValue(ReturnPeriods) IsNot Nothing Then
                        .RemoveByKey(ReturnPeriods)
                    End If
                    lRPs = New atcUtility.atcCollection()
                    If lRPsOriginal IsNot Nothing Then
                        For Each lRPKey As Double In lRPsOriginal.Keys
                            lRPs.Add(lRPKey, lRPsOriginal.ItemByKey(lRPKey))
                        Next
                        lRPsOriginal.Clear()
                        lRPsOriginal = Nothing
                    End If

                    For Each lRP As Double In .GetValue(ReturnPeriod)
                        If Not lRPs.Keys.Contains(lRP) Then
                            lRPs.Add(lRP, False)
                        End If
                    Next
                    .SetValue(ReturnPeriods, lRPs)
                End With
            Else
                With aSpecialSet
                    If .GetValue(HighLow) Is Nothing Then .SetValue(HighLow, aCommonSet.GetValue(HighLow, "Low"))
                    If .GetValue(Logarithmic) Is Nothing Then .SetValue(Logarithmic, aCommonSet.GetValue(Logarithmic, True))
                    If .GetValue(OutputDir) Is Nothing OrElse String.IsNullOrEmpty(.GetValue(OutputDir).ToString()) Then .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                    If .GetValue(OutputPrefix) Is Nothing Then .SetValue(OutputPrefix, aCommonSet.GetValue(OutputPrefix, ""))
                    If .GetValue(DataDir) Is Nothing Then .SetValue(DataDir, aCommonSet.GetValue(DataDir, ""))
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, aCommonSet.GetValue(StartYear, ""))
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, aCommonSet.GetValue(EndYear, ""))
                    If .GetValue(MultiNDayPlot) Is Nothing Then .SetValue(MultiNDayPlot, aCommonSet.GetValue(MultiNDayPlot, False))
                    If .GetValue(MultiStationPlot) Is Nothing Then .SetValue(MultiStationPlot, aCommonSet.GetValue(MultiStationPlot, False))
                    If .GetValue(HighFlowSeasonStart) Is Nothing Then .SetValue(HighFlowSeasonStart, aCommonSet.GetValue(HighFlowSeasonStart, "10/01"))
                    If .GetValue(HighFlowSeasonEnd) Is Nothing Then .SetValue(HighFlowSeasonEnd, aCommonSet.GetValue(HighFlowSeasonEnd, "09/30"))
                    If .GetValue(LowFlowSeasonStart) Is Nothing Then .SetValue(LowFlowSeasonStart, aCommonSet.GetValue(LowFlowSeasonStart, "04/01"))
                    If .GetValue(LowFlowSeasonEnd) Is Nothing Then .SetValue(LowFlowSeasonEnd, aCommonSet.GetValue(LowFlowSeasonEnd, "03/31"))
                    If .GetValue(NDay) Is Nothing Then
                        .SetValue(NDay, aCommonSet.GetValue(NDay, lstNDayDefault))
                    End If
                    Dim lNDaysOriginal As atcUtility.atcCollection = .GetValue(NDays)
                    If lNDaysOriginal IsNot Nothing Then
                        'detach from original attribute set
                        .RemoveByKey(NDays)
                    Else
                        lNDaysOriginal = aCommonSet.GetValue(NDays)
                    End If
                    lNDays = New atcUtility.atcCollection()
                    If lNDaysOriginal IsNot Nothing Then
                        For Each lNDaykey As Double In lNDaysOriginal.Keys
                            lNDays.Add(lNDaykey, lNDaysOriginal.ItemByKey(lNDaykey))
                        Next
                        'don't clear it as it is the global default
                    End If
                    For Each lNDay As Double In .GetValue(NDay)
                        If Not lNDays.Keys.Contains(lNDay) Then
                            lNDays.Add(lNDay, False)
                        End If
                    Next
                    .SetValue(NDays, lNDays)

                    If .GetValue(ReturnPeriod) Is Nothing Then
                        .SetValue(ReturnPeriod, aCommonSet.GetValue(ReturnPeriod, lstRPsDefault))
                    End If
                    Dim lRPsOriginal As atcUtility.atcCollection = .GetValue(ReturnPeriods)
                    If lRPsOriginal IsNot Nothing Then
                        .RemoveByKey(ReturnPeriods)
                    Else
                        lRPsOriginal = aCommonSet.GetValue(ReturnPeriods)
                    End If
                    lRPs = New atcUtility.atcCollection()
                    If lRPsOriginal IsNot Nothing Then
                        For Each lRPKey As Double In lRPsOriginal.Keys
                            lRPs.Add(lRPKey, lRPsOriginal.ItemByKey(lRPKey))
                        Next
                        'Don't clear it as it is the global default
                    End If
                    For Each lRP As Double In .GetValue(ReturnPeriod)
                        If Not lRPs.Keys.Contains(lRP) Then
                            lRPs.Add(lRP, False)
                        End If
                    Next
                    .SetValue(ReturnPeriods, lRPs)
                End With
            End If
        End Sub

        Public Shared Function ParametersToText(ByVal aArgs As atcDataAttributes) As String
            If aArgs Is Nothing Then Return ""
            Dim loperation As String = aArgs.GetValue("Operation", "")
            Dim lgroupname As String = aArgs.GetValue("Group", "")
            Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

            Dim lText As New Text.StringBuilder()
            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("SWSTAT")
                Dim lStationInfo As ArrayList = aArgs.GetValue(InputNames.StationsInfo)
                If lStationInfo IsNot Nothing Then
                    For Each lstation As String In lStationInfo
                        lText.AppendLine(lstation)
                    Next
                End If
            ElseIf lSetGlobal Then
                lText.AppendLine("GLOBAL")
            End If

            'Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
            'Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
            'Dim lDates(5) As Integer
            'J2Date(lStartDate, lDates)
            'lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
            'J2Date(lEndDate, lDates)
            'timcnv(lDates)
            'lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

            If Not lSetGlobal Then
                If aArgs.ContainsAttribute(InputNames.IncludeYears) Then
                    Dim lDuration As String = aArgs.GetValue(InputNames.IncludeYears)
                    If Not String.IsNullOrEmpty(lDuration) Then lText.AppendLine(InputNames.IncludeYears & vbTab & lDuration)
                End If
                If aArgs.ContainsAttribute(InputNames.StartYear) Then
                    Dim lStartYear As String = aArgs.GetValue(InputNames.StartYear)
                    If Not String.IsNullOrEmpty(lStartYear) Then lText.AppendLine(InputNames.StartYear & vbTab & lStartYear)
                End If
                If aArgs.ContainsAttribute(InputNames.EndYear) Then
                    Dim lEndYear As String = aArgs.GetValue(InputNames.EndYear)
                    If Not String.IsNullOrEmpty(lEndYear) Then lText.AppendLine(InputNames.EndYear & vbTab & lEndYear)
                End If
            End If
            If aArgs.ContainsAttribute(atcIDF.modUtil.InputNames.Method) Then
                Dim lMethods As ArrayList = aArgs.GetValue(atcIDF.modUtil.InputNames.Method)
                For Each lMethod As atcIDF.modUtil.InputNames.ITAMethod In lMethods
                    Select Case lMethod
                        Case atcIDF.InputNames.ITAMethod.FREQUECYGRID
                            lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.FREQUECYGRID.ToString())
                        Case atcIDF.InputNames.ITAMethod.FREQUENCYGRAPH
                            lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
                        Case atcIDF.InputNames.ITAMethod.NDAYTIMESERIES
                            lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
                        Case atcIDF.InputNames.ITAMethod.TRENDLIST
                            lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.TRENDLIST.ToString())
                    End Select
                Next
            ElseIf lSetGlobal Then
                lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.FREQUECYGRID.ToString())
                lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
                lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
                lText.AppendLine(atcIDF.InputNames.Method & vbTab & atcIDF.InputNames.ITAMethod.TRENDLIST.ToString())
            End If

            If lSetGlobal Then
                lText.AppendLine(atcIDF.InputNames.HighLowText & vbTab & "HIGH")
                Dim lHighStart As String = atcIDF.InputNames.HighFlowSeasonStart
                Dim lHighEnd As String = atcIDF.InputNames.HighFlowSeasonEnd
                lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
                lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
                lText.AppendLine(atcIDF.InputNames.HighLowText & vbTab & "LOW")
                Dim lLowStart As String = atcIDF.InputNames.LowFlowSeasonStart
                Dim lLowEnd As String = atcIDF.InputNames.LowFlowSeasonEnd
                lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
                lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
            ElseIf aArgs.ContainsAttribute(atcIDF.InputNames.HighLow) Then
                Dim lCondition As String = aArgs.GetValue(atcIDF.InputNames.HighLow)
                If Not String.IsNullOrEmpty(lCondition) Then
                    If lCondition.ToLower().Contains("high") Then
                        lText.AppendLine(atcIDF.InputNames.HighLowText & vbTab & "HIGH")
                        Dim lHighStart As String = atcIDF.InputNames.HighFlowSeasonStart
                        Dim lHighEnd As String = atcIDF.InputNames.HighFlowSeasonEnd
                        lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
                        lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
                    ElseIf lCondition.ToLower().Contains("low") Then
                        lText.AppendLine(atcIDF.InputNames.HighLowText & vbTab & "LOW")
                        Dim lLowStart As String = atcIDF.InputNames.LowFlowSeasonStart
                        Dim lLowEnd As String = atcIDF.InputNames.LowFlowSeasonEnd
                        lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
                        lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
                    End If
                End If
                'lText.AppendLine(atcIDF.InputNames.HighLowText & vbTab & aArgs.GetValue(atcIDF.InputNames.HighLow))
            End If
            'The high/low option will dictate the starting and ending dates

            If aArgs.ContainsAttribute(InputNames.Logarithmic) Then
                Dim log As String = "YES"
                If Not aArgs.GetValue(InputNames.Logarithmic) Then log = "NO"
                lText.AppendLine(InputNames.Logarithmic & vbTab & log)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcIDF.InputNames.Logarithmic & vbTab & "YES")
            End If

            If aArgs.ContainsAttribute(InputNames.MultiNDayPlot) Then
                Dim mplot As String = "YES"
                If Not aArgs.GetValue(InputNames.MultiNDayPlot) Then mplot = "NO"
                lText.AppendLine(InputNames.MultiNDayPlot & vbTab & mplot)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcIDF.InputNames.MultiNDayPlot & vbTab & "NO")
            End If

            If aArgs.ContainsAttribute(InputNames.MultiStationPlot) Then
                Dim mplot As String = "YES"
                If Not aArgs.GetValue(InputNames.MultiStationPlot) Then mplot = "NO"
                lText.AppendLine(InputNames.MultiStationPlot & vbTab & mplot)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcIDF.InputNames.MultiStationPlot & vbTab & "NO")
            End If

            If aArgs.ContainsAttribute(InputNames.NDays) Then
                Dim lNDays As atcCollection = aArgs.GetValue(InputNames.NDays, Nothing)
                Dim lNdaysText As String = ""
                If lNDays IsNot Nothing Then
                    For Each lNday As Double In lNDays.Keys
                        If lNDays.ItemByKey(lNday) Then
                            lNdaysText &= Int(lNday) & ","
                        End If
                    Next
                    If Not String.IsNullOrEmpty(lNdaysText) Then lText.AppendLine(InputNames.NDays & vbTab & lNdaysText.TrimEnd(","))
                End If
            End If

            If aArgs.ContainsAttribute(InputNames.ReturnPeriods) Then
                Dim lRPs As atcCollection = aArgs.GetValue(InputNames.ReturnPeriods, Nothing)
                Dim lRPsText As String = ""
                If lRPs IsNot Nothing Then
                    For Each lRP As Double In lRPs.Keys
                        If lRPs.ItemByKey(lRP) Then
                            lRPsText &= lRP & ","
                        End If
                    Next
                    If Not String.IsNullOrEmpty(lRPsText) Then lText.AppendLine(InputNames.ReturnPeriodText & vbTab & lRPsText.TrimEnd(","))
                End If
            End If

            If lSetGlobal Then
                Dim lDatadir As String = aArgs.GetValue(InputNames.DataDir, "")
                If IO.Directory.Exists(lDatadir) Then
                    lText.AppendLine(InputNames.DataDir & vbTab & lDatadir)
                End If
            End If

            Dim lOutputDir As String = aArgs.GetValue(InputNames.OutputDir, "")
            Dim lOutputPrefix As String = aArgs.GetValue(InputNames.OutputPrefix, "")
            If Not String.IsNullOrEmpty(lOutputDir) Then lText.AppendLine(InputNames.OutputDir & vbTab & lOutputDir)
            If Not String.IsNullOrEmpty(lOutputPrefix) Then lText.AppendLine(InputNames.OutputPrefix & vbTab & lOutputPrefix)

            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("END SWSTAT")
            ElseIf lSetGlobal Then
                lText.AppendLine("END GLOBAL")
            End If
            Return lText.ToString()
        End Function

        Public Shared Function CalculateNDayValues(ByVal aDataGroup As atcTimeseriesGroup,
                                           ByVal aGroupArgs As atcDataAttributes,
                                           ByVal aNDayDbl() As Double,
                                           ByVal aReturnPeriodDbl() As Double,
                                           ByVal aHighFlag As Boolean) As Boolean

            'ByVal aOperationName As String, ByVal aReturnPeriods() As Double
            'for NDay calculation, no need for high/low setting
            Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            Dim lDataGroup As atcTimeseriesGroup = aDataGroup
            With aGroupArgs
                'lDataGroup = .GetValue("Timeseries", Nothing)
                If lDataGroup Is Nothing Then Exit Function
                For Each lTs As atcTimeseries In lDataGroup
                    lTs.Attributes.SetValueIfMissing("CalcEMA", True)
                Next
            End With

            Dim lCalcArgs As New atcDataAttributes
            With lCalcArgs
                .SetValue("Timeseries", lDataGroup)
                .SetValue(NDay, aNDayDbl)
                If aHighFlag Then
                    .SetValue(ReturnPeriod, aReturnPeriodDbl)
                Else
                    .SetValue("Return Period", clsIDFPlugin.ListDefaultArray("Return Period"))
                End If
            End With

            Dim lOperationName As String = "n-day high value"
            If Not aHighFlag Then
                lOperationName = "n-day low value"
            End If
            SetInputsForAnalysis(aGroupArgs, lCalcArgs, aHighFlag)
            Dim lNDayValuesCalculated As Boolean
            Try
                lNDayValuesCalculated = lCalculator.Open(lOperationName, lCalcArgs)
            Catch ex As Exception
                lNDayValuesCalculated = False
            Finally
                lCalculator.DataSets.Clear()
                lCalculator = Nothing
            End Try
            Return lNDayValuesCalculated
        End Function

        Public Shared Function CalculateNDayTser(ByVal aDataGroup As atcTimeseriesGroup,
                                             ByVal aGroupArgs As atcDataAttributes,
                                             ByVal aNDayDbl() As Double,
                                             ByVal aHighFlag As Boolean) As String
            Dim lTserListing As String = ""
            If (aNDayDbl IsNot Nothing AndAlso aNDayDbl.Length > 0) AndAlso aGroupArgs IsNot Nothing Then
                Dim lCalcArgs As New atcDataAttributes()

                Dim lHighLowText As String = "high"
                If Not aHighFlag Then
                    lHighLowText = "low"
                End If

                SetInputsForAnalysis(aGroupArgs, lCalcArgs, aHighFlag)

                Dim lFirstYear As Integer = lCalcArgs.GetValue(InputNames.StartYear)
                Dim lastYear As Integer = lCalcArgs.GetValue(InputNames.EndYear)
                Dim lStartMonth As Integer = lCalcArgs.GetValue(InputNames.StartMonth)
                Dim lEndMonth As Integer = lCalcArgs.GetValue(InputNames.EndMonth)
                Dim lStartDay As Integer = lCalcArgs.GetValue(InputNames.StartDay)
                Dim lEndDay As Integer = lCalcArgs.GetValue(InputNames.EndDay)

                Dim lRankedAnnual As atcTimeseriesGroup =
                   clsIDFPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup,
                                                                 aNDay:=aNDayDbl, aHighFlag:=aHighFlag,
                                                                 aFirstYear:=lFirstYear, aLastYear:=lastYear,
                                                                 aBoundaryMonth:=lStartMonth, aBoundaryDay:=lStartDay,
                                                                 aEndMonth:=lEndMonth, aEndDay:=lEndDay)

                If lRankedAnnual.Count > 0 Then
                    For Each lTS As atcTimeseries In lRankedAnnual
                        lTS.Attributes.SetValue("Units", "Common")
                    Next

                    Dim lList As New atcList.atcListForm
                    With lList
                        With .DateFormat
                            .IncludeDays = False
                            .IncludeHours = False
                            .IncludeMinutes = False
                            .IncludeMonths = False
                        End With
                        .Text = "N-Day " & lHighLowText & " Annual Time Series and Ranking"
                        .Initialize(lRankedAnnual, NDayAttributes(), True, , False) 'show value, but not show form
                        .DisplayValueAttributes = True
                        lTserListing = .ToString()
                        '.Icon = Me.Icon
                        .Close()
                    End With
                End If
            Else
                'Logger.Msg("Inputs are incomplete.")
            End If
            Return lTserListing
        End Function
        Public Shared Function SetInputsForAnalysis(ByVal aSetInputArgs As atcDataAttributes,
                                         ByRef aCalcArgs As atcDataAttributes,
                                         Optional ByVal aHighFlow As Boolean = True) As Boolean

            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            If aCalcArgs Is Nothing Then
                Return False
            End If
            Try
                With aCalcArgs
                    '.SetValue("Timeseries", lDataGroup)
                    '.SetValue("NDay", aNDayDbl)
                    '.SetValue("Return Period", aReturnPeriodDbl)
                    Dim logCheck As Boolean = True
                    Dim logObj As Object = aSetInputArgs.GetValue(Logarithmic)
                    Dim lType As Type = logObj.GetType()
                    If lType.Name.ToLower() = "string" Then
                        If String.Compare(logObj.ToString, "no", True) = 0 Then
                            logCheck = False
                        End If
                    ElseIf lType.Name.ToLower() = "Boolean" Then
                        If Not logObj Then
                            logCheck = False
                        End If
                    End If
                    .SetValue("LogFlg", logCheck)
                    Dim lStartYear As String = aSetInputArgs.GetValue(StartYear)
                    Dim lEndYear As String = aSetInputArgs.GetValue(EndYear)
                    If String.IsNullOrEmpty(lStartYear) OrElse String.IsNullOrEmpty(lEndYear) Then
                        Dim lDuration As String = aSetInputArgs.GetValue(IncludeYears, "")
                        If Not String.IsNullOrEmpty(lDuration) AndAlso lDuration.ToLower.StartsWith("all") Then
                            Dim lArr() As String = lDuration.Split(" ")
                            lStartDate = lArr(1)
                            lEndDate = lArr(3)
                            Dim lSDate As DateTime
                            Dim lEDate As DateTime
                            If DateTime.TryParse(lStartDate, lSDate) Then
                                .SetValue(StartYear, lSDate.Year)
                            End If
                            If DateTime.TryParse(lEndDate, lEDate) Then
                                .SetValue(EndYear, lEDate.Year)
                            End If
                        End If
                    Else
                        Dim lStartYearD As Integer
                        Dim lEndYearD As Integer
                        If Integer.TryParse(lStartYear, lStartYearD) Then
                            .SetValue(StartYear, lStartYearD) ' pFirstYear)
                        End If
                        If Integer.TryParse(lEndYear, lEndYearD) Then
                            .SetValue(EndYear, lEndYearD) ' pLastYear)
                        End If
                    End If
                End With

                Dim lOperationName As String = ""
                Dim lsMonth As Integer
                Dim lsDay As Integer
                Dim leMonth As Integer
                Dim leDay As Integer
                If aHighFlow Then
                    lsMonth = 10
                    lsDay = 1
                    leMonth = 9
                    leDay = 30
                    lStartDate = aSetInputArgs.GetValue(HighFlowSeasonStart, "")
                    lEndDate = aSetInputArgs.GetValue(HighFlowSeasonEnd, "")
                Else
                    lsMonth = 4
                    lsDay = 1
                    leMonth = 3
                    leDay = 31
                    lStartDate = aSetInputArgs.GetValue(LowFlowSeasonStart, "")
                    lEndDate = aSetInputArgs.GetValue(LowFlowSeasonEnd, "")
                End If

                With aCalcArgs
                    If aSetInputArgs.GetValue(StartMonth) IsNot Nothing AndAlso aSetInputArgs.GetValue(StartDay) IsNot Nothing Then
                        .SetValue(StartMonth, aSetInputArgs.GetValue(StartMonth)) 'pYearStartMonth)
                        .SetValue(StartDay, aSetInputArgs.GetValue(StartDay)) 'pYearStartDay)
                    Else
                        If Not String.IsNullOrEmpty(lStartDate) AndAlso lStartDate.IndexOf("/") >= 0 Then
                            Integer.TryParse(lStartDate.Substring(0, lStartDate.IndexOf("/")), lsMonth)
                            Integer.TryParse(lStartDate.Substring(lStartDate.IndexOf("/") + 1), lsDay)
                        End If
                        .SetValue(StartMonth, lsMonth)
                        .SetValue(StartDay, lsDay)
                    End If

                    If aSetInputArgs.GetValue(EndMonth) IsNot Nothing AndAlso aSetInputArgs.GetValue(EndDay) IsNot Nothing Then
                        .SetValue(EndMonth, aSetInputArgs.GetValue(EndMonth)) 'pYearStartMonth)
                        .SetValue(EndDay, aSetInputArgs.GetValue(EndDay)) 'pYearStartDay)
                    Else
                        If Not String.IsNullOrEmpty(lEndDate) AndAlso lEndDate.IndexOf("/") >= 0 Then
                            Integer.TryParse(lEndDate.Substring(0, lEndDate.IndexOf("/")), leMonth)
                            Integer.TryParse(lEndDate.Substring(lEndDate.IndexOf("/") + 1), leDay)
                        End If
                        .SetValue(EndMonth, leMonth)
                        .SetValue(EndDay, leDay)
                    End If
                End With
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Sub DoFrequencyGraph(ByVal aDirectory As String,
                                           ByVal aDataGroup As atcData.atcTimeseriesGroup,
                                           ByVal aInputArgs As atcDataAttributes,
                                           ByVal aNDaysDbl() As Double,
                                           ByVal aReturnPeriods() As Double,
                                           ByVal aHighFlag As Boolean)

            Dim lHighLowText As String = "High"
            If Not aHighFlag Then lHighLowText = "Low"
            Dim lGraphFilePrefix As String = "FreqGraph_" & lHighLowText

            'Calculate("n-day " & lFlowCondition & " value", clsSWSTATPlugin.ListDefaultArray("Return Period"))
            CalculateNDayValues(aDataGroup, aInputArgs, aNDaysDbl, clsIDFPlugin.ListDefaultArray(InputNames.ReturnPeriod), aHighFlag)
            Dim lGraphPlugin As New atcGraph.atcGraphPlugin
            Dim lGraphForm As atcGraph.atcGraphForm

            'Get station list
            Dim lStnList As New Generic.List(Of String)
            For Each lDataSet As atcTimeseries In aDataGroup
                Dim loc As String = lDataSet.Attributes.GetValue("STAID", "")
                If String.IsNullOrEmpty(loc) Then
                    loc = lDataSet.Attributes.GetValue("Location", "")
                    lDataSet.Attributes.SetValue("STAID", loc)
                End If
                If Not String.IsNullOrEmpty(loc) AndAlso Not lStnList.Contains(loc) Then
                    lStnList.Add(loc)
                End If
            Next
            'Get Nday list
            Dim lNDayList As New Generic.List(Of String)
            For Each lDataSet As atcTimeseries In aDataGroup
                For Each lAtt As atcData.atcDefinedValue In lDataSet.Attributes
                    If lAtt.Definition.Name.ToLower.Contains("daytimeseries") Then
                        If Not lNDayList.Contains(lAtt.Definition.Name) Then
                            lNDayList.Add(lAtt.Definition.Name)
                        End If
                    End If
                Next
            Next

            Dim lMNDayPlot As Boolean = True
            Dim lMNDayPlotSetting As String = aInputArgs.GetValue(InputNames.MultiNDayPlot, "")
            If lMNDayPlotSetting.ToLower() = "no" Then
                lMNDayPlot = False
            End If
            Dim lMStationPlot As Boolean = True
            Dim lMStationPlotSetting As String = aInputArgs.GetValue(InputNames.MultiStationPlot, "")
            If lMStationPlotSetting.ToLower() = "no" Then
                lMStationPlot = False
            End If

            If lMNDayPlot AndAlso lMStationPlot Then 'MultipleNDayPlots() And MultipleStationPlots()
                For Each lStaId As String In lStnList
                    For Each lNDayTimeseriesName As String In lNDayList
                        Dim lDataGroup As New atcData.atcTimeseriesGroup
                        For Each lDataset As atcTimeseries In aDataGroup
                            Dim loc As String = lDataset.Attributes.GetValue("STAID", "")
                            If String.IsNullOrEmpty(loc) Then
                                loc = lDataset.Attributes.GetValue("Location", "")
                                lDataset.Attributes.SetValue("STAID", loc)
                            End If
                            If loc = lStaId Then
                                Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
                                If lTs IsNot Nothing Then
                                    lDataGroup.Add(lTs)
                                End If
                            End If
                        Next
                        If lDataGroup.Count > 0 Then
                            lGraphForm = New atcGraph.atcGraphForm()
                            With lGraphForm
                                Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
                                .Grapher = lGraph
                                Dim lGraphName As String = lGraphFilePrefix & "_" & lStaId & "_" & lNDayTimeseriesName & ".png"
                                .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
                                .Close()
                            End With
                        End If
                        lDataGroup.Clear()
                        lDataGroup = Nothing
                    Next
                Next

                'For Each lDataSet As atcTimeseries In pDataGroup
                '    lGraphForm = lGraphPlugin.Show(New atcTimeseriesGroup(lDataSet), "Frequency")
                '    lGraphForm.Icon = Me.Icon
                'Next
            ElseIf lMNDayPlot Then
                For Each lNDayTimeseriesName As String In lNDayList
                    Dim lDataGroup As New atcData.atcTimeseriesGroup
                    For Each lDataset As atcTimeseries In aDataGroup
                        Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
                        If lTs IsNot Nothing Then
                            lDataGroup.Add(lTs)
                        End If
                    Next
                    If lDataGroup IsNot Nothing AndAlso lDataGroup.Count > 0 Then
                        'lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
                        'lGraphForm.Icon = Me.Icon
                        lGraphForm = New atcGraph.atcGraphForm()
                        With lGraphForm
                            Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
                            .Grapher = lGraph
                            Dim lGraphName As String = lGraphFilePrefix & "_" & lNDayTimeseriesName & ".png"
                            .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
                            .Close()
                        End With
                    End If
                    lDataGroup.Clear()
                    lDataGroup = Nothing
                Next
            ElseIf lMStationPlot Then
                Dim lDataGroup As New atcData.atcTimeseriesGroup()
                For Each lStaId As String In lStnList
                    'lDataGroup = aDataGroup.FindData("STAID", lStaId)
                    For Each lDataset As atcTimeseries In aDataGroup
                        Dim loc As String = lDataset.Attributes.GetValue("STAID", "")
                        If String.IsNullOrEmpty(loc) Then
                            loc = lDataset.Attributes.GetValue("Location", "")
                            lDataset.Attributes.SetValue("STAID", loc)
                        End If
                        If loc = lStaId Then
                            lDataGroup.Add(lDataset)
                        End If
                    Next

                    If lDataGroup.Count > 0 Then
                        'lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
                        'lGraphForm.Icon = Me.Icon
                        lGraphForm = New atcGraph.atcGraphForm()
                        With lGraphForm
                            Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
                            .Grapher = lGraph
                            Dim lGraphName As String = lGraphFilePrefix & "_" & lStaId & ".png"
                            .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
                            .Close()
                        End With
                    End If
                    lDataGroup.Clear()
                    'lDataGroup = Nothing
                Next
            Else
                'lGraphForm = lGraphPlugin.Show(aDataGroup, "Frequency")
                'lGraphForm.Icon = Me.Icon
                lGraphForm = New atcGraph.atcGraphForm()
                With lGraphForm
                    Dim lGraph As New atcGraph.clsGraphFrequency(aDataGroup, .ZedGraphCtrl)
                    .Grapher = lGraph
                    Dim lGraphName As String = lGraphFilePrefix & "_All.png"
                    .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
                    .Close()
                End With
            End If

            lNDayList.Clear()
            lNDayList = Nothing
            lStnList.Clear()
            lStnList = Nothing
        End Sub

        Public Shared Function DoFrequencyGrid(ByVal aDataGroup As atcTimeseriesGroup,
                                           ByVal aGroupArgs As atcDataAttributes,
                                           ByVal aNDayDbl() As Double,
                                           ByVal aReturnPeriodDbl() As Double,
                                           ByVal aHighFlag As Boolean) As String

            Try
                Dim lNDayValuesHighDone As Boolean = InputNames.CalculateNDayValues(aDataGroup, aGroupArgs, aNDayDbl, aReturnPeriodDbl, aHighFlag)
                Dim lFreqForm As New frmDisplayFrequencyGrid(aDataGroup:=aDataGroup,
                                                 aHigh:=aHighFlag,
                                                 aNday:=aNDayDbl,
                                                 aReturns:=aReturnPeriodDbl,
                                                 aShowForm:=False)
                Return lFreqForm.ToString() & vbCrLf & vbCrLf & "Export Report" & vbCrLf & vbCrLf & lFreqForm.CreateReport(True)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Shared Function CheckFreqGrid(ByVal aSource As atcFrequencyGridSource) As String
            Dim lblNote As String = ""
            'Dim pSource As atcFrequencyGridSource = New atcFrequencyGridSource(aDataGroup, aNday, aReturns)
            'If pSource.Columns < 3 Then
            '    lContinue = UserSpecifyAttributes()
            '    If lContinue Then
            '        pSource = New atcFrequencyGridSource(pDataGroup)
            '    End If
            'End If

            Dim lCouldNotComputeAny As Boolean = False
            For lRow As Integer = 0 To aSource.Rows - 1
                For lColumn As Integer = 0 To aSource.Columns - 1
                    If aSource.CellValue(lRow, lColumn).Equals(atcFrequencyGridSource.CouldNotComputeText) Then
                        lCouldNotComputeAny = True
                        Exit For
                    End If
                Next
                If lCouldNotComputeAny Then Exit For
            Next
            If lCouldNotComputeAny Then
                lblNote = "Error: Could not complete analysis for all values. Review flow data for missing or zero values."
            End If
            Return lblNote
        End Function

        Public Shared Sub GetNdayReturnPeriodAttributes(ByVal aInputs As atcDataAttributes,
                                                        ByRef aNDay() As Double,
                                                        ByRef aReturnPeriod() As Double)
            ReDim aNDay(0)
            ReDim aReturnPeriod(0)
            aNDay = Nothing
            aReturnPeriod = Nothing
            'When read in from spec file, the ndays and return periods are already the selected on in 
            'comma-delimited strings
            Dim listNDays As String = aInputs.GetValue(NDays, "")
            If Not String.IsNullOrEmpty(listNDays) Then
                Dim lNDaysText() As String = listNDays.Split(",")
                Dim lNDay As Double
                Dim lNDaysCol As New ArrayList
                For Each lNDayText As String In lNDaysText
                    If Double.TryParse(lNDayText, lNDay) Then
                        If Not lNDaysCol.Contains(lNDay) Then lNDaysCol.Add(lNDay)
                    End If
                Next
                ReDim aNDay(lNDaysCol.Count - 1)
                For d As Integer = 0 To lNDaysCol.Count - 1
                    aNDay(d) = lNDaysCol(d)
                Next
            End If
            'Dim lNDays As atcCollection = aInputs.GetValue(NDays, Nothing)
            'If lNDays IsNot Nothing Then
            '    Dim lNdaysCol As New ArrayList()
            '    For Each lNDay As Double In lNDays.Keys
            '        If lNDays.ItemByKey(lNDay) Then
            '            lNdaysCol.Add(lNDay)
            '        End If
            '    Next
            '    ReDim aNDay(lNdaysCol.Count - 1)
            '    For d As Integer = 0 To aNDay.Length - 1
            '        aNDay(d) = lNdaysCol(d)
            '    Next
            'End If

            Dim listRPs As String = aInputs.GetValue(ReturnPeriodText, "")
            If Not String.IsNullOrEmpty(listRPs) Then
                Dim lRPsText() As String = listRPs.Split(",")
                Dim lRP As Double
                Dim lRPsCol As New ArrayList
                For Each lRPText As String In lRPsText
                    If Double.TryParse(lRPText, lRP) Then
                        If Not lRPsCol.Contains(lRP) Then lRPsCol.Add(lRP)
                    End If
                Next
                ReDim aReturnPeriod(lRPsCol.Count - 1)
                For d As Integer = 0 To lRPsCol.Count - 1
                    aReturnPeriod(d) = lRPsCol(d)
                Next
            End If

            'Dim lRPs As atcCollection = aInputs.GetValue(ReturnPeriods, Nothing)
            'If lRPs IsNot Nothing Then
            '    Dim lRPsCol As New ArrayList()
            '    For Each lRP As Double In lRPs.Keys
            '        If lRPs.ItemByKey(lRP) Then
            '            lRPsCol.Add(lRP)
            '        End If
            '    Next
            '    ReDim aReturnPeriod(lRPsCol.Count - 1)
            '    For d As Integer = 0 To aReturnPeriod.Length - 1
            '        aReturnPeriod(d) = lRPsCol(d)
            '    Next
            'End If
        End Sub

        Public Shared Function TrendAnalysis(ByVal aDataGroup As atcTimeseriesGroup,
                                             ByVal aInputArgs As atcDataAttributes,
                                             ByVal aNDays() As Double,
                                             ByVal aHighFlow As Boolean) As String
            Dim lCalcArgs As New atcDataAttributes()
            'With lCalcArgs
            ' not needed as this is done inside computeRankedAnnualTimeseries
            '    .SetValue("Timeseries", aDataGroup)
            '    .SetValue(NDay, aNDays)
            'End With
            SetInputsForAnalysis(aInputArgs, lCalcArgs, aHighFlow)
            Dim lRankedAnnual As atcTimeseriesGroup =
                   clsIDFPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup,
                                                                 aNDay:=aNDays,
                                                                 aHighFlag:=aHighFlow,
                                                                 aFirstYear:=lCalcArgs.GetValue(InputNames.StartYear),
                                                                 aLastYear:=lCalcArgs.GetValue(InputNames.EndYear),
                                                                 aBoundaryMonth:=lCalcArgs.GetValue(InputNames.StartMonth),
                                                                 aBoundaryDay:=lCalcArgs.GetValue(InputNames.StartDay),
                                                                 aEndMonth:=lCalcArgs.GetValue(InputNames.EndMonth),
                                                                 aEndDay:=lCalcArgs.GetValue(InputNames.EndDay))
            Dim lTrendGridText As String = ""
            If lRankedAnnual.Count > 0 Then
                Dim lTrendAttributes As New Generic.List(Of String)
                Dim lDateFormat As New atcDateFormat
                With lDateFormat
                    .IncludeHours = False
                    .IncludeMinutes = False
                    .IncludeSeconds = False
                End With
                For Each lTS As atcTimeseries In lRankedAnnual
                    With lTS.Attributes
                        .SetValue("Original ID", lTS.OriginalParentID)
                        .SetValue("From", lDateFormat.JDateToString(lTS.Dates.Value(1)))
                        .SetValue("To", lDateFormat.JDateToString(lTS.Dates.Value(lTS.numValues)))
                        .SetValue("Not Used", .GetValue("Count Missing"))
                    End With
                Next
                Dim lTrendSource As New atcTimeseriesGridSource(lRankedAnnual, TrendAttributes(), True, True)
                'mnuViewValues.Checked, _
                'mnuFilterNoData.Checked)
                With lTrendSource
                    .AttributeValuesEditable = True 'mnuEditAtrributeValues.Checked
                    .DisplayValueAttributes = False 'mnuViewValueAttributes.Checked
                    .DateFormat = lDateFormat
                End With
                Dim lList As New atcList.atcListForm
                With lList
                    With .DateFormat
                        .IncludeDays = False
                        .IncludeHours = False
                        .IncludeMinutes = False
                        .IncludeMonths = False
                    End With
                    If aHighFlow Then
                        .Text = "Trend of High Annual Time Series and Statistics"
                    Else
                        .Text = "Trend of Low Annual Time Series and Statistics"
                    End If
                    .Initialize(lRankedAnnual, TrendAttributes(), , , False)
                    .SwapRowsColumns = True
                    '.Icon = Me.Icon
                    lTrendGridText = .ToString()
                End With
                'Return lTrendSource
            Else
                'Return Nothing
            End If
            Return lTrendGridText
        End Function
    End Class '}

#Region "DFLOW"
    Public Class InputNamesDFLOW
        Inherits Inputs
        Public Enum EDFLOWCategory
            Biological
            NonBiological
        End Enum
        Public Shared DFLOWCategoryText As String = "DFLOWCategory"
        Public Shared BiologicalDFLOW As String = "BiologicalDFLOW"

        Public Enum EBioDFlowParamIndex
            P0FlowAveragingPeriodDays = 0
            P1AverageYearsBetweenExcursions
            P2ExcursionClusterPeriodDays
            P3AverageExcursionsPerCluster
        End Enum

        Public Shared TextTrue As String = "Yes"
        Public Shared TextFalse As String = "No"
        Public Enum EBioDFlowType
            Acute_maximum_concentration
            Chronic_continuous_concentration
            Ammonia
            Custom
        End Enum
        Public Shared EBioDFLOWTypeAbbrev() As String = {"AcuteMC", "Chronic", "Ammonia"}

        Public Shared BioUseDefault As String = "Bio_Use_Default"
        Public Shared BioSelectedMethod As String = "Bio_Selected_Method"
        Public Shared BioAvgPeriod As String = "Bio_Flow_Averaging_Period_days"
        Public Shared BioReturnYears As String = "Bio_Return_Period_of_Excursions_years"
        Public Shared BioClusterDays As String = "Bio_Excursion_Clustering_Period_days"
        Public Shared BioNumExcrsnPerCluster As String = "Bio_Num_Excursions_Per_Cluster"

        Public Enum EDFlowType
            Hydrological
            Explicit_Flow_Value
            Flow_Percentile
            Custom
        End Enum
        Public Shared EDFlowTypeAbbrev() As String = {"Hydrologic", "Explicit_V", "Percentile"}

        Public Shared NBioSelectedMethod As String = "NBio_Selected_Method"
        Public Shared NBioAveragingPeriod As String = "Flow_Averaging_Period_days"
        Public Shared NBioReturnPeriod As String = "Return_Period_of_Excursions_years"
        Public Shared NBioExplicitFlow As String = "Explicit_flow_value"
        Public Shared NBioFlowPercentile As String = "Flow_percentile"

        Public Shared SelectedMethodText As String = "Selected"

        Public Shared Function GetAbbrevParamSetName(ByVal aParamSet As atcCollection) As String
            If aParamSet Is Nothing Then Return ""
            Dim lAbbrevParamSetName As String = ""
            For Each lParamKey As String In aParamSet.Keys
                If lParamKey.ToLower().StartsWith("type_") Then
                    Dim lIndStr As String = lParamKey.Substring(lParamKey.LastIndexOf("_") + 1)
                    If lParamKey.Contains(EBioDFlowType.Acute_maximum_concentration.ToString()) Then
                        lAbbrevParamSetName = EBioDFLOWTypeAbbrev(EBioDFlowType.Acute_maximum_concentration)
                    ElseIf lParamKey.Contains(EBioDFlowType.Chronic_continuous_concentration.ToString()) Then
                        lAbbrevParamSetName = EBioDFLOWTypeAbbrev(EBioDFlowType.Chronic_continuous_concentration)
                    ElseIf lParamKey.Contains(EBioDFlowType.Ammonia.ToString()) Then
                        lAbbrevParamSetName = EBioDFLOWTypeAbbrev(EBioDFlowType.Ammonia)
                    ElseIf lParamKey.Contains(EDFlowType.Hydrological.ToString()) Then
                        lAbbrevParamSetName = EDFlowTypeAbbrev(EDFlowType.Hydrological)
                    ElseIf lParamKey.Contains(EDFlowType.Explicit_Flow_Value.ToString()) Then
                        lAbbrevParamSetName = EDFlowTypeAbbrev(EDFlowType.Explicit_Flow_Value)
                    ElseIf lParamKey.Contains(EDFlowType.Flow_Percentile.ToString()) Then
                        lAbbrevParamSetName = EDFlowTypeAbbrev(EDFlowType.Flow_Percentile)
                    End If
                    If Not String.IsNullOrEmpty(lIndStr) AndAlso IsNumeric(lIndStr) Then
                        lAbbrevParamSetName &= "_" & lIndStr
                    End If
                    Exit For
                End If
            Next
            Return lAbbrevParamSetName
        End Function

        '{
        Public Shared Sub BuildInputSet(ByRef aSpecialSet As atcDataAttributes, ByVal aCommonSet As atcDataAttributes)
            If aSpecialSet Is Nothing Then
                aSpecialSet = New atcDataAttributes()
            End If
            If aCommonSet Is Nothing Then
                With aSpecialSet
                    If .GetValue(StartDay) Is Nothing Then .SetValue(StartDay, 1)
                    If .GetValue(StartMonth) Is Nothing Then .SetValue(StartMonth, 4)
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, 0)
                    If .GetValue(EndDay) Is Nothing Then .SetValue(EndDay, 31)
                    If .GetValue(EndMonth) Is Nothing Then .SetValue(EndMonth, 3)
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, 0)

                    If .GetValue(BioUseDefault) Is Nothing Then
                        .SetValue(BioUseDefault, True)
                    End If
                    If .GetValue(EBioDFlowType.Acute_maximum_concentration.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Acute_maximum_concentration, I)
                        Next
                        .SetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), lParams)
                    End If
                    If .GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Chronic_continuous_concentration, I)
                        Next
                        .SetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), lParams)
                    End If
                    If .GetValue(EBioDFlowType.Ammonia.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Ammonia, I)
                        Next
                        .SetValue(EBioDFlowType.Ammonia.ToString(), lParams)
                    End If
                    If .GetValue(BioSelectedMethod) Is Nothing Then
                        .SetValue(BioSelectedMethod, EBioDFlowType.Acute_maximum_concentration)
                    End If

                    If .GetValue(EDFlowType.Hydrological.ToString()) Is Nothing Then
                        Dim lParams() As Integer = {7, 10}
                        .SetValue(EDFlowType.Hydrological.ToString(), lParams)
                    End If
                    If .GetValue(EDFlowType.Explicit_Flow_Value.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                    End If
                    If .GetValue(EDFlowType.Flow_Percentile.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Flow_Percentile.ToString(), 0.1)
                    End If
                    If .GetValue(NBioSelectedMethod) Is Nothing Then
                        .SetValue(NBioSelectedMethod, EDFlowType.Hydrological)
                    End If
                End With
            Else
                With aSpecialSet
                    If .GetValue(OutputDir) Is Nothing Then .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                    If .GetValue(OutputPrefix) Is Nothing Then .SetValue(OutputPrefix, aCommonSet.GetValue(OutputPrefix, ""))
                    If .GetValue(DataDir) Is Nothing Then .SetValue(DataDir, aCommonSet.GetValue(DataDir, ""))

                    If .GetValue(StartDay) Is Nothing Then .SetValue(StartDay, aCommonSet.GetValue(StartDay, 1))
                    If .GetValue(EndDay) Is Nothing Then .SetValue(EndDay, aCommonSet.GetValue(EndDay, 31))
                    If .GetValue(StartMonth) Is Nothing Then .SetValue(StartMonth, aCommonSet.GetValue(StartMonth, 4))
                    If .GetValue(EndMonth) Is Nothing Then .SetValue(EndMonth, aCommonSet.GetValue(EndMonth, 3))
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, aCommonSet.GetValue(StartYear, 0))
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, aCommonSet.GetValue(EndYear, 0))

                    If .GetValue(BioUseDefault) Is Nothing Then
                        .SetValue(BioUseDefault, aCommonSet.GetValue(BioUseDefault, True))
                    End If
                    If .GetValue(EBioDFlowType.Acute_maximum_concentration.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Acute_maximum_concentration, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Chronic_continuous_concentration, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EBioDFlowType.Ammonia.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Ammonia.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Ammonia, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Ammonia.ToString(), lParamsCopy)
                    End If
                    If .GetValue(BioSelectedMethod) Is Nothing Then
                        .SetValue(BioSelectedMethod, aCommonSet.GetValue(BioSelectedMethod, EBioDFlowType.Acute_maximum_concentration))
                    End If

                    If .GetValue(EDFlowType.Hydrological.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EDFlowType.Hydrological.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(1) : lParams(0) = 7 : lParams(1) = 10
                        End If
                        Dim lParamsCopy(1) As Integer
                        For I As Integer = 0 To 1
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EDFlowType.Hydrological.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EDFlowType.Explicit_Flow_Value.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Explicit_Flow_Value.ToString(), aCommonSet.GetValue(EDFlowType.Explicit_Flow_Value, 1.0))
                    End If
                    If .GetValue(EDFlowType.Flow_Percentile.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Flow_Percentile.ToString(), aCommonSet.GetValue(EDFlowType.Flow_Percentile, 0.1))
                    End If
                    If .GetValue(NBioSelectedMethod) Is Nothing Then
                        .SetValue(NBioSelectedMethod, aCommonSet.GetValue(NBioSelectedMethod, EDFlowType.Hydrological))
                    End If

                    If .GetValue(OutputDir) Is Nothing Then
                        .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                    Else
                        Dim lOutputDir As String = .GetValue(OutputDir)
                        If String.IsNullOrEmpty(lOutputDir) OrElse Not IO.Directory.Exists(lOutputDir) Then
                            .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                        End If
                    End If
                End With
            End If
        End Sub '}

        Public Shared Function ParametersToText(ByVal aArgs As atcDataAttributes) As String
            If aArgs Is Nothing Then Return ""
            Dim loperation As String = aArgs.GetValue("Operation", "")
            Dim lgroupname As String = aArgs.GetValue("Group", "")
            Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

            Dim lText As New Text.StringBuilder()
            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("DFLOW")
                Dim lStationInfo As ArrayList = aArgs.GetValue(InputNames.StationsInfo)
                If lStationInfo IsNot Nothing Then
                    For Each lstation As String In lStationInfo
                        lText.AppendLine(lstation)
                    Next
                End If
            ElseIf lSetGlobal Then
                lText.AppendLine("GLOBAL")
            End If

            'Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
            'Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
            'Dim lDates(5) As Integer
            'J2Date(lStartDate, lDates)
            'lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
            'J2Date(lEndDate, lDates)
            'timcnv(lDates)
            'lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

            If Not lSetGlobal Then
                If aArgs.ContainsAttribute(InputNames.IncludeYears) Then
                    Dim lDuration As String = aArgs.GetValue(InputNames.IncludeYears)
                    If Not String.IsNullOrEmpty(lDuration) Then lText.AppendLine(InputNames.IncludeYears & vbTab & lDuration)
                End If
                If aArgs.ContainsAttribute(InputNames.StartYear) Then
                    Dim lStartYear As String = aArgs.GetValue(InputNames.StartYear, 0)
                    If Not String.IsNullOrEmpty(lStartYear) Then lText.AppendLine(InputNames.StartYear & vbTab & lStartYear)
                End If
                If aArgs.ContainsAttribute(InputNames.EndYear) Then
                    Dim lEndYear As String = aArgs.GetValue(InputNames.EndYear, 0)
                    If Not String.IsNullOrEmpty(lEndYear) Then lText.AppendLine(InputNames.EndYear & vbTab & lEndYear)
                End If
            End If
            Dim lStartMonth As Integer = aArgs.GetValue(InputNames.StartMonth, 4)
            lText.AppendLine(InputNames.StartMonth & vbTab & lStartMonth)
            Dim lStartDay As Integer = aArgs.GetValue(InputNames.StartDay, 1)
            lText.AppendLine(InputNames.StartDay & vbTab & lStartDay)
            Dim lEndMonth As String = aArgs.GetValue(InputNames.EndMonth, 3)
            lText.AppendLine(InputNames.EndMonth & vbTab & lEndMonth)
            Dim lEndDay As String = aArgs.GetValue(InputNames.EndDay, 31)
            lText.AppendLine(InputNames.EndDay & vbTab & lEndDay)

            'list all analysis methods
            Dim lMethodText As String = Param_Method(aArgs, EBioDFlowType.Acute_maximum_concentration, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EBioDFlowType.Chronic_continuous_concentration, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EBioDFlowType.Ammonia, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)

            lMethodText = Param_Method(aArgs, EDFlowType.Hydrological, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EDFlowType.Explicit_Flow_Value, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EDFlowType.Flow_Percentile, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)

            If lSetGlobal Then
                Dim lDatadir As String = aArgs.GetValue(InputNames.DataDir, "")
                If IO.Directory.Exists(lDatadir) Then
                    lText.AppendLine(InputNames.DataDir & vbTab & lDatadir)
                End If
            End If

            Dim lOutputDir As String = aArgs.GetValue(InputNames.OutputDir, "")
            Dim lOutputPrefix As String = aArgs.GetValue(InputNames.OutputPrefix, "")
            If Not String.IsNullOrEmpty(lOutputDir) Then lText.AppendLine(InputNames.OutputDir & vbTab & lOutputDir)
            If Not String.IsNullOrEmpty(lOutputPrefix) Then lText.AppendLine(InputNames.OutputPrefix & vbTab & lOutputPrefix)

            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("END DFLOW")
            ElseIf lSetGlobal Then
                lText.AppendLine("END GLOBAL")
            End If
            Return lText.ToString()
        End Function

        Private Shared Function Param_Method(ByVal aArgs As atcDataAttributes, ByVal aMethod As Integer, ByVal aBio As Boolean) As String
            Dim lMethodBlock As New Text.StringBuilder()
            lMethodBlock.AppendLine(Method)
            Dim lParams() As Integer = Nothing
            If aBio Then
                lMethodBlock.AppendLine(BiologicalDFLOW & vbTab & TextTrue)
                Dim lSelectedMethod As Integer = aArgs.GetValue(BioSelectedMethod, -1)
                Select Case aMethod
                    Case EBioDFlowType.Acute_maximum_concentration
                        lMethodBlock.Append("Type_" & EBioDFlowType.Acute_maximum_concentration.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                    Case EBioDFlowType.Chronic_continuous_concentration
                        lMethodBlock.Append("Type_" & EBioDFlowType.Chronic_continuous_concentration.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                    Case EBioDFlowType.Ammonia
                        lMethodBlock.Append("Type_" & EBioDFlowType.Ammonia.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Ammonia.ToString(), Nothing)
                End Select

                If lSelectedMethod >= 0 Then
                    Select Case lSelectedMethod
                        Case EBioDFlowType.Acute_maximum_concentration
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EBioDFlowType.Chronic_continuous_concentration
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EBioDFlowType.Ammonia
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                    End Select
                End If

                If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                    lMethodBlock.AppendLine(BioAvgPeriod & vbTab & lParams(0))
                    lMethodBlock.AppendLine(BioReturnYears & vbTab & lParams(1))
                    lMethodBlock.AppendLine(BioClusterDays & vbTab & lParams(2))
                    lMethodBlock.AppendLine(BioNumExcrsnPerCluster & vbTab & lParams(3))
                Else
                    Return "" 'no parameters specified for this bio dflow profile
                End If
            Else
                Dim lSetGlobal As Boolean = True
                Dim lOpn As String = aArgs.GetValue("Operation")
                If lOpn = "GroupSetParm" Then
                    lSetGlobal = False
                End If

                Dim lExFlowValue As Double
                Dim lFlowPct As Double
                lMethodBlock.AppendLine(BiologicalDFLOW & vbTab & TextFalse)
                Dim lSelectedMethod As Integer = aArgs.GetValue(NBioSelectedMethod, -1)
                Select Case aMethod
                    Case EDFlowType.Hydrological
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Hydrological.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Hydrological.ToString())
                            lParams = aArgs.GetValue(EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                        Else
                            Return ""
                        End If
                    Case EDFlowType.Explicit_Flow_Value
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Explicit_Flow_Value.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Explicit_Flow_Value.ToString())
                            lExFlowValue = aArgs.GetValue(EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                        Else
                            Return ""
                        End If
                    Case EDFlowType.Flow_Percentile
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Flow_Percentile.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Flow_Percentile.ToString())
                            lFlowPct = aArgs.GetValue(EDFlowType.Flow_Percentile.ToString(), 0.1)
                        Else
                            Return ""
                        End If
                End Select
                If lSelectedMethod >= 0 Then
                    Select Case lSelectedMethod
                        Case EDFlowType.Hydrological
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EDFlowType.Explicit_Flow_Value
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EDFlowType.Flow_Percentile
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                    End Select
                End If
                Select Case aMethod
                    Case EDFlowType.Hydrological
                        If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                            lMethodBlock.AppendLine(NBioAveragingPeriod & vbTab & lParams(0))
                            lMethodBlock.AppendLine(NBioReturnPeriod & vbTab & lParams(1))
                        End If
                    Case EDFlowType.Explicit_Flow_Value
                        lMethodBlock.AppendLine(NBioExplicitFlow & vbTab & lExFlowValue)
                    Case EDFlowType.Flow_Percentile
                        lMethodBlock.AppendLine(NBioFlowPercentile & vbTab & lFlowPct)
                End Select
            End If
            lMethodBlock.AppendLine("END " & Method)
            Return lMethodBlock.ToString()
        End Function
    End Class

    Public Class clsInteractiveDFLOW
        Public Enum EDFLOWPARAM
            BIOAcute
            BIOChronic
            BIOAmmonia
            BIOCustom
            NBIOAcute
            NBIOChronic
            NBIOCustom
            NBIOCustom1
            NBIOCustom2
            NBIOExplicitFlow
            NBIOFlowPCT
        End Enum

        Public DataGroup As atcTimeseriesGroup = Nothing
        Public Scenario As String
        Public ScenarioID As Integer
        Public ReportSrc As atcGridSource
        Public TypeBio As EDFLOWPARAM
        Public TypeNBio As EDFLOWPARAM

        Public ParamBio1FlowAvgDays As Integer
        Public ParamBio2YearsBetweenExcursion As Integer
        Public ParamBio3ExcursionClusterDays As Integer
        Public ParamBio4ExcursionPerCluster As Integer

        Public ParamNBioNDay As Integer
        Public ParamNBioReturn As Integer
        Public ParamNBioExpFlow As Double
        Public ParamNBioFlowPct As Double

        Public CalculateHM As Boolean = True

        Public ExcursionCountArray As New atcCollection()
        Public ExcursionsArray As New atcCollection()
        Public ClustersArray As New atcCollection()

        Public Function AugmentReport(Optional ByVal aScenarioName As String = "", Optional ByVal aScenarioID As Integer = 0) As atcControls.atcGridSource
            If ReportSrc Is Nothing Then Return Nothing

            Dim lAugReportSrc As New atcControls.atcGridSource()
            With lAugReportSrc
                .FixedRows = 1
                .Rows = 1
                .Columns = ReportSrc.Columns + 4 + 4 + 1
                Dim I As Integer
                For I = 0 To .Columns - 1
                    .CellValue(0, I) = ReportSrc.CellValue(0, I)
                Next
                I = ReportSrc.Columns
                .CellValue(0, I) = "B_NDay"
                .CellValue(0, I + 1) = "B_Return"
                .CellValue(0, I + 2) = "B_ClusterDays"
                .CellValue(0, I + 3) = "B_ExcPerCluster"
                .CellValue(0, I + 4) = "NB_NDay"
                .CellValue(0, I + 5) = "NB_Return"
                .CellValue(0, I + 6) = "NB_FlowValue"
                .CellValue(0, I + 7) = "NB_Flow%"
                '.CellValue(0, I + 8) = "HM"
                .CellValue(0, I + 8) = "ScenID"

                .Rows = 2
                Dim lSuperRowIndex As Integer = .Rows - 1
                For lRow As Integer = 1 To ReportSrc.Rows - 1
                    Dim lCol As Integer = 0
                    For lCol = 0 To ReportSrc.Columns - 1
                        .CellValue(.Rows - 1, lCol) = ReportSrc.CellValue(lRow, lCol)
                    Next
                    lCol = ReportSrc.Columns
                    .CellValue(lSuperRowIndex, lCol) = ParamBio1FlowAvgDays
                    .CellValue(lSuperRowIndex, lCol + 1) = ParamBio2YearsBetweenExcursion
                    .CellValue(lSuperRowIndex, lCol + 2) = ParamBio3ExcursionClusterDays
                    .CellValue(lSuperRowIndex, lCol + 3) = ParamBio4ExcursionPerCluster
                    If ParamNBioNDay > 0 Then
                        .CellValue(lSuperRowIndex, lCol + 4) = ParamNBioNDay
                        .CellValue(lSuperRowIndex, lCol + 5) = ParamNBioReturn
                    Else
                        .CellValue(lSuperRowIndex, lCol + 4) = "-"
                        .CellValue(lSuperRowIndex, lCol + 5) = "-"
                    End If
                    If ParamNBioExpFlow > 0 Then
                        .CellValue(lSuperRowIndex, lCol + 6) = ParamNBioExpFlow
                    Else
                        .CellValue(lSuperRowIndex, lCol + 6) = "-"
                    End If
                    If ParamNBioFlowPct > 0 Then
                        .CellValue(lSuperRowIndex, lCol + 7) = ParamNBioFlowPct
                    Else
                        .CellValue(lSuperRowIndex, lCol + 7) = "-"
                    End If
                    '.CellValue(lSuperRowIndex, lCol + 8) = GetTserHM(.CellValue(lSuperRowIndex, 0), .CellValue(lSuperRowIndex, 1))

                    If Not String.IsNullOrEmpty(aScenarioName) Then
                        Scenario = aScenarioName
                    End If
                    If aScenarioID > 0 Then 'AndAlso ScenarioID = 0
                        ScenarioID = aScenarioID
                    End If

                    If Not String.IsNullOrEmpty(Scenario) AndAlso ScenarioID > 0 Then
                        .CellValue(lSuperRowIndex, lCol + 8) = ScenarioID & "-" & Scenario
                    Else
                        If Not String.IsNullOrEmpty(Scenario) Then
                            .CellValue(lSuperRowIndex, lCol + 8) = Scenario
                        Else
                            .CellValue(lSuperRowIndex, lCol + 8) = ScenarioID
                        End If
                    End If
                    .Rows = .Rows + 1
                    lSuperRowIndex = .Rows - 1
                Next
            End With
            Return lAugReportSrc
        End Function

        ''' <summary>
        ''' Construct a atcGridSource to report the excursion result for a specific time series in the DataGroup
        ''' </summary>
        ''' <param name="aStnID">Streamflow gaging station ID (Location, e.g 00010123)</param>
        ''' <param name="aIndex">The index of a time series in the DataGroup collection</param>
        ''' <returns></returns>
        Public Function ExcursionReport(ByVal aStnID As String, Optional ByVal aIndex As Integer = -1) As atcGridSource
            Dim pDateFormat As New atcDateFormat()
            With pDateFormat
                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False
            End With
            If DataGroup Is Nothing OrElse
               DataGroup.Count = 0 OrElse
               ExcursionCountArray.Count = 0 OrElse
               ExcursionsArray.Count = 0 OrElse
               ClustersArray.Count = 0 Then
                Return Nothing
            End If
            Dim lTs As atcTimeseries = Nothing
            Dim lIndex As Integer = aIndex
            If lIndex >= 0 AndAlso lIndex < DataGroup.Count Then
                lTs = DataGroup(lIndex)
            ElseIf Not String.IsNullOrEmpty(aStnID) Then
                For lIndex = 0 To DataGroup.Count - 1
                    lTs = DataGroup(lIndex)
                    If lTs.Attributes.GetValue("Location") = aStnID Then
                        Exit For
                    End If
                Next
            End If
            If lTs Is Nothing Then Return Nothing

            Dim lExcursionCount As Double = ExcursionCountArray(lIndex)
            Dim lClusters As ArrayList = ClustersArray(lIndex)
            Dim lExcursions As ArrayList = ExcursionsArray(lIndex)

            Dim lagsExcursions As New atcControls.atcGridSource
            With lagsExcursions
                .Columns = 5
                .Rows = 1 + lExcursions.Count
                .FixedRows = 1
                .CellValue(0, 0) = "Cluster Start"
                .CellValue(0, 1) = "Excursions"
                .CellValue(0, 2) = "Period Start"
                .CellValue(0, 3) = "Duration"
                .CellValue(0, 4) = "Avg Excursion"
                Dim lColumn As Integer
                For lColumn = 0 To 4
                    .CellColor(0, lColumn) = System.Drawing.Color.White
                    .Alignment(0, lColumn) = atcControls.atcAlignment.HAlignCenter
                Next
            End With

            Dim lRow As Integer = 0
            Dim lExcursionIdx As Integer = 0
            Dim lFirstDate As Double = lTs.Attributes.GetValue("xBy start date") 'pTimeseriesGroup(pRow - 1).Attributes.GetValue("xBy start date")

            For Each lCluster As DFLOWCalcs.stCluster In lClusters
                If lRow = 0 Then
                    lRow = 1 ' First cluster is always invalid, so it gets skipped
                Else
                    lagsExcursions.CellValue(lRow, 0) = pDateFormat.JDateToString(lFirstDate + lCluster.Start)
                    lagsExcursions.CellValue(lRow, 1) = Sig2(lCluster.Excursions) & " "
                    lagsExcursions.Alignment(lRow, 1) = atcControls.atcAlignment.HAlignRight

                    Dim lNExc As Integer = 0
                    Dim lExcursion As DFLOWCalcs.stExcursion
                    Do
                        lExcursion = lExcursions(lRow)
                        With lExcursion
                            lagsExcursions.CellValue(lRow, 2) = pDateFormat.JDateToString(lFirstDate + .Start)
                            lagsExcursions.CellValue(lRow, 3) = .Finish - .Start + 1 & " "
                            lagsExcursions.CellValue(lRow, 4) = Format((.SumMag / (.Count) - 1), "Percent") & " "

                            lagsExcursions.Alignment(lRow, 3) = atcControls.atcAlignment.HAlignRight
                            lagsExcursions.Alignment(lRow, 4) = atcControls.atcAlignment.HAlignRight
                        End With

                        lNExc = lNExc + 1
                        lRow = lRow + 1
                    Loop Until lNExc >= lCluster.Events
                End If
            Next

            lagsExcursions.CellValue(lRow, 0) = "Total"
            lagsExcursions.CellValue(lRow, 1) = Sig2(lExcursionCount)
            lagsExcursions.Alignment(lRow, 1) = atcControls.atcAlignment.HAlignRight
            Return lagsExcursions
        End Function

        Public Shared Function Sig2(ByVal x As Double) As String
            If x >= 100 Then
                Sig2 = Format(x, "Scientific")
            ElseIf x >= 10 Then
                Sig2 = Format(x, "00.0")
            Else
                Sig2 = Format(x, "0.00")
            End If
        End Function
    End Class

    Public Class DFLOWReportUtil
        Public Shared ReportInfos As Generic.Dictionary(Of Info, String) = New Dictionary(Of Info, String) From
            {
        {Info.i_Version, "DFLOW Version"},
        {Info.i_BuildDate, "Build Date"},
        {Info.i_RunDateTime, "Run Date And Time"},
        {Info.i_Username, "Username"},
        {Info.i_Seasonal, "Seasonal Calculation"},
        {Info.i_SeasonStartDate, "Season Or Year Start"},
        {Info.i_SeasonEndDate, "Season Or Year End"},
        {Info.i_SeasonStartYear, "Start"},
        {Info.i_SeasonEndYear, "End"},
        {Info.i_YearsIncluded, "Years Included in Calculations"}
            }

        Public Enum Info
            i_Version
            i_BuildDate
            i_RunDateTime
            i_Username
            i_Seasonal
            i_SeasonStartDate
            i_SeasonEndDate
            i_SeasonStartYear
            i_SeasonEndYear
            i_YearsIncluded
        End Enum
    End Class
    Public Class DFLOWReportAscii

        '"DFLOW version 4.0" & vbCrLf & _
        Public Shared DFLOWDisclaimer As String =
        "USGS has conducted limited testing and review of this program." & vbCrLf &
        "Final review and approval is pending." & vbCrLf & vbCrLf

        Public Scenarios As atcCollection
        Public i_RunDateTime As String = ""
        Public i_Username As String = "User"
        Public i_Seasonal As Boolean = False
        Public i_SeasonStartDate As String = "" 'could be anything user like to use such as 1-Apr
        Public i_SeasonEndDate As String = "" 'could be anything user like to use such as 31-Mar
        Public i_SeasonStartYear As Integer
        Public i_SeasonEndYear As Integer
        Public i_Version As String = "4.1"
        Public i_BuildDate As String = ""
        Public i_YearsIncluded As Integer = 0
        Public i_DataGroup As atcTimeseriesGroup = Nothing

        Public Sub New(ByVal aScenarios As atcCollection)
            Scenarios = aScenarios
            If Scenarios Is Nothing Then
                Scenarios = New atcCollection()
            End If
        End Sub

        Public ReadOnly Property Header() As String
            Get
                Dim lfldpf As String = ":"
                Dim lHeader As String = "***DFLOW CALCULATION REPORT***" & vbCrLf & vbCrLf

                'lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_Version) & lfldpf & vbTab & i_Version & vbCrLf
                'lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_BuildDate) & lfldpf & vbTab & i_BuildDate & vbCrLf
                'lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_RunDateTime) & lfldpf & vbTab & i_RunDateTime & vbCrLf

                'lRept.Append(DFLOWDisclaimer) 'Add Message Here
                'lRept.AppendLine()
                'lRept.AppendLine("Program SWToolbox         U.S. GEOLOGICAL SURVEY             ") 'Seq " & lPageCount.ToString.PadLeft(5, "0"))
                'lRept.AppendLine("Ver. 1.0                 DFLOW (version " & i_Version & "        Run Date / Time")
                'lRept.AppendLine(i_BuildDate & "           based on USGS Program A193           " & System.DateTime.Now.ToString("M/d/yyyy h:mm tt"))
                'lRept.AppendLine()
                'lRept.AppendLine(" Notice -- Log-Pearson Type III or Pearson Type III distributions are for")
                'lRept.AppendLine("           preliminary computations. Users are responsible for assessment")
                'lRept.AppendLine("           and interpretation.")
                'lRept.AppendLine()
                'lRept.AppendLine(vbFormFeed)

                lHeader &= "Program SWToolbox         U.S. GEOLOGICAL SURVEY             Version 1.0" & vbCrLf
                lHeader &= "Analysis: DFLOW (version " & i_Version & ")" & vbCrLf
                lHeader &= "Run Date and Time: " & System.DateTime.Now.ToString("M/d/yyyy h:mm tt")
                'lHeader &= i_BuildDate & "           based on USGS Program A193           " & System.DateTime.Now.ToString("M/d/yyyy h:mm tt")
                lHeader &= vbCrLf
                lHeader &= vbCrLf
                lHeader &= vbFormFeed
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_Username) & lfldpf & vbTab & i_Username & vbCrLf & vbCrLf

                lHeader &= "***INPUTS***" & vbCrLf & vbCrLf
                lHeader &= "YEARS AND SEASONS" & vbCrLf & vbCrLf

                If i_Seasonal Then
                    lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_Seasonal) & "?" & vbTab & "Yes" & vbCrLf
                Else
                    lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_Seasonal) & "?" & vbTab & "No" & vbCrLf
                End If
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonStartDate) & lfldpf & vbTab & i_SeasonStartDate & vbCrLf
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonEndDate) & lfldpf & vbTab & i_SeasonEndDate & vbCrLf
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_YearsIncluded) & lfldpf & vbTab & i_YearsIncluded & vbCrLf
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonStartYear) & lfldpf & vbTab & i_SeasonStartYear & vbCrLf
                lHeader &= DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonEndYear) & lfldpf & vbTab & i_SeasonEndYear & vbCrLf & vbCrLf

                lHeader &= "STATIONS" & vbCrLf & vbCrLf
                Return lHeader
            End Get
        End Property

        Public Function StationReport() As String
            Dim lRpt As New System.Text.StringBuilder()
            Dim lTs As atcTimeseries = Nothing
            Dim lDateFormat As New atcDateFormat()
            With lDateFormat
                .IncludeDays = True
                .IncludeMonths = True
                .IncludeYears = True
                .IncludeHours = False
                .IncludeMinutes = False
                .DateOrder = atcDateFormat.DateOrderEnum.MonthDayYear
            End With
            Dim lYearsExcluded As atcCollection = Nothing
            For I As Integer = 0 To i_DataGroup.Count - 1
                lTs = i_DataGroup(I)
                'lYearsExcluded.Clear()
                lYearsExcluded = AnalysisYears(lTs)
                With lTs.Attributes
                    Dim lInputFilename As String = .GetValue("history 1")
                    Dim loc As String = .GetValue("Location")
                    Dim lDesc As String = .GetValue("Description")
                    Dim lStart As String = lDateFormat.JDateToString(.GetValue("start date"))
                    Dim lEnd As String = lDateFormat.JDateToString(.GetValue("end date"))
                    Dim lDaysInRecord As Integer = lTs.numValues
                    Dim lMissInRecord As Integer = .GetValue("count missing")
                    Dim lZeroInRecord As Integer = .GetValue("count zero")
                    Dim lNegaInRecord As Integer = .GetValue("Count negative")
                    If .GetValue("Point") Then
                        .SetValue("Point", False)
                    End If
                    lRpt.AppendLine("Station #" & I + 1)
                    lRpt.AppendLine("Gage:" & vbTab & "USGS " & loc & " " & lDesc)
                    lRpt.AppendLine("Input File Name:" & vbTab & lInputFilename)
                    lRpt.AppendLine("Period of Record Start:" & vbTab & lStart)
                    lRpt.AppendLine("Period of Record End:" & vbTab & lEnd)
                    lRpt.AppendLine("Days in Record:" & vbTab & lDaysInRecord)
                    lRpt.AppendLine("Zero Values:" & vbTab & lZeroInRecord)
                    lRpt.AppendLine("Missing Values:" & vbTab & lMissInRecord)
                    lRpt.AppendLine("Negative Values:" & vbTab & lNegaInRecord)
                    lRpt.AppendLine()
                    lRpt.AppendLine("***RESULTS: USGS " & loc & " " & lDesc & "***")
                    lRpt.AppendLine()
                End With
                lRpt.AppendLine("BIOLOGICALLY BASED CRITICAL FLOWS")
                lRpt.AppendLine()
                lRpt.AppendLine("Flow Statistic" & vbTab & "Flow Value" & vbTab & "Percentile" & vbTab & "x-day avg. excur. per 3 yr.")
                Dim lAttName As String
                For Each lAtt As atcDefinedValue In lTs.Attributes
                    lAttName = lAtt.Definition.Name
                    If lAttName.StartsWith("BIOFLOW-") Then
                        lRpt.AppendLine(lAttName.Substring("BIOFLOW-".Length) & vbTab & lAtt.Value)
                    End If
                Next
                'For J As Integer = 0 To Scenarios.Count - 1
                '    Dim lScen As clsInteractiveDFLOW = Scenarios.ItemByIndex(J)
                '    Dim lBioPeriod As Integer = lScen.ParamBio1FlowAvgDays
                '    Dim lBioReturn As Integer = lScen.ParamBio2YearsBetweenExcursion
                '    Dim lxByKey As String = lBioPeriod & "B" & lBioReturn
                '    Dim lxBy As Double = lTs.Attributes.GetValue(lxByKey)
                '    Dim lxByPct As Double = lTs.Attributes.GetValue(lxByKey & "%")
                '    Dim lxByExc As Integer = lTs.Attributes.GetValue(lxByKey & "Exc")
                '    lRpt.AppendLine(lxByKey & vbCrLf & lxBy & vbCrLf & lxByPct & vbCrLf & lxByExc)
                'Next J
                lRpt.AppendLine()

                lRpt.AppendLine("NON-BIOLOGICALLY BASED CRITICAL FLOWS")
                lRpt.AppendLine()
                lRpt.AppendLine("Flow Statistic" & vbTab & "Flow Value" & vbTab & "Percentile" & vbTab & "1-day excur. per 3 yr.")
                Dim lHMeanrecord As String = ""
                For Each lAtt As atcDefinedValue In lTs.Attributes
                    lAttName = lAtt.Definition.Name
                    If lAttName.StartsWith("NONBIOFLOW-") Then
                        If lAttName.Contains("Harmonic") Then
                            lHMeanrecord = lAttName.Substring("NONBIOFLOW-".Length) & vbTab & lAtt.Value
                        Else
                            lRpt.AppendLine(lAttName.Substring("NONBIOFLOW-".Length) & vbTab & lAtt.Value)
                        End If
                    End If
                Next
                If Not String.IsNullOrEmpty(lHMeanrecord) Then
                    lRpt.AppendLine(lHMeanrecord)
                End If
                lRpt.AppendLine()

                'lRpt.AppendLine("EXCURSION ANALYSES FOR BIOLOGICALLY BASED CRITICAL FLOWS")
                'lRpt.AppendLine()
                'go through every scenario for a given dataset
                Dim lScen As clsInteractiveDFLOW = Scenarios.ItemByIndex(0)
                Dim lxByKey As String = lScen.ParamBio1FlowAvgDays & "B" & lScen.ParamBio2YearsBetweenExcursion
                Dim lGridSrc As atcGridSource = lScen.ExcursionReport("", I)
                If lGridSrc IsNot Nothing Then
                    lRpt.AppendLine("EXCURSION ANALYSES FOR BIOLOGICALLY BASED CRITICAL FLOWS (" & lxByKey & ")")
                    lRpt.AppendLine(lGridSrc.ToString())
                    lRpt.AppendLine()
                    lGridSrc.Rows = 1
                    lGridSrc.Columns = 1
                End If

                Dim lScenxByKey As String = ""
                For J As Integer = 1 To Scenarios.Count - 1
                    lScen = Scenarios.ItemByIndex(J)
                    With lScen
                        lScenxByKey = .ParamBio1FlowAvgDays & "B" & .ParamBio2YearsBetweenExcursion
                        If lScenxByKey <> lxByKey Then
                            lGridSrc = .ExcursionReport("", I)
                            If lGridSrc IsNot Nothing Then
                                lRpt.AppendLine("EXCURSION ANALYSES FOR BIOLOGICALLY BASED CRITICAL FLOWS (" & lScenxByKey & ")")
                                lRpt.AppendLine(lGridSrc.ToString())
                                lRpt.AppendLine()
                                lGridSrc.Rows = 1
                                lGridSrc.Columns = 1
                            End If
                            lxByKey = .ParamBio1FlowAvgDays & "B" & .ParamBio2YearsBetweenExcursion
                        End If
                    End With
                Next
                lRpt.AppendLine()
                lRpt.AppendLine("***YEARS EXCLUDED FROM ANALYSIS***")
                'ToDo: list excluded years???
                For Each lYear As Integer In lYearsExcluded
                    lRpt.AppendLine(lYear)
                Next
                lRpt.AppendLine()
                lRpt.AppendLine("*** END OF REPORT FOR STATION #" & I + 1 & " ***")
                lRpt.AppendLine()
            Next 'dataset

            lRpt.AppendLine()
            lRpt.AppendLine("END")
            Return lRpt.ToString()
        End Function

        ''' <summary>
        ''' This internal routine will get the list of years included or excluded by the analysis
        ''' </summary>
        ''' <param name="aTs">The target streamflow record that is under analysis</param>
        ''' <param name="aIncluded">Yes, get years included or No, get years excluded by the analysis</param>
        ''' <returns>an atcCollection of integer years</returns>
        Private Function AnalysisYears(ByVal aTs As atcTimeseries, Optional ByVal aIncluded As Boolean = False) As atcCollection
            If aTs Is Nothing OrElse aTs.Dates Is Nothing OrElse aTs.Values Is Nothing Then Return Nothing
            Dim lYears As New atcCollection()
            Dim lSYear As Integer = aTs.Attributes.GetValue("xBy start year", -99)
            Dim lEYear As Integer = aTs.Attributes.GetValue("xBy end year", -99)
            If lSYear > 0 AndAlso lEYear > 0 AndAlso lSYear < lEYear Then
                'figure out years not included in the analysis
                Dim lDates(5) As Integer
                Dim lSDate As Double = aTs.Dates.Value(0)
                Dim lEDate As Double = aTs.Dates.Value(aTs.numValues)
                If aIncluded Then
                    J2Date(lSDate, lDates)
                    Dim lYearStart As Integer = lDates(0)
                    J2Date(lEDate, lDates)
                    Dim lYearEnd As Integer = lDates(0)
                    For I As Integer = lYearStart To lYearEnd
                        If I >= lSYear AndAlso I <= lEYear Then
                            lYears.Add(I)
                        End If
                    Next
                Else
                    J2Date(lSDate, lDates)
                    Dim lYearStart As Integer = lDates(0)
                    While lYearStart < lSYear
                        lYears.Add(lYearStart)
                        lYearStart += 1
                    End While
                    J2Date(lEDate, lDates)
                    Dim lYearEnd As Integer = lDates(0)
                    lEYear += 1
                    While lEYear <= lYearEnd
                        lYears.Add(lEYear)
                        lEYear += 1
                    End While
                End If
            End If

            Return lYears
        End Function

        ''' <summary>
        ''' this routine will create a new atcGridSource for individual station's xBy and xQy flow information
        ''' </summary>
        ''' <param name="aStnID">a station id, e.g. 12490000</param>
        ''' <returns>a new atcGridSource</returns>
        Public Function StationReport(ByVal aStnID As String) As atcGridSource
            If String.IsNullOrEmpty(aStnID) Then Return Nothing
            If i_DataGroup Is Nothing OrElse i_DataGroup.Count = 0 Then Return Nothing
            Dim lTs As atcTimeseries = Nothing
            For I As Integer = 0 To i_DataGroup.Count - 1
                lTs = i_DataGroup.ItemByIndex(I)
                If lTs.Attributes.GetValue("Location") = aStnID Then
                    Exit For
                End If
            Next
            If lTs Is Nothing Then Return Nothing
            Dim lYears As atcCollection = AnalysisYears(lTs, True)

            Dim lNewSrc As New atcGridSource()
            With lNewSrc
                .FixedRows = 7
                .Columns = 4
                .CellValue(0, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_Seasonal) & "?"
                If i_Seasonal Then
                    .CellValue(0, 1) = "Yes"
                Else
                    .CellValue(0, 1) = "No"
                End If
                .CellValue(1, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonStartDate)
                .CellValue(1, 1) = i_SeasonStartDate
                .CellValue(2, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonEndDate)
                .CellValue(2, 1) = i_SeasonEndDate
                .CellValue(3, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_YearsIncluded)
                .CellValue(3, 1) = lYears.ItemByIndex(0) & "~" & lYears.ItemByIndex(lYears.Count - 1)
                .CellValue(4, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonStartYear)
                If lYears.ItemByIndex(0) <> i_SeasonStartYear Then
                    'This is when user picked Oct 1 as starting date, the routine auto set it back by one for water year
                    .CellValue(4, 1) = lYears.ItemByIndex(0)
                Else
                    .CellValue(4, 1) = i_SeasonStartYear
                End If
                .CellValue(5, 0) = DFLOWReportUtil.ReportInfos.Item(DFLOWReportUtil.Info.i_SeasonEndYear)
                If i_SeasonEndYear > 1000 Then
                    .CellValue(5, 1) = i_SeasonEndYear
                Else
                    Dim ldate(5) As Integer
                    J2Date(lTs.Dates.Value(lTs.numValues), ldate)
                    .CellValue(5, 1) = ldate(0).ToString()
                End If
                .CellValue(6, 0) = "Flow Statistic"
                .CellValue(6, 1) = "Flow Value"
                .CellValue(6, 2) = "Percentile"
                .CellValue(6, 3) = "x-day avg. Excur. per 3 yr."

                '.Rows = 1
                Dim lAttName As String
                Dim lArr() As String
                Dim lRow As Integer
                For Each lAtt As atcDefinedValue In lTs.Attributes
                    If lAtt.Value Is Nothing Then Continue For

                    lAttName = lAtt.Definition.Name
                    If lAttName.StartsWith("BIOFLOW-") Then
                        lArr = lAtt.Value.ToString().Split(vbTab)
                        If lArr.Length <> 3 Then Continue For
                        lRow = .Rows
                        .CellValue(lRow, 0) = lAttName.Substring("BIOFLOW-".Length)
                        Dim lfval As Double
                        If Double.TryParse(lArr(0), lfval) Then
                            .CellValue(lRow, 1) = DoubleToString(lfval)
                        Else
                            .CellValue(lRow, 1) = "N/A"
                        End If
                        .CellValue(lRow, 2) = lArr(1)
                        .CellValue(lRow, 3) = lArr(2)
                    End If
                Next

                lRow = .Rows
                .CellValue(lRow, 0) = ""
                .CellValue(lRow, 1) = ""
                .CellValue(lRow, 2) = ""
                .CellValue(lRow, 3) = ""
                lRow = .Rows
                .CellValue(lRow, 0) = "Flow Statistic"
                .CellValue(lRow, 1) = "Flow Value"
                .CellValue(lRow, 2) = "Percentile"
                .CellValue(lRow, 3) = "1-day Excur. per 3 yr."
                .CellEditable(lRow, 0) = False
                .CellEditable(lRow, 1) = False
                .CellEditable(lRow, 2) = False
                .CellEditable(lRow, 3) = False
                .CellColor(lRow, 0) = Drawing.Color.LightGray
                .CellColor(lRow, 1) = Drawing.Color.LightGray
                .CellColor(lRow, 2) = Drawing.Color.LightGray
                .CellColor(lRow, 3) = Drawing.Color.LightGray

                Dim lHMeanrecord As atcDefinedValue = Nothing
                Dim lHMeanAdjrecord As atcDefinedValue = Nothing
                For Each lAtt As atcDefinedValue In lTs.Attributes
                    If lAtt.Value Is Nothing Then Continue For

                    lAttName = lAtt.Definition.Name
                    If lAttName.StartsWith("NONBIOFLOW-") Then
                        lArr = lAtt.Value.ToString().Split(vbTab)
                        If lArr.Length <> 3 Then Continue For
                        If lAttName.Contains("Harmonic Mean Adj") Then
                            lHMeanAdjrecord = lAtt 'lAttName.Substring("NONBIOFLOW-".Length) & vbTab & lAtt.Value
                        ElseIf lAttName.Contains("Harmonic Mean") Then
                            lHMeanrecord = lAtt 'lAttName.Substring("NONBIOFLOW-".Length) & vbTab & lAtt.Value
                        Else
                            lRow = .Rows
                            .CellValue(lRow, 0) = lAttName.Substring("NONBIOFLOW-".Length)
                            Dim lfval As Double
                            If Double.TryParse(lArr(0), lfval) Then
                                .CellValue(lRow, 1) = DoubleToString(lfval,,,,, 5)
                            Else
                                .CellValue(lRow, 1) = "N/A"
                            End If
                            .CellValue(lRow, 2) = lArr(1)
                            .CellValue(lRow, 3) = lArr(2)
                        End If
                    End If
                Next
                If lHMeanrecord IsNot Nothing Then
                    lArr = lHMeanrecord.Value.ToString().Split(vbTab)
                    lRow = .Rows
                    .CellValue(lRow, 0) = lHMeanrecord.Definition.Name.Substring("NONBIOFLOW-".Length)
                    Dim lfval As Double
                    If Double.TryParse(lArr(0), lfval) Then
                        .CellValue(lRow, 1) = DoubleToString(lfval,,,,, 5)
                    Else
                        .CellValue(lRow, 1) = "N/A"
                    End If
                    .CellValue(lRow, 2) = lArr(1)
                    .CellValue(lRow, 3) = lArr(2)
                End If
                If lHMeanAdjrecord IsNot Nothing Then
                    lArr = lHMeanAdjrecord.Value.ToString().Split(vbTab)
                    lRow = .Rows
                    .CellValue(lRow, 0) = "Harmonic Mean, Adjusted" 'lHMeanAdjrecord.Definition.Name.Substring("NONBIOFLOW-".Length)
                    Dim lfval As Double
                    If Double.TryParse(lArr(0), lfval) Then
                        .CellValue(lRow, 1) = DoubleToString(lfval,,,,, 5)
                    Else
                        .CellValue(lRow, 1) = "N/A"
                    End If
                    .CellValue(lRow, 2) = lArr(1)
                    .CellValue(lRow, 3) = lArr(2)
                End If
            End With
            Return lNewSrc
        End Function

        Public Function Save(ByVal aFile As String) As Boolean
            Dim lSaveGood As Boolean = True
            Dim lSW As System.IO.StreamWriter = Nothing
            Try

            Catch ex As Exception
                lSaveGood = False
            Finally
                If lSW IsNot Nothing Then
                    lSW.Close()
                    lSW = Nothing
                End If
            End Try
            Return lSaveGood
        End Function
    End Class
#End Region
End Module
