Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility
Imports System.Data.SQLite
Imports System.Data.Common

Public Class atcTableSQLite
    Inherits atcTable

    Private pDataTable As DataTable
    Private pRow As Integer = 0

    Public Overrides Function Cousin() As IatcTable
        Return Nothing
    End Function

    Public Overrides Function CreationCode() As String
        Return ""
    End Function

    Public Overrides Function OpenFile(ByVal aFileName As String) As Boolean
        Try
            Dim lFileAndTableName As String = aFileName
            Dim lConnectionString As String = "Data Source=" & StrSplit(lFileAndTableName, vbTab, "'")
            Dim lDB As SQLiteConnection
            Dim lCmd As SQLiteCommand
            Dim lAdapter As SQLiteDataAdapter
            lDB = New SQLiteConnection(lConnectionString)
            lDB.Open()
            lCmd = New SQLiteCommand(lDB)
            lCmd.CommandText = "Select * from " & lFileAndTableName 'now just table name
            lAdapter = New SQLiteDataAdapter
            lAdapter.SelectCommand = lCmd
            pDataTable = New DataTable
            Dim iResult As Integer = lAdapter.Fill(pDataTable)

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
            Return pDataTable.Rows.Count
        End Get
        Set(ByVal newValue As Integer)
            Throw New ApplicationException("atcTableSQLite:NumRecords is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property NumFields() As Integer
        Get
            Return pDataTable.Columns.Count
        End Get
        Set(ByVal newValue As Integer)
            Throw New ApplicationException("atcTableSQLite:NumFields is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property CurrentRecord() As Integer
        Get
            Return pRow
        End Get
        Set(ByVal newValue As Integer)
            If newValue >= 0 And newValue < pDataTable.Rows.Count Then
                pRow = newValue
            Else
                Throw New ApplicationException("atcTableSQLite:CurrentRecord:" & newValue & ":NotBetween:0:" & pDataTable.Rows.Count - 1)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Dim lStr As String
                Try
                    lStr = pDataTable.Rows(pRow).Item(aFieldNumber)
                Catch
                    Logger.Dbg("ConversionError:Row:" & pRow & ":Column:" & aFieldNumber)
                    lStr = pDataTable.Columns(aFieldNumber).DataType.ToString
                End Try
                Return lStr
            End If
            Return Nothing
        End Get
        Set(ByVal newValue As String)
            Throw New ApplicationException("atcTableSQLite:Value is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber).ColumnName
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As String)
            If CheckFieldNumber(aFieldNumber) Then
                pDataTable.Columns(aFieldNumber).ColumnName = newValue
            End If
        End Set
    End Property

    Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber).MaxLength
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As Integer)
            Throw New ApplicationException("atcTableSQLite:FieldLength is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataTable.Columns(aFieldNumber).DataType.ToString()
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As String)
            Throw New ApplicationException("atcTableSQLite:FieldType is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Sub ClearData()

    End Sub

    Public Overrides Sub Clear()

    End Sub

    Private Function CheckFieldNumber(ByVal aFieldNumber As Integer) As Boolean
        If aFieldNumber >= 0 And aFieldNumber < pDataTable.Columns.Count Then
            Return True
        Else
            Throw New ApplicationException("atcTableSQLite:FieldNumber" & aFieldNumber & ":NotBetween:0:" & pDataTable.Columns.Count - 1)
        End If
    End Function
End Class

