VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsForm"
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
    Case "frm": pType = vbi_Form
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
'  IndentLen = 0
'  If pBody = "" Then pBody = GetFileString(pPath)
'  clsVBitem_Body = pBody
''  Dim retval As String, Index As Integer
''  retval = pBody & "----------------------" & vbCrLf
''  For Index = 1 To pItems.Count
''    retval = retval & pItems(Index).Body & vbCrLf
''  Next
''  clsVBitem_Body = retval
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
'  Dim vbi As clsVBitem
'  Dim buf As String, delimPos As Long, indent As String
'  Dim LHS As String
'  pBody = ""
'  pBody = newValue
'  While Len(pBody) > 0
'    buf = VBnextLine(pBody)
'    LHS = LTrim(buf)
'    indent = Left(buf, Len(buf) - Len(LHS))
'    delimPos = InStr(LHS, " ")
'    If delimPos > 0 Then LHS = Left(LHS, delimPos - 1)
'    Select Case LHS
'      Case "Begin"
'        Set vbi = New clsFormObject
'        vbi.Body = buf & vbCrLf & newValue
'        newValue = Mid(newValue, Len(vbi.Body))
'        pItems.Add vbi
'        Set vbi = Nothing
'      Case Else
'        pBody = pBody & buf & vbCrLf
'    End Select
'  Wend
End Property

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = pType
End Property


