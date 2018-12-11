Public Class frmBatchRun
    Dim NumToRun As Integer = 0
    Dim NumFinished As Integer = 0

    Friend Const g_AppNameShort As String = "BatchRun"
    Friend Const g_AppNameLong As String = "Batch Run"

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        btnRun.Enabled = False
        Try
            Dim lApplication As String = txtApplication.Text
            If Not IO.File.Exists(lApplication) Then
                MsgBox("Application not found: " & lApplication, MsgBoxStyle.OkOnly, g_AppNameLong)
                Exit Sub
            End If

            Dim lDirectory As String = txtDirectory.Text
            If Not IO.Directory.Exists(lDirectory) Then
                MsgBox("Directory not found: " & lDirectory, MsgBoxStyle.OkOnly, g_AppNameLong)
                Exit Sub
            End If

            SaveSetting(g_AppNameShort, "Init", "Application", lApplication)
            SaveSetting(g_AppNameShort, "Init", "Directory", lDirectory)

            Dim lAllArgs As String = txtArguments.Text
            lAllArgs = lAllArgs.Replace(vbLf, vbCr)
            While lAllArgs.Contains(vbCr & vbCr)
                lAllArgs = lAllArgs.Replace(vbCr & vbCr, vbCr)
            End While
            'Threading.ThreadPool.SetMaxThreads(txtParallel.Text, 1)
            Dim lArgsList As New List(Of String)
            For Each lLine As String In lAllArgs.Split(vbCr)
                If lLine.Trim.Length > 0 Then
                    lArgsList.Add(lLine)
                End If
            Next
            NumToRun = lArgsList.Count
            If NumToRun = 0 Then
                MsgBox("No arguments specified", MsgBoxStyle.OkOnly, g_AppNameLong)
                Exit Sub
            End If
            NumFinished = 0
            Me.Text = g_AppNameLong & " Running first of " & NumToRun & " runs"
            For Each lLine As String In lArgsList
                'Threading.ThreadPool.QueueUserWorkItem(AddressOf StartThread, Tuple.Create(Of String, String, String)(lApplication, lDirectory, lLine.Replace(vbTab, " ")))
                Dim lStartTime As Date = Date.Now
                txtProgress.Text &= vbCrLf & lStartTime.ToString("HH:mm:ss") & " Started " & lLine
                LaunchProgram(lApplication, lDirectory, lLine) '.Replace(vbTab, " "))
                Dim lFinishTime As Date = Date.Now
                txtProgress.Text &= vbCrLf & lFinishTime.ToString("HH:mm:ss")
                Dim lElapsed As TimeSpan = lFinishTime.Subtract(lStartTime)
                If lElapsed.TotalDays > 1 Then
                    txtProgress.Text &= " Finished in " & lElapsed.ToString("d\d\ hh\:mm") & " elapsed time"
                Else
                    txtProgress.Text &= " Finished in " & lElapsed.ToString("hh\:mm\:ss") & " elapsed time"
                End If
                Me.Text = g_AppNameLong & " Finished " & NumFinished & " of " & NumToRun
                Me.Refresh()
                Application.DoEvents()
                System.Threading.Thread.Sleep(200)
            Next
            'While NumFinished < NumToRun
            '    Me.Text = "Finished " & NumFinished & " of " & NumToRun
            '    Me.Refresh()
            '    Application.DoEvents()
            '    System.Threading.Thread.Sleep(200)
            'End While
            Me.Text = g_AppNameLong & " Completed all " & NumFinished & " runs"
        Finally
            btnRun.Enabled = True
        End Try
    End Sub

    Sub StartThread(ByVal stateInfo As Object)
        LaunchProgram(stateInfo.Item1, stateInfo.Item2, stateInfo.Item3, True)
    End Sub

    Function LaunchProgram(ByVal aExeName As String, _
                              ByVal aWorkingDirectory As String, _
                     Optional ByVal aArguments As String = "", _
                     Optional ByVal aWait As Boolean = True) As Integer
        Dim lExitCode As Integer = 0
        Try
            Dim lProcess As New System.Diagnostics.Process
            With lProcess.StartInfo
                .FileName = aExeName
                .WorkingDirectory = aWorkingDirectory
                .CreateNoWindow = True
                .UseShellExecute = False
                If Not String.IsNullOrEmpty(aArguments) Then .Arguments = aArguments
            End With
            lProcess.Start()
            If aWait Then
                Try
                    While Not lProcess.HasExited
                        Threading.Thread.Sleep(500)
                        Application.DoEvents()
                    End While
                Catch lWaitError As Exception
                    MsgBox(lWaitError.Message, MsgBoxStyle.Critical, g_AppNameLong)
                End Try
            End If
        Catch lEx As ApplicationException
            MsgBox("Exception: " & lEx.Message, MsgBoxStyle.Critical, g_AppNameLong)
            lExitCode = -1
        End Try
        NumFinished += 1
        Return lExitCode
    End Function

    Private Sub frmBatchRun_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtApplication.Text = GetSetting(g_AppNameShort, "Init", "Application")
        txtDirectory.Text = GetSetting(g_AppNameShort, "Init", "Directory")
    End Sub

    Private Sub txtArguments_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtArguments.KeyPress, txtProgress.KeyPress
        Select Case e.KeyChar
            Case Chr(1)
                sender.SelectAll()
                e.Handled = True
        End Select
    End Sub

    Private Sub frmBatchRun_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim lTxtWidth As Integer = Me.ClientRectangle.Width - txtApplication.Left - 20
        If lTxtWidth > 20 AndAlso txtArguments.Left + txtArguments.Width > Me.ClientRectangle.Width Then
            txtArguments.Width = lTxtWidth
            txtApplication.Width = lTxtWidth
            txtProgress.Width = lTxtWidth
            btnRun.Left = Me.ClientRectangle.Width - btnRun.Width - 20
        End If
    End Sub
End Class
