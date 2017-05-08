Attribute VB_Name = "modATCscript"
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Public ScriptState As Collection 'of variable names (as keys) and values
Public WholeDataFile As String   'Contains entire contents of data file
Public LenDataFile As Long
Public NextLineStart As Long     'Index in WholeDataFile of first character of next line to be read
Public LastPercent As Long       'For updating status messages
Public CurrentLine As String     'Current line of data file being parsed
Public LenCurrentLine As Long
Public CurrentRepeat As Long     'Current repeating part within CurrentLine (>=1)
Public TestingFile As Boolean    'True if we are just testing, False if we are reading data
Public FixedColumns As Boolean   'True if columns are fixed width
Public ColumnDelimiter As String 'character that delimits columns if FixedColumns is False
Public NumColumnDelimiters As Long 'ColumnDelimiter may contain more than one delimiter character
Public ColDefs() As ColDef       'Names of columns (and start/width if FixedColumns) (1..NamesColumns)
                                 'ColDefs(0) stores info about the first repeating column
Public NamedColumns As Long      'Number of cols defined in ColDefs (there may be gaps if delimited)
Public RepeatStartCol As Long    'First column that repeats
'Public RepeatEndCol As Long      'Last column that repeats

Public CurrentLineNum As Long    'Current line of script being printed
Public DebuggingScript As Boolean
Public ScriptAssigningLineNumbers As Boolean
Public AbortScript As Boolean

Public PrintEOL As String
Public InputEOL As String
Public LenInputEOL As Long
Public InputLineLen As Long

Public FillTU As ATCTimeUnit
Public FillTS As Long
Public FillVal As Single
Public FillMissing As Single
Public FillAccum As Single

Public TokenString As Variant 'Array of token names

Private Const DefaultScenario = "ScriptRead"
Private pTserFile As ATCclsTserFile
Private pDataFilename As String
Private pTserData As Collection 'of ATCclsTserData

Private Type InputBuffer
  DateCount As Long
  DateDim As Long
  DateArray() As Double
  ValueArray() As Single
  FlagArray() As Long
End Type
Private InBuf() As InputBuffer
Private CurBuf As Long

Private pMonitor As Object
Private pMonitorSet As Boolean

Public Sub TokenStringInit()
  'Be sure to synchronize with Private Enum ATCsToken in clsATCscriptExpression
  TokenString = Array("Unknown", _
                      "And", _
                      "ATCScript", _
                      "Attribute", _
                      "ColumnFormat", _
                      "Comment", _
                      "Dataset", _
                      "Date", _
                      "FatalError", _
                      "Fill", _
                      "Flag", _
                      "For", _
                      "If", _
                      "In", _
                      "Increment", _
                      "Instr", _
                      "IsNumeric", _
                      "LineEnd", _
                      "Literal", _
                      "+", "/", "*", "^", "-", _
                      "Mid", _
                      "NextLine", _
                      "Not", "Or", "Set", "Test", "Trim", "Unset", "Value", "Variable", "Warn", "While", ">", ">=", "<", "<=", "<>", "=", "Last") 'VB limits number of line continuations to 24
End Sub

Private Sub ScriptInit()
  ReDim InBuf(0)
  
  Set pTserData = Nothing
  Set pTserData = New Collection
  AddNewTserAndBuffer
  
  Set ScriptState = Nothing
  Set ScriptState = New Collection
  WholeDataFile = ""
  LenDataFile = 0
  NextLineStart = 1
  LastPercent = 0
  CurrentLine = ""
  LenCurrentLine = 0
  CurrentRepeat = 1
  ColumnDelimiter = ""
  NumColumnDelimiters = 0
  ReDim ColDefs(0)
  NamedColumns = 0
  RepeatStartCol = 0
'  RepeatEndCol = 0
  CurrentLineNum = 0
  FillTS = 0
  AbortScript = False
End Sub

Public Function ReadIntLeaveRest(ByRef str As String) As Long
  Dim chpos&, lenStr&
  chpos = 1
  If IsNumeric(Left(str, 1)) Then
    lenStr = Len(str)
    While (IsNumeric(Mid(str, chpos, 1)))
      chpos = chpos + 1
      If chpos > lenStr Then
        ReadIntLeaveRest = CLng(str)
        Exit Function
      End If
    Wend
    ReadIntLeaveRest = CLng(Left(str, chpos - 1))
    str = Mid(str, chpos)
  Else
    ReadIntLeaveRest = 0
  End If
End Function

Public Sub ScriptSetMonitor(o As Object)
  pMonitorSet = True
  Set pMonitor = o
End Sub

Public Function ScriptEndOfData() As Boolean
  If NextLineStart >= LenDataFile And LenCurrentLine = 0 Then ScriptEndOfData = True Else ScriptEndOfData = False
End Function

