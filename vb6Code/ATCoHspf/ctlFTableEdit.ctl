VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.UserControl ctlFTableEdit 
   ClientHeight    =   3540
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   8712
   ScaleHeight     =   3540
   ScaleWidth      =   8712
   Begin VB.ComboBox cboID 
      Height          =   315
      Left            =   960
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   0
      Width           =   3855
   End
   Begin ATCoCtl.ATCoGrid grdFTable 
      Height          =   2655
      Left            =   0
      TabIndex        =   0
      Top             =   480
      Width           =   6855
      _ExtentX        =   12086
      _ExtentY        =   4678
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
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   2772
      Left            =   6960
      TabIndex        =   3
      Top             =   480
      Width           =   1695
      Begin VB.CommandButton cmdFtables 
         Caption         =   "Compute New"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   0
         Left            =   120
         TabIndex        =   10
         Top             =   1560
         Width           =   1455
      End
      Begin VB.CommandButton cmdFtables 
         Caption         =   "Import From Cross Section"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   1
         Left            =   120
         TabIndex        =   5
         Top             =   960
         Width           =   1455
      End
      Begin VB.CommandButton cmdFtables 
         Caption         =   "F-Curve"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Index           =   2
         Left            =   120
         TabIndex        =   4
         Top             =   2160
         Width           =   1455
      End
      Begin ATCoCtl.ATCoText txtNRows 
         Height          =   255
         Left            =   840
         TabIndex        =   6
         Top             =   120
         Width           =   855
         _ExtentX        =   1503
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   25
         HardMin         =   1
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "8"
         Value           =   "8"
         Enabled         =   -1  'True
      End
      Begin ATCoCtl.ATCoText txtNCols 
         Height          =   255
         Left            =   840
         TabIndex        =   7
         Top             =   480
         Width           =   855
         _ExtentX        =   1503
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   8
         HardMin         =   4
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   5
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   "8"
         Value           =   "8"
         Enabled         =   -1  'True
      End
      Begin VB.Label lblNRows 
         Caption         =   "NRows:"
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
         Left            =   0
         TabIndex        =   9
         Top             =   120
         Width           =   735
      End
      Begin VB.Label lblNcols 
         Caption         =   "NCols:"
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
         Left            =   0
         TabIndex        =   8
         Top             =   480
         Width           =   615
      End
   End
   Begin VB.Label lblId 
      Alignment       =   1  'Right Justify
      Caption         =   "FTable:"
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
      Left            =   0
      TabIndex        =   1
      Top             =   0
      Width           =   855
   End
End
Attribute VB_Name = "ctlFTableEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFTable As HspfFtable
Private pEdited As Boolean
Event Change()
Private pFrm As Form
Private PrevListIndex As Long

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Property Set Owner(newFTable As HspfFtable)
  Dim vId As Variant
  
  Set pFTable = newFTable
  
  cboID.Clear
  For Each vId In pFTable.Operation.OpnBlk.Ids
    cboID.AddItem vId.Id & " - " & vId.Description
    If vId.Tables("HYDR-PARM2").Parms("FTBUCI") = pFTable.Id Then 'this is the default
      cboID.ListIndex = cboID.ListCount - 1
      PrevListIndex = cboID.ListIndex
    End If
  Next vId
  txtNRows.Value = pFTable.Nrows
  txtNCols.Value = pFTable.Ncols
  Refresh
  
End Property

Public Sub Save()
  Dim i&, j&
  
  pFTable.Nrows = txtNRows.Value
  pFTable.Ncols = txtNCols.Value
  With grdFTable
    For i = 1 To .rows
      pFTable.Depth(i) = .TextMatrix(i, 0)
      pFTable.Area(i) = .TextMatrix(i, 1)
      pFTable.Volume(i) = .TextMatrix(i, 2)
      For j = 3 To .cols - 1
        If j = 3 Then
          pFTable.Outflow1(i) = .TextMatrix(i, j)
        End If
        If j = 4 Then
          pFTable.Outflow2(i) = .TextMatrix(i, j)
        End If
        If j = 5 Then
          pFTable.Outflow3(i) = .TextMatrix(i, j)
        End If
        If j = 6 Then
          pFTable.Outflow4(i) = .TextMatrix(i, j)
        End If
        If j = 7 Then
          pFTable.Outflow5(i) = .TextMatrix(i, j)
        End If
      Next j
    Next i
  End With
  pFTable.Edited
  pEdited = False
  
