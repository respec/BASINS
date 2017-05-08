VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCTypes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Private Type ATCdetailEXEtype
  label As String
  Value As String
  filter As String
  isFile As Boolean
  isOutput As Boolean
  isOnCommandline As Boolean
End Type

Private Type ATCexternalEXEtype
  label As String
  Path As String
  details As Collection
End Type

Private externalEXE() As ATCexternalEXEtype
Private nExternalEXE As Long

Public Function AddEXE(label$, Path$) As Long
  nExternalEXE = nExternalEXE + 1
  ReDim Preserve externalEXE(1 To nExternalEXE)
  externalEXE(nExternalEXE).label = label
  externalEXE(nExternalEXE).Path = Path
  Set externalEXE(nExternalEXE).details = New Collection
  AddEXE = nExternalEXE
End Function

Public Property Get nEXE() As Long
  nEXE = nExternalEXE
End Property

Public Property Get EXEpath(exeIndex&) As String
  If exeIndex > 0 And exeIndex <= nExternalEXE Then
    EXEpath = externalEXE(exeIndex).Path
  Else
    EXEpath = ""
  End If
End Property

Public Property Get EXElabel(exeIndex&) As String
  If exeIndex > 0 And exeIndex <= nExternalEXE Then
    EXElabel = externalEXE(exeIndex).label
  Else
    EXElabel = ""
  End If
End Property

Public Property Get EXEdetails(exeIndex&) As Collection
  If exeIndex > 0 And exeIndex <= nExternalEXE Then
    Set EXEdetails = externalEXE(exeIndex).details
  End If
End Property

Public Sub AddDetail(label$, Value$, filter$, isFile As Boolean, isOutput As Boolean, isOnCommandline As Boolean)
  Dim detail As ATCdetailEXEtype
  detail.label = label
  detail.Value = Value
  detail.filter = filter
  detail.isFile = isFile
  detail.isOutput = isOutput
  detail.isOnCommandline = isOnCommandline
  externalEXE(nExternalEXE).details.Add detail
End Sub

Private Sub Class_Initialize()
  ReDim externalEXE(1 To 1)
End Sub