Public Sub ScriptNextLine()
  Dim percent As Long
  Dim EOLPos As Long
  Do
    If NextLineStart > LenDataFile Then
      CurrentLine = ""
      LenCurrentLine = 0
      Exit Sub
    End If
    If InputLineLen > 0 Then 'All lines are same length
      CurrentLine = Mid(WholeDataFile, NextLineStart, LenCurrentLine)
      LenCurrentLine = Len(CurrentLine)
      NextLineStart = NextLineStart + LenCurrentLine
    Else
      If (InputEOL = vbCr Or InputEOL = vbLf) Then
        EOLPos = FirstStringPos(NextLineStart, WholeDataFile, vbCr, vbLf)
      Else
        EOLPos = InStr(NextLineStart, WholeDataFile, InputEOL)
      End If
      If EOLPos = 0 Then EOLPos = LenDataFile + 1
      LenCurrentLine = EOLPos - NextLineStart
      CurrentLine = Mid(WholeDataFile, NextLineStart, LenCurrentLine)
      NextLineStart = NextLineStart + LenCurrentLine + LenInputEOL
      If (NextLineStart < LenDataFile) Then 'Skip LF after CR
        If Mid(WholeDataFile, EOLPos, 1) = vbCr Then
          If Mid(WholeDataFile, NextLineStart, 1) = vbLf Then
            NextLineStart = NextLineStart + 1
          End If
        End If
      End If
    End If
    CurrentLineNum = CurrentLineNum + 1
  Loop While Len(Trim(CurrentLine)) = 0
  If pMonitorSet Then
    percent = 100 * NextLineStart / LenDataFile 'Loc = 128 * bytes read for sequential file
    If percent <> LastPercent Then
      pMonitor.SendMonitorMessage "(MSG1 " & Left(CurrentLine, 100) & ")"
      pMonitor.SendMonitorMessage "(MSG3 " & CStr(percent) & "%)"
      pMonitor.SendMonitorMessage "(PROGRESS " & CStr(percent) & ")"
      LastPercent = percent
      DoEvents
    End If
  End If
  'Debug.Print "NextLine: " & Format(CurrentLineNum, "00") & " " & CurrentLine
  If DebuggingScript Then frmDebugScript.NextLine
End Sub

Public Function ScriptSetDate(jdy As Double) As Double
  With InBuf(CurBuf)
    .DateCount = .DateCount + 1
    If .DateCount > .DateDim Then
      .DateDim = .DateCount * 2
      ReDim Preserve .DateArray(.DateDim)
      ReDim Preserve .ValueArray(.DateDim)
      ReDim Preserve .FlagArray(.DateDim)
    End If
    .DateArray(.DateCount) = jdy
    If DebuggingScript Then frmDebugScript.NewDate .DateCount, jdy
    'Debug.Print "Date " & .DateCount & "=" & jdy;
  End With
  ScriptSetDate = jdy
End Function

Public Function ScriptSetValue(newValue As Single) As Single
  InBuf(CurBuf).ValueArray(InBuf(CurBuf).DateCount) = newValue
  ScriptSetValue = newValue
  If DebuggingScript Then frmDebugScript.newValue InBuf(CurBuf).DateCount, newValue
  'Debug.Print " Value " & pDateCount & "=" & newValue
End Function

Public Function ScriptSetFlag(newValue As Long) As Long
  InBuf(CurBuf).FlagArray(InBuf(CurBuf).DateCount) = newValue
  ScriptSetFlag = newValue
  'Debug.Print " Flag " & pDateCount & "=" & newValue
End Function

Public Function ScriptSetVariable(VarName$, newValue$) As String
  Static ShowedNumericMessage As Long
  Static ShowedRangeMessage As Long
  Select Case LCase(VarName)
    Case "repeat"
                If IsNumeric(newValue) Then
                  CurrentRepeat = CLng(newValue)
                  If CurrentRepeat < 1 Then
                    If ShowedRangeMessage < 2 Then
                      MsgBox "Repeat was set to '" & CurrentRepeat & "' but it should always be >= 1.", vbOKOnly, "modATCscript:ScriptSetVariable"
                      ShowedRangeMessage = ShowedRangeMessage + 1
                    End If
                  End If
                Else
                  If ShowedNumericMessage < 2 Then
                    MsgBox "Non-numeric value '" & newValue & "' assigned to Repeat", vbOKOnly, "modATCscript:ScriptSetVariable"
                    ShowedNumericMessage = ShowedNumericMessage + 1
                  End If
                End If
    Case Else:  On Error Resume Next
                ScriptState.Remove VarName
                ScriptState.Add newValue, VarName
  End Select
End Function

Public Function ScriptUnsetVariable(VarName$) As String
  On Error Resume Next
  ScriptState.Remove VarName
  ScriptUnsetVariable = VarName
End Function

