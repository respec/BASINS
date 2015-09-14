''' <summary>
'''     Interface for pluggable extensions that operate on data
''' </summary>
Public Interface IDataExtension

    ''' <summary>Unique name that users will see that identifies this extension.</summary>
    ReadOnly Property Name() As String

    ''' <summary>Longer version of <see cref="Name">Name</see> with room to expand acronyms</summary>
    ReadOnly Property Description() As String

    ''' <summary>A company name, individual, or organization name.</summary>
    ReadOnly Property Author() As String

    ''' <summary>Version number of the extension.</summary>
    ''' <remarks>
    ''' Can either return a hard-coded string such as "1.0.0.1" or use
    ''' GetVersionInfo to dynamically return the version number from the assembly itself.
    ''' </remarks>
    ReadOnly Property Version() As String

    ''' <summary>
    ''' XML specification of what function(s) this extension can perform
    ''' and what can or must be specified as parameters for each function.
    ''' </summary>
    ''' <remarks>
    ''' XML string may be replaced by a custom-designed class
    ''' </remarks>
    ReadOnly Property QuerySchema() As String

    ''' <summary>Perform a function that was described in QuerySchema.</summary>
    ''' <remarks>
    ''' Values must be included in the query for all required parameters.
    ''' Return type is to be determined.
    ''' </remarks>
    Function Execute(ByVal aQuery As String) As String

End Interface
