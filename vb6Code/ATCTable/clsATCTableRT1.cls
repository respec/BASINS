VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCTableRT1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Implements clsATCTable

Private Type FieldDescriptor
   FieldName As String
   BlankAllowed As Boolean
   LeftJustified As Boolean
   Numeric As Boolean
   DataAddress As Long 'offset from record start to field start
   FieldLength As Long
   Description As String
End Type

Private pFilename As String
Private pFields() As FieldDescriptor
Private pNumFields As Long
Private pData() As Byte
Private pDataBytes As Long
Private pCurrentRecord As Long
Private pCurrentRecordStart As Long
Private pLogger As Object

Private pNumRecs As Long
Private pNumBytesRec As Integer

'Capacity in pData for records. Set to pNumRecs when data is read from a file
'and in InitData when creating a new DBF from scratch. May increase in Let Value.
Private pNumRecsCapacity As Long

Public Property Get FieldDecimalCount(ByVal aFieldNumber As Long) As Byte
  FieldDecimalCount = 0
End Property
Public Property Let FieldDecimalCount(ByVal aFieldNumber As Long, ByVal newValue As Byte)
End Property

Private Sub Class_Initialize()
  clsATCTable_Clear
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

Private Function clsATCTable_BOF() As Boolean
  If pCurrentRecord <= 1 Then clsATCTable_BOF = True
End Function

Private Sub AddField(aFieldName As String, aBlankAllowed As Boolean, _
                     aLeftJustified As Boolean, aNumeric As Boolean, _
                     aStartChar As Long, EndChar As Long, aFieldLen As Long, _
                     aDescription As String)
  pNumFields = pNumFields + 1
  ReDim Preserve pFields(1 To pNumFields)
  With pFields(pNumFields)
    .FieldName = aFieldName
    .BlankAllowed = aBlankAllowed
    .LeftJustified = aLeftJustified
    .Numeric = aNumeric
    .DataAddress = aStartChar
    .FieldLength = aFieldLen
    .Description = aDescription
    pNumBytesRec = .DataAddress + .FieldLength + 1 '2 larger than fields for CR/LF
  End With
End Sub

Private Sub clsATCTable_Clear()
  Dim iField As Long
  clsATCTable_ClearData
  pNumFields = 0
  ReDim pFields(1 To 1)
  
