VERSION 5.00
Begin VB.Form frmTSPrintSave 
   ClientHeight    =   3324
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   7308
   LinkTopic       =   "Form1"
   ScaleHeight     =   3324
   ScaleWidth      =   7308
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoBrowse browse 
      Height          =   4212
      Left            =   3120
      TabIndex        =   0
      Top             =   120
      Width           =   4092
      _ExtentX        =   7218
      _ExtentY        =   7430
   End
   Begin VB.TextBox txtEmptyCell 
      Height          =   285
      Left            =   1800
      TabIndex        =   11
      ToolTipText     =   "Fill empty cells with this string"
      Top             =   2160
      Width           =   1095
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Character &delimited:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   2
      Left            =   240
      TabIndex        =   6
      Top             =   996
      Width           =   2175
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "&Tab delimited"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   0
      Left            =   240
      TabIndex        =   4
      Top             =   480
      Value           =   -1  'True
      Width           =   2175
   End
   Begin VB.TextBox txtDelimiter 
      Height          =   285
      Left            =   2640
      TabIndex        =   7
      Text            =   ","
      ToolTipText     =   "Single printable character delimiter"
      Top             =   960
      Width           =   255
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Fi&xed width space padded"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   3
      Left            =   240
      TabIndex        =   8
      Top             =   1260
      Width           =   2655
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
      Height          =   375
      Left            =   1800
      TabIndex        =   2
      Top             =   2760
      Width           =   975
   End
   Begin VB.CommandButton cmdGo 
      Caption         =   "Go"
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
      Height          =   375
      Left            =   360
      TabIndex        =   1
      Top             =   2760
      Width           =   975
   End
   Begin VB.CheckBox chkColTitles 
      Caption         =   "Include &Column Titles"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   9
      Top             =   1560
      Value           =   1  'Checked
      Width           =   2295
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Sp&ace delimited"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   1
      Left            =   240
      TabIndex        =   5
      Top             =   744
      Width           =   1695
   End
   Begin VB.Label Label2 
      Caption         =   "&Empty Cell Text:"
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
      TabIndex        =   10
      Top             =   2232
      Width           =   1452
   End
   Begin VB.Label lblFileType 
      BackStyle       =   0  'Transparent
      Caption         =   "Column &Format:"
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
      Left            =   120
      TabIndex        =   3
      Top             =   120
      Width           =   1452
   End
End
Attribute VB_Name = "frmTSPrintSave"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private mvarSaver As frmTSlist
Private mvarFilename As String
Private Const savelabel = "&Save"
Private Const printLabel = "&Print"

Public Property Get Filename() As String
  Filename = mvarFilename
End Property

Public Property Let Filename(newName As String)
  mvarFilename = newName
End Property

Public Sub PrintGrid(saver As frmTSlist)
  Caption = "Print Grid"
  cmdGo.Caption = printLabel
  InitControls saver
  Width = 3216
  browse.Visible = False
End Sub

Public Sub SaveGrid(saver As frmTSlist)
  Caption = "Specify Format of Text File"
  cmdGo.Caption = savelabel
  InitControls saver
  browse.Visible = True
  If Width < 3500 Then Width = 9408
End Sub
  
Private Sub InitControls(saver As frmTSlist)
  Dim Button&
  Set mvarSaver = saver
  On Error GoTo showmodal
  Me.Show
  Exit Sub
showmodal:
  Me.Show vbModal
End Sub


'Private Sub browseFilename()
'  dlgOpenFile.filename = mvarFilename
'  dlgOpenFile.ShowSave
'  mvarFilename = dlgOpenFile.filename
'End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdGo_Click()
  Select Case cmdGo.Caption
    Case savelabel:  PrintSave FreeFile(0)
    Case printLabel: PrintSave 0
  End Select
  cmdCancel_Click
End Sub

Private Sub Form_Load()
  browse.Filename = CurDir & "\Untitled.txt"
  browse.Pattern = "*.txt"
'  progress1.Visible = False
End Sub

Private Sub Form_Resize()
  If Width > 4400 And Height > 550 Then
    If browse.Visible Then
      browse.Width = Width - 3288
      browse.Height = Height - 540
    End If
  End If
End Sub

Private Sub txtDelimiter_Change()
  txtDelimiter.SelStart = 0
  txtDelimiter.SelLength = Len(txtDelimiter.Text)
  optDelimiter(2).Value = True
End Sub

'fileNum = 0 for print, file handle for save
Private Sub PrintSave(fileNum%)
  Dim delim$, quote$
  Dim DoColTitles As Boolean, DoRowTitles As Boolean
  
  quote = ""
  
  If optDelimiter(0).Value = True Then
    delim = Chr(9)
  ElseIf optDelimiter(1).Value = True Then
    delim = " "
    'quote = txtQuote.Text
  ElseIf optDelimiter(2).Value = True Then
    delim = txtDelimiter.Text
  Else
    delim = ""
  End If
  DoColTitles = chkColTitles.Enabled And chkColTitles.Value
  mvarSaver.SavePrint fileNum, DoColTitles, browse.Filename, delim, txtEmptyCell.Text
  'mvarGrid.SavePrint fileNum, DoColTitles, "#", browse.Filename, delim, txtEmptyCell.Text
  
End Sub
