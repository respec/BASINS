VERSION 5.00
Begin VB.Form frmGridPrintSaveLoad 
   ClientHeight    =   4425
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7305
   LinkTopic       =   "Form1"
   ScaleHeight     =   4425
   ScaleWidth      =   7305
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoBrowse browse 
      Height          =   4212
      Left            =   3120
      TabIndex        =   16
      Top             =   120
      Width           =   4092
      _ExtentX        =   7223
      _ExtentY        =   7435
   End
   Begin VB.TextBox txtEmptyCell 
      Height          =   285
      Left            =   1800
      TabIndex        =   13
      ToolTipText     =   "Fill empty cells with this string"
      Top             =   3480
      Width           =   1095
   End
   Begin VB.TextBox txtQuote 
      Height          =   285
      Left            =   2640
      TabIndex        =   6
      Text            =   "'"
      ToolTipText     =   "Single printable character delimiter"
      Top             =   2040
      Width           =   255
   End
   Begin VB.CheckBox chkRowTitles 
      Caption         =   "Include &Row Titles"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   11
      Top             =   3168
      Value           =   1  'Checked
      Width           =   2295
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Character &delimited:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   2
      Left            =   240
      TabIndex        =   7
      Top             =   2316
      Width           =   2175
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "&Tab delimited"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   0
      Left            =   240
      TabIndex        =   3
      Top             =   1800
      Value           =   -1  'True
      Width           =   2175
   End
   Begin VB.TextBox txtDelimiter 
      Height          =   285
      Left            =   2640
      TabIndex        =   8
      Text            =   ","
      ToolTipText     =   "Single printable character delimiter"
      Top             =   2280
      Width           =   255
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Fi&xed width space padded"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   3
      Left            =   240
      TabIndex        =   9
      Top             =   2580
      Width           =   2655
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1800
      TabIndex        =   15
      Top             =   3960
      Width           =   975
   End
   Begin VB.CommandButton cmdGo 
      Caption         =   "Go"
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
      Height          =   375
      Left            =   360
      TabIndex        =   14
      Top             =   3960
      Width           =   975
   End
   Begin VB.TextBox txtHeader 
      Height          =   765
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   1
      Text            =   "frmGridPrintSaveLoad.frx":0000
      ToolTipText     =   "Text at beginning of file"
      Top             =   480
      Width           =   2652
   End
   Begin VB.CheckBox chkColTitles 
      Caption         =   "Include &Column Titles"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   10
      Top             =   2880
      Value           =   1  'Checked
      Width           =   2295
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "Sp&ace delimited"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   1
      Left            =   240
      TabIndex        =   4
      Top             =   2064
      Width           =   1695
   End
   Begin VB.Label Label2 
      Caption         =   "&Empty Cell Text:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   240
      TabIndex        =   12
      Top             =   3552
      Width           =   1452
   End
   Begin VB.Label Label1 
      Caption         =   "&Quote:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   2040
      TabIndex        =   5
      Top             =   2064
      Width           =   612
   End
   Begin VB.Label lblHeaderRecord 
      BackStyle       =   0  'Transparent
      Caption         =   "&Header Comment Character:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   168
      Width           =   3492
   End
   Begin VB.Label lblFileType 
      BackStyle       =   0  'Transparent
      Caption         =   "Column &Format:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   1440
      Width           =   1452
   End
End
Attribute VB_Name = "frmGridPrintSaveLoad"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Private mvarGrid As ATCoGrid
Private mvarFilename As String
Private Const noHeader = "(none)"
Private Const savelabel = "&Save"
Private Const printLabel = "&Print"
Private Const loadLabel = "&Load"

Public Property Get Filename() As String
  Filename = mvarFilename
End Property

Public Property Let Filename(newName As String)
  mvarFilename = newName
End Property

Public Sub LoadGrid(agd As ATCoGrid)
  Caption = "Specify Format of Text File"
  lblHeaderRecord.Caption = "&Header Comment Character:"
  cmdGo.Caption = loadLabel
  InitControls agd
  chkColTitles.Value = False
  optDelimiter(0).Enabled = True
  optDelimiter(1).Enabled = True
  optDelimiter(2).Enabled = True
  optDelimiter(3).Enabled = False
  browse.Visible = True
  If Width < 3500 Then Width = 9408
