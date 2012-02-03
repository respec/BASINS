Public Class frmDownscale

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        If IO.File.Exists(txtRexe.Text) Then
            SaveSetting("Downscaling", "Rscript", "EXE", txtRexe.Text)
            If IO.File.Exists(txtRexe.Text) Then
                SaveSetting("Downscaling", "R", "script", txtRscript.Text)
            Else
                MsgBox("Did not find R script. Set the R script path above.")
            End If
            Dim lSaveDir As String = CurDir()
            atcUtility.ChDriveDir(IO.Path.GetDirectoryName(txtRscript.Text))
            Shell("""" & txtRexe.Text & """ -f """ & txtRscript.Text & """", AppWinStyle.NormalFocus, True)
            atcUtility.ChDriveDir(lSaveDir)
        Else
            MsgBox("Did not find R.exe. Make sure R is installed and check the path above.")
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        txtRexe.Text = GetSetting("Downscaling", "Rcmd", "EXE", "Set complete path to Rcmd.exe here")
        txtRscript.Text = GetSetting("Downscaling", "R", "script", "Set complete path to downscalingv3.R here")
    End Sub
End Class