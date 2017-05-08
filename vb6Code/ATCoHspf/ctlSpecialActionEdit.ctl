VERSION 5.00
Begin VB.UserControl ctlSpecialActionEdit 
   ClientHeight    =   3120
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   11052
   ScaleHeight     =   3120
   ScaleWidth      =   11052
   Begin VB.CommandButton cmdAgPrac 
      Caption         =   "Pre-defined Practices"
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
      Left            =   8520
      TabIndex        =   9
      Top             =   2640
      Width           =   2412
   End
   Begin TabDlg.SSTab tabSpecial 
      Height          =   2292
      Left            =   120
      TabIndex        =   1
      Top             =   120
      Width           =   10812
      _ExtentX        =   19071
      _ExtentY        =   4043
      _Version        =   393216
      Tabs            =   6
      TabsPerRow      =   6
      TabHeight       =   420
      TabCaption(0)   =   "Records"
      TabPicture(0)   =   "ctlSpecialActionEdit.ctx":0000
      Tab(0).ControlEnabled=   -1  'True
      Tab(0).Control(0)=   "fraRecords"
      Tab(0).Control(0).Enabled=   0   'False
      Tab(0).ControlCount=   1
      TabCaption(1)   =   "Actions"
      TabPicture(1)   =   "ctlSpecialActionEdit.ctx":001C
      Tab(1).ControlEnabled=   0   'False
      Tab(1).Control(0)=   "agdRecords(1)"
      Tab(1).ControlCount=   1
      TabCaption(2)   =   "Distributes"
      TabPicture(2)   =   "ctlSpecialActionEdit.ctx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "agdRecords(2)"
      Tab(2).ControlCount=   1
      TabCaption(3)   =   "User Define Names"
      TabPicture(3)   =   "ctlSpecialActionEdit.ctx":0054
      Tab(3).ControlEnabled=   0   'False
      Tab(3).Control(0)=   "agdRecords(3)"
      Tab(3).ControlCount=   1
      TabCaption(4)   =   "User Define Quans"
      TabPicture(4)   =   "ctlSpecialActionEdit.ctx":0070
      Tab(4).ControlEnabled=   0   'False
      Tab(4).Control(0)=   "agdRecords(4)"
      Tab(4).ControlCount=   1
      TabCaption(5)   =   "Conditions"
      TabPicture(5)   =   "ctlSpecialActionEdit.ctx":008C
      Tab(5).ControlEnabled=   0   'False
      Tab(5).Control(0)=   "agdRecords(5)"
      Tab(5).ControlCount=   1
      Begin VB.Frame fraRecords 
         BorderStyle     =   0  'None
         Height          =   1812
         Left            =   120
         TabIndex        =   2
         Top             =   360
         Width           =   10572
         Begin ATCoCtl.ATCoGrid agdRecords 
            Height          =   1812
            Index           =   0
            Left            =   0
            TabIndex        =   3
            Top             =   0
            Width           =   10572
            _ExtentX        =   18648
            _ExtentY        =   3196
            SelectionToggle =   0   'False
            AllowBigSelection=   0   'False
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
            SelectionMode   =   1
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
      End
      Begin ATCoCtl.ATCoGrid agdRecords 
         Height          =   1815
         Index           =   1
         Left            =   -74880
         TabIndex        =   4
         Top             =   360
         Width           =   10575
         _ExtentX        =   18648
         _ExtentY        =   3196
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
         SelectionMode   =   1
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
      Begin ATCoCtl.ATCoGrid agdRecords 
         Height          =   1815
         Index           =   2
         Left            =   -74880
         TabIndex        =   5
         Top             =   360
         Width           =   10575
         _ExtentX        =   18648
         _ExtentY        =   3196
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
         SelectionMode   =   1
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
      Begin ATCoCtl.ATCoGrid agdRecords 
         Height          =   1815
         Index           =   3
         Left            =   -74880
         TabIndex        =   6
         Top             =   360
         Width           =   10575
         _ExtentX        =   18648
         _ExtentY        =   3196
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
         SelectionMode   =   1
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
      Begin ATCoCtl.ATCoGrid agdRecords 
         Height          =   1815
         Index           =   4
         Left            =   -74880
         TabIndex        =   7
         Top             =   360
         Width           =   10575
         _ExtentX        =   18648
         _ExtentY        =   3196
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
         SelectionMode   =   1
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
      Begin ATCoCtl.ATCoGrid agdRecords 
         Height          =   1815
         Index           =   5
         Left            =   -74880
         TabIndex        =   8
         Top             =   360
         Width           =   10575
         _ExtentX        =   18648
         _ExtentY        =   3196
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
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
         SelectionMode   =   1
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
   End
   Begin VB.Label lblCounts 
      Caption         =   "Label1"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   2520
      Width           =   8292
   End
End
Attribute VB_Name = "ctlSpecialActionEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pSpecialActionBlk As HspfSpecialActionBlk
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get frm() As Form
  Set frm = pFrm
End Property

Public Property Get Owner() As HspfSpecialActionBlk
  Set Owner = pSpecialActionBlk
End Property

Public Property Set Owner(newSpecialActionBlk As HspfSpecialActionBlk)
  Set pSpecialActionBlk = newSpecialActionBlk
  Display
End Property

Private Sub Display()
  DisplayRecords
  DisplayCounts
End Sub

