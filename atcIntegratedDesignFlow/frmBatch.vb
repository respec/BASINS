Imports atcBatchProcessing
Imports MapWinUtility
Imports System.Windows.Forms
Imports System.Threading

Public Class frmBatch
    'Public WithEvents pBatchConfig As New clsBatchBFSpec()
    Private pBatchConfig As clsBatchSpec
    Private pBatchConfigDFLOW As clsBatchSpecDFLOW
    'Dim m_oWorker As BackgroundWorker
    Private pBatchSpecFilefullname As String = "'"
    Public BatchAnalysis As clsBatch.ANALYSIS = clsBatch.ANALYSIS.ITA
    Public Property BatchSpecFile() As String
        Get
            Return pBatchSpecFilefullname
        End Get
        Set(ByVal value As String)
            pBatchSpecFilefullname = value
            If IO.File.Exists(pBatchSpecFilefullname) Then
                txtSpecFile.Text = pBatchSpecFilefullname
            End If
        End Set
    End Property

    Public Sub Initialize()
        Me.Show()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim lFileOpenDiag As New OpenFileDialog()
        With lFileOpenDiag
            .DefaultExt = "txt"
            .Filter = ""
            .InitialDirectory = ""
            If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
                txtSpecFile.Text = .FileName
            End If
        End With
        If System.IO.File.Exists(txtSpecFile.Text) Then
            Dim lTestMsg As String = TestFile(txtSpecFile.Text)
            If lTestMsg.Trim() <> "" Then
                Logger.Msg("Problem with the batch configuration file: " & vbCrLf & lTestMsg, MsgBoxStyle.Information, "Address Batch File Problem")
                btnDoBatch.Enabled = False
            Else
                btnDoBatch.Enabled = True
                Me.Text = BatchAnalysis.ToString() & " Analysis: Batch Run"
            End If
        End If
    End Sub

    Private Function TestFile(ByVal aSpec As String) As String
        Dim lMsg As String = ""
        Dim lSR As System.IO.StreamReader = Nothing
        Dim lBatchAnalysis As clsBatch.ANALYSIS = clsBatch.ANALYSIS.ITA
        Try
            lSR = New System.IO.StreamReader(aSpec)
            While Not lSR.EndOfStream
                Dim line As String = lSR.ReadLine()
                If String.IsNullOrEmpty(line) Then Continue While
                If line.Trim() = "" Then Continue While
                If line.Contains("***") Then Continue While
                If line.ToUpper.StartsWith("SWSTAT") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        line = lSR.ReadLine()
                        If line.Contains("***") Then Continue While
                        If line.Trim() = "" Then Continue While
                        If line.ToUpper.StartsWith("END SWSTAT") Then
                            lReachedEnd = True
                            lBatchAnalysis = clsBatch.ANALYSIS.SWSTAT
                            Exit While
                        End If
                    End While
                    'If lReachedEnd Then Continue While
                End If
                If line.ToUpper.StartsWith("DFLOW") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        line = lSR.ReadLine()
                        If line.Contains("***") Then Continue While
                        If line.Trim() = "" Then Continue While
                        If line.ToUpper.StartsWith("END DFLOW") Then
                            lReachedEnd = True
                            lBatchAnalysis = clsBatch.ANALYSIS.DFLOW
                            Exit While
                        End If
                    End While
                    'If lReachedEnd Then Continue While
                End If
                If lBatchAnalysis = clsBatch.ANALYSIS.DFLOW OrElse lBatchAnalysis = clsBatch.ANALYSIS.SWSTAT Then
                    Exit While
                End If
            End While
        Catch ex As Exception

        Finally
            If lSR IsNot Nothing Then
                lSR.Close() : lSR = Nothing
            End If
        End Try
        If lBatchAnalysis <> clsBatch.ANALYSIS.ITA Then
            BatchAnalysis = lBatchAnalysis
        Else
            lMsg = "Uncertain Analysis Configuration."
        End If
        'If lBatchAnalysis = clsBatch.ANALYSIS.ITA Then
        '    lMsg = "Invalid Analysis Configuration."
        'Else
        '    If lBatchAnalysis = clsBatch.ANALYSIS.DFLOW And BatchAnalysis = clsBatch.ANALYSIS.SWSTAT Then
        '        lMsg = "This batch session is intended for SWSTAT. Please locate a SWSTAT Batch run config file."
        '    ElseIf lBatchAnalysis = clsBatch.ANALYSIS.SWSTAT And BatchAnalysis = clsBatch.ANALYSIS.DFLOW Then
        '        lMsg = "This batch session is intended for DFLOW. Please locate a DFLOW Batch run config file."
        '    Else
        '        BatchAnalysis = lBatchAnalysis
        '    End If
        'End If
        Return lMsg
    End Function

    Private Sub btnDoBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoBatch.Click
        Dim lNewSpecs As Boolean = False
        Dim lBatchConfig As clsBatch = Nothing
        Select Case BatchAnalysis
            Case clsBatch.ANALYSIS.DFLOW
                If pBatchConfigDFLOW Is Nothing Then pBatchConfigDFLOW = New clsBatchSpecDFLOW(Me.ProgressBar1, Me.txtMsg)
                lBatchConfig = pBatchConfigDFLOW
            Case clsBatch.ANALYSIS.SWSTAT
                If pBatchConfig Is Nothing Then pBatchConfig = New clsBatchSpec(Me.ProgressBar1, Me.txtMsg)
                lBatchConfig = pBatchConfig
        End Select
        If lBatchConfig Is Nothing Then
            Logger.Msg("Invalid Batch Configuration.")
            Exit Sub
        End If
        If Not lBatchConfig.SpecFilename = txtSpecFile.Text AndAlso IO.File.Exists(txtSpecFile.Text) Then
            lBatchConfig.SpecFilename = txtSpecFile.Text
            lNewSpecs = True
        End If
        If lNewSpecs Then
            lBatchConfig.Clear()
            lBatchConfig.PopulateScenarios()
            Application.DoEvents()
            lBatchConfig.DoBatch()
        End If
    End Sub

    'Private Sub StatusUpdateHandle(ByVal aMsg As String) Handles pBatchConfig.StatusUpdate
    '    Dim lArr() As String = aMsg.Split(",")
    '    Dim lCount As Integer = Integer.Parse(lArr(0))
    '    Dim lCountTotal As Integer = Integer.Parse(lArr(1))
    '    Dim lMsg As String = lArr(2)
    '    ProgressBar1.Step = 1
    '    ProgressBar1.Minimum = lCount
    '    ProgressBar1.Maximum = lCountTotal
    '    ProgressBar1.PerformStep()
    '    txtMsg.Text &= lMsg & vbCrLf
    'End Sub

    Private Sub frmBatch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'RemoveHandler pBatchConfig.StatusUpdate, AddressOf Me.StatusUpdateHandle
    End Sub

    Private Sub frmBatch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'AddHandler pBatchConfig.StatusUpdate, AddressOf Me.StatusUpdateHandle
        Select Case BatchAnalysis
            Case clsBatch.ANALYSIS.SWSTAT
                pBatchConfig = New clsBatchSpec(Me.ProgressBar1, Me.txtMsg)
                Me.Text = BatchAnalysis.ToString() & " Analysis: Batch Run"
            Case clsBatch.ANALYSIS.DFLOW
                pBatchConfigDFLOW = New clsBatchSpecDFLOW(Me.ProgressBar1, Me.txtMsg)
                Me.Text = BatchAnalysis.ToString() & " Analysis: Batch Run"
            Case Else
                Me.Text = "Batch Run"
        End Select
        btnDoBatch.Enabled = False
    End Sub
End Class