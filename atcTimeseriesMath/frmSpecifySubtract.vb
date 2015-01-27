Imports atcData

Public Class frmSpecifySubtract
    Private pSelectedOK As Boolean

    Private pTS1 As atcTimeseries
    Private pTS2 As atcTimeseries
    Private pNumber As Double
    Private pArgs As atcData.atcDataAttributes

    Public Function AskUser(ByRef aArgs As atcData.atcDataAttributes) As Boolean
        pSelectedOK = False

        Me.ShowDialog()

        If pSelectedOK AndAlso pArgs IsNot Nothing Then
            aArgs = pArgs
            Return True
        End If
        Return False
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            Dim lAttributes As New atcDataAttributes
            If radioTsMinusNumber.Checked Then
                lAttributes.Add("Timeseries", pTS1)
                If Not IsNumeric(txtNumber.Text) Then
                    If String.IsNullOrEmpty(txtNumber.Text) Then
                        MapWinUtility.Logger.Msg("Number is required but is not specified", Me.Text)
                    Else
                        MapWinUtility.Logger.Msg("Number specified is not recognized as a number: '" & txtNumber.Text & "'", Me.Text)
                    End If
                    Exit Sub
                End If
                lAttributes.Add("Number", CDbl(txtNumber.Text))
            ElseIf radioNumberMinusTS.Checked Then
                If Not IsNumeric(txtNumber.Text) Then
                    If String.IsNullOrEmpty(txtNumber.Text) Then
                        MapWinUtility.Logger.Msg("Number is required but is not specified", Me.Text)
                    Else
                        MapWinUtility.Logger.Msg("Number specified is not recognized as a number: '" & txtNumber.Text & "'", Me.Text)
                    End If
                    Exit Sub
                End If
                lAttributes.Add("Number", CDbl(txtNumber.Text))
                lAttributes.Add("Timeseries", pTS1)
            ElseIf radioTS1MinusTS2.Checked Then
                lAttributes.Add("Timeseries", New atcTimeseriesGroup(pTS1, pTS2))
            Else 'Should not be possible to get here
                MapWinUtility.Logger.Msg("Please select a radio button", Me.Text)
                Exit Sub
            End If
            pArgs = lAttributes
            pSelectedOK = True
            Close()
        Catch ex As Exception
            MapWinUtility.Logger.Msg("Error: " & ex.Message, Me.Text)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub radioTsMinusNumber_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioTsMinusNumber.CheckedChanged
        If radioTsMinusNumber.Checked Then
            lblTimeseries1.Text = "Time Series"
            lblTimeseries2.Visible = False
            txtTimeseries2.Visible = False
            btnSelectTimeseries2.Visible = False
            lblNumber.Top = lblTimeseries2.Top
            txtNumber.Top = txtTimeseries2.Top
            lblNumber.Visible = True
            txtNumber.Visible = True
        End If
    End Sub

    Private Sub radioTS1MinusTS2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioTS1MinusTS2.CheckedChanged
        If radioTS1MinusTS2.Checked Then
            lblTimeseries1.Text = "Time Series 1"
            lblTimeseries2.Visible = True
            txtTimeseries2.Visible = True
            btnSelectTimeseries2.Visible = True
            lblNumber.Visible = False
            txtNumber.Visible = False
        End If
    End Sub

    Private Sub radioNumberMinusTS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioNumberMinusTS.CheckedChanged
        If radioNumberMinusTS.Checked Then
            lblTimeseries1.Text = "Time Series"
            lblTimeseries2.Visible = False
            txtTimeseries2.Visible = False
            btnSelectTimeseries2.Visible = False
            lblNumber.Top = lblTimeseries1.Top - (lblTimeseries2.Top - lblTimeseries1.Top)
            txtNumber.Top = txtTimeseries1.Top - (txtTimeseries2.Top - txtTimeseries1.Top)
            lblNumber.Visible = True
            txtNumber.Visible = True
        End If
    End Sub

    Private Sub btnSelectTimeseries1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectTimeseries1.Click
        Dim lSelected As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Time Series", Nothing, Nothing, True, True, atcTimeseriesMath.pIcon)
        If lSelected Is Nothing OrElse lSelected.Count = 0 Then
            txtTimeseries1.Text = ""
        ElseIf lSelected.Count > 0 Then
            pTS1 = lSelected(0)
            txtTimeseries1.Text = pTS1.ToString
        End If
    End Sub

    Private Sub btnSelectTimeseries2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectTimeseries2.Click
        Dim lSelected As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Time Series", Nothing, Nothing, True, True, atcTimeseriesMath.pIcon)
        If lSelected Is Nothing OrElse lSelected.Count = 0 Then
            txtTimeseries2.Text = ""
        ElseIf lSelected.Count > 0 Then
            pTS2 = lSelected(0)
            txtTimeseries2.Text = pTS2.ToString
        End If
    End Sub

End Class