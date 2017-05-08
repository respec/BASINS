VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsFormObject"
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
Private pType As String
Private pItems As Collection
Private pProperties As Collection
Private pNextLevelEntry As String

Private Sub Class_Initialize()
  clsVBitem_clear
'  Set Reg = New ATCoRegistry
'  Reg.AppName = "ParseVB"
End Sub

Private Sub clsVBitem_clear()
  Set pItems = Nothing
  Set pItems = New Collection
  
  Set pProperties = Nothing
  Set pProperties = New Collection
  
  pName = ""
End Sub

Public Property Get clsVBitem_Path() As String
  clsVBitem_Path = pPath
End Property
Public Property Let clsVBitem_Path(newValue As String)
  pPath = newValue
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
  Dim retval As String, index As Integer, indent As String
  retval = Space(IndentLen) & "Begin " & pType & " " & pName & vbCrLf
  IndentLen = IndentLen + 3
  indent = Space(IndentLen)
  For index = 1 To pProperties.Count
    retval = retval & indent & pProperties(index) & vbCrLf
  Next
  For index = 1 To pItems.Count
    retval = retval & pItems(index).Body
  Next
  IndentLen = IndentLen - 3
  retval = retval & Space(IndentLen) & "End" & vbCrLf
  clsVBitem_Body = retval
End Property
Private Property Let clsVBitem_Body(newValue As String)
  Dim RemainingNewVal As String
  Dim vbi As clsVBitem
  Dim buf As String, delimPos As Long, indent As String
  Dim SearchFor As String
  Dim LHS As String
  'pBody = newValue
  RemainingNewVal = newValue
  buf = VBnextLine(newValue)
  
  LHS = LTrim(buf)
  IndentLen = Len(buf) - Len(LHS)
  indent = Space(IndentLen)
  If Left(buf, 5) <> "Begin" Then
    MsgBox buf & vbCr & "Form object not properly Begun"
    Exit Property
  End If
  delimPos = InStr(7, buf, " ")
  If delimPos < 1 Then
    MsgBox buf & vbCr & "Form object not properly typed"
    Exit Property
  End If
  pType = Mid(buf, 7, delimPos - 7)
  pName = Trim(Mid(buf, delimPos + 1))
  SearchFor = vbLf & indent & "End"
  delimPos = InStr(RemainingNewVal, SearchFor)
  If delimPos < 1 Then
    MsgBox buf & vbCr & "Not properly terminated"
    Exit Property
  End If
  RemainingNewVal = Left(RemainingNewVal, delimPos + Len(SearchFor))
  
  While Len(RemainingNewVal) > 0
    buf = VBnextLine(newValue)
    If Right(buf, 1) = vbCr Then buf = Left(buf, Len(buf) - 1)
    LHS = LTrim(buf)
    If Len(buf) - Len(LHS) > IndentLen Then
      delimPos = InStr(LHS, " ")
    Else
      delimPos = 0
    End If
    If delimPos > 0 Then LHS = Left(LHS, delimPos - 1)
    Select Case LHS
      Case "Begin"
        Set vbi = New clsFormObject
        vbi.Body = buf & vbCrLf & RemainingNewVal
        newValue = Mid(RemainingNewVal, Len(vbi.Body))
        pItems.Add vbi
        Set vbi = Nothing
      Case "End"
        RemainingNewVal = ""
      Case Else
        pProperties.Add LTrim(buf)
    End Select
  Wend
End Property

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = vbi_FormObject
End Property

