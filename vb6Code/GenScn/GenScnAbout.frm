VERSION 5.00
Begin VB.Form frmGenScnAbout 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "About GenScn"
   ClientHeight    =   6420
   ClientLeft      =   36
   ClientTop       =   252
   ClientWidth     =   6072
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   6420
   ScaleWidth      =   6072
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2040
      TabIndex        =   4
      Top             =   5760
      Width           =   1452
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
         Left            =   120
         TabIndex        =   5
         Top             =   0
         Width           =   1212
      End
   End
   Begin VB.PictureBox pctAquaTerra 
      AutoSize        =   -1  'True
      Height          =   1644
      Left            =   480
      MouseIcon       =   "GenScnAbout.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "GenScnAbout.frx":030A
      ScaleHeight     =   1596
      ScaleWidth      =   4800
      TabIndex        =   3
      ToolTipText     =   "http://www.aquaterra.com/"
      Top             =   3600
      Width           =   4848
   End
   Begin VB.PictureBox pctBasins 
      AutoSize        =   -1  'True
      Height          =   2232
      Left            =   480
      MouseIcon       =   "GenScnAbout.frx":1166
      MousePointer    =   99  'Custom
      Picture         =   "GenScnAbout.frx":1470
      ScaleHeight     =   2184
      ScaleWidth      =   4800
      TabIndex        =   2
      ToolTipText     =   "http://www.epa.gov/waterscience/BASINS/"
      Top             =   1200
      Width           =   4848
   End
   Begin VB.PictureBox pctUSGS 
      AutoSize        =   -1  'True
      Height          =   912
      Left            =   120
      MouseIcon       =   "GenScnAbout.frx":403C
      MousePointer    =   99  'Custom
      Picture         =   "GenScnAbout.frx":4346
      ScaleHeight     =   864
      ScaleWidth      =   5760
      TabIndex        =   0
      ToolTipText     =   "http://water.usgs.gov/software/genscn.html"
      Top             =   120
      Width           =   5808
   End
   Begin VB.Label lblInfo 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      Caption         =   "?"
      Height          =   192
      Left            =   2760
      TabIndex        =   1
      Top             =   5400
      Width           =   108
   End
End
Attribute VB_Name = "frmGenScnAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub Form_Load()
  Me.Icon = frmGenScn.Icon
  
  lblInfo.Caption = StatusString
  If lblInfo.Width > pctUSGS.Width Then Me.Width = lblInfo.Width + 240
  
  If pctUSGS.Width + 240 > Me.Width Then Me.Width = pctUSGS.Width + 240
  pctUSGS.Left = (Me.Width / 2) - (pctUSGS.Width / 2)
  
  pctAquaTerra.Left = (Me.Width / 2) - (pctAquaTerra.Width / 2)
  pctAquaTerra.Top = pctUSGS.Top + pctUSGS.Height + 60
  
  pctBasins.Left = (Me.Width / 2) - (pctBasins.Width / 2)
  pctBasins.Top = pctAquaTerra.Top + pctAquaTerra.Height + 60
  
  lblInfo.Left = Me.Width / 2 - lblInfo.Width / 2
  lblInfo.Top = pctBasins.Top + pctBasins.Height + 60
  
  fraButtons.Left = Me.Width / 2 - fraButtons.Width / 2
  fraButtons.Top = lblInfo.Top + lblInfo.Height + 200
  
  If WindowState = vbNormal Then Me.Height = fraButtons.Top + fraButtons.Height + 500
End Sub

Private Sub pctAquaTerra_Click()
  OpenFile "http://www.aquaterra.com/"
End Sub

Private Sub pctBasins_Click()
  OpenFile "http://www.epa.gov/waterscience/BASINS/"
End Sub

Private Sub pctUSGS_Click()
  OpenFile "http://water.usgs.gov/software/genscn.html"
End Sub
