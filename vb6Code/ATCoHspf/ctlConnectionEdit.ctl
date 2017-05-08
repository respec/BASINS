VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.UserControl ctlConnectionEdit 
   ClientHeight    =   4236
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   10596
   ScaleHeight     =   4236
   ScaleWidth      =   10596
   Begin VB.TextBox txtDefine 
      BackColor       =   &H80000000&
      CausesValidation=   0   'False
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   8.4
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   975
      Left            =   0
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Text            =   "ctlConnectionEdit.ctx":0000
      Top             =   3240
      Width           =   10455
   End
   Begin ATCoCtl.ATCoGrid agdConn 
      Height          =   3255
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   10455
      _ExtentX        =   18436
      _ExtentY        =   5736
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
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
Attribute VB_Name = "ctlConnectionEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pConnection As HspfConnection
Dim tabname$
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfConnection
  Set Owner = pConnection
End Property
Public Property Set Owner(newConnection As HspfConnection)

  Dim ltable As HspfTable
  Dim lOper As HspfOperation
  Dim lConn As HspfConnection
  Dim i&, s$, j&
  Dim tablename$
  
  Set pConnection = newConnection

  tablename = newConnection.DesiredRecordType
  With agdConn
    'set up grid
    If tablename = "NETWORK" Then
      .cols = 15
      .TextMatrix(0, 0) = "VolName"
      .ColType(0) = ATCoTxt
      .TextMatrix(0, 1) = "VolId"
      .ColType(1) = ATCoInt
      .TextMatrix(0, 2) = "Group"
      .ColType(2) = ATCoTxt
      .TextMatrix(0, 3) = "MemName"
      .ColType(3) = ATCoTxt
      .TextMatrix(0, 4) = "MemSub1"
      .ColType(4) = ATCoInt
      .TextMatrix(0, 5) = "MemSub2"
      .ColType(5) = ATCoInt
      .TextMatrix(0, 6) = "MultFact"
      .ColType(6) = ATCoSng
      .TextMatrix(0, 7) = "Tran"
      .ColType(7) = ATCoTxt
      .TextMatrix(0, 8) = "VolName"
      .ColType(8) = ATCoTxt
      .TextMatrix(0, 9) = "VolId"
      .ColType(9) = ATCoInt
      .TextMatrix(0, 10) = "Group"
      .ColType(10) = ATCoTxt
      .TextMatrix(0, 11) = "MemName"
      .ColType(11) = ATCoTxt
      .TextMatrix(0, 12) = "MemSub1"
      .ColType(12) = ATCoInt
      .TextMatrix(0, 13) = "MemSub2"
      .ColType(13) = ATCoInt
      .TextMatrix(0, 14) = "HIDE"
      For i = 0 To 13
        .ColEditable(i) = True
      Next i
      .rows = 0
      For i = 1 To newConnection.Uci.OpnSeqBlock.Opns.Count
        Set lOper = newConnection.Uci.OpnSeqBlock.Opn(i)
        For j = 1 To lOper.Sources.Count   'used to go thru targets, misses range
          Set lConn = lOper.Sources(j)
          If lConn.Typ = 2 Then
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lConn.Source.VolName
            .TextMatrix(.rows, 1) = lConn.Source.VolId
            .TextMatrix(.rows, 2) = lConn.Source.Group
            .TextMatrix(.rows, 3) = lConn.Source.Member
            .TextMatrix(.rows, 4) = lConn.Source.MemSub1
            .TextMatrix(.rows, 5) = lConn.Source.MemSub2
            .TextMatrix(.rows, 6) = lConn.MFact
            .TextMatrix(.rows, 7) = lConn.Tran
            .TextMatrix(.rows, 8) = lOper.Name
            .TextMatrix(.rows, 9) = lOper.Id
            .TextMatrix(.rows, 10) = lConn.Target.Group
            .TextMatrix(.rows, 11) = lConn.Target.Member
            .TextMatrix(.rows, 12) = lConn.Target.MemSub1
            .TextMatrix(.rows, 13) = lConn.Target.MemSub2
            .TextMatrix(.rows, 14) = lConn.Comment
          End If
        Next j
      Next i
    ElseIf tablename = "SCHEMATIC" Then
      .cols = 9
      .TextMatrix(0, 0) = "VolName"
      .ColType(0) = ATCoTxt
      .TextMatrix(0, 1) = "VolId"
      .ColType(1) = ATCoInt
      .TextMatrix(0, 2) = "AreaFact"
      .ColType(2) = ATCoSng
      .TextMatrix(0, 3) = "VolName"
      .ColType(3) = ATCoTxt
      .TextMatrix(0, 4) = "VolId"
      .ColType(4) = ATCoInt
      .TextMatrix(0, 5) = "MLId"
      .ColType(5) = ATCoInt
      .TextMatrix(0, 6) = "Sub1"
      .ColType(6) = ATCoInt
      .TextMatrix(0, 7) = "Sub2"
      .ColType(7) = ATCoInt
      .TextMatrix(0, 8) = "HIDE"
      For i = 0 To 7
        .ColEditable(i) = True
      Next i
      .rows = 0
      For i = 1 To newConnection.Uci.OpnSeqBlock.Opns.Count
        Set lOper = newConnection.Uci.OpnSeqBlock.Opn(i)
        For j = 1 To lOper.Sources.Count
          Set lConn = lOper.Sources(j)
          If lConn.Typ = 3 Then
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lConn.Source.VolName
            .TextMatrix(.rows, 1) = lConn.Source.VolId
            .TextMatrix(.rows, 2) = lConn.MFact
            .TextMatrix(.rows, 3) = lConn.Target.VolName
            .TextMatrix(.rows, 4) = lConn.Target.VolId
            .TextMatrix(.rows, 5) = lConn.MassLink
            .TextMatrix(.rows, 6) = lConn.Target.MemSub1
            .TextMatrix(.rows, 7) = lConn.Target.MemSub2
            .TextMatrix(.rows, 8) = lConn.Comment
          End If
        Next j
      Next i
    ElseIf tablename = "EXT SOURCES" Then
      .cols = 15
      .TextMatrix(0, 0) = "VolName"
      .ColType(0) = ATCoTxt
      .TextMatrix(0, 1) = "VolId"
      .ColType(1) = ATCoInt
      .TextMatrix(0, 2) = "MemName"
      .ColType(2) = ATCoTxt
      .TextMatrix(0, 3) = "QFlag"
      .ColType(3) = ATCoInt
      .TextMatrix(0, 4) = "SSystem"
      .ColType(4) = ATCoTxt
      .TextMatrix(0, 5) = "SgapStr"
      .ColType(5) = ATCoTxt
      .TextMatrix(0, 6) = "MultFact"
      .ColType(6) = ATCoSng
      .TextMatrix(0, 7) = "Tran"
      .ColType(7) = ATCoTxt
      .TextMatrix(0, 8) = "VolName"
      .ColType(8) = ATCoTxt
      .TextMatrix(0, 9) = "VolId"
      .ColType(9) = ATCoInt
      .TextMatrix(0, 10) = "Group"
      .ColType(10) = ATCoTxt
      .TextMatrix(0, 11) = "MemName"
      .ColType(11) = ATCoTxt
      .TextMatrix(0, 12) = "MemSub1"
      .ColType(12) = ATCoInt
      .TextMatrix(0, 13) = "MemSub2"
      .ColType(13) = ATCoInt
      .TextMatrix(0, 14) = "HIDE"
      For i = 0 To 13
        .ColEditable(i) = True
      Next i
      .rows = 0
      For i = 1 To newConnection.Uci.OpnSeqBlock.Opns.Count
        Set lOper = newConnection.Uci.OpnSeqBlock.Opn(i)
        For j = 1 To lOper.Sources.Count
          Set lConn = lOper.Sources(j)
          If lConn.Typ = 1 Then
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lConn.Source.VolName
            .TextMatrix(.rows, 1) = lConn.Source.VolId
            .TextMatrix(.rows, 2) = lConn.Source.Member
            .TextMatrix(.rows, 3) = lConn.Source.MemSub1
            .TextMatrix(.rows, 4) = lConn.Ssystem
            .TextMatrix(.rows, 5) = lConn.Sgapstrg
            .TextMatrix(.rows, 6) = lConn.MFact
            .TextMatrix(.rows, 7) = lConn.Tran
            .TextMatrix(.rows, 8) = lOper.Name
            .TextMatrix(.rows, 9) = lOper.Id
            .TextMatrix(.rows, 10) = lConn.Target.Group
            .TextMatrix(.rows, 11) = lConn.Target.Member
            .TextMatrix(.rows, 12) = lConn.Target.MemSub1
            .TextMatrix(.rows, 13) = lConn.Target.MemSub2
            .TextMatrix(.rows, 14) = lConn.Comment
          End If
        Next j
      Next i
    ElseIf tablename = "EXT TARGETS" Then
      .cols = 16
      .TextMatrix(0, 0) = "VolName"
      .ColType(0) = ATCoTxt
      .TextMatrix(0, 1) = "VolId"
      .ColType(1) = ATCoInt
      .TextMatrix(0, 2) = "Group"
      .ColType(2) = ATCoTxt
      .TextMatrix(0, 3) = "MemName"
      .ColType(3) = ATCoTxt
      .TextMatrix(0, 4) = "MemSub1"
      .ColType(4) = ATCoInt
      .TextMatrix(0, 5) = "MemSub2"
      .ColType(5) = ATCoInt
      .TextMatrix(0, 6) = "MultFact"
      .ColType(6) = ATCoSng
      .TextMatrix(0, 7) = "Tran"
      .ColType(7) = ATCoTxt
      .TextMatrix(0, 8) = "VolName"
      .ColType(8) = ATCoTxt
      .TextMatrix(0, 9) = "VolId"
      .ColType(9) = ATCoInt
      .TextMatrix(0, 10) = "MemName"
      .ColType(10) = ATCoTxt
      .TextMatrix(0, 11) = "Qflag"
      .ColType(11) = ATCoInt
      .TextMatrix(0, 12) = "TSystem"
      .ColType(12) = ATCoTxt
      .TextMatrix(0, 13) = "AggrStr"
      .ColType(13) = ATCoTxt
      .TextMatrix(0, 14) = "AmdStr"
      .ColType(14) = ATCoTxt
      .TextMatrix(0, 15) = "HIDE"
      For i = 0 To 14
        .ColEditable(i) = True
      Next i
      .rows = 0
      For i = 1 To newConnection.Uci.OpnSeqBlock.Opns.Count
        Set lOper = newConnection.Uci.OpnSeqBlock.Opn(i)
        For j = 1 To lOper.Targets.Count
          Set lConn = lOper.Targets(j)
          If lConn.Typ = 4 Then
            .rows = .rows + 1
            .TextMatrix(.rows, 0) = lConn.Source.VolName
            .TextMatrix(.rows, 1) = lConn.Source.VolId
            .TextMatrix(.rows, 2) = lConn.Source.Group
            .TextMatrix(.rows, 3) = lConn.Source.Member
            .TextMatrix(.rows, 4) = lConn.Source.MemSub1
            .TextMatrix(.rows, 5) = lConn.Source.MemSub2
            .TextMatrix(.rows, 6) = lConn.MFact
            .TextMatrix(.rows, 7) = lConn.Tran
            .TextMatrix(.rows, 8) = lConn.Target.VolName
            .TextMatrix(.rows, 9) = lConn.Target.VolId
            .TextMatrix(.rows, 10) = lConn.Target.Member
            .TextMatrix(.rows, 11) = lConn.Target.MemSub1
            .TextMatrix(.rows, 12) = lConn.Ssystem
            .TextMatrix(.rows, 13) = lConn.Sgapstrg
            .TextMatrix(.rows, 14) = lConn.Amdstrg
            .TextMatrix(.rows, 15) = lConn.Comment
          End If
        Next j
      Next i
    End If
    .ColsSizeByContents
    .Selected(.rows, 1) = False
    SetTableType tablename
  End With

