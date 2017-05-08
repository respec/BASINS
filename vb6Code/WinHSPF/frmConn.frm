VERSION 5.00
Begin VB.Form frmConn 
   Caption         =   "WinHSPF - Connections"
   ClientHeight    =   5010
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   7950
   Icon            =   "frmConn.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   5010
   ScaleWidth      =   7950
   StartUpPosition =   2  'CenterScreen
   Begin ATCoCtl.ATCoGrid agdConn 
      Height          =   3972
      Left            =   60
      TabIndex        =   0
      Top             =   60
      Width           =   7452
      _ExtentX        =   13150
      _ExtentY        =   7011
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
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   2520
      TabIndex        =   1
      Top             =   4440
      Width           =   2892
      Begin VB.CommandButton cmdOutput 
         Caption         =   "&OK"
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
         Index           =   0
         Left            =   0
         TabIndex        =   3
         Top             =   0
         Width           =   1335
      End
      Begin VB.CommandButton cmdOutput 
         Cancel          =   -1  'True
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
         Left            =   1560
         TabIndex        =   2
         Top             =   0
         Width           =   1335
      End
   End
End
Attribute VB_Name = "frmConn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdOutput_Click(Index As Integer)
  Dim i&
  If Index = 0 Then
    'okay
  End If
  Unload Me
End Sub


Private Sub Form_Load()
  
  Dim lTable As HspfTable
  Dim lOper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&
  Dim TableName$

  TableName = frmEdit.tabname
  With agdConn
    .rows = 0
    .Cols = 3
    .ColTitle(0) = "Source"
    .ColTitle(1) = "Member"
    .ColTitle(2) = "Target"
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      Set lOper = myUci.OpnSeqBlock.Opn(i)
      If TableName <> "EXT SOURCES" Then
        For j = 1 To lOper.targets.Count
          Set lConn = lOper.targets(j)
          If (lConn.typ = 2 And TableName = "NETWORK") Or _
             (lConn.typ = 3 And TableName = "SCHEMATIC") Or _
             (lConn.typ = 4 And TableName = "EXT TARGETS") Then
            '2-Network,3-Schematic,4-ExtTarget
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lOper.Name & " " & lOper.id
            .TextMatrix(.rows, 1) = lConn.Target.member
            .TextMatrix(.rows, 2) = lConn.Target.volname & " " & lConn.Target.volid
          End If
        Next j
      Else
        For j = 1 To lOper.Sources.Count
          Set lConn = lOper.Sources(j) 'Changed by Mark
          If (lConn.typ = 1) Then
            '1-ExtSource
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lOper.Name & " " & lOper.id
            .TextMatrix(.rows, 1) = lConn.Target.member
            .TextMatrix(.rows, 2) = lConn.Target.volname & " " & lConn.Target.volid
          End If
        Next j
      End If
    Next i
    .ColsSizeByContents
  End With
End Sub

Private Sub Form_Resize()
  If width > 200 And height > 1000 Then
    agdConn.width = width - 200
    fraButtons.Left = width / 2 - fraButtons.width / 2
    fraButtons.Top = height - fraButtons.height - 400
    agdConn.height = fraButtons.Top - 200
  End If
End Sub
