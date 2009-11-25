Imports atcUCI
Imports MapWinUtility
Imports atcUtility

Public Class frmAQUATOX

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Public Sub Init()

        cboRchres.Items.Clear()
        For Each lOper As HspfOperation In pUCI.OpnSeqBlock.Opns
            If lOper.Name = "RCHRES" Then
                If frmOutput.IsAQUATOXLocation(lOper.Name, lOper.Id) Then
                    'this is an aquatox output location
                    cboRchres.Items.Add(lOper.Name & " " & lOper.Id)
                End If
            End If
        Next
        If cboRchres.Items.Count > 0 Then
            cboRchres.SelectedIndex = 0
        End If

        'identify the output wdm
        Dim lWDMId As Integer = 0
        Dim lOutputWDM As atcData.atcTimeseriesSource = Nothing
        For lWdmIndex As Integer = 1 To 4
            If Not pUCI.GetWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                lWDMId = lWdmIndex
                lOutputWDM = pUCI.GetWDMObj(lWdmIndex)
                Exit For
            End If
        Next lWdmIndex

        If lWDMId > 0 Then
            lblWDMFile.Text = lOutputWDM.Specification
        End If

        'default watershed file name
        Dim lWsdName As String = FilenameNoExt(pUCIFullFileName) & ".wsd"
        If FileExists(lWsdName) Then    'wsd file exists
            lblWatershedFile.Text = lWsdName
        End If
    End Sub

    Private Sub cmdWatershedFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWatershedFile.Click

        OpenFileDialog1.InitialDirectory = PathNameOnly(pUCIFullFileName)
        OpenFileDialog1.Filter = "BASINS Watershed Files (*.wsd)|*.wsd"
        OpenFileDialog1.FileName = "*.wsd"
        OpenFileDialog1.Title = "Select BASINS Watershed File"

        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblWatershedFile.Text = OpenFileDialog1.FileName
        Else
            Exit Sub
        End If

    End Sub

    Private Sub cmdWDMFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWDMFile.Click

        OpenFileDialog1.InitialDirectory = PathNameOnly(pUCIFullFileName)
        OpenFileDialog1.Filter = "WDM files (*.wdm)|*.wdm"
        OpenFileDialog1.FileName = "*.wdm"
        OpenFileDialog1.Title = "Select Project WDM File"

        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblWDMFile.Text = OpenFileDialog1.FileName
        Else
            Exit Sub
        End If

    End Sub

    Private Sub cmdAQUATOX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAQUATOX.Click
        If lblWDMFile.Text = "<none>" Then
            'no project file specified, don't allow to okay
            Logger.Msg("A project WDM file must be specified.", "WinHSPF-AQUATOX Problem")
        Else
            StartAquatox()
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub StartAquatox()

        Dim lAQUATOXexe As String = FindFile("Please locate AQUATOX.exe", "AQUATOX.exe")

        If Not IO.File.Exists(lAQUATOXexe) Then
            Logger.Msg("Cannot find AQUATOX.exe", MsgBoxStyle.Critical, "WinHSPF AQUATOX Problem")
        Else
            'command line includes model, path to basins gis files,
            'project wdm name, scenario, and loc name
            Dim lRchid As String = cboRchres.SelectedItem
            Dim lCommand As String = ""
            Dim lTname As String = StrRetRem(lRchid)
            If lblWatershedFile.Text = "<none>" Then  'no gis files, do anyway
                lCommand = " HSPF XXX " & """" & _
                  lblWDMFile.Text.Trim & """" & " " & IO.Path.GetFileNameWithoutExtension(pUCIFullFileName).ToUpper & _
                  " RCH" & lRchid & " SUM"
            Else
                lCommand = " HSPF " & """" & FilenameNoExt(lblWatershedFile.Text) & """" & " " & _
                  """" & lblWDMFile.Text.Trim & """" & " " & IO.Path.GetFileNameWithoutExtension(pUCIFullFileName).ToUpper & _
                  " RCH" & lRchid & " SUM"
            End If

            Logger.Dbg("StartAQUATOX:" & lAQUATOXexe & ":" & lCommand)
            Process.Start(lAQUATOXexe, lCommand)
            Logger.Dbg("AQUATOXStarted")
        End If

    End Sub
End Class