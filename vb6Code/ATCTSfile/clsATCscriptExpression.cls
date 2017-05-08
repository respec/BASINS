VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsATCscriptExpression"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Private MySubExpressions As Collection 'of clsATCscriptExpression
Private MyToken As ATCsToken
Private MyString As String 'Script name, loop variable, etc.
Private MyLong As Long
Private MyLine As Long
Private MyNumSubExpressionsOnSameLine As Long 'used when creating Printable version

Public Type ColDef
  Name As String
  StartCol As Long
  ColWidth As Long
End Type

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

Private Sub Class_Initialize()
  Set MySubExpressions = New Collection
  MyNumSubExpressionsOnSameLine = 9999 'By default put all subexp on same line
  MyLong = 0
  MyString = ""
End Sub

Public Property Get Line() As Long
  Line = MyLine
End Property
Public Property Let Line(newValue As Long)
  MyLine = newValue
End Property

Public Property Get Name() As String
  Name = TokenString(MyToken)
End Property
Public Property Let Name(newValue As String)
  Token = TokenFromString(newValue)
End Property

Public Property Get Token() As ATCsToken
  Token = MyToken
End Property
Public Property Let Token(newValue As ATCsToken)
  MyToken = newValue
  Set MySubExpressions = Nothing
  Set MySubExpressions = New Collection
  Select Case MyToken
    Case tok_ColumnFormat:                 MyNumSubExpressionsOnSameLine = 1
    Case tok_ATCScript, tok_If, tok_While: MyNumSubExpressionsOnSameLine = 1
    Case tok_For:                          MyNumSubExpressionsOnSameLine = 3
    Case Else:                             MyNumSubExpressionsOnSameLine = 9999
  End Select
End Property

Public Property Get SubExpressionCount() As Long
  SubExpressionCount = MySubExpressions.Count
End Property

Public Property Get SubExpression(index As Long) As clsATCscriptExpression
  Set SubExpression = MySubExpressions(index)
End Property

'Returns token value if str matches a token or zero if it doesn't
Public Function TokenFromString(str As String) As Long
  Dim index As Long
  Dim cmpstr As String
  cmpstr = LCase(str)
  index = 0
  While index < tok_Last And cmpstr <> LCase(TokenString(index))
    index = index + 1
  Wend
  If index = tok_Last Then index = tok_Unknown
  TokenFromString = index
End Function

Public Function Printable(Optional indent As String = "", Optional indentIncrement As String = "") As String
  Dim index As Long, maxIndex As Long     'For looping through SubExpressions
  Dim retval As String, addCR As Boolean
  Dim childIndent As String
  childIndent = indent & indentIncrement
  retval = ""
  index = 1
  maxIndex = MySubExpressions.Count
  If ScriptAssigningLineNumbers Then
    If MyToken = tok_ATCScript Then CurrentLineNum = 1
    MyLine = CurrentLineNum
  End If
  Select Case MyToken
    
    Case tok_For
      retval = indent & "(" & TokenString(MyToken)
      retval = retval & " " & MySubExpressions(1).Printable & " = "
      retval = retval & MySubExpressions(2).Printable
      retval = retval & " to "
      retval = retval & MySubExpressions(3).Printable
      index = 4
      GoSub DefaultLoop
    Case tok_Literal
      If IsNumeric(MyString) Then
        retval = indent & MyString
      Else
        retval = indent & """" & MyString & """"
      End If
    Case tok_Variable
      retval = indent & MyString
    Case Else
      GoSub DefaultPrintable
  End Select
  Printable = retval
  Exit Function

DefaultPrintable:
  retval = indent & "(" & TokenString(MyToken)
  If Len(MyString) > 0 Then retval = retval & " " & MyString
DefaultLoop:
  While index <= MyNumSubExpressionsOnSameLine And index <= maxIndex
    AddSuffixNoDoubles retval, " "
    retval = retval & MySubExpressions(index).Printable
    index = index + 1
  Wend
  
  If index <= maxIndex Then addCR = True Else addCR = False
  
  While index <= maxIndex
    If Right(retval, Len(PrintEOL)) <> PrintEOL Then
      retval = retval & PrintEOL
      If ScriptAssigningLineNumbers Then CurrentLineNum = CurrentLineNum + 1
    End If
    retval = retval & MySubExpressions(index).Printable(childIndent, indentIncrement)
    index = index + 1
  Wend
  retval = retval & ")"
  If addCR Then
    retval = retval & PrintEOL
    If ScriptAssigningLineNumbers Then CurrentLineNum = CurrentLineNum + 1
  End If
  Return
