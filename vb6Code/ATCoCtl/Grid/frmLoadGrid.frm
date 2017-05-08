VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Object = "{6B7E6392-850A-101B-AFC0-4210102A8DA7}#1.3#0"; "COMCTL32.OCX"
Begin VB.Form frmLoadGrid 
   Caption         =   "frmLoadGrid"
   ClientHeight    =   9720
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   13935
   LinkTopic       =   "Form1"
   ScaleHeight     =   9720
   ScaleWidth      =   13935
   Begin VB.Frame fraTestMapping 
      Caption         =   "fraTestMapping"
      Height          =   3015
      Left            =   120
      TabIndex        =   22
      Top             =   6120
      Visible         =   0   'False
      Width           =   5535
      Begin ATCoCtl.ATCoGrid agdTestMapping 
         Height          =   2415
         Left            =   0
         TabIndex        =   23
         Top             =   360
         Width           =   5055
         _extentx        =   8916
         _extenty        =   4260
         allowbigselection=   -1
         rows            =   1
         cols            =   1
         header          =   ""
         fixedrows       =   1
         fixedcols       =   0
         scrollbars      =   3
         selectionmode   =   0
         backcolor       =   -2147483643
         forecolor       =   -2147483640
         backcolorbkg    =   8421504
         backcolorsel    =   -2147483635
         forecolorsel    =   -2147483634
         backcolorfixed  =   -2147483633
         forecolorfixed  =   -2147483630
         insidelimitsbackground=   16777215
         outsidehardlimitbackground=   8421631
         outsidesoftlimitbackground=   8454143
      End
   End
   Begin VB.CommandButton cmdCommit 
      Caption         =   "Commit"
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
      Left            =   120
      TabIndex        =   21
      Top             =   9240
      Width           =   1215
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
      Left            =   3600
      TabIndex        =   16
      Top             =   9240
      Width           =   1215
   End
   Begin VB.Frame fraColSample 
      Caption         =   "fraColSample"
      Height          =   2055
      Left            =   7800
      TabIndex        =   6
      Top             =   3960
      Visible         =   0   'False
      Width           =   7335
      Begin ATCoCtl.ATCoGrid agdSample 
         Height          =   1815
         Left            =   0
         TabIndex        =   7
         Top             =   240
         Width           =   7335
         _extentx        =   12938
         _extenty        =   3201
         allowbigselection=   -1
         rows            =   1
         cols            =   1
         header          =   ""
         fixedrows       =   1
         fixedcols       =   0
         scrollbars      =   3
         selectionmode   =   2
         backcolor       =   -2147483643
         forecolor       =   -2147483640
         backcolorbkg    =   8421504
         backcolorsel    =   -2147483635
         forecolorsel    =   -2147483634
         backcolorfixed  =   -2147483633
         forecolorfixed  =   -2147483630
         insidelimitsbackground=   0
         outsidehardlimitbackground=   0
         outsidesoftlimitbackground=   0
      End
   End
   Begin VB.CommandButton cmdSaveDDF 
      Caption         =   "Save Description"
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
      Left            =   1560
      TabIndex        =   14
      Top             =   9240
      Width           =   1815
   End
   Begin VB.Frame fraDataMapping 
      Caption         =   "fraDataMapping"
      Height          =   3135
      Left            =   7800
      TabIndex        =   12
      Top             =   480
      Visible         =   0   'False
      Width           =   5535
      Begin ATCoCtl.ATCoGrid agdDataMapping 
         Height          =   2415
         Left            =   0
         TabIndex        =   13
         Top             =   240
         Width           =   5055
         _extentx        =   8916
         _extenty        =   4260
         allowbigselection=   -1
         rows            =   1
         cols            =   2
         header          =   ""
         fixedrows       =   1
         fixedcols       =   0
         scrollbars      =   3
         selectionmode   =   0
         backcolor       =   -2147483643
         forecolor       =   -2147483640
         backcolorbkg    =   8421504
         backcolorsel    =   -2147483635
         forecolorsel    =   -2147483634
         backcolorfixed  =   -2147483633
         forecolorfixed  =   -2147483630
         insidelimitsbackground=   16777215
         outsidehardlimitbackground=   8421631
         outsidesoftlimitbackground=   8454143
      End
   End
   Begin VB.Frame fraFileProperties 
      BorderStyle     =   0  'None
      Caption         =   "Input File Properties"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3195
      Left            =   240
      TabIndex        =   0
      Top             =   540
      Width           =   7215
      Begin VB.TextBox txtHeader 
         Height          =   765
         Left            =   1680
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   34
         Text            =   "frmLoadGrid.frx":0000
         ToolTipText     =   "Name of file containing data to import"
         Top             =   2400
         Width           =   5175
      End
      Begin VB.Frame Frame1 
         Caption         =   "Columns"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1375
         Left            =   240
         TabIndex        =   28
         Top             =   860
         Width           =   2925
         Begin VB.TextBox txtDelimiter 
            Height          =   285
            Left            =   2280
            TabIndex        =   33
            Text            =   ","
            ToolTipText     =   "Single printable character delimiter"
            Top             =   720
            Width           =   255
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
            Left            =   120
            TabIndex        =   32
            Top             =   495
            Width           =   1695
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
            Left            =   120
            TabIndex        =   31
            Top             =   1020
            Width           =   2655
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
            Left            =   120
            TabIndex        =   30
            Top             =   240
            Value           =   -1  'True
            Width           =   2175
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
            Left            =   120
            TabIndex        =   29
            Top             =   765
            Width           =   2175
         End
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
         Left            =   3360
         TabIndex        =   26
         Top             =   1080
         Value           =   1  'Checked
         Width           =   2295
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
         Left            =   3360
         TabIndex        =   25
         Top             =   1365
         Value           =   1  'Checked
         Width           =   2295
      End
      Begin VB.TextBox txtQuote 
         Height          =   285
         Left            =   4920
         TabIndex        =   24
         Text            =   "'"
         ToolTipText     =   "Single printable character delimiter"
         Top             =   1920
         Width           =   255
      End
      Begin VB.TextBox txtQuoteChar 
         Height          =   285
         Index           =   1
         Left            =   4920
         TabIndex        =   20
         ToolTipText     =   "String indicating missing data"
         Top             =   1680
         Width           =   855
      End
      Begin VB.CommandButton cmdBrowseDD 
         Caption         =   "Browse"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   6000
         TabIndex        =   18
         Top             =   480
         Width           =   855
      End
      Begin VB.CommandButton cmdBrowseName 
         Caption         =   "Browse"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   6000
         TabIndex        =   17
         Top             =   120
         Width           =   855
      End
      Begin VB.TextBox txtDDFilename 
         Height          =   285
         Left            =   1800
         TabIndex        =   3
         Text            =   "txtDDFilename"
         ToolTipText     =   "Control file containing mapping from fields in input file to fileds in NWIS database"
         Top             =   480
         Width           =   4095
      End
      Begin VB.TextBox txtFilename 
         Height          =   285
         Left            =   1800
         TabIndex        =   2
         Text            =   "txtFilename"
         ToolTipText     =   "Name of file containing data to import"
         Top             =   120
         Width           =   4095
      End
      Begin VB.Label lblHeaderRecord 
         Caption         =   "&Header:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Left            =   240
         TabIndex        =   35
         Top             =   2445
         Width           =   1335
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "&Quote Character:"
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
         Left            =   3360
         TabIndex        =   27
         Top             =   1935
         Width           =   1575
      End
      Begin VB.Label lblNullChar 
         BackStyle       =   0  'Transparent
         Caption         =   "Missing Value:"
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
         Left            =   3360
         TabIndex        =   19
         Top             =   1680
         Width           =   1575
      End
      Begin VB.Label lblDDFilename 
         Caption         =   "Data Description:"
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
         TabIndex        =   4
         Top             =   480
         Width           =   1575
      End
      Begin VB.Label lblName 
         Caption         =   "File:"
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
         TabIndex        =   1
         Top             =   120
         Width           =   1575
      End
   End
   Begin ComctlLib.TabStrip TabStrip1 
      Height          =   3735
      Left            =   120
      TabIndex        =   11
      Top             =   120
      Width           =   7575
      _ExtentX        =   13361
      _ExtentY        =   6588
      _Version        =   327682
      BeginProperty Tabs {0713E432-850A-101B-AFC0-4210102A8DA7} 
         NumTabs         =   2
         BeginProperty Tab1 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "File Format"
            Key             =   ""
            Object.Tag             =   ""
            Object.ToolTipText     =   ""
            ImageVarType    =   2
         EndProperty
         BeginProperty Tab2 {0713F341-850A-101B-AFC0-4210102A8DA7} 
            Caption         =   "Column Mapping"
            Key             =   ""
            Object.Tag             =   ""
            Object.ToolTipText     =   ""
            ImageVarType    =   2
         EndProperty
      EndProperty
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   0
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Frame fraTextSample 
      BorderStyle     =   0  'None
      Height          =   2175
      Left            =   120
      TabIndex        =   5
      Top             =   3840
      Width           =   7335
      Begin VB.TextBox txtSample 
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1695
         Left            =   0
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   3  'Both
         TabIndex        =   8
         Text            =   "frmLoadGrid.frx":0007
         Top             =   480
         Width           =   7215
      End
      Begin VB.TextBox txtColPos 
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         HideSelection   =   0   'False
         Index           =   1
         Left            =   0
         Locked          =   -1  'True
         ScrollBars      =   1  'Horizontal
         TabIndex        =   10
         Text            =   $"frmLoadGrid.frx":000D
         ToolTipText     =   "Select fixed width columns by dragging along the ruler."
         Top             =   240
         Width           =   7215
      End
      Begin VB.TextBox txtColPos 
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Courier New"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         HideSelection   =   0   'False
         Index           =   0
         Left            =   0
         Locked          =   -1  'True
         ScrollBars      =   1  'Horizontal
         TabIndex        =   9
         Text            =   $"frmLoadGrid.frx":00B0
         ToolTipText     =   "Select fixed width columns by dragging along the ruler."
         Top             =   0
         Width           =   7215
      End
   End
   Begin VB.Frame hSash 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   100
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   15
      Top             =   3840
      Width           =   7695
   End
