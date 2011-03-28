Option Strict Off
Option Explicit On

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Interface IatcTable
    Function Cousin() As IatcTable
    Function CreationCode() As String

    Function OpenFile(ByVal aFileName As String) As Boolean
    Function WriteFile(ByVal aFileName As String) As Boolean

    Function FieldNumber(ByVal aFieldName As String) As Integer
    Function FindFirst(ByVal aFieldNumber As Integer, _
                       ByVal aFindValue As String, _
              Optional ByVal aStartRecord As Integer = 1, _
              Optional ByVal aEndRecord As Integer = -1) As Boolean
    Function FindNext(ByVal aFieldNumber As Integer, _
                      ByVal aFindValue As String) As Boolean
    Function Summary(Optional ByVal aFormat As String = "tab,headers,expandtype") As String
    Function SummaryFields(Optional ByVal aFormat As String = "tab,headers,expandtype") As String
    Function SummaryFile(Optional ByVal aFormat As String = "tab,headers") As String

    Property CurrentRecord() As Integer
    Function EOF() As Boolean
    Property FieldLength(ByVal aFieldNumber As Integer) As Integer
    Property FieldName(ByVal aFieldNumber As Integer) As String
    Property FieldType(ByVal aFieldNumber As Integer) As String
    Property Header(ByVal aHeaderRow As Integer) As String
    Property Header() As String
    Property NumHeaderRows() As Integer
    Property NumFields() As Integer
    Property NumRecords() As Integer
    Property Value(ByVal aFieldNumber As Integer) As String
    Property FileName() As String

    Sub Clear()
    Sub ClearData()
    Sub MoveFirst()
    Sub MoveLast()
    Sub MoveNext()
    Sub MovePrevious()
End Interface

''' <summary>
''' opens a file containing a table based on the file extension
''' </summary>
''' <remarks></remarks>
Public Class atcTableOpener
    ''' <summary>
    ''' opens a table for known table types based on file extension
    ''' </summary>
    ''' <param name="aFileName">name of file containing table</param>
    ''' <returns>opened table if file exists and of known type</returns>
    ''' <remarks></remarks>
    Public Shared Function OpenAnyTable(ByVal aFileName As String) As IatcTable
        Dim lOpenAnyTable As IatcTable = Nothing
        Select Case System.IO.Path.GetExtension(aFileName).ToLower
            Case ".dbf" : lOpenAnyTable = New atcTableDBF
            Case ".csv" : lOpenAnyTable = New atcTableDelimited
                'Case "abt": Set lOpenFile = New clsATCTableBin
        End Select
        If lOpenAnyTable IsNot Nothing Then
            lOpenAnyTable.OpenFile(aFileName)
        End If
        Return lOpenAnyTable
    End Function
End Class

