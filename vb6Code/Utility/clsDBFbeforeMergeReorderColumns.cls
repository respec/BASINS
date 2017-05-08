VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsDBF"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private Const CtrlZ As Byte = 26

Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (Destination As Any, Source As Any, ByVal Length As Long)
Private Declare Function ArrPtr Lib "msvbvm60.dll" Alias "VarPtr" (ptr() As Any) As Long

'Variables for comparing bytes of a record as longs
Private pLongHeader1(5) As Long
Private pLongHeader2(5) As Long
Private pCompareLongs1() As Long
Private pCompareLongs2() As Long

'Private CountAgreeNoMatch As Long
'Private CountAgreeMatch As Long

'===========================================================================
' Subject: READ DBASE III                    Date: 1/25/88 (00:00)
' Author:  David Perry                       Code: QB, PDS
' Keys:    READ,DBASE,III                  Packet: MISC.ABC
'===========================================================================

'This QB source was adjusted for use with VB by Robert Smith
'on June 14, 1999, source was provided to Smith by Marc Hoogerwerf
'contact Smith via: www.smithvoice.com/vbfun.htm

'This code was turned into a class by Mark.Gray@pobox.com March 14, 2001
'modification and extensions continue through 2003

'dBaseIII file header, 32 bytes
Private Type dBHeader
   version As Byte
   Year As Byte
   Month As Byte
   Day As Byte
   NumRecs As Long
   NumBytesHeader As Integer
   NumBytesRec As Integer
   Trash(1 To 20) As Byte
End Type

'Field Descriptions, 32 bytes * Number of Fields
'Up to 128 Fields
Private Type FieldDescriptor
   fname As String * 11
   FType As String * 1 'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
   DataAddress As Long 'offset from record start to field start
   Length As Byte      'Byte type limits field size in DBF to 255 bytes
   DecimalCount As Byte
   Trash(1 To 14) As Byte
End Type

Private pFilename As String
Private pHeader As dBHeader
Private pFields() As FieldDescriptor
Private pNumFields As Long
Private pData() As Byte
Private pDataBytes As Long
Private pCurrentRecord As Long
Private pCurrentRecordStart As Long
Private pLogger As Object

'Capacity in pData for records. Set to pHeader.NumRecs when data is read from a file
'and in InitData when creating a new DBF from scratch. May increase in Let Value.
Private pNumRecsCapacity As Long

Public Function Summary(Optional Format As String = "tab,headers,expandtype") As String
  Summary = SummaryFile(Format) & vbCrLf & SummaryFields(Format)
End Function

Public Function SummaryFile(Optional Format As String = "tab,headers") As String
  Dim retval As String
  Dim iTrash As Integer
  Dim ShowTrash As Boolean
  Dim ShowHeaders As Boolean
  
  If InStr(LCase(Format), "trash") > 0 Then ShowTrash = True
  If InStr(LCase(Format), "headers") > 0 Then ShowHeaders = True
  
  If LCase(Format) = "text" Then 'text version
    With pHeader
      retval = "DBF Header: "
      retval = retval & vbCrLf & "    FileName: " & pFilename
      retval = retval & vbCrLf & "    Version: " & .version
      retval = retval & vbCrLf & "    Date: " & .Year + 1900 & "/" & .Month & "/" & .Day
      retval = retval & vbCrLf & "    NumRecs: " & .NumRecs
      retval = retval & vbCrLf & "    NumBytesHeader: " & .NumBytesHeader
      retval = retval & vbCrLf & "    NumBytesRec: " & .NumBytesRec
      If ShowTrash Then
        retval = retval & vbCrLf & "    Trash: "
        For iTrash = 1 To 20
          retval = retval & .Trash(iTrash) & " "
        Next
      End If
    End With
  Else 'table version
    'build header header
    If ShowHeaders Then
      retval = "FileName "
      retval = retval & vbTab & "Version "
      retval = retval & vbTab & "Date "
      retval = retval & vbTab & "NumFields "
      retval = retval & vbTab & "NumRecs "
      retval = retval & vbTab & "NumBytesHeader "
      retval = retval & vbTab & "NumBytesRec "
    End If
    If ShowTrash Then
      For iTrash = 1 To 20
        retval = retval & vbTab & "Trash" & iTrash
      Next
    End If
    retval = retval & vbCrLf
    With pHeader 'now header data
      retval = retval & pFilename
      retval = retval & vbTab & .version
      retval = retval & vbTab & .Year + 1900 & "/" & .Month & "/" & .Day
      retval = retval & vbTab & pNumFields
      retval = retval & vbTab & .NumRecs
      retval = retval & vbTab & .NumBytesHeader
      retval = retval & vbTab & .NumBytesRec
      If ShowTrash Then
        For iTrash = 1 To 20
          retval = retval & vbTab & .Trash(iTrash)
        Next
      End If
      retval = retval & vbCrLf
    End With
  End If
  SummaryFile = retval
End Function

