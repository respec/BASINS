Public Class frmChooseColumns

    Private pInitialTitles() As String
    Private pInitialAttributes() As String
    Private pAvailableAttributes() As String = {"Max", "Min", "Sum", "Cumulative", "Mean", "Geometric Mean", "Variance", "Standard Deviation", "Skew"}
    Private pLists() As Windows.Forms.CheckedListBox
    Private pCheckboxes() As Windows.Forms.CheckBox

    Public Sub AskUser(ByRef aTitles() As String, ByRef aAttributes() As String)
        pInitialTitles = aTitles.Clone
        pInitialAttributes = aAttributes.Clone

        'Initializing these outside a routine is too early, and can't use {} to assign, just in declaration
        Dim lLists() As Windows.Forms.CheckedListBox = {lstVolume, lstDuration, lstIntensity, lstTimeSinceLast}
        pLists = lLists
        Dim lCheckboxes() As Windows.Forms.CheckBox = {chkEvents, chkMeasurements, chkStartDate, chkStartTime}
        pCheckboxes = lCheckboxes

        For Each lAttributeName As String In aAttributes
            If lAttributeName.Length > 0 AndAlso Array.IndexOf(pAvailableAttributes, lAttributeName) < 0 Then
                ReDim Preserve pAvailableAttributes(pAvailableAttributes.GetUpperBound(0) + 1)
                pAvailableAttributes(pAvailableAttributes.GetUpperBound(0)) = lAttributeName
            End If
        Next

        PopulateForm(aTitles, aAttributes)

        If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
            GetSelected(aTitles, aAttributes)
        End If
    End Sub

    Private Sub PopulateForm(ByVal aTitles() As String, ByVal aAttributes() As String)
        For Each lCheckbox As Windows.Forms.CheckBox In pCheckboxes
            If array.IndexOf(aTitles, lCheckbox.Text) >= 0 Then
                lCheckbox.Checked = True
            End If
        Next
        For Each lList As Windows.Forms.CheckedListBox In pLists
            SetListAttributes(lList, aTitles, aAttributes)
        Next
    End Sub

    Private Sub SetListAttributes(ByVal aList As System.Windows.Forms.CheckedListBox, ByVal aTitles() As String, ByVal aAttributes() As String)
        aList.Items.Clear()
        aList.Items.AddRange(pAvailableAttributes)

        'Select appropriate list items
        For lTitleIndex As Integer = 0 To aTitles.GetUpperBound(0)
            If aTitles(lTitleIndex) = aList.Tag Then
                aList.SetItemChecked(aList.Items.IndexOf(aAttributes(lTitleIndex)), True)
            End If
        Next
    End Sub

    Private Sub GetSelected(ByRef aTitles() As String, ByRef aAttributes() As String)
        Dim lNumSelected As Integer = 0
        Dim lCheckbox As Windows.Forms.CheckBox
        Dim lList As Windows.Forms.CheckedListBox

        For Each lCheckbox In pCheckboxes
            If lCheckbox.Checked Then lNumSelected += 1
        Next

        For Each lList In pLists
            lNumSelected += lList.CheckedIndices.Count
        Next

        ReDim aTitles(lNumSelected)
        ReDim aAttributes(lNumSelected)

        aTitles(0) = "Group"
        aAttributes(0) = ""
        Dim lIndex As Integer = 1

        For Each lCheckbox In pCheckboxes
            If lCheckbox.Checked Then
                aTitles(lIndex) = lCheckbox.Text
                aAttributes(lIndex) = ""
                lIndex += 1
            End If
        Next

        For Each lList In pLists
            For Each lAttribute As String In lList.CheckedItems
                aTitles(lIndex) = lList.Tag
                aAttributes(lIndex) = lAttribute
                lIndex += 1
            Next
        Next
    End Sub

    Private Sub btnRevert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevert.Click
        PopulateForm(pInitialTitles, pInitialAttributes)
    End Sub

    Private Sub btnAddAttribute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAttribute.Click
        'TODO
    End Sub

    Private Sub SelectAll(ByVal aList As System.Windows.Forms.CheckedListBox, ByVal aSelect As Boolean)
        'Select items in aSelected
        For lCurrentIndex As Integer = 0 To aList.Items.Count - 1
            aList.SetItemChecked(lCurrentIndex, aSelect)
        Next
    End Sub

    Private Sub btnAllVolume_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllVolume.Click
        SelectAll(lstVolume, True)
    End Sub

    Private Sub btnNoneVolume_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneVolume.Click
        SelectAll(lstVolume, False)
    End Sub


    Private Sub btnAllDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllDuration.Click
        SelectAll(lstDuration, True)
    End Sub

    Private Sub btnNoneDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneDuration.Click
        SelectAll(lstDuration, False)
    End Sub

    Private Sub btnAllIntensity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllIntensity.Click
        SelectAll(lstIntensity, True)
    End Sub

    Private Sub btnNoneIntensity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneIntensity.Click
        SelectAll(lstIntensity, False)
    End Sub

    Private Sub btnAllTimeSinceLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllTimeSinceLast.Click
        SelectAll(lstTimeSinceLast, True)
    End Sub

    Private Sub btnNoneTimeSinceLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneTimeSinceLast.Click
        SelectAll(lstTimeSinceLast, False)
    End Sub
End Class