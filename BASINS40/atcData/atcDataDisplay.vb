''' <summary><para>Base class for plugins that display atcData</para></summary>
Public Class atcDataDisplay
  Inherits atcDataPlugin

  ''' <summary>Show the specified data interactively.</summary>
  Public Overridable Function Show(ByVal aDataManager As atcData.atcDataManager, _
                   Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
  End Function

  ''' <summary>Save contents of data display to specified file</summary>
  ''' <remarks>
  ''' not ToString because the data display may have graphics that don't convert well
  ''' to a string
  ''' </remarks>
  Public Overridable Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                    ByVal aDataGroup As atcDataGroup, _
                    ByVal aFileName As String, _
                    ByVal ParamArray aOption() As String)
  End Sub
End Class
