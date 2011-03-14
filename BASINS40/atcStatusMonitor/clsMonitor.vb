Imports System.IO

Public Class clsMonitor
    Private Shared pfrmStatus As frmStatus

    Delegate Sub FormCallback()
    Private Shared pRedrawCallback As FormCallback
    Private Shared pShowCallback As FormCallback
    'Private Shared pHideCallback As FormCallback

    Delegate Sub FormCallbackInt(ByVal aArg As Integer)
    Private Shared pChangeLevelCallback As FormCallbackInt

    Private Const LogDisplayLines As Integer = 250
    Private Shared pLogDisplayBuffer(LogDisplayLines) As String 'Ring buffer of lines of log file to display
    Private Shared pLogDisplayNeedsUpdate As Boolean = False
    Private Shared pLogDisplayFirstLine As Integer = 0
    Private Shared pLogDisplayLastLine As Integer = 0

    Private Shared pButtonVisibleCancel As Boolean = True
    Private Shared pButtonVisibleDetails As Boolean = True
    Private Shared pButtonVisiblePause As Boolean = True

    Private Shared pProgressOpened As Boolean = False
    Private Shared pProgressStartTime As Double = Double.NaN
    Private Shared pProgressPercentOption As String = ""
    Private Shared pProgressTimeOption As String = ""

    Private Shared pLastUpdate As Double = Double.NaN
    Private Const UpdateInterval As Double = 2 / 720000.0# ' 1 / 720000.0 = 0.1 second
    Private Const UpdateMilliseconds As Integer = 1000

    Private Shared pIgnoringWindowCommands As Boolean = False
    Private Shared pExiting As Boolean = False

    Private Shared pParentProcess As Process = Nothing

    Private Shared pReadInputCallback As AsyncCallback
    Private Shared pInputBufferSize As Integer = 80
    Private Shared pInputBuffer(pInputBufferSize) As Byte
    Private Shared pInputString As String = ""

    Private Shared pLevels As New Generic.List(Of clsLevel)
    Private Shared pLevelsMutex As New Threading.Mutex

    Private Shared WithEvents pWindowTimer As System.Timers.Timer

    Public Shared Sub Main()
        'MsgBox("Pausing Status Monitor, Attach to process " & Process.GetCurrentProcess.Id)
        Application.DoEvents()

        Dim lCommandLine As String = Command()
        'Console.WriteLine("StatusMonitorEntryWith " & lCommandLine)

        pfrmStatus = New frmStatus
        pfrmStatus.Show()
        pfrmStatus.Visible = False

        pLevels.Add(New clsLevel)
        pfrmStatus.Level = 1
        pfrmStatus.Clear()
        pfrmStatus.Label(5) = ""

        'Console.WriteLine("StatusMonitorFormCreated")

        pChangeLevelCallback = New FormCallbackInt(AddressOf ChangeLevelUnsafe)
        pRedrawCallback = New FormCallback(AddressOf RedrawUnsafe)
        pShowCallback = New FormCallback(AddressOf ShowUnsafe)
        'pHideCallback = New FormCallback(AddressOf HideUnsafe)

        Dim lParentID As Integer  'StrFirstInt(lCommandLine)
        If Integer.TryParse(lCommandLine, lParentID) AndAlso lParentID > 0 Then
            Try
                pParentProcess = System.Diagnostics.Process.GetProcessById(lParentID)
            Catch ex As Exception
                ProcessInput("Failed to get Parent (" & lParentID & ") " & ex.Message)
            End Try
        End If
        If pParentProcess Is Nothing Then
            pfrmStatus.Exiting = True 'Let the user quit by closing the form
        Else
            If pParentProcess.ProcessName = "atcStatusMonitorTester" Then
                MsgBox("Pausing Status Monitor, Attach to process " & Process.GetCurrentProcess.Id)
                Application.DoEvents()
            End If
            'Console.WriteLine("ParentProcess " & pParentProcess.ProcessName)
            pfrmStatus.Text = pParentProcess.ProcessName & " Status Monitor"
        End If

        pWindowTimer = New Timers.Timer(200) 'New Threading.Timer(New Threading.TimerCallback(AddressOf RefreshWindow), pWindowTimer, 2000, 2000) 'Threading.Timeout.Infinite)
        pWindowTimer.Start()

        Dim lInput As IO.Stream = Console.OpenStandardInput
        pReadInputCallback = New AsyncCallback(AddressOf InputCallback)
        lInput.BeginRead(pInputBuffer, 0, pInputBufferSize, pReadInputCallback, lInput)

        While Not pExiting OrElse pfrmStatus IsNot Nothing AndAlso pfrmStatus.Visible
            Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        'Console.WriteLine("MonitorExitBeginning")
        Try
            lInput.Close()
            pWindowTimer.Dispose()
        Catch lEx As Exception 'Form might already be closed
        End Try
        If pfrmStatus IsNot Nothing Then
            pfrmStatus.Exiting = True
            pfrmStatus.Close()
            pfrmStatus = Nothing
        End If
        'Console.WriteLine("MonitorExitComplete")
    End Sub

    Public Shared Sub InputCallback(ByVal asyncResult As IAsyncResult)
        Dim lInput As IO.Stream = asyncResult.AsyncState
        Try
            Dim lBytesRead As Integer = lInput.EndRead(asyncResult)

            For lIndex As Integer = 0 To lBytesRead - 1
                Select Case pInputBuffer(lIndex)
                    Case 0 'skip null character
                    Case 10, 13
                        If pInputString.Length > 0 Then
                            ProcessInput(pInputString)
                            pInputString = ""
                        End If
                    Case 41 'Accept close paren ) as end of input line if it started with open paren (
                        If pInputString.StartsWith("(") Then
                            ProcessInput(pInputString.Substring(1))
                            pInputString = ""
                        End If
                    Case Else
                        pInputString &= Chr(pInputBuffer(lIndex))
                End Select
            Next
        Catch e As Exception
            ProcessInput("Exception in InputCallback: " & e.Message)
        End Try
        lInput.BeginRead(pInputBuffer, 0, pInputBufferSize, pReadInputCallback, lInput)
    End Sub

    Private Shared Function InvokeRequired() As Boolean
        Return pfrmStatus IsNot Nothing AndAlso pfrmStatus.lblBottom.InvokeRequired
    End Function

    Private Shared Sub ChangeLevel(ByVal aChange As Integer)
        If pfrmStatus IsNot Nothing Then
            If InvokeRequired() Then
                pfrmStatus.Invoke(pChangeLevelCallback, aChange)
            Else
                ChangeLevelUnsafe(aChange)
            End If
        End If
    End Sub

    Private Shared Sub ChangeLevelUnsafe(ByVal aChange As Integer)
        If aChange <> 0 Then
            pLevelsMutex.WaitOne()
            Dim lHide As Boolean = False
            If aChange > 0 Then
                'Inherit labels for Title=0 and Bottom=5 since those labels are shared by all levels
                Dim lFormerLast As clsLevel = pLevels(pLevels.Count - 1)
                For lNewLevelIndex As Integer = 1 To aChange
                    Dim lNewLevel As New clsLevel
                    lNewLevel.LabelLast(0) = lFormerLast.LabelLast(0)
                    'lNewLevel.LabelLast(5) = lFormerLast.LabelLast(5)
                    lNewLevel.LabelLogged(0) = lFormerLast.LabelLogged(0)
                    'lNewLevel.LabelLogged(5) = lFormerLast.LabelLogged(5)
                    lNewLevel.LabelText(0) = lFormerLast.LabelText(0)
                    'lNewLevel.LabelText(5) = lFormerLast.LabelText(5)
                    pLevels.Add(lNewLevel)
                Next
            Else
                If pLevels.Count + aChange < 1 Then 'Trying to remove only level = hide form
                    aChange = 1 - pLevels.Count
                    lHide = True
                End If
                'Remember last label set for Title=0 and Bottom=5 in case they need to be changed back
                Dim lRemovingLast As clsLevel = pLevels(pLevels.Count - 1)
                Dim lKeepingLast As clsLevel = pLevels(pLevels.Count + aChange - 1)
                lKeepingLast.LabelLast(0) = lRemovingLast.LabelLast(0)
                'lKeepingLast.LabelLast(5) = lRemovingLast.LabelLast(5)
                pLevels.RemoveRange(pLevels.Count + aChange, -aChange)
                lKeepingLast.LabelNeedsUpdate = True
            End If
            pfrmStatus.Level = pLevels.Count
            pLevelsMutex.ReleaseMutex()
            If lHide Then HideUnsafe()
        End If
    End Sub

    Private Shared Sub Redraw()
        If pfrmStatus IsNot Nothing Then
            If InvokeRequired() Then
                pfrmStatus.Invoke(pRedrawCallback)
            Else
                RedrawUnsafe()
            End If
        End If
    End Sub

    Private Shared Sub RedrawUnsafe()
        If pfrmStatus IsNot Nothing Then
            pLevelsMutex.WaitOne()
            Try
                With pfrmStatus
                    .btnCancel.Visible = pButtonVisibleCancel
                    .btnLog.Visible = pButtonVisibleDetails
                    .btnPause.Visible = pButtonVisiblePause
                    Dim lLevelIndex As Integer = 0
                    For Each lLevel As clsLevel In pLevels
                        lLevelIndex += 1
                        If lLevel.LabelNeedsUpdate Then
                            lLevel.LabelNeedsUpdate = False
                            Dim lAnyLabel As Boolean = False 'Becomes true if any label has content
                            For lLabelIndex As Integer = 0 To frmStatus.LastLabel
                                If Not lLevel.LabelText(lLabelIndex).Equals(lLevel.LabelLast(lLabelIndex)) Then
                                    'Console.WriteLine("UpdateLabel " & lLabelIndex)
                                    .Label(lLabelIndex, lLevelIndex) = lLevel.LabelText(lLabelIndex)
                                    lLevel.LabelLast(lLabelIndex) = lLevel.LabelText(lLabelIndex)
                                    lLevel.LabelLogged(lLabelIndex) = lLevel.LabelText(lLabelIndex)
                                End If
                                If Not lAnyLabel AndAlso lLabelIndex > 0 AndAlso lLevel.LabelText(lLabelIndex).Length > 0 Then lAnyLabel = True
                            Next
                            If lAnyLabel Then 'Make form visible if a label has something to say
                                .Visible = True
                            ElseIf lLevelIndex = 1 AndAlso lLevel.ProgressCurrent >= lLevel.ProgressFinal Then
                                .Visible = False 'Hide form if all labels at top level are blank
                            End If
                        End If
                        If pLogDisplayNeedsUpdate Then
                            pLogDisplayNeedsUpdate = False
                            .txtDetails.Text = CurrentLogDisplay()
                            .txtDetails.SelectionStart = .txtDetails.Text.Length
                            .txtDetails.ScrollToCaret()
                        End If

                        If lLevel.ProgressNeedsUpdate Then
                            lLevel.ProgressNeedsUpdate = False
                            Dim lProgress As Windows.Forms.ProgressBar = .Progress(lLevelIndex)

                            If lLevel.ProgressCurrent >= lLevel.ProgressFinal Then 'Progress is finished
                                lProgress.Visible = False
                                If lLevelIndex = 1 Then
                                    .Visible = False
                                    pProgressStartTime = Double.NaN
                                End If
                                pProgressOpened = False
                                'Refresh labels that may have been set to show progress
                                For lLabelIndex As Integer = 2 To frmStatus.LastLabel
                                    .Label(lLabelIndex, lLevelIndex) = lLevel.LabelText(lLabelIndex)
                                Next
                            Else
                                If lLevelIndex = 1 AndAlso Double.IsNaN(pProgressStartTime) Then
                                    pProgressStartTime = Now.ToOADate
                                End If

                                lProgress.Maximum = lLevel.ProgressFinal
                                lProgress.Value = lLevel.ProgressCurrent
                                If lLevel.ProgressCurrent > 0 Then
                                    If Not pProgressOpened Then
                                        ShowUnsafe()
                                        pProgressOpened = True
                                    End If
                                    lProgress.Visible = True
                                End If
                                If lProgress.Visible Then
                                    lProgress.Refresh()
                                End If

                                'If pLabelLogged(2).Length = 0 AndAlso pLabelLogged(4).Length = 0 Then
                                '    .Label(2,lLevelIndex) = "0"
                                '    .Label(4,lLevelIndex) = Format(pProgressFinal, "#,###")
                                'End If

                                If lLevel.LabelLogged(3).Length = 0 Then
                                    If lLevel.ProgressCurrent = 0 Then
                                        .Label(3, lLevelIndex) = ""
                                    ElseIf lLevel.ProgressFinal = 100 Then
                                        .Label(3, lLevelIndex) = lLevel.ProgressCurrent & "%"
                                    Else
                                        .Label(3, lLevelIndex) = Format(lLevel.ProgressCurrent, "#,##0") & " of " & Format(lLevel.ProgressFinal, "#,###")
                                    End If
                                End If

                                If lLevelIndex = 1 AndAlso _
                                       (pProgressPercentOption.Length > 0 OrElse pProgressTimeOption.Length > 0) Then
                                    Dim lEstimateLabel As String = ""
                                    Dim lElapsedDays As Double = Date.Now.ToOADate - pProgressStartTime
                                    'If elapsed time is short, skip estimating time remaining but still give progress percent if requested
                                    If lElapsedDays * 86400 < 10 Then '86400 seconds per day
                                        If pProgressPercentOption.Length > 0 Then
                                            lEstimateLabel = CInt(lLevel.ProgressCurrent * 100 / lLevel.ProgressFinal) & pProgressPercentOption
                                        End If
                                    Else
                                        Dim lEstimateTotalTime As Double = lElapsedDays * lLevel.ProgressFinal / lLevel.ProgressCurrent
                                        Dim lEstimateLeft As Double = (lEstimateTotalTime - lElapsedDays)
                                        If lEstimateLeft * 1440 < 1 Then 'Less than one minute
                                            If pProgressPercentOption.Length > 0 Then
                                                lEstimateLabel = CInt(lLevel.ProgressCurrent * 100 / lLevel.ProgressFinal) & pProgressPercentOption
                                            End If
                                            If pProgressTimeOption.Length > 0 Then
                                                lEstimateLabel &= " (" & Format(lEstimateLeft * 86400, "0") & " seconds remaining)"
                                            End If
                                        ElseIf lEstimateLeft * 24 < 1 Then 'Less than one hour
                                            Dim lMinutes As Integer = Math.Floor(lEstimateLeft * 1440)
                                            If pProgressPercentOption.Length > 0 Then
                                                lEstimateLabel = CInt(lLevel.ProgressCurrent * 1000 / lLevel.ProgressFinal) / 10 & "%"
                                            End If
                                            If pProgressTimeOption.Length > 0 Then
                                                lEstimateLabel &= " (" & lMinutes & ":" & Format((lEstimateLeft - lMinutes / 1440.0) * 86400, "00") & " remaining)"
                                            End If
                                        ElseIf lEstimateLeft < 1 Then
                                            Dim lEstimate As Date = Date.FromOADate(pProgressStartTime + lEstimateTotalTime)
                                            If pProgressPercentOption.Length > 0 Then
                                                lEstimateLabel = CInt(lLevel.ProgressCurrent * 10000 / lLevel.ProgressFinal) / 100 & pProgressPercentOption
                                            End If
                                            If pProgressTimeOption.Length > 0 Then
                                                lEstimateLabel &= " (complete at " & lEstimate.ToShortTimeString & ")"
                                            End If
                                        Else
                                            Dim lEstimate As Date = Date.FromOADate(pProgressStartTime + lEstimateTotalTime)
                                            If pProgressPercentOption.Length > 0 Then
                                                lEstimateLabel = CInt(lLevel.ProgressCurrent * 100000 / lLevel.ProgressFinal) / 1000 & pProgressPercentOption
                                            End If
                                            If pProgressTimeOption.Length > 0 Then
                                                lEstimateLabel &= " (complete on " & lEstimate.ToShortDateString & " at " & lEstimate.ToShortTimeString & ")"
                                            End If
                                        End If
                                    End If
                                    .Label(5) = lEstimateLabel
                                End If
                                'End If
                            End If
                        End If
                        If pParentProcess IsNot Nothing AndAlso _
                           pParentProcess.HasExited AndAlso _
                           Not .Exiting Then
                            .Exiting = True
                            .Close()
                            End
                            'Open form with a message indicating that parent exited
                            '.WindowState = FormWindowState.Normal
                            'pExiting = True
                            '.Exiting = True
                            '.Level = 1
                            '.Label(0) = pParentProcess.ProcessName & " Exited"
                            'For lLabelIndex As Integer = 1 To 5
                            '    .Label(lLabelIndex) = ""
                            'Next
                            '.btnCancel.Visible = False
                            '.btnPause.Visible = False
                            '.btnLog.Visible = True
                            'ShowUnsafe()
                        Else
                            'pWindowTimer.Change(UpdateMilliseconds, Threading.Timeout.Infinite)
                        End If
                    Next
                End With
            Catch e As Exception
                'MsgBox(e.Message, MsgBoxStyle.Critical, "Exception in RefreshWindow")
            End Try
            pLevelsMutex.ReleaseMutex()
            'pfrmStatus.Refresh()
        End If
        Application.DoEvents()
    End Sub

    Private Shared Function CurrentLogDisplay() As String
        Dim lLogIndex As Integer = pLogDisplayFirstLine
        Dim lSB As New System.Text.StringBuilder
        If lLogIndex > pLogDisplayLastLine Then
            While lLogIndex <= LogDisplayLines
                lSB.AppendLine(pLogDisplayBuffer(lLogIndex))
                lLogIndex += 1
            End While
            lLogIndex = 0
        End If
        While lLogIndex <= pLogDisplayLastLine
            lSB.AppendLine(pLogDisplayBuffer(lLogIndex))
            lLogIndex += 1
        End While
        Return lSB.ToString
    End Function

    Private Shared Sub LogDisplayAddLine(ByVal aNewLine As String)
        pLogDisplayLastLine += 1
        If pLogDisplayLastLine > LogDisplayLines Then pLogDisplayLastLine = 0
        If pLogDisplayLastLine = pLogDisplayFirstLine Then
            pLogDisplayFirstLine += 1
            If pLogDisplayFirstLine > LogDisplayLines Then pLogDisplayFirstLine = 0
        End If
        pLogDisplayBuffer(pLogDisplayLastLine) = aNewLine
        pLogDisplayNeedsUpdate = True
    End Sub

    Private Shared Sub ProcessInput(ByVal aInputLine As String)
        'Console.WriteLine("ProcessInput " & aInputLine)
        LogDisplayAddLine(aInputLine)
        Dim lInputLine As String = aInputLine.TrimStart("(").TrimEnd(")")
        'Console.WriteLine("ProcessInput " & lInputLine)

        'Dim lTimeStamp As String = StrSplit(aInputLine, vbTab, "") 'CreateTimeStamp()
        If lInputLine.StartsWith("MSG") Then
            lInputLine = "MSG " & lInputLine.Substring(3)
        End If
        Dim lWords() As String = lInputLine.Split(" ")
        Dim lAfterFirstWord As String = lInputLine.Substring(lWords(0).Length)

        If lWords(0).Length > 0 Then
            If lWords(0).StartsWith("MSG") Then
                Dim lMsgID As Integer = CInt(lWords(1))
                Select Case lMsgID
                    Case 99 : lWords(0) = "HIDE"
                    Case 0 'don't do anything
                    Case 1, 2, 3, 4 : lWords(0) = "LABEL"
                    Case 5 : lWords(0) = "PROGRESS"
                    Case 6 : lWords(0) = "LABEL" : lWords(1) = 5
                    Case 7 : lWords(0) = "DBG"
                    Case 10 : lWords(0) = "OPEN"
                End Select
            End If

            pLevelsMutex.WaitOne()
            Dim lLevel As clsLevel = pLevels(pLevels.Count - 1)
            pLevelsMutex.ReleaseMutex()

            Select Case lWords(0).ToUpper
                Case "BUTTON"
                    Select Case lWords(1).ToUpper
                        Case "CANCEL" : pButtonVisibleCancel = True
                        Case "PAUSE" : pButtonVisiblePause = True
                        Case "DETAILS", "OUTPUT" : pButtonVisibleDetails = True
                    End Select
                Case "BUTTOFF"
                    Select Case lWords(1).ToUpper
                        Case "CANCEL" : pButtonVisibleCancel = False
                        Case "PAUSE" : pButtonVisiblePause = False
                        Case "DETAILS", "OUTPUT" : pButtonVisibleDetails = False
                    End Select
                Case "CLEAR" : lLevel.ClearLabels()
                Case "DBG" 'Debug message, just goes into log
                Case "EXIT"
                    pExiting = True
                    'Console.WriteLine("pExitingNowTrue")
                    If pfrmStatus IsNot Nothing Then
                        pfrmStatus.Exiting = True
                        pfrmStatus.Close()
                        pfrmStatus = Nothing
                    End If
                Case "FOLLOWWINDOWCOMMANDS" : pIgnoringWindowCommands = False
                Case "IGNOREWINDOWCOMMANDS" : pIgnoringWindowCommands = True
                Case "LABEL" 'Change a label
                    'Console.WriteLine("ChangeLabel " & lWords(1) & " to " & lAfterFirstWord.Substring(lWords(1).Length + 2))
                    Dim lLabelIndex As Integer = -1
                    If IsNumeric(lWords(1)) Then
                        lLabelIndex = CInt(lWords(1))
                    Else
                        Select Case lWords(1).ToUpper
                            Case "TITLE" : lLabelIndex = 0
                            Case "TOP" : lLabelIndex = 1
                            Case "LEFT" : lLabelIndex = 2
                            Case "MIDDLE" : lLabelIndex = 3
                            Case "RIGHT" : lLabelIndex = 4
                            Case "BOTTOM" : lLabelIndex = 5
                        End Select
                    End If
                    'Console.WriteLine("LabelIndex " & lLabelIndex)
                    If lLabelIndex >= 0 And lLabelIndex <= frmStatus.LastLabel Then
                        If lAfterFirstWord.Length > lWords(1).Length + 2 Then
                            lLevel.LabelText(lLabelIndex) = lAfterFirstWord.Substring(lWords(1).Length + 2)
                        Else
                            lLevel.LabelText(lLabelIndex) = ""
                        End If
                    Else 'could not find valid label index, just put it all in top label
                        lLevel.LabelText(1) = lAfterFirstWord.Trim
                    End If
                    lLevel.LabelNeedsUpdate = True
                Case "PROGRESSLEVEL"
                    Select Case lWords(1).ToUpper
                        Case "+", "PUSH" : ChangeLevel(1) 'Starting a deeper level of progress
                        Case "-", "POP" : ChangeLevel(-1) 'Finished a deeper level of progress
                        Case "-+", "POP-INCREMENT" 'Finish a deeper level of progress and increment progress above
                            ChangeLevel(-1)
                            pLevelsMutex.WaitOne()
                            lLevel = pLevels(pLevels.Count - 1)
                            lLevel.ProgressCurrent += 1
                            lLevel.ProgressNeedsUpdate = True
                            pLevelsMutex.ReleaseMutex()
                        Case "-++", "POP-INCREMENT-PUSH" 'Finish a deeper level of progress, increment progress above, start new deeper level
                            lLevel.ProgressCurrent = 0
                            lLevel.ProgressFinal = 0
                            lLevel.ProgressNeedsUpdate = True
                            pLevelsMutex.WaitOne()
                            If pLevels.Count > 1 Then
                                Dim lLevelUp As clsLevel = pLevels(pLevels.Count - 2)
                                lLevelUp.ProgressCurrent += 1
                                lLevelUp.ProgressNeedsUpdate = True
                            End If
                            pLevelsMutex.ReleaseMutex()
                    End Select

                Case "PROGRESS"
                    Select Case lWords(1).ToUpper
                        Case "PERCENT"
                            If lWords.Length > 2 Then
                                pProgressPercentOption = lWords(2)
                            Else
                                pProgressPercentOption = " %"
                            End If
                            If pProgressPercentOption.ToUpper = "OFF" Then
                                pProgressPercentOption = ""
                            Else
                                pProgressPercentOption = pProgressPercentOption.Replace("_", " ")
                            End If
                        Case "TIME"
                            If lWords.Length > 2 Then
                                pProgressTimeOption = lWords(2)
                            Else
                                pProgressTimeOption = "ON"
                            End If
                            If pProgressTimeOption.ToUpper = "OFF" Then
                                pProgressTimeOption = ""
                            Else
                                pProgressTimeOption = pProgressTimeOption.Replace("_", " ")
                            End If
                    End Select
                    If lWords.Length = 2 AndAlso IsNumeric(lWords(1)) Then
                        'Interpret legacy "PROGRESS 10" like new "PROGRESS 10 of 100"
                        lLevel.ProgressCurrent = CInt(lWords(1))
                        'If pProgressCurrent >= 0 AndAlso pProgressCurrent <= 100 Then
                        lLevel.ProgressFinal = 100
                        lLevel.ProgressNeedsUpdate = True
                        'End If
                    End If
                    If lWords.Length > 3 AndAlso IsNumeric(lWords(lWords.Length - 3)) AndAlso lWords(lWords.Length - 2).Equals("of") AndAlso IsNumeric(lWords(lWords.Length - 1)) Then
                        lLevel.ProgressCurrent = CInt(lWords(lWords.Length - 3))
                        lLevel.ProgressFinal = CInt(lWords(lWords.Length - 1))
                        lLevel.ProgressNeedsUpdate = True
                    End If
                Case "SHOW"
                    If Not pIgnoringWindowCommands AndAlso pfrmStatus IsNot Nothing Then
                        If lAfterFirstWord.Length > 0 Then
                            lLevel.LabelText(0) = lAfterFirstWord
                            lLevel.LabelNeedsUpdate = True
                        End If
                        If pfrmStatus IsNot Nothing Then
                            If InvokeRequired() Then pfrmStatus.Invoke(pShowCallback) Else ShowUnsafe()
                        End If
                    End If
                Case "HIDE"
                    If Not pIgnoringWindowCommands AndAlso pfrmStatus IsNot Nothing Then
                        If pLevels.Count = 1 Then lLevel.ClearLabels()
                        'ChangeLevel(-1)
                        'If pfrmStatus IsNot Nothing Then
                        '    lLevel.ClearLabels()
                        '    If InvokeRequired() Then pfrmStatus.Invoke(pHideCallback) Else HideUnsafe()
                        'End If
                    End If
                Case Else
                    lLevel.LabelText(1) = aInputLine
                    lLevel.LabelNeedsUpdate = True
            End Select
        End If
        System.Threading.Thread.Sleep(0)
    End Sub

    Private Shared Sub ShowUnsafe()
        If pfrmStatus IsNot Nothing Then
            With pfrmStatus
                Select Case .WindowState
                    Case FormWindowState.Normal
                        .Visible = True
                        .Show()
                End Select
            End With
        End If
    End Sub

    Private Shared Sub HideUnsafe()
        If pfrmStatus IsNot Nothing Then
            pfrmStatus.Clear()
            pfrmStatus.Visible = False
            pProgressOpened = False
        End If
    End Sub

    Private Shared Sub pWindowTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles pWindowTimer.Elapsed
        pWindowTimer.Enabled = False
        If Not pExiting Then
            Redraw()
            pWindowTimer.Enabled = True
        End If
    End Sub

    Public Class clsLevel
        Public LabelNeedsUpdate As Boolean = False
        Public LabelText() As String
        Public LabelLast() As String
        Public LabelLogged() As String
        Public ProgressNeedsUpdate As Boolean = False
        Public ProgressCurrent As Integer
        Public ProgressFinal As Integer

        Public Sub New()
            ClearLabels()
        End Sub

        'Clear our cached versions of the labels on the form
        Public Sub ClearLabels()
            If pfrmStatus IsNot Nothing Then
                If LabelLast Is Nothing Then
                    ReDim LabelLast(frmStatus.LastLabel)
                    ReDim LabelText(frmStatus.LastLabel)
                    ReDim LabelLogged(frmStatus.LastLabel)
                End If
                For lLabelIndex As Integer = 1 To frmStatus.LastLabel
                    LabelText(lLabelIndex) = ""
                    LabelLogged(lLabelIndex) = ""
                Next
                LabelNeedsUpdate = True
            End If
        End Sub
    End Class

End Class
