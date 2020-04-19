Public Class frmSaveData

    Private pDataSource As atcDataSource

    Public Function AskUser(ByVal aDataGroup As atcDataGroup) As atcDataSource
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            If lDataSource.CanSave Then
                lstDataSources.Items.Add(lDataSource.Specification)
            End If
        Next

        If Me.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Select Case lstDataSources.SelectedIndex
                Case -1 'Nothing selected
                Case 0  'First item, browse for a file to save in
                    Dim lFilesOnly As New ArrayList(1)
                    lFilesOnly.Add("File")
                    Dim lDataSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", False, True)
                    If Not lDataSource Is Nothing Then
                        If Not lDataSource.Open("") Then lDataSource = Nothing
                    End If
                    Return lDataSource
                Case Is > 0 'Already-open file
                    Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lstDataSources.SelectedItem)
                    If lDataSource IsNot Nothing AndAlso lDataSource.CanSave Then
                        Return lDataSource
                    End If
            End Select
        End If
        Return Nothing
    End Function

    Private Sub lstDataSources_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstDataSources.DoubleClick
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

End Class