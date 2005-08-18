Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesStatistics
  Inherits atcDataSource
  Private pAvailableOperations As atcDataAttributes ' atcDataGroup
  Private Const pName As String = "Timeseries::Statistics"

  Public Overrides ReadOnly Property Name() As String
    Get
      Return pName
    End Get
  End Property

  Public Overrides ReadOnly Property Category() As String
    Get
      Return "Compute Statistics"
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get
      Return Name
    End Get
  End Property

  'Definitions of statistics supported by ComputeStatistics
  Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
    Get
      If pAvailableOperations Is Nothing Then
        pAvailableOperations = New atcDataAttributes

        Dim defCategory As New atcAttributeDefinition
        With defCategory
          .Name = "Category"
          .Description = ""
          .Editable = False
          .TypeString = "String"
        End With

        Dim defTimeSeriesOne As New atcAttributeDefinition
        With defTimeSeriesOne
          .Name = "Timeseries"
          .Description = "One time series"
          .Editable = True
          .TypeString = "atcTimeseries"
        End With

        AddOperation("Date Created", "Date Timeseries Created", defTimeSeriesOne, defCategory)

        AddOperation("Date Modified", "Date Timeseries Last Modified", defTimeSeriesOne, defCategory)

        AddOperation("Count", "Count of non missing values", defTimeSeriesOne, defCategory)

        AddOperation("SJDay", "Starting Julian Date", defTimeSeriesOne, defCategory)

        AddOperation("EJDay", "Ending Julian Date", defTimeSeriesOne, defCategory)

        AddOperation("Max", "Maximum value", defTimeSeriesOne, defCategory)

        AddOperation("Min", "Minimum value", defTimeSeriesOne, defCategory)

        AddOperation("Sum", "Summation of all values", defTimeSeriesOne, defCategory)

        AddOperation("Mean", "Sum of all values divided by number of values", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Geometric Mean", "10 ^ Mean of log(each value)", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Variance", "Statistical variance", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Standard Deviation", "Standard deviation", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Skew", "Skewness", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Standard Error of Skew", "Standard Error of Skewness", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Serial Correlation Coefficient", "Serial Correlation Coefficient", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Coefficient of Variation", "Coefficient of Variation", _
                     defTimeSeriesOne, defCategory)
      End If
      Return pAvailableOperations
    End Get
  End Property

  Private Sub AddOperation(ByVal aName As String, _
                           ByVal aDescription As String, _
                           ByVal aArg As atcAttributeDefinition, _
                           ByVal aCategory As atcAttributeDefinition)
    Dim lResult As New atcAttributeDefinition
    With lResult
      .Name = aName
      .Description = aDescription
      .DefaultValue = ""
      .Editable = False
      .TypeString = "Double"
      .Calculator = Me
    End With
    Dim lArguments As atcDataAttributes = New atcDataAttributes
    lArguments.SetValue(aArg, Nothing)
    pAvailableOperations.SetValue(lResult, Nothing, lArguments)

  End Sub

  'Compute all available statistics for aTimeseries and add them as attributes
  Private Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
    Dim lLastValueIndex As Integer = aTimeseries.numValues

    If lLastValueIndex > 0 Then
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

      If lCount > 0 Then
        aTimeseries.Attributes.SetValue("SJDay", aTimeseries.Dates.Value(0))
        aTimeseries.Attributes.SetValue("EJDay", aTimeseries.Dates.Value(lLastValueIndex))
        aTimeseries.Attributes.SetValue("Max", lMax)
        aTimeseries.Attributes.SetValue("Min", lMin)
        aTimeseries.Attributes.SetValue("Sum", lSum)
        aTimeseries.Attributes.SetValue("Count", lCount)
        lMean = lSum / lCount
        aTimeseries.Attributes.SetValue("Mean", lMean)
      End If

      If lMin > 0 Then
        lGeoMean = Math.Exp(lGeoMean / lCount)
        aTimeseries.Attributes.SetValue("Geometric Mean", lGeoMean)
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
          lSkew = (lCount * lSumDevCubes) / ((lCount - 1.0) * (lCount - 2.0) * lStdDev * lStdDev * lStdDev)
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
            lFirst = +1
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

  'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
  'remaining aArgs are expected to follow the args required for the specified operation
  Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    If aArgs Is Nothing Then
      Dim ltsGroup As atcDataGroup = DataManager.UserSelectData("Select data to compute statistics for")
      For Each lts As atcTimeseries In ltsGroup
        ComputeStatistics(lts)
      Next
    Else
      ComputeStatistics(aArgs.GetValue("Timeseries"))
    End If
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    For Each lOperation As atcDefinedValue In AvailableOperations
      atcDataAttributes.AddDefinition(lOperation.Definition)
    Next
  End Sub

  Public Overrides Function NewOne() As atcDataPlugin
    Return New atcTimeseriesStatistics
  End Function
End Class
