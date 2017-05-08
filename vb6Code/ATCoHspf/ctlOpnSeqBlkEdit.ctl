VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.UserControl ctlOpnSeqBlkEdit 
   ClientHeight    =   3216
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6516
   ScaleHeight     =   3216
   ScaleWidth      =   6516
   Begin ATCoCtl.ATCoText atxDelt 
      Height          =   252
      Left            =   840
      TabIndex        =   2
      Top             =   0
      Width           =   1212
      _ExtentX        =   2138
      _ExtentY        =   445
      InsideLimitsBackground=   16777215
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      HardMax         =   1440
      HardMin         =   1
      SoftMax         =   -999
      SoftMin         =   -999
      MaxWidth        =   -999
      Alignment       =   1
      DataType        =   1
      DefaultValue    =   -999
      Value           =   "-999"
      Enabled         =   -1  'True
   End
   Begin ATCoCtl.ATCoGrid grdOper 
      Height          =   2655
      Left            =   0
      TabIndex        =   1
      Top             =   480
      Width           =   6495
      _ExtentX        =   11451
      _ExtentY        =   4678
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
      Header          =   "lblHeader"
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
   Begin VB.Label lblInDelt 
      Caption         =   "Indelt:"
      Height          =   252
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   612
   End
End
Attribute VB_Name = "ctlOpnSeqBlkEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pOpnSeqBlk As HspfOpnSeqBlk
Private inLimits As Boolean, inInit As Boolean
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get frm() As Form
  Set frm = pFrm
End Property

Public Property Get Owner() As HspfOpnSeqBlk
  Set Owner = pOpnSeqBlk
End Property
Public Property Set Owner(newOpnSeqBlk As HspfOpnSeqBlk)
  Dim i&
  
  Set pOpnSeqBlk = newOpnSeqBlk
  grdOper.ClearData
  inInit = True
  For i = 1 To pOpnSeqBlk.Opns.Count
    With pOpnSeqBlk.Opn(i)
      grdOper.TextMatrix(i, 0) = .Name
      grdOper.TextMatrix(i, 1) = .Id
    End With
  Next i
  inInit = False
  DoLimits
  atxDelt.Value = pOpnSeqBlk.Delt
End Property

Private Sub atxDelt_Change()
  RaiseEvent Change
End Sub

Private Sub grdOper_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  DoLimits
  RaiseEvent Change
End Sub

Private Sub grdOper_RowColChange()
  'DoLimits
End Sub

Private Sub grdOper_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  'DoLimits
End Sub

Public Sub DoLimits()
  Dim i&, j&, sRow&, sCol&, ifound As Boolean
  
  If Not inLimits And Not inInit Then
    inLimits = True
    With grdOper
      sRow = .row
      sCol = .col
      .ClearValues
      If .col = 0 Then 'opername
        For i = 1 To pOpnSeqBlk.Uci.OpnBlks.Count
          .addvalue pOpnSeqBlk.Uci.OpnBlks(i).Name
        Next i
      End If
      For i = 2 To .rows
        ifound = False
        For j = 1 To i - 1
          If .TextMatrix(i, 0) = .TextMatrix(j, 0) And _
             .TextMatrix(i, 1) = .TextMatrix(j, 1) Then
            'duplicate
            ifound = True
            Exit For
          End If
        Next j
        .row = i
        If ifound Then
          .col = 0: .CellBackColor = .OutsideHardLimitBackground
          .col = 1: .CellBackColor = .OutsideHardLimitBackground
        Else
          .col = 0: .CellBackColor = .BackColor
          .col = 1: .CellBackColor = .BackColor
        End If
      Next i
      .row = sRow
      .col = sCol
    End With
    inLimits = False
  End If
End Sub

Private Sub UserControl_Initialize()
   With grdOper
    .Header = "Operations"
    .ColTitle(0) = "Name"
    .ColType(0) = ATCoTxt
    .ColEditable(0) = True
    .ColTitle(1) = "Number"
    .ColType(1) = ATCoInt
    .ColEditable(1) = True
    .ColMax(1) = 9999
    .ColMin(1) = 1
  End With
End Sub

