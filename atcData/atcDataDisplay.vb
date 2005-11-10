''' <summary><para>Base class for plugins that display atcData</para></summary>
Public Class atcDataDisplay
  Inherits atcDataPlugin

  ''' <summary>Show the specified group of Data. Open empty if no group was 
  ''' specified.</summary>
  Public Overridable Function Show(ByVal aDataManager As atcData.atcDataManager, _
                   Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
  End Function

  ''' <remarks>
  ''' not ToString because the data display may have graphics that don't convert well
  ''' to a string
  ''' </remarks>
  ''' <summary>Save contents of data display to specified file</summary>
  Public Overridable Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                    ByVal aDataGroup As atcDataGroup, _
                    ByVal aFileName As String, _
                    ByVal ParamArray aOption() As String)
  End Sub
End Class
