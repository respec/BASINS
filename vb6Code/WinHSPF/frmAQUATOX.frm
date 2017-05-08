VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Begin VB.Form frmAQUATOX 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - AQUATOX Linkage"
   ClientHeight    =   1992
   ClientLeft      =   36
   ClientTop       =   264
   ClientWidth     =   7728
   Icon            =   "frmAQUATOX.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1992
   ScaleWidth      =   7728
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOkayCancel 
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
      Index           =   1
      Left            =   4440
      TabIndex        =   9
      Top             =   1440
      Width           =   852
   End
   Begin VB.CommandButton cmdOkayCancel 
      Caption         =   "&Start AQUATOX"
      Default         =   -1  'True
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
      Index           =   0
      Left            =   2160
      TabIndex        =   8
      Top             =   1440
      Width           =   2052
   End
   Begin VB.ComboBox cboRchres 
      Height          =   288
      Left            =   3360
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   960
      Width           =   2172
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
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
      Left            =   120
      TabIndex        =   3
      Top             =   600
      Width           =   732
   End
   Begin VB.CommandButton cmdFile 
      Caption         =   "Select"
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
      Index           =   2
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   732
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   7320
      Top             =   480
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin VB.Label lblRchres 
      Caption         =   "Select RCHRES to Link:"
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
      TabIndex        =   6
      Top             =   1080
      Width           =   2412
   End
   Begin VB.Label lblFile 
      Appearance      =   0  'Flat
      BackColor       =   &H80000004&
      BorderStyle     =   1  'Fixed Single
      Caption         =   "<none>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   252
      Index           =   0
      Left            =   3360
      TabIndex        =   5
      Top             =   600
      Width           =   4212
   End
   Begin VB.Label lblName 
      Caption         =   "Project WDM File"
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
      Index           =   2
      Left            =   1080
      TabIndex        =   4
      Top             =   600
      Width           =   1932
   End
   Begin VB.Label lblFile 
      Appearance      =   0  'Flat
      BackColor       =   &H80000004&
      BorderStyle     =   1  'Fixed Single
      Caption         =   "<none>"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H80000008&
      Height          =   252
      Index           =   2
      Left            =   3360
      TabIndex        =   2
      Top             =   240
      Width           =   4212
   End
   Begin VB.Label lblName 
      Caption         =   "BASINS Watershed File"
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
      Left            =   1080
      TabIndex        =   1
      Top             =   240
      Width           =   2172
   End
End
Attribute VB_Name = "frmAQUATOX"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Sub Init(icnt&)
  Dim lOper As HspfOperation
  Dim i&
  
  icnt = 0
  For i = 1 To myUci.OpnSeqBlock.Opns.Count
    Set lOper = myUci.OpnSeqBlock.Opn(i)
    If lOper.Name = "RCHRES" Then
      If frmOutput.IsAQUATOXLocation(lOper.Name, lOper.Id) Then
        'this is an aquatox output location
        icnt = icnt + 1
      End If
    End If
  Next i
End Sub

Private Sub cmdFile_Click(Index As Integer)
  Dim i&, S$, f$, fun&
  
  If Index = 0 Then
    'project wdm file
    If FileExists(BASINSPath & "\modelout", True, False) Then
      ChDriveDir BASINSPath & "\modelout"
    End If
    CDFile.flags = &H1806&
    CDFile.Filter = "WDM files (*.wdm)|*.wdm"
    CDFile.Filename = "*.wdm"
    CDFile.DialogTitle = "Select Project WDM File"
    On Error GoTo 40
    CDFile.CancelError = True
    CDFile.Action = 1
    'project wdm
    lblFile(Index).Caption = CDFile.Filename
40    'continue here on cancel
  ElseIf Index = 2 Then
    If FileExists(BASINSPath & "\modelout", True, False) Then
      ChDriveDir BASINSPath & "\modelout"
    End If
    CDFile.flags = &H8806&
    CDFile.Filter = "BASINS Watershed Files (*.wsd)"
    CDFile.Filename = "*.wsd"
    CDFile.DialogTitle = "Select BASINS Watershed File"
    On Error GoTo 50
    CDFile.CancelError = True
    CDFile.Action = 1
    lblFile(Index).Caption = CDFile.Filename
50    'continue here on cancel
  End If
End Sub

Private Sub cmdOkayCancel_Click(Index As Integer)
    Dim i&, S$, wdmname$(3), outwdm$, tmpuci$, iresp&
    
    If Index = 0 Then
      If lblFile(0) = "<none>" Then
        'no project file specified, don't allow to okay
        myMsgBox.Show "A project WDM file must be specified.", "WinHSPF-AQUATOX Problem", "&OK"
      Else
        outwdm = lblFile(0)
        'If lblFile(2) = "<none>" Then
        '  myMsgBox.Show "User must specify a BASINS Watershed File.", _
        '               "WinHSPF-AQUATOX Problem", "&OK"
        'Else
          StartAquatox
          Unload Me
        'End If
      End If
    ElseIf Index = 1 Then 'cancel
      Unload Me
    End If
