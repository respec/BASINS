VERSION 5.00
Begin VB.Form frmSoilRead 
   Caption         =   "TMDL USLE Status"
   ClientHeight    =   1044
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   5100
   Icon            =   "frmSoilRead.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   1044
   ScaleWidth      =   5100
   StartUpPosition =   3  'Windows Default
   Begin VB.Label lblSoilRead 
      Caption         =   "Loading soils from database for selected HUCs..."
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   4452
   End
End
Attribute VB_Name = "frmSoilRead"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Form_Load()
  lblSoilRead.Caption = "Loading soils from database for selected HUCs..." & vbCr & "This may take a little time."
  Me.Visible = True
  DoEvents
End Sub