End Property

Private Sub agdConn_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Call DoLimits
  RaiseEvent Change
End Sub

Private Sub agdConn_RowColChange()
  Dim lBlockDef As HspfBlockDef, icol&
  
  Call DoLimits
  If Len(tabname) > 0 Then
    Set lBlockDef = Me.Owner.Uci.Msg.BlockDefs(tabname)
    icol = agdConn.col + 1
    If tabname = "EXT SOURCES" Then
      If agdConn.col < 10 Then
        icol = agdConn.col + 1
      Else
        icol = agdConn.col + 2
      End If
    ElseIf tabname = "EXT TARGETS" Then
      icol = agdConn.col + 1
    ElseIf tabname = "NETWORK" Then
      If agdConn.col < 10 Then
        icol = agdConn.col + 1
      Else
        icol = agdConn.col + 2
      End If
    ElseIf tabname = "SCHEMATIC" Then
      icol = agdConn.col + 1
    End If
    If icol <= lBlockDef.TableDefs(1).ParmDefs.Count Then
      txtDefine = lBlockDef.TableDefs(1).ParmDefs(icol).Name & ": " & _
                  lBlockDef.TableDefs(1).ParmDefs(icol).Define
    End If
  End If
End Sub
  
Private Sub DoLimits()
  
  If tabname = "EXT SOURCES" Then
    Call CheckLimitsExtSources(agdConn, Me.Owner.Uci)
  ElseIf tabname = "EXT TARGETS" Then
    Call CheckLimitsExtTargets(agdConn, Me.Owner.Uci)
  ElseIf tabname = "NETWORK" Then
    Call CheckLimitsNetwork(agdConn, Me.Owner.Uci)
  ElseIf tabname = "SCHEMATIC" Then
    Call CheckLimitsSchematic(agdConn, Me.Owner.Uci)
  End If
  'RaiseEvent Change
  
