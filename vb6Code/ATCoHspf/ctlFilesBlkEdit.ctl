VERSION 5.00
Begin VB.UserControl ctlFilesBlkEdit 
   ClientHeight    =   1500
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6756
   ScaleHeight     =   1500
   ScaleWidth      =   6756
   Begin ATCoCtl.ATCoGrid grdFiles 
      Height          =   1332
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   6612
      _ExtentX        =   11663
      _ExtentY        =   2350
      SelectionToggle =   0   'False
      AllowBigSelection=   -1  'True
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   1
      Cols            =   3
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
Attribute VB_Name = "ctlFilesBlkEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFilesBlk As HspfFilesBlk
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfFilesBlk
  Set Owner = pFilesBlk
End Property
Public Property Set Owner(newFilesBlk As HspfFilesBlk)
  Dim i&
  
  Set pFilesBlk = newFilesBlk
  grdFiles.ClearData
  For i = 1 To pFilesBlk.Count
    With pFilesBlk.Value(i)
      grdFiles.TextMatrix(i, 0) = .typ
      grdFiles.TextMatrix(i, 1) = .Unit
      grdFiles.TextMatrix(i, 2) = .Name
    End With
  Next i
  grdFiles.ColsSizeByContents
End Property

Public Sub Save()
  Dim i&
  Dim lFile As HspfFile
  
  pFilesBlk.Clear
  For i = 1 To grdFiles.rows
    lFile.Name = grdFiles.TextMatrix(i, 2)
    lFile.typ = grdFiles.TextMatrix(i, 0)
    lFile.Unit = grdFiles.TextMatrix(i, 1)
    pFilesBlk.Add lFile
  Next i
  pFilesBlk.Uci.ClearWDM
  pFilesBlk.Uci.InitWDMArray
  Call pFilesBlk.Uci.SetWDMFiles
End Sub

Public Sub Add()
  grdFiles.rows = grdFiles.rows + 1
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Remove()
  If grdFiles.row > 0 Then grdFiles.DeleteRow grdFiles.row
End Sub

Private Sub grdFiles_RowColChange()
  With grdFiles
    .ClearValues
    If .col = 0 Then
      .addValue "MESSU"
      .addValue "WDM"
      .addValue " "
    End If
  End With
End Sub

Private Sub grdFiles_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  RaiseEvent Change
End Sub

Private Sub UserControl_Initialize()
  grdFiles.ColTitle(0) = "Type"
  grdFiles.ColType(0) = ATCoTxt
  grdFiles.ColEditable(0) = True
  
  grdFiles.ColTitle(1) = "Unit"
  grdFiles.ColType(1) = ATCoInt
  grdFiles.ColMin(1) = 1
  grdFiles.ColSoftMin(1) = 21
  grdFiles.ColMax(1) = 99
  grdFiles.ColSoftMax(1) = 99
  grdFiles.ColEditable(1) = True
  grdFiles.ColTitle(2) = "Name"
  grdFiles.ColType(2) = ATCoTxt
  grdFiles.ColEditable(2) = True
End Sub

Private Sub UserControl_Resize()
  If Height > grdFiles.Top + 200 And Width > 200 Then
    grdFiles.Width = Width
    grdFiles.Height = Height - grdFiles.Top
  End If
End Sub
