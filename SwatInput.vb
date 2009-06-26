Imports System.Data.OleDb
Imports MapWinUtility
Imports atcUtility

''' <summary>
''' Class containing input needed for execution of SWAT2005 and methods to write text input files
''' </summary>
''' <remarks>Proof of concept - NOT COMPLETE</remarks>
Public Class SwatInput
#Region "Class Variables"
    Friend StatusBar As Windows.Forms.StatusBar = Nothing
    Friend CnSwatParm As OleDbConnection
    Friend CnSwatInput As OleDbConnection
    Friend CnSwatSoils As OleDbConnection
    Friend ProjectFolder As String = ""
    Friend TxtInOutFolder As String = ""

    Private pNeedToClose As Boolean = False
#End Region

#Region "Initialize and Shutdown"
    ''' <summary>
    ''' Initialize with existing open database connections
    ''' </summary>
    ''' <param name="aSwatGDB"></param>
    ''' <param name="aOutGDB"></param>
    ''' <param name="aOutputFolder"></param>
    ''' <param name="aStatusBar"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aSwatGDB As OleDbConnection, _
                   ByVal aOutGDB As OleDbConnection, _
                   ByVal aSwatSoilsDB As OleDbConnection, _
                   ByVal aOutputFolder As String, _
                   ByVal aScenario As String, _
                   ByVal aStatusBar As Windows.Forms.StatusBar)
        Initialize(aSwatGDB, aOutGDB, aSwatSoilsDB, aOutputFolder, aScenario, aStatusBar)
    End Sub

    ''' <summary>
    ''' Initialize with existing open database connections
    ''' </summary>
    ''' <param name="aSwatGDB"></param>
    ''' <param name="aOutGDB"></param>
    ''' <param name="aOutputFolder"></param>
    ''' <param name="aStatusBar"></param>
    ''' <remarks></remarks>
    Public Sub Initialize(ByVal aSwatGDB As OleDbConnection, _
                          ByVal aOutGDB As OleDbConnection, _
                          ByVal aSwatSoilsDB As OleDbConnection, _
                          ByVal aOutputFolder As String, _
                          ByVal aScenario As String, _
                          ByVal aStatusBar As Windows.Forms.StatusBar)
        ProjectFolder = aOutputFolder
        If ProjectFolder.Length > 0 Then
            IO.Directory.CreateDirectory(ProjectFolder)
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios")
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios\" & aScenario)
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios\" & aScenario & "\Scen")
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios\" & aScenario & "\TablesIn")
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios\" & aScenario & "\TablesOut")
            IO.Directory.CreateDirectory(aOutputFolder & "\Scenarios\" & aScenario & "\TxtInOut")
            IO.Directory.CreateDirectory(aOutputFolder & "\Watershed")
            IO.Directory.CreateDirectory(aOutputFolder & "\Watershed\Grid")
            IO.Directory.CreateDirectory(aOutputFolder & "\Watershed\Shapes")
            IO.Directory.CreateDirectory(aOutputFolder & "\Watershed\Tables")
            IO.Directory.CreateDirectory(aOutputFolder & "\Watershed\text")
        End If
        TxtInOutFolder = IO.Path.Combine(ProjectFolder, "Scenarios\" & aScenario & "\TxtInOut")
        CnSwatParm = aSwatGDB
        CnSwatInput = aOutGDB
        CnSwatSoils = aSwatSoilsDB
        StatusBar = aStatusBar
    End Sub

    ''' <summary>
    ''' Initialize with access database filenames
    ''' </summary>
    ''' <param name="aSwatGDB"></param>
    ''' <param name="aOutGDB"></param>
    ''' <param name="aProjectFolder"></param>
    ''' <remarks>Opens database connections</remarks>
    Public Sub New(ByVal aSwatGDB As String, _
                   ByVal aOutGDB As String, _
                   ByVal aProjectFolder As String, _
                   ByVal aScenario As String)

        CnSwatParm = OpenOleDB(aSwatGDB)

        Dim lSwatSoilsDBFileName As String = aSwatGDB.Replace("SWAT2005.mdb", "SWAT_US_Soils.mdb")
        CnSwatSoils = OpenOleDB(lSwatSoilsDBFileName)

        If Not IO.File.Exists(aOutGDB) Then
            CreateAccessDatabase(aOutGDB, aProjectFolder)
        End If
        If CnSwatInput Is Nothing Then
            CnSwatInput = OpenOleDB(aOutGDB)
        End If

        Initialize(CnSwatParm, CnSwatInput, CnSwatSoils, aProjectFolder, aScenario, Nothing)
        pNeedToClose = True
    End Sub

    ''' <summary>
    ''' Close locally opened databases and unset other state variables  
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Close()
        If pNeedToClose Then
            If CnSwatInput IsNot Nothing Then
                CnSwatInput.Close()
                CnSwatInput = Nothing
            End If
            If CnSwatParm IsNot Nothing Then
                CnSwatParm.Close()
                CnSwatParm = Nothing
            End If
            ProjectFolder = ""
            TxtInOutFolder = ""
            pNeedToClose = False
            StatusBar = Nothing
        End If
    End Sub