Public Sub ScriptSetDataset(index As Long)
  While index > pTserData.Count
    AddNewTserAndBuffer
  Wend
  CurBuf = index
End Sub

Private Sub AddNewTserAndBuffer()
  Dim newts As ATCclsTserData
  Set newts = New ATCclsTserData
  'Set newts = New ATCclsTserData
  newts.Header.desc = pDataFilename
  newts.Header.Sen = DefaultScenario
  Set newts.Dates = New ATCclsTserDate
  newts.AttribSet "FileImported", pDataFilename
  
  pTserData.Add newts
  
  ReDim Preserve InBuf(UBound(InBuf) + 1)
  CurBuf = UBound(InBuf)
  InBuf(CurBuf).DateCount = 0
  InBuf(CurBuf).DateDim = 0
  ReDim InBuf(CurBuf).DateArray(0)
  ReDim InBuf(CurBuf).ValueArray(0)
  ReDim InBuf(CurBuf).FlagArray(0)
End Sub

Public Sub ScriptManageDataset(cmd$, Optional AttrName$, Optional attrValue$)
  Static AttrNames(99) As String
  Static AttrValues(99) As String
  Static NumAttribs As Long
  Dim tserNum As Long, attrNum As Long
  Dim MisMatch As Boolean
  Dim tmp As String
  
  Select Case LCase(cmd)
    Case "clearcriteria"
      NumAttribs = 0
    Case "addcriteria"
      NumAttribs = NumAttribs + 1
      AttrNames(NumAttribs) = AttrName
      AttrValues(NumAttribs) = attrValue
    Case "matchcriteria"
      tserNum = 0
      MisMatch = True
      While tserNum < pTserData.Count And MisMatch
        tserNum = tserNum + 1
        MisMatch = False
        attrNum = 0
        While attrNum < NumAttribs And Not MisMatch
          attrNum = attrNum + 1
          AttrName = AttrNames(attrNum)
          attrValue = AttrValues(attrNum)
          With pTserData(tserNum).Header
            Select Case LCase(AttrName)
              Case "con", "cons", "constituent"
                                                If .con <> "" Then
                                                  If .con <> attrValue Then MisMatch = True
                                                End If
              Case "sen", "scen", "scenario"
                                                If .Sen <> "" And .Sen <> DefaultScenario Then
                                                  If .Sen <> attrValue Then MisMatch = True
                                                End If
              Case "loc", "location"
                                                If .loc <> "" Then
                                                  If .loc <> attrValue Then MisMatch = True
                                                End If
              Case "des", "desc", "description"
                                                If .desc <> "" And .desc <> pDataFilename Then
                                                  If .desc <> attrValue Then MisMatch = True
                                                End If
        
              Case Else:                        tmp = pTserData(tserNum).Attrib(AttrName, "")
                                                If tmp <> "" Then
                                                  If tmp <> attrValue Then MisMatch = True
                                                End If
            End Select
          End With
        Wend 'attrNum
      Wend   'tserNum
      If MisMatch Then
        AddNewTserAndBuffer
      Else
        CurBuf = tserNum
      End If
      For attrNum = 1 To NumAttribs 'In case some "Matching" values were default or blank, explicitly set them
        ScriptSetAttribute AttrNames(attrNum), AttrValues(attrNum)
      Next attrNum
    Case Else
      MsgBox "Unknown command " & cmd & " in ManageDataset", vbOKOnly, "modATCscript"
      Stop
  End Select
End Sub


Public Sub ScriptSetAttribute(AttrName$, newValue$)
  Dim searchBuf As Long
  With pTserData(CurBuf).Header
    Select Case LCase(AttrName)
      Case "con", "cons", "constituent": .con = newValue
      Case "sen", "scen", "scenario":    .Sen = newValue
      Case "loc", "location":            .loc = newValue
      Case "des", "desc", "description": .desc = newValue
      Case Else: pTserData(CurBuf).AttribSet AttrName, newValue
    End Select
  End With
End Sub

