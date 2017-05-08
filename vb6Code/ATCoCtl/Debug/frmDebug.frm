VERSION 5.00
Begin VB.Form frmDebug 
   Caption         =   "Debug"
   ClientHeight    =   5964
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7428
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5964
   ScaleWidth      =   7428
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraOptions 
      Caption         =   "Options"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1455
      Left            =   120
      TabIndex        =   8
      Top             =   4440
      Width           =   7155
      Begin VB.ListBox ListType 
         Height          =   912
         Left            =   240
         Style           =   1  'Checkbox
         TabIndex        =   1
         Top             =   360
         Width           =   1815
      End
      Begin VB.CheckBox CheckSave 
         Caption         =   "Continuous Save"
         Height          =   252
         Left            =   5160
         TabIndex        =   6
         Top             =   840
         Width           =   1575
      End
      Begin VB.TextBox txtLev 
         Alignment       =   1  'Right Justify
         Height          =   252
         Left            =   4320
         TabIndex        =   2
         Text            =   "3"
         Top             =   420
         Width           =   312
      End
      Begin VB.TextBox txtFlsh 
         Alignment       =   1  'Right Justify
         Height          =   252
         Left            =   4320
         TabIndex        =   3
         Text            =   "8"
         Top             =   780
         Width           =   312
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Save"
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
         Left            =   6000
         TabIndex        =   5
         Top             =   360
         Width           =   675
      End
      Begin VB.CommandButton cmdClear 
         Caption         =   "&Clear"
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
         Left            =   5160
         TabIndex        =   4
         Top             =   360
         Width           =   675
      End
      Begin VB.Label lblLev 
         Alignment       =   1  'Right Justify
         Caption         =   "Display Entries <= Level"
         Height          =   255
         Left            =   2280
         TabIndex        =   10
         Top             =   480
         Width           =   1875
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         Caption         =   "Flush New Entries > Level"
         Height          =   255
         Left            =   2160
         TabIndex        =   9
         Top             =   780
         Width           =   1995
      End
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   6540
      Top             =   0
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      DialogTitle     =   "Save Debug File as"
   End
   Begin VB.TextBox txtDetails 
      BackColor       =   &H8000000B&
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   7.8
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3912
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   0
      TabStop         =   0   'False
      Top             =   420
      Width           =   7095
   End
   Begin VB.Label lblDetails 
      Caption         =   "Time, Module, Type:Level:Msg"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   7
      Top             =   120
      Width           =   4632
   End
End
Attribute VB_Name = "frmDebug"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Private Sub CheckSave_Click()
  If instSave Then
    Close #101
  Else
    cdlg.Filename = frmDebug.Caption & ".txt"
    cdlg.ShowSave
    Open cdlg.Filename For Output As #101
  End If
  instSave = Not instSave
  CheckSave.Value = Switch(instSave, 1, Not instSave, 0)
End Sub

Private Sub cmdClear_Click()
  Dim j&
  For j = 0 To UBound(d)
    d(j) = dnull
  Next j
  p = 0
  ReDo True
End Sub

Private Sub cmdSave_Click()
  Dim j&, s$, X$
  
  cdlg.Filename = frmDebug.Caption & ".txt"
  cdlg.DialogTitle = "Save Debug File as"
  cdlg.ShowSave
  Open cdlg.Filename For Output As #102
  For j = p - 1 To 0 Step -1
    GoSub AddToBuff
  Next j
  For j = UBound(d) To p Step -1
    GoSub AddToBuff
  Next j
  Close #102
  Exit Sub
  
AddToBuff:
  If Len(d(j).msg) > 0 And lev >= d(j).lev Then
    s = BldDebugRec(d(j))
    If X <> s Then
      Print #102, s
    End If
    X = s
  End If
  Return
  
End Sub

Private Sub Form_Resize()
  If WindowState = vbNormal Then
    If frmDebug.Width < fraOptions.Width + 300 Then
      frmDebug.Width = fraOptions.Width + 300
    End If
    If frmDebug.Height < fraOptions.Height + txtDetails.Top + 640 Then
      frmDebug.Height = fraOptions.Height + txtDetails.Top + 640
    End If
  End If
  txtDetails.Width = frmDebug.Width - 300
  fraOptions.Top = frmDebug.Height - fraOptions.Height - 360
  txtDetails.Height = fraOptions.Top - 480
End Sub

'Private Sub optInstSave_Click(Index As Integer)
'  If Index = 0 Then
'    If Not (instSave) Then
'      cdlg.Filename = frmDebug.Caption & ".txt"
'      cdlg.DialogTitle = "Save Debug File as"
'      cdlg.ShowSave
'      Open cdlg.Filename For Output As #101
'      instSave = True
'    End If
'  Else
'    If instSave Then
'      Close #101
'      instSave = False
'    End If
'  End If
'End Sub

Private Sub ListType_Click()
  ReDo False
End Sub

Private Sub txtFlsh_Change()
  If IsNumeric(txtFlsh.Text) Then
    flsh = txtFlsh.Text
  Else
    Beep
    txtFlsh.Text = flsh
  End If
End Sub

Private Sub txtLev_Change()
  If IsNumeric(txtLev.Text) Then
    lev = txtLev.Text
    ReDo True
  Else
    Beep
    txtLev.Text = lev
  End If
End Sub
