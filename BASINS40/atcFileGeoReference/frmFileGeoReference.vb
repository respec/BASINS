Imports MapWinUtility
Imports atcMwGisUtility.GisUtil

Public Class frmFileGeoReference
    Private pRecordIndex As Integer = 1

    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLayer.SelectedIndexChanged
        Dim lCbo As System.Windows.Forms.ComboBox = sender
        Dim lLayerIndex As Integer = LayerIndex(lCbo.Items(lCbo.SelectedIndex).ToString)
        CurrentLayer = lLayerIndex

        With cboFields
            .Items.Clear()
            .Text = ""
            For lFieldIndex As Integer = 0 To NumFields - 1
                If FieldType(lFieldIndex) = MapWinGIS.FieldType.STRING_FIELD Then
                    .Items.Add(FieldName(lFieldIndex, lLayerIndex))
                End If
            Next
            If .Items.Count > 0 Then
                .SelectedIndex = 0 'TODO: use 'file', 'url', or similar
            Else
                Logger.Msg("No Fields Available for Links")
            End If
            pRecordIndex = 1
            RefreshRecordInfo()
        End With
    End Sub

    Private Sub RefreshRecordInfo()
        lblRecordInfo.Text = "Record " & pRecordIndex & " of " & NumFeatures
        Dim lImageLocation As String = FieldValue(CurrentLayer, pRecordIndex - 1, FieldIndex(CurrentLayer, cboFields.Text))
        txtValue.Text = lImageLocation
        pbxImage.ImageLocation = lImageLocation
    End Sub

    Private Sub btnPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        If pRecordIndex > 1 Then
            pRecordIndex -= 1
            RefreshRecordInfo()
        End If
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If pRecordIndex < NumFeatures Then
            pRecordIndex += 1
            RefreshRecordInfo()
        End If
    End Sub

    Private Sub txtValue_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtValue.MouseClick
        'TODO:select an image file here
    End Sub
End Class