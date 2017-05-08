VERSION 5.00
Begin VB.Form frmAsk 
   Caption         =   "Process Not Ending"
   ClientHeight    =   1428
   ClientLeft      =   60
   ClientTop       =   288
   ClientWidth     =   5304
   LinkTopic       =   "Form1"
   ScaleHeight     =   1428
   ScaleWidth      =   5304
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdEnd 
      Caption         =   "&End Process Now"
      Height          =   372
      Left            =   1800
      TabIndex        =   0
      Top             =   960
      Width           =   1692
   End
   Begin VB.Label lbl2 
      Alignment       =   2  'Center
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   480
      Width           =   5052
   End
   Begin VB.Label lbl 
      Alignment       =   2  'Center
      Height          =   252
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   5052
   End
End
Attribute VB_Name = "frmAsk"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public IPC As ATCoIPC
Private pProcessName As String

Public Sub AskAbout(ProcessName As String)
  pProcessName = ProcessName
  Caption = "Process '" & pProcessName & "'"
  lbl = "Still waiting for '" & pProcessName & "' to finish."
  lbl2 = ""
  Show
End Sub

Private Sub cmdEnd_Click()
  If Not IPC Is Nothing Then
    IPC.ExitProcess pProcessName
  Else
    MsgBox "IPC not set correctly, so process cannot be ended manually", vbOKOnly, "IPC Ask"
  End If
  Hide
End Sub
