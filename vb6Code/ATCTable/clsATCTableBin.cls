VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCTableBin"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Implements clsATCTable

Private Declare Sub CopyMemory Lib "Kernel32" Alias "RtlMoveMemory" (Destination As Any, Source As Any, ByVal Length As Long)

Private Const cMagic As String * 6 = "ATB001"
Private Const cZeroByte As Byte = 0

Private Type dBHeader
  Magic As String * 6 'ATB001
  NumFields As Long   'Number of fields per record
  NumRecs As Long     'Number of records in file
  NumBytesRec As Long 'Number of bytes per record
End Type

Private Const TypString = 0
Private Const TypInt8 = 1
Private Const TypInt16 = 2
Private Const TypInt32 = 4
'  TypInt64 = 8
Private Const TypFloat32 = 32
Private Const TypFloat64 = 64
Private Const TypBoolean = 100

Private Type FieldDescriptor
   FieldType As Byte   'see TypString through TypBoolean constants above
   FieldLength As Long 'Number of bytes used by a value in this field
   FieldStart As Long  'Number of byte within a record where this field starts (first is 0)
   FieldName As String 'Null-terminated name of the field
End Type

Private pFilename As String
Private pHeader As dBHeader
Private pFields() As FieldDescriptor
Private pData() As Byte
Private pDataBytes As Long
Private pCurrentRecord As Long
Private pCurrentRecordStart As Long
Private pLogger As Object

'Capacity in pData for records. Set to pHeader.NumRecs when data is read from a file
'and in InitData when creating a new DBF from scratch. May increase in Let Value.
Private pNumRecsCapacity As Long

Private pTmpBuffer() As Byte
'Private pTmpString As String
'Private pTmpInt8 As Boolean
Private pTmpInt16 As Integer
Private pTmpInt32 As Long
'Private pTmpInt64 = 8
Private pTmpFloat32 As Single
Private pTmpFloat64 As Double
'Private pTmpBoolean As Boolean

Private Function clsATCTable_BOF() As Boolean
  If pCurrentRecord <= 1 Then clsATCTable_BOF = True
End Function

Private Sub clsATCTable_Clear()
  clsATCTable_ClearData
  pHeader.Magic = cMagic
  pHeader.NumFields = 0
  pHeader.NumBytesRec = 0
  ReDim pFields(0)
End Sub

Private Sub clsATCTable_ClearData()
  pHeader.NumRecs = 0
  pDataBytes = 0
  pCurrentRecord = 1
  pCurrentRecordStart = 0
  pNumRecsCapacity = 0
  ReDim pData(0)
End Sub

Private Function clsATCTable_Cousin() As clsATCTable
  MsgBox "Cousin not yet impolemented for ATCTableBin"

End Function

Private Sub clsATCTable_CousinOf(aTable As clsATCTable)
  MsgBox "CousinOf not yet impolemented for ATCTableBin"
End Sub

Private Function clsATCTable_CreationCode() As String
  MsgBox "CreationCode not yet impolemented for ATCTableBin"
End Function

Private Property Let clsATCTable_CurrentRecord(ByVal newValue As Long)
  On Error GoTo ErrHand
  If newValue > pHeader.NumRecs Then clsATCTable_NumRecords = newValue
  If newValue < 1 Or newValue > pHeader.NumRecs Then
    pCurrentRecord = 1
  Else
    pCurrentRecord = newValue
  End If
  pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1)
  Exit Property
ErrHand:
  LogMsg "Cannot set CurrentRecord to " & newValue & vbCr & Err.Description, "Let CurrentRecord"
End Property

Private Property Get clsATCTable_CurrentRecord() As Long
  clsATCTable_CurrentRecord = pCurrentRecord
End Property

Private Function clsATCTable_EOF() As Boolean
  If pCurrentRecord >= pHeader.NumRecs Then clsATCTable_EOF = True
End Function

