Option Strict Off
Option Explicit On 
Public Class atcTableDBF
  Inherits atcTable

  'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1016"'
  'Private Declare Sub CopyMemory Lib "Kernel32"  Alias "RtlMoveMemory"(ByRef Destination As Any, ByRef Source As Any, ByVal Length As Integer)
  'Private Declare Function ArrPtr Lib "msvbvm60.dll"  Alias "VarPtr"(ByRef ptr() As Any) As Integer

  'Variables for comparing bytes of a record as longs
  'Private pLongHeader1(5) As Integer
  'Private pLongHeader2(5) As Integer
  'Private pCompareLongs1() As Integer
  'Private pCompareLongs2() As Integer

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

  'Converted to VB.NET by Mark.Gray@pobox.com October 2004

  'dBaseIII file header, 32 bytes
  Private Class clsHeader
    Public version As Byte
    Public dbfYear As Byte
    Public dbfMonth As Byte
    Public dbfDay As Byte
    Public NumRecs As Integer
    Public NumBytesHeader As Short
    Public NumBytesRec As Short
    Public Trash(19) As Byte

    Public Sub ReadFromFile(ByVal inFile As Short)
      FileGet(inFile, version)
      FileGet(inFile, dbfYear)
      FileGet(inFile, dbfMonth)
      FileGet(inFile, dbfDay)
      FileGet(inFile, NumRecs)
      FileGet(inFile, NumBytesHeader)
      FileGet(inFile, NumBytesRec)
      FileGet(inFile, Trash)
    End Sub

    Public Sub WriteToFile(ByVal outFile As Short)
      FilePut(outFile, version)
      FilePut(outFile, dbfYear)
      FilePut(outFile, dbfMonth)
      FilePut(outFile, dbfDay)
      FilePut(outFile, NumRecs)
      FilePut(outFile, NumBytesHeader)
      FilePut(outFile, NumBytesRec)
      FilePut(outFile, Trash)
    End Sub
  End Class


  'Field Descriptions, 32 bytes * Number of Fields
  'Up to 128 Fields
  Private Class clsFieldDescriptor
    Public FieldName As String    '10 character limit, 11th char is 0 in DBF file
    Public FieldType As String    'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
    Public DataAddress As Integer 'offset from record start to field start
    Public FieldLength As Byte    'Byte type limits field size in DBF to 255 bytes
    Public DecimalCount As Byte   'Hint for number of decimal places to display
    Public Trash(13) As Byte      'Extra bytes in field descriptor that are not used

    Public Sub ReadFromFile(ByVal inFile As Short)
      Dim buf As Byte() : ReDim buf(11)
      FileGet(inFile, buf) 'Get field name plus type character
      FieldName = System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, 11)
      FieldType = Chr(buf(11))
      FileGet(inFile, DataAddress)
      FileGet(inFile, FieldLength)
      FileGet(inFile, DecimalCount)
      FileGet(inFile, Trash)
    End Sub

    Public Sub WriteToFile(ByVal outFile As Short)
      Dim buf As Byte() : ReDim buf(11)
      buf = System.Text.ASCIIEncoding.ASCII.GetBytes(FieldName)
      If buf.Length() <> 12 Then ReDim Preserve buf(11)
      buf(10) = 0
      buf(11) = Asc(FieldType)
      FilePut(outFile, buf)
      'FilePut(outFile, DataAddress)
      FilePut(outFile, CInt(0)) 'DataAddress = 0 'Nobody seems to leave non-zero values in file
      FilePut(outFile, FieldLength)
      FilePut(outFile, DecimalCount)
      FilePut(outFile, Trash)
    End Sub
  End Class

  Private pFilename As String
  Private pHeader As clsHeader
  Private pFields() As clsFieldDescriptor
  Private pNumFields As Integer
  Private pData() As Byte
  Private pDataBytes As Integer
  Private pCurrentRecord As Integer
  Private pCurrentRecordStart As Integer

  'Capacity in pData for records. Set to pHeader.NumRecs when data is read from a file
  'and in InitData when creating a new DBF from scratch. May increase in Let Value.
  Private pNumRecsCapacity As Integer

  Public Property Year() As Byte
    Get
      Return pHeader.dbfYear
    End Get
    Set(ByVal newValue As Byte)
      pHeader.dbfYear = newValue
    End Set
  End Property

  Public Property Month() As Byte
    Get
      Return pHeader.dbfMonth
    End Get
    Set(ByVal newValue As Byte)
      pHeader.dbfMonth = newValue
    End Set
  End Property

  Public Property Day() As Byte
    Get
      Return pHeader.dbfDay
    End Get
    Set(ByVal newValue As Byte)
      pHeader.dbfDay = newValue
    End Set
  End Property

  Public Property FieldDecimalCount(ByVal aFieldNumber As Integer) As Byte
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldDecimalCount = pFields(aFieldNumber).DecimalCount
      Else
        FieldDecimalCount = 0
      End If
    End Get
    Set(ByVal Value As Byte)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        pFields(aFieldNumber).DecimalCount = Value
      End If
    End Set
  End Property

  'Public Property record() As Byte()
  '	Get
  '		'Dim i As Long
  '		Dim retval() As Byte
  '		ReDim retval(pHeader.NumBytesRec - 1)
  '		CopyMemory(retval(0), pData(pCurrentRecordStart), pHeader.NumBytesRec)
  '		'For i = 0 To pHeader.NumBytesRec - 1
  '		'  retval(i) = pData(pCurrentRecordStart + i)
  '		'Next
  '		record = VB6.CopyArray(retval)
  '	End Get
  '	Set(ByVal Value() As Byte)
  '		'  Dim i As Long
  '		'  For i = 0 To pHeader.NumBytesRec - 1
  '		'    pData(pCurrentRecordStart + i) = newValue(i)
  '		'  Next
  '		If UBound(newValue) = pHeader.NumBytesRec - 1 Then
  '			CopyMemory(pData(pCurrentRecordStart), Value(0), pHeader.NumBytesRec)
  '		Else
  '			LogMsg("Cannot Let record - wrong size newValue passed" & vbCr & "new record is " & UBound(newValue) + 1 & " bytes long" & vbCr & "but should be " & pHeader.NumBytesRec & " bytes long" & vbCr & Err.Description, "Let record")
  '		End If
  '	End Set
  'End Property

  'Merge records from dbf2Add into this dbf
  'keyFieldNames are field names in the DBFs that define a unique field.
  'If keyFieldNames is blank, no duplicate checking will occur
  'If keyFieldNames(1) = "**ALL**" then the entire record will be used as a key
  'DuplicateAction dictates handling of duplicate records as follows:
  ' 0 - duplicates allowed
  ' 1 - keep existing instance of duplicates and discard duplicates from dbf being added
  ' 2 - replace existing instance of duplicates with duplicates from dbf being added
  ' 3 - ask user what to do (not currently implemented)
  'Public Sub Merge(dbf2Add As atcTable, keyFieldNames() As String, DuplicateAction As Long)
  '  Dim addRecordNum As Long
  '  Dim fieldNum As Long
  '  Dim keyField() As Long
  '  Dim operator() As String
  '  Dim keyValue() As Variant
  '  Dim recordToCopy() As Byte
  '  Dim firstKeyField As Long
  '  Dim lastKeyField As Long
  '  Dim lMsg As String
  '  Dim LastOldRec As Long
  '  Dim AllFieldsKey As Boolean
  '  Dim foundDuplicate As Boolean
  '  Dim canCopyRecords As Boolean
  '
  '  Log "Merge " & dbf2Add.Filename & " into " & Filename
  '
  '  If dbf2Add.NumRecords > 0 And pNumFields <> dbf2Add.NumFields And pHeader.NumRecs < 1 Then
  '    'Replace our field definitions with the new ones since we have no data
  '    NumFields = dbf2Add.NumFields
  '    For fieldNum = 1 To pNumFields
  '      FieldName(fieldNum) = dbf2Add.FieldName(fieldNum)
  '      FieldType(fieldNum) = dbf2Add.FieldType(fieldNum)
  '      FieldLength(fieldNum) = dbf2Add.FieldLength(fieldNum)
  ''FIXME      FieldDecimalCount(fieldNum) = dbf2Add.FieldDecimalCount(fieldNum)
  '    Next
  '    NumRecords = 0
  '    Me.InitData
  '  End If
  '
  '  If dbf2Add.NumRecords < 1 Then
  '    Log "No records to add from empty DBF:" & vbCr & dbf2Add.Filename
  '  ElseIf pNumFields <> dbf2Add.NumFields Then
  '    LogMsg "Different number of fields:" & vbCr _
  ''          & pFilename & " = " & pNumFields & vbCr _
  ''          & dbf2Add.Filename & " = " & dbf2Add.NumFields & vbCr & vbCr _
  ''          & "Cannot merge DBF files", "Merge"
  '  Else
  '    For fieldNum = 1 To pNumFields
  '      If UCase(Trim(FieldName(fieldNum))) <> UCase(Trim(dbf2Add.FieldName(fieldNum))) Then
  '        If Not LogMsg("Field '" & FieldName(fieldNum) & "' does not appear to match '" _
  ''               & dbf2Add.FieldName(fieldNum) & "'" & vbCr _
  ''               & "Proceed with merge anyway, treating these fields as matching?", "Merge", True) Then
  '          Exit Sub
  '        End If
  '      End If
  '    Next
  '    If DuplicateAction > 0 Then
  '      firstKeyField = LBound(keyFieldNames)
  '      lastKeyField = UBound(keyFieldNames)
  'RedimKeys:
  '      ReDim keyField(firstKeyField To lastKeyField)
  '      ReDim operator(firstKeyField To lastKeyField)
  '      ReDim keyValue(firstKeyField To lastKeyField)
  '      For fieldNum = firstKeyField To lastKeyField
  '        If AllFieldsKey Then
  '          keyField(fieldNum) = fieldNum
  '          operator(fieldNum) = "="
  '        Else
  '          If keyFieldNames(fieldNum) = "**ALL**" Then
  '            AllFieldsKey = True
  '            firstKeyField = 1
  '            lastKeyField = pNumFields
  '            GoTo RedimKeys
  '          Else
  '            lMsg = lMsg & keyFieldNames(fieldNum) & ", "
  '            keyField(fieldNum) = FieldNumber(keyFieldNames(fieldNum))
  '            operator(fieldNum) = "="
  '          End If
  '        End If
  '      Next fieldNum
  '      If AllFieldsKey Then
  '        lMsg = "All fields must match to find a duplicate."
  '      Else
  '        If Len(lMsg) > 2 Then lMsg = " Looking for duplicate records in fields " & Left(lMsg, Len(lMsg) - 2)
  '      End If
  '
  '      If Len(lMsg) > 0 Then
  '        Select Case DuplicateAction
  '          Case 0: Log lMsg & " Not checking for duplicates"
  '          Case 2: Log lMsg & " Overwriting existing with new duplicates"
  '          Case Else: Log lMsg & " Keeping existing; discarding new duplicates"
  '        End Select
  '        lMsg = ""
  '      End If
  '    End If
  '
  '    LastOldRec = pHeader.NumRecs 'Don't search for duplicates in newly added records
  '    If LastOldRec < 1 Then DuplicateAction = 0 'Don't bother checking for duplicates since we start empty
  '    dbf2Add.CurrentRecord = 1
  '    recordToCopy = dbf2Add.record
  '
  '    If UBound(recordToCopy) + 1 = pHeader.NumBytesRec Then
  '      canCopyRecords = True
  '    Else
  ''      LogMsg "Different number of bytes per record:" & vbCr _
  '''            & Filename & " = " & pHeader.NumBytesRec & vbCr _
  '''            & dbf2Add.Filename & " = " & UBound(recordToCopy) + 1 & vbCr & vbCr _
  '''            & "Cannot merge DBF files", "Merge"
  ''      Exit Sub
  '      canCopyRecords = False
  '      Log "Different number of bytes per record:" & vbCr _
  ''            & Filename & " = " & pHeader.NumBytesRec & vbCr _
  ''            & dbf2Add.Filename & " = " & UBound(recordToCopy) + 1 & vbCr _
  ''            & "Attempting to copy fields instead of records"
  '    End If
  '
  ''    Dim starttime As Date
  ''    starttime = Now
  '    With dbf2Add
  '      For addRecordNum = 1 To .NumRecords
  ''        If (addRecordNum \ 100) * 100 = addRecordNum Then
  ''          Debug.Print "Adding " & addRecordNum & " at " & Format(Now - starttime, "h:mm:ss") & " Matches: " & CountAgreeMatch & " NoMatches: " & CountAgreeNoMatch
  ''        End If
  '        .CurrentRecord = addRecordNum
  '        If DuplicateAction = 0 Then
  '          'don't bother looking for a duplicate since we add them all anyway
  '        ElseIf AllFieldsKey And canCopyRecords Then
  '          'First check current record to see if it matches
  '          If Me.MatchRecord(.record) Then
  '            foundDuplicate = True
  '          ElseIf pCurrentRecord < LastOldRec Then
  '            'Check next record before searching hard for a match
  '            'if trying to merge same data, next record will always be the one that matches
  '            MoveNext
  '            If Me.MatchRecord(.record) Then
  '              foundDuplicate = True
  '            Else
  '              foundDuplicate = FindRecord(.record, 1, LastOldRec)
  '            End If
  '          Else
  '            foundDuplicate = FindRecord(.record, 1, LastOldRec)
  '          End If
  '        Else
  '          For fieldNum = firstKeyField To lastKeyField
  '            keyValue(fieldNum) = .Value(keyField(fieldNum))
  '          Next
  '          foundDuplicate = FindMatch(keyField, operator, keyValue, False, 1, LastOldRec)
  '        End If
  '        If foundDuplicate Then
  '          If DuplicateAction = 2 Then GoSub CopyRecord 'overwrite existing record with new record
  '        Else  'Copy this record in the DBF
  '          CurrentRecord = Me.NumRecords + 1
  '          GoSub CopyRecord
  '        End If
  '      Next
  '    End With
  '  End If
  '
  '  Exit Sub
  'CopyRecord:
  '  If canCopyRecords Then
  '    Me.record = dbf2Add.record
  '  Else
  '    For fieldNum = 1 To pNumFields
  '      Me.Value(fieldNum) = dbf2Add.Value(fieldNum)
  '    Next
  '  End If
  '  Return
  'End Sub

  Public Overrides Property CurrentRecord() As Integer
    Get
      CurrentRecord = pCurrentRecord
    End Get
    Set(ByVal Value As Integer)
      On Error GoTo ErrHand
      If Value > pHeader.NumRecs Then NumRecords = Value
      If Value < 1 Or Value > pHeader.NumRecs Then
        pCurrentRecord = 1
      Else
        pCurrentRecord = Value
      End If
      pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1
      Exit Property
ErrHand:
      LogMsg("Cannot set CurrentRecord to " & Value & vbCr & Err.Description, "Let CurrentRecord")
    End Set
  End Property


  Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldLength = pFields(aFieldNumber).FieldLength
      Else
        FieldLength = 0
      End If
    End Get
    Set(ByVal Value As Integer)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        pFields(aFieldNumber).FieldLength = Value
      End If
    End Set
  End Property

  'FieldName is a maximum of 10 characters long, padded to 11 characters with nulls

  Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldName = TrimNull(pFields(aFieldNumber).FieldName)
      Else
        FieldName = "Undefined"
      End If
    End Get
    Set(ByVal Value As String)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        Value = Trim(Left(Value, 10))
        pFields(aFieldNumber).FieldName = Value & New String(Chr(0), 11 - Len(Value))
      End If
    End Set
  End Property


  'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
  Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldType = pFields(aFieldNumber).FieldType
      Else
        FieldType = "Undefined"
      End If
    End Get
    Set(ByVal Value As String)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        pFields(aFieldNumber).FieldType = Value
      End If
    End Set
  End Property

  Public Overrides Property NumFields() As Integer
    Get
      NumFields = pNumFields
    End Get
    Set(ByVal Value As Integer)
      Dim iField As Integer
      pNumFields = Value
      ReDim pFields(pNumFields)
      For iField = 1 To pNumFields
        pFields(iField) = New clsFieldDescriptor
        pFields(iField).FieldType = "C"
      Next
      pHeader.NumBytesHeader = (pNumFields + 1) * 32 + 1
    End Set
  End Property


  Public Overrides Property NumRecords() As Integer
    Get
      NumRecords = pHeader.NumRecs
    End Get
    Set(ByVal Value As Integer)
      Dim iBlank As Integer
      If Value > pHeader.NumRecs Then
        pHeader.NumRecs = Value
        iBlank = pDataBytes + 1
        If Value > pNumRecsCapacity Then
          'Expand the data array capacity
          pNumRecsCapacity = (Value + 1) * 1.5
          ReDim Preserve pData(pNumRecsCapacity * pHeader.NumBytesRec)
        End If
        pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
        'fill all newly allocated bytes of data array with spaces
        While iBlank <= pDataBytes
          pData(iBlank) = 32
          iBlank = iBlank + 1
        End While
      ElseIf Value < pHeader.NumRecs Then
        'Shrink the data array
        pHeader.NumRecs = Value
        pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
        pNumRecsCapacity = Value
        ReDim Preserve pData(pDataBytes)
      End If
    End Set
  End Property


  Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
    Get
      Dim FieldStart As Integer
      Dim I As Integer
      Dim strRet As String
      If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
        Value = "Invalid Current Record Number"
      ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
        Value = "Invalid Field Number"
      Else

        FieldStart = pCurrentRecordStart + pFields(aFieldNumber).DataAddress

        strRet = ""
        For I = 0 To pFields(aFieldNumber).FieldLength - 1
          If pData(FieldStart + I) > 0 Then
            strRet = strRet & Chr(pData(FieldStart + I))
          Else
            I = 256
          End If
        Next
        Value = Trim(strRet)
        '    If pFields(aFieldNumber).FieldType = "N" Then
        '      Dim dblval As Double
        '      dblval = CDbl(strRet)
        '      If pFields(aFieldNumber).DecimalCount <> 0 Then
        '        dblval = dblval * 10 ^ pFields(aFieldNumber).DecimalCount
        '      End If
        '      Value = dblval
        '    End If
      End If
    End Get
    Set(ByVal Value As String)
      Dim FieldStart As Integer
      Dim I As Integer
      Dim strRet As String
      Dim lenStr As Integer

      If pHeader.NumBytesRec = 0 Then InitData()

      On Error GoTo ErrHand
      If pCurrentRecord < 1 Then
        'Value = "Invalid Current Record Number"
      ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
        'Value = "Invalid Field Number"
      Else
        pData(pCurrentRecordStart) = 32 'clear record deleted flag or overwrite EOF

        FieldStart = pCurrentRecordStart + pFields(aFieldNumber).DataAddress

        strRet = Value
        lenStr = Len(strRet)
        If lenStr > pFields(aFieldNumber).FieldLength Then
          strRet = Left(strRet, pFields(aFieldNumber).FieldLength)
        ElseIf pFields(aFieldNumber).FieldType = "N" Then
          strRet = Space(pFields(aFieldNumber).FieldLength - lenStr) & strRet
        Else
          strRet = strRet & Space(pFields(aFieldNumber).FieldLength - lenStr)
        End If
        For I = 0 To pFields(aFieldNumber).FieldLength - 1
          pData(FieldStart + I) = Asc(Mid(strRet, I + 1, 1))
        Next
      End If
      Exit Property
ErrHand:
      LogMsg("Cannot set field #" & aFieldNumber & " = '" & Value & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value")
    End Set
  End Property

  'public Overrides Function CurrentRecordAsDelimitedString(Optional aDelimiter As String = ",", Optional aQuote As String = "") As String
  '  Dim retval As String
  '  Dim fieldVal As String
  '  Dim usingQuotes As Boolean
  '  Dim iField As Long
  '  If Len(aQuote) > 0 Then usingQuotes = True
  '  For iField = 1 To pNumFields
  '    fieldVal = Value(iField)
  '    If usingQuotes Then
  '      If InStr(fieldVal, aDelimiter) > 0 Then fieldVal = aQuote & fieldVal & aQuote
  '    End If
  '    retval = retval & Value(iField)
  '    If iField < pNumFields Then retval = retval & aDelimiter
  '  Next
  '  CurrentRecordAsDelimitedString = retval
  'End Function

  'Returns True if found, moves CurrentRecord to first record with .Record = FindValue
  'If not found, returns False and moves CurrentRecord to aStartRecord
  '    Public Function FindRecord(ByRef FindValue() As Byte, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
  '        Dim I As Integer
  '        Dim firstByte As Integer
  '        Dim lastByte As Integer
  '        Dim lastLong As Integer
  '        Dim nLongs As Integer
  '        If aEndRecord < 1 Then aEndRecord = pHeader.NumRecs

  '        lastByte = pHeader.NumBytesRec - 1
  '        nLongs = pHeader.NumBytesRec \ 4
  '        lastLong = nLongs - 1
  '        firstByte = nLongs * 4

  '        '  Dim byt As Long
  '        '  Dim Match As Boolean
  '        '  Dim rec As Long
  '        '  For rec = aStartRecord To aEndRecord
  '        '    CurrentRecord = rec
  '        '    Match = True
  '        '    For byt = 0 To pHeader.NumBytesRec - 1
  '        '      If pData(pCurrentRecordStart + byt) <> FindValue(byt) Then
  '        '        Match = False
  '        '        Exit For
  '        '      End If
  '        '    Next
  '        '    If Match Then Exit For
  '        '  Next

  '        'CAUTION! DO NOT STOP VB after StartUsingCompareLongs until after FinishedUsingCompareLongs has been called
  '        StartUsingCompareLongs()
  '        'UPGRADE_ISSUE: VarPtr function is not supported. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1040"'
  '        pLongHeader1(3) = VarPtr(FindValue(0))
  '        pCurrentRecord = aStartRecord
  '        pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1

  'CompareCurrentRecord:
  '        I = 0
  '        'UPGRADE_ISSUE: VarPtr function is not supported.
  '        pLongHeader2(3) = VarPtr(pData(pCurrentRecordStart))
  '        For I = 0 To lastLong
  '            If pCompareLongs1(I) <> pCompareLongs2(I) Then GoTo NotEqual
  '        Next

  '        For I = firstByte To lastByte
  '            If pData(pCurrentRecordStart + I) <> FindValue(I) Then GoTo NotEqual
  '        Next

  '        FindRecord = True
  '        FinishedUsingCompareLongs()
  '        '    If Not Match Then Stop Else CountAgreeMatch = CountAgreeMatch + 1
  '        Exit Function

  'NotEqual:
  '        If pCurrentRecord < aEndRecord Then
  '            pCurrentRecord = pCurrentRecord + 1
  '            pCurrentRecordStart = pCurrentRecordStart + pHeader.NumBytesRec
  '            GoTo CompareCurrentRecord
  '        End If

  '        CurrentRecord = aStartRecord
  '        FindRecord = False
  '        FinishedUsingCompareLongs()
  '        '  If Match Then Stop Else CountAgreeNoMatch = CountAgreeNoMatch + 1
  '    End Function

  'Returns True if CurrentRecord matches FindValue
  'Public Function MatchRecord(ByRef FindValue() As Byte) As Boolean
  '    Dim byt As Integer
  '    Dim lastbyt As Integer
  '    If UBound(FindValue) < pHeader.NumBytesRec Then
  '        lastbyt = UBound(FindValue)
  '    Else
  '        lastbyt = pHeader.NumBytesRec - 1
  '    End If
  '    For byt = 0 To lastbyt
  '        If pData(pCurrentRecordStart + byt) <> FindValue(byt) Then
  '            MatchRecord = False
  '            Exit Function
  '        End If
  '    Next
  '    MatchRecord = True
  'End Function

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
  ''                            ByVal aStartRecord As Long, _
  ''                            ByVal aEndRecord As Long, _
  ''                            ParamArray aRules() As Variant) As Boolean
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

  Public Function FindMatch(ByRef aFieldNum() As Integer, ByRef aOperator() As String, ByRef aFieldVal() As Object, Optional ByVal aMatchAny As Boolean = False, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
    Dim numRules As Integer
    Dim iRule As Integer
    Dim Value As Object
    Dim allMatch As Boolean
    Dim thisMatches As Boolean
    Dim NotAtTheEnd As Boolean
    numRules = UBound(aFieldNum)

    If aEndRecord < 0 Then aEndRecord = pHeader.NumRecs

    'If we are supposed to look for matches only in records that don't exist, we won't find any
    If aStartRecord > pHeader.NumRecs Then
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
        Value = Value(aFieldNum(iRule))
        Select Case aOperator(iRule)
          Case "="
            If Value = aFieldVal(iRule) Then thisMatches = True
          Case "<"
            If Value < aFieldVal(iRule) Then thisMatches = True
          Case ">"
            If Value > aFieldVal(iRule) Then thisMatches = True
          Case "<="
            If Value <= aFieldVal(iRule) Then thisMatches = True
          Case ">="
            If Value >= aFieldVal(iRule) Then thisMatches = True
          Case Else : System.Diagnostics.Debug.WriteLine("Unrecognized operator:" & aOperator(iRule))
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
      End While
      If allMatch And Not aMatchAny Then
        FindMatch = True
        Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
      End If
      If pCurrentRecord < pHeader.NumRecs Then
        MoveNext()
      Else
        NotAtTheEnd = False
      End If
    End While
    CurrentRecord = aStartRecord
    FindMatch = False
  End Function

  'Dimension and initialize data buffer to all spaces (except for initial carriage return)
  'Do not call on an existing DBF since all data will be removed from memory
  'If creating a new DBF:
  ' Call after setting NumRecords, NumFields and all FieldLength
  ' Call before setting any Value
  Public Sub InitData()
    Dim I As Integer

    SetDataAddresses()

    pHeader.NumBytesRec = pFields(pNumFields).DataAddress + pFields(pNumFields).FieldLength

    pNumRecsCapacity = pHeader.NumRecs
    pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
    ReDim pData(pDataBytes)
    pData(0) = 13
    For I = 1 To pDataBytes
      pData(I) = 32
    Next
  End Sub

  Private Sub SetDataAddresses()
    Dim I As Integer
    pFields(1).DataAddress = 1
    For I = 2 To pNumFields
      pFields(I).DataAddress = pFields(I - 1).DataAddress + pFields(I - 1).FieldLength
    Next
  End Sub

  '     Select Case FieldDes(i).FieldType                       'Reading the dBASE Field Type
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

  Private Function TrimNull(ByRef Value As String) As String
    Dim nullPos As Integer
    nullPos = InStr(Value, Chr(0))
    If nullPos = 0 Then
      TrimNull = Trim(Value)
    Else
      TrimNull = Trim(Left(Value, nullPos - 1))
    End If
  End Function

  Public Sub New()
    MyBase.New()
    ' Set up our templates for comparing arrays
    'pLongHeader1(0) = 1 ' Number of dimensions
    'pLongHeader1(1) = 4 ' Bytes per element (long = 4)
    'pLongHeader1(4) = &H7FFFFFFF ' Array size

    'pLongHeader2(0) = 1 ' Number of dimensions
    'pLongHeader2(1) = 4 ' Bytes per element (long = 4)
    'pLongHeader2(4) = &H7FFFFFFF ' Array size

    Clear()
  End Sub

  'Private Sub StartUsingCompareLongs()
  '    ' Force pCompareLongs to use pLongHeader as its own header
  '    'UPGRADE_ISSUE: VarPtr function is not supported. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1040"'
  '    CopyMemory(ArrPtr(pCompareLongs1), VarPtr(pLongHeader1(0)), 4)
  '    'UPGRADE_ISSUE: VarPtr function is not supported
  '    CopyMemory(ArrPtr(pCompareLongs2), VarPtr(pLongHeader2(0)), 4)
  'End Sub
  'Private Sub FinishedUsingCompareLongs()
  '    ' Make pCompareLongs once again use their own headers
  '    ' If this code doesn't run the IDE will crash when this object is disposed of
  '    CopyMemory(ArrPtr(pCompareLongs1), 0, 4)
  '    CopyMemory(ArrPtr(pCompareLongs2), 0, 4)
  'End Sub

  Public Overrides Sub Clear()
    ClearData()
    pHeader.version = 3
    pHeader.dbfDay = 1
    pHeader.dbfMonth = 1
    pHeader.dbfYear = 70
    pHeader.NumBytesHeader = 32
    pHeader.NumBytesRec = 0
    pNumFields = 0
    ReDim pFields(0)
  End Sub

  Public Overrides Sub ClearData()
    If pHeader Is Nothing Then pHeader = New clsHeader
    pHeader.NumRecs = 0
    pDataBytes = 0
    pCurrentRecord = 1
    pCurrentRecordStart = 0
    pNumRecsCapacity = 0
    ReDim pData(0)
  End Sub

  Public Overrides Function Cousin() As IatcTable
    Dim iTrash As Short
    Dim iField As Short
    Dim newDBF As New atcTableDBF
    With newDBF
      .Year = CInt(Format(Now, "yyyy")) - 1900
      .Month = CByte(Format(Now, "mm"))
      .Day = CByte(Format(Now, "dd"))
      .NumFields = pNumFields

      For iField = 1 To pNumFields
        .FieldName(iField) = FieldName(iField)
        .FieldType(iField) = FieldType(iField)
        .FieldLength(iField) = FieldLength(iField)
        .FieldDecimalCount(iField) = FieldDecimalCount(iField)
      Next
    End With
    Return newDBF
  End Function

  Public Overrides Function CreationCode() As String
    Dim retval As String
    Dim iTrash As Short
    Dim iField As Short

    retval = "Dim newDBF as clsDBF"
    retval = retval & vbCrLf & "set newDBF = new clsDBF"
    retval = retval & vbCrLf & "With newDBF"

    retval = retval & vbCrLf & "  .Year = CInt(Format(Now, ""yyyy"")) - 1900"
    retval = retval & vbCrLf & "  .Month = CByte(Format(Now, ""mm""))"
    retval = retval & vbCrLf & "  .Day = CByte(Format(Now, ""dd""))"
    retval = retval & vbCrLf & "  .NumFields = " & pNumFields
    retval = retval & vbCrLf

    For iField = 1 To pNumFields
      With pFields(iField)
        retval = retval & vbCrLf & "  .FieldName(" & iField & ") = """ & TrimNull(.FieldName) & """"
        retval = retval & vbCrLf & "  .FieldType(" & iField & ") = """ & .FieldType & """"
        retval = retval & vbCrLf & "  .FieldLength(" & iField & ") = " & .FieldLength
        retval = retval & vbCrLf & "  .FieldDecimalCount(" & iField & ") = " & .DecimalCount
        retval = retval & vbCrLf
      End With
    Next
    retval = retval & vbCrLf & "  '.NumRecords = " & pHeader.NumRecs
    retval = retval & vbCrLf & "  '.InitData"
    retval = retval & vbCrLf & "End With"
    retval = retval & vbCrLf
    CreationCode = retval
  End Function

  'Returns zero if the named field does not appear in this file
  Public Overrides Function FieldNumber(ByVal aFieldName As String) As Integer
    Dim retval As Integer
    For retval = 1 To pNumFields
      If TrimNull(pFields(retval).FieldName) = aFieldName Then
        FieldNumber = retval
        Exit Function
      End If
    Next
  End Function

  Public Overrides Function OpenFile(ByVal Filename As String) As Boolean
    'Dim header As clsHeader, FieldDes As clsFieldDescriptor    'Creating variables for user-defined types
    'Dim memo As String * 512                               'Create a 512 byte fixed string variable
    ' to read memo fields
    Dim inFile As Short
    Dim I As Short

    If Not FileExists(Filename) Then
      Return False 'can't open a file that doesn't exist
    End If

    pFilename = Filename

    inFile = FreeFile()
    FileOpen(inFile, Filename, OpenMode.Binary)
    pHeader.ReadFromFile(inFile)
    Select Case pHeader.version 'Be sure we're using a dBASE III file
      Case 3 'Normal dBASEIII file
        '   Case &H83 'Open a .DBT file
      Case Else
        LogMsg("This is not a dBASE III file: '" & Filename & "'", "OpenDBF")
        FileClose(inFile)
        Return False
    End Select

    NumFields = pHeader.NumBytesHeader \ 32 - 1 'Calculate the number of fields

    For I = 1 To pNumFields
      pFields(I).ReadFromFile(inFile) 'Looping through NumFields by reading in 32 byte records
    Next I

    SetDataAddresses()

    pDataBytes = LOF(inFile) - pHeader.NumBytesHeader  'Adding one seems to help with some files
    ReDim pData(pDataBytes)
    FileGet(inFile, pData)
    pNumRecsCapacity = pHeader.NumRecs
    FileClose(inFile)
    If pHeader.NumRecs > 0 Then
      MoveFirst()
    Else
      pCurrentRecord = 0
    End If
    Return True
  End Function

  Public Overrides Function SummaryFields(Optional ByRef aFormat As String = "tab,headers,expandtype") As String
    Dim retval As String
    Dim iTrash As Short
    Dim iField As Short
    Dim ShowTrash As Boolean
    Dim ShowHeaders As Boolean
    Dim ExpandType As Boolean

    If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
    If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
    If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True

    If InStr(LCase(aFormat), "text") > 0 Then 'text version
      For iField = 1 To pNumFields
        With pFields(iField)
          retval = retval & vbCrLf & "Field " & iField & ": '" & TrimNull(.FieldName) & "'"
          retval = retval & vbCrLf & "    Type: " & .FieldType & " "
          If ExpandType Then
            Select Case .FieldType
              Case "C" : retval = retval & "Character"
              Case "D" : retval = retval & "Date     "
              Case "N" : retval = retval & "Numeric  "
              Case "L" : retval = retval & "Logical  "
              Case "M" : retval = retval & "Memo     "
            End Select
          Else
            retval = retval & .FieldType
          End If
          retval = retval & vbCrLf & "    Length: " & .FieldLength & " "
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
          retval = retval & iField & vbTab & "'" & TrimNull(.FieldName) & "' "
          If ExpandType Then
            Select Case .FieldType
              Case "C" : retval = retval & vbTab & "Character"
              Case "D" : retval = retval & vbTab & "Date     "
              Case "N" : retval = retval & vbTab & "Numeric  "
              Case "L" : retval = retval & vbTab & "Logical  "
              Case "M" : retval = retval & vbTab & "Memo     "
            End Select
          Else
            retval = retval & vbTab & .FieldType
          End If
          retval = retval & vbTab & .FieldLength
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
    Return retval
  End Function

  Public Overrides Function SummaryFile(Optional ByRef aFormat As String = "tab,headers") As String
    Dim retval As String
    Dim iTrash As Short
    Dim ShowTrash As Boolean
    Dim ShowHeaders As Boolean

    If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
    If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True

    If LCase(aFormat) = "text" Then 'text version
      With pHeader
        retval = "DBF Header: "
        retval = retval & vbCrLf & "    FileName: " & pFilename
        retval = retval & vbCrLf & "    Version: " & .version
        retval = retval & vbCrLf & "    Date: " & .dbfYear + 1900 & "/" & .dbfMonth & "/" & .dbfDay
        retval = retval & vbCrLf & "    NumRecs: " & .NumRecs
        retval = retval & vbCrLf & "    NumBytesHeader: " & .NumBytesHeader
        retval = retval & vbCrLf & "    NumBytesRec: " & .NumBytesRec
        If ShowTrash Then
          retval = retval & vbCrLf & "    Trash: "
          For iTrash = 1 To 20
            retval = retval & pHeader.Trash(iTrash) & " "
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
        For iTrash = 0 To 19
          retval = retval & vbTab & "Trash" & iTrash
        Next
      End If
      retval = retval & vbCrLf
      With pHeader 'now header data
        retval = retval & pFilename
        retval = retval & vbTab & .version
        retval = retval & vbTab & .dbfYear + 1900 & "/" & .dbfMonth & "/" & .dbfDay
        retval = retval & vbTab & pNumFields
        retval = retval & vbTab & .NumRecs
        retval = retval & vbTab & .NumBytesHeader
        retval = retval & vbTab & .NumBytesRec
        If ShowTrash Then
          For iTrash = 0 To 19
            retval = retval & vbTab & pHeader.Trash(iTrash)
          Next
        End If
        retval = retval & vbCrLf
      End With
    End If
    SummaryFile = retval
  End Function

  Public Overrides Function WriteFile(ByVal Filename As String) As Boolean
    Dim OutFile As Short
    Dim j, I, dot As Short
    Dim s As String
TryAgain:
    On Error GoTo ErrHand

    If FileExists(Filename) Then
      Kill(Filename)
    Else
      MkDirPath(System.IO.Path.GetDirectoryName(Filename))
    End If

    OutFile = FreeFile()
    FileOpen(OutFile, Filename, OpenMode.Binary)
    pHeader.WriteToFile(OutFile)

    For I = 1 To pNumFields
      pFields(I).WriteToFile(OutFile) 'FilePutObject(OutFile, pFields(I), (32 * I) + 1)
    Next I

    'If we have over-allocated for adding more records, trim unused records
    If pNumRecsCapacity > pHeader.NumRecs Then
      pNumRecsCapacity = pHeader.NumRecs
      ReDim Preserve pData(pHeader.NumRecs * pHeader.NumBytesRec)
    End If

    FilePut(OutFile, pData)
    FileClose(OutFile)

    pFilename = Filename

    Return True

ErrHand:
    Resume Next
    If LogMsg("Error saving " & Filename & vbCr & Err.Description, "Write DBF", "Retry", "Abort") = 1 Then
      On Error Resume Next
      FileClose(OutFile)
      GoTo TryAgain
    End If
    Return False
  End Function
End Class
