VERSION 5.00
Begin VB.UserControl ATCoScrollbar 
   BackColor       =   &H80000000&
   ClientHeight    =   2904
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   264
   KeyPreview      =   -1  'True
   ScaleHeight     =   242
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   22
   Begin VB.PictureBox picBuffer 
      Appearance      =   0  'Flat
      AutoRedraw      =   -1  'True
      BackColor       =   &H80000000&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   252
      Left            =   0
      ScaleHeight     =   21
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   22
      TabIndex        =   3
      Top             =   360
      Visible         =   0   'False
      Width           =   264
   End
   Begin VB.PictureBox picBackground 
      Appearance      =   0  'Flat
      AutoRedraw      =   -1  'True
      BackColor       =   &H80000000&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   2412
      Left            =   0
      ScaleHeight     =   201
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   22
      TabIndex        =   2
      TabStop         =   0   'False
      Top             =   240
      Width           =   264
   End
   Begin VB.CommandButton cmdScrollDown 
      Height          =   252
      Left            =   0
      Style           =   1  'Graphical
      TabIndex        =   1
      TabStop         =   0   'False
      Top             =   2640
      Width           =   264
   End
   Begin VB.CommandButton cmdScrollUp 
      Height          =   252
      Left            =   0
      Style           =   1  'Graphical
      TabIndex        =   0
      TabStop         =   0   'False
      Top             =   0
      Width           =   264
   End
End
Attribute VB_Name = "ATCoScrollbar"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'Public Event Scroll(NewValue As Long)
Public Event Change()

Private pEnabled As Boolean
Private pLargeChange As Long
Private pSmallChange As Long
Private pValue As Long
Private pMax As Long
Private pMin As Long
Private pDragEvents As Boolean

'Private pHaveFocus As Boolean
Private pDragging As Boolean
Private pDragThumb As Long
Private pThumbTop As Long
Private pThumbBottom As Long
Private pLastLargeChange As Long

Private Const MinThumbHeight = 8

Public Property Get DragEvents() As Boolean
  DragEvents = pDragEvents
End Property
Public Property Let DragEvents(ByVal NewDragEvents As Boolean)
  pDragEvents = NewDragEvents
End Property

Public Property Get Enabled() As Boolean
  Enabled = pEnabled
End Property
Public Property Let Enabled(ByVal NewEnabled As Boolean)
  pEnabled = NewEnabled
  DrawArrows
  cmdScrollUp.Enabled = pEnabled
  cmdScrollDown.Enabled = pEnabled
End Property

Public Property Get Max() As Long
  Max = pMax
End Property
Public Property Let Max(ByVal NewMax As Long)
  pMax = NewMax
End Property

Public Property Get Min() As Long
  Min = pMin
End Property
Public Property Let Min(ByVal NewMin As Long)
  pMin = NewMin
End Property

Public Property Get LargeChange() As Long
  LargeChange = pLargeChange
End Property
Public Property Let LargeChange(ByVal NewValue As Long)
  pLargeChange = NewValue
End Property

Public Property Get SmallChange() As Long
  SmallChange = pSmallChange
End Property
Public Property Let SmallChange(ByVal NewValue As Long)
  pSmallChange = NewValue
End Property

Public Property Get Value() As Long
  Value = pValue
End Property
Public Property Let Value(ByVal NewValue As Long)
  If NewValue < pMin Then NewValue = pMin
  If NewValue > pMax Then NewValue = pMax
  If pValue <> NewValue Then
    pValue = NewValue
    DrawThumb
    If pDragEvents Or Not pDragging Then RaiseEvent Change 'Scroll(pValue)
  End If
End Property

Private Sub cmdScrollDown_Click()
  Value = pValue + SmallChange
End Sub

Private Sub cmdScrollUp_Click()
  Value = pValue - SmallChange
End Sub

Private Sub picBackground_DblClick()
  If pLastLargeChange <> 0 Then Value = pValue + pLastLargeChange
End Sub

'Private Sub UserControl_GotFocus()
'  Debug.Print "Got Focus"
'  pHaveFocus = True
'  DrawThumb
'End Sub
'
'Private Sub UserControl_LostFocus()
'  Debug.Print "Lost Focus"
'  pHaveFocus = False
'  DrawThumb
'End Sub

Private Sub UserControl_KeyDown(KeyCode As Integer, Shift As Integer)
  Select Case KeyCode
    Case vbKeyUp, vbKeyLeft:    Value = pValue - SmallChange
    Case vbKeyDown, vbKeyRight: Value = pValue + SmallChange
    Case vbKeyPageUp:           Value = pValue - LargeChange
    Case vbKeyPageDown:         Value = pValue + LargeChange
  End Select
End Sub

Private Sub picBackground_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
  If Y < pThumbTop Then
    pLastLargeChange = -LargeChange
  ElseIf Y > pThumbBottom Then
    pLastLargeChange = LargeChange
  Else
    pLastLargeChange = 0
    pDragThumb = Y - pThumbTop
    pDragging = True
  End If
  If pLastLargeChange <> 0 Then Value = pValue + pLastLargeChange
End Sub

Private Sub picBackground_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
  Dim Denom As Double
  If pDragging Then
    Denom = (picBackground.Height - (pThumbBottom - pThumbTop))
    If Abs(Denom) > 0.0001 Then Value = (Y - pDragThumb) * (pMax - pMin) / Denom + pMin
    'picBackground.Line (0, Y)-(20, Y), vbBlue
  End If
End Sub

Private Sub picBackground_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)
  pDragging = False
  If Not pDragEvents Then RaiseEvent Change 'Scroll(pValue)
