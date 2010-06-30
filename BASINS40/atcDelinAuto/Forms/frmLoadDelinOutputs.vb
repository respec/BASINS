Public Class frmLoadDelinOutputs
    Public ad8Path As String
    Public scaPath As String
    Public gordPath As String
    Public plenPath As String
    Public tlenPath As String
    Public srcPath As String
    Public ordPath As String
    Public coordPath As String
    Public treePath As String
    Public netPath As String
    Public wPath As String

    Private Sub frmLoadDelinOutputs_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible = True Then
            ad8Path = ""
            scaPath = ""
            gordPath = ""
            plenPath = ""
            tlenPath = ""
            srcPath = ""
            ordPath = ""
            coordPath = ""
            treePath = ""
            netPath = ""
            wPath = ""

            If tdbChoiceList.useDinf Then
                txtbxSca.Enabled = True
                btnBrowseSca.Enabled = True
            Else
                txtbxSca.Enabled = False
                btnBrowseSca.Enabled = False
            End If
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If txtbxAd8.Text <> "" And IO.File.Exists(txtbxAd8.Text) Then
            ad8Path = txtbxAd8.Text
        End If
        If txtbxSca.Text <> "" And IO.File.Exists(txtbxSca.Text) Then
            scaPath = txtbxSca.Text
        End If
        If txtbxGord.Text <> "" And IO.File.Exists(txtbxGord.Text) Then
            gordPath = txtbxGord.Text
        End If
        If txtbxPlen.Text <> "" And IO.File.Exists(txtbxPlen.Text) Then
            plenPath = txtbxPlen.Text
        End If
        If txtbxTlen.Text <> "" And IO.File.Exists(txtbxTlen.Text) Then
            tlenPath = txtbxTlen.Text
        End If
        If txtbxSrc.Text <> "" And IO.File.Exists(txtbxSrc.Text) Then
            srcPath = txtbxSrc.Text
        End If
        If txtbxOrd.Text <> "" And IO.File.Exists(txtbxOrd.Text) Then
            ordPath = txtbxOrd.Text
        End If
        If txtbxCoord.Text <> "" And IO.File.Exists(txtbxCoord.Text) Then
            coordPath = txtbxCoord.Text
        End If
        If txtbxTree.Text <> "" And IO.File.Exists(txtbxTree.Text) Then
            treePath = txtbxTree.Text
        End If
        If txtbxNet.Text <> "" And IO.File.Exists(txtbxNet.Text) Then
            netPath = txtbxNet.Text
        End If
        If txtbxW.Text <> "" And IO.File.Exists(txtbxW.Text) Then
            wPath = txtbxW.Text
        End If

        If ad8Path = "" Or (tdbChoiceList.useDinf And scaPath = "") Or gordPath = "" Or plenPath = "" Or tlenPath = "" Or srcPath = "" Or ordPath = "" Or coordPath = "" Or treePath = "" Or netPath = "" Or wPath = "" Then
            MsgBox("There must be a path selected for each of the pre-existing threshold delineation intermediate grids.", MsgBoxStyle.OkOnly, "Load Intermediate Files")
            ad8Path = ""
            scaPath = ""
            gordPath = ""
            plenPath = ""
            tlenPath = ""
            srcPath = ""
            ordPath = ""
            coordPath = ""
            treePath = ""
            netPath = ""
            wPath = ""
            txtbxAd8.Text = ""
            txtbxSca.Text = ""
            txtbxGord.Text = ""
            txtbxPlen.Text = ""
            txtbxTlen.Text = ""
            txtbxSrc.Text = ""
            txtbxOrd.Text = ""
            txtbxCoord.Text = ""
            txtbxTree.Text = ""
            txtbxNet.Text = ""
            txtbxW.Text = ""
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

    Private Sub btnBrowseAd8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseAd8.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxAd8.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseSca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSca.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxSca.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseGord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseGord.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxGord.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowsePlen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePlen.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxPlen.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseTlen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseTlen.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxTlen.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseSrc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSrc.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxSrc.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseOrd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOrd.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxOrd.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseCoord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCoord.Click
        fdgOpen.Filter = "Data File|*.dat"
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxCoord.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseTree.Click
        fdgOpen.Filter = "Data File|*.dat"
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxTree.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseNet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseNet.Click
        Dim shp As New MapWinGIS.Shapefile
        fdgOpen.Filter = shp.CdlgFilter
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxNet.Text = fdgOpen.FileName
        End If
    End Sub

    Private Sub btnBrowseW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseW.Click
        Dim g As New MapWinGIS.Grid
        fdgOpen.Filter = g.CdlgFilter()
        fdgOpen.FilterIndex = 1
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtbxW.Text = fdgOpen.FileName
        End If
    End Sub
End Class