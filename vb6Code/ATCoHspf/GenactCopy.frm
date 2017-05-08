VERSION 5.00
Begin VB.Form frmGenScnActivate 
   Caption         =   "GenScn Activate"
   ClientHeight    =   6900
   ClientLeft      =   270
   ClientTop       =   1335
   ClientWidth     =   8280
   HelpContextID   =   53
   Icon            =   "GenactCopy.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   6900
   ScaleWidth      =   8280
   WhatsThisButton =   -1  'True
   WhatsThisHelp   =   -1  'True
   Begin VB.ListBox lstFiles 
      Height          =   645
      Left            =   1200
      TabIndex        =   28
      Top             =   1920
      Width           =   1695
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Delete"
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
      Index           =   4
      Left            =   5640
      TabIndex        =   18
      Top             =   3480
      Visible         =   0   'False
      Width           =   732
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Insert"
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
      Index           =   3
      Left            =   4800
      TabIndex        =   17
      Top             =   3480
      Visible         =   0   'False
      Width           =   735
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Dbg"
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
      Index           =   6
      Left            =   7560
      TabIndex        =   20
      Top             =   3480
      Visible         =   0   'False
      Width           =   492
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Text/Grid"
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
      Index           =   5
      Left            =   6480
      TabIndex        =   19
      Top             =   3480
      Visible         =   0   'False
      Width           =   972
   End
   Begin VB.TextBox txtPath 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   1080
      TabIndex        =   0
      Text            =   "????"
      Top             =   120
      WhatsThisHelpID =   24
      Width           =   6972
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&Modify"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   5
      Left            =   1680
      TabIndex        =   3
      Top             =   960
      WhatsThisHelpID =   34
      Width           =   1212
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&View"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   4
      Left            =   240
      TabIndex        =   6
      Top             =   2040
      WhatsThisHelpID =   33
      Width           =   855
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "Help"
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
      Index           =   2
      Left            =   3960
      TabIndex        =   16
      Top             =   3480
      Visible         =   0   'False
      Width           =   732
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "&Close"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   3
      Left            =   960
      TabIndex        =   7
      Top             =   2760
      WhatsThisHelpID =   32
      Width           =   1212
   End
   Begin VB.CommandButton cmdTable 
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
      Height          =   252
      Index           =   1
      Left            =   3000
      TabIndex        =   15
      Top             =   3480
      Visible         =   0   'False
      Width           =   855
   End
   Begin VB.CommandButton cmdTable 
      Caption         =   "OK"
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
      Index           =   0
      Left            =   2280
      TabIndex        =   14
      Top             =   3480
      Visible         =   0   'False
      Width           =   612
   End
   Begin VB.Frame fraTname 
      Caption         =   "Table"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   5760
      TabIndex        =   25
      Top             =   840
      Visible         =   0   'False
      Width           =   2292
      Begin VB.OptionButton optOpnTname 
         Caption         =   "Active"
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
         Left            =   1080
         TabIndex        =   13
         Top             =   1920
         Value           =   -1  'True
         WhatsThisHelpID =   40
         Width           =   975
      End
      Begin VB.OptionButton optOpnTname 
         Caption         =   "All"
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
         TabIndex        =   12
         Top             =   1920
         WhatsThisHelpID =   40
         Width           =   735
      End
      Begin VB.ListBox lstTname 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1425
         Left            =   120
         TabIndex        =   11
         Top             =   360
         WhatsThisHelpID =   39
         Width           =   2052
      End
   End
   Begin VB.Frame Frame1 
      Caption         =   "Block"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2295
      Left            =   3240
      TabIndex        =   24
      Top             =   840
      Width           =   2292
      Begin VB.OptionButton optOpnKwd 
         Caption         =   "Active"
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
         Left            =   1080
         TabIndex        =   10
         Top             =   1920
         Value           =   -1  'True
         WhatsThisHelpID =   38
         Width           =   975
      End
      Begin VB.OptionButton optOpnKwd 
         Caption         =   "All"
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
         Index           =   0
         Left            =   240
         TabIndex        =   9
         Top             =   1920
         WhatsThisHelpID =   38
         Width           =   615
      End
      Begin VB.ListBox lstKwd 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   1425
         ItemData        =   "GenactCopy.frx":0442
         Left            =   120
         List            =   "GenactCopy.frx":0444
         TabIndex        =   8
         Top             =   360
         WhatsThisHelpID =   37
         Width           =   2055
      End
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "Save &As"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   2
      Left            =   1680
      TabIndex        =   5
      Top             =   1440
      WhatsThisHelpID =   35
      Width           =   1212
   End
   Begin VB.TextBox txtInfo 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   288
      Left            =   1080
      TabIndex        =   1
      Text            =   "????"
      Top             =   480
      WhatsThisHelpID =   24
      Width           =   6972
   End
   Begin VB.CommandButton cmdOpt 
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
      Height          =   372
      Index           =   1
      Left            =   240
      TabIndex        =   4
      Top             =   1440
      WhatsThisHelpID =   34
      Width           =   1212
   End
   Begin VB.CommandButton cmdOpt 
      Caption         =   "Simulate(&R)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   0
      Left            =   240
      TabIndex        =   2
      Top             =   960
      WhatsThisHelpID =   31
      Width           =   1212
   End
   Begin VB.TextBox txtRec 
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   7.5
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1455
      Left            =   240
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   22
      Text            =   "GenactCopy.frx":0446
      Top             =   3840
      Visible         =   0   'False
      Width           =   7800
   End
   Begin ATCoCtl.ATCoGrid gridctrl1 
      Height          =   1335
      Left            =   120
      TabIndex        =   21
      Top             =   3840
      Width           =   7935
      _ExtentX        =   13996
      _ExtentY        =   2355
      SelectionToggle =   -1  'True
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
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
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   -1  'True
   End
   Begin VB.Label lblPath 
      Alignment       =   1  'Right Justify
      Caption         =   "Path:"
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
      TabIndex        =   27
      Top             =   120
      WhatsThisHelpID =   24
      Width           =   852
   End
   Begin VB.Label lblBlock 
      Caption         =   "????"
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
      TabIndex        =   26
      Top             =   3480
      Visible         =   0   'False
      Width           =   1935
   End
   Begin VB.Label lblScen 
      Alignment       =   1  'Right Justify
      Caption         =   "Run Info:"
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
      TabIndex        =   23
      Top             =   480
      WhatsThisHelpID =   24
      Width           =   852
   End
End
Attribute VB_Name = "frmGenScnActivate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Private Type HSPFOper
   Name As String
   Exists As Boolean
   OpF As Long
   OpL As Long
   cnt As Long
End Type
Dim Oper(11) As HSPFOper