End Function

Private Sub AddSuffixNoDoubles(ByRef str As String, suffix As String)
  If Right(str, Len(suffix)) <> suffix Then str = str & suffix
End Sub

Private Function SkipChars(start&, str$, chars$) As Long
  Dim retval&, lenStr&
  lenStr = Len(str)
  retval = start
  While retval <= lenStr
    If InStr(chars, Mid(str, retval, 1)) Then
      retval = retval + 1
    Else
      GoTo ExitFun
    End If
  Wend
ExitFun:
  SkipChars = retval
End Function

'Returns position of next character from chars in str
'Returns len(str) + 1 if none were found
Private Function FirstCharPos(start&, str$, chars$) As Long
  Dim retval&, curval&, CharPos&, LenChars&
  retval = Len(str) + 1
  LenChars = Len(chars)
  For CharPos = 1 To LenChars
    curval = InStr(start, str, Mid(chars, CharPos, 1))
    If curval > 0 And curval < retval Then retval = curval
  Next CharPos
  FirstCharPos = retval
End Function

'Specialized version of FirstCharPos
Private Function FirstDelimPos(start&) As Long
  Dim retval&, curval&, CharPos&
  retval = 9999
  For CharPos = 1 To NumColumnDelimiters
    curval = InStr(start, CurrentLine, Mid(ColumnDelimiter, CharPos, 1))
    If curval > 0 And curval < retval Then retval = curval
  Next CharPos
  If retval = 9999 Then retval = 0
  FirstDelimPos = retval
End Function


'Parse string expression into this object
Public Sub ParseExpression(buf As String)
  'Debug.Print "ParseExpression: " & buf
  Select Case Left(buf, 1)
    Case "("  'expression
      Dim ParsePos&, NextPos&, LenBuf&, ParenLevel&
      Dim newExp As clsATCscriptExpression
      ParsePos = 2
      LenBuf = Len(buf)
      NextPos = FirstCharPos(ParsePos, buf, "() " & vbTab & vbCr & vbLf)
      Token = TokenFromString(Mid(buf, ParsePos, NextPos - ParsePos))
      'Remember names of unknown tokens
      If Token = 0 Then MyString = Mid(buf, ParsePos, NextPos - ParsePos)
      ParsePos = SkipChars(NextPos, buf, " " & vbTab & vbCr & vbLf)
      While ParsePos < LenBuf
        Select Case Mid(buf, ParsePos, 1)
          
          Case "("  'sub expression, find matching ")" before parsing
            NextPos = ParsePos + 1
            ParenLevel = 1
            While NextPos <= LenBuf And ParenLevel > 0
              Select Case Mid(buf, NextPos, 1)
                Case "(": ParenLevel = ParenLevel + 1
                Case ")": ParenLevel = ParenLevel - 1
              End Select
              NextPos = NextPos + 1
            Wend
          
          Case """" 'literal
            NextPos = InStr(ParsePos + 1, buf, """") + 1
            If NextPos = 0 Then 'Unterminated string error
              MsgBox "Unterminated string: '" & Mid(buf, ParsePos, 80), vbOKOnly, "Script parse error"
              Exit Sub
            End If
          
          Case Else 'variable or numeric literal
            NextPos = FirstCharPos(ParsePos, buf, "() " & vbTab & vbCr & vbLf)
            If NextPos = ParsePos Then
              MsgBox "Parse error - Probably mismatched parentheses", vbExclamation, "ScriptExpression, ParseExpression"
              'Stop
              NextPos = FirstCharPos(ParsePos + 1, buf, "() " & vbTab & vbCr & vbLf)
            End If
        End Select
        
        If Token = tok_For Then
          If Mid(buf, ParsePos, NextPos - ParsePos) = "=" Then GoTo SkipString
          If LCase(Mid(buf, ParsePos, NextPos - ParsePos)) = "to" Then GoTo SkipString
        End If
        Set newExp = New clsATCscriptExpression
        newExp.ParseExpression Mid(buf, ParsePos, NextPos - ParsePos)
        MySubExpressions.Add newExp
        Set newExp = Nothing
