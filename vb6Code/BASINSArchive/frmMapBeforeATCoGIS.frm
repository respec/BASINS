VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmMap 
   Caption         =   "Details"
   ClientHeight    =   6660
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8580
   Icon            =   "frmMap.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6660
   ScaleWidth      =   8580
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer timRefresh 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   1200
      Top             =   6120
   End
   Begin ATCoCtl.ATCoGrid agd 
      Height          =   3135
      Index           =   0
      Left            =   2400
      TabIndex        =   3
      Top             =   3480
      Width           =   6015
      _ExtentX        =   10610
      _ExtentY        =   5530
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
      Header          =   "<filename>"
      FixedRows       =   1
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.ListBox treeDirectory 
      Height          =   5910
      Left            =   0
      Style           =   1  'Checkbox
      TabIndex        =   2
      Top             =   0
      Width           =   2295
   End
   Begin VB.PictureBox pctMap 
      AutoRedraw      =   -1  'True
      BackColor       =   &H80000005&
      Height          =   3015
      Index           =   0
      Left            =   2400
      ScaleHeight     =   2955
      ScaleWidth      =   5955
      TabIndex        =   0
      Top             =   360
      Width           =   6015
   End
   Begin VB.Frame fraToolbar 
      BorderStyle     =   0  'None
      Height          =   405
      Left            =   2400
      TabIndex        =   5
      Top             =   0
      Width           =   6015
      Begin MSComctlLib.Toolbar tbrMap 
         Height          =   390
         Left            =   0
         TabIndex        =   7
         Top             =   0
         Width           =   4650
         _ExtentX        =   8202
         _ExtentY        =   688
         ButtonWidth     =   609
         ButtonHeight    =   582
         Wrappable       =   0   'False
         ImageList       =   "imgListTools"
         _Version        =   393216
         BeginProperty Buttons {66833FE8-8583-11D1-B16A-00C0F0283628} 
            NumButtons      =   15
            BeginProperty Button1 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Zoom"
               Object.ToolTipText     =   "Zoom"
               Object.Tag             =   "0"
            EndProperty
            BeginProperty Button2 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "ZoomIn"
               Object.ToolTipText     =   "Zoom In"
               Object.Tag             =   "10"
            EndProperty
            BeginProperty Button3 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "ZoomOut"
               Object.ToolTipText     =   "Zoom Out"
               Object.Tag             =   "20"
            EndProperty
            BeginProperty Button4 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Pan"
               Object.ToolTipText     =   "Pan"
               Object.Tag             =   "30"
            EndProperty
            BeginProperty Button5 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "FullExtent"
               Object.ToolTipText     =   "Full Extent"
               Object.Tag             =   "40"
            EndProperty
            BeginProperty Button6 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Separator1"
               Style           =   3
               MixedState      =   -1  'True
            EndProperty
            BeginProperty Button7 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Identify"
               Object.ToolTipText     =   "Identify"
               Object.Tag             =   "60"
            EndProperty
            BeginProperty Button8 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Edit"
               Object.ToolTipText     =   "Edit Layers"
               Object.Tag             =   "300"
            EndProperty
            BeginProperty Button9 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Add"
               Object.ToolTipText     =   "Add Location"
               Object.Tag             =   "200"
            EndProperty
            BeginProperty Button10 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Move"
               Object.ToolTipText     =   "Move Location"
               Object.Tag             =   "220"
            EndProperty
            BeginProperty Button11 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Separator2"
               Style           =   3
               MixedState      =   -1  'True
            EndProperty
            BeginProperty Button12 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Save"
               Object.ToolTipText     =   "Save Map"
               Object.Tag             =   "80"
            EndProperty
            BeginProperty Button13 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Get"
               Object.ToolTipText     =   "Get Map Specs"
               Object.Tag             =   "90"
            EndProperty
            BeginProperty Button14 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Separator3"
               Style           =   3
               MixedState      =   -1  'True
            EndProperty
            BeginProperty Button15 {66833FEA-8583-11D1-B16A-00C0F0283628} 
               Key             =   "Print"
               Object.ToolTipText     =   "Print Map"
               Object.Tag             =   "110"
            EndProperty
         EndProperty
      End
      Begin VB.CheckBox chkLinkMaps 
         Caption         =   "Link"
         Height          =   375
         Left            =   4680
         TabIndex        =   6
         ToolTipText     =   "Keep Map Scales Synchronized"
         Top             =   0
         Width           =   735
      End
      Begin MSComctlLib.ImageList imgListTools 
         Left            =   5520
         Top             =   0
         _ExtentX        =   794
         _ExtentY        =   794
         BackColor       =   -2147483643
         ImageWidth      =   16
         ImageHeight     =   16
         MaskColor       =   8421376
         _Version        =   393216
         BeginProperty Images {2C247F25-8591-11D1-B16A-00C0F0283628} 
            NumListImages   =   23
            BeginProperty ListImage1 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":08CA
               Key             =   "Zoom"
            EndProperty
            BeginProperty ListImage2 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":0E1C
               Key             =   "Pointer"
            EndProperty
            BeginProperty ListImage3 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":0ED4
               Key             =   "ZoomIn"
            EndProperty
            BeginProperty ListImage4 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":1426
               Key             =   "ZoomOut"
            EndProperty
            BeginProperty ListImage5 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":1978
               Key             =   "Print"
            EndProperty
            BeginProperty ListImage6 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":1C92
               Key             =   "Pan"
            EndProperty
            BeginProperty ListImage7 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":21E4
               Key             =   "FullExtent"
            EndProperty
            BeginProperty ListImage8 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":2736
               Key             =   "Identify"
            EndProperty
            BeginProperty ListImage9 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":2C88
               Key             =   "Left"
            EndProperty
            BeginProperty ListImage10 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":2FA2
               Key             =   "Right"
            EndProperty
            BeginProperty ListImage11 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":32BC
               Key             =   "Select"
            EndProperty
            BeginProperty ListImage12 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":35D6
               Key             =   "Unselect"
            EndProperty
            BeginProperty ListImage13 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":38F0
               Key             =   "Get"
            EndProperty
            BeginProperty ListImage14 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":3C0A
               Key             =   "Save"
            EndProperty
            BeginProperty ListImage15 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":3F24
               Key             =   "PrintFolder"
            EndProperty
            BeginProperty ListImage16 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":423E
               Key             =   "Add"
            EndProperty
            BeginProperty ListImage17 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":4558
               Key             =   "Remove"
            EndProperty
            BeginProperty ListImage18 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":4872
               Key             =   "Properties"
            EndProperty
            BeginProperty ListImage19 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":4B8C
               Key             =   "Clear"
            EndProperty
            BeginProperty ListImage20 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":4EA6
               Key             =   "Up"
            EndProperty
            BeginProperty ListImage21 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":51C0
               Key             =   "Down"
            EndProperty
            BeginProperty ListImage22 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":54DA
               Key             =   "Edit"
            EndProperty
            BeginProperty ListImage23 {2C247F27-8591-11D1-B16A-00C0F0283628} 
               Picture         =   "frmMap.frx":57F4
               Key             =   "Move"
            EndProperty
         EndProperty
      End
   End
   Begin VB.PictureBox pctBuf 
      AutoRedraw      =   -1  'True
      BackColor       =   &H80000005&
      Height          =   1815
      Left            =   2400
      ScaleHeight     =   1755
      ScaleWidth      =   5955
      TabIndex        =   8
      Top             =   360
      Visible         =   0   'False
      Width           =   6015
   End
   Begin VB.Frame sash 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   4815
      Left            =   2280
      MousePointer    =   9  'Size W E
      TabIndex        =   1
      Top             =   0
      Width           =   120
   End
   Begin VB.Frame sashNS 
      BorderStyle     =   0  'None
      Height          =   120
      Left            =   2400
      MousePointer    =   7  'Size N S
      TabIndex        =   4
      Top             =   3360
      Width           =   6165
      Begin VB.Label lblHideGrid 
         Caption         =   "v"
         Height          =   255
         Left            =   360
         TabIndex        =   10
         Top             =   -45
         Width           =   135
      End
      Begin VB.Label lblHideMap 
         Caption         =   "^"
         Height          =   135
         Left            =   120
         TabIndex        =   9
         Top             =   10
         Width           =   135
      End
   End
   Begin VB.Image imgPan 
      Height          =   480
      Left            =   0
      Picture         =   "frmMap.frx":5B0E
      Top             =   6120
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image imgMovePoint 
      Height          =   480
      Left            =   360
      Picture         =   "frmMap.frx":63D8
      Top             =   6120
      Visible         =   0   'False
      Width           =   480
   End
   Begin VB.Image imgZoom 
      Height          =   480
      Left            =   720
      Picture         =   "frmMap.frx":66E2
      Top             =   6120
      Visible         =   0   'False
      Width           =   480
   End
