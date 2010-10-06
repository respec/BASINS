''' <summary>
''' Interface for handling status and progress updates
''' </summary>
Public Interface IProgressStatus
    ''' <summary>
    ''' Log the progress of a long-running task
    ''' </summary>
    ''' <param name="aCurrentPosition">Current position/item of task</param>
    ''' <param name="aLastPosition">Final position/item of task</param>
    ''' <remarks>
    ''' A final call when the task is done with aCurrent = aLast 
    ''' indicates completion and should clear the progress display.
    ''' </remarks>
    Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer)

    ''' <summary>
    ''' Update the current status message
    ''' </summary>
    ''' <param name="aStatusMessage">Description of current processing status</param>
    ''' <remarks></remarks>
    Sub Status(ByVal aStatusMessage As String)

End Interface

''' <summary>
''' Interface that can also be implemented when implementing IProgressStatus if also providing the option to cancel a long-running action
''' </summary>
''' <remarks></remarks>
Public Interface IProgressStatusCancel
    ''' <summary>
    ''' True if user has requested to abort the activity monitored by Progress
    ''' </summary>
    ''' <remarks>Automatically resets to False after returning True</remarks>
    Property Canceled() As Boolean
End Interface

''' <summary>
''' Default implementation does nothing
''' </summary>
Public Class NullProgressStatus
    Implements IProgressStatus

    Public Sub Progress(ByVal aCurrentPosition As Integer, _
                        ByVal aLastPosition As Integer) Implements IProgressStatus.Progress
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements IProgressStatus.Status
    End Sub
End Class

Public Class ProgressCancelException
    Inherits ApplicationException
    Public Overrides ReadOnly Property Message() As String
        Get
            Return "User Canceled"
        End Get
    End Property
End Class

Public Class ProgressLevel
    Implements IDisposable

    Private pIncrementAfter As Boolean = False
    Private pStaySameLevel As Boolean = False
    Private pDisposed As Boolean = False

    ''' <summary>
    ''' Create a new level in the stack of logger progress levels
    ''' </summary>
    ''' <param name="aIncrementAfter">True to increment progress by one at level above new level after finishing, Default = False</param>
    ''' <param name="aStaySameLevel">True to keep new level at the same level we are already on rather than adding to the stack</param>
    ''' <remarks>Recommendation: Using myLevel as new ProgressLevel()...[call other code that contains progress]...End Using</remarks>
    Public Sub New(Optional ByVal aIncrementAfter As Boolean = False, Optional ByVal aStaySameLevel As Boolean = False)
        pIncrementAfter = aIncrementAfter
        pStaySameLevel = aStaySameLevel
        Logger.Dbg("NewProgressLevel " & pIncrementAfter & ":" & pStaySameLevel)
        If Not pStaySameLevel Then
            Logger.Status("PROGRESSLEVEL +")
        End If
    End Sub

    ''' <summary>
    ''' Reset the progress without destroying this level and creating a new one
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reset()
        Logger.Dbg("ResetProgressLevel " & pIncrementAfter & ":" & pStaySameLevel)
        If pIncrementAfter AndAlso Not pStaySameLevel Then
            Logger.Status("PROGRESSLEVEL -++")
        Else
            Logger.Progress(0, 0)
        End If
    End Sub

    ''' <summary>
    ''' Ensure the stack of logger progress levels is consistent after we are finished with this level
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Logger.Dbg("DisposeProgressLevel " & pIncrementAfter & ":" & pStaySameLevel)
        If Not pDisposed Then
            pDisposed = True
            If pStaySameLevel Then
                Logger.Progress(0, 0)
            ElseIf pIncrementAfter Then
                Logger.Status("PROGRESSLEVEL -+")
            Else
                Logger.Status("PROGRESSLEVEL -")
            End If
            GC.SuppressFinalize(Me)
        End If
    End Sub

End Class