End
Attribute VB_Name = "frmLoadGrid"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Const MaxNWISCols = 50
Const delimItemText = "ASCII Delimited"
Const fixedItemText = "ASCII Fixed Columns"
Const UseFullNames = True

Dim InputFileHandle%
Dim nNWISColNames&(), nViewNames&
Dim NWISColName$(), viewName$()
Dim FixedColLeft&(99), FixedColRight&(99), nFixedCols&
Dim hSashDragging As Boolean
Dim delim$, delimQ As Boolean 'delimiter text, boolean for whether file is delimited

Private Sub agdDataMapping_Click()
  Static lastclick&, lastrow&, lastcol&
  With agdDataMapping
    If .MaxOccupiedRow < 1 Then
      cmdSelectFields_Click
    ElseIf Timer - lastclick > 1 Or lastrow <> .row Or lastcol <> .col Then
      If agdDataMapping.col = dmLookupCol Then AskForLookupFilename
    End If
    lastclick = Timer
    lastrow = .row
    lastcol = .col
  End With
End Sub

Private Sub agdDataMapping_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim r&
  With agdDataMapping
    
    'blank out dmInputCol and dmLookupCol if a dmConstantCol is entered
    If ChangeFromCol <= dmConstantCol And ChangeToCol >= dmConstantCol Then
      For r = ChangeFromRow To ChangeToRow
        If Len(.TextMatrix(r, dmConstantCol)) > 0 Then
          .TextMatrix(r, dmInputCol) = ""
          .TextMatrix(r, dmLookupCol) = ""
        End If
      Next r
    End If
    
    'blank out dmConstantCol if dmInputCol or dmLookupCol is entered
    If ChangeFromCol <= dmInputCol And ChangeToCol >= dmInputCol Or ChangeFromCol <= dmLookupCol And ChangeToCol >= dmLookupCol Then
      For r = ChangeFromRow To ChangeToRow
        If Len(.TextMatrix(r, dmInputCol)) > 0 Or Len(.TextMatrix(r, dmLookupCol)) > 0 Then
          .TextMatrix(r, dmConstantCol) = ""
        End If
      Next r
    End If
    
  End With
    
