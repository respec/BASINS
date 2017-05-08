VERSION 5.00
Begin VB.UserControl ctlChecklist 
   ClientHeight    =   1476
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   5088
   ScaleHeight     =   1476
   ScaleWidth      =   5088
   Begin VB.CommandButton cmdBrowse 
      Caption         =   "Browse"
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
      Left            =   3240
      TabIndex        =   3
      Top             =   1200
      Width           =   975
   End
   Begin VB.CommandButton cmdNone 
      Caption         =   "None"
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
      Left            =   2040
      TabIndex        =   2
      Top             =   1200
      Width           =   975
   End
   Begin VB.CommandButton cmdAll 
      Caption         =   "All"
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
      Left            =   840
      TabIndex        =   1
      Top             =   1200
      Width           =   975
   End
   Begin VB.VScrollBar vscroll 
      Height          =   975
      Left            =   4800
      TabIndex        =   0
      Top             =   120
      Visible         =   0   'False
      Width           =   255
   End
   Begin VB.CheckBox chkFile 
      Caption         =   "<name>"
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
      Index           =   0
      Left            =   120
      TabIndex        =   4
      Top             =   120
      Visible         =   0   'False
      Width           =   4575
   End
   Begin MSComDlg.CommonDialog cdAddFile 
      Left            =   4320
      Top             =   1080
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.873e-37
   End
   Begin VB.Label lblNone 
      Caption         =   "No files of this type identified.  Use 'Browse' to add to this list."
      Height          =   252
      Left            =   240
      TabIndex        =   5
      Top             =   240
      Width           =   4332
   End
End
Attribute VB_Name = "ctlChecklist"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim ItemCount&
Dim filtertext$
Dim defaultdir$

Public Sub AddItem(text$, selectValue&)
  lblNone.Visible = False
  If ItemCount > 0 Then
    Load chkFile(ItemCount)
    chkFile(ItemCount).Top = chkFile(ItemCount - 1).Top + chkFile(ItemCount - 1).Height
  End If
  chkFile(ItemCount).Caption = text
  chkFile(ItemCount).Value = selectValue
  chkFile(ItemCount).Visible = True
  ItemCount = ItemCount + 1
End Sub

Public Sub Clear()
  Do Until chkFile.UBound = 0
    Unload chkFile(chkFile.UBound)
  Loop
  chkFile(0).Visible = False
  ItemCount = 0
  lblNone.Visible = True
End Sub

Private Sub cmdAll_Click()
  Dim i&
  For i = 0 To chkFile.UBound
    chkFile(i).Value = 1
  Next i
End Sub

Private Sub cmdBrowse_Click()
  Dim ctemp$
  On Error GoTo errhandler
  ctemp = Dir(defaultdir, vbDirectory)
  If ctemp <> "" And defaultdir <> "" Then
    ChDriveDir defaultdir
  End If
  cdAddFile.DialogTitle = "Browse for Additional Files"
  cdAddFile.CancelError = True
  cdAddFile.flags = &H1804& 'not read only
  cdAddFile.filter = filtertext
  cdAddFile.ShowOpen
  Call AddItem(cdAddFile.Filename, 1)
errhandler:
  On Error Resume Next
End Sub

Private Sub CmdNone_Click()
  Dim i&
  For i = 0 To chkFile.UBound
    chkFile(i).Value = 0
  Next i
End Sub

Private Sub UserControl_Initialize()
  ItemCount = 0
End Sub

Public Property Let FilterString(text$)
  filtertext = text
End Property

Public Property Let DefaultDirectory(text$)
  defaultdir = text
End Property

Public Sub Resize()
  Call UserControl_Resize
End Sub

Private Sub UserControl_Resize()
  Dim i&
  cmdAll.Top = UserControl.Height - cmdAll.Height
  CmdNone.Top = UserControl.Height - CmdNone.Height
  cmdBrowse.Top = UserControl.Height - cmdBrowse.Height
  cmdAll.Left = (UserControl.Width / 2) - cmdAll.Width - cmdAll.Width
  CmdNone.Left = (UserControl.Width / 2) - (CmdNone.Width / 2)
  cmdBrowse.Left = (UserControl.Width / 2) + cmdBrowse.Width
  For i = 0 To chkFile.UBound
    chkFile(i).Width = UserControl.Width
  Next i
  'set scroll bar
  For i = 1 To ItemCount
    chkFile(i - 1).Visible = True
  Next i
  If ItemCount = 0 Then vscroll.Visible = False
  Dim f&, TopRow&, BotRow&, pos&, dy&
  pos = chkFile(0).Top
  dy = chkFile(0).Height
  If vscroll.Visible Then
    f = vscroll.Value
  Else
    f = 0
  End If
  TopRow = f
  BotRow = chkFile.UBound
  While f <= chkFile.UBound
    If chkFile(f).Visible Then
      chkFile(f).Top = pos
      pos = pos + dy
      If pos >= CmdNone.Top Then
        If f <= BotRow Then BotRow = f - 1
      End If
    End If
    f = f + 1
  Wend
  If BotRow < chkFile.UBound Or TopRow > 0 Then 'need scrollbar
    If vscroll.Visible Then
      vscroll.Visible = False
    Else
      vscroll.Value = 0
    End If
    vscroll.Min = 0
    vscroll.Max = chkFile.UBound - (BotRow - TopRow)
    If (BotRow - TopRow > 1) Then
      vscroll.LargeChange = BotRow - TopRow
    Else
      vscroll.LargeChange = 2
    End If
    vscroll.Left = UserControl.Width - vscroll.Width * 1.5
    If CmdNone.Top - vscroll.Top > 100 Then vscroll.Height = CmdNone.Top - vscroll.Top
    vscroll.Visible = True
    For i = BotRow + 1 To chkFile.UBound
      chkFile(i).Visible = False
    Next i
  Else
    vscroll.Visible = False
  End If
End Sub

Public Property Get Count() As Long
  Count = ItemCount
End Property

Public Property Get FileValue(item&) As Long
  FileValue = chkFile(item).Value
End Property

Public Property Let FileValue(item&, val&)
  'Public Sub SetFileOnOff(item&, val&)
  chkFile(item).Value = val
End Property

Public Property Get Filename(item&) As String
  Filename = chkFile(item).Caption
End Property

Private Sub VScroll_Change()
  VScroll_Scroll
End Sub

Private Sub VScroll_Scroll()
  If vscroll.Visible Then
    UserControl_Resize
  End If
End Sub
