Attribute VB_Name = "UtilColor"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private colorDatabase As Database
Private Const grayBasename = "gray" 'base name for shades of gray (gray0..gray255)
Private Const grayNameNumStart = 5  'len(grayBasename) + 1
Private Const forceKnownColor = True

Private nColorRules As Long
Private colorMatchingRules() As String
Private colorRamp() As Long
Private colorsUsed() As Long
Private MatchingColorsFilename As String

Public Sub InitMatchingColors(Filename As String)
  ReDim colorsUsed(0)
  On Error GoTo NeverMind
  If Len(Filename) > 0 Then
    If Len(Dir(Filename)) > 0 Then MatchingColorsFilename = Filename
  End If
  If Len(MatchingColorsFilename) > 0 Then
    Dim LineNumber As Long
    Dim buf As String, oneLine As String
    Dim CRpos As Long, LFpos As Long
    
    nColorRules = 0
    ReDim colorRamp(0)
    ReDim colorMatchingRules(0)
    buf = WholeFileString(MatchingColorsFilename)
    
    While Len(buf) > 0
      CRpos = InStr(buf, vbCr)
      LFpos = InStr(buf, vbLf)
      oneLine = ""
      If CRpos = 1 Or LFpos = 1 Then
        buf = Mid(buf, 2)
      ElseIf CRpos = 0 And LFpos = 0 Then
        oneLine = buf
        buf = ""
      ElseIf CRpos = 0 Then
        oneLine = Left(buf, LFpos - 1)
        buf = Mid(buf, LFpos + 1)
      ElseIf LFpos = 0 Or CRpos < LFpos Then
        oneLine = Left(buf, CRpos - 1)
        buf = Mid(buf, CRpos + 1)
      Else
        oneLine = Left(buf, LFpos - 1)
        buf = Mid(buf, LFpos + 1)
      End If
      oneLine = Trim(oneLine)
      If Len(oneLine) > 0 Then
        nColorRules = nColorRules + 1
        ReDim Preserve colorRamp(nColorRules)
        ReDim Preserve colorMatchingRules(nColorRules)
        colorRamp(nColorRules) = TextOrNumericColor(StrRetRem(oneLine))
        colorMatchingRules(nColorRules) = oneLine
      End If
    Wend
  End If
NeverMind:
End Sub

Public Function GetMatchingColor(Optional ByVal Specification As String = "") As Long
  Dim retval As Long
  If Len(MatchingColorsFilename) = 0 Then
    GoTo GetRandomColor
  ElseIf Len(Specification) = 0 Then
    GoTo NextUnusedColor
  Else
    Dim rule As Long, used As Long
    For rule = 1 To nColorRules
      If PatternMatch(LCase(Specification), LCase(colorMatchingRules(rule))) Then
        For used = 1 To UBound(colorsUsed)
          If colorsUsed(used) = colorRamp(rule) Then GoTo NextPatternMatch
        Next
        GoTo FoundColor
      End If
NextPatternMatch:
    Next
NextUnusedColor:
    For rule = 1 To nColorRules
      For used = 1 To UBound(colorsUsed)
        If colorsUsed(used) = colorRamp(rule) Then GoTo NextRule
      Next
      GoTo FoundColor
NextRule:
    Next
  End If
GetRandomColor:
  'Did not find a matching color or an unused color in the ramp - find a random one
  GetMatchingColor = rgb(64 + Rnd * 128, 64 + Rnd * 128, 64 + Rnd * 128)
  Exit Function

FoundColor:
  GetMatchingColor = colorRamp(rule)
  ReDim Preserve colorsUsed(UBound(colorsUsed) + 1)
  colorsUsed(UBound(colorsUsed)) = colorRamp(rule)
  
End Function

