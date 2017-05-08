VERSION 5.00
Begin VB.Form frmTSsave 
   Caption         =   "Save Timeseries"
   ClientHeight    =   2268
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   7272
   HelpContextID   =   960
   Icon            =   "frmTSsave.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2268
   ScaleWidth      =   7272
   StartUpPosition =   3  'Windows Default
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   1092
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   6972
      _ExtentX        =   12298
      _ExtentY        =   1926
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   5
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
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2520
      TabIndex        =   5
      Top             =   1800
      Width           =   2292
      Begin VB.CommandButton cmdOk 
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
         Height          =   372
         Left            =   0
         TabIndex        =   2
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
         Left            =   1200
         TabIndex        =   3
         Top             =   0
         Width           =   1092
      End
   End
   Begin VB.ComboBox cboFile 
      Height          =   288
      Left            =   960
      TabIndex        =   0
      Top             =   120
      Width           =   6132
   End
   Begin VB.Label lblFile 
      Caption         =   "Save in"
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
      TabIndex        =   4
      Top             =   180
      Width           =   732
   End
End
Attribute VB_Name = "frmTSsave"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private pTsers As Collection
Private pOpenFiles As Collection

Private Const IDcol = 0
Private Const ScenCol = 1
Private Const LocCol = 2
Private Const ConsCol = 3
Private Const DescCol = 4

Public Property Get Tsers() As Collection
  Set Tsers = pTsers
End Property
Public Property Set Tsers(newvalue As Collection)
  Set pTsers = Nothing
  Set pTsers = newvalue
  PopulateGrid
End Property

Public Property Get OpenFiles() As Collection
  Set OpenFiles = pOpenFiles
End Property
Public Property Set OpenFiles(newvalue As Collection)
  Dim vTSF As Variant, tsf As ATCclsTserFile
  Set pOpenFiles = newvalue
  If Not pOpenFiles Is Nothing Then
    cboFile.clear
    For Each vTSF In pOpenFiles
      Set tsf = vTSF
      cboFile.AddItem tsf.Filename
    Next
    'cboFile.AddItem "Other file..."
    'If Not pTser Is Nothing Then
    '  If Not pTser.File Is Nothing Then
    '    cboFile.Text = pTser.File.Filename
    '  End If
    'End If
  End If
End Property

'Public Property Get Tser() As ATCclsTserData
'  Set Tser = pTser
'End Property
Public Property Set Tser(newvalue As ATCclsTserData)
  Set pTsers = Nothing
  Set pTsers = New Collection
  If Not newvalue Is Nothing Then pTsers.Add newvalue
  PopulateGrid
End Property

Private Sub PopulateGrid()
  Dim OneTser As ATCclsTserData
  Dim row As Long
  agd.clear
  agd.cols = 5
  agd.Rows = pTsers.Count
  
  agd.ColEditable(IDcol) = True
  agd.ColEditable(ScenCol) = True
  agd.ColEditable(LocCol) = True
  agd.ColEditable(ConsCol) = True
  agd.ColEditable(DescCol) = True
  
  agd.TextMatrix(0, IDcol) = "ID"
  agd.TextMatrix(0, ScenCol) = "Scenario"
  agd.TextMatrix(0, LocCol) = "Location"
  agd.TextMatrix(0, ConsCol) = "Constituent"
  agd.TextMatrix(0, DescCol) = "Description"
  
  For row = 1 To pTsers.Count
    Set OneTser = pTsers(row)
    With OneTser.Header
      agd.TextMatrix(row, IDcol) = .id
      agd.TextMatrix(row, ScenCol) = .sen
      agd.TextMatrix(row, LocCol) = .Loc
      agd.TextMatrix(row, ConsCol) = .con
      agd.TextMatrix(row, DescCol) = .Desc
    End With
    If Not OneTser.File Is Nothing Then
      If OneTser.File.Label <> "<in memory>" Then cboFile.Text = OneTser.File.Filename
    End If
  Next
  agd.ColsSizeByContents
  agd.ColsSizeToWidth
End Sub

Private Sub cmdCancel_Click()
  Me.Hide
End Sub

Private Sub cmdOk_Click()
  Dim tsf As ATCclsTserFile, tsv As Variant
  Dim OneTser As ATCclsTserData
  Dim Filename As String
  Dim row As Long
  
  Filename = cboFile.Text
  If Filename = "Other file..." Then
    MsgBox "Saving to another file is not yet supported."
    Exit Sub
  End If
  
  For Each tsv In pOpenFiles
    Set tsf = tsv
    If tsf.Filename = Filename Then GoTo FoundFile
  Next
  
  MsgBox "Could not find file '" & Filename & "'", vbExclamation, "Save Timeseries"
  Exit Sub
  
FoundFile:
    
  For row = 1 To pTsers.Count
    Set OneTser = pTsers(row)
    With OneTser.Header
      .id = agd.TextMatrix(row, IDcol)
      .sen = agd.TextMatrix(row, ScenCol)
      .Loc = agd.TextMatrix(row, LocCol)
      .con = agd.TextMatrix(row, ConsCol)
      .Desc = agd.TextMatrix(row, DescCol)
    End With
    If tsf.addtimser(OneTser, TsIdReplAsk + TsIdAppendAsk + TsIdRenumAsk) Then 'Add successful
      'confimation messagebox is opened in addtimser, at least in WDM it is.
    Else
      MsgBox "Failed to save timeseries." & vbCr & tsf.ErrorDescription, vbExclamation, "Save Timeseries"
    End If
  Next
  Me.Hide
End Sub

Private Sub Form_Load()
  If pTsers Is Nothing Then Set pTsers = New Collection
  If OpenFiles Is Nothing Then Set OpenFiles = New Collection
End Sub

Private Sub Form_Resize()
  Dim w As Single, h As Single
  w = Me.ScaleWidth
  h = Me.ScaleHeight
  If w > 2000 And h > 2000 Then
    agd.Width = w - 336
    agd.Height = h - 1176
    cboFile.Width = w - 1080
    fraButtons.Top = h - 468
    fraButtons.Left = (w - fraButtons.Width) / 2
  End If
End Sub