Public Function ScriptFromString(ScriptString As String) As clsATCscriptExpression
  Dim Script As clsATCscriptExpression
  Dim CountParens As Long, CountPos As Long
  On Error GoTo ErrParse
  
  CountPos = 1
  While CountPos > 0
    CountPos = InStr(CountPos, ScriptString, "(")
    If CountPos > 0 Then
      CountPos = CountPos + 1
      CountParens = CountParens + 1
    End If
  Wend
  
  CountPos = 1
  While CountPos > 0
    CountPos = InStr(CountPos, ScriptString, ")")
    If CountPos > 0 Then
      CountPos = CountPos + 1
      CountParens = CountParens - 1
    End If
  Wend
  
  If CountParens > 0 Then
    err.Description = "More left parens in script than right parens (" & CountParens & ")" & vbCr & _
           "Not trying to parse this script further." & vbCr & Left(ScriptString, 50) ', _
           'vbExclamation, "ScriptFromString"
  ElseIf CountParens < 0 Then
    err.Description = "More right parens in script than right parens (" & -CountParens & ")" & vbCr & _
           "Not trying to parse this script further." & vbCr & Left(ScriptString, 50) ', _
           'vbExclamation, "ScriptFromString"
  Else
    TokenStringInit
    'skip text before open paren
    If Left(ScriptString, 1) <> "(" Then
      ScriptString = Mid(ScriptString, InStr(ScriptString, "("))
    End If
    
    'skip text after close paren
    If Right(ScriptString, 1) <> ")" Then
      ScriptString = Left(ScriptString, InStrRev(ScriptString, ")"))
    End If
    
    PrintEOL = vbCrLf
    InputEOL = vbLf
    LenInputEOL = Len(InputEOL)
    Set Script = New clsATCscriptExpression
    Script.ParseExpression ScriptString
    'MsgBox Script.Printable("", "        "), vbOKOnly, "Script read"
    Set ScriptFromString = Script
  End If
  Exit Function
ErrParse:
  'MsgBox err.Description & vbCr & "Probably a paren out of place in a script", vbCritical, "Error in ScriptFromString"
End Function

Public Function ScriptOpenDataFile(DataFilename As String) As String
  Dim DataFile As Integer       'File handle for data file being read
  DataFile = FreeFile(0)
  Open DataFilename For Input As DataFile
  LenDataFile = LOF(DataFile)
  WholeDataFile = Input(LenDataFile, DataFile)
  CurrentLine = Left(WholeDataFile, 1000)
  LenCurrentLine = 0
  NextLineStart = 1
  Close DataFile
  ScriptOpenDataFile = "OK"
  Exit Function
  
ErrorOpen:
  ScriptOpenDataFile = "Error opening data file: " & DataFilename & vbCr & err.Description
End Function

Public Function ScriptTest(Script As clsATCscriptExpression, DataFilename$) As String
  Dim msg As String
  TestingFile = True
  ScriptInit
  If pDataFilename = DataFilename And LenDataFile > 0 Then
    NextLineStart = 1
    LastPercent = 0
    CurrentLine = ""
    LenCurrentLine = 0
    CurrentRepeat = 1
    ScriptTest = Script.Evaluate
  Else
    pDataFilename = DataFilename
    msg = ScriptOpenDataFile(DataFilename)
    If msg <> "OK" Then
      MsgBox msg, vbOKOnly, "Data Import"
      ScriptTest = "0"
    Else
      ScriptTest = Script.Evaluate
    End If
  End If
End Function

Public Function ScriptRun(Script As clsATCscriptExpression, DataFilename$, TserFile As ATCclsTserFile) As String
  Dim msg As String
  Dim tmpData As ATCclsTserData
  pDataFilename = DataFilename
  Set pTserFile = TserFile
  
  ScriptInit
  msg = ScriptOpenDataFile(DataFilename)
  If msg <> "OK" Then
    MsgBox msg, vbOKOnly, "Data Import"
    ScriptRun = msg
  Else
    If pMonitorSet Then
      pMonitor.SendMonitorMessage "(MSG1 Reading)"
      pMonitor.SendMonitorMessage "(MSG2 0)"
      pMonitor.SendMonitorMessage "(MSG3 0)"
      pMonitor.SendMonitorMessage "(MSG4 100)"
    End If
    On Error GoTo 0
    
    If DebuggingScript Then
      frmDebugScript.ShowScript Script
      'frmDebugScript.Show 'vbModal 'FIXME vbModal conflict between GenScn and WDMUtil?
    End If
    TestingFile = False
    ScriptRun = Script.Evaluate
    
    If Not AbortScript Then
      For CurBuf = 1 To UBound(InBuf)
        With InBuf(CurBuf)
          If .DateCount > 0 Then
            If .DateDim > .DateCount Then
              ReDim Preserve .DateArray(.DateCount)
              ReDim Preserve .ValueArray(.DateCount)
              ReDim Preserve .FlagArray(.DateCount)
            End If
            pTserData(CurBuf).Values = .ValueArray
            pTserData(CurBuf).flags = .FlagArray
            pTserData(CurBuf).Dates.Values = .DateArray
            
            pTserData(CurBuf).Dates.calcSummary
            pTserData(CurBuf).calcSummary
            pTserData(CurBuf).Header.id = CurBuf
            If FillTS > 0 Then
              Set tmpData = pTserData(CurBuf).FillValues(FillTS, FillTU, FillVal, FillMissing, FillAccum)
              pTserFile.AddTimSer tmpData
              Set tmpData = Nothing
            Else
              pTserFile.AddTimSer pTserData(CurBuf), TsIdRenum
            End If
          End If
        End With
      Next
    End If
  End If
End Function