Private Function colorDB() As Database
  Static AlreadyReportedErrOpen As Boolean
  Static openedDB As Boolean
  If Not openedDB Then
    Dim DBpath As String
    Dim ff As New ATCoFindFile
    On Error GoTo erropen
    
    ff.SetRegistryInfo "ATCoCtl", "ATCoRend", "Path"
    ff.SetDialogProperties "Please locate 'ATCoRend.mdb' in a writable folder", "ATCoRend.mdb"
    DBpath = ff.GetName
    
    If Len(DBpath) > 0 Then
      If Len(Dir(DBpath)) > 0 Then
        Set colorDatabase = OpenDatabase(DBpath, False, True)
        openedDB = True
      End If
    End If
  End If
  Set colorDB = colorDatabase
  Exit Function
erropen:
  If Not AlreadyReportedErrOpen Then
    MsgBox "Error opening color database '" & DBpath & "'" & vbCr & err.Description
    AlreadyReportedErrOpen = True
  End If
  Set colorDB = Null
End Function

'For testing color database or cycling through available named colors
'Goes forward one entry in the color database (or backward if fwd=false)
'Starts at prevColorName
'sets nextColorName and nextColor
Public Sub testColor(fwd As Boolean, prevColorName$, ByRef nextColorName$, ByRef nextColor As OLE_COLOR)
  Static rs As Recordset
  Static rsOpen As Boolean

  Dim c$

  c = LCase(Trim(prevColorName))
  nextColor = -1
  If IsNumeric(c) Then 'They gave us a number, not a name, so default to white
    nextColor = vbBlack
    nextColorName = "black"
  Else
    If c = "black" And fwd Then
      nextColor = rgb(1, 1, 1): nextColorName = grayBasename & "1"
    ElseIf c = "black1" And Not fwd Then
      nextColor = rgb(254, 254, 254): nextColorName = grayBasename & "254"
    ElseIf Left(c, grayNameNumStart - 1) = LCase(grayBasename) Then
      Dim r$
      r = Mid(c, grayNameNumStart)
      If IsNumeric(r) Then
        If fwd Then r = r + 1 Else r = r - 1
        Select Case r
          Case Is <= 0:   nextColor = vbBlack:      nextColorName = "black"
          Case Is >= 255: nextColor = 1:            nextColorName = "black1"
          Case Else:      nextColor = rgb(r, r, r): nextColorName = grayBasename & r
        End Select
      End If
    End If
    If nextColor = -1 Then
      If Not rsOpen Then
        On Error GoTo erropen
        Dim db As Database
        Set db = colorDB
        If Not IsNull(db) Then
          Set rs = db.OpenRecordset("ColorLowercase", dbOpenDynaset, dbReadOnly, dbReadOnly)
          rsOpen = True
        End If
      End If
      If rsOpen Then
        rs.FindFirst "NameL='" & c & "'"
        If rs.NoMatch Then
          nextColor = vbBlack
          nextColorName = "black"
        Else
          If fwd Then
            rs.MoveNext
            If rs.EOF Then rs.MoveFirst
          Else
            rs.MovePrevious
            If rs.BOF Then rs.MoveLast
          End If
          nextColor = rs("Value")
          nextColorName = rs("NameL")
        End If
      End If
    End If
  End If
  Exit Sub
erropen:
  MsgBox "Error opening color table" & vbCr & err.Description
End Sub

Public Function TextOrNumericColor&(ByVal colr$)
  Static AlreadyReportedError As Boolean
  Static rs As Recordset
  Static rsOpen As Boolean
  
  Dim c$
  
  c = LCase(Trim(colr))
  TextOrNumericColor = -1
  If IsNumeric(c) Then
    TextOrNumericColor = CLng(c)
  Else
    If LCase(Left(colr, grayNameNumStart - 1)) = LCase(grayBasename) Then
      Dim r$
      r = Mid(colr, grayNameNumStart)
      If IsNumeric(r) Then TextOrNumericColor = rgb(r, r, r)
    End If
    If TextOrNumericColor = -1 Then
      If Not rsOpen Then
        On Error GoTo erropen
        Dim db As Database
        Set db = colorDB
        If Not IsNull(db) Then
          Set rs = db.OpenRecordset("ColorLowercase", dbOpenDynaset, dbReadOnly, dbReadOnly)
          rsOpen = True
        End If
      End If
      If rsOpen Then
        rs.FindFirst "NameL='" & c & "'"
        If rs.NoMatch Then
          TextOrNumericColor = &H808080 'default to gray
        Else
          TextOrNumericColor = rs("Value")
        End If
      End If
    End If
  End If
  Exit Function
