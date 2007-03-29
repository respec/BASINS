Public Class frmSaveData

    Private pDataManager As atcDataManager
    Private pDataSource As atcDataSource

    Public Function AskUser(ByVal aDataManager As atcDataManager, ByVal aDataGroup As atcDataGroup) As atcDataSource
        pDataManager = aDataManager

        For Each lDataSource As atcDataSource In pDataManager.DataSources
            If lDataSource.CanSave Then
                lstDataSources.Items.Add(lDataSource.Specification)
            End If
        Next

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Select Case lstDataSources.SelectedIndex
                Case -1 'Nothing selected
                Case 0  'First item, browse for a file to save in
                    Dim lFilesOnly As New ArrayList(1)
                    lFilesOnly.Add("File")
                    Dim lDataSource As atcDataSource = pDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", False, True)
                    If Not lDataSource Is Nothing Then
                        If Not lDataSource.Open("") Then lDataSource = Nothing
                    End If
                    Return lDataSource
                Case Is > 0 'Already-open file
                    For Each lDataSource As atcDataSource In pDataManager.DataSources
                        If lDataSource.CanSave AndAlso lDataSource.Specification.Equals(lstDataSources.SelectedItem) Then
                            Return lDataSource
                        End If
                    Next
            End Select
        End If
        Return Nothing
    End Function

    Private Sub lstDataSources_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstDataSources.DoubleClick
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

End Class