VERSION 5.00
Begin VB.Form frmGenProp 
   Caption         =   "Properties"
   ClientHeight    =   4620
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7128
   Icon            =   "GenProp.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4620
   ScaleWidth      =   7128
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoGrid atcGrid 
      Height          =   3912
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   7092
      _ExtentX        =   12510
      _ExtentY        =   6900
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
      Header          =   "lblHeader"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483633
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   492
      Left            =   2280
      TabIndex        =   1
      Top             =   4080
      Width           =   2652
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
         Height          =   432
         Left            =   0
         TabIndex        =   2
         Top             =   0
         Width           =   1092
      End
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   432
         Left            =   1500
         TabIndex        =   3
         Top             =   0
         Width           =   1152
      End
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
    
    On Error Resume Next
    For i = 1 To atcGrid.Rows
      Name = atcGrid.TextMatrix(i, 0)
      desc = atcGrid.TextMatrix(i, 1)
      If Left(Me.Caption, 2) = "Lo" Then  'location
        p.LocnDesc.Remove Name
        p.LocnDesc.Add desc, Name
      ElseIf Left(Me.Caption, 2) = "Sc" Then 'scenarios
        p.ScenDesc.Remove Name
        p.ScenDesc.Add desc, Name
        p.ScenType.Remove Name
        p.ScenType.Add atcGrid.TextMatrix(i, 2), Name
        p.ScenFile.Remove Name
        p.ScenFile.Add atcGrid.TextMatrix(i, 3), Name
      Else
        p.ConsDesc.Remove Name
        p.ConsDesc.Add desc, Name
      End If
    Next i
      
    Unload Me
    
    p.EditFlg = True
End Sub

Private Sub Form_Resize()
  If Width > 130 And Height > 1100 Then
    atcGrid.Width = Width - 130
    atcGrid.ColsSizeToWidth
    fraButtons.Left = (Width - fraButtons.Width) / 2
    fraButtons.Top = Height - 864
    atcGrid.Height = fraButtons.Top - 168
  End If
End Sub
