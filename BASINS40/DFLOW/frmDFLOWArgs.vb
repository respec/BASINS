Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Web
Imports System.Windows.Forms

Public Class frmDFLOWArgs

    Private pDateFormat As New atcDateFormat
    Private Shared pLastDayOfMonth() As Integer = {99, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    Public Event ParametersSet(ByVal aArgs As atcDataAttributes)
    Private pSetGlobalParams As Boolean
    Private pSetBatchParams As Boolean = False

    Private pAttributes As atcDataAttributes

    Public Sub Initialize(ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup, _
                          ByVal attributes As atcDataAttributes)
        pAttributes = attributes
        DFLOWCalcs.Initialize(attributes)
        pSetBatchParams = True
        gbTextOutput.Visible = True
        If aTimeseriesGroup Is Nothing Then
            pSetGlobalParams = True
        Else
            pSetGlobalParams = False
        End If
        Populate(aTimeseriesGroup)
        Dim lMessage As String = ""
        If attributes IsNot Nothing Then
            With attributes
                fFirstYear = .GetValue(InputNames.StartYear, -1)
                fStartMonth = .GetValue(InputNames.StartMonth, 4)
                fStartDay = .GetValue(InputNames.StartDay, 1)

                fLastYear = .GetValue(InputNames.EndYear, -1)
                fEndMonth = .GetValue(InputNames.EndMonth, 3)
                fEndDay = .GetValue(InputNames.EndDay, 31)

                cboStartMonth.SelectedIndex = fStartMonth - 1
                cboEndMonth.SelectedIndex = fEndMonth - 1

                tbStartDay.Text = fStartDay
                tbEndDay.Text = fEndDay

                txtOutputDir.Text = .GetValue(InputNames.OutputDir, "")
                txtOutputRootName.Text = .GetValue(InputNames.OutputPrefix, "")

                If fFirstYear > 0 Then tbOmitBeforeYear.Text = fFirstYear
                If fLastYear > 0 Then tbOmitAfterYear.Text = fLastYear

                If aTimeseriesGroup IsNot Nothing Then
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
                End If
            End With
            If pSetGlobalParams Then
                Me.Text = "DFLOW Inputs -Set Global Parameters"
            Else
                Me.Text = "DFLOW Inputs-Set Group Parameters (" & pAttributes.GetValue("Group", "") & ")"
            End If

            lMessage = SaveBatchSettings(attributes, aTimeseriesGroup)
        End If
        If String.IsNullOrEmpty(lMessage) Then
            If pSetGlobalParams Then
                Logger.Msg("Global DFLOW Parameters are set.", "Batch Run DFLOW")
            Else
                Logger.Msg("DFLOW Parameters are set for " & pAttributes.GetValue("Group", "") & ".", "Batch Run DFLOW")
            End If
            Me.Close()
            RaiseEvent ParametersSet(pAttributes)
        Else
            Logger.Msg("Please address the following issues before proceed." & vbCrLf & lMessage, "Batch Run DFLOW")
            lMessage = SaveBatchSettings(attributes, aTimeseriesGroup)
        End If
    End Sub

    Private Function SaveBatchSettings(ByVal aChoice As atcDataAttributes, Optional ByVal aDataGroup As atcTimeseriesGroup = Nothing) As String
        Dim lMessage As String = ""
        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'aCBList = Me.clbDataSets
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
        Else
        End If
        With aChoice
            .SetValue(InputNames.StartYear, fFirstYear)
            .SetValue(InputNames.EndYear, fLastYear)
            .SetValue(InputNames.StartMonth, fStartMonth)
            .SetValue(InputNames.StartDay, fStartDay)
            .SetValue(InputNames.EndMonth, fEndMonth)
            .SetValue(InputNames.EndDay, fEndDay)
            Dim loutputdir As String = txtOutputDir.Text
            If String.IsNullOrEmpty(loutputdir) OrElse Not IO.Directory.Exists(loutputdir) Then
                If pSetGlobalParams Then
                    lMessage &= "- Need to specify a global default output directory." & vbCrLf
                End If
            Else
                .SetValue(InputNames.OutputDir, loutputdir)
            End If
            .SetValue(InputNames.OutputPrefix, txtOutputRootName.Text)
            If Not pSetGlobalParams AndAlso aDataGroup IsNot Nothing Then
                Dim lStationsInfo As atcCollection = InputNames.BuildStationsInfo(aDataGroup)
                .SetValue(InputNames.StationsInfo, lStationsInfo)
            End If
        End With
        Return lMessage
    End Function

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

        RemoveHandler tbBio1.TextChanged, AddressOf tbBio1_TextChanged
        RemoveHandler tbBio2.TextChanged, AddressOf tbBio2_TextChanged
        RemoveHandler tbBio3.TextChanged, AddressOf tbBio3_TextChanged
        RemoveHandler tbBio4.TextChanged, AddressOf tbBio4_TextChanged

        RemoveHandler tbNonBio1.TextChanged, AddressOf tbNonBio1_TextChanged
        RemoveHandler tbNonBio2.TextChanged, AddressOf tbNonBio2_TextChanged
        RemoveHandler tbNonBio3.TextChanged, AddressOf tbNonBio3_TextChanged
        RemoveHandler tbNonBio4.TextChanged, AddressOf tbNonBio4_TextChanged


        If fBioFPArray(lBioIdx, 0) < 0 Then
            If pSetBatchParams Then
                tbBio1.Text = fBioPeriod.ToString()
            Else
                tbBio1.Text = ""
            End If
        Else
            If pSetBatchParams Then
                tbBio1.Text = fBioPeriod.ToString()
            Else
                tbBio1.Text = fBioFPArray(lBioIdx, 0)
            End If
        End If

        If fBioFPArray(lBioIdx, 1) < 0 Then
            If pSetBatchParams Then
                tbBio2.Text = fBioYears.ToString()
            Else
                tbBio2.Text = ""
            End If
        Else
            If pSetBatchParams Then
                tbBio2.Text = fBioYears.ToString()
            Else
                tbBio2.Text = fBioFPArray(lBioIdx, 1)
            End If
        End If

        If fBioFPArray(lBioIdx, 2) < 0 Then
            If pSetBatchParams Then
                tbBio3.Text = fBioExcursions.ToString()
            Else
                tbBio3.Text = ""
            End If
        Else
            If pSetBatchParams Then
                tbBio3.Text = fBioExcursions.ToString()
            Else
                tbBio3.Text = fBioFPArray(lBioIdx, 2)
            End If
        End If

        If fBioFPArray(lBioIdx, 3) < 0 Then
            If pSetBatchParams Then
                tbBio4.Text = fBioCluster.ToString()
            Else
                tbBio4.Text = ""
            End If
        Else
            If pSetBatchParams Then
                tbBio4.Text = fBioCluster.ToString()
            Else
                tbBio4.Text = fBioFPArray(lBioIdx, 3)
            End If
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

        AddHandler tbBio1.TextChanged, AddressOf tbBio1_TextChanged
        AddHandler tbBio2.TextChanged, AddressOf tbBio2_TextChanged
        AddHandler tbBio3.TextChanged, AddressOf tbBio3_TextChanged
        AddHandler tbBio4.TextChanged, AddressOf tbBio4_TextChanged

        AddHandler tbNonBio1.TextChanged, AddressOf tbNonBio1_TextChanged
        AddHandler tbNonBio2.TextChanged, AddressOf tbNonBio2_TextChanged
        AddHandler tbNonBio3.TextChanged, AddressOf tbNonBio3_TextChanged
        AddHandler tbNonBio4.TextChanged, AddressOf tbNonBio4_TextChanged

        clbDataSets.Items.Clear()

        Dim lDateFormat As atcDateFormat
        lDateFormat = New atcDateFormat
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        If aTimeseriesGroup IsNot Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            Dim lDataSet As atcDataSet
            For Each lDataSet In aTimeseriesGroup
                Dim lString As String
                lString = lDataSet.Attributes.GetFormattedValue("Location") & " (" & _
                          lDateFormat.JDateToString(lDataSet.Attributes.GetValue("start date")) & " - " & _
                          lDateFormat.JDateToString(lDataSet.Attributes.GetValue("end date")) & ")"

                clbDataSets.Items.Add(lString, True)
            Next
        End If
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

        If pSetBatchParams AndAlso pAttributes IsNot Nothing Then
            If fBioDefault Then
                Dim lParams() As Integer = Nothing
                If rbBio1.Checked Then
                    lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Acute_maximum_concentration.ToString, Nothing)
                ElseIf rbBio2.Checked Then
                    lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString, Nothing)
                ElseIf rbBio3.Checked Then
                    lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Ammonia.ToString, Nothing)
                End If
                If lParams IsNot Nothing Then
                    lParams(0) = fBioPeriod
                    lParams(1) = fBioYears
                    lParams(2) = fBioCluster
                    lParams(3) = fBioExcursions
                End If
            End If
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
        If pSetBatchParams AndAlso pAttributes IsNot Nothing Then
            Dim lParams() As Integer = Nothing
            If rbBio1.Checked Then
                lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Acute_maximum_concentration.ToString())
                pAttributes.SetValue(InputNames.BioSelectedMethod, InputNames.EBioDFlowType.Acute_maximum_concentration)
            ElseIf rbBio2.Checked Then
                lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString())
                pAttributes.SetValue(InputNames.BioSelectedMethod, InputNames.EBioDFlowType.Chronic_continuous_concentration)
            ElseIf rbBio3.Checked Then
                lParams = pAttributes.GetValue(InputNames.EBioDFlowType.Ammonia.ToString())
                pAttributes.SetValue(InputNames.BioSelectedMethod, InputNames.EBioDFlowType.Ammonia)
            End If
            If lParams IsNot Nothing Then
                tbBio1.Text = lParams(0).ToString()
                tbBio2.Text = lParams(1).ToString()
                tbBio3.Text = lParams(2).ToString()
                tbBio4.Text = lParams(3).ToString()
            End If
        Else
            tbBio1.Text = fBioFPArray(fBioType, 0)
            tbBio2.Text = fBioFPArray(fBioType, 1)
            tbBio3.Text = fBioFPArray(fBioType, 2)
            tbBio4.Text = fBioFPArray(fBioType, 3)
        End If
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

        If pSetBatchParams AndAlso pAttributes IsNot Nothing Then
            If rbNonBio1.Checked Then
                pAttributes.SetValue(InputNames.NBioSelectedMethod, InputNames.EDFlowType.Hydrological)
                'Dim lParams() As Integer = pAttributes.GetValue(InputNames.EDFlowType.Hydrological.ToString(), Nothing)
                'If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                '    tbNonBio1.Text = lParams(0).ToString()
                '    tbNonBio2.Text = lParams(1).ToString()
                'End If
            ElseIf rbNonBio2.Checked Then
                pAttributes.SetValue(InputNames.NBioSelectedMethod, InputNames.EDFlowType.Explicit_Flow_Value)
                'Dim lflowval As Double = pAttributes.GetValue(InputNames.EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                'tbNonBio3.Text = lflowval.ToString()
            ElseIf rbNonBio3.Checked Then
                pAttributes.SetValue(InputNames.NBioSelectedMethod, InputNames.EDFlowType.Flow_Percentile)
                'Dim lflowPct As Double = pAttributes.GetValue(InputNames.EDFlowType.Flow_Percentile.ToString(), 0.1)
                'tbNonBio4.Text = lflowPct.ToString()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub AtcGrid1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Function GetSavedParams(Optional ByVal aBio As Boolean = True, Optional ByVal aValue As Double = -99) As Object
        If pAttributes IsNot Nothing Then
            If aBio Then
                If rbBio1.Checked Then
                    Return pAttributes.GetValue(InputNames.EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                ElseIf rbBio2.Checked Then
                    Return pAttributes.GetValue(InputNames.EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                ElseIf rbBio3.Checked Then
                    Return pAttributes.GetValue(InputNames.EBioDFlowType.Ammonia.ToString(), Nothing)
                End If
            Else
                If rbNonBio1.Checked Then
                    Return pAttributes.GetValue(InputNames.EDFlowType.Hydrological.ToString(), Nothing)
                ElseIf rbNonBio2.Checked Then
                    If aValue > 0 Then
                        pAttributes.SetValue(InputNames.EDFlowType.Explicit_Flow_Value.ToString(), aValue)
                    End If
                    Return pAttributes.GetValue(InputNames.EDFlowType.Explicit_Flow_Value.ToString(), Nothing)
                ElseIf rbNonBio3.Checked Then
                    If aValue > 0 Then
                        pAttributes.SetValue(InputNames.EDFlowType.Flow_Percentile.ToString(), aValue)
                    End If
                    Return pAttributes.GetValue(InputNames.EDFlowType.Flow_Percentile.ToString(), Nothing)
                End If
            End If
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

    Private Sub tbNonBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio1.TextChanged
        If IsNumeric(tbNonBio1.Text) AndAlso Integer.TryParse(tbNonBio1.Text, fAveragingPeriod) Then
            Dim lParams() As Integer = GetSavedParams(False)
            If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                lParams(0) = fAveragingPeriod
            End If
        End If
    End Sub

    Private Sub tbNonBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio2.TextChanged
        If IsNumeric(tbNonBio2.Text) AndAlso Integer.TryParse(tbNonBio2.Text, fReturnPeriod) Then
            Dim lParams() As Integer = GetSavedParams(False)
            If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                lParams(1) = fReturnPeriod
            End If
        End If
    End Sub

    Private Sub tbNonBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio3.TextChanged
        If IsNumeric(tbNonBio3.Text) AndAlso Double.TryParse(tbNonBio3.Text, fExplicitFlow) AndAlso fExplicitFlow > 0 Then
            Dim lNewValue As Double = GetSavedParams(False, fExplicitFlow)
        End If
    End Sub

    Private Sub tbNonBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbNonBio4.TextChanged
        If IsNumeric(tbNonBio4.Text) AndAlso Double.TryParse(tbNonBio4.Text, fPercentile) AndAlso fPercentile > 0 Then
            Dim lNewValue As Double = GetSavedParams(False, fPercentile)
        End If
    End Sub

    Private Sub tbBio4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio4.TextChanged
        If IsNumeric(tbBio4.Text) AndAlso Integer.TryParse(tbBio4.Text, fBioFPArray(3, 3)) Then
            'fBioFPArray(3, 3) = tbBio4.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(InputNames.EBioDFlowParamIndex.P3AverageExcursionsPerCluster) = Integer.Parse(tbBio4.Text)
                pAttributes.SetValue(InputNames.BioUseDefault, False)
            End If
        End If
    End Sub

    Private Sub tbBio3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio3.TextChanged
        If IsNumeric(tbBio3.Text) AndAlso Integer.TryParse(tbBio3.Text, fBioFPArray(3, 2)) Then
            'fBioFPArray(3, 2) = tbBio3.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(InputNames.EBioDFlowParamIndex.P2ExcursionClusterPeriodDays) = Integer.Parse(tbBio3.Text)
                pAttributes.SetValue(InputNames.BioUseDefault, False)
            End If
        End If
    End Sub

    Private Sub tbBio2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio2.TextChanged
        If IsNumeric(tbBio2.Text) AndAlso Integer.TryParse(tbBio2.Text, fBioFPArray(3, 1)) Then
            'fBioFPArray(3, 1) = tbBio2.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(InputNames.EBioDFlowParamIndex.P1AverageYearsBetweenExcursions) = Integer.Parse(tbBio2.Text)
                pAttributes.SetValue(InputNames.BioUseDefault, False)
            End If
        End If
    End Sub

    Private Sub tbBio1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBio1.TextChanged
        If IsNumeric(tbBio1.Text) AndAlso Integer.TryParse(tbBio1.Text, fBioFPArray(3, 0)) Then
            'fBioFPArray(3, 0) = tbBio1.Text
            Dim lParams() As Integer = GetSavedParams()
            If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                lParams(InputNames.EBioDFlowParamIndex.P0FlowAveragingPeriodDays) = Integer.Parse(tbBio1.Text)
                pAttributes.SetValue(InputNames.BioUseDefault, False)
            End If
        End If
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

    Private Sub txtOutputDir_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputDir.TextChanged

    End Sub
End Class 'Form1 

