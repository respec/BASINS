Public Class frmMain
    Dim pSource As atcControls.atcGridSourceTable
    Dim pTable As atcTableSQLite.atcTableSQLite
    Dim pTableName As String = ""
    Dim pDatabaseFileName As String

    Private Sub mnuOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpen.Click
        pTable = New atcTableSQLite.atcTableSQLite
        pDatabaseFileName = atcUtility.FindFile("Open Database", , "db3", , True, , 1)

        If pTable.OpenFile(pDatabaseFileName & vbTab & pTableName) Then
            RefreshForm()
        End If
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Application.Exit()
    End Sub

    Private Sub lstTableName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTableName.Click
        pTable = Nothing
        pTable = New atcTableSQLite.atcTableSQLite
        pTableName = lstTableName.Items(lstTableName.SelectedIndex)
        pTable.OpenFile(pDatabaseFileName & vbTab & pTableName)
        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        pSource = New atcControls.atcGridSourceTable
        pSource.Table = pTable
        atcGrid.Initialize(pSource)
        atcGrid.SizeAllColumnsToContents()
        atcGrid.Invalidate()
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        atcUtility.ChDriveDir(Application.ExecutablePath)
        Dim lLogFileName As String = "..\logs\DatabaseViewer.log"
        MapWinUtility.Logger.StartToFile(lLogFileName)
        If Command.Length > 0 Then
            pTable.OpenFile(Command)
        End If
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        atcGrid.Refresh()
    End Sub

    Private Sub mnuSaveAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSaveAs.Click
        If Not pTable Is Nothing Then
            Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
            With lSaveDialog
                .Title = "Save Table In"
                .DefaultExt = ".db3"
                .FileName = pDatabaseFileName
                .OverwritePrompt = False
                If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    'need to update table name
                    If pTable.WriteFile(.FileName & vbTab & pTableName) Then
                        pDatabaseFileName = .FileName
                        RefreshForm()
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub mnuSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSave.Click
        If Not pTable Is Nothing Then
            pTable.WriteFile(pDatabaseFileName & vbTab & pTableName)
        End If
    End Sub

    Private Sub RefreshForm()
        lstTableName.Items.Clear()
        If pTable.TableNames.Count > 0 Then
            For Each lTableName As String In pTable.TableNames
                lstTableName.Items.Add(lTableName)
            Next
            Dim lSelectedIndex As Integer = pTable.TableNames.IndexFromKey(pTableName)
            If lSelectedIndex > 0 Then
                lstTableName.SelectedIndex = lSelectedIndex
            Else
                lstTableName.SelectedIndex = 0
            End If
            pTableName = lstTableName.Items(lstTableName.SelectedIndex)
        End If
        RefreshGrid()
        Me.Text = "DatabaseViewer " & atcUtility.FilenameNoPath(pDatabaseFileName)
    End Sub
End Class