End Sub

Private Sub Form_Load()
  Dim lOper As HspfOperation
  Dim i As Long
  Dim WDMId As Long
  Dim wsdpath As String
  Dim wsdname As String
  
  cboRchres.Clear
  For i = 1 To myUci.OpnSeqBlock.Opns.Count
    Set lOper = myUci.OpnSeqBlock.Opn(i)
    If lOper.Name = "RCHRES" Then
      If frmOutput.IsAQUATOXLocation(lOper.Name, lOper.Id) Then
        'this is an aquatox output location
        cboRchres.AddItem lOper.Name & " " & lOper.Id
      End If
    End If
  Next i
  If cboRchres.listcount = 0 Then
    myMsgBox.Show "At least one AQUATOX output location must be specified " & vbCrLf & "in the Output Manager " & _
       "before linking to AQUATOX.", "WinHSPF-AQUATOX Problem", "OK"
  Else
    cboRchres.ListIndex = 0
  End If
  'default project wdm name
  WDMId = 0
  For i = 4 To 1 Step -1
    If Not myUci.GetWDMObj(i) Is Nothing Then
      'use this as the output wdm
      WDMId = i
    End If
  Next i
  If WDMId > 0 Then
    If myUci.GetWDMObj(WDMId).Filename = FilenameNoPath(myUci.GetWDMObj(WDMId).Filename) Then
      lblFile(0).Caption = PathNameOnly(myUci.Name) & "\" & myUci.GetWDMObj(WDMId).Filename
    ElseIf Mid(myUci.GetWDMObj(WDMId).Filename, 1, 1) = "." Then
      lblFile(0).Caption = AbsolutePath(myUci.GetWDMObj(WDMId).Filename, PathNameOnly(myUci.Name))
    Else
      lblFile(0).Caption = myUci.GetWDMObj(WDMId).Filename
    End If
  End If
  'default watershed file name
  wsdpath = PathNameOnly(myUci.Name)
  wsdname = wsdpath & "\" & FilenameOnly(myUci.Name) & ".wsd"
  If FileExists(wsdname) Then    'wsd file exists
    lblFile(2).Caption = wsdname
  End If
End Sub

Private Sub StartAquatox()
    Dim AquatoxEXE$, uCommand$, rchid$, TempUciName$
    Dim reg As New ATCoRegistry
    Dim tname As String
    
    AquatoxEXE = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\Eco Modeling\AQUATOX\ExePath", "") & "\aquatox.exe"
    If Not FileExists(AquatoxEXE) Then
      'aquatox not in registry
      On Error GoTo NeverMind
      CDFile.CancelError = True
      CDFile.DialogTitle = "Please locate Aquatox.exe so AQUATOX can be started."
      CDFile.Filename = "Aquatox.exe"
      CDFile.ShowOpen
      AquatoxEXE = CDFile.Filename
    End If
    If Not FileExists(AquatoxEXE) Then
      DisableAll True
      myMsgBox.Show "WinHSPF could not find Aquatox.exe", "Aquatox Link Problem", "+-&Close"
      DisableAll False
    Else
      'command line includes model, path to basins gis files,
      'project wdm name, scenario, and loc name
      rchid = cboRchres.List(cboRchres.ListIndex)
      tname = StrRetRem(rchid)
      If lblFile(2).Caption = "<none>" Then  'no gis files, do anyway
        uCommand = " HSPF XXX " & """" & _
          Trim(lblFile(0).Caption) & """" & " " & UCase(FilenameOnly(myUci.Name)) & _
          " RCH" & rchid & " SUM"
      Else
        uCommand = " HSPF " & """" & Mid(lblFile(2).Caption, 1, Len(lblFile(2).Caption) - 4) & """" & " " & _
          """" & Trim(lblFile(0).Caption) & """" & " " & UCase(FilenameOnly(myUci.Name)) & _
          " RCH" & rchid & " SUM"
      End If
      'TempUciName = myUci.Name
      'HSPFMain.CloseUCI
      'Me.Hide
      'HSPFMain.Hide
      IPC.dbg "Starting Aquatox... " & AquatoxEXE & " " & uCommand
      IPC.StartProcess "Aquatox", AquatoxEXE & " " & uCommand, 0, 864000
      IPC.dbg "Finished Running Aquatox"
      'HSPFMain.Show
      'IPC.dbg "Opening UCI '" & TempUciName & "' after running Aquatox"
      'HSPFMain.OpenUCI TempUciName
    End If
NeverMind:
End Sub
