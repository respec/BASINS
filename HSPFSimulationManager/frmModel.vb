Imports atcUtility
Imports MapWinUtility

Public Class frmModel

    Private Sub btnWinHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinHSPF.Click
        RunUCI("WinHSPF.exe", txtUCI.Text)
    End Sub

    Private Sub btnRunHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunHSPF.Click
        RunUCI("WinHSPFlt.exe", txtUCI.Text)
    End Sub

    Private Sub RunUCI(ByVal aExeName As String, ByVal aUCIFilename As String)
        Dim lHspfExe As String = atcUtility.FindFile("Please locate " & aExeName, aExeName)
        If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
            lHspfExe = atcUtility.FindFile("Please locate " & aExeName, aExeName, , , True)
            If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
                Logger.Msg("Unable to locate " & aExeName & ", not running.", aExeName)
                Exit Sub
            End If
        End If

        Logger.Status("Running " & aUCIFilename & " (" & lHspfExe & ")")
        MapWinUtility.LaunchProgram(lHspfExe, IO.Path.GetDirectoryName(aUCIFilename), aUCIFilename)
    End Sub
End Class