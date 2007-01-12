Imports atcControls
Imports atcData
Imports atcUtility

Public Class frmSynoptic

    Private pDataManager As atcDataManager

    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcDataGroup

    Private WithEvents pEvents As atcDataGroup

    'Translator class between pDataGroup and agdMain
    Private pSource As atcGridSource
    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Private pGapUnitNames() As String = {"Seconds", "Minutes", "Hours", "Days", "Weeks", "Months", "Years"}
    Private pGapUnitFactor() As String = {JulianSecond, JulianMinute, JulianHour, 1, 7, 31, 366}

    Private pGroupByNames() As String = {"Event", "Month", "Season", "Year", "Volume", "Maximum", "Mean", "Length", "One Group"}

    Private pColumnTitles() As String = {"Group", "Maximum Volume", "Mean Volume", "Total Volume", "Maximum Duration", "Mean Duration", "Total Duration", "Maximum Intensity", "Mean Intensity", "Total Intensity"}

    Private pVolumeGroupEdges() As Double = {10, 5, 2, 1, 0.5, 0.2, 0.1, 0}


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
            Me.Show()
            PopulateGrid()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

        cboGapUnits.Items.AddRange(pGapUnitNames)
        cboGroupBy.Items.AddRange(pGroupByNames)

        cboGapUnits.SelectedIndex = GetSetting("Synoptic", "Defaults", "GapUnits", 3)
        cboGroupBy.SelectedIndex = GetSetting("Synoptic", "Defaults", "GroupBy", 0)
        txtThreshold.Text = GetSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
        radioAbove.Checked = GetSetting("Synoptic", "Defaults", "High", radioAbove.Checked)
        txtGap.Text = GetSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)
        cboGapUnits.SelectedIndex = GetSetting("Synoptic", "Defaults", "GapUnits", 3)

    End Sub

    Private Sub PopulateGrid()
        Dim lGroups As New atcCollection
        Dim lDataset As atcTimeseries
        Dim lValue As Double
        Dim lGroupIndex As Integer = 0

        If pEvents Is Nothing Then ComputeEventsFromFormParameters()

        Select Case cboGroupBy.Text
            Case "Event"
                For Each lEvent As atcTimeseries In pEvents
                    lGroups.Add(lGroupIndex, New atcDataGroup(lEvent))
                    lGroupIndex += 1
                Next
            Case "Month"
            Case "Season"
            Case "Year"
            Case "Volume"
                Dim lIndex As Integer
                For Each lValue In pVolumeGroupEdges
                    lGroups.Add(Format(lValue, "0.00"), New atcDataGroup)
                Next
                For Each lEvent As atcTimeseries In pEvents
                    lValue = lEvent.Attributes.GetValue("Sum")
                    For lIndex = 0 To pVolumeGroupEdges.GetUpperBound(0)
                        If lValue > pVolumeGroupEdges(lIndex) Then
                            lGroups.ItemByIndex(lIndex).Add(lEvent)
                        End If
                    Next
                Next
            Case "Maximum"
            Case "Mean"
            Case "Length"
            Case "One Group"
                lGroups.Add("All", pEvents)
        End Select

        pSource = New atcGridSource()
        pSource.Columns = pColumnTitles.Length
        pSource.Rows = lGroups.Count + 1
        pSource.FixedRows = 1
        For lColumn As Integer = 1 To pColumnTitles.Length
            pSource.CellValue(0, lColumn) = pColumnTitles(lColumn - 1)
        Next

        lGroupIndex = 0
        For Each lGroup As atcDataGroup In lGroups
            For lColumn As Integer = 1 To pColumnTitles.Length
                lValue = 0
                Select Case pColumnTitles(lColumn - 1)
                    Case "Group" : lValue = lGroups.Keys(lGroupIndex)
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
                    Case "Total Volume"
                        For Each lDataset In lGroup
                            lValue += lDataset.Attributes.GetValue("Sum")
                        Next
                    Case "Maximum Duration"
                    Case "Mean Duration"
                    Case "Total Duration"
                    Case "Maximum Intensity"
                        For Each lDataset In lGroup
                            If lDataset.Attributes.GetValue("Max") > lValue Then
                                lValue = lDataset.Attributes.GetValue("Max")
                            End If
                        Next
                    Case "Mean Intensity"
                    Case "Total Intensity"
                End Select
                pSource.CellValue(lGroupIndex + 1, lColumn) = DoubleToString(lValue, , , , , 3)
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
        pDataManager.ShowDisplay(sender.Text, pEvents)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If Me.Visible Then PopulateGrid()
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If Me.Visible Then PopulateGrid()
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
            .Title = "Save Grid As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
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

    Public Overrides Function ToString() As String
        Return Me.Text & vbCrLf & agdMain.ToString
    End Function

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\List.html")
    End Sub

    Private Sub btnComputeEvents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComputeEvents.Click
        ComputeEventsFromFormParameters()
    End Sub

    Private Sub ComputeEventsFromFormParameters()
        txtThreshold.Text = txtThreshold.Text.Trim
        If Not IsNumeric(txtThreshold.Text) Then txtThreshold.Text = "0"

        txtGap.Text = txtGap.Text.Trim
        If Not IsNumeric(txtGap.Text) Then txtGap.Text = "0"

        Dim lThreshold As Double = txtThreshold.Text
        Dim lDaysGapAllowed As Double = Double.Parse(txtGap.Text) * pGapUnitFactor(cboGapUnits.SelectedIndex) + JulianSecond / 2
        Dim lHighEvents As Boolean = radioAbove.Checked

        pEvents = atcSynopticAnalysisPlugin.ComputeEvents(pDataGroup, lThreshold, lDaysGapAllowed, lHighEvents)

        PopulateGrid()

        SaveSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
        SaveSetting("Synoptic", "Defaults", "High", lHighEvents)
        SaveSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)
        SaveSetting("Synoptic", "Defaults", "GapUnits", cboGapUnits.SelectedIndex)
    End Sub


    Private Sub cboGroupBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroupBy.SelectedIndexChanged
        PopulateGrid()
    End Sub
End Class