erropen:
  If Not AlreadyReportedError Then
    MsgBox "Error opening color table" & vbCr & err.Description
    AlreadyReportedError = True
  End If
End Function

Public Function colorName$(ByVal rgb&)
  Static rs As Recordset
  Static rsOpen As Boolean
  Static HadErrOpen As Boolean
  Dim dlg As CommonDialog
  
  Dim retval$, r&, g&, b&
  
  r = Redness(rgb)
  g = Greenness(rgb)
  b = Blueness(rgb)
  
  'If it is not black or white, check for gray
  If rgb <> vbWhite And rgb <> 0 And rgb <> 1 Then
    If r = g And g = b Then
      colorName = grayBasename & r
      Exit Function
    End If
  End If
  'rgb = rgb And &HFFFFFF
  'If rgb = rgb And &HFFFFFF Then '24-bit color
  If Not rsOpen Then
    On Error GoTo erropen
    Dim db As Database
    Set db = colorDB
    If Not IsNull(db) Then
      Set rs = db.OpenRecordset("Color", dbOpenDynaset, dbReadOnly, dbReadOnly)
      rsOpen = True
    End If
  End If
  If rsOpen Then
    rs.FindFirst "Value=" & rgb & ""
    If rs.NoMatch Then
      If rgb = rgb And &HFFFFFF Then
        If forceKnownColor Then 'Search for closest color in database
          Dim thisDist!, minDist!, minDistName$
          Dim thisColor&, r1&, b1&, g1&
          rs.MoveFirst
          minDist = 255 ^ 2 * 3
          While Not rs.EOF
            thisColor = rs("Value")
            r1 = Redness(thisColor)
            b1 = Blueness(thisColor)
            g1 = Greenness(thisColor)
            thisDist = (r - r1) ^ 2 + (b - b1) ^ 2 + (g - g1) ^ 2
            If thisDist < minDist Then
              minDist = thisDist
              minDistName = rs("Name")
            End If
            rs.MoveNext
          Wend
          retval = minDistName
        Else
          GoTo SetHexValue
        End If
      Else 'try again with 24-bit value
        retval = colorName(rgb And &HFFFFFF)
      End If
    Else
      retval = rs("Name")
    End If
  End If
'  End If
  colorName = retval
  Exit Function
erropen:
  If Not HadErrOpen Then MsgBox "Error opening color table." & vbCr & "Using hex values instead of color names." & vbCr & err.Description
  HadErrOpen = True
  GoTo SetHexValue
SetHexValue:
  retval = Hex(rgb)
  If Len(retval) < 8 Then retval = String(8 - Len(retval), "0") & retval
  colorName = "&H" & retval
End Function

'Returns a number between 0 and 255 indicating how much red is in this color
Public Function Redness&(clr As OLE_COLOR)
  Redness = clr And &HFF
End Function

Public Function Greenness&(clr As OLE_COLOR)
  Greenness = (clr And 65280) / &H100 '65280 = FF00 = green, VB thinks FF00 = -256
End Function

Public Function Blueness&(clr As OLE_COLOR)
  Blueness = (clr And &HFF0000) / &H10000
End Function

'Returns a number between 0 (black) and 1 (white)
Public Function Brightness!(clr As OLE_COLOR)
  Brightness = (Redness(clr) + Greenness(clr) + Blueness(clr)) / 765#
End Function