''' <summary>
''' generic table class
''' </summary>
''' <remarks></remarks>
Public MustInherit Class atcTable
    Implements IDisposable
    Implements IatcTable

    Protected pFilename As String
    Protected pHeaderLines As New Generic.List(Of String)
    Protected pNumHeaderRows As Integer = -1
    Protected pCurrentRecord As Integer
    Protected pEOF As Boolean = False

    'Used within FindMatch
    Private Enum ComparisonEnum
        EQ = 0
        LT
        GT
        LE
        GE
    End Enum

    ''' <summary>
    ''' copies a table's structure without data
    ''' </summary>
    ''' <returns>new table with the same fields as this one, but no data</returns>
    ''' <remarks></remarks>
    Public MustOverride Function Cousin() As IatcTable Implements IatcTable.Cousin

    ''' <summary>
    ''' produces Visual Basic code needed to create this table
    ''' </summary>
    ''' <returns>VB source code to create this table</returns>
    ''' <remarks></remarks>
    Public MustOverride Function CreationCode() As String Implements IatcTable.CreationCode

    ''' <summary>
    ''' Open the specified file, probably read at least the metadata about fields
    ''' </summary>
    ''' <param name="aFilename">name of file containing table</param>
    ''' <returns>true if table opened successfully</returns>
    ''' <remarks></remarks>
    Public MustOverride Function OpenFile(ByVal aFilename As String) As Boolean Implements IatcTable.OpenFile

    ''' <summary>
    ''' Write the current table to the specified file
    ''' </summary>
    ''' <param name="aFilename">name of file to write table to</param>
    ''' <returns>true if table written successfully</returns>
    ''' <remarks></remarks>
    Public MustOverride Function WriteFile(ByVal aFilename As String) As Boolean Implements IatcTable.WriteFile

    ''' <summary>
    ''' Get a specified row of the header
    ''' </summary>
    ''' <param name="aHeaderRow">Which row to get (range is 1..NumHeaderRows)</param>
    ''' <returns>text of specified row of the header</returns>
    Public Overridable Property Header(ByVal aHeaderRow As Integer) As String Implements IatcTable.Header
        Get
            If aHeaderRow < 1 OrElse aHeaderRow > pHeaderLines.Count Then
                Return "Header row requested (" & aHeaderRow & ") outside available range (1 to " & pHeaderLines.Count & ")"
            Else
                Return pHeaderLines(aHeaderRow - 1)
            End If
        End Get
        Set(ByVal newValue As String)
            If aHeaderRow < 1 OrElse aHeaderRow > pHeaderLines.Count Then
                Throw New ApplicationException("Cannot set header row " & aHeaderRow & ".  Row must be between 1 and " & pHeaderLines.Count & ".")
            Else
                pHeaderLines(aHeaderRow - 1) = newValue
            End If
        End Set
    End Property

    ''' <summary>
    ''' All rows of the header concatenated with cr/lf at the end of each line
    ''' </summary>
    Public Overridable Property Header() As String Implements IatcTable.Header
        Get
            Dim lReturnValue As String = ""
            For Each lString As String In pHeaderLines
                lReturnValue &= lString & vbCrLf
            Next
            Return lReturnValue
        End Get
        Set(ByVal newValue As String)
            pHeaderLines.Clear()
            pHeaderLines.AddRange(newValue.Replace(vbCrLf, vbCr).Replace(vbLf, vbCr).Split(vbCr))
        End Set
    End Property

    ''' <summary>
    ''' The number of header rows in the table
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Property NumHeaderRows() As Integer Implements IatcTable.NumHeaderRows
        Get
            If pNumHeaderRows >= 0 Then
                Return pNumHeaderRows
            Else
                Return pHeaderLines.Count
            End If
        End Get
        Set(ByVal newValue As Integer)
            pNumHeaderRows = newValue
            While pHeaderLines.Count < newValue
                pHeaderLines.Add("") 'dummy header
            End While
            While pHeaderLines.Count > newValue AndAlso pHeaderLines.Count > 0
                pHeaderLines.RemoveAt(pHeaderLines.Count - 1)
            End While
        End Set
    End Property

    'The number of fields (columns) in the table
    Public MustOverride Property NumFields() As Integer Implements IatcTable.NumFields

    'The number of records (rows) in the table
    Public MustOverride Property NumRecords() As Integer Implements IatcTable.NumRecords

    'The current record index [1..NumRecords]
    Public MustOverride Property CurrentRecord() As Integer Implements IatcTable.CurrentRecord

    'True if CurrentRecord was attempted to be set beyond the end of file
    Public Function EOF() As Boolean Implements IatcTable.EOF
        Return pEOF
    End Function

    'The value of the specified field in the current record
    'aFieldNumber [1..NumFields]
    Public MustOverride Property Value(ByVal aFieldNumber As Integer) As String Implements IatcTable.Value

    'Returns the name of the specified field, aFieldNumber should be in [1..numFields]
    Public MustOverride Property FieldName(ByVal aFieldNumber As Integer) As String Implements IatcTable.FieldName

    'Returns the width of the specified field
    Public MustOverride Property FieldLength(ByVal aFieldNumber As Integer) As Integer Implements IatcTable.FieldLength

    'Returns the type of the specified field
    'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
    Public MustOverride Property FieldType(ByVal aFieldNumber As Integer) As String Implements IatcTable.FieldType

    ''' <summary>
    ''' Trim leading and trailing spaces from value.
    ''' If numeric, also trim trailing zeroes after decimal point and trailing decimal point
    ''' </summary>
    ''' <param name="aValue">Value to be trimmed</param>
    ''' <param name="aType">Type character, N = Numeric</param>
    Public Overridable Function TrimValue(ByVal aValue As String, ByVal aType As String) As String
        If aValue Is Nothing Then Return ""
        'First trim surrounding null and space characters
        aValue = aValue.Trim(ControlChars.NullChar, " "c)
        If aType = "N" Then
            Dim lDecimalPos As Integer = aValue.IndexOf("."c)
            Select Case lDecimalPos
                Case -1 'No decimal, no need to trim trailing zeroes
                Case 0 'Decimal is first character, trim trailing zeroes, change lone decimal point to 0 if needed
                    aValue = aValue.TrimEnd("0"c)
                    If aValue.Length = 1 Then aValue = "0"
                Case Else 'Decimal is after first character, trim trailing zeroes, trim decimal if it is then last
                    aValue = aValue.TrimEnd("0"c).TrimEnd("."c)
            End Select
        End If
        Return aValue
    End Function

    'The name of the file used to populate the table
    'File is read by OpenFile and written by WriteFile
    Public Overridable Property FileName() As String Implements IatcTable.FileName
        Get
            Return pFilename
        End Get
        Set(ByVal Value As String)
            pFilename = Value
        End Set
    End Property

    'Forget the current contents of the table
    Public MustOverride Sub ClearData() Implements IatcTable.ClearData

    'Forget the current contents of the table and the fields
    Public MustOverride Sub Clear() Implements IatcTable.Clear, IDisposable.Dispose

    'Moves CurrentRecord to the beginning of the table
    Public Overridable Sub MoveFirst() Implements IatcTable.MoveFirst
        Me.CurrentRecord = 1
    End Sub

    'Moves CurrentRecord to the end of the table
    Public Overridable Sub MoveLast() Implements IatcTable.MoveLast
        Me.CurrentRecord = Me.NumRecords
    End Sub

    'Moves CurrentRecord to the next record
    Public Overridable Sub MoveNext() Implements IatcTable.MoveNext
        Me.CurrentRecord = Me.CurrentRecord + 1
    End Sub

    'Moves CurrentRecord to the previous record
    Public Overridable Sub MovePrevious() Implements IatcTable.MovePrevious
        Me.CurrentRecord = Me.CurrentRecord - 1
    End Sub

    'Returns a text description of the table
    Public Overridable Function Summary(Optional ByVal aFormat As String = "tab,headers,expandtype") As String Implements IatcTable.Summary
        Return SummaryFile(aFormat) & vbCrLf & SummaryFields(aFormat)
    End Function

    'Find width of widest existing value in each field
    Public Overridable Function ComputeFieldLengths() As Integer()
        Dim lMaxWidths(Me.NumFields) As Integer
        Dim lField As Integer
        Dim lWidth As Integer
        Dim lSaveCurrent As Integer = Me.CurrentRecord
        For lRecord As Integer = 1 To Me.NumRecords
            Me.CurrentRecord = lRecord
            For lField = 1 To NumFields
                lWidth = Me.Value(lField).Length
                If lWidth > lMaxWidths(lField) Then lMaxWidths(lField) = lWidth
            Next
        Next
        Me.CurrentRecord = lSaveCurrent
        Return lMaxWidths
    End Function

    'A summary of the file
    Public Overridable Function SummaryFile(Optional ByVal aFormat As String = "tab,headers") As String Implements IatcTable.SummaryFile
        Dim retval As String

        If LCase(aFormat) = "text" Then 'text version
            retval = "    FileName: " & FileName & vbCrLf
            retval = retval & "    NumFields:  " & NumFields
            retval = retval & "    NumRecords: " & NumRecords
        Else 'table version
            retval = "FileName " & vbTab & "NumFields " & vbTab & "NumRecords " & vbCrLf _
                   & pFilename & vbTab & NumFields & vbTab & NumRecords & vbCrLf
        End If
        Return retval
    End Function

    'A summary of the fields (names, types, lengths)
    Public Overridable Function SummaryFields(Optional ByVal aFormat As String = "tab,headers,expandtype") As String Implements IatcTable.SummaryFields
        Dim retval As String = ""
        Dim iField As Integer
        Dim ShowTrash As Boolean
        Dim ShowHeaders As Boolean
        Dim ExpandType As Boolean

        If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
        If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
        If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True

        If InStr(LCase(aFormat), "text") > 0 Then 'text version
            For iField = 1 To NumFields
                retval = retval & vbCrLf & "Field " & iField & ": '" & FieldName(iField) & "'"
                retval = retval & vbCrLf & "    Type: "
                If ExpandType Then
                    Select Case FieldType(iField)
                        Case "C" : retval = retval & "Character"
                        Case "D" : retval = retval & "Date     "
                        Case "N" : retval = retval & "Numeric  "
                        Case "L" : retval = retval & "Logical  "
                        Case "M" : retval = retval & "Memo     "
                    End Select
                End If
                retval = retval & vbCrLf & "    Length: " & FieldLength(iField) & " "
                retval = retval & vbCrLf
            Next
        Else 'table version
            If ShowHeaders Then
                retval = retval & "Field "
                retval = retval & vbTab & "Name "
                retval = retval & vbTab & "Type "
                retval = retval & vbTab & "Length "
            End If
            retval = retval & vbCrLf
            'now field details
            For iField = 1 To NumFields
                retval = retval & iField & vbTab & "'" & FieldName(iField) & "' " & vbTab
                If ExpandType Then
                    Select Case FieldType(iField)
                        Case "C" : retval = retval & "Character"
                        Case "D" : retval = retval & "Date     "
                        Case "N" : retval = retval & "Numeric  "
                        Case "L" : retval = retval & "Logical  "
                        Case "M" : retval = retval & "Memo     "
                    End Select
                Else
                    retval = retval & FieldType(iField)
                End If
                retval = retval & vbTab & FieldLength(iField)
                retval = retval & vbCrLf
            Next
        End If
        Return retval
    End Function

    ''' <summary>
    ''' determine field number from field name
    ''' </summary>
    ''' <param name="aFieldName">name of specified field</param>
    ''' <returns>the number of the field with the specified name or zero if the named field does not appear in this file</returns>
    ''' <remarks></remarks>
    Public MustOverride Function FieldNumber(ByVal aFieldName As String) As Integer Implements IatcTable.FieldNumber
 
    'Returns a string version of the current record
    Public Overridable Function CurrentRecordAsDelimitedString(Optional ByVal aDelimiter As String = ",", Optional ByVal aQuote As String = "") As String
        CurrentRecordAsDelimitedString = ""
        If NumFields > 0 Then
            For iField As Integer = 1 To NumFields - 1
                CurrentRecordAsDelimitedString &= aQuote & Value(iField) & aQuote & aDelimiter
            Next
            CurrentRecordAsDelimitedString &= aQuote & Value(NumFields) & aQuote
        End If
    End Function

    'Returns True if found, moves CurrentRecord to first record with .Value(aFieldNumber) = aFindValue
    'If not found, returns False and moves CurrentRecord to aStartRecord
    'If aStartRecord is specified, searching starts there instead of at first record
    'If aEndRecord is specified, search stops at aEndRecord
    Public Overridable Function FindFirst(ByVal aFieldNumber As Integer, ByVal aFindValue As String, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean Implements IatcTable.FindFirst
        If NumRecords < aStartRecord Then 'don't look past end
            Return False
        Else
            If aEndRecord < 1 Then aEndRecord = NumRecords
            CurrentRecord = aStartRecord
            aFindValue = TrimValue(aFindValue, FieldType(aFieldNumber))
            While CurrentRecord <= aEndRecord
                If Value(aFieldNumber) = aFindValue Then
                    Return True
                End If
                If CurrentRecord < aEndRecord Then
                    CurrentRecord += 1
                Else 'force exit since attempting to set CurrentRecord beyond NumRecords sets it back to 1
                    Exit While
                End If
            End While
            CurrentRecord = aStartRecord
            Return False
        End If
    End Function

    'Returns True if found, moves CurrentRecord to next record with .Value(FieldNumber) = FindValue
    'If not found, returns False and moves CurrentRecord to 1
    Public Overridable Function FindNext(ByVal aFieldNumber As Integer, ByVal aFindValue As String) As Boolean Implements IatcTable.FindNext
        Return FindFirst(aFieldNumber, aFindValue, Me.CurrentRecord + 1)
    End Function

    ''' <summary>
    ''' Find a record matching a set of rules
    ''' </summary>
    ''' <param name="aFieldNum">array of fields to compare</param>
    ''' <param name="aOperation">comparisons to make</param>
    ''' <param name="aFieldVal">values to compare fields to</param>
    ''' <param name="aMatchAny">true if any one comparison is enough to match, 
    ''' default of false means all comparisons must be true to match a record</param>
    ''' <param name="aStartRecord">record number to start searching</param>
    ''' <param name="aEndRecord">record number to stop searching (default of -1 searches to end of table)</param>
    ''' <returns>true if a matching record was found, with CurrentRecord set to the record that was found.
    ''' false if no matching record was found, with CurrentRecord set to aStartRecord</returns>
    Public Overridable Function FindMatch(ByVal aFieldNum() As Integer, ByVal aOperation() As String, ByVal aFieldVal() As Object, Optional ByVal aMatchAny As Boolean = False, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
        Dim lLastRule As Integer = aFieldNum.GetUpperBound(0)
        Dim curRule As Integer
        Dim lValue As Object
        Dim allMatch As Boolean
        Dim thisMatches As Boolean
        Dim NotAtTheEnd As Boolean
        Dim lOperation(lLastRule) As ComparisonEnum

        If aEndRecord < 1 Then aEndRecord = Me.NumRecords

        'If we are supposed to look for matches only in records that don't exist, we won't find any
        If aStartRecord > Me.NumRecords Then
            FindMatch = False
            Exit Function
        End If

        'Compile strings into integers for faster operation below
        For curRule = 0 To lLastRule
            Select Case aOperation(curRule)
                Case "=" : lOperation(curRule) = ComparisonEnum.EQ
                Case "<" : lOperation(curRule) = ComparisonEnum.LT
                Case ">" : lOperation(curRule) = ComparisonEnum.GT
                Case "<=" : lOperation(curRule) = ComparisonEnum.LE
                Case ">=" : lOperation(curRule) = ComparisonEnum.GE
                Case Else
                    Throw New ApplicationException("Unrecognized operation:" & aOperation(curRule))
            End Select
        Next

        CurrentRecord = aStartRecord
        NotAtTheEnd = True
        While NotAtTheEnd And CurrentRecord <= aEndRecord
            curRule = 0
            allMatch = True
            While curRule <= lLastRule And allMatch
                thisMatches = False
                lValue = Value(aFieldNum(curRule))
                Select Case lOperation(curRule)
                    Case ComparisonEnum.EQ
                        If lValue = aFieldVal(curRule) Then thisMatches = True
                    Case ComparisonEnum.LT
                        If lValue < aFieldVal(curRule) Then thisMatches = True
                    Case ComparisonEnum.GT
                        If lValue > aFieldVal(curRule) Then thisMatches = True
                    Case ComparisonEnum.LE
                        If lValue <= aFieldVal(curRule) Then thisMatches = True
                    Case ComparisonEnum.GE
                        If lValue >= aFieldVal(curRule) Then thisMatches = True
                End Select
                If aMatchAny Then
                    If thisMatches Then
                        FindMatch = True
                        Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    End If
                Else
                    If Not thisMatches Then
                        allMatch = False
                    End If
                End If
                curRule += 1
            End While
            If allMatch And Not aMatchAny Then
                FindMatch = True
                Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            End If
            If CurrentRecord < Me.NumRecords Then
                MoveNext()
            Else
                NotAtTheEnd = False
            End If
        End While
        CurrentRecord = aStartRecord
        FindMatch = False
    End Function

    ''' <summary>
    ''' Create a new list of objects, one for each record of the table, populated from columns of table
    ''' </summary>
    ''' <param name="aObjectType"></param>
    ''' <param name="aFieldMap">Mapping of table field names as keys to object field/property names as items</param>
    ''' <param name="aNewArgs"></param>
    ''' <param name="aNewBindingFlags"></param>
    ''' <returns></returns>
    ''' <remarks>If aFieldMap is Nothing, default mapping of exactly the same field names is attempted</remarks>
    Public Function PopulateObjects(ByVal aObjectType As Type, _
                           Optional ByVal aFieldMap As atcUtility.atcCollection = Nothing, _
                           Optional ByVal aNewArgs() As Object = Nothing, _
                           Optional ByVal aNewBindingFlags As Reflection.BindingFlags = Reflection.BindingFlags.CreateInstance _
                                                                                      + Reflection.BindingFlags.Public _
                                                                                      + Reflection.BindingFlags.Instance) As ArrayList
        Dim lNewList As New ArrayList
        For lRecord As Integer = 1 To NumRecords
            CurrentRecord = lRecord
            Dim lObject As Object = aObjectType.InvokeMember("New", aNewBindingFlags, Nothing, Nothing, aNewArgs)
            PopulateObject(lObject, aFieldMap)
            lNewList.Add(lObject)
        Next
        Return lNewList
    End Function

    ''' <summary>
    ''' Populate an object from the current record of the table
    ''' </summary>
    ''' <param name="aObject">Object to populate</param>
    ''' <param name="aFieldMap">Mapping of table field names as keys to object field/property names as items</param>
    ''' <remarks>If aFieldMap is Nothing, default mapping of exactly the same field names is attempted</remarks>
    Public Sub PopulateObject(ByRef aObject As Object, Optional ByVal aFieldMap As atcUtility.atcCollection = Nothing)
        Dim lFieldNumber As Integer
        For lFieldNumber = 1 To Me.NumFields
            Dim lFieldName As String = Me.FieldName(lFieldNumber)
            If aFieldMap IsNot Nothing AndAlso aFieldMap.Keys.Contains(lFieldName) Then
                SetSomething(aObject, aFieldMap.ItemByKey(lFieldName), Value(lFieldNumber), False)
            Else
                SetSomething(aObject, lFieldName, Value(lFieldNumber), False)
            End If
        Next
    End Sub

End Class
