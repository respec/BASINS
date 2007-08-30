Public Class frmMain
    Dim pSource As atcControls.atcGridSourceTable
    Dim pTable As atcTableSQLite.atcTableSQLite
    Dim pTableName As String
    Dim pDatabaseFilename As String

    Private Sub mnuOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpen.Click
        pTable = New atcTableSQLite.atcTableSQLite
        pDatabaseFilename = atcUtility.FindFile("Open Database", , "db3", , True, True, 1)

        pTable.OpenFile(pDatabaseFilename & vbTab & pTableName)
        lstTableName.Items.Clear()
        For Each lTableName As String In pTable.TableNames
            lstTableName.Items.Add(lTableName)
        Next
        lstTableName.SelectedIndex = 0
        RefreshGrid()
        Me.Text = "DatabaseViewer " & atcUtility.FilenameNoPath(pDatabaseFilename)
    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Application.Exit()
    End Sub

    Private Sub lstTableName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTableName.Click
        pTable = Nothing
        pTable = New atcTableSQLite.atcTableSQLite
        pTableName = lstTableName.Items(lstTableName.SelectedIndex)
        pTable.OpenFile(pDatabaseFilename & vbTab & pTableName)
        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        pSource = New atcControls.atcGridSourceTable
        pSource.Table = pTable
        atcGrid.Initialize(pSource)
        atcGrid.SizeAllColumnsToContents()
        atcGrid.Invalidate()
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        atcGrid.Refresh()
    End Sub
End Class