SkipString:
        ParsePos = SkipChars(NextPos, buf, " " & vbTab & vbCr & vbLf)
      Wend
      
    Case """"
      Token = tok_Literal
      MyString = Mid(buf, 2, InStr(2, buf, """") - 2)
    
    Case Else
      If IsNumeric(buf) Then
        Token = tok_Literal
      Else
        Token = tok_Variable
      End If
      MyString = buf
  End Select
End Sub

Private Function FindColumnValue() As String
  Static WarnedAboutCapitalization As Boolean
  Dim colNum As Long, tmpstr As String
  Dim StartPos As Long, endPos As Long, curCol As Long
  Dim colNameNum As String
  colNameNum = MyString
  FindColumnValue = colNameNum

  If MyLong > 0 Then
    colNum = MyLong
  Else
    If IsNumeric(colNameNum) Then
      colNum = CLng(colNameNum)
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
            Debug.Print "Warning - wrong capitalization '" & colNameNum & "' vs '" & ColDefs(colNum).Name & "'"
            WarnedAboutCapitalization = True
          End If
        End If
      End If
      
      If colNum > NamedColumns Then  'And FixedColumns
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
        StartPos = ColDefs(0).StartCol + (CurrentRepeat - 1) * ColDefs(0).ColWidth _
                   + (ColDefs(colNum).StartCol - ColDefs(0).StartCol)
      End If
      If StartPos > LenCurrentLine Then ' the column does not exist on this line
        FindColumnValue = ""
      ElseIf StartPos + ColDefs(colNum).ColWidth > LenCurrentLine Then 'Col narrower than expected
        If DebuggingScript Then
          frmDebugScript.txtCurrentLine.SelStart = StartPos - 1
          frmDebugScript.txtCurrentLine.SelLength = LenCurrentLine - StartPos + 1
        End If
        FindColumnValue = Mid(CurrentLine, StartPos)
      Else
        If DebuggingScript Then
          frmDebugScript.txtCurrentLine.SelStart = StartPos - 1
          frmDebugScript.txtCurrentLine.SelLength = ColDefs(colNum).ColWidth
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
      Wend
      endPos = FirstDelimPos(StartPos)
      If endPos = 0 Then 'Must be the last col on this line
        If DebuggingScript Then
          frmDebugScript.txtCurrentLine.SelStart = StartPos - 1
          frmDebugScript.txtCurrentLine.SelLength = LenCurrentLine - StartPos + 1
        End If
        FindColumnValue = Mid(CurrentLine, StartPos)
      Else
        If DebuggingScript Then
          frmDebugScript.txtCurrentLine.SelStart = StartPos - 1
          frmDebugScript.txtCurrentLine.SelLength = endPos - StartPos
        End If
        FindColumnValue = Mid(CurrentLine, StartPos, endPos - StartPos)
      End If
    End If
  End If
ExitFun:
End Function

Private Function SetColumnFormat() As String
  Dim rule As String, lrule As String
  Dim ColIndex As Long, SubExpIndex As Long, SubExpMax As Long
  Dim tmpstr As String
  Dim StartCol As Long
  Dim caretPos&, dollarPos&, colonPos&
  ReDim ColDefs(100)
  RepeatStartCol = 0
'  RepeatEndCol = 0
  ColIndex = 1
  rule = MySubExpressions(1).Printable
  
  NamedColumns = 0
  FixedColumns = False
  ColumnDelimiter = ""
  If IsNumeric(rule) Then
    ColumnDelimiter = Chr(CInt(rule))
  Else
    lrule = Trim(LCase(rule))
    If lrule = "fixed" Then
      FixedColumns = True
    Else
      If InStr(lrule, "tab") Then ColumnDelimiter = ColumnDelimiter & vbTab
      If InStr(lrule, "space") Then ColumnDelimiter = ColumnDelimiter & " "
      For StartCol = 33 To 126
        Select Case StartCol
          Case 48: StartCol = 58
          Case 65: StartCol = 91
          Case 97: StartCol = 123
        End Select
        If InStr(lrule, Chr(StartCol)) > 0 Then ColumnDelimiter = ColumnDelimiter & Chr(StartCol)
      Next StartCol
      NumColumnDelimiters = Len(ColumnDelimiter)
    End If
  End If
  
  SubExpIndex = 2
  SubExpMax = MySubExpressions.Count
  While SubExpIndex <= SubExpMax
    If ColIndex > UBound(ColDefs) Then ReDim Preserve ColDefs(ColIndex + 100)
    rule = MySubExpressions(SubExpIndex).Printable
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
          ColIndex = CInt(tmpstr)
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
  Wend
  ReDim Preserve ColDefs(NamedColumns)
  If FixedColumns Then
    SetColumnFormat = "Defined " & NamedColumns & " fixed columns"
  Else
    SetColumnFormat = "Defined " & NamedColumns & " delimited Columns"
  End If
