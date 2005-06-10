Option Strict Off
Option Explicit On 

Imports System.Drawing

Public Module UtilColor
  '##MODULE_REMARKS Copyright 2001-5 AQUA TERRA Consultants - Royalty-free use permitted under open source license

  Private colorDatabase As clsATCTable
  Private Const grayBasename As String = "gray" 'base name for shades of gray (gray0..gray255)
  Private Const grayNameNumStart As Short = 5 'len(grayBasename) + 1 = where the number starts
  Private Const forceKnownColor As Boolean = True

  Private nColorRules As Integer
  Private colorMatchingRules() As String
  Private colorRamp() As Color
  Private colorsUsed() As Color
  Private MatchingColorsFilename As String

  Public Sub InitMatchingColors(ByRef Filename As String)
    ReDim colorsUsed(0)
    On Error GoTo NeverMind

    If FileExists(Filename) Then
      MatchingColorsFilename = Filename
    End If

    Dim LineNumber As Integer
    Dim buf, oneLine As String
    Dim CRpos, LFpos As Integer

    If FileExists(MatchingColorsFilename) Then
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
      End While
    End If
NeverMind:
  End Sub

  Public Function GetMatchingColor(Optional ByVal Specification As String = "") As Color
    Dim rule, used As Integer

    If Len(MatchingColorsFilename) = 0 Then
      GoTo GetRandomColor
    ElseIf Len(Specification) = 0 Then
      GoTo NextUnusedColor
    Else
      For rule = 1 To nColorRules
        If PatternMatch(LCase(Specification), LCase(colorMatchingRules(rule))) Then
          For used = 1 To UBound(colorsUsed)
            If colorsUsed(used).Equals(colorRamp(rule)) Then GoTo NextPatternMatch
          Next
          GoTo FoundColor
        End If
NextPatternMatch:
      Next
NextUnusedColor:
      For rule = 1 To nColorRules
        For used = 1 To UBound(colorsUsed)
          If colorsUsed(used).Equals(colorRamp(rule)) Then GoTo NextRule
        Next
        GoTo FoundColor
NextRule:
      Next
    End If
GetRandomColor:
    'Did not find a matching color or an unused color in the ramp - find a random one
    GetMatchingColor = Color.FromArgb(255, 64 + Rnd() * 128, 64 + Rnd() * 128, 64 + Rnd() * 128)
    Exit Function

FoundColor:
    GetMatchingColor = colorRamp(rule)
    ReDim Preserve colorsUsed(UBound(colorsUsed) + 1)
    colorsUsed(UBound(colorsUsed)) = colorRamp(rule)

  End Function

  Private Function colorDB() As clsATCTable 'clsDBF
    Static AlreadyReportedErrOpen As Boolean
    Static openedDB As Boolean
    Dim DBpath As String
    'Dim ff As New ATCoCtl.ATCoFindFile
    If Not openedDB Then
      On Error GoTo erropen

      'ff.SetRegistryInfo("ATCoCtl", "ATCoRend", "Path")
      'ff.SetDialogProperties("Please locate 'ATCoRend.dbf'", "ATCoRend.dbf")
      'DBpath = ff.GetName
      DBpath = FindFile("Please locate 'ATCoRend.dbf'", "ATCoRend.dbf", "dbf")

      If LCase(FileExt(DBpath)) = "mdb" Then
        DBpath = FilenameNoExt(DBpath) & ".dbf"
      End If

      If FileExists(DBpath) Then
        colorDatabase = New clsATCTableDBF
        colorDatabase.OpenFile(DBpath)
        openedDB = True
      End If
    End If

    colorDB = colorDatabase

    Exit Function

erropen:
    If Not AlreadyReportedErrOpen Then
      MsgBox("Error opening color database '" & DBpath & "'" & vbCr & Err.Description)
      AlreadyReportedErrOpen = True
    End If
    colorDB = Nothing
  End Function

  Public Function TextOrNumericColor(ByVal colr As String) As Color
    Static AlreadyReportedError As Boolean

    Dim c As String

    c = LCase(Trim(colr))
    TextOrNumericColor = Color.Empty
    Dim r As String
    If IsNumeric(c) Then
      TextOrNumericColor = Color.FromArgb(CInt(c))
    Else
      If Left(c, grayNameNumStart - 1) = LCase(grayBasename) Then
        r = Mid(colr, grayNameNumStart)
        If IsNumeric(r) Then TextOrNumericColor = Color.FromArgb(255, CInt(r), CInt(r), CInt(r))
      End If
      If TextOrNumericColor.Equals(Color.Empty) Then
        Dim db As clsATCTable = colorDB()
        If Not db Is Nothing Then
          If db.FindFirst(1, c) Then
            TextOrNumericColor = ColorTranslator.FromOle(CInt(db.Value(2)))
          Else
            TextOrNumericColor = Color.Gray
          End If
        End If
      End If
    End If
    Exit Function
