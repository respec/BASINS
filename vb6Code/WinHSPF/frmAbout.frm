VERSION 5.00
Begin VB.Form frmAbout 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "About WinHSPF"
   ClientHeight    =   5112
   ClientLeft      =   36
   ClientTop       =   252
   ClientWidth     =   6072
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5112
   ScaleWidth      =   6072
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2040
      TabIndex        =   3
      Top             =   4680
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
         TabIndex        =   4
         Top             =   0
         Width           =   1212
      End
   End
   Begin VB.PictureBox pctAquaTerra 
      AutoSize        =   -1  'True
      Height          =   1644
      Left            =   480
      MouseIcon       =   "frmAbout.frx":0000
      MousePointer    =   99  'Custom
      Picture         =   "frmAbout.frx":030A
      ScaleHeight     =   1596
      ScaleWidth      =   4800
      TabIndex        =   2
      ToolTipText     =   "http://www.aquaterra.com"
      Top             =   2400
      Width           =   4848
   End
   Begin VB.PictureBox pctBasins 
      AutoSize        =   -1  'True
      Height          =   2232
      Left            =   480
      MouseIcon       =   "frmAbout.frx":1166
      MousePointer    =   99  'Custom
      Picture         =   "frmAbout.frx":1470
      ScaleHeight     =   2184
      ScaleWidth      =   4800
      TabIndex        =   1
      ToolTipText     =   "http://www.epa.gov/OST/BASINS/"
      Top             =   120
      Width           =   4848
   End
   Begin VB.Label lblInfo 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      Caption         =   "?"
      Height          =   192
      Left            =   2760
      TabIndex        =   0
      Top             =   4320
      Width           =   108
   End
End
Attribute VB_Name = "frmAbout"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdClose_Click()
    Unload Me
End Sub


'Private Sub cmdLink_Click(Index As Integer)
'  Select Case Index
'    Case 0: OpenFile "http://www.aquaterra.com"
'    Case 1: OpenFile "http://water.usgs.gov/software/genscn.html"
'    Case 2: OpenFile "http://www.epa.gov/OST/BASINS/"
'  End Select
'End Sub

Private Sub Form_Load()
    Me.Icon = HSPFMain.Icon
    
    lblInfo.Caption = HSPFMain.StatusString
    If lblInfo.width > pctBasins.width Then Me.width = lblInfo.width + 240
    
    If pctBasins.width + 240 > Me.width Then Me.width = pctBasins.width + 240
    pctBasins.Left = (Me.width / 2) - (pctBasins.width / 2)
    
    pctAquaTerra.Left = (Me.width / 2) - (pctAquaTerra.width / 2)
    pctAquaTerra.Top = pctBasins.Top + pctBasins.height + 60
    
    'pctBasins.Left = (Me.width / 2) - (pctBasins.width / 2)
    'pctBasins.Top = pctAquaTerra.Top + pctAquaTerra.height + 60
    
    lblInfo.Left = Me.width / 2 - lblInfo.width / 2
    lblInfo.Top = pctAquaTerra.Top + pctAquaTerra.height + 200
    
    fraButtons.Left = Me.width / 2 - fraButtons.width / 2
    fraButtons.Top = lblInfo.Top + lblInfo.height + 100
    
    If WindowState = vbNormal Then Me.height = fraButtons.Top + fraButtons.height + 500
End Sub

Private Sub pctAquaTerra_Click()
  OpenFile "http://www.aquaterra.com"
End Sub

Private Sub pctBasins_Click()
  OpenFile "http://www.epa.gov/OST/BASINS/"
End Sub