End Sub

Private Sub agdDataMapping_RowColChange()
  Static lastrow&, InRowColChange As Boolean
  Dim ics$, icl&, newrow&, newcol&
  If InRowColChange Then Exit Sub
  InRowColChange = True
  With agdDataMapping
    If lastrow > 0 And lastrow <> .row And lastrow <= .rows Then
      newrow = .row
      newcol = .col
      .row = lastrow
      .col = 0
      .CellBackColor = .BackColor
      .CellForeColor = .ForeColor
      .col = 1
      .CellBackColor = .BackColor
      .CellForeColor = .ForeColor
      .row = newrow
      .col = newcol
    End If
    If .row <> lastrow Then
      newcol = .col
      .col = 0
      .CellBackColor = .BackColorSel
      .CellForeColor = .ForeColorSel
      .col = 1
      .CellBackColor = .BackColorSel
      .CellForeColor = .ForeColorSel
      .col = newcol
      lastrow = .row
    End If
    If .col = dmDBView Or .col = dmDBFieldCol Or .col = dmLookupCol Then
      .ColEditable(.col) = False
    Else
      .ColEditable(.col) = True
      .ClearValues
      'Should this be moved ???
      If .col = dmInputCol And delimQ Then SetValidInputColNames
    End If
    ics = .TextMatrix(.row, dmInputCol)
    If Len(ics) > 0 Then
      If delimQ Then
        If IsNumeric(ics) Then
          icl = CLng(ics) - 1
          If icl <> agdSample.SelStartCol Then agdSample.Selected(1, icl) = True
        End If
      ElseIf .row <= nFixedCols Then
        If FixedColLeft(.row) > 0 Then
          txtColPos(0).SelStart = FixedColLeft(.row) - 1
          txtColPos(1).SelStart = txtColPos(0).SelStart
        End If
        txtColPos(0).SelLength = FixedColRight(.row) - txtColPos(0).SelStart
        txtColPos(1).SelLength = txtColPos(0).SelLength
      End If
    End If
  End With
  InRowColChange = False
End Sub

Private Sub agdSample_Click()
  'Select column means put this column number in current row af mapping
  If agdSample.SelStartCol = agdSample.SelEndCol And agdSample.SelStartRow = 1 And agdSample.SelEndRow = agdSample.rows Then
    agdDataMapping.TextMatrix(agdDataMapping.row, dmInputCol) = agdSample.ColTitle(agdSample.SelStartCol)
  End If
End Sub

Private Sub chkCollapseDelim_Click()

End Sub

Private Sub chkHeader_Click()
  If fraColSample.Visible Then PopulateGridSample
End Sub

Private Sub cmdBrowseDD_Click()
  txtDDFilename_Click
End Sub

Private Sub cmdBrowseName_Click()
  txtFilename_Click
End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdSaveDDF_Click()
  Dim outf%
  OutputFileOpen outf
  If outf >= 0 Then
    WriteDDF outf
    Close outf
  End If
End Sub

