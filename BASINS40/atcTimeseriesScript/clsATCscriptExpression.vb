Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility

<System.Runtime.InteropServices.ProgId("clsATCscriptExpression_NET.clsATCscriptExpression")> Public Class clsATCscriptExpression
    'Copyright 2002 by AQUA TERRA Consultants

    Private MySubExpressions As Generic.List(Of clsATCscriptExpression)
    Private MyToken As ATCsToken
    Private MyString As String 'Script name, loop variable, etc.
    Private MyLong As Integer
    Private MyLine As Integer
    Private MyNumSubExpressionsOnSameLine As Integer 'used when creating Printable version

    Public Structure ColDef
        Dim Name As String
        Dim StartCol As Integer
        Dim ColWidth As Integer
    End Structure

    'Be sure to synchronize with TokenString array below
    Public Enum ATCsToken
        tok_Unknown = 0
        'tok_Abs
        tok_And
        tok_ATCScript
        tok_Attribute
        tok_ColumnFormat
        'tok_ColumnValue
        tok_Comment
        tok_Dataset
        tok_Date
        tok_FatalError
        tok_Fill
        tok_Flag
        tok_For
        tok_If
        tok_In
        tok_Increment
        tok_Instr
        tok_IsNumeric
        tok_LineEnd
        tok_Literal
        tok_MathAdd
        tok_MathDivide
        tok_MathMultiply
        tok_MathPower
        tok_MathSubtract
        tok_Mid
        tok_NextLine
        tok_Not
        tok_Or
        tok_Set
        tok_Test
        tok_Trim
        tok_Unset
        tok_Value
        tok_Variable
        tok_Warn
        tok_While
        tok_GT
        tok_GE
        tok_LT
        tok_LE
        tok_NE
        tok_EQ
        tok_Last
    End Enum

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()
        MySubExpressions = New Generic.List(Of clsATCscriptExpression)
        MyNumSubExpressionsOnSameLine = 9999 'By default put all subexp on same line
        MyLong = 0
        MyString = ""
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    Public Property Line() As Integer
        Get
            Line = MyLine
        End Get
        Set(ByVal Value As Integer)
            MyLine = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            'UPGRADE_WARNING: Couldn't resolve default property of object TokenString(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Name = TokenString(MyToken)
        End Get
        Set(ByVal Value As String)
            Token = TokenFromString(Value)
        End Set
    End Property

    Public Property Token() As ATCsToken
        Get
            Token = MyToken
        End Get
        Set(ByVal Value As ATCsToken)
            MyToken = Value
            'UPGRADE_NOTE: Object MySubExpressions may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            MySubExpressions = Nothing
            MySubExpressions = New Generic.List(Of clsATCscriptExpression)
            Select Case MyToken
                Case ATCsToken.tok_ColumnFormat : MyNumSubExpressionsOnSameLine = 1
                Case ATCsToken.tok_ATCScript, ATCsToken.tok_If, ATCsToken.tok_While : MyNumSubExpressionsOnSameLine = 1
                Case ATCsToken.tok_For : MyNumSubExpressionsOnSameLine = 3
                Case Else : MyNumSubExpressionsOnSameLine = 9999
            End Select
        End Set
    End Property

    Public ReadOnly Property SubExpressionCount() As Integer
        Get
            SubExpressionCount = MySubExpressions.Count()
        End Get
    End Property

    Public ReadOnly Property SubExpression(ByVal index As Integer) As clsATCscriptExpression
        Get
            SubExpression = MySubExpressions.Item(index)
        End Get
    End Property

    'Returns token value if str matches a token or zero if it doesn't
    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Function TokenFromString(ByRef str_Renamed As String) As Integer
        Dim index As Integer
        Dim cmpstr As String
        cmpstr = LCase(str_Renamed)
        index = 0
        'UPGRADE_WARNING: Couldn't resolve default property of object TokenString(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        While index < ATCsToken.tok_Last And cmpstr <> LCase(TokenString(index))
            index = index + 1
        End While
        If index = ATCsToken.tok_Last Then index = ATCsToken.tok_Unknown
        TokenFromString = index
    End Function

    Public Function Printable(Optional ByRef indent As String = "", Optional ByRef indentIncrement As String = "") As String
        Dim index, maxIndex As Integer 'For looping through SubExpressions
        Dim retval As String
        Dim addCR As Boolean
        Dim childIndent As String
        childIndent = indent & indentIncrement
        retval = ""
        index = 0 'CHANGE: to zero-based, used to be = 1
        maxIndex = MySubExpressions.Count()
        If ScriptAssigningLineNumbers Then
            If MyToken = ATCsToken.tok_ATCScript Then CurrentLineNum = 1
            MyLine = CurrentLineNum
        End If
        Select Case MyToken

            Case ATCsToken.tok_For
                retval = indent & "(" & TokenString(MyToken)
                retval = retval & " " & MySubExpressions.Item(0).Printable & " = "
                retval = retval & MySubExpressions.Item(1).Printable
                retval = retval & " to "
                retval = retval & MySubExpressions.Item(2).Printable
                index = 3

DefaultLoop:
                While index <= MyNumSubExpressionsOnSameLine And index <= maxIndex - 1 'CHANGE: used to be index <= maxIndex
                    AddSuffixNoDoubles(retval, " ")
                    retval = retval & MySubExpressions.Item(index).Printable
                    index = index + 1
                End While

                If index <= maxIndex - 1 Then addCR = True Else addCR = False

                While index <= maxIndex - 1
                    If Right(retval, Len(PrintEOL)) <> PrintEOL Then
                        retval = retval & PrintEOL
                        If ScriptAssigningLineNumbers Then CurrentLineNum = CurrentLineNum + 1
                    End If
                    retval = retval & MySubExpressions.Item(index).Printable(childIndent, indentIncrement)
                    index = index + 1
                End While
                retval = retval & ")"
                If addCR Then
                    retval = retval & PrintEOL
                    If ScriptAssigningLineNumbers Then CurrentLineNum = CurrentLineNum + 1
                End If

            Case ATCsToken.tok_Literal
                If IsNumeric(MyString) Then
                    retval = indent & MyString
                Else
                    retval = indent & """" & MyString & """"
                End If

            Case ATCsToken.tok_Variable
                retval = indent & MyString

            Case Else
                retval = indent & "(" & TokenString(MyToken)
                If Len(MyString) > 0 Then retval = retval & " " & MyString
                GoTo DefaultLoop
        End Select
        Return retval
    End Function

    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub AddSuffixNoDoubles(ByRef str_Renamed As String, ByRef suffix As String)
        If Right(str_Renamed, Len(suffix)) <> suffix Then str_Renamed = str_Renamed & suffix
    End Sub

    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function SkipChars(ByRef start As Integer, ByRef str_Renamed As String, ByRef chars As String) As Integer
        Dim retval, lenStr As Integer
        lenStr = Len(str_Renamed)
        retval = start
        While retval <= lenStr
            If InStr(chars, Mid(str_Renamed, retval, 1)) Then
                retval = retval + 1
            Else
                GoTo ExitFun
            End If
        End While
ExitFun:
        SkipChars = retval
    End Function

    'Returns position of next character from chars in str
    'Returns len(str) + 1 if none were found
    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function FirstCharPos(ByRef start As Integer, ByRef str_Renamed As String, ByRef chars As String) As Integer
        Dim CharPos, retval, curval, LenChars As Integer
        retval = Len(str_Renamed) + 1
        LenChars = Len(chars)
        For CharPos = 1 To LenChars
            curval = InStr(start, str_Renamed, Mid(chars, CharPos, 1))
            If curval > 0 And curval < retval Then retval = curval
        Next CharPos
        FirstCharPos = retval
    End Function

    'Specialized version of FirstCharPos
    Private Function FirstDelimPos(ByRef start As Integer) As Integer
        Dim curval, retval, CharPos As Integer
        retval = 9999
        For CharPos = 1 To NumColumnDelimiters
            curval = InStr(start, CurrentLine, Mid(ColumnDelimiter, CharPos, 1))
            If curval > 0 And curval < retval Then retval = curval
        Next CharPos
        If retval = 9999 Then retval = 0
        FirstDelimPos = retval
    End Function


    'Parse string expression into this object
    Public Sub ParseExpression(ByRef buf As String)
        'Debug.Print "ParseExpression: " & buf
        Dim LenBuf, ParsePos, NextPos, ParenLevel As Integer
        Dim newExp As clsATCscriptExpression
        Select Case Left(buf, 1)
            Case "(" 'expression
                ParsePos = 2
                LenBuf = Len(buf)
                NextPos = FirstCharPos(ParsePos, buf, "() " & vbTab & vbCr & vbLf)
                Token = TokenFromString(Mid(buf, ParsePos, NextPos - ParsePos))
                'Remember names of unknown tokens
                If Token = 0 Then MyString = Mid(buf, ParsePos, NextPos - ParsePos)
                ParsePos = SkipChars(NextPos, buf, " " & vbTab & vbCr & vbLf)
                While ParsePos < LenBuf
                    Select Case Mid(buf, ParsePos, 1)

                        Case "(" 'sub expression, find matching ")" before parsing
                            NextPos = ParsePos + 1
                            ParenLevel = 1
                            While NextPos <= LenBuf And ParenLevel > 0
                                Select Case Mid(buf, NextPos, 1)
                                    Case "(" : ParenLevel = ParenLevel + 1
                                    Case ")" : ParenLevel = ParenLevel - 1
                                End Select
                                NextPos = NextPos + 1
                            End While

                        Case """" 'literal
                            NextPos = InStr(ParsePos + 1, buf, """") + 1
                            If NextPos = 0 Then 'Unterminated string error
                                MsgBox("Unterminated string: '" & Mid(buf, ParsePos, 80), MsgBoxStyle.OkOnly, "Script parse error")
                                Exit Sub
                            End If

                        Case Else 'variable or numeric literal
                            NextPos = FirstCharPos(ParsePos, buf, "() " & vbTab & vbCr & vbLf)
                            If NextPos = ParsePos Then
                                MsgBox("Parse error - Probably mismatched parentheses", MsgBoxStyle.Exclamation, "ScriptExpression, ParseExpression")
                                'Stop
                                NextPos = FirstCharPos(ParsePos + 1, buf, "() " & vbTab & vbCr & vbLf)
                            End If
                    End Select

                    If Token = ATCsToken.tok_For Then
                        If Mid(buf, ParsePos, NextPos - ParsePos) = "=" Then GoTo SkipString
                        If LCase(Mid(buf, ParsePos, NextPos - ParsePos)) = "to" Then GoTo SkipString
                    End If
                    newExp = New clsATCscriptExpression
                    newExp.ParseExpression(Mid(buf, ParsePos, NextPos - ParsePos))
                    MySubExpressions.Add(newExp)
                    'UPGRADE_NOTE: Object newExp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    newExp = Nothing
SkipString:
                    ParsePos = SkipChars(NextPos, buf, " " & vbTab & vbCr & vbLf)
                End While

            Case """"
                Token = ATCsToken.tok_Literal
                MyString = Mid(buf, 2, InStr(2, buf, """") - 2)

            Case Else
                If IsNumeric(buf) Then
                    Token = ATCsToken.tok_Literal
                Else
                    Token = ATCsToken.tok_Variable
                End If
                MyString = buf
        End Select
    End Sub

    Private Function FindColumnValue() As String
        Static WarnedAboutCapitalization As Boolean
        Dim colNum As Integer
        Dim tmpstr As String
        Dim endPos, StartPos, curCol As Integer
        Dim colNameNum As String
        colNameNum = MyString
        FindColumnValue = colNameNum

        If MyLong > 0 Then
            colNum = MyLong
        Else
            If IsNumeric(colNameNum) Then
                colNum = CInt(colNameNum)
                MyLong = colNum
            Else
                colNum = 1
                tmpstr = colNameNum
                Do Until colNum > NamedColumns
                    If ColDefs(colNum).Name = tmpstr Then Exit Do
                    colNum = colNum + 1
                Loop

                If colNum > NamedColumns Then 'didn't find column name on first try
                    colNum = 1
                    tmpstr = LCase(colNameNum)
                    Do Until colNum > NamedColumns 'try again ignoring capitalization
                        If LCase(ColDefs(colNum).Name) = tmpstr Then Exit Do
                        colNum = colNum + 1
                    Loop
                    If colNum <= NamedColumns Then
                        If Not WarnedAboutCapitalization Then
                            Logger.Dbg("Warning - wrong capitalization '" & colNameNum & "' vs '" & ColDefs(colNum).Name & "'")
                            WarnedAboutCapitalization = True
                        End If
                    End If
                End If

                If colNum > NamedColumns Then 'And FixedColumns
                    colNum = -1 'FindColumnValue = "Error - column '" & colNameNum & "' not defined"
                End If

                MyLong = colNum
            End If
        End If

        If colNum > 0 Then
            If FixedColumns Then
                If CurrentRepeat < 2 Then
                    StartPos = ColDefs(colNum).StartCol 'No repeat or first repeat
                ElseIf ColDefs(colNum).StartCol < ColDefs(0).StartCol Then
                    StartPos = ColDefs(colNum).StartCol 'Non-repeating column
                Else
                    If RepeatStartCol < 1 Then
                        FindColumnValue = "Error - Repeating column not defined '" & colNameNum & "'"
                        GoTo ExitFun
                    End If
                    StartPos = ColDefs(0).StartCol + (CurrentRepeat - 1) * ColDefs(0).ColWidth + (ColDefs(colNum).StartCol - ColDefs(0).StartCol)
                End If
                If StartPos > LenCurrentLine Then ' the column does not exist on this line
                    FindColumnValue = ""
                ElseIf StartPos + ColDefs(colNum).ColWidth > LenCurrentLine Then  'Col narrower than expected
                    If DebuggingScript Then
                        DebugScriptForm.txtCurrentLine.SelectionStart = StartPos - 1
                        DebugScriptForm.txtCurrentLine.SelectionLength = LenCurrentLine - StartPos + 1
                    End If
                    FindColumnValue = Mid(CurrentLine, StartPos)
                Else
                    If DebuggingScript Then
                        DebugScriptForm.txtCurrentLine.SelectionStart = StartPos - 1
                        DebugScriptForm.txtCurrentLine.SelectionLength = ColDefs(colNum).ColWidth
                    End If
                    FindColumnValue = Mid(CurrentLine, StartPos, ColDefs(colNum).ColWidth)
                End If
            Else 'Delimited columns
                If colNum >= RepeatStartCol Then
                    If CurrentRepeat > 1 Then colNum = colNum + (CurrentRepeat - 1) * (NamedColumns - RepeatStartCol + 1)
                End If
                StartPos = 1
                curCol = 1
                While curCol < colNum
                    StartPos = FirstDelimPos(StartPos) + 1
                    If StartPos = 1 Then ' the column does not exist on this line
                        FindColumnValue = ""
                        GoTo ExitFun
                    End If
                    curCol = curCol + 1
                End While
                endPos = FirstDelimPos(StartPos)
                If endPos = 0 Then 'Must be the last col on this line
                    If DebuggingScript Then
                        DebugScriptForm.txtCurrentLine.SelectionStart = StartPos - 1
                        DebugScriptForm.txtCurrentLine.SelectionLength = LenCurrentLine - StartPos + 1
                    End If
                    FindColumnValue = Mid(CurrentLine, StartPos)
                Else
                    If DebuggingScript Then
                        DebugScriptForm.txtCurrentLine.SelectionStart = StartPos - 1
                        DebugScriptForm.txtCurrentLine.SelectionLength = endPos - StartPos
                    End If
                    FindColumnValue = Mid(CurrentLine, StartPos, endPos - StartPos)
                End If
            End If
        End If
ExitFun:
    End Function

    Private Function SetColumnFormat() As String
        Dim rule, lrule As String
        Dim SubExpIndex, ColIndex, SubExpMax As Integer
        Dim tmpstr As String
        Dim StartCol As Integer
        'Dim dollarPos, caretPos As Integer
        Dim colonPos As Integer
        ReDim ColDefs(100)
        RepeatStartCol = 0
        '  RepeatEndCol = 0
        ColIndex = 1 'ZCHECK
        rule = MySubExpressions.Item(0).Printable

        NamedColumns = 0
        FixedColumns = False
        ColumnDelimiter = ""
        If IsNumeric(rule) Then
            ColumnDelimiter = Chr(CShort(rule))
        Else
            lrule = Trim(LCase(rule))
            If lrule = "fixed" Then
                FixedColumns = True
            Else
                If InStr(lrule, "tab") Then ColumnDelimiter = ColumnDelimiter & vbTab
                If InStr(lrule, "space") Then ColumnDelimiter = ColumnDelimiter & " "
                For StartCol = 33 To 126
                    Select Case StartCol
                        Case 48 : StartCol = 58
                        Case 65 : StartCol = 91
                        Case 97 : StartCol = 123
                    End Select
                    If InStr(lrule, Chr(StartCol)) > 0 Then ColumnDelimiter = ColumnDelimiter & Chr(StartCol)
                Next StartCol
                NumColumnDelimiters = Len(ColumnDelimiter)
            End If
        End If

        SubExpIndex = 1 'CHANGE: used to be 2
        SubExpMax = MySubExpressions.Count()
        While SubExpIndex <= SubExpMax - 1
            If ColIndex > UBound(ColDefs) Then ReDim Preserve ColDefs(ColIndex + 100)
            rule = MySubExpressions.Item(SubExpIndex).Printable
            If FixedColumns Then ' start-end:name or start+len:name
ParseFixedDef:
                ColDefs(ColIndex).StartCol = ReadIntLeaveRest(rule)
                tmpstr = Left(rule, 1)
                If tmpstr = ":" Then
                    ColDefs(ColIndex).ColWidth = 1
                Else
                    rule = Mid(rule, 2)
                    ColDefs(ColIndex).ColWidth = ReadIntLeaveRest(rule)
                    If tmpstr = "-" Then ColDefs(ColIndex).ColWidth = ColDefs(ColIndex).ColWidth - ColDefs(ColIndex).StartCol + 1
                End If
                ColDefs(ColIndex).Name = Mid(rule, 2)
                If ColIndex > NamedColumns Then NamedColumns = ColIndex
                If LCase(ColDefs(ColIndex).Name) = "repeating" Then
                    ColDefs(0).StartCol = ColDefs(ColIndex).StartCol
                    ColDefs(0).ColWidth = ColDefs(ColIndex).ColWidth
                    RepeatStartCol = ColIndex
                End If
                ColIndex = ColIndex + 1
            Else 'delimited definition - expect colNum:name or name
                colonPos = InStr(rule, ":")
                If colonPos > 0 Then
                    tmpstr = Left(rule, colonPos - 1)
                    If IsNumeric(tmpstr) Then
                        ColIndex = CShort(tmpstr)
                        rule = Mid(rule, colonPos + 1)
                    End If
                End If
                If LCase(rule) = "repeating" Then
                    If RepeatStartCol = 0 Or RepeatStartCol > ColIndex Then RepeatStartCol = ColIndex
                    '        If RepeatEndCol = 0 Or RepeatEndCol < ColIndex Then RepeatEndCol = ColIndex
                Else
                    ColDefs(ColIndex).Name = rule
                    If ColIndex > NamedColumns Then NamedColumns = ColIndex
                    ColIndex = ColIndex + 1
                End If
            End If
            SubExpIndex = SubExpIndex + 1
        End While
        ReDim Preserve ColDefs(NamedColumns)
        If FixedColumns Then
            SetColumnFormat = "Defined " & NamedColumns & " fixed columns"
        Else
            SetColumnFormat = "Defined " & NamedColumns & " delimited Columns"
        End If
    End Function

    Private Function ParseDate() As String
        Dim Min, da, yr, cnt, mo, hr, sec As Integer
        Dim str_Renamed As String
        cnt = MySubExpressions.Count()
        If cnt < 1 Then
            MsgBox("No values specified for date" & vbCr & Printable())
        Else
            str_Renamed = MySubExpressions.Item(0).Evaluate
            If IsNumeric(str_Renamed) Then
                yr = CInt(str_Renamed)
            Else
                Return ""
            End If
        End If

        mo = 12
        hr = 24
        Min = 0
        sec = 0

        If cnt >= 2 Then
            mo = MySubExpressions.Item(1).Evaluate
            If cnt < 3 Then
                da = daymon(yr, mo)
            Else
                da = MySubExpressions.Item(2).Evaluate
                If cnt >= 4 Then
                    hr = MySubExpressions.Item(3).Evaluate
                    If cnt < 5 Then
                        Min = 60
                    Else
                        Min = MySubExpressions.Item(4).Evaluate
                        If cnt >= 6 Then sec = MySubExpressions.Item(5).Evaluate
                    End If
                End If
            End If
        End If
        ParseDate = TokenString(MyToken) & " " & ScriptSetDate(Jday(yr, mo, da, hr, Min, sec))
    End Function

    Private pAbortResult As String = "Abort"

    Public Function Evaluate() As String
        Static WarnedAboutCannotIncrement As Boolean
        Static WarnedAboutNonNumericValue As Boolean
        Static WarnedAboutNonNumericDataset As Boolean
        Dim SubExp As Integer
        Dim tmpval As String
        Dim tmpval2 As String
        Dim retval As String
        Dim ForMin As Integer
        Dim ForCounter As Integer
        Dim ForMax As Integer
        Dim num1 As Object = Nothing
        Dim num2 As Object = Nothing
        'Dim num1 As Single, num2 As Single
        retval = ""
        If DebuggingScript Then DebugScriptForm.EvalExpression(Me)
        'Debug.Print "Evaluate: " & TokenString(MyToken);
        'If MyString = "" Then Debug.Print Else Debug.Print " MyString = " & MyString
        Select Case MyToken
            '    Case tok_Abs:
            '      tmpval = MySubExpressions(0).Evaluate
            '      If IsNumeric(tmpval) Then
            '        num1 = CSng(tmpval)
            '        retval = Abs(num1)
            '      Else
            '        retval = "0"
            '      End If
            Case ATCsToken.tok_And
                retval = "1"
                ForMax = MySubExpressions.Count() - 1
                For SubExp = 0 To ForMax
                    If Not EvalTruth(MySubExpressions.Item(SubExp).Evaluate) Then retval = "0" : Exit For
                    If AbortScript Then Return pAbortResult
                Next
            Case ATCsToken.tok_ATCScript
                retval = TokenString(MyToken) & " " & MySubExpressions.Item(0).Printable
                ForMax = MySubExpressions.Count() - 1
                For SubExp = 1 To ForMax
                    If MySubExpressions.Item(SubExp).Token = ATCsToken.tok_Test Then
                        If TestingFile Then
                            retval = MySubExpressions.Item(SubExp).Evaluate
                            Exit For
                        End If
                    Else
                        MySubExpressions.Item(SubExp).Evaluate()
                    End If
                    If AbortScript Then Return pAbortResult
                Next
            Case ATCsToken.tok_Attribute
                retval = MySubExpressions.Item(1).Evaluate 'CHANGE: used to be 2
                ScriptSetAttribute(MySubExpressions.Item(0).Printable, retval)
            Case ATCsToken.tok_ColumnFormat : retval = SetColumnFormat()
            Case ATCsToken.tok_Comment : retval = ""
                'Case tok_ColumnValue:   retval = FindColumnValue
            Case ATCsToken.tok_Dataset
                retval = TokenString(MyToken)
                ForMax = MySubExpressions.Count() - 1
                If ForMax = 0 Then
                    tmpval = MySubExpressions.Item(0).Evaluate
                    If IsNumeric(tmpval) Then
                        ScriptSetDataset(CInt(tmpval))
                    Else
                        If Not WarnedAboutNonNumericDataset Then
                            WarnedAboutNonNumericDataset = True
                            MsgBox("Non-numeric dataset index: " & tmpval, MsgBoxStyle.OkOnly, "Error Importing Data")
                        End If
                    End If
                Else
                    ScriptManageDataset("ClearCriteria")
                    SubExp = 0
                    While SubExp < ForMax
                        tmpval = MySubExpressions.Item(SubExp).Printable : SubExp = SubExp + 1
                        tmpval2 = MySubExpressions.Item(SubExp).Evaluate : SubExp = SubExp + 1
                        ScriptManageDataset("AddCriteria", tmpval, tmpval2)
                        If AbortScript Then Return pAbortResult
                    End While
                    ScriptManageDataset("MatchCriteria")
                End If
            Case ATCsToken.tok_Date : retval = ParseDate()
            Case ATCsToken.tok_FatalError
                retval = MySubExpressions.Item(0).Evaluate
                MsgBox(retval, MsgBoxStyle.OkOnly, "Fatal Error Importing Data")
            Case ATCsToken.tok_Fill
                If MySubExpressions.Count() < 1 Then
                    MsgBox("Fill requires at least Time Units (Y,M,D,h,m,s)" & vbCr & "Optional args are Time Step (1), Fill Value (0), Missing Value (-999), and Accumulated Value (-998)")
                Else
                    Select Case Left(MySubExpressions.Item(0).Evaluate, 1)
                        Case "C", "c" : modATCscript.FillTU = atcTimeUnit.TUCentury
                        Case "Y", "y" : modATCscript.FillTU = atcTimeUnit.TUYear
                        Case "M" : modATCscript.FillTU = atcTimeUnit.TUMonth
                        Case "D", "d" : modATCscript.FillTU = atcTimeUnit.TUDay
                        Case "H", "h" : modATCscript.FillTU = atcTimeUnit.TUHour
                        Case "m" : modATCscript.FillTU = atcTimeUnit.TUMinute
                        Case "S", "s" : modATCscript.FillTU = atcTimeUnit.TUSecond
                    End Select
                    If MySubExpressions.Count() < 2 Then
                        FillTS = 1
                    Else
                        FillTS = MySubExpressions.Item(1).Evaluate
                    End If
                    If MySubExpressions.Count() < 3 Then
                        FillVal = 0
                    Else
                        FillVal = MySubExpressions.Item(2).Evaluate
                    End If
                    If MySubExpressions.Count() < 4 Then
                        FillMissing = -999
                    Else
                        FillMissing = MySubExpressions.Item(3).Evaluate
                    End If
                    If MySubExpressions.Count() < 5 Then
                        FillAccum = -998
                    Else
                        FillAccum = MySubExpressions.Item(4).Evaluate
                    End If
                End If
            Case ATCsToken.tok_Flag
                tmpval = MySubExpressions.Item(0).Evaluate
                If IsNumeric(tmpval) Then
                    ScriptSetFlag(CInt(tmpval))
                Else
                    MsgBox("Flag specified '" & tmpval & "' is not numeric." & vbCr & tmpval, MsgBoxStyle.OkOnly, "Import Data")
                    Stop
                End If
                retval = tmpval
            Case ATCsToken.tok_For
                If MySubExpressions.Item(0).Token = ATCsToken.tok_Variable Then
                    tmpval = MySubExpressions.Item(0).Printable
                Else
                    tmpval = MySubExpressions.Item(0).Evaluate
                End If
                tmpval2 = MySubExpressions.Item(1).Evaluate
                retval = TokenString(MyToken) & " " & tmpval
                If IsNumeric(tmpval2) Then
                    ForMin = CInt(tmpval2)
                    tmpval2 = MySubExpressions.Item(2).Evaluate
                    If IsNumeric(tmpval2) Then
                        ForMax = CInt(tmpval2)
                        For ForCounter = ForMin To ForMax
                            ScriptSetVariable(tmpval, CStr(ForCounter))
                            For SubExp = 3 To MySubExpressions.Count() - 1
                                MySubExpressions.Item(SubExp).Evaluate()
                                If AbortScript Then Return pAbortResult
                            Next
                        Next
                    Else
                        MsgBox("Non-numeric maximum value in" & vbCr & Printable(), MsgBoxStyle.OkOnly, "Data Import")
                        'Stop
                    End If
                Else
                    MsgBox("Non-numeric minimum value in" & vbCr & Printable(), MsgBoxStyle.OkOnly, "Data Import")
                    'Stop
                End If
            Case ATCsToken.tok_If
                'UPGRADE_WARNING: Couldn't resolve default property of object TokenString(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                retval = TokenString(MyToken) & " " & MySubExpressions.Item(0).Printable
                If EvalTruth(MySubExpressions.Item(0).Evaluate) Then
                    ForMax = MySubExpressions.Count() - 1
                    For SubExp = 1 To ForMax
                        MySubExpressions.Item(SubExp).Evaluate()
                        If AbortScript Then Return pAbortResult
                    Next
                End If
            Case ATCsToken.tok_In
                retval = "0"
                tmpval = MySubExpressions.Item(0).Evaluate
                ForMax = MySubExpressions.Count() - 1
                For SubExp = 1 To ForMax
                    If MySubExpressions.Item(SubExp).Evaluate = tmpval Then
                        retval = "1"
                        Exit For
                    End If
                Next
            Case ATCsToken.tok_Increment
                tmpval = MySubExpressions.Item(0).Printable
                tmpval2 = MySubExpressions.Item(0).Evaluate
                retval = TokenString(MyToken) & " " & tmpval
                If IsNumeric(tmpval2) Then
                    ScriptSetVariable(tmpval, CStr(CInt(CDbl(tmpval2) + 1)))
                Else
                    If Not WarnedAboutCannotIncrement Then
                        MsgBox("Cannot increment variable '" & tmpval & "' Value= '" & tmpval2, MsgBoxStyle.OkOnly, "Script Evaluation")
                        WarnedAboutCannotIncrement = True
                    End If
                End If
            Case ATCsToken.tok_Instr
                If MySubExpressions.Count() > 2 Then
                    retval = CStr(InStr(CInt(MySubExpressions.Item(0).Evaluate), MySubExpressions.Item(1).Evaluate, MySubExpressions.Item(2).Evaluate))
                Else
                    retval = CStr(InStr(MySubExpressions.Item(0).Evaluate, MySubExpressions.Item(1).Evaluate))
                End If
            Case ATCsToken.tok_IsNumeric
                If IsNumeric(MySubExpressions.Item(0).Evaluate) Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_LineEnd
                InputEOL = vbCr
                tmpval = UCase(MySubExpressions.Item(0).Printable)
                If IsNumeric(tmpval) Then
                    InputEOL = ""
                    InputLineLen = CInt(tmpval)
                    LenCurrentLine = InputLineLen
                ElseIf Left(tmpval, 1) = "A" And IsNumeric(Mid(tmpval, 2)) Then 'ZCHECK
                    InputEOL = Chr(CInt(Mid(tmpval, 2)))
                ElseIf tmpval = "CR" Then
                    InputEOL = vbCr
                ElseIf tmpval = "LF" Then
                    InputEOL = vbLf
                Else : MsgBox("Unknown LineEnd '" & tmpval & "'" & vbCr & "Defaulting to Carriage Return", MsgBoxStyle.OkOnly, "clsATCscriptExpression:Evaluate")
                End If
                LenInputEOL = Len(InputEOL)
                If NextLineStart = 1 Then ScriptNextLine()
            Case ATCsToken.tok_Literal : retval = MyString
            Case ATCsToken.tok_MathAdd
                SetNumericVals(num1, num2, tmpval, tmpval2)
                retval = CStr(num1 + num2)
            Case ATCsToken.tok_MathDivide
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num2 = 0 Then
                    retval = CStr(0)
                Else
                    retval = CStr(num1 / num2)
                End If
            Case ATCsToken.tok_MathMultiply
                SetNumericVals(num1, num2, tmpval, tmpval2)
                retval = CStr(num1 * num2)
            Case ATCsToken.tok_MathPower
                SetNumericVals(num1, num2, tmpval, tmpval2)
                retval = CStr(num1 ^ num2)
            Case ATCsToken.tok_MathSubtract
                SetNumericVals(num1, num2, tmpval, tmpval2)
                retval = CStr(num1 - num2)
            Case ATCsToken.tok_Mid
                If MySubExpressions.Count() > 2 Then
                    retval = Mid(MySubExpressions.Item(0).Evaluate, MySubExpressions.Item(1).Evaluate, MySubExpressions.Item(2).Evaluate)
                Else
                    retval = Mid(MySubExpressions.Item(0).Evaluate, MySubExpressions.Item(1).Evaluate)
                End If
            Case ATCsToken.tok_Not
                If EvalTruth(MySubExpressions.Item(0).Evaluate) Then retval = "0" Else retval = "1"
            Case ATCsToken.tok_Or
                retval = "0"
                ForMax = MySubExpressions.Count() - 1
                For SubExp = 0 To ForMax
                    If EvalTruth(MySubExpressions.Item(SubExp).Evaluate) Then retval = "1" : Exit For
                    If AbortScript Then Return pAbortResult
                Next
            Case ATCsToken.tok_NextLine
                If MySubExpressions.Count() < 1 Then
                    ForMax = 0
                Else
                    ForMax = MySubExpressions.Item(0).Evaluate - 1
                End If
                For SubExp = 0 To ForMax
                    ScriptNextLine()
                Next
                retval = CurrentLine
            Case ATCsToken.tok_Set 'MySubExpressions(1) is variable name, (2) is new value
                If MySubExpressions.Item(0).Token = ATCsToken.tok_Variable Then
                    tmpval = MySubExpressions.Item(0).Printable
                Else
                    tmpval = MySubExpressions.Item(0).Evaluate
                End If
                tmpval2 = MySubExpressions.Item(1).Evaluate
                ScriptSetVariable(tmpval, tmpval2)
                retval = tmpval2
            Case ATCsToken.tok_Test
                ForMax = MySubExpressions.Count() - 2
                For SubExp = 0 To ForMax
                    MySubExpressions.Item(SubExp).Evaluate()
                    If AbortScript Then Return pAbortResult
                Next
                retval = MySubExpressions.Item(MySubExpressions.Count - 1).Evaluate
            Case ATCsToken.tok_Trim
                retval = Trim(MySubExpressions.Item(0).Evaluate)
            Case ATCsToken.tok_Unset
                If MySubExpressions.Item(0).Token = ATCsToken.tok_Variable Then
                    tmpval = MySubExpressions.Item(0).Printable
                Else
                    tmpval = MySubExpressions.Item(0).Evaluate
                End If
                ScriptUnsetVariable(tmpval)
            Case ATCsToken.tok_Value
                tmpval = MySubExpressions.Item(0).Evaluate
                If IsNumeric(tmpval) Then
                    ScriptSetValue(CSng(tmpval))
                ElseIf Len(tmpval) = 0 Then
                    ScriptSetValue(FillMissing)
                Else
                    ScriptSetValue(FillMissing)
                    If Not WarnedAboutNonNumericValue Then
                        MsgBox("Value specified '" & tmpval & "' is not numeric." & vbCr & tmpval, MsgBoxStyle.OkOnly, "Import Data")
                        WarnedAboutNonNumericValue = True
                    End If
                End If
                retval = tmpval
            Case ATCsToken.tok_Variable
                retval = GetVariable()
            Case ATCsToken.tok_Warn
                retval = MySubExpressions.Item(0).Evaluate
                MsgBox(retval, MsgBoxStyle.OkOnly, "Warning")
            Case ATCsToken.tok_While
                retval = TokenString(MyToken) & " " & MySubExpressions.Item(0).Printable
                While EvalTruth(MySubExpressions.Item(0).Evaluate)
                    ForMax = MySubExpressions.Count() - 1
                    For SubExp = 1 To ForMax
                        MySubExpressions.Item(SubExp).Evaluate()
                        If AbortScript Then Return pAbortResult
                    Next
                End While
            Case ATCsToken.tok_GT
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 > num2 Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_GE
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 >= num2 Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_LT
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 < num2 Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_LE
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 <= num2 Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_NE
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 <> num2 Then retval = "1" Else retval = "0"
            Case ATCsToken.tok_EQ
                SetNumericVals(num1, num2, tmpval, tmpval2)
                If num1 = num2 Then retval = "1" Else retval = "0"
            Case Else
                retval = "Unknown token evaluated: " & Printable()
                'Hacking "Abs" token in without breaking binary compatibility by adding tok_Abs
                If InStr(retval, "Unknown Abs ") > 0 And MySubExpressions.Count() = 1 Then
                    tmpval = MySubExpressions.Item(0).Evaluate
                    If IsNumeric(tmpval) Then
                        num1 = CSng(tmpval)
                        retval = CStr(System.Math.Abs(num1))
                    Else
                        retval = "0"
                    End If
                End If
        End Select
        Return retval
        'Debug.Print "EvaluateReturn = " & retval
    End Function

    Private Sub SetNumericVals(ByRef num1 As Object, ByRef num2 As Object, ByVal tmpval As Object, ByVal tmpval2 As Object)
        num1 = 0
        num2 = 0
        tmpval = MySubExpressions.Item(0).Evaluate
        tmpval2 = MySubExpressions.Item(1).Evaluate
        If IsNumeric(tmpval) And IsNumeric(tmpval2) Then
            num1 = CSng(tmpval)
            num2 = CSng(tmpval2)
        Else
            'MsgBox "Non-numeric value in comparison" & tmpval2
            'Stop
            num1 = tmpval
            num2 = tmpval2
        End If
    End Sub


    Private Function GetVariable() As String
        Dim retval As String
        retval = MyString 'Return variable name if there is no value - this may be confusing
        On Error Resume Next
        Select Case LCase(MyString)
            Case "repeat" : retval = CStr(CurrentRepeat)
            Case "eof" : If ScriptEndOfData() Then retval = "1" Else retval = "0"
            Case "eol" : If ColDefs(0).StartCol + CurrentRepeat * ColDefs(0).ColWidth >= LenCurrentLine Then retval = "1" Else retval = "0"
            Case Else
                Dim lScriptStateIndex As Integer = ScriptState.IndexFromKey(MyString)
                If lScriptStateIndex > -1 Then
                    retval = ScriptState.ItemByIndex(lScriptStateIndex)
                Else
                    retval = FindColumnValue()
                End If
        End Select
        Return retval
    End Function

    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function EvalTruth(ByRef str_Renamed As String) As Boolean
        Select Case LCase(str_Renamed)
            Case "0", "", "false" : EvalTruth = False
            Case Else : EvalTruth = True
        End Select
    End Function

    'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Terminate_Renamed()
        'UPGRADE_NOTE: Object MySubExpressions may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        MySubExpressions = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub
End Class