Public Class atcBasicStatistics
  Inherits atcDataPlugin
  Private pAvailableStatistics As Hashtable

  Public Shadows ReadOnly Property Name() As String
    Get
      Return "Basic Statistics"
    End Get
  End Property

  Public Shadows ReadOnly Property Description() As String
    Get
      Dim retval As String = Name & " ("
      For Each def As atcAttributeDefinition In pAvailableStatistics
        retval &= def.Name & ", "
      Next
      Return Left(retval, Len(retval) - 2) & ")" 'Replace last ", " with ")"
    End Get
  End Property

  'List of atcAttributeDefinition objects of statistics supported by ComputeStatistics
  Public Shadows ReadOnly Property AvailableStatistics() As Hashtable
    Get
      Return pAvailableStatistics
    End Get
  End Property

  'Compute all available statistics for aTimeseries and add them as attributes
  Public Shadows Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
    Dim iLastValue As Integer = aTimeseries.numValues - 1
    If iLastValue >= 0 Then
      Dim iValue As Integer
      Dim val As Double

      Dim lMax As Double = -1.0E+30
      Dim lMin As Double = 1.0E+30

      Dim lGeometricMean As Double = 0
      Dim lStandardDeviation As Double = 0
      Dim lMean As Double = 0
      Dim lSum As Double = 0
      Dim lSumSquares As Double = 0
      Dim lVariance As Double = 0

      For iValue = 0 To iLastValue
        val = aTimeseries.Value(iValue)
        If val > lMax Then lMax = val
        If val < lMin Then lMin = val
        lSum += val
        lSumSquares += val * val
        If lMin > 0 Then lGeometricMean += Math.Log(val)
      Next

      SetAttribute(aTimeseries, "Max", lMax)
      SetAttribute(aTimeseries, "Min", lMin)
      SetAttribute(aTimeseries, "Sum", lSum)

      iValue = aTimeseries.numValues
      lMean = lSum / iValue
      SetAttribute(aTimeseries, "Mean", lMean)

      If lMin > 0 Then
        lGeometricMean = Math.Exp(lGeometricMean / iValue)
        SetAttribute(aTimeseries, "GeometricMean", lGeometricMean)
      End If

      If iValue > 1 Then
        lVariance = (lSumSquares - (lSum * lSum) / iValue) / (iValue - 1)
        SetAttribute(aTimeseries, "Variance", lVariance)
        If lVariance > 0 Then
          lStandardDeviation = Math.Sqrt(lVariance)
          SetAttribute(aTimeseries, "StandardDeviation", lStandardDeviation)
        End If
      End If
    End If
  End Sub

  'Set the named Double attribute in aTimeseries using the definition from pAvailableStatistics
  Private Sub SetAttribute(ByVal aTimeseries As atcTimeseries, ByVal aName As String, ByVal aValue As Double)
    Dim def As atcAttributeDefinition = pAvailableStatistics.Item(aName)
    aTimeseries.Attributes.SetValue(def, aValue)
  End Sub

  Public Shadows Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    pAvailableStatistics = New Hashtable
    Dim def As atcAttributeDefinition

    def = New atcAttributeDefinition
    def.Name = "Mean"
    def.Description = "Sum of all values divided by number of values"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "Max"
    def.Description = "Maximum value"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "Min"
    def.Description = "Minimum value"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "Min"
    def.Description = "Minimum value"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "GeometricMean"
    def.Description = "10 ^ Mean of log(each value)"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "Variance"
    def.Description = "Variance"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "StandardDeviation"
    def.Description = "Standard deviation"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

  End Sub
End Class
