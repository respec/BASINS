VERSION 5.00
Begin VB.Form frmOptions 
   Caption         =   "Form1"
   ClientHeight    =   2520
   ClientLeft      =   48
   ClientTop       =   312
   ClientWidth     =   6360
   LinkTopic       =   "Form1"
   ScaleHeight     =   2520
   ScaleWidth      =   6360
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox txtComment 
      Height          =   288
      Left            =   2040
      TabIndex        =   6
      Text            =   "Text1"
      Top             =   1320
      Width           =   4212
   End
   Begin VB.CheckBox chkShowAll 
      Height          =   252
      Left            =   2040
      TabIndex        =   3
      Top             =   840
      Width           =   1332
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Ok"
      Height          =   372
      Left            =   2040
      TabIndex        =   2
      Top             =   1920
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
      Caption         =   "Every File Comment"
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
      Left            =   240
      TabIndex        =   5
      Top             =   1320
      Width           =   1692
   End
   Begin VB.Label Label2 
      Caption         =   "Show whole project"
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
      Left            =   240
      TabIndex        =   4
      Top             =   840
      Width           =   1692
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
      Left            =   240
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

Private Sub Command1_Click()
  If IsNumeric(txtTreeIndent.text) Then frmMain.tree1.Indentation = txtTreeIndent.text
  If chkShowAll.Value = vbChecked Then frmMain.ShowAllItems = True Else frmMain.ShowAllItems = False
  SourceComment = txtComment
  Unload Me
End Sub

Private Sub Form_Load()
  txtTreeIndent.text = frmMain.tree1.Indentation
  If frmMain.ShowAllItems Then chkShowAll.Value = vbChecked Else chkShowAll.Value = vbUnchecked
  txtComment = SourceComment
End Sub
