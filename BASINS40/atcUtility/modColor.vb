Option Strict On
Option Explicit On

Imports System.Drawing
Imports MapWinUtility

Public Module UtilColor
    '##MODULE_REMARKS Copyright 2001-6 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pColorDatabase As atcTable
    Private Const pGrayBasename As String = "gray" 'base name for shades of gray (gray0..gray255)
    Private Const pGrayNameNumStart As Integer = 5 'pGrayBasename.Length + 1  where the number starts
    Private Const pForceKnownColor As Boolean = True

    Private pNColorRules As Integer = 0
    Private pColorMatchingRules() As String
    Private pColorRamp() As Color
    Private pColorsUsed() As Color
    Private pMatchingColorsFilename As String = ""

    Public Sub InitMatchingColors(ByVal aFilename As String)
        ReDim pColorsUsed(0)
        On Error GoTo NeverMind

        If FileExists(aFilename) Then
            pMatchingColorsFilename = aFilename
        End If

        Dim lBuf, lOneLine As String
        Dim lCRpos, lLFpos As Integer

        If FileExists(pMatchingColorsFilename) Then
            pNColorRules = 0
            ReDim pColorRamp(0)
            ReDim pColorMatchingRules(0)
            lBuf = WholeFileString(pMatchingColorsFilename)

            While Len(lBuf) > 0
                lCRpos = InStr(lBuf, vbCr)
                lLFpos = InStr(lBuf, vbLf)
                lOneLine = ""
                If lCRpos = 1 Or lLFpos = 1 Then
                    lBuf = Mid(lBuf, 2)
                ElseIf lCRpos = 0 And lLFpos = 0 Then
                    lOneLine = lBuf
                    lBuf = ""
                ElseIf lCRpos = 0 Then
                    lOneLine = Left(lBuf, lLFpos - 1)
                    lBuf = Mid(lBuf, lLFpos + 1)
                ElseIf lLFpos = 0 Or lCRpos < lLFpos Then
                    lOneLine = Left(lBuf, lCRpos - 1)
                    lBuf = Mid(lBuf, lCRpos + 1)
                Else
                    lOneLine = Left(lBuf, lLFpos - 1)
                    lBuf = Mid(lBuf, lLFpos + 1)
                End If
                lOneLine = lOneLine.Trim
                If lOneLine.Length > 0 Then
                    pNColorRules += 1
                    ReDim Preserve pColorRamp(pNColorRules)
                    ReDim Preserve pColorMatchingRules(pNColorRules)
                    pColorRamp(pNColorRules) = TextOrNumericColor(StrRetRem(lOneLine))
                    pColorMatchingRules(pNColorRules) = lOneLine
                End If
            End While
        End If
NeverMind:
    End Sub

    Public Function GetMatchingColor(Optional ByVal aSpecification As String = "") As Color
        Dim lRule As Integer
        Dim lUsed As Integer

        If pMatchingColorsFilename.Length = 0 Then
            GoTo GetRandomColor
        ElseIf aSpecification.Length = 0 Then
            GoTo NextUnusedColor
        Else
            For lRule = 1 To pNColorRules
                If LCase(aSpecification) Like LCase(pColorMatchingRules(lRule)) Then
                    For lUsed = 1 To UBound(pColorsUsed)
                        If pColorsUsed(lUsed).Equals(pColorRamp(lRule)) Then
                            GoTo NextPatternMatch
                        End If
                    Next
                    GoTo FoundColor
                End If
NextPatternMatch:
            Next
NextUnusedColor:
            For lRule = 1 To pNColorRules
                For lUsed = 1 To UBound(pColorsUsed)
                    If pColorsUsed(lUsed).Equals(pColorRamp(lRule)) Then
                        GoTo NextRule
                    End If
                Next
                GoTo FoundColor
NextRule:
            Next
        End If
GetRandomColor:
        'Did not find a matching color or an unused color in the ramp - find a random one
        GetMatchingColor = Color.FromArgb(255, _
                                          64 + CInt(Rnd()) * 128, _
                                          64 + CInt(Rnd()) * 128, _
                                          64 + CInt(Rnd()) * 128)
        Exit Function

