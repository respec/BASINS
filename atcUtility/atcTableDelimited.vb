Option Strict Off
Option Explicit On
Imports System.IO
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' Read and write delimited text files as a table
''' </summary>
''' <remarks>
''' Default field delimiter is a comma
''' First row is read as field names
''' </remarks>
Public Class atcTableDelimited
    Inherits atcTable

    Private pFilename As String
    Private pFieldNames() As String
    Private pFieldLengths() As Integer
    Private pHeader As New ArrayList
    Private pNumFields As Integer
    Private pNumHeaderRows As Integer = -1
    Private pCurrentRowValues() As String
    Private pRecords() As String
    Private pNumRecords As Integer
    Private pCurrentRecord As Integer
    Private pCurrentRecordStart As Integer
    Private pDelimiter As Char = Chr(44) 'default to Chr(44) = comma

    Public Overrides Property CurrentRecord() As Integer
        Get
            CurrentRecord = pCurrentRecord
        End Get
        Set(ByVal newValue As Integer)
            Try
                If newValue < 1 Or newValue > pNumRecords Then
                    pCurrentRecord = 1
                Else
                    pCurrentRecord = newValue
                End If
                'parse fields values from this record
                'TODO: test whether prepending pDelimiter slows this down, could change usage of pCurrentRowValues to (index-1) instead
                pCurrentRowValues = (pDelimiter & pRecords(pCurrentRecord)).Split(pDelimiter)
                If pNumFields > pCurrentRowValues.GetUpperBound(0) Then
                    ReDim Preserve pCurrentRowValues(pNumFields)
                End If

                Exit Property
            Catch ex As Exception
                Throw New ApplicationException("ATCTableFixed: Cannot set CurrentRecord to " & newValue & vbCr & ex.Message)
            End Try
        End Set
    End Property

    Public Overrides Property FieldLength(ByVal aFieldNumber As Integer) As Integer
        Get
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                Return pFieldLengths(aFieldNumber)
            Else
                Return 0
            End If
        End Get
        Set(ByVal newValue As Integer)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFieldLengths(aFieldNumber) = newValue
            End If
        End Set
    End Property

    Public Overrides Property FieldName(ByVal aFieldNumber As Integer) As String
        Get
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                Return pFieldNames(aFieldNumber - 1)
            Else
                Return "Field " & aFieldNumber
            End If
        End Get
        Set(ByVal newValue As String)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFieldNames(aFieldNumber - 1) = newValue
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
            ReDim pFieldNames(pNumFields)
            ReDim pFieldLengths(pNumFields)
            For iField = 1 To pNumFields
                pFieldNames(iField) = ""
                pFieldLengths(iField) = 0
            Next
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
            If aHeaderRow < 1 Or aHeaderRow > pHeader.Count Then
                Return "Invalid Current Record Number"
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
            NumRecords = pNumRecords
        End Get
        Set(ByVal newValue As Integer)
            If newValue > pNumRecords Then
                pNumRecords = newValue
                'Expand the record array capacity
                ReDim Preserve pRecords(pNumRecords)
            ElseIf newValue < pNumRecords Then
                'Shrink the records array
                pNumRecords = newValue
                ReDim Preserve pRecords(pNumRecords)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If pCurrentRecord < 1 Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " < 1")
            ElseIf pCurrentRecord > pNumRecords Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " > " & pNumRecords)
            ElseIf aFieldNumber < 1 Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " < 1")
            ElseIf aFieldNumber > pNumFields Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " > " & pNumFields)
            Else
                Return pCurrentRowValues(aFieldNumber)
            End If
        End Get
        Set(ByVal newValue As String)
            On Error GoTo ErrHand
            If pCurrentRecord < 1 Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " < 1")
            ElseIf aFieldNumber < 1 Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " < 1")
            ElseIf aFieldNumber > pNumFields Then
                Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " > " & pNumFields)
            Else
                pCurrentRowValues(aFieldNumber) = newValue
                pRecords(pCurrentRecord) = CurrentRecordAsDelimitedString(pDelimiter)
            End If
            Exit Property
ErrHand:
            Logger.Msg("Cannot set field #" & aFieldNumber & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & Err.Description, "Let Value")
        End Set
    End Property

    Public Overrides Sub Clear()
        ClearData()
        pHeader.Clear()
        NumFields = 0
    End Sub

    Public Overrides Sub ClearData()
        ReDim pRecords(0)
    End Sub

    Public Overrides Function Cousin() As IatcTable
        Dim iField As Short
        Dim newFixed As New atcTableFixed
        With newFixed
            .NumFields = pNumFields

            For iField = 1 To pNumFields
                .FieldName(iField) = FieldName(iField)
                .FieldLength(iField) = FieldLength(iField)
            Next
        End With
        Return newFixed
    End Function

    'Returns zero if the named field does not appear in this file
    Public Overrides Function FieldNumber(ByVal aFieldName As String) As Integer
        Dim lField As Integer
        For lField = 1 To pNumFields
            If pFieldNames(lField) = aFieldName Then
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
            For iRec = 1 To NumHeaderRows 'read header rows, ignore for now
                pHeader.Add(NextLine(inReader))
            Next

            curLine = NextLine(inReader)
            NumFields = CountString(curLine, pDelimiter) + 1
            pFieldNames = (pDelimiter & curLine).Split(pDelimiter)

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
TryAgain:
        Try
            Dim lPath As String = System.IO.Path.GetDirectoryName(Filename)
            If lPath.Length > 0 And Not FileExists(lPath, True) Then
                MkDirPath(lPath)
            End If

            Dim lOutStream As StreamWriter = File.CreateText(Filename)

            lOutStream.Write(Header)

            For i As Integer = 1 To pNumFields - 1
                lOutStream.Write(pFieldNames(i) & pDelimiter)
            Next
            lOutStream.Write(pFieldNames(pNumFields) & vbCrLf)

            For i As Integer = 1 To pNumRecords
                lOutStream.Write(pRecords(i) & vbCrLf)
            Next

            lOutStream.Close()

            pFilename = Filename

            Return True
        Catch ex As Exception
            If Logger.Msg("Error saving " & Filename & vbCr & Err.Description, _
                          MsgBoxStyle.AbortRetryIgnore, "Write File") = MsgBoxResult.Retry Then
                GoTo TryAgain
            End If
            Return False
        End Try
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
