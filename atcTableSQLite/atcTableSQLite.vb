Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility

Public Class atcTableSQLite
    Inherits atcTable

    Private pDataTable As DataTable 'rows and columns are zero based collections
    Private pRow As Integer = 0
    Private pModified As Boolean = False 'TODO: where/when do we check this?
    Private pTableNames As New atcCollection

    Public Overrides Function Cousin() As IatcTable
        Return Nothing
    End Function

    Public Overrides Function CreationCode() As String
        Return ""
    End Function

    Public Overrides Function OpenFile(ByVal aFileName As String) As Boolean
        Dim lReturn As Boolean = True
        Dim lDB As System.Data.Common.DbConnection = Nothing
        Try
            Dim lFileAndTableName As String = aFileName
            Dim lDatabaseName As String = MapWinUtility.Strings.StrSplit(lFileAndTableName, vbTab, "'")

            Dim lTableName As String = lFileAndTableName
            ldb = OpenDatabase(lDatabaseName)
            If lTableName.Length = 0 AndAlso pTableNames.Count > 0 Then 'default to first available
                lTableName = pTableNames(0)
            End If

            If lTableName.Length > 0 Then
                pDataTable = New DataTable

                Dim lCmd As System.Data.Common.DbCommand
                Dim lAdapter As System.Data.Common.DbDataAdapter
                If lDB.GetType.Name = "SQLiteConnection" Then
                    lCmd = New System.Data.SQLite.SQLiteCommand(lDB)
                    lAdapter = New System.Data.SQLite.SQLiteDataAdapter
                Else
                    lCmd = New System.Data.OleDb.OleDbCommand
                    lCmd.Connection = lDB
                    lAdapter = New System.Data.OleDb.OleDbDataAdapter
                End If
                Dim lCmdText As String = "Select * from " & lTableName
                lCmd.CommandText = lCmdText
                lAdapter.SelectCommand = lCmd
                Dim iResult As Integer = lAdapter.Fill(pDataTable)
                pDataTable.TableName = lTableName

                Logger.Dbg("---- TableName " & lTableName)
                Logger.Dbg("FieldCount:" & pDataTable.Columns.Count & ":Rows:" & pDataTable.Rows.Count)
                Logger.Dbg("FieldDetails")
                Dim lStr As String = "Name"
                For lField As Integer = 0 To pDataTable.Columns.Count - 1
                    lStr &= ":" & pDataTable.Columns(lField).ColumnName
                Next
                Logger.Dbg(lStr)
                lStr = "DataType"
                For lField As Integer = 0 To pDataTable.Columns.Count - 1
                    lStr &= ":" & pDataTable.Columns(lField).DataType.ToString
                Next
                Logger.Dbg(lStr)
                lStr = "Length"
                For lField As Integer = 0 To pDataTable.Columns.Count - 1
                    lStr &= ":" & pDataTable.Columns(lField).MaxLength
                Next
                Logger.Dbg(lStr)
            Else
                pDataTable = Nothing
                Throw New ApplicationException("No Tables Available in " & lDatabaseName)
            End If
            pModified = False
        Catch ex As Exception
            Logger.Msg(ex.Message, "TableOpenProblem")
            lReturn = False
        End Try
        If Not lDB Is Nothing Then lDB.Close()
        Return lReturn
    End Function

    Public Overrides Function WriteFile(ByVal aFileName As String) As Boolean
        Dim lReturn As Boolean = True
        If aFileName.Length > 0 Then
            Dim lFileAndTableName As String = aFileName
            Dim lStr As String
            Dim lCmd As System.Data.Common.DbCommand

            Dim lDatabaseName As String = MapWinUtility.Strings.StrSplit(lFileAndTableName, vbTab, "'")
            Dim lTableName As String = lFileAndTableName

            Dim lDB As System.Data.Common.DbConnection = OpenDatabase(lDatabaseName)

            If TableNames.IndexFromKey(lTableName) >= 0 Then
                lStr = "DROP TABLE " & lTableName
                lCmd = lDB.CreateCommand
                lCmd.CommandText = lStr
                lCmd.ExecuteNonQuery()
            End If

            lStr = "CREATE TABLE " & lTableName & "("
            For Each lColumn As System.Data.DataColumn In pDataTable.Columns
                If InStr(lColumn.DataType.ToString.ToLower, "byte") > 0 Then
                    Logger.Dbg("DontKnowHowToProcess:Byte")
                Else
                    lStr &= lColumn.ColumnName & " " & lColumn.DataType.ToString & ", "
                End If
            Next
            lStr &= ")"
            lStr = lStr.Replace(", )", ")")
            lCmd = Nothing
            lCmd = lDB.CreateCommand
            lStr = lStr.Replace("System.", "")
            lCmd.CommandText = lStr.Replace(" Default ", " 'Default' ") 'reserved word!

            Dim lResult As Integer
            Try
                Logger.Dbg("ExecuteCommand:" & lCmd.CommandText)
                lResult = lCmd.ExecuteNonQuery()

                Dim lTransaction As System.Data.Common.DbTransaction = lDB.BeginTransaction
                For lRecordNumber As Integer = 1 To Me.NumRecords
                    Me.CurrentRecord = lRecordNumber
                    lCmd = Nothing
                    lCmd = lDB.CreateCommand
                    lCmd.Transaction = lTransaction
                    lStr = "INSERT INTO " & lFileAndTableName & "("
                    Dim lColumnNumber As Integer = 0
                    For Each lColumn As System.Data.DataColumn In pDataTable.Columns
                        lColumnNumber += 1
                        If InStr(lColumn.DataType.ToString.ToLower, "byte") = 0 Then
                            lStr &= lColumn.ColumnName & ", "
                            Dim lParameter As System.Data.Common.DbParameter = lCmd.CreateParameter
                            lCmd.Parameters.Add(lParameter)
                            lParameter.Value = Value(lColumnNumber)
                        End If
                    Next
                    lStr &= ") values ("
                    lStr = lStr.Replace(", )", ")")
                    For lColumnNumber = 1 To lCmd.Parameters.Count
                        lStr &= "?,"
                    Next
                    lStr &= ")"
                    lStr = lStr.Replace(",)", ")")
                    lCmd.CommandText = lStr.Replace(" Default,", " 'Default',") 'reserved word!
                    Logger.Dbg("ExecuteCommand:" & lCmd.CommandText & ":Parameters:" & lCmd.Parameters.Count)
                    lResult = lCmd.ExecuteNonQuery()
                    If lResult <> 1 Then 'should be one row updated, otherwise report
                        Logger.Dbg("ProblemValueChange:Row:" & Me.CurrentRecord & ":Result:" & lResult)
                    End If
                Next lRecordNumber
                lTransaction.Commit()
                pDataTable.TableName = lTableName
                RefreshTableNames(lDB)
            Catch ex As Exception
                Logger.Dbg(ex.Message)
                lReturn = False
            End Try
            If Not lDB Is Nothing Then lDB.Close()
        Else 'no file specified
            lReturn = False
        End If
        Return lReturn
    End Function

    Public Overrides Property NumRecords() As Integer
        Get
            Return pDataTable.Rows.Count
        End Get
        Set(ByVal newValue As Integer)
            Throw New ApplicationException("atcTableSQLite:NumRecords is ReadOnly (for now)")
        End Set
    End Property

    'undefined for this table type
    Public Overrides Property NumHeaderRows() As Integer
        Get
            Return 0
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property

    Public Overrides Property NumFields() As Integer
        Get
            Return pDataTable.Columns.Count
        End Get
        Set(ByVal newValue As Integer)
            While newValue > pDataTable.Columns.Count
                Dim lColumn As New DataColumn
                pDataTable.Columns.Add(lColumn)
                pModified = True
            End While
            'Throw New ApplicationException("atcTableSQLite:NumFields is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property CurrentRecord() As Integer
        Get
            Return pRow + 1 'back to 1 based
        End Get
        Set(ByVal newValue As Integer)
            If newValue > 0 And newValue <= pDataTable.Rows.Count Then
                pRow = newValue - 1 'make zero based
            Else
                Throw New ApplicationException("atcTableSQLite:CurrentRecord:" & newValue & ":NotBetween:0:" & pDataTable.Rows.Count - 1)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Dim lFieldNumber As Integer = aFieldNumber - 1
                Dim lStr As String
                Dim lValue As Object = pDataTable.Rows(pRow).Item(lFieldNumber)
                Try
                    lStr = lValue.ToString
                Catch
                    Logger.Dbg("ConversionError:Row:" & pRow + 1 & _
                               ":Column:" & aFieldNumber & _
                               ":Type:" & pDataTable.Columns(lFieldNumber).DataType.ToString)
                    lStr = ""
                End Try
                Return lStr
            End If
            Return Nothing
        End Get
        Set(ByVal newValue As String)
            If CheckFieldNumber(aFieldNumber) Then
                Try
                    Dim lRow As DataRow = pDataTable.Rows.Item(pRow)
                    lRow.BeginEdit()
                    lRow.Item(aFieldNumber - 1) = newValue
                    lRow.AcceptChanges()
                    pDataTable.AcceptChanges()
                Catch ex As ApplicationException
                    Logger.Msg("ValueSetFailed:Row:" & pRow & ":Column:" & aFieldNumber & ex.Message)
                End Try
            End If
        End Set
    End Property

    Public Function FieldAdd(ByVal aFieldName As String, ByVal aFieldType As String, ByVal aFieldLength As Integer) As Boolean
        Dim lType As System.Type
        Select Case aFieldType.ToLower
            Case "integer" : lType = System.Type.GetType("System.Int32")
            Case "string" : lType = System.Type.GetType("System.String")
            Case Else : lType = System.Type.GetType("Nothing")
        End Select
        Dim lColumn As New DataColumn
        lColumn.ColumnName = aFieldName
        lColumn.DataType = lType
        If aFieldType.ToLower = "string" Then
            lColumn.MaxLength = aFieldLength
        End If
        pDataTable.Columns.Add(lColumn)
        Return True
    End Function

    Public Overrides Function FieldNumber(aFieldName As String) As Integer

    End Function

    Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber - 1).ColumnName
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As String)
            If CheckFieldNumber(aFieldNumber) Then
                pDataTable.Columns(aFieldNumber - 1).ColumnName = newValue
            End If
        End Set
    End Property

    Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber - 1).MaxLength
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As Integer)
            If CheckFieldNumber(aFieldNumber) Then
                pDataTable.Columns(aFieldNumber - 1).MaxLength = newValue
            End If
        End Set
    End Property

    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber - 1).DataType.ToString()
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As String)
            If CheckFieldNumber(aFieldNumber) Then
                Throw New ApplicationException("atcTableSQLite:FieldType is ReadOnly, use FieldAdd")
            End If
        End Set
    End Property

    Public Overrides Sub ClearData()
        pDataTable.Clear()
    End Sub

    Public Overrides Sub Clear()
        ClearData()
        While pDataTable.Columns.Count > 0
            pDataTable.Columns.RemoveAt(0)
        End While
    End Sub

    Public ReadOnly Property TableNames() As atcCollection
        Get
            Return pTableNames
        End Get
    End Property

    ''' <summary>
    ''' check field number
    ''' </summary>
    ''' <param name="aFieldNumber">field number</param>
    ''' <returns></returns>
    ''' <remarks>follows atcTable convention - valid range starts at 1</remarks>
    Private Function CheckFieldNumber(ByVal aFieldNumber As Integer) As Boolean
        If aFieldNumber > 0 And aFieldNumber <= pDataTable.Columns.Count Then
            Return True
        Else
            Throw New ApplicationException("atcTableSQLite:FieldNumber" & aFieldNumber & ":NotBetween:1:" & pDataTable.Columns.Count)
        End If
    End Function

    Private Sub RefreshTableNames(ByVal aDb As System.Data.Common.DbConnection)
        Dim lTables As DataTable = aDb.GetSchema("tables")
        pTableNames.Clear()
        For lIndex As Integer = 0 To lTables.Rows.Count - 1
            Dim lDBTableName As String = lTables.Rows(lIndex).Item("TABLE_NAME")
            If Not lDBTableName.StartsWith("MSys") Then
                pTableNames.Add(lDBTableName)
            End If
        Next
    End Sub

    Private Function OpenDatabase(ByVal aDataBaseName As String) As System.Data.Common.DbConnection
        Dim lDB As System.Data.Common.DbConnection = Nothing
        Dim lConnectionString As String = ""
        If IO.Path.GetExtension(aDataBaseName) = ".mdb" Then
            lConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & aDataBaseName
            If Not FileExists(aDataBaseName) Then
                'TODO: create (or copy) an empty access database  
                'lConnectionString &= ";New=True"
                Throw New ApplicationException("Database " & aDataBaseName & " not found")
            End If
            lDB = New System.Data.OleDb.OleDbConnection(lConnectionString)
        Else
            lConnectionString = "Data Source=" & aDataBaseName
            lDB = New System.Data.SQLite.SQLiteConnection(lConnectionString)
        End If
        lDB.Open()
        RefreshTableNames(lDB)
        pRow = 0
        Return lDB
    End Function
End Class