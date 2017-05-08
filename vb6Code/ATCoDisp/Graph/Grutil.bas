Attribute VB_Name = "GrUtil"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants
Public lf As LOGFONT

Public Sub Angtx(outG As Object, _
                 xpos As Single, _
                 ypos As Single, _
                 LabelX As String, _
                 ihv As Long)
  outG.CurrentX = xpos
  outG.CurrentY = ypos
  frmG.GraphPrint LabelX
End Sub

Public Sub GrMark(outG As Object, xpos As Single, ypos As Single, imark As Long, clr As Long)
  'display specified marker on graph
  Dim radius As Single
  Dim lSymbolHeight As Long
  Dim lSymbolWidth As Long
  
  Dim lScaleHeight As Double
  Dim lScaleWidth As Double
  Dim lScaleLeft As Double
  Dim lScaleTop As Double
  Dim lx As Long
  Dim LY As Long
  
  lScaleHeight = outG.ScaleHeight
  lScaleWidth = outG.ScaleWidth
  lScaleLeft = outG.ScaleLeft
  lScaleTop = outG.ScaleTop
  
  outG.CurrentX = xpos
  outG.CurrentY = ypos
  
  'Pixel scale lets us draw more precise markers
  outG.ScaleMode = vbPixels
  
  lx = outG.CurrentX
  LY = outG.CurrentY
  
  lSymbolWidth = (Abs(outG.TextWidth("X")) + Abs(outG.TextHeight("X"))) / 4
  If lSymbolWidth < 2 Then lSymbolWidth = 2
  lSymbolHeight = lSymbolWidth 'Abs(outG.TextHeight("X")) * 2 / 3
    
  Select Case imark
    Case 1 'Small filled dot
      Dim saveFillColor As Long
      Dim saveFillStyle As Long
      saveFillColor = outG.FillColor
      saveFillStyle = outG.FillStyle
      outG.FillColor = clr
      outG.FillStyle = 0 'solid fill
      radius = 0.1 * lSymbolHeight
      If radius < 1 Then radius = 1
      outG.Circle (lx, LY), radius, clr
      outG.FillColor = saveFillColor
      outG.FillStyle = saveFillStyle
      
    Case 2: GoSub DrawPlus
    Case 3: GoSub DrawPlus 'Asterisk is combination of + and x
            GoSub DrawX
    Case 4 'Circle
      radius = 0.5 * lSymbolHeight
      If radius < 2 Then radius = 2
      outG.Circle (lx, LY), radius, clr
    Case 5: GoSub DrawX
  End Select
  
  'Return to normal scale
  outG.Scale (lScaleLeft, lScaleTop)-(lScaleLeft + lScaleWidth, lScaleTop + lScaleHeight)
  'Return to the point we just marked
  outG.CurrentX = xpos
  outG.CurrentY = ypos
  
  Exit Sub

DrawX: 'Could be done with two lines, but last pixel is always left off
  outG.Line (lx, LY)-(lx - lSymbolWidth / 2, LY - lSymbolHeight / 2), clr
  outG.Line (lx, LY)-(lx + lSymbolWidth / 2, LY - lSymbolHeight / 2), clr
  outG.Line (lx, LY)-(lx + lSymbolWidth / 2, LY + lSymbolHeight / 2), clr
  outG.Line (lx, LY)-(lx - lSymbolWidth / 2, LY + lSymbolHeight / 2), clr
  Return
  
DrawPlus: 'Could be done with two lines, but last pixel is always left off
  outG.Line (lx, LY)-(lx, LY - lSymbolHeight / 2), clr
  outG.Line (lx, LY)-(lx, LY + lSymbolHeight / 2), clr
  outG.Line (lx, LY)-(lx - lSymbolWidth / 2, LY), clr
  outG.Line (lx, LY)-(lx + lSymbolWidth / 2, LY), clr
  Return

End Sub

Public Function Rndlow(px As Single) As Single
  'Returns px rounded toward zero to nearest power of ten
  'Returns 0.0 for values less than 1.0E-19
  'for the plotting routines for bug in DISSPLA/PR1ME.

  Dim a As Long
  Dim x As Double
  
  x = Abs(px)
  If x < 1E-19 Then
    Rndlow = 0#
  Else
    a = Int(Log10(x))
    If px < 0# Then
      Rndlow = -(10# ^ a)
    Else
      Rndlow = 10# ^ a
    End If
  End If
End Function
