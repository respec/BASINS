Imports atcData

Public Class atcHydroligicIndices
  Inherits atcDataPlugin
  Private pAvailableStatistics As Hashtable
  Private pAvailableTimeseriesOperations As Hashtable

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Hydrologic Indices"
    End Get
  End Property

  Public Shadows ReadOnly Property Description() As String
    Get
      Dim retval As String = Name & " ("

      For Each def As atcAttributeDefinition In pAvailableStatistics
        retval &= def.Name & ", "
      Next

      For Each def As atcAttributeDefinition In pAvailableTimeseriesOperations
        retval &= def.Name & ", "
      Next

      Return Left(retval, Len(retval) - 2) & ")" 'Replace last ", " with ")"
    End Get
  End Property

  'Definitions of statistics supported by ComputeStatistics
  Public Shadows ReadOnly Property AvailableStatistics() As Hashtable
    Get
      Return pAvailableStatistics
    End Get
  End Property

  'Compute all available statistics for aTimeseries and add them as attributes
  Public Shadows Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
    Dim iLastValue As Integer = aTimeseries.numValues - 1
    If iLastValue >= 0 Then
      SetAttribute(aTimeseries, "7Q10", "7Q10 can not yet be calculated")
    End If
  End Sub

  'Set the named Double attribute in aTimeseries using the definition from pAvailableStatistics
  Private Sub SetAttribute(ByVal aTimeseries As atcTimeseries, ByVal aName As String, ByVal aValue As Double)
    Dim def As atcAttributeDefinition = pAvailableStatistics.Item(aName)
    aTimeseries.Attributes.SetValue(def, aValue)
  End Sub

  'List of atcAttributeDefinition objects representing operations supported by ComputeTimeseries
  Public Shadows ReadOnly Property AvailableTimeseriesOperations() As Hashtable
    Get
      Return pAvailableTimeseriesOperations
    End Get
  End Property

  'Compute a new atcTimeseries
  'Args are each usually either Double or atcTimeseries
  Public Shadows Function ComputeTimeseries(ByVal aOperationName As String, ByVal args As ArrayList) As atcTimeseries
    'SetAttribute(aTimeseries, "N-day Mean", "N-day Mean can not yet be calculated")
  End Function

  Public Shadows Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    Dim def As atcAttributeDefinition

    pAvailableStatistics = New Hashtable
    pAvailableTimeseriesOperations = New Hashtable

    def = New atcAttributeDefinition
    def.Name = "7Q10"
    def.Description = "Seven year 10-day low flow"
    def.Editable = False
    pAvailableStatistics.Add(def.Name, def)

    def = New atcAttributeDefinition
    def.Name = "N-day Mean"
    def.Description = "Mean of values within N-day range"
    def.Editable = False
    pAvailableTimeseriesOperations.Add(def.Name, def)

  End Sub

End Class
