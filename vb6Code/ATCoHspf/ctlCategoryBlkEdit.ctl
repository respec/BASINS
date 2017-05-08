VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Begin VB.UserControl ctlCategoryBlkEdit 
   ClientHeight    =   1500
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   6756
   ScaleHeight     =   1500
   ScaleWidth      =   6756
   Begin ATCoCtl.ATCoGrid grdCategory 
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
Attribute VB_Name = "ctlCategoryBlkEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pCategoryBlk As HspfCategoryBlk
Event Change()
Private pFrm As Form

Public Property Set frm(newFrm As Form)
  Set pFrm = newFrm
End Property

Public Property Get Owner() As HspfCategoryBlk
  Set Owner = pCategoryBlk
End Property
Public Property Set Owner(newCategoryBlk As HspfCategoryBlk)
  Dim i&
  
  Set pCategoryBlk = newCategoryBlk
  grdCategory.ClearData
  For i = 1 To pCategoryBlk.Count
    With pCategoryBlk.Value(i)
      grdCategory.TextMatrix(i, 0) = .Tag
      grdCategory.TextMatrix(i, 1) = .Name
    End With
  Next i
  grdCategory.ColsSizeByContents
End Property

Public Sub Save()
  Dim i&
  Dim lCategory As HspfCategory
  
  pCategoryBlk.Clear
  For i = 1 To grdCategory.rows
    lCategory.Tag = grdCategory.TextMatrix(i, 0)
    lCategory.Name = grdCategory.TextMatrix(i, 1)
    pCategoryBlk.Add lCategory
  Next i
End Sub

Public Sub Add()
  grdCategory.rows = grdCategory.rows + 1
End Sub

Public Sub Help()
  'just do contents for now
  HtmlHelp pFrm.hwnd, App.HelpFile, HH_DISPLAY_TOC, 0
End Sub

Public Sub Remove()
  If grdCategory.row > 0 Then grdCategory.DeleteRow grdCategory.row
End Sub

Private Sub grdCategory_TextChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  RaiseEvent Change
End Sub

Private Sub UserControl_Initialize()
  grdCategory.ColTitle(0) = "Tag"
  grdCategory.ColType(0) = ATCoTxt
  grdCategory.ColEditable(0) = True

  grdCategory.ColTitle(1) = "Name"
  grdCategory.ColType(1) = ATCoTxt
  grdCategory.ColEditable(1) = True
End Sub

Private Sub UserControl_Resize()
  If Height > grdCategory.Top + 200 And Width > 200 Then
    grdCategory.Width = Width
    grdCategory.Height = Height - grdCategory.Top
  End If
End Sub
