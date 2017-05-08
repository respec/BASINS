VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCTableCSV"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Implements clsATCTable

Private Type FieldDescriptor
   fname As String
   FType As String * 1 ' C = Character, D = Date, N = Numeric, L = Logical, M = Memo
   Length As Byte
   DecimalCount As Byte
End Type

Private pFilename As String
Private pNumRecs As Long
Private pFields() As FieldDescriptor
Private pData() As String
Private pNumFields As Long
Private pCurrentRecord As Long
Private pCurrentRecordStart As Long
Private pDelimiter As String
Private pQuote As String
Private pFileHeader As String

'Capacity in pData for records. Set to pNumRecs when data is read from a file
'and in InitData when creating a new CSV from scratch. May increase in Let NumRecords.
Private pNumRecsCapacity As Long

Private Function clsATCTable_BOF() As Boolean
  If pCurrentRecord <= 1 Then clsATCTable_BOF = True
End Function

'Default delimiter is a comma. Changing this after reading data is not advised
Public Property Get Delimiter() As String
  Delimiter = pDelimiter
End Property
Public Property Let Delimiter(newValue As String)
  pDelimiter = newValue
End Property

'Quote character that can enclose a delimiter in a field value
'Quotes are removed as data is read
'Set to blank if quotes are not allowed to contain a delimiter and should be preserved in values
'Default quote is "
Public Property Get quote() As String
  quote = pQuote
End Property
Public Property Let quote(newValue As String)
  pQuote = newValue
End Property

'Any comments at the top of the file, plus the line labeling the fields
Public Property Get FileHeader() As String
  FileHeader = pFileHeader
End Property
Public Property Let FileHeader(newValue As String)
  pFileHeader = newValue
End Property

Public Property Get FieldDecimalCount(ByVal aFieldNumber As Long) As Byte
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    FieldDecimalCount = pFields(aFieldNumber).DecimalCount
  Else
    FieldDecimalCount = 0
  End If
End Property
Public Property Let FieldDecimalCount(ByVal aFieldNumber As Long, ByVal newValue As Byte)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    pFields(aFieldNumber).DecimalCount = newValue
  End If
End Property

'Returns True if found, moves CurrentRecord to first record with .Record = FindValue
'Public Function FindRecord(ByRef FindValue As String) As Boolean
'  Dim Byt As Long
'  Dim Match As Boolean
'  Dim rec As Long
'
'  For rec = 1 To NumRecords
'    CurrentRecord = rec
'    Match = True
'
'    'need new guts here
'
'    If Match Then
'      FindRecord = True
'      Exit Function
'    End If
'  Next
'  pCurrentRecord = 1
'  FindRecord = False
'End Function

Private Function clsATCTable_Cousin() As clsATCTable
  Dim iField As Integer
  
  Set clsATCTable_Cousin = New clsATCTableCSV
  With clsATCTable_Cousin
    .NumFields = pNumFields
    
    For iField = 1 To pNumFields
      .FieldName(iField) = clsATCTable_FieldName(iField)
      .FieldType(iField) = clsATCTable_FieldType(iField)
      .FieldLength(iField) = clsATCTable_FieldLength(iField)
'FIXME      .FieldDecimalCount(iField) = clsATCTable_FieldDecimalCount(iField)
    Next
  End With
End Function

Private Sub clsATCTable_CousinOf(aTable As clsATCTable)
  Dim iField As Integer
  
  clsATCTable_NumFields = aTable.NumFields
    
  For iField = 1 To aTable.NumFields
    clsATCTable_FieldName(iField) = aTable.FieldName(iField)
    clsATCTable_FieldType(iField) = aTable.FieldType(iField)
    clsATCTable_FieldLength(iField) = aTable.FieldLength(iField)
  Next
  clsATCTable_NumRecords = 0
End Sub

