Attribute VB_Name = "MoLtUtil"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'Only works if both symbols are of same type
Public Sub copySymbol(fSym As MapObjectsLT.Symbol, tSym As MapObjectsLT.Symbol)
  If tSym.SymbolType = fSym.SymbolType Then
    tSym.Style = fSym.Style
    tSym.Color = fSym.Color
    If (fSym.SymbolType <> moLineSymbol) Then
      tSym.Outline = fSym.Outline
      tSym.OutlineColor = fSym.OutlineColor
    End If
    If fSym.SymbolType = moPointSymbol Then
      tSym.Rotation = fSym.Rotation
      tSym.Size = fSym.Size
      If fSym.Style = moTrueTypeMarker Then
        tSym.CenterOnAscent = fSym.CenterOnAscent
        tSym.CharacterIndex = fSym.CharacterIndex
        tSym.Font = fSym.Font
      End If
    End If
  End If
End Sub

Public Function TextOrNumericStyle&(ByVal stylestr$)
  Dim retval&, styl
  retval = 0 '= moCircleMarker = moSolidLine = moSolidFill
  styl = UCase(stylestr)
  If IsNumeric(styl) Then
    retval = styl
  Else
    Select Case Left(styl, 3)
      'Marker symbol styles
      Case "CIR": retval = moCircleMarker
      Case "SQU": retval = moSquareMarker
      Case "TRI": retval = moTriangleMarker
      Case "CRO": retval = moCrossMarker
      Case "TRU": retval = moTrueTypeMarker
  
      'Line styles
      Case "SOL": retval = moSolidLine 'Also matches solid fill. Fortunately, moSolidLine = moSolidFill
      Case "DAS":
        If Left(styl, 10) = "DASHDOTDOT" Then
          retval = moDashDotDotLine
        ElseIf Left(styl, 7) = "DASHDOT" Then
          retval = moDashDotLine
        Else
          retval = moDashLine
        End If
      Case "DOT": retval = moDotLine
  
      'Fill styles
      Case "TRA": retval = moTransparentFill
      Case "HOR": retval = moHorizontalFill
      Case "VER": retval = moVerticalFill
      Case "UPW": retval = moUpwardDiagonalFill
      Case "DOW": retval = moDownwardDiagonalFill
      Case "CRO": retval = moCrossFill
      Case "DOW": retval = moDownwardDiagonalFill
      Case "DIA": retval = moDiagonalCrossFill
      Case "LIG": retval = moLightGrayFill
      Case "DAR": retval = moDarkGrayFill
      Case Else:  MsgBox "Style not recognized: " & stylestr
    End Select
  End If
  TextOrNumericStyle = retval
End Function

Public Function StyleName$(ByVal symType&, ByVal styl&)
  Dim retval$
  retval = 0
  If symType = moPointSymbol Then 'Marker symbol styles
    Select Case styl
      Case moCircleMarker:   retval = "Circle"
      Case moSquareMarker:   retval = "Square"
      Case moTriangleMarker: retval = "Triangle"
      Case moCrossMarker:    retval = "Cross"
      Case moTrueTypeMarker: retval = "TrueType"
    End Select
  ElseIf symType = moLineSymbol Then  'Line styles
    Select Case styl
      Case moSolidLine:      retval = "SolidLine"
      Case moDashDotDotLine: retval = "DashDotDotLine"
      Case moDashDotLine:    retval = "DashDotLine"
      Case moDotLine:        retval = "DottedLine"
      Case moDashLine:       retval = "DashedLine"
    End Select
  ElseIf symType = moFillSymbol Then  'Fill styles
    Select Case styl
      Case moSolidFill:             retval = "Solid"
      Case moTransparentFill:       retval = "Transparent"
      Case moHorizontalFill:        retval = "HorizontalFill"
      Case moVerticalFill:          retval = "VerticalFill"
      Case moUpwardDiagonalFill:    retval = "UpwardDiagonalFill"
      Case moDownwardDiagonalFill:  retval = "DownwardDiagonalFill"
      Case moCrossFill:             retval = "CrossFill"
      Case moDownwardDiagonalFill:  retval = "DownwardDiagonalFill"
      Case moDiagonalCrossFill:     retval = "DiagonalCrossFill"
      Case moLightGrayFill:         retval = "LightGrayFill"
      Case moDarkGrayFill:          retval = "DarkGrayFill"
    End Select
  End If
  StyleName = retval
End Function

Public Function TextOrNumericAlignment&(ByVal align$)
  Dim retval, u$
  u = UCase(align)
  retval = moAlignTop
  If IsNumeric(u) Then
    retval = CLng(u)
  Else
    Select Case u
      Case "TOP":      retval = moAlignTop
      Case "BOTTOM":   retval = moAlignBottom
      Case "LEFT":     retval = moAlignLeft
      Case "RIGHT":    retval = moAlignRight
      Case "CENTER":   retval = moAlignCenter
      Case "BASELINE": retval = moAlignBaseline
    End Select
  End If
  TextOrNumericAlignment = retval
End Function