Dim s&(5), e&(5), ou&, sp&, ru&, em&, inf$
Dim i&, k$
Dim akey&(), nkey&
Dim oldsyear$, oldsmonth$, oldsday$, oldshour$, oldsminute$
Dim oldeyear$, oldemonth$, oldeday$, oldehour$, oldeminute$

Dim ousav&, spsav&, rusav&, emsav&
Dim sysav$, eysav$, smosav$, emosav$, sdsav$, edsav$
Dim shsav$, ehsav$, smisav$, emisav$

'Retrieved in a call to F90_XTINFO to get column info for grid
Dim lnflds&, lscol&(30), lflen&(30), lftyp$, lapos&(30)
Dim limin&(30), limax&(30), lidef&(30)
Dim lrmin!(30), lrmax!(30), lrdef!(30)
Dim lnmhdr&, hdrbuf$(5), lfdnam$(30)

Public newname$, RelAbs&, BaseDSN&

Public Function OperationExists(Name$) As Boolean

   OperationExists = False
   For i = 0 To lstKwd.ListCount
     If lstKwd.List(i) = Name Then
       OperationExists = True
       Exit For
     End If
   Next i
   
End Function

Private Sub Edit_Global()
  txtRec.Visible = False
  lblBlock.Visible = True
  gridctrl1.Visible = False
  cmdTable(0).Visible = True
  cmdTable(1).Visible = True
  cmdTable(2).Visible = True
  picGlobal.Visible = True
  lblBlock.Caption = lstKwd.List(lstKwd.ListIndex)
  frmGenScnActivate.Height = picGlobal.Height + picGlobal.Top + 250
  For i = 0 To 5
    cmdOpt(i).Enabled = False
  Next i
  lstTname.Enabled = False
  lstKwd.Enabled = False
  optOpnKwd(0).Enabled = False
  optOpnKwd(1).Enabled = False
  optOpnTname(0).Enabled = False
  optOpnTname(1).Enabled = False
  'save values we came in with
  ousav = comboOutput.ListIndex
  spsav = comboSpecial.ListIndex
  rusav = comboRunflag.ListIndex
  emsav = comboUnits.ListIndex
  sysav = txtStart.Text
  eysav = txtEnd.Text
  smosav = txtSmon.Text
  emosav = txtEmon.Text
  sdsav = txtSday.Text
  edsav = txtEday.Text
  shsav = txtShour.Text
  ehsav = txtEhour.Text
  smisav = txtSmin.Text
  emisav = txtEmin.Text
End Sub
Private Sub Edit_ExtSources(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Dim s$, dsn&
  Static TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 8))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 11), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then
    If g.Col = 1 Then
      s = g.Text
      If IsNumeric(s) Then
        dsn = CLng(s)
        'Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(1), CLng(4), s)
        If Len(s) > 0 Then
          g.TextBackColor = vbWhite
        Else
          g.TextBackColor = vbRed
        End If
      Else
        g.TextBackColor = vbRed
        s = ""
      End If
      g.TextMatrix(g.row, 2) = s
    ElseIf g.Col = 8 Then
      TarOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 11 Then
      TarGrpInd = g.ListIndex + 1
    End If
  ElseIf etype = 2 Then
    g.ClearValues
    If g.Col = 0 Then 'svol:wdm and what else?
      g.addValue "WDM"
    ElseIf g.Col = 1 Then 'svolno
    ElseIf g.Col = 2 Then 'smemn
      dsn = g.TextMatrix(g.row, 1)
      'Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(1), CLng(4), s)
      g.addValue s
    ElseIf g.Col = 3 Then 'qflg
    ElseIf g.Col = 4 Then 'ssyst
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 5 Then 'sgapst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 6 Then 'mfactr
    ElseIf g.Col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 8 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.Col = 9 Then 'topfst
      g.SetColRange g.Col, (Oper(TarOperTyp).OpF), (Oper(TarOperTyp).OpL)
    ElseIf g.Col = 10 Then 'toplst
      g.SetColRange g.Col, (Oper(TarOperTyp).OpF), (Oper(TarOperTyp).OpL)
    ElseIf g.Col = 11 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.Col = 12 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 13 Then 'tmems1
    ElseIf g.Col = 14 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_ExtTargets(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Dim s$, dsn&
  Static SouGrpInd&, SouOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 2), SouOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.Col = 0 Then 'svol
      SouOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 2 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.Col = 9 Then
      s = g.Text
      If IsNumeric(s) Then
        dsn = CLng(s)
        'Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(1), CLng(4), s)
        If Len(s) > 0 Then
          g.TextBackColor = vbWhite
        Else
          g.TextBackColor = vbRed
        End If
      Else
        g.TextBackColor = vbRed
        s = ""
      End If
      g.TextMatrix(g.row, 10) = s
    End If
  Else 'RowCol change
    g.ClearValues
    If g.Col = 0 Then 'svol
      g.SetColRange g.Col, (Oper(SouOperTyp).OpF), (Oper(SouOperTyp).OpL)
    ElseIf g.Col = 2 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.Col = 3 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 4 Then 'smems1
    ElseIf g.Col = 5 Then 'smems2
    ElseIf g.Col = 6 Then 'mfactr
    ElseIf g.Col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 8 Then 'tvol:wdm and what else
      g.addValue "WDM"
    ElseIf g.Col = 9 Then 'tvolno
    ElseIf g.Col = 10 Then 'tmemn
      dsn = g.TextMatrix(g.row, 9)
      'Call F90_WDBSGC(p.WDMFiles(1).FileUnit, dsn, CLng(1), CLng(4), s)
      g.addValue s
    ElseIf g.Col = 11 Then 'qflg
    ElseIf g.Col = 12 Then 'tsyst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 13 Then 'aggst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 14 Then 'amdst
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    End If
  End If
