VERSION 5.00
Begin VB.UserControl ATCoSelectList 
   ClientHeight    =   2400
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6672
   ScaleHeight     =   2400
   ScaleWidth      =   6672
   ToolboxBitmap   =   "ATCoSelectList.ctx":0000
   Begin VB.CommandButton cmdMove 
      Height          =   615
      Index           =   1
      Left            =   6120
      Picture         =   "ATCoSelectList.ctx":0312
      Style           =   1  'Graphical
      TabIndex        =   7
      ToolTipText     =   "Move Item Down In List"
      Top             =   1560
      Width           =   375
   End
   Begin VB.CommandButton cmdMove 
      Height          =   615
      Index           =   0
      Left            =   6120
      Picture         =   "ATCoSelectList.ctx":0754
      Style           =   1  'Graphical
      TabIndex        =   6
      ToolTipText     =   "Move Item Up In List"
      Top             =   480
      Width           =   375
   End
   Begin VB.CommandButton cmdMoveAllLeft 
      Caption         =   "<<- Remove All"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2280
      TabIndex        =   4
      Top             =   1800
      Width           =   1455
   End
   Begin VB.CommandButton cmdMoveAllRight 
      Caption         =   "Add All ->>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2280
      TabIndex        =   3
      Top             =   1320
      Width           =   1455
   End
   Begin VB.CommandButton cmdMoveLeft 
      Caption         =   "<- Remove"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2280
      TabIndex        =   2
      Top             =   840
      Width           =   1455
   End
   Begin VB.CommandButton cmdMoveRight 
      Caption         =   "Add ->"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2280
      TabIndex        =   1
      Top             =   360
      Width           =   1455
   End
   Begin VB.ListBox lstRight 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1968
      Left            =   3960
      MultiSelect     =   2  'Extended
      TabIndex        =   5
      Top             =   360
      Width           =   2055
   End
   Begin VB.ListBox lstLeft 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1968
      Left            =   0
      MultiSelect     =   2  'Extended
      TabIndex        =   0
      Top             =   360
      Width           =   2055
   End
   Begin VB.Label lblRight 
      Caption         =   "Selected:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   4080
      TabIndex        =   9
      Top             =   120
      Width           =   1695
   End
   Begin VB.Label lblLeft 
      Caption         =   "Available:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   120
      TabIndex        =   8
      Top             =   120
      Width           =   1695
   End
End
Attribute VB_Name = "ATCoSelectList"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Public Enum ATCoSortMode
  ATCoSortNone = 0
  ATCoSortNumeric = 1
  ATCoSortAlphabetic = 2
End Enum

Public Event Change()
Public Event ItemSelected(selectedtext$)

'Private pSortMode As ATCoSortMode

'butt is either
Public Property Get ButtonVisible(ByVal butt As Variant) As Boolean
  Dim label As String
  label = LCase(butt)
  Select Case butt
    Case "1", "add":        ButtonVisible = cmdMoveRight.Visible
    Case "2", "remove":     ButtonVisible = cmdMoveLeft.Visible
    Case "3", "add all":    ButtonVisible = cmdMoveAllRight.Visible
    Case "4", "remove all": ButtonVisible = cmdMoveAllLeft.Visible
    Case "5", "move up":    ButtonVisible = cmdMove(0).Visible
    Case "6", "move down":  ButtonVisible = cmdMove(1).Visible
  End Select
End Property
Public Property Let ButtonVisible(ByVal butt As Variant, ByVal NewValue As Boolean)
  Dim label As String
  label = LCase(butt)
  Select Case butt
    Case "1", "add", "move right":           cmdMoveRight.Visible = NewValue
    Case "2", "remove", "move left":         cmdMoveLeft.Visible = NewValue
    Case "3", "add all", "move all right":   cmdMoveAllRight.Visible = NewValue
    Case "4", "remove all", "move all left": cmdMoveAllLeft.Visible = NewValue
    Case "5", "move up":                     cmdMove(0).Visible = NewValue
    Case "6", "move down":                   cmdMove(1).Visible = NewValue
  End Select
End Property

'Public Property Get SortMode() As ATCoSortMode
'  SortMode = pSortMode
'End Property
'Public Property Let SortMode(ByVal NewValue As ATCoSortMode)
'  If pSortMode <> SortMode Then
'    pSortMode = SortMode
'    If SortMode = ATCoSortNone Then
'      lstLeft.Sorted = False
'      lstRight.Sorted = False
'      cmdMove(0).Visible = True
'      cmdMove(1).Visible = True
'    Else
'      lstLeft.Sorted = True
'      lstRight.Sorted = True
'      cmdMove(0).Visible = False
'      cmdMove(1).Visible = False
'    End If
'  End If
'End Property

Public Property Get RightItem(ByVal i&) As String
  If i >= 0 And i < lstRight.ListCount Then
    RightItem = lstRight.List(i)
  Else
    RightItem = ""
  End If
