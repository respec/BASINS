VERSION 5.00
Begin VB.Form frmActivityAll 
   Caption         =   "WinHSPF - Edit All Activity"
   ClientHeight    =   5850
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   10305
   HelpContextID   =   38
   LinkTopic       =   "Form1"
   ScaleHeight     =   5850
   ScaleWidth      =   10305
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   495
      Left            =   2640
      TabIndex        =   4
      Top             =   5160
      Width           =   5655
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
         Left            =   4320
         TabIndex        =   8
         Top             =   0
         Width           =   1215
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
         Left            =   2880
         TabIndex        =   7
         Top             =   0
         Width           =   1215
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
         Left            =   1440
         TabIndex        =   6
         Top             =   0
         Width           =   1215
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
         TabIndex        =   5
         Top             =   0
         Width           =   1215
      End
   End
   Begin TabDlg.SSTab SSTabPIR 
      Height          =   4815
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   9855
      _ExtentX        =   17383
      _ExtentY        =   8493
      _Version        =   393216
      TabHeight       =   520
      TabCaption(0)   =   "&Pervious Land"
      TabPicture(0)   =   "frmActivityAll.frx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "fraPerlnd"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "&Impervious Land"
      TabPicture(1)   =   "frmActivityAll.frx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "fraImplnd"
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "&Reaches/Reservoirs"
      TabPicture(2)   =   "frmActivityAll.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "fraRchres"
      Tab(2).ControlCount=   1
      Begin VB.Frame fraRchres 
         BorderStyle     =   0  'None
         Height          =   4335
         Left            =   -74880
         TabIndex        =   3
         Top             =   360
         Width           =   9615
      End
      Begin VB.Frame fraImplnd 
         BorderStyle     =   0  'None
         Height          =   4335
         Left            =   -74880
         TabIndex        =   2
         Top             =   360
         Width           =   9615
      End
      Begin VB.Frame fraPerlnd 
         BorderStyle     =   0  'None
         Height          =   4335
         Left            =   120
         TabIndex        =   1
         Top             =   360
         Width           =   9615
      End
   End
End
Attribute VB_Name = "frmActivityAll"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private WithEvents pCtrl As VBControlExtender
Attribute pCtrl.VB_VarHelpID = -1
Private pChange As Boolean
Private WithEvents iCtrl As VBControlExtender
Attribute iCtrl.VB_VarHelpID = -1
Private iChange As Boolean
Private WithEvents rCtrl As VBControlExtender
Attribute rCtrl.VB_VarHelpID = -1
Private rChange As Boolean

Public Sub init(b As Object, f As Form)
  Dim s$
  
  If b.OpnBlks("RCHRES").Ids.Count > 0 Then
    SSTabPIR.TabEnabled(2) = True
    rChange = False
    s = b.OpnBlks("RCHRES").Tables("ACTIVITY").EditControlName
    Set rCtrl = Controls.Add(s, "ctlRch", Me.fraRchres)
    rCtrl.Visible = False
    Set rCtrl.frm = f
    Set rCtrl.Owner = b.OpnBlks("RCHRES").Tables("ACTIVITY")
    rCtrl.Visible = True
    SSTabPIR.Tab = 2
  Else
    SSTabPIR.TabEnabled(2) = False
  End If
  rChange = False
  
  If b.OpnBlks("IMPLND").Ids.Count > 0 Then
    SSTabPIR.TabEnabled(1) = True
    s = b.OpnBlks("IMPLND").Tables("ACTIVITY").EditControlName
    Set iCtrl = Controls.Add(s, "ctlImp", Me.fraImplnd)
    iCtrl.Visible = False
    Set iCtrl.frm = f
    Set iCtrl.Owner = b.OpnBlks("IMPLND").Tables("ACTIVITY")
    iCtrl.Visible = True
    SSTabPIR.Tab = 1
  Else
    SSTabPIR.TabEnabled(1) = False
  End If
  iChange = False
  
  If b.OpnBlks("PERLND").Ids.Count > 0 Then
    SSTabPIR.TabEnabled(0) = True
    s = b.OpnBlks("PERLND").Tables("ACTIVITY").EditControlName
    Set pCtrl = Controls.Add(s, "ctlPer", Me.fraPerlnd)
    pCtrl.Visible = False
    Set pCtrl.frm = f
    Set pCtrl.Owner = b.OpnBlks("PERLND").Tables("ACTIVITY")
    pCtrl.Visible = True
    SSTabPIR.Tab = 0
  Else
    SSTabPIR.TabEnabled(0) = False
  End If
  pChange = False

End Sub

Private Sub cmdCancel_Click()
  If rChange Or iChange Or pChange Then
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

Private Sub cmdHelp_Click()
  'just do contents for now
  HtmlHelp Me.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Private Sub cmdOK_Click()
  If pChange Or iChange Or rChange Then
    cmdSave_Click
  End If
  Unload Me
End Sub

Private Sub cmdSave_Click()
  If pChange Then
    pCtrl.Save
  End If
  If iChange Then
    iCtrl.Save
  End If
  If rChange Then
    rCtrl.Save
  End If
  If pChange Or iChange Or rChange Then
    cmdSave.Enabled = False
  Else 'should not get here
    MsgBox "No Changes have been made."
  End If
  pChange = False
  iChange = False
  rChange = False
End Sub

Private Sub pctrl_ObjectEvent(info As EventInfo)
  If UCase(info.Name) = "CHANGE" And pCtrl.Visible Then
    pChange = True
    cmdSave.Enabled = True
  End If
End Sub

Private Sub ictrl_ObjectEvent(info As EventInfo)
  If UCase(info.Name) = "CHANGE" And iCtrl.Visible Then
    iChange = True
    cmdSave.Enabled = True
  End If
End Sub

Private Sub rctrl_ObjectEvent(info As EventInfo)
  If UCase(info.Name) = "CHANGE" And rCtrl.Visible Then
    rChange = True
    cmdSave.Enabled = True
  End If
End Sub

Private Sub Form_Load()
  cmdSave.Enabled = False
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 4200 Then Width = 4200
    If Height < 1500 Then Height = 1500
    SSTabPIR.Height = Height - fraButtons.Height - 1000
    
    If SSTabPIR.TabEnabled(0) Then
      fraButtons.Top = Height - fraButtons.Height - 540
      fraButtons.Left = Width / 2 - fraButtons.Width / 2
      SSTabPIR.Width = Width - 600
      fraPerlnd.Width = SSTabPIR.Width - 400
      fraPerlnd.Height = SSTabPIR.Height - 600
      pCtrl.Move 60, 60, Width - 1000, fraButtons.Top - 1200
    End If
    If SSTabPIR.TabEnabled(1) Then
      fraButtons.Top = Height - fraButtons.Height - 540
      fraButtons.Left = Width / 2 - fraButtons.Width / 2
      SSTabPIR.Width = Width - 600
      fraImplnd.Width = SSTabPIR.Width - 400
      fraImplnd.Height = SSTabPIR.Height - 600
      iCtrl.Move 60, 60, Width - 1000, fraButtons.Top - 1200
    End If
    If SSTabPIR.TabEnabled(2) Then
      fraButtons.Top = Height - fraButtons.Height - 540
      fraButtons.Left = Width / 2 - fraButtons.Width / 2
      SSTabPIR.Width = Width - 600
      fraRchres.Width = SSTabPIR.Width - 400
      fraRchres.Height = SSTabPIR.Height - 600
      rCtrl.Move 60, 60, Width - 1000, fraButtons.Top - 1200
    End If
  End If
  
End Sub


