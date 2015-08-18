Imports atcData
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
        Public Shared StartYear As String = "STARTYEAR"
        Public Shared EndYear As String = "ENDYEAR"
        Public Shared StartMonth As String = "StartMonth"
        Public Shared StartDay As String = "StartDay"
        Public Shared EndMonth As String = "EndMonth"
        Public Shared EndDay As String = "EndDay"
        Public Shared HighFlowSeasonStart As String = "HIGHFLOWSEASONSTART" '10/01
        Public Shared HighFlowSeasonEnd As String = "HIGHFLOWSEASONEND"     '09/30
        Public Shared LowFlowSeasonStart As String = "LOWFLOWSEASONSTART"   '04/01
        Public Shared LowFlowSeasonEnd As String = "LOWFLOWSEASONEND"       '03/31

        Public Shared MultiNDayPlot As String = "MultipleNDayPlots"
        Public Shared MultiStationPlot As String = "MultipleStationPlots"
        Public Shared StationsInfo As String = "StationsInfo"

        Public Shared Method As String = "METHOD"
        Public Enum ITAMethod
            NDAYTIMESERIES
            FREQUECYGRID
            FREQUENCYGRAPH
            TRENDLIST
        End Enum
        Public Shared MethodNdayTser As String = "NDAYTIMESERIES"
        Public Shared MethodFreqGrid As String = "FREQUECYGRID"
        Public Shared MethodFreqPlot As String = "FREQUENCYGRAPH"
        Public Shared MethodTrendLst As String = "TRENDLIST"

        Public Shared OutputDir As String = "OUTPUTDIR"
        Public Shared OutputPrefix As String = "OutputPrefix"
        Public Shared DataDir As String = "DataDir"

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
                    If .GetValue(OutputDir) Is Nothing Then .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
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

                    lStationInfo = "Station " & loc & "," & lDA & "," & lFrom
                    If Not lStationsInfo.Keys.Contains(loc) Then
                        lStationsInfo.Add(loc, lStationInfo)
                    End If
                End With
            Next
            Return lStationsInfo
        End Function
    End Class
End Module
