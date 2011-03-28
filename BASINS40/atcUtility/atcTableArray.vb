Option Strict Off
Option Explicit On
Imports System.IO
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' Manage a table of strings
''' </summary>
''' <remarks>
''' Values are stored as a Generic.List(Of String()) where the Generic.List manages the rows and each row is an array of String
''' </remarks>
Public Class atcTableArray
    Inherits atcTable

    Protected pFieldNames() As String
    Private pFieldLengths() As Integer
    Private pFieldTypes() As String
    Private pNumFields As Integer

    Friend pRecords As New Generic.List(Of String())
    Private pDelimiter As Char = Chr(44) 'default to Chr(44) = comma

    Public Overrides Property CurrentRecord() As Integer
        Get
            Return pCurrentRecord
        End Get
        Set(ByVal newValue As Integer)
            Try
                If newValue < 1 Then
                    pCurrentRecord = 1
                Else
                    pCurrentRecord = newValue
                End If
                If pCurrentRecord > pRecords.Count Then
                    pEOF = True
                    NumRecords = pCurrentRecord
                Else
                    pEOF = False
                End If
                Exit Property
            Catch ex As Exception
                Throw New ApplicationException("atcTableArray: Cannot set CurrentRecord to " & newValue & vbCr & ex.Message)
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

    Public Overrides Property NumRecords() As Integer
        Get
            Return pRecords.Count
        End Get
        Set(ByVal newValue As Integer)
            While newValue > pRecords.Count
                Dim lRow(pNumFields) As String
                pRecords.Add(lRow)
            End While
            If newValue < pRecords.Count Then
                pRecords.RemoveRange(newValue, pRecords.Count - newValue)
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
                Return TrimValue(pRecords(pCurrentRecord - 1)(aFieldNumber - 1), FieldType(aFieldNumber))
            End If
        End Get
        Set(ByVal newValue As String)
            Try
                If pCurrentRecord < 1 Then
                    Throw New ApplicationException("Value: Invalid Current Record Number: " & pCurrentRecord & " < 1")
                ElseIf aFieldNumber < 1 Then
                    Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " < 1")
                ElseIf aFieldNumber > pNumFields Then
                    Throw New ApplicationException("Value: Invalid Field Number: " & aFieldNumber & " > " & pNumFields)
                Else
                    pRecords(pCurrentRecord - 1)(aFieldNumber - 1) = newValue
                End If
                Exit Property
            Catch e As Exception
                Logger.Msg("Cannot set field #" & aFieldNumber & " = '" & newValue & "' in record #" & pCurrentRecord & vbCr & e.Message, "Set Value")
            End Try
        End Set
    End Property

    Public Overrides Sub Clear()
        ClearData()
        pHeaderLines.Clear()
        NumFields = 0
    End Sub

    Public Overrides Sub ClearData()
        pRecords.Clear()
    End Sub

    Public Overrides Function Cousin() As IatcTable
        Dim iField As Short
        Dim lCousin As New atcTableArray
        With lCousin
            .NumFields = pNumFields

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

    'Read a file into the table
    Public Overrides Function OpenFile(ByVal Filename As String) As Boolean
        Return False
    End Function

    Public Overrides Function WriteFile(ByVal aFilename As String) As Boolean
TryAgain:
        Try
            IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(aFilename))
            Dim lOutStream As StreamWriter = File.CreateText(aFilename)

            lOutStream.Write(Header)

            lOutStream.Write(String.Join(Delimiter, pFieldNames, 1, NumFields) & vbCrLf)
            For Each lRecord As String() In pRecords
                lOutStream.Write(String.Join(Delimiter, lRecord) & vbCrLf)
            Next
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