'Record Type 1 – Complete Chain Basic Data Record
'Field BV Fmt Type Beg End Len Description
  AddField "RT", False, True, False, 1, 1, 1, "Record Type"
  AddField "VERSION", False, True, True, 2, 5, 4, "Version Number"
  AddField "TLID", False, False, True, 6, 15, 10, "TIGER/Line ID, Permanent 1-Cell Number"
  AddField "SIDE1", True, False, True, 16, 16, 1, "Single-Side Source Code"
  AddField "SOURCE", True, True, False, 17, 17, 1, "Linear Segment Source Code"
  AddField "FEDIRP", True, True, False, 18, 19, 2, "Feature Direction, Prefix"
  AddField "FENAME", True, True, False, 20, 49, 30, "Feature Name"
  AddField "FETYPE", True, True, False, 50, 53, 4, "Feature Type"
  AddField "FEDIRS", True, True, False, 54, 55, 2, "Feature Direction, Suffix"
  AddField "CFCC", False, True, False, 56, 58, 3, "Census Feature Class Code"
  AddField "FRADDL", True, False, False, 59, 69, 11, "Start Address, Left"
  AddField "TOADDL", True, False, False, 70, 80, 11, "End Address, Left"
  AddField "FRADDR", True, False, False, 81, 91, 11, "Start Address, Right"
  AddField "TOADDR", True, False, False, 92, 102, 11, "End Address, Right"
  AddField "FRIADDL", True, True, False, 103, 103, 1, "Start Imputed Address Flag, Left"
  AddField "TOIADDL", True, True, False, 104, 104, 1, "End Imputed Address Flag, Left"
  AddField "FRIADDR", True, True, False, 105, 105, 1, "Start Imputed Address Flag, Right"
  AddField "TOIADDR", True, True, False, 106, 106, 1, "End Imputed Address Flag, Right"
  AddField "ZIPL", True, True, True, 107, 111, 5, "ZIP Code, Left"
  AddField "ZIPR", True, True, True, 112, 116, 5, "ZIP Code, Right"
  AddField "AIANHHFPL", True, True, True, 117, 121, 5, "FIPS 55 Code (American Indian/Alaska Native Area/Hawaiian Home Land), 2000 Left"
  AddField "AIANHHFPR", True, True, True, 122, 126, 5, "FIPS 55 Code (American Indian/Alaska Native Area/Hawaiian Home Land), 2000 Right"
  AddField "AIHHTLIL", True, True, False, 127, 127, 1, "American Indian/Hawaiian Home Land Trust Land Indicator, 2000 Left"
  AddField "AIHHTLIR", True, True, False, 128, 128, 1, "American Indian/Hawaiian Home Land Trust Land Indicator, 2000 Right"
  AddField "CENSUS1", True, True, False, 129, 129, 1, "Census Use 1"
  AddField "CENSUS2", True, True, False, 130, 130, 1, "Census Use 2"
  AddField "STATEL", True, True, True, 131, 132, 2, "FIPS State Code, 2000 Left (always filled both sides, except at U.S. boundaries)"
  AddField "STATER", True, True, True, 133, 134, 2, "FIPS State Code, 2000 Right (always filled both sides, except at U.S. boundaries)"
  AddField "COUNTYL", True, True, True, 135, 137, 3, "FIPS County Code, 2000 Left (always filled both sides, except at U.S. boundaries)"
  AddField "COUNTYR", True, True, True, 138, 140, 3, "FIPS County Code, 2000 Right (always filled both sides, except at U.S. boundaries)"
  AddField "COUSUBL", True, True, True, 141, 145, 5, "FIPS 55 Code (County Subdivision), 2000 Left"
  AddField "COUSUBR", True, True, True, 146, 150, 5, "FIPS 55 Code (County Subdivision), 2000 Right"
  AddField "SUBMCDL", True, True, True, 151, 155, 5, "FIPS 55 Code (Subbarrio), 2000 Left"
  AddField "SUBMCDR", True, True, True, 156, 160, 5, "FIPS 55 Code (Subbarrio), 2000 Right"
  AddField "PLACEL", True, True, True, 161, 165, 5, "FIPS 55 Code (Place/CDP), 2000 Left"
  AddField "PLACER", True, True, True, 166, 170, 5, "FIPS 55 Code (Place/CDP), 2000 Right"
  AddField "TRACTL", True, True, True, 171, 176, 6, "Census Tract, 2000 Left"
  AddField "TRACTR", True, True, True, 177, 182, 6, "Census Tract, 2000 Right"
  AddField "BLOCKL", True, True, True, 183, 186, 4, "Census Block Number, 2000 Left"
  AddField "BLOCKR", True, True, True, 187, 190, 4, "Census Block Number, 2000 Right"
  AddField "FRLONG", False, False, True, 191, 200, 10, "Start Longitude"
  AddField "FRLAT", False, False, True, 201, 209, 9, "Start Latitude"
  AddField "TOLONG", False, False, True, 210, 219, 10, "End Longitude"
  AddField "TOLAT", False, False, True, 220, 228, 9, "End Latitude"
End Sub

Private Sub clsATCTable_ClearData()
  pNumRecs = 0
  pDataBytes = 0
  pCurrentRecord = 1
  pCurrentRecordStart = 0
  pNumRecsCapacity = 0
  ReDim pData(0)
End Sub

Private Function clsATCTable_Cousin() As clsATCTable
  Set clsATCTable_Cousin = New clsATCTableRT1
End Function

Private Sub clsATCTable_CousinOf(aTable As clsATCTable)
  MsgBox "CousinOf not yet impolemented for ATCTableRT1"
End Sub

