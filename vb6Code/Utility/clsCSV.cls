VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsCSV"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

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

Public Function Summary(Optional Format As String = "Text") As String
  Dim retval As String
  Dim iField As Integer
  Dim lText As Boolean
  
  If LCase(Format) = "text" Then
    lText = True
  Else
    lText = False
  End If
  
  If lText Then 'text version
    retval = "CSV Header: "
    retval = retval & vbCrLf & "    NumRecs: " & pNumRecs
    For iField = 1 To pNumFields
      retval = retval & vbCrLf
      With pFields(iField)
        retval = retval & vbCrLf & "Field " & iField & ": '" & TrimNull(.fname) & "'"
        retval = retval & vbCrLf & "    Type: " & .FType & " "
        Select Case .FType
          Case "C": retval = retval & "(Character)"
          Case "D": retval = retval & "(Date)"
          Case "N": retval = retval & "(Numeric)"
          Case "L": retval = retval & "(Logical)"
          Case "M": retval = retval & "(Memo)"
        End Select
        retval = retval & vbCrLf & "    Length: " & .Length & " "
        retval = retval & vbCrLf & "    DecimalCount: " & .DecimalCount & " "
      End With
    Next
  Else 'table version
    'build header header
    retval = "#Dir " & vbTab & "FileName "
    retval = retval & vbTab & "Date "
    retval = retval & vbTab & "NumRecs "
    retval = retval & vbCrLf
    retval = retval & "#" & CurDir & vbTab & pFilename
    retval = retval & vbTab & pNumRecs
    retval = retval & vbCrLf
    'now field headers
    retval = retval & "Field "
    retval = retval & vbTab & "Name "
    retval = retval & vbTab & "Type "
    retval = retval & vbTab & "Length "
    retval = retval & vbTab & "DecimalCount "
    retval = retval & vbCrLf
    'now field details
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & iField & vbTab & "'" & TrimNull(.fname) & "' "
        Select Case .FType
          Case "C": retval = retval & vbTab & "(Character)"
          Case "D": retval = retval & vbTab & "(Date)     "
          Case "N": retval = retval & vbTab & "(Numeric)  "
          Case "L": retval = retval & vbTab & "(Logical)  "
          Case "M": retval = retval & vbTab & "(Memo)     "
        End Select
        retval = retval & vbTab & .Length
        retval = retval & vbTab & .DecimalCount
      End With
      retval = retval & vbCrLf
    Next
  End If
  Summary = retval
End Function

Public Property Get numFields() As Long
  numFields = pNumFields
End Property
Public Property Let numFields(ByVal newValue As Long)
  pNumFields = newValue
  ReDim pFields(1 To pNumFields)
End Property

Public Property Get FieldLength(ByVal FieldNumber As Long) As Byte
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldLength = pFields(FieldNumber).Length
  Else
    FieldLength = 0
  End If
End Property
Public Property Let FieldLength(ByVal FieldNumber As Long, ByVal newValue As Byte)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).Length = newValue
  End If
End Property

Public Property Get fieldName(ByVal FieldNumber As Long) As String
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    fieldName = TrimNull(pFields(FieldNumber).fname)
  Else
    fieldName = "Undefined"
  End If
End Property
'FieldName is a maximum of 10 characters long, padded to 11 characters with nulls
Public Property Let fieldName(ByVal FieldNumber As Long, ByVal newValue As String)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    newValue = Trim(Left(newValue, 10))
    pFields(FieldNumber).fname = newValue & String(11 - Len(newValue), Chr$(0))
  End If
End Property

'Returns zero if the named field does not appear in this file
Public Function FieldNumber(ByVal fieldName As String) As Long
  Dim retval As Long
  For retval = 1 To numFields
    If TrimNull(pFields(retval).fname) = fieldName Then
      FieldNumber = retval
      Exit Function
    End If
  Next
End Function

'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
Public Property Get FieldType(ByVal FieldNumber As Long) As String
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldType = pFields(FieldNumber).FType
  Else
    FieldType = "Undefined"
  End If
End Property
Public Property Let FieldType(ByVal FieldNumber As Long, ByVal newValue As String)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).FType = newValue
  End If
End Property

Public Property Get FieldDecimalCount(ByVal FieldNumber As Long) As Byte
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldDecimalCount = pFields(FieldNumber).DecimalCount
  Else
    FieldDecimalCount = 0
  End If
