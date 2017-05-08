VERSION 5.00
Begin VB.Form frmAbout 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "About WDMUtil"
   ClientHeight    =   5544
   ClientLeft      =   36
   ClientTop       =   252
   ClientWidth     =   7296
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5544
   ScaleWidth      =   7296
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "Close"
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
      Height          =   312
      Left            =   2280
      TabIndex        =   1
      Top             =   4080
      Width           =   1212
   End
   Begin VB.PictureBox pctAquaTerra 
      AutoSize        =   -1  'True
      Height          =   1644
      Left            =   360
      MouseIcon       =   "About.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "About.frx":030A
      ScaleHeight     =   1596
      ScaleWidth      =   4800
      TabIndex        =   0
      Top             =   2280
      Width           =   4848
   End
   Begin VB.Label lblInfo 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      Caption         =   "?"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   192
      Left            =   1548
      TabIndex        =   2
      Top             =   240
      Width           =   132
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub Form_Activate()
  Me.Icon = frmGenScn.Icon
  
  If lblInfo.Width > pctAquaTerra.Width Then
    Me.Width = lblInfo.Width + 400
  Else
    Me.Width = pctAquaTerra.Width + 400
  End If
  pctAquaTerra.Left = Me.Width / 2 - pctAquaTerra.Width / 2
  lblInfo.Left = Me.Width / 2 - lblInfo.Width / 2
  cmdClose.Left = Me.Width / 2 - cmdClose.Width / 2
  cmdClose.Top = pctAquaTerra.Top + pctAquaTerra.Height + 200
  Me.Height = cmdClose.Top + cmdClose.Height + 500
End Sub

Private Sub pctAquaTerra_Click()
  OpenFile "http://www.aquaterra.com"
End Sub
