Imports atcUtility
Imports MapWinUtility

Public Class frmSpecifyYearsSeasons

    Private Shared pLastDayOfMonth() As Integer = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

    Private pName As String = "Unnamed"
    Private pDateFormat As New atcDateFormat

    Public Function AskUser(ByVal aName As String, _
                            ByVal aDataGroup As atcData.atcDataGroup, _
                            ByRef aStartMonth As Integer, _
                            ByRef aStartDay As Integer, _
                            ByRef aEndMonth As Integer, _
                            ByRef aEndDay As Integer, _
                            ByRef aFirstYear As Integer, _
                            ByRef aLastYear As Integer, _
                   Optional ByRef aNDays As Integer = -999) As Boolean

        pName = aName

        If aNDays = -999 Then
            lblNDays.Visible = False
            txtNDays.Visible = False
        Else
            lblNDays.Visible = True
            txtNDays.Visible = True
            txtNDays.Text = aNDays
        End If

        txtStartDay.Text = aStartDay
        cboStartMonth.SelectedIndex = aStartMonth - 1

        txtEndDay.Text = aEndDay
        cboEndMonth.SelectedIndex = aEndMonth - 1

        If aFirstYear <> 0 Then txtOmitBeforeYear.Text = aFirstYear
        If aLastYear <> 0 Then txtOmitAfterYear.Text = aLastYear

        Dim lFirstDate As Double = Double.MaxValue
        Dim lLastDate As Double = Double.MinValue
        For Each lDataset As atcData.atcTimeseries In aDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
            End If
        Next
        If lFirstDate < Double.MaxValue Then
            'Dim lDate As Date = Date.FromOADate(lFirstDate)
            'txtOmitBeforeYear.Text = lDate.Year
            lblDataStart.Text = lblDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
        End If
        If lLastDate > Double.MinValue Then
            'Dim lDate As Date = Date.FromOADate(lLastDate)
            'txtOmitAfterYear.Text = lDate.Year
            lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
        End If

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            aStartMonth = cboStartMonth.SelectedIndex + 1
            If IsNumeric(txtStartDay.Text) Then
                aStartDay = txtStartDay.Text
            Else
                aStartDay = 1
            End If
            aEndMonth = cboEndMonth.SelectedIndex + 1
            If IsNumeric(txtEndDay.Text) Then
                aEndDay = txtEndDay.Text
            Else
                aEndDay = pLastDayOfMonth(aEndMonth)
            End If
            If IsNumeric(txtOmitBeforeYear.Text) Then
                aFirstYear = CInt(txtOmitBeforeYear.Text)
            Else
                aFirstYear = 0
            End If
            If IsNumeric(txtOmitAfterYear.Text) Then
                aLastYear = CInt(txtOmitAfterYear.Text)
            Else
                aLastYear = 0
            End If
            If IsNumeric(txtNDays.Text) Then aNDays = CInt(txtNDays.Text)
            Return True
        Else
            Return False
        End If
    End Function

    'Private Sub cboYearMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartMonth.SelectedIndexChanged
    '    Dim lDate As Integer = pLastDayOfMonth(cboStartMonth.SelectedIndex + 1)
    '    If lDate > 0 Then txtStartDay.Text = lDate
    'End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtNDays.Visible Then
            If Not IsNumeric(txtNDays.Text) Then
                Logger.Msg("Enter a number for the number of days")
                Exit Sub
            Else
                Dim lNDays As Integer = CInt(txtNDays.Text)
                If lNDays < 1 Or lNDays > 366 Then
                    Logger.Msg("Enter a number between 1 and 365 for the number of days")
                    Exit Sub
                End If
            End If
        End If
        If IsInvalidDay(txtStartDay.Text, lblYearStart.Text) Then Exit Sub
        If IsInvalidDay(txtEndDay.Text, lblYearEnd.Text) Then Exit Sub
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Function IsInvalidDay(ByVal aDay As String, ByVal aLabel As String) As Boolean
        If Not IsNumeric(aDay) Then
            Logger.Msg("Enter a number for the day", aLabel)
            Return True
        End If
        Dim lDay As Integer = CInt(aDay)
        If lDay < 1 Or lDay > 31 Then
            Logger.Msg("Enter a number between 1 and 31 for the day", aLabel)
            Return True
        End If
        Return False
    End Function

    Private Sub txtOmitBeforeYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOmitBeforeYear.LostFocus
        Try
            If Not IsNumeric(txtOmitAfterYear.Text) OrElse CInt(txtOmitBeforeYear.Text) > CInt(txtOmitAfterYear.Text) Then
                txtOmitAfterYear.Text = txtOmitBeforeYear.Text
            End If
        Catch ex As Exception
            'ignore if non-numeric
        End Try
    End Sub

End Class