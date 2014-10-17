Public Class frmSpecifySplit

    Private WithEvents pTimseriesGroup As atcTimeseriesGroup
    Private pSeasonsAvailable As atcDataAttributes
    Private pOk As Boolean

    Public Function AskUser(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal aSeasonsAvailable As atcDataAttributes, ByVal aNewDatasets As atcTimeseriesGroup) As Boolean
        pTimseriesGroup = aTimeseriesGroup
        pSeasonsAvailable = aSeasonsAvailable
        Clear()
        Me.ShowDialog()
        If pOk Then
            'For Each lts As atcTimeseries In pTimseriesGroup
            '    aNewDatasets.AddRange(pSeasons.Split(lts, Me))
            'Next

            'Dim lAttributesToCalculate As New atcDataAttributes
            'For Each lAttrName As String In lstAttributes.SelectedItems
            '    lAttributesToCalculate.SetValue(lAttrName, Nothing)
            'Next
            'CalculateAttributes(lAttributesToCalculate, True)
        End If
        Return pOk
    End Function

    Private Sub Clear()
        pOk = False
        cboSeasons.Items.Clear()
        lstSeasons.Items.Clear()
        'lstAttributes.Items.Clear()
        For Each lSeason As atcDefinedValue In pSeasonsAvailable
            cboSeasons.Items.Add(lSeason.Definition.Name.Substring(0, lSeason.Definition.Name.IndexOf("::")))
        Next
        'For Each lDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions()
        '    If lDef.Calculated AndAlso atcDataAttributes.IsSimple(lDef) Then
        '        lstAttributes.Items.Add(lDef.Name)
        '    End If
        'Next
        'LoadListSelected(lstAttributes)
        cboSeasons.SelectedItem = GetSetting("atcSeasons", "SeasonType", "combobox", "")
        frmSpecifySeasonalAttributes.LoadListSelectedSeasons(lstSeasons)
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        lstSeasons.Items.Clear()
        Dim lSeasonSource As atcTimeseriesSource = CurrentSeason()
        'If Not lSeasonSource Is Nothing AndAlso lstAttributes.SelectedItems.Count > 0 Then
        '    Dim lArguments As New atcDataAttributes
        '    Dim lAttributes As New atcDataAttributes
        '    lAttributes.SetValue(lstAttributes.SelectedItems(0), 0)
        '    lArguments.Add("Attributes", lAttributes)

        '    For Each lSeasonalAttribute As atcDefinedValue In CalculateAttributes(lAttributes, False)
        '        Dim lSeasonName As String = lSeasonalAttribute.Arguments.GetValue("SeasonName") 'Definition.Name
        '        If lSeasonName IsNot Nothing AndAlso Not lstSeasons.Items.Contains(lSeasonName) Then
        '            lstSeasons.Items.Add(lSeasonName)
        '            lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
        '        End If
        '    Next
        'End If
    End Sub

    Private Function CurrentSeason() As atcTimeseriesSource
        For Each lSeason As atcDefinedValue In pSeasonsAvailable
            If lSeason.Definition.Name.Equals(cboSeasons.Text & "::SeasonalAttributes") Then
                Return lSeason.Definition.Calculator
            End If
        Next
        Return Nothing
    End Function

    Private Function DoSplit() As atcTimeseriesGroup
        'Dim lSeasonSource As atcTimeseriesSource = CurrentSeason()
        'If lSeasonSource IsNot Nothing Then
        '    Dim lArguments As New atcDataAttributes
        '    For Each lTimeseries As atcTimeseries In pTimseriesGroup
        '        lArguments.SetValue("Timeseries", lTimeseries)
        '        If lSeasonSource.Open(cboSeasons.Text & "::SeasonalAttributes", lArguments) AndAlso aSetInTimeseries Then
        '            For Each lAtt As atcDefinedValue In lCalculatedAttributes 'Seasonal Attribute
        '                Dim lSeasonName As String = lAtt.Arguments.GetValue("SeasonName")
        '                If lstSeasons.SelectedItems.Contains(lSeasonName) Then 'This season is selected, set the attribute
        '                    lTimeseries.Attributes.SetValue(lAtt.Definition, lAtt.Value, lAtt.Arguments)
        '                    lAllCalculatedAttributes.SetValue(lAtt.Definition, lAtt.Value, lAtt.Arguments)
        '                End If
        '            Next
        '            lCalculatedAttributes.Clear()
        '        End If
        '    Next
        'End If
        'Return lAllCalculatedAttributes

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
End Class