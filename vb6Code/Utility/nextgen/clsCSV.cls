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
'Private pDataBytes As Long
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

Public Property Get FieldName(ByVal FieldNumber As Long) As String
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldName = TrimNull(pFields(FieldNumber).fname)
  Else
    FieldName = "Undefined"
  End If
End Property
'FieldName is a maximum of 10 characters long, padded to 11 characters with nulls
Public Property Let FieldName(ByVal FieldNumber As Long, ByVal newValue As String)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    newValue = Trim(Left(newValue, 10))
    pFields(FieldNumber).fname = newValue & String(11 - Len(newValue), Chr$(0))
  End If
End Property

'Returns zero if the named field does not appear in this file
Public Function FieldNumber(ByVal FieldName As String) As Long
  Dim retval As Long
  For retval = 1 To numFields
    If TrimNull(pFields(retval).fname) = FieldName Then
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

Public Property Get Filename() As String
  Filename = pFilename
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
  Dim byt As Long
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
  Dim val As String
  
  For iField = 1 To pNumFields
    val = Value(iField)
    If InStr(val, pDelimiter) > 0 Then val = pQuote & val & pQuote
    record = record & val
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

Public Sub OpenCSV(ByVal Filename As String)
  Dim inFile As Long
  Dim i As Long, j As Long, dot As Long, l As Long
  Dim iField As Long
  
  Const bufMax As Long = 65535
  Dim buf(0 To bufMax) As Byte
  Dim lastBufByte As Long
  Dim bufpos As Long
  Dim FileLength As Long
  Dim TotalBytesRead As Long
  Dim parseState As Long '0 = normal, 1 = in comment, 2 = in quote
  Dim bDelimiter As Byte
  Dim bQuote As Byte
  Dim bComment As Byte
  
  Dim header As String
  Dim data As String
  Dim name As String
  Dim quoteStart As Long
  Dim delimPos As Long
  Dim recordStart As Long
  Dim FieldStart As Long
  Dim trimmed As Boolean
  Dim lendata As Long
  Dim curLine As String
  
  pNumRecs = 0
  pNumFields = 0
'  pDataBytes = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  ReDim pData(0)
  ReDim pFields(1 To 1)
  
  If Not FileExists(Filename) Then Exit Sub
  
  bDelimiter = Asc(pDelimiter)
  bQuote = Asc(pQuote)
  bComment = Asc("#")
  
  pFilename = Filename

  inFile = FreeFile(0)
  Open Filename For Binary Access Read Shared As inFile
  FileLength = LOF(inFile)
  
  GoSub NextBufPos
  
  'Read first line of file into field names
  If bDelimiter = 9 Then
    If buf(bufpos) = bComment Then parseState = 1
  End If
  
NextHeaderCharacter:
  Select Case parseState
    Case 0:
      Select Case buf(bufpos)
        Case bDelimiter
          iField = iField + 1
          ReDim Preserve pFields(iField)
        Case bQuote:  parseState = 2
        Case 10, 13: GoTo FinishedHeader
        Case Else
          pFields(iField).fname = pFields(iField).fname + Chr(buf(bufpos))
      End Select
    Case 1:
      While buf(bufpos) = 10 Or buf(bufpos) = 13
        GoSub NextBufPos
        Select Case buf(bufpos)
          Case bComment: GoSub NextBufPos: GoTo NextHeaderCharacter
          Case 10, 13: 'ignore additional CR/LF once we find one
          Case Else 'Reached first character of first non-commented line of file
            parseState = 0
            GoTo NextHeaderCharacter
        End Select
      Wend
    Case 2:
      Select Case buf(bufpos)
        Case bQuote: parseState = 0
        Case 10, 13: GoTo FinishedHeader
        Case Else
          pFields(iField).fname = pFields(iField).fname + Chr(buf(bufpos))
      End Select
  End Select
  GoSub NextBufPos
  GoTo NextHeaderCharacter
    
FinishedHeader:
  pNumFields = iField

'  Open Filename For Input As inFile
'  Line Input #inFile, header 'this assumes file written with chr(10) only (no chr(13))
'  Close #InFile
  
