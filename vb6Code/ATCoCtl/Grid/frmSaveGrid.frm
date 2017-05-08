VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "COMCTL32.OCX"
Begin VB.Form frmSaveGrid 
   Caption         =   "Print or Save Grid As Text File"
   ClientHeight    =   3192
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   5376
   LinkTopic       =   "Form1"
   ScaleHeight     =   3192
   ScaleWidth      =   5376
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command1 
      Caption         =   "&Widths..."
      Enabled         =   0   'False
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
      Left            =   4200
      TabIndex        =   13
      Top             =   1800
      Visible         =   0   'False
      Width           =   975
   End
   Begin ComctlLib.ProgressBar progress1 
      Height          =   375
      Left            =   120
      TabIndex        =   12
      Top             =   2160
      Visible         =   0   'False
      Width           =   4980
      _ExtentX        =   8784
      _ExtentY        =   656
      _Version        =   327682
      Appearance      =   1
   End
   Begin VB.CommandButton cmdPrint 
      Caption         =   "P&rint"
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
      Left            =   2160
      TabIndex        =   10
      Top             =   2640
      Width           =   975
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "&Character delimited:"
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
      Left            =   1560
      TabIndex        =   5
      Top             =   1560
      Width           =   2175
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "T&ab delimited"
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
      Left            =   1560
      TabIndex        =   3
      Top             =   1080
      Value           =   -1  'True
      Width           =   2175
   End
   Begin VB.OptionButton optDelimiter 
      Caption         =   "S&pace delimited"
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
      Left            =   1560
      TabIndex        =   4
      Top             =   1320
      Width           =   2055
   End
   Begin VB.TextBox txtDelimiter 
      Height          =   285
      Left            =   3720
      TabIndex        =   6
      Text            =   ","
      ToolTipText     =   "Single printable character delimiter"
      Top             =   1515
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
      Left            =   1560
      TabIndex        =   7
      Top             =   1800
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
      Left            =   3360
      TabIndex        =   11
      Top             =   2640
      Width           =   975
   End
   Begin VB.CommandButton cmdSave 
      Caption         =   "&Save"
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
      Left            =   960
      TabIndex        =   9
      Top             =   2640
      Width           =   975
   End
   Begin VB.TextBox txtHeader 
      Height          =   765
      Left            =   1560
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   1
      Text            =   "frmSaveGrid.frx":0000
      ToolTipText     =   "Name of file containing data to import"
      Top             =   120
      Width           =   3495
   End
   Begin VB.CheckBox chkColTitles 
      Caption         =   "Use Column &Titles"
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
      Left            =   1560
      TabIndex        =   8
      ToolTipText     =   "After skipping n lines, read next record as list of attribute names"
      Top             =   2280
      Value           =   1  'Checked
      Width           =   2295
   End
   Begin MSComDlg.CommonDialog dlgOpenFile 
      Left            =   4320
      Top             =   1200
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      Filter          =   "Text files (*.txt)|*.txt|RDB files (*.rdb)|*.rdb|All Files (*.*)|*.*"
      FilterIndex     =   3
   End
   Begin VB.Label lblHeaderRecord 
      Caption         =   "&Header:"
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
      Left            =   120
      TabIndex        =   0
      Top             =   165
      Width           =   1455
   End
   Begin VB.Label lblFileType 
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
      Height          =   255
      Left            =   120
      TabIndex        =   2
      Top             =   1080
      Width           =   1455
   End
End
Attribute VB_Name = "frmSaveGrid"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private mvarGrid As ATCoGrid
Private mvarFilename As String
Private Const noHeader = "(none)"

Public Property Get Filename() As String
  Filename = mvarFilename
End Property

Public Property Let Filename(newName As String)
  mvarFilename = newName
End Property

Public Sub SaveGrid(agd As ATCoGrid)
  Dim enableColFormat As Boolean
  Dim Button&
  
  Set mvarGrid = agd
  
  If agd.header = "" Then
    txtHeader.Text = noHeader
  Else
    txtHeader.Text = agd.header
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

Private Sub browseFilename()
  dlgOpenFile.Filename = mvarFilename
  dlgOpenFile.ShowSave
  mvarFilename = dlgOpenFile.Filename
End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdPrint_Click()
  SavePrint 0
  cmdCancel_Click
End Sub

Private Sub cmdSave_Click()
  SavePrint FreeFile(0)
  cmdCancel_Click
End Sub

Private Sub Form_Load()
  'dlgOpenFile.filter = "Text files (*.txt)|*.txt|RDB files (*.rdb)|*.rdb|All Files (*.*)|*.*"
  'dlgOpenFile.FilterIndex = 3
  progress1.Visible = False
End Sub

Private Sub Form_Resize()
  If Width > 3000 Then
    txtHeader.Width = Width - 1785
    progress1.Width = Width - 400
  End If
End Sub

Private Sub txtDelimiter_Change()
  txtDelimiter.SelStart = 0
  txtDelimiter.SelLength = Len(txtDelimiter.Text)
End Sub

'fileNum = 0 for printer or file handle for file
Private Sub SavePrint(fileNum%)
  Dim delim$, header$, DoTitles As Boolean
  
  If optDelimiter(0).Value = True Then
    delim = Chr(9)
  ElseIf optDelimiter(1).Value = True Then
    delim = " "
  ElseIf optDelimiter(2).Value = True Then
    delim = txtDelimiter.Text
  Else
    delim = ""
  End If
  DoTitles = chkColTitles.Enabled And chkColTitles.Value
  If txtHeader.Text = noHeader Then header = "" Else header = txtHeader.Text
  mvarGrid.SavePrintGridBatch fileNum, DoTitles, DoTitles, header, "#", "", delim
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

