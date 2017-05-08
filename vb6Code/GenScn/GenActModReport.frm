VERSION 5.00
Begin VB.Form frmGenActModReport 
   Caption         =   "GenScn Activate Modify Report"
   ClientHeight    =   1572
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   4680
   HelpContextID   =   57
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   1572
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraReport 
      Caption         =   "Reports"
      Height          =   735
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   4455
      Begin VB.CheckBox chkReport 
         Caption         =   "Report Name"
         Height          =   375
         Index           =   0
         Left            =   240
         TabIndex        =   3
         Top             =   240
         Width           =   3855
      End
   End
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Close"
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
      Left            =   2640
      TabIndex        =   1
      Top             =   960
      Width           =   1215
   End
   Begin VB.CommandButton cmdUpU 
      Caption         =   "&Update UCI"
      Default         =   -1  'True
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
      Left            =   840
      TabIndex        =   0
      Top             =   960
      Width           =   1215
   End
End
Attribute VB_Name = "frmGenActModReport"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Private msgTitle$

Private Sub chkReport_Click(Index As Integer)
    cmdUpU.Enabled = True
End Sub

Private Sub cmdClose_Click()
    Dim ans&
    
    If cmdUpU.Enabled = True Then
      ans = MsgBox("You have changes to your UCI made which have not been saved. " & vbCrLf & _
                   "OK trashes them, Cancel allows you a chance to update your UCI.", _
                   vbOKCancel, msgTitle)
    Else 'no changes pending
      ans = vbOK
    End If
    
    If ans = vbOK Then
      Unload frmGenActModReport
    End If
End Sub

Private Sub cmdUpU_Click()
    Dim i&, irep&
    For i = 0 To 6
      irep = F90_REPEXT(i + 1)
      If irep = 1 And chkReport(i).value = 0 Then
        'this report is on and we want it off
        Call F90_DELREPT(i + 1)
      ElseIf irep = 0 And chkReport(i).value = 1 Then
        'this report is off and we want it on
        Call F90_ADDREPT(p.HSPFMsg.Unit, i + 1)
      End If
    Next i
    cmdUpU.Enabled = False
End Sub

Private Sub Form_Load()
  Dim iexist As Boolean, i%, irep&
  
  Me.Icon = frmGenScnActivate.Icon
  msgTitle = "GenScn Activate Modify Report"
  'check boxes for standard reports
  For i = 1 To 6
    Load chkReport(i)
    chkReport(i).Container = fraReport
    chkReport(i).Top = chkReport(i - 1).Top + chkReport(i - 1).Height
    chkReport(i).Visible = True
  Next i
  chkReport(0).Caption = "Land Surface total loads"
  chkReport(1).Caption = "Land Surface percent loads"
  chkReport(2).Caption = "Land Surface unit loads"
  chkReport(3).Caption = "Reach total loads"
  chkReport(4).Caption = "Reach total change(+gain,-loss)"
  chkReport(5).Caption = "Point Loads"
  chkReport(6).Caption = "BMP Removals"
  fraReport.Height = chkReport(6).Top + chkReport(6).Height + 200
  cmdUpU.Top = fraReport.Top + fraReport.Height + 200
  cmdClose.Top = cmdUpU.Top
  If WindowState = vbNormal Then Me.Height = cmdUpU.Top + cmdUpU.Height + 500
  
  'are any reports available?
  iexist = frmGenScnActivate.OperationExists("REPORT")
  If iexist Then
    For i = 0 To 6
      irep = F90_REPEXT(i + 1)
      If irep = 1 Then
        'this report is on
        chkReport(i).value = 1
      Else
        chkReport(i).value = 0
      End If
    Next i
  End If
  cmdUpU.Enabled = False
End Sub
