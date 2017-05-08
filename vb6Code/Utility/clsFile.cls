VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private Const pBufMax As Long = 65535
Private pBuffer(0 To pBufMax) As Byte
Private pLastBufByte As Long
Private pBufPos As Long
Private pFileLength As Long
Private pTotalBytesRead As Long
Private pLineDelimiter As Byte

Private pFilePointer As Integer

Private Sub Class_Initialize()
  pLineDelimiter = 10 'vbLF
  pLastBufByte = -1
End Sub

Public Function OpenFile(filename As String) As Boolean
  If FileExists(filename) Then
    pFilePointer = FreeFile
    Open filename For Binary Access Read As pFilePointer
    pFileLength = LOF(pFilePointer)
    OpenFile = True
  End If
End Function

Public Function ReadLine() As String
  Dim ch As String
  If pFilePointer > 0 Then
    While Not Me.EndOfFile
      While pLastBufByte >= pBufPos
        ch = Chr(pBuffer(pBufPos))
        Select Case pBuffer(pBufPos)
          Case vbCr: 'Should we check to make sure the next character is a LF?
          Case vbLf: Exit Function
          Case Else: ReadLine = ReadLine & ch
        End Select
        pBufPos = pBufPos + 1
      Wend
      FillBuffer
    Wend
  End If
End Function

Private Sub FillBuffer()
  If pFilePointer > 0 Then
    If Not EOF(pFilePointer) Then
      Get #pFilePointer, , pBuffer
      If pTotalBytesRead + pBufMax + 1 > pFileLength Then
        pLastBufByte = pFileLength - pTotalBytesRead - 1
      Else
        pLastBufByte = pBufMax
      End If
      pTotalBytesRead = pTotalBytesRead + pLastBufByte + 1
      pBufPos = 0
    End If
  End If
End Sub

Public Function EndOfFile() As Boolean
  If pLastBufByte >= pBufPos Then
    EndOfFile = False
  ElseIf pFilePointer = 0 Then
    EndOfFile = True
  ElseIf EOF(pFilePointer) Then
    EndOfFile = True
  Else
    EndOfFile = False
  End If
End Function