End Sub
Private Sub Edit_Network(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouGrpInd&, SouOperTyp&, TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 2), SouOperTyp)
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 8))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 11), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.Col = 0 Then 'svol
      SouOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 2 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.Col = 8 Then
      TarOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 11 Then
      TarGrpInd = g.ListIndex + 1
    End If
  Else 'RowCol change
    g.ClearValues
    If g.Col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.Col = 1 Then 'svolno
      g.SetColRange g.Col, (Oper(SouOperTyp).OpF), (Oper(SouOperTyp).OpL)
    ElseIf g.Col = 2 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.Col = 3 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 4 Then 'smems1
    ElseIf g.Col = 5 Then 'smems2
    ElseIf g.Col = 6 Then 'mfactr
    ElseIf g.Col = 7 Then 'tran
      g.addValue " " 'allow default blank
      Call SetTableKwds(g, -(g.Col + 1))
    ElseIf g.Col = 8 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.Col = 9 Then 'topfst
      g.SetColRange g.Col, (Oper(TarOperTyp).OpF), (Oper(TarOperTyp).OpL)
    ElseIf g.Col = 10 Then 'toplst
      g.SetColRange g.Col, (Oper(TarOperTyp).OpF), (Oper(TarOperTyp).OpL)
    ElseIf g.Col = 11 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.Col = 12 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 13 Then 'tmems1
    ElseIf g.Col = 14 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_MassLink(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouGrpInd&, SouOperTyp&, TarGrpInd&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    SouGrpInd = GetGrpInd(g.TextMatrix(g.row, 1), SouOperTyp)
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 6))
    TarGrpInd = GetGrpInd(g.TextMatrix(g.row, 7), TarOperTyp)
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.Col = 0 Then 'svol
      SouOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 1 Then 'sgrpn
      SouGrpInd = g.ListIndex + 1
    ElseIf g.Col = 6 Then 'tvol
      TarOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 7 Then 'tgrpn
      TarGrpInd = g.ListIndex + 1
    End If
  Else 'RowCol change
    g.ClearValues
    If g.Col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.Col = 1 Then 'sgrpn
      Call SetTableKwds(g, CLng(SouOperTyp + 140))
    ElseIf g.Col = 2 Then 'smemn
      i = (SouOperTyp + 140) * 1000 + SouGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 3 Then 'smems1
    ElseIf g.Col = 4 Then 'smems2
    ElseIf g.Col = 5 Then 'mfactr
    ElseIf g.Col = 6 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.Col = 7 Then 'tmemnm
      Call SetTableKwds(g, CLng(TarOperTyp + 140))
    ElseIf g.Col = 8 Then 'tgrpn
      i = (TarOperTyp + 140) * 1000 + TarGrpInd + 1
      Call SetTableKwds(g, i)
    ElseIf g.Col = 9 Then 'tmems1
    ElseIf g.Col = 10 Then 'tmems2
    End If
  End If
End Sub
Private Sub Edit_Schematic(g As Control, etype%)
  'etype - 1:text change, 2:RowCol change
  Static SouOperTyp&, TarOperTyp&, LastRow&
    
  If g.row <> LastRow Then
    'update info on current state of this row
    SouOperTyp = GetOperType(g.TextMatrix(g.row, 0))
    TarOperTyp = GetOperType(g.TextMatrix(g.row, 3))
    LastRow = g.row
  End If
  
  If etype = 1 Then 'text change
    If g.Col = 0 Then 'svol
      SouOperTyp = GetOperType(g.Text)
    ElseIf g.Col = 3 Then 'tvol
      TarOperTyp = GetOperType(g.Text)
    End If
  Else 'RowCol change
    g.ClearValues
    If g.Col = 0 Then 'svol
      Call SetActiveOpn(g)
    ElseIf g.Col = 1 Then 'svolno
      g.SetColRange g.Col, (Oper(SouOperTyp).OpF), (Oper(SouOperTyp).OpL)
    ElseIf g.Col = 2 Then 'afacter
    ElseIf g.Col = 3 Then 'tvol
      Call SetActiveOpn(g)
    ElseIf g.Col = 4 Then 'tvolno
      g.SetColRange g.Col, (Oper(TarOperTyp).OpF), (Oper(TarOperTyp).OpL)
    ElseIf g.Col = 5 Then 'mslkno
    End If
  End If
End Sub
Private Function GetGrpInd(CurGrp$, OperTyp&) As Long
  Dim init&, kwd$, kflg&, retid&, contfg&, ind&, cnt
  
  ind = OperTyp + 140
  init = 1
  cnt = 1
  GetGrpInd = 0
  Do
    Call F90_GTNXKW(init, ind, kwd, kflg, contfg, retid)
    init = 0
    If kwd = CurGrp Then
      GetGrpInd = cnt
    Else
      cnt = cnt + 1
    End If
  Loop While contfg = 1
  
End Function
Private Sub GetOperInfo(o As HSPFOper)
  'find information about hspf operation type
  Dim cbuff$, omcode&, init&, retcod&, retkey&, i%

  omcode = 3
  init = 1
  Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
  init = 0
  o.cnt = 0
  Do
    Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
    If Mid(cbuff, 7, 6) = o.Name Then
      o.cnt = o.cnt + 1
      i = Mid(cbuff, 18, 3)
      If o.OpF = 0 Or i < o.OpF Then
        o.OpF = i
      End If
      If o.OpL = 0 Or i > o.OpL Then
        o.OpL = i
      End If
    End If
  Loop While retcod = 1 Or retcod = 2
  
End Sub
Private Function GetOperType(s$) As Long
    Dim i%
    
    GetOperType = 0
    i = 1
    Do
      If Oper(i).Name = s Then
        GetOperType = i
        Exit Do
      Else
        i = i + 1
      End If
    Loop While i < UBound(Oper)
End Function

Private Sub SetActiveOpn(g As Object)
  Dim i%, pos%
  
  pos = 0
  Do
    i = lstKwd.ItemData(pos)
    If i > 120 Then
      If Oper(i - 120).Exists Then
        g.addValue Oper(i - 120).Name
      End If
    End If
    pos = pos + 1
  Loop While pos < lstKwd.ListCount - 1
End Sub
Private Sub SetTableKwds(g As Control, ind&)
  Dim init&, kwd$, kflg&, retid&, contfg&
  
  init = 1
  Do
    Call F90_GTNXKW(init, ind, kwd, kflg, contfg, retid)
    g.addValue kwd
    init = 0
  Loop While contfg = 1

End Sub
Private Sub open_table()
  lblBlock.Visible = True
  For i = 0 To 4
    cmdTable(i).Visible = True
  Next i
  txtRec.Visible = True
  'lblBlock.Caption = lstKwd.List(lstKwd.ListIndex)
  On Error Resume Next ' kludge for vb prob?
  frmGenScnActivate.Height = 6252
  'frmGenScnActivate.Height = 6252
  On Error GoTo 0
  'frmGenScnActivate.BorderStyle = 2 'resizable
  For i = 0 To 5
    cmdOpt(i).Enabled = False
  Next i
  lstTname.Enabled = False
  lstKwd.Enabled = False
  optOpnKwd(0).Enabled = False
  optOpnKwd(1).Enabled = False
  optOpnTname(0).Enabled = False
  optOpnTname(1).Enabled = False
End Sub

