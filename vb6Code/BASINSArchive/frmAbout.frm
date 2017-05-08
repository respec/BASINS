VERSION 5.00
Begin VB.Form frmAbout 
   Caption         =   "About "
   ClientHeight    =   5760
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6330
   Icon            =   "frmAbout.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5760
   ScaleWidth      =   6330
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2520
      TabIndex        =   3
      Top             =   5280
      Width           =   1452
      Begin VB.CommandButton cmdClose 
         Cancel          =   -1  'True
         Caption         =   "Close"
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
         Height          =   312
         Left            =   120
         TabIndex        =   4
         Top             =   0
         Width           =   1212
      End
   End
   Begin VB.PictureBox pctAquaTerra 
      AutoSize        =   -1  'True
      Height          =   2055
      Left            =   120
      MouseIcon       =   "frmAbout.frx":08CA
      MousePointer    =   99  'Custom
      Picture         =   "frmAbout.frx":0BD4
      ScaleHeight     =   1995
      ScaleWidth      =   6000
      TabIndex        =   1
      ToolTipText     =   "http://www.aquaterra.com"
      Top             =   2760
      Width           =   6060
   End
   Begin VB.PictureBox pctBasins 
      AutoSize        =   -1  'True
      Height          =   2790
      Left            =   0
      MouseIcon       =   "frmAbout.frx":1A30
      MousePointer    =   99  'Custom
      Picture         =   "frmAbout.frx":1D3A
      ScaleHeight     =   2730
      ScaleWidth      =   6000
      TabIndex        =   0
      ToolTipText     =   "http://www.epa.gov/OST/BASINS/"
      Top             =   0
      Width           =   6060
   End
   Begin VB.Label lblVersion 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      Caption         =   "About Information Goes Here"
      Height          =   195
      Left            =   2145
      TabIndex        =   2
      Top             =   4920
      Width           =   2085
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub cmdClose_Click()
  Unload Me
End Sub

Public Sub ShowVersions(myStatus As String)
  caption = "About " & App.Title
  lblVersion.caption = myStatus
  If pctBasins.Width + 240 > Me.Width Then Me.Width = pctBasins.Width + 240
  pctBasins.Left = (Me.Width / 2) - (pctBasins.Width / 2)
  
  pctAquaTerra.Left = (Me.Width / 2) - (pctAquaTerra.Width / 2)
  pctAquaTerra.Top = pctBasins.Top + pctBasins.Height + 60
    
  lblVersion.Left = Me.Width / 2 - lblVersion.Width / 2
  lblVersion.Top = pctAquaTerra.Top + pctAquaTerra.Height + 60
  
  fraButtons.Left = Me.Width / 2 - fraButtons.Width / 2
  fraButtons.Top = lblVersion.Top + lblVersion.Height + 200
  
  If WindowState = vbNormal Then Me.Height = fraButtons.Top + fraButtons.Height + 500

  Me.Show
End Sub

Private Sub pctAquaTerra_Click()
  OpenFile "http://www.aquaterra.com"
End Sub

Private Sub pctBasins_Click()
  OpenFile "http://www.epa.gov/OST/BASINS/"
End Sub

