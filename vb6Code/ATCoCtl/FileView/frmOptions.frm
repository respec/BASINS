VERSION 5.00
Begin VB.Form frmOptions 
   Caption         =   "Options"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOk 
      Caption         =   "Ok"
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
      Left            =   960
      TabIndex        =   4
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
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
      Left            =   2520
      TabIndex        =   3
      Top             =   2520
      Width           =   1215
   End
   Begin VB.CheckBox chkOption 
      Caption         =   "chkOption"
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
      Left            =   240
      TabIndex        =   1
      Top             =   1200
      Visible         =   0   'False
      Width           =   3855
   End
   Begin VB.TextBox txtOption 
      Height          =   375
      Index           =   0
      Left            =   3240
      TabIndex        =   0
      Text            =   "txtOption"
      Top             =   360
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.Label lblOption 
      BackStyle       =   0  'Transparent
      Caption         =   "lblOption"
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
      Left            =   240
      TabIndex        =   2
      Top             =   360
      Visible         =   0   'False
      Width           =   3855
   End
End
Attribute VB_Name = "frmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Dim i&
  For i = 1 To frmFileView.Options.Count
    If txtOption(i).Visible Then
      frmFileView.Options(i).Value = txtOption(i).Text
    ElseIf chkOption(i).Visible Then
      If chkOption(i).Value = 1 Then
        frmFileView.Options(i).Value = True
      Else
        frmFileView.Options(i).Value = False
      End If
    End If
  Next i
  frmFileView.RefreshMain
  Unload Me
End Sub
