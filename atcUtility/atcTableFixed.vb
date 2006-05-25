Option Strict Off
Option Explicit On
Imports System.IO
Imports atcUtility
Imports MapWinUtility

Public Class atcTableFixed
    Inherits atcTable

    Private Class clsFieldDescriptor
        Public FieldName As String    'name of field
        Public FieldLength As Integer 'length of field
        Public FieldStart As Integer  'column number in which field starts

    End Class

    Private Class clsHeader
        Public Recs() As String
        Public Count As Integer

        Public Sub WriteToFile(ByVal outFile As Short)
            For i As Integer = 1 To Count
                FilePut(outFile, Recs(i))
            Next
        End Sub
    End Class

    Private pFilename As String
    Private pFields() As clsFieldDescriptor
    Private pHeaders As clsHeader
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
            Logger.Msg("Cannot set CurrentRecord to " & Value & vbCr & Err.Description, "Let CurrentRecord")
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
            NumHeaderRows = pHeaders.Count
        End Get
        Set(ByVal Value As Integer)
            pHeaders.Count = Value
            ReDim pHeaders.Recs(pHeaders.Count)
        End Set
    End Property

    Public Property Header(ByVal aRec As Integer) As String
        Get
            If aRec < 1 Or aRec > pHeaders.Count Then
                Header = "Invalid Current Record Number"
            Else
                Header = pHeaders.Recs(aRec)
            End If
        End Get
        Set(ByVal Value As String)
            On Error GoTo ErrHand
            If aRec < 1 Or aRec > pHeaders.Count Then
                'Value = "Invalid Field Number"
            Else
                pHeaders.Recs(aRec) = Value
            End If
            Exit Property
ErrHand:
            Logger.Msg("Cannot set header record #" & aRec & ".  Record number must be between 1 and " & pHeaders.Count & "." & vbCr & Err.Description, "Let Value")
        End Set
    End Property

    Public Overrides Property NumRecords() As Integer
        Get
            NumRecords = pNumRecords
        End Get
        Set(ByVal Value As Integer)
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
                Value = pData(aFieldNumber)
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
            Logger.Msg("Cannot set field #" & aFieldNumber & " = '" & Value & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value")
        End Set
    End Property

    'Public Function FindMatch(ByVal aFieldNum() As Integer, ByVal aOperation() As String, ByVal aFieldVal() As Object, Optional ByVal aMatchAny As Boolean = False, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
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
    '      Select Case aoperation(iRule)
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
    '        Case Else : System.Diagnostics.Debug.WriteLine("Unrecognized operation:" & aoperation(iRule))
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
        Dim lField As Integer
        For lField = 1 To pNumFields
            If pFields(lField).FieldName = aFieldName Then
                Return lField
            End If
        Next
        Return 0
    End Function

    'Read a stream into the table
    Public Function OpenStream(ByVal aStream As Stream) As Boolean
        Dim iRec As Integer
        Dim curLine As String
        Dim inReader As New BinaryReader(aStream)

        Try
            For iRec = 1 To pHeaders.Count 'read header rows, ignore for now
                pHeaders.Recs(iRec) = NextLine(inReader)
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

    'Read a string into the table
    Public Function OpenString(ByVal aString As String) As Boolean
        pFilename = ""
        Dim encoding As New System.Text.ASCIIEncoding
        Dim inBuffer As New MemoryStream(encoding.GetBytes(aString))
        OpenString = OpenStream(inBuffer)
        encoding = Nothing
        inBuffer = Nothing
    End Function

    'Read a file into the table
    Public Overrides Function OpenFile(ByVal Filename As String) As Boolean
        If Not FileExists(Filename) Then
            Return False 'can't open a file that doesn't exist
        End If

        pFilename = Filename
        Dim inStream As New FileStream(pFilename, FileMode.Open, FileAccess.Read)
        Dim inBuffer As New BufferedStream(inStream)
        Return OpenStream(inBuffer)
    End Function

    Public Overrides Function WriteFile(ByVal Filename As String) As Boolean
        Dim OutFile As Short
        Dim j, i, dot As Short
        Dim s As String
TryAgain:
        On Error GoTo ErrHand

        If FileExists(Filename) Then
            Kill(Filename)
        Else
            MkDirPath(System.IO.Path.GetDirectoryName(Filename))
        End If

        OutFile = FreeFile()
        FileOpen(OutFile, Filename, OpenMode.Input)
        pHeaders.WriteToFile(OutFile)

        MoveFirst()
        For i = 1 To pNumRecords
            s = CurrentRecordAsDelimitedString("") & vbCrLf
            FilePut(OutFile, s)
            MoveNext()
        Next i

        FileClose(OutFile)

        pFilename = Filename

        Return True

ErrHand:
        Resume Next
        If Logger.Msg("Error saving " & Filename & vbCr & Err.Description, _
                      MsgBoxStyle.AbortRetryIgnore, "Write File") = MsgBoxResult.Retry Then
            On Error Resume Next
            FileClose(OutFile)
            GoTo TryAgain
        End If
        Return False
    End Function

    Public Overrides Function CreationCode() As String
        Return ("")
    End Function

    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get
            Return ("")
        End Get
        Set(ByVal Value As String)

        End Set
    End Property

    Public Sub New()
        pHeaders = New clsHeader
    End Sub

    'rewrites the current record with the values in pData
    Public Sub Update()
        pRecords(pCurrentRecord) = CurrentRecordAsDelimitedString("") & vbCrLf
    End Sub

End Class
