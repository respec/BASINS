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

    Private pFilename As String
    Private pFields() As clsFieldDescriptor
    Private pHeader As New ArrayList
    Private pNumFields As Integer
    Private pNumHeaderRows As Integer = -1
    Private pData() As String
    Private pRecords As New ArrayList
    Private pCurrentRecord As Integer
    Private pCurrentRecordStart As Integer

    Public Overrides Property CurrentRecord() As Integer
        Get
            Return pCurrentRecord + 1
        End Get
        Set(ByVal newValue As Integer)
            Try
                If newValue < 1 Or newValue > pRecords.Count Then
                    pCurrentRecord = 0
                Else
                    pCurrentRecord = newValue - 1
                End If
                'parse fields values from this record
                For i As Integer = 1 To pNumFields
                    pData(i) = Mid(pRecords(pCurrentRecord), pFields(i).FieldStart, pFields(i).FieldLength)
                Next
                Exit Property
            Catch ex As Exception
                Throw New ApplicationException("ATCTableFixed: Cannot set CurrentRecord to " & newValue & vbCr & ex.Message)
            End Try
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
        Set(ByVal newValue As Integer)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFields(aFieldNumber).FieldLength = newValue
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
        Set(ByVal newValue As String)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFields(aFieldNumber).FieldName = newValue
            End If
        End Set
    End Property

    ''' <summary>
    ''' Character position where the field starts
    ''' </summary>
    ''' <param name="aFieldNumber">Number of field (one to NumFields)</param>
    ''' <value></value>
    ''' <returns>Character position >= 1</returns>
    ''' <remarks>Returns zero if field does not exist</remarks>
    Public Property FieldStart(ByVal aFieldNumber As Integer) As Integer
        Get
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                Return pFields(aFieldNumber).FieldStart
            Else
                Return 0
            End If
        End Get
        Set(ByVal newValue As Integer)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFields(aFieldNumber).FieldStart = newValue
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
    '  Set(ByVal newValue As String)
    '    If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
    '      pFields(aFieldNumber).FieldType = newValue
    '    End If
    '  End Set
    'End Property

    Public Overrides Property NumFields() As Integer
        Get
            NumFields = pNumFields
        End Get
        Set(ByVal newValue As Integer)
            Dim iField As Integer
            pNumFields = newValue
            ReDim pFields(pNumFields)
            For iField = 1 To pNumFields
                pFields(iField) = New clsFieldDescriptor
            Next
            ReDim pData(NumFields)
        End Set
    End Property

    Public Property NumHeaderRows() As Integer
        Get
            If pNumHeaderRows >= 0 Then
                Return pNumHeaderRows
            Else
                Return pHeader.Count
            End If
        End Get
        Set(ByVal newValue As Integer)
            pNumHeaderRows = newValue
            'TODO: should anything happen here?
        End Set
    End Property

    ''' <summary>
    ''' Get a specified row of the header
    ''' </summary>
    ''' <param name="aHeaderRow">Which row to get (range is 1..NumHeaderRows)</param>
    ''' <returns>text of specified row of the header</returns>
    Public Property Header(ByVal aHeaderRow As Integer) As String
        Get
            If aHeaderRow < 1 Then
                Return "Header Row " & aHeaderRow & " is less than one"
            ElseIf aHeaderRow > pHeader.Count Then
                Return "Header Row " & aHeaderRow & " is greater than number of rows (" & pHeader.Count & ")"
            Else
                Return pHeader(aHeaderRow - 1)
            End If
        End Get
        Set(ByVal newValue As String)
            On Error GoTo ErrHand
            If aHeaderRow < 1 Or aHeaderRow > pHeader.Count Then
                'Value = "Invalid Field Number"
            Else
                pHeader(aHeaderRow - 1) = newValue
            End If
            Exit Property
