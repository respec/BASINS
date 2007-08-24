Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcTableSQLite

Public Module ScriptSQLLiteTester
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'Dim lConnectionString As String = "Data Source=c:\temp\test.db3"
        Dim lDatabaseFileName As String = "c:\temp\test.db3"
        Dim lTableName As String = "FullText"
        Dim lTableSQLite As New atcTableSQLite.atcTableSQLite
        Dim lResult As Boolean = lTableSQLite.OpenFile(lDatabaseFileName & vbTab & lTableName)
        Logger.Dbg("OpenResult:" & lResult)
        Logger.Dbg("NumRows:" & lTableSQLite.NumRecords & ":NumFields:" & lTableSQLite.NumFields)
        For lRow As Integer = lTableSQLite.NumRecords - 1 To 0 Step -1 'to from last to first
            Dim lStr As String = "Row:" & lRow
            lTableSQLite.CurrentRecord = lRow
            For lColumn As Integer = 0 To lTableSQLite.NumFields - 1
                lStr &= ":" & lTableSQLite.Value(lColumn)
            Next
            Logger.Dbg(lStr)
        Next
    End Sub
End Module
