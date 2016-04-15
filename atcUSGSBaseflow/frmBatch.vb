Imports System.Windows.Forms
Imports System.Threading

Public Class frmBatch
    'Public WithEvents pBatchConfig As New clsBatchBFSpec()
    Private pBatchConfig As clsBatchBFSpec
    'Dim m_oWorker As BackgroundWorker
    Private pBatchSpecFilefullname As String = "'"
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
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtSpecFile.Text = .FileName
            End If
        End With
    End Sub

    Private Sub btnDoBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoBatch.Click
        Dim lNewSpecs As Boolean = False
        If Not pBatchConfig.SpecFilename = txtSpecFile.Text AndAlso IO.File.Exists(txtSpecFile.Text) Then
            pBatchConfig.SpecFilename = txtSpecFile.Text
            lNewSpecs = True
        End If
        If lNewSpecs Then
            pBatchConfig.Clear()
            pBatchConfig.PopulateScenarios()
            Application.DoEvents()
            'pBatchConfig.DoBatch()
            pBatchConfig.DoBatchIntermittent()
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
        pBatchConfig = New clsBatchBFSpec(Me.ProgressBar1, Me.txtMsg)
    End Sub
End Class