ErrHand:
            Logger.Msg("Cannot set header record #" & aHeaderRow & ".  Record number must be between 1 and " & pHeader.Count & "." & vbCr & Err.Description, "Header")
        End Set
    End Property

    ''' <summary>
    ''' All rows of the header concatenated with cr/lf at the end of each line
    ''' </summary>
    Public Property Header() As String
        Get
            Header = ""
            For Each lString As String In pHeader
                Header &= lString & vbCrLf
            Next
        End Get
        Set(ByVal newValue As String)
            pHeader.Clear()
            pHeader.AddRange(newValue.Replace(vbCrLf, vbCr).Replace(vbLf, vbCr).Split(vbCr))
        End Set
    End Property

    Public Overrides Property NumRecords() As Integer
        Get
            Return pRecords.Count
        End Get
        Set(ByVal newValue As Integer)
            If newValue > pRecords.Count Then
                Dim lRecordWidth As Integer = 0
                For i As Integer = 1 To pNumFields
                    lRecordWidth += pFields(i).FieldLength
                Next
                Dim lBlankRecord As String = Space(lRecordWidth)
                While newValue > pRecords.Count
                    pRecords.Add(lBlankRecord)
                End While
            ElseIf newValue < pRecords.Count Then
                pRecords.RemoveRange(newValue, pRecords.Count - newValue)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If pCurrentRecord < 0 Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord + 1 & " < 1")
            ElseIf pCurrentRecord >= pRecords.Count Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord + 1 & " > " & pRecords.Count)
            ElseIf aFieldNumber < 1 Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " < 1")
            ElseIf aFieldNumber > pNumFields Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " > " & pNumFields)
            Else
                Return pData(aFieldNumber)
            End If
        End Get
        Set(ByVal newValue As String)
            On Error GoTo ErrHand
            If pCurrentRecord < 0 Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord + 1 & " < 1")
            ElseIf pCurrentRecord >= pRecords.Count Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord + 1 & " > " & pRecords.Count)
            ElseIf aFieldNumber < 1 Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " < 1")
            ElseIf aFieldNumber > pNumFields Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " > " & pNumFields)
            Else
                If newValue.Length < pFields(aFieldNumber).FieldLength Then
                    newValue = newValue.PadLeft(pFields(aFieldNumber).FieldLength)
                End If
                pData(aFieldNumber) = newValue
                'TODO: test this
                pRecords(pCurrentRecord) = pRecords(pCurrentRecord).Substring(0, pFields(aFieldNumber).FieldStart) _
                                         & newValue _
                                         & pRecords(pCurrentRecord).Substring(pFields(aFieldNumber).FieldStart + pFields(aFieldNumber).FieldLength)

            End If
            Exit Property
ErrHand:
            Logger.Msg("Cannot set field #" & aFieldNumber & " = '" & newValue & "' in record #" & pCurrentRecord + 1 & vbCr & Err.Description, "Let Value")
        End Set
    End Property

    Public Overrides Sub Clear()
        pHeader.Clear()
        ClearData()
    End Sub

    Public Overrides Sub ClearData()
        If pHeader Is Nothing Then pHeader = New ArrayList
        ReDim pData(0)
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
        Try
            Dim lLineReader As IEnumerator = LinesInFile(New BinaryReader(aStream))

            For iRec As Integer = 1 To NumHeaderRows 'read header rows, ignore for now
                lLineReader.MoveNext()
                pHeader.Add(lLineReader.Current)
            Next

            pRecords = New ArrayList
            While lLineReader.MoveNext
                pRecords.Add(lLineReader.Current)
            End While
            Me.CurrentRecord = 1
            Return True
        Catch e As Exception
            Logger.Msg("Error opening table '" & pFilename & "': " & e.Message)
            Return False
        End Try
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

        FilePut(OutFile, Header)
        
        MoveFirst()
        For i = 1 To pRecords.Count
            FilePut(OutFile, CurrentRecordAsDelimitedString("") & vbCrLf)
            MoveNext()
        Next

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
        Set(ByVal newValue As String)

        End Set
    End Property

End Class