End Function

Private Function ParseDate() As String
  Dim cnt&, yr&, mo&, da&, hr&, Min&, sec&, str As String
  cnt = MySubExpressions.Count
  If cnt < 1 Then
    MsgBox "No values specified for date" & vbCr & Printable
  Else
    str = MySubExpressions(1).Evaluate
    If IsNumeric(str) Then
      yr = str
    Else
      Exit Function
    End If
  End If
  
  mo = 12
  hr = 24
  Min = 0
  sec = 0
  
  If cnt >= 2 Then
    mo = MySubExpressions(2).Evaluate
    If cnt < 3 Then
      da = daymon(yr, mo)
    Else
      da = MySubExpressions(3).Evaluate
      If cnt >= 4 Then
        hr = MySubExpressions(4).Evaluate
        If cnt < 5 Then
          Min = 60
        Else
          Min = MySubExpressions(5).Evaluate
          If cnt >= 6 Then sec = MySubExpressions(6).Evaluate
        End If
      End If
    End If
  End If
  ParseDate = TokenString(MyToken) & " " & ScriptSetDate(jday(yr, mo, da, hr, Min, sec))
End Function

Public Function Evaluate() As String
  Static WarnedAboutCannotIncrement As Boolean
  Static WarnedAboutNonNumericValue As Boolean
  Static WarnedAboutNonNumericDataset As Boolean
  Dim SubExp As Long, retval As String, tmpval As String, tmpval2 As String
  Dim ForCounter As Long, ForMin As Long, ForMax As Long
  Dim num1 As Variant, num2 As Variant
  'Dim num1 As Single, num2 As Single
  retval = ""
  If DebuggingScript Then frmDebugScript.EvalExpression Me
  'Debug.Print "Evaluate: " & TokenString(MyToken);
  'If MyString = "" Then Debug.Print Else Debug.Print " MyString = " & MyString
  Select Case MyToken
