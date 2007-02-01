Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmSynoptic

    Private pDataManager As atcDataManager

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcDataGroup

    Private WithEvents pEvents As atcDataGroup

    Private pInitialized As Boolean = False
    Private pLastThreshold As Double = Double.NaN
    Private pLastDaysGapAllowed As Double = Double.NaN
    Private pLastHighEvents As Boolean = True

    Private pSource As atcGridSource
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Private pGapUnitNames() As String = {"Seconds", "Minutes", "Hours", "Days", "Weeks", "Months", "Years"}
    Private pGapUnitFactor() As String = {JulianSecond, JulianMinute, JulianHour, 1, 7, 31, 366}

    Private pGroupByNames() As String = {"Each Event", "Number of Measurements", "Maximum Intensity", "Mean Intensity", "Total Volume", "Month", "Year", "One Group"} ', "Season", "Year", "Length", }

    Private pColumnTitles() As String
    Private pColumnTitlesAll() As String   'Column titles to use for all but "Each Event" grouping
    Private pColumnTitlesEvent() As String 'Column titles in use for "Each Event" grouping

    Private pColumnTitlesAllDefault() As String = {"Group", "Events", "Measurements", "Maximum Volume", "Mean Volume", "Total Volume", "Cummulative Volume", "Maximum Duration", "Mean Duration", "Total Duration", "SD Duration", "Maximum Intensity", "Mean Intensity", "Mean Time Since Last"}
    Private pColumnTitlesEventDefault() As String = {"Group", "Start Date", "Start Time", "Measurements", "Total Volume", "Total Duration", "Maximum Intensity", "Mean Intensity", "Mean Time Since Last"}

    'Private pMeasurementsGroupEdges() As Double = {100, 50, 20, 10, 5, 2, 1}
    Private pMeasurementsGroupEdges() As Double = {100, 75, 50, 40, 30, 20, 15, _
                                                    10, 7.5, 5, 4, 3, 2, 1.5, _
                                                    1}
    'Private pVolumeGroupEdges() As Double = {10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01, 0}
    Private pVolumeGroupEdges() As Double = {10, 7.5, 5, 4, 3, 2, 1.5, _
                                              1, 0.75, 0.5, 0.4, 0.3, 0.2, 0.15, _
                                              0.1, 0.075, 0.05, 0.04, 0.03, 0.02, 0.015, _
                                              0.01, 0}
    'Private pMaximumGroupEdges() As Double = {10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01, 0}
    Private pMaximumGroupEdges() As Double = {10, 7.5, 5, 4, 3, 2, 1.5, _
                                              1, 0.75, 0.5, 0.4, 0.3, 0.2, 0.15, _
                                              0.1, 0.075, 0.05, 0.04, 0.03, 0.02, 0.015, _
                                              0.01, 0}

    Public Sub Initialize(ByVal aDataManager As atcData.atcDataManager, _
                 Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
        pDataManager = aDataManager
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcDataGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataManager.UserSelectData("Select Data for Synoptic Analysis", pDataGroup, True)
        End If

        If pDataGroup.Count > 0 Then
            Me.Text &= " of " & pDataGroup.ItemByIndex(0).ToString
            Me.Show()
            cboGapUnits.Items.AddRange(pGapUnitNames)
            cboGroupBy.Items.AddRange(pGroupByNames)

            Dim lTitleString As String
            lTitleString = GetSetting("Synoptic", "Defaults", "Columns")
            If lTitleString.Length > 0 Then
                pColumnTitlesAll = lTitleString.Split(",")
            Else
                pColumnTitlesAll = pColumnTitlesAllDefault.Clone
            End If

            lTitleString = GetSetting("Synoptic", "Defaults", "ColumnsEvent")
            If lTitleString.Length > 0 Then
                pColumnTitlesEvent = lTitleString.Split(",")
            Else
                pColumnTitlesEvent = pColumnTitlesEventDefault.Clone
            End If

            cboGapUnits.SelectedIndex = GetSetting("Synoptic", "Defaults", "GapUnits", 3)
            cboGroupBy.SelectedIndex = GetSetting("Synoptic", "Defaults", "GroupBy", 0)
            txtThreshold.Text = GetSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
            If GetSetting("Synoptic", "Defaults", "High", 0) Then
                cboAboveBelow.SelectedIndex = 0
            Else
                cboAboveBelow.SelectedIndex = 1
            End If
            txtGap.Text = GetSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)

            SetColumnTitlesFromGroupBy()
            pInitialized = True
            PopulateGrid()
        Else 'user declined to specify timeseries
            Me.Close()
        End If
    End Sub

    Private Sub SaveSettings()
        If pInitialized Then
            SaveSetting("Synoptic", "Defaults", "GapUnits", cboGapUnits.SelectedIndex)
            SaveSetting("Synoptic", "Defaults", "GroupBy", cboGroupBy.SelectedIndex)
            SaveSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
            SaveSetting("Synoptic", "Defaults", "High", (cboAboveBelow.SelectedIndex = 0))
            SaveSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)
            SaveSetting("Synoptic", "Defaults", "Columns", String.Join(",", pColumnTitlesAll))
            SaveSetting("Synoptic", "Defaults", "ColumnsEvent", String.Join(",", pColumnTitlesEvent))
        End If
    End Sub

    Private Function DataSetDuration(ByVal aTimeseries As atcTimeseries) As Double
        If Not Double.IsNaN(aTimeseries.Dates.Value(0)) Then
            Return aTimeseries.Dates.Value(aTimeseries.numValues) - aTimeseries.Dates.Value(0)
        Else
            Return aTimeseries.Dates.Value(aTimeseries.numValues) + aTimeseries.Dates.Value(2) - 2 * aTimeseries.Dates.Value(1)
        End If
    End Function

    Private Sub PopulateGrid()
        Dim lGroups As New atcCollection
        Dim lGroup As atcDataGroup
        Dim lDataset As atcTimeseries
        Dim lMetaDataset As atcTimeseries
        Dim lColumn As Integer
        Dim lDate As Date
        Dim lValue As Double
        Dim lTemp As Double
        Dim lGroupIndex As Integer = 0
        Dim lValueIndex As Integer
        Dim lDurationFactor As Double = pGapUnitFactor(cboGapUnits.SelectedIndex)

        If Not pInitialized Then Exit Sub 'too early to do this!

        If pEvents Is Nothing Then ComputeEventsFromFormParameters(False)

        Select Case cboGroupBy.Text
            Case "Each Event"
                For Each lEvent As atcTimeseries In pEvents
                    lGroupIndex += 1
                    lGroups.Add(lGroupIndex, New atcDataGroup(lEvent))
                Next
            Case "Number of Measurements"
                Dim lIndex As Integer
                For Each lValue In pMeasurementsGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    lValue = lEvent.numValues
                    For lIndex = 0 To pMeasurementsGroupEdges.GetUpperBound(0)
                        If lValue > pMeasurementsGroupEdges(lIndex) Then
                            lGroups.ItemByIndex(lIndex).Add(lEvent)
                            Exit For 'Only add to group with highest bound that fits
                        End If
                    Next
                Next
            Case "Month"
                For lGroupIndex = 1 To 12
                    lGroups.Add(MonthName(lGroupIndex, True), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    'Find peak if event spans a month boundary
                    Dim lPeakIndex As Integer = 1
                    If Date.FromOADate(lEvent.Dates.Value(lPeakIndex)).Month <> _
                       Date.FromOADate(lEvent.Dates.Value(lEvent.numValues)).Month Then
                        For lValueIndex = 2 To lEvent.numValues
                            If lEvent.Value(lValueIndex) > lEvent.Value(lPeakIndex) Then
                                lPeakIndex = lValueIndex
                            End If
                        Next
                    End If
                    Dim lMonth As Integer = Date.FromOADate(lEvent.Dates.Value(lPeakIndex)).Month
                    lGroups.ItemByIndex(lMonth - 1).Add(lEvent)
                Next
            Case "Season"
                'TODO
            Case "Year"
                Dim lFirstYear As Integer = 0
                For Each lEvent As atcTimeseries In pEvents
                    'Find peak if event spans a year boundary
                    Dim lPeakIndex As Integer = 1
                    If Date.FromOADate(lEvent.Dates.Value(lPeakIndex)).Year <> _
                       Date.FromOADate(lEvent.Dates.Value(lEvent.numValues)).Year Then
                        For lValueIndex = 2 To lEvent.numValues
                            If lEvent.Value(lValueIndex) > lEvent.Value(lPeakIndex) Then
                                lPeakIndex = lValueIndex
                            End If
                        Next
                    End If

                    Dim lYear As Integer = Date.FromOADate(lEvent.Dates.Value(lPeakIndex)).Year

                    If lFirstYear = 0 Then lFirstYear = lYear

                    For lAddYear As Integer = lGroups.Count To lYear - lFirstYear
                        lGroups.Add(lYear, New atcDataGroup)
                    Next
                    lGroups.ItemByIndex(lYear - lFirstYear).Add(lEvent)
                Next
            Case "Total Volume", "Cummulative Volume"
                Dim lIndex As Integer
                For Each lValue In pVolumeGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    lValue = lEvent.Attributes.GetValue("Sum")
                    For lIndex = 0 To pVolumeGroupEdges.GetUpperBound(0)
                        If lValue > pVolumeGroupEdges(lIndex) Then
                            lGroups.ItemByIndex(lIndex).Add(lEvent)
                            Exit For 'Only add to group with highest bound that fits
                        End If
                    Next
                Next
            Case "Maximum Intensity"
                Dim lIndex As Integer
                For Each lValue In pMaximumGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    lValue = lEvent.Attributes.GetValue("Max")
                    For lIndex = 0 To pVolumeGroupEdges.GetUpperBound(0)
                        If lValue > pVolumeGroupEdges(lIndex) Then
                            lGroups.ItemByIndex(lIndex).Add(lEvent)
                            Exit For 'Only add to group with highest bound that fits
                        End If
                    Next
                Next
            Case "Mean Intensity"
                Dim lIndex As Integer
                For Each lValue In pMaximumGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    lValue = lEvent.Attributes.GetValue("Mean")
                    For lIndex = 0 To pVolumeGroupEdges.GetUpperBound(0)
                        If lValue > pVolumeGroupEdges(lIndex) Then
                            lGroups.ItemByIndex(lIndex).Add(lEvent)
                            Exit For 'Only add to group with highest bound that fits
                        End If
                    Next
                Next
            Case "Length"
            Case "One Group"
                lGroups.Add("All", pEvents)
        End Select

        'Omit empty groups from display
        For lGroupIndex = lGroups.Count - 1 To 0 Step -1
            lGroup = lGroups.ItemByIndex(lGroupIndex)
            If lGroup.Count = 0 Then lGroups.RemoveAt(lGroupIndex)
        Next

        pSource = New atcGridSource()
        pSource.Columns = pColumnTitles.Length
        pSource.FixedRows = 2
        pSource.Rows = lGroups.Count + pSource.FixedRows
        For lColumn = 0 To pColumnTitles.Length - 1
            pSource.CellValue(0, lColumn) = pColumnTitles(lColumn)
            pSource.CellValue(1, lColumn) = ColumnUnits(pColumnTitles(lColumn))
        Next

        lGroupIndex = 0
        For Each lGroup In lGroups
            For lColumn = 0 To pColumnTitles.Length - 1
                lValue = 0
                Select Case pColumnTitles(lColumn)
                    Case "Group"
                        pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = lGroups.Keys(lGroupIndex)
                    Case "Start Date"
                        lDataset = lGroup.ItemByIndex(0)
                        lDate = Date.FromOADate(lDataset.Dates.Value(1))
                        pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = lDate.ToString("yyyy-MM-dd")

                    Case "Start Time"
                        lDataset = lGroup.ItemByIndex(0)
                        lDate = Date.FromOADate(lDataset.Dates.Value(1))
                        pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = lDate.ToString("HH:mm")
                    Case "Events"
                        lValue = lGroup.Count

                    Case "Measurements"
                        For Each lDataset In lGroup
                            lValue += lDataset.numValues
                        Next
                    Case "Maximum Volume"
                        For Each lDataset In lGroup
                            If lDataset.Attributes.GetValue("Sum") > lValue Then
                                lValue = lDataset.Attributes.GetValue("Sum")
                            End If
                        Next
                    Case "Mean Volume"
                        For Each lDataset In lGroup
                            lValue += lDataset.Attributes.GetValue("Sum")
                        Next
                        lValue /= lGroup.Count
                    Case "Total Volume", "Cummulative Volume"
                        For Each lDataset In lGroup
                            lValue += lDataset.Attributes.GetValue("Sum")
                        Next
                    Case "Maximum Duration"
                        For Each lDataset In lGroup
                            lTemp = DataSetDuration(lDataset)
                            If lTemp > lValue Then
                                lValue = lTemp
                            End If
                        Next
                        lValue /= lDurationFactor
                    Case "Mean Duration"
                        For Each lDataset In lGroup
                            lValue += DataSetDuration(lDataset)
                        Next
                        lValue /= lGroup.Count
                        lValue /= lDurationFactor

                    Case "SD Duration"
                        lMetaDataset = New atcTimeseries(Nothing)
                        lMetaDataset.Dates = New atcTimeseries(Nothing)
                        lMetaDataset.numValues = lGroup.Count
                        lMetaDataset.Dates.numValues = lGroup.Count
                        lValueIndex = 1
                        For Each lDataset In lGroup
                            lMetaDataset.Value(lValueIndex) = DataSetDuration(lDataset)
                            lValueIndex += 1
                        Next
                        lValue = lMetaDataset.Attributes.GetValue("Standard Deviation")

                    Case "Total Duration"
                        For Each lDataset In lGroup
                            lValue += DataSetDuration(lDataset)
                        Next
                        lValue /= lDurationFactor
                    Case "Maximum Intensity"
                        For Each lDataset In lGroup
                            lTemp = lDataset.Attributes.GetValue("Max")
                            If lTemp > lValue Then
                                lValue = lTemp
                            End If
                        Next
                    Case "Mean Intensity"
                        For Each lDataset In lGroup
                            lValue += lDataset.Attributes.GetValue("Sum") / lDataset.numValues
                        Next
                        lValue /= lGroup.Count
                    Case "Mean Time Since Last"
                        For Each lDataset In lGroup
                            lValue += lDataset.Attributes.GetValue("EventTimeSincePrevious")
                        Next
                        lValue /= lGroup.Count
                        lValue /= lDurationFactor
                End Select

                If lValue <> 0 Then
                    Select Case pColumnTitles(lColumn)
                        Case "Group", "Start Date", "Start Time"
                            'custom code above to set cell value
                        Case "Events", "Measurements"
                            pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = CInt(lValue)
                        Case Else
                            If pColumnTitles(lColumn) = "Cummulative Volume" AndAlso lGroupIndex > 0 Then
                                Dim lValuePrevious As Double = pSource.CellValue(lGroupIndex - 1 + pSource.FixedRows, lColumn)
                                pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = DoubleToString(lValue + lValuePrevious, , , , , 5)
                            Else
                                pSource.CellValue(lGroupIndex + pSource.FixedRows, lColumn) = DoubleToString(lValue, , , , , 5)
                            End If
                    End Select
                End If
            Next

            'Duration (hours) 'TODO: dont hard code hours?
            'TODO: 1 + should be 1 time unit, not 1 hour
            '            lDuration = 1 + 24 * (lStorm.Dates.Value(lStorm.Dates.numValues) - lStorm.Dates.Value(1))
            '            lReport.Write(vbTab & StrPad(CInt(lDuration), 8))


            'Average Intensity
            '            lReport.Write(vbTab & StrPad(Format(lVolume / lDuration, "0.00"), 9))

            'Time since previous event
            'lTimeSince = lStorm.Attributes.GetValue("EventTimeSincePrevious", 0)
            'If lTimeSince > 0 Then
            '    lReport.Write(vbTab & StrPad(Format(lTimeSince * 24, "#,###"), 9))
            'End If

            lGroupIndex += 1
        Next

        pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(pSource)
        pSwapperSource.SwapRowsColumns = mnuAttributeColumns.Checked
        agdMain.Initialize(pSwapperSource)
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    Private Function GetIndex(ByVal aName As String) As Integer
        Return CInt(Mid(aName, InStr(aName, "#") + 1))
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        pDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If pInitialized Then PopulateGrid()
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If pInitialized Then PopulateGrid()
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataManager = Nothing
        pDataGroup = Nothing
        pSource = Nothing
    End Sub

    Private Sub mnuAttributeRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeRows.Click
        SwapRowsColumns = False
    End Sub

    Private Sub mnuAttributeColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeColumns.Click
        SwapRowsColumns = True
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        System.Windows.Forms.Clipboard.SetDataObject(Me.ToString)
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Analysis As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
            End If
        End With
    End Sub

    Private Sub mnuFileSaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveAll.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save All Groups As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Logger.Progress("Saving Synoptic Analysis", 0, cboGroupBy.Items.Count - 1)
                SaveFileString(.FileName, "")
                For lGroupBy As Integer = cboGroupBy.Items.Count - 1 To 0 Step -1
                    cboGroupBy.Text = cboGroupBy.Items(lGroupBy)
                    AppendFileString(.FileName, Me.ToString & vbCrLf)
                    Logger.Progress(cboGroupBy.Items.Count - lGroupBy - 1, cboGroupBy.Items.Count - 1)
                Next
            End If
        End With
    End Sub

    Private Sub mnuFileSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectAttributes.Click
        Dim lst As New atcControls.atcSelectList
        Dim lAvailable As New ArrayList
        For Each lAttrDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions
            Select Case lAttrDef.TypeString.ToLower
                Case "double", "integer", "boolean", "string"
                    lAvailable.Add(lAttrDef.Name)
            End Select
        Next
        lAvailable.Sort()
        If lst.AskUser(lAvailable, pDataManager.DisplayAttributes) Then
            PopulateGrid()
        End If
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        pDataManager.UserSelectData(, pDataGroup, False)
    End Sub

    Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    'True for groups in columns, False for groups in rows
    Public Property SwapRowsColumns() As Boolean
        Get
            Return pSwapperSource.SwapRowsColumns
        End Get
        Set(ByVal newValue As Boolean)
            If pSwapperSource.SwapRowsColumns <> newValue Then
                pSwapperSource.SwapRowsColumns = newValue
                agdMain.SizeAllColumnsToContents()
                agdMain.Refresh()
            End If
            mnuAttributeColumns.Checked = newValue
            mnuAttributeRows.Checked = Not newValue
        End Set
    End Property

    Private Function HeaderInformation() As String
        Return Me.Text & vbCrLf _
            & "Events " & cboAboveBelow.Text & " " & txtThreshold.Text & " " & lblPercentInEvents.Text & vbCrLf _
            & "Allowing gaps of up to " & txtGap.Text & " " & cboGapUnits.Text & vbCrLf _
            & "Grouped by " & cboGroupBy.Text
    End Function

    Public Overrides Function ToString() As String
        Return HeaderInformation() & vbCrLf & agdMain.ToString
    End Function

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\Synoptic.html")
    End Sub

    ''' <summary>
    ''' ComputeEventsFromFormParameters
    ''' </summary>
    ''' <param name="aCheckForChange">True to make sure event parameters have changed, False to recompute regardless of change</param>
    ''' <returns>True if events were recomputed, False if they were not</returns>
    ''' <remarks></remarks>
    Private Function ComputeEventsFromFormParameters(ByVal aCheckForChange As Boolean) As Boolean
        txtThreshold.Text = txtThreshold.Text.Trim
        If Not IsNumeric(txtThreshold.Text) Then txtThreshold.Text = "0"

        txtGap.Text = txtGap.Text.Trim
        If Not IsNumeric(txtGap.Text) Then txtGap.Text = "0"

        Dim lThreshold As Double = txtThreshold.Text
        Dim lDaysGapAllowed As Double = Double.Parse(txtGap.Text) * pGapUnitFactor(cboGapUnits.SelectedIndex) + JulianSecond / 2
        Dim lHighEvents As Boolean = (cboAboveBelow.SelectedIndex = 0)

        If Not aCheckForChange OrElse lThreshold <> pLastThreshold OrElse lDaysGapAllowed <> pLastDaysGapAllowed OrElse lHighEvents <> pLastHighEvents Then
            pEvents = atcSynopticAnalysisPlugin.ComputeEvents(pDataGroup, lThreshold, lDaysGapAllowed, lHighEvents)
            lblPercentInEvents.Text = DoubleToString(GroupTotal(pEvents) / GroupTotal(pDataGroup) * 100, , , , , 3) & "% of volume in " & pEvents.Count & " events"
            SaveSettings()
            pLastThreshold = lThreshold
            pLastDaysGapAllowed = lDaysGapAllowed
            pLastHighEvents = lHighEvents
            Return True
        End If
        Return False
    End Function

    Private Sub cboGroupBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroupBy.SelectedIndexChanged
        SetColumnTitlesFromGroupBy()
        SaveSettings()
        PopulateGrid()
    End Sub

    Private Sub SetColumnTitlesFromGroupBy()
        If cboGroupBy.Text = "Each Event" Then
            pColumnTitles = pColumnTitlesEvent.Clone
        Else
            pColumnTitles = pColumnTitlesAll.Clone
        End If
    End Sub

    Private Sub mnuChooseColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChooseColumns.Click
        Dim lFrmChoose As New frmChooseColumns

        If cboGroupBy.Text = "Each Event" Then
            pColumnTitles = lFrmChoose.AskUser(pColumnTitlesEventDefault, pColumnTitles)
        Else
            pColumnTitles = lFrmChoose.AskUser(pColumnTitlesAllDefault, pColumnTitles)
        End If
        PopulateGrid()
    End Sub

    Private Function ColumnUnits(ByVal aColumnName As String) As String
        Select Case aColumnName
            Case "Events" : Return ""
            Case "Group" : Return ""
            Case "Start Date" : Return ""
            Case "Start Time" : Return ""
            Case "Measurements" : Return ""
            Case "Maximum Volume", "Cummulative Volume", "Mean Volume", "Total Volume" : Return "in"
            Case "Maximum Duration", "Mean Duration", "Total Duration", "SD Duration" : Return cboGapUnits.Text
            Case "Maximum Intensity", "Mean Intensity" : Return "in/hr" 'TODO: or cboGapUnits.Text
            Case "Mean Time Since Last" : Return cboGapUnits.Text
            Case Else : Return ""
        End Select
    End Function

    Private Sub mnuGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraph.Click
        Dim lFirstRowOfData As Integer = agdMain.Source.FixedRows
        Dim lLastRowOfData As Integer = agdMain.Source.Rows - 1
        Dim lGroupNames(lLastRowOfData - lFirstRowOfData) As String
        Dim lRow As Integer

        For lRow = lFirstRowOfData To lLastRowOfData
            lGroupNames(lRow - lFirstRowOfData) = agdMain.Source.CellValue(lRow, 0)
        Next

        For lColumn As Integer = 1 To agdMain.Source.Columns - 1
            If IsNumeric(agdMain.Source.CellValue(lFirstRowOfData, lColumn)) Then
                Dim lForm As New frmGraphBar
                Dim lPane As ZedGraph.GraphPane = lForm.Pane

                'Set the titles and axis labels
                lForm.Text = pColumnTitles(lColumn) & " grouped by " & cboGroupBy.Text
                lPane.Title.Text = lForm.Text
                lPane.XAxis.Title.Text = cboGroupBy.Text
                lPane.YAxis.Title.Text = pColumnTitles(lColumn)
                If ColumnUnits(pColumnTitles(lColumn)).Length > 0 Then
                    lPane.YAxis.Title.Text &= " (" & ColumnUnits(pColumnTitles(lColumn)) & ")"
                End If

                Dim lY(lLastRowOfData - lFirstRowOfData) As Double

                For lRow = lFirstRowOfData To lLastRowOfData
                    lY(lRow - lFirstRowOfData) = agdMain.Source.CellValue(lRow, lColumn)
                Next

                ' Add a bar to the graph
                lPane.AddBar("", Nothing, lY, GetMatchingColor(pColumnTitles(lColumn)))
                '        myCurve.Bar.Border.IsVisible = False


                'Draw the X tics between the labels instead of at the labels
                lPane.XAxis.MajorTic.IsBetweenLabels = True

                lPane.XAxis.Scale.TextLabels = lGroupNames
                lPane.XAxis.Type = ZedGraph.AxisType.Text

                ' Fill the axis background with a color gradient
                'lPane.Chart.Fill = New ZedGraph.Fill(Drawing.Color.White, Drawing.Color.SteelBlue, 45.0F)

                ' disable the legend
                lPane.Legend.IsVisible = False

                lForm.zgc.AxisChange()
                lForm.Show()
            End If
        Next
    End Sub

    Private Sub txtParameter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtThreshold.KeyPress, txtGap.KeyPress
        If e.KeyChar.Equals(Chr(13)) Then
            RecomputeEventsIfFormParametersChanged()
        End If
    End Sub

    Private Sub EventParametersChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles cboAboveBelow.SelectedIndexChanged, _
                txtThreshold.LostFocus, _
                txtGap.LostFocus, _
                cboGapUnits.SelectedIndexChanged
        RecomputeEventsIfFormParametersChanged()
    End Sub

    Private Sub RecomputeEventsIfFormParametersChanged()
        If pInitialized Then
            If ComputeEventsFromFormParameters(True) Then
                PopulateGrid()
            End If
        End If
    End Sub

    Private Function GroupTotal(ByVal aDataGroup As atcDataGroup) As Double
        Dim lTotal As Double = 0
        For Each lTimeseries As atcTimeseries In aDataGroup
            lTotal += lTimeseries.Attributes.GetValue("Sum")
        Next
        Return lTotal
    End Function

End Class