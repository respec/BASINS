Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcData
Imports atcMetCmp
Imports MapWinUtility

Friend Module modATCscript
    'Copyright 2002 by AQUA TERRA Consultants

    Friend DebugScriptForm As frmDebugScript
    Public ScriptState As atcCollection 'of variable names (as keys) and values
    Public MaxFileLenReadAll As Integer = 100000
    Friend DataFileHandle As System.IO.StreamReader = Nothing 'Read file this way if it is longer than MaxFileLenReadAll
    Private DataFileBuffer() As Char = Nothing 'Only used when InputLineLen is set (for fixed-length lines)
    Private WholeDataFile As String 'Contains entire contents of data file if smaller than MaxFileLenReadAll
    Public LenDataFile As Long
    Public NextLineStart As Long 'One-based index in WholeDataFile of first character of next line to be read
    Private LastPercent As Integer 'For updating status messages
    Public CurrentLine As String 'Current line of data file being parsed
    Public LenCurrentLine As Integer
    Public CurrentRepeat As Integer 'Current repeating part within CurrentLine (>=1)
    Public TestingFile As Boolean 'True if we are just testing, False if we are reading data
    Public FixedColumns As Boolean 'True if columns are fixed width
    Public ColumnDelimiter As String = "" 'character that delimits columns if FixedColumns is False
    Public ColDefs() As clsATCscriptExpression.ColDef 'Names of columns (and start/width if FixedColumns) (1..NamedColumns)
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
    Public FillTS As Integer = 0
    Public FillVal As Double = 0
    Public FillMissing As Double = -999
    Public FillAccum As Double = -998

    Friend pNaN As Double = GetNaN()
    Public MissingValueString As String = ""
    Public DatesAllShared As Boolean = False

    Public WarnedAboutCannotIncrement As Boolean = False
    Public WarnedAboutNonNumericValue As Boolean = False
    Public WarnedAboutNonNumericDataset As Boolean = False


    ''' <summary>
    ''' Array of token names
    ''' Be sure to synchronize with Private Enum ATCsToken in clsATCscriptExpression
    ''' </summary>
    ''' <remarks></remarks>
    Public TokenString As String() = {"Unknown", "Abs", "And", "ATCScript", "Attribute", "ColumnFormat", "ColumnValue", "Comment", "Dataset", "Date", "FatalError", "Fill", "Flag", "For", "If", "In", "Increment", "Instr", "IsNumeric", "Len", "LineEnd", "Literal", "+", "/", "*", "^", "-", "Mid", "NextLine", "Not", "NumColumns", "Or", "Save", "Set", "Test", "Trim", "Unset", "Value", "Variable", "Warn", "While", ">", ">=", "<", "<=", "<>", "=", "Last"}

    Private Const DefaultScenario As String = "ScriptRead"
    Private pTserFile As atcTimeseriesSource
    Private pSaveIn As atcTimeseriesSource
    Private pDataFilename As String

    Private Class InputBuffer
        Public DateCount As Integer = 0
        Public DateDim As Integer = 0
        Public DateArray() As Double
        Public ValueArray() As Double
        'Public FlagArray() As Integer
        Public ts As atcTimeseries

        Public Sub New()
            ReDim DateArray(0)
            ReDim ValueArray(0)
            'ReDim FlagArray(0)
            DateArray(0) = pNaN
            ValueArray(0) = pNaN

            ts = New atcData.atcTimeseries(pTserFile)
            With ts.Attributes
                .SetValue("Description", pDataFilename)
                .SetValue("Scenario", DefaultScenario)
                .SetValue("Data Source", pDataFilename)
                .AddHistory("Read From " & pDataFilename)
            End With
        End Sub
    End Class

    'Private InBuf() As InputBuffer
    Private InBuf As List(Of InputBuffer)
    Private CurBuf As InputBuffer

    ''' <summary>
    ''' Initialize state of script handling.
    ''' Used before running a script, before doing a "test run" of a script, and before opening the wizard that helps build a new script
    ''' </summary>
    Public Sub ScriptInit()
        'ReDim InBuf(0)
        InBuf = New List(Of InputBuffer)

        'pTserData = New atcTimeseriesGroup
        AddNewTserAndBuffer()

        ScriptState = New atcCollection
        ScriptState.Add("Filename", pDataFilename)
        If DataFileHandle IsNot Nothing Then
            DataFileHandle.Close()
            DataFileHandle = Nothing
        End If
        WholeDataFile = ""
        LenDataFile = 0
        NextLineStart = 1
        LastPercent = 0
        CurrentLine = ""
        LenCurrentLine = 0
        CurrentRepeat = 1
        'ColumnDelimiter = ""
        ReDim ColDefs(0)
        NamedColumns = 0
        RepeatStartCol = 0
        '  RepeatEndCol = 0
        CurrentLineNum = 0
        FillTS = 0
        AbortScript = False
        WarnedAboutCannotIncrement = False
        WarnedAboutNonNumericValue = False
        WarnedAboutNonNumericDataset = False
    End Sub

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

    'Returns position of next character from chars in str
    'Returns len(str) + 1 if none were found
    Friend Function FirstCharPos(ByRef start As Integer, ByRef aString As String, ByRef chars As String) As Integer
        Dim CharPos, retval, curval, LenChars As Integer
        retval = Len(aString) + 1
        LenChars = Len(chars)
        For CharPos = 1 To LenChars
            curval = InStr(start, aString, Mid(chars, CharPos, 1))
            If curval > 0 And curval < retval Then retval = curval
        Next CharPos
        Return retval
    End Function

    'Specialized version of FirstCharPos
    Friend Function FirstDelimPos(ByRef start As Integer) As Integer
        Dim curval As Integer
        Dim retval As Integer = Integer.MaxValue
        For Each lDelimiter As Char In ColumnDelimiter.ToCharArray
            curval = InStr(start, CurrentLine, lDelimiter)
            If curval > 0 AndAlso curval < retval Then retval = curval
        Next
        If retval = Integer.MaxValue Then retval = 0
        Return retval
    End Function

    Public Function ScriptEndOfData() As Boolean
        Return NextLineStart >= LenDataFile AndAlso LenCurrentLine = 0
    End Function

    Public Sub ScriptNextLine()
        Dim percent As Integer = LastPercent
        Do
            If NextLineStart > LenDataFile OrElse _
            (DataFileHandle IsNot Nothing AndAlso DataFileHandle.EndOfStream()) Then
                CurrentLine = ""
                LenCurrentLine = 0
                Exit Sub
            End If
            If InputLineLen > 0 Then 'All lines are same length
                If DataFileHandle IsNot Nothing Then
                    If DataFileBuffer Is Nothing Then ReDim DataFileBuffer(3000)
                    DataFileHandle.ReadBlock(DataFileBuffer, 0, InputLineLen)
                    CurrentLine = New String(DataFileBuffer, 0, InputLineLen)
                Else
                    CurrentLine = Mid(WholeDataFile, NextLineStart, InputLineLen)
                End If
                LenCurrentLine = Len(CurrentLine)
                NextLineStart = NextLineStart + LenCurrentLine
            Else
                If DataFileHandle IsNot Nothing Then
                    Dim sb As New Text.StringBuilder
                    Dim ch As Char
                    Try
                        If (InputEOL = vbCr OrElse InputEOL = vbLf) Then
                            Do
                                'Convert.ToChar raises an exception when reading -1 at EOF, this is caught below.
                                ch = Convert.ToChar(DataFileHandle.Read()) : NextLineStart += 1
                                Select Case ch
                                    Case vbLf : Exit Do
                                    Case vbCr
                                        If DataFileHandle.Peek() = 10 Then
                                            DataFileHandle.Read() : NextLineStart += 1 'Skip LF after CR
                                        End If
                                        Exit Do
                                End Select
                                sb.Append(ch)
                            Loop
                        ElseIf LenInputEOL = 1 Then
                            Do
                                ch = Convert.ToChar(DataFileHandle.Read()) : NextLineStart += 1
                                If ch = InputEOL Then Exit Do
                                sb.Append(ch)
                            Loop
                        Else
                            Do
                                sb.Append(Convert.ToChar(DataFileHandle.Read())) : NextLineStart += 1
                            Loop Until sb.ToString.EndsWith(InputEOL)
                            sb.Remove(sb.Length - LenInputEOL, LenInputEOL)
                        End If
                    Catch 'when we try to read past end of file, make sure Do Loop exits
                        NextLineStart = LenDataFile + 1
                    End Try
                    CurrentLine = sb.ToString
                    LenCurrentLine = Len(CurrentLine)
                    percent = NextLineStart / (LenDataFile / 100)
                Else
                    Dim EOLPos As Long
                    If (InputEOL = vbCr OrElse InputEOL = vbLf) Then
                        EOLPos = FirstStringPos(NextLineStart, WholeDataFile, vbCr, vbLf)
                    Else
                        EOLPos = InStr(CInt(NextLineStart), WholeDataFile, InputEOL)
                    End If
                    If EOLPos = 0 Then EOLPos = LenDataFile + 1
                    LenCurrentLine = EOLPos - NextLineStart
                    CurrentLine = Mid(WholeDataFile, NextLineStart, LenCurrentLine)
                    NextLineStart = NextLineStart + LenCurrentLine + LenInputEOL
                    If (NextLineStart < LenDataFile) Then 'Skip LF after CR
                        If Mid(WholeDataFile, EOLPos, 1) = vbCr Then
                            If Mid(WholeDataFile, NextLineStart, 1) = vbLf Then
                                NextLineStart += 1
                            End If
                        End If
                    End If
                    percent = 100 * NextLineStart / LenDataFile
                End If
            End If
            CurrentLineNum += 1
        Loop While Len(Trim(CurrentLine)) = 0

        If percent <> LastPercent AndAlso Not TestingFile Then
            Dim lProgressMessage As String = SafeSubstring(CurrentLine, 0, 100)
            If Not FixedColumns AndAlso ColumnDelimiter IsNot Nothing AndAlso ColumnDelimiter <> " " Then
                lProgressMessage = lProgressMessage.Replace(ColumnDelimiter, " ")
            End If
            Logger.Progress(lProgressMessage, percent, 100)
            LastPercent = percent
            System.Windows.Forms.Application.DoEvents()
        End If
        'Debug.Print "NextLine: " & Format(CurrentLineNum, "00") & " " & CurrentLine
        If DebuggingScript Then DebugScriptForm.NextLine()
    End Sub

    Public Function ScriptSetDate(ByRef jdy As Double) As Double
        Dim lDateBuf As InputBuffer
        If DatesAllShared Then
            lDateBuf = InBuf.Item(0)
        Else
            lDateBuf = CurBuf
        End If
        With lDateBuf
            .DateCount += 1
            If DatesAllShared Then
                For Each lBuf In InBuf
                    lBuf.DateCount = .DateCount
                Next
            End If
            If .DateCount > .DateDim Then
                .DateDim = .DateCount * 1.5
                ReDim Preserve .DateArray(.DateDim)
                If DatesAllShared Then
                    For Each lBuf In InBuf
                        lBuf.DateDim = .DateDim
                        ReDim Preserve lBuf.ValueArray(lBuf.DateDim)
                    Next
                Else
                    ReDim Preserve .ValueArray(.DateDim)
                End If
                'ReDim Preserve .FlagArray(.DateDim)
            End If
            .DateArray(.DateCount) = jdy
            If DebuggingScript Then DebugScriptForm.NewDate(.DateCount, jdy)
            'Debug.Print "Date " & .DateCount & "=" & jdy;
        End With
        Return jdy
    End Function

    Public Function ScriptSetValue(ByRef newValue As Double) As Double
        CurBuf.ValueArray(CurBuf.DateCount) = newValue
        If DebuggingScript Then DebugScriptForm.NewValue(CurBuf.DateCount, newValue)
        'Debug.Print " Value " & pDateCount & "=" & newValue
        Return newValue
    End Function

    Public Function ScriptSetFlag(ByRef newValue As Integer) As Integer
        'CurBuf.FlagArray(CurBuf.DateCount) = newValue
        ScriptSetFlag = newValue
        Logger.Dbg("ScriptSetFlag: no action taken (" & newValue & ")")
    End Function

    Public Function ScriptSetVariable(ByRef VarName As String, ByRef newValue As String) As String
        Static ShowedNumericMessage As Integer
        Static ShowedRangeMessage As Integer
        Select Case VarName.ToLower
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
            Case "datesallshared"
                DatesAllShared = EvalTruth(newValue)
                'Case "missingvalue"
                '    MissingValueString = newValue
                '    GoTo SetVariable
            Case Else
SetVariable:
                On Error Resume Next
                ScriptState.RemoveByKey(VarName)
                ScriptState.Add(VarName, newValue)
        End Select
    End Function

    Public Function ScriptUnsetVariable(ByRef VarName As String) As String
        On Error Resume Next
        ScriptState.Remove(VarName)
        ScriptUnsetVariable = VarName
    End Function

    Public Sub ScriptSetDataset(ByRef index As Integer)
        While index > InBuf.Count()
            AddNewTserAndBuffer()
        End While
        CurBuf = InBuf.Item(index - 1)
        'Logger.Dbg("ScriptSetDataset to " & index & " serial " & CurBuf.ts.Serial & " " & CurBuf.ts.Attributes.ToString)
    End Sub

    Private Sub AddNewTserAndBuffer()
        CurBuf = New InputBuffer
        InBuf.Add(CurBuf)
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
                While tserNum < InBuf.Count() And MisMatch
                    MisMatch = False
                    attrNum = 0
                    While attrNum < NumAttribs And Not MisMatch
                        attrNum = attrNum + 1
                        AttrName = AttrNames(attrNum)
                        attrValue = AttrValues(attrNum)
                        With InBuf(tserNum).ts.Attributes
                            Select Case LCase(AttrName)
                                Case "con", "cons", "constituent"
                                    If .ContainsAttribute("Constituent") Then
                                        If .GetValue("Constituent") <> attrValue Then MisMatch = True
                                    End If
                                Case "sen", "scen", "scenario"
                                    If .ContainsAttribute("Scenario") AndAlso .GetValue("Scenario") <> DefaultScenario Then
                                        If .GetValue("Scenario") <> attrValue Then MisMatch = True
                                    End If
                                Case "loc", "location"
                                    If .ContainsAttribute("Location") Then
                                        If .GetValue("Location") <> attrValue Then MisMatch = True
                                    End If
                                Case "des", "desc", "description"
                                    If .ContainsAttribute("Description") AndAlso .GetValue("Description") <> pDataFilename Then
                                        If .GetValue("Description") <> attrValue Then MisMatch = True
                                    End If

                                Case Else
                                    tmp = .GetValue(AttrName, "")
                                    If tmp <> "" Then
                                        If tmp <> attrValue Then MisMatch = True
                                    End If
                            End Select
                        End With
                    End While 'attrNum
                    tserNum += 1
                End While 'tserNum
                tserNum -= 1
                If MisMatch Then
                    AddNewTserAndBuffer()
                Else
                    CurBuf = InBuf.Item(tserNum)
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
        If InBuf.Count = 0 Then
            Exit Sub
        End If
        With CurBuf.ts.Attributes
            Select Case LCase(AttrName)
                Case "con", "cons", "constituent"
                    .SetValue("Constituent", newValue)
                Case "sen", "scen", "scenario"
                    .SetValue("Scenario", newValue)
                Case "loc", "location"
                    .SetValue("Location", newValue)
                Case "des", "desc", "description"
                    .SetValue("Description", newValue)
                Case Else
                    .SetValue(AttrName, newValue)
            End Select
            Logger.Dbg("SetAttribute " & AttrName & " = " & newValue)
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

    Public Function ScriptOpenDataFile(ByVal aDataFilename As String) As String
        If IO.File.Exists(aDataFilename) Then
            Try
                LenDataFile = FileLen(aDataFilename)
                If LenDataFile < MaxFileLenReadAll Then
                    WholeDataFile = IO.File.ReadAllText(aDataFilename)
                    LenDataFile = WholeDataFile.Length
                    CurrentLine = Left(WholeDataFile, 1000)
                    DataFileHandle = Nothing
                Else
                    DataFileHandle = New System.IO.StreamReader(aDataFilename)
                End If
                LenCurrentLine = 0
                NextLineStart = 1
                Return "OK"
            Catch ex As Exception
                Return "Error opening data file: " & aDataFilename & vbCr & Err.Description
            End Try
        Else
            Return "File not found: " & aDataFilename
        End If
    End Function

    Public Function EvalTruth(ByRef aString As String) As Boolean
        Select Case aString.ToLower
            Case "0", "", "false"
                Return False
        End Select
        Return True
    End Function

    ''' <summary>
    ''' Run the "Test" branch of the script to see whether this script thinks it can read this file
    ''' </summary>
    ''' <param name="Script"></param>
    ''' <param name="DataFilename"></param>
    ''' <returns></returns>
    Public Function ScriptTest(ByRef Script As clsATCscriptExpression, ByRef DataFilename As String) As String
        Dim msg As String
        TestingFile = True
        ScriptInit()
        If pDataFilename = DataFilename AndAlso LenDataFile > 0 Then
            NextLineStart = 1
            LastPercent = 0
            CurrentLine = ""
            LenCurrentLine = 0
            CurrentRepeat = 1
            Return Script.Evaluate
        Else
            pDataFilename = DataFilename
            msg = ScriptOpenDataFile(DataFilename)
            If msg <> "OK" Then
                MsgBox(msg, MsgBoxStyle.OkOnly, "Data Import")
                Return "0"
            Else
                Return Script.Evaluate
            End If
        End If
    End Function

    Public Function ScriptRun(ByRef Script As clsATCscriptExpression, ByRef DataFilename As String, ByRef TserFile As atcTimeseriesSource) As String
        Dim lReturnValue As String = Nothing
        Try
            pDataFilename = DataFilename
            pTserFile = TserFile

            ScriptInit()
            lReturnValue = ScriptOpenDataFile(DataFilename)
            If lReturnValue <> "OK" Then
                Logger.Msg(lReturnValue, MsgBoxStyle.OkOnly, "Data Import")
            Else
                Logger.Dbg("(MSG1 Reading)")
                Logger.Dbg("(MSG2 0)")
                Logger.Dbg("(MSG3 0)")
                Logger.Dbg("(MSG4 100)")

                If DebuggingScript Then
                    DebugScriptForm = New frmDebugScript
                    DebugScriptForm.ShowScript(Script)
                End If
                TestingFile = False
                lReturnValue = Script.Evaluate

                If Not AbortScript Then
                    Dim lAllDates As atcTimeseries = Nothing
                    Dim lSharedAttributes As New atcDataAttributes
                    With lSharedAttributes
                        .SetValue("Data Source", pDataFilename)
                        .AddHistory("Read From " & DataFilename)
                    End With
                    Dim lProgress As Integer = 0
                    Dim lLastProgress As Integer = InBuf.Count
                    Logger.Status("Compiling imported data into time series")
                    For Each lBuffer As InputBuffer In InBuf
                        With lBuffer
                            lProgress += 1
                            Logger.Progress(lProgress, lLastProgress)
                            If .DateCount > 0 Then
                                If .DateDim > .DateCount Then
                                    If Not DatesAllShared OrElse DatesAllShared AndAlso lAllDates Is Nothing Then
                                        ReDim Preserve .DateArray(.DateCount)
                                    End If
                                    ReDim Preserve .ValueArray(.DateCount)
                                    'ReDim Preserve .FlagArray(.DateCount)
                                End If
                                .ts.Values = .ValueArray
                                'TODO: import flags if any as ValueAttributes: pTserData.Item(CurBuf).flags = .FlagArray
                                If DatesAllShared Then
                                    If lAllDates Is Nothing Then
                                        lAllDates = New atcTimeseries(pTserFile)
                                        lAllDates.Values = .DateArray
                                        lAllDates.Attributes.SetValue("Shared", True)
                                    End If
                                    .ts.Dates = lAllDates
                                Else
                                    .ts.Dates = New atcTimeseries(pTserFile)
                                    .ts.Dates.Values = .DateArray
                                End If

                                If FillTS > 0 Then
                                    Dim lFilledTS As atcTimeseries = FillValues(.ts, FillTU, FillTS, FillVal, FillMissing, FillAccum)
                                    For Each lAttribute As atcDefinedValue In .ts.Attributes
                                        lFilledTS.Attributes.SetValueIfMissing(lAttribute.Definition.Name, lAttribute.Value)
                                    Next
                                    If Not DatesAllShared Then
                                        .ts.Dates.Clear()
                                    End If
                                    .ts.Clear()
                                    .ts = lFilledTS
                                Else
                                    .ts.SetInterval()
                                End If

                                With .ts
                                    .Attributes.SetValueIfMissing("ID", pTserFile.DataSets.Count + 1)
                                    .Attributes.SharedAttributes = lSharedAttributes
                                    'Set missing values to NaN
                                    For iVal As Long = 1 To .numValues
                                        If Math.Abs((.Value(iVal) - FillMissing)) < 1.0E-20 Then
                                            .Value(iVal) = pNaN
                                        End If
                                    Next
                                End With

                                'Re-use existing dates if this TS has same dates as another
                                For Each lExistingTS As atcTimeseries In pTserFile.DataSets
                                    If Not lExistingTS.ValuesNeedToBeRead AndAlso Not lExistingTS.Dates.ValuesNeedToBeRead AndAlso lExistingTS.Dates.numValues = .ts.Dates.numValues Then
                                        Dim lMatch As Boolean = True
                                        For lIndex As Long = 1 To lExistingTS.Dates.numValues
                                            If .ts.Dates.Value(lIndex) <> lExistingTS.Dates.Value(lIndex) Then
                                                lMatch = False
                                                Exit For
                                            End If
                                        Next
                                        If lMatch Then
                                            Dim lDisposingDates As atcTimeseries = .ts.Dates
                                            lExistingTS.Dates.Attributes.SetValue("Shared", True)
                                            .ts.Dates = lExistingTS.Dates
                                            If Not lDisposingDates.Attributes.GetValue("Shared", False) Then
                                                lDisposingDates.Clear()
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next

                                pTserFile.AddDataSet(.ts, atcDataSource.EnumExistAction.ExistRenumber)

                                'If requested, compute Hamon PET from .ts which is assumed to be sub-daily temperature
                                If (.ts.Attributes.ContainsAttribute("Hamon") OrElse .ts.Attributes.ContainsAttribute("HamonDaily")) AndAlso .ts.Attributes.ContainsAttribute("Latitude") Then
                                    Dim lLatitude As Double = .ts.Attributes.GetValue("Latitude")
                                    Dim lCTS() As Double = {0, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}
                                    For lCTSindex As Integer = 1 To 12 'Look for any specified coefficients in attributes, e.g. HamonCTS1 = January coefficient
                                        lCTS(lCTSindex) = .ts.Attributes.GetValue("HamonCTS" & lCTSindex, lCTS(lCTSindex))
                                    Next
                                    Dim lHamonTS As atcTimeseries = atcMetCmp.PanEvaporationTimeseriesComputedByHamonX( _
                                        .ts, pTserFile, _
                                        (.ts.Attributes.GetValue("Units", "TF") = "TF" OrElse .ts.Attributes.GetValue("Hamon", "F") = "F"), _
                                        lLatitude, lCTS)
                                    If Not .ts.Attributes.ContainsAttribute("HamonDaily") Then
                                        lHamonTS = atcMetCmp.DisSolPet(lHamonTS, pTserFile, 2, lLatitude)
                                    End If
                                    pTserFile.AddDataSet(lHamonTS, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                                End If

                                If pTserFile.CanSave Then 'Free memory now that data has been saved
                                    .ts.ValuesNeedToBeRead = True
                                End If
                            End If
                        End With
                    Next
                End If
            End If
        Finally
            If DataFileHandle IsNot Nothing Then
                DataFileHandle.Close()
                DataFileHandle = Nothing
                If pTserFile IsNot Nothing AndAlso pTserFile.GetType.Name <> "atcTimeseriesScriptPlugin" Then
                    Dim lSpec As String = pTserFile.Specification
                    atcDataManager.RemoveDataSource(lSpec)
                    pTserFile.Clear()
                End If
            End If
        End Try
        Return lReturnValue
    End Function

    Friend Sub SaveIn(ByVal aFileName As String)
        Dim lSaveIn As atcDataSource = atcDataManager.DataSourceBySpecification(aFileName)
        If lSaveIn Is Nothing Then
            If atcDataManager.OpenDataSource(aFileName) Then
                lSaveIn = atcDataManager.DataSourceBySpecification(aFileName)
            End If
        End If
        If lSaveIn Is Nothing Then
            Logger.Msg("Could not open '" & aFileName & "' so opening in BASINS", "Script Import")
        ElseIf Not lSaveIn.CanSave Then
            Logger.Msg("Could not save in '" & aFileName & "' so opening in BASINS", "Script Import")
        Else
            pTserFile = lSaveIn
        End If
    End Sub

    Public Sub SetDelimiter(aRule As String)
        FixedColumns = False
        ColumnDelimiter = ""
        If IsNumeric(aRule) AndAlso CInt(aRule) >= 0 AndAlso CInt(aRule) < 255 Then
            ColumnDelimiter = Chr(CShort(aRule))
        Else
            Dim lRule As String = aRule.Trim.ToLower
            If lRule = "fixed" Then
                FixedColumns = True
            Else
                If InStr(lRule, "tab") Then ColumnDelimiter &= vbTab
                If InStr(lRule, "space") Then ColumnDelimiter &= " "
                For lCharacter As Integer = 33 To 126
                    Select Case lCharacter
                        Case 48 : lCharacter = 58
                        Case 65 : lCharacter = 91
                        Case 97 : lCharacter = 123
                    End Select
                    If InStr(lRule, Chr(lCharacter)) > 0 Then ColumnDelimiter &= Chr(lCharacter)
                Next lCharacter
            End If
        End If
    End Sub

    Public Function GetDelimiterRule() As String        
        Dim lRule As String = ColumnDelimiter.Replace(vbTab, "tab").Replace(" ", "space")

        For lCharacter As Integer = 33 To 126
            Select Case lCharacter
                Case 42 : lCharacter = 60
                Case 63 : lCharacter = 127
            End Select
            lRule = lRule.Replace(Chr(lCharacter), lCharacter.ToString)
        Next lCharacter

        Return lRule
    End Function

End Module