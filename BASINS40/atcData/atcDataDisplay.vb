Public Class atcDataDisplay
  Inherits atcDataPlugin

  'Show the specified group of Data. Open empty if no group was specified.
  Public Overridable Sub Show(ByVal aDataManager As atcData.atcDataManager, _
                     Optional ByVal aDataGroup As atcDataGroup = Nothing)
  End Sub

End Class
