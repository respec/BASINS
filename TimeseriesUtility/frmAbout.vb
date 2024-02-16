Public Class frmAbout

    Private Sub UpdateLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles UpdateLink.LinkClicked
        atcUtility.OpenFile("https://www.respec.com/product/modeling-optimization/sara-timeseries-utility/")
    End Sub
End Class