End Sub

Private Sub Refresh()
  Dim i&, j&, units&
  
  units = pFTable.Operation.OpnBlk.Uci.GlobalBlock.emfg
  
  With grdFTable
    .rows = txtNRows.Value
    .cols = txtNCols.Value
    For j = 0 To .cols - 1
      .ColType(j) = ATCoSng
      .ColEditable(j) = True
    Next j
    If units = 1 Then 'english
      .ColTitle(0) = "Depth (ft)"
      .ColTitle(1) = "Area (acres)"
      .ColTitle(2) = "Volume (acre-ft)"
      For j = 3 To .cols - 1
        .ColTitle(j) = "Outflow" & j - 2 & " (ft3/s)"
      Next j
    Else 'metric
      .ColTitle(0) = "Depth (m)"
      .ColTitle(1) = "Area (ha)"
      .ColTitle(2) = "Volume (Mm3)"
      For j = 3 To .cols - 1
        .ColTitle(j) = "Outflow" & j - 2 & " (m3/s)"
      Next j
    End If
    For i = 1 To .rows
      .TextMatrix(i, 0) = pFTable.Depth(i)
      .TextMatrix(i, 1) = pFTable.Area(i)
      .TextMatrix(i, 2) = pFTable.Volume(i)
      For j = 3 To .cols - 1
        If j = 3 Then
          .TextMatrix(i, j) = pFTable.Outflow1(i)
        End If
        If j = 4 Then
          .TextMatrix(i, j) = pFTable.Outflow2(i)
        End If
        If j = 5 Then
          .TextMatrix(i, j) = pFTable.Outflow3(i)
        End If
        If j = 6 Then
          .TextMatrix(i, j) = pFTable.Outflow4(i)
        End If
        If j = 7 Then
          .TextMatrix(i, j) = pFTable.Outflow5(i)
        End If
      Next j
    Next i
    .ColsSizeByContents
  End With
  pEdited = False
End Sub

Private Sub refreshGrid()
  Dim i&, j&, units&
  
  units = pFTable.Operation.OpnBlk.Uci.GlobalBlock.emfg
  
  With grdFTable
    .rows = txtNRows.Value
    .cols = txtNCols.Value
    For j = 3 To .cols - 1
      If units = 1 Then
        .ColTitle(j) = "Outflow" & j - 2 & " (ft3/s)"
      Else
        .ColTitle(j) = "Outflow" & j - 2 & " (m3/s)"
      End If
      .ColType(j) = ATCoSng
      .ColEditable(j) = True
    Next j
    For j = 0 To .cols - 1
      For i = 1 To .rows
        If Len(.TextMatrix(i, j)) = 0 Then
          .TextMatrix(i, j) = 0
        End If
      Next i
    Next j
    .ColsSizeByContents
  End With
End Sub

Private Sub cboID_Click()
  Dim discard&, tempID&, i&
  
  i = InStr(1, cboID, "-")
  If i > 0 Then
    tempID = CInt(Mid(cboID, 1, i - 2))
  Else
    tempID = CInt(cboID)
  End If
    
  If tempID <> pFTable.Id Then 'need to change table
    If pEdited Then
      'pFrm.Hide
      discard = myMsgBox.Show("Changes to current FTable have not been saved. Discard them?", _
                pFTable.Caption & " Edit Confirm Discard", "&Yes", "&No")
      'pFrm.Show
    Else
      discard = 1 'no changes
    End If
    If discard = 1 Then 'discard
      Set pFTable = pFTable.Operation.OpnBlk.Ids("K" & CStr(tempID)).FTable
      pEdited = False
      txtNRows.Value = pFTable.Nrows
      txtNCols.Value = pFTable.Ncols
      Refresh
    Else 'dont discard, set back to previous listindex
      cboID.ListIndex = PrevListIndex
    End If
  End If
  PrevListIndex = cboID.ListIndex
End Sub

