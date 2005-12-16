Option Strict Off
Option Explicit On 
Imports System.IO
Public Class atcTableFixed
  Inherits atcTable

  Private Class clsFieldDescriptor
    Public FieldName As String    'name of field
    Public FieldLength As Integer 'length of field
    Public FieldStart As Integer  'column number in which field starts

    'Public Sub ReadFromFile(ByVal inFile As Short)
    '  Dim buf As Byte() : ReDim buf(11)
    '  FileGet(inFile, buf) 'Get field name plus type character
    '  FieldName = System.Text.ASCIIEncoding.ASCII.GetString(buf, 0, 11)
    '  FieldType = Chr(buf(11))
    '  FileGet(inFile, DataAddress)
    '  FileGet(inFile, FieldLength)
    '  FileGet(inFile, DecimalCount)
    '  FileGet(inFile, Trash)
    'End Sub

    'Public Sub WriteToFile(ByVal outFile As Short)
    '  Dim buf As Byte() : ReDim buf(11)
    '  buf = System.Text.ASCIIEncoding.ASCII.GetBytes(FieldName)
    '  If buf.Length() <> 12 Then ReDim Preserve buf(11)
    '  buf(10) = 0
    '  buf(11) = Asc(FieldType)
    '  FilePut(outFile, buf)
    '  'FilePut(outFile, DataAddress)
    '  FilePut(outFile, CInt(0)) 'DataAddress = 0 'Nobody seems to leave non-zero values in file
    '  FilePut(outFile, FieldLength)
    '  FilePut(outFile, DecimalCount)
    '  FilePut(outFile, Trash)
    'End Sub
  End Class

  Private pFilename As String
  Private pFields() As clsFieldDescriptor
  Private pNumFields As Integer
  Private pNumHeaderRows As Integer
  Private pData() As String
  Private pRecords() As String
  Private pNumRecords As Integer
  Private pCurrentRecord As Integer
  Private pCurrentRecordStart As Integer

  Public Overrides Property CurrentRecord() As Integer
    Get
      CurrentRecord = pCurrentRecord
    End Get
    Set(ByVal Value As Integer)
      On Error GoTo ErrHand
      'If Value > pHeader.NumRecs Then NumRecords = Value
      If Value < 1 Or Value > pNumRecords Then
        pCurrentRecord = 1
      Else
        pCurrentRecord = Value
      End If
      'parse fields values from this record
      For i As Integer = 1 To pNumFields
        pData(i) = Mid(pRecords(pCurrentRecord), pFields(i).FieldStart, pFields(i).FieldLength)
      Next
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

  Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldName = pFields(aFieldNumber).FieldName
      Else
        FieldName = "Undefined"
      End If
    End Get
    Set(ByVal Value As String)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        pFields(aFieldNumber).FieldName = Value
      End If
    End Set
  End Property

  Public Property FieldStart(ByVal aFieldNumber As Integer) As Integer
    Get
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        FieldStart = pFields(aFieldNumber).FieldStart
      Else
        FieldStart = 0
      End If
    End Get
    Set(ByVal Value As Integer)
      If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
        pFields(aFieldNumber).FieldStart = Value
      End If
    End Set
  End Property


  ''C = Character, D = Date, N = Numeric, L = Logical, M = Memo
  'Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
  '  Get
  '    If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
  '      FieldType = pFields(aFieldNumber).FieldType
  '    Else
  '      FieldType = "Undefined"
  '    End If
  '  End Get
  '  Set(ByVal Value As String)
  '    If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
  '      pFields(aFieldNumber).FieldType = Value
  '    End If
  '  End Set
  'End Property

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
      Next
      ReDim pData(NumFields)
    End Set
  End Property

  Public Property NumHeaderRows() As Integer
    Get
      NumHeaderRows = pNumHeaderRows
    End Get
    Set(ByVal Value As Integer)
      pNumHeaderRows = Value
    End Set
  End Property

  Public Overrides Property NumRecords() As Integer
    Get
      NumRecords = pNumRecords
    End Get
    Set(ByVal Value As Integer)
      Dim iBlank As Integer
      If Value > pNumRecords Then
        pNumRecords = Value
        'Expand the record array capacity
        ReDim Preserve pRecords(pNumRecords)
      ElseIf Value < pNumRecords Then
        'Shrink the records array
        pNumRecords = Value
        ReDim Preserve pRecords(pNumRecords)
      End If
    End Set
  End Property

  Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
    Get
      If pCurrentRecord < 1 Or pCurrentRecord > pNumRecords Then
        Value = "Invalid Current Record Number"
      ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
        Value = "Invalid Field Number"
      Else
        Value = Trim(pData(aFieldNumber))
      End If
    End Get
    Set(ByVal Value As String)
      On Error GoTo ErrHand
      If pCurrentRecord < 1 Then
        'Value = "Invalid Current Record Number"
      ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
        'Value = "Invalid Field Number"
      Else
        pData(aFieldNumber) = Value
      End If
      Exit Property