#End Region

#Region "Utility Routines - Public"
    ''' <summary>
    ''' Generic routine to query the SWAT parameter input database
    ''' </summary>
    ''' <param name="aQuery">SQL query</param>
    ''' <returns>DataTable of query results</returns>
    ''' <remarks></remarks>
    Public Function QueryInputDB(ByVal aQuery As String) As DataTable
        Return QueryDB(aQuery, CnSwatInput)
    End Function

    ''' <summary>
    ''' Generic routine to query the SWAT geodatabase
    ''' </summary>
    ''' <param name="aQuery">SQL query</param>
    ''' <returns>DataTable of query results</returns>
    ''' <remarks></remarks>
    Public Function QueryGDB(ByVal aQuery As String) As DataTable
        Return QueryDB(aQuery, CnSwatParm)
    End Function

    Public Function QuerySoils(ByVal aQuery As String) As DataTable
        Return QueryDB(aQuery, CnSwatSoils)
    End Function

    ''' <summary>
    ''' Generic routine to copy a table in the SWAT parameter input database
    ''' </summary>
    ''' <param name="aTableNameSource">Source table name</param>
    ''' <param name="aTableNameTarget">Target table name</param>
    ''' <remarks>Overwrites existing output table if required</remarks>
    Public Sub CopyTableInputDB(ByVal aTableNameSource As String, ByVal aTableNameTarget As String)
        Dim lTableSchema As DataTable = CnSwatInput.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, aTableNameSource, "TABLE"})
        If lTableSchema.Rows.Count = 1 Then
            lTableSchema = CnSwatInput.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, aTableNameTarget, "TABLE"})
            If lTableSchema.Rows.Count = 1 Then
                Dim lSQLDrop As String = "DROP TABLE " & aTableNameTarget
                Dim lDropCommand As OleDbCommand = New OleDbCommand(lSQLDrop, CnSwatInput)
                lDropCommand.CommandTimeout = 30
                lDropCommand.ExecuteNonQuery()
            End If
            Dim lSQL As String = "SELECT * INTO " & aTableNameTarget & " FROM(" & aTableNameSource & ")"
            Dim lUpdateCommand As OleDbCommand = New OleDbCommand(lSQL, CnSwatInput)
            lUpdateCommand.CommandTimeout = 30
            lUpdateCommand.ExecuteNonQuery()
        Else
            Throw New ApplicationException("Table " & aTableNameSource & " does not exist to copy")
        End If
    End Sub

    ''' <summary>
    ''' Generic routine to update a field value in a specified field in rows meeting specified criteria 
    ''' in a specified table in the SWAT parameter input database
    ''' </summary>
    ''' <param name="aTableName">Name of table containing values to update</param>
    ''' <param name="aIdFieldName">Match criteria field name</param>
    ''' <param name="aId">Match value</param>
    ''' <param name="aValueFieldName">Update field name</param>
    ''' <param name="aValue">New value</param>
    ''' <remarks></remarks>
    Public Sub UpdateInputDB(ByVal aTableName As String, _
                             ByVal aIdFieldName As String, ByVal aId As Integer, _
                             ByVal aValueFieldName As String, ByVal aValue As String)
        Dim lSQL As String = ""
        Dim lNumber As Double
        If Double.TryParse(aValue, lNumber) Then
            lSQL = "UPDATE " & aTableName _
                 & " SET " & aValueFieldName & " = " & aValue _
                 & " WHERE " & aIdFieldName & " = " & aId
        Else
            lSQL = "UPDATE " & aTableName _
                & " SET " & aValueFieldName & " = '" & aValue & "'" _
                & " WHERE " & aIdFieldName & " = " & aId
        End If
        Dim lUpdateCommand As OleDbCommand = New OleDbCommand(lSQL, CnSwatInput)
        lUpdateCommand.CommandTimeout = 30
        lUpdateCommand.ExecuteNonQuery()
    End Sub

    Public Sub UpdateInputDB(ByVal aTableName As String, _
                             ByVal aWhereClause As String, _
                             ByVal aValueFieldName As String, ByVal aValue As String)
        Dim lSQL As String = ""
        Dim lNumber As Double
        If Double.TryParse(aValue, lNumber) Then
            lSQL = "UPDATE " & aTableName _
                 & " SET " & aValueFieldName & " = " & aValue _
                 & " WHERE " & aWhereClause
        Else
            lSQL = "UPDATE " & aTableName _
                 & " SET " & aValueFieldName & " = '" & aValue & "'" _
                 & " WHERE " & aWhereClause
        End If
        Dim lUpdateCommand As OleDbCommand = New OleDbCommand(lSQL, CnSwatInput)
        lUpdateCommand.CommandTimeout = 30
        lUpdateCommand.ExecuteNonQuery()
    End Sub


    Public Sub DeleteRowInputDB(ByVal aTableName As String, _
                                ByVal aIdFieldName As String, ByVal aId As Integer)
        Dim lSQL As String = "DELETE FROM " & aTableName & " WHERE " & aIdFieldName & "=" & aId & ";"
        Dim lDeleteCommand As OleDbCommand = New OleDbCommand(lSQL, CnSwatInput)
        lDeleteCommand.CommandTimeout = 30
        lDeleteCommand.ExecuteNonQuery()
    End Sub

    Public Sub SaveAllTextInput()
        Dim a As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
        Dim lNewArgs() As Object = {Me}
        Dim lSaveArgs1() As Object = {Nothing}
        Dim lSaveArgs2() As Object = {Nothing, Nothing}
        Dim lSaveMethodNames() As String = {"Save", "Save1", "Save2"}

        For Each lType As Type In a.GetTypes
            Debug.Print(lType.Name)
            Select Case lType.Name
                Case "MySettings"
                Case Else
                    For Each lMethodName As String In lSaveMethodNames
                        Dim lSaveMethod As System.Reflection.MethodInfo = lType.GetMethod(lMethodName)
                        If Not lSaveMethod Is Nothing Then
                            Try
                                Dim lObject As Object = lType.InvokeMember("New", Reflection.BindingFlags.CreateInstance _
                                                                                + Reflection.BindingFlags.NonPublic _
                                                                                + Reflection.BindingFlags.Instance, Nothing, Nothing, lNewArgs)
                                Select Case lType.Name
                                    Case "clsChm", "clsMgt"
                                        lSaveMethod.Invoke(lObject, lSaveArgs2)
                                    Case Else
                                        lSaveMethod.Invoke(lObject, lSaveArgs1)
                                End Select
                            Catch e As Exception
                                Logger.Dbg("Could not save " & lType.Name & ": " & e.Message)
                            End Try
                        End If
                    Next
            End Select
        Next
    End Sub

    Friend Sub Status(ByVal aStatus As String)
        If StatusBar IsNot Nothing Then
            StatusBar.Text = aStatus
        End If
    End Sub