Private Sub DisplayCounts()
  Dim ac&, dc&, unc&, uqc&, cc&, i&
  
  ac = 0
  dc = 0
  unc = 0
  uqc = 0
  cc = 0
  With agdRecords(0)
    For i = 1 To .rows
      Select Case .TextMatrix(i, 0)
        Case "Action": ac = ac + 1
        Case "Distribute": dc = dc + 1
        Case "User Defn Name": unc = unc + 1
        Case "User Defn Quan": uqc = uqc + 1
        Case "Condition": cc = cc + 1
      End Select
    Next i
    lblCounts = "Records: " & (.rows) & _
                ", Actions: " & (ac) & _
                ", Distributes: " & (dc) & _
                ", User Define Names: " & (unc) & _
                ", User Define Quans: " & (uqc) & _
                ", Conditions: " & (cc)
  End With
End Sub

Private Sub DisplayRecords()
  Dim i&, s$
  
  With pSpecialActionBlk.Records
    For i = 1 To .Count
      s = HspfSpecialRecordName(.Item(i).SpecType)
      agdRecords(0).TextMatrix(i, 0) = s
      agdRecords(0).TextMatrix(i, 1) = .Item(i).Text
    Next i
  End With
  agdRecords(0).ColsSizeByContents
  tabSpecial.Tab = 0
  agdRecords(0).SelectionMode = ASfree
  agdRecords(0).ColEditable(0) = True
  agdRecords(0).ColEditable(1) = False
End Sub

Public Sub Add()
  Dim c$
  
  c = "Special Action Add Problem"
    
  With tabSpecial
    If .Tab = 0 Then 'add a record
      With agdRecords(0)
        If .selstartrow > 0 Then
          .InsertRow .selstartrow
          RaiseEvent Change
          .TextMatrix(.selstartrow, 0) = "Comment"
          .TextMatrix(.selstartrow, 1) = ""
          DisplayCounts
        Else
          pFrm.Hide
          myMsgBox.Show "Select a Row to Add after ", _
                        c, "-+&Ok"
          pFrm.Show
        End If
      End With
    Else
      pFrm.Hide
      myMsgBox.Show "Add " & .TabCaption(.Tab) & " records using the 'Records' tab.", _
                    c, "-+&Ok"
      pFrm.Show
    End If
  End With
End Sub

Public Sub AddToEnd(cbuff$, itype&)
  tabSpecial.Tab = 0
  With agdRecords(0)
    If (.rows = 1 And Len(.TextMatrix(1, 0)) > 0) Or .rows > 1 Then
      .InsertRow .rows
    End If
    .TextMatrix(.rows, 0) = HspfSpecialRecordName(itype)
    .TextMatrix(.rows, 1) = cbuff
    RaiseEvent Change
    .ColsSizeByContents
  End With
End Sub

Public Sub AddToBeginning(cbuff$, itype&)
  tabSpecial.Tab = 0
  With agdRecords(0)
    If (.rows = 1 And Len(.TextMatrix(1, 0)) > 0) Or .rows > 1 Then
      .InsertRow 1
    End If
    .TextMatrix(1, 0) = HspfSpecialRecordName(itype)
    .TextMatrix(1, 1) = cbuff
    RaiseEvent Change
    .ColsSizeByContents
  End With
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Remove()
  Dim c$
  
  c = "Special Action Remove Problem"
  
  With tabSpecial
    If .Tab = 0 Then 'remove a record
      With agdRecords(0)
        If .selstartrow > 0 Then
          .DeleteRow .selstartrow
          RaiseEvent Change
        Else
          pFrm.Hide
          myMsgBox.Show "No Special Action available to Remove", _
                         c, "-+&Ok"
          pFrm.Show
        End If
      End With
    Else
      pFrm.Hide
      myMsgBox.Show "Remove " & .TabCaption(.Tab) & " records using the 'Records' tab.", _
                    c, "-+&Ok"
      pFrm.Show
    End If
  End With
End Sub

