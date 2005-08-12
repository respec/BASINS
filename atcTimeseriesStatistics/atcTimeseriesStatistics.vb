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

        AddOperation("Count", "Count of non missing values", defTimeSeriesOne, defCategory)

        AddOperation("Max", "Maximum value", defTimeSeriesOne, defCategory)

        AddOperation("Min", "Minimum value", defTimeSeriesOne, defCategory)

        AddOperation("Mean", "Sum of all values divided by number of values", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Geometric Mean", "10 ^ Mean of log(each value)", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Variance", "Statistical variance", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Standard deviation", "Standard deviation", _
                     defTimeSeriesOne, defCategory)

        AddOperation("Skew", "Skewness", _
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
    Dim lLastValueIndex As Integer = aTimeseries.numValues - 1

    If lLastValueIndex >= 0 Then
      Dim lIndex As Integer
      Dim lVal As Double
      Dim lDev As Double

      Dim lMax As Double = -1.0E+30
      Dim lMin As Double = 1.0E+30

      Dim lGeometricMean As Double = 0
      Dim lStdDev As Double = 0
      Dim lCount As Double = 0
      Dim lMean As Double = 0
      Dim lSum As Double = 0
      Dim lSumDevSquares As Double = 0
      Dim lSumDevCubes As Double = 0
      Dim lVariance As Double = 0
      Dim lSkew As Double = 0

      For lIndex = 0 To lLastValueIndex
        lVal = aTimeseries.Value(lIndex)
        If Not aTimeseries.ValueMissing(lVal) Then
          lCount += 1
          If lVal > lMax Then lMax = lVal
          If lVal < lMin Then lMin = lVal
          lSum += lVal
          If lMin > 0 Then lGeometricMean += Math.Log(lVal)
        End If
      Next

      If lCount > 0 Then
        aTimeseries.Attributes.SetValue("Max", lMax)
        aTimeseries.Attributes.SetValue("Min", lMin)
        aTimeseries.Attributes.SetValue("Sum", lSum)
        aTimeseries.Attributes.SetValue("Count", lCount)
        lMean = lSum / lCount
        aTimeseries.Attributes.SetValue("Mean", lMean)
      End If

      If lMin > 0 Then
        lGeometricMean = Math.Exp(lGeometricMean / lCount)
        aTimeseries.Attributes.SetValue("Geometric Mean", lGeometricMean)
      End If

      If lCount > 1 Then
        For lIndex = 0 To lLastValueIndex
          lVal = aTimeseries.Value(lIndex)
          If Not aTimeseries.ValueMissing(lVal) Then
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
          lSkew = (lCount * lSumDevCubes) / ((lCount - 1.0) * (lCount * 2.0) * lStdDev * lStdDev * lStdDev)
          aTimeseries.Attributes.SetValue("Skew", lSkew)
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

End Class
