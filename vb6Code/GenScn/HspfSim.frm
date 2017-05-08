VERSION 5.00
Begin VB.Form frmHSPFSimulate 
   Caption         =   "HSPF Simulate"
   ClientHeight    =   1332
   ClientLeft      =   1056
   ClientTop       =   1980
   ClientWidth     =   6720
   HelpContextID   =   66
   Icon            =   "HspfSim.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   1332
   ScaleWidth      =   6720
   Begin VB.CommandButton cmdAll 
      Caption         =   "View &Output"
      Enabled         =   0   'False
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
      Index           =   4
      Left            =   3720
      TabIndex        =   3
      Top             =   840
      Width           =   1212
   End
   Begin VB.CommandButton cmdAll 
      Caption         =   "View &Echo"
      Enabled         =   0   'False
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
      Index           =   3
      Left            =   2280
      TabIndex        =   2
      Top             =   840
      Width           =   1212
   End
   Begin VB.CommandButton cmdAll 
      Caption         =   "Simulate(&R)"
      Enabled         =   0   'False
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
      Left            =   840
      TabIndex        =   1
      Top             =   840
      Width           =   1212
   End
   Begin VB.CommandButton cmdAll 
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
      Index           =   0
      Left            =   5160
      TabIndex        =   4
      Top             =   840
      Width           =   855
   End
   Begin VB.CommandButton cmdAll 
      Caption         =   "&Select"
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
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   732
   End
   Begin MSComDlg.CommonDialog CDUciFile 
      Left            =   120
      Top             =   720
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   3.09771e-37
   End
   Begin VB.Label lblFile 
      Caption         =   "<none>"
      Height          =   252
      Left            =   2280
      TabIndex        =   5
      Top             =   240
      Width           =   4212
   End
   Begin VB.Label lblName 
      Caption         =   "UCI File"
      Height          =   252
      Index           =   0
      Left            =   1200
      TabIndex        =   6
      Top             =   240
      Width           =   852
   End
End
Attribute VB_Name = "frmHSPFSimulate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdAll_Click(Index As Integer)
    Dim r&, cap$, s$, wdmUnits&(4), i&, j&
    
    If Len(lblFile.Caption) > 4 Then
      s = Left(lblFile.Caption, Len(lblFile.Caption) - 4)
    Else
      s = ""
    End If
    If Index = 0 Then 'cancel
      Unload Me
      frmGenScn.SetFocus
    ElseIf Index = 1 Then 'simulate
      If p.HSPFMsg.Unit = 0 Then
        MsgBox "No HSPF Message file available" & vbCrLf & "Check File:Edit from main screen", vbExclamation, "HSPF Simulate Problem"
      ElseIf p.WDMFiles(1).fileUnit = 0 Then
        MsgBox "No WDM data file available" & vbCrLf & "Check File:Edit from main screen", vbExclamation, "HSPF Simulate Problem"
      Else
        wdmUnits(0) = 0
        For i = 1 To 4
          wdmUnits(i) = 0
          If p.WDMFiles.Count >= i Then
            If p.WDMFiles(i).fileUnit > 0 Then
              j = F90_WDMOPN(p.WDMFiles(i).FileUnit, p.WDMFiles(i).Filename, Len(p.WDMFiles(i).Filename))
              wdmUnits(i) = p.WDMFiles(i).fileUnit
            End If
          End If
        Next i
        Call F90_ACTSCN(CLng(0), wdmUnits(1), p.HSPFMsg.Unit, r, s, Len(s))
        If r <> 0 Then
          MsgBox "Problem activating, retcod: " & CStr(r), vbExclamation, "HSPF Simulate Problem"
        Else
          Call F90_SIMSCN(r)
          If r <> 0 Then
            MsgBox "Problem executing, retcod: " & CStr(r), vbExclamation, "HSPF Simulate Problem"
          Else
            Dim vWDMFile As Variant
            'we might have just added or changed data
            For Each vWDMFile In p.WDMFiles
              vWDMFile.Refresh
            Next
            RefreshSLC
            cmdAll(4).Enabled = True 'view output
            cmdAll(3).Enabled = True 'view echo
          End If
        End If
      End If
    ElseIf Index = 2 Then 'select
      CDUciFile.flags = &H1800&
      CDUciFile.filter = "UCI files (*.uci)|*.uci"
      CDUciFile.Filename = "*.uci"
      CDUciFile.DialogTitle = "GenScn Select HSPF UCI File"
      On Error GoTo 40
      CDUciFile.CancelError = True
      CDUciFile.Action = 1
      lblFile.Caption = CDUciFile.Filename
      cmdAll(1).Enabled = True
40    'continue here on cancel
    ElseIf Index = 3 Then 'view echo
      cap = "HSPF Simulate View Echo"
      Call DispFile.OpenFile(s & ".ech", cap, frmHSPFSimulate.Icon, False)
    ElseIf Index = 4 Then 'view output
      cap = "HSPF Simulate View Output"
      Call DispFile.OpenFile(s & ".out", cap, frmHSPFSimulate.Icon, False)
    End If
End Sub