Private Sub agdRecords_CommitChange(Index As Integer, ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits Index
  RaiseEvent Change
End Sub

Private Sub agdRecords_RowColChange(Index As Integer)
  DoLimits Index
End Sub

Private Sub DoLimits(Index As Integer)
  If Index = 0 Then
    If agdRecords(0).col = 0 Then
      agdRecords(Index).ClearValues
      agdRecords(Index).addvalue "Comment"
      agdRecords(Index).addvalue "Action"
      agdRecords(Index).addvalue "Distribute"
      agdRecords(Index).addvalue "User Defn Name"
      agdRecords(Index).addvalue "User Defn Quan"
      agdRecords(Index).addvalue "Condition"
    End If
  Else
    Call CheckLimitsSpecialActions(Index, agdRecords(Index), Me.Owner.Uci)
  End If
End Sub

Private Sub cmdAgPrac_Click()
  frmAgPrac.init Me.Owner.Uci, Me
  frmAgPrac.Show vbModal
End Sub

Private Sub tabSpecial_Click(PreviousTab As Integer)
  Dim i, newText$
  
  If tabSpecial.Tab <> PreviousTab And PreviousTab <> 0 Then
    'changed tab, put previous tab recs back to first tab
    PutRecsToFrontTab PreviousTab
  End If
  
  'now load records for this tab
  frm.MousePointer = vbHourglass
  If tabSpecial.Tab = 1 Then
    'action type records
    With agdRecords(1)
      .cols = 21
      .rows = 1
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Action" Then
          .rows = .rows + 1
          newText = Mid(agdRecords(0).TextMatrix(i, 1), 3)
          .TextMatrix(.rows - 1, 0) = Trim(Left(newText, 6))
          .TextMatrix(.rows - 1, 1) = Mid(newText, 7, 3)
          If Mid(newText, 10, 4) = "    " Then
            .TextMatrix(.rows - 1, 2) = 0
          Else
            .TextMatrix(.rows - 1, 2) = Mid(newText, 10, 4)
          End If
          .TextMatrix(.rows - 1, 3) = Mid(newText, 14, 2)
          If Mid(newText, 16, 3) = "   " Then
            .TextMatrix(.rows - 1, 4) = 0
          Else
            .TextMatrix(.rows - 1, 4) = Mid(newText, 16, 3)
          End If
          If Mid(newText, 19, 4) = "    " Then
            .TextMatrix(.rows - 1, 5) = 0
          Else
            .TextMatrix(.rows - 1, 5) = Mid(newText, 19, 4)
          End If
          If Mid(newText, 23, 3) = "   " Then
            .TextMatrix(.rows - 1, 6) = 0
          Else
            .TextMatrix(.rows - 1, 6) = Mid(newText, 23, 3)
          End If
          If Mid(newText, 26, 3) = "   " Then
            .TextMatrix(.rows - 1, 7) = 0
          Else
            .TextMatrix(.rows - 1, 7) = Mid(newText, 26, 3)
          End If
          If Mid(newText, 29, 3) = "   " Then
            .TextMatrix(.rows - 1, 8) = 0
          Else
            .TextMatrix(.rows - 1, 8) = Mid(newText, 29, 3)
          End If
          If Mid(newText, 32, 3) = "   " Then
            .TextMatrix(.rows - 1, 9) = 0
          Else
            .TextMatrix(.rows - 1, 9) = Mid(newText, 32, 3)
          End If
          If Mid(newText, 35, 2) = "  " Then
            .TextMatrix(.rows - 1, 10) = 0
          Else
            .TextMatrix(.rows - 1, 10) = Mid(newText, 35, 2)
          End If
          .TextMatrix(.rows - 1, 11) = Mid(newText, 37, 2)
          'determine if vname or addr
          If IsNumeric(Mid(newText, 41, 8)) Then
            .TextMatrix(.rows - 1, 12) = Mid(newText, 41, 8) 'addr
            .TextMatrix(.rows - 1, 13) = ""
            .TextMatrix(.rows - 1, 14) = ""
            .TextMatrix(.rows - 1, 15) = ""
          Else
            .TextMatrix(.rows - 1, 12) = Mid(newText, 41, 6)
            .TextMatrix(.rows - 1, 13) = Mid(newText, 47, 3)
            .TextMatrix(.rows - 1, 14) = Mid(newText, 50, 3)
            .TextMatrix(.rows - 1, 15) = Mid(newText, 53, 3)
          End If
          .TextMatrix(.rows - 1, 16) = Mid(newText, 56, 3)
          'determine if value or uvquan
          If IsNumeric(Mid(newText, 59, 10)) Then
            .TextMatrix(.rows - 1, 17) = Trim(Mid(newText, 59, 10)) 'value
          Else
            .TextMatrix(.rows - 1, 17) = Mid(newText, 63, 6) 'quan
          End If
          .TextMatrix(.rows - 1, 18) = Mid(newText, 70, 2)
          If Len(Trim(Mid(newText, 73, 3))) = 0 Or Len(newText) < 73 Then
            .TextMatrix(.rows - 1, 19) = 0
          Else
            .TextMatrix(.rows - 1, 19) = Mid(newText, 73, 3)
          End If
          If Mid(newText, 76, 3) = "   " Or Len(newText) < 76 Then
            .TextMatrix(.rows - 1, 20) = 0
          Else
            .TextMatrix(.rows - 1, 20) = Mid(newText, 76, 3)
          End If
        End If
      Next i
      If .rows > 1 Then
        .rows = .rows - 1
      End If
      .ColsSizeByContents
      .SelectionMode = ASfree
    End With
  ElseIf tabSpecial.Tab = 2 Then
    'distributes
    With agdRecords(2)
      .cols = 15
      .rows = 1
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Distribute" Then
          .rows = .rows + 1
          newText = Mid(agdRecords(0).TextMatrix(i, 1), 3)
          .TextMatrix(.rows - 1, 0) = Mid(newText, 7, 3)
          .TextMatrix(.rows - 1, 1) = Mid(newText, 11, 3)
          .TextMatrix(.rows - 1, 2) = Mid(newText, 15, 2)
          .TextMatrix(.rows - 1, 3) = Mid(newText, 18, 3)
          .TextMatrix(.rows - 1, 4) = Mid(newText, 22, 5)
          .TextMatrix(.rows - 1, 5) = Mid(newText, 29, 5)
          .TextMatrix(.rows - 1, 6) = Mid(newText, 34, 5)
          .TextMatrix(.rows - 1, 7) = Mid(newText, 39, 5)
          .TextMatrix(.rows - 1, 8) = Mid(newText, 44, 5)
          .TextMatrix(.rows - 1, 9) = Mid(newText, 49, 5)
          .TextMatrix(.rows - 1, 10) = Mid(newText, 54, 5)
          .TextMatrix(.rows - 1, 11) = Mid(newText, 59, 5)
          .TextMatrix(.rows - 1, 12) = Mid(newText, 64, 5)
          .TextMatrix(.rows - 1, 13) = Mid(newText, 69, 5)
          .TextMatrix(.rows - 1, 14) = Mid(newText, 74, 5)
        End If
      Next i
      If .rows > 1 Then
        .rows = .rows - 1
      End If
      .ColsSizeByContents
      .SelectionMode = ASfree
    End With
  ElseIf tabSpecial.Tab = 3 Then
    'uvname
    With agdRecords(3)
      .cols = 14
      .rows = 1
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "User Defn Name" Then
          .rows = .rows + 1
          newText = Mid(agdRecords(0).TextMatrix(i, 1), 3)
          .TextMatrix(.rows - 1, 0) = Mid(newText, 9, 6)
          .TextMatrix(.rows - 1, 1) = Mid(newText, 15, 3)
          .TextMatrix(.rows - 1, 2) = Mid(newText, 19, 6)
          .TextMatrix(.rows - 1, 3) = Mid(newText, 25, 3)
          .TextMatrix(.rows - 1, 4) = Mid(newText, 28, 3)
          .TextMatrix(.rows - 1, 5) = Mid(newText, 31, 3)
          .TextMatrix(.rows - 1, 6) = Mid(newText, 35, 5)
          .TextMatrix(.rows - 1, 7) = Mid(newText, 41, 4)
          .TextMatrix(.rows - 1, 8) = Mid(newText, 49, 6)
          .TextMatrix(.rows - 1, 9) = Mid(newText, 55, 3)
          .TextMatrix(.rows - 1, 10) = Mid(newText, 58, 3)
          .TextMatrix(.rows - 1, 11) = Mid(newText, 61, 3)
          If Mid(newText, 65, 5) = "     " Then
            .TextMatrix(.rows - 1, 12) = 1
          Else
            .TextMatrix(.rows - 1, 12) = Mid(newText, 65, 5)
          End If
          .TextMatrix(.rows - 1, 13) = Mid(newText, 71, 4)
        End If
      Next i
      If .rows > 1 Then
        .rows = .rows - 1
      End If
      .ColsSizeByContents
      .SelectionMode = ASfree
    End With
  ElseIf tabSpecial.Tab = 4 Then
    'User Defn Quan
    With agdRecords(4)
      .cols = 14
      .rows = 1
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "User Defn Quan" Then
          .rows = .rows + 1
          newText = Mid(agdRecords(0).TextMatrix(i, 1), 3)
          .TextMatrix(.rows - 1, 0) = Mid(newText, 8, 6)
          .TextMatrix(.rows - 1, 1) = Mid(newText, 15, 6)
          .TextMatrix(.rows - 1, 2) = Mid(newText, 22, 3)
          .TextMatrix(.rows - 1, 3) = Mid(newText, 26, 6)
          .TextMatrix(.rows - 1, 4) = Mid(newText, 33, 3)
          .TextMatrix(.rows - 1, 5) = Mid(newText, 36, 3)
          .TextMatrix(.rows - 1, 6) = Mid(newText, 39, 3)
          .TextMatrix(.rows - 1, 7) = Mid(newText, 41, 3)
          If Mid(newText, 44, 10) = "          " Then
            .TextMatrix(.rows - 1, 8) = 1#
          Else
            .TextMatrix(.rows - 1, 8) = Mid(newText, 44, 10)
          End If
          .TextMatrix(.rows - 1, 9) = Mid(newText, 55, 2)
          If Mid(newText, 57, 3) = "   " Then
            .TextMatrix(.rows - 1, 10) = 1
          Else
            .TextMatrix(.rows - 1, 10) = Mid(newText, 57, 3)
          End If
          .TextMatrix(.rows - 1, 11) = Mid(newText, 61, 2)
          If Mid(newText, 63, 3) = "   " Then
            .TextMatrix(.rows - 1, 12) = 1
          Else
            .TextMatrix(.rows - 1, 12) = Mid(newText, 63, 3)
          End If
          .TextMatrix(.rows - 1, 13) = Mid(newText, 67, 4)
        End If
      Next i
      If .rows > 1 Then
        .rows = .rows - 1
      End If
      .ColsSizeByContents
      .SelectionMode = ASfree
    End With
  ElseIf tabSpecial.Tab = 5 Then
    'conditionals
    With agdRecords(5)
      .cols = 1
      .rows = 1
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Condition" Then
          .rows = .rows + 1
          .TextMatrix(.rows - 1, 0) = agdRecords(0).TextMatrix(i, 1)
        End If
      Next i
      If .rows > 1 Then
        .rows = .rows - 1
      End If
      .ColsSizeByContents
      .SelectionMode = ASfree
    End With
  End If
  frm.MousePointer = vbDefault
End Sub

Private Sub PutRecsToFrontTab(itab As Integer)
  Dim rowcount&, newText$, i&, ctemp$, j&
  
  If itab = 1 Then
    'action type records
    rowcount = 0
    With agdRecords(1)
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Action" Then
          'get next record from this tab
          rowcount = rowcount + 1
          newText = "  "
          newText = BlankPad(newText & .TextMatrix(rowcount, 0), 8)
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 1)
          newText = newText & ctemp
          If Len(.TextMatrix(rowcount, 2)) = 0 Then
            newText = newText & "    "
          ElseIf .TextMatrix(rowcount, 2) = 0 Then
            newText = newText & "    "
          Else
            ctemp = "    "
            RSet ctemp = .TextMatrix(rowcount, 2)
            newText = newText & ctemp
          End If
          newText = BlankPad(newText & .TextMatrix(rowcount, 3), 17)
          If Len(.TextMatrix(rowcount, 4)) = 0 Then
            newText = newText & "   "
          ElseIf .TextMatrix(rowcount, 4) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 4)
            newText = newText & ctemp
          End If
          If Len(.TextMatrix(rowcount, 5)) = 0 Then
            newText = newText & "    "
          ElseIf .TextMatrix(rowcount, 5) = 0 Then
            newText = newText & "    "
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 5), 24)
          End If
          If Len(.TextMatrix(rowcount, 6)) = 0 Then
            newText = newText & "   "
          ElseIf .TextMatrix(rowcount, 6) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 6)
            newText = newText & ctemp
          End If
          If Len(.TextMatrix(rowcount, 7)) = 0 Then
            newText = newText & "   "
          ElseIf .TextMatrix(rowcount, 7) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 7)
            newText = newText & ctemp
          End If
          If Len(.TextMatrix(rowcount, 8)) = 0 Then
            newText = newText & "   "
          ElseIf .TextMatrix(rowcount, 8) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 8)
            newText = newText & ctemp
          End If
          If Len(.TextMatrix(rowcount, 9)) = 0 Then
            newText = newText & "   "
          ElseIf .TextMatrix(rowcount, 9) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 9)
            newText = newText & ctemp
          End If
          If Len(.TextMatrix(rowcount, 10)) = 0 Then
            newText = newText & "  "
          ElseIf .TextMatrix(rowcount, 10) = 0 Then
            newText = newText & "  "
          Else
            ctemp = "  "
            RSet ctemp = .TextMatrix(rowcount, 10)
            newText = newText & ctemp
          End If
          ctemp = "  "
          RSet ctemp = .TextMatrix(rowcount, 11)
          newText = newText & ctemp & "  "
          If IsNumeric(.TextMatrix(rowcount, 12)) Then
            newText = BlankPad(newText & .TextMatrix(rowcount, 12), 57) 'addr
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 12), 48) 'vname
            newText = BlankPad(newText & .TextMatrix(rowcount, 13), 51)
            newText = BlankPad(newText & .TextMatrix(rowcount, 14), 54)
            newText = BlankPad(newText & .TextMatrix(rowcount, 15), 57)
          End If
          newText = BlankPad(newText & .TextMatrix(rowcount, 16), 60)
          If IsNumeric(.TextMatrix(rowcount, 17)) Then
            ctemp = "          "
            RSet ctemp = .TextMatrix(rowcount, 17)
            newText = newText & ctemp 'value
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 17), 70) 'quan
          End If
          newText = newText & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 18), 73)
          If .TextMatrix(rowcount, 19) = 0 Then
            newText = newText & "    "
          Else
            ctemp = "    "
            RSet ctemp = .TextMatrix(rowcount, 19)
            newText = newText & ctemp
          End If
          If .TextMatrix(rowcount, 20) = 0 Then
            newText = newText & "   "
          Else
            ctemp = "   "
            RSet ctemp = .TextMatrix(rowcount, 20)
            newText = newText & ctemp
          End If
          agdRecords(0).TextMatrix(i, 1) = newText
        End If
      Next i
    End With
  ElseIf itab = 2 Then
    'distribute records
    rowcount = 0
    With agdRecords(2)
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Distribute" Then
          'get next record from this tab
          rowcount = rowcount + 1
          newText = "  DISTRB"
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 0)
          newText = newText & ctemp
          ctemp = "    "
          RSet ctemp = .TextMatrix(rowcount, 1)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 2), 18)
          ctemp = "    "
          RSet ctemp = .TextMatrix(rowcount, 3)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 4), 30)
          For j = 1 To 10
            ctemp = "     "
            RSet ctemp = .TextMatrix(rowcount, 4 + j)
            newText = newText & ctemp
          Next j
          agdRecords(0).TextMatrix(i, 1) = newText
        End If
      Next i
    End With
  ElseIf itab = 3 Then
    'User Defn Name records
    rowcount = 0
    With agdRecords(3)
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "User Defn Name" Then
          'get next record from this tab
          rowcount = rowcount + 1
          newText = "  UVNAME  "
          newText = BlankPad(newText & .TextMatrix(rowcount, 0), 16)
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 1)
          newText = newText & ctemp & " "
          If IsNumeric(.TextMatrix(rowcount, 2)) Then
            newText = BlankPad(newText & .TextMatrix(rowcount, 2), 35) 'addr
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 2), 26) 'vname
            newText = BlankPad(newText & .TextMatrix(rowcount, 3), 29)
            newText = BlankPad(newText & .TextMatrix(rowcount, 4), 32)
            newText = BlankPad(newText & .TextMatrix(rowcount, 5), 35)
          End If
          newText = newText & " "
          ctemp = "     "
          RSet ctemp = .TextMatrix(rowcount, 6)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 7), 46)
          newText = newText & "    "
          If IsNumeric(.TextMatrix(rowcount, 8)) Then
            newText = BlankPad(newText & .TextMatrix(rowcount, 8), 65) 'addr
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 8), 56) 'vname
            newText = BlankPad(newText & .TextMatrix(rowcount, 9), 59)
            newText = BlankPad(newText & .TextMatrix(rowcount, 10), 62)
            newText = BlankPad(newText & .TextMatrix(rowcount, 11), 65)
          End If
          newText = newText & " "
          ctemp = "     "
          RSet ctemp = .TextMatrix(rowcount, 12)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 13), 76)
          agdRecords(0).TextMatrix(i, 1) = newText
        End If
      Next i
    End With
  ElseIf itab = 4 Then
    'User Defn Quan records
    rowcount = 0
    With agdRecords(4)
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "User Defn Quan" Then
          'get next record from this tab
          rowcount = rowcount + 1
          newText = "  UVQUAN "
          newText = BlankPad(newText & .TextMatrix(rowcount, 0), 16)
          newText = BlankPad(newText & .TextMatrix(rowcount, 1), 23)
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 2)
          newText = newText & ctemp & " "
          If IsNumeric(.TextMatrix(rowcount, 3)) Then
            newText = BlankPad(newText & .TextMatrix(rowcount, 3), 42) 'addr
          Else
            newText = BlankPad(newText & .TextMatrix(rowcount, 3), 33) 'vname
            newText = BlankPad(newText & .TextMatrix(rowcount, 4), 36)
            newText = BlankPad(newText & .TextMatrix(rowcount, 5), 39)
            newText = BlankPad(newText & .TextMatrix(rowcount, 6), 42)
          End If
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 7)
          newText = newText & ctemp
          ctemp = "          "
          RSet ctemp = .TextMatrix(rowcount, 8)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 9), 58)
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 10)
          newText = newText & ctemp & " "
          newText = BlankPad(newText & .TextMatrix(rowcount, 11), 64)
          ctemp = "   "
          RSet ctemp = .TextMatrix(rowcount, 12)
          newText = newText & ctemp & " "
          newText = newText & .TextMatrix(rowcount, 13)
          agdRecords(0).TextMatrix(i, 1) = newText
        End If
      Next i
    End With
  ElseIf itab = 5 Then
    'conditional records
    rowcount = 0
    With agdRecords(5)
      For i = 1 To agdRecords(0).rows
        If agdRecords(0).TextMatrix(i, 0) = "Condition" Then
          'get next record from this tab
          rowcount = rowcount + 1
          agdRecords(0).TextMatrix(i, 1) = .TextMatrix(rowcount, 0)
        End If
      Next i
    End With
  End If