'Private Sub Obsolete()
'  Dim c$, retval&, rgb&
'    Select Case c
'      Case "BLACK":        retval = 0        'moBlack
'      Case "RED":          retval = 255      'moRed
'      Case "GREEN":        retval = 65280    'moGreen
'      Case "BLUE":         retval = 16711680 'moBlue
'      Case "MAGENTA":      retval = 16711935 'moMagenta
'      Case "CYAN":         retval = 16776960 'moCyan
'      Case "WHITE":        retval = 16777215 'moWhite
'      Case "LIGHTGRAY":    retval = 12632256 'moLightGray
'      Case "DARKGRAY":     retval = 4210752  'moDarkGray
'      Case "GRAY", "GREY": retval = 8421504  'moGray
'      Case "PALEYELLOW":   retval = 13697023 'moPaleYellow
'      Case "LIGHTYELLOW":  retval = 8454143  'moLightYellow
'      Case "YELLOW":       retval = 65535    'moYellow
'      Case "LIMEGREEN":    retval = 12639424 'moLimeGreen
'      Case "TEAL":         retval = 1421440  'moTeal
'      Case "DARKGREEN":    retval = 32768    'moDarkGreen
'      Case "MAROON":       retval = 128      'moMaroon
'      Case "PURPLE":       retval = 8388736  'moPurple
'      Case "ORANGE":       retval = 33023    'moOrange
'      Case "KHAKI":        retval = 7051175  'moKhaki
'      Case "OLIVE":        retval = 32896    'moOlive
'      Case "DARKRED":      retval = 4210816  'moBrown
'      Case "NAVY":         retval = 8404992  'moNavy
'      Case "LIGHTCORAL":   retval = 8421631
'      Case "DARKBROWN":    retval = 64
'      Case "DARKORANGE":   retval = 4227327
'      Case "BROWN":        retval = 16512
'      Case "PALEGREEN":    retval = 8454016
'      Case "GREENYELLOW":  retval = 65408
'      Case "FORESTGREEN":  retval = 16384
'      Case "TAN":          retval = 4227200
'      Case "LIGHTGREEN":   retval = 8453888
'      Case "BRIGHTGREEN":  retval = 4259584
'      Case "BLUESMOKE":    retval = 8421376
'      Case "GREENBLUE":    retval = 4227072
'      Case "DARKGREENBLUE": retval = 4210688
'      Case "LIGHTCYAN":    retval = 16777088
'      Case "DARKBLUE":     retval = 8388608
'      Case "GREENSMOKE":   retval = 8421440
'      Case "LIGHTBLUE":    retval = 16744448
'      Case "SLATE":        retval = 12615680
'      Case "LAVENDER":     retval = 16744576
'      Case "ROYALBLUE":    retval = 10485760
'      Case "MIDNIGHTBLUE": retval = 4194304
'      Case "PINK":         retval = 12615935
'      Case "PALEPURPLE":   retval = 12615808
'      Case "REDPURPLE":    retval = 4194432
'      Case "PLUM":         retval = 4194368
'      Case "BRIGHTPURPLE": retval = 16711808
'      Case "DARKPURPLE":   retval = 8388672
'      Case "LIGHTMAGENTA": retval = 12508638
'      Case "HOTPINK":      retval = 8388863
'
'      Case "VBSCROLLBARS":           retval = vbScrollBars
'      Case "VBDESKTOP":              retval = vbDesktop
'      Case "VBACTIVETITLEBAR":       retval = vbActiveTitleBar
'      Case "VBINACTIVETITLEBAR":     retval = vbInactiveTitleBar
'      Case "VBMENUBAR":              retval = vbMenuBar
'      Case "VBWINDOWBACKGROUND":     retval = vbWindowBackground
'      Case "VBWINDOWFRAME":          retval = vbWindowFrame
'      Case "VBMENUTEXT":             retval = vbMenuText
'      Case "VBWINDOWTEXT":           retval = vbWindowText
'      Case "VBTITLEBARTEXT":         retval = vbTitleBarText
'      Case "VBACTIVEBORDER":         retval = vbActiveBorder
'      Case "VBINACTIVEBORDER":       retval = vbInactiveBorder
'      Case "VBAPPLICATIONWORKSPACE": retval = vbApplicationWorkspace
'      Case "VBHIGHLIGHT":            retval = vbHighlight
'      Case "VBHIGHLIGHTTEXT":        retval = vbHighlightText
'      Case "VBBUTTONFACE":           retval = vbButtonFace
'      Case "VBBUTTONSHADOW":         retval = vbButtonShadow
'      Case "VBGRAYTEXT":             retval = vbGrayText
'      Case "VBBUTTONTEXT":           retval = vbButtonText
'      Case "VBINACTIVECAPTIONTEXT":  retval = vbInactiveCaptionText
'      Case "VB3DHIGHLIGHT":          retval = vb3DHighlight
'      Case "VB3DDKSHADOW":           retval = vb3DDKShadow
'      Case "VB3DLIGHT":              retval = vb3DLight
'      Case "VBINFOTEXT":             retval = vbInfoText
'      Case "VBINFOBACKGROUND":       retval = vbInfoBackground
      
