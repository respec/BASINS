Imports atcData
Imports MapWinUtility

Public Class frmImport

    Private pSaveFolderSame As String = "(same folder as text file)"

    'Private Sub txtDataType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataType.Click
    '    Dim lFilesOnly As New ArrayList(1)
    '    lFilesOnly.Add("File")
    '    Dim lDataSource As atcData.atcDataSource = atcData.atcDataManager.UserSelectDataSource(lFilesOnly, "Select type of data to import")
    '    If lDataSource IsNot Nothing Then
    '        pDataSourceTemplate = lDataSource
    '        txtDataType.Text = pDataSourceTemplate.Name
    '    End If
    'End Sub

    Private Sub btnBrowseFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFiles.Click
        Dim lOpenDialog As New System.Windows.Forms.OpenFileDialog
        With lOpenDialog
            .Title = "Select Data File(s) to import"
            .Filter = "All Files|*.*"
            .FilterIndex = 0
            .Multiselect = True
            If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Filenames = .FileNames
            End If
        End With

    End Sub

    Private Sub btnBrowseScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseScript.Click
        Dim lFilenames() As String = Filenames
        If lFilenames.Length > 0 AndAlso IO.File.Exists(lFilenames(0)) Then
            Dim lScriptFilename As String = atcTimeseriesScript.atcTimeseriesScriptPlugin.SelectScript(lFilenames(0), "Select script for importing to WDM", False)
            If IO.File.Exists(lScriptFilename) Then
                txtScript.Text = lScriptFilename
                SaveSetting("BASINS", "WDMImport", "Script", txtScript.Text)
            End If
        Else
            MsgBox("Choose a file to import before choosing the script.")
        End If
        'Dim lOpenDialog As New Windows.Forms.OpenFileDialog
        'With lOpenDialog
        '    .Title = lblScript.Text
        '    .DefaultExt = "ws"
        '    .Filter = "Wizard Scripts (*.ws)|*.ws|All Files|*.*"
        '    .FilterIndex = 0
        '    If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        '        txtScript.Text = .FileName
        '    End If
        'End With
    End Sub

    Private Sub btnBrowseSaveIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSaveIn.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save in WDM"
            .Filter = "WDM Files (*.wdm)|*.wdm"
            .FilterIndex = 0
            .CheckFileExists = False
            .CheckPathExists = False
            If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                txtSaveIn.Text = .FileName
                SaveSetting("BASINS", "WDMImport", "SaveIn", txtSaveIn.Text)
            End If
        End With
    End Sub

    Public Property Filenames() As String()
        Get
            Return txtDataFiles.Text.Split(Chr(13), Chr(10))
        End Get
        Set(ByVal value As String())
            txtDataFiles.Text = String.Join(vbCrLf, value)
        End Set
    End Property

    Private Sub Form_DragEnter(
        ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, txtDataFiles.DragEnter, txtScript.DragEnter, btnBrowseScript.DragEnter, txtSaveFolder.DragEnter, txtSaveIn.DragEnter, btnBrowseSaveIn.DragEnter

        If e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = System.Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub Form_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragDrop, txtDataFiles.DragDrop
        Try
            If e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) Then
                Filenames = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)
            End If
        Catch
        End Try
    End Sub

    Private Sub txtScript_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtScript.DragDrop, btnBrowseScript.DragDrop
        Try
            If e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) Then
                txtScript.Text = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)(0)
                SaveSetting("BASINS", "WDMImport", "Script", txtScript.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub txtSaveFolder_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtSaveFolder.DragDrop
        Try
            If e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) Then
                txtSaveFolder.Text = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)(0)
                SaveSetting("BASINS", "WDMImport", "SaveFolder", txtSaveFolder.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub txtSaveIn_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtSaveIn.DragDrop, btnBrowseSaveIn.DragDrop
        Try
            If e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) Then
                txtSaveIn.Text = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)(0)
                SaveSetting("BASINS", "WDMImport", "SaveIn", txtSaveIn.Text)
            End If
        Catch
        End Try
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        SaveSetting("BASINS", "WDMImport", "Script", txtScript.Text)
        SaveSetting("BASINS", "WDMImport", "Each", radioWDMeach.Checked)
        If radioWDMeach.Checked Then
            SaveSetting("BASINS", "WDMImport", "SaveFolder", txtSaveFolder.Text)
            Import(Filenames, txtScript.Text, True, txtSaveFolder.Text)
        Else
            SaveSetting("BASINS", "WDMImport", "SaveIn", txtSaveIn.Text)
            Import(Filenames, txtScript.Text, False, txtSaveIn.Text)
        End If
        Me.Close()
    End Sub

    Private Sub Import(ByVal aFilenames() As String, ByVal aScriptFilename As String, ByVal aEach As Boolean, ByVal aSaveIn As String)
        Dim lSaveDisplayMessageBoxes As Boolean = Logger.DisplayMessageBoxes
        Try
            Logger.DisplayMessageBoxes = False
            Dim lSaveInIsDirectory As Boolean = IO.Directory.Exists(aSaveIn)
            Dim lWDMFilename As String = aSaveIn
            Dim lExistingWDM As atcWDM.atcDataSourceWDM = Nothing

            If Not aEach Then
                lExistingWDM = atcDataManager.DataSourceBySpecification(aSaveIn)
            End If

            Dim lWDM As atcWDM.atcDataSourceWDM = lExistingWDM
            If lWDM Is Nothing AndAlso Not aEach Then
                lWDM = New atcWDM.atcDataSourceWDM
                lWDM.Open(aSaveIn)
            End If

            Dim lFilesImported As Integer = 0
            Dim lDatasetsImported As Integer = 0
            Dim lFilenames As New Generic.List(Of String)
            For Each lImportFilename As String In Filenames
                If IO.File.Exists(lImportFilename) Then
                    lFilenames.Add(lImportFilename)
                End If
            Next
            For Each lImportFilename As String In lFilenames
                Logger.Progress("Importing " & lImportFilename, lFilesImported, lFilenames.Count)
                Using lProgress As New ProgressLevel
                    Dim lScript As New atcTimeseriesScript.atcTimeseriesScriptPlugin

                    If aEach Then
                        If lSaveInIsDirectory Then 'Save in WDM folder
                            lWDMFilename = IO.Path.Combine(aSaveIn, IO.Path.ChangeExtension(IO.Path.GetFileName(lImportFilename), "wdm"))
                        Else 'Save in same folder with input data
                            lWDMFilename = IO.Path.ChangeExtension(lImportFilename, "wdm")
                        End If
                        lWDM = atcDataManager.DataSourceBySpecification(lWDMFilename)
                        If lWDM Is Nothing Then
                            lWDM = New atcWDM.atcDataSourceWDM
                            lWDM.Open(lWDMFilename)
                        End If
                    End If
                    Dim lCountBefore As Integer = lWDM.DataSets.Count
                    lScript.RunSelectedScript(aScriptFilename, lImportFilename, lWDM)

                    lDatasetsImported += lWDM.DataSets.Count - lCountBefore

                    If aEach Then lWDM = Nothing

                    lScript.Clear()
                    lFilesImported += 1
                End Using
                Logger.Progress("Imported " & lImportFilename, lFilesImported, lFilenames.Count)
                Logger.Dbg("Pre-Collect " & atcUtility.MemUsage())
                System.GC.Collect()
                Logger.Dbg("Post-Collect " & atcUtility.MemUsage())
            Next
            Logger.DisplayMessageBoxes = lSaveDisplayMessageBoxes

            Dim lMessage As String = "Imported " & lDatasetsImported & " dataset"
            If lDatasetsImported > 1 Then lMessage &= "s"
            lMessage &= " from " & lFilesImported & " file"
            If lFilesImported > 1 Then lMessage &= "s"

            If lFilesImported = 1 OrElse Not aEach Then
                If lExistingWDM Is Nothing Then
                    If Logger.Msg(lMessage & vbCrLf & "into " & lWDMFilename & vbCrLf & "Open this WDM file now?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                        If atcDataManager.OpenDataSource(New atcWDM.atcDataSourceWDM, lWDMFilename, Nothing) Then
                            atcDataManager.UserManage(, atcDataManager.DataSources.Count - 1)
                        End If
                    End If
                Else
                    Logger.Msg(lMessage & vbCrLf & "into " & lWDMFilename & vbCrLf & "which is already open in BASINS.", Me.Text)
                End If
            ElseIf lSaveInIsDirectory Then
                Logger.Msg(lMessage & vbCrLf & "into " & aSaveIn, Me.Text)
            Else
                Logger.Msg(lMessage & ".", Me.Text)
            End If
        Catch ex As Exception
            Logger.DisplayMessageBoxes = lSaveDisplayMessageBoxes
            Logger.Msg("Exception importing " & aFilenames.Length & " files into " & aSaveIn & vbCrLf & "using " & aScriptFilename & "." & vbCrLf & ex.ToString, Me.Text)
        End Try
    End Sub

    Private Shared Function HaveSharedDates(ByVal aDataSets As atcTimeseriesGroup) As Boolean
        Dim lLastIndex As Integer = aDataSets.Count - 1
        For lTsToCheckIndex As Integer = 0 To lLastIndex - 1
            With aDataSets(lTsToCheckIndex)
                For lCompareToIndex As Integer = lTsToCheckIndex + 1 To lLastIndex
                    If aDataSets(lCompareToIndex).Dates.Serial = .Dates.Serial Then
                        Return True
                    End If
                Next
            End With
        Next
        Return False
    End Function

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        On Error Resume Next
        If g_MapWin IsNot Nothing Then Me.Icon = g_MapWin.ApplicationInfo.FormIcon()
        txtScript.Text = GetSetting("BASINS", "WDMImport", "Script", "")
        radioWDMeach.Checked = GetSetting("BASINS", "WDMImport", "Each", "True")
        radioWDMone.Checked = Not radioWDMeach.Checked
        txtSaveFolder.Text = GetSetting("BASINS", "WDMImport", "SaveFolder", pSaveFolderSame)
        txtSaveIn.Text = GetSetting("BASINS", "WDMImport", "SaveIn", "")
    End Sub
End Class