Public Function SummaryFields(Optional Format As String = "tab,headers,expandtype") As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  Dim ShowTrash As Boolean
  Dim ShowHeaders As Boolean
  Dim ExpandType As Boolean
  
  If InStr(LCase(Format), "trash") > 0 Then ShowTrash = True
  If InStr(LCase(Format), "headers") > 0 Then ShowHeaders = True
  If InStr(LCase(Format), "expandtype") > 0 Then ExpandType = True
  
  If InStr(LCase(Format), "text") > 0 Then 'text version
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & vbCrLf & "Field " & iField & ": '" & TrimNull(.fname) & "'"
        retval = retval & vbCrLf & "    Type: " & .FType & " "
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
        If ShowTrash Then
          retval = retval & vbCrLf & "    Trash: "
          For iTrash = 1 To 14
            retval = retval & .Trash(iTrash) & " "
          Next
        End If
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
      If ShowTrash Then
        For iTrash = 1 To 14
          retval = retval & vbTab & "Trash" & iTrash
        Next
      End If
    End If
    retval = retval & vbCrLf
    'now field details
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & iField & vbTab & "'" & TrimNull(.fname) & "' "
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
        If ShowTrash Then
          retval = retval & vbCrLf & "    Trash: "
          For iTrash = 1 To 14
            retval = retval & vbTab & .Trash(iTrash)
          Next
        End If
      End With
      retval = retval & vbCrLf
    Next
  End If
  SummaryFields = retval
End Function

