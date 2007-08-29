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
            lDatabaseFileName = lDatabaseFileName.Replace("test", "jack")
            .WriteFile(lDatabaseFileName & vbTab & lTableName)

            .NumFields += 1
            .FieldName(.NumFields) = "NewField"
            .NumFields += 1
            .FieldName(.NumFields) = "AnotherField"
            .FieldAdd("DetailField", "Integer", 4)
            Logger.Dbg("FieldCount " & .NumFields)
            Dim lStr As String = "FieldSummary"
            For lColumnIndex As Integer = 1 To .NumFields
                lStr &= ":" & .FieldName(lColumnIndex) & ":" & .FieldType(lColumnIndex)
            Next
            Logger.Dbg(lStr)
            .CurrentRecord = 1
            Dim lColumn As Integer = 6
            Logger.Dbg("Row:" & .CurrentRecord & ":Column:" & lColumn & ":Value:" & .Value(lColumn))
            .Value(lColumn) = 23
            Logger.Dbg("Row:" & .CurrentRecord & ":Column:" & lColumn & ":Value:" & .Value(lColumn))
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
            .ClearData()
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
            .Clear()
            Logger.Dbg("Rowcount:" & .NumRecords & ":ColumnCount:" & .NumFields)
        End With
        lTableSQLite = Nothing
        lTableSQLite = New atcTableSQLite.atcTableSQLite
        lResult = lTableSQLite.OpenFile(lDatabaseFileName & vbTab & lTableName)
        Logger.Dbg("OpenResult:" & lResult)
        Logger.Dbg("NumRows:" & lTableSQLite.NumRecords & ":NumFields:" & lTableSQLite.NumFields)
        With lTableSQLite
            For lRow As Integer = 1 To .NumRecords
                .CurrentRecord = lRow
                For lCol As Integer = 1 To .NumFields
                    Logger.Dbg("R:" & lRow & ":C:" & lCol & ":Value:" & .Value(lCol))
                Next
            Next
        End With
    End Sub
End Module
