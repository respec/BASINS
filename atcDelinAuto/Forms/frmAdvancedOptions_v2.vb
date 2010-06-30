Public Class frmAdvancedOptions_v2
    Private Sub frmAdvancedOptions_v2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkbxFillPits.Checked = tdbChoiceList.AddPitfillLayer
        chkbxD8.Checked = tdbChoiceList.AddD8Layer
        chkbxD8Area.Checked = tdbChoiceList.AddD8AreaLayer
        chkbxGridNet.Checked = tdbChoiceList.AddGridNetLayer
        chkbxFullNet.Checked = tdbChoiceList.AddRiverRasterLayer
        chkbxStreamOrder.Checked = tdbChoiceList.AddOrderGridLayer
        chkbxShedGrid.Checked = tdbChoiceList.AddWShedGridLayer
        chkBxStreamShape.Checked = tdbChoiceList.AddStreamShapeLayer
        chkbxShedToShape.Checked = tdbChoiceList.AddWShedShapeLayer
        chkbxStreamFields.Checked = tdbChoiceList.CalcSpecialStreamFields
        chkbxWshedFields.Checked = tdbChoiceList.CalcSpecialWshedFields
        chkbxDinf.Checked = tdbChoiceList.AddDinfLayer
        chkbxAreaDinf.Checked = tdbChoiceList.AddDinfAreaLayer
        chkbxUseDinf.Checked = tdbChoiceList.useDinf
        chkbxDinf.Enabled = chkbxUseDinf.Checked
        chkbxAreaDinf.Enabled = chkbxUseDinf.Checked
        chkbxCheckEdge.Checked = tdbChoiceList.EdgeContCheck
        txtbxOut.Text = tdbChoiceList.OutputPath
        chkbxMergedShed.Checked = tdbChoiceList.AddMergedWShedShapeLayer
        chkbxCalcMergeShed.Checked = tdbChoiceList.calcSpecialMergeWshedFields
    End Sub

    Private Sub frmAdvancedOptions_v2_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        tdbChoiceList.AddPitfillLayer = chkbxFillPits.Checked
        tdbChoiceList.AddD8Layer = chkbxD8.Checked
        tdbChoiceList.AddD8AreaLayer = chkbxD8Area.Checked
        tdbChoiceList.AddGridNetLayer = chkbxGridNet.Checked
        tdbChoiceList.AddRiverRasterLayer = chkbxFullNet.Checked
        tdbChoiceList.AddOrderGridLayer = chkbxStreamOrder.Checked
        tdbChoiceList.AddWShedGridLayer = chkbxShedGrid.Checked
        tdbChoiceList.AddStreamShapeLayer = chkBxStreamShape.Checked
        tdbChoiceList.AddWShedShapeLayer = chkbxShedToShape.Checked
        tdbChoiceList.CalcSpecialStreamFields = chkbxStreamFields.Checked
        tdbChoiceList.CalcSpecialWshedFields = chkbxWshedFields.Checked
        tdbChoiceList.AddDinfLayer = chkbxDinf.Checked
        tdbChoiceList.AddDinfAreaLayer = chkbxAreaDinf.Checked
        tdbChoiceList.useDinf = chkbxUseDinf.Checked
        tdbChoiceList.EdgeContCheck = chkbxCheckEdge.Checked
        tdbChoiceList.OutputPath = txtbxOut.Text
        tdbChoiceList.AddMergedWShedShapeLayer = chkbxMergedShed.Checked
        tdbChoiceList.calcSpecialMergeWshedFields = chkbxCalcMergeShed.Checked
    End Sub

    Private Sub chkbxSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxSelectAll.CheckedChanged
        chkbxFillPits.Checked = chkbxSelectAll.Checked
        chkbxD8.Checked = chkbxSelectAll.Checked
        chkbxD8Area.Checked = chkbxSelectAll.Checked
        chkbxGridNet.Checked = chkbxSelectAll.Checked
        chkbxFullNet.Checked = chkbxSelectAll.Checked
        chkbxStreamOrder.Checked = chkbxSelectAll.Checked
        chkbxShedGrid.Checked = chkbxSelectAll.Checked
        chkBxStreamShape.Checked = chkbxSelectAll.Checked
        chkbxShedToShape.Checked = chkbxSelectAll.Checked
        chkbxDinf.Checked = chkbxSelectAll.Checked
        chkbxAreaDinf.Checked = chkbxSelectAll.Checked
        chkbxMergedShed.Checked = chkbxSelectAll.Checked
    End Sub

    Private Sub chkbxUseDinf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxUseDinf.CheckedChanged
        chkbxDinf.Enabled = chkbxUseDinf.Checked
        chkbxAreaDinf.Enabled = chkbxUseDinf.Checked
    End Sub

    Private Sub btnBrowseOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOut.Click
        Dim myOpenFolder As New System.Windows.Forms.FolderBrowserDialog

        myOpenFolder.SelectedPath = tdbFileList.getAbsolutePath(txtbxOut.Text, tdbFileList.dem)

        If myOpenFolder.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxOut.Text = myOpenFolder.SelectedPath
            txtbxOut.Text = tdbFileList.GetRelativePath(txtbxOut.Text, tdbFileList.dem)
        End If
    End Sub

End Class