VERSION 5.00
Begin VB.Form frmLand 
   Caption         =   "WinHSPF - LandUse Editor"
   ClientHeight    =   6972
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   11400
   HelpContextID   =   37
   Icon            =   "frmLand.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6972
   ScaleWidth      =   11400
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraLand 
      BorderStyle     =   0  'None
      Height          =   6735
      Index           =   0
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   11175
      Begin ATCoCtl.ATCoSelectList lstTar 
         Height          =   2295
         Left            =   5880
         TabIndex        =   6
         Top             =   360
         Width           =   5295
         _ExtentX        =   9335
         _ExtentY        =   4043
         RightLabel      =   "Selected:"
         LeftLabel       =   "Available:"
      End
      Begin ATCoCtl.ATCoSelectList lstSou 
         Height          =   2175
         Left            =   120
         TabIndex        =   5
         Top             =   360
         Width           =   5415
         _ExtentX        =   9546
         _ExtentY        =   3831
         RightLabel      =   "Selected:"
         LeftLabel       =   "Available:"
      End
      Begin VB.CommandButton cmdLand 
         Caption         =   "&OK"
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
         Index           =   0
         Left            =   1440
         TabIndex        =   2
         Top             =   6000
         Width           =   1335
      End
      Begin VB.CommandButton cmdLand 
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
         Height          =   375
         Index           =   1
         Left            =   2880
         TabIndex        =   1
         Top             =   6000
         Width           =   1335
      End
      Begin ATCoCtl.ATCoGrid agdLand 
         Height          =   3015
         Left            =   240
         TabIndex        =   3
         Top             =   2640
         Width           =   10815
         _ExtentX        =   19071
         _ExtentY        =   5313
         SelectionToggle =   -1  'True
         AllowBigSelection=   -1  'True
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   0   'False
         Rows            =   2
         Cols            =   4
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
         BackColorBkg    =   -2147483638
         BackColorSel    =   -2147483635
         ForeColorSel    =   -2147483634
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   0   'False
      End
      Begin VB.Label lblTotal 
         Alignment       =   1  'Right Justify
         Caption         =   "Total: 0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H000000FF&
         Height          =   255
         Index           =   2
         Left            =   8280
         TabIndex        =   10
         Top             =   6360
         Width           =   2415
      End
      Begin VB.Label lblTotal 
         Alignment       =   1  'Right Justify
         Caption         =   "Total: 0"
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
         Left            =   8400
         TabIndex        =   9
         Top             =   6120
         Width           =   2295
      End
      Begin VB.Label lblTarget 
         Caption         =   "Targets"
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
         Left            =   6000
         TabIndex        =   8
         Top             =   120
         Width           =   1695
      End
      Begin VB.Label lblSource 
         Caption         =   "Sources"
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
         TabIndex        =   7
         Top             =   120
         Width           =   1695
      End
      Begin VB.Label lblTotal 
         Alignment       =   1  'Right Justify
         Caption         =   "Total: 0"
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
         Left            =   8040
         TabIndex        =   4
         Top             =   5880
         Width           =   2655
      End
   End
End
Attribute VB_Name = "frmLand"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim OrigTotal#
Private Sub agdLand_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim i&, j&, s$, lName$, lId&
  Dim t#
  
  t = 0
  With agdLand
    For i = 1 To .rows
      t = t + .TextMatrix(i, 4)
    Next i
    If t = OrigTotal Then
      lblTotal(0) = "Total: " & t
      lblTotal(1).Visible = False
      lblTotal(2).Visible = False
    Else
      lblTotal(0) = "New Total: " & t
      lblTotal(1) = "Original Total: " & OrigTotal
      lblTotal(1).Visible = True
      s = CStr(Format(t - OrigTotal, "#####0.00"))
      lblTotal(2) = "Difference: " & s
      lblTotal(2).Visible = True
    End If
  End With
End Sub


Private Sub cmdLand_Click(Index As Integer)
  Dim lTable As HspfTable
  Dim loper As HspfOperation, vConn As Variant, lConn As HspfConnection
  Dim i&, rows&, s$, lName$, lId&
  
  If Index = 0 Then
    'okay
    rows = 0
    With agdLand
      For i = 0 To lstTar.RightCount - 1
        s = lstTar.RightItem(i)
        lName = StrRetRem(s)
        lId = s
        Set loper = GetOper(lName, lId)
        For Each vConn In loper.Sources
          Set lConn = vConn
          If lConn.typ = 3 Then
            If lstSou.InRightList(lConn.Source.volname & " " & lConn.Source.volid) Then
              rows = rows + 1
              lConn.MFact = .TextMatrix(rows, 4)
            End If
          End If
        Next vConn
      Next i
    End With
    myUci.Edited = True
  End If
  Unload Me
End Sub

Private Sub cmdRefresh_Click()
  Call RefreshGrid
End Sub