End Sub

Public Sub PrintGrid(agd As ATCoGrid)
  Caption = "Print Grid"
  lblHeaderRecord.Caption = "&Header:"
  If agd.header = "" Then
    txtHeader.Text = noHeader
  Else
    txtHeader.Text = agd.header
  End If
  cmdGo.Caption = printLabel
  InitControls agd
  Width = 3216
  browse.Visible = False
End Sub

Public Sub SaveGrid(agd As ATCoGrid)
  Caption = "Specify Format of Text File"
  lblHeaderRecord.Caption = "&Header:"
  If agd.header = "" Then
    txtHeader.Text = noHeader
  Else
    txtHeader.Text = agd.header
  End If
  cmdGo.Caption = savelabel
  InitControls agd
  browse.Visible = True
  If Width < 3500 Then Width = 9408
End Sub
  
Private Sub InitControls(agd As ATCoGrid)
  Dim enableColFormat As Boolean
  Dim Button&
  
  Set mvarGrid = agd
    
  If agd.FixedCols > 0 Then
    chkRowTitles.Enabled = True
  Else
    chkRowTitles.Enabled = False
  End If
  
  If agd.FixedRows > 0 Then
    chkColTitles.Enabled = True
  Else
    chkColTitles.Enabled = False
  End If
  
  If agd.cols > 1 Then enableColFormat = True Else enableColFormat = False
  
  For Button = 0 To optDelimiter.Count - 1
    optDelimiter(Button).Enabled = enableColFormat
  Next Button
  txtDelimiter.Enabled = enableColFormat
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
    Case savelabel:  PrintSaveLoad FreeFile(0)
    Case printLabel: PrintSaveLoad 0
    Case loadLabel:  PrintSaveLoad -9
  End Select
  cmdCancel_Click
End Sub

Private Sub Form_Load()
  browse.Filename = CurDir
  browse.Pattern = "*.txt"
'  progress1.Visible = False
End Sub

Private Sub Form_Resize()
  If ScaleWidth > 4300 And ScaleHeight > 550 Then
    If browse.Visible Then
      txtHeader.Width = 2652
      browse.Width = ScaleWidth - 3168
      browse.Height = ScaleHeight - 135
    Else
      txtHeader.Width = ScaleWidth - 420
    End If
  End If
End Sub

Private Sub txtDelimiter_Change()
  txtDelimiter.SelStart = 0
  txtDelimiter.SelLength = Len(txtDelimiter.Text)
  optDelimiter(2).Value = True
End Sub

'fileNum = 0 for print, -9 for load, file handle for save
Private Sub PrintSaveLoad(fileNum%)
  Dim delim$, header$, quote$
  Dim DoColTitles As Boolean, DoRowTitles As Boolean
  
  quote = ""
  
  If optDelimiter(0).Value = True Then
    delim = Chr(9)
  ElseIf optDelimiter(1).Value = True Then
    delim = " "
    quote = txtQuote.Text
  ElseIf optDelimiter(2).Value = True Then
    delim = txtDelimiter.Text
  Else
    delim = ""
  End If
  DoColTitles = chkColTitles.Enabled And chkColTitles.Value
  DoRowTitles = chkRowTitles.Enabled And chkRowTitles.Value
  If txtHeader.Text <> noHeader Then header = txtHeader.Text Else header = ""
  If fileNum = -9 Then
    mvarGrid.LoadFile header, DoColTitles, DoRowTitles, browse.Filename, delim, txtEmptyCell.Text, quote
  Else
    mvarGrid.SavePrintGridBatch fileNum, DoColTitles, DoRowTitles, header, "#", browse.Filename, delim, txtEmptyCell.Text, quote
  End If
End Sub

