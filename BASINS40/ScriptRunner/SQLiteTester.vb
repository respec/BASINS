Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcTableSQLite

Public Module ScriptSQLLiteTester
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'Dim lConnectionString As String = "Data Source=c:\temp\test.db3"
        Dim lDatabaseFileName As String = "c:\temp\test.db3"
        Dim lTableName As String = "keyinfotest"
        Dim lTableSQLite As New atcTableSQLite.atcTableSQLite
        Dim lResult As Boolean = lTableSQLite.OpenFile(lDatabaseFileName & vbTab & lTableName)
        Logger.Dbg("OpenResult:" & lResult)
    End Sub
End Module