'      Case Else: MsgBox "Color not recognized: " & colr
'    End Select
    
'  If rgb > vbWhite Or rgb < 0 Then 'system color
'    Select Case rgb
'      Case vbScrollBars:           retval = "vbScrollBars"
'      Case vbDesktop:              retval = "vbDesktop"
'      Case vbActiveTitleBar:       retval = "vbActiveTitleBar"
'      Case vbInactiveTitleBar:     retval = "vbInactiveTitleBar"
'      Case vbMenuBar:              retval = "vbMenuBar"
'      Case vbWindowBackground:     retval = "vbWindowBackground"
'      Case vbWindowFrame:          retval = "vbWindowFrame"
'      Case vbMenuText:             retval = "vbMenuText"
'      Case vbWindowText:           retval = "vbWindowText"
'      Case vbTitleBarText:         retval = "vbTitleBarText"
'      Case vbActiveBorder:         retval = "vbActiveBorder"
'      Case vbInactiveBorder:       retval = "vbInactiveBorder"
'      Case vbApplicationWorkspace: retval = "vbApplicationWorkspace"
'      Case vbHighlight:            retval = "vbHighlight"
'      Case vbHighlightText:        retval = "vbHighlightText"
'      Case vbButtonFace:           retval = "vbButtonFace"
'      Case vbButtonShadow:         retval = "vbButtonShadow"
'      Case vbGrayText:             retval = "vbGrayText"
'      Case vbButtonText:           retval = "vbButtonText"
'      Case vbInactiveCaptionText:  retval = "vbInactiveCaptionText"
'      Case vb3DHighlight:          retval = "vb3DHighlight"
'      Case vb3DDKShadow:           retval = "vb3DDKShadow"
'      Case vb3DLight:              retval = "vb3DLight"
'      Case vbInfoText:             retval = "vbInfoText"
'      Case vbInfoBackground:       retval = "vbInfoBackground"
'    End Select
'  End If
    
    
'    Select Case rgb
'      Case 0, 1:     retval = "Black"
'      Case 255:      retval = "Red"
'      Case 65280:    retval = "Green"
'      Case 16711680: retval = "Blue"
'      Case 16711935: retval = "Magenta"
'      Case 16776960: retval = "Cyan"
'      Case 16777215: retval = "White"
'      Case 12632256: retval = "LightGray"
'      Case 4210752:  retval = "DarkGray"
'      Case 8421504:  retval = "Gray"
'      Case 13697023: retval = "PaleYellow"
'      Case 8454143:  retval = "LightYellow"
'      Case 65535:    retval = "Yellow"
'      Case 12639424: retval = "LimeGreen"
'      Case 1421440:  retval = "Teal"
'      Case 32768:    retval = "DarkGreen"
'      Case 128:      retval = "Maroon"
'      Case 8388736:  retval = "Purple"
'      Case 33023:    retval = "Orange"
'      Case 7051175:  retval = "Khaki"
'      Case 32896:    retval = "Olive"
'      Case 4210816:  retval = "DarkRed"
'      Case 8404992:  retval = "Navy"
'      Case 8421631:  retval = "LightCoral"
'      Case 64:       retval = "DarkBrown"
'      Case 4227327:  retval = "DarkOrange"
'      Case 16512:    retval = "Brown"
'      Case 8454016:  retval = "PaleGreen"
'      Case 65408:    retval = "GreenYellow"
'      Case 16384:    retval = "ForestGreen"
'      Case 4227200:  retval = "Tan"
'      Case 8453888:  retval = "LightGreen"
'      Case 4259584:  retval = "BrightGreen"
'      Case 8421376:  retval = "BlueSmoke"
'      Case 4227072:  retval = "GreenBlue"
'      Case 4210688:  retval = "DarkGreenBlue"
'      Case 16777088: retval = "LightCyan"
'      Case 8388608:  retval = "DarkBlue"
'      Case 8421440:  retval = "GreenSmoke"
'      Case 16744448: retval = "LightBlue"
'      Case 12615680: retval = "Slate"
'      Case 16744576: retval = "Lavender"
'      Case 10485760: retval = "RoyalBlue"
'      Case 4194304:  retval = "MidnightBlue"
'      Case 12615935: retval = "Pink"
'      Case 12615808: retval = "PalePurple"
'      Case 4194432:  retval = "RedPurple"
'      Case 4194368:  retval = "Plum"
'      Case 16711808: retval = "BrightPurple"
'      Case 8388672:  retval = "DarkPurple"
'      Case 16744703: retval = "LightMagenta"
'      Case 8388863:  retval = "HotPink"
'      Case Else:     GoTo SetHexValue
'    End Select
'
'End Sub

