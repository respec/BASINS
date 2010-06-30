Public Class frmLoadOutputs
    Public fillPath As String
    Public sd8Path As String
    Public d8Path As String
    Public dInfPath As String
    Public dinfSlopePath As String

    Private Sub btnBrowseFill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFill.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxFill.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseD8Slope_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseD8Slope.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxD8Slope.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseD8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseD8.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxD8.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseDinfSlope_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDinfSlope.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxDinfSlope.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseDinf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDinf.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxDinf.Text = fdgOpen.FileName
        End If
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If txtbxFill.Text <> "" And IO.File.Exists(txtbxFill.Text) Then
            fillPath = txtbxFill.Text
        End If
        If txtbxD8Slope.Text <> "" And IO.File.Exists(txtbxD8Slope.Text) Then
            sd8Path = txtbxD8Slope.Text
        End If
        If txtbxD8.Text <> "" And IO.File.Exists(txtbxD8.Text) Then
            d8Path = txtbxD8.Text
        End If
        If txtbxDinfSlope.Text <> "" And IO.File.Exists(txtbxDinfSlope.Text) Then
            dinfSlopePath = txtbxDinfSlope.Text
        End If
        If txtbxDinf.Text <> "" And IO.File.Exists(txtbxDinf.Text) Then
            dInfPath = txtbxDinf.Text
        End If
        If fillPath = "" Or sd8Path = "" Or d8Path = "" Or (tdbChoiceList.useDinf And dinfSlopePath = "") Or (tdbChoiceList.useDinf And dInfPath = "") Then
            MsgBox("There must be a path selected for each of the pre-existing preprocessing intermediate grids.", MsgBoxStyle.OkOnly, "Load Intermediate Files")
            fillPath = ""
            sd8Path = ""
            d8Path = ""
            dinfSlopePath = ""
            dInfPath = ""
            txtbxFill.Text = ""
            txtbxD8Slope.Text = ""
            txtbxD8.Text = ""
            txtbxDinfSlope.Text = ""
            txtbxDinf.Text = ""
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub frmLoadOutputs_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible = True Then
            fillPath = ""
            sd8Path = ""
            d8Path = ""
            dInfPath = ""
            dinfSlopePath = ""
            If tdbChoiceList.useDinf Then
                txtbxDinfSlope.Enabled = True
                txtbxDinf.Enabled = True
                btnBrowseDinf.Enabled = True
                btnBrowseDinfSlope.Enabled = True
            Else
                txtbxDinfSlope.Enabled = False
                txtbxDinf.Enabled = False
                btnBrowseDinf.Enabled = False
                btnBrowseDinfSlope.Enabled = False
            End If
        End If
    End Sub
End Class