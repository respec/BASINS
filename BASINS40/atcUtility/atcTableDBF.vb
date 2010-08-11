Option Strict Off
Option Explicit On
Imports atcUtility
Imports MapWinUtility

Public Class atcTableDBF
    Inherits atcTable

    '===========================================================================
    ' Subject: READ DBASE III                    Date: 1/25/88 (00:00)
    ' Author:  David Perry                       Code: QB, PDS
    ' Keys:    READ,DBASE,III                  Packet: MISC.ABC
    '===========================================================================

    'This QB source was adjusted for use with VB by Robert Smith
    'on June 14, 1999, source was provided to Smith by Marc Hoogerwerf
    'contact Smith via: www.smithvoice.com/vbfun.htm

    'This code was turned into a class by Mark Gray at Aqua Terra March 14, 2001
    'modification and extensions continue through 2003

    'Converted to VB.NET by Mark Gray at Aqua Terra October 2004
    'Maintainence continues in 2008

    'dBaseIII file header, 32 bytes
    Private Class clsHeader
        Public version As Byte
        Private dbfYear As Byte
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

        Public Sub Clear()
            Dim lDate As Date = Now
            version = 3
            dbfDay = lDate.Day
            dbfMonth = lDate.Month
            Year = lDate.Year
            NumBytesHeader = 32
            NumBytesRec = 0
        End Sub

        Public Property Year() As Integer
            Get
                Return dbfYear + 1900
            End Get
            Set(ByVal value As Integer)
                dbfYear = value - 1900
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim lTrashString As String = ""
            For Each lTrash As Byte In Trash
                lTrashString &= lTrash & " "
            Next
            Return "Version " & version _
            & " " & Year & "/" & dbfMonth & "/" & dbfDay _
            & " NumRecs=" & NumRecs _
            & " NumBytesHeader=" & NumBytesHeader _
            & " NumBytesRec=" & NumBytesRec _
            & " Trash=" & lTrashString
        End Function
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

        Public Overrides Function ToString() As String
            Dim lTrashString As String = ""
            For Each lTrash As Byte In Trash
                lTrashString &= lTrash & " "
            Next
            Return FieldName & " " & FieldType & " FieldLength=" & FieldLength & " DecimalCount=" & DecimalCount & " Trash=" & lTrashString
        End Function
    End Class

    Private pHeader As clsHeader
    Private pFields() As clsFieldDescriptor
    Private pNumFields As Integer
    Private pData() As Byte
    Private pDataBytes As Integer
    Private pCurrentRecordStart As Integer

    'Capacity in pData for records. Set to pHeader.NumRecs when data is read from a file
    'and in InitData when creating a new DBF from scratch. May increase in Let Value.
    Private pNumRecsCapacity As Integer

    Public Property Year() As Integer
        Get
            Return pHeader.Year
        End Get
        Set(ByVal newValue As Integer)
            pHeader.Year = newValue
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

    ''' <summary>
    ''' Merge records from dbf2Add into this dbf
    ''' </summary>
    ''' <param name="aAddFrom"></param>
    ''' <param name="aKeyFieldNames">Names of fields that together define a unique field. 
    ''' If blank, no duplicate checking will occur.
    ''' If Nothing or = "**ALL**" then the entire record will be used as a key</param>
    ''' <param name="aDuplicateAction">dictates handling of duplicate records as follows:
    ''' 0 - do not check for duplicates, just add all new records
    ''' 1 - keep existing instance and discard duplicates from dbf being added
    ''' 2 - replace existing instance with duplicates from dbf being added</param>
    ''' <param name="aAddedIndexes">Indexes from aAddFrom of items merged are added to aAddedIndexes if provided</param>
    ''' <remarks></remarks>
    Public Sub Merge(ByVal aAddFrom As atcTableDBF, _
                     ByVal aKeyFieldNames() As String, _
                     ByVal aDuplicateAction As Integer, _
            Optional ByVal aAddedIndexes As ArrayList = Nothing)
        Dim lAddIndexes As Boolean = Not (aAddedIndexes Is Nothing)
        Dim lAddRecordNum As Integer
        Dim lFieldNum As Integer
        Dim lKeyField(-1) As Integer
        Dim lOperation(-1) As String
        Dim lKeyValue(-1) As Object
        Dim lRecordToCopy() As Byte
        Dim lFirstKeyField As Integer
        Dim lLastKeyField As Integer
        Dim lMsg As String = ""
        Dim lLastOldRec As Integer
        Dim lAllFieldsKey As Boolean
        Dim lFoundDuplicate As Boolean
        Dim lCanCopyRecords As Boolean = True
        Dim lNumRecsAtStart As Integer = pHeader.NumRecs

        Logger.Dbg("Merge " & aAddFrom.FileName & " into " & FileName)

        If aAddFrom.NumRecords < 1 Then
            Logger.Dbg("No records to add from empty DBF:" & vbCr & aAddFrom.FileName)
        Else
            If pNumFields <> aAddFrom.NumFields And pHeader.NumRecs < 1 Then
                Logger.Dbg("Replacing field definitions with the new ones since we have no records")
                NumFields = aAddFrom.NumFields
                For lFieldNum = 1 To pNumFields
                    FieldName(lFieldNum) = aAddFrom.FieldName(lFieldNum)
                    FieldType(lFieldNum) = aAddFrom.FieldType(lFieldNum)
                    FieldLength(lFieldNum) = aAddFrom.FieldLength(lFieldNum)
                    FieldDecimalCount(lFieldNum) = aAddFrom.FieldDecimalCount(lFieldNum)
                Next
                NumRecords = 0
                Me.InitData()
            End If

            If pNumFields <> aAddFrom.NumFields Then
                Throw New ApplicationException("Different number of fields:" & vbCr _
                          & FileName & " = " & pNumFields & vbCr _
                          & aAddFrom.FileName & " = " & aAddFrom.NumFields & vbCr & vbCr _
                          & "Cannot merge DBF files")
            Else
                For lFieldNum = 1 To pNumFields
                    If UCase(Trim(FieldName(lFieldNum))) <> UCase(Trim(aAddFrom.FieldName(lFieldNum))) Then
                        If Not Logger.Msg("Field '" & FieldName(lFieldNum) & "' does not appear to match '" _
                               & aAddFrom.FieldName(lFieldNum) & "'" & vbCr _
                               & "Proceed with merge anyway, treating these fields as matching?", vbYesNo, "Merge") = MsgBoxResult.No Then
                            Exit Sub
                        End If
                    End If
                    If lCanCopyRecords AndAlso FieldLength(lFieldNum) <> aAddFrom.FieldLength(lFieldNum) Then
                        Logger.Dbg("Different field lengths for " & FieldName(lFieldNum) & vbCr _
                                 & FileName & " = " & FieldLength(lFieldNum) & vbCr _
                                 & aAddFrom.FileName & " = " & aAddFrom.FieldLength(lFieldNum) & vbCr _
                                 & "Will copy fields instead of records")
                        lCanCopyRecords = False
                    End If
                Next
                If aDuplicateAction > 0 Then
                    If aKeyFieldNames Is Nothing Then
AllKeys:
                        lAllFieldsKey = True
                        lFirstKeyField = 1
                        lLastKeyField = pNumFields
                    Else
                        lFirstKeyField = LBound(aKeyFieldNames)
                        lLastKeyField = UBound(aKeyFieldNames)
                    End If
                    ReDim lKeyField(lLastKeyField)
                    ReDim lOperation(lLastKeyField)
                    ReDim lKeyValue(lLastKeyField)
                    For lFieldNum = lFirstKeyField To lLastKeyField
                        If lAllFieldsKey Then
                            lKeyField(lFieldNum) = lFieldNum
                            lOperation(lFieldNum) = "="
                        Else
                            If aKeyFieldNames(lFieldNum) = "**ALL**" Then
                                GoTo AllKeys
                            Else
                                lMsg = lMsg & aKeyFieldNames(lFieldNum) & ", "
                                lKeyField(lFieldNum) = FieldNumber(aKeyFieldNames(lFieldNum))
                                lOperation(lFieldNum) = "="
                            End If
                        End If
                    Next

                    If lAllFieldsKey Then
                        lMsg = "All fields must match to find a duplicate."
                    Else
                        If Len(lMsg) > 2 Then lMsg = " Looking for duplicate records in fields " & Left(lMsg, Len(lMsg) - 2)
                    End If

                    If Len(lMsg) > 0 Then
                        Select Case aDuplicateAction
                            Case 0 : Logger.Dbg(lMsg & " Not checking for duplicates")
                            Case 2 : Logger.Dbg(lMsg & " Overwriting existing with new duplicates")
                            Case Else : Logger.Dbg(lMsg & " Keep existing, discarding new duplicates")
                        End Select
                        lMsg = ""
                    End If
                End If

                lLastOldRec = pHeader.NumRecs 'Don't search for duplicates in newly added records
                If lLastOldRec < 1 Then aDuplicateAction = 0 'Don't bother checking for duplicates since we start empty
                aAddFrom.CurrentRecord = 1
                lRecordToCopy = aAddFrom.RawRecord

                If lRecordToCopy.Length <> pHeader.NumBytesRec Then
                    lCanCopyRecords = False
                    Logger.Dbg("Different number of bytes per record:" & vbCr _
                          & FileName & " = " & pHeader.NumBytesRec & vbCr _
                                & aAddFrom.FileName & " = " & lRecordToCopy.Length & vbCr _
                                & "Attempting to copy fields instead of records")
                End If

                With aAddFrom
                    For lAddRecordNum = 1 To .NumRecords
                        Logger.Progress(lAddRecordNum, .NumRecords)
                        .CurrentRecord = lAddRecordNum
                        If aDuplicateAction = 0 Then
                            'don't bother looking for a duplicate since we add them all anyway
                        ElseIf lAllFieldsKey AndAlso lCanCopyRecords Then
                            'First check current record to see if it matches                            
                            lRecordToCopy = aAddFrom.RawRecord
                            If Me.MatchRecord(lRecordToCopy) Then
                                lFoundDuplicate = True
                            ElseIf pCurrentRecord < lLastOldRec Then
                                'Check next record before searching hard for a match
                                'if trying to merge same data, next record will always be the one that matches
                                MoveNext()
                                If Me.MatchRecord(lRecordToCopy) Then
                                    lFoundDuplicate = True
                                Else
                                    lFoundDuplicate = FindRecord(lRecordToCopy, 1, lLastOldRec)
                                End If
                            Else
                                lFoundDuplicate = FindRecord(lRecordToCopy, 1, lLastOldRec)
                            End If
                        Else
                            For lFieldNum = lFirstKeyField To lLastKeyField
                                lKeyValue(lFieldNum) = .Value(lKeyField(lFieldNum))
                            Next
                            lFoundDuplicate = FindMatch(lKeyField, lOperation, lKeyValue, False, 1, lLastOldRec)
                        End If

                        Dim lCopyThisRecord As Boolean = False
                        If lFoundDuplicate Then
                            If aDuplicateAction = 2 Then 'overwrite existing record with new record
                                lCopyThisRecord = True
                            End If
                        Else  'Copy this record in the DBF
                            CurrentRecord = Me.NumRecords + 1
                            lCopyThisRecord = True
                        End If

                        If lCopyThisRecord Then
                            If lCanCopyRecords Then
                                Me.RawRecord = aAddFrom.RawRecord
                            Else 'Copy record field-by-field
                                For lFieldNum = 1 To pNumFields
                                    Me.Value(lFieldNum) = aAddFrom.Value(lFieldNum)
                                Next
                            End If
                            If lAddIndexes Then aAddedIndexes.Add(aAddFrom.CurrentRecord)
                        End If
                    Next
                End With
            End If
            Logger.Dbg("Had " & lNumRecsAtStart & ", Added " & pHeader.NumRecs - lNumRecsAtStart & ", now have " & pHeader.NumRecs)
        End If
    End Sub

    Public Overrides Property CurrentRecord() As Integer
        Get
            CurrentRecord = pCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            Try
                If Value > pHeader.NumRecs Then
                    pEOF = True
                    NumRecords = Value
                Else
                    pEOF = False
                End If
                If Value < 1 OrElse Value > pHeader.NumRecs Then
                    pCurrentRecord = 1
                Else
                    pCurrentRecord = Value
                End If
                pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1
            Catch ex As Exception
                Logger.Msg("Cannot set CurrentRecord to " & Value & vbCr & ex.Message, "Let CurrentRecord")
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
            If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
                Return "Invalid Current Record Number"
            ElseIf aFieldNumber < 1 Or aFieldNumber > pNumFields Then
                Return "Invalid Field Number"
            Else
                'Dim lFirstByte As Integer = pCurrentRecordStart + pFields(aFieldNumber).DataAddress
                'Dim lLength As Integer = pFields(aFieldNumber).FieldLength
                'While lLength > 0 AndAlso pData(lFirstByte + lLength - 1) = 0
                '    lLength -= 1
                'End While
                Return TrimValue(System.Text.Encoding.ASCII.GetString( _
                                       pData, _
                                       pCurrentRecordStart + pFields(aFieldNumber).DataAddress, _
                                       pFields(aFieldNumber).FieldLength), _
                                 FieldType(aFieldNumber))
            End If
        End Get
        Set(ByVal Value As String)
            Dim FieldStart As Integer
            Dim I As Integer
            Dim strRet As String
            Dim lenStr As Integer

            If pHeader.NumBytesRec = 0 Then InitData()

            Try
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
            Catch ex As Exception
                Logger.Msg("Cannot set field #" & aFieldNumber & " = '" & Value & "' in record #" & pCurrentRecord & vbCr & ex.Message, "Let Value")
            End Try
        End Set
    End Property

    'public Overrides Function CurrentRecordAsDelimitedString(Optional aDelimiter As String = ",", Optional aQuote As String = "") As String
    '  Dim retval As String
    '  Dim fieldVal As String
    '  Dim usingQuotes As Boolean
    '  Dim iField As integer
    '  If Len(aQuote) > 0 Then usingQuotes = True
    '  For iField = 1 To pNumFields
    '    fieldVal = Value(iField)
    '    If usingQuotes Then
    '      If InStr(fieldVal, aDelimiter) > 0 Then fieldVal = aQuote & fieldVal & aQuote
    '    End If
    '    retval &= Value(iField)
    '    If iField < pNumFields Then retval &= aDelimiter
    '  Next
    '  CurrentRecordAsDelimitedString = retval
    'End Function

    Public Overrides Function FindFirst(ByVal aFieldNumber As Integer, ByVal aFindValue As String, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
        'Dim lBaseFound As Boolean = MyBase.FindFirst(aFieldNumber, aFindValue, aStartRecord, aEndRecord)
        'Dim lBaseFoundRecord As Integer = -1
        'If lBaseFound Then lBaseFoundRecord = CurrentRecord
        If NumRecords < aStartRecord Then 'don't look past end
            Return False
        Else
            Dim lNumBytesRec As Integer = pHeader.NumBytesRec
            Dim lFieldStart As Integer = pFields(aFieldNumber).DataAddress
            Dim lFieldLength As Integer = pFields(aFieldNumber).FieldLength
            If aEndRecord < 1 Then aEndRecord = NumRecords
            Dim lEndSearchByte As Integer = lNumBytesRec * aEndRecord

            Dim lFindBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(TrimValue(aFindValue, FieldType(aFieldNumber)))
            Dim lLastFindByte As Integer = lFindBytes.GetUpperBound(0)

            Dim lStartRecord As Integer = aStartRecord
            While lStartRecord <= aEndRecord
                Dim lFoundIndex As Integer = findBytes(lFindBytes, 0, lFindBytes.Length, _
                                                       pData, _
                                                       lNumBytesRec * (lStartRecord - 1) + 1 + lFieldStart, _
                                                       lNumBytesRec, lEndSearchByte, lFieldLength)
                If lFoundIndex = 0 Then 'Did not find
                    Exit While
                Else
                    CurrentRecord = lStartRecord + lFoundIndex - 1
                    'Found aFindValue using findBytes, but still need to make sure whole value matches
                    If Value(aFieldNumber) = aFindValue Then
                        'If Not lBaseFound Then Stop
                        'If lBaseFoundRecord <> CurrentRecord Then Stop
                        Return True
                    Else
                        lStartRecord += lFoundIndex
                    End If
                End If
            End While

            'If lBaseFound Then Stop
            CurrentRecord = aStartRecord
            Return False
        End If
    End Function

    'Returns True if found, moves CurrentRecord to first record with .Record = FindValue
    'If not found, returns False and moves CurrentRecord to aStartRecord
    Public Function FindRecord(ByVal FindValue() As Byte, Optional ByVal aStartRecord As Integer = 1, Optional ByVal aEndRecord As Integer = -1) As Boolean
        Dim lByteOfRecord As Integer

        If aEndRecord < 1 Then aEndRecord = pHeader.NumRecs
        Dim lLastByte As Integer = pHeader.NumBytesRec - 1

        pCurrentRecord = aStartRecord
        pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1

        While pCurrentRecord <= aEndRecord

            For lByteOfRecord = 0 To lLastByte
                If pData(pCurrentRecordStart + lByteOfRecord) <> FindValue(lByteOfRecord) Then GoTo NotEqual
            Next
            Return True
NotEqual:
            pCurrentRecord += 1
            pCurrentRecordStart += pHeader.NumBytesRec            
        End While
        CurrentRecord = aStartRecord
        Return False
    End Function

    'Returns True if CurrentRecord matches FindValue
    Public Function MatchRecord(ByVal FindValue() As Byte) As Boolean
        Dim byt As Integer
        Dim lastbyt As Integer
        If UBound(FindValue) < pHeader.NumBytesRec Then
            lastbyt = UBound(FindValue)
        Else
            lastbyt = pHeader.NumBytesRec - 1
        End If
        For byt = 0 To lastbyt
            If pData(pCurrentRecordStart + byt) <> FindValue(byt) Then
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
    'operation such as =, <, >, <=, >=
    'value to compare with
    'For example, FindNextWhere(1, "=", "Mercury", 2, "<=", 0)
    'will find next record where the first field value is "Mercury" and the second is less than or equal to zero
    'Public Function FindMatchPA(ByVal aMatchAny As Boolean, _
    ''                            ByVal aStartRecord As Integer, _
    ''                            ByVal aEndRecord As Integer, _
    ''                            ParamArray aRules() As Variant) As Boolean
    '  Dim iToken As Integer
    '  Dim numTokens As Integer
    '  Dim numArgs As Integer
    '  Dim iArg As Integer
    '  numArgs = UBound(aRules()) + 1
    '  numTokens = numArgs / 3
    '  If numTokens * 3 <> numArgs Then
    '    MsgBox "Could not parse:number of args (" & numArgs & ") not divisible by 3", vbOKOnly, "clsDBF:FindNextAnd"
    '  End If
    '  Dim fieldNum() As Integer
    '  Dim operation() As String
    '  Dim fieldVal() As Variant
    '  Dim Token As String
    '
    '  ReDim fieldNum(numTokens)
    '  ReDim operation(numTokens)
    '  ReDim Values(numTokens)
    '  iArg = 0
    '  For iToken = 0 To numTokens - 1
    '    Token = aRules(iArg)
    '    If Not IsNumeric(Token) Then Token = FieldNumber(Token)
    '    fieldNum(iToken) = CLng(Token)
    '    If fieldNum(iToken) = 0 Then Debug.Print "FindNextRules:Field(" & aRules(iArg) & ") not found"
    '    iArg = iArg + 1
    '    operation(iToken) = aRules(iArg)
    '    iArg = iArg + 1
    '    fieldVal(iToken) = aRules(iArg)
    '    iArg = iArg + 1
    '  Next
    '  FindMatchPA = FindMatch(fieldNum, operation, fieldVal, aMatchAny, aStartRecord, aEndRecord)
    'End Function


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

    Private Function TrimNull(ByVal Value As String) As String
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
        pHeader.Clear()
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
        Dim iField As Integer
        Dim newDBF As New atcTableDBF
        With newDBF
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
        Dim iField As Integer

        retval = "Dim newDBF as new atcTableDBF"
        retval &= vbCrLf & "With newDBF"
        retval &= vbCrLf & "  .NumFields = " & pNumFields
        retval &= vbCrLf
        For iField = 1 To pNumFields
            With pFields(iField)
                retval &= vbCrLf & "  .FieldName(" & iField & ") = """ & TrimNull(.FieldName) & """"
                retval &= vbCrLf & "  .FieldType(" & iField & ") = """ & .FieldType & """"
                retval &= vbCrLf & "  .FieldLength(" & iField & ") = " & .FieldLength
                retval &= vbCrLf & "  .FieldDecimalCount(" & iField & ") = " & .DecimalCount
                retval &= vbCrLf
            End With
        Next
        retval &= vbCrLf & "  '.NumRecords = " & pHeader.NumRecs
        retval &= vbCrLf & "  '.InitData"
        retval &= vbCrLf & "End With"
        retval &= vbCrLf
        Return retval
    End Function

    'Returns zero if the named field does not appear in this file
    Public Overrides Function FieldNumber(ByVal aFieldName As String) As Integer
        aFieldName = aFieldName.ToLower
        For lFieldIndex As Integer = 1 To pNumFields
            If TrimNull(pFields(lFieldIndex).FieldName).ToLower = aFieldName Then
                Return lFieldIndex
            End If
        Next
        Return 0
    End Function

    Public Overrides Function OpenFile(ByVal aFilename As String) As Boolean
        'Dim header As clsHeader, FieldDes As clsFieldDescriptor    'Creating variables for user-defined types
        'Dim memo As String * 512                               'Create a 512 byte fixed string variable
        ' to read memo fields
        Dim inFile As Short
        Dim I As Integer

        If Not IO.File.Exists(aFilename) Then
            Return False 'can't open a file that doesn't exist
        End If

        FileName = aFilename

        inFile = FreeFile()
        FileOpen(inFile, aFilename, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
        pHeader.ReadFromFile(inFile)
        Select Case pHeader.version 'Be sure we're using a dBASE III file
            Case 3 'Normal dBASEIII file
                '   Case &H83 'Open a .DBT file
            Case Else
                Logger.Msg("This is not a dBASE III file: '" & aFilename & "'", "OpenDBF")
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

    Public Overrides Function SummaryFields(Optional ByVal aFormat As String = "tab,headers,expandtype") As String
        Dim retval As String = ""
        Dim iField As Integer
        Dim ShowTrash As Boolean
        Dim ShowHeaders As Boolean
        Dim ExpandType As Boolean

        If InStr(LCase(aFormat), "trash") > 0 Then ShowTrash = True
        If InStr(LCase(aFormat), "headers") > 0 Then ShowHeaders = True
        If InStr(LCase(aFormat), "expandtype") > 0 Then ExpandType = True

        If InStr(LCase(aFormat), "text") > 0 Then 'text version
            For iField = 1 To pNumFields
                With pFields(iField)
                    retval &= vbCrLf & "Field " & iField & ": '" & TrimNull(.FieldName) & "'"
                    retval &= vbCrLf & "    Type: " & .FieldType & " "
                    If ExpandType Then
                        Select Case .FieldType
                            Case "C" : retval &= "Character"
                            Case "D" : retval &= "Date     "
                            Case "N" : retval &= "Numeric  "
                            Case "L" : retval &= "Logical  "
                            Case "M" : retval &= "Memo     "
                        End Select
                    Else
                        retval &= .FieldType
                    End If
                    retval &= vbCrLf & "    Length: " & .FieldLength & " "
                    retval &= vbCrLf & "    DecimalCount: " & .DecimalCount & " "
                    If ShowTrash Then
                        retval &= vbCrLf & "    Trash: "
                        For Each lTrash As Byte In .Trash
                            retval &= lTrash & " "
                        Next
                    End If
                End With
                retval &= vbCrLf
            Next
        Else 'table version
            If ShowHeaders Then
                retval &= "Field "
                retval &= vbTab & "Name "
                retval &= vbTab & "Type "
                retval &= vbTab & "Length "
                retval &= vbTab & "DecimalCount "
                If ShowTrash Then retval &= vbTab & "Trash"
            End If
            retval &= vbCrLf
            'now field details
            For iField = 1 To pNumFields
                With pFields(iField)
                    retval &= iField & vbTab & "'" & TrimNull(.FieldName) & "' "
                    If ExpandType Then
                        Select Case .FieldType
                            Case "C" : retval &= vbTab & "Character"
                            Case "D" : retval &= vbTab & "Date     "
                            Case "N" : retval &= vbTab & "Numeric  "
                            Case "L" : retval &= vbTab & "Logical  "
                            Case "M" : retval &= vbTab & "Memo     "
                        End Select
                    Else
                        retval &= vbTab & .FieldType
                    End If
                    retval &= vbTab & .FieldLength
                    retval &= vbTab & .DecimalCount
                    If ShowTrash Then
                        retval &= vbTab
                        For Each lTrash As Byte In .Trash
                            retval &= lTrash & " "
                        Next
                    End If
                End With
                retval &= vbCrLf
            Next
        End If
        Return retval
    End Function

    Public Overrides Function SummaryFile(Optional ByVal aFormat As String = "tab,headers") As String
        Dim retval As String = ""
        Dim lFormat As String = aFormat.ToLower
        Dim ShowTrash As Boolean = lFormat.Contains("trash")
        Dim ShowHeaders As Boolean = lFormat.Contains("headers")

        If lFormat.Contains("text") Then 'text version
            With pHeader
                retval = "DBF Header: "
                retval &= vbCrLf & "    FileName: " & FileName
                retval &= vbCrLf & "    Version: " & .version
                retval &= vbCrLf & "    Date: " & Year & "/" & Month & "/" & Day
                retval &= vbCrLf & "    NumRecs: " & .NumRecs
                retval &= vbCrLf & "    NumBytesHeader: " & .NumBytesHeader
                retval &= vbCrLf & "    NumBytesRec: " & .NumBytesRec
                If ShowTrash Then
                    retval &= vbCrLf & "    Trash: "
                    For Each lTrash As Byte In .Trash
                        retval &= lTrash & " "
                    Next
                End If
            End With
        Else 'table version
            'build header header
            If ShowHeaders Then
                retval = "FileName "
                retval &= vbTab & "Version "
                retval &= vbTab & "Date "
                retval &= vbTab & "NumFields "
                retval &= vbTab & "NumRecs "
                retval &= vbTab & "NumBytesHeader "
                retval &= vbTab & "NumBytesRec "
            End If
            If ShowTrash Then retval &= vbTab & "Trash"
            retval &= vbCrLf
            With pHeader 'now header data
                retval &= FileName
                retval &= vbTab & .version
                retval &= vbTab & Year & "/" & Month & "/" & Day
                retval &= vbTab & pNumFields
                retval &= vbTab & .NumRecs
                retval &= vbTab & .NumBytesHeader
                retval &= vbTab & .NumBytesRec
                If ShowTrash Then
                    retval &= vbTab
                    For Each lTrash As Byte In .Trash
                        retval &= lTrash & " "
                    Next
                End If
                retval &= vbCrLf
            End With
        End If
        SummaryFile = retval
    End Function

    Public Function SummaryDataBinary() As String
        Dim lSB As New Text.StringBuilder
        Dim lCurrentRecordStart As Integer
        For lCurrentRecord As Integer = 1 To Me.NumRecords
            lCurrentRecordStart = pHeader.NumBytesRec * (lCurrentRecord - 1) + 1
            lSB.Append(lCurrentRecord & " @ " & lCurrentRecordStart & vbCrLf)
            For lFieldNumber As Integer = 1 To pNumFields
                Dim lFirstByte As Integer = lCurrentRecordStart + pFields(lFieldNumber).DataAddress
                Dim lLastByte As Integer = lFirstByte + pFields(lFieldNumber).FieldLength - 1
                For lByte As Integer = lFirstByte To lLastByte
                    lSB.Append(Hex(pData(lByte)) & " ")
                Next
                lSB.Append(" = ")
                For lByte As Integer = lFirstByte To lLastByte
                    If pData(lByte) > 0 Then
                        lSB.Append(Chr(pData(lByte)))
                    Else
                        Exit For
                    End If
                Next
                lSB.Append(vbCrLf)
            Next
        Next
        Return Trim(lSB.ToString)
    End Function

    Public Overrides Function WriteFile(ByVal aFilename As String) As Boolean
        Dim OutFile As Short
TryAgain:
        Try
            If IO.File.Exists(aFilename) Then
                Kill(aFilename)
            Else
                Dim lPath As String = IO.Path.GetDirectoryName(aFilename)
                If lPath.Length > 0 Then
                    IO.Directory.CreateDirectory(lPath)
                End If
            End If

            OutFile = FreeFile()
            FileOpen(OutFile, aFilename, OpenMode.Binary)
            pHeader.WriteToFile(OutFile)

            For lField As Integer = 1 To pNumFields
                pFields(lField).WriteToFile(OutFile) 'FilePutObject(OutFile, pFields(I), (32 * I) + 1)
            Next

            'If we have over-allocated for adding more records, trim unused records
            If pNumRecsCapacity > pHeader.NumRecs Then
                pNumRecsCapacity = pHeader.NumRecs
                ReDim Preserve pData(pHeader.NumRecs * pHeader.NumBytesRec)
            End If

            FilePut(OutFile, pData)
            'Ensure that file ends with EOF character (Ctrl-Z = ASCII 26)
            If pData(UBound(pData)) <> 26 Then FilePut(OutFile, CByte(26))
            FileClose(OutFile)

            FileName = aFilename
            Return True

        Catch ex As Exception
            If Logger.Msg("Error saving " & aFilename & vbCr & ex.Message, MsgBoxStyle.AbortRetryIgnore, "Write DBF") = MsgBoxResult.Retry Then
                Try
                    FileClose(OutFile)
                Catch
                    'ignore error if file cannot be closed
                End Try
                GoTo TryAgain
            End If
            Return False
        End Try
    End Function

    Public Property RawBytesPerRecord() As Integer
        Get
            Return pHeader.NumBytesRec
        End Get
        Set(ByVal newValue As Integer)
            pHeader.NumBytesRec = newValue
        End Set
    End Property

    Public Property RawCurrentRecordStart() As Integer
        Get
            Return pCurrentRecordStart
        End Get
        Set(ByVal newValue As Integer)
            pCurrentRecordStart = newValue
        End Set
    End Property

    Public ReadOnly Property RawValueStart(ByVal aFieldNumber As Integer) As Integer
        Get
            Return pCurrentRecordStart + pFields(aFieldNumber).DataAddress
        End Get
    End Property

    Public Property RawData() As Byte()
        Get
            Return pData
        End Get
        Set(ByVal newValue() As Byte)
            pData = newValue
            pDataBytes = pData.Length
        End Set
    End Property

    Public Property RawRecord() As Byte()
        Get
            Dim retval(pHeader.NumBytesRec - 1) As Byte
            Array.Copy(pData, pCurrentRecordStart, retval, 0, pHeader.NumBytesRec)
            Return retval
        End Get
        Set(ByVal newValue() As Byte)
            If newValue.Length = pHeader.NumBytesRec Then
                Array.Copy(newValue, 0, pData, pCurrentRecordStart, pHeader.NumBytesRec)
            Else
                Logger.Msg("Cannot Let RawRecord - wrong size newValue passed" & vbCr & "new record is " & UBound(newValue) + 1 & " bytes long" & vbCr & "but should be " & pHeader.NumBytesRec & " bytes long" & vbCr & Err.Description, "Let record")
            End If
        End Set
    End Property

    'find(aField as Integer, aValue as String [, aStartRecord, aStopRecord] )
    'makes a Byte() representation of aValue as it would appear in me
    'in aField -- padded as appropriate, then scans raw data for those
    'bytes in that field. Is there consistency about what bytes are
    'used for padding? Might need to search for first aValue.length
    'bytes in field, then carefully check whether a match is a longer
    'string or the same string padded differently or as expected.

    'find(aField, aRawValue as Byte() [, aStartRecord, aStopRecord])
    'internally used by above find

    'find(aRawRecord() as Byte())

    ''' <summary>
    ''' Search for the set of bytes in aFindThis starting at index aThisFirstByte for aFindNumBytes
    ''' Search through aSearchIn starting at aSearchStart and advancing aSearchStride bytes.
    ''' </summary>
    ''' <param name="aFindThis">Sequence of bytes to find</param>
    ''' <param name="aFindFirstByte">First position in aFindThis to include in search</param>
    ''' <param name="aFindNumBytes">Number of bytes in aFindThis to include in search</param>
    ''' <param name="aSearchIn">Array to search</param>
    ''' <param name="aSearchStart">Index in aSearchIn to start search</param>
    ''' <param name="aSearchStride">Record length in aSearchIn, move this many bytes from aSearchStart to search for next possible match</param>
    ''' <param name="aSearchStop">End the search when we reach this position in aSearchIn</param>
    ''' <param name="aFieldLength">Search this section of each record for aFindThis, skipping zero or space padding. If zero, only check for value that starts exactly at aSearchStart [plus stride * n]</param>
    ''' <returns>
    ''' Number of times the pattern was searched for if found
    ''' 0 = pattern was not found
    ''' 1 = pattern was found in first place searched
    ''' n = pattern was found (n-1) strides after first place searched
    ''' </returns>
    ''' <remarks>
    ''' Example:
    ''' findBytes(aFindThis = {0, 1, 2, 3}, 
    '''           aFindFirstByte = 1,
    '''           aFindNumBytes = 2,
    '''           aSearchIn = { 0, 0, 1, 2, 4, 1, 2, 0, 0},
    '''           aSearchStart = 1,
    '''           aSearchStride = 4)
    ''' searches for the pattern {1, 2} (the two bytes starting at 1 of aFindThis)
    ''' does not match at the first comparison with bytes {0, 1} in aSearchIn
    ''' (does not match first instance of {1, 2} in aSearchIn because search strides past)
    ''' strides 4, matches {1, 2} after the 4, and returns 2 because it was found on the second comparison
    ''' </remarks>
    Private Function findBytes(ByVal aFindThis As Byte(), _
                               ByVal aFindFirstByte As Integer, _
                               ByVal aFindNumBytes As Integer, _
                               ByVal aSearchIn As Byte(), _
                               ByVal aSearchStart As Integer, _
                               ByVal aSearchStride As Integer, _
                               ByVal aSearchStop As Integer, _
                               ByVal aFieldLength As Integer) As Integer
        Dim lFindLastByte As Integer = aFindFirstByte + aFindNumBytes - 1
        Dim lFindByte As Integer
        Dim lSearchPos As Integer
        Dim lNumSearches As Integer = 0
        Dim lPaddingRoom As Integer = aFieldLength - aFindNumBytes 'How much room is in the field for padding before the value we are searching for
        Dim lSkipPad As Integer

        While aSearchStart < aSearchStop
            lNumSearches += 1
            lFindByte = aFindFirstByte
            lSearchPos = aSearchStart

            'First skip any padding characters (NUL or Space) at beginning of field
            For lSkipPad = 1 To lPaddingRoom
                Select Case aSearchIn(lSearchPos)
                    Case 0, 32 : lSearchPos += 1
                    Case Else : Exit For
                End Select
            Next

            While lFindByte <= lFindLastByte AndAlso aSearchIn(lSearchPos) = aFindThis(lFindByte)
                lFindByte += 1
                lSearchPos += 1
            End While
            If lFindByte > lFindLastByte Then 'Found a match
                Return lNumSearches
            End If
            aSearchStart += aSearchStride
        End While

        Return 0 'not found
    End Function

    ''' <summary>
    ''' Find records in aOtherTable that do not match any record in this table
    ''' </summary>
    ''' <param name="aOtherTable">Table to search for new records</param>
    ''' <param name="aField">Optional key field to search. If not specified, entire record will be compared.</param>
    ''' <returns>ArrayList of indexes of new records found in aOtherTable</returns>
    ''' <remarks></remarks>
    Public Function findAllNew(ByVal aOtherTable As atcTableDBF, Optional ByVal aField As Integer = 0) As ArrayList
        Dim lOtherData() As Byte = aOtherTable.RawData
        Dim lOtherBytes As Integer
        Dim lOtherStart As Integer
        Dim lOtherRecord As Integer
        Dim lOtherNumRecords As Integer = aOtherTable.NumRecords

        Dim lStride As Integer = pHeader.NumBytesRec
        Dim lStop As Integer = pData.GetUpperBound(0)

        Dim lNewRecords As New ArrayList

        If aField = 0 Then
            lOtherBytes = aOtherTable.RawBytesPerRecord
        Else
            lOtherBytes = aOtherTable.FieldLength(aField)
        End If

        For lOtherRecord = 1 To lOtherNumRecords
            aOtherTable.CurrentRecord = lOtherRecord

            If aField = 0 Then
                lOtherStart = aOtherTable.RawCurrentRecordStart
            Else
                lOtherStart = aOtherTable.RawValueStart(aField)
            End If

            If findBytes(lOtherData, lOtherStart, lOtherBytes, pData, 0, lStride, lStop, 0) = 0 Then
                lNewRecords.Add(lOtherRecord)
            End If
        Next

        Return lNewRecords
        'Dim lReturn As Integer()
        'If lNewRecords.Count > 0 Then
        '    ReDim lReturn(lNewRecords.Count - 1)
        '    For lRecord As Integer = 0 To lReturn.GetUpperBound(0)
        '        lReturn(lRecord) = lNewRecords(lRecord)
        '    Next
        'Else
        '    ReDim lReturn(0)
        'End If
        'Return lReturn
    End Function

    '''' <summary>
    '''' Add the non-duplicate fields from aAddFileName to aBaseFileName, save result in aBaseFileName
    '''' Records that contain the same value in a field in each table are joined together
    '''' </summary>
    '''' <param name="aBaseFileName">DBF with fields that will come first in joined DBF</param>
    '''' <param name="aAddFileName">DBF with fields that will be added</param>
    '''' <param name="aBaseFieldName">Name of field to match in Base</param>
    '''' <param name="aAddFieldName">Name of field to match in Add, defaults to aBaseFieldName</param>
    '''' <remarks></remarks>
    'Private Sub JoinAndSaveDbf(ByVal aBaseFileName As String, _
    '                           ByVal aAddFileName As String, _
    '                           ByVal aBaseFieldName As String, _
    '                  Optional ByVal aAddFieldName As String = "")
    '    If aAddFieldName.Length = 0 Then
    '        aAddFieldName = aBaseFieldName
    '    End If

    '    Dim lBaseDbf As New atcTableDBF
    '    If Not lBaseDbf.OpenFile(aBaseFileName) Then
    '        Throw New ApplicationException("Cannot Open Base File " & aBaseFileName)
    '    End If
    '    Dim lBaseJoinField As Integer = lBaseDbf.FieldNumber(aBaseFieldName)
    '    If lBaseJoinField = 0 Then
    '        Throw New ApplicationException("Missing Join Field " & aBaseFieldName & " on " & aBaseFileName)
    '    End If

    '    Dim lAddDbf As New atcTableDBF
    '    If Not lAddDbf.OpenFile(aAddFileName) Then
    '        Throw New ApplicationException("Cannot Open Join File " & aAddFileName)
    '    End If
    '    Dim lAddJoinField As Integer = lAddDbf.FieldNumber(aAddFieldName)
    '    If lAddJoinField = 0 Then
    '        Throw New ApplicationException("Missing Join Field " & aAddFieldName & " on " & aAddFileName)
    '    End If

    '    'build the joined file
    '    Dim lNewDbf As New atcTableDBF
    '    Dim lAddDbfUseFields As New ArrayList 'field numbers to include from lAddDbf
    '    Dim lOldFieldIndex As Integer
    '    For lOldFieldIndex = 1 To lAddDbf.NumFields
    '        'add fields from lAddDbf that are not the key and are not in lBaseDbf
    '        If lOldFieldIndex <> lAddJoinField AndAlso lBaseDbf.FieldNumber(lAddDbf.FieldName(lOldFieldIndex)) = 0 Then
    '            lAddDbfUseFields.Add(lOldFieldIndex)
    '        End If
    '    Next

    '    lNewDbf.NumFields = lBaseDbf.NumFields + lAddDbfUseFields.Count
    '    Dim lNewFieldIndex As Integer = 1
    '    For lOldFieldIndex = 1 To lBaseDbf.NumFields
    '        lNewDbf.FieldType(lNewFieldIndex) = lBaseDbf.FieldType(lOldFieldIndex)
    '        lNewDbf.FieldLength(lNewFieldIndex) = lBaseDbf.FieldLength(lOldFieldIndex)
    '        lNewDbf.FieldName(lNewFieldIndex) = lBaseDbf.FieldName(lOldFieldIndex)
    '        lNewDbf.FieldDecimalCount(lNewFieldIndex) = lBaseDbf.FieldDecimalCount(lOldFieldIndex)
    '        lNewFieldIndex += 1
    '    Next
    '    For Each lOldFieldIndex In lAddDbfUseFields
    '        lNewDbf.FieldType(lNewFieldIndex) = lAddDbf.FieldType(lOldFieldIndex)
    '        lNewDbf.FieldLength(lNewFieldIndex) = lAddDbf.FieldLength(lOldFieldIndex)
    '        lNewDbf.FieldName(lNewFieldIndex) = lAddDbf.FieldName(lOldFieldIndex)
    '        lNewDbf.FieldDecimalCount(lNewFieldIndex) = lAddDbf.FieldDecimalCount(lOldFieldIndex)
    '        lNewFieldIndex += 1
    '    Next
    '    lNewDbf.NumRecords = lBaseDbf.NumRecords
    '    lNewDbf.FileName = lBaseDbf.FileName
    '    lNewDbf.InitData()

    '    'do the join
    '    Dim lJoinValue As String
    '    For lRecordIndex As Integer = 1 To lNewDbf.NumRecords
    '        lBaseDbf.CurrentRecord = lRecordIndex
    '        lNewDbf.CurrentRecord = lRecordIndex
    '        lNewFieldIndex = 1
    '        For lOldFieldIndex = 1 To lBaseDbf.NumFields
    '            lNewDbf.Value(lNewFieldIndex) = lBaseDbf.Value(lOldFieldIndex)
    '            lNewFieldIndex += 1
    '        Next
    '        lJoinValue = lBaseDbf.Value(lBaseJoinField)
    '        If lAddDbf.FindFirst(lAddJoinField, lJoinValue) Then
    '            For Each lFieldIndex As Integer In lAddDbfUseFields
    '                lNewDbf.Value(lNewFieldIndex) = lAddDbf.Value(lFieldIndex)
    '                lNewFieldIndex += 1
    '            Next
    '        Else
    '            Logger.Dbg("No record found to join key " & lJoinValue)
    '        End If
    '    Next
    '    lBaseDbf.Clear()
    '    lAddDbf.Clear()
    '    lNewDbf.WriteFile(aBaseFileName)
    '    TryDelete(aAddFileName)
    '    Logger.Dbg("Joined " & IO.Path.GetFileName(aAddFileName) & " to " & IO.Path.GetFileName(aBaseFileName))
    'End Sub

End Class
