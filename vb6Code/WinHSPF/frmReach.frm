VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.Form frmReach 
   Caption         =   "WinHSPF - Reach Editor"
   ClientHeight    =   3228
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   8628
   HelpContextID   =   35
   Icon            =   "frmReach.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3228
   ScaleWidth      =   8628
   StartUpPosition =   2  'CenterScreen
   Begin VB.Frame fraReach 
      BorderStyle     =   0  'None
      Height          =   3015
      Left            =   0
      TabIndex        =   0
      Top             =   120
      Width           =   8535
      Begin VB.CommandButton cmdReach 
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
         Left            =   2760
         TabIndex        =   3
         Top             =   2520
         Width           =   1335
      End
      Begin VB.CommandButton cmdReach 
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
         Left            =   4440
         TabIndex        =   2
         Top             =   2520
         Width           =   1335
      End
      Begin VB.CommandButton cmdFtables 
         Caption         =   "&FTables"
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
         Left            =   6960
         TabIndex        =   1
         Top             =   2520
         Width           =   1335
      End
      Begin ATCoCtl.ATCoGrid agdReach 
         Height          =   2175
         Left            =   240
         TabIndex        =   4
         Top             =   120
         Width           =   8055
         _ExtentX        =   14203
         _ExtentY        =   3831
         SelectionToggle =   -1  'True
         AllowBigSelection=   -1  'True
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
         FixedCols       =   1
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
End
Attribute VB_Name = "frmReach"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Sub cmdReach_Click(Index As Integer)
  Dim lTable As HspfTable, sourcepos&, targetpos&, changecount&
  Dim lOper As HspfOperation, newOper As HspfOperation, OldOper As HspfOperation
  Dim i&, oldDownId&, j, newDownId&, tOper As HspfOperation
  Dim vConn As Variant, lConn As HspfConnection, tempConn As HspfConnection
  Dim errorfg As Boolean, switched As Boolean, k&, changednetwork As Boolean
  
  errorfg = False
  changednetwork = False
  If Index = 0 Then
    'okay
    Set lTable = myUci.OpnBlks("RCHRES").Ids(1).tables("GEN-INFO")
    With agdReach
      For i = 1 To .rows
        Set lOper = myUci.OpnBlks("RCHRES").operfromid(.TextMatrix(i, 0))
        Set lTable = lOper.tables("GEN-INFO")
        lTable.Parms("RCHID") = .TextMatrix(i, 1)
        lOper.Description = .TextMatrix(i, 1)
        lTable.Parms("NEXITS") = .TextMatrix(i, 5)
        lTable.Parms("LKFG") = .TextMatrix(i, 6)
        Set lTable = lOper.tables("HYDR-PARM2")
        lTable.Parms("LEN") = .TextMatrix(i, 2)
        lTable.Parms("DELTH") = .TextMatrix(i, 3)
        If lOper.DownOper("RCHRES") <> .TextMatrix(i, 4) Then
          'changed downstream id
          changednetwork = True
          oldDownId = lOper.DownOper("RCHRES")
          'check to make sure new one is ok
          newDownId = .TextMatrix(i, 4)
          Set tOper = lOper.OpnBlk.operfromid(newDownId)
          If tOper Is Nothing And newDownId <> 0 Then
            'invalid oper id
            errorfg = True
            myMsgBox.Show "RCHRES Operation ID " & newDownId & " is invalid.", "Reach Editor Problem", "OK"
            Exit For
          End If
          If Not errorfg Then
            'remove old connection from uci
            If oldDownId > 0 Then
              For j = 1 To myUci.Connections.Count
                Set lConn = myUci.Connections(j)
                If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                   And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                  myUci.Connections.Remove j
                  Set tempConn = lConn
                  Exit For
                End If
              Next j
            Else
              'no down id, used to go to 0
              Set tempConn = New HspfConnection
              For j = 1 To myUci.Connections.Count
                Set lConn = myUci.Connections(j)
                If lConn.Source.volname = "RCHRES" _
                   And lConn.Target.volname = "RCHRES" Then
                  'make like this one
                  tempConn.Amdstrg = lConn.Amdstrg
                  tempConn.MassLink = lConn.MassLink
                  tempConn.MFact = lConn.MFact
                  tempConn.Sgapstrg = lConn.Sgapstrg
                  tempConn.Ssystem = lConn.Ssystem
                  tempConn.Tran = lConn.Tran
                  tempConn.Typ = lConn.Typ
                  Set tempConn.Uci = lConn.Uci
                  Set tempConn.Source.Opn = lOper
                  tempConn.Source.volid = lOper.Id
                  tempConn.Source.volname = lOper.Name
                  Exit For
                End If
              Next j
            End If
            'remove old connection from source and target ops
            For j = 1 To lOper.targets.Count
              Set lConn = lOper.targets(j)
              If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                 And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                lOper.targets.Remove j
                Exit For
              End If
            Next j
            If oldDownId > 0 Then
              Set OldOper = lOper.OpnBlk.operfromid(oldDownId)
              For j = 1 To OldOper.Sources.Count
                Set lConn = OldOper.Sources(j)
                If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                   And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                  OldOper.Sources.Remove j
                  Exit For
                End If
              Next j
            End If
            'add new connection to uci
            If newDownId > 0 Then
              Set newOper = lOper.OpnBlk.operfromid(newDownId)
              Set tempConn.Target.Opn = newOper
              tempConn.Target.volid = newDownId
              myUci.Connections.Add tempConn
              'add new connection to source and target ops
              lOper.targets.Add tempConn
              newOper.Sources.Add tempConn
            End If
          End If
        End If
      Next i
      If changednetwork Then
        'update opn sequence if necessary
        switched = True
        changecount = 0
        Do Until switched = False
          switched = False
          For j = 1 To myUci.Connections.Count
            Set lConn = myUci.Connections(j)
            If lConn.Source.volname = "RCHRES" _
               And lConn.Target.volname = "RCHRES" Then
              sourcepos = 0
              targetpos = 0
              For k = 1 To myUci.OpnSeqBlock.Opns.Count
                Set tOper = myUci.OpnSeqBlock.Opns(k)
                If tOper.Name = lConn.Source.volname And tOper.Id = lConn.Source.volid Then
                  sourcepos = k
                End If
                If tOper.Name = lConn.Target.volname And tOper.Id = lConn.Target.volid Then
                  targetpos = k
                End If
              Next k
              If sourcepos > 0 And targetpos > 0 And sourcepos > targetpos Then
                'need to switch these 2
                switched = True
                Set tOper = myUci.OpnSeqBlock.Opns(targetpos)
                myUci.OpnSeqBlock.Opns.Remove targetpos
                myUci.OpnSeqBlock.Opns.Add tOper, after:=sourcepos - 1
                Exit For
              End If
            End If
          Next j
          changecount = changecount + 1
          If changecount > 2000 Then
            'must have infinite loop
            myMsgBox.Show "This reach network is not resolving." & vbCrLf & "Check for circular connections.", "Reach Editor Problem", "OK"
            switched = False
          End If
        Loop
      End If
      .ColsSizeByContents
    End With
  End If
  If Not errorfg Then
    Unload Me
  End If