End
Attribute VB_Name = "frmMap"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Long, ByVal X As Long, ByVal Y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal dwRop As Long) As Long
Private Const SRCCOPY = &HCC0020 ' (DWORD) dest = source

Private Const pSelectedColor = vbMagenta

Private pDBFs() As FastCollection
Private pLayers() As FastCollection
Private pHighlightedShape() As Long
Private pAllFilenames As FastCollection
Private pSashDragging As Boolean
Private pSashNSDragging As Boolean
Private pNumProjects As Long

Private pCurMap As Integer
Private pLinkMaps As Boolean
'Private pMapMovingPoint As Long
'Private pDeletingPoint As Boolean
Private pTrackNearestShape As Boolean
Private pRectActive As Boolean

Private pMouseDownX As Single
Private pMouseDownY As Single

Private pSkipGrid As Boolean
Private pDirectory_Clicking As Boolean

Private pIPC As Object

Private Sub agd_Click(Index As Integer)
  Dim tmpLayer As Layer
  
  If treeDirectory.Selected(treeDirectory.ListIndex) Then
    Set tmpLayer = GetMapLayer(treeDirectory.Text, pCurMap)
    If Not tmpLayer Is Nothing Then
      With agd(Index)
        SelectShape tmpLayer, .row, .Selected(.row, 0)
      End With
    End If
  End If
End Sub

Private Sub chkLinkMaps_Click()
  If chkLinkMaps.Value = vbChecked Then
    pLinkMaps = True
  Else
    pLinkMaps = False
  End If
End Sub

Private Sub DbgMsg(Msg$)
  If (pIPC Is Nothing) Then
    Debug.Print "Map: " & Msg
  Else
    pIPC.dbg "Map: " & Msg
  End If
End Sub

Private Sub Form_Initialize()
  Dim i As Long
  
  ReDim pDBFs(0):   Set pDBFs(0) = New FastCollection
  ReDim pLayers(0): Set pLayers(0) = New FastCollection
                    Set pAllFilenames = New FastCollection
  ReDim pHighlightedShape(0)
  On Error Resume Next
  With tbrMap.Buttons
    For i = 1 To .count
      If .item(i).Image = 0 And Left(.item(i).key, 9) <> "Separator" Then .item(i).Image = .item(i).key
    Next i
  End With
  With tbrMap.Buttons(tbrMap.Buttons.count)
    tbrMap.Width = .Left + .Width
  End With

End Sub

Private Sub lblHideGrid_Click()
  Dim needRedraw As Boolean
  If pctMap(0).Height < 100 Then needRedraw = True
  sashNS.Top = Me.scaleHeight - sashNS.Height
  pSashNSDragging = True
  sashNS_MouseMove 0, 0, 0, 0
  pSashNSDragging = False
  If needRedraw Then Redraw
End Sub