Private Sub RefreshGrid()
  Dim i&, j&, s$, lName$, lId&
  Dim loper As HspfOperation, vConn As Variant, lConn As HspfConnection
  Dim t#
  
  t = 0
  With agdLand
    .Visible = False
    .ClearData
    .rows = 0
    For i = 0 To lstTar.RightCount - 1
      s = lstTar.RightItem(i)
      lName = StrRetRem(s)
      lId = s
      Set loper = GetOper(lName, lId)
      For Each vConn In loper.Sources
        Set lConn = vConn
        If lConn.typ = 3 Then 'schematic record
          If lstSou.InRightList(lConn.Source.volname & " " & lConn.Source.volid) Then
  'Debug.Print lConn.Source.volname & " " & lConn.Source.volid
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lConn.Source.volname & " " & lConn.Source.volid
            .TextMatrix(.rows, 1) = GetOper(lConn.Source.volname, lConn.Source.volid).Description
            .TextMatrix(.rows, 2) = loper.Name & " " & loper.Id
            .TextMatrix(.rows, 3) = GetOper(loper.Name, loper.Id).Description
            .TextMatrix(.rows, 4) = lConn.MFact
            t = t + lConn.MFact
          End If
        End If
      Next vConn
    Next i
    .ColsSizeByContents
    .Visible = True
    lblTotal(0) = "Total: " & t
    lblTotal(1).Visible = False
    lblTotal(2).Visible = False
    OrigTotal = t
  End With
End Sub

Private Sub Form_Load()
  Dim lTable As HspfTable
  Dim loper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, j&, schemfound As Boolean, netfound As Boolean
  
  CheckSchematic schemfound, netfound
  If netfound And Not schemfound Then
    i = MsgBox("The Land Use Editor requires a Schematic Block." & vbCrLf & _
           "Would you like to convert the Network Block to a Schematic Block?", vbYesNo, _
           "WinHSPF - Land Use Editor")
    If i = vbYes Then
      ConvertNetworkToSchematic
      myUci.MaxAreaByLand2Stream = 0
    End If
  End If
  
  With agdLand
    .rows = 0
    .cols = 5
    .ColTitle(0) = "Source ID"
    .ColTitle(1) = "Source Description"
    .ColTitle(2) = "Target ID"
    .ColTitle(3) = "Target Description"
    If myUci.GlobalBlock.emfg = 1 Then
      .ColTitle(4) = "Area (Acres)"
    Else
      .ColTitle(4) = "Area (Hectares)"
    End If
    .ColEditable(4) = True
    .ColType(4) = ATCoSng
    .ColMin(4) = 0#
    For i = 1 To myUci.OpnSeqBlock.Opns.Count
      Set loper = myUci.OpnSeqBlock.Opn(i)
      If loper.Name = "RCHRES" Then
        lstTar.RightItem(lstTar.RightCount) = loper.Name & " " & loper.Id
        For j = 1 To loper.Sources.Count
          Set lConn = loper.Sources(j)
          If lConn.Source.volname = "PERLND" Then
            lstSou.RightItem(lstSou.RightCount) = lConn.Source.volname & " " & lConn.Source.volid
          End If
        Next j
        For j = 1 To loper.Sources.Count
          Set lConn = loper.Sources(j)
          If lConn.Source.volname = "IMPLND" Then
            lstSou.RightItem(lstSou.RightCount) = lConn.Source.volname & " " & lConn.Source.volid
          End If
        Next j
      End If
    Next i
  End With
  Call RefreshGrid
End Sub
Private Function GetOper(volname$, volid&) As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim loper As HspfOperation, vOper As Variant

  Set lOpnBlk = myUci.OpnBlks(volname)
  For Each vOper In lOpnBlk.Ids
    Set loper = vOper
    If loper.Id = volid Then
      Set GetOper = loper
      Exit For
    End If
  Next vOper
End Function

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If width < 1500 Then width = 1500
    If height < 1500 Then height = 1500
    lstSou.width = (width / 2) - 400
    lstTar.Left = lstSou.width + 500
    lstTar.width = lstSou.width
    agdLand.width = width - 600
    cmdLand(0).Left = (width / 2) - cmdLand(0).width - 200
    cmdLand(1).Left = (width / 2) + 200
    lblSource.Left = lstSou.Left + 200
    lblTarget.Left = lstTar.Left + 200
    fraLand(0).width = agdLand.width + 400
    lblTotal(0).Left = agdLand.Left + agdLand.width - lblTotal(0).width - 250
    lblTotal(1).Left = agdLand.Left + agdLand.width - lblTotal(1).width - 250
    lblTotal(2).Left = agdLand.Left + agdLand.width - lblTotal(2).width - 250
    lblTotal(2).Top = height - lblTotal(2).height - 500
    lblTotal(1).Top = height - lblTotal(2).height - 750
    lblTotal(0).Top = height - lblTotal(2).height - 1000
    cmdLand(0).Top = lblTotal(1).Top
    cmdLand(1).Top = lblTotal(1).Top
    agdLand.height = height - lstSou.height - (5 * cmdLand(0).height)
    fraLand(0).height = height
  End If
End Sub

Private Sub lstSou_Change()
  Call RefreshGrid
End Sub

Private Sub lstTar_Change()
  Call RefreshGrid
End Sub