ErrHand:
      LogMsg("Cannot set field #" & aFieldNumber & " = '" & Value & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value")
    End Set
  End Property

  'Public Function FindMatch(ByRef aFieldNum() As Integer, ByRef aOperator() As String, ByRef aFieldVal() As Object, Optional ByVal aMatchAny As Boolean = False, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
  '  Dim numRules As Integer
  '  Dim iRule As Integer
  '  Dim Value As Object
  '  Dim allMatch As Boolean
  '  Dim thisMatches As Boolean
  '  Dim NotAtTheEnd As Boolean
  '  numRules = UBound(aFieldNum)

  '  If aEndRecord < 0 Then aEndRecord = pHeader.NumRecs

  '  'If we are supposed to look for matches only in records that don't exist, we won't find any
  '  If aStartRecord > pHeader.NumRecs Then
  '    FindMatch = False
  '    Exit Function
  '  End If

  '  CurrentRecord = aStartRecord
  '  NotAtTheEnd = True
  '  While NotAtTheEnd And CurrentRecord <= aEndRecord
  '    iRule = 1
  '    allMatch = True
  '    While iRule <= numRules And allMatch
  '      thisMatches = False
  '      Value = Value(aFieldNum(iRule))
  '      Select Case aOperator(iRule)
  '        Case "="
  '          If Value = aFieldVal(iRule) Then thisMatches = True
  '        Case "<"
  '          If Value < aFieldVal(iRule) Then thisMatches = True
  '        Case ">"
  '          If Value > aFieldVal(iRule) Then thisMatches = True
  '        Case "<="
  '          If Value <= aFieldVal(iRule) Then thisMatches = True
  '        Case ">="
  '          If Value >= aFieldVal(iRule) Then thisMatches = True
  '        Case Else : System.Diagnostics.Debug.WriteLine("Unrecognized operator:" & aOperator(iRule))
  '      End Select
  '      If aMatchAny Then
  '        If thisMatches Then
  '          FindMatch = True
  '          Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  '        End If
  '      Else
  '        If Not thisMatches Then
  '          allMatch = False
  '        End If
  '      End If
  '      iRule = iRule + 1
  '    End While
  '    If allMatch And Not aMatchAny Then
  '      FindMatch = True
  '      Exit Function '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  '    End If
  '    If pCurrentRecord < pHeader.NumRecs Then
  '      MoveNext()
  '    Else
  '      NotAtTheEnd = False
  '    End If
  '  End While
  '  CurrentRecord = aStartRecord
  '  FindMatch = False
  'End Function

  ''Dimension and initialize data buffer to all spaces (except for initial carriage return)
  ''Do not call on an existing DBF since all data will be removed from memory
  ''If creating a new DBF:
  '' Call after setting NumRecords, NumFields and all FieldLength
  '' Call before setting any Value
  'Public Sub InitData()
  '  Dim I As Integer

  '  SetDataAddresses()

  '  pHeader.NumBytesRec = pFields(pNumFields).DataAddress + pFields(pNumFields).FieldLength

  '  pNumRecsCapacity = pHeader.NumRecs
  '  pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
  '  ReDim pData(pDataBytes)
  '  pData(0) = 13
  '  For I = 1 To pDataBytes
  '    pData(I) = 32
  '  Next
  'End Sub

  'Private Sub SetDataAddresses()
  '  Dim I As Integer
  '  pFields(1).DataAddress = 1
  '  For I = 2 To pNumFields
  '    pFields(I).DataAddress = pFields(I - 1).DataAddress + pFields(I - 1).FieldLength
  '  Next
  'End Sub

  Public Overrides Sub Clear()
    'ClearData()
    'pHeader.version = 3
    'pHeader.dbfDay = 1
    'pHeader.dbfMonth = 1
    'pHeader.dbfYear = 70
    'pHeader.NumBytesHeader = 32
    'pHeader.NumBytesRec = 0
    'pNumFields = 0
    'ReDim pFields(0)
  End Sub

  Public Overrides Sub ClearData()
    'If pHeader Is Nothing Then pHeader = New clsHeader
    'pHeader.NumRecs = 0
    'pDataBytes = 0
    'pCurrentRecord = 1
    'pCurrentRecordStart = 0
    'pNumRecsCapacity = 0
    'ReDim pData(0)
  End Sub

  Public Overrides Function Cousin() As IatcTable
    Dim iField As Short
    Dim newFixed As New atcTableFixed
    With newFixed
      .NumFields = pNumFields

      For iField = 1 To pNumFields
        .FieldName(iField) = FieldName(iField)
        .FieldLength(iField) = FieldLength(iField)
        .FieldStart(iField) = FieldStart(iField)
      Next
    End With
    Return newFixed
  End Function

  'Returns zero if the named field does not appear in this file
  Public Overrides Function FieldNumber(ByVal aFieldName As String) As Integer
    Dim retval As Integer
    For retval = 1 To pNumFields
      If pFields(retval).FieldName = aFieldName Then
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
    Dim iRec As Integer
    Dim curLine As String

    If Not FileExists(Filename) Then
      Return False 'can't open a file that doesn't exist
    End If

    pFilename = Filename
    Dim inStream As New FileStream(pFilename, FileMode.Open, FileAccess.Read)
    Dim inBuffer As New BufferedStream(inStream)
    Dim inReader As New BinaryReader(inBuffer)

    Try
      For iRec = 1 To pNumHeaderRows 'read header rows, ignore for now
        curLine = NextLine(inReader)
      Next

      ReDim pRecords(100) 'initial record buffer size
      iRec = 1
      Do
        curLine = NextLine(inReader)
        If iRec > pRecords.GetUpperBound(0) Then
          ReDim Preserve pRecords(iRec * 2)
        End If
        pRecords(iRec) = curLine
        iRec += 1
      Loop
    Catch endEx As EndOfStreamException
      ReDim Preserve pRecords(iRec - 1)
      pNumRecords = iRec - 1
      Me.CurrentRecord = 1
      Return True
    End Try
    Return False
  End Function

  'Public Overrides Function SummaryFields(Optional ByRef aFormat As String = "tab,headers,expandtype") As String
  '  Dim retval As String
  '  Dim iTrash As Short
  '  Dim iField As Short
  '  Dim ShowTrash As Boolean
  '  Dim ShowHeaders As Boolean
  '  Dim ExpandType As Boolean

  '  If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
  '  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
  '  If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True

  '  If InStr(LCase(aFormat), "text") > 0 Then 'text version
  '    For iField = 1 To pNumFields
  '      With pFields(iField)
  '        retval = retval & vbCrLf & "Field " & iField & ": '" & TrimNull(.FieldName) & "'"
  '        retval = retval & vbCrLf & "    Type: " & .FieldType & " "
  '        If ExpandType Then
  '          Select Case .FieldType
  '            Case "C" : retval = retval & "Character"
  '            Case "D" : retval = retval & "Date     "
  '            Case "N" : retval = retval & "Numeric  "
  '            Case "L" : retval = retval & "Logical  "
  '            Case "M" : retval = retval & "Memo     "
  '          End Select
  '        Else
  '          retval = retval & .FieldType
  '        End If
  '        retval = retval & vbCrLf & "    Length: " & .FieldLength & " "
  '        retval = retval & vbCrLf & "    DecimalCount: " & .DecimalCount & " "
  '        If ShowTrash Then
  '          retval = retval & vbCrLf & "    Trash: "
  '          For iTrash = 1 To 14
  '            retval = retval & .Trash(iTrash) & " "
  '          Next
  '        End If
  '      End With
  '      retval = retval & vbCrLf
  '    Next
  '  Else 'table version
  '    If ShowHeaders Then
  '      retval = retval & "Field "
  '      retval = retval & vbTab & "Name "
  '      retval = retval & vbTab & "Type "
  '      retval = retval & vbTab & "Length "
  '      retval = retval & vbTab & "DecimalCount "
  '      If ShowTrash Then
  '        For iTrash = 1 To 14
  '          retval = retval & vbTab & "Trash" & iTrash
  '        Next
  '      End If
  '    End If
  '    retval = retval & vbCrLf
  '    'now field details
  '    For iField = 1 To pNumFields
  '      With pFields(iField)
  '        retval = retval & iField & vbTab & "'" & TrimNull(.FieldName) & "' "
  '        If ExpandType Then
  '          Select Case .FieldType
  '            Case "C" : retval = retval & vbTab & "Character"
  '            Case "D" : retval = retval & vbTab & "Date     "
  '            Case "N" : retval = retval & vbTab & "Numeric  "
  '            Case "L" : retval = retval & vbTab & "Logical  "
  '            Case "M" : retval = retval & vbTab & "Memo     "
  '          End Select
  '        Else
  '          retval = retval & vbTab & .FieldType
  '        End If
  '        retval = retval & vbTab & .FieldLength
  '        retval = retval & vbTab & .DecimalCount
  '        If ShowTrash Then
  '          retval = retval & vbCrLf & "    Trash: "
  '          For iTrash = 1 To 14
  '            retval = retval & vbTab & .Trash(iTrash)
  '          Next
  '        End If
  '      End With
  '      retval = retval & vbCrLf
  '    Next
  '  End If
  '  Return retval
  'End Function

  'Public Overrides Function SummaryFile(Optional ByRef aFormat As String = "tab,headers") As String
  '  Dim retval As String
  '  Dim iTrash As Short
  '  Dim ShowTrash As Boolean
  '  Dim ShowHeaders As Boolean

  '  If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
  '  If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True

  '  If LCase(aFormat) = "text" Then 'text version
  '    With pHeader
  '      retval = "DBF Header: "
  '      retval = retval & vbCrLf & "    FileName: " & pFilename
  '      retval = retval & vbCrLf & "    Version: " & .version
  '      retval = retval & vbCrLf & "    Date: " & .dbfYear + 1900 & "/" & .dbfMonth & "/" & .dbfDay
  '      retval = retval & vbCrLf & "    NumRecs: " & .NumRecs
  '      retval = retval & vbCrLf & "    NumBytesHeader: " & .NumBytesHeader
  '      retval = retval & vbCrLf & "    NumBytesRec: " & .NumBytesRec
  '      If ShowTrash Then
  '        retval = retval & vbCrLf & "    Trash: "
  '        For iTrash = 1 To 20
  '          retval = retval & pHeader.Trash(iTrash) & " "
  '        Next
  '      End If
  '    End With
  '  Else 'table version
  '    'build header header
  '    If ShowHeaders Then
  '      retval = "FileName "
  '      retval = retval & vbTab & "Version "
  '      retval = retval & vbTab & "Date "
  '      retval = retval & vbTab & "NumFields "
  '      retval = retval & vbTab & "NumRecs "
  '      retval = retval & vbTab & "NumBytesHeader "
  '      retval = retval & vbTab & "NumBytesRec "
  '    End If
  '    If ShowTrash Then
  '      For iTrash = 0 To 19
  '        retval = retval & vbTab & "Trash" & iTrash
  '      Next
  '    End If
  '    retval = retval & vbCrLf
  '    With pHeader 'now header data
  '      retval = retval & pFilename
  '      retval = retval & vbTab & .version
  '      retval = retval & vbTab & .dbfYear + 1900 & "/" & .dbfMonth & "/" & .dbfDay
  '      retval = retval & vbTab & pNumFields
  '      retval = retval & vbTab & .NumRecs
  '      retval = retval & vbTab & .NumBytesHeader
  '      retval = retval & vbTab & .NumBytesRec
  '      If ShowTrash Then
  '        For iTrash = 0 To 19
  '          retval = retval & vbTab & pHeader.Trash(iTrash)
  '        Next
  '      End If
  '      retval = retval & vbCrLf
  '    End With
  '  End If
  '  SummaryFile = retval
  'End Function

  '  Public Overrides Function WriteFile(ByVal Filename As String) As Boolean
  '    Dim OutFile As Short
  '    Dim j, I, dot As Short
  '    Dim s As String
  'TryAgain:
  '    On Error GoTo ErrHand

  '    If FileExists(Filename) Then
  '      Kill(Filename)
  '    Else
  '      MkDirPath(System.IO.Path.GetDirectoryName(Filename))
  '    End If

  '    OutFile = FreeFile()
  '    FileOpen(OutFile, Filename, OpenMode.Binary)
  '    pHeader.WriteToFile(OutFile)

  '    For I = 1 To pNumFields
  '      pFields(I).WriteToFile(OutFile) 'FilePutObject(OutFile, pFields(I), (32 * I) + 1)
  '    Next I

  '    'If we have over-allocated for adding more records, trim unused records
  '    If pNumRecsCapacity > pHeader.NumRecs Then
  '      pNumRecsCapacity = pHeader.NumRecs
  '      ReDim Preserve pData(pHeader.NumRecs * pHeader.NumBytesRec)
  '    End If

  '    FilePut(OutFile, pData)
  '    FileClose(OutFile)

  '    pFilename = Filename

  '    Return True

  'ErrHand:
  '    Resume Next
  '    If LogMsg("Error saving " & Filename & vbCr & Err.Description, "Write DBF", "Retry", "Abort") = 1 Then
  '      On Error Resume Next
  '      FileClose(OutFile)
  '      GoTo TryAgain
  '    End If
  '    Return False
  '  End Function

  'Reads the next line from a text file whose lines end with carriage return and/or linefeed
  'Advances the position of the stream to the beginning of the next line
  'Returns Nothing if already at end of file
  Private Function NextLine(ByVal aReader As BinaryReader) As String
    Dim ch As Char
    Try
      NextLine = Nothing
ReadCharacter:
      ch = aReader.ReadChar
      Select Case ch
        Case vbCr 'Found end of line, consume linefeed if it is next
          If CInt(aReader.PeekChar) = CInt(10) Then aReader.ReadChar()
        Case vbLf 'Unix-style line ends without carriage return
        Case Else 'Found a character that does not end the line
          If NextLine Is Nothing Then
            NextLine = ch
          Else
            NextLine &= ch
          End If
          GoTo ReadCharacter
      End Select
    Catch endEx As EndOfStreamException
      If NextLine Is Nothing Then 'We had nothing to read, already finished file last time
        Throw endEx
      Else
        'Reaching the end of file is fine, we have finished reading this file
      End If
    End Try

  End Function

  Public Overrides Function CreationCode() As String

  End Function

  Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
    Get

    End Get
    Set(ByVal Value As String)

    End Set
  End Property

  Public Overrides Function WriteFile(ByVal filename As String) As Boolean

  End Function
End Class
