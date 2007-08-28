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
        For lRow As Integer = lTableSQLite.NumRecords To 1 Step -1 'to from last to first
            Dim lStr As String = "Row:" & lRow
            lTableSQLite.CurrentRecord = lRow
            For lColumn As Integer = 1 To lTableSQLite.NumFields
                lStr &= ":" & lTableSQLite.Value(lColumn)
            Next
            Logger.Dbg(lStr)
        Next
        With lTableSQLite
            .NumFields += 1
            .FieldName(.NumFields) = "NewField"
            .NumFields += 1
            .FieldName(.NumFields) = "AnotherField"
            .FieldAdd("DetailField", "Integer", 4)
            Logger.Dbg("FieldCount " & .NumFields)
            Dim lStr As String = "FieldSummary"
            For lColumn As Integer = 1 To .NumFields
                lStr &= ":" & .FieldName(lColumn) & ":" & .FieldType(lColumn)
            Next
            Logger.Dbg(lStr)
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
            .ClearData()
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
            .Clear()
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
        End With
    End Sub
End Module
