Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcData

Module modATCscript
    'Copyright 2002 by AQUA TERRA Consultants

    Public DebugScriptForm As frmDebugScript
    Public ScriptState As Collection 'of variable names (as keys) and values
    Public WholeDataFile As String 'Contains entire contents of data file
    Public LenDataFile As Integer
    Public NextLineStart As Integer 'Index in WholeDataFile of first character of next line to be read
    Public LastPercent As Integer 'For updating status messages
    Public CurrentLine As String 'Current line of data file being parsed
    Public LenCurrentLine As Integer
    Public CurrentRepeat As Integer 'Current repeating part within CurrentLine (>=1)
    Public TestingFile As Boolean 'True if we are just testing, False if we are reading data
    Public FixedColumns As Boolean 'True if columns are fixed width
    Public ColumnDelimiter As String 'character that delimits columns if FixedColumns is False
    Public NumColumnDelimiters As Integer 'ColumnDelimiter may contain more than one delimiter character
    Public ColDefs() As clsATCscriptExpression.ColDef 'Names of columns (and start/width if FixedColumns) (1..NamesColumns)
    'ColDefs(0) stores info about the first repeating column
    Public NamedColumns As Integer 'Number of cols defined in ColDefs (there may be gaps if delimited)
    Public RepeatStartCol As Integer 'First column that repeats
    'Public RepeatEndCol As Long      'Last column that repeats

    Public CurrentLineNum As Integer 'Current line of script being printed
    Public DebuggingScript As Boolean
    Public ScriptAssigningLineNumbers As Boolean
    Public AbortScript As Boolean

    Public PrintEOL As String
    Public InputEOL As String
    Public LenInputEOL As Integer
    Public InputLineLen As Integer

    Public FillTU As atcTimeUnit
    Public FillTS As Integer
    Public FillVal As Single
    Public FillMissing As Single
    Public FillAccum As Single

    ''' <summary>
    ''' Array of token names
    ''' Be sure to synchronize with Private Enum ATCsToken in clsATCscriptExpression
    ''' </summary>
    ''' <remarks></remarks>
    Public TokenString As String() = {"Unknown", "And", "ATCScript", "Attribute", "ColumnFormat", "Comment", "Dataset", "Date", "FatalError", "Fill", "Flag", "For", "If", "In", "Increment", "Instr", "IsNumeric", "LineEnd", "Literal", "+", "/", "*", "^", "-", "Mid", "NextLine", "Not", "Or", "Set", "Test", "Trim", "Unset", "Value", "Variable", "Warn", "While", ">", ">=", "<", "<=", "<>", "=", "Last"}

    Private Const DefaultScenario As String = "ScriptRead"
    Private pTserFile As atcTimeseriesSource
    Private pDataFilename As String
    Private pTserData As Collection 'of ATCclsTserData

    Private Structure InputBuffer
        Dim DateCount As Integer
        Dim DateDim As Integer
        Dim DateArray() As Double
        Dim ValueArray() As Single
        Dim FlagArray() As Integer
    End Structure
    Private InBuf() As InputBuffer
    Private CurBuf As Integer

    Private pMonitor As Object
    Private pMonitorSet As Boolean

    Private Sub ScriptInit()
        ReDim InBuf(0)

        pTserData = Nothing
        pTserData = New Collection
        AddNewTserAndBuffer()

        ScriptState = Nothing
        ScriptState = New Collection
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

    'UPGRADE_NOTE: str was upgraded to str_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Function ReadIntLeaveRest(ByRef str_Renamed As String) As Integer
        Dim chpos, lenStr As Integer
        chpos = 1
        If IsNumeric(Left(str_Renamed, 1)) Then
            lenStr = Len(str_Renamed)
            While (IsNumeric(Mid(str_Renamed, chpos, 1)))
                chpos = chpos + 1
                If chpos > lenStr Then
                    ReadIntLeaveRest = CInt(str_Renamed)
                    Exit Function
                End If
            End While
            ReadIntLeaveRest = CInt(Left(str_Renamed, chpos - 1))
            str_Renamed = Mid(str_Renamed, chpos)
        Else
            ReadIntLeaveRest = 0
        End If
    End Function

    Public Sub ScriptSetMonitor(ByRef o As Object)
        pMonitorSet = True
        pMonitor = o
    End Sub

    Public Function ScriptEndOfData() As Boolean
        If NextLineStart >= LenDataFile And LenCurrentLine = 0 Then ScriptEndOfData = True Else ScriptEndOfData = False
    End Function

    Public Sub ScriptNextLine()
        Dim percent As Integer
        Dim EOLPos As Integer
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
                'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pMonitor.SendMonitorMessage("(MSG1 " & Left(CurrentLine, 100) & ")")
                'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pMonitor.SendMonitorMessage("(MSG3 " & CStr(percent) & "%)")
                'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pMonitor.SendMonitorMessage("(PROGRESS " & CStr(percent) & ")")
                LastPercent = percent
                System.Windows.Forms.Application.DoEvents()
            End If
        End If
        'Debug.Print "NextLine: " & Format(CurrentLineNum, "00") & " " & CurrentLine
        If DebuggingScript Then DebugScriptForm.NextLine()
    End Sub

    Public Function ScriptSetDate(ByRef jdy As Double) As Double
        With InBuf(CurBuf)
            .DateCount = .DateCount + 1
            If .DateCount > .DateDim Then
                .DateDim = .DateCount * 2
                ReDim Preserve .DateArray(.DateDim)
                ReDim Preserve .ValueArray(.DateDim)
                ReDim Preserve .FlagArray(.DateDim)
            End If
            .DateArray(.DateCount) = jdy
            If DebuggingScript Then DebugScriptForm.NewDate(.DateCount, jdy)
            'Debug.Print "Date " & .DateCount & "=" & jdy;
        End With
        ScriptSetDate = jdy
    End Function

    Public Function ScriptSetValue(ByRef newValue As Single) As Single
        InBuf(CurBuf).ValueArray(InBuf(CurBuf).DateCount) = newValue
        ScriptSetValue = newValue
        If DebuggingScript Then DebugScriptForm.NewValue(InBuf(CurBuf).DateCount, newValue)
        'Debug.Print " Value " & pDateCount & "=" & newValue
    End Function

    Public Function ScriptSetFlag(ByRef newValue As Integer) As Integer
        InBuf(CurBuf).FlagArray(InBuf(CurBuf).DateCount) = newValue
        ScriptSetFlag = newValue
        'Debug.Print " Flag " & pDateCount & "=" & newValue
    End Function

    Public Function ScriptSetVariable(ByRef VarName As String, ByRef newValue As String) As String
        Static ShowedNumericMessage As Integer
        Static ShowedRangeMessage As Integer
        Select Case LCase(VarName)
            Case "repeat"
                If IsNumeric(newValue) Then
                    CurrentRepeat = CInt(newValue)
                    If CurrentRepeat < 1 Then
                        If ShowedRangeMessage < 2 Then
                            MsgBox("Repeat was set to '" & CurrentRepeat & "' but it should always be >= 1.", MsgBoxStyle.OkOnly, "modATCscript:ScriptSetVariable")
                            ShowedRangeMessage = ShowedRangeMessage + 1
                        End If
                    End If
                Else
                    If ShowedNumericMessage < 2 Then
                        MsgBox("Non-numeric value '" & newValue & "' assigned to Repeat", MsgBoxStyle.OkOnly, "modATCscript:ScriptSetVariable")
                        ShowedNumericMessage = ShowedNumericMessage + 1
                    End If
                End If
            Case Else : On Error Resume Next
                ScriptState.Remove(VarName)
                ScriptState.Add(newValue, VarName)
        End Select
    End Function

    Public Function ScriptUnsetVariable(ByRef VarName As String) As String
        On Error Resume Next
        ScriptState.Remove(VarName)
        ScriptUnsetVariable = VarName
    End Function

    Public Sub ScriptSetDataset(ByRef index As Integer)
        While index > pTserData.Count()
            AddNewTserAndBuffer()
        End While
        CurBuf = index
    End Sub

    Private Sub AddNewTserAndBuffer()
        'Dim newts As ATCclsTserData
        'newts = New ATCclsTserData
        ''Set newts = New ATCclsTserData
        'newts.Header.desc = pDataFilename
        'newts.Header.Sen = DefaultScenario
        'newts.Dates = New ATCclsTserDate
        'newts.AttribSet("FileImported", pDataFilename)

        'pTserData.Add(newts)

        'ReDim Preserve InBuf(UBound(InBuf) + 1)
        'CurBuf = UBound(InBuf)
        'InBuf(CurBuf).DateCount = 0
        'InBuf(CurBuf).DateDim = 0
        'ReDim InBuf(CurBuf).DateArray(0)
        'ReDim InBuf(CurBuf).ValueArray(0)
        'ReDim InBuf(CurBuf).FlagArray(0)
    End Sub

    Public Sub ScriptManageDataset(ByRef cmd As String, Optional ByRef AttrName As String = "", Optional ByRef attrValue As String = "")
        Static AttrNames(99) As String
        Static AttrValues(99) As String
        Static NumAttribs As Integer
        Dim tserNum, attrNum As Integer
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
                While tserNum < pTserData.Count() And MisMatch
                    tserNum = tserNum + 1
                    MisMatch = False
                    attrNum = 0
                    While attrNum < NumAttribs And Not MisMatch
                        attrNum = attrNum + 1
                        AttrName = AttrNames(attrNum)
                        attrValue = AttrValues(attrNum)
                        'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        With pTserData.Item(tserNum).Header
                            Select Case LCase(AttrName)
                                Case "con", "cons", "constituent"
                                    'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    If .con <> "" Then
                                        'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If .con <> attrValue Then MisMatch = True
                                    End If
                                Case "sen", "scen", "scenario"
                                    'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    If .Sen <> "" And .Sen <> DefaultScenario Then
                                        'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If .Sen <> attrValue Then MisMatch = True
                                    End If
                                Case "loc", "location"
                                    'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    If .loc <> "" Then
                                        'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If .loc <> attrValue Then MisMatch = True
                                    End If
                                Case "des", "desc", "description"
                                    'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    If .desc <> "" And .desc <> pDataFilename Then
                                        'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(tserNum).Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                        If .desc <> attrValue Then MisMatch = True
                                    End If

                                Case Else
                                    'UPGRADE_WARNING: Couldn't resolve default property of object pTserData().Attrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    tmp = pTserData.Item(tserNum).Attrib(AttrName, "")
                                    If tmp <> "" Then
                                        If tmp <> attrValue Then MisMatch = True
                                    End If
                            End Select
                        End With
                    End While 'attrNum
                End While 'tserNum
                If MisMatch Then
                    AddNewTserAndBuffer()
                Else
                    CurBuf = tserNum
                End If
                For attrNum = 1 To NumAttribs 'In case some "Matching" values were default or blank, explicitly set them
                    ScriptSetAttribute(AttrNames(attrNum), AttrValues(attrNum))
                Next attrNum
            Case Else
                MsgBox("Unknown command " & cmd & " in ManageDataset", MsgBoxStyle.OkOnly, "modATCscript")
                Stop
        End Select
    End Sub


    Public Sub ScriptSetAttribute(ByRef AttrName As String, ByRef newValue As String)
        If pTserData.Count = 0 Then
            Exit Sub
        End If
        With pTserData.Item(CurBuf).Header
            Select Case LCase(AttrName)
                Case "con", "cons", "constituent"
                    .con = newValue
                Case "sen", "scen", "scenario"
                    .Sen = newValue
                Case "loc", "location"
                    .loc = newValue
                Case "des", "desc", "description"
                    .desc = newValue
                Case Else
                    pTserData.Item(CurBuf).AttribSet(AttrName, newValue)
            End Select
        End With
    End Sub

    Public Function ScriptFromString(ByRef ScriptString As String) As clsATCscriptExpression
        Dim ErrDescription As String = ""
        Dim CountParens, CountPos As Integer
        Try

            CountPos = 1
            While CountPos > 0
                CountPos = InStr(CountPos, ScriptString, "(")
                If CountPos > 0 Then
                    CountPos = CountPos + 1
                    CountParens = CountParens + 1
                End If
            End While

            CountPos = 1
            While CountPos > 0
                CountPos = InStr(CountPos, ScriptString, ")")
                If CountPos > 0 Then
                    CountPos = CountPos + 1
                    CountParens = CountParens - 1
                End If
            End While

            If CountParens > 0 Then
                ErrDescription = "More left parens in script than right parens (" & CountParens & ")" & vbCr & "Not trying to parse this script further." & vbCr & Left(ScriptString, 50) ', |           'vbExclamation, "ScriptFromString"
            ElseIf CountParens < 0 Then
                ErrDescription = "More right parens in script than right parens (" & -CountParens & ")" & vbCr & "Not trying to parse this script further." & vbCr & Left(ScriptString, 50) ', |           'vbExclamation, "ScriptFromString"
            Else
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
                Dim Script As New clsATCscriptExpression
                Script.ParseExpression(ScriptString)
                'MsgBox Script.Printable("", "        "), vbOKOnly, "Script read"
                Return Script
            End If
        Catch e As Exception
            ErrDescription = e.ToString & vbCrLf & "Probably a paren out of place in a script"
        End Try
        MapWinUtility.Logger.Msg(ErrDescription, vbCritical, "Error in ScriptFromString")
        Return Nothing
    End Function

    Public Function ScriptOpenDataFile(ByRef DataFilename As String) As String
        Dim DataFile As Short 'File handle for data file being read
        DataFile = FreeFile()
        FileOpen(DataFile, DataFilename, OpenMode.Input)
        LenDataFile = LOF(DataFile)
        WholeDataFile = InputString(DataFile, LenDataFile)
        CurrentLine = Left(WholeDataFile, 1000)
        LenCurrentLine = 0
        NextLineStart = 1
        FileClose(DataFile)
        ScriptOpenDataFile = "OK"
        Exit Function

ErrorOpen:
        ScriptOpenDataFile = "Error opening data file: " & DataFilename & vbCr & Err.Description
    End Function

    Public Function ScriptTest(ByRef Script As clsATCscriptExpression, ByRef DataFilename As String) As String
        Dim msg As String
        TestingFile = True
        ScriptInit()
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
                MsgBox(msg, MsgBoxStyle.OkOnly, "Data Import")
                ScriptTest = "0"
            Else
                ScriptTest = Script.Evaluate
            End If
        End If
    End Function

    Public Function ScriptRun(ByRef Script As clsATCscriptExpression, ByRef DataFilename As String, ByRef TserFile As atcTimeseriesSource) As String
        Dim msg As String
        'Dim tmpData As ATCclsTserData
        pDataFilename = DataFilename
        pTserFile = TserFile

        ScriptInit()
        msg = ScriptOpenDataFile(DataFilename)
        If msg <> "OK" Then
            MsgBox(msg, MsgBoxStyle.OkOnly, "Data Import")
            ScriptRun = msg
        Else
            If pMonitorSet Then
                pMonitor.SendMonitorMessage("(MSG1 Reading)")
                pMonitor.SendMonitorMessage("(MSG2 0)")
                pMonitor.SendMonitorMessage("(MSG3 0)")
                pMonitor.SendMonitorMessage("(MSG4 100)")
            End If
            On Error GoTo 0

            If DebuggingScript Then
                DebugScriptForm = New frmDebugScript
                DebugScriptForm.ShowScript(Script)
                'DebugScriptForm.Show 'vbModal 'FIXME vbModal conflict between GenScn and WDMUtil?
            End If
            TestingFile = False
            ScriptRun = Script.Evaluate

            If Not AbortScript Then
                For CurBuf As Integer = 1 To UBound(InBuf)
                    With InBuf(CurBuf)
                        If .DateCount > 0 Then
                            If .DateDim > .DateCount Then
                                ReDim Preserve .DateArray(.DateCount)
                                ReDim Preserve .ValueArray(.DateCount)
                                ReDim Preserve .FlagArray(.DateCount)
                            End If
                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData(CurBuf).Values. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).Values = VB6.CopyArray(.ValueArray)
                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData(CurBuf).flags. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).flags = VB6.CopyArray(.FlagArray)
                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData(CurBuf).Dates. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).Dates.Values = VB6.CopyArray(.DateArray)

                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData().Dates. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).Dates.calcSummary()
                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData().calcSummary. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).calcSummary()
                            ''UPGRADE_WARNING: Couldn't resolve default property of object pTserData().Header. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            'pTserData.Item(CurBuf).Header.id = CurBuf
                            'If FillTS > 0 Then
                            '	'UPGRADE_WARNING: Couldn't resolve default property of object pTserData().FillValues. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            '	tmpData = pTserData.Item(CurBuf).FillValues(FillTS, FillTU, FillVal, FillMissing, FillAccum)
                            '	pTserFile.AddTimSer(tmpData)
                            '	'UPGRADE_NOTE: Object tmpData may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                            '	tmpData = Nothing
                            'Else
                            '	'UPGRADE_WARNING: Couldn't resolve default property of object pTserData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            '	pTserFile.AddTimSer(pTserData.Item(CurBuf), ATCDataTypes.ATCTsIdExistAction.TsIdRenum)
                            'End If
                        End If
                    End With
                Next
            End If
        End If
    End Function
End Module