'Public Function add256GrayToColorDB()
'  Dim r&, g&, b&, colorName$, rgbColor&
'  Dim db As Database, tb As Recordset
'  Set db = OpenDatabase("c:\windows\system\ATCoRend.mdb")
'  Set tb = db.OpenRecordset("Color", dbOpenTable, dbDenyWrite, dbOptimistic)
'  For r = 0 To 255
'    rgbColor = rgb(r, r, r)
'    tb.AddNew
'    tb.Fields("Name") = "gray" & r
'    tb.Fields("Value") = rgbColor
'    tb.Update
'  Next r
'  tb.Close
'  db.Close
'End Function

'Reads rgb.txt (X windows color database) and writes into Access database
'Public Function initColorDB()
'  Dim rgbFile&, rgbLine$
'  Dim r&, g&, b&, colorName$, lastName$, rgbColor&
'  Dim db As Database, tb As Recordset
'  Set db = OpenDatabase("c:\windows\system\ATCoRend.mdb")
'  Set tb = db.OpenRecordset("Color", dbOpenTable, dbDenyWrite, dbOptimistic)
'  rgbFile = FreeFile(0)
'  Open "c:\rgb.txt" For Input As rgbFile
'  While Not EOF(rgbFile)
'    Line Input #rgbFile, rgbLine
'    If IsNumeric(Trim(Left(rgbLine, 3))) Then
'      colorName = Trim(Mid(rgbLine, 12))
'      While (Asc(colorName) < 65)
'        colorName = Mid(colorName, 2)
'      Wend
'      'Filter out duplicate color names in rgb.txt with internal spaces or differences in capitalization
'      If InStr(colorName, " ") = 0 _
'         And UCase(colorName) <> UCase(lastName) Then
'        lastName = colorName
'        r = Trim(Left(rgbLine, 3))
'        g = Trim(Mid(rgbLine, 5, 3))
'        b = Trim(Mid(rgbLine, 9, 3))
'        rgbColor = rgb(r, g, b)
'        tb.AddNew
'        tb.Fields("Name") = colorName
'        tb.Fields("Value") = rgbColor
'        tb.Update
'      End If
'    End If
'  Wend
'  Close rgbFile
'  tb.Close
'  db.Close
'End Function

