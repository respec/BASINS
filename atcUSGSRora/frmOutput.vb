Public Class frmOutput

    Private pOutputPath As String
    Private OutputFilenameRoot As String

    Private pFileRoraSum10 As String = "rorasum.txt"
    Private pFileRoraPek12 As String = "rorapek.txt"
    Private pFileRoraMon14 As String = "roramon.txt"
    Private pFileRoraQrt15 As String = "roraqrt.txt"
    Private pFileRoraWY16 As String = "roraWY.txt"

    Private pOutputFileDictionary As New System.Collections.Generic.Dictionary(Of String, String)

    Public Sub Initialize(ByVal aOutputPath As String, ByVal aRootname As String)
        pOutputPath = aOutputPath
        OutputFilenameRoot = aRootname

        'Set output files' names
        Dim lFileRoraSum10 As String = IO.Path.Combine(pOutputPath, OutputFilenameRoot & "_" & pFileRoraSum10)
        Dim lFileRoraPek12 As String = IO.Path.Combine(pOutputPath, OutputFilenameRoot & "_" & pFileRoraPek12)
        Dim lFileRoraMon14 As String = IO.Path.Combine(pOutputPath, OutputFilenameRoot & "_" & pFileRoraMon14)
        Dim lFileRoraQrt15 As String = IO.Path.Combine(pOutputPath, OutputFilenameRoot & "_" & pFileRoraQrt15)
        Dim lFileRoraWY16 As String = IO.Path.Combine(pOutputPath, OutputFilenameRoot & "_" & pFileRoraWY16)

        With pOutputFileDictionary
            .Add("sum", lFileRoraSum10)
            .Add("pek", lFileRoraPek12)
            .Add("mon", lFileRoraMon14)
            .Add("qrt", lFileRoraQrt15)
            .Add("wy", lFileRoraWY16)
        End With

    End Sub

    Private Sub rdoResultType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRoraMon.CheckedChanged, _
                                                                                                              rdoRoraPek.CheckedChanged, _
                                                                                                              rdoRoraQrt.CheckedChanged, _
                                                                                                              rdoRoraWY.CheckedChanged, _
                                                                                                              rdoRoraSum.CheckedChanged

        Dim lSR As System.IO.StreamReader = Nothing
        Dim lFilename As String = ""
        If rdoRoraMon.Checked Then
            lFilename = pOutputFileDictionary("mon")
        ElseIf rdoRoraPek.Checked Then
            lFilename = pOutputFileDictionary("pek")
        ElseIf rdoRoraQrt.Checked Then
            lFilename = pOutputFileDictionary("qrt")
        ElseIf rdoRoraSum.Checked Then
            lFilename = pOutputFileDictionary("sum")
        ElseIf rdoRoraWY.Checked Then
            lFilename = pOutputFileDictionary("wy")
        End If
        If lFilename <> "" Then lSR = New System.IO.StreamReader(lFilename)
        If lSR IsNot Nothing Then
            txtResultFileContent.Text = lSR.ReadToEnd()
            System.Windows.Forms.Application.DoEvents()
            'txtResultFileContent.SelectionLength = 0
            txtResultFileContent.SelectionStart = 0
            lSR.Close()
            lSR = Nothing
            txtOutputPath.Text = lFilename
        End If

    End Sub

    Private Sub frmOutput_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtOutputPath.Text = pOutputPath
    End Sub
End Class