Private Function clsATCTable_CreationCode() As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  
  retval = "Dim newTable as clsATCTable"
  retval = retval & vbCrLf & "set newTable = new clsATCTableCSV"
  retval = retval & vbCrLf & "With newTable"
  
  retval = retval & vbCrLf & "  .NumFields = " & pNumFields
  retval = retval & vbCrLf
    
  For iField = 1 To pNumFields
    With pFields(iField)
      retval = retval & vbCrLf & "  .FieldName(" & iField & ") = """ & .fname & """"
      retval = retval & vbCrLf & "  .FieldType(" & iField & ") = """ & .FType & """"
      retval = retval & vbCrLf & "  .FieldLength(" & iField & ") = " & .FType
      retval = retval & vbCrLf & "  .FieldDecimalCount(" & iField & ") = " & .DecimalCount
      retval = retval & vbCrLf
    End With
  Next
  retval = retval & vbCrLf & "  '.NumRecords = " & pNumRecs
  retval = retval & vbCrLf & "  '.InitData"
  retval = retval & vbCrLf & "End With"
  retval = retval & vbCrLf
  clsATCTable_CreationCode = retval
End Function

Private Property Get clsATCTable_CurrentRecord() As Long
  clsATCTable_CurrentRecord = pCurrentRecord
End Property
Private Property Let clsATCTable_CurrentRecord(ByVal newValue As Long)
  On Error GoTo ErrHand
  If newValue > pNumRecs Then 'expand capacity of pData
    pNumRecs = newValue
    If newValue > pNumRecsCapacity Then
      pNumRecsCapacity = (pNumRecsCapacity + 1) * 1.5
      ReDim Preserve pData(pNumRecsCapacity * pNumFields)
    End If
  End If
  If newValue < 1 Or newValue > pNumRecs Then
    pCurrentRecord = 1
  Else
    pCurrentRecord = newValue
  End If
  pCurrentRecordStart = (pCurrentRecord - 1) * pNumFields
  Exit Property
ErrHand:
  MsgBox "Cannot set CurrentRecord to " & newValue & vbCr & Err.Description, vbOKOnly, "clsATCTableCSV Let CurrentRecord"
End Property

Private Function clsATCTable_EOF() As Boolean
  If pCurrentRecord >= pNumRecs Then clsATCTable_EOF = True
End Function

Private Property Get clsATCTable_FieldLength(ByVal aFieldNumber As Long) As Long
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    clsATCTable_FieldLength = pFields(aFieldNumber).Length
  Else
    clsATCTable_FieldLength = 0
  End If
End Property
Public Property Let clsATCTable_FieldLength(ByVal aFieldNumber As Long, ByVal newValue As Long)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    pFields(aFieldNumber).Length = newValue
  End If
End Property

Private Property Get clsATCTable_FieldName(ByVal aFieldNumber As Long) As String
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    clsATCTable_FieldName = pFields(aFieldNumber).fname
  Else
    clsATCTable_FieldName = "Undefined"
  End If
End Property
Private Property Let clsATCTable_FieldName(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    pFields(aFieldNumber).fname = Trim(newValue)
  End If
End Property

Private Function clsATCTable_FieldNumber(ByVal aFieldName As String) As Long
'Returns zero if the named field does not appear in this file
  Dim retval As Long
  For retval = 1 To pNumFields
    If pFields(retval).fname = aFieldName Then
      clsATCTable_FieldNumber = retval
      Exit Function
    End If
  Next
End Function

Private Property Get clsATCTable_FieldType(ByVal aFieldNumber As Long) As String
'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    clsATCTable_FieldType = pFields(aFieldNumber).FType
  Else
    clsATCTable_FieldType = "Undefined"
  End If
End Property
Private Property Let clsATCTable_FieldType(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    pFields(aFieldNumber).FType = newValue
  End If
End Property

Private Property Get clsATCTable_filename() As String
  clsATCTable_filename = pFilename
End Property

Private Function clsATCTable_FindFirst(ByVal aFieldNumber As Long, aFindValue As String, Optional ByVal aStartRecord As Long = 1&, Optional ByVal aEndRecord As Long = -1&) As Boolean
'Returns True if found, moves CurrentRecord to first record with .Value(FieldNumber) = FindValue
  If aEndRecord < 0 Then aEndRecord = pNumRecs
  For pCurrentRecord = aStartRecord To aEndRecord
    pCurrentRecordStart = pNumFields * (pCurrentRecord - 1)
    If clsATCTable_Value(aFieldNumber) = aFindValue Then
      clsATCTable_FindFirst = True
      Exit Function
    End If
  Next
  pCurrentRecord = 1
  clsATCTable_FindFirst = False
End Function

Private Function clsATCTable_FindNext(ByVal aFieldNumber As Long, aFindValue As String) As Boolean
'Returns True if found, moves CurrentRecord to next record with .Value(FieldNumber) = FindValue
  For pCurrentRecord = pCurrentRecord + 1 To pNumRecs
    pCurrentRecordStart = pNumFields * (pCurrentRecord - 1)
    If clsATCTable_Value(aFieldNumber) = aFindValue Then
      clsATCTable_FindNext = True
      Exit Function
    End If
  Next
  pCurrentRecord = 1
  clsATCTable_FindNext = False
End Function

Private Sub clsATCTable_MoveFirst()
  clsATCTable_CurrentRecord = 1
End Sub

Private Sub clsATCTable_MoveLast()
  clsATCTable_CurrentRecord = pNumRecs
End Sub

Private Sub clsATCTable_MoveNext()
  clsATCTable_CurrentRecord = pCurrentRecord + 1
End Sub

Private Sub clsATCTable_MovePrevious()
  clsATCTable_CurrentRecord = pCurrentRecord - 1
End Sub

Private Property Get clsATCTable_NumFields() As Long
  clsATCTable_NumFields = pNumFields
End Property
Private Property Let clsATCTable_NumFields(ByVal newValue As Long)
  pNumFields = newValue
  ReDim pFields(1 To pNumFields)
End Property

Private Property Get clsATCTable_NumRecords() As Long
  clsATCTable_NumRecords = pNumRecs
End Property
Private Property Let clsATCTable_NumRecords(ByVal newValue As Long)
  Dim s As Long
  pNumRecs = newValue
  s = pNumRecs * pNumFields
  If s > UBound(pData) Then
    ReDim Preserve pData(s)
  End If
End Property

Private Function clsATCTable_OpenFile(ByVal Filename As String) As clsATCTable
  'Dim inFile As Long
  Dim iField As Long
  Dim dot As Long
  Dim str As String
  Dim strlen As Long
  Dim header As String
  Dim data As String
  Dim FieldName As String
  Dim quoteStart As Long
  Dim delimPos As Long
  Dim recordStart As Long
  Dim recordEnd As Long
  Dim fieldStart As Long
  Dim fieldEnd As Long
  Dim trimmed As Boolean
  Dim curLine As String

  pNumRecs = 0
  pNumFields = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  ReDim pData(0)
  ReDim pFields(0)
  
  Debug.Print Now & " OpenCSV(" & Filename & ")"
  
  If FileExists(Filename) Then
    pFilename = Filename
    str = WholeFileString(Filename)
  Else 'Should we test more to make sure it doesn't look like a file name?
    str = Filename
  End If
  
  GoSub NextRecord
  
  header = Left(str, recordEnd - recordStart)
  pFileHeader = header
  If pDelimiter = vbTab Then 'Skip comments at the top of tab-delimited RDB files
    While Left(header, 1) = "#"
      GoSub NextRecord
      header = Mid(str, recordStart, recordEnd - recordStart)
      pFileHeader = pFileHeader & vbLf & header
    Wend
  End If
  While Len(header) > 0
    iField = UBound(pFields) + 1
    ReDim Preserve pFields(iField)
    FieldName = StrSplit(header, pDelimiter, pQuote)
    If Asc(Right(FieldName, 1)) < 32 Then FieldName = Left(FieldName, Len(FieldName) - 1)
    If Len(FieldName) > 9 Then
      dot = InStrRev(FieldName, ".")
      If dot > 0 Then FieldName = Mid(FieldName, dot + 1)
    End If
    pFields(iField).fname = FieldName
  Wend
  pNumFields = iField
  
  ReDim pData(256)
  pCurrentRecord = 0
  strlen = Len(str)
  While recordStart < strlen
    If recordEnd - recordStart > 0 Then
      clsATCTable_CurrentRecord = pCurrentRecord + 1
      clsATCTable_record = Mid(str, recordStart, recordEnd - recordStart)
    End If
  Wend
  ReDim Preserve pData(pCurrentRecord * pNumFields)
  pNumRecsCapacity = pNumRecs
  
  'Close #inFile
  clsATCTable_CurrentRecord = 1
  Set clsATCTable_OpenFile = Me

  Exit Function

NextRecord:
  recordStart = recordEnd + 1
  If Mid(str, recordStart, 1) = vbLf Then recordStart = recordStart + 1
  recordEnd = InStr(recordStart, str, vbLf)
  If recordEnd = 0 Then recordEnd = Len(str)
  If Mid(str, recordEnd - 1, 1) = vbCr Then recordEnd = recordEnd - 1
End Function

Public Sub CalculateFieldProperties()
  Dim data As String
  Dim dot As Long
  Dim lendata As Long
  Dim l As Long
  Dim iField As Long
  Dim iRecord As Long
  For iField = 1 To pNumFields
    With pFields(iField)
      Debug.Print Now & " Finding type and width of field " & iField & ": " & .fname
      .FType = "N"
      .Length = 0 'minimum size that we will allow
      .DecimalCount = 0
      For iRecord = 0 To pNumRecs - 1
        data = pData((iRecord * pNumFields) + iField)
        lendata = Len(data)
        If lendata > 255 Then
          .Length = 255
          .FType = "C"
          Exit For
        End If
        If lendata > .Length Then
          .Length = lendata
        End If
        If lendata > 0 Then
          If .FType = "N" Then
            If Not IsNumeric(data) Then
              'TODO what about dates?
              .FType = "C"
              If .Length = 255 Then Exit For
            Else
              dot = InStr(data, ".")
              If dot > 0 Then
                l = lendata - dot
                If l > .DecimalCount Then
                  .DecimalCount = l
                  'TODO are we sure that the field is wide enough?
                End If
              End If
            End If
          End If
        End If
      Next
    End With
  Next iField
End Sub

'ErrHand:
'  Resume Next
'  If MsgBox("Error saving " & Filename & vbCr & Err.Description, vbAbortRetryIgnore, "Write CSV") = vbRetry Then
'    On Error Resume Next
'    Close #OutFile
'    GoTo TryAgain
'  End If
'End Sub

Private Sub Class_Initialize()
  clsATCTable_Clear
End Sub

Private Sub clsATCTable_Clear()
  clsATCTable_ClearData
  pNumFields = 0
  pDelimiter = ","
  pQuote = """"
  ReDim pFields(1)
