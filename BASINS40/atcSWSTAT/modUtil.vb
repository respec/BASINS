Imports atcData
Imports atcUtility

Public Module modUtil
    ''' <summary>
    ''' Holds a set of inputs' names for batch run
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InputNames
        Public Shared HighLow As String = "HighOrLow"
        Public Shared HighLowText As String = "FLOWCONDITION"
        Public Shared Logarithmic As String = "LOGARITHMIC"
        Public Shared ReturnPeriod As String = "Return Period"
        Public Shared ReturnPeriods As String = "Return Periods"
        Public Shared ReturnPeriodText As String = "RECURRENCEINTERVAL"
        Public Shared NDay As String = "NDay"
        Public Shared NDays As String = "NDays"
        Public Shared IncludeYears As String = "IncludeYears"
        Public Shared StartYear As String = "FirstYear" '"STARTYEAR"
        Public Shared EndYear As String = "LastYear" '"ENDYEAR"
        Public Shared StartMonth As String = "BoundaryMonth" '"StartMonth"
        Public Shared StartDay As String = "BoundaryDay" '"StartDay"
        Public Shared EndMonth As String = "EndMonth"
        Public Shared EndDay As String = "EndDay"
        Public Shared HighFlowSeasonStart As String = "HIGHFLOWSEASONSTART" '10/01
        Public Shared HighFlowSeasonEnd As String = "HIGHFLOWSEASONEND"     '09/30
        Public Shared LowFlowSeasonStart As String = "LOWFLOWSEASONSTART"   '04/01
        Public Shared LowFlowSeasonEnd As String = "LOWFLOWSEASONEND"       '03/31

        Public Shared MultiNDayPlot As String = "MultipleNDayPlots"
        Public Shared MultiStationPlot As String = "MultipleStationPlots"
        Public Shared StationsInfo As String = "StationsInfo"

        Public Shared Streamflow As String = "Streamflow"

        Public Shared Method As String = "METHOD"
        Public Shared Methods As String = "Methods"
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

        Public Shared OutputDir As String = "OUTPUTDIR"
        Public Shared OutputPrefix As String = "OutputPrefix"
        Public Shared DataDir As String = "DataDir"

        Public Shared Function BasicAttributes() As Generic.List(Of String)
            Dim lBasicAttributes As New Generic.List(Of String)
            With lBasicAttributes
                .Add("ID")
                .Add("Min")
                .Add("Max")
                .Add("Mean")
                .Add("Standard Deviation")
                .Add("Count")
                .Add("Count Missing")
            End With
            Return lBasicAttributes
        End Function

        Public Shared Function NDayAttributes() As Generic.List(Of String)
            Dim lNDayAttributes As New Generic.List(Of String)
            With lNDayAttributes
                .Add("STAID")
                .Add("STANAM")
                .Add("Constituent")
            End With
            Return lNDayAttributes
        End Function

        Public Shared Function TrendAttributes() As Generic.List(Of String)
            Dim lTrendAttributes As New Generic.List(Of String)
            With lTrendAttributes
                .Add("Original ID")
                .Add("KENTAU")
                .Add("KENPLV")
                .Add("KENSLPL")
                .Add("From")
                .Add("To")
                .Add("Count")
                .Add("Not Used")
                .Add("Min")
                .Add("Max")
                .Add("Constituent")
                .Add("STAID")
            End With
            Return lTrendAttributes
        End Function

        Public Shared Sub BuildInputSet(ByRef aSpecialSet As atcDataAttributes, ByVal aCommonSet As atcDataAttributes)
            If aSpecialSet Is Nothing Then
                aSpecialSet = New atcDataAttributes()
            End If
            Dim lstNDayDefault() As Double = clsSWSTATPlugin.ListDefaultArray(NDay)
            Dim lstRPsDefault() As Double = clsSWSTATPlugin.ListDefaultArray(ReturnPeriod)
            Dim lNDays As atcUtility.atcCollection
            Dim lRPs As atcUtility.atcCollection
            If aCommonSet Is Nothing Then
                With aSpecialSet
                    If .GetValue(HighLow) Is Nothing Then .SetValue(HighLow, "Low")
                    If .GetValue(Logarithmic) Is Nothing Then .SetValue(Logarithmic, True)
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, "")
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, "")
                    'If .GetValue(MultiNDayPlot) Is Nothing Then .SetValue(MultiNDayPlot, False)
                    'If .GetValue(MultiStationPlot) Is Nothing Then .SetValue(MultiStationPlot, False)
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
        Public Shared Function BuildStationsInfo(ByVal aDataGroup As atcTimeseriesGroup) As ArrayList
            Dim lStationsInfo As New atcUtility.atcCollection()
            Dim lStationInfo As String = ""
            Dim loc As String
            Dim lDA As String
            Dim lFrom As String
            For Each lTser As atcTimeseries In aDataGroup
                With lTser.Attributes
                    loc = .GetValue("Location")
                    lDA = .GetValue("Drainage Area", "")
                    lFrom = .GetValue("History 1")
                    If Not String.IsNullOrEmpty(lFrom) Then
                        lFrom = lFrom.Substring("read from ".Length)
                    End If

                    lStationInfo = "Station" & vbTab & loc & "," & lDA & "," & lFrom
                    If Not lStationsInfo.Keys.Contains(loc) Then
                        lStationsInfo.Add(loc, lStationInfo)
                    End If
                End With
            Next
            Return lStationsInfo
        End Function

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
            If aArgs.ContainsAttribute(atcSWSTAT.modUtil.InputNames.Method) Then
                Dim lMethods As ArrayList = aArgs.GetValue(atcSWSTAT.modUtil.InputNames.Method)
                For Each lMethod As atcSWSTAT.modUtil.InputNames.ITAMethod In lMethods
                    Select Case lMethod
                        Case atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID
                            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID.ToString())
                        Case atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH
                            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
                        Case atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES
                            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
                        Case atcSWSTAT.InputNames.ITAMethod.TRENDLIST
                            lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.TRENDLIST.ToString())
                    End Select
                Next
            ElseIf lSetGlobal Then
                lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUECYGRID.ToString())
                lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.FREQUENCYGRAPH.ToString())
                lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.NDAYTIMESERIES.ToString())
                lText.AppendLine(atcSWSTAT.InputNames.Method & vbTab & atcSWSTAT.InputNames.ITAMethod.TRENDLIST.ToString())
            End If

            If lSetGlobal Then
                lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "HIGH")
                Dim lHighStart As String = atcSWSTAT.InputNames.HighFlowSeasonStart
                Dim lHighEnd As String = atcSWSTAT.InputNames.HighFlowSeasonEnd
                lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
                lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
                lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "LOW")
                Dim lLowStart As String = atcSWSTAT.InputNames.LowFlowSeasonStart
                Dim lLowEnd As String = atcSWSTAT.InputNames.LowFlowSeasonEnd
                lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
                lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
            ElseIf aArgs.ContainsAttribute(atcSWSTAT.InputNames.HighLow) Then
                Dim lCondition As String = aArgs.GetValue(atcSWSTAT.InputNames.HighLow)
                If Not String.IsNullOrEmpty(lCondition) Then
                    If lCondition.ToLower().Contains("high") Then
                        lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "HIGH")
                        Dim lHighStart As String = atcSWSTAT.InputNames.HighFlowSeasonStart
                        Dim lHighEnd As String = atcSWSTAT.InputNames.HighFlowSeasonEnd
                        lText.AppendLine(lHighStart & vbTab & aArgs.GetValue(lHighStart, ""))
                        lText.AppendLine(lHighEnd & vbTab & aArgs.GetValue(lHighEnd, ""))
                    ElseIf lCondition.ToLower().Contains("low") Then
                        lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & "LOW")
                        Dim lLowStart As String = atcSWSTAT.InputNames.LowFlowSeasonStart
                        Dim lLowEnd As String = atcSWSTAT.InputNames.LowFlowSeasonEnd
                        lText.AppendLine(lLowStart & vbTab & aArgs.GetValue(lLowStart, ""))
                        lText.AppendLine(lLowEnd & vbTab & aArgs.GetValue(lLowEnd, ""))
                    End If
                End If
                'lText.AppendLine(atcSWSTAT.InputNames.HighLowText & vbTab & aArgs.GetValue(atcSWSTAT.InputNames.HighLow))
            End If
            'The high/low option will dictate the starting and ending dates

            If aArgs.ContainsAttribute(InputNames.Logarithmic) Then
                Dim log As String = "YES"
                If Not aArgs.GetValue(InputNames.Logarithmic) Then log = "NO"
                lText.AppendLine(InputNames.Logarithmic & vbTab & log)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcSWSTAT.InputNames.Logarithmic & vbTab & "YES")
            End If

            If aArgs.ContainsAttribute(InputNames.MultiNDayPlot) Then
                Dim mplot As String = "YES"
                If Not aArgs.GetValue(InputNames.MultiNDayPlot) Then mplot = "NO"
                lText.AppendLine(InputNames.MultiNDayPlot & vbTab & mplot)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcSWSTAT.InputNames.MultiNDayPlot & vbTab & "NO")
            End If

            If aArgs.ContainsAttribute(InputNames.MultiStationPlot) Then
                Dim mplot As String = "YES"
                If Not aArgs.GetValue(InputNames.MultiStationPlot) Then mplot = "NO"
                lText.AppendLine(InputNames.MultiStationPlot & vbTab & mplot)
                'ElseIf lSetGlobal Then
                '    lText.AppendLine(atcSWSTAT.InputNames.MultiStationPlot & vbTab & "NO")
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

        Public Shared Function CalculateNDayValues(ByVal aDataGroup As atcTimeseriesGroup, _
                                           ByVal aGroupArgs As atcDataAttributes, _
                                           ByVal aNDayDbl() As Double, _
                                           ByVal aReturnPeriodDbl() As Double, _
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
                .SetValue(ReturnPeriod, aReturnPeriodDbl)
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

        Public Shared Function CalculateNDayTser(ByVal aDataGroup As atcTimeseriesGroup, _
                                             ByVal aGroupArgs As atcDataAttributes, _
                                             ByVal aNDayDbl() As Double, _
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

                Dim lRankedAnnual As atcTimeseriesGroup = _
                   clsSWSTATPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup, _
                                                                 aNDay:=aNDayDbl, aHighFlag:=aHighFlag, _
                                                                 aFirstYear:=lFirstYear, aLastYear:=lastYear, _
                                                                 aBoundaryMonth:=lStartMonth, aBoundaryDay:=lStartDay, _
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
                        .Initialize(lRankedAnnual.Clone, NDayAttributes(), True, , False) 'show value, but not show form
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
        Public Shared Function SetInputsForAnalysis(ByVal aSetInputArgs As atcDataAttributes, _
                                         ByRef aCalcArgs As atcDataAttributes, _
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

        Public Shared Sub DoFrequencyGraph(ByVal aDirectory As String, _
                                           ByVal aDataGroup As atcData.atcTimeseriesGroup, _
                                           ByVal aInputArgs As atcDataAttributes, _
                                           ByVal aNDaysDbl() As Double, _
                                           ByVal aReturnPeriods() As Double, _
                                           ByVal aHighFlag As Boolean)

            Dim lHighLowText As String = "High"
            If Not aHighFlag Then lHighLowText = "Low"
            Dim lGraphFilePrefix As String = "FreqGraph_" & lHighLowText

            'Calculate("n-day " & lFlowCondition & " value", clsSWSTATPlugin.ListDefaultArray("Return Period"))
            CalculateNDayValues(aDataGroup, aInputArgs, aNDaysDbl, clsSWSTATPlugin.ListDefaultArray(InputNames.ReturnPeriod), aHighFlag)
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

        Public Shared Function DoFrequencyGrid(ByVal aDataGroup As atcTimeseriesGroup, _
                                           ByVal aGroupArgs As atcDataAttributes, _
                                           ByVal aNDayDbl() As Double, _
                                           ByVal aReturnPeriodDbl() As Double, _
                                           ByVal aHighFlag As Boolean) As String

            Try
                Dim lNDayValuesHighDone As Boolean = InputNames.CalculateNDayValues(aDataGroup, aGroupArgs, aNDayDbl, aReturnPeriodDbl, aHighFlag)
                Dim lFreqForm As New frmDisplayFrequencyGrid(aDataGroup:=aDataGroup, _
                                                 aHigh:=aHighFlag, _
                                                 aNday:=aNDayDbl, _
                                                 aReturns:=aReturnPeriodDbl, _
                                                 aShowForm:=False)
                Return lFreqForm.ToString()
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

        Public Shared Sub GetNdayReturnPeriodAttributes(ByVal aInputs As atcDataAttributes, _
                                                        ByRef aNDay() As Double, _
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

        Public Shared Function TrendAnalysis(ByVal aDataGroup As atcTimeseriesGroup, _
                                             ByVal aInputArgs As atcDataAttributes, _
                                             ByVal aNDays() As Double, _
                                             ByVal aHighFlow As Boolean) As String
            Dim lCalcArgs As New atcDataAttributes()
            'With lCalcArgs
            ' not needed as this is done inside computeRankedAnnualTimeseries
            '    .SetValue("Timeseries", aDataGroup)
            '    .SetValue(NDay, aNDays)
            'End With
            SetInputsForAnalysis(aInputArgs, lCalcArgs, aHighFlow)
            Dim lRankedAnnual As atcTimeseriesGroup = _
                   clsSWSTATPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup, _
                                                                 aNDay:=aNDays, _
                                                                 aHighFlag:=aHighFlow, _
                                                                 aFirstYear:=lCalcArgs.GetValue(InputNames.StartYear), _
                                                                 aLastYear:=lCalcArgs.GetValue(InputNames.EndYear), _
                                                                 aBoundaryMonth:=lCalcArgs.GetValue(InputNames.StartMonth), _
                                                                 aBoundaryDay:=lCalcArgs.GetValue(InputNames.StartDay), _
                                                                 aEndMonth:=lCalcArgs.GetValue(InputNames.EndMonth), _
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
                        .SetValue("From", lDateFormat.JDateToString(lTS.Dates.Value(0)))
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
    End Class
End Module
