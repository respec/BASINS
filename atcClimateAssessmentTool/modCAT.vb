Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modCAT

#If GISProvider = "DotSpatial" Then
    Friend Function ScriptFolder() As String
        Return IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    End Function
#Else
    <CLSCompliant(False)> _
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend Function ScriptFolder() As String
        Return IO.Path.Combine(g_MapWin.Plugins.PluginFolder, "BASINS")
    End Function
#End If
    Friend g_Running As Boolean = False

    Friend Function ToXML(ByVal aString As String) As String
        'Have to replace ampersand first
        Return aString.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("""", "&quot;").Replace("'", "&apos;")
    End Function

    'Don't need this because parsing XML with System.XML and using InnerText takes care of it
    'Friend Function FromXML(ByVal aString As String) As String
    '    'Have to replace ampersand last
    '    Return aString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", """").Replace("&apos;", "'").Replace("&amp;", "&")
    'End Function
End Module