''' <summary>
''' Use StatusMonitor.exe to display Status and Progress and allow user to cancel long-running actions
''' </summary>
Public Class MonitorProgressStatus
    Implements IProgressStatus, IProgressStatusCancel, System.IDisposable

    Private pMonitorProcess As System.Diagnostics.Process
    Private pCanceled As Boolean = False
    Private pPaused As Boolean = False
    Private pProgressLevel As Integer = 1

    ''' <summary>
    ''' Another IProgressStatus which will also get a copy of Status and Progress messages
    ''' </summary>
    ''' <remarks>
    ''' This lets us send Progress and Status to MapWindow's status bar while also sending to StatusMonitor.exe
    ''' </remarks>
    <CLSCompliant(False)> _
    Public InnerProgressStatus As IProgressStatus = Nothing

    Public Sub Progress(ByVal aCurrentPosition As Integer, _
                        ByVal aLastPosition As Integer) Implements IProgressStatus.Progress
        While pPaused
            Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(50)
        End While
        If InnerProgressStatus IsNot Nothing Then InnerProgressStatus.Progress(aCurrentPosition, aLastPosition)
        WriteStatus("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements IProgressStatus.Status
        'Some messages intended for StatusMonitor.exe should not also be sent to default status
        Dim lUpcaseMessage As String = aStatusMessage.ToUpper
        Select Case lUpcaseMessage
            Case "HIDE", "SHOW", "CLEAR", "DBG", "EXIT", "FOLLOWWINDOWCOMMANDS", "IGNOREWINDOWCOMMANDS", "PROGRESSLEVEL -++"
            Case "PROGRESSLEVEL +"
                pProgressLevel += 1
            Case "PROGRESSLEVEL -", "PROGRESSLEVEL -+"
                pProgressLevel -= 1
            Case Else
                If InnerProgressStatus IsNot Nothing AndAlso Not (TypeOf InnerProgressStatus Is NullProgressStatus) Then 'Send cleaned-up message to inner progress/status
                    If lUpcaseMessage.StartsWith("BUTTO") Then
                    Else
                        Dim lWords() As String = lUpcaseMessage.Split(" ")
                        If lWords.Length > 2 AndAlso lWords(0) = "LABEL" Then 'Skip text describing which label
                            InnerProgressStatus.Status(aStatusMessage.Substring(lWords(0).Length + lWords(1).Length + 2))
                        Else
                            InnerProgressStatus.Status(aStatusMessage)
                        End If
                    End If
                End If
        End Select
        WriteStatus(aStatusMessage)
    End Sub

    Private Function WriteStatus(ByVal aStatusMessage As String) As Boolean
        If pMonitorProcess Is Nothing OrElse pMonitorProcess.HasExited Then
            Return False  'Process at other end of pipe is dead, stop talking to it
        End If

        If aStatusMessage Is Nothing OrElse aStatusMessage.Length = 0 Then
            aStatusMessage = "HIDE"
        ElseIf aStatusMessage.StartsWith("(") AndAlso aStatusMessage.EndsWith(")") Then
            aStatusMessage = aStatusMessage.Substring(1, aStatusMessage.Length - 2)
        End If

        'Escape open and close parens
        aStatusMessage = aStatusMessage.Replace("(", Chr(6)).Replace(")", Chr(7))

        If Asc(Right(aStatusMessage, 1)) > 31 Then
            aStatusMessage = "(" & aStatusMessage & ")"
        End If

        If aStatusMessage.Length > 0 Then
            Logger.Dbg(aStatusMessage)
            pMonitorProcess.StandardInput.WriteLine(aStatusMessage)
            pMonitorProcess.StandardInput.Flush()
        End If
        Return True
    End Function

    ''' <summary>
    ''' True if user has requested to cancel current operation
    ''' Automatically cleared after level 1 progress hears that it is True
    ''' </summary>
    Public Property Canceled() As Boolean Implements IProgressStatusCancel.Canceled
        Get
            If pCanceled Then
                'Clear the cancel flag if we are now telling lowest progress level to cancel
                If pProgressLevel = 1 Then pCanceled = False
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal aSetCanceled As Boolean)
            pCanceled = aSetCanceled
        End Set
    End Property

    ''' <summary>
    ''' True while user has requested a pause in execution
    ''' </summary>
    ''' <value></value>
    ''' <returns>True when paused, False when not paused</returns>
    ''' <remarks>
    ''' A button in StatusMonitor.exe labeled "Pause" 
    ''' sends a message "P" through MonitorMessageHandler.
    ''' The button then changes label to "Run" and if pressed 
    ''' sends a message "R", which changes Paused back to False.
    ''' Pausing is enforced by the While Paused loop in Sub Progress
    ''' </remarks>
    Property Paused() As Boolean
        Get
            Return pPaused
        End Get
        Set(ByVal aPaused As Boolean)
            pPaused = aPaused
        End Set
    End Property

    ''' <summary>
    ''' Start status monitor as a separate executable
    ''' </summary>
    ''' <param name="aMonitorFileName">Full path to status monitor program</param>
    ''' <param name="aWorkingDirectory">Directory to start running in, probably no need to specify this</param>
    ''' <param name="aArguments">Arguments to status monitor. If not specified, current process ID is used as only argument</param>
    ''' <returns>True if started or decided to use already running monitor, False if failed to start</returns>
    ''' <remarks>pMonitorProcess is set if monitor is successfully started</remarks>
    Public Function StartMonitor(ByVal aMonitorFileName As String, _
                         Optional ByVal aWorkingDirectory As String = Nothing, _
                         Optional ByVal aArguments As String = Nothing) As Boolean
        Logger.Dbg("Start " & aMonitorFileName & " in " & aWorkingDirectory)
        Try
            If Not IO.File.Exists(aMonitorFileName) Then
                Logger.Dbg("StartMonitor: File not found: '" & aMonitorFileName & "'")
            Else
                If pMonitorProcess Is Nothing Then
                    pMonitorProcess = New Process
                    With pMonitorProcess.StartInfo
                        .FileName = aMonitorFileName '"S:\dev\BASINS40\Bin\StatusMonitor.exe" 'atcUtility.FindFile("Status Monitor", "statusMonitor.exe")
                        If Not String.IsNullOrEmpty(aWorkingDirectory) Then .WorkingDirectory = aWorkingDirectory
                        If String.IsNullOrEmpty(aArguments) Then
                            .Arguments = Process.GetCurrentProcess.Id
                        Else
                            .Arguments = aArguments
                        End If
                        .CreateNoWindow = True
                        .UseShellExecute = False
                        .RedirectStandardInput = True
                        .RedirectStandardOutput = True
                        AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorMessageHandler
                        .RedirectStandardError = True
                        AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
                    End With
                    pMonitorProcess.Start()
                    '
                    'NOTE: to debug pMonitorProcess, need Visual Studio (not Express) - choose Tools:AttachToProcess - StatusMonitor
                    '
                    pMonitorProcess.BeginErrorReadLine()
                    pMonitorProcess.BeginOutputReadLine()
                    Logger.Dbg("MonitorStarted")
                Else
                    Logger.Dbg("UsingExistingMonitor")
                End If
                Return True
            End If
        Catch ex As Exception
            pMonitorProcess = Nothing
            Logger.Msg("StatusProcessStartError:" & ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Stop StatusMonitor which was started by StartMonitor
    ''' </summary>
    Public Sub StopMonitor() Implements IDisposable.Dispose
        Try
            If pMonitorProcess Is Nothing Then
                Logger.Dbg("NoMonitorToStop")
            ElseIf pMonitorProcess.HasExited Then
                Logger.Dbg("MonitorHasExited")
                pMonitorProcess = Nothing
            Else
                pMonitorProcess.StandardInput.WriteLine("Exit")
                Windows.Forms.Application.DoEvents()
                Threading.Thread.Sleep(100)
                If Not pMonitorProcess.HasExited Then pMonitorProcess.Kill()
                Logger.Dbg("MonitorStopped")
                pMonitorProcess = Nothing
            End If
        Catch e As Exception
            Logger.Dbg("StopMonitor: Exception: " & e.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Messages from standard output of StatusMonitor.exe are handled
    ''' </summary>
    ''' <param name="aSendingProcess"></param>
    ''' <param name="aOutLine"></param>
    ''' <remarks>
    ''' All messages are logged, "P" pauses, "R" resumes, "C" cancels
    ''' </remarks>
    Private Sub MonitorMessageHandler(ByVal aSendingProcess As Object, _
                                      ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Dim lMessage As String = aOutLine.Data.ToString
            Logger.Dbg("MsgFromStatusMonitor " & lMessage)
            Select Case lMessage
                Case "P" : Paused = True
                Case "R" : Paused = False
                Case "C" : Paused = False : Canceled = True
            End Select
        End If
    End Sub
End Class