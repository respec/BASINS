Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Module modCAT
    <CLSCompliant(False)> _
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
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