Public Function CreationCode() As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  
  retval = "Dim newDBF as clsDBF"
  retval = retval & vbCrLf & "set newDBF = new clsDBF"
  retval = retval & vbCrLf & "With newDBF"
  
  retval = retval & vbCrLf & "  .Year = CInt(Format(Now, ""yyyy"")) - 1900"
  retval = retval & vbCrLf & "  .Month = CByte(Format(Now, ""mm""))"
  retval = retval & vbCrLf & "  .Day = CByte(Format(Now, ""dd""))"
  retval = retval & vbCrLf & "  .NumFields = " & NumFields
  retval = retval & vbCrLf
    
  For iField = 1 To pNumFields
    With pFields(iField)
      retval = retval & vbCrLf & "  .FieldName(" & iField & ") = """ & TrimNull(.fname) & """"
      retval = retval & vbCrLf & "  .FieldType(" & iField & ") = """ & .FType & """"
      retval = retval & vbCrLf & "  .FieldLength(" & iField & ") = " & .Length
      retval = retval & vbCrLf & "  .FieldDecimalCount(" & iField & ") = " & .DecimalCount
      retval = retval & vbCrLf
    End With
  Next
  retval = retval & vbCrLf & "  '.NumRecords = " & NumRecords
  retval = retval & vbCrLf & "  '.InitData"
  retval = retval & vbCrLf & "End With"
  retval = retval & vbCrLf
  CreationCode = retval
End Function

'Returns a new DBF with the same fields as this one, but no data
Public Function Cousin() As clsDBF
  Dim iTrash As Integer
  Dim iField As Integer
  
  Dim newDBF As clsDBF
  Set newDBF = New clsDBF
  With newDBF
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .NumFields = NumFields
    
    For iField = 1 To pNumFields
      .FieldName(iField) = FieldName(iField)
      .FieldType(iField) = FieldType(iField)
      .FieldLength(iField) = FieldLength(iField)
      .FieldDecimalCount(iField) = FieldDecimalCount(iField)
    Next
  End With
  Set Cousin = newDBF
End Function

Public Property Get Year() As Byte
  Year = pHeader.Year
End Property
Public Property Let Year(ByVal newValue As Byte)
  pHeader.Year = newValue
End Property

Public Property Get Month() As Byte
  Month = pHeader.Month
End Property
Public Property Let Month(ByVal newValue As Byte)
  pHeader.Month = newValue
End Property

Public Property Get Day() As Byte
  Day = pHeader.Day
End Property
Public Property Let Day(ByVal newValue As Byte)
  pHeader.Day = newValue
End Property

Public Property Get NumFields() As Long
  NumFields = pNumFields
End Property
Public Property Let NumFields(ByVal newValue As Long)
  Dim i As Integer
  pNumFields = newValue
  ReDim pFields(1 To pNumFields)
  For i = 1 To pNumFields
    pFields(i).FType = "C"
  Next
  pHeader.NumBytesHeader = (pNumFields + 1) * 32 + 1
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
  FieldName = LCase(FieldName)
  For retval = 1 To NumFields
    If LCase(TrimNull(pFields(retval).fname)) = FieldName Then
      FieldNumber = retval
      Exit Function
    End If
  Next
End Function

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

Public Property Get Filename() As String
  Filename = pFilename
End Property

Private Function CurrentRecordAsDelimitedString(Optional aDelimiter As String = ",", Optional aQuote As String = "") As String
  Dim retval As String
  Dim fieldVal As String
  Dim usingQuotes As Boolean
  Dim iField As Long
  If Len(aQuote) > 0 Then usingQuotes = True
  For iField = 1 To pNumFields
    fieldVal = Value(iField)
    If usingQuotes Then
      If InStr(fieldVal, aDelimiter) > 0 Then fieldVal = aQuote & fieldVal & aQuote
    End If
    retval = retval & Value(iField)
    If iField < pNumFields Then retval = retval & aDelimiter
  Next
  CurrentRecordAsDelimitedString = retval
End Function

'Returns True if found, moves CurrentRecord to first record with .Value(FieldNumber) = FindValue
'If not found, returns False and moves CurrentRecord to 1
Public Function FindFirst(ByVal FieldNumber As Long, FindValue As String, _
                          Optional ByVal aStartRecord As Long = 1, _
                          Optional ByVal aEndRecord As Long = -1) As Boolean
  If aEndRecord < 1 Then aEndRecord = NumRecords
  For pCurrentRecord = aStartRecord To aEndRecord
    pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1
    If Value(FieldNumber) = FindValue Then
      FindFirst = True
      Exit Function
    End If
  Next
  CurrentRecord = 1
  FindFirst = False
End Function

'Returns True if found, moves CurrentRecord to next record with .Value(FieldNumber) = FindValue
'If not found, returns False and moves CurrentRecord to 1
Public Function FindNext(ByVal FieldNumber As Long, FindValue As String) As Boolean
  FindNext = FindFirst(FieldNumber, FindValue, pCurrentRecord + 1)
End Function

'Returns True if found, moves CurrentRecord to first record with .Record = FindValue
'If not found, returns False and moves CurrentRecord to 1
Public Function FindRecord(ByRef FindValue() As Byte, _
                          Optional ByVal aStartRecord As Long = 1, _
                          Optional ByVal aEndRecord As Long = -1) As Boolean
  Dim i As Long
  Dim firstByte As Long
  Dim lastByte As Long
  Dim lastLong As Long
  Dim nLongs As Long
  If aEndRecord < 1 Then aEndRecord = NumRecords
  
  lastByte = pHeader.NumBytesRec - 1
  nLongs = pHeader.NumBytesRec \ 4
  lastLong = nLongs - 1
  firstByte = nLongs * 4
  
'  Dim byt As Long
'  Dim Match As Boolean
'  Dim rec As Long
'  For rec = aStartRecord To aEndRecord
'    CurrentRecord = rec
'    Match = True
'    For byt = 0 To pHeader.NumBytesRec - 1
'      If pData(pCurrentRecordStart + byt) <> FindValue(byt) Then
'        Match = False
'        Exit For
'      End If
'    Next
'    If Match Then Exit For
'  Next
  
  'CAUTION! DO NOT STOP VB after StartUsingCompareLongs until after FinishedUsingCompareLongs has been called
  StartUsingCompareLongs
  pLongHeader1(3) = VarPtr(FindValue(0))
  pCurrentRecord = aStartRecord
  pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1

CompareCurrentRecord:
    i = 0
    pLongHeader2(3) = VarPtr(pData(pCurrentRecordStart))
    For i = 0 To lastLong
      If pCompareLongs1(i) <> pCompareLongs2(i) Then GoTo NotEqual
    Next

    For i = firstByte To lastByte
      If pData(pCurrentRecordStart + i) <> FindValue(i) Then GoTo NotEqual
    Next

    FindRecord = True
    FinishedUsingCompareLongs
'    If Not Match Then Stop Else CountAgreeMatch = CountAgreeMatch + 1
    Exit Function

NotEqual:
    If pCurrentRecord < aEndRecord Then
      pCurrentRecord = pCurrentRecord + 1
      pCurrentRecordStart = pCurrentRecordStart + pHeader.NumBytesRec
      GoTo CompareCurrentRecord
    End If
  
  CurrentRecord = 1
  FindRecord = False
  FinishedUsingCompareLongs
'  If Match Then Stop Else CountAgreeNoMatch = CountAgreeNoMatch + 1
End Function

'Returns True if CurrentRecord matches FindValue
Public Function MatchRecord(ByRef FindValue() As Byte) As Boolean
  Dim Byt As Long
  Dim lastbyt As Long
  If UBound(FindValue) < pHeader.NumBytesRec Then
    lastbyt = UBound(FindValue)
  Else
    lastbyt = pHeader.NumBytesRec - 1
  End If
  For Byt = 0 To lastbyt
    If pData(pCurrentRecordStart + Byt) <> FindValue(Byt) Then
      MatchRecord = False
      Exit Function
    End If
  Next
  MatchRecord = True
End Function

'FindMatch Param Array
'Returns True if a record matching rules is found
' CurrentRecord will point to the next record matching aRules
'If aMatchAny is true, search will stop at a record matching any one rule
'If aMatchAny is false, search will stop only at a record matching all rules
'If not found, returns False and moves CurrentRecord to 1
'Arguments must appear in order following the pattern:
'field number
'operator such as =, <, >, <=, >=
'value to compare with
'For example, FindNextWhere(1, "=", "Mercury", 2, "<=", 0)
'will find next record where the first field value is "Mercury" and the second is less than or equal to zero
'Public Function FindMatchPA(ByVal aMatchAny As Boolean, _
'                            ByVal aStartRecord As Long, _
'                            ByVal aEndRecord As Long, _
'                            ParamArray aRules() As Variant) As Boolean
'  Dim iToken As Long
'  Dim numTokens As Long
'  Dim numArgs As Long
'  Dim iArg As Long
'  numArgs = UBound(aRules()) + 1
'  numTokens = numArgs / 3
'  If numTokens * 3 <> numArgs Then
'    MsgBox "Could not parse:number of args (" & numArgs & ") not divisible by 3", vbOKOnly, "clsDBF:FindNextAnd"
'  End If
'  Dim fieldNum() As Long
'  Dim operator() As String
'  Dim fieldVal() As Variant
'  Dim Token As String
'
'  ReDim fieldNum(numTokens)
'  ReDim operator(numTokens)
'  ReDim Values(numTokens)
'  iArg = 0
'  For iToken = 0 To numTokens - 1
'    Token = aRules(iArg)
'    If Not IsNumeric(Token) Then Token = FieldNumber(Token)
'    fieldNum(iToken) = CLng(Token)
'    If fieldNum(iToken) = 0 Then Debug.Print "FindNextRules:Field(" & aRules(iArg) & ") not found"
'    iArg = iArg + 1
'    operator(iToken) = aRules(iArg)
'    iArg = iArg + 1
'    fieldVal(iToken) = aRules(iArg)
'    iArg = iArg + 1
'  Next
'  FindMatchPA = FindMatch(fieldNum, operator, fieldVal, aMatchAny, aStartRecord, aEndRecord)
'End Function

Public Function FindMatch(aFieldNum() As Long, _
                          aOperator() As String, _
                          aFieldVal() As Variant, _
                          Optional ByVal aMatchAny As Boolean = False, _
                          Optional ByVal aStartRecord As Long = 1, _
                          Optional ByVal aEndRecord As Long = -1) As Boolean
  Dim numRules As Long
  Dim iRule As Long
  Dim allMatch As Boolean
  Dim thisMatches As Boolean
  Dim NotAtTheEnd As Boolean
  numRules = UBound(aFieldNum)
  
  If aEndRecord < 0 Then aEndRecord = NumRecords
  
  'If we are supposed to look for matches only in records that don't exist, we won't find any
  If aStartRecord > NumRecords Then
    FindMatch = False
    Exit Function
  End If
  
  CurrentRecord = aStartRecord
  NotAtTheEnd = True
  While NotAtTheEnd And CurrentRecord <= aEndRecord
    iRule = 1
    allMatch = True
    While iRule <= numRules And allMatch
      thisMatches = False
      Select Case aOperator(iRule)
        Case "=": If Value(aFieldNum(iRule)) = aFieldVal(iRule) Then thisMatches = True
        Case "<": If Value(aFieldNum(iRule)) < aFieldVal(iRule) Then thisMatches = True
        Case ">": If Value(aFieldNum(iRule)) > aFieldVal(iRule) Then thisMatches = True
        Case "<=": If Value(aFieldNum(iRule)) <= aFieldVal(iRule) Then thisMatches = True
        Case ">=": If Value(aFieldNum(iRule)) >= aFieldVal(iRule) Then thisMatches = True
        Case Else: Debug.Print "Unrecognized operator:" & aOperator(iRule)
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
      iRule = iRule + 1
    Wend
    If allMatch And Not aMatchAny Then
      FindMatch = True
      Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    End If
    If CurrentRecord < NumRecords Then
      CurrentRecord = CurrentRecord + 1
    Else
      NotAtTheEnd = False
    End If
  Wend
  CurrentRecord = 1
  FindMatch = False
End Function

Public Property Get NumRecords() As Long
  NumRecords = pHeader.NumRecs
End Property
Public Property Let NumRecords(ByVal newValue As Long)
  Dim iBlank As Long
  If newValue > pHeader.NumRecs Then
    pHeader.NumRecs = newValue
    iBlank = pDataBytes + 1
    If newValue > pNumRecsCapacity Then
      'Expand the data array capacity
      pNumRecsCapacity = (newValue + 1) * 1.5
      ReDim Preserve pData(pNumRecsCapacity * pHeader.NumBytesRec)
    End If
    pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
    'fill all newly allocated bytes of data array with spaces
    While iBlank <= pDataBytes
      pData(iBlank) = 32
      iBlank = iBlank + 1
    Wend
  ElseIf newValue < pHeader.NumRecs Then
    'Shrink the data array
    pHeader.NumRecs = newValue
    pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
    pNumRecsCapacity = newValue
    ReDim Preserve pData(pDataBytes)
  End If

End Property

Public Property Get CurrentRecord() As Long
  CurrentRecord = pCurrentRecord
End Property
Public Property Let CurrentRecord(ByVal newValue As Long)
  On Error GoTo ErrHand
  If newValue > pHeader.NumRecs Then NumRecords = newValue
  If newValue < 1 Or newValue > pHeader.NumRecs Then
    pCurrentRecord = 1
  Else
    pCurrentRecord = newValue
  End If
  pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1
  Exit Property
ErrHand:
  LogMsg "Cannot set CurrentRecord to " & newValue & vbCr & Err.Description, "Let CurrentRecord"
End Property

Public Property Get record() As Byte()
  'Dim i As Long
  Dim retval() As Byte
  ReDim retval(pHeader.NumBytesRec - 1)
  CopyMemory retval(0), pData(pCurrentRecordStart), pHeader.NumBytesRec
  'For i = 0 To pHeader.NumBytesRec - 1
  '  retval(i) = pData(pCurrentRecordStart + i)
  'Next
  record = retval
End Property
Public Property Let record(newValue() As Byte)
'  Dim i As Long
'  For i = 0 To pHeader.NumBytesRec - 1
'    pData(pCurrentRecordStart + i) = newValue(i)
'  Next
  If UBound(newValue) = pHeader.NumBytesRec - 1 Then
    CopyMemory pData(pCurrentRecordStart), newValue(0), pHeader.NumBytesRec
  Else
    LogMsg "Cannot Let record - wrong size newValue passed" & vbCr _
         & "new record is " & UBound(newValue) + 1 & " bytes long" & vbCr _
         & "but should be " & pHeader.NumBytesRec & " bytes long" & vbCr _
         & Err.Description, "Let record"
  End If
End Property

Public Property Get Value(ByVal fieldNum As Long) As String
  If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
    Value = "Invalid Current Record Number"
  ElseIf fieldNum < 1 Or fieldNum > NumFields Then
    Value = "Invalid Field Number"
  Else
    Dim FieldStart As Long
    Dim i As Long
    Dim strRet As String
    
    FieldStart = pCurrentRecordStart + pFields(fieldNum).DataAddress
        
    strRet = ""
    For i = 0 To pFields(fieldNum).Length - 1
      If pData(FieldStart + i) > 0 Then
        strRet = strRet & Chr(pData(FieldStart + i))
      Else
        i = 256
      End If
    Next
    Value = Trim(strRet)
'    If pFields(FieldNum).FType = "N" Then
'      Dim dblval As Double
'      dblval = CDbl(strRet)
'      If pFields(FieldNum).DecimalCount <> 0 Then
'        dblval = dblval * 10 ^ pFields(FieldNum).DecimalCount
'      End If
'      Value = dblval
'    End If
  End If
End Property
Public Property Let Value(ByVal fieldNum As Long, ByVal newValue As String)
  Dim FieldStart As Long
  Dim i As Long
  Dim strRet As String
  Dim lenStr As Long
  
  On Error GoTo ErrHand
  If pCurrentRecord < 1 Then
    'Value = "Invalid Current Record Number"
  ElseIf fieldNum < 1 Or fieldNum > NumFields Then
    'Value = "Invalid Field Number"
  Else
    pData(pCurrentRecordStart) = 32 'clear record deleted flag or overwrite EOF
    
    FieldStart = pCurrentRecordStart + pFields(fieldNum).DataAddress
    
    strRet = newValue
    lenStr = Len(strRet)
    If lenStr > pFields(fieldNum).Length Then
      strRet = Left(strRet, pFields(fieldNum).Length)
    ElseIf pFields(fieldNum).FType = "N" Then
      strRet = Space(pFields(fieldNum).Length - lenStr) & strRet
    Else
      strRet = strRet & Space(pFields(fieldNum).Length - lenStr)
    End If
    For i = 0 To pFields(fieldNum).Length - 1
      pData(FieldStart + i) = Asc(Mid(strRet, i + 1, 1))
    Next
  End If
  Exit Property
ErrHand:
  LogMsg "Cannot set field #" & fieldNum & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value"
End Property

'Dimension and initialize data buffer to all spaces (except for initial carriage return)
'Do not call on an existing DBF since all data will be removed from memory
'If creating a new DBF:
' Call after setting NumRecords, NumFields and all FieldLength
' Call before setting any Value
Public Sub InitData()
  Dim i As Long
  
  SetDataAddresses
  
  pHeader.NumBytesRec = pFields(pNumFields).DataAddress + pFields(pNumFields).Length
  
  pNumRecsCapacity = pHeader.NumRecs
  pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
  ReDim pData(pDataBytes)
  pData(0) = 13
  For i = 1 To pDataBytes
    pData(i) = 32
  Next
End Sub

Public Sub OpenDBF(ByVal Filename As String)

  'Dim header As dBHeader, FieldDes As FieldDescriptor    'Creating variables for user-defined types
  'Dim memo As String * 512                               'Create a 512 byte fixed string variable
                                                          ' to read memo fields
  Dim InFile As Integer
  Dim i As Integer
  
  If Not FileExists(Filename) Then
    Exit Sub 'Don't need to do any work to open a file that doesn't exist yet
  End If
  
  pFilename = Filename

  InFile = FreeFile(0)
  Open Filename For Binary As InFile
  Get #InFile, , pHeader
  Select Case pHeader.version 'Be sure we're using a dBASE III file
    Case 3 'Normal dBASEIII file
'   Case &H83 'Open a .DBT file
    Case Else
      LogMsg "This is not a dBASE III file: '" & Filename & "'", "OpenDBF"
      Close InFile
      Exit Sub
  End Select

  NumFields = pHeader.NumBytesHeader \ 32 - 1 'Calculate the number of fields

  'frm.fa.TextMatrix(frm.fa.Rows - 1, 0) = "Field  Field Name     Type   Width  Dec"
  For i = 1 To pNumFields
    Get #InFile, , pFields(i)  'Looping through NumFields by reading in 32 byte records
  Next i

  SetDataAddresses

  pDataBytes = LOF(InFile) - pHeader.NumBytesHeader + 1 'Adding one seems to help with some files
  pData = InputB(pDataBytes, InFile)
  pNumRecsCapacity = pHeader.NumRecs
  Close #InFile
  If pHeader.NumRecs > 0 Then
    CurrentRecord = 1
  Else
    pCurrentRecord = 0
  End If
End Sub

Private Sub SetDataAddresses()
  Dim i As Long
  pFields(1).DataAddress = 1
  For i = 2 To pNumFields
    pFields(i).DataAddress = pFields(i - 1).DataAddress + pFields(i - 1).Length
  Next
End Sub

Public Sub WriteDBF(ByVal Filename As String)
  Dim OutFile As Integer
  Dim i As Integer, j As Integer, dot As Integer
  Dim S As String
TryAgain:
  On Error GoTo ErrHand

  If FileExists(Filename) Then
    Kill Filename
  Else
    MkDirPath PathNameOnly(Filename)
  End If
  
  OutFile = FreeFile(0)
  Open Filename For Binary As OutFile
  Put #OutFile, , pHeader

  For i = 1 To (NumFields)
    pFields(i).DataAddress = 0 'Nobody else seems to leave non-zero values in there
    Put #OutFile, (32 * i) + 1, pFields(i)
  Next i
  
  SetDataAddresses 'Set them again since we zeroed them as we wrote
  
  'If we have over-allocated for adding more records, trim unused records
  If pNumRecsCapacity > pHeader.NumRecs Then
    pNumRecsCapacity = pHeader.NumRecs
    ReDim Preserve pData(pHeader.NumRecs * pHeader.NumBytesRec)
  End If
  
  Put #OutFile, , pData
  If pData(UBound(pData)) <> CtrlZ Then Put #OutFile, , CtrlZ 'Add EOF character
  Close #OutFile

  pFilename = Filename

Exit Sub

ErrHand:
  Resume Next
  Log "Error saving " & Filename & vbCr & Err.Description
  If MsgBox("Error saving " & Filename & vbCr & Err.Description, vbAbortRetryIgnore, "Write DBF") = vbRetry Then
    On Error Resume Next
    Close #OutFile
    GoTo TryAgain
  End If
End Sub

'     Select Case FieldDes(i).FType                       'Reading the dBASE Field Type
'        Case "C":           printtype$ = "Character"
'        Case "D":           printtype$ = "Date"
'        Case "N":           printtype$ = "Numeric"
'        Case "L":           printtype$ = "Logical"
'        Case "M":           printtype$ = "Memo"
'     End Select


'Static Sub Stripchar(a As String)
'
'  Dim sTemp As String
'  Dim sTemp2 As String
'  Dim iCount As Integer
'
'  iCount = InStr(a, Chr$(&HA))
'  Do While iCount
'     sTemp = Left$(a, iCount - 1)
'     sTemp2 = Right$(a, Len(a$) - iCount)
'     a$ = sTemp & sTemp2
'     iCount = InStr(a$, Chr$(&HA))
'  Loop
'  iCount = InStr(a, Chr$(&H8D))
'  Do While iCount
'     sTemp = Left$(a$, iCount - 1)
'     sTemp2 = Right$(a$, Len(a$) - iCount)
'     a$ = sTemp & Chr$(&HD) & sTemp2
'     iCount = InStr(a$, Chr$(&H8D))
'  Loop
'End Sub

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
  ' Set up our templates for comparing arrays
  pLongHeader1(0) = 1              ' Number of dimensions
  pLongHeader1(1) = 4              ' Bytes per element (long = 4)
  pLongHeader1(4) = &H7FFFFFFF     ' Array size
  
  pLongHeader2(0) = 1              ' Number of dimensions
  pLongHeader2(1) = 4              ' Bytes per element (long = 4)
  pLongHeader2(4) = &H7FFFFFFF     ' Array size
  
  Clear
End Sub

Public Sub Clear()
  pHeader.version = 3
  pHeader.Day = 1
  pHeader.Month = 1
  pHeader.Year = 70
  pHeader.NumBytesHeader = 32
  pHeader.NumBytesRec = 0
  pHeader.NumRecs = 0
  pNumFields = 0
  pDataBytes = 0
  pCurrentRecord = 1
  pCurrentRecordStart = 0
  pNumRecsCapacity = 0
  ReDim pData(0)
  ReDim pFields(0)
End Sub

'Merge records from dbf2Add into this dbf
'keyFieldNames are field names in the DBFs that define a unique field.
'If keyFieldNames is blank, no duplicate checking will occur
'If keyFieldNames(1) = "**ALL**" then the entire record will be used as a key
'DuplicateAction dictates handling of duplicate records as follows:
' 0 - duplicates allowed
' 1 - keep existing instance of duplicates and discard duplicates from dbf being added
' 2 - replace existing instance of duplicates with duplicates from dbf being added
' 3 - ask user what to do (not currently implemented)
Public Sub Merge(dbf2Add As clsDBF, keyFieldNames() As String, DuplicateAction As Long)
  Dim addRecordNum As Long
  Dim fieldNum As Long
  Dim keyField() As Long
  Dim operator() As String
  Dim keyValue() As Variant
  Dim recordToCopy() As Byte
  Dim firstKeyField As Long
  Dim lastKeyField As Long
  Dim lMsg As String
  Dim LastOldRec As Long
  Dim AllFieldsKey As Boolean
  Dim foundDuplicate As Boolean
  Dim canCopyRecords As Boolean

  Log "Merge " & dbf2Add.Filename & " into " & Me.Filename
    
  If dbf2Add.NumRecords > 0 And Me.NumFields <> dbf2Add.NumFields And Me.NumRecords < 1 Then
    'Replace our field definitions with the new ones since we have no data
    Me.NumFields = dbf2Add.NumFields
    For fieldNum = 1 To pNumFields
      Me.FieldName(fieldNum) = dbf2Add.FieldName(fieldNum)
      Me.FieldType(fieldNum) = dbf2Add.FieldType(fieldNum)
      Me.FieldLength(fieldNum) = dbf2Add.FieldLength(fieldNum)
      Me.FieldDecimalCount(fieldNum) = FieldDecimalCount(fieldNum)
    Next
    Me.NumRecords = 0
    Me.InitData
  End If
  
  If dbf2Add.NumRecords < 1 Then
    Log "No records to add from empty DBF:" & vbCr & dbf2Add.Filename
  ElseIf Me.NumFields <> dbf2Add.NumFields Then
    LogMsg "Different number of fields:" & vbCr _
          & Filename & " = " & Me.NumFields & vbCr _
          & dbf2Add.Filename & " = " & dbf2Add.NumFields & vbCr & vbCr _
          & "Cannot merge DBF files", "Merge"
  Else
    For fieldNum = 1 To pNumFields
      If UCase(Trim(Me.FieldName(fieldNum))) <> UCase(Trim(dbf2Add.FieldName(fieldNum))) Then
        If Not LogMsg("Field '" & Me.FieldName(fieldNum) & "' does not appear to match '" _
               & dbf2Add.FieldName(fieldNum) & "'" & vbCr _
               & "Proceed with merge anyway, treating these fields as matching?", "Merge", True) Then
          Exit Sub
        End If
      End If
    Next
    If DuplicateAction > 0 Then
      firstKeyField = LBound(keyFieldNames)
      lastKeyField = UBound(keyFieldNames)
RedimKeys:
      ReDim keyField(firstKeyField To lastKeyField)
      ReDim operator(firstKeyField To lastKeyField)
      ReDim keyValue(firstKeyField To lastKeyField)
      For fieldNum = firstKeyField To lastKeyField
        If AllFieldsKey Then
          keyField(fieldNum) = fieldNum
          operator(fieldNum) = "="
        Else
          If keyFieldNames(fieldNum) = "**ALL**" Then
            AllFieldsKey = True
            firstKeyField = 1
            lastKeyField = Me.NumFields
            GoTo RedimKeys
          Else
            lMsg = lMsg & keyFieldNames(fieldNum) & ", "
            keyField(fieldNum) = Me.FieldNumber(keyFieldNames(fieldNum))
            operator(fieldNum) = "="
          End If
        End If
      Next fieldNum
      If AllFieldsKey Then
        lMsg = "All fields must match to find a duplicate."
      Else
        If Len(lMsg) > 2 Then lMsg = " Looking for duplicate records in fields " & Left(lMsg, Len(lMsg) - 2)
      End If
    
      If Len(lMsg) > 0 Then
        Select Case DuplicateAction
          Case 0: Log lMsg & " Not checking for duplicates"
          Case 2: Log lMsg & " Overwriting existing with new duplicates"
          Case Else: Log lMsg & " Keeping existing; discarding new duplicates"
        End Select
        lMsg = ""
      End If
    End If

    LastOldRec = NumRecords 'Don't search for duplicates in newly added records
    If LastOldRec < 1 Then DuplicateAction = 0 'Don't bother checking for duplicates since we start empty
    dbf2Add.CurrentRecord = 1
    recordToCopy = dbf2Add.record
        
    If UBound(recordToCopy) + 1 = pHeader.NumBytesRec Then
      canCopyRecords = True
    Else
'      LogMsg "Different number of bytes per record:" & vbCr _
'            & Filename & " = " & pHeader.NumBytesRec & vbCr _
'            & dbf2Add.Filename & " = " & UBound(recordToCopy) + 1 & vbCr & vbCr _
'            & "Cannot merge DBF files", "Merge"
'      Exit Sub
      canCopyRecords = False
      Log "Different number of bytes per record:" & vbCr _
            & Filename & " = " & pHeader.NumBytesRec & vbCr _
            & dbf2Add.Filename & " = " & UBound(recordToCopy) + 1 & vbCr _
            & "Attempting to copy fields instead of records"
    End If
    
'    Dim starttime As Date
'    starttime = Now
    With dbf2Add
      For addRecordNum = 1 To .NumRecords
'        If (addRecordNum \ 100) * 100 = addRecordNum Then
'          Debug.Print "Adding " & addRecordNum & " at " & Format(Now - starttime, "h:mm:ss") & " Matches: " & CountAgreeMatch & " NoMatches: " & CountAgreeNoMatch
'        End If
        .CurrentRecord = addRecordNum
        If DuplicateAction = 0 Then
          'don't bother looking for a duplicate since we add them all anyway
        ElseIf AllFieldsKey And canCopyRecords Then
          'First check current record to see if it matches
          If Me.MatchRecord(.record) Then
            foundDuplicate = True
          ElseIf CurrentRecord < LastOldRec Then
            'Check next record before searching hard for a match
            'if trying to merge same data, next record will always be the one that matches
            CurrentRecord = CurrentRecord + 1
            If Me.MatchRecord(.record) Then
              foundDuplicate = True
            Else
              foundDuplicate = FindRecord(.record, 1, LastOldRec)
            End If
          Else
            foundDuplicate = FindRecord(.record, 1, LastOldRec)
          End If
        Else
          For fieldNum = firstKeyField To lastKeyField
            keyValue(fieldNum) = .Value(keyField(fieldNum))
          Next
          foundDuplicate = FindMatch(keyField, operator, keyValue, False, 1, LastOldRec)
        End If
        If foundDuplicate Then
          If DuplicateAction = 2 Then GoSub CopyRecord 'overwrite existing record with new record
        Else  'Copy this record in the DBF
          Me.CurrentRecord = Me.NumRecords + 1
          GoSub CopyRecord
        End If
      Next
    End With
  End If
  
  Exit Sub
CopyRecord:
  If canCopyRecords Then
    Me.record = dbf2Add.record
  Else
    For fieldNum = 1 To pNumFields
      Me.Value(fieldNum) = dbf2Add.Value(fieldNum)
    Next
  End If
  Return
End Sub

Public Property Set Logger(ByVal newValue As Object)
  Set pLogger = newValue
End Property

Private Sub Log(ByVal message As String)
  If pLogger Is Nothing Then
    Debug.Print "  clsDBF " & message
  Else
    pLogger.Log "  clsDBF " & message
  End If
End Sub

Private Function LogMsg(ByVal message As String, title As String, Optional AskYesNo As Boolean = False) As Boolean
  If pLogger Is Nothing Then
    If AskYesNo Then
      If MsgBox(message, vbYesNo, title & " (clsDBF)") = vbYes Then LogMsg = True
    Else
      MsgBox message, vbOKOnly, title & " (clsDBF)"
    End If
  Else
    If AskYesNo Then
      If pLogger.LogMsg(message, title & " (clsDBF)", "Yes", "No") = 1 Then LogMsg = True
    Else
      pLogger.LogMsg message, title & " (clsDBF)"
    End If
  End If
End Function

Private Sub StartUsingCompareLongs()
  ' Force pCompareLongs to use pLongHeader as its own header
  CopyMemory ByVal ArrPtr(pCompareLongs1), VarPtr(pLongHeader1(0)), 4
  CopyMemory ByVal ArrPtr(pCompareLongs2), VarPtr(pLongHeader2(0)), 4
End Sub
Private Sub FinishedUsingCompareLongs()
  ' Make pCompareLongs once again use their own headers
  ' If this code doesn't run the IDE will crash when this object is disposed of
  CopyMemory ByVal ArrPtr(pCompareLongs1), 0&, 4
  CopyMemory ByVal ArrPtr(pCompareLongs2), 0&, 4
End Sub

