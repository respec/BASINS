VERSION 5.00
Begin VB.Form frmRun 
   Caption         =   "Run"
   ClientHeight    =   1632
   ClientLeft      =   1056
   ClientTop       =   1980
   ClientWidth     =   8580
   Icon            =   "Run.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   1632
   ScaleWidth      =   8580
   Begin VB.CommandButton cmdAddArg 
      Caption         =   "&Add Argument"
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
      Left            =   120
      TabIndex        =   5
      Top             =   1080
      Visible         =   0   'False
      Width           =   1695
   End
   Begin VB.TextBox txtEditLabel 
      Height          =   285
      Left            =   960
      TabIndex        =   2
      Top             =   840
      Visible         =   0   'False
      Width           =   1335
   End
   Begin VB.Frame fraArg 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   375
      Index           =   0
      Left            =   120
      TabIndex        =   7
      Top             =   240
      Width           =   8415
      Begin VB.ComboBox comType 
         Height          =   315
         Index           =   0
         ItemData        =   "Run.frx":0442
         Left            =   2020
         List            =   "Run.frx":044F
         Style           =   2  'Dropdown List
         TabIndex        =   3
         Top             =   0
         Visible         =   0   'False
         Width           =   630
      End
      Begin VB.TextBox txtArg 
         Height          =   285
         Index           =   0
         Left            =   2640
         Locked          =   -1  'True
         TabIndex        =   4
         Top             =   0
         Width           =   5655
      End
      Begin VB.CommandButton cmdView 
         Caption         =   "&View"
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
         Index           =   0
         Left            =   0
         TabIndex        =   0
         Top             =   0
         Visible         =   0   'False
         Width           =   732
      End
      Begin VB.Label lblArg 
         BackStyle       =   0  'Transparent
         Caption         =   "Program:"
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
         Left            =   960
         TabIndex        =   1
         Top             =   0
         Width           =   975
      End
   End
   Begin VB.CommandButton cmdRun 
      Caption         =   "&Run"
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
      Left            =   2400
      TabIndex        =   6
      Top             =   1080
      Width           =   975
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
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
      Left            =   3720
      TabIndex        =   8
      Top             =   1080
      Width           =   975
   End
   Begin MSComDlg.CommonDialog cdl 
      Left            =   120
      Top             =   840
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   3.09771e-37
   End
End
Attribute VB_Name = "frmRun"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private exeNum As Long

Public Sub ConfigureForm(externalEXEnum As Long)
  Dim detail As Variant, arg&
  exeNum = externalEXEnum
  
  'arg 0 is program
  txtArg(0).text = atEXE.ExePath(exeNum)
  'lblArg(0).Caption = "Program"
  'txtArg(0).Locked = True
  'cmdView(0).Visible = False
  'comType(0).Visible = False
  Me.Caption = atEXE.EXElabel(exeNum)
  
  'args 1..n are files and other arguments
  arg = 1
  For Each detail In atEXE.EXEdetails(exeNum)
    If arg >= fraArg.Count Then
      AddFrame
    End If
    lblArg(arg).Caption = detail.label
    If detail.isFile Then
      If detail.isOutput Then comType(arg).ListIndex = 1 Else comType(arg).ListIndex = 0
    Else
      comType(arg).ListIndex = 2
    End If
    comType(arg).Locked = True
    cmdView(arg).Visible = detail.isFile
    txtArg(arg).Locked = detail.isFile
    txtArg(arg).text = detail.value
    txtArg(arg).Tag = detail.filter
    arg = arg + 1
  Next detail
  While arg < fraArg.Count And fraArg.Count > 1
    Unload fraArg(fraArg.Count - 1)
    Unload lblArg(lblArg.Count - 1)
    Unload cmdView(cmdView.Count - 1)
    Unload txtArg(txtArg.Count - 1)
  Wend
  Form_Resize
End Sub

Private Sub AddFrame()
  Dim arg&
  arg = fraArg.Count
  Load fraArg(arg)
  fraArg(arg).Top = fraArg(arg - 1).Top + fraArg(arg).Height
  fraArg(arg).Left = fraArg(arg - 1).Left
  fraArg(arg).Visible = True
  Load lblArg(arg):  Set lblArg(arg).Container = fraArg(arg)
  Load txtArg(arg):  Set txtArg(arg).Container = fraArg(arg)
  Load cmdView(arg): Set cmdView(arg).Container = fraArg(arg)
  Load comType(arg): Set comType(arg).Container = fraArg(arg)
  lblArg(arg).Visible = True
  txtArg(arg).Visible = True
  cmdView(arg).Visible = True
  comType(arg).Visible = True
  comType(arg).AddItem "In"
  comType(arg).AddItem "Out"
  comType(arg).AddItem "Arg"
  comType(arg).ListIndex = 0
  Form_Resize
End Sub

Private Sub cmdAddArg_Click()
  Dim arg&
  arg = fraArg.Count
  AddFrame
  cmdView(arg).Visible = True
  txtArg(arg).Locked = False
  comType(arg).Locked = False
End Sub

Private Sub cmdCancel_Click()
  Unload Me
  frmGenScn.SetFocus
End Sub