Public Sub Save()
  Dim init&, retkey&, cbuff$, retcod&, l&
  Dim lOpn As HspfOperation, i&, InList As Boolean
  Dim vOpn As Variant, vId As Variant
  Dim lopnblk As HspfOpnBlk
  Dim ltable As HspfTable, nOpn As HspfOperation
  Dim myUci As HspfUci
  
  'refresh opn seq block
  pOpnSeqBlk.Delt = atxDelt.Value
  Set myUci = pOpnSeqBlk.Uci
  'find out if any operations have been deleted
  For Each vOpn In pOpnSeqBlk.Opns
    Set lOpn = vOpn
    InList = False
    For i = 1 To grdOper.rows
      If Len(grdOper.TextMatrix(i, 1)) > 0 And _
         Len(grdOper.TextMatrix(i, 0)) > 0 Then
        If lOpn.Id = grdOper.TextMatrix(i, 1) And _
           lOpn.Name = grdOper.TextMatrix(i, 0) Then
          InList = True
        End If
      End If
    Next i
    If Not InList Then
      'delete this operation
      myUci.DeleteOperation lOpn.Name, lOpn.Id
    End If
  Next vOpn
  'find out if any operations have been added
  For i = 1 To grdOper.rows
    InList = False
    For Each vOpn In pOpnSeqBlk.Opns
      Set lOpn = vOpn
      If Len(grdOper.TextMatrix(i, 1)) > 0 And _
         Len(grdOper.TextMatrix(i, 0)) > 0 Then
        If lOpn.Id = grdOper.TextMatrix(i, 1) And _
           lOpn.Name = grdOper.TextMatrix(i, 0) Then
          InList = True
        End If
      End If
    Next vOpn
    If Not InList And Len(grdOper.TextMatrix(i, 1)) > 0 And _
         Len(grdOper.TextMatrix(i, 0)) > 0 Then
      'add this operation
      Set lOpn = New HspfOperation
      lOpn.Name = grdOper.TextMatrix(i, 0)
      lOpn.Id = grdOper.TextMatrix(i, 1)
      myUci.AddOperation lOpn.Name, lOpn.Id
      AddOperationToOpnSeqBlock lOpn.Name, lOpn.Id, i
    End If
  Next i
End Sub

Private Sub UserControl_Resize()
  If Height > grdOper.Top Then
    grdOper.Height = Height - grdOper.Top
    grdOper.Width = Width
  End If
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Add()
  'pFrm.Hide
  frmAddOperation.init pOpnSeqBlk.Uci, Me, grdOper
  frmAddOperation.Show vbModal
'  With grdOper
'    If .SelStartRow > 0 Then
'      inLimits = True
'      .InsertRow .SelStartRow
'      .TextMatrix(.SelStartRow + 1, 1) = "" 'default to none
'      inLimits = False
'      DoLimits
'      RaiseEvent Change
'    End If
'  End With
End Sub

Public Sub Remove()
  With grdOper
    If .selstartrow > 0 Then
      .DeleteRow .selstartrow
      RaiseEvent Change
    End If
  End With
End Sub

Public Sub Edit()
  Dim lOper As HspfOperation
  Dim Name$, Id&
  
  Name = grdOper.TextMatrix(grdOper.row, 0)
  Id = grdOper.TextMatrix(grdOper.row, 1)
  Set lOper = pOpnSeqBlk.Uci.OpnBlks(Name).OperFromID(Id)
  If lOper Is Nothing Then
    pFrm.Hide
    myMsgBox.Show "This operation is not yet present to Edit." & vbCrLf & vbCrLf & _
      "Select 'Apply' or 'OK' before editing this operation.", _
      pOpnSeqBlk.Caption & " Edit Problem", "-+&OK"
    pFrm.Show
  Else
    lOper.Edit
  End If
End Sub

Public Sub AddOperationToOpnSeqBlock(opname$, opid&, irow&)
  Dim myUci As HspfUci
  Dim ltable As HspfTable
  Dim lopnblk As HspfOpnBlk, nOpn As HspfOperation
  
  Set myUci = pOpnSeqBlk.Uci
  
  'add to opn seq block
  If irow < grdOper.rows Then
    myUci.OpnSeqBlock.AddBefore myUci.OpnBlks(opname).OperFromID(opid), irow
  Else
    myUci.OpnSeqBlock.Add myUci.OpnBlks(opname).OperFromID(opid)
  End If
  Set myUci.OpnBlks(opname).OperFromID(opid).Uci = myUci
  
  If myUci.OpnBlks(opname).Count > 1 Then
    'already have some of this operation
    For Each ltable In myUci.OpnBlks(opname).Ids(1).Tables
      'add this opn id to this table
      myUci.AddTable opname, opid, ltable.Name
    Next ltable
  Else
    Set lopnblk = myUci.OpnBlks(opname)
    Set myUci.OpnBlks(opname).OperFromID(opid).OpnBlk = lopnblk
  End If
  
  'add dummy ftable if rchres
  If opname = "RCHRES" Then
    Set nOpn = myUci.OpnBlks("RCHRES").OperFromID(opid)
    Set nOpn.FTable = New HspfFtable
    Set nOpn.FTable.Operation = nOpn
    nOpn.FTable.Id = opid
  End If
End Sub
