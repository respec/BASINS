VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCTable"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

'Open the specified file, probably read at least the metadata about fields
Public Function OpenFile(ByVal Filename As String) As clsATCTable
  Dim CreateClassName As String
  On Error GoTo CouldNotCreate
  CreateClassName = "clsATCTable" & UCase(FileExt(Filename))
  Select Case UCase(FileExt(Filename))
    Case "DBF": Set OpenFile = New clsATCTableDBF
    Case Else:  Set OpenFile = CreateObject(CreateClassName)
  End Select
  OpenFile.OpenFile Filename
  
  Exit Function

CouldNotCreate:
  If Err.Number = 429 Then 'ActiveX component can't create object
    Err.Raise Err.Number, "clsATCTable:OpenFile", "Could not open table: " & Filename & vbCrLf & "Could not create " & CreateClassName
  Else
    On Error GoTo 0
    Err.Raise Err.Number, "clsATCTable:OpenFile", "Could not open table: " & Filename & vbCrLf & Err.Description
  End If
End Function

'Write the current table to the specified file
Public Sub WriteFile(ByVal Filename As String)
End Sub

'Forget the current contents of the table
Public Sub ClearData()
End Sub

'Forget the current contents of the table and the fields
Public Sub Clear()
End Sub

'The number of records (rows) in the table
Public Property Get NumRecords() As Long
End Property
Public Property Let NumRecords(ByVal newValue As Long)
End Property

'The number of fields (columns) in the table
Public Property Get NumFields() As Long
End Property
Public Property Let NumFields(ByVal aNewValue As Long)
End Property

'The current record index [1..NumRecords]
Public Property Get CurrentRecord() As Long
End Property
Public Property Let CurrentRecord(ByVal newValue As Long)
End Property

'The value of the specified field in the current record
'aFieldNumber [1..NumFields]
Public Property Get Value(ByVal aFieldNumber As Long) As String
End Property
Public Property Let Value(ByVal aFieldNumber As Long, ByVal newValue As String)
End Property

'The whole record as an array of bytes
Public Property Get record() As Byte()
End Property
Public Property Let record(newValue() As Byte)
End Property

'Returns True if CurrentRecord is at beginning of table
Public Function BOF() As Boolean
'  If pCurrentRecord <= 1 Then BOF = True
End Function

'Returns True if CurrentRecord is at end of table
Public Function EOF() As Boolean
'  If pCurrentRecord >= NumRecords Then EOF = True
End Function

'Moves CurrentRecord to the beginning of the table
Public Sub MoveFirst()
'  CurrentRecord = 1
End Sub

'Moves CurrentRecord to the end of the table
Public Sub MoveLast()
'  CurrentRecord = NumRecords
End Sub

'Moves CurrentRecord to the next record
Public Sub MoveNext()
'  CurrentRecord = CurrentRecord + 1
End Sub

'Moves CurrentRecord to the next record
Public Sub MovePrevious()
'  CurrentRecord = CurrentRecord - 1
End Sub

'Returns a text description of the table
Public Function Summary(Optional aFormat As String = "tab,headers,expandtype") As String
'  Summary = SummaryFile(aFormat) & vbCrLf & SummaryFields(aFormat)
End Function

Public Function SummaryFile(Optional aFormat As String = "tab,headers") As String
End Function

Public Function SummaryFields(Optional aFormat As String = "tab,headers,expandtype") As String
End Function

'Returns VB source code to create this table
Public Function CreationCode() As String
End Function

'Returns a new table with the same fields as this one, but no data
Public Function Cousin() As clsATCTable
End Function

'Make this table have the same fields as aTable, data is not copied
Public Sub CousinOf(aTable As clsATCTable)
End Sub

'Returns the name of the specified field, aFieldNumber should be in [1..numFields]
Public Property Get FieldName(ByVal aFieldNumber As Long) As String
End Property
Public Property Let FieldName(ByVal aFieldNumber As Long, ByVal aNewValue As String)
End Property

'Returns the number of the field with the specified name
'Returns zero if the named field does not appear in this file
Public Function FieldNumber(ByVal aFieldName As String) As Long
End Function

'Returns the width of the specified field
Public Property Get FieldLength(ByVal aFieldNumber As Long) As Long
End Property
Public Property Let FieldLength(ByVal aFieldNumber As Long, ByVal newValue As Long)
End Property

'Returns the type of the specified field
'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
Public Property Get FieldType(ByVal aFieldNumber As Long) As String
End Property
Public Property Let FieldType(ByVal aFieldNumber As Long, ByVal aNewValue As String)
End Property

'Returns the name of the file used to populate the table
Public Property Get Filename() As String
End Property

'Returns a string version of the current record
Private Function CurrentRecordAsDelimitedString(Optional aDelimiter As String = ",", _
                                                Optional aQuote As String = "") As String
End Function

'Returns True if found, moves CurrentRecord to first record with .Value(aFieldNumber) = aFindValue
'If not found, returns False and moves CurrentRecord to aStartRecord
'If aStartRecord is specified, searching starts there instead of at first record
'If aEndRecord is specified, search stops at aEndRecord
Public Function FindFirst(ByVal aFieldNumber As Long, aFindValue As String, _
                     Optional ByVal aStartRecord As Long = 1, _
                     Optional ByVal aEndRecord As Long = -1) As Boolean
End Function

'Returns True if found, moves CurrentRecord to next record with .Value(FieldNumber) = FindValue
'If not found, returns False and moves CurrentRecord to 1
Public Function FindNext(ByVal aFieldNumber As Long, aFindValue As String) As Boolean
'  FindNext = FindFirst(aFieldNumber, aFindValue, pCurrentRecord + 1)
End Function

'FindLast     (like FindFirst but searching from end to start)
'FindPrevious (like FindNext but searching from current to 1)

''Merge records from table2Add into this table
''keyFieldNames are field names that define a unique field.
''If keyFieldNames is blank, no duplicate checking will occur
''If keyFieldNames(1) = "**ALL**" then the entire record will be used as a key
''DuplicateAction dictates handling of duplicate records as follows:
'' 0 - duplicates allowed
'' 1 - keep existing instance of duplicates and discard duplicates from dbf being added
'' 2 - replace existing instance of duplicates with duplicates from dbf being added
'' 3 - ask user what to do (not currently implemented)
'Public Sub Merge(table2Add As clsATCTable, keyFieldNames() As String, DuplicateAction As Long)
'End Sub
'
'
'Public Property Get record() As Byte()
'End Property
'Public Property Let record(newValue() As Byte)
'End Property
'
''Returns True if found, moves CurrentRecord to first record with .Record = FindValue
''If not found, returns False and moves CurrentRecord to 1
'Public Function FindRecord(ByRef FindValue() As Byte, _
'                          Optional ByVal aStartRecord As Long = 1, _
'                          Optional ByVal aEndRecord As Long = -1) As Boolean
'End Function
'
''Returns True if CurrentRecord matches FindValue
'Public Function MatchRecord(ByRef FindValue() As Byte) As Boolean
'End Function
'
'Public Property Set Logger(ByVal newValue As Object)
'End Property


