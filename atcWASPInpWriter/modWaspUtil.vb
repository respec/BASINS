Module modWaspUtil

    ''' <summary>
    ''' Write line to file, substituting TAB character for \t in format string
    ''' </summary>
    Friend Sub WriteLine(ByRef sw As IO.StreamWriter, ByVal FormatString As String, ByVal ParamArray Args() As Object)
        If String.IsNullOrEmpty(FormatString) Then FormatString = ""
        sw.WriteLine(String.Format(FormatString, Args).Replace("\t", vbTab))
    End Sub

End Module
