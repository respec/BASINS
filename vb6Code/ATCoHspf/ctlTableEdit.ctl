VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.UserControl ctlTableEdit 
   ClientHeight    =   3888
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   8808
   ScaleHeight     =   3888
   ScaleWidth      =   8808
   Begin VB.Frame fraHsash 
      BackColor       =   &H8000000C&
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   64
      Left            =   0
      MousePointer    =   7  'Size N S
      TabIndex        =   5
      Top             =   2280
      Width           =   8535
   End
   Begin VB.ComboBox cboOccur 
      Height          =   288
      Left            =   4920
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   0
      Width           =   1452
   End
   Begin VB.CheckBox chkDesc 
      Caption         =   "Show Description"
      Height          =   252
      Left            =   0
      TabIndex        =   2
      Top             =   0
      Width           =   3015
   End
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
      Height          =   1455
      Left            =   0
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   1
      Text            =   "ctlTableEdit.ctx":0000
      Top             =   2400
      Width           =   8535
   End
   Begin ATCoCtl.ATCoGrid grdTable 
      Height          =   1935
      Left            =   0
      TabIndex        =   0
      Top             =   360
      Width           =   8535
      _ExtentX        =   15050
      _ExtentY        =   3408
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   -1  'True
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
      BackColorBkg    =   -2147483648
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Label lblOccur 
      Alignment       =   1  'Right Justify
      Caption         =   "Occurrence"
      Height          =   255
      Left            =   3240
      TabIndex        =   4
      Top             =   0
      Width           =   1575
   End
End
Attribute VB_Name = "ctlTableEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Private pTable As HspfTable
Event Change()
Private pFrm As Form
Private HsashDragging As Boolean
Private HsashDragStart As Single

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfTable
  Set Owner = pTable
End Property
Public Property Set Owner(newTable As HspfTable)
  Dim i, loi As Long
  Dim ctemp As String
  
  Set pTable = newTable
  
  fraHsash.BackColor = vbButtonFace
  
  If newTable.Def.NumOccur > 1 Then
    cboOccur.Clear
    
    For i = 1 To newTable.OccurCount 'how about later ones?
      If newTable.Def.OccurGroup > 0 Then
        'this is part of an occurance group, add name of occurance to combo box
        ctemp = ""
        If newTable.Opn.Name = "PERLND" Or newTable.Opn.Name = "IMPLND" Then
          ctemp = "QUAL-PROPS"
        ElseIf newTable.Opn.Name = "RCHRES" Then
          ctemp = "GQ-QALDATA"
        End If
        If i > 1 Then
          loi = pTable.Opn.OpnBlk.Ids(1).Tables(newTable.Name & ":" & i).OccurIndex
        Else
          loi = pTable.Opn.OpnBlk.Ids(1).Tables(newTable.Name).OccurIndex
        End If
        If loi = 0 Then
          loi = i
        End If
        If loi > 1 Then
          ctemp = ctemp & ":" & loi
        End If
        If pTable.Opn.OpnBlk.Ids(1).TableExists(ctemp) Then
          cboOccur.AddItem i & " - " & pTable.Opn.OpnBlk.Ids(1).Tables(ctemp).Parms(1).Value
        Else
          cboOccur.AddItem i
        End If
      Else
        cboOccur.AddItem i
      End If
    Next i
    cboOccur.ListIndex = newTable.OccurNum - 1
    lblOccur.Visible = True
    cboOccur.Visible = True
  Else
    lblOccur.Visible = False
    cboOccur.Visible = False
  End If
  If Len(pTable.Opn.Description) > 0 And pTable.Name <> "GEN-INFO" Then
    chkDesc.Value = 1
  Else
    chkDesc.Value = 0
    chkDesc.Visible = False
    grdTable.Height = grdTable.Top - chkDesc.Top + grdTable.Height
    grdTable.Top = chkDesc.Top
    refreshGrid
  End If
  
End Property

Private Sub cboOccur_Click()
  refreshGrid
End Sub

Private Sub chkDesc_Click()
  refreshGrid
End Sub

Private Sub grdTable_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  RaiseEvent Change
End Sub

Private Sub grdTable_RowColChange()
  Dim unitfg&
  If grdTable.col = 0 Then
    txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
                "Parameter: Operation Number" & vbCrLf & _
                vbCrLf
  ElseIf grdTable.col = chkDesc Then
    txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
                "Parameter: Description" & vbCrLf & _
                vbCrLf
  Else
    txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
                "Parameter: " & pTable.Parms(grdTable.col - chkDesc).Def.Define & vbCrLf & _
                vbCrLf
  End If
  unitfg = pTable.Opn.OpnBlk.Uci.GlobalBlock.emfg
  If unitfg = 1 Then 'english
    txtDefine = txtDefine & pTable.Def.HeaderE
  ElseIf unitfg = 2 Then 'metric
    txtDefine = txtDefine & pTable.Def.HeaderM
  End If
End Sub

Public Sub Help()
  Dim k$, c$, i&, d As HH_AKLINK, s As HH_FTS_QUERY, h$
  
  c = pFrm.Caption
  k = Right(c, Len(c) - InStrRev(c, ":")) & Mid(c, 6, InStr(c, ":") - 6) & vbNullString
  
  d.pszKeywords = k
  d.fReserved = vbFalse
  d.cbStruct = LenB(d)
  h = Trim(PathNameOnly(App.HelpFile)) & "\hspf.chm"
  HtmlHelp pFrm.hwnd, h, HH_ALINK_LOOKUP, d
End Sub

