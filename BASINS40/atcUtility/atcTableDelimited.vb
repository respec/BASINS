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
''' First row after NumHeaderRows is read as field names
''' </remarks>
Public Class atcTableDelimited
    Inherits atcTable

    Private pFilename As String
    Friend pFieldNames() As String
    Private pFieldLengths() As Integer
    Private pFieldTypes() As String
    Friend pHeader As New ArrayList
    Private pNumFields As Integer
    Private pNumHeaderRows As Integer = -1
    Private pCurrentRowValues() As String
    Friend pRecords As New ArrayList
    Private pCurrentRecord As Integer
    Private pCurrentRecordStart As Integer
    Private pDelimiter As Char = Chr(44) 'default to Chr(44) = comma

    Public Overrides Property CurrentRecord() As Integer
        Get
            CurrentRecord = pCurrentRecord
        End Get
        Set(ByVal newValue As Integer)
            Try
                If newValue < 1 Then
                    pCurrentRecord = 1
                Else
                    pCurrentRecord = newValue
                End If
                If newValue > NumRecords Then
                    NumRecords = newValue
                End If
                'parse fields values from this record
                'TODO: test whether prepending pDelimiter slows this down, could change usage of pCurrentRowValues to (index-1) instead
                pCurrentRowValues = (pDelimiter & pRecords(pCurrentRecord)).Split(pDelimiter)
                If pNumFields > pCurrentRowValues.GetUpperBound(0) Then
                    ReDim Preserve pCurrentRowValues(pNumFields)
                End If

                Exit Property
            Catch ex As Exception
                Throw New ApplicationException("atcTableDelimited: Cannot set CurrentRecord to " & newValue & vbCr & ex.Message)
            End Try
        End Set
    End Property

    Public Property Delimiter() As Char
        Get
            Return pDelimiter
        End Get
        Set(ByVal newValue As Char)
            pDelimiter = newValue
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
                Return pFieldNames(aFieldNumber)
            Else
                Return "Field " & aFieldNumber
            End If
        End Get
        Set(ByVal newValue As String)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFieldNames(aFieldNumber) = newValue
            End If
        End Set
    End Property

    Public Overrides Property NumFields() As Integer
        Get
            NumFields = pNumFields
        End Get
        Set(ByVal newValue As Integer)
            Dim iField As Integer
            pNumFields = newValue
            ReDim pFieldNames(pNumFields)
            ReDim pFieldLengths(pNumFields)
            ReDim pFieldTypes(pNumFields)
            For iField = 1 To pNumFields
                pFieldNames(iField) = ""
                pFieldLengths(iField) = 0
                pFieldTypes(iField) = ""
            Next
        End Set
    End Property

    Public Overrides Property NumHeaderRows() As Integer
        Get
            If pNumHeaderRows >= 0 Then
                Return pNumHeaderRows
            Else
                Return pHeader.Count
            End If
        End Get
        Set(ByVal newValue As Integer)
            pNumHeaderRows = newValue
            While pHeader.Count < newValue
                pHeader.Add("") 'dummy header
            End While
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
                Return "Header row " & aHeaderRow & " outside available range (1 to " & pHeader.Count & ")"
            Else
                Return pHeader(aHeaderRow - 1)
            End If
        End Get
        Set(ByVal newValue As String)
            If aHeaderRow < 1 Or aHeaderRow > pHeader.Count Then
                Throw New ApplicationException("Cannot set header row " & aHeaderRow & ".  Row must be between 1 and " & pHeader.Count & "." & vbCr & Err.Description)
            Else
                pHeader(aHeaderRow - 1) = newValue
            End If
        End Set
    End Property

    ''' <summary>
    ''' All rows of the header concatenated with cr/lf at the end of each line
    ''' </summary>
    Public Property Header() As String
        Get
            Dim lReturnValue As String = ""
            For Each lString As String In pHeader
                lReturnValue &= lString & vbCrLf
            Next
            Return lReturnValue
        End Get
        Set(ByVal newValue As String)
            pHeader.Clear()
            pHeader.AddRange(newValue.Replace(vbCrLf, vbCr).Replace(vbLf, vbCr).Split(vbCr))
        End Set
    End Property

    Public Overrides Property NumRecords() As Integer
        Get
            NumRecords = pRecords.Count - 1
        End Get
        Set(ByVal newValue As Integer)
            If newValue >= pRecords.Count Then
                Dim lEmptyRecord As String = Space(NumFields - 1).Replace(" ", Delimiter)
                While newValue >= pRecords.Count
                    pRecords.Add(lEmptyRecord)
                End While
            ElseIf newValue < pRecords.Count Then
                pRecords.RemoveRange(newValue + 1, pRecords.Count - newValue)
            End If
        End Set
    End Property

    Public Overrides Property Value(ByVal aFieldNumber As Integer) As String
        Get
            If pCurrentRecord < 1 Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " < 1")
            ElseIf pCurrentRecord > NumRecords Then
                Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " > " & NumRecords)
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
        pRecords.Clear()
    End Sub

    Public Overrides Function Cousin() As IatcTable
        Dim iField As Short
        Dim lCousin As New atcTableDelimited
        With lCousin
            .NumFields = pNumFields
            .Delimiter = pDelimiter

            For iField = 1 To pNumFields
                .FieldName(iField) = FieldName(iField)
                .FieldLength(iField) = FieldLength(iField)
            Next
        End With
        Return lCousin
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
    Public Overridable Function OpenStream(ByVal aStream As Stream) As Boolean
        Dim inReader As New BinaryReader(aStream)
        Dim lFinishedHeader As Boolean = False
        Dim lRecordCount As Integer = 0
        pRecords = New ArrayList(100)
        pRecords.Add("Unused record 0")
        For Each lCurrentLine As String In LinesInFile(inReader)
            lRecordCount += 1
            If lRecordCount <= NumHeaderRows Then
                pHeader.Add(lCurrentLine)
            ElseIf lRecordCount = NumHeaderRows + 1 Then
                NumFields = CountString(lCurrentLine, Delimiter) + 1
                'Split creates a zero-based array. Prepending pDelimiter inserts blank field name so pFieldNames(1) contains first name
                'TODO: are quoted ("") - is this correct?
                pFieldNames = (Delimiter & lCurrentLine).Split(Delimiter)
            Else
                pRecords.Add(lCurrentLine)
            End If
        Next
        Me.CurrentRecord = 1
        Return True
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

    Public Overrides Function WriteFile(ByVal aFilename As String) As Boolean
TryAgain:
        Try
            Dim lDirectory As String = System.IO.Path.GetDirectoryName(aFilename)
            If lDirectory.Length > 0 AndAlso Not IO.Directory.Exists(lDirectory) Then
                IO.Directory.CreateDirectory(lDirectory)
            End If
            Dim lOutStream As StreamWriter = File.CreateText(aFilename)
            lOutStream.Write(ToString)
            lOutStream.Close()
            FileName = aFilename
            Return True
        Catch ex As Exception
            If Logger.Msg("Error saving " & aFilename & vbCr & Err.Description, _
                          MsgBoxStyle.AbortRetryIgnore, "Write File") = MsgBoxResult.Retry Then
                GoTo TryAgain
            End If
            Return False
        End Try
    End Function

    Public Overrides Function ToString() As String
        Dim lSB As New Text.StringBuilder
        lSB.Append(Header)
        lSB.Append(String.Join(Delimiter, pFieldNames, 1, NumFields) & vbCrLf)
        For Each lRecord As String In pRecords
            lSB.AppendLine(lRecord.TrimEnd)
        Next
        Return lSB.ToString
    End Function

    Public Function ToStringPivoted() As String
        Dim lSB As New Text.StringBuilder
        lSB.Append(Header)

        lSB.Append(pFieldNames(1).Trim.PadLeft(8))
        Dim lFirstHeader As Boolean = True
        For lRow As Integer = 2 To NumRecords
            CurrentRecord = lRow
            Dim lConsName As String = Value(1)
            If lConsName.Trim.Length > 1 Then
                If lConsName.StartsWith(" ") Then
                    If lFirstHeader Then
                        lFirstHeader = False
                    Else
                        lSB.Append(Delimiter & Space(FieldLength(2)))
                    End If
                Else
                    lSB.Append(Delimiter & lConsName.Trim.PadLeft(FieldLength(2)))
                    lFirstHeader = True
                End If
            End If
        Next
        lSB.AppendLine()

        lSB.Append(Space(8))
        For lRow As Integer = 2 To NumRecords
            CurrentRecord = lRow
            Dim lConsName As String = Value(1)
            If lConsName.Trim.Length > 0 AndAlso lConsName.StartsWith(" ") Then
                lConsName = lConsName.Trim
                If lConsName.Length > FieldLength(2) Then
                    lConsName = lConsName.Substring(0, FieldLength(2))
                End If
                lSB.Append(Delimiter & lConsName.PadLeft(FieldLength(2)))
            End If
        Next
        lSB.AppendLine()

        Dim lColumnIndex As Integer = 1
        For Each lFieldName As String In pFieldNames
            If Not lFieldName Is Nothing AndAlso lFieldName.Length > 0 Then
                If lColumnIndex > 1 Then
                    lSB.Append(lFieldName.Trim.PadLeft(8))
                    For lRow As Integer = 1 To NumRecords
                        CurrentRecord = lRow
                        Dim lConsName As String = Value(1)
                        If lConsName.Trim.Length > 0 AndAlso lConsName.StartsWith(" ") Then
                            If Value(lColumnIndex).Length > 0 Then
                                lSB.Append(Delimiter & Value(lColumnIndex))
                            Else
                                lSB.Append(Delimiter & Space(FieldLength(lColumnIndex)))
                            End If
                        End If
                    Next
                    lSB.AppendLine()
                End If
                lColumnIndex += 1
            End If
        Next
        Return lSB.ToString
    End Function

    Public Overrides Function CreationCode() As String
        Return ("") 'TODO: write valid CreationCode
    End Function

    'on set translate to match DBF: s = string into C = Character, n = numeric into N = Numeric
    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                FieldType = pFieldTypes(aFieldNumber)
            Else
                FieldType = "Undefined"
            End If
        End Get
        Set(ByVal newValue As String)
            If aFieldNumber > 0 And aFieldNumber <= pNumFields Then
                pFieldTypes(aFieldNumber) = newValue
            End If
        End Set
    End Property

End Class
