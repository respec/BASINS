Imports System.Data.OleDb
Imports MapWinUtility
Imports MapWinUtility.Strings


Module modUtility

    Friend Function HeaderString() As String
        'For debugging, put a dummy date in all output files so we can compare files and not find unimportant differences
        Return "6/13/2008 12:00:00 AM D4EM-SWAT Interface"
        'Return Date.Now.ToString & " D4EM-SWAT Interface"
    End Function

    Friend Function StringFname(ByVal aSubBasin As String, ByVal aType As String) As String
        If aSubBasin.Length > 5 Then
            Throw New ApplicationException("Subbasin " & aSubBasin & " Problem")
        End If
        If aType.StartsWith(".") Then aType = aType.Substring(1)
        Return aSubBasin.PadLeft(5, "0") & "0000." & aType
    End Function

    Friend Function StringFnameHRUs(ByVal aSub As String, ByVal aHRU As String) As String
        If aSub.Length > 5 OrElse aHRU.Length > 4 Then
            Throw New ApplicationException("Subbasin " & aSub & " Hru " & aHRU & " Problem")
        End If
        Return aSub.PadLeft(5, "0") & aHRU.PadLeft(4, "0")
    End Function

    ''' <summary>
    ''' Create fixed with column string for writing to text file
    ''' </summary>
    ''' <param name="aValue">Number to format</param>
    ''' <param name="aDec">Number of decimal digits</param>
    ''' <param name="aSpc">Spaces to add after number</param>
    ''' <param name="aLeft">???</param>
    ''' <returns>Formatted string</returns>
    ''' <remarks>TODO: simplify!</remarks>
    Friend Function MakeString(ByVal aValue As Object, ByVal aDec As Integer, ByVal aSpc As Integer, ByVal aLeft As Integer) As String
        Dim lBufthevalue As String
        aValue = FormatNumber(aValue, aDec, TriState.True, TriState.False, TriState.False)

        Dim lValue As Integer = 16 - Left(aValue.ToString, aLeft).Length
        If (lValue = 0) Then
            lBufthevalue = ""
        Else
            lBufthevalue = Space(lValue)
        End If
        Return lBufthevalue & aValue.ToString & Space(aSpc)
    End Function

    Friend Sub ReplaceNonAscii(ByVal aFilename As String)
        Dim lBytes As Byte() = IO.File.ReadAllBytes(aFilename)
        Dim lByteIndex As Integer = 0
        Dim lLastByte As Integer = lBytes.GetUpperBound(0)
        While lByteIndex < lBytes.Length
            If lBytes(lByteIndex) = 194 Then
                For lByteCopyTo As Integer = lByteIndex To lLastByte - 1
                    lBytes(lByteCopyTo) = lBytes(lByteCopyTo + 1)
                Next
                lLastByte -= 1
            Else
                lByteIndex += 1
            End If
        End While
        If lLastByte < lBytes.GetUpperBound(0) Then
            Array.Resize(lBytes, lLastByte + 1)
            IO.File.WriteAllBytes(aFilename, lBytes)
        End If
    End Sub

    Friend Function GroupOfStrings(ByVal aPattern As String, ByVal aCount As Integer, ByVal aDelimiter As String) As String
        GroupOfStrings = ""
        For i As Integer = 1 To aCount
            GroupOfStrings &= aPattern.Replace("#", i.ToString)
            If i < aCount Then GroupOfStrings &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As Single, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As Double, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function ArrayToString(ByVal aArray() As String, ByVal aDelimiter As String) As String
        ArrayToString = ""
        For i As Integer = 0 To aArray.GetUpperBound(0)
            ArrayToString &= aArray(i)
            If i < aArray.GetUpperBound(0) Then ArrayToString &= aDelimiter
        Next
    End Function

    Friend Function QueryDB(ByVal aQuery As String, ByVal aConnection As OleDbConnection) As DataTable
        Dim lCommand As OleDbCommand = New OleDbCommand(aQuery, aConnection)
        lCommand.CommandTimeout = 30
        Dim lOleDbDataAdapter As New OleDbDataAdapter
        lOleDbDataAdapter.SelectCommand = lCommand
        Dim lDataSet As New DataSet
        Dim lTableName As String = "lTable"
        lOleDbDataAdapter.Fill(lDataSet, lTableName)
        Return lDataSet.Tables(lTableName)
    End Function

    Public Function ExecuteNonQuery(ByVal aSQL As String, ByVal aConnection As OleDbConnection) As Boolean
        Dim lCommand As New System.Data.OleDb.OleDbCommand(aSQL, aConnection)
        lCommand.CommandTimeout = 30
        Dim lStartTime As Date = Date.Now
        'Dim lLogged As Boolean = False
TryExecute:
        Try
            lCommand.ExecuteNonQuery()
            Return True
        Catch e As Exception
            Windows.Forms.Application.DoEvents()
            If Date.Now.Subtract(lStartTime).Seconds < lCommand.CommandTimeout Then
                System.Threading.Thread.Sleep(100)
                'If Not lLogged Then
                '    Logger.Dbg("ExecuteNonQuery Exception: " & e.Message & ", Retrying: " & aSQL)
                '    Logger.Flush()
                '    lLogged = True
                'End If
                GoTo TryExecute
            Else
                Logger.Dbg("ExecuteNonQuery: " & e.Message & vbCrLf & aSQL)
                Return False
            End If
        End Try
    End Function

    'Function find whether a table exist or not
    Friend Function IsTableExist(ByVal strTable As String, ByVal aConnection As OleDb.OleDbConnection) As Boolean
        Try

            Dim schemaTable As DataTable = aConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
            Dim I As Int32
            For I = 0 To schemaTable.Rows.Count - 1
                Dim rd As DataRow = schemaTable.Rows(I)
                If rd("TABLE_TYPE").ToString = "TABLE" Then
                    If (rd("TABLE_NAME").ToString) = strTable Then
                        'Table exists
                        IsTableExist = True
                        Exit For
                    Else
                        'Table not exists
                        IsTableExist = False
                    End If
                End If
            Next
        Catch ex As Exception
            Logger.Dbg(ex.Message)
        End Try
    End Function

    'Friend Sub CreateTable(ByVal aTableName As String, ByVal aKeyName As String, ByVal aColumns As String, ByVal aConnection As OleDb.OleDbConnection)
    '    DropTable(aTableName, aConnection)
    '    ExecuteNonQuery("CREATE TABLE " & aTableName & " (" _
    '                      & aKeyName & " INT IDENTITY PRIMARY KEY, " _
    '                      & aColumns & ");", aConnection)
    'End Sub

    'Friend Function CreateTable(ByVal aTableName As String, ByVal aKeyName As String, ByVal aColumnNames() As String, ByVal aColumnTypes() As String, ByVal ColumnCounts() As Integer, ByVal aConnection As OleDb.OleDbConnection) As Boolean
    '    DropTable(aTableName, aConnection)
    '    Dim lSQL As String = "CREATE TABLE " & aTableName & " (" _
    '                      & aKeyName & " INT IDENTITY PRIMARY KEY, "
    '    Dim lLastIndex As Integer = aColumnNames.GetUpperBound(0)
    '    For lColumnNameIndex As Integer = 0 To lLastIndex
    '        If Not ColumnCounts Is Nothing AndAlso ColumnCounts(lColumnNameIndex) > 1 Then
    '            For lColumnCountIndex As Integer = 1 To ColumnCounts(lColumnNameIndex)
    '                lSQL &= aColumnNames(lColumnNameIndex) & lColumnCountIndex & " " & aColumnTypes(lColumnNameIndex)
    '                If lColumnCountIndex < ColumnCounts(lColumnNameIndex) Then lSQL &= ", "
    '            Next
    '        Else
    '            lSQL &= aColumnNames(lColumnNameIndex) & " " & aColumnTypes(lColumnNameIndex)
    '        End If
    '        If lColumnNameIndex < lLastIndex Then lSQL &= ", "
    '    Next
    '    ExecuteNonQuery(lSQL & ");", aConnection)
    'End Function

    Friend Function CreateTable(ByVal aTableName As String, ByVal aKeyName As String, ByVal aDataColumns As Generic.List(Of clsDataColumn), ByVal aConnection As OleDb.OleDbConnection) As Boolean
        DropTable(aTableName, aConnection)
        Dim lSQL As String = "CREATE TABLE " & aTableName & " (" _
                          & aKeyName & " INT IDENTITY PRIMARY KEY, "

        For Each lField As clsDataColumn In aDataColumns
            With lField
                Dim lTypeString As String = .TypeString
                If lTypeString.ToLower = "string" Then
                    Logger.Dbg(aTableName & "." & .Name & ": String should be changed to VARCHAR(field width)")
                    lTypeString = "VARCHAR"
                End If
                lTypeString = " " & lTypeString & ", "

                If .Count > 1 Then
                    For lColumnCountIndex As Integer = 1 To .Count
                        lSQL &= .Name & lColumnCountIndex & lTypeString
                    Next
                Else
                    lSQL &= .Name & lTypeString
                End If
            End With
        Next
        'Trim trailing ", " and add ");"
        ExecuteNonQuery(lSQL.Substring(0, lSQL.Length - 2) & ");", aConnection)
    End Function

    Friend Function DropTable(ByVal tableName As String, ByVal aConnection As OleDb.OleDbConnection) As Boolean
        If IsTableExist(tableName, aConnection) Then
            ExecuteNonQuery("DROP TABLE " & tableName & ";", aConnection)
        End If
    End Function

    Public Sub SaveTableAsText(ByVal aTable As DataTable, ByVal aColumns As Generic.List(Of clsDataColumn), ByVal aSaveFileName As String)
        Dim lSB As New Text.StringBuilder
        For Each lRow As DataRow In aTable.Rows
            For Each lColumn As clsDataColumn In aColumns
                If lColumn.TextFormat.Length > 0 Then
                    If lColumn.TextFormat = "%s" Then
                        lSB.Append(lRow.Item(lColumn.Name).ToString.PadLeft(lColumn.TextPad))
                    Else
                        lSB.Append(Format(lRow.Item(lColumn.Name), lColumn.TextFormat).PadLeft(lColumn.TextPad))
                    End If
                End If
                lSB.Append(lColumn.TextSuffix)
            Next
            lSB.AppendLine()
        Next
        IO.File.WriteAllText(aSaveFileName, lSB.ToString)
    End Sub

    ''' <summary>
    ''' Create a new list of objects, one for each record of the table, populated from columns of table
    ''' </summary>
    ''' <param name="aObjectType"></param>
    ''' <param name="aFieldMap">Mapping of object field names as keys to table field names as items</param>
    ''' <param name="aNewArgs"></param>
    ''' <param name="aNewBindingFlags"></param>
    ''' <returns></returns>
    ''' <remarks>If aFieldMap is Nothing, default mapping of exactly the same field names is attempted</remarks>
    Public Function PopulateObjects(ByVal aTable As DataTable, _
                                    ByVal aObjectType As Type, _
                           Optional ByVal aFieldMap As atcUtility.atcCollection = Nothing, _
                           Optional ByVal aNewArgs() As Object = Nothing, _
                           Optional ByVal aNewBindingFlags As Reflection.BindingFlags = Reflection.BindingFlags.CreateInstance _
                                                                                      + Reflection.BindingFlags.Public _
                                                                                      + Reflection.BindingFlags.Instance) As ArrayList
        Dim lNewList As New ArrayList
        For Each lRow As DataRow In aTable.Rows
            Dim lObject As Object = aObjectType.InvokeMember("New", aNewBindingFlags, Nothing, Nothing, aNewArgs)
            PopulateObject(lRow, lObject, aFieldMap)
            lNewList.Add(lObject)
        Next
        Return lNewList
    End Function

    ''' <summary>
    ''' Populate an object from the current record of the table
    ''' </summary>
    ''' <param name="aObject">Object to populate</param>
    ''' <param name="aFieldMap">Mapping of object field names as keys to table field names as items</param>
    ''' <remarks>If aFieldMap is Nothing, default mapping of exactly the same field names is attempted</remarks>
    Public Sub PopulateObject(ByVal aRow As DataRow, ByRef aObject As Object, Optional ByVal aFieldMap As atcUtility.atcCollection = Nothing)
        Dim lType As Type = aObject.GetType
        For Each lField As Reflection.FieldInfo In lType.GetFields()
            Dim lValue As Object = Nothing
            Try
                If aFieldMap IsNot Nothing AndAlso aFieldMap.Keys.Contains(lField.Name) Then
                    lValue = aRow.Item(aFieldMap.ItemByKey(lField.Name))
                Else
                    lValue = aRow.Item(lField.Name)
                End If
            Catch
            End Try
            If lValue IsNot Nothing Then
                If Not IsDBNull(lValue) Then
                    Select Case Type.GetTypeCode(lField.FieldType)
                        Case TypeCode.Boolean : lField.SetValue(aObject, CBool(lValue))
                        Case TypeCode.Byte : lField.SetValue(aObject, CByte(lValue))
                        Case TypeCode.Char : lField.SetValue(aObject, CChar(lValue))
                        Case TypeCode.DateTime : lField.SetValue(aObject, CDate(lValue))
                        Case TypeCode.Decimal : lField.SetValue(aObject, CDec(lValue))
                        Case TypeCode.Double : lField.SetValue(aObject, CDbl(lValue))
                        Case TypeCode.Int16 : lField.SetValue(aObject, CShort(lValue))
                        Case TypeCode.Int32 : lField.SetValue(aObject, CInt(lValue))
                        Case TypeCode.Int64 : lField.SetValue(aObject, CLng(lValue))
                        Case TypeCode.SByte : lField.SetValue(aObject, CSByte(lValue))
                        Case TypeCode.Single : lField.SetValue(aObject, CSng(lValue))
                        Case TypeCode.String : lField.SetValue(aObject, CStr(lValue))
                        Case TypeCode.UInt16 : lField.SetValue(aObject, CUShort(lValue))
                        Case TypeCode.UInt32 : lField.SetValue(aObject, CUInt(lValue))
                        Case TypeCode.UInt64 : lField.SetValue(aObject, CULng(lValue))
                    End Select
                End If
            End If
        Next
    End Sub

    Function SqlAddToUpdate(ByVal aAddSQL As String, ByVal aWhereClause As String) As String
        Dim lAddSQLSplit() As String = aAddSQL.Split("("c, ")"c)
        Dim lTableName As String = lAddSQLSplit(0).Substring(12).Trim 'get rid of INSERT INTO
        Dim lSQL As String = "UPDATE " & lTableName & " SET "

        Dim lFieldNames() As String = lAddSQLSplit(1).Split(",")
        Dim lFieldValueString As String = lAddSQLSplit(3)
        For Each lFieldName As String In lFieldNames
            lSQL &= lFieldName & " = '" & StrSplit(lFieldValueString, ",", "'") & "',"
        Next
        lSQL = lSQL.Substring(0, lSQL.Length - 1) & " WHERE " & aWhereClause
        Return lSQL
    End Function
End Module