End Sub

Private Sub UserControl_InitProperties()
  pEnabled = True
  pValue = 1
  pMin = 1
  pMax = 100
  pSmallChange = 1
  pLargeChange = 10
  pDragEvents = True
End Sub

Private Sub UserControl_ReadProperties(PropBag As PropertyBag)
  Enabled = PropBag.ReadProperty("Enabled", True)
  Min = PropBag.ReadProperty("Min", 1)
  Max = PropBag.ReadProperty("Max", 100)
  SmallChange = PropBag.ReadProperty("SmallChange", 1)
  LargeChange = PropBag.ReadProperty("LargeChange", 10)
  DragEvents = PropBag.ReadProperty("DragEvents", True)
  Value = PropBag.ReadProperty("Value", 1)
End Sub

Private Sub UserControl_WriteProperties(PropBag As PropertyBag)
  PropBag.WriteProperty "Enabled", Enabled
  PropBag.WriteProperty "Min", Min
  PropBag.WriteProperty "Max", Max
  PropBag.WriteProperty "SmallChange", SmallChange
  PropBag.WriteProperty "LargeChange", LargeChange
  PropBag.WriteProperty "DragEvents", DragEvents
  PropBag.WriteProperty "Value", Value
End Sub

Private Sub UserControl_Resize()
  Static LastWidth As Long
  Dim H&, W&
  H = ScaleHeight
  W = ScaleWidth
  cmdScrollUp.Width = W
  cmdScrollDown.Width = W
  picBackground.Width = W
  
  cmdScrollUp.Height = W
  cmdScrollDown.Height = W
  picBackground.Top = W + 1
  cmdScrollDown.Top = H - W
  If picBackground.Top < cmdScrollDown.Top Then
    picBackground.Height = cmdScrollDown.Top - picBackground.Top
  End If
  If LastWidth <> W Then DrawArrows: LastWidth = W
  DrawThumb
End Sub

Private Sub DrawThumb()
  Dim H As Long, W As Long
  W = picBackground.Width - 1
  H = picBackground.Height
  If pMax - pLargeChange <= pMin Then
    pThumbTop = 0
    pThumbBottom = H
  Else
    pThumbTop = H * (pValue - pMin) / (pMax - pMin + pLargeChange)
    pThumbBottom = pThumbTop + H * pLargeChange / (pMax + pLargeChange - pMin)
  End If
  If pThumbBottom - pThumbTop < MinThumbHeight Then
    pThumbBottom = pThumbTop + MinThumbHeight
  End If
  
  picBackground.BackColor = vbWindowBackground
  
  'top edge
  picBackground.Line (0, pThumbTop)-(W, pThumbTop), vb3DLight
  picBackground.Line (1, pThumbTop + 1)-(W - 1, pThumbTop + 1), vb3DHighlight
  
  'left edge
  picBackground.Line (0, pThumbTop)-(0, pThumbBottom), vb3DLight
  picBackground.Line (1, pThumbTop + 1)-(1, pThumbBottom), vb3DHighlight
  
  'right edge
  picBackground.Line (W - 1, pThumbTop + 1)-(W - 1, pThumbBottom - 1), vb3DShadow
  picBackground.Line (W, pThumbTop)-(W, pThumbBottom), vb3DDKShadow
  
  'bottom edge
  picBackground.Line (1, pThumbBottom - 1)-(W, pThumbBottom - 1), vb3DShadow
  picBackground.Line (0, pThumbBottom)-(W + 1, pThumbBottom), vb3DDKShadow

  'Thumb
  'If pHaveFocus Then
  '  picBackground.ForeColor = vb3DShadow
  'Else
  '  picBackground.ForeColor = vbScrollBars
  'End If
  picBackground.Line (2, pThumbTop + 2)-(W - 2, pThumbBottom - 2), vbScrollBars, BF

End Sub

Private Sub DrawArrows()
  Dim W&
  W = ScaleWidth
  Dim X As Long, Y As Long
  Dim ArrowHeight As Long
  Dim maxArrowHeight As Long
  If pEnabled Then
    picBuffer.ForeColor = vbButtonText
  Else
    picBuffer.ForeColor = vbGrayText
  End If
  picBuffer.BackColor = cmdScrollUp.BackColor    'picBuffer.Cls
  maxArrowHeight = Fix(W / 2) - 4
  picBuffer.Width = W - 8
  picBuffer.Height = maxArrowHeight
  Y = maxArrowHeight
  ArrowHeight = 0
  X = 0
  While ArrowHeight <= maxArrowHeight
    ArrowHeight = ArrowHeight + 1
    picBuffer.Line (X, Y)-(X, Y - ArrowHeight)
    X = X + 1
  Wend
  While ArrowHeight > 0
    ArrowHeight = ArrowHeight - 1
    picBuffer.Line (X, Y)-(X, Y - ArrowHeight)
    X = X + 1
  Wend
  cmdScrollUp.Picture = picBuffer.Image
  
  picBuffer.BackColor = cmdScrollDown.BackColor    'picBuffer.Cls
  Y = 0
  ArrowHeight = 0
  X = 0
  While ArrowHeight <= maxArrowHeight
    ArrowHeight = ArrowHeight + 1
    picBuffer.Line (X, Y)-(X, Y + ArrowHeight)
    X = X + 1
  Wend
  While ArrowHeight > 0
    ArrowHeight = ArrowHeight - 1
    picBuffer.Line (X, Y)-(X, Y + ArrowHeight)
    X = X + 1
  Wend
  cmdScrollDown.Picture = picBuffer.Image
End Sub