Private Sub ComboFileType_Click()
  If ComboFileType.Text = delimItemText Then
    delimQ = True
    'txtSample.Top = txtColPos(0).Top
    fraTextSample.Visible = False
    fraColSample.Visible = True
    If fraColSample.Visible Then PopulateGridSample
    
    agdDataMapping.ColTitle(dmInputCol) = "Input Column"
  Else
    delimQ = False
    fraColSample.Visible = False
    fraTextSample.Visible = True
    txtSample.Top = txtColPos(1).Top + txtColPos(1).Height
    
    agdDataMapping.ColTitle(dmInputCol) = "Beg-End Column"
  End If
  If fraTextSample.Height > txtSample.Top Then txtSample.Height = fraTextSample.Height - txtSample.Top
  optDelimiter(0).Enabled = delimQ
  optDelimiter(1).Enabled = delimQ
  optDelimiter(2).Enabled = delimQ
  txtDelimiter.Enabled = delimQ
  lblDelimiter.Enabled = delimQ
End Sub

Private Function SiteNumFromName&(name$)
  Dim num&
  SiteNumFromName = 0
  num = 1
  While num <= nViewNames
    If viewName(num) = name Then SiteNumFromName = num: num = nViewNames
    num = num + 1
  Wend
End Function

Private Sub SetViewNames()
  Dim view&
  
  ReDim NWISColName(1 To nViewNames, 1 To MaxNWISCols)
  ReDim nNWISColNames(1 To nViewNames)
  
  'ComboAddView.Clear
  For view = 1 To nViewNames
  '  ComboAddView.AddItem viewName(view)
  '  ComboAddView.ItemData(view - 1) = view
    InitNWISColNames view
  Next view
  'ComboAddView.Text = viewName(1)
  
End Sub

Private Sub SetValidInputColNames()
  'If Not fraColSample.Visible Then
  '  tabSample.SelectedItem = tabSample.Tabs.Item(2)
  '  tabSample_Click
  'End If
  Dim c&, t$
  For c = 0 To agdSample.cols - 1
    t = agdSample.ColTitle(c)
    If delimQ Then
      If t <> CStr(c + 1) Then t = t & " (" & c + 1 & ")"
    End If
    agdDataMapping.addValue t
  Next c
End Sub

Private Sub InitNWISColNames(view&)
  Dim colnum&
'  With agdDataMapping
'    .Rows = nNWISColNames(view)
'    For colnum = 1 To nNWISColNames(view)
'      .TextMatrix(colnum, 0) = NWISColName(view, colnum)
'      .TextMatrix(colnum, 1) = "0"
'      .TextMatrix(colnum, 5) = "No"
'    Next colnum
'  End With
End Sub

Private Sub Form_Load()
  Dim frapos&
  frapos = 240
  fraFileProperties.Left = frapos
  fraDataMapping.Left = frapos
  fraTestMapping.Left = frapos
  
  fraTextSample.Left = TabStrip1.Left
  fraColSample.Left = fraTextSample.Left
  
  fraFileProperties.BorderStyle = 0
  fraDataMapping.BorderStyle = 0
  fraTestMapping.BorderStyle = 0
  fraDataMapping.Top = fraFileProperties.Top
  fraTestMapping.Top = fraFileProperties.Top
  
  fraTextSample.BorderStyle = 0
  fraColSample.BorderStyle = 0
  
  fraTextSample.Top = hSash.Top + hSash.Height
  fraColSample.Top = fraTextSample.Top
  
  agdSample.Top = 0
  agdSample.Left = 0
  
  agdTestMapping.Top = 0
  agdTestMapping.Left = 0
  
  With agdDataMapping
    .Left = 0
    .Top = 0
    .cols = 5
    .ColTitle(dmDBFieldCol) = "NWIS Name"
    .ColTitle(dmDBView) = "View Name"
    .ColTitle(dmInputCol) = "Input Column"
    .ColTitle(dmConstantCol) = "Constant"
    .ColTitle(dmLookupCol) = "Lookup Table"
    '.ColTitle(dmConstraintCol) = "Constraint"
    '.ColTitle(dmBlanksCol) = "Write Blanks"
  End With
  SetViewNames
  txtDDFilename.Text = ""
  txtFilename_Click
  'InputFileOpen
  'If InputFileHandle >= 0 Then PopulateTxtSample
  Form_Resize
  ComboFileType.Clear
  ComboFileType.AddItem delimItemText
  ComboFileType.AddItem fixedItemText
  ComboFileType.Text = delimItemText
  delim = txtDelimiter.Text
End Sub

