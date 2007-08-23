Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility
Imports System.Data.SQLite
Imports System.Data

Public Class atcTableSQLite
    Inherits atcTable

    Private pDB As SQLiteConnection

    Public Overrides Function Cousin() As IatcTable

    End Function

    Public Overrides Function CreationCode() As String

    End Function

    Public Overrides Function OpenFile(ByVal aFileName As String) As Boolean
        Try
            Dim lFileAndTableName As String = aFileName
            Dim lConnectionString As String = "Data Source=" & StrSplit(lFileAndTableName, vbTab, "'")
            pDB = New SQLiteConnection(lConnectionString)
            pDB.Open()
            Dim lCmd As New SQLiteCommand(pDB)
            lCmd.CommandText = "Select * from " & lFileAndTableName 'no just table name
            Dim lReader As Common.DbDataReader = lCmd.ExecuteReader
            Dim lSchemaTable As DataTable = lReader.GetSchemaTable
            Logger.Dbg(lSchemaTable.Rows.Count & ":" & lSchemaTable.Columns.Count)
        Catch ex As ApplicationException
            Logger.Msg(ex.Message)
            Return False
        End Try
        Return True

    End Function

    Public Overrides Function WriteFile(ByVal aFileName As String) As Boolean

    End Function

    Public Overrides Property NumRecords() As Integer
        Get

        End Get
        Set(ByVal newValue As Integer)

        End Set
    End Property

    Public Overrides Property NumFields() As Integer
        Get

        End Get
        Set(ByVal newValue As Integer)

        End Set
    End Property

    Public Overrides Property CurrentRecord() As Integer
        Get

        End Get
        Set(ByVal newValue As Integer)

        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get

        End Get
        Set(ByVal newValue As String)

        End Set
    End Property

    Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
        Get

        End Get
        Set(ByVal newValue As String)

        End Set
    End Property

    Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
        Get

        End Get
        Set(ByVal newValue As Integer)

        End Set
    End Property

    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get

        End Get
        Set(ByVal newValue As String)

        End Set
    End Property

    Public Overrides Sub ClearData()

    End Sub

    Public Overrides Sub Clear()

    End Sub
End Class

