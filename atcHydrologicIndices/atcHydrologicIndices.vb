Imports atcData

Public Class atcHydroligicIndices
  Inherits atcTimeseriesCompute
  Private pAvailableStatistics As Hashtable
  Private pAvailableTimeseriesOperations As Hashtable

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Hydrologic Indices"
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get
      Dim retval As String = Name & " ("

      For Each def As DictionaryEntry In AvailableStatistics
        retval &= def.Value.Name & ", "
      Next

      For Each def As DictionaryEntry In AvailableOperations
        retval &= def.Value.Name & ", "
      Next

      Return Left(retval, Len(retval) - 2) & ")" 'Replace last ", " with ")"
    End Get
  End Property

  'Definitions of statistics supported by ComputeStatistics
  Public Overrides ReadOnly Property AvailableStatistics() As Hashtable
    Get
      If pAvailableStatistics Is Nothing Then
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

        def = New atcAttributeDefinition
        def.Name = "7Q10"
        def.Description = "Seven year 10-day low flow"
        def.Editable = False
        pAvailableStatistics.Add(def.Name, def)
      End If
      Return pAvailableStatistics
    End Get
  End Property

  'Compute all available statistics for aTimeseries and add them as attributes
  Public Overrides Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
    'TODO: SetAttribute(aTimeseries, "7Q10", "7Q10 can not yet be calculated")
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

  'List of atcAttributeDefinition objects representing operations supported by ComputeTimeseries
  Public Overrides ReadOnly Property AvailableOperations() As Hashtable
    Get
      If pAvailableTimeseriesOperations Is Nothing Then
        pAvailableTimeseriesOperations = New Hashtable
        Dim def As atcAttributeDefinition

        def = New atcAttributeDefinition
        def.Name = "Add"
        def.Description = "Add to each value of a timeseries"
        def.Editable = False
        pAvailableTimeseriesOperations.Add(def.Name, def)

        def = New atcAttributeDefinition
        def.Name = "N-day Mean"
        def.Description = "Mean of values within N-day range"
        def.Editable = False
        pAvailableTimeseriesOperations.Add(def.Name, def)
      End If
      Return pAvailableTimeseriesOperations
    End Get
  End Property

  'Compute a new atcTimeseries
  'Args are each usually either Double or atcTimeseries
  Public Overrides Function ComputeTimeseries(ByVal aOperationName As String, ByVal args As ArrayList) As atcTimeseries
    Dim newVals() As Double
    Dim numberArg As Double = 0
    Dim retval As New atcTimeseries(Nothing)
    Dim iTS As Integer
    Dim curTS As atcTimeseries
    Dim tsArgs As New ArrayList
    Dim firstTS As atcTimeseries

    For Each arg As Object In args
      If retval.GetType.IsAssignableFrom(arg.GetType) Then 'atcTimeseries
        curTS = arg
        If firstTS Is Nothing Then
          firstTS = curTS
          ReDim newVals(firstTS.numValues)
        End If
        If curTS.numValues <> firstTS.numValues Then
          Err.Raise("Different lengths of atcTimeseries passed to ComputeTimeseries")
        Else
          tsArgs.Add(curTS)
        End If
      Else 'not atcTimeseries, probably a constant
        If numberArg <> 0 Then
          Err.Raise("Passing more than one constant passed to ComputeTimeseries not yet supported")
        End If
        numberArg = CDbl(arg)
      End If
    Next
    If firstTS Is Nothing Then
      Err.Raise("ComputeTimeseries requires at least one Timeseries as an argument")
    Else
      Select Case aOperationName.ToLower
        Case "add", "+"
          Array.Copy(firstTS.Values, newVals, firstTS.numValues) 'copy values from firstTS
          For iValue As Integer = 0 To firstTS.numValues - 1
            newVals(iValue) += numberArg
            For iTS = 1 To tsArgs.Count - 1 'don't need to add firstTS, starting at 1
              newVals(iValue) += tsArgs(iTS).Value(iValue)
            Next
          Next
        Case Else
          Err.Raise(aOperationName & " not yet implemented")
      End Select
    End If
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
  End Sub

End Class
