VERSION 5.00
Begin VB.Form frmAddDataSet 
   Caption         =   "Add Data Set"
   ClientHeight    =   3192
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   4728
   HelpContextID   =   29
   LinkTopic       =   "Form1"
   ScaleHeight     =   3192
   ScaleWidth      =   4728
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdAdd 
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
      Height          =   375
      Index           =   1
      Left            =   2520
      TabIndex        =   2
      Top             =   2760
      Width           =   1215
   End
   Begin VB.CommandButton cmdAdd 
      Caption         =   "&OK"
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
      Index           =   0
      Left            =   960
      TabIndex        =   1
      Top             =   2760
      Width           =   1215
   End
   Begin ATCoCtl.ATCoGrid agdAdd 
      Height          =   2535
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4455
      _ExtentX        =   7853
      _ExtentY        =   4466
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
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
Attribute VB_Name = "frmAddDataSet"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim myUci As HspfUci

Private Sub cmdAdd_Click(Index As Integer)
  
  Dim wdmid&, dsn&, scen$, cons$, locn$, Tu&, ts&
  Dim addeddsn As Boolean, i&
  
  'let module know whether ok or cancel
  Call UpdateRespFromAddDataSet(Index)
  If Index = 0 Then
    'go ahead and add these data sets
    For i = 1 To agdAdd.rows
      wdmid = WDMInd(agdAdd.TextMatrix(i, 0))
      dsn = agdAdd.TextMatrix(i, 1)
      scen = agdAdd.TextMatrix(i, 2)
      locn = agdAdd.TextMatrix(i, 3)
      cons = agdAdd.TextMatrix(i, 4)
      Tu = agdAdd.TextMatrix(i, 5)
      ts = agdAdd.TextMatrix(i, 6)
      addeddsn = myUci.AddWDMDataSet(wdmid, dsn, scen, locn, cons, Tu, ts)
    Next i
  End If
  Unload Me
End Sub

Private Sub Form_Load()
  Dim n&, adsn&, wid$, s$, l$, c$, i&
  
  Call GetNonExistentDataSetCount(n)
  
  With agdAdd
    .cols = 7
    .FixedCols = 2
    .TextMatrix(0, 0) = "WDM ID"
    .TextMatrix(0, 1) = "DSN"
    .TextMatrix(0, 2) = "Scenario"
    .ColType(2) = ATCoTxt
    .ColEditable(2) = True
    .TextMatrix(0, 3) = "Location"
    .ColType(3) = ATCoTxt
    .ColEditable(3) = True
    .TextMatrix(0, 4) = "Constituent"
    .ColType(4) = ATCoTxt
    .ColEditable(4) = True
    .TextMatrix(0, 5) = "Time Units"
    .ColType(5) = ATCoInt
    .ColMin(5) = 1
    .ColMax(5) = 6
    .ColEditable(5) = True
    .TextMatrix(0, 6) = "Time Step"
    .ColType(6) = ATCoInt
    .ColEditable(6) = True
    .ColMin(6) = 1
    .ColMax(6) = -999
    .rows = n
    For i = 1 To n
      Call GetNonExistentDataSetInfo(i, adsn, wid, s, l, c)
      .TextMatrix(i, 0) = wid
      .TextMatrix(i, 1) = adsn
      .TextMatrix(i, 2) = s
      .TextMatrix(i, 3) = l
      .TextMatrix(i, 4) = c
      .TextMatrix(i, 5) = 4
      .TextMatrix(i, 6) = 1
    Next i
  End With
End Sub

Public Sub SetUCI(u As HspfUci)
  Set myUci = u
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 4200 Then Width = 4200
    If Height < 1500 Then Height = 1500
    cmdAdd(0).Top = Height - cmdAdd(0).Height - 440
    cmdAdd(1).Top = Height - cmdAdd(1).Height - 440
    cmdAdd(0).Left = Width / 2 - cmdAdd(0).Width - 100
    cmdAdd(1).Left = Width / 2 + 100
    agdAdd.Height = Height - cmdAdd(0).Height - 660
    agdAdd.Width = Width - 440
  End If
End Sub