Private Sub Form_Resize()
  Dim tabWidth&, fraWidth&, tabheight&, oldtxtsampleheight&
  oldtxtsampleheight = txtSample.Height
  On Error Resume Next
  hSash.Width = Width
  tabWidth = Width - 400
  If tabWidth > 500 Then
    TabStrip1.Width = tabWidth
    'tabSample.Width = tabWidth
    
    fraTextSample.Width = tabWidth
    txtSample.Width = fraTextSample.Width
    txtColPos(0).Width = fraTextSample.Width
    txtColPos(1).Width = fraTextSample.Width
    
    fraColSample.Width = tabWidth
    agdSample.Width = fraColSample.Width
    
    fraWidth = tabWidth - 325
    If fraWidth > 500 Then
      
      
      fraFileProperties.Width = fraWidth
      fraDataMapping.Width = fraWidth
      agdDataMapping.Width = fraWidth
      fraTestMapping.Width = fraWidth
      agdTestMapping.Width = fraWidth
      If fraWidth > txtFilename.Left + cmdBrowseName.Width Then
        cmdBrowseName.Left = fraWidth - cmdBrowseName.Width
        cmdBrowseDD.Left = cmdBrowseName.Left
        txtFilename.Width = fraWidth - txtFilename.Left - cmdBrowseName.Width - 100
        txtDDFilename.Width = txtFilename.Width
      End If
    End If
  End If
  
  cmdSaveDDF.Top = Height - 915
  cmdAdd.Top = cmdSaveDDF.Top
  cmdModify.Top = cmdSaveDDF.Top
  cmdCommit.Top = cmdSaveDDF.Top
  cmdCancel.Top = cmdSaveDDF.Top
  
  If TabStrip1.Height > 640 Then
    Select Case TabStrip1.SelectedItem.index
      Case 1: fraFileProperties.Visible = True
      Case 2: fraDataMapping.Visible = True
      Case 3: fraTestMapping.Visible = True
    End Select
    fraFileProperties.Height = TabStrip1.Height - 540
    fraDataMapping.Height = fraFileProperties.Height
    fraTestMapping.Height = fraFileProperties.Height
    agdTestMapping.Height = fraFileProperties.Height
  Else
    fraFileProperties.Visible = False
    fraDataMapping.Visible = False
    fraTestMapping.Visible = False
  End If
  If cmdSaveDDF.Top > fraTextSample.Top + 200 Then ' tabSample.Top + 200 Then
    'tabSample.Visible = True
    fraTextSample.Height = cmdSaveDDF.Top - fraTextSample.Top - 100
    fraColSample.Height = fraTextSample.Height
    If ComboFileType.Text = fixedItemText Then fraTextSample.Visible = True Else fraColSample.Visible = True
    'If tabSample.Height > 700 Then
      'If tabSample.SelectedItem.Index = 1 Then fraTextSample.Visible = True Else fraColSample.Visible = True
      'fraTextSample.Top = tabSample.Top + 440
      'fraColSample.Top = fraTextSample.Top
      'fraTextSample.Height = tabSample.Height - 600
      'fraColSample.Height = fraTextSample.Height
      agdSample.Height = fraColSample.Height
      If fraTextSample.Height > txtSample.Top Then txtSample.Height = fraTextSample.Height - txtSample.Top
    'Else
    '  fraTextSample.Visible = False
    '  fraColSample.Visible = False
    'End If
  Else
    fraTextSample.Visible = False
    fraColSample.Visible = False
'    tabSample.Visible = False
  End If
  cmdSelectFields.Top = fraDataMapping.Height - 400
  'cmdCopyColumns.Top = cmdSelectFields.Top
  'ComboAddView.Top = cmdSelectFields.Top + 20
  If cmdSelectFields.Top > agdDataMapping.Top + 200 Then agdDataMapping.Height = cmdSelectFields.Top - 100
End Sub

Private Sub InputFileOpen(setFileHandle%)
  'sets InputFileHandle to read input file or -1 on failure
  Dim filename$, FileHandle%
  setFileHandle = -1
  txtFilename.Text = ""
  On Error GoTo exitsub
  With CommonDialog1
    .filter = "All Files (*.*)|*.*"
    .ShowOpen
    filename = .filename
    If Len(filename) = 0 Then Exit Sub
  End With
  FileHandle = FreeFile(0)
  Open filename For Input As #FileHandle
  setFileHandle = FileHandle
  txtFilename.Text = filename
exitsub:
End Sub

Private Sub OutputFileOpen(setFileHandle%)
  'sets setFileHandle to read input file or -1 on failure
  Dim filename$, FileHandle%
  setFileHandle = -1
  txtDDFilename.Text = ""
  On Error GoTo exitsub
  With CommonDialog1
    .filter = "Data Description Files (*.DDF)|*.DDF"
    .ShowSave
    filename = .filename
    If Len(filename) = 0 Then Exit Sub
  End With
  FileHandle = FreeFile(0)
  Open filename For Output As #FileHandle
  setFileHandle = FileHandle
  txtDDFilename.Text = filename
exitsub:
End Sub

'returns number of columns parsed from buffer into array
'populates parsed array from element 1 to index returned
Private Function ParseInputLine&(ByVal inBuf$, parsed$())
  Dim parseCol&, fromCol&, toCol&
  parseCol = 0
  fromCol = 1
  If delimQ Then 'parse delimited text
    While fromCol <= Len(inBuf) And parseCol < UBound(parsed)
      toCol = InStr(fromCol, inBuf, delim)
      If chkCollapseDelim.value = 1 Then 'treat multiple contiguous delimiters as one
        While toCol = fromCol And toCol < Len(inBuf)
          toCol = fromCol + 1
          toCol = InStr(fromCol, inBuf, delim)
        Wend
      End If
      If toCol < fromCol Then toCol = Len(inBuf) + 1
      parseCol = parseCol + 1
      parsed(parseCol) = Mid(inBuf, fromCol, toCol - fromCol)
      fromCol = toCol + 1
    Wend
  Else 'fixed columns
    While parseCol < nFixedCols
      parseCol = parseCol + 1
      If FixedColRight(parseCol) > 0 Then
        parsed(parseCol) = Mid(inBuf, FixedColLeft(parseCol), FixedColRight(parseCol) - FixedColLeft(parseCol) + 1)
      Else
        parsed(parseCol) = ""
      End If
    Wend
  End If
  If parseCol > UBound(parsed) Then parseCol = UBound(parsed)
  ParseInputLine = parseCol
