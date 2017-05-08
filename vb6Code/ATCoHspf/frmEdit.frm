VERSION 5.00
Begin VB.Form frmEdit 
   Caption         =   "Edit"
   ClientHeight    =   3165
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7650
   HelpContextID   =   30
   LinkTopic       =   "Form1"
   ScaleHeight     =   3165
   ScaleWidth      =   7650
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   612
      Left            =   120
      TabIndex        =   0
      Top             =   2640
      Width           =   7095
      Begin VB.CommandButton cmdHelp 
         Caption         =   "&Help"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2880
         TabIndex        =   7
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdEdit 
         Caption         =   "&Edit"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Index           =   2
         Left            =   6120
         TabIndex        =   6
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdAddRem 
         Caption         =   "&Remove"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Index           =   1
         Left            =   5040
         TabIndex        =   5
         Top             =   0
         Width           =   972
      End
      Begin VB.CommandButton cmdAddRem 
         Caption         =   "A&dd"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   492
         Index           =   0
         Left            =   4080
         TabIndex        =   4
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Apply"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   1920
         TabIndex        =   3
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   960
         TabIndex        =   2
         Top             =   0
         Width           =   852
      End
      Begin VB.CommandButton cmdOK 
         Caption         =   "&OK"
         Default         =   -1  'True
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   852
      End
   End
End
Attribute VB_Name = "frmEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Private WithEvents ctrl As VBControlExtender
Attribute ctrl.VB_VarHelpID = -1
Private pChange As Boolean

Public Sub init(b As Object, f As Form, addRemFlg As Boolean, _
  Optional editFlg As Boolean = False, _
  Optional applyFlg As Boolean = True)
  
  Set ctrl = Controls.Add(b.EditControlName, "foo")
  ctrl.Visible = False
  Set ctrl.frm = f
  If Not (editFlg) Then 'no edit allowed
    fraButtons.Width = cmdAddRem(1).Left + cmdAddRem(1).Width + 10
  End If
  If Not (addRemFlg) Then 'no add remove allowed (or edit)
    fraButtons.Width = cmdHelp.Left + cmdHelp.Width + 10
  End If
  If Not applyFlg Then
    cmdSave.Visible = False
  End If
  Width = ctrl.Width + 100
  Height = ctrl.Height + 1200
  Caption = Caption & " " & b.Caption
  Set ctrl.Owner = b
'  cmdSave.Top = ctrl.Height + 100
'  cmdOK.Top = cmdSave.Top
'  cmdCancel.Top = cmdSave.Top
  pChange = False
  ctrl.Visible = True
End Sub

Private Sub cmdAddRem_Click(Index As Integer)
  If Index = 0 Then
    ctrl.Add
  ElseIf Index = 1 Then
    ctrl.Remove
  End If
End Sub

Private Sub cmdCancel_Click()
  If pChange Then
    Me.Hide
    If myMsgBox.Show("Do you want to discard changes?", Me.Caption, "&Yes", "-+&No") = 1 Then
      Unload Me
    Else
      Me.Show vbModal
    End If
  Else
    Unload Me
  End If
End Sub

Private Sub cmdEdit_Click(Index As Integer)
  ctrl.Edit
End Sub

Private Sub cmdHelp_Click()
  ctrl.Help
   
'  Dim k$, c$, i&, d As HH_AKLINK, s As HH_FTS_QUERY
  
'  c = Me.Caption
'  k = Right(c, Len(c) - InStrRev(c, ":")) & Mid(c, 6, InStr(c, ":") - 6) & vbNullString
  
'  d.pszKeywords = k
'  d.fReserved = vbFalse
'  d.cbStruct = LenB(d)
'  i = HtmlHelp(Me.hwnd, App.HelpFile, HH_ALINK_LOOKUP, d)
              
'  i = HtmlHelp(Me.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0)
              
'  s.pszSearchQuery = k
'  s.fExecute = vbTrue
'  s.cbStruct = LenB(s)
'  i = HtmlHelp(Me.hwnd, App.HelpFile, HH_DISPLAY_SEARCH, s)

End Sub

Private Sub cmdOK_Click()
  If pChange Then
    cmdSave_Click
  End If
  Unload Me
End Sub

Private Sub cmdSave_Click()
  If pChange Then
    ctrl.Save
    cmdSave.Enabled = False
  Else 'should not get here
    MsgBox "No Changes have been made."
  End If
  pChange = False
End Sub

Private Sub ctrl_ObjectEvent(info As EventInfo)
  If UCase(info.Name) = "CHANGE" And ctrl.Visible Then
    pChange = True
    cmdSave.Enabled = True
  End If
End Sub

Private Sub Form_Load()
  cmdSave.Enabled = False
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < fraButtons.Width Then Width = fraButtons.Width
    If Height < 1500 Then Height = 1500
    If ctrl.Visible Then
      fraButtons.Top = Height - fraButtons.Height - 440
      fraButtons.Left = Width / 2 - fraButtons.Width / 2
      ctrl.Move 60, 60, Width - 200, fraButtons.Top - 200
    End If
  End If
End Sub

