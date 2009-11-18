Imports System.IO

Public Class RunMultiWeppForm1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        txtPathBase.Text = "<>"
        txtPathBase.Text = "Z:\Documents\filecabinet\employment\aquaterra\active.projects\SERDP\Roads\WEPP\cli.met\4\in.slp"
        txtPathWepp.Text = "<>"
        txtPathOutput.Text = "<>"
        txtPathOutput.Text = "C:\output.txt"

        txtSlopeStart.Text = "0"
        txtSlopeStop.Text = "0"
        txtSlopeDelta.Text = "0"

        txtLengthStart.Text = "0"
        txtLengthStop.Text = "0"
        txtLengthDelta.Text = "0"

    End Sub

    Public Sub ShellandWait(ByVal ProcessPath As String)
        Dim objProcess As System.Diagnostics.Process
        Try
            objProcess = New System.Diagnostics.Process()
            objProcess.StartInfo.FileName = ProcessPath
            objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            objProcess.Start()

            'Wait until the process passes back an exit code 
            objProcess.WaitForExit()

            'Free resources associated with this process
            objProcess.Close()
        Catch
            MessageBox.Show("Could not start process " & ProcessPath, "Error")
        End Try
    End Sub
    

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            OpenFileDialog1.ShowDialog()

            If Not OpenFileDialog1.FileName Is Nothing Then
                txtPathBase.Text = OpenFileDialog1.FileName
            End If

        Catch ex As Exception
            MsgBox("Bad Path Name")
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            OpenFileDialog1.Reset()
            OpenFileDialog1.ShowDialog()

            If Not OpenFileDialog1.FileName Is Nothing Then
                txtPathWepp.Text = OpenFileDialog1.FileName
            End If

        Catch ex As Exception
            MsgBox("Bad Path Name")
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            OpenFileDialog1.Reset()
            OpenFileDialog1.ShowDialog()

            If Not OpenFileDialog1.FileName Is Nothing Then
                txtPathOutput.Text = OpenFileDialog1.FileName
            End If

        Catch ex As Exception
            MsgBox("Bad Path Name")
        End Try
    End Sub


    Private Sub btnExecute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecute.Click
        Try

            'Read the source slop file (once)
            Dim objReader As New System.IO.StreamReader(txtPathBase.Text)
            Dim lLinesSlopeIn As New ArrayList

            Do While objReader.Peek() <> -1
                lLinesSlopeIn.Add(objReader.ReadLine())
            Loop

            'Copy the master slope file input to a local copy to much with
            Dim lLinesCurrentSlope As ArrayList = lLinesSlopeIn

            'Get the new length
            lLinesCurrentSlope(4) = "2 " & txtLengthStart.Text

            'read line 6 and separate the elements by delimiter of comma

            Dim lLine6Elements() As String = lLinesSlopeIn(5).ToString.Split(" ")
            lLine6Elements(1) = txtSlopeStart.Text

            lLinesCurrentSlope(5) = "2 " & CDbl(txtLengthStart.Text)

            'Write the modified Slope File
            If Not System.IO.File.Exists(txtPathOutput.Text) Then System.IO.File.Create(txtPathOutput.Text, 1, FileOptions.None)

            Dim objWriter As New System.IO.StreamWriter(txtPathOutput.Text)

            For i = 0 To lLinesSlopeIn.Count - 1
                objWriter.WriteLine(lLinesCurrentSlope(i))
            Next

            objWriter.Close()

        Catch ex As Exception
            MsgBox("Something(s) Bad, Check Input, paths and Wepp error file")
        End Try
    End Sub
End Class