End Function

Private Sub PopulateGridTest()
  Dim lines&, linecnt&, cbuff$, parsed$(MaxNWISCols), pcols&
  Dim cout&, cin& 'column out (agdTestMapping), in (agdDataMapping)
  Dim icl&, ics$ 'input column long, string
  Seek InputFileHandle, 1
  If IsNumeric(txtSkip.Text) Then
    linecnt = 0
    lines = CLng(txtSkip.Text)
    While linecnt < lines And Not EOF(InputFileHandle)
      Line Input #InputFileHandle, cbuff
      linecnt = linecnt + 1
    Wend
  End If
  With agdTestMapping
    .Clear
    .cols = agdDataMapping.MaxOccupiedRow
    .rows = 1
    lines = 50 'txtSample.Height / TextHeight("X")
    linecnt = 0
    For cout = 0 To .cols - 1
      .ColTitle(cout) = agdDataMapping.TextMatrix(cout + 1, dmDBFieldCol) & ", " & agdDataMapping.TextMatrix(cout + 1, dmDBView)
    Next cout
    If chkHeader.value = 1 Then Line Input #InputFileHandle, cbuff
    While Not EOF(InputFileHandle) And linecnt < lines
      Line Input #InputFileHandle, cbuff
      pcols = ParseInputLine(cbuff, parsed)
      linecnt = linecnt + 1
      For cout = 0 To .cols - 1
        ics = agdDataMapping.TextMatrix(cout + 1, dmInputCol)
        If Len(ics) > 0 Then
          If delimQ Then
            If IsNumeric(ics) Then
              icl = CLng(ics)
              If icl <= pcols Then .TextMatrix(linecnt, cout) = parsed(icl)
            End If
          Else
            .TextMatrix(linecnt, cout) = parsed(cout + 1)
          End If
        Else
          .TextMatrix(linecnt, cout) = agdDataMapping.TextMatrix(cout + 1, dmConstantCol)
        End If
      Next cout
    Wend
    Seek InputFileHandle, 1
  End With
End Sub

Private Sub PopulateGridSample()
  Dim lines&, linecnt&, cbuff$, parsed$(MaxNWISCols), pcols&, c&
  Seek InputFileHandle, 1
  If IsNumeric(txtSkip.Text) Then
    linecnt = 0
    lines = CLng(txtSkip.Text)
    While linecnt < lines And Not EOF(InputFileHandle)
      Line Input #InputFileHandle, cbuff
      linecnt = linecnt + 1
    Wend
  End If
  If fraColSample.Visible Then
    With agdSample
      .Clear
      .cols = 1
      .rows = 1
      lines = 50 'txtSample.Height / TextHeight("X")
      linecnt = 0
      If chkHeader.value = 1 Then
        Line Input #InputFileHandle, cbuff
        pcols = ParseInputLine(cbuff, parsed)
        .cols = pcols
        For c = 1 To pcols
          .ColTitle(c - 1) = parsed(c)
          .ColEditable(c - 1) = True
        Next c
      End If
      While Not EOF(InputFileHandle) And linecnt < lines
        Line Input #InputFileHandle, cbuff
        pcols = ParseInputLine(cbuff, parsed)
        linecnt = linecnt + 1
        For c = 1 To pcols
          .TextMatrix(linecnt, c - 1) = parsed(c)
        Next c
      Wend
      If chkHeader.value <> 1 Then 'number columns
        For c = 0 To .cols - 1
          If delimQ Then .ColTitle(c) = c + 1 Else .ColTitle(c) = FixedColLeft(c + 1) & "-" & FixedColRight(c + 1)
        Next c
      End If
      Seek InputFileHandle, 1
    End With
  Else
  End If
End Sub

Private Sub PopulateTxtSample()
  Dim lines&, linecnt&, cbuff$
  On Error GoTo exitsub
  With txtSample
    .Text = ""
    lines = 50 'txtSample.Height / TextHeight("X")
    linecnt = 0
    Seek InputFileHandle, 1
    While Not EOF(InputFileHandle) And linecnt < lines
      Line Input #InputFileHandle, cbuff
      .Text = .Text & cbuff & vbCrLf
      linecnt = linecnt + 1
    Wend
    Seek InputFileHandle, 1
  End With
exitsub:
End Sub

Private Sub hSash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  hSashDragging = True
End Sub

Private Sub hSash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim newHeight&
  If hSashDragging And (hSash.Top + Y) > 0 And (hSash.Top + Y < Height) Then
    hSash.Top = hSash.Top + Y
    If hSash.Top < TabStrip1.Top + 100 Then hSash.Top = TabStrip1.Top + 100
    fraTextSample.Top = hSash.Top + hSash.Height
    fraColSample.Top = fraTextSample.Top
    TabStrip1.Height = hSash.Top - TabStrip1.Top
    Form_Resize
  End If
End Sub

Private Sub hSash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  hSashDragging = False
End Sub

