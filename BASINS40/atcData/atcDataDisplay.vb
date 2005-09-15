Public Class atcDataDisplay
  Inherits atcDataPlugin

  'Show the specified group of Data. Open empty if no group was specified.
  Public Overridable Function Show(ByVal aDataManager As atcData.atcDataManager, _
                     Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
  End Function

  'not ToString because the display may have graphics that don't convert well to a string
  Public Overridable Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                              ByVal aDataGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)
  End Sub
End Class