erropen:
    If Not AlreadyReportedError Then
      MsgBox("Error opening color table" & vbCr & Err.Description)
      AlreadyReportedError = True
    End If
  End Function

  Public Function colorName(ByVal aColor As Color) As String
    Static HadErrOpen As Boolean
    Dim iColor As Integer

    Dim retval As String
    Dim g, r, b As Integer

    retval = aColor.Name

    r = aColor.R
    g = aColor.G
    b = aColor.B

    'If it is not black or white, check for gray
    If Not aColor.Equals(Color.White) AndAlso Not aColor.Equals(Color.Black) Then
      If r = g AndAlso g = b Then
        colorName = grayBasename & r
        Exit Function
      End If
    End If

    Dim db As clsATCTable = colorDB()
    If db Is Nothing Then GoTo SetHexValue

    Dim b1, r1, g1 As Integer
    Dim colorRaw As String
    Dim thisColor As Color
    Dim thisDist, minDist As Single
    Dim minDistName As String
    If db.FindFirst(2, ColorTranslator.ToOle(aColor)) Then
      retval = db.Value(1)
    Else
      'If rgb_Renamed = rgb_Renamed And &HFFFFFF Then
      If forceKnownColor Then 'Search for closest color in database
        minDist = 255 ^ 2 * 3
        For iColor = 1 To db.NumRecords
          db.CurrentRecord = iColor
          colorRaw = db.Value(2)
          If IsNumeric(colorRaw) Then
            thisColor = ColorTranslator.FromOle(CInt(colorRaw))
            r1 = thisColor.R
            g1 = thisColor.G
            b1 = thisColor.B
            thisDist = (r - r1) ^ 2 + (b - b1) ^ 2 + (g - g1) ^ 2
            If thisDist < minDist Then
              minDist = thisDist
              minDistName = db.Value(1)
            End If
          End If
        Next
        retval = minDistName
      Else
        GoTo SetHexValue
      End If
      'Else 'try again with 24-bit value
      'retval = colorName(rgb_Renamed And &HFFFFFF)
      'End If
    End If
    colorName = retval
    Exit Function
erropen:
    If Not HadErrOpen Then MsgBox("Error opening color table." & vbCr & "Using hex values instead of color names." & vbCr & Err.Description)
    HadErrOpen = True
    GoTo SetHexValue
SetHexValue:
    retval = Hex(aColor.ToArgb)
    If Len(retval) < 8 Then retval = New String("0", 8 - Len(retval)) & retval
    colorName = "&H" & retval
  End Function

  'For testing color database or cycling through available named colors
  'Goes forward one entry in the color database (or backward if fwd=false)
  'Starts at prevColorName
  'sets nextColorName and nextColor
  '  Public Sub testColor(ByVal fwd As Boolean, _
  '                       ByVal prevColorName As String, _
  '                       ByRef nextColorName As String, _
  '                       ByRef nextColor As Color)

  '    Dim c As String

  '    c = LCase(Trim(prevColorName))
  '    nextColor = Color.FromArgb(-1) '-1
  '    If IsNumeric(c) Then 'They gave us a number, not a name, so default to white
  '      nextColor = Color.Black  'vbBlack
  '      nextColorName = "black"
  '    Else
  '      If c = "black" And fwd Then
  '        nextColor = Color.FromArgb(1, 1, 1)   'RGB(1, 1, 1)
  '        nextColorName = grayBasename & "1"
  '      ElseIf c = "black1" And Not fwd Then
  '        nextColor = Color.FromArgb(254, 254, 254) ' RGB(254, 254, 254)
  '        nextColorName = grayBasename & "254"
  '      ElseIf Left(c, grayNameNumStart - 1) = LCase(grayBasename) Then
  '        Dim r$
  '        r = Mid(c, grayNameNumStart)
  '        If IsNumeric(r) Then
  '          If fwd Then r = r + 1 Else r = r - 1
  '          Select Case r
  '            Case Is <= 0 : nextColor = Color.Black : nextColorName = "black"
  '            Case Is >= 255 : nextColor = Color.FromArgb(1) : nextColorName = "black1"
  '            Case Else : nextColor = Color.FromArgb(r, r, r) : nextColorName = grayBasename & r
  '          End Select
  '        End If
  '      End If
  '      If nextColor.Equals(Color.FromArgb(-1)) Then
  '        Dim db As clsATCTableDBF
  '        db = colorDB()
  '        If Not db Is Nothing Then
  '          If db.FindFirst(1, c) Then
  '            If fwd Then
  '              If db.CurrentRecord < db.NumRecords Then
  '                db.CurrentRecord = db.CurrentRecord + 1
  '              Else
  '                db.CurrentRecord = 1
  '              End If
  '            Else
  '              If db.CurrentRecord > 1 Then
  '                db.CurrentRecord = db.CurrentRecord - 1
  '              Else
  '                db.CurrentRecord = db.NumRecords
  '              End If
  '            End If
  '            nextColor = Color.FromArgb(db.Value(2))
  '            nextColorName = db.Value(1)
  '          Else
  '            nextColor = Color.Black
  '            nextColorName = "black"
  '          End If
  '        End If
  '      End If
  '    End If
  '    Exit Sub
  'erropen:
  '    MsgBox("Error opening color table" & vbCr & Err.Description)
  '  End Sub
End Module