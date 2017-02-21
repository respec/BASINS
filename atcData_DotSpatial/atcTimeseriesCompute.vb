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
  Public Overridable Function ComputeTimeseries(ByVal aOperationName As String, _
                                                ByVal args As ArrayList) As atcTimeseries
  End Function

  'Add the controls to the given collection that will be used to specify the named operation.
  'Operations that do not populate the interface cannot be used interactively.
  Public Overridable Sub PopulateInterface(ByVal aOperationName As String, _
                             ByVal aContainer As System.Windows.Forms.Control.ControlCollection, _
                             ByVal aDataManager As atcDataManager)
    aContainer.Clear()
  End Sub

  'Get the state of the controls added by PopulateInterface, return appropriate args for ComputeTimeseries
  'Operations that do not populate the interface will not have to be able to ExtractArgs either
  Public Overridable Function ExtractArgs() As ArrayList
  End Function
End Class
