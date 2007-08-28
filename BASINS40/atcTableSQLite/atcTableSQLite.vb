Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility
Imports System.Data.SQLite
Imports System.Data.Common

Public Class atcTableSQLite
    Inherits atcTable

    Private pDataTable As DataTable 'rows and columns are zero based collections
    Private pRow As Integer = 0
    Private pModified As Boolean = False 'TODO: where/when do we check this?

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
        pModified = False
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
                Try
                    lStr = pDataTable.Rows(pRow).Item(lFieldNumber)
                Catch
                    Logger.Dbg("ConversionError:Row:" & pRow + 1 & ":Column:" & aFieldNumber)
                    lStr = pDataTable.Columns(lFieldNumber).DataType.ToString
                End Try
                Return lStr
            End If
            Return Nothing
        End Get
        Set(ByVal newValue As String)
            Throw New ApplicationException("atcTableSQLite:Value is ReadOnly (for now)")
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

End Class