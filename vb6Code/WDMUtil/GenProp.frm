VERSION 5.00
Begin VB.Form frmGenProp 
   Caption         =   "Properties"
   ClientHeight    =   5112
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7500
   Icon            =   "GenProp.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5112
   ScaleWidth      =   7500
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Height          =   372
      Left            =   2520
      TabIndex        =   3
      Top             =   4560
      Width           =   2412
      Begin VB.CommandButton cmdOkay 
         Caption         =   "&Ok"
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
         Left            =   0
         TabIndex        =   1
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "&Cancel"
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
         Left            =   1320
         TabIndex        =   2
         Top             =   0
         Width           =   1092
      End
   End
   Begin ATCoCtl.ATCoGrid atcGrid 
      Height          =   4212
      Left            =   180
      TabIndex        =   0
      Top             =   120
      Width           =   7092
      _ExtentX        =   12510
      _ExtentY        =   7430
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
      Rows            =   2
      Cols            =   2
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   ""
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483634
      ForeColorSel    =   16777215
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
End
Attribute VB_Name = "frmGenProp"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub cmdCancel_Click()
    Unload Me
End Sub

Private Sub cmdOkay_Click()
    Dim i&, Name$, desc$
    
    For i = 1 To atcGrid.Rows
      Name = atcGrid.TextMatrix(i, 0)
      desc = atcGrid.TextMatrix(i, 1)
      If Left(Me.Caption, 2) = "Lo" Then  'location
        p.Locn(i - 1).desc = desc
      ElseIf Left(Me.Caption, 2) = "Sc" Then 'scenarios
        p.Scen(i - 1).desc = desc
      Else
        p.Cons(i - 1).desc = desc
      End If
    Next i
      
    Unload Me
    
    p.EditFlg = True
End Sub

Private Sub Form_Resize()
  Dim w&, h&
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 500 And h > 1000 Then
    fraButtons.Left = (w - fraButtons.Width) / 2
    fraButtons.Top = h - fraButtons.Height - 180
    atcGrid.Width = w - 408
    atcGrid.Height = fraButtons.Top - atcGrid.Top - 228
  End If
End Sub
