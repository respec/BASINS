Option Strict Off
Option Explicit On

Imports atcUtility
Imports atcData
Imports MapWinUtility

Friend Module modATCscript
    'Copyright 2002 by AQUA TERRA Consultants

    Friend DebugScriptForm As frmDebugScript
    Public ScriptState As atcCollection 'of variable names (as keys) and values

    Private DataFileHandle As System.IO.StreamReader = Nothing
    Private DataFileBuffer() As Char = Nothing 'Only used when InputLineLen is set (for fixed-length lines)
    Private WholeDataFile As String 'Contains entire contents of data file
    Public LenDataFile As Long
    Public NextLineStart As Long 'One-based index in WholeDataFile of first character of next line to be read
    Private LastPercent As Integer 'For updating status messages
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
    Public FillTS As Integer = 0
    Public FillVal As Double = 0
    Public FillMissing As Double = -999
    Public FillAccum As Double = -998

    ''' <summary>
    ''' Array of token names
    ''' Be sure to synchronize with Private Enum ATCsToken in clsATCscriptExpression
    ''' </summary>
    ''' <remarks></remarks>
    Public TokenString As String() = {"Unknown", "And", "ATCScript", "Attribute", "ColumnFormat", "Comment", "Dataset", "Date", "FatalError", "Fill", "Flag", "For", "If", "In", "Increment", "Instr", "IsNumeric", "LineEnd", "Literal", "+", "/", "*", "^", "-", "Mid", "NextLine", "Not", "Or", "Save", "Set", "Test", "Trim", "Unset", "Value", "Variable", "Warn", "While", ">", ">=", "<", "<=", "<>", "=", "Last"}

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
            DateArray(0) = GetNaN()
            ValueArray(0) = GetNaN()

            ts = New atcData.atcTimeseries(pTserFile)
            With ts
                .Attributes.SetValue("Description", pDataFilename)
                .Attributes.SetValue("Scenario", DefaultScenario)
                .Attributes.SetValue("Data Source", pDataFilename)
                .Attributes.AddHistory("Read From " & pDataFilename)
            End With
        End Sub
    End Class

    'Private InBuf() As InputBuffer
    Private InBuf As List(Of InputBuffer)
    Private CurBuf As InputBuffer

    Private Sub ScriptInit()
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

    Public Function ScriptEndOfData() As Boolean
        If NextLineStart >= LenDataFile AndAlso LenCurrentLine = 0 Then ScriptEndOfData = True Else ScriptEndOfData = False
    End Function

    Public Sub ScriptNextLine()
        Dim percent As Integer = LastPercent
        Do
            If NextLineStart > LenDataFile Then
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
            Logger.Dbg("(MSG1 " & Left(CurrentLine, 100) & ")")
            Logger.Progress(percent, 100)
            LastPercent = percent
            System.Windows.Forms.Application.DoEvents()
        End If
        'Debug.Print "NextLine: " & Format(CurrentLineNum, "00") & " " & CurrentLine
        If DebuggingScript Then DebugScriptForm.NextLine()
    End Sub

    Public Function ScriptSetDate(ByRef jdy As Double) As Double
        With CurBuf
            .DateCount = .DateCount + 1
            If .DateCount > .DateDim Then
                .DateDim = .DateCount * 2
                ReDim Preserve .DateArray(.DateDim)
                ReDim Preserve .ValueArray(.DateDim)
                'ReDim Preserve .FlagArray(.DateDim)
            End If
            .DateArray(.DateCount) = jdy
            If DebuggingScript Then DebugScriptForm.NewDate(.DateCount, jdy)
            'Debug.Print "Date " & .DateCount & "=" & jdy;
        End With
        ScriptSetDate = jdy
    End Function

    Public Function ScriptSetValue(ByRef newValue As Single) As Single
        CurBuf.ValueArray(CurBuf.DateCount) = newValue
        ScriptSetValue = newValue
        If DebuggingScript Then DebugScriptForm.NewValue(CurBuf.DateCount, newValue)
        'Debug.Print " Value " & pDateCount & "=" & newValue
    End Function

    Public Function ScriptSetFlag(ByRef newValue As Integer) As Integer
        'CurBuf.FlagArray(CurBuf.DateCount) = newValue
        ScriptSetFlag = newValue
        Logger.Dbg("ScriptSetFlag: no action taken (" & newValue & ")")
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
                If LenDataFile < 100000 Then
                    WholeDataFile = IO.File.ReadAllText(aDataFilename)
                    LenDataFile = WholeDataFile.Length
                    CurrentLine = Left(WholeDataFile, 1000)
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
                    For Each lBuffer As InputBuffer In InBuf
                        With lBuffer
                            If .DateCount > 0 Then
                                If .DateDim > .DateCount Then
                                    ReDim Preserve .DateArray(.DateCount)
                                    ReDim Preserve .ValueArray(.DateCount)
                                    'ReDim Preserve .FlagArray(.DateCount)
                                End If
                                .ts.Values = .ValueArray
                                'TODO: import flags if any as ValueAttributes: pTserData.Item(CurBuf).flags = .FlagArray
                                .ts.Dates = New atcTimeseries(pTserFile)
                                .ts.Dates.Values = .DateArray

                                If FillTS > 0 Then
                                    Dim lFilledTS As atcTimeseries = FillValues(.ts, FillTU, FillTS, FillVal, FillMissing, FillAccum)
                                    lFilledTS.Attributes.ChangeTo(.ts.Attributes)
                                    .ts.Dates.Clear()
                                    .ts.Clear()
                                    .ts = lFilledTS
                                End If

                                With .ts
                                    .Attributes.SetValueIfMissing("ID", pTserFile.DataSets.Count + 1)
                                    If Not .Attributes.ContainsAttribute("History 1") Then
                                        .Attributes.SetValue("Data Source", pDataFilename)
                                        .Attributes.AddHistory("Read From " & DataFilename)
                                    End If
                                    'Set missing values to NaN
                                    Dim lNaN As Double = GetNaN()
                                    For iVal As Long = 1 To .numValues
                                        If Math.Abs((.Value(iVal) - FillMissing)) < 1.0E-20 Then
                                            .Value(iVal) = lNaN
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
                                            .ts.Dates = lExistingTS.Dates
                                            lDisposingDates.Clear()
                                            Exit For
                                        End If
                                    End If
                                Next

                                pTserFile.AddDataSet(.ts, atcDataSource.EnumExistAction.ExistRenumber)

                                'If requested, compute Hamon PET from .ts which is assumed to be sub-daily temperature
                                If .ts.Attributes.ContainsAttribute("Hamon") AndAlso .ts.Attributes.ContainsAttribute("Latitude") Then
                                    Dim lCTS() As Double = {0, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}
                                    For lCTSindex As Integer = 1 To 12 'Look for any specified coefficients in attributes, e.g. HamonCTS1 = January coefficient
                                        lCTS(lCTSindex) = .ts.Attributes.GetValue("HamonCTS" & lCTSindex, lCTS(lCTSindex))
                                    Next
                                    Dim lHamonTS As atcTimeseries = atcMetCmp.PanEvaporationTimeseriesComputedByHamonX( _
                                        .ts, pTserFile, _
                                        (.ts.Attributes.GetValue("Units", "TF") = "TF" OrElse .ts.Attributes.GetValue("Hamon", "F") = "F"), _
                                        .ts.Attributes.GetValue("Latitude"), lCTS)
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
            End If
        End Try
        Return lReturnValue
    End Function

    Friend Sub SaveIn(ByVal aFileName As String)
        Dim lSaveIn As atcDataSource = atcDataManager.DataSourceBySpecification(aFileName)
        If pTserFile Is Nothing Then
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

End Module