'    Case tok_Abs:
'      tmpval = MySubExpressions(1).Evaluate
'      If IsNumeric(tmpval) Then
'        num1 = CSng(tmpval)
'        retval = Abs(num1)
'      Else
'        retval = "0"
'      End If
    Case tok_And:
        retval = "1"
        ForMax = MySubExpressions.Count
        For SubExp = 1 To ForMax
          If Not EvalTruth(MySubExpressions(SubExp).Evaluate) Then retval = "0": Exit For
          If AbortScript Then Exit Function
        Next
    Case tok_ATCScript:
      retval = TokenString(MyToken) & " " & MySubExpressions(1).Printable
      ForMax = MySubExpressions.Count
      For SubExp = 2 To ForMax
        If MySubExpressions(SubExp).Token = tok_Test Then
          If TestingFile Then
            retval = MySubExpressions(SubExp).Evaluate
            Exit For
          End If
        Else
          MySubExpressions(SubExp).Evaluate
        End If
        If AbortScript Then Exit Function
      Next
    Case tok_Attribute:     retval = MySubExpressions(2).Evaluate
                            ScriptSetAttribute MySubExpressions(1).Printable, retval
    Case tok_ColumnFormat:  retval = SetColumnFormat
    Case tok_Comment:       retval = ""
    'Case tok_ColumnValue:   retval = FindColumnValue
    Case tok_Dataset:
      retval = TokenString(MyToken)
      ForMax = MySubExpressions.Count
      If ForMax = 1 Then
        tmpval = MySubExpressions(1).Evaluate
        If IsNumeric(tmpval) Then
          ScriptSetDataset CLng(tmpval)
        Else
          If Not WarnedAboutNonNumericDataset Then
            WarnedAboutNonNumericDataset = True
            MsgBox "Non-numeric dataset index: " & tmpval, vbOKOnly, "Error Importing Data"
          End If
        End If
      Else
        ScriptManageDataset "ClearCriteria"
        SubExp = 1
        While SubExp < ForMax
          tmpval = MySubExpressions(SubExp).Printable:  SubExp = SubExp + 1
          tmpval2 = MySubExpressions(SubExp).Evaluate: SubExp = SubExp + 1
          ScriptManageDataset "AddCriteria", tmpval, tmpval2
          If AbortScript Then Exit Function
        Wend
        ScriptManageDataset "MatchCriteria"
      End If
    Case tok_Date:          retval = ParseDate
    Case tok_FatalError:    retval = MySubExpressions(1).Evaluate
                            MsgBox retval, vbOKOnly, "Fatal Error Importing Data"
    Case tok_Fill
      If MySubExpressions.Count < 1 Then
        MsgBox "Fill requires at least Time Units (Y,M,D,h,m,s)" & vbCr & "Optional args are Time Step (1), Fill Value (0), Missing Value (-999), and Accumulated Value (-998)"
      Else
        Select Case Left(MySubExpressions(1).Evaluate, 1)
          Case "C", "c": FillTU = TUCentury
          Case "Y", "y": FillTU = TUYear
          Case "M":      FillTU = TUMonth
          Case "D", "d": FillTU = TUDay
          Case "H", "h": FillTU = TUHour
          Case "m":      FillTU = TUMinute
          Case "S", "s": FillTU = TUSecond
        End Select
        If MySubExpressions.Count < 2 Then FillTS = 1 Else FillTS = MySubExpressions(2).Evaluate
        If MySubExpressions.Count < 3 Then FillVal = 0 Else FillVal = MySubExpressions(3).Evaluate
        If MySubExpressions.Count < 4 Then FillMissing = -999 Else FillMissing = MySubExpressions(4).Evaluate
        If MySubExpressions.Count < 5 Then FillAccum = -998 Else FillAccum = MySubExpressions(5).Evaluate
      End If
    Case tok_Flag:          tmpval = MySubExpressions(1).Evaluate
                            If IsNumeric(tmpval) Then
                              ScriptSetFlag CLng(tmpval)
                            Else
                              MsgBox "Flag specified '" & tmpval & "' is not numeric." & vbCr & tmpval, vbOKOnly, "Import Data"
                              Stop
                            End If
                            retval = tmpval
    Case tok_For
      If MySubExpressions(1).Token = tok_Variable Then
        tmpval = MySubExpressions(1).Printable
      Else
        tmpval = MySubExpressions(1).Evaluate
      End If
      tmpval2 = MySubExpressions(2).Evaluate
      retval = TokenString(MyToken) & " " & tmpval
      If IsNumeric(tmpval2) Then
        ForMin = CLng(tmpval2)
        tmpval2 = MySubExpressions(3).Evaluate
        If IsNumeric(tmpval2) Then
          ForMax = CLng(tmpval2)
          For ForCounter = ForMin To ForMax
            ScriptSetVariable tmpval, CStr(ForCounter)
            For SubExp = 4 To MySubExpressions.Count
              MySubExpressions(SubExp).Evaluate
              If AbortScript Then Exit Function
            Next
          Next
        Else
          MsgBox "Non-numeric maximum value in" & vbCr & Printable, vbOKOnly, "Data Import"
          'Stop
        End If
      Else
        MsgBox "Non-numeric minimum value in" & vbCr & Printable, vbOKOnly, "Data Import"
        'Stop
      End If
    Case tok_If
      retval = TokenString(MyToken) & " " & MySubExpressions(1).Printable
      If EvalTruth(MySubExpressions(1).Evaluate) Then
        ForMax = MySubExpressions.Count
        For SubExp = 2 To ForMax
          MySubExpressions(SubExp).Evaluate
          If AbortScript Then Exit Function
        Next
      End If
    Case tok_In
      retval = "0"
      tmpval = MySubExpressions(1).Evaluate
      ForMax = MySubExpressions.Count
      For SubExp = 2 To ForMax
        If MySubExpressions(SubExp).Evaluate = tmpval Then
          retval = "1"
          Exit For
        End If
      Next
    Case tok_Increment:     tmpval = MySubExpressions(1).Printable
                            tmpval2 = MySubExpressions(1).Evaluate
                            retval = TokenString(MyToken) & " " & tmpval
                            If IsNumeric(tmpval2) Then
                              ScriptSetVariable tmpval, CStr(CLng(tmpval2 + 1))
                            Else
                              If Not WarnedAboutCannotIncrement Then
                                MsgBox "Cannot increment variable '" & tmpval & "' Value= '" & tmpval2, vbOKOnly, "Script Evaluation"
                                WarnedAboutCannotIncrement = True
                              End If
                            End If
    Case tok_Instr:
      If MySubExpressions.Count > 2 Then
        retval = InStr(MySubExpressions(1).Evaluate, MySubExpressions(2).Evaluate, MySubExpressions(3).Evaluate)
      Else
        retval = InStr(MySubExpressions(1).Evaluate, MySubExpressions(2).Evaluate)
      End If
    Case tok_IsNumeric:     If IsNumeric(MySubExpressions(1).Evaluate) Then retval = "1" Else retval = "0"
    Case tok_LineEnd:
      InputEOL = vbCr
      tmpval = UCase(MySubExpressions(1).Printable)
      If IsNumeric(tmpval) Then
        InputEOL = ""
        InputLineLen = tmpval
        LenCurrentLine = InputLineLen
      ElseIf Left(tmpval, 1) = "A" And IsNumeric(Mid(tmpval, 2)) Then
        InputEOL = Chr(Mid(tmpval, 2))
      ElseIf tmpval = "CR" Then InputEOL = vbCr
      ElseIf tmpval = "LF" Then InputEOL = vbLf
      Else: MsgBox "Unknown LineEnd '" & tmpval & "'" & vbCr & "Defaulting to Carriage Return", vbOKOnly, "clsATCscriptExpression:Evaluate"
      End If
      LenInputEOL = Len(InputEOL)
      If NextLineStart = 1 Then ScriptNextLine
    Case tok_Literal:       retval = MyString
    Case tok_MathAdd:       GoSub SetNumericVals: retval = CStr(num1 + num2)
    Case tok_MathDivide:    GoSub SetNumericVals: If num2 = 0 Then retval = 0 Else retval = CStr(num1 / num2)
    Case tok_MathMultiply:  GoSub SetNumericVals: retval = CStr(num1 * num2)
    Case tok_MathPower:     GoSub SetNumericVals: retval = CStr(num1 ^ num2)
    Case tok_MathSubtract:  GoSub SetNumericVals: retval = CStr(num1 - num2)
    Case tok_Mid:
      If MySubExpressions.Count > 2 Then
        retval = Mid(MySubExpressions(1).Evaluate, MySubExpressions(2).Evaluate, MySubExpressions(3).Evaluate)
      Else
        retval = Mid(MySubExpressions(1).Evaluate, MySubExpressions(2).Evaluate)
      End If
    Case tok_Not:           If EvalTruth(MySubExpressions(1).Evaluate) Then retval = "0" Else retval = "1"
    Case tok_Or:
        retval = "0"
        ForMax = MySubExpressions.Count
        For SubExp = 1 To ForMax
          If EvalTruth(MySubExpressions(SubExp).Evaluate) Then retval = "1": Exit For
          If AbortScript Then Exit Function
        Next
    Case tok_NextLine:
      If MySubExpressions.Count < 1 Then ForMax = 1 Else ForMax = MySubExpressions(1).Evaluate
      For SubExp = 1 To ForMax
        ScriptNextLine
      Next
      retval = CurrentLine
    Case tok_Set:           'MySubExpressions(1) is variable name, (2) is new value
                            If MySubExpressions(1).Token = tok_Variable Then
                              tmpval = MySubExpressions(1).Printable
                            Else
                              tmpval = MySubExpressions(1).Evaluate
                            End If
                            tmpval2 = MySubExpressions(2).Evaluate
                            ScriptSetVariable tmpval, tmpval2
                            retval = tmpval2
    Case tok_Test:
        ForMax = MySubExpressions.Count - 1
        For SubExp = 1 To ForMax
          MySubExpressions(SubExp).Evaluate
          If AbortScript Then Exit Function
        Next
        retval = MySubExpressions(SubExp).Evaluate
    Case tok_Trim:          retval = Trim(MySubExpressions(1).Evaluate)
    Case tok_Unset:
                            If MySubExpressions(1).Token = tok_Variable Then
                              tmpval = MySubExpressions(1).Printable
                            Else
                              tmpval = MySubExpressions(1).Evaluate
                            End If
                            ScriptUnsetVariable tmpval
    Case tok_Value:         tmpval = MySubExpressions(1).Evaluate
                            If IsNumeric(tmpval) Then
                              ScriptSetValue CSng(tmpval)
                            ElseIf Len(tmpval) = 0 Then
                              ScriptSetValue FillMissing
                            Else
                              ScriptSetValue FillMissing
                              If Not WarnedAboutNonNumericValue Then
                                MsgBox "Value specified '" & tmpval & "' is not numeric." & vbCr & tmpval, vbOKOnly, "Import Data"
                                WarnedAboutNonNumericValue = True
                              End If
                            End If
                            retval = tmpval
                            
    Case tok_Variable:      retval = GetVariable
    Case tok_Warn:          retval = MySubExpressions(1).Evaluate
                            MsgBox retval, vbOKOnly, "Warning"
    Case tok_While
      retval = TokenString(MyToken) & " " & MySubExpressions(1).Printable
      While EvalTruth(MySubExpressions(1).Evaluate)
        ForMax = MySubExpressions.Count
        For SubExp = 2 To ForMax
          MySubExpressions(SubExp).Evaluate
          If AbortScript Then Exit Function
        Next
      Wend
    Case tok_GT: GoSub SetNumericVals: If num1 > num2 Then retval = "1" Else retval = "0"
    Case tok_GE: GoSub SetNumericVals: If num1 >= num2 Then retval = "1" Else retval = "0"
    Case tok_LT: GoSub SetNumericVals: If num1 < num2 Then retval = "1" Else retval = "0"
    Case tok_LE: GoSub SetNumericVals: If num1 <= num2 Then retval = "1" Else retval = "0"
    Case tok_NE: GoSub SetNumericVals: If num1 <> num2 Then retval = "1" Else retval = "0"
    Case tok_EQ: GoSub SetNumericVals: If num1 = num2 Then retval = "1" Else retval = "0"
    Case Else
      retval = "Unknown token evaluated: " & Printable
      'Hacking "Abs" token in without breaking binary compatibility by adding tok_Abs
      If InStr(retval, "Unknown Abs ") > 0 And MySubExpressions.Count = 1 Then
        tmpval = MySubExpressions(1).Evaluate
        If IsNumeric(tmpval) Then
          num1 = CSng(tmpval)
          retval = Abs(num1)
        Else
          retval = "0"
        End If
      End If
  End Select
  
  Evaluate = retval
  'Debug.Print "EvaluateReturn = " & retval
  Exit Function

