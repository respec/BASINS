''' <summary><para>Base class for plugins that use atcData</para></summary>
Public MustInherit Class atcDataTool
    Inherits atcDataPlugin
    Public MustOverride Function Show() As Object
End Class

''' <summary><para>Base class for plugins that display atcData</para></summary>
Public Class atcDataDisplay
    Inherits atcDataTool

    ''' <summary>Show the specified data interactively.</summary>
    Public Overrides Function Show() As Object
        Return Show(Nothing)
    End Function
    Public Overridable Overloads Function Show(ByVal aDataGroup As atcDataGroup) As Object
        Return Nothing
    End Function

    ''' <summary>Save contents of data display to specified file</summary>
    ''' <remarks>
    ''' not ToString because the data display may have graphics that don't convert well
    ''' to a string
    ''' </remarks>
    Public Overridable Sub Save(ByVal aDataGroup As atcDataGroup, _
                                ByVal aFileName As String, _
                                ByVal ParamArray aOption() As String)
    End Sub
End Class

