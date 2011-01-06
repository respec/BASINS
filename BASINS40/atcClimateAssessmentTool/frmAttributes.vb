Imports MapWinUtility

Public Class frmAttributes

    Public Function AskUser(ByRef aAttributes() As String) As Boolean
        lstAttributes.Items.Clear()
        lstAttributes.Items.AddRange(aAttributes)
        If Me.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim lLastIndex As Integer = lstAttributes.Items.Count - 1
            ReDim aAttributes(lLastIndex)
            For lIndex As Integer = 0 To lLastIndex
                aAttributes(lIndex) = lstAttributes.Items(lIndex)
            Next
            SaveSetting("BasinsCAT", "Settings", "Attributes", String.Join(vbLf, aAttributes))
            Return True
        End If
        Return False
    End Function

    Private Sub btnNdayAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNdayAdd.Click
        If Not IsNumeric(txtNday.Text.Trim) Then
            Logger.Msg("Non-numeric number of days '" & txtNday.Text & "'")
        ElseIf Not IsNumeric(txtReturnPeriod.Text.Trim) Then
            Logger.Msg("Non-numeric Return Period '" & txtReturnPeriod.Text & "'")
        Else
            Dim lNumDays As Integer = CInt(txtNday.Text.Trim)
            Dim lArgName As String = txtNday.Text.Trim
            If radioHigh.Checked Then
                lArgName &= "High"
            Else
                lArgName &= "Low"
            End If
            lArgName &= txtReturnPeriod.Text.Trim
            If Not lstAttributes.Items.Contains(lArgName) Then

                'insert in alphanumeric order by first integer (number of days)
                'TODO: also sort by return period when we have multiple of the same nday
                For lIndex As Integer = 0 To lstAttributes.Items.Count - 1
                    If atcUtility.StrFirstInt(lstAttributes.Items(lIndex)) > lNumDays Then
                        lstAttributes.Items.Insert(lIndex, lArgName)
                        Return
                    End If
                Next
                lstAttributes.Items.Add(lArgName)
            End If
        End If
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        With lstAttributes
            Dim lRemoveThese As New Generic.List(Of Integer)
            Dim lIndex As Integer
            For lIndex = .SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(.SelectedIndices.Item(lIndex))
            Next

            For Each lIndex In lRemoveThese
                .Items.RemoveAt(lIndex)
            Next
        End With
    End Sub

    Private Sub btnDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefaults.Click
        lstAttributes.Items.Clear()
        For Each lAttribute As atcData.atcAttributeDefinition In atcData.atcDataAttributes.AllDefinitions
            If lAttribute.TypeString.ToLower.Equals("double") AndAlso atcData.atcDataAttributes.IsSimple(lAttribute) Then
                lstAttributes.Items.Add(lAttribute.Name)
            End If
        Next
    End Sub

    Private Sub btnPercentileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPercentileAdd.Click
        If Not IsNumeric(txtPercentile.Text.Trim) Then
            Logger.Msg("Non-numeric Percentile '" & txtPercentile.Text & "'")
        Else
            Dim lPercentile As Integer = CInt(txtPercentile.Text.Trim)
            Dim lArgName As String = "%" & Format(lPercentile, "00")
            If Not lstAttributes.Items.Contains(lArgName) Then
                'insert in alphanumeric order by Percentile
                For lIndex As Integer = 0 To lstAttributes.Items.Count - 1
                    Dim lExistingAttributeName As String = lstAttributes.Items(lIndex)
                    If lExistingAttributeName.StartsWith("%") Then
                        lExistingAttributeName = lExistingAttributeName.Substring(1)
                    End If
                    If atcUtility.StrFirstInt(lExistingAttributeName) > lPercentile Then
                        lstAttributes.Items.Insert(lIndex, lArgName)
                        Return
                    End If
                Next
                lstAttributes.Items.Add(lArgName)
            End If
        End If
    End Sub
End Class