End Sub

Private Function BlankPad(ctxt$, ilen&)
  'pad a string to be the desired length
  Dim i&, j&
  If Len(ctxt) > ilen Then
    BlankPad = Left(ctxt, ilen)
  ElseIf Len(ctxt) < ilen Then
    j = ilen - Len(ctxt)
    BlankPad = ctxt
    For i = 1 To j
      BlankPad = BlankPad & " "
    Next i
  Else
    BlankPad = ctxt
  End If
End Function

Private Sub UserControl_Initialize()
  Dim i&
  
  With agdRecords(0)
    .ColTitle(0) = "Type"
    .ColEditable(0) = False
    .ColType(0) = ATCoTxt
    .ColTitle(1) = "Text"
    .ColEditable(1) = True
    .ColType(1) = ATCoTxt
    .gridFontName = "Courier"
  End With
  'action records
  With agdRecords(1)
    .ColTitle(0) = "OpTyp"
    .ColEditable(0) = True
    .ColType(0) = ATCoTxt
    .ColTitle(1) = "OpFst"
    .ColEditable(1) = True
    .ColType(1) = ATCoInt
    .ColTitle(2) = "OpLst"
    .ColEditable(2) = True
    .ColType(2) = ATCoInt
    .ColTitle(3) = "Dc"
    .ColEditable(3) = True
    .ColType(3) = ATCoTxt
    .ColTitle(4) = "Ds"
    .ColEditable(4) = True
    .ColType(4) = ATCoInt
    .ColTitle(5) = "Yr"
    .ColEditable(5) = True
    .ColType(5) = ATCoInt
    .ColTitle(6) = "Mo"
    .ColEditable(6) = True
    .ColType(6) = ATCoInt
    .ColTitle(7) = "Dy"
    .ColEditable(7) = True
    .ColType(7) = ATCoInt
    .ColTitle(8) = "Hr"
    .ColEditable(8) = True
    .ColType(8) = ATCoInt
    .ColTitle(9) = "Mn"
    .ColEditable(9) = True
    .ColType(9) = ATCoInt
    .ColTitle(10) = "DsInd"
    .ColEditable(10) = True
    .ColType(10) = ATCoInt
    .ColTitle(11) = "Typ"
    .ColEditable(11) = True
    .ColType(11) = ATCoInt
    .ColTitle(12) = "Vname/Addr"
    .ColEditable(12) = True
    .ColType(12) = ATCoTxt
    .ColTitle(13) = "Sub1"
    .ColEditable(13) = True
    .ColType(13) = ATCoTxt
    .ColTitle(14) = "Sub2"
    .ColEditable(14) = True
    .ColType(14) = ATCoTxt
    .ColTitle(15) = "Sub3"
    .ColEditable(15) = True
    .ColType(15) = ATCoTxt
    .ColTitle(16) = "ActCod"
    .ColEditable(16) = True
    .ColType(16) = ATCoTxt
    .ColTitle(17) = "Value/Uvquan"
    .ColEditable(17) = True
    .ColType(17) = ATCoTxt
    .ColTitle(18) = "Tc"
    .ColEditable(18) = True
    .ColType(18) = ATCoTxt
    .ColTitle(19) = "Ts"
    .ColEditable(19) = True
    .ColType(19) = ATCoInt
    .ColTitle(20) = "Num"
    .ColEditable(20) = True
    .ColType(20) = ATCoInt
  End With
  'distributes
  With agdRecords(2)
    .ColTitle(0) = "DSInd"
    .ColEditable(0) = True
    .ColType(0) = ATCoInt
    .ColTitle(1) = "Count"
    .ColEditable(1) = True
    .ColType(1) = ATCoInt
    .ColTitle(2) = "CTCode"
    .ColEditable(2) = True
    .ColType(2) = ATCoTxt
    .ColTitle(3) = "TStep"
    .ColEditable(3) = True
    .ColType(3) = ATCoInt
    .ColTitle(4) = "DefFg"
    .ColEditable(4) = True
    .ColType(4) = ATCoTxt
    .ColTitle(5) = "Frac1"
    .ColEditable(5) = True
    .ColType(5) = ATCoSng
    .ColTitle(6) = "Frac2"
    .ColEditable(6) = True
    .ColType(6) = ATCoSng
    .ColTitle(7) = "Frac3"
    .ColEditable(7) = True
    .ColType(7) = ATCoSng
    .ColTitle(8) = "Frac4"
    .ColEditable(8) = True
    .ColType(8) = ATCoSng
    .ColTitle(9) = "Frac5"
    .ColEditable(9) = True
    .ColType(9) = ATCoSng
    .ColTitle(10) = "Frac6"
    .ColEditable(10) = True
    .ColType(10) = ATCoSng
    .ColTitle(11) = "Frac7"
    .ColEditable(11) = True
    .ColType(11) = ATCoSng
    .ColTitle(12) = "Frac8"
    .ColEditable(12) = True
    .ColType(12) = ATCoSng
    .ColTitle(13) = "Frac9"
    .ColEditable(13) = True
    .ColType(13) = ATCoSng
    .ColTitle(14) = "Frac10"
    .ColEditable(14) = True
    .ColType(14) = ATCoSng
  End With
  'uvnames
  With agdRecords(3)
    .ColTitle(0) = "UVName"
    .ColEditable(0) = True
    .ColType(0) = ATCoTxt
    .ColTitle(1) = "VCount"
    .ColEditable(1) = True
    .ColType(1) = ATCoInt
    .ColTitle(2) = "VName/Addr"
    .ColEditable(2) = True
    .ColType(2) = ATCoTxt
    .ColTitle(3) = "Sub1"
    .ColEditable(3) = True
    .ColType(3) = ATCoTxt
    .ColTitle(4) = "Sub2"
    .ColEditable(4) = True
    .ColType(4) = ATCoTxt
    .ColTitle(5) = "Sub3"
    .ColEditable(5) = True
    .ColType(5) = ATCoTxt
    .ColTitle(6) = "Frac"
    .ColEditable(6) = True
    .ColType(6) = ATCoSng
    .ColTitle(7) = "ActCd"
    .ColEditable(7) = True
    .ColType(7) = ATCoTxt
    .ColTitle(8) = "VName/Addr"
    .ColEditable(8) = True
    .ColType(8) = ATCoTxt
    .ColTitle(9) = "Sub1"
    .ColEditable(9) = True
    .ColType(9) = ATCoTxt
    .ColTitle(10) = "Sub2"
    .ColEditable(10) = True
    .ColType(10) = ATCoTxt
    .ColTitle(11) = "Sub3"
    .ColEditable(11) = True
    .ColType(11) = ATCoTxt
    .ColTitle(12) = "Frac"
    .ColEditable(12) = True
    .ColType(12) = ATCoSng
    .ColTitle(13) = "ActCd"
    .ColEditable(13) = True
    .ColType(13) = ATCoTxt
  End With
  'User Defn Quan
  With agdRecords(4)
    .ColTitle(0) = "UVQNam"
    .ColEditable(0) = True
    .ColType(0) = ATCoTxt
    .ColTitle(1) = "OpTyp"
    .ColEditable(1) = True
    .ColType(1) = ATCoTxt
    .ColTitle(2) = "OpNo"
    .ColEditable(2) = True
    .ColType(2) = ATCoInt
    .ColTitle(3) = "VName/Addr"
    .ColEditable(3) = True
    .ColType(3) = ATCoTxt
    .ColTitle(4) = "Sub1"
    .ColEditable(4) = True
    .ColType(4) = ATCoTxt
    .ColTitle(5) = "Sub2"
    .ColEditable(5) = True
    .ColType(5) = ATCoTxt
    .ColTitle(6) = "Sub3"
    .ColEditable(6) = True
    .ColType(6) = ATCoTxt
    .ColTitle(7) = "Typ"
    .ColEditable(7) = True
    .ColType(7) = ATCoInt
    .ColTitle(8) = "Mult"
    .ColEditable(8) = True
    .ColType(8) = ATCoSng
    .ColTitle(9) = "LagCode"
    .ColEditable(9) = True
    .ColType(9) = ATCoTxt
    .ColTitle(10) = "LagStep"
    .ColEditable(10) = True
    .ColType(10) = ATCoInt
    .ColTitle(11) = "AgCode"
    .ColEditable(11) = True
    .ColType(11) = ATCoTxt
    .ColTitle(12) = "AgStep"
    .ColEditable(12) = True
    .ColType(12) = ATCoInt
    .ColTitle(13) = "Tran"
    .ColEditable(13) = True
    .ColType(13) = ATCoTxt
  End With
  'Conditionals
  With agdRecords(5)
    .ColTitle(0) = "Text"
    .ColEditable(0) = True
    .ColType(0) = ATCoTxt
    .gridFontName = "Courier"
  End With
