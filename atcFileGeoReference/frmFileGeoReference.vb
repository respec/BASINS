Imports MapWinUtility
Imports atcMwGisUtility.GisUtil

Public Class frmFileGeoReference
    Private pRecordIndex As Integer = 1
    Private pAddingPoint As Boolean = False

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

    Friend Sub RefreshRecordInfo(ByVal aRecordIndex As Integer)
        pRecordIndex = aRecordIndex
        RefreshRecordInfo()
    End Sub

    Friend Sub RefreshRecordInfo()
        If pRecordIndex > NumFeatures Then pRecordIndex = NumFeatures
        lblRecordInfo.Text = "Record " & pRecordIndex & " of " & NumFeatures
        Dim lImageLocation As String = FieldValue(CurrentLayer, pRecordIndex - 1, FieldIndex(CurrentLayer, cboFields.Text))
        txtValue.Text = lImageLocation
        pbxImage.ImageLocation = lImageLocation
        ClearSelectedFeatures(CurrentLayer)
        SetSelectedFeature(CurrentLayer, pRecordIndex - 1)
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

    Private Sub cboFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFields.SelectedIndexChanged
        RefreshRecordInfo()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pbxImage.Visible = False
        lblStatus.Text = "Click at the Point on the Map associated with the new Item, RightClick anywhere to Cancel"
        lblStatus.Visible = True
        pAddingPoint = True
        While pAddingPoint
            System.Windows.Forms.Application.DoEvents()
        End While
        lblStatus.Visible = False
        pbxImage.Visible = True
    End Sub

    Friend Property AddingPoint() As Boolean
        Get
            Return pAddingPoint
        End Get
        Set(ByVal value As Boolean)
            pAddingPoint = value
        End Set
    End Property

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Logger.Msg("Not Yet Implemented")
    End Sub
End Class