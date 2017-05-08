VERSION 5.00
Begin VB.Form frmXSect 
   Caption         =   "Import From Cross-Section"
   ClientHeight    =   6405
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5535
   HelpContextID   =   35
   LinkTopic       =   "Form1"
   ScaleHeight     =   6405
   ScaleWidth      =   5535
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraXFile 
      Caption         =   "Cross-Section Files"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   855
      Left            =   240
      TabIndex        =   4
      Top             =   120
      Width           =   5055
      Begin VB.ComboBox cboXFile 
         Height          =   315
         Left            =   1680
         Style           =   2  'Dropdown List
         TabIndex        =   7
         Top             =   360
         Width           =   1695
      End
      Begin VB.CommandButton cmdSave 
         Caption         =   "&Save"
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
         TabIndex        =   6
         Top             =   360
         Width           =   1215
      End
      Begin VB.CommandButton cmdOpen 
         Caption         =   "&Open"
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
         Left            =   240
         TabIndex        =   5
         Top             =   360
         Width           =   1215
      End
      Begin MSComDlg.CommonDialog CDFile 
         Left            =   0
         Top             =   480
         _ExtentX        =   688
         _ExtentY        =   688
         _Version        =   393216
         FontSize        =   4.09255e-38
      End
   End
   Begin VB.CommandButton cmdXSect 
      Caption         =   "&Help"
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
      HelpContextID   =   22
      Index           =   4
      Left            =   3720
      TabIndex        =   3
      Top             =   5880
      Width           =   1215
   End
   Begin VB.CommandButton cmdXSect 
      Caption         =   "&Cancel"
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
      Index           =   1
      Left            =   2160
      TabIndex        =   2
      Top             =   5880
      Width           =   1215
   End
   Begin VB.CommandButton cmdXSect 
      Caption         =   "&OK"
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
      Index           =   0
      Left            =   600
      TabIndex        =   1
      Top             =   5880
      Width           =   1215
   End
   Begin ATCoCtl.ATCoGrid agdXSect 
      Height          =   4455
      Left            =   240
      TabIndex        =   0
      Top             =   1200
      Width           =   5055
      _ExtentX        =   8916
      _ExtentY        =   7858
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
      Cols            =   3
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "RCHRES"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483637
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
End
Attribute VB_Name = "frmXSect"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim currch&
Dim curftab As HspfFtable

Private Sub cboXFile_Click()
  Dim ArrayVals!(16)
  Call GetPTFData(cboXFile.Text, ArrayVals)
  With agdXSect
    .TextMatrix(1, 2) = ArrayVals(1)
    .TextMatrix(2, 2) = ArrayVals(2)
    .TextMatrix(3, 2) = ArrayVals(3)
    .TextMatrix(4, 2) = ArrayVals(4)
    .TextMatrix(5, 2) = ArrayVals(5)
    .TextMatrix(6, 2) = ArrayVals(6)
    .TextMatrix(7, 2) = ArrayVals(7)
    .TextMatrix(8, 2) = ArrayVals(8)
    .TextMatrix(9, 2) = ArrayVals(9)
    .TextMatrix(10, 2) = ArrayVals(10)
    .TextMatrix(11, 2) = ArrayVals(11)
    .TextMatrix(12, 2) = ArrayVals(12)
    .TextMatrix(13, 2) = ArrayVals(13)
    .TextMatrix(14, 2) = ArrayVals(14)
    .TextMatrix(15, 2) = ArrayVals(15)
    .TextMatrix(16, 2) = ArrayVals(16)
  End With
End Sub

