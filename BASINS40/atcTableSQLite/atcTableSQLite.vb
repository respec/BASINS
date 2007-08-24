Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility
Imports System.Data.SQLite
Imports System.Data.Common

Public Class atcTableSQLite
    Inherits atcTable

    Private pDB As SQLiteConnection
    Private pCmd As SQLiteCommand
    Private pAdapter As SQLiteDataAdapter
    Private pDataSet As DataSet
    Private pRow As Integer = 0

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
            pCmd = New SQLiteCommand(pDB)
            pCmd.CommandText = "Select * from " & lFileAndTableName 'now just table name
            pAdapter = New SQLiteDataAdapter
            pAdapter.SelectCommand = pCmd
            pDataSet = New DataSet
            Dim iResult As Integer = pAdapter.Fill(pDataSet)

            Logger.Dbg("FieldCount:" & pDataSet.Tables(0).Columns.Count & ":Rows:" & pDataSet.Tables(0).Rows.Count)
            Logger.Dbg("FieldDetails")
            Dim lStr As String = "Name"
            For lField As Integer = 0 To pDataSet.Tables(0).Columns.Count - 1
                lStr &= ":" & pDataSet.Tables(0).Columns(lField).ColumnName
            Next
            Logger.Dbg(lStr)
            lStr = "DataType"
            For lField As Integer = 0 To pDataSet.Tables(0).Columns.Count - 1
                lStr &= ":" & pDataSet.Tables(0).Columns(lField).DataType.ToString
            Next
            Logger.Dbg(lStr)
            lStr = "Length"
            For lField As Integer = 0 To pDataSet.Tables(0).Columns.Count - 1
                lStr &= ":" & pDataSet.Tables(0).Columns(lField).MaxLength
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
            Return pDataSet.Tables(0).Rows.Count
        End Get
        Set(ByVal newValue As Integer)
            Throw New ApplicationException("atcTableSQLite:NumRecords is ReadOnly (for now)")
        End Set
    End Property

    Public Overrides Property NumFields() As Integer
        Get
            Return pDataSet.Tables(0).Columns.Count
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
            If newValue >= 0 And newValue < pDataSet.Tables(0).Rows.Count Then
                pRow = newValue
            Else
                Throw New ApplicationException("atcTableSQLite:CurrentRecord:" & newValue & ":NotBetween:0:" & pDataSet.Tables(0).Rows.Count - 1)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Dim lStr As String
                Try
                    lStr = pDataSet.Tables(0).Rows(pRow).Item(aFieldNumber)
                Catch
                    Logger.Dbg("ConversionError:Row:" & pRow & ":Column:" & aFieldNumber)
                    lStr = pDataSet.Tables(0).Columns(aFieldNumber).DataType.ToString
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
                Return pDataSet.Tables(0).Columns(aFieldNumber).ColumnName
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As String)
            If CheckFieldNumber(aFieldNumber) Then
                pDataSet.Tables(0).Columns(aFieldNumber).ColumnName = newValue
            End If
        End Set
    End Property

    Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
        Get
            If CheckFieldNumber(aFieldNumber) Then
                Return pDataSet.Tables(0).Columns(aFieldNumber).MaxLength
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
                Return pDataSet.Tables(0).Columns(aFieldNumber).DataType.ToString()
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
        If aFieldNumber >= 0 And aFieldNumber < pDataSet.Tables(0).Columns.Count Then
            Return True
        Else
            Throw New ApplicationException("atcTableSQLite:FieldNumber" & aFieldNumber & ":NotBetween:0:" & pDataSet.Tables(0).Columns.Count - 1)
        End If
    End Function
End Class

