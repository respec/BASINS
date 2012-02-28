Option Strict Off
Option Explicit On
Imports System.IO
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' Read and write USGS RDB-formatted delimited text files as a table
''' </summary>
''' <remarks>
''' Field delimiter is a tab
''' Header rows start with #
''' First row after headers is column labels
''' Second row after headers contains field widths and types
''' </remarks>
''' 
Public Class atcTableRDB
    Inherits atcTableDelimited

    Public Sub New()
        Me.Delimiter = vbTab
    End Sub

    'Read a stream into the table
    Public Overrides Function OpenStream(ByVal aStream As Stream) As Boolean
        Dim inReader As New BinaryReader(aStream)
        Dim lFinishedHeader As Boolean = False
        Dim lRecordCount As Integer = 0
        Dim lGotFieldSpecs As Boolean = False
        pRecords = New ArrayList(100)
        pRecords.Add("Unused record 0")
        NumFields = 0
        For Each lCurrentLine As String In LinesInFile(inReader)
            If lGotFieldSpecs Then
                pRecords.Add(lCurrentLine)
            ElseIf lCurrentLine.StartsWith("#") Then
                pHeaderLines.Add(lCurrentLine)
            ElseIf NumFields = 0 Then
                NumFields = CountString(lCurrentLine, Delimiter) + 1
                'Split creates a zero-based array. Prepending pDelimiter inserts blank field name so pFieldNames(1) contains first name
                pFieldNames = (Delimiter & lCurrentLine).Split(Delimiter)
            Else
                Dim lFieldSpecs() As String = (Delimiter & lCurrentLine).Split(Delimiter)
                For lField As Integer = 1 To NumFields
                    Me.FieldLength(lField) = StrFirstInt(lFieldSpecs(lField))
                    Me.FieldType(lField) = lFieldSpecs(lField).Trim                    
                    If Me.FieldLength(lField) = 0 Then 'Field width is missing, default to 10 wide
                        Me.FieldLength(lField) = 10
                    End If
                    If Me.FieldType(lField).Length = 0 Then 'Field type is missing, assume string type unless it is a count
                        If Me.FieldName(lField).Contains("_count") Then
                            Me.FieldType(lField) = "n"
                        Else
                            Me.FieldType(lField) = "s"
                        End If
                    End If
                Next
                lGotFieldSpecs = True
            End If
        Next

        Me.CurrentRecord = 1
        Return True
    End Function

    Public Overrides Function WriteFile(ByVal aFilename As String) As Boolean
TryAgain:
        Try
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(aFilename))

            Dim lOutStream As StreamWriter = File.CreateText(aFilename)

            lOutStream.Write(Header)

            lOutStream.Write(String.Join(Delimiter, pFieldNames, 1, NumFields) & vbCrLf)

            Dim lLastIndex As Integer = NumFields - 1
            For lFieldIndex As Integer = 1 To lLastIndex
                lOutStream.Write(FieldLength(lFieldIndex) & RDBfieldtype(FieldType(lFieldIndex)) & Delimiter)
            Next
            lOutStream.Write(FieldLength(NumFields) & RDBfieldtype(FieldType(NumFields)) & vbCrLf)

            lOutStream.Write(String.Join(Delimiter, pRecords.ToArray, 1, NumRecords) & vbCrLf)

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

    Private Function RDBfieldtype(ByVal aDBFfieldType As String) As String
        Select Case aDBFfieldType.ToUpper
            Case "C" : Return "s"
            Case "N" : Return "n"
            Case Else : Return aDBFfieldType
        End Select
    End Function


    ''' <summary>
    ''' Overriding default FieldType to translate from RDB to DBF version of type character
    ''' </summary>
    ''' <param name="aFieldNumber"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' RDB s = string  -> DBF C = Character
    ''' RDB d = date    -> DBF C = Character
    ''' RDB n = numeric -> DBF N = Numeric
    ''' </remarks>
    Public Overrides Property FieldType(ByVal aFieldNumber As Integer) As String
        Get
            Return MyBase.FieldType(aFieldNumber)
        End Get
        Set(ByVal newValue As String)
            Select Case newValue.ToLower
                Case "s", "d" : newValue = "C"
                Case "n" : newValue = "N"
            End Select
            MyBase.FieldType(aFieldNumber) = newValue
        End Set
    End Property
End Class
