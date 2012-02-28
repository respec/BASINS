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
    End Sub

    ''' <summary>
    ''' Open a table within the database
    ''' </summary>
    ''' <param name="aTableName">Name of a table</param>
    ''' <returns>System.Data.DataTable</returns>
    ''' <remarks>aTableName can be a query, "SELECT * from " will be prepended within GetTable</remarks>
    Public Function GetTable(ByVal aTableName As String) As DataTable
        If pDataBaseType = "OO" Then
            Dim lTable As New DataTable
            Dim lStatement As Object = pConnection.createStatement
            Dim lSQL As String = "SELECT * FROM " & aTableName
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
                    lDataAdapter = New OdbcDataAdapter("SELECT * from " & aTableName, DirectCast(pConnection, OdbcConnection))
                Case "OLE"
                    lDataAdapter = New OleDbDataAdapter("SELECT * from " & aTableName, DirectCast(pConnection, OleDbConnection))
            End Select
            Dim lDataSet As New DataSet
            lDataAdapter.Fill(lDataSet)
            GetTable = lDataSet.Tables(0)
            pConnection.Close()
        End If
    End Function
End Class