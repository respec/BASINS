Public Class frmSpecifySplit

    Private WithEvents pTimseriesGroup As atcTimeseriesGroup
    Private pAllDates As atcTimeseries
    Private pSeasonsAvailable As atcDataAttributes

    Public Function AskUser(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aSeasonsAvailable As atcDataAttributes, ByVal aNewDatasets As atcTimeseriesGroup) As Boolean
        pTimseriesGroup = aTimeseriesGroup
        pSeasonsAvailable = aSeasonsAvailable

        Clear()
        Me.ShowDialog()
        If Me.DialogResult = Windows.Forms.DialogResult.OK Then
            If radioSeasonsCombine.Checked Then
                SaveSetting("atcSeasons", "SplitType", "Split", "Combine")
            End If
            If radioSeasonsSeparate.Checked Then
                SaveSetting("atcSeasons", "SplitType", "Split", "Separate")
            End If
            Dim lNumToGroup As Integer = 0
            If radioSeasonsGroup.Checked Then
                Integer.TryParse(txtGroupSeasons.Text, lNumToGroup)
                If lNumToGroup < 1 Then
                    Throw New ApplicationException("Grouping selected, but number to group not specified")
                End If
                SaveSetting("atcSeasons", "SplitType", "Split", txtGroupSeasons.Text)
            End If
            DoSplit(CurrentSeason, lstSeasons.SelectedItems, pTimseriesGroup, radioSeasonsCombine.Checked, radioSeasonsSeparate.Checked, lNumToGroup, aNewDatasets)
            Return True
        End If
        Return False
    End Function

    Private Sub Clear()
        cboSeasons.Items.Clear()
        lstSeasons.Items.Clear()
        For Each lSeason As atcDefinedValue In pSeasonsAvailable
            cboSeasons.Items.Add(lSeason.Definition.Name.Substring(0, lSeason.Definition.Name.IndexOf("::")))
        Next
        pAllDates = MergeDates(pTimseriesGroup)

        cboSeasons.SelectedItem = GetSetting("atcSeasons", "SeasonType", "Split", "Month")
        Dim lTypeSetting As String = GetSetting("atcSeasons", "SplitType", "Split", "Separate")
        Select Case lTypeSetting
            Case "Combine"
                radioSeasonsCombine.Checked = True
                radioSeasonsSeparate.Checked = False
                radioSeasonsGroup.Checked = False
            Case "Separate"
                radioSeasonsCombine.Checked = False
                radioSeasonsSeparate.Checked = True
                radioSeasonsGroup.Checked = False
            Case Else
                If IsNumeric(lTypeSetting) Then
                    txtGroupSeasons.Text = lTypeSetting
                    radioSeasonsCombine.Checked = False
                    radioSeasonsSeparate.Checked = False
                    radioSeasonsGroup.Checked = True
                End If
        End Select
        frmSpecifySeasonalAttributes.LoadListSelectedSeasons(lstSeasons)
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        SaveSetting("atcSeasons", "SeasonType", "Split", cboSeasons.SelectedItem)
        Dim lType As Type = CurrentSeason().Definition.DefaultValue
        Dim lSeasonType As atcSeasonBase = lType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
        lstSeasons.Items.Clear()

        For Each lSeasonIndex As Integer In lSeasonType.AllSeasonsInDates(pAllDates.Values)
            lstSeasons.Items.Add(lSeasonType.SeasonName(lSeasonIndex))
            lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
        Next

        'Dim lSeasonSource As atcTimeseriesSource = CurrentSeason().Definition.Calculator
        'If lSeasonSource IsNot Nothing Then
        '    Dim lArguments As New atcDataAttributes
        '    Dim lAttributes As New atcDataAttributes
        '    lAttributes.SetValue("Data Source", 0)
        '    lArguments.Add("Attributes", lAttributes)

        '    'Figure out which seasons exist in pTimeseriesGroup. Especially important for calendar or water year seasons that have no native names and are only named by the years in the data.
        '    For Each lSeasonalAttribute As atcDefinedValue In frmSpecifySeasonalAttributes.CalculateAttributes(cboSeasons.Text, lSeasonSource, lstSeasons.SelectedItems, pTimseriesGroup, lAttributes, False)
        '        Dim lSeasonName As String = lSeasonalAttribute.Arguments.GetValue("SeasonName") 'Definition.Name
        '        If lSeasonName IsNot Nothing AndAlso Not lstSeasons.Items.Contains(lSeasonName) Then
        '            lstSeasons.Items.Add(lSeasonName)
        '            lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
        '        End If
        '    Next
        'End If
    End Sub

    Private Function CurrentSeason() As atcDefinedValue
        For Each lSeason As atcDefinedValue In pSeasonsAvailable
            If lSeason.Definition.Name.StartsWith(cboSeasons.Text & "::") Then
                Return lSeason
            End If
        Next
        Return Nothing
    End Function

    Private Function DoSplit(ByVal aSeasonType As atcDefinedValue, _
                             ByVal aSeasonsSelected As Windows.Forms.ListBox.SelectedObjectCollection, _
                             ByVal aTimseriesGroup As atcTimeseriesGroup, _
                             ByVal aCombineAllSelected As Boolean, _
                             ByVal aEachSelected As Boolean, _
                             ByVal aGroupEveryN As Integer, _
                             ByVal aNewDatasets As atcTimeseriesGroup) As atcTimeseriesGroup
        If aSeasonType IsNot Nothing Then
            Dim lType As Type = aSeasonType.Definition.DefaultValue
            Dim lSeasonType As atcSeasonBase = lType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
            Dim lSeasonIndexes() As Integer = lSeasonType.AllSeasonsInDates(pAllDates.Values)
            Dim lSeasonIndexesSelected As New Generic.List(Of Integer)
            For Each lSeasonIndex As Integer In lSeasonIndexes
                If lstSeasons.SelectedItems.Contains(lSeasonType.SeasonName(lSeasonIndex)) Then
                    lSeasonType.SeasonSelected(lSeasonIndex) = True
                    lSeasonIndexesSelected.Add(lSeasonIndex)
                Else
                    lSeasonType.SeasonSelected(lSeasonIndex) = False
                End If
            Next

            If aCombineAllSelected Then
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    Dim lSplitTS As atcTimeseriesGroup = lSeasonType.SplitBySelected(lTimeseries, Nothing)
                    aNewDatasets.Add(lSplitTS(0))
                Next
            End If
            If aEachSelected Then
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    For Each lSplitTS As atcTimeseries In lSeasonType.Split(lTimeseries, Nothing)
                        If lSeasonIndexesSelected.Contains(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
                            aNewDatasets.Add(lSplitTS)
                        End If
                    Next
                Next
            End If
            If aGroupEveryN > 0 Then
                Dim lGroup As New atcTimeseriesGroup
                Dim lGroupSeasonName As String = ""
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    For Each lSplitTS As atcTimeseries In lSeasonType.Split(lTimeseries, Nothing)
                        If lSeasonIndexesSelected.Contains(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
                            lGroup.Add(lSplitTS)
                            Select Case lGroup.Count
                                Case 1
                                    lGroupSeasonName = lSplitTS.Attributes.GetValue("SeasonName")
                                Case aGroupEveryN
                                    lGroupSeasonName &= " - " & lSplitTS.Attributes.GetValue("SeasonName")
                                    lSplitTS = MergeTimeseries(lGroup)
                                    lSplitTS.Attributes.SetValue("SeasonName", lGroupSeasonName)
                                    aNewDatasets.Add(lSplitTS)
                                    lGroup.Clear()

                            End Select
                        End If
                    Next
                Next
                If lGroup.Count > 0 Then
                    aNewDatasets.Add(MergeTimeseries(lGroup))
                End If
            End If
        End If
        Return aNewDatasets
    End Function


    Private Sub btnSeasonsAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsAll.Click
        For index As Integer = 0 To lstSeasons.Items.Count - 1
            lstSeasons.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnSeasonsNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsNone.Click
        For index As Integer = 0 To lstSeasons.Items.Count - 1
            lstSeasons.SetSelected(index, False)
        Next
    End Sub

    'Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    '    pOk = True
    '    SaveSetting("atcSeasons", "SeasonType", "combobox", cboSeasons.SelectedItem)
    '    frmSpecifySeasonalAttributes.SaveListSelectedSeasons(lstSeasons)
    '    Close()
    'End Sub

    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    Close()
    'End Sub

    Private Sub pTimseriesGroup_Added(ByVal aAddedOrRemoved As atcUtility.atcCollection) Handles pTimseriesGroup.Added, pTimseriesGroup.Removed
        Clear()
    End Sub

End Class