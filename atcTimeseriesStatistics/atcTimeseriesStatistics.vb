Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

''' <summary>Computes statistics of a timeseries</summary>
Public Class atcTimeseriesStatistics
    Inherits atcTimeseriesSource
    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private Shared pNaN As Double = GetNaN()
    Private Shared pMinValue As Double = GetMinValue()
    Private Shared pMaxValue As Double = GetMaxValue()

    ''' <summary>returns 'Timeseries::Statistics'</summary>
    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Statistics"
        End Get
    End Property

    ''' <summary>Returns 'Compute Statistics'</summary>
    Public Overrides ReadOnly Property Category() As String
        Get
            Return "Compute Statistics"
        End Get
    End Property

    ''' <summary>Returns 'Computes statistics of a timeseries and stores them as attributes'</summary>
    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Computes statistics of a timeseries and stores them as attributes"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        For Each lOperation As atcDefinedValue In AvailableOperations
            atcDataAttributes.AddDefinition(lOperation.Definition)
        Next
    End Sub

    ''' <summary>Definitions of statistics supported by this class.</summary>
    Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            If pAvailableOperations Is Nothing Then
                pAvailableOperations = New atcDataAttributes

                Dim lCategory As String = "Statistics"

                Dim defTimeSeriesOne As New atcAttributeDefinition
                With defTimeSeriesOne
                    .Name = "Timeseries"
                    .Description = "One time series"
                    .Editable = True
                    .TypeString = "atcTimeseries"
                End With

                AddOperation("Date Created", "Date Timeseries Created", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Date Modified", "Date Timeseries Last Modified", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Count", "Count of non missing values", defTimeSeriesOne, lCategory, "Integer", 0)

                AddOperation("Count Positive", "Count of values greater than zero", defTimeSeriesOne, lCategory, "Integer", 0)

                AddOperation("Count Zero", "Count of values equal to zero", defTimeSeriesOne, lCategory, "Integer", 0)

                AddOperation("Count Missing", "Count of values that are undefined", defTimeSeriesOne, lCategory, "Integer", 0)

                'AddOperation("Point", "True for sparse data", defTimeSeriesOne, lCategory, "Boolean", False)

                AddOperation("Start Date", "Starting Julian Date", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("End Date", "Ending Julian Date", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Last", "Last value", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Max", "Maximum value", defTimeSeriesOne, lCategory, "Double", pNaN)
                AddOperation("MaxDate", "Date of maximum value", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Min", "Minimum value", defTimeSeriesOne, lCategory, "Double", pNaN)
                AddOperation("MinDate", "Date of minimum value", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Sum", "Summation of all values", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("SumAnnual", "Average annual value from summation of all values", defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Mean", "Sum of all values divided by number of values", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Geometric Mean", "10 ^ Mean of log(each value)", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Variance", "Statistical variance", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Standard Deviation", "Standard deviation", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Skew", "Skewness", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Standard Error of Skew", "Standard Error of Skewness", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Serial Correlation Coefficient", "Serial Correlation Coefficient", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                AddOperation("Coefficient of Variation", "Coefficient of Variation", _
                             defTimeSeriesOne, lCategory, "Double", pNaN)

                lCategory = "Percentile"
                AddOperation("%*", "percentile value", defTimeSeriesOne, lCategory, "Double", pNaN)
                'AddOperation("%01", "1st percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%02", "2nd percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%05", "5th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%10", "10th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%20", "20th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%25", "25th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%30", "30th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%40", "40th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%50", "50th percentile value (Median)", defTimeSeriesOne, lCategory)
                'AddOperation("%60", "60th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%70", "70th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%75", "75th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%80", "80th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%90", "90th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%95", "95th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%98", "98th percentile value", defTimeSeriesOne, lCategory)
                'AddOperation("%99", "99th percentile value", defTimeSeriesOne, lCategory)

                AddOperation("%sum*", "percentile sum", defTimeSeriesOne, lCategory, "Double", pNaN)

                'Dim lBinsDefinition As atcAttributeDefinition = atcDataAttributes.AllDefinitions.ItemByKey("bins")
                'If lBinsDefinition Is Nothing Then
                '    lBinsDefinition = New atcAttributeDefinition
                '    With lBinsDefinition
                '        .Name = "Bins"
                '        .Category = lCategory
                '        .Description = "Values sorted into a collection of bins"
                '        .DefaultValue = Nothing
                '        .Editable = False
                '        .TypeString = "atcCollection"
                '        .Calculator = Me
                '    End With
                '    atcDataAttributes.AllDefinitions.Add(lBinsDefinition.Name.ToLower, lBinsDefinition)
                'End If

                AddOperation("bins", "Values sorted into a collection of bins", defTimeSeriesOne, lCategory, "atcCollection", Nothing)

            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                             ByVal aDescription As String, _
                             ByVal aArg As atcAttributeDefinition, _
                             ByVal aCategory As String, _
                     ByVal aTypeString As String, _
                     ByVal aDefaultValue As Object)
        Dim lResult As New atcAttributeDefinition
        With lResult
            .Name = aName
            .Category = aCategory
            .Description = aDescription
            .DefaultValue = aDefaultValue
            .Editable = False
            .TypeString = aTypeString
            .Calculator = Me
        End With
        Dim lArguments As atcDataAttributes = New atcDataAttributes
        lArguments.SetValue(aArg, Nothing)
        pAvailableOperations.SetValue(lResult, Nothing, lArguments)

    End Sub

    'Compute all available statistics for aTimeseries and add them as attributes
    Private Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
        Dim lLastValueIndex As Integer = aTimeseries.numValues

        If lLastValueIndex = 0 Then
            aTimeseries.Attributes.SetValue("Count", 0)
        ElseIf lLastValueIndex > 0 Then
            Dim lIndex As Integer
            Dim lVal As Double
            Dim lDev As Double

            Dim lMax As Double = pMinValue
            Dim lMaxDate As Double = pNaN
            Dim lMin As Double = pMaxValue
            Dim lMinDate As Double = pNaN

            Dim lGeoMean As Double = 0
            Dim lStdDev As Double = pNaN
            Dim lCount As Double = 0
            Dim lCountPositive As Integer = 0
            Dim lCountZero As Integer = 0
            Dim lMean As Double = pNaN
            Dim lSum As Double = 0
            Dim lSumDevSquares As Double = 0
            Dim lSumDevCubes As Double = 0
            Dim lVariance As Double = pNaN
            Dim lSkew As Double = pNaN
            Dim lStErSkew As Double = pNaN
            Dim lScc As Double = pNaN
            Dim lCvr As Double = pNaN

            For lIndex = 1 To lLastValueIndex
                Dim lIsNaN As Boolean = True
                Try 'for unexpected overflow exception when IsNaN should return True
                    lVal = aTimeseries.Value(lIndex)
                    lIsNaN = Double.IsNaN(lVal)
                    If Not lIsNaN Then
                        lCount += 1
                        If lVal > lMax Then
                            lMax = lVal
                            If aTimeseries.Dates IsNot Nothing Then lMaxDate = aTimeseries.Dates.Value(lIndex)
                        End If
                        If lVal < lMin Then
                            lMin = lVal
                            If aTimeseries.Dates IsNot Nothing Then lMinDate = aTimeseries.Dates.Value(lIndex)
                        End If
                        lSum += lVal
                        If lMin > 0 Then lGeoMean += Math.Log(lVal)

                        If Math.Abs(lVal) < 1.0E-30 Then
                            lCountZero += 1
                        ElseIf lVal > 0 Then
                            lCountPositive += 1
                        End If
                    End If
                Catch e As Exception
                    Logger.Dbg("ComputeStatistics: caught exception: " & e.Message & vbCrLf & e.StackTrace)
                End Try
                If lIsNaN AndAlso aTimeseries.ValueAttributesGetValue(lIndex, "Was Zero", False) Then
                    lCount += 1
                    lCountZero += 1
                End If
            Next

            aTimeseries.Attributes.SetValue("Count", CInt(lCount))
            aTimeseries.Attributes.SetValue("Count Positive", CInt(lCountPositive))
            aTimeseries.Attributes.SetValue("Count Missing", CInt(lLastValueIndex - lCount))
            If (lLastValueIndex - lCount > lLastValueIndex * 0.75) Then
                aTimeseries.Attributes.SetValueIfMissing("Point", True)
            End If
            aTimeseries.Attributes.SetValue("Count Zero", lCountZero)
            If lCount > 0 Then
                aTimeseries.Attributes.SetValue("Last", aTimeseries.Value(lLastValueIndex))
                If Not Double.IsNaN(lMax) Then
                    aTimeseries.Attributes.SetValue("Max", lMax)
                End If
                If Not Double.IsNaN(lMin) Then
                    aTimeseries.Attributes.SetValue("Min", lMin)
                End If
                aTimeseries.Attributes.SetValue("Sum", lSum)
                lMean = lSum / lCount
                aTimeseries.Attributes.SetValue("Mean", lMean)

                If aTimeseries.Dates IsNot Nothing Then
                    If Not Double.IsNaN(lMaxDate) Then
                        aTimeseries.Attributes.SetValue("MaxDate", lMaxDate)
                    End If
                    If Not Double.IsNaN(lMinDate) Then
                        aTimeseries.Attributes.SetValue("MinDate", lMinDate)
                    End If
                    With aTimeseries.Dates
                        Dim lSJDate As Double = .FirstNumeric
                        Dim lEJDate As Double = .Value(lLastValueIndex)
                        Dim lSDate(5), lEDate(5) As Integer

                        aTimeseries.Attributes.SetValue("SJDay", lSJDate)
                        aTimeseries.Attributes.SetValue("EJDay", lEJDate)

                        Dim lTimeInterval As Double = aTimeseries.Attributes.GetValue("interval", -1.0)
                        If lTimeInterval <= 0 Then
                            Dim lTimeUnit As atcTimeUnit = aTimeseries.Attributes.GetValue("Time Unit", atcTimeUnit.TUUnknown)
                            Select Case lTimeUnit
                                Case atcTimeUnit.TUYear
                                    lTimeInterval = JulianYear * 100
                                Case atcTimeUnit.TUCentury
                                    lTimeInterval = JulianYear
                                Case atcTimeUnit.TUMonth
                                    'TODO: We may want to weight months by length, this seems to treat each month as 30.6875 days
                                    lTimeInterval = JulianYear / 12
                                Case atcTimeUnit.TUDay, atcTimeUnit.TUHour, atcTimeUnit.TUMinute, atcTimeUnit.TUSecond
                                    Dim lTimeStep As Double = aTimeseries.Attributes.GetValue("Time Step", -1)
                                    If lTimeStep > 0 Then
                                        aTimeseries.SetInterval(lTimeUnit, lTimeStep)
                                        lTimeInterval = aTimeseries.Attributes.GetValue("interval", -1.0)
                                    End If
                                Case Else
                                    Logger.Dbg("Missing Interval, cannot compute SumAnnual")
                            End Select
                        End If
                        If lTimeInterval > 0 Then
                            Dim lSeasonYearFraction As Double = aTimeseries.Attributes.GetValue("SeasonYearFraction", 1)
                            Dim lIntervalsPerYear As Double = lSeasonYearFraction * 365.25 / lTimeInterval
                            Dim lSumAnnual As Double = lMean * lIntervalsPerYear
                            aTimeseries.Attributes.SetValue("SumAnnual", lSumAnnual)
                        End If
                    End With
                End If

                If lMin > 0 Then
                    lGeoMean = Math.Exp(lGeoMean / lCount)
                    aTimeseries.Attributes.SetValue("Geometric Mean", lGeoMean)
                End If
            End If

            If lCount > 1 Then
                For lIndex = 1 To lLastValueIndex
                    lVal = aTimeseries.Value(lIndex)
                    If Not Double.IsNaN(lVal) Then
                        lDev = lVal - lMean
                        lSumDevSquares += lDev * lDev
                        lSumDevCubes += lDev * lDev * lDev
                    End If
                Next
                lVariance = lSumDevSquares / (lCount - 1)
                aTimeseries.Attributes.SetValue("Variance", lVariance)

                If lVariance > 0 Then
                    lStdDev = Math.Sqrt(lVariance)
                    aTimeseries.Attributes.SetValue("Standard Deviation", lStdDev)
                    lSkew = (lCount * lSumDevCubes) / ((lCount - 1) * (lCount - 2) * lStdDev * lStdDev * lStdDev)
                    aTimeseries.Attributes.SetValue("Skew", lSkew)
                    lStErSkew = Math.Sqrt((6.0 * lCount * (lCount - 1)) / ((lCount - 2) * (lCount + 1) * (lCount + 3)))
                    aTimeseries.Attributes.SetValue("Standard Error of Skew", lStErSkew)
                    lCvr = lStdDev / lMean
                    aTimeseries.Attributes.SetValue("Coefficient of Variation", lCvr)

                    Dim lSum1 As Double = 0
                    Dim lSum2 As Double = 0
                    Dim lSum3 As Double = 0
                    Dim lSum4 As Double = 0
                    Dim lValPl1 As Double
                    Dim lFirst As Integer = 1
                    Dim lX(lLastValueIndex) As Double

                    While Double.IsNaN(aTimeseries.Value(lFirst))
                        lFirst += 1
                    End While
                    lVal = aTimeseries.Value(lFirst)
                    For lIndex = lFirst + 1 To lLastValueIndex
                        lValPl1 = aTimeseries.Value(lIndex)
                        If Not Double.IsNaN(lValPl1) Then
                            lX(lIndex) = lVal * lValPl1
                            lSum3 += lX(lIndex)
                            lVal = lValPl1
                        End If
                    Next
                    lScc = (lSum - aTimeseries.Value(lLastValueIndex)) * (lSum - aTimeseries.Value(1))
                    lScc = ((lCount - 1) * lSum3) - lScc
                    lSum3 = 0
                    For lIndex = 1 To lLastValueIndex
                        lVal = aTimeseries.Value(lIndex)
                        If Not Double.IsNaN(lVal) Then
                            lX(lIndex) = lVal * lVal
                            lSum3 += lX(lIndex)
                        End If
                    Next
                    lSum4 = (lSum3 - (aTimeseries.Value(1) * aTimeseries.Value(1))) * (lCount - 1)
                    lSum3 = (lSum3 - (aTimeseries.Value(lLastValueIndex) * aTimeseries.Value(lLastValueIndex))) * (lCount - 1)
                    lSum2 = lSum - aTimeseries.Value(1)
                    lSum1 = lSum - aTimeseries.Value(lLastValueIndex)
                    lSum1 = lSum1 * lSum1
                    lSum2 = lSum2 * lSum2
                    lSum3 = (lSum3 - lSum1) * (lSum4 - lSum2)
                    lSum3 = Math.Sqrt(lSum3)
                    lScc = lScc / lSum3
                    aTimeseries.Attributes.SetValue("Serial Correlation Coefficient", lScc)
                End If
            End If
        End If
    End Sub

    'The only element of aArgs is an atcDataGroup or atcTimeseries
    'The attribute(s) will be set to the result(s) of calculation(s)
    Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Dim ltsGroup As atcTimeseriesGroup
        If aArgs Is Nothing Then
            ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        Else
            ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
        End If
        If Not ltsGroup Is Nothing Then
            For Each lts As atcTimeseries In ltsGroup
                If aOperationName.ToLower.StartsWith("%sum") AndAlso IsNumeric(aOperationName.Substring(4)) Then
                    ComputePercentileSum(lts, CDbl(aOperationName.Substring(4)))
                ElseIf aOperationName.StartsWith("%") Then
                    Dim lPercentString As String = aOperationName.Substring(1)
                    If IsNumeric(lPercentString) Then
                        ComputePercentile(lts, CDbl(lPercentString))
                    End If
                ElseIf aOperationName.ToLower.Equals("bins") Then
                    lts.Attributes.SetValue("Bins", MakeBins(lts))
                Else
                    ComputeStatistics(lts)
                End If
            Next
        End If
    End Function

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        MyBase.ItemClicked(aItemName, aHandled)
    End Sub
End Class