Private Property Let clsATCTable_FieldLength(ByVal aFieldNumber As Long, ByVal newValue As Long)
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    pFields(aFieldNumber).FieldLength = newValue
  End If
End Property

Private Property Get clsATCTable_FieldLength(ByVal aFieldNumber As Long) As Long
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    clsATCTable_FieldLength = pFields(aFieldNumber).FieldLength
  End If
End Property

Private Property Let clsATCTable_FieldName(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    pFields(aFieldNumber).FieldName = newValue
  End If
End Property

Private Property Get clsATCTable_FieldName(ByVal aFieldNumber As Long) As String
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    clsATCTable_FieldName = pFields(aFieldNumber).FieldName
  Else
    clsATCTable_FieldName = "Undefined"
  End If
End Property

Private Function clsATCTable_FieldNumber(ByVal aFieldName As String) As Long
  Dim retval As Long
  For retval = 1 To pHeader.NumFields
    If pFields(retval).FieldName = aFieldName Then
      clsATCTable_FieldNumber = retval
      Exit Function
    End If
  Next
End Function

Private Property Let clsATCTable_FieldType(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    If IsNumeric(newValue) Then
      pFields(aFieldNumber).FieldType = CByte(newValue)
    Else
      Select Case UCase(newValue)
        Case "C", "D", "M": pFields(aFieldNumber).FieldType = TypString
        Case "N": pFields(aFieldNumber).FieldType = TypFloat32
        Case "L": pFields(aFieldNumber).FieldType = TypBoolean
      End Select
    End If
  End If
End Property

Private Property Get clsATCTable_FieldType(ByVal aFieldNumber As Long) As String
  If aFieldNumber > 0 And aFieldNumber <= pHeader.NumFields Then
    Select Case pFields(aFieldNumber).FieldType
      Case TypString:
        If pFields(aFieldNumber).FieldLength < 255 Then
          clsATCTable_FieldType = "C"
        Else
          clsATCTable_FieldType = "M"
        End If
      Case TypInt8, TypInt16, TypInt32, TypFloat32, TypFloat64: clsATCTable_FieldType = "N"
      Case TypBoolean:    clsATCTable_FieldType = "L"
    End Select
  End If
End Property

Private Property Get clsATCTable_filename() As String
  clsATCTable_filename = pFilename
End Property

Private Function clsATCTable_FindFirst(ByVal aFieldNumber As Long, _
                                               aFindValue As String, _
                                Optional ByVal aStartRecord As Long = 1&, _
                                Optional ByVal aEndRecord As Long = -1&) As Boolean
  If aEndRecord < 1 Then aEndRecord = pHeader.NumRecs
  pCurrentRecordStart = pHeader.NumBytesRec * (aStartRecord - 1)
  For pCurrentRecord = aStartRecord To aEndRecord
    If clsATCTable_Value(aFieldNumber) = aFindValue Then
      clsATCTable_FindFirst = True
      Exit Function
    End If
    pCurrentRecordStart = pCurrentRecordStart + pHeader.NumBytesRec
  Next
  clsATCTable_CurrentRecord = aStartRecord
End Function

Private Function clsATCTable_FindNext(ByVal aFieldNumber As Long, aFindValue As String) As Boolean
  clsATCTable_FindNext = clsATCTable_FindFirst(aFieldNumber, aFindValue, pCurrentRecord + 1)
End Function

Private Property Let clsATCTable_record(newValue() As Byte)

End Property
Private Property Get clsATCTable_record() As Byte()

End Property

Private Sub clsATCTable_MoveFirst()
  clsATCTable_CurrentRecord = 1
End Sub

Private Sub clsATCTable_MoveLast()
  clsATCTable_CurrentRecord = pHeader.NumRecs
End Sub

Private Sub clsATCTable_MoveNext()
  clsATCTable_CurrentRecord = pCurrentRecord + 1
End Sub

Private Sub clsATCTable_MovePrevious()
  clsATCTable_CurrentRecord = pCurrentRecord - 1
End Sub

Private Property Let clsATCTable_NumFields(ByVal newValue As Long)
  Dim i As Long
  pHeader.NumFields = newValue
  ReDim pFields(1 To pHeader.NumFields)
End Property

Private Property Get clsATCTable_NumFields() As Long
  clsATCTable_NumFields = pHeader.NumFields
End Property

Private Property Let clsATCTable_NumRecords(ByVal newValue As Long)
  Dim iBlank As Long
  If newValue > pHeader.NumRecs Then
    pHeader.NumRecs = newValue
    iBlank = pDataBytes
    If newValue > pNumRecsCapacity Then
      'Expand the data array capacity
      pNumRecsCapacity = (newValue + 1) * 1.5
      ReDim Preserve pData(pNumRecsCapacity * pHeader.NumBytesRec)
    End If
    pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
    'fill all newly allocated bytes of data array with spaces
    While iBlank <= pDataBytes
      pData(iBlank) = 0
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

Private Property Get clsATCTable_NumRecords() As Long
  clsATCTable_NumRecords = pHeader.NumRecs
End Property

Private Function clsATCTable_OpenFile(ByVal Filename As String) As clsATCTable
  Dim iField As Long
  Dim inFile As Integer
  inFile = FreeFile
  If FileExists(Filename) Then
    Open Filename For Binary Access Read As inFile
    Get #inFile, , pHeader
    ReDim pFields(1 To pHeader.NumFields)
    For iField = 1 To pHeader.NumFields
      With pFields(iField)
        Get #inFile, , .FieldType
        Get #inFile, , .FieldLength
        .FieldName = ReadNullTerminatedStringFromFile(inFile)
      End With
    Next
    SetFieldStart
    pDataBytes = LOF(inFile) - Loc(inFile)
    pData = InputB(pDataBytes, inFile)
    pNumRecsCapacity = pHeader.NumRecs
    Close inFile
    Set clsATCTable_OpenFile = Me
    pFilename = Filename
  End If
End Function

Private Function ReadNullTerminatedStringFromFile(aFileHandle As Integer) As String
  Dim s As String
  Dim ch As Byte
  While Not EOF(aFileHandle)
    Get #aFileHandle, , ch
    If ch = 0 Then GoTo Finished
    s = s & Chr(ch)
  Wend
Finished:
  ReadNullTerminatedStringFromFile = s
End Function

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
    For iField = 1 To pHeader.NumFields
      With pFields(iField)
        retval = retval & vbCrLf & "Field " & iField & ": '" & .FieldName & "'"
        retval = retval & vbCrLf & "    Type: " & .FieldType & " "
        If ExpandType Then
          Select Case .FieldType
            Case TypString:  retval = retval & "String   "
            Case TypInt8:    retval = retval & "Byte     "
            Case TypInt16:   retval = retval & "Integer  "
            Case TypInt32:   retval = retval & "Long     "
            Case TypFloat32: retval = retval & "Single   "
            Case TypFloat64: retval = retval & "Double   "
            Case TypBoolean: retval = retval & "Boolean  "
            Case Else:       retval = retval & "Unknown  "
          End Select
        Else
          retval = retval & .FieldType
        End If
        retval = retval & vbCrLf & "    Length: " & .FieldLength & " "
      End With
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
    For iField = 1 To pHeader.NumFields
      With pFields(iField)
        retval = retval & iField & vbTab & "'" & .FieldName & "' "
        If ExpandType Then
          Select Case .FieldType
            Case TypString:  retval = retval & vbTab & "String   "
            Case TypInt8:    retval = retval & vbTab & "Byte     "
            Case TypInt16:   retval = retval & vbTab & "Integer  "
            Case TypInt32:   retval = retval & vbTab & "Long     "
            Case TypFloat32: retval = retval & vbTab & "Single   "
            Case TypFloat64: retval = retval & vbTab & "Double   "
            Case TypBoolean: retval = retval & vbTab & "Boolean  "
            Case Else:       retval = retval & vbTab & "Unknown  "
          End Select
        Else
          retval = retval & vbTab & .FieldType
        End If
        retval = retval & vbTab & .FieldLength
      End With
      retval = retval & vbCrLf
    Next
  End If
  clsATCTable_SummaryFields = retval
End Function

Private Function clsATCTable_SummaryFile(Optional aFormat As String = "tab,headers") As String
  Dim retval As String
  Dim ShowHeaders As Boolean
  
  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
  
  If LCase(aFormat) = "text" Then 'text version
    With pHeader
      retval = "Header: "
      retval = retval & vbCrLf & "    FileName: " & pFilename
      retval = retval & vbCrLf & "    Magic: " & .Magic
      retval = retval & vbCrLf & "    NumFields: " & .NumFields
      retval = retval & vbCrLf & "    NumRecs: " & .NumRecs
      retval = retval & vbCrLf & "    NumBytesRec: " & .NumBytesRec
    End With
  Else 'table version
    'build header header
    If ShowHeaders Then
      retval = "FileName "
      retval = retval & vbTab & "Magic "
      retval = retval & vbTab & "NumFields "
      retval = retval & vbTab & "NumRecs "
      retval = retval & vbTab & "NumBytesRec "
    End If
    retval = retval & vbCrLf
    With pHeader 'now header data
      retval = retval & pFilename
      retval = retval & vbTab & .Magic
      retval = retval & vbTab & .NumFields
      retval = retval & vbTab & .NumRecs
      retval = retval & vbTab & .NumBytesRec
      retval = retval & vbCrLf
    End With
  End If
  clsATCTable_SummaryFile = retval
End Function

Private Property Let clsATCTable_Value(ByVal aFieldNumber As Long, ByVal newValue As String)
  Dim FieldStart As Long
  Dim iSrc As Long
  Dim iDst As Long
  Dim lLen As Long
  Dim tmpstr As String
  
  If pHeader.NumBytesRec = 0 Then InitData
  
  On Error GoTo ErrHand
  If pCurrentRecord < 1 Then
    'Value = "Invalid Current Record Number"
  ElseIf aFieldNumber < 1 Or aFieldNumber > pHeader.NumFields Then
    'Value = "Invalid Field Number"
  Else
    
    FieldStart = pCurrentRecordStart + pFields(aFieldNumber).FieldStart
    
    Select Case pFields(aFieldNumber).FieldType
      Case TypString:
    
        tmpstr = newValue
        lLen = Len(tmpstr)
        If lLen > pFields(aFieldNumber).FieldLength Then
          lLen = pFields(aFieldNumber).FieldLength
          tmpstr = Left(tmpstr, lLen)
        End If

        pTmpBuffer = tmpstr 'Assign string to byte array (unicode double bytes)
        iDst = FieldStart
        iSrc = 0
        lLen = lLen * 2
        While iSrc < lLen 'Copy lower (ASCII) byte of each char to data
          pData(iDst) = pTmpBuffer(iSrc)
          iDst = iDst + 1
          iSrc = iSrc + 2
        Wend
      Case TypInt8:      pData(FieldStart) = CByte(newValue)
      Case TypInt16:     pTmpInt16 = CInt(newValue): CopyMemory pData(FieldStart), pTmpInt16, 2
      Case TypInt32:     pTmpInt32 = CLng(newValue): CopyMemory pData(FieldStart), pTmpInt32, 4
      'Case TypInt64:
      Case TypFloat32: pTmpFloat32 = CSng(newValue): CopyMemory pData(FieldStart), pTmpFloat32, 4
      Case TypFloat64: pTmpFloat64 = CDbl(newValue): CopyMemory pData(FieldStart), pTmpFloat64, 8
      Case TypBoolean:
        If Len(newValue) = 0 Then
          pData(FieldStart) = 0
        ElseIf CBool(newValue) Then
          pData(FieldStart) = 1
        Else
          pData(FieldStart) = 0
        End If
    End Select
  End If
  Exit Property
ErrHand:
  If Err.Number = 13 Then
    Select Case pFields(aFieldNumber).FieldType
      Case TypString:
      Case TypInt8, TypInt16, TypInt32, TypFloat32, TypFloat64
        If Len(newValue) < 1 Then
          Debug.Print "Let Value: Empty value not assigned to numeric field #" & aFieldNumber & " in record #" & pCurrentRecord
        ElseIf Not IsNumeric(newValue) Then
          Debug.Print "Let Value: Non-numeric '" & newValue & "' not assigned to numeric field #" & aFieldNumber & " in record #" & pCurrentRecord
        Else
          Debug.Print "Let Value: '" & newValue & "' cannot be assigned to field #" & aFieldNumber & " in record #" & pCurrentRecord
        End If
      Case TypBoolean:
        Debug.Print "Let Value: Non-Boolean '" & newValue & "' not assigned to Boolean field #" & aFieldNumber & " in record #" & pCurrentRecord
      Case Else
        Debug.Print "Let Value: '" & newValue & "' cannot be assigned to field #" & aFieldNumber & " in record #" & pCurrentRecord & vbCr & Err.Description
    End Select
  Else
    Debug.Print "Let Value:  '" & newValue & "' cannot be assigned to field #" & aFieldNumber & " in record #" & pCurrentRecord & vbCr & Err.Description
  End If
End Property

Private Property Get clsATCTable_Value(ByVal aFieldNumber As Long) As String
  Dim lLen As Long
  Dim iSrc As Long
  Dim iDst As Long
  
  If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
    clsATCTable_Value = "Invalid Current Record Number"
  ElseIf aFieldNumber < 1 Or aFieldNumber > pHeader.NumFields Then
    clsATCTable_Value = "Invalid Field Number"
  Else
    Dim FieldStart As Long
    Dim i As Long
    Dim strRet As String
    
    FieldStart = pCurrentRecordStart + pFields(aFieldNumber).FieldStart
            
    Select Case pFields(aFieldNumber).FieldType
      Case TypString:
        lLen = pFields(aFieldNumber).FieldLength * 2
        ReDim pTmpBuffer(lLen)
        iSrc = FieldStart
        iDst = 0
        While iDst < lLen
          pTmpBuffer(iDst) = pData(iSrc)
          If pTmpBuffer(iDst) < 1 Then
            ReDim Preserve pTmpBuffer(iDst)
            clsATCTable_Value = pTmpBuffer
            iDst = lLen + 5
          Else
            iSrc = iSrc + 1
            iDst = iDst + 2 'Add odd zero byte so string will read the byte array correctly
          End If
        Wend
        If iDst < lLen + 2 Then clsATCTable_Value = pTmpBuffer
      
      Case TypInt8:  clsATCTable_Value = Chr(pData(FieldStart))
      Case TypInt16:    CopyMemory pTmpInt16, pData(FieldStart), 2: clsATCTable_Value = CStr(pTmpInt16)
      Case TypInt32:    CopyMemory pTmpInt32, pData(FieldStart), 4: clsATCTable_Value = CStr(pTmpInt32)
      'Case TypInt64:
      Case TypFloat32: CopyMemory pTmpFloat32, pData(FieldStart), 4: clsATCTable_Value = CStr(pTmpFloat32)
      Case TypFloat64: CopyMemory pTmpFloat64, pData(FieldStart), 8: clsATCTable_Value = CStr(pTmpFloat64)
      Case TypBoolean: If pData(FieldStart) = 0 Then clsATCTable_Value = "False" Else clsATCTable_Value = "True"
      Case Else: clsATCTable_Value = "Unknown Type #" _
                                   & pFields(aFieldNumber).FieldType _
                                   & " in field " & aFieldNumber
    End Select
  End If
End Property

Private Sub clsATCTable_WriteFile(ByVal Filename As String)
  Dim iField As Long
  Dim OutFile As Integer
  OutFile = FreeFile
  If FileExists(Filename) Then Kill Filename
  Open Filename For Binary Access Write As OutFile
  Put #OutFile, , pHeader
  For iField = 1 To pHeader.NumFields
    With pFields(iField)
      Put #OutFile, , .FieldType
      Put #OutFile, , .FieldLength
      Put #OutFile, , .FieldName
      Put #OutFile, , cZeroByte
    End With
  Next
  If pNumRecsCapacity > pDataBytes Then
    pNumRecsCapacity = pDataBytes
    ReDim Preserve pData(pDataBytes)
  End If
  Put #OutFile, , pData
  Close OutFile
End Sub

'Call only on a table being created from scratch, not read from a file
' Call after setting NumFields and all FieldLength and FieldType.
' Will use existing NumRecords and can expand later if NumRecords is increased
' Call before setting any Value
Private Sub InitData()
  Dim i As Long
  
  SetFieldStart
  
  pHeader.NumBytesRec = pFields(pHeader.NumFields).FieldStart + pFields(pHeader.NumFields).FieldLength
  
  pNumRecsCapacity = pHeader.NumRecs
  pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
  ReDim pData(pDataBytes)
End Sub

Private Sub SetFieldStart()
  Dim i As Long
  pFields(1).FieldStart = 0
  For i = 2 To pHeader.NumFields
    pFields(i).FieldStart = pFields(i - 1).FieldStart + pFields(i - 1).FieldLength
  Next
End Sub

Private Sub Class_Initialize()
  clsATCTable_Clear
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

'  Dim iRecord As Long
'  Dim t As clsATCTable
'  Set t = New clsATCTableBin
'
'  t.NumFields = 2
'  t.FieldLength(1) = 5
'  t.FieldType(1) = "C"
'  t.FieldName(1) = "Five-Character String"
'  t.FieldLength(2) = 4
'  t.FieldType(2) = 4
'  t.FieldName(2) = "Long Integer"
'
'  t.NumRecords = 4
'
'  t.Value(1) = "Hello"
'  t.Value(2) = "1"
'
'  t.CurrentRecord = 2
'  t.Value(1) = "Hallo"
'  t.Value(2) = "2"
'
'  t.CurrentRecord = 3
'  t.Value(1) = "Hullo"
'  t.Value(2) = "3"
'
'  t.CurrentRecord = 4
'  t.Value(1) = "Hiya"
'  t.Value(2) = "4"
'
'  Debug.Print t.FieldName(1), t.FieldName(2)
'  Debug.Print t.FieldType(1), t.FieldType(2)
'  Debug.Print t.FieldLength(1), t.FieldLength(2)
'
'  t.WriteFile "C:\test.abt"
'  Set t = Nothing
'  Set t = New clsATCTableBin
'  t.OpenFile "C:\test.abt"
'
'  Debug.Print t.FieldName(1), t.FieldName(2)
'  Debug.Print t.FieldType(1), t.FieldType(2)
'  Debug.Print t.FieldLength(1), t.FieldLength(2)
'
'  For iRecord = 1 To t.NumRecords
'    t.CurrentRecord = iRecord
'    Debug.Print t.Value(1), t.Value(2)
'  Next
'
''  Dim b() As Byte
''  Dim s As String
''  s = "ded"
''  ReDim b(0 To 5)
''  b(0) = Asc("1")
''  'b(1) = Asc("a")
''  'b(2) = Asc("b")
''  'b(3) = Asc("c")
''
''  s = b
''  Debug.Print s, Len(s), Asc(Mid(s, 2))
''
''  Debug.Print b(1)
''  Debug.Print b(2)
''  Debug.Print b(3)

