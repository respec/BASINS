VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsLst"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Implements clsVBitem
'Reads and writes Setup.lst

Private pPath As String
Private pName As String
Private pItems As Collection 'Each item is a line from Setup.lst

Private Type SetupLine
  isFile As Boolean
  filename As String
  installpath As String
  selfregister As String
  shared As String
  date As String
  filesize As String
  version As String
End Type
Dim pLines(200) As SetupLine

Private Sub Class_Initialize()
  clsVBitem_clear
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
  clsVBitem_Body = GetFileString(pPath)
End Property

Private Property Get clsVBitem_Name() As String
  clsVBitem_Name = pName
End Property

Private Property Let clsVBitem_Name(newValue As String)
  pName = newValue
End Property

Private Property Get clsVBitem_nItems() As Long
  clsVBitem_nItems = 0 'pItems.Count
End Property

Private Property Get clsVBitem_Item(index As Long) As clsVBitem
  'If index <= pItems.Count Then
  '  Set clsVBitem_Item = pItems(index)
  'End If
End Property

Private Property Get clsVBitem_Body() As String
  Dim retval As String, index As Integer
  Dim nextFileNum As Long
  nextFileNum = 1
  For index = 1 To pItems.Count
    If Not pLines(index).isFile Then
      retval = retval & pItems(index) & vbLf
      nextFileNum = 1
    Else
      retval = retval & "File" & nextFileNum & "=@" _
             & pLines(index).filename & "," _
             & pLines(index).installpath & "," _
             & pLines(index).selfregister & "," _
             & pLines(index).shared & "," _
             & pLines(index).date & "," _
             & pLines(index).filesize & "," _
             & pLines(index).version & vbLf
      nextFileNum = nextFileNum + 1
    End If
  Next
  clsVBitem_Body = retval
End Property
Private Property Let clsVBitem_Body(newValue As String)
  Dim buf As String, index As Long
  Dim EqualPos As Long, CommaPos As Long, LastCommaPos As Long
  Dim LHS As String, RHS As String 'Left/Right Hand Side of a line in a .vbp
  Dim myPathOnly As String
  myPathOnly = PathNameOnly(pPath)
  index = 0
  frmTextBox.text = frmTextBox.text & vbCrLf & "Setting Setup.Lst body---------------------"
  frmTextBox.Visible = True
  While Len(newValue) > 0
    buf = VBnextLine(newValue)
    pItems.Add buf
    index = index + 1
    pLines(index).isFile = False
    If Left(buf, 4) = "File" Then
      EqualPos = InStr(buf, "=@")
      If EqualPos > 0 Then
        pLines(index).isFile = True
        buf = Mid(buf, EqualPos + 2)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).filename = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).installpath = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).selfregister = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).shared = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).date = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        CommaPos = InStr(buf, ",")
        If CommaPos > 0 Then pLines(index).filesize = Left(buf, CommaPos - 1)
        buf = Mid(buf, CommaPos + 1)
        pLines(index).version = buf
        buf = Format(FileDateTime(myPathOnly & "\" & pLines(index).filename), "m/d/yy h:mm:ss AMPM")
        If buf <> pLines(index).date Then
          frmTextBox.text = frmTextBox.text & vbCrLf & pLines(index).filename & ": Date: " & pLines(index).date & " -> " & buf
          pLines(index).date = buf
        End If
        buf = FileLen(myPathOnly & "\" & pLines(index).filename)
        If buf <> pLines(index).filesize Then
          frmTextBox.text = frmTextBox.text & vbCrLf & pLines(index).filename & ": Size: " & pLines(index).filesize & " -> " & buf
          pLines(index).filesize = buf
        End If
      End If
    End If
  Wend
  Debug.Print "New Setup.Lst:------------------------------"
  Debug.Print clsVBitem_Body
End Property

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = vbi_List
End Property