Private Sub cmdOpen_Click()
  Dim ret&, ArrayIds$(), cnt&, i&
  
  ChDriveDir "\basins\modelout"
  CDFile.flags = &H8806&
  CDFile.Filter = "BASINS Trapezoidal Files (*.ptf)"
  CDFile.FileName = "*.ptf"
  CDFile.DialogTitle = "Select BASINS Trapezoidal File"
  On Error GoTo 50
  CDFile.CancelError = True
  CDFile.Action = 1
  'read file here
  Call ReadPTFFile(CDFile.FileName, ret)
  If ret = 0 Then
    Call GetPTFFileIds(cnt, ArrayIds)
    cboXFile.Clear
    For i = 1 To cnt
      cboXFile.AddItem ArrayIds(i)
    Next i
    cboXFile.ListIndex = 0
  End If
50        'continue here on cancel
End Sub

Private Sub cmdSave_Click()
  Dim ArrayVals!(16)
  
  CDFile.flags = &H8806&
  CDFile.Filter = "BASINS Trapezoidal Files (*.ptf)|*.ptf"
  CDFile.FileName = "*.ptf"
  CDFile.DialogTitle = "Save Cross Section Specifications"
  On Error GoTo 10
    CDFile.CancelError = True
    CDFile.Action = 2
    With agdXSect
      ArrayVals(1) = .TextMatrix(1, 2)
      ArrayVals(2) = .TextMatrix(2, 2)
      ArrayVals(3) = .TextMatrix(3, 2)
      ArrayVals(4) = .TextMatrix(4, 2)
      ArrayVals(5) = .TextMatrix(5, 2)
      ArrayVals(6) = .TextMatrix(6, 2)
      ArrayVals(7) = .TextMatrix(7, 2)
      ArrayVals(8) = .TextMatrix(8, 2)
      ArrayVals(9) = .TextMatrix(9, 2)
      ArrayVals(10) = .TextMatrix(10, 2)
      ArrayVals(11) = .TextMatrix(11, 2)
      ArrayVals(12) = .TextMatrix(12, 2)
      ArrayVals(13) = .TextMatrix(13, 2)
      ArrayVals(14) = .TextMatrix(14, 2)
      ArrayVals(15) = .TextMatrix(15, 2)
      ArrayVals(16) = .TextMatrix(16, 2)
    End With
    Call WritePTFFile(CDFile.FileName, 1, ArrayVals)
10   'continue here on cancel
End Sub

Private Sub cmdXSect_Click(Index As Integer)
  Dim l!, ym!, wm!, n!, s!, m32!, m22!, w12!
  Dim m12!, m11!, w11!, m21!, m31!, yc!, yt1!, yt2!
  
  If Index = 0 Then
    'okay
    With agdXSect
      l = .TextMatrix(1, 2)
      ym = .TextMatrix(2, 2)
      wm = .TextMatrix(3, 2)
      n = .TextMatrix(4, 2)
      s = .TextMatrix(5, 2)
      m32 = .TextMatrix(6, 2)
      m22 = .TextMatrix(7, 2)
      w12 = .TextMatrix(8, 2)
      m12 = .TextMatrix(9, 2)
      m11 = .TextMatrix(10, 2)
      w11 = .TextMatrix(11, 2)
      m21 = .TextMatrix(12, 2)
      m31 = .TextMatrix(13, 2)
      yc = .TextMatrix(14, 2)
      yt1 = .TextMatrix(15, 2)
      yt2 = .TextMatrix(16, 2)
      curftab.FTableFromCrossSect l, ym, wm, n, s, m11, m12, yc, m21, _
                     m22, yt1, yt2, m31, m32, w11, w12
    End With
    Unload Me
  ElseIf Index = 1 Then
    'cancel
    Unload Me
  Else
    Dim d As HH_AKLINK, h$
    d.pszKeywords = "Reach Editor"
    d.fReserved = vbFalse
    d.cbStruct = LenB(d)
    HtmlHelp Me.hwnd, App.HelpFile, HH_KEYWORD_LOOKUP, d
  End If
End Sub

