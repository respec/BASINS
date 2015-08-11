Imports atcData
Public Module modUtil
    ''' <summary>
    ''' Holds a set of inputs' names for batch run
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InputNames
        Public Shared HighLow As String = "HighLow"
        Public Shared HighLowText As String = "FLOWCONDITION"
        Public Shared Logarithmic As String = "LOGARITHMIC"
        Public Shared ReturnPeriod As String = "Return Period"
        Public Shared ReturnPeriodText As String = "RECURRENCEINTERVAL"
        Public Shared NDay As String = "NDay"
        Public Shared NDays As String = "NDays"
        Public Shared ReturnPeriods As String = "Return Periods"
        Public Shared StartYear As String = "STARTYEAR"
        Public Shared EndYear As String = "ENDYEAR"
        Public Shared HighFlowSeasonStart As String = "HIGHFLOWSEASONSTART" '10/01
        Public Shared HighFlowSeasonEnd As String = "HIGHFLOWSEASONEND"     '09/30
        Public Shared LowFlowSeasonStart As String = "LOWFLOWSEASONSTART"   '04/01
        Public Shared LowFlowSeasonEnd As String = "LOWFLOWSEASONEND"       '03/31

        Public Shared MultiNDayPlot As String = "MULTI-NDAYPLOT"
        Public Shared MultiStationPlot As String = "MULTI-STATIONPLOT"

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
            If aCommonSet Is Nothing Then
                With aSpecialSet
                    If .GetValue(HighLow) Is Nothing Then .SetValue(HighLow, "Low")
                    If .GetValue(Logarithmic) Is Nothing Then .SetValue(Logarithmic, True)
                    If .GetValue(NDay) Is Nothing Then .SetValue(NDay, clsSWSTATPlugin.ListDefaultArray(NDay))
                    If .GetValue(ReturnPeriod) Is Nothing Then .SetValue(ReturnPeriod, clsSWSTATPlugin.ListDefaultArray(ReturnPeriod))
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, "")
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, "")
                    If .GetValue(MultiNDayPlot) Is Nothing Then .SetValue(MultiNDayPlot, False)
                    If .GetValue(MultiStationPlot) Is Nothing Then .SetValue(MultiStationPlot, False)
                    If .GetValue(HighFlowSeasonStart) Is Nothing Then .SetValue(HighFlowSeasonStart, "10/01")
                    If .GetValue(HighFlowSeasonEnd) Is Nothing Then .SetValue(HighFlowSeasonEnd, "09/30")
                    If .GetValue(LowFlowSeasonStart) Is Nothing Then .SetValue(LowFlowSeasonStart, "04/01")
                    If .GetValue(LowFlowSeasonEnd) Is Nothing Then .SetValue(LowFlowSeasonEnd, "03/31")
                End With
            Else
                With aSpecialSet
                    If .GetValue(HighLow) Is Nothing Then .SetValue(HighLow, aCommonSet.GetValue(HighLow, "Low"))
                    If .GetValue(Logarithmic) Is Nothing Then .SetValue(Logarithmic, aCommonSet.GetValue(Logarithmic, True))
                    If .GetValue(NDay) Is Nothing Then .SetValue(NDay, aCommonSet.GetValue(NDay, clsSWSTATPlugin.ListDefaultArray(NDay)))
                    If .GetValue(ReturnPeriod) Is Nothing Then .SetValue(ReturnPeriod, aCommonSet.GetValue(ReturnPeriod, clsSWSTATPlugin.ListDefaultArray(ReturnPeriod)))
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
                End With
            End If
        End Sub
    End Class
End Module