Private Sub refreshGrid()
  Dim i&, j&, lParm As HSPFParm, ltable As HspfTable
  Dim tname$, unitfg&
  Dim more As Boolean, skip As Boolean
  
  With grdTable
    .cols = pTable.Parms.Count + chkDesc
    .ColTitle(0) = "OpNum"
    If chkDesc > 0 Then
      .ColTitle(1) = "Description"
      .ColType(1) = ATCoTxt
      .ColEditable(1) = False
    End If
    For i = 1 To pTable.Parms.Count
      Set lParm = pTable.Parms(i)
      .ColTitle(i + chkDesc) = lParm.Name
      If lParm.Def.Typ = 2 Then
        .ColType(i + chkDesc) = ATCoSng  'causes formatting problems
      End If
      If lParm.Def.Typ = 1 Then
        .ColType(i + chkDesc) = ATCoInt
      End If
      If lParm.Def.Typ = 0 Then
        .ColType(i + chkDesc) = ATCoTxt
      End If
      unitfg = pTable.Opn.OpnBlk.Uci.GlobalBlock.emfg
      If unitfg = 1 Then 'english
        .ColMax(i + chkDesc) = lParm.Def.Max
        .ColMin(i + chkDesc) = lParm.Def.Min
      ElseIf unitfg = 2 Then 'metric
        .ColMax(i + chkDesc) = lParm.Def.MetricMax
        .ColMin(i + chkDesc) = lParm.Def.MetricMin
      End If
      .ColEditable(i + chkDesc) = True
    Next i
    
    'may need index here
    tname = pTable.Name
    If pTable.OccurCount > 1 Then
      If cboOccur.ListIndex > 0 Then
        tname = tname & ":" & cboOccur.ListIndex + 1
      End If
    End If
    
    more = True
    .rows = 0
    j = 1
    Do While more
      If pTable.EditAllSimilar Then
        If pTable.Opn.OpnBlk.NthOper(j).TableExists(tname) Then
          Set ltable = pTable.Opn.OpnBlk.NthOper(j).Tables(tname)
          skip = False
        Else
          skip = True
        End If
        j = j + 1
        If j > pTable.Opn.OpnBlk.Ids.Count Then
          more = False
        End If
      ElseIf pTable.OccurCount > 1 Then
        Set ltable = pTable.Opn.OpnBlk.OperFromID(pTable.Opn.Id).Tables(tname)
        skip = False
        more = False
      Else
        Set ltable = pTable
        skip = False
        more = False
      End If
        
      If Not skip Then
        .rows = .rows + 1
        .TextMatrix(.rows, 0) = ltable.Opn.Id
        If chkDesc > 0 Then
          .TextMatrix(.rows, 1) = ltable.Opn.Description
        End If
        For i = 1 To pTable.Parms.Count
          .TextMatrix(.rows, i + chkDesc) = ltable.Parms(i).Value
        Next i
      End If
    Loop
    
    .ColsSizeByContents
    .ToolTipText = pTable.Def.Define
    .Header = ""
    '.Sort 0, True
  End With
End Sub

Private Sub grdTable_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  RaiseEvent Change
End Sub

Public Sub Save()
  Dim i&, j&, lParm As HSPFParm, ltable As HspfTable
  Dim tname$
  
  tname = pTable.Name
  If pTable.OccurCount > 1 Then
    If cboOccur.ListIndex > 0 Then
      tname = tname & ":" & cboOccur.ListIndex + 1
    End If
  End If
      
  With grdTable
    For j = 1 To .rows
      If .rows = 1 Then
        Set ltable = Nothing
        Set ltable = pTable.Opn.Tables(tname)
      Else
        'Set ltable = pTable.Opn.OpnBlk.Ids(j).Tables(tname) 'changed for sort
        Set ltable = Nothing
        If Len(.TextMatrix(j, 0)) > 0 Then
          If Not pTable.Opn.OpnBlk.OperFromID(.TextMatrix(j, 0)) Is Nothing Then
            'make sure there is an operation by this number
            Set ltable = pTable.Opn.OpnBlk.OperFromID(.TextMatrix(j, 0)).Tables(tname)
          End If
        End If
      End If
      If Not ltable Is Nothing Then
        For i = 1 To ltable.Parms.Count
          Set lParm = ltable.Parms(i)
          lParm.Value = .TextMatrix(j, i + chkDesc)
          If ltable.Name = "GEN-INFO" And i = 1 Then
            ltable.Opn.Description = lParm.Value
          End If
        Next i
        ltable.Edited = True
      End If
    Next j
  End With
End Sub

Private Sub UserControl_Resize()
  If Height > grdTable.Top + txtDefine.Height Then
    txtDefine.Width = Width
    grdTable.Width = Width
    txtDefine.Top = Height - txtDefine.Height
    grdTable.Height = Height - grdTable.Top - txtDefine.Height
    fraHsash.Top = txtDefine.Top - fraHsash.Height
    If fraHsash.Top > grdTable.Top Then grdTable.Height = fraHsash.Top - grdTable.Top
    fraHsash.Width = Width
  End If
End Sub

Private Sub fraHsash_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = True
  HsashDragStart = y
  fraHsash.BackColor = vb3DShadow
End Sub

Private Sub fraHsash_MouseUp(Button As Integer, Shift As Integer, x As Single, y As Single)
  HsashDragging = False
  fraHsash.BackColor = vbButtonFace
End Sub

Private Sub fraHsash_MouseMove(Button As Integer, Shift As Integer, x As Single, y As Single)
  If HsashDragging Then
    Dim h&
    h = ScaleHeight - (fraHsash.Top + (y - HsashDragStart) / Screen.TwipsPerPixelY + fraHsash.Height)
    If h > 0 Then txtDefine.Height = h
    UserControl_Resize
  End If
End Sub
