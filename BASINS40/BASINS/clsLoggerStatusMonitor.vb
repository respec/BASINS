Imports MapWinUtility

Public Class clsLoggerStatusMonitor
    Implements MapWinUtility.IProgressStatus

    Private pInit As Boolean = False
    Private pMonitorProcess As Process
    Private pPipeWriteToStatus As Integer = 0
    Private pPipeReadFromStatus As Integer = 0

    <CLSCompliant(False)> _
    Public InnerProgressStatus As IProgressStatus = Nothing

    Public Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
        If InnerProgressStatus IsNot Nothing Then InnerProgressStatus.Progress(aCurrentPosition, aLastPosition)
        WriteStatus("PROGRESS " & aCurrentPosition & " of " & aLastPosition)
    End Sub

    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
        If InnerProgressStatus IsNot Nothing Then InnerProgressStatus.Status(aStatusMessage)
        If Not pInit Then
            Try
                Dim lProcessId As Integer = Process.GetCurrentProcess.Id
                pMonitorProcess = New Process
                With pMonitorProcess.StartInfo
                    .FileName = atcUtility.FindFile("Status Monitor", "statusMonitor.exe")
                    .Arguments = lProcessId
                    .CreateNoWindow = True
                    .UseShellExecute = False
                    .RedirectStandardInput = True
                    .RedirectStandardOutput = True
                    AddHandler pMonitorProcess.OutputDataReceived, AddressOf MonitorMessageHandler
                    .RedirectStandardError = True
                    'AddHandler pMonitorProcess.ErrorDataReceived, AddressOf MonitorMessageHandler
                End With
                pMonitorProcess.Start()
                '
                'NOTE: to debug pMonitorProcess, in VS2005 (not Express) - choose Tools:AttachToProcess - StatusMonitor
                '
                pMonitorProcess.StandardInput.WriteLine("Show")
                'pMonitorProcess.BeginErrorReadLine()
                'pMonitorProcess.BeginOutputReadLine()
                Logger.Dbg("MonitorLaunched")
                Dim lStreamMonitorInputFromMyOutput As IO.FileStream = pMonitorProcess.StandardInput.BaseStream
                pPipeWriteToStatus = lStreamMonitorInputFromMyOutput.SafeFileHandle.DangerousGetHandle
                Dim lStreamMonitorOutputToMyInput As IO.FileStream = pMonitorProcess.StandardOutput.BaseStream
                pPipeReadFromStatus = lStreamMonitorOutputToMyInput.SafeFileHandle.DangerousGetHandle
            Catch ex As Exception
                Logger.Msg("StatusProcessStartError:" & ex.Message)
            End Try
            pInit = True
        End If

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
        Else

        End If
        pMonitorProcess.StandardInput.WriteLine(aStatusMessage)

        Return True
    End Function

    Private Sub MonitorMessageHandler(ByVal aSendingProcess As Object, _
                                      ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Logger.Dbg(aOutLine.Data.ToString)
            Logger.Flush()
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Try
            If pMonitorProcess IsNot Nothing AndAlso Not pMonitorProcess.HasExited Then
                pMonitorProcess.StandardInput.WriteLine("Exit")
                Windows.Forms.Application.DoEvents()
                Threading.Thread.Sleep(100)
                If Not pMonitorProcess.HasExited Then pMonitorProcess.Kill()
            End If
        Catch e As Exception
            Windows.Forms.Application.DoEvents()
        End Try
        MyBase.Finalize()
    End Sub
End Class
