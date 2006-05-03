Imports atcData
Imports atcUtility

Imports System.Windows.Forms

''' <summary>Computes statistics of a timeseries</summary>
Public Class atcTimeseriesStatistics
    Inherits atcDataSource
    Private pAvailableOperations As atcDataAttributes ' atcDataGroup

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

                AddOperation("Date Created", "Date Timeseries Created", defTimeSeriesOne, lCategory)

                AddOperation("Date Modified", "Date Timeseries Last Modified", defTimeSeriesOne, lCategory)

                AddOperation("Count", "Count of non missing values", defTimeSeriesOne, lCategory, "Integer", 0)

                AddOperation("SJDay", "Starting Julian Date", defTimeSeriesOne, lCategory)

                AddOperation("EJDay", "Ending Julian Date", defTimeSeriesOne, lCategory)

                AddOperation("Max", "Maximum value", defTimeSeriesOne, lCategory)

                AddOperation("Min", "Minimum value", defTimeSeriesOne, lCategory)

                AddOperation("Sum", "Summation of all values", defTimeSeriesOne, lCategory)

                AddOperation("SumAnnual", "Average annual value from summation of all values", defTimeSeriesOne, lCategory)

                AddOperation("Mean", "Sum of all values divided by number of values", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Geometric Mean", "10 ^ Mean of log(each value)", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Variance", "Statistical variance", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Standard Deviation", "Standard deviation", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Skew", "Skewness", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Standard Error of Skew", "Standard Error of Skewness", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Serial Correlation Coefficient", "Serial Correlation Coefficient", _
                             defTimeSeriesOne, lCategory)

                AddOperation("Coefficient of Variation", "Coefficient of Variation", _
                             defTimeSeriesOne, lCategory)

                lCategory = "Percentile"
                AddOperation("%01", "1st percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%02", "2nd percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%05", "5th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%10", "10th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%20", "20th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%25", "25th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%30", "30th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%40", "40th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%50", "50th percentile value (Median)", defTimeSeriesOne, lCategory)
                AddOperation("%60", "60th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%70", "70th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%75", "75th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%80", "80th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%90", "90th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%95", "95th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%98", "98th percentile value", defTimeSeriesOne, lCategory)
                AddOperation("%99", "99th percentile value", defTimeSeriesOne, lCategory)
            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                             ByVal aDescription As String, _
                             ByVal aArg As atcAttributeDefinition, _
                             ByVal aCategory As String, _
                    Optional ByVal aTypeString As String = "Double", _
                    Optional ByVal aDefaultValue As Object = Double.NaN)
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

            Dim lMax As Double = Double.MinValue
            Dim lMin As Double = Double.MaxValue

            Dim lGeoMean As Double = 0
            Dim lStdDev As Double = Double.NaN
            Dim lCount As Double = 0
            Dim lMean As Double = Double.NaN
            Dim lSum As Double = 0
            Dim lSumDevSquares As Double = 0
            Dim lSumDevCubes As Double = 0
            Dim lVariance As Double = Double.NaN
            Dim lSkew As Double = Double.NaN
            Dim lStErSkew As Double = Double.NaN
            Dim lScc As Double = Double.NaN
            Dim lCvr As Double = Double.NaN

            For lIndex = 1 To lLastValueIndex
                lVal = aTimeseries.Value(lIndex)
                If Not Double.IsNaN(lVal) Then
                    lCount += 1
                    If lVal > lMax Then lMax = lVal
                    If lVal < lMin Then lMin = lVal
                    lSum += lVal
                    If lMin > 0 Then lGeoMean += Math.Log(lVal)
                End If
            Next

            aTimeseries.Attributes.SetValue("Count", CInt(lCount))
            If lCount > 0 Then
                aTimeseries.Attributes.SetValue("SJDay", aTimeseries.Dates.Value(0))
                aTimeseries.Attributes.SetValue("EJDay", aTimeseries.Dates.Value(lLastValueIndex))
                aTimeseries.Attributes.SetValue("Max", lMax)
                aTimeseries.Attributes.SetValue("Min", lMin)
                aTimeseries.Attributes.SetValue("Sum", lSum)
                Dim lYearSpan As Double = (aTimeseries.Dates.Value(lLastValueIndex) - aTimeseries.Dates.Value(0)) / 365.25
                aTimeseries.Attributes.SetValue("SumAnnual", lSum / lYearSpan)
                lMean = lSum / lCount
                aTimeseries.Attributes.SetValue("Mean", lMean)
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
        Dim ltsGroup As atcDataGroup
        If aArgs Is Nothing Then
            ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
        Else
            ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
        End If
        If Not ltsGroup Is Nothing Then
            For Each lts As atcTimeseries In ltsGroup
                If aOperationName.StartsWith("%") Then
                    Dim lBinsDefinition As atcAttributeDefinition = atcDataAttributes.AllDefinitions.ItemByKey("bins")
                    If lBinsDefinition Is Nothing Then
                        lBinsDefinition = New atcAttributeDefinition
                        With lBinsDefinition
                            .Name = "Bins"
                            .Category = "Statistics"
                            .Description = "Values sorted into a collection of bins"
                            .DefaultValue = Nothing
                            .Editable = False
                            .TypeString = "atcCollection"
                            .Calculator = Me
                        End With
                        atcDataAttributes.AllDefinitions.Add(lBinsDefinition.Name.ToLower, lBinsDefinition)
                    End If
                    ComputePercentile(lts, CDbl(aOperationName.Substring(1, 2)))
                Else
                    ComputeStatistics(lts)
                End If
            Next
        End If
    End Function

    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        For Each lOperation As atcDefinedValue In AvailableOperations
            atcDataAttributes.AddDefinition(lOperation.Definition)
        Next
    End Sub

End Class