#End Region

    Private Function CreateAccessDatabase(ByVal aDatabaseName As String, ByVal aDatabaseFullPath As String) As Boolean
        Dim lResult As Boolean
        Try
            Dim lDatabaseFullPathAndFile As String = IO.Path.Combine(aDatabaseFullPath, aDatabaseName)
            If IO.File.Exists(lDatabaseFullPathAndFile) Then
                IO.File.Delete(lDatabaseFullPathAndFile)
            End If

            Dim lConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lDatabaseFullPathAndFile
            Dim lCatalog As New ADOX.Catalog()
            lCatalog.Create(lConnectionString)

            CnSwatInput = OpenOleDB(lDatabaseFullPathAndFile)

            Dim lAssembly As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly
            Dim lNewArgs() As Object = {Me}
            Dim lArgs(-1) As Object

            For Each lType As Type In lAssembly.GetTypes
                Debug.Print(lType.Name)
                Dim lTableCreateMethod As System.Reflection.MethodInfo = lType.GetMethod("TableCreate")
                If Not lTableCreateMethod Is Nothing Then
                    Try
                        Dim lObject As Object = lType.InvokeMember("New", Reflection.BindingFlags.CreateInstance _
                                                                        + Reflection.BindingFlags.NonPublic _
                                                                        + Reflection.BindingFlags.Instance, Nothing, Nothing, lNewArgs)
                        lTableCreateMethod.Invoke(lObject, lArgs)
                    Catch e As Exception
                        Logger.Dbg("Could not create table for " & lType.Name & ": " & e.Message)
                    End Try
                End If
            Next
            lResult = True

            'If DatabaseName = "RasterStore.mdb" Then
            '    FindAndCreateFile(DatabaseFullPath, "rasterDBConfig.txt", conStr)
            'Else
            '    FindAndCreateFile(DatabaseFullPath, "prjDBConfig.txt", conStr)
            'End If
        Catch Excep As System.Runtime.InteropServices.COMException
            lResult = False
            Logger.Dbg(Excep.Message)
        End Try
        Return lResult
    End Function

    Private Function OpenADOConnection() As ADODB.Connection
        'Open the connection
        Dim lConnection As New ADODB.Connection
        lConnection.Open(CnSwatInput.ConnectionString)
        Return lConnection
    End Function
    Private Function OpenOleDB(ByVal aFileName As String) As OleDb.OleDbConnection
        Dim lOleDbConnection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & aFileName)
        lOleDbConnection.Open()
        Return lOleDbConnection
    End Function

    Public Function FieldUnits(ByVal aTableName As String, ByVal aFieldName As String) As String
        Select Case aTableName.ToLower
            Case "bsn"
                Select Case aFieldName.ToUpper
                    Case "SFTMP" : Return ""
                End Select
        End Select
        Return ""
    End Function

End Class


