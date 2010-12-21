Imports atcData
Imports atcUtility
Imports System.Web
Imports System.Windows.Forms



Public Class frmDFLOWArgs

    Private pDateFormat As New atcDateFormat
    Private Shared pLastDayOfMonth() As Integer = {99, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'Done - Force calculation

        fBioPeriod = fBioFPArray(fBioType, 0)
        fBioYears = fBioFPArray(fBioType, 1)
        fBioCluster = fBioFPArray(fBioType, 2)
        fBioExcursions = fBioFPArray(fBioType, 3)

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Function AskUser(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByRef aCBList As CheckedListBox) As Boolean

        DFLOWCalcs.Initialize()
        Populate(aTimeseriesGroup)

        ' ----- More population of fields based on atcTimeSeriesNdayHighLow

        cboStartMonth.SelectedIndex = fStartMonth - 1
        cboEndMonth.SelectedIndex = fEndMonth - 1

        tbStartDay.Text = fStartDay
        tbEndDay.Text = fEndDay

        If fFirstYear > 0 Then tbOmitBeforeYear.Text = fFirstYear
        If fLastYear > 0 Then tbOmitAfterYear.Text = fLastYear

        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()
        For Each lDataset As atcData.atcTimeseries In aTimeseriesGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(1)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
            End If
        Next

        If lFirstDate < GetMaxValue() Then
            'Dim lDate As Date = Date.FromOADate(lFirstDate)
            'txtOmitBeforeYear.Text = lDate.Year
            lblDataStart.Text = lblDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate)
        End If
        If lLastDate > GetMinValue() Then
            'Dim lDate As Date = Date.FromOADate(lLastDate)
            'txtOmitAfterYear.Text = lDate.Year
            lblDataEnd.Text = lblDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
        End If

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then

            aCBList = Me.clbDataSets
            fStartMonth = cboStartMonth.SelectedIndex + 1
            If IsNumeric(tbStartDay.Text) Then
                fStartDay = tbStartDay.Text
            Else
                fStartDay = 1
            End If
            fEndMonth = cboEndMonth.SelectedIndex + 1
            If IsNumeric(tbEndDay.Text) Then
                fEndDay = tbEndDay.Text
            Else
                fEndDay = pLastDayOfMonth(fEndMonth)
            End If
            If IsNumeric(tbOmitBeforeYear.Text) Then
                fFirstYear = CInt(tbOmitBeforeYear.Text)
            Else
                fFirstYear = 0
            End If
            If IsNumeric(tbOmitAfterYear.Text) Then
                fLastYear = CInt(tbOmitAfterYear.Text)
            Else
                fLastYear = 0
            End If

            Return True

        Else

            Return False

        End If

    End Function
    Private Sub SetDefaults()


    End Sub

    Private Sub Populate(ByVal aTimeseriesGroup As atcTimeseriesGroup)
        ' This populates the DFLOW input form according to the DFLOW calculation values
        ckbBio.Checked = fBioDefault


        rbBio1.Enabled = fBioDefault
        rbBio2.Enabled = fBioDefault
        rbBio3.Enabled = fBioDefault

        Select Case fBioType
            Case 0
                rbBio1.Checked = True
            Case 1
                rbBio2.Checked = True
            Case 2
                rbBio3.Checked = True
        End Select


        tbBio1.Enabled = Not fBioDefault
        tbBio2.Enabled = Not fBioDefault
        tbBio3.Enabled = Not fBioDefault

        Dim lBioIdx As Integer
        If fBioDefault Then
            lBioIdx = fBioType
        Else
            lBioIdx = 3
        End If

        If fBioFPArray(lBioIdx, 0) < 0 Then
            tbBio1.Text = ""
        Else
            tbBio1.Text = fBioFPArray(lBioIdx, 0)
        End If

        If fBioFPArray(lBioIdx, 1) < 0 Then
            tbBio2.Text = ""
        Else
            tbBio2.Text = fBioFPArray(lBioIdx, 1)
        End If

        If fBioFPArray(lBioIdx, 2) < 0 Then
            tbBio3.Text = ""
        Else
            tbBio3.Text = fBioFPArray(lBioIdx, 2)
        End If

        If fBioFPArray(lBioIdx, 3) < 0 Then
            tbBio4.Text = ""
        Else
            tbBio4.Text = fBioFPArray(lBioIdx, 3)
        End If


        Select Case fNonBioType
            Case 0
                rbNonBio1.Checked = True
            Case 1
                rbNonBio2.Checked = True
            Case 2
                rbNonBio3.Checked = True
        End Select

        tbNonBio1.Text = fAveragingPeriod
        tbNonBio2.Text = fReturnPeriod
        tbNonBio3.Text = fExplicitFlow
        tbNonBio4.Text = fPercentile

        clbDataSets.Items.Clear()

        Dim lDateFormat As atcDateFormat
        lDateFormat = New atcDateFormat
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        Dim lDataSet As atcDataSet
        For Each lDataSet In aTimeseriesGroup
            Dim lString As String
            lString = lDataSet.Attributes.GetFormattedValue("Location") & " (" & _
                      lDateFormat.JDateToString(lDataSet.Attributes.GetValue("start date")) & " - " & _
                      lDateFormat.JDateToString(lDataSet.Attributes.GetValue("end date")) & ")"

            clbDataSets.Items.Add(lString, True)
        Next

    End Sub


    Private Sub ckbBio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbBio.CheckedChanged
        Dim i As Integer

        fBioDefault = ckbBio.Checked

        rbBio1.Enabled = fBioDefault
        rbBio2.Enabled = fBioDefault
        rbBio3.Enabled = fBioDefault

        tbBio1.Enabled = Not fBioDefault
        tbBio2.Enabled = Not fBioDefault
        tbBio3.Enabled = Not fBioDefault
        tbBio4.Enabled = Not fBioDefault
        Label1.Enabled = Not fBioDefault
        Label2.Enabled = Not fBioDefault
        Label3.Enabled = Not fBioDefault
        Label4.Enabled = Not fBioDefault


        If fBioDefault Then
            tbBio1.Text = fBioFPArray(fBioType, 0)
            tbBio2.Text = fBioFPArray(fBioType, 1)
            tbBio3.Text = fBioFPArray(fBioType, 2)
            tbBio4.Text = fBioFPArray(fBioType, 3)
        Else
            For i = 0 To 3
                If fBioFPArray(3, i) < 0 Then
                    fBioFPArray(3, i) = fBioFPArray(fBioType, i)

                End If
            Next
            tbBio1.Text = fBioFPArray(3, 0)
            tbBio2.Text = fBioFPArray(3, 1)
            tbBio3.Text = fBioFPArray(3, 2)
            tbBio4.Text = fBioFPArray(3, 3)
        End If

    End Sub

    Private Sub rbBio1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBio1.CheckedChanged, rbBio3.CheckedChanged, rbBio2.CheckedChanged
        If rbBio1.Checked Then
            fBioType = 0
        ElseIf rbBio2.Checked Then
            fBioType = 1
        Else
            fBioType = 2
        End If
        tbBio1.Text = fBioFPArray(fBioType, 0)
        tbBio2.Text = fBioFPArray(fBioType, 1)
        tbBio3.Text = fBioFPArray(fBioType, 2)
        tbBio4.Text = fBioFPArray(fBioType, 3)
    End Sub
    ' Boolean flag used to determine when a character other than a number is entered.
    Private nonNumberEntered As Boolean = False


    ' Handle the KeyDown event to determine the type of character entered into the control.
    Private Sub textBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
         Handles tbBio1.KeyDown, tbBio2.KeyDown, tbBio3.KeyDown, tbBio4.KeyDown, _
                 tbNonBio1.KeyDown, tbNonBio2.KeyDown, tbNonBio4.KeyDown, _
                 tbStartDay.KeyDown, tbEndDay.KeyDown, tbOmitAfterYear.KeyDown, tbOmitBeforeYear.KeyDown
        ' Initialize the flag to false.
        nonNumberEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.
                If e.KeyCode <> Keys.Back Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    nonNumberEntered = True
                End If
            End If
        End If
    End Sub 'textBox1_KeyDown
    Private Sub textBox2_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbNonBio3.KeyDown
        ' Initialize the flag to false.
        nonNumberEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode = Keys.OemPeriod And InStr(sender.text, ".") = 1 Then
            If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
                ' Determine whether the keystroke is a number from the keypad.
                If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                    ' Determine whether the keystroke is a backspace.
                    If e.KeyCode <> Keys.Back Then
                        ' A non-numerical keystroke was pressed. 
                        ' Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = True
                    End If
                End If
            End If
        End If
    End Sub

    ' This event occurs after the KeyDown event and can be used 
    ' to prevent characters from entering the control.
    Private Sub textBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
        Handles tbBio1.KeyPress, tbBio2.KeyPress, tbBio3.KeyPress, tbBio4.KeyPress, _
                tbNonBio1.KeyPress, tbNonBio2.KeyPress, tbNonBio4.KeyPress, tbNonBio3.KeyPress, _
                tbStartDay.KeyPress, tbEndDay.KeyPress, tbOmitAfterYear.KeyPress, tbOmitBeforeYear.KeyPress

        ' Check for the flag being set in the KeyDown event.
        If nonNumberEntered = True Then
            ' Stop the character from being entered into the control since it is non-numerical.
            e.Handled = True
        End If
    End Sub 'textBox1_KeyPress


    Private Sub rbNonBio1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNonBio1.CheckedChanged, rbNonBio3.CheckedChanged, rbNonBio2.CheckedChanged

        If rbNonBio1.Checked Then
            fNonBioType = 0
        ElseIf rbNonBio2.Checked Then
            fNonBioType = 1
        Else
            fNonBioType = 2
        End If
        Label7.Enabled = rbNonBio1.Checked
        Label8.Enabled = rbNonBio1.Checked
        tbNonBio1.Enabled = rbNonBio1.Checked
        tbNonBio2.Enabled = rbNonBio1.Checked

        Label5.Enabled = rbNonBio2.Checked
        tbNonBio3.Enabled = rbNonBio2.Checked

        Label6.Enabled = rbNonBio3.Checked
        tbNonBio4.Enabled = rbNonBio3.Checked


    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub AtcGrid1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub tbNonBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio1.TextChanged
        If IsNumeric(tbNonBio1.Text) Then fAveragingPeriod = tbNonBio1.Text
    End Sub

    Private Sub tbNonBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio2.TextChanged
        If IsNumeric(tbNonBio2.Text) Then fReturnPeriod = tbNonBio2.Text
    End Sub

    Private Sub tbNonBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio3.TextChanged
        If IsNumeric(tbNonBio3.Text) Then fExplicitFlow = tbNonBio3.Text
    End Sub

    Private Sub tbNonBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio4.TextChanged
        If IsNumeric(tbNonBio4.Text) Then fPercentile = tbNonBio4.Text
    End Sub

    Private Sub tbBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio4.TextChanged
        If IsNumeric(tbBio4.Text) Then fBioFPArray(3, 3) = tbBio4.Text
    End Sub

    Private Sub tbBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio3.TextChanged
        If IsNumeric(tbBio3.Text) Then fBioFPArray(3, 2) = tbBio3.Text
    End Sub

    Private Sub tbBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio2.TextChanged
        If IsNumeric(tbBio2.Text) Then fBioFPArray(3, 1) = tbBio2.Text
    End Sub

    Private Sub tbBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio1.TextChanged
        If IsNumeric(tbBio1.Text) Then fBioFPArray(3, 0) = tbBio1.Text
    End Sub

    Private Sub cboStartMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartMonth.SelectedIndexChanged
        If fStartDay > pLastDayOfMonth(cboStartMonth.SelectedIndex + 1) Then tbStartDay.Text = pLastDayOfMonth(cboStartMonth.SelectedIndex + 1)
    End Sub

    Private Sub cboEndMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEndMonth.SelectedIndexChanged
        If fEndDay > pLastDayOfMonth(cboEndMonth.SelectedIndex + 1) Then tbEndDay.Text = pLastDayOfMonth(cboEndMonth.SelectedIndex + 1)
    End Sub

    Private Sub tbOmitBeforeYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbOmitBeforeYear.TextChanged
        If IsNumeric(tbOmitBeforeYear.Text) Then
            fFirstYear = tbOmitBeforeYear.Text
        Else
            fFirstYear = -1
        End If
        
    End Sub

    Private Sub txtOmitAfterYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbOmitAfterYear.TextChanged
        If IsNumeric(tbOmitAfterYear.Text) Then
            fLastYear = tbOmitAfterYear.Text
        Else
            fLastYear = -1
        End If

    End Sub

    Private Sub tbStartDay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbStartDay.TextChanged
        If IsNumeric(tbStartDay.Text) Then
            fStartDay = tbStartDay.Text
            If fStartDay > pLastDayOfMonth(cboStartMonth.SelectedIndex + 1) Then tbStartDay.Text = pLastDayOfMonth(cboStartMonth.SelectedIndex + 1)
        End If
    End Sub

    Private Sub tbEndDay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbEndDay.TextChanged
        If IsNumeric(tbEndDay.Text) Then
            fEndDay = tbEndDay.Text
            If fEndDay > pLastDayOfMonth(cboEndMonth.SelectedIndex + 1) Then tbEndDay.Text = pLastDayOfMonth(cboEndMonth.SelectedIndex + 1)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ShowDFLOWHelp(Replace(Application.StartupPath.ToLower, g_PathChar & "bin", g_PathChar & "docs") & g_PathChar & "dflow4.chm")
        ShowDFLOWHelp("html\dlfo6hps.htm")
    End Sub
End Class 'Form1 