Private Sub cmdFtables_Click(Index As Integer)
  Dim i&, j&, ipos&, lnts&, ilen&
  Dim capt$
  Dim g As Object
  Dim n&, nplt&
  Dim XYD() As xyplotdata
  
  If Index = 0 Then
    'compute new using mean annual flow method from athens
    Dim newFTable As New HspfFtable
    Set newFTable.Operation = pFTable.Operation
    newFTable.Id = pFTable.Id
    frmNewFtable.SetCurrentFTable newFTable, Me
    frmNewFtable.Show vbModal
    'txtNRows.Value = newFTable.Nrows
    'txtNCols.Value = newFTable.Ncols
  ElseIf Index = 1 Then
    'cross section import
    frmXSect.icon = pFTable.Operation.Uci.icon
    Call frmXSect.CurrentReach(pFTable.Operation.Id, pFTable.Operation.FTable)
    frmXSect.Show 1
    txtNRows.Value = pFTable.Nrows
    txtNCols.Value = pFTable.Ncols
    Refresh
  ElseIf Index = 2 Then
    'set data for plot
    ReDim XYD(0)
    XYD(0).NVal = pFTable.Nrows
    ReDim XYD(0).Var(0).Vals(XYD(0).NVal)
    ReDim XYD(0).Var(1).Vals(XYD(0).NVal)
    XYD(0).Var(0).Trans = 1
    XYD(0).Var(1).Trans = 1
    For j = 0 To 1
      XYD(0).Var(j).Min = 1E+30
      XYD(0).Var(j).Max = -1E+30
    Next j
    For i = 0 To XYD(0).NVal - 1
      XYD(0).Var(0).Vals(i) = grdFTable.TextMatrix(i + 1, 3)
      XYD(0).Var(1).Vals(i) = grdFTable.TextMatrix(i + 1, 0)
      For j = 0 To 1
        If XYD(0).Var(j).Vals(i) < XYD(0).Var(j).Min Then
          XYD(0).Var(j).Min = XYD(0).Var(j).Vals(i)
        End If
        If XYD(0).Var(j).Vals(i) > XYD(0).Var(j).Max Then
          XYD(0).Var(j).Max = XYD(0).Var(j).Vals(i)
        End If
      Next j
    Next i
    Call GLInit(1, g, 1, 2)
    capt = "F-Curve for Reach " & CStr(pFTable.Operation.Id) & " (" & pFTable.Operation.Description & ")"
    Call GLTitl("", capt)
    Call GLAxLab("Depth (ft)", "Outflow (cfs)", "", "")
    Call GLDoXY(g, 1, XYD(), 1)
  Else
    Call MsgBox("This option is not yet implemented.", , "FTable Problem")
  End If
End Sub

Private Sub grdFTable_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  'need to check values
  RaiseEvent Change
  pEdited = True
End Sub

Private Sub grdFTable_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  'need to check values
  RaiseEvent Change
  pEdited = True
End Sub

Private Sub txtNCols_Change()
  If txtNCols.Value > 3 And txtNCols.Value < 9 Then
    refreshGrid
    RaiseEvent Change
    pEdited = True
  End If
End Sub

Private Sub txtNRows_Change()
  If txtNRows.Value > 1 And txtNCols.Value < 26 Then
    refreshGrid
    RaiseEvent Change
    pEdited = True
  End If
End Sub

Private Sub UserControl_Resize()
  If Width > fraButtons.Width * 2 And Height > 700 Then
    grdFTable.Width = Width - fraButtons.Width - 250
    grdFTable.Height = Height - grdFTable.Top
    fraButtons.Left = Width - fraButtons.Width
  End If
End Sub

Public Sub UpdateFTABLE(aFtab As HspfFtable)
  Dim i As Long
  Dim j As Long
  
  txtNRows.Value = aFtab.Nrows
  txtNCols.Value = aFtab.Ncols
  With grdFTable
    .rows = txtNRows.Value
    .cols = txtNCols.Value
    For j = 0 To .cols - 1
      .ColType(j) = ATCoSng
      .ColEditable(j) = True
    Next j
    For i = 1 To .rows
      .TextMatrix(i, 0) = aFtab.Depth(i)
      .TextMatrix(i, 1) = aFtab.Area(i)
      .TextMatrix(i, 2) = aFtab.Volume(i)
      .TextMatrix(i, 3) = aFtab.Outflow1(i)
    Next i
    .ColsSizeByContents
  End With
End Sub
