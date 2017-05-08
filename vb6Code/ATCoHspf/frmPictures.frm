VERSION 5.00
Begin VB.Form frmPictures 
   Caption         =   "Pictures"
   ClientHeight    =   2556
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   3744
   LinkTopic       =   "Form1"
   ScaleHeight     =   213
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   312
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox picBlank 
      AutoRedraw      =   -1  'True
      Height          =   432
      Left            =   2280
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   3
      Top             =   240
      Width           =   432
   End
   Begin VB.PictureBox picBMP 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   432
      Left            =   1680
      Picture         =   "frmPictures.frx":0000
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   2
      Top             =   240
      Width           =   432
   End
   Begin VB.PictureBox picStream 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   432
      Left            =   1080
      Picture         =   "frmPictures.frx":030A
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   1
      Top             =   240
      Width           =   432
   End
   Begin VB.PictureBox picLake 
      AutoRedraw      =   -1  'True
      AutoSize        =   -1  'True
      Height          =   432
      Left            =   480
      Picture         =   "frmPictures.frx":0614
      ScaleHeight     =   32
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   32
      TabIndex        =   0
      Top             =   240
      Width           =   432
   End
End
Attribute VB_Name = "frmPictures"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

