Public Module modLaunch

    Private pMonitorProcess As System.Diagnostics.Process

    Public Function LaunchProgram(ByVal aExeName As String, _
                                  ByVal aWorkingDirectory As String, _
                         Optional ByVal aArguments As String = "", _
                         Optional ByVal aWait As Boolean = True) As Integer
        Logger.Dbg("LaunchProgram " & aExeName & " in " & aWorkingDirectory)
        Dim lExitCode As Integer = 0
        Try
            Dim lProcess As New System.Diagnostics.Process
            With lProcess.StartInfo
                .FileName = aExeName
                .WorkingDirectory = aWorkingDirectory
                .CreateNoWindow = True
                .UseShellExecute = False
                .Arguments = aArguments
                .RedirectStandardOutput = True
                AddHandler lProcess.OutputDataReceived, AddressOf MessageHandler
                .RedirectStandardError = True
                AddHandler lProcess.ErrorDataReceived, AddressOf MessageHandler
            End With
            lProcess.Start()
            lProcess.BeginErrorReadLine()
            lProcess.BeginOutputReadLine()
            If aWait Then
KeepWaiting:
                Try
                    lProcess.WaitForExit()
                    lExitCode = lProcess.ExitCode
                Catch lWaitError As Exception
                    Logger.Dbg(lWaitError.Message)
                End Try
                If Not lProcess.HasExited Then GoTo KeepWaiting
                Logger.Dbg("LaunchProgram: " & aExeName & ": Exit code " & lExitCode)
            End If
        Catch lEx As ApplicationException
            Logger.Dbg("LaunchProgram: " & aExeName & ": Exception: " & lEx.Message)
            lExitCode = -1
        End Try
        Return lExitCode
    End Function

    Private Sub MessageHandler(ByVal aSendingProcess As Object, _
                               ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Logger.Dbg(aOutLine.Data.ToString)
            Logger.Flush()
        End If
    End Sub

    Public Function LaunchMonitor(ByVal aMonitorName As String, _
                             ByVal aWorkingDirectory As String, _
                    Optional ByVal aArguments As String = "") As Boolean
        Logger.Dbg("Start " & aMonitorName & " in " & aWorkingDirectory)
        Try
            If Not IO.File.Exists(aMonitorName) Then
                Logger.Dbg("Monitor not found to launch at '" & aMonitorName & "'")
            Else
                If pMonitorProcess Is Nothing Then
                    pMonitorProcess = New System.Diagnostics.Process
                    With pMonitorProcess.StartInfo
                        .FileName = aMonitorName
                        .WorkingDirectory = aWorkingDirectory
                        .CreateNoWindow = True
                        .UseShellExecute = False
                        .Arguments = aArguments
                        .RedirectStandardInput = True
                        .RedirectStandardOutput = True
                        AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorMessageHandler
                        .RedirectStandardError = True
                        AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
                    End With
                    pMonitorProcess.Start()
                    pMonitorProcess.BeginErrorReadLine()
                    pMonitorProcess.BeginOutputReadLine()
                    Logger.Dbg("MonitorLaunched")
                Else
                    Logger.Dbg("UsingExistingMonitor")
                End If
                Return True
            End If
        Catch lEx As ApplicationException
            pMonitorProcess = Nothing
            Logger.Dbg("Problem " & lEx.Message)
        End Try
        Return False
    End Function

    Public Function StopMonitor() As Boolean
        If pMonitorProcess Is Nothing Then
            Logger.Dbg("NoMonitorToStop")
            Return False
        ElseIf pMonitorProcess.HasExited Then
            Logger.Dbg("MonitorHasExited")
            pMonitorProcess = Nothing
            Return False
        Else
            pMonitorProcess.Kill()
            Logger.Dbg("MonitorStopped")
            pMonitorProcess = Nothing
            Return True
        End If
    End Function

    Public Sub SendMonitorMessage(ByVal aMessage As String)
        If pMonitorProcess IsNot Nothing Then
            pMonitorProcess.StandardInput.WriteLine(aMessage)
            pMonitorProcess.StandardInput.Flush()
        End If
    End Sub

    Private Sub MonitorMessageHandler(ByVal aSendingProcess As Object, _
                                      ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Logger.Dbg(aOutLine.Data.ToString)
            Logger.Flush()
        End If
    End Sub
End Module
