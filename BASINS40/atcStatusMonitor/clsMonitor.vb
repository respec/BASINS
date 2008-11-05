Imports System.IO

Public Class clsMonitor
    Private Shared pfrmStatus As frmStatus

    Private Shared pLabelText(frmStatus.LastLabel) As String
    Private Shared pLabelLast(frmStatus.LastLabel) As String
    Private Shared pLabelLogged(frmStatus.LastLabel) As String

    Private Const LogDisplayLines As Integer = 250
    Private Shared pLogDisplayBuffer(LogDisplayLines) As String 'Ring buffer of lines of log file to display
    Private Shared pLogDisplayNeedsUpdate As Boolean = False
    Private Shared pLogDisplayFirstLine As Integer = 0
    Private Shared pLogDisplayLastLine As Integer = 0

    Private Shared pProgressNeedsUpdate As Boolean
    Private Shared pProgressStartTime As Double = Double.NaN
    Private Shared pProgressCurrent As Integer
    Private Shared pProgressFinal As Integer

    Private Shared pLastUpdate As Double = Double.NaN
    Private Const UpdateInterval As Double = 2 / 720000.0# ' 1 / 720000.0 = 0.1 second

    Private Shared pIgnoringWindowCommands As Boolean = False
    Private Shared pExiting As Boolean = False

    Private Shared pParentProcess As Process = Nothing

    Public Shared Sub Main()
        Dim lCommandLine As String = Command()
        Console.WriteLine("StatusMonitorEntryWith " & lCommandLine)

        pfrmStatus = New frmStatus
        pfrmStatus.Clear()
        'Console.WriteLine("StatusMonitorFormCreated")

        ClearLabels()

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
            Console.WriteLine("ParentProcess " & pParentProcess.ProcessName)
            pfrmStatus.Text = pParentProcess.ProcessName & "Status Monitor"
        End If

        'ProcessInput("Monitoring Started,Parent = " & lParentID)

        ManageInterface()

        Try
            pfrmStatus.Exiting = True
            pfrmStatus.Close()
        Catch lEx As Exception 'Form might already be closed
        End Try
    End Sub

    Public Shared Sub ManageInterface()
        Dim lNowDouble As Double = Now.ToOADate
        pLastUpdate = lNowDouble - UpdateInterval * 2
        While Not pExiting
            If Console.In.Peek > -1 Then
                Dim lInputLine As String = Console.In.ReadLine
                Console.WriteLine("ManageInterface lInputLine=" & lInputLine)
                ProcessInput(lInputLine)
            End If

            If lNowDouble - pLastUpdate > UpdateInterval Then
                For lLabelIndex As Integer = 0 To frmStatus.LastLabel
                    If Not pLabelText(lLabelIndex).Equals(pLabelLast(lLabelIndex)) Then
                        Console.WriteLine("UpdateLabel " & lLabelIndex)
                        pfrmStatus.Label(lLabelIndex) = pLabelText(lLabelIndex)
                        pLabelLast(lLabelIndex) = pLabelText(lLabelIndex)
                        pLabelLogged(lLabelIndex) = pLabelText(lLabelIndex)
                    End If
                Next
                If pLogDisplayNeedsUpdate Then
                    With pfrmStatus.txtLog
                        .Text = CurrentLogDisplay()
                        .SelectionStart = pfrmStatus.txtLog.Text.Length
                        .ScrollToCaret()
                        pLogDisplayNeedsUpdate = False
                    End With
                End If

                If pProgressNeedsUpdate Then
                    'Console.WriteLine("UpdateProgress")
                    If Double.IsNaN(pProgressStartTime) Then 'Progress is finished
                        pfrmStatus.Visible = False
                        pfrmStatus.Progress.Visible = False
                        For lLabelIndex As Integer = 2 To 5
                            pfrmStatus.Label(lLabelIndex) = pLabelText(lLabelIndex)
                        Next
                    Else
                        If Not pfrmStatus.Progress.Visible Then 'See if we should show it
                            If lNowDouble - pProgressStartTime > UpdateInterval * 3 Then
                                pfrmStatus.Label(2) = "0"
                                pfrmStatus.Label(4) = pProgressFinal
                                pfrmStatus.Progress.Maximum = pProgressFinal
                                pfrmStatus.Progress.Visible = True
                                pfrmStatus.Visible = True
                            End If
                        End If
                        If pfrmStatus.Progress.Visible Then
                            pfrmStatus.Progress.Value = pProgressCurrent
                            If pLabelLogged(2).Length = 0 AndAlso pLabelLogged(3).Length = 0 AndAlso pLabelLogged(4).Length = 0 Then
                                pfrmStatus.Label(3) = pProgressCurrent & " of " & pProgressFinal
                            End If
                            If pLabelLogged(5).Length = 0 Then pfrmStatus.Label(5) = CInt(pProgressCurrent * 1000 / pProgressFinal) / 10 & "%"
                        End If
                    End If
                    pProgressNeedsUpdate = False
                End If

                If Not pParentProcess Is Nothing AndAlso _
                   pParentProcess.HasExited AndAlso _
                   Not pfrmStatus.Exiting Then
                    pExiting = True
                    pfrmStatus.Exiting = True
                    pfrmStatus.Label(0) = "Parent Process Exited"
                    pfrmStatus.Visible = False 'Needs to not be visible to call ShowDialog
                    pfrmStatus.ShowDialog()
                End If
                pLastUpdate = lNowDouble
            Else
                System.Threading.Thread.Sleep(50)
            End If
            Application.DoEvents()
            lNowDouble = Now.ToOADate
            'TODO: Check for form button presses here?
        End While
    End Sub

    Private Shared Function CurrentLogDisplay() As String
        Dim lLogIndex As Integer = pLogDisplayFirstLine
        CurrentLogDisplay = ""
        If lLogIndex > pLogDisplayLastLine Then
            While lLogIndex <= LogDisplayLines
                CurrentLogDisplay &= pLogDisplayBuffer(lLogIndex) & vbCrLf
                lLogIndex += 1
            End While
            lLogIndex = 0
        End If
        While lLogIndex <= pLogDisplayLastLine
            CurrentLogDisplay &= pLogDisplayBuffer(lLogIndex) & vbCrLf
            lLogIndex += 1
        End While
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
        Console.WriteLine("ProcessInput " & aInputLine)
        LogDisplayAddLine(aInputLine)
        Dim lInputLine As String = aInputLine.TrimStart("(").TrimEnd(")")
        Console.WriteLine("ProcessInput " & lInputLine)

        'Dim lTimeStamp As String = StrSplit(aInputLine, vbTab, "") 'CreateTimeStamp()
        Dim lWords() As String = lInputLine.Split(" ")
        Dim lAfterFirstWord As String = lInputLine.Substring(lWords(0).Length)

        If lWords(0).Length > 0 Then
            Select Case lWords(0).ToUpper
                'Case "BUTTON"
                '    Select Case UCase(Rest)
                '        Case "CANCEL" : cmdCancel.Visible = True
                '        Case "PAUSE" : cmdPause.Visible = True
                '        Case "DETAILS", "OUTPUT" : cmdDetails.Visible = True
                '    End Select
                'Case "BUTTOFF"
                '    Select Case UCase(Rest)
                '        Case "CANCEL" : cmdCancel.Visible = False
                '        Case "PAUSE" : cmdPause.Visible = False
                '        Case "DETAILS", "OUTPUT" : cmdDetails.Visible = False
                '    End Select
                Case "CLEAR" : ClearLabels()
                    'Case "DBG" 'Debug message, just goes into log
                    'Console.WriteLine("Debug")
                Case "EXIT" : pExiting = True
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
                        pLabelText(lLabelIndex) = lAfterFirstWord.Substring(lWords(1).Length + 2)
                    Else 'could not find valid label index, just put it all in top label
                        pLabelText(1) = lAfterFirstWord
                    End If
                    Show()
                Case "PROGRESS"
                    'Console.WriteLine("Progress " & aInputLine)
                    If lWords.Length > 3 AndAlso IsNumeric(lWords(lWords.Length - 3)) AndAlso lWords(lWords.Length - 2).Equals("of") AndAlso IsNumeric(lWords(lWords.Length - 1)) Then
                        pProgressCurrent = CInt(lWords(lWords.Length - 3))
                        pProgressFinal = CInt(lWords(lWords.Length - 1))
                        'ProgressPercent = lCurrent * 100 / lFinal
                        If pProgressCurrent >= pProgressFinal Then
                            pProgressStartTime = Double.NaN
                            'ProgressPercent = 100
                            Hide()
                        ElseIf Double.IsNaN(pProgressStartTime) Then
                            pProgressStartTime = Now.ToOADate
                            Show()
                        End If
                        pProgressNeedsUpdate = True
                    End If
                Case "SHOW" : If Not pIgnoringWindowCommands Then Show()
                Case "HIDE" : If Not pIgnoringWindowCommands Then ClearLabels() : Hide()
            End Select
        End If
    End Sub

    Private Shared Sub Show()
        With pfrmStatus
            If Not .Visible Then
                .Visible = True
                .WindowState = FormWindowState.Normal
                .Show()
            End If
        End With
    End Sub

    Private Shared Sub Hide()
        pfrmStatus.Visible = False
    End Sub

    'Clear our cached versions of the labels on the form
    Private Shared Sub ClearLabels()
        For lLabelIndex As Integer = 0 To frmStatus.LastLabel
            pLabelText(lLabelIndex) = ""
            pLabelLast(lLabelIndex) = ""
            pLabelLogged(lLabelIndex) = ""
        Next
    End Sub
End Class
