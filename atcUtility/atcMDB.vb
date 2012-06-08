Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Reflection

''' <summary>
''' Opens a MDB file and returns tables within it as DataTable
''' </summary>
Public Class atcMDB
    Private pDataBaseType As String = ""
    Private pConnection As Object = Nothing
    Private pServiceManager As Object = Nothing
    Public Name As String 'this is the MDB file's full path and name

    ''' <summary>
    ''' Open an MDB file given its filename
    ''' </summary>
    ''' <param name="aFilename">Name of database file to open</param>
    Sub New(ByVal aFilename As String)
        Dim lPeKind As PortableExecutableKinds
        Dim lMachine As ImageFileMachine
        Me.GetType().Module.GetPEKind(lPeKind, lMachine)

        Select Case lMachine
            Case ImageFileMachine.AMD64, ImageFileMachine.IA64
                'type to use open office - TODO: polish details!!!
                pDataBaseType = "OO"
            Case ImageFileMachine.I386
                pDataBaseType = "ODBC" ',"OLE",
        End Select

        Dim lOpened As Boolean = False
        Try
            Select Case pDataBaseType
                Case "ODBC"
                    pConnection = New OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};Dbq=" & aFilename & ";")
                Case "OLE"
                    pConnection = New OleDbConnection
                    pConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & aFilename & ";"
                Case "OO"
                    'Root object for accessing OpenOffice from VB    
                    pServiceManager = CreateObject("com.sun.star.ServiceManager")
                    Dim lDatabaseContext As Object = pServiceManager.createInstance("com.sun.star.sdb.DatabaseContext")
                    Dim lDataSource As Object = lDatabaseContext.getByName("file:///" & IO.Path.ChangeExtension(aFilename, "odb"))
                    'TODO: points to mdb in the OO program folder - now kludged by making a copy there.
                    pConnection = lDataSource.getConnection("", "")
            End Select
            lOpened = True
        Catch ex As Exception
            Throw New ApplicationException("Failed to open " & aFilename)
        End Try

        If lOpened Then
            Name = aFilename
        Else
            Name = ""
        End If
    End Sub

    ''' <summary>
    ''' Open a table within the database
    ''' </summary>
    ''' <param name="aTableName">Name of a table</param>
    ''' <returns>System.Data.DataTable</returns>
    ''' <remarks>aTableName can be a query, "SELECT * from " will be prepended within GetTable</remarks>
    Public Function GetTable(ByVal aTableName As String) As DataTable

        Dim lSQL As String = aTableName
        If Not aTableName.ToUpper.StartsWith("SELECT") Then lSQL = "SELECT * from " & aTableName

        If pDataBaseType = "OO" Then
            Dim lTable As New DataTable
            Dim lStatement As Object = pConnection.createStatement

            Dim lResultSet As Object = lStatement.executeQuery(lSQL)
            If lResultSet Is Nothing Then
                Debug.Print("No Result From Query '" & lSQL & "'")
            Else
                Dim lMeta As Object = lResultSet.getMetaData
                Dim lFieldCount As Integer = lMeta.getColumnCount
                With lTable
                    Dim lColumn As DataColumn
                    For lFieldIndex As Integer = 1 To lFieldCount
                        lColumn = New DataColumn
                        With lColumn
                            .ColumnName = lMeta.getColumnName(lFieldIndex)
                        End With
                        .Columns.Add(lColumn)
                    Next
                    Dim lRow As DataRow
                    While lResultSet.next
                        lRow = .NewRow
                        For lFieldIndex As Integer = 1 To lFieldCount
                            lRow(lFieldIndex - 1) = lResultSet.GetString(lFieldIndex)
                        Next
                        .Rows.Add(lRow)
                    End While
                End With
            End If
            GetTable = lTable
        Else
            pConnection.Open()
            Dim lDataAdapter As Object = Nothing
            Select Case pDataBaseType
                Case "ODBC"
                    lDataAdapter = New OdbcDataAdapter(lSQL, DirectCast(pConnection, OdbcConnection))
                Case "OLE"
                    lDataAdapter = New OleDbDataAdapter(lSQL, DirectCast(pConnection, OleDbConnection))
            End Select
            Dim lDataSet As New DataSet
            lDataAdapter.Fill(lDataSet)
            GetTable = lDataSet.Tables(0)
            pConnection.Close()
        End If
    End Function

    Public Function InsertRowIntoTable(ByVal aTableName As String, ByVal aFieldValues As Collection) As Boolean
        pConnection.Open()
        Dim lCommand As New OdbcCommand
        lCommand.Connection = pConnection
        Dim lStr As String = aFieldValues(1)
        For lIndex As Integer = 2 To aFieldValues.Count
            lStr = lStr & "," & aFieldValues(lIndex)
        Next
        lCommand.CommandText = "INSERT INTO " & aTableName & " VALUES (" & lStr & ")"
        lCommand.ExecuteNonQuery()
        pConnection.Close()
        Return True
    End Function

    Public Function DeleteRowFromTable(ByVal aTableName As String, ByVal aFieldName As String, ByVal aID As String) As Boolean
        pConnection.Open()
        Dim lCommand As New OdbcCommand
        lCommand.Connection = pConnection
        lCommand.CommandText = "DELETE FROM " & aTableName & " WHERE " & aFieldName & " = " & aID
        lCommand.ExecuteNonQuery()
        pConnection.Close()
        Return True
    End Function
End Class