Private Function clsATCTable_CreationCode() As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  
  retval = "Dim newTable as clsATCTable"
  retval = retval & vbCrLf & "set newTable = new clsATCTableRT1"
  retval = retval & vbCrLf & "With newTable"
  
  retval = retval & vbCrLf & "  .NumFields = " & pNumFields
  retval = retval & vbCrLf
    
  For iField = 1 To pNumFields
    With pFields(iField)
      retval = retval & vbCrLf & "  .FieldName(" & iField & ") = """ & .FieldName & """"
      retval = retval & vbCrLf & "  .FieldType(" & iField & ") = """ & clsATCTable_FieldType(iField) & """"
      retval = retval & vbCrLf & "  .FieldLength(" & iField & ") = " & .FieldLength
      retval = retval & vbCrLf
    End With
  Next
  retval = retval & vbCrLf & "  '.NumRecords = " & pNumRecs
  retval = retval & vbCrLf & "  '.InitData"
  retval = retval & vbCrLf & "End With"
  retval = retval & vbCrLf
  clsATCTable_CreationCode = retval
End Function

Private Property Let clsATCTable_CurrentRecord(ByVal newValue As Long)
  On Error GoTo ErrHand
  'If newValue > pNumRecs Then clsATCTable_NumRecords = newValue
  If newValue < 1 Or newValue > pNumRecs Then
    pCurrentRecord = 1
  Else
    pCurrentRecord = newValue
  End If
  pCurrentRecordStart = pNumBytesRec * (pCurrentRecord - 1)
  Exit Property
ErrHand:
  LogMsg "Cannot set CurrentRecord to " & newValue & vbCr & Err.Description, "Let CurrentRecord"
End Property

Private Property Get clsATCTable_CurrentRecord() As Long
  clsATCTable_CurrentRecord = pCurrentRecord
End Property

Private Function clsATCTable_EOF() As Boolean
  If pCurrentRecord >= pNumRecs Then clsATCTable_EOF = True
End Function

Private Property Let clsATCTable_FieldLength(ByVal aFieldNumber As Long, ByVal newValue As Long)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    pFields(aFieldNumber).FieldLength = newValue
  End If
End Property

Private Property Get clsATCTable_FieldLength(ByVal aFieldNumber As Long) As Long
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    clsATCTable_FieldLength = pFields(aFieldNumber).FieldLength
  Else
    clsATCTable_FieldLength = 0
  End If
End Property

'FieldName is a maximum of 10 characters long, padded to 11 characters with nulls
Private Property Let clsATCTable_FieldName(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    newValue = Trim(Left(newValue, 10))
    pFields(aFieldNumber).FieldName = newValue
  End If
End Property

Private Property Get clsATCTable_FieldName(ByVal aFieldNumber As Long) As String
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    clsATCTable_FieldName = pFields(aFieldNumber).FieldName
  Else
    clsATCTable_FieldName = "Undefined"
  End If
End Property

'Returns zero if the named field does not appear in this file
Private Function clsATCTable_FieldNumber(ByVal aFieldName As String) As Long
  Dim retval As Long
  For retval = 1 To pNumFields
    If pFields(retval).FieldName = UCase(aFieldName) Then
      clsATCTable_FieldNumber = retval
      Exit Function
    End If
  Next
End Function

Private Property Let clsATCTable_FieldType(ByVal aFieldNumber As Long, ByVal newValue As String)
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    If UCase(newValue) = "N" Then
      pFields(aFieldNumber).Numeric = True
    Else
      pFields(aFieldNumber).Numeric = False
    End If
  End If
End Property

'C = Character, N = Numeric
Private Property Get clsATCTable_FieldType(ByVal aFieldNumber As Long) As String
  If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    If pFields(aFieldNumber).Numeric Then
      clsATCTable_FieldType = "N"
    Else
      clsATCTable_FieldType = "C"
    End If
  Else
    clsATCTable_FieldType = "Undefined"
  End If
End Property

Private Property Get clsATCTable_filename() As String
  clsATCTable_filename = pFilename
End Property

Private Function clsATCTable_FindFirst(ByVal aFieldNumber As Long, aFindValue As String, _
                                       Optional ByVal aStartRecord As Long = 1&, _
                                       Optional ByVal aEndRecord As Long = -1&) As Boolean
  If aEndRecord < 1 Then aEndRecord = pNumRecs
  For pCurrentRecord = aStartRecord To aEndRecord
    pCurrentRecordStart = pNumBytesRec * (pCurrentRecord - 1)
    If clsATCTable_Value(aFieldNumber) = aFindValue Then
      clsATCTable_FindFirst = True
      Exit Function
    End If
  Next
  clsATCTable_CurrentRecord = aStartRecord
End Function

Private Function clsATCTable_FindNext(ByVal aFieldNumber As Long, aFindValue As String) As Boolean
  clsATCTable_FindNext = clsATCTable_FindFirst(aFieldNumber, aFindValue, pCurrentRecord + 1)
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

Private Property Let clsATCTable_NumFields(ByVal newValue As Long)
  Dim i As Integer
  pNumFields = newValue
  ReDim pFields(1 To pNumFields)
End Property

Private Property Get clsATCTable_NumFields() As Long
  clsATCTable_NumFields = pNumFields
End Property

Private Property Let clsATCTable_NumRecords(ByVal newValue As Long)
  Dim iBlank As Long
  If newValue > pNumRecs Then
    pNumRecs = newValue
    iBlank = pDataBytes + 1
    If newValue > pNumRecsCapacity Then
      'Expand the data array capacity
      pNumRecsCapacity = (newValue + 1) * 1.5
      ReDim Preserve pData(pNumRecsCapacity * pNumBytesRec)
    End If
    pDataBytes = pNumRecs * pNumBytesRec
    'fill all newly allocated bytes of data array with spaces
    While iBlank <= pDataBytes
      pData(iBlank) = 32
      iBlank = iBlank + 1
    Wend
  ElseIf newValue < pNumRecs Then
    'Shrink the data array
    pNumRecs = newValue
    pDataBytes = pNumRecs * pNumBytesRec
    pNumRecsCapacity = newValue
    ReDim Preserve pData(pDataBytes)
  End If
End Property

Private Property Get clsATCTable_NumRecords() As Long
  clsATCTable_NumRecords = pNumRecs
End Property

Private Function clsATCTable_OpenFile(ByVal Filename As String) As clsATCTable
  Dim inFile As Integer
  Dim i As Integer
  
  pFilename = Filename
  
  If Not FileExists(Filename) Then
    Exit Function 'Don't need to do any work to open a file that doesn't exist yet
  End If

  pDataBytes = FileLen(pFilename)
  pData = WholeFileBytes(pFilename)
  
  pNumRecs = pDataBytes / pNumBytesRec
  pNumRecsCapacity = pNumRecs
  
  If pNumRecs > 0 Then
    clsATCTable_MoveFirst
  Else
    pCurrentRecord = 0
  End If
  Set clsATCTable_OpenFile = Me
End Function

Private Property Let clsATCTable_record(newValue() As Byte)

End Property
Private Property Get clsATCTable_record() As Byte()

End Property

Private Function clsATCTable_Summary(Optional aFormat As String = "tab,headers,expandtype") As String
  clsATCTable_Summary = clsATCTable_SummaryFile(aFormat) & vbCrLf & clsATCTable_SummaryFields(aFormat)
End Function

Private Function clsATCTable_SummaryFields(Optional aFormat As String = "tab,headers,expandtype") As String
  Dim retval As String
  Dim iField As Integer
  Dim ShowHeaders As Boolean
  Dim ExpandType As Boolean
  
  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
  If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True
  
  If InStr(LCase(aFormat), "text") > 0 Then 'text version
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & vbCrLf & "Field " & iField & ": '" & .FieldName & "'"
        retval = retval & vbCrLf & "    Type: "
        If ExpandType Then
          Select Case clsATCTable_FieldType(iField)
            Case "C": retval = retval & "Character"
            Case "D": retval = retval & "Date     "
            Case "N": retval = retval & "Numeric  "
            Case "L": retval = retval & "Logical  "
            Case "M": retval = retval & "Memo     "
          End Select
        Else
          retval = retval & clsATCTable_FieldType(iField)
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
    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & iField & vbTab & "'" & .FieldName & "' "
        If ExpandType Then
          Select Case clsATCTable_FieldType(iField)
            Case "C": retval = retval & vbTab & "Character"
            Case "D": retval = retval & vbTab & "Date     "
            Case "N": retval = retval & vbTab & "Numeric  "
            Case "L": retval = retval & vbTab & "Logical  "
            Case "M": retval = retval & vbTab & "Memo     "
          End Select
        Else
          retval = retval & vbTab & clsATCTable_FieldType(iField)
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
    retval = retval & vbCrLf & "    FileName: " & pFilename
    retval = retval & vbCrLf & "    NumRecs: " & pNumRecs
    retval = retval & vbCrLf & "    NumBytesRec: " & pNumBytesRec
  Else 'table version
    'build header header
    If ShowHeaders Then
      retval = "FileName "
      retval = retval & vbTab & "NumFields "
      retval = retval & vbTab & "NumRecs "
      retval = retval & vbTab & "NumBytesRec "
    End If
    retval = retval & vbCrLf
    retval = retval & pFilename
    retval = retval & vbTab & pNumFields
    retval = retval & vbTab & pNumRecs
    retval = retval & vbTab & pNumBytesRec
    retval = retval & vbCrLf
  End If
  clsATCTable_SummaryFile = retval
End Function

Private Property Let clsATCTable_Value(ByVal aFieldNumber As Long, ByVal newValue As String)
'  Dim FieldStart As Long
'  Dim I As Long
'  Dim strRet As String
'  Dim lenStr As Long
'
'  If pNumBytesRec = 0 Then InitData
'
'  On Error GoTo ErrHand
'  If pCurrentRecord < 1 Then
'    'Value = "Invalid Current Record Number"
'  ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
'    'Value = "Invalid Field Number"
'  Else
'    pData(pCurrentRecordStart) = 32 'clear record deleted flag or overwrite EOF
'
'    FieldStart = pCurrentRecordStart + pFields(aFieldNumber).DataAddress
'
'    strRet = newValue
'    lenStr = Len(strRet)
'    If lenStr > pFields(aFieldNumber).FieldLength Then
'      strRet = Left(strRet, pFields(aFieldNumber).FieldLength)
'    ElseIf pFields(aFieldNumber).FieldType = "N" Then
'      strRet = Space(pFields(aFieldNumber).FieldLength - lenStr) & strRet
'    Else
'      strRet = strRet & Space(pFields(aFieldNumber).FieldLength - lenStr)
'    End If
'    For I = 0 To pFields(aFieldNumber).FieldLength - 1
'      pData(FieldStart + I) = Asc(Mid(strRet, I + 1, 1))
'    Next
'  End If
'  Exit Property
'ErrHand:
'  LogMsg "Cannot set field #" & aFieldNumber & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value"
End Property

Private Property Get clsATCTable_Value(ByVal aFieldNumber As Long) As String
  If pCurrentRecord < 1 Or pCurrentRecord > pNumRecs Then
    clsATCTable_Value = "Invalid Current Record Number"
  ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
    clsATCTable_Value = "Invalid Field Number"
  Else
    Dim FieldStart As Long
    Dim i As Long
    Dim strRet As String
    
    FieldStart = pCurrentRecordStart + pFields(aFieldNumber).DataAddress - 1
        
    strRet = ""
    For i = 0 To pFields(aFieldNumber).FieldLength - 1
      If pData(FieldStart + i) > 0 Then
        strRet = strRet & Chr(pData(FieldStart + i))
      Else
        i = 256
      End If
    Next
    clsATCTable_Value = Trim(strRet)
'    If pFields(aFieldNumber).FieldType = "N" Then
'      Dim dblval As Double
'      dblval = CDbl(strRet)
'      If pFields(aFieldNumber).DecimalCount <> 0 Then
'        dblval = dblval * 10 ^ pFields(aFieldNumber).DecimalCount
'      End If
'      Value = dblval
'    End If
  End If
End Property

Private Sub clsATCTable_WriteFile(ByVal Filename As String)
'  Dim OutFile As Integer
'  Dim I As Integer, j As Integer, dot As Integer
'  Dim S As String
'TryAgain:
'  On Error GoTo ErrHand
'
'  If FileExists(Filename) Then
'    Kill Filename
'  Else
'    MkDirPath PathNameOnly(Filename)
'  End If
'
'  OutFile = FreeFile(0)
'  Open Filename For Binary As OutFile
'  Put #OutFile, , pHeader
'
'  For I = 1 To pNumFields
'    pFields(I).DataAddress = 0 'Nobody else seems to leave non-zero values in there
'    Put #OutFile, (32 * I) + 1, pFields(I)
'  Next I
'
'  SetDataAddresses 'Set them again since we zeroed them as we wrote
'
'  'If we have over-allocated for adding more records, trim unused records
'  If pNumRecsCapacity > pNumRecs Then
'    pNumRecsCapacity = pNumRecs
'    ReDim Preserve pData(pNumRecs * pNumBytesRec)
'  End If
'
'  Put #OutFile, , pData
'  Close #OutFile
'
'  pFilename = Filename
'
'Exit Sub
'
'ErrHand:
'  Resume Next
'  Log "Error saving " & Filename & vbCr & Err.Description
'  If MsgBox("Error saving " & Filename & vbCr & Err.Description, vbAbortRetryIgnore, "Write DBF") = vbRetry Then
'    On Error Resume Next
'    Close #OutFile
'    GoTo TryAgain
'  End If
End Sub


