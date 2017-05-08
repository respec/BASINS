VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsFtnUnfFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim pFileName As String
Dim pFileNum As Long
Dim pBytesInFile As Long
Dim pErrorDescription As String
Private Type UnfRec
  StartPos As Long
  len As Long
  rec() As Byte
End Type
Dim pRecords() As UnfRec
Dim pRecordCount As Long

Public Property Get RecordCount() As Long
  RecordCount = pRecordCount
End Property
Public Property Get reclen(index As Long) As Long
  reclen = pRecords(index).len
End Property
Public Property Get rec(index As Long) As Byte()
  ReDim rec(pRecords(index).len - 1)
  rec = pRecords(index).rec
End Property

Public Property Get Filename() As String
  Filename = pFileName
End Property
Public Property Let Filename(newFileName As String)
  Dim byt As Byte
  Dim l As Long
  
  If Len(Dir(newFileName)) = 0 Then
    pErrorDescription = "File '" & newFileName & "' not found"
  Else
    pFileNum = FreeFile(0)
    Open newFileName For Binary Access Read Shared As #pFileNum
    pBytesInFile = LOF(pFileNum)
    If pBytesInFile = 0 Then
      Close pFileNum
      pErrorDescription = "File '" & newFileName & "' is empty"
    Else
      Get #pFileNum, , byt
      If byt <> &HFD Then
        Close pFileNum
        pErrorDescription = "File: '" & newFileName & "' is not a Fortran Unformatted Sequential File" & vbCrLf & "(does not begin with hex FD)"
      Else
        pFileName = newFileName
        pRecordCount = 0
        ReDim pRecords(1000)
        ReadRestOfRecordsInFile True
      End If
    End If
  End If
End Property

Public Sub ReadRestOfRecordsInFile(Optional first As Boolean = False)

  pBytesInFile = LOF(pFileNum)
  Do While Seek(pFileNum) < pBytesInFile - 2
    pRecordCount = pRecordCount + 1
    If pRecordCount > UBound(pRecords) Then
      ReDim Preserve pRecords(pRecordCount * 2)
    End If
    With pRecords(pRecordCount)
      .StartPos = Seek(pFileNum)
      .len = FtnUnfSeqRecLen((pFileNum), first)
      'Debug.Print .StartPos, .Len
      If .len > 0 Then
        ReDim .rec(.len - 1)
        Get #pFileNum, , .rec
      Else
        'whats the problem?
        MsgBox "ReadRestOfRecordsInFile:Len=0:Start=" & .StartPos & ":Lof=" & pBytesInFile & ":File=" & pFileName
        pRecordCount = pRecordCount - 1
      End If
    End With
  Loop
  'ReDim Preserve pRecords(pRecordCount)
End Sub

Private Function FtnUnfSeqRecLen(f%, first As Boolean) As Long
  Dim b As Byte, reclen As Long, bytes As Integer, c As Long, h As Long
  Static LastLen As Long
  
  If first Then
    LastLen = 0
    first = False
  Else
    c = 64
    Get #f, , b
    While LastLen >= c
      c = c * 256
      Get #f, , b
    Wend
  End If
  Get #f, , b
  bytes = b And 3
  reclen = Fix(CSng(b) / 4)
  c = 64
  h = bytes + 1
  Do While bytes > 0
    Get #f, , b
    bytes = bytes - 1
    reclen = reclen + b * c
    c = c * 256
  Loop
  LastLen = reclen + h
  FtnUnfSeqRecLen = reclen
End Function

Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Sub Class_Initialize()
  pErrorDescription = ""
  pFileName = ""
End Sub