End Sub

Private Sub agdConn_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits
End Sub

Private Sub UserControl_Initialize()
  With agdConn
    .rows = 0
    .cols = 3
    .ColTitle(0) = "Source"
    .ColTitle(1) = "Member"
    .ColTitle(2) = "Target"
  End With
End Sub

Public Sub SetTableType(tablename$)
  tabname = tablename
End Sub

Public Sub Save()
  Dim ltable As HspfTable
  Dim lOper As HspfOperation
  Dim lConn As HspfConnection, vConn As Variant
  Dim i&, s$, j&, rows&, retcod&
  Dim tablename$, complete As Boolean
  Dim lOpn As HspfOperation, vOpn As Variant
  Dim NewSrcTars As New Collection 'of hspfsrctar

  tablename = pConnection.DesiredRecordType
  With agdConn
    If tablename = "NETWORK" Then
      'put network block back
      RemoveSourcesFromOperations
      RemoveTargetsFromOperations
      pConnection.Uci.RemoveConnectionsFromCollection 2
      'create new connections
      For i = 1 To agdConn.rows
        complete = True
        For j = 1 To .cols - 1
          If Len(.TextMatrix(i, j - 1)) < 1 And j <> 8 And j <> 4 And j <> 12 Then
            'no data entered for this field, dont do this row
            complete = False
          End If
        Next j
        If Not complete Then
          pFrm.Hide
          myMsgBox.Show "Some required fields on row " & i & " are empty." & vbCrLf & _
                        "This row will be ignored.", _
                        pConnection.Caption & " Edit Problem", _
                        "-+&Ok"
          pFrm.Show
        Else
          Set lConn = New HspfConnection
          Set lConn.Uci = pConnection.Uci
          lConn.Typ = 2
          lConn.Source.VolName = .TextMatrix(i, 0)
          lConn.Source.VolId = .TextMatrix(i, 1)
          lConn.Source.Group = .TextMatrix(i, 2)
          lConn.Source.Member = .TextMatrix(i, 3)
          lConn.Source.MemSub1 = .TextMatrix(i, 4)
          lConn.Source.MemSub2 = .TextMatrix(i, 5)
          lConn.MFact = .TextMatrix(i, 6)
          lConn.Tran = .TextMatrix(i, 7)
          lConn.Target.VolName = .TextMatrix(i, 8)
          lConn.Target.VolId = .TextMatrix(i, 9)
          lConn.Target.Group = .TextMatrix(i, 10)
          lConn.Target.Member = .TextMatrix(i, 11)
          lConn.Target.MemSub1 = .TextMatrix(i, 12)
          lConn.Target.MemSub2 = .TextMatrix(i, 13)
          lConn.Comment = .TextMatrix(i, 14)
          pConnection.Uci.Connections.Add lConn
        End If
      Next i
      'set timser connections
      For Each vOpn In pConnection.Uci.OpnSeqBlock.Opns
        Set lOpn = vOpn
        lOpn.setTimSerConnections
      Next vOpn
      pConnection.Uci.Edited = True
    ElseIf tablename = "SCHEMATIC" Then
      'put schematic block back
      RemoveSourcesFromOperations
      RemoveTargetsFromOperations
      pConnection.Uci.RemoveConnectionsFromCollection 3
      'create new connections
      For i = 1 To agdConn.rows
        complete = True
        For j = 1 To .cols - 1
          If Len(.TextMatrix(i, j - 1)) < 1 Then
            'no data entered for this field, dont do this row
            complete = False
          End If
        Next j
        If Not complete Then
          pFrm.Hide
          myMsgBox.Show "Some required fields on row " & i & " are empty." & vbCrLf & _
                        "This row will be ignored.", _
                        pConnection.Caption & " Edit Problem", _
                        "-+&Ok"
          pFrm.Show
        Else
          Set lConn = New HspfConnection
          Set lConn.Uci = pConnection.Uci
          lConn.Typ = 3
          lConn.Source.VolName = .TextMatrix(i, 0)
          lConn.Source.VolId = .TextMatrix(i, 1)
          lConn.MFact = .TextMatrix(i, 2)
          lConn.Target.VolName = .TextMatrix(i, 3)
          lConn.Target.VolId = .TextMatrix(i, 4)
          lConn.MassLink = .TextMatrix(i, 5)
          lConn.Target.MemSub1 = .TextMatrix(i, 6)
          lConn.Target.MemSub2 = .TextMatrix(i, 7)
          lConn.Comment = .TextMatrix(i, 8)
        End If
        pConnection.Uci.Connections.Add lConn
      Next i
      'set timser connections
      For Each vOpn In pConnection.Uci.OpnSeqBlock.Opns
        Set lOpn = vOpn
        lOpn.setTimSerConnections
      Next vOpn
      pConnection.Uci.Edited = True
    ElseIf tablename = "EXT SOURCES" Then
      'put external sources back
      RemoveSourcesFromOperations
      pConnection.Uci.RemoveConnectionsFromCollection 1
      'create new connections
      For i = 1 To agdConn.rows
        complete = True
        For j = 1 To .cols - 1
          If Len(.TextMatrix(i, j - 1)) < 1 And j <> 6 And j <> 8 Then
            'no data entered for this field, dont do this row
            complete = False
          End If
        Next j
        If Not complete Then
          pFrm.Hide
          myMsgBox.Show "Some required fields on row " & i & " are empty." & vbCrLf & _
                        "This row will be ignored.", _
                        pConnection.Caption & " Edit Problem", _
                        "-+&Ok"
          pFrm.Show
        Else
          Set lConn = New HspfConnection
          Set lConn.Uci = pConnection.Uci
          lConn.Typ = 1
          lConn.Source.VolName = .TextMatrix(i, 0)
          lConn.Source.VolId = .TextMatrix(i, 1)
          lConn.Source.Member = .TextMatrix(i, 2)
          lConn.Source.MemSub1 = .TextMatrix(i, 3)
          lConn.Ssystem = .TextMatrix(i, 4)
          lConn.Sgapstrg = .TextMatrix(i, 5)
          lConn.MFact = .TextMatrix(i, 6)
          lConn.Tran = .TextMatrix(i, 7)
          lConn.Target.VolName = .TextMatrix(i, 8)
          lConn.Target.VolId = .TextMatrix(i, 9)
          lConn.Target.Group = .TextMatrix(i, 10)
          lConn.Target.Member = .TextMatrix(i, 11)
          lConn.Target.MemSub1 = .TextMatrix(i, 12)
          lConn.Target.MemSub2 = .TextMatrix(i, 13)
          lConn.Comment = .TextMatrix(i, 14)
          pConnection.Uci.Connections.Add lConn
        End If
      Next i
      'set timser connections
      For Each vOpn In pConnection.Uci.OpnSeqBlock.Opns
        Set lOpn = vOpn
        lOpn.setTimSerConnectionsSources
      Next vOpn
      pConnection.Uci.Edited = True
    ElseIf tablename = "EXT TARGETS" Then
      'put ext targets back
      Call CheckDataSetExistance(agdConn, Me.Owner.Uci, retcod)
      If retcod < 1 Then
        'data sets have all been created, go ahead and refresh
        RemoveTargetsFromOperations
        pConnection.Uci.RemoveConnectionsFromCollection 4
        'create new connections
        For i = 1 To agdConn.rows
          complete = True
          For j = 1 To .cols - 1
            If Len(.TextMatrix(i, j - 1)) < 1 And j <> 8 And j <> 14 Then
              'no data entered for this field, dont do this row
              complete = False
            End If
          Next j
          If Not complete Then
            pFrm.Hide
            myMsgBox.Show "Some required fields on row " & i & " are empty." & vbCrLf & _
                          "This row will be ignored.", _
                          pConnection.Caption & " Edit Problem", _
                          "-+&Ok"
            pFrm.Show
          Else
            Set lConn = New HspfConnection
            Set lConn.Uci = pConnection.Uci
            lConn.Typ = 4
            lConn.Source.VolName = .TextMatrix(i, 0)
            lConn.Source.VolId = .TextMatrix(i, 1)
            lConn.Source.Group = .TextMatrix(i, 2)
            lConn.Source.Member = .TextMatrix(i, 3)
            lConn.Source.MemSub1 = .TextMatrix(i, 4)
            lConn.Source.MemSub2 = .TextMatrix(i, 5)
            lConn.MFact = .TextMatrix(i, 6)
            lConn.Tran = .TextMatrix(i, 7)
            lConn.Target.VolName = .TextMatrix(i, 8)
            lConn.Target.VolId = .TextMatrix(i, 9)
            lConn.Target.Member = .TextMatrix(i, 10)
            If Len(.TextMatrix(i, 11)) > 0 Then
              lConn.Target.MemSub1 = .TextMatrix(i, 11)
            End If
            lConn.Ssystem = .TextMatrix(i, 12)
            lConn.Sgapstrg = .TextMatrix(i, 13)
            lConn.Amdstrg = .TextMatrix(i, 14)
            lConn.Comment = .TextMatrix(i, 15)
            pConnection.Uci.Connections.Add lConn
          End If
        Next i
        'set timser connections
        For Each vOpn In pConnection.Uci.OpnSeqBlock.Opns
          Set lOpn = vOpn
          lOpn.setTimSerConnectionsTargets
        Next vOpn
        pConnection.Uci.Edited = True
      End If
    End If
  End With
  
