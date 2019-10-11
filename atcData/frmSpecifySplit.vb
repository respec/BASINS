Public Class frmSpecifySplit

    Private WithEvents pTimseriesGroup As atcTimeseriesGroup
    Private pAllDates As atcTimeseries
    Private pSeasonsAvailable As atcDataAttributes

    Public Function AskUser(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aSeasonsAvailable As atcDataAttributes, ByVal aNewDatasets As atcTimeseriesGroup) As Boolean
        pTimseriesGroup = aTimeseriesGroup
        pSeasonsAvailable = aSeasonsAvailable

        Clear()
        Me.ShowDialog()
        If Me.DialogResult = System.Windows.Forms.DialogResult.OK Then
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
            Try 'Try saving first so DeleteSetting will not throw an exception
                SaveSetting("atcSeasons", "Split" & cboSeasons.Text, "X", "X")
                DeleteSetting("atcSeasons", "Split" & cboSeasons.Text)
            Catch
            End Try
            Dim lSelectedSeasons As System.Windows.Forms.ListBox.SelectedObjectCollection = lstSeasons.SelectedItems
            If lSelectedSeasons.Count < lstSeasons.Items.Count Then 'Only save selection if fewer than all were selected
                For Each lSeason As Object In lSelectedSeasons
                    SaveSetting("atcSeasons", "Split" & cboSeasons.Text, lSeason.ToString, "True")
                Next
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
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        SaveSetting("atcSeasons", "SeasonType", "Split", cboSeasons.Text)
        Dim lType As Type = CurrentSeason().Definition.DefaultValue
        Dim lSeasonType As atcSeasonBase = lType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
        lstSeasons.Items.Clear()
        Dim lSeasonName As String
        Dim lNumSelected As Integer = 0

        For Each lSeasonIndex As Integer In lSeasonType.AllSeasonsInDates(pAllDates.Values)
            lSeasonName = lSeasonType.SeasonName(lSeasonIndex)
            lstSeasons.Items.Add(lSeasonName)
            If GetSetting("atcSeasons", "Split" & cboSeasons.Text, lSeasonName) = "True" Then
                lNumSelected += 1
                lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
            End If
        Next

        If lNumSelected = 0 Then 'If none were selected in saved settings, default to selecting all
            For lSeasonIndex As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
                lstSeasons.SetSelected(lSeasonIndex, True)
            Next
        End If
    End Sub

    Private Function CurrentSeason() As atcDefinedValue
        For Each lSeason As atcDefinedValue In pSeasonsAvailable
            If lSeason.Definition.Name.StartsWith(cboSeasons.Text & "::") Then
                Return lSeason
            End If
        Next
        Return Nothing
    End Function

    Private Function DoSplit(ByVal aSeasonType As atcDefinedValue,
                             ByVal aSeasonsSelected As System.Windows.Forms.ListBox.SelectedObjectCollection,
                             ByVal aTimseriesGroup As atcTimeseriesGroup,
                             ByVal aCombineAllSelected As Boolean,
                             ByVal aEachSelected As Boolean,
                             ByVal aGroupEveryN As Integer,
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
                                    Dim lMergedTS As atcTimeseries = MergeTimeseries(lGroup)
                                    lMergedTS.Attributes.SetValue("SeasonName", lGroupSeasonName)
                                    lMergedTS.Attributes.SetValue("SeasonDefinition", lSplitTS.Attributes.GetValue("SeasonDefinition"))
                                    aNewDatasets.Add(lMergedTS)
                                    lGroup.Clear()

                            End Select
                        End If
                    Next
                Next
                If lGroup.Count > 0 Then
                    If lGroup.Count > 1 Then
                        lGroupSeasonName &= " - " & lGroup(lGroup.Count - 1).Attributes.GetValue("SeasonName")
                    End If
                    Dim lMergedTS As atcTimeseries = MergeTimeseries(lGroup)
                    lMergedTS.Attributes.SetValue("SeasonName", lGroupSeasonName)
                    lMergedTS.Attributes.SetValue("SeasonDefinition", lGroup(0).Attributes.GetValue("SeasonDefinition"))
                    aNewDatasets.Add(lMergedTS)
                    lGroup.Clear()
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

    Private Sub btnSplit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSplit.Click
        If lstSeasons.SelectedIndices.Count = 0 Then
            MapWinUtility.Logger.Msg("At least one season must be selected.", MsgBoxStyle.OkOnly, "No seasons selected")
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub
End Class