End Sub

Private Sub clsATCTable_ClearData()
  pNumRecs = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  ReDim pData(0)
End Sub

Private Property Get clsATCTable_record() As Byte()
  Dim iField As Long
  Dim val As String
  Dim record As String
  
  For iField = 1 To pNumFields
    val = clsATCTable_Value(iField)
    If InStr(val, pDelimiter) > 0 Then val = pQuote & val & pQuote
    record = record & val
    If iField < pNumFields Then record = record & pDelimiter
  Next
  clsATCTable_record = record
End Property
Private Property Let clsATCTable_record(newValue() As Byte)
  Dim i As Long, data As String
  
  data = newValue
  For i = 1 To pNumFields
    pData(pCurrentRecordStart + i) = StrSplit(data, pDelimiter, pQuote)
  Next
End Property

Private Function clsATCTable_Summary(Optional aFormat As String = "tab,headers,expandtype") As String
  clsATCTable_Summary = clsATCTable_SummaryFile(aFormat) & vbCrLf & clsATCTable_SummaryFields(aFormat)
End Function

Private Function clsATCTable_SummaryFields(Optional aFormat As String = "tab,headers,expandtype") As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  Dim ShowHeaders As Boolean
  Dim ExpandType As Boolean
  
  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
  If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True
  
  If InStr(LCase(aFormat), "text") > 0 Then 'text version
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & vbCrLf & "Field " & iField & ": '" & .fname & "'"
        retval = retval & vbCrLf & "    Type: "
        If ExpandType Then
          Select Case .FType
            Case "C": retval = retval & "Character"
            Case "D": retval = retval & "Date     "
            Case "N": retval = retval & "Numeric  "
            Case "L": retval = retval & "Logical  "
            Case "M": retval = retval & "Memo     "
          End Select
        Else
          retval = retval & .FType
        End If
        retval = retval & vbCrLf & "    Length: " & .Length & " "
        retval = retval & vbCrLf & "    DecimalCount: " & .DecimalCount & " "
      End With
      retval = retval & vbCrLf
    Next
  Else 'table version
    If ShowHeaders Then
      retval = retval & "Field "
      retval = retval & vbTab & "Name "
      retval = retval & vbTab & "Type "
      retval = retval & vbTab & "Length "
      retval = retval & vbTab & "DecimalCount "
    End If
    retval = retval & vbCrLf
    'now field details
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & iField & vbTab & "'" & .fname & "' "
        If ExpandType Then
          Select Case .FType
            Case "C": retval = retval & vbTab & "Character"
            Case "D": retval = retval & vbTab & "Date     "
            Case "N": retval = retval & vbTab & "Numeric  "
            Case "L": retval = retval & vbTab & "Logical  "
            Case "M": retval = retval & vbTab & "Memo     "
          End Select
        Else
          retval = retval & vbTab & .FType
        End If
        retval = retval & vbTab & .Length
        retval = retval & vbTab & .DecimalCount
      End With
      retval = retval & vbCrLf
    Next
  End If
  clsATCTable_SummaryFields = retval
