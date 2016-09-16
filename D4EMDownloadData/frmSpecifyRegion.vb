Public Class frmSpecifyRegion

    Private Const pAppName = "BASINS Data Download"

    Public Shared Function AskUser(ByVal aIcon As Drawing.Icon) As atcD4EMLauncher.Region
        Dim lForm As New frmSpecifyRegion
        With lForm
            .Icon = aIcon
            .txtTop.Text = GetSetting(pAppName, "Defaults", "BoxTop", .txtTop.Text)
            .txtBottom.Text = GetSetting(pAppName, "Defaults", "BoxBottom", .txtBottom.Text)
            .txtLeft.Text = GetSetting(pAppName, "Defaults", "BoxLeft", .txtLeft.Text)
            .txtRight.Text = GetSetting(pAppName, "Defaults", "BoxRight", .txtRight.Text)
            .txtRegionProjection.Text = GetSetting(pAppName, "Defaults", "BoxProjection", .txtRegionProjection.Text)

            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim lRegion As New atcD4EMLauncher.Region(.txtTop.Text, .txtBottom.Text, .txtLeft.Text, .txtRight.Text, .txtRegionProjection.Text)
                Try
                    lRegion.Validate()
                    SaveSetting(pAppName, "Defaults", "BoxTop", .txtTop.Text)
                    SaveSetting(pAppName, "Defaults", "BoxBottom", .txtBottom.Text)
                    SaveSetting(pAppName, "Defaults", "BoxLeft", .txtLeft.Text)
                    SaveSetting(pAppName, "Defaults", "BoxRight", .txtRight.Text)
                    SaveSetting(pAppName, "Defaults", "BoxProjection", .txtRegionProjection.Text)
                    Return lRegion
                Catch ex As Exception
                    MapWinUtility.Logger.Msg(ex.Message, "Could not set region")
                End Try
            End If
        End With
        Return Nothing
    End Function
End Class