Private Sub Put_Table_Back()
  Dim ucibf$, r&, c&, k&, replaceRows&
  'Call F90_SCNDBG(-2)
  With gridctrl1
    If nkey < gridctrl1.Rows Then replaceRows = nkey Else replaceRows = gridctrl1.Rows
    For r = 1 To replaceRows
      GoSub buildRow
      If akey(r) > 0 Then
        Call F90_REPUCI(akey(r), ucibf, Len(ucibf))
      Else
        MsgBox "PutTableBack:repl" & akey(r)
      End If
    Next r
    For k = replaceRows + 1 To nkey
      If akey(k) > 0 Then
        Call F90_DELUCI(akey(k))
      Else
        MsgBox "PutTableBack:dele" & akey(k)
      End If
    Next k
    For r = replaceRows + 1 To gridctrl1.Rows
      GoSub buildRow
      If akey(nkey) > 0 Then
        Call F90_PUTUCI(akey(nkey), 0, ucibf, Len(ucibf))
      Else
        MsgBox "PutTableBack:add" & akey(nkey)
      End If
    Next r
  End With
  Exit Sub
  
buildRow:
  Dim txt$, lentxt%, pos%
  ucibf = ""
  pos = 1
  For c = 0 To gridctrl1.Cols - 1
    If pos < lscol(c) Then
      ucibf = ucibf & Space(lscol(c) - pos)
      pos = lscol(c)
    End If
    txt = gridctrl1.TextMatrix(r, c)
    lentxt = Len(txt)
    If lentxt > lflen(c) Then
      ucibf = ucibf & Left(txt, lflen(c))
    Else
      If IsNumeric(txt) Then
        ucibf = ucibf & Space(lflen(c) - lentxt) & txt
      Else
        ucibf = ucibf & txt & Space(lflen(c) - lentxt)
      End If
    End If
    pos = pos + lflen(c)
  Next c
  Return
End Sub

Private Sub Put_TextBox_Back()
  Dim ucibf$, istart&, iend&, ilen&, moretext
  istart = 1
  moretext = True
  For i = 1 To nkey
    If moretext = True Then
      iend = InStr(istart, txtRec.Text, Chr(13) & Chr(10))
      If iend = 0 Then
        'last line of text
        ilen = 80
        If akey(i) > 0 Then
          moretext = False
        End If
      Else
        ilen = iend - istart
      End If
      ucibf = Mid(txtRec.Text, istart, ilen)
      If akey(i) > 0 Then
        Call F90_REPUCI(akey(i), ucibf, Len(ucibf))
      End If
      istart = iend + 2
    Else
      'remove this record
      Call F90_DELUCI(akey(i))
    End If
  Next i
  While moretext = True
    'still have more lines to put
    iend = InStr(istart, txtRec.Text, Chr(13) & Chr(10))
    If iend = 0 Then
      'last line of text
      ilen = 80
      moretext = False
    Else
      ilen = iend - istart
    End If
    ucibf = Mid(txtRec.Text, istart, ilen)
    Call F90_PUTUCI(akey(nkey), 0, ucibf, Len(ucibf))
    istart = iend + 2
  Wend
End Sub

Private Sub unfill_lstRec()
  txtRec.Visible = False
  lblBlock.Visible = False
  gridctrl1.Visible = False
  For i = 0 To 6
    cmdTable(i).Visible = False
  Next i
  picGlobal.Visible = False
  nkey = 0
  ReDim akey(nkey)
  txtRec.Text = ""
  If frmGenScnActivate.WindowState = vbNormal Then
    frmGenScnActivate.Height = txtRec.Top - 20
    frmGenScnActivate.Width = fraTname.Left + fraTname.Width + 300
  End If
  'frmGenScnActivate.BorderStyle = 3 'Fixed Dialog, no resize
  For i = 0 To 5
    cmdOpt(i).Enabled = True
  Next i
  lstTname.Enabled = True
  lstKwd.Enabled = True
  optOpnKwd(0).Enabled = True
  optOpnKwd(1).Enabled = True
  optOpnTname(0).Enabled = True
  optOpnTname(1).Enabled = True
End Sub
Private Sub refresh_optKwd()
     Dim kwd$, kflg&, init&, id&, contfg&, retid&, ind%
     
     lstKwd.Clear
     init = 1
     id = 0
     Do
       Call F90_GTNXKW(init, id, kwd, kflg, contfg, retid)
       If (kflg > 0 Or optOpnKwd(0)) Then 'And retid <> 2 Then
         lstKwd.AddItem kwd
         lstKwd.ItemData(lstKwd.NewIndex) = retid
         If kflg > 0 And (retid > 120 And retid < 132) Then
           'this operation exists, mark it
           ind = retid - 120
           Oper(ind).Exists = True
           Oper(ind).Name = kwd
           Call GetOperInfo(Oper(ind))
         End If
       End If
       init = 0
     Loop While contfg = 1
     fraTname.Visible = False

End Sub

Private Sub refresh_lstTname(id As Integer)
  Dim kwd$, kflg&, init&, contfg&, retid&, lid&
  Dim ctxt$, i&, noccur&
  
  lstTname.Clear
  If id > 0 Then
    init = 1
    Do
      Call F90_GTNXKW(init, CLng(id), kwd, kflg, contfg, retid)
      If (kflg > 0 Or optOpnTname(0)) And retid <> 0 Then
        'check for multiple occurances
        Call F90_GETOCR(retid, noccur)
        If noccur > 1 Then
          'multiple occurances
          For i = 1 To noccur
            ctxt = kwd & " (" & i & ")"
            lstTname.AddItem ctxt
            lstTname.ItemData(lstTname.NewIndex) = retid
          Next i
        Else
          'single occurance
          lstTname.AddItem kwd
          lstTname.ItemData(lstTname.NewIndex) = retid
        End If
      End If
      init = 0
    Loop While contfg = 1
    optOpnTname(0).Visible = True
    optOpnTname(1).Visible = True
    lstTname.Height = lstKwd.Height
    fraTname.Caption = "Table"
  Else
    optOpnTname(0).Visible = False
    optOpnTname(1).Visible = False
    lstTname.Height = optOpnTname(0).Top + optOpnTname(0).Height - lstTname.Top
    If id = -1 Then 'MassLink
      fraTname.Caption = "MassLink"
    ElseIf id = -2 Then 'Ftable
      fraTname.Caption = "Ftable"
    End If
    lid = id - 100
    init = 1
    Do
      Call F90_GTNXKW(init, lid, kwd, kflg, contfg, retid)
      If retid <> 0 Then
        lstTname.AddItem fraTname.Caption & " " & LTrim(kwd)
        lstTname.ItemData(lstTname.NewIndex) = retid
      End If
      init = 0
    Loop While contfg = 1
  End If
  fraTname.Visible = True
End Sub