End Function

Private Function clsATCTable_SummaryFile(Optional aFormat As String = "tab,headers") As String
  Dim retval As String
  Dim iTrash As Integer
  Dim ShowHeaders As Boolean
  
  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
  
  If LCase(aFormat) = "text" Then 'text version
      retval = "CSV Header: "
      retval = retval & vbCrLf & "    FileName: " & pFilename
      retval = retval & vbCrLf & "   NumFields: " & pNumFields
      retval = retval & vbCrLf & "     NumRecs: " & pNumRecs
  Else 'table version
    'build header header
    If ShowHeaders Then
      retval = "FileName "
      retval = retval & vbTab & "NumFields "
      retval = retval & vbTab & "NumRecs "
    End If
    retval = retval & vbCrLf
    retval = retval & pFilename
    retval = retval & vbTab & pNumFields
    retval = retval & vbTab & pNumRecs
    retval = retval & vbCrLf
    
  End If
  clsATCTable_SummaryFile = retval
End Function

Private Property Get clsATCTable_Value(ByVal aFieldNumber As Long) As String
  If pCurrentRecord < 1 Or pCurrentRecord > pNumRecs Then
    clsATCTable_Value = "Invalid Current Record Number"
  ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
    clsATCTable_Value = "Invalid Field Number"
  Else
    Dim FieldPos As Long
    FieldPos = pCurrentRecordStart + aFieldNumber
    clsATCTable_Value = pData(FieldPos)
    'If InStr(Value, pDelimiter) > 0 Then Value = pQuote & Value & pQuote
  End If