End Sub

Private Sub UserControl_Resize()
  Dim i&
  
  If Height > 600 Then
    lblCounts.Top = Height - lblCounts.Height - 20
    cmdAgPrac.Top = Height - lblCounts.Height - 120
    tabSpecial.Height = lblCounts.Top - 300
    tabSpecial.Width = Width - 120
    fraRecords.Height = lblCounts.Top - 760
    fraRecords.Width = Width - 360
    For i = 0 To 5
      agdRecords(i).Height = lblCounts.Top - 760
      agdRecords(i).Width = Width - 360
      agdRecords(i).ColsSizeByContents
    Next i
  End If
End Sub

Public Sub Save()
  Dim mySpecialRecord As HspfSpecialRecord
  Dim i&
  
  PutRecsToFrontTab tabSpecial.Tab
  
  With pSpecialActionBlk.Records
    Do Until .Count = 0
      .Remove 1
    Loop
    For i = 1 To agdRecords(0).rows
      Set mySpecialRecord = New HspfSpecialRecord
      mySpecialRecord.Text = agdRecords(0).TextMatrix(i, 1)
      If agdRecords(0).TextMatrix(i, 0) = "Comment" Then
        mySpecialRecord.SpecType = hComment
      ElseIf agdRecords(0).TextMatrix(i, 0) = "Condition" Then
        mySpecialRecord.SpecType = hCondition
      ElseIf agdRecords(0).TextMatrix(i, 0) = "Distribute" Then
        mySpecialRecord.SpecType = hDistribute
      ElseIf agdRecords(0).TextMatrix(i, 0) = "User Defn Name" Then
        mySpecialRecord.SpecType = hUserDefineName
      ElseIf agdRecords(0).TextMatrix(i, 0) = "User Defn Quan" Then
        mySpecialRecord.SpecType = hUserDefineQuan
      Else
        mySpecialRecord.SpecType = hAction
      End If
      pSpecialActionBlk.Records.Add mySpecialRecord
    Next i
  End With
