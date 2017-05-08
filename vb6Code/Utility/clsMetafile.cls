VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsMetafile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private Type RECTL
      Left As Integer
      Top As Integer
      Right As Integer
      Bottom As Integer
End Type

Private Type RECT
        Left As Long
        Top As Long
        Right As Long
        Bottom As Long
End Type

Private Type POINTAPI
        x As Long
        y As Long
End Type

'Aldus Pleaceable Metafile Header
Private Type APMFILEHEADER
   key As Long
   hMF As Integer
   bbox As RECTL
   inch As Integer
   reserved As Long
   checksum As Integer
End Type

'Aldus Pleaceable Metafile Constant
Const APMHEADER_KEY As Long = &H9AC6CDD7

'Draw functions
Private Declare Function LineTo Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As Long
Private Declare Function MoveToEx Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, lpPoint As POINTAPI) As Long

'EMF file functions
Private Declare Function CreateEnhMetaFile Lib "gdi32" Alias "CreateEnhMetaFileA" (ByVal hdcRef As Long, ByVal lpFileName As String, lpRect As RECT, ByVal lpDescription As String) As Long
Private Declare Function CloseEnhMetaFile Lib "gdi32" (ByVal hdc As Long) As Long
Private Declare Function DeleteEnhMetaFile Lib "gdi32" (ByVal hemf As Long) As Long
Private Declare Function PlayEnhMetaFile Lib "gdi32" (ByVal hdc As Long, ByVal hemf As Long, lpRect As RECT) As Long

'Non documented functions
Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (pDst As Any, pSrc As Any, ByVal ByteLen As Long)

'Create a vector of points
Private hdc As Long
Private hMeta As Long
Private BoundingRect As RECT


'If filename is "" then metafile is created only in memory, not saved to a file
Public Sub CreateFile(filename As String, Optional widthMM As Long = 250, Optional heightMM As Long = 185, Optional Description)
  If Len(filename) > 0 Then
    If UCase(Right(filename, 4)) <> ".EMF" Then
      filename = filename & ".EMF"
    End If
  End If
  With BoundingRect
    .Top = 0
    .Left = 0
    .Right = widthMM * 100
    .Bottom = heightMM * 100
  End With
  If Description Is missing Then
    Description = App.EXEName & Chr(0) & "clsMetafile"
  End If
  hdc = CreateEnhMetaFile(0, filename, BoundingRect, Description & Chr(0) & Chr(0))

End Sub

Public Sub CloseFile()
  If hdc > 0 Then hMeta = CloseEnhMetaFile(hdc)
  hdc = 0
End Sub

Public Sub Line(X1 As Long, Y1 As Long, X2 As Long, Y2 As Long)
  Dim OldPoint As POINTAPI
  MoveToEx hdc, X1, Y1, OldPoint
  LineTo hdc, X2, Y2
End Sub

Public Sub LineTo(X2 As Long, Y2 As Long)
  LineTo hdc, X2, Y2
End Sub

Public Sub Play(toDC As Long)
  If hMeta > 0 Then PlayEnhMetaFile toDC, hMeta, BoundingRect
End Sub

Private Sub Class_Terminate()
  CloseFile
  If hMeta > 0 Then DeleteEnhMetaFile hMeta
End Sub