End Property
Private Property Let clsATCTable_Value(ByVal aFieldNumber As Long, ByVal newValue As String)
  Dim FieldPos As Long
  
  On Error GoTo ErrHand
  If pCurrentRecord < 1 Then
    Debug.Print "clsATCTableCSV Let Value: Invalid Current Record Number: " & pCurrentRecord & " (value = '" & newValue & "'"
  ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
    Debug.Print "clsATCTableCSV Let Value: Invalid Field Number: " & aFieldNumber & " (value = '" & newValue & "'"
  Else
    FieldPos = pCurrentRecordStart + aFieldNumber
    pData(FieldPos) = newValue
  End If
  Exit Property
ErrHand:
  MsgBox "Cannot set field #" * aFieldNumber & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & Err.Description, vbOKOnly, "clsATCTableCSV Let Value"
End Property

Private Sub clsATCTable_WriteFile(ByVal Filename As String)
  Dim OutFile As Integer
  Dim i As Integer, j As Integer, dot As Integer
  Dim s As String
TryAgain:
  'On Error GoTo ErrHand

  OutFile = FreeFile(0)
  MkDirPath PathNameOnly(Filename)
  Open Filename For Output As OutFile
  
  s = ""
  For i = 1 To pNumFields
    If InStr(pFields(i).fname, pDelimiter) > 0 Then
      s = s & pQuote & pFields(i).fname & pQuote
    Else
      s = s & pFields(i).fname
    End If
    If i < pNumFields Then s = s & pDelimiter
  Next
  Print #OutFile, s
  
  'If we have over-allocated for adding more records, trim unused records
  If pNumRecsCapacity > pNumRecs Then
    pNumRecsCapacity = pNumRecs
    ReDim Preserve pData(pNumRecs * pNumFields)
  End If
  
  For j = 0 To pNumRecs - 1
    s = ""
    For i = 0 To pNumFields - 1
      If InStr(pData(j * pNumFields + i), pDelimiter) > 0 Then
        s = s & pQuote & pData(j * pNumFields + i) & pQuote
      Else
        s = s & pData(j * pNumFields + i)
      End If
      If i < pNumFields - 1 Then s = s & pDelimiter
    Next i
    Print #OutFile, s
  Next j
  
  Close #OutFile

  pFilename = Filename