'u = 0 for printer or file handle for file
'Private Sub PrintSave(u%)
'  Dim r&, c&, ip&, ncol&, fname$
'  Dim colChars&()
'  Dim rdb As Boolean, delim$
'  Dim lstr$, colstr$, ltitl$, rdbfmt$, tstr$
'  Dim tFont As New StdFont
'
'retry:
'  rdb = False
  
'  If optDelimiter(0).value = True Then
'    rdb = True: delim = Chr(9)
'  ElseIf optDelimiter(1).value = True Then
'    rdb = True: delim = " "
'  ElseIf optDelimiter(2).value = True Then
'    rdb = True: delim = txtDelimiter.Text
'  Else
'    rdb = False
'  End If
  
  'On Error GoTo errhandler
'  If u = 0 Then 'print to printer
'    Dim fp&, tp&
'    fp = 1
'    tp = 1
'    Call ShowPrinterX(Me, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)

'    If tp < 0 Then Exit Sub 'Cancel selected in print dialog

'    tFont = Font
'    Set Printer.Font = tFont
'    Printer.FontBold = False
'  Else 'save to file
'    browseFilename
'    fname = mvarFilename
'    If fname = "" Then Exit Sub
'    If Len(Dir(fname)) > 0 Then Kill fname
'    Open fname For Output As #u
'  End If

'  With progress1
'    .Min = 1 - mvarGrid.FixedRows
'    .Max = mvarGrid.rows
'    .value = 0
'    .Visible = True
'  End With
'  If txtHeader.Text <> noHeader Then
'    If u = 0 Then
'      ltitl = ReplaceString(txtHeader, vbLf, vbLf & PrintMargin)
'      Printer.Print
'      Printer.Print
'      Printer.Print
'      Printer.Print PrintMargin & ltitl
'      Printer.Print
'    ElseIf rdb Then 'RDB file
'      ltitl = ReplaceString(txtHeader, vbLf, vbLf & "# ")
'      Print #u, "# " & ltitl
'      Print #u, "#"
'    Else 'text file
'      Print #u, txtHeader
'      Print #u,
'    End If
'  End If
'  lstr = ""
'  rdbfmt = ""
  
'  ncol = mvarGrid.cols
'  ReDim colChars(ncol)
'  Me.Font.Name = mvarGrid.gridFontName
'  For c = 0 To ncol - 1
'    colstr = "1"
'    While Me.TextWidth(colstr) < mvarGrid.colWidth(c)
'      colstr = colstr & "1"
'    Wend
'    colChars(c) = Len(colstr)
'  Next c
'
'  If chkColTitles.Enabled And chkColTitles.value Then
'    r = 1 - mvarGrid.FixedRows 'output column titles
'  Else
'    r = 1                      'skip column titles
'  End If
'  While r <= mvarGrid.rows
'    progress1.value = r
'    lstr = ""
'    For c = 0 To ncol - 1
'      colstr = mvarGrid.TextMatrix(r, c)
'      If rdb Then 'include delimiter
'        If c < ncol - 1 Then colstr = colstr & delim
'      Else 'add spaces to align columns
'        If Len(colstr) < colChars(c) Then colstr = colstr & SPACE(colChars(c) - Len(colstr))
'        colstr = colstr & " " 'always add at least one space between columns
'      End If
'      lstr = lstr & colstr
'    Next c
'    If u = 0 Then
'      Printer.Print PrintMargin & lstr
'    Else
'      Print #u, lstr
'    End If
'    r = r + 1
'  Wend
  'end printer output
'  If u = 0 Then
'    Printer.EndDoc
'  Else
'    Close #u
'  End If
'  progress1.Visible = False
'  Exit Sub
'errhandler:
'  Dim msg$
'  If u = 0 Then
'    msg = "Error printing grid:" & vbCr & Err.Description
'  Else
'    msg = "Error saving grid to file '" & fname & "':" & vbCr & Err.Description
'  End If
'  Select Case MsgBox(msg, vbAbortRetryIgnore)
'    Case vbAbort:  progress1.Visible = False: Exit Sub
'    Case vbRetry:  progress1.Visible = False: GoTo retry
'    Case vbIgnore:                            Resume Next
'  End Select
'End Sub