FoundColor:
        GetMatchingColor = pColorRamp(lRule)
        ReDim Preserve pColorsUsed(UBound(pColorsUsed) + 1)
        pColorsUsed(UBound(pColorsUsed)) = pColorRamp(lRule)

    End Function

    Private Function colorDB() As atcTable 'clsDBF
        Static lAlreadyReportedErrOpen As Boolean = False
        Static lOpenedDB As Boolean = False
        Dim lDBpath As String

        If Not lOpenedDB Then
            On Error GoTo erropen
            'ff.SetRegistryInfo("ATCoCtl", "ATCoRend", "Path")
            'ff.SetDialogProperties("Please locate 'ATCoRend.dbf'", "ATCoRend.dbf")
            'DBpath = ff.GetName
            lDBpath = FindFile("Please locate 'ATCoRend.dbf'", "ATCoRend.dbf", "dbf")

            If LCase(FileExt(lDBpath)) = "mdb" Then
                lDBpath = FilenameNoExt(lDBpath) & ".dbf"
            End If

            If FileExists(lDBpath) Then
                pColorDatabase = New atcTableDBF
                pColorDatabase.OpenFile(lDBpath)
                lOpenedDB = True
            End If
        End If

        Return pColorDatabase
erropen:
        If Not lAlreadyReportedErrOpen Then
            Logger.Msg("Error opening color database '" & lDBpath & "'" & vbCr & Err.Description)
            lAlreadyReportedErrOpen = True
        End If
        Return Nothing
    End Function

    Public Function TextOrNumericColor(ByVal aColorName As String) As Color
        Static lAlreadyReportedError As Boolean = False

        Dim lColorName As String = aColorName.Trim.ToLower

        TextOrNumericColor = Color.Empty

        If IsNumeric(lColorName) Then
            Return Color.FromArgb(CInt(lColorName))
        Else
            If Left(lColorName, pGrayNameNumStart - 1) = LCase(pGrayBasename) Then
                Dim lGrayId As String = Mid(aColorName, pGrayNameNumStart)
                If IsNumeric(lGrayId) Then
                    Return (Color.FromArgb(255, _
                                          CInt(lGrayId), _
                                          CInt(lGrayId), _
                                          CInt(lGrayId)))
                End If
            End If
            If TextOrNumericColor.Equals(Color.Empty) Then
                Dim lDb As atcTable = colorDB()
                If Not lDb Is Nothing Then
                    If lDb.FindFirst(1, lColorName) Then
                        TextOrNumericColor = ColorTranslator.FromOle(CInt(lDb.Value(2)))
                    Else
                        TextOrNumericColor = Color.Gray
                    End If
                End If
            End If
        End If
        Exit Function
erropen:
        If Not lAlreadyReportedError Then
            Logger.Msg("Error opening color table" & vbCr & Err.Description)
            lAlreadyReportedError = True
        End If
    End Function

    Public Function colorName(ByVal aColor As Color) As String
        Static lHadErrOpen As Boolean = False

        Dim lRetval As String = aColor.Name
        Dim lR As Integer = aColor.R
        Dim lG As Integer = aColor.G
        Dim lB As Integer = aColor.B

        'If it is not black or white, check for gray
        If Not aColor.Equals(Color.White) AndAlso _
           Not aColor.Equals(Color.Black) Then
            If lr = lg AndAlso lg = lb Then
                Return pGrayBasename & lr
            End If
        End If

        Dim ldb As atcTable = colorDB()
        If ldb Is Nothing Then GoTo SetHexValue

        If ldb.FindFirst(2, ColorTranslator.ToOle(aColor).ToString) Then
            lRetval = ldb.Value(1)
        Else
            'If rgb_Renamed = rgb_Renamed And &HFFFFFF Then
            If pForceKnownColor Then 'Search for closest color in database
                Dim lminDistName As String = ""
                Dim lminDist As Double = 255 ^ 2 * 3
                For iColor As Integer = 1 To ldb.NumRecords
                    ldb.CurrentRecord = iColor
                    Dim lcolorRaw As String = ldb.Value(2)
                    If IsNumeric(lcolorRaw) Then
                        Dim lThisColor As Color = ColorTranslator.FromOle(CInt(lcolorRaw))
                        Dim lR1 As Integer = lThisColor.R
                        Dim lG1 As Integer = lThisColor.G
                        Dim lB1 As Integer = lThisColor.B
                        Dim lThisDist As Double = (lr - lR1) ^ 2 + _
                                                  (lb - lB1) ^ 2 + _
                                                  (lg - lG1) ^ 2
                        If lThisDist < lminDist Then
                            lminDist = lThisDist
                            lminDistName = ldb.Value(1)
                        End If
                    End If
                Next
                Return lminDistName
            Else
                GoTo SetHexValue
            End If
        End If
        Return lRetval
erropen:
        If Not lHadErrOpen Then
            Logger.Msg("Error opening color table." & vbCr & "Using hex values instead of color names." & vbCr & Err.Description)
        End If
        lHadErrOpen = True
SetHexValue:
        Return "&H" & Hex(aColor.ToArgb).PadLeft(8, CChar("0"))
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
    '        Dim db As atcTableDBF
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