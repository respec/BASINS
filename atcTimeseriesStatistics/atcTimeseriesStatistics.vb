Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesStatistics
  Inherits atcDataSource
  Private pAvailableOperations As atcDataGroup
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
  Public Overrides ReadOnly Property AvailableOperations() As atcDataGroup
    Get
      If pAvailableOperations Is Nothing Then
        pAvailableOperations = New atcDataGroup

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

        AddOperation("VarianceA193", "Statistical variance calc from A193", _
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
    Dim lData As New atcDataSet
    lData.Attributes.SetValue(lResult, Nothing, lArguments)
    'lData.Attributes.SetValue(aCategory, Category)
    pAvailableOperations.Add(lResult.Name.ToLower, lData)

  End Sub

  'Compute all available statistics for aTimeseries and add them as attributes
  Private Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
    Dim lLastValueIndex As Integer = aTimeseries.numValues - 1

    If lLastValueIndex >= 0 Then
      Dim lIndex As Integer
      Dim lVal As Double

      Dim lMax As Double = -1.0E+30
      Dim lMin As Double = 1.0E+30

      Dim lGeometricMean As Double = 0
      Dim lStdDev As Double = 0
      Dim lCount As Double = 0
      Dim lMean As Double = 0
      Dim lSum As Double = 0
      Dim lSumSquares As Double = 0
      Dim lSumCubes As Double = 0
      Dim lVariance As Double = 0
      Dim lVarianceA193 As Double = 0
      Dim lSkew As Double = 0

      For lIndex = 0 To lLastValueIndex
        lVal = aTimeseries.Value(lIndex)
        If Not aTimeseries.ValueMissing(lVal) Then
          lCount += 1
          If lVal > lMax Then lMax = lVal
          If lVal < lMin Then lMin = lVal
          lSum += lVal
          lSumSquares += lVal * lVal
          lSumCubes += lVal * lVal * lVal
          If lMin > 0 Then lGeometricMean += Math.Log(lVal)
        End If
      Next

      SetAttribute(aTimeseries, "Max", lMax)
      SetAttribute(aTimeseries, "Min", lMin)
      SetAttribute(aTimeseries, "Sum", lSum)
      SetAttribute(aTimeseries, "Count", lCount)

      lMean = lSum / lCount
      SetAttribute(aTimeseries, "Mean", lMean)

      If lMin > 0 Then
        lGeometricMean = Math.Exp(lGeometricMean / lCount)
        SetAttribute(aTimeseries, "Geometric Mean", lGeometricMean)
      End If

      If lCount > 1 Then
        lVarianceA193 = lSumSquares / (lCount - 1)
        SetAttribute(aTimeseries, "VarianceA193", lVarianceA193)
        lVariance = (lSumSquares - (lSum * lSum) / lCount) / (lCount - 1)
        SetAttribute(aTimeseries, "Variance", lVariance)
        If lVariance > 0 Then
          lStdDev = Math.Sqrt(lVariance)
          SetAttribute(aTimeseries, "Standard Deviation", lStdDev)
          lSkew = (lCount * lSumCubes) / ((lCount - 1.0) * (lCount * 2.0) * lStdDev * lStdDev * lStdDev)
          SetAttribute(aTimeseries, "Skew", lSkew)
        End If
      End If
    End If
  End Sub

  'Set the named attribute in aTimeseries using the definition from pAvailableStatistics
  Private Sub SetAttribute(ByVal aTimeseries As atcTimeseries, ByVal aName As String, ByVal aValue As Double)
    Dim ds As atcDataSet = pAvailableOperations.ItemByKey(aName.ToLower)
    If Not ds Is Nothing Then
      Dim def As atcAttributeDefinition = ds.Attributes.GetDefinition(aName)
      If Not def Is Nothing Then
        aTimeseries.Attributes.SetValue(def, aValue)
      End If
    End If
  End Sub

  'Opening computes statistics rather than opening a file
  'Public Overrides ReadOnly Property CanOpen() As Boolean
  '  Get
  '    Return True
  '  End Get
  'End Property

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
    For Each operation As atcDataSet In AvailableOperations
      atcDataAttributes.AddDefinition(operation.Attributes(0).Definition)
    Next
  End Sub

End Class
