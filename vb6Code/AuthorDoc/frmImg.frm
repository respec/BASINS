VERSION 5.00
Begin VB.Form frmImg 
   Caption         =   "Form1"
   ClientHeight    =   2916
   ClientLeft      =   48
   ClientTop       =   312
   ClientWidth     =   4512
   LinkTopic       =   "Form1"
   ScaleHeight     =   2916
   ScaleWidth      =   4512
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox img 
      AutoSize        =   -1  'True
      Height          =   2052
      Left            =   0
      ScaleHeight     =   2004
      ScaleWidth      =   3084
      TabIndex        =   0
      Top             =   0
      Width           =   3132
   End
End
Attribute VB_Name = "frmImg"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public Sub SetImage(filename$)
  Me.caption = filename
  img.Picture = LoadPicture(filename)
  Me.Show
  'Me.Width = img.Width
  'Me.Height = img.Height
End Sub

Private Sub Form_Resize()
  img.Width = Width
  img.Height = Height
End Sub