End Property

Public Property Get LeftItem(ByVal i&) As String
  If i >= 0 And i < lstLeft.ListCount Then
    LeftItem = lstLeft.List(i)
  Else
    LeftItem = ""
  End If
End Property

Public Property Let RightItem(ByVal i&, ByVal NewValue$)
  If Not InRightList(NewValue) Then
    If i = lstRight.ListCount Then lstRight.AddItem NewValue
    If i >= 0 And i < lstRight.ListCount Then lstRight.List(i) = NewValue
  End If
End Property

Public Property Let LeftItem(ByVal i&, ByVal NewValue$)
  If Not InLeftList(NewValue) Then
    If i = lstLeft.ListCount Then lstLeft.AddItem NewValue
    If i >= 0 And i < lstLeft.ListCount Then lstLeft.List(i) = NewValue
  End If
End Property

'Used in place of Let LeftItem to speed addition of hundreds of items
'If you are adding fewer than 100 items, use LeftItem property instead
Public Sub LeftItemFastAdd(ByVal NewValue$)
  lstLeft.AddItem NewValue
End Sub

Public Property Get RightItemData&(ByVal i&)
  RightItemData = lstRight.ItemData(i)
End Property

Public Property Get LeftItemData&(ByVal i&)
  LeftItemData = lstLeft.ItemData(i)
End Property

Public Property Let RightItemData(ByVal i&, ByVal NewValue&)
  lstRight.ItemData(i) = NewValue
End Property

Public Property Let LeftItemData(ByVal i&, ByVal NewValue&)
  lstLeft.ItemData(i) = NewValue
End Property

Public Property Get RightCount&()
  RightCount = lstRight.ListCount
End Property

Public Property Get LeftCount&()
  LeftCount = lstLeft.ListCount
End Property

Public Property Get RightLabel$()
  RightLabel = lblRight.Caption
End Property

Public Property Get LeftLabel$()
  LeftLabel = lblLeft.Caption
End Property

Public Property Let RightLabel(ByVal NewValue$)
  lblRight.Caption = NewValue
End Property

Public Property Let LeftLabel(ByVal NewValue$)
  lblLeft.Caption = NewValue
End Property

Public Property Get MoveUpTip$()
  MoveUpTip = cmdMove(0).ToolTipText
End Property

Public Property Get MoveDownTip$()
  MoveDownTip = cmdMove(1).ToolTipText
End Property

Public Property Let MoveUpTip(ByVal NewValue$)
  cmdMove(0).ToolTipText = NewValue
End Property

Public Property Let MoveDownTip(ByVal NewValue$)
  cmdMove(1).ToolTipText = NewValue
End Property

Public Sub MoveRight(ByVal i&)
  If i >= 0 And i < lstLeft.ListCount Then
    lstRight.AddItem lstLeft.List(i)
    lstRight.ItemData(lstRight.ListCount - 1) = lstLeft.ItemData(i)
    lstLeft.RemoveItem i
  End If
  RaiseEvent Change
End Sub

Public Sub MoveLeft(ByVal i&)
  If i >= 0 And i < lstRight.ListCount Then
    lstLeft.AddItem lstRight.List(i)
    lstLeft.ItemData(lstLeft.ListCount - 1) = lstRight.ItemData(i)
    lstRight.RemoveItem i
  End If
  RaiseEvent Change
End Sub

Public Sub MoveAllRight()
  Dim i&
  For i = 0 To lstLeft.ListCount - 1
    lstRight.AddItem lstLeft.List(i)
    lstRight.ItemData(lstRight.ListCount - 1) = lstLeft.ItemData(i)
  Next i
  lstLeft.Clear
  RaiseEvent Change
End Sub

Public Sub MoveAllLeft()
  Dim i&
  For i = 0 To lstRight.ListCount - 1
    lstLeft.AddItem lstRight.List(i)
    lstLeft.ItemData(lstLeft.ListCount - 1) = lstRight.ItemData(i)
  Next i
  lstRight.Clear
  RaiseEvent Change
End Sub

Public Sub ClearRight()
  lstRight.Clear
  RaiseEvent Change
End Sub

Public Sub ClearLeft()
  lstLeft.Clear
  RaiseEvent Change
End Sub

Public Function InRightList(ByVal search$)
  InRightList = InList(search, lstRight)
End Function

Public Function InLeftList(ByVal search$)
  InLeftList = InList(search, lstLeft)
End Function

Private Function InList(ByVal s$, lst As Object) As Boolean
    Dim i&, found As Boolean
    
    i = 0
    found = False
    Do While Not found
      If s = lst.List(i) Then
        found = True
      ElseIf i < lst.ListCount - 1 Then
        i = i + 1
      Else
        Exit Do
      End If
    Loop
    
    InList = found
    
End Function

