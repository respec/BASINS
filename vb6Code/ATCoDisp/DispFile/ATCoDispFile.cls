VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoDispFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants
Dim frmDF As frmDispFile

Public Property Get FindString() As String
  FindString = frmDF.txtFind.Text
End Property
Public Property Let FindString(newvalue As String)
  frmDF.txtFind.Text = newvalue
End Property

Public Sub OpenFile(Filename$, Caption$, icon As Object, Editable As Boolean)
  Dim j&, f$, DisplayMode&
  If Len(Trim(Filename)) > 0 And Len(Dir(Filename, vbNormal)) > 0 Then
    If FileLen(Filename) > 0 Then
      j = InStr(Filename, FilenameOnly(Filename)) - 1
      f = Right(Filename, Len(Filename) - j)
      Select Case UCase(Right(f, 3))
        Case "SHP": DisplayMode = 3        'DispShp
        Case "OUT", "ECH": DisplayMode = 1 'DispFortCC
        Case Else:  DisplayMode = 0        'DispText"
      End Select
      frmDF.DispFile Filename, Caption, icon, Editable, DisplayMode
      If Len(frmDF.txtFind) > 0 Then frmDF.cmdFind_Click
    Else
      MsgBox "File (" & Trim(Filename) & ") is Empty: ", vbOKOnly, "Open File"
    End If
  Else
    MsgBox "File (" & Trim(Filename) & ") not found: ", vbOKOnly, "Open File"
  End If
End Sub

Private Sub Class_Initialize()
  Set frmDF = New frmDispFile
End Sub
