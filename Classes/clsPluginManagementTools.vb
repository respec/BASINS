Public Class PluginManagementTools
    Public Shared Function FindPluginDLLs(ByVal PluginFolder As String) As ArrayList
        Dim finallist As New ArrayList

        Try
            If System.IO.Directory.Exists(PluginFolder) Then
                ' Plugin folder exists
                For Each filename As String In IO.Directory.GetFiles(PluginFolder, "*.dll", IO.SearchOption.AllDirectories)
                    If Not filename.Contains("Interop") Then
                        finallist.Add(filename)
                    End If
                Next
            End If
            Return finallist
        Catch ex As Exception
            Logger.Dbg(ex.ToString())
            Return finallist
        End Try
    End Function

    Public Shared Function CreatePluginObject(ByVal Filename As String, ByVal CreateString As String) As Object
        Try
            Return System.Reflection.Assembly.LoadFrom(Filename).CreateInstance(CreateString)
        Catch ex As System.Exception
            Logger.Dbg(ex.ToString())
            Return Nothing
        End Try
    End Function

    Public Shared Function GetCreateString(ByVal Assem As System.Type) As String
        Return Assem.FullName
    End Function

    Public Shared Function GenerateKey(ByVal Assem As System.Type) As String
        Return GetCreateString(assem).Replace(".", "_")
    End Function
End Class
