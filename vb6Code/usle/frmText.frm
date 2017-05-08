VERSION 5.00
Begin VB.Form frmText 
   Caption         =   "USLE"
   ClientHeight    =   5484
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   5628
   LinkTopic       =   "Form1"
   ScaleHeight     =   5484
   ScaleWidth      =   5628
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
      Default         =   -1  'True
      Height          =   372
      Left            =   2280
      TabIndex        =   2
      Top             =   5040
      Width           =   1092
   End
   Begin VB.TextBox txtBody 
      Height          =   3852
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Text            =   "frmText.frx":0000
      Top             =   1080
      Width           =   5412
   End
   Begin VB.Label lblTitle 
      Caption         =   "Estimating Sediment Load for Hydrologic Cataloging Units"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   18
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   972
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   5292
   End
End
Attribute VB_Name = "frmText"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub Form_Resize()
  If Width > 300 Then
    txtBody.Width = Width - 400
    cmdClose.Left = (Width - cmdClose.Width) / 2
  End If
  If Height > 2000 Then
    txtBody.Height = Height - 2000
    cmdClose.Top = Height - 768
  End If
End Sub