Private Sub lblHideMap_Click()
  Dim needRedraw As Boolean
  Me.MousePointer = vbHourglass
  If agd(0).Height < 100 Then needRedraw = True
  sashNS.Top = pctMap(0).Top
  pSashNSDragging = True
  sashNS_MouseMove 0, 0, 0, 0
  pSashNSDragging = False
  If needRedraw Then RedrawGrid
  Me.MousePointer = vbDefault
End Sub

Private Sub ClearToolTips()
  Dim i As Integer
  For i = 1 To pNumProjects - 1
    pctMap(i).ToolTipText = ""
  Next
End Sub

Private Sub pctMap_MouseDown(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  DbgMsg "pctMap(" & Index & ")_MouseDown: (" & X & ", " & Y & ") Button=" & Button
  If Index <> pCurMap Then
    pctMap(pCurMap).BorderStyle = 0
    pCurMap = Index
    pctMap(pCurMap).BorderStyle = 1
  End If
  
  pMouseDownX = X
  pMouseDownY = Y
  
  If Index < 0 Then '  If curLay < 0 Then
    'Debug.Print "MouseDown but no curLay"
  ElseIf tbrMap.Buttons("Zoom").Value = tbrPressed Then
    If Button = vbKeyRButton Then
      MapZoom "OutXY"
    Else
      pRectActive = True
    End If
  ElseIf Button = vbKeyRButton Then
    MapAction -1 'Deselect any mode (identify, zoom, add, move)
    ' do we want to respond to right mouse events?
  ElseIf tbrMap.Buttons("Pan").Value = tbrPressed Then
    'UnsavedChanges = True
        'copy the background to the buffer
    With pctMap(Index)
      pctBuf.Width = .Width
      pctBuf.Height = .Height
      pctBuf.Scale (.scaleLeft, .scaleTop)-(.scaleLeft + .scaleWidth, .scaleTop + .scaleHeight)
      pctBuf.Picture = .Image

      'BitBlt pctBuf.hdc, 0, 0, .Width / Screen.TwipsPerPixelX, .Height / Screen.TwipsPerPixelY, .hdc, 0, 0, SRCCOPY
    End With
'  ElseIf tbrMap.Buttons("Add").Value = tbrPressed Then
'    frmMapAddPt.Show vbModal, Me
'    If frmMapAddPt.ok Then
'      Dim pt As point
'      Set pt = pctMap.ToMapPoint(X, Y)
'      AddPointToShapeFileOnMap frmMapAddPt.ptname, pt.X, pt.Y
'    End If
'    tbrMap.Buttons("Add").Value = tbrUnpressed
'    fraMap1.MousePointer = vbDefault
'  'ElseIf tbrMap.Buttons("Remove").Value = tbrPressed Then
'
'  ElseIf tbrMap.Buttons("Move").Value = tbrPressed Then
'    pTrackNearestShape = False
'    pMapMovingPoint = IdPoint(X, Y)
'    Highlight = ""
'  ElseIf pDeletingPoint Then
'    If CurShapeFldVals.count < 1 Then IdPoint X, Y
'    If CurShapeFldVals.count < 1 Then
'      MsgBox "Did not find a point to delete", vbOKOnly, "ATCoMap"
'    ElseIf DeletePointOnMap(CurShapeFldVals(0)) Then
'      MsgBox "Deleted a point", vbOKOnly, "ATCoMap"
'    Else
'      MsgBox "Did not delete a point", vbOKOnly, "ATCoMap"
'    End If
'    pTrackNearestShape = False
'    pDeletingPoint = False
'    fraMap1.MousePointer = vbDefault
  ElseIf Button = vbKeyLButton Then
    HighlightShapeNear X, Y, True
  End If
  
End Sub

Private Sub pctMap_MouseMove(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Xpixels As Long
  Dim Ypixels As Long
  'Static variables address a strange behavior: sometimes we see continuous MouseMove events in same place
  Static LastX As Single, LastY As Single
  If X = LastX And Y = LastY Then Exit Sub
  LastX = X
  LastY = Y
  'DbgMsg "pctMap_MouseMove: (" & x & ", " & y & ") Button=" & Button
  If tbrMap.Buttons("Move").Value = tbrPressed Then
    'If pMapMovingPoint > 0 Then RewritePointInShapeFile pMapMovingPoint, pctMap.ToMapPoint(x, y)
  ElseIf tbrMap.Buttons("Pan").Value = tbrPressed Then 'drag picture as it exists around quickly
    If pMouseDownX <> 0 And pMouseDownY <> 0 Then
      With pctMap(Index)
        .Cls
        Xpixels = (X - pMouseDownX) / .scaleWidth * .Width / Screen.TwipsPerPixelX
        Ypixels = (Y - pMouseDownY) / .scaleHeight * .Height / Screen.TwipsPerPixelY
        BitBlt .hdc, Xpixels, Ypixels, .Width / Screen.TwipsPerPixelX, .Height / Screen.TwipsPerPixelY, pctBuf.hdc, 0, 0, SRCCOPY
        .Refresh
      End With
    End If
  ElseIf tbrMap.Buttons("Zoom").Value = tbrPressed Then 'Draw xor rubber band
    If pRectActive Then XorBox Index, X, Y
  ElseIf pTrackNearestShape Then
    HighlightShapeNear X, Y
  Else
    pctMap(Index).ToolTipText = ""
  End If
End Sub

Private Sub pctMap_MouseUp(Index As Integer, Button As Integer, Shift As Integer, X As Single, Y As Single)
  With pctMap(Index)
    If tbrMap.Buttons("Move").Value = tbrPressed Then
'      pMapMovingPoint = 0
'      pTrackNearestShape = True
'      Redraw
    ElseIf tbrMap.Buttons("Pan").Value = tbrPressed Then
      If pMouseDownX <> 0 And pMouseDownY <> 0 Then
        pctMap(Index).scaleLeft = pctMap(Index).scaleLeft + (pMouseDownX - X)
        pctMap(Index).scaleTop = pctMap(Index).scaleTop + (pMouseDownY - Y)
        Redraw
      End If
    ElseIf tbrMap.Buttons("Zoom").Value = tbrPressed Then
      If pRectActive Then
        pRectActive = False
        XorBox Index, 0, 0
        If Abs(pMouseDownX - X) < Abs(.scaleWidth) / 100 And _
           Abs(pMouseDownY - Y) < Abs(.scaleHeight) / 100 Then 'Too small a rectangle, assule they just meant to zoom 2x
          If Button = vbLeftButton Then
            MapZoom "InXY"
          ElseIf Button = vbRightButton Then
            MapZoom "OutXY"
          End If
          'UnsavedChanges = True
        Else  '65.33.237.102:8964 12.247.71.46:6346 24.222.60.156:6346
          Dim ZoomPercent As Single, xZoomPercent As Single
          ZoomPercent = Abs((pMouseDownY - Y) / .scaleHeight)
          xZoomPercent = Abs((pMouseDownX - X) / .scaleWidth)
          If xZoomPercent > ZoomPercent Then ZoomPercent = xZoomPercent
          MapScale (pMouseDownX + X - .scaleWidth * ZoomPercent) / 2, _
                   (pMouseDownY + Y - .scaleHeight * ZoomPercent) / 2, _
                   .scaleWidth * ZoomPercent, _
                   .scaleHeight * ZoomPercent
        End If
      End If
    End If
  End With
  pMouseDownX = 0
  pMouseDownY = 0
End Sub

'If X and Y are zero, just erase last XorBox, else draw new one from pMouseDownX, pMouseDownY to X, Y
Private Sub XorBox(Index As Integer, X As Single, Y As Single)
  Static LastX As Single, LastY As Single
  Dim saveDrawMode As Long
  With pctMap(Index)
    saveDrawMode = .DrawMode
    .DrawMode = vbNotXorPen
    
    If LastX <> 0 And LastY <> 0 Then 'Erase last Xor box
      If pMouseDownX = LastX Or pMouseDownY = LastY Then
        pctMap(Index).Line (pMouseDownX, pMouseDownY)-(LastX, LastY), vbBlack
      Else
        pctMap(Index).Line (pMouseDownX, pMouseDownY)-(LastX, LastY), vbBlack, B
      End If
    End If
      
    If X <> 0 And Y <> 0 Then 'Draw new Xor box
      If pMouseDownX = X Or pMouseDownY = Y Then
        pctMap(Index).Line (pMouseDownX, pMouseDownY)-(X, Y), vbBlack
      Else
        pctMap(Index).Line (pMouseDownX, pMouseDownY)-(X, Y), vbBlack, B
      End If
    End If
    LastX = X
    LastY = Y
    .DrawMode = saveDrawMode
  End With
End Sub

Private Sub sash_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pSashDragging = True
End Sub
Private Sub sash_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If pSashDragging And (sash.Left + X) > 1000 And (sash.Left + X < Width - 1000) Then
    sash.Left = sash.Left + X
    treeDirectory.Width = sash.Left
    Form_Resize
  End If
End Sub
Private Sub sash_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pSashDragging = False
End Sub

Private Sub sashNS_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pSashNSDragging = True
End Sub
Private Sub sashNS_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If pSashNSDragging And (sashNS.Top + Y) >= pctMap(0).Top Then
    'And (sashNS.Top + y) > 1000 And (sashNS.Top + y < Height - 1000) Then
    Dim i As Long
    sashNS.Top = sashNS.Top + Y
    pctMap(0).Height = sashNS.Top - pctMap(0).Top
    For i = 1 To pNumProjects - 1
      pctMap(i).Height = pctMap(0).Height
    Next
    Form_Resize
  End If
End Sub
Private Sub sashNS_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pSashNSDragging = False
End Sub

Private Sub Form_Resize()
  Dim eachWidth As Long
  Dim gridHeight As Long
  Dim i As Long
  
  sash.Height = scaleHeight
  sashNS.Width = scaleWidth 'Makes it go beyond the right edge of the window, but that is ok
  
  sashNS.Left = sash.Left + sash.Width
  pctMap(0).Left = sashNS.Left
  agd(0).Left = sashNS.Left
  fraToolbar.Left = pctMap(0).Left
  If scaleWidth > fraToolbar.Left + 100 Then fraToolbar.Width = scaleWidth - fraToolbar.Left
  
  eachWidth = (scaleWidth - pctMap(0).Left) / pNumProjects

  If eachWidth > 100 Then
    pctMap(0).Width = eachWidth
    agd(0).Width = eachWidth
    For i = 1 To pNumProjects - 1
      pctMap(i).Width = eachWidth: pctMap(i).Left = pctMap(i - 1).Left + eachWidth
      agd(i).Width = eachWidth:       agd(i).Left = agd(i - 1).Left + eachWidth
    Next
  End If
  
  gridHeight = scaleHeight - sashNS.Top - sashNS.Height
  If gridHeight < 0 Then gridHeight = 0
  For i = 0 To agd.UBound
    agd(i).Top = sashNS.Top + sashNS.Height
    agd(i).Height = gridHeight
  Next
  
  If scaleHeight > treeDirectory.Top + 100 Then
    treeDirectory.Height = scaleHeight - treeDirectory.Top
  End If
  'Redraw
End Sub

Public Sub Add(label As String, realFilenames As FastCollection, Selected As Boolean)
  If pNumProjects = 0 Then
    ReDim Preserve pLayers(0 To realFilenames.count - 1)
    ReDim Preserve pDBFs(0 To realFilenames.count - 1)
    For pNumProjects = 1 To realFilenames.count - 1
      Load pctMap(pNumProjects)
      Load agd(pNumProjects)
      pctMap(pNumProjects).Visible = True
      agd(pNumProjects).Visible = True
      Set pLayers(pNumProjects) = New FastCollection
      Set pDBFs(pNumProjects) = New FastCollection
    Next
    ReDim pHighlightedShape(pNumProjects)
  ElseIf realFilenames.count <> pNumProjects Then
    MsgBox "Must have same number of realFilenames for each label", vbOKOnly, "Map Add"
  Else
    pAllFilenames.Add realFilenames, label
    treeDirectory.AddItem label
    If Selected Then treeDirectory.Selected(treeDirectory.ListCount - 1) = True
  End If
End Sub

Public Sub Redraw()
  Dim iMap As Integer
  Dim iSave As Integer
  Dim ilyr As Long
  Dim tmpLayer As Layer
  Dim lLabel As String 'layer label from treeDirectory
  Dim startTime As Single
  
  If Not Me.Visible Then Exit Sub
  If treeDirectory.ListCount < 1 Then Exit Sub
  
  ClearToolTips
  startTime = Timer
  Me.MousePointer = vbHourglass
  timRefresh.Enabled = True
  For iMap = 0 To pNumProjects - 1
    timRefresh.Tag = iMap
    If pctMap(iMap).Height > 100 And pctMap(iMap).Width > 100 Then
      pctMap(iMap).Cls
      
      For ilyr = 0 To treeDirectory.ListCount - 1
        If treeDirectory.Selected(ilyr) Then
          lLabel = treeDirectory.List(ilyr)
          
          'If there is a map layer for this item, draw it
          Set tmpLayer = GetMapLayer(lLabel, iMap)
          If Not tmpLayer Is Nothing Then
            If pctMap(iMap).ScaleMode <> 0 Then
              iSave = pCurMap
              pCurMap = iMap
              MapZoom "Full"
              pCurMap = iSave
            Else
              tmpLayer.Render pctMap(iMap)
            End If
          End If
          
        End If
      Next ilyr
      pctMap(iMap).Refresh
      pHighlightedShape(iMap) = 0
    End If
  Next iMap
  timRefresh.Enabled = False
  Debug.Print "Map Redrawn in " & Format(Timer - startTime, "0.00") & " seconds"
  
  If pSkipGrid Then  'Just unselected this item, dont fill grid with unselected data
    pSkipGrid = False
  Else
    RedrawGrid
  End If
  
  Me.MousePointer = vbDefault
End Sub

Private Sub RedrawGrid()
  Dim iMap As Integer
  Dim lLabel As String
  
  For iMap = 0 To pNumProjects - 1
    If agd(iMap).Height > 100 And agd(iMap).Width > 100 Then
      'If there is something to put in the grid, put it there
      lLabel = treeDirectory.List(treeDirectory.ListIndex)
      Select Case LCase(FileExt(lLabel))
        Case "shp", "dbf"
          PutDBFinGrid GetDBF(lLabel, iMap), agd(iMap)
        Case Else
          agd(iMap).Clear: agd(iMap).Visible = False
      End Select
    End If
  Next iMap
End Sub

Private Sub MapZoom(rule As String)
  Dim ilyr As Long
  Dim tmpLayer As Layer
  Dim SetScale As Boolean
  Dim iMap As Integer
  Dim firstMap As Long
  Dim lastMap As Long
  
  With pctMap(pCurMap)
    Select Case rule
      Case "Full" 'Set map scale to fit all layers
        If pLinkMaps Then 'Need to set scale to fit all layers on all maps
          firstMap = 0
          lastMap = pNumProjects - 1
        Else
          firstMap = pCurMap
          lastMap = pCurMap
        End If
        For ilyr = 0 To treeDirectory.ListCount - 1
          If treeDirectory.Selected(ilyr) Then
            For iMap = firstMap To lastMap
              Set tmpLayer = GetMapLayer(treeDirectory.List(ilyr), iMap)
              If Not tmpLayer Is Nothing Then
                tmpLayer.SetMaxScale pctMap(pCurMap), SetScale
                SetScale = True
              End If
            Next
          End If
        Next ilyr
        MapScale .scaleLeft, .scaleTop, .scaleWidth, .scaleHeight
      Case "In":    MapScale .scaleLeft + .scaleWidth / 4, .scaleTop + .scaleHeight / 4, .scaleWidth / 2, .scaleHeight / 2
      Case "Out":   MapScale .scaleLeft - .scaleWidth / 2, .scaleTop - .scaleHeight / 2, .scaleWidth * 2, .scaleHeight * 2
      Case "InXY":  MapScale pMouseDownX - .scaleWidth / 4, pMouseDownY - .scaleHeight / 4, .scaleWidth / 2, .scaleHeight / 2
      Case "OutXY": MapScale pMouseDownX - .scaleWidth, pMouseDownY - .scaleHeight, .scaleWidth * 2, .scaleHeight * 2
      Case Else
        
    End Select
  End With
End Sub

'Left and Top must be specified and either Width and Height or Right and Bottom must be specified
'If specifying Right and Bottom, values for Width and Height will be ignored
Private Sub MapScale(Left As Single, Top As Single, Width As Single, Height As Single, Optional Right As Single = 0, Optional Bottom As Single = 0)
  Dim iMap As Integer
  If Right = 0 Then Right = Left + Width
  If Bottom = 0 Then Bottom = Top + Height
  If Abs(Right - Left) > 0 And Abs(Bottom - Top) > 0 Then
    If Not pLinkMaps Then
      pctMap(pCurMap).Scale (Left, Top)-(Right, Bottom)
    Else 'Need to set scale on all maps
      With pctMap(pCurMap)
        For iMap = 0 To pNumProjects - 1
          pctMap(iMap).Scale (Left, Top)-(Right, Bottom)
        Next
      End With
    End If
  Else
    Debug.Print "Avoided setting scale to zero width or height"
  End If
  Redraw
End Sub

Private Function GetMapLayer(label As String, mapIndex As Integer) As Layer
  Dim shpFilename As String
  Dim tmpLayer As Layer
  Dim iLayer As Long
  
  On Error GoTo NeverMind
  
  iLayer = pLayers(mapIndex).IndexFromKey(label)
  If iLayer > 0 Then
    Set GetMapLayer = pLayers(mapIndex).ItemByIndex(iLayer)
  Else
    Set tmpLayer = Nothing
    shpFilename = pAllFilenames.ItemByKey(label).ItemByIndex(mapIndex + 1)
    If LCase(FileExt(shpFilename)) = "shp" Then
      Set tmpLayer = New Layer
      tmpLayer.ShapeFile = shpFilename
      If tmpLayer.NumRecords < 1 Then
        Set tmpLayer = Nothing
      Else
        pLayers(mapIndex).Add tmpLayer, label
      End If
    End If
    Set GetMapLayer = tmpLayer
  End If
NeverMind:
End Function

Private Function GetDBF(label As String, mapIndex As Integer) As clsDBF
  Dim dbfFilename As String
  Dim tmpDBF As clsDBF
  Dim iDBF As Long
  
  On Error GoTo NeverMind
  
  iDBF = pDBFs(mapIndex).IndexFromKey(label)
  If iDBF > 0 Then
    Set GetDBF = pDBFs(mapIndex).ItemByIndex(iDBF)
  Else
    Set tmpDBF = Nothing
    dbfFilename = pAllFilenames.ItemByKey(label).ItemByIndex(mapIndex + 1)
    If LCase(FileExt(dbfFilename)) = "shp" Then
      dbfFilename = Left(dbfFilename, Len(dbfFilename) - 3) & "dbf"
    End If
    If FileExists(dbfFilename) Then
      Set tmpDBF = New clsDBF
      tmpDBF.OpenDBF dbfFilename
      pDBFs(mapIndex).Add tmpDBF, label
    End If
    Set GetDBF = tmpDBF
  End If
NeverMind:
End Function

Private Sub PutDBFinGrid(dbf As clsDBF, g As ATCoGrid)
  Dim row As Long
  Dim col As Long
  Dim field As Long
  Dim HUCfield As Long
  
  If dbf Is Nothing Then
    g.Clear: g.Visible = False
  ElseIf dbf.FileName = g.header And Len(dbf.FileName) > 0 Then
    'Already have this dbf displayed
  Else
    g.Visible = False
    g.header = dbf.FileName
    g.cols = dbf.NumFields
    g.Rows = 0
    g.Rows = dbf.NumRecords
    g.SelectionMode = ASdisjointByRow
    HUCfield = -1
    For field = 1 To dbf.NumFields
      col = field - 1
      g.ColSelectable(col) = True
      g.ColTitle(col) = dbf.FieldName(field)
      If UCase(g.ColTitle(col)) = "HUC" Then
        g.ColType(col) = ATCoTxt
        HUCfield = field
      Else
        Select Case dbf.FieldType(field) 'C = Character, D = Date, N = Numeric, L = Logical, M = Memo
          Case "C": g.ColType(col) = ATCoTxt '= 0
          Case "D": g.ColType(col) = ATCoTxt
          Case "N": g.ColType(col) = ATCoSng '= 2
          Case "L": g.ColType(col) = ATCoTxt
          Case "M": g.ColType(col) = ATCoTxt
        End Select
      End If
    Next
    row = 1
    While row <= dbf.NumRecords
      dbf.CurrentRecord = row
      For field = 1 To dbf.NumFields
        'Select Case g.ColType(field - 1)
        '  Case ATCoSng: g.TextMatrix(row, field - 1) = Format(dbf.Value(field), "###,###,###,###,###.###")
          'Case Else:
        If field = HUCfield Then
          g.TextMatrix(row, field - 1) = Format(CLng(dbf.Value(field)), "00000000")
        Else
          g.TextMatrix(row, field - 1) = dbf.Value(field)
        End If
        'End Select
      Next
      row = row + 1
    Wend
    g.ColsSizeByContents
    g.Visible = True
  End If
End Sub

Private Sub tbrMap_ButtonClick(ByVal Button As MSComctlLib.Button)
  DbgMsg "tbrMap:Click:" & Button.key
  If IsNumeric(Button.Tag) Then
    MapAction CInt(Button.Tag)
  Else
    MsgBox "Non-numeric toolbar button tag in tbrMap_ButtonClick: " & Button.key & " = " & Button.Tag
  End If
End Sub

Private Sub timRefresh_Timer()
  Debug.Print "timRefresh " & timRefresh.Tag
  pctMap(CInt(timRefresh.Tag)).Refresh
End Sub

Private Sub treeDirectory_Click()
  If Not pDirectory_Clicking Then
    pDirectory_Clicking = True
    Debug.Print "treeDir Click"
    Redraw
    pDirectory_Clicking = False
  End If
End Sub

Private Sub treeDirectory_ItemCheck(item As Integer)
  If Not pDirectory_Clicking Then
    pDirectory_Clicking = True
    Debug.Print "treeDir ItemCheck:" & item
    If treeDirectory.Selected(item) Then
      pSkipGrid = False 'Just selected this item, so let grid be redrawn in next Redraw
    Else
      pSkipGrid = True  'Just unselected this item, so skip redrawing grid in next Redraw
    End If
    If LCase(FileExt(treeDirectory.Text)) <> "shp" Then
      treeDirectory.Selected(item) = False
    End If
    pDirectory_Clicking = False
  End If
End Sub

'buttonID can be the name of a button "Pan" or the 1-based index
Public Property Get ButtonVisible(ByVal buttonID As Variant) As Boolean
  On Error GoTo NoSuchButton
  ButtonVisible = tbrMap.Buttons(buttonID).Visible
  Exit Property
NoSuchButton:
  ButtonVisible = False
End Property

Public Property Let ButtonVisible(ByVal buttonID As Variant, ByVal state As Boolean)
  DbgMsg "Let ButtonVisible " & buttonID & " = " & state
  On Error GoTo NoSuchButton
  tbrMap.Buttons(buttonID).Visible = state
  With tbrMap.Buttons(tbrMap.Buttons.count)
    tbrMap.Width = .Left + .Width
  End With
  Exit Property
NoSuchButton:
End Property

Private Sub SelectShape(lyr As Layer, iShape As Long, SelectIt As Boolean)
  Static Selecting As Boolean
  If Not Selecting Then
    Selecting = True
    If iShape > 0 Then
      If SelectIt Then
        lyr.ShapeColor(iShape) = pSelectedColor
      Else
        lyr.ShapeColor(iShape) = 0
      End If
      With agd(pCurMap)
        If .Height > 100 And .Rows >= iShape Then
          If .Selected(iShape, 0) <> SelectIt Then .Selected(iShape, 0) = SelectIt
          If Not .RowIsVisible(iShape) Then .TopRow = iShape
        End If
      End With
      lyr.Render pctMap(pCurMap), iShape
      If pHighlightedShape(pCurMap) <> iShape And pHighlightedShape(pCurMap) > 0 Then
        lyr.Render pctMap(pCurMap), -pHighlightedShape(pCurMap)
      End If
      pHighlightedShape(pCurMap) = 0
    End If
    Selecting = False
  End If
End Sub

Private Sub HighlightShapeNear(X As Single, Y As Single, Optional Selecting As Boolean = False)
  Dim iShape As Long
  Dim tmpLayer As Layer
  Dim tmpDBF As clsDBF
  Dim iLabelField As Long
  
  If treeDirectory.Selected(treeDirectory.ListIndex) Then
    Set tmpLayer = GetMapLayer(treeDirectory.Text, pCurMap)
    If Not tmpLayer Is Nothing Then
      iShape = tmpLayer.IndexNearest(X, Y)
      If Selecting Then
        If iShape > 0 Then
          Select Case tmpLayer.ShapeColor(iShape)
            Case 0:              SelectShape tmpLayer, iShape, True
            Case pSelectedColor: SelectShape tmpLayer, iShape, False
          End Select
        End If
      ElseIf iShape <> pHighlightedShape(pCurMap) Then
        If pHighlightedShape(pCurMap) > 0 Then
          If tmpLayer.ShapeColor(pHighlightedShape(pCurMap)) <> 0 Then
            tmpLayer.Render pctMap(pCurMap), pHighlightedShape(pCurMap)
          Else
            tmpLayer.Render pctMap(pCurMap), -pHighlightedShape(pCurMap)
          End If
        End If
        'If tmpLayer.ShapeColor(iShape) <> 0 Then
        '  pHighlightedShape(pCurMap) = 0
        'Else
          pHighlightedShape(pCurMap) = iShape
          If iShape = 0 Then
            pctMap(pCurMap).ToolTipText = ""
          Else
            tmpLayer.Render pctMap(pCurMap), -iShape
            Set tmpDBF = GetDBF(treeDirectory.Text, pCurMap)
            If Not tmpDBF Is Nothing Then
              tmpDBF.CurrentRecord = iShape
              iLabelField = tmpDBF.FieldNumber(tmpLayer.LabelField)
              If iLabelField > 0 Then
                pctMap(pCurMap).ToolTipText = tmpDBF.Value(iLabelField) ' agd(pCurMap).TextMatrix(iShape, agd(pCurMap).col)
              Else
                pctMap(pCurMap).ToolTipText = ""
              End If
              If agd(pCurMap).Height > 100 Then
                If Not agd(pCurMap).RowIsVisible(iShape) Then
                  agd(pCurMap).TopRow = iShape
                End If
              End If
            End If
          End If
        'End If
      End If
    End If
  Else
    pctMap(pCurMap).ToolTipText = ""
  End If
End Sub

'Called when a toolbar button or menu item is clicked
'cmd is the index of the command (index property of menu = tag of button)
Public Sub MapAction(ByVal cmd&)
  Static LastCommand As Long
  Dim j As Long
  Dim mouseptr As Integer
  Dim b As Buttons
  Dim tmpLayer As Layer
  
  DbgMsg "MapAction " & cmd
  Set b = tbrMap.Buttons
  
  'Un-press a mode button if we were already in that mode
  If cmd = LastCommand Then
    Select Case cmd
      Case 0, 30, 60, 200, 210, 220: cmd = -1 ' Zoom,Pan,Identify,Add,Remove,Move
    End Select
  End If
  
  ClearToolTips
  
  'Set all buttons to unpressed state
  For j = 1 To b.count
    b.item(j).Value = tbrUnpressed
  Next j
  
  mouseptr = vbDefault
  pTrackNearestShape = False
'  pDeletingPoint = False
'  Highlight = ""
'  pMapMovingPoint = 0
'  If RecentlyRefreshedLayer Then
'    pctMap.Refresh
'    RecentlyRefreshedLayer = False
'  End If
  Select Case cmd
  Case -1 'Nothing, just clear buttons and tracking state
    pRectActive = False
    pMouseDownX = 0
    pMouseDownY = 0
  Case 0: DbgMsg "Map Rectangle Zoom Start"
    b("Zoom").Value = tbrPressed
    mouseptr = 99
  Case 10: DbgMsg "Map Zoom In":      MapZoom "In"
  Case 20: DbgMsg "Map Zoom Out":     MapZoom "Out"
  Case 30: DbgMsg "Map Pan Start"
    b("Pan").Value = tbrPressed
    mouseptr = 99
  Case 40: DbgMsg "Map Full Zoom":    MapZoom "Full"
  Case 60: DbgMsg "Map Identify mode"
    If treeDirectory.Selected(treeDirectory.ListIndex) Then
      Set tmpLayer = GetMapLayer(treeDirectory.Text, pCurMap)
      If Not tmpLayer Is Nothing Then
        b("Identify").Value = tbrPressed
        mouseptr = vbArrowQuestion
        pTrackNearestShape = True
      End If
    End If
'    If curLay < 0 Then
'      MsgBox "No Feature Specified to Identify." & vbCr & "Choose a table below."
'    'ElseIf pctMap.Layers(curLay).symbol.SymbolType = 2 Then
'    '  MsgBox "Identify mode is supported for point and line layers only, not areas."
'    Else
'      b("Identify").Value = tbrPressed
'      mouseptr = vbArrowQuestion
'      pTrackNearestShape = True
'      'Dim l&
'      'l = 1
'      'curLay = -1
'      'While l < nLayers And curLay < 0
'      '  If LayerInfo(l).TableIndex = sstLegend.Tab Then curLay = l Else l = l + 1
'      'Wend
'    End If
'  Case 80 'Save .map file
'    Call MapSave
'  Case 90 'Read .map file
'    Call MapGet
'  Case 110: DbgMsg "Map Print"
'    'molt always writes to default printer!
'    Dim lDefB4 As String, lDef As String, lRet As Long, lLand As Boolean
'
'    lDefB4 = String$(128, 0)
'    lRet = GetProfileString("WINDOWS", "DEVICE", "", lDefB4, 127) 'default printer
'
'    On Error GoTo printcancel:
'    comMap.HelpContext = 511
'    comMap.PrinterDefault = True
'    comMap.CancelError = True
'    comMap.ShowPrinter
'    If comMap.Orientation = cdlLandscape Then
'      lLand = True
'    Else
'      lLand = False
'    End If
'    pctMap.PrintMap "Map", "", lLand
'printcancel:
'    On Error GoTo 0
'    lDef = String$(128, 0)
'    lRet = GetProfileString("WINDOWS", "DEVICE", "", lDef, 127)
'    If lDef <> lDefB4 Then 'changed default printer
'      lRet = WriteProfileString("WINDOWS", "DEVICE", lDefB4)
'      lDef = String$(128, 0)
'      lRet = GetProfileString("WINDOWS", "DEVICE", "", lDef, 127) 'this seems to force an update
'      If lDef <> lDefB4 Then 'reset to orig failed
'        MsgBox "Default printer is now:" & vbCr & lDef
'      End If
'    End If
'  Case 200: DbgMsg "Map Add Point"
'    If pctMap.Layers(curLay).symbol.SymbolType = moPointSymbol Then
'      b("Add").Value = tbrPressed
'      mouseptr = vbCrosshair
'    Else
'      MsgBox "Adding shapes is only supported for point layers."
'    End If
'  Case 210: DbgMsg "Map Delete Point"
'  '  b("Remove").Value = tbrPressed
'    If pctMap.Layers(curLay).symbol.SymbolType = moPointSymbol Then
'      'If LayerInfo(curLay).TableIndex = sstLegend.Tab Then
'        pTrackNearestShape = True
'        pDeletingPoint = True
'        mouseptr = vbArrow
'      'Else
'      '  MsgBox "Table for current layer must be visible to delete points."
'      'End If
'    Else
'      MsgBox "Deleting is only supported for point layers."
'    End If
'  Case 220: DbgMsg "Map Move Point Start"
'    If pctMap.Layers(curLay).symbol.SymbolType = moPointSymbol Then
'      Dim result As VbMsgBoxResult
'      result = MsgBox("Do you really want to permanently move points on the map?", vbYesNo, "Confirmation")
'      If result = vbYes Then
'        b("Move").Value = tbrPressed
'        mouseptr = 99
'        pTrackNearestShape = True
'      End If
'    Else
'      MsgBox "Moving shapes is only supported for point layers."
'    End If
'  Case 230 'Up
'  Case 240 'Down
'  Case 250 'Select All
'    SelectAll curLay, True
'  Case 260 'Select None
'    SelectAll curLay, False
'  Case 300: DbgMsg "Map Edit"
'    If frmMapCov.Visible Then
'      frmMapCov.ZOrder
'    Else
'      If LegendIsVisible Then
'        'Selections can get messed up in frmMapCov as layers are rearranged. This should help
'        For j = 0 To nLayers - 1
'          SelectAll j, False
'        Next j
'      End If
'      frmMapCov.Hide 'make sure form is loaded but not visible
'      Set frmMapCov.m = pctMap
'      Set frmMapCov.am = Me
'      On Error Resume Next
'      frmMapCov.Icon = UserControl.Parent.Icon
'      On Error GoTo 0
'Debug.Print "ATML2k:Show frmMapCov"
'      frmMapCov.Show 1
'    End If
'  Case 400 'Branch Select Mode
'    If Me.StreamSelectMode = 1 Then
'      Me.StreamSelectMode = 0
'    Else
'      Me.StreamSelectMode = 1
'    End If
'  Case 410 'Downstream Select Mode
'    If Me.StreamSelectMode = 2 Then
'      Me.StreamSelectMode = 0
'    Else
'      Me.StreamSelectMode = 2
'    End If
  Case Else
    MsgBox "Unknown MapAction: " & cmd
  End Select
  pctMap(pCurMap).MousePointer = mouseptr
  If mouseptr = 99 Then
    Select Case cmd
      Case 0:   pctMap(pCurMap).MouseIcon = imgZoom.Picture
      Case 30:  pctMap(pCurMap).MouseIcon = imgPan.Picture
      Case 220: pctMap(pCurMap).MouseIcon = imgMovePoint.Picture
    End Select
  End If
  tbrMap.Refresh
  LastCommand = cmd
End Sub

Private Sub treeDirectory_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Button = vbRightButton Then
  
  End If
End Sub
