Public Class frmSpecifyStations

    Private Const pAppName = "BASINS Data Download"

    'Public Shared Function AskUser(ByVal aIcon As Drawing.Icon, ByVal aStations As Generic.List(Of String)) As D4EMDataManager.Region
    '    Dim lForm As New frmSpecifyStations
    '    With lForm
    '        .Icon = aIcon
    '        If aStations IsNot Nothing AndAlso aStations.Count > 0 Then
    '            For Each lStation As String In aStations
    '                .txtStations.Text &= lStation & vbCrLf
    '            Next
    '        Else
    '            .txtStations.Text = GetSetting(pAppName, "Defaults", "SpecifyStations", .txtStations.Text)
    '        End If

    '        If .ShowDialog() = Windows.Forms.DialogResult.OK Then
    '            Dim lRegion As New D4EMDataManager.RegionStations(lForm.StationsList)
    '            Try
    '                lRegion.Validate()
    '                SaveSetting(pAppName, "Defaults", "SpecifyStations", .txtStations.Text)
    '                Return lRegion
    '            Catch ex As Exception
    '                MapWinUtility.Logger.Msg(ex.Message, "Could not set region")
    '            End Try
    '        End If
    '    End With
    '    Return Nothing
    'End Function

    'Private Function StationsList() As Generic.List(Of String)
    '    Dim lStations As New Generic.List(Of String)
    '    lStations.AddRange(txtStations.Text.Replace(vbCr, vbLf).Replace(vbLf & vbLf, vbLf).Split(vbLf))
    '    Return lStations
    'End Function

    Public Shared Function AskUser(ByVal aIcon As Drawing.Icon, ByVal aStations As Generic.List(Of String)) As Generic.List(Of String)
        Dim lForm As New frmSpecifyStations
        With lForm
            .Icon = aIcon
            If aStations IsNot Nothing AndAlso aStations.Count > 0 Then
                For Each lStation As String In aStations
                    .txtStations.Text &= lStation & vbCrLf
                Next
            Else
                .txtStations.Text = GetSetting(pAppName, "Defaults", "SpecifyStations", .txtStations.Text)
            End If

            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim lStations As New Generic.List(Of String)
                lStations.AddRange(lForm.txtStations.Text.Replace(vbCr, vbLf).Replace(vbLf & vbLf, vbLf).Split(vbLf))

                SaveSetting(pAppName, "Defaults", "SpecifyStations", .txtStations.Text)
                Return lStations
            End If
        End With
        Return Nothing
    End Function

End Class