'  str = WholeFileString(Filename)
'  header = StrSplit(str, Chr(10), "'")
'  pFileHeader = header
'  If pDelimiter = vbTab Then 'Skip comments at the top of tab-delimited RDB files
'    While Left(header, 1) = "#"
'      Line Input #inFile, header
'      pFileHeader = pFileHeader & vbLf & header
'    Wend
'  End If
'  While Len(header) > 0
'    i = UBound(pFields) + 1
'    ReDim Preserve pFields(i)
'    name = StrSplit(header, pDelimiter, pQuote)
'    If Asc(Right(name, 1)) < 32 Then name = Left(name, Len(name) - 1)
'    dot = InStrRev(name, ".")
'    name = Mid(name, dot + 1)
'    pFields(i).fname = name
'  Wend
'  pNumFields = i
  
'  ReDim pData(256)
'  pDataBytes = LOF(inFile)
'  recordStart = 1
'  While Not EOF(inFile)
'    Line Input #inFile, data
'    trimmed = False
'    While Not trimmed 'Trim CR if CR/LF
'      If Len(data) = 0 Then
'        trimmed = True
'      Else
'        Select Case Asc(Right(data, 1))
'          Case 10, 12, 13: data = Left(data, Len(data) - 1)
'          Case Else: trimmed = True
'        End Select
'      End If
'    Wend
'    If Len(Trim(data)) > 0 Then
'      i = pNumRecs * pNumFields
'      While Len(data) > 0
'        pData(i) = StrSplit(data, pDelimiter, pQuote)
'        i = i + 1
'        If i >= UBound(pData) Then
'        ReDim Preserve pData(i * 2)
'        End If
'      Wend
'      pNumRecs = pNumRecs + 1
'    End If
'    'Debug.Print pNumRecs & ":" & recordStart / pDataBytes
'  Wend
'  ReDim Preserve pData(i)
  pNumRecsCapacity = pNumRecs
  
  'Figure out types, sizes, decimal counts of fields
  For i = 1 To pNumFields
    With pFields(i + 1)
      .FType = "N"
      .Length = 4 'minimum size that we will allow
      .DecimalCount = 0
      For j = 0 To pNumRecs - 1
        data = pData((j * pNumFields) + i)
        lendata = Len(data)
        If lendata > 255 Then lendata = 255
        If lendata > .Length Then
          .Length = lendata
        End If
        If lendata > 0 Then
          If Not IsNumeric(data) Then
            'TODO what about dates?
            .FType = "C"
            Exit For
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
      Next
    End With
  Next i

Finished:
  Close #inFile
  CurrentRecord = 1
  
  Exit Sub
  
NextBufPos:
  bufpos = bufpos + 1
  If bufpos > lastBufByte Then
    If TotalBytesRead < FileLength Then
      'Fill buffer with another chunk of file
      Get #inFile, , buf
      If TotalBytesRead + bufMax + 1 > FileLength Then
        lastBufByte = FileLength - TotalBytesRead - 1
      Else
        lastBufByte = bufMax
      End If
      TotalBytesRead = TotalBytesRead + lastBufByte + 1
      bufpos = 0
    Else
      GoTo Finished
    End If
  End If
  Return
  
End Sub

Public Sub WriteCSV(ByVal Filename As String)
  Dim OutFile As Integer
  Dim i As Integer, j As Integer, dot As Integer
  Dim s As String
TryAgain:
  On Error GoTo ErrHand

  OutFile = FreeFile(0)
  MkDirPath PathNameOnly(Filename)
  Open Filename For Output As OutFile
  
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
    For i = 1 To pNumFields - 1
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
Exit Sub

ErrHand:
  Resume Next
  If MsgBox("Error saving " & Filename & vbCr & Err.Description, vbAbortRetryIgnore, "Write CSV") = vbRetry Then
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
'  pDataBytes = 0
  pCurrentRecord = 1
  pNumRecsCapacity = 0
  pDelimiter = ","
  pQuote = """"
  ReDim pData(0)
  ReDim pFields(0)
End Sub
