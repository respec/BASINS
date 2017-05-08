VERSION 5.00
Begin VB.UserControl ctlMassLinkEdit 
   ClientHeight    =   3600
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6915
   ScaleHeight     =   3600
   ScaleWidth      =   6915
   Begin VB.TextBox txtDefine 
      BackColor       =   &H80000000&
      CausesValidation=   0   'False
      BeginProperty Font 
         Name            =   "Courier New"
         Size            =   8.25
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
      TabIndex        =   4
      Text            =   "ctlMassLinkEdit.ctx":0000
      Top             =   2640
      Width           =   6855
   End
   Begin VB.CommandButton cmdAddNew 
      Caption         =   "Add New"
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
      Left            =   5280
      TabIndex        =   3
      Top             =   120
      Width           =   1575
   End
   Begin VB.ComboBox cboID 
      Height          =   288
      Left            =   1800
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   120
      Width           =   975
   End
   Begin ATCoCtl.ATCoGrid grdMassLink 
      Height          =   2175
      Left            =   0
      TabIndex        =   0
      Top             =   480
      Width           =   6855
      _ExtentX        =   12091
      _ExtentY        =   3836
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
      BackColorBkg    =   -2147483644
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Label lblId 
      Alignment       =   1  'Right Justify
      Caption         =   "Mass-Link Number:"
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
      Left            =   0
      TabIndex        =   1
      Top             =   120
      Width           =   1692
   End
End
Attribute VB_Name = "ctlMassLinkEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pMassLink As HspfMassLink
Private pEdited As Boolean
Event Change()
Dim prevMLid&
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfMassLink
  Set Owner = pMassLink
End Property

Public Property Set Owner(newMassLink As HspfMassLink)
  Dim i&, k&, found As Boolean
  Dim lMassLink As HspfMassLink, mlcnt, mlno&()
  
  Set pMassLink = newMassLink
  
  'build list of masslinks
  mlcnt = 0
  For i = 1 To pMassLink.Uci.MassLinks.Count
    Set lMassLink = pMassLink.Uci.MassLinks(i)
    found = False
    For k = 0 To mlcnt - 1
      If lMassLink.MassLinkID = mlno(k) Then
        found = True
      End If
    Next k
    If found = False Then
      mlcnt = mlcnt + 1
      ReDim Preserve mlno(mlcnt)
      mlno(mlcnt - 1) = lMassLink.MassLinkID
    End If
  Next i
  
  cboID.Clear
  For i = 1 To mlcnt
    cboID.AddItem mlno(i - 1)
  Next i
  cboID.ListIndex = 0
  prevMLid = cboID
  Refresh
  
End Property

Public Sub Save()
  Dim i&, j&, rows&
  Dim lMassLink As HspfMassLink
  Dim complete As Boolean
  
  'remove mass link records with this id
  i = 1
  Do While i <= pMassLink.Uci.MassLinks.Count
    'remove mass links from collection
    Set lMassLink = pMassLink.Uci.MassLinks(i)
    If lMassLink.MassLinkID = cboID Then
      pMassLink.Uci.MassLinks.Remove i
    Else
      i = i + 1
    End If
  Loop
  
  'put back mass links with this id
  With grdMassLink
    For i = 1 To .rows
      complete = True
      For j = 1 To .cols - 1
        If Len(.TextMatrix(i, j - 1)) < 1 And j <> 3 And j <> 9 Then
          'no data entered for this field, dont do this row
          complete = False
        End If
      Next j
      If Not complete Then
        pFrm.Hide
        myMsgBox.Show "Some required fields on row " & i & " are empty." & vbCrLf & _
                      "This row will be ignored.", _
                      pMassLink.Caption & " Edit Problem", _
                      "-+&Ok"
        pFrm.Show
      Else
        Set lMassLink = New HspfMassLink
        Set lMassLink.Uci = pMassLink.Uci
        lMassLink.MassLinkID = cboID
        lMassLink.Source.VolName = .TextMatrix(i, 0)
        lMassLink.Source.Group = .TextMatrix(i, 1)
        lMassLink.Source.Member = .TextMatrix(i, 2)
        lMassLink.Source.MemSub1 = .TextMatrix(i, 3)
        lMassLink.Source.MemSub2 = .TextMatrix(i, 4)
        lMassLink.MFact = .TextMatrix(i, 5)
        lMassLink.Target.VolName = .TextMatrix(i, 6)
        lMassLink.Target.Group = .TextMatrix(i, 7)
        lMassLink.Target.Member = .TextMatrix(i, 8)
        lMassLink.Target.MemSub1 = .TextMatrix(i, 9)
        lMassLink.Target.MemSub2 = .TextMatrix(i, 10)
        lMassLink.Comment = .TextMatrix(i, 11)
        pMassLink.Uci.MassLinks.Add lMassLink
      End If
    Next i
  End With
  pEdited = False