Private Sub open_lstRecTab()
  Dim cbuff$, omcode&, tabno&, init&, retcod&, uunits&, retkey&, temptext$
  Dim lastUnbroken%, breakAfter% 'for deciding which cols headers go in
  Dim i&, j&, c&, ch$, thisoccur&, noccur&, ctmp$
  hdrbuf(0) = ""
  open_table
  Call F90_PUTOLV(6)
  nkey = 0
  ReDim akey(nkey)
  txtRec.Text = ""
  temptext = ""
  With gridctrl1
    .Clear
    .Cols = 2
    .Rows = 2
  
    init = 1
    uunits = comboUnits.ListIndex + 1
    omcode = lstKwd.ItemData(lstKwd.ListIndex)
    tabno = lstTname.ItemData(lstTname.ListIndex)
    tabno = tabno - ((omcode - 120) * 1000)
    
    Call F90_XTINFO(omcode, tabno, uunits, _
      lnflds, lscol, lflen, lftyp, lapos, _
      limin, limax, lidef, lrmin, lrmax, lrdef, _
      lnmhdr, hdrbuf, lfdnam, retcod)
    If retcod <> 0 Then MsgBox "lstRecTab:XTINFO:ret: " & retcod
    If lnflds < 1 Then
      .header = "This table does not exist in this project."
      Exit Sub
    End If
    
    .Rows = 1000
    .FixedRows = 1
    .Cols = lnflds
    
    If lflen(0) = 8 Then
      For c = 1 To lnflds - 1
        lscol(c) = lscol(c) + 2
      Next c
    End If
  
    Dim tlen%
    For c = 0 To lnflds - 1
      .ColTitle(c) = lfdnam(c)
      tlen = 0
      If lfdnam(c) = "OPNID" Then
        tlen = Len("OPNID")
        lflen(0) = 5
        .ColEditable(c) = False
      Else
        .ColEditable(c) = True
      End If
      ch = Mid(lftyp, c + 1, 1)
      If ch = "C" Then
        tlen = lflen(c) + 2
      ElseIf tlen < lflen(c) Then
        tlen = lflen(c)
      End If
      If Len(lfdnam(c)) > tlen Then
        tlen = Len(lfdnam(c))
      End If
      .colWidth(c) = Me.TextWidth(String(tlen, "R"))
     
      If ch = "C" Then
        .ColType(c) = ATCoCtl.ATCoTxt
        .ColMin(c) = 0
        .ColMax(c) = (lflen(c))
      ElseIf ch = "I" Then
        .ColType(c) = ATCoCtl.ATCoInt
        .ColMin(c) = (limin(lapos(c) - 1))
        .ColMax(c) = (limax(lapos(c) - 1))
      ElseIf ch = "R" Then
        .ColType(c) = ATCoCtl.ATCoSng
        .ColMin(c) = lrmin(lapos(c) - 1)
        .ColMax(c) = lrmax(lapos(c) - 1)
      End If
    Next c
    
    .header = "" 'hdrbuf(0)
    Dim lenHdr
    .header = hdrbuf(0)
    For i = 1 To lnmhdr - 2
      .header = .header & vbCr & hdrbuf(i)
    Next i
    
    .row = 1
    txtRec.Visible = False
    thisoccur = 1
    'check for multiple occurances
    Call F90_GETOCR(lstTname.ItemData(lstTname.ListIndex), noccur)
    If noccur > 1 Then
      'which occurance is wanted
      ctmp = lstTname.List(lstTname.ListIndex)
      thisoccur = CInt(Mid(ctmp, Len(ctmp) - 1, 1))
    End If
    
    Do
      Call F90_XTABLE(omcode, tabno, uunits, init, CLng(1), thisoccur, _
                      retkey, cbuff, retcod)
      init = 0
      'If (retcod = 1 Or retcod = 2) And nkey < 1000 Then
      If nkey < 1000 Then
        If Len(temptext) = 0 Then
          temptext = cbuff
        Else
          temptext = temptext & vbCrLf & cbuff
        End If
        
        If retcod = 2 Then
          ' **** may need add multiple records with opn range (something in col 6/10)
          nkey = nkey + 1
          ReDim Preserve akey(nkey)
          akey(nkey) = retkey
          For c = 0 To .Cols - 1
            .Col = c
            If Len(cbuff) > lscol(c) Then
              .Text = Trim(Mid(cbuff, lscol(c), lflen(c)))
            Else
              .Text = ""
            End If
          Next c
          .row = .row + 1
        End If
      End If
    Loop While retcod = 1 Or retcod = 2
    .row = .row - 1
    .Rows = .row
    .row = 1
    .Col = 0
    .Visible = True
    txtRec.Visible = Not .Visible
    txtRec.Text = temptext
  End With
End Sub
Public Sub SaveUci()
  If Len(txtInfo.Text) = 0 Then
    inf = "<none>"
  Else
    inf = txtInfo.Text
  End If
  ou = comboOutput.ListIndex
  sp = comboSpecial.ListIndex
  ru = comboRunflag.ListIndex
  em = comboUnits.ListIndex + 1
  MousePointer = vbHourglass
  Call F90_PUTGLO(s(0), e(0), ou, sp, ru, em, inf, Len(inf))
  Call F90_UCISAV
  MousePointer = vbDefault
End Sub


