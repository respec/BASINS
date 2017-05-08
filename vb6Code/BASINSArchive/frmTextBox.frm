VERSION 5.00
Begin VB.Form frmTextBox 
   ClientHeight    =   6150
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7590
   Icon            =   "frmTextBox.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6150
   ScaleWidth      =   7590
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txt 
      BackColor       =   &H8000000F&
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   12
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   6135
      Left            =   0
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   0
      Top             =   0
      Width           =   5775
   End
End
Attribute VB_Name = "frmTextBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub Form_Resize()
  txt.Width = ScaleWidth
  txt.Height = ScaleHeight
End Sub
