Imports System.Data.Odbc

''' <summary>
''' Opens a MDB file and returns tables within it as DataTable
''' </summary>
Public Class atcMDB
    Private pConnection As OdbcConnection

    ''' <summary>
    ''' Open an MDB file given its filename
    ''' </summary>
    ''' <param name="aFilename">Name of database file to open</param>
    Sub New(ByVal aFilename As String)
        pConnection = New Odbc.OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};Dbq=" & aFilename & ";")
    End Sub

    ''' <summary>
    ''' Open a table within the database
    ''' </summary>
    ''' <param name="aTableName">Name of a table</param>
    ''' <returns>System.Data.DataTable</returns>
    ''' <remarks>aTableName can be a query, "SELECT * from " will be prepended within GetTable</remarks>
    Public Function GetTable(ByVal aTableName As String) As DataTable
        pConnection.Open()
        Dim lOdbcDataAdapter As New OdbcDataAdapter("SELECT * from " & aTableName, pConnection)
        Dim lDataSet As New DataSet
        lOdbcDataAdapter.Fill(lDataSet)
        GetTable = lDataSet.Tables(0)
        pConnection.Close()
    End Function
End Class