Private Sub cmdRun_Click()
  Dim cmdLine$
  Dim detail As Variant, arg&
  If cmdRun.Caption = "&Run" Then
    cmdLine = txtArg(0).text 'Program
    arg = 1
    For Each detail In atEXE.EXEdetails(exeNum)
      If detail.isOnCommandline Then cmdLine = cmdLine & " " & txtArg(arg).text
      arg = arg + 1
    Next detail
    If MsgBox(cmdLine & vbCrLf & "(This may take some time and there may be no indication of progress.)", vbOKCancel, "Run this command?") = vbOK Then
      Dim dirName$
      If Mid(cmdLine, 2, 1) = ":" Then ChDrive Left(cmdLine, 1)
      dirName = PathNameOnly(txtArg(0).text)
      ChDriveDir dirName
      Me.MousePointer = vbHourglass
      IPC.SendMonitorMessage "(OPEN " & Caption & ")"
      IPC.SendMonitorMessage "(TEXT ON)"
      IPC.SendMonitorMessage "(BUTTOFF CANCEL)"
      IPC.SendMonitorMessage "(BUTTOFF PAUSE)"
      IPC.StartProcess txtArg(0).text, cmdLine, 10
      IPC.SendMonitorMessage "(CLOSE)"
      IPC.SendMonitorMessage "(BUTTON CANCEL)"
      IPC.SendMonitorMessage "(BUTTON PAUSE)"
      Me.MousePointer = vbDefault
    End If
  ElseIf cmdRun.Caption = "OK" Then
    Dim exeIndex&, label$, path$, isFile As Boolean, isOutput As Boolean
    label = lblArg(0).Caption
    path = txtArg(0).text
    exeIndex = atEXE.AddEXE(label, path)
    With frmGenScn
      Load .mnuAnal(.mnuAnal.Count)
      .mnuAnal(.mnuAnal.Count - 1).Caption = label
      .mnuAnal(.mnuAnal.Count - 1).Tag = exeIndex
    End With
    If InStr(UCase(label), "SWMM") > 0 Then SWExeName = path
    If InStr(UCase(label), "STRMDEPL") > 0 Then SDExeName = path

    For arg = 1 To lblArg.Count - 1
      Select Case comType(arg).text
        Case "In":  isFile = True: isOutput = False
        Case "Out": isFile = True: isOutput = True
        Case "Arg": isFile = False: isOutput = False
      End Select
      atEXE.AddDetail lblArg(arg), txtArg(arg), "", isFile, isOutput, True
    Next arg
    MsgBox "Be sure to save your project to save this new definition.", vbOKOnly, "New External Program Defined"
  End If
  cmdCancel_Click
End Sub

Private Sub cmdView_Click(Index As Integer)
  Call DispFile.OpenFile(txtArg(Index).text, lblArg(Index).Caption, frmRun.Icon, False)
End Sub

Private Sub Form_Resize()
  Dim arg&, fraWidth&, txtWidth&
  fraWidth = Width - 285
  If fraWidth < 3000 Then fraWidth = 3000
  txtWidth = fraWidth - 2760
  For arg = 0 To fraArg.Count - 1
    fraArg(arg).Width = fraWidth
    txtArg(arg).Width = txtWidth
  Next arg
  cmdRun.Top = fraArg(fraArg.Count - 1).Top + fraArg(0).Height + 375
  cmdCancel.Top = cmdRun.Top
  cmdAddArg.Top = cmdRun.Top
  Me.Height = cmdRun.Top + 950
End Sub

Private Sub lblArg_Click(Index As Integer)
  If cmdRun.Caption = "OK" Then
    txtEditLabel.Tag = Index
    txtEditLabel.text = lblArg(Index).Caption
    Set txtEditLabel.Container = lblArg(Index).Container
    txtEditLabel.Left = lblArg(Index).Left
    txtEditLabel.Width = lblArg(Index).Width
    txtEditLabel.Top = lblArg(Index).Top
    txtEditLabel.Visible = True
  End If
End Sub

Private Sub txtArg_Click(Index As Integer)
  If txtArg(Index).Locked Then SelectFile Index
End Sub

Private Sub txtArg_DblClick(Index As Integer)
  SelectFile Index
End Sub

Private Sub txtArg_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
  If txtArg(Index).Locked Then SelectFile Index
End Sub

Private Sub SelectFile(ByVal arg&)
  cdl.flags = &H1800&
  cdl.filter = txtArg(arg).Tag
  cdl.Filename = txtArg(arg).text
  cdl.DialogTitle = lblArg(arg).Caption
  On Error GoTo 40
  cdl.CancelError = True
  If arg = 0 Or comType(arg).text = "In" Then
    cdl.Action = 1 'Open
  Else
    cdl.Action = 2 'Save As
  End If
  txtArg(arg).text = cdl.Filename
40        'continue here on cancel
End Sub

Private Sub txtEditLabel_Change()
  lblArg(txtEditLabel.Tag).Caption = txtEditLabel.text
End Sub

Private Sub txtEditLabel_KeyPress(KeyAscii As Integer)
  If KeyAscii = 13 Then txtEditLabel.Visible = False
End Sub
