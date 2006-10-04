Imports System.IO

Public Class clsMonitor

    Private Shared frm As frmStatus

    Private Shared lblText(frmStatus.LastLabel) As String
    Private Shared lblLast(frmStatus.LastLabel) As String
    Private Shared lblLogged(frmStatus.LastLabel) As String

    Private Shared LogProcessBuffer As String = ""

    Private Const LogDisplayLines As Integer = 250
    Private Shared LogDisplayBuffer(LogDisplayLines) As String 'Ring buffer of lines of log file to display
    Private Shared LogDisplayNeedsUpdate As Boolean = False
    Private Shared LogDisplayFirstLine As Integer = 0
    Private Shared LogDisplayLastLine As Integer = 0

    Private Shared ProgressNeedsUpdate As Boolean
    Private Shared ProgressStartTime As Double = Double.NaN
    Private Shared ProgressCurrent As Integer
    Private Shared ProgressFinal As Integer

    Private Shared LastUpdate As Double = Double.NaN
    Private Shared UpdateInterval As Double = 2 / 720000.0# ' 1 / 720000.0 = 0.1 second
    Private Shared LogTimeStamps As Boolean = True

    Private Shared LogFile As Integer = -1
    Private Shared LogFileName As String = ""
    Private Shared LogFileIndex As Integer = 0
    Private Shared LogFileOpen As Boolean = False

    Private Shared Exiting As Boolean = False
    Private Shared Watcher As New System.IO.FileSystemWatcher

    Private Shared ParentProcess As Process = Nothing

    Private Shared CRLFchars() As Char = {vbCr, vbLf}

    Public Shared Sub Main()
        LogFile = FreeFile()
        LogFileName = Microsoft.VisualBasic.Command
        Dim lTimeStamp As String = CreateTimeStamp() & vbTab & "DBG "
        Dim lParentID As Integer = StrFirstInt(LogFileName)
        If lParentID > 0 Then
            Try
                ParentProcess = System.Diagnostics.Process.GetProcessById(lParentID)
            Catch ex As Exception
                ProcessInput(lTimeStamp & "Failed to get Parent (" & lParentID & ") " & ex.Message)
            End Try
        End If

        frm = New frmStatus
        frm.Clear()
        frm.Visible = False
        'frm.Show()
        If ParentProcess Is Nothing Then
            frm.Exiting = True 'Let the user quit by closing the form

        End If

        ClearLabels()

        Watcher.Path = System.IO.Path.GetDirectoryName(LogFileName)
        Watcher.NotifyFilter = (NotifyFilters.LastWrite)
        Watcher.Filter = System.IO.Path.GetFileName(LogFileName)

        ProcessInput(Now & vbTab & "Monitoring Started")
        ProcessInput(lTimeStamp & "Path = " & Watcher.Path & " File = " & Watcher.Filter)
        ProcessInput(lTimeStamp & "Parent = " & lParentID)

        ProcessRecentLogEntries() 'Process any existing log before watching for new entries

        AddHandler Watcher.Changed, AddressOf OnChanged
        Watcher.EnableRaisingEvents = True

        ManageInterface()
        Try
            frm.Exiting = True
            frm.Close()
        Catch ex As Exception 'Form might already be closed
        End Try

        Try
            FileClose(LogFile)
        Catch ex As Exception 'File might already be closed
        End Try
    End Sub

    Public Shared Sub ManageInterface()
        Dim NowDouble As Double = Now.ToOADate
        LastUpdate = NowDouble
        While Not Exiting
            If NowDouble - LastUpdate > UpdateInterval Then
                For iLabel As Integer = 0 To frmStatus.LastLabel
                    If Not lblText(iLabel).Equals(lblLast(iLabel)) Then
                        frm.Label(iLabel) = lblText(iLabel)
                        lblLast(iLabel) = lblText(iLabel)
                        lblLogged(iLabel) = lblText(iLabel)
                    End If
                Next
                If LogDisplayNeedsUpdate Then
                    With frm.txtLog
                        .Text = CurrentLogDisplay()
                        .SelectionStart = frm.txtLog.Text.Length
                        .ScrollToCaret()
                        LogDisplayNeedsUpdate = False
                    End With
                End If

                If ProgressNeedsUpdate Then
                    If Double.IsNaN(ProgressStartTime) Then 'Progress is finished
                        frm.Visible = False
                        frm.Progress.Visible = False
                        For iLabel As Integer = 2 To 5
                            frm.Label(iLabel) = lblText(iLabel)
                        Next
                    Else
                        If Not frm.Progress.Visible Then 'See if we should show it
                            If NowDouble - ProgressStartTime > UpdateInterval * 3 Then
                                frm.Label(2) = "0"
                                frm.Label(4) = ProgressFinal
                                frm.Progress.Maximum = ProgressFinal
                                frm.Progress.Visible = True
                                frm.Visible = True
                            End If
                        End If
                        If frm.Progress.Visible Then
                            frm.Progress.Value = ProgressCurrent
                            If lblLogged(2).Length = 0 AndAlso lblLogged(3).Length = 0 AndAlso lblLogged(4).Length = 0 Then
                                frm.Label(3) = ProgressCurrent & " of " & ProgressFinal
                            End If
                            If lblLogged(5).Length = 0 Then frm.Label(5) = CInt(ProgressCurrent * 1000 / ProgressFinal) / 10 & "%"
                        End If
                    End If
                    ProgressNeedsUpdate = False
                End If

                If Not ParentProcess Is Nothing AndAlso ParentProcess.HasExited AndAlso Not frm.Exiting Then
                    Exiting = True
                    frm.Exiting = True
                    frm.Label(0) = "Parent Process Exited"
                    frm.Visible = False 'Needs to not be visible to call ShowDialog
                    frm.ShowDialog()
                End If
                LastUpdate = NowDouble
            Else
                System.Threading.Thread.Sleep(10)
            End If
            Application.DoEvents()
            'TODO: Check for form button presses here?
        End While
    End Sub

    Private Shared Function CurrentLogDisplay() As String
        Dim iLog As Integer = LogDisplayFirstLine
        CurrentLogDisplay = ""
        If iLog > LogDisplayLastLine Then
            While iLog <= LogDisplayLines
                CurrentLogDisplay &= LogDisplayBuffer(iLog) & vbCrLf
                iLog += 1
            End While
            iLog = 0
        End If
        While iLog <= LogDisplayLastLine
            CurrentLogDisplay &= LogDisplayBuffer(iLog) & vbCrLf
            iLog += 1
        End While
    End Function

    Private Shared Sub LogDisplayAddLine(ByVal aNewLine As String)
        LogDisplayLastLine += 1
        If LogDisplayLastLine > LogDisplayLines Then LogDisplayLastLine = 0
        If LogDisplayLastLine = LogDisplayFirstLine Then
            LogDisplayFirstLine += 1
            If LogDisplayFirstLine > LogDisplayLines Then LogDisplayFirstLine = 0
        End If
        LogDisplayBuffer(LogDisplayLastLine) = aNewLine
        LogDisplayNeedsUpdate = True
    End Sub

    Private Shared Sub OnChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)
        ProcessRecentLogEntries()
    End Sub

    Private Shared Sub ProcessRecentLogEntries()
        If Not LogFileOpen Then