End Sub

Public Function UVNameInUse(Name$) As Boolean
  Dim ctmp$, i&
  
  UVNameInUse = False
  'check front tab
  For i = 1 To agdRecords(0).rows
    If agdRecords(0).TextMatrix(i, 0) = "User Defn Name" Then
      ctmp = agdRecords(0).TextMatrix(i, 1)
      If Trim(Mid(ctmp, 11, 6)) = Name Then
        UVNameInUse = True
      End If
    End If
  Next i
  'check uvname tab as well
  For i = 1 To agdRecords(3).rows
    ctmp = agdRecords(3).TextMatrix(i, 0)
    If Trim(ctmp) = Name Then
      UVNameInUse = True
    End If
  Next i
End Function

Public Function NextDistribNumber() As Long
  Dim ctmp$, i&
  Dim ifound As Boolean
  
  NextDistribNumber = 1
  
  ifound = True
  Do Until ifound = False
    ifound = False
    'check front tab
    For i = 1 To agdRecords(0).rows
      If agdRecords(0).TextMatrix(i, 0) = "Distrib" Then
        ctmp = agdRecords(0).TextMatrix(i, 1)
        If CInt(Mid(ctmp, 9, 3)) = NextDistribNumber Then
          ifound = True
          Exit For
        End If
      End If
    Next i
    If Not ifound Then
      'check distrib tab as well
      For i = 1 To agdRecords(2).rows
        ctmp = agdRecords(2).TextMatrix(i, 0)
        If IsNumeric(ctmp) Then
          If CInt(ctmp) = NextDistribNumber Then
            ifound = True
            Exit For
          End If
        End If
      Next i
    End If
    If ifound Then
      NextDistribNumber = NextDistribNumber + 1
    End If
  Loop
  