End Sub

Private Sub Refresh()
  Dim i&, j&, lMassLink As HspfMassLink
  
  prevMLid = cboID
  With grdMassLink
    .rows = 0
    .cols = 12
    For j = 0 To 10
      .ColEditable(j) = True
    Next j
    .ColTitle(0) = "VolName"
    .ColTitle(1) = "Group"
    .ColTitle(2) = "MemName"
    .ColTitle(3) = "MemSub1"
    .ColTitle(4) = "MemSub2"
    .ColTitle(5) = "MultFactor"
    .ColTitle(6) = "VolName"
    .ColTitle(7) = "Group"
    .ColTitle(8) = "MemName"
    .ColTitle(9) = "MemSub1"
    .ColTitle(10) = "MemSub2"
    .ColTitle(11) = "HIDE"
    For i = 1 To pMassLink.Uci.MassLinks.Count
      Set lMassLink = pMassLink.Uci.MassLinks(i)
      If lMassLink.MassLinkID = cboID Then
        .rows = .rows + 1
        .TextMatrix(.rows, 0) = lMassLink.Source.VolName
        .TextMatrix(.rows, 1) = lMassLink.Source.Group
        .TextMatrix(.rows, 2) = lMassLink.Source.Member
        .TextMatrix(.rows, 3) = lMassLink.Source.MemSub1
        .TextMatrix(.rows, 4) = lMassLink.Source.MemSub2
        .TextMatrix(.rows, 5) = lMassLink.MFact
        .TextMatrix(.rows, 6) = lMassLink.Target.VolName
        .TextMatrix(.rows, 7) = lMassLink.Target.Group
        .TextMatrix(.rows, 8) = lMassLink.Target.Member
        .TextMatrix(.rows, 9) = lMassLink.Target.MemSub1
        .TextMatrix(.rows, 10) = lMassLink.Target.MemSub2
        .TextMatrix(.rows, 11) = lMassLink.Comment
      End If
    Next i
    .ColsSizeToWidth
  End With
  pEdited = False
End Sub

Private Sub cboID_Click()
  Dim discard&
    
  If prevMLid <> cboID Then
    If pEdited Then
      pFrm.Hide
      discard = myMsgBox.Show("Changes to current MassLink have not been saved. Discard them?", _
                pMassLink.Caption & " Edit Confirm Discard", "&Yes", "&No")
      pFrm.Show
    Else
      discard = 1 'no changes
    End If
    If discard = 1 Then 'discard
      pEdited = False
      Refresh
    Else 'no discard
      cboID = prevMLid
    End If
  End If

End Sub

Private Sub cmdAddNew_Click()
  Dim i&, Id&, ifound As Boolean
  'look for unused ml id
  Id = 0
  ifound = True
  Do Until ifound = False
    Id = Id + 1
    ifound = False
    For i = 1 To cboID.ListCount
      If cboID.List(i - 1) = Id Then
        ifound = True
      End If
    Next i
  Loop
  cboID.AddItem Id
  cboID = Id
End Sub

Private Sub grdMassLink_RowColChange()
  Dim lBlockDef As HspfBlockDef, icol&
  
  Call CheckLimitsMassLink(grdMassLink, pMassLink.Uci)
  
  Set lBlockDef = Me.Owner.Uci.Msg.BlockDefs("MASS-LINK")
  icol = grdMassLink.col + 1
  txtDefine = lBlockDef.TableDefs(1).ParmDefs(icol).Name & ": " & _
              lBlockDef.TableDefs(1).ParmDefs(icol).Define
  'RaiseEvent Change
  'pEdited = True
End Sub

Private Sub grdMassLink_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  grdMassLink_RowColChange
  RaiseEvent Change
  pEdited = True
End Sub

Public Sub Add()
  grdMassLink.rows = grdMassLink.rows + 1
  RaiseEvent Change
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Remove()
  Dim i&, j&
  For i = 1 To grdMassLink.rows
    For j = 0 To grdMassLink.cols - 1
      If grdMassLink.Selected(i, j) Then
        'remove selected rows
        grdMassLink.DeleteRow i
      End If
    Next j
  Next i
  RaiseEvent Change
End Sub

Private Sub UserControl_Resize()
  If Height > grdMassLink.Top + txtDefine.Height Then
    grdMassLink.Width = Width
    txtDefine.Width = Width
    txtDefine.Top = Height - txtDefine.Height
    grdMassLink.Height = Height - grdMassLink.Top - txtDefine.Height
    cmdAddNew.Left = grdMassLink.Width - cmdAddNew.Width
  End If
End Sub