TryToOpen:
            Try
                Try
                    FileClose(LogFile)
                Catch ex As Exception 'File might already be closed
                End Try
                FileOpen(LogFile, LogFileName, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
                LogFileOpen = True
                If LogFileIndex > 1 Then Seek(LogFile, LogFileIndex)
            Catch ex As Exception
                If MsgBox("""" & LogFileName & """" & vbCrLf & ex.Message & vbCr & vbCr & "Do you want to find the log file to open?", MsgBoxStyle.YesNo, "Unable to open log file") = MsgBoxResult.Yes Then
                    Dim dlg As New Windows.Forms.OpenFileDialog
                    dlg.Title = "Open log file"
                    dlg.FileName = LogFileName
                    If dlg.ShowDialog = DialogResult.OK Then
                        LogFileName = dlg.FileName
                        GoTo TryToOpen
                    End If
                End If
                LogFileOpen = False
                Exiting = True
                Exit Sub
            End Try
        End If
        Dim FileLength As Integer
        Try
            FileLength = LOF(LogFile)
            LogDisplayAddLine("Log File Length = " & FileLength & " (" & FileLength - LogFileIndex & " new bytes)")
            LogProcessBuffer &= InputString(LogFile, FileLength - LogFileIndex)
            LogFileIndex = FileLength
        Catch ex As Exception
            LogFileOpen = False
            ProcessRecentLogEntries()
            Exit Sub
        End Try

        Dim lBufferLength As Integer = LogProcessBuffer.Length
        Dim lStartLine As Integer = 0
        Dim lEndLine As Integer

        lEndLine = LogProcessBuffer.IndexOfAny(CRLFchars)
        While lEndLine >= 0
            ProcessInput(LogProcessBuffer.Substring(lStartLine, lEndLine - lStartLine))
            lStartLine = lEndLine + 1
            While lStartLine < lBufferLength AndAlso LogProcessBuffer.Substring(lStartLine, 1).IndexOfAny(CRLFchars) = 0
                lStartLine += 1
            End While
            If lStartLine >= lBufferLength Then
                lEndLine = -1
            Else
                lEndLine = LogProcessBuffer.IndexOfAny(CRLFchars, lStartLine + 1)
            End If
        End While
        If lStartLine >= lBufferLength Then 'Processed through end of buffer
            LogProcessBuffer = ""
        Else 'Did not find end of line at end of buffer, save unprocessed part of buffer
            LogProcessBuffer = LogProcessBuffer.Substring(lStartLine)
        End If

    End Sub

    Private Shared Sub ProcessInput(ByVal aInputLine As String)
        LogDisplayAddLine(aInputLine)

        Dim lTimeStamp As String = StrSplit(aInputLine, vbTab, "") 'CreateTimeStamp()
        Dim Words() As String = aInputLine.Split(" ")
        Dim AfterFirstWord As String = aInputLine.Substring(Words(0).Length)

        If Len(Words(0)) > 0 Then
            Select Case Mid(Words(0), 1, 3).ToUpper 'Using Mid since Substring generates error when arg too short
                Case "DBG" 'Debug message, just goes into log
                Case "EXI" : Exiting = True
                Case "LAB" 'Change a label
                    Dim LabelIndex As Integer = -1
                    If IsNumeric(Words(1)) Then
                        LabelIndex = Words(1)
                    Else
                        Select Case Words(1).ToUpper
                            Case "TITLE" : LabelIndex = 0
                            Case "TOP" : LabelIndex = 1
                            Case "LEFT" : LabelIndex = 2
                            Case "MIDDLE" : LabelIndex = 3
                            Case "RIGHT" : LabelIndex = 4
                            Case "BOTTOM" : LabelIndex = 5
                            Case "CLEAR" : ClearLabels()
                        End Select
                    End If
                    If LabelIndex >= 0 And LabelIndex <= frmStatus.LastLabel Then
                        lblText(LabelIndex) = AfterFirstWord.Substring(Words(1).Length)
                    Else 'could not find valid label index, just put it all in top label
                        lblText(1) = AfterFirstWord
                    End If
                Case "PRO"
                    If Words.Length > 3 AndAlso IsNumeric(Words(Words.Length - 3)) AndAlso Words(Words.Length - 2).Equals("of") AndAlso IsNumeric(Words(Words.Length - 1)) Then
                        ProgressCurrent = CInt(Words(Words.Length - 3))
                        ProgressFinal = CInt(Words(Words.Length - 1))
                        'ProgressPercent = lCurrent * 100 / lFinal
                        If ProgressCurrent >= ProgressFinal Then
                            ProgressStartTime = Double.NaN
                            'ProgressPercent = 100
                        ElseIf Double.IsNaN(ProgressStartTime) Then
                            ProgressStartTime = Now.ToOADate
                        End If
                        ProgressNeedsUpdate = True
                    End If
            End Select
        End If
    End Sub

    Private Shared Function CreateTimeStamp() As String
        If LogTimeStamps Then
            With Now
                Return Format(.Hour, "00") & ":" & _
                       Format(.Minute, "00") & ":" & _
                       Format(.Second, "00") & "." & _
                       Format(.Millisecond, "000") & "  "
            End With
        Else
            Return ""
        End If
    End Function

    'Clear our cached versions of the labels on the form
    Private Shared Sub ClearLabels()
        For iLabel As Integer = 0 To frmStatus.LastLabel
            lblText(iLabel) = ""
            lblLast(iLabel) = ""
            lblLogged(iLabel) = ""
        Next
    End Sub

    'Copied from atcUtility
    Private Shared Function StrFirstInt(ByRef Source As String) As Integer
        ' ##SUMMARY Divides alpha numeric sequence into leading numbers and trailing characters.
        ' ##SUMMARY   Example: StrFirstInt("123Go!) = "123", and changes Source to "Go!"
        ' ##PARAM Source M String to be analyzed
        ' ##RETURNS  Returns leading numbers in Source, and returns Source without those numbers.
        Dim retval As Integer = 0
        Dim pos As Integer = 1
        ' ##LOCAL retval - number found at beginning of Source
        ' ##LOCAL pos - long character position in search through Source

        If IsNumeric(Left(Source, 2)) Then pos = 3 'account for negative number - sign
        While IsNumeric(Mid(Source, pos, 1))
            pos += 1
        End While

        If pos >= 2 Then
            retval = CInt(Left(Source, pos - 1))
            Source = LTrim(Mid(Source, pos))
        End If

        Return retval
    End Function

    'Copied from atcUtility
    Private Shared Function StrSplit(ByRef Source As String, ByRef delim As String, ByRef quote As String) As String
        ' ##SUMMARY Divides string into 2 portions at position of 1st occurence of specified _
        'delimeter. Quote specifies a particular string that is exempt from the delimeter search.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "") = "Julie", and "Todd, Jane, and Ray" is returned as Source.
        ' ##SUMMARY   Example: StrSplit("Julie, Todd, Jane, and Ray", ",", "Julie, Todd") = "Julie, Todd", and "Jane, and Ray" is returned as Source.
        ' ##PARAM Source M String to be analyzed
        ' ##PARAM delim I Single-character string delimeter
        ' ##PARAM quote I Multi-character string exempted from search.
        ' ##RETURNS  Returns leading portion of incoming string up to first occurence of delimeter. _
        'Returns input parameter without that portion. If no delimiter in string, _
        'returns whole string, and input parameter reduced to null string.
        Dim retval As String
        Dim i As Integer
        Dim quoted As Boolean
        Dim trimlen As Integer
        Dim quotlen As Integer
        ' ##LOCAL retval - string to return as StrSplit
        ' ##LOCAL i - long character position of search through Source
        ' ##LOCAL quoted - Boolean whether quote was encountered in Source
        ' ##LOCAL trimlen - long length of delimeter, or quote if encountered first
        ' ##LOCAL quotlen - long length of quote

        Source = LTrim(Source) 'remove leading blanks
        quotlen = Len(quote)
        If quotlen > 0 Then
            i = InStr(Source, quote)
            If i = 1 Then 'string beginning
                trimlen = quotlen
                Source = Mid(Source, trimlen + 1)
                i = InStr(Source, quote) 'string end
                quoted = True
            Else
                i = InStr(Source, delim)
                trimlen = Len(delim)
            End If
        Else
            i = InStr(Source, delim)
            trimlen = Len(delim)
        End If

        If i > 0 Then 'found delimeter
            retval = Left(Source, i - 1) 'string to return
            If Right(retval, 1) = " " Then retval = RTrim(retval)
            Source = LTrim(Mid(Source, i + trimlen)) 'string remaining
            If quoted And Len(Source) > 0 Then
                If Left(Source, Len(delim)) = delim Then Source = LTrim(Mid(Source, Len(delim) + 1))
            End If
        Else 'take it all 
            retval = Source
            Source = "" 'nothing left
        End If

        StrSplit = retval

    End Function
End Class