End Function

Public Function UVQuanInUse(Name$, Id&) As String
  Dim ctmp$, i&
  
  UVQuanInUse = ""
  'check front tab
  For i = 1 To agdRecords(0).rows
    If agdRecords(0).TextMatrix(i, 0) = "User Defn Quan" Then
      ctmp = agdRecords(0).TextMatrix(i, 1)
      If Trim(Mid(ctmp, 17, 6)) = Name And CInt(Mid(ctmp, 24, 3)) = Id Then
        UVQuanInUse = Mid(ctmp, 10, 6)
      End If
    End If
  Next i
  'check uvquan tab as well
  For i = 1 To agdRecords(4).rows
    ctmp = agdRecords(4).TextMatrix(i, 1)
    If Trim(ctmp) = Name Then
      ctmp = agdRecords(4).TextMatrix(i, 2)
      If IsNumeric(ctmp) Then
        If CInt(ctmp) = Id Then
          UVQuanInUse = Mid(ctmp, 10, 6)
        End If
      End If
    End If
  Next i
End Function

Public Function NextUVQuanName(firstfour$) As String
  Dim ctmp$, i&, nextnumber&
  Dim ifound As Boolean
  
  nextnumber = 1
  
  ifound = True
  Do Until ifound = False
    ifound = False
    NextUVQuanName = firstfour & CStr(nextnumber)
    'check front tab
    For i = 1 To agdRecords(0).rows
      If agdRecords(0).TextMatrix(i, 0) = "User Defn Quan" Then
        ctmp = agdRecords(0).TextMatrix(i, 1)
        If Trim(Mid(ctmp, 10, 6)) = NextUVQuanName Then
          ifound = True
          Exit For
        End If
      End If
    Next i
    If Not ifound Then
      'check user defn quan tab as well
      For i = 1 To agdRecords(4).rows
        ctmp = agdRecords(4).TextMatrix(i, 1)
        If Trim(ctmp) = NextUVQuanName Then
          ifound = True
          Exit For
        End If
      Next i
    End If
    If ifound Then
      nextnumber = nextnumber + 1
    End If
  Loop
  
End Function