End Property
Public Property Let FieldDecimalCount(ByVal FieldNumber As Long, ByVal newValue As Byte)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).DecimalCount = newValue
  End If
End Property

Public Property Get FileName() As String
  FileName = pFilename
End Property

'Returns True if found, moves CurrentRecord to first record with .Value(FieldNumber) = FindValue
Public Function FindFirst(ByVal FieldNumber As Long, FindValue As String) As Boolean
  For pCurrentRecord = 1 To NumRecords
    pCurrentRecordStart = pNumFields * (pCurrentRecord - 1)
    If Value(FieldNumber) = FindValue Then
      FindFirst = True
      Exit Function
    End If
  Next
  pCurrentRecord = 1
  FindFirst = False
End Function

'Returns True if found, moves CurrentRecord to first record with .Record = FindValue
Public Function FindRecord(ByRef FindValue As String) As Boolean
  Dim Byt As Long
  Dim Match As Boolean
  Dim rec As Long
  
  For rec = 1 To NumRecords
    CurrentRecord = rec
    Match = True
    
    'need new guts here
    
    If Match Then
      FindRecord = True
      Exit Function
    End If
  Next
  pCurrentRecord = 1
  FindRecord = False
End Function

'Returns True if found, moves CurrentRecord to next record with .Value(FieldNumber) = FindValue
Public Function FindNext(ByVal FieldNumber As Long, FindValue As String) As Boolean
  For pCurrentRecord = pCurrentRecord + 1 To NumRecords
    If Value(FieldNumber) = FindValue Then
      FindNext = True
      Exit Function
    End If
  Next
  pCurrentRecord = 1
  FindNext = False
End Function

Public Property Get NumRecords() As Long
  NumRecords = pNumRecs
End Property
Public Property Let NumRecords(ByVal newValue As Long)
  Dim s As Long
  pNumRecs = newValue
  s = pNumRecs * pNumFields
  If s > UBound(pData) Then
    ReDim Preserve pData(s)
  End If
End Property

Public Property Get CurrentRecord() As Long
  CurrentRecord = pCurrentRecord
End Property
Public Property Let CurrentRecord(ByVal newValue As Long)
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
  MsgBox "Cannot set CurrentRecord to " & newValue & vbCr & Err.Description, vbOKOnly, "clsCSV Let CurrentRecord"
End Property

Public Property Get record() As String
  Dim iField As Long
  Dim Val As String
  
  For iField = 1 To pNumFields
    Val = Value(iField)
    If InStr(Val, pDelimiter) > 0 Then Val = pQuote & Val & pQuote
    record = record & Val
    If iField < pNumFields Then record = record & pDelimiter
  Next
End Property
Public Property Let record(newValue As String)
  Dim i As Long, data As String
  
  data = newValue
  For i = 1 To pNumFields
    pData(pCurrentRecordStart + i) = StrSplit(data, pDelimiter, pQuote)
  Next
End Property

Public Property Get Value(ByVal fieldNum As Long) As String
  If pCurrentRecord < 1 Or pCurrentRecord > pNumRecs Then
    Value = "Invalid Current Record Number"
  ElseIf fieldNum < 1 Or fieldNum > numFields Then
    Value = "Invalid Field Number"
  Else
    Dim FieldPos As Long
    
    FieldPos = pCurrentRecordStart + fieldNum
    Value = pData(FieldPos)
    'If InStr(Value, pDelimiter) > 0 Then Value = pQuote & Value & pQuote
  End If
End Property
Public Property Let Value(ByVal fieldNum As Long, ByVal newValue As String)
  Dim FieldPos As Long
  
  On Error GoTo ErrHand
  If pCurrentRecord < 1 Then
    Debug.Print "clsCSV Let Value: Invalid Current Record Number: " & pCurrentRecord & " (value = '" & newValue & "'"
  ElseIf fieldNum < 1 Or fieldNum > numFields Then
    Debug.Print "clsCSV Let Value: Invalid Field Number: " & fieldNum & " (value = '" & newValue & "'"
  Else
    FieldPos = pCurrentRecordStart + fieldNum
    pData(FieldPos) = newValue
  End If
  Exit Property
ErrHand:
  MsgBox "Cannot set field #" * fieldNum & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & Err.Description, vbOKOnly, "clsCSV Let Value"
End Property

