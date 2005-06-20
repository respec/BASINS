Public Class atcTimeseriesDisplay
  Inherits atcDataPlugin

  'Show the specified group of Timeseries. Open empty if no group was specified.
  Public Overridable Sub Show(ByVal aTimeseriesManager As atcData.atcDataManager, _
                     Optional ByVal aTimeseriesGroup As atcTimeseriesGroup = Nothing)
  End Sub

End Class