Private Sub open_lstRecOpn()
  Dim cbuff$, omcode&, init&, retcod&, retkey&, temptext$
  Dim i&, c&, ch$
  
  'Call F90_SCNDBG(2)
  open_table
  hdrbuf(0) = ""
  nkey = 0
  ReDim akey(nkey)
  txtRec.Text = ""
  temptext = ""
  With gridctrl1
    .Clear
    gridctrl1.Cols = 2
    .Rows = 2
    omcode = lstKwd.ItemData(lstKwd.ListIndex)
    If omcode = 4 Or omcode = 11 Then
      init = lstTname.ItemData(lstTname.ListIndex)
    Else
      init = 1
    End If
    
    Call F90_XTINFO(omcode, 0, 0, _
        lnflds, lscol, lflen, lftyp, lapos, _
        limin, limax, lidef, lrmin, lrmax, lrdef, _
        lnmhdr, hdrbuf, lfdnam, retcod)
    If retcod <> 0 Then
      MsgBox "lstRecTab:XTINFO:ret: " & retcod & " " & omcode
    Else
      If lnflds < 1 Then
        .header = "This table does not exist in this project."
        Exit Sub
      End If
      If omcode <> 4 Then ' not ftable
        lftyp = Mid(lftyp, 2, lnflds - 1) 'left shift types
        For c = 1 To lnflds - 1
          lscol(c - 1) = lscol(c) - 3
          lflen(c - 1) = lflen(c)
          lfdnam(c - 1) = lfdnam(c)
          lapos(c - 1) = lapos(c)
          If Mid(lftyp, c, 1) = "I" Then
            limin(lapos(c - 1)) = limin(lapos(c))
            limax(lapos(c - 1)) = limax(lapos(c))
            lidef(lapos(c - 1)) = lidef(lapos(c))
          ElseIf Mid(lftyp, c, 1) = "R" Then
            lrmin(lapos(c - 1)) = lrmin(lapos(c))
            lrmax(lapos(c - 1)) = lrmax(lapos(c))
            lrdef(lapos(c - 1)) = lrdef(lapos(c))
          End If
        Next c
        lnflds = lnflds - 1
      End If
      .Cols = lnflds
      .Rows = 1000
      .header = hdrbuf(0)
      For i = 1 To lnmhdr - 1
        .header = .header & vbCr & hdrbuf(i)
      Next i
      
      Dim tlen%
      For c = 0 To lnflds - 1
        .ColTitle(c) = lfdnam(c)
        .ColEditable(c) = True
        ch = Mid(lftyp, c + 1, 1)
        If ch = "C" Then
          tlen = lflen(c) + 2
        Else
          tlen = lflen(c)
        End If
        If Len(lfdnam(c)) > tlen Then
          tlen = Len(lfdnam(c))
        End If
        .colWidth(c) = Me.TextWidth(String(tlen, "R"))
        If ch = "C" Then
          .ColType(c) = ATCoCtl.ATCoTxt
          .ColMin(c) = 0
          .ColMax(c) = (lflen(c))
        ElseIf ch = "I" Then
          .ColType(c) = ATCoCtl.ATCoInt
          .ColMin(c) = (limin(lapos(c) - 1))
          .ColMax(c) = (limax(lapos(c) - 1))
        ElseIf ch = "R" Then
          .ColType(c) = ATCoCtl.ATCoSng
          .ColMin(c) = lrmin(lapos(c) - 1)
          .ColMax(c) = lrmax(lapos(c) - 1)
        End If
      Next c
    
      .ClearValues
      .row = 1
      txtRec.Visible = False
      Do
        Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
        'If (retcod = 1 Or retcod = 2) And nkey < 500 Then
        If nkey < 500 Then
          If Len(temptext) = 0 Then
            temptext = cbuff
          Else
            temptext = temptext & vbCrLf & cbuff
          End If
         
          If omcode = 4 And init > 0 Then
            'skip row col part of ftable
          ElseIf retcod = 2 Or (omcode = 4 And retcod = 10) Or (omcode = 11 And retcod = 10) Then
            nkey = nkey + 1
            ReDim Preserve akey(nkey)
            akey(nkey) = retkey
            For c = 0 To .Cols - 1
              .Col = c
              If Len(cbuff) > lscol(c) Then
                If omcode <> 9 Then
                  'not specact block, can trim
                  .Text = Trim(Mid(cbuff, lscol(c), lflen(c)))
                Else
                  'need indentation
                  .Text = Mid(cbuff, lscol(c), lflen(c))
                End If
              Else
                .Text = ""
              End If
            Next c
            .row = .row + 1
          End If
       
        End If
        init = 0
      Loop While retcod = 1 Or retcod = 2
      .row = .row - 1
      .Rows = .row
      .row = 1
      .Col = 0
      .Visible = True
      txtRec.Visible = Not .Visible
      txtRec.Text = temptext
    End If
  End With
  'Call F90_SCNDBG(0)
End Sub
Private Sub cmdOpt_Click(Index As Integer)
   Dim cap$, FileName$, retcod&, init&, omcode&, retkey&, cbuff$
   If Index = 0 Then 'simulate
     Dim d&, r&, rsp%, lena&, lenn&, newcap$
     d = 3
     MousePointer = vbHourglass
     On Error Resume Next
     Call F90_SIMSCN(r)
     MousePointer = vbDefault
     On Error GoTo 0
   ElseIf Index = 1 Then 'save
     rsp = MsgBox("Do you want to Overwrite Scenario '" & Left(ScenFile, Len(ScenFile) - 4) & "'?", _
       4, "GenScnActivateSave Overwrite Query")
     If rsp = vbYes Then
       Call SaveUci
     End If
   ElseIf Index = 2 Then 'save as
     frmGenActCopySV.Show 1, frmGenScnActivate
     'newname = InputBox("Enter new scenario name (eight characters maximum):", "GenScnActivate SaveAs")
     If newname <> "" Then
       newname = UCase(newname)
       Dim invalid&, i&
       invalid = 0
       For i = 0 To frmGenScn.lstSLC(0).ListCount - 1
         If frmGenScn.lstSLC(0).List(i) = newname Then
           'invalid, name already used
           invalid = 1
         End If
       Next i
       If invalid = 1 Then
         MsgBox "This scenario name is already in use.", _
         vbExclamation, "GenScnActivate SaveAs"
       Else
         MousePointer = vbHourglass
         lena = Len(ScenFile) - 4
         lenn = Len(newname)
         Call F90_COPSCN(BaseDSN, RelAbs, Left(ScenFile, lena), Left(newname, lenn), lena, lenn)
         ScenFile = Left(newname, lenn) & ".uci"
         'reset files list
         lstFiles.Clear
         retcod = 0
         init = 1
         omcode = 12
         Do
           Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
           If retcod <> 2 Then Exit Do
           init = 0
           If InStr(UCase(cbuff), "WDM") = 0 Then
             lstFiles.AddItem Right(cbuff, Len(cbuff) - 16)
           End If
         Loop
         lstFiles.ListIndex = 0
         If Len(txtInfo.Text) = 0 Then
           inf = "<none>"
         Else
           inf = txtInfo.Text
         End If
         ou = comboOutput.ListIndex
         sp = comboSpecial.ListIndex
         ru = comboRunflag.ListIndex
         em = comboUnits.ListIndex + 1
         Call F90_PUTGLO(s(0), e(0), ou, sp, ru, em, inf, Len(inf))
         Call F90_UCISAV
         newcap = Mid(Caption, 1, 16) + Left(newname, lenn) + "        "
         Caption = newcap
         'reset scenario list and number of data sets
         p.ScenCount = p.ScenCount + 1
         ReDim Preserve p.Scen(p.ScenCount)
         p.Scen(p.ScenCount - 1).Name = newname
         frmGenScn.lstSLC(0).AddItem newname
         Dim Scen$, newcnt&
         'scen = "Scenarios:" & Chr(13) & Chr(10)
         Scen = frmGenScn.lstSLC(0).SelCount & " of " & frmGenScn.lstSLC(0).ListCount
         frmGenScn.lblSLC(0).Caption = Scen
         'refresh wdm
         p.WDMFiles(1).Refresh
         CntDsn = CountAllTimser
         Call frmGenScn.UpdateLblDsn
         MousePointer = vbDefault
       End If
     End If
   ElseIf Index = 3 Then 'close
     Unload frmGenScnActivate
   ElseIf Index = 4 Then 'view file from files block
     cap = "GenScn Activate View"
     FileName = lstFiles.List(lstFiles.ListIndex)
     Call ATCoDispFile1.OpenFile(FileName, cap, frmGenScnActivate.Icon, False)
   ElseIf Index = 5 Then 'modify
     frmGenActCopyMod.Show 1
     Call refresh_optKwd
   End If