Public Sub OpenCSV(ByVal FileName As String)
  'Dim inFile As Long
  Dim iField As Long
  Dim dot As Long
  Dim str As String
  Dim header As String
  Dim data As String
  Dim fieldName As String
  Dim quoteStart As Long
  Dim delimPos As Long
  Dim recordStart As Long
  Dim recordEnd As Long
  Dim FieldStart As Long
  Dim trimmed As Boolean
  Dim curLine As String
  Dim WholeFileLen As Long

  pNumRecs = 0
  pNumFields = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  ReDim pData(0)
  ReDim pFields(0)
  
  If Len(FileName) > 255 Then
    Debug.Print Now & " OpenCSV(" & Left(FileName, 255) & "...)"
  Else
    Debug.Print Now & " OpenCSV(" & FileName & ")"
  End If
  
  If FileExists(FileName) Then
    pFilename = FileName
    str = WholeFileString(FileName)
  Else 'Should we test more to make sure it doesn't look like a file name?
    str = FileName
  End If
  
  header = StrSplit(str, Chr(10), "'")
  pFileHeader = header
  If pDelimiter = vbTab Then 'Skip comments at the top of tab-delimited RDB files
    While Left(header, 1) = "#"
      header = StrSplit(str, vbLf, "'")
      pFileHeader = pFileHeader & vbLf & header
    Wend
  End If
  While Len(header) > 0
    iField = UBound(pFields) + 1
    ReDim Preserve pFields(iField)
    fieldName = StrSplit(header, pDelimiter, pQuote)
    If Asc(Right(fieldName, 1)) < 32 Then fieldName = Left(fieldName, Len(fieldName) - 1)
    dot = InStrRev(fieldName, ".")
    fieldName = Mid(fieldName, dot + 1)
    pFields(iField).fname = fieldName
  Wend
  pNumFields = iField
  
  ReDim pData(256)
  WholeFileLen = Len(str)
  recordStart = 1
  pCurrentRecord = 0
  While recordStart < WholeFileLen 'Len(str) > 0
    recordEnd = InStr(recordStart, str, vbLf)
    If recordEnd < 1 Then recordEnd = WholeFileLen
    data = Mid(str, recordStart, recordEnd - recordStart) 'StrSplit(str, vbLf, pQuote)
    trimmed = False
    While Not trimmed 'Trim CR if CR/LF
      If Len(data) = 0 Then
        trimmed = True
      Else
        Select Case Asc(Right(data, 1))
          Case 10, 12, 13: data = Left(data, Len(data) - 1)
          Case Else: trimmed = True
        End Select
      End If
    Wend
    If Len(Trim(data)) > 0 Then
      CurrentRecord = CurrentRecord + 1
      record = data
    End If
    recordStart = recordEnd + 1
  Wend
  ReDim Preserve pData(pCurrentRecord * pNumFields)
  pNumRecsCapacity = pNumRecs
  
  'Close #inFile
  CurrentRecord = 1
End Sub

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

Public Sub WriteCSV(ByVal FileName As String)
  Dim OutFile As Integer
  Dim i As Integer, j As Integer, dot As Integer
  Dim s As String
TryAgain:
  On Error GoTo ErrHand

  OutFile = FreeFile(0)
  MkDirPath PathNameOnly(FileName)
  Open FileName For Output As OutFile
  
  s = ""
  For i = 1 To pNumFields
    If InStr(pFields(i).fname, pDelimiter) > 0 Then
      s = s & pQuote & TrimNull(pFields(i).fname) & pQuote
    Else
      s = s & TrimNull(pFields(i).fname)
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

  pFilename = FileName
Exit Sub

ErrHand:
  Resume Next
  If MsgBox("Error saving " & FileName & vbCr & Err.Description, vbAbortRetryIgnore, "Write CSV") = vbRetry Then
    On Error Resume Next
    Close #OutFile
    GoTo TryAgain
  End If
End Sub

Private Function TrimNull(str As String) As String
  Dim nullPos As Long
  nullPos = InStr(str, Chr(0))
  If nullPos = 0 Then
    TrimNull = Trim(str)
  Else
    TrimNull = Trim(Left(str, nullPos - 1))
  End If
End Function

Private Sub Class_Initialize()
  Clear
End Sub

Public Sub Clear()
  pNumRecs = 0
  pNumFields = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  pDelimiter = ","
  pQuote = """"
  ReDim pData(0)
  ReDim pFields(1)
End Sub
