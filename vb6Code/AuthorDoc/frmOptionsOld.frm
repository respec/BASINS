VERSION 5.00
Begin VB.Form frmOptions 
   Caption         =   "Form1"
   ClientHeight    =   2256
   ClientLeft      =   48
   ClientTop       =   312
   ClientWidth     =   3744
   LinkTopic       =   "Form1"
   ScaleHeight     =   2256
   ScaleWidth      =   3744
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtFont 
      Height          =   288
      Left            =   960
      TabIndex        =   6
      Text            =   "Text1"
      Top             =   1080
      Width           =   2412
   End
   Begin VB.TextBox txtFindTimeout 
      Height          =   288
      Left            =   2040
      TabIndex        =   3
      Text            =   "Text1"
      Top             =   720
      Width           =   1332
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Ok"
      Height          =   372
      Left            =   1200
      TabIndex        =   2
      Top             =   1680
      Width           =   1332
   End
   Begin VB.TextBox txtTreeIndent 
      Height          =   288
      Left            =   2040
      TabIndex        =   0
      Text            =   "Text1"
      Top             =   324
      Width           =   1332
   End
   Begin VB.Label Label3 
      Caption         =   "Font"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   360
      TabIndex        =   5
      Top             =   1080
      Width           =   492
   End
   Begin VB.Label Label2 
      Caption         =   "Find Timeout"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   360
      TabIndex        =   4
      Top             =   720
      Width           =   1332
   End
   Begin VB.Label Label1 
      Caption         =   "Tree Indent"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   360
      TabIndex        =   1
      Top             =   360
      Width           =   1332
   End
End
Attribute VB_Name = "frmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private Sub Command1_Click()
  If IsNumeric(txtTreeIndent.Text) Then frmMain.tree1.Indentation = txtTreeIndent.Text
  If IsNumeric(txtFindTimeout.Text) Then FindTimeout = txtFindTimeout.Text
  Unload Me
End Sub

Private Sub Form_Load()
  txtTreeIndent.Text = frmMain.tree1.Indentation
  txtFindTimeout.Text = FindTimeout
  
  CopyFont frmMain.txtMain, txtFont
  With txtFont
    .Text = .FontName & .FontSize
    If .FontBold Then .Text = .Text & "Bold"
    If .FontItalic Then .Text = .Text & "Italic"
    If .FontUnderline Then .Text = .Text & "Underline"
  End With

End Sub

Private Sub txtFont_Click()
  CopyFont txtFont, frmMain.cdlg
  On Error GoTo ExitSub
  frmMain.cdlg.CancelError = True
  frmMain.cdlg.flags = cdlCFBoth + cdlCFScalableOnly + cdlCFWYSIWYG
  frmMain.cdlg.ShowFont
  
  CopyFont frmMain.cdlg, txtFont
  CopyFont frmMain.cdlg, frmMain.txtMain
  
ExitSub:
  frmMain.cdlg.CancelError = False
End Sub

Private Sub CopyFont(src As Object, dst As Object)
  On Error Resume Next 'Some objects have only some of the font attributes
  dst.FontBold = src.FontBold
  dst.FontItalic = src.FontItalic
  dst.FontName = src.FontName
  dst.FontSize = src.FontSize
  dst.FontStrikethru = src.FontStrikethru
  dst.FontUnderline = src.FontUnderline
  dst.FontTransparent = src.FontTransparent
End Sub

