Public Module modLaunch

    ''' <summary>
    ''' Launch a program and redirect its standard output and error into Logger.Dbg
    ''' </summary>
    ''' <param name="aExeName">Full path of program to launch</param>
    ''' <param name="aWorkingDirectory">Folder to start program in</param>
    ''' <param name="aArguments">Command-line arguments to program to launch</param>
    ''' <param name="aWait">
    ''' True to wait until program finishes before returning from LaunchProgram, 
    ''' False to return immediately while program continues to run
    ''' </param>
    ''' <returns>
    ''' 0 if aWait=False and launch was successful
    ''' -1 if launch was unsuccessful
    ''' exit code of program if aWait=True and launch was successful</returns>
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
                If Not String.IsNullOrEmpty(aArguments) Then .Arguments = aArguments
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
        End If
    End Sub

End Module
