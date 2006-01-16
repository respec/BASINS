Option Strict Off
Option Explicit On 

Imports Microsoft.VisualBasic.FileSystem
Imports MapWinUtility
Imports atcUtility

Friend Class clsFtnUnfFile
  '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license
  Dim pFileName As String = ""
  Dim pFileNum As Integer = 0
  Dim pBytesInFile As Integer = 0
  Dim pErrorDescription As String = ""
  Dim pRecords As ArrayList 'of clsUnfRec

  Private Class clsUnfRec
    Public StartPos As Integer = 0
    Public Len As Integer = 0
    Public Rec() As Byte
  End Class

  Friend ReadOnly Property RecordCount() As Integer
    Get
      Return pRecords.Count
    End Get
  End Property

  Friend ReadOnly Property Reclen(ByVal index As Integer) As Integer
    Get
      Return pRecords(index).len
    End Get
  End Property

  Friend ReadOnly Property Rec(ByVal index As Integer) As Byte()
    Get
      Return pRecords(index).Rec
    End Get
  End Property

  Friend Property Filename() As String
    Get
      Return pFileName
    End Get
    Set(ByVal newFileName As String)
      Dim Byt As Byte

      If Not FileExists(newFileName) Then
        pErrorDescription = "File '" & newFileName & "' not found"
      Else
        pFileNum = FreeFile()
        FileOpen(pFileNum, newFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
        pBytesInFile = LOF(pFileNum)
        If pBytesInFile = 0 Then
          FileClose(pFileNum)
          pErrorDescription = "File '" & newFileName & "' is empty"
        Else
          FileGet(pFileNum, Byt)
          If Byt <> &HFD Then
            FileClose(pFileNum)
            pErrorDescription = "File: '" & newFileName & "' is not a Fortran Unformatted Sequential File" & vbCrLf & "(does not begin with hex FD)"
          Else
            pFileName = newFileName
            pRecords = New ArrayList 'of UnfRec (TODO: make initial allocation here?)
            ReadRestOfRecordsInFile(True)
          End If
        End If
      End If
    End Set
  End Property

  Friend Sub ReadRestOfRecordsInFile(Optional ByVal first As Boolean = False)
    pBytesInFile = LOF(pFileNum)
    Do While Seek(pFileNum) < pBytesInFile - 2
      Dim lUnfRec As New clsUnfRec
      With lUnfRec
        .StartPos = Seek(pFileNum)
        .Len = FtnUnfSeqRecLen(pFileNum, first)
        'Debug.Print .StartPos, .Len
        If .Len > 0 Then 'fill in the data
          ReDim .Rec(.Len - 1)
          FileGet(pFileNum, .Rec)
        Else 'whats the problem?
          MsgBox("clsFtnUnfFile:ReadRestOfRecordsInFile:Len=0:Start=" & .StartPos & ":Lof=" & pBytesInFile & ":File=" & pFileName)
        End If
      End With
      pRecords.Add(lUnfRec)
    Loop
  End Sub

  Private Function FtnUnfSeqRecLen(ByVal f As Integer, ByRef first As Boolean) As Integer
    Dim b As Byte, reclen As Integer, bytes As Integer, c As Integer, h As Integer
    Static LastLen As Integer

    If first Then
      LastLen = 0
      first = False
    Else
      c = 64
      FileGet(f, b)
      While LastLen >= c
        c = c * 256
        FileGet(f, b)
      End While
    End If
    FileGet(f, b)
    bytes = b And 3
    reclen = Fix(CSng(b) / 4)
    c = 64
    h = bytes + 1
    Do While bytes > 0
      FileGet(f, b)
      bytes = bytes - 1
      reclen = reclen + b * c
      c = c * 256
    Loop
    LastLen = reclen + h
    FtnUnfSeqRecLen = reclen
  End Function

  Friend ReadOnly Property ErrorDescription() As String
    Get
      ErrorDescription = pErrorDescription
      pErrorDescription = ""
    End Get
  End Property
End Class