End Sub

Public Sub Add()
  agdConn.rows = agdConn.rows + 1
  RaiseEvent Change
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Remove()
  Dim i&, j&
  For i = 1 To agdConn.rows
    For j = 0 To agdConn.cols - 1
      If agdConn.Selected(i, j) Then
        'remove selected rows
        agdConn.DeleteRow i
      End If
    Next j
  Next i
  RaiseEvent Change
End Sub

Private Sub UserControl_Resize()
  If Height > agdConn.Top + txtDefine.Height Then
    txtDefine.Width = Width
    agdConn.Width = Width
    txtDefine.Top = Height - txtDefine.Height
    agdConn.Height = Height - agdConn.Top - txtDefine.Height
  End If
End Sub

Private Sub RemoveSourcesFromOperations()
  Dim i&, lOper As HspfOperation
  
  For i = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count
    'remove sources from opns
    Set lOper = pConnection.Uci.OpnSeqBlock.Opn(i)
    Do While lOper.Sources.Count > 0
      lOper.Sources.Remove 1
    Loop
  Next i
End Sub

Private Sub RemoveTargetsFromOperations()
  Dim i&, lOper As HspfOperation
  
  For i = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count
    'remove targets from opns
    Set lOper = pConnection.Uci.OpnSeqBlock.Opn(i)
    Do While lOper.Targets.Count > 0
      lOper.Targets.Remove 1
    Loop
  Next i
End Sub