End Sub

'Private Function clsATCTable_SummaryFields(Optional aFormat As String = "tab,headers,expandtype") As String
'  Dim retval As String
'  Dim iField As Integer
'  Dim lText As Boolean
'
'  If LCase(Format) = "text" Then
'    lText = True
'  Else
'    lText = False
'  End If
'
'  If lText Then 'text version
'    retval = "CSV Header: "
'    retval = retval & vbCrLf & "    NumRecs: " & pNumRecs
'    For iField = 1 To pNumFields
'      retval = retval & vbCrLf
'      With pFields(iField)
'        retval = retval & vbCrLf & "Field " & iField & ": '" & .fname & "'"
'        retval = retval & vbCrLf & "    Type: " & .FType & " "
'        If InStr(LCase(aFormat), "expandtype") > 0 Then
'          Select Case .FType
'            Case "C": retval = retval & "(Character)"
'            Case "D": retval = retval & "(Date)"
'            Case "N": retval = retval & "(Numeric)"
'            Case "L": retval = retval & "(Logical)"
'            Case "M": retval = retval & "(Memo)"
'          End Select
'        End If
'        retval = retval & vbCrLf & "    Length: " & .Length & " "
'        retval = retval & vbCrLf & "    DecimalCount: " & .DecimalCount & " "
'      End With
'    Next
'  Else 'table version
'    'build header header
'    retval = "#Dir " & vbTab & "FileName "
'    retval = retval & vbTab & "Date "
'    retval = retval & vbTab & "NumRecs "
'    retval = retval & vbCrLf
'    retval = retval & "#" & CurDir & vbTab & pFilename
'    retval = retval & vbTab & pNumRecs
'    retval = retval & vbCrLf
'    'now field headers
'    retval = retval & "Field "
'    retval = retval & vbTab & "Name "
'    retval = retval & vbTab & "Type "
'    retval = retval & vbTab & "Length "
'    retval = retval & vbTab & "DecimalCount "
'    retval = retval & vbCrLf
'    'now field details
'    For iField = 1 To pNumFields
'      With pFields(iField)
'        retval = retval & iField & vbTab & "'" & .fname & "' "
'        Select Case .FType
'          Case "C": retval = retval & vbTab & "(Character)"
'          Case "D": retval = retval & vbTab & "(Date)     "
'          Case "N": retval = retval & vbTab & "(Numeric)  "
'          Case "L": retval = retval & vbTab & "(Logical)  "
'          Case "M": retval = retval & vbTab & "(Memo)     "
'        End Select
'        retval = retval & vbTab & .Length
'        retval = retval & vbTab & .DecimalCount
'      End With
'      retval = retval & vbCrLf
'    Next
'  End If
'  clsATCTable_SummaryFields = retval
'End Function