SetNumericVals:
  num1 = 0
  num2 = 0
  tmpval = MySubExpressions(1).Evaluate
  tmpval2 = MySubExpressions(2).Evaluate
  If IsNumeric(tmpval) And IsNumeric(tmpval2) Then
    num1 = CSng(tmpval)
    num2 = CSng(tmpval2)
  Else
    'MsgBox "Non-numeric value in comparison" & tmpval2
    'Stop
    num1 = tmpval
    num2 = tmpval2
  End If
  Return
End Function

Private Function GetVariable() As String
  Dim retval As String
  retval = MyString 'Return variable name if there is no value - this may be confusing
  On Error Resume Next
  Select Case LCase(MyString)
    Case "repeat": retval = CurrentRepeat
    Case "eof":    If ScriptEndOfData Then retval = "1" Else retval = "0"
    Case "eol":    If ColDefs(0).StartCol + CurrentRepeat * ColDefs(0).ColWidth >= LenCurrentLine Then retval = "1" Else retval = "0"
    Case Else:     retval = ScriptState(MyString)
  End Select
  If retval = MyString Then
    retval = FindColumnValue
  End If
  GetVariable = retval
End Function

Private Function EvalTruth(str As String) As Boolean
  Select Case LCase(str)
    Case "0", "", "false": EvalTruth = False
    Case Else:             EvalTruth = True
  End Select
End Function

Private Sub Class_Terminate()
  Set MySubExpressions = Nothing
End Sub