End Sub

Private Sub cmdTable_Click(Index As Integer)
    If Index = 0 Then 'ok, put table back
      If lstKwd.ItemData(lstKwd.ListIndex) <> 2 Then
        Call Put_Table_Back
      End If
      Call unfill_lstRec
    ElseIf Index = 1 Then 'cancel
      If lstKwd.ListIndex < 0 Then 'nothing selected
        'MsgBox "Bad lstkwd.listindex (missing table)"
      ElseIf lstKwd.ItemData(lstKwd.ListIndex) = 2 Then
        'put global block settings back to the way they were
        comboOutput.ListIndex = ousav
        comboSpecial.ListIndex = spsav
        comboRunflag.ListIndex = rusav
        comboUnits.ListIndex = emsav
        txtStart.Text = sysav
        txtEnd.Text = eysav
        txtSmon.Text = smosav
        txtEmon.Text = emosav
        txtSday.Text = sdsav
        txtEday.Text = edsav
        txtShour.Text = shsav
        txtEhour.Text = ehsav
        txtSmin.Text = smisav
        txtEmin.Text = emisav
      End If
      Call unfill_lstRec
    ElseIf Index = 2 Then 'help
      k = lblBlock.Caption
      i = WinHelp(CLng(frmGenScnActivate.hWnd), ExPath & "\doc\hspfhelp.hlp", _
                  CLng(HELP_KEY), k)
    ElseIf Index = 3 Then 'insert
      gridctrl1.InsertRow (1)
    ElseIf Index = 4 Then 'delete
      If gridctrl1.Rows > 1 Then
        gridctrl1.DeleteRows
      Else
        MsgBox "Can't delete last row"
      End If
    ElseIf Index = 5 Then 'toggle text/grid
      gridctrl1.Visible = Not gridctrl1.Visible
      txtRec.Visible = Not gridctrl1.Visible
    ElseIf Index = 6 Then 'debug
      Dim s$, t$
      If lstKwd.ListIndex < 0 Then
        s = lstKwd.ListIndex
      Else
        s = "Name: " & lstKwd.List(lstKwd.ListIndex) & vbCrLf
        s = s & "Type: " & lstKwd.ItemData(lstKwd.ListIndex) & vbCrLf
        s = s & "NFld: " & lnflds & vbCrLf
        For i = 0 To lnflds - 1
          s = s & i & vbTab & "FNam: " & lfdnam(i) & vbTab
          t = Mid(lftyp, i + 1, 1)
          s = s & " T:" & t & vbTab & "P:" & lapos(i) & vbTab & " S:" & lscol(i) & vbTab & "W:" & lflen(i) & vbTab
          If t = "I" Then
            s = s & "Mn: " & limin(lapos(i) - 1) & vbTab & "Mx: " & limax(lapos(i) - 1) & vbCrLf
          ElseIf t = "R" Then
            s = s & "Mn: " & lrmin(lapos(i) - 1) & vbTab & "Mx: " & lrmax(lapos(i) - 1) & vbCrLf
          ElseIf t = "C" Then
            s = s & vbCrLf
          End If
        Next i
      End If
      MsgBox s
    End If
End Sub

Private Sub Form_Load()
     Dim r&, u$
     Dim omcode&, retcod&, init&, retkey&, cbuff$, wdmUnit&
     Dim plgIn As Variant
          
     'Call F90_SCNDBG(-1) '1:msg from Fortran dll to console, -1:msg and pause
     For i = 0 To 11
       Oper(i).Exists = False
     Next i
     u = Left(ScenFile, Len(ScenFile) - 4)
     Call F90_ACTSCN(CLng(0), p.WDMFiles(1).FileUnit, p.HSPFMsg.Unit, r&, u, Len(u))
     Caption = Caption & " " & u
     txtPath.Text = ScenPath(u)
    
     lstFiles.Clear
     retcod = 0
     init = 1
     omcode = 12
     Do
       Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
       If retcod <> 2 Then Exit Do
       init = 0
       If InStr(UCase(cbuff), "WDM") = 0 Then
         lstFiles.AddItem Right(cbuff, Len(cbuff) - 16)
       End If
     Loop
     lstFiles.ListIndex = 0
     
     Call refresh_optKwd
     
     lstTname.Clear
     On Error Resume Next ' kludge for vb prob?
     frmGenScnActivate.Height = txtRec.Top - 20
     On Error GoTo 0
     Show
     MousePointer = vbDefault
     
End Sub

Private Sub Form_Resize()
  With frmGenScnActivate
    If .Width > 1500 Then
      txtPath.Width = .Width - 1500
      txtInfo.Width = txtPath.Width
      gridctrl1.Width = .Width - 675
      txtRec.Width = gridctrl1.Width
    End If
    If .Height > 5000 Then
      gridctrl1.Height = .Height - gridctrl1.Top - 500
      txtRec.Height = gridctrl1.Height
    End If
  End With
End Sub

Private Sub Form_Unload(Cancel As Integer)
    'frmGenScn.Enabled = True
    frmGenScn.MousePointer = vbDefault
    i = WinHelp(CLng(frmGenScnActivate.hWnd), "", CLng(HELP_QUIT), CLng(0))
End Sub


Private Sub GridCtrl1_RowColChange()
  With gridctrl1
    If .Visible Then
      If lblBlock.Caption = "EXT SOURCES" Then
        Call Edit_ExtSources(gridctrl1, 2)
      ElseIf lblBlock.Caption = "EXT TARGETS" Then
        Call Edit_ExtTargets(gridctrl1, 2)
      ElseIf lblBlock.Caption = "NETWORK" Then
        Call Edit_Network(gridctrl1, 2)
      ElseIf lblBlock.Caption = "MASS-LINK" Then
        Call Edit_MassLink(gridctrl1, 2)
      ElseIf lblBlock.Caption = "SCHEMATIC" Then
        Call Edit_Schematic(gridctrl1, 2)
      End If
    End If
  End With
End Sub

Private Sub gridctrl1_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  With gridctrl1
    If .Visible Then
      If lblBlock.Caption = "EXT SOURCES" Then
        Call Edit_ExtSources(gridctrl1, 1)
      ElseIf lblBlock.Caption = "EXT TARGETS" Then
        Call Edit_ExtTargets(gridctrl1, 1)
      ElseIf lblBlock.Caption = "NETWORK" Then
        Call Edit_Network(gridctrl1, 1)
      ElseIf lblBlock.Caption = "MASS-LINK" Then
        Call Edit_MassLink(gridctrl1, 1)
      ElseIf lblBlock.Caption = "SCHEMATIC" Then
        Call Edit_Schematic(gridctrl1, 1)
      End If
    End If
  End With
