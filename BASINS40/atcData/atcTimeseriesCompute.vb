Public Class atcTimeseriesCompute
  Inherits atcDataPlugin

  'atcAttributeDefinitions of statistics supported by ComputeStatistics
  Public Overridable ReadOnly Property AvailableStatistics() As Hashtable
    Get
      Return New Hashtable 'default to an empty list with nothing available
    End Get
  End Property

  'atcAttributeDefinitions representing operations supported by ComputeTimeseries
  Public Overridable ReadOnly Property AvailableOperations() As Hashtable
    Get
      Return New Hashtable 'default to an empty list with nothing available
    End Get
  End Property

  'Compute all available statistics for aTimeseries and add them as attributes
  Public Overridable Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
  End Sub

  'Compute a new atcTimeseries
  'Args are each usually either Double or atcTimeseries
  Public Overridable Function ComputeTimeseries(ByVal aOperationName As String, ByVal args As ArrayList) As atcTimeseries
  End Function

End Class
