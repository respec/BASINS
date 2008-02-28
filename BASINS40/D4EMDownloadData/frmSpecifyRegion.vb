Public Class frmSpecifyRegion

    Private Const pAppName = "BASINS Data Download"

    Public Function AskUser() As D4EMDataManager.Region
        txtTop.Text = GetSetting(pAppName, "Defaults", "BoxTop", txtTop.Text)
        txtBottom.Text = GetSetting(pAppName, "Defaults", "BoxBottom", txtBottom.Text)
        txtLeft.Text = GetSetting(pAppName, "Defaults", "BoxLeft", txtLeft.Text)
        txtRight.Text = GetSetting(pAppName, "Defaults", "BoxRight", txtRight.Text)
        txtRegionProjection.Text = GetSetting(pAppName, "Defaults", "BoxProjection", txtRegionProjection.Text)

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim lRegion As New D4EMDataManager.Region(txtTop.Text, txtBottom.Text, txtLeft.Text, txtRight.Text, txtRegionProjection.Text)
            Try
                lRegion.Validate()
                SaveSetting(pAppName, "Defaults", "BoxTop", txtTop.Text)
                SaveSetting(pAppName, "Defaults", "BoxBottom", txtBottom.Text)
                SaveSetting(pAppName, "Defaults", "BoxLeft", txtLeft.Text)
                SaveSetting(pAppName, "Defaults", "BoxRight", txtRight.Text)
                SaveSetting(pAppName, "Defaults", "BoxProjection", txtRegionProjection.Text)
                Return lRegion
            Catch ex As Exception
                MapWinUtility.Logger.Msg(ex.Message, "Could not set region")
            End Try
        End If
        Return Nothing
    End Function
End Class