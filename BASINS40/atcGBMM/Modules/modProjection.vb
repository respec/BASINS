Module modProjection
    Public Function ValidateSpatialReferenceOfInputLayers() As Boolean
        Try
            Dim frm As New FrmListBox
            frm.ListBoxErrors.Items.Clear()
            Dim someErrors As Boolean = False

            For i As Integer = 0 To GisUtil.NumLayers - 1
                With gMapWin.Layers(i)
                    If Not gMapWin.GetOCX.IsSameProjection(.Projection, gMapWin.Project.ProjectProjection) Then
                        'Dim map As = gMapWin.GetOCX
                        '    If Not map.IsSameProjection(.Projection, gMapWin.Project.ProjectProjection) Then
                        frm.ListBoxErrors.Items.Add(.Name)
                        someErrors = True
                    End If
                End With
            Next

            If someErrors Then
                frm.lblMissing.Text = "All input layers are not in same Projection Systems."
                frm.ShowDialog()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function

End Module