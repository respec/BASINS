Imports System.Drawing
Imports MapWinUtility

Public Module UtilColor
    '##MODULE_REMARKS Copyright 2001-9 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pColorDatabase As atcTable
    Private Const pGrayBasename As String = "gray" 'base name for shades of gray (gray0..gray255)
    Private Const pGrayNameNumStart As Integer = 5 'pGrayBasename.Length + 1  where the number starts
    Private Const pForceKnownColor As Boolean = True

    Private pColorMatchingRules As New Generic.List(Of Generic.KeyValuePair(Of String, Color))
    Private pColorsUsed As New Generic.List(Of Color)
    Private pMatchingColorsFilename As String = ""

    Public Sub InitMatchingColors(ByVal aFilename As String)
        pColorsUsed.Clear()
        Try
            If IO.File.Exists(aFilename) Then
                pColorMatchingRules.Clear()
                pMatchingColorsFilename = aFilename
                Dim lColor As Color
                For Each lOneLine As String In LinesInFile(aFilename)
                    lOneLine = lOneLine.Trim
                    If lOneLine.Length > 0 Then
                        lColor = TextOrNumericColor(StrRetRem(lOneLine))
                        pColorMatchingRules.Add(New Generic.KeyValuePair(Of String, Color)(lOneLine, lColor))
                    End If
                Next
            Else
                Logger.Dbg("InitMatchingColors:Missing:" & aFilename)
            End If
        Catch e As Exception
            Logger.Dbg("InitMatchingColors:Problem:" & e.Message)
        End Try
    End Sub

    Public Function GetMatchingColor(Optional ByVal aSpecification As String = "") As Color
        Dim lSpec As String = aSpecification.ToLower
        Dim lRule As Generic.KeyValuePair(Of String, Color)

        'Find a color that matches a rule
        For Each lRule In pColorMatchingRules
            If lSpec Like lRule.Key.ToLower AndAlso Not pColorsUsed.Contains(lRule.Value) Then
                GoTo FoundColor
            End If
        Next

        ''ignoring the text of the rules, just find the next color we haven't used
        'For Each lRule In pColorMatchingRules
        '    If Not pColorsUsed.Contains(lRule.Value) Then GoTo FoundColor
        'Next

        'Did not find a matching color or an unused color in the ramp - find a random one
        Do
            lRule = New Generic.KeyValuePair(Of String, Color)("", _
                    Color.FromArgb(255, _
                                   64 + CInt(Rnd() * 128), _
                                   64 + CInt(Rnd() * 128), _
                                   64 + CInt(Rnd() * 128)))
        Loop While pColorsUsed.Contains(lRule.Value)

FoundColor:
        GetMatchingColor = lRule.Value
        pColorsUsed.Add(lRule.Value)
    End Function

    Private Function colorDB() As atcTable
        Static lAlreadyReportedErrOpen As Boolean = False
        Static lOpenedDB As Boolean = False

        If Not lOpenedDB Then
            Dim lDBpath As String = ""
            Try
                lDBpath = FindFile("Please locate 'ATCoRend.dbf'", "ATCoRend.dbf", "dbf")

                'Old registry entry may refer to .mdb version but we use .dbf now
                If IO.Path.GetExtension(lDBpath).ToLower = ".mdb" Then
                    lDBpath = IO.Path.ChangeExtension(lDBpath, ".dbf")
                End If

                If IO.File.Exists(lDBpath) Then
                    pColorDatabase = New atcTableDBF
                    pColorDatabase.OpenFile(lDBpath)
                    lOpenedDB = True
                End If
            Catch e As Exception
                If Not lAlreadyReportedErrOpen Then
                    Logger.Msg("Error opening color database '" & lDBpath & "'" & vbCr & e.Message)
                    lAlreadyReportedErrOpen = True
                End If
                Return Nothing
            End Try
        End If

        Return pColorDatabase
    End Function

    Public Function TextOrNumericColor(ByVal aColorName As String) As Color
        Static lAlreadyReportedError As Boolean = False
        Try
            Dim lColorName As String = aColorName.Trim.ToLower

            TextOrNumericColor = Color.Empty

            If IsNumeric(lColorName) Then
                Return Color.FromArgb(CInt(lColorName))
            Else
                If lColorName.StartsWith(pGrayBasename.ToLower) Then
                    Dim lGrayId As String = Mid(aColorName, pGrayNameNumStart)
                    If IsNumeric(lGrayId) Then
                        Dim lGrayInt As Integer = CInt(lGrayId)
                        Return (Color.FromArgb(255, lGrayInt, lGrayInt, lGrayInt))
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
        Catch e As Exception
            If Not lAlreadyReportedError Then
                Logger.Msg("Error finding color '" & aColorName & "'" & vbCr & e.Message)
                lAlreadyReportedError = True
            End If
        End Try
    End Function

    Public Function colorName(ByVal aColor As Color) As String
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
SetHexValue:
        Return "&H" & Hex(aColor.ToArgb).PadLeft(8, "0"c)
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