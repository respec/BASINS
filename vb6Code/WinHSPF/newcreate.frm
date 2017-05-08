VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form Form1 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Create Project"
   ClientHeight    =   3330
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   8040
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   8.25
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "newcreate.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3330
   ScaleWidth      =   8040
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOkayCancel 
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
      Height          =   375
      Index           =   0
      Left            =   3000
      TabIndex        =   15
      Top             =   2880
      Width           =   852
   End
   Begin VB.CommandButton cmdOkayCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Index           =   1
      Left            =   4200
      TabIndex        =   14
      Top             =   2880
      Width           =   852
   End
   Begin VB.Frame fraScheme 
      Caption         =   "Segment Scheme"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1095
      Left            =   120
      TabIndex        =   11
      Top             =   1680
      Width           =   4215
      Begin VB.OptionButton opnScheme 
         Caption         =   "Group Land Segments within Met Segment"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   120
         TabIndex        =   13
         Top             =   360
         Value           =   -1  'True
         Width           =   3975
      End
      Begin VB.OptionButton opnScheme 
         Caption         =   "Individual Land Segments for each Reach"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   1
         Left            =   120
         TabIndex        =   12
         Top             =   600
         Width           =   3975
      End
   End
   Begin VB.Frame fraFiles 
      Caption         =   "Files"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1455
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   7815
      Begin VB.ListBox lstWDM 
         Appearance      =   0  'Flat
         BackColor       =   &H00FFFFFF&
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   225
         Left            =   3480
         TabIndex        =   5
         Top             =   600
         Width           =   4212
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Index           =   1
         Left            =   120
         TabIndex        =   4
         Top             =   600
         Width           =   735
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Index           =   2
         Left            =   120
         TabIndex        =   3
         Top             =   240
         Width           =   732
      End
      Begin VB.CommandButton cmdFile 
         Caption         =   "Select"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Index           =   0
         Left            =   120
         TabIndex        =   2
         Top             =   960
         Width           =   732
      End
      Begin VB.Label lblName 
         Caption         =   "Met WDM Files"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   4
         Left            =   1200
         TabIndex        =   10
         Top             =   600
         Width           =   1935
      End
      Begin VB.Label lblName 
         Caption         =   "Output WDM File"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   1200
         TabIndex        =   9
         Top             =   960
         Width           =   1935
      End
      Begin VB.Label lblName 
         Caption         =   "BASINS Watershed File"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   1200
         TabIndex        =   8
         Top             =   240
         Width           =   2175
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   2
         Left            =   3480
         TabIndex        =   7
         Top             =   240
         Width           =   4215
      End
      Begin VB.Label lblFile 
         Caption         =   "<none>"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   0
         Left            =   3480
         TabIndex        =   6
         Top             =   960
         Width           =   4215
      End
   End
   Begin VB.Frame fraMet 
      Caption         =   "Met Data"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1095
      Left            =   4440
      TabIndex        =   0
      Top             =   1680
      Width           =   3495
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   7440
      Top             =   2880
      _ExtentX        =   688
      _ExtentY        =   688
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdFile_Click(Index As Integer)
  Dim iwdm&, i&, s$, f$, fun&
  
  If Index = 0 Then
    'output wdm file
    CDFile.Flags = &H8806&
  ElseIf Index = 1 Then
    'met wdm file
    CDFile.Flags = &H1806&
  End If
  If Index = 0 Or Index = 1 Then
    CDFile.Filter = "WDM files (*.wdm)|*.wdm"
    CDFile.filename = "*.wdm"
    If Index = 0 Then
      CDFile.DialogTitle = "Select Output WDM File"
    Else
      CDFile.DialogTitle = "Select Met WDM File"
    End If
    On Error GoTo 40
    CDFile.CancelError = True
    CDFile.Action = 1
    If Index = 1 Then
      'met wdms
      f = CDFile.FileTitle
      If InList(f, lstWDM) Then
        GoTo 25
      Else
        If lstWDM.List(0) = "<none>" Then lstWDM.RemoveItem 0
        If lstWDM.ListCount < 3 Then
          lstWDM.AddItem f
          iwdm = 0
          myUci.openWDM iwdm, f, fun
          If fun < 1 Then
            Call MsgBox("Problem opening the Met WDM file.", _
                        vbOKOnly, "Create Project Problem")
          End If
        Else
          Call MsgBox("No more than three Met WDM files may be included in a project.", _
                      vbOKOnly, "Create Project Problem")
        End If
      End If
25    ' continue here on cancel
    Else
      'output wdm
      lblFile(Index).Caption = CDFile.filename
      'does wdm exist?
      On Error GoTo 20
      Open lblFile(0).Caption For Input As #1
      'yes, it exists
      Close #1
      iwdm = 0
      GoTo 30
20    'no, it does not exist, create it
      iwdm = 2
30    'open wdm file
      myUci.openWDM iwdm, lblFile(0).Caption, fun
      If iwdm = 2 And fun < 1 Then
        Call MsgBox("Problem creating the output WDM file.", _
                      vbOKOnly, "Create Project Problem")
        lblFile(Index).Caption = "<none>"
      ElseIf iwdm = 0 And fun < 1 Then
        Call MsgBox("Problem opening the output WDM file.", _
                      vbOKOnly, "Create Project Problem")
        lblFile(Index).Caption = "<none>"
      End If
    End If
40      'continue here on cancel
  ElseIf Index = 2 Then
    CDFile.Flags = &H8806&
    CDFile.Filter = "BASINS Watershed Files (*.wsd)"
    CDFile.filename = "*.wsd"
    CDFile.DialogTitle = "Select BASINS Watershed File"
    On Error GoTo 50
    CDFile.CancelError = True
    CDFile.Action = 1
    lblFile(Index).Caption = CDFile.filename
50      'continue here on cancel
  End If
End Sub

Private Sub cmdOkayCancel_Click(Index As Integer)
    Dim i&, s$, wdmname$(3)
    
    For i = 1 To 3
      wdmname(i) = ""
    Next i
    For i = 1 To lstWDM.ListCount
      wdmname(i) = lstWDM.List(i - 1)
    Next i
    If Index = 0 Then
      'okay to create new
      If lblFile(2) <> "<none>" Then
        Call HSPFMain.DoCreate(lblFile(2), lblFile(0), wdmname)
        Unload Me
      Else
        Call MsgBox("User must specify a BASINS Watershed File.", _
                      vbOKOnly, "Create Project Problem")
      End If
      'add files to files block
    ElseIf Index = 1 Then 'cancel
      Unload Me
    End If
End Sub

Private Sub Form_Load()
    lstWDM.AddItem "<none>"
    myUci.InitWDMArray
End Sub

Private Sub lblName_Click(Index As Integer)

End Sub