Private Sub optDelimiter_Click(index As Integer)
  Select Case index
    Case 0: delim = " "
    Case 1: delim = Chr$(9)
    Case 2: delim = txtDelimiter.Text
  End Select
  If fraColSample.Visible Then PopulateGridSample
End Sub

'Private Sub tabSample_Click()
'  Select Case tabSample.SelectedItem.Index
'  Case 1:
'    fraColSample.Visible = False
'    fraTextSample.Visible = True
'  Case 2:
'    fraTextSample.Visible = False
'    fraColSample.Visible = True
'    PopulateGridSample
'  End Select
'End Sub

Private Sub TabStrip1_Click()
  Select Case TabStrip1.SelectedItem.index
  Case 1:
    fraDataMapping.Visible = False
    fraTestMapping.Visible = False
    fraFileProperties.Visible = True
  Case 2:
    fraFileProperties.Visible = False
    fraTestMapping.Visible = False
    If agdDataMapping.MaxOccupiedRow < 1 Then cmdSelectFields_Click
    fraDataMapping.Visible = True
  Case 3:
    fraFileProperties.Visible = False
    fraDataMapping.Visible = False
    PopulateGridTest
    fraTestMapping.Visible = True
  End Select
End Sub

'Private Sub txtColPos_KeyDown(Index As Integer, KeyCode As Integer, Shift As Integer)
'  txtColPosAnyChange Index
'End Sub
'
'Private Sub txtColPos_KeyUp(Index As Integer, KeyCode As Integer, Shift As Integer)
'  txtColPosAnyChange Index
'End Sub

