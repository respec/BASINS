VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsProject"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Implements clsVBitem

'Private Reg As ATCoRegistry
Private pPath As String
Private pName As String
Private pItems As Collection

Private Sub Class_Initialize()
  clsVBitem_clear
End Sub

Private Sub clsVBitem_clear()
'  Set Reg = Nothing
'  Set Reg = New ATCoRegistry
'  Reg.AppName = "ParseVB"
  Set pItems = Nothing
  Set pItems = New Collection
  pName = ""
'  pNextLevelEntry = ""
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
  Dim retval As String, index As Integer, myPathOnly As String
  myPathOnly = PathNameOnly(pPath)
  For index = 1 To pItems.Count
    Select Case pItems(index).VBItype
      Case vbi_Form:        retval = retval & "Form=" & RelativeFilename(pItems(index).Path, myPathOnly)
      Case vbi_UserControl: retval = retval & "UserControl=" & RelativeFilename(pItems(index).Path, myPathOnly)
      Case vbi_Module:      retval = retval & "Module=" & pItems(index).Name & "; " & RelativeFilename(pItems(index).Path, myPathOnly)
      Case vbi_Class:       retval = retval & "Class=" & pItems(index).Name & "; " & RelativeFilename(pItems(index).Path, myPathOnly)
      Case Else:            retval = retval & pItems(index).Name & vbCrLf
    End Select
  Next
  clsVBitem_Body = retval
End Property
Private Property Let clsVBitem_Body(newValue As String)
  Dim vbi As clsVBitem, buf As String, RegKey As String, newName As String
  Dim EqualPos As Long, SemiPos As Long
  Dim LHS As String, RHS As String 'Left/Right Hand Side of a line in a .vbp
  Dim CLSID As String, myPathOnly As String
  myPathOnly = PathNameOnly(pPath)
  
  Dim ProjectInDir As String, WasInDir As String
  ProjectInDir = PathNameOnly(pPath)
  If ProjectInDir <> "" Then
    WasInDir = CurDir
    ChDir ProjectInDir
  End If
  
  While Len(newValue) > 0
    buf = VBnextLine(newValue)
    EqualPos = Len(buf)
    While EqualPos > 0
      If Asc(Right(buf, 1)) < 33 Then
        buf = Left(buf, Len(buf) - 1)
        EqualPos = EqualPos - 1
      Else
        EqualPos = 0
      End If
    Wend
    EqualPos = InStr(buf, "=")
    If EqualPos < 1 Then
      GoSub DefaultItem
    Else
      LHS = Left(buf, EqualPos - 1)
      RHS = Mid(buf, EqualPos + 1)
      Select Case LHS
        Case "Form", "UserControl"
          Set vbi = New clsForm
          vbi.Path = AbsolutePath(RelativeFilename(RHS, myPathOnly), myPathOnly)
          'vbi.NextLevelEntry = LHS & "=" & vbi.Name
        Case "Module", "Class", "Object"
          If LHS = "Object" Then
            GoSub DefaultItem
          Else
            Set vbi = New clsModule
          End If
          newName = RHS
          SemiPos = InStr(RHS, ";")
          If SemiPos > 0 Then
            If Left(RHS, 1) = "{" Then
'              'CLSID = Left(RHS, 38)
'              RegKey = "TypeLib\" & ReplaceString(Left(RHS, SemiPos - 1), "#", "\") & "\win32"
'              newName = Reg.RegGetString(HKEY_CLASSES_ROOT, RegKey, "")
              vbi.Name = Mid(RHS, SemiPos + 2)
            Else
              vbi.Name = Left(RHS, SemiPos - 1)
              newName = ""
            End If
            If newName = "" Then newName = Mid(RHS, SemiPos + 2)
          End If
          vbi.Path = AbsolutePath(RelativeFilename(newName, myPathOnly), myPathOnly)
'          vbi.NextLevelEntry = buf
'        Case "Reference"
'          If Left(RHS, 4) = "*\G{" And Mid(RHS, 40) = "}" Then
'            CLSID = Mid(RHS, 5, 39)
'          End If
        Case "Name"
          pName = Mid(RHS, 2, Len(RHS) - 2) 'Strip quotes
          GoSub DefaultItem
        Case Else
          GoSub DefaultItem
      End Select
    End If
    If vbi.Path = "" Then vbi.Path = pPath
    'vbi.NextLevelEntry = buf
    pItems.Add vbi
    Set vbi = Nothing
  Wend
  If ProjectInDir <> "" Then ChDir WasInDir
Exit Property

DefaultItem:
  Set vbi = New clsVBitem
  vbi.Name = buf
  vbi.Body = buf
  'vbi.NextLevelEntry = buf
  Return
End Property

Private Property Get clsVBitem_VBItype() As VBitemType
  clsVBitem_VBItype = vbi_Project
End Property