End Sub

Private Sub lstFiles_DblClick()
  cmdOpt_Click (4)
End Sub

Private Sub lstKwd_Click()
    Dim s$
    
    MousePointer = vbHourglass
    If lstKwd.ItemData(lstKwd.ListIndex) = 2 Then
      'global block
      fraTname.Visible = False
      Call Edit_Global
    ElseIf lstKwd.ItemData(lstKwd.ListIndex) < 100 Then
      fraTname.Visible = False
      lstTname.Clear
      s = lstKwd.List(lstKwd.ListIndex)
      lblBlock.Caption = s
      If s = "MASS-LINK" Then 'which one
        Call refresh_lstTname(-1)
      ElseIf s = "FTABLES" Then 'which one
        Call refresh_lstTname(-2)
      Else
        Call open_lstRecOpn
      End If
    Else
      Call refresh_lstTname(lstKwd.ItemData(lstKwd.ListIndex))
    End If
    MousePointer = vbDefault

End Sub

Private Sub lstTname_Click()
  lblBlock.Caption = lstTname.List(lstTname.ListIndex)
  If fraTname.Caption = "Table" Then
    'need to check to see if this table exists
    i = lstKwd.ItemData(lstKwd.ListIndex)
    If Oper(i - 120).Exists Then
      Call open_lstRecTab
    Else
      MsgBox "Adding a new operation type block is not yet supported.", 64, "GenScn Activate"
      lblBlock.Caption = ""
    End If
  ElseIf fraTname.Caption = "Ftable" Then
    Call open_lstRecOpn
  ElseIf fraTname.Caption = "MassLink" Then
    Call open_lstRecOpn
  End If
End Sub

Private Sub optOpnKwd_Click(Index As Integer)
    Call refresh_optKwd
    lstTname.Clear
End Sub

Private Sub optOpnTname_Click(Index As Integer)
    Dim i%
    If lstKwd.ListIndex >= 0 Then
      i = lstKwd.ItemData(lstKwd.ListIndex)
      If i >= 100 Then
        Call refresh_lstTname(i)
      End If
    End If
End Sub


Private Sub txtEDay_GotFocus()
    oldeday = txtEday.Text
End Sub
Private Sub txtEDay_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEday.Text) > 0 Then
      snew = txtEday.Text
      Call ChkTxtI("Day", 1, 31, snew, e(2), chgflg)
      If chgflg <> 1 Then
        txtEday.Text = oldeday
      End If
    Else
      txtEday.Text = oldeday
    End If
End Sub
Private Sub txtEhour_GotFocus()
    oldehour = txtEhour.Text
End Sub
Private Sub txtEhour_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEhour.Text) > 0 Then
      snew = txtEhour.Text
      Call ChkTxtI("Hour", 0, 24, snew, e(3), chgflg)
      If chgflg <> 1 Then
        txtEhour.Text = oldehour
      End If
    Else
      txtEhour.Text = oldehour
    End If
End Sub


Private Sub txtEmin_GotFocus()
    oldeminute = txtEmin.Text
End Sub


Private Sub txtEmin_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEmin.Text) > 0 Then
      snew = txtEmin.Text
      Call ChkTxtI("Minute", 0, 60, snew, e(4), chgflg)
      If chgflg <> 1 Then
        txtEmin.Text = oldeminute
      End If
    Else
      txtEmin.Text = oldeminute
    End If
End Sub


Private Sub txtEmon_GotFocus()
    oldemonth = txtEmon.Text
End Sub


Private Sub txtEmon_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEmon.Text) > 0 Then
      snew = txtEmon.Text
      Call ChkTxtI("Month", 1, 12, snew, e(1), chgflg)
      If chgflg <> 1 Then
        txtEmon.Text = oldemonth
      End If
    Else
      txtEmon.Text = oldemonth
    End If
End Sub


Private Sub txtEnd_GotFocus()
    oldeyear = txtEnd.Text
End Sub


Private Sub txtEnd_LostFocus()
    Dim snew$, chgflg&
    If Len(txtEnd.Text) > 0 Then
      snew = txtEnd.Text
      Call ChkTxtI("Year", 1900, 2100, snew, e(0), chgflg)
      If chgflg <> 1 Then
        txtEnd.Text = oldeyear
      End If
    Else
      txtEnd.Text = oldeyear
    End If
End Sub


Private Sub txtSDay_GotFocus()
    oldsday = txtSday.Text
End Sub


Private Sub txtSDay_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSday.Text) > 0 Then
      snew = txtSday.Text
      Call ChkTxtI("Day", 1, 31, snew, s(2), chgflg)
      If chgflg <> 1 Then
        txtSday.Text = oldsday
      End If
    Else
      txtSday.Text = oldsday
    End If
End Sub


Private Sub txtShour_GotFocus()
    oldshour = txtShour.Text
End Sub


Private Sub txtShour_LostFocus()
    Dim snew$, chgflg&
    If Len(txtShour.Text) > 0 Then
      snew = txtShour.Text
      Call ChkTxtI("Hour", 0, 24, snew, s(3), chgflg)
      If chgflg <> 1 Then
        txtShour.Text = oldshour
      End If
    Else
      txtShour.Text = oldshour
    End If
End Sub


Private Sub txtSmin_GotFocus()
    oldsminute = txtSmin.Text
End Sub


Private Sub txtSmin_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSmin.Text) > 0 Then
      snew = txtSmin.Text
      Call ChkTxtI("Minute", 0, 60, snew, s(4), chgflg)
      If chgflg <> 1 Then
        txtSmin.Text = oldsminute
      End If
    Else
      txtSmin.Text = oldsminute
    End If
End Sub


Private Sub txtSmon_GotFocus()
    oldsmonth = txtSmon.Text
End Sub
Private Sub txtSmon_LostFocus()
    Dim snew$, chgflg&
    If Len(txtSmon.Text) > 0 Then
      snew = txtSmon.Text
      Call ChkTxtI("Month", 1, 12, snew, s(1), chgflg)
      If chgflg <> 1 Then
        txtSmon.Text = oldsmonth
      End If
    Else
      txtSmon.Text = oldsmonth
    End If
End Sub
Private Sub txtStart_GotFocus()
    oldsyear = txtStart.Text
End Sub
Private Sub txtStart_LostFocus()
    Dim snew$, chgflg&
    If Len(txtStart.Text) > 0 Then
      snew = txtStart.Text
      Call ChkTxtI("Year", 1900, 2100, snew, s(0), chgflg)
      If chgflg <> 1 Then
        txtStart.Text = oldsyear
      End If
    Else
      txtStart.Text = oldsyear
    End If
End Sub