End Sub
Private Sub cmdFtables_Click()
  Dim i As Long
  
  If agdReach.SelCount > 0 Then
    i = agdReach.SelStartRow
  Else 'no selection
    i = 1
  End If
    
  myUci.OpnBlks("RCHRES").nthoper(i).Ftable.Edit
End Sub

Private Sub Form_Load()
  Dim lTable As HspfTable
  Dim lOper As HspfOperation
  Dim lOpnBlk As HspfOpnBlk
  Dim i&, units&

  units = myUci.GlobalBlock.emfg
  
    Set lOpnBlk = myUci.OpnBlks("RCHRES")
    With agdReach
      .rows = lOpnBlk.Count
      .cols = 5
      .TextMatrix(0, 0) = "ID"
      .ColSelectable(0) = True
      
      Set lTable = lOpnBlk.tables("HYDR-PARM2")
      
      .TextMatrix(0, 1) = "Description"
      .ColType(1) = ATCoTxt
      .ColEditable(1) = True
      .ColSelectable(1) = True
      
      .ColType(2) = ATCoSng
      .ColEditable(2) = True
      If units = 1 Then
        .TextMatrix(0, 2) = "Length (mi)"
        .ColMin(2) = lTable.Parms("LEN").Def.Min
        .ColMax(2) = lTable.Parms("LEN").Def.Max
      Else
        .TextMatrix(0, 2) = "Length (km)"
        .ColMin(2) = lTable.Parms("LEN").Def.MetricMin
        .ColMax(2) = lTable.Parms("LEN").Def.MetricMax
      End If
      
      .ColType(3) = ATCoSng
      .ColEditable(3) = True
      If units = 1 Then
        .TextMatrix(0, 3) = "Delta H (ft)"
        .ColMin(3) = lTable.Parms("DELTH").Def.Min
        .ColMax(3) = lTable.Parms("DELTH").Def.Max
      Else
        .TextMatrix(0, 3) = "Delta H (m)"
        .ColMin(3) = lTable.Parms("DELTH").Def.MetricMin
        .ColMax(3) = lTable.Parms("DELTH").Def.MetricMax
      End If
      
      .TextMatrix(0, 4) = "DownstreamID"
      .ColType(4) = ATCoInt
      .ColMin(4) = 0
      .ColMax(4) = 999
      .ColEditable(4) = True
       
      Set lTable = lOpnBlk.tables("GEN-INFO")
       
      .TextMatrix(0, 5) = "N Exits"
      .ColType(5) = ATCoInt
      .ColMin(5) = lTable.Parms("NEXITS").Def.Min
      .ColMax(5) = lTable.Parms("NEXITS").Def.Max
      .ColEditable(5) = True
       
      .TextMatrix(0, 6) = "Lake Flag"
      .ColType(6) = ATCoInt
      .ColMin(6) = lTable.Parms("LKFG").Def.Min
      .ColMax(6) = lTable.Parms("LKFG").Def.Max
      .ColEditable(6) = True
      
      For i = 1 To .rows
        Set lOper = lOpnBlk.nthoper(i)
        .TextMatrix(i, 0) = lOper.Id
        Set lTable = lOper.tables("GEN-INFO")
        .TextMatrix(i, 1) = lTable.Parms("RCHID")
        .TextMatrix(i, 5) = lTable.Parms("NEXITS")
        .TextMatrix(i, 6) = lTable.Parms("LKFG")
        Set lTable = lOper.tables("HYDR-PARM2")
        .TextMatrix(i, 2) = lTable.Parms("LEN")
        .TextMatrix(i, 3) = lTable.Parms("DELTH")
        .TextMatrix(i, 4) = lOper.DownOper("RCHRES")
      Next i
        
      .ColsSizeByContents
      '.Sort 0, True
    End With
End Sub

Private Sub Form_Resize()
  If Not (Me.WindowState = vbMinimized) Then
    If Width < 1500 Then Width = 1500
    If Height < 1500 Then Height = 1500
    fraReach.Width = Width
    fraReach.Height = Height
    agdReach.Width = Width - 600
    cmdReach(0).Left = (Width / 2) - cmdReach(0).Width - 200
    cmdReach(1).Left = (Width / 2) + 200
    cmdReach(0).Top = Height - cmdReach(0).Height - 700
    cmdReach(1).Top = Height - cmdReach(1).Height - 700
    cmdFtables.Left = agdReach.Left + agdReach.Width - cmdFtables.Width
    cmdFtables.Top = cmdReach(0).Top
    agdReach.Height = Height - (3.75 * cmdReach(0).Height)
  End If
End Sub
