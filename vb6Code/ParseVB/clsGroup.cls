VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsGroup"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Implements clsVBitem

Private pPath As String
Private pName As String
Private pItems As Collection
Private pStartupIndex As Long
Private pFirstLine As String

Public Property Get clsVBitem_Path() As String
  clsVBitem_Path = pPath
End Property
Public Property Let clsVBitem_Path(newValue As String)
  pPath = newValue
  clsVBitem_Name = FilenameNoPath(newValue)
  clsVBitem_Body = GetFileString(pPath)
End Property

Private Property Get clsVBitem_Name() As String
  clsVBitem_Name = pName
End Property
Private Property Let clsVBitem_Name(newValue As String)
  pName = newValue
End Property

'Private Property Get clsVBitem_NextLevelEntry() As String
'  clsVBitem_NextLevelEntry = pName
'End Property
'Private Property Let clsVBitem_NextLevelEntry(newValue As String)
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
  Dim retval As String, index As Integer
  retval = pFirstLine & vbCrLf
  For index = 1 To pItems.Count
    If pItems(index).VBItype = vbi_Project Then
      retval = retval & RelativeFilename(pItems(index).path, PathNameOnly(pPath)) & vbCrLf
    Else
      retval = retval & pItems(index).Name & vbCrLf
    End If
  Next
  clsVBitem_Body = retval
End Property
Private Property Let clsVBitem_Body(newValue As String)
  Dim buf As String
  Dim vbi As clsVBitem
  Dim newDir As String, lastDir As String
  If Left(newValue, 7) <> "VBGROUP" Then
    If MsgBox("This group file does not begin with the expected string 'VBGROUP'" & vbLf & "Attempt to parse anyway?", vbOKCancel, "Group Format Problem") = vbOK Then GoTo ParseGroup
  Else
ParseGroup:
    While Len(newValue) > 0
      buf = VBnextLine(newValue)
      If Left(buf, 15) = "StartupProject=" Then
        pStartupIndex = pItems.Count + 1
        Set vbi = New clsProject
        vbi.path = AbsolutePath(Mid(buf, 16), PathNameOnly(pPath))
      ElseIf Left(buf, 8) = "Project=" Then
        Set vbi = New clsProject
        'newDir = Trim(PathNameOnly(Mid(buf, 9)))
        'If newDir <> "" Then
        '  lastDir = CurDir
        '  ChDir newDir
        'End If
        vbi.path = AbsolutePath(Mid(buf, 9), PathNameOnly(pPath))
        'If newDir <> "" Then ChDir lastDir
      ElseIf Left(buf, 7) = "VBGROUP" Then
        pFirstLine = buf
        GoTo ParseGroup 'Don't add to pItems
      ElseIf Len(Trim(buf)) > 0 Then
        Set vbi = New clsVBitem
        vbi.Body = buf
        vbi.Name = buf
      End If
      'vbi.NextLevelEntry = buf
      If Not vbi Is Nothing Then pItems.Add vbi
      Set vbi = Nothing
    Wend
  End If
End Property

Private Sub clsVBitem_clear()
  Set pItems = Nothing
  Set pItems = New Collection
  pStartupIndex = 0
End Sub

Private Sub Class_Initialize()
  clsVBitem_clear
End Sub

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = vbi_Group
End Property
