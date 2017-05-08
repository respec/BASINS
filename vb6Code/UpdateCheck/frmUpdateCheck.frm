VERSION 5.00
Begin VB.Form frmUpdate 
   Caption         =   "Update Complete"
   ClientHeight    =   3210
   ClientLeft      =   60
   ClientTop       =   285
   ClientWidth     =   4560
   Icon            =   "frmUpdateCheck.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3210
   ScaleWidth      =   4560
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtLog 
      Height          =   2175
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   1
      Top             =   120
      Width           =   4335
   End
   Begin VB.CommandButton CmdOK 
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
      Height          =   495
      Left            =   1560
      TabIndex        =   0
      Top             =   2520
      Width           =   1455
   End
End
Attribute VB_Name = "frmUpdate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub CmdOK_Click()
  Me.Visible = False
End Sub