Private Sub Form_Load()
  Dim i&
  With agdXSect
    .cols = 3
    .FixedCols = 2
    .TextMatrix(0, 0) = "Variable"
    .TextMatrix(0, 1) = "Description"
    .TextMatrix(0, 2) = "Value"
    .TextMatrix(1, 0) = "L"
    .TextMatrix(2, 0) = "Ym"
    .TextMatrix(3, 0) = "Wm"
    .TextMatrix(4, 0) = "n"
    .TextMatrix(5, 0) = "S"
    .TextMatrix(6, 0) = "m32"
    .TextMatrix(7, 0) = "m22"
    .TextMatrix(8, 0) = "W12"
    .TextMatrix(9, 0) = "m12"
    .TextMatrix(10, 0) = "m11"
    .TextMatrix(11, 0) = "W11"
    .TextMatrix(12, 0) = "m21"
    .TextMatrix(13, 0) = "m31"
    .TextMatrix(14, 0) = "Yc"
    .TextMatrix(15, 0) = "Yt1"
    .TextMatrix(16, 0) = "Yt2"
    For i = 1 To 16
      .TextMatrix(i, 2) = 0.01
      .ColType(2) = ATCoSng
      .ColMin(2) = 0.00001
    Next i
    .TextMatrix(1, 1) = "Length (ft)"
    .TextMatrix(2, 1) = "Mean Depth (ft)"
    .TextMatrix(3, 1) = "Mean Width (ft)"
    .TextMatrix(4, 1) = "Mannings Roughness Coefficient"
    .TextMatrix(5, 1) = "Longitudinal Slope"
    .TextMatrix(6, 1) = "Side Slope of Upper Flood Plain Left"
    .TextMatrix(7, 1) = "Side Slope of Lower Flood Plain Left"
    .TextMatrix(8, 1) = "Zero Slope Flood Plain Width Left (ft)"
    .TextMatrix(9, 1) = "Side Slope of Channel Left"
    .TextMatrix(10, 1) = "Side Slope of Channel Right"
    .TextMatrix(11, 1) = "Zero Slope Flood Plain Width Right (ft)"
    .TextMatrix(12, 1) = "Side Slope Lower Flood Plain Right"
    .TextMatrix(13, 1) = "Side Slope Upper Flood Plain Right"
    .TextMatrix(14, 1) = "Channel Depth (ft)"
    .TextMatrix(15, 1) = "Flood Side Slope Change at Depth (ft)"
    .TextMatrix(16, 1) = "Maximum Depth (ft)"
    .ColEditable(2) = True
    .ColsSizeByContents
  End With
End Sub

Public Sub CurrentReach(r&, ftab As HspfFtable)
  currch = r
  Set curftab = ftab
  agdXSect.Header = "FTABLE " & CStr(currch)
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 1500 Then Width = 1500
    If Height < 1500 Then Height = 1500
    agdXSect.Width = Width - 500
    fraXFile.Width = Width - 500
    cmdOpen.Left = 200
    cmdSave.Left = fraXFile.Width - cmdSave.Width - 200
    cboXFile.Left = (fraXFile.Width - cboXFile.Width) / 2
    cmdXSect(0).Left = agdXSect.Left + (agdXSect.Width / 2) - (1.5 * cmdXSect(0).Width) - 400
    cmdXSect(4).Left = agdXSect.Left + (agdXSect.Width / 2) + (0.5 * cmdXSect(0).Width) + 400
    cmdXSect(1).Left = agdXSect.Left + (agdXSect.Width / 2) - (0.5 * cmdXSect(1).Width)
    agdXSect.Height = Height - (4 * cmdXSect(1).Height) + 50
    cmdXSect(1).Top = Height - agdXSect.Top - cmdXSect(1).Height - 200 + fraXFile.Height
    cmdXSect(0).Top = Height - agdXSect.Top - cmdXSect(0).Height - 200 + fraXFile.Height
    cmdXSect(4).Top = Height - agdXSect.Top - cmdXSect(4).Height - 200 + fraXFile.Height
  End If
End Sub
