VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsModule"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Implements clsVBitem

'Private Reg As ATCoRegistry
Private pPath As String
Private pName As String
Private pBody As String
Private pType As VBitemType
Private pItems As Collection
Private pNextLevelEntry As String

Private Sub Class_Initialize()
  clsVBitem_clear
'  Set Reg = New ATCoRegistry
'  Reg.AppName = "ParseVB"
End Sub

Private Sub clsVBitem_clear()
  Set pItems = Nothing
  Set pItems = New Collection
  pName = ""
End Sub

Public Property Get clsVBitem_Path() As String
  clsVBitem_Path = pPath
End Property
Public Property Let clsVBitem_Path(newValue As String)
  pPath = newValue
  Select Case LCase(Right(newValue, 3))
    Case "bas": pType = vbi_Module
    Case "cls": pType = vbi_Class
    Case "ctl": pType = vbi_UserControl
  End Select
  clsVBitem_Body = GetFileString(newValue)
End Property

Private Property Get clsVBitem_Name() As String
  clsVBitem_Name = pName
End Property
Private Property Let clsVBitem_Name(newValue As String)
  pName = newValue
End Property

'Private Property Get clsVBitem_NextLevelEntry() As String
'  clsVBitem_NextLevelEntry = pNextLevelEntry
'End Property
'Private Property Let clsVBitem_NextLevelEntry(newValue As String)
'  pNextLevelEntry = newValue
'End Property

Private Property Get clsVBitem_nItems() As Long
  clsVBitem_nItems = pItems.Count
End Property

Private Property Get clsVBitem_Item(index As Long) As clsVBitem
  If index <= pItems.Count Then
    Set clsVBitem_Item = pItems(index)
  End If
End Property

Private Property Get clsVBitem_Body() As String
  clsVBitem_Body = GetFileString(pPath)
'  Dim retval As String, Index As Integer
'  For Index = 1 To pItems.Count
'    retval = retval & pItems(Index).Body
'  Next
'  clsVBitem_Body = retval
End Property
Private Property Let clsVBitem_Body(newValue As String)
  Dim NamePos As Long, endquotePos As Long
  NamePos = InStr(newValue, "Attribute VB_Name = ")
  If NamePos > 0 Then
    NamePos = NamePos + 21
    endquotePos = InStr(NamePos, newValue, """")
    If endquotePos > 0 Then
      pName = Mid(newValue, NamePos, endquotePos - NamePos)
    End If
  End If
End Property

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = pType
End Property