Private Sub txtColPos_MouseDown(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  txtColPosAnyChange index
End Sub

Private Sub txtColPos_MouseMove(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  txtColPosAnyChange index
End Sub

Private Sub txtColPos_MouseUp(index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  txtColPosAnyChange index
  With txtColPos(0)
    If .SelLength > 0 Then
      agdDataMapping.TextMatrix(agdDataMapping.row, dmInputCol) = .SelStart + 1 & "-" & .SelStart + .SelLength
      SetFixedWidthsFromDataMapping
    End If
  End With
End Sub

Private Sub txtColPosAnyChange(index)
  Dim Other&
  If index = 0 Then Other = 1 Else Other = 0
  txtColPos(Other).SelStart = txtColPos(index).SelStart
  txtColPos(Other).SelLength = txtColPos(index).SelLength
  'Debug.Print txtColPos(Index).SelStart & " -> " & txtColPos(Index).SelLength
End Sub

Private Sub txtFilename_Click()
  If InputFileHandle > 0 Then Close InputFileHandle
  InputFileOpen InputFileHandle
  If InputFileHandle >= 0 Then PopulateTxtSample
End Sub

Private Sub txtDDFilename_Click()
  Dim DDFileHandle%
  InputFileOpen DDFileHandle
  If DDFileHandle >= 0 Then
    ReadDDF DDFileHandle
    Close DDFileHandle
  End If
End Sub

Private Sub txtDelimiter_Change()
  optDelimiter(2).value = True
  delim = txtDelimiter.Text
  If fraColSample.Visible Then PopulateGridSample
End Sub

Private Sub txtQuoteChar_KeyPress(index As Integer, KeyAscii As Integer)
  PopulateGridSample
End Sub

Private Sub txtSkip_Change()
  If fraColSample.Visible Then PopulateGridSample
End Sub

Private Sub AskForLookupFilename()
  CommonDialog1.filter = "All Files (*.*)|*.*"
  CommonDialog1.ShowOpen
  With agdDataMapping
    .TextMatrix(.row, dmLookupCol) = CommonDialog1.filename
    If Len(CommonDialog1.filename) > 0 Then .TextMatrix(.row, dmConstantCol) = ""
  End With
End Sub

Public Sub SetFixedWidthsFromDataMapping()
  Dim r&, colspec$, dashpos& ', existing&
  nFixedCols = 0
  For r = 1 To agdDataMapping.MaxOccupiedRow
    colspec = agdDataMapping.TextMatrix(r, dmInputCol)
    nFixedCols = nFixedCols + 1
    If Len(colspec) > 0 Then
      dashpos = InStr(colspec, "-")
      If dashpos > 0 Then   'range (left-right)
        FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
        FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1)
      Else
        dashpos = InStr(colspec, "+")
        If dashpos > 0 Then 'range (left+length)
          FixedColLeft(nFixedCols) = Left(colspec, dashpos - 1)
          FixedColRight(nFixedCols) = Mid(colspec, dashpos + 1) + FixedColLeft(nFixedCols)
        Else                'Single number, assume single character column
          FixedColLeft(nFixedCols) = colspec
          FixedColRight(nFixedCols) = colspec
        End If
      End If
      'existing = 1
      'While existing < nFixedCols 'check to see if this column is already listed (when same input goes to more than one field)
      '  If FixedColLeft(existing) = FixedColLeft(nFixedCols) Then nFixedCols = nFixedCols - 1: existing = nFixedCols
      '  existing = existing + 1
      'Wend
    Else
      FixedColLeft(nFixedCols) = 0
      FixedColRight(nFixedCols) = 0
    End If
  Next r
  'Debug.Print "SetFixedWidthsFromDataMapping: nFixedCols=" & nFixedCols
End Sub

Private Sub ReadDDF(infile%)
  Dim buf$, ubuf$, view$, field$, clearedDataMapping As Boolean, eq&, r&, endpos&
  clearedDataMapping = False
  While Not EOF(infile)
    Line Input #infile, buf
    If Left(buf, 1) <> "#" Then 'It is not a comment
      ubuf = UCase(buf)
      
      'File Properties
      If Left(ubuf, 9) = "FILETYPE=" Then
        ComboFileType.Text = Mid(buf, 10)
        ComboFileType_Click
      ElseIf Left(ubuf, 10) = "DELIMITER=" Then
        delim = Mid(buf, 11)
        Select Case delim
          Case "SPACE": delim = " ": optDelimiter(0).value = True
          Case "TAB": delim = Chr$(9): optDelimiter(1).value = True
          Case Else:  optDelimiter(2).value = True
        End Select
      ElseIf Left(ubuf, 10) = "SKIPLINES=" Then
        txtSkip.Text = Mid(buf, 11)
      ElseIf Left(ubuf, 7) = "HEADER=" Then
        If Mid(ubuf, 8, 1) = "Y" Then chkHeader.value = 1 Else chkHeader.value = 0
      ElseIf Left(ubuf, 6) = "QUOTE=" Then
        txtQuoteChar(0).Text = Mid(ubuf, 7)
      ElseIf Left(ubuf, 5) = "NULL=" Then
        txtQuoteChar(1).Text = Mid(ubuf, 6)
        
      'Data Mapping
      ElseIf Left(ubuf, 5) = "VIEW=" Then
        With agdDataMapping
          If Not clearedDataMapping Then 'allows us to save DDF without mapping and load them without disturbing mapping
            .ClearData
            .rows = 1
            clearedDataMapping = True
            r = 0
          End If
          view = Mid(buf, 6)
          While Not EOF(infile) And Left(ubuf, 3) <> "END"
            Line Input #infile, buf
            ubuf = UCase(buf)
            If Left(buf, 1) <> "#" And Left(ubuf, 3) <> "END" Then
              eq = InStr(buf, "=")
              If eq > 0 Then
                r = r + 1
                field = Left(buf, eq - 1)
                .TextMatrix(r, dmDBView) = view
                .TextMatrix(r, dmDBFieldCol) = field
                buf = Mid(buf, eq + 1)
                While Len(buf) > 0
                  Select Case UCase(Left(buf, 1))
                    Case "(": ' (Columns)
                      
                    Case Chr(34): ' "constant"
                    Case "L": ' LOOKUP
                    Case "C": ' CONSTRAINT
                  End Select
                Wend
              End If
            End If
          Wend
        End With
      End If
    End If
  Wend
  If fraColSample.Visible Then PopulateGridSample
End Sub

Private Sub WriteDDF(outfile%)
  Dim r&, view$, field$, tmp$
  If outfile < 0 Then
    MsgBox "Cannot write to output file.", vbOKOnly, "Data Descriptor File"
  Else
    Print #outfile, "#Data Descriptor File"
    Print #outfile, "#"
    Print #outfile, "#File Properties"
    Print #outfile, "FILETYPE=" & ComboFileType.Text
    If delimQ Then
      Select Case delim
        Case "":      'no delimiter
        Case " ":     Print #outfile, "DELIMITER=SPACE"
        Case Chr$(9): Print #outfile, "DELIMITER=TAB"
        Case Else:    Print #outfile, "DELIMITER=" & delim
      End Select
    End If
    Print #outfile, "SKIPLINES=" & txtSkip
    If chkHeader.value = 1 Then Print #outfile, "HEADER=Y" Else Print #outfile, "HEADER=N"
    If txtQuoteChar(0).Text <> "none" Then Print #outfile, "QUOTE=" & txtQuoteChar(0).Text
    If txtQuoteChar(1).Text <> "none" Then Print #outfile, "NULL=" & txtQuoteChar(1).Text
    
    If agdDataMapping.MaxOccupiedRow > 0 Then
      Print #outfile, "#Data Mapping"
      With agdDataMapping
        view = ""
        For r = 1 To .MaxOccupiedRow
          If view <> .TextMatrix(r, dmDBView) Then
            If Len(view) > 0 Then Print #outfile, "END"
            view = .TextMatrix(r, dmDBView)
            Print #outfile, "#"
            Print #outfile, "VIEW=" & view
          End If
          field = .TextMatrix(r, dmDBFieldCol)
          Print #outfile, field & "=";
          tmp = .TextMatrix(r, dmInputCol)
          If Len(tmp) > 0 Then Print #outfile, "(" & tmp & ")"; '(columns)
          tmp = .TextMatrix(r, dmConstantCol)
          If Len(tmp) > 0 Then Print #outfile, Chr(34) & tmp & Chr(34); ' "Constant"
          tmp = .TextMatrix(r, dmLookupCol)
          If Len(tmp) > 0 Then Print #outfile, "LOOKUP=" & Chr(34) & tmp & Chr(34);
          'tmp = .TextMatrix(r, dmConstraintCol)
          'If Len(tmp) > 0 Then Print #outfile, "CONSTRAINT=" & Chr(34) & tmp & Chr(34);
          Print #outfile,
        Next r
        Print #outfile, "END"
      End With
    End If
  End If
End Sub