Private Sub cmdMove_Click(Index As Integer)
  Dim i&, tmp$, tmpData&
  If Index = 0 Then 'Move Up
    For i = 1 To lstRight.ListCount - 1
      If lstRight.Selected(i) And Not lstRight.Selected(i - 1) Then
        
        tmp = lstRight.List(i - 1)
        tmpData = lstRight.ItemData(i - 1)
        
        lstRight.List(i - 1) = lstRight.List(i)
        lstRight.ItemData(i - 1) = lstRight.ItemData(i)
        
        lstRight.List(i) = tmp
        lstRight.ItemData(i) = tmpData
        
        lstRight.Selected(i) = False
        lstRight.Selected(i - 1) = True
        RaiseEvent Change
      End If
    Next i
  ElseIf Index = 1 Then 'Move Down
    For i = lstRight.ListCount - 2 To 0 Step -1
      If lstRight.Selected(i) And Not lstRight.Selected(i + 1) Then
        tmp = lstRight.List(i + 1)
        tmpData = lstRight.ItemData(i + 1)
        
        lstRight.List(i + 1) = lstRight.List(i)
        lstRight.ItemData(i + 1) = lstRight.ItemData(i)
        
        lstRight.List(i) = tmp
        lstRight.ItemData(i) = tmpData
        
        lstRight.Selected(i) = False
        lstRight.Selected(i + 1) = True
        RaiseEvent Change
      End If
    Next i
  End If
End Sub

Private Sub cmdMoveAllLeft_Click()
  MoveAllLeft
End Sub

Private Sub cmdMoveAllRight_Click()
  MoveAllRight
End Sub

Private Sub cmdMoveLeft_Click()
  Dim i&
  i = 0
  While i < lstRight.ListCount
    If lstRight.Selected(i) Then MoveLeft i Else i = i + 1
  Wend
End Sub

Private Sub cmdMoveRight_Click()
  Dim i&
  i = 0
  While i < lstLeft.ListCount
    If lstLeft.Selected(i) Then MoveRight i Else i = i + 1
  Wend
End Sub

Private Sub lstLeft_Click()
  RaiseEvent ItemSelected(lstLeft.List(lstLeft.ListIndex))
End Sub

Private Sub lstRight_Click()
  RaiseEvent ItemSelected(lstRight.List(lstRight.ListIndex))
End Sub

Private Sub lstRight_DblClick()
  MoveLeft lstRight.ListIndex
End Sub

Private Sub lstLeft_DblClick()
  MoveRight lstLeft.ListIndex
End Sub

Private Sub UserControl_Resize()
  Dim UsedWidth&, margin&
  margin = 225
  UsedWidth = cmdMoveRight.Width + cmdMove(0).Width + margin * 3
  If Width - UsedWidth > 1000 Then
    lstLeft.Width = (Width - UsedWidth) / 2
    lblLeft.Width = lstLeft.Width - (lblLeft.Left - lstLeft.Left)
    lstRight.Width = lstLeft.Width
    cmdMoveRight.Left = lstLeft.Left + lstLeft.Width + margin
    cmdMoveLeft.Left = cmdMoveRight.Left
    cmdMoveAllRight.Left = cmdMoveRight.Left
    cmdMoveAllLeft.Left = cmdMoveRight.Left
    lstRight.Left = cmdMoveRight.Left + cmdMoveRight.Width + margin
    lblRight.Left = lstRight.Left + (lblLeft.Left - lstLeft.Left)
    lblRight.Width = lstRight.Width - (lblRight.Left - lstRight.Left)
    cmdMove(0).Left = lstRight.Left + lstRight.Width + margin
    cmdMove(1).Left = cmdMove(0).Left
  End If
  If Height > cmdMoveAllLeft.Top + cmdMoveAllLeft.Height Then
    lstLeft.Height = Height - 400
    lstRight.Height = lstLeft.Height
    cmdMove(1).Top = lstRight.Top + lstRight.Height - cmdMove(1).Height - (cmdMove(0).Top - lstRight.Top)
  End If
End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  RightLabel = PropBag.ReadProperty("RightLabel", "Selected:")
  LeftLabel = PropBag.ReadProperty("LeftLabel", "Available:")
  'SortMode = PropBag.ReadProperty("SortMode", ATCoSortNone)
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
  PropBag.WriteProperty "RightLabel", RightLabel
  PropBag.WriteProperty "LeftLabel", LeftLabel
  'PropBag.WriteProperty "SortMode", SortMode
End Sub

Public Property Get Enabled() As Boolean
  Enabled = lstRight.Enabled
End Property

Public Property Let Enabled(ByVal NewValue As Boolean)
  lstRight.Enabled = NewValue
  lstLeft.Enabled = NewValue
  cmdMoveRight.Enabled = NewValue
  cmdMoveLeft.Enabled = NewValue
  cmdMoveAllRight.Enabled = NewValue
  cmdMoveAllLeft.Enabled = NewValue
  cmdMove(0).Enabled = NewValue
  cmdMove(1).Enabled = NewValue
End Property
