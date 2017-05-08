VERSION 5.00
Begin VB.Form frmDebug 
   AutoRedraw      =   -1  'True
   Caption         =   "NFF Web"
   ClientHeight    =   1920
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7008
   LinkTopic       =   "Form1"
   ScaleHeight     =   160
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   584
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdClose 
      Caption         =   "Close"
      Default         =   -1  'True
      Height          =   372
      Left            =   3120
      TabIndex        =   1
      Top             =   1440
      Width           =   732
   End
   Begin VB.TextBox txt 
      Height          =   1212
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   0
      Top             =   120
      Width           =   6732
   End
End
Attribute VB_Name = "frmDebug"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdClose_Click()
  End
End Sub
