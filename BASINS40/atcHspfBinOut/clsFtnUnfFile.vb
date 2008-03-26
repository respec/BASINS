Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic.FileSystem
Imports MapWinUtility
Imports atcUtility
Imports System.Collections.ObjectModel

''' <summary>
''' 
''' </summary>
''' <remarks>
''' Copyright 2005-8 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Friend Class clsFtnUnfFile
    Dim pFileName As String = ""
    Dim pFileNum As Integer = 0
    Dim pBytesInFile As Integer = 0
    Dim pErrorDescription As String = ""
    Dim pSeek As Integer
    Dim pRecords As Collection(Of UnformattedRecord)

    Private Class UnformattedRecord
        Friend StartPosition As Integer = 0
        Friend Length As Integer = 0
        Friend Record() As Byte
    End Class

    Friend ReadOnly Property RecordCount() As Integer
        Get
            Return pRecords.Count
        End Get
    End Property

    Friend ReadOnly Property RecordLength(ByVal aIndex As Integer) As Integer
        Get
            If aIndex >= 0 AndAlso aIndex < pRecords.Count Then
                Return pRecords(aIndex).Length
            Else
                Return Nothing
            End If
        End Get
    End Property

    Friend ReadOnly Property Record(ByVal aIndex As Integer) As Byte()
        Get
            If aIndex >= 0 AndAlso aIndex < pRecords.Count Then
                Return pRecords(aIndex).Record
            Else
                Return Nothing
            End If
        End Get
    End Property

    Friend Property Filename() As String
        Get
            Return pFileName
        End Get
        Set(ByVal aFileName As String)
            Dim lByte As Byte

            If Not FileExists(aFileName) Then
                pErrorDescription = "File '" & aFileName & "' not found"
            Else
                pFileName = aFileName
                Open()
                If pBytesInFile = 0 Then
                    pErrorDescription = "File '" & aFileName & "' is empty"
                Else
                    FileGet(pFileNum, lByte)
                    If lByte <> &HFD Then
                        pErrorDescription = "File: '" & aFileName & "' is not a Fortran Unformatted Sequential File" & vbCrLf & "(does not begin with hex FD)"
                    Else
                        pRecords = New Collection(Of UnformattedRecord)
                        ReadRestOfRecordsInFile(True)
                    End If
                End If
                Close()
            End If
        End Set
    End Property

    Friend Sub ReadRestOfRecordsInFile(Optional ByVal aFirst As Boolean = False)
        Dim lNeedToClose As Boolean = Open()
        pBytesInFile = LOF(pFileNum)
        Do While Seek(pFileNum) < pBytesInFile - 2
            Dim lUnfRec As New UnformattedRecord
            With lUnfRec
                .StartPosition = Seek(pFileNum)
                .Length = FtnUnfSeqRecLen(pFileNum, aFirst)
                'Debug.Print .StartPos, .Len
                If .Length > 0 Then 'fill in the data
                    ReDim .Record(.Length - 1)
                    FileGet(pFileNum, .Record)
                Else 'whats the problem?
                    Logger.Dbg("***** clsFtnUnfFile:ReadRestOfRecordsInFile:Len=0:Start=" & .StartPosition & ":Lof=" & pBytesInFile & ":File=" & pFileName)
                End If
            End With
            pRecords.Add(lUnfRec)
        Loop
        If lNeedToClose Then Close()
    End Sub

    Private Function FtnUnfSeqRecLen(ByVal aFileUnit As Integer, ByRef aFirst As Boolean) As Integer
        Static mLengthLast As Integer

        Dim c As Integer
        Dim h As Integer
        Dim lByte As Byte

        If aFirst Then
            mLengthLast = 0
            aFirst = False
        Else
            c = 64
            FileGet(aFileUnit, lByte)
            While mLengthLast >= c
                c = c * 256
                FileGet(aFileUnit, lByte)
            End While
        End If
        FileGet(aFileUnit, lByte)
        Dim lBytes As Integer = lByte And 3
        Dim lRecordLength As Integer = Fix(CSng(lByte) / 4)
        c = 64
        h = lBytes + 1
        Do While lBytes > 0
            FileGet(aFileUnit, lByte)
            lBytes = lBytes - 1
            lRecordLength = lRecordLength + lByte * c
            c = c * 256
        Loop
        mLengthLast = lRecordLength + h
        Return lRecordLength
    End Function

    Friend ReadOnly Property ErrorDescription() As String
        Get
            ErrorDescription = pErrorDescription
            pErrorDescription = ""
        End Get
    End Property

    'Open file if it is not already open; seek to the position where we were when Close was called
    'Returns true if file needed to be opened, false if file was already open
    Private Function Open() As Boolean
        If pFileNum = 0 Then
            pFileNum = FreeFile()
            FileOpen(pFileNum, pFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
            pBytesInFile = LOF(pFileNum)
            If pSeek > 0 Then Seek(pFileNum, pSeek)
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Close()
        If pFileNum > 0 Then
            pSeek = Seek(pFileNum)
            FileClose(pFileNum)
            pFileNum = 0
        End If
    End Sub
End Class
