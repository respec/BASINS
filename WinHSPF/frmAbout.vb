Imports atcUtility
Imports MapWinUtility

Public Class frmAbout
    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        PictureBox1.Image = ImageList1.Images(0)
        PictureBox2.Image = ImageList1.Images(2)
        PictureBox1.Cursor = Windows.Forms.Cursors.Hand
        PictureBox2.Cursor = Windows.Forms.Cursors.Hand

        txtLabel.Text = StatusString()

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("https://www.epa.gov/exposure-assessment-models/basins")
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        System.Diagnostics.Process.Start("http://www.respec.com/solution/modeling-optimization/")
    End Sub

    Private Function StatusString() As String
        Dim lS As String = ""

        lS = "WinHSPF - Version " & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart & _
             "." & System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart
        Dim lRev As Integer = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart
        If lRev >= 1000 Then
            If lRev > 1000 Then lS = lS & " build " & lRev - 1000
            lS = lS & vbCrLf ' " final" & vbCrLf
        Else
            lS = lS & " beta " & lRev & vbCrLf
            lS = lS & "FOR TESTING AND EVALUATION USE ONLY" & vbCrLf
        End If
        lS = lS & "-----------" & vbCrLf
        lS = lS & "Inquiries about this software should be directed to" & vbCrLf
        lS = lS & "the organization which supplied you this software." & vbCrLf
        lS = lS & "-----------" & vbCrLf

        lS = lS & Space(2) & "Current Directory: " & CurDir() & vbCrLf & vbCrLf
        If pUCI Is Nothing OrElse Len(pUCI.Name) = 0 Then
            lS = lS & Space(2) & "No Project Active" & vbCrLf
        Else
            lS = lS & Space(2) & "Project File: " & pUCI.Name & vbCrLf
            Dim HSPFEngineExe As String = GetSetting("HSPFEngineNet", "files", "HSPFEngineNet.exe", "HSPFEngineNet.exe")
            If HSPFEngineExe.Length > 0 Then
                lS = lS & Space(2) & "HSPF Engine: " & HSPFEngineExe & vbCrLf
                lS = lS & Space(2) & "HSPF Message File: " & PathNameOnly(HSPFEngineExe) & "hspfmsg.wdm" & vbCrLf
            End If
        End If

        StatusString = lS
    End Function

    Private Sub frmAbout_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("")
        End If
    End Sub
End Class